$(document).ready(function () {
    debugger;
    
    BindDdlUomName();
    $("#ddlItemName").select2({
        ajax: {
            url: "/BusinessLayer/UnitOfMeasure/GetItemsList",
            data: function (params) {
                var query = {
                    SearchName: params.term,
                    Page: params.page
                }
                return query;
            },
            cache: true,
            delay: 250,
            processResults: function (data, params) {
                debugger;
                var pageSize,
                    pageSize = 20;//50000; // or whatever pagesize
                var arr = JSON.parse(data);

                data = arr.Table;
                var page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.Item_id, text: val.Item_name };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
            }

        },
    });
});
function BindDdlUomName() {
    $("#ddlUomName").select2({
        ajax: {
            url: "/BusinessLayer/UnitOfMeasure/GetUomNameDict",
            data: function (params) {
                var query = {
                    SearchValue: params.term,
                    Page: params.page
                }
                return query;
            },
            cache: true,
            delay: 250,
            processResults: function (data) {
                
                // Transforms the top-level key of the response object from 'items' to 'results'
                if ($("#ddlItemName").val() != '' && $("#ddlItemName").val() != '0') {
                    let arr = [];
                    var UOMID = $("#UOMID").val();
                    arr.push(UOMID);
                    var ItemId = $("#ddlItemName").val();
                    $("#datatable-buttons1 >tbody >tr #Conv_item_id:contains(" + ItemId + ")").parent().each(function () {

                        var currentRow = $(this);
                        var Conv_item_id = currentRow.find("#Conv_item_id").text();
                        if (Conv_item_id == ItemId) {
                            var Conv_uom_id = currentRow.find("#Conv_uom_id").text();
                            arr.push(Conv_uom_id);
                        }
                    });

                    data = data.filter(v => !arr.includes(v.ID));
                } else {
                    data = data.filter(v => v.ID == '0');
                }
                
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }

        },
    });
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    return true;
}
function OnChangeConvRate() {
    debugger;
    var convRate = $("#ConversionRate").val()
    if (parseFloat(convRate) == 0 || convRate == "") {
        $('#vmConversionRate').text($("#valueReq").text());
        $("#vmConversionRate").css("display", "block");
        $("#ConversionRate").css("border-color", "red");
        $("#ConversionRate").val(parseFloat(CheckNullNumber(convRate)).toFixed($("#RateDigit").text()))
    }
    else {
        $("#vmConversionRate").css("display", "none");
        $("#ConversionRate").css("border-color", "#ced4da");
        $("#ConversionRate").val(parseFloat(convRate).toFixed($("#RateDigit").text()))
    }   
}
function SaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickconversion") {
        $("#SaveConversion").css("filter", "grayscale(100%)");
        $("#SaveConversion").prop("disabled", true);
        return false;
    }
    var Flag = 'N';
    var convRate = $("#ConversionRate").val();
    var ItemId = $("#ddlItemName").val();
    if (ItemId == "0" || ItemId == "") {
        $('#vmddlItemName').text($("#valueReq").text());
        $("#vmddlItemName").css("display", "block");
        $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "red");
        Flag = 'Y';
    }
    var UomId = $("#ddlUomName option:selected").val();
    if (UomId == "0" || UomId == "") {
        $('#vmddlUomName').text($("#valueReq").text());
        $("#vmddlUomName").css("display", "block");
        $("[aria-labelledby='select2-ddlUomName-container']").css("border-color", "red");
        Flag = 'Y';
    }
    if (parseFloat(convRate) == "0" || convRate == "") {
        $('#vmConversionRate').text($("#valueReq").text());
        $("#vmConversionRate").css("display", "block");
        $("#ConversionRate").css("border-color", "red");
        Flag = 'Y';
    }
    if (Flag == "Y") {
        return false;
    }
    else {
        $("#ddlItemName").attr("disabled", false);
        $("#ddlUomName").attr("disabled", false);
        $("#SaveConversion").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickconversion");
        return true;
    }
}
function UOMSetupSaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickUom") {
        $("#btn_saveUOM").css("filter", "grayscale(100%)");
        $("#btn_saveUOM").prop("disabled", true);
        return false;
    }
    var flag = 'N';
    if (CheckVallidation("uom_name", "vmuom_name") == false ) {
        flag = 'Y';
    }
    if (CheckVallidation("uom_alias", "vmuom_alias") == false) {
        flag = 'Y';
    }
    if (flag == 'Y') {
        return false;
    }
    else {
        $("#btn_saveUOM").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickUom");
        return true;
    }
}
function EditUOM(e) {
    debugger;
    var CurrentRow = $(e.target).closest('tr');
    var uom_id = CurrentRow.find("#HdnUOM_Id").val();
    var uom_name = CurrentRow.find("#UOMname2").text();
    var uom_alias = CurrentRow.find("#UOMname3").text();

    $("#hdnuomID").val(uom_id)
    $("#uom_name").val(uom_name).trigger('change');
    $("#uom_alias").val(uom_alias).trigger('change');
    $("#btn_saveUOM").val("Update");
    $("#hdnSavebtn").val("");

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnable").css("display", "block");
        $("#uom_name").attr("disabled", false)
        $("#uom_alias").attr("disabled", false)
    }
}
function EditConversionUOM(e) {
    debugger;
    $("#vmConversionRate").css("display", "none");
    $("#ConversionRate").css("border-color", "#ced4da");

    var CurrentRow = $(e.target).closest('tr');
    var Conv_item_name = CurrentRow.find("#Conv_item_name").text();
    var Conv_item_id = CurrentRow.find("#Conv_item_id").text();
    var Conv_uom_name = CurrentRow.find("#Conv_uom_name").text();
    var ConvUomId = CurrentRow.find("#ConvUomId").text();
    var Conv_uom = CurrentRow.find("#Conv_uom").text();
    var Conv_uom_id = CurrentRow.find("#Conv_uom_id").text();
    var Conv_rate = CurrentRow.find("#Conv_rate").text();
    var show_stk = CurrentRow.find("#show_stk").text();

    $("#UomID").val(ConvUomId)
    $("#UOMID").val(ConvUomId)
    $("#UOM").val(Conv_uom_name);
    //$("#ddlItemName").val(Conv_item_id).trigger('change');
    $("#ddlItemName").html(`<option value=${Conv_item_id}>${Conv_item_name}</option>`);
    $("#ddlUomName").html(`<option value=${Conv_uom_id}>${Conv_uom}</option>`);
    //$("#ddlUomName").val(Conv_uom_id).trigger('change');
    $("#ConversionRate").val(parseFloat(Conv_rate).toFixed($("#RateDigit").text()));

    if (show_stk == "Y") {
        $("#ShowStock").prop("checked", true);
    }
    else {
        $("#ShowStock").prop("checked", false);
    }

    $("#SaveConversion").val("UpdateConversion");

    $("#ddlItemName").attr("disabled", true);
    $("#ddlUomName").attr("disabled", true);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#ConvSaveBtnEnable").css("display", "block");
        $("#ConversionRate").attr("disabled", false)
    }
}
function OnChangeUOM() {
    CheckVallidation("uom_name", "vmuom_name")
}
function OnChangeUOM_alias() {
    CheckVallidation("uom_alias", "vmuom_alias")
}
function OnchangeProductName() {
    $("#vmddlItemName").css("display", "none");
    $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "#ced4da");
    var ddlItemName = $('#ddlItemName').val();
    if (ddlItemName != "0") {
        Cmn_BindUOM(null, ddlItemName, "UOM", "", "Pur")
    }
}
function onchangeddlUom() {
    $("#vmddlUomName").css("display", "none");
    $("[aria-labelledby='select2-ddlUomName-container']").css("border-color", "#ced4da");
}
function functionConfirm(e) {
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
            $("#hdnAction").val("Delete");
            var TrancType = $("#hdnAction").val()
            debugger
            var currentRow = $(e.target).closest('tr');
            var uom_id = currentRow.find("#HdnUOM_Id").val();
            var Conv_item_id = "";
            var conv_uom_id = "";

            window.location.href = "/BusinessLayer/UnitOfMeasure/deleteUOM/?uom_id=" + uom_id + "&TrancType=" + TrancType + "&Conv_item_id=" + Conv_item_id + "&conv_uom_id=" + conv_uom_id;
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function functionConfirmConversionTable(e) {
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
            debugger;
            $("#hdnAction").val("DeleteConversionTable");
            var TrancType = $("#hdnAction").val()
            debugger
            var currentRow = $(e.target).closest('tr');
            var uom_id = currentRow.find("#ConvUomId").text();
            var Conv_item_id = currentRow.find("#Conv_item_id").text();
            var conv_uom_id = currentRow.find("#Conv_uom_id").text();

            window.location.href = "/BusinessLayer/UnitOfMeasure/deleteUOM/?uom_id=" + uom_id + "&TrancType=" + TrancType + "&Conv_item_id=" + Conv_item_id + "&conv_uom_id=" + conv_uom_id;
            return true;
        } else {
            return false;
        }
    });
    return false;
}