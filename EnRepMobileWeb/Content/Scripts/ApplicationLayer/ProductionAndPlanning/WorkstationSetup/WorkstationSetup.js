$(document).ready(function () {
    //var currentDate = moment().format('YYYY-MM-DD');
    //$("#txt_op_st_date").val(currentDate);
    $("#ddl_ShopfloorName").select2();
    $("#ddl_ShopfloorNameList").select2();
    $("#divUpdate").hide();
    BindDLLSOItemList();
    $("#WSTbody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var comp_id = clickedrow.children("#tbl_comp_id").text();
            var br_id = clickedrow.children("#tbl_br_id").text();
            var ws_id = clickedrow.children("#tbl_ws_id").text(); 
            window.location = "/ApplicationLayer/WorkstationSetup/dbClickEdit/?comp_id=" + comp_id + "&br_id=" + br_id + "&ws_id=" + ws_id + "&ListFilterData=" + ListFilterData;
        }
        catch (err) {
            debugger;
            alert(err.message);
        }
    });
    //GetGroupName();

    
    $("#OptimumQuantity").on("paste", function (e) {
        debugger;
        var pastedData = e.originalEvent.clipboardData.getData('text');
        if ($.isNumeric(pastedData)) {
            return true;
        }
        else {
            event.preventDefault();
            return false;
        }
           });
});
function BindDLLSOItemList() {   /*Added By Nitesh 06-10-2023 14:09 For Bind Data of item And Uom*/
    debugger;
    BindItemList("#ddl_ItemName", "", "#tblProdCap", "#tbl_hdn_Itm_ID", "listToHide", "ShflSetup");
}
function ddl_ItemName_onchange(e) {

    $("#spanddl_ItemName").css("display", "none");
    $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
    var Itm_ID = $("#ddl_ItemName").val();
    Cmn_BindUOM(e, Itm_ID, "", "Y", "");
}

function OnclickReqQty(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ReqQty = $("#OptimumQuantity").val();
    if (ReqQty == "" || ReqQty == "0" || ReqQty == parseFloat(0)) {/*Add by Hina on 02-03-2024 to show blank instaed of 0 in inserted field*/
        $("#spanOptmQty").text($("#valueReq").text());
        $("#spanOptmQty").css("display", "block");
        $("#OptimumQuantity").css("border-color", "red");
        $("#OptimumQuantity").val("");
        return false;
    }
    else {
        $("#spanOptmQty").css("display", "none");
        $("#OptimumQuantity").css("border-color", "#ced4da");
    }
    $("#OptimumQuantity").val(parseFloat(ReqQty).toFixed(QtyDecDigit));

}

function AmtFloatQtyonly(el, evt) {
    debugger;
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
}
function AddNewRow() {
    var rowIdx = 0;
    //debugger;
    var rowCount = $('#tblProdCap >tbody >tr').length + 1;
    var RowNo = 0;

    $("#tblProdCap >tbody >tr").each(function (i, row) {
        // debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    var itemName = $("#ddl_ItemName option:selected").text();
    var Itm_ID = $("#ddl_ItemName").val();

    var UOMName = $("#UOM").val();
    var UOMID = $("#UOMID").val();
    var OptimumQuantity = $("#OptimumQuantity").val();
    var PerUnitName = $("#PerUnit option:selected").text();
    var PerUnit_Val = $("#PerUnit").val();

    var flag = 'N';
    if (Itm_ID != "0") {
        $("#spanddl_ItemName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
    }
    else {
        $('#spanddl_ItemName').text($("#valueReq").text());
        $("#spanddl_ItemName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
        flag = 'Y';
    }
    if (OptimumQuantity != "" && OptimumQuantity != "0") {
        $("#spanOptmQty").css("display", "none");
        $("#OptimumQuantity").css("border-color", "#ced4da");
    }
    else {
        $('#spanOptmQty').text($("#valueReq").text());
        $("#spanOptmQty").css("display", "block");
        $("#OptimumQuantity").css("border-color", "red");
        flag = 'Y';
    }
    if (PerUnit_Val != "0") {
        $("#spanPerUnit").css("display", "none");
        $("#PerUnit").css("border-color", "#ced4da");
    }
    else {
        $('#spanPerUnit').text($("#valueReq").text());
        $("#spanPerUnit").css("display", "block");
        $("#PerUnit").css("border-color", "red");
        flag = 'Y';
    }
    if (flag == 'Y') {
        return false;
    }



    $('#tblProdCap tbody').append(`<tr>
       <td class="red center"> <i class="fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}" onclick="deleteRow(this,event)"></i></td>
        <td class="center edit_icon"> <i class="fa fa-edit editshopfloor" aria-hidden="true" id="editBtnIcon${RowNo}" aria-hidden="true" title="${$("#Edit").text()}" onclick="editRow(this,event)"></i></td>
        <td id="count">${RowNo}</td>
        <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
        <td id="item_name">${itemName}</td>
        <td style="display: none;"><input type="hidden" id="tbl_hdn_Itm_ID" name="tbl_hdn_Itm_ID" value="${Itm_ID}" /></td>
        <td id="uom_alias">${UOMName}</td>
        <td style="display: none;"><input type="hidden" id="tbl_hdn_UOM_ID" value="${UOMID}" /></td>
        <td id="optm_qty" class="num_right">${OptimumQuantity}</td>
        <td>${PerUnitName}</td>
        <td style="display: none;"><input type="hidden" id="tbl_hdn_PerUnit_Val" value="${PerUnit_Val}" /></td> 
        <td style="display: none;"><input type="hidden" id="tbl_hdn_sno" value="${RowNo}" /></td> 
        <td id="item_id" class=" " hidden="hidden">${Itm_ID}</td>
        <td id="uom_id" class=" " hidden="hidden">${UOMID}</td>
        <td id="per_unit_val" class=" " hidden="hidden">${PerUnit_Val}</td>
        </tr>`);
    debugger;
    $('#ddl_ItemName').val("0").change();
    $("#UOM").val('');
    $("#OptimumQuantity").val('');
    $('#PerUnit').val("0").prop('selected', true);
    /*HideItemListItm(Itm_ID);*/
    SerialNoAfterDelete();

};
function OnClickParaItemUpdateBtn(e) {
    debugger;

    var RowNo = $('#hdnUpdateInTable').val();
    if (RowNo != '') {

        var flag = 'N';
        var Itm_ID = $("#ddl_ItemName").val();
        var OptimumQuantity = $("#OptimumQuantity").val();
        var PerUnitName = $("#PerUnit option:selected").text();
        var PerUnit_Val = $("#PerUnit").val();

        var flag = 'N';
        if (Itm_ID != "0") {
            $("#spanddl_ItemName").css("display", "none");
            $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
        }
        else {
            $('#spanddl_ItemName').text($("#valueReq").text());
            $("#spanddl_ItemName").css("display", "block");
            $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
            flag = 'Y';
        }
        if (OptimumQuantity != "" && OptimumQuantity != "0") {
            $("#spanOptmQty").css("display", "none");
            $("#OptimumQuantity").css("border-color", "#ced4da");
        }
        else {
            $('#spanOptmQty').text($("#valueReq").text());
            $("#spanOptmQty").css("display", "block");
            $("#OptimumQuantity").css("border-color", "red");
            flag = 'Y';
        }
        if (PerUnit_Val != "0") {
            $("#spanPerUnit").css("display", "none");
            $("#PerUnit").css("border-color", "#ced4da");
        }
        else {
            $('#spanPerUnit').text($("#valueReq").text());
            $("#spanPerUnit").css("display", "block");
            $("#PerUnit").css("border-color", "red");
            flag = 'Y';
        }
        if (flag == 'Y') {
            return false;
        }
       
        var table = document.getElementById("tblProdCap"), rIndex;
        table.rows[RowNo].cells[8].innerHTML = OptimumQuantity;
        var PerUnitName = $("#PerUnit option:selected").text();
        var PerUnit_Val = $("#PerUnit").val();
        table.rows[RowNo].cells[9].innerHTML = PerUnitName;
        var ItemId = $("#ddl_ItemName option:selected").val();
        //$("#tblProdCap > tbody > tr #tbl_hdn_Itm_ID:contains(" + ItemId + ")").closest('tr').find("#tbl_hdn_PerUnit_Val").val(PerUnit_Val);
        $("#tblProdCap > tbody > tr").each(function () {
            debugger;
            var CurrRow = $(this);
            var ItemId1 = CurrRow.find("#tbl_hdn_Itm_ID").val();
            if (ItemId == ItemId1) {
                CurrRow.find("#tbl_hdn_PerUnit_Val").val(PerUnit_Val);
            }
        })
        //$("#tbl_hdn_PerUnit_Val").val(PerUnit_Val);
        table.rows[RowNo].cells[14].innerHTML = PerUnit_Val;


        $('#ddl_ItemName').val("0").change();
        $("#ddl_ItemName").prop("disabled", false);
        $("#UOM").val('');
        $("#OptimumQuantity").val('');
        $('#PerUnit').val("0").prop('selected', true);


        $("#divAddNew").show();
        $("#divUpdate").hide();
        SerialNoAfterDelete();

    }
}
function editRow(el, e) {
    debugger;
    var rowJavascript = el.parentNode.parentNode;
    try {
        var QtyDecDigit = $("#QtyDigit").text();
        var clickedrow = $(e.target).closest("tr");

        var hdn_Itm_ID = clickedrow.find("#tbl_hdn_Itm_ID").val();
        var hdn_Itm_Name = clickedrow.find("#item_name").text();
        $('#ddl_ItemName').append('<option value=' + hdn_Itm_ID + ' selected>' + hdn_Itm_Name + '</option>');
        var uom_ = clickedrow.find("#uom_alias").text();
        $("#UOM").val(uom_);
        var opt_qty = clickedrow.children("#optm_qty").text();
        var sno = clickedrow.children("#count").text();
        var hdn = clickedrow.find("#tbl_hdn_sno").val();
        $('#hdnUpdateInTable').val(rowJavascript.rowIndex);

        $("#OptimumQuantity").val(parseFloat(opt_qty).toFixed(QtyDecDigit));
        var ddlPerUnit = clickedrow.find("#tbl_hdn_PerUnit_Val").val();
        $('#PerUnit').val(ddlPerUnit).change();

        $("#divAddNew").hide();
        $("#divUpdate").show();
        $("#spanddl_ItemName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
        $("#ddl_ItemName").prop("disabled", true);
        if (opt_qty != "" && opt_qty != "0") {
            $("#spanOptmQty").css("display", "none");
            $("#OptimumQuantity").css("border-color", "#ced4da");
        }
        else {
            $('#spanOptmQty').text($("#valueReq").text());
            $("#spanOptmQty").css("display", "block");
            $("#OptimumQuantity").css("border-color", "red");
            flag = 'Y';
        }
    }
    catch (err) {
        debugger
        alert(err.message);
    }

}
function deleteRow(el, e) {
    debugger;
    var i = el.parentNode.parentNode.rowIndex;
    //alert(i);
    var clickedrow = $(e.target).closest("tr");
    var hdn_Itm_ID = clickedrow.find("#tbl_hdn_Itm_ID").val();
    document.getElementById("tblProdCap").deleteRow(i);
    $('#ddl_ItemName').val("0").change();
    $('#ddl_ItemName').prop('disabled', false);
    $("#UOM").val('');
    $("#OptimumQuantity").val('');
    $("#OptimumQuantity").css("border-color", "#ced4da");
    $("#spanOptmQty").css("display", "none");
    $("#PerUnit").css("border-color", "#ced4da");
    $("#spanPerUnit").css("display", "none");
    $('#PerUnit').val("0").prop('selected', true);
    $("#divAddNew").show();
    $("#divUpdate").hide();
    SerialNoAfterDelete();
    ShowItemListItm(hdn_Itm_ID);
}
function SerialNoAfterDelete() {
    debugger;
    var i = 0;
    $("#tblProdCap > tbody > tr").each(function () {
        var CurrRow = $(this);
        i = i + 1;
        CurrRow.find("#count").text(i);
    })
}
function ShowItemListItm(ID) {
    debugger;
    $("#ddl_ItemName option[value=" + ID + "]").removeClass("select2-hidden-accessible");
}
function onchangePerUnit() {
    debugger;

    var PerUnit = $("#PerUnit").val();
    if (PerUnit == "0") {
        $('#spanPerUnit').text($("#valueReq").text());
        $("#spanPerUnit").css("display", "block");
        $("#PerUnit").css("border-color", "red");
    }
    else {
        $("#spanPerUnit").css("display", "none");
        $("#PerUnit").css("border-color", "#ced4da");
    }
}
//function GetGroupName() {
//    $.ajax({
//        type:"POST",
//        url: $("#ajaxUrlGetAutoCompleteSearchSuggestion").val(),
//        data: function (params) {
//            var queryParameters = {
//                //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
//                ddlgroup_name: params.term, // search term like "a" then "an"
//                Group: params.page
//            };
//            return queryParameters;
//        },
//        success: function (data, params) {
//            if (data == 'ErrorPage') {
//                ErrorPage();
//                return false;
//            }
//            params.page = params.page || 1;
//            return {
//                //results:data.results,
//                results: $.map(data, function (val, item) {
//                    return { id: val.ID, text: val.Name };
//                })
//            };
//        }
//    });

//    $("#group_name").select2({
       
//    });
//}

function OnChangeopname() {
    debugger;
    var op_name = $('#txt_ws_name').val().trim();
    if (op_name != "") {
        document.getElementById("vm_txt_ws_name").innerHTML = "";
        $("#txt_ws_name").css("border-color", "#ced4da");
        
    }
    else {
        document.getElementById("vm_txt_ws_name").innerHTML = $("#valueReq").text();
        $("#txt_ws_name").css("border-color", "red");
        
    }    
}
function OnChange_op_st_date() {
    debugger;
    
    var txt_op_st_date = $('#txt_op_st_date').val().trim();
    if (txt_op_st_date != "") {
        document.getElementById("vm_op_st_date").innerHTML = "";
        $("#txt_op_st_date").css("border-color", "#ced4da");

    }
    else {
        document.getElementById("vm_op_st_date").innerHTML = $("#valueReq").text();
        $("#txt_op_st_date").css("border-color", "red");

    }
}
//function OnChange_spanGroupName() {
//    debugger;

//    var group_name = $('#group_name').val().trim();
//    if (group_name != "") {
//        document.getElementById("spanGroupName").innerHTML = "";
//        $("#group_name").css("border-color", "#ced4da");
//        $('[aria-labelledby="select2-group_name-container"]').css("border-color", "#ced4da");

//    }
//    else {
//        document.getElementById("spanGroupName").innerHTML = $("#valueReq").text();
//        $("#group_name").css("border-color", "red");
//        $('[aria-labelledby="select2-group_name-container"]').css("border-color", "red");

//    }
//}
function validateSFDetailInsert() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }


    var txt_ws_name = $("#txt_ws_name").val();
    var ddl_ShopfloorName = $("#ddl_ShopfloorName").val();
    var txt_op_st_date = $("#txt_op_st_date").val();
    //var group_name = $("#group_name").val();
    //alert('test validation');
    var ValidationFlag = true;
    if (txt_ws_name == '') {
        document.getElementById("vm_txt_ws_name").innerHTML = $("#valueReq").text();
        $("#txt_ws_name").css("border-color", "red");
        ValidationFlag = false;
    }
    if (ddl_ShopfloorName == '0') {
        //document.getElementById("vm_ddl_ShopfloorName").innerHTML = $("#valueReq").text();
        $("#spanddl_ShopfloorName").text($("#valueReq").text());
        $("#spanddl_ShopfloorName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ShopfloorName-container']").css("border-color", "red");
        ValidationFlag = false;
    }
    if (txt_op_st_date == '') {
        document.getElementById("vm_op_st_date").innerHTML = $("#valueReq").text();
        $("#txt_op_st_date").css("border-color", "red");
        ValidationFlag = false;
    }
    //if (group_name == '0') {
    //    document.getElementById("spanGroupName").innerHTML = $("#valueReq").text();
    //    $("#group_name").css("border-color", "red");
    //    $('[aria-labelledby="select2-group_name-container"]').css("border-color", "red");
    //    ValidationFlag = false;
    //}

    if (ValidationFlag == true) {
        /*----------WorkStation Capacity----------*/
        FinalItemDetail = InsertItemAttributeDetails();
        debugger;
        var ItemAttrDt = JSON.stringify(FinalItemDetail);
        $('#hdnWorkstationItemsetup').val(ItemAttrDt);

        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
        /*----- Attatchment End--------*/
        /*----- Attatchment start--------*/
        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
      /*----- Attatchment End--------*/
        //InsertItemDetail();
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }

    else {
        return false;
    }
}
function InsertItemAttributeDetails() {
    debugger;
    var AttributeList = [];
    var itemDTransType = sessionStorage.getItem("ItmDTransType");
    var itmcode = sessionStorage.getItem("EditItemCode");
    var TransType = '';
    if (itemDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var AttrList = [];
    $("#tblProdCap tr").each(function () {

        var currentRow = $(this);
        var item_id = currentRow.find("#item_id").text();//tbl_hdn_Itm_ID
        var item_name = currentRow.find("#item_name").text();//tbl_hdn_Itm_ID
        var uom_alias = currentRow.find("#uom_alias").text();//tbl_hdn_Itm_ID
        var uom_id = currentRow.find("#uom_id").text();
        var optm_qty = currentRow.find("#optm_qty").text();
        var per_unit = currentRow.find("#per_unit_val").text();
        if (item_id != '') {
            AttrList.push({ item_id: item_id, item_name: item_name, uom_alias: uom_alias, uom_id: uom_id, optm_qty: optm_qty, per_unit: per_unit, })
        }
    });

    return AttrList;

};
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
function OnChangeddlddlList() {
    debugger;
    //var ddlOpTypeList = $('#ddl_ShopfloorName').val().trim();
    //if (ddlOpTypeList != "0") {
    //    document.getElementById("vm_ddl_ShopfloorName").innerHTML = "";
    //    $("[aria-labelledby='select2-ddl_ShopfloorName-container']").css("border-color", "#ced4da");

    //}
    //else {
        //document.getElementById("vm_ddl_ShopfloorName").innerHTML = $("#valueReq").text();
        //$("#SpanAccGrp").css("display", "none");
    //$("[aria-labelledby='select2-ddl_ShopfloorName-container']").css("border-color", "red");

    $("#spanddl_ShopfloorName").css("display", "none");
    $("[aria-labelledby='select2-ddl_ShopfloorName-container']").css("border-color", "#ced4da")
    //}
}