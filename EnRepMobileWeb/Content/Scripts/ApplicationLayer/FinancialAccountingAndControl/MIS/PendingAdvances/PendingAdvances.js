$(document).ready(function () {

    //    $(document).ready(function () {
    //    /*$(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" onclick="AccountRecevablPDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')*/
    //    $(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="PendingAdvancesCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    //});
});

function InsertRangeDetail() {
    $("#hdnCSVPrint").val('');
    if (RangeValidation() == false) {
        return false;
    }
    if (RangeValidation1() == false) {
        return false;
    }

    return true;
};
function RangeValidation() {
    var ErrorFlag = "N";
    var range1 = $("#range1").val();
    if (range1 == "" || range1 == "0" || range1 == null) {
        $("#vmrange1").css("display", "block");
        $("#range1").css("border-color", "red");
        $("#vmrange1").text($("#valueReq").text());

        ErrorFlag = "Y";
    }
    else {
        $("#vmrange1").css("display", "none");
        $("#range1").css("border-color", "#ced4da");
    }

    var range2 = $("#range2").val();
    if (range2 == "" || range2 == "0" || range2 == null) {
        $("#vmrange2").css("display", "block");
        $("#range2").css("border-color", "red");
        $("#vmrange2").text($("#valueReq").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange2").css("display", "none");
        $("#range2").css("border-color", "#ced4da");
    }

    var range3 = $("#range3").val();
    if (range3 == "" || range3 == "0" || range3 == null) {
        $("#vmrange3").css("display", "block");
        $("#range3").css("border-color", "red");
        $("#vmrange3").text($("#valueReq").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange3").css("display", "none");
        $("#range3").css("border-color", "#ced4da");
    }

    var range4 = $("#range4").val();
    if (range4 == "" || range4 == "0" || range4 == null) {
        $("#vmrange4").css("display", "block");
        $("#range4").css("border-color", "red");
        $("#vmrange4").text($("#valueReq").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange4").css("display", "none");
        $("#range4").css("border-color", "#ced4da");
    }

    var range5 = $("#range5").val();
    if (range5 == "" || range5 == "0" || range5 == null) {
        $("#vmrange5").css("display", "block");
        $("#range5").css("border-color", "red");
        $("#vmrange5").text($("#valueReq").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange5").css("display", "none");
        $("#range5").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function RangeValidation1() {
    var ErrorFlag = "N";
    var range1 = $("#range1").val();
    var range2 = $("#range2").val();
    var range3 = $("#range3").val();
    var range4 = $("#range4").val();
    var range5 = $("#range5").val();

    if (parseFloat(range1) >= parseFloat(range2)) {
        $("#vmrange1").css("display", "block");
        $("#range1").css("border-color", "red");
        $("#vmrange1").text($("#InvalidRange").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange1").css("display", "none");
        $("#range1").css("border-color", "#ced4da");
    }
    if (parseFloat(range2) >= parseFloat(range3)) {
        $("#vmrange2").css("display", "block");
        $("#range2").css("border-color", "red");
        $("#vmrange2").text($("#InvalidRange").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange2").css("display", "none");
        $("#range2").css("border-color", "#ced4da");
    }
    if (parseFloat(range3) >= parseFloat(range4)) {
        $("#vmrange3").css("display", "block");
        $("#range3").css("border-color", "red");
        $("#vmrange3").text($("#InvalidRange").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange3").css("display", "none");
        $("#range3").css("border-color", "#ced4da");
    }
    if (parseFloat(range4) >= parseFloat(range5)) {
        $("#vmrange4").css("display", "block");
        $("#range4").css("border-color", "red");
        $("#vmrange4").text($("#InvalidRange").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange4").css("display", "none");
        $("#range4").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function PendingAdvancesCSV() {
    debugger;
    var EntityType = $("#EntityType").val();
    $("#HdnEntityType").val($("#EntityType").val());
    $("#hdnCSVPrint").val("CsvPrint");
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function SearchAgingDetail() {
    try {
        $("#hdnCSVPrint").val("CsvPrint");
        var EntityType = $("#EntityType").val();
        $("#HdnEntityType").val(EntityType);
        var AsDate = $("#AsDate").val();
        var ReportType = "";
        if ($("#ReportTypeS").is(":checked")) {
            ReportType = "S";
        }
        if ($("#ReportTypeD").is(":checked")) {
            ReportType = "D";
        }
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/PendingAdvances/SearchPendingAdvancesDetail",
            data: {
                EntityType: EntityType,
                AsDate: AsDate,
                ReportType: ReportType,
            },
            success: function (data) {
                $('#Pend_table').html(data);
                $("#Pend_table").css("display", "block")
                $('.datatable-buttons5').addClass('rowhighlighttbl')
                hideLoader();
                //$(".btn.btn-default.buttons-pdf.buttons-html5.btn-sm.ap").remove();
                $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" onclick="AccountRecevablPDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')
                $(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountRecevablCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            },
            error: function OnError(xhr, errorType, exception) {
            }
        });
    } catch (err) {
        console.log("Trial Balance Error : " + err.message);
    }
}
function OnCliekAdvanceAmountIcon(e, type) {
    debugger
    var EntityType = $("#EntityType").val();
    var AsDate = $("#AsDate").val();
    var CurrentRow = $(e.target).closest("tr");
    var Acc_ID = CurrentRow.find("#acc_id").text();
    var Curr = CurrentRow.find("#Curr").text();
    var CurrId = CurrentRow.find("#Curr_id").text();
    var Entity_name = CurrentRow.find("#entityname").text();
    var Curr = CurrentRow.find("#Curr").text();

    var range = "";
    if (type === "r1") {
        range = CurrentRow.find("#pahdnr1").val();
    }
    if (type === "r2") {
        range = CurrentRow.find("#pahdnr2").val();
    }
    if (type === "r3") {
        range = CurrentRow.find("#pahdnr3").val();
    }
    if (type === "r4") {
        range = CurrentRow.find("#pahdnr4").val();
    }
    if (type === "r5") {
        range = CurrentRow.find("#pahdnr5").val();
    }
    if (type === "r6") {
        range = CurrentRow.find("#pahdnr6").val();
    }
    var lurange = range.split(',');
    if (lurange[1] == null) {
        lurange[1] = "10000";
    }
    var ReportType = "S";
    if ($("#ReportTypeS").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ReportTypeD").is(":checked")) {
        ReportType = "D";
    }
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/PendingAdvances/GetPendingAdvancesList",
            data: {
                Acc_ID: Acc_ID,
                EntityType: EntityType,
                AsDate: AsDate,
                lrange: lurange[0],
                urange: lurange[1],
                CurrId: CurrId,
                ReportType: ReportType
            },
            success: function (data) {
                $('#AdvancePaymentDetailsPopup').html(data);
                $("#EntityName").val(Entity_name);
                $("#Currency").val(Curr);
                cmn_apply_datatable("#TblAdvPmt");
            },
            error: function OnError(xhr, errorType, exception) {
            }
        });
    } catch (err) {
        debugger;
        console.log("Error : " + err.message);
    }
}