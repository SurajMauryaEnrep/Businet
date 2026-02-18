/************************************************
Javascript Name:Dashboard
Created By:Prem
Created Date: 12-11-2021
Description: This Javascript use for the Binding Dataq

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    GetFromDate();
    pendingDocument();
    OnclickChart();
    $('#ddl_branch').multiselect();
});
let currentPage = 0;
const rowsPerPage = 10;
let dataRows = [];

function renderTable() {
    const tbody = $('#TblPending_Document tbody');
    tbody.empty(); // Clear previous rows
    $('#TblPending_Document tbody tr').remove();
    const totalPages = Math.ceil(dataRows.length / rowsPerPage);  
    const start = currentPage * rowsPerPage;
    const end = Math.min(start + rowsPerPage, dataRows.length);

    for (let k = start; k < end; k++) {
        const row = dataRows[k];
        tbody.append(`
                <tr>
                    <td>${row.doc_name}</td>
                    <td style="display:none;">
                        <input type="hidden" id="dhd_docid" value="${row.doc_id}" />
                    </td>
                    <td onclick="Doc_RedirectToListPage(${row.review},event,'rv')">
                        <a onclick="Doc_RedirectToListPage(${row.review},event,'rv')">${row.review}</a>
                    </td>
                    <td onclick="Doc_RedirectToListPage(${row.unposted},event,'up')">
                        <a onclick="Doc_RedirectToListPage(${row.unposted},event,'up')">${row.unposted}</a>
                    </td>
                    <td onclick="Doc_RedirectToListPage(${row.forward},event,'fw')">
                        <a onclick="Doc_RedirectToListPage(${row.forward},event,'fw')">${row.forward}</a>
                    </td>
                    <td onclick="Doc_RedirectToListPage(${row.reject},event,'rj')">
                        <a onclick="Doc_RedirectToListPage(${row.reject},event,'rj')">${row.reject}</a>
                    </td>
                </tr>
            `);
    }
    $("#HdnTblPending_Document >tbody").html($('#TblPending_Document tbody').html());
    // Update pagination UI
    $('#pageInfo').text(`Page ${currentPage + 1} of ${totalPages}`);
    $('#btnPrev').prop('disabled', currentPage === 0);
    $('#btnNext').prop('disabled', currentPage >= totalPages - 1);
}

function changePage(direction) {
    const totalPages = Math.ceil(dataRows.length / rowsPerPage);

    currentPage += direction;

    if (currentPage <= 0) currentPage = 0;
    if (currentPage >= totalPages) currentPage = totalPages - 1;

    renderTable();
}

// Call this after AJAX completes and arr.Table3 is available
function loadDataFromServer(arr, flag) {
    if (flag == "FirstTime") {
        if (arr.Table3 && arr.Table3.length > 0) {
            dataRows = arr.Table3;
            currentPage = 0;
            renderTable();
        } else {
            $('#TblPending_Document tbody tr').remove();

            $('#btnPrev, #btnNext').prop('disabled', true);
        }
    }
    else {
        if (arr.Table && arr.Table.length > 0) {
            dataRows = arr.Table;
            currentPage = 0;
            renderTable();
        } else {
            $('#TblPending_Document tbody tr').remove();

            $('#btnPrev, #btnNext').prop('disabled', true);
        }
    }
   
}
function FilterImportantMIS(e) {
    var searchText = e.currentTarget.value.toLowerCase();

    // Loop through all .imp_mis anchors inside #imp_mis
    $('#imp_mis .imp_mis').each(function () {
        var linkText = $(this).text().toLowerCase();

        // Check if the anchor text includes the search text
        if (linkText.indexOf(searchText) > -1) {
            $(this).show(); // Match found: show the link
        } else {
            $(this).hide(); // No match: hide the link
        }
    });
}
function FilterPendingDocuments(e) {
    debugger;
    var listdt = e.currentTarget.value.toLocaleLowerCase();
    var list = "";
    $("#HdnTblPending_Document >tbody > tr").each(function () {
        var currentRow = $(this);
        var DocName = currentRow.find("td:eq(0)").text();
        if (DocName.toLocaleLowerCase().indexOf(listdt) > -1) {
            list += "<tr>" + currentRow.html() + "</tr>";
        }
    });
    $("#TblPending_Document > tbody").html(list)
}
function GetAndBindDashboardData(datefilterflag,top, chart_type, chart_name, from_dt, to_dt,flag) {
    debugger;
    var currFormate = "";
    var dflag = GetFYVal();
    if (dflag == "C") {
        var Fromdt = $("#txtFromdate").val();
        var Todt = $("#txtTodate").val();
        from_dt = Fromdt;
        to_dt = Todt;
    }
    if (datefilterflag == null || datefilterflag == "") {
        datefilterflag = dflag;
    }
    $.ajax({
        type: "POST",
        url: "/Dashboard/DashboardHome/Get_AllDashboardData",
        data: { Dateflag: datefilterflag, Fromdt: from_dt, Todt: to_dt, Top: top, Charttype: chart_type, Flag: flag},
        dataType: "json",
        success: function (data) {
            if (data == 'ErrorPage') {
                //PO_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                debugger;
                arr = JSON.parse(data);
                var countTable = [];
                countTable.push({ arr: arr });
                if (flag == "Chart") {
                    //-------------Charts-----------------------
                    $("#PervierChart").text(chart_name);
                    var chart_type = $("#ddl_chart").val();
                    if (arr.Table.length > 0) {

                        if (chart_type == "PC") {
                            var name_ = new Array();
                            var val_ = new Array();
                            for (var k = 0; k < arr.Table.length; k++) {
                                name_.push(arr.Table[k].dataname);
                                val_.push(parseFloat(arr.Table[k].val).toFixed(2));
                            }

                            $("#graph_bar").find("#graph_bar-wrapper").remove();

                            $('#graph_bar').graphify({
                                start: 'pie',
                                obj: {
                                    width: 285,
                                    height: 285,
                                    x: name_,
                                    points: val_.map(Number)
                                }
                            });
                        }
                        else if (chart_type == "DC") {
                            var name_ = new Array();
                            var val_ = new Array();
                            for (var k = 0; k < arr.Table.length; k++) {
                                name_.push(arr.Table[k].dataname);
                                val_.push(parseFloat(arr.Table[k].val).toFixed(2));
                            }

                            $("#graph_bar").find("#graph_bar-wrapper").remove();

                            $('#graph_bar').graphify({
                                start: 'donut',
                                obj: {
                                    width: 285,
                                    height: 285,
                                    x: name_,
                                    points: val_.map(Number)
                                }
                            });
                        }
                        else {
                            //currFormate = arr.Table5.curr_format;
                            var ItemDetailList = new Array();
                            for (var k = 0; k < arr.Table.length; k++) {
                                var InputList = {};
                                InputList.name = arr.Table[k].dataname;
                                InputList.val = parseFloat(arr.Table[k].val).toFixed(2);
                                ItemDetailList.push(InputList);
                            }
                            var findata = [];
                            findata = ItemDetailList;

                            $("#graph_bar").find("svg").remove();
                            $("#graph_bar").find("div").remove();

                            $("#graph_bar").length && Morris.Bar({
                                element: "graph_bar",
                                data: findata,
                                xkey: "name",
                                ykeys: ["val"],
                                labels: ["Val"],
                                barRatio: .4,
                                barColors: ["#26B99A", "#34495E", "#ACADAC", "#3498DB"],
                                xLabelAngle: 35,
                                hideHover: "auto",
                                resize: !0,
                                //xLabelFormat: function (x) {
                                //    return x; // Keep x-axis labels as they are
                                //},
                                yLabelFormat: function (y) {
                                    return y.toLocaleString($("#curr_format").text());
                                }
                            })
                        }
                    }
                    else {
                        if (chart_type == "PC" || chart_type == "DC") {
                            $("#graph_bar").find("#graph_bar-wrapper").remove();
                        }
                        else {
                            $("#graph_bar").find("svg").remove();
                            $("#graph_bar").find("div").remove();
                        }
                    }
                    //--------------End-------------------------
                }
                else if (flag == "def_frmt") {
                    //this is for setting currency format in session 
                    $("#curr_format").text(arr.Table[0].curr_format);
                }
                else {

                    //-------------Charts-----------------------
                    $("#PervierChart").text(chart_name);
                    var chart_type = $("#ddl_chart").val();
                    if (arr.Table.length > 0) {
                        if (chart_type == "PC") {
                            var name_ = new Array();
                            var val_ = new Array();
                            for (var k = 0; k < arr.Table.length; k++) {
                                name_.push(arr.Table[k].dataname);
                                val_.push(parseFloat(arr.Table[k].val).toFixed(2));
                            }

                            $("#graph_bar").find("#graph_bar-wrapper").remove();

                            $('#graph_bar').graphify({
                                start: 'pie',
                                obj: {
                                    width: 285,
                                    height: 285,
                                    x: name_,
                                    points: val_.map(Number)
                                }
                            });
                        }
                        else if (chart_type == "DC") {
                            var name_ = new Array();
                            var val_ = new Array();
                            for (var k = 0; k < arr.Table.length; k++) {
                                name_.push(arr.Table[k].dataname);
                                val_.push(parseFloat(arr.Table[k].val).toFixed(2));
                            }

                            $("#graph_bar").find("#graph_bar-wrapper").remove();

                            $('#graph_bar').graphify({
                                start: 'donut',
                                obj: {
                                    width: 285,
                                    height: 285,
                                    x: name_,
                                    points: val_.map(Number)
                                }
                            });
                        }
                        else {
                            //currFormate = arr.Table5.curr_format;
                            var ItemDetailList = new Array();
                            for (var k = 0; k < arr.Table.length; k++) {
                                var InputList = {};
                                InputList.name = arr.Table[k].dataname;
                                InputList.val = parseFloat(arr.Table[k].val).toFixed(2);
                                ItemDetailList.push(InputList);
                            }
                            var findata = [];
                            findata = ItemDetailList;

                            $("#graph_bar").find("svg").remove();
                            $("#graph_bar").find("div").remove();

                            $("#graph_bar").length && Morris.Bar({
                                element: "graph_bar",
                                data: findata,
                                xkey: "name",
                                ykeys: ["val"],
                                labels: ["Val"],
                                barRatio: .4,
                                barColors: ["#26B99A", "#34495E", "#ACADAC", "#3498DB"],
                                xLabelAngle: 35,
                                hideHover: "auto",
                                resize: !0,
                                //xLabelFormat: function (x) {
                                //    return x; // Keep x-axis labels as they are
                                //},
                                yLabelFormat: function (y) {
                                    return y.toLocaleString($("#curr_format").text());
                                    //return y.toLocaleString(arr.Table[0].currformat);
                                }
                            })
                        }
                    }
                    else {
                        if (chart_type == "PC" || chart_type == "DC") {
                            $("#graph_bar").find("#graph_bar-wrapper").remove();
                        }
                        else {
                            $("#graph_bar").find("svg").remove();
                            $("#graph_bar").find("div").remove();
                        }
                    }
                    //--------------End-------------------------

                    //-------------From Date--------------------
                    if (dflag != "C") {
                        if (arr.Table1.length > 0) {
                            $("#txtFromdate").val(arr.Table1[0].FromDt);
                            //BindDateOnLoad();
                        }
                    }
                    //---------------End------------------------


                    //---------------Ticker---------------------
                    if (arr.Table2.length > 0) {

                        /***commented by shubham maurya on 14-01-2025 for seperater start***/
                        //$("#span_revenue").text(parseFloat(arr.Table2[0].Revenue).toFixed(2) + " M");
                        //$("#span_Expenses").text(parseFloat(arr.Table2[0].Expenses).toFixed(2) + " M");
                        //$("#span_GrossProfit").text(parseFloat(arr.Table2[0].GrossProfit).toFixed(2) + " M");
                        //$("#span_Production").text(parseFloat(arr.Table2[0].Production).toFixed(2) + " M");
                        //$("#span_Receivable").text(parseFloat(arr.Table2[0].Receivable).toFixed(2) + " M");
                        //$("#span_Payable").text(parseFloat(arr.Table2[0].Payable).toFixed(2) + " M");
                        //$("#span_DomesticPurchase").text(parseFloat(arr.Table2[0].DomesticPurchase).toFixed(2) + " M");
                        //$("#span_DomesticSales").text(parseFloat(arr.Table2[0].DomesticSales).toFixed(2) + " M");
                        //$("#span_ExportSales").text(parseFloat(arr.Table2[0].ExportSales).toFixed(2) + " M");
                        //$("#span_ImportPurchase").text(parseFloat(arr.Table2[0].ImportPurchase).toFixed(2) + " M");
                        //$("#span_SaleReturn").text(parseFloat(arr.Table2[0].SaleReturn).toFixed(2) + " M");
                        //$("#span_PurchaseReturn").text(parseFloat(arr.Table2[0].PurchaseReturn).toFixed(2) + " M");
                        /***commented by shubham maurya on 14-01-2025 for seperater End***/

                        var currFormat = arr.Table5[0].curr_format;
                        if (currFormat == "en-in") {
                            $("#span_revenue").text(arr.Table6[0].curr_logo +" " +arr.Table2[0].Revenue + " L");
                            $("#span_Expenses").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].Expenses + " L");
                            $("#span_GrossProfit").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].GrossProfit + " L");
                            $("#span_Production").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].Production + " L");
                            $("#span_Receivable").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].Receivable + " L");
                            $("#span_Payable").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].Payable + " L");
                            $("#span_DomesticPurchase").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].DomesticPurchase + " L");
                            $("#span_DomesticSales").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].DomesticSales + " L");
                            $("#span_ExportSales").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].ExportSales + " L");
                            $("#span_ImportPurchase").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].ImportPurchase + " L");
                            $("#span_SaleReturn").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].SaleReturn + " L");
                            $("#span_PurchaseReturn").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].PurchaseReturn + " L");
                        }
                        else {
                            $("#span_revenue").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].Revenue + " M");
                            $("#span_Expenses").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].Expenses + " M");
                            $("#span_GrossProfit").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].GrossProfit + " M");
                            $("#span_Production").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].Production + " M");
                            $("#span_Receivable").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].Receivable + " M");
                            $("#span_Payable").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].Payable + " M");
                            $("#span_DomesticPurchase").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].DomesticPurchase + " M");
                            $("#span_DomesticSales").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].DomesticSales + " M");
                            $("#span_ExportSales").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].ExportSales + " M");
                            $("#span_ImportPurchase").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].ImportPurchase + " M");
                            $("#span_SaleReturn").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].SaleReturn + " M");
                            $("#span_PurchaseReturn").text(arr.Table6[0].curr_logo + " " +arr.Table2[0].PurchaseReturn + " M");
                        }

                        

                    }
                    //----------------End-----------------------

                    //--------Pending Document------------------
                    loadDataFromServer(arr, 'FirstTime');
                    /*Commented BY NItesh 10062025 */
                    //if (arr.Table3.length > 0) { 
                    //    loadDataFromServer(arr);
                    //    //$('#TblPending_Document tbody tr').remove();
                    //    //for (var k = 0; k < arr.Table3.length; k++) {
                    //    //    $('#TblPending_Document tbody').append(`<tr">
                    //    //    <td>${arr.Table3[k].doc_name}</td>
                    //    //    <td style="display:none;"><input type="hidden" id="dhd_docid" value="${arr.Table3[k].doc_id}" /></td>
                    //    //    <td onclick="Doc_RedirectToListPage(${arr.Table3[k].review},event,'rv')"><a onclick="Doc_RedirectToListPage(${arr.Table3[k].review},event,'rv')">${arr.Table3[k].review}</a></td>
                    //    //    <td onclick="Doc_RedirectToListPage(${arr.Table3[k].unposted},event,'up')"><a onclick="Doc_RedirectToListPage(${arr.Table3[k].unposted},event,'up')">${arr.Table3[k].unposted}</a></td>
                    //    //    <td onclick="Doc_RedirectToListPage(${arr.Table3[k].forward},event,'fw')"><a onclick="Doc_RedirectToListPage(${arr.Table3[k].forward},event,'fw')">${arr.Table3[k].forward}</a></td>
                    //    //    <td onclick="Doc_RedirectToListPage(${arr.Table3[k].reject},event,'rj')"><a onclick="Doc_RedirectToListPage(${arr.Table3[k].reject},event,'rj')">${arr.Table3[k].reject}</a></td>
                    //    //    </tr>`);
                    //    //}
                    //    $("#HdnTblPending_Document >tbody").html($('#TblPending_Document tbody').html());
                    //}
                    //else {
                    //    $('#TblPending_Document tbody tr').remove();
                    //}
                    //--------------End-------------------------

                    //-----------------Alerts-------------------
                    if (arr.Table4.length > 0) {
                        //if (parseInt(arr.Table4[0].overdue_dso) > 0) {
                        //    $('#span_overdue_dso').addClass("bg-red");
                        //    $('#span_overdue_dso').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_overdue_dso').addClass("bg-green");
                        //    $('#span_overdue_dso').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].overdue_eso) > 0) {
                        //    $('#span_overdue_eso').addClass("bg-red");
                        //    $('#span_overdue_eso').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_overdue_eso').addClass("bg-green");
                        //    $('#span_overdue_eso').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].overdue_dpo) > 0) {
                        //    $('#span_overdue_dpo').addClass("bg-red");
                        //    $('#span_overdue_dpo').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_overdue_dpo').addClass("bg-green");
                        //    $('#span_overdue_dpo').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].overdue_ipo) > 0) {
                        //    $('#span_overdue_ipo').addClass("bg-red");
                        //    $('#span_overdue_ipo').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_overdue_ipo').addClass("bg-green");
                        //    $('#span_overdue_ipo').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].overdue_production) > 0) {
                        //    $('#span_overdue_production').addClass("bg-red");
                        //    $('#span_overdue_production').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_overdue_production').addClass("bg-green");
                        //    $('#span_overdue_production').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].overdue_receipts) > 0) {
                        //    $('#span_overdue_receipts').addClass("bg-red");
                        //    $('#span_overdue_receipts').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_overdue_receipts').addClass("bg-green");
                        //    $('#span_overdue_receipts').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].overdue_payments) > 0) {
                        //    $('#span_overdue_payments').addClass("bg-red");
                        //    $('#span_overdue_payments').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_overdue_payments').addClass("bg-green");
                        //    $('#span_overdue_payments').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].Upcoming_payments) > 0) {//add by sm on 17-02-2025
                        //    $('#span_upcoming_payments').addClass("bg-red");
                        //    $('#span_upcoming_payments').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_upcoming_payments').addClass("bg-green");
                        //    $('#span_upcoming_payments').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].Upcoming_receipts) > 0) {//add by sm on 17-02-2025
                        //    $('#span_upcoming_receipt').addClass("bg-red");
                        //    $('#span_upcoming_receipt').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_upcoming_receipt').addClass("bg-green");
                        //    $('#span_upcoming_receipt').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].overdue_maintenance) > 0) {
                        //    $('#span_overdue_maintenance').addClass("bg-red");
                        //    $('#span_overdue_maintenance').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_overdue_maintenance').addClass("bg-green");
                        //    $('#span_overdue_maintenance').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].near_expiry_items) > 0) {//add by sm on 19-02-2025
                        //    $('#span_near_expiry_items').addClass("bg-red");
                        //    $('#span_near_expiry_items').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_near_expiry_items').addClass("bg-green");
                        //    $('#span_near_expiry_items').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].expired_items) > 0) {
                        //    $('#span_expired_items').addClass("bg-red");
                        //    $('#span_expired_items').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_expired_items').addClass("bg-green");
                        //    $('#span_expired_items').removeClass("bg-red");
                        //}
                        //if (parseInt(arr.Table4[0].stockout_items) > 0) {
                        //    $('#span_stockout_items').addClass("bg-red");
                        //    $('#span_stockout_items').removeClass("bg-green");
                        //}
                        //else {
                        //    $('#span_stockout_items').addClass("bg-green");
                        //    $('#span_stockout_items').removeClass("bg-red");
                        //}
                        if (parseInt(arr.Table4[0].overdue_dso) > 0) {
                            $('#span_overdue_dso').addClass("bg-red");
                            $('#span_overdue_dso').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_overdue_dso', "show");
                        }
                        else {
                            $('#span_overdue_dso').addClass("bg-green");
                            $('#span_overdue_dso').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_overdue_dso', "Hide");
                        }
                        if (parseInt(arr.Table4[0].overdue_eso) > 0) {
                            $('#span_overdue_eso').addClass("bg-red");
                            $('#span_overdue_eso').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_overdue_eso', "show");
                        }
                        else {
                            $('#span_overdue_eso').addClass("bg-green");
                            $('#span_overdue_eso').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_overdue_eso', "Hide");
                        }
                        if (parseInt(arr.Table4[0].overdue_dpo) > 0) {
                            $('#span_overdue_dpo').addClass("bg-red");
                            $('#span_overdue_dpo').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_overdue_dpo', "show");
                        }
                        else {
                            $('#span_overdue_dpo').addClass("bg-green");
                            $('#span_overdue_dpo').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_overdue_dpo', "Hide");
                        }
                        if (parseInt(arr.Table4[0].overdue_ipo) > 0) {
                            $('#span_overdue_ipo').addClass("bg-red");
                            $('#span_overdue_ipo').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_overdue_ipo', "show");
                        }
                        else {
                            $('#span_overdue_ipo').addClass("bg-green");
                            $('#span_overdue_ipo').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_overdue_ipo', "Hide");
                        }
                        if (parseInt(arr.Table4[0].overdue_production) > 0) {
                            $('#span_overdue_production').addClass("bg-red");
                            $('#span_overdue_production').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_overdue_production', "show");
                        }
                        else {
                            $('#span_overdue_production').addClass("bg-green");
                            $('#span_overdue_production').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_overdue_production', "Hide");
                        }
                        if (parseInt(arr.Table4[0].overdue_receipts) > 0) {
                            $('#span_overdue_receipts').addClass("bg-red");
                            $('#span_overdue_receipts').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_overdue_receipts', "show");
                        }
                        else {
                            $('#span_overdue_receipts').addClass("bg-green");
                            $('#span_overdue_receipts').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_overdue_receipts', "Hide");
                        }
                        if (parseInt(arr.Table4[0].overdue_payments) > 0) {
                            $('#span_overdue_payments').addClass("bg-red");
                            $('#span_overdue_payments').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_overdue_payments', "show");
                        }
                        else {
                            $('#span_overdue_payments').addClass("bg-green");
                            $('#span_overdue_payments').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_overdue_payments', "Hide");
                        }
                        if (parseInt(arr.Table4[0].Upcoming_payments) > 0) {//add by sm on 17-02-2025
                            $('#span_upcoming_payments').addClass("bg-red");
                            $('#span_upcoming_payments').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_upcoming_payments', "show");
                        }
                        else {
                            $('#span_upcoming_payments').addClass("bg-green");
                            $('#span_upcoming_payments').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_upcoming_payments', "Hide");

                        }
                        if (parseInt(arr.Table4[0].Upcoming_receipts) > 0) {//add by sm on 17-02-2025
                            $('#span_upcoming_receipt').addClass("bg-red");
                            $('#span_upcoming_receipt').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_upcoming_receipt', "show");
                        }
                        else {
                            $('#span_upcoming_receipt').addClass("bg-green");
                            $('#span_upcoming_receipt').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_upcoming_receipt', "Hide");
                        }
                        if (parseInt(arr.Table4[0].overdue_maintenance) > 0) {
                            $('#span_overdue_maintenance').addClass("bg-red");
                            $('#span_overdue_maintenance').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_overdue_maintenance', "show");
                        }
                        else {
                            $('#span_overdue_maintenance').addClass("bg-green");
                            $('#span_overdue_maintenance').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_overdue_maintenance', "Hide");
                        }
                        if (parseInt(arr.Table4[0].near_expiry_items) > 0) {//add by sm on 19-02-2025
                            $('#span_near_expiry_items').addClass("bg-red");
                            $('#span_near_expiry_items').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_near_expiry_items', "show");
                        }
                        else {
                            $('#span_near_expiry_items').addClass("bg-green");
                            $('#span_near_expiry_items').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_near_expiry_items', "Hide");
                        }
                        if (parseInt(arr.Table4[0].expired_items) > 0) {
                            $('#span_expired_items').addClass("bg-red");
                            $('#span_expired_items').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_expired_items', "show");
                        }
                        else {
                            $('#span_expired_items').addClass("bg-green");
                            $('#span_expired_items').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_expired_items', "Hide");
                        }
                        if (parseInt(arr.Table4[0].stockout_items) > 0) {
                            $('#span_stockout_items').addClass("bg-red");
                            $('#span_stockout_items').removeClass("bg-green");
                            DisplayNoneShowAlertCount('#span_stockout_items', "show");
                        }
                        else {
                            $('#span_stockout_items').addClass("bg-green");
                            $('#span_stockout_items').removeClass("bg-red");
                            DisplayNoneShowAlertCount('#span_stockout_items', "Hide");
                        }


                        $("#span_overdue_dso").text(arr.Table4[0].overdue_dso);
                        $("#span_overdue_eso").text(arr.Table4[0].overdue_eso);
                        $("#span_overdue_dpo").text(arr.Table4[0].overdue_dpo);
                        $("#span_overdue_ipo").text(arr.Table4[0].overdue_ipo);
                        $("#span_overdue_production").text(arr.Table4[0].overdue_production);
                        $("#span_overdue_receipts").text(arr.Table4[0].overdue_receipts);
                        $("#span_overdue_payments").text(arr.Table4[0].overdue_payments);
                        $("#span_upcoming_payments").text(arr.Table4[0].Upcoming_payments);//add by sm on 17-02-2025
                        $("#span_upcoming_receipt").text(arr.Table4[0].Upcoming_receipts);//add by sm on 17-02-2025
                        $("#span_overdue_maintenance").text(arr.Table4[0].overdue_maintenance);
                        $("#span_near_expiry_items").text(arr.Table4[0].near_expiry_items);//add by sm on 19-02-2025
                        $("#span_expired_items").text(arr.Table4[0].expired_items);
                        $("#span_stockout_items").text(arr.Table4[0].stockout_items);
                    }
                    //------------------End---------------------
                }
            }
        }
    });
}
function DisplayNoneShowAlertCount(id, flag) {
    if (flag == "show") {
        $(id).css("display", "")
    }
    else {
        $(id).css("display", "none")
    }

}
function Doc_RedirectToListPage(value,e,flag) {
    debugger
    if (value > 0) {
        var clickedrow = $(e.target).closest("tr");
        var DocID = clickedrow.find("#dhd_docid").val();
        if (DocID === "105101130") {
            window.location.href = "/ApplicationLayer/DPO/GetPurchadeOrdersList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101136") {
            window.location.href = "/ApplicationLayer/DPO/GetPurchadeOrdersList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101135") {
            window.location.href = "/ApplicationLayer/ServicePurchaseOrder/GetSPOList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101137") {
            window.location.href = "/ApplicationLayer/ServiceVerification/GetVerificList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101150") {
            window.location.href = "/ApplicationLayer/ServicePurchaseInvoice/GetInvoiceList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103147") {
            window.location.href = "/ApplicationLayer/ServiceSaleInvoice/GetInvoiceList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101140101") {
            window.location.href = "/ApplicationLayer/DPO/GetPurchadeOrdersList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101120") {
            window.location.href = "/ApplicationLayer/PurchaseQuotation/GetPurchaseQuotationsList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103125") {
            window.location.href = "/ApplicationLayer/LSOList/GetSaleOrderList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102115120") {
            window.location.href = "/ApplicationLayer/OpeningMaterialReceipt/GetOpeningMaterialRecieptList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103130") {
            window.location.href = "/ApplicationLayer/DomesticPacking/GetDomesticPackingList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103145115") {
            window.location.href = "/ApplicationLayer/DomesticPacking/GetDomesticPackingList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102150") {
            window.location.href = "/ApplicationLayer/PurchaseReturn/GetPurchaseReturnList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103135") {
            window.location.href = "/ApplicationLayer/Shipment/GetShipmentList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103145120") {
            window.location.href = "/ApplicationLayer/Shipment/GetShipmentList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103120") {
            window.location.href = "/ApplicationLayer/DomesticSalesQuotationList/GetDomesticSalesQuotationList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103142") {
            window.location.href = "/ApplicationLayer/SalesReturn/GetSalesReturnList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105105117") {
            window.location.href = "/ApplicationLayer/MaterialRequirementPlan/GetMaterialRequirementPlanList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105105116") {
            window.location.href = "/ApplicationLayer/ProductionPlan/GetProductionPlanList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102155") {
            window.location.href = "/ApplicationLayer/StockTake/GetStockTakeList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103145105") {
            window.location.href = "/ApplicationLayer/DomesticSalesQuotationList/GetDomesticSalesQuotationList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103145110") {
            window.location.href = "/ApplicationLayer/LSOList/GetSaleOrderList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103140") {
            window.location.href = "/ApplicationLayer/LocalSaleInvoice/GetSaleInvoiceList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103145125") {
            window.location.href = "/ApplicationLayer/LocalSaleInvoice/GetSaleInvoiceList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105105115") {
            window.location.href = "/ApplicationLayer/BillofMaterial/GetBillofMaterialList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105105125") {
            window.location.href = "/ApplicationLayer/ProductionOrder/GetProductionOrderList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105105140") {
            window.location.href = "/ApplicationLayer/ShopfloorStockTransfer/GetShopfloorStockTransferList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105105130") {
            window.location.href = "/ApplicationLayer/ProductionConfirmation/GetProductionConfirmationList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105105120") {
            window.location.href = "/ApplicationLayer/ProductionAdvice/GetProductionAdviceList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103185") {
            window.location.href = "/ApplicationLayer/ProductCatalouge/GetPRoductCatalogueList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115101") {
            window.location.href = "/ApplicationLayer/JournalVoucher/GetJournalVoucherList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115120") {
            window.location.href = "/ApplicationLayer/BankPayment/GetBankPaymentList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115125") {
            window.location.href = "/ApplicationLayer/Contra/GetContraList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115110") {
            window.location.href = "/ApplicationLayer/BankReceipt/GetBankReceiptList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115115") {
            window.location.href = "/ApplicationLayer/CashPayment/GetCashPaymentList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115105") {
            window.location.href = "/ApplicationLayer/CashReceipt/GetCashReceiptList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115130") {
            window.location.href = "/ApplicationLayer/DebitNote/GetDebitNoteList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102120") {
            window.location.href = "/ApplicationLayer/QualityInspection/GetQualityInspectionList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105105135") {
            window.location.href = "/ApplicationLayer/QualityInspection/GetQualityInspectionList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105108115") {
            window.location.href = "/ApplicationLayer/QualityInspection/GetQualityInspectionList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115135") {
            window.location.href = "/ApplicationLayer/CreditNote/GetCreditNoteListDashboard/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101110") {
            window.location.href = "/ApplicationLayer/PurchaseRequisition/GetPRListDashboard/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101115") {
            window.location.href = "/ApplicationLayer/RequestForQuotation/GetRFQListDashboard/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115140") {
            window.location.href = "/ApplicationLayer/PurchaseVoucher/GetPurchaseVoucherList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115145") {
            window.location.href = "/ApplicationLayer/SalesVoucher/GetSalesVoucherList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101145") {
            window.location.href = "/ApplicationLayer/LocalPurchaseInvoice/GetPurchaseInvoiceDashbrd/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101140125") {
            window.location.href = "/ApplicationLayer/LocalPurchaseInvoice/GetPurchaseInvoiceDashbrd/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102110") {
            window.location.href = "/ApplicationLayer/DeliveryNoteList/GetDeliveryNoteDashbrd/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102115101") {
            window.location.href = "/ApplicationLayer/GRNDetail/GetGRNList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102115110") {
            window.location.href = "/ApplicationLayer/MaterialTransferReceipt/GetMTRDetailDashbrd/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102115125") {
            window.location.href = "/ApplicationLayer/SampleReceipt/GetSRList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102125") {
            window.location.href = "/ApplicationLayer/MaterialRequisitionSlip/GetMTSDetailDashbrd/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103101") {
            window.location.href = "/ApplicationLayer/CustomerPriceList/GetCustomerPriceList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103150") {
            window.location.href = "/ApplicationLayer/SalesForecast/GetSFRCastDetailDashbrd/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102140") {
            window.location.href = "/ApplicationLayer/MaterialTransferOrder/GetMaterialTransferOrderList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104115150") {
            window.location.href = "/ApplicationLayer/InvoiceAdjustment/GetInvoiceAdjustmentList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105108101") {
            window.location.href = "/ApplicationLayer/JobOrderSC/GetJODashbordList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103145127") {
            window.location.href = "/ApplicationLayer/CustomInvoice/GetCustomInvoiceList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105108105") {
            window.location.href = "/ApplicationLayer/MaterialDispatchSC/GetMDDashbordList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105108110") {
            window.location.href = "/ApplicationLayer/DeliveryNoteSC/GetDNDashbordList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105108120") {
            window.location.href = "/ApplicationLayer/GoodReceiptNoteSC/GetGRNDashbordList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101152") {
            window.location.href = "/ApplicationLayer/ConsumableInvoice/GetConsumableInvoiceList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105108123") {
            window.location.href = "/ApplicationLayer/JobInvoiceSC/GetJobInvoiceDashbordList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105105127") {
            window.location.href = "/ApplicationLayer/ReworkableJobOrder/GetReworkJobOrderDashbordList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104130") {
            window.location.href = "/ApplicationLayer/TDSPosting/TDSPosting/?wfStatus=" + flag;
        }
        if (DocID === "105102165") {
            window.location.href = "/ApplicationLayer/StockSwap/StockSwap/?wfStatus=" + flag;
        }
        if (DocID === "105103148") {
            window.location.href = "/ApplicationLayer/ScrapSaleInvoice/GetInvoiceList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102115105") {
            window.location.href = "/ApplicationLayer/FinishedGoodsReceipt/DashBordtoListFinishGoodsReceipt/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102105") {
            window.location.href = "/ApplicationLayer/GatePassOutward/DashBordtoList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102101") {
            window.location.href = "/ApplicationLayer/GatePassInward/DashBordtoList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102115130") {
            window.location.href = "/ApplicationLayer/ExternalReceipt/DashBordtoList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105104105") {
            window.location.href = "/ApplicationLayer/FinanceBudget/DashBordtoList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105103117") {/*Add by Hina sharma on 27-01-2025 for sales enquiry*/
            window.location.href = "/ApplicationLayer/SalesEnquiry/GetSalesEnquiryDashbordList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105102160") {
            window.location.href = "/ApplicationLayer/AssemblyKit/DashBordtoList/?docid=" + DocID + "&status=" + flag;
        }
        if (DocID === "105101154") {
            window.location.href = "/ApplicationLayer/DirectPurchaseInvoice/DirectPurchaseInvoice/?wfStatus=" + flag;
        }
        /*Added by Surbhi on 03/06/2025 for Supplementary Purchase Invoice(105101147)*/
        if (DocID === "105101147") {
            window.location.href = "/ApplicationLayer/SupplementaryPurchaseInvoice/SupplementaryPurchaseInvoice/?wfStatus=" + flag;
        }
        /*Added by Shubham Maurya on06-06-2025 for IBS*/
        if (DocID === "105103149") {
            window.location.href = "/ApplicationLayer/InterBranchSale/InterBranchSale/?wfStatus=" + flag;
        }
    }
}

function OnChangeTop() {
    debugger;
    OnChangeFYAndTopddl();
}
function OnChange_yearMnth() {
    GetFromDate();
    OnChangeFYAndTopddl();
}
function GetFYVal() {
    var dflag = "";

    if ($("#filterY").is(":checked")) {
        dflag = "Y";
    }
    if ($("#filterM").is(":checked")) {
        dflag = "M";
    }
    if ($("#Custom").is(":checked")) {
        dflag = "C";
        $("#txtFromdate").prop("disabled", false);
        $("#txtTodate").prop("disabled", false);
        $("#BtnSearch").prop("disabled", false);
    }
    else {
        $("#txtFromdate").prop("disabled", true);
        $("#txtTodate").prop("disabled", true);
        $("#BtnSearch").prop("disabled", true);
    }
    return dflag
}
function OnChangeFYAndTopddl() {
    var data = $("#PervierChart").text();
    var top = $("#ddl_top option:selected").text();
    var Fromdt = $("#txtFromdate").val();
    var Todt = $("#txtTodate").val();
    debugger
    BindChartsDetails(data.replace(' ', '_'), top, Fromdt, Todt);
}
function OnClickSearchBtn() {
    debugger;
    var Fromdt = $("#txtFromdate").val();
    var Todt = $("#txtTodate").val();

    if (Fromdt != null && Fromdt != "" && Todt != null && Todt != "") {
        GetFromDate();
    }

}
function GetFromDate() {
    debugger;
    var dflag = "";
   // pendingDocument();
    if ($("#filterY").is(":checked")) {
        dflag = "Y";
        var date = moment().format('YYYY-MM-DD');


        $("#txtTodate").val(date);
    }
    if ($("#filterM").is(":checked")) {
        dflag = "M";
        var date = moment().format('YYYY-MM-DD');
      
        
        $("#txtTodate").val(date);
    }
    if ($("#Custom").is(":checked")) {
        dflag = "C";
        //var date = moment().format('YYYY-MM-DD');
        var date = moment($("#txtTodate").val()).format('YYYY-MM-DD');
        $("#txtTodate").val(date);
    }
    var chartname = $("#span_TopCustomers").text();

    if ($('#myTab li').length > 1) {
        var fstliname = "";
        $('#myTab li').each(function (e) {

            var dtl = this;
            var liname = dtl.children[0].innerText;

            if (liname == chartname) {
                fstliname = liname;
            }
        });
        if (fstliname == chartname) {
            GetAndBindDashboardData(dflag, 5, "Customers", chartname, "", "", "");

        }
        else {
            GetAndBindDashboardData(dflag, 5, "Customers", chartname, "", "", "def_frmt");
        }

    } else {
        GetAndBindDashboardData(dflag, 5, "Customers", chartname, "", "", "def_frmt");
    }
    //else {
    //    pendingDocument();
    //}
    
    jQuery(document).trigger('jquery.loaded');

}
function OnclickChart() {
    $('.chart_icon').on('click', function () {
        debugger;
        var data = this.id;
        var Fromdt = $("#txtFromdate").val();
        var Todt = $("#txtTodate").val();
        $("#myTab li").removeClass("active");
        $("#li_" + data).addClass("active");
        BindChartsDetails(data, 5, Fromdt, Todt);

        $("#ddl_top").val(1);
    });
}
function OnClickAlerts(evt) {
    debugger;
    var alertname = $(evt).attr('id');

    if (alertname == "od_dso") {
        window.location.href = "/ApplicationLayer/LSOList/GetSaleOrderList/?docid=" + "105103125" + "&status=" + "alrt";
    }
    if (alertname == "od_eso") {
        window.location.href = "/ApplicationLayer/LSOList/GetSaleOrderList/?docid=" + "105103145110" + "&status=" + "alrt";
    }
    if (alertname == "od_dpo") {
        window.location.href = "/ApplicationLayer/DPO/GetPurchadeOrdersList/?docid=" + "105101130" + "&status=" + "alrt";
    }
    if (alertname == "od_ipo") {
        window.location.href = "/ApplicationLayer/DPO/GetPurchadeOrdersList/?docid=" + "105101140101" + "&status=" + "alrt";
    }
    if (alertname == "od_product") {
        window.location.href = "/ApplicationLayer/ProductionOrder/GetProductionOrderList/?docid=" + "105105125" + "&status=" + "alrt";
    }
    if (alertname == "od_recpt") {
        window.location.href = "/ApplicationLayer/AccountReceivable/GetOverDueReceiptsList/?Basis=I" + "&ReceivableType=" + "O" + "&AsDate=" + moment().format("YYYY-MM-DD");
    }
    if (alertname == "od_payment") {
        window.location.href = "/ApplicationLayer/AccountPayable/GetOverDuePaymentList/?Basis=S" + "&PayableType=" + "O" + "&AsDate=" + moment().format("YYYY-MM-DD");
    }
    if (alertname == "up_payment") {//add by shubham maurya on 13-02-2025
        window.location.href = "/ApplicationLayer/AccountPayable/GetOverDuePaymentList/?Basis=S" + "&PayableType="+"U" + "&AsDate=" + moment().format("YYYY-MM-DD");
    }
    if (alertname == "up_receipt") {//add by shubham maurya on 13-02-2025
        window.location.href = "/ApplicationLayer/AccountReceivable/GetOverDueReceiptsList/?Basis=I" + "&ReceivableType=" + "U" + "&AsDate=" + moment().format("YYYY-MM-DD");
    }
    if (alertname == "near_expired_items") {//add by shubham maurya on 13-02-2025
        window.location.href = "/ApplicationLayer/StockDetail/GetStockDetailFromDashBoard/?ExpiredItm=N" + "&StockOutItm=" + "N" + "&NearExpiryItm=" + "Y";
    }
    if (alertname == "expired_items") {//add by shubham maurya on 13-02-2025
        window.location.href = "/ApplicationLayer/StockDetail/GetStockDetailFromDashBoard/?ExpiredItm=Y" + "&StockOutItm=" + "N" + "&NearExpiryItm=" + "N";
    }
    if (alertname == "stockout_items") {//add by shubham maurya on 13-02-2025
        window.location.href = "/ApplicationLayer/StockDetail/GetStockDetailFromDashBoard/?ExpiredItm=N" + "&StockOutItm=" + "Y" + "&NearExpiryItm=" + "N";
    }
}
function ShowMISReports(featureId) {
    debugger
    var FromDt = $("#txtFromdate").val();
    var ToDt = $("#txtTodate").val();
    if (featureId == "101145") {
        window.location.href = "/ApplicationLayer/SalesDetail/showReportFromDashBoard/?salesby=P&FromDt=" + FromDt + "&ToDt=" + ToDt;
    }
    if (featureId == "101140") {
        window.location.href = "/ApplicationLayer/SalesDetail/showReportFromDashBoard/?salesby=C&FromDt=" + FromDt + "&ToDt=" + ToDt;
    }
    if (featureId == "101150") {
        window.location.href = "/ApplicationLayer/SalesDetail/showReportFromDashBoard/?salesby=G&FromDt=" + FromDt + "&ToDt=" + ToDt;
    }
    if (featureId == "101051") {
        window.location.href = "/ApplicationLayer/SalesDetail/showReportFromDashBoard/?salesby=R&FromDt=" + FromDt + "&ToDt=" + ToDt;
    }
    if (featureId == "101155") {
        window.location.href = "/ApplicationLayer/ProcurementDetail/showReportFromDashBoard/?purchaseBy=S&FromDt=" + FromDt + "&ToDt=" + ToDt;
    }
    if (featureId == "101160") {
        window.location.href = "/ApplicationLayer/ProcurementDetail/showReportFromDashBoard/?purchaseBy=S&P_type=I&FromDt=" + FromDt + "&ToDt=" + ToDt;
    }
    if (featureId == "101165") {
        window.location.href = "/ApplicationLayer/ProcurementDetail/showReportFromDashBoard/?purchaseBy=T&FromDt=" + FromDt + "&ToDt=" + ToDt;
    }
    if (featureId == "101170") {
        window.location.href = "/ApplicationLayer/ProcurementDetail/showReportFromDashBoard/?purchaseBy=G&FromDt=" + FromDt + "&ToDt=" + ToDt;
    }
    if (featureId == "101175") {
        window.location.href = "/ApplicationLayer/AccountReceivable/AccountReceivable";
    }
    if (featureId == "101180") {
        window.location.href = "/ApplicationLayer/AccountPayable/AccountPayable";
    }
    if (featureId == "101185") {
        window.location.href = "/ApplicationLayer/ProductionAnalysis/showReportFromDashBoard/?FromDt=" + FromDt + "&ToDt=" + ToDt;
    }
    if (featureId == "101215") {
        window.location.href = "/ApplicationLayer/StockDetail/showReportFromDashBoard/?ToDt=" + ToDt;
    }
    if (featureId == "101220") {
        window.location.href = "/ApplicationLayer/StockDetailShopfloor/showReportFromDashBoard/?ToDt=" + ToDt;
    }
    if (featureId == "101221") {
        window.location.href = "/ApplicationLayer/StockDetailConsolidated/showReportFromDashBoard/?ToDt=" + ToDt;
    }
    if (featureId == "101255") {
        window.location.href = "/ApplicationLayer/PurchaseTracking/PurchaseTracking";
    }
    if (featureId == "101260") {
        window.location.href = "/ApplicationLayer/SalesTracking/SalesTracking";
    }
    if (featureId == "101222") {
        //FromDt= FromDt.format('DD-MM-YYYY')
        var date = FromDt.split("-");
        var dt = date[2] + '-' + date[1] + '-' + date[0];
        window.location.href = "/ApplicationLayer/BudgetAnalysis/BudgetAnalysisDsahboard/?FromDt=" + dt + "&ToDt=" + ToDt;
    }

}
function OnchangeDBFromDt() {

    var DBFromDt = $("#txtFromdate").val();
    var DBToDt = $("#txtTodate").val();
    $.ajax({
        type: "POST",
        url: "/Dashboard/DashboardHome/SetSession_FromDateAndToDt",
        data: { DBFromDt: DBFromDt, DBToDt: DBToDt },
    });
}
function BindChartsDetails(data, top, Fromdt, Todt) {
    var chartname = "";
    if (data == "Top_Customers") {
        chartname = $("#span_TopCustomers").text();
        GetAndBindDashboardData("", top, "Customers", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Supplier") {
        chartname = $("#span_TopSupplier").text();
        GetAndBindDashboardData("", top, "Suppliers", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Products") {
        chartname = $("#span_TopProducts").text();
        GetAndBindDashboardData("", top, "Products", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Sales_Executive" || data == "Top_Sales Executive") {
        chartname = $("#Top_Sales_Executive").text();
        GetAndBindDashboardData("", top, "SalesExecutive", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Materials") {
        chartname = $("#span_TopMaterials").text();
        GetAndBindDashboardData("", top, "Materials", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Production") {
        chartname = $("#span_TopProduction").text();
        GetAndBindDashboardData("", top, "Production", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Regions") {
        chartname = $("#span_TopRegions").text();
        GetAndBindDashboardData("", top, "Regions", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Stocks") {
        chartname = $("#span_TopStocks").text();
        GetAndBindDashboardData("", top, "Stocks", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Payables") {
        chartname = $("#span_TopPayables").text();
        GetAndBindDashboardData("", top, "Payables", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Receivables") {
        chartname = $("#span_TopReceivables").text();
        GetAndBindDashboardData("", top, "Receivables", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Receipts") {
        chartname = $("#span_TopReceipts").text();
        GetAndBindDashboardData("", top, "Receipts", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Payments") {
        chartname = $("#span_TopPayments").text();
        GetAndBindDashboardData("", top, "Payments", chartname, Fromdt, Todt, "Chart");
    }
    if (data == "Top_Sales_Return" || data == "Top_Sale Return") {
        chartname = $("#span_TopSalesReturn").text();
        GetAndBindDashboardData("", top, "SalesReturn", chartname, Fromdt, Todt, "Chart");
    }
}
function pendingDocument() { /*Added This Function Nitesh 05-12-2023 for pending Document*/
    debugger;
    $.ajax({
        type: "POST",
        url: "/Dashboard/DashboardHome/GetPendingDocument",
        data: {},
        dataType: "json",
        success: function (data) {
            if (data == 'ErrorPage') {
                //PO_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                debugger;
                arr = JSON.parse(data);
                loadDataFromServer(arr,'SecondTime');
              
                // if (arr.Table.length > 0) { /*Commented By NItesh 10062025 For Pagenation*/
                //    $('#TblPending_Document tbody tr').remove();
                //    for (var k = 0; k < arr.Table.length; k++) {
                //        $('#TblPending_Document tbody').append(`<tr">
                //            <td>${arr.Table[k].doc_name}</td>
                //            <td style="display:none;"><input type="hidden" id="dhd_docid" value="${arr.Table[k].doc_id}" /></td>
                //            <td onclick="Doc_RedirectToListPage(${arr.Table[k].review},event,'rv')"><a onclick="Doc_RedirectToListPage(${arr.Table[k].review},event,'rv')">${arr.Table[k].review}</a></td>
                //            <td onclick="Doc_RedirectToListPage(${arr.Table[k].unposted},event,'up')"><a onclick="Doc_RedirectToListPage(${arr.Table[k].unposted},event,'up')">${arr.Table[k].unposted}</a></td>
                //            <td onclick="Doc_RedirectToListPage(${arr.Table[k].forward},event,'fw')"><a onclick="Doc_RedirectToListPage(${arr.Table[k].forward},event,'fw')">${arr.Table[k].forward}</a></td>
                //            <td onclick="Doc_RedirectToListPage(${arr.Table[k].reject},event,'rj')"><a onclick="Doc_RedirectToListPage(${arr.Table[k].reject},event,'rj')">${arr.Table[k].reject}</a></td>
                //            </tr>`);
                //    }
                //    $("#HdnTblPending_Document >tbody").html($('#TblPending_Document tbody').html());
                //}
                //else {
                //    $('#TblPending_Document tbody tr').remove();
                //}
               
            }
        }
    });
}

function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    if ((FromDate != null || FromDate == "") && (ToDate != null || ToDate == "")) {
        if (FromDate > ToDate) {
            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            $("#txtTodate").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdate").val(today);

            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    validatefydate(FromDate, ToDate);
}
function validatefydate(FromDate, ToDate) {
    debugger
    var validate = "N";
    var finfromdt = "";
    var finTodt = "";
    var fin_fromdt = "";
    var fin_Todt = "";
    $("#tbl_hdnfylist tbody tr").each(function () {
        var curr_row = $(this);
        var fystdt = curr_row.find("#fystdt").text();
        var fyenddt = curr_row.find("#fyenddt").text();

        var boolfromdt = "N";
        var booltodt = "N";

        if ((Date.parse(fystdt) <= Date.parse(FromDate)) && (Date.parse(fyenddt) >= Date.parse(FromDate))) {
            boolfromdt = "Y";
            finfromdt = fystdt;
            finTodt = fyenddt;
        }
        if ((Date.parse(fystdt) <= Date.parse(ToDate)) && (Date.parse(fyenddt) >= Date.parse(ToDate))) {
            booltodt = "Y";
        }
        if (boolfromdt == "Y" && booltodt == "Y") {
            validate = "Y";
            fin_fromdt = fystdt;
            fin_Todt = fyenddt;
            return;
        }
    });

    if (validate != "Y") {
        //$("#txtFromdate").val(finfromdt);
        $("#txtTodate").val(finTodt);
    }
}
function OnChangecharttype() {
    debugger;

    var chart_type = $("#ddl_chart").val();

    if (chart_type == "PC" || chart_type == "DC") {
        $("#graph_bar").find("svg").remove();
        $("#graph_bar").find("div").remove();
    }
    else {
        $("#graph_bar").find("#graph_bar-wrapper").remove();
    }
    OnChangeFYAndTopddl();
}