$(document).ready(function () {
    debugger;
    $("#ddlRequiredArea").select2();
    BindDDlList();
    ReplicateWith();
    $('select[id="Itemname_1"]').bind('change', function (e) {
        debugger;
        if ($("#ddlSourceType").val() != "O") {
            BindItemList_mrs(e);
            BindAvlbStock(e);
        }
    });
    //if ($("#MrsItmDetailsTbl >tbody >tr").length > 0) {
    //    $("#MrsItmDetailsTbl >tbody >tr").each(function () {
    //        var currentRow = $(this);
    //        //var clickedrow = $(e.target).closest("tr");
    //        var Itm_ID;
    //        var SNo = currentRow.find("#SNohf").val();
    //        var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    //        var src_type = $('#ddlSourceType').val();
    //        if (src_type == "O") {
    //            Itm_ID = currentRow.find("#hfItemID").val();
    //            var pre_Item_id = currentRow.find("#hfItemID").val();

    //            //Itm_ID = currentRow.find("#Itemname_" + SNo).val();
    //            //var pre_Item_id = currentRow.find("#hfItemID").val();
    //        }
    //        else{
    //            Itm_ID = currentRow.find("#Itemname_" + SNo).val();
    //            var pre_Item_id = currentRow.find("#hfItemID").val();
    //        }

    //        try {
    //            $.ajax(
    //                {
    //                    type: "POST",
    //                    url: "/ApplicationLayer/MaterialRequisitionSlip/GetAvlbStockForItem",
    //                    data: {
    //                        Itm_ID: Itm_ID
    //                    },
    //                    success: function (data) {
    //                        debugger;
    //                        if (data == 'ErrorPage') {
    //                            LSO_ErrorPage();
    //                            return false;
    //                        }
    //                        if (data !== null && data !== "") {
    //                            var arr = [];
    //                            arr = JSON.parse(data);
    //                            if (arr.Table.length > 0) {
    //                                currentRow.find("#ItemAvlbStock").val(arr.Table[0].item_avl_stk_bs);
    //                                //clickedrow.find("#UOMID").val(arr.Table[0].uom_id);
    //                            }
    //                            else {

    //                                currentRow.find("#ItemAvlbStock").val("");

    //                            }
    //                        }
    //                    },
    //                });
    //        } catch (err) {
    //        }
    //    });
    //}
    BindDLLItemList();
    //hfItemID
    $('#MrsItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
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
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        //ShowItemListItm(ItemCode);
        SerialNoAfterDelete();
        // Decreasing total number of rows by 1. 
        //rowIdx--;
        if ($("#MrsItmDetailsTbl >tbody >tr").length == 0) {
            debugger;
            var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
            var src_type = $('#ddlSourceType').val();

            if (src_type == "O") {
                // $("#ddlSourceType").attr('disabled', false);
                $("#src_doc_number").attr('disabled', false);
                // $("#ddlRequisitionTypeList").attr('disabled', false);
                $("#ddlRequiredArea").attr('disabled', false);
                $("#plusbtnhd").css('display', 'block');
            }

            else {
                // $("#src_doc_number").attr('disabled', false);
                if (RequisitionTypeList == "S" || RequisitionTypeList == "E") {
                    $("#EntityName").attr('disabled', false);
                    if (RequisitionTypeList == "E") {
                        $("#ddlReplicateWith").attr('disabled', false);
                        $("#ddlReplicateWith").val(0);//.trigger("change");
                    }
                }
                if (RequisitionTypeList == "I") {
                    if (src_type == "D") {
                        $("#ddlReplicateWith").attr('disabled', false);
                        $("#ddlReplicateWith").val(0);//.trigger("change");
                    }
                }
                //$("#ddlRequisitionTypeList").attr('disabled', false);
                $("#ddlRequiredArea").attr('disabled', false);
                $("#plusbtnhd").css('display', 'none');
            }

        }
    });

    onChangeddlRequisitionTypeList();
    onChangeddlSourceType();

    //$("#MRSListTbl #datatable-buttons tbody").bind("dblclick", function (e) {
    //    debugger;
    //    try {
    //        var clickedrow = $(e.target).closest("tr");
    //        var MRSId = clickedrow.children("#MRSNo").text();
    //        if (MRSId != null && MRSId != "") {
    //            window.location.href = "/ApplicationLayer/MaterialRequisitionSlip/EditMRS/?MRSId=" + MRSId;
    //        }
    //    }
    //    catch (err) {
    //        debugger
    //        //alert(err.message);
    //    }
    //});


    var Doc_no = $("#MRSNumber").val();
    $("#hdDoc_No").val(Doc_no);

})
function ReplicateWith() {
    debugger;
    var ReqType = $("#ddlRequisitionTypeList").val();
    var req_area = $("#ddlRequiredArea").val();
    $("#ddlReplicateWith").append("<option value='0'>---Select---</option>");
    if (ReqType != "0") {
        $("#ddlReplicateWith").select2({
            ajax: {
                url: "/ApplicationLayer/MaterialRequisitionSlip/BindReplicateWithlist",
                data: function (params) {
                    var queryParameters = {
                        Search: params.term,
                        mrs_type: ReqType,
                        req_area: req_area,
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
                    <div class="col-md-4 col-xs-12 def-cursor" id="">${$("#span_RequirementArea").text()}</div>
                    <div class="col-md-4 col-xs-12 def-cursor" hidden="hidden">${$("#span_RequirementArea").text()}</div>
                    </div>
                    </strong></li></ul>`)
                    }
                    var page = 1;
                    data = data.slice((page - 1) * pageSize, page * pageSize)
                    debugger;
                    return {
                        results: $.map(data, function (val, Item) {
                            return { id: val.ID, text: val.ID.split(",")[0], document: val.ID.split(",")[1], docReqArea: val.ID.split(",")[2], UOM: val.Name, };
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
                   /* '<div class="col-md-4 col-xs-12' + classAttr + '" hidden="hidden" id="HdnRequirementArea">' + data.docReqArea + '</div>' +*/
                    '</div>'
                );
                return $result;
                firstEmptySelect = false;
            },
        });
    } 
}
function OnChanngReplicateWith() {
    debugger;
    var ExpImpQtyDigit = $("#QtyDigit").text();
    $("#tbl_AlternateItem > tbody > tr").remove();
    $('#tbladd >tbody >tr').remove();
    var replicate = $("#ddlReplicateWith").val();
    var mrs_no = replicate.split(",")[0];
    var mrs_dt1 = replicate.split(",")[1];
    var reqArea = replicate.split(",")[2];

    $("#ddlRequiredArea").val(reqArea).trigger("change");
    var date = mrs_dt1.split("-");
    var mrs_dt = date[2] + '-' + date[1] + '-' + date[0];
    $('#MrsItmDetailsTbl >tbody >tr').remove();
    $("#ddlReplicateWith").attr("disabled", true);
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/MaterialRequisitionSlip/GetReplicateWithMRSNumber",
            data: {
                mrs_no: mrs_no,
                mrs_dt: mrs_dt
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
                                var mrs_qty = arr.Table[i].mrs_qty;
                                var it_remarks = arr.Table[i].it_remarks;
                                var sub_item = sub_item;
                                onchangereplicate_item(item_id, item_name, uom_id, uom_name, mrs_qty, it_remarks, sub_item);
                            }
                        }
                        $("#MrsItmDetailsTbl tbody tr").each(function () {
                            var currentRow = $(this);
                            OnChangeItemName("1", currentRow,"Raplicate")
                        });
                    }
                    if (arr.Table1.length > 0) {
                        for (var y = 0; y < arr.Table1.length; y++) {

                            var ItmId = arr.Table1[y].item_id;
                            var SubItmId = arr.Table1[y].sub_item_id;
                            var QtQty = arr.Table1[y].Qty;

                            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${ItmId}'></td>
                            <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                            <td><input type="text" id="subItemQty" value='${QtQty}'></td>
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
function onchangereplicate_item(item_id, item_name, uom_id, uom_name, mrs_qty, it_remarks, sub_item) {
    debugger;
    var rowIdx = 0;
    debugger;
    // var origin = window.location.origin + "/Content/Images/Calculator.png";
    var rowCount = $('#MrsItmDetailsTbl >tbody >tr').length + 1
    var RowNo = 0;
    // onMRSItemName();
    $("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        //var Sno = currentRow.find("#SNohf").val();
        //if (rowCount == Sno) {
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
        //}
    });
    if (RowNo == 0) {
        RowNo = 1;
    }
    var Disable = ""
    if (sub_item == "N") {
        Disable = "disabled style='filter: grayscale(100 %)'"
    }
    var src_type = $('#ddlSourceType').val();
    var MRSType = $("#ddlRequisitionTypeList").val();
    var reqqty_subitm = "";
    var issue_qty_subitm = "";
    if (MRSType != "S") {
        var span_SubItemDetail = $("#span_SubItemDetail").val();
        $('#MrsItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                <td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i></td>
                <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
                          <input id="SNohiddenfiled" type="hidden" value="${rowCount}" /></td>
                <td><div class="col-sm-11 lpo_form no-padding"><select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" onchange ="OnChangeItemName(${RowNo},event)"><option value="${item_id}">${item_name}</option></select> <input  type="hidden" id="hfItemID" value="" /><span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div>
                <td><input id="UOM" class="form-control" autocomplete="off" type="text" value="${uom_name}" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden"  value="${uom_id}" /></td>
                <td>
                  <div class="col-sm-8 lpo_form no-padding">
                     <input id="ItemAvlbStock" class="form-control num_right" autocomplete="off" type="text" maxlength="12" name="" placeholder="0000.00" disabled>
                 </div>
                 <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
                     <button type="button" id="SubItemAvlQty" ${Disable} class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                 </div>
                 <div class="col-sm-2 i_Icon">
                    <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#ItemStockLotBatchSrWise" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                </div>
                </td>
                <td>
                <div class="col-sm-10 lpo_form no-padding">
                <input id="RequisitionQuantity" maxlength="12"  onkeypress="return QtyFloatValueonly(this, event)" onchange ="OnclickReqQty(event)" value="${mrs_qty}" class="form-control num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"   ><span id="RequisitionQuantity_Error" class="error-message is-visible"></span>
                </div>
                <input hidden type="text" id="sub_item" value="" />
                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReqQty">
                    <button type="button" id="SubItemReqQty" class="calculator subItmImg" ${Disable} onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                </div>
                </td>
                <td>
                <div class="col-sm-10 lpo_form no-padding">
                    <input id="ItemIssueQuantity" onchange ="OnclickReqQty(event)" class="form-control num_right" autocomplete="off" onchange ="OnChangePOItemQty(,event)" onkeypress="return QtyFloatValueonly(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00" disabled></span>
                </div>
                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemIssueQty">
                    <button type="button" id="SubItemIssueQty" class="calculator subItmImg" ${Disable} onclick="return SubItemDetailsPopUp('Issue',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                </div>
                </td>
                <td><textarea  id="ItemRemarks" class="form-control remarksmessage" maxlength="100" autocomplete="off" type="text" name="ItemRemarks" value="${it_remarks}"  placeholder="${$("#span_remarks").text()}" ></textarea>
                <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
        </tr>`);
    }
    else {
        $('#MrsItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                <td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i></td>
                <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
                          <input id="SNohiddenfiled" type="hidden" value="${rowCount}" /></td>
                <td><div class="col-sm-11 lpo_form no-padding"><select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" onchange ="OnChangeItemName(${RowNo},event)"><option value="${item_id}">${item_name}</option></select></select> <input  type="hidden" id="hfItemID" value="" /><span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div>
                <td><input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" value="${uom_name}" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" value="${uom_id}" /></td>
               
                <td>
                <div class="lpo_form">
                <input id="RequisitionQuantity" maxlength="12"  onkeypress="return QtyFloatValueonly(this, event)" value="${mrs_qty}" onchange ="OnclickReqQty(event)" class="form-control num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  ><span id="RequisitionQuantity_Error" class="error-message is-visible"></span>
                </div>
                
                </td>
                <td>
                <div class="lpo_form">
                    <input id="ItemIssueQuantity" onchange ="OnclickReqQty(event)" class="form-control num_right" autocomplete="off" onchange ="OnChangePOItemQty(,event)" onkeypress="return QtyFloatValueonly(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00" disabled></span>
                </div>
                </td>
                <td><textarea  id="ItemRemarks" class="form-control remarksmessage" maxlength="100" autocomplete="off" type="text" name="ItemRemarks" value="${it_remarks}"  placeholder="${$("#span_remarks").text()}" ></textarea>
                <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
        </tr>`);
    }


    //if (src_type == "O") {

    //}
    //else {
    //    BindItmList(RowNo);
    //}
}
//function ShowItemListItm(ItemCode) {
//    debugger;
//    if (ItemCode != "0") {
//        $("#MrsItmDetailsTbl >tbody >tr").each(function () {
//            debugger;
//            var currentRow = $(this);
//            var Sno = currentRow.find("#SNohf").val();
//            $("#Itemname_"+Sno+" option[value=" + ItemCode + "]").removeClass("select2-hidden-accessible");
//        });
//    }
//}

function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });

};

function BindItemList_mrs(e, RaplicateFlag) {
    debugger;
    if (RaplicateFlag == "Raplicate") {
        var clickedrow = e;
    }
    else {
        var clickedrow = $(e.target).closest("tr");
    }
    var Itm_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Itm_ID = clickedrow.find("#Itemname_" + SNo).val();
    var pre_Item_id = clickedrow.find("#hfItemID").val();
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
    //HideItemListItm(SNo);
    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "")
    //try {
    //    $.ajax(
    //        {
    //            type: "POST",
    //            url: "/ApplicationLayer/LSODetail/GetSOItemUOM",
    //            data: {
    //                Itm_ID: Itm_ID
    //            },
    //            success: function (data) {
    //                debugger;
    //                if (data == 'ErrorPage') {
    //                    LSO_ErrorPage();
    //                    return false;
    //                }
    //                if (data !== null && data !== "") {
    //                    var arr = [];
    //                    arr = JSON.parse(data);
    //                    if (arr.Table.length > 0) {
    //                        clickedrow.find("#UOM").val(arr.Table[0].uom_alias);
    //                        clickedrow.find("#UOMID").val(arr.Table[0].uom_id);
    //                    }
    //                    else {

    //                        clickedrow.find("#UOM").val("");

    //                    }
    //                }
    //            },
    //        });
    //} catch (err) {
    //}
}
function BindDDlList() {
    debugger;
    var sr_type = $("#ddlRequisitionTypeList").val();
    $("#EntityName").select2({
        ajax: {
            url: $("#GetAutoCompleteEntity").val(),
            data: function (params) {
                var queryParameters = {
                    ddlissueto: params.term, // search term like "a" then "an"
                    sr_type: sr_type
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                if (data == 'ErrorPage') {
                    GRN_ErrorPage();
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

function OnGetReqAreaList() {
    var Value = $("#RequiredAreaName").find("option:selected").val();
    if (Value != "" || Value != null) {
        $("#ReqAreaIDErrorMsg").css("display", "none");
        $("#RequiredAreaName").css("border-color", "#ced4da");
    }

}
function OnChangeIssueto() {
    debugger;
    var IssueTo = $("#EntityName option:selected").text().trim();
    var Issue = $("#EntityName option:selected").val();
    var IssueToEntityType = IssueTo.slice(-3);
    if (Issue != '0') {
        var EntityType = IssueToEntityType.split('(')
        var EntityType1 = EntityType[1];
        var Entitype = EntityType1.replace(")", "");
        $("#hdEntitype").val(Entitype);
    }

    var ddlEntityType = $("#EntityName").val();
    if (ddlEntityType == "" || ddlEntityType == "0") {
        $("#vmEntity").text($("#valueReq").text());
        $("#vmEntity").css("display", "block");
        $('[aria-labelledby="select2-EntityName-container"]').css("border-color", "red");
        $("EntityName").css("border-color", "red");

        Flag = 'Y';
    }
    else {
        $("#vmEntity").css("display", "none");
        $('[aria-labelledby="select2-EntityName-container"]').css("border-color", "#ced4da");
        $("EntityName").css("border-color", "#ced4da");
        $("#vmEntity").text("");
    }

}
function OnChangeItem1(rowid, e) {
    debugger;
    var Value = $("#MRSItemName" + rowid).find("option:selected").val();
    if (Value == "0" || Value == "" || Value == String.empty) {

        $("#ItmUOM").val("");

    }

    else {

        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "/ApplicationLayer/MaterialRequisitionSlip/getListOfItems",
            data: {},
            dataType: "json",

            success: function (result) {
                debugger;
                for (var i = 0; i < result.length; i++) {
                    var ItemCode = result[i].Item_Id;
                    var uom = result[i].Item_UOM;
                    if (Value == ItemCode) {
                        $("#UOM_name" + rowid).val(uom);
                        $("#hdItemId" + rowid).val(ItemCode);

                    }
                }
            },
            //called on jquery ajax call failure
            error: function ajaxError(result) {
                // alert(result.status + ' : ' + result.statusText);
            }
        });
    }
}
function AddNewRow() {

    var rowIdx = 0;
    debugger;
    // var origin = window.location.origin + "/Content/Images/Calculator.png";
    var rowCount = $('#MrsItmDetailsTbl >tbody >tr').length + 1
    var RowNo = 0;
    // onMRSItemName();
    $("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        //var Sno = currentRow.find("#SNohf").val();
        //if (rowCount == Sno) {
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
        //}
    });
    if (RowNo == 0) {
        RowNo = 1;
    }
    var src_type = $('#ddlSourceType').val();
    var MRSType = $("#ddlRequisitionTypeList").val();
    var reqqty_subitm = "";
    var issue_qty_subitm = "";
    if (MRSType != "S") {
        var span_SubItemDetail = $("#span_SubItemDetail").val();
        $('#MrsItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                <td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i></td>
                <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
                          <input id="SNohiddenfiled" type="hidden" value="${rowCount}" /></td>
                <td><div class="col-sm-11 lpo_form no-padding"><select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" onchange ="OnChangeItemName(${RowNo},event)"></select> <input  type="hidden" id="hfItemID" value="" /><span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div>
                <td><input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" /></td>
                <td>
                  <div class="col-sm-8 lpo_form no-padding">
                     <input id="ItemAvlbStock" class="form-control num_right" autocomplete="off" type="text" maxlength="12" name="" placeholder="0000.00" disabled>
                 </div>
                 <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
                     <button type="button" id="SubItemAvlQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                 </div>
                 <div class="col-sm-2 i_Icon">
                    <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#ItemStockLotBatchSrWise" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                </div>
                </td>
                <td>
                <div class="col-sm-10 lpo_form no-padding">
                <input id="RequisitionQuantity" maxlength="12"  onkeypress="return QtyFloatValueonly(this, event)" onchange ="OnclickReqQty(event)" class="form-control num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  ><span id="RequisitionQuantity_Error" class="error-message is-visible"></span>
                </div>
                <input hidden type="text" id="sub_item" value="" />
                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReqQty">
                    <button type="button" id="SubItemReqQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                </div>
                </td>
                <td>
                <div class="col-sm-10 lpo_form no-padding">
                    <input id="ItemIssueQuantity" onchange ="OnclickReqQty(event)" class="form-control num_right" autocomplete="off" onchange ="OnChangePOItemQty(,event)" onkeypress="return QtyFloatValueonly(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00" disabled></span>
                </div>
                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemIssueQty">
                    <button type="button" id="SubItemIssueQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('Issue',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                </div>
                </td>
                <td><textarea  id="ItemRemarks" class="form-control remarksmessage" maxlength="100" autocomplete="off" type="text" name="ItemRemarks"  placeholder="${$("#span_remarks").text()}" ></textarea>
                <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
        </tr>`);
    }
    else {
        $('#MrsItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                <td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i></td>
                <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
                          <input id="SNohiddenfiled" type="hidden" value="${rowCount}" /></td>
                <td><div class="col-sm-11 lpo_form no-padding"><select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" onchange ="OnChangeItemName(${RowNo},event)"></select> <input  type="hidden" id="hfItemID" value="" /><span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div>
                <td><input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" /></td>
               
                <td>
                <div class="lpo_form">
                <input id="RequisitionQuantity" maxlength="12"  onkeypress="return QtyFloatValueonly(this, event)" onchange ="OnclickReqQty(event)" class="form-control num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  ><span id="RequisitionQuantity_Error" class="error-message is-visible"></span>
                </div>
                
                </td>
                <td>
                <div class="lpo_form">
                    <input id="ItemIssueQuantity" onchange ="OnclickReqQty(event)" class="form-control num_right" autocomplete="off" onchange ="OnChangePOItemQty(,event)" onkeypress="return QtyFloatValueonly(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00" disabled></span>
                </div>
                </td>
                <td><textarea  id="ItemRemarks" class="form-control remarksmessage" maxlength="100" autocomplete="off" type="text" name="ItemRemarks"  placeholder="${$("#span_remarks").text()}" ></textarea>
                <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
        </tr>`);
    }


    if (src_type == "O") {

    }
    else {
        BindItmList(RowNo);
    }
}
function GetSubItemAvlStock(e) {
    debugger;
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#SNohiddenfiled").val();
    var ProductNm = Crow.find("#Itemname_" + rowNo + " option:selected").text();
    var ProductId = Crow.find("#Itemname_" + rowNo + " option:selected").val();
    var UOM = Crow.find("#UOM").val();
    var UOMID = Crow.find("#UOMID").val();
    var AvlStk = Crow.find("#ItemAvlbStock").val();
    Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, "0", AvlStk, "br", UOMID);
}
function OnChangeItemName(RowID, e,RaplicateFlag) {
    debugger;
    if (RaplicateFlag == "Raplicate") {
        var currentrow = e;
    }
    else {
        var currentrow = $(e.target.closest('tr'));
    }
   
    if (currentrow.find("#hfItemID").val() != "0") {
        var MRSType = $("#ddlRequisitionTypeList").val();
        if ((MRSType == "I") && ($("#ddlRequiredArea").val() != "0") && ($("#ddlRequiredArea").val() != "") && ($("#ddlRequiredArea").val() != null)) {
            $("#ddlRequisitionTypeList").attr('disabled', true);
            $("#ddlRequiredArea").attr('disabled', true);
            $("#ddlSourceType").attr('disabled', true);
            $("#plusbtnhd").css('display', 'none');
        }
        if ((MRSType == "S" || MRSType == "E") && $("#EntityName").val() != "0" && $("#EntityName").val() != "" && $("#EntityName").val() != null) {
            $("#EntityName").attr('disabled', true);
            $("#ddlRequisitionTypeList").attr('disabled', true);
            $("#ddlRequiredArea").attr('disabled', true);

        }
    }
    BindItemList_mrs(e, RaplicateFlag);
    BindAvlbStock(e, RaplicateFlag);
}
function BindAvlbStock(e, RaplicateFlag) {
    debugger;
    if (RaplicateFlag == "Raplicate") {
        var clickedrow = e;
    }
    else {
        var clickedrow = $(e.target).closest("tr");
    }
    //var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Itm_ID = clickedrow.find("#Itemname_" + SNo).val();
    var pre_Item_id = clickedrow.find("#hfItemID").val();
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/MaterialRequisitionSlip/GetAvlbStockForItem",
                data: {
                    Itm_ID: Itm_ID
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
                            clickedrow.find("#ItemAvlbStock").val(arr.Table[0].item_avl_stk_bs);
                            //clickedrow.find("#UOMID").val(arr.Table[0].uom_id);
                        }
                        else {

                            clickedrow.find("#ItemAvlbStock").val("");

                        }
                    }
                },
            });
    } catch (err) {
    }
}
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var Itm_ID;
        var ItemId
        var ItemName
        var SNo = clickedrow.find("#SNohf").val()
        var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
        var src_type = $('#ddlSourceType').val();
        if (src_type == "O") {
            //ItemId = clickedrow.find("#hfItemID").val();
            // ItemName = clickedrow.find("#Itemname_" + SNo).val();
            ItemId = clickedrow.find("#Itemname_" + SNo).val();
            ItemName = clickedrow.find("#Itemname_" + SNo + " option:selected").text();
        }
        else {
            ItemId = clickedrow.find("#Itemname_" + SNo).val();
            ItemName = clickedrow.find("#Itemname_" + SNo + " option:selected").text();
        }


        var UOMName = clickedrow.find("#UOM").val();
        var UOMID = clickedrow.find("#UOMID").val();
        Cmn_GetItemStockWhLotBatchSerialWise(ItemId, ItemName, UOMName);
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
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function BindItmList(ID) {
    debugger;
    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    if (RequisitionTypeList == "S") {
        BindItemList("#Itemname_", ID, "#MrsItmDetailsTbl", "#SNohf", "BindData", "MRS_SI");

    }
    else {
        BindItemList("#Itemname_", ID, "#MrsItmDetailsTbl", "#SNohf", "BindData", "MRS");
    }
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/MaterialRequisitionSlip/getListOfItems",
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
    //                            var selected = $("#Itemname_" + ID).val();
    //                            if (check(data, selected, "#MrsItmDetailsTbl", "#SNohf", "#Itemname_") == true) {
    //                                var UOM = $(data.element).data('uom');
    //                                var classAttr = $(data.element).attr('class');
    //                                var hasClass = typeof classAttr != 'undefined';
    //                                classAttr = hasClass ? ' ' + classAttr : '';
    //                                var $result = $(
    //                                    '<div class="row">' +
    //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                                    '</div>'
    //                                );
    //                                return $result;
    //                            }
    //                            //var UOM = $(data.element).data('uom');
    //                            //var classAttr = $(data.element).attr('class');
    //                            //var hasClass = typeof classAttr != 'undefined';
    //                            //classAttr = hasClass ? ' ' + classAttr : '';
    //                            //var $result = $(
    //                            //    '<div class="row">' +
    //                            //    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                            //    '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                            //    '</div>'
    //                            //);
    //                            //return $result;
    //                            firstEmptySelect = false;
    //                        }
    //                    });
    //                    debugger;
    //                    //HideItemListItm(ID);
    //                }
    //            }
    //        },
    //    });
}
//function HideItemListItm(ID) {
//    $("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        var ItmID = currentRow.find("#hfItemID").val();
//        var rowid = currentRow.find("#SNohf").val();

//        var ItemCode;
//        ItemCode = ItmID;

//        if (ItemCode != '0' && ItemCode != "") {
//            $("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) {
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
//            $("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) {
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
function BindDLLItemList() {
    debugger;
    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    if (RequisitionTypeList == "S") {
        BindItemList("#Itemname_", "1", "#MrsItmDetailsTbl", "#SNohf", "BindData", "MRS_SI")

    }
    else {
        BindItemList("#Itemname_", "1", "#MrsItmDetailsTbl", "#SNohf", "BindData", "MRS")
    }
    //BindData();
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/MaterialRequisitionSlip/getListOfItems",
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
    //                    sessionStorage.removeItem("itemList");
    //                    sessionStorage.setItem("itemList", JSON.stringify(arr.Table));
    //                    //$('#Itemname_1').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    //for (var i = 0; i < arr.Table.length; i++) {
    //                    //    $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                    //}
    //                    //var firstEmptySelect = true;
    //                    //$('#Itemname_1').select2({
    //                    //    templateResult: function (data) {
    //                    //        var UOM = $(data.element).data('uom');
    //                    //        var classAttr = $(data.element).attr('class');
    //                    //        var hasClass = typeof classAttr != 'undefined';
    //                    //        classAttr = hasClass ? ' ' + classAttr : '';
    //                    //        var $result = $(
    //                    //            '<div class="row">' +
    //                    //            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                    //            '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                    //            '</div>'
    //                    //        );
    //                    //        return $result;
    //                    //        firstEmptySelect = false;
    //                    //    }
    //                    //});

    //                    BindData();
    //                }
    //            }
    //        },
    //    });
}
function BindData() {
    debugger;

    var PLItemListData = JSON.parse(sessionStorage.getItem("itemList"));
    if (PLItemListData != null) {
        if (PLItemListData.length > 0) {
            $("#MrsItmDetailsTbl >tbody >tr").each(function () {
                debugger;
                //var currentRow = $(this);
                //var rowid = parseFloat(currentRow.find("#SNohf").val()) + 1;
                //if (rowid == parseFloat($("#MrsItmDetailsTbl >tbody >tr").length)+1) {
                //   return false;
                //}
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();

                $("#Itemname_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#SOItemName").text()}' data-uom='${$("#SOItemUOM").text()}'></optgroup>`);
                for (var i = 0; i < PLItemListData.length; i++) {
                    $('#Textddl' + rowid).append(`<option data-uom="${PLItemListData[i].uom_name}" value="${PLItemListData[i].Item_id}">${PLItemListData[i].Item_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#Itemname_" + rowid).select2({
                    templateResult: function (data) {
                        var selected = $("#Itemname_" + rowid).val();
                        if (check(data, selected, "#MrsItmDetailsTbl", "#SNohf", "#Itemname_") == true) {
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
    //$("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) { 
    //    debugger;
    //    var currentRow = $(this);
    //    var ItmID = currentRow.find("#hfItemID").val();
    //    var rowid = currentRow.find("#SNohf").val();
    //    var ItemID = '#Itemname_' + rowid;
    //    if (ItmID != '0' && ItmID != "") {
    //        currentRow.find("#Itemname_" + rowid).val(ItmID).trigger('change.select2');
    //    }

    //});

    //$("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this);
    //    var ItmID = currentRow.find("#hfItemID").val();
    //    var rowid = currentRow.find("#SNohf").val();

    //    var ItemCode;
    //    ItemCode = ItmID;

    //    if (ItemCode != '0' && ItemCode != "") {

    //        $("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) {
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
}
//function OnChangeMRSItem(RowID, e) {
//    var Value = $("#MRSItemName").find("option:selected").val();
//    if (Value == "0" || Value == "" || Value == String.empty) {

//        $("#ItmUOM").val("");

//    }

//    else {

//        $.ajax({
//            type: "GET",
//            contentType: "application/json; charset=utf-8",
//            url: "/ApplicationLayer/MaterialRequisitionSlip/getListOfItems",
//            data: {},
//            dataType: "json",

//            success: function (result) {
//                debugger;
//                for (var i = 0; i < result.length; i++) {
//                    var ItemCode = result[i].Item_Id;
//                    var uom = result[i].Item_UOM;
//                    if (Value == ItemCode) {
//                        $("#UOM_name").val(uom);

//                    }
//                }
//            },
//            //called on jquery ajax call failure
//            error: function ajaxError(result) {
//                alert(result.status + ' : ' + result.statusText);
//            }
//        });
//    }
//    debugger;
//   // BindPOItemList(e);
//}
function BindIssueToList() {
    debugger;

    $("#ddlEntityType").select2({
        ajax: {
            url: $("#hdEntityType").val(),
            data: function (params) {
                var queryParameters = {
                    FilterPackNumber: params.term // search term like "a" then "an"

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
                arr = data;//JSON.parse(data);
                if (arr.length > 0) {

                    $("#ddlEntityType optgroup").remove();
                    $('#ddlEntityType').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                    for (var i = 0; i < arr.length; i++) {
                        $('#Textddl').append(`<option data-date="${arr[i].Type}" value="${arr[i].ID}">${arr[i].Name}</option>`);
                    }
                    var firstEmptySelect = true;
                    $('#ddlEntityType').select2({
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

            }
        },

    })
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohf").val();
    var ItmCode = "";
    var ItmName = "";
    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    var src_type = $('#ddlSourceType').val();

    if (src_type != "O") {
        ItmCode = clickedrow.find("#Itemname_" + Sno).val();
    }
    else {
        ItmCode = clickedrow.find("#hfItemID").val();
    }
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
//function updateItemSerialNumber() {
//    debugger;
//    var SerialNo = 0;
//    $("#MRSItmDetailsTbl >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        SerialNo = SerialNo + 1;
//        currentRow.find("td:eq(1)").text(SerialNo);

//    });
//};
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
function onChangeddlRequisitionTypeList() {
    debugger;
    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    if (RequisitionTypeList != "0") {
        if (RequisitionTypeList == "S") {
            $("#avlstk").hide();
            var crow = $('#MrsItmDetailsTbl');
            var itemid = crow.find("#hfItemID").val();
            var Status = $("#hdStatus_Code").val();

            if (Status == "") {
                if (itemid == null || itemid == "") {

                    $('#MrsItmDetailsTbl tbody tr').remove();

                }
            }
        }
        else {
            $("#avlstk").show();
            var crow = $('#MrsItmDetailsTbl');
            var itemid = crow.find("#hfItemID").val();
            var Status = $("#hdStatus_Code").val();
            if (Status == "") {
                if (itemid == null || itemid == "") {
                    $('#MrsItmDetailsTbl tbody tr').remove();
                }
            }
        }
        if (RequisitionTypeList == "E" || RequisitionTypeList == "S") {
            if ($('#MrsItmDetailsTbl  > tbody > tr').length >= 0) {
                var currentRow = $('#MrsItmDetailsTbl  > tbody > tr');
                var SNo = currentRow.find("#SNohiddenfiled").val();
                if ((currentRow.find("#hfItemID").val() == null) || (currentRow.find("#hfItemID").val() == "0") || (currentRow.find("#hfItemID").val() == "")) {/*added by Nitesh 19-10-2023 10:13  for case not in production order*/
                    $("#EntityName").val(0).trigger('change');
                    $("#vmEntity").css("display", "none");
                    $('[aria-labelledby="select2-EntityName-container"]').css("border-color", "#ced4da");
                    $("EntityName").css("border-color", "#ced4da");
                    $("#vmEntity").text("");
                }
            }
            $("#div_IssueTo").css('display', 'block');
            $("#SourceTypediv").css('display', 'none');
            if (RequisitionTypeList == "E") {
                ReplicateWith();
                $("#div_replicateWith").css('display', 'block');
            }
            else {
                $("#div_replicateWith").css('display', 'none');
            }
            //$("#div_replicateWith").css('display', 'none');
            bindRequrementArea(RequisitionTypeList);
            BindDDlList();
        }
        if (RequisitionTypeList == "I") {
            $("#div_IssueTo").css('display', 'none');
            $("#SourceTypediv").css('display', 'block');
            $("#div_replicateWith").css('display', 'block');
            //$("#ddlSourceType").attr('disabled', false);
            bindRequrementArea(RequisitionTypeList);
            ReplicateWith();//add by shubham Maurya on 02-12-2025
        }
        if (RequisitionTypeList == "E" || RequisitionTypeList == "S" || RequisitionTypeList == "I") {
            if ($('#MrsItmDetailsTbl  > tbody > tr').length >= 0) {
                var currentRow = $('#MrsItmDetailsTbl  > tbody > tr');
                var SNo = currentRow.find("#SNohiddenfiled").val();
                if ((currentRow.find("#hfItemID").val() == null) || (currentRow.find("#hfItemID").val() == "0") || (currentRow.find("#hfItemID").val() == "")) {/*added by Nitesh 19-10-2023 10:13  for case not in production order*/
                    currentRow.find("#Textddl1").val("0");
                    $("#ddlRequiredArea").val("0");
                    $("#txtMrsRemarks").val("");
                    $("#src_doc_number").val("0");
                }
            }
            //$("#Remarks_01").css('display', 'block');
            //$("#Remarks_02").css('display', 'none');
            $("#productiondt_no").css('display', 'none');
            $("#src_doc_date").css('display', 'none');
            $("#src_doc_number").css('display', 'none');
            $("#src_doc_number").css('display', 'none');
            $("#Output_Item").css('display', 'none');
            $("#pro_orderNumb").css('display', 'none');
            $("#productionorder_dt").css('display', 'none');
            $("#output_itm").css('display', 'none');
            $("#plusbtnhd").css('display', 'none');
            $("#addbtn").css('display', 'block');

        }
        //var Status = $("#hdStatus_Code").val();
        //if (Status == "") {
        //    AddNewRow();
        //}
        //var Status = $("#hdStatus_Code").val();
        //if ((Status == "D" || Status == "" || Status == null) && RequisitionTypeList != "0") {
        //    $.ajax({
        //        url: "/ApplicationLayer/MaterialRequisitionSlip/getReqAreaByRequisitionType",
        //        data: { flag: RequisitionTypeList },
        //        success: function (data) {
        //            if (data != null && data != "") {
        //                var arr = JSON.parse(data);
        //                var option = '<option value="0">---Select---</option>';
        //                for (var i = 0; i < arr.length; i++) {
        //                    option += '<option value="' + arr[i].setup_id + '">' + arr[i].setup_val + '</option>';
        //                }
        //                var reqArea = $("#ddlRequiredArea").val();
        //                $("#ddlRequiredArea").html(option);
        //                $("#ddlRequiredArea").val(reqArea);
        //            }
        //            debugger;
        //        },
        //    })
        //}

        document.getElementById("vmRequisitionTypeList").innerHTML = null;
        $("#ddlRequisitionTypeList").css("border-color", "#ced4da");       
    }
    else {
        /* $("#Remarks_02").css('display', 'none');*/
        $("#div_IssueTo").css('display', 'none');
        $("#src_doc_date").css('display', 'none');
        $("#src_doc_number").css('display', 'none');
        $("#Output_Item").css('display', 'none');
        $("#pro_orderNumb").css('display', 'none');
        $("#productionorder_dt").css('display', 'none');
        $("#plusbtnhd").css('display', 'none');
        $("#output_itm").css('display', 'none');
        $("#addbtn").css('display', 'none');
        $("#SourceTypediv").css('display', 'none');
        $("#div_replicateWith").css('display', 'none');
    }
    //if (RequisitionTypeList == "O") {  /*added By Nitesh 19-10-2023 10:10 when requsition type is production order */
    //    debugger;
    //    if ($('#MrsItmDetailsTbl  > tbody > tr').length > 0) {
    //        var currentRow = $('#MrsItmDetailsTbl  > tbody > tr');
    //        var SNo = currentRow.find("#SNohiddenfiled").val();
    //        if ((currentRow.find("#hfItemID").val() == null) || (currentRow.find("#hfItemID").val() == "0") || (currentRow.find("#hfItemID").val() == "")) {
    //            $('#MrsItmDetailsTbl  > tbody > tr').remove();            
    //            $("#ddlRequiredArea").val("0");
    //            $("#txtMrsRemarks").val("");

    //            $("#EntityName").val(0).trigger('change');
    //            $("#vmEntity").css("display", "none");
    //            $('[aria-labelledby="select2-EntityName-container"]').css("border-color", "#ced4da");
    //            $("EntityName").css("border-color", "#ced4da");
    //            $("#vmEntity").text("");
    //            //$("#plusbtnhd").css('display', 'block');
    //        }
    //        else {
    //            if (currentRow.find("#hfItemID").val() != null || currentRow.find("#hfItemID").val() != "" || currentRow.find("#hfItemID").val() != "0") {
    //                $("#src_doc_number").attr('disabled', true);
    //                $("#ddlRequisitionTypeList").attr('disabled', true);
    //                $("#ddlRequiredArea").attr('disabled', true);
    //                $("#plusbtnhd").css('display', 'none');

    //            $('#MrsItmDetailsTbl  > tbody > tr').each(function () {
    //                var currentRows = $(this);
    //                var SrNo = currentRows.find("#SNohiddenfiled").val();
    //                currentRows.find("#Itemname_" + SrNo).attr('disabled', true);
    //                currentRows.find("#RequisitionQuantity").attr('disabled', true);
    //            })
    //            }
    //        }

    //    }
    //    else {
    //        $("#plusbtnhd").css('display', 'none');
    //    }


    //    $("#productiondt_no").css('display', 'block');
    //    $("#div_IssueTo").css('display', 'none');
    //    $("#pro_orderNumb").css('display', 'block');
    //    $("#productionorder_dt").css('display', 'block');
    //    $("#output_itm").css('display', 'block');
    //    $("#src_doc_date").css('display', 'block');
    //    $("#src_doc_number").css('display', 'block');
    //    $("#Output_Item").css('display', 'block');
    //    //$("#Remarks_01").css('display', 'none');
    //    //$("#Remarks_02").css('display', 'block');

    //    $("#addbtn").css('display', 'none');

    //}
    //else {
    //var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    //if (RequisitionTypeList != "0")
    //{
    //    if ($('#MrsItmDetailsTbl  > tbody > tr').length == 0) {
    //        AddNewRow();
    //       //$("#Itemname_1").val("0").trigger('change');
    //    }
    //}
    // }
    if (RequisitionTypeList != "0") {
        if ($('#MrsItmDetailsTbl  > tbody > tr').length == 0) {
            AddNewRow();
            //$("#Itemname_1").val("0").trigger('change');
        }
        BindDLLItemList();
    }
}
function onChangeRequiredArea() {
    debugger;
    //var RequirementArea = $('#ddlRequiredArea').val();
    //if (RequirementArea == "" || RequirementArea == "0") {
    //    document.getElementById("vmRequiredArea").innerHTML = $("#valueReq").text();
    //    $("#ddlRequiredArea").css("border-color", "red");
    //    Flag = 'Y';
    //}
    /* else {*/
    document.getElementById("vmRequiredArea").innerHTML = null;
    $("#ddlRequiredArea").css("border-color", "#ced4da");
    $('[aria-labelledby="select2-ddlRequiredArea-container"]').css("border-color", "#ced4da");
    /* }*/

    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    var src_type = $('#ddlSourceType').val();
    if (src_type == "O") {
        BindSrcDocNumberOnBehalfSrcType();
    }
    ReplicateWith();
}

/***Added By Nitesh 19-10-2023 10:07 for OnchangeSrcDocNumber  case of Production Order***/
function OnchangeSrcDocNumber() {
    var docid = $("#DocumentMenuId").val();
    var doc_no = $("#src_doc_number").val();
    var QtyDecDigit = $("#QtyDigit").text();//Quantity
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
    if (doc_no != "" || doc_no != "0" || doc_no != "---Select---") {
        var docno = $("#src_doc_number").val();
        var docdt = $("#src_doc_date").val();
        outputitem(docno, docdt);
        $("#ddlRequisitionTypeList").attr('disabled', true);
        $("#ddlSourceType").attr('disabled', true);
        // $("#ddlRequiredArea").attr('disabled', true);
        // $("#plusbtnhd").css('display', 'none');
    }
}
/***Added By Nitesh 19-10-2023 10:07 for outputitm  case of Production Order***/
function outputitem(docno, docdt) {
    debugger;
    $.ajax({
        type: 'post',
        url: '/ApplicationLayer/MaterialRequisitionSlip/Getoutputitm',
        data: { docno: docno, docdt: docdt },
        success: function (data) {
            if (data == 'ErrorPage') {
                Error_Page();
                return false;
            }
            if (data !== null && data !== "" && data !== "{}") {
                var arr = [];
                arr = JSON.parse(data);
                debugger
                debugger;
                var output = arr.Table[0].output_item;
                $('#Output_Item').text(output);
                $('#Output_Item').val(output);
            }
        }
    })
}

/***Added By NItesh 19-10-2023 10:07 for Bind Src_doc_no case of Production Order***/
function BindSrcDocNumberOnBehalfSrcType() {
    debugger;
    var RequiredArea = $("#ddlRequiredArea  option:selected").val();
    var sr_number = $("#SR_Number").val();
    var Req_type = $('#ddlRequisitionTypeList option:selected').val();
    if (RequiredArea != null && RequiredArea != "") {
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/MaterialRequisitionSlip/GetSourceDocList',
            data: { RequiredArea: RequiredArea, Req_type: Req_type },
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
                            $('#Textddl1').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no}">${arr.Table[i].doc_no}  </option>`);
                        }

                        var firstEmptySelect = true;
                        $('#src_doc_number').select2({
                            templateResult: function (data) {
                                debugger;
                                var list = [];
                                $("#MrsItmDetailsTbl >tbody >tr").each(function (i, row) {
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
function InsertMRSDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");

    if (CheckMRSFormValidation() == false) {
        return false;
    }

    if (CheckMRSItemValidations() == false) {
        return false;
    }
    if (CheckValidations_forSubItems() == false) {
        return false;
    }
    debugger;
    // if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
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

    FinalItemDetail = InsertMRSItemDetails();

    /*-----------Sub-item-------------*/

    var SubItemsListArr = Cmn_SubItemList();
    var str2 = JSON.stringify(SubItemsListArr);
    $('#SubItemDetailsDt').val(str2);

    /*-----------Sub-item end-------------*/
    debugger;
    var ItemDt = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(ItemDt);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
    // }
    //else {
    //    //alert("Check network");
    //    return false;
    //}
};
function CheckMRSFormValidation() {

    debugger;
    var rowcount = $('#MrsItmDetailsTbl tr').length;
    var ValidationFlag = true;
    var Flag = 'N';

    var RequirementArea = $('#ddlRequiredArea').val();
    var src_type = $('#ddlSourceType').val();
    $('#hdRequiredArea').val(RequirementArea);
    $('#hdsrc_type').val(src_type);
    if (RequirementArea == "" || RequirementArea == "0") {
        document.getElementById("vmRequiredArea").innerHTML = $("#valueReq").text();
        $("#ddlRequiredArea").css("border-color", "red");

        $("#vmRequiredArea").text($("#valueReq").text());
        $("#vmRequiredArea").css("display", "block");
        $('[aria-labelledby="select2-ddlRequiredArea-container"]').css("border-color", "red");
        $("ddlRequiredArea").css("border-color", "red");
        Flag = 'Y';
    }
    else {
        document.getElementById("vmRequiredArea").innerHTML = null;
        $('[aria-labelledby="select2-ddlRequiredArea-container"]').css("border-color", "#ced4da");
        $("#ddlRequiredArea").css("border-color", "#ced4da");
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
    var src_type = $("#ddlSourceType").val();
    $("#hdMrsType").val(MRSType);
    if (MRSType == "E" || MRSType == "S") {
        var ddlEntityType = $("#EntityName").val();
        if (ddlEntityType == "" || ddlEntityType == "0") {
            $("#vmEntity").text($("#valueReq").text());
            $("#vmEntity").css("display", "block");
            $('[aria-labelledby="select2-EntityName-container"]').css("border-color", "red");
            $("EntityName").css("border-color", "red");
            Flag = 'Y';
        }
        else {
            $("#vmEntity").css("display", "none");
            $('[aria-labelledby="select2-EntityName-container"]').css("border-color", "#ced4da");
            $("EntityName").css("border-color", "#ced4da");
            $("#vmEntity").text("");
            $("#hdEntity").val(ddlEntityType);
        }
    }
    if (src_type == "O") { // added By NItesh 19-10-2023 10:06
        var srcdocno = $('#src_doc_number option:selected').val();
        if (srcdocno != "0" && srcdocno != null && srcdocno != "") {
            document.getElementById("SpanSourceDocNoErrorMsg").innerHTML = null;
            $("#src_doc_number").css("border-color", "#ced4da");
            $("#Hdn_src_doc_number").val(srcdocno);

        }
        else {
            document.getElementById("SpanSourceDocNoErrorMsg").innerHTML = $("#valueReq").text();
            $("#src_doc_number").css("border-color", "red");
            $("#SpanSourceDocNoErrorMsg").css('display', 'block');
            Flag = 'Y';
        }
    }

    if (Flag == 'Y') {
        ValidationFlag = false;
    }
    else {
        $("#ddlRequisitionTypeList").attr('disabled', true);
        $("#ddlRequiredArea").attr('disabled', true);
        ValidationFlag = true;
    }

    if (ValidationFlag == true) {
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        if (CheckMRSItemValidations() == false) {
            return false;
        }

        if (rowcount > 1) {

            //var flag = CheckMRSItemValidations();
            //if (flag == true) {

            //    var MRSItemDetailList = new Array();
            //    $("#MRSItmDetailsTbl TBODY TR").each(function () {
            //        var row = $(this);

            //        var Index = row.find("#SNohiddenfiled").val();
            //        //   var whERRID = "#wh_id" + Index + "  option:selected";
            //        var ItemNameID = "#MRSItemName" + Index;
            //        var ItemUOMID = "#UOM_name" + Index;
            //        var RequisitionQuantityID = "#RequisitionQuantity" + Index;


            //        var ItemList = {};
            //        ItemList.ItemName = row.find(ItemNameID).val();
            //        ItemList.ItemId = row.find("#hdItemId").val();
            //        ItemList.UOM = row.find(ItemUOMID).val();
            //        ItemList.UOMId = row.find('#hdItemUOMId').val();
            //        ItemList.RequisitionQuantity = row.find(RequisitionQuantityID).val();
            //        ItemList.IssueQuantity = row.find("#IssueQuantity").val();
            //        ItemList.remarks = row.find('#ItemRemarks').val();
            //        MRSItemDetailList.push(ItemList);
            //        debugger;
            //    });
            //    var str = JSON.stringify(MRSItemDetailList);
            //    $('#hdMRSItemdetails').val(str);
            //    return true;
            //}
            //else {
            //    return false;
            //}
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
function CheckMRSItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    $("#MrsItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohf").val();
        var MRSType = $("#ddlRequisitionTypeList").val();
        var src_type = $("#ddlSourceType").val();
        var ReqQty = currentRow.find("#RequisitionQuantity").val();
        var ItemAvlbStock = currentRow.find("#ItemAvlbStock").val();
        if (src_type != "O") {
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
        }
        else {

            if (currentRow.find("#hfItemID").val() == "0") {
                currentRow.find("#ItemNameError").text($("#valueReq").text());
                currentRow.find("#ItemNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-Itemname_" + Sno + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ItemNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }


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
        if (currentRow.find("#RequisitionQuantity").val() != "" && parseFloat(currentRow.find("#RequisitionQuantity").val()).toFixed(QtyDecDigit) != 0) {
            //***Modifyed By Shubham Maurya on 04-01-2023 for Not Required***//
            //if (parseFloat(ReqQty) > parseFloat(ItemAvlbStock)) {
            //    currentRow.find("#RequisitionQuantity_Error").text($("#ExceedingQty").text());
            //    currentRow.find("#RequisitionQuantity_Error").css("display", "block");
            //    currentRow.find("#RequisitionQuantity").css("border-color", "red");
            //    //  currentRow.find("#RequisitionQuantity").val("");
            //    ErrorFlag = "Y";
            //}
            //else {
            //    currentRow.find("#RequisitionQuantity_Error").css("display", "none");
            //    currentRow.find("#RequisitionQuantity").css("border-color", "#ced4da");

            //}
        }

    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
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
    var ItemAvlbStock = clickedrow.find("#ItemAvlbStock").val();
    //***Modifyed By Shubham Maurya on 04-01-2023 for Not Required ***//
    //if (parseFloat(ReqQty) > parseFloat(ItemAvlbStock)) {
    //    clickedrow.find("#RequisitionQuantity_Error").text($("#ExceedingQty").text());
    //    clickedrow.find("#RequisitionQuantity_Error").css("display", "block");
    //    clickedrow.find("#RequisitionQuantity").css("border-color", "red");
    //    clickedrow.find("#RequisitionQuantity").val("");
    //}
    //else {
    //    clickedrow.find("#RequisitionQuantity_Error").css("display", "none");
    //    clickedrow.find("#RequisitionQuantity").css("border-color", "#ced4da");

    //}
    clickedrow.find("#RequisitionQuantity").val(parseFloat(ReqQty).toFixed(QtyDecDigit));

}
function InsertMRSItemDetails() {
    debugger;
    var ItemDetailList = new Array();
    $("#MrsItmDetailsTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var ItemList = {};
        ItemList.ItemID = currentRow.find("#Itemname_" + rowid).val();
        ItemList.UOMID = currentRow.find("#UOMID").val();
        ItemList.RequQty = currentRow.find("#RequisitionQuantity").val();
        //ItemList.IssueQty = currentRow.find("#ItemIssueQuantity").val();
        ItemList.ItemRemarks = currentRow.find('#ItemRemarks').val();

        ItemDetailList.push(ItemList);
        debugger;
    });

    return ItemDetailList;
};

/*------------- For Workflow,Forward,Approve------------------*/
function ForwardBtnClick() {
    debugger;
    //var MRSStatus = "";
    //MRSStatus = $('#hdStatus_Code').val().trim();
    //if (MRSStatus === "D" || MRSStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");

    //    }
    //    //$("#radio_reject").prop("disabled", true);
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
        var MRSDt = $("#txtMrsDate").val();
        $.ajax({
            type: "POST",
            /*  url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: MRSDt
            },
            success: function (data) {
                /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var MRSStatus = "";
                    MRSStatus = $('#hdStatus_Code').val().trim();
                    if (MRSStatus === "D" || MRSStatus === "F") {

                        if ($("#hd_nextlevel").val() === "0") {
                            $("#Btn_Forward").attr("data-target", "");
                        }
                        else {
                            $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                            $("#Btn_Approve").attr("data-target", "");

                        }
                        //$("#radio_reject").prop("disabled", true);
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
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var MRSNo = "";
    var MRSDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var ReqstionType = "";
    var WF_Status1 = "";
    var dashbordData = "";
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    MRSNo = $("#MRSNumber").val();
    MRSDate = $("#txtMrsDate").val();
    $("#hdDoc_No").val(MRSNo);
    Remarks = $("#fw_remarks").val();
    WF_Status1 = $("#WF_status1").val();
    dashbordData = (docid + ',' + MRSNo + ',' + MRSDate + ',' + WF_Status1);
    ReqstionType = $("#ddlRequisitionTypeList").val();
    var ListFilterData1 = $("#ListFilterData1").val();


    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("td:eq(3)").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("td:eq(3)").text();
            level = currentRow.find("td:eq(4)").text();
        }
    });
    if(fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "MaterialRequisitionSlip_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(MRSNo, MRSDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/MaterialRequisitionSlip/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && MRSNo != "" && MRSDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(MRSNo, MRSDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/MaterialRequisitionSlip/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&dashbordData=" + dashbordData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/MaterialRequisitionSlip/MRSApprove?MRS_no=" + MRSNo + "&MRS_date=" + MRSDate + "&ReqType=" + ReqstionType + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1 + "&docid=" + docid;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && MRSNo != "" && MRSDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(MRSNo, MRSDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/MaterialRequisitionSlip/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&dashbordData=" + dashbordData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && MRSNo != "" && MRSDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(MRSNo, MRSDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/MaterialRequisitionSlip/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&dashbordData=" + dashbordData + "&Mailerror=" + mailerror;
        }
    }

}
//function GetPdfFilePathToSendonEmailAlert(grnNo, grnDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/MaterialRequisitionSlip/SavePdfDocToSendOnEmailAlert",
//        data: { grnNo: grnNo, grnDate: grnDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#MRSNumber").val();
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
function OnChangeCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertMRSDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
function CheckedFClose() {
    if ($("#ForceClosed").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertMRSDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemReqQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemIssueQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlQty");//add by shubham maurya on 15-03-2025.
}
function SubItemDetailsPopUp(flag, e) {
    var clickdRow = $(e.target).closest('tr');
    var SrcTyp = $("#ddlSourceType").val();
    var RequiredArea = $('#ddlRequiredArea option:selected').val();
    var srcdocno = $('#src_doc_number option:selected').val();
    var srcdocdt = $('#src_doc_date').val();
    var hfsno = clickdRow.find("#SNohf").val();
    var ProductNm = clickdRow.find("#Itemname_" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#Itemname_" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    //var UOMID = clickdRow.find("#UOMID").val();
    var Doc_no = $("#MRSNumber").val();
    var Doc_dt = $("#txtMrsDate").val();
    debugger;
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
        Sub_Quantity = clickdRow.find("#RequisitionQuantity").val();
    } else if (flag == "Issue") {
        Sub_Quantity = clickdRow.find("#ItemIssueQuantity").val();
    }
    var IsDisabled = "";
    if (SrcTyp == "O") {
        IsDisabled = $("#DisableSubItem").val();
        //   IsDisabled = "Y";
    }
    else {
        IsDisabled = $("#DisableSubItem").val();
    }

    var hd_Status = $("#hdStatus_Code").val();


    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialRequisitionSlip/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            SrcTyp: SrcTyp,
            srcdocno: srcdocno,
            srcdocdt: srcdocdt,
            RequiredArea: RequiredArea
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);


        }

    })

}
function CheckValidations_forSubItems() {
    return Cmn_CheckValidations_forSubItems("MrsItmDetailsTbl", "SNohf", "Itemname_", "RequisitionQuantity", "SubItemReqQty", "Y");
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("MrsItmDetailsTbl", "SNohf", "Itemname_", "RequisitionQuantity", "SubItemReqQty", "N");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/
var QtyDecDigit = $("#QtyDigit").text();

/***Add Function(Disablefunction()) By Nitesh 19-10-2023 10:00 for Production Order When Plus Btn Press Header Disable***/
function Disablefunction() {
    $("#ddlRequisitionTypeList").attr('disabled', true);
    $("#ddlRequiredArea").attr('disabled', true);
    $("#src_doc_number").attr('disabled', true);
}
/***Add Function(ValidationAddbtn()) By Nitesh 19-10-2023 10:00 for Production Order When Plus Btn prees check Validation***/
function ValidationAddbtn() {
    var ValidationFlag = true;
    var Flag = 'N';
    var RequisitionTypeList = $('#pro_orderNumb ').val();
    var RequirementArea = $('#ddlRequiredArea').val();
    $('#hdRequiredArea').val(RequirementArea);
    if (RequirementArea == "" || RequirementArea == "0") {
        document.getElementById("vmRequiredArea").innerHTML = $("#valueReq").text();
        $("#ddlRequiredArea").css("border-color", "red");

        $("#vmRequiredArea").text($("#valueReq").text());
        $("#vmRequiredArea").css("display", "block");
        $('[aria-labelledby="select2-ddlRequiredArea-container"]').css("border-color", "red");
        $("ddlRequiredArea").css("border-color", "red");
        Flag = 'Y';
    }
    else {
        document.getElementById("vmRequiredArea").innerHTML = null;
        $('[aria-labelledby="select2-ddlRequiredArea-container"]').css("border-color", "#ced4da");
        $("#ddlRequiredArea").css("border-color", "#ced4da");
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

    var srcdocno = $('#src_doc_number option:selected').val();
    if (srcdocno != "0" && srcdocno != null && srcdocno != "" && srcdocno != "---Select---") {
        document.getElementById("SpanSourceDocNoErrorMsg").innerHTML = null;
        $("#src_doc_number").css("border-color", "#ced4da");

    }
    else {
        document.getElementById("SpanSourceDocNoErrorMsg").innerHTML = $("#valueReq").text();
        $('[aria-labelledby="select2-src_doc_number-container"]').css("border-color", "red");
        $("#src_doc_number").css("border-color", "red");
        $("#SpanSourceDocNoErrorMsg").css('display', 'block');
        Flag = 'Y';
    }

    if (Flag == 'Y') {
        ValidationFlag = false;
        return false;
    }
    else {
        $("#ddlRequisitionTypeList").attr('disabled', true);
        $("#ddlRequiredArea").attr('disabled', true);
        ValidationFlag = true;
        return true;
    }
}

/***Add Function(addbtnnewdata()) By Nitesh 19-10-2023 10:00 for Production Order Item Data Bind***/
function addbtnnewdata() {
    debugger;
    if (ValidationAddbtn() == false) {
        return false;
    }
    var QtyDecDigit = $("#QtyDigit").text();
    var RequiredArea = $('#ddlRequiredArea option:selected').val();
    var srcdocno = $('#src_doc_number option:selected').val();
    var srcdocdt = $('#src_doc_date').val();
    $.ajax(
        {
            type: "Post",
            url: "/ApplicationLayer/MaterialRequisitionSlip/getMaterialIssuedata",
            data: {
                RequiredArea: RequiredArea,
                srcdocno: srcdocno,
                srcdocdt: srcdocdt,
            },
            success: function (data) {
                debugger;
                if (data != null && data != "") {
                    var arr = [];
                    var rowIdx = 0;
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        if (arr.Table.length > 0) {
                            debugger;
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                $("#plusbtnhd").css('display', 'none');
                                AddNewRow();
                            }
                        }
                        else {
                            AddNewRow();
                        }
                        var DataSno = 0;
                        var Dmenu = $("#DocumentMenuId").val();
                        $("#MrsItmDetailsTbl >tbody >tr").each(function () {
                            var currentRow = $(this);
                            debugger;
                            var SNo = currentRow.find("#SNohiddenfiled").val();
                            var itm_id = currentRow.find("#Itemname_" + SNo).val();
                            if (currentRow.find("#Itemname_" + SNo).val() == null) {
                                var subitemflag = arr.Table[DataSno].sub_item
                                if (subitemflag === "Y") {
                                    currentRow.find("#SubItemReqQty").css("filter", "grayscale(0%)");
                                    currentRow.find("#SubItemReqQty").attr("disabled", false);
                                    currentRow.find("#SubItemIssueQty").css("filter", "grayscale(0%)");
                                    currentRow.find("#SubItemIssueQty").attr("disabled", false);
                                    currentRow.find("#SubItemAvlQty").css("filter", "grayscale(0%)");
                                    currentRow.find("#SubItemAvlQty").attr("disabled", false);
                                }
                                else {
                                    currentRow.find("#SubItemReqQty").css("filter", "grayscale(100%)");
                                    currentRow.find("#SubItemReqQty").attr("disabled", true);
                                    currentRow.find("#SubItemIssueQty").css("filter", "grayscale(0%)");
                                    currentRow.find("#SubItemIssueQty").attr("disabled", true);
                                    currentRow.find("#SubItemAvlQty").css("filter", "grayscale(0%)");
                                    currentRow.find("#SubItemAvlQty").attr("disabled", true);
                                }
                                currentRow.find("#Itemname_" + SNo).append(`<option value="${arr.Table[DataSno].item_id}" selected>${arr.Table[DataSno].item_name}</option>`);//.val(arr.Table1[DataSno].item_id).trigger('change.select2');
                                var clickedrow = currentRow.find("#Itemname_" + SNo).closest("tr");
                                currentRow.find("#hfItemID").val(arr.Table[DataSno].item_id);
                                currentRow.find("#sub_item").val(arr.Table[DataSno].sub_item);
                                currentRow.find("#UOM").val(arr.Table[DataSno].uom_name);
                                currentRow.find("#UOMID").val(arr.Table[DataSno].uom_id);
                                currentRow.find("#ItemAvlbStock").val(parseFloat(arr.Table[DataSno].item_avl_stk_bs).toFixed(QtyDecDigit));
                                currentRow.find("#RequisitionQuantity").val(parseFloat(arr.Table[DataSno].req_qty).toFixed(QtyDecDigit));
                                currentRow.find("#RequisitionQuantity").attr('disabled', false);
                                currentRow.find("#Itemname_" + SNo).attr('disabled', true);
                            }

                            DataSno = DataSno + 1;
                        })
                        Disablefunction();
                    }
                    var src_type = $("#ddlSourceType").val();
                    if (src_type == "O") {
                        debugger;
                        if (arr.Table1.length > 0) {
                            var rowIdx = 0;
                            for (var y = 0; y < arr.Table1.length; y++) {
                                var ItmId = arr.Table1[y].item_id;
                                var SubItmId = arr.Table1[y].sub_item_id;
                                var SubItmName = arr.Table1[y].sub_item_name;
                                var Qty = arr.Table1[y].Qty;
                                $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemName" value='${SubItmName}'></td>
                                    <td><input type="text" id="subItemQty" value='${Qty}'></td>
                                    
                                    </tr>`);
                            }

                        }
                    }

                }
            }
        })

}

function onChangeddlSourceType() {
    debugger;
    var src_type = $("#ddlSourceType").val();
    if (src_type == "O") {
        $("#div_replicateWith").css('display', 'none');
    }
    else {
        $("#div_replicateWith").css('display', 'block');
    }
    var req_type = $("#ddlRequisitionTypeList").val();
    var RequisitionTypeList = "";

    if (src_type != "O") {

        $("#productiondt_no").css('display', 'none');
        if (req_type == "I") {
            $("#div_IssueTo").css('display', 'none');
        }
        $("#pro_orderNumb").css('display', 'none');
        $("#productionorder_dt").css('display', 'none');
        $("#output_itm").css('display', 'none');
        $("#src_doc_date").css('display', 'none');
        $("#src_doc_number").css('display', 'none');
        $("#Output_Item").css('display', 'none');
        $("#addbtn").css('display', 'block');
        $("#plusbtnhd").css('display', 'none');
        RequisitionTypeList = "I";
        // var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
        //if (src_type != "0") {
        //    if ($('#MrsItmDetailsTbl  > tbody > tr').length == 0) {
        //        AddNewRow();
        //        //$("#Itemname_1").val("0").trigger('change');
        //    }
        //}
        if ($('#MrsItmDetailsTbl  > tbody > tr').length >= 0) {
            var currentRow = $('#MrsItmDetailsTbl  > tbody > tr');
            var SNo = currentRow.find("#SNohiddenfiled").val();

            if ((currentRow.find("#hfItemID").val() == null) || (currentRow.find("#hfItemID").val() == "0") || (currentRow.find("#hfItemID").val() == "")) {/*added by Nitesh 19-10-2023 10:13  for case not in production order*/
                currentRow.find("#Textddl1").val("0");
                $("#Itemname_1").val("0");
                $("#ddlRequiredArea").val("0");
                $("#txtMrsRemarks").val("");
                $("#src_doc_number").val("0");
            }
        }
    }
    else {
        $("#productiondt_no").css('display', 'block');
        $("#div_IssueTo").css('display', 'none');
        $("#pro_orderNumb").css('display', 'block');
        $("#productionorder_dt").css('display', 'block');
        $("#output_itm").css('display', 'block');
        $("#src_doc_date").css('display', 'block');
        $("#src_doc_number").css('display', 'block');
        $("#Output_Item").css('display', 'block');
        $("#addbtn").css('display', 'none');
        RequisitionTypeList = "O";
        if ($('#MrsItmDetailsTbl  > tbody > tr').length == 0) {
            $("#ddlRequiredArea").val("0");
            // $("#src_doc_number").val("0");
            $("#txtMrsRemarks").val("");

        }
        if ($('#MrsItmDetailsTbl  > tbody > tr').length > 0) {
            var currentRow = $('#MrsItmDetailsTbl  > tbody > tr');
            var SNo = currentRow.find("#SNohiddenfiled").val();
            if ((currentRow.find("#hfItemID").val() == null) || (currentRow.find("#hfItemID").val() == "0") || (currentRow.find("#hfItemID").val() == "")) {
                $('#MrsItmDetailsTbl  > tbody > tr').remove();
                $("#ddlRequiredArea").val("0");
                $("#txtMrsRemarks").val("");
                $("#src_doc_date").val("");
                //  $("#src_doc_number").val("0").trigger('change');
                $("#ddlSourceType").attr('disabled', false)
                document.getElementById("SpanSourceDocNoErrorMsg").innerHTML = null;
                $('[aria-labelledby="select2-src_doc_number-container"]').css("border-color", "#ced4da");
                $("#src_doc_number").css("border-color", "#ced4da");

                $("#Output_Item").val("");
                $("#EntityName").val(0).trigger('change');
                $("#vmEntity").css("display", "none");
                $('[aria-labelledby="select2-EntityName-container"]').css("border-color", "#ced4da");
                $("EntityName").css("border-color", "#ced4da");
                $("#vmEntity").text("");
                $("#plusbtnhd").css('display', 'block');
            }
            else {
                if (currentRow.find("#hfItemID").val() != null || currentRow.find("#hfItemID").val() != "" || currentRow.find("#hfItemID").val() != "0") {
                    $("#src_doc_number").attr('disabled', true);
                    $("#ddlRequisitionTypeList").attr('disabled', true);
                    $("#ddlRequiredArea").attr('disabled', true);
                    $("#ddlSourceType").attr('disabled', true);
                    $("#plusbtnhd").css('display', 'none');

                    $('#MrsItmDetailsTbl  > tbody > tr').each(function () {
                        var currentRows = $(this);
                        var SrNo = currentRows.find("#SNohiddenfiled").val();
                        currentRows.find("#Itemname_" + SrNo).attr('disabled', true);
                        var Command = $("#hdnCommand").val();
                        var Documentstatus = $("#hdStatus_Code").val();
                        if (Command == "Edit" && Documentstatus == "D") {
                            currentRows.find("#RequisitionQuantity").attr('disabled', false);
                        }
                        else {
                            currentRows.find("#RequisitionQuantity").attr('disabled', true);
                        }

                    })
                }
            }

        }
        else {
            $("#plusbtnhd").css('display', 'none');
        }
    }
    bindRequrementArea(RequisitionTypeList);



}
function bindRequrementArea(RequisitionTypeList) {
    var Status = $("#hdStatus_Code").val();
    if ((Status == "D" || Status == "" || Status == null) && RequisitionTypeList != "0") {
        $.ajax({
            url: "/ApplicationLayer/MaterialRequisitionSlip/getReqAreaByRequisitionType",
            data: { flag: RequisitionTypeList },
            success: function (data) {
                if (data != null && data != "") {
                    var arr = JSON.parse(data);
                    var option = '<option value="0">---Select---</option>';
                    for (var i = 0; i < arr.length; i++) {
                        option += '<option value="' + arr[i].setup_id + '">' + arr[i].setup_val + '</option>';
                    }
                    var reqArea = $("#ddlRequiredArea").val();
                    $("#ddlRequiredArea").html(option);
                    $("#ddlRequiredArea").val(reqArea);
                }
                debugger;
            },
        })
    }
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
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "MrsItmDetailsTbl", [{ "FieldId": "Itemname_", "FieldType": "select", "SrNo": "SNohf" }]);
}



