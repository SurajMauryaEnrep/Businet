$(document).ready(function () {
   // $('#ddlStatus').multiselect();
    $('#ddlSuppliersList').select2();
    $('#ddlItemsList').select2();
    //$('#All').click(function () {
    //    $('#ddlStatus option').prop('selected', true);
    //});
    Cmn_initializeMultiselect(['#ddlStatus']);// Added By Nidhi on 16-12-2025
});
function StatusOnclick() {
    debugger;
    var arr = [];
    arr=  $("#ddlStatus option:selected").val();
}
function StatusOnchange() {
    debugger;
    //var arr = [];
    //arr = $("#ddlStatus option:selected").val();
    var selected = [];
    var abc = "";
    $('#ddlStatus option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
        }
        else {
            abc +=  selected[$(this).text()] = ","+$(this).val();
        }
        var value = $(".checkbox").val();

       //$('#ddlStatus option').prop('selected', true);
       
        
    });
    $("#MultiselectStatusHdn").val(abc);
}
function BindItemWiseSummary() {
    debugger;
    var ReceiptType = $("#ReceiptType").val();
    if (ReceiptType == "GRN") {

        var fromDate = $("#txt_Fromdate").val();
        var toDate = $("#txt_Todate").val();
    }
    else {
        var fromDate = $("#txtFromdate").val();
        var toDate = $("#txtTodate").val();
    }
    var suppId = $("#ddlSuppliersList").val();
    var itemId = $("#ddlItemsList").val();
    var MultiselectStatusHdn = $("#MultiselectStatusHdn").val();
    try {
        $("#datatable-buttons2")[0].id = "dttbl2";
        $("#datatable-buttons3")[0].id = "dttbl3";
        $("#datatable-buttons4")[0].id = "dttbl4";
    }
    catch {}
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISGoodReceiptDetail/GetGRNMisItemWiseSummary",
        data: {
            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId, MultiselectStatusHdn: MultiselectStatusHdn
        },
        success: function (data) {
            debugger; 
            
            $("#GoodsReceiptSummaryItemWise").html(data);
            try {
                $("#dttbl2")[0].id = "datatable-buttons2";
                $("#dttbl3")[0].id = "datatable-buttons3";
                $("#dttbl4")[0].id = "datatable-buttons4";
            }
            catch {}

        }
    })
}
function BindMRSDetails() {
    debugger;
    var suppId = "";
    var EntityType = "";
    var ReceiptType = $("#ReceiptType").val();
    if (ReceiptType == "GRN") {

        var fromDate = $("#txt_Fromdate").val();
        var toDate = $("#txt_Todate").val();
    }
    else {
        var fromDate = $("#txtFromdate").val();
        var toDate = $("#txtTodate").val();
    }
    if (ReceiptType == "GRN") {
         suppId = $("#ddlSuppliersList").val();
    }
    else {
        //suppId = $("#ddl_EntityName").val();
         suppId = cmn_getddldataasstring("#ddl_EntityName");
         EntityType = $("#ddl_EntityType").val();
    }
 
    var itemId = $("#ddlItemsList").val();
   
    var MultiselectStatusHdn = $("#MultiselectStatusHdn").val();
   
    try {
        if (ReceiptType == "GRN") {
            
            $("#datatable-buttons1").attr("id", "dttbl1");
            $("#datatable-buttons2").attr("id", "dttbl2");
            $("#datatable-buttons3").attr("id", "dttbl3");
            $("#datatable-buttons4").attr("id", "dttbl4");
            //$("#datatable-buttons1")[0].id = "dttbl1";
            //$("#datatable-buttons2")[0].id = "dttbl2";
            //$("#datatable-buttons3")[0].id = "dttbl3";
            //$("#datatable-buttons4")[0].id = "dttbl4";
          
        }
        else {
            $("#datatable-buttons1").attr("id", "dttbl1");
            $("#datatable-buttons2").attr("id", "dttbl2");
            $("#datatable-buttons3").attr("id", "dttbl3");
            $("#datatable-buttons4").attr("id", "dttbl4");
            //$("#datatable-buttons1")[0].id = "dttbl1";
            //$("#datatable-buttons2")[0].id = "dttbl2";
            //$("#datatable-buttons3")[0].id = "dttbl3";
            //$("#datatable-buttons4")[0].id = "dttbl4";
           
        }
    }
    catch { }
  
  
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISGoodReceiptDetail/GetGRNMisDetails",
        data: {
            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId, MultiselectStatusHdn: MultiselectStatusHdn,
            ReceiptType: ReceiptType, EntityType: EntityType
        },
        success: function (data) {
            debugger;
            if (ReceiptType == "GRN") {
                try {
                    $("#GoodsReceiptDetail").html(data);
                    $("#dttbl1").attr("id", "datatable-buttons1");
                    $("#dttbl2").attr("id", "datatable-buttons2");
                    $("#dttbl3").attr("id", "datatable-buttons3");
                    $("#dttbl4").attr("id", "datatable-buttons4");
                }
                catch { }
               
            }
            else {
               
                $("#PartialMISExternalReceipt").html(data);
                $("#dttbl1").attr("id", "datatable-buttons1");
                $("#dttbl2").attr("id", "datatable-buttons2");
                $("#dttbl3").attr("id", "datatable-buttons3");
                $("#dttbl4").attr("id", "datatable-buttons4");
              
              
               
              
            }
           
           
        }
    })
}
function BindMrsMisDetails() {
    debugger;
    var showAs = $('#ShowAs').val();
    var ReceiptType = $("#ReceiptType").val();
    if (showAs == "S" && ReceiptType=="GRN") {
        BindItemWiseSummary();
    }
    else {
        BindMRSDetails();
    }
}   
function BindMRSSummary(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var ReceiptType = $("#ReceiptType").val();
    if (ReceiptType == "GRN") {

        var fromDate = $("#txt_Fromdate").val();
        var toDate = $("#txt_Todate").val();
    }
    else {
        var fromDate = $("#txtFromdate").val();
        var toDate = $("#txtTodate").val();
    }
    var suppId = $("#ddlSuppliersList").val();
    var itemId = currentrow.find("#hdnItemId").text();
    var itemName = currentrow.find("#hdnItemName").text();
    var uomName = currentrow.find("#hdnUomName").text();
    var itemQty = currentrow.find("#hdnItemQty").text();
    var MultiselectStatusHdn = $("#MultiselectStatusHdn").val();
    try {
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons3")[0].id = "dttbl3";
        $("#datatable-buttons4").attr("id", "dttbl4");
    }
    catch {}
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISGoodReceiptDetail/GetGRNMisSummary",
        data: {
            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId, MultiselectStatusHdn: MultiselectStatusHdn
        },
        success: function (data) {
            debugger;
            $("#GoodsReceiptSummary").html(data);
            //txtItemName,txtUomId,txtShippedQty
            $('#itemName').val(itemName);
            $('#uomName').val(uomName);
            $('#itemQty').val(itemQty);
            try {
                $("#dttbl1")[0].id = "datatable-buttons1";
                $("#dttbl3")[0].id = "datatable-buttons3";
                $("#dttbl4")[0].id = "datatable-buttons4";
            }
            catch {}
        }
    })
}
function ShowAsChange() {
    const ShowAs = $("#ShowAs").val();
    if (ShowAs == "S") {
        BindItemWiseSummary();
    }
    else {
        BindMRSDetails();
    }
}
function OnChangeReceiptType_List() {
    debugger;
    var ReceiptType = $("#ReceiptType").val();
    if (ReceiptType == "GRN") {
        $("#div_FromDate").removeClass("col-md-2 col-sm-12");
        $("#div_ItemName").removeClass("col-md-3 col-sm-12");
        $("#div_FromDate").addClass("col-md-2 col-sm-12 pl-0");
        $("#div_ItemName").addClass("col-md-3 col-sm-12 pl-0");
        $("#div_SupplierName").css("display", "");
        $("#DIV_Status").css("display", "");
        $("#GoodsReceipt_Div").css("display", "");
       
        $("#ExternalRecipt_Div").css("display", "none");
        $("#DIV_EntityType").css("display", "none");
        $("#DIV_EntityName").css("display", "none");
        $("#div_grndate").css("display", "");
        $("#div_externeldate").css("display", "none");
    }
    else {
        $("#div_FromDate").removeClass("col-md-2 col-sm-12 pl-0");
        $("#div_FromDate").addClass("col-md-2 col-sm-12");
        $("#div_ItemName").removeClass("col-md-3 col-sm-12 pl-0");
        $("#div_ItemName").addClass("col-md-3 col-sm-12");
        $("#div_SupplierName").css("display", "none");
        $("#DIV_Status").css("display", "none");
        $("#GoodsReceipt_Div").css("display", "none");
        $("#ExternalRecipt_Div").css("display", "");
        $("#DIV_EntityType").css("display", "");
        $("#DIV_EntityName").css("display", "");
        $("#div_grndate").css("display", "none");
        $("#div_externeldate").css("display", "");
    }
    BindMrsMisDetails();
}
function OnChangeEntityType_List() {
    debugger;
    var EntityType = $("#ddl_EntityType").val();
    $("#ddl_EntityName").val(0).trigger('change');
    BindSR_SuppCustList_List(EntityType);
}
//function BindSR_SuppCustList_List(EntityType) {
//    debugger;

//    $("#ddl_EntityName").select2({
//        ajax: {
//            url: $("#hfsuppcustlist2").val(),
//            data: function (params) {
//                var queryParameters = {
//                    EntityName: params.term, // search term like "a" then "an"
//                    entity_type: EntityType,
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            //type: 'POST',
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    Error_Page();
//                    return false;
//                }
//                params.page = params.page || 1;
//                return {
//                    //results:data.results,
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });
//}
function BindSR_SuppCustList_List(EntityType) {
    debugger;
    $.ajax({
        url: $("#hfsuppcustlist2").val(),
        type: "GET",
        data: { entity_type: EntityType },
        success: function (data) {
            debugger
            $("#ddl_EntityName").empty();
            $.each(data, function (i, val) {
                $("#ddl_EntityName").append(
                    $('<option></option>').val(val.ID).text(val.Name)
                );
            });
            Cmn_initializeMultiselect(['#ddl_EntityName']);
            $('#ddl_EntityName').multiselect('rebuild');
        }
    });
}
function ItemStockBatchWise(el, evt) {
    debugger;
    var QtyDigit = $("#QtyDigit").text()
    var currentrow = $(evt.target).closest('tr');
    var recept_no = currentrow.find("#recept_no").text();
    var recept_dt = currentrow.find("#hdn_ddlrecpt_dt").text();
    var Item_id = currentrow.find("#ItemName_id").val();
    var Item_Name = currentrow.find("#ItmNameSpan").text();
    var Uom_name = currentrow.find("#Uom_name").text();
    var recQty = currentrow.find("#recQty").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISGoodReceiptDetail/GetBatchDeatilMIS",
        data: {
            recept_no: recept_no, recept_dt: recept_dt, Item_id: Item_id
        },
        success: function (data) {
            debugger;
            $("#MISExternalReceipt_BatchNumber").html(data);
            $("#BatchItemName").val(Item_Name)
            $("#batchUOM").val(Uom_name)           
            $("#BatchReceivedQuantity").val(recQty)
        }



    })
}

function ItemStockSerialWise(el, evt) {
    debugger;
    var QtyDigit = $("#QtyDigit").text()
    var currentrow = $(evt.target).closest('tr');
    var recept_no = currentrow.find("#recept_no").text();
    var recept_dt = currentrow.find("#hdn_ddlrecpt_dt").text();
    var Item_id = currentrow.find("#ItemName_id").val();
    var Item_Name = currentrow.find("#ItmNameSpan").text();
    var Uom_name = currentrow.find("#Uom_name").text();
    var recQty = currentrow.find("#recQty").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISGoodReceiptDetail/GetSerialDetailData",
        data: {
            recept_no: recept_no, recept_dt: recept_dt, Item_id: Item_id
        },
        success: function (data) {
            debugger;
            $("#MISExternalReceipt_SerialDeatil").html(data);
            $("#SerialItemName").val(Item_Name)
            $("#serialUOM").val(Uom_name)
            $("#SerialReceivedQuantity").val(recQty)
           
        }



    })
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#ItemName_id").val();
    ItemInfoBtnClick(ItmCode);
}