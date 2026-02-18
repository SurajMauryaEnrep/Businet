$(document).ready(function () {
    debugger
    //$("#ddl_Sales_by").val("I").change();
    $("#ddlItemsList").select2();
    //$("#ddlCurrencylist").select2();
   // $("#ddl_RegionName").select2();
   // $("#CustomerCategory").select2();
    //$("#CustomerPortfolio").select2();
    $("#ddlsono").select2();
    $("#ddlsodt").select2();
    //OnchangeSalesBy()
    BindCustomerList();
    BindSalesPersonList();
    BindProductGrpName();
    BindProductPortfolio();
    OnchangeSalesBy();
    replaceDatatablesIds("DataTable1");
    Cmn_initializeMultiselect([
        '#ddlCurrencylist',
        '#ddl_RegionName',
        '#ddl_SaleType',
        '#CustomerCategory',
        '#CustomerPortfolio',
        '#ddlcustzone',
        '#ddlcustgroup',
    ]);
    BindCityList();
    BindStateList();
});
function onchangeShowAs() {
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    if (ddl_Sales_by == "O") {
        BindOrderWiseDetails();
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
    else if (ddl_Sales_by == "PO") {
        BindPendingOrderDetails();
    }
    else if (ddl_Sales_by == "POD") {
        BindPendingOrderItemWise();
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
        $("#" + TableId + "_next").prev().trigger("click");
        for (var i = 0; i < $("#" + TableId + "_next").prev().text(); i++) {
            $("#" + TableId + " > tbody > tr").each(function () {
                var currentRow = $(this);
                var saleAmt = parseFloat(currentRow.find("#txt_SaleAmtSpec").text().trim());
                if (isNaN(saleAmt)) saleAmt = 0;
                SaleAmtSpec = parseFloat(SaleAmtSpec) + parseFloat(saleAmt);
                var taxAmt = parseFloat(currentRow.find("#txt_TaxAmtSpec").text().trim());
                if (isNaN(taxAmt)) taxAmt = 0;
                TaxAmtSpec = parseFloat(TaxAmtSpec) + parseFloat(taxAmt);
                var ocAmt = parseFloat(currentRow.find("#txt_OcAmtSpec").text().trim());
                if (isNaN(ocAmt)) ocAmt = 0;
                OcAmtSpec = parseFloat(OcAmtSpec) + ocAmt;
                var saleAmount = parseFloat(currentRow.find("#txt_SaleAmt").text().trim());
                if (isNaN(saleAmount)) saleAmount = 0;
                SaleAmt = parseFloat(SaleAmt) + saleAmount;
                var taxamt1 = parseFloat(currentRow.find("#txt_TaxAmt").text().trim());
                if (isNaN(taxamt1)) taxamt1 = 0;
                TaxAmt = parseFloat(TaxAmt) + taxamt1;
                var txOcAmt = parseFloat(currentRow.find("#txt_OcAmt").text().trim());
                if (isNaN(txOcAmt)) txOcAmt = 0;
                OcAmt = parseFloat(OcAmt) + txOcAmt;
                var orderAmt = parseFloat(currentRow.find("#txt_InvoiceAmt").text().trim());
                if (isNaN(orderAmt)) orderAmt = 0;
                InvAmtBs = parseFloat(InvAmtBs) + orderAmt;
                var orderAmtSpec = parseFloat(currentRow.find("#txt_InvoiceAmtSpec").text().trim());
                if (isNaN(orderAmtSpec)) orderAmtSpec = 0;
                InvAmtSpcs = parseFloat(InvAmtSpcs) + orderAmtSpec;

            });
            $("#" + TableId + "_previous > a").trigger("click");
        }
    }

    var td = "";
    if (TableId == "datatable-buttons1" || TableId == "datatable-buttons")
        td = "<td colspan='1' align='right'></td>";
    $("#" + TableId + " > tfoot").html(`
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
           // var s = "<option value='0'>---All---</option>";
            $.map(data, function (val, item) {
                if (val.Name.trim() != "---Select---") {
                    s += '<option value=' + val.ID + '>' + val.Name + '</option>';
                }
            })
            $("#ddl_SalesRepresentative").html(s);
            Cmn_initializeMultiselect(['#ddl_SalesRepresentative']);
            $('#ddl_SalesRepresentative').multiselect('rebuild');
           // $("#ddl_SalesRepresentative").select2({});
        }
    });

}
function BindCustomerList() {
    debugger;
    var Branch = sessionStorage.getItem("BranchID");
    $.ajax({
        url: "/ApplicationLayer/MISSalesOrderDetail/GetAutoCompleteSearchCustList",
        data: {},
        success: function (data, params) {
            debugger;
            if (data == 'ErrorPage') {
                LSO_ErrorPage();
                return false;
            }
            var s = "";
           // var s = "<option value='0'>---All---</option>";
            $.map(data, function (val, item) {
                if (val.Name.trim() != "---Select---") {
                    s += '<option value=' + val.ID + '>' + val.Name + '</option>';
                }
            })
            $("#ddl_CustomerName").html(s);
            Cmn_initializeMultiselect(['#ddl_CustomerName']);
            $('#ddl_CustomerName').multiselect('rebuild');
           //$("#ddl_CustomerName").select2({});
        }
    });

}
function BindProductGrpName() {
    var $ddl = $("#ddl_ProductGrpName");
    var url = $("#ItemGrpName").val();
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (data) {
            $ddl.empty();
            $.each(data, function (i, val) {
                if (val.Name.trim() != "---Select---") {
                    $ddl.append('<option value="' + val.ID + '">' + val.Name + '</option>');
                }
            });
            Cmn_initializeMultiselect([$ddl]);
        },
        error: function (xhr, status, error) {
            console.error("Error loading product groups:", error);
        }
    });
}

//function BindProductGrpName() {
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
function BindProductPortfolio() {
    var $ddl = $("#ProductPortfolio");
    var url = $("#ItemPortfName").val();
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (data) {
            $ddl.empty();
            $.each(data, function (i, val) {
                if (val.Name.trim() != "---Select---") {
                    $ddl.append('<option value="' + val.ID + '">' + val.Name + '</option>');
                }
            });
            Cmn_initializeMultiselect([$ddl]);
        },
        error: function (xhr, status, error) {
        }
    });
}
//function BindProductPortfolio() {
//    $("#ProductPortfolio").select2({
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
    /*    debugger;*/
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    if (ddl_Sales_by == "O" || ddl_Sales_by == "C") {
        $("#div_clear").addClass("clear");
    } else {
        $("#div_clear").removeClass("clear");
    }
    if (ddl_Sales_by == "O") {
        $("#AllPartials > div, #div_productGrpName,#div_productName,#div_ddl_OrderDate,#div_ddl_OrderNumber").css("display", "none");
        $("#PartialMIS_SalesOrderDetailsOrderWiseList, #div_ddl_CustomerName, #div_ddl_RegionName, #div_ddl_SaleType, #div_ddl_SalesRepresentative, #div_ProductPortfolio, #div_CustomerCategory,#div_CustomerPortfolio,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "");
        BindOrderWiseDetails();
    }
    else if (ddl_Sales_by == "C") {
        $("#div_productGrpName, #div_ProductPortfolio,#div_productName, #div_ddl_SalesRepresentative,#div_ddl_OrderDate,#div_ddl_OrderNumber").css("display", "none");
        $("#AllPartials > div").css("display", "none");
        $("#PartialMIS_SalesOrderDetailsCustomerWiseList, #div_ddl_CustomerName, #div_ddl_RegionName,#div_CustomerCategory, #div_CustomerPortfolio, #div_ddl_SaleType,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "");
        BindCostomerWiseDetails();
    }
    else if (ddl_Sales_by == "P") {
        $("#AllPartials > div , #div_ddl_CustomerName,#div_ddl_SalesRepresentative, #div_ddl_RegionName, #div_CustomerCategory, #div_CustomerPortfolio,#div_ddl_OrderDate,#div_ddl_OrderNumber,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "none");
        $("#PartialMIS_SalesOrderDetailsProductWiseList,#div_productName, #div_ddl_SaleType,#div_productName, #div_productGrpName, #div_ProductPortfolio").css("display", "");
        BindProductWiseDetails();
    }
    else if (ddl_Sales_by == "G") {
        $("#AllPartials > div, #div_ddl_CustomerName, #div_productName, #div_ddl_SalesRepresentative, #div_ddl_RegionName, #div_CustomerCategory, #div_CustomerPortfolio,#div_ddl_OrderDate,#div_ddl_OrderNumber,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "none");
        $("#PartialMIS_SalesOrderDetailsProductGroupWiseList, #div_ddl_SaleType, #div_productGrpName, #div_ProductPortfolio").css("display", "");
        BindProductGroupWiseDetails();
    }
    else if (ddl_Sales_by == "R") {
        $("#div_productName, #div_ProductPortfolio, #div_CustomerCategory, #div_CustomerPortfolio, #div_ddl_SaleType, #div_ddl_SalesRepresentative,#div_ddl_OrderDate,#div_ddl_OrderNumber,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "none");
        $("#AllPartials > div, #div_ddl_CustomerName,#div_productName, #div_productGrpName").css("display", "none");
        $("#PartialMIS_SalesOrderDetailsRegionWiseList, #div_ddl_SaleType, #div_ddl_RegionName").css("display", "");
        BindRegionWiseDetails();
    }
    else if (ddl_Sales_by == "S") {
        $("#div_ddl_CustomerName, #div_ddl_RegionName, #div_productName, #div_CustomerCategory, #div_CustomerPortfolio, #div_ddl_SalesRepresentative,#div_ddl_OrderDate,#div_ddl_OrderNumber,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city").css("display", "none");
        $("#AllPartials > div, #div_productName, #div_ProductPortfolio").css("display", "none");
        $("#PartialMIS_SalesOrderDetailsSalesExecutiveWiseList, #div_ddl_SalesRepresentative, #div_ddl_SaleType").css("display", "");
        BindSalesExecutiveWiseDetails();
    }
    else if (ddl_Sales_by == "PO") {
        $("#div_ddl_RegionName, #div_productName, #div_ddl_OrderDate,#div_ddl_SalesRepresentative,#div_ddl_OrderNumber").css("display", "none");
        $("#AllPartials > div, #div_productName, #div_ProductPortfolio, #div_productGrpName, #div_ProductPortfolio").css("display", "none");
        $("#PartialMIS_SalesOrderDetailsPendingOrderWiseList,#div_ddl_CustomerName, #div_ddl_SaleType,#div_CustomerZone, #div_CustomerGroup,#div_ddl_state,#div_ddl_city, #div_CustomerCategory, #div_CustomerPortfolio").css("display", "");
        BindPendingOrderDetails();
    }
    else if (ddl_Sales_by == "POD") {
        $("#div_ddl_RegionName, #div_productName, #div_CustomerCategory, #div_CustomerPortfolio, #div_ddl_SalesRepresentative,#div_ddl_SaleType").css("display", "none");
        $("#AllPartials > div, #div_productName, #div_ProductPortfolio, #div_productGrpName").css("display", "none");
        $("#PartialMIS_SalesOrderDetailsPendingOrderWiseDetailedList,#div_ddl_CustomerName,#div_currencyList,#div_ddl_OrderDate,#div_ddl_OrderNumber").css("display", "");
        BindPendingOrderItemWise();
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
    debugger
    cmn_apply_datatable("#" + convertTo)
}

function BtnSearch() {
    debugger;
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    if (ddl_Sales_by == "O") {
        BindOrderWiseDetails();
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
    else if (ddl_Sales_by == "PO") {
        BindPendingOrderDetails();
    }
    else if (ddl_Sales_by == "POD") {
        BindPendingOrderItemWise();
    }
}

/*---------------------------------- Order Wise Sales Details ------------------------------------------ */
function BindOrderWiseDetails() {
    debugger;
   // var cust_id = $("#ddl_CustomerName").val();
    var cust_id = cmn_getddldataasstring("#ddl_CustomerName");
    //var reg_name = $("#ddl_RegionName").val();
    var reg_name = cmn_getddldataasstring("#ddl_RegionName");
    //var sale_type = $("#ddl_SaleType").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    var product_id = "0";
   // var sale_per = $("#ddl_SalesRepresentative").val();
    var sale_per = cmn_getddldataasstring("#ddl_SalesRepresentative");
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
   // var currId = $("#ddlCurrencylist").val();
    var currId = cmn_getddldataasstring("#ddlCurrencylist");
    var cust_zone = cmn_getddldataasstring("#ddlcustzone");
    var cust_group = cmn_getddldataasstring("#ddlcustgroup");
    var custCat = cmn_getddldataasstring("#CustomerCategory");
    var custPort = cmn_getddldataasstring("#CustomerPortfolio");
    var state = $("#ddl_state").val();
    var city = $("#ddl_city").val();
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetOrderDetailsByFilter",
        data: {
            cust_id: cust_id, reg_name: reg_name, sale_type: sale_type, product_id: product_id, sale_per: sale_per,
            From_dt: From_dt, To_dt: To_dt, currId: currId, ShowAs: ShowAs, custCat: custCat, custPort: custPort, cust_zone: cust_zone, cust_group: cust_group, state: state, city: city
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesOrderDetailsOrderWiseList").html(data);
            replaceDatatablesIds("DataTable1");
            //$("#DataTable1_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#DataTable1_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetOrderDetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        }
    })
}
function OnclickOrderAmount(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var inv_no = currentrow.find("#txtOrd_no").text();
    var inv_dt = currentrow.find("#txtOrd_dt").text();
    var curr_id = currentrow.find("#txtCurr_id").text();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetOrderItemDetails",
        data: {
            inv_no: inv_no, inv_dt: inv_dt, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt,
        },
        success: function (data) {
            debugger;
            $("#InvoiceDetailPopUp").html(data);
            replaceDatatablesIds("Datatable11");
            //$("#Datatable11_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable11_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetOrderItemDetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            $("#ItemDetCurr_Id_CSV").val(curr_id);
            //cmn_apply_datatable("#Datatable11");
        }
    })


}
/*---------------------------------- Order Wise Sales Details End------------------------------------------ */

/*---------------------------------- Customer Wise Sales Details ------------------------------------------ */
function BindCostomerWiseDetails() {
    debugger;
    //var cust_id = $("#ddl_CustomerName").val();
    var cust_id = cmn_getddldataasstring("#ddl_CustomerName");
    //var reg_name = $("#ddl_RegionName").val();
    var reg_name = cmn_getddldataasstring("#ddl_RegionName");
    //var sale_type = $("#ddl_SaleType").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    //var custCat = $("#CustomerCategory").val();
    var custCat = cmn_getddldataasstring("#CustomerCategory");
    //var custPort = $("#CustomerPortfolio").val();
    var custPort = cmn_getddldataasstring("#CustomerPortfolio");
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var currId = $("#ddlCurrencylist").val();
    var currId = cmn_getddldataasstring("#ddlCurrencylist");
    var cust_zone = cmn_getddldataasstring("#ddlcustzone");
    var cust_group = cmn_getddldataasstring("#ddlcustgroup");
    var state = $("#ddl_state").val();
    var city = $("#ddl_city").val();
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    //replaceDatatablesIds("dttbl1");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetOrderDetailsCostomer",
        data: {
            cust_id: cust_id, reg_name: reg_name, custCat: custCat, custPort: custPort, sale_type: sale_type, curr_id: currId,
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs, cust_zone: cust_zone, cust_group: cust_group, state: state, city: city
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesOrderDetailsCustomerWiseList").html(data);
            replaceDatatablesIds("DataTable2");
            //$("#DataTable2_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#DataTable2_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetOrderDetailsCostomerCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
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
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetCW_Sales_Order_Details",
        data: {
            Cust_Id: Cust_Id, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt,
        },
        success: function (data) {
            debugger;
            $("#CustomerInvoiceDetailPopUp").html(data);
            $("#Currency_id").val(curr_id);
            $("#CustomerName").val(Cust_Name);
            $("#hdnCustId").val(Cust_Id)
            $("#Currency").val(Curr_symbal);
            $("#FromDate").val(from_dt);
            $("#ToDate").val(to_dt);
            //replaceDatatablesIds("DatatablePopup2");
            cmn_apply_datatable("#Datatable12");
            //$("#Datatable12_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable12_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetCW_Sales_Order_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        }
    })
}
function Pending_OnclickInvoiceAmt(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var Cust_Id = currentrow.find("#txtCustName").text();
    var Cust_Name = currentrow.find("#cust_name").text();
    var Curr_symbal = currentrow.find("#curr_symbol").text();
    var curr_id = currentrow.find("#txtCurr_id").text();

    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/Pending_Sales_Order_Details",
        data: {
            Cust_Id: Cust_Id, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt,
        },
        success: function (data) {
            debugger;
            $("#CustomerInvoiceDetailPopUp").html(data);
            $("#Currency_id").val(curr_id);
            $("#CustomerName").val(Cust_Name);
            $("#hdnCustId").val(Cust_Id)
            $("#Currency").val(Curr_symbal);
            $("#FromDate").val(from_dt);
            $("#ToDate").val(to_dt);
        }
    })
}
function CW_OnclickInvoiceAmtGetItem(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    if ($("#SalesMISOrderDetail").is(":checked") && ddl_Sales_by == "C") {
        var Cust_Id = currentrow.find("#hdnCustId").text();
        var Cust_Name = currentrow.find("#CustomerName").text();
        var Curr_symbal = currentrow.find("#Currency").text();
        var curr_id = currentrow.find("#Currency_id").text();
    }
    else {
        var Cust_Id = $("#hdnCustId").val();
        var Cust_Name = $("#CustomerName").val();
        var Curr_symbal = $("#Currency").val();
        var curr_id = $("#Currency_id").val();
    }
    var inv_no = currentrow.find("#app_inv_no").text();
    var inv_dt = currentrow.find("#txtInv_dt").text();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetCW_Sales_Invoice_Item_Details",
        data: {
            Cust_Id: Cust_Id, inv_no: inv_no, inv_dt: inv_dt, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt,
        },
        success: function (data) {
            debugger;
            $("#CustomerInvoiceProductDetailPopUp").html(data);

            $("#itmCustomerName").val(Cust_Name);
            $("#itmInvoiceNumber").val(inv_no);
            $("#itmInvoiceDate").val(inv_dt);
            $("#itmCurrency").val(Curr_symbal);
            $("#itmCurrency_id").val(curr_id);
            $("#itmFromDate").val(from_dt);
            $("#itmToDate").val(to_dt);
            //replaceDatatablesIds("DatatablePopup3");
            cmn_apply_datatable("#Datatable13");
            $("#OrdDetCust_Id_CSV").val(Cust_Id);
            //$("#Datatable13_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable13_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetCW_Sales_Invoice_Item_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        }
    })
}
/*---------------------------------- Customer Wise Sales Details End------------------------------------------ */

/*---------------------------------- Product Wise Sales Details ------------------------------------------ */
function BindProductWiseDetails() {
    debugger;
    //var sale_type = $("#ddl_SaleType").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    //var productgrp = $("#ddl_ProductGrpName").val();
    var productgrp = cmn_getddldataasstring("#ddl_ProductGrpName");
    //var productPort = $("#ProductPortfolio").val();
    var productPort = cmn_getddldataasstring("#ProductPortfolio");
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    //var currId = $("#ddlCurrencylist").val();
    var currId = cmn_getddldataasstring("#ddlCurrencylist");
    var itemId = $("#ddlItemsList").val();
   // var itemId = cmn_getddldataasstring("#ddlItemsList");
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    //replaceDatatablesIds("dttbl2");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetOrder_DetailsProductWiseByFilter",
        data: {
            sale_type: sale_type, curr_id: currId, productgrp: productgrp, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt, itemId: itemId, ShowAs: ShowAs
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesOrderDetailsProductWiseList").html(data);
            replaceDatatablesIds("DataTable3");

            //$("#DataTable3_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#DataTable3_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetOrder_DetailsProductWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
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
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetPW_Sales_Order_Details",
        data: {
            Product_Id: Product_Id, sale_type: sale_type, curr_id: curr_id, productgrp: productgrp, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt
        },
        success: function (data) {
            debugger;
            $("#ProductWiseInvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#Datatable14");
            //$("#Datatable14_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable14_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetPW_Sales_Order_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#PWProductName").val(Product_name);
            $("#PWUOM").val(uom)
            $("#PWCurrency").val(currency);
            $("#PWCurrency_id").val(curr_id);
            $("#PWQuantity").val(Quantity);
            $("#PwFromDate").val(From_dt);
            $("#PWToDate").val(To_dt);
            $("#GetPW_Product_Id_CSV").val(Product_Id);//add by SM on 13-01-2025 for CSV
            //replaceDatatablesIds("DatatablePopup4");
        }
    })
}
/*---------------------------------- Product Wise Sales Details End------------------------------------------ */

/*---------------------------------- Product Group Wise Sales Details ------------------------------------------ */
function BindProductGroupWiseDetails() {
    debugger;
    //var sale_type = $("#ddl_SaleType").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
   // var productgrp = $("#ddl_ProductGrpName").val();
    var productgrp = cmn_getddldataasstring("#ddl_ProductGrpName");
   // var productPort = $("#ProductPortfolio").val();
    var productPort = cmn_getddldataasstring("#ProductPortfolio");
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    //var currId = $("#ddlCurrencylist").val();
    var currId = cmn_getddldataasstring("#ddlCurrencylist");
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    //replaceDatatablesIds("dttbl3");

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetOrder_DetailsProductGroupWiseByFilter",
        data: {
            sale_type: sale_type, curr_id: currId, productgrp: productgrp, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesOrderDetailsProductGroupWiseList").html(data);
            replaceDatatablesIds("DataTable8");

            //$("#DataTable8_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#DataTable8_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetOrder_DetailsProductGroupWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
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
    var sale_type = $("#ddl_SaleType").val();
    // var productgrp = $("#ddl_ProductGrpName").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetPGW_Sales_Invoice_product_Details",
        data: {
            sale_type: sale_type, curr_id: curr_id, productgrp: grp_Id, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt
        },
        success: function (data) {
            debugger;
            $("#ProductGroupWiseProductDetailPopUp").html(data);
            cmn_apply_datatable("#Datatable15");
            //$("#Datatable15_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable15_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetPGW_Sales_Invoice_product_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#PGWProductgrpId").val(grp_Id);
            $("#PGWGroupName").val(grp_name);
            $("#PGWCurrency").val(currency);
            $("#PGWCurrency_id").val(curr_id);
            $("#PGWFromDate").val(From_dt);
            $("#PGWToDate").val(To_dt);
            //replaceDatatablesIds("DatatablePopup5");
        }
    })
}
function PGW_OnclickPopUpInvoiceAmt(e) {

    debugger;
    var currentrow = $(e.target).closest("tr");
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    var Product_Id = currentrow.find("#Hdn_item_id").val();
    var Product_name = currentrow.find("#item_nam").text().trim();
    var uom = currentrow.find("#uom_alia").text().trim();
    if ($("#SalesMISOrderDetail").is(":checked") && ddl_Sales_by == "G") {
        var currency = currentrow.find("#_curr_symbol").text().trim();
        var curr_id = currentrow.find("#txtCurr_id").text().trim();
    }
    else {
        var currency = $("#PGWCurrency").val().trim();
        var curr_id = $("#PGWCurrency_id").val().trim();
    }
    var Quantity = currentrow.find("#item_qt").text().trim();
    var sale_type = $("#ddl_SaleType").val();
    var productgrp = $("#PGWProductgrpId").val();
    var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetPGW_Sales_Invoice_Details",
        data: {
            Product_Id: Product_Id, sale_type: sale_type, curr_id: curr_id, productgrp: productgrp, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt
        },
        success: function (data) {
            debugger;
            $("#ProductGroupWiseInvoiceDetailPopUp").html(data);
            cmn_apply_datatable("#Datatable16");
            //$("#Datatable16_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable16_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetPGW_Sales_Invoice_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#PGWP_ProductName").val(Product_name);
            $("#PGWP_UOM").val(uom)
            $("#PGWP_Currency").val(currency);
            $("#PGWP_Quantity").val(Quantity);
            $("#PGWP_FromDate").val(From_dt);
            $("#PGWP_ToDate").val(To_dt);
            $("#GetPGW_Product_Id_CSV").val(Product_Id);//add by SM on 13-01-2025 for CSV
            $("#GetPGW_curr_id_CSV").val(curr_id);//add by SM on 13-01-2025 for CSV
            // replaceDatatablesIds("DatatablePopup6");
        }
    })

}
/*---------------------------------- Product Group Wise Sales Details End------------------------------------------ */

/*---------------------------------- Region Wise Sales Details ------------------------------------------ */
function BindRegionWiseDetails() {
    debugger;

    //var sale_type = $("#ddl_SaleType").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    //var RegionName = $("#ddl_RegionName").val();
    var RegionName = cmn_getddldataasstring("#ddl_RegionName");
    //var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    //var currId = $("#ddlCurrencylist").val();
    var currId = cmn_getddldataasstring("#ddlCurrencylist");
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    //replaceDatatablesIds("dttbl4");

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetSales_DetailsRegionWiseByFilter",
        data: {
            sale_type: sale_type, curr_id: currId, RegionName: RegionName,
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesOrderDetailsRegionWiseList").html(data);
            replaceDatatablesIds("DataTable7");
            //$("#DataTable7_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#DataTable7_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetSales_DetailsRegionWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
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
    var To_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetRegionWise_Customer_Details",
        data: {
            sale_type: sale_type, curr_id: curr_id, Region_Id: Region_Id, productPort: productPort,
            From_dt: From_dt, To_dt: To_dt
        },
        success: function (data) {
            debugger;
            $("#RegionWiseCustomerDetailPopUp").html(data);
            cmn_apply_datatable("#Datatable17");
            //$("#Datatable17_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable17_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetRegionWise_Customer_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#HdnRegionId").val(Region_Id);
            $("#RW_RegionName").val(Region_name);
            $("#RW_Currency1").val(currency);
            $("#RWCurrency_id").val(curr_id);
            $("#RW_FromDate").val(From_dt);
            $("#RW_ToDate").val(To_dt);
            //replaceDatatablesIds("DatatablePopup7");
        }
    });
}
function R_OnclickPopUpInvoiceAmt(e) {

    debugger;
    var currentrow = $(e.target).closest("tr");
    var Cust_Id = currentrow.find("#hdnCustomerId").text();
    var Cust_name = currentrow.find("#custmar_name").text().trim();
    //var uom = currentrow.find("td:eq(2)").text().trim();
    var currency = currentrow.find("#currancy_symbol").text().trim();
    var curr_id = $("#RWCurrency_id").val().trim();
    // var Quantity = currentrow.find("td:eq(4)").text().trim();
    var sale_type = $("#ddl_SaleType").val();
    var regionId = $("#HdnRegionId").val();
    var regionName = $("#RW_RegionName").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetRegionWise_Sales_Invoice_Details",
        data: {
            Cust_Id: Cust_Id, sale_type: sale_type, curr_id: curr_id, regionId: regionId,
            From_dt: From_dt, To_dt: To_dt
        },
        success: function (data) {
            debugger;
            $("#RegionWiseInvoiceSummaryPopUp").html(data);
            cmn_apply_datatable("#Datatable18");

            //$("#Datatable18_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable18_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetRegionWise_Sales_Invoice_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#RWP_RegionName").val(regionName);
            $("#RWP_hdnRegionId").val(regionId);
            $("#RWP_CustomerName").val(Cust_name);
            $("#RWP_hdnCustomerId").val(Cust_Id);
            $("#RWP_Currency").val(currency);
            $("#PWPCurrency_id").val(curr_id);
            $("#RWP_FromDate").val(From_dt);
            $("#RWP_ToDate").val(To_dt);
           //replaceDatatablesIds("DatatablePopup8");
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
    //var FinalDetail = [];
    //FinalDetail.push({
    //    Cust_Id: Cust_Id, sale_type: sale_type, regionId: regionId, invoice_no: invoice_no, invoice_dt: invoice_dt,
    //    From_dt: From_dt, To_dt: To_dt });

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetRegionWise_Sales_Invoice_Product_Details",
        data: //JSON.stringify({ OrderDetail: FinalDetail}),
        {
            Cust_Id: Cust_Id, sale_type: sale_type, curr_id: curr_id, regionId: regionId, invoice_no: invoice_no, invoice_dt: invoice_dt,
            From_dt: From_dt, To_dt: To_dt
        },
        success: function (data) {
            debugger;
            $("#RegionWiseInvoiceProductDetailPopUp").html(data);
            cmn_apply_datatable("#Datatable19");

            //$("#Datatable19_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable19_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetRegionWise_Sales_Invoice_Product_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#RWPP_CustomerName").val(Cust_name);
            $("#RWPP_InvoiceNumber").val(invoice_no);
            $("#RWPP_InvoiceDate").val(invoice_dt);
            $("#RWPP_Currency").val(currency);
            //replaceDatatablesIds("DatatablePopup9");
        }
    })

}
/*---------------------------------- Region Wise Sales Details End------------------------------------------ */

/*---------------------------------- Sales Executive Wise Sales Details ------------------------------------------ */
function BindSalesExecutiveWiseDetails() {
    debugger;

    //var sale_type = $("#ddl_SaleType").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    //var SaleRepresentive = $("#ddl_SalesRepresentative").val();
    var SaleRepresentive = cmn_getddldataasstring("#ddl_SalesRepresentative");
    //var productPort = $("#ProductPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var currId = $("#ddlCurrencylist").val();
    var currId = cmn_getddldataasstring("#ddlCurrencylist");
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    //replaceDatatablesIds("dttbl5");

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetSales_DetailsSalePersonWiseByFilter",
        data: {
            sale_type: sale_type, curr_id: currId, sale_per: SaleRepresentive,
            From_dt: From_dt, To_dt: To_dt, ShowAs: ShowAs
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesOrderDetailsSalesExecutiveWiseList").html(data);
            replaceDatatablesIds("DataTable5");

            //$("#DataTable5_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#DataTable5_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetSales_DetailsSalePersonWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
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
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetSalePersonWise_Customer_Details",
        data: {
            sale_type: sale_type, curr_id: curr_id, sale_per: sale_per_id,
            From_dt: From_dt, To_dt: To_dt
        },
        success: function (data) {
            debugger;
            $("#SalesExecutiveWiseCustomerDetailPopUp").html(data);
            cmn_apply_datatable("#Datatable20");

            //$("#Datatable20_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable20_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetSalePersonWise_Customer_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#sale_Exc_id").val(sale_per_id);
            $("#SalesExecutiveName").val(sale_per);
            $("#SaleEx_Currency").val(currency);
            $("#SaleEx_Currency_id").val(curr_id);
            $("#SaleEx_FromDate").val(From_dt);
            $("#SaleEx_ToDate").val(To_dt);
            //replaceDatatablesIds("DatatablePopup10");
        }
    });
}
function SaleExctv_OnclickPopUpInvoiceAmt(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var sale_per_id = $("#sale_Exc_id").val().trim();
    var sale_per = $("#SalesExecutiveName").val();
    var cust_id = currentrow.find("#custmer_id").text().trim();
    var cust_name = currentrow.find("#custmer_nm").text();
    var currency = currentrow.find("#currsymbol").text();
    var curr_id = currentrow.find("#SaleEx_Currency_id").text();
    var sale_type = $("#ddl_SaleType").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetSalePersonWise_Customer_Invoice_Details",
        data: {
            sale_type: sale_type, curr_id: curr_id, sale_per: sale_per_id, cust_id: cust_id,
            From_dt: From_dt, To_dt: To_dt
        },
        success: function (data) {
            debugger;
            $("#SalesExecutiveWiseInvoiceSummaryPopUp").html(data);
            cmn_apply_datatable("#Datatable21");

            //$("#Datatable21_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable21_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetSalePersonWise_Customer_Invoice_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#sale_Exc_id").val(sale_per_id);
            $("#P_SalesExecutiveName").val(sale_per);
            $("#SE_CustomerName").val(cust_name)
            $("#SE_Currency").val(currency);
            $("#SE_Currency_id").val(curr_id);
            $("#SE_FromDate").val(From_dt);
            $("#SE_ToDate").val(To_dt);
            $("#SalePersonCust_Id_CSV").val(cust_id);//add by SM on 13-01-2025 for CSV
            //replaceDatatablesIds("DatatablePopup11");
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
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetSalePersonWise_Customer_Invoice_Item_Details",
        data: {
            inv_no: inv_no, inv_dt: inv_dt, curr_id: curr_id,
            From_dt: From_dt, To_dt: To_dt
        },
        success: function (data) {
            debugger;
            $("#SalesExecutiveWiseProductDetailPopUp").html(data);
            cmn_apply_datatable("#Datatable22");

            //$("#Datatable22_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable22_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetSalePersonWise_Customer_Invoice_Item_DetailsCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#SE_Item_CustomerName").val(cust_name)
            $("#SE_Item_Currency").val(currency);
            $("#SE_Item_InvoiceNumber").val(inv_no);
            $("#SE_Item_InvoiceDate").val(inv_dt);
            //replaceDatatablesIds("DatatablePopup12");
        }
    });
}
/*---------------------------------- Sales Executive Wise Sales Details ------------------------------------------ */


/*---------------------------------- Pending Orders Details ------------------------------------------ */
function BindPendingOrderDetails() {
    debugger;
    //var sale_type = $("#ddl_SaleType").val();
    var sale_type = cmn_getddldataasstring("#ddl_SaleType");
    var product_id = "0";
    //var sale_per = $("#ddl_SalesRepresentative").val();
    var sale_per = cmn_getddldataasstring("#ddl_SalesRepresentative");
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    //var currId = $("#ddlCurrencylist").val();
    var currId = cmn_getddldataasstring("#ddlCurrencylist");
    //var custId = $("#ddl_CustomerName").val();
    var custId = cmn_getddldataasstring("#ddl_CustomerName");
    var cust_zone = cmn_getddldataasstring("#ddlcustzone");
    var cust_group = cmn_getddldataasstring("#ddlcustgroup");
    var custCat = cmn_getddldataasstring("#CustomerCategory");
    var custPort = cmn_getddldataasstring("#CustomerPortfolio");
    var state = $("#ddl_state").val();
    var city = $("#ddl_city").val();
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    //replaceDatatablesIds("dttbl6");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetPendingOrdersDetail",
        data: {
            orderType: sale_type, salesRepresentatives: sale_per, fromDate: From_dt,
            toDate: To_dt, currId: currId, custId: custId, ShowAs: ShowAs, custCat: custCat, custPort: custPort, cust_zone: cust_zone, cust_group: cust_group, state: state, city: city
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesOrderDetailsPendingOrderWiseList").html(data);
            replaceDatatablesIds("DataTable6");

            //$("#DataTable6_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#DataTable6_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetPendingOrdersDetailCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        }
    })
}
function BindPendingOrderItemWise() {
    debugger;
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
   // var currId = $("#ddlCurrencylist").val();
    var currId = cmn_getddldataasstring("#ddlCurrencylist");
   // var custId = $("#ddl_CustomerName").val();
    var custId = cmn_getddldataasstring("#ddl_CustomerName");
    var soNo = $("#ddlsono").val();
    var soDate = $("#ddlsodt").val();
    if (soDate == "---All---") {
        soDate = "0";
    }
    if (soNo == "---All---") {
        soNo = "0";
    }
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    //replaceDatatablesIds("dttbl6");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetPendingOrderDetails",
        data: {
            custId: custId, currId: currId, orderNo: soNo, orderDate: soDate,
            fromDate: From_dt, toDate: To_dt, ShowAs: ShowAs
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_SalesOrderDetailsPendingOrderWiseDetailedList").html(data);
            replaceDatatablesIds("DataTable4");
        }
    })
}
function CW_OnclickPendingInvoiceAmt(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var Cust_Id = currentrow.find("#txtCustName").text();
    var Cust_Name = currentrow.find("#cust_name").text();
    var Curr_symbal = currentrow.find("#curr_symbol").text();
    var curr_id = currentrow.find("#txtCurr_id").text();

    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/Pending_Sales_Order_Details",
        data: {
            Cust_Id: Cust_Id, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt,
        },
        success: function (data) {
            debugger;
            $("#CustomerPendingInvoiceDetailPopUp").html(data);
            $("#Currency_id").val(curr_id);
            $("#CustomerName").val(Cust_Name);
            $("#hdnCustId").val(Cust_Id)
            $("#Currency").val(Curr_symbal);
            $("#FromDate").val(from_dt);
            $("#ToDate").val(to_dt);
            //replaceDatatablesIds("DatatablePopup13");
            cmn_apply_datatable("#Datatable23");
        }
    })
}
function PendingOrderItemWise(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");

    var txtsono = currentrow.find("#txtsono").text();
    var txtsodt = currentrow.find("#txtsodt").text();
    var Cust_Name = currentrow.find("#cust_name").text();
    var curr = currentrow.find("#curr_symbol").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetPendingOrdersItemWiseDetail",
        data: {
            so_no: txtsono, so_dt: txtsodt,
        },
        success: function (data) {
            debugger;
            $("#popupPendingOrderDetail").html(data);
            cmn_apply_datatable("#Datatable24");

            //$("#Datatable24_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#Datatable24_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetPendingOrdersItemWiseDetailCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

            $("#ordNumber").val(txtsono);
            $("#ordDate").val(txtsodt);
            $("#custName").val(Cust_Name);
            $("#Currency12").val(curr);
            //replaceDatatablesIds("DatatablePopup14");
            
        }
    })
}
function CW_OnclickPendingInvoiceAmtGetItem(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var Cust_Id = $("#hdnCustId").val();
    var Cust_Name = $("#CustomerName").val();
    var Curr_symbal = $("#Currency").val();
    var curr_id = $("#Currency_id").val();
    var inv_no = currentrow.find("#app_inv_no").text();
    var inv_dt = currentrow.find("#txtInv_dt").text();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/GetCW_Pending_Sales_Invoice_Item_Details",
        data: {
            Cust_Id: Cust_Id, inv_no: inv_no, inv_dt: inv_dt, curr_id: curr_id, from_dt: from_dt, to_dt: to_dt,
        },
        success: function (data) {
            debugger;
            $("#CustomerInvoiceProductDetailPopUp").html(data);
            $("#itmCustomerName").val(Cust_Name);
            $("#itmInvoiceNumber").val(inv_no);
            $("#itmInvoiceDate").val(inv_dt);
            $("#itmCurrency").val(Curr_symbal);
            $("#itmCurrency_id").val(curr_id);
            $("#itmFromDate").val(from_dt);
            $("#itmToDate").val(to_dt);
        }
    })
}
/*---------------------------------- Pending Orders Details End ------------------------------------------ */

function OnclickDeliverySchedule(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var ddl_Sales_by = $("#ddl_Sales_by").val();
    if (ddl_Sales_by == "PO" || ddl_Sales_by == "POD") {
        if ($("#SalesMISOrderDetail").is(":checked")) {
            var Sono = currentrow.find("#hdnappsono").val();
        }
        else {
            var Sono = $("#ordNumber").val();
        }
    }
    else if ($("#SalesMISOrderDetail").is(":checked") && (ddl_Sales_by == "O" || ddl_Sales_by == "R" || ddl_Sales_by == "S")) {
        var Sono = currentrow.find("#td_app_so_no").text();
    }
    else {
        var Sono = $("#InvoiceNumber").val();
    }
    var itemId = currentrow.find("#Hdn_item_id").val();
    // var OrderType = $("#OrderType").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISSalesOrderDetail/DeliveryShudule",
        data: {
            orderNo: Sono, itemId: itemId
        },
        success: function (data) {
            debugger;
            $("#ddldeliverysch").html(data);
        }
    });
}
function GetOrderDetailsCSV() {
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var product_id = "0";
    var sale_per = $("#ddl_SalesRepresentative").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#ddlCurrencylist").val();
    var productGrp = "";
    var productPort = "";
    var custCat = "";
    var custPort = "";
    var inv_no = "";
    var inv_dt = "";
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "OrderWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetOrderItemDetailsCSV() {
    var cust_id = "";
    var reg_name = "";
    var sale_type = "";
    var product_id = "0";
    var sale_per ="";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#ItemDetCurr_Id_CSV").val();
    var productGrp = "";
    var productPort = "";
    var custCat = "";
    var custPort = "";
    var inv_no = $("#InvoiceNumber").val();
    var inv_dt = $("#InvoiceDate").val();
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "ItemDetails";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetOrderDetailsCostomerCSV() {
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var custCat = $("#CustomerCategory").val();
    var custPort = $("#CustomerPortfolio").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#ddlCurrencylist").val();
    var product_id = "0";
    var sale_per = "";
    var productGrp = "";
    var productPort = "";
    var inv_no = "";
    var inv_dt = "";
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "CustomerWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetCW_Sales_Order_DetailsCSV() {
    var cust_id = $("#hdnCustId").val();
    var reg_name = "";
    var sale_type = "";
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#Currency_id").val();
    var product_id = "0";
    var sale_per = "";
    var productGrp = "";
    var productPort = "";
    var inv_no = "";
    var inv_dt = "";
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "CW_Ord_Details";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetCW_Sales_Invoice_Item_DetailsCSV() {
    var cust_id = $("#OrdDetCust_Id_CSV").val();
    var reg_name = "";
    var sale_type = "";
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#itmCurrency_id").val();
    var product_id = "0";
    var sale_per = "";
    var productGrp = "";
    var productPort = "";
    var inv_no = $("#itmInvoiceNumber").val();
    var inv_dt = $("#itmInvoiceDate").val();
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "CW_Ord_Item_Details";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetOrder_DetailsProductWiseCSV() {
    var cust_id = "";
    var reg_name = "";
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#ddlCurrencylist").val();
    var product_id = $("#ddlItemsList").val();
    var sale_per = "";
    var productGrp = $("#ddl_ProductGrpName").val();
    var productPort = $("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "ProductWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetPW_Sales_Order_DetailsCSV() {
    var cust_id = "";
    var reg_name = "";
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#PWCurrency_id").val();
    var product_id = $("#GetPW_Product_Id_CSV").val();
    var sale_per = "";
    var productGrp = $("#ddl_ProductGrpName").val();
    var productPort = $("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs="";
    var DataShow = "ODProductWiseInvoce";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetOrder_DetailsProductGroupWiseCSV() {
    var cust_id = "";
    var reg_name = "";
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#ddlCurrencylist").val();
    var product_id = "";
    var sale_per = "";
    var productGrp = $("#ddl_ProductGrpName").val();
    var productPort = $("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "ProductGroupWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetPGW_Sales_Invoice_product_DetailsCSV() {
    var cust_id = "";
    var reg_name = "";
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#PGWCurrency_id").val();
    var product_id = "";
    var sale_per = "";
    var productGrp = $("#PGWProductgrpId").val();
    var productPort = $("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs = "";
    var DataShow = "ODProductWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetPGW_Sales_Invoice_DetailsCSV() {
    var cust_id = "";
    var reg_name = "";
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#GetPGW_curr_id_CSV").val();;
    var product_id = $("#GetPGW_Product_Id_CSV").val();
    var sale_per = "";
    var productGrp = $("#PGWProductgrpId").val();
    var productPort = $("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs = "";
    var DataShow = "ODProductWiseInvoce";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetSales_DetailsRegionWiseCSV() {
    var cust_id = "";
    var reg_name = $("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#ddlCurrencylist").val();
    var product_id = "";
    var sale_per = "";
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = "";//$("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "RegionWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetRegionWise_Customer_DetailsCSV() {
    var cust_id = "";
    var reg_name = $("#HdnRegionId").val();
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#RWCurrency_id").val();
    var product_id = "";
    var sale_per = "";
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = $("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs="";
    var DataShow = "SOCostomerWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetRegionWise_Sales_Invoice_DetailsCSV() {
    var cust_id = $("#RWP_hdnCustomerId").val();
    var reg_name = $("#HdnRegionId").val();
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#RWCurrency_id").val().trim();
    var product_id = "";
    var sale_per = "";
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = "";//$("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs = "";
    var DataShow = "CW_Ord_Details";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetRegionWise_Sales_Invoice_Product_DetailsCSV() {
    var cust_id = $("#RWP_hdnCustomerId").val();
    var reg_name = $("#HdnRegionId").val();
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#PWPCurrency_id").val().trim();
    var product_id = "";
    var sale_per = "";
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = "";//$("#ProductPortfolio").val();
    var inv_no = $("#RWPP_InvoiceNumber").val();
    var inv_dt = $("#RWPP_InvoiceDate").val();
    var ShowAs = "";
    var DataShow = "CW_Ord_Item_Details";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetSales_DetailsSalePersonWiseCSV() {
    var cust_id = "";
    var reg_name = "";//$("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#ddlCurrencylist").val();
    var product_id = "";
    var sale_per = $("#ddl_SalesRepresentative").val();
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = "";//$("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "SalePersonWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetSalePersonWise_Customer_DetailsCSV() {
    var cust_id = "";
    var reg_name = "";//$("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#SaleEx_Currency_id").val();
    var product_id = "";
    var sale_per = $("#sale_Exc_id").val();
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = "";//$("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs="";
    var DataShow = "SOCostomerWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetSalePersonWise_Customer_Invoice_DetailsCSV() {
    var cust_id = $("#SalePersonCust_Id_CSV").val();
    var reg_name = "";//$("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#SE_Currency_id").val();
    var product_id = "";
    var sale_per = $("#sale_Exc_id").val();
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = "";//$("#ProductPortfolio").val();
    var inv_no = "";
    var inv_dt = "";
    var ShowAs="";
    var DataShow = "CW_Ord_Details";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetSalePersonWise_Customer_Invoice_Item_DetailsCSV() {
    var cust_id = "";//$("#SalePersonCust_Id_CSV").val();
    var reg_name = "";//$("#ddl_RegionName").val();
    var sale_type = "";//$("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#SE_Currency_id").val();
    var product_id = "";
    var sale_per = "";//$("#sale_Exc_id").val();
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = "";//$("#ProductPortfolio").val();
    var inv_no = $("#SE_Item_InvoiceNumber").val();
    var inv_dt = $("#SE_Item_InvoiceDate").val();
    var ShowAs = "";
    var DataShow = "CW_Ord_Item_Details";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetPendingOrdersDetailCSV() {
    var cust_id = $("#ddl_CustomerName").val();
    var reg_name = "";//$("#ddl_RegionName").val();
    var sale_type = $("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var curr_id = $("#ddlCurrencylist").val();
    var product_id = "";
    var sale_per = $("#ddl_SalesRepresentative").val();
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = "";//$("#ProductPortfolio").val();
    var inv_no = "";//$("#SE_Item_InvoiceNumber").val();
    var inv_dt = "";//$("#SE_Item_InvoiceDate").val();
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "PendingOrdersWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function GetPendingOrdersItemWiseDetailCSV() {
    var cust_id = "";//$("#ddl_CustomerName").val();
    var reg_name = "";//$("#ddl_RegionName").val();
    var sale_type = "";//$("#ddl_SaleType").val();
    var custCat = "";
    var custPort = "";
    var From_dt = "";//$("#txtFromdate").val();
    var To_dt = "";//$("#txtTodate").val();
    var curr_id = "";//$("#ddlCurrencylist").val();
    var product_id = "";
    var sale_per = "";//$("#ddl_SalesRepresentative").val();
    var productGrp = "";//$("#ddl_ProductGrpName").val();
    var productPort = "";//$("#ProductPortfolio").val();
    var inv_no = $("#ordNumber").val();
    var inv_dt = $("#ordDate").val();
    var ShowAs;
    if ($("#SalesMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var DataShow = "PendingOrdersItemWise";//ItemDetails
    window.location.href = "/ApplicationLayer/MISSalesOrderDetail/OrderDetailExporttoExcelDt?cust_id=" + cust_id + "&reg_name=" + reg_name + "&DataShow=" + DataShow + "&sale_type=" + sale_type + "&product_id=" + product_id + "&sale_per=" + sale_per + "&From_dt=" + From_dt + "&To_dt=" + To_dt + "&ShowAs=" + ShowAs + "&curr_id=" + curr_id + "&productGrp=" + productGrp + "&productPort=" + productPort + "&custCat=" + custCat + "&CustPort=" + custPort + "&inv_no=" + inv_no + "&inv_dt=" + inv_dt;
}
function BindCityList() {
    debugger;
    var state_id = $("#ddl_state").val()
    $("#ddl_city").append("<option value='0'>---Select---</option>");
    $("#ddl_city").select2({
        ajax: {
            url: "/ApplicationLayer/MISSalesOrderDetail/BindCityListdata",
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
            url: "/ApplicationLayer/MISSalesOrderDetail/BindStateListData",
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
function onchangeState() {
    BindCityList();
}