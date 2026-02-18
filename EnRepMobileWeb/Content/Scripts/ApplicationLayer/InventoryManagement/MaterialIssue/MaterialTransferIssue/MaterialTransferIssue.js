$(document).ready(function () {
    debugger;
    //var itemId = $("#hdnStockItemWiseMessage1test").val();
    //alert(itemId)
    $("#fromWh").select2();
    $('#MaterialIssueItemDetailsTbl').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        
        var Itemid = $(this).closest('tr').find("#hdItemId").val();
        $(this).closest('tr').remove();
        Cmn_DeleteSubItemQtyDetail(Itemid);
        var MaterialIssueNo = $("#txtMaterialIssueNo").val();
        if (MaterialIssueNo == null || MaterialIssueNo == "") {
            if ($('#MaterialIssueItemDetailsTbl tr').length <= 1) {
                debugger;
                $("#ddlMTR_No").prop("disabled", false);
                $("#ddlRequisitionTypeList").prop("disabled", false);
                $("#ddlRequiredArea").prop("disabled", false);
                $(".plus_icon1").css('display', 'block');
            }
        }
        updateItemSerialNumber()
    });

    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = now.getFullYear() + '-' + month + '-' + day;
    $("#txtTodate").val(today);

    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        $("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
    });

    //$("#datatable-buttons >tbody").bind("dblclick", function (e) {
    //    debugger;
    //    try {
    //        var clickedrow = $(e.target).closest("tr");
    //        var IssueType = clickedrow.children("#trfType").text();
    //        var IssueNumber = clickedrow.children("#MTINo").text();
    //        var IssueDate = clickedrow.children("#issue_date").text();
    //        if (IssueNumber != null && IssueNumber != "") {
    //            window.location.href = "/ApplicationLayer/MaterialIssue/EditMaterialIssue/?IssueType=" + IssueType + "&IssueNumber=" + IssueNumber + "&IssueDate=" + IssueDate;
    //        }
            
    //    }
    //    catch (err) {
    //        debugger
    //    }
    //});
    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#MaterialIssueItemDetailsTbl > tbody > tr').each(function () {
            var cellText = $(this).find('#hdItemId').val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
});

function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function onChangeddlMTR_No() {
    debugger;
    var MTRDate = $("#ddlMTR_No").val();
    //var MTRDate = $('#txtMTR_Date').val();
    var date = MTRDate.split("-");
    var DFinal = date[2] + "-" + date[1] + "-" + date[0];
    var MTRNo = $("#ddlMTR_No option:selected").text();
    mrs_type = $("#ddlRequisitionTypeList").val();
    $("#txtMTR_Date").val(DFinal);
    if (MTRDate != "0") {
        document.getElementById("vmMTR_No").innerHTML = null;
        $("#ddlMTR_No").css("border-color", "#ced4da");
        $(".plus_icon1").css('display', 'block');
    }
    else {
        document.getElementById("vmMTR_No").innerHTML = $("#valueReq").text();
        $("#ddlMTR_No").css("border-color", "red");
        $(".plus_icon1").css('display', 'none');
        $("#txtMTR_Date").val("");
        $("#txtEntityType").val("");
    }    
}
function BindddlMaterialList() {
    debugger;
    var FromWH = $('#fromWh').val();  
    var ToWH = $('#toWh').val(); 
    var ToBR = $('#tobranch').val(); 
    var TransferType = $('#TransferType').val();
    
    var firstEmptySelect = true;
    $("#ddlMTR_No").select2({
        ajax: {
            url: $("#hdMaterialIssueNo_Path").val(),
            data: function (params) {
                var queryParameters = {
                    FilterMTRNo: params.term, // search term like "a" then "an"
                    FilterSourceWH: FromWH,
                    FilterToWH: ToWH,
                    FilterToBR: ToBR,
                    FilterTransferType: TransferType,
                    page: params.page || 1
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
                var pageSize,
                    pageSize = 20;
                if (arr.length > 0) {
                    //$("#ddlMTR_No option").remove();//commented buy shubham maurya on 22-05-2025 for ticket number:2159
                    //$("#ddlMTR_No optgroup").remove();
                    //$('#ddlMTR_No').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                    //for (var i = 0; i < arr.length; i++) {
                    //    $('#Textddl').append(`<option data-date="${arr[i].ID}" value="${arr[i].ID}">${arr[i].Name}</option>`);
                    //}
                    //var firstEmptySelect = true;
                    //$('#ddlMTR_No').select2({
                    //    templateResult: function (data) {
                    //        var DocDate = $(data.element).data('date');
                    //        var classAttr = $(data.element).attr('class');
                    //        var hasClass = typeof classAttr != 'undefined';
                    //        classAttr = hasClass ? ' ' + classAttr : '';
                    //        var $result = $(
                    //            '<div class="row">' +
                    //            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                    //            '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                    //            '</div>'
                    //        );
                    //        return $result;
                    //        firstEmptySelect = false;
                    //    }
                    //});
                    
                }
                var page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                    //results: data.slice((page - 1) * pageSize, page * pageSize),
                    //more: data.length >= page * pageSize,
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };

            }
        },
        templateResult: function (data) {
            var DocDate = $(data.element).data('date');
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result = $(
                '<div class="row">' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.id + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        }
    })
}
function AddItemDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var span_SubItemDetail = $("#span_SubItemDetail").text();
    var ValidationFlag = true;
    var TransferType = $('#TransferType').val();
    var fromwh = $('#fromWh').val();
    var towh = $('#toWh').val();
    var Tobranch = $("#tobranch").val();
    var MTR_No = $('#ddlMTR_No').val();
    var Fr_branch = $('#Fr_branch').val();
    var PR_number = $('#ddlMTR_No option:selected').text();

    if (TransferType == "" || TransferType == "0") {
        document.getElementById("vmTransferType").innerHTML = $("#valueReq").text();
        $("#TransferType").css("border-color", "red");
        ValidationFlag = false;
    }
    if (fromwh == "" || fromwh == "0") {
        $('#spanFromWh').text($("#valueReq").text());
        $("#spanFromWh").css("display", "block");
        //document.getElementById("vmwarehouse").innerHTML = $("#valueReq").text();
        $("#fromWh").css("border-color", "red");
        ValidationFlag = false;
    }
    if (towh == "" || towh == "0") {
        document.getElementById("vmtowarehouse").innerHTML = $("#valueReq").text();
        $("#toWh").css("border-color", "red");
        ValidationFlag = false;
    }
    if (Tobranch == "" || Tobranch == "0") {
        document.getElementById("vmbranch").innerHTML = $("#valueReq").text();
        $("#tobranch").css("border-color", "red");
        ValidationFlag = false;
    }
    if (MTR_No == "" || MTR_No == "0") {
        document.getElementById("vmMTR_No").innerHTML = $("#valueReq").text();
        $("#ddlMTR_No").css("border-color", "red");
        $(".select2-container--default .select2-selection--single").css("border-color", "red");
        ValidationFlag = false;
    }

    if (ValidationFlag == true) {
        $("#HdMTRNo").val($('#ddlMTR_No option:selected').text());
        //$("#HdMTRDate").val($('#ddlMTR_No').val());
        $(".plus_icon1").css('display', 'none');
        debugger;
        $("#ddlMTR_No").prop("readonly", true);
        $("#txtMTR_Date").prop("readonly", true);
        $("#TransferType").prop("disabled", true);
        $("#fromWh").prop("disabled", true);
        $("#toWh").prop("disabled", true);
        $("#tobranch").prop("disabled", true);
        $("#ddlMTR_No").prop("disabled", true);

        var TRFType = $('#TransferType').val();
        var MTRNo = $('#ddlMTR_No option:selected').text();
        var MTRDate = $('#txtMTR_Date').val();
        var tobranch = $('#tobranch').val();

        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/MaterialTransferIssue/GetMTRDetailByNumber",
                data: {
                    TRFType: TRFType,
                    TRFNo: MTRNo,
                    TRFDate: MTRDate,
                    BrchID: tobranch
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            var Iserial = "";
                            var Ibatch = "";
                            var ItemType = "";
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var subitmDisable = "";
                                if (arr.Table[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }

                                if (arr.Table[i].i_serial == 'Y') {
                                    Iserial = `<td class="center"><button type="button" onclick="ItemStockSerialWise(this,event)" id="BtnSerialDetail" class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_serial" value="Y" style="display: none;" /></td>`;
                                }
                                else {
                                    Iserial = `<td class="center"><button type="button" onclick="ItemStockSerialWise(this,event)" id="BtnSerialDetail" disabled class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_serial" value="N" style="display: none;" /></td>`;
                                }

                                if (arr.Table[i].i_batch == 'Y') {
                                    Ibatch = `<td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" id="BtnBatchDetail" onchange="OnChangeIssueQty" class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                              <input type="hidden" id="hdi_batch" value="Y" style="display: none;" /></td>`;
                                }
                                else {
                                    Ibatch = `<td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" id="BtnBatchDetail" onchange="OnChangeIssueQty" disabled class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_batch" value="N" style="display: none;" /></td>`;
                                }

                                if (arr.Table[i].i_batch == 'N' && arr.Table[i].i_serial == 'N') {
                                    ItemType = `<td style="display:none"><input id="ItemType" type="text" value="YES" /></td>`;
                                }
                                else {
                                    ItemType = `<td style="display:none"><input id="ItemType" type="text" value="NO" /></td>`;
                                }
                                var IssueQty = ""
                                if (parseFloat(arr.Table[i].IssuedQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
                                    IssueQty = "";
                                }
                                else if ((arr.Table[i].IssuedQuantity == "") || (arr.Table[i].IssuedQuantity == null)) {
                                    IssueQty = "";
                                }
                                else {
                                    IssueQty = parseFloat(arr.Table[i].IssuedQuantity).toFixed(QtyDecDigit);
                                }
                                $('#MaterialIssueItemDetailsTbl tbody').append(`
                                   <tr id="${++rowIdx}">
                                                            <td class=" red center"><i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                                            <td class="sr_padding"><span id="SpanRowId"  value="${rowIdx}" >${rowIdx}</span> <input  type="hidden" id="SNohiddenfiled" value="${rowIdx}"</td>                                                        
                                                            <td class="ItmNameBreak itmStick tditemfrz">
                                                                <div class=" col-sm-11 no-padding">
                                                                    <input id="ItemName" value='${arr.Table[i].item_name}' class="form-control" autocomplete="off" type="text" name="OrderNumber" placeholder="${$("#ItemName").text()}" disabled>
                                                                    <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                                                </div>
                                                                <div class="col-sm-1 i_Icon">
                                                                    <button type="button" onclick="OnClickIconBtn(event);" class="calculator" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input value='${arr.Table[i].uom_name}' id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
                                                                <input type="hidden" id="hdUOMId" value="${arr.Table[i].base_uom_id}" style="display: none;" />
                                                            </td>
                                                            <td>
                                                              <div class="col-sm-10 no-padding">
                                                                <input id="RequisitionQuantity"  value="${parseFloat(arr.Table[i].Requisition_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  disabled>
                                                              </div>
                                                              <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReqQty">
                                                                  <button type="button" id="SubItemReqQty" ${subitmDisable} class="calculator subItmImg"  onclick="return SubItemDetailsPopUp('ReqQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                              </div>
                                                            </td>
                                                            <td>
                                                               <div class="col-sm-10 no-padding">
                                                                <input id="PendingQuantity" value="${parseFloat(arr.Table[i].PendingQuantity).toFixed(QtyDecDigit)}"  class="form-control num_right" autocomplete="off" type="text" name="PendingQuantity" placeholder="0000.00"  disabled>
                                                               </div>
                                                               <div class="col-sm-2 i_Icon no-padding" id="div_SubItemPendQty">
                                                                   <button type="button" id="SubItemPendQty" ${subitmDisable} class="calculator subItmImg"  onclick="return SubItemDetailsPopUp('PendingQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                               </div>
                                                            </td>                                                     
                                                           <td>
                                                                <div class=" col-sm-11 no-padding">
                                                                    <div class="lpo_form">
                                                                     <input id="WHName" value="${arr.Table[i].wh_name}" class="form-control" autocomplete="off" type="text" name="OrderNumber" placeholder="${$("#ItemName").text()}" disabled>
                                                                      <input type="hidden" id="hdwhId" value="${arr.Table[i].wh_id}"  style="display: none;" />
                                                                     </div>
                                                                 </div>
                                                                    <div class="col-sm-1 i_Icon">
                                                                        <button type="button" onclick="ItemStockWareHouseWise(this,event)" class="calculator" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                                                                    </div>
                                                                </td>
                                                            <td>
                                                            <div class=" col-sm-10 no-padding" >
                                                                <input id="AvailableQuantity" value="${parseFloat(arr.Table[i].AvailableQuantity).toFixed(QtyDecDigit)}" readonly class="form-control num_right" autocomplete="off" type="text" name="AvailableQuantity" placeholder="0000.00"  >
                                                            </div>
                                                             <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlStk">
                                                                 <button type="button" id="SubItemAvlStk" ${subitmDisable} class="calculator subItmImg"  onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                             </div>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="IssuedQuantity" value="${IssueQty}" onchange="OnChangeIssueQuantity(this,event)" onkeypress="return QtyFloatValueonly(this, event)" class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" placeholder="0000.00"  >
                                                                <span id="IssuedQuantity_Error" class="error-message is-visible"></span>
                                                                </div>
                                                             <div class="col-sm-2 i_Icon no-padding" id="div_SubItemIssueQty">
                                                                 <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                                                                 <button type="button" id="SubItemIssueQty" ${subitmDisable} class="calculator subItmImg"  onclick="return SubItemDetailsPopUp('IssueQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                             </div>
                                                            </td>
                                                             <td>
                                                                    <input id="itemprice" value="${parseFloat(arr.Table[i].sale_price).toFixed(RateDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" placeholder="0000.00" onchange="OnChangeItemPrice1(this,event)" onkeypress="return QtyFloatValueonly(this, event)">
                                                                    <span id="itemprice_Error" class="error-message is-visible"></span>
                                                            </td>
                                                            <td>

                                                                    <input id="tblvalue" value="${parseFloat(arr.Table[i].total_value).toFixed(ValDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" placeholder="0000.00" disabled>
                                                            </td>
                                                           `
                                    + Ibatch + ``
                                    + Iserial + ``
                                    + ItemType + `
                                                            <td>
                                                                <textarea id="remarks" class="form-control remarksmessage" name="remarks" @maxlength = "100",  placeholder="${$("#span_remarks").text()}" ></textarea>
                                                            </td>
                                                        </tr>
                                `);

                                BindWarehouseList(rowIdx);

                                //if (MRSType === "I") {
                                //    $('#MaterialIssueItemDetailsTbl td:nth-child(' + 10 + ')').hide();
                                //    $('#MaterialIssueItemDetailsTbl th:nth-child(' + 10 + ')').hide();
                                //}
                                //else {
                                //    $('#MaterialIssueItemDetailsTbl td:nth-child(' + 10 + ')').show();
                                //    $('#MaterialIssueItemDetailsTbl th:nth-child(' + 10 + ')').show();
                                //}
                            }

                            for (var i = 0; i < arr.Table1.length; i++) {
                                $("#hdn_Sub_ItemDetailTbl tbody").append(
                                    `<tr>
                                                <td><input type="text" id="ItemId" value='${arr.Table1[i].item_id}'></td>
                                                <td><input type="text" id="subItemId" value='${arr.Table1[i].sub_item_id}'></td>
                                                <td><input type="text" id="subItemQty" value=''></td>
                                                <td><input type="text" id="subItemReqQty" value='${arr.Table1[i].req_qty}'></td>
                                                <td><input type="text" id="subItemPendQty" value='${arr.Table1[i].pend_qty}'></td>
                                                <td><input type="text" id="subItemAvlQty" value=''></td>
                                            </tr>`)
                            }

                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    }

 
}

function QtyVSPlaceholder(qty) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    if (parseFloat(qty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {

    }
    else {

    }
}
function BindWarehouseList(id) {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/GRNDetail/GetWarehouseList",
            data: {},
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //PO_ErrorPage();
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
                        $("#wh_id" + id).html(s);
                    }
                }
            },
        });
}
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        Comn_BindItemBatchDetail(); //BindItemBatchDetail();
        var clickedrow = $(evt.target).closest("tr");
        var ItemId = clickedrow.find("#hdItemId").val();;
        var CompId = $("#HdCompId").val();
        var BranchId = $("#HdBranchId").val();
        var ItemId = clickedrow.find("#hdItemId").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();

        $("#WareHouseWiseItemName").val(ItemName);
        $("#WareHouseWiseUOM").val(UOMName);
        $.ajax(
            {

                type: "Post",
                url: "/ApplicationLayer/MaterialTransferIssue/MTI_getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId,
                    //CompId: CompId,
                    //BranchId: BranchId
                },
                success: function (data) {
                    debugger;
                    $('#ItemStockWareHouseWise').html(data);
                    $("#WareHouseWiseItemName").val(ItemName);
                    $("#WareHouseWiseUOM").val(UOMName);
                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();

    ItemInfoBtnClick(ItmCode);

    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/LSODetail/GetSOItemDetail",
    //                data: { ItemID: ItmCode },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                        ErrorPage();
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
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var MI_pedQty = clickedrow.find("#IssuedQuantity").val();

        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var CMD = $("#CMN_Command").val();
        var typ = $("#cmn_TransType").val();
        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            $("#BatchNumber").css("display", "block");
            clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/MaterialTransferIssue/getItemStockBatchWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemdetail: "",
                        CMD: "",
                        typ:""

                    },
                    success: function (data) {
                        debugger;
                        $('#ItemStockBatchWise').html(data);

                       
                    },
                });
        }
        else {
            var Mrs_Status = $("#hdMTIStatus").val();
            if (Mrs_Status == "" || Mrs_Status == null) {
                Comn_BindItemBatchDetail(); //BindItemBatchDetail();
                var Index = clickedrow.find("#SNohiddenfiled").val();
                var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                //var ddlId = "#wh_id" + Index;
                //var ItemId = clickedrow.find("#hdItemId").val();;
                var WarehouseId = $("#fromWh").val();
                var CompId = $("#HdCompId").val();
                var BranchId = $("#HdBranchId").val();
                var CMD = $("#CMN_Command").val();
                var typ = $("#cmn_TransType").val();
                var Docid = $("#DocumentMenuId").val();

                //$("#ItemNameBatchWise").val(ItemName);
                //$("#UOMBatchWise").val(UOMName);
                //$("#QuantityBatchWise").val(MI_pedQty);
                //$("#HDItemNameBatchWise").val(ItemId);
                //$("#HDUOMBatchWise").val(UOMId);
                //$("#BatchwiseTotalIssuedQuantity").text("");
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialTransferIssue/getItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            SelectedItemdetail: SelectedItemdetail,
                            CMD: CMD,
                            typ: typ

                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);

                            var Index = clickedrow.find("#SNohiddenfiled").val();
                            var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                            //var ddlId = "#wh_id" + Index;
                            //var ItemId = clickedrow.find("#hdItemId").val();;
                            var WarehouseId = $("#fromWh").val();
                            var CompId = $("#HdCompId").val();
                            var BranchId = $("#HdBranchId").val();

                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(MI_pedQty);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            //$("#BatchwiseTotalIssuedQuantity").text("");

                            try {
                                //For Auto fill Quantity on FIFO basis in the Batch Table.
                                //this will work only first time after save old value will come in the table
                                //Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", PackedQty, "ToatlAvlQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                                Cmn_PackingAutoFillBatchQty("BatchWiseItemStockTbl", IssueQuantity, "ToatlAvlQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                            } catch (err) {
                                console.log('Error : ' + err.message)
                            }
                        },
                    });
            }
            //if (Mrs_Status == "C" || Mrs_Status == "I") {
            else {
                $("#ItemNameBatchWise").val(ItemName);
                $("#UOMBatchWise").val(UOMName);
                $("#QuantityBatchWise").val(MI_pedQty);
                $("#HDItemNameBatchWise").val(ItemId);
                $("#HDUOMBatchWise").val(UOMId);
                $("#BatchwiseTotalIssuedQuantity").text("");

                var Mrs_IssueType = $("#TransferType").val();                
                var Mrs_No = $("#txtMaterialIssueNo").val();
                var Mrs_Date = $("#txtMaterialIssueDate").val();
                var CMD = $("#CMN_Command").val();
                var typ = $("#cmn_TransType").val();

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialTransferIssue/getItemStockBatchWiseAfterStockUpadte",
                        data: {
                            IssueType: Mrs_IssueType,
                            IssueNo: Mrs_No,
                            IssueDate: Mrs_Date,
                            ItemID: ItemId,
                            CMD: CMD,
                            typ: typ
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);

                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(MI_pedQty);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            //$("#BatchwiseTotalIssuedQuantity").text("");

                        },
                    });
            }
        }
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl tbody tr").each(function () {
        $(this).find("#IssuedQuantity").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function ItemStockSerialWise(el, evt) {
    try {
        debugger;

        var clickedrow = $(evt.target).closest("tr");
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var QtyDecDigit = $("#QtyDigit").text();
        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var ItemId = clickedrow.find("#hdItemId").val();;
        var UOMID = clickedrow.find("#hdUOMId").val();
        var MI_pedQty = clickedrow.find("#IssuedQuantity").val();
        var CMD = $("#CMN_Command").val();
        var typ = $("#cmn_TransType").val();
        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");

            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/MaterialTransferIssue/getItemstockSerialWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemSerial: "",
                        CMD: "",
                        typ: ""

                    },
                    success: function (data) {
                        $('#ItemStockSerialWise').html(data);
                    },
                });

        }
        else {

            var Mrs_Status = $("#hdMTIStatus").val();
            if (Mrs_Status == "" || Mrs_Status == null) {
                Comn_BindItemSerialDetail(); //BindItemSerialDetail();
                var Index = clickedrow.find("#SNohiddenfiled").val();
                //var ddlId = "#wh_id" + Index;

                var WarehouseId = $("#fromWh").val();
                var CompId = $("#HdCompId").val();
                var BranchId = $("#HdBranchId").val();
                var CMD = $("#CMN_Command").val();
                var typ = $("#cmn_TransType").val();
                var SelectedItemSerial = $("#HDSelectedSerialwise").val();

                //$("#ItemNameSerialWise").val(ItemName);
                //$("#UOMSerialWise").val(UOMName);
                //$("#ShippedQuantitySerialWise").val(MI_pedQty);
                //$("#HDItemIDSerialWise").val(ItemId);
                //$("#HDUOMIDSerialWise").val(UOMID);
                //$("#TotalIssuedSerial").val("");

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialTransferIssue/getItemstockSerialWise",
                        data: {
                            ItemId: ItemId,
                            WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            SelectedItemSerial: SelectedItemSerial,
                            CMD: CMD,
                            typ: typ
                        },
                        success: function (data) {
                            debugger;
                           
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(MI_pedQty);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);

                        },
                    });
            }
            if (Mrs_Status == "C" || Mrs_Status == "I") {
                //$("#ItemNameSerialWise").val(ItemName);
                //$("#UOMSerialWise").val(UOMName);
                //$("#ShippedQuantitySerialWise").val(MI_pedQty);
                //$("#HDItemIDSerialWise").val(ItemId);
                //$("#HDUOMIDSerialWise").val(UOMID);
                //$("#TotalIssuedSerial").val("");

                var Mrs_IssueType = $("#TransferType").val();      
                var Mrs_No = $("#txtMaterialIssueNo").val();
                var Mrs_Date = $("#txtMaterialIssueDate").val(); var CMD = $("#CMN_Command").val();
                var typ = $("#cmn_TransType").val();
               
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialTransferIssue/getItemstockSerialWiseAfterStockUpadte",
                        data: {
                            IssueType: Mrs_IssueType,
                            IssueNo: Mrs_No,
                            IssueDate: Mrs_Date,
                            ItemID: ItemId,
                            CMD: CMD,
                            typ: typ
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(MI_pedQty);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                            var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                            if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                                $("#SaveItemSerialTbl TBODY TR").each(function () {
                                    var row = $(this)
                                    var HdnItemId = row.find("#Serial_ItemId").val();
                                    if (ItemId === HdnItemId) {
                                        TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#Serial_Qty").val());
                                    }
                                });
                            }
                            $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
                        },
                    });
            }

        }

    } catch (err) {
        console.log("Material Issue Error : " + err.message);
    }
}
function OnChangeIssueQty(el, evt) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalIssueQty = 0;
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
        var AvailableQuantity = clickedrow.find("#AvailableQuantity").val();
        if (IssuedQuantity != "" && IssuedQuantity != null && AvailableQuantity != "" && AvailableQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {

                clickedrow.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
                clickedrow.find("#IssuedQuantity_Error").css("display", "block");
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
                var test = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
                if (parseFloat(IssuedQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit) || IssuedQuantity == "" || IssuedQuantity==null) { /**Modifed BY Nitesh 02-03-2024 For REmove "0.000"**/
                    clickedrow.find("#IssuedQuantity").val("");
                }
                else {
                    clickedrow.find("#IssuedQuantity").val(test);
                }
            }
            else {
                clickedrow.find("#IssuedQuantity_Error").css("display", "none");
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(IssuedQuantity).toFixed(QtyDecDigit);
                if (parseFloat(IssuedQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit) || IssuedQuantity == "" || IssuedQuantity == null) { /**Modifed BY Nitesh 02-03-2024 For REmove "0.000"**/
                    clickedrow.find("#IssuedQuantity").val("");
                }
                else {
                    clickedrow.find("#IssuedQuantity").val(test);
                }
               
            }
        }

        $("#BatchWiseItemStockTbl TBODY TR").each(function () {
            var row = $(this);
            var Issueqty = row.find("#IssuedQuantity").val();
            if (Issueqty != "" && Issueqty != null) {
                TotalIssueQty = parseFloat(TotalIssueQty) + parseFloat(Issueqty);
            }
            debugger;
        });

        $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssueQty).toFixed(QtyDecDigit));
    }
    catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var IsuueFlag = true;
    var ItemMI_Qty = $("#QuantityBatchWise").val();
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var AvailableQuantity = row.find("#AvailableQuantity").val();
        var IssuedQuantity = row.find("#IssuedQuantity").val();
        if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {
            row.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
            ValidateEyeColor(row, "BtnBatchDetail", "Y");
            IsuueFlag = false;
        }
    });

    if (IsuueFlag) {
        if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
            swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
        }
        else {
            debugger;
            var SelectedItem = $("#HDItemNameBatchWise").val();
            $("#SaveItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#Batch_ItemId").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });

            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemIssueQuantity = row.find("#IssuedQuantity").val();
                if (ItemIssueQuantity != "" && ItemIssueQuantity != null) {

                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    //var ItemExpiryDate = row.find("#ExpiryDate").val();
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    var AvailableQty = row.find("#AvailableQuantity").val();
                    var mfg_name = row.find("#BtMfgName").val();
                    var mfg_mrp = row.find("#BtMfgMrp").val();
                    var mfg_date = row.find("#BtMfgDate").val();
                    var LotNo = row.find("#Lot").val();
                    $('#SaveItemBatchTbl tbody').append(
                        `<tr>
                    <td><input type="text" id="Batch_LotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="Batch_ItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="Batch_UOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="Batch_BatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="Batch_Qty" value="${ItemIssueQuantity}" /></td>
                    <td><input type="text" id="Batch_ExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="Batch_AvlBatchQty" value="${AvailableQty}" /></td>
                    <td id="Batch_MfgName">${mfg_name}</td>
                    <td><input type="text" id="Batch_MfgMrp" value="${mfg_mrp}" /></td>
                    <td><input type="text" id="Batch_MfgDate" value="${mfg_date}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');
        }
        $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
                /*commented by Hina on 13-02-2024 to chng validate Eye Color*/
                 //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                ValidateEyeColor(clickedrow, "BtnBatchDetail", "N");
               
            }
        });
    }
}
function onchangeChkItemSerialWise() {
    var TotalIssueLot = 0;
    var QtyDigit = $("#QtyDigit").text();
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        if (row.find("#ChkItemSerialWise").is(":checked")) {
            TotalIssueLot = parseFloat(TotalIssueLot) + 1;
        }
    });
    $("#TotalIssuedSerial").text(parseFloat(TotalIssueLot).toFixed(QtyDigit));
    // localStorage.setItem('BatchResetFlag', 'True');
}
function onclickbtnItemSerialReset() {
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#ChkItemSerialWise").attr("checked", false);
    });
    $("#TotalIssuedSerial").text("");
}
function onclickbtnItemSerialSaveAndExit() {
    debugger;
    var ItemMI_Qty = $("#QuantitySerialWise").val();
    var ItemTotalIssuedQuantity = $("#TotalIssuedSerial").text();
    if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
        swal("", $("#SerializedIssuedQtydoesnotmatchwithIssueQty").text(), "warning");
    }
    else {
        debugger;
        var SelectedItem = $("#HDItemIDSerialWise").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#Serial_ItemId").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
        $("#ItemSerialwiseTbl TBODY TR").each(function () {
            var row = $(this);
            var ItemUOMID = $("#HDUOMIDSerialWise").val();
            var ItemId = $("#HDItemIDSerialWise").val();
            var ItemLOTNO = row.find("#Lot").val();
            var mfg_name = row.find("#SrMfgName").val();
            var mfg_mrp = row.find("#SrMfgMrp").val();
            var mfg_date = row.find("#SrMfgDate").val();
            var ItemIssueQuantity = "1";
            var ItemSerialNO = row.find("#SerialNumberDetail").val();
            if (row.find("#ChkItemSerialWise").is(":checked")) {
                $('#SaveItemSerialTbl tbody').append(`<tr>
            <td><input type="text" id="Serial_ItemId" value="${ItemId}" /></td>
            <td><input type="text" id="Serial_UOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="Serial_LOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="Serial_Qty" value="${ItemIssueQuantity}" /></td>
            <td><input type="text" id="Serial_SerialNo" value="${ItemSerialNO}" /></td>
            <td><input type="text" id="Serial_MfgName" value="${mfg_name}" /></td>
            <td><input type="text" id="Serial_MfgMrp" value="${mfg_mrp}" /></td>
            <td><input type="text" id="Serial_MfgDate" value="${mfg_date}" /></td>
            </tr>
            `
                );
            }
        });
        $("#SerialDetail").modal('hide');

        $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
               
                /*commented by Hina on 13-02-2024 to chng validate Eye Color*/
                //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                ValidateEyeColor(clickedrow, "BtnSerialDetail", "N");
            }
        });
    }
}
function onclickbtnItemBatchDiscardAndExit() {
    $("#BatchNumber").modal('hide');
}
function onclickbtnItemSerialDiscardAndExit() {
    $("#SerialDetail").modal('hide');
}
function CheckTransportDetailValidation() {/*Add By NItesh on 05-08-2024 for Transport Detail*/
    debugger;
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
    if (NoOfPacks == "" || NoOfPacks == "0" || NoOfPacks == "0.000") {
        $("#Span_No_Of_Packages").text($("#valueReq").text());
        $("#Span_No_Of_Packages").css("display", "block");
        $("#NoOfPacks").css("border-color", "red");
        ErrorFlag = "Y";     
    }
    if (txttrpt_name == "" || txttrpt_name == "0") {
        $("#Span_TransporterName").text($("#valueReq").text());
        $("#Span_TransporterName").css("display", "block");
        //$("#TxtTranspt_Name").css("border-color", "red");
       // $("[aria-labelledby='select2-DdlTranspt_Name-container']").css("border-color", "red");
        $("#DdlTranspt_Name").css("border-color", "red");
        ErrorFlag = "Y";
       
    }
    if (txtveh_number == "" || txtveh_number == "0") {
        $("#Span_VehicleNumber").text($("#valueReq").text());
        $("#Span_VehicleNumber").css("display", "block");
        $("#TxtVeh_Number").css("border-color", "red");
        ErrorFlag = "Y";
    }

    if (ErrorFlag == "Y") {
       // swal("", $("#TransporterDetailNotFound").text(), "warning");
        return false;
    }
    else {
        return true;
    }
}
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
    $("#DdlTranspt_Name").css("border-color", "#ced4da");
}
function OnChangeVehicleNumber() {
    $("#Span_VehicleNumber").css("display", "none");
    $("#TxtVeh_Number").css("border-color", "#ced4da");
}
function OnChangeNoOfPackages(el, e) {
    var QtyDecDigit = $("#QtyDigit").text();
    FreightAmount = $("#NoOfPacks").val();
    $("#NoOfPacks").val(parseFloat(CheckNullNumber(FreightAmount)).toFixed(QtyDecDigit));
    $("#Span_No_Of_Packages").css("display", "none");
    $("#NoOfPacks").css("border-color", "#ced4da");
}
function CheckFormValidation() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }    
    var rowcount = $('#MaterialIssueItemDetailsTbl tr').length;
    var ValidationFlag = true;
    var TransferType = $('#TransferType').val();
    var fromwh = $('#fromWh').val();
    var towh = $('#toWh').val();
    var Tobranch = $("#tobranch").val();
    var MTR_No = $('#ddlMTR_No').val();
    var Fr_branch = $('#Fr_branch').val();
    var PR_number = $('#ddlMTR_No option:selected').text();


    $("#hdtrfType").val(TransferType);
    $("#hdWh_ID").val(fromwh);
    $("#hdWhID").val(towh);
    $("#hdBRID").val(Tobranch);
    $("#hdfrom_whName").val(Fr_branch);
    $("#hdReq_Number").val(PR_number);
   

    //$('#hdMrsType').val(MRS_Type);   

    if (TransferType == "" || TransferType == "0") {
        document.getElementById("vmTransferType").innerHTML = $("#valueReq").text();
        $("#TransferType").css("border-color", "red");
        ValidationFlag = false;
    }
    if (fromwh == "" || fromwh == "0") {
        $('#spanFromWh').text($("#valueReq").text());
        $("#spanFromWh").css("display", "block");
        //document.getElementById("vmwarehouse").innerHTML = $("#valueReq").text();
        $("#fromWh").css("border-color", "red");
        ValidationFlag = false;
    }
    if (towh == "" || towh == "0") {
        document.getElementById("vmtowarehouse").innerHTML = $("#valueReq").text();
        $("#toWh").css("border-color", "red");
        ValidationFlag = false;
    }
    if (Tobranch == "" || Tobranch == "0") {
        document.getElementById("vmbranch").innerHTML = $("#valueReq").text();
        $("#tobranch").css("border-color", "red");
        ValidationFlag = false;
    }    
    if (MTR_No == "" || MTR_No == "0") {
        document.getElementById("vmMTR_No").innerHTML = $("#valueReq").text();
        $("#ddlMTR_No").css("border-color", "red");
        $(".select2-container--default .select2-selection--single").css("border-color", "red");
        ValidationFlag = false;
    }
    var MRS_Status = $('#hdMTIStatus').val().trim();

    if (MRS_Status == "I") {
        return true;
    }
    if (ValidationFlag == true) {
     
    
        if (rowcount > 1) {

            var flag = CheckMI_mentItemValidations();
            if (flag == false) {
                return false;
            }
            var Batchflag = CheckItemBatchValidation();
            if (Batchflag == false) {
                return false;
            }
            var SerialFlag = CheckItemSerialValidation();
            if (SerialFlag == false) {
                return false;
            }
            debugger;
            var subitmFlag = CheckValidations_forSubItems();
            if (subitmFlag == false) {
                return false;
            }

            /*Modified By Nitesh 23-09-2024  Commented for Transport Deatil is not Mandatory */
            //var flag1 = CheckTransportDetailValidation()
            //if (flag1 == false) {/*Add By Nitesh on 05-08-2024 for transporter detail for MTR*/
               
            //    $("#collapsethree").removeClass("panel-collapse collapse").addClass("panel-collapse collapse show");
            //    return false;
            //}

            if (flag == true && Batchflag == true && SerialFlag == true && subitmFlag == true) {

                var MTRNo = $("#ddlMTR_No option:selected").text();
                //$("#HdMTRNo").val(MTRNo);
                $("#ddlMTR_No").val(MTRNo);

                var MI_NONo = $("hdmi_no").val();
                $("#mi_Number").val(MI_NONo);

                var MI_dt = $("hdmi_dt").val();
                $("#txtship_dt").val(MI_dt);



                var DeliveryNoteItemDetailList = new Array();
                $("#MaterialIssueItemDetailsTbl TBODY TR").each(function () {
                    var row = $(this);

                    //var Index = row.find("#SNohiddenfiled").val();
                    //var whERRID = ("#wh_id").val();
                    var ItemList = {};

                    ItemList.ItemName = row.find("#ItemName").val();
                    ItemList.ItemId = row.find("#hdItemId").val();
                    ItemList.UOM = row.find("#UOM").val();
                    ItemList.UOMId = row.find('#hdUOMId').val();
                    ItemList.wh_name = row.find('#WHName').val();
                    ItemList.sub_item = row.find('#sub_item').val();
                    ItemList.WareHouseId = fromwh;
                    ItemList.mrs_qty = row.find('#RequisitionQuantity').val();
                    ItemList.pend_qty = row.find("#PendingQuantity").val();
                    ItemList.TotalWeight = row.find("#TotalWeight").val();
                    ItemList.MI_pedQuantity = row.find("#ShippedQuantity").val();
                    ItemList.avl_stock = row.find('#AvailableQuantity').val();
                    ItemList.issue_qty = row.find('#IssuedQuantity').val();
                    ItemList.remarks = row.find('#remarks').val();                  
                    ItemList.i_batch = row.find('#hdi_batch').val();
                    ItemList.i_serial = row.find('#hdi_serial').val();
                    ItemList.itemprice = row.find('#itemprice').val();
                    ItemList.value = row.find('#tblvalue').val();
                    DeliveryNoteItemDetailList.push(ItemList);
                    debugger;
                });
                var str = JSON.stringify(DeliveryNoteItemDetailList);
                $('#hdMaterialIssueItemDetails').val(str);
                Comn_BindItemBatchDetail(); //BindItemBatchDetail();
                Comn_BindItemSerialDetail(); //BindItemSerialDetail();

                /*-----------Sub-item-------------*/

                var SubItemsListArr = MTI_SubItemList();
                var str2 = JSON.stringify(SubItemsListArr);
                $('#SubItemDetailsDt').val(str2);

                /*-----------Sub-item end-------------*/
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
              
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
        debugger;
       
    }
    else {
        //if (TotalBatchIssueQty != TotalMI_Qty) {
        //    swal("", $("#IssuedExceedsRequireQty").text(), "warning");
        //}
        return false;
    }

}
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Batchable = clickedrow.find("#hdi_batch").val();

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                /*commented by Hina on 13-02-2024 to chng validate Eye Color*/
                /*clickedrow.find("#IssuedQuantity").css("border-color", "red");*/
                ValidateEyeColor(clickedrow, "BtnBatchDetail", "Y");
                
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemBatchTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchIssueQty = currentRow.find('#Batch_Qty').val();
                    var bchitemid = currentRow.find('#Batch_ItemId').val();
                    var bchuomid = currentRow.find('#Batch_UOMId').val();
                    if (ItemId == bchitemid && UOMId == bchuomid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                    }
                });

                if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    /*commented by Hina on 13-02-2024 to chng validate Eye Color*/
                    //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                    ValidateEyeColor(clickedrow, "BtnBatchDetail", "N");
                }
                else {
                    /*commented by Hina on 13-02-2024 to chng validate Eye Color*/
                    /*clickedrow.find("#IssuedQuantity").css("border-color", "red");*/
                    ValidateEyeColor(clickedrow, "BtnBatchDetail", "Y");
                    BatchableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
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
    $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                /*commented by Hina on 13-02-2024 to chng validate Eye Color*/
                /*clickedrow.find("#IssuedQuantity").css("border-color", "red");*/
                ValidateEyeColor(clickedrow, "BtnSerialDetail", "Y");
                
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#Serial_Qty').val();
                    var srialitemid = currentRow.find('#Serial_ItemId').val();
                    var srialuomid = currentRow.find('#Serial_UOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });

                if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                    /*commented by Hina on 13-02-2024 to chng validate Eye Color*/
                    /*clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");*/
                    ValidateEyeColor(clickedrow, "BtnSerialDetail", "N");
                }
                else {
                    /*commented by Hina on 13-02-2024 to chng validate Eye Color*/
                    //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "BtnSerialDetail", "Y");
                    SerialableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#Serialqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Serialqtydoesnotmatchwithissuedqty").text(), "warning");
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
function OnChangeIssueQuantity(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var ValDecDigit = $("#ValDigit").text();///Amount
    var clickedrow = $(evt.target).closest("tr");
    var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
    var itemprice = clickedrow.find("#itemprice").val();
    if ((IssuedQuantity == "") || (parseFloat(IssuedQuantity) == parseFloat("0")) || (IssuedQuantity == null)) {
        clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
        clickedrow.find("#IssuedQuantity_Error").css("display", "block");
        clickedrow.find("#IssuedQuantity").css("border-color", "red");
        //clickedrow.find("#IssuedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val("");
        return false;
    }
    else {
        clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val(mi_issueqty);
       
        var totat_value = parseFloat(itemprice) * parseFloat(mi_issueqty)
        var totat_value1 = parseFloat(parseFloat(totat_value)).toFixed(parseFloat(ValDecDigit));
        clickedrow.find("#tblvalue").val(totat_value1);
    }
    var PendingQuantity = clickedrow.find("#PendingQuantity").val();
    var AvailableQuantity = clickedrow.find("#AvailableQuantity").val();


    if (parseFloat(IssuedQuantity) > parseFloat(PendingQuantity)) {
        debugger;
        clickedrow.find("#IssuedQuantity_Error").text($("#IssuedQtycannotbegreaterthanPendingQty").text());
        clickedrow.find("#IssuedQuantity_Error").css("display", "block");
        clickedrow.find("#IssuedQuantity").css("border-color", "red");
        clickedrow.find("#IssuedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        return false;
    }
    else {
        clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val(mi_issueqty);
    }
    if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {
        debugger;
        clickedrow.find("#IssuedQuantity_Error").text($("#IssuedQtyGreaterthanAvaiQty").text());
        clickedrow.find("#IssuedQuantity_Error").css("display", "block");
        clickedrow.find("#IssuedQuantity").css("border-color", "red");
        clickedrow.find("#IssuedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        return false;
    }
    else {
        clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val(mi_issueqty);
    }

}
function OnChangeItemPrice1(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var clickedrow = $(evt.target).closest("tr");
    var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
    var itemprice = clickedrow.find("#itemprice").val();
    
    var itemprice1 = parseFloat(parseFloat(itemprice)).toFixed(parseFloat(RateDecDigit));
    clickedrow.find("#itemprice").val(itemprice1);

    var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));

    var totat_value = parseFloat(itemprice) * parseFloat(mi_issueqty)
    var totat_value1 = parseFloat(parseFloat(totat_value)).toFixed(parseFloat(ValDecDigit));
    clickedrow.find("#tblvalue").val(totat_value1);
}
function CheckMI_mentItemValidations() {
    debugger;
    var ErrorFlag = "N";
    var RowIndex = 0;
    var MI_Type = $('#hdMrsType').val();
    var TxtQtyFocus = false;//Added by Suraj Maurya on 26-08-2025
    var TxtPriceFocus = false;//Added by Suraj Maurya on 26-08-2025
    $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        RowIndex++;
        var ddlId = "#wh_id" + RowIndex;
        var whERRID = "#wh_Error" + RowIndex;

        var IssueQuantity = currentRow.find("#IssuedQuantity").val();
        var PendingQuantity = currentRow.find("#PendingQuantity").val();
        var AvaialbleQuantity = currentRow.find("#AvailableQuantity").val();

        if (currentRow.find(ddlId).val() == "" || parseFloat(currentRow.find(ddlId).val()) == parseFloat("0")) {
            currentRow.find(whERRID).text($("#valueReq").text());
            currentRow.find(whERRID).css("display", "block");
            currentRow.find(ddlId).css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find(whERRID).css("display", "none");
            currentRow.find(ddlId).css("border-color", "#ced4da");
        }
        if (parseFloat(IssueQuantity) > parseFloat(AvaialbleQuantity)) {
            currentRow.find("#IssuedQuantity_Error").text($("#IssuedQtyGreaterthanAvaiQty").text());
            currentRow.find("#IssuedQuantity_Error").css("display", "block");
            currentRow.find("#IssuedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
            if (!TxtQtyFocus) {
                currentRow.find("#IssuedQuantity").focus();
                TxtQtyFocus = true;
            }
        }
        else {
            currentRow.find("#IssuedQuantity_Error").css("display", "none");
            currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
        }
        if (parseFloat(IssueQuantity) > parseFloat(PendingQuantity)) {
            currentRow.find("#IssuedQuantity_Error").text($("#IssuedQtycannotbegreaterthanPendingQty").text());
            currentRow.find("#IssuedQuantity_Error").css("display", "block");
            currentRow.find("#IssuedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
            if (!TxtQtyFocus) {
                currentRow.find("#IssuedQuantity").focus();
                TxtQtyFocus = true;
            }
        }
        else {
            currentRow.find("#IssuedQuantity_Error").css("display", "none");
            currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
        }
        
        /*if (parseFloat(IssueQuantity) == parseFloat("0") || parseFloat(IssueQuantity) == "") {*/ // Commented by Suraj Maurya on 26-08-2025
        if (parseFloat(CheckNullNumber(IssueQuantity)) == 0) {
            currentRow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            currentRow.find("#IssuedQuantity_Error").css("display", "block");
            currentRow.find("#IssuedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
            if (!TxtQtyFocus) {
                currentRow.find("#IssuedQuantity").focus();
                TxtQtyFocus = true;
            }
        }
        else {
            currentRow.find("#IssuedQuantity_Error").css("display", "none");
            currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
        }
        if (MI_Type === 'E') {
            var CostPrice = currentRow.find("#CostPrice").val();

            if (parseFloat(CostPrice) == parseFloat("0") || parseFloat(CostPrice) == "" || parseFloat(CostPrice) == null) {
                currentRow.find("#CostPrice_Error").text($("#valueReq").text());
                currentRow.find("#CostPrice_Error").css("display", "block");
                currentRow.find("#CostPrice").css("border-color", "red");
                ErrorFlag = "Y";
                if (!TxtPriceFocus) {
                    currentRow.find("#CostPrice").focus();
                    TxtPriceFocus = true;
                }
            }
            else {
                currentRow.find("#CostPrice_Error").css("display", "none");
                currentRow.find("#CostPrice").css("border-color", "#ced4da");
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
            //  var today = fromDate.getFullYear() + '-' + month + '-' + day;

            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdate").val(today);
            // alert('From Date cannnot be greater then To date');
            swal("", $("#fromtodatemsg").text(), "warning");



        }
    }
    else {
        //  alert('please select from and to date');
    }
}
function FilterMaterialIssueListData() {
    debugger;
    try {
        var RequisitionTyp = $("#ddlRequisitionTypeList option:selected").val();
        var RequiredArea = $("#ddlRequiredArea option:selected").val();
        var MaterialIssueTo = $("#ddlMaterialIssueTo option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus option:selected").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialIssue/MaterialIssuetListSearch",
            data: {
                RequisitionTyp: RequisitionTyp,
                RequiredArea: RequiredArea,
                MaterialIssueTo: MaterialIssueTo,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#MaterialIssuetbBody').html(data);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

    }
}
function onChangeddlRequisitionTypeList() {
    debugger;
    $("#ddlMTR_No").val(0);
    $("#txtMTR_Date").val("");
    $("#txtEntityType").val("");

    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    if (RequisitionTypeList != "0") {

        if (RequisitionTypeList == "E") {
            $("#div_IssueTo").css('display', 'block');
            //$("#txtEntityType").css('display', 'block');
            //$("#lblIssueTo").css('display', 'block');
        }
        if (RequisitionTypeList == "I") {
            $("#div_IssueTo").css('display', 'none');
            //$("#txtEntityType").css('display', 'none');
            //$("#lblIssueTo").css('display', 'none');
        }
        document.getElementById("vmRequisitionTypeList").innerHTML = null;
        $("#ddlRequisitionTypeList").css("border-color", "#ced4da");
        var RequiredArea = $('#ddlRequiredArea').val();
        if (RequiredArea != "0") {
            BindddlMaterialList();
        }
    }
    else {

        document.getElementById("vmRequisitionTypeList").innerHTML = $("#valueReq").text();
        $("#ddlRequisitionTypeList").css("border-color", "red");
        $("#txtEntityType").css('display', 'none');
        $("#lblIssueTo").css('display', 'none');
    }
}
function onChangeRequiredArea() {
    debugger;
    //$("#ddlMTR_No").val(0);
    $("#txtMTR_Date").val("");
    $("#txtEntityType").val("");
    var RequiredArea = $('#ddlRequiredArea').val();
    if (RequiredArea != "0") {
        document.getElementById("vmRequiredArea").innerHTML = null;
        $("#ddlRequiredArea").css("border-color", "#ced4da");
        var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
        if (RequisitionTypeList != "0") {
            BindddlMaterialList();
        }
    }
    else {
        document.getElementById("vmRequiredArea").innerHTML = $("#valueReq").text();
        $("#ddlRequiredArea").css("border-color", "red");

    }
}
function OnChangeToBranch() {
    debugger;
    var Tobranch = $("#tobranch").val();
    $("#hdBRID").val(Tobranch); 
    $("#vmbranch").css("display", "none");
    $("#tobranch").css("border-color", "#ced4da");
    $("#vmbranch").text("");
    var fromwh = $('#fromWh').val();
    if (fromwh != null && fromwh != "") {
        MTI_BindDestiWHList(Tobranch);
    }
    
    //$.ajax({
    //    type: "POST",
    //    url: "/ApplicationLayer/MaterialTransferOrder/GetToWHList1",
    //    //contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    //async: true,
    //    data: { Tobranch: Tobranch, },/*Registration pass value like model*/
    //    success: function (data) {
    //        if (data == 'ErrorPage') {
    //            ErrorPage();
    //            return false;
    //        }
    //        /*dynamically dropdown list of all Assessment */
    //        debugger;
    //        if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
    //            var arr = [];
    //            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
    //            var s = '<option value="0">---Select---</option>';
    //            for (var i = 0; i < arr.length; i++) {
    //                s += '<option value="' + arr[i].wh_id + '">' + arr[i].wh_name + '</option>';
    //            }
    //            $("#toWh").html(s);

    //        }
    //    },
    //    error: function (Data) {
    //    }
    //});

    BindddlMaterialList();
}
function MTI_BindDestiWHList() {
    debugger;
    var wh_id = $("#fromWh").val()
    var doc_id = $("#DocumentMenuId").val();
    var TransferType = $("#TransferType").val();
    if (TransferType == "0") {
        TransferType = "B"
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialTransferOrder/GetSourceAndDestinationList",
        data: { wh_id: wh_id, doc_id: doc_id },
        success: function (data) {
            debugger;
            var arr = JSON.parse(data);
            // if (TransferType == "SW") {
            var b = '<option value="0">---Select---</option>';
            for (var i = 0; i < arr.Table.length; i++) {
                b += '<option value=' + arr.Table[i].wh_id + '>' + arr.Table[i].wh_name + '</option>'
            }
            $("#toWh").html(b);
            //if (TransferType == "W") {
            //    $("#toWh").select2({
            //        templateResult: function (data) {
            //            debugger
            //            var selected = $("#fromWh").val();
            //            if (data.id != selected) {

            //                var classAttr = $(data.element).attr('class');
            //                var hasClass = typeof classAttr != 'undefined';
            //                classAttr = hasClass ? ' ' + classAttr : '';
            //                var $result = $(
            //                    '<div ' + classAttr + '">' + data.text + '</div>'
            //                );
            //                return $result;
            //            }
            //            firstEmptySelect = false;
            //        }
            //    });
            //}
            //$("#toWh").select2()
            //} else {
            //$("#DestinationShopfloor").html(a);
            //}
        },
    });
}
//function MTI_BindDestiWHList(Branch) {
//    debugger;

//    var TransferType = $("#TransferType").val();
//    var fromWh = $("#fromWh").val();
//    $.ajax(
//        {
//            type: "POST",
//            url: "/ApplicationLayer/MaterialTransferIssue/GetToWHList1",
//            data: {
//                Tobranch: Branch
//                /*FromWhID: fromwh*/
//            },
//            dataType: "json",
//            success: function (data) {
//                debugger;
//                if (data == 'ErrorPage') {
//                    GRN_ErrorPage();
//                    return false;
//                }
//                if (data !== null && data !== "") {
//                    var arr = [];
//                    arr = JSON.parse(data);
//                    if (arr.length > 0) {

//                        var s = '<option value="0">---Select---</option>';

//                        if (TransferType == "B") {
//                            for (var i = 0; i < arr.length; i++) {
//                                s += '<option value="' + arr[i].wh_id + '">' + arr[i].wh_name + '</option>';
//                            }
//                        }
//                        else {
//                            for (var i = 0; i < arr.length; i++) {

//                                var wh_id = arr[i].wh_id
//                                /*if (hdfrbr_ID == tobranch) {*/
//                                if (fromWh == wh_id) {
//                                    }
//                               /* }*/
//                                else {
//                                s += '<option value="' + arr[i].wh_id + '">' + arr[i].wh_name + '</option>';
//                                 }
//                            }
//                        }                     
//                        $("#toWh").html(s);
//                        $("#toWh").select2()
//                    }
//                }
//            },
//        });
//}
function OnChangeTranferType() {
    debugger;
    document.getElementById("vmTransferType").innerHTML = null;
    $("#TransferType").css("border-color", "#ced4da");
    var TraType = $("#TransferType").val();
    if (TraType == "W") {
        $("#fromWh").val(0).trigger('change');
        $('#tobranch').val($("#hdfrbr_ID").val()).prop('selected', true);
        OnChangeToBranch();
        $("#tobranch").attr("disabled", true);
        $("#vmbranch").css("display", "none");
        $("#tobranch").css("border-color", "#ced4da");
        $("#vmbranch").text("");
    }
    else {
        $("#fromWh").val(0).trigger('change');
        $("#toWh").val(0).trigger('change');
        var SrcBrnch = $("#hdfrbr_ID").val();
        //$("#Fr_branch option[value=" + DestBrnch + "]").attr('selected', true);
        $("#tobranch option[value=" + SrcBrnch + "]").select2().hide();
        $("#tobranch").attr("disabled", false);
        $('#tobranch').val(0).prop('selected', true);
    }
    BindddlMaterialList();
}
function OnChangefromWh() {
    debugger;
    var fromwh = $('#fromWh').val();
   
    if (fromwh == "0" || fromwh == "") {
        document.getElementById("vmwarehouse").innerHTML = $("#valueReq").text();
        $("#fromWh").css("border-color", "red");
    }
    else {
        var SrcBrId = $('#hdfrbr_ID').val();
        MTI_BindDestiWHList(SrcBrId);
        
        //document.getElementById("vmwarehouse").innerHTML = null;
        //$("#fromWh").css("border-color", "#ced4da");
        $("#spanFromWh").css("display", "none");
        $("[aria-labelledby='select2-fromWh-container']").css("border-color", "#ced4da");
        var transtyp = $("#TransferType").val();
        if (transtyp == "W") {
            $("#toWh").select2({
                templateResult: function (data) {
                    debugger
                    var selected = $("#fromWh").val();
                    if (data.id != selected) {

                        var classAttr = $(data.element).attr('class');
                        var hasClass = typeof classAttr != 'undefined';
                        classAttr = hasClass ? ' ' + classAttr : '';
                        var $result = $(
                            '<div ' + classAttr + '">' + data.text + '</div>'
                        );
                        return $result;
                    }
                    firstEmptySelect = false;
                }
            });
        }
        else {
            $("#toWh").select2();
        }
    }
   
    BindddlMaterialList();
}

function OnChangeToWh() {
    debugger;
    var towh = $('#toWh').val();
    var towhname = $('#toWh option:selected').text();
    if (towh == "0" || towh == "") {
        document.getElementById("vmtowarehouse").innerHTML = $("#valueReq").text();
        $("#toWh").css("border-color", "red");
    }
    else {
        $("#hdWhID").val(towh);
        $('#hdWhName').val(towhname);
        document.getElementById("vmtowarehouse").innerHTML = null;
        $("#toWh").css("border-color", "#ced4da");
    }
    BindddlMaterialList();
}
function OnclickCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").css("filter", "grayscale(0%)");
        $("#btn_save").attr("onclick", "return  CheckFormValidation();")
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").attr("onclick", "")
    }
}

/***--------------------------------Sub Item Section-----------------------------------------***/
// Commented due to not required this in this page
//function HideShowPageWise(sub_item, clickedrow) { 
//    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemProdQty",);
//    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemPlanQty",);
//    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlStockBtn",);
//}
function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#hfsno").val();
    var ProductNm = clickdRow.find("#ItemName").val();
    var ProductId = clickdRow.find("#hdItemId").val();
    var UOM = clickdRow.find("#UOM").val();
    var hdwhId = clickdRow.find("#hdwhId").val();

    var doc_no = $("#txtMaterialIssueNo").val();
    var doc_dt = $("#txtMaterialIssueDate").val();

    //var UOMID = clickdRow.find("#UOMID").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.qty = row.find('#subItemQty').val();
        List.req_qty = row.find('#subItemReqQty').val();
        List.pend_qty = row.find('#subItemPendQty').val();
        List.avl_stock = row.find('#subItemAvlQty').val();
        NewArr.push(List);
    });

    if (flag == "IssueQty") {
        Sub_Quantity = clickdRow.find("#IssuedQuantity").val();
    } else if (flag == "PendingQty") {
        Sub_Quantity = clickdRow.find("#PendingQuantity").val();
    } else if (flag == "ReqQty") {
        Sub_Quantity = clickdRow.find("#RequisitionQuantity").val().trim();
    }

    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hdMTIStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialTransferIssue/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: doc_no,
            Doc_dt: doc_dt,
            whId: hdwhId
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
function fn_CustomReSetSubItemData(itemId) {

    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
        var Crow = $(this).closest("tr");
        var subItemId = Crow.find("#subItemId").val();
        var subItemQty = Crow.find("#subItemQty").val();
        var subItemReqQty = Crow.find("#subItemReqQty").val();
        var subItemPendQty = Crow.find("#subItemPendQty").val();
        var subItemAvlQty = Crow.find("#subItemAvlQty").val();
        var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').length;
        if (len > 0) {
            $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').each(function () {
                var InnerCrow = $(this).closest("tr");
                //var ItemId = InnerCrow.find("#ItemId").val();
                InnerCrow.find("#subItemQty").val(subItemQty);
                InnerCrow.find("#subItemReqQty").val(subItemReqQty);
                InnerCrow.find("#subItemPendQty").val(subItemPendQty);
                InnerCrow.find("#subItemAvlQty").val(subItemAvlQty);

            });
        } else {
            
                $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                            <td><input type="text" id="subItemReqQty" value='${subItemReqQty}'></td>
                            <td><input type="text" id="subItemPendQty" value='${subItemPendQty}'></td>
                            <td><input type="text" id="subItemAvlQty" value='${subItemAvlQty}'></td>
                        </tr>`);

        }

    });

}
function CheckValidations_forSubItems() {
    //return Cmn_CheckValidations_forSubItems("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y");
    return Cmn_CheckValidations_forSubItems("MaterialIssueItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "SubItemIssueQty", "Y");
}
function GetSubItemAvlStock(e) {
    var Crow = $(e.target).closest('tr');
    var ProductNm = Crow.find("#ItemName").val();
    var ProductId = Crow.find("#hdItemId").val();
    var UOM = Crow.find("#UOM").val();
    var hdwhId = Crow.find("#hdwhId").val();
    var AvlStk = Crow.find("#AvailableQuantity").val();
    Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, hdwhId, AvlStk, "wh");

}

function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("MaterialIssueItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "SubItemIssueQty", "N");
}
function MTI_SubItemList() {
    var NewArr = new Array();

    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        //if (row.find('#subItemQty').val() != "" && row.find('#subItemQty').val() != "0") {
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.req_qty = row.find('#subItemReqQty').val();
            List.pend_qty = row.find('#subItemPendQty').val();
            List.avl_qty = row.find('#subItemAvlQty').val();
            NewArr.push(List);
        //}
    });
    return NewArr;
}
/***--------------------------------Sub Item Section End-----------------------------------------***/
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
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#txtMaterialIssueNo").val();
    //debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null) {
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    }

    return false;
}
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "MaterialIssueItemDetailsTbl", [{ "FieldId": "ItemName", "FieldType": "input" }]);
}


