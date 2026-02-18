$(document).ready(function () {
    debugger
    BindCustomerList();
    BindCityList();
    BindStateList();
    $(document).ready(function () {
        $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" onclick="CollectioDetailPDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')
    });

    Cmn_initializeMultiselect(
        ["#CustomerCategory", "#CustomerPortfolio","#ddlSalesPerson", "#ddl_RegionName", "#ddl_customerZone", "#ddl_CustomerGroup"]
    );
    Cmn_initializeMultiselect_ddl_br(["#ddl_branch"]);
});
function BindCustomerList() {
    var Branch = sessionStorage.getItem("BranchID");
    $.ajax({
        url: "/ApplicationLayer/SalesDetail/GetAutoCompleteSearchCustList",
        data: {},
        success: function (data, params) {

            if (data == 'ErrorPage') {
                LSO_ErrorPage();
                return false;
            }
            var s = '';
            $.map(data, function (val) {
                if (val.Name.trim() != "---All---") {
                    s += '<option value="' + val.ID + '">' + val.Name + '</option>';
                }
            });
            $("#ddl_CustomerName").html(s);
            // $('#ddl_CustomerName').multiselect('destroy');
            Cmn_initializeMultiselect(["#ddl_CustomerName"]);
        }
    });
}
function BindCityList() {
    debugger;
    var state_id = $("#ddl_state").val();
    $("#ddl_city").append("<option value='0'>---Select---</option>");
    $("#ddl_city").select2({
        ajax: {
            url: "/ApplicationLayer/AccountReceivable/BindCityListdata",
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
                if (Fdata == 1) {
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
            url: "/ApplicationLayer/AccountReceivable/BindStateListData",
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
                        return { id: val.ID.split(",")[0], text: val.Name, UOM: val.ID.split(",")[1] };
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
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function onchangeStateName() {
    BindCityList()
}
function onchangeShowAs() {
    debugger;
    SearchAgingDetail()
}
function onclickReceivableType() {
    if ($("#ReceivableTypeA").is(":checked")) {
        SearchAgingDetail();
    }
    if ($("#ReceivableTypeO").is(":checked")) {
        SearchAgingDetail();
    }
    if ($("#ReceivableTypeU").is(":checked")) {
        SearchAgingDetail();
    }
}
function SearchAgingDetail() {
    debugger;
    try {
        var Cust_id = cmn_getddldataasstring("#ddl_CustomerName"); 
        var Cat_id = cmn_getddldataasstring("#CustomerCategory"); 
        var Prf_id = cmn_getddldataasstring("#CustomerPortfolio"); 
        var Reg_id = cmn_getddldataasstring("#ddl_RegionName"); 
        var brid_list = cmn_getddldataasstring("#ddl_branch");
        var sales_per = cmn_getddldataasstring("#ddlSalesPerson");
        var customerZone = cmn_getddldataasstring("#ddl_customerZone");
        var CustomerGroup = cmn_getddldataasstring("#ddl_CustomerGroup");
        var state_id = $('#ddl_state').val();
        var city_id = $('#ddl_city').val();
        var AsDate = $("#AsDate").val();

        var ReceivableType = "A";
        if ($("#ReceivableTypeA").is(":checked")) {
            ReceivableType = "A";
        }
        if ($("#ReceivableTypeO").is(":checked")) {
            ReceivableType = "O";
        }
        if ($("#ReceivableTypeU").is(":checked")) {
            ReceivableType = "U";
        }
        var ReportType = "S";
        if ($("#ProcurementMISOrderSummary").is(":checked")) {
            ReportType = "S";
            sales_per = "0";
            $("#SlsPersIdDiv").css("display", "none");
        }
        if ($("#ProcurementMISOrderDetail").is(":checked")) {
            ReportType = "D";
            $("#SlsPersIdDiv").css("display", "block");
        }
        $("#HdnReportType").val(ReportType);

        //var br_list = $("#ddl_branch").val();

        //var brid_list = cmn_multibranchlist(br_list);
        var brid_list = cmn_getddldataasstring("#ddl_branch");
        if (brid_list == '0' || brid_list == "" || brid_list == null) {
            return false;
        }
        var includeZero = "N";
        if ($("#IncludeZeroStock").is(":checked")) {
            includeZero = "Y";
        }
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/MISCollectionDetail/SearchCollectionDetail",
            data: {
                Cust_id: Cust_id,
                Cat_id: Cat_id,
                Prf_id: Prf_id,
                Reg_id: Reg_id,
                AsDate: AsDate,
                ReceivableType: ReceivableType,
                ReportType: ReportType,
                brlist: brid_list,
                customerZone: customerZone,
                CustomerGroup: CustomerGroup,
                state_id: state_id,
                city_id: city_id,
                includeZero: includeZero,
                sales_per: sales_per
            },
            success: function (data) {
                debugger;
                $('#DivCollectiontable').html(data);
                $("a.btn.btn-default.buttons-print.btn-sm").remove();
                $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" onclick="CollectioDetailPDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')

                $("#DivCustAgingtable").css("display", "block")
                hideLoader();/*Add by Hina sharma on 19-11-2024 */
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
function CollectioDetailPDF() {
    $("#hdnPDFPrint").val("Print");
    $('form').submit();
}
function CheckedIncludeZeroStock() {
    SearchAgingDetail();
}
function GetSalesAmtDetails(e) {
    debugger;
    //var Basis = "I";
    var AsDate = $("#AsDate").val();
    var CurrentRow = $(e.target).closest("tr");
    var CustId = CurrentRow.find("#cust_id").text();

    var Cust_name = CurrentRow.find("#cust_name").text();
    var Curr = CurrentRow.find("#curr_symbol").text();
    var CurrId = CurrentRow.find("#curr_id").text();
   

    var ReceivableType = "A";
    if ($("#ReceivableTypeA").is(":checked")) {
        ReceivableType = "A";
    }
    if ($("#ReceivableTypeO").is(":checked")) {
        ReceivableType = "O";
    }
    if ($("#ReceivableTypeU").is(":checked")) {
        ReceivableType = "U";
    }
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    var inv_no = CurrentRow.find("#Inv_no").text();
    var inv_dt = CurrentRow.find("#Inv_dt").text();
    if (ReportType == "S") {
        var InvDate = "";
    }
    else {
        var inv_date = inv_dt.split("-");
        var InvDate = inv_date[2] + '-' + inv_date[1] + '-' + inv_date[0];
    }
    var brid_list = cmn_getddldataasstring("#ddl_branch");

    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    var includeZero = "N";
    if ($("#IncludeZeroStock").is(":checked")) {
        includeZero = "Y";
    }
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/MISCollectionDetail/GetSalesAmtDetails",
            data: {
                Cust_id: CustId,
                AsDate: AsDate,
                CurrId: CurrId,
                ReceivableType: ReceivableType,
                ReportType: ReportType,
                inv_no: inv_no,
                inv_dt: InvDate,
                brlist: brid_list,
                includeZero: includeZero
            },
            success: function (data) {
                debugger;
                $('#SaleAmountDetailPopup').html(data);
                $("#CustomerName").val(Cust_name);
                $("#Currency").val(Curr);
                $("#HdnCustomerNameForPaidAmt").val(CustId);
                cmn_apply_datatable("#tbl_SalesDetails");
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
function GetPaidAmtDetails(e) {
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var Cust_name = CurrentRow.find("#cust_name").text();
    var Curr = CurrentRow.find("#curr_symbol").text();
    var Cust_id = CurrentRow.find("#cust_id").text();
    var curr_id = CurrentRow.find("#curr_id").text();
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    var AsDate = $("#AsDate").val();

    var ReceivableType = "A";
    if ($("#ReceivableTypeA").is(":checked")) {
        ReceivableType = "A";
    }
    if ($("#ReceivableTypeO").is(":checked")) {
        ReceivableType = "O";
    }
    if ($("#ReceivableTypeU").is(":checked")) {
        ReceivableType = "U";
    }
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    $("#HdnReportType").val(ReportType);
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    var includeZero = "N";
    if ($("#IncludeZeroStock").is(":checked")) {
        includeZero = "Y";
    }
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/MISCollectionDetail/SearchPaidAmountDetail",
            data: {
                Cust_id: Cust_id,
                curr_id: curr_id,
                AsDate: AsDate,
                ReceivableType: ReceivableType,
                ReportType: ReportType,
                brlist: brid_list,
                includeZero: includeZero,
            },
            success: function (data) {
                debugger;
                $('#PartialPaymentDetail').html(data);
                $("#CustomerNamePaidDetail").val(Cust_name);
                $("#CurrencyPaidDetail").val(Curr);
                cmn_apply_datatable("#tbl_PaidAmountDetails");
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
function OnclickInvoiceAmount(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var inv_no = currentrow.find("#Inv_no").text();
    var inv_dt = currentrow.find("#Inv_dt").text();
    var curr_id = currentrow.find("#txtcurr_id").text();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    var Flag = "ItemDetails";/*add by Hina Sharma on 27-06-2025*/
    var brid_list = currentrow.find("#txtbr_id").text();
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
function AccRec_GetPopupInoviceDetailsPDF(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Invoiceno = clickedrow.find("#Inv_No").text().trim();
    var InvDate = clickedrow.find("#InvDate").text();
    var Invno = Invoiceno.split("/");
    var INo = Invno[3];
    var code = INo.split("0");
    var Doccode = code[0];
    if (Doccode == "DSI" || Doccode == "CIN" || Doccode == "ESI" || Doccode == "SSI" || Doccode == "SCI" || Doccode == "SJI" || Doccode == "DSI" || Doccode == "SSI" || Doccode == "IPI" || Doccode == "PDI")
        window.location.href = "/ApplicationLayer/AccountReceivable/GenerateInvoiceDetails?invNo=" + Invoiceno + "&invDate=" + InvDate + "&dataType=" + Doccode;
}
function OnclickPaidAmtDetailIcon(e, flag) {
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var InVNo = CurrentRow.find("#Hdn_InvNo").text();
    var InvDate = CurrentRow.find("#td_InvDt").text();
    var InvDt = CurrentRow.find("#td_InvDt").text();
    if (flag == "Detail") {
        var cust_id = CurrentRow.find("#cust_id").text();
    }
    else {
        var cust_id = $("#HdnCustomerNameForPaidAmt").val();
    }
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/AccountReceivable/SearchPaidAmountDetail",
            data: {
                InVNo: InVNo,
                InvDate: InvDate,
                cust_id: cust_id
            },
            success: function (data) {
                debugger;
                $('#PaidAmountDetailsPopup').html(data);
                $("#Prtal_PAInvoiceNumber").val(InVNo);
                $("#Prtal_PAInvoiceDate").val(InvDt);
                cmn_apply_datatable("#tbl_PaidAmtDetails");
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