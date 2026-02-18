/************************************************
Javascript Name:ItemSetup
Created By:Mukesh
Created Date: 25-09-2020
Description: This Javascript use for the Item setup many function

Modified By:
Modified Date:
Description:
*************************************************/

$(document).ready(function () {
    debugger;
    $("#IDInterBranch_pur_coa").select2();
    $("#IDInterBranch_sls_coa").select2();
    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");
    var param_status = $("#param_status").val();
    if (param_status == "Y") {
        $("#RequiredRefNo").css("display", "inherit");
    }
    else {
        $("#RequiredRefNo").attr("style", "display: none;");
    }
   
    debugger;
    var SubItemToggle = "";
    if ($("#SubItem").is(":checked")) {
        SubItemToggle = 'Y';
    }
    else {

        SubItemToggle = 'N';
    }
    if (SubItemToggle == 'Y') {
        $("#SubItemAccourdion").css("display", "block");
        //$("#collapseThirteen").removeClass('collapse');
    }
    else {
        $("#SubItemAccourdion").css("display", "none");
        //$("#collapseThirteen").addClass('collapse');
    }
   // $("#itemAttrList").select2();
    $("#itemAttrList").select2({
        //sorter: data => data.sort((a, b) => a.text.toLowerCase() > b.text.toLowerCase() ? 0 : -1),
        matcher: matchCustom

        /* sorter: data => data.sort((a, b) => a.text.toLowerCase() > b.text.toLowerCase() ? 0 : -1)*/
        //matcher: function (term, option) {

    });
    $("#wh_id").select2();
    ReadBranchList();
    if ($("#hdGrpID").val() != "") {
        //$('#group_name').val($("#hdGrpID").val()).trigger('change.select2');
    }
    if ($("#hdHsnID").val() != "") {
        $('#HSN_cd').val($("#hdHsnID").val()).trigger('change.select2');
    }
    $("#spanItemName").css("display", "block");
    $('#AttriTBody').on('click', '#AttrDlt', function () {
        debugger;
        // Getting all the rows next to the row 
        // containing the clicked button 
        var child = $(this).closest('tr').nextAll();
        // Iterating across all the rows  
        // obtained to change the index 
        child.each(function () {
            // Getting <tr> id. 
            debugger;
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
        // Removing the current row. 
        $(this).closest('tr').remove();
        var AttName = $(this).closest('tr')[0].cells[1].innerHTML;
        //var hdAttrid = $(this).closest('tr').find("#hdAttrid").text();
        //var hdAttrname = $(this).closest('tr').find("#hdAttrname").text();
        //$("#itemAttrList option[value=" + AttName + "]").show();
        //$("#itemAttrList").append('<option value=' + hdAttrid + ' >' + hdAttrname + '</option>');
        //$("#itemAttrList").select2({
            
        //   // sortResults: data => data.sort((a, b) => a.text.localeCompare(b.text)),
        //    //sorter: function (data) {
        //        //debugger;
        //        sorter: data => data.sort((a, b) => a.text.toLowerCase() > b.text.toLowerCase() ? 0 : -1)
        //    //}            
        //});
        $('#itemAttrList').val("0").prop('selected', true);
        $('#itemAttrValList').empty().append('<option value="0" selected="selected">---Select---</option>');
        // Decreasing total number of rows by 1. 
        //rowIdx--;
    }); 
    $('#CustomerDetailsTbl tbody').on('click', '.deleteIcon', function () {
        // debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();

        // debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var CustID = "";
        CustID = $(this).closest('tr').find("#CustomerName_" + SNo).val();
        ShowCustomerList(CustID);

    }); 
    $('#SupplierDetailsTbl tbody').on('click', '.deleteIcon', function () {
        // debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();

        // debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var SuppID = "";
        SuppID = $(this).closest('tr').find("#SupplierName_" + SNo).val();
        ShowSupplierList(SuppID);

    }); 
    $('#PortfolioTbl tbody').on('click', '.deleteIcon', function () {
        // debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();

        // debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var PrfID = "";
        PrfID = $(this).closest('tr').find("#PortfolioName_" + SNo).val();
        ShowPortfolioList(PrfID);

    }); 
    $('#VehicleTbl tbody').on('click', '.deleteIcon', function () {
        // debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();

        // debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var PrfID = "";
        PrfID = $(this).closest('tr').find("#VehicleName_" + SNo).val();
        ShowVehicleList(PrfID);

    });
    $('#NewSubItemtable tbody').on('click', '.deleteIcon', function () {
        // debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();

        SerialNoAfterDelete();
    });
    BindSearchableDDLListData();
    //$("#act_stats").prop("checked", true);
    jQuery(document).trigger('jquery.loaded');
    BindDLLCustomerList();
    BindDLLSupplierList();
    BindDLLPortfolioList();
    BindDLLVehicleList();
    RlvntAccountLinkingMendatory("","");  
    $("#file-1").on("change", function () {
        setTimeout('Cmn_OnchangeFileInput()', 700);
    });
    if ($("#i_srvc").is(":checked")) {
        $("#requiredBaseUOMId").attr("style", "display: none;");
        $("#requiredPurchaseUOMId").attr("style", "display: none;");
        $("#requiredSaleUOMId").attr("style", "display: none;");
    }
    else {
        $("#requiredBaseUOMId").css("display", "inherit");
        $("#requiredPurchaseUOMId").css("display", "inherit");
        $("#requiredSaleUOMId").css("display", "inherit");
    }
    if ($("#i_stk").is(":checked")) {
        $("#requiredStk_coa").css("display", "inherit");
    }
    else {
        $("#requiredStk_coa").attr("style", "display: none;");
    }

    onpasteValueDiscard();

    $("#i_exp").on("change", function ()
    {
        if ($("#i_exp").is(":checked")) {
            $("#ExpiryAlertDt").val(30);
        }
        else {
            $("#ExpiryAlertDt").val(0);
        }
    })
});
function onpasteValueDiscard() {
    $('#item_name').on('paste', function (e) {
        debugger;
        var data = e.originalEvent.clipboardData.getData('text');
        if (data.includes("_") || data.includes(",")) {

            $("#item_name").css("border-color", "red");
            $("#spanItemName").text($("#span_AreNotAllowedInItemName").text());
           // document.getElementById("spanItemName").innerText = "(_ ,) are Not Allowed in Item Name."
            $("#spanItemName").css("font-weight", "bold");
            $("#spanItemName").css("display", "block");
            return false;
        }
        //IE9 Equivalent ==> window.clipboardData.getData("Text");   
    });
}
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#NewSubItemtable >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
    });
};
function AddNewSubItem() {
    debugger;
    var ErrorFlag = "N";
    if ($("#SubItemName").val() == null || $("#SubItemName").val() == "") {
        $('#SubItemNameErrorMsg').text($("#valueReq").text());
        $("#SubItemNameErrorMsg").css("display", "block");
        $("#SubItemName").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SubItemNameErrorMsg").css("display", "none");
        $("#SubItemName").css("border-color", "#ced4da");
    }
    var SubItemName = $("#SubItemName").val().trim().toUpperCase();
    $("#NewSubItemtable >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var TAndC = currentRow.find("#subItem").text().trim().toUpperCase();
        if (SubItemName == TAndC) {
            $('#SubItemNameErrorMsg').text($("#valueduplicate").text());
            $("#SubItemNameErrorMsg").css("display", "block");
           // document.getElementById("SubItemNameErrorMsg").innerHTML = $("#valueduplicate").text();
            $("#SubItemName").css("border-color", "red");
            ErrorFlag = "Y";
            //return false;
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    var SrNo = $("#NewSubItemtable tbody tr").length;
    SrNo = parseInt(SrNo) + 1;

    var subItem = $("#SubItemName").val();
    var HdnSubItemName = $("#HdnSubItemName").val();
    var HdnEdit = $("#HdnEdit").val();
    var rowIdx = 0;
    if (HdnEdit == "Edit") {
        $("#NewSubItemtable >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var SubItemId = currentRow.find("#SubItemId").text();
            if (HdnSubItemName == SubItemId) {
                $(this).closest('tr').remove();
            }
        });
    }
    var deletetext = $("#Span_Delete_Title").text();
    var Edit = $("#Edit").text();
    $("#NewSubItemtable tbody").append(`<tr id="R${++rowIdx}">
    <td id="srno">${SrNo}</td>
    <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="deleteIcon" title="${deletetext}"></i></td>
    <td class="center edit_icon"> <i class="fa fa-edit editAttrName" onclick="OnClickEditBtn(event);" aria-hidden="true" id="EditSubItem" title="${Edit}"></i></td>
    <td id="subItem">${subItem}</td>
 <td hidden="hidden" id="SubItemId">${SrNo}</td>
 </tr>`)
    $("#SubItemName").val(null);
}
function OnClickEditBtn(e) {
    debugger;
    $("#NewSubItemtable >tbody >tr").on('click', "#EditSubItem", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            var SubItemId = clickedrow.children("#SubItemId").text();
            var subItem = clickedrow.children("#subItem").text();
            $("#HdnSubItemName").val(SubItemId);
            $("#SubItemName").val(subItem);
            $('#Btn_AddNew').css('display', 'none');
            $('#ResetTD').css('display', 'block');
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
}
function ResetTaxDetails() {
    debugger;
    var ValidInfo = "N";
    if ($('#SubItemName').val() == "" || $('#SubItemName').val() == null) {
        ValidInfo = "Y";
        $('#SubItemNameErrorMsg').text($("#valueReq").text());
        $("#SubItemNameErrorMsg").css("display", "block");
        $("#SubItemName").css("border-color", "red");
    } else {
        $("#SubItemNameErrorMsg").css("display", "none");
        $("#SubItemName").css("border-color", "#ced4da");
    }

    if (ValidInfo == "Y") {
        return false;
    }
    var SubItemName = $("#SubItemName").val().trim().toUpperCase();
    var HdnTextBoxId = $("#HdnSubItemName").val();
    $("#NewSubItemtable >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var TAndC = currentRow.find("#subItem").text().trim().toUpperCase();
        var HdnId = currentRow.find("#SubItemId").text();
        if (SubItemName == TAndC) {
            debugger;
            if (HdnTextBoxId == HdnId) {
                debugger;

            }
            else {
                $('#SubItemNameErrorMsg').text($("#valueduplicate").text());
                $("#SubItemNameErrorMsg").css("display", "block");
                $("#SubItemName").css("border-color", "red");
                ValidInfo = "Y";
            }
        }
    });
    if (ValidInfo == "Y") {
        return false;
    }
    debugger;
    var HdnSubItemId = $("#HdnSubItemName").val();
    $("#NewSubItemtable > tbody > tr").each(function () {
        debugger;
        var currentRow = $(this);
        var SubItemName = $("#SubItemName").val();
        var HdnHdnSubItemId1 = currentRow.find("#SubItemId").text();
        if (HdnSubItemId == HdnHdnSubItemId1) {
            debugger;
            currentRow.find("#subItem").text(SubItemName);
            $('#Btn_AddNew').css('display', 'block');
            $('#ResetTD').css('display', 'none');
            $("#SubItemName").val("");
        }
    });
}
function OnClickSubItemToggle() {
    debugger;
    var SubItemToggle = "";
    if ($("#SubItem").is(":checked")) {
        SubItemToggle = 'Y';
    }
    else {

        SubItemToggle = 'N';
    }
    if (SubItemToggle == 'Y') {
        $("#SubItemAccourdion").css("display", "block");
        $("#collapseThirteen").removeClass('collapse');
        $("#i_cons").prop("checked", false);
    }
    else {
        $("#SubItemAccourdion").css("display", "none");
        $("#collapseThirteen").addClass('collapse');
        $('#NewSubItemtable tbody tr').remove();
    }
}
function onchangeSubItem() {
    $("#SubItemNameErrorMsg").css("display", "none");
    $("#SubItemName").css("border-color", "#ced4da");
}
function tofilterAttrArrey() {
    debugger;
    var options = new Array();
    $("#ItmAttributeListTbl tbody tr").each(function () {
        debugger;
        var row = $(this);
        var hdAttrid = row.find("#hdAttrid").text();
        options.push(hdAttrid);
    })
    return options;
}
function matchCustom(params, data) {
    debugger;
    // If there are no search terms, return all of the data
    var options = tofilterAttrArrey();
   
    for (var i = 0; i < options.length; i++) {
        if (data != null) {
            if (options[i] == data.id) {
                return null;
            }
        }
       
    }
    //return data;
    if ($.trim(params.term) === '') {
        return data;
    }

    // Do not display the item if there is no 'text' property
    if (data != null) {
        if (typeof data.text === 'undefined') {
            return null;
        }
    }
    
    // `params.term` should be the term that is used for searching
    // `data.text` is the text that is displayed for the data object
    if (data.text.toLowerCase().indexOf(params.term.toLowerCase()) > -1) {
        var modifiedData = $.extend({}, data, true);
        debugger;
        
        return modifiedData;
        // You can return modified objects from here
        // This includes matching the `children` how you want in nested data sets
       

    }

    // Return `null` if the term should not be displayed
    return null;
}
function ReadBranchList() {
    debugger;
    var Branches = new Array();
    $("#CustBrDetail TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var Branch = {};
        Branch.Id = row.find("#hdCustomerBranchId").val();
        var checkBoxId = "#cust_act_stat_" + Branch.Id;
        if (row.find(checkBoxId).is(":checked")) {
            Branch.BranchFlag = "Y";
        }
        else {
            Branch.BranchFlag = "N";
        }
        Branches.push(Branch);
    });
    debugger;
    var str = JSON.stringify(Branches);
    $('#hdCustomerBranchList').val(str);
}
function QtyFloatValueonly(el, evt) {
    debugger
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function QtyFloatValueonlyForWeight(el, evt) {
    debugger
    if (Cmn_FloatValueonly(el, evt, "0") == false) {
        return false;
    }
    return true;
}
function AmountFloatValueOnly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    return true;
}
function currencyFormat1(id) {
    var x;
    x = id.toString();
    var lastThree = x.substring(x.length - 3);
    var otherNumbers = x.substring(0, x.length - 3);
    if (otherNumbers != '')
        lastThree = ',' + parseFloat(lastThree).toFixed($("#RateDigit").text());
    var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    return res;
}
function BindSearchableDDLListData() {
    $("#group_name").select2({
        ajax: {
            url: $("#ajaxUrlGetAutoCompleteSearchSuggestion").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlgroup_name: params.term, // search term like "a" then "an"
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
    debugger;
    $("#HSN_cd").select2({
        ajax: {
            url: $("#ajaxUrlGetAutoCompleteSearchHSN").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlhsncode: params.term, // search term like "a" then "an"
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
    $('#IDInterBranch_sls_coa').select2({
        ajax: {
            url: $("#LocalSaleAccount").val(),
            data: function (params) {
                return {
                    ddlcoa_name: params.term || "", // search term
                    Group: params.page || 1         // page number
                };
            },
            dataType: "json",
            delay: 250,
            cache: true,
            processResults: function (data, params) {
                params.page = params.page || 1;

                if (data === 'ErrorPage') {
                    ErrorPage();
                    return { results: [] };
                }

                return {
                    results: $.map(data, function (val) {
                        return { id: val.ID, text: val.Name };
                    }),
                    pagination: {
                        more: (data.length >= 10) // adjust according to API
                    }
                };
            }
        }
    });

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
function BindDDLListData() {
    GetUOM();
    Getwarehouse();
    Getbin();
    Getport();
}
function ItemNameKeyPress() {
        $("#item_name").attr("style", "border-color: #ced4da;");
        $("#spanItemName").attr("style", "display: none;");
}
function InActResKeyPress() {
    $("#inact_reason").attr("style", "border-color: #ced4da;");
    $("#SpanItminactreason").attr("style", "display: none;");
}
function InterValueonly(event) {
    if (event.which == 46) {
            if ($(this).val().indexOf('.') != -1) {
                return false;
            }
        }
    if (event.which != 8 && event.which != 0 && event.which != 46 && (event.which < 48 || event.which > 57)) {
            return false;
        }
}
function base_uom_cdOnChange() {
    debugger;
    //$("#base_uom_cd").on('change', function () {
    var UomID = $('#base_uom_cd').val();
        var PurID = $('#pur_uom_cd').val();
        var SalID = $('#sl_uom_cd').val();
        $("#spanBaseUom").attr("style", "display: none;");
        $("#base_uom_cd").attr("style", "border-color: #ced4da;");
        if (PurID == 0) {
            $('#pur_uom_cd').val(UomID);
            $("#spanPurUom").attr("style", "display: none;");
            $("#pur_uom_cd").attr("style", "border-color: #ced4da;");
        }
        if (SalID == 0) {
            $('#sl_uom_cd').val(UomID);
            $("#spanSalUom").attr("style", "display: none;");
            $("#sl_uom_cd").attr("style", "border-color: #ced4da;");
        }

    //});
};
function OnchangeAttribute() {
    debugger;
    var AttributeID = $("#itemAttrList").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemDetail/GetAttributeValue",/*Controller=ItemSetup and Fuction=GetAttributeValue*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: {AttributeID: AttributeID},/*Registration pass value like model*/
        success: function (data) {
            debugger;
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].attr_val_id + '">' + arr.Table[i].attr_val_name + '</option>';
                }
                $("#itemAttrValList").html(s);
            }
        },
        error: function (Data) {
        }
    });
};
function OnchangeImage() {
    debugger;
    $('#container').empty();
    var $images = $('#container img');
    var Len = 0;
    Len = $images.length;
    if ($images.length > 5) {
        $("#file").val(null);
        $('#SpanItmImgError').text("Maximum allowed limit of 5");
        $("#SpanItmImgError").attr("style", "display: block; font-size:18px;");
    }
    else {
        var fp = $('#file');
        var lg = fp[0].files.length;
        var files = fp[0].files
        if (lg > 0) {
            Len = Len + lg;
            if (Len > 5) {
                $("#file").val(null);
                $('#SpanItmImgError').text("Maximum allowed limit of 5");
                $("#SpanItmImgError").attr("style", "display: block; font-size:18px;");
            }
            else {
                $("#SpanItmImgError").attr("style", "display: none;");
                for (var i = 0; i < lg; i++) {
                    var f = files[i];
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        debugger;
                        //var origin = window.location.origin + "/Content/Images/delete_red1.png";
                        //$('#container').append('<div class="slide"><img class="btn-delete" onclick="alertTest(this)" src="' + origin + '"/><img id="ItmImgTest" src="' + e.target.result + '" width="175px" height="175px" style="border-radius:5px;" /></div>')
                        $('#container').append('<div><img src="' + e.target.result + '" width="175px" height="175px" style="border-radius:5px;" /></div>')
                        debugger;
                    }
                    reader.readAsDataURL(f);
                    //<span class="deleteBtn" onclick="deleteFunction()">delete image</span><br />
                }
            }
        }
    }
    
};
function alertTest(evnt) {
    debugger;
    evnt.parentElement.remove();
    var $images = $('#container #ItmImgTest');
}
function AddAttribute() {
    var rowIdx = 0;
    debugger;

    var AttributeID = $("#itemAttrList").val();
    var AttributeValue = $("#itemAttrValList").val();
    var Title = $("#hf_deletetitle").val();
    if (AttributeID != "0" && AttributeValue !== "0") {
        $('#AttriTBody').append(`<tr id="R${++rowIdx}">
<td class="red center" id="AttrDlt"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${Title}"></i></td>
<td id="hdAttrname">${$("#itemAttrList option:selected").text()}</td>
<td hidden="hidden" id="hdAttrid">${$("#itemAttrList").val()}</td>
<td id="hdAttrValName">${$("#itemAttrValList option:selected").text()}</td>
<td hidden="hidden" id="hdAttrValId">${$("#itemAttrValList").val()}</td>

              </tr>`);
        debugger;
        //$('#select2-itemAttrList-result-b45s-115').hide().val(AttributeID);
        //$('#itemAttrList').empty().append('<option value="0" selected="selected">---Select---</option>');
        //////////$("#itemAttrList option[value=" + $("#itemAttrList").val() + "]").remove();
   /*  *//*  *//* $("#itemAttrList").select2();*/
       // $("#itemAttrList option[value=" + AttributeID + "]").hide();
        //$('#itemAttrList').val("0").prop('selected', true).trigger('change');
        $("#itemAttrList").val("0").trigger('change').prop('selected', true);
        $('#itemAttrValList').empty().append('<option value="0" selected="selected">---Select---</option>');

        //$("#itemAttrList option[value=" + $("#itemAttrList").val() + "]").hide();

        //$('#itemAttrList').empty().append('<option value="0" selected="selected">---Select---</option>');
    }
}
function SaveBtnClick() {
    //$('#btn_save').click(function () {
    debugger;
    //var fp = $('#file-1');
    //var lg = fp[0].files.length; // get length
    //var items = fp[0].files[0];
    //var Fpath = $('#file-1').val();
    //var FFpath = fp.results;
    //if (lg > 0) {
    //    for (var i = 0; i < lg; i++) {
    //        var fileName = items[i].name; // get file name
    //        var fileSize = items[i].size; // get file size 
    //        var fileType = items[i].type; // get file type
    //        var FpathF = items[i].results;
    //    }
    //}
    //debugger;
    //var DataList = [];
    //$('.kv-file-content img').each(function () {
    //    debugger;
    //    //var DataList = [];
    //    var Ex = $(this).attr('title').split('.').pop().toLowerCase();
    //    DataList.push({
    //        name: $(this).attr('title'),
    //        size: 00,
    //        type: "image/" + Ex
    //    }); 
    //    alert(DataList[0].name);
    //});
    //debugger;
    InsertItemSetupDetails();
    //});
}
function BackBtnClick() {
    RemoveSession();
        ItemSetup();
}
function DeleteBtnClick() {
    debugger;
    DeleteItemDetails();
}
function ApproveBtnClick() {
    debugger;
    ApproveItemDetails();
}
function EditBtnClick() {
    debugger;
    sessionStorage.removeItem("ItmDTransType");
    sessionStorage.setItem("ItmDTransType", "Update");

    var ItmPortfolio = $('#item_portfolio').val();
    var Baseuom = $('#base_uom_cd').val();
    var Puruom = $('#pur_uom_cd').val();
    var Saleuom = $('#sl_uom_cd').val();
    var Bin = $('#deafault_bin').val();
    var Wharehouse = $('#default_warehouse').val();

    sessionStorage.setItem("ItmPortfolio", ItmPortfolio);
    sessionStorage.setItem("Baseuom", Baseuom);
    sessionStorage.setItem("Puruom", Puruom);
    sessionStorage.setItem("Saleuom", Saleuom);
    sessionStorage.setItem("Bin", Bin);
    sessionStorage.setItem("Wharehouse", Wharehouse);

    BindSearchableDDLListData();
    BindDDLListData();
    EditItmBtnDetails();
    AttributeEnable();

    $("#btn_save").attr('onclick', "SaveBtnClick()");
    $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    $("#btn_add_new_item").attr('onclick', '');
    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Edit").attr('onclick', '');
    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Delete").attr('onclick', '');
    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Approve").attr('onclick', '');
    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_back").attr('onclick', '');
    $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
}
function RefreshBtnClick() {
    //$('#btn_close').click(function () {
    Ref_DelItmDetails();
    readonlyaftersave();
    $('#AttriTBody tr').remove();

    $("#btn_back").attr('onclick', "BackBtnClick()");
    $("#btn_add_new_item").attr('onclick', "AddNewItemBtnClick()");

    $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

    $("#btn_save").attr('onclick', '');
    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_close").attr('onclick', '');
    $("#btn_close").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Edit").attr('onclick', '');
    $("#Btn_Delete").attr('onclick', '');
    $("#Btn_Approve").attr('onclick', '');
    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //});
}
function Ref_DelItmDetails() {
    debugger;
  //  clearscreen();
    BindDDLListData();
}
function AddNewItemBtnClick() {
    //$('#btn_add_new_item').click(function () {
    debugger;
    BindSearchableDDLListData();
    BindDDLListData();
    
    RemoveSession();
    $("#item_name").focus();

  //  clearscreen();
    readafternew();
    validation();

    $("#btn_close").attr('onclick', 'RefreshBtnClick()');
    $("#btn_save").attr('onclick', 'SaveBtnClick()');
    $("#btn_close").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

    $("#btn_add_new_item").attr('onclick', '');
    $("#Btn_Edit").attr('onclick', '');
    $("#Btn_Delete").attr('onclick', '');
    $("#Btn_Approve").attr('onclick', '');
    $("#btn_back").attr('onclick', '');
    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //});
}
function ItemSetup() {
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemSetup/ItemList",
                data: {},
                success: function (data) {
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });
    } catch (err) {
        console.log(PFName + " Error : " + err.message);
    }
}
function GetAllGroupDetails() {
    $.ajax({
        type: 'POST',
        url: '/BusinessLayer/ItemSetup/GetAllGroup',/*Controller=ItemSetup and Fuction=GetAllGroup*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: '',
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/

                var s = '<option value="-1">Choose option</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].item_grp_id + '">' + arr.Table[i].ItemGroupChildNood + '</option>';
                }
                $("#group_name").html(s);
            }
        },
        error: function (Data) {
        }
    });
}
//function Getviewitemdetail() {
//    debugger;
//    var NewStatus = 0;
//    var itmcode = sessionStorage.getItem("EditItemCode");

//    if (itmcode != null) {
//        $.ajax(
//            {
//                type: "POST",
//                url: "/BusinessLayer/ItemSetup/Getviewitemdetail",
//                data: {
//                    EditItemCode: itmcode
//                },
//                dataType: "json",
//                success: function (data) {
//                    if (data == 'ErrorPage') {
//                        ErrorPage();
//                        return false;
//                    }
//                    if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
//                        var arr = [];
//                        debugger;
//                        arr = JSON.parse(data);
//                        //for (var i = 0; i < arr.Table.length; i++) {

//                        $("#item_name").val(arr.Table[0].item_name);
//                        $("#item_oem_no").val(arr.Table[0].item_oem_no);
//                        $("#item_sam_cd").val(arr.Table[0].item_sam_cd);
//                        $("#item_sam_des").val(arr.Table[0].item_tech_des);
//                        $("#item_leg_cd").val(arr.Table[0].item_leg_cd);
//                        $("#item_leg_des").val(arr.Table[0].item_leg_des);
//                        $("#bs_price").val(arr.Table[0].cost_price);
//                        $("#pr_price").val(arr.Table[0].sale_price);
//                        $("#min_stk_lvl").val(arr.Table[0].min_stk_lvl);
//                        $("#min_pr_stk").val(arr.Table[0].min_pr_stk);
//                        $("#re_ord_lvl").val(arr.Table[0].re_ord_lvl);

//                        //setTimeout(function () {
//                        //    $('#base_uom_cd').val(base_uom_cd).prop('selected', true);
//                        //    $('#pur_uom_cd').val(pur_uom_cd).prop('selected', true);
//                        //    $('#sl_uom_cd').val(sl_uom_cd).prop('selected', true);
//                        //}, 1000);
//                        //setTimeout(function () {

//                        //    $('#deafault_bin').val(deafault_bin).prop('selected', true);
//                        //    $('#item_portfolio').val(item_portfolio).prop('selected', true);
//                        $('#issue_method').val(arr.Table[0].issue_method).prop('selected', true);
//                        $('#cost_method').val(arr.Table[0].cost_method).prop('selected', true);
//                        //    $('#default_warehouse').val(default_warehouse).prop('selected', true);
//                        //}, 1000);

//                        $('#base_uom_cd').empty().append('<option value=' + arr.Table[0].baseuomID + ' selected="selected">' + arr.Table[0].baseuom + '</option>');
//                        $('#pur_uom_cd').empty().append('<option value=' + arr.Table[0].purcuomID + ' selected="selected">' + arr.Table[0].purcuom + '</option>');
//                        $('#sl_uom_cd').empty().append('<option value=' + arr.Table[0].saluomID + ' selected="selected">' + arr.Table[0].saluom + '</option>');
//                        $('#deafault_bin').empty().append('<option value=' + arr.Table[0].BinID + ' selected="selected">' + arr.Table[0].Bin + '</option>');
//                        $('#item_portfolio').empty().append('<option value=' + arr.Table[0].portfID + ' selected="selected">' + arr.Table[0].portf + '</option>');
//                        $('#default_warehouse').empty().append('<option value=' + arr.Table[0].wh_id + ' selected="selected">' + arr.Table[0].wh_name + '</option>');

//                        if (arr.Table[0].hsnID === '' || arr.Table[0].hsnID === null || arr.Table[0].hsnID === 'null') {
//                            $('#HSN_cd').empty().append('<option value="-1" selected="selected">---Select---</option>');
//                        }
//                        else {
//                            $('#HSN_cd').empty().append('<option value=' + arr.Table[0].hsnID + ' selected="selected">' + arr.Table[0].hsn + '</option>');
//                        }
//                        $('#group_name').empty().append('<option value=' + arr.Table[0].GroupID + ' selected="selected">' + arr.Table[0].groupname + '</option>');
//                        $("#group_name").attr("style", "border-color: #ced4da;");
//                        $("#item_rem").val(arr.Table[0].item_remarks);
//                        $('#ItmDCreatedBy').text(arr.Table[0].create_id);
//                        $('#ItmDCreatedDate').text(arr.Table[0].create_dt);
//                        $('#ItmDAmdedBy').text(arr.Table[0].mod_id);
//                        $('#ItmDAmdedDate').text(arr.Table[0].mod_dt);
//                        $('#ItmDApproveBy').text(arr.Table[0].app_id);
//                        $('#ItmDApproveDate').text(arr.Table[0].app_dt);
//                        $('#ItmStatus').text(arr.Table[0].app_status);

//                        if (arr.Table[0].stkout_warn === "Y") {
//                            $("#stkout_warn").prop("checked", true);
//                        }
//                        else {
//                            $("#stkout_warn").prop("checked", false);
//                        }
//                        debugger;
//                        if (arr.Table[0].act_status === "Y") {
//                            $("#act_stats").prop("checked", true);
//                        }
//                        else {
//                            $("#act_stats").prop("checked", false);
//                        }
//                        $('#inact_reason').text(arr.Table[0].inact_reason);

//                        if (arr.Table[0].loc_sls_coa != null && arr.Table[0].loc_sls_coa != 0) {
//                            $('#Local_Sale_Account').empty().append('<option value=' + arr.Table[0].loc_sls_coa + ' selected="selected">' + arr.Table[0].LocalSaleAccount + '</option>');
//                        } else {
//                            $('#Local_Sale_Account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].exp_sls_coa != null && arr.Table[0].exp_sls_coa != 0) {
//                            $('#export_sale_account').empty().append('<option value=' + arr.Table[0].exp_sls_coa + ' selected="selected">' + arr.Table[0].ExportSaleAccount + '</option>');
//                        } else {
//                            $('#export_sale_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].loc_pur_coa != null && arr.Table[0].loc_pur_coa != 0) {
//                            $('#local_purchase_account').empty().append('<option value=' + arr.Table[0].loc_pur_coa + ' selected="selected">' + arr.Table[0].LocalPurchaseAccount + '</option>');
//                         } else {
//                            $('#local_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                         }
//                        if (arr.Table[0].imp_pur_coa != null && arr.Table[0].imp_pur_coa != 0) {
//                            $('#import_purchase_account').empty().append('<option value=' + arr.Table[0].imp_pur_coa + ' selected="selected">' + arr.Table[0].ImportPurchaseAccount + '</option>');
//                         } else {
//                            $('#import_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                         }
//                        if (arr.Table[0].stk_coa != null && arr.Table[0].stk_coa != 0) {
//                            $('#stock_account').empty().append('<option value=' + arr.Table[0].stk_coa + ' selected="selected">' + arr.Table[0].StockAccount + '</option>');
//                        } else {
//                            $('#stock_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].sal_ret_coa != null && arr.Table[0].sal_ret_coa != 0) {
//                            $('#sale_return_account').empty().append('<option value=' + arr.Table[0].sal_ret_coa + ' selected="selected">' + arr.Table[0].SaleReturnAccount + '</option>');
//                        } else {
//                            $('#sale_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].Disc_coa != null && arr.Table[0].Disc_coa != 0) {
//                            $('#discount_account').empty().append('<option value=' + arr.Table[0].Disc_coa + ' selected="selected">' + arr.Table[0].DiscountAccount + '</option>');
//                        } else {
//                            $('#discount_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].pur_ret_coa != null && arr.Table[0].pur_ret_coa != 0) {
//                            $('#purchase_return_account').empty().append('<option value=' + arr.Table[0].pur_ret_coa + ' selected="selected">' + arr.Table[0].PurchaseReturnAccount + '</option>');
//                        } else {
//                            $('#purchase_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].prov_pay_coa != null && arr.Table[0].prov_pay_coa != 0) {
//                            $('#provisional_payable_account').empty().append('<option value=' + arr.Table[0].prov_pay_coa + ' selected="selected">' + arr.Table[0].ProvisionalPayableAccount + '</option>');
//                        } else {
//                            $('#provisional_payable_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].cogs_coa != null && arr.Table[0].cogs_coa != 0) {
//                            $('#cost_of_goods_sold_account').empty().append('<option value=' + arr.Table[0].cogs_coa + ' selected="selected">' + arr.Table[0].CostofGoodsSoldAccount + '</option>');
//                        } else {
//                            $('#cost_of_goods_sold_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].stk_adj_coa != null && arr.Table[0].stk_adj_coa != 0) {
//                            $('#stock_adjustment_account').empty().append('<option value=' + arr.Table[0].stk_adj_coa + ' selected="selected">' + arr.Table[0].StockAdjustmentAccount + '</option>');
//                        } else {
//                            $('#stock_adjustment_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].dep_coa != null && arr.Table[0].dep_coa != 0) {
//                            $('#depreciation_account').empty().append('<option value=' + arr.Table[0].dep_coa + ' selected="selected">' + arr.Table[0].DepreciationAccount + '</option>');
//                        } else {
//                            $('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        }
//                        if (arr.Table[0].asset_coa != null && arr.Table[0].asset_coa != 0) {
//                            $('#asset_account').empty().append('<option value=' + arr.Table[0].asset_coa + ' selected="selected">' + arr.Table[0].AssetAccount + '</option>');
//                        } else {
//                            $('#asset_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//                        } 

//                        $("#wght_kg").val(arr.Table[0].wght_kg);
//                        $("#wght_ltr").val(arr.Table[0].wght_ltr);
//                        $("#gr_wght").val(arr.Table[0].gr_wght);
//                        $("#nt_wght").val(arr.Table[0].nt_wght);
//                        $("#item_hgt").val(arr.Table[0].item_hgt);
//                        $("#item_wdh").val(arr.Table[0].item_wdh);
//                        $("#item_len").val(arr.Table[0].item_len);
//                        $("#item_pack_sz").val(arr.Table[0].item_pack_sz);

//                        for (var i = 0; i < arr.Table1.length; i++) {
//                            var BrchID = arr.Table1[i].br_id;
//                            var BrchFlag = arr.Table1[i].act_status;
//                            var Actid = "#BrAct_stat_" + BrchID;
//                            if (BrchFlag === "Y") {
//                                $('#ItmBranchList tbody>tr').find(Actid).prop("checked", true)
//                            }
//                            else {
//                                $('#ItmBranchList tbody>tr').find(Actid).prop("checked", false)
//                            }
//                        }
//                        debugger;
//                        if (arr.Table3.length > 0) {
//                            debugger;
//                            for (var i = 0; i < arr.Table3.length; i++) {
//                                var ImgName = arr.Table3[i].item_img_name;
//                                var ImgPath = arr.Table3[i].item_img_path;
//                                var url = window.location.href; 
//                                var origin = window.location.origin + "/Images/ItemSetup/" + ImgName;
//                                $('#container').append('<a href="' + origin + '" target="_blank"><img src="' + origin + '" width="175px" height="175px" style="border-radius:5px;" /></a>')
//                            }
//                            //var $images = $('#container img');
//                            //$images.hide();
//                            //$images.each(function (index) {
//                            //    $(this).delay(index * 50).fadeIn();
//                            //});
//                        }

//                        for (var j = 0; j < arr.Table2.length; j++) {
//                            var rowIdx = 0;
//                            var AttrName = arr.Table2[j].attr_name;
//                            var AttrID = arr.Table2[j].attr_id;
//                            var AttrValName = arr.Table2[j].attr_val_name;
//                            var AttrValID = arr.Table2[j].attr_val_id;

//                            $('#AttriTBody').append(`<tr id="R${++rowIdx}"> 
//                                        <td class=" ">${AttrName}</td>
//<td class=" " hidden="hidden">${AttrID}</td>
//                                        <td class=" ">${AttrValName}</td>
//<td class=" " hidden="hidden">${AttrValID}</td>
//                                        <td class="red center"> <i class="fa fa-trash" aria-hidden="true"></i></td>
//              </tr>`);
//                        }
//                        if (arr.Table[0].app_status === "Drafted") {
//                        }
//                        else {
//                            $("#Btn_Delete").attr('onclick', '');
//                            $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

//                            $("#Btn_Approve").attr('onclick', '');
//                            $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//                        }
//                        //}
//                        NewStatus = 1;
//                        $("#btn_save").attr('onclick', '');
//                        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });


//                    }
//                    readonlyaftersave();
//                }
//            });
//    }
//    if (itmcode === null) {
//        debugger;
//        BindSearchableDDLListData();
//        BindDDLListData();

//        $("#btn_add_new_item").attr('onclick', '');
//        $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//        $("#Btn_Edit").attr('onclick', '');
//        $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//        $("#Btn_Delete").attr('onclick', '');
//        $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//        $("#Btn_Approve").attr('onclick', '');
//        $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//        $("#btn_back").attr('onclick', '');
//        $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//        $("#item_name").focus();
//    }
//    //readonlyaftersave();
//}

/*This Ajax use for Getting UOM List */
function GetUOM() {
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemSetup/GetUOM",/*Controller=ItemSetup and Fuction=GetUOM*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: '',/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="-1">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].uom_id + '">' + arr.Table[i].uom_name + '</option>';
                }
                $("#base_uom_cd").html(s);
                $("#pur_uom_cd").html(s);
                $("#sl_uom_cd").html(s);

                var Uom = sessionStorage.getItem("Baseuom");
                if (Uom !== null) {
                    $('#base_uom_cd').val(Uom).prop('selected', true);
                }
                var PurUom = sessionStorage.getItem("Puruom");
                if (PurUom !== null) {
                    $('#pur_uom_cd').val(PurUom).prop('selected', true);
                }
                var SaleUom = sessionStorage.getItem("Saleuom");
                if (SaleUom !== null) {
                    $('#sl_uom_cd').val(SaleUom).prop('selected', true);
                }
            }
        },
        error: function (Data) {

        }
    });
};
function Getwarehouse() {
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemSetup/Getwarehouse",/*Controller=ItemSetup and Fuction=Getwarehouse*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: '',/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="-1">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                }
                $("#default_warehouse").html(s);

                var WareHouse = sessionStorage.getItem("Wharehouse");
                if (WareHouse !== null) {
                    $('#default_warehouse').val(WareHouse).prop('selected', true);
                }
            }
        },
        error: function (Data) {

        }
    });
};
function Getbin() {
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemSetup/Getbin",/*Controller=ItemSetup and Fuction=Getbin*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: '',/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="-1">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].setup_id + '">' + arr.Table[i].setup_val + '</option>';
                }
                $("#deafault_bin").html(s);

                var Bin = sessionStorage.getItem("Bin");
                if (Bin !== null) {
                    $('#deafault_bin').val(Bin).prop('selected', true);
                }
            }
        }
    });
}
function Getport() {
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemSetup/Getportf",/*Controller=ItemSetup and Fuction=Getportf*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: '',/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="-1">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].setup_id + '">' + arr.Table[i].setup_val + '</option>';
                }
                $("#item_portfolio").html(s);

                var PortFolio = sessionStorage.getItem("ItmPortfolio");
                if (PortFolio !== null) {
                    $('#item_portfolio').val(PortFolio).prop('selected', true);
                }
            }
        }
    });
}
function Geterrormsg() {
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemSetup/Geterrormsg",/*Controller=ItemSetup and Fuction=Geterrormsg*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: '',/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                //alert(data);
                //alert(arr);

                //$('#sl_uom_cd').val(arr);
                var s = '<option value="-1">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].setup_id + '">' + arr.Table[i].setup_val + '</option>';
                }
                $("#deafault_bin").html(s);

            }
        }
    });
}
function validation() {
    $("#spanBaseUom").attr("style", "display: none;");
    $("#base_uom_cd").attr("style", "border-color: #ced4da;");
    $("#spanPurUom").attr("style", "display: none;");
    $("#pur_uom_cd").attr("style", "border-color: #ced4da;");
    $("#spanSalUom").attr("style", "display: none;");
    $("#sl_uom_cd").attr("style", "border-color: #ced4da;");
    $("#item_name").attr("style", "border-color: #ced4da;");
    $("#spanItemName").attr("style", "display: none;");
    $("#spanGroupName").attr("style", "display: none;");
    $("#group_name").attr("style", "border-color: #ced4da;");
    $("#inact_reason").attr("style", "border-color: #ced4da;");
    $("#SpanItminactreason").attr("style", "display: none;");
    $("#InactiveRemarksReq").attr("style", "display: none;");

};
function readafternew() {
    debugger;
    $("#item_name").prop("readonly", false);
    $("#item_oem_no").prop("readonly", false);
    $("#item_sam_cd").prop("readonly", false);
    $("#item_sam_des").prop("readonly", false);
    $("#item_leg_des").prop("readonly", false);
    $("#bs_price").prop("readonly", false);
    $("#item_leg_cd").prop("readonly", false);
    $("#group_name").attr('disabled', false);
    $("#base_uom_cd").attr('disabled', false);
    $("#pur_uom_cd").attr('disabled', false);
    $("#sl_uom_cd").attr('disabled', false);
    $("#item_portfolio").attr('disabled', false);
    $("#pr_price").prop("readonly", false);
    $("#issue_method").attr('disabled', false);
    $("#cost_method").attr('disabled', false);
    $("#min_stk_lvl").prop("readonly", false);
    $("#min_pr_stk").prop("readonly", false);
    $("#re_ord_lvl").prop("readonly", false);
    $("#stkout_warn").prop("disabled", false);
    $("#item_rem").prop("readonly", false);
    $("#act_stats").prop("disabled", false);
    $("#inact_reason").prop("readonly", true);
    $("#default_warehouse").attr('disabled', false);
    $("#deafault_bin").attr('disabled', false);
    $("#HSN_cd").attr('disabled', false);

    $("#Local_Sale_Account").attr('disabled', false);
    $("#export_sale_account").attr('disabled', false);
    $("#local_purchase_account").attr('disabled', false);
    $("#import_purchase_account").attr('disabled', false);
    $("#stock_account").attr('disabled', false);
    $("#sale_return_account").attr('disabled', false);
    $("#discount_account").attr('disabled', false);
    $("#purchase_return_account").attr('disabled', false);
    //$("#provisional_payable_account").attr('disabled', false);
    //$("#cost_of_goods_sold_account").attr('disabled', false);
    //$("#stock_adjustment_account").attr('disabled', false);
    $("#depreciation_account").attr('disabled', false);
    $("#asset_account").attr('disabled', false);

    $("#wght_kg").prop("readonly", false);
    $("#wght_ltr").prop("readonly", false);
    $("#gr_wght").prop("readonly", false);
    $("#nt_wght").prop("readonly", false);
    $("#item_hgt").prop("readonly", false);
    $("#item_wdh").prop("readonly", false);
    $("#item_len").prop("readonly", false);
    $("#item_pack_sz").prop("readonly", false);

    $("#ItmBranchList").find("*").attr("disabled", false);

    $("#itemAttrList").attr('disabled', false);
    $("#itemAttrValList").attr('disabled', false);
    $("#file").attr('disabled', false);
}
function readonlyaftersave() {
    $("#item_name").prop("readonly", true);
    $("#item_oem_no").prop("readonly", true);
    $("#item_sam_cd").prop("readonly", true);
    $("#item_sam_des").prop("readonly", true);
    $("#item_leg_des").prop("readonly", true);
    $("#bs_price").prop("readonly", true);
    $("#item_leg_cd").prop("readonly", true);
    $("#group_name").attr('disabled', true);
    $("#base_uom_cd").attr('disabled', true);
    $("#pur_uom_cd").attr('disabled', true);
    $("#sl_uom_cd").attr('disabled', true);
    $("#item_portfolio").attr('disabled', true);
    $("#pr_price").prop("readonly", true);
    $("#issue_method").attr('disabled', true);
    $("#cost_method").attr('disabled', true);
    $("#min_stk_lvl").prop("readonly", true);
    $("#min_pr_stk").prop("readonly", true);
    $("#re_ord_lvl").prop("readonly", true);
    $("#stkout_warn").prop("disabled", true);
    $("#item_rem").prop("readonly", true);
    $("#act_stats").prop("disabled", true);
    $("#inact_reason").prop("readonly", true);
    $("#default_warehouse").attr('disabled', true);
    $("#deafault_bin").attr('disabled', true);
    $("#HSN_cd").attr('disabled', true);

    $("#Local_Sale_Account").attr('disabled', true);
    $("#export_sale_account").attr('disabled', true);
    $("#local_purchase_account").attr('disabled', true);
    $("#import_purchase_account").attr('disabled', true);
    $("#stock_account").attr('disabled', true);
    $("#sale_return_account").attr('disabled', true);
    $("#discount_account").attr('disabled', true);
    $("#purchase_return_account").attr('disabled', true);
    //$("#provisional_payable_account").attr('disabled', true);
    //$("#cost_of_goods_sold_account").attr('disabled', true);
    //$("#stock_adjustment_account").attr('disabled', true);
    $("#depreciation_account").attr('disabled', true);
    $("#asset_account").attr('disabled', true);

    $("#wght_kg").prop("readonly", true);
    $("#wght_ltr").prop("readonly", true);
    $("#gr_wght").prop("readonly", true);
    $("#nt_wght").prop("readonly", true);
    $("#item_hgt").prop("readonly", true);
    $("#item_wdh").prop("readonly", true);
    $("#item_len").prop("readonly", true);
    $("#item_pack_sz").prop("readonly", true);

    $("#ItmBranchList").find("*").attr("disabled", true);
    //$("#ItmAttributeListTbl").find("*").attr("disabled", true);
    $("#itemAttrList").attr('disabled', true);
    $("#itemAttrValList").attr('disabled', true);
    $("#file").attr('disabled', true);
}
function EditItmBtnDetails() {
    $("#item_name").prop("readonly", false);
    $("#item_oem_no").prop("readonly", false);
    $("#item_sam_cd").prop("readonly", false);
    $("#item_sam_des").prop("readonly", false);
    $("#item_leg_des").prop("readonly", false);
    $("#bs_price").prop("readonly", false);
    $("#item_leg_cd").prop("readonly", false);
    $("#group_name").attr('disabled', false);
    $("#base_uom_cd").attr('disabled', false);
    $("#pur_uom_cd").attr('disabled', false);
    $("#sl_uom_cd").attr('disabled', false);
    $("#item_portfolio").attr('disabled', false);
    $("#pr_price").prop("readonly", false);
    $("#issue_method").attr('disabled', false);
    $("#cost_method").attr('disabled', false);
    $("#min_stk_lvl").prop("readonly", false);
    $("#min_pr_stk").prop("readonly", false);
    $("#re_ord_lvl").prop("readonly", false);
    $("#stkout_warn").prop("disabled", false);
    $("#item_rem").prop("readonly", false);
    $("#act_stats").prop("disabled", false);
    $("#inact_reason").prop("readonly", false);
    $("#default_warehouse").attr('disabled', false);
    $("#deafault_bin").attr('disabled', false);
    $("#HSN_cd").attr('disabled', false);

    $("#Local_Sale_Account").attr('disabled', false);
    $("#export_sale_account").attr('disabled', false);
    $("#local_purchase_account").attr('disabled', false);
    $("#import_purchase_account").attr('disabled', false);
    $("#stock_account").attr('disabled', false);
    $("#sale_return_account").attr('disabled', false);
    $("#discount_account").attr('disabled', false);
    $("#purchase_return_account").attr('disabled', false);
    //$("#provisional_payable_account").attr('disabled', false);
    //$("#cost_of_goods_sold_account").attr('disabled', false);
    //$("#stock_adjustment_account").attr('disabled', false);
    $("#depreciation_account").attr('disabled', false);
    $("#asset_account").attr('disabled', false);

    $("#wght_kg").prop("readonly", false);
    $("#wght_ltr").prop("readonly", false);
    $("#gr_wght").prop("readonly", false);
    $("#nt_wght").prop("readonly", false);
    $("#item_hgt").prop("readonly", false);
    $("#item_wdh").prop("readonly", false);
    $("#item_len").prop("readonly", false);
    $("#item_pack_sz").prop("readonly", false);

    $("#ItmBranchList").find("*").attr("disabled", false);
    $("#itemAttrList").attr('disabled', false);
    $("#itemAttrValList").attr('disabled', false);
    $("#file").attr('disabled', false);
}
//function clearscreen() {
//    $("#ItmDCreatedBy").text("");
//    $("#ItmDCreatedDate").text("");
//    $("#ItmDAmdedBy").text("");
//    $("#ItmDAmdedDate").text("");
//    $("#ItmDApproveBy").text("");
//    $("#ItmDApproveDate").text("");
//    $('#ItmStatus').text("");
    
//    $("#item_cd").val('')
//    $("#item_name").val('');
//    $("#item_oem_no").val('');
//    $("#item_sam_cd").val('');
//    $("#item_sam_des").val('');
//    $("#item_leg_des").val('');
//    $("#bs_price").val('');
//    $("#item_leg_cd").val('');
//    //$("#group_name").val(null);
//    $('#group_name').empty().append('<option value="-1" selected="selected">---Select---</option>');
//    $("#base_uom_cd").val('-1');
//    $("#pur_uom_cd").val('-1');
//    $("#sl_uom_cd").val('-1');
//    $("#item_portfolio").val('-1');
//    $("#pr_price").val('');
//    $("#issue_method").val('F');
//    $("#cost_method").val('S');
//    $("#min_stk_lvl").val('');
//    $("#min_pr_stk").val('');
//    $("#re_ord_lvl").val('');
//    $("#stkout_warn").prop("checked", true);
//    //$("#stkout_warn").checked = true;
//    $("#item_rem").val('');
//    $("#act_stats").prop("checked", true);
//    //$("#act_stats").checked=true;
//    $("#inact_reason").val('');
//    $("#default_warehouse").val('-1');
//    $("#deafault_bin").val('-1');
//    $('#HSN_cd').empty().append('<option value="0" selected="selected">---Select---</option>');

//    $("#itemAttrList").val('0');
//    $("#itemAttrValList").val('0');

//    $('#Local_Sale_Account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#export_sale_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#local_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#import_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#stock_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#sale_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#discount_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#purchase_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#provisional_payable_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#cost_of_goods_sold_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#stock_adjustment_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');
//    $('#asset_account').empty().append('<option value="0" selected="selected">---Select---</option>');

//    $("#wght_kg").val('')
//    $("#wght_ltr").val('');
//    $("#gr_wght").val('');
//    $("#nt_wght").val('');
//    $("#item_hgt").val('');
//    $("#item_wdh").val('');
//    $("#item_len").val('');
//    $("#item_pack_sz").val('');
//    ItemBranchchecked();
//    $('#AttriTBody tr').remove();

//    $('#container').empty();
//    $("#file").val('');
//}
function GetCurrentDatetime(ActionType) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemSetup/GetCurrentDT",/*Controller=ItemSetup and Fuction=GetCurrentDT*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',/*Registration pass value like model*/
        success: function (response) {
            debugger;
            if (response === 'ErrorPage') {
                ErrorPage();
                return false;
            }
            if (ActionType === "Save") {
                $("#ItmDCreatedBy").text(response.CurrentUser);
                $("#ItmDCreatedDate").text(response.CurrentDT);
            }
            if (ActionType === "Edit") {
                $("#ItmDAmdedBy").text(response.CurrentUser);
                $("#ItmDAmdedDate").text(response.CurrentDT);
            }
            if (ActionType === "Approved") {
                $("#ItmDApproveBy").text(response.CurrentUser);
                $("#ItmDApproveDate").text(response.CurrentDT);
            }
        }
        //,
        //failure: function (response) {
        //    alert(response.responseText);
        //},
        //error: function (response) {
        //    alert(response.responseText);
        //}
    });
}
function RemoveSession() {
    sessionStorage.removeItem("ItmPortfolio");
    sessionStorage.removeItem("Baseuom");
    sessionStorage.removeItem("Puruom");
    sessionStorage.removeItem("Saleuom");
    sessionStorage.removeItem("Bin");
    sessionStorage.removeItem("Wharehouse");
    sessionStorage.removeItem("EditItemCode");
    sessionStorage.removeItem("ItmDTransType");
}
function DeleteItemDetails() {
    debugger;
    var itmcode = sessionStorage.getItem("EditItemCode");
    try {
        swal({
            title: $("#deltital").text()+"?",
            text: $("#deltext").text()+"!",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: false
        },
            function () {
                $.ajax({
                    type: "POST",
                    url: "/BusinessLayer/ItemSetup/DeleteItemDetails",/*Controller=ItemSetup and Fuction=DeleteItemDetails*/
                    dataType: "json",
                    data: {ItemID: itmcode},
                    success: function (data) {
                        if (data == 'ErrorPage') {
                            ErrorPage();
                            return false;
                        }
                        RemoveSession();
                        readafternew();
                        Ref_DelItmDetails();
                        BindSearchableDDLListData();

                        $("#btn_back").attr('onclick', "BackBtnClick()");
                        $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                        $("#btn_add_new_item").attr('onclick', "AddNewItemBtnClick()");
                        $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

                        $("#btn_save").attr('onclick', '');
                        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                        $("#btn_close").attr('onclick', '');
                        $("#btn_close").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                        $("#Btn_Edit").attr('onclick', '');
                        $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                        $("#Btn_Delete").attr('onclick', '');
                        $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                        $("#Btn_Approve").attr('onclick', '');
                        $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                        swal("", $("#deletemsg").text(), "success");
                    },
                    error: function (Data) {

                    }
                });
            });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}
function DeleteUploadedImages() {
    debugger;
    var itmcode = sessionStorage.getItem("EditItemCode");
    $.ajax({
        type: 'post',
        url: '/BusinessLayer/ItemSetup/DeleteUploadedImages',
        data: { itmcode: itmcode },
        success: function (response) {
            debugger;
            if (response == 'ErrorPage') {
                ErrorPage();
                return false;
            }
        },
        error: function () {
            //alert("Whoops something went wrong!");
        }
    });
}
function DeleteItemAttributeDetails() {
    debugger;
    var itmcode = sessionStorage.getItem("EditItemCode");
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemSetup/DeleteItemAttributeDetail",/*Controller=ItemSetup and Fuction=DeleteItemAttributeDetail*/
        dataType: "json",
        data: { Itemcode: itmcode },/*Registration pass value like model*/
        success: function (Data) {
            debugger;
            if (Data == 'ErrorPage') {
                ErrorPage();
                return ;
            }
        },
        //error: function () {
        //}
    });
};
function ApproveItemDetails() {
    debugger;
    var itmcode = sessionStorage.getItem("EditItemCode");
    try {
                $.ajax({
                    type: "POST",
                    url: "/BusinessLayer/ItemSetup/ApproveItemDetails",/*Controller=ItemSetup and Fuction=ApproveItemDetails*/
                    //contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: {ItemID: itmcode},
                    success: function (data) {
                        if (data == 'ErrorPage') {
                            ErrorPage();
                            return false;
                        }

                        debugger;
                        GetCurrentDatetime("Approved");
                        $('#ItmStatus').text("Approved");

                        $("#Btn_Delete").attr('onclick', '');
                        $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                        $("#Btn_Approve").attr('onclick', '');
                        $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                        swal("", $("#approvemsg").text(), "success");
                    },
                    error: function (Data) {
                    }
                });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}
function EnableToggleSaveTime() {
    $("#i_sls").attr("disabled", false);
    $("#i_pur").attr("disabled", false);
    $("#i_wip").attr("disabled", false);
    $("#i_capg").attr("disabled", false);
    $("#i_stk").attr("disabled", false);
    $("#i_qc").attr("disabled", false);
    $("#i_srvc").attr("disabled", false);
    $("#i_cons").attr("disabled", false);
    $("#i_serial").attr("disabled", false);
    $("#i_sam").attr("disabled", false);
    $("#i_batch").attr("disabled", false);
    $("#i_exp").attr("disabled", false);
    $("#i_pack").attr("disabled", false);
    $("#i_catalog").attr("disabled", false);
    $("#i_ws").css("attr", false);
    $("#i_exempted").attr("disabled", false);
    $("#SubItem").attr("disabled", false);
  
}
function InsertItemDetail() {
    debugger;
    var btn = $("#hdnSavebtn").val();
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
           $("#btn_save").prop("disabled",true);
    }
    else {
        debugger;
        if (CheckCustomerValidations1() == false) {
            return false;
        }

        //if (CheckSuppValidations() == false) {
        //    return false;
        //}
        //if (CheckVehicleValidation() == false) {
        //    return false;
        //}
        if (validateItemSetupInsertform() == false) {
            return false;
        }
        if (ItemPropertyCheckValidation() == false) {
            return false;
        }
        if (CheckSubItemValidation() == false) {
            return false;
        }
        if ($("input[name='default']").length > 0) {
            if ($("input[name='default']").length == 1) {
                $("input[name='default']").click();
            } else {
                var Imgdefault = "";
                $("input[name='default']").each(function () {
                    var curInput = $(this);
                    if (curInput[0].checked) {
                        Imgdefault = "Y";
                    }
                });
                if (Imgdefault != "Y") {
                    swal("", $("#PleaseSelectDefaultImage").text(), "warning");
                    return false;

                }
            }
            
        }

        if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
            debugger;

            FinalItemDetail = InsertItemAttributeDetails();
            FinalCustomerDetail = InsertCustomerDetails();
            FinalSupplierDetail = InsertSupplierDetails();
            FinalPortfolioDetail = InsertPortfolioDetails();
            FinalVehicleDetail = InsertVehicleDetails();
            FinalSubItemDetail = InsertSubItemDetails();
            /*----- Attatchment start--------*/
            $(".fileinput-upload").click();/*To Upload Img in folder*/
            FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
            /*----- Attatchment End--------*/

            debugger;
            var ItemAttrDt = JSON.stringify(FinalItemDetail);
            $('#hdItemAttrDetailList').val(ItemAttrDt);

            var ItemCustDt = JSON.stringify(FinalCustomerDetail);
            $('#hdItemCustDetail').val(ItemCustDt);

            var ItemSuppDt = JSON.stringify(FinalSupplierDetail);
            $('#hdItemSuppDetail').val(ItemSuppDt);

            var ItemPortfDt = JSON.stringify(FinalPortfolioDetail);
            $('#hdPortfolioDetail').val(ItemPortfDt);

            var ItemVehiclefDt = JSON.stringify(FinalVehicleDetail);
            $('#hdVehicleDetail').val(ItemVehiclefDt);

            var SubItemDt = JSON.stringify(FinalSubItemDetail);
            $('#hdSubItemDetail').val(SubItemDt);

            /*----- Attatchment start--------*/
            var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
            $('#hdn_Attatchment_details').val(ItemAttchmentDt);

            /*----- Attatchment End--------*/
            $("#i_stk").attr("disabled", false);
            $("#i_serial").attr("disabled", false);
            $("#i_exp").attr("disabled", false);
            $("#i_batch").attr("disabled", false);
            $("#i_srvc").attr("disabled", false);
            $("#SubItem").attr("disabled", false);
            $("#act_stats").attr("disabled", false);
            $("#base_uom_cd").attr("disabled", false);
            $("#btn_save").css("filter", "grayscale(100%)");
            $("#hdnSavebtn").val("AllreadyClick");
            // $("#btn_save").prop("disabled",true);
            EnableToggleSaveTime();
            return true;
        }
        else {
            alert("Check network");
            return false;
        }
    }
  
};


function ItemPropertyCheckValidation() {
    
       
        // List of checkbox IDs to check
        var checkboxIds = [
        'i_sls', 'i_pur', 'i_wip', 'i_capg', 'i_stk',
        'i_qc', 'i_srvc', 'i_cons', 'i_serial', 'i_sam',
        'i_batch', 'i_exp', 'i_pack', 'i_catalog', 'i_ws',
        'i_exempted', 'SubItem'
        ];

        var isAnyChecked = false;

        for (var i = 0; i < checkboxIds.length; i++) {
            var checkbox = document.getElementById(checkboxIds[i]);

        // Skip disabled checkboxes
        if (checkbox &&  checkbox.checked) {
            isAnyChecked = true;
        break;
            }
        }

    if (!isAnyChecked) {
        //  swal("", $("#approvemsg").text(), "success");
        swal("", $("#AtleastOneItemPropertymustbeselected").text(), "warning");
   /*         alert("Atleast One Item Property must be selected.");*/
        return false;
        }

        return true;
   

}
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
    $("#AttriTBody tr").each(function () {
       
        var currentRow = $(this);
        /*var AttriName = currentRow.find("td:eq(0)").text();*/
        /*var AttriID = currentRow.find("td:eq(1)").text();*/
        /*var AttriValName = currentRow.find("td:eq(2)").text();*/
        /*var AttriValID = currentRow.find("td:eq(3)").text();*/

        var AttriName = currentRow.find("#hdAttrname").text();
        var AttriID = currentRow.find("#hdAttrid").text();
        var AttriValName = currentRow.find("#hdAttrValName").text();
        var AttriValID = currentRow.find("#hdAttrValId").text();

        AttrList.push({ AttriName: AttriName, AttriID: AttriID, AttriValName: AttriValName, AttriValID: AttriValID, })
    });

    return AttrList;   
};
function InsertCustomerDetails() {
    debugger;
   
    var CustomerDTransType = sessionStorage.getItem("CustomerDTransType");   
    var TransType = '';
    if (CustomerDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var CustomerList = [];
    //var Sno = 0;
    //var rowCount = $('#CustomerDetailsTbl >tbody >tr').length + 1;
    //if (rowCount > 1) {
        $("#CustomerDetailsTbl tbody tr").each(function () {
            debugger;

            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            //var CustID = currentRow.find("#CustomerName_" + Sno).val();
            var CustID = currentRow.find("#hfCustID").val();
            var ItemCode = currentRow.find("#ItemCode").val();
            var ItemDes = currentRow.find("#ItemDescription").val();
            var Packdt = currentRow.find("#PackingDetail").val();
            var Boxdt = currentRow.find("#BoxDetail").val();
            var ItemRem = currentRow.find("#ItemRemarks").val();
            debugger;
            CustomerList.push({ CustID: CustID, ItemCode: ItemCode, ItemDes: ItemDes, Packdt: Packdt, Boxdt: Boxdt, ItemRem: ItemRem, })
        });
        debugger;
        return CustomerList;
    //}
};
function InsertSupplierDetails() {
    debugger;

    var SupplierDTransType = sessionStorage.getItem("SupplierDTransType");
    var TransType = '';
    if (SupplierDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var SupplierList = [];
    $("#SupplierDetailsTbl tbody tr").each(function () {
        debugger;

        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        //var SuppID = currentRow.find("#SupplierName_" + Sno).val();
        var SuppID = currentRow.find("#hfSuppID").val();
        var ItemCode = currentRow.find("#ItemCode").val();
        var ItemDes = currentRow.find("#ItemDescription").val();
        var Packdt = currentRow.find("#PackingDetail").val();
        var Boxdt = currentRow.find("#BoxDetail").val();
        var Wt = currentRow.find("#NetWeight").val();
        var ItemRem = currentRow.find("#SuppItemRemarks").val();
        debugger;
        SupplierList.push({ SuppID: SuppID, ItemCode: ItemCode, ItemDes: ItemDes, Packdt: Packdt, Boxdt: Boxdt, Wt: Wt, ItemRem: ItemRem, })
    });
    debugger;
    return SupplierList;
};
function InsertPortfolioDetails() {
    debugger;

    var PortfolioDTransType = sessionStorage.getItem("PortfolioDTransType");
    var TransType = '';
    if (PortfolioDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var PortfList = [];
    $("#PortfolioTbl tbody tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        //var PortfID = currentRow.find("#PortfolioName_" + Sno).val();       
        var PortfID = currentRow.find("#hfPrfID").val();
        var PortfDes = currentRow.find("#PortfolioDescription").val();      
        var PortfRem = currentRow.find("#portfRemarks").val();
        debugger;
        PortfList.push({ PortfID: PortfID, PortfDes: PortfDes, PortfRem: PortfRem, })
    });
    debugger;
    return PortfList;
};
function InsertVehicleDetails() {
    debugger;

    var VehicleDTransType = sessionStorage.getItem("VehicleDTransType");
    var TransType = '';
    if (VehicleDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var VehList = [];
    $("#VehicleTbl tbody tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        //var VehID = currentRow.find("#VehicleName_" + Sno).val();
        var VehID = currentRow.find("#hfVehID").val();
        var Model = currentRow.find("#VehModelNumber").val();
        var OEM = currentRow.find("#VehOEMNo").val();
        var PartNo = currentRow.find("#VehPartNo").val();
        var DES = currentRow.find("#VehItemDescription").val();
        var Rem = currentRow.find("#VehItemRemarks").val();
        debugger;
        VehList.push({ VehID: VehID, Model: Model, OEM: OEM, PartNo: PartNo, DES: DES, Rem: Rem, })
    });
    debugger;
    return VehList;
};
function InsertSubItemDetails() {
    var SubItemList = [];
    $("#NewSubItemtable tbody tr").each(function () {
        debugger;
        var currentRow = $(this);
        var SubItemId = currentRow.find("#SubItemId").text();
        var SubItemName = currentRow.find("#subItem").text();
        SubItemList.push({ SubItemName: SubItemName, SubItemId: SubItemId })
    });
    debugger;
    return SubItemList;
}

function SaveAndInsertUploadImage() {
    debugger;
    var itmcode = sessionStorage.getItem("EditItemCode");

    //var DefauiltImg = "";
    //if (DataList.length > 0) {
    //    for (var i = 0; i < DataList.length; i++) {
    //        if (i == 0) {
    //            DefauiltImg = "Y";
    //        }
    //        else {
    //            DefauiltImg = "N";
    //        }
    //        var formData = new FormData();
    //        formData.append('file', DataList[i]);
    //        formData.append('itmcode', "Test00012"); // modify
    //        formData.append('DefImg', DefauiltImg);
    //        $.ajax({
    //            type: 'post',
    //            url: '/ItemSetup/SaveUploadedImages',
    //            data: formData,
    //            success: function (response) {
    //                debugger;
    //                if (response == 'ErrorPage') {
    //                    ErrorPage();
    //                    return false;
    //                }
    //                if (response != null) {
    //                    var my_path = "/temp/" + response;
    //                    var image = '<img src="' + my_path + '" alt="image" style="width:150px">';
    //                    //$("#imgPreview").append(image);
    //                }
    //            },
    //            processData: false,
    //            contentType: false,
    //            error: function () {
    //                //alert("Whoops something went wrong!");
    //            }
    //        });
    //    }
    //}

    //var fp = $('#file-1');
    var DefauiltImg = "";
    var fp = $('#file');
    var lg = fp[0].files.length;
    if (lg > 0) {
        for (var i = 0; i < lg; i++) {
            if (i == 0) {
                DefauiltImg = "Y";
            }
            else {
                DefauiltImg = "N";
            }
            var formData = new FormData();
            formData.append('file', $('#file')[0].files[i]);
            formData.append('itmcode', itmcode); // modify
            formData.append('DefImg', DefauiltImg);

            if (i == 0) {
                DeleteUploadedImages();
            }

            $.ajax({
                type: 'post',
                url: '/BusinessLayer/ItemSetup/SaveUploadedImages',
                data: formData,
                success: function (response) {
                    debugger;
                    if (response == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    //if (response != null) {
                    //    var my_path = "/temp/" + response;
                    //    var image = '<img src="' + my_path + '" alt="image" style="width:150px">';
                    //    //$("#imgPreview").append(image);
                    //}
                },
                processData: false,
                contentType: false,
                error: function () {
                }
            });
        }
    }
};
function AttributeDisable() {
    var rowIdx = 0;
    debugger;
    var AttrList = [];
    $("#AttriTBody tr").each(function () {
        debugger;
        var currentRow = $(this);
        //var AttriName = currentRow.find("td:eq(0)").text();
        //var AttriID = currentRow.find("td:eq(1)").text();
        //var AttriValName = currentRow.find("td:eq(2)").text();
        //var AttriValID = currentRow.find("td:eq(3)").text();
        
        var AttriName = currentRow.find("#hdAttrname").text();
        var AttriID = currentRow.find("#hdAttrid").text();
        var AttriValName = currentRow.find("#hdAttrValName").text();
        var AttriValID = currentRow.find("hdAttrValId").text();


        AttrList.push({ AttriName: AttriName, AttriID: AttriID, AttriValName: AttriValName, AttriValID: AttriValID,})
    });
    debugger;
    $('#AttriTBody tr').remove();

    for(var i = 0; i < AttrList.length; i++) {
        $('#AttriTBody').append(`<tr id="R${++rowIdx}"> 
                                        <td >${AttrList[i].AttriName}</td>
<td  hidden="hidden">${AttrList[i].AttriID}</td>
                                        <td >${AttrList[i].AttriValName}</td>
<td  hidden="hidden">${AttrList[i].AttriValID}</td>
<td class="red center"> <i class="fa fa-trash" aria-hidden="true"></i></td>
              </tr>`);
    }

}
function AttributeEnable() {
    var rowIdx = 0;
    debugger;
    var AttrList = [];
    $("#AttriTBody tr").each(function () {
        debugger;
        var currentRow = $(this);
        //var AttriName = currentRow.find("td:eq(0)").text();
        //var AttriID = currentRow.find("td:eq(1)").text();
        //var AttriValName = currentRow.find("td:eq(2)").text();
        //var AttriValID = currentRow.find("td:eq(3)").text();
        
        var AttriName = currentRow.find("#hdAttrname").text();
        var AttriID = currentRow.find("#hdAttrid").text();
        var AttriValName = currentRow.find("#hdAttrValName").text();
        var AttriValID = currentRow.find("#hdAttrValId").text();

        AttrList.push({ AttriName: AttriName, AttriID: AttriID, AttriValName: AttriValName, AttriValID: AttriValID, })
    });
    debugger;
    $('#AttriTBody tr').remove();

    for (var i = 0; i < AttrList.length; i++) {
        $('#AttriTBody').append(`<tr id="R${++rowIdx}"> 
                                        <td >${AttrList[i].AttriName}</td>
<td  hidden="hidden">${AttrList[i].AttriID}</td>
                                        <td >${AttrList[i].AttriValName}</td>
<td  hidden="hidden">${AttrList[i].AttriValID}</td>
<td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true"></i></td>
              </tr>`);

        $("#itemAttrList option[value=" + AttrList[i].AttriID + "]").hide();
    }

}
function HideAttributeValue() {
    debugger;
    $("#AttriTBody tr").each(function () {
        debugger;
        var currentRow = $(this);
        /*var AttriID = currentRow.find("td:eq(1)").text();*/
       var AttriID = currentRow.find("#hdAttrid").text();

        $("#itemAttrList option[value=" + AttriID + "]").hide();
    });

}

function ItemBranchchecked() {
    debugger;
    $('#ItmBranchList tbody>tr').each(function () {
        var i = 0;
        var ItemBrID = "";
        $('td', this).each(function () {
            debugger;
            if (i === 0) {
                ItemBrID = $(this).find("#ItemBrID").text();
                i = i + 1;
            }
            else {
                var Actid = "#BrAct_stat_" + ItemBrID;
                $(Actid).prop("checked", true);
            }
        });
    });
};
/*for Form Validation*/
function CheckSubItemValidation() {
    debugger;
    var Flag="N";
    var checkedSubItem = '';
    if ($("#SubItem").is(":checked")) {
        checkedSubItem = "Y";
    }
    else {
        checkedSubItem = "N";
    }
    if (checkedSubItem == "Y") {
        var SrNo = $("#NewSubItemtable tbody tr").length;
        if (SrNo == "0" || SrNo == null) {
            swal("", $("#SubItemDetaiNoFound").text(), "warning");
            Flag = "Y";
        }
    }
    if (Flag=="Y") {
        return false
    }
}
function validateItemSetupInsertform() {
    debugger;
    var Flag;
    var active = '';
    var InActiveReason = $("#inact_reason").val();
    var baseuom = $("#base_uom_cd").val();
    if ($("#act_stats").is(":checked")) {
        active = "Y";
    }
    else {
        active = "N";
    }

    var Packagingtgl = "";
    if ($("#i_pack").is(":checked")) {
        Packagingtgl = 'Y';
    }
    else {
        Packagingtgl = 'N';
    }
    if (Packagingtgl == 'Y') {
        $("#collapseTwelve").removeClass('collapse');
        if ($("#pack_uom").val() == '0') {
            $("#pack_uom").attr("style", "border-color: #ff0000;");
            $("#spanpackuom").text($("#valueReq").text());
            $("#spanpackuom").css("display", "block");
            Flag = 'Y';
        }
        if ($("#Length").val() == '' || $("#Length").val()== '0') {
            $("#Length").attr("style", "border-color: #ff0000;");
            $("#spanlength").text($("#valueReq").text());
            $("#spanlength").css("display", "block");
            Flag = 'Y';
        }
        if ($("#Width").val() == '' || $("#Width").val() == '0') {
            $("#Width").attr("style", "border-color: #ff0000;");
            $("#spanwidth").text($("#valueReq").text());
            $("#spanwidth").css("display", "block");
            Flag = 'Y';
        }
        if ($("#Height").val() == ''|| $("#Height").val() == '0') {
            $("#Height").attr("style", "border-color: #ff0000;");
            $("#spanheight").text($("#valueReq").text());
            $("#spanheight").css("display", "block");
            Flag = 'Y';
        }
    }
    if ($("#item_name").val() == '') {
        $("#item_name").attr("style", "border-color: #ff0000;");
        $("#spanItemName").text($("#valueReq").text());
        $("#spanItemName").css("display", "block");
        Flag = 'Y';
    }
    else {
        var data = $("#item_name").val();
        if (data.includes("_") || data.includes(",")) {

            $("#item_name").css("border-color", "red");
            $("#spanItemName").text($("#span_AreNotAllowedInItemName").text());
            //document.getElementById("spanItemName").innerText = "(_ ,) are Not Allowed in Item Name."
            $("#spanItemName").css("font-weight", "bold");
            $("#spanItemName").css("display", "block");
            Flag = 'Y';
        }
        else {
            $("#spanItemName").text("");
            $("#item_name").css("border-color", "#ced4da");
            $("#spanItemName").css("font-weight", "");
        }
    }
    var param_status = $("#param_status").val();
    if (param_status == "Y") {
        if ($("#item_leg_cd").val() == '') {
            $("#item_leg_cd").attr("style", "border-color: #ff0000;");
            $("#spanRefNo").text($("#valueReq").text());
            $("#spanRefNo").css("display", "block");
            Flag = 'Y';
        }
    }  
    if ($("#group_name").val() == '0') {
        $("[aria-labelledby='select2-group_name-container']").prop("style", "border-color: #ff0000;");
        $("#spanGroupName").text($("#valueReq").text());
        $("#spanGroupName").css("display", "block");
        Flag = 'Y';
    }
    debugger;
    if ($("#i_srvc").is(":checked")) {

    }
    else {
        debugger;
        if ($("#base_uom_cd").val() == '0') {
            $("#base_uom_cd").attr("style", "border-color:#ff0000;");
            $("#spanBaseUom").text($("#valueReq").text());
            $("#spanBaseUom").css("display", "block");
            Flag = 'Y';
        }
        //if ($("#pur_uom_cd").val() == '0') {
        //    $("#pur_uom_cd").attr("style", "border-color:#ff0000;");
        //    $("#spanPurUom").text($("#valueReq").text());
        //    $("#spanPurUom").css("display", "block");
        //    Flag = 'Y';
        //}
        //if ($("#sl_uom_cd").val() == '0') {
        //    $("#sl_uom_cd").attr("style", "border-color:#ff0000;");
        //    $("#spanSalUom").text($("#valueReq").text());
        //    $("#spanSalUom").css("display", "block");
        //    Flag = 'Y';
        //}
    }
    debugger;
    if (active === 'N' && InActiveReason === '') {
        $("#inact_reason").attr("style", "border-color:#ff0000;", "required");
        $("#SpanItminactreason").text($("#valueReq").text());
        $("#SpanItminactreason").css("display", "block");
        $("#InactiveRemarksReq").css("display", "inherit");
        Flag = 'Y';
    }
    if (Flag == 'Y') {
        return false;
    }
    debugger;
    if (active == 'N') {
        $("#act_stats").prop("checked", false);
    }
    else {
        $("#act_stats").prop("checked", true);
    }
    //var active = "true";
    //$("#act_stats").val(active);
    if (RlvntAccountLinkingMendatory("Save","") == false) {
        return false;
    }
    if (Flag == 'Y') {
        return false;
    }

    return true;
}
function OnclickRefNo() {
    $("#spanRefNo").text("");
    $("#item_leg_cd").css("border-color", "#ced4da");
    var refNo = $("#item_leg_cd").val();
   
    if (refNo != "" && refNo != null) {
        
        generateCatalogNo(refNo)
    }
   
}
function GetCompanyDetails() {
    var HTMLstrinh = "";
    $.ajax({
        type: 'POST',
        url: '/Company/GetCompanyDetails',/*Controller=Company and Fuction=GetCompanyDetails*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: '',
        success: function (Objdata) {
            if (Objdata == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            if (Objdata !== null && Objdata !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(Objdata);/*this is use for json data braking in array type and put in a Array*/
                //alert(Objdata);
                var lenth = arr.Table.length;/*This is check the Data length*/
                if (arr.Table.length > 0) {
                    for (i = 0; i < arr.Table.length; i++) {

                        HTMLstrinh += '<tr class="even pointer">';
                        HTMLstrinh += '<td class="comp_name">' + arr.Table[i].comp_nm + '</td>';
                        HTMLstrinh += '<td class="comp_select"><input id="act_stat" comp_id="' + arr.Table[i].comp_id + '" type="checkbox" class="js-switch" checked="" data-switchery="true" style="display:none;"/>';
                        HTMLstrinh += '<span class="switchery switchery-default"><small></small></span>';

                        HTMLstrinh += '</td > ';
                        HTMLstrinh += '</tr>';

                    }
                    $("#add_comp_in_table").append(HTMLstrinh);

                }


            }
        }

    });
};
/*This Ajax use for Getting All Assessment */
function GetAllItemGroupChildNood() {
    var HTMLString = "";
    //sdocument.getElementById('1').innerHTML = "";        
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemGroupSetup/GetAllItemGroupChildNood",/*Controller=ItemGroupSetup and Fuction=GetAllItemGroupChildNood*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: '',/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/

                if (arr.Table.length > 0) {
                    for (i = 0; i < arr.Table.length; i++) {
                        HTMLString += '<option value="' + arr.Table[i].item_grp_cd + '">' + arr.Table[i].ItemGroupChildNood + '</option>';
                    }
                    $('#group_name_list').append(HTMLString);
                }
                else {
                }
            }
        },
        error: function (Data) {

        }
    });
};
function ErrorPage() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemSetup/ErrorPage",
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
function GetSelectedParentDetail(GroupID) {
    debugger;
    $("#spanGroupName").attr("style", "display: none;");
    $("[aria-labelledby='select2-group_name-container']").attr("style", "border-color: #ced4da;");

    var item_grp_id = GroupID.value;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemDetail/GetSelectedParentDetail",
                data: {
                    item_grp_id: item_grp_id
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
                    
                        if (arr.Table.length > 0) {
                            if (arr.Table[0].loc_sls_coa != null && arr.Table[0].loc_sls_coa != 0) {
                                $("#spanLocal_Sale_Account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-Local_Sale_Account-container']").attr("style", "border-color: #ced4da;");
                                $('#Local_Sale_Account').empty().append('<option value=' + arr.Table[0].loc_sls_coa + ' selected="selected">' + arr.Table[0].LocalSaleAccount + '</option>');
                            } else {
                                $('#Local_Sale_Account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].exp_sls_coa != null && arr.Table[0].exp_sls_coa != 0) {
                                $("#spanexport_sale_account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-export_sale_account-container']").attr("style", "border-color: #ced4da;");
                                $('#export_sale_account').empty().append('<option value=' + arr.Table[0].exp_sls_coa + ' selected="selected">' + arr.Table[0].ExportSaleAccount + '</option>');
                            } else {
                                $('#export_sale_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].loc_pur_coa != null && arr.Table[0].loc_pur_coa != 0) {
                                $("#spanlocal_purchase_account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-local_purchase_account-container']").attr("style", "border-color: #ced4da;");
                                $('#local_purchase_account').empty().append('<option value=' + arr.Table[0].loc_pur_coa + ' selected="selected">' + arr.Table[0].LocalPurchaseAccount + '</option>');
                            } else {
                                $('#local_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].imp_pur_coa != null && arr.Table[0].imp_pur_coa != 0) {
                                $("#spanimport_purchase_account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-import_purchase_account-container']").attr("style", "border-color: #ced4da;");
                                $('#import_purchase_account').empty().append('<option value=' + arr.Table[0].imp_pur_coa + ' selected="selected">' + arr.Table[0].ImportPurchaseAccount + '</option>');
                            } else {
                                $('#import_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].stk_coa != null && arr.Table[0].stk_coa != 0) {
                                $("#spanstock_account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-stock_account-container']").attr("style", "border-color: #ced4da;");
                                $('#stock_account').empty().append('<option value=' + arr.Table[0].stk_coa + ' selected="selected">' + arr.Table[0].StockAccount + '</option>');
                            } else {
                                $('#stock_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].sal_ret_coa != null && arr.Table[0].sal_ret_coa != 0) {
                                $("#spansale_return_account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-sale_return_account-container']").attr("style", "border-color: #ced4da;");
                                $('#sale_return_account').empty().append('<option value=' + arr.Table[0].sal_ret_coa + ' selected="selected">' + arr.Table[0].SaleReturnAccount + '</option>');
                            } else {
                                $('#sale_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].disc_coa != null && arr.Table[0].disc_coa != 0) {
                                $("#spandiscount_account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-discount_account-container']").attr("style", "border-color: #ced4da;");
                                $('#discount_account').empty().append('<option value=' + arr.Table[0].disc_coa + ' selected="selected">' + arr.Table[0].DiscountAccount + '</option>');
                            } else {
                                $('#discount_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].pur_ret_coa != null && arr.Table[0].pur_ret_coa != 0) {
                                $("#spanpurchase_return_account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-purchase_return_account-container']").attr("style", "border-color: #ced4da;");
                                $('#purchase_return_account').empty().append('<option value=' + arr.Table[0].pur_ret_coa + ' selected="selected">' + arr.Table[0].PurchaseReturnAccount + '</option>');
                            } else {
                                $('#purchase_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            //if (arr.Table[0].prov_pay_coa != null && arr.Table[0].prov_pay_coa != 0) {
                            //    $("#spanprovisional_payable_account").attr("style", "display: none;");
                            //    $("[aria-labelledby='select2-provisional_payable_account-container']").attr("style", "border-color: #ced4da;");
                            //    $('#provisional_payable_account').empty().append('<option value=' + arr.Table[0].prov_pay_coa + ' selected="selected">' + arr.Table[0].ProvisionalPayableAccount + '</option>');
                            //} else {
                            //    $('#provisional_payable_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //}
                            //if (arr.Table[0].cogs_coa != null && arr.Table[0].cogs_coa != 0) {
                            //    $("#spancost_of_goods_sold_account").attr("style", "display: none;");
                            //    $("[aria-labelledby='select2-cost_of_goods_sold_account-container']").attr("style", "border-color: #ced4da;");
                            //    $('#cost_of_goods_sold_account').empty().append('<option value=' + arr.Table[0].cogs_coa + ' selected="selected">' + arr.Table[0].CostofGoodsSoldAccount + '</option>');
                            //} else {
                            //    $('#cost_of_goods_sold_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //}
                            //if (arr.Table[0].cogs_adj_coa != null && arr.Table[0].cogs_adj_coa != 0) {
                            //    $("#spanstock_adjustment_account").attr("style", "display: none;");
                            //    $("[aria-labelledby='select2-stock_adjustment_account-container']").attr("style", "border-color: #ced4da;");
                            //    $('#stock_adjustment_account').empty().append('<option value=' + arr.Table[0].cogs_adj_coa + ' selected="selected">' + arr.Table[0].StockAdjustmentAccount + '</option>');
                            //} else {
                            //    $('#stock_adjustment_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //}
                            if (arr.Table[0].dep_coa != null && arr.Table[0].dep_coa != 0) {
                                $("#spandepreciation_account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-depreciation_account-container']").attr("style", "border-color: #ced4da;");
                                $('#depreciation_account').empty().append('<option value=' + arr.Table[0].dep_coa + ' selected="selected">' + arr.Table[0].DepreciationAccount + '</option>');
                            } else {
                                $('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].asset_coa != null && arr.Table[0].asset_coa != 0) {
                                $("#spanasset_account").attr("style", "display: none;");
                                $("[aria-labelledby='select2-asset_account-container']").attr("style", "border-color: #ced4da;");
                                $('#asset_account').empty().append('<option value=' + arr.Table[0].asset_coa + ' selected="selected">' + arr.Table[0].AssetAccount + '</option>');
                            } else {
                                $('#asset_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].interbr_sls_coa != null && arr.Table[0].interbr_sls_coa != 0) {
                                //$("#spanasset_account").attr("style", "display: none;");
                                //$("[aria-labelledby='select2-asset_account-container']").attr("style", "border-color: #ced4da;");
                                $('#IDInterBranch_sls_coa').empty().append('<option value=' + arr.Table[0].interbr_sls_coa + ' selected="selected">' + arr.Table[0].interbr_sls_Name + '</option>');
                            } else {
                                $('#IDInterBranch_sls_coa').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            if (arr.Table[0].interbr_pur_coa != null && arr.Table[0].interbr_pur_coa != 0) {
                                //$("#spanasset_account").attr("style", "display: none;");
                                //$("[aria-labelledby='select2-asset_account-container']").attr("style", "border-color: #ced4da;");
                                $('#IDInterBranch_pur_coa').empty().append('<option value=' + arr.Table[0].interbr_pur_coa + ' selected="selected">' + arr.Table[0].interbr_pur_Name + '</option>');
                            } else {
                                $('#IDInterBranch_pur_coa').empty().append('<option value="0" selected="selected">---Select---</option>');
                            }
                            $("#base_uom_cd").css("border-color", "#ced4da");
                            $("#spanBaseUom").text("");
                            $("#pur_uom_cd").css("border-color", "#ced4da");
                            $("#spanPurUom").text("");
                            $("#sl_uom_cd").css("border-color", "#ced4da");
                            $("#spanSalUom").text("");
                            //OnChangebaseUom();
                            //OnChangePurUom();
                            //OnChangeSalUom();
                        }
                        else {
                            debugger;
                            $('#Local_Sale_Account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#export_sale_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#local_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#import_purchase_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#stock_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#sale_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#discount_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#purchase_return_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //$('#provisional_payable_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //$('#cost_of_goods_sold_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            //$('#stock_adjustment_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#depreciation_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#asset_account').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#IDInterBranch_sls_coa').empty().append('<option value="0" selected="selected">---Select---</option>');
                            $('#IDInterBranch_pur_coa').empty().append('<option value="0" selected="selected">---Select---</option>');
                        }
                    }
                    var Ststus = $("#app_status").val();
                    if (Ststus != "Approved") {
                        GetItemPropertyToggleDetail(GroupID);
                    }
                    //GetItemPropertyToggleDetail(GroupID);
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function GetItemPropertyToggleDetail(GroupID) {
    debugger;
    $("#spanGroupName").attr("style", "display: none;");
    $("[aria-labelledby='select2-group_name-container']").attr("style", "border-color: #ced4da;");

    var item_grp_id = GroupID.value;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemDetail/GetItemPropertyToggleDetail",
                data: {
                    item_grp_id: item_grp_id
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
                        
                        if (arr.Table.length > 0) {
                           if (arr.Table[0].i_sls =='Y') {                               
                               $("#i_sls").prop("checked", true);
                               $("#i_catalog").prop("disabled", false);
                               }
                               else {
                               $("#i_sls").prop("checked", false);
                               $("#i_catalog").prop("disabled", true);
                               $("#i_catalog").prop("checked", false);
                            }
                            if (arr.Table[0].i_pur == 'Y') {
                                $("#i_pur").prop("checked", true);
                            }
                            else {
                                $("#i_pur").prop("checked", false);
                            }
                            if (arr.Table[0].i_wip == 'Y') {
                                $("#i_wip").prop("checked", true);
                            }
                            else {
                                $("#i_wip").prop("checked", false);
                            }
                            if (arr.Table[0].i_capg == 'Y') {
                                $("#i_capg").prop("checked", true);
                            }
                            else {
                                $("#i_capg").prop("checked", false);
                            }
                            if (arr.Table[0].i_stk == 'Y') {
                                $("#i_stk").prop("checked", true);
                                $("#requiredStk_coa").css("display", "inherit");
                               
                            }
                            else {
                                $("#i_stk").prop("checked", false);
                                $("#requiredStk_coa").attr("style", "display: none;");
                               // AutoGenerateRefNoAndCatlogno("N","N");
                            }
                            if (arr.Table[0].i_qc == 'Y') {
                                $("#i_qc").prop("checked", true);
                            }
                            else {
                                $("#i_qc").prop("checked", false);
                            }
                            if (arr.Table[0].i_srvc == 'Y') {
                                $("#i_srvc").prop("checked", true);
                                $("#requiredBaseUOMId").attr("style", "display: none;");
                                $("#requiredPurchaseUOMId").attr("style", "display: none;");
                                $("#requiredSaleUOMId").attr("style", "display: none;");
                            }
                            else {
                                $("#requiredBaseUOMId").css("display", "inherit");
                                $("#requiredPurchaseUOMId").css("display", "inherit");
                                $("#requiredSaleUOMId").css("display", "inherit");
                                $("#i_srvc").prop("checked", false);
                            }
                            if (arr.Table[0].i_cons == 'Y') {
                                $("#i_cons").prop("checked", true);
                            }
                            else {
                                $("#i_cons").prop("checked", false);
                            }
                            if (arr.Table[0].i_serial == 'Y') {
                                $("#i_serial").prop("checked", true);
                            }
                            else {
                                $("#i_serial").prop("checked", false);
                            }
                            if (arr.Table[0].i_sam == 'Y') {
                                $("#i_sam").prop("checked", true);
                            }
                            else {
                                $("#i_sam").prop("checked", false);
                            }
                            if (arr.Table[0].i_batch == 'Y') {
                                $("#i_batch").prop("checked", true);
                            }
                            else {
                                $("#i_batch").prop("checked", false);
                            }
                            if (arr.Table[0].i_exp == 'Y') {
                                $("#i_exp").prop("checked", true);
                                $("#ExpiryAlertDays").css("display", "")
                            }
                            else {
                                $("#i_exp").prop("checked", false);
                                $("#ExpiryAlertDt").val(0);
                                $("#ExpiryAlertDays").css("display", "none")
                            }
                            if (arr.Table[0].i_pack == 'Y') {
                                $("#i_pack").prop("checked", true);
                                $("#PackdetailAccord").css("display", "block");
                                $("#collapseTwelve").removeClass('collapse');
                            }
                            else {
                                $("#i_pack").prop("checked", false);
                                $("#PackdetailAccord").css("display", "none");
                                $("#collapseTwelve").addClass('collapse');
                            }
                            if (arr.Table[0].i_catalog == 'Y') {
                                $("#i_catalog").prop("checked", true);
                            }
                            else {
                                $("#i_catalog").prop("checked", false);
                            }

                        }
                        else {
                            debugger;
                            $("#i_sls").prop("checked", false);
                            $("#i_pur").prop("checked", false);
                            $("#i_wip").prop("checked", false);
                            $("#i_capg").prop("checked", false);
                            $("#i_stk").prop("checked", false);
                            $("#i_qc").prop("checked", false);
                            $("#i_srvc").prop("checked", false);
                            $("#i_cons").prop("checked", false);
                            $("#i_serial").prop("checked", false);
                            $("#i_sam").prop("checked", false);
                            $("#i_batch").prop("checked", false);
                            $("#i_exp").prop("checked", false);
                            $("#i_pack").prop("checked", false);
                            $("#i_catalog").prop("checked", false);

                            $("#ExpiryAlertDt").val(0);
                            $("#ExpiryAlertDays").css("display", "none")
                            $("#requiredBaseUOMId").css("display", "inherit");
                            $("#requiredPurchaseUOMId").css("display", "inherit");
                            $("#requiredSaleUOMId").css("display", "inherit");
                            $("#requiredStk_coa").attr("style", "display: none;");
                        }
                        RlvntAccountLinkingMendatory("", "");
                        if (refNo == "" || refNo == null) {
                            AutoGenerateRefNoAndCatlogno();
                        }
                        else {
                            generateCatalogNo(refNo)
                        }
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
    debugger;
    var refNo = $("#item_leg_cd").val();

    
}
function AutoGenerateRefNoAndCatlogno() {
    var Ststus = $("#app_status").val();
    if (Ststus == "") {
        var stockable = $("#i_stk").is(":checked") ? "Y" : "N";
        var saleable = $("#i_sls").is(":checked") ? "Y" : "N";
        var ItemCatalog = $("#i_catalog").is(":checked") ? "Y" : "N";
        var AutoGen_Ref_noParameter = $("#hdnAutoGen_Ref_noParameter").val();

        if (stockable === "Y" && AutoGen_Ref_noParameter == "Y") {
            $.ajax({
                type: "POST",
                url: "/BusinessLayer/ItemDetail/AutoGenerateRef_NoAndCatlog_no",
                dataType: "json",
                data: {
                    stockable: stockable,
                    saleable: saleable,
                    ItemCatalog: ItemCatalog
                },
                success: function (data) {
                    debugger;

                    if (data) {
                        if (data.Generated_REF_NO) {
                            $("#item_leg_cd").val(data.Generated_REF_NO);
                        }
                        if (data.CATALOG_NO) {
                            $("#CatlNo").val(data.CATALOG_NO);
                        }
                    }
                },
                error: function (err) {
                    console.error("Error in AutoGenerateRefNoAndCatlogno:", err);
                }
            });
        }
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
       // $("#i_cons").prop("checked", false);
        //----------------------------------//
        $("#i_srvc").prop("checked", false);
        $("#i_cons").prop("checked", false);
        //----------------------------------//
        $("#requiredStk_coa").css("display", "inherit");
        var refNo = $("#item_leg_cd").val();
        if (refNo == "" || refNo == null) {
            AutoGenerateRefNoAndCatlogno();
        }
        else {
            generateCatalogNo(refNo)
        }
    }
    else {
        $("#i_srvc").prop("checked", true);
        $("#i_wip").prop("checked", false);
        if ($("#hdnIDdependcy_i_capg").val() == "i_capg_NotEdit") {
            //$("#i_capg").prop("checked", true);
        }
        else {
            $("#i_capg").prop("checked", false);
        }
      //  $("#i_capg").prop("checked", false);
        $("#i_qc").prop("checked", false);
        $("#i_cons").prop("checked", false);
        $("#i_serial").prop("checked", false);
        $("#i_sam").prop("checked", false);
        $("#i_batch").prop("checked", false);
        $("#i_exp").prop("checked", false);
        $("#i_pack").prop("checked", false);
        $("#i_pack").prop("disabled", true);

        $("#ExpiryAlertDt").val(0);
        $("#ExpiryAlertDays").css("display", "none")
        $("#PackdetailAccord").css("display", "none");
        //----------------------------------//
        $("#requiredStk_coa").attr("style", "display: none;");
        $("#item_leg_cd").val("");
        $("#CatlNo").val("");
    }
    RlvntAccountLinkingMendatory("","");
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
        $("#i_stk").prop("checked", true);
        $("#i_cons").prop("checked", false);

        $("#requiredStk_coa").css("display", "inherit");
    }  
    RlvntAccountLinkingMendatory("", "");
}
function OnClickConsToggle() {
    debugger;
    var constgl = "";
    if ($("#i_cons").is(":checked")) {
        constgl = 'Y';
    }
    else {
        constgl = 'N';
    }
    if (constgl == 'Y') {
        //$("#i_srvc").prop("checked", false);
        //$("#i_stk").prop("checked", false);
        //$("#i_batch").prop("checked", false);
        $("#i_exp").prop("checked", false);
        //$("#i_serial").prop("checked", false);
        //---------------------------------------------//
        $("#i_srvc").prop("checked", false);
        $("#i_stk").prop("checked", false);
        $("#i_batch").prop("checked", false);
        $("#i_serial").prop("checked", false);
        $("#SubItem").prop("checked", false);
        $("#i_qc").prop("checked", false);
        $("#i_capg").prop("checked", false);
        $("#i_wip").prop("checked", false);
        $("#i_pack").prop("checked", false);
        $("#i_sam").prop("checked", false);
       // $("#i_sls").prop("checked", false);
       // $("#i_exempted").prop("checked", false);
      //  $("#i_catalog").prop("checked", false);

        $("#ExpiryAlertDt").val(0);
        $("#ExpiryAlertDays").css("display", "none")
        $("#requiredStk_coa").attr("style", "display: none;");

    }
    
    RlvntAccountLinkingMendatory("", "");
}
function onclickcatalog() {
    debugger;
    var catalog = "";
    if ($("#i_catalog").is(":checked")) {
        catalog = 'Y';
       
    }
    else {
        catalog = 'N';
    }
    if (catalog == 'Y') {
        // $("#i_cons").prop("checked", false);
        if ($("#i_stk").is(":checked") && $("#i_sls").is(":checked")) {
            var refNo = $("#item_leg_cd").val();
            if (refNo == "" || refNo == null) {
                AutoGenerateRefNoAndCatlogno();
            }
            else {
                generateCatalogNo(refNo)
            }



        }
    }
    else {
        $("#CatlNo").val("");
    }
    
   
}
function isNumeric(value) {
    return /^\d+$/.test(value);  // Only digits 0-9
}

function generateCatalogNo(refNo) {
    var Ststus = $("#app_status").val();
    if (Ststus == "") {
        var stockable = $("#i_stk").is(":checked") ? "Y" : "N";
        var saleable = $("#i_sls").is(":checked") ? "Y" : "N";
        var ItemCatalog = $("#i_catalog").is(":checked") ? "Y" : "N";
        var AutoGen_catalogNoParameter = $("#hdnAutoGen_catalogNoParameter").val();
        if (stockable === "Y" && saleable === "Y" && ItemCatalog === "Y" && AutoGen_catalogNoParameter === "Y") {
            const mapping = {
                '0': '9', '1': '3', '2': '7', '3': '2', '4': '5',
                '5': '1', '6': '0', '7': '4', '8': '8', '9': '6'
            };
            refNo = refNo.trim();
            if (!isNumeric(refNo)) {
                $("#CatlNo").val("");
            }
            else {
                let result = '';
                for (let i = 0; i < refNo.length; i++) {
                    const digit = refNo.charAt(i);
                    result += mapping[digit];
                }

                $("#CatlNo").val(result);
            }

            
        }

        else {
            $("#CatlNo").val("");
        }
    }
}
function onclickExempted() {
    var exempted = "";
    if ($("#i_exempted").is(":checked")) {
        exempted = 'Y';
    }
    else {
        exempted = 'N';
    }
    if (exempted == 'Y') {
      //  $("#i_cons").prop("checked", false);
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
    debugger;
    var constgl = "";
    if ($("#i_qc").is(":checked")) {
        constgl = 'Y';
    }
    else {
        constgl = 'N';
    }
    if (constgl == 'Y') {

       // $("#i_srvc").prop("checked", false);
       // $("#i_stk").prop("checked", true); //.trigger("checked";

       // $("#requiredStk_coa").css("display", "inherit");
        //OnClickStockToggle();
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
        $("#i_exempted").prop("checked", false);
        $("#SubItem").prop("checked", false);
        $("#i_ws").prop("checked", false);

        $("#i_sls").prop("checked", true);
        $("#i_pur").prop("checked", true);
        $("#i_stk").prop("checked", true);
        $("#i_serial").prop("checked", true);
        $("#i_qc").prop("checked", true);

        $("#requiredStk_coa").css("display", "inherit");
    }
    RlvntAccountLinkingMendatory("","");
}
function OnClickServiceToggle() {
    debugger;
    var srvctgl = "";
    if ($("#i_srvc").is(":checked")) {
        $("#requiredBaseUOMId").attr("style", "display: none;");
        $("#requiredPurchaseUOMId").attr("style", "display: none;");
        $("#requiredSaleUOMId").attr("style", "display: none;");

        $("#base_uom_cd").css("border-color", "#ced4da");
        $("#spanBaseUom").text("");
        $("#pur_uom_cd").css("border-color", "#ced4da");
        $("#spanPurUom").text("");
        $("#sl_uom_cd").css("border-color", "#ced4da");
        $("#spanSalUom").text("");
        srvctgl = 'Y';
    }
    else {
        $("#requiredBaseUOMId").css("display", "inherit");
        $("#requiredPurchaseUOMId").css("display", "inherit");
        $("#requiredSaleUOMId").css("display", "inherit");
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
        $("#PackdetailAccord").css("display", "none");
        //------------------------------------------//
        $("#i_cons").prop("checked", false);
        $("#i_stk").prop("checked", false);
        $("#i_batch").prop("checked", false);
        $("#i_serial").prop("checked", false);
        $("#SubItem").prop("checked", false);
      //  $("#i_qc").prop("checked", false);
        $("#i_capg").prop("checked", false);
        $("#i_wip").prop("checked", false);
        $("#i_pack").prop("checked", false);
        $("#i_sam").prop("checked", false);
        $("#i_exp").prop("checked", false);

        $("#ExpiryAlertDt").val(0);
        $("#ExpiryAlertDays").css("display", "none")
        $("#requiredStk_coa").attr("style", "display: none;");
    }
    else {
        $("#i_stk").prop("checked", true);
        $("#i_pack").prop("disabled", false);

        $("#requiredStk_coa").css("display", "inherit");
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
        if ($("#i_exp").is(":checked")) {
            $("#ExpiryAlertDt").val(30);
        }
        else {
            $("#ExpiryAlertDt").val(0);
        }
        $("#ExpiryAlertDays").css("display", "")
        $("#requiredStk_coa").css("display", "inherit");
    }
    else {
        $("#i_exp").prop("checked", false);
        $("#ExpiryAlertDt").val(0);
        $("#ExpiryAlertDays").css("display", "none")
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
        $("#ExpiryAlertDt").val(0);
        $("#ExpiryAlertDays").css("display", "none")
    }
    if (exptgl == 'Y') {
      
        $("#i_batch").prop("checked", true);
        $("#i_serial").prop("checked", false);
        $("#i_srvc").prop("checked", false);
        $("#i_stk").prop("checked", true);
        $("#i_cons").prop("checked", false);

        $("#requiredStk_coa").css("display", "inherit");

        $("#ExpiryAlertDays").css("display","")
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
        $("#i_cons").prop("checked", false);
        $("#i_pack").prop("disabled", false);

        $("#ExpiryAlertDays").css("display", "none")
        $("#ExpiryAlertDt").val(0);
        $("#requiredStk_coa").css("display", "inherit");
    }
    else {
        if ($("#hdnIDdependcy_i_capg").val() == "i_capg_NotEdit") {
           // $("#i_capg").prop("checked", true);
        }
        else {
            $("#i_capg").prop("checked", false);
        }
       // $("#i_capg").prop("checked", false);
        
    }
    RlvntAccountLinkingMendatory("", "");
}
function OnClickPackagingToggle() {
    debugger;
    var Packagingtgl = "";
    if ($("#i_pack").is(":checked")) {
        Packagingtgl = 'Y';
    }
    else {
        $("#spanlength").text("");
        $("#Length").css("border-color", "#ced4da");
        $("#spanwidth").text("");
        $("#Width").css("border-color", "#ced4da");
        $("#spanheight").text("");
        $("#Height").css("border-color", "#ced4da");
        Packagingtgl = 'N';
    }
    if (Packagingtgl == 'Y') {
        $("#PackdetailAccord").css("display", "block");
        $("#collapseTwelve").removeClass('collapse');
        $("#i_cons").prop("checked", false);
    }
    else {
        $("#PackdetailAccord").css("display", "none");
        $("#collapseTwelve").addClass('collapse');
    }
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
        //$("#i_cons").prop("checked", false);
        if ($("#i_stk").is(":checked") && $("#i_catalog").is(":checked")) {
            var refNo = $("#item_leg_cd").val();
            if (refNo == "" && refNo == null) {
                AutoGenerateRefNoAndCatlogno();
            }
            else {
                generateCatalogNo(refNo)
            }
        }
    }
    else {
        $("#CatlNo").val("");
        $("#i_catalog").prop("disabled", true);
        $("#i_catalog").prop("checked", false);
        if ($("#hdnIDdependcy_i_capg").val() == "i_capg_NotEdit") {
           // $("#i_capg").prop("checked", true);
        }
        else {
            $("#i_capg").prop("checked", false);
        }
     
      
      
    }
    RlvntAccountLinkingMendatory("", "i_sls");
   
}
function OnClickPurchaseToggle() {
    if ($("#i_pur").is(":checked")) {


    }
    else {
        if ($("#hdnIDdependcy_i_capg").val() == "i_capg_NotEdit")
        {
            //$("#i_capg").prop("checked", true);
        }
        else {
            $("#i_capg").prop("checked", false);
        }
    }
    RlvntAccountLinkingMendatory("","i_pur");
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
function OnclickItemname() {
    debugger;
    onpasteValueDiscard();
    var ItemName = $('#item_name').val().trim();
    if (ItemName != "") {
        $("#spanItemName").text("");
        $("#item_name").css("border-color", "#ced4da");
    }
    else {
      
        $("#item_name").css("border-color", "red");
        $("#spanItemName").text($("#valueReq").text());
        $("#spanItemName").css("display", "block");
    }

}
function ItemNameKeyDown(event) {
    debugger;
    let value = event.keyCode;
    
    if (value == 188 || event.key === "_" ) {
        debugger;
        $("#item_name").css("border-color", "red");
        $("#spanItemName").text($("#span_AreNotAllowedInItemName").text());
       // document.getElementById("spanItemName").innerText = "(_ ,) are not allowed in item name.";
        $("#spanItemName").css("font-weight","bold");
        $("#spanItemName").css("display", "block");
        return false;
    }
}

function GetDataOnpaste() {
    debugger;
    var itemname = ClipboardEvent.value;
}
function OnChangebaseUom() {
    debugger;
    var Baseuom = $('#base_uom_cd').val().trim();
    if (Baseuom != "0") {
        $("#spanBaseUom").text("");
        $("#base_uom_cd").css("border-color", "#ced4da");
    }
    else {

        $("#base_uom_cd").css("border-color", "red");
        $("#spanBaseUom").text($("#valueReq").text());
        $("#spanBaseUom").css("display", "block");
    }
    base_uom_cdOnChange();
}
//function OnChangePurUom() {
//    debugger;
//    var Puruom = $('#pur_uom_cd').val().trim();
//    if (Puruom != "0") {
//        $("#spanPurUom").text("");
//        $("#pur_uom_cd").css("border-color", "#ced4da");
//    }
//    //else {
//    //    $("#pur_uom_cd").css("border-color", "red");
//    //    $("#spanPurUom").text($("#valueReq").text());
//    //    $("#spanPurUom").css("display", "block");
//    //}
//}
//function OnChangeSalUom() {
//    debugger;
//    var Saleuom = $('#sl_uom_cd').val().trim();
//    if (Saleuom != "0") {
//        $("#spanSalUom").text("");
//        $("#sl_uom_cd").css("border-color", "#ced4da");
//    }
//    //else {
//    //    $("#sl_uom_cd").css("border-color", "red");
//    //    $("#spanSalUom").text($("#valueReq").text());
//    //    $("#spanSalUom").css("display", "block");   
//    //}
//}
function OnActiveStatusChange() {
    debugger;
    if ($("#act_stats").is(":checked")) {
        $("#inact_reason").val("");
        $("#inact_reason").prop("readonly", true);
        $("#SpanItminactreason").text("");
        $("#inact_reason").css("border-color", "#ced4da");
        $("#InactiveRemarksReq").attr("style", "display: none;");

    }
    else {
        $("#inact_reason").prop("readonly", false);
        $("#inact_reason").attr("style", "border-color:#ff0000;", "required");
        $("#SpanItminactreason").text($("#valueReq").text());
      $("#SpanItminactreason").css("display", "block");
        $("#InactiveRemarksReq").css("display", "inherit");
    }
}
function OnchangeReason() {
    debugger;
    $("#SpanItminactreason").text("");
    $("#inact_reason").css("border-color", "#ced4da");
    //$("#InactiveRemarksReq").attr("style", "display: none;");
}
function OnchangeCost() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var costval = $("#bs_price").val();
    if (AvoidDot(costval) == false) {
        costval = 0;
    }
    $("#bs_price").val(parseFloat(costval).toFixed(RateDecDigit));
}
function OnchangePrice() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var costval = $("#pr_price").val();
    if (AvoidDot(costval) == false) {
        costval = 0;
    }
    //var abc = currencyFormat1(costval);
    $("#pr_price").val(parseFloat(costval).toFixed(RateDecDigit));
    //$("#pr_price").val(abc);
    
}

function OnchangeMinStkLvl() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var minstk = $("#min_stk_lvl").val();
    if (AvoidDot(minstk) == false) {
        minstk = 0;
    }
    $("#min_stk_lvl").val(parseFloat(minstk).toFixed(QtyDecDigit));
}
function OnchangeMinPurStkLvl() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var purstk = $("#min_pr_stk").val();
    if (AvoidDot(purstk) == false) {
        purstk = 0;
    }
    $("#min_pr_stk").val(parseFloat(purstk).toFixed(QtyDecDigit));
}
function OnchangeReOrdStkLvl() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var reordqty = $("#re_ord_lvl").val();
    if (AvoidDot(reordqty) == false) {
        reordqty = 0;
    }
    $("#re_ord_lvl").val(parseFloat(reordqty).toFixed(QtyDecDigit));
}
//-------------------------------------------CustInfo--------------------------//
function BindDLLCustomerList() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/BusinessLayer/ItemDetail/GetCustListAuto",
            data: function (params) {
                var queryParameters = {
                    CustName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                // debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        sessionStorage.removeItem("CustomerList");
                        sessionStorage.setItem("CustomerList", JSON.stringify(arr.Table));

                        BindData();

                    }
                }
            },
        });
}
function BindCustomerList(ID) {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/BusinessLayer/ItemDetail/GetCustListAuto",
            data: function (params) {
                var queryParameters = {
                    CustName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        //$('#CustomerName_' + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#CustomerName_' + ID).append(`<option value="${arr.Table[i].cust_id}">${arr.Table[i].cust_name}</option>`);
                        }
                        var firstEmptySelect = true;
                        //$('#CustomerName' + ID).select2({

                        //    processResults: function (data, params) {
                        //        if (data == 'ErrorPage') {
                        //            LSO_ErrorPage();
                        //            return false;
                        //        }
                        //        params.page = params.page || 1;
                        //        return {

                        //            results: $.map(data, function (val, item) {
                        //                return { id: val.ID, text: val.Name };
                        //            })
                        //        };
                        //    }
                        //});

                        $('#CustomerName_' + ID).select2({
                            templateResult: function (data) {                             
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +                                   
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });
                        // debugger;
                        HideCustomerList(ID);
                    }
                }
            },
        });
}
function HideCustomerList(ID) {
    $("#CustomerDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var ItemCode;

        ItemCode = currentRow.find("#CustomerName_" + Sno).val();

        if (ItemCode != '0') {
            if (currentRow.find("#CustomerName_" + Sno).val() != ItemCode) {
                $("#CustomerName_ option[value=" + ItemCode + "]").select2().hide();
            }
            if (Sno != "0") {
            $("#CustomerDetailsTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRowD = $(this);
                var SnoD = currentRowD.find("#SNohiddenfiled").val();
                var CustomerCodeD;

                CustomerCodeD = currentRowD.find("#CustomerName_" + SnoD).val();

                if (CustomerCodeD != '0') {
                    if (currentRow.find("#CustomerName_" + Sno).val() !== CustomerCodeD) {
                        $("#CustomerName_" + Sno + " option[value=" + CustomerCodeD + "]").select2().hide();
                    }
                }
            })
            }
            $("#CustomerName_" + ID + " option[value=" + ItemCode + "]").select2().hide();
        }
        else {
            $("#CustomerDetailsTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRowD = $(this);
                var SnoD = currentRowD.find("#SNohiddenfiled").val();
                var CustomerCodeD;

                CustomerCodeD = currentRowD.find("#CustomerName_" + SnoD).val();

                if (CustomerCodeD != '0') {
                    if (currentRow.find("#CustomerName_" + Sno).val() !== CustomerCodeD) {
                        $("#CustomerName_" + Sno + " option[value=" + CustomerCodeD + "]").select2().hide();
                    }
                }
            })
        }
    });
}
function OnChangeCustomer(RowID, e) {
    // debugger;
    BindCustList(e);
}
function BindCustList(e) {
    debugger;   
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var CustomerID = clickedrow.find("#hfCustID").val();   
    var CustID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    CustID = clickedrow.find("#CustomerName_" + SNo).val();
    clickedrow.find("#hfCustID").val(CustID);

    if (CustID == "0") {
        clickedrow.find("#CustNameError").text($("#valueReq").text());
        clickedrow.find("#CustNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    }
    else {
        clickedrow.find("#CustNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    }
    //clickedrow.find("#SOItemListNameError" + SNo).css("display", "none");
    //clickedrow.find("[aria-labelledby='select2-SOItemListName" + SNo + "-container']").css("border", "1px solid #ced4da");
    ShowCustomerList(CustomerID);
    HideCustomerList(SNo);
}
function ShowCustomerList(CustID) {
    debugger;
    if (CustID != "0" && CustID !="" ) {
        $("#CustomerDetailsTbl >tbody >tr").each(function () {
            // debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            $("#CustomerName_" + Sno + " option[value=" + CustID + "]").removeClass("select2-hidden-accessible");
        });
    }
}
function AddNewRow() {
   
    var rowIdx = 0;
    // debugger;    
    var rowCount = $('#CustomerDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;

    $("#CustomerDetailsTbl >tbody >tr").each(function (i, row) {
        // debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    
    $('#CustomerDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"></td>
<td><div class="col-sm-11 lpo_form no-padding"><select class="form-control" onchange ="OnChangeCustomer(${RowNo},event)" id="CustomerName_${RowNo}"></select>
<input  type="hidden" id="hfCustID" />
<span id="CustNameError" class="error-message is-visible"></span></div></td>
<td><input id="ItemCode" class="form-control date" autocomplete="off" type="text" name="ItemCode" maxlength="25" placeholder="${$("#span_ItemCode").text()}"  onblur="this.placeholder='${$("#span_ItemCode").text()}'"></td>
<td><input id="ItemDescription" class="form-control date" autocomplete="off" type="text" maxlength="500" name="ItemDescription" placeholder="${$("#span_ItemDescription").text()}"  onblur="this.placeholder='${$("#span_ItemDescription").text()}'"></td>
<td><input id="PackingDetail" class="form-control date" autocomplete="off" type="text" maxlength="50" name="PackingDetail" placeholder="${$("#Span_PackingDetail_Title").text()}"  onblur="this.placeholder='${$("#Span_PackingDetail_Title").text()}'"></td>

<td><textarea id="ItemRemarks" class="form-control remarksmessage" name="ItemRemarks" maxlength="500" placeholder="${$("#span_remarks").text()}"></textarea></td>
</tr>`);
    BindCustomerList(RowNo);
};
function BindData() {
    debugger;
    var PLCustListData = JSON.parse(sessionStorage.getItem("CustomerList"));
    if (PLCustListData != null) {
        if (PLCustListData.length > 0) {
            $("#CustomerDetailsTbl >tbody >tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohiddenfiled").val();

                //$("#CustomerName_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#SOItemName").text()}' data-uom='${$("#SOItemUOM").text()}'></optgroup>`);
                for (var i = 0; i < PLCustListData.length; i++) {
                    $("#CustomerName_" + rowid).append(`<option value="${PLCustListData[i].cust_id}">${PLCustListData[i].cust_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#CustomerName_" + rowid).select2({
                    templateResult: function (data) {
                        //var UOM = $(data.element).data('uom');
                        var classAttr = $(data.element).attr('class');
                        var hasClass = typeof classAttr != 'undefined';
                        classAttr = hasClass ? ' ' + classAttr : '';
                        var $result = $(
                            '<div class="row">' +
                            '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
                            //'<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
                            '</div>'
                        );
                        return $result;
                        firstEmptySelect = false;
                    }
                });

            });
        }
    }

    $("#CustomerDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var CusID = currentRow.find("#hfCustID").val();
        var rowid = currentRow.find("#SNohiddenfiled").val();
        var CustID = '#CustomerName_' + rowid;
        if (CusID != '0' && CusID != "") {
            currentRow.find("#CustomerName_" + rowid).val(CusID).trigger('change.select2');
        }

    });

    $("#CustomerDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var CusID = currentRow.find("#hfCustID").val();
        var rowid = currentRow.find("#SNohiddenfiled").val();

        var CustomerCode;
        CustomerCode = CusID;

        if (CustomerCode != '0' && CustomerCode != "") {

            $("#CustomerDetailsTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRowD = $(this);
                var CustomerCodeD;
                CustomerCodeD = currentRowD.find("#hfCustID").val();
                var rowidD = currentRowD.find("#SNohiddenfiled").val();
                if (CustomerCodeD != '0' && CustomerCodeD != "") {
                    if (currentRow.find("#CustomerName_" + rowidD).val() != CustomerCode) {
                        $("#CustomerName_" + rowid + " option[value=" + CustomerCodeD + "]").select2().hide();
                    }
                }
            })
        }
    });
}
function CheckCustomerValidations() {
    debugger;
    var rowCount = $('#CustomerDetailsTbl >tbody >tr').length;
   if (rowCount > 0) {
        var ErrorFlag = "N";
        $("#CustomerDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#CustomerName_" + Sno).val() == "0") {
                currentRow.find("#CustNameError").text($("#valueReq").text());
                currentRow.find("#CustNameError").css("display", "block");
                debugger;
                currentRow.find("[aria-labelledby='select2-CustomerName_" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
        });
       if (ErrorFlag == "Y") {
           return false;
       }
       //else {
       //    return true;
       //}
    }
    var rowCount1 = $('#SupplierDetailsTbl >tbody >tr').length;
    if (rowCount1 > 0) {
        var ErrorFlag = "N";
        $("#SupplierDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno1 = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#SupplierName_" + Sno1).val() == "0") {
                currentRow.find("#SuppNameError").text($("#valueReq").text());
                currentRow.find("#SuppNameError").css("display", "block");
                debugger;
                currentRow.find("[aria-labelledby='select2-SupplierName_" + Sno1 + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
        });
        if (ErrorFlag == "Y") {
            return false;
        }
        //else {
        //    return true;
        //}
    }
    var rowCount2 = $('#PortfolioTbl >tbody >tr').length;
    if (rowCount2 > 0) {
        var ErrorFlag = "N";
        $("#PortfolioTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno2 = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#PortfolioName_" + Sno2).val() == "0") {
                currentRow.find("#PortfolioNameError").text($("#valueReq").text());
                currentRow.find("#PortfolioNameError").css("display", "block");
                debugger;

                currentRow.find("[aria-labelledby='select2-PortfolioName_" + Sno2 + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            
        });
        if (ErrorFlag == "Y") {
            return false;
        }
        //else {
        //    return true;
        //}
    }
    var rowCount3 = $('#VehicleTbl >tbody >tr').length;
    if (rowCount3 > 0) {
        var ErrorFlag = "N";
        $("#VehicleTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno3 = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#VehicleName_" + Sno3).val() == "0") {
                currentRow.find("#VehcNameError").text($("#valueReq").text());
                currentRow.find("#VehcNameError").css("display", "block");
                debugger;

                currentRow.find("[aria-labelledby='select2-VehicleName_" + Sno3 + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            
        });
        if (ErrorFlag == "Y") {
            return false;
        }
        else {
            return true;
        }
    }

        if (ErrorFlag == "Y") {
            return false;
        }
        else {
            return true;
        }
    
    
}

function CheckCustomerValidations1() {
    var rowCount = $('#CustomerDetailsTbl >tbody >tr').length;
    var rowCount1 = $('#SupplierDetailsTbl >tbody >tr').length;
    var rowCount2 = $('#PortfolioTbl >tbody >tr').length;
    var rowCount3 = $('#VehicleTbl >tbody >tr').length;
    var rowCount4 = $('#NewSubItemtable >tbody >tr').length;

    if (rowCount > 0 || rowCount1 > 0 || rowCount2 > 0 || rowCount3 > 0 || rowCount4 > 0) {
        var ErrorFlag = "N";

        $("#CustomerDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#CustomerName_" + Sno).val() == "0") {
                currentRow.find("#CustNameError").text($("#valueReq").text());
                currentRow.find("#CustNameError").css("display", "block");
                debugger;
                currentRow.find("[aria-labelledby='select2-CustomerName_" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
        });

        $("#SupplierDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno1 = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#SupplierName_" + Sno1).val() == "0") {
                currentRow.find("#SuppNameError").text($("#valueReq").text());
                currentRow.find("#SuppNameError").css("display", "block");
                debugger;
                currentRow.find("[aria-labelledby='select2-SupplierName_" + Sno1 + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }

        });
        $("#PortfolioTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno2 = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#PortfolioName_" + Sno2).val() == "0") {
                currentRow.find("#PortfolioNameError").text($("#valueReq").text());
                currentRow.find("#PortfolioNameError").css("display", "block");
                debugger;

                currentRow.find("[aria-labelledby='select2-PortfolioName_" + Sno2 + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }

        });

        $("#VehicleTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno3 = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#VehicleName_" + Sno3).val() == "0") {
                currentRow.find("#VehcNameError").text($("#valueReq").text());
                currentRow.find("#VehcNameError").css("display", "block");
                debugger;

                currentRow.find("[aria-labelledby='select2-VehicleName_" + Sno3 + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }

        });
        if (ErrorFlag == "Y") {
            return false;
        }
        else {
            return true;
        }
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
//-------------------------------------------CustInfo END--------------------------//
//-------------------------------------------Portfolio Info Start--------------------------//
function BindDLLPortfolioList() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/BusinessLayer/ItemDetail/GetPortfolioListAuto",
            data: function (params) {
                var queryParameters = {
                    PortfName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                // debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        sessionStorage.removeItem("PortfolioList");
                        sessionStorage.setItem("PortfolioList", JSON.stringify(arr.Table));

                        BindPortfolioData();

                    }
                }
            },
        });
}
function BindPortfolioList(ID) {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/BusinessLayer/ItemDetail/GetPortfolioListAuto",
            data: function (params) {
                var queryParameters = {
                    CustName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#PortfolioName_' + ID).append(`<option value="${arr.Table[i].setup_id}">${arr.Table[i].setup_val}</option>`);
                        }
                        var firstEmptySelect = true;  
                        $('#PortfolioName_' + ID).select2({
                            templateResult: function (data) {
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });
                        // debugger;
                        HidePortfolioList(ID);
                    }
                }
            },
        });
}
function HidePortfolioList(ID) {
    $("#PortfolioTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var ItemCode;

        ItemCode = currentRow.find("#PortfolioName_" + Sno).val();

        if (ItemCode != '0') {
            if (currentRow.find("#PortfolioName_" + Sno).val() != ItemCode) {
                $("#PortfolioName_ option[value=" + ItemCode + "]").select2().hide();
            }
            if (Sno != "0") {
                $("#PortfolioTbl >tbody >tr").each(function (i, row) {
                    debugger;
                    var currentRowD = $(this);
                    var SnoD = currentRowD.find("#SNohiddenfiled").val();
                    var PortfolioCodeD;

                    PortfolioCodeD = currentRowD.find("#PortfolioName_" + SnoD).val();

                    if (PortfolioCodeD != '0') {
                        if (currentRow.find("#PortfolioName_" + Sno).val() !== PortfolioCodeD) {
                            $("#PortfolioName_" + Sno + " option[value=" + PortfolioCodeD + "]").select2().hide();
                        }
                    }
                })
            }
            $("#PortfolioName_" + ID + " option[value=" + ItemCode + "]").select2().hide();
        }
        else {
            $("#PortfolioTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRowD = $(this);
                var SnoD = currentRowD.find("#SNohiddenfiled").val();
                var PortfolioCodeD;

                PortfolioCodeD = currentRowD.find("#PortfolioName_" + SnoD).val();

                if (PortfolioCodeD != '0') {
                    if (currentRow.find("#PortfolioName_" + Sno).val() !== PortfolioCodeD) {
                        $("#PortfolioName_" + Sno + " option[value=" + PortfolioCodeD + "]").select2().hide();
                    }
                }
            })
        }
    });
}
function OnChangePortfolio(RowID, e) {
    // debugger;
    BindPrfList(e);
}
function BindPrfList(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var PortfolioID = clickedrow.find("#hfPrfID").val();
    var CustID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    CustID = clickedrow.find("#PortfolioName_" + SNo).val();
    clickedrow.find("#hfPrfID").val(CustID);

    if (CustID == "0") {
        clickedrow.find("#PortfolioNameError").text($("#valueReq").text());
        clickedrow.find("#PortfolioNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    }
    else {
        clickedrow.find("#PortfolioNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    }

    ShowPortfolioList(PortfolioID);
    HidePortfolioList(SNo);
}
function ShowPortfolioList(PrfID) {
    debugger;
    if (PrfID != "0" && PrfID != "") {
        $("#PortfolioTbl >tbody >tr").each(function () {
            // debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            $("#PortfolioName_" + Sno + " option[value=" + PrfID + "]").removeClass("select2-hidden-accessible");
        });
    }
}
function AddNewPrfRow() {
    var rowIdx = 0;
    // debugger;    
    var rowCount = $('#PortfolioTbl >tbody >tr').length + 1;
    var RowNo = 0;

    $("#PortfolioTbl >tbody >tr").each(function (i, row) {
        // debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#PortfolioTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"></td>
<td><div class="col-sm-11 lpo_form no-padding" ><select class="form-control" onchange ="OnChangePortfolio(${RowNo},event)" id="PortfolioName_${RowNo}"></select>
<input  type="hidden" id="hfPrfID" />
<span id="PortfolioNameError" class="error-message is-visible"></span></div></td>
<td><input id="PortfolioDescription" class="form-control date" autocomplete="off" type="text" maxlength="50" name="PortfolioDescription" placeholder="${$("#span_PortfolioDescription").text()}"  onblur="this.placeholder='${$("#span_PortfolioDescription").text()}'"></td>
<td><textarea id="portfRemarks" class="form-control remarksmessage" name="portfRemarks" maxlength="50" placeholder="${$("#span_remarks").text()}"></textarea></td>
</tr>`);
    BindPortfolioList(RowNo);
};
function BindPortfolioData() {
    debugger;
    var PLPortListData = JSON.parse(sessionStorage.getItem("PortfolioList"));
    if (PLPortListData != null) {
        if (PLPortListData.length > 0) {
            $("#PortfolioTbl >tbody >tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohiddenfiled").val();
                for (var i = 0; i < PLPortListData.length; i++) {
                    $("#PortfolioName_" + rowid).append(`<option value="${PLPortListData[i].setup_id}">${PLPortListData[i].setup_val}</option>`);
                }
                var firstEmptySelect = true;
                $("#PortfolioName_" + rowid).select2({
                    templateResult: function (data) {
                        //var UOM = $(data.element).data('uom');
                        var classAttr = $(data.element).attr('class');
                        var hasClass = typeof classAttr != 'undefined';
                        classAttr = hasClass ? ' ' + classAttr : '';
                        var $result = $(
                            '<div class="row">' +
                            '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
                            '</div>'
                        );
                        return $result;
                        firstEmptySelect = false;
                    }
                });

            });
        }
    }

    $("#PortfolioTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var prfID = currentRow.find("#hfPrfID").val();
        var rowid = currentRow.find("#SNohiddenfiled").val();
        var CustID = '#PortfolioName_' + rowid;
        if (prfID != '0' && prfID != "") {
            currentRow.find("#PortfolioName_" + rowid).val(prfID).trigger('change.select2');
        }

    });

    $("#PortfolioTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var CusID = currentRow.find("#hfPrfID").val();
        var rowid = currentRow.find("#SNohiddenfiled").val();

        var PortfolioCode;
        PortfolioCode = CusID;

        if (PortfolioCode != '0' && PortfolioCode != "") {

            $("#PortfolioTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRowD = $(this);
                var PortfolioCodeD;
                PortfolioCodeD = currentRowD.find("#hfPrfID").val();
                var rowidD = currentRowD.find("#SNohiddenfiled").val();
                if (PortfolioCodeD != '0' && PortfolioCodeD != "") {
                    if (currentRow.find("#PortfolioName_" + rowidD).val() != PortfolioCode) {
                        $("#PortfolioName_" + rowid + " option[value=" + PortfolioCodeD + "]").select2().hide();
                    }
                }
            })
        }
    });
}
function CheckPortfolioValidation() {
    debugger;
    var rowCount = $('#PortfolioTbl >tbody >tr').length;
    if (rowCount > 0) {
        var ErrorFlag = "N";
        $("#PortfolioTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#PortfolioName_" + Sno).val() == "0") {
                currentRow.find("#PortfolioNameError").text($("#valueReq").text());
                currentRow.find("#PortfolioNameError").css("display", "block");
                debugger;

                currentRow.find("[aria-labelledby='select2-PortfolioName_" + Sno + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#PortfolioNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
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
//-------------------------------------------Portfolio Info END--------------------------//

//-------------------------------------------SuppInfo--------------------------//
function AddNewSuppRow() {
    var rowIdx = 0;
    // debugger;    
    var rowCount = $('#SupplierDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;

    $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
        // debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#SupplierDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"></td>
<td><div class="col-sm-11 lpo_form no-padding"><select class="form-control" onchange ="OnChangeSupplier(${RowNo},event)" id="SupplierName_${RowNo}"></select>
<input  type="hidden" id="hfSuppID" />
<span id="SuppNameError" class="error-message is-visible"></span></div></td>
<td><input id="ItemCode" class="form-control date" autocomplete="off" type="text" name="ItemCode" placeholder="${$("#span_ItemCode").text()}" maxlength="25"  onblur="this.placeholder='${$("#span_ItemCode").text()}'"></td>
<td><input id="ItemDescription" class="form-control date" autocomplete="off" type="text" name="ItemDescription" maxlength="500" placeholder="${$("#span_ItemDescription").text()}"  onblur="this.placeholder='${$("#span_ItemDescription").text()}'"></td>
<td><input id="PackingDetail" class="form-control date" autocomplete="off" type="text" name="PackingDetail" maxlength="50" placeholder="${$("#Span_PackingDetail_Title").text()}"  onblur="this.placeholder='${$("#Span_PackingDetail_Title").text()}'"></td>
<td><input id="BoxDetail" class="form-control date" value='' autocomplete="" maxlength="50" type="text" name="BoxDetail" placeholder="Box Detail"></td>
<td><input id="NetWeight" class="form-control num_right"  type="text" autocomplete="off" name="NetWeight" maxlength="50" placeholder="0000.00"  ></td>
<td><textarea id="SuppItemRemarks" class="form-control remarksmessage" name="ItemRemarks" maxlength="250" placeholder="${$("#span_remarks").text()}"></textarea></td>
</tr>`);
    BindSupplierList(RowNo);
};
function BindDLLSupplierList() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/BusinessLayer/ItemDetail/GetSuppListAuto",
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                // debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        sessionStorage.removeItem("SupplierList");
                        sessionStorage.setItem("SupplierList", JSON.stringify(arr.Table));

                        BindSuppData();
                       
                    }
                }
            },
        });
}
function BindSupplierList(ID) {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/BusinessLayer/ItemDetail/GetSuppListAuto",
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        //$('#SupplierName_' + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#SupplierName_' + ID).append(`<option value="${arr.Table[i].supp_id}">${arr.Table[i].supp_name}</option>`);
                        }
                        var firstEmptySelect = true;
                      
                        $('#SupplierName_' + ID).select2({
                            templateResult: function (data) {
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });
                        // debugger;
                        HideSupplierList(ID);
                    }
                }
            },
        });
}
function HideSupplierList(ID) {
    $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var ItemCode;

        ItemCode = currentRow.find("#SupplierName_" + Sno).val();

        if (ItemCode != '0') {
            if (currentRow.find("#SupplierName_" + Sno).val() != ItemCode) {
                $("#SupplierName_ option[value=" + ItemCode + "]").select2().hide();
            }
            if (Sno != "0") {
                $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
                    debugger;
                    var currentRowD = $(this);
                    var SnoD = currentRowD.find("#SNohiddenfiled").val();
                    var SupplierCodeD;

                    SupplierCodeD = currentRowD.find("#SupplierName_" + SnoD).val();

                    if (SupplierCodeD != '0') {
                        if (currentRow.find("#SupplierName_" + Sno).val() !== SupplierCodeD) {
                            $("#SupplierName_" + Sno + " option[value=" + SupplierCodeD + "]").select2().hide();
                        }
                    }
                })
            }
            $("#SupplierName_" + ID + " option[value=" + ItemCode + "]").select2().hide();
        }
        else {
            $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRowD = $(this);
                var SnoD = currentRowD.find("#SNohiddenfiled").val();
                var SupplierCodeD;

                SupplierCodeD = currentRowD.find("#SupplierName_" + SnoD).val();

                if (SupplierCodeD != '0') {
                    if (currentRow.find("#SupplierName_" + Sno).val() !== SupplierCodeD) {
                        $("#SupplierName_" + Sno + " option[value=" + SupplierCodeD + "]").select2().hide();
                    }
                }
            })
        }
    });
}
function OnChangeSupplier(RowID, e) {
    // debugger;
    BindSuppList(e);
}
function BindSuppList(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var SupplierID = clickedrow.find("#hfSuppID").val();
    var SuppID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    SuppID = clickedrow.find("#SupplierName_" + SNo).val();
    clickedrow.find("#hfSuppID").val(SuppID);

    if (SuppID == "0") {
        clickedrow.find("#SuppNameError").text($("#valueReq").text());
        clickedrow.find("#SuppNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    }
    else {
        clickedrow.find("#SuppNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    }

    ShowSupplierList(SupplierID);
    HideSupplierList(SNo);
}
function ShowSupplierList(SuppID) {
    debugger;
    if (SuppID != "0" && SuppID != "") {
        $("#SupplierDetailsTbl >tbody >tr").each(function () {
            // debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            $("#SupplierName_" + Sno + " option[value=" + SuppID + "]").removeClass("select2-hidden-accessible");
        });
    }
}
function BindSuppData() {
    debugger;
    var PLSuppListData = JSON.parse(sessionStorage.getItem("SupplierList"));
    if (PLSuppListData != null) {
        if (PLSuppListData.length > 0) {
            $("#SupplierDetailsTbl >tbody >tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohiddenfiled").val();

                //$("#SupplierName_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#SOItemName").text()}' data-uom='${$("#SOItemUOM").text()}'></optgroup>`);
                for (var i = 0; i < PLSuppListData.length; i++) {
                    $("#SupplierName_" + rowid).append(`<option value="${PLSuppListData[i].supp_id}">${PLSuppListData[i].supp_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#SupplierName_" + rowid).select2({
                    templateResult: function (data) {
                        //var UOM = $(data.element).data('uom');
                        var classAttr = $(data.element).attr('class');
                        var hasClass = typeof classAttr != 'undefined';
                        classAttr = hasClass ? ' ' + classAttr : '';
                        var $result = $(
                            '<div class="row">' +
                            '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
                            //'<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
                            '</div>'
                        );
                        return $result;
                        firstEmptySelect = false;
                    }
                });

            });
        }
    }

    $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var CusID = currentRow.find("#hfSuppID").val();
        var rowid = currentRow.find("#SNohiddenfiled").val();
        var SuppID = '#SupplierName_' + rowid;
        if (CusID != '0' && CusID != "") {
            currentRow.find("#SupplierName_" + rowid).val(CusID).trigger('change.select2');
        }

    });

    $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var CusID = currentRow.find("#hfSuppID").val();
        var rowid = currentRow.find("#SNohiddenfiled").val();

        var SupplierCode;
        SupplierCode = CusID;

        if (SupplierCode != '0' && SupplierCode != "") {

            $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRowD = $(this);
                var SupplierCodeD;
                SupplierCodeD = currentRowD.find("#hfSuppID").val();
                var rowidD = currentRowD.find("#SNohiddenfiled").val();
                if (SupplierCodeD != '0' && SupplierCodeD != "") {
                    if (currentRow.find("#SupplierName_" + rowidD).val() != SupplierCode) {
                        $("#SupplierName_" + rowid + " option[value=" + SupplierCodeD + "]").select2().hide();
                    }
                }
            })
        }
    });
}
function CheckSuppValidations() {
    debugger;
    var rowCount = $('#SupplierDetailsTbl >tbody >tr').length;
    if (rowCount > 0) {
        var ErrorFlag = "N";
        $("#SupplierDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#SupplierName_" + Sno).val() == "0") {
                currentRow.find("#SuppNameError").text($("#valueReq").text());
                currentRow.find("#SuppNameError").css("display", "block");
                debugger;

                currentRow.find("[aria-labelledby='select2-SupplierName_" + Sno + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#SuppNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
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
//-------------------------------------------SuppInfo End--------------------------//
//-------------------------------------------Vehicle Info Start--------------------------//
function BindDLLVehicleList() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/BusinessLayer/ItemDetail/GetVehicleListAuto",
            data: function (params) {
                var queryParameters = {
                    VehicleName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                // debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        sessionStorage.removeItem("VehicleList");
                        sessionStorage.setItem("VehicleList", JSON.stringify(arr.Table));

                        BindVehicleData();

                    }
                }
            },
        });
}
function BindVehicleList(ID) {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/BusinessLayer/ItemDetail/GetVehicleListAuto",
            data: function (params) {
                var queryParameters = {
                    CustName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#VehicleName_' + ID).append(`<option value="${arr.Table[i].setup_id}">${arr.Table[i].setup_val}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#VehicleName_' + ID).select2({
                            templateResult: function (data) {
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });
                        // debugger;
                        HideVehicleList(ID);
                    }
                }
            },
        });
}
function HideVehicleList(ID) {
    $("#VehicleTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var ItemCode;

        ItemCode = currentRow.find("#VehicleName_" + Sno).val();

        if (ItemCode != '0') {
            if (currentRow.find("#VehicleName_" + Sno).val() != ItemCode) {
                $("#VehicleName_ option[value=" + ItemCode + "]").select2().hide();
            }
            if (Sno != "0") {
                $("#VehicleTbl >tbody >tr").each(function (i, row) {
                    debugger;
                    var currentRowD = $(this);
                    var SnoD = currentRowD.find("#SNohiddenfiled").val();
                    var VehicleCodeD;

                    VehicleCodeD = currentRowD.find("#VehicleName_" + SnoD).val();

                    if (VehicleCodeD != '0') {
                        if (currentRow.find("#VehicleName_" + Sno).val() !== VehicleCodeD) {
                            $("#VehicleName_" + Sno + " option[value=" + VehicleCodeD + "]").select2().hide();
                        }
                    }
                })
            }
            $("#VehicleName_" + ID + " option[value=" + ItemCode + "]").select2().hide();
        }
        else {
            $("#VehicleTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRowD = $(this);
                var SnoD = currentRowD.find("#SNohiddenfiled").val();
                var VehicleCodeD;

                VehicleCodeD = currentRowD.find("#VehicleName_" + SnoD).val();

                if (VehicleCodeD != '0') {
                    if (currentRow.find("#VehicleName_" + Sno).val() !== VehicleCodeD) {
                        $("#VehicleName_" + Sno + " option[value=" + VehicleCodeD + "]").select2().hide();
                    }
                }
            })
        }
    });
}
function OnChangeVehicle(RowID, e) {
    // debugger;
    //BindPrfList(e);
    BindVehcList(e);
}
//function BindPrfList(e) {
//    debugger;
//    var QtyDecDigit = $("#QtyDigit").text();
//    var RateDecDigit = $("#RateDigit").text();
//    var clickedrow = $(e.target).closest("tr");
//    var VehicleID = clickedrow.find("#hfVehID").val();
//    var CustID;
//    var SNo = clickedrow.find("#SNohiddenfiled").val();
//    CustID = clickedrow.find("#VehicleName_" + SNo).val();
//    clickedrow.find("#hfVehID").val(CustID);

//    if (CustID == "0") {
//        clickedrow.find("#VehcNameError").text($("#valueReq").text());
//        clickedrow.find("#VehcNameError").css("display", "block");
//        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
//    }
//    else {
//        clickedrow.find("#VehcNameError").css("display", "none");
//        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
//    }

//    ShowVehicleList(VehicleID);
//    HideVehicleList(SNo);
//}

function BindVehcList(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var VehicleID = clickedrow.find("#hfVehID").val();
    var CustID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    CustID = clickedrow.find("#VehicleName_" + SNo).val();
    clickedrow.find("#hfVehID").val(CustID);

    if (CustID == "0") {
        clickedrow.find("#VehcNameError").text($("#valueReq").text());
        clickedrow.find("#VehcNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    }
    else {
        clickedrow.find("#VehcNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    }

    ShowVehicleList(VehicleID);
    HideVehicleList(SNo);
}
function ShowVehicleList(PrfID) {
    debugger;
    if (PrfID != "0" && PrfID != "") {
        $("#VehicleTbl >tbody >tr").each(function () {
            // debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            $("#VehicleName_" + Sno + " option[value=" + PrfID + "]").removeClass("select2-hidden-accessible");
        });
    }
}
function AddNewVehicleRow() {
    var rowIdx = 0;
    // debugger;    
    var rowCount = $('#VehicleTbl >tbody >tr').length + 1;
    var RowNo = 0;

    $("#VehicleTbl >tbody >tr").each(function (i, row) {
        // debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#VehicleTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td><div class="col-sm-11 lpo_form no-padding"><select class="form-control" onchange ="OnChangeVehicle(${RowNo},event)" id="VehicleName_${RowNo}"></select>
<input  type="hidden" id="hfVehID" />
<span id="VehcNameError" class="error-message is-visible"></span></div></td>
<td><input id="VehModelNumber" class="form-control"  autocomplete="off" type="text" name="ModelNumber" maxlength="50" placeholder="${$("#span_ModelNumber").text()}"></td>
<td><input id="VehOEMNo" class="form-control" autocomplete="off" type="text" name="OEMNo" maxlength="50" placeholder="${$("#span_OEMNo").text()}"></td>
<td><input id="VehPartNo" class="form-control" autocomplete="off" type="text" name="VehPartNo" maxlength="50" placeholder="${$("#span_PartNumber").text()}"></td>
<td><input id="VehItemDescription" class="form-control" autocomplete="off" type="text" name="ItemDescription" maxlength="50" placeholder="${$("#span_ItemDescription").text()}"  onblur="this.placeholder='${$("#span_ItemDescription").text()}'"></td>
<td><textarea id="VehItemRemarks" class="form-control remarksmessage" name="VehItemRemarks" maxlength="50" placeholder="${$("#span_remarks").text()}"></textarea></td>
tr>`);
    BindVehicleList(RowNo);
};
function BindVehicleData() {
    debugger;
    var PLVehListData = JSON.parse(sessionStorage.getItem("VehicleList"));
    if (PLVehListData != null) {
        if (PLVehListData.length > 0) {
            $("#VehicleTbl >tbody >tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohiddenfiled").val();
                for (var i = 0; i < PLVehListData.length; i++) {
                    $("#VehicleName_" + rowid).append(`<option value="${PLVehListData[i].setup_id}">${PLVehListData[i].setup_val}</option>`);
                }
                var firstEmptySelect = true;
                $("#VehicleName_" + rowid).select2({
                    templateResult: function (data) {
                        //var UOM = $(data.element).data('uom');
                        var classAttr = $(data.element).attr('class');
                        var hasClass = typeof classAttr != 'undefined';
                        classAttr = hasClass ? ' ' + classAttr : '';
                        var $result = $(
                            '<div class="row">' +
                            '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
                            '</div>'
                        );
                        return $result;
                        firstEmptySelect = false;
                    }
                });

            });
        }
    }

    $("#VehicleTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var prfID = currentRow.find("#hfVehID").val();
        var rowid = currentRow.find("#SNohiddenfiled").val();
        var CustID = '#VehicleName_' + rowid;
        if (prfID != '0' && prfID != "") {
            currentRow.find("#VehicleName_" + rowid).val(prfID).trigger('change.select2');
        }

    });

    $("#VehicleTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var CusID = currentRow.find("#hfVehID").val();
        var rowid = currentRow.find("#SNohiddenfiled").val();

        var VehicleCode;
        VehicleCode = CusID;

        if (VehicleCode != '0' && VehicleCode != "") {

            $("#VehicleTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRowD = $(this);
                var VehicleCodeD;
                VehicleCodeD = currentRowD.find("#hfVehID").val();
                var rowidD = currentRowD.find("#SNohiddenfiled").val();
                if (VehicleCodeD != '0' && VehicleCodeD != "") {
                    if (currentRow.find("#VehicleName_" + rowidD).val() != VehicleCode) {
                        $("#VehicleName_" + rowid + " option[value=" + VehicleCodeD + "]").select2().hide();
                    }
                }
            })
        }
    });
}
function CheckVehicleValidation() {
    debugger;
    var rowCount = $('#VehicleTbl >tbody >tr').length;
    if (rowCount > 0) {
        var ErrorFlag = "N";
        $("#VehicleTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#VehicleName_" + Sno).val() == "0") {
                currentRow.find("#VehcNameError").text($("#valueReq").text());
                currentRow.find("#VehcNameError").css("display", "block");
                debugger;

                currentRow.find("[aria-labelledby='select2-VehicleName_" + Sno + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#VehcNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
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
//-------------------------------------------Vehicle Info END--------------------------//
function CalculateCBM() {
    var ValDecDigit = $("#RateDigit").text();   
    debugger;
    var Uom = $("#pack_uom").val();
    var Length = $("#Length").val();
    var Width = $("#Width").val();
    var Height = $("#Height").val();   
    debugger;
    if (Uom == 101) {
        var CBM = Length * Width * Height / 1000000000;
        var FinVal = parseFloat(CBM).toFixed(ValDecDigit);
        $("#CBM").val(FinVal);
    }
    if (Uom == 102) {
        var CBM = Length * Width * Height/1000000;
        var FinVal = parseFloat(CBM).toFixed(ValDecDigit);
        $("#CBM").val(FinVal);
    }  
    if (Uom == 103) {
        var CBM = Length * Width * Height;
        var FinVal = parseFloat(CBM).toFixed(ValDecDigit);
        $("#CBM").val(FinVal);
    }
    $("#spanpackuom").text("");
    $("#pack_uom").css("border-color", "#ced4da");
    $("#spanlength").text("");
    $("#Length").css("border-color", "#ced4da");
    $("#spanwidth").text("");
    $("#Width").css("border-color", "#ced4da");
    $("#spanheight").text("");
    $("#Height").css("border-color", "#ced4da");
     
}

function RlvntAccountLinkingMendatory(Action,ToggleName) {
    var Flag = "N";
    debugger;
    if (ToggleName == "i_sls" || ToggleName=="") {
        if ($("#i_sls").is(":checked")) {
            $("#Req_Lcl_Sale_Acc").removeClass("d-none");
            $("#Req_Exp_Sale_Acc").removeClass("d-none");
            $("#Req_Discount_Acc").removeClass("d-none");
            /*$("#Req_cost_of_goods_sold_Acc").removeClass("d-none");*/
            $("#Req_sale_return_Acc").removeClass("d-none");
            if (Action == "Save") {

                Flag = ApplyCheckVallidationJs("Local_Sale_Account", Flag);
                Flag = ApplyCheckVallidationJs("export_sale_account", Flag);
                Flag = ApplyCheckVallidationJs("discount_account", Flag);
              /*  Flag = ApplyCheckVallidationJs("cost_of_goods_sold_account", Flag);*/
                Flag = ApplyCheckVallidationJs("sale_return_account", Flag);

            }
        }
        else
        {

           // $("#i_capg").prop("checked", false);
            $("#Req_Lcl_Sale_Acc").addClass("d-none");
            $("#Req_Exp_Sale_Acc").addClass("d-none");
            if ($("#i_pur").is(":checked") == false) {
                $("#Req_Discount_Acc").addClass("d-none");
            }
            $("#Req_cost_of_goods_sold_Acc").addClass("d-none");
            $("#Req_sale_return_Acc").addClass("d-none");
            RemoveCheckVallidationJs("Local_Sale_Account,export_sale_account,discount_account,sale_return_account");
       
        }
    }
    if (ToggleName == "i_pur" || ToggleName == "") {
        if ($("#i_pur").is(":checked")) {
            $("#Req_Lcl_Pur_Acc").removeClass("d-none");
            $("#Req_Import_Pur_Acc").removeClass("d-none");
            $("#Req_Discount_Acc").removeClass("d-none");
           /* $("#Req_ProvisinalPayble_Acc").removeClass("d-none");*/
            $("#Req_Pur_Return_Acc").removeClass("d-none");
            if (Action == "Save") {
                Flag = ApplyCheckVallidationJs("local_purchase_account", Flag);
                Flag = ApplyCheckVallidationJs("import_purchase_account", Flag);
                Flag = ApplyCheckVallidationJs("discount_account", Flag);
            /*    Flag = ApplyCheckVallidationJs("provisional_payable_account", Flag);*/
                Flag = ApplyCheckVallidationJs("purchase_return_account", Flag);

            }
        }
        else {
           // $("#i_capg").prop("checked", false);
            $("#Req_Lcl_Pur_Acc").addClass("d-none");
            $("#Req_Import_Pur_Acc").addClass("d-none");
            if ($("#i_sls").is(":checked") == false) {
                $("#Req_Discount_Acc").addClass("d-none");
                RemoveCheckVallidationJs("discount_account");
            }
          /*  $("#Req_ProvisinalPayble_Acc").addClass("d-none");*/
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
        $("#" + FieldID).attr("onchange","");
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
function OnclickApproveBtn() {
    debugger;
    if ($("#hdnSavebtn").val() == "AllreadyclickApprovebtn") {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#btn_approve").attr("disabled", true);
    }
    else {
        $("#hdnSavebtn").val("AllreadyclickApprovebtn");
        $("#btn_approve").css("filter", "grayscale(100%)");
    }   
}