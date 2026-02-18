$(document).ready(function () {
    debugger;
    var Doc_No = $("#GatePassNumber").val();
    $("#hdDoc_No").val(Doc_No);
    $('#GatepassItmDetailsTbl').on('click', '.deleteIcon', function () {
        debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id);
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        debugger;
        var Itemid = $(this).closest('tr').find("#hfItemID").val();

        updateItemSerialNumber()
    });
    BindDLLItemList();

    var EntityType = $("#ddl_EntityType").val();
    if (EntityType == "E") {
        $("#Div_Address").css("display", "none");

    }
    else {
        $("#Div_Address").css("display", "block");

    }
})
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#GatepassItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function OnChangeItemName(RowID, e) {
    debugger;
    BindPRItemList(e);
}
function BindDLLItemList() {
    debugger;

    BindItemList("#Itemname_", "1", "#GatepassItmDetailsTbl", "#SNohf", "BindData", "GatePassOutWard");

}
function BindPRItemList(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Itm_ID = clickedrow.find("#Itemname_" + SNo).val();
    var pre_Item_id = clickedrow.find("#hfItemID").val();
    clickedrow.find("#RequisitionQuantity").val("");
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

    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "GatePassOutWard")

}
function AddNewRow() {
    debugger;
    //if (CheckValidations() == false) {
    //    return false;
    //}

    var rowIdx = 0;
    debugger;
    var rowCount = $('#GatepassItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;

    $("#GatepassItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#GatepassItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center">  <i class="deleteIcon fa fa-trash" aria-hidden="true" title=Delete></i></td>
<td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td>
<div class="col-sm-11 lpo_form" style="padding:0px;" id="multiWrapper">
  <select class="form-control" id="Itemname_${RowNo}" name="Itemname" onchange="OnChangeItemName(this,event)">
</select>
<input  type="hidden" id="hfItemID" />
<span id="ItemNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
</div>
</td>
<td>
<input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly">
<input id="UOMID" type="hidden" />
</td>
 <td>
 <div class="lpo_form">
  <input id="Quantity" class="form-control num_right" autocomplete="off" name="Quantity" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" onpaste="return CopyPasteData(event);" type="text" onchange="OnChangeQty(event);" placeholder="0000.00" onblur="this.placeholder='0000.00'">
 <span id="ReceivedQtyError" class="error-message is-visible"></span>
     </div>
 </td>
  <td>
<textarea id="ItemRemarks" class="form-control remarksmessage" maxlength = "300", name="remarks"  placeholder="${$("#span_remarks").text()}"    ></textarea>
  </td>
 <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);
    BindItmList(RowNo);
}
function BindItmList(ID) {
    debugger;

    BindItemList("#Itemname_", ID, "#GatepassItmDetailsTbl", "#SNohf", "", "GatePassOutWard");

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
                    // source_type: sou_type
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
function CheckFormValidation() {
    debugger;
    var Headervalid = CheckHeaderValidation();
    if (Headervalid == false)
        return false;

    var rowCount = $('#GatepassItmDetailsTbl >tbody >tr').length
    if (rowCount == 0) {
        swal("", $("#noitemselectedmsg").text(), "warning");
       // ErrorFlag = "Y";
        return false;
        //  }
    }
    var ItemValidation = ItemDeatilValidation();
    if (ItemValidation == false)
        return false;
    var ItemList = new Array;

    $("#GatepassItmDetailsTbl tbody tr").each(function () {
        var Currentrow = $(this);
        var arr = {};
        arr.item_id = Currentrow.find("#hfItemID").val();
        arr.Sno = Currentrow.find("#SNohf").val();
        arr.Srno = Currentrow.find("#SpanRowId").text();
        arr.UOMID = Currentrow.find("#UOMID").val();
        arr.ReceivedQuantity = Currentrow.find("#Quantity").val();
        arr.ItemRemarks = Currentrow.find("#ItemRemarks").val();
        ItemList.push(arr);
    })
    $("#hdnItemDeatilDataSave").val(JSON.stringify(ItemList));

    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
}
function CheckHeaderValidation() {
    var ErrorFlag = "N";
    var Entity_Type = $("#ddl_EntityType").val();
    var Entity_Name = $("#ddl_EntityName option:selected").val();
    var Issueto = $("#idIssuedto").val();
    var Issueby = $("#IssuedBy").val();
    if (Entity_Type == "0" || Entity_Type == "" || Entity_Type == null) {
        $("#ddl_EntityType").css("border-color", "Red");
        $('#vmEntityType').text($("#valueReq").text());
        $("#vmEntityType").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmEntityType").css("display", "none");
        $("#ddl_EntityType").css("border-color", "#ced4da");
    }
    if (Entity_Name == "0" || Entity_Name == "" || Entity_Name == null) {

        if (Entity_Type == "0" || Entity_Type == "" || Entity_Type == null) {
            $("#ddl_EntityName").css("border-color", "Red");
        }
        else {
            $("[aria-labelledby='select2-ddl_EntityName-container']").css("border-color", "Red");
        }
        $('#vmEntityName').text($("#valueReq").text());
        $("#vmEntityName").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmEntityName").css("display", "none");
        $("#ddl_EntityName").css("border-color", "#ced4da");
    }
    if (Issueto == "0" || Issueto == "" || Issueto == null) {
        $("#idIssuedto").css("border-color", "Red");
        $('#vmIssuedto').text($("#valueReq").text());
        $("#vmIssuedto").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmIssuedto").css("display", "none");
        $("#idIssuedto").css("border-color", "#ced4da");
    }
    if (Issueby == "0" || Issueby == "" || Issueby == null) {
        $("#IssuedBy").css("border-color", "Red");
        $('#vmIssuedBy').text($("#valueReq").text());
        $("#vmIssuedBy").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmIssuedBy").css("display", "none");
        $("#IssuedBy").css("border-color", "#ced4da");
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function ItemDeatilValidation() {
    var ErrorFlag = "N";
  
  
    $("#GatepassItmDetailsTbl tbody tr").each(function () {
        var Currentrow = $(this);
       
        var item_id = Currentrow.find("#hfItemID").val();
        var Sno = Currentrow.find("#SNohf").val();
        var Quantity = Currentrow.find("#Quantity").val();
      
        //var rowCount = $('#GatepassItmDetailsTbl >tbody >tr').length
        //if (rowCount = 1) {
        //    if (Currentrow.find("#Itemname_" + Sno).val() == "" || Currentrow.find("#Itemname_" + Sno).val() == "0") {
        //        swal("", $("#noitemselectedmsg").text(), "warning");
        //        ErrorFlag = "Y";
        //        return false;
        //    }
        //}
        if (Quantity == "0" || Quantity == "" || Quantity == null) {
            Currentrow.find("#Quantity").css("border-color", "Red");
            Currentrow.find('#ReceivedQtyError').text($("#valueReq").text());
            Currentrow.find("#ReceivedQtyError").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            Currentrow.find("#ReceivedQtyError").css("display", "none");
            Currentrow.find("#Quantity").css("border-color", "#ced4da");
        }
        if (item_id == "0" || item_id == "" || item_id == null) {
            Currentrow.find("[aria-labelledby='select2-Itemname_"+ Sno +"-container']").css("border-color", "Red");        
            Currentrow.find('#ItemNameError').text($("#valueReq").text());
            Currentrow.find("#ItemNameError").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            Currentrow.find("#ItemNameError").css("display", "none");          
            Currentrow.find("[aria-labelledby='select2-Itemname_" + Sno + "-container']").css("border-color", "#ced4da");
        }

    })
   
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnChangeEntityType_List() {
    debugger;
    var EntityType = $("#ddl_EntityType").val();

    $("#ddl_EntityName").val(0).trigger('change');
  

    if (EntityType == "0" || EntityType == "" || EntityType == null) {
        $("#ddl_EntityType").css("border-color", "Red");
        $('#vmEntityType').text($("#valueReq").text());
        $("#vmEntityType").css("display", "block");
        // ErrorFlag = "Y";
    }
    else {
        $("#vmEntityType").css("display", "none");
        $("#ddl_EntityType").css("border-color", "#ced4da");
         $("#vmEntityName").css("display", "none");
        $("#ddl_EntityName").css("border-color", "#ced4da");
        $("#hd_EntityTypeID").val(EntityType);
        BindSR_SuppCustList_List(EntityType);
       
    }
    if (EntityType == "E") {
        $("#Div_Address").css("display", "none");
        $("#bill_add_id").val("0");
    }
    else {
        $("#Div_Address").css("display", "block");
        $("#bill_add_id").val("0");
    }
}

function OnChangeEntityNameList() {
    debugger;
    var Entity_Name = $("#ddl_EntityName option:selected").val();
    var Entity_Type = $("#ddl_EntityType").val();

    if (Entity_Name == "0" || Entity_Name == "" || Entity_Name == null) {

        if (Entity_Type == "0" || Entity_Type == "" || Entity_Type == null)
        {

            $("#ddl_EntityName").css("border-color", "Red");
        }
        else {
            $("[aria-labelledby='select2-ddl_EntityName-container']").css("border-color", "Red");
        }
        $('#vmEntityName').text($("#valueReq").text());
        $("#vmEntityName").css("display", "block");
       // ErrorFlag = "Y";
    }
    else {
        if (Entity_Type == "0" || Entity_Type == "" || Entity_Type == null) {

            $("#ddl_EntityName").css("border-color", "Red");
        }
        else {
            $("[aria-labelledby='select2-ddl_EntityName-container']").css("border-color", "#ced4da");
        }
        $("#vmEntityName").css("display", "none");
        //$("#ddl_EntityName").css("border-color", "#ced4da");
        $("#hd_entity_id").val(Entity_Name);
    }
    if ((Entity_Type == "C" || Entity_Type == "S") && Entity_Name != "0") {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/GatePassOutward/GetCustandSuppAddrDetail",
                    data: {
                        Entity_Name: Entity_Name,
                        Entity_Type: Entity_Type
                    },
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {

                            return false;
                        }
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                debugger;
                                $("#Address").val(arr.Table[0].BillingAddress);
                                $("#Address").text(arr.Table[0].BillingAddress);
                                $("#bill_add_id").val(arr.Table[0].bill_add_id);

                            }
                            else {
                                debugger;
                                $("#Address").val("");
                                $("#Address").text("");
                                $("#bill_add_id").val("");
                            }


                        }
                    },
                });
        } catch (err) {
            console.log("GetMenuData Error : " + err.message);
        }
    }
}
function onchngeIssuedto() {
    var Issueto = $("#idIssuedto").val();
    if (Issueto == "0" || Issueto == "" || Issueto == null) {
        $("#idIssuedto").css("border-color", "Red");
        $('#vmIssuedto').text($("#valueReq").text());
        $("#vmIssuedto").css("display", "block");
       // ErrorFlag = "Y";
    }
    else {
        $("#vmIssuedto").css("display", "none");
        $("#idIssuedto").css("border-color", "#ced4da");
    }
}
function onchngeIssuedBy() {
    var Issueby = $("#IssuedBy").val();
    if (Issueby == "0" || Issueby == "" || Issueby == null) {
        $("#IssuedBy").css("border-color", "Red");
        $('#vmIssuedBy').text($("#valueReq").text());
        $("#vmIssuedBy").css("display", "block");
    //    ErrorFlag = "Y";
    }
    else {
        $("#vmIssuedBy").css("display", "none");
        $("#IssuedBy").css("border-color", "#ced4da");
    }
}
function OnChangeQty(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var Quantity = currentrow.find("#Quantity").val();

    if (Quantity == "0" || Quantity == "" || Quantity == null) {
        Currentrow.find("#Quantity").css("border-color", "Red");
        Currentrow.find('#ReceivedQtyError').text($("#valueReq").text());
        Currentrow.find("#ReceivedQtyError").css("display", "block");
       // ErrorFlag = "Y";
    }
    else {
        Currentrow.find("#ReceivedQtyError").css("display", "none");
        Currentrow.find("#Quantity").css("border-color", "#ced4da");
    }
}
function QtyFloatValueonlyRecQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function OnKeyupRecQty(e) {

    var currentrow = $(e.target).closest('tr');
    var RecQty = currentrow.find("#Quantity").val();
    if (RecQty != "") {
        currentrow.find("#Quantity").css("border-color", "#ced4da");
        currentrow.find("#ReceivedQtyError").text("");
        currentrow.find("#ReceivedQtyError").css("display", "none");
    }
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";

    Remarks = $("#fw_remarks").val();
    DocNo = $("#GatePassNumber").val();
    DocDate = $("#ddlGatePassDate").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (DocNo + ',' + DocDate + ',' + WF_Status1);
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
    //var pdfAlertEmailFilePath = 'BOM_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    //GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath);

    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            window.location.href = "/ApplicationLayer/GatePassOutward/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ DocNo: DocNo, DocDate: DocDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/GatePassOutward/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/GatePassOutward/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            await Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/GatePassOutward/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
}
function onclickcancilflag() {

    if ($("#CancelFlag").is(":checked")) {
        $("#CancelFlag").val(true) 
        $("#btn_save").prop('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        
      
      
    }
    else {
        $("#CancelFlag").val(false);
        $("#btn_save").prop('disabled', true);     
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
       
       
    }
};
function onclickReturnflag() {

    if ($("#Returnable").is(":checked")) {
        $("#Returnable").val(true);
        //$("#btn_save").prop('disabled', false);
        //$("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
       
       
    }
    else {
        $("#Returnable").val(false);
        //$("#btn_save").prop('disabled', true);
        //$("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
     
       
    }
};

function ForwardBtnClick() {
    debugger;
    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    $.ajax({
        type: "POST",
        url: "/Common/Common/CheckFinancialYear",
        data: {
            compId: compId,
            brId: brId
        },
        success: function (data) {
            if (data == "Exist") { /*End to chk Financial year exist or not*/
                var OrderStatus = "";
                OrderStatus = $('#hdn_Status').val().trim();
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
                swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");
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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#GatePassNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
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

function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "GatepassItmDetailsTbl", [{ "FieldId": "Itemname_", "FieldType": "select", "SrNo": "SNohf" }]);
}

function OnClickbillingAddressIconBtn(e) {
    debugger;
    $('#hd_add_type').val("S");
    var status = $("#hdn_Status").val().trim();
    var EntityType = $("#hd_EntityTypeID").val();
    var Entity_id = $('#hd_entity_id').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    var TransType = "";// $('#hdn_TransType').val().trim();
    var Disabled = $('#FlagDisabled').val();
    if (Disabled == "Disabled") {
        TransType = "";
    }
    else {
        TransType = "Update" 
    }
    if (EntityType == "C") {
        Cmn_AddrInfoBtnClick(Entity_id, EntityType, bill_add_id, status, TransType);
    }
    else {
        Cmn_SuppAddrInfoBtnClick1(Entity_id, bill_add_id, status, TransType, EntityType);
    }
}