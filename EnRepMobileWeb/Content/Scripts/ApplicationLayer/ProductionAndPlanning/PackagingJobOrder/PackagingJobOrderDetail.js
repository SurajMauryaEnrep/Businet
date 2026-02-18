$(document).ready(function () {
    debugger;
    //$('#ddl_ItemName').append(`<option value="0">---Select---</option>`);
    //BindItemsList();
    //$("#ddl_ItemName").select2();
    DynamicSerchableItemDDL("", "#ddl_ItemName", "", "", "", "PkgJobOrdr");/* Added by Suraj Maurya on 28-07-2025 */
    $("#ddl_WarehouseName").select2();
    $("#ddl_ShopfloorName").select2();


    $('#TblConsumedMaterialDetail').on('click', '.deleteIcon', function () {
        debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#ConsumeMaterialId").val();
        DeleteCMSubItemQtyDetail(ItemCode,"SubCM")
        
        DeleteItemBatchSerialDetails(ItemCode)
        updateConsumeMaterialSRNumber();
        
    });
    
    dn_no = $("#JobCardNumber").val();
    $("#hdDoc_No").val(dn_no);
});
function OnClickIconBtn(e, flag) { /*Added By Nitesh 03-05-2024 for Item Information*/
    debugger;
    var ItmCode = "";
    if (flag != 'Header' && flag != 'Req') {
        var clickedrow = $(e.target).closest("tr");
        if (flag == 'Reqtbl') {
            ItmCode = clickedrow.find("#tblhd_RMItemId").val();
        }
        else if (flag == 'Cunsum') {
            ItmCode = clickedrow.find("#ConsumeMaterialId").val();
        }
        else if (flag == 'OutPut') {
            ItmCode = clickedrow.find("#OutputMateriallId").val();
        }
        else {

        }

    }
    else {
        if (flag == 'Header') {
            ItmCode = $("#ddl_ItemName").val();
        }
        else if (flag == 'Req') {
            ItmCode = $("#ddlMaterialName").val();
        }
        else {

        }

    }
    ItemInfoBtnClick(ItmCode);
}

var QtyDecDigit = $("#QtyDigit").text();
function OnChange_ProductName() {
    debugger;
    var productid = $("#ddl_ItemName").val();
    $("#hdn_item_id").val(productid);
    var Hedr_ItemName = $("#ddl_ItemName option:selected").text();
    $("#hdn_item_name").val(Hedr_ItemName);

    if (productid == "0" || productid == null) {
        $("#vmddl_ItemName").text($("#valueReq").text());
        $("#vmddl_ItemName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
    }
    else {
        $("#vmddl_ItemName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");

    }
    var ItemID = $("#ddl_ItemName option:selected").val();
    var WarehouseID = $("#ddl_WarehouseName option:selected").val();
    if (productid != "0" && productid != null) {
        
        if ((ItemID != "" && ItemID != "0") && (WarehouseID != "0" && WarehouseID != "")) {
            var src_type = $("#hdnsrc_type").val();
          
                GetAvlStock(ItemID, WarehouseID, src_type, 'Orderdetail')
            
        }
        $("#ddl_WarehouseName").prop("disabled",false);
        debugger;
        Cmn_BindUOM(null, productid, "", "N", "");
    }
    $("#ddlMaterialName").val("0");
    $("#ddlMaterialType").val("0").trigger('change');
}

function ddl_ShopfloorName_onchange(e) {
    debugger;
    var ddl_ShopfloorName = $("#ddl_ShopfloorName").val();
    $("#hdn_ShopfloorID").val(ddl_ShopfloorName);
    if (ddl_ShopfloorName != "0") {
        $("#vm_ddlShopfloorName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ShopfloorName-container']").css("border-color", "#ced4da");
        //$("#ddl_ShopfloorName").css("border-color", "#ced4da")
        $("#ddlMaterialType").attr("disabled", false);
        $("#ddlMaterialName").attr("disabled", false);
        $("#RequiredQuantity").attr("readonly", false);
        $("#divAddNewMaterialRwrkJO").css("display", "");
    }
    else {
        $('#vm_ddlShopfloorName').text($("#valueReq").text());
        $("#vm_ddlShopfloorName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ShopfloorName-container']").css("border-color", "red");
        $("#ddlMaterialType").attr("disabled", true);
        $("#ddlMaterialName").attr("disabled", true);
        $("#RequiredQuantity").attr("readonly", true);
        $("#divAddNewMaterialRwrkJO").css("display", "none");
    }
    bindworkstation(ddl_ShopfloorName);
};
function bindworkstation(ddl_ShopfloorName) {
   
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/PackagingJobOrder/BindWorkStationList",
            data: {
                shfl_id: ddl_ShopfloorName,
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
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
                    }
                }
            },
        });
}
function ddl_WorkstationName_onchange(e) {
    debugger;
    var ddl_Workstation = $("#ddl_WorkstationName").val();
    $("#hdn_WorkstationID").val(ddl_Workstation);
    if (ddl_Workstation != "0") {
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
        $("#txt_SupervisorName").val(txt_SupervisorName)
    }
    else {
        document.getElementById("vm_SupervisorName").innerHTML = $("#valueReq").text();
        $("#txt_SupervisorName").css("border-color", "red");

    }
}
function ddl_Warehouse_onchange() {
    debugger;
    
    var ItemID = $("#ddl_ItemName option:selected").val();
    $("#hdn_item_id").val(ItemID);
    var ddl_Warehouse = $("#ddl_WarehouseName").val();
    var WarehouseID = $("#ddl_WarehouseName option:selected").val();
    var WarehouseName = $("#ddl_WarehouseName option:selected").text();
    var splitWarehouse = WarehouseName.split(' (')
    firstname = splitWarehouse[1].trim();
    var srctype = firstname.split(')');
    var srctpefinal = "";
    if (srctype[0].trim() == "Warehouse") {
        srctpefinal = "W";
        $("#ddl_ShopfloorName").attr("disabled", false);
    }
    if (srctype[0].trim() == "Shopfloor") {
        srctpefinal = "S";
        $("#ddl_ShopfloorName").attr("disabled", true);
        var ddl_ShopfloorName = WarehouseID;
        $("#ddl_ShopfloorName").val(ddl_ShopfloorName).change();
        bindworkstation(ddl_ShopfloorName);
    }    
    $("#hdnsrc_type").val(srctpefinal);
    $("#hdn_WarehouseID").val(WarehouseID);
    $("#hdn_WarehouseName").val(WarehouseName);
    if (ddl_Warehouse != "0" || ddl_Warehouse != "") {
        $("#vm_ddl_WarehouseName").css("display", "none");
        $("[aria-labelledby='select2-ddl_WarehouseName-container']").css("border-color", "#ced4da");
        $("#txtReworkQuantity").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_WarehouseName').text($("#valueReq").text());
        $("#vm_ddl_WarehouseName").css("display", "block");
        $("[aria-labelledby='select2-ddl_WarehouseName-container']").css("border-color", "red");
        $("#AvailableQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
    }
    if ((ItemID != "" && ItemID != "0") && (ddl_Warehouse != "0" && ddl_Warehouse != "")) {
      var src_type=  $("#hdnsrc_type").val();
        GetAvlStock(ItemID, WarehouseID, src_type,'Orderdetail')
    }
    else {
        $('#vm_ddl_WarehouseName').text($("#valueReq").text());
        $("#vm_ddl_WarehouseName").css("display", "block");
        $("[aria-labelledby='select2-ddl_WarehouseName-container']").css("border-color", "red");
        $("#AvailableQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
    }
   
};
function CheckHeaderValidation() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ddl_ProductName = $("#ddl_ItemName").val();
   var flagVali = "N";
    if (ddl_ProductName != "0")
    {
        $("#vmddl_ItemName").css("display", "none");
        var ddl_Warehouse = $("#ddl_WarehouseName").val();
        if (ddl_Warehouse == "0" || ddl_Warehouse == "") {
            $('#vm_ddl_WarehouseName').text($("#valueReq").text());
            $("#vm_ddl_WarehouseName").css("display", "block");
            $("[aria-labelledby='select2-ddl_WarehouseName-container']").css("border-color", "red");
            flagVali = "Y";
        }
        else {
            $("#vm_ddl_WarehouseName").css("display", "none");
            $("[aria-labelledby='select2-ddl_WarehouseName-container']").css("border-color", "#ced4da")
        }
    }
    else
    {
        $('#vmddl_ItemName').text($("#valueReq").text());
        $("#vmddl_ItemName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
        flagVali = "Y";
    }

    debugger;
    var ddl_Warehouse = $("#ddl_WarehouseName").val();
    if (ddl_Warehouse != "0") {
        var AvailableQuantity = $("#AvailableQuantity").val();
        if (AvoidDot(AvailableQuantity) == false) {
            $("#AvailableQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
            AvailableQuantity = 0;
        }
        if ((parseFloat(AvailableQuantity).toFixed(QtyDecDigit)) != parseFloat(0).toFixed(QtyDecDigit) || AvailableQuantity != "") {
            $("#SpanAvailableQuantityErrorMsg").css("display", "none");
            $("#AvailableQuantity").css("border-color", "#ced4da");
        }
        else {
            $('#SpanAvailableQuantityErrorMsg').text($("#Stocknotavailable").text());
            $("#AvailableQuantity").css("border-color", "Red");
            $("#SpanAvailableQuantityErrorMsg").css("display", "block");
            flagVali = "Y";
        }

    }

    if (ddl_ProductName != "0" && ddl_Warehouse != "0" && parseFloat(AvailableQuantity).toFixed(QtyDecDigit) != parseFloat(0).toFixed(QtyDecDigit)) {
        var txtReworkQty = $("#txtReworkQuantity").val();
        if (AvoidDot(txtReworkQty) == false) {
            $("#txtReworkQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
            txtReworkQty = 0;
        }
        if ((parseFloat(txtReworkQty).toFixed(QtyDecDigit)) != parseFloat(0).toFixed(QtyDecDigit) || txtReworkQty != "") {
            $("#SpanReworkQuantityErrorMsg").css("display", "none");
            $("#txtReworkQuantity").css("border-color", "#ced4da");
            $("#ReworkQtyIconBtn").css("border", "#ced4da");
        }
        else {
            swal("", $("#LotBatchDetailsNotFound").text(), "warning");
            $("#ReworkQtyIconBtn").css("border", "1px solid red");
            
            //$('#SpanReworkQuantityErrorMsg').text($("#valueReq").text());
            /*$("#txtReworkQuantity").css("border-color", "Red");*/
            //$("#SpanReworkQuantityErrorMsg").css("display", "block");
            flagVali = "Y";
        }

    }
    else {
        //$('#SpanReworkQuantityErrorMsg').text($("#valueReq").text());
        $("#ReworkQtyIconBtn").css("border", "1px solid red");
        //$("#txtReworkQuantity").css("border-color", "Red");
        $("#SpanReworkQuantityErrorMsg").css("display", "block");
        flagVali = "Y";
    }
    
   var ddl_ShopfloorName = $("#ddl_ShopfloorName").val();
        if (ddl_ShopfloorName != "0") {
            $("#vm_ddlShopfloorName").css("display", "none");
            $("[aria-labelledby='select2-ddl_ShopfloorName-container']").css("border-color", "#ced4da");
        }
        else {
            $('#vm_ddlShopfloorName').text($("#valueReq").text());
            $("#vm_ddlShopfloorName").css("display", "block");
            $("[aria-labelledby='select2-ddl_ShopfloorName-container']").css("border-color", "red");
            flagVali = "Y";
        }
        var ddl_WorkstationName = $("#ddl_WorkstationName").val();
        if (ddl_WorkstationName != "0") {
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
    var NewBatchNo = $("#txt_NewBatchNumber").val();
    if (NewBatchNo == "" || txt_NewBatchNumber == null) {
        $('#vm_newBatchNumber').text($("#valueReq").text());
        $("#vm_newBatchNumber").css("display", "block");
        $("#txt_NewBatchNumber").css("border-color", "red");
        flagVali = "Y";
    }
    else {
        $("#vm_newBatchNumber").css("display", "none");
        $("#txt_NewBatchNumber").css("border-color", "#ced4da");
    }
    debugger;
    var I_Exp = $("#hdn_i_exp").val();
    if (I_Exp == "Y") {
        var NewExpiryDate = $("#NewExpiry_Date").val();
        if (NewExpiryDate == "" || NewExpiryDate == null) {
            $('#SpanNewExpiry_DateErrorMsg').text($("#valueReq").text());
            $("#SpanNewExpiry_DateErrorMsg").css("display", "block");
            $("#NewExpiry_Date").css("border-color", "red");
            flagVali = "Y";
        }
        else {
            $("#SpanNewExpiry_DateErrorMsg").css("display", "none");
            $("#NewExpiry_Date").css("border-color", "#ced4da");
        }
    }
   
    if (flagVali == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnChangeExpDate(ExpDate) {
    //debugger;
    var Exp_Date = ExpDate.value;
    var RwkJODate = $("#JobCardDate").val();
    try {
        if (RwkJODate > Exp_Date) {
            $("#NewExpiry_Date").val("");
            $('#SpanNewExpiry_DateErrorMsg').text($("#VDateCNBGTPODate").text());
            $("#SpanNewExpiry_DateErrorMsg").css("display", "block");
            return false;
        }
        else {
            $("#SpanNewExpiry_DateErrorMsg").css("display", "none");
            $("#NewExpiry_Date").css("border-color", "#ced4da");
        }

    } catch (err) {

    }
}
function GetAvlStock(ItemID, WarehouseID, src_type,accodian_type) {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/PackagingJobOrder/BindPkgWHAvalStk",
            data: {
                ItemID: ItemID,
                WarehouseID: WarehouseID,
                src_type: src_type
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            var AvailableQty = arr.Table[i].wh_avl_stk_bs;
                            if (accodian_type == "Orderdetail") {
                                if (AvailableQty > 0) {
                                    $("#AvailableQuantity").val(parseFloat(AvailableQty).toFixed(QtyDecDigit));
                                    $("#SpanAvailableQuantityErrorMsg").css("display", "none");
                                    $("#AvailableQuantity").css("border-color", "#ced4da");
                                    var hdn_item_ibatch = $("#hdn_i_batch").val();
                                    var hdn_item_iserial = $("#hdn_i_serial").val();
                                    //$("#ddlMaterialType").prop("disabled", false);
                                    //$("#ddlMaterialName").prop("disabled", false);
                                    //$("#RequiredQuantity").prop("readonly", false);
                                   // $("#divAddNewMaterialRwrkJO").css("display", "block");

                                }
                                else {
                                    $("#AvailableQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                                    //$("#ddlMaterialType").prop("disabled", true);
                                    //$("#ddlMaterialName").prop("disabled", true);
                                    //$("#RequiredQuantity").prop("readonly", true);
                                  //  $("#divAddNewMaterialRwrkJO").css("display", "none");

                                }
                            }
                            else if (accodian_type == "reqmatrial") {
                                $("#IDShopFloorAvl_Qty").val(parseFloat(AvailableQty).toFixed(QtyDecDigit));                              
                            }

                        }
                    }
                    else {
                        if (accodian_type == "reqmatrial") {
                            $("#IDShopFloorAvl_Qty").val(parseFloat(0).toFixed(QtyDecDigit));
                        }
                        else {
                            $("#AvailableQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                        }
                    }
                    debugger;
                    //if (arr.Table1.length > 0) {
                    //    for (var k = 0; k <= arr.Table1.length; k++) {

                    //        $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                    //        <td><input type="text" id="ItemId" value='${arr.Table1[k].Item_id}'></td>
                    //        <td><input type="text" id="subItemId" value='${arr.Table1[k].sub_item_id}'></td>
                    //        <td><input type="text" id="subItemQty" value='${arr.Table1[k].Qty}'></td>
                    //    </tr>`);
                    //    }
                    //}
                }
            },
        });
}

/*--------------------Required Material Detail Section Start----------------------------------*/
function ddlMaterialType_onchange(e) {
    debugger;
    var ddl_MaterialTyp = $("#ddlMaterialType option:selected").val();
    var ddl_HedrItemId = $("#ddl_ItemName option:selected").val();
   
    if (ddl_MaterialTyp != "0" || ddl_MaterialTyp != "" || ddl_MaterialTyp != null) {
        $("#spanMaterialType").css("display", "none");
        $("#ddlMaterialType").css("border-color", "#ced4da")
        $("#vmddl_MaterialName").css("display", "none");
        $("#ddlMaterialName").css("border-color", "#ced4da");
    }
    else {
        $('#spanMaterialType').text($("#valueReq").text());
        $("#spanMaterialType").css("display", "block");
        $("#ddlMaterialType").css("border-color", "red")
    }
    $("#MaterialUOM").val("");
    $("#RequiredQuantity").val("");
    $("#IDShopFloorAvl_Qty").val("");
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/PackagingJobOrder/BindMaterialName",
    //        data: {
    //            ddl_MaterialTyp: ddl_MaterialTyp,
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                $('#ddlMaterialName').empty();
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
                       
    //                    $("#ddlMaterialName optgroup option").remove();
    //                    $("#ddlMaterialName optgroup").remove();
                        
    //                    $("#ddlMaterialName").append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    $('#Textddl').append(`<option data-uom="0" value="0">---Select---</option>`);
                       
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $("#ddlMaterialName option[value=" + ddl_HedrItemId + "]").select2().hide();
    //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_alias}" value="${arr.Table[i].item_id}">${arr.Table[i].item_name}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $("#ddlMaterialName").select2({
    //                            templateResult: function (data) {
    //                            var selected = $("#ddlMaterialName").val();
    //                            if (checkRM(data, selected, "#TblReqMaterialDetail", '#tblhd_RMItemId') == true) {
    //                                    var UOM = $(data.element).data('uom');
    //                                    var classAttr = $(data.element).attr('class');
    //                                    var hasClass = typeof classAttr != 'undefined';
    //                                    classAttr = hasClass ? ' ' + classAttr : '';
    //                                    var $result = $(
    //                                        '<div class="row">' +
    //                                        '<div class="col-md-8 col-xs-8' + classAttr + '">' + data.text + '</div>' +
    //                                        '<div class="col-md-4 col-xs-4' + classAttr + '">' + UOM + '</div>' +
    //                                        '</div>'
    //                                    );
    //                                    return $result;
    //                                }
    //                                firstEmptySelect = false;
    //                            }
    //                    });
    //                }
    //                else {
                       
    //                }
                   
    //            }
    //        },
    //    });
    $("#ddlMaterialName").val("0");
    BindItemNameDetail();
};
function BindItemNameDetail() {
    debugger;
    var ddl_MaterialTyp = $("#ddlMaterialType option:selected").val();
    var ddl_HedrItemId = $("#hdn_item_id").val();
    $("#ddlMaterialName").select2({

        ajax: {
            url: $("#ItemListName").val(),
            data: function (params) {
                var queryParameters = {
                    ItemName1: params.term,// search term like "a" then "an"
                    //Group: params.page
                    ddl_MaterialTyp: ddl_MaterialTyp,
                    ddl_HedrItemId: ddl_HedrItemId
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 20;//50000; // or whatever pagesize
                //results = [];



                ConvertIntoArreyList_RWK('#TblReqMaterialDetail', "#tblhd_RMItemId");
                var ItemListArrey = [];
                if (sessionStorage.getItem("selecteditemlist").length != null) {
                    ItemListArrey = JSON.parse(sessionStorage.getItem("selecteditemlist"));
                }
                let selected = [];
                selected.push({ id: $("#ItemListName").val() });
                selected = JSON.stringify(selected);

                var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));
                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));


                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
             <div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
                  <div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div></div>
                   </strong></li></ul>`)
                }
                var page = params.page || 1;
                // data = data.slice((page - 1) * pageSize, page * pageSize); /*commented by Suraj For creating scroll*/
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0_0", Name: " ---Select---" };
                            Fdata.unshift(select);
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

            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },

    });
}
function ConvertIntoArreyList_RWK(TableID, ItmDDLName) {
    let array = [];
    $(TableID + " tbody tr").each(function () {
        var currentRow = $(this);
        //debugger;
        // var rowno = currentRow.find(SnoHiddenField).val();
        var itemId = "";
        //if (OtherFx == "listToHide") {
        //    itemId = currentRow.find(SnoHiddenField).val();
        //}
        //else {
        itemId = currentRow.find(ItmDDLName).val();
        //}
        if (itemId != "0" /*&& itemId != $(ItmDDLName + RowID).val()*/) {
            array.push({ id: itemId });
        }
    });

    sessionStorage.removeItem("selecteditemlist");
    sessionStorage.setItem("selecteditemlist", JSON.stringify(array));
    //return array;
}
function ddlMaterialName_onchange() {
    debugger;
    var MaterialID = $("#ddlMaterialName").val();
    //var productid = $("#ddlMaterialName option:selected").val();
    /*var hdn_product_id = $("#hdn_item_id").val();*/
    $("#hdn_MaterialID").val(MaterialID);
    if (MaterialID == "0" || MaterialID == null) {
        $("#vmddl_MaterialName").text($("#valueReq").text());
        $("#vmddl_MaterialName").css("display", "block");
        $("[aria-labelledby='select2-ddlMaterialName-container']").css("border-color", "red");
    }
    else {
        $("#vmddl_MaterialName").css("display", "none");
        $("[aria-labelledby='select2-ddlMaterialName-container']").css("border-color", "#ced4da");
    }
    if (MaterialID != "0" && MaterialID != null) {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/PackagingJobOrder/GetItemUOM",
                data: {
                    MaterialID: MaterialID,
                    
                },
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            for (var i = 0; i < arr.Table.length; i++) {
                                var M_UOM = arr.Table[i].uom_alias;
                                var M_UOMID = arr.Table[i].uom_id;
                                var M_SubItem = arr.Table[i].sub_item;
                                $("#MaterialUOM").val(M_UOM); 
                                $("#MaterialUOMID").val(M_UOMID);
                                $("#sub_itemReqMatrial").val(M_SubItem);
                                //$("#SpanAvailableQuantityErrorMsg").css("display", "none");
                                //$("#AvailableQuantity").css("border-color", "#ced4da");
                            }
                        }
                        else {
                            $("#MaterialUOM").val('');
                            $("#MaterialUOMID").val('');
                        }
                    }
                },
            });
        var src_type = $("#hdnsrc_type").val();
        var shfl_and_warehouse_ID = "";
        if (src_type=="S") {
            shfl_and_warehouse_ID=  $("#ddl_WarehouseName").val();
        }
        else if (src_type == "W") {
            shfl_and_warehouse_ID = $("#ddl_ShopfloorName").val();
        }
        GetAvlStock(MaterialID, shfl_and_warehouse_ID, 'S','reqmatrial');
    }
}
function AmountFloatQty(el, evt) {
    //debugger
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
}
function NewBtachNo(el, evt) {
    debugger
    $("#vm_newBatchNumber").css("display", "none");
    $("#txt_NewBatchNumber").css("border-color", "#ced4da");
}
function OnchangeRequiredQty() {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    ReqQty = $("#RequiredQuantity").val();
 
   RequiredQty = parseFloat($("#RequiredQuantity").val()).toFixed(QtyDecDigit);
    if (AvoidDot(RequiredQty) == false) {
        $("#DispatchQuantity").val("");
        RequiredQty = 0;
    }

    if (ReqQty == "" || ReqQty == "0" || ReqQty == parseFloat(0)) {
        $("#RequiredQuantity").val("");
        $("#SpanRequiredQuantityErrorMsg").text($("#valueReq").text());
        $("#SpanRequiredQuantityErrorMsg").css("display", "block");
        $("#RequiredQuantity").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        parseFloat($("#RequiredQuantity").val(RequiredQty)).toFixed(QtyDecDigit);
        $("#SpanRequiredQuantityErrorMsg").css("display", "none");
        $("#RequiredQuantity").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }

}
function CheckReqMaterialValidation() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var flag = "N";
    var MtrlTyp = $("#ddlMaterialType").val();
    if (MtrlTyp != "0") {
        $("#spanMaterialType").css("display", "none");
        $("#ddlMaterialType").css("border-color", "#ced4da");
    }
    else {
        $('#spanMaterialType').text($("#valueReq").text());
        $("#spanMaterialType").css("display", "block");
        $("#ddlMaterialType").css("border-color", "red");
        flag = "Y";
    }
    var ddl_RMName = $("#ddlMaterialName").val();
   if (ddl_RMName != "0") {
        $("#vmddl_MaterialName").css("display", "none");
       $("#ddlMaterialName").css("border-color", "#ced4da");
       //$("[aria-labelledby='select2-ddlMaterialName-container']").css("border-color", "#ced4da");
    }
    else {
        $('#vmddl_MaterialName').text($("#valueReq").text());
        $("#vmddl_MaterialName").css("display", "block");
       $("#ddlMaterialName").css("border-color", "red");
       /*$("[aria-labelledby='select2-ddlMaterialName-container']").css("border-color", "red");*/
        flag = "Y";
    }
    //if (MtrlTyp !="SR") {
    //    var avl_qty = $("#IDShopFloorAvl_Qty").val();
    //    if (avl_qty == "0" || parseFloat(avl_qty) == 0 || parseFloat(ReqQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
    //        $('#SpanShopFloorAvl_QtyErrorMsg').text($("#valueReq").text());
    //        $("#IDShopFloorAvl_Qty").css("border-color", "Red");
    //        $("#SpanShopFloorAvl_QtyErrorMsg").css("display", "block");
    //        flag = "Y";
    //    }
    //    else {
    //        $("#SpanShopFloorAvl_QtyErrorMsg").css("display", "none");
    //        $("#IDShopFloorAvl_Qty").css("border-color", "#ced4da");
    //    }
    //}
    var ReqQuantity = $("#RequiredQuantity").val();
   
    if (AvoidDot(ReqQuantity) == false) {
        $("#RequiredQuantity").val("");
        ReqQuantity = "";
    }
    if ((parseFloat(ReqQuantity).toFixed(QtyDecDigit)) == "0.000" || ReqQuantity == "") {
        $('#SpanRequiredQuantityErrorMsg').text($("#valueReq").text());
        $("#RequiredQuantity").css("border-color", "Red");
        $("#SpanRequiredQuantityErrorMsg").css("display", "block");
        flag = "Y";
    }
    else {
        //if (MtrlTyp != "SR") {
        //    if (avl_qty < ReqQuantity) {
        //        $('#SpanRequiredQuantityErrorMsg').text($("#ExceedingQty").text());
        //        $("#RequiredQuantity").css("border-color", "Red");
        //        $("#SpanRequiredQuantityErrorMsg").css("display", "block");
        //        flag = "Y";
        //    }
        //    else {
        //        $("#SpanRequiredQuantityErrorMsg").css("display", "none");
        //        $("#RequiredQuantity").css("border-color", "#ced4da");
        //    }
        //}
        //else {
            $("#SpanRequiredQuantityErrorMsg").css("display", "none");
            $("#RequiredQuantity").css("border-color", "#ced4da");
       // }
    }
    
    if (flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function PlusBtn_Add_RM_MaterialDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RMflag = 'N';
    var RM_MaterialTyp = $("#ddlMaterialType option:selected").text();
    var RM_MaterialTypID = $("#ddlMaterialType").val();
    var RM_ddlItem_id = $("#ddlMaterialName").val();
    var RM_ddlItemName = $("#ddlMaterialName option:selected").text();
    var RM_UOMName = $("#MaterialUOM").val();
    var RM_UOMID = $("#MaterialUOMID").val();
    var RM_ReqQty = $("#RequiredQuantity").val();
    var avl_qty_matrial = $("#IDShopFloorAvl_Qty").val();
    var avl_stkvalue = "";
    if (avl_qty_matrial == "" || avl_qty_matrial == parseFloat(0).toFixed(QtyDecDigit)) {
        avl_stkvalue = "";
    }
    else {
        avl_stkvalue = parseFloat(avl_qty_matrial).toFixed(QtyDecDigit)
    }
    var span_SubItemDetail = $("#span_SubItemDetail").text();
    var RM_subitem = $("#sub_itemReqMatrial").val();
    var RM_subitmDisable = "";
    if (RM_subitem != "Y") {
        RM_subitmDisable = "disabled";
    }
    var ValidationFlag = CheckReqMaterialValidation();
    if (ValidationFlag == false) {
        return false;
    }
    
    if (ValidationFlag == true) {
        var RowId = 0;
        var rowCount = $('#TblReqMaterialDetail >tbody >tr').length + 1;
        if (rowCount > 0) {
            $("#ddl_ItemName").prop("disabled", true);
          $("#ddl_WarehouseName").prop("disabled", true);
        }
        debugger;
        var RowNo = 0;
        var levels = [];
        $("#TblReqMaterialDetail >tbody >tr").each(function (i, row) {
            //debugger;
            var currentRow = $(this);
            RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
            levels.push(RowNo);
        });
        if (levels.length > 0) {
            RowNo = Math.max(...levels);
        }

        RowNo = RowNo + 1;
        if (RowNo == "0") {
            RowNo = 1;
        }
        $('#TblReqMaterialDetail tbody').append(`

            <tr id="${++RowId}">
        
            <td class=" red center"><i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" onclick="deleteRow(this, event,'${RM_ddlItem_id}')" title="${$("#Span_Delete_Title").text()}"></i>
            </td>
            <td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" onclick="editRow(this, event,'${RM_ddlItem_id}')" title="${$("#Edit").text()}"></i>
            </td>
            <td id="SRNO" class="sr_padding">
            <span id="SpanRowId"  value="${rowCount}">${rowCount}</span>
            <input  type="hidden" id="SNohiddenfiled" value="${RowNo}"/>
            </td>
            
            <td>
                <input id="tblMaterialType" value="${RM_MaterialTyp}" class="form-control" type="text" name="MaterialType" placeholder="${$("#span_MaterialType").text()}" readonly>
                <input type="hidden" id="tblhd_RMMtrlTyp" value="${RM_MaterialTypID}" style="display: none;" />
            </td>
            <td>
              <div class="col-md-11 col-sm-12 no-padding">
                <input id="tblMaterialName" value="${RM_ddlItemName}" class="form-control" autocomplete="off" type="text" name="MaterialName" placeholder="${$("#ItemName").text()}"  onblur="this.placeholder='Item Name'" readonly>
                <input type="hidden" id="tblhd_RMItemId" value="${RM_ddlItem_id}" style="display: none;" />
              </div>
             <div class="col-sm-1 i_Icon ">
              <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'Reqtbl');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title=" @Resource.ItemInformation"> </button>
             </div>
            </td>
            <td>
                <input  id="tblRM_UOM" value="${RM_UOMName}" class="form-control" autocomplete="off" type="text" name="RM_UOM" placeholder="${$("#ItemUOM").text()}" readonly>
                <input type="hidden" id="tblhd_RMUOMId" value="${RM_UOMID}" style="display: none;" />
            </td>
          <td>
            <input id="tdavl_qty" value='${avl_stkvalue}' class="form-control" autocomplete="off"  name="Avl_qty" placeholder="0000.00" readonly>
            </td>
            <td>
                <div class="col-sm-10 lpo_form no-padding">
                    <input id="tblRM_ReqQuantity"  value="${parseFloat(RM_ReqQty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="RequiredQuantity" placeholder="0000.00"  readonly>
                </div>
                <div class="col-sm-2 i_Icon" id="div_SubItemRMReqQty" style="padding:0px; ">
                  <input hidden type="text" id="sub_item" value="${RM_subitem}" />
                  <button type="button" id="SubItemRMReqQty" ${RM_subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('RM_RequireQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}">
                  </button>
                </div>
             </td>

            </tr>

        `);
        
        if (RMflag == 'N') {
            $("#ddlMaterialName").append(`<option value="0" selected>---Select---</option>`);
        AddRemoveRMItemName(RM_ddlItem_id)
        

    }
    }
    
}
function AddRemoveRMItemName(RM_ddlItem_id) {
    debugger;
    var flag = 'N';
    var RMddlItem_id = $("#ddlMaterialName").val();
    //$("#ddl_ItemName option").removeClass("select2-hidden-accessible");
    //$('#ddl_ItemName').val("0").change();

    $("#TblReqMaterialDetail >tbody >tr").each(function (i, rows) {
        var currentRowChild = $(this);
        //Hide item name, if exist in table
        debugger;
        var item_id = currentRowChild.find("#tblhd_RMItemId").val();//recheck
        //$("#ddlMaterialName option[value=" + item_id + "]").select2().hide();
        $('#ddlMaterialName').val("0").change();
        //$('#ddlMaterialType').val("0");
        $("#ddlMaterialType").val("0").trigger('change');
        $("#RequiredQuantity").val('');
        $("#IDShopFloorAvl_Qty").val('');
        var Req_Quantity = $("#RequiredQuantity").val();
      
        if (Req_Quantity == "") {
            Req_Quantity = 0.000;
        }
      
        $('#MaterialUOM').val("");
    });

}
function checkRM(data, selected, TableNameID, ItemDDlNameID) {
    debugger;
    var mtype = $("#ddlMaterialType").val();
    var ddl_HedrItemId = $("#ddl_ItemName option:selected").val();
    if (mtype != '0') {
        var Flag = "N";
        if (data.id != null) {
            if ($(TableNameID + " tbody tr").length > 0) {
                /*$("#ddlMaterialName option[value=" + ddl_HedrItemId + "]").select2().hide();*/
                var checkItemlen = $(TableNameID + " tbody tr " + ItemDDlNameID + "[value='" + data.id + "']").length;
                if (checkItemlen > 0) {
                    Flag = "Y";
                }
            }

        }
        if (Flag == "Y") {
            return false;
        }
        else {
            return true;
        }
    }
    
}
function editRow(el, e, option) {
    debugger;
    $("#ddl_ItemName").removeAttr('onchange');
    var QtyDecDigit = $("#QtyDigit").text();
    var rowJavascript = el.parentNode.parentNode;
    var clickedrow = $(e.target).closest("tr");
    $('#hdnUpdateInTableRM').val(rowJavascript.rowIndex);

    var Materialtyp = clickedrow.find("#tblMaterialType").val();
    var tblhd_RMMtrlTyp = clickedrow.find("#tblhd_RMMtrlTyp").val();
 
    //$("#ddlMaterialType option:selected").text(Materialtyp);
    $("#ddlMaterialType").val(tblhd_RMMtrlTyp);
  //  var MaterialtypID = clickedrow.find("#tblhd_RMMtrlTyp").val();
    $("#hdn_MaterialTypID").val(tblhd_RMMtrlTyp);

    var avl_stock = clickedrow.find("#tdavl_qty").val();
    $("#IDShopFloorAvl_Qty").val(avl_stock);
    var item_id = clickedrow.find("#tblhd_RMItemId").val();
    $("#hdn_MaterialID").val(item_id);
    //var item_Name = clickedrow.find("#tblMaterialName").val();

    var UOM = clickedrow.find("#tblRM_UOM").val();
    $("#MaterialUOM").val(UOM);
    var uom_id = clickedrow.find("#tblhd_RMUOMId").val();
    $("#MaterialUOMID").val(uom_id);

    var ReqQTY = clickedrow.find("#tblRM_ReqQuantity").val();
    $("#RequiredQuantity").val(ReqQTY);

    var td_ItemName = clickedrow.find("#tblMaterialName").val();
    if (td_ItemName != null) {
        td_ItemName = td_ItemName.trim();
    }
    //$('#ddlMaterialType').append('<option value=' + MaterialtypID + ' selected>' + Materialtyp + '</option>');
    $('#ddlMaterialName').append('<option value=' + item_id + ' selected>' + td_ItemName + '</option>');
    $("#MaterilAddBtn").hide();
    $("#divUpdateMRwrkJO").show();
    $("#ddlMaterialType").prop("disabled", true)
    $("#ddlMaterialName").prop("disabled", true)
    removeValueRequired_MsgValidate();

};
function removeValueRequired_MsgValidate()
{
    $("#spanMaterialType").css("display", "none");
    $("#ddlMaterialType").css("border-color", "#ced4da");
    $("#vmddl_MaterialName").css("display", "none");
    $("[aria-labelledby='select2-ddlMaterialName-container']").css("border-color", "#ced4da");
    $("#SpanRequiredQuantityErrorMsg").css("display", "none");
    $("#RequiredQuantity").css("border-color", "#ced4da");
}
function OnClickReqMaterialUpdateBtn() {
    debugger;
    var flag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    //var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var ReqQuantity = $("#RequiredQuantity").val();
    if (AvoidDot(ReqQuantity) == false) {
        $("#RequiredQuantity").val("0.000");
        ReqQuantity = 0;
    }
    if ((parseFloat(ReqQuantity).toFixed(QtyDecDigit)) != "0.000" || ReqQuantity != "") {
        $("#SpanRequiredQuantityErrorMsg").css("display", "none");
        $("#RequiredQuantity").css("border-color", "#ced4da");
    }
    else {
        $('#SpanRequiredQuantityErrorMsg').text($("#valueReq").text());
        $("#RequiredQuantity").css("border-color", "Red");
        $("#SpanRequiredQuantityErrorMsg").css("display", "block");
        flag = "Y";
    }
    if (flag == "Y") {
        return false;
    }
    debugger;
    var ddl_RMItem_id = $("#ddlMaterialName").val();
    var hdn_RMItem_id = $("#hdn_MaterialID").val();
    if (flag == 'N') {
        var tableRow = $('#hdnUpdateInTableRM').val();
        debugger;
        var txtReqQuantity = $("#RequiredQuantity").val();
        $('#TblReqMaterialDetail').find("tr:eq(" + tableRow + ")").find("#tblRM_ReqQuantity").val(txtReqQuantity);
        $('#TblReqMaterialDetail').find("tr:eq(" + tableRow + ")").find("#tblRM_ReqQuantity").html(`<div class='num_right'>${txtReqQuantity}</div>`);
        //$("#ddlMaterialType").prop('disabled', false);
        //$("#ddlMaterialType").prop('onchange', 'ddlMaterialName_onchange()');
        $("#MaterilAddBtn").show();
        $("#divUpdateMRwrkJO").hide();
        resetReqMaterialHeaderdetail();
        AddRemoveRMItemName(hdn_RMItem_id);
    }
};
function resetReqMaterialHeaderdetail() {
    var QtyDecDigit = $("#QtyDigit").text();
    $("#divAddNewMaterialRwrkJO").show();
    $("#divUpdateMRwrkJO").hide();
    $("#ddlMaterialType").prop("disabled", false)
  //  $("#ddlMaterialType option:selected").text('---Select---');
    $("#ddlMaterialType").val('0');
    $("#ddlMaterialName").prop("disabled", false)
   $("#MaterialUOM").val('');
    $("#RequiredQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
};
function deleteRow(i, e, option) {
    debugger;
    //var rowlen = $('#TblReqMaterialDetail >tbody >tr').length;
    //if (rowlen == 0) {

    //}
    debugger;
    DeleteRMSubItemQtyDetail(option, "SubRM")
    $(i).closest('tr').remove();
    SerialNoAfterDelete();
    resetReqMaterialHeaderdetail();
    $("#ddlMaterialName option").remove();
    $("#ddlMaterialName").append(`<option value="0" selected>---Select---</option>`);
    removeValueRequired_MsgValidate();
    debugger;
    $("#divAddNewMaterialRwrkJO").css("display", "block");
    $("#MaterilAddBtn").show();
};
function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    $("#TblReqMaterialDetail >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);
    });
};
/*-------------------Partial ReworkQuantity Detail Section Start------------------------*/

function OnKeyPressPRDReworkQty(el, evt) {
    var clickedrow = $(evt.target).closest("tr");

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }

    clickedrow.find("#Rework_Qty_Error").css("display", "none");
    clickedrow.find("#PRqtyD_ReworkQuantity ").css("border-color", "#ced4da");
    return true;
}
function OnChangePRDReworkQty(evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var clickedrow = $(evt.target).closest("tr");
    var PRD_ReworkQty = clickedrow.find("#PRqtyD_ReworkQuantity").val();
    /*Add by Hina on 02-03-2024 to show blank instead of 0 in inserted field*/
    if ((PRD_ReworkQty == "") || (PRD_ReworkQty == null) || (PRD_ReworkQty == "0") || (PRD_ReworkQty == parseFloat(0))) {
       clickedrow.find("#PRqtyD_ReworkQuantity").val("");
    }
    /*Code End*/
    var AvailStkQty = clickedrow.find("#ReworkAvailStk_Qty").val();
    if (PRD_ReworkQty > 0) {
        if (parseFloat(PRD_ReworkQty) > parseFloat(AvailStkQty)) {
            debugger;
            clickedrow.find("#Rework_Qty_Error").text($("#ExceedingQty").text());
            clickedrow.find("#Rework_Qty_Error").css("display", "block");
            clickedrow.find("#PRqtyD_ReworkQuantity").css("border-color", "red");
        }
        else {
            clickedrow.find("#Rework_Qty_Error").css("display", "none");
            clickedrow.find("#PRqtyD_ReworkQuantity").css("border-color", "#ced4da");
            var test = parseFloat(PRD_ReworkQty).toFixed(QtyDecDigit);
            clickedrow.find("#PRqtyD_ReworkQuantity").val(test);
        }
    }
    TotalReworkQtyDetail();
}
function TotalReworkQtyDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalReworkQty = parseFloat(0).toFixed(QtyDecDigit);
    $("#ReworkQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var ReworkQty = CurrentRow.find("#PRqtyD_ReworkQuantity").val();
        if (ReworkQty != null && ReworkQty != "") {
            TotalReworkQty = parseFloat(TotalReworkQty) + parseFloat(ReworkQty);
        }
    });
    $("#LblTotalReworkQty").text(parseFloat(TotalReworkQty).toFixed(QtyDecDigit));
}
function onclickbtnReworkQtyReset() {
    debugger;
    $("#ReworkQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        debugger;
        CurrentRow.find("#PRqtyD_ReworkQuantity").val("");

    });

    $("#LblTotalReworkQty").text("");
}
function onclickbtnReworkQtySaveAndExit () {
    debugger
    var HdItemId = $("#hd_RewrkQtyItemId").val();
    var ItemTotalReworkableQuantity = $("#LblTotalReworkQty").text();
    var FlagMsg = "N";
    $("#ReworkQtyInfoTbl TBODY TR").each(function () {
        var row = $(this);
        //var AvailableQuantity = row.find("#AvailableQuantity").val();
        var TotalAvailableQuantity = row.find("#ReworkAvailStk_Qty").val();
        var RewrkQuantity = row.find("#PRqtyD_ReworkQuantity").val();
        if (parseFloat(RewrkQuantity) > parseFloat(TotalAvailableQuantity)) {
            FlagMsg = "Y";
            row.find("#Rework_Qty_Error").text($("#ExceedingQty").text());
            row.find("#Rework_Qty_Error").css("display", "block");
            row.find("#PRqtyD_ReworkQuantity").css("border-color", "red");
            return false;
        }
    });
    
if (FlagMsg === "N") {
        debugger;
    var SelectedItem = $("#hd_RewrkQtyItemId").val();
    $("#SaveItemReworkQtyDetails TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#PR_RewrkQtyItemId").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });

    $("#ReworkQtyInfoTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
        var ItemReworkQuantity = row.find("#PRqtyD_ReworkQuantity").val();
        if (ItemReworkQuantity != "" && ItemReworkQuantity != null) {

                    var ItemUOMID = $("#hd_RewrkQtyUOMId").val();
                    var ItemId = $("#hd_RewrkQtyItemId").val();
                    var WHId = row.find("#hdn_Rework_WhIdtbl").val();
                    var ItemBatchNo = row.find("#ReworkQtyBatchNo").val();
                    var ItemBatchAvlstock = row.find("#ReworkAvailStk_Qty").val();
                    var ItemExpiryDate = row.find("#ReworkExpryDate").val();
                    var LotNo = row.find("#ReworkQtyLotNo").val();
                    var SerialNo = row.find("#ReworkQtySerialNo").val();
                    $('#SaveItemReworkQtyDetails tbody').append(`<tr>
                    
                    <td><input type="text" id="PR_RewrkQtyItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="PR_RewrkQtyUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="PR_RewrkQtyWhId" value="${WHId}" />
                    <td><input type="text" id="PR_RewrkQtyLot" value="${LotNo}" /></td>
                    <td><input type="text" id="PR_RewrkQtyBatch" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="PR_RewrkQtySerial" value="${SerialNo}" /></td>
                    <td><input type="text" id="PR_RewrkQtyExpryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="PR_RewrkAvailQty" value="${ItemBatchAvlstock}" /></td>
                    <td><input type="text" id="PR_ReworkQty" value="${ItemReworkQuantity}" /></td>
                    
                </tr>`
                    );
                }
            });
    $("#ReworkQuantityDEtail").modal('hide');
    $("#txtReworkQuantity").val(ItemTotalReworkableQuantity);
    $("#SpanReworkQuantityErrorMsg").css("display", "none");
    $("#txtReworkQuantity").css("border-color", "#ced4da");
    $("#ReworkQtyIconBtn").css("border", "#ced4da");
            
       
    }
}
function CheckReworkQtyValidation() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var status = "N";
    $("#ReworkQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var ReworkQty = CurrentRow.find("#PRqtyD_ReworkQuantity").val();
        var AvailStkQty = CurrentRow.find("#ReworkAvailStk_Qty").val();

        if (ReworkQty != "" && ReworkQty != null) {
            if (parseFloat(ReworkQty) > parseFloat(AvailStkQty)) {
                debugger;
                CurrentRow.find("#Rework_Qty_Error").text($("#ExceedingQty").text());
                CurrentRow.find("#Rework_Qty_Error").css("display", "block");
                CurrentRow.find("#PRqtyD_ReworkQuantity").css("border-color", "red");
                status = "Y";
            }
            else
            {
                CurrentRow.find("#Rework_Qty_Error").css("display", "none");
                CurrentRow.find("#PRqtyD_ReworkQuantity").css("border-color", "#ced4da");
                var test = parseFloat(ReworkQty).toFixed(QtyDecDigit);
                CurrentRow.find("#PRqtyD_ReworkQuantity").val(test);
            }
        }
        else {
            CurrentRow.find("#Rework_Qty_Error").text($("#valueReq").text());
            CurrentRow.find("#Rework_Qty_Error").css("display", "block");
            CurrentRow.find("#PRqtyD_ReworkQuantity").css("border-color", "red");
            status = "Y";
        }
    });
    if (status === "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnClickItempkgQtyIconBtn(el, evt) {
    try {
        debugger;
        var QtyDecDigit = $("#QtyDigit").text();///Quantity
        PLBindItemReworkQtyDetail();
        
        var ItmCode = $("#hdn_item_id").val();
        var ItmName = $("#hdn_item_name").val();
        var WHID = $("#hdn_WarehouseID").val();
        var WhName = $("#hdn_WarehouseName").val();
        var UOM = $("#UOM").val();
        var UOMid = $("#UOMID").val();
        
        var SelectedItemdetail = $("#hdReworkQtyDetails").val();
        debugger;

        var RewrkJO_Status = $("#hdRJO_Status").val().trim();
        $("#hdRJO_Status").val(RewrkJO_Status);
        var hdnTranstyp = $("#hdtranstype").val();
        var hdnCommand = $("#hdn_command").val();
        var src_type = $("#hdnsrc_type").val();
       
        

        if (RewrkJO_Status == "" || RewrkJO_Status == null || RewrkJO_Status == "D") {
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/PackagingJobOrder/getItemPkgQuantityDetail",
                data: {
                    ItemID: ItmCode, WHID: WHID, Status: RewrkJO_Status,
                    SelectedItemdetail: SelectedItemdetail, hdnTranstyp: hdnTranstyp,
                    hdnCommand: hdnCommand, src_type: src_type
                },
                success: function (data) {
                    debugger;
                    $('#ItemReworkQuantityDetail').html(data);

                    $("#ReworkQtyItemName").val(ItmName);
                    $("#hd_RewrkQtyItemId").val(ItmCode);
                    $("#ReworkQtyUOM").val(UOM);
                    $("#hd_RewrkQtyUOMId").val(UOMid);
                    //TotalReworkQtyDetail();


                    var TotalRewrkQty = parseFloat(0).toFixed(QtyDecDigit);
                    debugger;
                    if ($("#SaveItemReworkQtyDetails TBODY TR").length > 0) {
                        debugger;
                        $("#SaveItemReworkQtyDetails TBODY TR").each(function () {
                            var row = $(this)
                            var tblItemId = row.find("#PR_RewrkQtyItemId").val();
                            var tblWhId = row.find("#PR_RewrkQtyWhId").val();
                            if (tblItemId === ItmCode && tblWhId === WHID) {
                                TotalRewrkQty = parseFloat(TotalRewrkQty) + parseFloat(row.find("#PR_ReworkQty").val());
                            }
                        });
                    }

                    $("#ReworkQtyInfoTbl TBODY TR").each(function () {
                        var row = $(this)
                        var rewrkQty = row.find("#PRqtyD_ReworkQuantity").val();
                        if (rewrkQty != null && rewrkQty != "") {
                            row.find("#PRqtyD_ReworkQuantity").val(parseFloat(rewrkQty).toFixed(QtyDecDigit))
                        }
                    });
                    $("#LblTotalReworkQty").text(parseFloat(TotalRewrkQty).toFixed(QtyDecDigit));
                },
            });
        }
        //else if (PL_Status != "D") {
        else {
            //var Type = "D";
            var PJO_No = $("#JobCardNumber").val();
            var PJO_Date = $("#JobCardDate").val();
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/PackagingJobOrder/getItemPkgQuantityDetailAfterInsert",
                data: {
                    PJO_No: PJO_No,
                    PJO_Date: PJO_Date,
                    Status: RewrkJO_Status,
                    ItemId: ItmCode,
                    WHID: WHID,
                    TransType: hdnTranstyp,
                    Command: hdnCommand,
                    src_type: src_type
                },
                success: function (data) {
                    debugger;
                    $('#ItemReworkQuantityDetail').html(data);
                    $("#ReworkQtyItemName").val(ItmName);
                    $("#hd_RewrkQtyItemId").val(ItmCode);
                    $("#ReworkQtyUOM").val(UOM);
                    $("#hd_RewrkQtyUOMId").val(UOMid);
                    $("#HDWhIDRewrkQtyWise").val(WHID);
                    $("#HDItemNameRewrkQtyWise").val(ItmCode);
                    /*$("#HDUOMBatchWise").val(UOMId);*/

                    var TotalRewrkQty = parseFloat(0).toFixed(QtyDecDigit);
                    debugger;
                    if ($("#SaveItemReworkQtyDetails TBODY TR").length > 0) {
                        debugger;
                        $("#SaveItemReworkQtyDetails TBODY TR").each(function () {
                            var row = $(this)
                            var tblItemId = row.find("#PR_RewrkQtyItemId").val();
                            var tblWhId = row.find("#PR_RewrkQtyWhId").val();
                            if (tblItemId === ItmCode && tblWhId === WHID) {
                                TotalRewrkQty = parseFloat(TotalRewrkQty) + parseFloat(row.find("#PR_ReworkQty").val());
                            }
                        });
                    }
                    $("#LblTotalReworkQty").text(parseFloat(TotalRewrkQty).toFixed(QtyDecDigit));
                    
                },
            });
        }
        

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function PLBindItemReworkQtyDetail() {
    debugger;
    var rowcount = $('#SaveItemReworkQtyDetails tr').length;
    if (rowcount > 0) {
        var ItemList = new Array();
        $("#SaveItemReworkQtyDetails TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            debugger;
            batchList.ItemId = row.find('#PR_RewrkQtyItemId').val();
            batchList.UOMId = row.find('#PR_RewrkQtyUOMId').val();
            batchList.WHID = row.find('#PR_RewrkQtyWhId').val();
            batchList.LotNo = row.find('#PR_RewrkQtyLot').val();
            batchList.BatchNo = row.find('#PR_RewrkQtyBatch').val();
            batchList.SerialNo = row.find('#PR_RewrkQtySerial').val();
            batchList.AvailableQty = row.find('#PR_RewrkAvailQty').val();
            batchList.ReworkQty = row.find('#PR_ReworkQty').val();

            var ExDate = row.find('#PR_RewrkQtyExpryDate').val().trim();
            var FDate = "";
            if (ExDate == "") {
                FDate = "";
            }
            else {
                var date = ExDate.split("-");
                FDate = date[2] + '-' + date[1] + '-' + date[0];
            }
            batchList.ExpiryDate = FDate;
            ItemList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemList);
        $("#hdReworkQtyDetails").val(str1);
    }

}

/*---------------Insert Save Data Section start---------------*/

function ReqMaterialValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    debugger;
    if ($("#TblReqMaterialDetail >tbody >tr").length > 0) {
        var subitmFlagRM = CheckValidations_forSubItemsRMDetail();
        if (subitmFlagRM == false) {
            /*return false;*/
            ErrorFlag = "Y"
        }
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function SaveBtnClick() {
    debugger;

    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 11-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var RJOstatus = $("#hdRJO_Status").val();
    if (RJOstatus != "I") {
        if (CheckHeaderValidation() == false) {
            return false;
        }
        var subitmFlag = CheckValidations_forSubItems();
        if (subitmFlag == false) {
            return false;
        }
        if (ReqMaterialValidations() == false) {
            return false;
        }

    }
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {

        var RJONum = $("#JobCardNumber").val();
        if (RJONum == null || RJONum == "") {
            $("#hdtranstype").val("Save");
            $("#hdn_TransType").val($("#hdtranstype").val());
        }
        else {
            $("#hdtranstype").val("Update");
            $("#hdn_TransType").val($("#hdtranstype").val());
        }
       
      
            debugger;
            var FinalReqMaterialDetail = [];
            FinalReqMaterialDetail = InsertRMItemDetails();
            $("#hdnMatrlReqDetailList").val(JSON.stringify(FinalReqMaterialDetail));
            debugger;
            InsertReworkQty_Detail();
            var i_batch = $("#hdn_i_batch").val();
            var i_serial = $("#hdn_i_serial").val();
            debugger;
            /*----- Attatchment start--------*/
            $(".fileinput-upload").click();/*To Upload Img in folder*/
            var FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
            var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
            $("#hdn_Attatchment_details").val(ItemAttchmentDt);
            /*----- Attatchment End--------*/
     


            debugger;
            if ($("#JobCompletion").is(":checked")) {
                debugger;
                if (ConsumeMaterialValidations() == false) {
                    return false;
                }
                /* var subitmFlag = CheckValidations_forSubItems();*/
                if (CheckValidations_forSubItemsCMDetail() == false) {
                    return false;
                }
                if (CheckCMBatchValidation() == false) {
                    return false;
                }
                if (CheckCMSerialValidation() == false) {
                    return false;
                }
                var FinalConsMaterialDetail = [];
                FinalConsMaterialDetail = InsertCMItemDetails();
                $("#hdnConsumeMatrlDetailList").val(JSON.stringify(FinalConsMaterialDetail));

                InsertConsumeMaterialBatchDetail()
                InsertConsumeMaterialSerialDetail()
            }
            debugger;
        //var JobCmplt = "";
        debugger;
            var condition = $("#JobCompletion").is(":checked")
            $("#hdJobcompletion").val(condition);
       
       
        /*-----------Sub-item-------------*/
        var SubItemsListArr = RJO_SubItemList();
        var str2 = JSON.stringify(SubItemsListArr);
        $('#SubItemDetailsDt').val(str2);
        $("#RequiredQuantity").val("");
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        /*-----------Sub-item end-------------*/
        return true;

    }
}
function InsertRMItemDetails() {
    debugger;
    var RMItemList = [];
    $("#TblReqMaterialDetail >tbody >tr").each(function (i, row) {
        debugger;

        var currentRow = $(this);
        var SNo = currentRow.find("#hdSpanRowID").val();
        
        var RMItemTyp = "";
        var RMItemID = "";
        var RMUOMID = "";
        var RMReq_Qty = "";
        
        RMItemTyp = currentRow.find("#tblhd_RMMtrlTyp").val();
        RMItemID = currentRow.find("#tblhd_RMItemId").val();
        RMUOMID = currentRow.find("#tblhd_RMUOMId").val();
        RMReq_Qty = currentRow.find("#tblRM_ReqQuantity").val();

        

        RMItemList.push({ RMItemTyp: RMItemTyp, RMItemID: RMItemID, RMUOMID: RMUOMID, RMReq_Qty: RMReq_Qty });
    });

    return RMItemList;
};
function InsertReworkQty_Detail() {
    debugger;
    var Rewrkrowcount = $('#SaveItemReworkQtyDetails tr').length;
    if (Rewrkrowcount > 1) {
        var RwrkQtyList = new Array();
        $("#SaveItemReworkQtyDetails TBODY TR").each(function () {
            var row = $(this)
            var RwrkList = {};

            debugger;
            RwrkList.ItemId = row.find('#PR_RewrkQtyItemId').val();
            RwrkList.UOMId = row.find('#PR_RewrkQtyUOMId').val();
            RwrkList.WHID = row.find('#PR_RewrkQtyWhId').val();
            RwrkList.LotNo = row.find('#PR_RewrkQtyLot').val();
            RwrkList.BatchNo = row.find('#PR_RewrkQtyBatch').val();
            RwrkList.SerialNo = row.find('#PR_RewrkQtySerial').val();
            RwrkList.AvailStkQty = row.find('#PR_RewrkAvailQty').val();
            RwrkList.ReworkQty = row.find('#PR_ReworkQty').val();

            var ExDate = row.find('#PR_RewrkQtyExpryDate').val().trim();
            var FDate = "";
            if (ExDate == "") {
                FDate = "";
            }
            else {
                var date = ExDate.split("-");
                FDate = date[2] + '-' + date[1] + '-' + date[0];
            }
            RwrkList.ExpiryDate = FDate;
            RwrkQtyList.push(RwrkList);
            debugger;
        });
        var str1 = JSON.stringify(RwrkQtyList);
        $("#hdReworkQtyDetails").val(str1);
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
/*-------------Insert Section End-------------------------*/

/*-----------Start Workflow--------------*/
function ForwardBtnClick() {
    debugger;
    //var RJOStatus = "";
    //RJOStatus = $("#hdRJO_Status").val().trim();
    //if (RJOStatus === "D" || RJOStatus === "F") {

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
    var PkgJODt = $("#JobCardDate").val();
    $.ajax({
        type: "POST",
        /* url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: PkgJODt
        },
        success: function (data) {
            /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var RJOStatus = "";
                RJOStatus = $("#hdRJO_Status").val().trim();
                if (RJOStatus === "D" || RJOStatus === "F") {

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
                /*swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
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
    var RJONo = "";
    var RJODate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status = "";

    docid = $("#DocumentMenuId").val();
    RJONo = $("#JobCardNumber").val();
    RJODate = $("#JobCardDate").val();
    $("#hdDoc_No").val(RJONo);
    WF_Status = $("#WF_Status1").val();
    Remarks = $("#fw_remarks").val();
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

    if (fwchkval === "Forward") {
        if (fwchkval != "" && RJONo != "" && RJODate != "" && level != "") {
            debugger;
             Cmn_InsertDocument_ForwardedDetail(RJONo, RJODate, docid, level, forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/PackagingJobOrder/ToRefreshByJS";
            var list = [{ RJONo: RJONo, RJODate: RJODate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/PackagingJobOrder/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
    if (fwchkval === "Approve") {
        var list = [{ RJONo: RJONo, RJODate: RJODate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/PackagingJobOrder/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/PackagingJobOrder/SIListApprove?SI_No=" + RJONo + "&SI_Date=" + RJODate + "&InvType=" + InvType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&SaleVouMsg=" + SaleVouMsg;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && RJONo != "" && RJODate != "") {
             Cmn_InsertDocument_ForwardedDetail(RJONo, RJODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            var list = [{ RJONo: RJONo, RJODate: RJODate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/PackagingJobOrder/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && RJONo != "" && RJODate != "") {
             Cmn_InsertDocument_ForwardedDetail(RJONo, RJODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/PackagingJobOrder/ToRefreshByJS";
            var list = [{ RJONo: RJONo, RJODate: RJODate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/PackagingJobOrder/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#JobCardNumber").val();
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

/*-----------End Workflow--------------*/
function CheckedCancelled() {
    var src_type = $("#hdnsrc_type").val();
    if ($("#Cancelled").is(":checked")) {
        /*$("#JobCompletion").attr("disabled", true);*/
       
        if (src_type =="S") {
            $("#JobCompletion").attr("disabled", true);
        }
        $("#btn_save").attr("disabled", false);
     
        $("#btn_save").attr("onclick", "return SaveBtnClick()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        //$("#JobCompletion").attr("disabled", false);
        if (src_type == "S") {
            $("#JobCompletion").attr("disabled", false);
        }
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
/*------------------------Consumption Material Detail-------------------------*/

function CheckedJobCompletion() {
    debugger;
    var src_type = $("#hdnsrc_type").val();
    if ($("#JobCompletion").is(":checked")) {
        if (src_type == "S") {
            $("#Cancelled").attr("disabled", true);
        }
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return SaveBtnClick()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

        var RJO_No = $("#JobCardNumber").val();
        var RJO_Date = $("#JobCardDate").val();
        var ShopfloorId = $("#hdn_ShopfloorID").val();
        //$("#hdn_Sub_ItemDetailTbl >tbody>tr").remove();
        debugger;
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/PackagingJobOrder/GetDetailsOfRequiredMaterialTbl',
            data: {
                RJO_No: RJO_No,
                RJO_Date: RJO_Date,
                ShopfloorId: ShopfloorId

            },
            success: function (data)
            {
                debugger;
                if (data == 'ErrorPage') {
                    JO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "" && data !== "{}")
                {
                    var arr = [];
                    var arr = JSON.parse(data);

                    if (arr.Table.length > 0)
                    {
                        debugger;
                        var rowIdx = 0;
                        for (var k = 0; k < arr.Table.length; k++)
                        {
                            var S_NO = $('#TblConsumedMaterialDetail tbody tr').length;

                            var RowNo = 0;
                            var levels = [];
                            $("#TblConsumedMaterialDetail >tbody >tr").each(function (i, row) {
                                var currentRow = $(this);
                                RowNo = parseInt(currentRow.find("#CM_SNohiddenfiled").val());// + 1;
                                levels.push(RowNo);
                            });
                            if (levels.length > 0) {
                                RowNo = Math.max(...levels);
                            }
                            RowNo = RowNo + 1;
                            if (S_NO == "0") {
                                S_NO = 1;
                            }
                            debugger;
                            var MatrlTypeID = (arr.Table[k].MtrlTypId).trim();
                            var MatrlName = (arr.Table[k].item_name).trim();
                            var span_SubItemDetail = $("#span_SubItemDetail").text();
                            //var CM_subitem = $("#sub_itemReqMatrial").val();
                            var CM_subitem = arr.Table[k].sub_item;
                            var CM_subitmDisable = "";
                            if (CM_subitem != "Y") {
                                CM_subitmDisable = "disabled";
                            }
                            //var MatrlTypeID = MatrlTyp_ID
                            //$("#TblConsumedMaterialDetail >tbody >tr").remove();
                            $('#TblConsumedMaterialDetail tbody').append(`<tr id="R${++rowIdx}">
                                <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                <td id="CM_SRNO" class="sr_padding">
                                <span id="CM_SpanRowId"  value="${RowNo}">${RowNo}</span>
                                <input  type="hidden" id="CM_SNohiddenfiled" value="${RowNo}"/>
                                </td>
                                <td>
                                <input id="ConsumeMaterialType" value="${arr.Table[k].MtrlTypName}" class="form-control" type="text" name="MaterialType" placeholder="${$("#span_MaterialType").text()}" readonly>
                                <input type="hidden" id="ConsumeMaterialTypID" value="${MatrlTypeID}" style="display: none;" />
                                </td>
                                <td>
                                  <div class="col-md-11 col-sm-12 no-padding">
                                <input id="ConsumeMaterialName" value='${MatrlName}' class="form-control" autocomplete="off" type="text" name="MaterialName" placeholder="${$("#ItemName").text()}"  onblur="this.placeholder='Item Name'" readonly>
                                <input type="hidden" id="ConsumeMaterialId" value="${arr.Table[k].material_id}" style="display: none;" />
                                </div>
                                <div class="col-sm-1 i_Icon">
                                 <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'Cunsum');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title=" @Resource.ItemInformation"> </button>
                                 </div>
                                </td>
                                <td>
                                <input  id="CM_UOM" value="${arr.Table[k].UomName}" class="form-control" autocomplete="off" type="text" name="RM_UOM" placeholder="${$("#ItemUOM").text()}" readonly>
                                <input type="hidden" id="CM_UOMId" value="${arr.Table[k].uom_id}" style="display: none;" />
                                </td>
                                <td>
                                <div class="lpo_form">
                                <input  id="CM_Avl_Shfl_Stk" value="${parseFloat(arr.Table[k].avl_stock_shfl).toFixed(QtyDecDigit)}"class="form-control num_right" autocomplete="off" type="text" name="ShfloorStock" placeholder="0000.00" readonly>
                                <span id="CM_AvlShflStk_Error" class="error-message is-visible"></span>
                                </div>
                                </td>
                                <td>
                                <input id="CM_RequiredQuantity"  value="${parseFloat(arr.Table[k].req_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="RequiredQuantity" placeholder="0000.00"  readonly>
                                </td>
                                <td>
                                <div class="col-sm-10 lpo_form no-padding">
                                <input id="CM_ConsumedQuantity"  value="" onkeypress="return OnKeyPressConsumeQty(this,event);" onchange="OnChangeTblConsumeQty(this,event)" onpaste="return CopyPasteData(event)" class="form-control num_right" autocomplete="off" type="text" name="ConsumedQuantity" placeholder="0000.00">
                                <span id="CM_ConsumedQuantity_Error" class="error-message is-visible"></span>
                                </div>
                                <div class="col-sm-2 i_Icon" id="div_SubItemCMConsQty" style="padding:0px; ">
                                <input hidden type="text" id="sub_item" value="${CM_subitem}" />
                                <button type="button" id="SubItemCMConsQty" ${CM_subitmDisable} class="calculator subItmImg"  onclick="return SubItemDetailsPopUp('CM_ConsumeQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                 </div>
                                </td>

                                <td class="center"><button type="button" id="BtnBatchDetail" onclick="CMItemStockBatchWise(this,event)" class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                <input type="hidden" id="hdi_batch" value="${arr.Table[k].i_batch}" style="display: none;">
                                </td>
                                <td class="center"><button type="button" id="BtnSerialDetail" onclick="CMItemStockSerialWise(this,event)" class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                <input type="hidden" id="hdi_serial" value="${arr.Table[k].i_serial}" style="display: none;">
                                </td>
                                </tr>`);
                        }
                        HideAndShow_BatchSerial_Button();
                     }
                }
             }
        });

    }
    else {
        $("#btn_save").attr("disabled", true);
        if (src_type == "S") {
            $("#Cancelled").attr("disabled", false);
        }
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#TblConsumedMaterialDetail >tbody >tr").remove();
    }
}
function HideAndShow_BatchSerial_Button() {
    $("#TblConsumedMaterialDetail tbody tr").each(function () {
        debugger;
        var row = $(this);
        var b_flag = row.find("#hdi_batch").val();
        var s_flag = row.find("#hdi_serial").val();
        if (b_flag === "Y") {
            row.find("#BtnBatchDetail").prop("disabled", false);
        }
        else {
            row.find("#BtnBatchDetail").prop("disabled", true);
        }
        if (s_flag === "Y") {
            row.find("#BtnSerialDetail").prop("disabled", false);
        }
        else {
            row.find("#BtnSerialDetail").prop("disabled", true);
        }
    });
}
function updateConsumeMaterialSRNumber() {
    debugger;
    var SerialNo = 0;
    $("#TblConsumedMaterialDetail >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#CM_SpanRowId").text(SerialNo);
        
    });
};
function OnKeyPressConsumeQty(el, evt) {
    var clickedrow = $(evt.target).closest("tr");

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }

    clickedrow.find("#CM_ConsumedQuantity_Error").css("display", "none");
    clickedrow.find("#CM_ConsumedQuantity ").css("border-color", "#ced4da");
    return true;
}
function OnChangeTblConsumeQty(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var MatryTyp = clickedrow.find("#ConsumeMaterialTypID").val()

        var ConsumedQuantity = clickedrow.find("#CM_ConsumedQuantity").val();
        if (ConsumedQuantity == "0" || ConsumedQuantity == parseFloat(0)) {
            clickedrow.find("#CM_ConsumedQuantity").val("");
        }
        var AvlShflStk = clickedrow.find("#CM_Avl_Shfl_Stk").val();
        var RequiredQuantity = clickedrow.find("#CM_RequiredQuantity").val();
        if (ConsumedQuantity > 0) {
            if (MatryTyp == "SR") {
                //if (ConsumedQuantity != "" && ConsumedQuantity != null && RequiredQuantity != "" && RequiredQuantity != null) {
                //    if (parseFloat(ConsumedQuantity) > parseFloat(RequiredQuantity)) {
                //        clickedrow.find("#CM_ConsumedQuantity_Error").text($("#ExceedingQty").text());
                //        clickedrow.find("#CM_ConsumedQuantity_Error").css("display", "block");
                //        clickedrow.find("#CM_ConsumedQuantity").css("border-color", "red");
                //    }
                //    else {
                //        clickedrow.find("#CM_ConsumedQuantity_Error").css("display", "none");
                //        clickedrow.find("#CM_ConsumedQuantity").css("border-color", "#ced4da");
                //        var test = parseFloat(parseFloat(ConsumedQuantity)).toFixed(parseFloat(QtyDecDigit));
                //        clickedrow.find("#CM_ConsumedQuantity").val(test);
                //    }
                //}
            }
            else {
                if (ConsumedQuantity != "" && ConsumedQuantity != null && AvlShflStk != "" && AvlShflStk != null) {
                    if (parseFloat(ConsumedQuantity) > parseFloat(AvlShflStk)) {
                        clickedrow.find("#CM_ConsumedQuantity_Error").text($("#ExceedingQty").text());
                        clickedrow.find("#CM_ConsumedQuantity_Error").css("display", "block");
                        clickedrow.find("#CM_ConsumedQuantity").css("border-color", "red");
                    }
                    else {
                        clickedrow.find("#CM_ConsumedQuantity_Error").css("display", "none");
                        clickedrow.find("#CM_ConsumedQuantity").css("border-color", "#ced4da");
                        var test = parseFloat(parseFloat(ConsumedQuantity)).toFixed(parseFloat(QtyDecDigit));
                        clickedrow.find("#CM_ConsumedQuantity").val(test);
                    }
                }
            }
        }
        
        
        
    }
    catch (err) {
        console.log("Error : " + err.message);
    }
}
function ConsumeMaterialValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#TblConsumedMaterialDetail >tbody >tr").length > 0) {
        $("#TblConsumedMaterialDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            //var currentRow = $(evt.target).closest("tr");
            var ConsumedQuantity = currentRow.find("#CM_ConsumedQuantity").val();
            var AvlShflStk = currentRow.find("#CM_Avl_Shfl_Stk").val();
            var MatryTyp = currentRow.find("#ConsumeMaterialTypID").val()
            var RequiredQuantity = currentRow.find("#CM_RequiredQuantity").val();
            if (MatryTyp == "SR") {
                
            }
            else {
                if (AvlShflStk == "0.000" || AvlShflStk == "" || AvlShflStk == null) {
                    currentRow.find("#CM_AvlShflStk_Error").text($("#Stocknotavailable").text());
                    currentRow.find("#CM_AvlShflStk_Error").css("display", "block");
                    currentRow.find("#CM_Avl_Shfl_Stk").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#CM_AvlShflStk_Error").css("display", "none");
                    currentRow.find("#CM_Avl_Shfl_Stk").css("border-color", "#ced4da");
                    var test = parseFloat(parseFloat(AvlShflStk)).toFixed(parseFloat(QtyDecDigit));
                    currentRow.find("#CM_Avl_Shfl_Stk").val(test);
                }
            }
            
            if (ConsumedQuantity == "" || ConsumedQuantity == null) {
                currentRow.find("#CM_ConsumedQuantity_Error").text($("#valueReq").text());
                currentRow.find("#CM_ConsumedQuantity_Error").css("display", "block");
                currentRow.find("#CM_ConsumedQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#CM_ConsumedQuantity_Error").css("display", "none");
                currentRow.find("#CM_ConsumedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(parseFloat(ConsumedQuantity)).toFixed(parseFloat(QtyDecDigit));
                currentRow.find("#CM_ConsumedQuantity").val(test);
            }
            if (MatryTyp == "SR") {
                //if (ConsumedQuantity != "" && ConsumedQuantity != null && RequiredQuantity != "" && RequiredQuantity != null) {
                //    if (parseFloat(ConsumedQuantity) > parseFloat(RequiredQuantity)) {
                //        currentRow.find("#CM_ConsumedQuantity_Error").text($("#ExceedingQty").text());
                //        currentRow.find("#CM_ConsumedQuantity_Error").css("display", "block");
                //        currentRow.find("#CM_ConsumedQuantity").css("border-color", "red");
                //        ErrorFlag = "Y";
                //    }
                //    else {
                //        currentRow.find("#CM_ConsumedQuantity_Error").css("display", "none");
                //        currentRow.find("#CM_ConsumedQuantity").css("border-color", "#ced4da");
                //        var test = parseFloat(parseFloat(ConsumedQuantity)).toFixed(parseFloat(QtyDecDigit));
                //        currentRow.find("#CM_ConsumedQuantity").val(test);
                //    }
                //}
            }
            else {
                if (ConsumedQuantity != "" && ConsumedQuantity != null && AvlShflStk != "" && AvlShflStk != null) {
                    if (parseFloat(ConsumedQuantity) > parseFloat(AvlShflStk)) {
                        currentRow.find("#CM_ConsumedQuantity_Error").text($("#ExceedingQty").text());
                        currentRow.find("#CM_ConsumedQuantity_Error").css("display", "block");
                        currentRow.find("#CM_ConsumedQuantity").css("border-color", "red");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#CM_ConsumedQuantity_Error").css("display", "none");
                        currentRow.find("#CM_ConsumedQuantity").css("border-color", "#ced4da");
                        var test = parseFloat(parseFloat(ConsumedQuantity)).toFixed(parseFloat(QtyDecDigit));
                        currentRow.find("#CM_ConsumedQuantity").val(test);
                    }
                }
            }

        });
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function InsertCMItemDetails() {
    debugger;
    var CMItemList = [];
    $("#TblConsumedMaterialDetail >tbody >tr").each(function (i, row) {
        debugger;

        var currentRow = $(this);
        var SNo = currentRow.find("#hdSpanRowID").val();
        var CMRJONo = "";
        var CMRJODate = "";
        var CMItemTyp = "";
        var CMItemID = "";
        var CMUOMID = "";
        var CMReq_Qty = "";
        var CMCons_Qty = "";
        CMRJONo = $("#JobCardNumber").val();
        CMRJODate = $("#JobCardDate").val();
        CMItemTyp = currentRow.find("#ConsumeMaterialTypID").val();
        CMItemID = currentRow.find("#ConsumeMaterialId").val();
        CMUOMID = currentRow.find("#CM_UOMId").val();
        CMReq_Qty = currentRow.find("#CM_RequiredQuantity").val();
        CMCons_Qty = currentRow.find("#CM_ConsumedQuantity").val();



        CMItemList.push({ CMRJONo: CMRJONo, CMRJODate:CMRJODate,CMItemTyp: CMItemTyp, CMItemID: CMItemID, CMUOMID: CMUOMID, CMReq_Qty: CMReq_Qty, CMCons_Qty: CMCons_Qty });
    });

    return CMItemList;
};
//-----------------------------Consumed Material Batch Details---------------------------//
function CheckCMBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    //var QtyDecDigit = $("#QtyDigit").text();
    $("#TblConsumedMaterialDetail >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var MatrlTypId = clickedrow.find("#ConsumeMaterialTypID").val();
        var ConsumedQuantity = clickedrow.find("#CM_ConsumedQuantity").val();
        var ItemId = clickedrow.find("#ConsumeMaterialId").val();
        var UOMId = clickedrow.find("#CM_UOMId").val();
        var Batchable = clickedrow.find("#hdi_batch").val();

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#CMSaveItemBatchTbl >tbody >tr").length == 0) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#BtnBatchDetail").css("border-color", "red");
                ValidateEyeColor(clickedrow, "BtnBatchDetail", "Y");
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#CMSaveItemBatchTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchConsumeQty = currentRow.find('#CM_BatchConsumeQty').val();
                    var bchMtrlTypId = currentRow.find('#CM_BatchMtrlTypId').val();
                    var bchitemid = currentRow.find('#CM_BatchItemId').val();
                    var bchuomid = currentRow.find('#CM_BatchUOMId').val();
                    if (ItemId == bchitemid && UOMId == bchuomid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchConsumeQty);
                    }
                });

                if (parseFloat(ConsumedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
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
        swal("", $("#Batchqtydoesnotmatchwithconsumedqty").text(), "warning");
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
function CMItemStockBatchWise(el, evt) {
    try {
        debugger;
        CMBindItemBatchDetail();
        var clickedrow = $(evt.target).closest("tr");
        var ItemId = clickedrow.find("#ConsumeMaterialId").val();
        var MtrlTypId = clickedrow.find("#ConsumeMaterialTypID").val();
        var ShpfloorId = $("#hdn_ShopfloorID").val();

        var ItemName = clickedrow.find("#ConsumeMaterialName").val();
        var UOMName = clickedrow.find("#CM_UOM").val();
        var ConsumedQty = clickedrow.find("#CM_ConsumedQuantity").val();
        var SelectedItemdetail = $("#HDSelectedBatchwiseCM").val();
        var UOMId = clickedrow.find("#CM_UOMId").val();
        var docid = $("#DocumentMenuId").val();
        var hdnTranstyp = $("#hdtranstype").val();
        var hdnCommand = $("#hdn_command").val();
        //var TransType = $("#qty_TransType").val();/*This is use only Partial View */
        //var Command = $("#Qty_pari_Command").val();/*This is use only Partial View */
        var CMRJO_Status = $("#hdRJO_Status").val().trim();
        var documentlist = new Array();
        

        if (CMRJO_Status == "I" || CMRJO_Status == "PFC") {
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/PackagingJobOrder/getCMItemStockBatchWiseBYShpfloor",
                data: {
                    ItemId: ItemId,
                    ShpfloorId: ShpfloorId,
                    Status: CMRJO_Status,
                    SelectedItemdetail: SelectedItemdetail,
                    docid: docid,
                    TransType: hdnTranstyp,
                    Command: hdnCommand
                },
                success: function (data) {
                    debugger;
                    $('#CMItemStockBatchWise').html(data);
                    $("#ItemNameBatchWise").val(ItemName);
                    $("#UOMBatchWise").val(UOMName);
                    $("#QuantityBatchWise").val(ConsumedQty);
                    $("#HDItemNameBatchWise").val(ItemId);
                    $("#HDMatrlTypIDBatchWise").val(MtrlTypId);
                    //$("#HDWhIDBatchWise").val(WarehouseId);
                    $("#HDUOMBatchWise").val(UOMId); 


                    var TotalConsumedBatch = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#CMSaveItemBatchTbl TBODY TR").length > 0) {
                        $("#CMSaveItemBatchTbl TBODY TR").each(function () {
                            var row = $(this)
                            var BtMtrlTypId = row.find("#CM_BatchMtrlTypId").val();
                            var BtItemId = row.find("#CM_BatchItemId").val();
                            if (BtItemId === ItemId) {
                                TotalConsumedBatch = parseFloat(TotalConsumedBatch) + parseFloat(row.find("#CM_BatchConsumeQty").val());
                            }
                        });
                    }

                    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                        var row = $(this)
                        var consumeQty = row.find("#ConsumedQuantity").val();
                        if (consumeQty != null && consumeQty != "") {
                            row.find("#ConsumedQuantity").val(parseFloat(consumeQty).toFixed(QtyDecDigit))
                        }
                    });
                    $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalConsumedBatch).toFixed(QtyDecDigit));

                    try {
                        //For Auto fill Quantity on FIFO basis in the Batch Table.
                        //this will work only first time after save old value will come in the table
                        Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", ConsumedQty, "AvailableQuantity", "ConsumedQuantity", "BatchwiseTotalIssuedQuantity");
                    } catch (err) {
                        console.log('Error : ' + err.message)
                    }
                },
            });
        }
        else {
            var RJO_No = $("#JobCardNumber").val();
            var RJO_Date = $("#JobCardDate").val();
            var docid = $("#DocumentMenuId").val();
            
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/PackagingJobOrder/getCMItemStockBatchWiseAfterInsert",
                data: {
                    RJO_No: RJO_No,
                    RJO_Date: RJO_Date,
                    Status: CMRJO_Status,
                    MtrlTypId: MtrlTypId,
                    ItemId: ItemId,
                    docid: docid,
                    TransType: hdnTranstyp,
                    Command: hdnCommand
                },
                success: function (data) {
                    debugger;
                    $('#CMItemStockBatchWise').html(data);
                    $("#ItemNameBatchWise").val(ItemName);
                    $("#UOMBatchWise").val(UOMName);
                    $("#QuantityBatchWise").val(ConsumedQty);
                    //$("#HDWhIDBatchWise").val(WarehouseId);
                    $("#HDItemNameBatchWise").val(ItemId);
                    $("#HDMatrlTypIDBatchWise").val(MtrlTypId);
                    $("#HDUOMBatchWise").val(UOMId);

                    var TotalConsumedBatch = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#CMSaveItemBatchTbl TBODY TR").length > 0) {
                        $("#CMSaveItemBatchTbl TBODY TR").each(function () {
                            var row = $(this)
                            var btItemId = row.find("#CM_BatchItemId").val();
                            if (btItemId === ItemId) {
                                TotalConsumedBatch = parseFloat(TotalConsumedBatch) + parseFloat(row.find("#CM_BatchConsumeQty").val());
                            }
                        });
                    }
                    $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalConsumedBatch).toFixed(QtyDecDigit));
                   
                },
            });
        }
        

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickItemBatchResetbtn() {
    debugger;
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#ConsumedQuantity").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var HdItemId = $("#HDItemNameBatchWise").val();
    //var MtrlTypId = clickedrow.find("#ConsumeMaterialTypID").val();
    var ItemCMReqQty = $("#QuantityBatchWise").val();
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    var FlagMsg = "N";
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var TotalAvailableQuantity = row.find("#AvailableQuantity").val();
        //var TotalAvailableQuantity = row.find("#ToatlAvlQuantity").val();
        var ConsumedQuantity = row.find("#ConsumedQuantity").val();
        if (parseFloat(ConsumedQuantity) > parseFloat(TotalAvailableQuantity)) {
            FlagMsg = "Y";
            row.find("#Quantity_Error").text($("#ExceedingQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
            ValidateEyeColor(row, "BtnBatchDetail", "Y");
            return false;
        }
    });
    
    if (FlagMsg === "N") {
        if (parseFloat(ItemCMReqQty) != parseFloat(ItemTotalIssuedQuantity)) {
            swal("", $("#ConsumedQuantityDoesNotMatchWithBatchQuantity").text(), "warning");
        }
        else {
            debugger;
            var SelectedItem = $("#HDItemNameBatchWise").val();
            $("#CMSaveItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#CM_BatchItemId").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });

            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemConsumeQuantity = row.find("#ConsumedQuantity").val();
                if (ItemConsumeQuantity != "" && ItemConsumeQuantity != null) {
                    debugger;
                    var BatchMarlTypId = $("#HDMatrlTypIDBatchWise").val();
                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    var ItemBatchAvlstock = row.find("#AvailableQuantity").val();
                    /*var ItemBatchResstock = row.find("#ResQty").val();*/
                    var ItemExpiryDate = row.find("#ExpiryDate").val();
                    var LotNo = row.find("#Lot").val();
                    $('#CMSaveItemBatchTbl tbody').append(`<tr>
                    <td><input type="text" id="CM_BatchMtrlTypId" value="${BatchMarlTypId}" /></td>
                    <td><input type="text" id="CM_BatchItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="CM_BatchUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="CM_BatchLotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="CM_BatchBatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="CM_BatchExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="CM_BatchBatchAvlStk" value="${ItemBatchAvlstock}" /></td>
                    <td><input type="text" id="CM_BatchConsumeQty" value="${ItemConsumeQuantity}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');

            $("#TblConsumedMaterialDetail >tbody >tr").each(function () {
                debugger;
                var clickedrow = $(this);
                var ItemId = clickedrow.find("#ConsumeMaterialId").val();
                if (ItemId == SelectedItem) {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#BtnBatchDetail").css("border-color", "#007bff");
                    ValidateEyeColor(clickedrow, "BtnBatchDetail", "N");
                    
                }
            });
        }
    }
}
function CMBindItemBatchDetail() {
    debugger;
    var batchrowcount = $('#CMSaveItemBatchTbl tr').length;
    if (batchrowcount > 1) {
        var ItemBatchList = new Array();
        $("#CMSaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            debugger;
            batchList.MatrlTypeID = row.find('#CM_BatchMtrlTypId').val();
            batchList.ItemId = row.find('#CM_BatchItemId').val();
            batchList.UOMId = row.find('#CM_BatchUOMId').val();
            batchList.LotNo = row.find('#CM_BatchLotNo').val();
            batchList.BatchNo = row.find('#CM_BatchBatchNo').val();
            batchList.BatchAvlStock = row.find('#CM_BatchBatchAvlStk').val();
            batchList.ConsumeQty = row.find('#CM_BatchConsumeQty').val();

            var ExDate = row.find('#CM_BatchExpiryDate').val().trim();
            var FDate = "";
            if (ExDate == "") {
                FDate = "";
            }
            else {
                var date = ExDate.split("-");
                FDate = date[2] + '-' + date[1] + '-' + date[0];
            }
            batchList.ExpiryDate = FDate;
            ItemBatchList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwiseCM").val(str1);
    }

}
function onclickbtnItemBatchDiscardAndExit() {
    $("#BatchNumber").modal('hide');
}
function OnChangeConsumedQty(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var ConsumedQuantity = clickedrow.find("#ConsumedQuantity").val();
        if (ConsumedQuantity == "0" || ConsumedQuantity == parseFloat(0)) {
            clickedrow.find("#ConsumedQuantity").val("");
        }
        var TotalAvailableQuantity = clickedrow.find("#AvailableQuantity").val();
        if (ConsumedQuantity > 0) {
            if (ConsumedQuantity != "" && ConsumedQuantity != null && TotalAvailableQuantity != "" && TotalAvailableQuantity != null) {
                if (parseFloat(ConsumedQuantity) > parseFloat(TotalAvailableQuantity)) {
                    clickedrow.find("#ConsumedQuantity_Error").text($("#ExceedingQty").text());
                    clickedrow.find("#ConsumedQuantity_Error").css("display", "block");
                    clickedrow.find("#ConsumedQuantity").css("border-color", "red");
                }
                else {
                    clickedrow.find("#ConsumedQuantity_Error").css("display", "none");
                    clickedrow.find("#ConsumedQuantity").css("border-color", "#ced4da");
                    var test = parseFloat(parseFloat(ConsumedQuantity)).toFixed(parseFloat(QtyDecDigit));
                    clickedrow.find("#ConsumedQuantity").val(test);
                }
            }

        }
         TotalBatchConsumeQty();
    }
    catch (err) {
        console.log("Error : " + err.message);
    }
}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function TotalBatchConsumeQty() {
    var TotalConsumeQty = 0;
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var Consumeqty = row.find("#ConsumedQuantity").val();
        if (Consumeqty != "" && Consumeqty != null) {
            TotalConsumeQty = TotalConsumeQty + parseFloat(Consumeqty);
        }
    });
    $("#BatchwiseTotalIssuedQuantity").text(parseFloat(parseFloat(TotalConsumeQty)).toFixed(parseFloat(QtyDecDigit)));
}
//------ For Insert or Save Data of Batch and Serial Detail into Table------//

function InsertConsumeMaterialBatchDetail() {
    debugger;
    var batchrowcount = $('#CMSaveItemBatchTbl tr').length;
    if (batchrowcount > 1) {
        var ItemBatchList = new Array();
        $("#CMSaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            batchList.MatrlTypeID = row.find('#CM_BatchMtrlTypId').val();
            batchList.ItemId = row.find('#CM_BatchItemId').val();
            batchList.UOMId = row.find('#CM_BatchUOMId').val();
            batchList.LotNo = row.find('#CM_BatchLotNo').val();
            batchList.BatchNo = row.find('#CM_BatchBatchNo').val();
            batchList.BatchAvlStock = row.find('#CM_BatchBatchAvlStk').val();
            batchList.ConsumeQty = row.find('#CM_BatchConsumeQty').val();

            var ExDate = row.find('#CM_BatchExpiryDate').val().trim();
            var FDate = "";
            if (ExDate == "") {
                FDate = "";
            }
            else {
                var date = ExDate.split("-");
                FDate = date[2] + '-' + date[1] + '-' + date[0];
            }
            batchList.ExpiryDate = FDate;
            ItemBatchList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwiseCM").val(str1);


    //    batchList.MatrlTypeID = row.find('#CM_BatchMtrlTypId').val();
    //    batchList.ItemId = row.find('#CM_BatchItemId').val();
    //    batchList.UOMId = row.find('#CM_BatchUOMId').val();
    //    batchList.LotNo = row.find('#CM_BatchLotNo').val();
    //    batchList.BatchNo = row.find('#CM_BatchBatchNo').val();
    //    batchList.BatchAvlStock = row.find('#CM_BatchBatchAvlStk').val();
    //    batchList.ConsumeQty = row.find('#CM_BatchConsumeQty').val();

    //    var ExDate = row.find('#CM_BatchExpiryDate').val().trim();
    //    var FDate = "";
    //    if (ExDate == "") {
    //        FDate = "";
    //    }
    //    else {
    //        var date = ExDate.split("-");
    //        FDate = date[2] + '-' + date[1] + '-' + date[0];
    //    }
    //    batchList.ExpiryDate = FDate;
    //    ItemBatchList.push(batchList);
    //    debugger;
    //});
    //var str1 = JSON.stringify(ItemBatchList);
    //$("#HDSelectedBatchwiseCM").val(str1);
    }

}
function InsertConsumeMaterialSerialDetail() {
    debugger;
    var serialrowcount = $('#CMSaveItemSerialTbl tr').length;
    if (serialrowcount > 1) {
        var ItemSerialList = new Array();
        $("#CMSaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this)
            var SerialList = {};
            SerialList.MaterialtypID = row.find("#CM_SerialMtrlTypId").val();
            SerialList.ItemId = row.find("#CM_SerialItemId").val();
            SerialList.UOMId = row.find("#CM_SerialUOMId").val();
            SerialList.LOTId = row.find("#CM_SerialLOTNo").val();
            SerialList.ConsumedQty = row.find("#CM_SerialConsumeQty").val();
            SerialList.SerialNO = row.find("#CM_BatchSerialNO").val().trim();
            ItemSerialList.push(SerialList);

        });
        var str2 = JSON.stringify(ItemSerialList);
        $("#HDSelectedSerialwiseCM").val(str2);

    }

}
function DeleteItemBatchSerialDetails(Itemid) {
    $("#CMSaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#CM_BatchItemId").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    $("#CMSaveItemSerialTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#CM_SerialItemId").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });

}
//----------------------------------End-------------------------------//
//-----------------------------Serial No Details---------------------------//
function CheckCMSerialValidation() {
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    //var QtyDecDigit = $("#QtyDigit").text();
    $("#TblConsumedMaterialDetail >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ConsumedQuantity = clickedrow.find("#ConsumedQuantity").val();
        var ItemId = clickedrow.find("#ConsumeMaterialId").val();
        var UOMId = clickedrow.find("#CM_UOMId").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#CMSaveItemSerialTbl >tbody >tr").length == 0) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#BtnSerialDetail").css("border-color", "red");
                ValidateEyeColor(clickedrow, "BtnSerialDetail", "Y");
                
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#CMSaveItemSerialTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var srialConsumeQty = currentRow.find('#CM_SerialConsumeQty').val();
                    var srialitemid = currentRow.find('#CM_SerialItemId').val();
                    var srialuomid = currentRow.find('#CM_SerialUOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialConsumeQty);
                    }
                });

                if (parseFloat(ConsumedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
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
    

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CMItemStockSerialWise(el, evt) {
    try {
        debugger;
        CMBindItemSerialDetail();
        var clickedrow = $(evt.target).closest("tr");
        
        var MtrlTypId = clickedrow.find("#ConsumeMaterialTypID").val();
        var ItemName = clickedrow.find("#ConsumeMaterialName").val();
        var UOMName = clickedrow.find("#CM_UOM").val();
        var ItemId = clickedrow.find("#ConsumeMaterialId").val();;
        var UOMID = clickedrow.find("#CM_UOMId").val();
        var ShpfloorId = $("#hdn_ShopfloorID").val();
        
        var ConsumedQty = clickedrow.find("#CM_ConsumedQuantity").val();
        var SelectedItemSerial = $("#HDSelectedSerialwiseCM").val();
        var hdnTranstyp = $("#hdtranstype").val();
        var hdnCommand = $("#hdn_command").val();
        var CMRJO_Status = $("#hdRJO_Status").val().trim();
        var docid = $("#DocumentMenuId").val();

        
        if (CMRJO_Status == "I") {
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/PackagingJobOrder/getItemstockSerialWiseByShpFloor",
                data: {
                    ItemId: ItemId,
                    Status: CMRJO_Status,
                    ShpfloorId: ShpfloorId,
                    SelectedItemSerial: SelectedItemSerial,
                    docid: docid,
                    TransType: hdnTranstyp,
                    Command: hdnCommand

                },
                success: function (data) {
                    debugger;
                    $('#CMItemStockSerialWise').html(data);

                    $("#ItemNameSerialWise").val(ItemName);
                    $("#UOMSerialWise").val(UOMName);
                    $("#QuantitySerialWise").val(ConsumedQty);
                    $("#HDMatrlTypIdSerialWise").val(MtrlTypId);
                    $("#HDItemIDSerialWise").val(ItemId);
                    $("#HDUOMIDSerialWise").val(UOMID);
                    
                    var TotalConsumedSerial = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#CMSaveItemSerialTbl TBODY TR").length > 0) {
                        $("#CMSaveItemSerialTbl TBODY TR").each(function () {
                            var row = $(this)
                            var ItemId = row.find("#CM_SerialItemId").val();
                            if (ItemId === ItemId) {
                                TotalConsumedSerial = parseFloat(TotalConsumedSerial) + parseFloat(row.find("#CM_SerialConsumeQty").val());
                            }
                        });
                    }
                    $("#TotalIssuedSerial").text(parseFloat(TotalConsumedSerial).toFixed(QtyDecDigit));
                },
            });
        }
        else /*if (PL_Status == "A" || PL_Status == "C")*/ {
            //var PL_Type = "D";
            var RJO_No = $("#JobCardNumber").val();
            var RJO_Date = $("#JobCardDate").val();
            var docid = $("#DocumentMenuId").val();
            
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/PackagingJobOrder/getCMItemStockSerialWiseAfterInsert",
                data: {
                    RJO_No: RJO_No,
                    RJO_Date: RJO_Date,
                    Status: CMRJO_Status,
                    MtrlTypId: MtrlTypId,
                    ItemId: ItemId,
                    docid: docid,
                    TransType: hdnTranstyp,
                    Command: hdnCommand
                },
                success: function (data) {
                    debugger;
                    $('#CMItemStockSerialWise').html(data);
                    $("#ItemNameSerialWise").val(ItemName);
                    $("#UOMSerialWise").val(UOMName);
                    $("#QuantitySerialWise").val(ConsumedQty);
                    $("#HDItemIDSerialWise").val(ItemId);
                    $("#HDUOMIDSerialWise").val(UOMID);

                    var TotalConsumedSerial = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#CMSaveItemSerialTbl TBODY TR").length > 0) {
                        $("#CMSaveItemSerialTbl TBODY TR").each(function () {
                            var row = $(this)
                            var ItemId = row.find("#CM_SerialItemId").val();
                            if (ItemId === ItemId) {
                                TotalConsumedSerial = parseFloat(TotalConsumedSerial) + parseFloat(row.find("#CM_SerialConsumeQty").val());
                            }
                        });
                    }
                    $("#TotalIssuedSerial").text(parseFloat(TotalConsumedSerial).toFixed(QtyDecDigit));
                },
            });
        }
        
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function onchangeChkItemSerialWise() {
    var TotalConsumeLot = 0;
    debugger;
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        if (row.find("#ChkItemSerialWise").is(":checked")) {
            TotalConsumeLot = parseFloat(TotalConsumeLot) + 1;
        }
    });
    $("#TotalIssuedSerial").text(parseFloat(TotalConsumeLot).toFixed(QtyDecDigit));
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
    var ItemShipQty = $("#QuantitySerialWise").val();
    var ItemTotalIssuedQuantity = $("#TotalIssuedSerial").text();
    if (parseFloat(ItemShipQty) != parseFloat(ItemTotalIssuedQuantity)) {
        swal("", $("#ConsumedQuantityDoesNotMatchWithSerialQuantity").text(), "warning");
    }
    else {
        debugger;
        var SelectedItem = $("#HDItemIDSerialWise").val();
        $("#CMSaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#CM_SerialItemId").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
        $("#ItemSerialwiseTbl TBODY TR").each(function () {
            var row = $(this);
            var MatrlTypID = $("#HDMatrlTypIdSerialWise").val();
            var ItemUOMID = $("#HDUOMIDSerialWise").val();
            var ItemId = $("#HDItemIDSerialWise").val();
            var ItemLOTNO = row.find("#Lot").val();
            var ItemConsumeQuantity = "1";
            var ItemSerialNO = row.find("#SerialNumberDetail").val();
            if (row.find("#ChkItemSerialWise").is(":checked")) {
                $('#CMSaveItemSerialTbl tbody').append(`<tr>
            <td><input type="text" id="CM_SerialMtrlTypId" value="${MatrlTypID}" /></td>
            <td><input type="text" id="CM_SerialItemId" value="${ItemId}" /></td>
            <td><input type="text" id="CM_SerialUOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="CM_SerialLOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="CM_SerialConsumeQty" value="${ItemConsumeQuantity}" /></td>
            <td><input type="text" id="CM_BatchSerialNO" value="${ItemSerialNO}" /></td>
            </tr>`);
            }
        });
        $("#SerialDetail").modal('hide');

        $("#TblConsumedMaterialDetail >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#ConsumeMaterialId").val();
            if (ItemId == SelectedItem) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#BtnSerialDetail").css("border-color", "#007bff");
                ValidateEyeColor(clickedrow, "BtnSerialDetail", "N");
                
            }
        });
    }
}
function CMBindItemSerialDetail() {
    var serialrowcount = $('#CMSaveItemSerialTbl tr').length;
    if (serialrowcount > 1) {
        var ItemSerialList = new Array();
        $("#CMSaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this)
            var SerialList = {};
            SerialList.MatrlTypeID = row.find("#CM_SerialMtrlTypId").val();
            SerialList.ItemId = row.find("#CM_SerialItemId").val();
            SerialList.UOMId = row.find("#CM_SerialUOMId").val();
            SerialList.LOTId = row.find("#CM_SerialLOTNo").val();
            SerialList.ConsumedQuantity = row.find("#CM_SerialConsumeQty").val();
            SerialList.SerialNO = row.find("#CM_BatchSerialNO").val().trim();
            ItemSerialList.push(SerialList);
            debugger;
        });
        var str2 = JSON.stringify(ItemSerialList);
        $("#HDSelectedSerialwiseCM").val(str2);

    }

}
function onclickbtnItemSerialDiscardAndExit() {
    $("#SerialDetail").modal('hide');
}
//----------------------------------End-------------------------------//

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_itemHeadr", "SubItemRewrkQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemRMReqQty");
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var IsDisabled = $("#DisableSubItem").val();
    var Shfl_Id = "";
    var matrial_type = "";
    if (flag == "RJO_RewrkQuantity") {
        $("#sub_itemHeadrFlag").val(flag); 
        var ProductNm = $("#ddl_ItemName option:selected").text();
        var ProductId = $("#ddl_ItemName").val();
        var UOM = $("#UOM").val();
        var Wh_id = $("#ddl_WarehouseName option:selected").val();       
    }
    else if (flag == "RM_RequireQty")
    {
        matrial_type = clickdRow.find("#tblhd_RMMtrlTyp").val();
        ProductId = clickdRow.find("#tblhd_RMItemId").val();
        ProductNm = clickdRow.find("#tblMaterialName").val();
        UOM = clickdRow.find("#tblRM_UOM").val();
        Shfl_Id = $("#ddl_ShopfloorName").val();
    } 
    else if (flag == "CM_ConsumeQty") {
        $("#sub_itemConsMtrFlag").val(flag);
        ProductId = clickdRow.find("#ConsumeMaterialId").val();
        ProductNm = clickdRow.find("#ConsumeMaterialName").val();
        UOM = clickdRow.find("#CM_UOM").val();      
        Shfl_Id = $("#ddl_ShopfloorName").val();
        ConsumeMaterialTypID = clickdRow.find("#ConsumeMaterialTypID").val();
        $("#hdnMatrialtypeForCunsume").val(ConsumeMaterialTypID);
       IsDisabled="N"
    }
    else
    {
        ProductId = clickdRow.find("#OutputMateriallId").val();
        ProductNm = clickdRow.find("#OutputMaterialName").val();
        UOM = clickdRow.find("#OutputMaterial_UOM").val();
     }
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "RJO_RewrkQuantity" || flag == "RM_RequireQty" || flag == "CM_ConsumeQty") {
        $("#hdn_Sub_ItemDetailTbl tbody tr  #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            debugger;
            if (flag == "RJO_RewrkQuantity" || flag == "CM_ConsumeQty" || flag == "CM_ConsumeQty") {
                List.avl_stock = row.find('#subItemAvlQty').val();
            }
            List.type = row.find('#subItemType').val();
            if (flag == "CM_ConsumeQty") {
                List.req_qty = row.find('#subItemReqQty').val();
            }
            NewArr.push(List);
        });
        debugger;
        if (flag == "RJO_RewrkQuantity") {
            Sub_Quantity = $("#txtReworkQuantity").val();
        }
        else if (flag == "RM_RequireQty") {
            Sub_Quantity = clickdRow.find("#tblRM_ReqQuantity").val();
        }
        else {
            Sub_Quantity = clickdRow.find("#CM_ConsumedQuantity").val();
        }
    }
    else if (flag == "RJO_QCAccptQty") {
        Sub_Quantity = clickdRow.find("#QCAcceptQuantity").val()
    }
    else if (flag == "RJO_QCRejQty") {
        Sub_Quantity = clickdRow.find("#QCRejQuantity").val()
    }
    else if (flag == "RJO_QCRwkQty") {
        Sub_Quantity = clickdRow.find("#QCRewrkQuantity").val()
    }
    /*var IsDisabled = $("#DisableSubItem").val();*/
    var hd_Status = $("#hdRJO_Status").val();
    hd_Status = IsNull(hd_Status, "").trim();
    var RJO_no = $("#JobCardNumber").val();
    var RJO_dt = $("#JobCardDate").val();
    var src_type = $("#hdnsrc_type").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/PackagingJobOrder/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: RJO_no,
            Doc_dt: RJO_dt,
            Wh_id: Wh_id,
            Shfl_Id: Shfl_Id,
            src_type: src_type,
            matrial_type: matrial_type
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
function fn_CustomReSetRewrkSubItemData(itemId, SubItmType) {
    debugger;
    //$("#hdn_Sub_ItemDetailTbl >tbody>tr").remove();
    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
        debugger;
        var Crow = $(this).closest("tr");
        //var ItemId = Crow.find("#ItemId").val();
        var subItemId = Crow.find("#subItemId").val();
        var subItemQty = Crow.find("#subItemQty").val();
        if (SubItmType == "SubHedr" || SubItmType == "SubCM") {
            var subItemAvlQty = Crow.find("#subItemAvlQty").val();

        }
        else {
            var subItemAvlQty = "0";
        }
        if (SubItmType == "SubCM") {
            var subItemReqQty = Crow.find("#subItemReqQty").val();
        }
        else {
            var subItemReqQty = "0";
        }
        var Matrialtype = $("#hdnMatrialtypeForCunsume").val();
        if (SubItmType == "SubCM" && Matrialtype != "SR") {
            if (parseFloat(subItemAvlQty) == parseFloat(0)) {
                Crow.find('#subItemQty_Error').text($("#ExceedingQty").text());
                Crow.find("#subItemQty").css("border-color", "Red");
                Crow.find("#subItemQty_Error").css("display", "block");
                return false;
            }
            else {
                Crow.find("#subItemQty_Error").css("display", "none");
                Crow.find("#subItemQty").css("border-color", "#ced4da");
                var subItemTyp = Crow.find("#subItemType").val(SubItmType);
                var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr').length;
                var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr');

                /*if (rows.len > 0) {*/
                if (rows.length > 0) {
                    rows.each(function () {
                        var InnerCrow = $(this).closest("tr");
                        debugger;
                        //var ItemId = InnerCrow.find("#ItemId").val();
                        //var subitm_typ = InnerCrow.find("#subItemType").val();
                        //if (subitm_typ == SubItmType) {
                        InnerCrow.find("#subItemQty").val(subItemQty);
                        InnerCrow.find("#subItemAvlQty").val(subItemAvlQty);
                        /* }*/

                    });
                } else {

                    $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                            <td><input type="text" id="subItemAvlQty" value='${subItemAvlQty}'></td>
                            <td><input type="text" id="subItemType" value='${SubItmType}'></td>
                            <td><input type="text" id="subItemReqQty" value='${subItemReqQty}'></td>
                        </tr>`);

                }
            }
        }
        else {
            Crow.find("#subItemQty_Error").css("display", "none");
            Crow.find("#subItemQty").css("border-color", "#ced4da");
            var subItemTyp = Crow.find("#subItemType").val(SubItmType);
            var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr').length;
            var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr');

            /*if (rows.len > 0) {*/
            if (rows.length > 0) {
                rows.each(function () {
                    var InnerCrow = $(this).closest("tr");
                    debugger;
                    //var ItemId = InnerCrow.find("#ItemId").val();
                    //var subitm_typ = InnerCrow.find("#subItemType").val();
                    //if (subitm_typ == SubItmType) {
                    InnerCrow.find("#subItemQty").val(subItemQty);
                    InnerCrow.find("#subItemAvlQty").val(subItemAvlQty);
                    /* }*/

                });
            } else {

                $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                            <td><input type="text" id="subItemAvlQty" value='${subItemAvlQty}'></td>
                            <td><input type="text" id="subItemType" value='${SubItmType}'></td>
                            <td><input type="text" id="subItemReqQty" value='${subItemReqQty}'></td>
                        </tr>`);

            }
        }
       

    });

}
function CheckValidations_forSubItems() {
    debugger;
    return HeaderCheckValidations_forSubItemsNonTable("ddl_ItemName", "txtReworkQuantity", "SubItemRewrkQty", "Y", "SubHedr");
}
function CheckValidations_forSubItemsRMDetail() {
    return CheckValidations_forSubItemsRM("TblReqMaterialDetail", "", "tblhd_RMItemId", "tblRM_ReqQuantity", "SubItemRMReqQty", "Y", "SubRM");
 }
function CheckValidations_forSubItemsCMDetail() {
    return CheckValidations_forSubItemsCM("TblConsumedMaterialDetail", "", "ConsumeMaterialId", "CM_ConsumedQuantity", "SubItemCMConsQty", "Y","SubCM");
}
function ResetWorningBorderColor() {
    debugger;
    var ErrFlg = "N";
    var RJOstatus = $("#hdRJO_Status").val();
    if (RJOstatus != "I") {
        var HSubItmFlag = $("#sub_itemHeadrFlag").val();
        //var ReqMtrlSubItmFlag = $("#sub_itemReqMtrFlag").val();

        if (HSubItmFlag == "RJO_RewrkQuantity") {
            HeaderCheckValidations_forSubItemsNonTable("ddl_ItemName", "txtReworkQuantity", "SubItemRewrkQty", "N", "SubHedr");
            var RJORMTbllen = $("#TblReqMaterialDetail>tbody>tr").length;
            if (RJORMTbllen > 0) {
                return CheckValidations_forSubItemsRM("TblReqMaterialDetail", "", "tblhd_RMItemId", "tblRM_ReqQuantity", "SubItemRMReqQty", "N", "SubRM")
            }
        }
        //if (ReqMtrlSubItmFlag == "RJO_RMReqQty") {
        //    return Cmn_CheckValidations_forSubItems("TblReqMaterialDetail", "SNohiddenfiled", "tblhd_RMItemId", "tblRM_ReqQuantity", "SubItemRequiredQty", "N");
        //}
        //if (ConsMtrlSubItmFlag == "CM_ConsumeQty") {
        //    return Cmn_CheckValidations_forSubItems("TblConsumedMaterialDetail", "CM_SNohiddenfiled", "ConsumeMaterialId", "CM_ConsumedQuantity", "SubItemCMConsQty", "N");
        //}
        debugger;
        //return  CheckValidations_forSubItemsRM("TblReqMaterialDetail", "", "tblhd_RMItemId", "tblRM_ReqQuantity", "SubItemRequiredQty", "N", "SubRM")
           
        
    }
    else {
        return CheckValidations_forSubItemsCM("TblConsumedMaterialDetail", "", "ConsumeMaterialId", "CM_ConsumedQuantity", "SubItemCMConsQty", "N", "SubCM")
            
    }
    
    //if (ErrFlg == "Y") {
    //    return false
    //}
    //else {
    //    return true
    //}

}

function RJO_SubItemList() {
    var NewArr = new Array();
    debugger;
    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        var Qty = row.find('#subItemQty').val();
        debugger;
        if (parseFloat(CheckNullNumber(Qty)) >0) {
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.qty = row.find('#subItemQty').val();
        List.avl_qty = row.find('#subItemAvlQty').val();
            List.subItemTyp = row.find('#subItemType').val();
            List.req_qty = row.find('#subItemReqQty').val();
        NewArr.push(List);
        }
    });
    return NewArr;
}
function HeaderCheckValidations_forSubItemsNonTable(Item_field_id, Item_Qty_field_id, Button_id, ShowMessage, SubHedr) {
    debugger;
    var flag = "N";
    var item_id = $("#" + Item_field_id).val();
    var item_PrdQty = $("#" + Item_Qty_field_id).val();
    if (item_PrdQty == null || item_PrdQty == "") {
        item_PrdQty = $("#" + Item_Qty_field_id).text();
    }
    var sub_item = $("#sub_item").val();
    var Sub_Quantity = 0;
    var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubHedr + "']").closest('tr');
    rows.each(function () {
   /* $("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr').each(function () {*/
        var Crow = $(this).closest("tr");
        //var subItemId = Crow.find("#subItemId").val();
        var subItemQty = Crow.find("#subItemQty").val();
        Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
    });
    if (sub_item == "Y") {
        if (item_PrdQty != Sub_Quantity) {
            flag = "Y";
            $("#" + Button_id).css("border", "1px solid red");
        } else {
            $("#" + Button_id).css("border", "");
        }
    }
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
function CheckValidations_forSubItemsCM(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage, SubCM) {
    debugger;
    var docid = $("#DocumentMenuId").val();
    /*var docSrcTypid = $("#hdSrcTyp").val();*/

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
        var sub_item = PPItemRow.find("#sub_item").val();
        var Sub_Quantity = 0;
        //var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').find("#subItemType[value='" + SubCM + "']").closest('tr');
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubCM + "']").closest('tr');

       
        rows.each(function () {
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
function CheckValidations_forSubItemsRM(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage, SubRM) {
    debugger;
    var docid = $("#DocumentMenuId").val();
    /*var docSrcTypid = $("#hdSrcTyp").val();*/

    var flag = "N";
    $("#" + Main_table + " tbody tr").each(function () {
        var PPItemRow = $(this);
        var item_id;
        if (Row_Id != "" && Row_Id != null) {
            var Sno = PPItemRow.find("#" + Row_Id).val();
            item_id = PPItemRow.find("#" + Item_field_id + Sno).val();
        }
        else {
            item_id = PPItemRow.find("#" + Item_field_id).val();
        }

        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
        var sub_item = PPItemRow.find("#sub_item").val();
        var Sub_Quantity = 0;
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubRM + "']").closest('tr');
        

        rows.each(function () {
            var Crow = $(this).closest("tr");
            //var subItemId = Crow.find("#subItemId").val();
            var subItemQty = Crow.find("#subItemQty").val();
            Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
        });
        

        if (sub_item == "Y") {
            if (item_PrdQty != Sub_Quantity) {
                flag = "Y";
               //flag = "N";
                PPItemRow.find("#" + SubItemButton).css("border", "1px solid red");
               // PPItemRow.find("#" + SubItemButton).css("border", "");
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
function DeleteRMSubItemQtyDetail(item_id,SubItmTyp) {
    debugger;
    if (item_id != null && item_id != "") {
        if ($("#hdn_Sub_ItemDetailTbl >tbody >tr").length > 0) {
            if ($("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").length > 0) {
                $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').remove();
            }
            //if ($("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubRM + "']").closest('tr').length > 0) {
            //  $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubRM + "']").closest('tr').remove();
            //}
        } 

    }

}
function DeleteCMSubItemQtyDetail(item_id, SubItmTyp) {
    debugger;
    if (item_id != null && item_id != "") {
        if ($("#hdn_Sub_ItemDetailTbl >tbody >tr").length > 0) {
            if ($("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").length > 0) {
                $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').remove();
            }
            //if ($("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubRM + "']").closest('tr').length > 0) {
            //  $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubRM + "']").closest('tr').remove();
            //}
        }

    }

}
/***--------------------------------Sub Item Section End-----------------------------------------***/

function approveonclick() { /**Added this Condition by Nitesh 11-01-2024 for Disable Approve btn after one Click**/
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