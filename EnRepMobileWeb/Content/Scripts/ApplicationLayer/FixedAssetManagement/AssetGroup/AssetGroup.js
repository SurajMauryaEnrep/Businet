const QtyDecDigit = $("#QtyDigit").text();///Quantity
const ValDecDigit = $("#ValDigit").text();///Amount
$(document).ready(function () {
    BindSearchableDDLListData();
    // DefaultItemGroupSetup();
    BindSearchableDDL();
    $(this).on('click', '.simpleTree-label', function (e) {
        var GrpID = this.nextSibling.innerText;
        var Parent = this.innerText;
        GetMenuData(GrpID)
    });
});

function GetAllItemGrp() {
    debugger;
    var RequestedUrl = window.location.protocol + "//" + window.location.host + "/ApplicationLayer/FixedAssetManagement/AssetGroup/GetAllItemGrp";
    $.ajax({
        type: 'POST',
        url: RequestedUrl,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify(),
        success: function (Objdata) {
            debugger;

            if (Objdata !== null && Objdata !== "") {
                var arr = [];
                arr = JSON.parse(Objdata);

                FinalData = []
                $.each(arr, function (i, n) {
                    FinalData.push(n);
                });

                debugger;

                var options = {
                    // Optionally provide here the jQuery element that you use as the search box for filtering the tree. simpleTree then takes control over the provided box, handling user input
                    searchBox: $('#tree_menu'),

                    // Search starts after at least 3 characters are entered in the search box
                    searchMinInputLength: 1,

                    // Number of pixels to indent each additional nesting level
                    indentSize: 25,

                    // Show child count badges?
                    childCountShow: true,

                    // Symbols for expanded and collapsed nodes that have child nodes
                    symbols: {
                        collapsed: '▶',
                        expanded: '▼'
                    },

                    // these are the CSS class names used on various occasions. If you change these names, you also need to provide the corresponding CSS class
                    css: {
                        childrenContainer: 'simpleTree-childrenContainer',
                        highlight: 'simpleTree-highlight',
                        indent: 'simpleTree-indent',
                        label: 'simpleTree-label',
                        mainContainer: 'simpleTree-mainContainer',
                        nodeContainer: 'simpleTree-nodeContainer',
                        selected: 'simpleTree-selected',
                        toggle: 'simpleTree-toggle'
                    },
                };
                debugger;
                $('#mytree').simpleTree(options, FinalData);
            }

        }

    });
}
function GetMenuData(item_grp_id) {
    debugger;
    // window.location.href = "/ApplicationLayer/AssetGroup/ItemGroupSetupView/?ItemGrpId=" + item_grp_id;
    $("#hdnAssetSelectedGrpId").val(item_grp_id);
    loadFormData(item_grp_id);
}

function loadFormData(itemGroupId) {
    debugger;
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetGroup/UpdateAssetGroupHeaderAndDetail?ItemGrpId=" + itemGroupId,
            data: { AssetGroupId: itemGroupId },
            dataType: "json",
            success: function (response) {
                debugger;
                if (response.success) {
                    debugger;
                    $('#AGHeaderContainer').html(response.HeaderHtmlResponse);
                    $('#AGDetailsContainer').html(response.DetailsHtmlResponse);
                } else {
                    alert(response.message || "No data found");
                }
            },
            error: function (xhr, status, error) {
                console.error("Error loading form data:", xhr.responseText);
            }
        });
    } catch (err) {
        console.error("LoadFormDataError: " + err.message);
    }
}
function FloatValuePerOnly(el, evt) {
    $("#SpanTaxPercent").css("display", "none");
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    } else {
        return true;
    }
}
function OnClickEdit() {
    var itemGroupId = $("#hdnAssetSelectedGrpId").val();
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetGroup/EditAssetGroupHeaderAndDetail",
            data: { AssetGroupId: itemGroupId },
            dataType: "json",
            success: function (response) {
                debugger;
                if (response.success) {
                    debugger;
                    $('#AGHeaderContainer').html(response.HeaderHtmlResponse);
                    $('#AGDetailsContainer').html(response.DetailsHtmlResponse);
                    BindSearchableDDL();
                } else {
                    alert(response.message || "No data found");
                }
            },
            error: function (xhr, status, error) {
                console.error("Error Editing form data:", xhr.responseText);
            }
        });
    } catch (err) {
        console.error("EditFormDataError: " + err.message);
    }
}
function DefaultItemGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/FixedAssetManagement/AssetGroup/GetDefaultItemDetail",
                data: {},
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);

                        DisabledToolBar();

                        if (arr.Table[0].CreatedBy != null) {
                            $("#CreatedBy").text(arr.Table[0].CreatedBy);
                        }
                        if (arr.Table[0].create_on != null) {
                            $("#CreatedOn").text(arr.Table[0].CreatedOn);
                        }
                        if (arr.Table[0].ModifiedBy != null) {
                            $("#ModifiedBy").text(arr.Table[0].ModifiedBy);
                        }
                        if (arr.Table[0].ModifiedOn != null) {
                            $("#ModifiedOn").text(arr.Table[0].ModifiedOn);
                        }

                        Hidevalidation();

                        $('#asset_group_name').val(arr.Table[0].asset_group_name).attr("disabled", true);
                        $('#asset_grp_id').val(arr.Table[0].item_grp_id).attr("disabled", true);
                        $('#parent_group_name option[value=' + arr.Table[0].asset_grp_par_id + ']').prop('selected', true);
                        $("#parent_group_name").attr('disabled', true);

                        if (arr.Table[0].asset_cat != null && arr.Table[0].asset_cat != 0) {
                            $('#asset_category').empty().append('<option value=' + arr.Table[0].asset_cat + ' selected="selected">' + arr.Table[0].AssetCategory + '</option>');
                            $("#asset_category").attr('disabled', true);
                        } else {
                            $('#asset_category').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#asset_category").attr('disabled', true);
                        }

                        if (arr.Table[0].dep_coa != null && arr.Table[0].dep_coa != 0) {
                            $('#depreciation_account').empty().append('<option value=' + arr.Table[0].dep_coa + ' selected="selected">' + arr.Table[0].DepreciationAccount + '</option>');
                            $("#depreciation_account").attr('disabled', true);
                        } else {
                            $('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#depreciation_account").attr('disabled', true);
                        }

                        if (arr.Table[0].asset_coa != null && arr.Table[0].asset_coa != 0) {
                            $('#asset_account').empty().append('<option value=' + arr.Table[0].asset_coa + ' selected="selected">' + arr.Table[0].AssetAccount + '</option>');
                            $("#asset_account").attr('disabled', true);
                        } else {
                            $('#asset_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#asset_account").attr('disabled', true);
                        }
                    }

                    $("#btn_back").on('click', function () {
                        window.location.href = '/Dashboard/Home/';
                    });
                    $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $('#btn_delete').on('click',
                        function () {
                            DeleteItemGroup();
                        });
                    $("#btn_delete").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}

function EditItemGroupSetup(item_grp_id) {
    var ItemGrpId = item_grp_id;
    try {
        $("#asset_group_name").attr('disabled', false);
        $("#asset_group_name").focus();
        $("#parent_group_name").attr('disabled', false);
        $("#asset_category").attr('disabled', false);
        $("#depreciation_account").attr('disabled', false);
        $("#asset_accounte").attr('disabled', false);
        debugger;
        DisabledToolBar();
        $('#btn_add_new_item').off('click');
        $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $('#btn_edit_item').off('click');
        $("#btn_edit_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $('#btn_save').on('click', function () {
            InsertItemGroupDetail("Update");
        });
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        $('#btn_clear_item').on('click',
            function () {
                ItemGroupSetup();
            });
        $("#btn_clear_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        $("#asset_group_name").focus();

    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}

$('#btn_add_new_item').click(function () {
    debugger;
    NewItemGroupSetup();
});

$('#btn_clear_item').click(function () {
    debugger;
    ItemGroupSetup();
});

$('#btn_edit_item').click(function () {
    debugger;
    var group_name = $('#asset_group_name').val().trim()
    EditItemGroupSetup(group_name);

});

function NewItemGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/FixedAssetManagement/AssetGroup/Index",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                    $('#btn_add_new_item').off('click');
                    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    $('#btn_edit_item').off('click');
                    $("#btn_edit_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    $('#btn_back').off('click');
                    $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    DisabledToolBar();
                    $('#btn_save').on('click', function () {
                        debugger;
                        InsertItemGroupDetail("Insert");
                    });
                    $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $('#btn_clear_item').on('click',
                        function () {
                            ItemGroupSetup();
                        });
                    $("#btn_clear_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#asset_group_name").focus();
                },
            });
    } catch (err) {
        console.log("AssetGroupSetup Error : " + err.message);
    }
}

function ItemGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/FixedAssetManagement/AssetGroup/Index",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                    DefaultItemGroupSetup();

                },
            });
    } catch (err) {
        console.log("AssetGroupSetup Error : " + err.message);
    }
}
function BindSearchableDDL() {
    $("#asset_grp_par_id").select2();
    $("#depreciation_account").select2();
    $("#asset_account").select2();
    $("#asset_category").select2();
}
function BindSearchableDDLListData() {
    $("#asset_grp_par_id").select2();
    $("#depreciation_account").select2({
        ajax: {
            url: $("#depreciationAccount").val(),
            data: function (params) {
                var queryParameters = {
                    ddlcoa_name: params.term,
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
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
    $("#asset_account").select2({
        ajax: {
            url: $("#assetAccount").val(),
            data: function (params) {
                var queryParameters = {
                    ddlcoa_name: params.term,
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
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
    $("#asset_category").select2({
        ajax: {
            url: $("#assetcategoryAccount").val(),
            data: function (params) {
                var queryParameters = {
                    ddlcoa_name: params.term,
                    Group: params.page
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

$('#btn_save').click(function () {
    debugger;
    InsertItemGroupDetail();
});

function Hidevalidation() {
    $("#Asset_group_name").attr("style", "border-color: #ced4da;");
    $("#span_asset_group_name").attr("style", "display: none;");
}

function CheckAssetGroup_Validations(VoucherValidate) {
    debugger;
    var ErrorFlag = "N";
    if ($('#Asset_group_name').val() === '') {
        $("#Asset_group_name").attr("style", "border-color: #ff0000;");
        $("#span_asset_group_name").text($("#valueReq").text());
        $("#span_asset_group_name").css("display", "block");
        ErrorFlag = "Y";
    }
    let depPerVal = $('#depreciation_per').val();
    let depPerNum = parseFloat(depPerVal);

    // Check if empty, not a number, or zero
    if (depPerVal === '' || isNaN(depPerNum) || depPerNum === 0) {
        // if ($('#depreciation_per').val().trim() == '' || $('#depreciation_per').val().trim() == '0') {
        $("#depreciation_per").attr("style", "border-color: #ff0000;");
        $("#span_depreciation_per").text($("#valueReq").text());
        $("#span_depreciation_per").css("display", "block");
        ErrorFlag = "Y";
    }

    if ($('#asset_category').val() === "0") {
        $("[aria-labelledby='select2-asset_category-container']").css("border-color", "red");
        $("#span_asset_category").text($("#valueReq").text());
        $("#span_asset_category").css("display", "block");
        ErrorFlag = "Y";
    }
    if ($('#asset_account').val() === "0") {
        // $("#asset_account").attr("style", "border-color: #ff0000;");
        $("[aria-labelledby='select2-asset_account-container']").css("border-color", "red");
        $("#span_asset_account").text($("#valueReq").text());
        $("#span_asset_account").css("display", "block");
        ErrorFlag = "Y";
    }
    if ($('#depreciation_account').val() === "0") {
        //$("#depreciation_account").attr("style", "border-color: #ff0000;");
        $("[aria-labelledby='select2-depreciation_account-container']").css("border-color", "red");
        $("#spandepreciation_account").text($("#valueReq").text());
        $("#spandepreciation_account").css("display", "block");
        ErrorFlag = "Y";
    }
    if ($('#depreciation_freq').val() == '') {
        $("#depreciation_freq").attr("style", "border-color: #ff0000;");
        $("#spandepreciation_freq").text($("#valueReq").text());
        $("#spandepreciation_freq").css("display", "block");
        ErrorFlag = "Y";
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function InsertItemGroupDetail() {
    debugger;
    var ErrorFlag = "N";
    try {
        if (CheckAssetGroup_Validations("Y") == false) {
            return false;
        }

        if ($("#hdnonetimeclick").val() == "AllreadySavebtnclick") {
            $("#btn_save").css("filter", "grayscale(100%)");
            $("#btn_save").attr("disabled", true);
        }
        else {
            $("#hdnonetimeclick").val("AllreadySavebtnclick")
        }
        $('#dep_meth').prop("disabled", false);
        $('#asset_category').prop("disabled", false);
        $('#depreciation_account').prop("disabled", false);
        $('#asset_account').prop("disabled", false);
        $('#depreciation_freq').prop("disabled", false);
        $('#depreciation_per').prop("disabled", false);
    } catch (err) {
        console.log("InsertItemGroupDetail Error : " + err.message);
    }
}

function OnChangeAssetCategory(ACID) {
    var ACID = ACID.value;
    if (ACID == "" || ACID == null || ACID == undefined) {
        ACID = ACID;
    }
    if (ACID != "") {
        $("[aria-labelledby='select2-asset_category-container']").css("border-color", "#ced4da");
        $("#span_asset_category").css("display", "none");
    }
}

function OnChangeasset_account(ACID) {
    var ACID = ACID.value;
    if (ACID == "" || ACID == null || ACID == undefined) {
        ACID = ACID;
    }
    if (ACID != "") {
        $("[aria-labelledby='select2-asset_account-container']").css("border-color", "#ced4da");
        $("#span_asset_account").css("display", "none");
    }
}
function OnChangedepreciation_account(ACID) {
    var ACID = ACID.value;
    if (ACID == "" || ACID == null || ACID == undefined) {
        ACID = ACID;
    }
    if (ACID != "") {
        $("[aria-labelledby='select2-depreciation_account-container']").css("border-color", "#ced4da");
        $("#spandepreciation_account").css("display", "none");
    }
}

function OnChangedepreciation_per(ACID) {
    if ($('#depreciation_per').val().trim() != '') {
        $("#depreciation_per").attr("style", "border-color: #ced4da;");
        $("#span_depreciation_per").css("display", "none");
    }
    var DisAmt = $('#depreciation_per').val()//.trim();
    $('#depreciation_per').val(parseFloat(DisAmt).toFixed(ValDecDigit));////.trim();
}
function DisabledToolBar() {
    $('#btn_delete').off('click');
    $("#btn_delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $('#btn_save').off('click');
    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $('#btn_forward').off('click');
    $("#btn_forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $('#btn_approve').off('click');
    $("#btn_approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $('#btn_workflow').off('click');
    $("#btn_workflow").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $('#btn_print').off('click');
    $("#btn_print").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $('#btn_clear_item').off('click');
    $("#btn_clear_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $('#btn_back').off('click');
    $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
}

$('#btn_back').click(function () {
    debugger;
    window.location.href = '/Dashboard/Home/';
});
$('#btn_delete').click(function () {
    debugger;
    DeleteItemGroup();
});

function DeleteItemGroup() {
    debugger;
    var item_grp_id = $('#item_grp_id').val()//.trim();
    try {
        swal({
            title: $("#deltital").text(),
            text: $("#deltext").text() + "!",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: false
        },
            function () {
                $.ajax(
                    {
                        type: "POST",
                        url: "/FixedAssetManagement/AssetGroup/DeleteAssetGroup",
                        data: { item_grp_id: item_grp_id },
                        success: function (data) {
                            debugger;
                            if (data == 'ErrorPage') {
                                ErrorPage();
                                return false;
                            }
                            if (data == "-1") {
                                // alert("Dependencies exists, Unable to Delete!!"); 
                                swal("", $("#DependenciesExist").text(), "warning");
                                return false;
                            }
                            else {
                                NewItemGroupSetup();
                                DefaultItemGroupSetup();
                                swal("", $("#deletemsg").text(), "success");
                            }
                            $("#rightPageContent").empty().html(data);
                        },
                    });
            });
    } catch (err) {
        console.log("AssetGroupSetup Error : " + err.message);
    }

}
function ErrorPage() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/FixedAssetManagement/AssetGroup/ErrorPage",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });
    } catch (err) {
        console.log("ErrorPage Error : " + err.message);
    }
}

function AmountFloatQty(el, evt) {
    let QtyDecDigit = "#QtyDigit";
    if (Cmn_FloatValueonly(el, evt, QtyDecDigit) == false) {
        return false;
    }
    return true;
}

function GetSelectedParentDetail(ddlAG) {
    debugger;
    var item_grp_struc = ddlAG.value;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/AssetGroup/GetSelectedParentDetail",
                data: {
                    item_grp_struc: item_grp_struc
                },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }

                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        Hidevalidation();
                        if (arr.Table.length > 0) {
                            if ($('#asset_category') != "") {
                                $("[aria-labelledby='select2-asset_category-container']").css("border-color", "#ced4da");
                                $("#span_asset_category").css("display", "none");
                            }
                            if ($('#asset_account') != "") {
                                $("[aria-labelledby='select2-asset_account-container']").css("border-color", "#ced4da");
                                $("#span_asset_account").css("display", "none");
                            }
                            if ($('#depreciation_account') != "") {
                                $("[aria-labelledby='select2-depreciation_account-container']").css("border-color", "#ced4da");
                                $("#spandepreciation_account").css("display", "none");
                            }
                            if (arr.Table[0].dep_method != null && arr.Table[0].dep_method != 0) {
                               // $('#dep_meth').empty().append('<option value=' + arr.Table[0].dep_method + ' selected="selected">' + arr.Table[0].Depreciation_method + '</option>');
                                $('#dep_meth').val(arr.Table[0].dep_method);

                            } else {
                               // $('#dep_meth').empty().append('<option value="0" selected="selected">---Select---</option>');
                                $('#dep_meth').val("SL");
                            }
                            if (arr.Table[0].asset_cat != null && arr.Table[0].asset_cat != 0) {
                                //$('#asset_category').empty().append('<option value=' + arr.Table[0].asset_cat + ' selected="selected">' + arr.Table[0].AssetCategory + '</option>');
                                $('#asset_category').val(arr.Table[0].asset_cat).trigger('change');

                            } else {
                                //$('#asset_category').empty().append('<option value="0" selected="selected">---Select---</option>');
                                $('#asset_category').val("0").trigger('change');
                            }
                            if (arr.Table[0].asset_coa != null && arr.Table[0].asset_coa != 0) {
                              //  $('#asset_account').empty().append('<option value=' + arr.Table[0].asset_coa + ' selected="selected">' + arr.Table[0].AssetAccount + '</option>');
                                $('#asset_account').val(arr.Table[0].asset_coa).trigger('change');

                            } else {
                                //$('#asset_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                                $('#asset_category').val("0").trigger('change');
                            }
                            if (arr.Table[0].dep_coa != null && arr.Table[0].dep_coa != 0) {
                               // $('#depreciation_account').empty().append('<option value=' + arr.Table[0].dep_coa + ' selected="selected">' + arr.Table[0].DepreciationAccount + '</option>');
                                $('#depreciation_account').val(arr.Table[0].dep_coa).trigger('change');
                            } else {
                               // $('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                                $('#depreciation_account').val("0").trigger('change');
                            }
                            if (arr.Table[0].dep_freq != null && arr.Table[0].dep_freq != 0) {
                               // $('#depreciation_freq').empty().append('<option value=' + arr.Table[0].dep_freq + ' selected="selected">' + arr.Table[0].dep_frequency + '</option>');
                                $('#depreciation_freq').val(arr.Table[0].dep_freq).trigger('change');
                            } else {
                                //$('#depreciation_freq').empty().append('<option value="0" selected="selected">---Select---</option>');
                                $('#depreciation_freq').val("M").trigger('change');
                            }
                            $('#depreciation_per').empty().val(arr.Table[0].dep_per);
                            $('#dep_meth').prop("disabled", true);
                            $('#asset_category').prop("disabled", true);
                            $('#depreciation_account').prop("disabled", true);
                            $('#asset_account').prop("disabled", true);
                            $('#depreciation_freq').prop("disabled", true);
                            $('#depreciation_per').prop("disabled", true);
                        } else {
                            //$('#dep_meth').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //$('#asset_category').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //$('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //$('#asset_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //$('#depreciation_freq').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //$('#depreciation_per').empty();

                            $('#dep_meth').val("SL");
                            $('#asset_category').val("0").trigger('change');
                            $('#depreciation_account').val("0").trigger('change');
                            $('#asset_account').val("0").trigger('change');
                            $('#depreciation_freq').val("M");
                            $('#depreciation_per').val("");
                            $('#dep_meth').prop("disabled", false);
                            $('#asset_category').prop("disabled", false);
                            $('#depreciation_account').prop("disabled", false);
                            $('#asset_account').prop("disabled", false);
                            $('#depreciation_freq').prop("disabled", false);
                            $('#depreciation_per').prop("disabled", false);
                        }
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
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
        debugger;
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

function VallidateData(FieldId) {
    CheckVallidation(FieldId, "span" + FieldId);
}
