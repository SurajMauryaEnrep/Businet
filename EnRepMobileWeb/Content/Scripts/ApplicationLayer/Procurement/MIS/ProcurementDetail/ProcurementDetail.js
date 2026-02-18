
$(document).ready(function () {

    OnchangePurchaseBy();
    //BindSupplierList();
    BindSupplierPOList();
    BindProductGrpName();
    BindProductPortfolio();
    //$("#ddl_SupplierCategory").select2();
       /*Commented and modify  by Hina sharma on 15-11-2024 to make multiselect dropdown*/
   // $("#SupplierPortfolio").select2()
    //$('#ddl_SupplierCategory').multiselect();
    //$("#SupplierPortfolio").multiselect();
    //$('#ddl_SupplierName').multiselect();
    //$('#ddl_ItemGroup').multiselect();
    //$('#ItemPortfolio').multiselect();
    BindHSNCode();
    // Initialize all multiselect dropdowns
    Cmn_initializeMultiselect([
        '#ddl_SupplierCategory',
        '#SupplierPortfolio',
        '#ddl_PurchaseType',
        '#ddl_RCMApplicable',
        '#ddl_branch',
        // Add more selectors here as needed
    ]);
    
})
//function BindHSNCode() {
//    $("#HSN_cd").select2({
//        ajax: {
//            url: $("#ajaxUrlGetAutoCompleteSearchHSN").val(),
//            data: function (params) {
//                var queryParameters = {
//                    ddlhsncode: params.term, // search term
//                    Group: params.page      // page number
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",

//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    ErrorPage();
//                    return false;
//                }

//                params.page = params.page || 1;

//                return {
//                    results: $.map(data, function (val) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        }
//    });
//}

function BindHSNCode() {
    Cmn_initializeMultiselect(["#HSN_cd"],
        {
            ajax: {
                url: $("#ajaxUrlGetAutoCompleteSearchHSN").val(),
                data: function (params) {
                    return {
                        ddlhsncode: params.term,
                        Group: params.page
                    };
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
                        results: $.map(data, function (val) {
                            return { id: val.ID, text: val.Name };
                        })
                    };
                }
            }
        }
    );
}

//function BindSupplierPOList() {
//    var Branch = sessionStorage.getItem("BranchID");
//   // $("#ddl_SupplierName").select2()
//    $("#ddl_SupplierName").multiselect({
//        includeSelectAllOption: true,
//        enableFiltering: true,
//        enableCaseInsensitiveFiltering: true,
//        buttonWidth: '100%',
//        buttonClass: 'btn btn-default btn-custom',
//        nonSelectedText: '---All---',
//    //$("#ddl_SupplierName").multiselect({/*Add by Hina sharma on 15-11-2024 to multi select dropdown*/
//        ajax: {
//            url: $("#SuppNameList").val(),
//            data: function (params) {
//                var queryParameters = {
//                    SuppName: params.term, // search term like "a" then "an"
//                    SuppPage: params.page,
//                    BrchID: Branch
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    ErrorPage();
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
function BindSupplierPOList() {
    var Branch = sessionStorage.getItem("BranchID");
    Cmn_initializeMultiselect(
        ["#ddl_SupplierName"],
        {
            ajax: {
                url: $("#SuppNameList").val(),
                data: function (params) {
                    return {
                        SuppName: params.term,
                        SuppPage: params.page,
                        BrchID: Branch
                    };
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
                        results: $.map(data, function (val) {
                            return { id: val.ID, text: val.Name };
                        })
                    };
                }
            }
        }
    );
}

function CalculateAmtOnSearchInput(TableId, colspan) {
    debugger;
    var ValDigit = $("#ValDigit").text();
    var td = "";
    var flag = $("#SumDetSupplierWiseFlag").val();
    if (flag == "SupplierWiseDetail") {
        colspan = 5;
        td = "<td colspan='1' align='right'></td>";
    }
    var PurchaseAmtSpec = 0;
    var PurchaseAmt = 0;
    var TaxAmtSpec = 0;
    var TaxAmt = 0;
    var OcAmtSpec = 0;
    var OcAmt = 0;
    var InvAmtBs = 0;
    var InvAmtSpcs = 0;
    if ($("#" + TableId + " > tbody > tr > td").length > 1) {
        $("#" + TableId + "_next").prev().trigger("click");
        for (var i = 0; i < $("#" + TableId + "_next").prev().text(); i++) {
            $("#" + TableId + " > tbody > tr").each(function () {
                var currentRow = $(this);
                PurchaseAmtSpec = parseFloat(PurchaseAmtSpec) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_PurchaseAmtSpec").text().trim()));
                PurchaseAmt = parseFloat(PurchaseAmt) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_PurchaseAmt").text().trim()));
                TaxAmtSpec = parseFloat(TaxAmtSpec) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_TaxAmtSpec").text().trim()));
                TaxAmt = parseFloat(TaxAmt) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_TaxAmt").text().trim()));
                OcAmtSpec = parseFloat(OcAmtSpec) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_OcAmtSpec").text().trim()));
                OcAmt = parseFloat(OcAmt) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_OcAmt").text().trim()));
                InvAmtBs = parseFloat(InvAmtBs) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_InvoiceAmt").text().trim()));
                InvAmtSpcs = parseFloat(InvAmtSpcs) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_InvoiceAmtSpec").text().trim()));

            });
            $("#" + TableId + "_previous > a").trigger("click");
        }
    }
    if (TableId == "datatable-buttons")
        td = "<td colspan='1' align='right'></td>";
    $("#" + TableId + " > tfoot").html(`
<tr class="total">
                <td colspan="${colspan}" align="right"><strong>${$("#span_Total").text()}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(PurchaseAmtSpec).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(PurchaseAmt).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(TaxAmtSpec).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(TaxAmt).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(OcAmtSpec).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(OcAmt).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(InvAmtSpcs).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(InvAmtBs).toFixed(ValDigit))}</strong></td>           
${td}
            </tr>
`)
}
function CalculateAmtOnSearchInputDetail(TableId, colspan) {
    debugger;
    var td = "";
    var ValDigit = $("#ValDigit").text();
    var flag = $("#SumDetItemWiseFlag").val();
    if (flag == "ItemWiseDetail") {
        td = "<td colspan='1' align='right'></td>";
    }
    var txt_mr_qty = 0;
    var txt_item_rate_spec = 0;
    var txt_item_rate_bs = 0;
    var PurchaseAmtSpec = 0;
    var PurchaseAmt = 0;
    var TaxAmtSpec = 0;
    var TaxAmt = 0;
    var OcAmtSpec = 0;
    var OcAmt = 0;
    var InvAmtBs = 0;
    var InvAmtSpcs = 0;
    if ($("#" + TableId + " > tbody > tr > td").length > 1) {
        $("#" + TableId + "_next").prev().trigger("click");
        for (var i = 0; i < $("#" + TableId + "_next").prev().text(); i++) {
            $("#" + TableId + " > tbody > tr").each(function () {
                var currentRow = $(this);
                txt_mr_qty = parseFloat(txt_mr_qty) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_mr_qty").text().trim()));
                txt_item_rate_spec = parseFloat(txt_item_rate_spec) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_item_rate_spec").text().trim()));
                txt_item_rate_bs = parseFloat(txt_item_rate_bs) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_item_rate_bs").text().trim()));
                PurchaseAmtSpec = parseFloat(PurchaseAmtSpec) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_PurchaseAmtSpec").text().trim()));
                PurchaseAmt = parseFloat(PurchaseAmt) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_PurchaseAmt").text().trim()));
                TaxAmtSpec = parseFloat(TaxAmtSpec) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_TaxAmtSpec").text().trim()));
                TaxAmt = parseFloat(TaxAmt) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_TaxAmt").text().trim()));
                OcAmtSpec = parseFloat(OcAmtSpec) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_OcAmtSpec").text().trim()));
                OcAmt = parseFloat(OcAmt) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_OcAmt").text().trim()));
                InvAmtBs = parseFloat(InvAmtBs) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_InvoiceAmt").text().trim()));
                InvAmtSpcs = parseFloat(InvAmtSpcs) + parseFloat(cmn_ReplaceCommas(currentRow.find("#txt_InvoiceAmtSpec").text().trim()));

            });
            $("#" + TableId + "_previous > a").trigger("click");
        }
    }
    if (TableId == "datatable-buttons")
        td = "<td colspan='1' align='right'></td>";
    $("#" + TableId + " > tfoot").html(`
<tr class="total">
                <td colspan="${colspan}" align="right"><strong>${$("#span_Total").text()}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(txt_mr_qty).toFixed(ValDigit))}</strong ></td >
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(txt_item_rate_spec).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(txt_item_rate_bs).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(PurchaseAmtSpec).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(PurchaseAmt).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(TaxAmtSpec).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(TaxAmt).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(OcAmtSpec).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(OcAmt).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(InvAmtSpcs).toFixed(ValDigit))}</strong></td>
                <td class="num_right"><strong>${cmn_addCommas(parseFloat(InvAmtBs).toFixed(ValDigit))}</strong></td>           
${td}
            </tr>
`)
}

function BtnSearchReport() {
    debugger;
    BindPurchaseDetails();
    
}
function OnClickIconBtn(e) {
    var ItmCode = $(e.target).closest('tr').find("#Hdn_item_id").val();

    ItemInfoBtnClick(ItmCode);

}
function OnchangePurchaseBy() {
    debugger;
   
    var PurchaseBy = $("#ddl_Purchase_by").val();
    var ShowAs;
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    if (PurchaseBy == "I") {
        if (ShowAs == "S") { /*add  div_ddl_RCMApplicable by Hina sharma on 30-06-2025 in all conditions*/
            $("#AllPartials > div,#div_ddl_ItemGroup,#div_ItemPortfolio,#div_HSNCode").css("display", "none");
            $("#Tbl_ProcurementDetailInvoiceWiseList,#div_ddl_SupplierName,#div_SupplierCategory,#div_SupplierPortfolio,#div_ddl_RCMApplicable").css("display", "");
            $("#div_clear").addClass("clear");
        }
        else {
            $("#AllPartials > div,#div_ddl_ItemGroup,#div_ItemPortfolio,#div_HSNCode,#div_ddl_RCMApplicable").css("display", "none");
            $("#Tbl_ProcurementDetailInvoiceWiseList,#div_ddl_SupplierName,#div_SupplierCategory,#div_SupplierPortfolio").css("display", "");
            $("#div_clear").addClass("clear");
        }
        
    }
    else if (PurchaseBy == "S") {
        $("#AllPartials > div,#div_ddl_ItemGroup,#div_ItemPortfolio,#div_HSNCode,#div_ddl_RCMApplicable").css("display", "none");
        $("#PartialMIS_ProcurementDetailSupplierWiseList,#div_ddl_SupplierName,#div_SupplierCategory,#div_SupplierPortfolio").css("display", "");
        $("#div_clear").addClass("clear");
    }
    else if (PurchaseBy == "T") {
        $("#AllPartials > div,#div_ddl_SupplierName,#div_SupplierCategory,#div_SupplierPortfolio,#div_ddl_RCMApplicable").css("display", "none");
        $("#PartialMIS_ProcurementDetailItemWiseList,#div_ddl_ItemGroup,#div_ItemPortfolio,#div_HSNCode").css("display", "");
        $("#div_clear").removeClass("clear");
    }
    else if (PurchaseBy == "G") {
        $("#AllPartials > div,#div_ddl_SupplierName,#div_SupplierCategory,#div_SupplierPortfolio,#div_HSNCode,#div_ddl_RCMApplicable").css("display", "none");
        $("#PartialMIS_ProcurementDetailItemGroupWiseList,#div_ddl_ItemGroup,#div_ItemPortfolio").css("display", "");
        $("#div_clear").removeClass("clear");
    }
    else if (PurchaseBy == "B") {/*add  by Hina Sharma on 04-09-2025 */
        if (ShowAs == "S") { 
            $("#AllPartials > div,#div_ddl_ItemGroup,#div_ItemPortfolio,#div_HSNCode").css("display", "none");
            $("#Tbl_ProcurementDetailInterBranchPurchaseWiseList,#div_ddl_SupplierName,#div_SupplierCategory,#div_SupplierPortfolio,#div_ddl_RCMApplicable").css("display", "");
            $("#div_clear").addClass("clear");
        }
        else {
            $("#AllPartials > div,#div_ddl_ItemGroup,#div_ItemPortfolio,#div_HSNCode,#div_ddl_RCMApplicable").css("display", "none");
            $("#Tbl_ProcurementDetailInterBranchPurchaseWiseList,#div_ddl_SupplierName,#div_SupplierCategory,#div_SupplierPortfolio").css("display", "");
            $("#div_clear").addClass("clear");
        }

    }
    BindPurchaseDetails();  
}
//function BindSupplierList() {
//    var Branch = sessionStorage.getItem("BranchID");
//    $("#ddl_SupplierName").select2({
//        ajax: {
//            url: $("#SuppNameList").val(),
//            data: function (params) {
//                var queryParameters = {
//                    SuppName: params.term, // search term like "a" then "an"
//                    SuppPage: params.page,
//                    BrchID: Branch
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    PO_ErrorPage();
//                    return false;
//                }
//                params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });

//}
//function BindProductGrpName() {
//    //$("#ddl_ItemGroup").select2()
//    $("#ddl_ItemGroup").multiselect({
//        includeSelectAllOption: true,
//        enableFiltering: true,
//        enableCaseInsensitiveFiltering: true,
//        buttonWidth: '100%',
//        buttonClass: 'btn btn-default btn-custom',
//        nonSelectedText: '---All---',/*Commented and modify by Hina on 16-11-2024 to multiselect dropdown*/
//     //$("#ddl_ItemGroup").multiselect({
//        ajax: {
//            url: $("#ItemGrpName").val(),
//            data: function (params) {
//                var queryParameters = {
//                    GroupName: params.term // search term like "a" then "an"
//                    //Group: params.page
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                debugger;
//                params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                    //results: $.map(data, function (val, item) {
//                    //    if (val.ID == "0") {
//                    //        return { id: val.ID, text: "---All---" };
//                    //    } else {
//                    //        return { id: val.ID, text: val.Name };
//                    //    }
//                    //})
//                };
//            }
//        },
//    });
//}
function BindProductGrpName() {
    Cmn_initializeMultiselect(
        ["#ddl_ItemGroup"],
        {
            ajax: {
                url: $("#ItemGrpName").val(),
                data: function (params) {
                    return {
                        GroupName: params.term
                    };
                },
                dataType: "json",
                cache: true,
                delay: 250,
                contentType: "application/json; charset=utf-8",
                processResults: function (data, params) {

                    params.page = params.page || 1;

                    return {
                        results: $.map(data, function (val) {
                            return { id: val.ID, text: val.Name };
                        })
                    };
                }
            }
        }
    );
}
//function BindProductPortfolio() {
//    //$("#ItemPortfolio").select2({
//    $("#ItemPortfolio").multiselect({
//        includeSelectAllOption: true,
//        enableFiltering: true,
//        enableCaseInsensitiveFiltering: true,
//        buttonWidth: '100%',
//        buttonClass: 'btn btn-default btn-custom',
//        nonSelectedText: '---All---',
//        /*Commented and modify by Hina on 16-11-2024 to multiselect dropdown*/
//    //$("#ItemPortfolio").multiselect({
//        ajax: {
//            url: $("#ItemPortfName").val(),
//            data: function (params) {
//                var queryParameters = {
//                    GroupName: params.term // search term like "a" then "an"
//                    //Group: params.page
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                debugger;
//                params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                    //results: $.map(data, function (val, item) {
//                    //    if (val.ID == "0") {
//                    //        return { id: val.ID, text: "---All---" };
//                    //    } else {
//                    //        return { id: val.ID, text: val.Name };
//                    //    }

//                    //})
//                };
//            }
//        },
//    });
//}
function BindProductPortfolio() {
    Cmn_initializeMultiselect(
        ["#ItemPortfolio"],
        {
            ajax: {
                url: $("#ItemPortfName").val(),
                data: function (params) {
                    return {
                        GroupName: params.term
                    };
                },
                dataType: "json",
                cache: true,
                delay: 250,
                contentType: "application/json; charset=utf-8",

                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data, function (val) {
                            return { id: val.ID, text: val.Name };
                        })
                    };
                }
            }
        }
    );
}

function replaceDatatablesIds(convertTo) {
    if (convertTo == "dttbl0") {
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons2")[0].id = "dttbl2";
        $("#datatable-buttons3")[0].id = "dttbl3";
        
    }
    if (convertTo == "datatable-buttons0") {
        $("#dttbl1")[0].id = "datatable-buttons1";
        $("#dttbl2")[0].id = "datatable-buttons2";
        $("#dttbl3")[0].id = "datatable-buttons3";
        
    }
    if (convertTo == "dttbl1") {
        $("#datatable-buttons")[0].id = "dttbl";
        $("#datatable-buttons2")[0].id = "dttbl2";
        $("#datatable-buttons3")[0].id = "dttbl3";

    }
    if (convertTo == "datatable-buttons1") {
        $("#dttbl")[0].id = "datatable-buttons";
        $("#dttbl2")[0].id = "datatable-buttons2";
        $("#dttbl3")[0].id = "datatable-buttons3";

    }
    if (convertTo == "dttbl2") {
        $("#datatable-buttons")[0].id = "dttbl";
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons3")[0].id = "dttbl3";

    }
    if (convertTo == "datatable-buttons2") {
        $("#dttbl")[0].id = "datatable-buttons";
        $("#dttbl1")[0].id = "datatable-buttons1";
        $("#dttbl3")[0].id = "datatable-buttons3";

    }
    if (convertTo == "dttbl3") {
        $("#datatable-buttons")[0].id = "dttbl";
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons2")[0].id = "dttbl2";

    }
    if (convertTo == "datatable-buttons3") {
        $("#dttbl")[0].id = "datatable-buttons";
        $("#dttbl1")[0].id = "datatable-buttons1";
        $("#dttbl2")[0].id = "datatable-buttons2";

    }
  
}
function BindPurchaseDetails() {
    debugger;
    var HSN_code = "";
    var RCMApp = $("#ddl_RCMApplicable").val();
    var PurchaseBy = $("#ddl_Purchase_by").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
   
    var ShowAs;
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var flag = "";
    if (PurchaseBy == "I") {
        if (ShowAs == "S") {
            flag = "Summary";
            $("#div_ddl_RCMApplicable").css("display", "");/*add by Hina sharma on 30-06-2025*/

        }
        else {
            flag = "Detail";
            $("#div_ddl_RCMApplicable").css("display", "none");/*add by Hina sharma on 30-06-2025*/
        }
        //replaceDatatablesIds("dttbl0");
    }
    else if (PurchaseBy == "S") {
        if (ShowAs == "S") {
            flag = "SupplierWiseSummery";
        }
        else {
            flag = "SupplierWiseDetail";
        }
        //replaceDatatablesIds("dttbl1");
    }
    else if (PurchaseBy == "T") {
        if (ShowAs == "S") {
            flag = "ItemWiseSummery";
        }
        else {
            flag = "ItemWiseDetail";
        }
         HSN_code = $("#HSN_cd").val();
        //replaceDatatablesIds("dttbl2");
    }
    else if (PurchaseBy == "G") {
        if (ShowAs == "S") {
            flag = "ItemGroupWiseSummery";
        }
        else {
            flag = "ItemGroupWiseDetail";
        }        
        //replaceDatatablesIds("dttbl3");
    }
    else if (PurchaseBy == "B") {/*add by Hina Sharma on 04-09-2025*/
        if (ShowAs == "S") {
            flag = "InterBranchPurchaseSummary";
            $("#div_ddl_RCMApplicable").css("display", "");

        }
        else {
            flag = "InterBranchPurchaseDetail";
            $("#div_ddl_RCMApplicable").css("display", "none");
        }
        //replaceDatatablesIds("dttbl0");
    }
    debugger;
    var search_prm = {     
        /*Supp_id: $("#ddl_SupplierName").val(),*//*Commented by hina on 15-11-2024 to modify multile select value*/
        Supp_id: $("#hdn_MultiselectSuppName").val(),
       // Inv_type: $("#ddl_PurchaseType").val(),
        Inv_type: Cmn_formatSelectedValues($("#ddl_PurchaseType").val()),
        RCMApp: Cmn_formatSelectedValues($("#ddl_RCMApplicable").val()),
        /* category: $("#ddl_SupplierCategory").val(),*//*Commented by hina on 15-11-2024 to modify multile select value*/
        /*portFolio: $("#SupplierPortfolio").val(),*/
        category: $("#hdn_MultiselectSuppCatgry").val(),
        portFolio: $("#hdn_MultiselectPortfolio").val(),
        //Group: $("#ddl_ItemGroup").val(),
        //Item_PortFolio: $("#ItemPortfolio").val(),
        Group: $("#hdn_MultiselectItmGrpName").val(),/*Commented by hina on 16-11-2024 to modify multile select value*/
        Item_PortFolio: $("#hdn_MultiselectItmportfolio").val(),/*Commented by hina on 16-11-2024 to modify multile select value*/
        From_dt: $("#txtFromdate").val(),
        To_dt: $("#txtTodate").val(),
        HSN_code: Cmn_formatSelectedValues($("#HSN_cd").val()),
        flag: flag,
        brid_list: brid_list,
        //HSN_code: HSN_code,
        //RCMApp: RCMApp /*add by Hina sharma on 30-06-2025*/
    }
    var model = {
        "search_prm": search_prm
    }
    $.ajax({
        type: "post",
        url: "/ProcurementDetail/GetPurchase_DetailsSummery",
        data: { model, PurchaseBy, ShowAs },
        datatype: "json",
        cache: false,
        success: function (data) {
            if (PurchaseBy == "I") {
                $("#Tbl_ProcurementDetailInvoiceWiseList").html(data);
                //replaceDatatablesIds("datatable-buttons0");
                //$("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
                //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetInvoiceWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else if (PurchaseBy == "S") {
                $("#PartialMIS_ProcurementDetailSupplierWiseList").html(data);
                //replaceDatatablesIds("datatable-buttons1");
                //$("#datatable-buttons1_wrapper a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
                //$("#datatable-buttons1_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetInvoiceWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else if (PurchaseBy == "T") {
                $("#PartialMIS_ProcurementDetailItemWiseList").html(data);
                //replaceDatatablesIds("datatable-buttons2");
                //$("#datatable-buttons2_wrapper a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
                //$("#datatable-buttons2_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetInvoiceWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else if (PurchaseBy == "G") {
                $("#PartialMIS_ProcurementDetailItemGroupWiseList").html(data);
                //replaceDatatablesIds("datatable-buttons3");
                //$("#datatable-buttons3_wrapper a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
                //$("#datatable-buttons3_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetInvoiceWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else if (PurchaseBy == "B") {/*add by Hina sharma on 04-09-2025*/
                $("#Tbl_ProcurementDetailInterBranchPurchaseWiseList").html(data);
                //replaceDatatablesIds("datatable-buttons0");
                //$("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
                //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetInvoiceWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
        },
        error: function (xhr) {
            alert('No Valid Data');
        }
    });
}
function onchangeShowAs() {
    BindPurchaseDetails();
}
function OnchngeSuppCategory() {/*Add by Hina sharma on 15-11-2024 to multi select dropdown*/
    debugger;
    var selected = [];
    var abc = "";
    $('#ddl_SupplierCategory option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
        }
        
        else {
            abc += selected[$(this).text()] = "," + $(this).val();
        }
    });
    if (abc == "0") {

        abc = null;
    }
    $("#hdn_MultiselectSuppCatgry").val(abc);

}
function OnchngeSuppPortFolio() {/*Add by Hina sharma on 15-11-2024 to multi select dropdown*/
    debugger;
    var selected = [];
    var abc = "";
    $('#SupplierPortfolio option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
        }
        else {
            abc += selected[$(this).text()] = "," + $(this).val();
        }
    });
    if (abc == "0") {

        abc = null;
    }
    $("#hdn_MultiselectPortfolio").val(abc);

}
function OnchngeSuppName() {/*Add by Hina sharma on 15-11-2024 to multi select dropdown*/
    debugger;
    var selected = [];
    var abc = "";
    $('#ddl_SupplierName option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
        }
        else {
            abc += selected[$(this).text()] = "," + $(this).val();
        }
    });
    if (abc == "0") {

        abc = null;
    }
    $("#hdn_MultiselectSuppName").val(abc);

}
function OnchngeItmGrpName() {/*Add by Hina sharma on 16-11-2024 to multi select dropdown*/
    debugger;
    var selected = [];
    var abc = "";
    $('#ddl_ItemGroup option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
        }
        else {
            abc += selected[$(this).text()] = "," + $(this).val();
        }
    });
    if (abc == "0") {

        abc = null;
    }
    $("#hdn_MultiselectItmGrpName").val(abc);

}
function OnchngeItmPFName() {/*Add by Hina sharma on 16-11-2024 to multi select dropdown*/
    debugger;
    var selected = [];
    var abc = "";
    $('#ItemPortfolio option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
        }
        else {
            abc += selected[$(this).text()] = "," + $(this).val();
        }
    });
    if (abc == "0") {

        abc = null;
    }
    $("#hdn_MultiselectItmportfolio").val(abc);

}
//function OnchngeRCM() {/*Add by Hina sharma on 15-11-2024 to multi select dropdown*/
//    debugger;
//    var selected = [];
//    var abc = "";
//    $('#ddl_RCMApplicable option:selected').each(function () {
//        if (abc == "") {

//            abc += selected[$(this).text()] = $(this).val();
//        }
//        else {
//            abc += selected[$(this).text()] = "," + $(this).val();
//        }
//    });
//    $("#hdnRCMApplicable").val(abc);

//}

/************************************* Invoice Wise Purchase Details  ***************************************/
function OnClickInvAmt(e) {
    var currentRow = $(e.target).closest("tr");
    debugger;
    var search_prm = {
        Supp_Name: currentRow.find("#supp_name").text().trim(),
        Inv_no: currentRow.find("#inv_no").text().trim(),
        Inv_dt: currentRow.find("#inv_dt").text().trim(),
        From_dt: $("#txtFromdate").val(),
        To_dt: $("#txtTodate").val(),
        flag: "InvoiceWiseItem"
    }
    var model = {
        "search_prm": search_prm
    }
    var arr = [];

    var list = {};
    list.Inv_no = currentRow.find("#inv_no").text().trim();
    list.Inv_dt = currentRow.find("#inv_dt").text().trim();
    list.From_dt = $("#txtFromdate").val();
    list.To_dt = $("#txtTodate").val();
    list.flag = "InvoiceWiseItem";
    list.Supp_Name = currentRow.find("#supp_name").text().trim();
    arr.push(list);
    var array = JSON.stringify(arr);
    $("#hdnInvoiceWiseItemDetail").val(array);

    $.ajax({
        type: "post",
        url: "/ProcurementDetail/GetInvoiceWiseItemDetail",
        data: model,
        datatype: "json",
        cache: false,
        success: function (data) {
            $("#ProcurementInvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#tblinvdetails");
            //$("#tblinvdetails_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tblinvdetails_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetInvoiceWiseItemDetailCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        },
    });
}

/************************************* Invoice Wise Purchase Details End  ***************************************/

/************************************* Supplier Wise Purchase Details  ***************************************/
function OnClickSWSummeryInvAmt(e) {
    var currentRow = $(e.target).closest("tr");
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    debugger;
    var search_prm = {
        Supp_Name: currentRow.find("#supp_name").text().trim(),
        Supp_id: currentRow.find("#supp_id").text().trim(),
        currency: currentRow.find("#curr_symbol").text().trim(),
        From_dt: $("#txtFromdate").val(),
        To_dt: $("#txtTodate").val(),
        flag: "SupplierWiseInvoice",
        brid_list: brid_list
    }
    var model = {
        "search_prm": search_prm
    }
    var arr = [];
    var list = {};
    list.currency = currentRow.find("#curr_symbol").text().trim();
    list.Supp_id = currentRow.find("#supp_id").text().trim();
    list.From_dt = $("#txtFromdate").val();
    list.To_dt = $("#txtTodate").val();
    list.flag = "SupplierWiseInvoice";
    list.Supp_Name = currentRow.find("#supp_name").text().trim(),
    arr.push(list);
    var array = JSON.stringify(arr);
    $("#hdnSupplierWiseInvoiceDetail").val(array);

    $.ajax({
        type: "post",
        url: "/ProcurementDetail/GetSupplierWiseInvoice",
        data: model,
        datatype: "json",
        cache: false,
        success: function (data) {
            $("#ProcurementSupplierWiseInvoiceSummaryPopUp").html(data);
            cmn_apply_datatable("#tblProSuppWiseInvSummary");
            //$("#tblProSuppWiseInvSummary_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tblProSuppWiseInvSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetSupplierWiseInvoiceCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        },
    });
}
function OnClickSWInvoiceInvAmt(e) {
    var currentRow = $(e.target).closest("tr");
    debugger;
    var search_prm = {
        //Supp_Name: $("#SWSupplierName").val(),
        Supp_Name: currentRow.find("#supp_name").text().trim(),
        Inv_no: currentRow.find("#inv_no").text().trim(),
        Inv_dt: currentRow.find("#inv_dt").text().trim(),
        //currency: $("#SWCurrency").val(),
        currency: currentRow.find("#curr_symbol").text().trim(),
        From_dt: $("#txtFromdate").val(),
        To_dt: $("#txtTodate").val(),
        flag: "SupplierWiseInvoiceItem",
        brid_list: currentRow.find("#hd_brid").text().trim(),
    }
    var model = {
        "search_prm": search_prm
    }
    var arr = [];
    var list = {};
    list.currency = $("#SWCurrency").val();
    list.From_dt = $("#txtFromdate").val();
    list.To_dt = $("#txtTodate").val();
    list.flag = "SupplierWiseInvoiceItem";
    list.Supp_Name = $("#SWSupplierName").val();
    list.Inv_no = currentRow.find("#inv_no").text().trim();
    list.Inv_dt = currentRow.find("#inv_dt").text().trim();
    arr.push(list);
    var array = JSON.stringify(arr);
    $("#hdnSupplierWiseInvoiceItemDetail").val(array);

    $.ajax({
        type: "post",
        url: "/ProcurementDetail/GetSupplierWiseInvoiceItem",
        data: model,
        datatype: "json",
        cache: false,
        success: function (data) {
            $("#ProcurementSupplierWiseInvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#tblProSuppWiseInvDetail");
            //$("#tblProSuppWiseInvDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tblProSuppWiseInvDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetSupplierWiseInvoiceItemCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        },
    });
}

/************************************* Supplier Wise Purchase Details End  ***************************************/

/************************************* Item Wise Purchase Details End  ***************************************/
function OnClickIWSummeryInvAmt(e) {
    var currentRow = $(e.target).closest("tr");
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    debugger;
    var search_prm = {
        ItemID: currentRow.find("#Hdn_item_id").val(),
        ItemName: currentRow.find("#item_name").text().trim(),
        Uom: currentRow.find("#uom_alias").text().trim(),
        currency: currentRow.find("#curr_symbol").text().trim(),
        Qty: currentRow.find("#item_qty").text().trim(),
        From_dt: $("#txtFromdate").val(),
        To_dt: $("#txtTodate").val(),
        flag: "ItemWiseInvoice",
        brid_list: brid_list
    }
    var model = {
        "search_prm": search_prm
    }
    var arr = [];
    var list = {};
    list.ItemID = currentRow.find("#Hdn_item_id").val();
    list.ItemName = currentRow.find("#item_name").text().trim();
    list.Uom = currentRow.find("#uom_alias").text().trim();
    list.currency = currentRow.find("#curr_symbol").text().trim();
    list.Qty = currentRow.find("#item_qty").text().trim();
    list.From_dt = $("#txtFromdate").val();
    list.To_dt = $("#txtTodate").val();
    list.flag = "ItemWiseInvoice";
    arr.push(list);
    var array = JSON.stringify(arr);
    $("#hdnItemWiseInvoiceDetail").val(array);

    $.ajax({
        type: "post",
        url: "/ProcurementDetail/GetItemWiseInvoice",
        data: model,
        datatype: "json",
        cache: false,
        success: function (data) {
            $("#ProcurementItemWiseInvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#tblProItemWiseInvDetail");
            //$("#tblProItemWiseInvDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tblProItemWiseInvDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetItemWiseInvoiceCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        },
    });
}

/************************************* Item Wise Purchase Details End  ***************************************/

/************************************* Item Group Wise Purchase Details  ***************************************/

function OnClickIGWSummeryInvAmt(e) {
    var currentRow = $(e.target).closest("tr");
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    debugger;
    var search_prm = {
        Group: currentRow.find("#item_grp_id").text().trim(),
        GroupName: currentRow.find("#item_group_name").text().trim(),
        currency: currentRow.find("#curr_symbol").text().trim(),
        From_dt: $("#txtFromdate").val(),
        To_dt: $("#txtTodate").val(),
        flag: "ItemGroupWiseItemSummery",
        brid_list: brid_list
    }
    var model = {
        "search_prm": search_prm
    }
    var arr = [];
    var list = {};
    list.Group = currentRow.find("#item_grp_id").text().trim();
    list.GroupName = currentRow.find("#item_group_name").text().trim();
    list.currency = currentRow.find("#curr_symbol").text().trim();
    list.From_dt = $("#txtFromdate").val();
    list.To_dt = $("#txtTodate").val();
    list.flag = "ItemGroupWiseItemSummery";
    arr.push(list);
    var array = JSON.stringify(arr);
    $("#hdnItemGroupWiseItemDetail").val(array);

    $.ajax({
        type: "post",
        url: "/ProcurementDetail/GetItemGroupWiseItem",
        data: model,
        datatype: "json",
        cache: false,
        success: function (data) {
            $("#ProcurementItemGroupWiseItemDetailPopUp").html(data);
            cmn_apply_datatable("#tblProItemGrpWiseItemDetail");
            //$("#tblProItemGrpWiseItemDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tblProItemGrpWiseItemDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetItemGroupWiseItemCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        },
    });
}
function OnClickIGWItemInvAmt(e) {
    var currentRow = $(e.target).closest("tr");
    var curr = "";
    var ItemQty = "";
    var PurchaseBy = $("#ddl_Purchase_by").val();
    if ($("#ProcurementMISOrderDetail").is(":checked") && PurchaseBy=="G") {
        curr = currentRow.find("#curr_symbol").text();
        ItemQty = currentRow.find("#txt_mr_qty").text().trim();
    }
    else {
        curr = $("#IGWP_Currency").val()
        ItemQty = currentRow.find("#item_qty").text().trim();
    }
    debugger;
    var search_prm = {
        ItemID: currentRow.find("#Hdn_item_id").val(),
        ItemName: currentRow.find("#item_name").text().trim(),
        Uom: currentRow.find("#uom_alias").text().trim(),
        currency: curr,
        Qty: ItemQty,
        From_dt: $("#txtFromdate").val(),
        To_dt: $("#txtTodate").val(),
        flag: "ItemWiseInvoice"
    }
    var model = {
        "search_prm": search_prm
    }
    var arr = [];
    var list = {};
    list.ItemID = currentRow.find("#Hdn_item_id").val();
    list.ItemName = currentRow.find("#item_name").text().trim();
    list.Uom = currentRow.find("#uom_alias").text().trim();
    list.currency = curr;
    list.Qty = ItemQty;
    list.From_dt = $("#txtFromdate").val();
    list.To_dt = $("#txtTodate").val();
    list.flag = "ItemWiseInvoice";
    arr.push(list);
    var array = JSON.stringify(arr);
    $("#hdnItemGroupWiseItemInvoiceDetail").val(array);

    $.ajax({
        type: "post",
        url: "/ProcurementDetail/GetItemGroupWiseItemInvoice",
        data: model,
        datatype: "json",
        cache: false,
        success: function (data) {
            $("#ProcurementItemGroupWiseInvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#tblProItemGrpWiseInvDetail");
            //$("#tblProItemGrpWiseInvDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tblProItemGrpWiseInvDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintPD" onclick="GetItemGroupWiseItemInvoiceCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        },
    });
}
/************************************* Item Group Wise Purchase Details End  ***************************************/
function GetInvoiceWiseCSV() {
    debugger;
    var HSNcode = "";
    var PurchaseBy = $("#ddl_Purchase_by").val();
    var ShowAs;
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var flag = "";
    if (PurchaseBy == "I") {
        if (ShowAs == "S") {
            flag = "Summary";
        }
        else {
            flag = "Detail";
        }
    }
    else if (PurchaseBy == "S") {
        if (ShowAs == "S") {
            flag = "SupplierWiseSummery";
        }
        else {
            flag = "SupplierWiseDetail";
        }
    } 
    else if (PurchaseBy == "T") {
        if (ShowAs == "S") {
            flag = "ItemWiseSummery";
        }
        else {
            flag = "ItemWiseDetail";
        }
        HSNcode = $("#HSN_cd").val();

    }
    else if (PurchaseBy == "G") {
        if (ShowAs == "S") {
            flag = "ItemGroupWiseSummery";
        }
        else {
            flag = "ItemGroupWiseDetail";
        }
    }
    if (PurchaseBy == "B") {
        if (ShowAs == "S") {
            flag = "InterBranchPurchaseSummary";
        }
        else {
            flag = "InterBranchPurchaseDetail";
        }
    }
    var Supp_id = $("#hdn_MultiselectSuppName").val();
    var Inv_type = $("#ddl_PurchaseType").val();
    var category = $("#hdn_MultiselectSuppCatgry").val();
    var portFolio = $("#hdn_MultiselectPortfolio").val();
    var Group = $("#hdn_MultiselectItmGrpName").val();
    var Item_PortFolio = $("#hdn_MultiselectItmportfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var RCMApp = $("#ddl_RCMApplicable").val();
    var flag = flag
    var HSN_code = HSNcode
    var arr = [];

    var list = {};
    list.Supp_id = Supp_id
    list.Inv_type = Inv_type
    list.category = category
    list.portFolio = portFolio
    list.Group = Group
    list.Item_PortFolio = Item_PortFolio
    list.From_dt = From_dt
    list.To_dt = To_dt
    list.flag = flag
    list.PurchaseBy = PurchaseBy
    list.HSN_code = HSN_code
    list.RCMApp = RCMApp /* Added by Suraj Maurya on 21-07-2025 */
    arr.push(list);

    $("#hdnCSVPrint").val("CsvPrint");
    var array = JSON.stringify(arr);

    if (PurchaseBy == "B") {
        let csv = $("#hdn_ibpPrcDtlTaxClmnJson").text();
        $("#hdn_prcDtlTaxClmnJson_forCsv").val(csv);
    }
    else {
        let csv = $("#hdn_prcDtlTaxClmnJson").text();
        $("#hdn_prcDtlTaxClmnJson_forCsv").val(csv);
    }
    
    $("#HdnCsvData").val(array);
    $("#hdnInsightBtn").val("");
    $('form').submit();
}
function GetInvoiceWiseItemDetailCSV() {
    $("#hdnInsightBtn").val("");
    $("#hdnInsightBtn").val("InvoiceWiseItemDetail");
    $("#hdnCSVPrint").val("CsvPrint");
    $('form').submit();
}
function GetSupplierWiseInvoiceCSV() {
    $("#hdnInsightBtn").val("");
    $("#hdnInsightBtn").val("SupplierWiseInvoice");
    $("#hdnCSVPrint").val("CsvPrint");
    $('form').submit();
}
function GetSupplierWiseInvoiceItemCSV() {
    $("#hdnInsightBtn").val("");
    $("#hdnInsightBtn").val("SupplierWiseInvoiceItem");
    $("#hdnCSVPrint").val("CsvPrint");
    $('form').submit();
}
function GetItemWiseInvoiceCSV() {
    $("#hdnInsightBtn").val("");
    $("#hdnInsightBtn").val("ItemWiseInvoice");
    $("#hdnCSVPrint").val("CsvPrint");
    $('form').submit();
}
function GetItemGroupWiseItemCSV() {
    $("#hdnInsightBtn").val("");
    $("#hdnInsightBtn").val("ItemGroupWiseItem");
    $("#hdnCSVPrint").val("CsvPrint");
    $('form').submit();
}
function GetItemGroupWiseItemInvoiceCSV() {
    $("#hdnInsightBtn").val("");
    $("#hdnInsightBtn").val("ItemGroupWiseItemInvoice");
    $("#hdnCSVPrint").val("CsvPrint");
    $('form').submit();
}
