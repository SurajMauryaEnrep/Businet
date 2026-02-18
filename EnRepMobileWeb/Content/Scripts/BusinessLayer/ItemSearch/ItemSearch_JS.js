$(document).ready(function () {
    debugger;
  // BindProductNameDDL();
  
    DynamicSerchableItemDDL("", "#itemNameList", "", "", "", "ItemSearch")


    

});
function PrintInvoice() {
         var itemId = $("#itemID").val();
    var PrintItemName = $("#HdnPrintItemName").val();
    var ShowUOM = $("#hdn_ShowUOM").val();
    var ShowOEMNumber = $("#Hdn_ShowOEMNumber").val();
    var ShowItemAliasName = $("#Hdn_ShowItemAliasName").val();
    var ShowRefNumber = $("#Hdn_ShowRefNumber").val();
    var ShowProdTechSpec = $("#Hdn_ShowProdTechSpec").val();
    var ProdTechDesc = $("#ShowProdTechDesc").val();
    var ItemGroupName = $("#HdnPrintItemGroupName").val();
    var HsnNumber = $("#HdnPrintHsnNumber").val();
    var Remarks = $("#HdnPrintRemarks").val();
    var WeightinKG = $("#HdnPrintWeightinKG").val();
    var Volumeinliter = $("#HdnPrintVolumeinliter").val();
    var grossweight = $("#HdnPrintgrossweight").val();
    var Netweight = $("#HdnPrintNetweight").val();  
    var PrintItemImage = $("#HdnPrintItemImage").val();
        

        var queryParams = $.param({
            itemId: itemId,
            PrintItemName: PrintItemName,
            ShowUOM: ShowUOM,
            ShowOEMNumber: ShowOEMNumber,
            ShowItemAliasName: ShowItemAliasName,
            ShowRefNumber: ShowRefNumber,
            ShowProdTechSpec: ShowProdTechSpec,
            ProdTechDesc: ProdTechDesc,
            ItemGroupName: ItemGroupName,
            HsnNumber: HsnNumber,
            Remarks: Remarks,
            WeightinKG: WeightinKG,
            Volumeinliter: Volumeinliter,
            grossweight: grossweight,
            Netweight: Netweight,
            PrintItemImage: PrintItemImage,
        });

        window.location.href = `/BusinessLayer/ItemSearch/GetitemsearchGenratePdfFile?${queryParams}`;
}
$(document).keydown(function (e) {
    // Check for Ctrl (e.ctrlKey), Shift (e.shiftKey) and R key (e.keyCode === 82)
    if (e.ctrlKey && e.shiftKey && e.keyCode === 82) {
        window.location.href = "/BusinessLayer/ItemSearch/ItemSearch";
    }
});
function itemSearchPartial() {
    $("#spanItemName").text("");
    $("#spanItemName").css("display", "none");
    $("#SearchItemNameText").css("border-color", "#ced4da");
    $("#SearchItemNameID").val("");
    $("#SearchItemNameText").val("");

    $("#spanItemName1").text("");
    $("#spanItemName1").css("display", "none");
}

async function NullValueAllField() {
  //  $("#itemNameList").val("0").trigger('change');
    $("#ItemNametext").text(null);
    $("#ItemNametext").val("");
    $("#itemID").val("");
    $("#ItemUOMtext").text(null);
    $("#ItemUOMtext").val("");
    $("#ItemSearchOEM").val("");
    $("#AliasName").val("");
    $("#ReferenceNumber").val("");
    $("#TechnicalSpecification").val("");
    $("#TechnicalDescription").val("");
    $("#GroupName").val("");
    $("#HSNCode").val("");
    $("#remarks").val("");
    $("#WeightInKg").val("");
    $("#VolumeInLitres").val("");
    $("#GrossWeight").val("");
    $("#NetWeight").val("");
    $("#act_stats").prop("checked", false);
    $("#hdn_attatchment_list tbody tr").remove();

    await $.ajax({
        url: "/BusinessLayer/ItemSearch/PartialAttachmentDetail",
        type: 'GET',
        success: function (data) {
            $('#Itm_Details_List').html(data);
        },
        error: function (xhr, status, error) {
            console.error("Error loading partial view: " + error);
        }
    })
};
   

function OnchangeItemName() {
    debugger;
    var ItemID = "";
    $("#SpanSerchItemID").css("display", "none");
    $("#itemNameList").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-itemNameList-container']").css("border-color", "#ced4da");
    var hf_ItemID = $("#hf_ItemID").val();
    ItemID = $("#itemNameList").val();
    if (ItemID != null && ItemID != "" && ItemID != "0") {
        $("#hf_ItemID").val(ItemID);
    }
    else {
        ItemID = $("#itemNameList").val(hf_ItemID);
      
    }
   

}
function GetItemDeatilData() {
    debugger;
    // Retrieve the ItemID from the hidden field
    var ItemID = $("#hf_ItemID").val();

    // Check if the ItemID is valid
    if (ItemID !== "" && ItemID !== "0") {
        // Hide the error message and reset borders if ItemID is valid
        $("#SpanSerchItemID").css("display", "none");
        $("#itemNameList").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-itemNameList-container']").css("border-color", "#ced4da");
      
       window.location.href = "/BusinessLayer/ItemSearch/GetItemListFilter/?ItemID=" + encodeURIComponent(ItemID);
    } else {
        // If ItemID is invalid, show the error message and highlight the border
        var Error_msg = $("#valueReq").text();
        $("#SpanSerchItemID").text(Error_msg);
        $("#SpanSerchItemID").css("display", "block");
        $("[aria-labelledby='select2-itemNameList-container']").css("border-color", "Red");

        // Optionally, focus the dropdown for better UX
        $("#itemNameList").focus();
    }
}
function OnCheckedShowItemName() {
    debugger;
    if ($('#chkItemName').prop('checked')) {
        $('#HdnPrintItemName').val('Y');
    }
    else {
        $('#HdnPrintItemName').val('N');
    }
}
function OnCheckedChangeUOM() {
    debugger
    if ($('#chkshowUOM').prop('checked')) {
        $('#hdn_ShowUOM').val('Y');
    }
    else {
        $('#hdn_ShowUOM').val('N');
    }
}
function OnCheckedChangeOEM() {
    if ($('#chkshowOEM').prop('checked')) {
        $('#Hdn_ShowOEMNumber').val('Y');
    }
    else {
        $('#Hdn_ShowOEMNumber').val('N');
    }
}
function OnCheckedChangeItmAliasName() {
    debugger;
    if ($('#chkshowItemAliasName').prop('checked')) {
        $('#Hdn_ShowItemAliasName').val('Y');
    }
    else {
        $('#Hdn_ShowItemAliasName').val('N');
    }
}
function OnCheckedChangeRefNum() {
    debugger;
    if ($('#chkshowRefNum').prop('checked')) {
        $('#Hdn_ShowRefNumber').val('Y');
    }
    else {
        $('#Hdn_ShowRefNumber').val('N');
    }
}
function OnCheckedChangeItmTechSpecific() {
    debugger;
    if ($('#chkshowItmTechSpecific').prop('checked')) {
        $('#Hdn_ShowProdTechSpec').val('Y');
    }
    else {
        $('#Hdn_ShowProdTechSpec').val('N');
    }
}
function OnCheckedChangeProdTechDesc() {
    if ($('#chkprodtechdesc').prop('checked')) {
        $('#ShowProdTechDesc').val('Y');
    }
    else {
        $('#ShowProdTechDesc').val('N');
    }
}
function OnCheckedChangegroupname() {
    debugger
    if ($('#groupname').prop('checked')) {
        //$('#chkshowcustspecproddesc').prop('checked', false);
        //$('#chkitmaliasname').prop('checked', false);/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
        $("#HdnPrintItemGroupName").val("Y");
        //$('#ShowCustSpecProdDesc').val('N');
        //$('#ItemAliasName').val('N');/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
    }
    else {
        $("#HdnPrintItemGroupName").val("N");
    }
}
function OnCheckedChangeHSNNumber() {
    debugger
    if ($('#chkHSNNumber').prop('checked')) {
        $("#HdnPrintHsnNumber").val("Y");
    }
    else {
        $("#HdnPrintHsnNumber").val("N");
    }
}
function OnCheckedChangeHSNNumber() {
    debugger
    if ($('#chkHSNNumber').prop('checked')) {
        $("#HdnPrintHsnNumber").val("Y");
    }
    else {
        $("#HdnPrintHsnNumber").val("N");
    }
}
function OnCheckedChangePrintRemarks() {
    debugger
    if ($('#chkprintremarks').prop('checked')) {
        $("#HdnPrintRemarks").val("Y");
    }
    else {
        $("#HdnPrintRemarks").val("N");
    }
}
function OnCheckedChangeWeightInKg() {
    debugger
    if ($('#chkWeightInKg').prop('checked')) {
        $("#HdnPrintWeightinKG").val("Y");
    }
    else {
        $("#HdnPrintWeightinKG").val("N");
    }
}
function OnCheckedChangeVolumeInLitres() {
    debugger
    if ($('#chkVolumeInLitres').prop('checked')) {
        $("#HdnPrintVolumeinliter").val("Y");
    }
    else {
        $("#HdnPrintVolumeinliter").val("N");
    }
}
function OnCheckedChangeGrossWeight() {
    debugger
    if ($('#chkGrossWeight').prop('checked')) {
        $("#HdnPrintgrossweight").val("Y");
    }
    else {
        $("#HdnPrintgrossweight").val("N");
    }
}
function OnCheckedChangeNetWeight() {
    debugger
    if ($('#chkNetWeighte').prop('checked')) {
        $("#HdnPrintNetweight").val("Y");
    }
    else {
        $("#HdnPrintNetweight").val("N");
    }
}

function OnCheckedChangePrintItemImage() {
    debugger;
    if ($('#chkshowItemImage').prop('checked')) {
        $('#HdnPrintItemImage').val('Y');
    }
    else {
        $('#HdnPrintItemImage').val('N');
    }
}


