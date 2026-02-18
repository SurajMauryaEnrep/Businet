$(document).ready(function () {
    $('#Ddl_ItemName').select2();
    var fromDate = $('#txtFromdate').val();
    $('#txtTodate').attr('min', fromDate);
    Cmn_initializeMultiselect(['#ddlQcType']);
});

function validateToDate() {
    var fromDate = $('#txtFromdate').val();
    $('#txtTodate').attr('min', fromDate);
}

function BindItemDetailsList(e) {
    var clickdRow = $(e.target).closest('tr');
    var itemId = clickdRow.find("#itemId").text();
    var itemName = clickdRow.find("#itemName").text();
    var uomAlias = clickdRow.find("#uomAlias").text();
    var srcType = clickdRow.find('#hdQcType').text();
    //if (srcType == "Reworkable") {
    //    srcType="RWK"
    //}
    //if (srcType == "Production") {
    //    srcType="PRD"
    //}
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var txtFromdate = $("#txtFromdate").val();
    var txtTodate = $("#txtTodate").val();
    var qcType = $("#ddlQcType").val();
    $('#ProductName').val(itemName);
    $('#UOM').val(uomAlias);

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/QualityAnalysis/SearchQualityAnalysisDetailsByItemId",
        data: {
            itemId: itemId, srcType: srcType, fromDate: txtFromdate, toDate: txtTodate, DocId: DocumentMenuId
        },
        success: function (data) {
            $("#divQcDetails").html(data);

            /*cmn_apply_datatable("#tblqcDetails");*/
        }
    })
}

function SearchQualityAnalysisDetails() {
   
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var txtFromdate = $("#txtFromdate").val();
    var txtTodate = $("#txtTodate").val();
   // var qcType = $("#ddlQcType").val();
    var qcType = cmn_getddldataasstring("#ddlQcType");
    var itemId = $('#Ddl_ItemName').val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/QualityAnalysis/SearchQualityAnalysisDetails",
        data: {
            itemId: itemId, srcType: qcType, fromDate: txtFromdate, toDate: txtTodate,DocId: DocumentMenuId
        },
        success: function (data) {
            $("#divQADetails").html(data);

            /*cmn_apply_datatable("#tblqcDetails");*/
        }
    })

}

function SearchQualityAnalysisSummary() {
    debugger
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var txtFromdate = $("#txtFromdate").val();
    var txtTodate = $("#txtTodate").val();
    //var qcType = $("#ddlQcType").val();
    var qcType = cmn_getddldataasstring("#ddlQcType");
    var itemId = $('#Ddl_ItemName').val();


    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/QualityAnalysis/SearchQualityAnalysisSummary",
        data: {
            itemId: itemId, srcType: qcType, fromDate: txtFromdate, toDate: txtTodate, DocId: DocumentMenuId
        },
        success: function (data) {
            $("#divQAsummary").html(data);

            /*cmn_apply_datatable("#tblqcDetails");*/
        }
    })

}

function BtnSearchClick() {
    debugger;
    var period = $('#ddl_period').val();
    if (period == '1') {
        $('#vm_ddl_period').text('').css('display', 'none');
        $('#divQADetails').css('display', 'none');
        SearchQualityAnalysisSummary();
        $("#divQADetails").html('');
    }
    else if (period == '2') {
        $('#vm_ddl_period').text('').css('display', 'none');
        $('#divQADetails').css('display', 'block');
        SearchQualityAnalysisDetails();
        $("#divQAsummary").html('');

    }
    else {
        $('#vm_ddl_period').text('').css('display', 'block');
        $('#vm_ddl_period').text($("#valueReq").text());
    }
}