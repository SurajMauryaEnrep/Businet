$(document).ready(function () {
    debugger;
    //var PageName = sessionStorage.getItem("MenuName");
    //$('#ItmgrpDetailsPageName').text(PageName);
    ////GetAllItemGrp();
    $("#IDInterBranch_pur_coa").select2();
    $("#IDInterBranch_sls_coa").select2();
    $("#item_grp_par_id").select2({
       /* matcher: matchCustom,*/
    });
    BindSearchableDDLListData();
    $(this).on('click', '.simpleTree-label', function (e) {
        debugger;

        var GrpID = this.nextSibling.innerText;
        
        var Parent = this.innerText;
        GetMenuData(GrpID)
    });
    RlvntAccountLinkingMendatory("", "");
    if ($("#i_stk").is(":checked")) {
        $("#requiredStk_coa").css("display", "inherit");
    }
    else {
        $("#requiredStk_coa").attr("style", "display: none;");
    }
});

function GetAllItemGrp() {
    debugger;
    var RequestedUrl = window.location.protocol + "//" + window.location.host + "/BusinessLayer/ItemGroupSetup/GetAllItemGrp";
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
    window.location.href = "/BusinessLayer/ItemGroupSetup/ItemGroupSetupView/?ItemGrpId=" + item_grp_id;
}

function DefaultItemGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemGroupSetup/GetDefaultItemDetail",
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

                        $('#item_group_name').val(arr.Table[0].item_group_name).attr("disabled", true);
                        $('#item_grp_id').val(arr.Table[0].item_grp_id).attr("disabled", true);
                        $('#parent_group_name option[value=' + arr.Table[0].item_grp_par_id + ']').prop('selected', true);
                        $("#parent_group_name").attr('disabled', true);

                        $('#issue_method option[value=' + arr.Table[0].issue_method + ']').prop('selected', true);
                        $("#issue_method").attr('disabled', true);

                        $('#cost_method option[value=' + arr.Table[0].cost_method + ']').prop('selected', true);
                        $("#cost_method").attr('disabled', true);

                        $('#It_Remarks').val(arr.Table[0].It_Remarks).attr("disabled", true);

                        if (arr.Table[0].i_sls == "Y") {
                            $('#i_sls').prop("checked", true);
                            $("#i_sls").prop("disabled", true);
                        } else {
                            $('#i_sls').prop("checked", false);
                            $("#i_sls").prop("disabled", true);
                        }

                        if (arr.Table[0].i_pur == "Y") {
                            $('#i_pur').prop("checked", true);
                            $("#i_pur").prop("disabled", true);
                        } else {
                            $('#i_pur').prop("checked", false);
                            $("#i_pur").prop("disabled", true);
                        }

                        if (arr.Table[0].i_wip == "Y") {
                            $('#i_wip').prop("checked", true);
                            $("#i_wip").prop("disabled", true);
                        } else {
                            $('#i_wip').prop("checked", false);
                            $("#i_wip").prop("disabled", true);
                        }
                        if (arr.Table[0].i_capg == "Y") {
                            $("#i_capg").prop("checked", true);
                            $("#i_capg").prop("disabled", true);
                        } else {
                            $("#i_capg").prop("checked", false);
                            $("#i_capg").prop("disabled", true);
                        }

                        if (arr.Table[0].i_stk == "Y") {
                            $('#i_stk').prop("checked", true);
                            $("#i_stk").prop("disabled", true);

                            $("#requiredStk_coa").css("display", "inherit");
                        } else {
                            $('#i_stk').prop("checked", false);
                            $("#i_stk").prop("disabled", true);

                            $("#requiredStk_coa").attr("style", "display: none;");
                        }

                        if (arr.Table[0].i_qc == "Y") {
                            $('#i_qc').prop("checked", true);
                            $("#i_qc").prop("disabled", true);
                        } else {
                            $('#i_qc').prop("checked", false);
                            $("#i_qc").prop("disabled", true);
                        }

                        if (arr.Table[0].i_srvc == "Y") {
                            $('#i_srvc').prop("checked", true);
                            $("#i_srvc").prop("disabled", true);
                        } else {
                            $('#i_srvc').prop("checked", false);
                            $("#i_srvc").prop("disabled", true);
                        }

                        if (arr.Table[0].i_cons == "Y") {
                            $('#i_cons').prop("checked", true);
                            $("#i_cons").prop("disabled", true);
                        } else {
                            $('#i_cons').prop("checked", false);
                            $("#i_cons").prop("disabled", true);
                        }

                        if (arr.Table[0].i_serial == "Y") {
                            $('#i_serial').prop("checked", true);
                            $("#i_serial").prop("disabled", true);
                        } else {
                            $('#i_serial').prop("checked", false);
                            $("#i_serial").prop("disabled", true);
                        }

                        if (arr.Table[0].i_sam == "Y") {
                            $('#i_sam').prop("checked", true);
                            $("#i_sam").prop("disabled", true);
                        } else {
                            $('#i_sam').prop("checked", false);
                            $("#i_sam").prop("disabled", true);
                        }

                        if (arr.Table[0].i_batch == "Y") {
                            $('#i_batch').prop("checked", true);
                            $("#i_batch").prop("disabled", true);
                        } else {
                            $('#i_batch').prop("checked", false);
                            $("#i_batch").prop("disabled", true);
                        }

                        if (arr.Table[0].i_exp == "Y") {
                            $('#i_exp').prop("checked", true);
                            $("#i_exp").prop("disabled", true);
                        } else {
                            $('#i_exp').prop("checked", false);
                            $("#i_exp").prop("disabled", true);
                        }
                        if (arr.Table[0].i_pack == "Y") {
                            $('#i_pack').prop("checked", true);
                            $("#i_pack").prop("disabled", true);
                        } else {
                            $('#i_pack').prop("checked", false);
                            $("#i_pack").prop("disabled", true);
                        }
                        if (arr.Table[0].i_catalog == "Y") {
                            $('#i_catalog').prop("checked", true);
                            $("#i_catalog").prop("disabled", true);
                        } else {
                            $('#i_catalog').prop("checked", false);
                            $("#i_catalog").prop("disabled", true);
                        }

                        if (arr.Table[0].loc_sls_coa != null && arr.Table[0].loc_sls_coa != 0) {
                            debugger;
                            $('#Local_Sale_Account').empty().append('<option value=' + arr.Table[0].loc_sls_coa + ' selected="selected">' + arr.Table[0].LocalSaleAccount + '</option>');
                            $("#Local_Sale_Account").attr('disabled', true);
                        } else {
                            $('#Local_Sale_Account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#Local_Sale_Account").attr('disabled', true);
                        }

                        if (arr.Table[0].exp_sls_coa != null && arr.Table[0].exp_sls_coa != 0) {
                            $('#export_sale_account').empty().append('<option value=' + arr.Table[0].exp_sls_coa + ' selected="selected">' + arr.Table[0].ExportSaleAccount + '</option>');
                            $("#export_sale_account").attr('disabled', true);
                        } else {
                            $('#export_sale_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#export_sale_account").attr('disabled', true);
                        }

                        if (arr.Table[0].loc_pur_coa != null && arr.Table[0].loc_pur_coa != 0) {
                            $('#local_purchase_account').empty().append('<option value=' + arr.Table[0].loc_pur_coa + ' selected="selected">' + arr.Table[0].LocalPurchaseAccount + '</option>');
                            $("#local_purchase_account").attr('disabled', true);
                        } else {
                            $('#local_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#local_purchase_account").attr('disabled', true);
                        }

                        if (arr.Table[0].imp_pur_coa != null && arr.Table[0].imp_pur_coa != 0) {
                            $('#import_purchase_account').empty().append('<option value=' + arr.Table[0].imp_pur_coa + ' selected="selected">' + arr.Table[0].ImportPurchaseAccount + '</option>');
                            $("#import_purchase_account").attr('disabled', true);
                        } else {
                            $('#import_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#import_purchase_account").attr('disabled', true);
                        }
                        /*As Vishal sir said to hide below data on page*/
                        if (arr.Table[0].stk_coa != null && arr.Table[0].stk_coa != 0) {
                            $('#stock_account').empty().append('<option value=' + arr.Table[0].stk_coa + ' selected="selected">' + arr.Table[0].StockAccount + '</option>');
                            $("#stock_account").attr('disabled', true);
                        } else {
                            $('#stock_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#stock_account").attr('disabled', true);
                        }

                        if (arr.Table[0].sal_ret_coa != null && arr.Table[0].sal_ret_coa != 0) {
                            $('#sale_return_account').empty().append('<option value=' + arr.Table[0].sal_ret_coa + ' selected="selected">' + arr.Table[0].SaleReturnAccount + '</option>');
                            $("#sale_return_account").attr('disabled', true);
                        } else {
                            $('#sale_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#sale_return_account").attr('disabled', true);
                        }

                        if (arr.Table[0].disc_coa != null && arr.Table[0].disc_coa != 0) {
                            $('#discount_account').empty().append('<option value=' + arr.Table[0].disc_coa + ' selected="selected">' + arr.Table[0].DiscountAccount + '</option>');
                            $("#discount_account").attr('disabled', true);
                        } else {
                            $('#discount_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#discount_account").attr('disabled', true);
                        }

                        if (arr.Table[0].pur_ret_coa != null && arr.Table[0].pur_ret_coa != 0) {
                            $('#purchase_return_account').empty().append('<option value=' + arr.Table[0].pur_ret_coa + ' selected="selected">' + arr.Table[0].PurchaseReturnAccount + '</option>');
                            $("#purchase_return_account").attr('disabled', true);
                        } else {
                            $('#purchase_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#purchase_return_account").attr('disabled', true);
                        }
                        /*As Vishal sir said to hide below data on page*/
                        //if (arr.Table[0].prov_pay_coa != null && arr.Table[0].prov_pay_coa != 0) {
                        //    $('#provisional_payable_account').empty().append('<option value=' + arr.Table[0].prov_pay_coa + ' selected="selected">' + arr.Table[0].ProvisionalPayableAccount + '</option>');
                        //    $("#provisional_payable_account").attr('disabled', true);
                        //} else {
                        //    $('#provisional_payable_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                        //    $("#provisional_payable_account").attr('disabled', true);
                        //}

                        //if (arr.Table[0].cogs_coa != null && arr.Table[0].cogs_coa != 0) {
                        //    $('#cost_of_goods_sold_account').empty().append('<option value=' + arr.Table[0].cogs_coa + ' selected="selected">' + arr.Table[0].CostofGoodsSoldAccount + '</option>');
                        //    $("#cost_of_goods_sold_account").attr('disabled', true);
                        //} else {
                        //    $('#cost_of_goods_sold_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                        //    $("#cost_of_goods_sold_account").attr('disabled', true);
                        //}

                        //if (arr.Table[0].cogs_adj_coa != null && arr.Table[0].cogs_adj_coa != 0) {
                        //    $('#stock_adjustment_account').empty().append('<option value=' + arr.Table[0].cogs_adj_coa + ' selected="selected">' + arr.Table[0].StockAdjustmentAccount + '</option>');
                        //    $("#stock_adjustment_account").attr('disabled', true);
                        //} else {
                        //    $('#stock_adjustment_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                        //    $("#stock_adjustment_account").attr('disabled', true);
                        //}

                        if (arr.Table[0].dep_coa != null && arr.Table[0].dep_coa != 0) {
                            $('#depreciation_account').empty().append('<option value=' + arr.Table[0].dep_coa + ' selected="selected">' + arr.Table[0].DepreciationAccount + '</option>');
                            $("#depreciation_account").attr('disabled', true);
                        } else {
                            $('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#depreciation_account").attr('disabled', true);
                        }

                        if (arr.Table[0].asset_coa != null && arr.Table[0].asset_coa != 0) {
                            $('#asset_accounte').empty().append('<option value=' + arr.Table[0].asset_coa + ' selected="selected">' + arr.Table[0].AssetAccount + '</option>');
                            $("#asset_accounte").attr('disabled', true);
                        } else {
                            $('#asset_accounte').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $("#asset_accounte").attr('disabled', true);
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
        $("#item_group_name").attr('disabled', false);
        $("#item_group_name").focus();
        $("#parent_group_name").attr('disabled', false);
        $("#issue_method").attr('disabled', false);
        $("#cost_method").attr('disabled', false);
        $("#It_Remarks").attr('disabled', false);
        $("#i_sls").prop("disabled", false);
        $("#i_pur").prop("disabled", false);
        $("#i_wip").prop("disabled", false);
        $("#i_capg").prop("disabled", false);
        $("#i_stk").prop("disabled", false);
        $("#i_qc").prop("disabled", false);
        $("#i_srvc").prop("disabled", false);
        $("#i_cons").prop("disabled", false);
        $("#i_serial").prop("disabled", false);
        $("#i_sam").prop("disabled", false);
        $("#i_batch").prop("disabled", false);
        $("#i_exp").prop("disabled", false);
        $("i_pack").pro("disabled", false);
        $("i_catalog").pro("disabled", false);        
        $("#Local_Sale_Account").attr('disabled', false);
        $("#export_sale_account").attr('disabled', false);
        $("#local_purchase_account").attr('disabled', false);
        $("#import_purchase_account").attr('disabled', false);
        /*As Vishal sir said to hide below data on page*/
        $("#stock_account").attr('disabled', false);
        $("#sale_return_account").attr('disabled', false);
        $("#discount_account").attr('disabled', false);
        $("#purchase_return_account").attr('disabled', false);
        /*As Vishal sir said to hide below data on page*/
        //$("#provisional_payable_account").attr('disabled', false);
        //$("#cost_of_goods_sold_account").attr('disabled', false);
        //$("#stock_adjustment_account").attr('disabled', false);
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
        $("#item_group_name").focus();
        
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
    var group_name = $('#item_group_name').val().trim()
    EditItemGroupSetup(group_name);

}); 

function NewItemGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemGroupSetup/Index",
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
                    $("#item_group_name").focus();
                },
            });
    } catch (err) {
        console.log("ItemGroupSetup Error : " + err.message);
    }
}

function ItemGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemGroupSetup/Index",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data); 
                    DefaultItemGroupSetup();
                   
                },
            });
    } catch (err) {
        console.log("ItemGroupSetup Error : " + err.message);
    }
}

function BindSearchableDDLListData() {
    $("#Local_Sale_Account").select2({
        ajax: {
            url: $("#LocalSaleAccount").val(),
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
    $("#IDInterBranch_sls_coa").select2({
        ajax: {
            url: $("#LocalSaleAccount").val(),
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
    $("#export_sale_account").select2({
        ajax: {
            url: $("#exportSaleAccount").val(),
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
    $("#IDInterBranch_pur_coa").select2({
        ajax: {
            url: $("#localPurchaseAccount").val(),
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
    $("#local_purchase_account").select2({
        ajax: {
            url: $("#localPurchaseAccount").val(),
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
    $("#import_purchase_account").select2({
        ajax: {
            url: $("#importPurchaseAccount").val(),
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
    /*As Vishal sir said to hide below data on page*/
    $("#stock_account").select2({
        ajax: {
            url: $("#stockAccount").val(),
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
    $("#sale_return_account").select2({
        ajax: {
            url: $("#saleReturnAccount").val(),
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
    $("#discount_account").select2({
        ajax: {
            url: $("#discountAccount").val(),
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
    $("#purchase_return_account").select2({
        ajax: {
            url: $("#purchaseReturnAccount").val(),
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
    /*As Vishal sir said to hide below data on page*/
    //$("#provisional_payable_account").select2({
    //    ajax: {
    //        url: $("#provisionalPayableAccount").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                ddlcoa_name: params.term,
    //                Group: params.page
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                ErrorPage();
    //                return false;
    //            }
    //            params.page = params.page || 1;
    //            return {
    //                results: $.map(data, function (val, item) {
    //                    return { id: val.ID, text: val.Name };
    //                })
    //            };
    //        }
    //    },
    //});
    //$("#cost_of_goods_sold_account").select2({
    //    ajax: {
    //        url: $("#costOfGoodsSoldAccount").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                ddlcoa_name: params.term,
    //                Group: params.page
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                ErrorPage();
    //                return false;
    //            }
    //            params.page = params.page || 1;
    //            return {
    //                results: $.map(data, function (val, item) {
    //                    return { id: val.ID, text: val.Name };
    //                })
    //            };
    //        }
    //    },
    //});
    //$("#stock_adjustment_account").select2({
    //    ajax: {
    //        url: $("#stockAdjustmentAccount").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                ddlcoa_name: params.term,
    //                Group: params.page
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                ErrorPage();
    //                return false;
    //            }
    //            params.page = params.page || 1;
    //            return {
    //                results: $.map(data, function (val, item) {
    //                    return { id: val.ID, text: val.Name };
    //                })
    //            };
    //        }
    //    },
    //});
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
}

$('#btn_save').click(function () {
    debugger;
    InsertItemGroupDetail(); 
});

function Hidevalidation() {

    $("#item_group_name").attr("style", "border-color: #ced4da;");
    $("#span_item_group_name").attr("style", "display: none;"); 

};

function InsertItemGroupDetail()
{
    debugger;       
    try { 

        if ($('#item_group_name').val().trim() == '') {
            $("#item_group_name").attr("style", "border-color: #ff0000;");
            $("#span_item_group_name").text($("#valueReq").text());
            $("#span_item_group_name").css("display", "block");
            return false;
        }
        if (RlvntAccountLinkingMendatory("Save", "") == false) {
            return false;
        }
        if ($("#hdnonetimeclick").val() == "AllreadySavebtnclick") {
            $("#btn_save").css("filter", "grayscale(100%)");
            $("#btn_save").attr("disabled", true);
          
        }
        else {
            $("#hdnonetimeclick").val("AllreadySavebtnclick")
        }
    } catch (err) {
        console.log("InsertItemGroupDetail Error : " + err.message);
    }
} 

function DisabledToolBar()
{
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

function DeleteItemGroup()
{
    debugger;
    var item_grp_id = $('#item_grp_id').val().trim(); 
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
                url: "/BusinessLayer/ItemGroupSetup/DeleteItemGroup",
                data: { item_grp_id: item_grp_id},
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
                        //alert("Data Deleted Successfully.");
                        NewItemGroupSetup();
                        DefaultItemGroupSetup();
                        swal("", $("#deletemsg").text(), "success");
                    }
                    $("#rightPageContent").empty().html(data); 
                },
            });
            });
        

        
    } catch (err) {
        console.log("ItemGroupSetup Error : " + err.message);
    }

}
function ErrorPage() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemGroupSetup/ErrorPage",
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

function GetSelectedParentDetail(ddlFruits) {
    debugger;
    //var selectedText = ddlFruits.options[ddlFruits.selectedIndex].innerHTML;
    //var selectedValue = ddlFruits.value;
    //alert("Selected Text:   Value: " + selectedValue);

    var item_grp_struc =  ddlFruits.value;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemGroupSetup/GetSelectedParentDetail",
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
                            if (arr.Table[0].i_sls == "Y") {
                                $('#i_sls').prop("checked", true);
                                $("#i_catalog").prop("disabled", false);
                            } else {
                                $('#i_sls').prop("checked", false);
                                $("#i_catalog").prop("disabled", true);
                                $("#i_catalog").prop("checked", false);
                            }

                            if (arr.Table[0].i_pur == "Y") {
                                $('#i_pur').prop("checked", true);
                            } else {
                                $('#i_pur').prop("checked", false);
                            }

                            if (arr.Table[0].i_wip == "Y") {
                                $('#i_wip').prop("checked", true);
                            } else {
                                $('#i_wip').prop("checked", false);
                            }
                            if (arr.Table[0].i_capg == "Y") {
                                $("#i_capg").prop("checked", true);
                            } else {
                                $("#i_capg").prop("checked", false);
                            }

                            if (arr.Table[0].i_stk == "Y") {
                                $('#i_stk').prop("checked", true);

                                $("#requiredStk_coa").css("display", "inherit");
                            } else {
                                $('#i_stk').prop("checked", false);

                                $("#requiredStk_coa").attr("style", "display: none;");
                            }

                            if (arr.Table[0].i_qc == "Y") {
                                $('#i_qc').prop("checked", true);
                            } else {
                                $('#i_qc').prop("checked", false);
                            }

                            if (arr.Table[0].i_srvc == "Y") {
                                $('#i_srvc').prop("checked", true);
                            } else {
                                $('#i_srvc').prop("checked", false);
                            }

                            if (arr.Table[0].i_cons == "Y") {
                                $('#i_cons').prop("checked", true);
                            } else {
                                $('#i_cons').prop("checked", false);
                            }

                            if (arr.Table[0].i_serial == "Y") {
                                $('#i_serial').prop("checked", true);
                            } else {
                                $('#i_serial').prop("checked", false);
                            }

                            if (arr.Table[0].i_sam == "Y") {
                                $('#i_sam').prop("checked", true);
                            } else {
                                $('#i_sam').prop("checked", false);
                            }

                            if (arr.Table[0].i_batch == "Y") {
                                $('#i_batch').prop("checked", true);
                            } else {
                                $('#i_batch').prop("checked", false);
                            }

                            if (arr.Table[0].i_exp == "Y") {
                                $('#i_exp').prop("checked", true);
                            } else {
                                $('#i_exp').prop("checked", false);
                            }
                            if (arr.Table[0].i_pack == "Y") {
                                $('#i_pack').prop("checked", true);                              
                            } else {
                                $('#i_pack').prop("checked", false);                               
                            }
                            if (arr.Table[0].i_catalog == "Y") {
                                $('#i_catalog').prop("checked", true);
                            } else {
                                $('#i_catalog').prop("checked", false);
                            }


                            if (arr.Table[0].loc_sls_coa != null && arr.Table[0].loc_sls_coa != 0) {
                                $('#Local_Sale_Account').empty().append('<option value=' + arr.Table[0].loc_sls_coa + ' selected="selected">' + arr.Table[0].LocalSaleAccount + '</option>');

                            } else {
                                $('#Local_Sale_Account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            }

                            if (arr.Table[0].exp_sls_coa != null && arr.Table[0].exp_sls_coa != 0) {
                                $('#export_sale_account').empty().append('<option value=' + arr.Table[0].exp_sls_coa + ' selected="selected">' + arr.Table[0].ExportSaleAccount + '</option>');

                            } else {
                                $('#export_sale_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            }

                            if (arr.Table[0].loc_pur_coa != null && arr.Table[0].loc_pur_coa != 0) {
                                $('#local_purchase_account').empty().append('<option value=' + arr.Table[0].loc_pur_coa + ' selected="selected">' + arr.Table[0].LocalPurchaseAccount + '</option>');

                            } else {
                                $('#local_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            }

                            if (arr.Table[0].imp_pur_coa != null && arr.Table[0].imp_pur_coa != 0) {
                                $('#import_purchase_account').empty().append('<option value=' + arr.Table[0].imp_pur_coa + ' selected="selected">' + arr.Table[0].ImportPurchaseAccount + '</option>');

                            } else {
                                $('#import_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            }
                            /*As Vishal sir said to hide below data on page*/
                            if (arr.Table[0].stk_coa != null && arr.Table[0].stk_coa != 0) {
                                $('#stock_account').empty().append('<option value=' + arr.Table[0].stk_coa + ' selected="selected">' + arr.Table[0].StockAccount + '</option>');

                            } else {
                                $('#stock_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            }

                            if (arr.Table[0].sal_ret_coa != null && arr.Table[0].sal_ret_coa != 0) {
                                $('#sale_return_account').empty().append('<option value=' + arr.Table[0].sal_ret_coa + ' selected="selected">' + arr.Table[0].SaleReturnAccount + '</option>');

                            } else {
                                $('#sale_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            }

                            if (arr.Table[0].disc_coa != null && arr.Table[0].disc_coa != 0) {
                                $('#discount_account').empty().append('<option value=' + arr.Table[0].disc_coa + ' selected="selected">' + arr.Table[0].DiscountAccount + '</option>');

                            } else {
                                $('#discount_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            }

                            if (arr.Table[0].pur_ret_coa != null && arr.Table[0].pur_ret_coa != 0) {
                                $('#purchase_return_account').empty().append('<option value=' + arr.Table[0].pur_ret_coa + ' selected="selected">' + arr.Table[0].PurchaseReturnAccount + '</option>');

                            } else {
                                $('#purchase_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            }
                            /*As Vishal sir said to hide below data on page*/
                            //if (arr.Table[0].prov_pay_coa != null && arr.Table[0].prov_pay_coa != 0) {
                            //    $('#provisional_payable_account').empty().append('<option value=' + arr.Table[0].prov_pay_coa + ' selected="selected">' + arr.Table[0].ProvisionalPayableAccount + '</option>');

                            //} else {
                            //    $('#provisional_payable_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            //}

                            //if (arr.Table[0].cogs_coa != null && arr.Table[0].cogs_coa != 0) {
                            //    $('#cost_of_goods_sold_account').empty().append('<option value=' + arr.Table[0].cogs_coa + ' selected="selected">' + arr.Table[0].CostofGoodsSoldAccount + '</option>');

                            //} else {
                            //    $('#cost_of_goods_sold_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            //}

                            //if (arr.Table[0].cogs_adj_coa != null && arr.Table[0].cogs_adj_coa != 0) {
                            //    $('#stock_adjustment_account').empty().append('<option value=' + arr.Table[0].cogs_adj_coa + ' selected="selected">' + arr.Table[0].StockAdjustmentAccount + '</option>');

                            //} else {
                            //    $('#stock_adjustment_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            //}

                            if (arr.Table[0].dep_coa != null && arr.Table[0].dep_coa != 0) {
                                $('#depreciation_account').empty().append('<option value=' + arr.Table[0].dep_coa + ' selected="selected">' + arr.Table[0].DepreciationAccount + '</option>');

                            } else {
                                $('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }

                            if (arr.Table[0].asset_coa != null && arr.Table[0].asset_coa != 0) {
                                $('#asset_account').empty().append('<option value=' + arr.Table[0].asset_coa + ' selected="selected">' + arr.Table[0].AssetAccount + '</option>');

                            } else {
                                $('#asset_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            }
                        } else
                        {
                            debugger;
                            $('#i_sls').prop("checked", true);
                            $('#i_pur').prop("checked", true);
                            $('#i_wip').prop("checked", false);
                            $("#i_capg").prop("checked", false);
                            $('#i_stk').prop("checked", false);
                            $('#i_qc').prop("checked", false);
                            $('#i_srvc').prop("checked", false);
                            $('#i_cons').prop("checked", false);
                            $('#i_serial').prop("checked", false);
                            $('#i_sam').prop("checked", false);
                            $('#i_batch').prop("checked", false);
                            $('#i_exp').prop("checked", false);
                            $('#i_pack').prop("checked", false);
                            $('#i_catalog').prop("checked", false);

                            $("#requiredStk_coa").attr("style", "display: none;");
                            

                                $('#Local_Sale_Account').empty().append('<option value="0" selected="selected">---Select---</option>');
                           
                                $('#export_sale_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                       
                                $('#local_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                     
                                $('#import_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            /*As Vishal sir said to hide below data on page*/
                                $('#stock_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                        
                                $('#sale_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                     
                                $('#discount_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                     
                                $('#purchase_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                            /*As Vishal sir said to hide below data on page*/
                                //$('#provisional_payable_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                       
                                //$('#cost_of_goods_sold_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                      
                                //$('#stock_adjustment_account').empty().append('<option value="0" selected="selected">---Select---</option>');

                                $('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');                          
                           
                                $('#asset_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                        }
                        RlvntAccountLinkingMendatory("", "");
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function OnClickStockToggle() {
    debugger;
    var stktgl = "";
    if ($("#i_stk").is(":checked")) {
        stktgl = 'Y';
    }
    else {
        stktgl = 'N';
    }
    if (stktgl == 'Y') {
        $("#i_srvc").prop("checked", false);
        $("#i_pack").prop("disabled", false);
         //---------Modifyed By Shubham Maurya 08-11-2023----------------------------------//
        $("#i_srvc").prop("checked", false);
        $("#i_cons").prop("checked", false);
        //----------------------------------//
        $("#requiredStk_coa").css("display", "inherit");
    }
    else {
        $("#i_srvc").prop("checked", true);
        $("#i_wip").prop("checked", false);
        $("#i_capg").prop("checked", false);
        $("#i_qc").prop("checked", false);
        $("#i_cons").prop("checked", false);
        $("#i_serial").prop("checked", false);
        $("#i_sam").prop("checked", false);
        $("#i_batch").prop("checked", false);
        $("#i_exp").prop("checked", false);
        $("#i_pack").prop("checked", false);
        $("#i_pack").prop("disabled", true);

        $("#requiredStk_coa").attr("style", "display: none;");
    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickSamToggle() {
    debugger;
    var samtgl = "";
    if ($("#i_sam").is(":checked")) {
        samtgl = 'Y';
    }
    else {
        samtgl = 'N';
    }
    if (samtgl == 'Y') {
        $("#i_srvc").prop("checked", false);
        $("#i_cons").prop("checked", false);
        $("#i_stk").prop("checked", true);

        $("#requiredStk_coa").css("display", "inherit");
    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickConsToggle() {
    var constgl = "";
    if ($("#i_cons").is(":checked")) {
        constgl = 'Y';
    }
    else {
        constgl = 'N';
    }
    if (constgl == 'Y') {
        //$("#i_srvc").prop("checked", false);
        //$("#i_stk").prop("checked", true);
        $("#i_exp").prop("checked", false);
        //---------Modifyed By Shubham Maurya 08-11-2023----------------------------------//
        $("#i_srvc").prop("checked", false);
        $("#i_stk").prop("checked", false);
        $("#i_batch").prop("checked", false);
        $("#i_serial").prop("checked", false);
        $("#i_qc").prop("checked", false);
        $("#i_capg").prop("checked", false);
        $("#i_wip").prop("checked", false);
        $("#i_pack").prop("checked", false);
        $("#i_sam").prop("checked", false);
        //---------------------------------------------//
        $("#i_sls").prop("checked", false);
        $("#i_catalog").prop("checked", false);

        $("#requiredStk_coa").attr("style", "display: none;");
    }
    RlvntAccountLinkingMendatory("", "");
}
function onclickpacking() {
    var Pack = "";
    if ($("#i_pack").is(":checked")) {
        Pack = 'Y';
    }
    else {
        Pack = 'N';
    }
    if (Pack == 'Y') {
        $("#i_cons").prop("checked", false);
    }
}
function onclickcatalog() {
    var catalog = "";
    if ($("#i_catalog").is(":checked")) {
        catalog = 'Y';
    }
    else {
        catalog = 'N';
    }
    if (catalog == 'Y') {
        $("#i_cons").prop("checked", false);
    }
}
function OnClickWipToggle() {
    var constgl = "";
    if ($("#i_wip").is(":checked")) {
        constgl = 'Y';
    }
    else {
        constgl = 'N';
    }
    if (constgl == 'Y') {

        $("#i_srvc").prop("checked", false);
        $("#i_stk").prop("checked", true);
        $("#i_cons").prop("checked", false);

        $("#requiredStk_coa").css("display", "inherit");
    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickQcToggle() {
    var constgl = "";
    if ($("#i_qc").is(":checked")) {
        constgl = 'Y';
    }
    else {
        constgl = 'N';
    }
    if (constgl == 'Y') {

        $("#i_srvc").prop("checked", false);
        $("#i_stk").prop("checked", true);
        OnClickStockToggle();
    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickCapitalToggle() {
    debugger;
    var capitaltgl = "";
    if ($("#i_capg").is(":checked")) {
        capitaltgl = 'Y';
    }
    else {
        capitaltgl = 'N';
    }
    if (capitaltgl == 'Y') {

        $("#i_srvc").prop("checked", false);
        $("#i_cons").prop("checked", false);
        $("#i_sam").prop("checked", false);
        $("#i_batch").prop("checked", false);
        $("#i_wip").prop("checked", false);
        $("#i_exp").prop("checked", false);
        $("#i_catalog").prop("checked", false);
        $("#i_pack").prop("checked", false);

        $("#i_sls").prop("checked", true);
        $("#i_pur").prop("checked", true);
        $("#i_stk").prop("checked", true);       
        $("#i_serial").prop("checked", true);
        $("#i_qc").prop("checked", true);

        $("#requiredStk_coa").css("display", "inherit");
    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickServiceToggle() {
    debugger;
    var srvctgl = "";
    if ($("#i_srvc").is(":checked")) {
        srvctgl = 'Y';
    }
    else {
        srvctgl = 'N';
    }
    if (srvctgl == 'Y') {
        //$("#i_stk").prop("checked", false);
        //$("#i_wip").prop("checked", false);
        //$("#i_capg").prop("checked", false);
        //$("#i_qc").prop("checked", false);
        //$("#i_cons").prop("checked", false);
        //$("#i_serial").prop("checked", false);
        //$("#i_sam").prop("checked", false);
        //$("#i_batch").prop("checked", false);
        //$("#i_exp").prop("checked", false);
        $("#i_pack").prop("disabled", true);
        //$("#i_pack").prop("checked", false);
         //---------Modifyed By Shubham Maurya 08-11-2023----------------------------------//
        $("#i_cons").prop("checked", false);
        $("#i_stk").prop("checked", false);
        $("#i_batch").prop("checked", false);
        $("#i_serial").prop("checked", false);
        $("#i_qc").prop("checked", false);
        $("#i_capg").prop("checked", false);
        $("#i_wip").prop("checked", false);
        $("#i_pack").prop("checked", false);
        $("#i_sam").prop("checked", false);
        $("#i_exp").prop("checked", false);
        //------------------------------------------//
        //$("#requiredStk_coa").attr("style", "display: none;");
    }
    else {
        $("#i_stk").prop("checked", true);
        $("#i_pack").prop("disabled", false);
    }
    OnClickStockToggle();
}
function OnClickBatchToggle() {
    debugger;
    var batchtgl = "";
    if ($("#i_batch").is(":checked")) {
        batchtgl = 'Y';
    }
    else {
        batchtgl = 'N';
    }
    if (batchtgl == 'Y') {
        $("#i_serial").prop("checked", false);
        $("#i_exp").prop("checked", true);
        $("#i_srvc").prop("checked", false);
        $("#i_stk").prop("checked", true);
        $("#i_cons").prop("checked", false);

        $("#requiredStk_coa").css("display", "inherit");
    }
    else {
        $("#i_exp").prop("checked", false);

        //$("#requiredStk_coa").attr("style", "display: none;");
    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickExpirableToggle() {
    debugger;
    var batchtgl = "";
    if ($("#i_exp").is(":checked")) {
        exptgl = 'Y';
    }
    else {
        exptgl = 'N';
    }
    if (exptgl == 'Y') {

        $("#i_batch").prop("checked", true);
        $("#i_serial").prop("checked", false);
        $("#i_srvc").prop("checked", false);
        $("#i_stk").prop("checked", true);
        $("#i_cons").prop("checked", false);

        $("#requiredStk_coa").css("display", "inherit");
    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickSerialToggle() {
    debugger;
    var serialtgl = "";
    if ($("#i_serial").is(":checked")) {
        serialtgl = 'Y';
    }
    else {
        serialtgl = 'N';
    }
    if (serialtgl == 'Y') {
        $("#i_batch").prop("checked", false);
        $("#i_exp").prop("checked", false);
        $("#i_srvc").prop("checked", false);
        $("#i_stk").prop("checked", true);
        //---------Modifyed By Shubham Maurya 08-11-2023----------------------------------//
        $("#i_pack").prop("disabled", false);
        $("#i_cons").prop("checked", false);
        //----------------------------------//
        $("#requiredStk_coa").css("display", "inherit");
    }
    else {
        $("#i_capg").prop("checked", false);
    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickSalesToggle() {
    var slstgl = "";
    if ($("#i_sls").is(":checked")) {
        slstgl = 'Y';
    }
    else {
        slstgl = 'N';
    }
    if (slstgl == 'Y') {
        $("#i_catalog").prop("disabled", false);
        $("#i_cons").prop("checked", false);

    }
    else {
        $("#i_capg").prop("checked", false);
        $("#i_catalog").prop("disabled", true);
        $("#i_catalog").prop("checked", false);

    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickPurchasable() {
    var Pur = "";
    if ($("#i_pur").is(":checked")) {
        Pur = 'Y';

    }
    else {
        Pur = 'N';
    }
    if (Pur == "N") {
        $("#i_capg").prop("checked", false);
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
//function CheckGroupDependency() {
//    debugger;
//    try {
//        var PGroupID = $('#itemgroupid').val().trim();
//        if (PGroupID != "") {
//            $.ajax(
//                {
//                    type: "POST",
//                    url: "/BusinessLayer/ItemGroupSetup/Check_GroupDependency",
//                    data: { GroupID: PGroupID },
//                    success: function (data) {
//                        debugger;
//                        if (data == 'ErrorPage') {

//                            ErrorPage();
//                            return false;
//                        }
//                        //if (data == "Y") {
//                        //    $("#parent_group_name").attr('disabled', true);
//                        //}
//                        //if (data == "N") {
//                        //    $("#parent_group_name").attr('disabled', false);
//                        //}
//                   },
//                });
//        }
//            return true;

//    } catch (err) {
//        console.log("InsertItemGroupDetail Error : " + err.message);
//    }
//}


function RlvntAccountLinkingMendatory(Action, ToggleName) {
    var Flag = "N";
    debugger;
    if (ToggleName == "i_sls" || ToggleName == "") {
        if ($("#i_sls").is(":checked")) {
            $("#Req_Lcl_Sale_Acc").removeClass("d-none");
            $("#Req_Exp_Sale_Acc").removeClass("d-none");
            $("#Req_Discount_Acc").removeClass("d-none");
           /* $("#Req_cost_of_goods_sold_Acc").removeClass("d-none");*/
            $("#Req_sale_return_Acc").removeClass("d-none");
            if (Action == "Save") {

                Flag = ApplyCheckVallidationJs("Local_Sale_Account", Flag);
                Flag = ApplyCheckVallidationJs("export_sale_account", Flag);
                Flag = ApplyCheckVallidationJs("discount_account", Flag);
               /* Flag = ApplyCheckVallidationJs("cost_of_goods_sold_account", Flag);*/
                Flag = ApplyCheckVallidationJs("sale_return_account", Flag);

            }
        }
        else {
            $("#Req_Lcl_Sale_Acc").addClass("d-none");
            $("#Req_Exp_Sale_Acc").addClass("d-none");
            if ($("#i_pur").is(":checked") == false) {
                $("#Req_Discount_Acc").addClass("d-none");
            }
           /* $("#Req_cost_of_goods_sold_Acc").addClass("d-none");*/
            $("#Req_sale_return_Acc").addClass("d-none");
            RemoveCheckVallidationJs("Local_Sale_Account,export_sale_account,discount_account,sale_return_account");

        }
    }
    if (ToggleName == "i_pur" || ToggleName == "") {
        if ($("#i_pur").is(":checked")) {
            $("#Req_Lcl_Pur_Acc").removeClass("d-none");
            $("#Req_Import_Pur_Acc").removeClass("d-none");
            $("#Req_Discount_Acc").removeClass("d-none");
         /*   $("#Req_ProvisinalPayble_Acc").removeClass("d-none");*/
            $("#Req_Pur_Return_Acc").removeClass("d-none");
            if (Action == "Save") {
                Flag = ApplyCheckVallidationJs("local_purchase_account", Flag);
                Flag = ApplyCheckVallidationJs("import_purchase_account", Flag);
                Flag = ApplyCheckVallidationJs("discount_account", Flag);
                /*Flag = ApplyCheckVallidationJs("provisional_payable_account", Flag);*/
                Flag = ApplyCheckVallidationJs("purchase_return_account", Flag);

            }
        }
        else {
            $("#Req_Lcl_Pur_Acc").addClass("d-none");
            $("#Req_Import_Pur_Acc").addClass("d-none");
            if ($("#i_sls").is(":checked") == false) {
                $("#Req_Discount_Acc").addClass("d-none");
                RemoveCheckVallidationJs("discount_account");
            }
           /* $("#Req_ProvisinalPayble_Acc").addClass("d-none");*/
            $("#Req_Pur_Return_Acc").addClass("d-none");
            RemoveCheckVallidationJs("local_purchase_account,import_purchase_account, purchase_return_account");
        }
    }
    if (ToggleName == "i_stk" || ToggleName == "") {
        if ($("#i_stk").is(":checked")) {
            if (Action == "Save") {
                Flag = ApplyCheckVallidationJs("stock_account", Flag);
            }
        }
        else {
            RemoveCheckVallidationJs("stock_account");
        }
    }
    //if (ToggleName == "i_stk" || ToggleName == "") {
    //    if ($("#i_stk").is(":checked")) {
    //        $("#Req_Stk_Adj_Acc").removeClass("d-none");
    //        $("#Req_Stk_Acc").removeClass("d-none");
    //        if (Action == "Save") {
    //            Flag = ApplyCheckVallidationJs("stock_account", Flag);
    //            Flag = ApplyCheckVallidationJs("stock_adjustment_account", Flag);

    //        }
    //    }
    //    else {
    //        $("#Req_Stk_Adj_Acc").addClass("d-none");
    //        $("#Req_Stk_Acc").addClass("d-none");
    //        RemoveCheckVallidationJs("stock_account,stock_adjustment_account");

    //    }
    //}
    if (ToggleName == "i_capg" || ToggleName == "") {
        if ($("#i_capg").is(":checked")) {
            $("#Req_Depreciation_Acc").removeClass("d-none");
            $("#Req_Assets_Acc").removeClass("d-none");
            if (Action == "Save") {

                Flag = ApplyCheckVallidationJs("depreciation_account", Flag);
                Flag = ApplyCheckVallidationJs("asset_account", Flag);
            }
        }
        else {
            $("#Req_Depreciation_Acc").addClass("d-none");
            $("#Req_Assets_Acc").addClass("d-none");
            RemoveCheckVallidationJs("depreciation_account,asset_account");

        }
    }

    if (Flag == "Y") {
        $("#collapseThree").addClass("show");
        return false;
    } else {
        return true;
    }
}
function RemoveCheckVallidationJs(FieldIds) {

    //if (CheckVallidation(FieldId, "span" + FieldId) == false) {
    //    Flag = "Y";
    var IDs = FieldIds.split(",");
    var count = IDs.length;
    for (var i = 0; i < count; i++) {
        var FieldID = IDs[i].trim();
        $("#" + FieldID).attr("onchange", "");
        $("#" + FieldID).css("border-color", "#ced4da");
        $('[aria-labelledby="select2-' + FieldID + '-container"]').css("border-color", "#ced4da");
        $("#" + "span" + FieldID).text("");
        $("#" + "span" + FieldID).css("display", "none");
    }

    //}
    //return Flag;
}
function ApplyCheckVallidationJs(FieldId, Flag) {

    if (CheckVallidation(FieldId, "span" + FieldId) == false) {
        Flag = "Y";
        $("#" + FieldId).attr('onchange', 'VallidateData("' + FieldId + '")');
        $("#" + FieldId).focus();
        //$("#" + FieldId).on("change", function () { CheckVallidation(FieldId, "span" + FieldId) });
    }
    return Flag;
}
function VallidateData(FieldId) {
    CheckVallidation(FieldId, "span" + FieldId);
}