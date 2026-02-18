$(document).ready(function () {
    debugger
    
    //$("#ddl_Sales_by").val("I").change();
    OnchangeSalesBy()
    /*-----------Start code add by Hina Sharma on 27-06-2025----------*/
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    var Cust_Typ = "";
    if (ddl_Sales_by == "B") {
        Cust_Typ = "B";
        BindCustomerList(Cust_Typ);
    }
    else {
        BindCustomerList(Cust_Typ);
    }
    /*-----------End code add by Hina Sharma on 27-06-2025----------*/
    BindSalesPersonList();
    BindProductGrpName();
    BindProductPortfolio();
    BindHSNCode();
    BindCityList();
    BindStateList();
    //BindCustomerZone();
    /*cmn_apply_datatable("#SummaryWiseData");*/
    Cmn_initializeMultiselect([
        '#ddl_RegionName',
        '#ddl_SaleType',
        '#CustomerCategory',
        '#CustomerPortfolio',
        '#ddlcustzone',
        '#ddlcustgroup',
        '#ddl_branch',
    ]);
});
//function BindHSNCode() {
//    $("#HSN_cd").select2({
//        ajax: {
//            url: $("#ajaxUrlGetAutoCompleteSearchHSN").val(),
//            data: function (params) {
//                var queryParameters = {
//                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
//                    ddlhsncode: params.term, // search term like "a" then "an"
//                    Group: params.page
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
function BindHSNCode() {
    $.ajax({
        url: $("#ajaxUrlGetAutoCompleteSearchHSN").val(),
        dataType: "json",
        success: function (data) {
            var $ddl = $("#HSN_cd");
            $ddl.empty();
            $.each(data, function (i, val) {
                if (val.ID !== "0") { // Skip option with value '0'
                    $ddl.append($('<option>', {
                        value: val.ID,
                        text: val.Name
                    }));
                }
            });
            Cmn_initializeMultiselect(['#HSN_cd']);
        }
    });
}
function onchangeShowAs() {
    var salesBy = $("#ddl_Sales_by").val();
    if (salesBy == "I") {
        BindInvoiceWiseDetails()
    }
    if (salesBy == "C") {
        BindCostomerWiseDetails();
    }
    if (salesBy == "P") {
        BindProductWiseDetails();
    }
    if (salesBy == "G") {
        BindProductGroupWiseDetails();
    }
    if (salesBy == "R") {
        BindRegionWiseDetails();
    }
    if (salesBy == "S") {
        BindSalesExecutiveWiseDetails();
    }
    if (salesBy == "B") {/* add by Hina Sharma on 27-06-2025*/
        BindInterBranchWiseDetails();
    }
}
function CalculateAmtOnSearchInput(TableId, colspan) {
    debugger;
    var ValDigit = $("#ValDigit").text();
    var SaleAmtSpec = 0;
    var TaxAmtSpec = 0;
    var OcAmtSpec = 0;
    var SaleAmt = 0;
    var TaxAmt = 0;
    var OcAmt = 0;
    var InvAmtBs = 0;
    var InvAmtSpcs = 0;
    if ($("#" + TableId + " > tbody > tr > td").length > 1) {
    $("#" + TableId+"_next").prev().trigger("click");
    for (var i = 0; i < $("#" + TableId+"_next").prev().text(); i++) {
            $("#" + TableId + " > tbody > tr").each(function () {
                var currentRow = $(this);
                SaleAmtSpec = parseFloat(SaleAmtSpec) + parseFloat(currentRow.find("#txt_SaleAmtSpec").text().trim());
                TaxAmtSpec = parseFloat(TaxAmtSpec) + parseFloat(currentRow.find("#txt_TaxAmtSpec").text().trim());
                OcAmtSpec = parseFloat(OcAmtSpec) + parseFloat(currentRow.find("#txt_OcAmtSpec").text().trim());
                SaleAmt = parseFloat(SaleAmt) + parseFloat(currentRow.find("#txt_SaleAmt").text().trim());
                TaxAmt = parseFloat(TaxAmt) + parseFloat(currentRow.find("#txt_TaxAmt").text().trim());
                OcAmt = parseFloat(OcAmt) + parseFloat(currentRow.find("#txt_OcAmt").text().trim());
                InvAmtBs = parseFloat(InvAmtBs) + parseFloat(currentRow.find("#txt_InvoiceAmt").text().trim());
                InvAmtSpcs = parseFloat(InvAmtSpcs) + parseFloat(currentRow.find("#txt_InvoiceAmtSpec").text().trim());

            });
        $("#" + TableId+"_previous > a").trigger("click");
        }
    }
   
    var td = "";
    if (TableId == "datatable-buttons")
        td = "<td colspan='1' align='right'></td>";
    $("#" + TableId +" > tfoot").html(`
<tr class="total">
                <td colspan="${colspan}" align="right"><strong>Total</strong></td>
                <td class="num_right"><strong>${parseFloat(SaleAmtSpec).toFixed(ValDigit)}</strong></td>
                <td class="num_right"><strong>${parseFloat(SaleAmt).toFixed(ValDigit)}</strong></td>
                <td class="num_right"><strong>${parseFloat(TaxAmtSpec).toFixed(ValDigit)}</strong></td>
                <td class="num_right"><strong>${parseFloat(TaxAmt).toFixed(ValDigit)}</strong></td>
                <td class="num_right"><strong>${parseFloat(OcAmtSpec).toFixed(ValDigit)}</strong></td>
                <td class="num_right"><strong>${parseFloat(OcAmt).toFixed(ValDigit)}</strong></td>
                <td class="num_right"><div class="col-sm-11"><strong>${parseFloat(InvAmtSpcs).toFixed(ValDigit)}</strong></div></td>
                <td class="num_right"><div class="col-sm-11"><strong>${parseFloat(InvAmtBs).toFixed(ValDigit)}</strong></div></td>
                
${td}
            </tr>
`)
}
function BindSalesPersonList() {
    debugger;
    $.ajax({
        type: "POST",
        url: $("#salespersonList").val(),
        data: function (params) {
            var queryParameters = {
                SO_SalePerson: params.term,
                BrchID: Branch
            };
            return queryParameters;
        },
        success: function (data, params) {
            var s = "";
            $.map(data, function (val, item) {
                if (val.Name.trim() != "---Select---") {
                    s += '<option value=' + val.ID + '>' + val.Name + '</option>';
                }
            })
            $("#ddl_SalesRepresentative").html(s);
            Cmn_initializeMultiselect([
                '#ddl_SalesRepresentative',
            ]);
           // $("#ddl_SalesRepresentative").select2({});
        }
    });

}
function BindCustomerList() {
     debugger;
    var Branch = sessionStorage.getItem("BranchID");
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    var Cust_Typ = "";
    if (ddl_Sales_by == "B") {
        Cust_Typ = "B";
    }
    
    $.ajax({
        url: "/ApplicationLayer/SalesDetail/GetAutoCompleteSearchCustList",
        data: {
            Cust_Typ: Cust_Typ
        },
        success: function (data, params) {
            debugger;
            if (data == 'ErrorPage') {
                LSO_ErrorPage();
                return false;
            }
            var s = "";
            $.map(data, function (val, item) {
                if (val.Name.trim() != "---Select---") {
                    s += '<option value=' + val.ID + '>' + val.Name + '</option>';
                } 
            })
           $("#ddl_CustomerName").html(s);
            Cmn_initializeMultiselect(['#ddl_CustomerName']);
            $('#ddl_CustomerName').multiselect('rebuild');
        }
    });
   
}
//function BindProductGrpName() {
//    debugger
//    $("#ddl_ProductGrpName").select2({
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
//                //params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        if (val.ID == "0") {
//                            return { id: val.ID, text: "---All---" };
//                        } else {
//                            return { id: val.ID, text: val.Name };
//                        }
//                    })
//                };
//            }
//        },
//    });
//}
function BindProductGrpName() {
    debugger
    $.ajax({
        url: $("#ItemGrpName").val(),
        dataType: "json",
        success: function (data) {
            var $ddl = $("#ddl_ProductGrpName");
            $ddl.empty();
            $.each(data, function (i, val) {
                if (val.ID !== "0") { // Skip option with value '0'
                    $ddl.append($('<option>', {
                        value: val.ID,
                        text: val.Name
                    }));
                }
            });
            Cmn_initializeMultiselect(['#ddl_ProductGrpName']);
        }
    });
}
function BindProductPortfolio() {
    $.ajax({
        url: $("#ItemPortfName").val(),
        dataType: "json",
        success: function (data) {
            var $ddl = $("#ProductPortfolio");
            $ddl.empty();
            $.each(data, function (i, val) {
                if (val.ID !== "0") { // Skip option with value '0'
                    $ddl.append($('<option>', {
                        value: val.ID,
                        text: val.Name
                    }));
                }
            });
            Cmn_initializeMultiselect(['#ProductPortfolio']);
        }
    });
}
//function BindCustomerZone() {
//    $.ajax({
//        url: $("#ItemPortfName").val(),
//        dataType: "json",
//        success: function (data) {
//            var $ddl = $("#ddlcustzone");
//            $ddl.empty();
//            $.each(data, function (i, val) {
//                if (val.ID !== "0") { // Skip option with value '0'
//                    $ddl.append($('<option>', {
//                        value: val.ID,
//                        text: val.Name
//                    }));
//                }
//            });
//            Cmn_initializeMultiselect(['#ddlcustzone']);
//        }
//    });
//}
//function BindProductPortfolio() {
//    $("#ProductPortfolio").multiselect({
//        includeSelectAllOption: true,
//        enableFiltering: true,
//        buttonWidth: '100%',
//        buttonClass: 'btn btn-default btn-custom',
//        nonSelectedText: '---All---',
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
//                //params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        if (val.ID == "0") {
//                            return { id: val.ID, text: "---All---" };
//                        } else {
//                            return { id: val.ID, text: val.Name };
//                        }
                        
//                    })
//                };
//            }
//        },
//    });
//}
function OnchangeSalesBy() {
    debugger;
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    if (ddl_Sales_by == "I" || ddl_Sales_by == "C") {
        $("#div_clear").addClass("clear");
    } else {
        $("#div_clear").removeClass("clear");
    }
    if (ddl_Sales_by == "I") {
        $("#ddl_CustomerName").empty();
        $('#ddl_CustomerName').append(`<option value="0">---All---</option>`);
        BindCustomerList("");
        $("#AllPartials > div, #div_productGrpName, #div_ProductPortfolio, #div_HSNCode").css("display", "none");
        $("#Tbl_salesDetailInvoiceWiseList, #div_ddl_CustomerName, #div_ddl_RegionName, #div_ddl_SaleType, #div_ddl_SalesRepresentative, #div_CustomerCategory, #div_CustomerPortfolio,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "");
        BindInvoiceWiseDetails();
       
    }
    else if (ddl_Sales_by == "C") {
        $("#ddl_CustomerName").empty();
        $('#ddl_CustomerName').append(`<option value="0">---All---</option>`);
        BindCustomerList("");
        $("#div_productGrpName, #div_ProductPortfolio, #div_ddl_SalesRepresentative,#div_HSNCode").css("display", "none");
        $("#AllPartials > div").css("display", "none");
        $("#PartialMIS_SalesDetailsCustomerWiseList, #div_ddl_CustomerName, #div_ddl_RegionName,#div_CustomerCategory, #div_CustomerPortfolio, #div_ddl_SaleType,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "");
        BindCostomerWiseDetails();
    }
    else if (ddl_Sales_by == "P") {
        
        $("#AllPartials > div , #div_ddl_CustomerName,#div_ddl_SalesRepresentative, #div_ddl_RegionName, #div_CustomerCategory, #div_CustomerPortfolio, #div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "none");
        $("#PartialMIS_SalesDetailsProductWiseList, #div_ddl_SaleType, #div_productGrpName, #div_ProductPortfolio,#div_HSNCode").css("display", "");
        BindProductWiseDetails();
    }
    else if (ddl_Sales_by == "G") {
        
        $("#AllPartials > div, #div_ddl_CustomerName,#div_ddl_SalesRepresentative, #div_ddl_RegionName, #div_CustomerCategory, #div_CustomerPortfolio, #div_HSNCode,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "none");
        $("#PartialMIS_SalesDetailsProductGroupWiseList, #div_ddl_SaleType, #div_productGrpName, #div_ProductPortfolio").css("display", "");
        BindProductGroupWiseDetails();
    }
    else if (ddl_Sales_by == "R") {
        
        $("#div_productName, #div_ProductPortfolio, #div_CustomerCategory, #div_CustomerPortfolio, #div_ddl_SaleType, #div_ddl_SalesRepresentative, #div_HSNCode,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "none");
        $("#AllPartials > div, #div_ddl_CustomerName, #div_productGrpName").css("display", "none");
        $("#PartialMIS_SalesDetailsRegionWiseList, #div_ddl_SaleType, #div_ddl_RegionName").css("display", "");
        BindRegionWiseDetails();
    }
    else if (ddl_Sales_by == "S") {
        
        $("#div_ddl_CustomerName, #div_ddl_RegionName, #div_CustomerCategory, #div_CustomerPortfolio, #div_ddl_SalesRepresentative,#div_HSNCode, #div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "none");
        $("#AllPartials > div, #div_productName, #div_ProductPortfolio").css("display", "none");
        $("#PartialMIS_SalesDetailsSalesExecutiveWiseList, #div_ddl_SalesRepresentative, #div_ddl_SaleType").css("display", "");
        BindSalesExecutiveWiseDetails();
    }
    else if (ddl_Sales_by == "B") {
        $("#ddl_CustomerName").empty();
        $('#ddl_CustomerName').append(`<option value="0">---All---</option>`);
        BindCustomerList("B");
        $("#AllPartials > div,#div_ddl_RegionName, #div_ddl_SaleType, #div_ddl_SalesRepresentative, #div_productGrpName, #div_ProductPortfolio, #div_CustomerCategory, #div_CustomerPortfolio,#div_HSNCode, #div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "none");
        $("#PartialMIS_SalesDetailInterBranchWiseList, #div_ddl_CustomerName ").css("display", "");

        BindInterBranchWiseDetails();

    }
    else {
        $("#div_productName").css("display", "");
        $("#div_ProductPortfolio").css("display", "");
        $("#div_CustomerCategory").css("display", "");
        $("#div_CustomerPortfolio").css("display", "");
    }
}
function OnClickIconBtn(e) {
    var ItmCode = $(e.target).closest('tr').find("#Hdn_item_id").val();
   
    ItemInfoBtnClick(ItmCode);
    
}
function replaceDatatablesIds(convertTo) {
    if (convertTo == "dttbl0") {
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons2")[0].id = "dttbl2";
        $("#datatable-buttons3")[0].id = "dttbl3";
        $("#datatable-buttons4")[0].id = "dttbl4";
        $("#datatable-buttons5")[0].id = "dttbl5";
    }
    if (convertTo == "datatable-buttons0") {
        $("#dttbl1")[0].id = "datatable-buttons1";
        $("#dttbl2")[0].id = "datatable-buttons2";
        $("#dttbl3")[0].id = "datatable-buttons3";
        $("#dttbl4")[0].id = "datatable-buttons4";
        $("#dttbl5")[0].id = "datatable-buttons5";
    }
    if (convertTo == "dttbl1") {
        $("#datatable-buttons")[0].id = "dttbl";
        $("#datatable-buttons2")[0].id = "dttbl2";
        $("#datatable-buttons3")[0].id = "dttbl3";
        $("#datatable-buttons4")[0].id = "dttbl4";
        $("#datatable-buttons5")[0].id = "dttbl5";
    }
    if (convertTo == "datatable-buttons1") {
        $("#dttbl")[0].id = "datatable-buttons";
        $("#dttbl2")[0].id = "datatable-buttons2";
        $("#dttbl3")[0].id = "datatable-buttons3";
        $("#dttbl4")[0].id = "datatable-buttons4";
        $("#dttbl5")[0].id = "datatable-buttons5";
    }
    if (convertTo == "dttbl2") {
        $("#datatable-buttons")[0].id = "dttbl";
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons3")[0].id = "dttbl3";
        $("#datatable-buttons4")[0].id = "dttbl4";
        $("#datatable-buttons5")[0].id = "dttbl5";
    }
    if (convertTo == "datatable-buttons2") {
        $("#dttbl")[0].id = "datatable-buttons";
        $("#dttbl1")[0].id = "datatable-buttons1";
        $("#dttbl3")[0].id = "datatable-buttons3";
        $("#dttbl4")[0].id = "datatable-buttons4";
        $("#dttbl5")[0].id = "datatable-buttons5";
    }
    if (convertTo == "dttbl3") {
        $("#datatable-buttons")[0].id = "dttbl";
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons2")[0].id = "dttbl2";
        $("#datatable-buttons4")[0].id = "dttbl4";
        $("#datatable-buttons5")[0].id = "dttbl5";
    }
    if (convertTo == "datatable-buttons3") {
        $("#dttbl")[0].id = "datatable-buttons";
        $("#dttbl1")[0].id = "datatable-buttons1";
        $("#dttbl2")[0].id = "datatable-buttons2";
        $("#dttbl4")[0].id = "datatable-buttons4";
        $("#dttbl5")[0].id = "datatable-buttons5";
    }
    if (convertTo == "dttbl4") {
        $("#datatable-buttons")[0].id = "dttbl";
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons2")[0].id = "dttbl2";
        $("#datatable-buttons3")[0].id = "dttbl3";
        $("#datatable-buttons5")[0].id = "dttbl5";
    }
    if (convertTo == "datatable-buttons4") {
        $("#dttbl")[0].id = "datatable-buttons";
        $("#dttbl1")[0].id = "datatable-buttons1";
        $("#dttbl2")[0].id = "datatable-buttons2";
        $("#dttbl3")[0].id = "datatable-buttons3";
        $("#dttbl5")[0].id = "datatable-buttons5";
    }
    if (convertTo == "dttbl5") {
        $("#datatable-buttons")[0].id = "dttbl";
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons2")[0].id = "dttbl2";
        $("#datatable-buttons3")[0].id = "dttbl3";
        $("#datatable-buttons4")[0].id = "dttbl4";
    }
    if (convertTo == "datatable-buttons5") {
        $("#dttbl")[0].id = "datatable-buttons";
        $("#dttbl1")[0].id = "datatable-buttons1";
        $("#dttbl2")[0].id = "datatable-buttons2";
        $("#dttbl3")[0].id = "datatable-buttons3";
        $("#dttbl4")[0].id = "datatable-buttons4";
    }
}
function BtnSearch() {
    debugger;
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    if (ddl_Sales_by == "I") {
        BindInvoiceWiseDetails();
    }
    else if (ddl_Sales_by == "C") {
        BindCostomerWiseDetails();
    }
    else if (ddl_Sales_by == "P") {
        BindProductWiseDetails();
    }
    else if (ddl_Sales_by == "G") {
        BindProductGroupWiseDetails();
    }
    else if (ddl_Sales_by == "R") {
        BindRegionWiseDetails();
    }
    else if (ddl_Sales_by == "S") {
        BindSalesExecutiveWiseDetails();
    }
    else if (ddl_Sales_by == "B") {
        BindInterBranchWiseDetails();
    }
   

}
/*---------------------------------- Invoice Wise Sales Details ------------------------------------------ */
function BindInvoiceWiseDetails() {
    //var cust_id = $("#ddl_CustomerName").val();
    //var reg_name = $("#ddl_RegionName").val();
    //var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    //var sale_per = $("#ddl_SalesRepresentative").val();
    var cust_id = cmn_getddldataasstring("#ddl_CustomerName");
    var reg_name = cmn_getddldataasstring("#ddl_RegionName");
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    var sale_per = cmn_getddldataasstring("#ddl_SalesRepresentative");
    var cust_zone = cmn_getddldataasstring("#ddlcustzone");
    var cust_group = cmn_getddldataasstring("#ddlcustgroup");
    var custCat = cmn_getddldataasstring("#CustomerCategory");
    var custPort = cmn_getddldataasstring("#CustomerPortfolio");
    var state = $("#ddl_state").val();
    var city = $("#ddl_city").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    //replaceDatatablesIds("dttbl0");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSales_DetailsByFilter",
        data: {
            cust_id: cust_id, reg_name: reg_name, sale_type: sale_type, product_id: product_id, sale_per: sale_per,
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs, cust_zone: cust_zone, cust_group: cust_group, custCat: custCat,
            custPort: custPort, state: state, city: city, brlist: brid_list,
        },
        success: function (data) {
            debugger;
            $("#Tbl_salesDetailInvoiceWiseList").html(data);
            if (ShowAs == "S") {
                cmn_apply_datatable("#InvoiceWiseSummery");
                //$("#InvoiceWiseSummery_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#InvoiceWiseSummery_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else {
                cmn_apply_datatable("#InvoiceWiseDetail");
                //$("#InvoiceWiseDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#InvoiceWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            $("#DataShowCsvWise").val("InoviceWise");
            //replaceDatatablesIds("datatable-buttons0");
        }
    })
}

function OnclickInvoiceAmount(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var inv_no = currentrow.find("#txtInv_no").text();
    var inv_dt = currentrow.find("#txtInv_dt").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    var brid_list = currentrow.find("#txtbr_id").text();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    var Flag = "ItemDetails";/*add by Hina Sharma on 27-06-2025*/
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSales_Item_Details",
        data: {
            inv_no: inv_no, inv_dt: inv_dt, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt, Flag: Flag,brlist: brid_list
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_InvoiceDetail");
            $("#InvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#tblInvDetail");
            $("#Inv_no_Csv").val(inv_no)
            $("#inv_dt_Csv").val(inv_dt)
            $("#curr_id_Csv").val(curr_id)
            $("#DataShowCsvWise").val("ItemDetails");
            //$("#tblInvDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            ////$("#tblInvDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailInoviceInsightBtnWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            //$("#tblInvDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailInoviceWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        }
    });
}
/*---------------------------------- Invoice Wise Sales Details End------------------------------------------ */
/*---------------------------------- Inter Branch Wise Sales Details Start add by Hina Sharma on 27-06-2025 ------------------------------------------ */
function BindInterBranchWiseDetails() {
    debugger;
    //var cust_id = $("#ddl_CustomerName").val();
    var cust_id = cmn_getddldataasstring("#ddl_CustomerName");
    //var reg_name = $("#ddl_RegionName").val();
    //var sale_type = $("#ddl_SaleType").val();
    //var product_id = $("#ddl_ProductName").val();
    //var sale_per = $("#ddl_SalesRepresentative").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    //replaceDatatablesIds("dttbl0");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSales_DetailsInterBrchWiseByFilter",
        data: {
            cust_id: cust_id,/* reg_name: reg_name, sale_type: sale_type, product_id: product_id, sale_per: sale_per,*/
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs, brlist: brid_list,
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesDetailInterBranchWiseList").html(data);
            //$("#DataShowCsvWise").val("");
            //$("#DataShowCsvWise").val("InterBrchWise");
            if (ShowAs == "S") {
                cmn_apply_datatable("#InterBrchWiseSummary");
                //$("#InterBrchWiseSummary_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#InterBrchWiseSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSalesDetailsInterBrchCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else {
                cmn_apply_datatable("#InterBrchWiseDetail");
                //$("#InterBrchWiseDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#InterBrchWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSalesDetailsInterBrchCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            $("#DataShowCsvWise").val("InterBrchWise");
            //replaceDatatablesIds("datatable-buttons0");
        }
    })
}

function OnclickInterBranchInvoiceAmount(e) {/*add by Hina Sharma on 27-06-2025*/
    debugger;
    var currentrow = $(e.target).closest("tr");
    var inv_no = currentrow.find("#txtInv_no").text();
    var inv_dt = currentrow.find("#txtInv_dt").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    var Flag = "ItemDetailsInterBrch";
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSales_Item_Details",
        data: {
            inv_no: inv_no, inv_dt: inv_dt, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt, Flag: Flag, brlist: brid_list
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_InvoiceDetail");
            $("#InvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#tblInvDetail");
            $("#Inv_no_Csv").val(inv_no)
            $("#inv_dt_Csv").val(inv_dt)
            $("#curr_id_Csv").val(curr_id)
            $("#DataShowCsvWise").val("ItemDetailsInterBrch");
            //$("#tblInvDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            ////$("#tblInvDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailInoviceInsightBtnWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            //$("#tblInvDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailInoviceWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        }
    });
}
/*---------------------------------- Invoice Wise Sales Details End------------------------------------------ */
/*---------------------------------- Customer Wise Sales Details ------------------------------------------ */
function BindCostomerWiseDetails() {
    debugger;
    //var cust_id = $("#ddl_CustomerName").val();
    //var reg_name = $("#ddl_RegionName").val();
    //var sale_type = $("#ddl_SaleType").val();
    //var custCat = $("#CustomerCategory").val();
    //var custPort = $("#CustomerPortfolio").val();
   
    var cust_id = cmn_getddldataasstring("#ddl_CustomerName");
    var reg_name = cmn_getddldataasstring("#ddl_RegionName");
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    var custCat = cmn_getddldataasstring("#CustomerCategory");
    var custPort = cmn_getddldataasstring("#CustomerPortfolio");
    var cust_zone = cmn_getddldataasstring("#ddlcustzone");
    var cust_group = cmn_getddldataasstring("#ddlcustgroup");
    var state = $("#ddl_state").val();
    var city = $("#ddl_city").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    //replaceDatatablesIds("dttbl1");
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSales_DetailsCostomerWiseByFilter",
        data: {
            cust_id: cust_id, reg_name: reg_name, custCat: custCat, custPort: custPort, sale_type: sale_type,curr_id:"",
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs, cust_zone: cust_zone, cust_group: cust_group, state: state, city: city, brlist: brid_list
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesDetailsCustomerWiseList").html(data);
            if (ShowAs == "S") {
                cmn_apply_datatable("#CustomerWiseSummery");
                //$("#CustomerWiseSummery_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                ////$("#CustomerWiseSummery_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailCustWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //$("#CustomerWiseSummery_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsCostomerWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else {
                cmn_apply_datatable("#CustomerWiseDetail");
               // $("#CustomerWiseDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
               //// $("#CustomerWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailCustWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
               // $("#CustomerWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsCostomerWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            $("#DataShowCsvWise").val("CustomerWise");
            //replaceDatatablesIds("datatable-buttons1");
        }
    })
}
function CW_OnclickInvoiceAmt(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var Cust_Id = currentrow.find("#txtCustName").text();
    var Cust_Name = currentrow.find("#cust_name").text();
    var Curr_symbal = currentrow.find("#curr_symbol").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetCW_Sales_Invoice_Details",
        data: {
            Cust_Id: Cust_Id, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt, brlist: brid_list/*--, sale_type: Sale_Type*/
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_CustomerInvoiceDetail");
            $("#CustomerInvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_CustomerInvoiceDetail");
            //$("#tbl_CustomerInvoiceDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            ////$("#tbl_CustomerInvoiceDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailInoviceInsightBtnWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            //$("#tbl_CustomerInvoiceDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetCW_Sales_Invoice_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#Currency_id").val(curr_id);
            $("#CustomerName").val(Cust_Name);
            $("#hdnCustId").val(Cust_Id)
            $("#Currency").val(Curr_symbal);
            $("#FromDate").val(from_dt);
            $("#ToDate").val(to_dt);
            //----------for Csv Print------------//
            $("#Cust_Id_Csv").val(Cust_Id)
            $("#curr_id_Csv").val(curr_id)
            $("#DataShowCsvWise").val("CW_Inv_Details");
            //----------for Csv Print------------//
        }
    })
}
function CW_OnclickInvoiceAmtGetItem(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var Cust_Id = currentrow.find("#txtCustName").text();
    var Cust_Name = currentrow.find("#cust_name").text();
    var Curr_symbal = currentrow.find("#curr_symbol").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    //var currentrow = $(e.target).closest("tr");
    //var Cust_Id = $("#hdnCustId").val();
    //var Cust_Name = $("#CustomerName").val();
    //var Curr_symbal = $("#Currency").val();
    //var curr_id = $("#Currency_id").val();
    var inv_no = currentrow.find("#app_inv_no").text();
    var inv_dt = currentrow.find("#txtInv_dt").text();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    var brid_list = currentrow.find("#txtbr_id").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetCW_Sales_Invoice_Item_Details",
        data: {
            Cust_Id: Cust_Id, inv_no: inv_no, inv_dt: inv_dt, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt,brlist: brid_list
        },
        success: function (data) {
            debugger;
            
            $("#CustomerInvoiceProductDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_CustomerInvoiceProductDetail");
            //$("#tbl_CustomerInvoiceProductDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_CustomerInvoiceProductDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetCW_Sales_Invoice_Item_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#itmCustomerName").val(Cust_Name);
            $("#itmInvoiceNumber").val(inv_no);
            $("#itmInvoiceDate").val(inv_dt);
            $("#itmCurrency").val(Curr_symbal);
            $("#itmCurrency_id").val(curr_id);
            $("#itmFromDate").val(from_dt);
            $("#itmToDate").val(to_dt);
            //----------for Csv Print------------//
            $("#Cust_Id_Csv").val(Cust_Id);
            $("#itmInvoiceNumber_Csv").val(inv_no);
            $("#itmInvoiceDate_Csv").val(inv_dt);
            $("#curr_id_Csv").val(curr_id);
            $("#CustRegion_Csv").val("CustReionWise");
            $("#DataShowCsvWise").val("CW_Inv_Item_Details");
            //----------for Csv Print------------//
        }
    })
}
/*---------------------------------- Customer Wise Sales Details End------------------------------------------ */
/*---------------------------------- Product Wise Sales Details ------------------------------------------ */
function BindProductWiseDetails() {
    debugger;
  
    //var sale_type = $("#ddl_SaleType").val();
   // var productgrp = $("#ddl_ProductGrpName").val();
    //var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    //var HSN_code = $("#HSN_cd").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    var productgrp = cmn_getddldataasstring("#ddl_ProductGrpName");
    var productPort = cmn_getddldataasstring("#ProductPortfolio");
    var HSN_code = cmn_getddldataasstring("#HSN_cd");
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    //replaceDatatablesIds("dttbl2");
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSales_DetailsProductWiseByFilter",
        data: {
            sale_type: sale_type, curr_id: "", productgrp: productgrp, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs, HSN_code: HSN_code,brlist: brid_list
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesDetailsProductWiseList").html(data);
            if (ShowAs == "S") {
                cmn_apply_datatable("#ProductWiseSummary");
                //$("#ProductWiseSummary_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                ////$("#ProductWiseSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailProductWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //$("#ProductWiseSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsProductWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else {
                //cmn_delete_datatable("#ProductWiseDetail11");
                cmn_apply_datatable("#ProductWiseDetail");
               // $("#ProductWiseDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
               //// $("#CustomerWiseSummery_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailProductWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
               // $("#ProductWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsProductWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            $("#DataShowCsvWise").val("ProductWise");
            //replaceDatatablesIds("datatable-buttons2");
        }
    })
}
function PW_OnclickInvoiceAmt(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var Product_Id = currentrow.find("#Hdn_item_id").val();
    var Product_name = currentrow.find("#item_name_").text();
    var uom = currentrow.find("#uom_alias_").text();
    var currency = currentrow.find("#curr_symbol_").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    var Quantity = currentrow.find("#item_qty_").text();
    var sale_type = $("#ddl_SaleType").val();
    var productgrp = $("#ddl_ProductGrpName").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch")
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetPW_Sales_Invoice_Details",
        data: {
            Product_Id: Product_Id, sale_type: sale_type, curr_id: curr_id, productgrp: productgrp, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt, brlist: brid_list
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_ProductWiseInvoiceDetail");
            $("#ProductWiseInvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_ProductWiseInvoiceDetail");
            //$("#tbl_ProductWiseInvoiceDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_ProductWiseInvoiceDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailInoviceWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#PWProductName").val(Product_name);
            $("#PWUOM").val(uom)
            $("#PWCurrency").val(currency);
            $("#PWCurrency_id").val(curr_id);
            $("#PWQuantity").val(Quantity);
            $("#PwFromDate").val(From_dt);
            $("#PWToDate").val(To_dt);
            //----------for Csv Print------------//
            $("#Product_Id_Csv").val(Product_Id);
            $("#sale_type_Csv").val(sale_type);
            $("#productgrp_Csv").val(productgrp);
            $("#productPort_Csv").val(productPort);
            $("#curr_id_Csv").val(curr_id);
            $("#DataShowCsvWise").val("SDProductWiseInvoce");
            //----------for Csv Print------------//
        }
    })
}
/*---------------------------------- Product Wise Sales Details End------------------------------------------ */
/*---------------------------------- Product Group Wise Sales Details ------------------------------------------ */
function BindProductGroupWiseDetails() {
    debugger;

    //var sale_type = $("#ddl_SaleType").val();
   // var productgrp = $("#ddl_ProductGrpName").val();
    //var productPort = $("#ProductPortfolio").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    var productgrp = cmn_getddldataasstring("#ddl_ProductGrpName");
    var productPort = cmn_getddldataasstring("#ProductPortfolio");
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    //replaceDatatablesIds("dttbl3");
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSales_DetailsProductGroupWiseByFilter",
        data: {
            sale_type: sale_type,curr_id:"", productgrp: productgrp, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs,brlist: brid_list
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesDetailsProductGroupWiseList").html(data);
            if (ShowAs == "S") {
                cmn_apply_datatable("#ProductGroupWiseSummary");
                //$("#ProductGroupWiseSummary_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                ////$("#ProductGroupWiseSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailProductGroupWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //$("#ProductGroupWiseSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsProductGroupWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else {
                cmn_apply_datatable("#ProductGroupWiseDetail");
                //$("#ProductGroupWiseDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                ////$("#ProductGroupWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailProductGroupWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //$("#ProductGroupWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsProductGroupWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            $("#DataShowCsvWise").val("ProductGroupWise");
            //replaceDatatablesIds("datatable-buttons3");
        }
    })
}
function PGW_OnclickInvoiceAmt(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var grp_Id = currentrow.find("#hdnGrpId").text().trim();
    var grp_name = currentrow.find("#item_group_name").text();
    var currency = currentrow.find("#_curr_symbol").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    var uom_id = currentrow.find("#txt_uomid").text();
    var sale_type = $("#ddl_SaleType").val();
   // var productgrp = $("#ddl_ProductGrpName").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetPGW_Sales_Invoice_product_Details",
        data: {
            sale_type: sale_type, curr_id: curr_id, productgrp: grp_Id, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt, brlist: brid_list, uom_id: uom_id
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_ProductGroupWiseProductDetail");
            $("#ProductGroupWiseProductDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_ProductGroupWiseProductDetail");
            //$("#tbl_ProductGroupWiseProductDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_ProductGroupWiseProductDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetPGW_Sales_Invoice_product_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#PGWProductgrpId").val(grp_Id);
            $("#PGWGroupName").val(grp_name);
            $("#PGWCurrency").val(currency);
            $("#PGWCurrency_id").val(curr_id);
            $("#PGWFromDate").val(From_dt);
            $("#PGWToDate").val(To_dt);
            //----------for Csv Print------------//
            $("#sale_type_Csv").val(sale_type);
            $("#productgrp_Csv").val(grp_Id);
            $("#productPort_Csv").val(productPort);
            $("#curr_id_Csv").val(curr_id);
            $("#DataShowCsvWise").val("SDProductWise");
            //----------for Csv Print------------//
        }
    })
}
function PGW_OnclickPopUpInvoiceAmt(e) {

    debugger;
    var currentrow = $(e.target).closest("tr");
    var Product_Id = currentrow.find("#Hdn_item_id").val();
    var Product_name = currentrow.find("#item_nam").text().trim();
    var uom = currentrow.find("#uom_alia").text().trim();
    //var currency = $("#PGWCurrency").val().trim();
    //var curr_id = $("#PGWCurrency_id").val().trim();
    var currency = currentrow.find("#_curr_symbol").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    var Quantity = currentrow.find("#item_qt").text().trim();
    var sale_type = $("#ddl_SaleType").val();
    var productgrp = $("#PGWProductgrpId").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var pagename = $("#txtpage").text();
    var uom_id = currentrow.find("#txt_uomid").text().trim();
    if (pagename == "ProductGroupWisedetail") {
        var brid_list = currentrow.find("#txtbr_id").text();
    }
    else {
        var brid_list = cmn_getddldataasstring("#ddl_branch");
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetPGW_Sales_Invoice_Details",
        data: {
            Product_Id: Product_Id, sale_type: sale_type, curr_id: curr_id, productgrp: productgrp, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt, brlist: brid_list, uom_id: uom_id
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_ProductGroupWiseInvoiceDetail");
            $("#ProductGroupWiseInvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_ProductGroupWiseInvoiceDetail");
            //$("#tbl_ProductGroupWiseInvoiceDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_ProductGroupWiseInvoiceDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetPGW_Sales_Invoice_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#PGWP_ProductName").val(Product_name);
            $("#PGWP_UOM").val(uom)
            $("#PGWP_Currency").val(currency);
            $("#PGWP_Quantity").val(Quantity);
            $("#PGWP_FromDate").val(From_dt);
            $("#PGWP_ToDate").val(To_dt);
            //----------for Csv Print------------//
            $("#PGWP_Product_ID_Csv").val(Product_Id);
            $("#sale_type_Csv").val(sale_type);
            $("#productgrp_Csv").val(productgrp);
            $("#productPort_Csv").val(productPort);
            $("#curr_id_Csv").val(curr_id);
            $("#DataShowCsvWise").val("SDProductWiseInvoce");
            //----------for Csv Print------------//

        }
    })
    $("#PGWProductgrpId").val('');
}
/*---------------------------------- Product Group Wise Sales Details End------------------------------------------ */
/*---------------------------------- Region Wise Sales Details ------------------------------------------ */
function BindRegionWiseDetails() {
    debugger;

    //var sale_type = $("#ddl_SaleType").val();
    //var RegionName = $("#ddl_RegionName").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    var RegionName = cmn_getddldataasstring("#ddl_RegionName");
    //var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    //replaceDatatablesIds("dttbl4");
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSales_DetailsRegionWiseByFilter",
        data: {
            sale_type: sale_type,curr_id:"", RegionName: RegionName,
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs,brlist: brid_list
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesDetailsRegionWiseList").html(data);
            if (ShowAs == "S") {
                cmn_apply_datatable("#RegionWiseSummary");
                //$("#RegionWiseSummary_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                ////$("#RegionWiseSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailRegionWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //$("#RegionWiseSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsRegionWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else {
                cmn_apply_datatable("#RegionWiseDetail");
                //$("#RegionWiseDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                ////$("#RegionWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailRegionWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //$("#RegionWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="GetSales_DetailsRegionWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            $("#DataShowCsvWise").val("RegionWise");
            //replaceDatatablesIds("datatable-buttons4");
        }
    })
}
function R_OnclickInvoiceAmt(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var Region_Id = currentrow.find("#hdnRegionId").text().trim();
    var Region_name = currentrow.find("#cust_region").text();
    var currency = currentrow.find("#curr_symbo").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    var sale_type = $("#ddl_SaleType").val();
    // var productgrp = $("#ddl_ProductGrpName").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    var To_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetRegionWise_Customer_Details",
        data: {
            sale_type: sale_type, curr_id: curr_id, Region_Id: Region_Id, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt, brlist: brid_list
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_RegionWiseCustomerDetail");
            $("#RegionWiseCustomerDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_RegionWiseCustomerDetail");
            //$("#tbl_RegionWiseCustomerDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_RegionWiseCustomerDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="R_OnclickInvoiceAmtCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#HdnRegionId").val(Region_Id);
            $("#RW_RegionName").val(Region_name);
            $("#RW_Currency").val(currency);
            $("#RWCurrency_id").val(curr_id);
            $("#RW_FromDate").val(From_dt);
            $("#RW_ToDate").val(To_dt);
            //----------for Csv Print------------//
            $("#Region_Id_Csv").val(Region_Id);
            $("#sale_type_Csv").val(sale_type);
            $("#productPort_Csv").val(productPort);
            $("#curr_id_Csv").val(curr_id);
            $("#DataShowCsvWise").val("SDCostomerWise");
            //----------for Csv Print------------//
        }
    });
}
function R_OnclickPopUpInvoiceAmt(e) {

    debugger;
    var currentrow = $(e.target).closest("tr");
    var Cust_Id = currentrow.find("#hdnCustomerId").text();
    var Cust_name = currentrow.find("#custmar_name").text().trim();
    var currency = currentrow.find("#currancy_symbol").text().trim();
    var curr_id = $("#RWCurrency_id").val().trim();
    var sale_type = $("#ddl_SaleType").val();
    var regionId = $("#HdnRegionId").val();
    var regionName = $("#RW_RegionName").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetRegionWise_Sales_Invoice_Details",
        data: {
            Cust_Id: Cust_Id, sale_type: sale_type, curr_id: curr_id, regionId: regionId,
            From_dt: From_dt, To_dt: To_dt, brlist: brid_list
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_RegionWiseInvoiceSummary");
            $("#RegionWiseInvoiceSummaryPopUp").html(data);
            cmn_apply_datatable("#tbl_RegionWiseInvoiceSummary");
            //$("#tbl_RegionWiseInvoiceSummary_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_RegionWiseInvoiceSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailInoviceWiseCSV1()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#RWP_RegionName").val(regionName);
            $("#RWP_hdnRegionId").val(regionId);
            $("#RWP_CustomerName").val(Cust_name);
            $("#RWP_hdnCustomerId").val(Cust_Id);
            $("#RWP_Currency").val(currency);
            $("#PWPCurrency_id").val(curr_id);
            $("#RWP_FromDate").val(From_dt);
            $("#RWP_ToDate").val(To_dt);
            //----------for Csv Print------------//
            $("#Cust_Id_Csv").val(Cust_Id);
            $("#Region_Id_Csv").val(regionId);
            $("#sale_type_Csv").val(sale_type);
            $("#curr_id_Csv").val(curr_id);
            $("#DataShowCsvWise").val("CW_Inv_Details");
            //----------for Csv Print------------//

        }
    })

}
function R_OnclickPopUp2InvoiceAmt(e) {

    debugger;
    var currentrow = $(e.target).closest("tr");
    var Cust_Id = $("#RWP_hdnCustomerId").val();
    var Cust_name = $("#RWP_CustomerName").val().trim();
    var invoice_no = currentrow.find("#app_inv_n").text().trim();
    var currency = $("#RWP_Currency").val().trim();
    var curr_id = $("#PWPCurrency_id").val().trim();
    var invoice_dt = currentrow.find("#inv_dte").text().trim();
    var sale_type = $("#ddl_SaleType").val();
    var regionId = $("#HdnRegionId").val();
    //var regionName = $("#RW_RegionName").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = currentrow.find("#txtbr_id").text();
    //var FinalDetail = [];
    //FinalDetail.push({
    //    Cust_Id: Cust_Id, sale_type: sale_type, regionId: regionId, invoice_no: invoice_no, invoice_dt: invoice_dt,
    //    From_dt: From_dt, To_dt: To_dt });

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetRegionWise_Sales_Invoice_Product_Details",
        data: //JSON.stringify({ SalesDetail: FinalDetail}),
        {
            Cust_Id: Cust_Id, sale_type: sale_type, curr_id: curr_id, regionId: regionId, invoice_no: invoice_no, invoice_dt: invoice_dt,
            From_dt: From_dt, To_dt: To_dt, brlist: brid_list
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_RegionWiseInvoiceProductDetail");
            $("#RegionWiseInvoiceProductDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_RegionWiseInvoiceProductDetail");
            //$("#tbl_RegionWiseInvoiceProductDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_RegionWiseInvoiceProductDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailInoviceWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#RWPP_CustomerName").val(Cust_name);
            $("#RWPP_InvoiceNumber").val(invoice_no);
            $("#RWPP_InvoiceDate").val(invoice_dt);//
            $("#RWPP_Currency").val(currency);
            //----------for Csv Print------------//
            $("#Cust_Id_Csv").val(Cust_Id);
            $("#Region_Id_Csv").val(regionId);
            $("#sale_type_Csv").val(sale_type);
            $("#Inv_no_Csv").val(invoice_no);
            $("#inv_dt_Csv").val(invoice_dt);
            $("#curr_id_Csv").val(curr_id);
            $("#InvRegion_Csv").val("InvRegion");
            $("#DataShowCsvWise").val("CW_Inv_Item_Details");
            //----------for Csv Print------------//
        }
    })

}
/*---------------------------------- Region Wise Sales Details End------------------------------------------ */
/*---------------------------------- Sales Executive Wise Sales Details ------------------------------------------ */
function BindSalesExecutiveWiseDetails() {
    debugger;
   // var sale_type = $("#ddl_SaleType").val();
    //var SaleRepresentive = $("#ddl_SalesRepresentative").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    var SaleRepresentive = cmn_getddldataasstring("#ddl_SalesRepresentative");
    //var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    //replaceDatatablesIds("dttbl5");
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSales_DetailsSalePersonWiseByFilter",
        data: {
            sale_type: sale_type,curr_id:"", sale_per: SaleRepresentive,
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs, brlist: brid_list
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesDetailsSalesExecutiveWiseList").html(data);
            if (ShowAs == "S") {
                cmn_apply_datatable("#SalePerWiseSummary");
                //$("#SalePerWiseSummary_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                ////$("#SalePerWiseSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 Csv" id="CSVPrintSD" onclick="SalesDetailSalesExecutiveWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //$("#SalePerWiseSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 Csv" id="CSVPrintSD" onclick="GetSales_DetailsSalePersonWise()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            else {
                cmn_apply_datatable("#SalePerWiseDetail");
                //$("#SalePerWiseDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                ////$("#SalePerWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 Csv" id="CSVPrintSD" onclick="SalesDetailSalesExecutiveWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //$("#SalePerWiseDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 Csv" id="CSVPrintSD" onclick="GetSales_DetailsSalePersonWise()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            }
            $("#DataShowCsvWise").val("SalesExecutiveWise");
            //replaceDatatablesIds("datatable-buttons5");
        }
    })
}
function SaleExctv_OnclickInvoiceAmt(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var sale_per_id = currentrow.find("#sale_pr_id").text().trim();
    var sale_per = currentrow.find("#sale_pr").text();
    var currency = currentrow.find("#currancy").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    var sale_type = $("#ddl_SaleType").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSalePersonWise_Customer_Details",
        data: {
            sale_type: sale_type, curr_id: curr_id, sale_per: sale_per_id,
            From_dt: From_dt, To_dt: To_dt,brlist: brid_list
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_SalesExecutiveWiseCustomerDetail");
            $("#SalesExecutiveWiseCustomerDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_SalesExecutiveWiseCustomerDetail");
            //$("#tbl_SalesExecutiveWiseCustomerDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_SalesExecutiveWiseCustomerDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SalesDetailSaleExctvCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#sale_Exc_id").val(sale_per_id);
            $("#SalesExecutiveName").val(sale_per);
            $("#SaleEx_Currency").val(currency);
            $("#SaleEx_Currency_id").val(curr_id);
            $("#SaleEx_FromDate").val(From_dt);
            $("#SaleEx_ToDate").val(To_dt);
            //----------for Csv Print------------//
            $("#sale_per_id_Csv").val(sale_per_id);
            $("#sale_type_Csv").val(sale_type);
            $("#curr_id_Csv").val(curr_id);
            $("#DataShowCsvWise").val("SDCostomerWise");
            //----------for Csv Print------------//
        }
    });
}
function SaleExctv_OnclickPopUpInvoiceAmt(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    if ($("#SalesMISSalesSummary").is(":checked")) {
        var sale_per_id = $("#sale_Exc_id").val().trim();
        var sale_per = $("#SalesExecutiveName").val();
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        var sale_per_id = currentrow.find("#sale_pr_id").text().trim();
        var sale_per = currentrow.find("#sale_pr").text();
    }
   
    var cust_id = currentrow.find("#custmer_id").text().trim();
    var cust_name = currentrow.find("#custmer_nm").text();
    var currency = currentrow.find("#currsymbol").text();
    var curr_id = currentrow.find("#SaleEx_Currency_id").text();
    var sale_type = $("#ddl_SaleType").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSalePersonWise_Customer_Invoice_Details",
        data: {
            sale_type: sale_type, curr_id: curr_id, sale_per: sale_per_id, cust_id: cust_id,
            From_dt: From_dt, To_dt: To_dt, brlist: brid_list
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_SalesExecutiveWiseInvoiceSummary");

            $("#SalesExecutiveWiseInvoiceSummaryPopUp").html(data);

            cmn_apply_datatable("#tbl_SalesExecutiveWiseInvoiceSummary");
            //$("#tbl_SalesExecutiveWiseInvoiceSummary_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_SalesExecutiveWiseInvoiceSummary_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SaleExctv_OnclickPopUpInvoiceAmtCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#sale_Exc_id").val(sale_per_id);
            $("#P_SalesExecutiveName").val(sale_per);
            $("#SE_CustomerName").val(cust_name)
            $("#SE_Currency").val(currency);
            $("#SE_Currency_id").val(curr_id); 
            $("#SE_FromDate").val(From_dt);
            $("#SE_ToDate").val(To_dt);

            //----------for Csv Print------------//
            $("#Cust_Id_Csv").val(cust_id);
            $("#sale_per_id_Csv").val(sale_per_id);
            $("#sale_type_Csv").val(sale_type);
            $("#curr_id_Csv").val(curr_id);
            //----------for Csv Print------------//
        }
    });
}
function SaleExctv_OnclickPopUpGetItemDetail(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var cust_name = $("#SE_CustomerName").val();
    var inv_dt = currentrow.find("#invoice_dt").text().trim();
    var inv_no = currentrow.find("#app_inv").text().trim();
    var currency = $("#SE_Currency").val();
    var curr_id = $("#SE_Currency_id").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var brid_list = currentrow.find("#txtbr_id").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesDetail/GetSalePersonWise_Customer_Invoice_Item_Details",
        data: {
            inv_no: inv_no, inv_dt: inv_dt, curr_id:curr_id,
            From_dt: From_dt, To_dt: To_dt, brlist: brid_list
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_SalesExecutiveWiseProductDetail");

            $("#SalesExecutiveWiseProductDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_SalesExecutiveWiseProductDetail");
            //$("#tbl_SalesExecutiveWiseProductDetail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#tbl_SalesExecutiveWiseProductDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CSVPrintSD" onclick="SaleExctv_OnclickPopUpGetItemDetailCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#SE_Item_CustomerName").val(cust_name)
            $("#SE_Item_Currency").val(currency);
            $("#SE_Item_InvoiceNumber").val(inv_no);
            $("#SE_Item_InvoiceDate").val(inv_dt);
            //----------for Csv Print------------//
            $("#SE_Item_Currency_Csv").val(curr_id);
            //----------for Csv Print------------//
        }
    });
}
/*---------------------------------- Sales Executive Wise Sales Details ------------------------------------------ */

/*Add by Hina  on for Service sale invoice and scrap sale invoice */
function OnChngSalesTyp() {
    debugger;
    var Salestyp = $("#ddl_SaleType option:selected").val();
    if (Salestyp == "SS" || Salestyp == "DI") {
        $("#div_ddl_SalesRepresentative").css("display", "none");
    }
    else {
        $("#div_ddl_SalesRepresentative").css("display", "block");
    }
}
function SalesDetailInoviceWiseCSV() {
    debugger;
    var DataShow = $("#DataShowCsvWise").val();
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    if (DataShow == "SDProductWiseInvoce" || DataShow == "SDProductWise") {  
        if (DataShow == "SDProductWiseInvoce") {
            var product_id = $("#Product_Id_Csv").val();
        }
        var sale_type = $("#sale_type_Csv").val();
        var productGrp = $("#productgrp_Csv").val();
        var productPort = $("#productPort_Csv").val();
    }
    if (DataShow == "SDCostomerWise") {
        var reg_name = $("#Region_Id_Csv").val();
        var productPort = $("#productPort_Csv").val();
        var sale_type = $("#sale_type_Csv").val();
    }
    var inv_no = $("#Inv_no_Csv").val();
    var inv_dt = $("#inv_dt_Csv").val();
    var curr_id = $("#curr_id_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    if (DataShow == "CW_Inv_Details" || DataShow == "CW_Inv_Item_Details") {
        var cust_id = $("#Cust_Id_Csv").val();
        var reg_name = $("#Region_Id_Csv").val();
        var sale_type = $("#sale_type_Csv").val();
    }
    if (DataShow == "ItemDetailsInterBrch") {/*add by Hina Sharma on 27-06-2025*/
        var reg_name = "";
        var sale_type = "";
        var productGrp = "";
        var productPort = "";
        var sale_per = "";
        var custCat = "";
        var custPort = "";
        
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function SalesDetailInoviceWiseCSV1() {
    debugger;
    var DataShow = "CW_Inv_Details";
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var inv_no = "";
    var inv_dt = "";  
    var curr_id = $("#curr_id_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var reg_name = 0;
    var cust_id = $("#Cust_Id_Csv").val();
    var sale_type = $("#sale_type_Csv").val();
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function SalesDetailSaleExctvCSV() {
    debugger;
    //var DataShow = "SDCostomerWise";
    var DataShow = "SalePersonWiseSDCostomerWise";
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var sale_per = $("#sale_per_id_Csv").val();
    var sale_type = $("#sale_type_Csv").val();
    var curr_id = $("#SaleEx_Currency_id").val()//$("#curr_id_Csv").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function SaleExctv_OnclickPopUpInvoiceAmtCSV() {
    //var DataShow = "CW_Inv_Details";
    var DataShow = "SalePersonWiseCW_Inv_Details";
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var sale_per = $("#sale_per_id_Csv").val();
    var sale_type = $("#sale_type_Csv").val();
    var curr_id = $("#curr_id_Csv").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var cust_id = $("#Cust_Id_Csv").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function SaleExctv_OnclickPopUpGetItemDetailCSV() {
    var DataShow = "CW_Inv_Item_Details";
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var sale_per = $("#sale_per_id_Csv").val();
    var sale_type = $("#sale_type_Csv").val();
    var curr_id = $("#SE_Item_Currency_Csv").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var cust_id = $("#Cust_Id_Csv").val();
    var inv_no = $("#SE_Item_InvoiceNumber").val();
    var inv_dt = $("#SE_Item_InvoiceDate").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function R_OnclickInvoiceAmtCSV() {
    var DataShow = "SDCostomerWise";
    var cust_id = "";//$("#ddl_CustomerName").val();
    var reg_name = $("#HdnRegionId").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var sale_per = $("#sale_per_id_Csv").val();
    var sale_type = $("#ddl_SaleType").val();
    var curr_id = $("#RWCurrency_id").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var cust_id = "";//$("#Cust_Id_Csv").val();
    var inv_no = $("#SE_Item_InvoiceNumber").val();
    var inv_dt = $("#SE_Item_InvoiceDate").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetSales_DetailsSalePersonWise() {
    debugger;
    var DataShow = "SalesExecutiveWise";//$("#DataShowCsvWise").val();
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    if (DataShow == "SDProductWiseInvoce" || DataShow == "SDProductWise") {
        if (DataShow == "SDProductWiseInvoce") {
            var product_id = $("#Product_Id_Csv").val();
        }
        var sale_type = $("#sale_type_Csv").val();
        var productGrp = $("#productgrp_Csv").val();
        var productPort = $("#productPort_Csv").val();
    }
    if (DataShow == "SDCostomerWise") {
        var reg_name = $("#Region_Id_Csv").val();
        var productPort = $("#productPort_Csv").val();
        var sale_type = $("#sale_type_Csv").val();
    }
    if (DataShow == "CW_Inv_Item_Details") {
        var InvRegion = $("#InvRegion_Csv").val()
        if (InvRegion == "InvRegion") {
            var inv_no = $("#Inv_no_Csv").val();
            var inv_dt = $("#inv_dt_Csv").val();
        }
    }
    else {
        var inv_no = $("#Inv_no_Csv").val();
        var inv_dt = $("#inv_dt_Csv").val();
    }
    var curr_id = $("#curr_id_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    if (DataShow == "CW_Inv_Details" || DataShow == "CW_Inv_Item_Details") {
        var cust_id = $("#Cust_Id_Csv").val();
        var CustRegionWise = $("#CustRegion_Csv").val();
        var InvRegion = $("#InvRegion_Csv").val()
        if (InvRegion == "InvRegion") {
            var reg_name = 0;
        }
        if (CustRegionWise == "CustReionWise") {
            var reg_name = $("#Region_Id_Csv").val();
        }
        var sale_type = $("#sale_type_Csv").val();
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetSales_DetailsRegionWiseCSV() {
    var DataShow = "RegionWise";//$("#DataShowCsvWise").val();
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val(); 
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    if (DataShow == "SDProductWiseInvoce" || DataShow == "SDProductWise") {
        if (DataShow == "SDProductWiseInvoce") {
            var product_id = $("#Product_Id_Csv").val();
        }
        var sale_type = $("#sale_type_Csv").val();
        var productGrp = $("#productgrp_Csv").val();
        var productPort = $("#productPort_Csv").val();
    }
    if (DataShow == "SDCostomerWise") {
        var reg_name = $("#Region_Id_Csv").val();
        var productPort = $("#productPort_Csv").val();
        var sale_type = $("#sale_type_Csv").val();
    }
    var inv_no = "";//$("#Inv_no_Csv").val();
    var inv_dt = "";//$("#inv_dt_Csv").val();
    var curr_id = $("#curr_id_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    if (DataShow == "CW_Inv_Details" || DataShow == "CW_Inv_Item_Details") {
        var cust_id = $("#Cust_Id_Csv").val();
        var reg_name = $("#Region_Id_Csv").val();
        var sale_type = $("#sale_type_Csv").val();
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetSales_DetailsProductGroupWiseCSV() {
    var DataShow = "ProductGroupWise";//$("#DataShowCsvWise").val();
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    if (DataShow == "SDProductWiseInvoce" || DataShow == "SDProductWise") {
        if (DataShow == "SDProductWiseInvoce") {
            var product_id = $("#Product_Id_Csv").val();
        }
        var sale_type = $("#sale_type_Csv").val();
        var productGrp = $("#productgrp_Csv").val();
        var productPort = $("#productPort_Csv").val();
    }
    if (DataShow == "SDCostomerWise") {
        var reg_name = $("#Region_Id_Csv").val();
        var productPort = $("#productPort_Csv").val();
        var sale_type = $("#sale_type_Csv").val();
    }
    var inv_no = "";//$("#Inv_no_Csv").val();
    var inv_dt = "";//$("#inv_dt_Csv").val();
    var curr_id = $("#curr_id_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    if (DataShow == "CW_Inv_Details" || DataShow == "CW_Inv_Item_Details") {
        var cust_id = $("#Cust_Id_Csv").val();
        var reg_name = $("#Region_Id_Csv").val();
        var sale_type = $("#sale_type_Csv").val();
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetPGW_Sales_Invoice_product_DetailsCSV() {
    var DataShow = "SDProductWise";
    var cust_id = "";
    var reg_name = "";
    var sale_type = $("#ddl_SaleType").val();
    var curr_id = $("#PGWCurrency_id").val()
    var product_id = "";//$("#ddl_ProductName").val();
    var productGrp = $("#PGWProductgrpId").val();
    var sale_per = "";//$("#ddl_SalesRepresentative").val();
    var custCat = "";//$("#CustomerCategory").val();
    var custPort = "";//$("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var inv_no = "";//$("#Inv_no_Csv").val();
    var inv_dt = "";//$("#inv_dt_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetPGW_Sales_Invoice_DetailsCSV() {
    var DataShow = "SDProductWiseInvoce";
    var cust_id = "";
    var reg_name = "";
    var sale_type = $("#ddl_SaleType").val();
    var curr_id = $("#PGWCurrency_id").val().trim();
    var product_id = $("#PGWP_Product_ID_Csv").val();//$("#ddl_ProductName").val();
    var productGrp = $("#PGWProductgrpId").val();
    var sale_per = "";//$("#ddl_SalesRepresentative").val();
    var custCat = "";//$("#CustomerCategory").val();
    var custPort = "";//$("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var inv_no = "";//$("#Inv_no_Csv").val();
    var inv_dt = "";//$("#inv_dt_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetSales_DetailsProductWiseCSV() {
    debugger;
    var DataShow = "ProductWise";//$("#DataShowCsvWise").val();
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var HSN_code = $("#HSN_cd").val();
    var inv_no = "";//$("#Inv_no_Csv").val();
    var inv_dt = "";//$("#inv_dt_Csv").val();
    var curr_id = "";//$("#curr_id_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetSales_DetailsCostomerWiseCSV() {
    debugger;
    var DataShow = "CustomerWise";//$("#DataShowCsvWise").val();
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val(); 
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var inv_no = "";//$("#Inv_no_Csv").val();
    var inv_dt = "";//$("#inv_dt_Csv").val();
    var curr_id = ""//$("#curr_id_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetCW_Sales_Invoice_DetailsCSV() {
    debugger;
    var DataShow = "CW_Inv_Details";//$("#DataShowCsvWise").val();
    var cust_id = $("#hdnCustId").val();
    var curr_id = $("#Currency_id").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var inv_no = "";//$("#Inv_no_Csv").val();
    var inv_dt = "";//$("#inv_dt_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetCW_Sales_Invoice_Item_DetailsCSV() {
    var DataShow = "CW_Inv_Item_Details";//$("#DataShowCsvWise").val();
    var cust_id = $("#hdnCustId").val();
    var curr_id = $("#Currency_id").val();
    var reg_name = "";//$("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var inv_no = $("#itmInvoiceNumber_Csv").val();
    var inv_dt = $("#itmInvoiceDate_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetSales_DetailsCSV() {
    debugger;
    var DataShow = "InoviceWise";//$("#DataShowCsvWise").val();
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var inv_no = "";//$("#Inv_no_Csv").val();
    var inv_dt = "";//$("#inv_dt_Csv").val();

    var curr_id = "";//$("#curr_id_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}
function GetSalesDetailsInterBrchCSV() {
    debugger;
    var DataShow = "InterBrchWise";//$("#DataShowCsvWise").val();
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = $("#ddl_ProductName").val();
    var productGrp = $("#ddl_ProductGrpName").val();
    var sale_per = $("#ddl_SalesRepresentative").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var inv_no = "";//$("#Inv_no_Csv").val();
    var inv_dt = "";//$("#inv_dt_Csv").val();

    var curr_id = "";//$("#curr_id_Csv").val();
    var ShowAs;
    if ($("#SalesMISSalesSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISSalesDetail").is(":checked")) {
        ShowAs = "D";
    }
    var HSN_code = "";
    window.location.href = "/ApplicationLayer/SalesDetail/SalesDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt + "&HSN_code=" + HSN_code;
}

function OnclickPaidAmtDetailIcon(e,Flag) {/*Add by hina sharma on 10-12-2024*/
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var InVNo = CurrentRow.find("#txtInv_no").text();
    var InvDt = CurrentRow.find("#txtInv_dt1").text();
    var InvDate = InvDt.split("-").reverse().join("-");
    var Fromdate = $("#txtFromdate").val();
    var Todate = $("#txtTodate").val();
    var Curr_id = CurrentRow.find("#txtCurr_id").text();
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/SalesDetail/OnclickPaidAmountDetail",
            data: {
                InVNo: InVNo,
                InvDate: InvDate,
                Curr_id: Curr_id,
                Fromdate: Fromdate,
                Todate: Todate,
                Flag: Flag
            },
            success: function (data) {
                debugger;
                $('#PaidAmountDetailsPopup').html(data);
                $("#Prtal_PAInvoiceNumber").val(InVNo);
                $("#Prtal_PAInvoiceDate").val(InvDt);

                cmn_apply_datatable("#tbl_PaidAmtDetails");

                //$("#tbl_PaidAmtDetails_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#tbl_PaidAmtDetails_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="AccountRecevablPaidAmtInsightCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });

    } catch (err) {
        debugger;
        console.log("Trial Balance Error : " + err.message);
    }
}
function onchangeState() {
    BindCityList();
}
function BindCityList() {
    debugger;
    var state_id = $("#ddl_state").val()
    $("#ddl_city").append("<option value='0'>---Select---</option>");
    $("#ddl_city").select2({
        ajax: {
            url: "/ApplicationLayer/SalesDetail/BindCityListdata",
            data: function (params) {
                var queryParameters = {
                    SearchCity: params.term,
                    state_id: state_id,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize
                pageSize = 20; // or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                  <div class="row"><div class="col-md-6 col-xs-6 def-cursor">${$("#span_City").text()}</div>
                  <div class="col-md-3 col-xs-6 def-cursor">${$("#span_State").text()}</div>
                  <div class="col-md-3 col-xs-6 def-cursor">${$("#span_Country").text()}</div></div>
                  </strong></li></ul>`)
                }
                //var page = 1;
                var page = params.page || 1;
                Fdata = data.slice((page - 1) * pageSize, page * pageSize)
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0,0,0", Name: " ---Select---" };
                            Fdata.unshift(select);
                        }
                    }
                }
                debugger;
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split(",")[0], text: val.Name, UOM: val.ID.split(",")[1], type: val.ID.split(",")[2] };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
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
function BindStateList() {
    debugger;
    $("#ddl_state").append("<option value='0'>---Select---</option>");
    $("#ddl_state").select2({
        ajax: {
            url: "/ApplicationLayer/SalesDetail/BindStateListData",
            data: function (params) {
                var queryParameters = {
                    SearchState: params.term,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize
                pageSize = 20; // or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                   <div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#span_State").text()}</div>
                   <div class="col-md-3 col-xs-6 def-cursor">${$("#span_Country").text()}</div></div>
                   </strong></li></ul>`)
                }
                //var page = 1;
                var page = params.page || 1;
                Fdata = data.slice((page - 1) * pageSize, page * pageSize)
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0,0", Name: " ---Select---" };
                            Fdata.unshift(select);
                        }
                    }
                }
                debugger;
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split(",")[0], text: val.Name, State: val.ID.split(",")[1] };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
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
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.State + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}