/*
 modifyed by shubham maurya on 21-09-2022 17:51 js to model
 */
/************************************************
Javascript Name:Sample Receipt Detail
Created By:Prem
Created Date: 29-07-2021
Description: This Javascript use for the  opening material  many function

Modified By:
Modified Date:
Description:

*************************************************/
let itmidArr = [];
$(document).ready(function () {
    debugger;

    var Wh_id = $("#wh_id").val();
    if (Wh_id != "0") {
        BindSampleRcptItmList(1);
        //ItemStockBatchWise(el, evt);
    }
    else {

        RemoveSessionNew()
    }


    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var WF_status = $("#WF_status").val();
            var clickedrow = $(e.target).closest("tr");
            var Wh_id = clickedrow.children("#td_wh_id").text().trim();
            var id = clickedrow.find("#opstk_id").val();
            var _OPR_Date = clickedrow.children("#OPR_DT").text();
            var ListFilterData = $("#ListFilterData").val();
            if (Wh_id != null && _OPR_Date != "") {
                window.location.href = "/ApplicationLayer/OpeningMaterialReceipt/EditOMR/?id=" + id + "&wh_id=" + Wh_id + "&Opening_dt=" + _OPR_Date + "&WF_status=" + WF_status + "&ListFilterData=" + ListFilterData;
            }
        }
        catch (err) {
            debugger
        }
    });

    OnClickSerialDeleteIcon();//Added by Suraj Maurya on 07-10-2024 to delete serial no

    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var Wh_id = clickedrow.children("#td_wh_id").text().trim();
        var op_id = clickedrow.find("#opstk_id").val();
        var _OPR_Date = clickedrow.children("#OPR_DT").text();
        row_id = op_id;
        var op_docno = Wh_id + "_" + op_id;
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(op_docno);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(op_docno, _OPR_Date, Doc_id, Doc_Status);
    });
    debugger;
    GetViewDetails();
    var wh_id = $("#wh_id").val();
    var op_id = $("#opstk_rno").val();
    var op_docno = (wh_id + "_" + op_id);
    $("#hdDoc_No").val(op_docno);
});
function GetViewDetails() {
    //var QtyDecDigit = $("#QtyDigit").text();///Quantity
    //var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    //var ValDecDigit = $("#ValDigit").text();///Amount
    debugger;
    if ($("#HdnBatchDetail").val() != null && $("#HdnBatchDetail").val() != "") {
        var arr2 = $("#HdnBatchDetail").val();
        var arr = JSON.parse(arr2);
        $("#HdnBatchDetail").val("");
        debugger;

        if (arr.length > 0) {
            var BatchDetailList = [];
            for (var j = 0; j < arr.length; j++) {
                var RowSNo;
                $("#datatable-buttons >tbody >tr").each(function () {
                    //debugger;
                    var currentRow = $(this);
                    var SNo = currentRow.find("#hfSNo").val();
                    var ItmCode = "";
                    ItmCode = currentRow.find('#hf_ItemID').val();
                    if (ItmCode == arr[j].item_id) {
                        RowSNo = SNo;
                    }
                });

                var BItmCode = arr[j].item_id;
                var BBatchQty = arr[j].batch_qty;
                var BBatchNo = arr[j].batch_no;
                var BExDate = arr[j].exp_dt;
                var mfg_name = arr[j].mfg_name || '';
                var mfg_mrp = arr[j].mfg_mrp || '';
                var mfg_date = arr[j].mfg_date || '';
                if (BExDate == null) {
                    BExDate = "";
                }
                BatchDetailList.push({
                    UserID: $("#UserID").text(), RowSNo: RowSNo, ItemID: BItmCode, BatchQty: BBatchQty, BatchNo: BBatchNo, BatchExDate: BExDate
                    , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                })
            }
            //debugger;
            sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
        }


        //if (arr.length > 0) {
        //    var rowIdx = 0;
        //    var BatchDetail = [];
        //    for (var j = 0; j < arr.length; j++) {

        //        var Item_id = arr[j].item_id;
        //        var Batch_no = arr[j].batch_no;
        //        var Batch_qty = arr[j].batch_qty;
        //        var Exp_dt = arr[j].exp_dt;

        //        BatchDetail.push({
        //            Item_id: Item_id, Batch_no: Batch_no, Batch_qty: Batch_qty, Exp_dt: Exp_dt
        //        })
        //    }
        //    sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetail));
        //    bindBatchDetail(BatchDetail)
        //}
    }
    else {
        sessionStorage.removeItem("BatchDetailSession");
    }
    if ($("#HdnSerialDetail").val() != null && $("#HdnSerialDetail").val() != "") {
        debugger;
        var arr2 = $("#HdnSerialDetail").val();
        var arr = JSON.parse(arr2);
        $("#HdnSerialDetail").val("");
        debugger;
        if (arr.length > 0) {
            var SerialDetailList = [];

            for (var k = 0; k < arr.length; k++) {
                var RowSNo;
                $("#datatable-buttons >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var SNo = currentRow.find("#hfSNo").val();
                    var ItmCode = "";
                    ItmCode = currentRow.find('#hf_ItemID').val();
                    if (ItmCode == arr[k].item_id) {
                        RowSNo = SNo;
                    }
                });

                var SItmCode = arr[k].item_id;
                var SSerialNo = arr[k].serial_no;
                var MfgName = arr[k].mfg_name || '';
                var MfgMrp = arr[k].mfg_mrp || '';
                var MfgDate = arr[k].mfg_date || '';

                SerialDetailList.push({
                    UserID: UserID, RowSNo: RowSNo, ItemID: SItmCode, SerialNo: SSerialNo
                    , MfgName: MfgName, MfgMrp: MfgMrp, MfgDate: MfgDate
                })
            }
            //debugger;
            sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
        }

        //if (arr.length > 0) {
        //    var rowIdx = 0;
        //    var SerialDetail = [];
        //    for (var j = 0; j < arr.length; j++) {

        //        var Item_id = arr[j].item_id;
        //        var Batch_no = arr[j].serial_no;

        //        SerialDetail.push({
        //            Item_id: Item_id, serial_no: serial_no
        //        })
        //    }
        //    sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetail));
        //}
    } else {
        sessionStorage.removeItem("SerialDetailSession");
    }
}

var QtyDecDigit = $("#QtyDigit").text();///Quantity
var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
var ValDecDigit = $("#ValDigit").text();///Amount

function InsertPRDetail() {
    debugger;
    try {
        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") {
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }
        if (!CheckItemBatchAndSerialAndSubItemDetails()) {/* Added by Suraj Maurya on 21-08-2025 */
            return false;
        }
        var DecDigit = $("#ValDigit").text();
        var INSDTransType = sessionStorage.getItem("INSTransType");

        if (Insert_OpeningItem_Detail() == false) {
            return false;
        }
        //var subitmFlag = CheckValidations_forSubItems();/* Commented by Suraj Maurya on 21-08-2025 due to not able to check if items is on multiple page,through pagination */
        //if (subitmFlag == false) {
        //    return false;
        //}
        //debugger;

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
        showLoader();
        try {
            FinalItemDetail = InsertPRItemDetails();
            hideLoader();
        } catch (ex) {
            console.log(ex);
            hideLoader();
        }


        debugger;
        var str = JSON.stringify(FinalItemDetail);
        $('#hdShipmentItemDetail').val(str);

        $("#wh_id").attr("disabled", false);
        $("#OpeningValue").attr("disabled", false);
        debugger;
        var batchdetails = [];
        batchdetails = GetOP_ItemBatchDetails();
        debugger;
        var str1 = JSON.stringify(batchdetails);
        $('#HDSelectedBatchwise').val(str1);
        debugger;
        var serialdetails = [];
        serialdetails = GetOP_ItemSerialDetails();
        debugger;
        var str2 = JSON.stringify(serialdetails);
        $('#HDSelectedSerialwise').val(str2);


        /*-----------Sub-item-------------*/

        var SubItemsListArr = Cmn_SubItemList();
        var str2 = JSON.stringify(SubItemsListArr);
        $('#SubItemDetailsDt').val(str2);

        /*-----------Sub-item end-------------*/
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");

        return true;
    }
    catch
    (ex) {
        console.log(ex);
        return false;
    }
    // sessionStorage.setItem("BatchDetailSession", JSON.stringify([]));
    ResetEntryDetails()
};
function InsertPRItemDetails() {
    debugger;
    var ItemDetailList = new Array();

    var newTableArr = $('#datatable-buttons').DataTable().rows().data().toArray();
    //$("#datatable-buttons tbody tr").remove();

    newTableArr.map((item) => {

        var ItemList = {};
        debugger;
        ItemList.row_id = item[0];
        ItemList.ItemId = item[1];
        ItemList.UOMId = item[2]
        ItemList.OPQty = item[3]
        ItemList.LotNumber = item[4]
        ItemList.ItemCost = item[5]
        ItemList.OpValue = item[6]
        ItemDetailList.push(ItemList);

        //        $("#datatable-buttons tbody").append(`<tr>
        //<td>${item[0]}</td>
        //<td>${item[1]}</td>
        //<td>${item[2]}</td>
        //<td>${item[3]}</td>
        //<td>${item[4]}</td>
        //<td hidden>${item[5]}</td>
        //<td>${item[6]}</td>
        //<td hidden>${item[7]}</td>
        //<td>${item[8]}</td>
        //</tr>`);
    })

    //$("#datatable-buttons tbody tr").each(function () {
    //    debugger;
    //    var row = $(this);
    //    var ItemList = {};
    //    debugger;
    //    ItemList.ItemId = row.find("#tblhdn_item_id").val();
    //    ItemList.UOMId = row.find('#tblhdn_uom_id').val();
    //    ItemList.OPQty = row.find('#SpanOpQty').text();
    //    ItemList.LotNumber = row.find("#SpanLotNumber").text();
    //    ItemList.ItemCost = row.find("#SpanItemRate").text();
    //    ItemList.OpValue = row.find('#SpanOp_val').text();
    //    ItemDetailList.push(ItemList);
    //    debugger;
    //});
    return ItemDetailList;
}
function OPR_Detail() {
    debugger;
    try {
        location.href = "/ApplicationLayer/OpeningMaterialReceipt/EditOMR";
    } catch (err) {
        console.log("OPR Error : " + err.message);
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
        debugger;
        if (isConfirm) {
            debugger;
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();
            RemoveSessionNew()
            return true;
        } else {
            return false;
        }


    });
    return false;
}
function EnableEditkBtn() {
    debugger;
    $("#wh_id").attr("disabled", false);
    $("#OpeningValue").attr("disabled", false);


}
function CheckOPR_ItemValidations() {
    debugger;
    var ErrorFlag = "N";
    $("#OP_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        //var Sno = currentRow.find("#hfSNo").val();


        if (currentRow.find("#ItemName_1").val() == "" || currentRow.find("#ItemName_1").val() == "0") {

            currentRow.find("#ItemNameError").text($("#valueReq").text());
            currentRow.find("#ItemNameError").css("display", "block");
            $("[aria-labelledby='select2-ItemName_1-container']").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ItemNameError").css("display", "none");
            $("[aria-labelledby='select2-ItemName_1-container']").css("border-color", "#ced4da");


        }

        if (currentRow.find("#ItemCost").val() == "" || parseFloat(currentRow.find("#ItemCost").val()) == parseFloat("0")) {
            currentRow.find("#ItemCostError").text($("#valueReq").text());
            currentRow.find("#ItemCostError").css("display", "block");
            currentRow.find("#ItemCost").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ItemCostError").css("display", "none");
            currentRow.find("#ItemCost").css("border-color", "#ced4da");
        }
        if (currentRow.find("#OPQty").val() == "" || parseFloat(currentRow.find("#OPQty").val()) == parseFloat("0")) {
            currentRow.find("#OPQtyError").text($("#valueReq").text());
            currentRow.find("#OPQtyError").css("display", "block");
            currentRow.find("#OPQty").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#OPQtyError").css("display", "none");
            currentRow.find("#OPQty").css("border-color", "#ced4da");
        }

    })
    var Warehouse = $("#wh_id").val();
    if (Warehouse == "" || Warehouse == null || Warehouse == "0") {
        $("#wh_Error").text($("#valueReq").text());
        $("#wh_Error").css("display", "block");
        $("#wh_id").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        $("#wh_Error").css("display", "none");
        $("#wh_id").css("border-color", "#ced4da");
    }
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
    $("#OP_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var OPQty = clickedrow.find("#OPQty").val();
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
                    if (parseFloat(OPQty).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                        /*commented byHina on 05-02-2024 to validate by Eye color*/
                        //clickedrow.find("#BtnBatchDetail").css("border", "1px solid");
                        //clickedrow.find("#BtnBatchDetail").css("border-color", "#007bff");
                        ValidateEyeColor(clickedrow, "BtnBatchDetail", "N");
                    }
                    else {
                        /*commented byHina on 05-02-2024 to validate by Eye color*/
                        //clickedrow.find("#BtnBatchDetail").css("border", "2px solid");
                        //clickedrow.find("#BtnBatchDetail").css("border-color", "red");
                        ValidateEyeColor(clickedrow, "BtnBatchDetail", "Y");
                        BatchableFlag = "Y";
                        EmptyFlag = "N";
                    }
                }
                else {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#BtnBatchDetail").css("border", "2px solid");
                    //clickedrow.find("#BtnBatchDetail").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "BtnBatchDetail", "Y");
                    BatchableFlag = "Y";
                    EmptyFlag = "Y";
                }
            }
            else {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#BtnBatchDetail").css("border", "2px solid");
                //clickedrow.find("#BtnBatchDetail").css("border-color", "red");
                ValidateEyeColor(clickedrow, "BtnBatchDetail", "Y");
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
    $("#OP_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var OPQty = clickedrow.find("#OPQty").val();
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
                    if (parseFloat(OPQty).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                        /*commented byHina on 05-02-2024 to validate by Eye color*/
                        //clickedrow.find("#BtnSerialDetail").css("border", "1px solid");
                        //clickedrow.find("#BtnSerialDetail").css("border-color", "#007bff");
                        ValidateEyeColor(clickedrow, "BtnSerialDetail", "N");

                    }
                    else {
                        /*commented byHina on 05-02-2024 to validate by Eye color*/
                        //clickedrow.find("#BtnSerialDetail").css("border", "2px solid");
                        //clickedrow.find("#BtnSerialDetail").css("border-color", "red");
                        ValidateEyeColor(clickedrow, "BtnSerialDetail", "Y");
                        SerialableFlag = "Y";
                        EmptyFlag = "N";
                    }
                }
                else {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#BtnSerialDetail").css("border", "2px solid");
                    //clickedrow.find("#BtnSerialDetail").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "BtnSerialDetail", "Y");
                    SerialableFlag = "Y";
                    EmptyFlag = "Y";
                }
            }
            else {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#BtnSerialDetail").css("border", "2px solid");
                //clickedrow.find("#BtnSerialDetail").css("border-color", "red");
                ValidateEyeColor(clickedrow, "BtnSerialDetail", "Y");
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
function BindOPR_ItemList(e) {
    debugger;
    var RateDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();///Amount
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var EditDetail = sessionStorage.getItem("EditItemDetail");
    Itm_ID = $("#ItemName_1 option:selected").val();
    $("#hf_ItemID").val(Itm_ID);
    if (Itm_ID != "0") {
        $("#ItemNameError").css("display", "none");
        $("[aria-labelledby='select2-ItemName_1-container']").css("border-color", "#ced4da");
        $("#ItemCostError").css("display", "none");
        $("#ItemCost").css("border-color", "#ced4da");
    }
    if (Itm_ID == "0") {
        $("#ItemNameError").text($("#valueReq").text());
        return false;
    }
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/OpeningMaterialReceipt/GetItemUOM",
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
                        $("#UOM").val(arr.Table[0].uom_alias);
                        $("#UOMID").val(arr.Table[0].uom_id);
                        $("#hfbatchable").val(arr.Table[0].i_batch);
                        $("#hfserialable").val(arr.Table[0].i_serial);
                        $("#hfexpiralble").val(arr.Table[0].i_exp);
                        try {
                            HideShowPageWise(arr.Table[0].sub_item, clickedrow);
                        }
                        catch (ex) {
                            //console.log(ex);
                        }
                        if (EditDetail == "" || EditDetail == null) {
                            $("#ItemCost").val(parseFloat(arr.Table[0].cost_price).toFixed(RateDigit));
                        }

                        if (arr.Table[0].i_batch == "Y") {
                            $("#BtnBatchDetail").attr("disabled", false);
                        }
                        else {
                            $("#BtnBatchDetail").attr("disabled", true);
                        }
                        if (arr.Table[0].i_serial == "Y") {
                            $("#BtnSerialDetail").attr("disabled", false);
                        }
                        else {
                            $("#BtnSerialDetail").attr("disabled", true);
                        }
                        sessionStorage.removeItem("EditItemDetail");
                    }
                    else {
                        $("#UOM").val("");
                        $("#BtnSerialDetail").attr("disabled", true);
                        $("#BtnBatchDetail").attr("disabled", true);
                    }
                }
            },
        });
    } catch (err) {
    }
}
function OnChangeWarehouse() {
    debugger;
    var Warehouse = $("#wh_id").val();
    if (Warehouse == "" || Warehouse == null || Warehouse == "0") {
        $("#wh_Error").text($("#valueReq").text());
        $("#wh_Error").css("display", "block");
        $("#wh_id").css("border-color", "red");
        $("#ItemName_1").prop("disabled", true);
    }
    else {
        $("#wh_Error").css("display", "none");
        $("#wh_id").css("border-color", "#ced4da");
        $("#ItemName_1").prop("disabled", false);
        BindSampleRcptItmList(1);
        EnableTbleDetail();
    }
}
function OnChangeImportWarehouse() {
    debugger;
    var Warehouse = $("#importwhid").val();
    if (Warehouse == "" || Warehouse == null || Warehouse == "0") {
        $("#vmimportwhid_RequiredArea").text($("#valueReq").text());
        $("#vmimportwhid_RequiredArea").css("display", "block");
        $("#importwhid").css("border-color", "red");
    }
    else {
        $("#vmimportwhid_RequiredArea").css("display", "none");
        $("#importwhid").css("border-color", "#ced4da");
    }
}
function EnableTbleDetail() {
    $("#divAddNew").css("display", "block");
    $("#OP_ItmDetailsTbl >tbody >tr").each(function () {
        var SNo = $("#hfSNo").val();
        $("#ItemName_1").attr("disabled", false);
        $("#UOM").attr("disabled", true);
        $("#ItemCost").prop("readonly", false);
        $("#OPQty").prop("readonly", false);
        $("#BtnBatchDetail").attr("disabled", true);
        $("#BtnSerialDetail").attr("disabled", true);
    });
}

function AddNewRowForEditOPRItem() {
    debugger
    var rowIdx = 0;
    var RowNo = 0;

    $("#OP_ItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#hfSNo").val()) + 1;
    });
    if (RowNo == "0" || RowNo == "NaN") {
        RowNo = 1;
    }
    $('#OP_ItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class="red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td class="sr_padding"><span id="SpanRowId">${RowNo}</span><input  type="hidden" id="hfSNo" value="${RowNo}" /></td>
<td><div class=" col-sm-11 no-padding"><select class="form-control" id="ItemName_${RowNo}" ></select><input  type="hidden" id="hf_ItemID" />
<span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">  </button></div></td>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" name="idUOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly"><input id="UOMID" type="hidden" /><input type="hidden" id="hfbatchable" /><input type="hidden" id="hfserialable" /><input type="hidden" id="hfexpiralble" /></td>
<td><div class="lpo_form"><input id="ItemCost" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="ItemCost" placeholder="0000.00"  > <span id="ItemCostError" class="error-message is-visible"></span></div></td>
<td><div class="lpo_form"><input id="OPQty" class="form-control num_right" name="OPQty" autocomplete="off" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00"  ><span id="ReceivedQtyError" class="error-message is-visible"></span></div></td>
<td width="7%"><div class="lpo_form"><select class="form-control" id="wh_id_${RowNo}" onchange="OnChangeWarehouse(event);"><option value="0">---Select---</option></select><span id="wh_Error" class="error-message is-visible"></span></div></td>
<td><input id="LotNumber" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="${$("#span_LotNumber").text()}"  onblur="this.placeholder='${$("#span_LotNumber").text()}'" disabled></td>
<td><button type="button" class="calculator" id="BtnBatchDetail" onclick="OnClickBatchDetailBtn(event)" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button></td>
<td><button type="button" class="calculator" id="BtnSerialDetail" onclick="OnClickSerialDetailBtn(event)" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button></td>
</tr>`);
    //BindSampleRcptItmList(RowNo);
    //BindWarehouseList(RowNo);
};
function BindSampleRcptItmList(ID) {
    debugger;
    var wh_id = $("#wh_id").val();
    /*var rowId = document.getElementById("row_id_index").getAttribute("row_id");*/
    var rowId = $('#row_id_index').data('row-id');
    debugger;
    $("#ItemName_1").append("<option value='0' selected>---Select---</option>");
    DynamicSerchableItemDDL("#datatable-buttons", "#ItemName_", "1", "#tblhdn_item_id", "listToHide", "OML_WH" + "_" + wh_id + "_" + rowId)

    //    $("#ItemName_1").select2({

    //        ajax: {
    //            url: "/ApplicationLayer/OpeningMaterialReceipt/GetOPR_ItemList",
    //            data: function (params) {
    //                var queryParameters = {
    //                    SearchName: params.term,
    //                    wh_id: wh_id,
    //                    page: params.page || 1
    //                };
    //                return queryParameters;
    //            },
    //            multiple: true,
    //            cache: true,
    //            processResults: function (data, params) {
    //                debugger
    //                var pageSize,
    //                    pageSize = 2000; // or whatever pagesize
    //                results = [];
    //                let array = [];
    //                $("#datatable-buttons > tbody > tr").each(function () {
    //                    var currentRow = $(this);
    //                    debugger;
    //                    var itemId = currentRow.find("#tblhdn_item_id").val();
    //                    if (itemId != "0") {
    //                        array.push({ id: itemId });
    //                    }

    //                });
    //                var ItemListArrey = JSON.stringify(array);

    //                let selected = [];
    //                selected.push({ id: $("#ItemName_1").val() });
    //                selected = JSON.stringify(selected);

    //                var NewArrey = JSON.parse(ItemListArrey).filter(i => !selected.includes(i.id))

    //                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));

    //                if ($(".select2-search__field").parent().find("ul").length == 0) {
    //                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
    //<div class="row"><div class="col-md-8 col-xs-6 def-cursor">Item Name</div>
    //<div class="col-md-4 col-xs-6 def-cursor">UOM</div></div>
    //</strong></li></ul>`)
    //                }
    //                page = 1;
    //                data = data.slice((page - 1) * pageSize, page * pageSize);
    //                if (data[0] != null) {
    //                    if (data[0].Name.trim() != "---Select---") {
    //                        var select = { ID: "0_0", Name: " ---Select---" };
    //                        data.unshift(select);
    //                    }
    //                }
    //                return {
    //                    results: $.map(data, function (val, Item) {
    //                        return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
    //                    }),
    //                };
    //            },
    //            cache: true
    //        },
    //        templateResult: function (data) {
    //            debugger
    //            var classAttr = $(data.element).attr('class');
    //            var hasClass = typeof classAttr != 'undefined';
    //            classAttr = hasClass ? ' ' + classAttr : '';
    //            var $result;
    //            $result = $(
    //                '<div class="row">' +
    //                '<div class="col-md-8 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                '<div class="col-md-4 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
    //                '</div>'
    //            );
    //            return $result;
    //            firstEmptySelect = false;
    //        },
    //    });
}
function AddNewRow() {
    debugger;
    var ErrorFlag = "N";
    if (CheckOPR_ItemValidations() == false) {
        return false;
    }
    //Commented by Suraj Maurya on 21-08-2025
    //$("#datatable-buttons #OpeningItemDetail tr").each(function () {
    //    var Rows = $(this);
    //    debugger;
    //    if (Rows.find("#tblhdn_item_id").val() == $("#ItemName_1").val()) {
    //        $("#ItemNameError").text($("#valueduplicate").text());
    //        $("#ItemNameError").css("display", "block");
    //        $("[aria-labelledby='select2-ItemName_1-container']").css("border-color", "red");
    //        ErrorFlag = "Y";
    //    }
    //});

    //Added by Suraj Maurya on 21-08-2025
    $('#datatable-buttons').DataTable().rows().data().toArray().map((item) => {
        if (item[1] == $("#ItemName_1").val()) {
            $("#ItemNameError").text($("#valueduplicate").text());
            $("#ItemNameError").css("display", "block");
            $("[aria-labelledby='select2-ItemName_1-container']").css("border-color", "red");
            ErrorFlag = "Y";
        }
    });

    if (ErrorFlag == "Y") {
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
    var ItemName = $("#ItemName_1").val();
    var OPQty = $("#OPQty").val();
    var sub_item = $("#sub_item").val();
    //Cmn_ApendToHdnSubItemTable("Quantity");
    var subitmFlag = CheckValidations_forSingleItem_SubItems(ItemName, OPQty, sub_item, "SubItemOPQty");
    if (subitmFlag == false) {
        return false;
    }

    //var TblLen = $("#datatable-buttons TBODY TR").length;
    var TblLen = $('#datatable-buttons').DataTable().rows().data().toArray().length;
    if (TblLen == 1) {
        var sno = $("#spanrowid").text();
        if (sno != "" && sno != null) {
            rowIdx = TblLen + 1;
        }
        else {
            rowIdx = TblLen;
        }
    }
    else {
        rowIdx = TblLen + 1;
    }
    debugger;

    var rowId = '';
    var ItemID = $("#hf_ItemID").val();
    var UOM = $("#UOM").val();
    var UOMID = $("#UOMID").val();
    var Batchable = $("#hfbatchable").val();
    var Seriable = $("#hfserialable").val();
    var Expirable = $("#hfexpiralble").val();
    var LotNumber = $("#LotNumber").val();

    var ItemCost = $("#ItemCost").val();
    var OpValue = $("#OpValue").val();
    var sub_item = $("#tbodyGRNList > tr #sub_item").val();
    var subitmDisable = "";
    if (sub_item != "Y") {
        subitmDisable = "disabled";
    }
    $("#SubItemOPQty").attr("disabled", true);

    var t = $('#datatable-buttons').DataTable();
    //var t = $('#datatable-buttons').DataTable({
    //    "ordering": true,
    //    "destroy": true,
    //    "aaSorting": [[4, 'asc']],
    //    "columnDefs": [
    //        { orderable: true, targets: '_all' }
    //    ]
    //});

    var span_SubItemDetail = $("#span_SubItemDetail").text();
    if (ItemID != "" && OPQty != "" && ItemCost != "0") {
        t.row.add([
            rowId,
            ItemID,
            $("#UOMID").val(),
            $("#OPQty").val(),
            $("#LotNumber").val(),
            $("#ItemCost").val(),
            $("#OpValue").val(),
            `<td><div class="red center"><i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" onclick="deleteRow(this, event,'${ItemID}')"></i></div></td>`,
            `<td><div class="center edit_icon"><i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" onclick="editItem(this, event,'${ItemID}')" title="${$("#Edit").text()}"></i></div></td>`,
            `<td><span id ="spanrowid">${rowIdx}</span></td>`,
            `<td><div class="col-sm-12 no-padding">${$("#ItemName_1 option:selected").text()}<input type="hidden" id="tblhdn_item_id" value="${ItemID}" style="display:none;"><input type="hidden" id="tblhdn_item_name" value="${$("#ItemName_1 option:selected").text()}" style="display:none;"></div></td>`,
            `<td >${$("#UOM").val()}<input type="hidden" id="tblhdn_uom_id" value="${$("#UOMID").val()}" style="display:none;"><input type="hidden" id="tblhdn_uom_alias" value="${$("#UOM").val()}" style="display:none;"><input type="hidden" id="tblhdn_batchable_id" value="${$("#hfbatchable").val()}" style="display:none;"><input type="hidden" id="tblhdn_serialable_id" value="${$("#hfserialable").val()}" style="display:none;"><input type="hidden" id="tblhdn_expiralble_id" value="${$("#hfexpiralble").val()}" style="display:none;"><input hidden id="SpanItemRate" value="${$("#ItemCost").val()}" /><input hidden id="SpanLotNumber" value="${$("#LotNumber").val()}" /></td>`,
            `<td class="num_right">
<div class=" col-sm-9 num_right">
<span id="SpanOpQty">${$("#OPQty").val()}</span>
<input type="hidden" id="TxtOpQty" value="${$("#OPQty").val()}">
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemOPQty">
    <input hidden type="text" id="sub_item" value="${sub_item}" />
    <button type="button" id="SubItemOPQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('OpSubitmQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
</div>
<div class="col-sm-1 i_Icon">
          <button type="button" id="BatchSerialNumber" class="calculator item_pop" data-toggle="modal" onclick="OnClickOpeningQuantityIcon(this,event);" data-target="#BatchSerialNumberpop" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title=""></button>
            </div></td>`,
            `<td><div class="num_right"><span id="SpanOp_val">${$("#OpValue").val()}</span></div></td>`,
            $("#hfbatchable").val(),
            $("#hfserialable").val(),
            $("#hfexpiralble").val(),
            $("#ItemName_1 option:selected").text(),
            sub_item
        ]).draw();

        HideItemListItm();
        t.column(0).visible(false)
        t.column(1).visible(false)
        t.column(2).visible(false)
        t.column(3).visible(false)
        t.column(4).visible(false)
        t.column(5).visible(false)
        t.column(6).visible(false)
        t.column(14).visible(false)
        t.column(15).visible(false)
        t.column(16).visible(false)
        t.column(17).visible(false)
        t.column(18).visible(false)
        //t.column(13).visible(false)
        //$("#datatable-buttons >tbody >tr").each(function () {//commented by Suraj on 05-10-2024, because it will work on only front page columns
        //    var currentRow = $(this);
        //    currentRow.find("#delBtnIcon").addClass("red");

        //    //currentRow.find("td:eq(0)").attr("hidden", true);
        //    //currentRow.find("td:eq(1)").attr("hidden", true);
        //    //currentRow.find("td:eq(2)").attr("hidden", true);
        //    //currentRow.find("td:eq(3)").attr("hidden", true);
        //    //currentRow.find("td:eq(4)").attr("hidden", true);
        //    //currentRow.find("td:eq(5)").attr("hidden", true);
        //    currentRow.find("#SpanLotNumber").parent().css("display", "none");
        //    currentRow.find("#SpanItemRate").parent().parent().css("display", "none");
        //});
    }
    itmidArr.push(ItemID);
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var Opening_val = $("#OpeningValue").val();
    if (Opening_val != "") {
        var Opening_valitem = parseFloat($("#OpValue").val());
        var total = parseFloat(Opening_val) + parseFloat(Opening_valitem);
        var FinalTotal = parseFloat(total).toFixed(ValDecDigit);
        $("#OpeningValue").val(FinalTotal);
    }
    else {
        var Opening_valitem = parseFloat($("#OpValue").val());
        $("#OpeningValue").val(Opening_valitem);
    }
    ResetEntryDetails();
};
function ResetEntryDetails() {
    $("#OPQty").val("");
    $("#ItemCost").val("");
    $("#OpValue").val("");
    $("#ItemName_1").val("0").change();
    $("#UOM").val("");
    $("#wh_id").attr("disabled", true);
    $("#divUpdate").css("display", "none");
}
function HideItemListItm() {
    debugger;
    var newTableArr = $('#datatable-buttons').DataTable().rows().data().toArray();//Added by suraj on 05-10-2024
    newTableArr.map((item) => {
        $("#ItemName_1 option[value=" + item[1] + "]").select2().hide();
        $('#ItemName_1').val("0").change();
    });
    //$("#datatable-buttons >tbody >tr").each(function (j, rows) {
    //    var currentRowChild = $(this);
    //    debugger;
    //    var item_id = currentRowChild.find("#tblhdn_item_id").val();//recheck                

    //    $("#ItemName_1 option[value=" + item_id + "]").select2().hide();
    //    $('#ItemName_1').val("0").change();
    //});
}
function Insert_OpeningItem_Detail() {
    debugger;
    var rowcount = $('#datatable-buttons tr td').length;
    var DecDigit = $("#ValDigit").text();

    var Warehouse = $("#wh_id").val();
    if (Warehouse == "" || Warehouse == null || Warehouse == "0") {

        $("#wh_Error").text($("#valueReq").text());
        $("#wh_Error").css("display", "block");
        $("#wh_id").css("border-color", "red");

        return false;
    }
    else {
        $("#wh_Error").css("display", "none");
        $("#wh_id").css("border-color", "#ced4da");

        debugger;
    }
    if (rowcount > 2) {
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        return false;
    }
}
function OnClickBatchDetailBtn(e) {
    debugger;
    Cmn_OnClickBatchDetailBtn_New(e, "#hfSNo", "#ItemName_1", "#hf_ItemID", "#UOM", "#hfexpiralble", "#OPQty", "OPREnableDisable", "");
}
function OnClickSerialDetailBtn(e) {
    debugger;
    Cmn_OnClickSerialDetailBtn_New(e, "#hfSNo", "#ItemName_1", "#hf_ItemID", "#UOM", "#OPQty", "OPREnableDisable", "");

}
function OnKeyDownBatchNoGRN(e) {//Copied from GRN
    debugger
    if (Cmn_Block_Keys(e, "Space") == false) {
        return false;
    }
}
function OnChangeGRNBatchNo(e) {
    var data = Cmn_RemoveSpacesAndTabs(e.target.value);
    $("#txtBatchNumber").val(data)
}
function deleteRow(el, e, option) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();

    // Use closest to get the row and DataTable to remove it
    var clickedrow = $(e.target).closest("tr");

    var hdn_Itm_ID = clickedrow.find("#tblhdn_item_id").val();
    var Opening_valitem = parseFloat(clickedrow.find("#SpanOp_val").text());

    // Initialize DataTable
    var table = $('#datatable-buttons').DataTable();

    // Get the row index using DataTables API to avoid issues with row reordering
    var row = table.row(clickedrow);
    row.remove().draw(false);

    // Update the select dropdown options
    /* //unnecessary code commnted by suraj maurya on 21-08-2025
    $("#ItemName_1 option[value=" + hdn_Itm_ID + "]").removeClass("select2-hidden-accessible");
    
    */
    $('#ItemName_1').val("0").change();
    // Clear input fields
    $("#OPQty").val("");
    $("#ItemCost").val("");
    $("#OpValue").val("");
    $("#ItemName_1").attr("disabled", false);
    $("#UOM").val("");
    $("#wh_id").attr("disabled", true);

    // Show and hide appropriate divs
    $("#divAddNew").show();
    $("#divUpdate").hide();

    // Update the opening value
    var ValDecDigit = $("#ValDigit").text();
    var Opening_val = parseFloat($("#OpeningValue").val());
    var total = Opening_val - Opening_valitem;
    var FinalTotal = parseFloat(total).toFixed(ValDecDigit);
    $("#OpeningValue").val(FinalTotal);

    // Call additional functions if necessary
    if (hdn_Itm_ID != "" && hdn_Itm_ID != null && hdn_Itm_ID != "0") {
        // After delete functions (adjust as needed)
        AfterDeleteResetBatchAndSerialDetails(hdn_Itm_ID);
        Cmn_DeleteSubItemQtyDetail(hdn_Itm_ID);
    }
    SerialNoAfterDelete();
}

//function deleteRow(el, e, option) {
//    debugger;
//    var QtyDecDigit = $("#QtyDigit").text();
//    var RateDigit = $("#RateDigit").text();
//    var ValDigit = $("#ValDigit").text();
//    var i = el.parentNode.parentNode.rowIndex - 1;

//    var clickedrow = $(e.target).closest("tr");
//    var hdn_Itm_ID = clickedrow.find("#tblhdn_item_id").val();

//    var Opening_valitem = parseFloat(clickedrow.find("#SpanOp_val").text());
//    var table = $('#datatable-buttons').DataTable();
//    table.row(i).remove().draw(false);

//    $("#ItemName_1 option[value=" + hdn_Itm_ID + "]").removeClass("select2-hidden-accessible");
//    $('#ItemName_1').val("0").change();
//    $("#ItemName_1 option[value=" + hdn_Itm_ID + "]").addClass("select2-hidden-accessible");


//    $("#OPQty").val("");
//    $("#ItemCost").val("");
//    $("#OpValue").val("");
//    $("#ItemName_1").attr("disabled", false);
//    $("#UOM").val("");
//    $("#wh_id").attr("disabled", true);

//    $("#divAddNew").show();
//    $("#divUpdate").hide();
//    debugger;
//    debugger;
//    var ValDecDigit = $("#ValDigit").text();
//    var Opening_val = parseFloat($("#OpeningValue").val());

//    var total = Opening_val - Opening_valitem;
//    var FinalTotal = parseFloat(total).toFixed(ValDecDigit);
//    $("#OpeningValue").val(FinalTotal);



//    if (hdn_Itm_ID != "" && hdn_Itm_ID != null && hdn_Itm_ID != "0") {
//        // ShowItemListItm(hdn_Itm_ID);
//        AfterDeleteResetBatchAndSerialDetails(hdn_Itm_ID);
//        Cmn_DeleteSubItemQtyDetail(hdn_Itm_ID);
//    }
//    SerialNoAfterDelete();
//};
function AfterDeleteResetBatchAndSerialDetails(ItemCode) {
    debugger;
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
function SerialNoAfterDelete() {
    var SerialNo = 0;
    debugger

    $("#datatable-buttons >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#spanrowid").text(SerialNo);

    });
};
function editItem(el, e, option) {
    debugger;
    sessionStorage.setItem("EditItemDetail", "Y");
    $("#ItemName_1").prop("disabled", true);
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var rowJavascript = el.parentNode.parentNode;
    var clickedrow = $(e.target).closest("tr");
    $('#hdnUpdateInTable').val(rowJavascript._DT_CellIndex.row);
    var hdn_item_id = clickedrow.find("#tblhdn_item_id").val();
    var hdn_item_name = clickedrow.find("#tblhdn_item_id").parent().text();

    $("#vmddl_item_id").css("display", "none");
    $("[aria-labelledby='select2-ItemName_1-container']").css("border-color", "#ced4da");

    var Batchable = $("#hfbatchable").val();
    var Seriable = $("#hfserialable").val();
    var Expirable = $("#hfexpiralble").val();
    var LotNumber = $("#LotNumber").val();
    var UOM = clickedrow.find("td:eq(5)").text();
    $("#UOM").val(UOM);

    $("#hf_ItemID").val(hdn_item_id);

    var hdn_uom_id = clickedrow.find("#tblhdn_uom_id").val();
    $("#UOMID").val(hdn_uom_id);

    var OPQty = clickedrow.find("#SpanOpQty").text();
    var FinalOPQty = parseFloat(OPQty).toFixed(RateDigit);
    $("#OPQty").val(FinalOPQty);
    var OpValue = clickedrow.find("#SpanOp_val").text();
    var FinalOpValue = parseFloat(OpValue).toFixed(RateDigit);
    $("#OpValue").val(FinalOpValue);

    sessionStorage.setItem("ItemOPVal", FinalOpValue);
    $('#ItemName_1').append('<option value="' + hdn_item_id + '" selected>' + hdn_item_name + '</option>').change();
    $('#ItemName_1').change();
    $("#divAddNew").hide();
    $("#divUpdate").show();
    var ItemCost = clickedrow.find("#SpanItemRate").val();
    var FinalItemCost = parseFloat(ItemCost).toFixed(RateDigit);
    $("#ItemCost").val(FinalItemCost);
    $("#HdnBatchData").text("Y");
    if (!itmidArr.includes(hdn_item_id)) {
        itmidArr.push(hdn_item_id);
    }
};



function OnClickOpeningQuantityIcon(el, evt) {
    try {
        debugger;
        var op_dt = $("#txtFromdate").val();
        var wh_id = $("#wh_id").val();

        var clickedrow = $(evt.target).closest("tr");
        var OpQty = clickedrow.find("#SpanOpQty").text();
        var OpVal = clickedrow.find("#SpanOp_val").text();
        // var ItemName = clickedrow.find("td:eq(3)").text();
        var ItemName = clickedrow.find("#tblhdn_item_name").val();
        var UOMName = clickedrow.find("#tblhdn_uom_alias").val();
        var ItemID = clickedrow.find("#tblhdn_item_id").val();
        var Batchable = clickedrow.find("#tblhdn_batchable_id").val();
        var Serialable = clickedrow.find("#tblhdn_serialable_id").val();
        var rate1 = clickedrow.find("#SpanItemRate").val();
        var flag = Batchable + Serialable;
        $("#ItemNameBatchWise").val(ItemName);
        $("#UOMBatchWise").val(UOMName);
        $("#popBatchReceivedQty").val(OpQty);
        $("#pop_openingvalue").val(OpVal);
        var bat = $("#BatchSpanDetail").text();
        var ser = $("#SerialSpanDetail").text();
        var expdate = $("#ExpirySpanDetail").text();
   
        //var lbsdetails = $("#HdnLotBatchSerialDetail").val();
        //var _L_B_S_Details = JSON.parse(lbsdetails);

        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/OpeningMaterialReceipt/GetOpeningQuantityDetails",
                data: { Op_dt: op_dt, Wh_id: wh_id, Item_ID: ItemID, Flag: flag },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        Error_Page();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0 && !itmidArr.includes(ItemID)) {

                            $("#BatchDetailTbl1 >tbody >tr").remove();
                            var totalqty = 0;
                            var totalvalue = 0;
                            for (var j = 0; j < arr.Table.length; j++) {

                                var item_id = arr.Table[j].item_id;
                                var lot_no = arr.Table[j].lot_id;
                                var batchserial_no = arr.Table[j].batch_serial_no;
                                var qty = arr.Table[j].Qty;
                                var rate = arr.Table[j].rate;
                                var ex_dt = arr.Table[j].ex_dt;
                                var value = arr.Table[j].Val;
                                var MfgName = arr.Table[j].mfg_name || '';
                                var MfgMrp = arr.Table[j].mfg_mrp || '';
                                var MfgDate = arr.Table[j].mfg_date || '';

                                totalqty = parseFloat(totalqty) + parseFloat(qty);
                                totalvalue = parseFloat(totalvalue) + parseFloat(value);

                                if (Batchable == "Y" && Serialable == "N") {

                                    $("#Headingbatchserial").text(bat);
                                    $("#Headingbatchserialdetail").text(bat);
                                    $("#expdatedetail").text(expdate);
                                    RenderHtmlForItemBatchSerialDetails(item_id, lot_no, batchserial_no, "", MfgName, MfgMrp, MfgDate, ex_dt, qty, rate, value);
                             //       $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none" id="item_id1">${item_id}</td>
                             //   <td>${lot_no}</td>
                             //   <td id="BatchNo1">${batchserial_no}</td>
                             //   <td></td>
                             //   <td>${MfgName}</td>
                             //   <td class="num_right">${MfgMrp}</td>
                             //   <td>${MfgDate}</td>
                             //   <td>${ex_dt}</td>
                             //   <td class="num_right">${parseFloat(qty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                }
                                if (Batchable == "N" && Serialable == "Y") {
                                    $("#Headingbatchserial").text(ser);
                                    $("#Headingbatchserialdetail").text(ser);
                                    $("#expdatedetail").text("");
                                    RenderHtmlForItemBatchSerialDetails(item_id, lot_no, "", batchserial_no, MfgName, MfgMrp, MfgDate, ex_dt, qty, rate, value);
                             //       $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none" id="item_id1">${item_id}</td>
                             //   <td>${lot_no}</td>
                             //   <td></td>
                             //   <td id="BatchNo1">${batchserial_no}</td>
                             //   <td>${MfgName}</td>
                             //   <td class="num_right">${MfgMrp}</td>
                             //   <td>${MfgDate}</td>
                             //   <td>${ex_dt}</td>
                             //   <td class="num_right">${parseFloat(qty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                }
                                if (Batchable == "N" && Serialable == "N") {
                                    RenderHtmlForItemBatchSerialDetails(item_id, lot_no, "", "", MfgName, MfgMrp, MfgDate, ex_dt, qty, rate, value);
                             //       $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${item_id}</td>
                             //   <td>${lot_no}</td>
                             //   <td></td>
                             //   <td></td>
                             //   <td>${MfgName}</td>
                             //   <td class="num_right">${MfgMrp}</td>
                             //   <td>${MfgDate}</td>
                             //   <td></td>
                             //   <td class="num_right">${parseFloat(qty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                }
                            }

                            $("#span_opqty").text(parseFloat(totalqty).toFixed(QtyDecDigit));
                            $("#span_opvalue").text(parseFloat(totalvalue).toFixed(ValDecDigit));

                            //if ($("#BatchDetailTbl >tbody >tr").length != arr.Table.length && $("#BatchDetailTbl >tbody >tr").length!="0") {
                            $("#BatchDetailTbl >tbody >tr").remove();
                            var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
                            if (FBatchDetails != null) {
                                // $("#BatchDetailTbl1 >tbody >tr").remove();
                                //if ($("#HdnBatchData").text() == "N") {
                                //    $("#BatchDetailTbl1 >tbody >tr").remove();
                                //    $("#HdnBatchData").text(null);
                                //}
                                let itemIdArr = [];
                                itemIdArr.push({ ItemID: ItemID });
                                itemIdArr = JSON.stringify(itemIdArr);

                                FBatchDetails = FBatchDetails.filter(j => itemIdArr.includes(j.ItemID));
                                //if (FBatchDetails.length <= arr.Table.length) {
                                //    if ($("#HdnBatchData").text() == "N") {
                                //        $("#BatchDetailTbl1 >tbody >tr").remove();
                                //        $("#HdnBatchData").text(null);
                                //    }                                      
                                //}
                                if (FBatchDetails.length != arr.Table.length && FBatchDetails.length != "0") {
                                    //if (FBatchDetails.length != "0") {
                                    $("#BatchDetailTbl1 >tbody >tr").remove();
                                    if (FBatchDetails.length > 0) {
                                        for (i = 0; i < FBatchDetails.length; i++) {
                                            var SItemID = FBatchDetails[i].ItemID;
                                            var batchserial_no = FBatchDetails[i].BatchNo;
                                            var BatchQty = FBatchDetails[i].BatchQty;
                                            var BatchExDate = FBatchDetails[i].BatchExDate;
                                            var MfgName = FBatchDetails[i].mfg_name || '';
                                            var MfgMrp = FBatchDetails[i].mfg_mrp || '';
                                            var MfgDate = FBatchDetails[i].mfg_date || '';
                                            //var rate = $("#SpanItemRate").text();
                                            var value = BatchQty * rate1;
                                            var lot_no = "";
                                            var btflag = "N";
                                            $("#BatchDetailTbl1 >tbody >tr").each(function () {
                                                var currentRow = $(this);
                                                debugger;
                                                var item_id1 = currentRow.find("#item_id1").text();
                                                var batchserial_no1 = currentRow.find("#BatchNo1").text();
                                                if (batchserial_no == batchserial_no1 && SItemID == item_id1) {
                                                    btflag = "Y";
                                                }
                                            });
                                            if (btflag == "N") {
                                                if (Batchable == "Y" && Serialable == "N") {
                                                    $("#Headingbatchserial").text(bat);
                                                    $("#Headingbatchserialdetail").text(bat);
                                                    $("#expdatedetail").text(expdate);
                                                    RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, batchserial_no, "", MfgName, MfgMrp, MfgDate, BatchExDate, BatchQty, rate1, value);
                             //                       $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td>${batchserial_no}</td>
                             //   <td></td>
                             //   <td>${MfgName}</td>
                             //   <td class="num_right">${MfgMrp}</td>
                             //   <td>${MfgDate}</td>
                             //   <td>${BatchExDate}</td>
                             //   <td class="num_right">${parseFloat(BatchQty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                                }
                                                if (Batchable == "N" && Serialable == "Y") {
                                                    $("#Headingbatchserial").text(ser);
                                                    $("#Headingbatchserialdetail").text(ser);
                                                    $("#expdatedetail").text("");
                                                    RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, "", batchserial_no, MfgName, MfgMrp, MfgDate, BatchExDate, BatchQty, rate1, value);
                             //                       $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td></td>
                             //   <td>${batchserial_no}</td>
                             //   <td>${MfgName}</td>
                             //   <td class="num_right">${MfgMrp}</td>
                             //   <td>${MfgDate}</td>
                             //   <td>${BatchExDate}</td>
                             //   <td class="num_right">${parseFloat(BatchQty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                                }
                                                if (Batchable == "N" && Serialable == "N") {
                                                    RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, "", "", MfgName, MfgMrp, MfgDate, "", BatchQty, rate1, value);
                             //                       $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td></td>
                             //   <td></td>
                             //   <td>${MfgName}</td>
                             //   <td class="num_right">${MfgMrp}</td>
                             //   <td>${MfgDate}</td>
                             //   <td></td>
                             //   <td class="num_right">${parseFloat(BatchQty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                                }
                                            }
                                        }
                                    }
                                }
                                $("#span_opqty").text(parseFloat(OpQty).toFixed(QtyDecDigit));
                                $("#span_opvalue").text(parseFloat(OpVal).toFixed(ValDecDigit));
                            }
                            var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
                            if (FSerialDetails != null) {
                                let itemIdArr = [];
                                itemIdArr.push({ ItemID: ItemID });
                                itemIdArr = JSON.stringify(itemIdArr);

                                FSerialDetails = FSerialDetails.filter(j => itemIdArr.includes(j.ItemID));
                                if (FSerialDetails.length != arr.Table.length && FSerialDetails.length != "0") {
                                    $("#BatchDetailTbl1 >tbody >tr").remove();
                                    if (FSerialDetails.length > 0) {
                                        for (i = 0; i < FSerialDetails.length; i++) {
                                            var SItemID = FSerialDetails[i].ItemID;
                                            var batchserial_no = FSerialDetails[i].SerialNo;
                                            var MfgName = FBatchDetails[i].MfgName;
                                            var MfgMrp = FBatchDetails[i].MfgMrp;
                                            var MfgDate = FBatchDetails[i].MfgDate;
                                            var qty = "1";
                                            var BatchExDate = "";

                                            var value = qty * rate1;
                                            var lot_no = "";
                                            var btflag = "N";

                                            totalqty = parseFloat(totalqty) + parseFloat(qty);
                                            totalvalue = parseFloat(totalvalue) + parseFloat(value);
                                            if (Batchable == "Y" && Serialable == "N") {

                                                $("#Headingbatchserial").text(bat);
                                                $("#Headingbatchserialdetail").text(bat);
                                                $("#expdatedetail").text(expdate);
                                                RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, batchserial_no, "", MfgName, MfgMrp, MfgDate, BatchExDate, qty, rate1, value);
                             //                   $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td>${batchserial_no}</td>
                             //   <td></td>
                             //   <td>${MfgName}</td>
                             //   <td class="num_right">${MfgMrp}</td>
                             //   <td>${MfgDate}</td>
                             //   <td>${BatchExDate}</td>
                             //   <td class="num_right">${parseFloat(qty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                            }
                                            if (Batchable == "N" && Serialable == "Y") {
                                                $("#Headingbatchserial").text(ser);
                                                $("#Headingbatchserialdetail").text(ser);
                                                $("#expdatedetail").text("");
                                                RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, "", batchserial_no, MfgName, MfgMrp, MfgDate, BatchExDate, qty, rate1, value);
                             //                   $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td></td>
                             //   <td>${SerialNo}</td>
                             //   <td>${MfgName}</td>
                             //   <td class="num_right">${MfgMrp}</td>
                             //   <td>${MfgDate}</td>
                             //   <td>${BatchExDate}</td>
                             //   <td class="num_right">${parseFloat(qty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                            }
                                            if (Batchable == "N" && Serialable == "N") {
                                                RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, "", "", MfgName, MfgMrp, MfgDate, "", qty, rate1, value);
                             //                   $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td></td>
                             //   <td></td>
                             //   <td>${MfgName}</td>
                             //   <td class="num_right">${MfgMrp}</td>
                             //   <td>${MfgDate}</td>
                             //   <td></td>
                             //   <td class="num_right">${parseFloat(qty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                            }
                                        }
                                        $("#span_opqty").text(parseFloat(OpQty).toFixed(QtyDecDigit));
                                        $("#span_opvalue").text(parseFloat(OpVal).toFixed(ValDecDigit));
                                    }
                                }
                            }
                            //}


                            //if (localStorage.getItem("flag_lot") === "true") {
                            //    $('#BatchDetailTbl1 >tbody').html('');
                            //    $('#BatchDetailTbl1 >tbody').append(`<tr>
                            //                  <td style="display:none">${SItemID}</td>
                            //                  <td>${''}</td>
                            //                  <td></td>
                            //                  <td></td>
                            //                  <td></td>
                            //                  <td class="num_right">${parseFloat(OpQty).toFixed(QtyDecDigit)}</td>
                            //                  <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                            //                  <td class="num_right">${parseFloat(OpVal).toFixed(ValDecDigit)}</td>
                            //                  </tr>`);
                            //    $("#span_opqty").text(parseFloat(OpQty).toFixed(QtyDecDigit));
                            //    $("#span_opvalue").text(parseFloat(OpVal).toFixed(ValDecDigit));
                            //    localStorage.setItem("flag_lot", false);
                            //}
                        }
                        else
                        {
                            var rowIdx = 0;
                            var totalqty = 0;
                            var totalvalue = 0;
                            $("#BatchDetailTbl1 >tbody >tr").remove();

                            var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
                            if (FBatchDetails != null) {


                                let itemIdArr = [];
                                itemIdArr.push({ ItemID: ItemID });
                                itemIdArr = JSON.stringify(itemIdArr);

                                FBatchDetails = FBatchDetails.filter(j => itemIdArr.includes(j.ItemID));
                                if (FBatchDetails.length > 0) {
                                    for (i = 0; i < FBatchDetails.length; i++) {
                                        var SItemID = FBatchDetails[i].ItemID;
                                        var batchserial_no = FBatchDetails[i].BatchNo;
                                        var BatchQty = FBatchDetails[i].BatchQty;
                                        var BatchExDate = Cmn_FormatDate_ddmmyyyy(FBatchDetails[i].BatchExDate);

                                        var MfgName = FBatchDetails[i].mfg_name || '';
                                        var MfgMrp = FBatchDetails[i].mfg_mrp || '';
                                        var MfgDate = FBatchDetails[i].mfg_date || '';
                                        var value = BatchQty * rate1;
                                        var lot_no = "";
                                        var btflag = "N";

                                        totalqty = parseFloat(totalqty) + parseFloat(BatchQty);
                                        totalvalue = parseFloat(totalvalue) + parseFloat(value);
                                        if (Batchable == "Y" && Serialable == "N") {

                                            $("#Headingbatchserial").text(bat);
                                            $("#Headingbatchserialdetail").text(bat);
                                            $("#expdatedetail").text(expdate);
                                            RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, batchserial_no, "", MfgName, MfgMrp, MfgDate, BatchExDate, BatchQty, rate1, value);
                             //               $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td>${batchserial_no}</td>
                             //   <td></td>
                             //   <td>${BatchExDate}</td>
                             //   <td class="num_right">${parseFloat(BatchQty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                        }
                                        if (Batchable == "N" && Serialable == "Y") {
                                            $("#Headingbatchserial").text(ser);
                                            $("#Headingbatchserialdetail").text(ser);
                                            $("#expdatedetail").text("");
                                            RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, "", batchserial_no, MfgName, MfgMrp, MfgDate, BatchExDate, BatchQty, rate1, value);
                             //               $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td></td>
                             //   <td>${batchserial_no}</td>
                             //   <td>${BatchExDate}</td>
                             //   <td class="num_right">${parseFloat(BatchQty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                        }
                                        if (Batchable == "N" && Serialable == "N") {
                                            RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, "", "", MfgName, MfgMrp, MfgDate, "", BatchQty, rate1, value);
                             //               $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${Temasfsdf}</td>
                             //   <td></td>
                             //   <td></td>
                             //   <td></td>
                             //   <td class="num_right">${parseFloat(BatchQty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                        }
                                        //});
                                    }
                                }
                                $("#span_opqty").text(parseFloat(OpQty).toFixed(QtyDecDigit));
                                $("#span_opvalue").text(parseFloat(OpVal).toFixed(ValDecDigit));
                            }
                            var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
                            if (FSerialDetails != null) {
                                let itemIdArr = [];
                                itemIdArr.push({ ItemID: ItemID });
                                itemIdArr = JSON.stringify(itemIdArr);

                                FSerialDetails = FSerialDetails.filter(j => itemIdArr.includes(j.ItemID));
                                if (FSerialDetails.length > 0) {
                                    for (i = 0; i < FSerialDetails.length; i++) {
                                        var SItemID = FSerialDetails[i].ItemID;
                                        var batchserial_no = FSerialDetails[i].SerialNo;
                                        var MfgName = FSerialDetails[i].MfgName;
                                        var MfgMrp = FSerialDetails[i].MfgMrp;
                                        var MfgDate = FSerialDetails[i].MfgDate;
                                        var qty = "1";
                                        var BatchExDate = "";

                                        var value = qty * rate1;
                                        var lot_no = "";
                                        var btflag = "N";

                                        totalqty = parseFloat(totalqty) + parseFloat(qty);
                                        totalvalue = parseFloat(totalvalue) + parseFloat(value);
                                        if (Batchable == "Y" && Serialable == "N") {

                                            $("#Headingbatchserial").text(bat);
                                            $("#Headingbatchserialdetail").text(bat);
                                            $("#expdatedetail").text(expdate);
                                            RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, batchserial_no, "", MfgName, MfgMrp, MfgDate, BatchExDate, qty, rate1, value);
                             //               $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td>${batchserial_no}</td>
                             //   <td></td>
                             //   <td>${BatchExDate}</td>
                             //   <td class="num_right">${parseFloat(qty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                        }
                                        if (Batchable == "N" && Serialable == "Y") {
                                            $("#Headingbatchserial").text(ser);
                                            $("#Headingbatchserialdetail").text(ser);
                                            $("#expdatedetail").text("");
                                            RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, "", batchserial_no, MfgName, MfgMrp, MfgDate, BatchExDate, qty, rate1, value);
                             //               $('#BatchDetailTbl1 >tbody').append(`<tr>
                             //   <td style="display:none">${SItemID}</td>
                             //   <td>${lot_no}</td>
                             //   <td></td>
                             //   <td>${SerialNo}</td>
                             //   <td>${BatchExDate}</td>
                             //   <td class="num_right">${parseFloat(qty).toFixed(QtyDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                             //   <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                             //</tr>`);
                                        }
                                        if (Batchable == "N" && Serialable == "N") {
                                            RenderHtmlForItemBatchSerialDetails(SItemID, lot_no, "", "", MfgName, MfgMrp, MfgDate, "", qty, rate1, value);
                                            //$('#BatchDetailTbl1 >tbody').append(`<tr>
                                            //  <td style="display:none">${SItemID}</td>
                                            //  <td>${lot_no}</td>
                                            //  <td></td>
                                            //  <td></td>
                                            //  <td></td>
                                            //  <td class="num_right">${parseFloat(qty).toFixed(QtyDecDigit)}</td>
                                            //  <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                                            //  <td class="num_right">${parseFloat(value).toFixed(ValDecDigit)}</td>
                                            //  </tr>`);
                                        }
                                    }
                                    $("#span_opqty").text(parseFloat(OpQty).toFixed(QtyDecDigit));
                                    $("#span_opvalue").text(parseFloat(OpVal).toFixed(ValDecDigit));
                                }
                            }


                            if (Batchable == "N" && Serialable == "N") {
                                $('#BatchDetailTbl1 >tbody').html('');
                                RenderHtmlForItemBatchSerialDetails(SItemID, "", "", "", MfgName, MfgMrp, MfgDate, "", OpQty, rate1, OpVal);
                                //$('#BatchDetailTbl1 >tbody').append(`<tr>
                                //              <td style="display:none">${SItemID}</td>
                                //              <td>${''}</td>
                                //              <td></td>
                                //              <td></td>
                                //              <td></td>
                                //              <td class="num_right">${parseFloat(OpQty).toFixed(QtyDecDigit)}</td>
                                //              <td class="num_right">${parseFloat(rate1).toFixed(RateDecDigit)}</td>
                                //              <td class="num_right">${parseFloat(OpVal).toFixed(ValDecDigit)}</td>
                                //              </tr>`);
                                $("#span_opqty").text(parseFloat(OpQty).toFixed(QtyDecDigit));
                                $("#span_opvalue").text(parseFloat(OpVal).toFixed(ValDecDigit));
                                localStorage.setItem("flag_lot", false);
                            }
                        }
                    }
                    else {
                        $("#BatchDetailTbl1 >tbody >tr").remove();
                    }
                },
            });
        } catch (err) {
        }
    } catch (err) {
        //console.log("UserValidate Error : " + err.message);
    }
}
function RenderHtmlForItemBatchSerialDetails(ItemId, LotId, BatchNo,SerialNo,MfgName,MfgMrp,MfgDate,ExpDate,BatchQty,Rate,Value) {
    $('#BatchDetailTbl1 >tbody').append(`<tr>
                                <td style="display:none">${ItemId}</td>
                                <td>${LotId}</td>
                                <td>${BatchNo}</td>
                                <td>${SerialNo}</td>
                                <td>${IsNull(MfgName,'')}</td>
                                <td class="num_right">${IsNull(MfgMrp,'')}</td>
                                <td>${Cmn_FormatDate_ddmmyyyy(MfgDate)}</td>
                                <td>${ExpDate}</td>
                                <td class="num_right">${parseFloat(BatchQty).toFixed(QtyDecDigit)}</td>
                                <td class="num_right">${parseFloat(Rate).toFixed(RateDecDigit)}</td>
                                <td class="num_right">${parseFloat(Value).toFixed(ValDecDigit)}</td>
                             </tr>`);
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#hfSNo").val();
    var ItmCode = "";
    ItmCode = clickedrow.find("#hf_ItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function GetOP_ItemBatchDetails() {
    debugger;
    var OPRItemsBatchDetail = [];

    var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
    if (FBatchDetails != null) {
        if (FBatchDetails.length > 0) {
            var newTableArr = $('#datatable-buttons').DataTable().rows().data().toArray();
            //$("#datatable-buttons tbody tr").remove();

            newTableArr.map((item) => {

                var newFBatchDetails = FBatchDetails.filter(t => t.ItemID == item[1])
                for (i = 0; i < newFBatchDetails.length; i++) {
                    var ItemID = newFBatchDetails[i].ItemID;
                    var BatchNo = newFBatchDetails[i].BatchNo;
                    var BatchQty = newFBatchDetails[i].BatchQty;
                    var BatchExDate = newFBatchDetails[i].BatchExDate;
                    var BatchDate = newFBatchDetails[i].BatchExDate;
                    var MfgName = newFBatchDetails[i].mfg_name || '';//Added Mfg Details by Suraj Maurya on 25-11-2025
                    var MfgMrp = newFBatchDetails[i].mfg_mrp || '';
                    var MfgDate = newFBatchDetails[i].mfg_date || '';
                    //start Added by Nidhi 15-04-2025
                    if (!/^\d{4}-\d{2}-\d{2}$/.test(BatchDate) && BatchDate != "") {
                        var btdt = BatchDate.split("-");
                        BatchExDate = btdt[2] + "-" + btdt[1] + "-" + btdt[0];
                    }
                    //End
                    if (ItemID == item[1]) {
                        OPRItemsBatchDetail.push({
                            item_id: ItemID, batch_no: BatchNo, batch_qty: BatchQty, exp_dt: BatchExDate
                            , MfgName: MfgName, MfgMrp: MfgMrp, MfgDate: MfgDate
                        });
                    }
                }
            })
            //$("#datatable-buttons >tbody >tr").each(function (i, row) {
            //    for (i = 0; i < FBatchDetails.length; i++) {
            //        var ItemID = FBatchDetails[i].ItemID;
            //        var BatchNo = FBatchDetails[i].BatchNo;
            //        var BatchQty = FBatchDetails[i].BatchQty;
            //        var BatchExDate = FBatchDetails[i].BatchExDate;

            //        debugger;
            //        var currentRow = $(this);
            //        var hdn_Itm_ID = currentRow.find("#tblhdn_item_id").val();

            //        debugger;
            //        if (ItemID == hdn_Itm_ID) {
            //            OPRItemsBatchDetail.push({ item_id: ItemID, batch_no: BatchNo, batch_qty: BatchQty, exp_dt: BatchExDate });
            //        }
            //    }
            //});
        }
    }

    return OPRItemsBatchDetail;
};
function GetOP_ItemSerialDetails() {
    debugger
    var OPRItemsSerialDetail = [];

    var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    if (FSerialDetails != null) {
        if (FSerialDetails.length > 0) {
            var newTableArr = $('#datatable-buttons').DataTable().rows().data().toArray();//Added by Suraj on 05-10-2024
            newTableArr.map((item) => {

                var newFSerialDetails = FSerialDetails.filter(t => t.ItemID == item[1])
                for (i = 0; i < newFSerialDetails.length; i++) {
                    var ItemID = newFSerialDetails[i].ItemID;
                    var SerialNo = newFSerialDetails[i].SerialNo;
                    var MfgName = newFSerialDetails[i].MfgName;//Added Mfg Details by Suraj Maurya on 25-11-2025
                    var MfgMrp = newFSerialDetails[i].MfgMrp;
                    var MfgDate = newFSerialDetails[i].MfgDate;
                    if (ItemID == item[1]) {
                        OPRItemsSerialDetail.push({
                            item_id: ItemID, serial_no: SerialNo
                            , MfgName: MfgName, MfgMrp: MfgMrp, MfgDate: MfgDate
                        });
                    }
                }
            })

            //$("#datatable-buttons >tbody >tr").each(function (i, row) {//commented by Suraj on 05-10-2024 
            //    for (i = 0; i < FSerialDetails.length; i++) {
            //        var ItemID = FSerialDetails[i].ItemID;
            //        var SerialNo = FSerialDetails[i].SerialNo;
            //        var currentRow = $(this);
            //        var hdn_Itm_ID = currentRow.find("#tblhdn_item_id").val();
            //        if (ItemID == hdn_Itm_ID) {
            //            OPRItemsSerialDetail.push({ item_id: ItemID, serial_no: SerialNo });
            //        }
            //    }
            //});
        }
    }

    return OPRItemsSerialDetail;
};

function RemoveSession() {
    debugger;
    /*    sessionStorage.removeItem("OPRTransType");*/
    sessionStorage.removeItem("BatchDetailSession");
    sessionStorage.removeItem("SerialDetailSession");
    //sessionStorage.removeItem("EditOPRDate");
    //sessionStorage.removeItem("EditWh_id");
    //sessionStorage.removeItem("OPRItemDetailSession");
    //sessionStorage.removeItem("OPREnableDisable");
    //sessionStorage.removeItem("dbclickwh_id");
}
function RemoveSessionNew() {
    /* sessionStorage.removeItem("OPRTransType");*/
    sessionStorage.removeItem("BatchDetailSession");
    sessionStorage.removeItem("SerialDetailSession");
    /*    sessionStorage.removeItem("OPRItemDetailSession");*/
    //sessionStorage.removeItem("OPREnableDisable");
    //sessionStorage.removeItem("dbclickwh_id");

}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");
    $("#OPQtyError").css("display", "none");
    $("#OPQty").css("border-color", "#ced4da");
    return true;
}
function QtyFloatValueonlyRecQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    $("#ReceivedQtyError").css("display", "none");
    $("#OPQty").css("border-color", "#ced4da");
    return true;
}
function RateFloatValueonly(el, evt) {
    debugger;
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    $("#ItemCostError").css("display", "none");
    $("#ItemCost").css("border-color", "#ced4da");
    return true;
}
function OnChangeItemCost(e) {
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    debugger;

    var clickedrow = $(e.target).closest("tr");

    var ItemCost = clickedrow.find("#ItemCost").val();
    if (ItemCost == "" || ItemCost == null || parseFloat(ItemCost) == parseFloat("0")) {
        clickedrow.find("#ItemCostError").text($("#valueReq").text());
        clickedrow.find("#ItemCostError").css("display", "block");
        clickedrow.find("#ItemCost").css("border-color", "red");
    }
    else {
        clickedrow.find("#ItemCost").val(parseFloat(ItemCost).toFixed(RateDecDigit));
        clickedrow.find("#ItemCostError").css("display", "none");
        clickedrow.find("#ItemCost").css("border-color", "#ced4da");
    }

    var OpQty = clickedrow.find("#OPQty").val();
    var ItmRate = clickedrow.find("#ItemCost").val();

    if ((OpQty !== 0 || OpQty !== null || OpQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        debugger;
        var FAmt = OpQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#OpValue").val(FinVal);

    }
}
function OnChangeOpeningQty(e) {
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    debugger;

    var clickedrow = $(e.target).closest("tr");

    var ReceivedQty = clickedrow.find("#OPQty").val();
    if (ReceivedQty == "" || ReceivedQty == null || parseFloat(ReceivedQty) == parseFloat("0")) {
        clickedrow.find("#OPQtyError").text($("#valueReq").text());
        clickedrow.find("#OPQtyError").css("display", "block");
        clickedrow.find("#OPQty").css("border-color", "red");
        clickedrow.find("#OPQty").val("");
    }
    else {
        clickedrow.find("#OPQty").val(parseFloat(ReceivedQty).toFixed(QtyDecDigit));
        clickedrow.find("#OPQtyError").css("display", "none");
        clickedrow.find("#OPQty").css("border-color", "#ced4da");
    }

    var OpQty = clickedrow.find("#OPQty").val();
    var ItmRate = clickedrow.find("#ItemCost").val();

    if ((OpQty !== 0 || OpQty !== null || OpQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        debugger;
        var FAmt = OpQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#OpValue").val(FinVal);

    }
}

function OnClickBatchDetailBtn(e) {
    debugger;

    Cmn_OnClickBatchDetailBtn_New(e, "#hfSNo", "#ItemName_1", "#hf_ItemID", "#UOM", "#hfexpiralble", "#OPQty", "OPREnableDisable", "");
}

function OnClickSaveAndClose() {
    Cmn_OnClickSaveAndClose("#OP_ItmDetailsTbl", "#hf_ItemID");
    if ($("#HdnBatchData").text() == "Y") {
        $("#HdnBatchData").text("N");
    }
}
//--------------------End--------------------------//

//---------------Serial Deatils-----------------------//
function OnClickSerialDetailBtn(e) {
    debugger;
    Cmn_OnClickSerialDetailBtn_New(e, "#hfSNo", "#ItemName_1", "#hf_ItemID", "#UOM", "#OPQty", "OPREnableDisable", "");

}

//--------------------End----------------------------//
function OnClickItemUpdateBtn(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    flag = 'N';
    var item_id = $("#hf_ItemID").val();
    var sub_item = $("#sub_item").val();
    var ddl_ItemName = $("#ItemName_1 option:selected").text();
    var OpValue = $("#OpValue").val();
    var OPQty = $("#OPQty").val();
    if (CheckOPR_ItemValidations() == false) {
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
    //Cmn_ApendToHdnSubItemTable("Quantity");
    var subitmFlag = CheckValidations_forSingleItem_SubItems(item_id, OPQty, sub_item, "SubItemOPQty");
    if (subitmFlag == false) {
        return false;
    } else {
        $('#datatable-buttons tbody tr #SubItemOPQty').css("border", "");
    }

    debugger;

    var tableRow = $('#hdnUpdateInTable').val();
    tableRow = parseInt(tableRow) + 1;

    //$('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#SpanOpQty").text(OPQty);
    //$('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#TxtOpQty").val(OPQty);

    //var ItemCost = $("#ItemCost").val();
    //$('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#SpanItemRate").text(ItemCost);
    //$('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#SpanOp_val").text(OpValue);


    //var abc = `<div class="col-sm-11 lpo_form no-padding" id=''> ${ddl_ItemName}</div> `;
    //$('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#tblhdn_item_id").val(item_id);

    var UOMID = $("#UOMID").val();
    var UOM_name = $("#UOM").val();
    //$('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#UOMID").val(UOMID);
    ////$('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("td:eq(5)").text(UOM_name);

    var newTableArr = $('#datatable-buttons').DataTable().rows().data().toArray();

    //var Opening_val1 = 0; // Initialize the variable to accumulate the sum

    //for (var i = 0; i < newTableArr.length; i++) {
    //    Opening_val1 += parseFloat(newTableArr[i][5]) || 0; // Add the value from the 6th column
    //}
    var ValDecDigit = $("#ValDigit").text();
    var Opening_val = parseFloat($("#OpeningValue").val());
    //var Opening_val = Opening_val1;
    var Opening_valitem = parseFloat($("#OpValue").val());
    var ItemOldValue = parseFloat(sessionStorage.getItem("ItemOPVal"));
    var total = Opening_val + Opening_valitem - ItemOldValue;
    var FinalTotal = parseFloat(total).toFixed(ValDecDigit);
    $("#OpeningValue").val(FinalTotal);
    sessionStorage.removeItem("ItemOPVal");
    var rowId = '';
    //--------------------------------
    var ItemCost = $("#ItemCost").val();
    var subitmDisable = "";
    if (sub_item != "Y") {
        subitmDisable = "disabled";
    }
    var span_SubItemDetail = $("#span_SubItemDetail").text();
    //var rw = $('#datatable-buttons').find("tr:eq(" + tableRow + ")");
    var pageParamTable = $('#datatable-buttons').DataTable();
    var tableRowdt = pageParamTable.row($('#hdnUpdateInTable').val());
    var rData = [
        rowId,
        item_id,
        UOMID,
        OPQty,
        $("#LotNumber").val(),
        ItemCost,
        OpValue,
        `<td class="red-center"> <i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" onclick="deleteRow(this, event,'${item_id}')"></i></td>`,
        `<td ><div class="center edit_icon"><i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" onclick="editItem(this, event,'${item_id}')" title="${$("#Edit").text()}"></i></div></td>`,
        `<td><span id ="spanrowid">${tableRow}</span></td>`,
        `<td><div class="col-sm-12 no-padding">${$("#ItemName_1 option:selected").text()}<input type="hidden" id="tblhdn_item_id" value="${item_id}" style="display:none;"><input type="hidden" id="tblhdn_item_name" value="${$("#ItemName_1 option:selected").text()}" style="display:none;"></div></td>`,
        `<td >${UOM_name}<input type="hidden" id="tblhdn_uom_id" value="${UOMID}" style="display:none;"><input type="hidden" id="tblhdn_uom_alias" value="${UOM_name}" style="display:none;"><input type="hidden" id="tblhdn_batchable_id" value="${$("#hfbatchable").val()}" style="display:none;"><input type="hidden" id="tblhdn_serialable_id" value="${$("#hfserialable").val()}" style="display:none;"><input type="hidden" id="tblhdn_expiralble_id" value="${$("#hfexpiralble").val()}" style="display:none;"><input hidden id="SpanItemRate" value="${ItemCost}" /><input hidden id="SpanLotNumber" value="${$("#LotNumber").val()}" /></td>`,
        `<td class="num_right">
<div class=" col-sm-9 num_right">
<span id="SpanOpQty">${OPQty}</span>
<input type="hidden" id="TxtOpQty" value="${OPQty}">
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemOPQty">
    <input hidden type="text" id="sub_item" value="${sub_item}" />
    <button type="button" id="SubItemOPQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('OpSubitmQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
</div>
<div class="col-sm-1 i_Icon">
          <button type="button" id="BatchSerialNumber" class="calculator item_pop" data-toggle="modal" onclick="OnClickOpeningQuantityIcon(this,event);" data-target="#BatchSerialNumberpop" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title=""></button>
            </div></td>`,

        `<td><div class="num_right"><span id="SpanOp_val">${OpValue}</span></div></td>`,
        $("#hfbatchable").val(),
        $("#hfserialable").val(),
        $("#hfexpiralble").val(),
        $("#ItemName_1 option:selected").text(),
        sub_item
    ];
    pageParamTable
        .row(tableRowdt)
        .data(rData)
        .draw();

    //$("#datatable-buttons >tbody >tr").each(function () {
    //    var currentRow = $(this);
    //    currentRow.find("td:eq(0)").attr("hidden", true);
    //    currentRow.find("td:eq(1)").attr("hidden", true);
    //    currentRow.find("td:eq(2)").attr("hidden", true);
    //    currentRow.find("td:eq(3)").attr("hidden", true);
    //    currentRow.find("td:eq(4)").attr("hidden", true);
    //    currentRow.find("td:eq(5)").attr("hidden", true);
    //    currentRow.find("#delBtnIcon").addClass("red");

    //    currentRow.find("#SpanLotNumber").parent().css("display", "none");
    //    currentRow.find("#SpanItemRate").parent().parent().css("display", "none");
    //});
    //--------------------------------
    $("#OPQty").val("");
    $("#ItemCost").val("");
    $("#OpValue").val("");
    $("#UOM").val("");

    $("#ItemName_1").attr('disabled', false);
    $("#ItemName_1").val("0").change();
    $("#divAddNew").show();
    $("#divUpdate").hide();
    HideItemListItm(item_id);
};
function ForwardBtnClick() {
    debugger;
    try {
    var OMRStatus = "";
    OMRStatus = $('#StatusCode').val().trim();
    if (OMRStatus === "D" || OMRStatus === "F") {

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
    catch (ex) {
        console.log(ex);
        return false;
    }
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var wh_id = "";
    var op_id = "";
    var op_docno = "";
    var OMRDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";

    docid = $("#DocumentMenuId").val();
    wh_id = $("#wh_id").val().trim();
    op_id = $("#opstk_rno").val();
    OMRDate = $("#txtFromdate").val();
    $("#hdDoc_No").val(wh_id);
    Remarks = $("#fw_remarks").val();
    op_docno = (wh_id + "_" + op_id);
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (OMRDate + ',' + wh_id + ',' + "Update" + ',' + WF_status1 + ',' + op_id)

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
        if (fwchkval != "" && op_docno != "" && OMRDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail(op_docno, OMRDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/OpeningMaterialReceipt/ToRefreshByJS?TrancType=" + TrancType;
        }
    }

    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/OpeningMaterialReceipt/Approve_OpeningMaterialReceiptDetails?id=" + op_id + "&wh_id=" + wh_id + "&OPR_Date=" + OMRDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&WF_status1=" + WF_status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && op_docno != "" && OMRDate != "") {
            await Cmn_InsertDocument_ForwardedDetail(op_docno, OMRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/OpeningMaterialReceipt/ToRefreshByJS?TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && op_docno != "" && OMRDate != "") {
            await Cmn_InsertDocument_ForwardedDetail(op_docno, OMRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/OpeningMaterialReceipt/ToRefreshByJS?TrancType=" + TrancType;
        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#wh_id").val();
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
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemOPQty",);
}
function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');
    //var hfsno = clickdRow.find("#hfsno").val();
    var ProductNm = clickdRow.find("#ItemName_1 option:selected").text();
    var ProductId = clickdRow.find("#hf_ItemID").val();
    var UOM = clickdRow.find("#UOM").val();

    var wh_id = $("#wh_id").val();
    var OP_dt = $("#txtFromdate").val();
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#StatusCode").val();
    hd_Status = IsNull(hd_Status, "").trim();
    //var UOMID = clickdRow.find("#UOMID").val();
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
        Sub_Quantity = clickdRow.find("#OPQty").val();
    }
    else if (flag == "OpSubitmQty") {
        ProductNm = clickdRow.find("#tblhdn_item_name").val();
        ProductId = clickdRow.find("#tblhdn_item_id").val();
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);
        });
        UOM = clickdRow.find("#tblhdn_uom_alias").val();
        Sub_Quantity = clickdRow.find("#TxtOpQty").val();
        IsDisabled = "Y";
        flag = "Quantity";
    }


    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/OpeningMaterialReceipt/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            wh_id: wh_id,
            OP_dt: OP_dt
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
    return Cmn_CheckValidations_forSubItems("datatable-buttons", "", "tblhdn_item_id", "TxtOpQty", "SubItemOPQty", "Y");
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("OP_ItmDetailsTbl", "", "ItemName_1", "OPQty", "SubItemOPQty", "N");
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
function FetchOpeningMaterialData() {
    debugger
    var isfileexist = $('#OPRfile').val();
    if (isfileexist != null && isfileexist != "") {
        FetchAndValidateData('0');
    }
    else {
        swal("", "Please choose file", "warning");
    }
}
function SearchByUploadStatus() {
    debugger
    var uploadStatus = $('#OMRitem_Status').val();
    if ($('#DttblBtnsmaterialreceipt tbody tr').length > 0) {
        FetchAndValidateData(uploadStatus);
    }
}
function FetchAndValidateData(uploadStatus) {
    debugger
    $(".loader1").show();
    var Warehouse = $('#importwhid').val();
    if (Warehouse == '0') {
        $("#vmimportwhid_RequiredArea").text($("#valueReq").text());
        $("#vmimportwhid_RequiredArea").css("display", "block");
        $("#importwhid").css("border-color", "red");

        $(".loader1").hide();
        return false
    }
    $('#BtnSearch').prop('disabled', false);
    var op_dt = $('#txtFromdate').val();
    var formData = new FormData();
    var file = $("#OPRfile").get(0).files[0];
    formData.append("file", file);

    $('#btnOPRImportData').prop('disabled', true);
    var PName = window.location.pathname.split('/')
    var addr = '/' + PName[1] + '/' + PName[2]
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ValidateExcelFile?uploadStatus=' + encodeURIComponent(uploadStatus) + '&Warehouse=' + encodeURIComponent(Warehouse));
    xhr.send(formData);
    xhr.onreadystatechange = function () {

        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#PartialImportOPRDetail').html(xhr.response);
            cmn_apply_datatable("#DttblBtnsmaterialreceipt")
            $(".loader1").hide();
            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            if (responseMessage == "Excel file is empty. Please fill data in excel file and try again" || responseMessage == "ErrorPage") {
                $('#btnOPRImportData').prop('disabled', true); // Keep the button disabled
                $('#btnOPRImportData').css('background-color', '#D3D3D3')
            } else {
                $('#btnOPRImportData').prop('disabled', false); // Enable the button
                $('#btnOPRImportData').css('background-color', '#007bff')
            }
        }
    }
}
function OnClickOMRErrorDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemName = "";
    var Warehouse = $('#importwhid').val();
    ItemName = clickedrow.find("#tdItmname").text();
    var formData = new FormData();
    var file = $("#OPRfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?ItemName=' + encodeURIComponent(ItemName) + '&Warehouse=' + encodeURIComponent(Warehouse));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#divErrorDetail').html(xhr.response);
        }
    }
}
function BindBatchDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemName = "";
    ItemName = clickedrow.find("#tdItmname").text();
    var formData = new FormData();
    var file = $("#OPRfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindBatchDetail?ItemName=' + encodeURIComponent(ItemName));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#bssid").css("display", "block")
            $('#bssid').html(xhr.response);
        }
    }
}

function BindSerialDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemName = "";
    ItemName = clickedrow.find("#tdItmname").text();
    var formData = new FormData();
    var file = $("#OPRfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindSerialDetail?ItemName=' + encodeURIComponent(ItemName));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#bssid").css("display", "block")
            $('#bssid').html(xhr.response);
        }
    }
}
function BindSubItemDetail(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemName = "";
    ItemName = clickedrow.find("#tdItmname").text();
    var formData = new FormData();
    var file = $("#OPRfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindSubItemDetail?ItemName=' + encodeURIComponent(ItemName));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#bssid").css("display", "block");
            $('#bssid').html(xhr.response);
        }
    }
}
function ImportOMRDataFromExcel() {
    debugger;
    $(".loader1").show();
    if ($('#hdnNotOk').val() != "0") {
        $('.loader1').hide();
        swal("", 'Invalid data in excel file. Please modify the data and try again..!', "warning");
    }
    else {
        var formData = new FormData();
        var Warehouse = $('#importwhid').val();
        var op_dt = $('#txtFromdate').val();
        var file = $("#OPRfile").get(0).files[0];
        formData.append("file", file);
        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest(); Warehouse
        xhr.open('POST', addr + '/ImportOMRDetailFromExcel?Warehouse=' + encodeURIComponent(Warehouse) + '&op_dt=' + encodeURIComponent(op_dt));
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            $(".loader1").hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                if (xhr.responseText.toLowerCase().includes("Success")) {
                    swal("", xhr.responseText, "success");
                    $('.loader1').hide();
                }
                else {
                    var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
                    if (responseMessage && responseMessage.trim() != "" && responseMessage != "The opening stock already exists in selected warehouse") {
                        swal("", "Data saved successfully", "success");
                        $('#btnOPRImportData').prop('disabled', true);
                        $('#btnOPRImportData').css('background-color', '#D3D3D3')
                        $('#DttblBtnsmaterialreceipt').DataTable().destroy();
                        $('#DttblBtnsmaterialreceipt tbody').empty()
                        $('#tbstatus_id').empty();
                        $('#OPRfile').val('');
                        $('#importwhid').val('0');
                        $('#PartialImportOMRData').modal('hide');
                        $('.loader1').hide();
                        window.location.href = "/ApplicationLayer/OpeningMaterialReceipt/EditOMR/?id=" + responseMessage + "&wh_id=" + Warehouse + "&Opening_dt=" + op_dt + "&WF_status=" + '';
                    }
                    else {
                        swal("", xhr.responseText, "warning");
                        $('.loader1').hide();
                    }
                }
            }
        }
    }
}
function Closemodal() {
    debugger
    $('#DttblBtnsmaterialreceipt').DataTable().destroy();
    $('#DttblBtnsmaterialreceipt tbody').empty()
    $('#tbstatus_id').empty();
    $('#OPRfile').val('');
    $('#importwhid').val('0');
    $('#btnOPRImportData').prop('disabled', true);
    $('#btnOPRImportData').css('background-color', '#D3D3D3')

}
function Close_modal() {
    debugger;
    $("#bssid").css("display", "none")
    $(".modal-backdrop").css("display", "none")
}

function BtnSearch() {
    debugger;

    try {
        var financial_year = $("#ddl_financial_year").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/OpeningMaterialReceipt/SearchOpeningDetail",
            data: {
                financial_year: financial_year,
            },
            success: function (data) {
                debugger;
                $('#opMaterialRecpt').html(data);
                $('#ListFilterData').val(financial_year);
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


function CheckItemBatchAndSerialAndSubItemDetails() {/* Created by Suraj Maurya on 21-08-2025 to check Batch And Serial Detail before Save  */
    var ErrorFlag = "N";
    var ErrorSubItemFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    var SerialableFlag = "N";
    var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
    var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    var arrItemList = [];
    $('#datatable-buttons').DataTable().rows().data().toArray().map((item) => {
        let OPQty = item[3];
        let ItemId = item[1];
        let ItemName = item[17];
        let Batchable = item[14];
        let Serialable = item[15];
        let SubItem = item[18];
        let Sub_Quantity = 0;
        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (FBatchDetails != null) {
                if (FBatchDetails.length > 0) {
                    FBatchDetails.filter(v => v.ItemID == ItemId).map((item) => {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(item.BatchQty);
                    })
                    if (parseFloat(OPQty).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {

                    }
                    else {
                        BatchableFlag = "Y";
                        EmptyFlag = "N";
                        arrItemList.push({ itemName: ItemName });
                    }
                }
                else {
                    BatchableFlag = "Y";
                    EmptyFlag = "Y";
                }
            }
            else {
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }

        }
        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (FSerialDetails != null) {
                if (FSerialDetails.length > 0) {
                    FSerialDetails.filter(v => v.ItemID == ItemId).map((item) => {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(1);
                    });
                    if (parseFloat(OPQty).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                        
                    }
                    else {
                        SerialableFlag = "Y";
                        EmptyFlag = "N";
                        arrItemList.push({ itemName: ItemName });
                    }
                }
                else {
                    SerialableFlag = "Y";
                    EmptyFlag = "Y";
                }
            }
            else {
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
        }
        if (SubItem == "Y") {
            $("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + ItemId + "]").closest('tr').each(function () {
                var Crow = $(this).closest("tr");
                var subItemQty = Crow.find("#subItemQty").val();
                Sub_Quantity = (parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty))).toFixed(QtyDecDigit);
            });
            if (parseFloat(OPQty) != parseFloat(Sub_Quantity)) {
                ErrorSubItemFlag = "Y";
                arrItemList.push({ itemName: ItemName });
            } 
        }
    })
    
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterBatchDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithreceivedqty").text() + " " + $("#span_for").text() + " " + arrItemList[0].itemName, "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterSerialDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Serialqtydoesnotmatchwithreceivedqty").text() + " " + $("#span_for").text() + " " + arrItemList[0].itemName, "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (ErrorSubItemFlag == "Y") {
        swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text() + " " + $("#span_for").text()+" " + arrItemList[0].itemName, "warning");
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

