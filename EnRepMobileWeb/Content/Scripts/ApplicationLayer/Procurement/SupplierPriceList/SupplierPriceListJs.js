let QtyDecDigit = $("#QtyDigit").text();
let RateDecDigit = $("#RateDigit").text();///Rate And Percentaconst
let ValDecDigit = $("#ValDigit").text();///Amount
$(document).ready(function () {
    $("#portfolio").select2();
    $("#catalog").select2();
    $("#SupplierName").select2();
    ReplicateWith();
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var supp_id = clickedrow.children("#supp_id").text();
        if (supp_id != "" && supp_id != null) {
            location.href = "/ApplicationLayer/SupplierPriceList/AddSupplierPriceListDetail/?DocNo=" + supp_id + "&ListFilterData=" + ListFilterData;
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var InvoiceNo = clickedrow.children("#supp_id").text();
        var InvDate = clickedrow.children("#createdt").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(InvoiceNo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(InvoiceNo, InvDate, Doc_id, Doc_Status);
    });
    $('#SupplierPriceListTbl tbody').on('click', '.deleteIcon', function () {
        debugger;
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
        $(this).closest('tr').remove();
        if ($("#SupplierPriceListTbl >tbody >tr").length == 0) {
            $("#ddlReplicateWith").attr('disabled', false);
        }
        SerialNoAfterDelete();       
    });
    debugger;
    var InvoiceNo = $("#hdnSuppId").val();
    $("#hdDoc_No").val(InvoiceNo);
    var hdStatus = $('#hfPIStatus').val();
    var InvoiceDate = $('#Create_DT').val();

    var datePart = InvoiceDate.split(" ")[0]; // "15-12-2025"
    var parts = datePart.split("-");

    var formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];

    GetWorkFlowDetails(InvoiceNo, formattedDate, "105101101", hdStatus);
});
function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    $("#SupplierPriceListTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
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
function OnChangeSupplier() {
    debugger;
    var suppdt = $("#SupplierName").val();
    if (suppdt == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#AddItemsInDPI").css("display", "none")
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");

        var spDetail = suppdt.split(",");
        var supp_id = spDetail[0];
        var catId = spDetail[1];
        var catNm = spDetail[2];
        var portId = spDetail[3];
        var portNm = spDetail[4];

        $("#hdnSuppId").val(supp_id);
        $("#Category").val(catNm);
        $("#HdnCategory").val(catId);

        $("#Portfolio").val(portNm);
        $("#HdnPortfolio").val(portId);
    }   
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
function BtnSearch() {
    FilterSPL_List();
    ResetWF_Level();
}
function FilterSPL_List() {
    try {
        var catalog = $("#catalog option:selected").val();
        var portfolio = $("#portfolio option:selected").val();
        var supp_act = $("#supp_act").val();
        var AsDate = $("#AsDate").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SupplierPriceList/SPListSearch",
            data: {
                catalog: catalog,
                portfolio: portfolio,
                supp_act: supp_act,
                AsDate: AsDate
            },
            success: function (data) {
                $('#SupplierPriceLt').html(data);
                $("#ListFilterData").val(catalog + ',' + portfolio + ',' + supp_act + ',' + "")
            },
            error: function OnError(xhr, errorType, exception) {
            }
        });
    } catch (err) {
        console.log("SuppPI Error : " + err.message);
    }
}
function AddNewRow() {

    var suppdt = $("#SupplierName").val();
    if (suppdt == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#AddItemsInDPI").css("display", "none")
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");

        var S_NO = $('#SupplierPriceListTbl tbody tr').length + 1;
        var RowNo = 0;
        if (RowNo == "0") {
            RowNo = 1;
        }
        $("#SupplierPriceListTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;
        });
        $('#SupplierPriceListTbl tbody').append(`<tr id="R${S_NO}">
                                                <td class=" red center">
                                                    <i class="deleteIcon fa fa-trash" aria-hidden="true" title=${$("#Span_Delete_Title").text()}></i>
                                                </td>
                                                
                                                    <td class="sr_padding" id="srno">${RowNo}</td>
                                                    
                                                <td> 
                                                 <div class="lpo_form">
                                                    <div class="col-sm-11 no-padding">
                                                       <select class="form-control" id="SPLItemListName${RowNo}" name="SPLItemListName${RowNo}" onchange="OnChangePOItemName(${RowNo},event)" ></select>
                                                    </div>
                                                    <div class="col-sm-1 i_Icon">
                                                        <button type="button" class="calculator subItmImg" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" onclick="OnClickIconBtn(event);" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="@Resource.ItemInformation">  </button>
                                                    </div>
                                                    <span id="SpanSPLItemListName_Error${S_NO}" class="error-message is-visible"></span>
                                                    <input class="" type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
</div>
                                                </td>
                                                <td><input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder=${$("#ItemUOM").text()} readonly="readonly">
                                                    <input type="hidden" id="UOMID" value="">
                                                </td>
                                                <td> <input id="PackSize" autocomplete="off" class="form-control" name="PackSize" placeholder=${$("#span_PackSize").text()}></td>
                                                <td><input id="MRP" autocomplete="off" onchange="onchangeMRP(event)" class="form-control num_right" name="MRP" placeholder="0000.00"></td>
                                                <td>
                                                    <div class="lpo_form">
                                                        <input id="Price" autocomplete="off" onchange="onchangePrice(event)" class="form-control num_right" name="Price" placeholder="0000.00">
                                                        <span id="Span_Price" class="error-message is-visible"></span>
                                                    </div>
                                                </td>
                                                <td><input id="DiscountInPercentage" autocomplete="off" onchange="onchangeDiscountInPercentage(event)" onkeypress="return FloatValuePerOnly(this,event);" class="form-control num_right" name="DiscountInPercentage" placeholder="0000.00"></td>
                                                <td><input id="EffectivePrice" class="form-control num_right" name="EffectivePrice" placeholder="0000.00" disabled="disabled"></td>
                                                <td class="center">
                                                    <button type="button" id="SubItemAvlQty" class="calculator subItmImg" data-toggle="modal" onclick="" data-target="#RevisionHistory" data-backdrop="static" data-keyboard="false"> <i class="fa fa-refresh ref" id="" onclick="" title="@Resource.Revision @Resource.History "></i> </button>
                                                </td>
    </tr>`)
        BindPOItmList(RowNo)
    }   
}
function BindPOItmList(ID) {
    debugger;
    var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
    ItemCode = $(this).closest('tr').find("#SPLItemListName" + SNo).val();
    BindPItemList("#SPLItemListName", ID, "#SupplierPriceListTbl", "#SNohiddenfiled", "", "PO");
}
function BindPItemList(ItmDDLName, RowID, TableID, SnoHiddenField, OtherFx, PageName) {
    debugger;
    if ($(ItmDDLName + RowID).val() == "0" || $(ItmDDLName + RowID).val() == "" || $(ItmDDLName + RowID).val() == null) {
        $(ItmDDLName + RowID).append(`<optgroup class='def-cursor' id="Textddl${RowID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        $('#Textddl' + RowID).append(`<option data-uom="0" value="0">---Select---</option>`);
    }
    if ($(TableID + " tbody tr").length > 0) {
        $(TableID + " tbody tr").each(function () {
            var currentRow = $(this);
            var rowno = "";
            if (RowID != null && RowID != "") {
                rowno = currentRow.find(SnoHiddenField).val();
            }
            DynamicSerchableItemDDLPO(TableID, ItmDDLName, rowno, SnoHiddenField, OtherFx, PageName)

        });
    }
    else {
        DynamicSerchableItemDDLPO(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName)
    }
    if (OtherFx == "BindData") {
        BindData();
    }
}
function DynamicSerchableItemDDLPO(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName) {
    $(ItmDDLName + RowID).select2({

        ajax: {
            url: "/Common/Common/GetItemList3",
            data: function (params) {
                var queryParameters = {
                    SearchName: params.term,
                    PageName: PageName,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                var pageSize,
                    pageSize = 20;//50000; // or whatever pagesize

                ConvertIntoArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField);
                var ItemListArrey = [];
                if (sessionStorage.getItem("selecteditemlist").length != null) {
                    ItemListArrey = JSON.parse(sessionStorage.getItem("selecteditemlist"));
                }
                let selected = [];
                selected.push({ id: $(ItmDDLName + RowID).val() });
                selected = JSON.stringify(selected);

                var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));
                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-6 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemType").text()}</div></div>
</strong></li></ul>`)
                }
                var page = params.page || 1;
                // data = data.slice((page - 1) * pageSize, page * pageSize); /*commented by Suraj For creating scroll*/
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0_0", Name: " ---Select---_0" };
                            Fdata.unshift(select);
                        }
                    }
                }
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split("_")[0], text: val.Name.split("_")[0], UOM: val.ID.split("_")[1], type: val.Name.split("_")[1] };
                    }),
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
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.type + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function OnChangePOItemName(SrNo, e, RaplicateFlag) {
    //var clickedrow = $(e.target).closest("tr");

    if (RaplicateFlag == "Raplicate") {
        var clickedrow = e;
    }
    else {
        var clickedrow = $(e.target.closest('tr'));
    }

    var ItemID = clickedrow.find("#hfItemID").val();
    var Itm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    Cmn_DeleteSubItemQtyDetail(ItemID);
    Itm_ID = clickedrow.find("#SPLItemListName" + SNo).val();
    clickedrow.find("#hfItemID").val(Itm_ID);
    if (Itm_ID == "0") {
        clickedrow.find("#SpanSPLItemListName_Error" + SNo).text($("#valueReq").text());
        clickedrow.find("#SpanSPLItemListName_Error" + SNo).css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SPLItemListName" + SNo + "-container']").css("border-color", "red");
    }
    else {
        clickedrow.find("#SpanSPLItemListName_Error" + SNo).css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SPLItemListName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    ClearRowDetails(e, ItemID);
    DisableHeaderField(RaplicateFlag);
    try {
        Cmn_BindUOM(clickedrow, Itm_ID, "", "Y", "pur");
        debugger
    } catch (err) {
    }
}
function ClearRowDetails(e, ItemID) {
    var clickedrow = $(e.target).closest("tr");
    clickedrow.find("#ItmInfoBtnIcon").val("");
    clickedrow.find("#UOM").val("");
    clickedrow.find("#PackSize").val("");
    clickedrow.find("#MRP").val("");
    clickedrow.find("#Price").val("");
    clickedrow.find("#DiscountInPercentage").val("");
    clickedrow.find("#EffectivePrice").val("");
    clickedrow.find("#SubItemAvlQty").val("");
}
function DisableHeaderField(RaplicateFlag) {
    if (RaplicateFlag == "Raplicate") {
        if ($("#SupplierName").val() == "0") {
            $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
            $("#SupplierName").css("border-color", "Red");
            $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
            $("#SpanSuppNameErrorMsg").css("display", "block");
        }
        else {
            $("#SupplierName").attr('disabled', false);
        }
    }
    else {
        if ($("#SupplierName").val() == "0") {
            $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
            $("#SupplierName").css("border-color", "Red");
            $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
            $("#SpanSuppNameErrorMsg").css("display", "block");
        }
        else {
            $("#SupplierName").attr('disabled', true);
        }
    }
}
function InsertSPLDetails() {
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    if (CheckSPL_Validations() == false) {
        return false;
    }
    if (CheckSPL_ItemValidations("N") == false) {
        return false;
    }
    var FinalItemDetail = [];
    FinalItemDetail = InsertDPIItemDetails();
    var str = JSON.stringify(FinalItemDetail);
    $('#ItemDetails').val(str);

    //var FinalItemDetail = [];
    //FinalItemDetail = InsertDPIItemDetails();
    //var str = JSON.stringify(FinalItemDetail);
    //$('#hdItemDetailList').val(str);

    $("#SupplierName").attr('disabled', false);
}
function InsertDPIItemDetails() {
    var PI_ItemsDetail = [];
    $("#SupplierPriceListTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var item_id = currentRow.find("#SPLItemListName" + Sno + " option:selected").val();
        var item_name = currentRow.find("#SPLItemListName" + Sno + " option:selected").text();
        var uom_id = IsNull(currentRow.find("#UOMID").val(), 0);
        var uom_name = IsNull(currentRow.find("#UOM").text(), 0);
        var PackSize = currentRow.find("#PackSize").val();
        var MRP = currentRow.find("#MRP").val();
        var Price = currentRow.find("#Price").val();
        var item_disc_perc = currentRow.find("#DiscountInPercentage").val();
        var EffectivePrice = currentRow.find("#EffectivePrice").val();
        
        PI_ItemsDetail.push({
            item_id: item_id, item_name: item_name
            , uom_id: uom_id, uom_name: uom_name, PackSize: PackSize, MRP: MRP, Price: Price, item_disc_perc: item_disc_perc, EffectivePrice: EffectivePrice
        });
    });
    return PI_ItemsDetail;
}
function CheckSPL_Validations() {
    var ErrorFlag = "N";
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SupplierName").css("border-color", "Red");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
        $("#SupplierName").css("border-color", "#ced4da");
    }
    if ($("#ValidUpto").val() == "") {
        $('#SpanValidUptoErrorMsg').text($("#valueReq").text());
        $("#SpanValidUptoErrorMsg").css("display", "block");
        $("#ValidUpto").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanValidUptoErrorMsg").css("display", "none");
        $("#ValidUpto").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    } else {   
        return true;
    }
}
function OnchangeValidUpto() {
    if ($("#ValidUpto").val() == "") {
        $('#SpanValidUptoErrorMsg').text($("#valueReq").text());
        $("#SpanValidUptoErrorMsg").css("display", "block");
        $("#ValidUpto").css("border-color", "Red");
    }
    else {
        $("#SpanValidUptoErrorMsg").css("display", "none");
        $("#ValidUpto").css("border-color", "#ced4da");
    }
}
function CheckSPL_ItemValidations() {
    var ErrorFlag = "N";
    if ($("#SupplierPriceListTbl >tbody >tr").length > 0) {
        $("#SupplierPriceListTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var itemName = currentRow.find("#SPLItemListName" + Sno).val();
            if (itemName == "" || itemName == null || itemName == "0") {
                currentRow.find("#SpanSPLItemListName_Error" + Sno).text($("#valueReq").text());
                currentRow.find("#SpanSPLItemListName_Error" + Sno).css("display", "block");
                currentRow.find("[aria-labelledby='select2-SPLItemListName" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#SpanSPLItemListName_Error" + Sno).css("display", "none");
                currentRow.find("[aria-labelledby='select2-SPLItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
            }
            if (parseFloat(CheckNullNumber(currentRow.find("#Price").val())) == 0) {
                currentRow.find("#Span_Price").text($("#valueReq").text());
                currentRow.find("#Span_Price").css("display", "block");
                currentRow.find("#Price").css("border-color", "red").focus();
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#Span_Price").css("display", "none");
                currentRow.find("#Price").css("border-color", "#ced4da");
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
function onchangeMRP(e) {
    var currentRow = $(e.target).closest("tr");
    var MRP = currentRow.find("#MRP").val();
    var Price = currentRow.find("#Price").val();
    //if (parseFloat(CheckNullNumber(Price)) > parseFloat(CheckNullNumber(MRP))) {
        //$("#Price").css("border-color", "Red");
        //$('#Span_Price').text($("#ExceedingAmount").text());
        //$("#Span_Price").css("display", "block");
    //}
    //else {
        currentRow.find("#Span_Price").css("display", "none");
        currentRow.find("#Price").css("border-color", "#ced4da");
        currentRow.find("#MRP").val(parseFloat(MRP).toFixed(RateDecDigit))
    //}
}
function onchangePrice(e) {
    var currentRow = $(e.target).closest("tr");
    var MRP = currentRow.find("#MRP").val();
    var Price = currentRow.find("#Price").val();
    var DiscountInPercentage = currentRow.find("#DiscountInPercentage").val();
    //if (parseFloat(CheckNullNumber(Price)) > parseFloat(CheckNullNumber(MRP))) {
    //    $("#Price").css("border-color", "Red");
    //    $('#Span_Price').text($("#ExceedingAmount").text());
    //    $("#Span_Price").css("display", "block");
    //}
    //else {
        currentRow.find("#Span_Price").css("display", "none");
        currentRow.find("#Price").css("border-color", "#ced4da");
        currentRow.find("#Price").val(parseFloat(Price).toFixed(RateDecDigit))
    //}
    if (parseFloat(DiscountInPercentage) > 0) {
        var disAmt = parseFloat(Price) * parseFloat(DiscountInPercentage) / 100;
        var effAmt = parseFloat(Price) - parseFloat(disAmt);
        currentRow.find("#EffectivePrice").val(parseFloat(effAmt).toFixed(RateDecDigit));
    }
    else {
        currentRow.find("#EffectivePrice").val(parseFloat(Price).toFixed(RateDecDigit));
    }
}
function onchangeDiscountInPercentage(e) {
    var currentRow = $(e.target).closest("tr");
    var MRP = currentRow.find("#MRP").val();
    var Price = currentRow.find("#Price").val();
    var DiscountInPercentage = currentRow.find("#DiscountInPercentage").val();

    if (parseFloat(DiscountInPercentage) > 0) {
        var disAmt = parseFloat(Price) * parseFloat(DiscountInPercentage) / 100;
        var effAmt = parseFloat(Price) - parseFloat(disAmt);
        currentRow.find("#EffectivePrice").val(parseFloat(effAmt).toFixed(RateDecDigit));

        currentRow.find("#DiscountInPercentage").val(parseFloat(DiscountInPercentage).toFixed(cmn_PerDecDigit))
    }
}
function onclickRevisionHistory(e) {
    var currentRow = $(e.target).closest("tr");
    var Sno = currentRow.find("#SNohiddenfiled").val();
    var itemName = currentRow.find("#SPLItemListName" + Sno + " option:selected").text();
    var ProductId = currentRow.find("#SPLItemListName" + Sno ).val();
    var uom_name = IsNull(currentRow.find("#UOM").val(), 0);
    var supp_id = $("#hdnSuppId").val();
    
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SupplierPriceList/GetRevPriceHistoryDetails",
        data: {
            supp_id: supp_id,
            Item_id: ProductId,
        },
        success: function (data) {
            debugger;
            $("#HistoryRev_popup").html(data);
            $("#PPItemName").val(itemName);
            $("#PPUOM").val(uom_name);
        }
    });
}
function OnClickIconBtn(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    var Sno = currentRow.find("#SNohiddenfiled").val();
    var ItmCode = currentRow.find("#SPLItemListName" + Sno).val();
    ItemInfoBtnClick(ItmCode);
}
function ForwardBtnClick() {
    debugger;
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var DrPinvDate = $("#Create_DT").val();
        var datePart = DrPinvDate.split(" ")[0]; // "15-12-2025"
        var parts = datePart.split("-");
        var formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
        $.ajax({
            type: "POST",
            /*  url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: formattedDate
            },
            success: function (data) {
                /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var INVStatus = "";
                    INVStatus = $('#hfPIStatus').val().trim();
                    if (INVStatus === "D" || INVStatus === "F") {

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
                    /*   swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                    /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
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
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var supp_id = "";
    var Create_date = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var VoucherNarr = $("#hdn_Nurration").val()
    var BP_VoucherNarr = $("#hdn_BP_Nurration").val()
    var DN_VoucherNarr = $("#hdn_DN_Nurration").val()
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    supp_id = $("#hdnSuppId").val();
    Create_date1 = $("#Create_DT").val();
    var datePart = Create_date1.split(" ")[0]; // "15-12-2025"
    var parts = datePart.split("-");
    Create_date = parts[2] + "-" + parts[1] + "-" + parts[0];
    WF_Status1 = $("#WF_Status1").val();
    //var TrancType = (docid + ',' + INV_NO + ',' + INVDate + ',' + WF_Status1)
    var TrancType = (supp_id + ',' + Create_date + ',' + "Update" + ',' + WF_Status1 + ',' + docid)
    $("#hdDoc_No").val(supp_id);
    Remarks = $("#fw_remarks").val();
    var FilterData = $("#FilterData1").val();
    var ListFilterData1 = $("#ListFilterData1").val();

    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = supp_id.replace(/\//g, "") + "_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        //var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(INV_NO, INVDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/DirectPurchaseInvoice/SavePdfDocToSendOnEmailAlert");
        //if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        //}
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && supp_id != "" && Create_date != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(supp_id, Create_date, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/SupplierPriceList/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
            //showLoader();
            //window.location.reload();
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/SupplierPriceList/ApproveSPLDetails?supp_id=" + supp_id + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&FilterData=" + FilterData + "&docid=" + docid + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && supp_id != "" && Create_date != "") {
            await Cmn_InsertDocument_ForwardedDetail1(supp_id, Create_date, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/SupplierPriceList/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && supp_id != "" && Create_date != "") {
            await Cmn_InsertDocument_ForwardedDetail1(supp_id, Create_date, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/SupplierPriceList/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
            //showLoader();
            //window.location.reload();
        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#hdnSuppId").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        var Doc_Status = $("#hfPIStatus").val();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    Cmn_GetForwarderHistoryList(Doc_No, Doc_ID, Doc_Status);
    return false;
}
function ReplicateWith() {
    debugger;
    $("#ddlReplicateWith").append("<option value='0'>---Select---</option>");
        $("#ddlReplicateWith").select2({
            ajax: {
                url: "/ApplicationLayer/SupplierPriceList/BindReplicateWithlist",
                data: function (params) {
                    var queryParameters = {
                        Search: params.term,
                        page: params.page || 1
                    };
                    return queryParameters;
                },
                multiple: true,
                cache: true,
                processResults: function (data, params) {
                    debugger
                    var pageSize, OnChanngReplicateWith
                        pageSize = 2000; // or whatever pagesize

                    if ($(".select2-search__field").parent().find("ul").length == 0) {
                        $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                    <div class="row">
                    <div class="col-md-12 col-xs-12 def-cursor">${$("#span_SupplierName").text()}</div>
                    </div>
                    </strong></li></ul>`)
                    }
                    var page = 1;
                    data = data.slice((page - 1) * pageSize, page * pageSize)
                    debugger;
                    return {
                        results: $.map(data, function (val, Item) {
                            return { id: val.Name, text: val.ID };
                        }),
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
                    '<div class="col-md-12 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                    '</div>'
                );
                return $result;
                firstEmptySelect = false;
            },
        });
}
function OnChanngReplicateWith() {
    debugger;
    $("#tbl_AlternateItem > tbody > tr").remove();
    $('#tbladd >tbody >tr').remove();
    var supp_id = $("#ddlReplicateWith").val();
    $('#SupplierPriceListTbl >tbody >tr').remove();
    $("#ddlReplicateWith").attr("disabled", true);
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/SupplierPriceList/GetReplicateWithSUpplierPriceList",
            data: {
                supp_id: supp_id
            },
            success: function (data) {
                debugger;
                if (data !== null && data !== "" && data != "ErrorPage") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            for (var i = 0; i < arr.Table.length; i++) {
                                var item_id = arr.Table[i].item_id;
                                var item_name = arr.Table[i].item_name;
                                var uom_name = arr.Table[i].uom_alias;
                                var uom_id = arr.Table[i].uom_id;
                                var packSize = arr.Table[i].pack_size;
                                var mrp = arr.Table[i].mrp;
                                var price = arr.Table[i].price;
                                var DiscountInPercentage = arr.Table[i].DiscountInPercentage;
                                var effactivePrice = arr.Table[i].effactivePrice;
                                onchangereplicate_item(item_id, item_name,uom_id, uom_name, packSize, mrp, price, DiscountInPercentage, effactivePrice);
                            }
                        }
                        $("#SupplierPriceListTbl tbody tr").each(function () {
                            var currentRow = $(this);
                            OnChangePOItemName("1", currentRow, "Raplicate")
                        });
                    }
                    if (arr.Table1.length > 0) {
                        for (var y = 0; y < arr.Table1.length; y++) {

                            var supp_id = arr.Table1[y].supp_id;
                            var supp_name = arr.Table1[y].supp_name;
                            var supp_hdn_id = arr.Table1[y].supp_hdn_id;
                            var supp_catg = arr.Table1[y].supp_catg;
                            var supp_cat_name = arr.Table1[y].supp_cat_name;
                            var supp_port = arr.Table1[y].supp_port;
                            var supp_port_name = arr.Table1[y].supp_port_name;
                            var valid_upto = arr.Table1[y].valid_upto;

                            //$("#hdnSuppId").val(supp_id);
                            //$("#Category").val(supp_cat_name);
                            //$("#HdnCategory").val(supp_catg);

                            //$("#Portfolio").val(supp_port_name);
                            //$("#HdnPortfolio").val(supp_port);
                            //$("#ValidUpto").val(valid_upto);

                            //$('#SupplierName').append(`<option value="${supp_hdn_id}">${supp_name}</option>`);
                            //$("#SupplierName").val(supp_hdn_id).trigger('change');

                            
                        }
                    }
                }
                else {
                    $("#txt_Quantity").val("");
                }
            }
        })
}
function onchangereplicate_item(item_id, item_name, uom_id, uom_name, packSize, mrp, price, DiscountInPercentage,effactivePrice) {
    debugger;
    var rowIdx = 0;
    debugger;
    var rowCount = $('#SupplierPriceListTbl >tbody >tr').length + 1
    var RowNo = 0;
    $("#SupplierPriceListTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#srno").text()) + 1;
    });
    if (RowNo == 0) {
        RowNo = 1;
    }
    $('#SupplierPriceListTbl tbody').append(`<tr id="R${RowNo}">
                                                <td class=" red center">
                                                    <i class="deleteIcon fa fa-trash" aria-hidden="true" title=${$("#Span_Delete_Title").text()}></i>
                                                </td>
                                                
                                                    <td class="sr_padding" id="srno">${RowNo}</td>
                                                    
                                                <td> 
                                                 <div class="lpo_form">
                                                    <div class="col-sm-11 no-padding">
                                                       <select class="form-control" id="SPLItemListName${RowNo}" name="SPLItemListName${RowNo}" onchange="OnChangePOItemName(${RowNo},event)" ><option value="${item_id}">${item_name}</option></select>
                                                    </div>
                                                    <div class="col-sm-1 i_Icon">
                                                        <button type="button" class="calculator subItmImg" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" onclick="OnClickIconBtn(event);" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").val()}">  </button>
                                                    </div>
                                                    <span id="SpanSPLItemListName_Error${RowNo}" class="error-message is-visible"></span>
                                                    <input class="" type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
</div>
                                                </td>
                                                <td><input id="UOM" class="form-control" autocomplete="off" value="${uom_name}" type="text" name="UOM" placeholder=${$("#ItemUOM").text()} readonly="readonly">
                                                    <input type="hidden" id="UOMID" value="${uom_id}">
                                                </td>
                                                <td> <input id="PackSize" autocomplete="off" class="form-control" value="${packSize}" name="PackSize" placeholder=${$("#span_PackSize").text()}></td>
                                                <td><input id="MRP" autocomplete="off" onchange="onchangeMRP(event)" value="${mrp}" class="form-control num_right" name="MRP" placeholder="0000.00"></td>
                                                <td>
                                                    <div class="lpo_form">
                                                        <input id="Price" autocomplete="off" onchange="onchangePrice(event)" value="${price}" class="form-control num_right" name="Price" placeholder="0000.00">
                                                        <span id="Span_Price" class="error-message is-visible"></span>
                                                    </div>
                                                </td>
                                                <td><input id="DiscountInPercentage" autocomplete="off" value="${DiscountInPercentage}" onkeypress="return FloatValuePerOnly(this,event);" onchange="onchangeDiscountInPercentage(event)" class="form-control num_right" name="DiscountInPercentage" placeholder="0000.00"></td>
                                                <td><input id="EffectivePrice" class="form-control num_right" value="${effactivePrice}" name="EffectivePrice" placeholder="0000.00" disabled="disabled"></td>
                                                <td class="center">
                                                    <button type="button" id="SubItemAvlQty" class="calculator subItmImg" data-toggle="modal" onclick="" data-target="#RevisionHistory" data-backdrop="static" data-keyboard="false"> <i class="fa fa-refresh ref" id="" onclick="" title="@Resource.Revision @Resource.History "></i> </button>
                                                </td>
    </tr>`)
    BindPOItmList(RowNo)
}
function OtherFunctions(StatusC, StatusName) {

}
function FloatValuePerOnly(el, evt) {
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    } else {
        return true;
    }
}
