
$(document).ready(function () {
    debugger;
    $("#fromWh").select2();
    $("#toWh").select2();
    var Doc_no = $("#TRFNumber").val();
    $("#hdDoc_No").val(Doc_no);
    BindItmList(1,"")
    $('#MTOItmDetailTbl tbody').on('click', '.deleteIcon', function () {
       
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
        
        var SNo1 = $(this).closest('tr').find("#SpanRowId").text();
        var SNo = SNo1.trim();
        var ItemCode = "";
       
        ItemCode = $(this).closest('tr').find("#Itemname_" + SNo).val();
       
        Cmn_DeleteSubItemQtyDetail(ItemCode);
       
        // Removing the current row. 
            $(this).closest('tr').remove();
        SerialNoAfterDelete();
        var rowCount = $('#MTOItmDetailTbl >tbody >tr').length;
        if (rowCount == 0) {
            $("#TransferType").attr("disabled", false)
            $("#fromWh").attr("disabled", false)
            $("#toWh").attr("disabled", false)
            if ($("#TransferType").val() == "B") {
                $("#Fr_branch").attr("disabled", false)
            }
        }
        // Decreasing total number of rows by 1. 
        //rowIdx--;
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var DocNo = clickedrow.children("#TRFNo").text();
        var DocDate = clickedrow.children("#trf_date").text();
        var doc_Id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(DocNo);
        debugger;
        GetWorkFlowDetails(DocNo, DocDate, doc_Id);

    });
    $("#MTOItmDetailTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        debugger;
        var rowno = currentRow.find("#SNohf").val();
        if (rowno != "1") {
            BindItmList(rowno,"")
        }
    });
    debugger;
    var Doc_no = $("#TRFNumber").val();
    $("#hdDoc_No").val(Doc_no);
    EnableSaveButtonAfterClickCancelOrFC()
    var rowCount = $('#MTOItmDetailTbl >tbody >tr').length;
    if (rowCount == 0) {
        $("#MTOItmDetailTbl .plus_icon1").css("display", "none");
    }
    else if (rowCount > 0) {
        $("#TransferType").attr("disabled", true)
        $("#fromWh").attr("disabled", true)
        $("#toWh").attr("disabled", true)
        if ($("#TransferType").val() == "W" || $("#TransferType").val() == "B") {
            $("#Fr_branch").attr("disabled", true)
        }
    }
})
function EnableSaveButtonAfterClickCancelOrFC() {
    $("#ForceClose").on("click",function () {
        if ($("#ForceClose").is(":checked")) {
            $("#btn_save").prop('disabled', false);
            $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        } else {
            $("#btn_save").prop('disabled', true);
            $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        }
       });
    $("#CancelFlag").on("click", function () {
        if ($("#CancelFlag").is(":checked")) {
            $("#btn_save").prop('disabled', false);
            $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        } else {
            $("#btn_save").prop('disabled', true);
            $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        }
    });
    
}
//function ShowItemListItm(ItemCode) {
//    debugger;
//    if (ItemCode != "0") {
//        $("#MTOItmDetailTbl >tbody >tr").each(function () {
//            debugger;
//            var currentRow = $(this);
//            var Sno = currentRow.find("#SNohf").val();
//            $("#Itemname_"+Sno+" option[value=" + ItemCode + "]").removeClass("select2-hidden-accessible");
//        });
//    }
//}
function GetSubItemAvlStock1(e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohf").val();
    var ProductNm = clickdRow.find("#Itemname_" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#Itemname_" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var Wh_id = $("#fromWh").val();
    var br_id = $("#Fr_branch").val();
    var ItemQty = clickdRow.find("#AvailableStockInBase").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialTransferOrder/GetSubItemDetails1",
        data: {
            Wh_id: Wh_id,
            Item_id: ProductId,
            br_id: br_id
        },
        success: function (data) {
            debugger;
            $("#SubItemStockPopUp").html(data);
            $("#Stk_Sub_ProductlName").val(ProductNm);
            $("#Stk_Sub_ProductlId").val(ProductId);
            $("#Stk_Sub_serialUOM").val(UOM);
            $("#Stk_Sub_Quantity").val(ItemQty);
        }
    });
}
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#MTOItmDetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);
    });
};
function OnChangeFromBranch() {
    debugger;
    var Tobranch = $("#Fr_branch").val();
    $("#vmbranch").css("display", "none");
    $("#Fr_branch").css("border-color", "#ced4da");
    $("#vmbranch").text("");
    BindSrcWHList(Tobranch);
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
    //            $("#fromWh").html(s);
              
    //        }
    //    },
    //    error: function (Data) {
    //    }
    //});

    //BindDLLItemList();
}
function BindDestinationDDL() {
    debugger;
    var wh_id = $("#fromWh").val();
    var doc_id = $("#DocumentMenuId").val();
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
                var b = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    b += '<option value=' + arr.Table[i].wh_id + '>' + arr.Table[i].wh_name + '</option>'
                }
            $("#toWh").html(b);

            var towh = $('#toWh').val();
            if (towh != "0" && towh != "" && towh != null) {
                $("#MTOItmDetailTbl .plus_icon1").css("display", "block");
            }
            else {
                $("#MTOItmDetailTbl .plus_icon1").css("display", "none");
            }
        },
    });
}
function OnChangeTranferType() {
    debugger;
    document.getElementById("vmTransferType").innerHTML = null;
    $("#TransferType").css("border-color", "#ced4da");
    var TraType = $("#TransferType").val();
    $("#hdtrfType").val(TraType);
    if (TraType == "W") {
        $("#fromWh").val(0).trigger('change');
        $("#toWh").val(0).trigger('change');
        $("#hdtrfType").val(TraType);
        //var DestBrName = $("#tobranch").val();
       /* var DestBrId = $("#hfbr_ID").val();*/
        //$('#Fr_branch').empty();
        //var towh = '<option value='+ DestBrId+ '>'+DestBrName+'</option>'
        //$('#Fr_branch').append(towh);
        var DestBrnch = $("#hfbr_ID").val();
        $("#Fr_branch option[value=" + DestBrnch + "]").select2().show();
        $("#Fr_branch").attr('disabled', true);
        $('#Fr_branch').val($("#hfbr_ID").val()).prop('selected', true);
        BindSrcWHList(DestBrnch)
     }
    else {
        $("#fromWh").val(0).trigger('change');
        $("#toWh").val(0).trigger('change');
        var DestBrnch=  $("#hfbr_ID").val();
        var SrcBrnch = $("#Fr_branch").val();
        //$("#Fr_branch option[value=" + DestBrnch + "]").attr('selected', true);
        $("#Fr_branch option[value=" + DestBrnch + "]").select2().hide();
        $("#Fr_branch").attr('disabled', false);
        $('#Fr_branch').val(0).prop('selected', true);
        
    }
   
}

function OnChangefromWh() {
    debugger;
    var towh = $('#toWh').val();
    var fromwh = $('#fromWh').val();
    var fromwhname = $('#fromWh option:selected').text();
    if (fromwh == "0" || fromwh == "" || fromwh == null) {
        $("[aria-labelledby='select2-fromWh-container']").css("border-color", "red");
        $("#Val_from_wh").text($("#valueReq").text());
        $("#Val_from_wh").css("display", "block");
        //document.getElementById("vmwarehouse").innerHTML = $("#valueReq").text();
        //$("#fromWh").css("border-color", "red");      
        if (towh == "0" || towh == "" || towh == null) {
            $("#MTOItmDetailTbl .plus_icon1").css("display", "none");
        }
    }
    else {
        BindDestinationDDL()
        //document.getElementById("vmwarehouse").innerHTML = null;
        //$("#fromWh").css("border-color", "#ced4da");
        $("#Val_from_wh").css("display", "none");
        $("[aria-labelledby='select2-fromWh-container']").css("border-color", "#ced4da");
        var transtyp = $("#hdtrfType").val();
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
        var towh = $('#toWh').val();
        if (towh == "0" || towh == "" || towh == null) {
            $("[aria-labelledby='select2-toWh-container']").css("border-color", "red");
            $("#VAL_to_wh").text($("#valueReq").text());
            $("#VAL_to_wh").css("display", "block");
        }
        else {
            $("#VAL_to_wh").css("display", "none");
            $("[aria-labelledby='select2-toWh-container']").css("border-color", "#ced4da");
        }
        //$("#toWh option[value=" + fromwh + "]").select2().hide();
       //$("#toWh option[value=" + $('#fromWh').val() + "]").hide();
      //var d = "<option value='" + fromwh + "'>" + fromwhname + "</option>"
        //$("#toWh").remove(d);
        //$('#toWh').remove("<option value='" + fromwh + "'>" + fromwhname + "</option>");
     }
}
function BindSrcWHList(branch) {
    debugger;
    let DocumentMenuId = $("#DocumentMenuId").val();
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/MaterialTransferOrder/GetToWHList1",
            data: {
                SrcBrId: branch,
                DocumentMenuId: DocumentMenuId
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    GRN_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.length > 0) {
                        
                        var s = '<option value="0">---Select---</option>';
                        
                            for (var i = 0; i < arr.length; i++) {
                              //var wh_id=  arr[i].wh_id
                              //  if (fromwh == wh_id) {

                              //  }
                              //  else {
                                   s += '<option value="' + arr[i].wh_id + '">' + arr[i].wh_name + '</option>';
                               /* }*/
                            
                            }
                        $("#fromWh").html(s);
                        $("#fromWh").select2()
                        $("#Val_from_wh").css("display", "none");
                        $("[aria-labelledby='select2-fromWh-container']").css("border-color", "#ced4da");

                        $("#VAL_to_wh").css("display", "none");
                        $("[aria-labelledby='select2-toWh-container']").css("border-color", "#ced4da");

                        $("#vmbranch").css("display", "none");
                        $("#Fr_branch").css("border-color", "#ced4da");
                    }
                }
            },
        });
}

function OnChangeToWh() {
    debugger;
    var towh = $('#toWh').val();
    var fromWh = $('#fromWh').val();
    var towhname = $('#toWh option:selected').text();
    if (towh == "0" || towh == "" || towh == null) {
        $("[aria-labelledby='select2-toWh-container']").css("border-color", "red");
        $("#VAL_to_wh").text($("#valueReq").text());
        $("#VAL_to_wh").css("display", "block");
        //document.getElementById("vmtowarehouse").innerHTML = $("#valueReq").text();
        //$("#toWh").css("border-color", "red");    
        if (fromWh == "0" || fromWh == "" || fromWh == null) {
            $("#MTOItmDetailTbl .plus_icon1").css("display", "none");
        }   
    }
    else {
        $("#hdWhID").val(towh);
        //$('#hdWhID').val(towh);
        $('#hdWhName').val(towhname);
        
        $("#VAL_to_wh").css("display", "none");
        $("[aria-labelledby='select2-toWh-container']").css("border-color", "#ced4da");
        //document.getElementById("vmtowarehouse").innerHTML = null;
        //$("#toWh").css("border-color", "#ced4da");
        if (fromWh != "0" || fromWh != "" || fromWh != null) {
            $("#MTOItmDetailTbl .plus_icon1").css("display", "block");
        }
       
        //if ($("#MTOItmDetailTbl tbody tr").length == 1) {
        //    $("#Itemname_1").attr("disabled", false)
        //    $("#RequisitionQuantity").attr("disabled", false)
        //}
    }
}
function BindItemList_mto(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Itm_ID = clickedrow.find("#Itemname_" + SNo).val();
    var ItemID = clickedrow.find("#hfItemID").val();
    Cmn_DeleteSubItemQtyDetail(ItemID);
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
    //HideSelectedItem("#Itemname_", SNo, "#MTOItmDetailTbl", "#SNohf");
    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "");

    var Sno = clickedrow.find("#SNohf").val();
    let ItemId = clickedrow.find("#Itemname_" + Sno).val();
    let UomId = clickedrow.find("#UOMID").val();
    let WarehouseId = $("#fromWh").val();
    let Fr_branch = $("#Fr_branch").val();
    let DocumentMenuId = $("#DocumentMenuId").val();

    $.ajax({
        type: "Post",
        url: "/Common/Common/getWarehouseWiseItemStock",
        data: {
            ItemId: ItemId,
            WarehouseId: WarehouseId,
            UomId: null,
            br_id: Fr_branch,
            DocumentMenuId: DocumentMenuId,
        },
        success: function (data) {
            var avaiableStock = JSON.parse(data);
            var parseavaiableStock = parseFloat(avaiableStock).toFixed(QtyDecDigit)
            clickedrow.find("#AvailableStockInBase").val(parseavaiableStock);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
        }
    });

    //Cmn_StockUomWise(ItemId, UomId).then((res) => {
    //    clickedrow.find("#AvailableStockInBase").val(res);
    //    //clickedrow.find("#UOMID").val(UomId);
    //    clickedrow.find("#AvailableStockInBase").val(parseFloat(0).toFixed(QtyDecDigit));
    //}).catch(err => console.log(err.message));
}
//function GetSubItemAvlStock(e) {
//    var Crow = $(e.target).closest('tr');
//    var rowNo = Crow.find("#SNohf").val();
//    var ProductNm = Crow.find("#Itemname_" + rowNo + " option:selected").text();
//    var ProductId = Crow.find("#Itemname_" + rowNo + " option:selected").val();
//    var UOMId = Crow.find("#UOMID").val();
//    var UOM = Crow.find("#UOM option:selected").text();
//    var AvlStk = Crow.find("#AvailableStockInBase").val();
//    var br_id = $("#Fr_branch").val();
//    var wh_id = $("#fromWh").val();
//    Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, wh_id, AvlStk, "br", UOMId, br_id);
//}
//////////////////////////////////////////////////////////////////////////////////////////////////////////
function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var TRFNo = clickedrow.children("#TRFNo").text();
    var TRFDate = clickedrow.children("#TRFDate").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(TRFNo);
    GetWorkFlowDetails(TRFNo, TRFDate, Doc_id);
}
function ForwardBtnClick() {
    debugger;
    //var DNStatus = "";
    //DNStatus = $('#hdMTRStatus').val().trim();
    //if (DNStatus === "D" || DNStatus === "F") {

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

    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var MTODt = $("#txtTrfDate").val();
    $.ajax({
        type: "POST",
        /* url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: MTODt
        },
        success: function (data) {
            /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var DNStatus = "";
                DNStatus = $('#hdMTRStatus').val().trim();
                if (DNStatus === "D" || DNStatus === "F") {

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
 function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var TRFNumber = "";
    var txtTrfDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    //var TransferType = "";
    var TRF_type = "";
    var WF_status1 = "";
    var DashBord = "";
    //var SourcDocNo = "";
    //var SourcDocDt = "";

    docid = $("#DocumentMenuId").val();
    TRFNumber = $("#TRFNumber").val();
    txtTrfDate = $("#txtTrfDate").val();
    WF_status1 = $("#WF_status1").val();
    DashBord = (docid + ',' + TRFNumber + ',' + txtTrfDate + ',' + WF_status1);
    $("#hdDoc_No").val(TRFNumber);
    Remarks = $("#fw_remarks").val();
    //TransferType = $("#TransferType").val();
    TRF_type = $("#TransferType").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    //SourcDocNo = $("#ddlPRNumberList").val();
    //SourcDocDt = $("#txtPRDate").val();


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
        if (fwchkval != "" && TRFNumber != "" && txtTrfDate != "" && level != "") {
            debugger;
           Cmn_InsertDocument_ForwardedDetail(TRFNumber, txtTrfDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/MaterialTransferOrder/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&DashBord=" + DashBord;
        }
        }
    
    if (fwchkval === "Approve") {
        var list = [{ trf_no: TRFNumber, create_dt: txtTrfDate, trf_type: TRF_type, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/MaterialTransferOrder/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&docid=" + docid;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/MaterialTransferOrder/MTOApprove?trf_no=" + TRFNumber + "&trf_dt=" + txtTrfDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && TRFNumber != "" && txtTrfDate != "") {
            Cmn_InsertDocument_ForwardedDetail(TRFNumber, txtTrfDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/MaterialTransferOrder/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&DashBord=" + DashBord;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && TRFNumber != "" && txtTrfDate != "") {
            Cmn_InsertDocument_ForwardedDetail(TRFNumber, txtTrfDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/MaterialTransferOrder/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&DashBord=" + DashBord;
        }
    }

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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#TRFNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
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
            url: "/ApplicationLayer/MaterialTransferOrder/getListOfItems",
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
    var rowCount = $('#MTOItmDetailTbl >tbody >tr').length + 1
    var RowNo = 0;
   // onMRSItemName();
    $("#MTOItmDetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        //var Sno = currentRow.find("#SNohf").val();
        //if (rowCount == Sno) {
            RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
        //}
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    if (rowCount > 0) {
        $("#TransferType").attr("disabled", true)
        $("#fromWh").attr("disabled", true)
        $("#toWh").attr("disabled", true)
        if ($("#TransferType").val() == "B") {
            $("#Fr_branch").attr("disabled", true)
        }
    } 
    var span_SubItemDetail = $("#span_SubItemDetail").val();

    $('#MTOItmDetailTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td><div class="col-sm-11 lpo_form no-padding"><select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" onchange ="OnChangeItemName(${RowNo},event)"></select> <input  type="hidden" id="hfItemID" value="" /><span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"> <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button></div>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" /></td>
<td><div class="col-sm-10 lpo_form no-padding"> <input id="AvailableStockInBase" class="form-control num_right" autocomplete="off" type="text" name="AvailableStockInBase" placeholder="0000.00" readonly="">
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
<button type="button" id="SubItemAvlQty" disabled="" class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock1(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="Sub-Item Detail"> </button>
</div>
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="RequisitionQuantity" onkeypress="return ReqQtyFloatValueonly(this,event);" onchange ="OnclickReqQty(event)" class="form-control num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  ><span id="RequisitionQuantity_Error" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemReqQty">
    <input hidden="" type="text" id="sub_item" value="">
    <button type="button" id="SubItemReqQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
</div>
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="IssuedQuantity" class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" required="required" placeholder="0000.00"  disabled="">
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemIssuedQty">
    <button type="button" id="SubItemIssuedQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('Issued',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
</div>
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="ReceivedQuantity" class="form-control num_right" autocomplete="off" onchange ="OnChangePOItemQty(,event)" onkeypress="return QtyFloatValueonly(this,event);" type="text" name="ord_qty_spec" placeholder="0000.00" disabled></span>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemRecvdQty">
<button type="button" id="SubItemRecvdQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('Received',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
</div>
</td>
<td><textarea id="ItemRemarks" class="form-control remarksmessage" autocomplete="off" type="text" name="ItemRemarks" @maxlength = "100",  placeholder="${$("#span_remarks").text()}" ></textarea></td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);

    BindItmList(RowNo,"new");
}
function OnChangeItemName(RowID, e) {
    debugger;
    BindItemList_mto(e);
    //var Sno = clickedrow.find("#SNohf").val();
    //let ItemId = Crow.find("#Itemname_" + Sno).val();
    //let UomId = Crow.find("#UOMID").val();
    //await Cmn_StockUomWise(ItemId, UomId).then((res) => {
    //    Crow.find("#AvailableStockInBase").val(res);
    //    Crow.find("#UOMID").val(UomId);

    //    if (ItemType == "Consumable") {
    //        Crow.find("#AvailableStockInBase").val(parseFloat(0).toFixed(QtyDecDigit));
    //    }
    //}).catch(err => console.log(err.message));
}
function BindItmList(ID,type) {
    debugger;
    //BindItemList("#Itemname_", ID, "#MTOItmDetailTbl", "#SNohf", "", "MTO");
    //DynamicSerchableItemDDL("#MTOItmDetailTbl", "#Itemname_", ID, "#SNohf", "", "MTO")
    if (type == "new") {
        $("#Itemname_" + ID).append("<option value='0' selected>---Select---</option>");
    }
    else {
        $("#Itemname_" + ID).append("<option value='0'>---Select---</option>");
    }
    $("#Itemname_" + ID).select2({

        ajax: {
            url: "/ApplicationLayer/MaterialTransferOrder/getListOfItems",
            data: function (params) {
                var queryParameters = {
                    MTO_ItemName: params.term,
                    //PageName: PageName,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 20;//00; /*Commented by Suraj On 12-12-2022*/
                let array = [];
                $("#MTOItmDetailTbl > tbody > tr").each(function () {
                    var currentRow = $(this);
                    debugger;
                    var rowno = currentRow.find("#SNohf").val();
                    var itemId = currentRow.find("#Itemname_" + rowno).val();
                    if (itemId != "0") {
                        array.push({ id: itemId });
                    }

                });
                var ItemListArrey = JSON.stringify(array);

                let selected = [];
                selected.push({ id: $("#Itemname_" + ID).val() });
                selected = JSON.stringify(selected);

                var NewArrey = JSON.parse(ItemListArrey).filter(i => !selected.includes(i.id))
                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
<div class="row"><div class="col-md-8 col-xs-6 def-cursor">Item Name</div>
<div class="col-md-4 col-xs-6 def-cursor">UOM</div></div>
</strong></li></ul>`)
                }
                var page = params.page || 1;
                var Fdata = data.slice((page - 1) * pageSize, page * pageSize)
                if (data[0] != null) {
                    if (page == 1) {
                        if (data[0].Name.trim() != "---Select---") {
                            var select = { ID: "0_0", Name: " ---Select---" };
                            data.unshift(select);
                        }
                    }
                  
                }
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
                    }),
                    //results: data.slice((page - 1) * pageSize, page * pageSize),
                    //more: data.length >= page * pageSize,
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
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
                    '<div class="col-md-8 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                    '<div class="col-md-4 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                    '</div>'
                );
            
            return $result;
           
            firstEmptySelect = false;
        },

    });

    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/MaterialTransferOrder/getListOfItems",
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
    //                            if (check(data, selected, "#MTOItmDetailTbl", "#SNohf","#Itemname_") == true) {
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
    //                            firstEmptySelect = false;
    //                        }
    //                    });
    //                    debugger;
    //              //      HideItemListItm(ID);
    //                }
    //            }
    //        },
    //    });
}
//function HideItemListItm(ID) {
//    $("#MTOItmDetailTbl >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        var ItmID = currentRow.find("#hfItemID").val();
//        var rowid = currentRow.find("#SNohf").val();

//        var ItemCode;
//        ItemCode = ItmID;

//        if (ItemCode != '0' && ItemCode != "") {
//            $("#MTOItmDetailTbl >tbody >tr").each(function (i, row) {
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
//            $("#MTOItmDetailTbl >tbody >tr").each(function (i, row) {
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
    //BindItemList("#Itemname_", "1", "#MTOItmDetailTbl", "#SNohf", "BindData","MTO");
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/MaterialTransferOrder/getListOfItems",
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
                        sessionStorage.removeItem("itemList");
                        sessionStorage.setItem("itemList", JSON.stringify(arr.Table));

                        //$('#Itemname_1').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                        //for (var i = 0; i < arr.Table.length; i++) {
                        //    $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
                        //}
                        //var firstEmptySelect = true;
                        //$('#Itemname_1').select2({
                        //    templateResult: function (data) {
                        //        var UOM = $(data.element).data('uom');
                        //        var classAttr = $(data.element).attr('class');
                        //        var hasClass = typeof classAttr != 'undefined';
                        //        classAttr = hasClass ? ' ' + classAttr : '';
                        //        var $result = $(
                        //            '<div class="row">' +
                        //            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                        //            '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
                        //            '</div>'
                        //        );
                        //        return $result;
                        //        firstEmptySelect = false;
                        //    }
                        //});

                      //  BindData();
                    }
                    
                }
            },
        });
}
//function BindData() {
//    debugger;

//    var PLItemListData = JSON.parse(sessionStorage.getItem("itemList"));
//    if (PLItemListData != null) {
//        if (PLItemListData.length > 0) {
//            $("#MTOItmDetailTbl >tbody >tr").each(function () {
//                //debugger;
//                var currentRow = $(this);
//                var rowid = currentRow.find("#SNohf").val();
//                //rowid = parseFloat(rowid) + 1;
//                //if (rowid > $("#MTOItmDetailTbl >tbody >tr").length) {
//                //    return false;
//                //}
//                $("#Itemname_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#SOItemName").text()}' data-uom='${$("#SOItemUOM").text()}'></optgroup>`);
//                for (var i = 0; i < PLItemListData.length; i++) {
//                    $('#Textddl' + rowid).append(`<option data-uom="${PLItemListData[i].uom_name}" value="${PLItemListData[i].Item_id}">${PLItemListData[i].Item_name}</option>`);
//                }
//                var firstEmptySelect = true;
//                $("#Itemname_" + rowid).select2({
//                    templateResult: function (data) {
//                        var selected = $("#Itemname_" + rowid).val();
//                        if (check(data, selected, "#MTOItmDetailTbl", "#SNohf", "#Itemname_") == true) {
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
//                        }
//                        firstEmptySelect = false;
//                    }
//                });

//            });
//        }
//    }

//    $("#MTOItmDetailTbl >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        var ItmID = currentRow.find("#hfItemID").val();
//        var rowid = currentRow.find("#SNohf").val();
//        var ItemID = '#Itemname_' + rowid;
//        if (ItmID != '0' && ItmID != "") {
//            currentRow.find("#Itemname_" + rowid).val(ItmID).trigger('change.select2');
//        }

//    });

//    //$("#MTOItmDetailTbl >tbody >tr").each(function (i, row) {
//    //    debugger;
//    //    var currentRow = $(this);
//    //    var ItmID = currentRow.find("#hfItemID").val();
//    //    var rowid = currentRow.find("#SNohf").val();

//    //    var ItemCode;
//    //    ItemCode = ItmID;

//    //    if (ItemCode != '0' && ItemCode != "") {

//    //        $("#MTOItmDetailTbl >tbody >tr").each(function (i, row) {
//    //            debugger;
//    //            var currentRowD = $(this);
//    //            var ItemCodeD;
//    //            ItemCodeD = currentRowD.find("#hfItemID").val();
//    //            var rowidD = currentRowD.find("#SNohf").val();
//    //            if (ItemCodeD != '0' && ItemCodeD != "") {
//    //                if (currentRow.find("#Itemname_" + rowidD).val() != ItemCode) {
//    //                    $("#Itemname_" + rowid + " option[value=" + ItemCodeD + "]").select2().hide();
//    //                }
//    //            }
//    //        })
//    //    }
//    //});
//}
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
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#MTOItmDetailTbl >tbody >tr").each(function (i, row) {
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
function InsertMTODetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");

    if (CheckMTOFormValidation() == false) {
        return false;
    }
    if (CheckMTOItemValidations() == false) {
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
  
    FinalItemDetail = InsertMTOItemDetails();

    /*-----------Sub-item-------------*/

    var SubItemsListArr = Cmn_SubItemList();
    var str2 = JSON.stringify(SubItemsListArr);
    $('#SubItemDetailsDt').val(str2);

    /*-----------Sub-item end-------------*/

    var ItemDt = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(ItemDt);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");

    $("#TransferType").attr("disabled", false)
    $("#fromWh").attr("disabled", false)
    $("#toWh").attr("disabled", false)
    $("#Fr_branch").attr("disabled", false)
    return true;
};
function CheckMTOFormValidation() {

    debugger;
    var rowcount = $('#MTOItmDetailTbl tr').length;
    var ValidationFlag = true;
    var Flag = 'N';

    var TransferType = $('#TransferType').val();
    if (TransferType == "" || TransferType == "0") {
        document.getElementById("vmTransferType").innerHTML = $("#valueReq").text();
        $("#TransferType").css("border-color", "red");
        Flag = 'Y';
    }
    var fromwh = $('#fromWh').val();
    if (fromwh == "0" || fromwh == "") {
        //document.getElementById("vmwarehouse").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-fromWh-container']").css("border-color", "red");
        $("#Val_from_wh").text($("#valueReq").text());
        $("#Val_from_wh").css("display", "block");
        //$("#fromWh").css("border-color", "red");
        Flag = 'Y';     
    }
    else {
        document.getElementById("vmwarehouse").innerHTML = null;
        $("#fromWh").css("border-color", "#ced4da");
    }
    var towh = $('#toWh').val();
    if (towh == "0" || towh == "") {
        //document.getElementById("vmtowarehouse").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-toWh-container']").css("border-color", "red");
        $("#VAL_to_wh").text($("#valueReq").text());
        $("#VAL_to_wh").css("display", "block");
        //$("#toWh").css("border-color", "red");
        Flag = 'Y';
    }
    else {
        document.getElementById("vmtowarehouse").innerHTML = null;
        $("#toWh").css("border-color", "#ced4da");
    }

    var TransferType = $("#TransferType").val();
    if (TransferType == "B" || TransferType == "0") {
        var ToBranch = $("#Fr_branch").val();
        if (ToBranch == "" || ToBranch == "0") {
            $("#vmbranch").text($("#valueReq").text());
            $("#vmbranch").css("display", "block");        
            $("#Fr_branch").css("border-color", "red");
            Flag = 'Y';
        }
        else {
            $("#vmbranch").css("display", "none");      
            $("#Fr_branch").css("border-color", "#ced4da");
            $("#vmbranch").text("");
        }
    }
    debugger;
    var TransferType = $("#TransferType").val();
    if (TransferType == "B") {
        var ToBranch = $("#tobranch").val();
        var FromBranch = $("#hfbr_ID").val();
        if (ToBranch == FromBranch) {
            $("#vmbranch").text($("#Branchcannotsame").text());
            $("#vmbranch").css("display", "block");
            $("#tobranch").css("border-color", "red");
            Flag = 'Y';
        }
        else {
            $("#vmbranch").css("display", "none");
            $("#tobranch").css("border-color", "#ced4da");
            $("#vmbranch").text("");
        }
    }
    debugger;
    var TransferType = $("#TransferType").val();
    var fromwh = $('#fromWh').val();
    var towh = $('#toWh').val();
    if (TransferType == "W") {      
        if (fromwh == towh) {
            document.getElementById("vmtowarehouse").innerHTML =  $("#Warehosecannotsame").text();
            $("#toWh").css("border-color", "red");
            Flag = 'Y';
        }
        else {
            document.getElementById("vmtowarehouse").innerHTML = null;
            $("#toWh").css("border-color", "ced4da");           
        }
    }
    if (Flag == 'Y') {
        ValidationFlag = false;
    }
    else {
        ValidationFlag = true;
    }

    if (ValidationFlag == true) {
        if (CheckMTOItemValidations() == false) {
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
function CheckMTOItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    $("#MTOItmDetailTbl >tbody >tr").each(function () {
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
    if (AvoidDot(ReqQty) == false) {     
        clickedrow.find("#RequisitionQuantity").val("");
        ReqQty=""
    }
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
function ReqQtyFloatValueonly(el, evt) {
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
function InsertMTOItemDetails() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ItemDetailList = new Array();
    $("#MTOItmDetailTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var ItemList = {};
        ItemList.ItemID = currentRow.find("#Itemname_" + rowid).val();
        ItemList.UOMID = currentRow.find("#UOMID").val();
        ItemList.RequQty = parseFloat(currentRow.find("#RequisitionQuantity").val()).toFixed(QtyDecDigit);
        ItemList.ItemRemarks = currentRow.find('#ItemRemarks').val();

        ItemDetailList.push(ItemList);
        debugger;
    });

    return ItemDetailList;
};


/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemReqQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemIssuedQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemRecvdQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlQty");
    
}
function SubItemDetailsPopUp(flag, e) {
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohf").val();
    var ProductNm = clickdRow.find("#Itemname_" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#Itemname_" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var Doc_no = $("#TRFNumber").val();
    var Doc_dt = $("#txtTrfDate").val();
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
    } else if (flag == "Issued") {
        Sub_Quantity = clickdRow.find("#IssuedQuantity").val();
    } else if (flag == "Received") {
        Sub_Quantity = clickdRow.find("#ReceivedQuantity").val();
    }

    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hdMTRStatus").val();


    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialTransferOrder/GetSubItemDetails",
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
    return Cmn_CheckValidations_forSubItems("MTOItmDetailTbl", "SNohf", "Itemname_", "RequisitionQuantity", "SubItemReqQty", "Y");
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("MTOItmDetailTbl", "SNohf", "Itemname_", "RequisitionQuantity", "SubItemReqQty", "N");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

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
    Cmn_FilterTableData(e, "MTOItmDetailTbl", [{ "FieldId": "Itemname_", "FieldType": "select", "SrNo": "SNohf" }]);
}

