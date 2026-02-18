$(document).ready(function () {
    debugger
    $("#sales_person").select2();
    $('#ddlItemsList').select2();
    DynamicSerchableItemDDL("", "#ddlItemsList", "", "", "", "SO", "Y")
    BindCustomerList();
    BindSONumberList();
    $(document).ready(function () {
        $("#intimationtbl_wrapper a.btn.btn-default.buttons-pdf.buttons-html5").remove()
        $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" type="button" onclick="OrderIntimationPDF()" data-toggle="modal" data-target="#SalesInvoicePrint" data-backdrop="static" data-keyboard="false"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')
        //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountPayableCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    });
});
function BindCustomerList() {
    debugger;
    var Cust_Typ = "I";
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
            var s = "<option value='0'>---All---</option>";
            $.map(data, function (val, item) {
                if (val.Name.trim() != "---Select---") {
                    s += '<option value=' + val.ID + '>' + val.Name + '</option>';
                }
            })
            $("#ddl_CustomerName").html(s);
            $("#ddl_CustomerName").select2({});
        }
    });
}
function OnchangeFromDate() {
    BindSONumberList();
}
function OnchangeToDate() {
    BindSONumberList();
}
function OnchangeOrderType() {
    BindSONumberList();
}
function BindSONumberList() {
    debugger;
    var CustID = $('#ddl_CustomerName').val();
    var Curr_Id = "0";
    var doc_id = "105103190125";
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var OrderType = $("#OrderType").val();
    if (CustID != null && CustID != "") {
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/MISOrderIntimation/GetoOrderIntimationSONoLists",
                data: { Cust_id: CustID, Curr_Id: Curr_Id, doc_id: doc_id, From_dt: From_dt, To_dt: To_dt, OrderType: OrderType},
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
                            $("#ddlOrderNumber option").remove();
                            $("#ddlOrderNumber optgroup").remove();
                            $('#ddlOrderNumber').append(`<optgroup class='def-cursor' id="SOTextddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}' data-supp='${$("#span_CustomerName").text()}'></optgroup>`);
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#SOTextddl').append(`<option data-date="${arr.Table[i].so_dt}" data-supp="${arr.Table[i].cust_name}" value="${arr.Table[i].app_so_no}">${arr.Table[i].app_so_no}</option>`);
                            }
                            var firstEmptySelect = true;
                            $('#ddlOrderNumber').select2({
                                templateResult: function (data) {
                                    var PDRows = "";//$("#SaveItemOrderQtyDetails >tbody > tr td #PD_OrderQtyDocNo[value='" + data.id + "']").val();
                                    if (data.id != PDRows) {
                                        var DocDate = $(data.element).data('date');
                                        var doc = $(data.element).data('supp');
                                        var classAttr = $(data.element).attr('class');
                                        var hasClass = typeof classAttr != 'undefined';
                                        classAttr = hasClass ? ' ' + classAttr : '';
                                        var $result = $(
                                            '<div class="row OrderIntimation">' +
                                            '<div class="col-md-4 col-xs-4' + classAttr + '">' + data.text + '</div>' +
                                            '<div class="col-md-3 col-xs-3' + classAttr + '">' + DocDate + '</div>' +
                                            '<div class="col-md-5 col-xs-5' + classAttr + '">' + doc + '</div>' +
                                            '</div>'
                                        );
                                        return $result;
                                    }
                                    firstEmptySelect = false;
                                }
                            });
                            //document.getElementById("vmso_no").innerHTML = null;
                            //$("#ddlOrderNumber").css("border-color", "#ced4da");
                            //$("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
                            //$("#txtso_dt").val(null);
                        }
                    }
                },
            });
        } catch (err) {
            console.log("GetMenuData Error : " + err.message);
        }
    }
}
function OrderIntimationBtnSearch() {
    var cust_id = $("#ddl_CustomerName").val();
    var OrderType = $("#OrderType").val();
    var OrderNumber = $("#ddlOrderNumber").val();
    var From_dt = $("#txtFromdate").val();
    var To_dt = $("#txtTodate").val();
    var SalesPerson = $("#sales_person").val();
    var ItemId = $('#ddlItemsList').val();

    if (OrderNumber == "---Select---")
        OrderNumber = "0";
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISOrderIntimation/GetOrderIntimationByFilter",
        data: {
            cust_id: cust_id,
            From_dt: From_dt,
            To_dt: To_dt,
            OrderType: OrderType,
            OrderNumber: OrderNumber,
            SalesPerson: SalesPerson,
            ItemId: ItemId
        },
        success: function (data) {
            debugger;
            $("#PartialMIS_OrderIntimationList").html(data);
            $("#intimationtbl_wrapper a.btn.btn-default.buttons-pdf.buttons-html5").remove()
            $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" type="button" onclick="OrderIntimationPDF()" data-toggle="modal" data-target="#SalesInvoicePrint" data-backdrop="static" data-keyboard="false"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')
        }
    })
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hdnItemId").val();
    ItemInfoBtnClick(ItmCode)
}
function OnClickAllOrderIntimationCheck() {
    var Allcheck = "";
    if ($("#AllIntimationCheck").is(":checked")) {
        Allcheck = 'Y';
    }
    else {
        Allcheck = 'N';
    }
    if (Allcheck == 'Y') {

        //var table = $('#datatable-buttons').DataTable();
        var table = $('#intimationtbl').DataTable();
        table.rows().every(function () {
            var CurrentRow = $(this.node());
            CurrentRow.find("#IntimationCheck").prop("checked", true);
            CurrentRow.find("#IntimationCheck").is(":checked")
        });

        //$("#datatable-buttons tbody tr").each(function () {
        //    debugger;
        //    var CurrentRow = $(this);
        //    CurrentRow.find("#IntimationCheck").prop("checked", true);
        //    CurrentRow.find("#IntimationCheck").is(":checked")
        //});
    }
    else {
        //$("#datatable-buttons tbody tr").each(function () {

        var table = $('#intimationtbl').DataTable();
        table.rows().every(function () {
            var CurrentRow = $(this.node());
            CurrentRow.find("#IntimationCheck").prop("checked", false);
            if (CurrentRow.find("#IntimationCheck").is(":checked")) {
                check = 'Y';
            }
            else {
                check = 'N';
            }
            if (check == 'N') {
                CurrentRow.find("#IntimationCheck").prop("checked", false);
            }

            //CurrentRow.find("#IntimationCheck").prop("checked", true);
            //CurrentRow.find("#IntimationCheck").is(":checked")
        });

        //$("#intimationtbl tbody tr").each(function () {
        //    debugger;
        //    var CurrentRow = $(this);
        //    CurrentRow.find("#IntimationCheck").prop("checked", false);
        //    if (CurrentRow.find("#IntimationCheck").is(":checked")) {
        //        check = 'Y';
        //    }
        //    else {
        //        check = 'N';
        //    }
        //    if (check == 'N') {
        //        CurrentRow.find("#IntimationCheck").prop("checked", false);
        //    }
        //});
    }
}
function OrderIntimationPDF() {
    debugger;
    //var Flag = "N";
    //$("#datatable-buttons tbody tr").each(function () {
    //    var CurrentRow = $(this);
    //    if (CurrentRow.find("#IntimationCheck").is(":checked")) {
    //        Flag = "Y";
    //    }
    //});

    //var table = $('#datatable-buttons').DataTable();
    //var table = $('#datatable-buttons').DataTable();
    var table = $('#intimationtbl').DataTable();
    var Flag = "N";

    // Ye code har row (sabhi pages ki) check karega
    table.rows().every(function () {
        var CurrentRow = $(this.node());
        if (CurrentRow.find("#IntimationCheck").is(":checked")) {
            Flag = "Y";
        }
    });

    if (Flag == "N") {
        swal("", $("#NoRecordsFound").text(), "warning");
        $(".ap").attr("data-target", "");
    }
    else {
        $(".ap").attr("data-target", "#SalesInvoicePrint");
    }

}
function OnCheckedItemName() {
    if ($('#chkItemName').prop('checked')) {
        $('#ShowItemName').val('Y');
    }
    else {
        $('#ShowItemName').val('N');
    }
}
function OnCheckedCustSpecItemDesc() {
    debugger;
    if ($('#chkCustSpecItemDesc').prop('checked')) {
        $('#ShowCustSpecItemDesc').val('Y');
    }
    else {
        $('#ShowCustSpecItemDesc').val('N');
    }
}
function OnCheckedBomAvl() {
    if ($('#chkBomAvl').prop('checked')) {
        $('#ShowBomAvl').val('Y');
    }
    else {
        $('#ShowBomAvl').val('N');
    }
}
function OnCheckedHSN() {
    if ($('#chkHSN').prop('checked')) {
        $('#ShowHSN').val('Y');
    }
    else {
        $('#ShowHSN').val('N');
    }
}
function OnCheckedPendingAmt() {
    if ($('#chkPendingAmt').prop('checked')) {
        $('#ShowPendingAmt').val('Y');
    }
    else {
        $('#ShowPendingAmt').val('N');
    }
}
function OnCheckedUOM() {
    if ($('#chkUOM').prop('checked')) {
        $('#ShowUom').val('Y');
    }
    else {
        $('#ShowUom').val('N');
    }
}
function OnCheckedReferenceNo() {
    if ($('#chkReferenceNo').prop('checked')) {
        $('#ShowRefNumber').val('Y');
    }
    else {
        $('#ShowRefNumber').val('N');
    }
}
function OnCheckedTechspec() {
    if ($('#chkTechspec').prop('checked')) {
        $('#ShowTechSpec').val('Y');
    }
    else {
        $('#ShowTechSpec').val('N');
    }
}
function OnCheckedTechDesc() {
    if ($('#chkTechDesc').prop('checked')) {
        $('#ShowTechDesc').val('Y');
    }
    else {
        $('#ShowTechDesc').val('N');
    }
}
function OnCheckedweight() {
    if ($('#chkweight').prop('checked')) {
        $('#ShowWeight').val('Y');
    }
    else {
        $('#ShowWeight').val('N');
    }
}
function OnCheckedCustName() {
    if ($('#chkCustNm').prop('checked')) {
        $('#ShowCustName').val('Y');
    }
    else {
        $('#ShowCustName').val('N');
    }
}
function OnCheckedprice() {
    if ($('#chkPrice').prop('checked')) {
        $('#ShowPrice').val('Y');
    }
    else {
        $('#ShowPrice').val('N');
    }
}
function OnCheckedBomAvl() {
    if ($('#chkBomAvl').prop('checked')) {
        $('#ShowBomAvl').val('Y');
    }
    else {
        $('#ShowBomAvl').val('N');
    }
}
function OnCheckedChangeOEM() {
    if ($('#chkshowOEM').prop('checked')) {
        $('#ShowOEMNo').val('Y');
    }
    else {
        $('#ShowOEMNo').val('N');
    }
}
function OnCheckedremarks() {
    if ($('#chkremarks').prop('checked')) {
        $('#ShowRemarks').val('Y');
    }
    else {
        $('#ShowRemarks').val('N');
    }
}
function OnCheckedOrderedQuantity() {
    if ($('#chkOrderedQuantity').prop('checked')) {
        $('#ShowOrderedQuantity').val('Y');
    }
    else {
        $('#ShowOrderedQuantity').val('N');
    }
}
function PrintInvoice() {
    var arr = [];

    //var table = $('#datatable-buttons').DataTable();
    var table = $('#intimationtbl').DataTable();

    // Ye code har row (sabhi pages ki) check karega
    table.rows().every(function () {
        var list = {};
        var CurrentRow = $(this.node());
        if (CurrentRow.find("#IntimationCheck").is(":checked")) {
            list.so_no = CurrentRow.find("#hdn_so_no").text();
            list.so_dt = CurrentRow.find("#hdn_so_date").text();
            list.item_id = CurrentRow.find("#hdnItemId").val();
            arr.push(list);
        }
    });

    //$("#datatable-buttons >tbody >tr").each(function () {
    //    var list = {};
    //    var CurrentRow = $(this);
    //    if (CurrentRow.find("#IntimationCheck").is(":checked")) {
    //        list.so_no = CurrentRow.find("#hdn_so_no").text();
    //        list.so_dt = CurrentRow.find("#hdn_so_date").text();
    //        list.item_id = CurrentRow.find("#hdnItemId").val();
    //        arr.push(list);
    //    }
    //});
    var arrList = JSON.stringify(arr);
    $("#hdnIntimationList").val(arrList);
    $("#hdnCommand").text("Print");
    $('form').submit();
}