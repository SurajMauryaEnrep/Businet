/************************************************
Javascript Name:Job Order SC Detail
Created By:Hina Sharma
Created Date: 21-12-2022
Description: This Javascript use for the JobOrder SC many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {

    
    debugger;
    JOOnClickSrcType();
    var hdnsrctyp = $("#Hdn_JOSrcType").val();
    if (hdnsrctyp == "Direct") {
        var ProductID = $("#hdn_Fproduct_id").val();
        var ProductName = $("#hdn_Fproduct_name").val();
        if (ProductID != "" && ProductName != "") {
            $('#ddl_FProductName').val(ProductID).trigger('change.select2');
            $('#ddl_FProductName').empty().append('<option value=' + ProductID + ' selected="selected">' + ProductName + '</option>');
        }
        else {
            BindProductNameDDL();
            $("#ddl_FProductName").attr('onchange', 'ddl_FProductName_onchange()');
        }
    }
    BindDDlJOList();
    var abc = $("#hfStatus").val();
    if ($("#hfStatus").val() == "D" || $("#hfStatus").val() == "") {
        debugger;
        BindJOItmListPageLoad(1);
    }
    if (hdnsrctyp == "Direct") {
        if ($("#hfStatus").val() == "D" ) {
            debugger;
            $("#JoInputItmDetailsTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRow = $(this);
                RowNo = currentRow.find("#JOInputSNohiddenfiled").val();// + 1;
                BindProductNameDDLInputItmDtl(RowNo)
            });
            
        }
    }
    
    /*commented on 11-04-2024 by Hina */
   // var JONum = $("#JobOrderNumber").val();
   // var Message = $("#hdn_massage").val();
   //if (Message == "DocModify") {

   // }
   // else {
   //     if (JONum == null || JONum == "") {
   //         debugger;
   //         BindProdOrderNumber();
   //     }
   // }
    var SuppName = $("#SupplierName").val();
    $("#Hdn_SupplierName").val(SuppName);
    var ProductName = $("#Prod_Ord_number").val();
    $("#Hdn_ProducNum").val(ProductName);
    var ProductName = $("#Prod_Ord_number option selected").text();
    $("#Hdn_ProducNum").val(ProductName);
    //var hedrPrdNo = $("#hdn_Fproduct_name").val();
    //$("#ddl_FProductName").val(hedrPrdNo);
    
    //BindJOItemList(e);
    $('#JoItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();

        debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";
        
        ItemCode = $(this).closest('tr').find("#JOItemListName" + SNo).val();
       

        
        CalculateAmount();
        debugger;
        var TOCAmount = parseFloat($("#JO_OtherCharges").val());
        if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
            TOCAmoun = 0;
        }
        //var len = $("#QTItmDetailsTbl >tbody >tr").length;
        //if (len == "0")
        //{
        //    $("#QT_OtherCharges").val()
        //}
        debugger;
        Calculate_OC_AmountItemWise(TOCAmount);
        SerialNoAfterDelete();
        AfterDeleteResetJO_ItemTaxDetail();
        debugger;
        BindOtherChargeDeatils();

    });
    $('#JoInputItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
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
        debugger;
        ItemCode = $(this).closest('tr').find("#JOInput_hfItemID").val();
        DeleteInputSubItemQtyDetail(ItemCode, "SubRM")
        SerialNoAfterDeleteInputItm();
    });
    
    $("#ItemDetailsTbl tbody").bind("click", function (e) {
        ////debugger;

        var clickedrow = $(e.target).closest("tr");
        if (clickedrow.find('#OrderTypeLocal').prop("disabled") == false) {
            $("#ItemDetailsTbl >tbody >tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");

            clickedrow.find('#OrderTypeLocal').prop("checked", true);
            var Address_id = clickedrow.find("#addhd").text();
            var Address = clickedrow.find("#cust_add").text();
            var pin = clickedrow.find("#cust_pin").text();
            var city = clickedrow.find("#city_name").text();
            var dist = clickedrow.find("#district_name").text();
            var state = clickedrow.find("#state_name").text();
            var country = clickedrow.find("#country_name").text();

            $('#hd_TxtBillingAddr').val(Address + ',' + ' ' + city + '-' + ' ' + pin + ',' + ' ' + dist + ',' + ' ' + state + ',' + ' ' + country);
            $("#hd_bill_add_id").val(Address_id);
        }
    });

    Cmn_DeleteDeliverySch();
    DeleteTermCondition();

    jo_no = $("#JobOrderNumber").val();
    $("#hdDoc_No").val(jo_no);
});

/*---------------------------In Direct Source Type Code Start (Add on 11-04-2024)----------------------- */
function JOOnClickSrcType() {
    debugger;
    //var SrcType;
    var SrcType = "PrdOrd";
    if ($("#SrcTypD").is(":checked")) {
        SrcType = "Direct"
        $("#divPrdOrdNo").css("display", "none");
        $("#divPrdOrdDt").css("display", "none");
       
        $("#divDDLFinishPrdct").css("display", "block");
        $("#TxtFinishedProduct").css("display", "none");
        $("#divOrdQty").css("display", "block");
        $("#divItmWeight").css("display", "block");
        
        $("#divOPName").css("display", "none");
        $("#SecOutputItmDtl").css("display", "none");
        $("#InpuItmDtlHedr_ItmTyp").css("display", "none");
        //BindProductNameDDL();
        //$("#ddl_FProductName").attr('onchange', 'ddl_FProductName_onchange()');
       
    //    $("#txt_JobQuantity").val("");
    //    $("#txt_BatchNumber").val("");
    //    $("#ddlAdviceNo").val("--- Select ---");
    //    $("#Txt_AdviceDate").val("");
    //    $("#txt_JobQuantity").prop("readonly", false);
    //    $("#txt_BatchNumber").prop("readonly", false);
    //    var ProductID = 0;
    //    var ProductName = '---Select---';

    //    $('#ddl_ProductName').val(ProductID).trigger('change.select2');
    //    $('#ddl_ProductName').empty().append('<option value=' + ProductID + ' selected="selected">' + ProductName + '</option>');
       
    }
    if ($("#SrcTypPrdOrd").is(":checked")) {
        SrcType = "PrdOrd"
        $("#divPrdOrdNo").css("display", "block");
        $("#divPrdOrdDt").css("display", "block");
        $("#divDDLFinishPrdct").css("display", "none");
        $("#TxtFinishedProduct").css("display", "block");
        $("#divOPName").css("display", "block");
        $("#divOrdQty").css("display", "none");
        $("#divItmWeight").css("display", "none");
        $("#SecOutputItmDtl").css("display", "block");
        $("#InpuItmDtlHedr_ItmTyp").css("display", "");
        $("#SrcTypD").attr("disabled", true);
        
        //$("#txt_JobQuantity").val("");block
        //$("#txt_JobQuantity").prop("readonly", true);
        var JONum = $("#JobOrderNumber").val();
        var Message = $("#hdn_massage").val();
        if (Message == "DocModify") {

        }
        else {
            if (JONum == null || JONum == "") {
                debugger;
                BindProdOrderNumber();
            }
        }
    }

    debugger;
    $("#Hdn_JOSrcType").val(SrcType);
}
function BindProductNameDDL() {
    debugger;
    var Itm_ID = $("#ddl_FProductName").val();
    if (Itm_ID == null) {
        BindItemList("#ddl_FProductName", "", "", "", "", "JOHedr");
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
function ddl_FProductName_onchange() {
    debugger;
    var Itm_ID = $("#ddl_FProductName").val();
    var Itm_Name = $("#ddl_FProductName option:selected").text();
    $("#hdn_Fproduct_name").val(Itm_Name);
    $("#hdn_Fproduct_id").val(Itm_ID);
    if (Itm_ID != "0") {
        $("#SrcTypPrdOrd").attr("disabled",true)
        $("#vmddl_FProductName").css("display", "none");
        $("[aria-labelledby='select2-ddl_FProductName-container']").css("border-color", "#ced4da");

        /*change for Product name to reflect in Delivery schedule item table */
        
        var OrdQty = $("#JOOrdQuantity").val();
        if (OrdQty != "" && OrdQty != null && OrdQty != "0") {
            $("#DeliverySchItemDDL").empty();
            $("#DeliverySchQty").val("");
            var delschlen = $("#DeliverySchTble >tbody >tr").length;
            if (delschlen > 0) {
                $("#DeliverySchTble >tbody >tr").remove();
            }
            BindDelivertSchItemList_ForHeadrItm();
        }
        
    }
    else {
        $("#TxtFinishedUom").val("");
        $("#JOOrdQuantity").val("");
        $("#JOOrdQuantity").prop('readonly', true);
        $('#vmddl_FProductName').text($("#valueReq").text());
        $("#vmddl_FProductName").css("display", "block");
        /*change for Product name to reflect in Delivery schedule item table */
        $("#DeliverySchItemDDL").empty();
        $('#DeliverySchItemDDL').append(`<option value="0">---Select---</option>`);
        $("#DeliverySchQty").val("");
        var delschlen = $("#DeliverySchTble >tbody >tr").length;
        if (delschlen > 0) {
            $("#DeliverySchTble >tbody >tr").remove();
        }
        //$('#ddl_OperationName').empty();
        //$('#ddl_OperationName').append(`<option value="0">---Select---</option>`);
    }

    if (Itm_ID != null && Itm_ID != "0") {
        $("#hdn_product_id").val(Itm_ID);
    }
    //Cmn_BindUOM("", Itm_ID, "", "Y", "JOHdr");
    debugger;
    if (Itm_ID != null && Itm_ID != "" && Itm_ID != "0") {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/JobOrderSC/GetJOHedrItemUOM",
                    data: {
                        Itm_ID: Itm_ID
                    },
                    success: function (data) {
                        debugger;
                        $("#JOOrdQuantity").prop("readonly",false);
                        if (data !== null && data !== "" && data != "ErrorPage") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.length > 0) {
                                $("#TxtFinishedUom").val(arr[0].uom_alias);
                                $("#Hdn_FinishUomId").val(arr[0].uom_id);
                                $("#Hdn_ItmWeight").val(arr[0].nt_wght);
                                $("#sub_item").val(arr[0].sub_item);
                                $("#sub_itemHeadr").val(arr[0].sub_item);
                                HideShowPageWise(arr[0].sub_item, "NoRow");
                                //$("#UOMID").val(arr.Table[0].uom_id);
                            }
                            else {
                                $("#TxtFinishedUom").val("");
                                $("#Hdn_ItmWeight").val("");
                                $("#Hdn_FinishUomId").val("");
                                //$("#UOMID").val("");
                            }
                        }
                        else {
                            $("#TxtFinishedUom").val("");
                            $("#Hdn_ItmWeight").val("");
                            $("#Hdn_FinishUomId").val("");
                            //$("#UOMID").val('');

                        }
                    },
                });

        } catch (err) {
        }
    }
    
}
function OnChngOrdQty() {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    ReqQty = $("#JOOrdQuantity").val();
    RequiredQty = parseFloat($("#JOOrdQuantity").val()).toFixed(QtyDecDigit);
    if (AvoidDot(RequiredQty) == false) {
        RequiredQty = 0;
    }
   if (ReqQty == "" || ReqQty == "0" || ReqQty == parseFloat(0)) {
       $("#JOOrdQuantity").val("");
       $("#SpanJOOrdQtyErrorMsg").text($("#valueReq").text());
       $("#SpanJOOrdQtyErrorMsg").css("display", "block");
       $("#JOOrdQuantity").css("border-color", "red");
        ErrorFlag = "Y";
    }
   else {
       parseFloat($("#JOOrdQuantity").val(RequiredQty)).toFixed(QtyDecDigit);
       $("#SpanJOOrdQtyErrorMsg").css("display", "none");
       $("#JOOrdQuantity").css("border-color", "#ced4da");
       var itmwght = $("#Hdn_ItmWeight").val();
       if (itmwght == "") {
           itmwght = 0;
       }
       var joordqty = $("#JOOrdQuantity").val()
       var totalwght = parseFloat(0).toFixed(QtyDecDigit)
       var itmwgt = $("#JOItmWeight").val();
       if (itmwgt == "") {
           totalwght = parseFloat(joordqty).toFixed(QtyDecDigit) * parseFloat(itmwght).toFixed(QtyDecDigit);
           $("#JOItmWeight").val(parseFloat(totalwght).toFixed(QtyDecDigit));
           $("#SpanJOItmWghtErrorMsg").css("display", "none");
           $("#JOItmWeight").css("border-color", "#ced4da");
       }
       var hedrItm = $("#hdn_Fproduct_id").val();
       var joInputitm = $("#JoInputItmDetailsTbl >tbody >tr").length;
       if (joInputitm > 0) {
           $("#JoInputItmDetailsTbl >tbody >tr").each(function (i, row) {
               /*var clickedrow = $(e.target).closest("tr");*/
               var currentRow = $(this);
               var ItemID = currentRow.find("#JOInput_hfItemID").val();
               var Itm_ID;
               var SNo = currentRow.find("#JOInputSNohiddenfiled").val();

               Itm_ID = currentRow.find("#JOInputItemListName" + SNo).val();
               currentRow.find("#JOInput_hfItemID").val(Itm_ID);
               if (Itm_ID == hedrItm) {
                   currentRow.find("#JOInputRequiredQty").val(joordqty);
               }

           });
       }
       EnableItemDetailsJO();
       var hdCommand = $("#hdCommand").val();
       if (hdCommand != "Edit") {
           $("#JoItmDetailsTbl .plus_icon1").css("display", "block");
           $("#JoInputItmDetailsTbl .plus_icon1").css("display", "block");
          /* $("#JoInputItmDetailsTbl .required").css("display", "block");*/
       }
       

       /*change for Job Ord qty to reflect in Service item table */
       var srvcOrdQty = $("#JOOrdQuantity").val();
       var joservitm = $("#JoItmDetailsTbl >tbody >tr").length;
       if (joservitm > 0) {
           $("#JoItmDetailsTbl >tbody >tr").each(function (i, row) {
               var currentRow = $(this);
               debugger;
               RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
               currentRow.find("#OrdQtyServc").val(srvcOrdQty);
               
            });
       }
       debugger;
       /*change for Job Ord qty to reflect in Delivery schedule item table */
       var delschlen = $("#DeliverySchTble >tbody >tr").length;
       if (delschlen > 0) {
           $("#DeliverySchItemDDL").empty();
           $('#DeliverySchItemDDL').append(`<option value="0">---Select---</option>`);
           $("#DeliverySchQty").val("");
           $("#DeliverySchTble >tbody >tr").remove();
       }
       BindDelivertSchItemList_ForHeadrItm();
    }
    //if (ErrorFlag == "Y") {
    //    return false;
    //}
    //else {
        //EnableItemDetailsJO(); 
        //$("#JoItmDetailsTbl .plus_icon1").css("display", "block");
        //$("#JoInputItmDetailsTbl .plus_icon1").css("display", "block");
        //$("#JoInputItmDetailsTbl .required").css("display", "block");
        //var srvcOrdQty = $("#JOOrdQuantity").val();
        //$("#OrdQtyServc").val(srvcOrdQty);
        //$("#Reqitm").css("display", "block"); 
    //    return true;
    //}
    
}
function OnChngItmWeight() {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    ItmWgt = $("#JOItmWeight").val();
    ItmWeight = parseFloat($("#JOItmWeight").val()).toFixed(QtyDecDigit);
    if (AvoidDot(ItmWeight) == false) {
        ItmWeight = 0;
    }
    if (ItmWgt == "" || ItmWgt == "0" || ItmWgt == parseFloat(0)) {
        $("#JOItmWeight").val("");
        $("#SpanJOItmWghtErrorMsg").text($("#valueReq").text());
        $("#SpanJOItmWghtErrorMsg").css("display", "block");
        $("#JOItmWeight").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        parseFloat($("#JOItmWeight").val(ItmWeight)).toFixed(QtyDecDigit);
        $("#SpanJOItmWghtErrorMsg").css("display", "none");
        $("#JOItmWeight").css("border-color", "#ced4da");
    }
    //if (ErrorFlag == "Y") {
    //    return false;
    //}
    //else {
    //EnableItemDetailsJO(); 
    //$("#JoItmDetailsTbl .plus_icon1").css("display", "block");
    //$("#JoInputItmDetailsTbl .plus_icon1").css("display", "block");
    //$("#JoInputItmDetailsTbl .required").css("display", "block");
    //var srvcOrdQty = $("#JOOrdQuantity").val();
    //$("#OrdQtyServc").val(srvcOrdQty);
    //$("#Reqitm").css("display", "block"); 
    //    return true;
    //}

}
/*---------------------------In Direct Source Type Code End----------------------- */
/*---------------------------In Direct Source Type Input item detail Code End----------------------- */

function AddNewRowInputItmDtl() {
    debugger;
    var rowIdx = 0;

    var ItemInfo = $("#ItmInfo").text();
    var Span_Delete_Title = $("#Span_Delete_Title").text();
    debugger;

    var rowCount = $('#JoInputItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    rowIdx = rowCount - 1
    var levels = [];
    $("#JoInputItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#JOInputSNohiddenfiled").val());// + 1;
        levels.push(RowNo);
    });
    if (levels.length > 0) {
        RowNo = Math.max(...levels);
    }
    RowNo = RowNo + 1;
    if (RowNo == "0") {
        RowNo = 1;
    }

    debugger;
    $('#JoInputItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="JOInputdelBtnIcon${RowNo}" title="${Span_Delete_Title}"></i></td>
<td id="JOInputSRNO" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input  type="hidden" id="JOInputSNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"><input  type="hidden" id="JOInput_hfItemID" /></td>
<td style="display: none;"><input  type="hidden" id="JOInput_hfItemName" /></td>
<td>
<div class="col-sm-11 lpo_form no-padding">
<select class="form-control" id="JOInputItemListName${RowNo}" name="JOInputItemListName" onchange ="ddl_InputItemName_onchange(${RowNo},event)"></select>
<span id="JOInputItemListNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator ItmInfoBtnIcon" onclick="OnClickReqItemIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button>
</div>
</td>
<td><input id="JOInputUOM" class="form-control date" autocomplete="off" type="text" name="JOInputUOM"  placeholder="${$("#ItemUOM").text()}" disabled>
<input id="JOInputUOMID" type="hidden" />
</td>

<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="JOInputRequiredQty" class="form-control num_right" onpaste="return CopyPasteData(event)" onkeypress="return OnKeyPressReqQty(this,event);" onchange="OnChngReqQty(this,event)" autocomplete="off" type="text" name="InputRequiredQty" placeholder="0000.00">
<span id="JOInputRequiredQtyError" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemJOReqQty" >
   <input hidden type="text" id="JoInsub_item" value="" />
   <button type="button" id="SubItemJOReqQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('JOReqQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$(" #span_SubItemDetail").text()}"> </button>
 </div>
</td>

</tr>`);
    
    BindProductNameDDLInputItmDtl(RowNo)
};
function BindProductNameDDLInputItmDtl(ID) {
    debugger;
    //BindItemList("#JOInputItemListName", RowNo, "#JoInputItmDetailsTbl", "#InputSNohiddenfiled", "", "JOHedr");
    var ItmDDLName = "#JOInputItemListName";
    var TableID = "#JoInputItmDetailsTbl";
    var SnoHiddenField = "#JOInputSNohiddenfiled";
    $(ItmDDLName + ID).append(`<optgroup class='def-cursor' id="Textddl1${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    $('#Textddl1' + ID).append(`<option data-uom="0" value="0">---Select---</option>`);
    DynamicSerchableItemDDL(TableID, ItmDDLName, ID, SnoHiddenField, "", "JOHedr")

    
}
function ddl_InputItemName_onchange(RowID, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#JOInput_hfItemID").val();
    var Itm_ID;
    var SNo = clickedrow.find("#JOInputSNohiddenfiled").val();

    Itm_ID = clickedrow.find("#JOInputItemListName" + SNo).val();
    clickedrow.find("#JOInput_hfItemID").val(Itm_ID);
    var Itm_Name = clickedrow.find("#JOInputItemListName" + SNo + " option:selected" ).text();
    clickedrow.find("#JOInput_hfItemName").val(Itm_Name);
    var HedrItm = $("#hdn_Fproduct_id").val();
    var HedrOrdQty = $("#JOOrdQuantity").val();
    debugger;
    if (Itm_ID != null && Itm_ID != "") {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/JobOrderSC/GetJOHedrItemUOM",
                    data: {
                        Itm_ID: Itm_ID
                    },
                    success: function (data) {
                        debugger;

                        if (data !== null && data !== "" && data != "ErrorPage") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.length > 0) {
                                clickedrow.find("#JOInputUOM").val(arr[0].uom_alias);
                                clickedrow.find("#JOInputUOMID").val(arr[0].uom_id);
                                clickedrow.find("#JoInsub_item").val(arr[0].sub_item);
                                if (HedrItm == Itm_ID) {
                                    clickedrow.find("#JOInputRequiredQty").val(HedrOrdQty);
                                    clickedrow.find("#JOInputItemListName" + SNo).attr("disabled", true);
                                    clickedrow.find("#JOInputRequiredQty").attr("disabled", true);
                                }
                                //else {
                                //    clickedrow.find("#JOInputRequiredQty").attr("disabled", false);
                                //}
                                var sub_item = arr[0].sub_item;
                                var flag = "JOInputReq";
                                HideShowPageWise(sub_item, clickedrow,flag)
                                //$("#UOMID").val(arr.Table[0].uom_id);
                            }
                            else {
                                clickedrow.find("#JOInputUOM").val("");
                                clickedrow.find("#JOInputUOMID").val("");
                                //$("#UOMID").val("");
                            }
                        }
                        else {
                            clickedrow.find("#JOInputUOM").val("");
                            clickedrow.find("#JOInputUOMID").val("");
                            //$("#UOMID").val('');

                        }
                    },
                });

        } catch (err) {
        }
    }
   if (Itm_ID == "0") {
       clickedrow.find("#JOInputItemListNameError").text($("#valueReq").text());
       clickedrow.find("#JOInputItemListNameError").css("display", "block");
       clickedrow.find("[aria-labelledby='select2-JOInputItemListName" + SNo + "-container']").css("border-color", "red");
    }
    else {
       clickedrow.find("#JOInputItemListNameError").css("display", "none");
       clickedrow.find("[aria-labelledby='select2-JOInputItemListName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    ClearRowInpuItmDetails(e, ItemID);
 }
function ClearRowInpuItmDetails(e, ItemID) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    clickedrow.find("#ItmInfoBtnIcon").val("");
    clickedrow.find("#JOInputUOM").val("");
    clickedrow.find("#JOInputRequiredQty").val("");
}
function OnChngReqQty(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity

    var clickedrow = $(evt.target).closest("tr");
    //var ItemType = clickedrow.find("#MaterialTypeID").val();
    var RequireQuantity = clickedrow.find("#JOInputRequiredQty").val();
    if ((RequireQuantity == "") || RequireQuantity == parseFloat(0) || (RequireQuantity == null)) {
        clickedrow.find("#JOInputRequiredQtyError").text($("#valueReq").text());
        clickedrow.find("#JOInputRequiredQtyError").css("display", "block");
        clickedrow.find("#JOInputRequiredQty").css("border-color", "red");
        clickedrow.find("#JOInputRequiredQty").val("");
        return false;
    }
    else {
        clickedrow.find("#JOInputRequiredQtyError").css("display", "none");
        clickedrow.find("#JOInputRequiredQty").css("border-color", "#ced4da");
        var reqqty = parseFloat(parseFloat(RequireQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#JOInputRequiredQty").val(reqqty);
    }
}
function OnKeyPressReqQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#JOInputRequiredQtyError").css("display", "none");
    clickedrow.find("#JOInputRequiredQty").css("border-color", "#ced4da");
    return true;
}
function OnClickReqItemIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#JOInput_hfItemID").val();
    ItemInfoBtnClick(ItmCode)

}
function SerialNoAfterDeleteInputItm() {
    debugger
    var SerialNo = 0;
    $("#JoInputItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#JOInputSRNO").text(SerialNo);


    });
};
/*---------------------------In Direct Source Type Input item detail Code End----------------------- */

//------------------Start Delivery Schedule Section For Input item Detail only for single Item using Direct Srctyp------------------//
function BindDelivertSchItemList_ForHeadrItm() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var delitem = $('#DeliverySchItemDDL').val();
    var PQItemQty1 = 0;
    var s = '<option value="0">---Select---</option>';
   
        debugger;
        var PQItemName = "";
        var PQItemQty = parseFloat(0);
        var PQItemID = "";
        
        PQItemName = $("#hdn_Fproduct_name").val();
        PQItemID = $("#hdn_Fproduct_id").val();
        PQItemQty = $("#JOOrdQuantity").val();
            
        if (delitem == PQItemID) {
            PQItemQty1 = parseFloat(PQItemQty1) + parseFloat(PQItemQty);
        }

        if (PQItemID != "0" && PQItemID != "" && PQItemQty > 0) {
            var Flag = "N";
            $(s).each(function () {
                if (this.value == PQItemID) {
                    Flag = "Y";
                }
            });
            if (Flag == "N") {
                s += '<option value="' + PQItemID + '" text="' + PQItemName + '">' + PQItemName + '</option>';
            }
        }
  
        $("#DeliverySchItemDDL").html(s);
        debugger;
       
        var DSItemID = "";
        var DSItemQty = parseFloat(0);
        
        if ($("#DeliverySchTble >tbody >tr").length > 0) {
            $("#DeliverySchTble >tbody >tr").each(function () {
                //debugger;
                var currentRowDel = $(this);
                var DSchItemID = currentRowDel.find("#Hd_ItemIdFrDS").text();
                var DSQty = currentRowDel.find("#delv_qty").text();

                if (PQItemID === DSchItemID) {
                    DSItemID = DSchItemID;
                    DSItemQty = (parseFloat(DSItemQty) + parseFloat(DSQty));
                }
            });
        }
        
        if (DSItemID === PQItemID && parseFloat(DSItemQty).toFixed(QtyDecDigit) === parseFloat(PQItemQty).toFixed(QtyDecDigit)) {
            $("#DeliverySchItemDDL option[value=" + DSItemID + "]").hide();
            $('#DeliverySchItemDDL').val("0").prop('selected', true);
        }
  
    var DSItemQty = 0;
    if ($("#DeliverySchTble >tbody >tr").length > 0) {
        $("#DeliverySchTble >tbody >tr").each(function () {
            debugger;
            var currentRowDel = $(this);
            var DSchItemID = currentRowDel.find("#Hd_ItemIdFrDS").text();
            var DSQty = currentRowDel.find("#delv_qty").text();

            if (delitem === DSchItemID) {
                // DSItemID = DSchItemID;
                DSItemQty = (parseFloat(DSItemQty) + parseFloat(DSQty));
            }
        });
    }
    if (PQItemQty1 > DSItemQty) {
        $('#DeliverySchItemDDL').val(delitem).prop('selected', true);
        $("#DeliverySchQty").val(parseFloat(PQItemQty1 - DSItemQty).toFixed(QtyDecDigit));
    } else {
        $("#DeliverySchQty").val(""/*parseFloat(0).toFixed(QtyDecDigit)*/);
    }

};

function OnChangeDelSchItm_ForDirect() {
    debugger;
    var DocMenuId = $("#DocumentMenuId").val();
    var QtyDecDigit = $("#QtyDigit").text();
    if ($('#DeliverySchItemDDL').val() == "0") {
        ValidInfo = "Y";
        $('#SpanDelSchItemName').text($("#valueReq").text());
        $("#SpanDelSchItemName").css("display", "block");
        $("#DeliverySchItemDDL").css("border-color", "red");
        $("#DeliveryDate").val("");
        $('#DeliverySchQty').val("");
        $("#SpanDeliSchDeliDate").css("display", "none");
        $("#DeliveryDate").css("border-color", "#ced4da");
        $("#DeliverySchQty").css("border-color", "#ced4da");
        $("#SpanDelSchItemQty").css("display", "none");
    }
    else {
        $("#SpanDelSchItemName").css("display", "none");
        $("#DeliverySchItemDDL").css("border-color", "#ced4da");
        var DelSchItem = $('#DeliverySchItemDDL').val();
        var POItmQtyF = parseFloat(0).toFixed(QtyDecDigit);
        var AllDelSchQty = parseFloat(0).toFixed(QtyDecDigit);
           
        var POItemQty = parseFloat(0).toFixed(QtyDecDigit);
        var POItemID = "";
            
            //POItemName = $("#hdn_Fproduct_name").val();
            POItemID = $("#hdn_Fproduct_id").val();
            POItemQty = $("#JOOrdQuantity").val();
           

            
            if (POItemID === DelSchItem) {
                POItmQtyF = parseFloat(POItmQtyF) + parseFloat(POItemQty);
            }
        
        $("#DeliverySchTble >tbody >tr").each(function () {
            debugger;
            var currentRowDS = $(this);
            var DelSchItemtbl = currentRowDS.find("#Hd_ItemIdFrDS").text();
            debugger;
            var DelSchQtyTbl = currentRowDS.find("#delv_qty").text();
            if (DelSchItem == DelSchItemtbl) {
                AllDelSchQty = (parseFloat(AllDelSchQty) + parseFloat(DelSchQtyTbl)).toFixed(QtyDecDigit);
            }
        });
        var Date12 = moment().format('YYYY-MM-DD');
        $("#DeliveryDate").val(Date12);
        $("#SpanDeliSchDeliDate").css("display", "none");
        $("#DeliveryDate").css("border-color", "#ced4da");
        $('#DeliverySchQty').val((parseFloat(POItmQtyF) - parseFloat(AllDelSchQty)).toFixed(QtyDecDigit));
        OnChangeDeliveryQty_ForDirect();
    }

}

function OnChangeDeliveryQty_ForDirect() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var DelQty = $('#DeliverySchQty').val();
    if (AvoidDot(DelQty) == false) {
        DelQty = "";
        $('#DeliverySchQty').val("");
    }
    if ($('#DeliverySchQty').val() == "0" || $('#DeliverySchQty').val() == "") {
        ValidInfo = "Y";
        $('#DeliverySchQty').css("border-color", "red");
        $('#SpanDelSchItemQty').text($("#valueReq").text());
        $("#SpanDelSchItemQty").css("display", "block");
    }
    else {
        $('#DeliverySchQty').css("border-color", "#ced4da");
        $("#SpanDelSchItemQty").css("display", "none");
        $('#DeliverySchQty').val(parseFloat(DelQty).toFixed(QtyDecDigit));
        if (CalculateDeliverySchQty_ForDirect() === false) {
            $('#DeliverySchQty').css("border-color", "red");
            $('#SpanDelSchItemQty').text($("#SchReqQtyEqualTo").text());
            $("#SpanDelSchItemQty").css("display", "block");
            //$('#DeliverySchQty').val("");
            return false;
        }
        else {
            $('#DeliverySchQty').css("border-color", "#ced4da");
            $("#SpanDelSchItemQty").css("display", "none");
        }
    }
}
function CalculateDeliverySchQty_ForDirect() {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();
    var DelSchItem = $('#DeliverySchItemDDL').val();
    var POItemQty = parseFloat(0).toFixed(QtyDecDigit);
    var DelSchQty = parseFloat($('#DeliverySchQty').val()).toFixed(QtyDecDigit);
    var AllDelSchQty = parseFloat(0).toFixed(QtyDecDigit);
   
        var POItemID = "";
        var ItemQty = parseFloat(0).toFixed(QtyDecDigit);

    //POItemName = $("#hdn_Fproduct_name").val();
    POItemID = $("#hdn_Fproduct_id").val();
    //POItemQty = $("#JOOrdQuantity").val();

        
    ItemQty = parseFloat($("#JOOrdQuantity").val()).toFixed(QtyDecDigit);
        
        if (POItemID === DelSchItem) {
            POItemQty = parseFloat(POItemQty) + parseFloat(ItemQty);
        }
   
    $("#DeliverySchTble >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var DelSchItemtbl = currentRow.find("#Hd_ItemIdFrDS").text();
        var DelSchQtyTbl = currentRow.find("#delv_qty").text();
        if (DelSchItem == DelSchItemtbl) {
            AllDelSchQty = (parseFloat(AllDelSchQty) + parseFloat(DelSchQtyTbl)).toFixed(QtyDecDigit);
        }
    });
    AllDelSchQty = (parseFloat(AllDelSchQty) + parseFloat(DelSchQty)).toFixed(QtyDecDigit);
    if (parseFloat(POItemQty) >= parseFloat(AllDelSchQty)) {
        return true;
    }
    else {
        return false;
    }
}
function OnClickAddDeliveryDetail_ForDirect() {
    var rowIdx = 0;
    debugger;
    // var QtyDecDigit = $("#QtyDigit").text();

    var ValidInfo = "N";
    if ($('#DeliverySchItemDDL').val() == "0") {
        ValidInfo = "Y";
        $('#SpanDelSchItemName').text($("#valueReq").text());
        $("#SpanDelSchItemName").css("display", "block");
        $("#DeliverySchItemDDL").css("border-color", "red");
    }
    else {
        $("#SpanDelSchItemName").css("display", "none");
        $("#DeliverySchItemDDL").css("border-color", "#ced4da");
    }
    if ($('#DeliverySchQty').val() == "0" || $('#DeliverySchQty').val() == "") {
        ValidInfo = "Y";
        $('#SpanDelSchItemQty').text($("#valueReq").text());
        $("#SpanDelSchItemQty").css("display", "block");
        $("#DeliverySchQty").css("border-color", "red");
    }
    else {
        $("#SpanDelSchItemQty").css("display", "none");
        $("#DeliverySchQty").css("border-color", "#ced4da");
    }
    var DelDate = $('#DeliveryDate').val();
    var someDate = new Date(DelDate);
    var Dyear = someDate.getUTCFullYear();
    var Maxyear = parseInt(moment().format("YYYY")) + 20;
    if (DelDate == "") {
        ValidInfo = "Y";
        $('#SpanDeliSchDeliDate').text($("#valueReq").text());
        $("#SpanDeliSchDeliDate").css("display", "block");
        $("#DeliveryDate").css("border-color", "red");
    }
    else {
        if (parseInt(Dyear) > parseInt(Maxyear)) {
            ValidInfo = "Y";
            $('#SpanDeliSchDeliDate').text($("#JC_InvalidDate").text());
            $("#SpanDeliSchDeliDate").css("display", "block");
            $("#DeliveryDate").css("border-color", "red");
        } else {
            $("#SpanDeliSchDeliDate").css("display", "none");
            $("#DeliveryDate").css("border-color", "#ced4da");
        }

    }
    var DelItem = $('#DeliverySchItemDDL').val();
    $("#DeliverySchTble >tbody >tr #Hd_ItemIdFrDS:contains(" + DelItem + ")").parent().each(function () {
        debugger;
        var currentRowDS = $(this);
        var DelSchItemtbl = currentRowDS.find("#Hd_ItemIdFrDS").text();
        var sch_date = currentRowDS.find("#sch_date").text();
        if (DelItem == DelSchItemtbl && sch_date == DelDate) {
            ValidInfo = "Y";
            $('#SpanDeliSchDeliDate').text($("#valueduplicate").text());
            $("#SpanDeliSchDeliDate").css("display", "block");
            $("#DeliveryDate").css("border-color", "red");
        }
    });
    if (ValidInfo == "Y") {
        return false;
    }
    else {
        if (CalculateDeliverySchQty_ForDirect() === false) {
            $('#SpanDelSchItemQty').text($("#SchReqQtyEqualTo").text());
            $("#SpanDelSchItemQty").css("display", "block");
            $("#DeliverySchQty").css("border-color", "red");
            return false;
        }
        else {
            $("#DeliverySchQty").css("border-color", "#ced4da");
            $("#SpanDelSchItemQty").css("display", "none");

            AppendDeliverySchedule_ForDirect(1,$('#DeliverySchItemDDL option:selected').text(), $('#DeliverySchItemDDL').val(),
                $('#DeliveryDate').val(), $('#DeliverySchQty').val());
            BindDelivertSchItemList_ForInputItm();
            $("#DeliveryDate").val("");
        }
    }
}
function AppendDeliverySchedule_ForDirect(rowIdx,item_name, item_id, sch_date, delv_qty) {
    debugger;
    if (item_name != "---Select---") {
        var QtyDecDigit = $("#QtyDigit").text();
        var deletetext = $("#Span_Delete_Title").text();
        var rowCount = $('#DeliverySchTble tbody tr').length;

      
            $('#DeliverySchTble tbody').append(`<tr id="R${rowIdx}" class="even pointer">
 <td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="DeliveryDelIconBtn" title="${deletetext}"></i></td>
 <td id="cmn_srno">${++rowCount}</td>
 <td class="" hidden="hidden" id="Hd_ItemIdFrDS">${item_id}</td>
 <td class="a-center " id="Hd_ItemNameFrDS">${item_name}</td>
 <td class="">${moment(sch_date).format('DD-MM-YYYY')}</td>
 <td class="num_right" id="delv_qty">${parseFloat(delv_qty).toFixed(QtyDecDigit)}</td>
 <td style="display:none;" id="sch_date">${sch_date}</td>
</tr>`);
        
    }
    DeleteDeliverySch_ForDirect();
    ResetSerialNoInDelShcdl_ForDirect();
}
function DeleteDeliverySch_ForDirect() {
    $('#DeliverySchTble tbody').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        debugger
        $(this).closest('tr').remove();
        var ItemID = $(this).closest('tr')[0].cells[2].innerHTML;
        ResetSerialNoInDelShcdl_ForDirect();
        BindDelivertSchItemList_ForInputItm();
        //$("#DeliverySchItemDDL option[value=" + ItemID + "]").show();
        //$('#DeliverySchItemDDL').val("0").prop('selected', true);
    });
}
function ResetSerialNoInDelShcdl_ForDirect() {
    var SrNo = 0;
    $('#DeliverySchTble tbody tr').each(function () {
        $(this).find("#cmn_srno").text(++SrNo);
    })

}

function CheckDeliverySchdlValidations_ForDirect( HedrItemDDlId, HedrItemDDlName, ItemQtyId) {

    debugger
    var QtyDecDigit = $("#QtyDigit").text();
    var DocMenuId = $("#DocumentMenuId").val();
    var flag = "N";
    debugger;
        var ItemID = $("#" + HedrItemDDlId).val();
        var ItmQty = $("#" + ItemQtyId).val();
        if (ItmQty == "" || ItmQty == null) {
            ItmQty = $("#" + ItemQtyId).val();
        }
        /*var ItemName = $("#" + ItemDDlId + SNohf + " option:selected").text();*/
        var ItemName = $("#" + HedrItemDDlName).val();
        
        var DelSchdlQty = $("#DeliverySchTble > tbody > tr").length;
            if (DelSchdlQty == 0) {
                swal("", $("#DeliveryScheduleQuantityDoesNotMatchWithItemQuantityFor").text() + " " + ItemName, "warning");
                flag = "Y";
                return false;
            }
       
        var ItemQTYDS = 0;

        /*if (DocMenuId == "105101130" || DocMenuId == "105101140101") {*/
            if ($("#DeliverySchTble > tbody > tr #Hd_ItemIdFrDS:contains(" + ItemID + ")").length > 0) {
                $("#DeliverySchTble > tbody > tr #Hd_ItemIdFrDS:contains(" + ItemID + ")").each(function () {
                    var delv_qty = $(this).closest('tr').find("#delv_qty").text();
                    ItemQTYDS = parseFloat(ItemQTYDS) + parseFloat(CheckNullNumber(delv_qty));
                });
                if (parseFloat(CheckNullNumber(ItmQty)).toFixed(QtyDecDigit) != parseFloat(CheckNullNumber(ItemQTYDS)).toFixed(QtyDecDigit)) {
                    swal("", $("#DeliveryScheduleQuantityDoesNotMatchWithItemQuantityFor").text() + " " + ItemName, "warning");
                    flag = "Y";
                    return false;
                }
            }
            else {
                swal("", $("#DeliveryScheduleQuantityDoesNotMatchWithItemQuantityFor").text() + " " + ItemName, "warning");
                flag = "Y";
                return false;
            }
       /* }*/
        
   
    if (flag == "Y") {
        return false;
    }
}
//------------------End Delivery Schedule Section For Input item Detail only for single Item using Direct Srctyp------------------//

function BindDDlJOList() {
    debugger;
    var Branch = sessionStorage.getItem("BranchID");
    $("#SupplierName").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term, // search term like "a" then "an"
                    SuppPage: params.page,
                    BrchID: Branch
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });

}

function OnChangeSuppName(SuppID) {
    debugger;
    var Supp_id = SuppID.value;
    var SrcTyp= $("#Hdn_JOSrcType").val();
    if (Supp_id == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        //DisableItemDetailsPO();
        //$("#PoItmDetailsTbl .plus_icon1").css("display", "none");
        $("#Address").val("");

    }
    else {
        var SuppName = $("#SupplierName option:selected").text();
        $("#Hdn_JOSupplierName").val(SuppName)

        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
        $("#ddl_FProductName").attr('disabled', false);
    }
    if (SrcTyp == "PrdOrd") {
        $("#Prod_Ord_number").attr('disabled', false);
    }
    
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/JobOrderSC/GetSuppAddrDetail",
                data: {
                    Supp_id: Supp_id,
                },
                success: function (data) {
                    //debugger;
                    if (data == 'ErrorPage') {
                        PO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#Rqrdaddr").css("display", "");
                            $("#Address").val(arr.Table[0].BillingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#ship_add_gstNo").val(arr.Table[0].supp_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].state_code);
                            //CheckJOHeaderValidations();
                            if ($("#Address").val() === "") {
                                $('#SpanSuppAddrErrorMsg').text($("#valueReq").text());
                                $("#Address").css("border-color", "Red");
                                $("#SpanSuppAddrErrorMsg").css("display", "block");
                                ErrorFlag = "Y";
                            }
                            else {
                                $("#SpanSuppAddrErrorMsg").css("display", "none");
                                $("#Address").css("border-color", "#ced4da");
                            }
                        }
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function BindProdOrderNumber() {
    debugger;
    //SuppID = $("#SupplierName").val();
   
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/JobOrderSC/GetProducORDDocList',
            data: { },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "" && data !== "{}") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        debugger;

                        var status = $("#hfStatus").val();
                        $("#Prod_Ord_number option").remove();
                        $("#Prod_Ord_number optgroup").remove();
                        $('#Prod_Ord_number').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}' data-fitem='${$("#span_FinishedProduct").text()}' data-fuom='${$("#ItemUOM").text()}' data-opname='${$("#span_OpName").text()}' ></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no + '-' + arr.Table[i].fitem_id + '-' + arr.Table[i].uom_id + '-' + arr.Table[i].op_id}" data-fitem = "${arr.Table[i].fitem_name}" data-fuom = "${arr.Table[i].fuom}" data-opname = "${arr.Table[i].op_name}">${arr.Table[i].doc_no}</option>`);
                         }
                        var firstEmptySelect = true;
                       
                        $('#Prod_Ord_number').select2({
                            templateResult: function (data) {
                                debugger;
                                var DocDate = $(data.element).data('date');
                                var fitem = $(data.element).data('fitem');
                                var fuom = $(data.element).data('fuom');
                                var opname = $(data.element).data('opname');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-3 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-2 col-xs-12' + classAttr + '">' + DocDate + '</div>' +
                                    '<div class="col-md-3 col-xs-12' + classAttr + '">' + fitem + '</div>' +
                                    '<div class="col-md-1 col-xs-12' + classAttr + '">' + fuom + '</div>' +
                                    '<div class="col-md-3 col-xs-12' + classAttr + '">' + opname+ '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });
                       /* '<div class="col-md-4 col-xs-4' + classAttr + '">' + opname + '</div>' +*/
                        $("#produc_ord_date").val("");

                    }
                    
                }
            }

        })
    
}
function OnClickbillingAddressIconBtn(e) {
    debugger;
    var Supp_id = $('#SupplierName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#hfStatus").val().trim();
    var JODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        JODTransType = "Update";
    }

    Cmn_SuppAddrInfoBtnClick(Supp_id, bill_add_id, status, JODTransType);
}
function OnClickIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    ItmCode = clickedrow.find("#JOItemListName" + Sno).val();
    ItemInfoBtnClick(ItmCode);
}
function OnchangePrducORDDocNumber() {
    debugger;
    var suppname = $("#SupplierName").val();
   
    var doc_no = $("#Prod_Ord_number").val();
    var ProductionOrdrNum = $("#Prod_Ord_number option:selected").text();
    $("#HdnProducNumbr").val(ProductionOrdrNum);
    var docno = doc_no.split('-');

    var prdordNo = docno[0];
    $("#Hdn_ProducNum").val(prdordNo);
    var FItem_Id = docno[1];
    var FUom_id = docno[2];
    var OPid = docno[3];
    var DocMenuId = $("#DocumentMenuId").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    debugger
    
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $('#Prod_Ord_number').empty().append('<option value="---Select----0-0-0" selected="selected">---Select---</option>');
        BindProdOrderNumber();
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");

        if (prdordNo != 0) {
            $("#SpanProdOrdNoErrorMsg").css("display", "none");
            $("#Prod_Ord_number").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-Prod_Ord_number-container']").css("border-color", "#ced4da");
            var doc_Date = $("#Prod_Ord_number option:selected")[0].dataset.date
            var newdate = doc_Date.split("-").reverse().join("-");
            var OpName = $("#Prod_Ord_number option:selected")[0].dataset.opname
            var Fitem = $("#Prod_Ord_number option:selected")[0].dataset.fitem
            var Fuom = $("#Prod_Ord_number option:selected")[0].dataset.fuom

            $("#produc_ord_date").val(newdate);
            $("#Prod_Ord_number").val(prdordNo);

            $("#Hdn_ProducNum").val(prdordNo);
            $("#TxtFinishedProduct").val(Fitem)
            $("#Hdn_FinishProductId").val(FItem_Id)
            $("#TxtFinishedUom").val(Fuom)
            $("#Hdn_FinishUomId").val(FUom_id)
            $("#OperationName").val(OpName)
            $("#Hdn_OPid").val(OPid)

            var QtyDecDigit = $("#QtyDigit").text();///Quantity
            EnableItemDetailsJO();
            $("#JoItmDetailsTbl .plus_icon1").css("display", "block");
            
            

            $.ajax({
                type: 'POST',
                url: '/ApplicationLayer/JobOrderSC/GetDetailsAgainstProducOrdNo',
                data: {
                    ProductionOrd_no: prdordNo,
                    ProductionOrd_date: newdate,

                },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        JO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "" && data !== "{}") {
                        var arr = [];
                        var arr = JSON.parse(data);

                       if (arr.Table.length > 0) {
                            debugger;
                            var rowIdx = 0;
                            for (var k = 0; k < arr.Table.length; k++) {
                                var S_NO = $('#JoOutputItmDetailsTbl tbody tr').length;
                                var subitmDisable = "";
                                if (arr.Table[k].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                var RowNo = 0;
                                var levels = [];
                                $("#JoOutputItmDetailsTbl >tbody >tr").each(function (i, row) {
                                    var currentRow = $(this);
                                    RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
                                    levels.push(RowNo);
                                });
                                if (levels.length > 0) {
                                    RowNo = Math.max(...levels);
                                }
                                RowNo = RowNo + 1;
                                if (S_NO == "0") {
                                    S_NO = 1;
                                }
                               //$("#JoInputItmDetailsTbl >tbody >tr").remove();
                                $('#JoOutputItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td class="sr_padding">${RowNo}</td>
                                    <td style="display: none;"><input type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
                                                                <td id="JOOutputItemListName">
                                                                            <div class="col-sm-11 i_Icon">
                                                                                ${arr.Table[k].item_name}
                                                                            </div>
                                                                            <div class="col-sm-1 i_Icon">
                                                                                <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title=" @Resource.ItemInformation"> </button>
                                                                            </div>
                                                                        </td>

                                    <td style="display: none;"><input type="hidden" id="hdnOutItemName" value="${arr.Table[k].item_name}" /></td>
                                    <td style="display: none;"><input type="hidden" id="hfItemID" value="${arr.Table[k].item_id}" /></td>

                                    <td id="UOM">${arr.Table[k].uom_alias}</td>
                                    <td style="display: none;"><input id="UOMID" type="hidden" value="${arr.Table[k].uom_id}"></td>
                                    <td style="display: none;"><input id="ItmTypID" type="hidden" value="${arr.Table[k].item_type_id}"></td>
                                    <td id="OrderQuantity" class="num_right">
                                        <div class="col-sm-10 lpo_form no-padding">
                                         ${parseFloat(arr.Table[k].Ord_qty).toFixed(QtyDecDigit)}
                                        </div>
                                        <div class="col-sm-2 i_Icon no-padding" id="div_SubItemJOOrdQty" >
                                        <input hidden type="text" id="JoOutsub_item" value="${arr.Table[k].sub_item}" />
                                        <button type="button" id="SubItemJOOrdQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('JOOrdQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                        </div>
                                    </td>

                                    <td>&nbsp;</td>

                                </tr>`);
                            }
                            debugger;
                            
                            BindDelivertSchItemList();
                        }

                        if (arr.Table2.length > 0)
                        {
                            debugger;
                            var rowIdx = 0;
                            for (var d = 0; d < arr.Table2.length; d++) {
                                var S_NO = $('#JoInputItmDetailsTbl tbody tr').length;
                                var subitmDisable = "";
                                if (arr.Table2[d].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                //$("#Prod_Ord_number option[value='" + doc_no + "']").select2().show();
                                var RowNo = 0;
                                var levels = [];
                                $("#JoInputItmDetailsTbl >tbody >tr").each(function (i, row) {
                                    var currentRow = $(this);
                                    RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
                                    levels.push(RowNo);
                                });
                                if (levels.length > 0) {
                                    RowNo = Math.max(...levels);
                                }
                                RowNo = RowNo + 1;
                                if (S_NO == "0") {
                                    S_NO = 1;
                                }
                               
                                $('#JoInputItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td>&nbsp;</td>
<td class="sr_padding">${RowNo}</td>
                                    <td style="display: none;"><input type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
                                                                        <td id="JOInputItemListName" class="pl-2">
                                                                            <div class="col-sm-11 i_Icon pl-2">
                                                                                ${arr.Table2[d].item_name}
                                                                            </div>
                                                                            <div class="col-sm-1 i_Icon">
                                                                                <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title=" @Resource.ItemInformation"> </button>
                                                                            </div>
                                                                        </td>

                                    <td style="display: none;" class="pr-2 pl-2"><input type="hidden" id="hdnInItemName" value="${arr.Table2[d].item_name}" /></td>
                                    <td style="display: none;" class="pr-2 pl-2"><input type="hidden" id="hfItemID" value="${arr.Table2[d].item_id}" /></td>

                                    <td id="UOM" class="pr-2 pl-2">${arr.Table2[d].uom_alias}</td>
                                    <td style="display: none;" class="pr-2 pl-2"><input id="UOMID" type="hidden" value="${arr.Table2[d].uom_id}"></td>

                                    <td id="ItemType" class="pr-2 pl-2">${arr.Table2[d].Item_type}</td>
                                    <td style="display: none;" class="pr-2 pl-2"><input id="ItemTypeID" type="hidden" value="${arr.Table2[d].item_type_id}"></td>

                                    <td id="RequiredQuantity" class="num_right ">
                                        <div class="col-sm-10 lpo_form no-padding pt-2">
                                        ${parseFloat(arr.Table2[d].Req_qty).toFixed(QtyDecDigit)}
                                        </div>
                                        <div class="col-sm-2 i_Icon" id="div_SubItemJOReqQty" >
                                        <input hidden type="text" id="JoInsub_item" value="${arr.Table2[d].sub_item}" />
                                        <button type="button" id="SubItemJOReqQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('JOReqQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                        </div>
                                    </td>
<td>&nbsp;</td>
                                    
                                </tr>`);
                            }

                        }
                        debugger;
                        if (arr.Table3.length > 0) {
                            debugger;
                            var rowIdx = 0;
                             for (var y = 0; y < arr.Table3.length; y++) {

                                var ItmId = arr.Table3[y].item_id;
                                var SubItmId = arr.Table3[y].sub_item_id;
                                var Qty = arr.Table3[y].qty;
                                 var itmtyp = "";
                                 $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${ItmId}'></td>
                            <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                            <td><input type="text" id="subItemQty" value='${Qty}'></td>
                            </tr>`);
                             }

                        }
                    }
                    debugger;
                    //BindDelivertSchItemList();
                    debugger;
                    $("#SupplierName").attr('disabled', true);
                    $("#Prod_Ord_number").attr('disabled', true);
                    $("#JoOutputItmDetailsTbl >tbody >tr").each(function (i, row) {
                        var currentRow = $(this);
                        debugger;
                        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
                        var ItmType = currentRow.find("#ItmTypID").val();
                        if (ItmType == "OF" || ItmType == "OW") {
                            OrdQtyOut = currentRow.find("#OrderQuantity").text().trim();
                            $("#ServiceOrdQty_ID").val(OrdQtyOut);
                            
                        }
                    });
                    var srvcOrdQty = $("#ServiceOrdQty_ID").val();
                    $("#OrdQtyServc").val(srvcOrdQty);
                }
            });


        }
    }
}
function EnableItemDetailsJO() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    $("#JoItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
       
        currentRow.find("#JOItemListName" + Sno).attr("disabled", false);
        /*currentRow.find("#ItmInfoBtnIcon").attr("disabled", true);*/
        
        currentRow.find("#item_rate").attr("disabled", false);
        
        currentRow.find("#remarks").attr("disabled", false);
        currentRow.find("#TaxExempted").attr("disabled", false);
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            currentRow.find("#ManualGST").attr("disabled", false);
        }
        
    });
}
function OnClickIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    //ItmCode = clickedrow.find("#JOItemListName").val();
    ItemInfoBtnClick(ItmCode);
}

function OnClickHistoryIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    var UOMName = "";
    var SuppID = "";
    SuppID = $("#SupplierName").val();
    ItmCode = clickedrow.find("#hfItemID").val();
    ItmName = clickedrow.find("#JOItemListName" + Sno + " option:selected").text();
    //ItmNamebr = clickedrow.find("#JOItemListName" + Sno).text().trim();
    //var iTE = ItmNamebr.split('-');
    
    //ItmName = iTE[6];
    UOMName = clickedrow.find("#UOM").val();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
}
function OnclickHistorySearchBtn() {
    debugger;
    ItmCode = $("#hfHistoryItemID").val();
    ItmName = $("#hfHistoryItemName").val();
    UOMName = $("#hfHistoryUOM").val();
    var SuppID = "";
    SuppID = $("#SupplierName").val();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
}
//-----------------Service Item Detail Section Start--------------------------------//


function AddNewRow() {
    debugger;
    //if (JOHeaderValidations() == false) {
    //    return false;
    //}
   
    var rowIdx = 0;
    var jobOrdNo = $("#JobOrderNumber").val();
    if (jobOrdNo == "" || jobOrdNo == null) {
       
        var JOSrcType = $("#Hdn_JOSrcType").val();
        if (JOSrcType == "Direct") {
            var SerOrdQty = $("#JOOrdQuantity").val();
        }
        else {
            var SerOrdQty = $("#ServiceOrdQty_ID").val();
        }
        
    }
    else {
        var itmlen = $("#JoItmDetailsTbl >tbody >tr").length;
        if (itmlen == 0) {
            var JOSrcType = $("#Hdn_JOSrcType").val();
            if (JOSrcType == "Direct") {
                var SerOrdQty = $("#JOOrdQuantity").val();
                $("#ServiceOrdQty_ID").val(SerOrdQty);
            }
            else {
                var SerOrdQty = $("#ServiceOrdQty_ID").val();
            }
            
        }
        else {
            $("#JoItmDetailsTbl >tbody >tr").each(function (i, row) {
                var currentRow = $(this);
                debugger;
                RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
                var OrdQtyOut = currentRow.find("#OrdQtyServc").val();
                $("#ServiceOrdQty_ID").val(OrdQtyOut);
            });
        }
        
        var SerOrdQty = $("#ServiceOrdQty_ID").val();
    }
    var ItemInfo = $("#ItmInfo").text();
    var TaxInfo = $("#TaxInfo").text();
    var RemarksInfo=$("#span_remarks").text()
    var Span_BuyerInformation_Title = $("#Span_BuyerInformation_Title").text();
    var Span_Delete_Title = $("#Span_Delete_Title").text();
    debugger;
 
    var rowCount = $('#JoItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#JoItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
        levels.push(RowNo);
    });
    if (levels.length > 0) {
        RowNo = Math.max(...levels);
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var ManualGst = "";
        if (GstApplicable == "Y") {
            ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
        }
    RowNo = RowNo + 1;
    if (RowNo == "0") {
        RowNo = 1;
    }
   $('#JoItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${Span_Delete_Title}"></i></td>
<td id="SRNO" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;">
<input  type="hidden" id="hfItemID" />
<input  type="hidden" id="ItemHsnCode" />
</td>
<td class="ItmNameBreak itmStick tditemfrz">
<div class="col-sm-11 lpo_form no-padding">
<select class="form-control" id="JOItemListName${RowNo}" name="JOItemListName" onchange ="OnChangeJOItemName(${RowNo},event)"></select>
<span id="JOItemListNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="SaleHistory" class="calculator" onclick="OnClickHistoryIconBtn(event);" data-toggle="modal" data-target="#SaleHistory" data-backdrop="static" data-keyboard="false"> <i class="fa fa-history" aria-hidden="true" title="${span_OrderHistory}" ></i> </button>
</div>
</td>

<td>
<input id="OrdQtyServc" class="form-control num_right" value="${SerOrdQty}" autocomplete="off" type="text" name="OrdQtyServc_name" placeholder="0000.00"  disabled>
 <input id="ItmTypIDSer" type="hidden" />
</td>

<td>
<div class="lpo_form">
<input id="item_rate" class="form-control date num_right"  onchange ="OnChangeJOItemRate(1,event)" autocomplete="off" onpaste="return CopyPasteData(event);" onkeypress="return AmountFloatRate(this,event);" type="text" name="Price"  placeholder="0000.00">
<span id="item_rateError" class="error-message is-visible"></span>
</div>
</td>
<td>
<input id="item_gr_val" class="form-control date num_right" autocomplete="off" type="text" name="item_gr_val_name"  placeholder="0000.00"  disabled>
</td>
<td class="qt_to"> <div class="custom-control custom-switch sample_issue"> <input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"> <label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>
`+ ManualGst + `
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="item_tax_amt" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt_name"  placeholder="0000.00"  disabled>
</div>
<div class="col-sm-2 lpo_form no-padding">
<button type="button" class="calculator" id="BtnTxtCalculation"  onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" data-toggle="tooltip" title="${TaxInfo}" data-original-title=""></i></button>
</div>
</td>
<td>
<input id="item_oc_amt" class="form-control date num_right" autocomplete="off" type="text" name="item_oc_amt_name"  placeholder="0000.00"  disabled>
</td>
<td>
<input id="item_net_val" class="form-control num_right" autocomplete="off" type="text" name="Total_Value"  placeholder="0000.00" disabled>
</td>
<td>
<div class="custom-control custom-switch sample_issue">
<input type="checkbox" class="custom-control-input  margin-switch" id="SimpleIssue" disabled>
<label class="custom-control-label " for="SimpleIssue1"></label>
</div>
</td>
<td>
<input id="MRSNumber" class="form-control num_right" autocomplete="off" type="text" name="MRSNumber"  placeholder="0000.00" onblur="this.placeholder='MRSNumber'" disabled>
</td>
<td>
<textarea id="remarks" class="form-control remarksmessage" maxlength="100" name="message"  onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}"></textarea>
</td>
</tr>`);
    BindJOItmList(RowNo);
};
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var ValDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#JOItemListName" + RowSNo).val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        CalculationBaseRate(e);
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "JoItmDetailsTbl", "hfItemID", "QuotationNumber", "item_gr_val", "TaxCalcGRNNo", "TaxCalcItemCode",e)
        /*CalculationBaseRate(e);*/
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    //var ItmCode = currentrow.find("#SQItemListName" + RowSNo).val();
    //var AssAmount = currentrow.find("#item_ass_val").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        $("#taxTemplate").text("Template")
        //currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        //CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        CalculationBaseRate(e);
        currentrow.find('#TaxExempted').prop("checked", false);
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "JoItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_gr_val","","",e)
        CalculationBaseRate(e);
        $("#taxTemplate").text("Template")
    }
}
function BindJOItmList(ID) {
    debugger;
    var ItmDDLName = "#JOItemListName";
    var TableID = "#JoItmDetailsTbl";
    var SnoHiddenField = "#SNohiddenfiled";
    $(ItmDDLName + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    $('#Textddl' + ID).append(`<option data-uom="0" value="0">---Select---</option>`);
    DynamicSerchableItemDDL(TableID, ItmDDLName, ID, SnoHiddenField, "", "JO")

    //BindItemList("#JOItemListName", ID, "#JoItmDetailsTbl", "#SNohiddenfiled", "", "JO");
}
function BindJOItmListPageLoad(ID)
{
    debugger;
    BindItemList("#JOItemListName", ID, "#JoItmDetailsTbl", "#SNohiddenfiled", "", "JO");
}
function SerialNoAfterDelete() {
    debugger
    var SerialNo = 0;
    $("#JoItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SRNO").text(SerialNo);


    });
};

function OnChangeJOItemName(RowID, e) {
    debugger;
    BindJOItemList(e);
}

function BindJOItemList(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    var Itm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    Itm_ID = clickedrow.find("#JOItemListName" + SNo).val();
    clickedrow.find("#hfItemID").val(Itm_ID);
    debugger;
    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "jobord");
    if (Itm_ID == "0") {
        clickedrow.find("#JOItemListNameError").text($("#valueReq").text());
        clickedrow.find("#JOItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-JOItemListName" + SNo + "-container']").css("border-color", "red");
    }
    else {
        clickedrow.find("#JOItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-JOItemListName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    ClearRowDetails(e, ItemID);
    
    //try {
    //    debugger;
    //    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "jobord");

    //} catch (err) {
    //}
    
}
function ClearRowDetails(e, ItemID) {
    //debugger;
    var docid = $("#DocumentMenuId").val();
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#ItmInfoBtnIcon").val("");
    clickedrow.find("#UOM").val("");
    clickedrow.find("#BtnTxtCalculation").val("");
    
    //clickedrow.find("#OrdQtyServc").val("");
    clickedrow.find("#item_rate").val("");
    
    clickedrow.find("#item_gr_val").val("");
   
    clickedrow.find("#item_tax_amt").val("");
    clickedrow.find("#item_oc_amt").val("");
  
    clickedrow.find("#item_net_val").val("");
    clickedrow.find("#SimpleIssue").attr("checked", false)
    clickedrow.find("#MRSNumber").val("");
    clickedrow.find("#remarks").val("");
    
    
    CalculateAmount();
    var TOCAmount = parseFloat($("#JO_OtherCharges").val());
    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        TOCAmoun = 0;
    }
    Calculate_OC_AmountItemWise(TOCAmount);
    AfterDeleteResetJO_ItemTaxDetail();
    //if (ItemID != "" && ItemID != null) {
    //    DelDeliSchAfterDelItem(ItemID);
    //}
}


//----------------------Service Item Detail Section End---------------------------//
//------------------Delivert Schedule Section------------------//
function BindDelivertSchItemList() {
    debugger;
    Cmn_BindDelivertSchItemList("JoOutputItmDetailsTbl", "N", "SNohiddenfiled", "JOOutputItemListName-" + "hfItemID-" + "ItmTypID", "OrderQuantity");
   
    };
function OnChangeDelSchItm() {
    debugger;
    var SrcTyp = $("#Hdn_JOSrcType").val();
    if (SrcTyp == "PrdOrd") {
        Cmn_OnChangeDelSchItm("JoOutputItmDetailsTbl", "N", "SNohiddenfiled", "JOOutputItemListName-" + "hfItemID-" + "ItmTypID", "OrderQuantity");
        /*Cmn_OnChangeDelSchItm("JoItmDetailsTbl", "N", "SNohiddenfiled", "JOItemListName-" +"hfItemID", "OrderQuantity");*/
    }
    else {
        OnChangeDelSchItm_ForDirect();
    }
    }
function OnChangeDeliveryQty() {
    debugger;
    var SrcTyp = $("#Hdn_JOSrcType").val();
    if (SrcTyp == "PrdOrd") {
        Cmn_OnChangeDeliveryQty("JoOutputItmDetailsTbl", "N", "SNohiddenfiled", "JOOutputItemListName-" + "hfItemID-" + "ItmTypID", "OrderQuantity");
        /*Cmn_OnChangeDeliveryQty("JoItmDetailsTbl", "N", "SNohiddenfiled", "JOItemListName-" + "hfItemID", "OrderQuantity");*/
    }
    else {
        OnChangeDeliveryQty_ForDirect();
    }
}
function OnClickAddDeliveryDetail() {
    debugger;
    var SrcTyp = $("#Hdn_JOSrcType").val();
    if (SrcTyp == "PrdOrd") {
        Cmn_OnClickAddDeliveryDetail("JoOutputItmDetailsTbl", "N", "SNohiddenfiled", "JOOutputItemListName-" + "hfItemID-" + "ItmTypID", "OrderQuantity");
        /*Cmn_OnClickAddDeliveryDetail("JoItmDetailsTbl", "N", "SNohiddenfiled", "JOItemListName-" + "hfItemID", "OrderQuantity");*/
    }
    else {
        OnClickAddDeliveryDetail_ForDirect();
    }
}
function DeleteDeliverySch() {
    debugger;
    Cmn_DeleteDeliverySch();
}
function OnChangeDeliveryDate(DeliveryDate) {
    debugger;
    Cmn_OnChangeDeliveryDate(DeliveryDate);
}
function DelDeliSchAfterDelItem(ItemID) {
    debugger;
    Cmn_DelDeliSchAfterDelItem(ItemID, "JoOutputItmDetailsTbl", "N", "SNohiddenfiled", "JOOutputItemListName-" + "hfItemID", "OrderQuantity");

    /*Cmn_DelDeliSchAfterDelItem(ItemID, "JoItmDetailsTbl", "N", "SNohiddenfiled", "JOItemListName-" + "hfItemID", "OrderQuantity");*/
}

//------------------Delivert Schedule Section End------------------//

//------------------Term & Condition Start------------------//
function appendTermsAndCondition(rowIdx, TandC) {
    debugger;
    var deletetext = $("#Span_Delete_Title").text();
    $('#TblTerms_Condition tbody').append(`<tr id="R${rowIdx}" class="even pointer">
<td class="red"><i class="deleteIcon fa fa-trash" id="TC_DelIcon" title="${deletetext}"></i> </td>
<td>${TandC}</td>
</tr>`);
}
//------------------Term & Condition End------------------//

//------------------Tax & Other Charge Section Start------------------//
function CalculateAmount() {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var GrossValue = parseFloat(0).toFixed(DecDigit);
    //var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var TaxValue = parseFloat(0).toFixed(DecDigit);
    //var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

    $("#JoItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        if (currentRow.find("#item_gr_val").val() == "" || currentRow.find("#item_gr_val").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
        }
        //if (currentRow.find("#item_gr_val").val() == "" || currentRow.find("#item_gr_val").val() == "NaN") {
        //    AssessableValue = (parseFloat(AssessableValue) + parseFloat(0)).toFixed(DecDigit);
        //}
        //else {
        //    AssessableValue = (parseFloat(AssessableValue) + parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
        //}
        if (currentRow.find("#item_tax_amt").val() == "" || currentRow.find("#item_tax_amt").val() == "0" || currentRow.find("#item_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#item_tax_amt").val())).toFixed(DecDigit);
        }
        //if (currentRow.find("#item_net_val").val() == "" || currentRow.find("#item_net_val").val() == "NaN") {
        //    NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(DecDigit);
        //}
        //else {
        //    NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#item_net_val").val())).toFixed(DecDigit);
        //}
        if (currentRow.find("#item_net_val").val() == "" || currentRow.find("#item_net_val").val() == "NaN") {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#item_net_val").val())).toFixed(DecDigit);
        }
    });
    $("#TxtGrossValue").val(GrossValue);
    //$("#TxtAssessableValue").val(AssessableValue);
    $("#TxtTaxAmount").val(TaxValue);
    //$("#NetOrderValueSpe").val(NetOrderValueSpec);
    $("#NetOrderValueInBase").val(NetOrderValueBase);
};
function OnChangeGrossAmt() {
    debugger;
    var TotalOCAmt = $('#Total_OC_Amount').text();
    var Total_JO_OCAmt = $('#JO_OtherCharges').val();
    if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_JO_OCAmt) > 0) {
        if (parseFloat(TotalOCAmt) === parseFloat(Total_JO_OCAmt)) {
            Calculate_OC_AmountItemWise(TotalOCAmt);
        }
    }
}
function OnChangeJOItemRate(RowID, e) {
    debugger;
    //let trgtrow = $(e.target).closest("tr");
    //CalculationBaseRate(trgtrow);
    CalculationBaseRate(e);
}
function CalculationBaseRate(e) {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    //var clickedrow = $(e.target).closest("tr");
    
    var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var OrderQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer;
    OrderQty = clickedrow.find("#OrdQtyServc").val();

    ItemName = clickedrow.find("#JOItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
   
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
   /* if (ItmRate == "" || ItmRate == 0 || ItemName == "0") {*/
    if (ItmRate == "" || ItmRate == 0 ) {
        clickedrow.find("#item_rateError").text($("#valueReq").text());
        clickedrow.find("#item_rateError").css("display", "block");
        clickedrow.find("#item_rate").css("border-color", "red");
        clickedrow.find("#item_rate").val("");
        if (OrderQty != "" && OrderQty != null) {
            clickedrow.find("#item_rate").focus();
        }
        clickedrow.find("#item_oc_amt").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#item_rateError").css("display", "none");
        clickedrow.find("#item_rate").css("border-color", "#ced4da");
    }

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#item_rate").val("");
        ItmRate = 0;
        // return false;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#item_gr_val").val(FinVal);
        //clickedrow.find("#item_ass_val").val(FinVal);
        clickedrow.find("#item_net_val").val(FinVal);
        //clickedrow.find("#item_net_val_bs").val(FinVal);
        CalculateAmount();
    }

    clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    OnChangeGrossAmt();
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
    var assVal = clickedrow.find("#item_gr_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        //clickedrow.find("#item_ass_valError").css("display", "none");
        //clickedrow.find("#item_gr_val").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }

}
function AmountFloatRate(el, evt) {
    //debugger
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
}
function AmountFloatVal(el, evt) {
    debugger
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var key = evt.key;
    var number = el.value.split('.');
    var item_rate = clickedrow.find("#QuotedPrice").val();
    item_rate = CheckNullNumber(item_rate);

    var selectedval = Cmn_SelectedTextInTextField(evt);
    if (number.length == 1) {
        var valPer = number[0] + '' + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }

    }
    else {
        var valPer = el.value + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;

        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }
    }
    //clickedrow.find("#item_disc_amtError").css("display", "none");
    //clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
    return true;
}
function AmountFloatQty(el, evt) {
    //debugger
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
}
function AmountFloatPer(el, evt) {
    debugger
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
}

function Calculate_OC_AmountItemWise(TotalOCAmt) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var TotalGAmt = $("#TxtGrossValue").val();

    $("#JoItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();

        var GrossValue = currentRow.find("#item_gr_val").val();
        if (GrossValue == "" || GrossValue == null) {
            GrossValue = "0";
        }
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            debugger;
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(DecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(DecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#item_oc_amt").val(parseFloat(OCAmtItemWise).toFixed(DecDigit));
            }
            else {
                currentRow.find("#item_oc_amt").val(parseFloat(0).toFixed(DecDigit));
            }
        }
        if (parseFloat(TotalOCAmt) == 0) {
            currentRow.find("#item_oc_amt").val(parseFloat(0).toFixed(DecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(DecDigit);
    $("#JoItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var OCValue = currentRow.find("#item_oc_amt").val();
        if (OCValue != null && OCValue != "") {
            if (parseFloat(OCValue) > 0) {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
            }
            else {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(0);
            }
        }
    });
    if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
        $("#JoItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        $("#JoItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#JoItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var POItm_GrossValue = currentRow.find("#item_gr_val").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
        var POItm_OCAmt = currentRow.find("#item_oc_amt").val();
        if (POItm_GrossValue == null || POItm_GrossValue == "") {
            POItm_GrossValue = "0";
        }
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#item_tax_amt").val() != null && currentRow.find("#item_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#item_tax_amt").val()).toFixed(DecDigit);
        }
        //var POItm_NetOrderValueSpec = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        //currentRow.find("#item_net_val").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(DecDigit));
        currentRow.find("#item_net_val").val((parseFloat(POItm_NetOrderValueBase)).toFixed(DecDigit));
    });
    CalculateAmount();
};
function OnClickSaveAndExit_OC_Btn() {
    //debugger;
    //var NetOrderValueSpe = "#NetOrderValueSpe";
    var NetOrderValueInBase = "#NetOrderValueInBase";
    var JO_otherChargeId = '#Tbl_OtherChargeList';

    CMNOnClickSaveAndExit_OC_Btn(JO_otherChargeId, NetOrderValueInBase, NetOrderValueInBase);
}
function BindOtherChargeDeatils() {
    debugger;
    var DecDigit = $("#ValDigit").text();//Tbl_OC_Deatils
    if ($("#JoItmDetailsTbl >tbody >tr").length == 0) {
        $("#Tbl_OC_Deatils >tbody >tr").remove();
        $("#Total_OC_Amount").text(parseFloat(0).toFixed(DecDigit));
        $("#JO_OtherCharges").val(parseFloat(0).toFixed(DecDigit));
    }

    $("#Tbl_OtherChargeList >tbody >tr").remove();
    $("#JO_OtherChargeTotal").text(parseFloat(0).toFixed($("#ValDigit").text()));
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td>${currentRow.find("#OCName").text()}</td>
<td align="right">${currentRow.find("#OCAmount").text()}</td>
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
        });

    }
    $("#_OtherChargeTotal").text(TotalAMount);
    $("#JO_OtherCharges").val(TotalAMount);
}
function SetOtherChargeVal() {

}


//------------------Tax Amount Calculation------------------//
function OnClickTaxCalculationBtn(e) {
    debugger;
    var JOItemListName = "#JOItemListName";
    var SNohiddenfiled = "#SNohiddenfiled";

    var currentrow = $(e.target).closest('tr');
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("GST Slab");
    }
    else {
        $("#taxTemplate").text("Template")
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, JOItemListName);
    if ($("#Disable").val() == "Y") {
        $("#Tax_Template").attr("disabled", true);
        $("#SaveAndExitBtn").prop("disabled", true);
    }
    else {
        if (currentrow.find("#ManualGST").is(":checked")) {
            $("#Tax_Template").attr("disabled", false);
            $("#SaveAndExitBtn").prop("disabled", false);
        }
        else {
            $("#Tax_Template").attr("disabled", true);
            $("#SaveAndExitBtn").prop("disabled", true);
        }
    }
}
function OnClickSaveAndExit() {
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    //var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();

    debugger;
    let NewArr = [];
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    if (FTaxDetails > 0) {
        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text();
            //var TaxRowID = currentRow.find("#RowSNo").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = currentRow.find("#TaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
            if (/*TaxUserID == UserID && TaxRowID == RowSNo &&*/ TaxItemID == TaxItmCode) {
                currentRow.remove();
            }
            else {

                NewArr.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
            }
        });
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            $("#Hdn_TaxCalculatorTbl > tbody").append(`
                                <tr>
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxName}</td>
                                    <td id="TaxNameID">${TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                </tr>`)
            //TotalTaxAmount = parseFloat(TotalTaxAmount)+parseFloat(TaxAmount);
            NewArr.push({ /*UserID: UserID, RowSNo: RowSNo, */TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        //sessionStorage.removeItem("TaxCalcDetails");
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(NewArr));
        BindTaxAmountDeatils(NewArr);
    }
    else {
        var TaxCalculationList = [];
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            $("#Hdn_TaxCalculatorTbl > tbody").append(`
 <tr>
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxName}</td>
                                    <td id="TaxNameID">${TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                </tr>
`)
            TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationList));
        BindTaxAmountDeatils(TaxCalculationList);
    }
    $("#JoItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        if (currentRow.find("#SNohiddenfiled").val() == RowSNo) {
           
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = parseFloat(0).toFixed(DecDigit);
            }
            if (currentRow.find("#TaxExempted").is(":checked")) {
                TaxAmt = (parseFloat(0)).toFixed(DecDigit);
            }
            var price = currentRow.find("#item_rate").val();
            if (price == null || price=="") {
                TaxAmt = parseFloat(0).toFixed(DecDigit);
                currentRow.find("#item_tax_amt").val(TaxAmt);
            }
            else {
                currentRow.find("#item_tax_amt").val(TaxAmt);
            }
            OC_Amt = (parseFloat(0)).toFixed(DecDigit);
            if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
            }
            if (price == null || price == "") {
                //AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                AssessableValue = parseFloat(0).toFixed(DecDigit);
                //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                //currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                currentRow.find("#item_net_val").val(NetOrderValueBase);
            }
            else {
                AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);

                //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                //currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                currentRow.find("#item_net_val").val(NetOrderValueBase);
            }

        }
        var TaxAmt3 = parseFloat(0).toFixed(DecDigit)
        var ItemTaxAmt3 = currentRow.find("#item_tax_amt").val();
        if (ItemTaxAmt3 != TaxAmt3) {
            currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
        }
    });
    CalculateAmount();
}
function OnClickReplicateOnAllItems() {
    var errorflag = "N";
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmRowSNo = RowSNo;
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    //var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    debugger;
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    $("#Hdn_TaxCalculatorTbl > tbody > tr").remove();
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();

        TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            $("#JoItmDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemCode;
                var AssessVal;
                //debugger;
                ItemCode = currentRow.find("#JOItemListName" + Sno).val();
                AssessVal = currentRow.find("#item_gr_val").val();
                //if (CheckPOItemValidations() == false) {
                //    errorflag = "Y"
                //}
                var AssessValcheck = parseFloat(AssessVal);
                if (AssessVal != "" && AssessValcheck > 0) {
                    var NewArray = [];
                    var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);

                    for (i = 0; i < TaxCalculationList.length; i++) {
                        var TaxPercentage = "";
                        var TaxName = TaxCalculationList[i].TaxName;
                        var TaxNameID = TaxCalculationList[i].TaxNameID;
                        var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                        var RowSNo = TaxCalculationList[i].RowSNo;
                        TaxPercentage = TaxCalculationList[i].TaxPercentage;
                        var TaxLevel = TaxCalculationList[i].TaxLevel;
                        var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                        var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
                        var TaxAmount;
                        var TaxPec;
                        TaxPec = TaxPercentage.replace('%', '');

                        if (TaxApplyOn == "Immediate Level") {
                            if (TaxLevel == "1") {
                                TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                            }
                            else {
                                var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                                var TaxLevelTbl = parseInt(TaxLevel) - 1;
                                for (j = 0; j < NewArray.length; j++) {
                                    var Level = NewArray[j].TaxLevel;
                                    var TaxAmtLW = NewArray[j].TaxAmount;
                                    if (TaxLevelTbl == Level) {
                                        TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                    }
                                }
                                var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
                                TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                            }
                        }
                        if (TaxApplyOn == "Cummulative") {
                            var Level = TaxLevel;
                            if (TaxLevel == "1") {
                                TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                            }
                            else {
                                var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

                                for (j = 0; j < NewArray.length; j++) {
                                    var Level = NewArray[j].TaxLevel;
                                    var TaxAmtLW = NewArray[j].TaxAmount;
                                    if (TaxLevel != Level) {
                                        TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                    }
                                }
                                var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                                TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                            }
                        }
                        NewArray.push({ /*UserID: UserID, RowSNo: Sno,*/ TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                    }
                    if (NewArray != null) {
                        if (NewArray.length > 0) {
                            for (k = 0; k < NewArray.length; k++) {
                                var TaxName = NewArray[k].TaxName;
                                var TaxNameID = NewArray[k].TaxNameID;
                                var TaxItmCode = NewArray[k].TaxItmCode;
                                var RowSNo = NewArray[k].RowSNo;
                                var TaxPercentage = NewArray[k].TaxPercentage;
                                var TaxLevel = NewArray[k].TaxLevel;
                                var TaxApplyOn = NewArray[k].TaxApplyOn;
                                var TaxAmount = NewArray[k].TaxAmount;
                                var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                                if (CitmTaxItmCode != TaxItmCode && CitmRowSNo != RowSNo) {
                                    //    $("#Hdn_TaxCalculatorTbl > tbody").append(`
                                    // <tr>
                                    //    <td id="UserID">${UserID}</td>
                                    //    <td id="RowSNo">${RowSNo}</td>
                                    //    <td id="TaxItmCode">${TaxItmCode}</td>
                                    //    <td id="TaxName">${TaxName}</td>
                                    //    <td id="TaxNameID">${TaxNameID}</td>
                                    //    <td id="TaxPercentage">${TaxPercentage}</td>
                                    //    <td id="TaxLevel">${TaxLevel}</td>
                                    //    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    //    <td id="TaxAmount">${TaxAmount}</td>
                                    //    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    //    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                    //</tr>`)
                                    TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                        }
                    }
                }

            });
        }
    }
    if (TaxCalculationListFinalList.length > 0) {
        // sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
        for (var j = 0; j < TaxCalculationListFinalList.length; j++) {
            $("#Hdn_TaxCalculatorTbl > tbody").append(`
                                 <tr>
                                   <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxCalculationListFinalList[j].TaxItmCode}</td>
                                    <td id="TaxName">${TaxCalculationListFinalList[j].TaxName}</td>
                                    <td id="TaxNameID">${TaxCalculationListFinalList[j].TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxCalculationListFinalList[j].TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxCalculationListFinalList[j].TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxCalculationListFinalList[j].TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxCalculationListFinalList[j].TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TaxCalculationListFinalList[j].TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxCalculationListFinalList[j].TaxApplyOnID}</td>
                                </tr>`)
        }

        BindTaxAmountDeatils(TaxCalculationListFinalList);
    } else {
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }

    //debugger;
    $("#JoItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //debugger;
        //var Sno = currentRow.find("#SNohiddenfiled").val();
        var hfItemID = currentRow.find("#hfItemID").val();
        if (TaxCalculationListFinalList != null) {
            if (TaxCalculationListFinalList.length > 0) {
                for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                    var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                    //var RowSNoF = TaxCalculationListFinalList[i].RowSNo;
                    var txtTaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;

                    if (hfItemID == txtTaxItmCode) {

                        TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                        currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                        var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                        }
                        AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                        //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                        NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                        //currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                        currentRow.find("#item_net_val").val(NetOrderValueBase);
                    }
                }
            }
            else {
                var GrossAmtOR = parseFloat(0).toFixed(DecDigit);
                if (currentRow.find("#item_gr_val").val() != "") {
                    GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(DecDigit);
                }
                currentRow.find("#item_tax_amt").val(TaxAmt);
                var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                    OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                }
                var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                //currentRow.find("#item_ass_val").val(GrossAmtOR);
                //currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                currentRow.find("#item_net_val").val(FGrossAmtOR);

            }
        }
    });
    CalculateAmount();
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var UserID = $("#UserID").text();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                var currentRow = $(this);
                //var TaxUserID = currentRow.find("#UserID").text();
                //var TaxRowID = currentRow.find("#RowSNo").text();
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
                if ( /*TaxUserID == UserID &&TaxRowID == RowSNo &&*/ TaxItemID == ItmCode) {

                    var TaxNameID = currentRow.find("#TaxNameID").text();
                    var TaxLevel = currentRow.find("#TaxLevel").text();
                    var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                    var TaxPercentage = "";
                    TaxPercentage = currentRow.find("#TaxPercentage").text();
                    var TaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');

                    if (TaxApplyOn == "Immediate Level") {
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    //debugger;
                    currentRow.find("#TaxAmount ").text(TaxAmount);
                    currentRow.find("#TotalTaxAmount ").text(TotalTaxAmt);
                }
                var TaxName = currentRow.find("#TaxName").text();
                var TaxNameID = currentRow.find("#TaxNameID").text();
                var TaxPercentage = currentRow.find("#TaxPercentage").text();
                var TaxLevel = currentRow.find("#TaxLevel").text();
                var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                var TaxAmount = currentRow.find("#TaxAmount").text();
                var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();

                NewArray.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });

            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                //var TaxUserID = currentRow.find("#UserID").text();
                //var TaxRowID = currentRow.find("#RowSNo").text();
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if ( /*TaxUserID == UserID &&TaxRowID == RowSNo &&*/ TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            //$("#PoItmDetailsTbl >tbody >tr #POItemListName" + RowSNo + ":contains(" + ItmCode + ")").parent().parent().each(function () {
            $("#JoItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").parent().parent().each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;

                ItemNo = currentRow.find("#JOItemListName" + Sno).val();
                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = (parseFloat(0)).toFixed(DecDigit);
                            }
                            var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                            var TaxAmt = parseFloat(0).toFixed(DecDigit)
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(DecDigit)
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                                }
                                AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                               // NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                //currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                                currentRow.find("#item_net_val").val(NetOrderValueBase);

                                //}
                            }
                        });
                    }
                    else {
                        var TaxAmt = parseFloat(0).toFixed(DecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(DecDigit);
                        currentRow.find("#item_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                        //currentRow.find("#item_ass_val").val(GrossAmtOR);
                        //currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                        currentRow.find("#item_net_val").val(FGrossAmtOR);
                    }
                }
            });
            CalculateAmount();
            //sessionStorage.removeItem("TaxCalcDetails");
            //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(FTaxDetailsItemWise));
            BindTaxAmountDeatils(NewArray);
        }
    }
}
function BindTaxAmountDeatils(TaxAmtDetail) {
    debugger;

    var JO_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var JO_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";

    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, JO_ItemTaxAmountList, JO_ItemTaxAmountTotal);
}
function AfterDeleteResetJO_ItemTaxDetail() {
    //debugger;
    var JoItmDetailsTbl = "#JoItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var JOItemListName = "#JOItemListName";
    CMNAfterDeleteReset_ItemTaxDetailModel(JoItmDetailsTbl, SNohiddenfiled, JOItemListName);
}

//------------------End------------------//


//------------------Tax & Other Charge Section End------------------//
function OnChangeValidUpToDateJO(ValidUpToDate) {
    //debugger;
    var ValidUpTo = ValidUpToDate.value;
    var JODate = $("#jo_date").val();
    try {
        if (JODate > ValidUpTo) {
            $("#ValidUptoDateJO").val("");
            $('#SpanValidUpToErrorMsg').text($("#VDateCNBGTPODate").text());
            $("#SpanValidUpToErrorMsg").css("display", "block");
            return false;
        }
        else {
            $("#SpanValidUpToErrorMsg").css("display", "none");
            $("#ValidUptoDateJO").css("border-color", "#ced4da");
        }

    } catch (err) {

    }
}

function SaveBtnClick() {
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    if (JOHeaderValidations() == false) {
        return false;
    }
    if (JOItemValidations() == false) {
        return false;
    }
    var hdnsrctyp = $("#Hdn_JOSrcType").val();
    if (hdnsrctyp == "Direct") {
        if (JOInputItemValidations() == false) {
            return false;
        }
        var FlagInpuItm="JOInpuItm"
        if (CheckValidations_forSubItems(FlagInpuItm) == false) {
            return false;
        }
        
    }
    
    //if (Cmn_CheckDeliverySchdlValidations("JoItmDetailsTbl", "hfItemID", "JOItemListName", "OrderQuantity", "SNohiddenfiled") == false) {
    //    return false;
    //}
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        debugger;
        if (Cmn_taxVallidation("JoItmDetailsTbl", "item_tax_amt", "JOItemListName", "Hdn_TaxCalculatorTbl", "SNohiddenfiled") == false) {
            return false;
        }
    }
    if (hdnsrctyp == "Direct") {
        if (CheckDeliverySchdlValidations_ForDirect("hdn_Fproduct_id", "hdn_Fproduct_name", "JOOrdQuantity") == false) {
            return false;
        }
    }
    else {
        if (Cmn_CheckDeliverySchdlValidations("JoOutputItmDetailsTbl", "hfItemID", "JOOutputItemListName-" + "ItmTypID", "OrderQuantity", "SNohiddenfiled") == false) {
            return false;
        }
    }
    
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {

        
        var JONum = $("#JobOrderNumber").val(); 
        var hdAmend = $("#hdAmend").val();
        if (JONum == null || JONum == "") {
            
            $("#hdtranstype").val("Save");
        }
       else {
            $("#hdtranstype").val("Update");
        }
            
        var FinalJOItemDetail = [];
        var FinalJOOutputItemDetail = [];
        var FinalJOInputItemDetail = [];
        var FinalJODeliveryDetail = [];
        var FinalJOTaxDetail = [];
        var FinalJOOCDetail = [];
        var FinalJOTermDetail = [];

        FinalJOItemDetail = InsertJOItemDetails();
        FinalJOOutputItemDetail = InsertJOOutputItemDetails();
        FinalJOInputItemDetail = InsertJOInputItemDetails();
        FinalJOTaxDetail = InsertJOTaxDetails();
        FinalJOOCDetail = InsertJOOtherChargeDetails();
        FinalJODeliveryDetail = InsertJOItem_DeliverySchDetails();
        FinalJOTermDetail = InsertJOTermConditionDetails();

        $("#hdItemDetailList").val(JSON.stringify(FinalJOItemDetail));
        $("#hdOutputItemDetailList").val(JSON.stringify(FinalJOOutputItemDetail));
        $("#hdInputItemDetailList").val(JSON.stringify(FinalJOInputItemDetail));
        $("#hdTaxDetailList").val(JSON.stringify(FinalJOTaxDetail));
        $("#hdOCDetailList").val(JSON.stringify(FinalJOOCDetail));
        $("#hdDelSchDetailList").val(JSON.stringify(FinalJODeliveryDetail));
        $("#hdTermsDetailList").val(JSON.stringify(FinalJOTermDetail));
        if (hdnsrctyp == "Direct") {
            var SubItemsListArr = InsertJOSubItemList_ForDirect();
            var str2 = JSON.stringify(SubItemsListArr);
            $('#SubItemDetailsDt').val(str2);
           
        }
        else {
            /*----- Sub Item Detail start--------*/
            var SubItemsListArr = Cmn_SubItemList();
            var str2 = JSON.stringify(SubItemsListArr);
            $('#SubItemDetailsDt').val(str2);
        /*----- Sub Item Detail End--------*/
        }
       

        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        var FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);

        $("#hdn_Attatchment_details").val(ItemAttchmentDt);
        /*----- Attatchment End--------*/


        $("#Hdn_SupplierName").val($("#SupplierName").val());
        var PrdNo = $("#Hdn_ProducNum").val();
        $("#Hdn_ProducNum").val(PrdNo);
        var hedrPrdNo = $("#hdn_Fproduct_name").val();
        $("#ddl_FProductName").val(hedrPrdNo);
        debugger;
        var SuppName = $("#SupplierName option:selected").text();
        $("#Hdn_JOSupplierName").val(SuppName);
        var ProductionOrdrNum = $("#Prod_Ord_number option:selected").text();
        $("#HdnProducNumbr").val(ProductionOrdrNum);

        $("#hdnsavebtn").val("AllreadyclickSaveBtn");

      return true;

    }
}
function JOHeaderValidations() {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    var suppname = $("#SupplierName").val();
 
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");

        if ($("#Address").val() === "" || $("#Address").val() === null) {
            $('#SpanSuppAddrErrorMsg').text($("#valueReq").text());
            $("#Address").css("border-color", "Red");
            $("#SpanSuppAddrErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("#SpanSuppAddrErrorMsg").css("display", "none");
            $("#Address").css("border-color", "#ced4da");
        }
    }

    if ($("#ValidUptoDateJO").val() == "" || $("#ValidUptoDateJO").val() == null) {
        $('#SpanValidUpToErrorMsg').text($("#valueReq").text());
        $("#SpanValidUpToErrorMsg").css("display", "block");
        $("#ValidUptoDateJO").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanValidUpToErrorMsg").css("display", "none");
        $("#ValidUptoDate").css("border-color", "#ced4da");
    }
    debugger;
    if (suppname != "0") {
        var srctyp = $("#Hdn_JOSrcType").val();
        if (srctyp == "PrdOrd") {
            if ($("#Prod_Ord_number").val() == "0" || $("#Prod_Ord_number").val() == "---Select----0-0-0") {
                $("#SpanProdOrdNoErrorMsg").text($("#valueReq").text());
                $("[aria-labelledby='select2-Prod_Ord_number-container']").css("border-color", "red")
                $("#SpanProdOrdNoErrorMsg").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                $("#SpanProdOrdNoErrorMsg").css("display", "none");
                $("#Prod_Ord_number").css("border-color", "#ced4da");
                $("[aria-labelledby='select2-Prod_Ord_number-container']").css("border-color", "#ced4da");
            }
        }
        else {
            if ($("#ddl_FProductName").val() == "0" || $("#ddl_FProductName").val() == "") {
                $("#vmddl_FProductName").text($("#valueReq").text());
                $("[aria-labelledby='select2-ddl_FProductName-container']").css("border-color", "red")
                $("#vmddl_FProductName").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                $("#vmddl_FProductName").css("display", "none");
                $("#ddl_FProductName").css("border-color", "#ced4da");
                $("[aria-labelledby='select2-ddl_FProductName-container']").css("border-color", "#ced4da");
            }
            if ($("#JOOrdQuantity").val() === "" || $("#JOOrdQuantity").val() === null) {
                $('#SpanJOOrdQtyErrorMsg').text($("#valueReq").text());
                $("#JOOrdQuantity").css("border-color", "Red");
                $("#SpanJOOrdQtyErrorMsg").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                $("#SpanJOOrdQtyErrorMsg").css("display", "none");
                $("#JOOrdQuantity").css("border-color", "#ced4da");
                var FlagHdr="JoHedr"
            }
            var itmweight = $("#JOItmWeight").val();
            if (itmweight === "" || itmweight === null || parseFloat(itmweight).toFixed(QtyDecDigit) === parseFloat(0).toFixed(QtyDecDigit)) {
                $('#SpanJOItmWghtErrorMsg').text($("#valueReq").text());
                $("#JOItmWeight").css("border-color", "Red");
                $("#SpanJOItmWghtErrorMsg").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                $("#SpanJOItmWghtErrorMsg").css("display", "none");
                $("#JOItmWeight").css("border-color", "#ced4da");
            }
            if (CheckValidations_forSubItems(FlagHdr) == false) {
                return false;
            }
        }
        
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function JOItemValidations1() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#JoItmDetailsTbl >tbody >tr").length > 0) {
        $("#JoItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            
            if (currentRow.find("#item_rate").val() == "") {
                currentRow.find("#item_rateError").text($("#valueReq").text());
                currentRow.find("#item_rateError").css("display", "block");
                currentRow.find("#item_rate").css("border-color", "red");
                if (currentRow.find("#ord_qty_spec").val() != "" && currentRow.find("#ord_qty_spec").val() != null) {
                    currentRow.find("#item_rate").focus();
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#item_rateError").css("display", "none");
                currentRow.find("#item_rate").css("border-color", "#ced4da");
            }
            if (currentRow.find("#item_rate").val() != "") {
                if (parseFloat(currentRow.find("#item_rate").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#item_rateError").text($("#valueReq").text());
                    currentRow.find("#item_rateError").css("display", "block");
                    currentRow.find("#item_rate").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_rateError").css("display", "none");
                    currentRow.find("#item_rate").css("border-color", "#ced4da");
                }
            }
            
        });
    }
  
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function JOItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#JoItmDetailsTbl >tbody >tr").length > 0) {
        $("#JoItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#JOItemListName" + Sno).val() == "0") {
                currentRow.find("#JOItemListNameError").text($("#valueReq").text());
                currentRow.find("#JOItemListNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-JOItemListName" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#JOItemListNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#item_rate").val() == "") {
                currentRow.find("#item_rateError").text($("#valueReq").text());
                currentRow.find("#item_rateError").css("display", "block");
                currentRow.find("#item_rate").css("border-color", "red");
                if (currentRow.find("#ord_qty_spec").val() != "" && currentRow.find("#ord_qty_spec").val() != null) {
                    currentRow.find("#item_rate").focus();
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#item_rateError").css("display", "none");
                currentRow.find("#item_rate").css("border-color", "#ced4da");
            }
            if (currentRow.find("#item_rate").val() != "") {
                if (parseFloat(currentRow.find("#item_rate").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#item_rateError").text($("#valueReq").text());
                    currentRow.find("#item_rateError").css("display", "block");
                    currentRow.find("#item_rate").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_rateError").css("display", "none");
                    currentRow.find("#item_rate").css("border-color", "#ced4da");
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
function JOInputItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#JoInputItmDetailsTbl >tbody >tr").length > 0) {
        $("#JoInputItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#JOInputSNohiddenfiled").val();

            if (currentRow.find("#JOInputItemListName" + Sno).val() == "0") {
                currentRow.find("#JOInputItemListNameError").text($("#valueReq").text());
                currentRow.find("#JOInputItemListNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-JOInputItemListName" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#JOInputItemListNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#JOInputRequiredQty").val() == "") {
                currentRow.find("#JOInputRequiredQtyError").text($("#valueReq").text());
                currentRow.find("#JOInputRequiredQtyError").css("display", "block");
                currentRow.find("#JOInputRequiredQty").css("border-color", "red");
                //if (currentRow.find("#ord_qty_spec").val() != "" && currentRow.find("#ord_qty_spec").val() != null) {
                //    currentRow.find("#item_rate").focus();
                //}
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#JOInputRequiredQtyError").css("display", "none");
                currentRow.find("#JOInputRequiredQty").css("border-color", "#ced4da");
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
function InsertJOItemDetails() {
    debugger;
    var JOItemList = [];
    $("#JoItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var ItemID = "";
        var ItemName = "";
        
        var UOMID = null;
        var OrderQty = "";
        var ItmRate = "";
        var GrossVal = "";
        var TaxAmt = "";
        var OCAmt = "";
        var NetVal = "";
        var SimpleIssue = "";
        var MRSNo = "";
        var Remarks = "";
        var ItemType = "SR";
        var TaxExempted = "";
        var ManualGST = "";

        var currentRow = $(this);
        var SNo = currentRow.find("#SNohiddenfiled").val();

        ItemID = currentRow.find("#hfItemID").val();
        ItemName = currentRow.find("#JOItemListName" + SNo + " option:selected").text();
        //UOMID = currentRow.find("#UOMID").val();
       
        OrderQty = currentRow.find("#OrdQtyServc").val();
        ItmRate = currentRow.find("#item_rate").val();
        GrossVal = currentRow.find("#item_gr_val").val();
     
        if (currentRow.find("#item_tax_amt").val() === "" || currentRow.find("#item_tax_amt").val() === null) {
            TaxAmt = "0";
        }
        else {
            TaxAmt = currentRow.find("#item_tax_amt").val();
        }
        if (currentRow.find("#item_oc_amt").val() === "" || currentRow.find("#item_oc_amt").val() === null) {
            OCAmt = "0";
        }
        else {
            OCAmt = currentRow.find("#item_oc_amt").val();
        }

        NetVal = currentRow.find("#item_net_val").val();
        
        if (currentRow.find("#SimpleIssue").is(":checked")) {
            SimpleIssue = "Y";
        }
        else {
            SimpleIssue = "N";
        }

        MRSNo = currentRow.find("#MRSNumber").val();
        Remarks = currentRow.find("#remarks").val();
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TaxExempted = "Y"
        }
        else {
            TaxExempted = "N"
        }
        if (currentRow.find("#ManualGST").is(":checked")) {
            ManualGST = "Y"
        }
        else {
            ManualGST = "N"
        }
        var hsn_code = currentRow.find("#ItemHsnCode").val();
        JOItemList.push({ ItemID: ItemID, ItemName: ItemName, UOMID: UOMID, OrderQty: OrderQty, ItmRate: ItmRate, GrossVal: GrossVal, TaxAmt: TaxAmt, OCAmt: OCAmt, NetVal: NetVal, SimpleIssue: SimpleIssue, MRSNo: MRSNo, Remarks: Remarks, ItemType: ItemType, TaxExempted: TaxExempted, ManualGST: ManualGST, hsn_code: hsn_code });
    });

    return JOItemList;
};
function InsertJOOutputItemDetails() {
    debugger;
    //var itmtype = $("#ItmTypID").val();
    var JOOutputItemList = [];
    var srctyp = $("#Hdn_JOSrcType").val();
    if (srctyp == "PrdOrd") {
        $("#JoOutputItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var ItemID = "";
            var ItemName = "";
            var subitem = "";
            var UOMID = "";
            var UOMName = "";
            var OrdQty = "";
            var ItmRate = "0";
            var GrossVal = "0";
            var TaxAmt = "0";
            var OCAmt = "0";
            var NetVal = "0";
            var SimpleIssue = "N";
            var MRSNo = "0";
            var Remarks = "0";
            var ItemType = "";


            var currentRow = $(this);
            var SNo = currentRow.find("#SNohiddenfiled").val();
            ItemID = currentRow.find("#hfItemID").val();
            ItemName = currentRow.find("#hdnOutItemName").val();
            subitem = currentRow.find("#JoOutsub_item").val();
            UOMID = currentRow.find("#UOMID").val();
            UOMName = currentRow.find("#UOM").val();
            debugger;
            OrdQty = currentRow.find("#OrderQuantity").text().trim();
            ItemType = currentRow.find("#ItmTypID").val();
            JOOutputItemList.push({ ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID, UOMName: UOMName, OrdQty: OrdQty, ItmRate: ItmRate, GrossVal: GrossVal, TaxAmt: TaxAmt, OCAmt: OCAmt, NetVal: NetVal, SimpleIssue: SimpleIssue, MRSNo: MRSNo, Remarks: Remarks, ItemType: ItemType });
        });
    }
    else {
        var ItemID = "";
        var ItemName = "";
        var subitem = "";
        var UOMID = "";
        var UOMName = "";
        var OrdQty = "";
        var ItmRate = "0";
        var GrossVal = "0";
        var TaxAmt = "0";
        var OCAmt = "0";
        var NetVal = "0";
        var SimpleIssue = "N";
        var MRSNo = "0";
        var Remarks = "0";
        var ItemType = "OF";

        debugger;
        ItemID = $("#hdn_Fproduct_id").val();
        ItemName = $("#hdn_Fproduct_name").val();
        subitem = "N";
        UOMID = $("#Hdn_FinishUomId").val();
        UOMName = $("#TxtFinishedUom").val();
        OrdQty = $("#JOOrdQuantity").val();
        
        JOOutputItemList.push({ ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID, UOMName: UOMName, OrdQty: OrdQty, ItmRate: ItmRate, GrossVal: GrossVal, TaxAmt: TaxAmt, OCAmt: OCAmt, NetVal: NetVal, SimpleIssue: SimpleIssue, MRSNo: MRSNo, Remarks: Remarks, ItemType: ItemType });
    }

    return JOOutputItemList;
};
function InsertJOInputItemDetails() {
    debugger;
    var JOInputItemList = [];
    var srctyp = $("#Hdn_JOSrcType").val();
    $("#JoInputItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var ItemID = "";
        var ItemName = "";
        var subitem = "";
        var UOMID = "";
        var UOMName = "";
        var ReqQty = "";
        var ItmType = "";
        var ItmTypeName = "";
       

        var currentRow = $(this);
        if (srctyp == "PrdOrd") {
            var SNo = currentRow.find("#SNohiddenfiled").val();
            ItemID = currentRow.find("#hfItemID").val();
            ItemName = currentRow.find("#hdnInItemName").val();
            subitem = currentRow.find("#JoInsub_item").val();
            UOMID = currentRow.find("#UOMID").val();
            UOMName = currentRow.find("#UOM").val();
            ItmType = currentRow.find("#ItemTypeID").val();
            ItmTypeName = currentRow.find("#ItemType").text();
            ReqQty = currentRow.find("#RequiredQuantity").text().trim();
        }
        else {
            var SNo = currentRow.find("#JOInputSNohiddenfiled").val();
            ItemID = currentRow.find("#JOInput_hfItemID").val();
            ItemName = currentRow.find("#JOInput_hfItemName").val();
            subitem = "N";
            UOMID = currentRow.find("#JOInputUOMID").val();
            UOMName = currentRow.find("#JOInputUOM").val();
            ItmType = "";
            ItmTypeName = "";
            ReqQty = currentRow.find("#JOInputRequiredQty").val().trim();
        }
        
        JOInputItemList.push({ ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID, UOMName: UOMName, ItmType: ItmType, ItmTypeName: ItmTypeName, ReqQty: ReqQty });
    });
    return JOInputItemList;
};


function InsertJOTaxDetails() {
    debugger
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var JOTaxList = [];
    $("#JoItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            var ItmCode = currentRow.find("#JOItemListName" + RowSNo).val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var Crow = $(this);
                        var ItemID = Crow.find("#TaxItmCode").text().trim();
                        var TaxID = Crow.find("#TaxNameID").text().trim();
                        var TaxName = Crow.find("#TaxName").text().trim();
                        var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                        var TaxLevel = Crow.find("#TaxLevel").text().trim();
                        var TaxValue = Crow.find("#TaxAmount").text().trim();
                        var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                        var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                        var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();
                        JOTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxName: TaxName, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount });
                    });
                }
            }
        }
    });
    return JOTaxList;
    //if (FTaxDetails != null) {
    //    if (FTaxDetails > 0) {
    //        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
    //            var Crow = $(this);
    //            var ItemID = Crow.find("#TaxItmCode").text().trim();
    //            var TaxID = Crow.find("#TaxNameID").text().trim();
    //            var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
    //            var TaxLevel = Crow.find("#TaxLevel").text().trim();
    //            var TaxValue = Crow.find("#TaxAmount").text().trim();
    //            var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
    //            JOTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn });
    //        });
    //    }
    //}
};
function InsertJOOtherChargeDetails() {
    debugger;
    var JO_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var OC_ID = "";
            var OCValue = "";

            var OCName = "";
            var OC_Curr = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            var OC_TaxAmt = "";
            var OC_TotlAmt = "";

            OC_ID = currentRow.find("#OCValue").text();
            OCValue = currentRow.find("#OCAmount").text();

            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            OC_TaxAmt = currentRow.find("#OCTaxAmt").text();
            OC_TotlAmt = currentRow.find("#OCTotalTaxAmt").text();
            JO_OCList.push({ OC_ID: OC_ID, OCValue: OCValue, OCName: OCName, OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs, OC_TaxAmt: OC_TaxAmt, OC_TotlAmt: OC_TotlAmt });
        });
    }
    return JO_OCList;
};
function InsertJOItem_DeliverySchDetails() {
    var JODelieryList = [];
    if ($("#DeliverySchTble >tbody >tr").length > 0) {
        $("#DeliverySchTble >tbody >tr").each(function () {
            var currentRow = $(this);
            var ItemID = "";
            var ItemName = "";
            var SchDate = "";
            var DeliveryQty = "";
            var RecQty = "";
            var PendingQty = "";
            ItemID = currentRow.find("#Hd_ItemIdFrDS").text();
            ItemName = currentRow.find("#Hd_ItemNameFrDS").text();
            DeliveryQty = currentRow.find("#delv_qty").text();
            SchDate = currentRow.find("#sch_date").text();
            RecQty = currentRow.find("#SRQty").text();
            PendingQty = currentRow.find("#PenQty").text();

            JODelieryList.push({ ItemID: ItemID, ItemName: ItemName, SchDate: SchDate, DeliveryQty: DeliveryQty });
        });
    }
    return JODelieryList;
};
function InsertJOTermConditionDetails() {
    var JOTermsList = [];
    if ($("#TblTerms_Condition >tbody >tr").length > 0) {
        $("#TblTerms_Condition >tbody >tr").each(function () {
            var currentRow = $(this);
            var TermsDesc = "";
            TermsDesc = currentRow.find("#term_desc").text();
            JOTermsList.push({ TermsDesc: TermsDesc });
        });
    }
    return JOTermsList;
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

/*-----------Start Workflow--------------*/
function ForwardBtnClick() {
    debugger;
    try {
        var JOStatus = "";
        JOStatus = $('#hfStatus').val().trim();
        if (JOStatus === "D" || JOStatus === "F") {

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
    /*Commented by Hina sharma on 30-05-2025 as per discuss with vishal sir*/
    ///*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
    //var compId = $("#CompID").text();
    //var brId = $("#BrId").text();
    //var joDt = $("#jo_date").val();
    //$.ajax({
    //    type: "POST",
    //    /* url: "/Common/Common/CheckFinancialYear",*/
    //    url: "/Common/Common/CheckFinancialYearAndPreviousYear",
    //    data: {
    //        compId: compId,
    //        brId: brId,
    //        DocDate: joDt
    //    },
    //    success: function (data) {
    //        /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
    //        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
    //        if (data == "TransAllow") {
    //            var JOStatus = "";
    //            JOStatus = $('#hfStatus').val().trim();
    //            if (JOStatus === "D" || JOStatus === "F") {

    //                if ($("#hd_nextlevel").val() === "0") {
    //                    $("#Btn_Forward").attr("data-target", "");
    //                }
    //                else {
    //                    $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //                    $("#Btn_Approve").attr("data-target", "");
    //                }
    //                var Doc_ID = $("#DocumentMenuId").val();
    //                $("#OKBtn_FW").attr("data-dismiss", "modal");

    //                Cmn_GetForwarderList(Doc_ID);

    //            }
    //            else {
    //                $("#Btn_Forward").attr("data-target", "");
    //                $("#Btn_Forward").attr('onclick', '');
    //                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //            }
    //        }
    //        else {/* to chk Financial year exist or not*/
    //            /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
    //            /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
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
 function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var JONo = "";
    var JODate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status = "";


    docid = $("#DocumentMenuId").val();
    JONo = $("#JobOrderNumber").val();
    JODate = $("#jo_date").val();
    WF_Status = $("#WF_Status1").val();
    $("#hdDoc_No").val(JONo);
    Remarks = $("#fw_remarks").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    //if ($("#OrderTypeD").is(":checked")) {
    //    InvType = "D";
    //}
    //if ($("#OrderTypeE").is(":checked")) {
    //    InvType = "E";
    //}
    //SaleVouMsg = $("#SaleVoucherPassAgainstInv").text()


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
        if (fwchkval != "" && JONo != "" && JODate != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(JONo, JODate, docid, level, forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/JobOrderSC/ToRefreshByJS";
            var list = [{ JONo: JONo, JODate: JODate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/JobOrderSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
    if (fwchkval === "Approve") {
        var list = [{ JONo: JONo, JODate: JODate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/JobOrderSC/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/JobOrderSC/SIListApprove?SI_No=" + JONo + "&SI_Date=" + JODate + "&InvType=" + InvType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&SaleVouMsg=" + SaleVouMsg;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && JONo != "" && JODate != "") {
            Cmn_InsertDocument_ForwardedDetail(JONo, JODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            var list = [{ JONo: JONo, JODate: JODate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/JobOrderSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && JONo != "" && JODate != "") {
             Cmn_InsertDocument_ForwardedDetail(JONo, JODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/JobOrderSC/ToRefreshByJS";
            var list = [{ JONo: JONo, JODate: JODate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/JobOrderSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#JobOrderNumber").val();
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
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return SaveBtnClick()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
function CheckedFClose() {
    debugger;
    if ($("#ForceClosed1").is(":checked")) {
        $("#Cancelled").attr("disabled", true);
        $("#Cancelled").prop("checked", false);
       
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr('onclick', 'SaveBtnClick()');
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
/***--------------------------------Sub Item Section for Production Order and Direct case-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow, flag) {
    var srctyp = $("#Hdn_JOSrcType").val();
    if (srctyp == "Direct") {
        if (flag == "JOInputReq") {
            Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemJOReqQty",);
        }
        else {
            Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemHdrOrdQty",);
        }
        
    }
    else {
        Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemOrdQty",);
    }
   
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var Sub_Quantity = 0;
    var srctyp = $("#Hdn_JOSrcType").val(); 
    var HedrItemId = $("#hdn_Fproduct_id").val();
    var hdCommand = $("#hdCommand").val();
    //var hfsno = clickdRow.find("#hfsno").val();
    
    if (flag == "JOReqQty") {
        if (srctyp == "Direct") {
            var ProductNm = clickdRow.find("#JOInput_hfItemName").val();
            var ProductId = clickdRow.find("#JOInput_hfItemID").val();
            var UOM = clickdRow.find("#JOInputUOM").val();
            Sub_Quantity = clickdRow.find("#JOInputRequiredQty").val().trim();
            var Itemtyp = "SubRM"
            
        }
        else {
            var ProductNm = clickdRow.find("#hdnInItemName").val();
            var ProductId = clickdRow.find("#hfItemID").val();
            var UOM = clickdRow.find("#UOM").text();
            Sub_Quantity = clickdRow.find("#RequiredQuantity").text().trim();
        }
        
    }
    else if (flag == "JO_HdrOrdQty")
    {
        var ProductNm = $("#hdn_Fproduct_name").val();
        var ProductId = $("#hdn_Fproduct_id").val();
        var UOM = $("#TxtFinishedUom").val();
        Sub_Quantity = $("#JOOrdQuantity").val().trim();
        var decimalAllowed = "Y";
        var Itemtyp = "SubHedr"
         
    }
    else
    {
        ProductNm = clickdRow.find("#hdnOutItemName").val();
        ProductId = clickdRow.find("#hfItemID").val();
        UOM = clickdRow.find("#UOM").text();
        Sub_Quantity = clickdRow.find("#OrderQuantity").text().trim();
    }
    //var Sub_Quantity = 0;
    var NewArr = new Array();
    var NewArr1 = new Array();
    if (srctyp == "Direct" && flag =="JO_HdrOrdQty") {
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + ProductId + "]").closest('tr').find("#subItemType[value='" + Itemtyp + "']").closest('tr');
        //var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + ProductId + "]").closest('tr');
        rows.each(function () {
                var row = $(this);
                var List = {};
                List.item_id = row.find("#ItemId").val();
                List.sub_item_id = row.find('#subItemId').val();
                List.qty = row.find('#subItemQty').val();
                if (srctyp == "Direct") {
                    List.type = row.find('#subItemType').val();
                }
                else {
                    List.type = "";
                }
                NewArr.push(List);
        });
        
    }
    else if (srctyp == "Direct" && flag == "JOReqQty")
    {
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + ProductId + "]").closest('tr');
        var rows1 = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + ProductId + "]").closest('tr');
        rows1.each(function () {
            var row = $(this);
            var List = {};
            var subitmqty = row.find('#subItemQty').val();
            var subitmid = row.find('#subItemId').val();
            var subitmtyp1 = row.find('#subItemType').val();
            if (parseFloat(CheckNullNumber(row.find('#subItemQty').val())) > 0 && subitmtyp1 == "SubHedr" ) {
                List.item_id = row.find("#ItemId").val();
                List.sub_item_id = row.find('#subItemId').val();
                List.qty = row.find('#subItemQty').val();
                List.type = row.find('#subItemType').val();
                NewArr1.push(List);
                IsDisabled = "Y";
            }
            else {
                if (ProductId != HedrItemId) {
                    List.item_id = row.find("#ItemId").val();
                    List.sub_item_id = row.find('#subItemId').val();
                    List.qty = row.find('#subItemQty').val();
                    List.type = row.find('#subItemType').val();
                    NewArr1.push(List);
                     
                }
             }
        });
        if (NewArr1.length > 0) {
            
            rows.each(function () {
                var row = $(this);
                var List = {};
                var itmtyp = row.find("#subItemType").val();
                var subitmqty = row.find('#subItemQty').val();
                var subitmid = row.find('#subItemId').val();
                if (NewArr1.findIndex(v => v.sub_item_id == subitmid) > -1)
                {
                    if (parseFloat(CheckNullNumber(subitmqty)) > 0 && itmtyp == "SubHedr" || itmtyp == "SubRM")
                    {
                        var Item_Id = row.find("#ItemId").val();
                        var itmtyp = row.find("#subItemType").val();
                        List.item_id = row.find("#ItemId").val();
                        List.sub_item_id = row.find('#subItemId').val();
                        debugger;
                        /*List.qty = itmtyp == "SubHedr" ?"0":row.find('#subItemQty').val();*/
                        List.qty =  row.find('#subItemQty').val();
                        List.type = row.find('#subItemType').val();

                        NewArr.push(List);
                    }
                    
                }
                
            });
        }
        
    }
    else {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.type = row.find('#subItemType').val();
            
            NewArr.push(List);
        });
    }
    
    //}
    
    if (srctyp == "Direct") {
       var IsDisabled = $("#DisableSubItem").val();
    }
    else {
        var IsDisabled = "Y";
    }
   
    
   
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    if (hd_Status != "") {
        var jc_no = $("#Prod_Ord_number option:selected").text();
    }
    else {

        //var jc_no = $("#select2-Prod_Ord_number-container").text();
        //var jc_no = $("#Prod_Ord_number option:selected").text();
        var jc_no= $("#Hdn_ProducNum").val();
    }
   
    var JobOrdNo = $("#JobOrderNumber").val();
    var JobOrdDt = $("#jo_date").val();
    
    var jc_dt = $("#produc_ord_date").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/JobOrderSC/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            jc_no: jc_no,
            jc_dt: jc_dt,
            JobOrdNo: JobOrdNo,
            JobOrdDt: JobOrdDt,
            srctyp: srctyp,
            decimalAllowed: decimalAllowed,
            HedrItemId: HedrItemId
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

//----------------Sub Item Detail Only for Direct Case----------------------------
function fn_CustomJOSubItemDataForDirect(itemId, SubItmType) {
    debugger; 
   
    //$("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + itemId + "]").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr').remove();
    $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + itemId + "]").closest('tr').remove();

    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
            debugger;
            var Crow = $(this).closest("tr");
            //var ItemId = Crow.find("#ItemId").val();
            var subItemId = Crow.find("#subItemId").val();
            var subItemQty = Crow.find("#subItemQty").val();
            var subItem_Typ = Crow.find("#subItemType").val(SubItmType);
           
                var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr').length;
                var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr');


                if (rows.length > 0) {
                    rows.each(function () {
                        var InnerCrow = $(this).closest("tr");
                        debugger;
                        InnerCrow.find("#subItemQty").val(subItemQty);
                    });
                }
                else
                {
                    /*if (parseFloat(CheckNullNumber(row.find('#subItemQty').val())) > 0) {*/
                    $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                            <td><input type="text" id="subItemType" value='${SubItmType}'></td>
                            
                        </tr>`);
                    /*  }*/

                 }
           
           
                //var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + itemId + "]").closest('tr').find("#subItemId[value='" + subItemId + "']").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr').length;
                //var rows_sub = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + itemId + "]").closest('tr').find("#subItemId[value='" + subItemId + "']").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr');
                //$("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                //            <td><input type="text" id="ItemId" value='${itemId}'></td>
                //            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                //            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                //            <td><input type="text" id="subItemType" value='${SubItmType}'></td>
                            
                //</tr>`);
            

        });
    
  }
function CheckValidations_forSubItems(Flag) {
    if (Flag == "JoHedr") {
        $("#sub_itemJOHeadrFlag").val(Flag);
        return JOHeaderSubItemValidation("hdn_Fproduct_id", "JOOrdQuantity", "SubItemHdrOrdQty", "Y", "SubHedr");
    }
    else {
        return JOInputSubItemValidation("JoInputItmDetailsTbl", "JOInputSNohiddenfiled", "JOInputItemListName", "JOInputRequiredQty", "SubItemJOReqQty", "Y", "SubRM");
    }

    }
function ResetWorningBorderColor() {
    var HSubItmFlag = $("#sub_itemJOHeadrFlag").val();
    //var ReqMtrlSubItmFlag = $("#sub_itemReqMtrFlag").val();

    if (HSubItmFlag == "JoHedr") {
        JOHeaderSubItemValidation("hdn_Fproduct_id", "JOOrdQuantity", "SubItemHdrOrdQty", "N", "SubHedr");
        var JORMTbllen = $("#JoInputItmDetailsTbl>tbody>tr").length;
        if (JORMTbllen > 0) {
            return JOInputSubItemValidation("JoInputItmDetailsTbl", "JOInputSNohiddenfiled", "JOInputItemListName", "JOInputRequiredQty", "SubItemJOReqQty", "N", "SubRM")
        }
    }
}
function JOHeaderSubItemValidation(Item_field_id, Item_Qty_field_id, Button_id, ShowMessage, SubHedr) {
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
function JOInputSubItemValidation(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage, SubRM) {
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
        var sub_item = PPItemRow.find("#JoInsub_item").val();
        var Sub_Quantity = 0;
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubRM + "']").closest('tr');


        rows.each(function () {
            var Crow = $(this).closest("tr");
            //var subItemId = Crow.find("#subItemId").val();
            var subItemQty = Crow.find("#subItemQty").val();
            Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
        });


        if (sub_item == "Y") {
            var HedrItemId = $("#hdn_Fproduct_id").val();
            if (HedrItemId == item_id) {
                PPItemRow.find("#" + SubItemButton).css("border", "");
            }
            else {
                if (item_PrdQty != Sub_Quantity) {
                    flag = "Y";
                    PPItemRow.find("#" + SubItemButton).css("border", "1px solid red");
                } else {
                    PPItemRow.find("#" + SubItemButton).css("border", "");
                }
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
function DeleteInputSubItemQtyDetail(item_id, SubItmTyp) {
    debugger;
    if (item_id != null && item_id != "") {
        if ($("#hdn_Sub_ItemDetailTbl >tbody >tr").length > 0) {
            //if ($("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").length > 0) {
            //    $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').remove();
            //}
            if ($("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubItmTyp + "']").closest('tr').length > 0) {
                $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubItmTyp + "']").closest('tr').remove();
            }
        }

    }

}
function InsertJOSubItemList_ForDirect() {
    var NewArr = new Array();
    debugger;
    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        var Qty = row.find('#subItemQty').val();
        debugger;
        if (parseFloat(CheckNullNumber(Qty)) > 0) {
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.subItemTyp = row.find('#subItemType').val();
            NewArr.push(List);
        }
    });
    return NewArr;
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

function OnClickRemoveValidUpto() {
    debugger;
    var jodate = $("#jo_date").val();
    document.getElementById("ValidUptoDateJO").setAttribute('min', '');
}
/***---------------------Print Popup Start-------------------------------***/
function OnCheckedChangeProdDesc() {
    debugger;
    if ($('#chkproddesc').prop('checked')) {
        //$('#chkshowcustspecproddesc').prop('checked', false);
        //$('#ShowCustSpecProdDesc').val('N');
        $("#JO_ShowProdDesc").val("Y");
    }
    else {
        $("#JO_ShowProdDesc").val("N");
    }
}
function onCheckedChangeCustSpecProdDesc() {
    debugger;
    if ($('#chkshowcustspecproddesc').prop('checked')) {
        //$('#chkproddesc').prop('checked', false);
        //$("#ShowProdDesc").val("N");
        $('#JO_ShowCustSpecProdDesc').val('Y');
    }
    else {
        $('#JO_ShowCustSpecProdDesc').val('N');
    }
}
function OnCheckedChangeProdTechDesc() {
    debugger;
    if ($('#chkprodtechdesc').prop('checked')) {
        $('#JO_ShowProdTechDesc').val('Y');
    }
    else {
        $('#JO_ShowProdTechDesc').val('N');
    }
}
function OnCheckedChangeShowSubItem() {
    if ($('#chkshowsubitm').prop('checked')) {
        $('#JO_ShowSubItem').val('Y');
    }
    else {
        $('#JO_ShowSubItem').val('N');
    }
}
function onCheckedChangeFormatBtn() {
    if ($("#OrderTypeI").is(":checked")) {
        $('#JO_PrintFormat').val('F2');
    }
    else {
        $('#JO_PrintFormat').val('F1');
    }
}

//function OnCheckedChangeProdDesc() {
//    if ($('#chkproddesc').prop('checked')) {
//        $('#chkshowcustspecproddesc').prop('checked', false);
//        $('#ShowCustSpecProdDesc').val('N');
//        $("#JO_ShowProdDesc").val("Y");
//    }
//    else {
//        $("#JO_ShowProdDesc").val("N");
//    }
//}
//function onCheckedChangeCustSpecProdDesc() {
//    if ($('#chkshowcustspecproddesc').prop('checked')) {
//        $('#chkproddesc').prop('checked', false);
//        $("#ShowProdDesc").val("N");
//        $('#JO_ShowCustSpecProdDesc').val('Y');
//    }
//    else {
//        $('#JO_ShowCustSpecProdDesc').val('N');
//    }
//}
//function OnCheckedChangeProdTechDesc() {
//    if ($('#chkprodtechdesc').prop('checked')) {
//        $('#JO_ShowProdTechDesc').val('Y');
//    }
//    else {
//        $('#JO_ShowProdTechDesc').val('N');
//    }
//}
//function OnCheckedChangeShowSubItem() {
//    if ($('#chkshowsubitm').prop('checked')) {
//        $('#JO_ShowSubItem').val('Y');
//    }
//    else {
//        $('#JO_ShowSubItem').val('N');
//    }
//}
//function onCheckedChangeFormatBtn() {
//    if ($("#OrderTypeI").is(":checked")) {
//        $('#JO_PrintFormat').val('F2');
//    }
//    else {
//        $('#JO_PrintFormat').val('F1');
//    }
//}
/***---------------------Print Popup End-------------------------------***/