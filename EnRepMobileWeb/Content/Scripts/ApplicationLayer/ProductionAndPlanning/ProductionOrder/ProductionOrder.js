$(document).ready(function () {
    debugger;
    sessionStorage.removeItem("productorderitemList");
    var ProductID = $("#hdn_product_id").val();
    var ProductName = $("#hdn_product_name").val();
    if (ProductID != "" && ProductName != "") {
        $('#ddl_ProductName').val(ProductID).trigger('change.select2');
        $('#ddl_ProductName').empty().append('<option value=' + ProductID + ' selected="selected">' + ProductName + '</option>');
        var hdn_Workstationid = $("#hdn_WorkstationName").val();
        var hdn_WorkstationText = $("#hdn_WorkstationText").val();
        if (hdn_Workstationid == "") {
            hdn_Workstationid = "0";
        }
        if (hdn_WorkstationText == "") {
            hdn_WorkstationText = "---Select---";
        }
        $('#ddl_WorkstationName').empty().append('<option value=' + hdn_Workstationid + ' selected="selected">' + hdn_WorkstationText + '</option>');
    }
    else {
        BindProductNameDDL();
        $("#ddl_ProductName").attr('onchange', 'ddl_ProductName_onchange()');
    }
    var Doc_No = $("#txt_ProductionOrderNumber").val();

    $("#hdDoc_No").val(Doc_No);
    $('#DnItmDetailsTbl tbody ').on('click', '.deleteIcon', function () {
        ////debugger;
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
        //debugger;       
        SerialNoAfterDelete();
    });
    GetItemList();
    OnClickOrderType();
    HideAdviceNo();
    debugger;
    DeleteDeliverySchdule();
    var ddl_ShopfloorName = $("#ddl_ShopfloorName").val();
    BindWorkStation(ddl_ShopfloorName);

    //var srcDoc = $("#ddlAdviceNo").val()
    //if (srcDoc != null || srcDoc != "") {
    //    if ($("#OrderTypeSub").is(":checked")) {

    //    }
    //    if ($("#OrderTypeInH").is(":checked")) {
    //       // $("#OrderTypeInH").attr("disabled", true)
    //       // $("#OrderTypeSub").attr("disabled", true)
    //    }
    //}
    $("#vm_ddl_ShopfloorName").css("display", "none");
    $("#ddl_ShopfloorName").css("border-color", "#ced4da")
});
var QtyDecDigit = $("#QtyDigit").text();
function SerialNoAfterDelete() {
    var SerialNo = 0;
    debugger;
    $("#DnItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#PO_srno").text(SerialNo);
    });
};
function OnClickOrderType() {
    debugger;
    var OrderType;
    var OrderType = "IH";

    if ($("#OrderTypeSub").is(":checked")) {
        OrderType = "SC"
        $("#divSubContract").css("display", "none");
        $("#CostDtlSubcontractID").css("display", "none");
        $("#ProdcOrdDtlSubcontractID").css("display", "none");
        $("#ProductionScheduleAccrdn").css("display", "none");

        //if (Flag == "Y") {
            $("#ddl_ShopfloorName").val(0);
            $("#ddl_WorkstationName").val(0);
            $("#txt_SupervisorName").val("");
            $("#ddl_shift").val(0);
            $("#txt_JobStartDateAndTime").val("");
            $("#txt_JobEndDateAndTime").val("");
        //}

    }
    if ($("#OrderTypeInH").is(":checked")) {
        OrderType = "IH"
        $("#divSubContract").css("display", "block");
        $("#CostDtlSubcontractID").css("display", "block");
        $("#ProdcOrdDtlSubcontractID").css("display", "block");
        $("#ProductionScheduleAccrdn").css("display", "block");

        
    }

    debugger;

    $("#Hdn_PrducOrderType").val(OrderType);
}
function HideAdviceNo() {
    var SrcType = "PA";
    if ($("#SourceTypeD").is(":checked")) {
        SrcType = "D"
        if ($("#ddlAdviceNo").val() == "0" || $("#ddlAdviceNo").val() == "") {
            $("#divAdvNo").css("display", "none");
            $("#divAdvDt").css("display", "none");
        }
    }
    if ($("#SourceTypePA").is(":checked")) {
        SrcType = "PA"
        $("#divAdvNo").css("display", "block");
        $("#divAdvDt").css("display", "block");

    }
    $("#Hdn_SrcType").val(SrcType);
}
function OnClickSrcType() {
    debugger;
    //var SrcType;
    var SrcType = "PA";
    if ($("#SourceTypeD").is(":checked")) {
        SrcType = "D"
        $("#divAdvNo").css("display", "none");
        $("#divAdvDt").css("display", "none");
        $("#txt_JobQuantity").val("");
        $("#txt_BatchNumber").val("");
        $("#ddlAdviceNo").val("--- Select ---");
        $("#Txt_AdviceDate").val("");
        $("#txt_JobQuantity").prop("readonly", false);
        $("#txt_BatchNumber").prop("readonly", false);
        var ProductID = 0;
        var ProductName = '---Select---';

        $('#ddl_ProductName').val(ProductID).trigger('change.select2');
        $('#ddl_ProductName').empty().append('<option value=' + ProductID + ' selected="selected">' + ProductName + '</option>');
    }
    if ($("#SourceTypePA").is(":checked")) {
        SrcType = "PA"
        $("#divAdvNo").css("display", "block");
        $("#divAdvDt").css("display", "block");
        $("#txt_JobQuantity").val("");
        $("#txt_BatchNumber").val("");
        $("#txt_JobQuantity").prop("readonly", true);
        $("#txt_BatchNumber").prop("readonly", true);
    }

    debugger;
    $("#Hdn_SrcType").val(SrcType);
}
function BindProductNameDDL() {
    debugger;
    var Itm_ID = $("#ddl_ProductName").val();
    if (Itm_ID == null) {
        BindItemList("#ddl_ProductName", "", "", "", "", "PRDORD");
        //$.ajax(
        //    {
        //        type: "POST",
        //        url: "/ApplicationLayer/ProductionOrder/GetProductNameInDDL",
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
        //                $('#ddl_ProductName').empty();
        //                var arr = [];
        //                arr = JSON.parse(data);
        //                if (arr.Table.length > 0) {
        //                    sessionStorage.removeItem("PLitemList");
        //                    sessionStorage.setItem("PLitemList", JSON.stringify(arr.Table));
        //                    $('#ddl_ProductName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        //                    for (var i = 0; i < arr.Table.length; i++) {
        //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
        //                    }
        //                    var firstEmptySelect = true;
        //                    $('#ddl_ProductName').select2({
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
        //                }
        //            }
        //        },
        //    });
    }
}
function ddl_ProductName_onchange() {
    debugger;
    var Itm_ID = $("#ddl_ProductName").val();
    var Itm_Name = $("#ddl_ProductName option:selected").text();
    $("#hdn_product_name").val(Itm_Name);
    if (Itm_ID != "0") {
        $("#vmddl_ProductName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");
    }
    else {
        $('#vmddl_ProductName').text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");

        $('#ddl_OperationName').empty();
        $('#ddl_OperationName').append(`<option value="0">---Select---</option>`);
    }

    if (Itm_ID != null) {
        $("#hdn_product_id").val(Itm_ID);
    }
    if (Itm_ID != null && Itm_ID != "") {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/ProductionOrder/GetProductionOrederItemUOM",
                    data: {
                        Itm_ID: Itm_ID
                    },
                    success: function (data) {
                        debugger;
                        if (data !== null && data !== "" && data != "ErrorPage") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                $("#UOM").val(arr.Table[0].uom_alias);
                                $("#UOMID").val(arr.Table[0].uom_id);
                                $("#UOMName").val(arr.Table[0].uom_alias);
                            }
                            else {
                                $("#UOM").val("");
                                $("#UOMID").val("");
                            }
                            //try {
                            //    HideShowPageWise(arr.Table[0].sub_item, "NoRow");
                            //} catch (ex) {

                            //}
                            if (arr.Table1.length > 0) {
                                $("#ddlAdviceNo option").remove();
                                $("#ddlAdviceNo optgroup").remove();
                                $('#ddlAdviceNo').append(`<optgroup class='def-cursor' id="Textddlorder" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                                for (var i = 0; i < arr.Table1.length; i++) {
                                    $('#Textddlorder').append(`<option data-date="${arr.Table1[i].adv_dt}" value="${arr.Table1[i].adv_dt}">${arr.Table1[i].adv_no}</option>`);
                                }
                                var firstEmptySelect = true;
                                $('#ddlAdviceNo').select2({
                                    templateResult: function (data) {
                                        var DocDate = $(data.element).data('date');
                                        var classAttr = $(data.element).attr('class');
                                        var hasClass = typeof classAttr != 'undefined';
                                        classAttr = hasClass ? ' ' + classAttr : '';
                                        var $result = $(
                                            '<div class="row">' +
                                            '<div class="col-md-6 col-xs-8' + classAttr + '">' + data.text + '</div>' +
                                            '<div class="col-md-6 col-xs-4' + classAttr + '">' + DocDate + '</div>' +
                                            '</div>'
                                        );
                                        return $result;
                                        firstEmptySelect = false;
                                    }
                                });

                                $("#Txt_AdviceDate").val("");
                                $("#vm_advice_no").css("display", "none");
                            }
                            if (arr.Table2.length > 0) {
                                $('#ddl_OperationName option').remove();
                                $('#ddl_OperationName').append('<option value="0" selected="selected">---Select---</option>');
                                for (var i = 0; i < arr.Table2.length; i++) {
                                    $('#ddl_OperationName').append('<option value="' + arr.Table2[i].op_id + '">' + arr.Table2[i].op_name + '</option>');
                                }
                            }
                            else {
                                $('#ddl_OperationName option').remove();
                                $('#ddl_OperationName').append('<option value="0" selected="selected">---Select---</option>');
                            }
                            //$('#ddl_RevisionNumber').empty();
                            //if (arr.Table3.length > 0) {
                            //    var rev = arr.Table3[0].rev_no.toString();

                            //    if (rev != "" && rev != null) {
                            //        //if (arr.Table1.length > 0) {
                            //        //    $('#ddl_RevisionNumber').empty();
                            //        //    arr = JSON.parse(data);
                            //        //    if (arr.Table1.length > 0) {
                            //        //        for (var i = 0; i < arr.Table1.length; i++) {
                            //        //            $('#ddl_RevisionNumber').append(`<option value="${arr.Table1[i].rev_no}">${arr.Table1[i].rev_no}</option>`);
                            //        //        }

                            //        //        $('#ddl_RevisionNumber').val(rev).trigger('change.select2');
                            //        //    }
                            //        //}
                            //        //else {
                            //        //    $('#ddl_RevisionNumber').append(`<option value=>---Select---</option>`);
                            //        //}
                            //        //var ddl_RevisionNumber = $("#ddl_RevisionNumber").val();
                            //        //$("#hdn_RevisionNumber").val(ddl_RevisionNumber);
                            //        //var hdn_RevisionNumberBDClick = $("#hdn_RevisionNumberBDClick").val();
                            //        //if (hdn_RevisionNumberBDClick != "") {
                            //        //    $('#ddl_RevisionNumber').val(hdn_RevisionNumberBDClick).trigger('change.select2');

                            //        //}

                            //        if (arr.Table2.length > 0) {
                            //            $('#ddl_OperationName').empty();
                            //            arr = JSON.parse(data);
                            //            if (arr.Table2.length > 0) {
                            //                $('#ddl_OperationName').append(`<option value=0>---Select---</option>`);
                            //                for (var i = 0; i < arr.Table2.length; i++) {
                            //                    $('#ddl_OperationName').append(`<option value="${arr.Table2[i].op_id}">${arr.Table2[i].op_name}</option>`);
                            //                }
                            //            }
                            //            var OP_Name = $("#hdn_OP_Name").val();
                            //            var hdn_op_id = $("#hdn_op_id").val();
                            //            if (hdn_op_id != "" && OP_Name != "") {
                            //                $('#ddl_OperationName').val(hdn_op_id).trigger('change.select2');
                            //            }
                            //        }
                            //    }
                            //    else {
                            //        //$('#ddl_RevisionNumber').empty();
                            //        $('#ddl_OperationName').empty();
                            //        $('#ddl_OperationName').append(`<option value=>---Select---</option>`);
                            //        //$('#ddl_RevisionNumber').append(`<option value=>---Select---</option>`);
                            //    }
                            //}
                            //else {
                            //    //$('#ddl_RevisionNumber').empty();
                            //    $('#ddl_OperationName').empty();
                            //    $('#ddl_OperationName').append(`<option value=>---Select---</option>`);
                            //    //$('#ddl_RevisionNumber').append(`<option value=>---Select---</option>`);
                            //}
                        }
                        else {
                            $("#UOM").val("");
                            $("#UOMID").val('');

                        }
                    },
                });

        } catch (err) {
        }
    }
}
function ddl_ShopfloorName_onchange(e) {
    debugger;
    var ddl_ShopfloorName = $("#ddl_ShopfloorName").val();
    $("#hdn_ShopfloorName").val(ddl_ShopfloorName);
    if (ddl_ShopfloorName != "0") {
        $("#vm_ddl_ShopfloorName").css("display", "none");
        $("#ddl_ShopfloorName").css("border-color", "#ced4da")
    }
    else {
        if (e != "SetDefaultValue") {
            $('#vm_ddl_ShopfloorName').text($("#valueReq").text());
            $("#vm_ddl_ShopfloorName").css("display", "block");
            $("#ddl_ShopfloorName").css("border-color", "red")
        }
    }
    if (e != "SetDefaultValue") {
        $("#ddl_WorkstationName").val("0");
        $("#hdn_WorkstationName").val("0");
    }
    BindWorkStation(ddl_ShopfloorName);
};
function BindWorkStation(ddl_ShopfloorName) {
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ProductionOrder/BindWorkStationList",
            data: {
                shfl_id: ddl_ShopfloorName,
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    var WorkStationId = $("#ddl_WorkstationName").val();
                    if (WorkStationId == "0" || WorkStationId == null) {
                        var WorkStationId = $("#hdn_WorkstationName").val();
                    }
                    $('#ddl_WorkstationName').empty();
                    arr = JSON.parse(data);
                    
                    if (arr.Table.length > 0) {
                        $('#ddl_WorkstationName').append(`<option value=0>---Select---</option>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#ddl_WorkstationName').append(`<option value="${arr.Table[i].ws_id}">${arr.Table[i].ws_name}</option>`);
                        }
                    }
                    else {
                        $('#ddl_WorkstationName').append(`<option value=0>---Select---</option>`);
                        WorkStationId = "0";
                    }
                    //var $ddl = $('#ddl_WorkstationName');
                    //var valueToSet = $ddl.find('option[value="' + WorkStationId + '"]').length ? WorkStationId : 0;
                    //$ddl.val(valueToSet);
                    $('#ddl_WorkstationName').val(WorkStationId);
                }
            },
        });
}
function PlusBtn_AddAttribute() {
    debugger;
    var ddl_ProductName = $("#ddl_ProductName").val();
    var Hdn_SrcType = $("#Hdn_SrcType").val();
    var AdviceNo = "";
    var AdviceDate = "";
    if (Hdn_SrcType == "A") {
        AdviceNo = $("#ddlAdviceNo option:selected").text();
        AdviceDate = $("#Txt_AdviceDate").val();
    }
    //var AdviceNo = $("#ddlAdviceNo option:selected").text();
    //var AdviceDate = $("#Txt_AdviceDate").val();
    var ddl_ShopfloorName = $("#ddl_ShopfloorName").val();
    var ddl_OperationName = $("#ddl_OperationName").val();
    var ddl_ProdQty = $("#txt_JobQuantity").val();



    var flag = CheckHeaderValidation();

    if (flag == false) {
        return false;
    }
    else {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ProductionOrder/Plus_AddAttribute",
                data: {
                    advice_no: AdviceNo,
                    advice_dt: AdviceDate,
                    op_id: ddl_OperationName,
                    shflid: ddl_ShopfloorName,
                    itemid: "",
                    ProdQty: ddl_ProdQty,
                    Product_id: ddl_ProductName,
                },
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        $('#DnItmDetailsTbl tbody tr').remove();
                        $('#Output_ItmDetailsTbl tbody tr').remove();
                        if (arr.Table.length > 0) {
                            $('#DnItmDetailsTbl tbody tr').remove();

                            AddNewRowForItem(arr);

                            var DataSno;
                            //for (var i = 0; i < arr.Table.length; i++) {
                            debugger;
                            $("#DnItmDetailsTbl >tbody >tr").each(function () {
                                var currentRow = $(this);
                                var SNo = currentRow.find("#SNohf").val();
                                DataSno = parseInt(SNo) - 1;
                                //currentRow.find("#ItemListName" + SNo).attr("onchange", "");
                                //currentRow.find("#ItemListName" + SNo).val(arr.Table[DataSno].item_id).trigger('change');
                                currentRow.find("#ItemListName").val(arr.Table[DataSno].item_name).trigger('change');
                                if (arr.Table[DataSno].alt_fill == "Y") {
                                    currentRow.find("#AltItemInfoBtnIcon").addClass("green1");
                                }
                                debugger;
                                //currentRow.find("#ItemListName" + SNo).attr("onchange", "OnChange_ItemName(event);");
                                currentRow.find("#hdn_item_id").val(arr.Table[DataSno].item_id);
                                currentRow.find("#UOM").val(arr.Table[DataSno].uom_alias);
                                currentRow.find("#hdn_uom_id").val(arr.Table[DataSno].uom_id);
                                currentRow.find("#ItemType").val(arr.Table[DataSno].Item_type);
                                currentRow.find("#hdn_Item_type_id").val(arr.Table[DataSno].item_type_id);
                                currentRow.find("#ReqQuantity").val((parseFloat(arr.Table[DataSno].req_qty).toFixed(QtyDecDigit)));
                                currentRow.find("#Insub_item").val(arr.Table[DataSno].sub_item);
                                currentRow.find("#hdn_req_qty").val(parseFloat(arr.Table[DataSno].req_qty).toFixed(QtyDecDigit));
                                currentRow.find("#hdn_seq_no").val(arr.Table[DataSno].seq_no);
                                currentRow.find("#ShflQuantity").val(parseFloat(arr.Table[DataSno].avl_stock_shfl).toFixed(QtyDecDigit));
                                currentRow.find("#WhQuantity").val(parseFloat(arr.Table[DataSno].avl_stock_warehouse).toFixed(QtyDecDigit));

                                flag = 'N';
                            });
                            //}
                            //for (var i = 0; i < arr.Table.length; i++) {
                            //    $('#DnItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">                                                                        
                            // <td>${rowIdx}</td>
                            // <td>
                            // <div class=" col-sm-11" style="padding:0px;">
                            // <input id="ItemName" value="${arr.Table[i].item_name}" class="form-control" autocomplete="off" type="text" name="ItemName" placeholder="${$("#ItemName").text()}"  onblur="this.placeholder='Item Name'" disabled="">
                            // </div>
                            // <div class="col-sm-1 i_Icon">
                            // <button type="button" class="calculator" onclick="OnClickIconBtntbl(event,'${arr.Table[i].item_id}');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <i class="fa fa-info" aria-hidden="true" title="" data-original-title="Item Information"></i> </button>
                            // </div>
                            // </td>
                            // <td hidden='hidden'><input type='hidden' id='hdn_item_id' value="${arr.Table[i].item_id}" /></td>
                            // <td>
                            // <input id="UOM" value="${arr.Table[i].uom_alias}" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}"  onblur="this.placeholder='UOM'" disabled="">
                            // </td>
                            // <td hidden='hidden'><input type='hidden' id='hdn_uom_id' value="${arr.Table[i].uom_id}" /></td>
                            // <td>
                            // <input id="ItemType" value="${arr.Table[i].Item_type}" class="form-control" autocomplete="" type="text" name="ItemType" placeholder="Item Type"  onblur="this.placeholder='Item Type'" disabled="">

                            // </td>
                            // <td hidden='hidden'><input type='hidden' id='hdn_Item_type_id' value="${arr.Table[i].item_type_id}" /></td>
                            // <td>
                            // <input id="ReqQuantity" value="${(parseFloat(arr.Table[i].req_qty).toFixed(QtyDecDigit))}" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled="">
                            // </td>
                            // <td hidden='hidden'><input type='hidden' id='hdn_req_qty' value="${(parseFloat(arr.Table[i].req_qty).toFixed(QtyDecDigit))}" /></td>
                            // <td hidden='hidden'><input type='hidden' id='hdn_seq_no' value="${arr.Table[i].seq_no}" /></td>                             
                            // <td>
                            // <input id="ShflQuantity" value="${parseFloat(arr.Table[i].avl_stock_shfl).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled="">
                            // </td>
                            // <td><div class=" col-sm-10" style="padding:0px; text-align:right;">
                            // <input id="WhQuantity" value="${parseFloat(arr.Table[i].avl_stock_warehouse).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled="">
                            // </div><div class=" col-sm-2" style="padding:0px; text-align:right;">
                            // <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"> <i class="fa fa-info" aria-hidden="true" title="Warehouse Wise Stock"></i> </button>
                            // </div></td>
                            //</tr>`);
                            //    flag = 'N';
                            //}

                            if (flag == 'N') {
                                //$("#ddl_SourceType").attr("readOnly", true);
                                $("#ddl_ProductName").prop("disabled", true);

                                $("#divAddNew").hide();
                                $("#Btn_AddItem").show();
                            }
                        }
                        if (arr.Table1.length > 0) {
                            $('#Output_ItmDetailsTbl tbody tr').remove();
                            var rowIdx = 0;
                            debugger;
                            for (var i = 0; i < arr.Table1.length; i++) {
                                var subitmDisable = "";
                                if (arr.Table1[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                debugger;
                                var ItemNameOuput = arr.Table1[i].item_name;
                                var ItemNameOuput1  = ItemNameOuput.replace(/"/g, '&quot;');

                                var OperationOutputProduct = $("#OutputItemID").val();
                                var OutputItemID = arr.Table1[i].item_id;
                                var OutputQuantity = 0;
                                if (OperationOutputProduct == OutputItemID) {
                                    OutputQuantity = $("#hdtxt_JobQuantity").val();
                                }
                                else {
                                    OutputQuantity = arr.Table1[i].req_qty;
                                }

                                $('#Output_ItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">                                                                        
                             <td>${rowIdx}</td>
                             <td><div class=" col-sm-11" style="padding:0px;">
                             <input id="ItemName" value="${ItemNameOuput1}" class="form-control" autocomplete="off" type="text" name="ItemName" disabled=""></div><div class="col-sm-1 i_Icon">
                             <button type="button" class="calculator" onclick="OnClickIconBtn(event,'${arr.Table1[i].item_id}');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}"></button></div></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_item_id' value="${arr.Table1[i].item_id}" /></td>
                             <td><input id="UOM" value="${arr.Table1[i].uom_alias}" class="form-control" autocomplete="off" type="text" name="UOM" disabled=""></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_uom_id' value="${arr.Table1[i].uom_id}" /></td>
                             <td><input id="ItemType" value="${arr.Table1[i].Item_type}" class="form-control" autocomplete="" type="text" name="ItemType" disabled=""></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_Item_type_id' value="${arr.Table1[i].item_type_id.trim()}" /></td>
                             <td>
                                <div class="col-sm-10 lpo_form no-padding">
                                 <input id="Quantity" value="${(parseFloat(OutputQuantity).toFixed(QtyDecDigit))}" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled="">
                                </div>
                                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemOutQty" >
                                <input hidden type="text" id="Outsub_item" value="${arr.Table1[i].sub_item}" />
                                <button type="button" id="SubItemOutQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('PrdOrdOutQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                </div>
                            </td>
                             <td hidden='hidden'><input type='hidden' id='hdn_req_qty' value="${(parseFloat(arr.Table1[i].req_qty).toFixed(QtyDecDigit))}" /></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_seq_no' value="${arr.Table1[i].seq_no}" /></td>   
                             <td><input id="PendingQuantity" value="${(parseFloat(arr.Table1[i].req_qty).toFixed(QtyDecDigit))}" class="form-control num_right" autocomplete="" type="text" name="PendingQuantity" placeholder="0000.00"  disabled=""></td>
                             <td><textarea id="remarksOutputitm" class="form-control remarksmessage" name="remarks" maxlength="100" autocomplete="off" value="" placeholder="${$("#span_remarks").text()}" title="${$("#span_remarks").text()}"></textarea></td>
                            </tr>`);
                            }
                        }
                    }
                    debugger;
                    $("#OrderTypeInH").attr("disabled", true)
                    $("#OrderTypeSub").attr("disabled", true)
                    $("#ddlAdviceNo").attr("disabled", true)
                    $("#ddl_OperationName").attr("disabled", true)
                },
            });
    }
};

function AddNewRowForItem(arr) {
    debugger;
    var rowIdx = 0;
    var deletetext = $("#Span_Delete_Title").text();
    for (var i = 0; i < arr.Table.length; i++) {
        var subitmDisable = "";
        if (arr.Table[i].sub_item != "Y") {
            subitmDisable = "disabled";
        }
        $('#DnItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}"> 
                            
                            <td id="PO_srno" class="sr_padding">${rowIdx}</td>
                            <td style="display: none;"><input class="" type="hidden" id="SNohf" value="${rowIdx}" /></td>
                            <td><div class="col-sm-10 lpo_form" style="padding:0px;">
                            <input class="form-control" disabled id="ItemListName" name="ItemListName" />
                            <span id="ItemListNameError${rowIdx}" class="error-message is-visible"></span></div>
                            <div class="col-sm-1 i_Icon">
                            <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtntbl(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
                            <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div>
                            <div class="col-sm-1 i_Icon">
                                <button type="button" id="AltItemInfoBtnIcon" class="calculator alter" onclick="OnClickAlternateItem(event)" data-toggle="modal" data-target="#PartialSelectAlternateItem" data-backdrop="static" data-keyboard="false" title="${$("#span_AlternateItemDetail").text()}"> <i class="fa fa-th-large" aria-hidden="true"></i>  </button>
                            </div>
                            </td><td hidden='hidden'><input type='hidden' id='hdn_item_id' value="" /></td>
                            <td><input id="UOM" value="" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}"  onblur="this.placeholder='${$("#ItemUOM").text()}'" disabled=""></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_uom_id' value="" /></td>
                             <td><input id="ItemType" value="" class="form-control" autocomplete="" type="text" name="ItemType" placeholder="${$("#ItemType").text()}"  onblur="this.placeholder='${$("#ItemType").text()}'" disabled=""></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_Item_type_id' value="" /></td>
                             <td>
                               <div class="col-sm-10 lpo_form no-padding">
                                <input id="ReqQuantity" value="" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled="">
                               </div>
                               <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReqirQty" >
                                <input hidden type="text" id="Insub_item" value="" />
                                <button type="button" id="SubItemReqirQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('PrdOrdReqQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                               </div>
                             </td>
                             <td hidden='hidden'><input type='hidden' id='hdn_req_qty' value="" /></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_seq_no' value="" /></td>                             
                             <td>
                             <div class="col-sm-10 lpo_form no-padding">
                                <input id="ShflQuantity" value="" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled="">
                             </div>
                             <div class="col-sm-2 i_Icon no-padding" id="div_SubItemShflAvlQty">
                                 <button type="button" id="SubItemShflAvlQty" ${subitmDisable} class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event,'Shfl')" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                             </div>
                             </td>
                             <td><div class=" col-sm-8" style="padding:0px; text-align:right;">
                             <input id="WhQuantity" value="" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled="">
                             </div><div class="col-sm-2 i_Icon">
                             <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                             </div>
                             <div class="col-sm-2 i_Icon no-padding" id="div_SubItemWhAvlQty">
                                 <button type="button" id="SubItemWhAvlQty" ${subitmDisable} class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event,'WH')" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                             </div>
                             </td>
                            </tr>`);
    }
    if ($("#DnItmDetailsTbl >tbody >tr").length > 0) {
        var ItemListData = JSON.parse(sessionStorage.getItem("productorderitemList"));
        debugger;
        if (ItemListData != null && ItemListData != "") {
            if (ItemListData.length > 0) {
                $("#DnItmDetailsTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var SNo = currentRow.find("#SNohf").val();
                    $('#ItemListName' + SNo).append(`<optgroup class='def-cursor' id="Textddl${SNo}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                    for (var i = 0; i < ItemListData.length; i++) {
                        $('#Textddl' + SNo).append(`<option data-uom="${ItemListData[i].uomname}" value="${ItemListData[i].itemid}">${ItemListData[i].itemname}</option>`);
                    }
                    var firstEmptySelect = true;
                    $('#ItemListName' + SNo).select2({
                        templateResult: function (data) {
                            var selected = $('#ItemListName' + SNo).val();
                            if (check(data, selected, "#DnItmDetailsTbl", "#SNohf", '#ItemListName') == true) {
                                var UOM = $(data.element).data('uom');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-8 col-xs-8' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-4 col-xs-4' + classAttr + '">' + UOM + '</div>' +
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
    }
};
function AmtFloatQtyonly(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit")) {
        return true;
    } else {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //debugger;
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    //return true;
}
function AddNewRow() {
    debugger;
    var ItemListData = JSON.parse(sessionStorage.getItem("productorderitemList"));
    if (ItemListData != null && ItemListData != "") {
        var tbllen = $('#DnItmDetailsTbl tbody tr').length;
        if (ItemListData.length == tbllen) {
        }
        else {
            var SrNo = 0;
            var RowNo = 0;
            var levels = [];
            $("#DnItmDetailsTbl >tbody >tr").each(function (i, row) {
                var currentRow = $(this);
                RowNo = parseInt(currentRow.find("#SNohf").val());
                levels.push(RowNo);
            });
            if (levels.length > 0) {
                RowNo = Math.max(...levels);
            }
            RowNo = RowNo + 1;
            SrNo = $("#DnItmDetailsTbl >tbody >tr").length;
            if (SrNo == 0) {
                SrNo = 1;
            }
            else {
                SrNo = parseInt(SrNo) + 1;
            }
            var deletetext = $("#Span_Delete_Title").text();

            $('#DnItmDetailsTbl tbody').append(`<tr id="R${RowNo}"> 
                            <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${deletetext}"></i></td>
                            <td id="PO_srno" class="sr_padding">${SrNo}</td>
                            <td style="display: none;"><input class="" type="hidden" id="SNohf" value="${RowNo}" /></td>
                            <td><div class="col-sm-11 lpo_form" style="padding:0px;">
                            <select class="form-control" id="ItemListName${RowNo}" name="ItemListName${RowNo}" onchange="OnChange_ItemName(event);" ></select>
                            <span id="ItemListNameError${RowNo}" class="error-message is-visible"></span></div>
                            <div class="col-sm-1 i_Icon">
                            <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtntbl(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
                            <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div></td>
                            <td hidden='hidden'><input type='hidden' id='hdn_item_id' value="" /></td>
                            <td><input id="UOM" value="" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}"  onblur="this.placeholder='${$("#ItemUOM").text()}'" disabled=""></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_uom_id' value="" /></td>
                             <td><input id="ItemType" value="" class="form-control" autocomplete="" type="text" name="ItemType" placeholder="${$("#ItemType").text()}"  onblur="this.placeholder='${$("#ItemType").text()}'" disabled=""></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_Item_type_id' value="" /></td>
                             <td><input id="ReqQuantity" value="" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled=""></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_req_qty' value="" /></td>
                             <td hidden='hidden'><input type='hidden' id='hdn_seq_no' value="" /></td>                             
                             <td><input id="ShflQuantity" value="" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled=""></td>
                             <td><div class=" col-sm-10" style="padding:0px; text-align:right;">
                             <input id="WhQuantity" value="" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled="">
                             </div><div class="col-sm-2 i_Icon">
                             <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                             </div></td>
                            </tr>`);

            if ($("#DnItmDetailsTbl >tbody >tr").length > 0) {
                var ItemListData = JSON.parse(sessionStorage.getItem("productorderitemList"));
                if (ItemListData != null && ItemListData != "") {
                    if (ItemListData.length > 0) {
                        $('#ItemListName' + RowNo).append(`<optgroup class='def-cursor' id="Textddl${RowNo}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                        $('#Textddl' + RowNo).append(`<option data-uom="0" value="0">---Select---</option>`);
                        for (var i = 0; i < ItemListData.length; i++) {
                            $('#Textddl' + RowNo).append(`<option data-uom="${ItemListData[i].uomname}" value="${ItemListData[i].itemid}">${ItemListData[i].itemname}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#ItemListName' + RowNo).select2({
                            templateResult: function (data) {
                                var selected = $('#ItemListName' + RowNo).val();
                                if (check(data, selected, "#DnItmDetailsTbl", "#SNohf", '#ItemListName') == true) {
                                    var UOM = $(data.element).data('uom');
                                    var classAttr = $(data.element).attr('class');
                                    var hasClass = typeof classAttr != 'undefined';
                                    classAttr = hasClass ? ' ' + classAttr : '';
                                    var $result = $(
                                        '<div class="row">' +
                                        '<div class="col-md-8 col-xs-8' + classAttr + '">' + data.text + '</div>' +
                                        '<div class="col-md-4 col-xs-4' + classAttr + '">' + UOM + '</div>' +
                                        '</div>'
                                    );
                                    return $result;
                                }
                                firstEmptySelect = false;
                            }
                        });
                    }
                }
            }
        }
    }
}
function OnChange_ItemName(e) {
    debugger;
    var Hdn_SrcType = $("#Hdn_SrcType").val();
    var AdviceNo = "";
    var AdviceDate = "";
    if (Hdn_SrcType == "A") {
        AdviceNo = $("#ddlAdviceNo option:selected").text();
        AdviceDate = $("#Txt_AdviceDate").val();
    }
    var ddl_ProductName = $("#ddl_ProductName").val();
  
    var ddl_ShopfloorName = $("#ddl_ShopfloorName").val();
    var ddl_OperationName = $("#ddl_OperationName").val();
    var ddl_ProdQty = $("#txt_JobQuantity").val();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohf").val();
    ItmCode = clickedrow.find("#ItemListName" + Sno).val();

    if (ItmCode != "" && ItmCode != null && ItmCode != "0") {
        clickedrow.find("#ItemListNameError" + Sno).css("display", "none");

        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ProductionOrder/Plus_AddAttribute",
                data: {
                    advice_no: AdviceNo,
                    advice_dt: AdviceDate,
                    op_id: ddl_OperationName,
                    shflid: ddl_ShopfloorName,
                    itemid: ItmCode,
                    ProdQty: ddl_ProdQty,
                    Product_id: ddl_ProductName,
                },
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        debugger;
                        if (arr.Table.length > 0) {
                            for (var i = 0; i < arr.Table.length; i++) {
                                $("#DnItmDetailsTbl >tbody >tr").each(function () {
                                    var currentRow = $(this);
                                    var SNo = currentRow.find("#SNohf").val();
                                    var Itm_Code = currentRow.find("#ItemListName" + SNo).val();
                                    if (Itm_Code === ItmCode) {
                                        currentRow.find("#hdn_item_id").val(arr.Table[i].item_id);
                                        currentRow.find("#UOM").val(arr.Table[i].uom_alias);
                                        currentRow.find("#hdn_uom_id").val(arr.Table[i].uom_id);
                                        currentRow.find("#ItemType").val(arr.Table[i].Item_type);
                                        currentRow.find("#hdn_Item_type_id").val(arr.Table[i].item_type_id);
                                        currentRow.find("#ReqQuantity").val((parseFloat(arr.Table[i].req_qty).toFixed(QtyDecDigit)));
                                        currentRow.find("#hdn_req_qty").val(parseFloat(arr.Table[i].req_qty).toFixed(QtyDecDigit));
                                        currentRow.find("#hdn_seq_no").val(arr.Table[i].seq_no);
                                        currentRow.find("#ShflQuantity").val(parseFloat(arr.Table[i].avl_stock_shfl).toFixed(QtyDecDigit));
                                        currentRow.find("#WhQuantity").val(parseFloat(arr.Table[i].avl_stock_warehouse).toFixed(QtyDecDigit));
                                    }
                                });
                            }
                        }
                    }
                },
            });
    }
    else {
        clickedrow.find('#ItemListNameError' + Sno).text($("#valueReq").text());
        clickedrow.find("#ItemListNameError" + Sno).css("display", "block");
        clickedrow.find("#ItemListName" + Sno).css("border-color", "red");
    }
};

function OnClickIconBtntbl(e) {
    debugger;
    var ItmCode = "";
    var clickedrow = $(e.target).closest("tr");
    //var Sno = clickedrow.find("#SNohf").val();

    //ItmCode = clickedrow.find("#ItemListName" + Sno).val();
    ItmCode = clickedrow.find("#hdn_item_id").val();

    ItemInfoBtnClick(ItmCode);

}
function OnClickIconBtntblMRS(e) {
    debugger;
    var ItmCode = "";
    var clickedrow = $(e.target).closest("tr");
    //var Sno = clickedrow.find("#SNohf").val();

    //ItmCode = clickedrow.find("#ItemListName" + Sno).val();
    ItmCode = clickedrow.find("#MRS_ItemID").val();

    ItemInfoBtnClick(ItmCode);

}
function OnClickOPIconBtntbl(e, ItmCode) {
    debugger;
    //var ItmCode = "";
    //var clickedrow = $(e.target).closest("tr");
    //var Sno = clickedrow.find("#SNohf").val();

    //ItmCode = clickedrow.find("#ItemListName" + Sno).val();

    ItemInfoBtnClick(ItmCode);

}
function OnClickIconBtn(e, item_id) {
    debugger;
    ItemInfoBtnClick(item_id);
}
function ProductNameItemInfo(Ptype) {
    var item_id = "";
    if (Ptype == "OWItem") {
        item_id = $("#OutputItemID").val();
    }
    if (Ptype == "FGItem") {
        item_id = $("#ddl_ProductName").val();
    }

    ItemInfoBtnClick(item_id);
}
function ddl_OperationName_onchange(e) {
    debugger;

    //var ddl_advno = $("#ddlAdviceNo option:selected").text();
    //var ddl_advdt = $("#Txt_AdviceDate").val();
    //var ddl_productid = $("#hdn_product_id").val();
    //var ddl_advno = $("#hd_advice_no").val();
    //var ddl_advdt = $("#hd_advice_dt").val();
    var ddl_OperationName = $("#ddl_OperationName").val();
    $("#hdn_OperationName").val(ddl_OperationName);
    var OperationName = $("#ddl_OperationName option:selected").text();
    $("#hdn_OP_Name").val(OperationName);
    $('#DnItmDetailsTbl tbody tr').remove();
    $('#Output_ItmDetailsTbl tbody tr').remove();
    $("#divAddNew").show();
    if (ddl_OperationName != "0" && ddl_OperationName != null) {
        $("#span_vm_ddl_OperationName").css("display", "none");
        $("#ddl_OperationName").css("border-color", "#ced4da")
        GetItemList();
    }
    else {
        $('#span_vm_ddl_OperationName').text($("#valueReq").text());
        $("#span_vm_ddl_OperationName").css("display", "block");
        $("#ddl_OperationName").css("border-color", "red")
    }
};
function GetItemList() {
    debugger;
    var Hdn_SrcType = $("#Hdn_SrcType").val();
    var advno = "";
    var advdt = "";
    if (Hdn_SrcType == "PA") {
        var advno = $("#ddlAdviceNo option:selected").text();
        var advdt = $("#Txt_AdviceDate").val();
    }

    var productid = $("#hdn_product_id").val();
    var opname = $("#ddl_OperationName").val();
    var jcqty = $("#hdtxt_JobQuantity").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ProductionOrder/GetItemDetailsList",
        data: {
            productid: productid,
            advice_no: advno,
            advice_dt: advdt,
            op_id: opname
        },
        dataType: "json",
        success: function (data) {
            debugger;
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.Table.length > 0) {
                    sessionStorage.removeItem("productorderitemList");
                    sessionStorage.setItem("productorderitemList", JSON.stringify(arr.Table));

                    if ($("#DnItmDetailsTbl >tbody >tr").length > 0) {

                        var ItemListData = JSON.parse(sessionStorage.getItem("productorderitemList"));
                        if (ItemListData != null && ItemListData != "") {
                            if (ItemListData.length > 0) {
                                $("#DnItmDetailsTbl >tbody >tr").each(function () {
                                    var currentRow = $(this);
                                    var RowNo = currentRow.find("#SNohf").val();
                                    var itemId = $('#ItemListName' + RowNo).val();
                                    $('#ItemListName' + RowNo).html(`<optgroup class='def-cursor' id="Textddl${RowNo}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                                    $('#Textddl' + RowNo).append(`<option data-uom="0" value="0">---Select---</option>`);
                                    for (var i = 0; i < ItemListData.length; i++) {
                                        $('#Textddl' + RowNo).append(`<option data-uom="${ItemListData[i].uomname}" value="${ItemListData[i].itemid}">${ItemListData[i].itemname}</option>`);
                                    }
                                    $('#ItemListName' + RowNo).val(itemId);
                                    var firstEmptySelect = true;
                                    $('#ItemListName' + RowNo).select2({
                                        templateResult: function (data) {
                                            var selected = $('#ItemListName' + RowNo).val();
                                            if (check(data, selected, "#DnItmDetailsTbl", "#SNohf", '#ItemListName') == true) {
                                                var UOM = $(data.element).data('uom');
                                                var classAttr = $(data.element).attr('class');
                                                var hasClass = typeof classAttr != 'undefined';
                                                classAttr = hasClass ? ' ' + classAttr : '';
                                                var $result = $(
                                                    '<div class="row">' +
                                                    '<div class="col-md-8 col-xs-8' + classAttr + '">' + data.text + '</div>' +
                                                    '<div class="col-md-4 col-xs-4' + classAttr + '">' + UOM + '</div>' +
                                                    '</div>'
                                                );
                                                return $result;
                                            }
                                            firstEmptySelect = false;
                                        }
                                    });

                                });

                                //$("#DnItmDetailsTbl >tbody >tr").each(function () {
                                //    var currentRow = $(this);
                                //    var RowNo = currentRow.find("#SNohf").val();
                                //    var itemid = currentRow.find("#hdn_item_id").val();

                                //    currentRow.find("#ItemListName" + RowNo).val(itemid).trigger('change');

                                //});
                            }
                        }
                    }
                }
                debugger;
                if (arr.Table1.length > 0) {
                    $("#OutputItemName").val(arr.Table1[0].itemname);
                    $("#OutputItemID").val(arr.Table1[0].itemid);
                    $("#OutputUOM").val(arr.Table1[0].uomname);
                    $("#OutputUOMName").val(arr.Table1[0].uomname);
                    $("#OutputUOMID").val(arr.Table1[0].uomid);
                    $("#txt_JobQuantity").val(arr.Table[0].ProdQty);
                    HideShowPageWise(arr.Table1[0].sub_item, "NoRow");
                }
                if (arr.Table2.length > 0) {
                    for (var k = 0; k <= arr.Table2.length; k++) {

                        $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${arr.Table2[k].item_id}'></td>
                            <td><input type="text" id="subItemId" value='${arr.Table2[k].sub_item_id}'></td>
                            <td><input type="text" id="subItemQty" value='${arr.Table2[k].Qty}'></td>
                        </tr>`);
                    }
                }
                var ddl_ShopfloorName = $("#ddl_ShopfloorName").val();
                if (ddl_ShopfloorName == null || ddl_ShopfloorName == 0 || ddl_ShopfloorName == "0") {
                    $("#hdn_WorkstationName").val("0");
                    $("#ddl_ShopfloorName").val("0");                    
                        if (arr.Table3.length > 0) {
                            $("#hdn_WorkstationName").val(arr.Table3[0].ws_id);
                            if (arr.Table3[0].shfl_id != null && arr.Table3[0].shfl_id != 0 && arr.Table3[0].shfl_id != "0") {
                                $("#ddl_ShopfloorName").val(arr.Table3[0].shfl_id);
                            }
                            else {
                                $("#ddl_ShopfloorName").val("0");
                            }
                            ddl_ShopfloorName_onchange("SetDefaultValue");
                            $("#txt_SupervisorName").val(arr.Table3[0].supervisor_name)
                            $("#ddl_shift").val("1")
                        }
                }
               
                /*commented AND Add by Hina on 01-03-2024 to show blank instead of 0 where insert field*/
                //if ($("#txt_JobQuantity").val() == 0) {
                //   //$("#txt_JobQuantity").val(jcqty);
                //}
                var TxtJobQty = $("#hdtxt_JobQuantity").val()
                if (TxtJobQty == 0) {
                   $("#txt_JobQuantity").val("");
                }
                else {
                    $("#txt_JobQuantity").val(TxtJobQty);
                }
            }
        },
    });
}

function BindOPNameBaseOnRevNo() {
    var product_id = $("#hdn_product_id").val();
    //var revisionNumber = $("#ddl_RevisionNumber").val();
    var ProductionOrderNumber = $("#txt_ProductionOrderNumber").val();
    var ProductionOrderDate = $("#ProductionOrderDate").val();
    //if (ProductionOrderNumber != "") {
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ProductionOrder/BindOPName_BaseOnRevNo",
            data: {
                Itm_ID: product_id,
                rev_no: revisionNumber,
                ProductionOrderNumber: ProductionOrderNumber,
                Jc_Date: ProductionOrderDate,
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    $('#ddl_OperationName').empty();
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        $('#ddl_OperationName').append(`<option value=0>---Select---</option>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#ddl_OperationName').append(`<option value="${arr.Table[i].op_id}">${arr.Table[i].op_name}</option>`);
                        }
                    }
                    else {
                        $('#ddl_OperationName').append(`<option value=0>---Select---</option>`);
                    }

                    if (arr.Table1.length > 0) {
                        $('#txt_JobQuantity').val('');
                        for (var i = 0; i < arr.Table1.length; i++) {
                            $('#txt_JobQuantity').val(arr.Table1[i].jc_qty);
                        }
                    }
                    else {
                        $('#txt_JobQuantity').val('0');
                    }

                }
            },
        });
    //}
}
function ddl_WorkstationName_onchange(e) {
    debugger;
    var ddl_WorkstationName = $("#ddl_WorkstationName").val();
    $("#hdn_WorkstationName").val(ddl_WorkstationName);
    var WorkstationName = $("#ddl_WorkstationName option:selected").text();
    $("#hdn_WorkstationText").val(WorkstationName);
    if (ddl_WorkstationName != "0") {
        $("#span_vm_ddl_WorkstationName").css("display", "none");
        $("#ddl_WorkstationName").css("border-color", "#ced4da")
    }
    else {
        $('#span_vm_ddl_WorkstationName').text($("#valueReq").text());
        $("#span_vm_ddl_WorkstationName").css("display", "block");
        $("#ddl_WorkstationName").css("border-color", "red")
    }
};
function ddl_ddl_shift_onchange(e) {
    debugger;
    var ddl_shift = $("#ddl_shift").val();
    $("#hdn_shift").val(ddl_shift);
    
    if (ddl_shift != "0") {
        $("#span_vm_ddl_shift").css("display", "none");
        $("#ddl_shift").css("border-color", "#ced4da")
    }
    else {
        $('#span_vm_ddl_shift').text($("#valueReq").text());
        $("#span_vm_ddl_shift").css("display", "block");
        $("#ddl_shift").css("border-color", "red")
    }
};

function SupervisorName_OnChange() {
    debugger;
    var txt_SupervisorName = $('#txt_SupervisorName').val().trim();
    if (txt_SupervisorName != "") {
        document.getElementById("vm_SupervisorName").innerHTML = "";
        $("#txt_SupervisorName").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vm_SupervisorName").innerHTML = $("#valueReq").text();
        $("#txt_SupervisorName").css("border-color", "red");

    }
}
function CheckHeaderValidation() {
    debugger;
    var SrcType;
    if ($("#SourceTypeD").is(":checked")) {
        SrcType = "D"
    }
    if ($("#SourceTypePA").is(":checked")) {
        SrcType = "PA"
    }
    var OrderType = $("#Hdn_PrducOrderType").val();
    $("#Hdn_SrcType").val(SrcType);

    var ddl_ProductName = $("#ddl_ProductName").val();
    //var txt_JobQuantity = $("#txt_JobQuantity").val();

    var flagVali = "N";

    var ProductionOrderDate = $("#ProductionOrderDate").val();
    if (ProductionOrderDate != "" && ProductionOrderDate != "0") {
        $("#vmjc_dt").css("display", "none");
    }
    else {
        $('#vmjc_dt').text($("#valueReq").text());
        $("#vmjc_dt").css("display", "block");
        $("#ProductionOrderDate").css("border-color", "red");
        flagVali = "Y";
    }
    var ProductionOrderQty = $("#txt_JobQuantity").val();
    if (parseFloat(CheckNullNumber(ProductionOrderQty)) > 0) {
        $("#vm_JobQuantity").css("display", "none");
        $("#txt_JobQuantity").css("border-color", "#ced4da");
    }
    else {
        $('#vm_JobQuantity').text($("#valueReq").text());
        $("#vm_JobQuantity").css("display", "block");
        $("#txt_JobQuantity").css("border-color", "red");
        flagVali = "Y";
    }
    var txt_BatchNumber = $("#txt_BatchNumber").val();
    if (txt_BatchNumber != "") {
        $("#vm_BatchNumber").css("display", "none");
        $("#txt_BatchNumber").css("border-color", "#ced4da");
    }
    else {
        $('#vm_BatchNumber').text($("#valueReq").text());
        $("#vm_BatchNumber").css("display", "block");
        $("#txt_BatchNumber").css("border-color", "red");
        flagVali = "Y";
    }
    if (ddl_ProductName != "0") {
        $("#vmddl_ProductName").css("display", "none");
    }
    else {
        $('#vmddl_ProductName').text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
        flagVali = "Y";
    }
    if (SrcType == "PA") {
        var ddl_AdviceNo = $("#ddlAdviceNo").val();
        if (ddl_AdviceNo != "0") {
            $("#vm_advice_no").css("display", "none");
        }
        else {
            $('#vm_advice_no').text($("#valueReq").text());
            $("#vm_advice_no").css("display", "block");
            $("[aria-labelledby='select2-ddlAdviceNo-container']").css("border-color", "red");
            $("#ddlAdviceNo").css("border-color", "red");
            flagVali = "Y";
        }
    }
    var ddl_OperationName = $("#ddl_OperationName").val();
    if (ddl_OperationName != "0") {
        $("#span_vm_ddl_OperationName").css("display", "none");
    }
    else {
        $('#span_vm_ddl_OperationName').text($("#valueReq").text());
        $("#span_vm_ddl_OperationName").css("display", "block");
        $("#ddl_OperationName").css("border-color", "red");
        flagVali = "Y";
    }
    if (OrderType == "IH") {
        var ddl_ShopfloorName = $("#ddl_ShopfloorName").val();
        if (ddl_ShopfloorName != "0") {
            $("#vm_ddl_ShopfloorName").css("display", "none");
        }
        else {
            $('#vm_ddl_ShopfloorName').text($("#valueReq").text());
            $("#vm_ddl_ShopfloorName").css("display", "block");
            $("#ddl_ShopfloorName").css("border-color", "red");
            flagVali = "Y";
        }
        var ddl_WorkstationName = $("#ddl_WorkstationName").val();
        if (ddl_WorkstationName != "0" && ddl_WorkstationName != null) {
            $("#span_vm_ddl_WorkstationName").css("display", "none");
        }
        else {
            $('#span_vm_ddl_WorkstationName').text($("#valueReq").text());
            $("#span_vm_ddl_WorkstationName").css("display", "block");
            $("#ddl_WorkstationName").css("border-color", "red");
            flagVali = "Y";
        }
        var txt_SupervisorName = $("#txt_SupervisorName").val();
        if (txt_SupervisorName != "") {
            $("#vm_SupervisorName").css("display", "none");
        }
        else {
            $('#vm_SupervisorName').text($("#valueReq").text());
            $("#vm_SupervisorName").css("display", "block");
            $("#txt_SupervisorName").css("border-color", "red");
            flagVali = "Y";
        }
        var ProductionOrderDate = $("#ProductionOrderDate").val();
        if (ProductionOrderDate != "") {
            $("#span_vm_ProductionOrderDate").css("display", "none");
        }
        else {
            $('#span_vm_ProductionOrderDate').text($("#valueReq").text());
            $("#span_vm_ProductionOrderDate").css("display", "block");
            $("#ProductionOrderDate").css("border-color", "red");
            flagVali = "Y";
        }
        var txt_JobStartDateAndTime = $("#txt_JobStartDateAndTime").val();
        if (txt_JobStartDateAndTime != "") {
            $("#span_vm_JobStartDateAndTime").css("display", "none");
            $("#txt_JobStartDateAndTime").css("border-color", "#ced4da");
        }
        else {
            $('#span_vm_JobStartDateAndTime').text($("#valueReq").text());
            $("#span_vm_JobStartDateAndTime").css("display", "block");
            $("#txt_JobStartDateAndTime").css("border-color", "red");
            flagVali = "Y";
        }
        var txt_JobEndDateAndTime = $("#txt_JobEndDateAndTime").val();
        if (txt_JobEndDateAndTime != "") {
            $("#span_vm_JobEndDateAndTime").css("display", "none");
            $("#txt_JobEndDateAndTime").css("border-color", "#ced4da");

            var Dates = moment(txt_JobStartDateAndTime.replace('T', ' ')).toDate();
            var Datee = moment(txt_JobEndDateAndTime.replace('T', ' ')).toDate();
            if (Datee.getTime() <= Dates.getTime()) {
                document.getElementById("span_vm_JobEndDateAndTime").innerHTML = $("#JC_InvalidDate").text();
                $("#txt_JobEndDateAndTime").css("border-color", "red");
                $("#span_vm_JobEndDateAndTime").css("display", "block");
                flagVali = "Y";
            }
        }
        else {
            $('#span_vm_JobEndDateAndTime').text($("#valueReq").text());
            $("#span_vm_JobEndDateAndTime").css("display", "block");
            $("#txt_JobEndDateAndTime").css("border-color", "red");
            flagVali = "Y";
        }
        var ddl_shift = $("#ddl_shift").val();
        if (ddl_shift != "0") {
            $("#span_vm_ddl_shift").css("display", "none");
        }
        else {
            $('#span_vm_ddl_shift').text($("#valueReq").text());
            $("#span_vm_ddl_shift").css("display", "block");
            $("#ddl_shift").css("border-color", "red");
            flagVali = "Y";
        }
    }
    if (flagVali == "Y") {
        return false;
    }
    else {
        return true;
    }
}
///// Commented by Suraj maurya on 19-10-2023 for change in alternate item and dropdown changed to input 
//function CheckItemValidation() {
//    debugger;
//    var flagVali = "N";

//    $("#DnItmDetailsTbl >tbody >tr").each(function () {
//        var currentRow = $(this);
//        var SNo = currentRow.find("#SNohf").val();
//        var Itemid = currentRow.find("#ItemListName" + SNo).val();

//        if (Itemid == "" || Itemid == null || Itemid == "0") {
//            currentRow.find('#ItemListNameError' + SNo).text($("#valueReq").text());
//            currentRow.find("#ItemListNameError" + SNo).css("display", "block");
//            currentRow.find("#ItemListName" + SNo).css("border-color", "red");
//            flagVali = "Y";
//        }
//        else {
//            currentRow.find("#ItemListNameError" + SNo).css("display", "none");
//        }
//    });

//    if (flagVali == "Y") {
//        return false;
//    }
//    else {
//        return true;
//    }
//}
function JobStartDateAndTime() {
    debugger;
    var txt_JobStartDateAndTime = $('#txt_JobStartDateAndTime').val().trim();
    $("#hdn_JobStartDateAndTime").val(txt_JobStartDateAndTime);
    if (txt_JobStartDateAndTime != "") {
        document.getElementById("span_vm_JobStartDateAndTime").innerHTML = "";
        $("#txt_JobStartDateAndTime").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("span_vm_JobStartDateAndTime").innerHTML = $("#valueReq").text();
        $("#txt_JobStartDateAndTime").css("border-color", "red");

    }
}
function JobEndDateAndTime() {
    debugger;
    var txt_JobStartDateAndTime = $('#txt_JobStartDateAndTime').val().trim();
    var txt_JobEndDateAndTime = $('#txt_JobEndDateAndTime').val().trim();
    $("#hdn_JobEndDateAndTime").val(txt_JobEndDateAndTime);

    var Dates = moment(txt_JobStartDateAndTime.replace('T', ' ')).toDate();
    var Datee = moment(txt_JobEndDateAndTime.replace('T', ' ')).toDate();
    if (Datee.getTime() < Dates.getTime()) {
        document.getElementById("span_vm_JobEndDateAndTime").innerHTML = $("#JC_InvalidDate").text();
        $("#txt_JobEndDateAndTime").css("border-color", "red");
        //$("#txt_JobEndDateAndTime").val('');
        return false;
    }
    if (txt_JobEndDateAndTime != "") {
        document.getElementById("span_vm_JobEndDateAndTime").innerHTML = "";
        $("#txt_JobEndDateAndTime").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("span_vm_JobEndDateAndTime").innerHTML = $("#valueReq").text();
        $("#txt_JobEndDateAndTime").css("border-color", "red");

    }
}
function validateJCDetailInsert() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 11-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var flag = CheckHeaderValidation();
    if (flag == false) {
        return false;
    }
    ///// Commented by Suraj maurya on 19-10-2023 for change in alternate item and dropdown changed to input
    //var Itemflag = CheckItemValidation();
    var flagSch = true;
    if ($("#OrderTypeSub").is(":checked") == false) {
        if (CalPendingSchQty() != 0) {
            flagSch = false;
            swal("", $("#SchOrdQtyEqualTo").text(), "warning")
        }
    }
    if (flag == true && flagSch==true) {
        var flagItem = false;
        var rowCount = $('#DnItmDetailsTbl >tbody >tr').length;
        if (rowCount > 0) {
            flagItem = true;
        }
        else {
            flagItem = false;
        }
        if (flagItem == true) {
        }
        else {
            swal("", $("#JC_noinputoutputitem").text(), "warning");
            return false;
        }
        var flagsubItemVald = CheckValidations_forSubItems();
        if (flagsubItemVald == false) {
            return false;
        }
        if (flag == true /*&& Itemflag == true*/) {
            validateJCDetailInsert1();

            /*----- Attatchment start--------*/
            $(".fileinput-upload").click();/*To Upload Img in folder*/
            FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
            /*----- Attatchment End--------*/
            /*----- Attatchment start--------*/
            var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
            $('#hdn_Attatchment_details').val(ItemAttchmentDt);
            /*----- Attatchment End--------*/

            /*----- Production Schedule start--------*/
            var ListProductionSch = fn_ListProductionSch();
            $("#HdnProductionSchDetail").val(JSON.stringify(ListProductionSch));
            /*----- Production Schedule End--------*/

            debugger;
            $("#Hdn_PrducOrderType").val();

            var SubItemsListArr = Cmn_SubItemList();
            var str2 = JSON.stringify(SubItemsListArr);
            $('#SubItemDetailsDt').val(str2);
            var Itm_Name = $("#ddl_ProductName option:selected").text();
            $("#hdn_product_name").val(Itm_Name);
            var OperationName = $("#ddl_OperationName option:selected").text();
            $("#hdn_OP_Name").val(OperationName);
            var UomName = $("#UOMName").val();
            $("#UOMName").val(UomName)
            var outUomName = $("#OutputUOMName").val();
            $("#OutputUOMName").val(outUomName)
            var WorkstationName = $("#ddl_WorkstationName option:selected").text();
            $("#hdn_WorkstationText").val(WorkstationName);
            var ddl_shiftName = $("#ddl_shift option:selected").text();
            $("#hdn_shiftName").val(ddl_shiftName);
            $("#hdnsavebtn").val("AllreadyclickSaveBtn");
            return true;
        }
    }
    else {
        return false;
    }
};

function validateJCDetailInsert1() {
    debugger;
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
        debugger;
        FinalItemDetail = InsertItemAttributeDetails();

        var Advice_No = $("#ddlAdviceNo option:selected").text();
        $("#hd_advice_no").val(Advice_No);

        debugger;
        var ItemAttrDt = JSON.stringify(FinalItemDetail);
        $('#hd_JCItemdetails').val(ItemAttrDt);

        return true;
    }
    else {
        //alert("Check network");
        return false;
    }
};
function OnChangeBatchNumber(e) {
    debugger;
    var txt_BatchNumber = $("#txt_BatchNumber").val();
    txt_BatchNumber = Cmn_RemoveSpacesAndTabs(txt_BatchNumber);
    $("#txt_BatchNumber").val(txt_BatchNumber);

    if (txt_BatchNumber != "") {
        $("#vm_BatchNumber").css("display", "none");
        $("#txt_BatchNumber").css("border-color", "#ced4da");
    }
    else {
        $('#vm_BatchNumber').text($("#valueReq").text());
        $("#vm_BatchNumber").css("display", "block");
        $("#txt_BatchNumber").css("border-color", "red");
        
    }
}
function OnKeyDownBatchNo(e) {
    debugger
    if (Cmn_Block_Keys(e, "Space") == false) {
        return false;
    }
}
function functionConfirm(event) {
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
function validateBOMDetailInsert() {
    debugger;
    var ddl_ProductName = $("#ddl_ProductName").val();
    var ValidationFlag = true;
    if (ddl_ProductName == "0") {

        document.getElementById("vmddl_ProductName").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
        ValidationFlag = false;
    }
    var txt_Quantity = $("#txt_Quantity").val();
    if (txt_Quantity == "0" || txt_Quantity == "") {
        document.getElementById("vm_Quantity").innerHTML = $("#valueReq").text();
        $("#txt_Quantity").css("border-color", "red");
        ValidationFlag = false;
    }
    if (ValidationFlag == true) {
        var Inflag = 'N';
        var Outflag = 'N';
        $("#tbladd >tbody >tr").each(function (i, row) {
            debugger;
            var Input = 'N';
            var Output = 'N';
            Inflag = 'N';
            Outflag = 'N';
            var currentRow = $(this);
            var op_id_tbl = currentRow.find("#tbl_hdn_ddl_op_id").val();
            $("#tbl_" + op_id_tbl + " >tbody >tr").each(function (j, rows) {
                var currentRowChild = $(this);
                var itemtypeid = currentRowChild.find("#tbl_hdn_ddl_ItemType_ID").val();
                if (itemtypeid == "I") {
                    Input = 'Y';
                    Inflag = 'Y';
                }
                if (itemtypeid == "O") {
                    Output = 'Y';
                    Outflag = 'Y';
                }

            });
            if (Input == "Y" && Output == "Y") {
                ValidationFlag = true;
            }
            else {
                ValidationFlag = false;
                return false;
            }
        });

        if (Inflag == "Y" && Outflag == "Y") {

        }
        else {
            swal("", $("#Eachoptreqatleastonin_outputitem").text(), "warning");
            return false;
        }

        if (ValidationFlag == true) {
            InsertItemDetail();
            return true;
        }
    }

    else {
        return false;
    }
}
function InsertItemAttributeDetails() {
    debugger;
    var txt_ProductionOrderNumber = $("#txt_ProductionOrderNumber").val();
    var ProductionOrderDate = $("#ProductionOrderDate").val();
    var itemDTransType = sessionStorage.getItem("ItmDTransType");
    if (itemDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var AttrList = [];
    debugger;
    $("#DnItmDetailsTbl >tbody >tr").each(function (j, rows) {
        var currentRowChild = $(this);
        debugger; 
        var SNo = currentRowChild.find("#SNohf").val();
        var hdn_item_id = currentRowChild.find("#hdn_item_id").val();
        var item_name = currentRowChild.find("#ItemListName").val();
        //var item_name = currentRowChild.find("#ItemListName" + SNo + " option:selected").text();
        //var whname = currentRow.find("#wh_id" + Sno + " option:selected").text();
        var hdn_uom_id = currentRowChild.find("#hdn_uom_id").val();
        var uom_name = currentRowChild.find("#UOM").val();
        var hdn_Item_type_id = currentRowChild.find("#hdn_Item_type_id").val().trim();
        var Item_type_name = currentRowChild.find("#ItemType").val().trim();
        var hdn_req_qty = currentRowChild.find("#hdn_req_qty").val();
        /*var sub_item = currentRowChild.find("#Insub_item").val();*/
        var seq_no = currentRowChild.find("#hdn_seq_no").val();
        var avl_stock_shfl = currentRowChild.find("#ShflQuantity").val();
        var avl_stock_warehouse = currentRowChild.find("#WhQuantity").val();
        var remarks = "";
        AttrList.push({ jc_no: txt_ProductionOrderNumber, jc_dt: ProductionOrderDate, item_id: hdn_item_id, item_name: item_name, uom_id: hdn_uom_id, uom_name: uom_name, Item_type_id: hdn_Item_type_id, Item_type_name: Item_type_name, req_qty: hdn_req_qty, /*sub_item: sub_item,*/ seq_no: seq_no, avl_stock_shfl: avl_stock_shfl, avl_stock_warehouse: avl_stock_warehouse, remarks: remarks })
    });

    $("#Output_ItmDetailsTbl >tbody >tr").each(function (j, rows) {
        var CurrRowChild = $(this);
        debugger;
        var item_id = CurrRowChild.find("#hdn_item_id").val();
        var item_name = CurrRowChild.find("#ItemName").val();
        var uom_id = CurrRowChild.find("#hdn_uom_id").val();
        var uom_name = CurrRowChild.find("#UOM").val();
        var Item_type_id = CurrRowChild.find("#hdn_Item_type_id").val().trim();
        var Item_type_name = CurrRowChild.find("#ItemType").val().trim();
        var req_qty = CurrRowChild.find("#hdn_req_qty").val();
        var seq_no = CurrRowChild.find("#hdn_seq_no").val();
        var pend_qty = CurrRowChild.find("#PendingQuantity").val();
        var remarks = CurrRowChild.find("#remarksOutputitm").val();
        AttrList.push({ jc_no: txt_ProductionOrderNumber, jc_dt: ProductionOrderDate, item_id: item_id, item_name: item_name, uom_id: uom_id, uom_name: uom_name, Item_type_id: Item_type_id, Item_type_name: Item_type_name, req_qty: req_qty, seq_no: seq_no, pend_qty: pend_qty, remarks: remarks })
    });

    return AttrList;
};
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var ItemId = clickedrow.find("#hdn_item_id").val();;
        //var ItemName = clickedrow.find("#ItemName").val();
        var ItemName = clickedrow.find("#ItemListName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var UOMID = clickedrow.find("#hdn_uom_id").val();

        $.ajax(
            {
                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId, UomId: UOMID
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

function onclickcancilflag() {

    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").prop('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").prop('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
};
function OnchangeAdviceNo() {
    var Advice_No = $("#ddlAdviceNo option:selected").text();
    var Advice_Dt = $("#ddlAdviceNo").val();
    var AdviceNo = $("#ddlAdviceNo").val();
    if (AdviceNo != "0") {
        var date = Advice_Dt.split("-");
        var FDate = date[2] + '-' + date[1] + '-' + date[0];
    }

    $("#Txt_AdviceDate").val(FDate);
    $("#hd_advice_no").val(Advice_No);

    if (AdviceNo == "0") {
        $('#vm_advice_no').text($("#valueReq").text());
        $("#vm_advice_no").css("display", "block");
        $("[aria-labelledby='select2-ddlAdviceNo-container']").css("border-color", "red");
        $("#Txt_AdviceDate").val("");
    }
    else {
        $("#vm_advice_no").css("display", "none");
        $("[aria-labelledby='select2-ddlAdviceNo-container']").css("border-color", "#ced4da");
    }
    if (Advice_No != "" && Advice_No != "0" && Advice_No != null && Advice_Dt != "" && Advice_Dt != "0" && Advice_Dt != null) {
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ProductionOrder/GetAdviceDetail",
                data: { AdviceNo: Advice_No, AdviceDate: FDate },
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
                            $('#ddl_OperationName option').remove();
                            /* $("#txt_JobQuantity").val(arr.Table[0].adv_qty);*/
                            $("#txt_BatchNumber").val(arr.Table[0].batch_no);

                            $('#ddl_OperationName').append('<option value="0" selected="selected">---Select---</option>');
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#ddl_OperationName').append('<option value="' + arr.Table[i].op_id + '">' + arr.Table[i].op_name + '</option>');
                            }
                        }
                        else {
                            $('#ddl_OperationName option').remove();
                            $('#ddl_OperationName').append('<option value="0" selected="selected">---Select---</option>');
                        }
                    }
                    else {
                        $('#ddl_OperationName option').remove();
                        $('#ddl_OperationName').append('<option value="0" selected="selected">---Select---</option>');
                    }
                },
            });
        } catch (err) {
            console.log("Error : " + err.message);
        }
    }
}

//----------------------------------------Work Flow----------------------------------------//

function CmnGetWorkFlowDetails(e) {
    //var clickedrow = $(e.target).closest("tr");
    //var PQ_No = clickedrow.children("td:eq(1)").text();
    //var PQ_Date = clickedrow.children("td:eq(2)").text();
    //var Doc_id = $("#DocumentMenuId").val();
    //$("#hdDoc_No").val(PQ_No);
    //GetWorkFlowDetails(PQ_No, PQ_Date, Doc_id);
    //var a = 1;
}
function CheckHeaderAndItemsValidation() {
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
        return false;
    }
    var flag = CheckHeaderValidation();
    ///// Commented by Suraj maurya on 19-10-2023 for change in alternate item and dropdown changed to input
    //var Itemflag = CheckItemValidation();
    if (flag == true /*&& Itemflag == true*/) {
        $("#hdnsavebtn").val("AllreadyclickApprove");
        return true;
      
    } else {
        return false;
    }
}
function ForwardBtnClick() {
    debugger;
    try {
    var OrderStatus = "";
    OrderStatus = $('#hfStatus').val().trim();
    var flag = CheckHeaderValidation();
    //var Itemflag = CheckItemValidation();
    if (flag == true /*&& Itemflag == true*/) {
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
            //$("#Btn_Forward").attr('onclick', '');
            //$("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        }
    } else {
        $("#btn_forward").attr("data-target", "");
        //$("#Btn_Forward").attr("data-target", "");
        //$("#Btn_Forward").attr('onclick', '');
        //$("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#btn_approve").attr("data-target", "");
        //$("#btn_approve").attr('onclick', '');
        //$("#btn_approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    return false;


    /*commented by hina sharma on 30-05-2025 as per discuss with vishal sir */
    ///*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
    //var compId = $("#CompID").text();
    //var brId = $("#BrId").text();
    //var PrdOrdDt = $("#ProductionOrderDate").val();
    //$.ajax({
    //    type: "POST",
    //    /*   url: "/Common/Common/CheckFinancialYear",*/
    //    url: "/Common/Common/CheckFinancialYearAndPreviousYear",
    //    data: {
    //        compId: compId,
    //        brId: brId,
    //        DocDate: PrdOrdDt
    //    },
    //    success: function (data) {
    //        /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
    //        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
    //        if (data == "TransAllow") {
    //            var OrderStatus = "";
    //            OrderStatus = $('#hfStatus').val().trim();
    //            var flag = CheckHeaderValidation();
    //            //var Itemflag = CheckItemValidation();
    //            if (flag == true /*&& Itemflag == true*/) {
    //                if (OrderStatus === "D" || OrderStatus === "F") {

    //                    if ($("#hd_nextlevel").val() === "0") {
    //                        $("#Btn_Forward").attr("data-target", "");
    //                    }
    //                    else {
    //                        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //                        $("#Btn_Approve").attr("data-target", "");
    //                    }
    //                    var Doc_ID = $("#DocumentMenuId").val();
    //                    $("#OKBtn_FW").attr("data-dismiss", "modal");

    //                    Cmn_GetForwarderList(Doc_ID);

    //                }
    //                else {
    //                    $("#Btn_Forward").attr("data-target", "");
    //                    //$("#Btn_Forward").attr('onclick', '');
    //                    //$("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //                }
    //            } else {
    //                $("#btn_forward").attr("data-target", "");
    //                //$("#Btn_Forward").attr("data-target", "");
    //                //$("#Btn_Forward").attr('onclick', '');
    //                //$("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //                $("#btn_approve").attr("data-target", "");
    //                //$("#btn_approve").attr('onclick', '');
    //                //$("#btn_approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //            }
    //        }
    //        else {/* to chk Financial year exist or not*/
    //            /*  swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
    //            /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
    //            swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
    //            $("#Btn_Forward").attr("data-target", "");
    //            $("#Btn_Approve").attr("data-target", "");
    //            $("#Forward_Pop").attr("data-target", "");

    //        }
    //    }
    //});
    ///*End to chk Financial year exist or not*/
    //return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var DocNo = "";
    var DocDate = "";
    var AdvNo = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var OrderType = "";
    var forwardedto = "";
    var SrcType = "";
    var WF_Status1 = "";
    var ModelData = "";
    var mailerror = "";


    SrcType = $("#Hdn_SrcType").val();
    OrderType = $("#Hdn_PrducOrderType").val();
    Remarks = $("#fw_remarks").val();
    DocNo = $("#txt_ProductionOrderNumber").val();
    DocDate = $("#ProductionOrderDate").val();
    var Hdn_SrcType = $("#Hdn_SrcType").val();
    if (Hdn_SrcType == "A") {
        AdvNo = $("#ddlAdviceNo").val();
    }
    
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
    if (DocDate.split('-')[2].length == 4) {
        DocDate = DocDate.split('-').reverse().join('-');
    }
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "ProductionOrder_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/ProductionOrder/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ProductionOrder/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ DocNo: DocNo, DocDate: DocDate, OrderType: OrderType, SrcType: SrcType, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks, product_id: $("#hdn_product_id").val(), Adv_No: AdvNo }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/ProductionOrder/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ProductionOrder/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ProductionOrder/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(jcNo, jcDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/ProductionOrder/SavePdfDocToSendOnEmailAlert",
//        data: { jcNo: jcNo, jcDate: jcDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function OtherFunctions(StatusC, StatusName) {

}

function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#txt_ProductionOrderNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}

//----------------------------------------Work Flow End----------------------------------------//

function OnClickProductionQtyDetail(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();

    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdn_op_item_id").val();
    var ItemName = clickedrow.find("#ItemName").val();
    var UOM = clickedrow.find("#UOM").val();
    var TotalProdQty = clickedrow.find("#ProdQuantity").val();
    var TotalAccQty = clickedrow.find("#AcceptQuantity").val();
    var TotalRejQty = clickedrow.find("#RejectQuantity").val();
    var TotalRewQty = clickedrow.find("#Rework_qtyQuantity").val();


    $("#IconProductionDescription").val(ItemName);
    $("#IconUOM").val(UOM);

    $('#TotProd').text(TotalProdQty);
    $('#TotAcc').text(TotalAccQty);
    $('#TotRej').text(TotalRejQty);
    $('#TotRew').text(TotalRewQty);

    ConfirmationDetail(ItmCode);

}
function OnClickConsumptionDetail(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();

    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdn_inp_item_id").val();
    var ItemName = clickedrow.find("#ItemName").val();
    var UOM = clickedrow.find("#UOM").val();
    var TotalConsQty = clickedrow.find("#Quantity").val();


    $("#InpIconProductionDescription").val(ItemName);
    $("#InpIconUOM").val(UOM);
    $("#TotCons").text(TotalConsQty);

    ConfirmationDetail(ItmCode);

}
function ConfirmationDetail(ItmID) {
    debugger;
    //var clickedrow = $(e.target).closest("tr");
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var ItemID = ItmID;
    var JCNumber = $("#txt_ProductionOrderNumber").val();
    var JCDate = $("#ProductionOrderDate").val();
    var jc_dt = JCDate;
    var date = jc_dt.split("-");
    var FDate = date[2] + '-' + date[1] + '-' + date[0];

    if (ItemID != "" && ItemID != null && JCNumber != "" && JCNumber != null) {
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/ProductionOrder/GetConfirmationDetail",
                data: {
                    ItemID: ItemID, JCNumber: JCNumber, JCDate: JCDate
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        $('#OutputItemDtTbl tbody tr').remove();
                        $('#ConsDtTbl tbody tr').remove();
                        if (arr.Table1.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table1.length; i++) {

                                ++rowIdx;
                                debugger;

                                $('#OutputItemDtTbl tbody').append(` <tr id="${rowIdx}">

                                <td>${rowIdx}</td>
                            <td>${arr.Table1[i].cnf_no}</td>
                            <td>${arr.Table1[i].cnf_dt}</td>
                            <td class="num_right">${parseFloat(arr.Table1[i].prod_qty).toFixed(QtyDecDigit)}</td>
                            <td class="num_right">${parseFloat(arr.Table1[i].accept_qty).toFixed(QtyDecDigit)}</td>
                            <td class="num_right">${parseFloat(arr.Table1[i].reject_qty).toFixed(QtyDecDigit)}</td>
                            <td class="num_right">${parseFloat(arr.Table1[i].rework_qty).toFixed(QtyDecDigit)}</td>

                                
                </tr>`);

                            }

                        }
                        else {
                            $("#OutputItemDtTbl >tbody >tr").remove();
                            $('#TotProd').text(parseFloat(0).toFixed(QtyDecDigit));
                            $('#TotAcc').text(parseFloat(0).toFixed(QtyDecDigit));
                            $('#TotRej').text(parseFloat(0).toFixed(QtyDecDigit));
                            $('#TotRew').text(parseFloat(0).toFixed(QtyDecDigit));
                        }

                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table.length; i++) {

                                ++rowIdx;
                                debugger;
                                //var ConsQty;
                                //var TotalConsQty = "";  

                                //var ConsQty = arr.Table[i].TotalConsQty;
                                //if (ConsQty == "" || ConsQty == null) {
                                //    ConsQty = "0";
                                //}
                                //TotalConsQty = ConsQty;

                                $('#ConsDtTbl tbody').append(` <tr id="${rowIdx}">

                                <td>${rowIdx}</td>
                            <td>${arr.Table[i].cnf_no}</td>
                            <td>${arr.Table[i].cnf_dt}</td>
                            <td class="num_right">${parseFloat(arr.Table[i].cons_qty).toFixed(QtyDecDigit)}</td>                                
                </tr>`);

                            }
                            //if (TotalConsQty == "") {
                            //    $('#TotCons').text(parseFloat(0).toFixed(ValDecDigit));
                            //}
                            //else {
                            //    $("#TotCons").text(parseFloat(TotalConsQty).toFixed(QtyDecDigit));
                            //}                          
                        }
                        else {
                            $("#ConsDtTbl >tbody >tr").remove();
                            $('#TotCons').text(parseFloat(0).toFixed(QtyDecDigit));

                        }

                    }
                    else {
                        $("#OutputItemDtTbl >tbody >tr").remove();
                        $('#TotProd').text(parseFloat(0).toFixed(QtyDecDigit));
                        $('#TotAcc').text(parseFloat(0).toFixed(QtyDecDigit));
                        $('#TotRej').text(parseFloat(0).toFixed(QtyDecDigit));
                        $('#TotRew').text(parseFloat(0).toFixed(QtyDecDigit));

                        $("#ConsDtTbl >tbody >tr").remove();
                        $('#TotCons').text(parseFloat(0).toFixed(QtyDecDigit));
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

    }

}
function OnchangeOrderQty() {
    debugger
    var txt_JobQty = $("#txt_JobQuantity").val();
    var hdtxt_JobQty = $("#hdtxt_JobQuantity").val();
    /*Code add by Hina on 02-03-2024 to show blank instead of 0 in insert fields*/
    if (txt_JobQty == "0" || txt_JobQty == parseFloat(0)) {
        $("#txt_JobQuantity").val("");
    }
    /*Code End*/
    if (CheckNullNumber(txt_JobQty) > 0) {

        var ChangePerc = ((parseFloat(CheckNullNumber(txt_JobQty)) - parseFloat(CheckNullNumber(hdtxt_JobQty))) / parseFloat(CheckNullNumber(hdtxt_JobQty))) * 100;

        if (parseFloat(CheckNullNumber(txt_JobQty)) > 0) {
            $("#vm_JobQuantity").css("display", "none");
            $("#txt_JobQuantity").css("border-color", "#ced4da");
        }
        else {
            $('#vm_JobQuantity').text($("#valueReq").text());
            $("#vm_JobQuantity").css("display", "block");
            $("#txt_JobQuantity").css("border-color", "red");
        }
        $("#DnItmDetailsTbl tbody tr").each(function () {
            var Crow = $(this);
            var ReqQuantity = Crow.find("#ReqQuantity").val();
            var NewReqQuantity = parseFloat(CheckNullNumber(ReqQuantity)) + parseFloat(CheckNullNumber(ReqQuantity)) * parseFloat(ChangePerc) / 100
            Crow.find("#ReqQuantity").val(parseFloat(NewReqQuantity).toFixed(QtyDecDigit));
            Crow.find("#hdn_req_qty").val(parseFloat(NewReqQuantity).toFixed(QtyDecDigit));
        });

        $("#Output_ItmDetailsTbl tbody tr").each(function () {
            var Crow = $(this);
            var Quantity = Crow.find("#Quantity").val();
            var NewQuantity = parseFloat(CheckNullNumber(Quantity)) + parseFloat(CheckNullNumber(Quantity)) * parseFloat(ChangePerc) / 100
            Crow.find("#Quantity").val(parseFloat(NewQuantity).toFixed(QtyDecDigit));
            Crow.find("#PendingQuantity").val(parseFloat(NewQuantity).toFixed(QtyDecDigit));
            Crow.find("#hdn_req_qty").val(parseFloat(NewQuantity).toFixed(QtyDecDigit));

        });

        $("#hdtxt_JobQuantity").val(CheckNullNumber(txt_JobQty));
        //$("#txt_JobQuantity").prop("readonly", true);
    }
    
}



/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemOrdQty",);
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var OutputItemID = $("#OutputItemID").val();
    //var hd_advice_no = $("#hd_advice_no").val();
    //var hfsno = clickdRow.find("#hfsno").val();
    if (flag == "Quantity") {
        var ProductNm = $("#OutputItemName").val();
        /*var ProductId = $("#ddl_ProductName").val();*//*Commented and add var ProductId = $("#OutputItemID").val(); by Hina sharma on 28-10-2024 to change Operation Output Product Id instead of finished Product ID*/
        var ProductId = $("#OutputItemID").val();
        var UOM = $("#OutputUOM").val();
    }
    else if (flag == "PrdOrdReqQty") {
        ProductNm = clickdRow.find("#ItemListName").val();
        ProductId = clickdRow.find("#hdn_item_id").val();
        UOM = clickdRow.find("#UOM").val();
        /*Sub_Quantity = $("#ReqQuantity").val();*/
    }
    else if (flag == "PrdOrdOutQty") {
        ProductNm = clickdRow.find("#ItemName").val();
        ProductId = clickdRow.find("#hdn_item_id").val();
        UOM = clickdRow.find("#UOM").val();
        /*Sub_Quantity = $("#Quantity").val();*/
    }
    //var UOMID = clickdRow.find("#UOMID").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "Quantity" || flag == "PrdOrdReqQty" || flag == "PrdOrdOutQty") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            var Qty = row.find('#subItemQty').val();
            if (Qty == "0") {
                Qty = "";
                List.qty = Qty;
            }
            else {
                List.qty = row.find('#subItemQty').val();
            }
            NewArr.push(List);
        });
        if (flag == "Quantity") {
            Sub_Quantity = $("#txt_JobQuantity").val();
        }
        else if (flag == "PrdOrdReqQty") {
            Sub_Quantity = clickdRow.find("#ReqQuantity").val();
        }
        else if (flag == "PrdOrdOutQty") {
            Sub_Quantity = clickdRow.find("#Quantity").val();
        }


    }
    else if (flag == "PrdOrdReqQty") {
        Sub_Quantity = clickdRow.find("#ReqQuantity").val();
    }
    else if (flag == "PrdOrdOutQty") {
        Sub_Quantity = clickdRow.find("#Quantity").val();
    }
    else if (flag == "QCAccQty") {
        ProductNm = clickdRow.find("#ItemName").val();
        ProductId = clickdRow.find("#hdn_op_item_id").val();
        UOM = clickdRow.find("#UOM").val();
        Sub_Quantity = clickdRow.find("#AcceptQuantity").val();
    }
    else if (flag == "QCRejQty") {
        ProductNm = clickdRow.find("#ItemName").val();
        ProductId = clickdRow.find("#hdn_op_item_id").val();
        UOM = clickdRow.find("#UOM").val();
        Sub_Quantity = clickdRow.find("#RejectQuantity").val();
    }
    else if (flag == "QCRewQty") {
        ProductNm = clickdRow.find("#ItemName").val();
        ProductId = clickdRow.find("#hdn_op_item_id").val();
        UOM = clickdRow.find("#UOM").val();
        Sub_Quantity = clickdRow.find("#Rework_qtyQuantity").val();
    }
    else if (flag == "POMRS_Quantity" || flag == "POMRS_Issue")
    {
        ProductNm = clickdRow.find("#MRS_ItemName").val();
        ProductId = clickdRow.find("#MRS_ItemID").val();
        UOM = clickdRow.find("#MRS_UOM").val();
        if (flag == "POMRS_Quantity") {
            Sub_Quantity = clickdRow.find("#mrs_qty").val();
        }
        else {
            Sub_Quantity = clickdRow.find("#mrd_issue_qty").val();
        }
    }
    else {
        ProductNm = clickdRow.find("#ItemName").val();
        ProductId = clickdRow.find("#hdn_op_item_id").val();
        UOM = clickdRow.find("#UOM").val();
        Sub_Quantity = clickdRow.find("#ProdQuantity").val();
    }
    if (flag == "POMRS_Quantity") {
        var IsDisabled = "Y";
    }
    else {
        var IsDisabled = $("#DisableSubItem").val();
    }
    if ($("#SourceTypePA").is(":checked") == true) {
        IsDisabled = "Y";
    }
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    var jc_no = $("#txt_ProductionOrderNumber").val();
    var jc_dt = $("#ProductionOrderDate").val();


    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ProductionOrder/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            jc_no: jc_no,
            jc_dt: jc_dt,
            OutputItemID: OutputItemID
            
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
    if (flag == 'enable') {

    }
    else if (flag = 'readonly') {

    }
}
function CheckValidations_forSubItems() {
    /*return Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y");*/
    var ErrFlg = "";
    //if (Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y") == false) {
    //    ErrFlg = "Y"
    //}/*Commented and change OutputItemID by Hina sharma on 28-10-2024 to change Operation Output Product Id instead of Finished Product ID*/
    if (Cmn_CheckValidations_forSubItemsNonTable("OutputItemID", "txt_JobQuantity", "SubItemOrdQty", "Y") == false) {
        ErrFlg = "Y"
    }
    if (PrdOrd_CheckValidations_forSubItems("DnItmDetailsTbl", "", "hdn_item_id", "ReqQuantity", "SubItemReqirQty","PrdOrdReqQty","Y") == false) {
        ErrFlg = "Y"
    }
    if (PrdOrd_CheckValidations_forSubItems("Output_ItmDetailsTbl", "", "hdn_item_id", "Quantity", "SubItemOutQty","PrdOrdOutQty","Y") == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }
}
function ResetWorningBorderColor() {
    //return Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "N");
    var ErrFlg = "";
    //if (Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "N") == false) {
    //    ErrFlg = "Y"
    //}/*Commented and change OutputItemID by Hina sharma on 28-10-2024 to change Operation Output Product Id instead of Finished Product ID*/
    if (Cmn_CheckValidations_forSubItemsNonTable("OutputItemID", "txt_JobQuantity", "SubItemOrdQty", "Y") == false) {
        ErrFlg = "Y"
    }
    if (PrdOrd_CheckValidations_forSubItems("DnItmDetailsTbl", "", "hdn_item_id", "ReqQuantity", "SubItemReqirQty", "PrdOrdReqQty", "N") == false) {
        ErrFlg = "Y"
    }
    if (PrdOrd_CheckValidations_forSubItems("Output_ItmDetailsTbl", "", "hdn_item_id", "Quantity", "SubItemOutQty", "PrdOrdOutQty", "N") == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }
}
function PrdOrd_CheckValidations_forSubItems(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton,FlagPg, ShowMessage) {
    debugger;
    
    var flag = "N";
    $("#" + Main_table + " tbody tr").each(function () {
        var PPItemRow = $(this);
        var item_id;
        if (Row_Id != "" && Row_Id != null) {
            var Sno = PPItemRow.find("#" + Row_Id).val();
            item_id = PPItemRow.find("#" + Item_field_id + Sno).val();
        }
        else
            {
           item_id = PPItemRow.find("#" + Item_field_id).val();
         }

        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
        if (FlagPg == "PrdOrdReqQty")
        {
            var sub_item = PPItemRow.find("#Insub_item").val();
        }
        else {
            var sub_item = PPItemRow.find("#Outsub_item").val();
        }

        /*var sub_item = PPItemRow.find("#sub_item").val();*/
        var Sub_Quantity = 0;
        $("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr').each(function () {
            var Crow = $(this).closest("tr");
            //var subItemId = Crow.find("#subItemId").val();
            var subItemQty = Crow.find("#subItemQty").val();
            Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
        });

        if (sub_item == "Y") {
            if (item_PrdQty != Sub_Quantity) {
                flag = "Y";
                PPItemRow.find("#" + SubItemButton).css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + SubItemButton).css("border", "");
            }
        }
    });

    if (flag == "Y") {
        if (ShowMessage == "Y") {

            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }

        return false;
    }
    else {
        return true;
    }
}
function GetSubItemAvlStock(e,flag) {/*Add by Hina sharma on 18-11-2024 for show sub item stock byWarehouse or byShopfloor*/
    debugger;
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#SNohf").val();
    var ProductNm = Crow.find("#ItemListName").val();
    var ProductId = Crow.find("#hdn_item_id").val();
    var UOMId = Crow.find("#hdn_uom_id").val();
    var UOM = Crow.find("#UOM").val();
    var status = $("#hfStatus").val();
    var Doc_no = $("#txt_ProductionOrderNumber").val();
    var Doc_dt = $("#ProductionOrderDate").val();
    if (flag == "WH") {
        var AvlStk = Crow.find("#WhQuantity").val();
        if (status == "D" || status == "F") {
            Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, "0", AvlStk, "br", UOMId);
        }
        else {
            GetSubitemDetailwithstock(ProductNm, ProductId, UOM, "0", AvlStk, "br", UOMId, flag, Doc_no, Doc_dt);
        }
     
    }
    else {
        var AvlStk = Crow.find("#ShflQuantity").val();
        if (flag == "Shfl") {
            GetSubitemDetailwithstock(ProductNm, ProductId, UOM, "0", AvlStk, "AC", UOMId, flag, Doc_no, Doc_dt);          
        }
        else {
            if (status == "D" || status == "F" || flag == "whres") {
                Cmn_SubItemShopfloorAvlStock(ProductNm, ProductId, UOMId, "0", AvlStk, "AC");
            }
            else {
                GetSubitemDetailwithstock(ProductNm, ProductId, UOM, "0", AvlStk, "AC", UOMId, flag, Doc_no, Doc_dt);
            }
        }
       
    }
}
function GetSubitemDetailwithstock(ProductNm, ProductId, UOM, Wh_id, ItemQty, flag, UomId, flag1, Doc_no, Doc_dt) {
    var br_id = "";
    var DocumentMenuId = $("#DocumentMenuId").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ProductionOrder/GetSubItemWareshflAvlstockDetails",
        data: {
            Wh_id: Wh_id,
            Item_id: ProductId,
            flag: flag,
            DocumentMenuId: DocumentMenuId,
            UomId: UomId,         
            flag1: flag1,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt
        },
        success: function (data) {

            $("#SubItemStockPopUp").html(data);
            $("#Stk_Sub_ProductlName").val(ProductNm);
            $("#Stk_Sub_ProductlId").val(ProductId);
            $("#Stk_Sub_serialUOM").val(UOM);
            $("#Stk_Sub_Quantity").val(ItemQty);
        }

    });
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

/***--------------------------------Production Schedule Section-----------------------------------------***/
function AddProductionSchedule() {
    debugger
    var ProdSchDate = $("#ProductionSchDate").val();
    var ProdSchQuantity = $("#ProductionSchQuantity").val();
    var ProdSchRemarks = $("#ProductionSchRemarks").val();
    if (validateProductionSchFields() == true) {
        var RowNo = $("#ProductionSchDetailsTbl tbody tr").length;
        $("#ProductionSchDetailsTbl tbody").append(`
                 <tr>
                     <td>${RowNo + 1}</td>
                     <td class=" red center"> <i class=" fa fa-trash deleteIcon" aria-hidden="true" id="" title="${$("#Span_Delete_Title").text()}"></i></td>
                     <td class="center edit_icon"> <i class=" fa fa-edit" aria-hidden="true" onclick="EditSchedule(event);" id="" title="${$("#Edit").text()}"></i></td>
                     <td id="SchDate">${moment(ProdSchDate).format('DD-MM-YYYY')}</td>
                     <td hidden id="SchDate1">${ProdSchDate}</td>
                     <td class="num_right" id="SchQty">${ProdSchQuantity}</td>
                     <td id="SchRemarks">${ProdSchRemarks}</td
                 </tr>
    `)
        DeleteDeliverySchdule();
        ResetPrdSchedule();
    }
}
function DeleteDeliverySchdule() {
    $(".deleteIcon").on("click", function () {
        var row = $(this).closest("tr");
        row.remove();
    });
}
function validateProductionSchFields() {
    var ProdSchDate = $("#ProductionSchDate").val();
    var ProdSchQuantity = $("#ProductionSchQuantity").val();
    debugger;
    var flagErr = "N";
    var PrdStartDt = $("#txt_JobStartDateAndTime").val();
    var PrdEndDt = $("#txt_JobEndDateAndTime").val();
    if (CheckProdSchDate(ProdSchDate) == true) {
        if (/*moment(ProdSchDate) >= moment(PrdStartDt)*/moment(ProdSchDate) >= moment(moment(PrdStartDt).format('YYYY-MM-DD')) && moment(ProdSchDate) <= moment(moment(PrdEndDt).format('YYYY-MM-DD'))) {
            $("#ProductionSchDate").css("border-color", "#ced4da");
            $('#vm_ProductionSchDate').text("");
            $('#vm_ProductionSchDate').css("display", "none");
        } else {
            $("#ProductionSchDate").css("border-color", "red");
            $('#vm_ProductionSchDate').text($("#JC_InvalidDate").text());
            $('#vm_ProductionSchDate').css("display", "block");
            flagErr = "Y";

        }
    } else {
        $("#ProductionSchDate").css("border-color", "red");
        $('#vm_ProductionSchDate').text($("#JC_InvalidDate").text());
        $('#vm_ProductionSchDate').css("display", "block");
        flagErr = "Y";
    }
    
    if (parseFloat(ProdSchQuantity) > parseFloat(0)) {

        if (CheckSchQuantity(ProdSchQuantity) == false) {
            flagErr = "Y";
            $("#ProductionSchQuantity").css("border-color", "red");
            $('#vm_ProductionSchQuantity').text($("#ExceedingQty").text());
            $('#vm_ProductionSchQuantity').css("display", "block");
        } else {
            $("#ProductionSchQuantity").css("border-color", "#ced4da");
            $('#vm_ProductionSchQuantity').text("");
            $('#vm_ProductionSchQuantity').css("display", "none");
        }
    } else {
        flagErr = "Y";
        $("#ProductionSchQuantity").css("border-color", "red");
        $('#vm_ProductionSchQuantity').text($("#valueReq").text());
        $('#vm_ProductionSchQuantity').css("display", "block");
    }
    
    if (flagErr == "Y") {
        return false;
    } else {
        return true;
    }

}
function CheckSchQuantity(ProdSchQuantity) {
    var Qty = 0;
    var OrderQty = $("#txt_JobQuantity").val();
    let SchDate = $("#ProductionSchDate").val();
    $("#ProductionSchDetailsTbl tbody tr").each(function () {
        var Row = $(this);
        if (SchDate != Row.find("#SchDate1").text()) {
            Qty = parseFloat(Qty) + parseFloat(CheckNullNumber(Row.find("#SchQty").text()));
        }
        
    });
    if (parseFloat(Qty) + parseFloat(ProdSchQuantity) <= parseFloat(CheckNullNumber(OrderQty))) {
        return true;
    } else {
        return false;
    }
}
function OnClickProductionSchDate() {
    var PrdStartDt = $("#txt_JobStartDateAndTime").val();
    var PrdEndDt = $("#txt_JobEndDateAndTime").val();

    $("#ProductionSchDate").attr("min", moment(PrdStartDt).format('YYYY-MM-DD'))
    $("#ProductionSchDate").attr("max", moment(PrdEndDt).format('YYYY-MM-DD'))

}
function OnChangeProductionSchDate() {
    var SchDate = $("#ProductionSchDate").val();

    if (CheckProdSchDate(SchDate)) {
        $("#ProductionSchDate").css("border-color", "#ced4da");
        $('#vm_ProductionSchDate').text("");
        $('#vm_ProductionSchDate').css("display", "none");

        $("#ProductionSchQuantity").css("border-color", "#ced4da");
        $('#vm_ProductionSchQuantity').text("");
        $('#vm_ProductionSchQuantity').css("display", "none");

    } else {
        $("#ProductionSchDate").css("border-color", "red");
        $('#vm_ProductionSchDate').text($("#JC_InvalidDate").text());
        $('#vm_ProductionSchDate').css("display", "block");
    }
    $("#ProductionSchQuantity").val(CalPendingSchQty());

}
function OnChangeProductionSchQuantity() {

    var SchQty = $("#ProductionSchQuantity").val();
    if (parseFloat(CheckNullNumber(SchQty)) > parseFloat(0)) {
        if (CalPendingSchQty() >= parseFloat(CheckNullNumber(SchQty))) {
            $("#ProductionSchQuantity").css("border-color", "#ced4da");
            $('#vm_ProductionSchQuantity').text("");
            $('#vm_ProductionSchQuantity').css("display", "none");
        }
        else {
            $("#ProductionSchQuantity").css("border-color", "red");
            $('#vm_ProductionSchQuantity').text($("#ExceedingQty").text());
            $('#vm_ProductionSchQuantity').css("display", "block");
        }
    } else {
        $("#ProductionSchQuantity").css("border-color", "red");
        $('#vm_ProductionSchQuantity').text($("#valueReq").text());
        $('#vm_ProductionSchQuantity').css("display", "block");
    }
   
}
function fn_ListProductionSch() {
    var arr = [];
    $("#ProductionSchDetailsTbl tbody tr").each(function () {
        var Row = $(this);
        var List = {};
        List.sch_date_toShow = Row.find("#SchDate").text();
        List.sch_date = Row.find("#SchDate1").text();
        List.sch_qty = Row.find("#SchQty").text();
        List.remarks = Row.find("#SchRemarks").text();
        arr.push(List);
    });
    return arr;
}
function ResetPrdSchedule() {

    $("#ProductionSchDate").attr("min","");
    $("#ProductionSchDate").attr("max","");

    $("#ProductionSchDate").val("").attr("disabled", false);
    

    $("#ProductionSchQuantity").val(CalPendingSchQty());
    $("#ProductionSchRemarks").val("");
}
function CalPendingSchQty() {
    var Qty = 0;
    var OrderQty = $("#txt_JobQuantity").val();

    let SchDate = $("#ProductionSchDate").val();

    $("#ProductionSchDetailsTbl tbody tr").each(function () {
        var Row = $(this);
        if (SchDate != Row.find("#SchDate1").text()) {
            Qty = parseFloat(Qty) + parseFloat(CheckNullNumber(Row.find("#SchQty").text()));
        }
    });
    var pending = parseFloat(CheckNullNumber(OrderQty)) - parseFloat(Qty);
    return pending;
}
function CheckProdSchDate(SchDate) {
    let ProdSchDate = $("#ProductionSchDate").attr("disabled");
    if (ProdSchDate) {
        return true;
    } else {
        var len = $("#ProductionSchDetailsTbl tbody tr #SchDate1:contains('" + SchDate + "')").length;
        if (len > 0) {
            return false;
        } else {
            return true;
        }
    }
    
}
function EditSchedule(e) {
    var Crow = $(e.target).closest("tr");
    $("#BtnUpdateScheduleData").css("display", "block");
    $("#BtnAddScheduleData").css("display", "none");
    let SchDate = Crow.find("#SchDate1").text();
    let SchQty = Crow.find("#SchQty").text();
    let SchRemarks = Crow.find("#SchRemarks").text();
    $("#ProductionSchDate").val(SchDate).attr("disabled",true);

    $("#ProductionSchQuantity").val(SchQty);
    $("#ProductionSchRemarks").val(SchRemarks);
}
function UpdateSchedule() {

    if (validateProductionSchFields() == true) {
        $("#BtnAddScheduleData").css("display", "block");
        $("#BtnUpdateScheduleData").css("display", "none");

        let SchDate = $("#ProductionSchDate").val();
        let SchQty = $("#ProductionSchQuantity").val();
        let SchRemarks = $("#ProductionSchRemarks").val();

        var Crow = $("#ProductionSchDetailsTbl tbody tr td:contains('" + SchDate + "')").closest("tr");

        //Crow.find("#SchDate1").text(SchDate);
        //Crow.find("#SchDate").text(moment(SchDate).format('DD-MM-YYYY'));
        Crow.find("#SchQty").text(SchQty);
        Crow.find("#SchRemarks").text(SchRemarks);
        ResetPrdSchedule();
    }
    
}

/***--------------------------------Production Schedule Section End-----------------------------------------***/
/*----------------------------------------------Alternate Item Detail-------------------------------------------------*/
function OnClickAlternateItem(e) {
    var Crow = $(e.target).closest('tr');
    let Item_Name = Crow.find("#ItemListName").val();
    let Item_Id = Crow.find("#hdn_item_id").val();
    let uom = Crow.find("#UOM").val();
    let ItemType = Crow.find("#ItemType").val();
    let Product_Id = $("#ddl_ProductName").val();
    let Op_Id = $("#ddl_OperationName").val();
    let shfl_id = $("#ddl_ShopfloorName").val();
    let Disabled = $("#DisableSubItem").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ProductionOrder/GetAlternateItemDetails",
        data: { Product_Id: Product_Id, Op_Id: Op_Id, Item_Id: Item_Id, shfl_id: shfl_id, Disabled: Disabled },
        success: function (data) {
            debugger;
            $("#PopupPartialSelectAlternateItem").html(data);
            SelectAltItem();
            $("#Alt_PP_ItemType").val(ItemType);
            $("#Alt_PP_ItemName").val(Item_Name);
            $("#Alt_PP_ItemId").val(Item_Id);
            $("#Alt_PP_UOM").val(uom);
        }
    })

}
function SelectAltItem() {
    $("#AltItemDetailTable tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");

        if (clickedrow.find('#CheckedAltItem').prop("disabled") == false) {
            $("#AltItemDetailTable >tbody >tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
            clickedrow.find('#CheckedAltItem').prop("checked", true);
            var alt_item_id = clickedrow.find("#alt_item_id").text();
            $("#hdnAltItemId").val(alt_item_id);
            $("#BtnAltSaveAndExit").attr("disabled", false);
        }
    });
}
function AltItemSaveAndExit() {
    let AltItemID = $("#hdnAltItemId").val();
    let ItemID = $("#Alt_PP_ItemId").val();
    let prod_qty = $("#txt_JobQuantity").val();
    let Product_Id = $("#ddl_ProductName").val();
    let Op_Id = $("#ddl_OperationName").val();
    let shfl_id = $("#ddl_ShopfloorName").val();
    var AltItemName, AltReqQty, Shfl_stk, Wh_Stk, uom_id, uom_alias, sub_item;
    debugger
    if (AltItemID != "" && $("#AltItemDetailTable > tbody > tr #alt_item_id:contains('" + AltItemID + "')").length > 0) {
        $("#AltItemDetailTable > tbody > tr #alt_item_id:contains('" + AltItemID + "')").closest('tr').each(function () {
            var row = $(this);
            AltItemName = row.find("#alt_item_name").text();
            AltReqQty = row.find("#alt_req_qty").text();
            Shfl_stk = row.find("#alt_shfl_stk").text();
            Wh_Stk = row.find("#alt_wh_stk").text();
            uom_id = row.find("#Alt_UOM_Id").text();
            uom_alias = row.find("#Alt_UOM").text();
            sub_item = row.find("#alt_sub_item").text();
        });

        $("#DnItmDetailsTbl > tbody > tr").each(function () {
            var row = $(this);
            if (row.find("#hdn_item_id").val() == ItemID) {
                row.find("#ItemListName").val(AltItemName);
                row.find("#ItemName").val(AltItemName);
                //row.find("#in_ItemId").val(AltItemID).trigger('change');
                row.find("#hdn_item_id").val(AltItemID).trigger('change');
                row.find("#Insub_item").val(sub_item);
                row.find("#UOM").val(uom_alias);
                row.find("#hdn_uom_id").val(uom_id);
                row.find("#ShflQuantity").val(parseFloat(Shfl_stk).toFixed(QtyDecDigit));
                row.find("#WhQuantity").val(parseFloat(Wh_Stk).toFixed(QtyDecDigit));
                row.find("#ReqQuantity").val(parseFloat(parseFloat(AltReqQty) * parseFloat(prod_qty)).toFixed(QtyDecDigit));
                row.find("#hdn_req_qty").val(parseFloat(parseFloat(AltReqQty) * parseFloat(prod_qty)).toFixed(QtyDecDigit));
                if (sub_item == "Y") {
                    row.find("#SubItemReqirQty").attr("disabled",false);
                } else {
                    row.find("#SubItemReqirQty").attr("disabled",true);
                }

            }

        });
    }
   
   
    $("#BtnAltSaveAndExit").attr("data-dismiss", "modal");
}
/*----------------------------------------------Alternate Item Detail End-------------------------------------------------*/
function onclickAutoMrs() {
    if ($("#AutoMRS").is(":checked")) {
        $("#hdn_automrs").val("Y");
    }
    else {
        $("#hdn_automrs").val("N");
    }
}


