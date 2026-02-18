var Span_Delete_Title = $("#Span_Delete_Title").text();
var ItemUOM = $("#ItemUOM").text();
var ItmInfo = $("#ItmInfo").text();
var span_remarks = $("#span_remarks").text();

$(document).ready(function () {
    //OnChangeTransferType();
    debugger
    hideLoader();
    $("#SHFLTransferList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var WF_Status = $('#WF_Status').val();
            var clickedrow = $(e.target).closest("tr");
            var trfNumber = clickedrow.children("#trf_no").text();
            var trfDate = clickedrow.children("#trf_dt1").text();
            if (trfNumber != null && trfNumber != "" && trfDate != null && trfDate != "") {
                window.location.href = "/ApplicationLayer/ShopfloorStockTransfer/ShopfloorStockTransfer_Edit/?Trf_Number=" + trfNumber + "&Trf_Date=" + trfDate + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $('#ShopfloorStockTransferDetailTbl tbody').on('click', '.deleteIcon', function () {
        var clickedrow = $(this).closest('tr');
        debugger
       
        var rowno = clickedrow.find("#HdnFieldSrNo").val();
        var itemId = clickedrow.find("#ProductName" + rowno).val();//
        if ($('#ShopfloorStockTransferDetailTbl tbody tr').length == 1) {
            EnableHeader();
        }
        $("#SaveItemBatchTbl tbody tr").each(function () {
            var row = $(this);
            var B_itemId = row.find("#Batch_ItemId").val();
            if (itemId == B_itemId) {
                row.remove();
            }
        });
        $("#SaveItemSerialTbl tbody tr").each(function () {
            var row = $(this);
            var S_itemId = row.find("#Serial_ItemId").val();
            if (itemId == S_itemId) {
                row.remove();
            }
        });
        Cmn_DeleteSubItemQtyDetail(itemId);
        $(this).closest('tr').remove();
        ResetSrNoAfterDeleteRow();
    });
    GetViewAndDetail();
    OnChangeTransferType();
    BindDestinationDDL();

    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#ShopfloorStockTransferDetailTbl > tbody > tr').each(function () {
            var cellText = $(this).find('#HdnProductName').val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
});

//----------------------WorkFlow------------------------------
function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Doc_No = clickedrow.children("#trf_no").text();
    var Doc_Dt = clickedrow.children("#trf_dt1").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(Doc_No);

    var Doc_Status = clickedrow.children("#Doc_Status").text();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    GetWorkFlowDetails(Doc_No, Doc_Dt, Doc_id, Doc_Status);
    var a = 1;
}
function onchangeMaterialType() {
    debugger;
    var materialTyp = $("#MaterialType").val();
    if (materialTyp == "RJ" || materialTyp == "RW" || materialTyp == "AC") {
        $("#DestinationShopfloor").val("0").trigger('change');
    }
    BindDestinationDDL();
}
function BtnSearch() {
    debugger;
    try {
        var TransferType = $("#TransferType").val();
        var MaterialType = $("#MaterialType").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ShopfloorStockTransfer/GetSHFLTransferListFiltered",
            data: {
                TransferType: TransferType,
                MaterialType: MaterialType,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#SHFLTransferList').html(data);
                $('#ListFilterData').val(TransferType + ',' + MaterialType + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
    ResetWF_Level();
}
function ForwardBtnClick() {
    debugger;
    //var OrderStatus = "";
    //OrderStatus = $('#hdn_Status').val().trim();
    //if (OrderStatus === "D" || OrderStatus === "F") {

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

    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var SHFL_STKtRFdT = $("#TransferDate").val();
    $.ajax({
        type: "POST",
        /* url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: SHFL_STKtRFdT
        },
        success: function (data) {
            /*if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 12-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
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
                /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                /*Above Commented and modify by Hina sharma on 12-05-2025 to check Existing with previous year transaction*/
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
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
     var ModelData = "";
     var mailerror = "";

    Remarks = $("#fw_remarks").val();
    DocNo = $("#TransferNumber").val();
    DocDate = $("#TransferDate").val();
    docid = $("#DocumentMenuId").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (DocNo + ',' + DocDate + ',' + WF_Status1);
    var ListFilterData1 = $("#ListFilterData1").val();
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
     if (fwchkval != "Approve") {
         var pdfAlertEmailFilePath = "ShopfloorStockTransfer_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
         var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/ShopfloorStockTransfer/SavePdfDocToSendOnEmailAlert");
         if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
             pdfAlertEmailFilePath = "";
         }
     }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ShopfloorStockTransfer/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ trf_no: DocNo, trf_dt: DocDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/ShopfloorStockTransfer/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ShopfloorStockTransfer/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ShopfloorStockTransfer/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
}
function GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, fileName) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ShopfloorStockTransfer/SavePdfDocToSendOnEmailAlert",
        data: { DocNo: DocNo, DocDate: DocDate, fileName: fileName },
        /*dataType: "json",*/
        success: function (data) {

        }
    });
}
function OtherFunctions(StatusC, StatusName) {

}

function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#TransferNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}

//----------------------WorkFlow End------------------------------

function GetViewAndDetail() {

    BindItemList("#ProductName", 1, "#ShopfloorStockTransferDetailTbl", "#HdnFieldSrNo", "BindData", "MRS");
    var Doc_No = $("#TransferNumber").val();
    $("#hdDoc_No").val(Doc_No);
    var SourceShopfloor = $("#TransferType").val();
    if (SourceShopfloor == "SW") {
        //BindDestinationDDL();//add by shubham maurya on 30-12-2024.
    }
}

function BindItemsList() {
    //var PLItemListData = JSON.parse(sessionStorage.getItem("itemList"));
    //if (PLItemListData != null) {
    //    if (PLItemListData.length > 0) {
    //        $("#ShopfloorStockTransferDetailTbl >tbody >tr").each(function () {
    //            debugger;
    //            var currentRow = $(this);
    //            var rowid = currentRow.find("#HdnFieldSrNo").val();
    //            rowid = parseInt(rowid) + 1;
    //            $("#ProductName" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#SOItemName").text()}' data-uom='${$("#SOItemUOM").text()}'></optgroup>`);
    //            for (var i = 0; i < PLItemListData.length; i++) {
    //                $('#Textddl' + rowid).append(`<option data-uom="${PLItemListData[i].uom_name}" value="${PLItemListData[i].Item_id}">${PLItemListData[i].Item_name}</option>`);
    //            }
    //            var firstEmptySelect = true;
    //            $("#ProductName" + rowid).select2({
    //                templateResult: function (data) {
    //                    var selected = $("#ProductName" + rowid).val();
    //                    if (check(data, selected, "#ShopfloorStockTransferDetailTbl", "#HdnFieldSrNo", "#ProductName") == true) {
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
    //                    }
    //                    firstEmptySelect = false;
    //                }
    //            });

    //        });
    //    }
    //}
}

function BindData() {

    debugger
    BindItemsList();

    //$("#ShopfloorStockTransferDetailTbl tbody tr").each(function () {
    //    var row = $(this);
    //    var productid = row.find("#HdnProductName").val();
    //    if (productid != null && productid != "") {
    //        var RowNo = row.find("#HdnFieldSrNo").val();//OnChangeProductNameSST(event)
    //        row.find("#ProductName" + RowNo).attr("onchange","")
    //        row.find("#ProductName" + RowNo).val(productid).change();
    //        row.find("#ProductName" + RowNo).attr("onchange", "OnChangeProductNameSST(event)");
    //    }

    //});
}
function OnClickIconBtn(e) {

    var clickedrow = $(e.target).closest('tr');
    var Rowno = clickedrow.find("#HdnFieldSrNo").val();
    var ItmCode = clickedrow.find("#ProductName" + Rowno).val();
    ItemInfoBtnClick(ItmCode);

}
function OnChangeTransferType() {
    var TransferType = $("#TransferType").val();
    var MaterialType = $("#MaterialType").val();
    if (TransferType == "0") {
        TransferType="SW"
    }
    debugger
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ShopfloorStockTransfer/GetSourceAndDestinationList",
            data: { TransferType, MaterialType: MaterialType },
            success: function (data) {
                debugger;
                var arr = JSON.parse(data);
                var a = '<option value="0">---Select---</option>';
                if (arr.Table.length > 0) {
                    for (var i = 0; i < arr.Table.length; i++) {
                        a += '<option value=' + arr.Table[i].shfl_id + '>' + arr.Table[i].shfl_name + '</option>'
                    }
                    $("#SourceShopfloor").html(a);
                    $("#SourceShopfloor").select2();
                    var SourceShopfloor = $("#hdnSourceShopfloor").val();
                    if (SourceShopfloor != null && SourceShopfloor != "" && SourceShopfloor != "0") {
                        if (TransferType == "SW") {
                            var b = '<option value="0">---Select---</option>';
                            for (var i = 0; i < arr.Table1.length; i++) {
                                b += '<option value=' + arr.Table1[i].wh_id + '>' + arr.Table1[i].wh_name + '</option>'
                            }
                            $("#DestinationShopfloor").html(b);
                        } else {
                            $("#DestinationShopfloor").html(a);
                        }


                        $("#SourceShopfloor").val(SourceShopfloor).change();
                        var DestinationShopfloor = $("#hdnDestinationShopfloor").val();
                        $("#DestinationShopfloor").val(DestinationShopfloor).change();
                        $("#BtnAddItem").closest(".plus_icon1").show();
                    }
                    else {
                        $("#DestinationShopfloor").html('<option value="0">---Select---</option>');
                        $("#BtnAddItem").closest(".plus_icon1").hide();
                    }                   
                    $("#DestinationShopfloor").select2();
                    $("#vmDestinationShopfloor").text("");
                    $("#vmDestinationShopfloor").css("display", "none");
                }
                
            },
            error: function OnError(xhr, errorType, exception) {
                hideLoader();
                debugger;
            }
        });
    }
    catch (err) {
    }

}
function OnChangeMaterialType() {
    var MaterialType = $("#MaterialType").val();
    $.ajax({
        type: "POST",
        url: "",
        data: { TransferType },
        success: function (data) {

        },

    });

}
function DisableHeader() {
    $("#TransferType").attr("disabled", true);
    $("#MaterialType").attr("disabled", true);
    $("#SourceShopfloor").attr("disabled", true);

    //$("#DestinationShopfloor").attr("disabled", true);
}
function EnableHeader() {
    $("#TransferType").attr("disabled", false);
    $("#MaterialType").attr("disabled", false);
    $("#SourceShopfloor").attr("disabled", false);

    //$("#DestinationShopfloor").attr("disabled", true);
}
function BindDestinationDDL() {
    var TransferType = $("#TransferType").val();
    var MaterialType = $("#MaterialType").val();
    if (TransferType == "0") {
        TransferType = "SW"
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ShopfloorStockTransfer/GetSourceAndDestinationList",
        data: { TransferType, MaterialType: MaterialType },
        success: function (data) {
            debugger;
            var arr = JSON.parse(data);
            var a = '<option value="0">---Select---</option>';
            var SourceShfl = $("#SourceShopfloor").val();
            for (var i = 0; i < arr.Table.length; i++) {
                if (SourceShfl != arr.Table[i].shfl_id) {
                    a += '<option value=' + arr.Table[i].shfl_id + '>' + arr.Table[i].shfl_name + '</option>'
                }
            }
            if (TransferType == "SW") {
                var b = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.Table1.length; i++) {
                    b += '<option value=' + arr.Table1[i].wh_id + '>' + arr.Table1[i].wh_name + '</option>'
                }
                $("#DestinationShopfloor").html(b);
            } else {
                $("#DestinationShopfloor").html(a);
            }
            //$("#SourceShopfloor").select2();
            //$("#DestinationShopfloor").select2();
            var DestinationShopfloor = $("#hdnDestinationShopfloor").val();
            if (DestinationShopfloor != null && DestinationShopfloor != "") {
                $("#DestinationShopfloor").val(DestinationShopfloor).change();
            }
        },

    });

}
function OnchangeSourceShopfloor() {
    if (CheckVallidation("SourceShopfloor", "vmSourceShopfloor") == true) {
      
        $("[aria-labelledby='select2-SourceShopfloor-container']").css("border-color", "#ced4da");

        $("#BtnAddItem").closest(".plus_icon1").show();
        $("#ShopfloorStockTransferDetailTbl >tbody >tr").each(function () {
            var clickedrow = $(this);
            var rowno = clickedrow.find("#HdnFieldSrNo").val();
            clickedrow.find("#ProductName" + rowno).attr("disabled", false);
            clickedrow.find("#TransferQuantity").attr("disabled", false);
            clickedrow.find("#remarks").attr("disabled", false);
            clickedrow.find("#delBtnIcon").addClass("deleteIcon");
        });
        BindDestinationDDL();
    }
    else {
        $("#vmSourceShopfloor").text($("#valueReq").text());
        $("#vmSourceShopfloor").css("display", "block");
        $("[aria-labelledby='select2-SourceShopfloor-container']").css("border-color", "red");
        $("#BtnAddItem").closest(".plus_icon1").hide();

        $("#ShopfloorStockTransferDetailTbl >tbody >tr").each(function () {
            var clickedrow = $(this);
            var rowno = clickedrow.find("#HdnFieldSrNo").val();
            clickedrow.find("#ProductName" + rowno).attr("disabled", true);
            clickedrow.find("#TransferQuantity").attr("disabled", true);
            clickedrow.find("#remarks").attr("disabled", true);
        });
    }
}
function OnchangeDestinationShopfloor() {

    if (CheckVallidation("DestinationShopfloor", "vmDestinationShopfloor") == true) {
        $("[aria-labelledby='select2-DestinationShopfloor-container']").css("border-color", "#ced4da");
    }
    else {
        $("[aria-labelledby='select2-DestinationShopfloor-container']").css("border-color", "red");
    }
    
}
function AddNewRow() {
  
    var RowNo = $("#ShopfloorStockTransferDetailTbl> tbody> tr:last-child td #HdnFieldSrNo").val();
    if (RowNo != null) {
        RowNo = RowNo;
    }
    else {
        RowNo = 0;
    }
    AddNewEmptyRow(parseInt(RowNo) + 1);

    BindItemList("#ProductName", (parseInt(RowNo) + 1), "#ShopfloorStockTransferDetailTbl", "#HdnFieldSrNo", "", "MRS");
  
}
function AddNewEmptyRow(RowNo) {

    var SrNo = $("#ShopfloorStockTransferDetailTbl> tbody> tr").length;
    var span_SubItemStockDetail = $("#span_SubItemStockDetail").val();
    $('#ShopfloorStockTransferDetailTbl > tbody').append(`
                                                    <tr>
                                                       <td class="red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="${Span_Delete_Title}"></i></td>
                                                        <td id="rowId" class="sr_padding">${++SrNo}</td>
                                                        <td>
                                                            <div class="col-sm-11 lpo_form" style="padding:0px;" id='ProductName'>
                                                                <select class="form-control" id="ProductName${RowNo}" name="ProductName${RowNo}" onchange="OnChangeProductNameSST(event)">
                                                                </select>
                                                                 <input id="HdnProductName" value="" hidden="hidden" />
                                                                <span id="ItemListNameError" class="error-message is-visible"></span>
                                                                <input id="HdnFieldSrNo" value=${RowNo} hidden="hidden"/>
                                                            </div>
                                                            <div class="col-sm-1 i_Icon">
                                                                <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${ItmInfo}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM"  placeholder="${ItemUOM}"  onblur="this.placeholder='${ItemUOM}'" disabled>
                                                                  <input id="UOMID" type="hidden">
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 no-padding">
                                                            <input id="AvailableQuantity" class="form-control num_right" autocomplete="off" type="text" name="AvailableQuantity" disabled  placeholder="0000:00"  onblur="this.placeholder ='0000:00'">
                                                            </div>
                                                            <div class="col-sm-2 i_Icon" id="div_SubItemAvlQty" >
                                                                <button type="button" id="SubItemAvlQty" disabled class="calculator subItmImg" onclick="return SubItemShopfloorAvlStock('AvlQty',event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemStockDetail}">
                                                                </button>
                                                            </div>
                                                        </td>                                                        
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="TransferQuantity" class="form-control num_right" onchange="OnChangeTransferQuantity(this)" onkeypress="return QtyFloatValueonly(this,event)" autocomplete="off" type="text" name="TransferQuantity"  placeholder="0000:00" >
                                                         <span id="TransferQuantityError" class="error-message is-visible"></span>
                                                            </div>
                                                            <div class="col-sm-2 i_Icon" id="div_SubItemTrfQty" >
                                                                <input hidden type="text" id="sub_item" value="" />
                                                                <button type="button" id="SubItemTrfQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemStockDetail}"></button>
                                                            </div>
                                                        </td>                                                       
                                                        <td class="center">
                                                            <button type="button" id="BtnBatchDetail" onclick="ItemStockBatchWise(this,event)" disabled onchange="OnChangeIssueQty" class="calculator" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                                            <input type="hidden" id="hdi_batch" value="N" style="display: none;">
                                                        </td>
                                                        <td class="center">
                                                            <button type="button" id="BtnSerialDetail" onclick="ItemStockSerialWise(this,event)" disabled class="calculator " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                            <input type="hidden" id="hdi_serial" value="N" style="display: none;">
                                                        </td>
                                                        <td>
                                                            <textarea id="remarks"  class="form-control remarksmessage" name="remarks"  placeholder="${span_remarks}" ></textarea>
                                                        </td>
                                                    </tr>
`)


   
}
function OnChangeProductNameSST(e) {
    debugger
    var Itm_ID = e.currentTarget.value;
    var clickedrow = $(e.target).closest("tr");
    DeleteItemBatchDetailOnchangeItem(clickedrow.find("#HdnProductName").val());
    clickedrow.find("#HdnProductName").val(Itm_ID);
    if (Itm_ID != null && Itm_ID != "" && Itm_ID != 0) {
        var rowno = clickedrow.find("#HdnFieldSrNo").val();
        clickedrow.find("[aria-labelledby='select2-ProductName" + rowno +"-container']").css("border-color", "#cda4da");
        clickedrow.find("#ItemListNameError").text("");
        clickedrow.find("#ItemListNameError").css("display", "none");
        $.ajax(
            {
                type: "POST",
                url: "/Common/Common/GetItemUOM",
                data: {
                    Itm_ID: Itm_ID,
                    ItemUomType: "base"
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
                            clickedrow.find("#UOM").val(arr.Table[0].uom_alias);
                            clickedrow.find("#UOMID").val(arr.Table[0].uom_id);
                            clickedrow.find("#hdi_batch").val(arr.Table[0].i_batch);
                            clickedrow.find("#hdi_serial").val(arr.Table[0].i_serial);
                            try {
                                HideShowPageWise(arr.Table[0].sub_item, clickedrow);
                            }
                            catch (ex) {
                                //console.log(ex);
                            }
                            if (arr.Table[0].i_batch == "Y") {
                                clickedrow.find("#BtnBatchDetail").attr("disabled", false);
                                clickedrow.find("#BtnBatchDetail").removeClass("subItmImg");
                            }
                            else {
                                clickedrow.find("#BtnBatchDetail").attr("disabled", true);
                                clickedrow.find("#BtnBatchDetail").addClass("subItmImg");
                            }
                            if (arr.Table[0].i_serial == "Y") {
                                clickedrow.find("#BtnSerialDetail").attr("disabled", false);
                                clickedrow.find("#BtnSerialDetail").removeClass("subItmImg");
                            }
                            else {
                                clickedrow.find("#BtnSerialDetail").attr("disabled", true);
                                clickedrow.find("#BtnSerialDetail").addClass("subItmImg");
                            }
                        }
                        else {
                            clickedrow.find("#UOM").val("");
                            clickedrow.find("#BtnBatchDetail").attr("disabled", true);
                            clickedrow.find("#BtnSerialDetail").attr("disabled", true);
                            clickedrow.find("#hdi_batch").val("N");
                            clickedrow.find("#hdi_serial").val("N");
                        }

                    }
                    var MaterialType = $("#MaterialType").val();
                    var SourceShopfloor = $("#SourceShopfloor").val();
                    $.ajax({
                        type: "POST",
                        url: "/Common/Common/GetItemAvlStockShopfloor",
                        data: {
                            Itm_ID: Itm_ID,
                            MaterialType: MaterialType,
                            SourceShopfloor: SourceShopfloor
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
                                if (clickedrow != null) {
                                    if (arr.Table.length > 0) {
                                        clickedrow.find("#AvailableQuantity").val(parseFloat(arr.Table[0].avl_stock_shfl).toFixed($("#QtyDigit").text()));
                                    }
                                    else {
                                        clickedrow.find("#AvailableQuantity").val(parseFloat("0").toFixed($("#QtyDigit").text()));
                                    }
                                }


                            }
                        },
                    });
                    var srno = clickedrow.find("#HdnFieldSrNo").val();
                    clickedrow.find("#ProductName" + srno).focus();
                    clickedrow.find("#TransferQuantity").val("");
                    clickedrow.find("#remarks").val("");
                    DisableHeader();

                },
            });
    }
    else {
        clickedrow.find("#AvailableQuantity").val("");
        var srno = clickedrow.find("#HdnFieldSrNo").val();
        clickedrow.find("#ProductName" + srno).focus();
        clickedrow.find("#TransferQuantity").val("");
        clickedrow.find("#remarks").val("");

        clickedrow.find("#UOM").val("");
        clickedrow.find("#BtnBatchDetail").attr("disabled", true);
        clickedrow.find("#BtnSerialDetail").attr("disabled", true);
        clickedrow.find("#hdi_batch").val("N");
        clickedrow.find("#hdi_serial").val("N");
    }

    

}
function OnChangeTransferQuantity(el) {
    debugger
    var TransferQuantity = el.value;
    if (AvoidDot(TransferQuantity) == false) {
        TransferQuantity = 0;
    }
    el.value = parseFloat(TransferQuantity).toFixed($("#QtyDigit").text());
    /*Add by Hina on 02-03-2024 to show blank instead of 0 in inserted field*/
    if (el.value == "0" || el.value == parseFloat(0)) {
        $(el).closest("tr").find("#TransferQuantity").val("");
    }
    /*Code End*/ 
    var AvailableQuantity = $(el).closest("tr").find("#AvailableQuantity").val();
    if (parseFloat(AvailableQuantity) < parseFloat(TransferQuantity)) {
        el.value = parseFloat(0).toFixed($("#QtyDigit").text());
        $(el).closest("tr").find("#TransferQuantity").css("border-color", "red");
        $(el).closest("tr").find("#TransferQuantityError").text($("#IssuedQtyGreaterthanAvaiQty").text());
        $(el).closest("tr").find("#TransferQuantityError").css("display", "block");
    }
    else {
        $(el).closest("tr").find("#TransferQuantity").css("border-color", "#ced4da");
        $(el).closest("tr").find("#TransferQuantityError").text("");
        $(el).closest("tr").find("#TransferQuantityError").css("display", "none");
    }

  
}
function ResetSrNoAfterDeleteRow() {

    var SrNo = 1;
    $("#ShopfloorStockTransferDetailTbl tbody tr").each(function () {

        $(this).find("#rowId").text(SrNo);
        SrNo++;
    })
}
function CheckValidationOnfields() {
    var Flag = "N";
    if (CheckVallidation("TransferType", "vmTransferType") == false) {
        Flag = "Y";
    }
    if (CheckVallidation("MaterialType", "vmMaterialType") == false) {
        Flag = "Y";
    }
    if (CheckVallidation("SourceShopfloor", "vmSourceShopfloor") == false) {
        $("[aria-labelledby='select2-SourceShopfloor-container']").css("border-color", "red");
        Flag = "Y";
    }
    if (CheckVallidation("DestinationShopfloor", "vmDestinationShopfloor") == false) {
        $("[aria-labelledby='select2-DestinationShopfloor-container']").css("border-color", "red");
        Flag = "Y";
    }
    if (Flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemValidations() {
    debugger;
    var ErrorFlag = "N";
    $("#ShopfloorStockTransferDetailTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var srno = currentRow.find("#HdnFieldSrNo").val();
        var TransferQuantity = currentRow.find("#TransferQuantity").val();
        if (AvoidDot(TransferQuantity) == false) {
            TransferQuantity = 0;
        }
        var Batchable = currentRow.find("#hdi_batch").val();
        var Serialable = currentRow.find("#hdi_serial").val();
        var ItemId = currentRow.find("#ProductName" + srno).val();//TransferQuantity
        if (ItemId == "" || ItemId == "0" || ItemId==null) {
            currentRow.find("#ItemListNameError").text($("#valueReq").text());
            currentRow.find("#ItemListNameError").css("display", "block");
            currentRow.find("[aria-labelledby='select2-ProductName" + srno+"-container']").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ItemListNameError").css("display", "none");
            currentRow.find("[aria-labelledby='select2-ProductName" + srno+"-container']").css("border-color", "#ced4da");
        }
        if (parseFloat(TransferQuantity) <= 0 || TransferQuantity == null) {
            currentRow.find("#TransferQuantityError").text($("#valueReq").text());
            currentRow.find("#TransferQuantityError").css("display", "block");
            currentRow.find("#TransferQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#TransferQuantityError").css("display", "none");
            currentRow.find("#TransferQuantity").css("border-color", "#ced4da");
        }

    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemBatch_Validation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#ShopfloorStockTransferDetailTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var TransferQuantity = clickedrow.find("#TransferQuantity").val();
        var rowno = clickedrow.find("#HdnFieldSrNo").val();
        var ItemId = clickedrow.find("#ProductName" + rowno).val();
        var UOMId = clickedrow.find("#UOMID").val();
        var Batchable = clickedrow.find("#hdi_batch").val();

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                 //clickedrow.find("#BtnBatchDetail").css("border-color", "red");
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

                if (parseFloat(TransferQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#BtnBatchDetail").css("border-color", "#007bff");
                    ValidateEyeColor(clickedrow, "BtnBatchDetail", "N");
                }
                else {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#BtnBatchDetail").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "BtnBatchDetail", "Y");
                    BatchableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterBatchDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithpackedqty").text(), "warning");
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
function CheckItemSerial_Validation() {
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#ShopfloorStockTransferDetailTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var TransferQuantity = clickedrow.find("#TransferQuantity").val();
        var rowno = clickedrow.find("#HdnFieldSrNo").val();
        var ItemId = clickedrow.find("#ProductName" + rowno).val();
        var UOMId = clickedrow.find("#UOMID").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#BtnSerialDetail").css("border-color", "red");
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

                if (parseFloat(TransferQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#BtnSerialDetail").css("border-color", "#007bff");
                    ValidateEyeColor(clickedrow, "BtnSerialDetail", "N");
                }
                else {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#BtnSerialDetail").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "BtnSerialDetail", "Y");
                    SerialableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterSerialDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Serializedqtydoesnotmatchwithpackedqty").text(), "warning");
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
function CheckFormValidation() {
    debugger;

    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var StockTransferItemTable = $('#ShopfloorStockTransferDetailTbl tbody tr').length;
    

    if (CheckValidationOnfields() == true) {
        if (StockTransferItemTable > 0 ) {
           
            if (CheckItemValidations() == false) {
                return false;
            }
            else if (CheckItemBatch_Validation() == false) {
                return false;
            }
            else if (CheckItemSerial_Validation() == false) {
                return false;
            }
            else if (CheckValidations_forSubItems() == false) {
                return false;
            }
            else {
                var ItemDetailList = new Array();
               
                $("#ShopfloorStockTransferDetailTbl TBODY TR").each(function () {
                    var row = $(this);
                    var InputItemList = {};
                    var RowNo = row.find("#HdnFieldSrNo").val();
                    InputItemList.ItemId = row.find("#ProductName" + RowNo).val();
                    InputItemList.UOMId = row.find('#UOMID').val();
                    InputItemList.TransferQuantity = row.find('#TransferQuantity').val();
                    InputItemList.remarks = row.find('#remarks').val();
                    ItemDetailList.push(InputItemList);
                    debugger;
                });

                var ItmStr = JSON.stringify(ItemDetailList);
               
                $('#hd_ItemDetail').val(ItmStr);

                var SubItemsListArr = Cmn_SubItemList();
                var strsubitemlist = JSON.stringify(SubItemsListArr);
                $('#SubItemDetailsDt').val(strsubitemlist);

                Comn_BindItemBatchDetail();
                Comn_BindItemSerialDetail();
                EnableHeader();
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
                return true;
            }
      
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
function QtyFloatValueonly(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }
}




//----------------------Batch Detail-----------------------------//


function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssueQuantity = clickedrow.find("#TransferQuantity").val();
        var srno = clickedrow.find("#HdnFieldSrNo").val();
        var ItemName = clickedrow.find("#ProductName" + srno+" option:selected").text();
        var UOMName = clickedrow.find("#UOM").val();
        var MI_pedQty = clickedrow.find("#TransferQuantity").val();

        var ItemId = clickedrow.find("#ProductName" + srno).val();
        var UOMId = clickedrow.find("#UOMID").val();
        if (AvoidDot(IssueQuantity) == false) {
            IssueQuantity = "";
        }
        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            $("#BatchNumber").css("display", "block");
            clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/ShopfloorStockTransfer/getItemStockBatchWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemdetail: ""
                    },
                    success: function (data) {
                        debugger;
                        $('#ItemStockBatchWise').html(data);

                    },
                });
        }
        else {
            var Doc_Status = $("#hdn_Status").val();
            //if (Mrs_Status == "" || Mrs_Status == null) {
            Comn_BindItemBatchDetail();
            debugger;
                var SelectedItemdetail = $("#HDSelectedBatchwise").val();
            var trf_no = $("#TransferNumber").val();
            var trf_dt = $("#TransferDate").val();
            var MaterialType = $("#MaterialType").val();
            var typ = $("#batchwiseTransType").val();
            var Cmd = $("#BatchWiseCommand").val();
                var ShflID = $("#SourceShopfloor").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/ShopfloorStockTransfer/getItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            trf_no: trf_no,
                            trf_dt:trf_dt,
                            ShflID: ShflID,
                            MaterialType: MaterialType,
                            Doc_Status: Doc_Status,
                            SelectedItemdetail: SelectedItemdetail,
                            typ: typ,
                            Cmd: Cmd
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);
                            var Index = clickedrow.find("#SNohiddenfiled").val();
                           
                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(MI_pedQty);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            
                            $("#BatchwiseTotalIssuedQuantity").val("");
                            //Added by Suraj on 16-03-2024
                            try {
                                //For Auto fill Quantity on FIFO basis in the Batch Table.
                                //this will work only first time after save old value will come in the table
                                Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", MI_pedQty, "AvailableQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                            } catch (err) {
                                console.log('Error : ' + err.message)
                            }
                        },
                    });
           // }
            
            //else {


            //    var Mrs_IssueType = $("#hdMrsType").val();
            //    var Mrs_No = $("#txtMaterialIssueNo").val();
            //    var Mrs_Date = $("#txtMaterialIssueDate").val();

            //    $.ajax(
            //        {
            //            type: "Post",
            //            url: "/ApplicationLayer/MaterialIssue/getItemStockBatchWiseAfterStockUpadte",
            //            data: {
            //                IssueType: Mrs_IssueType,
            //                IssueNo: Mrs_No,
            //                IssueDate: Mrs_Date,
            //                ItemID: ItemId,
            //            },
            //            success: function (data) {
            //                debugger;
            //                $('#ItemStockBatchWise').html(data);
            //                $("#ItemNameBatchWise").val(ItemName);
            //                $("#UOMBatchWise").val(UOMName);
            //                $("#QuantityBatchWise").val(MI_pedQty);
            //                $("#HDItemNameBatchWise").val(ItemId);
            //                $("#HDUOMBatchWise").val(UOMId);
            //                $("#BatchwiseTotalIssuedQuantity").val("");
            //            },
            //        });
            //}
        }
    } catch (err) {
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
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    var AvailableQty = row.find("#AvailableQuantity").val();
                    var LotNo = row.find("#Lot").val();
                    var mfg_name = row.find("#BtMfgName").val();
                    var mfg_mrp = row.find("#BtMfgMrp").val();
                    var mfg_date = row.find("#BtMfgDate").val();
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
        $("#ShopfloorStockTransferDetailTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var rowno = clickedrow.find("#HdnFieldSrNo").val();
            var ItemId = clickedrow.find("#ProductName" + rowno).val();
            if (ItemId == SelectedItem) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#BtnBatchDetail").css("border-color", "#007bff");
                ValidateEyeColor(clickedrow, "BtnBatchDetail", "N");
                
            }
        });
    }
}
function DeleteItemBatchDetailOnchangeItem(DelItemID) {
    $("#SaveItemBatchTbl > tbody > tr").each(function () {
        var Crow = $(this);
        var rowitem = Crow.find("#Batch_ItemId").val();
        if (rowitem == DelItemID) {
            $(this).remove();
        }
    });
    Cmn_DeleteSubItemQtyDetail(DelItemID)
}
function OnChangeIssueQty(el, evt) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalIssueQty = 0;
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
        var AvailableQuantity = clickedrow.find("#AvailableQuantity").val();
        if (AvoidDot(IssuedQuantity) == false) {
            IssuedQuantity = "0";
        }
        if (IssuedQuantity != "" && IssuedQuantity != null && AvailableQuantity != "" && AvailableQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {

                clickedrow.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
                clickedrow.find("#IssuedQuantity_Error").css("display", "block");
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
                var test = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
                clickedrow.find("#IssuedQuantity").val(test);
            }
            else {
                clickedrow.find("#IssuedQuantity_Error").css("display", "none");
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(IssuedQuantity).toFixed(QtyDecDigit);
                clickedrow.find("#IssuedQuantity").val(test);
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
function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl tbody tr").each(function () {
        $(this).find("#IssuedQuantity").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
//----------------------Serial Detail-----------------------------//

function ItemStockSerialWise(el, evt) {
    try {
        debugger;

        var clickedrow = $(evt.target).closest("tr");
        var ConsumedQuantity = clickedrow.find("#TransferQuantity").val();
        var srno = clickedrow.find("#HdnFieldSrNo").val();
        var ItemName = clickedrow.find("#ProductName" + srno + " option:selected").text();
        var UOMName = clickedrow.find("#UOM").val();
        var ItemId = clickedrow.find("#ProductName" + srno).val();
        var UOMID = clickedrow.find("#UOMID").val();

        if (parseFloat(ConsumedQuantity) == "0" || parseFloat(ConsumedQuantity) == "") {
            clickedrow.find("#consumedqty_Error").text($("#FillQuantity").text());
            clickedrow.find("#consumedqty_Error").css("display", "block");
            clickedrow.find("#CConsumedQuantity").css("border-color", "red");
        }
        else {

            var Doc_Status = $("#hdn_Status").val();
           
                Comn_BindItemSerialDetail();

                var SelectedItemSerial = $("#HDSelectedSerialwise").val();
                var Shfl_Id = $("#SourceShopfloor").val();
            var trf_no = $("#TransferNumber").val();
            var trf_dt = $("#TransferDate").val();
            var typ = $("#batchwiseTransType").val();
            var Cmd = $("#BatchWiseCommand").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/ShopfloorStockTransfer/getItemstockSerialWise",
                        data: {
                            ItemId: ItemId,
                            ShflId: Shfl_Id, 
                            trf_no: trf_no,
                            trf_dt: trf_dt,
                            Doc_Status: Doc_Status,
                            SelectedItemSerial: SelectedItemSerial,
                            typ: typ,
                            Cmd: Cmd
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(ConsumedQuantity);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                        },
                    });
           
        }
    } catch (err) {
        console.log("Production Confirmation Error : " + err.message);
    }
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

        $("#ShopfloorStockTransferDetailTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var rowno = clickedrow.find("#HdnFieldSrNo").val();
            var ItemId = clickedrow.find("#ProductName" + rowno).val();
            if (ItemId == SelectedItem) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#BtnSerialDetail").css("border-color", "#007bff");
                ValidateEyeColor(clickedrow, "BtnSerialDetail", "N");
                
            }
        });
       
    }
}
function onclickbtnItemSerialReset() {
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#SrMfgName").val("");
        row.find("#SrMfgMrp").val("");
        row.find("#SrMfgDate").val("");
        row.find("#ChkItemSerialWise").attr("checked", false);
    });
    $("#TotalIssuedSerial").text("");
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
}
function functionConfirm(e) {
    debugger;
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
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");

            /* $("#txtFromdate").val(FromDate);*/
        }
    }

}

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {

    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemTrfQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlQty");

}
function SubItemDetailsPopUp(flag, e) {
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#HdnFieldSrNo").val();
    var ProductNm = clickdRow.find("#ProductName" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#ProductName" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    //var UOMID = clickdRow.find("#UOMID").val();
    var Doc_no = $("#TransferNumber").val();
    var Doc_dt = $("#TransferDate").val();
    var src_shfl = $("#SourceShopfloor").val();
    var MaterialType = $("#MaterialType").val();
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
        Sub_Quantity = clickdRow.find("#TransferQuantity").val();
    } else if (flag == "AvlQty") {
        Sub_Quantity = clickdRow.find("#AvailableQuantity").val();
    } 

    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hdn_Status").val();


    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ShopfloorStockTransfer/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: Doc_no,
            src_shfl: src_shfl,
            Doc_dt: Doc_dt,
            MaterialType: MaterialType
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
    return Cmn_CheckValidations_forSubItems("ShopfloorStockTransferDetailTbl", "HdnFieldSrNo", "ProductName", "TransferQuantity", "SubItemTrfQty", "Y");
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("ShopfloorStockTransferDetailTbl", "HdnFieldSrNo", "ProductName", "TransferQuantity", "SubItemTrfQty", "N");
}

function SubItemShopfloorAvlStock(flag,e) {
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#HdnFieldSrNo").val();
    var ProductNm = clickdRow.find("#ProductName" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#ProductName" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var SourceShopfloor = $("#SourceShopfloor").val();
    var Sub_Quantity = clickdRow.find("#AvailableQuantity").val();
    var MaterialType = $("#MaterialType").val();
    //To Get Available Stock of sub-item.
    Cmn_SubItemShopfloorAvlStock(ProductNm, ProductId, UOM, SourceShopfloor, Sub_Quantity, MaterialType);
    //$.ajax({
    //    type: "POST",
    //    url: "/Common/Common/GetSubItemShflAvlstockDetails",
    //    data: {
    //        shfl_id: SourceShopfloor,
    //        Item_id: ProductId
    //    },
    //    success: function (data) {
    //        debugger;
    //        $("#SubItemStockPopUp").html(data);
    //        $("#Stk_Sub_ProductlName").val(ProductNm);
    //        $("#Stk_Sub_ProductlId").val(ProductId);
    //        $("#Stk_Sub_serialUOM").val(UOM);
    //        $("#Stk_Sub_Quantity").val(Sub_Quantity);
    //    }

    //});
}

/***--------------------------------Sub Item Section End-----------------------------------------***/


function approveonclick() { /**Added this Condition by Nitesh 15-01-2024 for Disable Approve btn after one Click**/
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