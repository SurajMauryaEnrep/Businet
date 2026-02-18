/************************************************
Javascript Name:Stock Reserve Detail
Created By:Mukesh
Created Date: 05-04-2022
Description: This Javascript use for the Stock Reserve many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    BindItemName();    
    //BindCustomerList();
    ListRowHighLight();
    $("#toWh").select2();
    $("#ddlCustomerName").select2();
    $("#Rev_UnrevDetails").attr("disabled","disabled");
});
var QtyDecDigit = $("#QtyDigit").text();///Quantity
function BindItemName() {
    debugger;
    var DocModify = $("#DocModify").val();
    if (DocModify == "DocModify") {

    }
    else {
        BindItemList("#ddl_ProductName", "", "", "", "", "StkRes")
    }
}

//function BindCustomerList() {
//    debugger;
//    $("#ddlCustomerName").select2({
//        ajax: {
//            url: $("#CustNameList").val(),
//            data: function (params) {
//                var queryParameters = {
//                    SO_CustName: params.term // search term like "a" then "an"
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    ErrorPage();
//                    return false;
//                }
//                params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });
//}
function OnchangeItem(e) {
    debugger;
    var Itm_ID = $("#ddl_ProductName").val();
    if (Itm_ID != "0") {
        $("#vmddl_ProductName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");
    }
    else {
        $('#vmddl_ProductName').text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
    }

    if (Itm_ID != null) {
        $("#hdn_product_id").val(Itm_ID);
    }
    else if (Itm_ID == null) {
        Itm_ID = $("#hdn_product_id").val();
    }
    try {
       
        Cmn_BindUOM(e, Itm_ID, "", "", "");

        $('#hdres_unresdetails').val("");
        SetDefault_hideErrormsg();
        //$.ajax({
        //    success: function () {
        //        var QtyDecDigit = $("#QtyDigit").text();
        //        var hdn_i_serial = $("#hdn_i_serial").val();
        //        var hdn_i_batch = $("#hdn_i_batch").val();

        //        if (hdn_i_serial == "Y") {
        //            $("#SerialDt").css('display', 'block');
        //            $("#BatchDt").css('display', 'none');
        //        }
        //        else if (hdn_i_batch == "Y") {
        //            $("#BatchDt").css('display', 'block');
        //            $("#SerialDt").css('display', 'none');
        //        }
        //        else if (hdn_i_batch == "N" && hdn_i_serial == "N") {
        //            $("#BatchDt").css('display', 'none');
        //            $("#SerialDt").css('display', 'none');
        //        }
        //        debugger;
        //        $("#toWh").val("0");
        //        $("#ReservedStock").val(parseFloat(0).toFixed(QtyDecDigit));
        //        $("#TotalStock").val(parseFloat(0).toFixed(QtyDecDigit));
        //        $("#AvailableStock").val(parseFloat(0).toFixed(QtyDecDigit));
        //        $("#ddlCustomerName").val("0").trigger('change');
        //    }
        //});

    } catch (err) {
    }
  
}
function OnChangeCustomer() {
    debugger;
    var customerName = $('#ddlCustomerName option:selected').val();
    if (customerName != "0") {
        document.getElementById("vmcust_id").innerHTML = null;
        $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmcust_id").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "red");
        $("#vmcust_id").css("display", "block");
    }
    debugger;
    $('#hdres_unresdetails').val("");
    BindSONumberList();
}
function BindSONumberList() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var EntityID = $('#ddlCustomerName').val();
    var Itm_ID = $("#ddl_ProductName").val();
    var wh_id = $("#toWh").val();
    var TransType = $("#TransType").val();

    if (EntityID != null && EntityID != "" && Itm_ID != null && Itm_ID != "") {
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/StockReservation/GetDocumentNo",
                data: { Entity_id: EntityID, Itm_ID: Itm_ID, wh_id: wh_id, Type: TransType },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#ddlOrderNumber option").remove();
                            $("#ddlOrderNumber optgroup").remove();
                            $('#ddlOrderNumber').append(`<optgroup class='def-cursor' id="SOTextddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                            for (var i = 0; i < arr.Table.length; i++) {
                                if ($("#ddlOrderNumber option[value='" + arr.Table[i].app_so_no + "']").length == 0) {
                                    $('#SOTextddl').append(`<option data-date="${arr.Table[i].so_dt}" value="${arr.Table[i].app_so_no}">${arr.Table[i].app_so_no}</option>`);
                                }
                                
                            }
                            var firstEmptySelect = true;
                            $('#ddlOrderNumber').select2({
                                templateResult: function (data) {
                                    var PDRows = $("#SaveItemOrderQtyDetails >tbody > tr td #PD_OrderQtyDocNo[value='" + data.id + "']").val();
                                    if (data.id != PDRows) {
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
                            document.getElementById("vmdoc_no").innerHTML = null;
                            $("#ddlOrderNumber").css("border-color", "#ced4da");
                            $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
                            $("#txtdoc_dt").val(null);

                            $("#DocQty").val(parseFloat(0).toFixed(QtyDecDigit));
                            $("#PendQty").val(parseFloat(0).toFixed(QtyDecDigit));
                            $("#UnResQty").val(parseFloat(0).toFixed(QtyDecDigit));
                        }
                    }
                },
            });
        } catch (err) {
            console.log("GetMenuData Error : " + err.message);
        }
    }
}
function ddlSONumberSelect() {
    debugger;
    var SourceDocumentDate = $('#ddlOrderNumber').val();
    if (SourceDocumentDate != null && SourceDocumentDate != "") {
        var doc_Date = $("#ddlOrderNumber option:selected")[0].dataset.date;
        var date = doc_Date.split("-");
        var FDate = date[2] + '-' + date[1] + '-' + date[0];
        $("#txtdoc_dt").val(FDate);
        OnchangeDocNo();
    }
    var DocumentNumber = $('#ddlOrderNumber').val();
    if (DocumentNumber != "---Select---") {
        document.getElementById("vmdoc_no").innerHTML = null;
        document.getElementById("vmsrc_doc_no").innerHTML = null;
        $("#ddlOrderNumber").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
    }   
}
function OnchangeWarehouse() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var Itm_ID = $("#ddl_ProductName").val();
    var Wh_ID = $("#toWh").val();
    $("#ReservedStock").val(parseFloat(0).toFixed(QtyDecDigit));
    $("#TotalStock").val(parseFloat(0).toFixed(QtyDecDigit));
    $("#AvailableStock").val(parseFloat(0).toFixed(QtyDecDigit));   
   
    if (Wh_ID == "0" || Wh_ID == "") {
        $("[aria-labelledby='select2-toWh-container']").css("border-color", "red");
        $("#Val_warehouse_id").text($("#valueReq").text());
        $("#Val_warehouse_id").css("display", "block");
        //document.getElementById("vmtowarehouse").innerHTML = $("#valueReq").text();
        //$("#toWh").css("border-color", "red");       
    }
    else {
        $("#Val_warehouse_id").css("display", "none");
        $("[aria-labelledby='select2-toWh-container']").css("border-color", "#ced4da");
        //document.getElementById("vmtowarehouse").innerHTML = null;
        //$("#toWh").css("border-color", "#ced4da");
    }
    var ddltype = $("#TransType").val();
    $('#hdres_unresdetails').val("");
    if (Itm_ID != null && Itm_ID != "" && Wh_ID != null && Wh_ID != "") {
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/StockReservation/GetStock",
                data: { item_id: Itm_ID, wh_id: Wh_ID},
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#ReservedStock").val(parseFloat(arr.Table[0].res_stk).toFixed(QtyDecDigit));
                            $("#TotalStock").val(parseFloat(arr.Table[0].total_stk).toFixed(QtyDecDigit));
                            $("#AvailableStock").val(parseFloat(arr.Table[0].aval_stk).toFixed(QtyDecDigit));                           
                        }
                       
                        $("#ddlOrderNumber").attr("disabled", false);                   
                        //$("#ddlCustomerName").val("0").trigger('change');
                        if (ddltype == "U") {
                            $("#Rev_UnrevDetails").removeAttr("disabled");
                        }
                    }
                },
            });
        } catch (err) {
            console.log("GetMenuData Error : " + err.message);
        }
    }

    $("#ddlCustomerName").val("0").trigger('change');
    document.getElementById("vmcust_id").innerHTML = null;
    $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "#ced4da");
}
function OnchangeDocNo() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var Itm_ID = $("#ddl_ProductName").val();
    var Docno = $('#ddlOrderNumber').val();
    var WhID = $('#toWh').val();

    $("#DocQty").val(parseFloat(0).toFixed(QtyDecDigit));
    $("#PendQty").val(parseFloat(0).toFixed(QtyDecDigit));
    $("#UnResQty").val(parseFloat(0).toFixed(QtyDecDigit));
    if (Itm_ID != null && Itm_ID != "" && Docno != null && Docno != "") {
        $('#hdres_unresdetails').val("");
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/StockReservation/GetDocdetail",
                data: { wh_id: WhID,item_id: Itm_ID, Docno: Docno },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        $("#Rev_UnrevDetails").removeAttr("disabled");
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#DocQty").val(parseFloat(arr.Table[0].ord_qty_base).toFixed(QtyDecDigit));
                            $("#PendQty").val(parseFloat(arr.Table[0].pending_qty).toFixed(QtyDecDigit));
                            $("#UnResQty").val(parseFloat(arr.Table[0].unres_qty).toFixed(QtyDecDigit));

                            //$("#LotNo").attr("Disabled", false)
                            //$("#BatchNo").attr("Disabled", false)
                            //$("#SerialNo").attr("Disabled", false)
                        }
                    }
                },
            });
        } catch (err) {
            console.log("GetMenuData Error : " + err.message);
        }
    }
}

function OnChangeResQty() {
    debugger;
    var ReservedQuantity = $('#ReservedQuantity').val().trim();
    if (ReservedQuantity > 0) {
        document.getElementById("vmReservedQuantity").innerHTML = "";
        $("#ReservedQuantity").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmReservedQuantity").innerHTML = $("#valueReq").text();
        $("#ReservedQuantity").css("border-color", "red");
    }
}
function AmtFloatQtyonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function OnclickResQty(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ReservedQuantity = $("#ReservedQuantity").val();   
    //var BtSrAvalStock = $("#BtSrAvalStock").val(); 
    //var UnResQty = $("#UnResQty").val(); 
   
    if (ReservedQuantity == 0) {
        $("#vmReservedQuantity").text($("#valueReq").text());
        $("#vmReservedQuantity").css("display", "block");
        $("#ReservedQuantity").css("border-color", "red")
        $("#ReservedQuantity").val('');
        return false;
    }
    else {
        $("#vmReservedQuantity").css("display", "none");
        $("#ReservedQuantity").css("border-color", "#ced4da");
    }
    if (isNaN(parseFloat(ReservedQuantity).toFixed(QtyDecDigit))) {
        $("#ReservedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
    }
    else {
        $("#ReservedQuantity").val(parseFloat(ReservedQuantity).toFixed(QtyDecDigit));
    }
    //debugger;
    //if (parseFloat(ReservedQuantity) > parseFloat(BtSrAvalStock)) {
    //    $("#vmReservedQuantity").text($("#ExceedingQty").text());
    //    $("#vmReservedQuantity").css("display", "block");
    //    $("#ReservedQuantity").css("border-color", "red")
    //}
    //else {
    //    if (parseFloat(ReservedQuantity) > parseFloat(UnResQty)) {
    //        $("#vmReservedQuantity").text($("#ExceedingQty").text());
    //        $("#vmReservedQuantity").css("display", "block");
    //        $("#ReservedQuantity").css("border-color", "red")
    //    }
    //    else {
    //        $("#vmReservedQuantity").css("display", "none");
    //        $("#ReservedQuantity").css("border-color", "#ced4da");
    //    }
    //}
}
function CheckStkResFormValidation() {
   
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }


    var ValidationFlag = true;
    var Flag = 'N';
    var TransType = $("#TransType").val();

    var UOMName = $('#UOM').val();
    var ItemName = $('#ddl_ProductName option:selected').text();
    var ItemId = $('#ddl_ProductName option:selected').val();
    var DocNo = $('#ddlOrderNumber option:selected').text();
    var DocDt = $('#txtdoc_dt').val();
    var CustName = $('#ddlCustomerName option:selected').text();
    var WhName = $('#toWh option:selected').text();

    $("#UOMName").val(UOMName);
    $("#hdn_product_name").val(ItemName);
    $("#hdn_product_id").val(ItemId);
    $("#hd_so_no").val(DocNo);
    $("#hddoc_dt").val(DocDt);
    $("#CustNam").val(CustName);
    $("#warehouse_Name").val(WhName);

    var Itm_ID = $("#ddl_ProductName").val();
    if (Itm_ID != "0") {
        $("#vmddl_ProductName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");
    }
    else {
        $('#vmddl_ProductName').text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
        Flag = 'Y';
    }   
    var Wh_ID = $("#toWh").val();
    if (Wh_ID == "0" || Wh_ID == "") {
        $("[aria-labelledby='select2-toWh-container']").css("border-color", "red");
        $("#Val_warehouse_id").text($("#valueReq").text());
        $("#Val_warehouse_id").css("display", "block");
        //document.getElementById("vmtowarehouse").innerHTML = $("#valueReq").text();
        //$("#toWh").css("border-color", "red");
        Flag = 'Y';
    }
    else {
        document.getElementById("vmtowarehouse").innerHTML = null;
        $("#toWh").css("border-color", "#ced4da");
    }

    var totalres_unresqty = $('#hdtotalres_unresqty').val()
    if (totalres_unresqty == "" || totalres_unresqty == null) {
        totalres_unresqty = 0;
    }
    if (TransType == "R") {
        var customerName = $('#ddlCustomerName option:selected').val();
        if (customerName != "0") {
            document.getElementById("vmcust_id").innerHTML = null;
            $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "#ced4da");
        }
        else {
            document.getElementById("vmcust_id").innerHTML = $("#valueReq").text();
            $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "red");
            $("#vmcust_id").css("display", "block");
            Flag = 'Y';
        }
        var OrderNumber = $('#ddlOrderNumber option:selected').val();
        if (OrderNumber != "0" && OrderNumber != "---Select---") {
            document.getElementById("vmsrc_doc_no").innerHTML = null;
            document.getElementById("vmdoc_no").innerHTML = null;
            $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
            $("#ddlOrderNumber").css("border-color", "#ced4da");
        }
        else {
            document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
            $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "red");
            $("#ddlOrderNumber").css("border-color", "red");
            $("#vmsrc_doc_no").css("display", "block");
            $("#vmdoc_no").css("display", "block");
            Flag = 'Y';
        }

        
        var docpendingqty = $('#UnResQty').val()
        if (parseFloat(totalres_unresqty) > parseFloat(docpendingqty)) {
            swal("", $("#ReserveQtyExceedingPendingQty").text(), "warning");
            return false
        }
    }
    
    if (Flag == 'N') {
        if (TransType == "R") {
            if (parseFloat(totalres_unresqty) == parseFloat(0)) {
                if (TransType == "R") {
                    swal("", $("#ResstkDetailsnotfound").text(), "warning");
                }
                //if (TransType == "U") {
                //    swal("", $("#UnresstkDetailsnotfound").text(), "warning");
                //}
                $("#Rev_UnrevDetails").css("border-color", "red");
                return false;
            }
        }
        if (CheckResstockDetails() == false) {
            if (TransType == "R") {
                swal("", $("#ResstkDetailsnotfound").text(), "warning");
            }
            if (TransType == "U") {
                swal("", $("#UnresstkDetailsnotfound").text(), "warning");
            }
            $("#Rev_UnrevDetails").css("border-color", "red");
            return false;
        }
    }
    
    if (Flag == 'Y') {
        ValidationFlag = false;
    }
    else {
        ValidationFlag = true;
    }
    if (ValidationFlag == true) {
        if (CheckValidations_forSubItems() == false) {
            return false;
        }
        var SubItemsListArr = Cmn_SubItemList();
        var strsubitemlist = JSON.stringify(SubItemsListArr);
        $('#SubItemDetailsDt').val(strsubitemlist);
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
            return true;       
    }
    else {
        return false;
    }

}
function CheckResstockDetails() {
    debugger
    var res_unresdetails = $("#hdres_unresdetails").val();
    if (res_unresdetails == "" || res_unresdetails == null) {
        return false;
    }
    var Res_UnresDetails = JSON.parse(res_unresdetails);
    if (Res_UnresDetails != null) {
        if (Res_UnresDetails.length > 0) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
}
function OnClickIconBtn(e, item_id, item_mame, edit) {
    debugger;
    var ItmCode = "";
    ItmCode = $("#ddl_ProductName").val();
    if (edit == 'edit') {
        ItmCode = item_id;
      }
    ItemInfoBtnClick(ItmCode);
}
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        var ItemId = $("#hdn_product_id").val();;
        var ItemName = $("#hdn_product_name").val();
        var UOMName = $("#UOM").val();

        $.ajax(
            {
                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId
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
function OnClickIconBtn1(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hdItemId").val();
   
    ItemInfoBtnClick(ItmCode);
}
function OnClickReservedQtyIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();   
    var ItemName = clickedrow.find("#lblitemname").text();
    var UOMName = clickedrow.find("#lbluom").text(); 
    var UOMid = clickedrow.find("#hduomid").text();
    var Branch = clickedrow.find("#lblcompname").text();
    var Warehouse = clickedrow.find("#lblwhname").text(); 
    var ResQty = clickedrow.find("#lblresqty").text(); 
    var TotStk = clickedrow.find("#lblwhtotstk").text(); 
    var AvalStk = clickedrow.find("#lblwhavltstk").text(); 
    var Wh_id = clickedrow.find("#ReshdWhId").val();
    var sub_item = clickedrow.find("#hdmListSubItem").val();
    $("#list_sub_item").val(sub_item);
    if (sub_item == null) {
        $("#div_SubItemPopupPendResQty").css("display", "");
    } else {
        $("#div_SubItemPopupPendResQty").css("display", "none");
    }
    $("#UR_Insight_OpenBy").val("ResTbl");

    var poptitle = $("#hdnresval").val();
    $("#SubItemPopupTotalUnResQty").attr("onclick", "return SubItemDetailsPopUp('ListUnResQuantity',event)");
    AddReservedItemDetail(ItmCode, Wh_id, "tblrev", ItemName, UOMName, UOMid, Warehouse, Branch, ResQty, TotStk, AvalStk, poptitle);
}
function AddReservedItemDetail(ItmID, Wh_id, Flag, ItemName, uomName, uomid, WhName, BranchName, RevQty, TotalStk
    , AvlTotalStk, poptitle) {
    debugger;
    var EntityName = $("#ddlCustomerName option:selected").text();
    var EntityId = $("#ddlCustomerName").val();
    var DocType = $("#Document_Type option:selected").text();
    var DocNo = $("#ddlOrderNumber").val();
    var DocDate = $("#txtdoc_dt").val();
    var DocQty = $("#DocQty").val();
    var DocPendingQty = $("#UnResQty").val();
    if (DocNo == "---Select---") {
        DocNo = "0";
    }

    var BranchID = $("#TopNavBranchList").val();
    var ItemLotBatchdetail = $("#hdres_unresdetails").val();

    var ItemID = ItmID;   
    if (ItemID != "" && ItemID != null && Wh_id != "" && Wh_id != null) {
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/StockReservation/GetReservedItemDetail",
                data: { ItemID: ItmID, wh_id: Wh_id, flag: Flag, entity_id: EntityId, DocNo: DocNo, SelectedItemLotBatchdetail: ItemLotBatchdetail},
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        $('#Un_ReservedQtyDetail tbody tr').remove();
                        $('#ReservedQtyDetail tbody tr').remove();
                        var span_SubItemDetail = $("#span_SubItemDetail").val();
                        if (arr.Table.length > 0) {
                            ++rowIdx;
                            var sub_item = $("#list_sub_item").val();
                            var disablesubitm = "";
                            if (sub_item != "Y") {
                                disablesubitm = "disabled";
                            }
                            HideShowPopupSubItem(sub_item, "");
                            if (Flag == "rev") {
                                $("#Span_ResStkPopup").text(poptitle);
                                $("#ResItemName").val(ItemName.trim());
                                $("#hdn_itemid").val(ItmID.trim());
                                $("#ResUOM").val(uomName.trim());
                                $("#hdn_uom").val(uomid.trim());
                                $("#ResWarehouse").val(WhName.trim());
                                $("#hdn_whid").val(Wh_id.trim());
                                $("#ResBranch").val(BranchName.trim());
                                $("#hdn_branch").val(BranchID.trim());
                                $("#ResEntityName").val(EntityName.trim());
                                $("#ResDocumentType").val(DocType.trim());
                                $("#ResDocumentNumber").val(DocNo.trim());
                                $("#ResDocumentDate").val(DocDate.trim());
                                $("#ResDocumentQuantity").val(DocQty.trim());
                                $("#ResPendingQuantity").val(parseFloat(DocPendingQty.trim()).toFixed(QtyDecDigit));
                                $("#popupReservedQuantity").val(parseFloat(RevQty.trim()).toFixed(QtyDecDigit));
                                $("#popupTotalQuantity").val(parseFloat(TotalStk.trim()).toFixed(QtyDecDigit));
                                $("#popupAvailableQuantity").val(parseFloat(AvlTotalStk.trim()).toFixed(QtyDecDigit));

                                var totalrevqty = 0;
                                for (var i = 0; i < arr.Table.length; i++) {
                                    var resqty = arr.Table[i].res_qty;
                                    if (resqty != "" && resqty != null) {
                                        if (parseFloat(resqty) != parseFloat(0)) {
                                            totalrevqty = parseFloat(totalrevqty) + parseFloat(resqty);
                                        }
                                    }
                                    var rowIdx = 0;
                                    $('#ReservedQtyDetail tbody').append(`<tr id="${rowIdx}">
                                
                                <td><input id="ResLot" value="${arr.Table[i].lotno}" class="form-control" type="text" name="Lot" disabled></td>
                                <td><input id="ResBatch" value="${arr.Table[i].batchno}" class="form-control" type="text" name="Batch1" disabled></td>
                                <td><input id="MfgName" value="${IsNull(arr.Table[i].mfg_name,'')}" class="form-control" type="text" name="Batch1" disabled></td>
                                <td><input id="MfgMrp" value="${parseFloat(CheckNullNumber(arr.Table[i].mfg_mrp)).toFixed(cmn_RateDecDigit)}" class="form-control" type="text" name="Batch1" disabled></td>
                                <td><input id="" value="${Cmn_FormatDate_ddmmyyyy(arr.Table[i].mfg_date)}" class="form-control" type="text" name="Batch1" disabled><input id="MfgDate" value="${IsNull(arr.Table[i].mfg_date, '')}" type="hidden"></td>
                                <td><input id="ResExDate" value="${arr.Table[i].expirydate}" class="form-control" type="text" name="Batch1" disabled><input id="hdn_ResExDate" value="${arr.Table[i].ex_date}" class="form-control" type="hidden" disabled></td>
                                <td><input id="ResSerial" value="${arr.Table[i].serialno}" class="form-control" type="text" name="Serial1" disabled></td>
                                <td>
                                <div class="col-sm-12 lpo_form no-padding">
                                <input id="ResAvlQty" value="${parseFloat(arr.Table[i].Qty).toFixed(QtyDecDigit)}" class="form-control num_right" type="text" name="AvlQuantity"  disabled>
                                </div>
                                
                                </td>
                                <td>
                                <div class="col-sm-12 lpo_form no-padding">
                                <input id="ResReservedQty" value="${parseFloat(arr.Table[i].res_qty).toFixed(QtyDecDigit)}" class="form-control num_right" onchange="OnchangeUnres_ResQty(event);" onkeypress="return OnKeyPressUnres_ResQty(this,event);" type="text" name="ReservedQuantity">
                                <span id="resqty_Error" class="error-message is-visible"></span>
                                </div>
                               
                                </td>
                                <td hidden="hidden" id="bt_sale">${arr.Table[i].bt_sale}</td>
                                </tr>`);
                                }
                                $("#TotalResQuantity").text(parseFloat(totalrevqty).toFixed(QtyDecDigit));
                            }
                            else {
                                $("#Span_UnResStkPopup").text(poptitle);
                                $("#UnresItemName").val(ItemName.trim());
                                $("#hdn_Unresitemid").val(ItmID.trim());
                                $("#UnresUOM").val(uomName.trim());
                                $("#hdn_Unresuom").val(uomid.trim());
                                $("#UnresWarehouse").val(WhName.trim());
                                $("#hdn_Unreswhid").val(Wh_id.trim());
                                $("#UnresBranch").val(BranchName.trim());
                                $("#hdn_Unresbranch").val(BranchID.trim());
                                $("#UnrespopupReservedQuantity").val(parseFloat(RevQty.trim()).toFixed(QtyDecDigit));
                                $("#UnrespopupTotalQuantity").val(parseFloat(TotalStk.trim()).toFixed(QtyDecDigit));
                                $("#UnrespopupAvailableQuantity").val(parseFloat(AvlTotalStk.trim()).toFixed(QtyDecDigit));
                                var totalunrevqty = 0;
                                var Doc_type = "";
                                for (var i = 0; i < arr.Table.length; i++) {
                                    var rowIdx = 0;
                                    var unresqty;
                                    if (Flag == "tblrevstk") {
                                        unresqty = arr.Table[i].res_qty;
                                    }
                                    else {
                                        unresqty = arr.Table[i].unres_qty;
                                    }
                                    if (Flag == "tblrev" || Flag == "unrev") {
                                        Doc_type = DocType;
                                    }
                                    else {
                                        Doc_type = arr.Table[i].srcname;
                                    }

                                    if (unresqty != "" && unresqty != null) {
                                        if (parseFloat(unresqty) != parseFloat(0)) {
                                            totalunrevqty = parseFloat(totalunrevqty) + parseFloat(unresqty);
                                        }
                                    }
                                    $('#Un_ReservedQtyDetail tbody').append(`<tr id="${rowIdx}">
                                <td><input id="Unrestransdt" value="${arr.Table[i].trans_date}" class="form-control" type="text" disabled></td>
                                <td class="ItmNameBreak itmStick tditemfrz"><input id="EntityName" value="${arr.Table[i].CustName}" class="form-control" type="text" name="EntityName" disabled></td>
                                <td><input id="DocumentType" value="${Doc_type}" class="form-control " type="text" name="DocumentType" disabled></td>
                                <td><input id="DocumentNumber" value="${arr.Table[i].srcno}" class="form-control" type="text" name="DocumentNumber" disabled></td>
                                <td><input id="DocumentDate" value="${arr.Table[i].srcdate}" class="form-control" type="text" name="DocumentDate" disabled><input type="hidden" id="hd_docdt" value="${arr.Table[i].src_date}" style="display: none;" /></td>
                                <td><input id="DocumentQuantity" value="${parseFloat(CheckNullNumber(arr.Table[i].SrcQty)).toFixed(QtyDecDigit)}" class="form-control num_right" type="text" name="DocumentQuantity" disabled></td>
                                <td><input id="Lot" value="${arr.Table[i].lotno}" class="form-control" type="text" name="Lot" disabled></td>
                                <td><input id="Batch" value="${arr.Table[i].batchno}" class="form-control" type="text" name="Batch1" disabled></td>
                                <td><input id="MfgName" value="${IsNull(arr.Table[i].mfg_name, '')}" class="form-control" type="text" name="Batch1" disabled></td>
                                <td><input id="MfgMrp" value="${parseFloat(CheckNullNumber(arr.Table[i].mfg_mrp)).toFixed(cmn_RateDecDigit)}" class="form-control" type="text" name="Batch1" disabled></td>
                                <td><input id="" value="${Cmn_FormatDate_ddmmyyyy(arr.Table[i].mfg_date)}" class="form-control" type="text" name="Batch1" disabled><input id="MfgDate" value="${IsNull(arr.Table[i].mfg_date, '')}" type="hidden"></td>
                                <td><input id="ExDate" value="${arr.Table[i].expirydate}" class="form-control" type="text" name="Batch1" disabled><input id="hdn_ExDate" value="${arr.Table[i].ex_date}" class="form-control" type="hidden" disabled></td>
                                <td><input id="Serial" value="${arr.Table[i].serialno}" class="form-control" type="text" name="Serial1" disabled></td>
                                <td id="res_qty"><input id="ReservedQuantity" value="${parseFloat(CheckNullNumber(arr.Table[i].res_qty)).toFixed(QtyDecDigit)}" class="form-control num_right" type="text" name="ReservedQuantity"></td>
                                <td id="unres_qty"><div class="lpo_form">
                                <input id="UnreserveQuantity" value="${parseFloat(CheckNullNumber(arr.Table[i].unres_qty)).toFixed(QtyDecDigit)}" class="form-control num_right" onchange="OnchangeUnres_ResQty(event);" onkeypress="return OnKeyPressUnres_ResQty(this,event);" type="text" name="UnreserveQuantity">
                                <span id="unresqty_Error" class="error-message is-visible"></span>
                                 </div></td>
                                </tr>`);
                                }
                                $("#TotalUnResQuantity").text(parseFloat(totalunrevqty).toFixed(QtyDecDigit));
                                if (Flag == "unrev") {
                                    $("#div_Unresfooter").removeAttr("style");
                                }
                                else {
                                    $("#div_Unresfooter").css("display", "none");
                                }

                            }
                        } else {

                            $("#U_SubItemPopupResStock").attr("disabled", true);
                            $("#U_SubItemPopupTotalStock").attr("disabled", true);
                            $("#U_SubItemPopupAvlStock").attr("disabled", true);

                            $("#UnrespopupReservedQuantity").val(null);
                            $("#UnrespopupTotalQuantity").val(null);
                            $("#UnrespopupAvailableQuantity").val(null);
                            $("#UnresItemName").val(null);
                            $("#UnresUOM").val(null);
                            $("#UnresBranch").val(null);
                            $("#UnresWarehouse").val(null);
                        }
                    }
                    $("#Un_ReservedQtyDetail TBODY TR").each(function () {
                        var row = $(this);

                        if (Flag == "tblrev") {
                            $("#thunrevqty").css("display", "none");
                            $("#tdftotal").css("display", "none");
                            row.find("#unres_qty").css("display", "none");

                            $("#threvqty").removeAttr("style");
                            row.find("#res_qty").removeAttr("style");
                            row.find("#ReservedQuantity").attr("readonly", "readonly");

                            var TotResStk = 0;
                            $("#Un_ReservedQtyDetail TBODY TR").each(function () {
                                var row = $(this);
                                var resqty = row.find("#ReservedQuantity").val();
                                if (resqty != "" && resqty != null) {
                                    if (parseFloat(resqty) != parseFloat(0)) {
                                        TotResStk = parseFloat(TotResStk) + parseFloat(resqty);
                                    }
                                }
                            });
                            if (TotResStk != "" && TotResStk != null) {
                                if (parseFloat(TotResStk) != parseFloat(0)) {
                                    $("#TotalUnResQuantity").text(parseFloat(TotResStk).toFixed(QtyDecDigit));
                                }
                                else {
                                    $("#TotalUnResQuantity").text(parseFloat(0).toFixed(QtyDecDigit));
                                }
                            }
                            else {
                                $("#TotalUnResQuantity").text(parseFloat(0).toFixed(QtyDecDigit));
                            }

                        }
                        if (Flag == "tblrevstk") {
                            $("#thunrevqty").css("display", "none");
                            $("#tdftotal").css("display", "none");
                            row.find("#unres_qty").css("display", "none");

                            $("#threvqty").removeAttr("style");
                            row.find("#res_qty").removeAttr("style");
                            row.find("#ReservedQuantity").attr("readonly", "readonly");

                        }
                       
                        if (Flag == "unrev") {
                            $("#thunrevqty").removeAttr("style");
                            $("#tdftotal").removeAttr("style");
                            row.find("#unres_qty").removeAttr("style");

                            $("#threvqty").removeAttr("style");
                            row.find("#res_qty").removeAttr("style");
                            row.find("#ReservedQuantity").attr("readonly", "readonly");

                        }
                    });
                    $("#ReservedQtyDetail TBODY TR").each(function () {
                        var row = $(this);
                        var bt_sale = row.find("#bt_sale").text();
                        if (bt_sale == "N") {
                            row.find("#ResReservedQty").attr("disabled", true)
                        }
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                }
            });
    } else {
    }
}

function OnchangeType() {
    debugger;
    var reserve = $("#LblReserveDetail").text();
    var unreserve = $("#LblUnreserveDetail").text();
    var ddltype = $("#TransType").val();
    
    if (ddltype == "U") {
        $("#Rev_UnrevDetails").attr("data-target", "#UnReservedStockDetail");
        $("#entitystr").css("display", "none");
        $("#docnostr").css("display", "none");
        $("#divrevsec").css("display", "none");
        $("#Rev_UnrevDetails").html(unreserve);
    }
    else {
        $("#Rev_UnrevDetails").attr("data-target", "#ReservedStockDetail");
        $("#entitystr").removeAttr("style");
        $("#docnostr").removeAttr("style");
        $("#divrevsec").css("display", "block");
        $("#Rev_UnrevDetails").html(reserve);
    }
    $("#ddl_ProductName").val("0").trigger('change');
    $('#hdres_unresdetails').val("");
    SetDefault_hideErrormsg();
}
function OnClickRes_UnResDetails() {
    debugger;
    var ItemID, WhID,Type = "";
    ItemID = $("#ddl_ProductName").val();
    WhID = $("#toWh").val();
    Type = $("#TransType").val();

    var ItemName = $("#ddl_ProductName option:selected").text();
    var uomName = $("#UOM").val();
    var uomid = $("#UOMID").val();
    var WhName = $("#toWh option:selected").text();
    var BranchName = $("#TopNavBranchList option:selected").text();
    var RevQty = $("#ReservedStock").val();
    var TotalStk = $("#TotalStock").val();
    var AvlTotalStk = $("#AvailableStock").val();
    let subitem = $("#sub_item").val();
    $("#list_sub_item").val(subitem);
    $("#SubItemPopupTotalUnResQty").attr("onclick", "return SubItemDetailsPopUp('PPTotalUnResQuantity',event)");
    $("#div_SubItemPopupPendResQty").css("display","block")
    if (Type == "R") {
        var rpoptitle = $("#hdnresval").val();
        AddReservedItemDetail(ItemID, WhID, "rev", ItemName, uomName, uomid, WhName, BranchName, RevQty, TotalStk, AvlTotalStk, rpoptitle);
    }
    if (Type == "U") {
        var upoptitle = $("#hdnunresval").val();
        $("#UR_Insight_OpenBy").val("UrBtn");
        AddReservedItemDetail(ItemID, WhID, "unrev", ItemName, uomName, uomid, WhName, BranchName, RevQty, TotalStk, AvlTotalStk, upoptitle);
    }
}
function OnClickTotResStockDetailsIcon() {
    debugger;
    var ItemID, WhID, Type = "";
    ItemID = $("#ddl_ProductName").val();
    WhID = $("#toWh").val();
    Type = $("#TransType").val();

    var ItemName = $("#ddl_ProductName option:selected").text();
    var uomName = $("#UOM").val();
    var uomid = $("#UOMID").val();
    var WhName = $("#toWh option:selected").text();
    var BranchName = $("#TopNavBranchList option:selected").text();
    var RevQty = $("#ReservedStock").val();
    var TotalStk = $("#TotalStock").val();
    var AvlTotalStk = $("#AvailableStock").val();
    var poptitle = $("#lbl_totresstk").val();
    let subitem = $("#sub_item").val();
    $("#list_sub_item").val(subitem);
    AddReservedItemDetail(ItemID, WhID, "tblrevstk", ItemName, uomName, uomid, WhName, BranchName, RevQty, TotalStk, AvlTotalStk, poptitle);

}
function OnClickSaveAndExitxBtn() {
    if (CheckValidationOnSaveAndExit() === true) {
        
        var ddltype = $("#TransType").val();
        var Res_UnresItemDetailList = new Array();
        var totalresqty = 0;
        $("#UnReservedStockDetail").modal('hide');
        if (ddltype == "U") {
            $("#Un_ReservedQtyDetail TBODY TR").each(function () {
                var row = $(this);
                var Unres_ItemList = {};
                var unresqty = row.find("#UnreserveQuantity").val();
                if (unresqty != null && unresqty != "") {
                    if (parseFloat(unresqty) > parseFloat(0)) {
                        Unres_ItemList.itemid = $("#hdn_Unresitemid").val();
                        Unres_ItemList.uomid = $("#hdn_Unresuom").val();
                        Unres_ItemList.whid = $("#hdn_Unreswhid").val();
                        Unres_ItemList.docno = row.find("#DocumentNumber").val();
                        Unres_ItemList.docdt = row.find("#hd_docdt").val();
                        Unres_ItemList.transtype = "UR";
                        Unres_ItemList.lotno = row.find("#Lot").val();
                        Unres_ItemList.batchno = row.find("#Batch").val();
                        Unres_ItemList.expirydt = row.find("#hdn_ExDate").val();
                        Unres_ItemList.serialno = row.find("#Serial").val();
                        Unres_ItemList.res_unresQty = row.find("#UnreserveQuantity").val();
                        Unres_ItemList.mfg_name = row.find("#MfgName").val();
                        Unres_ItemList.mfg_mrp = row.find("#MfgMrp").val();
                        Unres_ItemList.mfg_date = row.find("#MfgDate").val();

                        Res_UnresItemDetailList.push(Unres_ItemList);
                    }
                }
            });
        }
        else {
            $("#ReservedQtyDetail TBODY TR").each(function () {
                var row = $(this);
                var Res_ItemList = {};
                var resqty = row.find("#ResReservedQty").val();
                if (resqty != null && resqty != "") {
                    if (parseFloat(resqty) > parseFloat(0)) {
                        totalresqty = parseFloat(totalresqty) + parseFloat(resqty);

                        Res_ItemList.itemid = $("#hdn_itemid").val();
                        Res_ItemList.uomid = $("#hdn_uom").val();
                        Res_ItemList.whid = $("#hdn_whid").val();
                        Res_ItemList.docno = $("#ResDocumentNumber").val();
                        Res_ItemList.docdt = $("#ResDocumentDate").val();
                        Res_ItemList.transtype = "R";
                        Res_ItemList.lotno = row.find("#ResLot").val();
                        Res_ItemList.batchno = row.find("#ResBatch").val();
                        Res_ItemList.expirydt = row.find("#hdn_ResExDate").val();
                        Res_ItemList.serialno = row.find("#ResSerial").val();
                        Res_ItemList.res_unresQty = row.find("#ResReservedQty").val();
                        Res_ItemList.mfg_name = row.find("#MfgName").val();
                        Res_ItemList.mfg_mrp = row.find("#MfgMrp").val();
                        Res_ItemList.mfg_date = row.find("#MfgDate").val();

                        Res_UnresItemDetailList.push(Res_ItemList);
                    }
                }
            });
        }
        debugger;
        var str = JSON.stringify(Res_UnresItemDetailList);
        $('#hdres_unresdetails').val(str);
        $('#hdtotalres_unresqty').val(totalresqty);
        $("#Rev_UnrevDetails").removeAttr("style");
        
        return true;
    }
    else {
        return false;
    }
}
function CheckValidationOnSaveAndExit() {
    var ddltype = $("#TransType").val();
    if (ddltype == "U") {
        var unrflag = "N"; 
        var unrtblqtyflag = "N"; 
        $("#Un_ReservedQtyDetail TBODY TR").each(function () {
            var row = $(this);
            var ResQty = row.find("#ReservedQuantity").val();
            var UnResQty = row.find("#UnreserveQuantity").val();

            if (parseFloat(UnResQty) > parseFloat(ResQty)) {
                row.find("#unresqty_Error").text($("#ExceedingQty").text());
                row.find("#unresqty_Error").css("display", "block");
                row.find("#UnreserveQuantity").css("border-color", "red");
                unrflag = "Y";
            }
            else {
                row.find("#unresqty_Error").css("display", "none");
                row.find("#UnreserveQuantity").css("border-color", "#ced4da");
                row.find("#UnreserveQuantity").val(parseFloat(UnResQty).toFixed(QtyDecDigit));
            }
            if (parseFloat(UnResQty) > parseFloat(0)) {
                unrtblqtyflag = "Y";
            }
        });
        
        if (unrtblqtyflag === "N") {
            swal("", $("#Unreserveqtyshouldbegreaterthanzero").text(), "warning");
            return false;
        }
        if (CheckValidations_forSubItems() == false) {
            return false;
        }
        $("#UnReservedStockDetail").modal('hide');
        if (unrflag === "Y") {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        var resflag = "N"; 
        var frestotal = $("#TotalResQuantity").text();
        var fresqty = $("#ResPendingQuantity").val();
        if (parseFloat(frestotal) == parseFloat(0)) {
            swal("", $("#ReserveQuantityShouldBeGreaterThan0").text(), "warning");
            return false
        }
        if (parseFloat(frestotal) > parseFloat(fresqty)) {
            swal("", $("#ReserveQtyExceedingPendingQty").text(), "warning");
            return false
        }
        $("#ReservedQtyDetail TBODY TR").each(function () {
            var row = $(this);
            var ResQty = row.find("#ResReservedQty").val();
            var AvlQty = row.find("#ResAvlQty").val();

            if (parseFloat(ResQty) > parseFloat(AvlQty)) {
                row.find("#resqty_Error").text($("#ExceedingQty").text());
                row.find("#resqty_Error").css("display", "block");
                row.find("#ResReservedQty").css("border-color", "red");
                resflag = "Y";
            }
            else {
                row.find("#resqty_Error").css("display", "none");
                row.find("#ResReservedQty").css("border-color", "#ced4da");
                row.find("#ResReservedQty").val(parseFloat(ResQty).toFixed(QtyDecDigit));
            }
        });
        if (CheckValidations_forSubItems() == false) {
            return false;
        }
        $("#ReservedStockDetail").modal('hide');
        if (resflag === "Y") {
            return false;
        }
        else {
            return true;
        }
    }
}
function OnchangeUnres_ResQty(evt) {
    var clickedrow = $(evt.target).closest("tr");
    debugger;
    var ddltype = $("#TransType").val();
    if (ddltype == "U") {
        var ResQty = clickedrow.find("#ReservedQuantity").val();
        var UnResQty = clickedrow.find("#UnreserveQuantity").val();

        if (parseFloat(UnResQty) > parseFloat(ResQty)) {
            clickedrow.find("#unresqty_Error").text($("#ExceedingQty").text());
            clickedrow.find("#unresqty_Error").css("display", "block");
            clickedrow.find("#UnreserveQuantity").css("border-color", "red");
            clickedrow.find("#UnreserveQuantity").val(parseFloat(UnResQty).toFixed(QtyDecDigit));
        }
        else {
            clickedrow.find("#unresqty_Error").css("display", "none");
            clickedrow.find("#UnreserveQuantity").css("border-color", "#ced4da");
            clickedrow.find("#UnreserveQuantity").val(parseFloat(UnResQty).toFixed(QtyDecDigit));
        }
        if ((UnResQty == "") || (UnResQty == null) || (UnResQty == "0")) {
            clickedrow.find("#UnreserveQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        }
        var TotUnResQty = 0;
        $("#Un_ReservedQtyDetail TBODY TR").each(function () {
            var row = $(this);
            var unresqty = row.find("#UnreserveQuantity").val();
            if (unresqty != "" && unresqty != null) {
                if (parseFloat(unresqty) != parseFloat(0)) {
                    TotUnResQty = parseFloat(TotUnResQty) + parseFloat(unresqty);
                }
            }
        });
        if (TotUnResQty != "" && TotUnResQty != null) {
            if (parseFloat(TotUnResQty) != parseFloat(0)) {
                $("#TotalUnResQuantity").text(parseFloat(TotUnResQty).toFixed(QtyDecDigit));
            }
            else {
                $("#TotalUnResQuantity").text(parseFloat(0).toFixed(QtyDecDigit));
            }
        }
        else {
            $("#TotalUnResQuantity").text(parseFloat(0).toFixed(QtyDecDigit));
        }
    }
    else {
        var resqty = clickedrow.find("#ResReservedQty").val();
        var avlqty = clickedrow.find("#ResAvlQty").val();

        if (parseFloat(resqty) > parseFloat(avlqty)) {
            clickedrow.find("#resqty_Error").text($("#ExceedingQty").text());
            clickedrow.find("#resqty_Error").css("display", "block");
            clickedrow.find("#ResReservedQty").css("border-color", "red");
            clickedrow.find("#ResReservedQty").val(parseFloat(resqty).toFixed(QtyDecDigit));
        }
        else {
            clickedrow.find("#resqty_Error").css("display", "none");
            clickedrow.find("#ResReservedQty").css("border-color", "#ced4da");
            clickedrow.find("#ResReservedQty").val(parseFloat(resqty).toFixed(QtyDecDigit));
        }

        if ((resqty == "") || (resqty == null) || (resqty == "0")) {
            clickedrow.find("#ResReservedQty").val(parseFloat(0).toFixed(QtyDecDigit));
        }

        var TotResQty = 0;
        $("#ReservedQtyDetail TBODY TR").each(function () {
            var row = $(this);
            var resqty = row.find("#ResReservedQty").val();
            if (resqty != "" && resqty != null) {
                if (parseFloat(resqty) != parseFloat(0)) {
                    TotResQty = parseFloat(TotResQty) + parseFloat(resqty);
                }
            }
        });
        if (TotResQty != "" && TotResQty != null) {
            if (parseFloat(TotResQty) != parseFloat(0)) {
                $("#TotalResQuantity").text(parseFloat(TotResQty).toFixed(QtyDecDigit));
            }
            else {
                $("#TotalResQuantity").text(parseFloat(0).toFixed(QtyDecDigit));
            }
        }
        else {
            $("#TotalResQuantity").text(parseFloat(0).toFixed(QtyDecDigit));
        }
    }
}
function OnKeyPressUnres_ResQty(el, evt) {

    var clickedrow = $(evt.target).closest("tr");

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var ddltype = $("#TransType").val();
    if (ddltype == "U") {
        clickedrow.find("#unresqty_Error").css("display", "none");
        clickedrow.find("#UnreserveQuantity").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#resqty_Error").css("display", "none");
        clickedrow.find("#ResReservedQty").css("border-color", "#ced4da");
    }
    return true;
}
function GetTotalResStock() {

}
function SetDefault_hideErrormsg() {
    SetDefaultVal();
    document.getElementById("vmsrc_doc_no").innerHTML = null;
    document.getElementById("vmdoc_no").innerHTML = null;
    $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");

    document.getElementById("vmcust_id").innerHTML = null;
    $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "#ced4da");

    document.getElementById("vmtowarehouse").innerHTML = null;
    $("#toWh").css("border-color", "#ced4da");

    $("#vmddl_ProductName").css("display", "none");
    $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");

    document.getElementById("vmsrc_doc_no").innerHTML = null;
    document.getElementById("vmdoc_no").innerHTML = null;
    $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
    $("#ddlOrderNumber").css("border-color", "#ced4da");

    $("#Rev_UnrevDetails").attr("disabled", "disabled");
}
function SetDefaultVal() {
    //$("#ddl_ProductName").val("0").trigger('change');
    $("#ddlCustomerName").val("0").trigger('change');
    $("#toWh").val("0");
    $("#ReservedStock").val(parseFloat(0).toFixed(QtyDecDigit));
    $("#TotalStock").val(parseFloat(0).toFixed(QtyDecDigit));
    $("#AvailableStock").val(parseFloat(0).toFixed(QtyDecDigit));
}

/***--------------------------------Sub Item Section-----------------------------------------***/

function HideShowPageWise(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, "NoRow", "sub_item", "SubItemTotalResStk",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemTotalStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemAvlStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemDocQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPendingPackQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPendResQty",);
}
function HideShowPopupSubItem(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, "NoRow", "Popup_sub_item", "SubItemPopupDocQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupResQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupTotalStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupAvlStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupPendResQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupTotalResQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "U_SubItemPopupResStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "U_SubItemPopupTotalStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "U_SubItemPopupAvlStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupTotalUnResQty",);
}
function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');

    var ProductNm = $("#ddl_ProductName option:selected").text();
    var ProductId = $("#ddl_ProductName").val();
    var UOM = $("#UOM").val();

    /*var hdn_branch = $("#hdn_branch").val();*/
    var wh_id = $("#toWh").val();
    var Document_Type = $("#Document_Type").val();
    var ResDocNumber = $("#ddlOrderNumber").val();
    var ResDocumentDate = $("#txtdoc_dt").val();
    var ddlCustomerName = $("#ddlCustomerName").val();
    var TransType = $("#TransType").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();

    if (flag == "PPTotalResQuantity") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);
        });
        
        Sub_Quantity = $("#TotalResQuantity").text();
    }
    else if (flag == "PPTotalUnResQuantity") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);
        });
        
        Sub_Quantity = $("#TotalUnResQuantity").text();
    }
    else if (flag == "DocQuantity") {
        Sub_Quantity = $("#DocQty").val();
    }
    else if (flag == "PendPackQty") {
        Sub_Quantity = $("#PendQty").val();
    }
    else if (flag == "PendResQty") {
        Sub_Quantity = $("#UnResQty").val();
    }

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/StockReservation/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            Flag: flag,
            doc_type: Document_Type,
            cust_id: ddlCustomerName,
            doc_no: ResDocNumber,
            doc_dt: ResDocumentDate,
            wh_id: wh_id,
            TransType: TransType
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
    //return Cmn_CheckValidations_forSubItems("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y");
    //return Cmn_CheckValidations_forSubItems("datatable-buttons", "", "tblhdn_item_id", "TxtOpQty", "SubItemOPQty", "Y");
    if ($("#TransType").val() == "R") {
        return Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "TotalResQuantity", "SubItemPopupTotalResQty", "Y");
    } else {
        return Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "TotalUnResQuantity", "SubItemPopupTotalResQty", "Y");
    }
    
}
function ResetWorningBorderColor() {
    if ($("#TransType").val() == "R") {
        return Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "TotalResQuantity", "SubItemPopupTotalResQty", "N");
    } else {
        return Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "TotalUnResQuantity", "SubItemPopupTotalResQty", "N");
    }
}
function SubItemStockDetailsPopUp(flag, e) {
    debugger;
    var ProductNm = $("#ddl_ProductName option:selected").text();
    var ProductId = $("#ddl_ProductName").val();
    var UOM = $("#UOM").val();
    var toWh = $("#toWh").val();
    var Quantity = 0;
    if (flag == "U_AvlStk" || flag == "U_TotalResStk" || flag == "U_TotalStk") {
        ProductNm = $("#UnresItemName").val();
        ProductId = $("#hdn_Unresitemid").val();
        UOM = $("#UnresUOM").val();
        toWh = $("#hdn_Unreswhid").val();
    }
    if (flag == "AvlStk") {

        Quantity = $("#AvailableStock").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "wh");
    } else if (flag == "U_AvlStk") {

        Quantity = $("#UnrespopupAvailableQuantity").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "wh");
    } else if (flag == "TotalResStk") {

        Quantity = $("#ReservedStock").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "whres");
    } else if (flag == "U_TotalResStk") {

        Quantity = $("#UnrespopupReservedQuantity").val();
        if ($("#UR_Insight_OpenBy").val() == "ResTbl") {
            Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "whsores");
        }
        else {
            Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "whres");
        }
    } else if (flag == "TotalStk") {

        Quantity = $("#TotalStock").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "wh_tstk");
    } else if (flag == "U_TotalStk") {

        Quantity = $("#UnrespopupTotalQuantity").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "wh_tstk");
    }
    
}
/***--------------------------------Sub Item Section End-----------------------------------------***/
