$(document).ready(function () {
    debugger;
    $("#datatable-buttons >tbody").on("dblclick", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var tmplt_id = clickedrow.find("#tmpltId").text();
        window.location.href = "/BusinessLayer/TermAndConditionTemplate/DblClickToDetail?tmplt_id=" + tmplt_id;
    })
    cmn_apply_datatable("#termandcondtion");

    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");
})
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
function SaveBtnCkick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    var ValidInfo = "N";
    if ($('#TemplateName').val() == "" || $('#TemplateName').val() == null) {
        ValidInfo = "Y";
        $('#SpanTemplateName').text($("#valueReq").text());
        $("#SpanTemplateName").css("display", "block");
        $("#TemplateName").css("border-color", "red");
    } else {
        $("#SpanTemplateName").css("display", "none");
        $("#TemplateName").css("border-color", "#ced4da");
    }
    if (ValidInfo == "Y") {
        return false;
    }
    if ($("#TaxCalculatorTbl >tbody >tr").length <= 0) {
        swal("", $("#TermsConditionsDetailsNotFound").text(), "warning");
        return false;
    }
    var TermCndsn = [];
    var FinalBranchDetail = [];
    var FinalTermAndConditionForDuplicate = [];

    TermCndsn = InsertTermCndsn();
    FinalBranchDetail = InsertBranchDetail();
    FinalTermAndConditionForDuplicate = InsertTermAndConditionForDuplicate();

    var TermAndCndsn = JSON.stringify(TermCndsn);
    var BranchDT = JSON.stringify(FinalBranchDetail);
    var TermAndConditionForDuplicate = JSON.stringify(FinalTermAndConditionForDuplicate);

    $("#TermsConditions").val(TermAndCndsn);
    $("#BranchDetail").val(BranchDT);
    $("#TermAndConditionForDuplicate").val(TermAndConditionForDuplicate);
    $("#TamplateStatus").prop("disabled", false);
    if (ValidInfo == "N") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
    }
}
function InsertTermAndConditionForDuplicate() {
    var TaxDetailList = new Array();
    if ($("#TaxCalculatorTbl >tbody >tr").length > 0) {
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var ItemList = {};
            debugger;
            ItemList.TermCndsn = currentRow.find("#cc_name1").text();
            TaxDetailList.push(ItemList);
        });
    }
    return TaxDetailList;
}
function InsertBranchDetail() {
    debugger;
    var BranchDetailList = new Array();
    if ($("#CustBrDetail >tbody >tr").length > 0) {
        $("#CustBrDetail >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var List = {};

            var brid = currentRow.find("#hdCustomerBranchId").val();
            List.brid = brid;
            if (currentRow.find("#cust_act_stat_" + brid).is(":checked")) {
                List.ActiveStatus = "Y";
            }
            else {
                List.ActiveStatus = "N";
            }
            BranchDetailList.push(List);
        });
    }
    return BranchDetailList;
}
function InsertTermCndsn() {
    debugger;
    var TaxDetailList = new Array();
    if ($("#TaxCalculatorTbl >tbody >tr").length > 0) {
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var ItemList = {};
            debugger;
            ItemList.TermCndsn = currentRow.find("#cc_name1").text();
            TaxDetailList.push(ItemList);
        });
    }
    return TaxDetailList;
}
function OnClickAddTermCnsdsn() {
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";
    if ($('#TermsAndConditions').val() == "" || $('#TermsAndConditions').val() == null) {
        ValidInfo = "Y";
        $('#SpanTermsAndConditions').text($("#valueReq").text());
        $("#SpanTermsAndConditions").css("display", "block");
        $("#TermsAndConditions").css("border-color", "red");
    } else {
        $("#SpanTermsAndConditions").css("display", "none");
        $("#TermsAndConditions").css("border-color", "#ced4da");
    }
    if (ValidInfo == "Y") {
        return false;
    }
    var Terms = $("#TermsAndConditions").val().trim();
    var TermsAndConditions = $("#TermsAndConditions").val().trim().toUpperCase();
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var TAndC = currentRow.find("#cc_name1").text().trim().toUpperCase();
        if (TermsAndConditions == TAndC) {
            document.getElementById("vmTermsAndConditions").innerHTML = $("#valueduplicate").text();
            $("#TermsAndConditions").css("border-color", "red");
            ValidInfo = "Y";
            //return false;
        }
    });
    if (ValidInfo == "Y") {
        return false;
    }
    var rowIdx = 0;
    var rowCount = $('#TaxCalculatorTbl >tbody >tr').length + 1;
    debugger;
    $('#TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
<td class="center">
   <i class="fa fa-trash red" aria-hidden="true" id="delBtnIcon" title="${$("#Span_Delete_Title").text()}" onclick="return functionConfirm(event)"></i>
   </td>
   <td class="center">
   <i type="submit" class="fa fa-edit" id="EditTermcondaction" onclick="EditCostCenter(event)" aria-hidden="true" title="${$("#Edit").text()}"></i>
   </td>
   <td class="sorting_1" id="ROWNO" >${rowCount}</td>
   <td id="cc_name1">${Terms}</td>
</tr>`);
    $("#TermsAndConditions").val(null)
}
function OnChangeTamplateName() {
    $("#SpanTemplateName").css("display", "none");
    $("#TemplateName").css("border-color", "#ced4da");
    document.getElementById("vmTemplateName").innerHTML = "";
}
function OnChangeTermsAndConditions() {
    $("#SpanTermsAndConditions").css("display", "none");
    $("#TermsAndConditions").css("border-color", "#ced4da");
    document.getElementById("vmTermsAndConditions").innerHTML = "";
}
function EditCostCenter(e) {
    debugger;
    $("#TaxCalculatorTbl >tbody >tr").on('click', "#EditTermcondaction", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            var HdnTermsAndConditions = clickedrow.children("#cc_name1").text();
            var Termcondaction = clickedrow.children("#cc_name1").text();
            $("#HdnTermsAndConditions").val(HdnTermsAndConditions);
            $("#TermsAndConditions").val(Termcondaction);
            $('#Btn_AddNew').css('display', 'none');
            $('#ResetTD').css('display', 'block');
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
}
function DeleteTermCondication(event) {
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
        if (isConfirm) {
            debugger
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            // return true;
        } else {
            return false;
        }
    });
    return false;
}
function ResetTaxDetails() {
    debugger;
    var ValidInfo = "N";
    if ($('#TermsAndConditions').val() == "" || $('#TermsAndConditions').val() == null) {
        ValidInfo = "Y";
        $('#SpanTermsAndConditions').text($("#valueReq").text());
        $("#SpanTermsAndConditions").css("display", "block");
        $("#TermsAndConditions").css("border-color", "red");
    } else {
        $("#SpanTermsAndConditions").css("display", "none");
        $("#TermsAndConditions").css("border-color", "#ced4da");
    }

    if (ValidInfo == "Y") {
        return false;
    }
    var TermsAndConditions = $("#TermsAndConditions").val().trim().toUpperCase();
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var TAndC = currentRow.find("#cc_name1").text().trim().toUpperCase();
        if (TermsAndConditions == TAndC) {
            document.getElementById("vmTermsAndConditions").innerHTML = $("#valueduplicate").text();
            $("#TermsAndConditions").css("border-color", "red");
            ValidInfo = "Y";
            //return false;
        }
    });
    if (ValidInfo == "Y") {
        return false;
    }
    debugger;
    var HdnTermsAndConditions = $("#HdnTermsAndConditions").val();
    $("#TaxCalculatorTbl > tbody > tr").each(function () {
        debugger;
        var currentRow = $(this);
        var TermsAndConditions = $("#TermsAndConditions").val();
        var HdnTermsAndConditions1 = currentRow.find("#cc_name1").text();
        if (HdnTermsAndConditions == HdnTermsAndConditions1) {
            debugger;
            currentRow.find("#cc_name1").text(TermsAndConditions);
            $('#Btn_AddNew').css('display', 'block');
            $('#ResetTD').css('display', 'none');
            $("#TermsAndConditions").val("");
        }
    });
}
function functionConfirm(e) {
    $("#TaxCalculatorTbl >tbody >tr").on('click', "#delBtnIcon", function (e) {
        debugger;
        var currentRow = $(this).closest('tr')
        var optionValue = currentRow.find("#HdnTermsAndConditions").text()
        //$('#Btn_AddNew').css('display', 'block');
        //$('#ResetTD').css('display', 'none');
        currentRow.remove();
        SerialNoAfterDelete();
    });
}
function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    $("#TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#ROWNO").text(SerialNo);
    });
};