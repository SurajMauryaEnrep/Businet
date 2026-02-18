$(document).ready(function () {
    debugger;
    DisabledField();
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
        deleteIConSrcNumber(Itemid);
        updateItemSerialNumber();
        var status = $("#hdn_Status").val();
        if (status == "") {
            DisablesFieldAdditem();
        }
    });
  
    var src_type = $("#ddlSourceType").val();
    if (src_type == "D") {
        BindDLLItemList();
    }
    else {
        BindSrcDocNumberOnBehalfSrcType();
    }
    var EntityType = $("#ddl_EntityType").val();
    if (EntityType == "E") {
        $("#Div_Address").css("display", "none");
       
    }
    else {
        $("#Div_Address").css("display", "block");
       
    }
})
function deleteIConSrcNumber(Itemid) {
    $("#SaveSrcDocDeatil TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#GPassItemID").val();
        var Docno1 = row.find("#GPassDocNo").val();
        if (rowitem == Itemid) {
            $(this).closest('tr').remove();
        }

    });
}
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
function BindDLLItemList() {
    debugger;

    BindItemList("#Itemname_", "1", "#GatepassItmDetailsTbl", "#SNohf", "", "GatePassInWard");

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

function OnChangeSourceType_List() {
    debugger;
    var ddlSourceType = $("#ddlSourceType").val();  
    $("#hd_SourceTypeID").val(ddlSourceType);
    if (ddlSourceType == "0" || ddlSourceType == "" || ddlSourceType == null) {
        $("#ddlSourceType").css("border-color", "Red");
        $('#vmSource_Type').text($("#valueReq").text());
        $("#vmSource_Type").css("display", "block");           
    }
    else {
        $("#vmSource_Type").css("display", "none");
        $("#ddlSourceType").css("border-color", "#ced4da");
      
    }
    if (ddlSourceType == "A") {
        $("#GatepassItmDetailsTbl tbody tr").remove();
        $("#DivItemAddItem").css("display", "none");
        $("#res_issueQty").attr("hidden", false);
        $("#res_pendingQty").attr("hidden", false);
    }
    else {
        if (ddlSourceType == "0" || ddlSourceType == "" || ddlSourceType == null) {
            $("#DivItemAddItem").css("display", "none");
            $("#GatepassItmDetailsTbl tbody tr").remove();
        }
        else {
           // $("#DivItemAddItem").css("display", "block");
            $("#res_issueQty").attr("hidden", true);
            $("#res_pendingQty").attr("hidden", true);
        }
    }
    $("#ddl_EntityType").val(0).trigger('change');
    $("#vmEntityType").css("display", "none");
    $("#ddl_EntityType").css("border-color", "#ced4da");
    $("#vmEntityName").css("display", "none");
    $("#ddl_EntityName").css("border-color", "#ced4da");
    DisabledField();
   
}
function DisabledField()
{
    var ddlSourceType = $("#ddlSourceType").val();
    if (ddlSourceType == "A") {
        $("#DivSrcDocNo").css("display", "block");       
        $("#DivDocumentDate").css("display", "block");
    }
    else {
        $("#DivSrcDocNo").css("display", "none");
        $("#DivDocumentDate").css("display", "none");     
    }
}
//function OnChangeEntityType_List() {
//    debugger;
//    var EntityType = $("#ddl_EntityType").val();

//    $("#ddl_EntityName").val(0).trigger('change');


//    if (EntityType == "0" || EntityType == "" || EntityType == null) {
//        $("#ddl_EntityType").css("border-color", "Red");
//        $('#vmEntityType').text($("#valueReq").text());
//        $("#vmEntityType").css("display", "block");
//        // ErrorFlag = "Y";
//    }
//    else {
//        $("#vmEntityType").css("display", "none");
//        $("#ddl_EntityType").css("border-color", "#ced4da");
//        $("#vmEntityName").css("display", "none");
//        $("#ddl_EntityName").css("border-color", "#ced4da");
//        $("#hd_EntityTypeID").val(EntityType);
//        BindSR_SuppCustList_List(EntityType);

//    }
//}
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
        $("#Div_Address").css("display","none");
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
    var ddlSourceType = $("#ddlSourceType").val();
   

    if (Entity_Name == "0" || Entity_Name == "" || Entity_Name == null) {

        if (Entity_Type == "0" || Entity_Type == "" || Entity_Type == null) {

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
        if (ddlSourceType == "A") {

            BindSrcDocNumberOnBehalfSrcType();
        }
        else {
            $("#DivItemAddItem").css("display", "block");
        }
    }

    if ((Entity_Type == "C" || Entity_Type == "S") && Entity_Name != "0")
    {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/GatePassInward/GetCustandSuppAddrDetail",
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

function BindSrcDocNumberOnBehalfSrcType() {
    debugger;
    var SuppID = $("#ddl_EntityName option:selected").val();
  
    var entity_type = $('#ddl_EntityType option:selected').val();
    if (SuppID != null && SuppID != "" && entity_type != null && entity_type != "") {
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/GatePassInward/GetSourceDocList',
            data: { SuppID: SuppID,entity_type: entity_type },
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
                                $("#SaveSrcDocDeatil >tbody >tr").each(function (i, row) {
                                    debugger;
                                    var currentRow = $(this);
                                    var Suppid = "";
                                   // var rowin = currentRow.find('#SpanRowId').text();
                                    src_docno = currentRow.find('#GPassDocNo').val();
                                    list.push(src_docno);
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
function OnchangeSrcDocNumber() {
    var docid = $("#DocumentMenuId").val();
    var doc_no = $("#src_doc_number").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    //$("#src_doc_number").css("border-color", "#ced4da");
    //$("#SpanSourceDocNoErrorMsg").css("display", "none");
    debugger
   // $("#plusbtnhd").css("display", "block");
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
       // ErrorFlag = "Y";
        $("#DivAddHeaderPlusIcon").css("display", "none");
    }
    else {
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
        $("#src_doc_number").css("border-color", "#ced4da");
        $("#SpanSourceDocNoErrorMsg").css("display", "none");
        $("#DivAddHeaderPlusIcon").css("display", "block");
    }
}
function AddNewRow() {
    var QtyDecDigit = $("#QtyDigit").text();
    var rowIdx = 0;
    debugger;
    var rowCount =  $('#GatepassItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
 //   <td>
 //       <div class="lpo_form">
 //           <input id="IssuedQuantity" class="form-control num_right" autocomplete="off" name="Quantity" readonly="readonly" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" onpaste="return CopyPasteData(event);" type="text" placeholder="0000.00" onblur="this.placeholder='0000.00'">
 //    </div>
 //</td>

    if (RowNo == "0") {
        RowNo = 1;
    }
    var SourceType = $("#ddlSourceType").val();
    if (SourceType == "D") {
        $("#GatepassItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
        });
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
  <input id="ReceivedQuantity" class="form-control num_right" autocomplete="off" name="Quantity" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" onpaste="return CopyPasteData(event);" type="text" onchange="OnChangeReceivedQuantity(event);" placeholder="0000.00" onblur="this.placeholder='0000.00'">
 <span id="ReceivedQtyError" class="error-message is-visible"></span>
     </div>
 </td>
  <td>
<textarea id="ItemRemarks" class="form-control remarksmessage" maxlength = "300", name="remarks"  placeholder="${$("#span_remarks").text()}"    ></textarea>
  </td>
 <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);
        BindItmList(RowNo);
        DisablesFieldAdditem();
    }
    else {
     var entity_type=   $("#ddl_EntityType").val();
        var entity_Name = $("#ddl_EntityName").val();
        var Doc_no = $("#src_doc_number").val();
        var Doc_dt = $("#src_doc_date").val();
        var hdn_Status = $("#hdn_Status").val();
       
            var GatePassNumber = $("#GatePassNumber").val();
            var ddlGatePassDate = $("#ddlGatePassDate").val();
        

        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/GatePassInward/BindItemTable",
                data: {
                    entity_type: entity_type,
                    entity_Name: entity_Name,
                    Doc_no: Doc_no,
                    Doc_dt: Doc_dt,
                    hdn_Status: hdn_Status, GatePassNumber: GatePassNumber, ddlGatePassDate: ddlGatePassDate,
                   
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        var rowIdx = 0;
                        var Issue_qty = parseFloat(0).toFixed(QtyDecDigit);
                        var Pending_qty = parseFloat(0).toFixed(QtyDecDigit);
                      
                        $("#DivAddHeaderPlusIcon").css("display", "none");
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                                                  
                            debugger;
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var item_id = "";
                                var rowCount1 = 0;
                                var flag = "N";

                                var DocNo = arr.Table[i].gpass_no;
                                var DocDate = arr.Table[i].gpass_dt;
                                var Doc_Date = arr.Table[i].gpassdt;
                                var ItemID = arr.Table[i].item_id;
                                var UOMID = arr.Table[i].uom_id;
                                var OrderQty = arr.Table[i].Issue_qty;
                                var PendingQty = arr.Table[i].Pending_qty;
                                
                              
                                var CheckDocNo = "";
                                var flag1 = 'N';
                                var row_Count = 'N';
                                row_Count = parseInt($('#SaveItemOrderQtyDetails >tbody >tr').length);
                              //  if (row_Count > 0) {
                                $("#SaveSrcDocDeatil TBODY TR").each(function () {
                                        var row = $(this);
                                        var rowitem = row.find("#GPassItemID").val();
                                        var Docno1 = row.find("#GPassDocNo").val();
                                        if (rowitem == ItemID && DocNo == Docno1) {
                                            CheckDocNo = Docno;
                                           /* flag1 = 'Y';*/
                                        }
                                       
                                    });
                                //}
                                //else {
                                if (CheckDocNo == "") {
                                    $('#SaveSrcDocDeatil tbody').append(`<tr>
                    <td><input type="text" id="GPassItemID" value="${ItemID}" /></td>
                    <td><input type="text" id="GPassUomID" value="${UOMID}" /></td>
                    <td><input type="text" id="GPassDocNo" value="${DocNo}" /></td>
                    <td><input type="text" id="GPassDocdt" value="${DocDate}" /></td>
                    <td><input type="text" id="hdnGPassDocNo" value="${Doc_Date}" /></td>
                    <td><input type="text" id="GPassIssueQty" value="${parseFloat(OrderQty).toFixed(QtyDecDigit)}" /></td>
                    <td><input type="text" id="GPassPendingQty" value="${parseFloat(PendingQty).toFixed(QtyDecDigit)}" /></td>
                    <td><input type="text" id="GPassRec_qty" value="${parseFloat(0).toFixed(QtyDecDigit)}" /></td>
                </tr>`)
                                }
                                

                                $("#GatepassItmDetailsTbl >tbody >tr").each(function (i, row) {
                                    debugger;
                                    var currentRow = $(this);
                                    RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
                                });

                                rowCount = parseInt($('#GatepassItmDetailsTbl >tbody >tr').length) + 1;
                                rowCount1 = parseInt($('#GatepassItmDetailsTbl >tbody >tr').length);
                                if (rowCount1 > 0) {
                                    $("#GatepassItmDetailsTbl tbody tr #hfItemID[value='" + arr.Table[i].item_id + "']").closest('tr').each(function () {
                                        debugger;
                                        flag = "Y";
                                        var Currentrow = $(this);
                                        var itemid = arr.Table[i].item_id;
                                        rowCount = parseInt($('#GatepassItmDetailsTbl >tbody >tr').length) + 1;
                                       // RowNo = parseInt(Currentrow.find("#SNohf").val()) + 1;
                                        item_id = Currentrow.find("#hfItemID").val();
                                        Issue_qty = Currentrow.find("#IssuedQuantity").val();
                                        Pending_qty = Currentrow.find("#PendingQty").val();
                                      
                                        Issue_qty = parseFloat(Issue_qty) + parseFloat(arr.Table[i].Issue_qty);
                                        Pending_qty = parseFloat(Pending_qty) + parseFloat(arr.Table[i].Pending_qty);
                                        Currentrow.find("#IssuedQuantity").val(Issue_qty.toFixed(QtyDecDigit));                    
                                        Currentrow.find("#PendingQty").val(Pending_qty.toFixed(QtyDecDigit));
                                    })
                                    if (flag=="N") {
                                        $('#GatepassItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                                 <td class=" red center">  <i class="deleteIcon fa fa-trash" aria-hidden="true" title=Delete></i></td>
                      <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
                                  <td>
                                  <div class="col-sm-11 lpo_form" style="padding:0px;" id="multiWrapper">
                                   <input  id="Itemname_${RowNo}" value='${arr.Table[i].item_name}' class="form-control" autocomplete="off" type="text" name="Itemname" placeholder='${$("#ItemName").text()}'
                                  onblur="this.placeholder='${$("#ItemName").text()}'" disabled>
                                  <input  type="hidden" id="hfItemID" value='${arr.Table[i].item_id}' />                             
                                  <span id="ItemNameError" class="error-message is-visible"></span>
                                  </div>
                                  <div class="col-sm-1 i_Icon">
                                  <button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                  </div>
                                  </td>
                                    <td>
                                    <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" value='${arr.Table[i].uom_alias}' readonly="readonly">
                                    <input id="UOMID" type="hidden" value='${arr.Table[i].uom_id}' />
                                    </td>
                                    <td>
     <div class="col-sm-10 no-padding">
     <input id="IssuedQuantity" value='${arr.Table[i].Issue_qty}' class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" onpaste="return CopyPasteData(event);" placeholder="0000.00" readonly="readonly">
      </div>
      <div class="col-sm-2 i_Icon">
       <button type="button" id="IssuedInfoBtnIcon" class="calculator ItmInfoBtnIcon " onclick="OnClickItemOrderQtyIconBtn(event,'issued_qty');" data-toggle="modal" data-target="#GatePassInwardDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_issuedQuantity").text()}">  </button>
        </div>
    </td>
 <td>
 <div class="col-sm-10 no-padding">
   <input id="PendingQty" class="form-control num_right" value='${arr.Table[i].Pending_qty}' autocomplete="off" name="Pending" readonly="readonly" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" onpaste="return CopyPasteData(event);" type="text" placeholder="0000.00" onblur="this.placeholder='0000.00'">
</div>
                                                                            <div class="col-sm-2 i_Icon">
                                                                                <button type="button" id="PendingInfoBtnIcon" class="calculator ItmInfoBtnIcon " onclick="OnClickItemOrderQtyIconBtn(event,'PendingQty');" data-toggle="modal" data-target="#GatePassInwardDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_pendingquantity").text()}">  </button>
                                                                            </div>
</td>
<td>
   <div class="col-sm-10 lpo_form no-padding">
  <input id="ReceivedQuantity" class="form-control num_right" readonly="readonly" autocomplete="off" value='${""}' name="Quantity" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" onpaste="return CopyPasteData(event);" type="text" onchange="OnChangeReceivedQuantity(event);" placeholder="0000.00" onblur="this.placeholder='0000.00'">
 <span id="ReceivedQtyError" class="error-message is-visible"></span>
     </div>
 </div>
                                                                            <div class="col-sm-2 i_Icon lpo_form">
                                                                                <button type="button" id="recQtyInfoBtnIcon" class="calculator ItmInfoBtnIcon " onclick="OnClickItemOrderQtyIconBtn(event,'recqty');" data-toggle="modal" data-target="#GatePassInwardDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_ReceivedQuantity").text()}">  </button>
<span id="spanrecQtyInfoBtnIcon" class="error-message is-visible"></span>
</div>
 </td>
                                         <td>
                                       <textarea id="ItemRemarks" class="form-control remarksmessage" maxlength = "300", name="remarks"  placeholder="${$("#span_remarks").text()}"    ></textarea>
                                         </td>
                                        <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
                                       </tr>`);
                                    }
                                }
                                else {
                                    $('#GatepassItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center">  <i class="deleteIcon fa fa-trash" aria-hidden="true" title=Delete></i></td>
<td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td>
<div class="col-sm-11 lpo_form" style="padding:0px;" id="multiWrapper">
 <input  id="Itemname_${rowIdx}" value='${arr.Table[i].item_name}' class="form-control" autocomplete="off" type="text" name="Itemname" placeholder='${$("#ItemName").text()}'
onblur="this.placeholder='${$("#ItemName").text()}'" disabled>
<input  type="hidden" id="hfItemID" value='${arr.Table[i].item_id}' />
<span id="ItemNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
</div>
</td>
<td>
<input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" value='${arr.Table[i].uom_alias}' readonly="readonly">
<input id="UOMID" type="hidden" value='${arr.Table[i].uom_id}' />
</td>
 
 <td>
     <div class="col-sm-10 no-padding">
     <input id="IssuedQuantity" value='${arr.Table[i].Issue_qty}' class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" onpaste="return CopyPasteData(event);" placeholder="0000.00" readonly="readonly">
      </div>
      <div class="col-sm-2 i_Icon">
       <button type="button" id="IssuedInfoBtnIcon" class="calculator ItmInfoBtnIcon " onclick="OnClickItemOrderQtyIconBtn(event,'issued_qty');" data-toggle="modal" data-target="#GatePassInwardDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_issuedQuantity").text()}">  </button>
        </div>
    </td>
 <td>
 <div class="col-sm-10 no-padding">
   <input id="PendingQty" class="form-control num_right" value='${arr.Table[i].Pending_qty}' autocomplete="off" name="Pending" readonly="readonly" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" onpaste="return CopyPasteData(event);" type="text" placeholder="0000.00" onblur="this.placeholder='0000.00'">
</div>
                                                                            <div class="col-sm-2 i_Icon">
                                                                                <button type="button" id="PendingInfoBtnIcon" class="calculator ItmInfoBtnIcon " onclick="OnClickItemOrderQtyIconBtn(event,'PendingQty');" data-toggle="modal" data-target="#GatePassInwardDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_pendingquantity").text()}">  </button>
                                                                            </div>
</td>
<td>
   <div class="col-sm-10 lpo_form no-padding">
  <input id="ReceivedQuantity" class="form-control num_right" value='${""}' readonly="readonly" autocomplete="off" name="Quantity" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" onpaste="return CopyPasteData(event);" type="text" onchange="OnChangeReceivedQuantity(event);" placeholder="0000.00" onblur="this.placeholder='0000.00'">
 <span id="ReceivedQtyError" class="error-message is-visible"></span>
     </div>
 </div>
                                                                            <div class="col-sm-2 i_Icon lpo_form">
                                                                                <button type="button" id="recQtyInfoBtnIcon" class="calculator ItmInfoBtnIcon " onclick="OnClickItemOrderQtyIconBtn(event,'recqty');" data-toggle="modal" data-target="#GatePassInwardDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt=""  title="${$("#span_ReceivedQuantity").text()}">  </button>
<span id="spanrecQtyInfoBtnIcon" class="error-message is-visible"></span>
</div>
 </td>
  <td>
<textarea id="ItemRemarks" class="form-control remarksmessage" maxlength = "300", name="remarks"  placeholder="${$("#span_remarks").text()}"    ></textarea>
  </td>
 <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);
                                }



                                



                                 
                              //  }
                                
                               

                             

                            }
                            
                            DisablesFieldAdditem();
                            $("#src_doc_number").val('---Select---').trigger('change');

                            $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
                            $("#src_doc_number").css("border-color", "#ced4da");
                            $("#SpanSourceDocNoErrorMsg").css("display", "none");

                        }
                    }
                }
            });

       
    }

   
}

function BindItmList(ID) {
    debugger;
    BindItemList("#Itemname_", ID, "#GatepassItmDetailsTbl", "#SNohf", "", "GatePassInWard");
}
function OnChangeItemName(RowID, e) {
    debugger;
    BindItemList1(e);
}
function BindItemList1(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Itm_ID = clickedrow.find("#Itemname_" + SNo).val();
    var pre_Item_id = clickedrow.find("#hfItemID").val();
    clickedrow.find("#ReceivedQuantity").val("");
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
function OnChangeReceivedQuantity(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var Quantity = currentrow.find("#ReceivedQuantity").val();

    if (Quantity == "0" || Quantity == "" || Quantity == null) {
        currentrow.find("#ReceivedQuantity").css("border-color", "Red");
        currentrow.find('#ReceivedQtyError').text($("#valueReq").text());
        currentrow.find("#ReceivedQtyError").css("display", "block");
        currentrow.find("#ReceivedQuantity").val("");
        // ErrorFlag = "Y";
    }
    else {
        currentrow.find("#ReceivedQtyError").css("display", "none");
        currentrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
    }
}

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function AddAttribute() {
    debugger;
    var Header = HeaderValidation("AddItem");
    if (Header==true) {
        AddNewRow();
    }
    
}
function DisablesFieldAdditem() {
    debugger;
    var SourceType= $("#ddlSourceType").val();
    var rowCount = $('#GatepassItmDetailsTbl >tbody >tr').length
    if (rowCount == 0) {
       
       $("#ddlSourceType").prop("disabled", false);
        $("#ddl_EntityType").prop("disabled", false);
        $("#ddl_EntityName").prop("disabled", false);
        $("#src_doc_number").prop("disabled", false);
        if (SourceType == "A") {
            $("DivAddHeaderPlusIcon").css("display", "block");
        }
        else {

            $("DivItemAddItem").css("display", "block");
        }
        //BindSR_SuppCustList_List(EntityType);
    }
    else {
        $("#ddlSourceType").prop("disabled", true);
        $("#ddl_EntityType").prop("disabled", true);
        $("#ddl_EntityName").prop("disabled", true);
       //  $("#src_doc_number").prop("disabled", true);
       
       
    }
}
function CheckFormValidation() {
    debugger;
    var ddlSourceType = $("#ddlSourceType").val();
    var Headervalid = HeaderValidation('SaveDeatil');
    if (Headervalid == false)
        return false;

    var rowCount = $('#GatepassItmDetailsTbl >tbody >tr').length
    if (rowCount == 0) {
        swal("", $("#noitemselectedmsg").text(), "warning");        
        return false;       
    }
    var ItemValidation = ItemDeatilValidation();
    if (ItemValidation == false)
        return false;
    //var checkvalidIconrecqty = CheckValidationIConRecQty();
    //if (checkvalidIconrecqty == false)
    //    return false;

    var ItemList = new Array;
    var ItemList1 = new Array;

    $("#GatepassItmDetailsTbl tbody tr").each(function () {
        var Currentrow = $(this);
        var arr = {};
        arr.item_id = Currentrow.find("#hfItemID").val();
        arr.Sno = Currentrow.find("#SNohf").val();
        arr.Srno = Currentrow.find("#SpanRowId").text();
        arr.UOMID = Currentrow.find("#UOMID").val();
        if (ddlSourceType == "A") {
            arr.IssuedQuantity = Currentrow.find("#IssuedQuantity").val();
        }
        else {
            arr.IssuedQuantity = 0;
        }
        
        arr.ReceivedQuantity = Currentrow.find("#ReceivedQuantity").val();
        arr.ItemRemarks = Currentrow.find("#ItemRemarks").val();
        ItemList.push(arr);
    })
    $("#hdnItemDeatilDataSave").val(JSON.stringify(ItemList));
    var srcrowcount = $('#SaveSrcDocDeatil tr').length;
  
        $("#SaveSrcDocDeatil tbody tr").each(function () {
            var Currentrow = $(this);

            var arr1 = {};
            var recqty = Currentrow.find("#GPassRec_qty").val();
           // if (recqty != null && recqty != "" && parseFloat(recqty) != parseFloat(0) && parseFloat != "NaN") {
                arr1.item_id = Currentrow.find("#GPassItemID").val();
                // arr.Sno = Currentrow.find("#GPassUomID").val();
                arr1.UOMID = Currentrow.find("#GPassUomID").val();
                arr1.Document_no = Currentrow.find("#GPassDocNo").val();
                arr1.Document_dt = Currentrow.find("#GPassDocdt").val();
                   arr1.Documentdt = Currentrow.find("#hdnGPassDocNo").val();
                arr1.IssuedQuantity = Currentrow.find("#GPassIssueQty").val();
                arr1.ReceivedQuantity = Currentrow.find("#GPassRec_qty").val();
                ItemList1.push(arr1);
           // }
        })
   
    $("#hdnSaveSrcDocDeatil").val(JSON.stringify(ItemList1));
    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/
}
function ItemDeatilValidation() {
    var ErrorFlag = "N";


    $("#GatepassItmDetailsTbl tbody tr").each(function () {
        var Currentrow = $(this);

        var item_id = Currentrow.find("#hfItemID").val();
        var Sno = Currentrow.find("#SNohf").val();
        var Quantity = Currentrow.find("#ReceivedQuantity").val();
        var IssueQuantity = Currentrow.find("#IssuedQuantity").val();
        var SourceType = $("#ddlSourceType").val();
        if (Quantity == "0" || Quantity == "" || Quantity == null) {
            Currentrow.find("#ReceivedQuantity").css("border-color", "Red");
            Currentrow.find('#ReceivedQtyError').text($("#valueReq").text());
            Currentrow.find("#ReceivedQtyError").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            if (SourceType=="A") {
                if (parseFloat(Quantity) > parseFloat(IssueQuantity)) {
                    Currentrow.find("#ReceivedQuantity").css("border-color", "Red");
                    Currentrow.find('#ReceivedQtyError').text($("#ExceedingQty").text());
                    Currentrow.find("#ReceivedQtyError").css("display", "block");
                    ErrorFlag = "Y";
                }
                else {
                    Currentrow.find("#ReceivedQtyError").css("display", "none");
                    Currentrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
                }
            }
            else {
                Currentrow.find("#ReceivedQtyError").css("display", "none");
                Currentrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
            }
         
           
        }
        if (item_id == "0" || item_id == "" || item_id == null) {
            Currentrow.find("[aria-labelledby='select2-Itemname_" + Sno + "-container']").css("border-color", "Red");
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
function CheckValidationIConRecQty()
{
    var ErrorFlag = "N";
    $("#GatepassItmDetailsTbl tbody tr").each(function () {
        var Currentrow = $(this);
        var item_id = Currentrow.find("#hfItemID").val();

        $("#SaveSrcDocDeatil tbody tr").each(function () {
            var curr = $(this);
            var rec_qty = curr.find("#GPassRec_qty").val();
            var src_docno = curr.find("#GPassDocNo").val();
            var IconItem_id = curr.find("#GPassItemID").val();

            var PendingQty = curr.find("#GPassPendingQty").val();
            if (IconItem_id == item_id) {
                if ((src_docno != null || src_docno != "") && (parseFloat(rec_qty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit) || rec_qty == null || rec_qty == "")) {
                    Currentrow.find("#spanrecQtyInfoBtnIcon").text($("#valueReq").text());
                    Currentrow.find("#spanrecQtyInfoBtnIcon").css("display", "block");
                    Currentrow.find("#recQtyInfoBtnIcon").css("border", "1px solid red");
                    ErrorFlag = "Y";
                }
                else {
                    Currentrow.find("#spanrecQtyInfoBtnIcon").css("display", "none");
                    Currentrow.find("#recQtyInfoBtnIcon").css("border-color", "#ced4da");
                }
            }
            else {
                Currentrow.find("#spanrecQtyInfoBtnIcon").css("display", "none");
                Currentrow.find("#recQtyInfoBtnIcon").css("border-color", "#ced4da");
            }
            if (ErrorFlag == "Y") {
                return false;
            }
            else {
                return true;
            }
        })
        if (ErrorFlag == "Y") {
            return false;
        }
        else {
            return true;
        }
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function HeaderValidation(flag) {
    debugger;
    var ErrorFlag = "N";
    var SourceType = $("#ddlSourceType").val();
    var EntityType = $("#ddl_EntityType").val();
    var EntityName = $("#ddl_EntityName").val();
    var ReceivedBy = $("#ReceivedBy").val();
    var src_doc_number = $("#src_doc_number").val();
    if (SourceType == "" || SourceType == null || SourceType == "0") {
        $("#ddlSourceType").css("border-color", "Red");
        $('#vmSource_Type').text($("#valueReq").text());
        $("#vmSource_Type").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmSource_Type").css("display", "none");
        $("#ddlSourceType").css("border-color", "#ced4da");
    }

    if (EntityType == "0" || EntityType == "" || EntityType == null) {
        $("#ddl_EntityType").css("border-color", "Red");
        $('#vmEntityType').text($("#valueReq").text());
        $("#vmEntityType").css("display", "block");
         ErrorFlag = "Y";
    }
    else {
        $("#vmEntityType").css("display", "none");
        $("#ddl_EntityType").css("border-color", "#ced4da");
        $("#hd_EntityTypeID").val(EntityType);
        

    }
    if (EntityName == "0" || EntityName == "" || EntityName == null) {

        if (EntityType == "0" || EntityType == "" || EntityType == null) {
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
    if (ReceivedBy == "0" || ReceivedBy == "" || ReceivedBy == null) {
        $("#ReceivedBy").css("border-color", "Red");
        $('#vmReceivedBy').text($("#valueReq").text());
        $("#vmReceivedBy").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmReceivedBy").css("display", "none");
        $("#ReceivedBy").css("border-color", "#ced4da");
    }
    if (SourceType == "A" && flag == "AddItem") {
        if (src_doc_number == "" || src_doc_number == "0" || src_doc_number == "---Select---") {
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
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function onchageReceivedBy() {
    debugger;
    var ReceivedBy = $("#ReceivedBy").val();
    if (ReceivedBy == "0" || ReceivedBy == "" || ReceivedBy == null) {
        $("#ReceivedBy").css("border-color", "Red");
        $('#vmReceivedBy').text($("#valueReq").text());
        $("#vmReceivedBy").css("display", "block");
    }
    else {
        $("#vmReceivedBy").css("display", "none");
        $("#ReceivedBy").css("border-color", "#ced4da");
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
    var RecQty = currentrow.find("#ReceivedQuantity").val();
    if (RecQty != "") {
        currentrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        currentrow.find("#ReceivedQtyError").text("");
        currentrow.find("#ReceivedQtyError").css("display", "none");
    }
}

function OnClickItemOrderQtyIconBtn(e,flag) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    BindOrderQtyDetails();
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hfItemID").val();
    var SNohf = clickedrow.find("#SNohf").val();
   
    var ItmName = clickedrow.find("#Itemname_" + SNohf).val();
    var UOM = clickedrow.find("#UOM").val();
    var UOMid = clickedrow.find("#UOMID").val();
    var OrderQty = clickedrow.find("#IssuedQuantity").val();
    var PendingQty = clickedrow.find("#PendingQty").val();
    var SelectedItemdetail = $("#hdnSaveSrcDocDeatil").val();
    var docid = $("#DocumentMenuId").val();
    var Command = $("#Qty_pari_Command").val();
    var TransType = $("#hdn_TransType").val();
   
    debugger;

    var PL_Status = $("#hdn_Status").val().trim();
    if (ItmCode != "" || ItmCode != null) {
        $.ajax({
            type: "Post",
            url: "/ApplicationLayer/GatePassInward/getItemOrderQuantityDetail",
            data: {
                ItemID: ItmCode,Status: PL_Status, SelectedItemdetail: SelectedItemdetail
                , TransType: TransType,
                Command: Command,
                docid: docid, flag, flag
            },
            success: function (data) {
                debugger;
                $('#ItemIssuedQuantityDetail').html(data);
                $("#OrderQtyItemName").val(ItmName);
                $("#hd_OQtyItemId").val(ItmCode);
                $("#OrderQtyUOM").val(UOM);
                $("#hd_OQtyUOMId").val(UOMid);
                $("#ItemIssuedQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit));
                $("#ItemPendingQuantity").val(parseFloat(PendingQty).toFixed(QtyDecDigit));
                TotalQtyInGPassDetail();
            },
        });
    }
}
function BindOrderQtyDetails() {
    debugger;
    var FDocNoWiseItemQtyDetails = [];
    var DocNoWiseItemQtyDetails = JSON.parse(sessionStorage.getItem("ItemQtyDocNoWiseDetails"));
    $("#SaveSrcDocDeatil tbody tr").each(function () {
        debugger;
        var row = $(this);
        var DocNo = row.find("#GPassDocNo").val();
        var DocDate = row.find("#GPassDocdt").val();
        var Doc_Date = row.find("#hdnGPassDocNo").val();
        var ItemID = row.find("#GPassItemID").val();
        var UomID = row.find("#GPassUomID").val();
        var Issue_qty = row.find("#GPassIssueQty").val();
        var PendingQty = row.find("#GPassPendingQty").val();
        var rec_qty = row.find("#GPassRec_qty").val();
        if (parseFloat(rec_qty) == 0) {
            rec_qty = "";
        }
        if (DocNoWiseItemQtyDetails != null) {
            if (DocNoWiseItemQtyDetails.length > 0) {
                for (j = 0; j < DocNoWiseItemQtyDetails.length; j++) {
                    var SDocNo = DocNoWiseItemQtyDetails[j].DocNo;
                    var SItemID = DocNoWiseItemQtyDetails[j].ItemID;

                    if (SDocNo != DocNo && SItemID != SItemID) {
                        FDocNoWiseItemQtyDetails.push({ DocNo: DocNo, DocDate: DocDate, Doc_Date: Doc_Date, ItemID: ItemID, UomID: UomID, Issue_qty: Issue_qty, PendingQty: PendingQty, rec_qty: rec_qty })
                    }
                }
            }
            else {
                FDocNoWiseItemQtyDetails.push({ DocNo: DocNo, DocDate: DocDate, Doc_Date: Doc_Date, ItemID: ItemID, UomID: UomID, Issue_qty: Issue_qty, PendingQty: PendingQty, rec_qty: rec_qty })
            }
        }
        else {
            FDocNoWiseItemQtyDetails.push({ DocNo: DocNo, DocDate: DocDate, Doc_Date: Doc_Date, ItemID: ItemID, UomID: UomID, Issue_qty: Issue_qty, PendingQty: PendingQty, rec_qty: rec_qty })
        }
    });

    if (FDocNoWiseItemQtyDetails != null) {
        if (FDocNoWiseItemQtyDetails.length > 0) {
            //sessionStorage.setItem("ItemQtyDocNoWiseDetails", JSON.stringify(FDocNoWiseItemQtyDetails));
            var str1 = JSON.stringify(FDocNoWiseItemQtyDetails);
            $("#hdnSaveSrcDocDeatil").val(str1);

        }
    }
}
function TotalQtyInGPassDetail() {
   
        QtyDecDigit = $("#QtyDigit").text();
    var TotalRec_qty = parseFloat(0).toFixed(QtyDecDigit);
    $("#OrderQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var recQty = CurrentRow.find("#Order_RecQty").val();
        if (recQty != null && recQty != "") {
            TotalRec_qty = parseFloat(TotalRec_qty) + parseFloat(recQty);
        }
    });
    $("#LblTotalOrderQty").text(parseFloat(TotalRec_qty).toFixed(QtyDecDigit));
}
function onclickbtnOrderQtyReset() {
    debugger;
    $("#OrderQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        debugger;
        CurrentRow.find("#Order_RecQty").val("");

    });

    $("#LblTotalOrderQty").text("");
}
function OnKeyPressOrderRecQty(el, evt) {
    var clickedrow = $(evt.target).closest("tr");
    var DocumentMenuId = $("#DocumentMenuId").val();  
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    clickedrow.find("#Order_RecQty_Error").css("display", "none");
    clickedrow.find("#Order_RecQty ").css("border-color", "#ced4da");
    return true;
}
function OnChangeOrderRecQty(evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(evt.target).closest("tr");
    var RecQty = clickedrow.find("#Order_RecQty").val();

    if ((RecQty == "") || (RecQty == null)) {
        RecQty = "";
        clickedrow.find("#Order_RecQty").val(RecQty);
    }
    var PendingQty = clickedrow.find("#Order_PendingQty").val();
        var QtyDigit = $("#QtyDigit").text();;
    if (parseFloat(RecQty) > parseFloat(PendingQty)) {
        debugger;
        clickedrow.find("#Order_RecQty_Error").text($("#ExceedingQty").text());
        clickedrow.find("#Order_RecQty_Error").css("display", "block");
        clickedrow.find("#Order_RecQty").css("border-color", "red");
    }
    else {
        clickedrow.find("#Order_RecQty_Error").css("display", "none");
        clickedrow.find("#Order_RecQty").css("border-color", "#ced4da");
        if (parseFloat(RecQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {         
            clickedrow.find("#Order_RecQty").val("");
        }
        else {
            var test = parseFloat(RecQty).toFixed(QtyDigit);
            clickedrow.find("#Order_RecQty").val(test);
        }
       
    }

   
    TotalQtyInGPassDetail();
}
function onclickbtnOrderQtySaveAndExit(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    if (CheckOrderRecQtyValidation() == true) {
        TotalQtyInGPassDetail();
        var clickedrow = $(e.target).closest("tr");              
        var ItemPendingQty = $("#ItemPendingQuantity").val();
        var ProductID = $("#hd_OQtyItemId").val();
        var ItemTotalRecQuantity = $("#LblTotalOrderQty").text();
        var flagSrcMatch = "N";
        if ((parseFloat(ItemTotalRecQuantity) > parseFloat(ItemPendingQty))) {
            swal("", $("#PackedQtycannotexceedtoPendingQty").text(), "warning");
            flagSrcMatch = "Y";
            return false;
        }
        else {
            debugger;
            var SelectedItem = $("#hd_OQtyItemId").val();
            var ItemUomId = $("#hd_OQtyUOMId").val();
            $("#SaveSrcDocDeatil TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#GPassItemID").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });
            $("#OrderQtyInfoTbl tbody tr").each(function () {
                var CurrentRow = $(this);
                var Rec_qty = CurrentRow.find("#Order_RecQty").val();
                if (Rec_qty != null && Rec_qty != "") {
                    var OQtyDocNo = CurrentRow.find("#OrderQtyDocNo").val();
                    var OQtyDocDate = CurrentRow.find("#OrderQtyDocDate").val();
                    var hdn_OQtyDocDate = CurrentRow.find("#hdnOrderQtyDocDate").val();
                    var OQtyOrderQty = CurrentRow.find("#OrderedQuantity").val();
                    var PendingQty = CurrentRow.find("#Order_PendingQty").val();
                    $('#SaveSrcDocDeatil tbody').append(`<tr>
                    <td><input type="text" id="GPassItemID" value="${SelectedItem}" /></td>
                    <td><input type="text" id="GPassUomID" value="${ItemUomId}" /></td>
                    <td><input type="text" id="GPassDocNo" value="${OQtyDocNo}" /></td>                   
                    <td><input type="text" id="GPassDocdt" value="${OQtyDocDate}" /></td>
                    <td><input type="text" id="hdnGPassDocNo" value="${hdn_OQtyDocDate}" /></td>
                    <td><input type="text" id="GPassIssueQty" value="${OQtyOrderQty}" /></td>
                    <td><input type="text" id="GPassPendingQty" value="${PendingQty}" /></td>
                    <td><input type="text" id="GPassRec_qty" value="${Rec_qty}" /></td>
                </tr>`
                    );
                }
            })
          
            $("#GatepassItmDetailsTbl >tbody >tr").each(function () {
                debugger;
                var clickedrow = $(this);
                var ItemId = clickedrow.find("#hfItemID").val();
                if (ItemId == SelectedItem) {
                    clickedrow.find("#ReceivedQuantity").val(parseFloat($("#LblTotalOrderQty").text()).toFixed(QtyDecDigit));
                    clickedrow.find("#ReceivedQtyError").css("display", "none");
                    clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
                }
                clickedrow.find("#spanrecQtyInfoBtnIcon").css("display", "none");
                clickedrow.find("#recQtyInfoBtnIcon").css("border-color", "#ced4da");
            });
            $("#GatePassInwardDetail").modal('hide');
        }
    }
    else {
        return false;
    }
}
function CheckOrderRecQtyValidation() {
    var QtyDecDigit = $("#QtyDigit").text();
    var status = "N";
    $("#OrderQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var rec_qty = CurrentRow.find("#Order_RecQty").val();
        var src_docno = CurrentRow.find("#OrderQtyDocNo").val();
      
        var PendingQty = CurrentRow.find("#Order_PendingQty").val();
        if (rec_qty != "" && rec_qty != null && parseFloat(rec_qty).toFixed(QtyDecDigit) != parseFloat(0).toFixed(QtyDecDigit)) {

            if (parseFloat(rec_qty) > parseFloat(PendingQty)) {
                debugger;
                CurrentRow.find("#Order_RecQty_Error").text($("#ExceedingQty").text());
                CurrentRow.find("#Order_RecQty_Error").css("display", "block");
                CurrentRow.find("#Order_RecQty").css("border-color", "red");
                status = "Y";
            }
            else {
                CurrentRow.find("#Order_RecQty_Error").css("display", "none");
                CurrentRow.find("#Order_RecQty").css("border-color", "#ced4da");
                var test = parseFloat(rec_qty).toFixed(QtyDecDigit);
                CurrentRow.find("#Order_RecQty").val(test);
            }
        }
        else {
            CurrentRow.find("#Order_RecQty_Error").text($("#valueReq").text());
            CurrentRow.find("#Order_RecQty_Error").css("display", "block");
            CurrentRow.find("#Order_RecQty").css("border-color", "red");
           
            status = "Y";
        }
      
    })
    if (status === "Y") {
        return false;
    }
    else {
        return true;
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
        debugger;
        if (isConfirm) {
            debugger;
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();
            RemoveSessionNew();
            return true;
        } else {
            return false;
        }
    });
    return false;
}

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
            window.location.href = "/ApplicationLayer/GatePassInward/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ DocNo: DocNo, DocDate: DocDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/GatePassInward/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/GatePassInward/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            await Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/GatePassInward/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
}
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "GatepassItmDetailsTbl", [{ "FieldId": "Itemname_", "FieldType": "input", "SrNo": "SNohf" }]);
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
        TransType ="Update"
    }
    if (EntityType == "C") {
        Cmn_AddrInfoBtnClick(Entity_id, EntityType, bill_add_id, status, TransType);
    }
    else {
        Cmn_SuppAddrInfoBtnClick1(Entity_id, bill_add_id, status, TransType, EntityType);
    }
}