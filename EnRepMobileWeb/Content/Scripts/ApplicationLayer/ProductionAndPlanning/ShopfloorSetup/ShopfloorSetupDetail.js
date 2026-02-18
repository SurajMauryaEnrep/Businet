
$(document).ready(function () {
    //$("#shfl_name").prop("disabled", false);
   
    BindDLLSOItemList();
    
    $("#divUpdate").hide();
    $("#ShopFloorTbody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {

            var clickedrow = $(e.target).closest("tr");
            var br_id = clickedrow.children("#tbl_br_id").text();
            var shfl_id = clickedrow.children("#tbl_shfl_id").text();                   
            window.location = "/ApplicationLayer/ShopfloorSetup/dbClickEdit/?br_id=" + br_id + "&shfl_id=" + shfl_id;
            
            enableProductionCap();
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
   
});

function validateSFDetailInsert() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    //$("#shfl_name").prop("disabled", false)
    var op_name = $("#shfl_name").val();
   
    var ValidationFlag = true;
    if (op_name == '') {
        document.getElementById("vmshfl_name").innerHTML = $("#valueReq").text();
        $("#shfl_name").css("border-color", "red");
        ValidationFlag = false;
    }
    //ValidationFlag = ValidationPro_cap();      //// Commented By Nitesh 12-10-2023 for not mandatory
    //if (ValidationFlag == false) {
    //    return false
    //}
    //if ($("#tblProdCap >tbody >tr").length == "0") {
    //    debugger;
    //    // var currentRow = $(this);
    //    swal("", $("#noitemselectedmsg").text(), "warning");
    //    return false;

    //}
    if (ValidationFlag == true) {
        InsertItemDetail();
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }
    else {
        return false;
    }
  
}
//function ValidationPro_cap() {        //// Commented By Nitesh 12-10-2023 for not mandatory
//    debugger;
//    var flag = 'N';
//    if ($("#tblProdCap >tbody >tr").length == "0") {
//        debugger;
//        var itemName = $("#ddl_ItemName option:selected").text();
//        var Itm_ID = $("#ddl_ItemName").val();

//        var UOMName = $("#UOM").val();
//        var UOMID = $("#UOMID").val();
//        var OptimumQuantity = $("#OptimumQuantity").val();
//        var PerUnitName = $("#PerUnit option:selected").text();
//        var PerUnit_Val = $("#PerUnit").val();
//        if (Itm_ID != "0") {
//            $("#spanddl_ItemName").css("display", "none");
//            $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
//        }
//        else {
//            $('#spanddl_ItemName').text($("#valueReq").text());
//            $("#spanddl_ItemName").css("display", "block");
//            $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
//            flag = 'Y';
//        }
//        if (OptimumQuantity != "" && OptimumQuantity != "0") {
//            $("#spanOptmQty").css("display", "none");
//            $("#OptimumQuantity").css("border-color", "#ced4da");
//        }
//        else {
//            $('#spanOptmQty').text($("#valueReq").text());
//            $("#spanOptmQty").css("display", "block");
//            $("#OptimumQuantity").css("border-color", "red");
//            flag = 'Y';
//        }
//        if (PerUnit_Val != "0") {
//            $("#spanPerUnit").css("display", "none");
//            $("#PerUnit").css("border-color", "#ced4da");
//        }
//        else {
//            $('#spanPerUnit').text($("#valueReq").text());
//            $("#spanPerUnit").css("display", "block");
//            $("#PerUnit").css("border-color", "red");
//            flag = 'Y';
//        }
//    }
//    if (flag == 'Y') {
//        return false;
//    }
//    else {
//        return true;
//    }
   
//}
function InsertItemDetail() {
    debugger;
  
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
        debugger;
        FinalItemDetail = InsertItemAttributeDetails();
        debugger;
        var ItemAttrDt = JSON.stringify(FinalItemDetail);
        $('#hdnshopfloorattrdetailsList').val(ItemAttrDt);
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
      /*----- Attatchment End--------*/
        /*----- Attatchment start--------*/
        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
      /*----- Attatchment End--------*/

        return true;
    }
    else {
        //alert("Check network");
        return false;
    }
};
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
    $("#tblProdCap >tbody >tr").each(function () {

        var currentRow = $(this);
        var item_id = currentRow.find("#item_id").text();//tbl_hdn_Itm_ID
        var uom_id = currentRow.find("#uom_id").text();
        var optm_qty = currentRow.find("#optm_qty").text();
        var per_unit = currentRow.find("#tbl_hdn_PerUnit_Val").val();
        if (per_unit == "" || per_unit == null) {
            var perunit = currentRow.find("#per_units").text();
            per_unit = perunit[0];
            currentRow.find("#tbl_hdn_PerUnit_Val").val(per_unit);
        }
        var item_name = currentRow.find("#item_name").text();
        var uom_alias = currentRow.find("#uom_alias").text();
        var tbl_hdn_PerUnit_Val = currentRow.find("#per_units").text();
        if (item_id != '') {
            AttrList.push({
                item_id: item_id, uom_id: uom_id, optm_qty: optm_qty, per_unit: per_unit, item_name: item_name,
                uom_alias: uom_alias, tbl_hdn_PerUnit_Val: tbl_hdn_PerUnit_Val
            })
        }
    });

    return AttrList;
    
};
function OnChangeopname() {
    debugger;
    var op_name = $('#shfl_name').val().trim();
    if (op_name != "") {
        document.getElementById("vmshfl_name").innerHTML = "";
        $("#shfl_name").css("border-color", "#ced4da");
        enableProductionCap();
    }
    else {
        document.getElementById("vmshfl_name").innerHTML = $("#valueReq").text();
        $("#shfl_name").css("border-color", "red");
        disableProductionCap();
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
function BindDLLSOItemList() {
    debugger;
    BindItemList("#ddl_ItemName", "", "#tblProdCap", "#tbl_hdn_Itm_ID", "listToHide", "ShflSetup");
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/ShopfloorSetup/GetSOItemList",
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
    //                    sessionStorage.removeItem("PLitemList");
    //                    sessionStorage.setItem("PLitemList", JSON.stringify(arr.Table));
    //                    $('#ddl_ItemName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $('#ddl_ItemName').select2({
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

    //                    $("#tblProdCap >tbody >tr").each(function () {
    //                        debugger;
    //                        var currentRow = $(this);
    //                        var ID = currentRow.find("td:eq(12)").text();//tbl_hdn_Itm_ID
                            
    //                        if (ID != '') {
    //                            $("#ddl_ItemName option[value=" + ID + "]").select2().hide();
    //                        }
    //                    });
    //                }
    //            }
    //        },
    //    });
}

function ddl_ItemName_onchange(e) {
   
    $("#spanddl_ItemName").css("display", "none");
    $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
    var Itm_ID = $("#ddl_ItemName").val();
    Cmn_BindUOM(e, Itm_ID, "", "Y","");
    //try {
    //    $.ajax(
    //        {
    //            type: "POST",
    //            url: "/ApplicationLayer/ShopfloorSetup/GetSOItemUOM",
    //            data: {
    //                Itm_ID: Itm_ID
    //            },
    //            success: function (data) {
    //                debugger;
                 
    //                if (data !== null && data !== "" && data != "ErrorPage") {
    //                    var arr = [];
    //                    arr = JSON.parse(data);
    //                    if (arr.Table.length > 0) {
    //                        $("#UOM").val(arr.Table[0].uom_alias);
                 
    //                        $("#UOMID").val(arr.Table[0].uom_id);
    //                    }
    //                    else {
    //                        $("#UOM").val("");
    //                        $("#UOMID").val("");
                            

    //                    }
    //                }
    //                else {
    //                    $("#UOM").val("");
    //                    $("#UOMID").val('');
                        

    //                }
    //            },
    //        });
    //} catch (err) {
    //}
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
        <td id="per_units">${PerUnitName}</td>
        <td style="display: none;"><input type="hidden" id="tbl_hdn_PerUnit_Val" value="${PerUnit_Val}" /></td> 
        <td style="display: none;"><input type="hidden" id="tbl_hdn_sno" value="${RowNo}" /></td> 
        <td id="item_id" class=" " hidden="hidden">${Itm_ID}</td>
        <td id="uom_id" class=" " hidden="hidden">${UOMID}</td>
</tr>`);
    // Comented BY Nitesh 13102023 1015 for this column add two times
       // <td id="per_unit_val" class=" " hidden="hidden">${PerUnit_Val}</td> 
        
    debugger;
    $('#ddl_ItemName').val("0").change();
    $("#UOM").val('');
    $("#OptimumQuantity").val('');
    $('#PerUnit').val("0").prop('selected', true);
    HideItemListItm(Itm_ID);
   
};

function editRow(el,e) {
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
        if (ddlPerUnit == "" || ddlPerUnit == null) {
            var perunit = clickedrow.find("#per_units").text();
            ddlPerUnit = perunit[0];
            clickedrow.find("#tbl_hdn_PerUnit_Val").val(ddlPerUnit);
        }
            $('#PerUnit').val(ddlPerUnit).change();
            
            $("#divAddNew").hide();
        $("#divUpdate").show();

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
        //table.rows[RowNo].cells[8].innerHTML = OptimumQuantity;
        var PerUnitName = $("#PerUnit option:selected").text();
       // var ddl_ItemName = $("#Textddl option:selected").val();
        var PerUnit_Val = $("#PerUnit").val();
        var OptimumQuantity = $("#OptimumQuantity").val();
       // $("#per_units").val(PerUnitName);
        // table.rows[RowNo].cells[9].innerHTML = PerUnitName;
       // $("#tbl_hdn_PerUnit_Val").val(PerUnit_Val);

        $("#tblProdCap tbody tr").each(function() {    //Added By Nitesh 13102023 1017 for update time is match column
            var row = $(this);
            var item = row.find("#tbl_hdn_Itm_ID").val();
            if (Itm_ID != item) {               
            }
            else {
                row.find("#per_units").text(PerUnitName);
                row.find("#tbl_hdn_PerUnit_Val").val(PerUnit_Val);
                row.find("#optm_qty").text(OptimumQuantity);
            }

        })
      
    
       // table.rows[RowNo].cells[14].innerHTML = PerUnit_Val;
       

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
function deleteRow(el, e) {
    debugger;
    var i = el.parentNode.parentNode.rowIndex;
    //alert(i);
    var clickedrow = $(e.target).closest("tr");
    var hdn_Itm_ID = clickedrow.find("#tbl_hdn_Itm_ID").val();
    document.getElementById("tblProdCap").deleteRow(i);
    SerialNoAfterDelete();
    ShowItemListItm(hdn_Itm_ID);
}
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#tblProdCap >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#count").text(SerialNo);

    });

};
function AmtFloatQtyonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit")) {
        return true;
    } else {
        return false;
    }
}
function HideItemListItm(ID) {
    debugger;
    $("#ddl_ItemName option[value=" + ID + "]").select2().hide();
}
function ShowItemListItm(ID) {
    debugger;
    $("#ddl_ItemName option[value=" + ID + "]").removeClass("select2-hidden-accessible");
    //$("#ddl_ItemName option[value=" + ID + "]").select2().show();
}
function onchangeOptimumQuantity(el, evnt) {
    debugger;
    
    var OptimumQuantity = $("#OptimumQuantity").val();
    if (OptimumQuantity == "") {
        $('#spanOptmQty').text($("#valueReq").text());
        $("#spanOptmQty").css("display", "block");
        $("#OptimumQuantity").css("border-color", "red");
    }
    else {        
        $("#spanOptmQty").css("display", "none");
        $("#OptimumQuantity").css("border-color", "#ced4da");
        return true;
    }
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

function disableProductionCap() {
    debugger;
    $("#ddl_ItemName").prop("disabled", true);
    $("#OptimumQuantity").prop("disabled", true);
    $("#PerUnit").prop("disabled", true);    
}
function enableProductionCap() {
    debugger;
   
    $("#ddl_ItemName").prop("disabled", false);
    $("#OptimumQuantity").prop("disabled", false);
    $("#PerUnit").prop("disabled", false);
}
function AmtFloatQtyonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
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

    return true;
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
