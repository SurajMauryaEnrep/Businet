
$(document).ready(function () {
    debugger;
    $("#hdnsavebtn").val("Again");
    //$("#ddlCustomerName").select2();
    var exportcheck = $("#rbExport").is(':checked');
    //if (exportcheck == true) {
        checkprameter();
    //}

    $('#packItemDetailsttbl').on('click', '.deleteIcon', function () {
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

        var Packno = $("#PackingNumber").val();
        if (Packno == null || Packno == "") {
            if ($('#PDItmDetailsTbl tr').length <= 1) {
                debugger;
                $("#ddlCustomerName").prop("disabled", false);
            }
        }
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#hdItemId").val();
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        OnDeleteItemDeleteReferenceTable(ItemCode)
        $("#PackSrlzsnTbody tr #ItemId:contains(" + ItemCode + ")").closest("tr").remove();
        CalCulatePackSerializationTotal();
        DeleteItemBatchSerialOrderQtyDetails(ItemCode);
        updateItemSerialNumber();
        SumGross_Net_CBM();
        DisableAllPackingCheck();
        ResetPackingItemDdl();
    });
    $("#hdCustomer_type").val('D');
    BindDnCustomerList();
    BindPackagingItemList();
    BindSONumberList();
    GetViewDetails();
    sessionStorage.removeItem("ItemQtyDocNoWiseDetails");
    //DisableAllPackingCheck();
    if ($("#PDItmDetailsTbl tbody tr").length == 0) {
        $("#AllPackingCheck").attr("disabled", true);
        $("#AllDelete").attr("disabled", true);
        $("#AllDelete").css("filter", "grayscale(100%)");
    }
    ResetPackingItemDdl();
    $('#PackSrlzsnTbody').on('click', '.deleteIcon', function () {
        debugger;
        let SrCode = $(this).closest('tr').find("#SrNo").text();
        let ItemId = $(this).closest('tr').find("#ItemId").text();
        var Hdn_PackSrNo = $("#Hdn_PackSrNo").val();
        $(this).closest('tr').remove();
        if (SrCode == Hdn_PackSrNo) {
            ResetPackDetailFields();
        }
        PackingNoSetOnItemDetail(ItemId);
        ResetSerialNumber();
        CalCulateTotalPackingDetail();
        CalCulateTotalPackingDetailsForAllItems();
        let SrNo = $(this).closest('tr').find("#SrNo").text();
        if ($("#PackSrlzsnTable tbody tr").length > 0) {
          
            $("#AutoSerialization").attr("disabled", false);
            if (SrNo == Hdn_PackSrNo) {
                $("#Icon_AddNewPackDetails").html(`<i class="fa fa-plus" onclick="OnClickAddPackDetail();" title="${$("#span_AddNew").text()}"></i>`);
                $("#Pack_ItemName").attr("disabled", false);
            }
            else {
                $("#Pack_ItemName").attr("disabled", false);
                $("#Pack_SerialFrom, #Pack_SerialTo, #Pack_QtyPack, #Pack_NetWeight, #Pack_GrossWeight, #grossweightperpiece, #netweightperpiece, #Qty_PerInner").val("");
            }
        }
        else {
            $("#AutoSerialization").attr("disabled", true);
            if (SrNo == Hdn_PackSrNo) {
                $("#Pack_ItemName").attr("disabled", false);
                $("#Icon_AddNewPackDetails").html(`<i class="fa fa-plus" onclick="OnClickAddPackDetail();" title="${$("#span_AddNew").text()}"></i>`);
            }
            else {
                $("#Pack_ItemName").attr("disabled", false);
                $("#Pack_SerialFrom, #Pack_SerialTo, #Pack_QtyPack, #Pack_NetWeight, #Pack_GrossWeight, #grossweightperpiece, #netweightperpiece, #Qty_PerInner").val("");
            }
           
        }

    });
    $("#PDItmDetailsTbl > tbody > tr").each(function () {
        var row = $(this);
        var rowNo = row.find("#hdSpanRowId").val();
        row.find("#wh_id" + rowNo).select2();
    });
    var Itemorientation = $("#hdn_Itemorientation").val();

    if (Itemorientation && Itemorientation !== "") {
        // User already has a stored orientation, act accordingly
        if (Itemorientation === "IW") {
            $("#ddlItemWise").prop("checked", true);
            BindPackingListItemDropdown(1);
            //BindPackingListItemDropdown(ID); // assuming ID is defined
        } else {
            $("#ddlOrderWise").prop("checked", true);
            // Possibly call something else here if needed
        }
    } else {
        // No stored value yet, detect from checked radio button
        if ($("#ddlItemWise").is(":checked")) {
            Itemorientation = "IW";
            BindPackingListItemDropdown(1);
            $("#hdn_Itemorientation").val(Itemorientation);
           // BindPackingListItemDropdown(ID);
        } else {
            Itemorientation = "OW";
            $("#hdn_Itemorientation").val(Itemorientation);
        }
    }

   
    HideAndShowItemorientationWise();
    //document.getElementById("vmcust_id").innerHTML = "";
    //$("#ddlCustomerName").css("border-color", "#ced4da");
    //$("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "#ced4da");

    //document.getElementById("vmso_no").innerHTML = null;
    //$("#ddlOrderNumber").css("border-color", "#ced4da");
    //$("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
    
    CancelledRemarks("#CancelFlag", "Disabled");

    var itemId = $("#StockMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#PDItmDetailsTbl > tbody > tr').each(function () {

            var cellText = $(this).find('#hdItemId').val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#StockMessage").val("");
    }
});
function onchangeCancelledRemarks() {
    debugger;
    //var remrks = $("#Cancelledremarks").attr("title");
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}
function FilterPackingLotBatch(e) {
  
    var searchValue = e.currentTarget.value.trim().toUpperCase();

    $("#BatchWiseItemStockTbl > tbody > tr").each(function () {
        var row = $(this);

        // Get values from input fields (Lot and BatchNumber)
        var lotVal = row.find('input[name="Lot"]').val()?.trim().toUpperCase() || "";
        var batchVal = row.find('input[name="BatchNumber"]').val()?.trim().toUpperCase() || "";
        var ExpiryDt = row.find('input[name="ExpiryDate"]').val()?.trim().toUpperCase() || "";

        // Check if searchValue is contained in Lot or BatchNumber
        if (searchValue === "" || lotVal.includes(searchValue) || batchVal.includes(searchValue) || ExpiryDt.includes(searchValue)) {
            row.show(); // Show matching or all if empty
        } else {
            row.hide(); // Hide non-matching
        }
    });
}


function DisableAllPackingCheck() {
    if ($("#PDItmDetailsTbl tbody tr").length == 0) {
        $("#AllPackingCheck").attr("disabled", true);
        $("#AllDelete").attr("disabled", true);
        $("#AllDelete").css("filter", "grayscale(100%)");
    }
    else {
        $("#AllPackingCheck").attr("disabled", false);
        $("#AllDelete").attr("disabled", false);
        $("#AllDelete").css("filter", "");
    }
}
var DocumentMenuId = $("#DocumentMenuId").val();
//if (DocumentMenuId == "105103145115") {
//    var QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
//    var ValDecDigit = $("#ExpImpValDigit").text();///Quantity
//    var WeightDecDigit = $("#WeightDigit").text();///Quantity
//}
//else {
var QtyDecDigit = DocumentMenuId == "105103145115" ? $("#ExpImpQtyDigit").text() : $("#QtyDigit").text();///Quantity
var ValDecDigit = DocumentMenuId == "105103145115" ? $("#ExpImpValDigit").text() : $("#ValDigit").text();///Value
var WeightDecDigit = $("#WeightDigit").text();///Weight
//}

function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    var ItemOriantation = $("#hdn_Itemorientation").val();
    $("#PDItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;

        if (ItemOriantation != "IW") {
            //var ID = currentRow.find("#hdSpanRowId").val();
            var ID = currentRow.find("#SpanRowId").text();
            var Newwhid = "wh_id" + SerialNo;
            var whid = "#wh_id" + ID;
            currentRow.find(whid).attr("id", Newwhid);
           // currentRow.find("#hdSpanRowId").val(SerialNo);
        }
        else {
            //var ID = currentRow.find("#hdSpanRowId").val();
            var ID = currentRow.find("#SpanRowId").text();
            var Newwhid = "wh_id" + SerialNo;
            var whid = "#wh_id" + ID;
            currentRow.find(whid).attr("id", Newwhid);
        }
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function DeleteItemBatchSerialOrderQtyDetails(Itemid) {
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#PD_BatchItemId").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    $("#SaveItemSerialTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#PD_SerialItemId").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#PD_OrderQtyItemID").val();
        if (rowitem == Itemid) {
            var Docno = row.find("#PD_OrderQtyDocNo").val();

            $("#ddlOrderNumber option[value='" + Docno + "']").removeClass("select2-hidden-accessible");

            debugger;
            $(this).remove();
        }
    });
}
function OnPackTypeChaneg() {
    debugger;
    var domesticcheck = ("#rbDomestic").is(':checked');
    var exportcheck = ("#rbExport").is(':checked');
    if (domesticcheck == true) {
        $("#hdCustomer_type").val('D');
    }
    if (exportcheck == true) {
        $("#hdCustomer_type").val('E');
    }
}
function BindDnCustomerList() {
    var Pack_type;
    if ($("#rbDomestic").prop('checked')) {
        Pack_type = "D";
    }
    if ($("#rbExport").prop('checked')) {
        Pack_type = "E";
    }
    var DocumentMenuId = $("#DocumentMenuId").val();
    debugger;
    $("#ddlCustomerName").select2({
        ajax: {
            url: $("#hdCustomerList").val(),
            data: function (params) {
                var queryParameters = {
                    filterCustomerName: params.term,
                    pack_type: Pack_type// search term like "a" then "an"
                    , DocumentMenuId: DocumentMenuId
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
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
function OnChangeCustomer() {
    debugger;
    var customerName = $('#ddlCustomerName option:selected').val();
    var Itemorientation = $("#hdn_Itemorientation").val();
    if (customerName != "0") {
        var CustName = $("#ddlCustomerName option:selected").text();
        $("#hdcust_Name").val(CustName)
        document.getElementById("vmcust_id").innerHTML = "";
        $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "#ced4da");
        //$(".select2-container--default .select2-selection--single").css("border-color", "#ced4da");
        if (Itemorientation === "IW") {
            $("#Div_ItemPlusBtn").css("display", "block");
        }
        else {
            $("#Div_ItemPlusBtn").css("display", "none");
        }
        $(".ddlItemOrientation").attr('disabled', true);
    }
    else {
        document.getElementById("vmcust_id").innerHTML = $("#valueReq").text();
        //$(".select2-container--default .select2-selection--single").css("border-color", "red");
        $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "red");
        $("#Div_ItemPlusBtn").css("display", "none");
        $(".ddlItemOrientation").attr('disabled', false);
    }
    debugger;
    let DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103130") {

    }
    BindSONumberList();
}
function BindSONumberList() {
    debugger;
    var CustID = $('#ddlCustomerName').val();
    var Curr_Id = $('#ddlCurrencies').val();
    if (CustID != null && CustID != "") {
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/DomesticPacking/GetPackingListSONoLists",
                data: { Cust_id: CustID, Curr_Id: Curr_Id },
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
                            $('#ddlOrderNumber').append(`<optgroup class='def-cursor' id="SOTextddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}' data-uom='${$("#span_CustomerReferenceNumber").text()}'></optgroup>`);
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#SOTextddl').append(`<option data-date="${arr.Table[i].so_dt}" value="${arr.Table[i].app_so_no}" data-uom="${arr.Table[i].ref_doc_no}">${arr.Table[i].app_so_no}</option>`);
                            }
                            var firstEmptySelect = true;
                            $('#ddlOrderNumber').select2({
                                templateResult: function (data) {
                                    var PDRows = "";//$("#SaveItemOrderQtyDetails >tbody > tr td #PD_OrderQtyDocNo[value='" + data.id + "']").val();
                                    if (data.id != PDRows) {
                                        var DocDate = $(data.element).data('date');
                                        var doc = $(data.element).data('uom');
                                        var classAttr = $(data.element).attr('class');
                                        var hasClass = typeof classAttr != 'undefined';
                                        classAttr = hasClass ? ' ' + classAttr : '';
                                        var $result = $(
                                            '<div class="row OrderNumber">' +
                                            '<div class="col-md-3 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                                            '<div class="col-md-2 col-xs-12' + classAttr + '">' + DocDate + '</div>' +
                                            '<div class="col-md-7 col-xs-12' + classAttr + '">' + doc + '</div>' +
                                            '</div>'
                                        );
                                        return $result;
                                    }

                                    firstEmptySelect = false;
                                }
                            });

                            document.getElementById("vmso_no").innerHTML = null;
                            $("#ddlOrderNumber").css("border-color", "#ced4da");
                            $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
                            $("#txtso_dt").val(null);
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
        $("#txtso_dt").val(FDate);
    }

    var DocumentNumber = $('#ddlOrderNumber').val();
    if (DocumentNumber != "---Select---") {
        document.getElementById("vmso_no").innerHTML = null;
        $("#ddlOrderNumber").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
    }
    //else {
    //    document.getElementById("vmso_no").innerHTML = $("#valueReq").text();
    //    $("#ddlOrderNumber").css("border-color", "red");
    //    $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "red");
    //    $("#txtso_dt").val(null);
    //}
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
function CheckFormValidation() {
    debugger;
    try {
        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }
        var rowcount = $('#PDItmDetailsTbl tr').length;
        var ValidationFlag = true;
        var CustomerName = $('#ddlCustomerName').val();

        $("#hdcust_Id").val(CustomerName);
        if (CustomerName == "" || CustomerName == "0") {
            document.getElementById("vmcust_id").innerHTML = $("#valueReq").text();
            $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "red");
            ValidationFlag = false;
        }

        if ($("#CancelFlag").is(":checked")) {
            if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
                $('#SpanCancelledRemarks').text($("#valueReq").text());
                $("#SpanCancelledRemarks").css("display", "block");
                $("#Cancelledremarks").css("border-color", "Red");
                return false;
            }
            else {
                $('#SpanCancelledRemarks').text("");
                $("#SpanCancelledRemarks").css("display", "none");
                $("#Cancelledremarks").css("border-color", "#ced4da");
            }

        }
        //let CurrId = $("#ddlCurrencies").val();
        //if (CurrId != "0") {
        //    document.getElementById("vmddlCurrencies").innerHTML = "";
        //    $("#ddlCurrencies").css("border-color", "#ced4da");
        //}
        //else {
        //    document.getElementById("vmddlCurrencies").innerHTML = $("#valueReq").text();
        //    $("#ddlCurrencies").css("border-color", "red");
        //    ValidationFlag = false;
        //}
        if (ValidationFlag == true) {
            if (rowcount > 1) {
                var flagPackedQty = CheckPackItemValidations_PackedQty();
                if (flagPackedQty == false) {
                    return false;
                }
                //var flagNoOfPacked = CheckPackItemValidations_NoOfPacked();
                //if (flagNoOfPacked == false) {
                //    return false;
                //}
                //var flagWeight = CheckPackItemValidations_Weight();//commented by shubham maurya on 05-03-2025 for ticketId:1427
                //if (flagWeight == false) {
                //    return false;
                //}
                var warehouse = CheckPackItemValidations_warehouse();
                if (warehouse == false) {
                    return false;
                }
                var Batchflag = CheckItemBatch_Validation();
                if (Batchflag == false) {
                    return false;
                }
                //var SerialFlag = CheckItemSerial_Validation();
                //if (SerialFlag == false) {
                //    return false;
                //}
                var BatchIssueQtyAndResIssueQty = CheckBatchIssueQtyAndResIssueQty();
                if (BatchIssueQtyAndResIssueQty == false) {
                    return false;
                }
                /*check serialization is commented by Suraj on 26-02-2024 when Saving Document*/
                var HdnPackSerialization = $("#HdnPackSerialization").val();
                if (HdnPackSerialization == "Y") {
                    var PL_PackSrlznDtlCheckValid = fn_PackSrlznDtlCheckValidSave();
                    if (PL_PackSrlznDtlCheckValid == false) {
                        return false;
                    }
                }

                //var flagPackSubItm = CheckValidations_forSubItems();
                //if (flagPackSubItm == false) {
                //    return false;
                //}
                if (SaveAndExitUnReserveSubItemDetailsForAllItems() == false) {
                    FlagMsg = "Y";
                    return false;
                }
                if (flagPackedQty == true && /*flagNoOfPacked == true &&*/ /*flagWeight == true && */warehouse == true
                    && Batchflag == true /*&& SerialFlag == true*/ && BatchIssueQtyAndResIssueQty == true) {

                    var PackNONo = $("hdpack_no").val();
                    $("#DeliveryNoteNumber").val(PackNONo);

                    var pack_dt = $("hdpack_dt").val();
                    $("#txtpack_dt").val(pack_dt);

                    var PLItemDetailList = new Array();

                    $("#PDItmDetailsTbl TBODY TR").each(function () {
                        var row = $(this);
                        var ItemList = {};
                        debugger;
                        var id = row.find('#SpanRowId').text();
                        ItemList.ItemName = row.find("#ItemName").val();
                      
                        ItemList.UOMId = row.find('#hdUOMId').val();
                        ItemList.UOMName = row.find('#UOM').val();
                        ItemList.Avlstock  = row.find('#AvailableStock').val();
                        ItemList.sub_item = row.find('#sub_item').val();
                        ItemList.i_batch = row.find('#hdi_batch').val();
                        ItemList.i_serial = row.find('#hdi_serial').val();

                       
                        ItemList.ItemId = row.find("#hdItemId").val();

                        var totalOrderQty = 0;
                        var totalPendingQty = 0;
                        var totalOrderFocQty = 0;
                        var totalPendingFocQty = 0;

                        $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
                            var row = $(this);

                            var ItemID = row.find("#PD_OrderQtyItemID").val();
                            var OrderQty = parseFloat(row.find("#PD_OrderQty").val()) || 0;
                            var PendingQty = parseFloat(row.find("#PD_OrderQtyPendingQty").val()) || 0;
                            var OrderFocQty = parseFloat(row.find("#PD_OrdFocQty").val()) || 0;
                            var PendingFocQty = parseFloat(row.find("#PD_OrdFocPendingQty").val()) || 0;
                            var PackedQtyRaw = CheckNullNumber(row.find("#PD_OrderQtyPackedQty").val());
                            var PackedFocQtyRaw = CheckNullNumber(row.find("#PD_OrderQtyPackedFocQty").val());
                            //var PackedQty = parseFloat(PackedQtyRaw);

                            // Conditions:
                            if (
                                ItemID === ItemList.ItemId &&                     // Match Item ID
                                (parseFloat(PackedQtyRaw) + parseFloat(PackedFocQtyRaw))>0
                            ) {
                                totalOrderQty += OrderQty;
                                totalPendingQty += PendingQty;
                                totalOrderFocQty += OrderFocQty;
                                totalPendingFocQty += PendingFocQty;
                            }
                        });

                        // Result:
                        console.log("Total Order Qty:", totalOrderQty);
                        console.log("Total Pending Qty:", totalPendingQty);

                        //ItemList.OrderQuantity = row.find('#OrderQuantity').val();
                        ItemList.OrderQuantity = (totalOrderQty + totalOrderFocQty);
                        //ItemList.BalanceQuantity = row.find("#BalanceQuantity").val();
                        ItemList.BalanceQuantity = (totalPendingQty + totalPendingFocQty);
                        ItemList.OrderFocQuantity = totalOrderFocQty;
                        ItemList.BalanceFocQuantity = totalPendingFocQty;
                        ItemList.PackedQuantity = row.find("#PackedQuantity").val();
                        ItemList.NumberOfPacks = row.find("#NumberOfPacks").val();
                        ItemList.GrossWeight = row.find('#GrossWeight').val();
                        ItemList.NetWeight = row.find('#NetWeight').val();
                        ItemList.WhID = row.find('#wh_id' + id).val();
                       
                        ItemList.PackagingItemId = row.find('#hdpackagingItemId').val();
                        ItemList.PackagingItemName = row.find('#hdpackagingItemName').val();
                        ItemList.CBM = row.find('#CBM').val();
                        ItemList.remarks = row.find('#remarks').val();
                        ItemList.PhysicalPacks = row.find('#PhysicalPacks').val();
                        ItemList.sr_no = row.find('#hdSpanRowId').val();
                        ItemList.PackSize = row.find('#PackSize').val();
                        PLItemDetailList.push(ItemList);
                        debugger;
                    });

                    var str = JSON.stringify(PLItemDetailList);
                    $('#hdPackDetailList').val(str);
                    BindItemBatch_Detail();
                    BindItemSerial_Detail();
                    debugger;
                    BindItemOrderQtyDetail()
                    PLBindOrderReservedItemBatchDetail();
                    //if (HdnPackSerialization == "Y") {
                        PL_PackSrlznDetail();
                    //}
                    ///*-----------Sub-item-------------*/
                    debugger;
                    var SubItemsListArr = Cmn_SubItemList();
                    var str2 = JSON.stringify(SubItemsListArr);
                    $('#SubItemDetailsDt').val(str2);

                    var SubItemsResListArr = PackSubItemUnReserveListArr();
                    var str3 = JSON.stringify(SubItemsResListArr);
                    $('#SubItemResDetailsDt').val(str3);

                    var SubItemsPackResListArr = PackSubItemReserveListArr();
                    var str4 = JSON.stringify(SubItemsPackResListArr);
                    $('#SubItemPackResDetailsDt').val(str4);


                    ///*-----------Sub-item end-------------*/
                    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
                    return true;
                }
                else {
                    return false;
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
    catch (ex) {
        console.log(ex.message);
        return false;
    }
    
}
function CheckPackItemValidations_PackedQty() {
    debugger;
    var ErrorFlag = "N";
    //var docid = $("#DocumentMenuId").val();
    $("#PDItmDetailsTbl >tbody >tr").each(function () {

        var currentRow = $(this);
        if (currentRow.find("#PackedQuantity").val() == "" || parseFloat(currentRow.find("#PackedQuantity").val()) == "0") {
            currentRow.find("#PackedQuantity_Error").text($("#valueReq").text());
            currentRow.find("#PackedQuantity_Error").css("display", "block");
            currentRow.find("#PackedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#PackedQuantity_Error").css("display", "none");
            currentRow.find("#PackedQuantity").css("border-color", "#ced4da");

            var packedQty = currentRow.find("#PackedQuantity").val();
            var balanceQty = currentRow.find("#BalanceQuantity").val();
            var preametercheckqty = $("#hdn_checkpreameter").val();
            var status = $("#hd_Status").val().trim();
            if (status != "A") {
                if (preametercheckqty == "Y"/* && docid == "105103145115" //Commented by Suraj on 09-09-2024*/) {
                    currentRow.find("#PackedQuantity_Error").css("display", "none");
                    currentRow.find("#PackedQuantity").css("border-color", "#ced4da");
                }
                else {
                    if (parseFloat(packedQty) > parseFloat(balanceQty)) {
                        currentRow.find("#PackedQuantity_Error").text($("#PackedQtyBalanceQty").text());
                        currentRow.find("#PackedQuantity_Error").css("display", "block");
                        currentRow.find("#PackedQuantity").css("border-color", "red");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#PackedQuantity_Error").css("display", "none");
                        currentRow.find("#PackedQuantity").css("border-color", "#ced4da");
                    }
                }

            }
        }
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckPackItemValidations_NoOfPacked() {
    debugger;
    var ErrorFlag = "N";
    var HdnPackSerialization = $("#HdnPackSerialization").val();
    $("#PDItmDetailsTbl >tbody >tr").each(function () {

        var currentRow = $(this);
        var OuterPack = currentRow.find("#NumberOfPacks").val();
        if (OuterPack == "" || parseFloat(OuterPack) == parseFloat(0)) {
            if (HdnPackSerialization == "Y") {
                currentRow.find("#PackingBoxbtn").addClass("packing");
            }
            else {
                currentRow.find("#NumberOfPacks_Error").text($("#valueReq").text());
                currentRow.find("#NumberOfPacks_Error").css("display", "block");
                currentRow.find("#NumberOfPacks").css("border-color", "red");
            }

            ErrorFlag = "Y";
        }
        else {
            if (HdnPackSerialization == "Y") {
                currentRow.find("#PackingBoxbtn").removeClass("packing");
            } else {
                currentRow.find("#NumberOfPacks_Error").css("display", "none");
                currentRow.find("#NumberOfPacks").css("border-color", "#ced4da");
            }

        }

        if (currentRow.find("#PhysicalPacks").val() == "" || parseFloat(currentRow.find("#PhysicalPacks").val()) == "0") {
            if (HdnPackSerialization == "Y") {
             
            }
            else {
                currentRow.find("#PackingBoxbtn").addClass("packing");
                currentRow.find("#PhysicalPacks_Error").text($("#valueReq").text());
                currentRow.find("#PhysicalPacks_Error").css("display", "block");
                currentRow.find("#PhysicalPacks").css("border-color", "red");
                ErrorFlag = "Y";
            }

           
        }
        else {
            if (HdnPackSerialization == "Y") {
                currentRow.find("#PackingBoxbtn").removeClass("packing");
            } else {
                currentRow.find("#PhysicalPacks_Error").css("display", "none");
                currentRow.find("#PhysicalPacks").css("border-color", "#ced4da");
            }

        }

    });
    if (ErrorFlag == "Y") {
        if (HdnPackSerialization == "Y") {
            swal("", $("#PackSerializationDetailNotFound").text(), "warning");
        }
        return false;
    }
    else {
        return true;
    }
}
function CheckPackItemValidations_Weight() {
    debugger;
    var ErrorFlag = "N";
    $("#PDItmDetailsTbl >tbody >tr").each(function () {

        var currentRow = $(this);
        if (parseFloat(currentRow.find("#NetWeight").val()) > parseFloat(currentRow.find("#GrossWeight").val())) {
            currentRow.find("#NetWeight_Error").text($("#PackingList_NetWeightExceedingGrossWeight").text());
            currentRow.find("#NetWeight_Error").css("display", "block");
            currentRow.find("#NetWeight").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#NetWeight_Error").css("display", "none");
            currentRow.find("#NetWeight").css("border-color", "#ced4da");
        }
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckPackItemValidations_warehouse() {
    debugger;
    var ErrorFlag = "N";
    $("#PDItmDetailsTbl >tbody >tr").each(function () {

        var currentRow = $(this);
        var sno = currentRow.find("#SpanRowId").text();
        if (currentRow.find("#wh_id" + sno).val() == "" || parseFloat(currentRow.find("#wh_id" + sno).val()) == "0") {
            currentRow.find("#wh_Error").text($("#valueReq").text());
            currentRow.find("#wh_Error").css("display", "block");
            currentRow.find("#wh_id" + sno).css("border-color", "red");
            currentRow.find('[aria-labelledby="select2-wh_id' + sno + '-container"]').css("border", "1px solid red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#wh_Error").css("display", "none");
            currentRow.find("#wh_id" + sno).css("border-color", "#ced4da");
            currentRow.find('[aria-labelledby="select2-wh_id' + sno + '-container"]').css("border", "1px solid #ced4da");
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
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    $("#PDItmDetailsTbl >tbody >tr").each(function () {

        var clickedrow = $(this);
        var PackedQuantity = clickedrow.find("#PackedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        //var Batchable = clickedrow.find("#hdi_batch").val();

        var TotalItemBatchQty = parseFloat("0");
        $("#SaveItemBatchTbl >tbody >tr").each(function () {

            var currentRow = $(this);
            var bchIssueQty = currentRow.find('#PD_BatchIssueQty').val();
            var bchitemid = currentRow.find('#PD_BatchItemId').val();
            var bchuomid = currentRow.find('#PD_BatchUOMId').val();
            if (ItemId == bchitemid && UOMId == bchuomid) {
                TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
            }
        });

        if (parseFloat(PackedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
            /*commented byHina on 05-02-2024 to validate by Eye color*/
            //clickedrow.find("#btnbatchdeatil").css("border", "none");
            ValidateEyeColor(clickedrow, "btnbatchdeatil", "N");
        }
        else {
            /*commented byHina on 05-02-2024 to validate by Eye color*/
            //clickedrow.find("#btnbatchdeatil").css("border", "1px solid #dc3545");
            ValidateEyeColor(clickedrow, "btnbatchdeatil", "Y");
            BatchableFlag = "Y";
            EmptyFlag = "N";
        }

        //if (Batchable == "Y") { //Commented by Suraj on 12-09-2024
        //    var TotalItemBatchQty = parseFloat("0");
        //    if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
        //        /*commented byHina on 05-02-2024 to validate by Eye color*/
        //        /*clickedrow.find("#btnbatchdeatil").css("border", "1px solid #dc3545");*/
        //        ValidateEyeColor(clickedrow, "btnbatchdeatil", "Y");
        //        BatchableFlag = "Y";
        //        EmptyFlag = "Y";
        //    }
        //    else {
        //        $("#SaveItemBatchTbl >tbody >tr").each(function () {

        //            var currentRow = $(this);
        //            var bchIssueQty = currentRow.find('#PD_BatchIssueQty').val();
        //            var bchitemid = currentRow.find('#PD_BatchItemId').val();
        //            var bchuomid = currentRow.find('#PD_BatchUOMId').val();
        //            if (ItemId == bchitemid && UOMId == bchuomid) {
        //                TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
        //            }
        //        });

        //        if (parseFloat(PackedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
        //            /*commented byHina on 05-02-2024 to validate by Eye color*/
        //            //clickedrow.find("#btnbatchdeatil").css("border", "none");
        //            ValidateEyeColor(clickedrow, "btnbatchdeatil", "N");
        //        }
        //        else {
        //            /*commented byHina on 05-02-2024 to validate by Eye color*/
        //            //clickedrow.find("#btnbatchdeatil").css("border", "1px solid #dc3545");
        //            ValidateEyeColor(clickedrow, "btnbatchdeatil", "Y");
        //            BatchableFlag = "Y";
        //            EmptyFlag = "N";
        //        }
        //    }
        //}
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
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    $("#PDItmDetailsTbl >tbody >tr").each(function () {

        var clickedrow = $(this);
        var PackedQuantity = clickedrow.find("#PackedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#btnserialdeatil").css("border-color", "red");
                ValidateEyeColor(clickedrow, "btnserialdeatil", "Y");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {

                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#PD_SerialIssueQty').val();
                    var srialitemid = currentRow.find('#PD_SerialItemId').val();
                    var srialuomid = currentRow.find('#PD_SerialUOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });

                if (parseFloat(PackedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#btnserialdeatil").css("border-color", "#007bff");
                    ValidateEyeColor(clickedrow, "btnserialdeatil", "N");
                }
                else {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#btnserialdeatil").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "btnserialdeatil", "Y");
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
function CheckBatchIssueQtyAndResIssueQty() {
    debugger;
    var TotalItemBatchQty = 0;
    var TotalResIssueQty = 0;

    $("#SaveItemBatchTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var bchIssueQty = currentRow.find('#PD_BatchIssueQty').val();
        var bchResQty = currentRow.find('#PD_BatchResAvlStk').val();
        if (bchIssueQty != "" && bchIssueQty != null && bchResQty != "" && bchResQty != null) {
            if (parseFloat(bchResQty) > parseFloat(0)) {
                TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
            }
        }
    });
    $("#SaveOrderReservedItemBatchTbl TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var IssuedQuantity = row.find("#POR_IssueQty").val();
        if (IssuedQuantity != "" && IssuedQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(0)) {
                TotalResIssueQty = parseFloat(TotalResIssueQty) + parseFloat(IssuedQuantity);
            }
        }
    });
    debugger;
    if (parseFloat(TotalItemBatchQty) != parseFloat(TotalResIssueQty)) {
        swal("", $("#BatchIssueQtydoesnotMatchwithResIssueQty").text(), "warning");
        return false;
    }
    else {
        return true;
    }
}
function fn_PackSrlznDtlCheckValidSave() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var ErrorFlg = "N";
    if ($("#PackSrlzsnTable tbody tr").length > 0) {
        $("#PDItmDetailsTbl tbody tr").each(function () {
            var Crow = $(this);
            var hdItemId = Crow.find("#hdItemId").val();
            var ItemName = Crow.find("#ItemName").val();
            var PackedQuantity = Crow.find("#PackedQuantity").val();
            var TotalQty = 0;
            var serialForm = "0";
            var serialTo = "0";
            var QtyPerPack = "0";
            $("#PackSrlzsnTable tbody tr #ItemId:contains(" + hdItemId + ")").closest("tr").each(function () {
                var C1Row = $(this);
                //   TotalQty = (parseFloat(TotalQty) + parseFloat(C1Row.find("#TotalQty").text())).toFixed(QtyDecDigit);
                serialForm = C1Row.find("#SerialFrom").text();
                SerialTo = C1Row.find("#SerialTo").text();
                QtyPerPack = C1Row.find("#QtyPerPack").text();
                TotalQty = C1Row.find("#TotalQty").text();
                if (serialForm == "0" || serialForm == "" || SerialTo == "0" || SerialTo == "") {
                    ErrorFlg = "Y";
                    swal("", $("#span_InvalidSerialNumber").text(), "warning");
                    // return false;
                }
            });
        
           

            //if (parseFloat(PackedQuantity).toFixed(QtyDecDigit) != parseFloat(TotalQty).toFixed(QtyDecDigit)) {
            //    ErrorFlg = "Y";
            //    Crow.find("#PackingBoxbtn").addClass("packing");
            //    swal("", $("#PackedQuantityMismatchWithSerializationQuantity").text()+" " + ItemName, "warning");
            //    return false;
            //}
        });
    }

    if (ErrorFlg == "Y") {
        return false;
    } else {
        return true;
    }
}
function fn_PackSrlznDtlCheckValid() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var ErrorFlg = "N";
    //if ($("#PackSrlzsnTable tbody tr").length > 0) {
    $("#PDItmDetailsTbl tbody tr").each(function () {
        var Crow = $(this);
        var hdItemId = Crow.find("#hdItemId").val();
        var ItemName = Crow.find("#ItemName").val();
        var PackedQuantity = Crow.find("#PackedQuantity").val();
        var TotalQty = 0;
        $("#PackSrlzsnTable tbody tr #ItemId:contains(" + hdItemId + ")").closest("tr").each(function () {
            var C1Row = $(this);
            TotalQty = (parseFloat(TotalQty) + parseFloat(C1Row.find("#TotalQty").text())).toFixed(QtyDecDigit);
        });
        if (parseFloat(PackedQuantity).toFixed(QtyDecDigit) != parseFloat(TotalQty).toFixed(QtyDecDigit)) {
            ErrorFlg = "Y";
            Crow.find("#PackingBoxbtn").addClass("packing");
            swal("", $("#PackedQuantityMismatchWithSerializationQuantity").text()/*+" " + ItemName*/, "warning");
            //return false;
        }
    });
    //}

    if (ErrorFlg == "Y") {
        return false;
    } else {
        return true;
    }
}
function BindItemBatch_Detail() {
    debugger;
    var batchrowcount = $('#SaveItemBatchTbl tr').length;
    if (batchrowcount > 1) {
        var ItemBatchList = new Array();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            debugger;
            batchList.LotNo = row.find('#PD_BatchLotNo').val();
            batchList.ItemId = row.find('#PD_BatchItemId').val();
            batchList.UOMId = row.find('#PD_BatchUOMId').val();
            batchList.BatchNo = row.find('#PD_BatchBatchNo').val();
            batchList.BatchResStock = row.find('#PD_BatchResAvlStk').val();
            batchList.BatchAvlStock = row.find('#PD_BatchBatchAvlStk').val();
            batchList.IssueQty = row.find('#PD_BatchIssueQty').val();

            var ExDate = row.find('#PD_BatchExpiryDate').val().trim();
            var FDate = "";
            if (ExDate == "") {
                FDate = "";
            }
            else {
                var date = ExDate.split("-");
                FDate = date[2] + '-' + date[1] + '-' + date[0];
            }
            batchList.ExpiryDate = FDate;
            batchList.bt_sale = row.find('#PD_bt_sale').val();
            batchList.mfg_name = row.find('#PD_bt_mfg_name').val();
            batchList.mfg_mrp = row.find('#PD_bt_mfg_mrp').val();
            batchList.mfg_date = row.find('#PD_bt_mfg_date').val();
            ItemBatchList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwise").val(str1);
    }

}
function BindItemSerial_Detail() {
    debugger;
    var serialrowcount = $('#SaveItemSerialTbl tr').length;
    if (serialrowcount > 1) {
        var ItemSerialList = new Array();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this)
            var SerialList = {};
            debugger;
            SerialList.ItemId = row.find("#PD_SerialItemId").val();
            SerialList.UOMId = row.find("#PD_SerialUOMId").val();
            SerialList.LOTId = row.find("#PD_SerialLOTNo").val();
            SerialList.IssuedQuantity = row.find("#PD_SerialIssueQty").val();
            SerialList.SerialNO = row.find("#PD_BatchSerialNO").val().trim();
            SerialList.mfg_name = row.find("#PD_SerialMfgName").val().trim();
            SerialList.mfg_mrp = row.find("#PD_SerialMfgMrp").val().trim();
            SerialList.mfg_date = row.find("#PD_SerialMfgDate").val().trim();
            ItemSerialList.push(SerialList);

        });
        var str2 = JSON.stringify(ItemSerialList);
        $("#HDSelectedSerialwise").val(str2);

    }

}
function BindItemOrderQtyDetail() {
    debugger;
    var batchrowcount = $('#SaveItemOrderQtyDetails tr').length;
    if (batchrowcount > 1) {
        var ItemOrderQtyList = new Array();
        $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
            var row = $(this)
            var orderqtyList = {};

            orderqtyList.packedqty = CheckNullNumber(row.find('#PD_OrderQtyPackedQty').val());
            orderqtyList.packedfocqty = CheckNullNumber(row.find('#PD_OrdFocPackedQty').val());

            if ((parseFloat(orderqtyList.packedqty) + parseFloat(orderqtyList.packedfocqty)) != parseFloat(0)) {
                orderqtyList.itemid = row.find('#PD_OrderQtyItemID').val();
                orderqtyList.uomid = row.find('#PD_OrderQtyUomID').val();
                orderqtyList.docno = row.find('#PD_OrderQtyDocNo').val();
                var ExDate = row.find('#PD_OrderQtyDocDate').val();
                var FDate = "";
                if (ExDate == "") {
                    FDate = "";
                }
                else {
                    var date = ExDate.split("-");
                    FDate = date[2] + '-' + date[1] + '-' + date[0];
                }
                orderqtyList.docdate = FDate;
                orderqtyList.orderqty = CheckNullNumber(row.find('#PD_OrderQty').val());
                orderqtyList.pendingqty = CheckNullNumber(row.find('#PD_OrderQtyPendingQty').val());
                orderqtyList.orderfocqty = CheckNullNumber(row.find('#PD_OrdFocQty').val());
                orderqtyList.pendingfocqty = CheckNullNumber(row.find('#PD_OrdFocPendingQty').val());
                ItemOrderQtyList.push(orderqtyList);
            }
          

            //var Orderpackqty = row.find('#PD_OrderQtyPackedQty').val();
            //if (parseFloat(Orderpackqty) > parseFloat(0)) {
            //    ItemOrderQtyList.push(orderqtyList);
            //}
           
        });
        var str1 = JSON.stringify(ItemOrderQtyList);
        $("#hdOrderQtyDetails").val(str1);
    }

}
function AddDocumentDetails() {
    debugger;
    try {
        var docno = $('#ddlOrderNumber option:selected').text();
        var docnoValue = $('#ddlOrderNumber option:selected').val();
        let CurrId = $("#ddlCurrencies").val();
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103145115") {
            QtyDecDigit = $("#ExpImpQtyDigit").text();
            ValDecDigit = $("#ExpImpValDigit").text();
        }
        let CurrErr = "N";
        if (CurrId != "0") {
            document.getElementById("vmddlCurrencies").innerHTML = "";
            $("#ddlCurrencies").css("border-color", "#ced4da");
        }
        else {
            document.getElementById("vmddlCurrencies").innerHTML = $("#valueReq").text();
            $("#ddlCurrencies").css("border-color", "red");
            CurrErr == "Y";
        }

        if (($('#ddlOrderNumber').val() != "---Select---" && $('#ddlOrderNumber').val() != "") && CurrErr == "N") {
            var text = $('#ddlDocumentNumber').val();
            debugger;
            $("#ddlCustomerName").prop("disabled", true);
            $("#ddlCurrencies").prop("disabled", true);

            var So_date = $("#txtso_dt").val();
            $("#hdso_dt").val(So_date);
            $("#hd_so_no").val(docno);
            var PackType;
            let DocumentMenuId = $("#DocumentMenuId").val();
            if (DocumentMenuId == "105103130") {
                PackType = "DPacking";
            }
            else {
                PackType = "EPacking";
            }
            var PackingNumber = $("#PackingNumber").val();
            showLoader();
            let startTime = moment();//for time log
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/DomesticPacking/getDetailByOrderNo",
                data: { OrderNumber: docno, PackingNumber: PackingNumber, PackType: PackType, DocumentMenuId: DocumentMenuId },
                success: function (data) {
                    debugger;
                    let completeTime = moment();
                    duration = moment.duration(completeTime.diff(startTime));
                    console.log("Responce : " + duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');//for time log in responce
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            var HdnPackSerialization = $("#HdnPackSerialization").val();
                            var DisableIfSrlzn = "";
                            var Td_balanceQty = "";

                            var len = parseInt($('#PDItmDetailsTbl tbody tr').length);
                           
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var subitmDisable = "";
                                $("#Hdn_Curr_Id").val(arr.Table[i].curr_id);
                                if (arr.Table[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                if (HdnPackSerialization == "Y") {
                                    DisableIfSrlzn = "disabled";
                                    Td_balanceQty = `<div class=" col-sm-8 no-padding">
                                                    <input id="PackedQuantity" value="${parseFloat(arr.Table[i].pack_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="PackedQuantity" placeholder="0000.00" disabled>
                                                    <span id="PackedQuantity_Error" class="error-message is-visible"></span>
                                                 </div>
                                                 <div class="col-sm-4 no-padding">
                                                  <div class="col-sm-6 i_Icon">
                                                    <button type="button" class="calculator" onclick="OnClickItemOrderQtyIconBtn(event);" data-toggle="modal" data-target="#QuantityDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_QuantityDetail_Title").text()}"> </button>
                                                  </div>
                                                  <div class="col-sm-6 i_Icon">
                                                    <button type="button" id="PackingBoxbtn" class="calculator" onclick="OnClickPackingDetailsBtn(event);" data-toggle="modal" data-target="#PackingListDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/packing.png" alt="" title="${$("#span_PackSerializationDetail").text()}"> </button>
                                                  </div>
                                                    
                                                 </div>`
                                } else {
                                    Td_balanceQty = `<div class="col-sm-8 no-padding">
                                                    <input id="PackedQuantity" value="${parseFloat(arr.Table[i].pack_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="PackedQuantity" placeholder="0000.00" disabled>
                                                    <span id="PackedQuantity_Error" class="error-message is-visible"></span>
                                                  </div>
                                                  <div class=" col-sm-4 no-padding">
                                                   <div class="col-sm-6 i_Icon">
                                                    <button type="button" class="calculator" onclick="OnClickItemOrderQtyIconBtn(event);" data-toggle="modal" data-target="#QuantityDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_QuantityDetail_Title").text()}"> </button>
                                                   </div>
                                                    
                                                 </div>`
                                }
                                var FOrderQty = parseFloat(0).toFixed(QtyDecDigit);
                                var FPendingQty = parseFloat(0).toFixed(QtyDecDigit);
                                debugger;
                                var DocNo = arr.Table[i].app_so_no;
                                var DocDate = arr.Table[i].so_dt;
                                var ItemID = arr.Table[i].item_id;
                                var UOMID = arr.Table[i].uom_id;
                                var OrderQty = arr.Table[i].ord_qty_base;
                                var PendingQty = arr.Table[i].pending_qty;
                                var CheckDocNo = "";
                                $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
                                    var row = $(this);
                                    var rowitem = row.find("#PD_OrderQtyItemID").val();
                                    var Docno = row.find("#PD_OrderQtyDocNo").val();
                                    if (rowitem == ItemID && DocNo == Docno) {
                                        CheckDocNo = Docno;
                                    }
                                });
                                if (CheckDocNo == "") {
                                    $('#SaveItemOrderQtyDetails tbody').append(`<tr>
                    <td><input type="text" id="PD_OrderQtyItemID" value="${ItemID}" /></td>
                    <td><input type="text" id="PD_OrderQtyUomID" value="${UOMID}" /></td>
                    <td><input type="text" id="PD_OrderQtyDocNo" value="${DocNo}" /></td>
                    <td><input type="text" id="PD_OrderQtyDocDate" value="${DocDate}" /></td>
                    <td><input type="text" id="PD_OrderQty" value="${parseFloat(OrderQty).toFixed(QtyDecDigit)}" /></td>
                    <td><input type="text" id="PD_OrderQtyPendingQty" value="${parseFloat(PendingQty).toFixed(QtyDecDigit)}" /></td>
                    <td><input type="text" id="PD_OrderQtyPackedQty" value="${parseFloat(0).toFixed(QtyDecDigit)}" /></td>
                    <td><input type="text" id="PD_OrdFocQty" value="${parseFloat(arr.Table[i].ord_foc_qty).toFixed(QtyDecDigit)}" /></td>
                    <td><input type="text" id="PD_OrdFocPendingQty" value="${parseFloat(arr.Table[i].pending_foc_qty).toFixed(QtyDecDigit)}" /></td>
                    <td><input type="text" id="PD_OrdFocPackedQty" value="${parseFloat(0).toFixed(QtyDecDigit)}" /></td>
                </tr>`)
                                }
                                if ($('#PDItmDetailsTbl tbody tr').length > 0) {
                                    var hfitemid = "";
                                    $('#PDItmDetailsTbl tbody tr').each(function () {
                                        var CurrRow = $(this);
                                        var tblItemID = CurrRow.find("#hdItemId").val();
                                        var tblOrderQty = CurrRow.find("#OrderQuantity").val();
                                        var tblPendingrQty = CurrRow.find("#BalanceQuantity").val();

                                        if (tblItemID === ItemID) {
                                            hfitemid = tblItemID;
                                            if (DocNo != CheckDocNo) {

                                                FOrderQty = parseFloat(OrderQty) + parseFloat(arr.Table[i].ord_foc_qty) + parseFloat(tblOrderQty);
                                                FPendingQty = parseFloat(PendingQty) + parseFloat(arr.Table[i].pending_foc_qty) + parseFloat(tblPendingrQty);
                                                CurrRow.find("#OrderQuantity").val(parseFloat(FOrderQty).toFixed(QtyDecDigit));
                                                CurrRow.find("#BalanceQuantity").val(parseFloat(FPendingQty).toFixed(QtyDecDigit));
                                            }
                                        }
                                        else {
                                            FOrderQty = parseFloat(OrderQty) + parseFloat(arr.Table[i].ord_foc_qty);
                                            FPendingQty = parseFloat(PendingQty) + parseFloat(arr.Table[i].pending_foc_qty);
                                        }
                                    });

                                    if (hfitemid != ItemID) {
                                        len = len + 1;
                                        var CBM = "";
                                        if (DocumentMenuId == "105103145115") {
                                            CBM = `<td><input id="CBM" value="0.00" class="form-control date num_right" autocomplete="" type="text" name="CBM" required="required" placeholder="0000.00"  disabled="">
                                  <input type="hidden" id="hdpackagingItemId" value="" style="display: none;" />
                                  <input type="hidden" id="hdpackagingItemName" value="" style="display: none;"></td>`;
                                        }
                                        else {
                                            CBM = `<td><input id="CBM" value="0.000" class="form-control date num_right" autocomplete="" type="text" name="CBM" required="required" placeholder="0000.00"  disabled="">
                                  <input type="hidden" id="hdpackagingItemId" value="" style="display: none;" />
                                  <input type="hidden" id="hdpackagingItemName" value="" style="display: none;"></td>`;
                                        }
                                        $('#PDItmDetailsTbl tbody').append(`<tr id="${++rowIdx}">
                                   <td class="center"><input type="checkbox" class="tableflat" id="PackingCheck"></td>
                                   <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                   <td class="sr_padding"><span id="SpanRowId">${len}</span><input type="hidden" id="hdSpanRowId" value="${len}" style="display: none;" /></td>
                                   <td class="ItmNameBreak itmStick tditemfrz"><div class=" col-sm-10 no-padding">
                                   <input id="ItemName${len}"  value="${arr.Table[i].item_name}"class="form-control" autocomplete="off" type="text" name="ItemName" required="required" placeholder="${$("#ItemName").text()}"  onblur="this.placeholder='${$("#ItemName").text()}'" disabled>
                                   <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                   <input type="hidden" id="ItemName"  value="${arr.Table[i].item_name}" style="display: none;" />
                                   </div>
                                   <div class="col-sm-1 i_Icon">
                                   <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                   </div><div class="col-sm-1 i_Icon"><button type="button" id="BuyerInfoIcon" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button></div>
                                  </td>
                                  <td><input id="UOM" value="${arr.Table[i].uom_name}" class="form-control" autocomplete="off" type="text" name="UOM" required="required" placeholder="${$("#ItemUOM").text()}" disabled>
                                  <input type="hidden" id="hdUOMId" value="${arr.Table[i].base_uom_id}" style="display: none;" />
                                  </td>
<td>
                                                                                <input id="PackSize" maxlength="50" class="form-control" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" value="" onmouseover="OnMouseOver(this.value)">
                                                                            </td>
                                  <td style="display:none">
                                    <div class="col-sm-9 lpo_form no-padding">
                                    <input id="OrderQuantity" value="${parseFloat(FOrderQty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="OrderQuantity" required="required" placeholder="0000.00"  disabled>
                                    </div>
                                    <div class="col-sm-3 i_Icon no-padding" id="div_SubItemOrdQty">
                                    <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                                    <button type="button" id="SubItemOrdQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Order',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                    </td>
                                  <td style="display:none">
                                  <div class="col-sm-9 lpo_form no-padding">
                                  <input id="BalanceQuantity"  value="${parseFloat(FPendingQty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="ord_qty_spec" required="required" placeholder="0000.00"  disabled>
                                  <span id="ord_qty_specError" class="error-message is-visible"></span>
                                  </div>
                                  <div class="col-sm-3 i_Icon no-padding" id="div_SubItemBalncQty">
                                  <button type="button" id="SubItemBalncQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Pending',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                  </div>
                                  </td>
                                  <td>
                                  <div class="lpo_form">
                                        ${Td_balanceQty}
                                  </div></td>
                                  <td><div class="lpo_form">
                                  <input id="NumberOfPacks" ${DisableIfSrlzn} value="${parseFloat(arr.Table[i].numberPacks).toFixed(ValDecDigit)}" onkeypress="return OnKeyPressNumberOfPacks(this,event);" onchange="OnChangeNumberOfPacks(this,event)"  class="form-control num_right" autocomplete="off" type="text" name="NumberOfPacks"  placeholder="0000.00"  >
                                  <span id="NumberOfPacks_Error" class="error-message is-visible"></span></div></td>
                                  <td><div class="lpo_form">
                                  <input id="PhysicalPacks" ${DisableIfSrlzn} value="${parseFloat(0).toFixed(ValDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="PhysicalPacks"  placeholder="0000.00"  >
                                  <span id="PhysicalPacks_Error" class="error-message is-visible"></span></div></td>
                                  <td><div class="lpo_form">
                                  <input id="NetWeight" ${DisableIfSrlzn} value="${parseFloat(arr.Table[i].NetWeight).toFixed(WeightDecDigit)}" onchange="OnChangeNetWeight(this,event)" onkeypress="return OnKeyPressNetWeight(this,event);" class="form-control num_right" autocomplete="off" type="text" name="NetWeight" placeholder="0000.00" ><input type="hidden" id="hditemnetwgt" value="${parseFloat(arr.Table[i].itmnet_wght).toFixed(ValDecDigit)}" style="display: none;" />
                                  <span id="NetWeight_Error" class="error-message is-visible"></span></div></td>
                                  <td><input id="GrossWeight" ${DisableIfSrlzn} value="${parseFloat(arr.Table[i].GrossWeight).toFixed(WeightDecDigit)}" onchange="OnChangeGrossWeight(this,event)" onkeypress="return OnKeyPressGrossWeight(this,event);"  class="form-control num_right" autocomplete="off" type="text" name="GrossWeight"  placeholder="0000.00" ><input type="hidden" id="hditemgrosswgt" value="${parseFloat(arr.Table[i].itmgross_wght).toFixed(ValDecDigit)}" style="display: none;" /></td>
                                  <td><div class=" col-sm-11 no-padding"><div class="lpo_form">
                                  <select class="form-control" id="wh_id${len}" onchange="OnChangeWarehouse(this,event)"></select>
                                  <span id="wh_Error" class="error-message is-visible"></span>
                                  </div></div>
                                  <div class=" col-sm-1 i_Icon">
                                  <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                                  </div></td>
                                  <td>
                                    <div class="col-sm-9 lpo_form no-padding">
                                        <input id="AvailableStock" value="0.000" class="form-control date num_right" autocomplete="" type="text" name="AvailableStock" required="required" placeholder="0000.00"  disabled="">
                                     </div>
                                    <div class="col-sm-3 i_Icon" id="div_SubItemAvlStk">
                                        <button type="button" id="SubItemAvlStk" ${subitmDisable} class="calculator subItmImg" onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                  </td>
                                  <td class="center"><button type="button" id="btnbatchdeatil" onclick="ItemStockBatchWise(this,event)" class="calculator " data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#BatchSpanDetail").text()}"></i></button>
                                  <input type="hidden" id="hdi_batch" value="${arr.Table[i].i_batch}" style="display: none;">
                                  <input type="hidden" id="hdi_serial" value="${arr.Table[i].i_serial}" style="display: none;">
                                  </td>
                                  <!--<td class="center" hidden><button type="button" id="btnserialdeatil" onclick="ItemStockSerialWise(this,event)" class="calculator " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#SerialSpanDetail").text()}"></i></button>
                                  <input type="hidden" id="hdi_serial" value="${arr.Table[i].i_serial}" style="display: none;">
                                  </td>-->
                                  <td class="center"><button type="button" id="PackagingDetail" onclick="OnClickPackagingDetailBtn(this,event)" class="calculator " data-toggle="modal" data-target="#PackagingDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#Span_PackingDetail_Title").text()}"></i></button>
                                  </td>
                                  `+ CBM + `
                                  <td><textarea id="remarks" class="form-control remarksmessage" maxlength="100" name="remarks"  placeholder="${$("#span_remarks").text()}"></textarea>
                                  </td>
                                  </tr>`);
                                        var clickedrow = $("#PDItmDetailsTbl >tbody >tr #hdSpanRowId[value=" + len + "]").closest("tr");
                                        clickedrow.find("#ItemName").val(arr.Table[i].item_name);
                                    }
                                }
                                else {
                                    len = len + 1;
                                    FOrderQty = parseFloat(OrderQty) + parseFloat(arr.Table[i].ord_foc_qty);
                                    FPendingQty = parseFloat(PendingQty) + parseFloat(arr.Table[i].pending_foc_qty);
                                    var CBM = "";
                                    if (DocumentMenuId == "105103145115") {
                                        CBM = `<td>
                                  <input id="CBM" value="0.00" class="form-control date num_right" autocomplete="" type="text" name="CBM" required="required" placeholder="0000.00"  disabled="">
                                  <input type="hidden" id="hdpackagingItemId" value="" style="display: none;" />
                                  <input type="hidden" id="hdpackagingItemName" value="" style="display: none;">
                                  </td>`;
                                    }
                                    else {
                                        CBM = `<td>
                                  <input id="CBM" value="0.000" class="form-control date num_right" autocomplete="" type="text" name="CBM" required="required" placeholder="0000.00"  disabled="">
                                  <input type="hidden" id="hdpackagingItemId" value="" style="display: none;" />
                                  <input type="hidden" id="hdpackagingItemName" value="" style="display: none;">
                                  </td>`;
                                    }
                                    $('#PDItmDetailsTbl tbody').append(`<tr id="${++rowIdx}">
                                   <td class="center"><input type="checkbox" class="tableflat" id="PackingCheck"></td>
                                   <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                   <td class="sr_padding"><span id="SpanRowId">${len}</span><input type="hidden" id="hdSpanRowId" value="${len}" style="display: none;" /></td>
                                   <td class="ItmNameBreak itmStick tditemfrz"><div class=" col-sm-10 no-padding">
                                   <input id="ItemName${len}" class="form-control" autocomplete="off" type="text" name="ItemName" required="required" placeholder="${$("#ItemName").text()}"  onblur="this.placeholder='${$("#ItemName").text()}'" disabled>
                                   <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                   <input type="hidden" id="ItemName" value="" style="display: none;" />
                                   </div>
                                   <div class="col-sm-1 i_Icon">
                                   <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                   </div>
                                   <div class="col-sm-1 i_Icon"><button type="button" id="BuyerInfoIcon" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button></div>
                                  </td>
                                  <td><input id="UOM" value="${arr.Table[i].uom_name}" class="form-control" autocomplete="off" type="text" name="UOM" required="required" placeholder="${$("#ItemUOM").text()}" disabled>
                                  <input type="hidden" id="hdUOMId" value="${arr.Table[i].base_uom_id}" style="display: none;" />
                                  </td>
<td>
                                                                                <input id="PackSize" maxlength="50" class="form-control" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" value="" onmouseover="OnMouseOver(this.value)">
                                                                            </td>
                                  <td style="display:none">
                                    <div class="col-sm-9 lpo_form no-padding">
                                    <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                                    <input id="OrderQuantity" value="${parseFloat(FOrderQty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="OrderQuantity" required="required" placeholder="0000.00"  disabled>
                                    </div>
                                    <div class="col-sm-3 i_Icon no-padding" id="div_SubItemOrdQty">
                                    <button type="button" id="SubItemOrdQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Order',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                   </div>
                                  </td>
                                  <td style="display:none">
                                   <div class="col-sm-9 lpo_form no-padding">
                                  <input id="BalanceQuantity"  value="${parseFloat(FPendingQty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="ord_qty_spec" required="required" placeholder="0000.00"  disabled>
                                  <span id="ord_qty_specError" class="error-message is-visible"></span>
                                  </div>
                                  <div class="col-sm-3 i_Icon no-padding" id="div_SubItemBalncQty">
                                  <button type="button" id="SubItemBalncQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Pending',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                  </div>
                                  </td>
                                  <td>
                                   <div class="lpo_form">
                                       ${Td_balanceQty}
                                  </div></td>
                                  <td><div class="lpo_form">
                                  <input id="NumberOfPacks" ${DisableIfSrlzn} value="${parseFloat(arr.Table[i].numberPacks).toFixed(ValDecDigit)}" onkeypress="return OnKeyPressNumberOfPacks(this,event);" onchange="OnChangeNumberOfPacks(this,event)"  class="form-control num_right" autocomplete="off" type="text" name="NumberOfPacks"  placeholder="0000.00"  >
                                  <span id="NumberOfPacks_Error" class="error-message is-visible"></span></div></td>
                                  <td><div class="lpo_form">
                                  <input id="PhysicalPacks" ${DisableIfSrlzn} value="${parseFloat(0).toFixed(ValDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="PhysicalPacks"  placeholder="0000.00"  >
                                  <span id="PhysicalPacks_Error" class="error-message is-visible"></span></div></td>
                                  <td><div class="lpo_form">
                                  <input id="NetWeight" ${DisableIfSrlzn} value="${parseFloat(arr.Table[i].NetWeight).toFixed(WeightDecDigit)}" onchange="OnChangeNetWeight(this,event)" onkeypress="return OnKeyPressNetWeight(this,event);" class="form-control num_right" autocomplete="off" type="text" name="NetWeight" placeholder="0000.00" ><input type="hidden" id="hditemnetwgt" value="${parseFloat(arr.Table[i].itmnet_wght).toFixed(ValDecDigit)}" style="display: none;" />
                                  <span id="NetWeight_Error" class="error-message is-visible"></span></div></td>
                                  <td><input id="GrossWeight" ${DisableIfSrlzn} value="${parseFloat(arr.Table[i].GrossWeight).toFixed(WeightDecDigit)}" onchange="OnChangeGrossWeight(this,event)" onkeypress="return OnKeyPressGrossWeight(this,event);"  class="form-control num_right" autocomplete="off" type="text" name="GrossWeight"  placeholder="0000.00" ><input type="hidden" id="hditemgrosswgt" value="${parseFloat(arr.Table[i].itmgross_wght).toFixed(ValDecDigit)}" style="display: none;" /></td>
                                  <td><div class=" col-sm-11 no-padding"><div class="lpo_form">
                                  <select class="form-control" id="wh_id${len}" onchange="OnChangeWarehouse(this,event)">
                                 </select><span id="wh_Error" class="error-message is-visible"></span></div></div>
                                  <div class=" col-sm-1 i_Icon">
                                  <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                                  </div></td>
                                  <td>
                                    <div class="col-sm-9 lpo_form no-padding">
                                  <input id="AvailableStock" value="0.000" class="form-control date num_right" autocomplete="" type="text" name="AvailableStock" required="required" placeholder="0000.00"  disabled>
                                     </div>
                                    <div class="col-sm-3 i_Icon" id="div_SubItemAvlStk">
                                        <button type="button" id="SubItemAvlStk" ${subitmDisable} class="calculator subItmImg" onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                  </td>
                                  <td class="center">
                                  <button type="button" id="btnbatchdeatil" onclick="ItemStockBatchWise(this,event)" class="calculator" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#BatchSpanDetail").text()}"></i></button>
                                  <input type="hidden" id="hdi_batch" value="${arr.Table[i].i_batch}" style="display: none;">
                                  <input type="hidden" id="hdi_serial" value="${arr.Table[i].i_serial}" style="display: none;">
                                  </td>
                                 <!--<td class="center"> commented by Suraj on 12-09-2024
                                  <button type="button" id="btnserialdeatil" onclick="ItemStockSerialWise(this,event)" class="calculator " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#SerialSpanDetail").text()}"></i></button>
                                  <input type="hidden" id="hdi_serial" value="${arr.Table[i].i_serial}" style="display: none;">
                                  </td>-->
                                  <td class="center">
                                  <button type="button" id="PackagingDetail" onclick="OnClickPackagingDetailBtn(this,event)" class="calculator " data-toggle="modal" data-target="#PackagingDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#Span_PackingDetail_Title").text()}"></i></button>
                                  </td>
                                 `+ CBM + `
                                  <td>
                                  <textarea id="remarks" class="form-control remarksmessage" maxlength="100" name="remarks"  placeholder="${$("#span_remarks").text()}"></textarea>
                                  </td>
                                  </tr>`);

                                    var clickedrow = $("#PDItmDetailsTbl >tbody >tr #hdSpanRowId[value=" + len + "]").closest("tr");
                                    clickedrow.find("#ItemName").val(arr.Table[i].item_name);
                                    clickedrow.find("#ItemName" + len).val(arr.Table[i].item_name);
                                }
                                //$("#ItemName").val(arr.Table[i].item_name);
                                BindWarehouseList(len);
                                //HideAndShow_BS_Button();

                                $("#ddlOrderNumber option[value='" + docnoValue + "']").select2().hide();
                                $("#ddlOrderNumber").val("---Select---").trigger("change");
                            }
                            ResetPackingItemDdl();
                        }
                        DisableAllPackingCheck();
                        hideLoader();
                    }
                    completeTime = moment();
                    duration = moment.duration(completeTime.diff(startTime));
                    console.log("Done : " + duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');//for time log in responce
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //alert("some error");
                    hideLoader();
                }
            });
        } else {
            var DocumentNumber = $('#ddlOrderNumber').val();
            if (DocumentNumber != "---Select---") {
                document.getElementById("vmso_no").innerHTML = null;
                $("#ddlOrderNumber").css("border-color", "#ced4da");
                $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
            }
            else {
                document.getElementById("vmso_no").innerHTML = $("#valueReq").text();
                $("#ddlOrderNumber").css("border-color", "red");
                $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "red");
                $("#txtso_dt").val(null);
            }
        }
    }
    catch (ex) {
        console.log(ex)
        hideLoader();
    }
    
}
function OnClickAllPackingCheck() {
    debugger;
    var Allcheck = "";
    if ($("#AllPackingCheck").is(":checked")) {
        Allcheck = 'Y';
    }
    else {
        Allcheck = 'N';
    }
    if (Allcheck == 'Y') {
        $("#PDItmDetailsTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            CurrentRow.find("#PackingCheck").prop("checked", true);
            CurrentRow.find("#PackingCheck").is(":checked")
        });
    }
    else {
        $("#PDItmDetailsTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            CurrentRow.find("#PackingCheck").prop("checked", false);
            if (CurrentRow.find("#PackingCheck").is(":checked")) {
                check = 'Y';
            }
            else {
                check = 'N';
            }
            if (check == 'N') {
                CurrentRow.find("#PackingCheck").prop("checked", false);
            }
        });
    }
}
function OnClickAllPackingDelete() {
    $("#PDItmDetailsTbl tbody tr").each(function () {
        debugger;
        var CurrentRow = $(this);
        if (CurrentRow.find("#PackingCheck").is(":checked")) {
            var child = CurrentRow.nextAll();
            child.each(function () {
                var id = $(this).attr('id');
                var idx = $(this).children('.row-index').children('p');
                var dig = parseInt(id.substring(1));
                idx.html(`Row ${dig - 1}`);
                $(this).attr('id', `R${dig - 1}`);
            });
            CurrentRow.remove();

            var Packno = $("#PackingNumber").val();
            if (Packno == null || Packno == "") {
                if ($('#PDItmDetailsTbl tr').length <= 1) {
                    debugger;
                    $("#ddlCustomerName").prop("disabled", false);
                }
            }
            var ItemCode = "";
            ItemCode = CurrentRow.find("#hdItemId").val();
            Cmn_DeleteSubItemQtyDetail(ItemCode);
            OnDeleteItemDeleteReferenceTable(ItemCode)
            $("#PackSrlzsnTbody tr #ItemId:contains(" + ItemCode + ")").closest("tr").remove();
            CalCulatePackSerializationTotal();
            DeleteItemBatchSerialOrderQtyDetails(ItemCode);
            updateItemSerialNumber();
            SumGross_Net_CBM();
        }
    });
    $("#AllPackingCheck").prop("checked", false);
    DisableAllPackingCheck();
    ResetPackingItemDdl();
}
function BindWarehouseList(id) {
    //debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/DomesticPacking/GetWarehouseList1",
            data: {},
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //GRN_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        if (id == "Replicate") {
                            var PreWhVal = $("#Replicat_wh_id").val();
                            var s = '<option value="0">---Select---</option>';
                            for (var i = 0; i < arr.Table.length; i++) {
                                s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                            }
                            $("#Replicat_wh_id").html(s);
                            $("#Replicat_wh_id").val(IsNull(PreWhVal, '0'));
                            $("#Replicat_wh_id").select2();
                        }
                        else {
                            var PreWhVal = $("#wh_id" + id).val();
                            var s = '<option value="0">---Select---</option>';
                            for (var i = 0; i < arr.Table.length; i++) {
                                s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                            }
                            $("#wh_id" + id).html(s);
                            $("#wh_id" + id).val(IsNull(PreWhVal, '0'));
                            $("#wh_id" + id).select2();
                        }
                       
                    }
                }
            },
        });
}
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var ItemId = clickedrow.find("#hdItemId").val();;
        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var DocumentMenuId = $("#DocumentMenuId").val();
        $.ajax(
            {
                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId, UomId: null, DocumentMenuId: DocumentMenuId
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
//function HideAndShow_BS_Button() {
//    $("#PDItmDetailsTbl tbody tr").each(function () {
//        debugger;
//        var row = $(this);
//        var b_flag = row.find("#hdi_batch").val();
//        var s_flag = row.find("#hdi_serial").val();
//        if (b_flag === "Y") {
//            row.find("#btnbatchdeatil").prop("disabled", false);
//            row.find("#btnbatchdeatil").removeClass("subItmImg");
//        }
//        else {
//            row.find("#btnbatchdeatil").prop("disabled", true);
//            row.find("#btnbatchdeatil").addClass("subItmImg");
//        }
//        if (s_flag === "Y") {
//            row.find("#btnserialdeatil").prop("disabled", false);
//            row.find("#btnserialdeatil").removeClass("subItmImg");
//        }
//        else {
//            row.find("#btnserialdeatil").prop("disabled", true);
//            row.find("#btnserialdeatil").addClass("subItmImg");
//        }

//        //row.find("#PackagingDetail").prop("disabled", true);
//    });
//}
function OnKeyPressGrossWeight(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpQtyDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    }
   
    return true;
}
function OnKeyPressNetWeight(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpQtyDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    }
   
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#NetWeight_Error").css("display", "none");
    clickedrow.find("#NetWeight").css("border-color", "#ced4da");
    return true;
}
function OnKeyPressNumberOfPacks(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpQtyDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    }
    
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#NumberOfPacks_Error").css("display", "none");
    clickedrow.find("#NumberOfPacks").css("border-color", "#ced4da");

    return true;
}
function OnChangeNumberOfPacks(el, evt) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();
    }
    var clickedrow = $(evt.target).closest("tr");
    var NumberOfPacks = clickedrow.find("#NumberOfPacks").val();

    if ((NumberOfPacks == "") || (NumberOfPacks == null)) {
        NumberOfPacks = "0";
        clickedrow.find("#NumberOfPacks").val(NumberOfPacks);
    }
    clickedrow.find("#NumberOfPacks").val(parseFloat(NumberOfPacks).toFixed(QtyDecDigit));
    clickedrow.find("#PhysicalPacks").val(parseFloat(NumberOfPacks).toFixed(QtyDecDigit));
}
function OnChangephy_pack(el, evt) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();
    }
    var clickedrow = $(evt.target).closest("tr");
    var Physical_Packs = clickedrow.find("#PhysicalPacks").val();

    if ((Physical_Packs == "") || (Physical_Packs == null)) {
        Physical_Packs = "0";
        clickedrow.find("#PhysicalPacks").val(Physical_Packs);
    }
    clickedrow.find("#PhysicalPacks").val(parseFloat(Physical_Packs).toFixed(QtyDecDigit));
}
function OnChangePackedQty(el, evt) {
    debugger;
    BindPackagingItemList(evt);
}

function OnChangeGrossWeight(el, evt) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();
    }
    var clickedrow = $(evt.target).closest("tr");
    var GrossWeight = clickedrow.find("#GrossWeight").val();
    if (GrossWeight != "" && GrossWeight != null) {
        clickedrow.find("#GrossWeight").val(parseFloat(GrossWeight).toFixed(QtyDecDigit));
    }

    else {
        clickedrow.find("#GrossWeight").val(parseFloat(0).toFixed(QtyDecDigit));
    }
    SumGross_Net_CBM("Gross");
}
function SumGross_Net_CBM(type) {
    //var TotalValue = parseFloat(0);
    var TotalGrossVal = parseFloat(0);
    var TotalNetVal = parseFloat(0);
    var TotalCBM = parseFloat(0);
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();
    }
    $("#PDItmDetailsTbl TBODY TR").each(function () {
        var row = $(this);

        var GrossVal = row.find('#GrossWeight').val();
        if (GrossVal != "" && GrossVal != null) {
            TotalGrossVal = parseFloat(TotalGrossVal) + parseFloat(GrossVal);
        }
        var NetVal = row.find('#NetWeight').val();
        if (NetVal != "" && NetVal != null) {
            TotalNetVal = parseFloat(TotalNetVal) + parseFloat(NetVal);
        }
        var CBM = row.find('#CBM').val();
        if (CBM != "" && CBM != null) {
            TotalCBM = parseFloat(TotalCBM) + parseFloat(CBM);
        }   
    });
    $('#TotalGrossWeight').val(parseFloat(TotalGrossVal).toFixed(WeightDecDigit));
    $('#TotalNetWeight').val(parseFloat(TotalNetVal).toFixed(WeightDecDigit));
    $('#TotalCBM').val(parseFloat(TotalCBM).toFixed(QtyDecDigit));
}
function OnChangeNetWeight(el, evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var NetWeight = clickedrow.find("#NetWeight").val();
    if (NetWeight != "" && NetWeight != null) {
        clickedrow.find("#NetWeight").val(parseFloat(NetWeight).toFixed(WeightDecDigit));
    }
    else {
        clickedrow.find("#NetWeight").val(parseFloat(0).toFixed(WeightDecDigit));
    }

    SumGross_Net_CBM("Net");
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickBuyerInfoIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SpanRowId").text();
    var ItmCode = "";

    $("#ItemCode").val("");
    $("#ItemDescription").val("");
    $("#PackingDetail").val("");
    $("#detailremarks").val("");

    ItmCode = clickedrow.find("#hdItemId").val();
    var Cust_id = $('#ddlCustomerName option:selected').val();

    if (ItmCode != "" && ItmCode != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/DomesticSalesQuotation/GetItemCustomerInfo",
                    data: { ItemID: ItmCode, CustID: Cust_id },
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            //LSO_ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                $("#ItemCode").val(arr.Table[0].Item_code);
                                $("#ItemDescription").val(arr.Table[0].item_des);
                                $("#PackingDetail").val(arr.Table[0].pack_dt);
                                $("#detailremarks").val(arr.Table[0].remark);

                            }
                        }
                    },
                });
        } catch (err) {
        }
    }
}
function OnChangeWarehouse(el, evt) {
    debugger;
    var Itemorientation = "";
    if ($("#ddlItemWise").is(":checked")) {
        Itemorientation = "IW";
        $("#hdn_Itemorientation").val(Itemorientation);
    }
    else {
        Itemorientation = "OW";
        $("#hdn_Itemorientation").val(Itemorientation);
    }
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();
    }
    var clickedrow = $(evt.target).closest("tr");
    var Index = clickedrow.find("#SpanRowId").text();
    var ddlId = "#wh_id" + Index;
    var whERRID = "#wh_Error";
    if (clickedrow.find(ddlId).val() == "0") {
        debugger;
        if (Itemorientation != "IW") {

            clickedrow.find(whERRID).text($("#valueReq").text());
            clickedrow.find(whERRID).css("display", "block");
            clickedrow.find(ddlId).css("border-color", "red");
            clickedrow.find('[aria-labelledby="select2-wh_id' + Index + '-container"]').css("border", "1px solid red");
        }
       
    }
    else {
        var WHName = $("#wh_id" + Index + " option:selected").text();
        $("#hdwh_name").val(WHName)
        clickedrow.find(whERRID).css("display", "none");
        clickedrow.find(ddlId).css("border-color", "#ced4da");
        clickedrow.find('[aria-labelledby="select2-wh_id' + Index + '-container"]').css("border", "1px solid #ced4da");
    }

    var ItemId = clickedrow.find("#hdItemId").val();
    var WarehouseId = clickedrow.find(ddlId).val();

    //var CompId = $("#HdCompId").val();
    //var BranchId = $("#HdBranchId").val();
    debugger;
    if (WarehouseId != "0" && WarehouseId != null) {
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#PD_BatchItemId").val();
            if (rowitem == ItemId) {
                debugger;
                $(this).remove();
            }
        });


        $.ajax({
            type: "Post",
            url: "/Common/Common/getWarehouseWiseItemStock",
            data: {
                ItemId: ItemId,
                WarehouseId: WarehouseId,
            },
            success: function (data) {
                var avaiableStock = JSON.parse(data);
                var parseavaiableStock = parseFloat(avaiableStock).toFixed(QtyDecDigit)
                clickedrow.find("#AvailableStock").val(parseavaiableStock);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
            }
        });
    }
}
function BindPackagingItemList() {
    debugger;
    $("#PackagingItemddl optgroup option").remove();
    $("#PackagingItemddl optgroup").remove();
    BindItemList("#PackagingItemddl", "", "#PackagingItemDetailTbl", "", "", "PL");
}
function OnClickCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr('onclick', "return  CheckFormValidation();");
        $("#btn_save").attr('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr('onclick', "");
        $("#btn_save").attr('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#CancelFlag", "Enable");
}

//------------------------Order Quantity Details-----------------------//
function OnClickItemOrderQtyIconBtn(e) {
    debugger;
  
    var clickedrow = $(e.target).closest("tr");
   
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var Status = $("#hd_Status").val();
  
   
        BindOrderQtyDetails();
   
   
    var ItmName = clickedrow.find("#ItemName").val();
    var UOM = clickedrow.find("#UOM").val();
    var UOMid = clickedrow.find("#hdUOMId").val();
  
    var ItmCode = clickedrow.find("#hdItemId").val();
    var totalOrderQty = 0;
    var totalPendingQty = 0;
    $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
        var existingRow = $(this);
        var rowItemID = existingRow.find("#PD_OrderQtyItemID").val();
        if (ItmCode == rowItemID) {
            var order_Qty =  parseFloat(existingRow.find("#PD_OrderQty").val());
            var Pending_Qty =  parseFloat(existingRow.find("#PD_OrderQtyPendingQty").val());

            totalOrderQty += order_Qty;
            totalPendingQty += Pending_Qty;

        }
       
    });
    //var OrderQty = clickedrow.find("#OrderQuantity").val();
    //var PendingQty = clickedrow.find("#BalanceQuantity").val();

    var OrderQty = totalOrderQty;
    var PendingQty = totalPendingQty;
    var SubItemId = clickedrow.find("#sub_item").val();
    var hdi_serial = clickedrow.find("#hdi_serial").val();
    var SelectedItemdetail = $("#hdOrderQtyDetails").val();
    var docid = $("#DocumentMenuId").val();
    var Command = $("#Qty_pari_Command").val();
    var TransType = $("#qty_TransType").val();
    var Amend = $("#Amend").val()
    debugger;
  
    var PL_Status = $("#hd_Status").val().trim();
    if (ItmCode != "" || ItmCode != null) {
        $.ajax({
            type: "Post",
            url: "/ApplicationLayer/DomesticPacking/getItemOrderQuantityDetail",
            data: {
                ItemID: ItmCode, SubItemId: SubItemId,
                Status: PL_Status, SelectedItemdetail: SelectedItemdetail
                , TransType: TransType,
                Command: Command,
                docid: docid, Amend: Amend
                
            },
            success: function (data) {
                debugger;
                $('#ItemOrderQuantityDetail').html(data);

                $("#OrderQtyItemName").val(ItmName);
                $("#hd_OQtyItemId").val(ItmCode);
                $("#OrderQtyUOM").val(UOM);
                $("#hd_OQtyUOMId").val(UOMid);
                $("#ItemOrderQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit));
                $("#ItemPendingQuantity").val(parseFloat(PendingQty).toFixed(QtyDecDigit));
                $("#hdn_PQDi_serial").val(hdi_serial);
                
                if (hdi_serial == "Y") {
                    $("#OrderQtyInfoTbl tbody tr #Order_PackedQty").attr("onkeypress", 'return Cmn_IntValueonly(this,event)');
                }
                

                TotalPackedQtyInOrderDetail();
            },
        });
    }

    //var DocNoWiseItemQtyDetails = JSON.parse(sessionStorage.getItem("ItemQtyDocNoWiseDetails"));
    //if (DocNoWiseItemQtyDetails != null) {
    //    if (DocNoWiseItemQtyDetails.length > 0) {
    //        $("#OrderQtyInfoTbl tbody tr").remove();
    //        for (j = 0; j < DocNoWiseItemQtyDetails.length; j++) {
    //            var SDocNo = DocNoWiseItemQtyDetails[j].DocNo;
    //            var SDocDate = DocNoWiseItemQtyDetails[j].DocDate;

    //            var Finaldt = SDocDate.split('-');
    //            var Finaldate = Finaldt[2] + "-" + Finaldt[1] + "-" + Finaldt[0];
    //            var SItemID = DocNoWiseItemQtyDetails[j].ItemID;
    //            var SOrderQty = DocNoWiseItemQtyDetails[j].OrderQty;
    //            var SPendingQty = DocNoWiseItemQtyDetails[j].PendingQty;

    //            var PackedQty = parseFloat(0).toFixed(QtyDecDigit);
    //            $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
    //                var row = $(this);
    //                var rowitem = row.find("#PD_OrderQtyItemID").val();
    //                var rowdocno = row.find("#PD_OrderQtyDocNo").val();
    //                if (rowitem == ItmCode && rowdocno == SDocNo) {
    //                    PackedQty = parseFloat(row.find("#PD_OrderQtyPackedQty").val()).toFixed(QtyDecDigit);
    //                }
    //            });
    //            if (parseFloat(PackedQty) == parseFloat(0)) {
    //                PackedQty = "";
    //            }
    //            //if (ItmCode === SItemID) {
    //            //    $("#OrderQtyInfoTbl tbody").append(`<tr>
    //            //    <td><input id="OrderQtyDocNo" disabled class="form-control" type="text" value="${SDocNo}"></td>
    //            //    <td><input id="OrderQtyDocDate" disabled class="form-control" type="text" value="${Finaldate}"></td>
    //            //    <td><input id="OrderedQuantity" disabled class="form-control" type="text" value="${parseFloat(SOrderQty).toFixed(QtyDecDigit)}"></td>
    //            //    <td><input id="Order_PendingQty" disabled class="form-control" type="text" value="${parseFloat(SPendingQty).toFixed(QtyDecDigit)}"></td>
    //            //    <td><div class="lpo_form">
    //            //    <input id="Order_PackedQty" class="form-control" autocomplete="off" value="${PackedQty}" onkeypress="return OnKeyPressOrderPackedQty(this,event)" onchange="OnChangeOrderPackedQty(event);" maxlength="25" type="text" placeholder="0000.00"><span id="Order_PackedQty_Error" class="error-message is-visible"></span>
    //            //    </div></td>
    //            //    </tr>`);
    //            //}

    //            if (ItmCode == "" || ItmCode == null) {
    //                $.ajax({
    //                    type: "Post",
    //                    url: "/ApplicationLayer/DomesticPacking/getItemOrderQuantityDetail",
    //                    data: {
    //                        ItemID: ItemId,
    //                        SelectedItemdetail: SelectedItemdetail
    //                    },
    //                    success: function (data) {
    //                        debugger;
    //                        $('#ItemOrderQuantityDetail').html(data);

    //                        $("#OrderQtyItemName").val(ItmName);
    //                        $("#hd_OQtyItemId").val(ItmCode);
    //                        $("#OrderQtyUOM").val(UOM);
    //                        $("#hd_OQtyUOMId").val(UOMid);
    //                        $("#ItemOrderQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit));
    //                        $("#ItemPendingQuantity").val(parseFloat(PendingQty).toFixed(QtyDecDigit));
    //                    },
    //                });
    //            }

    //        }
    //        TotalPackedQtyInOrderDetail();
    //    }
    //    else {
    //        $("#OrderQtyInfoTbl tbody tr").remove();
    //    }
    //}
    //else {
    //    $("#OrderQtyInfoTbl tbody tr").remove();
    //}
}
function BindOrderQtyDetails() {
    debugger;

   


    var FDocNoWiseItemQtyDetails = [];

    var DocNoWiseItemQtyDetails = JSON.parse(sessionStorage.getItem("ItemQtyDocNoWiseDetails"));
    $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
        var row = $(this);
        var DocNo = row.find("#PD_OrderQtyDocNo").val();
        var DocDate = row.find("#PD_OrderQtyDocDate").val();
        var ItemID = row.find("#PD_OrderQtyItemID").val();
        var UomID = row.find("#PD_OrderQtyUomID").val();
        var OrderQty = row.find("#PD_OrderQty").val();
        var PendingQty = row.find("#PD_OrderQtyPendingQty").val();
        var PackedQty = row.find("#PD_OrderQtyPackedQty").val();
        
        var OrdFocQty = row.find("#PD_OrdFocQty").val();
        var PendFocQty = row.find("#PD_OrdFocPendingQty").val();
        var PackedFocQty = row.find("#PD_OrdFocPackedQty").val();
        if (parseFloat(PackedQty) == 0) {
            PackedQty = "";
        }
        if (parseFloat(PackedFocQty) == 0) {
            PackedFocQty = "";
        }
        if (DocNoWiseItemQtyDetails != null) {
            if (DocNoWiseItemQtyDetails.length > 0) {
                for (j = 0; j < DocNoWiseItemQtyDetails.length; j++) {
                    var SDocNo = DocNoWiseItemQtyDetails[j].DocNo;
                    var SItemID = DocNoWiseItemQtyDetails[j].ItemID;

                    if (SDocNo != DocNo && SItemID != SItemID) {
                        FDocNoWiseItemQtyDetails.push({ DocNo: DocNo, DocDate: DocDate, ItemID: ItemID, UomID: UomID, OrderQty: OrderQty, PendingQty: PendingQty, PackedQty: PackedQty, OrdFocQty: OrdFocQty, PendFocQty: PendFocQty, PackedFocQty: PackedFocQty })
                    }
                }
            }
            else {
                FDocNoWiseItemQtyDetails.push({ DocNo: DocNo, DocDate: DocDate, ItemID: ItemID, UomID: UomID, OrderQty: OrderQty, PendingQty: PendingQty, PackedQty: PackedQty, OrdFocQty: OrdFocQty, PendFocQty: PendFocQty, PackedFocQty: PackedFocQty })
            }
        }
        else {
            FDocNoWiseItemQtyDetails.push({ DocNo: DocNo, DocDate: DocDate, ItemID: ItemID, UomID: UomID, OrderQty: OrderQty, PendingQty: PendingQty, PackedQty: PackedQty, OrdFocQty: OrdFocQty, PendFocQty: PendFocQty, PackedFocQty: PackedFocQty })
        }
    });

    if (FDocNoWiseItemQtyDetails != null) {
        if (FDocNoWiseItemQtyDetails.length > 0) {
            //sessionStorage.setItem("ItemQtyDocNoWiseDetails", JSON.stringify(FDocNoWiseItemQtyDetails));
            var str1 = JSON.stringify(FDocNoWiseItemQtyDetails);
            $("#hdOrderQtyDetails").val(str1);

        }
    }
         
}
function onclickbtnOrderQtyReset() {
    debugger;
    $("#OrderQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        debugger;
        CurrentRow.find("#Order_PackedQty").val("");

    });

    $("#LblTotalOrderQty").text("");
}
function OnChangeOrderPackedQty(evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var PackedQty = clickedrow.find("#Order_PackedQty").val();

    if ((PackedQty == "") || (PackedQty == null)) {
        PackedQty = "0";
        clickedrow.find("#Order_PackedQty").val(PackedQty);
    }
    var PendingQty = clickedrow.find("#Order_PendingQty").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    let QtyDigit = QtyDecDigit;
    if ($("#hdn_PQDi_serial").val() == "Y") {
        QtyDigit = 0;
    }
    //if ($("#rbExport").is(":checked")) { //Commented by Suraj on 06-09-2024
        if ($("#hdn_checkpreameter").val() == "Y") {
            clickedrow.find("#Order_PackedQty_Error").css("display", "none");
            clickedrow.find("#Order_PackedQty").css("border-color", "#ced4da");
            var test = parseFloat(PackedQty).toFixed(QtyDigit);
            clickedrow.find("#Order_PackedQty").val(test);
        }
        else {
            if (parseFloat(PackedQty) > parseFloat(PendingQty)) {
                debugger;
                clickedrow.find("#Order_PackedQty_Error").text($("#ExceedingQty").text());
                clickedrow.find("#Order_PackedQty_Error").css("display", "block");
                clickedrow.find("#Order_PackedQty").css("border-color", "red");
            }
            else {
                clickedrow.find("#Order_PackedQty_Error").css("display", "none");
                clickedrow.find("#Order_PackedQty").css("border-color", "#ced4da");
                var test = parseFloat(PackedQty).toFixed(QtyDigit);
                clickedrow.find("#Order_PackedQty").val(test);
            }
        }
    //}
    //else {
    //    if (parseFloat(PackedQty) > parseFloat(PendingQty)) {
    //        debugger;
    //        clickedrow.find("#Order_PackedQty_Error").text($("#ExceedingQty").text());
    //        clickedrow.find("#Order_PackedQty_Error").css("display", "block");
    //        clickedrow.find("#Order_PackedQty").css("border-color", "red");
    //    }
    //    else {
    //        clickedrow.find("#Order_PackedQty_Error").css("display", "none");
    //        clickedrow.find("#Order_PackedQty").css("border-color", "#ced4da");
    //        var test = parseFloat(PackedQty).toFixed(QtyDigit);
    //        clickedrow.find("#Order_PackedQty").val(test);
    //    }
    //}


    TotalPackedQtyInOrderDetail();
    
  
}
function OnChangeOrderPackedFocQty(evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var PackedQty = clickedrow.find("#Order_PackedFocQty").val();

    if ((PackedQty == "") || (PackedQty == null)) {
        PackedQty = "0";
        clickedrow.find("#Order_PackedFocQty").val(PackedQty);
    }
    var PendingQty = clickedrow.find("#Order_PendingFocQty").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    let QtyDigit = QtyDecDigit;
    
        if ($("#hdn_checkpreameter").val() == "Y") {
            clickedrow.find("#Order_PackedFocQty_Error").css("display", "none");
            clickedrow.find("#Order_PackedFocQty").css("border-color", "#ced4da");
            var test = parseFloat(PackedQty).toFixed(QtyDigit);
            clickedrow.find("#Order_PackedFocQty").val(test);
        }
        else {
            if (parseFloat(PackedQty) > parseFloat(PendingQty)) {
                debugger;
                clickedrow.find("#Order_PackedFocQty_Error").text($("#ExceedingQty").text());
                clickedrow.find("#Order_PackedFocQty_Error").css("display", "block");
                clickedrow.find("#Order_PackedFocQty").css("border-color", "red");
            }
            else {
                clickedrow.find("#Order_PackedFocQty_Error").css("display", "none");
                clickedrow.find("#Order_PackedFocQty").css("border-color", "#ced4da");
                var test = parseFloat(PackedQty).toFixed(QtyDigit);
                clickedrow.find("#Order_PackedFocQty").val(test);
            }
        }
    TotalPackedQtyInOrderDetail();
}
function OnKeyPressOrderPackedQty(el, evt) {
    var clickedrow = $(evt.target).closest("tr");
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpQtyDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    }

   

    clickedrow.find("#Order_PackedQty_Error").css("display", "none");
    clickedrow.find("#Order_PackedQty ").css("border-color", "#ced4da");
    return true;
}
function TotalPackedQtyInOrderDetail() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    var TotalPackedQty = parseFloat(0).toFixed(QtyDecDigit);
    var TotalPackedFocQty = parseFloat(0).toFixed(QtyDecDigit);
    $("#OrderQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PackedQty = CheckNullNumber(CurrentRow.find("#Order_PackedQty").val());
        var PackedFocQty = CheckNullNumber(CurrentRow.find("#Order_PackedFocQty").val());
        if ((parseFloat(PackedQty) + parseFloat(PackedFocQty)) > 0) {
            TotalPackedQty = parseFloat(TotalPackedQty) + parseFloat(PackedQty);
            TotalPackedFocQty = parseFloat(TotalPackedFocQty) + parseFloat(PackedFocQty);
        }
    });
    $("#LblTotalOrderQty").text(parseFloat(TotalPackedQty).toFixed(QtyDecDigit));
    $("#LblTotalOrderFocQty").text(parseFloat(TotalPackedFocQty).toFixed(QtyDecDigit));
}
function onclickbtnOrderQtySaveAndExit(e) {
    debugger
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();///Quantity
    }
    if (CheckOrderPackedQtyValidation() === true) {
        TotalPackedQtyInOrderDetail();
        var clickedrow = $(e.target).closest("tr");
        var SubItemTotalPackedQty = $("#SubItemTotalQty").text()
        var sub_item = $("#Packingsub_item").val();
        var ItemPendingQty = $("#ItemPendingQuantity").val();
        var ItemPendingFocQty = $("#ItemPendingFocQuantity").val();
        var ProductID = $("#hd_OQtyItemId").val();
        var ItemTotalPackedQuantity = $("#LblTotalOrderQty").text();
        var ItemTotalPackedFocQuantity = $("#LblTotalOrderFocQty").text();
        var flagSrcMatch = "N";
        if (sub_item == "Y") {
            var flagSrcMatch = "N";
            $("#OrderQtyInfoTbl tbody tr").each(function () {
                var QtyItemRow = $(this);
                debugger;
                var TotalSubPackQty = 0;
                var TotalSubPackFocQty = 0;
                //var ItemOrder_ItemId = QtyItemRow.find("#PD_OrderQtyItemID").val();
                var ItemOrder_SONo = QtyItemRow.find("#OrderQtyDocNo").val();
                var ItemOrder_SODate = QtyItemRow.find("#OrderQtyDocDate").val().split("-").reverse().join("-");
                var ItemOrder_PackedQty = CheckNullNumber(QtyItemRow.find("#Order_PackedQty").val());
                var ItemOrder_PackedFocQty = CheckNullNumber(QtyItemRow.find("#Order_PackedFocQty").val());
                
                //len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + ProductID + "]").closest('tr').length;
                //if (len == 0) {
                //    $("#SubItemPackQty").css("border", "1px solid red");
                //    swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
                //    flagSrcMatch = "Y";
                //}
                //else {
                $("#hdn_Sub_ItemDetailTbl >tbody >tr").each(function () {
                    var SubitmRow = $(this);
                    debugger;
                    let subItemSONo = SubitmRow.find("#subItemSrcDocNo").val();
                    let subItemSODate = SubitmRow.find("#subItemSrcDocDate").val();
                    let subItemPackQty = SubitmRow.find("#subItemQty").val();
                    let subItemPackFocQty = SubitmRow.find("#subItemFocQty").val();
                    let Item_id_subitem = SubitmRow.find("#ItemId").val();

                    if (ItemOrder_SONo == subItemSONo && subItemSODate == ItemOrder_SODate && Item_id_subitem == ProductID) {
                        TotalSubPackQty = parseFloat(TotalSubPackQty) + parseFloat(CheckNullNumber(subItemPackQty));
                        TotalSubPackFocQty = parseFloat(TotalSubPackFocQty) + parseFloat(CheckNullNumber(subItemPackFocQty));
                    }
                });
                var ItemOrderPackedQty = (parseFloat(ItemOrder_PackedQty) + parseFloat(ItemOrder_PackedFocQty)).toFixed(QtyDecDigit);
                var TotalSub_PackQty = (parseFloat(TotalSubPackQty) + parseFloat(TotalSubPackFocQty)).toFixed(QtyDecDigit);
                if (TotalSub_PackQty != ItemOrderPackedQty) {
                    //clickedrow.find("#SubItemPackQty").css("border", "1px solid red");
                    QtyItemRow.find("#SubItemPackQty").css("border", "1px solid red");
                    swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
                    flagSrcMatch = "Y";
                }
                else {
                    QtyItemRow.find("#SubItemPackQty").css("border", "");
                }
            });
            debugger;
        }
        if (flagSrcMatch == "Y" || flagSrcMatch == "" || flagSrcMatch == null) {
            return false;
        }
        debugger;

        //var docid = $("#DocumentMenuId").val();

        if (((parseFloat(ItemTotalPackedQuantity) + parseFloat(ItemTotalPackedFocQuantity)) > (parseFloat(ItemPendingQty) + parseFloat(ItemPendingFocQty))) && ($("#hdn_checkpreameter").val() != "Y")) {
            swal("", $("#PackedQtycannotexceedtoPendingQty").text(), "warning");
            flagSrcMatch = "Y";
        }

        else {
            debugger;
            var SelectedItem = $("#hd_OQtyItemId").val();
            var ItemUomId = $("#hd_OQtyUOMId").val();
            $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#PD_OrderQtyItemID").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });
            $("#OrderQtyInfoTbl tbody tr").each(function () {
                var CurrentRow = $(this);
                var PackedQty = CurrentRow.find("#Order_PackedQty").val();
                var PackedFocQty = CurrentRow.find("#Order_PackedFocQty").val();
                var hd_Status = $("#hd_Status").val();

                var OQtyDocNo = CurrentRow.find("#OrderQtyDocNo").val();
                var OQtyDocDate = CurrentRow.find("#OrderQtyDocDate").val();
                var OQtyOrderQty = CurrentRow.find("#OrderedQuantity").val();
                var PendingQty = CurrentRow.find("#Order_PendingQty").val();
                var OrderFocQty = CurrentRow.find("#OrderedFocQuantity").val();
                var PendingFocQty = CurrentRow.find("#Order_PendingFocQty").val();
                if (hd_Status == "D" || hd_Status == "F" || hd_Status == "") {
                    if (PackedQty == null && PackedQty == "" ) {
                        PackedQty = "";
                    }
                    //    var OQtyDocNo = CurrentRow.find("#OrderQtyDocNo").val();
                    //    var OQtyDocDate = CurrentRow.find("#OrderQtyDocDate").val();
                    //    var OQtyOrderQty = CurrentRow.find("#OrderedQuantity").val();
                    //    var PendingQty = CurrentRow.find("#Order_PendingQty").val();
                    //var OrderFocQty = CurrentRow.find("#OrderedFocQuantity").val();
                    //var PendingFocQty = CurrentRow.find("#Order_PendingFocQty").val();
                        $('#SaveItemOrderQtyDetails tbody').append(`<tr>
                    <td><input type="text" id="PD_OrderQtyItemID" value="${SelectedItem}" /></td>
                    <td><input type="text" id="PD_OrderQtyUomID" value="${ItemUomId}" /></td>
                    <td><input type="text" id="PD_OrderQtyDocNo" value="${OQtyDocNo}" /></td>
                    <td><input type="text" id="PD_OrderQtyDocDate" value="${OQtyDocDate}" /></td>
                    <td><input type="text" id="PD_OrderQty" value="${OQtyOrderQty}" /></td>
                    <td><input type="text" id="PD_OrderQtyPendingQty" value="${PendingQty}" /></td>
                    <td><input type="text" id="PD_OrderQtyPackedQty" value="${PackedQty}" /></td>
                    <td><input type="text" id="PD_OrdFocQty" value="${OrderFocQty}" /></td>
                    <td><input type="text" id="PD_OrdFocPendingQty" value="${PendingFocQty}" /></td>
                    <td><input type="text" id="PD_OrdFocPackedQty" value="${PackedFocQty}" /></td>
                </tr>`
                        );
                   
                }
                else {
                    if (PackedQty != null && PackedQty != "") {
                        $('#SaveItemOrderQtyDetails tbody').append(`<tr>
                    <td><input type="text" id="PD_OrderQtyItemID" value="${SelectedItem}" /></td>
                    <td><input type="text" id="PD_OrderQtyUomID" value="${ItemUomId}" /></td>
                    <td><input type="text" id="PD_OrderQtyDocNo" value="${OQtyDocNo}" /></td>
                    <td><input type="text" id="PD_OrderQtyDocDate" value="${OQtyDocDate}" /></td>
                    <td><input type="text" id="PD_OrderQty" value="${OQtyOrderQty}" /></td>
                    <td><input type="text" id="PD_OrderQtyPendingQty" value="${PendingQty}" /></td>
                    <td><input type="text" id="PD_OrderQtyPackedQty" value="${PackedQty}" /></td>
                    <td><input type="text" id="PD_OrdFocQty" value="${OrderFocQty}" /></td>
                    <td><input type="text" id="PD_OrdFocPendingQty" value="${PendingFocQty}" /></td>
                    <td><input type="text" id="PD_OrdFocPackedQty" value="${PackedFocQty}" /></td>
                </tr>`
                        );
                    }
                }
             
            });
            debugger;
            $("#SubItemPackQty").css("border", "");
            $("#QuantityDetail").modal('hide');

            var HdnPackSerialization = $("#HdnPackSerialization").val();

            $("#PDItmDetailsTbl >tbody >tr").each(function () {
                debugger;
                var clickedrow = $(this);
                var ItemId = clickedrow.find("#hdItemId").val();
                var grwgt = parseFloat(CheckNullNumber(clickedrow.find("#hditemgrosswgt").val()));
                var ntwgt = parseFloat(CheckNullNumber(clickedrow.find("#hditemnetwgt").val()));
                if (ItemId == SelectedItem) {
                    var totalOrdQty = parseFloat(CheckNullNumber($("#LblTotalOrderQty").text()))
                        + parseFloat(CheckNullNumber($("#LblTotalOrderFocQty").text()));
                    clickedrow.find("#PackedQuantity").val((totalOrdQty).toFixed(QtyDecDigit));
                    if (HdnPackSerialization != "Y") {
                        clickedrow.find("#GrossWeight").val((totalOrdQty * parseFloat(grwgt)).toFixed(WeightDecDigit));
                        clickedrow.find("#NetWeight").val((totalOrdQty * parseFloat(ntwgt)).toFixed(WeightDecDigit));
                    }
                    clickedrow.find("#PackedQuantity_Error").css("display", "none");
                    clickedrow.find("#PackedQuantity").css("border-color", "#ced4da");

                    var PackedQuantity = clickedrow.find("#PackedQuantity").val()
                    $("#PackSrlzsnTable tbody tr #ItemId:contains('" + ItemId + "')").closest('tr').each(function () {                      
                       // $(this).closest('tr').remove();

                        ResetPackDetailFields();
                        PackingNoSetOnItemDetail(ItemId);
                        ResetSerialNumber();
                        CalCulateTotalPackingDetail();
                        CalCulateTotalPackingDetailsForAllItems();
                        if ($("#PackSrlzsnTable tbody tr").length > 0) {
                            $("#AutoSerialization").attr("disabled", false);                           
                        }
                        else {
                            $("#AutoSerialization").attr("disabled", true);
                            $("#Pack_SerialFrom, #Pack_SerialTo, #Pack_QtyPack, #Pack_NetWeight, #Pack_GrossWeight, #grossweightperpiece, #netweightperpiece, #Qty_PerInner,#Pack_Quantity").val("");
                        }
                    });
                }
             
                
            });

            //SumGross_Net_CBM("Gross");
            //SumGross_Net_CBM("Net");
            if (HdnPackSerialization != "Y") {
                SumGross_Net_CBM();
            }

        }
        debugger;
        //else if (parseFloat(ItemTotalPackedQuantity) > parseFloat(ItemPendingQty)) {
        //     swal("", $("#PackedQtycannotexceedtoPendingQty").text(), "warning");
        // }
        //     else {
        //     debugger;
        //     var SelectedItem = $("#hd_OQtyItemId").val();
        //     var ItemUomId = $("#hd_OQtyUOMId").val();
        //     $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
        //         var row = $(this);
        //         var rowitem = row.find("#PD_OrderQtyItemID").val();
        //         if (rowitem == SelectedItem) {
        //             debugger;
        //             $(this).remove();
        //         }
        //     });
        //     $("#OrderQtyInfoTbl tbody tr").each(function () {
        //         var CurrentRow = $(this);
        //         var PackedQty = CurrentRow.find("#Order_PackedQty").val();
        //         if (PackedQty != null && PackedQty != "") {
        //             var OQtyDocNo = CurrentRow.find("#OrderQtyDocNo").val();
        //             var OQtyDocDate = CurrentRow.find("#OrderQtyDocDate").val();
        //             var OQtyOrderQty = CurrentRow.find("#OrderedQuantity").val();
        //             var PendingQty = CurrentRow.find("#Order_PendingQty").val();
        //             $('#SaveItemOrderQtyDetails tbody').append(`<tr>
        //             <td><input type="text" id="PD_OrderQtyItemID" value="${SelectedItem}" /></td>
        //             <td><input type="text" id="PD_OrderQtyUomID" value="${ItemUomId}" /></td>
        //             <td><input type="text" id="PD_OrderQtyDocNo" value="${OQtyDocNo}" /></td>
        //             <td><input type="text" id="PD_OrderQtyDocDate" value="${OQtyDocDate}" /></td>
        //             <td><input type="text" id="PD_OrderQty" value="${OQtyOrderQty}" /></td>
        //             <td><input type="text" id="PD_OrderQtyPendingQty" value="${PendingQty}" /></td>
        //             <td><input type="text" id="PD_OrderQtyPackedQty" value="${PackedQty}" /></td>
        //         </tr>`
        //             );
        //         }
        //     });
        //     $("#QuantityDetail").modal('hide');

        //     var HdnPackSerialization = $("#HdnPackSerialization").val();

        //         $("#PDItmDetailsTbl >tbody >tr").each(function () {
        //             debugger;
        //             var clickedrow = $(this);
        //             var ItemId = clickedrow.find("#hdItemId").val();
        //             var grwgt = parseFloat(clickedrow.find("#hditemgrosswgt").val());
        //             var ntwgt = parseFloat(clickedrow.find("#hditemnetwgt").val());
        //             if (ItemId == SelectedItem) {
        //                 clickedrow.find("#PackedQuantity").val(parseFloat($("#LblTotalOrderQty").text()).toFixed(QtyDecDigit));
        //                 if (HdnPackSerialization != "Y") {
        //                     clickedrow.find("#GrossWeight").val((parseFloat($("#LblTotalOrderQty").text()) * parseFloat(grwgt)).toFixed(QtyDecDigit));
        //                     clickedrow.find("#NetWeight").val((parseFloat($("#LblTotalOrderQty").text()) * parseFloat(ntwgt)).toFixed(QtyDecDigit));
        //                 }
        //                 clickedrow.find("#PackedQuantity_Error").css("display", "none");
        //                 clickedrow.find("#PackedQuantity").css("border-color", "#ced4da");
        //             }
        //         });

        //         //SumGross_Net_CBM("Gross");
        //         //SumGross_Net_CBM("Net");
        //     if (HdnPackSerialization != "Y") {
        //         SumGross_Net_CBM();
        //     }

        // }
        ResetPackingItemDdl();
    }
    else {
        
        return;
    }
}
function CheckOrderPackedQtyValidation() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();///Quantity
    }
    var status = "N";
    var ItemWiseCountPAckQty = 0;
    var hdn_Itemorientation = $("#hdn_Itemorientation").val();
    var allowOverPacking = $("#hdn_checkpreameter").val() === "Y";
    var hasPackedQty = false; // <-- NEW
    var totalPAckQty = 0; // <-- NEW
    $("#OrderQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PackedQty = CheckNullNumber(CurrentRow.find("#Order_PackedQty").val());
        var PendingQty = CheckNullNumber(CurrentRow.find("#Order_PendingQty").val());
        var PackedFocQty = CheckNullNumber(CurrentRow.find("#Order_PackedFocQty").val());
        var PendingFocQty = CheckNullNumber(CurrentRow.find("#Order_PendingFocQty").val());

        var packed = parseFloat(PackedQty) || 0;
        var pending = parseFloat(PendingQty) || 0;
        var packedFoc = parseFloat(PackedFocQty) || 0;
        var pendingFoc = parseFloat(PendingFocQty) || 0;

       
      

        if (packed.toString() != "" && packed != null &&
            (parseFloat(packed) + parseFloat(packedFoc)).toFixed(QtyDecDigit) != parseFloat(0).toFixed(QtyDecDigit)) {
            totalPAckQty = totalPAckQty + packed + packedFoc;

            // if ($("#rbExport").is(":checked")) { //commented by Suraj on 06-09-2024 to Enable Over packing in Domestic Packing
            if ($("#hdn_checkpreameter").val() == "Y") {
                CurrentRow.find("#Order_PackedQty_Error").css("display", "none");
                CurrentRow.find("#Order_PackedQty").css("border-color", "#ced4da");
                var test = parseFloat(PackedQty).toFixed(QtyDecDigit);
                CurrentRow.find("#Order_PackedQty").val(test);
            }
            else {
                if (parseFloat(PackedQty) > parseFloat(PendingQty)) {
                    debugger;
                    CurrentRow.find("#Order_PackedQty_Error").text($("#ExceedingQty").text());
                    CurrentRow.find("#Order_PackedQty_Error").css("display", "block");
                    CurrentRow.find("#Order_PackedQty").css("border-color", "red");
                    status = "Y";
                }
                else {
                    CurrentRow.find("#Order_PackedQty_Error").css("display", "none");
                    CurrentRow.find("#Order_PackedQty").css("border-color", "#ced4da");
                    var test = parseFloat(PackedQty).toFixed(QtyDecDigit);
                    CurrentRow.find("#Order_PackedQty").val(test);
                }
                if (parseFloat(PackedFocQty) > parseFloat(PendingFocQty)) {
                    debugger;
                    CurrentRow.find("#Order_PackedFocQty_Error").text($("#ExceedingQty").text());
                    CurrentRow.find("#Order_PackedFocQty_Error").css("display", "block");
                    CurrentRow.find("#Order_PackedFocQty").css("border-color", "red");
                    status = "Y";
                }
                else {
                    CurrentRow.find("#Order_PackedFocQty_Error").css("display", "none");
                    CurrentRow.find("#Order_PackedFocQty").css("border-color", "#ced4da");
                    var test = parseFloat(PackedFocQty).toFixed(QtyDecDigit);
                    CurrentRow.find("#Order_PackedFocQty").val(test);
                }
            }


            //}
            //else {
            //    if (parseFloat(PackedQty) > parseFloat(PendingQty)) {
            //        debugger;
            //        CurrentRow.find("#Order_PackedQty_Error").text($("#ExceedingQty").text());
            //        CurrentRow.find("#Order_PackedQty_Error").css("display", "block");
            //        CurrentRow.find("#Order_PackedQty").css("border-color", "red");
            //        status = "Y";
            //    }

            //    //else if (parseFloat(PackedQty) === parseFloat(0)) {
            //    //    CurrentRow.find("#Order_PackedQty_Error").text($("#valueReq").text());
            //    //    CurrentRow.find("#Order_PackedQty_Error").css("display", "block");
            //    //    CurrentRow.find("#Order_PackedQty").css("border-color", "red");
            //    //    status = "Y";
            //    //}
            //    else {
            //        CurrentRow.find("#Order_PackedQty_Error").css("display", "none");
            //        CurrentRow.find("#Order_PackedQty").css("border-color", "#ced4da");
            //        var test = parseFloat(PackedQty).toFixed(QtyDecDigit);
            //        CurrentRow.find("#Order_PackedQty").val(test);
            //    }
            //}

        }
        //else {
        //    if (parseFloat(totalPAckQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
        //        CurrentRow.find("#Order_PackedQty_Error").text($("#valueReq").text());
        //        CurrentRow.find("#Order_PackedQty_Error").css("display", "block");
        //        CurrentRow.find("#Order_PackedQty").css("border-color", "red");
        //        status = "Y";
        //    }

        //}

    });
    if (parseFloat(totalPAckQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit))
    {
        $("#OrderQtyInfoTbl tbody tr").each(function () {
            var CurrentRow = $(this);
            CurrentRow.find("#Order_PackedQty_Error").text($("#valueReq").text());
            CurrentRow.find("#Order_PackedQty_Error").css("display", "block");
            CurrentRow.find("#Order_PackedQty").css("border-color", "red");
            status = "Y";
        })
       
    }
    if (status === "Y") {
        return false;
    }
    else {
        return true;
    }
}

//---------------------------------End--------------------------------//

//-----------------------------Batch Details---------------------------//
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();///Quantity
    }
    $("#PDItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var PackedQuantity = clickedrow.find("#PackedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Batchable = "Y";//clickedrow.find("#hdi_batch").val(); //Commented by Suraj on 13-09-2024 to run this validation for all items

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                clickedrow.find("#btnbatchdeatil").css("border", "1px solid #dc3545");
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemBatchTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchIssueQty = currentRow.find('#PD_BatchIssueQty').val();
                    var bchitemid = currentRow.find('#PD_BatchItemId').val();
                    var bchuomid = currentRow.find('#PD_BatchUOMId').val();
                    if (ItemId == bchitemid && UOMId == bchuomid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                    }
                });

                if (parseFloat(PackedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    clickedrow.find("#btnbatchdeatil").css("border", "none");
                }
                else {
                    clickedrow.find("#btnbatchdeatil").css("border", "1px solid #dc3545");
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
        swal("", $("#BatchQtydoesnotmatchwithShippedQty").text(), "warning");
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
function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        PLBindItemBatchDetail();
        var clickedrow = $(evt.target).closest("tr");
        var Index = clickedrow.find("#SpanRowId").text();
        var subitem = clickedrow.find("#sub_item").val();
        var ddlId = "#wh_id" + Index;
        var ItemId = clickedrow.find("#hdItemId").val();
        var WarehouseId = clickedrow.find(ddlId).val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var PackedQty = clickedrow.find("#PackedQuantity").val();
        var SelectedItemdetail = $("#HDSelectedBatchwise").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var docid = $("#DocumentMenuId").val();
        var TransType = $("#qty_TransType").val();/*This is use only Partial View */
        var Command = $("#Qty_pari_Command").val();/*This is use only Partial View */
        var PL_Status = $("#hd_Status").val().trim();
        var Amend = $("#Amend").val()
        if (docid == "105103145115") {
            QtyDecDigit = $("#ExpImpQtyDigit").text();
        }
        else {
            QtyDecDigit = $("#QtyDigit").text();
        }
        var documentlist = new Array();
        $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
            var row = $(this);
            var docList = {};
            var docno = row.find("#PD_OrderQtyDocNo").val();
            var docdt = row.find("#PD_OrderQtyDocDate").val();

            docList.docno = docno;
            docList.docdt = docdt;
            documentlist.push(docList);
        });
        var vdoclist = JSON.stringify(documentlist);

        if (PL_Status == "" || PL_Status == null || PL_Status == "D" || Amend == "Amend") {
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/DomesticPacking/getItemStockBatchWise",
                data: {
                    ItemId: ItemId,
                    WarehouseId: WarehouseId,
                    Status: PL_Status,
                    SelectedItemdetail: SelectedItemdetail,
                    SelectedDocdetail: vdoclist,
                    docid: docid,
                    TransType: TransType,
                    Command: Command,
                    Amend: Amend
                },
                success: function (data) {
                    debugger;
                    $('#ItemStockBatchWisePaking').html(data);
                    $("#HDBatchSubItem").val(subitem);
                    if (subitem == "N") {
                        $("#SubItemBatchPopupQty").attr("disabled", true);
                    } else {
                        $("#SubItemBatchPopupQty").attr("disabled", false);
                    }

                    $("#ItemNameBatchWise").val(ItemName);
                    $("#UOMBatchWise").val(UOMName);
                    $("#QuantityBatchWise").val(PackedQty);
                    $("#HDItemNameBatchWise").val(ItemId);
                    $("#HDWhIDBatchWise").val(WarehouseId);
                    $("#HDUOMBatchWise").val(UOMId);


                    var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#SaveItemBatchTbl TBODY TR").length > 0) {
                        $("#SaveItemBatchTbl TBODY TR").each(function () {
                            var row = $(this)
                            var BtItemId = row.find("#PD_BatchItemId").val();
                            if (BtItemId === ItemId) {
                                TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#PD_BatchIssueQty").val());
                            }
                        });
                    }

                    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                        var row = $(this)
                        var issueQty = row.find("#IssuedQuantity").val();
                        var bt_sale = row.find("#bt_sale").text();
                        if (bt_sale == "N") {
                            row.find("#IssuedQuantity").attr("disabled", true)
                        }
                        if (issueQty != null && issueQty != "") {
                            row.find("#IssuedQuantity").val(parseFloat(issueQty).toFixed(QtyDecDigit))
                        }
                    });
                    $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));

                    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                        var row = $(this)
                        var ResQty = row.find("#ResQty").val();
                        if (ResQty != null && ResQty != "") {
                            if (parseFloat(ResQty) > parseFloat(0)) {
                                row.find("#IssuedQuantity").prop('readonly', true);
                                row.find("#Icon_Order_resqty").removeAttr("style")
                            }
                            else {
                                row.find("#Icon_Order_resqty").css("display", "none")
                            }
                        }
                        else {
                            row.find("#Icon_Order_resqty").css("display", "none")
                        }
                    });
                    //Added by Suraj on 20-02-2024
                    try {
                        //For Auto fill Quantity on FIFO basis in the Batch Table.
                        //this will work only first time after save old value will come in the table
                        //Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", PackedQty, "ToatlAvlQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                        Cmn_PackingAutoFillBatchQty("BatchWiseItemStockTbl", PackedQty, "ToatlAvlQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                    } catch (err) {
                        console.log('Error : '+err.message)
                    }
                    
                },
            });
        }
        //else if (PL_Status != "D") {
        else {
            //var Type = "D";
            var PL_No = $("#PackingNumber").val();
            var PL_Date = $("#txtpack_dt").val();
            var docid = $("#DocumentMenuId").val();
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/DomesticPacking/getItemStockBatchWiseAfterInsert",
                data: {
                    PL_No: PL_No,
                    PL_Date: PL_Date,
                    Status: PL_Status,
                    ItemId: ItemId,
                    docid: docid,
                    TransType: TransType,
                    Command: Command

                },
                success: function (data) {
                    debugger;
                    $('#ItemStockBatchWisePaking').html(data);
                    if (subitem == "N") {
                        $("#SubItemBatchPopupQty").attr("disabled", true);
                    } else {
                        $("#SubItemBatchPopupQty").attr("disabled", false);
                    }
                    $("#ItemNameBatchWise").val(ItemName);
                    $("#UOMBatchWise").val(UOMName);
                    $("#QuantityBatchWise").val(PackedQty);
                    $("#HDWhIDBatchWise").val(WarehouseId);
                    $("#HDItemNameBatchWise").val(ItemId);
                    $("#HDUOMBatchWise").val(UOMId);

                    var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#SaveItemBatchTbl TBODY TR").length > 0) {
                        $("#SaveItemBatchTbl TBODY TR").each(function () {
                            var row = $(this)
                            var btItemId = row.find("#PD_BatchItemId").val();
                            if (btItemId === ItemId) {
                                TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#PD_BatchIssueQty").val());
                            }
                        });
                    }
                    $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));
                    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                        var row = $(this)
                        var ResQty = row.find("#ResQty").val();
                        if (ResQty != null && ResQty != "") {
                            if (parseFloat(ResQty) > parseFloat(0)) {
                                row.find("#IssuedQuantity").prop('readonly', true);
                                row.find("#Icon_Order_resqty").removeAttr("style")
                            }
                            else {
                                row.find("#Icon_Order_resqty").css("display", "none")
                            }
                        }
                        else {
                            row.find("#Icon_Order_resqty").css("display", "none")
                        }
                    });
                },
            });
        }
        //else {
        //    $.ajax({
        //        type: "Post",
        //        url: "/ApplicationLayer/DomesticPacking/getItemStockBatchWise",
        //        data: {
        //            ItemId: ItemId,
        //            WarehouseId: WarehouseId,
        //            Status: PL_Status,
        //            SelectedItemdetail: SelectedItemdetail,
        //            SelectedDocdetail: vdoclist
        //        },
        //        success: function (data) {
        //            debugger;
        //            $('#ItemStockBatchWisePaking').html(data);

        //            $("#ItemNameBatchWise").val(ItemName);
        //            $("#UOMBatchWise").val(UOMName);
        //            $("#QuantityBatchWise").val(PackedQty);
        //            $("#HDWhIDBatchWise").val(WarehouseId);
        //            $("#HDItemNameBatchWise").val(ItemId);
        //            $("#HDUOMBatchWise").val(UOMId);

        //            var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

        //            if ($("#SaveItemBatchTbl TBODY TR").length > 0) {
        //                $("#SaveItemBatchTbl TBODY TR").each(function () {
        //                    var row = $(this)
        //                    var BtItemId = row.find("#PD_BatchItemId").val();
        //                    if (BtItemId === ItemId) {
        //                        TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#PD_BatchIssueQty").val());
        //                    }
        //                });
        //            }

        //            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        //                var row = $(this)
        //                var issueQty = row.find("#IssuedQuantity").val();
        //                if (issueQty != null && issueQty != "") {
        //                    row.find("#IssuedQuantity").val(parseFloat(issueQty).toFixed(QtyDecDigit))
        //                }
        //            });

        //            $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));
        //            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        //                var row = $(this)
        //                var ResQty = row.find("#ResQty").val();
        //                if (ResQty != null && ResQty != "") {
        //                    if (parseFloat(ResQty) > parseFloat(0)) {
        //                        row.find("#IssuedQuantity").prop('readonly', true);
        //                        row.find("#Icon_Order_resqty").removeAttr("style")
        //                    }
        //                    else {
        //                        row.find("#Icon_Order_resqty").css("display", "none")
        //                    }
        //                }
        //                else {
        //                    row.find("#Icon_Order_resqty").css("display", "none")
        //                }
        //            });
        //        },
        //    });
        //}

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickItemBatchResetbtn() {
    debugger;
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#IssuedQuantity").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var HdItemId = $("#HDItemNameBatchWise").val();
    var sub_item = $("#HDBatchSubItem").val();
    var ItemPackedQty = $("#QuantityBatchWise").val();
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    var FlagMsg = "N";
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        //debugger;
        var row = $(this);
        //var AvailableQuantity = row.find("#AvailableQuantity").val();
        var TotalAvailableQuantity = row.find("#ToatlAvlQuantity").val();
        var IssuedQuantity = row.find("#IssuedQuantity").val();
        if (parseFloat(IssuedQuantity) > parseFloat(TotalAvailableQuantity)) {
            FlagMsg = "Y";
            row.find("#IssuedQuantity_Error").text($("#IssuedQtyisGreaterthanTotalAvlQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
            ValidateEyeColor(row, "btnbatchdeatil", "Y");
            return false;
        }
    });
    debugger;
    if (sub_item == "Y") {
        if (SaveAndExitUnReserveSubItemDetailsForAllItems(HdItemId) == false) {
            FlagMsg = "Y";
            return false;
        }
    }

    if (FlagMsg === "N") {
        if (parseFloat(ItemPackedQty) != parseFloat(ItemTotalIssuedQuantity)) {
            swal("", $("#IssuedqtyshouldbeequaltoPackedqty").text(), "warning");
        }
        else {
            debugger;
            var SelectedItem = $("#HDItemNameBatchWise").val();
            $("#SaveItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#PD_BatchItemId").val();
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
                    var ItemBatchAvlstock = row.find("#AvailableQuantity").val();
                    var ItemBatchResstock = row.find("#ResQty").val();
                    var ItemExpiryDate = row.find("#ExpiryDate").val();
                    var mfg_name = row.find("#BatchMfgName").val();
                    var mfg_mrp = row.find("#BatchMfgMrp").val();
                    var mfg_date = row.find("#BatchMfgDate").val();
                    var LotNo = row.find("#Lot").val();
                    var bt_sale = row.find("#bt_sale").text();
                    $('#SaveItemBatchTbl tbody').append(`<tr>
                    <td><input type="text" id="PD_BatchLotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="PD_BatchItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="PD_BatchUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="PD_BatchBatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="PD_BatchResAvlStk" value="${ItemBatchResstock}" /></td>
                    <td><input type="text" id="PD_BatchBatchAvlStk" value="${ItemBatchAvlstock}" /></td>
                    <td><input type="text" id="PD_BatchIssueQty" value="${ItemIssueQuantity}" /></td>
                    <td><input type="text" id="PD_BatchExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="PD_bt_sale" value="${bt_sale}" /></td>
                    <td><input type="text" id="PD_bt_mfg_name" value="${mfg_name}" /></td>
                    <td><input type="text" id="PD_bt_mfg_mrp" value="${mfg_mrp}" /></td>
                    <td><input type="text" id="PD_bt_mfg_date" value="${mfg_date}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');

            $("#PDItmDetailsTbl >tbody >tr").each(function () {
                debugger;
                var clickedrow = $(this);
                var ItemId = clickedrow.find("#hdItemId").val();
                if (ItemId == SelectedItem) {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#btnbatchdeatil").css("border", "none");
                    ValidateEyeColor(clickedrow, "btnbatchdeatil", "N");
                }
            });
        }
    }
}
function PLBindItemBatchDetail() {
    var batchrowcount = $('#SaveItemBatchTbl tr').length;
    if (batchrowcount > 1) {
        var ItemBatchList = new Array();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            debugger;
            batchList.LotNo = row.find('#PD_BatchLotNo').val();
            batchList.ItemId = row.find('#PD_BatchItemId').val();
            batchList.UOMId = row.find('#PD_BatchUOMId').val();
            batchList.BatchNo = row.find('#PD_BatchBatchNo').val();
            batchList.BatchAvlStock = row.find('#PD_BatchBatchAvlStk').val();
            batchList.IssueQty = row.find('#PD_BatchIssueQty').val();

            var ExDate = row.find('#PD_BatchExpiryDate').val().trim();
            var FDate = "";
            if (ExDate == "") {
                FDate = "";
            }
            else {
                var date = ExDate.split("-");
                FDate = date[2] + '-' + date[1] + '-' + date[0];
            }
            batchList.ExpiryDate = FDate;
            batchList.bt_sale = row.find('#PD_bt_sale').val();
            batchList.mfg_name = row.find('#PD_bt_mfg_name').val();
            batchList.mfg_mrp = row.find('#PD_bt_mfg_mrp').val();
            batchList.mfg_date = row.find('#PD_bt_mfg_date').val();
            ItemBatchList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwise").val(str1);
    }

}
function onclickbtnItemBatchDiscardAndExit() {
    $("#BatchNumber").modal('hide');
}
function OnChangeIssueQty(el, evt) {
    try {
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103145115") {
            QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
        }
        else {
            QtyDecDigit = $("#QtyDigit").text();///Quantity
        }
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
        var TotalAvailableQuantity = clickedrow.find("#ToatlAvlQuantity").val();
        if (IssuedQuantity != "" && IssuedQuantity != null && TotalAvailableQuantity != "" && TotalAvailableQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(TotalAvailableQuantity)) {
                clickedrow.find("#IssuedQuantity_Error").text($("#IssuedQtyisGreaterthanTotalAvlQty").text());
                clickedrow.find("#IssuedQuantity_Error").css("display", "block");
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
            }
            else {
                clickedrow.find("#IssuedQuantity_Error").css("display", "none");
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
                clickedrow.find("#IssuedQuantity").val(test);
            }
        }
        TotalBatchIssueQty();
    }
    catch (err) {
        console.log("Error : " + err.message);
    }
}
function QtyFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpQtyDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    }
   
    return true;
}
function TotalBatchIssueQty() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var TotalIssueQty = 0;
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var Issueqty = row.find("#IssuedQuantity").val();
        if (Issueqty != "" && Issueqty != null) {
            TotalIssueQty = TotalIssueQty + parseFloat(Issueqty);
        }
    });
    $("#BatchwiseTotalIssuedQuantity").text(parseFloat(parseFloat(TotalIssueQty)).toFixed(parseFloat(QtyDecDigit)));
}
//----------------------------------End-------------------------------//
//-----------------------------Order Details---------------------------//
function OnClickIssueIcon(evt) {
    debugger;
    PLBindOrderReservedItemBatchDetail();
    var clickedrow = $(evt.target).closest("tr");
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var lotno = clickedrow.find("#Lot").val();
    var batchno = clickedrow.find("#BatchNumber").val();
    var Avlqty = clickedrow.find("#AvailableQuantity").val();
    var totalAvlqty = clickedrow.find("#ToatlAvlQuantity").val();
    var packedQty = clickedrow.find("#IssuedQuantity").val();
    var Mfg_Name = clickedrow.find("#BatchMfgName").val();
    var Mfg_Mrp = clickedrow.find("#BatchMfgMrp").val();
    var Mfg_Date = clickedrow.find("#BatchMfgDate").val();

    var SelectedItemdetail = $("#HDSelectedOrderReservedBatchwiseDetails").val();

    var ItemName = $("#ItemNameBatchWise").val();
    var Itemid = $("#HDItemNameBatchWise").val();
    var WarehouseId = $("#HDWhIDBatchWise").val();
    var UOMName = $("#UOMBatchWise").val();
    var UOMid = $("#HDUOMBatchWise").val();
    var PL_Status = $("#hd_Status").val().trim();
    var docid = $("#DocumentMenuId").val();
    var TransType = $("#qty_TransType").val();
    var Command = $("#Qty_pari_Command").val();
    var documentlist = new Array();
    $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
        var row = $(this);
        var docList = {};
        var docno = row.find("#PD_OrderQtyDocNo").val();
        var docdt = row.find("#PD_OrderQtyDocDate").val();

        docList.docno = docno;
        docList.docdt = docdt;
        documentlist.push(docList);
    });
    var vdoclist = JSON.stringify(documentlist);

    if (PL_Status == "" || PL_Status == null || PL_Status == "D") {
        if (Itemid != "" && Itemid != null && WarehouseId != "" && WarehouseId != null && lotno != ""
            && lotno != null && batchno != "" && batchno != null) {
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/DomesticPacking/GetOrderResItemStockBatchWise",
                data: {
                    ItemId: Itemid,
                    WarehouseId: WarehouseId,
                    Status: PL_Status,
                    LotNo: lotno,
                    BatchNo: batchno,
                    SelectedItemdetail: SelectedItemdetail,
                    SelectedDocdetail: vdoclist,
                    docid: docid,
                    TransType: TransType,
                    Command: Command
                },
                success: function (data) {
                    debugger;
                    $('#ItemOrderWiseReservedDetails').html(data);

                    $("#ItemNameOrdRevDetails").val(ItemName)
                    $("#HDItemIDOrdRevDetails").val(Itemid)
                    $("#HDWhidOrdRevDetails").val(WarehouseId)
                    $("#UOMOrdRevDetails").val(UOMName)
                    $("#hdnUomIdOrdRevDetails").val(UOMid)
                    $("#LotNumberOrdRevDetails").val(lotno)
                    $("#BatchNumberOrdRevDetails").val(batchno)
                    $("#AvailQtyOrdRevDetails").val(Avlqty)
                    $("#hdntotalAvlQty").val(totalAvlqty)
                    $("#hdResMfgName").val(Mfg_Name);
                    $("#hdResMfgMrp").val(Mfg_Mrp);
                    $("#hdResMfgDate").val(Mfg_Date);

                    var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#SaveOrderReservedItemBatchTbl TBODY TR").length > 0) {
                        $("#SaveOrderReservedItemBatchTbl TBODY TR").each(function () {
                            var row = $(this)
                            var BtItemId = row.find("#POR_ItemId").val();
                            var BtLotNo = row.find("#POR_LotNo").val();
                            var BtBatchNo = row.find("#POR_BatchNo").val();
                            if (BtItemId === Itemid && BtLotNo === lotno && BtBatchNo === batchno) {
                                TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#POR_IssueQty").val());
                            }
                        });
                    }

                    $("#OrderReservedStockTbl TBODY TR").each(function () {
                        var row = $(this)
                        var issueQty = row.find("#OrdResIssuedQty").val();
                        if (issueQty != null && issueQty != "") {
                            row.find("#OrdResIssuedQty").val(parseFloat(issueQty).toFixed(QtyDecDigit))
                        }
                    });
                    $("#OrdResTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));
                    $("#OrdResTotalAvlQuantity").text(parseFloat(totalAvlqty).toFixed(QtyDecDigit));
                    //Added by Suraj on 20-02-2024
                    try {
                        //For Auto fill Quantity on FIFO basis in the Batch Table.
                        //this will work only first time after save old value will come in the table
                        Cmn_AutoFillBatchQty("OrderReservedStockTbl", packedQty, "tblresqty", "OrdResIssuedQty", "OrdResTotalIssuedQuantity");
                    } catch (err) {
                        console.log('Error : ' + err.message)
                    }
                },
            });
        }
    }
    else {
        var Pack_No = $("#PackingNumber").val();
        var Pack_Dt = $("#txtpack_dt").val();
        var docid = $("#DocumentMenuId").val();
        var TransType = $("#qty_TransType").val();
        var Command = $("#Qty_pari_Command").val();
        $.ajax({
            type: "Post",
            url: "/ApplicationLayer/DomesticPacking/GetOrderResItemStockBatchWiseAfterInsert",
            data: {
                Pack_No: Pack_No,
                Pack_Date: Pack_Dt,
                ItemId: Itemid,
                Status: PL_Status,
                LotNo: lotno,
                BatchNo: batchno,
                docid: docid,
                TransType: TransType,
                Command: Command

            },
            success: function (data) {
                debugger;
                $('#ItemOrderWiseReservedDetails').html(data);

                $("#ItemNameOrdRevDetails").val(ItemName)
                $("#HDItemIDOrdRevDetails").val(Itemid)
                $("#HDWhidOrdRevDetails").val(WarehouseId)
                $("#UOMOrdRevDetails").val(UOMName)
                $("#hdnUomIdOrdRevDetails").val(UOMid)
                $("#LotNumberOrdRevDetails").val(lotno)
                $("#BatchNumberOrdRevDetails").val(batchno)
                $("#AvailQtyOrdRevDetails").val(Avlqty)
                $("#hdntotalAvlQty").val(totalAvlqty)

                var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

                if ($("#SaveOrderReservedItemBatchTbl TBODY TR").length > 0) {
                    $("#SaveOrderReservedItemBatchTbl TBODY TR").each(function () {
                        var row = $(this)
                        var BtItemId = row.find("#POR_ItemId").val();
                        var BtLotNo = row.find("#POR_LotNo").val();
                        var BtBatchNo = row.find("#POR_BatchNo").val();
                        if (BtItemId === Itemid && BtLotNo === lotno && BtBatchNo === batchno) {
                            TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#POR_IssueQty").val());
                        }
                    });
                }

                $("#OrderReservedStockTbl TBODY TR").each(function () {
                    var row = $(this)
                    var issueQty = row.find("#OrdResIssuedQty").val();
                    if (issueQty != null && issueQty != "") {
                        row.find("#OrdResIssuedQty").val(parseFloat(issueQty).toFixed(QtyDecDigit))
                    }
                });
                $("#OrdResTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));
                $("#OrdResTotalAvlQuantity").text(parseFloat(totalAvlqty).toFixed(QtyDecDigit));
            },
        });
    }
}

function OnClickIssueForResSubItemInfo() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var ItemName = $("#ItemNameBatchWise").val();
    var Itemid = $("#HDItemNameBatchWise").val();
    var WarehouseId = $("#HDWhIDBatchWise").val();
    var UOMName = $("#UOMBatchWise").val();
    var BatchQty = $("#QuantityBatchWise").val();
    var PL_Status = $("#hd_Status").val().trim();
    var subitem = $("#HDBatchSubItem").val().trim();
    var docid = $("#DocumentMenuId").val();
    //PLBindOrderReservedItemBatchDetail();
    var OrdResItemBatchList = new Array();
    var rowcount = $('#SaveOrderReservedItemBatchTbl tr').length;
    if (rowcount > 1) {

        $("#SaveOrderReservedItemBatchTbl TBODY TR #POR_ItemId[value='" + Itemid + "']").closest('tr').each(function () {
            var row = $(this)
            var OrdResbatchList = {};

            OrdResbatchList.OrderNo = row.find('#POR_OrderNo').val();
            OrdResbatchList.OrderDt = row.find('#POR_OrderDate').val();
            OrdResbatchList.ItemId = row.find('#POR_ItemId').val();
            OrdResbatchList.UOMId = row.find('#POR_UOMId').val();
            OrdResbatchList.ResQty = row.find('#POR_ResAvlStk').val();
            OrdResbatchList.IssueQty = row.find('#POR_IssueQty').val();

            OrdResItemBatchList.push(OrdResbatchList);

        });
        var FinalOrdResItemBatchList = JSON.stringify(OrdResItemBatchList);
        //$("#HDSelectedOrderReservedBatchwiseDetails").val(FinalOrdResItemBatchList);
    }
    var SelectedItemdetail = FinalOrdResItemBatchList;//$("#HDSelectedOrderReservedBatchwiseDetails").val();



    var documentlist = new Array();
    if (PL_Status == "D" || PL_Status == "F" || PL_Status == "") {
        $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
            var row = $(this);
            var docList = {};
            var docno = row.find("#PD_OrderQtyDocNo").val();
            var docdt = row.find("#PD_OrderQtyDocDate").val();

            docList.docno = docno;
            docList.docdt = docdt;
            documentlist.push(docList);
        });
    } else {
        var docList = {};
        docList.docno = $("#PackingNumber").val();
        docList.docdt = $("#txtpack_dt").val();
        documentlist.push(docList);
    }
    var TransType = $("#qty_TransType").val();
    var Command = $("#Qty_pari_Command").val();
    var docid = $("#DocumentMenuId").val();
    var vdoclist = JSON.stringify(documentlist);
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/DomesticPacking/GetOrderResItemStockForSubItemBatchWise",
        data: {
            ItemId: Itemid,
            WarehouseId: WarehouseId,
            Status: PL_Status,
            SelectedItemdetail: SelectedItemdetail,
            SelectedDocdetail: vdoclist,
            docid, docid,
            TransType: TransType,
            Command: Command

        },
        success: function (data) {
            debugger;
            $('#ItemOrderWiseSubItemReservedDetails').html(data);
            if (subitem == "N") {
                $("#SubItemPackResDetailQty").attr("disabled", true);
            } else {
                $("#SubItemPackResDetailQty").attr("disabled", false);
            }
            $("#ItemNameOrdRevSubItemDetails").val(ItemName);
            $("#HDItemIDOrdRevSubItemDetails").val(Itemid);
            $("#HDWhidOrdRevSubItemDetails").val(WarehouseId);
            $("#UOMOrdRevSubItemDetails").val(UOMName);

            var tbl = $("#OrderReservedStockSubItemTbl tbody tr");
            let resQty = 0;
            tbl.each(function () {
                var row = $(this);
                resQty = parseFloat(resQty) + parseFloat(CheckNullNumber(row.find("#OrdResIssuedQty").val()));
            });
            if (parseFloat(resQty) < parseFloat(BatchQty)) {
                BatchQty = parseFloat(resQty).toFixed(QtyDecDigit);
            }
            $("#OrdReserveQuantitySubItemDetails").val(BatchQty);

        },
    });

}


function PLBindOrderReservedItemBatchDetail() {
    var rowcount = $('#SaveOrderReservedItemBatchTbl tr').length;
    if (rowcount > 1) {
        var OrdResItemBatchList = new Array();
        $("#SaveOrderReservedItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var OrdResbatchList = {};

            OrdResbatchList.OrderNo = row.find('#POR_OrderNo').val();
            OrdResbatchList.OrderDt = row.find('#POR_OrderDate').val();
            OrdResbatchList.ItemId = row.find('#POR_ItemId').val();
            OrdResbatchList.UOMId = row.find('#POR_UOMId').val();
            OrdResbatchList.LotNo = row.find('#POR_LotNo').val();
            OrdResbatchList.BatchNo = row.find('#POR_BatchNo').val();
            OrdResbatchList.ResQty = row.find('#POR_ResAvlStk').val();
            OrdResbatchList.IssueQty = row.find('#POR_IssueQty').val();
            OrdResbatchList.mfg_name = row.find('#POR_MfgName').val();
            OrdResbatchList.mfg_mrp = row.find('#POR_MfgMrp').val();
            OrdResbatchList.mfg_date = row.find('#POR_MfgDate').val();

            OrdResItemBatchList.push(OrdResbatchList);

        });
        var FinalOrdResItemBatchList = JSON.stringify(OrdResItemBatchList);
        $("#HDSelectedOrderReservedBatchwiseDetails").val(FinalOrdResItemBatchList);
    }
}

function PL_PackSrlznDetail() {
    debugger;
    var Pack_ItemName = $('#Pack_ItemName').val();
    var Pack_UomName = $('#Pack_UOM').val();
    var rowcount = $('#PackSrlzsnTbody tr').length;
    var List = new Array();
    if (rowcount > 0) {
       
        $("#PackSrlzsnTbody tr").each(function () {
            var row = $(this)
            var ListItem = {};
            debugger;
            ListItem.ItemId = row.find('#ItemId').text();
            ListItem.ItemName = Pack_ItemName;
            ListItem.UOMId = row.find('#UOMId').text();
            ListItem.UOMName = Pack_UomName;
            ListItem.PackQty = row.find('#PackQty').text();
            ListItem.SerialFrom = row.find('#SerialFrom').text();
            ListItem.SerialTo = row.find('#SerialTo').text();        
            ListItem.QtyPerPack = row.find('#QtyPerPack').text();
            ListItem.QtyPerInner = row.find('#QtyPerInner').text();
            ListItem.tblTotalInnerBox = row.find('#tblTotalInnerBox').text();
            ListItem.PhyPerPack = row.find('#PhyPerPack').text();
            ListItem.NetWeight = row.find('#NetWeight').text();
            ListItem.GrossWeight = row.find('#GrossWeight').text();
            ListItem.TotalPack = row.find('#TotalPacks').text();
            ListItem.TotalQty = row.find('#TotalQty').text();
            ListItem.TotalNetWeight = row.find('#TotalNetWeight').text();
            ListItem.TotalGrossWeight = row.find('#TotalGrossWeight').text();
            ListItem.Netweight_perpiece = row.find('#Netweight_perpiece').text();/*Added By Nitesh 24-11-2023 for Net_weight Per Piece*/
            ListItem.GrossWeight_perpiece = row.find('#GrossWeight_perpiece').text();/*Added By Nitesh 24-11-2023 for Gross_weight Per Piece*/


            List.push(ListItem);
        });
    }
    var FinalList = JSON.stringify(List);
    $("#hdn_PackingSrlzn").val(FinalList);
}
function OnClickItemOrdRes_Resetbtn() {
    $("#OrderReservedStockTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#OrdResIssuedQty").val("");
    });
    $("#OrdResTotalIssuedQuantity").text("");
}
function OnClickBtnItemOrderResSaveAndExit() {
    debugger
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    //var ItemPackedQty = $("#QuantityBatchWise").val();
    var TotalAvlQty = $("#hdntotalAvlQty").val();
    var TotalIssueQty = 0;
    var FlagMsg = "N";
    $("#OrderReservedStockTbl TBODY TR").each(function () {
        var row = $(this);
        var IssuedQuantity = row.find("#OrdResIssuedQty").val();
        if (IssuedQuantity != "" && IssuedQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(0)) {
                TotalIssueQty = parseFloat(TotalIssueQty) + parseFloat(IssuedQuantity);
            }
        }
    });
    if (parseFloat(TotalIssueQty) > parseFloat(TotalAvlQty)) {
        FlagMsg = "Y";
        swal("", $("#IssuedqtyshouldnotbegreaterthanTotalAvlqty").text(), "warning");
        return false;
    }
    if (FlagMsg === "N") {
        debugger;
        var SelectedItem = $("#HDItemIDOrdRevDetails").val();
        var SelectedLotNo = $("#LotNumberOrdRevDetails").val();
        var SelectedBatchNo = $("#BatchNumberOrdRevDetails").val();
        $("#SaveOrderReservedItemBatchTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#POR_ItemId").val();
            var rowlotno = row.find("#POR_LotNo").val();
            var rowbatchno = row.find("#POR_BatchNo").val();
            if (rowitem == SelectedItem && SelectedLotNo == rowlotno && SelectedBatchNo == rowbatchno) {
                debugger;
                $(this).remove();
            }
        });

        $("#OrderReservedStockTbl TBODY TR").each(function () {
            debugger;
            var row = $(this);
            var ItemIssueQuantity = row.find("#OrdResIssuedQty").val();
            if (ItemIssueQuantity != "" && ItemIssueQuantity != null) {

                var OrderNo = row.find("#tblorderno").val();
                var OrderDate = row.find("#hdn_orderdt").val();
                var ItemId = $("#HDItemIDOrdRevDetails").val();
                var UOMId = $("#hdnUomIdOrdRevDetails").val();
                var LotNo = $("#LotNumberOrdRevDetails").val();
                var BatchNo = $("#BatchNumberOrdRevDetails").val();
                var BatchResQty = row.find("#tblresqty").val();
                var IssueQty = row.find("#OrdResIssuedQty").val();
                var MfgName = $("#hdResMfgName").val();
                var MfgMrp = $("#hdResMfgMrp").val();
                var MfgDate = $("#hdResMfgDate").val();

                $('#SaveOrderReservedItemBatchTbl tbody').append(`<tr>
                        <td><input type="text" id="POR_OrderNo" value="${OrderNo}" /></td>
                        <td><input type="text" id="POR_OrderDate" value="${OrderDate}" /></td>
                        <td><input type="text" id="POR_ItemId" value="${ItemId}" /></td>
                        <td><input type="text" id="POR_UOMId" value="${UOMId}" /></td>
                        <td><input type="text" id="POR_LotNo" value="${LotNo}" /></td>
                        <td><input type="text" id="POR_BatchNo" value="${BatchNo}" /></td>
                        <td><input type="text" id="POR_ResAvlStk" value="${BatchResQty}" /></td>
                        <td><input type="text" id="POR_IssueQty" value="${IssueQty}" /></td>
                        <td><input type="text" id="POR_MfgName" value="${MfgName}" /></td>
                        <td><input type="text" id="POR_MfgMrp" value="${MfgMrp}" /></td>
                        <td><input type="text" id="POR_MfgDate" value="${MfgDate}" /></td>
                </tr>`
                );
            }
        });

        $("#OrderWiseReserved").modal('hide');

        var OrderResItemId = $("#HDItemIDOrdRevDetails").val();
        var OrderResLotNo = $("#LotNumberOrdRevDetails").val();
        var OrderResBatchNo = $("#BatchNumberOrdRevDetails").val();

        $("#BatchWiseItemStockTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = $("#HDItemNameBatchWise").val();
            var lotno = clickedrow.find("#Lot").val();
            var batchno = clickedrow.find("#BatchNumber").val();
            if (ItemId == OrderResItemId && lotno == OrderResLotNo && batchno == OrderResBatchNo) {

                clickedrow.find("#IssuedQuantity").val(parseFloat(TotalIssueQty).toFixed(QtyDecDigit));
            }
        });
        TotalBatchIssueQty();
    }
}
function OnChangeOrderResIssueQty(evt) {
    var TotalIssueQty = 0;
    try {
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103145115") {
            QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
        }
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssuedQuantity = clickedrow.find("#OrdResIssuedQty").val();

        if (IssuedQuantity != null && IssuedQuantity != "") {
            clickedrow.find("#OrdResIssuedQty").val(parseFloat(IssuedQuantity).toFixed(QtyDecDigit));
        }
        else {
            clickedrow.find("#OrdResIssuedQty").val(parseFloat(0).toFixed(QtyDecDigit));
        }

        $("#OrderReservedStockTbl TBODY TR").each(function () {
            var row = $(this);
            var Issueqty = row.find("#OrdResIssuedQty").val();
            if (Issueqty != "" && Issueqty != null) {
                TotalIssueQty = parseFloat(TotalIssueQty) + parseFloat(Issueqty);
            }
            debugger;
        });

        $("#OrdResTotalIssuedQuantity").text(parseFloat(TotalIssueQty).toFixed(QtyDecDigit));
    }
    catch (err) {
        console.log("Error : " + err.message);
    }
}
//---------------------------------End---------------------------------//

//-----------------------------Serial No Details---------------------------//
function CheckItemSerialValidation() {
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    $("#PDItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var PackeQuantity = clickedrow.find("#PackedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                clickedrow.find("#btnserialdeatil").css("border-color", "red");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#PD_SerialIssueQty').val();
                    var srialitemid = currentRow.find('#PD_SerialItemId').val();
                    var srialuomid = currentRow.find('#PD_SerialUOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });

                if (parseFloat(PackedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                    clickedrow.find("#btnserialdeatil").css("border-color", "#007bff");
                }
                else {
                    clickedrow.find("#btnserialdeatil").css("border-color", "red");
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
        swal("", $("#SerializedQtydoesnotmatchwithShippedQty").text(), "warning");
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
function ItemStockSerialWise(el, evt) {
    try {
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103145115") {
            QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
        }
        debugger;
        PLBindItemSerialDetail();
        var clickedrow = $(evt.target).closest("tr");
        var Index = clickedrow.find("#SpanRowId").text();
        var ddlId = "#wh_id" + Index;
        var WarehouseId = clickedrow.find(ddlId).val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var ItemId = clickedrow.find("#hdItemId").val();;
        var UOMID = clickedrow.find("#hdUOMId").val();

        var PackedQty = clickedrow.find("#PackedQuantity").val();

        var SelectedItemSerial = $("#HDSelectedSerialwise").val();
        var TransType = $("#qty_TransType").val();
        var Command = $("#Qty_pari_Command").val();
        var docid = $("#DocumentMenuId").val();
        var Amend = $("#Amend").val();
        var PL_Status = $("#hd_Status").val().trim();
        if (PL_Status == "" || PL_Status == null || PL_Status == "D" || Amend == "Amend") {
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/DomesticPacking/getItemstockSerialWise",
                data: {
                    ItemId: ItemId,
                    Status: PL_Status,
                    WarehouseId: WarehouseId,
                    SelectedItemSerial: SelectedItemSerial,
                    docid: docid,
                    TransType: TransType,
                    Command: Command,
                    Amend: Amend

                },
                success: function (data) {
                    debugger;
                    $('#ItemStockSerialWise').html(data);

                    $("#ItemNameSerialWise").val(ItemName);
                    $("#UOMSerialWise").val(UOMName);
                    $("#QuantitySerialWise").val(PackedQty);

                    $("#HDItemIDSerialWise").val(ItemId);
                    $("#HDUOMIDSerialWise").val(UOMID);

                    var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                        $("#SaveItemSerialTbl TBODY TR").each(function () {
                            var row = $(this)
                            var Row_ItemId = row.find("#PD_SerialItemId").val();
                            if (Row_ItemId === ItemId) {
                                TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#PD_SerialIssueQty").val());
                            }
                        });
                    }
                    $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
                },
            });
        }
        else /*if (PL_Status == "A" || PL_Status == "C")*/ {
            //var PL_Type = "D";
            var PL_No = $("#PackingNumber").val();
            var PL_Date = $("#txtpack_dt").val();
            var docid = $("#DocumentMenuId").val();
            var TransType = $("#qty_TransType").val();
            var Command = $("#Qty_pari_Command").val();
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/DomesticPacking/getItemstockSerialWiseAfterInsert",
                data: {
                    PL_No: PL_No,
                    PL_Date: PL_Date,
                    Status: PL_Status,
                    ItemId: ItemId,
                    docid: docid,
                    TransType: TransType,
                    Command: Command
                },
                success: function (data) {
                    debugger;
                    $('#ItemStockSerialWise').html(data);
                    $("#ItemNameSerialWise").val(ItemName);
                    $("#UOMSerialWise").val(UOMName);
                    $("#QuantitySerialWise").val(PackedQty);
                    $("#HDItemIDSerialWise").val(ItemId);
                    $("#HDUOMIDSerialWise").val(UOMID);

                    var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                        $("#SaveItemSerialTbl TBODY TR").each(function () {
                            var row = $(this)
                            var ItemId = row.find("#PD_SerialItemId").val();
                            if (ItemId === ItemId) {
                                TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#PD_SerialIssueQty").val());
                            }
                        });
                    }
                    $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
                },
            });
        }
        //else {
        //    $.ajax({
        //        type: "Post",
        //        url: "/ApplicationLayer/DomesticPacking/getItemstockSerialWise",
        //        data: {
        //            ItemId: ItemId,
        //            Status: PL_Status,
        //            WarehouseId: WarehouseId,
        //            SelectedItemSerial: SelectedItemSerial
        //        },
        //        success: function (data) {
        //            debugger;
        //            $('#ItemStockSerialWise').html(data);

        //            $("#ItemNameSerialWise").val(ItemName);
        //            $("#UOMSerialWise").val(UOMName);
        //            $("#QuantitySerialWise").val(PackedQty);

        //            $("#HDItemIDSerialWise").val(ItemId);
        //            $("#HDUOMIDSerialWise").val(UOMID);

        //            var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

        //            if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
        //                $("#SaveItemSerialTbl TBODY TR").each(function () {
        //                    var row = $(this)
        //                    var ItemId = row.find("#PD_SerialItemId").val();
        //                    if (ItemId === ItemId) {
        //                        TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#PD_SerialIssueQty").val());
        //                    }
        //                });
        //            }
        //            $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
        //        },
        //    });
        //}
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function onchangeChkItemSerialWise() {
    var TotalIssueLot = 0;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    debugger;
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        if (row.find("#ChkItemSerialWise").is(":checked")) {
            TotalIssueLot = parseFloat(TotalIssueLot) + 1;
        }
    });
    $("#TotalIssuedSerial").text(parseFloat(TotalIssueLot).toFixed(QtyDecDigit));
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
        swal("", $("#IssuedqtyshouldbeequaltoPackedqty").text(), "warning");
    }
    else {
        debugger;
        var SelectedItem = $("#HDItemIDSerialWise").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#PD_SerialItemId").val();
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
            var ItemIssueQuantity = "1";
            var ItemSerialNO = row.find("#SerialNumberDetail").val();
            if (row.find("#ChkItemSerialWise").is(":checked")) {
                $('#SaveItemSerialTbl tbody').append(`<tr>
            <td><input type="text" id="PD_SerialItemId" value="${ItemId}" /></td>
            <td><input type="text" id="PD_SerialUOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="PD_SerialLOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="PD_SerialIssueQty" value="${ItemIssueQuantity}" /></td>
            <td><input type="text" id="PD_BatchSerialNO" value="${ItemSerialNO}" /></td>
            </tr>`);
            }
        });
        $("#SerialDetail").modal('hide');

        $("#PDItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#btnserialdeatil").css("border-color", "#007bff");
                ValidateEyeColor(clickedrow, "btnserialdeatil", "N");

            }
        });
    }
}
function PLBindItemSerialDetail() {
    var serialrowcount = $('#SaveItemSerialTbl tr').length;
    if (serialrowcount > 1) {
        var ItemSerialList = new Array();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this)
            var SerialList = {};
            SerialList.ItemId = row.find("#PD_SerialItemId").val();
            SerialList.UOMId = row.find("#PD_SerialUOMId").val();
            SerialList.LOTId = row.find("#PD_SerialLOTNo").val();
            SerialList.IssuedQuantity = row.find("#PD_SerialIssueQty").val();
            SerialList.SerialNO = row.find("#PD_BatchSerialNO").val().trim();
            SerialList.mfg_name = row.find("#PD_SerialMfgName").val().trim();
            SerialList.mfg_mrp = row.find("#PD_SerialMfgMrp").val().trim();
            SerialList.mfg_date = row.find("#PD_SerialMfgDate").val().trim();
            ItemSerialList.push(SerialList);
            debugger;
        });
        var str2 = JSON.stringify(ItemSerialList);
        $("#HDSelectedSerialwise").val(str2);

    }

}
function onclickbtnItemSerialDiscardAndExit() {
    $("#SerialDetail").modal('hide');
}
//----------------------------------End-------------------------------//
//-----------------------------Packaging Detail---------------------------//
function OnClickPackagingDetailBtn(el, evt) {
    try {
        debugger;
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103145115") {
            QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
        }
        var clickedrow = $(evt.target).closest("tr");

        $("#packagingItem_Error").css("display", "none");
        $("#PackagingItemddl").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-PackagingItemddl-container"]').css("border-color", "#ced4da");
        var ItemName = clickedrow.find("#ItemName").val();
        var ItmCode = clickedrow.find("#hdItemId").val();
        var UOMName = clickedrow.find("#UOM").val();

        var PackagingItemCode = clickedrow.find("#hdpackagingItemId").val();
        var PackagingItemName = clickedrow.find("#hdpackagingItemName").val();
        var TotalCBM = clickedrow.find("#CBM").val();
        var PackedQty = clickedrow.find("#PackedQuantity").val();
        //var NoOfPacks = clickedrow.find("#NumberOfPacks").val();
        var PhysicalPacks = clickedrow.find("#PhysicalPacks").val();
        if (PackedQty === "") {
            PackedQty = "0";
        }
        if (PhysicalPacks === "") {
            PhysicalPacks = "0";
        }

        var docid = $("#DocumentMenuId").val();
        var TransType = $("#qty_TransType").val();
        var Command = $("#Qty_pari_Command").val();
        var PL_Status = $("#hd_Status").val().trim();
        var Amend = $("#Amend").val()
        if (ItmCode != "" || ItmCode != null) {
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/DomesticPacking/getItemPackagingDetail",
                data: {
                    Status: PL_Status,
                    docid: docid,
                    TransType: TransType,
                    Command: Command,
                    Amend: Amend
                },
                success: function (data) {
                    debugger;
                    $('#ItemPackagingDetails').html(data);

                    $("#PackagingDtlItemName").val(ItemName);
                    $("#hd_PackagingItemId").val(ItmCode);
                    $("#PackagingDtlUOM").val(UOMName);
                    $("#PackagingDtlPackedQty").val(parseFloat(PackedQty).toFixed(QtyDecDigit));

                    $("#PackagingPackedQuantity").val(parseFloat(PackedQty).toFixed(QtyDecDigit));
                    //$("#PackagingNumberOfPacks").val(parseFloat(NoOfPacks).toFixed(QtyDecDigit));
                    $("#PackagingNumberOfPacks").val(parseFloat(PhysicalPacks).toFixed(QtyDecDigit));
                    BindPackagingItemList();

                    //var ItemListData = JSON.parse(sessionStorage.getItem("itemList"));
                    //if (ItemListData != null && ItemListData != "") {
                    //    if (ItemListData.length > 0) {
                    //            $('#PackagingItemddl').append(`<optgroup class='def-cursor' id="PLTextddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                    //            for (var i = 0; i < ItemListData.length; i++) {
                    //                $('#PLTextddl').append(`<option data-uom="${ItemListData[i].uom_name}" value="${ItemListData[i].Item_id}">${ItemListData[i].Item_name}</option>`);
                    //            }
                    //            var firstEmptySelect = true;
                    //            $('#PackagingItemddl').select2({
                    //                templateResult: function (data) {
                    //                    var UOM = $(data.element).data('uom');
                    //                    var classAttr = $(data.element).attr('class');
                    //                    var hasClass = typeof classAttr != 'undefined';
                    //                    classAttr = hasClass ? ' ' + classAttr : '';
                    //                    var $result = $(
                    //                        '<div class="row">' +
                    //                        '<div class="col-md-8 col-xs-8' + classAttr + '">' + data.text + '</div>' +
                    //                        '<div class="col-md-4 col-xs-4' + classAttr + '">' + UOM + '</div>' +
                    //                        '</div>'
                    //                    );
                    //                    return $result;
                    //                    firstEmptySelect = false;
                    //                }
                    //            });
                    //    }
                    //}

                    if (PackagingItemCode !== null && PackagingItemCode != "") {
                        //$("#PackagingItemddl").val(PackagingItemCode).trigger("change");
                        $("#PackagingItemddl").append('<option value=' + PackagingItemCode + ' selected>' + PackagingItemName + '</option>');
                        $("#PackagingItemddl").trigger("change");
                    }

                },
            });
        }

        //if (ItemId != "" && ItemId != null && PackedQty != "" && parseFloat(PackedQty) != parseFloat("0") && NoOfPacks != "" && parseFloat(NoOfPacks) != parseFloat("0")) {
        //    $("#PackagingDtlItemName").val(ItemName);
        //    $("#hd_PackagingItemId").val(ItemId);
        //    $("#PackagingDtlUOM").val(UOMName);
        //    $("#PackagingDtlPackedQty").val(parseFloat(PackedQty).toFixed(QtyDecDigit));

        //    var P_FItemID = "";

        //    $("#SaveItemPackagingDetails TBODY TR").each(function () {
        //        var row = $(this);
        //        var rowitem = row.find("#PD_ItemID").val();
        //        if (rowitem == ItemId) {
        //            P_FItemID = rowitem;
        //        }
        //    });

        //    if (P_FItemID === "" || P_FItemID === null) {
        //        $("#PackagingItemddl").attr('onchange', "");
        //        $("#PackagingItemddl").val(0).trigger("change");
        //        $("#PackagingDimensions").val("");
        //        $("#PackagingPackedQuantity").val(parseFloat(PackedQty).toFixed(QtyDecDigit));
        //        $("#PackagingNumberOfPacks").val(parseFloat(NoOfPacks).toFixed(QtyDecDigit));
        //        $("#PackagingCBM").val("");
        //        $("#PackagingItemddl").attr('onchange', "OnChangePackagingItemList()");
        //    }
        //    else {
        //        $("#SaveItemPackagingDetails TBODY TR").each(function () {
        //            var row = $(this);
        //            var rowitem = row.find("#PD_ItemID").val();
        //            if (rowitem == ItemId) {
        //                $("#PackagingItemddl").attr('onchange', "");
        //                $("#PackagingItemddl").val(row.find("#PD_PackagingItemID").val()).trigger("change");
        //                $("#PackagingDimensions").val(row.find("#PD_PackagingDimension").val());
        //                $("#PackagingPackedQuantity").val(parseFloat(row.find("#PD_PackagingPackedQty").val()).toFixed(QtyDecDigit));
        //                $("#PackagingNumberOfPacks").val(parseFloat(row.find("#PD_PackagingNoofPacks").val()).toFixed(QtyDecDigit));
        //                $("#PackagingCBM").val(parseFloat(row.find("#PD_PackagingTotalCBM").val()).toFixed(QtyDecDigit));

        //                $("#PackagingItemddl").attr('onchange', "OnChangePackagingItemList()");
        //            }
        //        });
        //    }
        //}
        //else {
        //    $("#PackagingItemddl").attr('onchange', "");

        //    $("#PackagingItemddl").val(0).trigger("change");
        //    $("#PackagingDimensions").val("");
        //    $("#PackagingPackedQuantity").val("");
        //    $("#PackagingNumberOfPacks").val("");
        //    $("#PackagingCBM").val("");

        //    $("#PackagingItemddl").attr('onchange', "OnChangePackagingItemList()");
        //}

    } catch (err) {
        console.log("Error : " + err.message);
    }
}
function OnChangePackagingItemList() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    var PackagingItemID = $("#PackagingItemddl").val();
    var PackagingNoOfPacks = $("#PackagingNumberOfPacks").val();

    if (PackagingItemID != "0" && parseFloat(PackagingNoOfPacks) > 0) {
        $("#packagingItem_Error").css("display", "none");
        $("#PackagingItemddl").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-PackagingItemddl-container"]').css("border-color", "#ced4da");
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/DomesticPacking/getPackagingItemDetail",
                data: { Itemid: PackagingItemID },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var packagingcbm = parseFloat(arr.Table[0].pack_cbm).toFixed(QtyDecDigit);
                            var Total_CBM = parseFloat(packagingcbm) * parseFloat(PackagingNoOfPacks);

                            $("#PackagingDimensions").val(arr.Table[0].dimension);
                            $("#PackagingCBM").val(parseFloat(Total_CBM).toFixed(QtyDecDigit));
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    }
    else {
        $("#packagingItem_Error").text($("#valueReq").text());
        $("#packagingItem_Error").css("display", "block");
        $("#PackagingItemddl").css("border-color", "red");
        $('[aria-labelledby="select2-PackagingItemddl-container"]').css("border-color", "red");
    }
}
function OnclickPackagindSaveAndExitBtn() {
    debugger;
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    var Packagingitemid = $("#PackagingItemddl").val();
    var PackagingitemName = $("#PackagingItemddl option:selected").text();
    var P_itemid = $("#hd_PackagingItemId").val();
    var tcbm = $("#PackagingCBM").val();

    if (Packagingitemid === "0") {
        debugger;
        $("#packagingItem_Error").text($("#valueReq").text());
        $("#packagingItem_Error").css("display", "block");
        $("#PackagingItemddl").css("border-color", "red");
        $('[aria-labelledby="select2-PackagingItemddl-container"]').css("border-color", "red");
        return false;
    }
    else {
        $("#packagingItem_Error").css("display", "none");
        $("#PackagingItemddl").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-PackagingItemddl-container"]').css("border-color", "#ced4da");
    }

    if (Packagingitemid != "0" && P_itemid != "" && P_itemid != null && tcbm != null && parseFloat(tcbm) > 0) {

        $("#SaveItemPackagingDetails TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#PD_ItemID").val();
            if (rowitem == P_itemid) {
                debugger;
                $(this).remove();
            }
        });

        var dimension = $("#PackagingDimensions").val();
        var packedqty = $("#PackagingPackedQuantity").val();
        var noofpacks = $("#PackagingNumberOfPacks").val();
        var cbm = $("#PackagingCBM").val();

        $('#SaveItemPackagingDetails tbody').append(`<tr>
         <td><input type="text" id="PD_ItemID" value="${P_itemid}" /></td>
         <td><input type="text" id="PD_PackagingItemID" value="${Packagingitemid}" /></td>
         <td><input type="text" id="PD_PackagingDimension" value="${dimension}" /></td>
         <td><input type="text" id="PD_PackagingPackedQty" value="${packedqty}" /></td>
         <td><input type="text" id="PD_PackagingNoofPacks" value="${noofpacks}" /></td>
         <td><input type="text" id="PD_PackagingTotalCBM" value="${cbm}" /></td>
         </tr>`);

        $("#PackagingDetail").modal('hide');

        $("#PDItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == P_itemid) {
                clickedrow.find("#CBM").val(parseFloat(cbm).toFixed(QtyDecDigit));
                clickedrow.find("#hdpackagingItemId").val(Packagingitemid);
                clickedrow.find("#hdpackagingItemName").val(PackagingitemName);
            }
        });
        SumGross_Net_CBM("CBM");
    }
}

//-----------------------------------End---------------------------------//

//-----------------------------------WorkFlow-------------------------------------------//
function GetViewDetails() {
    debugger
    $("#hdDoc_No").val($("#PackingNumber").val());
    //if ($("#WFBarStatus").val() != null && $("#WFBarStatus").val() != "") {
    //    var arr2 = $("#WFBarStatus").val();
    //    var arr = JSON.parse(arr2);
    //    $("#WFBarStatus").val();
    //    if (arr.length > 0) {
    //        for (var yw = 0; yw < arr.length; yw++) {
    //            var Level = parseInt(arr[yw].level);
    //            var nextLevel = parseInt(Level) + 1;
    //            $("#a_" + Level).removeClass("disabled");
    //            $("#a_" + Level).addClass("done");

    //            $("#a_" + nextLevel).removeClass("disabled");
    //            $("#a_" + nextLevel).addClass("selected");
    //        }
    //    }
    //}
    //if ($("#WFStatus").val() != null && $("#WFStatus").val() != "") {
    //    debugger
    //    var arr2 = $("#WFStatus").val();
    //    var arr = JSON.parse(arr2);
    //    $("#WFStatus").val();
    //    if (arr.length > 0) {
    //        $("#hd_nextlevel").val(arr[0].nextlevel);
    //        $("#hd_currlevel").val(arr[0].userlevel);
    //        $("#hd_createrlevel").val(arr[0].createrlevel);
    //        var createlvl = arr[0].createrlevel;
    //        var userlvl = arr[0].userlevel;
    //        if (AvoidDot(createlvl) == false) {
    //            createlvl = 0;
    //        }
    //        if (AvoidDot(userlvl) == false) {
    //            userlvl = 0;
    //        }
    //        if (parseFloat(createlvl) < (parseFloat(userlvl) - parseFloat(createlvl))) {
    //            $("#radio_revert").prop("disabled", false);
    //        }
    //        else {
    //            $("#radio_revert").prop("disabled", true);
    //        }
    //        if (arr[0].nextlevel === "0") {
    //            $("#span_forward").css("display", "none");
    //            $("#span_approve").removeAttr("style");
    //            $("#radio_forward").val("Approve");
    //        }
    //        else {

    //        }
    //    }
    //}
    //debugger;
    //var create_Id = $("#Create_ID").val();
    //var UserID = $("#UserID").text();
    //if (UserID === create_Id) {
    //    $("#radio_reject").prop("disabled", true);
    //    $("#radio_revert").prop("disabled", true);
    //}

}

function ForwardBtnClick() {
    debugger;
    //var OrderStatus = "";
    //OrderStatus = $('#hd_Status').val().trim();
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

    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/

    //let errorFlg = "N";
    //var flagNoOfPacked = CheckPackItemValidations_NoOfPacked();
    //if (flagNoOfPacked == false) {
    //    errorFlg = "Y";
    //} else {

    //}

    //var HdnPackSerialization = $("#HdnPackSerialization").val();
    //if (HdnPackSerialization == "Y") {
    //    var PL_PackSrlznDtlCheckValid = fn_PackSrlznDtlCheckValid();
    //    if (PL_PackSrlznDtlCheckValid == false) {
    //        errorFlg = "Y";
    //    }
    //}
    //if (errorFlg == "Y") {
    //    $("#btn_forward").attr("data-target", "");
    //    $("#btn_approve").attr("data-target", "");
    //    $("#OKBtn_FW").attr("disabled", true);
    //    return false;
    //} else {
    //    $("#OKBtn_FW").attr("disabled", false);
    //    $("#btn_forward").attr("data-target", "#Forward_Pop");
    //    $("#btn_approve").attr("data-target", "#Forward_Pop");
    //}
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var PkDate = $("#txtpack_dt").val();
        $.ajax({
            type: "POST",
            /*url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: PkDate
            },
            async: false,
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    let errorFlg = "N";
                    var flagNoOfPacked = CheckPackItemValidations_NoOfPacked();
                    if (flagNoOfPacked == false) {
                        errorFlg = "Y";
                    }
                    var HdnPackSerialization = $("#HdnPackSerialization").val();
                    if (HdnPackSerialization == "Y") {
                        var PL_PackSrlznDtlCheckValid = fn_PackSrlznDtlCheckValid();
                        if (PL_PackSrlznDtlCheckValid == false) {
                            errorFlg = "Y";
                        }
                    }
                    if (errorFlg == "Y") {
                        $("#btn_forward").attr("data-target", "");
                        $("#btn_approve").attr("data-target", "");
                        $("#OKBtn_FW").attr("disabled", true);
                        return false;
                    }
                    else {
                        $("#OKBtn_FW").attr("disabled", false);
                        $("#btn_forward").attr("data-target", "#Forward_Pop");
                        $("#btn_approve").attr("data-target", "#Forward_Pop");
                        var OrderStatus = "";
                        OrderStatus = $('#hd_Status').val().trim();
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

                    //var OrderStatus = "";
                    //OrderStatus = $('#hd_Status').val().trim();
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
                }
                else {/* to chk Financial year exist or not*/
                    /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                    /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
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
    var PQNo = "";
    var PQDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var mailerror = "";
    Remarks = $("#fw_remarks").val();
    PQNo = $("#PackingNumber").val();
    PQDate = $("#txtpack_dt").val();
    docid = $("#DocumentMenuId").val();
    WF_Status1 = $("#WF_Status1").val();
    var TrancType = (PQNo + ',' + docid + ',' + WF_Status1);
    var WF_Status1 = $("#WF_Status1").val();
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
    try {
        if (fwchkval != "Approve") {
            var pdfAlertEmailFilePath = "PackingList_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
            var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(PQNo, PQDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/DomesticPacking/SavePdfDocToSendOnEmailAlert");
            if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
                pdfAlertEmailFilePath = "";
            }
        }
    }
    catch {

    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && PQNo != "" && PQDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DomesticPacking/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/DomesticPacking/PackingListApprove?PackNo=" + PQNo + "&PackDate=" + PQDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1 + "&docid=" + docid;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && PQNo != "" && PQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DomesticPacking/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && PQNo != "" && PQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DomesticPacking/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(poNo, poDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/DomesticPacking/SavePdfDocToSendOnEmailAlert",
//        data: { soNo: poNo, soDate: poDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
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
    var Doc_No = $("#PackingNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
//-----------------------------------WorkFlow End-------------------------------------------//

//-----------------------------Packing Detail---------------------------//
function PackDetails_QtyFloatValueonly(el, evt) {
   // debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpQtyDigit ") == false) {
            return false;
        }

    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    
    }
    
    let key = evt.key;
    let selectedval = Cmn_SelectedTextInTextField(evt);
    let KeyLocation = evt.currentTarget.selectionStart;
    let valPer = el.value.splice(KeyLocation, 0, key);
   
    valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;
  
    let SerialFrom = $("#Pack_SerialFrom").val();
    let SerialTo = $("#Pack_SerialTo").val();
    let Pack_Quantity = $("#Pack_Quantity").val();
    let TotalPack = 0;
    let QtyPack = valPer;
    if (SerialFrom == "0" || SerialFrom == "" || SerialTo == "" || SerialTo == "0") {
        TotalPack = Pack_Quantity / QtyPack;
    }
    else {
         TotalPack = (parseInt(SerialTo) - parseInt(SerialFrom)) + 1;
    }
   
 
    let TotalQty = TotalPack * parseFloat(CheckNullNumber(QtyPack));
    
    if (parseFloat(TotalQty) > parseFloat(CheckNullNumber(Pack_Quantity))) {
        
        $("#Pack_QtyPack").css("border-color", "red");
        $("#Pack_QtyPackError").css("display", "block");
        $("#Pack_QtyPackError").text($("#ExceedingQty").text());
    }
    else {

        $("#Pack_QtyPack").css("border-color", "#ced4da");
        $("#Pack_QtyPackError").css("display", "none");
        $("#Pack_QtyPackError").text("");
    }

}

function PackDetails_AmtFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpValDigit ") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#ValDigit ") == false) {
            return false;
        }
    }
   
}
function PackDetails_AmtFloatWghtValueonly(el, evt) {
        if (Cmn_FloatValueonly(el, evt, "#WeightDigit ") == false) {
            return false;
        }
}
function OnClickAddPackDetail() {
    debugger;
    var ITEMid = $("#Pack_ItemName").val();
    var Packing_Quantity = $("#Pack_Quantity").val();

    if (ITEMid != "0" && ITEMid != "") {
        if (Packing_Quantity != "0" && Packing_Quantity != "" && parseFloat(Packing_Quantity) != parseFloat(0))
        {
            $("#Pack_Quantity").css("border-color", "#ced4da");
            $("#Pack_QuantityError").text("").css("display", "none");
            $("#Pack_QuantityError").css("display", "none");
            var ErrorFlag = "N";
            var SerialFrom = $("#Pack_SerialFrom").val();
            var SerialTo = $("#Pack_SerialTo").val();
            SerialFrom = (SerialFrom && SerialFrom !== "0" && SerialFrom !== "NaN") ? SerialFrom : "0";
            SerialTo = (SerialTo && SerialTo !== "0" && SerialTo !== "NaN") ? SerialTo : "0";
            if (SerialFrom == "0" || SerialFrom == "" || SerialTo == "0" || SerialTo == "") {
                SerialFrom = "0";
                SerialTo = "0";
            }
            if (validatePackingDetail() === false || CheckQtyPerPack_quantity() === false) {
                return false;
            }

            let ItemProps = $('#Pack_ItemName option:selected').data().data;
            let ItemName = ItemProps.text;
            let ItemId = ItemProps.id;
            let UOM = ItemProps.UOM;
            let UOMId = ItemProps.UomId;
            //let PackQty = ItemProps.PackQty;
            let PackQty = $("#Pack_Quantity").val();

            let FinTotalQty = 0;
            let MasterFintotalPacked = 0;

            $("#PackSrlzsnTable tbody tr #ItemId:contains('" + ItemId + "')").closest('tr').each(function () {
                let TotalQty = $(this).find("#TotalQty").text();
                FinTotalQty += parseFloat(CheckNullNumber(TotalQty));
            });
            $("#PDItmDetailsTbl tbody tr").each(function () {
                debugger;
                if ($(this).find("#hdItemId").val().trim() === ItemId) {
                    let PackedQuantity = $(this).find("#PackedQuantity").val();
                    MasterFintotalPacked += parseFloat(CheckNullNumber(PackedQuantity));
                }
            });
            if (MasterFintotalPacked < (parseFloat(CheckNullNumber(PackQty)) + parseFloat(CheckNullNumber(FinTotalQty)))) {
                $("#Pack_Quantity").css("border-color", "red");
                $("#Pack_QuantityError").text($('#ExceedingQty').text()).css("display", "block");
                ErrorFlag = "Y";
                return false;
            }
            else {
                $("#Pack_Quantity").css("border-color", "#ced4da");
                $("#Pack_QuantityError").text("").css("display", "none");
            }

            var DocumentMenuId = $("#DocumentMenuId").val();
            var QtyDecDigit = DocumentMenuId == "105103145115" ? $("#ExpImpQtyDigit").text() : $("#QtyDigit").text();


            var QtyPack = $("#Pack_QtyPack").val() || "0";
            var Total_Qty = $("#Pack_Quantity").val() || "0";
            var total_pack = 0, TotalPack = 0;

            if (SerialFrom === "0" || SerialTo === "0") {
                if (QtyPack !== "0" && QtyPack !== "NaN") {
                    total_pack = Total_Qty / QtyPack;
                    if (total_pack % 1 !== 0) {
                        $("#Pack_QtyPack").css("border-color", "red");
                        $("#Pack_QtyPackError").text($('#span_InvalidQuantity').text()).css("display", "block");
                        ErrorFlag = "Y";
                        return false;
                    } else {
                        $("#Pack_QtyPack").css("border-color", "#ced4da");
                        $("#Pack_QtyPackError").text("").css("display", "none");
                        TotalPack = total_pack;
                    }
                }

            }
            else {
                TotalPack = (parseInt(SerialTo) - parseInt(SerialFrom)) + 1;
            }
            if (Total_Qty % QtyPack !== 0) {
                $("#Pack_QtyPack").css("border-color", "red");
                $("#Pack_QtyPackError").text($('#span_InvalidQuantity').text()).css("display", "block");
                ErrorFlag = "Y";
                return false;
            } else {
                $("#Pack_QtyPack").css("border-color", "#ced4da");
                $("#Pack_QtyPackError").text("").css("display", "none");

            }
            var PerInner = parseFloat($("#Qty_PerInner").val()).toFixed(QtyDecDigit);
            
            if (parseFloat(QtyPack) != parseFloat(0))
            {
                if (parseFloat(CheckNullNumber(PerInner)) > parseFloat(CheckNullNumber(QtyPack))) {
                    $("#QtyPerInnerError").text($("#span_InvalidQuantity").text());
                    $("#Qty_PerInner").css("border-color", "red");
                    $("#QtyPerInnerError").css("display", "block");
                    ErrorFlag = "Y";
                    return false;
                }
                else {
                    $("#QtyPerInnerError").text("");
                    $("#QtyPerInnerError").css("display", "none");
                    $("#Qty_PerInner").css("border-color", "#ced4da");
                 //   var TotalInnerBox = parseFloat(qtyperpack) / parseFloat(qtyperinner)
                  //  $("#Qty_PerInner").val(parseFloat(qtyperinner).toFixed(QtyDecDigit))
                }
            }
            var TotalInner_Box = parseFloat($("#TotalInnerBox").val()).toFixed(QtyDecDigit);
            PerInner = (PerInner === "NaN" || PerInner === "" || PerInner == null) ? parseFloat(0) : PerInner;
            TotalInner_Box = (TotalInner_Box === "NaN" || TotalInner_Box === "" || TotalInner_Box == null) ? parseFloat(0) : TotalInner_Box;

            var TotalQty = parseFloat(CheckNullNumber(Total_Qty));
            var NetWeight = parseFloat(CheckNullNumber($("#Pack_NetWeight").val()));
            var TotalNetWeight = parseFloat(NetWeight * TotalPack).toFixed(WeightDecDigit);
            var GrossWeight = $("#Pack_GrossWeight").val();
            var TotalGrossWeight = GrossWeight * TotalPack;
            var NetWeight_perpiece = $("#netweightperpiece").val();
            var GrossWeight_piece = $("#grossweightperpiece").val();
            var PhyPack = "0";

            if (ErrorFlag === "N") {
                if (SerialFrom !== "0" && SerialTo !== "0") {
                    var MainArr = [];
                    for (var i = parseInt(SerialFrom); i <= parseInt(SerialTo); i++) {
                        MainArr.push(i);
                    }

                    if ($("#PackSrlzsnTable tbody tr  #ItemId:contains('" + ItemId + "')").length > 0) {
                        $("#PackSrlzsnTable tbody tr  #ItemId:contains('" + ItemId + "')").closest('tr').each(function () {
                            var arr = [], NewArr = [];
                            var CurrantRow = $(this);
                            var PackItemId = CurrantRow.find("#ItemId").text();

                            if (ItemId !== PackItemId) {
                                var SerialFrom1 = CurrantRow.find("#SerialFrom").text();
                                var SerialTo1 = CurrantRow.find("#SerialTo").text();
                                for (var j = parseInt(SerialFrom1); j <= parseInt(SerialTo1); j++) {
                                    arr.push(j);
                                }
                                for (var k = 0; k < MainArr.length; k++) {
                                    if (!arr.includes(MainArr[k])) {
                                        NewArr.push(MainArr[k]);
                                    }
                                }
                                MainArr = NewArr;
                            }
                        });
                        PhyPack = MainArr.length;
                    } else {
                        PhyPack = (parseInt(SerialTo) - parseInt(SerialFrom)) + 1;
                    }
                } else {
                    PhyPack = TotalPack;
                }
                if (!Number.isInteger(PhyPack) || !Number.isInteger(TotalPack)) {
                    $("#Pack_QtyPack").css("border-color", "red");
                    $("#Pack_QtyPackError").text($('#span_InvalidQuantity').text()).css("display", "block");
                    ErrorFlag = "Y";
                    return false;
                }
                AppendPackingListDetail({
                    SrNo: 0,
                    ItemName, ItemId, UOM, UOMId, PackQty, SerialFrom, SerialTo,
                    TotalPacks: TotalPack,
                    QtyPerPack: QtyPack,
                    PhyPerPack: PhyPack,
                    TotalQty,
                    netweight_perpiece: NetWeight_perpiece,
                    NetWeight: NetWeight,
                    TotalNetWeight: TotalNetWeight,
                    GrossWeight_perPiece: GrossWeight_piece,
                    GrossWeight: GrossWeight,
                    TotalGrossWeight: TotalGrossWeight,
                    PerInner: PerInner,
                    TotalInner_Box: TotalInner_Box
                });

                CalCulateTotalPackingDetail();
                ResetSerialNumber();
                ResetPhyCount();

                //  let hdnPack_Quantity = $("#hdnPack_QuantitySerialization").val();
                //  let PackedQty = parseFloat(hdnPack_Quantity) - parseFloat(TotalQty);
                let PackedQty = parseFloat($("#Pack_Quantity").val()) - parseFloat(TotalQty);
                // $("#hdnPack_QuantitySerialization").val(PackedQty);
                $("#Pack_Quantity").val((PackedQty).toFixed(QtyDecDigit));

                $("#Pack_SerialFrom, #Pack_SerialTo, #Pack_QtyPack, #Pack_NetWeight, #Pack_GrossWeight, #grossweightperpiece, #netweightperpiece, #Qty_PerInner,#TotalInnerBox").val("");

                var srFrom = (SerialTo === "0") ? "0" : parseFloat(SerialTo) + 1;
                $("#Pack_SerialFrom").val(srFrom);

                if (PackedQty === 0) {
                    $("#Pack_ItemName").val(0).change();
                }

                CalCulateTotalPackingDetailsForAllItems();
                PackingNoSetOnItemDetail(ItemId);
            }

            $("#AutoSerialization").attr("disabled", $("#PackSrlzsnTable tbody tr").length <= 0);
        }
        else {
            $("#Pack_Quantity").css("border-color", "red");
            $("#Pack_QuantityError").text($("#valueReq").text());
            $("#Pack_QuantityError").css("display", "block");
        }
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
    }
}
function onchangePackedQuantitySerlization() {


    $("#Pack_Quantity").css("border-color", "#ced4da");
    $("#Pack_QuantityError").text("").css("display", "none");
    $("#Pack_QuantityError").css("display", "none");
}
function onclickAutoSerialization() {

    var $rows = $("#PackSrlzsnTable tbody tr");
    if ($rows.length === 0) return;

    $('#loader').show();

    // Allow loader to render
    setTimeout(function () {

        let currentSerial = 1;
        let currentSerial_1 = 1;
        let itemSet = {};

        $rows.each(function () {

            var $row = $(this);

            var sr_no = parseInt($row.find("#SrNo").text());
            if (sr_no !== currentSerial_1) return;

            var ItemId = $row.find("#ItemId").text();

            var totalQty = parseFloat($row.find("#TotalQty").text());
            var PerPack_Qty = parseFloat($row.find("#QtyPerPack").text());
            var total_Packs = parseInt($row.find("#TotalPacks").text());

            var qtyPerPack = totalQty / PerPack_Qty;
            var serialFrom = currentSerial;
            var serialTo = currentSerial + qtyPerPack - 1;

            // SAME VALIDATIONS
            if (serialFrom % 1 !== 0 || serialTo % 1 !== 0) {
                swal("", $("#span_InvalidSerialNumber").text(), "warning");
                $('#loader').hide();
                return false;
            }

            if (!Number.isInteger(total_Packs)) {
                $("#Pack_QtyPack").css("border-color", "red");
                $("#Pack_QtyPackError")
                    .text($('#span_InvalidQuantity').text())
                    .show();
                $('#loader').hide();
                return false;
            }

            // SAME DOM UPDATES
            $row.find("#SerialFrom").text(serialFrom);
            $row.find("#SerialTo").text(serialTo);
            $row.find("#PhyPerPack").text(total_Packs);

            itemSet[ItemId] = true;

            currentSerial = serialTo + 1;
            currentSerial_1++;
        });

        // 🔥 SAME FUNCTIONS — BUT ONLY ONCE
        ResetPhyCount();

        for (var itemId in itemSet) {
            PackingNoSetOnItemDetail(itemId);
        }

        $('#loader').hide();

    }, 10);
}

//function onclickAutoSerialization() {
//    if ($("#PackSrlzsnTable tbody tr").length > 0) {
//        debugger;
//        let currentSerial = 1;
//        let currentSerial_1 = 1;

//        $("#PackSrlzsnTable tbody tr").each(function () {
//            var currentRow = $(this);
//            var sr_no = parseInt(currentRow.find("#SrNo").text());
//            var ItemId = currentRow.find("#ItemId").text();
//            var Item_Name = currentRow.find("#ItemName").text();

//            if (sr_no === currentSerial_1) {
//                var totalQty = parseFloat(currentRow.find("#TotalQty").text());
//                var PerPack_Qty = parseFloat(currentRow.find("#QtyPerPack").text());
//                var total_Packs = parseInt(currentRow.find("#TotalPacks").text());

//                var qtyPerPack = totalQty / PerPack_Qty;              
//                var serialFrom = currentSerial;
//                var serialTo = currentSerial + qtyPerPack - 1;

//                if (serialFrom % 1 !== 0) {

//                    swal("", $("#span_InvalidSerialNumber").text(), "warning");
//                    return false;
//                }
//                if (serialTo % 1 !== 0) {

//                    swal("", $("#span_InvalidSerialNumber").text() , "warning");
//                    return false;
//                }
//                if (!Number.isInteger(total_Packs)) {
//                    $("#Pack_QtyPack").css("border-color", "red");
//                    $("#Pack_QtyPackError").text($('#span_InvalidQuantity').text()).css("display", "block");
//                    ErrorFlag = "Y";
//                    return false;
//                }
//                currentRow.find("#SerialFrom").text(serialFrom);
//                currentRow.find("#SerialTo").text(serialTo);
//                currentRow.find("#PhyPerPack").text(total_Packs);

//                ResetPhyCount();
//                PackingNoSetOnItemDetail(ItemId);

//                currentSerial = serialTo + 1;
//                currentSerial_1 += 1;
//            }
//        });
//    }
//}


function AppendPackDetailList(SerialFrom, SerialTo, TotalPack, QtyPack, PhyPack, TotalQty, NetWeight
    , TotalNetWeight, GrossWeight, TotalGrossWeight, NetWeight_perpiece, GrossWeight_piece, QtyPerInner, TotalInnerBox) {
    var Edit = $("#Edit").text();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();
    }
    var Span_Delete_Title = $("#Span_Delete_Title").text();
    var hd_Status = $("#hd_Status").val();
    var Disable = $("#HdnForDisable").val();
    var Amend = $("#Amend").val();
    var deleteIcon = "";
    var Greyscale = "";
    var OnEditClick = "";
    if (Disable == "Enable" && hd_Status != "A") {
        deleteIcon = "deleteIcon";
        OnEditClick = "EditPackingDetails(event)";
    } else {
        if (Amend == "Amend") {
            deleteIcon = "deleteIcon";
            OnEditClick = "EditPackingDetails(event)";
        }
        else {
            Greyscale = 'style="filter: grayscale(100%)"';
        }
    }
    var arr = [];
    $("#Tbl_PackingDetail tbody tr").each(function () {
        arr.push($(this).find("#td_SrNo").text());
    });
    var SrNo
    if (arr.length > 0) {
        SrNo = Math.max(...arr) + 1;
    }
    else {
        SrNo = 1;
    }

    //var SrNo = $("#Tbl_PackingDetail tbody tr").length + 1;

    $("#Tbl_PackingDetail tbody").append(`
                        <tr>
                            <td id="td_SrNo" hidden>${SrNo}</td>
                           
                            <td id="td_SerialFrom" class="num_right">${SerialFrom}</td>
                            <td id="td_SerialTo" class="num_right">${SerialTo}</td>
                            <td id="td_TotalPack" class="num_right">${TotalPack}</td>                       
                            <td id="td_QtyPerPack" class="num_right">${parseFloat(QtyPack).toFixed(QtyDecDigit)}</td>
                            <td id="td_QtyPerinner" class="num_right">${parseFloat(QtyPerInner).toFixed(QtyDecDigit)}</td>
                            <td id="td_TotalInnerBox" class="num_right">${parseFloat(TotalInnerBox).toFixed(QtyDecDigit)}</td>
                            <td id="td_PhyPerPack" class="num_right">${PhyPack}</td>
                            <td id="td_TotalQty" class="num_right">${parseFloat(TotalQty).toFixed(QtyDecDigit)}</td>
                            <td id="td_NetWeight_perpiece" class="num_right">${parseFloat(NetWeight_perpiece).toFixed(QtyDecDigit)}</td>
                            <td id="td_NetWeight" class="num_right">${parseFloat(NetWeight).toFixed(QtyDecDigit)}</td>
                            <td id="td_TotalNetWeight" class="num_right">${parseFloat(TotalNetWeight).toFixed(QtyDecDigit)}</td>
                            <td id="td_GrossWeight_perpiece" class="num_right">${parseFloat(GrossWeight_piece).toFixed(QtyDecDigit)}</td>
                            <td id="td_GrossWeight" class="num_right">${parseFloat(GrossWeight).toFixed(QtyDecDigit)}</td>
                            <td id="td_TotalGrossWeight" class="num_right">${parseFloat(TotalGrossWeight).toFixed(QtyDecDigit)}</td>
                        </tr>
`)

    $('#Tbl_PackingDetail').on('click', '.deleteIcon', function () {
        var SrCode = $(this).closest('tr').find("#td_SrNo").text();
        var Hdn_PackSrNo = $("#Hdn_PackSrNo").val();
        $(this).closest('tr').remove();
        if (SrCode == Hdn_PackSrNo) {
            ResetPackDetailFields();
        }
        CalCulateTotalPackingDetail();
        if ($("#PackSrlzsnTable tbody tr").length > 0) {
            $("#AutoSerialization").attr("disabled", false);
        }
        else {
            $("#AutoSerialization").attr("disabled", true);
        }
    });
}
function EditPackingDetails(e) {
    debugger;
    var Row = $(e.target).closest("tr");
    var td_SrNo = Row.find("#SrNo").text();
    var td_item_id = Row.find("#ItemId").text();
    var td_item_name = Row.find("#ItemName").text();
    var SerialFrom = Row.find("#SerialFrom").text();
    var SerialTo = Row.find("#SerialTo").text();
    var QtyPerInner = Row.find("#QtyPerInner").text();
    var QtyPerPack = Row.find("#QtyPerPack").text();
    var NetWeight_perpiece = Row.find("#Netweight_perpiece").text();  /*Added By Nitesh 24-11-2023 for Net_weight Per Piece*/
    var NetWeight = Row.find("#NetWeight").text();
    var GrossWeight_perpiece = Row.find("#GrossWeight_perpiece").text();  /*Added By Nitesh 24-11-2023 for Gross_weight Per Piece*/
    var GrossWeight = Row.find("#GrossWeight").text();
    var Total_Qty = Row.find("#TotalQty").text();
    var TotalInnerBox = Row.find("#tblTotalInnerBox").text();

    $("#Hdn_PackSrNo").val(td_SrNo);
    $("#Pack_SerialFrom").val(SerialFrom);
    $("#Pack_SerialTo").val(SerialTo);
    $("#Qty_PerInner").val(QtyPerInner);
    $("#Pack_QtyPack").val(QtyPerPack);
    $("#Pack_NetWeight").val(NetWeight);
    $("#Pack_GrossWeight").val(GrossWeight);
    $("#netweightperpiece").val(NetWeight_perpiece);
    $("#grossweightperpiece").val(GrossWeight_perpiece);
    $("#TotalInnerBox").val(TotalInnerBox);
   
    $("#Pack_ItemName").val(td_item_id).trigger('change');
    $("#Pack_Quantity").val(Total_Qty);
    $("#Pack_ItemName").attr("disabled",true);

    var updatehtml = `<i class="fa fa-refresh ref" id="ParaItemUpdateBtn" 
onclick="OnClickUpdatePackDetail()" title="${$("#span_Update").text()}"></i>`;
    $("#Icon_AddNewPackDetails").html(updatehtml);
    validatePackingDetail();

}
function OnClickUpdatePackDetail() {
    debugger;
    var ErrorFlag = "N";
    var SerialFrom = $("#Pack_SerialFrom").val()
    var SerialTo = $("#Pack_SerialTo").val()
    if (SerialFrom == "0" || SerialFrom == "" || SerialTo == "0" || SerialTo == "") {
        SerialFrom = "0";
        SerialTo = "0";
    }
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }

    if (!validatePackingDetail() || !CheckQtyPerPack_quantity()) return false;

    var toatlpacked_qty = $("#hdnPacked_Quantity").val();
    var itemTotalPack = $("#Pack_Quantity").val();
    var HdnSrNo = $("#Hdn_PackSrNo").val();
    var ItemId = $("#Pack_ItemName").val();
    let PackQty = $("#Pack_Quantity").val();

    let FinTotalQty = 0;
    let MasterFintotalPacked = 0;

    // Calculate total already packed
    $("#PackSrlzsnTable tbody tr").each(function () {
        let rowItemId = $(this).find("#ItemId").text().trim();
        let TotalQty = $(this).find("#TotalQty").text();
        let srNo = $(this).find("#SrNo").text();

        if (rowItemId === ItemId) {
            if (srNo === HdnSrNo) {
                // Skip this row if it's being edited to avoid double-counting
                return true; // continue
            }
            else {
                FinTotalQty += parseFloat(CheckNullNumber(TotalQty));
            }
            
        }
    });

    // Get the total allowed quantity
    $("#PDItmDetailsTbl tbody tr").each(function () {
        if ($(this).find("#hdItemId").val().trim() === ItemId) {
            let PackedQuantity = $(this).find("#PackedQuantity").val();
            MasterFintotalPacked += parseFloat(CheckNullNumber(PackedQuantity));
        }
    });

    // Validation: (Already packed + new pack) should NOT exceed allowed quantity
    if ((FinTotalQty + parseFloat(CheckNullNumber(PackQty))) > MasterFintotalPacked) {
        $("#Pack_Quantity").css("border-color", "red");
        $("#Pack_QuantityError").text($('#ExceedingQty').text()).css("display", "block");
        ErrorFlag = "Y";
        return false;
    } else {
        $("#Pack_Quantity").css("border-color", "#ced4da");
        $("#Pack_QuantityError").text("").css("display", "none");
    }


    var TotalPack = "0";
    var QtyPack = "0";
    var TotalQty = "0";
  
    QtyPack = $("#Pack_QtyPack").val();
    if (SerialFrom !== "0" && SerialFrom !== "" && SerialTo !== "0"  && SerialTo !== "") {
        TotalPack = (parseInt(SerialTo) - parseInt(SerialFrom) + 1)

        TotalQty1 = $("#Pack_Quantity").val();


        TotalQty2 = TotalPack * QtyPack;
        if (TotalQty1 == TotalQty2) {
            TotalQty = TotalQty2;
        }
        else {
            SerialFrom = "0";
            SerialTo = "0";
            TotalQty = TotalQty1;
        }
    }
    else {
         TotalQty = $("#Pack_Quantity").val();
       
      
        TotalPack = TotalQty / QtyPack;
        if (TotalPack % 1 !== 0) {
            $("#Pack_QtyPack").css("border-color", "red");
            $("#Pack_QtyPackError").text($('#span_InvalidQuantity').text()).css("display", "block");
            ErrorFlag = "Y";
            return false;
        } else {
            $("#Pack_QtyPack").css("border-color", "#ced4da");
            $("#Pack_QtyPackError").text("").css("display", "none");
           
        }
    }
    if (TotalQty % QtyPack !== 0) {
        $("#Pack_QtyPack").css("border-color", "red");
        $("#Pack_QtyPackError").text($('#span_InvalidQuantity').text()).css("display", "block");
        ErrorFlag = "Y";
        return false;
    } else {
        $("#Pack_QtyPack").css("border-color", "#ced4da");
        $("#Pack_QtyPackError").text("").css("display", "none");
       
    }
    var Qty_PerInner = parseFloat($("#Qty_PerInner").val()) || 0;

    if (parseFloat(QtyPack) != parseFloat(0)) {
        if (parseFloat(CheckNullNumber(Qty_PerInner)) > parseFloat(CheckNullNumber(QtyPack))) {
            $("#QtyPerInnerError").text($("#span_InvalidQuantity").text());
            $("#Qty_PerInner").css("border-color", "red");
            $("#QtyPerInnerError").css("display", "block");
            ErrorFlag = "Y";
            return false;
        }
        else {
            $("#QtyPerInnerError").text("");
            $("#QtyPerInnerError").css("display", "none");
            $("#Qty_PerInner").css("border-color", "#ced4da");
            //   var TotalInnerBox = parseFloat(qtyperpack) / parseFloat(qtyperinner)
            //  $("#Qty_PerInner").val(parseFloat(qtyperinner).toFixed(QtyDecDigit))
        }
    }

    var TotalInner_Box = parseFloat(CheckNullNumber($("#TotalInnerBox").val())).toFixed(QtyDecDigit);
    var NetWeight = $("#Pack_NetWeight").val();
    var GrossWeight = $("#Pack_GrossWeight").val();
    var NetWeight_perpiece = $("#netweightperpiece").val();
    var GrossWeight_piece = $("#grossweightperpiece").val();
    var TotalNetWeight = parseFloat(NetWeight) * parseFloat(TotalPack);
    var TotalGrossWeight = parseFloat(GrossWeight) * parseFloat(TotalPack);

    var PhyPack = 0;
    if (ErrorFlag === "N") {
        if (SerialFrom !== "0" && SerialTo !== "0") {
            var MainArr = [];
            for (let i = parseInt(SerialFrom); i <= parseInt(SerialTo); i++) MainArr.push(i);

            if ($("#PackSrlzsnTable tbody tr").length > 0) {
                $("#PackSrlzsnTable tbody tr").each(function () {
                    var row = $(this);
                    if (row.find("#SrNo").text() != HdnSrNo) {
                        var SerialFrom1 = row.find("#SerialFrom").text();
                        var SerialTo1 = row.find("#SerialTo").text();
                        var existing = [];
                        for (let i = parseInt(SerialFrom1); i <= parseInt(SerialTo1); i++) existing.push(i);
                        MainArr = MainArr.filter(n => !existing.includes(n));
                    }
                });
            }

             PhyPack = MainArr.length || (parseInt(SerialTo) - parseInt(SerialFrom) + 1);
        }
        else {
            PhyPack = parseFloat(TotalPack);
        }

        var currentRow = $("#PackSrlzsnTable tbody tr #SrNo:contains(" + HdnSrNo + ")").closest("tr");
        currentRow.each(function () {
            var row = $(this);
            var sr_noTD = row.find("#SrNo").text()
            if (sr_noTD == HdnSrNo) {
                row.find("#SerialFrom").text(SerialFrom);
                row.find("#SerialTo").text(SerialTo);
                row.find("#TotalPacks").text(TotalPack);
              
                row.find("#QtyPerPack").text(parseFloat(QtyPack).toFixed(QtyDecDigit));
                row.find("#QtyPerInner").text(parseFloat(Qty_PerInner).toFixed(QtyDecDigit));
                row.find("#tblTotalInnerBox").text(parseFloat(TotalInner_Box).toFixed(QtyDecDigit));
                row.find("#PhyPerPack").text(PhyPack);
                row.find("#TotalQty").text(parseFloat(TotalQty).toFixed(QtyDecDigit));
                row.find("#NetWeight_perpiece").text(parseFloat(NetWeight_perpiece).toFixed(WeightDecDigit));
                row.find("#NetWeight").text(parseFloat(NetWeight).toFixed(WeightDecDigit));
                row.find("#TotalNetWeight").text(parseFloat(TotalNetWeight).toFixed(WeightDecDigit));
                row.find("#GrossWeight_perpiece").text(parseFloat(GrossWeight_piece).toFixed(WeightDecDigit));
                row.find("#GrossWeight").text(parseFloat(GrossWeight).toFixed(WeightDecDigit));
                row.find("#TotalGrossWeight").text(parseFloat(TotalGrossWeight).toFixed(WeightDecDigit));
            }
          
        });

        ResetPhyCount();

        $("#Pack_SerialFrom, #Pack_SerialTo, #Qty_PerInner, #Pack_QtyPack, #Pack_NetWeight, #Pack_GrossWeight, #netweightperpiece, #grossweightperpiece, #Hdn_PackSrNo, #TotalInnerBox").val("");

        $("#Icon_AddNewPackDetails").html(`<i class="fa fa-plus" onclick="OnClickAddPackDetail();" title="${$("#span_AddNew").text()}"></i>`);

        CalCulateTotalPackingDetailsForAllItems();
        PackingNoSetOnItemDetail(ItemId);

        let Pack_Quantity = $("#Pack_Quantity").val();
        if ((parseFloat(CheckNullNumber(Pack_Quantity)) - parseFloat(TotalQty)) == 0) {
            $("#Pack_ItemName").val(0).trigger("change");
            $("#Pack_ItemName").attr("disabled", false);
        } else {
            $("#Pack_ItemName").attr("disabled", false);
            $("#Pack_ItemName").trigger("change");
        }
    }
}

function CalCulateTotalPackingDetail() {
    var TotalPacks = 0;
    var TotalQty = 0;
    var TotalNetWeight = 0;
    var TotalGrossWeight = 0;
    var TotalPhysicalPacks = 0;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();///Quantity
    }
    $("#Tbl_PackingDetail tbody tr").each(function () {
        var Row = $(this);
        TotalPacks = TotalPacks + parseFloat(Row.find("#td_TotalPack").text());
        TotalQty = TotalQty + parseFloat(Row.find("#td_TotalQty").text());
        TotalPhysicalPacks = TotalPhysicalPacks + parseFloat(Row.find("#td_PhyPerPack").text());
        TotalNetWeight = TotalNetWeight + parseFloat(Row.find("#td_TotalNetWeight").text());
        TotalGrossWeight = TotalGrossWeight + parseFloat(Row.find("#td_TotalGrossWeight").text());
    });
    $("#tf_TotalPacks").text(TotalPacks);
    $("#tf_TotalQty").text(parseFloat(TotalQty).toFixed(QtyDecDigit));
    $("#tf_PhyPacks").text(parseFloat(TotalPhysicalPacks).toFixed(QtyDecDigit));
    $("#tf_TotalNetWeight").text(parseFloat(TotalNetWeight).toFixed(QtyDecDigit));
    $("#tf_TotalGrossWeight").text(parseFloat(TotalGrossWeight).toFixed(QtyDecDigit));
}
function CalCulateTotalPackingDetailsForAllItems() {
    debugger;
    debugger;
    var TotalPacks = 0;
    var TotalQty = 0;
    var TotalNetWeight = 0;
    var TotalGrossWeight = 0;
    var TotalPhysicalPacks = 0;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();///Quantity
    }
    $("#PackSrlzsnTable tbody tr").each(function () {
        var Row = $(this);
        TotalPacks = TotalPacks + parseFloat(Row.find("#TotalPacks").text());
        TotalQty = TotalQty + parseFloat(Row.find("#TotalQty").text());
        TotalPhysicalPacks = TotalPhysicalPacks + parseFloat(Row.find("#PhyPerPack").text());
        TotalNetWeight = TotalNetWeight + parseFloat(Row.find("#TotalNetWeight").text());
        TotalGrossWeight = TotalGrossWeight + parseFloat(Row.find("#TotalGrossWeight").text());
    });
    $("#PackSRLZSN_TotalPacks").text(TotalPacks);
    //$("#tf_TotalQty").text(parseFloat(TotalQty).toFixed(QtyDecDigit));
    $("#PackSRLZSN_TotalPhyscialQty").text(parseFloat(TotalPhysicalPacks).toFixed(QtyDecDigit));
  
    $("#totalPacked_qty").text(parseFloat(TotalQty).toFixed(QtyDecDigit));
 
    $("#PackSRLZSN_TotalNetWeight").text(parseFloat(TotalNetWeight).toFixed(WeightDecDigit));
    $("#PackSRLZSN_TotalGrossWeight").text(parseFloat(TotalGrossWeight).toFixed(WeightDecDigit));
}
function validatePackingDetail() {
    var ErrorFlag = "N";
    var TotalPack = 0;
    var SerialFrom = $("#Pack_SerialFrom").val();
    var SerialTo = $("#Pack_SerialTo").val();
    var QtyPack = $("#Pack_QtyPack").val();
    var PackQuantity = $("#Pack_Quantity").val();
    if (SerialFrom == "0" || SerialFrom == "" || SerialTo == "0" || SerialTo == "") {
        TotalPack = PackQuantity / QtyPack;
    }
    else {
      
        TotalPack = (parseInt(SerialTo) - parseInt(SerialFrom)) + 1;
    }
   
    
    var TotalQty = parseFloat(CheckNullNumber(TotalPack)) * parseFloat(CheckNullNumber(QtyPack));
    var NetWeight = $("#Pack_NetWeight").val();
    var TotalNetWeight = parseFloat(CheckNullNumber(NetWeight)) * parseFloat(CheckNullNumber(TotalPack));//TotalQty;
    var GrossWeight = $("#Pack_GrossWeight").val();
    var TotalGrossWeight = parseFloat(CheckNullNumber(GrossWeight)) * parseFloat(CheckNullNumber(TotalPack));//TotalQty;

    //var arrSrTo = [];
    //var arrSrFrom = [];
    //$("#Tbl_PackingDetail tbody tr").each(function () {
    //    var SrTo = $(this).find("#td_SerialTo").text();
    //    var SrFrom = $(this).find("#td_SerialFrom").text();
    //    arrSrTo.push(SrTo);
    //    arrSrFrom.push(SrFrom);
    //});

    //var MaxSrto = Math.max(arrSrTo)
    //var MinSrFrom = arrSrFrom.min();

    /*Commented by NItesh 30062025*/
    //if (CheckVallidation("Pack_SerialFrom", "Pack_SerialFromError") == false) {
    //    ErrorFlag = "Y";
    //}
    //if (CheckVallidation("Pack_SerialTo", "Pack_SerialToError") == false) {
    //    ErrorFlag = "Y";
    //}
    /*Commented End*/
    if (CheckVallidation("Pack_QtyPack", "Pack_QtyPackError") == false) {
        ErrorFlag = "Y";
    }
    if (CheckVallidation("Pack_NetWeight", "Pack_NetWeightError") == false) {
        ErrorFlag = "Y";
    }
    //if (CheckVallidation("Pack_GrossWeight", "Pack_GrossWeightError") == false) {
    //    ErrorFlag = "Y"; //commented by shubham maurya on 05-03-2025 for ticketId:1427
    //}
    if (CheckVallidation("netweightperpiece", "piece_NetWeightError") == false) {
        ErrorFlag = "Y";
    }
    //if (CheckVallidation("grossweightperpiece", "piece_GrossWeightError") == false) {
    //    ErrorFlag = "Y"; //commented by shubham maurya on 05-03-2025 for ticketId:1427
    //}
    if (ErrorFlag == "N") {
        if (GrossWeight != null && parseFloat(GrossWeight) != 0) {
            if (TotalGrossWeight < TotalNetWeight) { //commented by shubham maurya on 05-03-2025 for ticketId:1427
            $("#Pack_NetWeight").css("border-color", "red")
            $("#Pack_NetWeightError").text($("#PackingList_NetWeightExceedingGrossWeight").text());
            $("#Pack_NetWeightError").css("display", "block");
            $("#Pack_NetWeightError").css("z-index", "1000");
            ErrorFlag = "Y";
        }
        }
        //if (TotalGrossWeight < TotalNetWeight) { //commented by shubham maurya on 05-03-2025 for ticketId:1427
        //    $("#Pack_NetWeight").css("border-color", "red")
        //    $("#Pack_NetWeightError").text($("#PackingList_NetWeightExceedingGrossWeight").text());
        //    $("#Pack_NetWeightError").css("display", "block");
        //    $("#Pack_NetWeightError").css("z-index", "1000");
        //    ErrorFlag = "Y";
        //}
    }

    if (ErrorFlag == "Y") {
        return false;
    } else {
        if (parseInt(SerialFrom) > parseInt(SerialTo)) {
            ErrorFlag = "Y";
            $("#Pack_SerialFrom").css("border-color", "red")
            $("#Pack_SerialFromError").text($('#InvalidSerialFrom').text());
            $("#Pack_SerialFromError").css("display", "block");
            $("#Pack_SerialTo").css("border-color", "red")
            $("#Pack_SerialToError").text($('#InvalidSerialTo').text());
            $("#Pack_SerialToError").css("display", "block");
        } else {
            $("#Pack_SerialFrom").css("border-color", "#ced4da")
            $("#Pack_SerialFromError").text("");
            $("#Pack_SerialFromError").css("dispaly", "none");
            $("#Pack_SerialTo").css("border-color", "#ced4da")
            $("#Pack_SerialToError").text("");
            $("#Pack_SerialToError").css("dispaly", "none");
        }


        if (ErrorFlag == "Y") {
            return false;
        } else {
            return true;
        }
    }



}
function OnchangePackSerialFrom() {
    
    var SerialFrom = $("#Pack_SerialFrom").val();
    var SerialTo = $("#Pack_SerialTo").val();
    var ItemId = $("#Pack_ItemName").val();
    var ErrorFlag = "N";
    //if (SerialTo == "") {
    //    ErrorFlag = "Y";
    //}
    if (SerialFrom == "") {
        ErrorFlag = "Y";
    }
    //SerialTo = parseInt(SerialTo);
    SerialFrom = parseInt(SerialFrom);
    var ArrSerials = [];
    if (ErrorFlag == "N") {
        var Hdn_PackSrNo = $("#Hdn_PackSrNo").val();
        $("#PackSrlzsnTable tbody tr #ItemId:contains('" + ItemId + "')").closest('tr').each(function () {
            var SrTo = $(this).find("#SerialTo").text();
            var SrFrom = $(this).find("#SerialFrom").text();
            var td_SrNo = $(this).find("#SrNo").text();

            if (Hdn_PackSrNo == td_SrNo) {

            } else {
                if (parseInt(SrFrom) <= SerialFrom && SerialFrom <= parseInt(SrTo)) {
                    ErrorFlag = "Y";
                    return false;
                }
                ArrSerials.push(SrFrom + "-" + SrTo);
            }

        });

    }
    if (ErrorFlag == "Y") {
        $("#Pack_SerialFrom").css("border-color", "red")
        $("#Pack_SerialFromError").text($('#InvalidSerialFrom').text());
        $("#Pack_SerialFromError").css("display", "block");
        return false;
    }
    else {
        $("#Pack_SerialFrom").css("border-color", "#ced4da")
        $("#Pack_SerialFromError").text("");
        $("#Pack_SerialFromError").css("dispaly", "none");
    }

    //ArrSerials.sort();
    ArrSerials.sort(function (a, b) { return a.split('-')[0] - b.split('-')[0] });
    if (ArrSerials.length > 0) {
        if (Math.abs(SerialFrom - parseInt(ArrSerials[0].split("-")[0])) == 1 && SerialFrom < parseInt(ArrSerials[0].split("-")[0])) {
            $("#Pack_SerialTo").val(SerialFrom);
        }
    }

    for (var i = 0; i < ArrSerials.length - 1; i++) {
        if (SerialFrom - parseInt(ArrSerials[i].split("-")[1]) == 1 && parseInt(ArrSerials[i + 1].split("-")[0]) - SerialFrom == 1) {
            $("#Pack_SerialTo").val(SerialFrom);
        }
        if (parseInt(ArrSerials[i].split("-")[0]) < SerialFrom && SerialFrom < parseInt(ArrSerials[i + 1].split("-")[0])) {
            if (/*SerialFrom - parseInt(ArrSerials[i].split("-")[1]) == 1 &&*/ (parseInt(ArrSerials[i + 1].split("-")[0]) - SerialFrom) == 1) {
                $("#Pack_SerialTo").val(SerialFrom);
            }
        }
    }
    if (SerialTo != "" && SerialTo != null) {
        OnchangePackSerialTo();
    }
}
function OnchangePackSerialTo() {
    
    var SerialFrom = $("#Pack_SerialFrom").val();
    var SerialTo = $("#Pack_SerialTo").val();
    var ItemId = $("#Pack_ItemName").val();
    var ErrorFlag = "N";
    if (SerialTo == "") {
        ErrorFlag = "Y";
    }
    if (SerialFrom == "") {
        ErrorFlag = "Y";
    }
    SerialTo = parseInt(SerialTo);
    SerialFrom = parseInt(SerialFrom);
    if (SerialFrom > SerialTo) {
        ErrorFlag = "Y";
    }
    var ArrSerials = [];
    if (ErrorFlag == "N") {
        var Hdn_PackSrNo = $("#Hdn_PackSrNo").val();
        $("#PackSrlzsnTable tbody tr #ItemId:contains('" + ItemId + "')").closest('tr').each(function () {
            var SrTo = $(this).find("#SerialTo").text();
            var SrFrom = $(this).find("#SerialFrom").text();
            var td_SrNo = $(this).find("#SrNo").text();

            if (Hdn_PackSrNo == td_SrNo) {

            } else {
                if (parseInt(SrFrom) <= parseInt(SerialTo) && parseInt(SerialTo) <= parseInt(SrTo)) {
                    ErrorFlag = "Y";
                    return false;
                }
                ArrSerials.push(SrFrom + "-" + SrTo);
            }


        });
    }


    if (ErrorFlag == "N") {
        ArrSerials.sort();
        if (ArrSerials.length > 0) {
            if (parseInt(SerialFrom) < parseInt(ArrSerials[0].split("-")[0])) {
                if (SerialTo >= parseInt(ArrSerials[0].split("-")[0])) {
                    ErrorFlag = "Y";
                }
            } else if (SerialFrom > parseInt(ArrSerials[ArrSerials.length - 1].split("-")[0])) {
                if (ArrSerials[ArrSerials.length - 1].split("-")[1] < SerialTo) {

                } else {
                    ErrorFlag = "Y";
                }
            }
        }

    }


    if (ErrorFlag == "N") {
        for (var i = 0; i < ArrSerials.length - 1; i++) {
            if (parseInt(ArrSerials[i].split("-")[0]) < SerialFrom && SerialFrom < parseInt(ArrSerials[i + 1].split("-")[0])) {
                if (parseInt(ArrSerials[i].split("-")[1]) < SerialTo && SerialTo < parseInt(ArrSerials[i + 1].split("-")[0])) {
                } else {
                    ErrorFlag = "Y"
                }
            }
        }
    }


    if (ErrorFlag == "Y") {
        $("#Pack_SerialTo").css("border-color", "red")
        $("#Pack_SerialToError").text($('#InvalidSerialTo').text());
        $("#Pack_SerialToError").css("display", "block");
        return false;
    }
    else {
        $("#Pack_SerialTo").css("border-color", "#ced4da")
        $("#Pack_SerialToError").text("");
        $("#Pack_SerialToError").css("dispaly", "none");
    }
}
function OnchangeSrQtyPerPack() {
    debugger;
    var Pack_QtyPack = $("#Pack_QtyPack").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    if (CheckVallidation("Pack_QtyPack", "Pack_QtyPackError") == false) {
        $("#TotalInnerBox").val("")
        return false;
    };  
    if (CheckQtyPerPack_quantity() == false)
    {
        $("#TotalInnerBox").val("")
        return false;
    };

    var qtyperinner = CheckNullNumber($("#Qty_PerInner").val());
    if (parseFloat(Pack_QtyPack) != parseFloat(0)) {
        if (parseFloat(CheckNullNumber(qtyperinner)) > parseFloat(CheckNullNumber(Pack_QtyPack))) {
            $("#QtyPerInnerError").text($("#span_InvalidQuantity").text());
            $("#Qty_PerInner").css("border-color", "red");
            $("#QtyPerInnerError").css("display", "block");
            $("#TotalInnerBox").val("")
            return false;
        }
        else {
            $("#QtyPerInnerError").text("");
            $("#QtyPerInnerError").css("display", "none");
            $("#Qty_PerInner").css("border-color", "#ced4da");
            if (parseFloat(qtyperinner) != parseFloat(0)) {
                var TotalInnerBox = parseFloat(CheckNullNumber(Pack_QtyPack)) / parseFloat(CheckNullNumber(qtyperinner))
                $("#TotalInnerBox").val(parseInt(CheckNullNumber(TotalInnerBox)))
            }
            else {
                $("#TotalInnerBox").val("")
            }

        }
    }
    else {
        $("#TotalInnerBox").val("")
    }
  
    var hd_Pack_ItemId = $("#Pack_ItemName").val();
    var netweightperpiece = $("#netweightperpiece").val();
    $("#PDItmDetailsTbl tbody tr td #hdItemId[value=" + hd_Pack_ItemId + "]").closest("tr").each(function () {
        debugger
        var row = $(this);
     
        var netwght = row.find("#hditemnetwgt").val();
        var grwght = row.find("#hditemgrosswgt").val();
        if (parseFloat(CheckNullNumber(netweightperpiece)) > 0) {

        }
        else {
            //$("#Pack_NetWeight").val(parseFloat(CheckNullNumber(netwght)).toFixed(ValDecDigit));
            //$("#Pack_GrossWeight").val(parseFloat(CheckNullNumber(grwght)).toFixed(ValDecDigit));
            $("#netweightperpiece").val(parseFloat(CheckNullNumber(netwght)).toFixed(WeightDecDigit));
            $("#grossweightperpiece").val(parseFloat(CheckNullNumber(grwght)).toFixed(WeightDecDigit));
        }
        calculateperpack();
    });

    $("#Pack_QtyPack").val(parseFloat(CheckNullNumber(Pack_QtyPack)).toFixed(QtyDecDigit));


  
    
   

}

function calculateperpack() {
    debugger;
    var Pack_QtyPack = parseFloat(CheckNullNumber($("#Pack_QtyPack").val()));
    var Grossweightperpiece = $("#grossweightperpiece").val();
    var netweightperpiece = $("#netweightperpiece").val();
    var totalnetweigthpack = parseFloat(Pack_QtyPack) * parseFloat(CheckNullNumber(netweightperpiece));
    $("#Pack_NetWeight").val(parseFloat(CheckNullNumber(totalnetweigthpack)).toFixed(WeightDecDigit));
    OnchangeSrNetWeightperpiece();
    OnchangeSrGrossWeight();
    /*Commented By Nitesh 12-12-2023 10:56*/
    // var totalGrossweigthpack = parseFloat(Pack_QtyPack) * parseFloat(Grossweightperpiece);
    //  $("#Pack_GrossWeight").val(parseFloat(CheckNullNumber(totalGrossweigthpack)).toFixed(WeightDecDigit)); 
    // OnchangeSrGrossWeightperpiece();
}
function OnchangeSrNetWeightperpiece() {
    CheckVallidation("netweightperpiece", "piece_NetWeightError");
    var Pack_NetWeightperpiece = $("#netweightperpiece").val();
    $("#netweightperpiece").val(parseFloat(CheckNullNumber(Pack_NetWeightperpiece)).toFixed(WeightDecDigit))
    var Pack_QtyPack = parseFloat(CheckNullNumber($("#Pack_QtyPack").val()));
    var netweightperpiece = parseFloat(CheckNullNumber($("#netweightperpiece").val()));
    var totalnetweigthpack = parseFloat(Pack_QtyPack) * parseFloat(netweightperpiece);
    $("#Pack_NetWeight").val(parseFloat(CheckNullNumber(totalnetweigthpack)).toFixed(WeightDecDigit));
    OnchangeSrNetWeight();

}
function OnchangeSrNetWeight() {
    CheckVallidation("Pack_NetWeight", "Pack_NetWeightError");
  
    var Pack_NetWeight = $("#Pack_NetWeight").val();
    $("#Pack_NetWeight").val(parseFloat(CheckNullNumber(Pack_NetWeight)).toFixed(WeightDecDigit))


    var Pack_QtyPack = parseFloat(CheckNullNumber($("#Pack_QtyPack").val()));
    var netweightperPack = parseFloat(CheckNullNumber($("#Pack_NetWeight").val()));
    var totalnetweigthpack = parseFloat(netweightperPack) / parseFloat(Pack_QtyPack);
    $("#netweightperpiece").val(parseFloat(CheckNullNumber(totalnetweigthpack)).toFixed(WeightDecDigit));
    CheckVallidation("netweightperpiece", "piece_NetWeightError");

}
function OnchangeSrGrossWeightperpiece() {
    //CheckVallidation("grossweightperpiece", "piece_GrossWeightError");//commented by shubham maurya on 05-03-2025 for ticketId:1427
    var Pack_GrossWeightperpiece = $("#grossweightperpiece").val();
    $("#grossweightperpiece").val(parseFloat(CheckNullNumber(Pack_GrossWeightperpiece)).toFixed(WeightDecDigit))
    var Grossweightperpiece = $("#grossweightperpiece").val();

    /*Commented By Nitesh 12-12-2023 10:56*/
    // var totalGrossweigthpack = parseFloat(Pack_QtyPack) * parseFloat(Grossweightperpiece);
    // $("#Pack_GrossWeight").val(parseFloat(CheckNullNumber(totalGrossweigthpack)).toFixed(WeightDecDigit));
    //OnchangeSrGrossWeight();
}

function OnchangeSrGrossWeight() {
    debugger;
    //CheckVallidation("Pack_GrossWeight", "Pack_GrossWeightError");//commented by shubham maurya on 05-03-2025 for ticketId:1427
    var Pack_GrossWeight = $("#Pack_GrossWeight").val();
    var Pack_NetWeight = $("#Pack_NetWeight").val();
    $("#Pack_GrossWeight").val(parseFloat(CheckNullNumber(Pack_GrossWeight)).toFixed(WeightDecDigit))

    if (parseFloat(Pack_GrossWeight) > parseFloat(Pack_NetWeight)) {
        $("#Pack_NetWeight").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-Pack_NetWeight-container"]').css("border-color", "#ced4da");
        $("#Pack_NetWeightError").text("");
        $("#Pack_NetWeightError").css("display", "none");
    }

    /*Added By Nitesh 12-12-2023 for per piece Gross Weight*/
    var Pack_GrossWeight_1 = $("#Pack_GrossWeight").val();
    var Pack_QtyPack = $("#Pack_QtyPack").val();
    var perpeiece_Grossweigthpack = (parseFloat(Pack_GrossWeight_1)) / (parseFloat(Pack_QtyPack))
    $("#grossweightperpiece").val(parseFloat(CheckNullNumber(perpeiece_Grossweigthpack)).toFixed(WeightDecDigit));
    OnchangeSrGrossWeightperpiece();
}
function OnClickPackingDetailsBtn(e) {
    debugger;
    //$("#Pack_SerialFrom").val("");
    //$("#Pack_SerialTo").val("");
    //$("#Pack_QtyPack").val("");
    //$("#Pack_NetWeight").val("");
    //$("#Pack_GrossWeight").val("");
    //$("#netweightperpiece").val("");
    //$("#grossweightperpiece").val("");

    //RemoveCheckVallidation("Pack_SerialFrom", "Pack_SerialFromError")
    //RemoveCheckVallidation("Pack_SerialTo", "Pack_SerialToError")
    //RemoveCheckVallidation("Pack_QtyPack", "Pack_QtyPackError")
    //RemoveCheckVallidation("Pack_NetWeight", "Pack_NetWeightError")
    //RemoveCheckVallidation("Pack_GrossWeight", "Pack_GrossWeightError")
    //RemoveCheckVallidation("netweightperpiece", "piece_NetWeightError")
    //RemoveCheckVallidation("grossweightperpiece", "piece_GrossWeightError")

    //$("#Pack_SerialFrom").val("");
    //$("#Pack_SerialTo").val("");
    //$("#Pack_QtyPack").val("");
    //$("#Pack_NetWeight").val("");
    //$("#Pack_GrossWeight").val("");
    //$("#netweightperpiece").val("");
    //$("#grossweightperpiece").val("");
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();
    var ItmName = clickedrow.find("#ItemName").val();
    var UOM = clickedrow.find("#UOM").val();
    //var UOMid = clickedrow.find("#hdUOMId").val();
    var PackQty = clickedrow.find("#PackedQuantity").val();

    $("#box_Pack_ItemName").val(ItmName);
    $("#box_Pack_UOM").val(UOM);
    $("#box_Pack_Quantity").val(parseFloat(PackQty).toFixed(QtyDecDigit));

    $("#Tbl_PackingDetail tbody tr").remove();
    $("#PackSrlzsnTbody tr").each(function () {
        var Row = $(this);
        var ItemId = Row.find("#ItemId").text();
        if (ItmCode == ItemId) {
            var SerialFrom = Row.find("#SerialFrom").text();
            var SerialTo = Row.find("#SerialTo").text();
            var TotalPacks = Row.find("#TotalPacks").text();
            var QtyPerInner = Row.find("#QtyPerInner").text();
            var QtyPerPack = Row.find("#QtyPerPack").text();
            var TotalInnerBox = CheckNullNumber(Row.find("#tblTotalInnerBox").text());
            var PhyPack = Row.find("#PhyPerPack").text();
            var TotalQty = Row.find("#TotalQty").text();
            var NetWeight = Row.find("#NetWeight").text();
            var TotalNetWeight = Row.find("#TotalNetWeight").text();
            var GrossWeight = Row.find("#GrossWeight").text();
            var TotalGrossWeight = Row.find("#TotalGrossWeight").text();
            var NetWeight_perpiece = Row.find("#Netweight_perpiece").text();  /*Added By Nitesh 24-11-2023 for Net_weight Per Piece*/
            var GrossWeight_piece = Row.find("#GrossWeight_perpiece").text();  /*Added By Nitesh 24-11-2023 for Gross_weight Per Piece*/

            AppendPackDetailList(SerialFrom, SerialTo, TotalPacks, QtyPerPack, PhyPack, TotalQty, NetWeight
                , TotalNetWeight, GrossWeight, TotalGrossWeight, NetWeight_perpiece, GrossWeight_piece, QtyPerInner, TotalInnerBox);
        }
    });
    CalCulateTotalPackingDetail();
    var len = $("#PackSrlzsnTable tbody tr").length;
    if (len == 0) {
        $("#Pack_SerialFrom").val("1");
    }
    else {
        $("#PackSrlzsnTable tbody tr").each(function () {
            var Row = $(this);
            var SerialTo = Row.find("#SerialTo").text();
            var SerialTo1 = parseFloat(SerialTo) + 1;
            $("#Pack_SerialFrom").val(SerialTo1);
        });
    }
}
//function SaveAndExitPackDetails() {
//    debugger
//    //$("#MainTblPackingDetail").DataTable().destroy();
//    var ItemName = $("#Pack_ItemName").val();
//    var ItemId = $("#hd_Pack_ItemId").val();
//    var UOM = $("#Pack_UOM").val();
//    var UOMId = $("#hd_Pack_UOMId").val();
//    var PackQty = $("#Pack_Quantity").val();
//    var TotalQty = 0;
//    $("#Tbl_PackingDetail tbody tr").each(function () {
//        var Qty = $(this).find("#td_TotalQty").text();
//        TotalQty = parseFloat(TotalQty) + parseFloat(Qty);
//    });
//    //$("#MainTblPackingDetail tr .dataTables_empty").remove();
//    //$(".dataTables_filter input[type=search]").val("").blur();
//    //var t = $('#datatable-buttons1').DataTable();
//    //var SrNo = t.data().length+1;
//    if (parseFloat(PackQty).toFixed(QtyDecDigit) == parseFloat(TotalQty).toFixed(QtyDecDigit)) {
//        var SrNo = $("#PackSrlzsnTbody tr").length + 1;
//        $("#PackSrlzsnTbody tr #ItemId:contains(" + ItemId + ")").closest("tr").remove();
//        $("#Tbl_PackingDetail tbody tr").each(function () {
//            var Row = $(this);
//            var SerialFrom = Row.find("#td_SerialFrom").text();
//            var SerialTo = Row.find("#td_SerialTo").text();
//            var TotalPacks = Row.find("#td_TotalPack").text();
//            var QtyPerPack = Row.find("#td_QtyPerPack").text();
//            var PhyPerPack = Row.find("#td_PhyPerPack").text();
//            var TotalQty = Row.find("#td_TotalQty").text();
//            var NetWeight = Row.find("#td_NetWeight").text();
//            var TotalNetWeight = Row.find("#td_TotalNetWeight").text();
//            var GrossWeight = Row.find("#td_GrossWeight").text();
//            var TotalGrossWeight = Row.find("#td_TotalGrossWeight").text();
//            var netweight_perpiece = Row.find("#td_NetWeight_perpiece").text();  /*Added By Nitesh 24-11-2023 for Net_weight Per Piece*/
//            var GrossWeight_perPiece = Row.find("#td_GrossWeight_perpiece").text();  /*Added By Nitesh 24-11-2023 for Gross_weight Per Piece*/

//            $("#PackSrlzsnTbody").append(`<tr>
//                        <td id="SrNo">${SrNo}</td>
//                        <td id="ItemName">${ItemName}</td>
//                        <td id="ItemId" hidden>${ItemId}</td>
//                        <td id="UOM">${UOM}</td>
//                        <td id="UOMId" hidden>${UOMId}</td>
//                        <td hidden id="PackQty" class="num_right">${PackQty}</td>
//                        <td id="SerialFrom" class="num_right">${SerialFrom}</td>
//                        <td id="SerialTo" class="num_right">${SerialTo}</td>
//                        <td id="TotalPacks" class="num_right">${TotalPacks}</td>
//                        <td id="QtyPerPack" class="num_right">${QtyPerPack}</td>
//                        <td id="PhyPerPack" class="num_right">${PhyPerPack}</td>
//                        <td id="TotalQty" class="num_right">${TotalQty}</td>
//                        <td hidden id="Netweight_perpiece" class="num_right">${netweight_perpiece}</td> 
//                        <td id="NetWeight" class="num_right">${NetWeight}</td>
//                        <td id="TotalNetWeight" class="num_right">${TotalNetWeight}</td>
//                        <td hidden id="GrossWeight_perpiece" class="num_right">${GrossWeight_perPiece}</td>
//                        <td id="GrossWeight" class="num_right">${GrossWeight}</td>
//                        <td id="TotalGrossWeight" class="num_right">${TotalGrossWeight}</td>
//                        </tr>`
//            );
//            SrNo++;
//        });
//        PackSerializationTable("PackSrlzsnTable", 3);
//        //var i = 1;
//        //$("#PackSrlzsnTbody tr").each(function () {
//        //    $(this).find("#SrNo").text(i++);
//        //});
//        //var AllRows = t.table().body().children;
//        //for (var i = 0; i < AllRows.length; i++) {
//        //    AllRows[i].children[2].hidden = true
//        //    AllRows[i].children[4].hidden = true
//        //}
//        //t.columns([2,4]).visible(false);
//        //t.columns([2, 4]).addClass("Hidden");
//        //var tbltbody = $("#PackSrlzsnTbody tr");
//        //tbltbody.find("td:eq(2),td:eq(4)").css("display", "none");

//        var tf_TotalPacks = $("#tf_TotalPacks").text();

//        var tf_TotalNetWeight = $("#tf_TotalNetWeight").text();
//        var tf_TotalGrossWeight = $("#tf_TotalGrossWeight").text();

//        var CurrentRow = $("#PDItmDetailsTbl tbody tr #hdItemId[value=" + ItemId + "]").closest("tr");
//        CurrentRow.find("#NumberOfPacks").val(parseFloat(tf_TotalPacks).toFixed(QtyDecDigit));
//        CurrentRow.find("#NetWeight").val(parseFloat(tf_TotalNetWeight).toFixed(WeightDecDigit));
//        CurrentRow.find("#GrossWeight").val(parseFloat(tf_TotalGrossWeight).toFixed(WeightDecDigit));
//        CurrentRow.find("#PackingBoxbtn").removeClass("packing");
//        CalCulatePackSerializationTotal();
//        SumGross_Net_CBM();
//        $("#PD_btnOrderQtySaveAndExit").attr("data-dismiss", "modal");
//    }
//    else {
//        swal("", $("#PackedQuantityMismatchWithSerializationQuantity").text(), "warning");
//        $("#PD_btnOrderQtySaveAndExit").attr("data-dismiss", "");
//        return false;
//    }
//    ResetPhyCount();
//    PackSerializationTable("PackSrlzsnTable", 6);
//    PackSerializationTable("PackSrlzsnTable", 7);
//    ResetSerialNumber();
//    var PackingItem = $("#hd_Pack_ItemId").val();
//    var Srno = parseFloat(0).toFixed(QtyDecDigit)
//    $("#PDItmDetailsTbl tbody tr").each(function () {
//        debugger;
//        var currantrow = $(this);
//        var ItemId = currantrow.find("#hdItemId").val();
//        let ItemPhyPck = 0;
//        //added by Suraj on 19-12-2023 for serial item level reset
//        $("#PackSrlzsnTbody tr #ItemId:contains(" + ItemId + ")").closest("tr").each(function () {
//            var rw = $(this);
//            var PhyPerPack = rw.find("#PhyPerPack").text();
//            ItemPhyPck = parseFloat(ItemPhyPck) + parseFloat(CheckNullNumber(PhyPerPack));
//        });

//        // if (PackingItem == ItemId) {
//        var totalPhyPacks = ItemPhyPck;//$("#tf_PhyPacks").text();
//        currantrow.find("#PhysicalPacks").val(totalPhyPacks)
//        if (totalPhyPacks == Srno) {
//            currantrow.find("#PackagingDetail").prop("disabled", true);
//        }
//        else {
//            currantrow.find("#PackagingDetail").prop("disabled", false);
//        }
//        //}
//    });
//}
function AppendPackingListDetail(Arr) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();
    }
    var Edit = $("#Edit").text();
    var Span_Delete_Title = $("#Span_Delete_Title").text();
    var hd_Status = $("#hd_Status").val();
    var Disable = $("#HdnForDisable").val();
    var Amend = $("#Amend").val();
    var deleteIcon = "";
    var Greyscale = "";
    var OnEditClick = "";
    if (Disable == "Enable" && hd_Status != "A") {
        deleteIcon = "deleteIcon";
        OnEditClick = "EditPackingDetails(event)";
    } else {
        if (Amend == "Amend") {
            deleteIcon = "deleteIcon";
            OnEditClick = "EditPackingDetails(event)";
        }
        else {
            Greyscale = 'style="filter: grayscale(100%)"';
        }
    }
   
    $("#PackSrlzsnTbody").append(`<tr>
                        <td class="red center">
                            <i class="fa fa-trash ${deleteIcon}" ${Greyscale} aria-hidden="true" title="${Span_Delete_Title}"></i>
                        </td>
                        <td class="edit_pencil center">
                            <i class="fa fa-edit" onclick="${OnEditClick}" aria-hidden="true" title="${Edit}"></i>
                        </td>
                        <td id="SrNo">${Arr.SrNo}</td>
                        <td id="ItemName">${Arr.ItemName}</td>
                        <td id="ItemId" hidden>${Arr.ItemId}</td>
                        <td id="UOM">${Arr.UOM}</td>
                        <td id="UOMId" hidden>${Arr.UOMId}</td>
                        <td hidden id="PackQty" class="num_right">${parseFloat(Arr.PackQty).toFixed(QtyDecDigit)}</td >
                        <td id="SerialFrom" class="num_right">${Arr.SerialFrom}</td>
                        <td id="SerialTo" class="num_right">${Arr.SerialTo}</td>
                        <td id="TotalPacks" class="num_right">${parseFloat(Arr.TotalPacks)}</td>
                     
                        <td id="QtyPerPack" class="num_right">${parseFloat(Arr.QtyPerPack).toFixed(QtyDecDigit)}</td>
                        <td id="QtyPerInner" class="num_right">${parseFloat(Arr.PerInner)}</td>
                        <td id="tblTotalInnerBox" class="num_right">${parseFloat(Arr.TotalInner_Box)}</td>
                        <td id="PhyPerPack" class="num_right">${parseFloat(Arr.PhyPerPack)}</td>
                        <td id="TotalQty" class="num_right">${parseFloat(Arr.TotalQty).toFixed(QtyDecDigit)}</td>
                        <td hidden id="Netweight_perpiece" class="num_right">${parseFloat(Arr.netweight_perpiece).toFixed(WeightDecDigit)}</td>
                        <td id="NetWeight" class="num_right">${parseFloat(Arr.NetWeight).toFixed(WeightDecDigit)}</td>
                        <td id="TotalNetWeight" class="num_right">${parseFloat(Arr.TotalNetWeight).toFixed(WeightDecDigit)}</td>
                        <td hidden id="GrossWeight_perpiece" class="num_right">${parseFloat(Arr.GrossWeight_perPiece).toFixed(WeightDecDigit)}</td>
                        <td id="GrossWeight" class="num_right">${parseFloat(Arr.GrossWeight).toFixed(WeightDecDigit)}</td>
                        <td id="TotalGrossWeight" class="num_right">${parseFloat(Arr.TotalGrossWeight).toFixed(WeightDecDigit)}</td>
                        </tr>`
    );

    //$('#PackSrlzsnTbody').on('click', '.deleteIcon', function () {
    //    var SrCode = $(this).closest('tr').find("#td_SrNo").text();
    //    var Hdn_PackSrNo = $("#Hdn_PackSrNo").val();
    //    $(this).closest('tr').remove();
    //    if (SrCode == Hdn_PackSrNo) {
    //        ResetPackDetailFields();
    //    }
    //    CalCulateTotalPackingDetail();
    //});

    
}
function ResetSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#PackSrlzsnTable >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        //var ID = currentRow.find("#hdSpanRowId").val();
        //var Newwhid = "wh_id" + SerialNo;
        //var whid = "#wh_id" + ID;
        currentRow.find("#SrNo").text(SerialNo);
        //currentRow.find(whid).attr("id", Newwhid);
        //currentRow.find("#hdSpanRowId").val(SerialNo);
    });
};
function ResetPhyCount() {
    debugger;
    var Array = [];
    let Arr = [];
   
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    if ($("#PackSrlzsnTable tbody tr").length > 0) {
        $("#PackSrlzsnTable tbody tr").each(function () {
            var MainArr = [];
            var CurrantRow = $(this);
            var SerialFrom1 = CurrantRow.find("#SerialFrom").text();
            var SerialTo1 = CurrantRow.find("#SerialTo").text();
            let ItemId = CurrantRow.find("#ItemId").text();
            let NoOfPacks = CurrantRow.find("#TotalPacks").text();
            if (SerialFrom1 == "0" || SerialTo1 == "0") {
                var Pcount1 = NoOfPacks;
                Arr.push({ itemId: ItemId, NoOfPacks: NoOfPacks, PhyQty: Pcount1 });
               
            }
            else {
              
                var Pcount = 0;
                for (var i = parseInt(SerialFrom1); i <= parseInt(SerialTo1); i++) {
                    MainArr.push(i);
                }
                for (var j = 0; j < MainArr.length; j++) {
                    if (Array.includes(MainArr[j]) == false) {
                        Array.push(MainArr[j]);
                        Pcount++;
                    }
                }
                let index = Arr.findIndex(v => v.itemId == ItemId);
                if (index > -1) {
                    Arr[index].PhyQty = Arr[index].PhyQty + Pcount;
                    Arr[index].NoOfPacks = parseFloat(Arr[index].NoOfPacks) + parseFloat(NoOfPacks);
                } else {
                    Arr.push({ itemId: ItemId, NoOfPacks: NoOfPacks, PhyQty: Pcount });
                }


                CurrantRow.find("#PhyPerPack").text(Pcount);
            }
           
        });

        $("#PDItmDetailsTbl tbody tr").each(function () {
            let row = $(this);
            let ItemID = row.find("#hdItemId").val();
            let index = Arr.findIndex(v => v.itemId == ItemID);
            if (index > -1) {
                row.find("#PhysicalPacks").val(parseFloat(Arr[index].PhyQty));
                row.find("#NumberOfPacks").val(parseFloat(Arr[index].NoOfPacks));
            }
        });
    }
}
function ResetPackDetailFields() {
    RemoveCheckVallidation("Pack_SerialFrom", "Pack_SerialFromError")
    RemoveCheckVallidation("Pack_SerialTo", "Pack_SerialToError")
    RemoveCheckVallidation("Pack_QtyPack", "Pack_QtyPackError")
    RemoveCheckVallidation("Pack_NetWeight", "Pack_NetWeightError")
    RemoveCheckVallidation("Pack_GrossWeight", "Pack_GrossWeightError")
    RemoveCheckVallidation("netweightperpiece", "piece_NetWeightError")
    RemoveCheckVallidation("grossweightperpiece", "piece_GrossWeightError")

    $("#Pack_SerialFrom").val("");
    $("#Pack_SerialTo").val("");
    $("#Pack_QtyPack").val("");
    $("#Pack_NetWeight").val("");
    $("#Pack_GrossWeight").val("");
    $("#Hdn_PackSrNo").val("");
    $("#netweightperpiece").val("");
    $("#grossweightperpiece").val("");
    var Addhtml = `<i class="fa fa-plus" onclick="OnClickAddPackDetail();" id="" title="${$("#span_AddNew").text()}"></i>`;
    $("#Icon_AddNewPackDetails").html(Addhtml);
}
function ResetPackDetails() {


    ResetPackDetailFields();
    $("#Tbl_PackingDetail tbody tr").remove();
    CalCulateTotalPackingDetail();


}
function CalCulatePackSerializationTotal() {
    var TotalPacks = 0;
    var TotalQty = 0;
    var TotalNetWeight = 0;
    var TotalGrossWeight = 0;
    var TotalPhyPerPack = 0;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        ValDecDigit = $("#ExpImpValDigit").text();///Quantity
    }
    else {
        ValDecDigit = $("#ValDigit").text();///Quantity
    }
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();///Quantity
    }
    var i = 1;
    $("#PackSrlzsnTbody tr").each(function () {
        var Row = $(this);
        Row.find("#SrNo").text(i++);
        TotalPacks = TotalPacks + parseFloat(Row.find("#TotalPacks").text());
        TotalPhyPerPack = TotalPhyPerPack + parseFloat(Row.find("#PhyPerPack").text());
        TotalNetWeight = TotalNetWeight + parseFloat(Row.find("#TotalNetWeight").text());
        TotalGrossWeight = TotalGrossWeight + parseFloat(Row.find("#TotalGrossWeight").text());
    });
    //var tblData = t.data();
    //var len = tblData.length;
    //for (var i = 0; i < len; i++) {
    //    TotalPacks = TotalPacks + parseFloat(tblData[i][8]);
    //    //TotalQty = TotalQty + parseFloat(Row.find("#td_TotalQty").text());
    //    TotalNetWeight = TotalNetWeight + parseFloat(Row.find("#TotalNetWeight").text());
    //    TotalGrossWeight = TotalGrossWeight + parseFloat(Row.find("#TotalGrossWeight").text());
    //}


    //var tfoot = `<tr class="total">
    //                    <td colspan="6" style="text-align:right;"><strong>${$("#span_Total").text()}</strong></td>
    //                    <td class="num_right"><strong><span id="PackSRLZSN_TotalPacks">${TotalPacks}</span></strong></td>
    //                    <td class="num_right" colspan="4"><strong><span id="PackSRLZSN_TotalNetWeight">${TotalNetWeight}</span></strong></td>
    //                    <td class="num_right" colspan="2"><strong><span id="PackSRLZSN_TotalGrossWeight">${TotalGrossWeight}</span></strong></td>
    //            </tr>`
    //$("#datatable-buttons1 tfoot").html(tfoot);
    $("#PackSRLZSN_TotalPacks").text(TotalPacks);
    //$("#tf_TotalQty").text(TotalQty)
    $("#PackSRLZSN_TotalPhyscialQty").text(parseFloat(TotalPhyPerPack));
    $("#PackSRLZSN_TotalNetWeight").text(parseFloat(TotalNetWeight).toFixed(WeightDecDigit));
    $("#PackSRLZSN_TotalGrossWeight").text(parseFloat(TotalGrossWeight).toFixed(WeightDecDigit));
}
function PackSerializationTable(TableId, Col1, Col2) {
    var table, rows, switching, i, x, y, x1, y1, shouldSwitch;
    if (TableId == "Tbl_PackingDetail") {
        x = 0, y = 0;
    }
    table = document.getElementById(TableId);
    switching = true;
    while (switching) {
        switching = false;
        rows = table.tBodies[0].rows;
        for (i = 0; i < (rows.length - 1); i++) {
            shouldSwitch = false;
            x = rows[i].getElementsByTagName("TD")[Col1].innerText;
            y = rows[i + 1].getElementsByTagName("TD")[Col1].innerText;
            if (TableId == "Tbl_PackingDetail") {
                if (parseInt(x) > parseInt(y)) {
                    shouldSwitch = true;
                    break;
                }
            } else {
                if (parseInt(x) > parseInt(y)) {
                    shouldSwitch = true;
                    break;
                }
            }

        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}
function PackingItemData() {
    let data = new Array();
    data.push({
        id: "0",
        text: "---Select---",
        UOM: ""
    })
    $("#PDItmDetailsTbl tbody tr").each(function () {
        let Row = $(this);
        let ItemName = Row.find("#ItemName").val();
        let ItemId = Row.find("#hdItemId").val();
        let UOM = Row.find("#UOM").val();
        let UomId = Row.find("#hdUOMId").val();
        let PackedQty = Row.find("#PackedQuantity").val();
        data.push({
            id: ItemId,
            text: ItemName,
            UOM: UOM,
            UomId: UomId,
            PackQty: PackedQty
        });

    });
    return data;
}
function ResetPackingItemDdl() {
   
    $("#Pack_ItemName").empty();
    $("#Pack_ItemName").append(`<optgroup class="def-cursor" id="Textddl1" label="${$("#ItemName").text()}" data-uom='${$("#ItemUOM").text()}'>
        <option value="0">---Select---</option>
        </optgroup>`)
    $("#Pack_ItemName").select2({
        data: PackingItemData(),
        templateResult: function (data) {
            debugger;
            if(data.PackQty && data.PackQty == CheckSerializedQty(data.id)){
                return false;
            }
            var UOM = $(data.element).data('uom');
            UOM = UOM == null ? data.UOM : UOM;
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + UOM + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },

    });

}
function onChangePackItem() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();
    }
    let ItemProps = $('#Pack_ItemName option:selected').data().data;
    //let ItemName = ItemProps.text;
    let ItemId = ItemProps.id;
    let UomName = ItemProps.UOM;
    //let UomId = ItemProps.UomId;
    let PackedQty = 0;
    if (ItemId != "0") {
        PackedQty = parseFloat(CheckNullNumber(ItemProps.PackQty)) - parseFloat(CheckSerializedQty(ItemId));
    }
    let FinTotalQty = 0;
    $("#PDItmDetailsTbl tbody tr #hdItemId:contains('" + ItemId + "')").closest('tr').each(function () {
        let row = $(this);
        let PackedQuantity = row.find("#PackedQuantity").text();
        $("#hdnPacked_Quantity").val((PackedQuantity).toFixed(QtyDecDigit));
    })
  
    $("#Pack_UOM").val(UomName);
    if ($("#Hdn_PackSrNo").val() == "") {
        $("#Pack_Quantity").val((PackedQty).toFixed(QtyDecDigit));
        $("#hdnPack_QuantitySerialization").val((PackedQty).toFixed(QtyDecDigit));
       
        let srFrom = CheckAvailableSerialNo(ItemId);
        if (PackedQty == 0) {
            $("#Pack_SerialFrom").val("");
        } else {
           $("#Pack_SerialFrom").val(srFrom);
            //$("#Pack_SerialFrom").val(0);
        }        
    }
    else {
        let SerialFrom = $("#Pack_SerialFrom").val();
        let SerialTo = $("#Pack_SerialTo").val();
        if (SerialFrom == "0" || SerialFrom == "" || SerialTo == "0" || SerialTo == "") {
            $("#PackSrlzsnTable tbody tr #SrNo:contains('" + $("#Hdn_PackSrNo").val() + "')").closest('tr').each(function () {
                let row = $(this);
                let totalPacked_Quantity = row.find("#TotalQty").text();
                $("#Pack_Quantity").val(totalPacked_Quantity);
            })
        }
        else {
            let TotalPack = (parseInt(SerialTo) - parseInt(SerialFrom)) + 1;

            PackedQty = parseFloat(PackedQty) + parseFloat(CheckNullNumber($("#Pack_QtyPack").val())) * TotalPack;
            $("#Pack_Quantity").val((PackedQty).toFixed(QtyDecDigit));
            if (PackedQty == 0) {
                $("#Pack_SerialFrom").val("");
            }
        }
        
    }
    
    

}
function CheckQtyPerPack_quantity() {
    let ErrQty = "N";
    let SerialFrom = $("#Pack_SerialFrom").val();
    let SerialTo = $("#Pack_SerialTo").val();
    if (SerialTo != "" && SerialFrom != "") {
        let TotalPack = (parseInt(SerialTo) - parseInt(SerialFrom)) + 1;
        let QtyPack = $("#Pack_QtyPack").val();
        let TotalQty = TotalPack * parseFloat(CheckNullNumber(QtyPack));
        let Pack_Quantity = $("#Pack_Quantity").val();
        if (parseFloat(TotalQty) > parseFloat(CheckNullNumber(Pack_Quantity))) {

            $("#Pack_QtyPack").css("border-color", "red");
            $("#Pack_QtyPackError").css("display", "block");
            $("#Pack_QtyPackError").text($("#ExceedingQty").text());
            ErrQty = "Y";
        }
        else {

            $("#Pack_QtyPack").css("border-color", "#ced4da");
            $("#Pack_QtyPackError").css("display", "none");
            $("#Pack_QtyPackError").text("");
        }
    }
    if (ErrQty == "Y") {
        return false;
    } else {
        return true;
    }
    
}
function CheckSerializedQty(ItemId) {
    debugger;
    let FinTotalQty = 0;
    $("#PackSrlzsnTable tbody tr #ItemId:contains('" + ItemId + "')").closest('tr').each(function () {
        debugger;
        let row = $(this);
        let TotalQty = row.find("#TotalQty").text();
        FinTotalQty = parseFloat(FinTotalQty) + parseFloat(CheckNullNumber(TotalQty));
    })
    return FinTotalQty;
}
function CheckAvailableSerialNo(ItemId) {
    let FinTotalQty = 0;
    let arr = [];
    
    //$("#PackSrlzsnTable tbody tr #ItemId:contains('" + ItemId + "')").closest('tr').each(function () {
    $("#PackSrlzsnTable tbody tr").each(function () {
        let row = $(this);
        //let SerialFrom = row.find("#SerialFrom").text();
        let SerialTo = row.find("#SerialTo").text();
        arr.push(SerialTo);
    })
    return arr.length>0 ? Math.max(...arr)+1 : 1;
}
function PackingNoSetOnItemDetail(ItemId) {
    let arr = [];
    let TotalPacks = 0;
    let PhyPerPack = 0;
    let TotalNetWeight = 0;
    let TotalGrossWeight = 0;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    $("#PackSrlzsnTable tbody tr #ItemId:contains('" + ItemId + "')").closest('tr').each(function () {
        let row = $(this);
        //let SerialFrom = row.find("#SerialFrom").text();
        let td_TotalPacks = row.find("#TotalPacks").text();
        let td_PhyPerPack = row.find("#PhyPerPack").text();
        let td_TotalNetWeight = row.find("#TotalNetWeight").text();
        let td_TotalGrossWeight = row.find("#TotalGrossWeight").text();
        let SerialTo = row.find("#SerialTo").text();
        TotalPacks = parseFloat(TotalPacks) + parseFloat(CheckNullNumber(td_TotalPacks));
        PhyPerPack = parseFloat(PhyPerPack) + parseFloat(CheckNullNumber(td_PhyPerPack));
        TotalNetWeight = parseFloat(TotalNetWeight) + parseFloat(CheckNullNumber(td_TotalNetWeight));
        TotalGrossWeight = parseFloat(TotalGrossWeight) + parseFloat(CheckNullNumber(td_TotalGrossWeight));
        arr.push(SerialTo);
    })
   

    var CurrentRow = $("#PDItmDetailsTbl tbody tr #hdItemId[value=" + ItemId + "]").closest("tr");
    CurrentRow.find("#NumberOfPacks").val(parseFloat(TotalPacks).toFixed(QtyDecDigit));
    CurrentRow.find("#PhysicalPacks").val(parseFloat(PhyPerPack).toFixed(QtyDecDigit));
    CurrentRow.find("#NetWeight").val(parseFloat(TotalNetWeight).toFixed(WeightDecDigit));
    CurrentRow.find("#GrossWeight").val(parseFloat(TotalGrossWeight).toFixed(WeightDecDigit));
    CurrentRow.find("#PackingBoxbtn").removeClass("packing");
    if (parseFloat(PhyPerPack) > 0) {
        CurrentRow.find("#PackagingDetail").attr("disabled", false);
        
    } else {
        CurrentRow.find("#PackagingDetail").attr("disabled", true);
        CurrentRow.find("#CBM").val("");
        CurrentRow.find("#hdpackagingItemId").val("");
        CurrentRow.find("#hdpackagingItemName").val("");
    }
    

    CalCulatePackSerializationTotal();
    SumGross_Net_CBM();
}
//-----------------------------------End---------------------------------//
/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemPackQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemBalncQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemOrdQty");
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var pack_type = '';
    var pktyp = $("#rbDomestic").val();
    var pktype = $("#rbExport").val();

    if (pktyp || pktype) {
        if (pktyp == 'D') {
            pack_type = 'D'
        }
        else {
            pack_type = 'E'
        }
    }
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#hdSpanRowId").val();
    var Doc_no = $("#PackingNumber").val();
    var Doc_dt = $("#txtpack_dt").val();
    var SoNo = clickdRow.find("#OrderQtyDocNo").val();
    var Wh_id = "";
    var ProductNm = "";
    var ProductId = "";
    var UOM = "";
    var SoDate = "";
    var PackQty = clickdRow.find("#Order_PackedQty").val();
    if (flag == "Order" || flag == "Pending") {
        ProductNm = clickdRow.find("#ItemName").val();
        ProductId = clickdRow.find("#hdItemId").val();
        UOM = clickdRow.find("#UOM").val();
        SoDate = clickdRow.find("#OrderQtyDocDate").val();
    }
    else if (flag == "PackResTotalQty") {
        ProductNm = $("#ItemNameBatchWise").val();
        ProductId = $("#HDItemNameBatchWise").val();
        UOM = $("#UOMBatchWise").val();
        Wh_id = $("#HDWhIDBatchWise").val();
        PackQty = $("#QuantityBatchWise").val();
        var SoNos = "";
        if ($("#SaveItemOrderQtyDetails tbody tr #PD_OrderQtyItemID[value='" + ProductId + "']").length > 0) {
            $("#SaveItemOrderQtyDetails tbody tr #PD_OrderQtyItemID[value='" + ProductId + "']").closest("tr").each(function () {
                SoNos = SoNos == "" ? $(this).find("#PD_OrderQtyDocNo").val() : SoNos + "," + $(this).find("#PD_OrderQtyDocNo").val();
            });
        }
        SoNo = SoNos;
    }
    else if (flag == "PackResQuantity") {
        ProductNm = $("#ItemNameOrdRevSubItemDetails").val();
        ProductId = $("#HDItemIDOrdRevSubItemDetails").val();
        UOM = $("#UOMBatchWise").val();
        Wh_id = $("#HDWhidOrdRevSubItemDetails").val();
        SoNo = clickdRow.find("#tblorderno").val();
        SoDate = clickdRow.find("#hdn_orderdt").val();
        PackQty = clickdRow.find("#OrdResIssuedQty").val();
    }
    else {
        ProductNm = $("#OrderQtyItemName").val();
        ProductId = $("#hd_OQtyItemId").val();
        UOM = $("#OrderQtyUOM").val();
        SoDate = clickdRow.find("#OrderQtyDocDate").val().split("-").reverse().join("-");
    }

    if (flag == "Order" || flag == "Pending") {
        if ($("#SaveItemOrderQtyDetails tbody tr").length > 0) {
            var SoNo = "", SoDate = "";
            $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var rowitem = row.find("#PD_OrderQtyItemID").val();
                if (rowitem == ProductId) {

                    if (SoNo == "" && SoDate == "") {
                        SoNo = row.find("#PD_OrderQtyDocNo").val()
                        SoDate = row.find("#PD_OrderQtyDocDate").val().split("-").reverse().join("-");
                    } else {
                        SoNo = SoNo + "," + row.find("#PD_OrderQtyDocNo").val()
                        SoDate = SoDate + "," + row.find("#PD_OrderQtyDocDate").val().split("-").reverse().join("-");
                    }
                }
            });
        }
    }
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "PackList" || flag == "FocPackList"/*||flag == "Order" || flag == "Pending"*/) {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.ord_qty = row.find('#subItemOrdQty').val();
            List.pend_qty = row.find('#subItemPendQty').val();
            List.foc_qty = row.find('#subItemFocQty').val() || "0";
            List.ord_foc_qty = row.find('#subItemOrdFocQty').val() || "0";
            List.pend_foc_qty = row.find('#subItemPendFocQty').val() || "0";
            List.SoNo = row.find('#subItemSrcDocNo').val();
            NewArr.push(List);
        });
        Sub_Quantity = clickdRow.find("#SubItemPackQty").val();
        PackQty = flag == "FocPackList" ? clickdRow.find("#Order_PackedFocQty").val() : PackQty;
    }
    else if (flag == "Pending") {
        Sub_Quantity = clickdRow.find("#BalanceQuantity").val();
    }
    else if (flag == "Order") {
        Sub_Quantity = clickdRow.find("#OrderQuantity").val().trim();
    }
    else if (flag == "PackResQuantity") {
        $("#hdn_Sub_ItemResDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").find("#subItemSrcDocNo[value='" + SoNo + "']").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.res_qty = row.find('#subItemResQty').val();
            List.SoNo = row.find('#subItemSrcDocNo').val();
            List.SoDate = row.find('#subItemSrcDocDate').val();
            NewArr.push(List);
        });
        Sub_Quantity = clickdRow.find("#OrdResIssuedQty").val().trim();
    }
    else if (flag == "PackResTotalQty") {

        $("#hdn_Sub_ItemPackResDetailTblTemp tbody tr #ItemId[value='" + ProductId + "']").closest('tr').remove();
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            let item_id = row.find("#ItemId").val();
            let sub_item_id = row.find('#subItemId').val();
            let qty = row.find('#subItemQty').val();
            let qtyFoc = row.find('#subItemFocQty').val();
            qty = (parseFloat(CheckNullNumber(qty)) + parseFloat(CheckNullNumber(qtyFoc))).toFixed(QtyDecDigit);
            let len = $("#hdn_Sub_ItemPackResDetailTblTemp tbody tr td #subItemId[value=" + sub_item_id + "]").length;
            if (len > 0) {
                var hdnpackTable = $("#hdn_Sub_ItemPackResDetailTblTemp tbody tr td #subItemId[value=" + sub_item_id + "]").closest('tr');
                var OrdPackQty = hdnpackTable.find("#subItemOrdPackQty").val();
                qty = parseFloat(CheckNullNumber(qty)) + parseFloat(CheckNullNumber(OrdPackQty));
                hdnpackTable.find("#subItemOrdPackQty").val(qty);
                hdnpackTable.find("#subItemQty").val(qty);
            }
            else {
                $("#hdn_Sub_ItemPackResDetailTblTemp tbody").append
                    (`<tr>
                                <td><input type="text" id="ItemId" value='${item_id}'></td>
                                <td><input type="text" id="subItemId" value='${sub_item_id}'></td>
                                <td><input type="text" id="subItemQty" value='${qty}'></td>
                                <td><input type="text" id="subItemOrdPackQty" value='${qty}'></td>
                            </tr>
                  `);
            }


        });

        $("#hdn_Sub_ItemPackResDetailTblTemp tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            //let item_id = row.find("#ItemId").val();
            let sub_item_id = row.find('#subItemId').val();
            let ordpackQty = 0;
            //$("#hdn_Sub_ItemDetailTbl tbody tr td #subItemId[value=" + sub_item_id + "]").closest("tr").each(function () {
            //    var row = $(this);
            //    let qty = row.find('#subItemQty').val();
            //    ordpackQty = parseFloat(ordpackQty) + parseFloat(qty);
            //});
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.ord_pack_qty = row.find('#subItemOrdPackQty').val();
            NewArr.push(List);
        });
        $("#hdn_Sub_ItemPackResDetailTblTemp tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").remove();

        Sub_Quantity = $("#QuantityBatchWise").val();
    }
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hd_Status").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    hd_Status = IsNull(hd_Status, "").trim();
    var Amend = $("#Amend").val()
    if (Amend == "Amend") {
        IsDisabled = "N";
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DomesticPacking/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            SoNo: SoNo,
            SoDate: SoDate,
            pack_type: pack_type,
            Wh_id: Wh_id,
            Amend: Amend,
            DocumentMenuId: DocumentMenuId
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            if (flag == "PackList" || flag == "FocPackList" || flag == "PackResQuantity") {
                $("#subSrcDocNo").css('display', 'block');
                $("#subSrcDocdate").css('display', 'block');
                $("#Sub_SrcDocNo").val(SoNo);
                $("#Sub_SrcDocDate").val(SoDate);
                $("#hdSub_SrcDocDate").val(SoDate);
                $("#Sub_Quantity").val(PackQty);
            }
            if (flag == "Order" || flag == "Pending" || flag == "PackResTotalQty") {
                $("#Sub_Quantity").val(Sub_Quantity);
            }
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);

            if (flag == "PackResTotalQty") {
                var status = $("#hd_Status").val().trim();
                var Command = $("#Qty_pari_Command").val();
                var TransType = $("#qty_TransType").val();
                $("#Sub_ItemDetailTbl tbody tr").each(function () {
                    var curr = $(this);
                    var orderqty = curr.find("#subItemOrderPackQty").val();
                    if ((parseFloat(orderqty) > 0) && (Command == "Edit" || Command == "Add") && (status == "" || status == "D")) {
                        //curr.find("#subItemQty").prop("disabled", false);/***commented By Shubham maurya on 29-07-2024 for disable ***/
                        curr.find("#subItemQty").prop("disabled", true);
                    }
                    else {
                        if (Amend == "Amend") {
                            curr.find("#subItemQty").prop("disabled", false);
                        }
                        else {
                            curr.find("#subItemQty").prop("disabled", true);
                        }
                    }
                })
                
            }
            
        }
    });



}
function PackSubItemUnReserveListArr() {
    var NewArr = new Array();
    $("#hdn_Sub_ItemResDetailTbl tbody tr").each(function () {
        var row = $(this);
        if (parseFloat(CheckNullNumber(row.find('#subItemQty').val()))>0) {
        debugger;
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.qty = row.find('#subItemQty').val();
        List.src_doc_no = row.find('#subItemSrcDocNo').val();
        List.src_doc_date = row.find('#subItemSrcDocDate').val();
        List.res_qty = row.find('#subItemResQty').val();
        NewArr.push(List);
        }
    });

    return NewArr;
}
function SaveAndExitUnReserveSubItemDetails() {
    var ItemId = $("#HDItemIDOrdRevSubItemDetails").val();

    var ErrorFlag = "N";
    if ($("#HDBatchSubItem").val() == "Y") {
        $("#OrderReservedStockSubItemTbl tbody tr").each(function () {
            var Crow = $(this);
            var tblorderno = Crow.find("#tblorderno").val();
            var OrdResIssuedQty = Crow.find("#OrdResIssuedQty").val();
            var ChecksubItemQty = 0;
            var TblReserveSubItem = $("#hdn_Sub_ItemResDetailTbl > tbody > tr #ItemId[value='" + ItemId + "']").closest('tr').find("#subItemSrcDocNo[value='" + tblorderno + "']").closest('tr');
            TblReserveSubItem.each(function () {
                var InnerCrow = $(this);
                var subItemQty = InnerCrow.find("#subItemQty").val();
                ChecksubItemQty = parseFloat(ChecksubItemQty) + parseFloat(CheckNullNumber(subItemQty));
            });
            if (parseFloat(OrdResIssuedQty) != parseFloat(ChecksubItemQty)) {
                ErrorFlag = "Y";
                Crow.find("#SubItemPackResDetailQty").css("border", "1px solid red");
            }
        });
    }


    if (ErrorFlag == "Y") {
        swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        return false;
    } else {
        $("#SubItemReserveQtyIcon").css("border", "#ced4da");
        $("#OrderWiseSubItemReserved").modal('hide');
        return true;
    }

}
function SaveAndExitUnReserveSubItemDetailsForAllItems(ItemId) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145115") {
        QtyDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        QtyDigit = $("#QtyDigit").text();
    }
    var ErrorFlag = "N";

    var Table = $("#PDItmDetailsTbl tbody tr");
    if (ItemId != null && ItemId != "") {
        Table = $("#PDItmDetailsTbl tbody tr #hdItemId[value='" + ItemId + "']").closest('tr');
    }
    Table.each(function () {
        var Crow = $(this);
        var sub_item = Crow.find("#sub_item").val();
        //var batchable = Crow.find("#hdi_batch").val();
        var ItemId = Crow.find("#hdItemId").val();
        var OrdResIssuedQty = Crow.find("#PackedQuantity").val();
        var ChecksubItemQty = 0;
        if (sub_item == "Y"/* && batchable=="Y"*/) {
            var TblReserveSubItem = $("#hdn_Sub_ItemPackResDetailTbl > tbody > tr #ItemId[value='" + ItemId + "']").closest('tr');
            TblReserveSubItem.each(function () {
                var InnerCrow = $(this);
                var subItemQty = InnerCrow.find("#subItemQty").val();
                ChecksubItemQty = (parseFloat(ChecksubItemQty) + parseFloat(CheckNullNumber(subItemQty))).toFixed(QtyDigit);
            });
            if (parseFloat(OrdResIssuedQty) != parseFloat(ChecksubItemQty)) {
                ErrorFlag = "Y";
            }
        }
    });
    var ErrorFlag1 = "N";
    var arr = new Array();
    var resTable = $('#SaveOrderReservedItemBatchTbl tbody tr');
    if (ItemId != null && ItemId != "") {
        resTable = $("#SaveOrderReservedItemBatchTbl tbody tr #POR_ItemId[value='" + ItemId + "']").closest('tr');
    }
    resTable.each(function () {
        var row = $(this)
        var OrdResbatchList = {};

        OrdResbatchList.OrderNo = row.find('#POR_OrderNo').val();
        OrdResbatchList.OrderDt = row.find('#POR_OrderDate').val();
        OrdResbatchList.ItemId = row.find('#POR_ItemId').val();
        OrdResbatchList.UOMId = row.find('#POR_UOMId').val();
        OrdResbatchList.ResQty = row.find('#POR_ResAvlStk').val();
        OrdResbatchList.IssueQty = row.find('#POR_IssueQty').val();

        arr.push(OrdResbatchList);
    });

    var result = [];
    arr.reduce(function (res, value) {
        if (!res[value.ItemId]) {
            res[value.ItemId] = { ItemId: value.ItemId, OrderNo: value.OrderNo, ResQty: 0, IssueQty: 0 };
            result.push(res[value.ItemId])
        }
        res[value.ItemId].IssueQty += parseFloat(value.IssueQty);
        res[value.ItemId].ResQty += parseFloat(value.ResQty);
        
        return res;
    }, {});


    for (var i = 0; i < result.length; i++) {

        var ResItemId = result[i].ItemId;
        var OrderNo = result[i].OrderNo;
        var IssueQty = result[i].IssueQty;
        var ResQty = result[i].ResQty;
        if (parseFloat(CheckNullNumber(ResQty)) < parseFloat(CheckNullNumber(IssueQty))) {
            IssueQty = ResQty;
        }
        var ChecksubItemQty = 0;
        let checkSubItm = "";
        //let checkBatchable = "";
        $("#PDItmDetailsTbl tbody tr #hdItemId[value='" + ResItemId + "']").closest("tr").each(function () {
            checkSubItm = $(this).find("#sub_item").val();
            //checkBatchable = $(this).find("#hdi_batch").val();
        })
        if (checkSubItm == "Y"/* && checkBatchable=="Y"*/) {

            var TblReserveSubItem = $("#hdn_Sub_ItemResDetailTbl > tbody > tr #ItemId[value='" + ResItemId + "']").closest('tr');
            TblReserveSubItem.find("#subItemSrcDocNo[value='" + OrderNo + "']").closest('tr').each(function () {
                var InnerCrow1 = $(this);
                var subItemQty = InnerCrow1.find("#subItemQty").val();
                ChecksubItemQty = (parseFloat(ChecksubItemQty) + parseFloat(CheckNullNumber(subItemQty))).toFixed(QtyDigit);
            });
            if (parseFloat(IssueQty) != parseFloat(ChecksubItemQty)) {
                ErrorFlag1 = "Y";
            }
        }

    }
    if (ErrorFlag1 == "Y") {
        $("#SubItemReserveQtyIcon").css("border", "1px solid red");
    } else {
        $("#SubItemReserveQtyIcon").css("border", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        $("#SubItemBatchPopupQty").css("border", "1px solid red");
    } else {
        $("#SubItemBatchPopupQty").css("border", "#ced4da");
    }
    if (ErrorFlag1 == "Y" || ErrorFlag == "Y") {
        swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        return false;
    } else {
        $("#OrderWiseSubItemReserved").modal('hide');
        return true;
    }

}
function OnDeleteItemDeleteReferenceTable(ItemId) {
    var ResDetailTbl = $("#hdn_Sub_ItemResDetailTbl tbody tr #ItemId[value='" + ItemId + "']").closest('tr');
    var PackResDetailTbl = $("#hdn_Sub_ItemPackResDetailTbl tbody tr #ItemId[value='" + ItemId + "']").closest('tr');
    var PackOrderResDetailTbl = $("#SaveOrderReservedItemBatchTbl tbody tr #POR_ItemId[value='" + ItemId + "']").closest('tr');
    if (ResDetailTbl.length > 0) {
        ResDetailTbl.remove();
    }
    if (PackResDetailTbl.length > 0) {
        PackResDetailTbl.remove();
    }
    if (PackOrderResDetailTbl.length > 0) {
        PackOrderResDetailTbl.remove();
    }
}
function PackSubItemReserveListArr() {
    var NewArr = new Array();
    $("#hdn_Sub_ItemPackResDetailTbl tbody tr").each(function () {
        var row = $(this);
        if (parseFloat(CheckNullNumber(row.find('#subItemQty').val())) > 0) {
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.ord_pack_qty = row.find('#subItemOrdPackQty').val();
            NewArr.push(List);
        }
    });

    return NewArr;
}
function CheckValidations_forSubItems() {
    debugger
    return Cmn_CheckValidations_forSubItems("SaveItemOrderQtyDetails", "", "PD_OrderQtyItemID", "PD_OrderQtyPackedQty", "SubItemPackQty", "Y");
}
function ResetWorningBorderColor() {
    debugger;
    return Cmn_CheckValidations_forSubItems("OrderQtyInfoTbl", "", "hd_OQtyItemId", "Order_PackedQty", "SubItemPackQty", "PackPopup");
}
function GetSubItemAvlStock(e) {
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#hdSpanRowId").val();
    var ProductNm = Crow.find("#ItemName").val();
    var ProductId = Crow.find("#hdItemId").val();
    var UOM = Crow.find("#UOM").val();
    var AvlStk = Crow.find("#AvailableStock").val();
    var hdwh_Id = Crow.find("#wh_id" + rowNo).val();
    Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, hdwh_Id, AvlStk, "wh");
}

/***--------------------------------Sub Item Section End-----------------------------------------***/
function checkprameter() {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DomesticPacking/checkorderqtymorethenpackingqty",
        data: {},
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            //var hdn_checkpreameter = JSON.parse(data.table[0].param_stat);
            //$("#hdn_checkpreameter").val(hdn_checkpreameter)         
            var arr = [];
            arr = JSON.parse(data);
            if (arr.Table.length > 0) {
                $("#hdn_checkpreameter").val(arr.Table[0].param_stat);
            }
        }
    })
}

function ExportItemsListOrSerializationDetailsToExcel(action) {
    debugger;
    var packNo = $('#hdpack_no').val();
    if (packNo != null && packNo != "" && packNo != undefined) {
        window.location.href = "/ApplicationLayer/DomesticPacking/ExportItemsORSerializationDetailsToExcel?act=" + action + "&packNo=" + packNo;
    }
}

function approveonclick() { /**Added this Condition by Nitesh 09-01-2024 for Disable Approve btn after one Click**/
    debugger;

    var flagNoOfPacked = CheckPackItemValidations_NoOfPacked();
    if (flagNoOfPacked == false) {
        return false;
    }

    var HdnPackSerialization = $("#HdnPackSerialization").val();
    if (HdnPackSerialization == "Y") {
        var PL_PackSrlznDtlCheckValid = fn_PackSrlznDtlCheckValid();
        if (PL_PackSrlznDtlCheckValid == false) {
            return false;
        }
    }
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
    else {
        $("#btn_approve").css("filter", "grayscale(0%)");
        $("#hdnsavebtn").val("AllreadyclickApprove");
    }


}

function onChangeCurrency() {
    let CurrId = $("#ddlCurrencies").val();
    $("#Hdn_Curr_Id").val(CurrId);

    if (CurrId != "0") {

        document.getElementById("vmddlCurrencies").innerHTML = "";
        $("#ddlCurrencies").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmddlCurrencies").innerHTML = $("#valueReq").text();
        $("#ddlCurrencies").css("border-color", "red");
    }
    BindSONumberList();
}

function FilterSrlzsn(e) {
    
    var listdt = e.currentTarget.value.toLocaleLowerCase();
    var list = "";
    $("#PackSrlzsnTable >tbody > tr").each(function () {
        var currentRow = $(this);
        var DocName = currentRow.find("#ItemName").text();
        var UOM = currentRow.find("#UOM").text();
        var SerialFrom = currentRow.find("#SerialFrom").text();
        var SerialTo = currentRow.find("#SerialTo").text();
        
        if (checkfiltercolumns([DocName, UOM, SerialFrom, SerialTo], listdt)==-1) {
            currentRow.attr("hidden", true);
        } else {
            currentRow.attr("hidden", false);
        }
    });
}
function checkfiltercolumns(Arr, listdt) {
    let indexs = -1
    Arr.map((res, index) => {
        indexs = res.toLocaleLowerCase().indexOf(listdt) > -1 ? res.toLocaleLowerCase().indexOf(listdt) : indexs;
    })
    return indexs;
}

function OnClickBtnAmendment() {
    let AmendConfirm = "N";
    debugger;
   swal({
        title: "",//$("#deltital").text() + "?",
       text: $("#WouldYouLikeToSaveTheDocumentAsADraft").text(),
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-primary",
        confirmButtonText: "Yes, Amend it!",
        closeOnConfirm: false
    }, function (isConfirm) {
       if (isConfirm) {
           AmendConfirm = "Y";
           $("#Btn_Amendment").attr("onclick", "");
           $("#Btn_Amendment").click();
            return true;
        } else {
           AmendConfirm = "N";
        }
   });
    if (AmendConfirm == "Y") {
        return true;
    }
    else {
        return false;
    }
    
}
function OnchangeQtyPerinner() {
    if (DocumentMenuId == "105103145115") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    var qtyperpack = CheckNullNumber($("#Pack_QtyPack").val());
    var qtyperinner = CheckNullNumber($("#Qty_PerInner").val());
    if (parseFloat(qtyperpack) != parseFloat(0)) {
        if (parseFloat(CheckNullNumber(qtyperinner)) > parseFloat(CheckNullNumber(qtyperpack))) {
            $("#QtyPerInnerError").text($("#span_InvalidQuantity").text());
            $("#Qty_PerInner").css("border-color", "red");
            $("#QtyPerInnerError").css("display", "block");
            $("#TotalInnerBox").val("")
        }
        else {
            $("#QtyPerInnerError").text("");
            $("#QtyPerInnerError").css("display", "none");
            $("#Qty_PerInner").css("border-color", "#ced4da");

            if (parseFloat(qtyperinner) != parseFloat(0)) {
                var TotalInnerBox = parseFloat(qtyperpack) / parseFloat(qtyperinner)
                $("#TotalInnerBox").val(parseInt(CheckNullNumber(TotalInnerBox)).toFixed(QtyDecDigit))
            }
            else {
                $("#TotalInnerBox").val("")
            }
        }
    }
    else {
        $("#TotalInnerBox").val("")
    }

    $("#Qty_PerInner").val(parseFloat(qtyperinner).toFixed(QtyDecDigit))

  
}
function FilterItemDetail(e) {//added by Suraj Maurya on 13-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "PDItmDetailsTbl", [{ "FieldId": "ItemName", "FieldType": "input" }]);
}
function onclickReplicateWithAllItems() {
    $("#Replicat_wh_id_Error").css("display", "none");
    $("#Replicat_wh_id").css("border-color", "#ced4da");
    $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid #ced4da");
    BindWarehouseList("Replicate");
}
function OnChangeReplicatWarehouse() {
    var ErrorFlag = "N";
    var wh_id = $("#Replicat_wh_id").val();
    if (wh_id == "0" || wh_id == "") {
        $("#Replicat_wh_id_Error").text($("#valueReq").text());
        $("#Replicat_wh_id_Error").css("display", "block");
        $("#Replicat_wh_id").css("border-color", "red");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        $("#Replicat_wh_id_Error").css("display", "none");
        $("#Replicat_wh_id").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid #ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnClickSaveAndExitReplicateBtn() {
    debugger;
    var ErrorFlag = "N";
    var wh_id = $("#Replicat_wh_id").val();
    if (wh_id == "0" || wh_id == "") {
        $("#Replicat_wh_id_Error").text($("#valueReq").text());
        $("#Replicat_wh_id_Error").css("display", "block");
        $("#Replicat_wh_id").css("border-color", "red");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        $("#Replicat_wh_id_Error").css("display", "none");
        $("#Replicat_wh_id").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid #ced4da");
    }
    if (ErrorFlag == "Y") {
        $("#SaveExitReplicateBtn").attr("data-dismiss", "");
        return false;
    }
    $("#PDItmDetailsTbl > tbody > tr").each(function () {
        var row = $(this);
        //var rowNo = row.find("#hdSpanRowId").val();
        var rowNo = row.find("#SpanRowId").text();
        row.find("#wh_id" + rowNo).val(wh_id).trigger("change");
    });
    $("#SaveExitReplicateBtn").attr("data-dismiss", "modal");
}
function onclickItemorientation() {
    debugger;
    var Itemorientation = "";
    if ($("#ddlItemWise").is(":checked")) {
        Itemorientation = "IW";
        $("#hdn_Itemorientation").val(Itemorientation);
    }
    else {
        Itemorientation = "OW";
        $("#hdn_Itemorientation").val(Itemorientation);
    }
    if (status == "") {
        $("#ddlCustomerName").val("0").trigger('change')
        $("#ddlOrderNumber").val("0").trigger('change')
        document.getElementById("vmcust_id").innerHTML = "";
        $("#ddlCustomerName").css("border-color", "#ced4da");
        $("#Pack_ItemName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "#ced4da");

        document.getElementById("vmso_no").innerHTML = null;
        $("#ddlOrderNumber").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
    }
    HideAndShowItemorientationWise();

}

function HideAndShowItemorientationWise() {
    var status = $("#hd_Status").val();
    var Itemorientation = $("#hdn_Itemorientation").val();
    if (Itemorientation === "IW") {
        $("#Div_OrderNumber").css("display", "none");
        $("#Div_OrderDate").css("display", "none");
        $("#Div_AddIcon").css("display", "none");
      
     
        // $("#Div_ItemPlusBtn").css("display", "block");
    } else {
        $("#Div_OrderNumber").css("display", "block");
        $("#Div_OrderDate").css("display", "block");
        var ForDisable = $("#HdnForDisable").val();
        var Status = $("#hd_Status").val();
        if (ForDisable == "Enable" && Status == "A") {
            $("#Div_AddIcon").css("display", "none");
        }
        else if (Status=="C") {
            $("#Div_AddIcon").css("display", "none");
        }
        else {
            $("#Div_AddIcon").css("display", "block");
        }
       
    }
  
   

    $("#Div_ItemPlusBtn").css("display", "none");
}
function AddNewRow() {

    var rowIdx = 0;
    var rowCount = $('#PDItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#PDItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#hdSpanRowId").val());// + 1;
        levels.push(RowNo);
    });
    if (levels.length > 0) {
        RowNo = Math.max(...levels);
    }
    //RowNo = RowNo + 1;
    if (RowNo == "0") {
        RowNo = 1;
    }
    else {
        RowNo = RowNo + 1;
    }

    var CBM = "";
    var Td_balanceQty = "";
    var DisableIfSrlzn = "";

    var DocumentMenuId = $("#DocumentMenuId").val();
    var HdnPackSerialization = $("#HdnPackSerialization").val();

    if (DocumentMenuId == "105103145115") {
        CBM = `<td><input id="CBM" value="0.00" class="form-control date num_right" autocomplete="" type="text" name="CBM" required="required" placeholder="0000.00"  disabled="">
                                  <input type="hidden" id="hdpackagingItemId" value="" style="display: none;" />
                                  <input type="hidden" id="hdpackagingItemName" value="" style="display: none;"></td>`;
    }
    else {
        CBM = `<td><input id="CBM" value="0.000" class="form-control date num_right" autocomplete="" type="text" name="CBM" required="required" placeholder="0000.00"  disabled="">
                                  <input type="hidden" id="hdpackagingItemId" value="" style="display: none;" />
                                  <input type="hidden" id="hdpackagingItemName" value="" style="display: none;"></td>`;
    }


    if (HdnPackSerialization == "Y") {
        DisableIfSrlzn = "disabled";
        Td_balanceQty = `<div class=" col-sm-8 no-padding">
                                                    <input id="PackedQuantity" value="" class="form-control num_right" autocomplete="off" type="text" name="PackedQuantity" placeholder="0000.00" disabled>
                                                    <span id="PackedQuantity_Error" class="error-message is-visible"></span>
                                                 </div>
                                                 <div class="col-sm-4 no-padding">
                                                  <div class="col-sm-6 i_Icon">
                                                    <button type="button" class="calculator" onclick="OnClickItemOrderQtyIconBtn(event);" data-toggle="modal" data-target="#QuantityDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_QuantityDetail_Title").text()}"> </button>
                                                  </div>
                                                  <div class="col-sm-6 i_Icon">
                                                    <button type="button" id="PackingBoxbtn" class="calculator" onclick="OnClickPackingDetailsBtn(event);" data-toggle="modal" data-target="#PackingListDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/packing.png" alt="" title="${$("#span_PackSerializationDetail").text()}"> </button>
                                                  </div>
                                                    
                                                 </div>`
    } else {
        Td_balanceQty = `<div class="col-sm-8 no-padding">
                                                    <input id="PackedQuantity" value="" class="form-control num_right" autocomplete="off" type="text" name="PackedQuantity" placeholder="0000.00" disabled>
                                                    <span id="PackedQuantity_Error" class="error-message is-visible"></span>
                                                  </div>
                                                  <div class=" col-sm-4 no-padding">
                                                   <div class="col-sm-6 i_Icon">
                                                    <button type="button" class="calculator" onclick="OnClickItemOrderQtyIconBtn(event);" data-toggle="modal" data-target="#QuantityDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_QuantityDetail_Title").text()}"> </button>
                                                   </div>
                                                    
                                                 </div>`
    }
    //<input id="ItemName" value="" class="form-control" autocomplete="off" type="text" name="ItemName" required="required" placeholder="${$(" #ItemName").text()}"  onblur = "this.placeholder='${$("#ItemName").text()}'" disabled >
    $('#PDItmDetailsTbl tbody').append(`<tr id="${++rowIdx}">
                                   <td class="center"><input type="checkbox" class="tableflat" id="PackingCheck"></td>
                                   <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                   <td class="sr_padding"><span id="SpanRowId">${rowCount}</span>
                                  <input type="hidden" id="hdSpanRowId" value="${RowNo}" style="display: none;" />
                                    
                                       </td>
                                   <td class="ItmNameBreak itmStick tditemfrz"><div class=" col-sm-10 no-padding">
                                  
                                      <select class="form-control" id="ItemName${RowNo}" name="ItemName" onchange ="OnChangePackingItemName(${RowNo},event)"></select>
                                     <input type="hidden" id="hdItemId" value="" style="display: none;" />
                                    <input type="hidden" id="ItemName"  value="" style="display: none;" />
                                   </div>
                                   <div class="col-sm-1 i_Icon">
                                   <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                   </div><div class="col-sm-1 i_Icon"><button type="button" id="BuyerInfoIcon" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button></div>
                                  </td>
                                  <td><input id="UOM" value="" class="form-control" autocomplete="off" type="text" name="UOM" required="required" placeholder="${$("#ItemUOM").text()}" disabled>
                                  <input type="hidden" id="hdUOMId" value="" style="display: none;" />
                                  </td>
<td>
                                                                                <input id="PackSize" maxlength="50" class="form-control" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" value="" onmouseover="OnMouseOver(this.value)">
                                                                            </td>
                                  <td style="display:none">
                                    <div class="col-sm-9 lpo_form no-padding">
                                    <input id="OrderQuantity" value="" class="form-control num_right" autocomplete="off" type="text" name="OrderQuantity" required="required" placeholder="0000.00"  disabled>
                                    </div>
                                    <div class="col-sm-3 i_Icon no-padding" id="div_SubItemOrdQty">
                                    <input hidden type="text" id="sub_item" value="" />
                                    <button type="button" id="SubItemOrdQty" class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Order',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                    </td>
                                  <td style="display:none">
                                  <div class="col-sm-9 lpo_form no-padding">
                                  <input id="BalanceQuantity"  value="" class="form-control num_right" autocomplete="off" type="text" name="ord_qty_spec" required="required" placeholder="0000.00"  disabled>
                                  <span id="ord_qty_specError" class="error-message is-visible"></span>
                                  </div>
                                  <div class="col-sm-3 i_Icon no-padding" id="div_SubItemBalncQty">
                                  <button type="button" id="SubItemBalncQty"  class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Pending',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                  </div>
                                  </td>
                                  <td>
                                  <div class="lpo_form">
                                        ${Td_balanceQty}
                                  </div></td>
                                  <td><div class="lpo_form">
                                  <input id="NumberOfPacks" ${DisableIfSrlzn} value="" onkeypress="return OnKeyPressNumberOfPacks(this,event);" onchange="OnChangeNumberOfPacks(this,event)"  class="form-control num_right" autocomplete="off" type="text" name="NumberOfPacks"  placeholder="0000.00"  >
                                  <span id="NumberOfPacks_Error" class="error-message is-visible"></span></div></td>
                                  <td><div class="lpo_form">
                                  <input id="PhysicalPacks" ${DisableIfSrlzn} value="" class="form-control num_right" onchange="OnChangephy_pack(this,event)" autocomplete="off" type="text" name="PhysicalPacks"  placeholder="0000.00"  >
                                  <span id="PhysicalPacks_Error" class="error-message is-visible"></span></div></td>
                                  <td><div class="lpo_form">
                                  <input id="NetWeight" ${DisableIfSrlzn}  value="" onchange="OnChangeNetWeight(this,event)" onkeypress="return OnKeyPressNetWeight(this,event);" class="form-control num_right" autocomplete="off" type="text" name="NetWeight" placeholder="0000.00" ><input type="hidden" id="hditemnetwgt" value="" style="display: none;" />
                                  <span id="NetWeight_Error" class="error-message is-visible"></span></div></td>
                                  <td><input id="GrossWeight" ${DisableIfSrlzn} value="" onchange="OnChangeGrossWeight(this,event)" onkeypress="return OnKeyPressGrossWeight(this,event);"  class="form-control num_right" autocomplete="off" type="text" name="GrossWeight"  placeholder="0000.00" ><input type="hidden" id="hditemgrosswgt" value="" style="display: none;" /></td>
                                  <td><div class=" col-sm-11 no-padding"><div class="lpo_form">
                                  <select class="form-control" id="wh_id${RowNo}" onchange="OnChangeWarehouse(this,event)"></select>
                                  <span id="wh_Error" class="error-message is-visible"></span>
                                  </div></div>
                                  <div class=" col-sm-1 i_Icon">
                                  <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                                  </div></td>
                                  <td>
                                    <div class="col-sm-9 lpo_form no-padding">
                                        <input id="AvailableStock" value="0.000" class="form-control date num_right" autocomplete="" type="text" name="AvailableStock" required="required" placeholder="0000.00"  disabled="">
                                     </div>
                                    <div class="col-sm-3 i_Icon" id="div_SubItemAvlStk">
                                        <button type="button" id="SubItemAvlStk"  class="calculator subItmImg" onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                  </td>
                                  <td class="center"><button type="button" id="btnbatchdeatil" onclick="ItemStockBatchWise(this,event)" class="calculator " data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#BatchSpanDetail").text()}"></i></button>
                                  <input type="hidden" id="hdi_batch" value="" style="display: none;">
                                  <input type="hidden" id="hdi_serial" value="" style="display: none;">
                                  </td>
                                  <!--<td class="center" hidden><button type="button" id="btnserialdeatil" onclick="ItemStockSerialWise(this,event)" class="calculator " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#SerialSpanDetail").text()}"></i></button>
                                  <input type="hidden" id="hdi_serial" value="" style="display: none;">
                                  </td>-->
                                  <td class="center"><button type="button" id="PackagingDetail" onclick="OnClickPackagingDetailBtn(this,event)" class="calculator " data-toggle="modal" data-target="#PackagingDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#Span_PackingDetail_Title").text()}"></i></button>
                                  </td>
                                  `+ CBM + `
                                  <td><textarea id="remarks" class="form-control remarksmessage" maxlength="100" name="remarks"  placeholder="${$("#span_remarks").text()}"></textarea>
                                  </td>
                                  </tr>`);
    BindPackingListItemDropdown(RowNo);
    BindWarehouseList(RowNo);
}
function BindPackingListItemDropdown(ID) {
    debugger;
    var ItmDDLName = "#ItemName";
    var TableID = "#PDItmDetailsTbl";
    var SnoHiddenField = "#hdSpanRowId";
    $(ItmDDLName + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    $('#Textddl' + ID).append(`<option data-uom="0" value="0">---Select---</option>`);
    DynamicSerchableItemDDL(TableID, ItmDDLName, ID, SnoHiddenField, "", "SO")



}
function OnChangePackingItemName(RowID, e) {
    debugger;

    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#hdSpanRowId").val();
    var Item_ID=  clickedrow.find("#hdItemId").val();
    ClearRowDetails(e, Item_ID);

    var ItemCode = clickedrow.find("#ItemName" + SNo).val();

    var ItemName = clickedrow.find("#ItemName" + SNo + " option:selected").text();
    //clickedrow.find("#ItemName" + SNo).text(ItemName);
    var ItemID = "";
    if (ItemCode != "" && ItemCode != "0" && ItemCode != null) {
        clickedrow.find("#hdItemId").val(ItemCode);
       clickedrow.find("#ItemName").val(ItemName);
    }


    $("#hdn_Sub_ItemDetailTbl TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var rowitem = row.find("#ItemId").val();
        if (rowitem == Item_ID) {
            $(this).remove();
        }
    });
    BindPackingListUom(e);
}
function BindPackingListUom(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hdItemId").val();

    var Itm_ID;
    var SNo = clickedrow.find("#hdSpanRowId").val();

    //Itm_ID = clickedrow.find("#SQItemListName" + SNo).val();
    //clickedrow.find("#hfItemID").val(Itm_ID);
  

    DisableHeaderField();
    debugger;
    Cmn_BindUOM(clickedrow, ItemID, "", "", "PackingList")
    GetOrderQty(clickedrow,ItemID);


}
function ClearRowDetails(e, ItemID) {
    debugger;
    var clickedrow = $(e.target).closest("tr");



  var  ItemCode = ItemID;
    Cmn_DeleteSubItemQtyDetail(ItemCode);
    OnDeleteItemDeleteReferenceTable(ItemCode)
    $("#SaveItemOrderQtyDetails TBODY TR ").each(function () {
        var curr = $(this).closest("tr");
        var pditem = curr.find("#PD_OrderQtyItemID").val();
        if (pditem == ItemCode) {
            //$(this).remove();
            $(this).closest('tr').remove();
        }
    })
        //: contains(" + ItemCode + ")").closest("tr").remove();
   
    //$("#PackSrlzsnTbody tr #ItemId:contains(" + ItemCode + ")").closest("tr").remove();
    CalCulatePackSerializationTotal();
    DeleteItemBatchSerialOrderQtyDetails(ItemCode);
    updateItemSerialNumber();
    SumGross_Net_CBM();
    DisableAllPackingCheck();
    ResetPackingItemDdl();

    if ($("#PackSrlzsnTable tbody tr").length > 0) {
        $("#PackSrlzsnTbody TBODY TR ").each(function () {
            var curr = $(this).closest("tr");
            var ser_item = curr.find("#ItemId").val();
            if (ser_item == ItemCode) {
                //$(this).remove();
                $(this).closest('tr').remove();
                ResetPackDetailFields();

                PackingNoSetOnItemDetail(ItemId);
                ResetSerialNumber();
                CalCulateTotalPackingDetail();
                CalCulateTotalPackingDetailsForAllItems();
            }

        })
    }

    var SNo = clickedrow.find("#hdSpanRowId").val();

    clickedrow.find("#UOM").val("");
    clickedrow.find("#hdUOMId").val("");
    clickedrow.find("#OrderQuantity").val("");
    clickedrow.find("#BalanceQuantity").val("");
    clickedrow.find("#PackedQuantity").val("");
    clickedrow.find("#NumberOfPacks").val("");
    clickedrow.find("#PhysicalPacks").val("");
    clickedrow.find("#NetWeight").val("");
    clickedrow.find("#hditemnetwgt").val("");
    clickedrow.find("#GrossWeight").val("");
    clickedrow.find("#hditemgrosswgt").val("");
    clickedrow.find("#wh_id" + SNo).val("0").trigger('change');
    clickedrow.find("#AvailableStock").val("");
    clickedrow.find("#hdi_batch").val("");
    clickedrow.find("#hdi_serial").val("");
    


}
function DisableHeaderField() {
    debugger;
    $(".ddlItemOrientation").attr('disabled', true);
    $("#ddlCustomerName").attr('disabled', true);
    $("#Customer_Name").attr('disabled', true);

}
function GetOrderQty(e,ItemID)
{
    debugger;
    try {
        var packingNo = "";
        var status = $("#hd_Status").val();
        if (status != "" && status != null) {
            packingNo = $("#PackingNumber").val();
        }

      var custtomerID= $("#ddlCustomerName" + " option:selected").val();
        var PackType;
        let DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103130") {
            PackType = "DPacking";
        }
        else {
            PackType = "EPacking";
        }
        $.ajax({
            type: "Post",
            url: "/ApplicationLayer/DomesticPacking/GetOrderQtyAndBalenceQty",
            data: {
                custtomerID: custtomerID, ItemID: ItemID, PackType: PackType, DocumentMenuId: DocumentMenuId,
                packingNo: packingNo},
            success: function (data) {
                debugger;

                if (data !== null && data !== "") {
                    var obj = JSON.parse(data);

                    if (obj.Table.length > 0 && obj.Table1.length > 0) {
                        var currentrow = $(e).closest('tr');

                        // Get base quantity data from Table[0]
                        if (obj.Table[0].ord_qty_base != "NaN" && obj.Table[0].ord_qty_base != "") {
                            currentrow.find("#OrderQuantity").val(obj.Table[0].ord_qty_base);
                            currentrow.find("#BalanceQuantity").val(obj.Table[0].pending_qty);
                        }
                        else {
                            currentrow.find("#OrderQuantity").val("0");
                            currentrow.find("#BalanceQuantity").val("0");
                        }
                       

                        // Loop through Table1 (multiple document rows)
                        obj.Table1.forEach(function (row) {
                            var DocNo = row.app_so_no;
                            var DocDate = row.so_dt;
                            var ItemID = row.item_id;
                            var UOMID = row.uom_id;
                            var OrderQty = row.ord_qty_base;
                            var PendingQty = row.pending_qty;

                            var CheckDocNo = "";
                            var packedQty = "";
                            // Check for duplicates
                            $("#SaveItemOrderQtyDetails TBODY TR").each(function () {
                                var existingRow = $(this);
                                var rowItemID = existingRow.find("#PD_OrderQtyItemID").val();
                                var rowDocNo = existingRow.find("#PD_OrderQtyDocNo").val();
                                if (rowItemID == ItemID && rowDocNo == DocNo) {
                                    CheckDocNo = DocNo;
                                }
                            });

                            if (CheckDocNo === "") {
                                $('#SaveItemOrderQtyDetails tbody').append(`
                    <tr>
                        <td><input type="text" id="PD_OrderQtyItemID" value="${ItemID}" /></td>
                        <td><input type="text" id="PD_OrderQtyUomID" value="${UOMID}" /></td>
                        <td><input type="text" id="PD_OrderQtyDocNo" value="${DocNo}" /></td>
                        <td><input type="text" id="PD_OrderQtyDocDate" value="${DocDate}" /></td>
                        <td><input type="text" id="PD_OrderQty" value="${parseFloat(OrderQty).toFixed(QtyDecDigit)}" /></td>
                        <td><input type="text" id="PD_OrderQtyPendingQty" value="${parseFloat(PendingQty).toFixed(QtyDecDigit)}" /></td>
                        <td><input type="text" id="PD_OrderQtyPackedQty" value="${parseFloat(0).toFixed(QtyDecDigit)}" /></td>
                        <td><input type="text" id="PD_OrdFocQty" value="${parseFloat(row.pending_qty||0).toFixed(QtyDecDigit)}" /></td>
                        <td><input type="text" id="PD_OrdFocPendingQty" value="${parseFloat(row.pending_qty||0).toFixed(QtyDecDigit)}" /></td>
                        <td><input type="text" id="PD_OrdFocPackedQty" value="${parseFloat(0).toFixed(QtyDecDigit)}" /></td>
                    </tr>`);
                            }
                        });
                    }
                }



            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
                //alert("some error");
                hideLoader();
            }
        });
    }
    catch (ex) {
        console.log(ex)
        hideLoader();
    }
}
// Added by Nidhi on 19-05-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var custID = $("#ddlCustomerName" + " option:selected").val();
    Cmn_SendEmail(docid, custID, 'Cust');
}
function ViewEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $("#hd_Status").val();
    var docid = $("#DocumentMenuId").val();
    var PLNo = $("#PackingNumber").val();
    var PLDate = $("#txtpack_dt").val();
    var filepath = "";
    var fileName = 'PL_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DomesticPacking/SavePdfDocToSendOnEmailAlert_Ext",
        data: { PLNo: PLNo, PLDate: PLDate, fileName: fileName },
        success: function (data) {
            filepath = data;
            $('#hdfilepathpdf').val(filepath)
        }
        
    });
    Cmn_ViewEmailAlert(mail_id, status, docid, PLNo, PLDate, $('#hdfilepathpdf').val());
}
function SendEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $("#hd_Status").val();
    var docid = $("#DocumentMenuId").val();
    var PLNo = $("#PackingNumber").val();
    var PLDate = $("#txtpack_dt").val();
    var statusAM = $("#Amendment").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_SendEmailAlert(mail_id, status, docid, PLNo, PLDate, statusAM, "/ApplicationLayer/DomesticPacking/SendEmailAlert", filepath)
}
function EmailAlertLogDetails() {
    var PLDate = $("#txtpack_dt").val();
    var status = $("#hd_Status").val();
    var docid = $("#DocumentMenuId").val();
    var PLNo = $("#PackingNumber").val();
    Cmn_EmailAlertLogDetails(status, docid, PLNo, PLDate)
}

function OnclickDownExcelSerlization() {

    var rowCount = $("#PDItmDetailsTbl tbody tr").length;

    if (rowCount === 0) {
        swal("", $("#noitemselectedmsg").text(), "warning");
        return;
    }

    var PLItemDetailList = [];

    $("#PDItmDetailsTbl tbody tr").each(function () {
        var row = $(this);
        PLItemDetailList.push({
            id: row.find('#SpanRowId').text(),
            ItemName: row.find("#ItemName").val(),
            UOMName: row.find('#UOM').val(),
            PackedQuantity: row.find('#PackedQuantity').val()
        });
    });

    // Create a form dynamically for POST download
    var form = $('<form>', {
        action: '/ApplicationLayer/DomesticPacking/DownloadFileExcel',
        method: 'POST'
    });

    $('<input>').attr({
        type: 'hidden',
        name: 'ItemTableData',
        value: JSON.stringify(PLItemDetailList)
    }).appendTo(form);

    form.appendTo('body').submit();
    form.remove();
}

function FetchPackingSerializationData() {
    debugger;

    var rowCount = $("#PDItmDetailsTbl tbody tr").length;
    if (rowCount === 0) {
        swal("", $("#noitemselectedmsg").text(), "warning");
        return;
    }

    var PLItemDetailList = [];

    $("#PDItmDetailsTbl tbody tr").each(function () {
        var row = $(this);
        PLItemDetailList.push({
            id: row.find('#SpanRowId').text(),
            ItemName: row.find("#ItemName").val(),
            UOMName: row.find('#UOM').val(),
            PackedQuantity: row.find('#PackedQuantity').val()
        });
    });

    var ItemTableData = JSON.stringify(PLItemDetailList);
    var isfileexist = $('#ChooseSerialNofile').val();

    if (isfileexist) {
        FetchAndValidateDataPAckingSerlization('0', ItemTableData);
    } else {
        swal("", "Please choose file", "warning");
    }
}
function FetchAndValidateDataPAckingSerlization(uploadStatus, ItemTableData) {
    debugger;

    $(".loader1").show();

    var formData = new FormData();
    var file = $("#ChooseSerialNofile").get(0).files[0];

    formData.append("file", file);
    formData.append("uploadStatus", uploadStatus);
    formData.append("ItemTableData", ItemTableData); // ✅ JSON IN BODY

    $('#btnImportData').prop('disabled', false)
        .css('background-color', '#007bff');

    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];

    var xhr = new XMLHttpRequest();

    // ✅ NO JSON IN URL
    xhr.open('POST', addr + '/ValidateExcelFile', true);

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {

            $('#PartialImportPackingSerialization').html(xhr.response);
            cmn_apply_datatable("#TblPackingSerializationImportEXL");

            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            var InvalidData = parseInt($("#hdnNotOkPackSerli").val());

            if (
                responseMessage === "Excel file is empty. Please fill data in excel file and try again" ||
                responseMessage === "ErrorPage" ||
                InvalidData > 0
            ) {
                $('#btnImportData').prop('disabled', true)
                    .css('background-color', '#D3D3D3');
            } else {
                $('#btnImportData').prop('disabled', false)
                    .css('background-color', '#007bff');
            }
        }
    };

    xhr.send(formData);
}
function SearchByUploadStatusUploadStatusPackSeriali() {
    debugger;

    var PLItemDetailList = [];

    $("#PDItmDetailsTbl tbody tr").each(function () {
        var row = $(this);

        PLItemDetailList.push({
            id: row.find('#SpanRowId').text(),
            ItemName: row.find("#ItemName").val(),
            UOMName: row.find('#UOM').val(),
            PackedQuantity: row.find('#PackedQuantity').val()
        });
    });

    var ItemTableData = JSON.stringify(PLItemDetailList);
    var uploadStatus = $('#UploadStatusPackSeriali').val();

    if ($('#TblPackingSerializationImportEXL tbody tr').length > 0) {
        // ✅ SAFE: JSON goes in FormData BODY
        FetchAndValidateDataPAckingSerlization(uploadStatus, ItemTableData);
    }
}
function onclickImportData() {
    if ($.fn.DataTable.isDataTable('#TblPackingSerializationImportEXL')) {
        $('#TblPackingSerializationImportEXL').DataTable().clear().destroy();
    }
    $('#TblPackingSerializationImportEXL tbody').empty();
}
function ImportDataPackingSerilization(btn) {

    // 🔥 SHOW LOADER ONCE
    $('#loader').show();

    // 🔥 Allow UI to paint loader
    setTimeout(function () {

        // Clear existing table ONCE
        if ($.fn.DataTable.isDataTable('#PackSrlzsnTable')) {
            $('#PackSrlzsnTable').DataTable().clear().destroy();
        }
        $('#PackSrlzsnTable tbody').empty();

        var table = $('#TblPackingSerializationImportEXL').DataTable();
        let rowsToAppend = [];

        // 1️⃣ COLLECT DATA ONLY
        table.rows().every(function () {

            var currrow = $(this.node());

            rowsToAppend.push({
                SrNo: 0,
                ItemName: currrow.find("#EXL_ItemName").text(),
                ItemId: currrow.find("#EXL_itemID").val(),
                UOM: currrow.find("#EXL_UOM").text(),
                UOMId: currrow.find("#EXL_uomid").val(),
                PackQty: CheckNullNumber(currrow.find("#EXL_Pkd_Qty").text()),
                SerialFrom: CheckNullNumber(currrow.find("#EXL_sr_FM").text()),
                SerialTo: CheckNullNumber(currrow.find("#EXL_sr_to").text()),
                TotalPacks: CheckNullNumber(currrow.find("#EXL_Tot_out_pck").text()),
                QtyPerPack: CheckNullNumber(currrow.find("#EXL_out_pck").text()),
                PhyPerPack: CheckNullNumber(currrow.find("#EXL_tot_phyOut_pck").text()),
                TotalQty: CheckNullNumber(currrow.find("#EXL_Pkd_Qty").text()),
                netweight_perpiece: CheckNullNumber(currrow.find("#EXL_NWPpcs").text()),
                NetWeight: CheckNullNumber(currrow.find("#EXL_NWPpck").text()),
                TotalNetWeight: CheckNullNumber(currrow.find("#EXL_tot_NWPpck").text()),
                GrossWeight_perPiece: CheckNullNumber(currrow.find("#EXL_grwppcs").text()),
                GrossWeight: CheckNullNumber(currrow.find("#EXL_grwppck").text()),
                TotalGrossWeight: CheckNullNumber(currrow.find("#EXL_tot_grwppck").text()),
                PerInner: CheckNullNumber(currrow.find("#EXL_inn_pck").text()),
                TotalInner_Box: CheckNullNumber(currrow.find("#EXL_tot_inn_pck").text())
            });
        });
      
        // 2️⃣ APPEND ALL ROWS
        rowsToAppend.forEach(function (rowData) {
            AppendPackingListDetail(rowData);
        });

        // 3️⃣ RUN CALCULATIONS ONCE
        CalCulateTotalPackingDetail();
        ResetSerialNumber();
        ResetPhyCount();
        CalCulateTotalPackingDetailsForAllItems();

        // 4️⃣ Set packing no per unique item
        [...new Set(rowsToAppend.map(r => r.ItemId))]
            .forEach(PackingNoSetOnItemDetail);

        // 🔥 HIDE LOADER
        $('#loader').hide();
        if ($("#PackSrlzsnTable tbody tr").length > 0) {

            $("#AutoSerialization").attr("disabled", false);

        }
        else {
            $("#AutoSerialization").attr("disabled", true);
        }
        Closemodal(btn);

    }, 50); // small delay for UI repaint
}
function onCheckedChangeFormatBtn() {
    debugger;
    if ($("#OrderTypeImport").is(":checked")) {
        $('#PrintFormat').val('F2');
    }
    else {
        $('#PrintFormat').val('F1');
    }
}




