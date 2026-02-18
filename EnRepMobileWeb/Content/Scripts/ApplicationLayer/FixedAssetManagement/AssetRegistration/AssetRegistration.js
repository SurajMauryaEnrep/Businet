////const QtyDecDigit = $("#QtyDigit").text();///Quantity
const ValDecDigit = $("#ValDigit").text();///Amount
$(document).ready(function () {
    $("#ddlAssetDescription").select2();
    $("#ddlAssetSerialNo").select2();
    $("#ddlAssetGroup").select2();
    $("#ddlAssetGroupCategory").select2();
    $("#ddlRequirementArea").select2();
    $("#ddlWorkingStatus").select2();
    $("#ddlAssignedRequirementArea").select2();
    var selectedValue = $('#ddlWorkingStatus').val();
    $('#Hdn_WorkingStatusId').val(selectedValue);

    $("#Tbl_list_FA_RA #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        var WF_status = $("#WF_status").val();
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var AssetRegId = clickedrow.children("#AssetRegId").text();
        if (AssetRegId != "" && AssetRegId != null) {
            // window.location.href = "/ApplicationLayer/AssetRegistration/AssetRegistrationAdd?regid=" + AssetRegId;
            window.location.href = "/ApplicationLayer/AssetRegistration/AddAssetRegistrationDetail?RegId=" + AssetRegId + "&ListFilterData=" + ListFilterData;
        }
    });
    $("#Tbl_list_FA_RA #datatable-buttons tbody").bind("click", function (e) {

    });
    GetAssignedRequirementArea();
    SetAssetWorkingDateRange();
    SetProcurementAsOnDateRange();
    SetAsOnDateRange();
    $("#ValiduptoReq").css("display", "none");
    //$("#lblValidUpto").text("");
});
function FilterARList() {
    debugger;
    try {
        var Group = $("#ddlAssetGroup").val();
        var Category = $("#ddlAssetGroupCategory").val();
        var RequirementArea = $("#ddlAssignedRequirementArea").val();
        var WorkingStatus = $("#ddlWorkingStatus").val();
        var Status = $("#ddlStatus").val();
        $("#Hdn_AssignedRequirementAreaId").val(RequirementArea);
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetRegistration/APListSearch",
            data: {
                Group: Group, Category: Category, RequirementArea: RequirementArea, WorkingStatus: WorkingStatus, Status: Status
            },
            success: function (data) {
                debugger;
                $('#Tbl_list_FA_RA').html(data);
                $('#ListFilterData').val(Group + ',' + Category + ',' + RequirementArea + ',' + WorkingStatus + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    }
    catch (err) {
        debugger;
        console.log("PQA Error : " + err.message);
    }
}
function functionConfirm(event) {
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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function SetProcurementAsOnDateRange() {
    let maxDate = new Date().toISOString().slice(0, 10);
    var pastDate = new Date();
    pastDate.setFullYear(pastDate.getFullYear() - 10); // 10 years back
    var minDate = pastDate.toISOString().split('T')[0];
    $('#OC_BillDate').attr('min', minDate);
    $('#OC_BillDate').attr('max', maxDate);
}
function SetAssetWorkingDateRange() {
    var today = new Date();
    // Minimum date = 10 years ago
    var pastDate = new Date();
    pastDate.setFullYear(today.getFullYear() - 10);
    var minDate = pastDate.toISOString().split('T')[0];

    // Maximum date = 10 years from today
    //var futureDate = new Date();
    //futureDate.setFullYear(today.getFullYear() + 10);
    //var maxDate = futureDate.toISOString().split('T')[0];
    var maxDate = today.toISOString().split('T')[0];
    // Set min and max attributes on the date input
    $('#AssetWorkingDate').attr('min', minDate);
    $('#AssetWorkingDate').attr('max', maxDate);
}
function SetBillDateRange() {
    var today = new Date();
    // Minimum date = 10 years ago

    var maxDate = today.toISOString().split('T')[0];
    // Set min and max attributes on the date input
    $('#PDBillDate').attr('max', maxDate);
}
function SetAsOnDateRange() {
    var today = new Date();
    var minDate = $("#CurrentYearFirstDate").val();
    var maxDate = today.toISOString().split('T')[0];
    $('#AsOn').attr('min', minDate);
    $('#AsOn').attr('max', maxDate);

}
function OnChangeddlAssetGroup(assetgrp) {
    debugger;
    var assetgrp = assetgrp.value;
    if (assetgrp == "" || assetgrp == null || assetgrp == undefined) {
        assetgrp = assetgrp;
    }
    $("#SpanddlAssetGroupErrorMsg").css("display", "none");
    $("[aria-labelledby='select2-ddlAssetGroup-container']").css("border-color", "#ced4da");
    $("#ddlAssetGroup").css("border-color", "#ced4da");
    $("#SpanCategoryErrorMsg").css("display", "none");
    $("#Category").css("border-color", "#ced4da");
    $("#SpanDepreciationPerErrorMsg").css("display", "none");
    $("#DepreciationPer").css("border-color", "#ced4da");
    $("#SpanDepreciationfreqErrorMsg").css("display", "none");
    $("#Depreciationfreq").css("border-color", "#ced4da");
    $("#SpanDepreciationMethodErrorMsg").css("display", "none");
    $("#DepreciationMethod").css("border-color", "#ced4da");
    $("#AssetLife").css("border-color", "#ced4da");
    $("#SpanAssetLifeErrorMsg").css("display", "none");

    if (assetgrp == "") {
        $("#ddlAssetGroup").val("");
        $('#SpanddlAssetGroupErrorMsg').text($("#valueReq").text());
        $("#SpanddlAssetGroupErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlAssetGroup-container']").css("border-color", "red");
        $("#Hdn_AssetsGroupId").val('');
    }
    else {
        $("#Hdn_AssetsGroupId").val($('#ddlAssetGroup').val().trim());
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/AssetRegistration/GetAssetCategoryDetails",
                data: { AssetGroupId: assetgrp },
                success: function (data) {
                    if (typeof data === 'string') {
                        try {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                $("#Hdn_AssetCategoryId").val(arr.Table[0].CategoryId);
                                $("#Category").val(arr.Table[0].CategoryName);
                                $("#DepreciationPer").val(arr.Table[0].dep_per);
                                $("#DepreciationMethod").val(arr.Table[0].DepreciationMethod);
                                $("#Depreciationfreq").val(arr.Table[0].dep_Frequency);
                                $("#lblValidUpto").text("(in " + arr.Table[0].dep_Frequency + ")");
                                var DepPer = $("#DepreciationPer").val();
                                debugger;
                                var AssetLife = Math.round(parseFloat((100 / DepPer) * 12));
                                $("#AssetLife").val(parseFloat(AssetLife).toFixed(0));
                                var dep_Frequency = $("#Depreciationfreq").val();
                                //if (dep_Frequency != "Monthly") {
                                //    // $("#AddDepreciationPer").prop("disabled", true);
                                //    $("#AddDepreciationPer").prop("readonly", true);
                                //    $("#ValidUpto").prop("disabled", true);
                                //}
                                //else {
                                //    //  $("#AddDepreciationPer").prop("disabled", false);
                                //    $("#AddDepreciationPer").prop("readonly", false);
                                //    $("#ValidUpto").prop("disabled", false);
                                //}
                            }
                            else {
                                $("#Hdn_AssetCategoryId").val("");
                                $("#Category").val("");
                                $("#DepreciationPer").val("");
                                $("#DepreciationMethod").val("");
                                $("#Depreciationfreq").val("");
                                // $("#AddDepreciationPer").prop("disabled", false);
                                $("#ValidUpto").prop("disabled", false);
                            }
                        } catch (e) {
                            console.error("Invalid JSON:", e);
                            return;
                        }
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX error:", error);
                }
            });

        } catch (err) {
            console.log("OnChangeddlAssetGroup Error: " + err.message);
        }
    }
}
function OnChangeddlAssetGroupList(assetgrp) {
    debugger;
    var assetgrp = assetgrp.value;
    if (assetgrp == "" || assetgrp == null || assetgrp == undefined) {
        assetgrp = assetgrp;
    }

    if (assetgrp == "") {
        $("#ddlAssetGroup").val("");
        $('#SpanddlAssetGroupErrorMsg').text($("#valueReq").text());
        $("#SpanddlAssetGroupErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlAssetGroup-container']").css("border-color", "red");
        $("#Hdn_AssetsGroupId").val('');
    }
    else {
        $("#Hdn_AssetsGroupId").val($('#ddlAssetGroup').val().trim());
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/AssetRegistration/GetAssetCategoryDetails",
                data: { AssetGroupId: assetgrp },
                success: function (data) {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        try {
                            if (arr.Table[0].CategoryId != null && arr.Table[0].CategoryId != 0) {
                                $("[aria-labelledby='select2-ddlAssetGroupCategory-container']").attr("style", "border-color: #ced4da;");
                                $('#ddlAssetGroupCategory').empty().append('<option value="0" selected="selected">All</option>');
                                $('#ddlAssetGroupCategory').append('<option value=' + arr.Table[0].CategoryId + '>' + arr.Table[0].CategoryName + '</option>');
                            } else {
                                $('#ddlAssetGroupCategory').empty().append('<option value="0" selected="selected">All</option>');
                            }
                        } catch (e) {
                            console.error("Invalid JSON:", e);
                            return;
                        }
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX error:", error);
                }
            });

        } catch (err) {
            console.log("OnChangeddlAssetGroupList Error: " + err.message);
        }
    }
}
function FloatValuePerOnly(el, evt) {
    $("#SpanTaxPercent").css("display", "none");
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    } else {
        return true;
    }
}
function RateFloatValueonly(el, evt) {
    //let RateDigit = import_ord_type ? "#ExpImpRateDigit" : "#RateDigit";//Added by Suraj on 22-10-2024
    if (Cmn_FloatValueonly(el, evt, RateDigit) == false) {
        return false;
    }
}
function OnChangeddlAssetDescription(ACID) {
    debugger;
    if ($('#ddlAssetDescription').val().trim() != '') {
        $('[aria-labelledby="select2-ddlAssetDescription-container"]').css("border-color", "#ced4da");
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "none");
        $("#Hdn_AssetItemsId").val($('#ddlAssetDescription').val().trim());
        GetAssetSerialNo();
    }
}
function GetAssetSerialNo() {
    try {
        var AssetDescriptionId = $('#Hdn_AssetItemsId').val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetRegistration/GetSerialNoJS",
            data: {
                AssetDescriptionId: AssetDescriptionId,
            },
            success: function (data) {
                debugger;
                if (data === 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                if (data && data !== "{}") {
                    if (typeof data === 'string') {
                        try {
                            data = JSON.parse(data);
                        } catch (e) {
                            console.error("Error parsing data:", e);
                        }
                    }
                    if (data && data.Table && data.Table.length > 0) {
                        $("#ddlAssetSerialNo").empty();
                        for (var i = 0; i < data.Table.length; i++) {
                            $('#ddlAssetSerialNo').append(`<option value="${data.Table[i].serial_noid}">${data.Table[i].serial_no}</option>`);
                        }
                    }
                    else {
                        $('#ddlAssetSerialNo').append(`<option value=0>---Select---</option>`);
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: ", error); // Log AJAX errors
            }
        });
    } catch (err) {
        console.log("GetAssetSerialNo Error: " + err.message); // Handle any JavaScript errors
    }
}
function OnChangeddlAssetSerialNo(ACID) {
    if ($('#ddlAssetSerialNo').val().trim() != '') {
        $('[aria-labelledby="select2-ddlAssetSerialNo-container"]').css("border-color", "#ced4da");
        $("#SpanSerialNumberErrorMsg").css("display", "none");
        $("#Hdn_SerialNumber").val($('#ddlAssetSerialNo').val().trim());
    }
}
function OnChangeAssetLabel(ACID) {
    if ($('#AssetLabel').val().trim() != '') {
        $('#AssetLabel').css("border-color", "#ced4da");
        $("#SpanAssetLabelErrorMsg").css("display", "none");
    }
}
//function OnChangeSerialNumber(ACID) {
//    if ($('#SerialNumber').val().trim() != '') {
//        $('#SerialNumber').css("border-color", "#ced4da");
//        $("#SpanSerialNumberErrorMsg").css("display", "none");
//    }
//}
function OnChangeAddDepreciationPer(ACID) {
    var AddDepreciationPer = parseFloat($('#AddDepreciationPer').val());
    if (isNaN(AddDepreciationPer)) {
        AddDepreciationPer = 0;
        $('#AddDepreciationPer').val(0);
    }
    if ($('#AddDepreciationPer').val().trim() != '') {
        $("#AddDepreciationPer").attr("style", "border-color: #ced4da;");
        $("#SpanAddDepreciationPerMsg").css("display", "none");
    }
    var DisAmt = $('#AddDepreciationPer').val().trim();
    if (DisAmt == "" || DisAmt == null) {
        DisAmt = 0;
    }
    if (DisAmt == 0) {
        $("#ValidUpto").val("0");
        $("#ValidUpto").prop("disabled", true);
        $("#ValiduptoReq").css("display", "none");
    }
    else if (DisAmt > 0) {
        $("#ValidUpto").prop("disabled", false);
        $("#ValiduptoReq").css("display", "");
    }
    $('#AddDepreciationPer').val(parseFloat(DisAmt).toFixed(ValDecDigit));
    debugger;
    //CalculateDisPercent($('#AddDepreciationPer').val());
}
function OnChangeProcuredValue(ACID) {
    debugger;
    var ProcuredValue = parseFloat($('#ProcuredValue').val());
    if (isNaN(ProcuredValue)) {
        ProcuredValue = 0;
        $('#ProcuredValue').val(0);
    }
    if ($('#ProcuredValue').val().trim() != '') {
        $("#ProcuredValue").attr("style", "border-color: #ced4da;");
        $("#SpanProcuredValueErrorMsg").css("display", "none");

        var ValDigit = $("#ValDigit").text();
        var debitAmt12 = $('#ProcuredValue').val();
        var debitAmt = cmn_ReplaceCommas(debitAmt12);

        if (AvoidDot(debitAmt) == false) {
            debitAmt = "";
            $('#ProcuredValue').val("");
        }
        else {
            // var debitAmt2 = parseFloat(debitAmt).toFixed(ValDigit);
            var debitAmt2 = toDecimal(cmn_ReplaceCommas(debitAmt), ValDigit);
            var debitAmt1 = cmn_addCommas(debitAmt2);
            $('#ProcuredValue').val(debitAmt1);
            $('#CurrentValue').val(parseFloat(0).toFixed(ValDecDigit));
        }
    }
    //var DisAmt = $('#ProcuredValue').val().trim();
    //if (DisAmt == "" || DisAmt == null) {
    //    DisAmt = 0;
    //}
    //var CurrentValue = parseFloat($('#CurrentValue').val());
    //if (CurrentValue == "" || isNaN(CurrentValue)) {
    //    CurrentValue = 0;
    //}
    //$('#ProcuredValue').val(parseFloat(DisAmt).toFixed(ValDecDigit));
    //if (CurrentValue == 0) {
    //    $('#CurrentValue').val(parseFloat(DisAmt).toFixed(ValDecDigit));
    //}

    //debugger;
    //var CurrentValueF = $('#CurrentValue').val();
    //var Diff = ProcuredValue - CurrentValueF;
    //$("#AccumulatedDepreciation").val(parseFloat(Diff).toFixed(ValDecDigit));
    //if (CurrentValue > ProcuredValue) {
    //    $("#CurrentValue").css("border-color", "red");
    //    $("#SpanCurrentValueErrorMsg").css("display", "block");
    //    // $("#SpanCurrentValueErrorMsg").text("Current Value can't be greater than Procured Value");
    //    swal("", $("#Currentvaluecantbegreaterthanprocuredvalue").text(), "warning");
    //}
    //else {
    //    $("#CurrentValue").attr("style", "border-color: #ced4da;");
    //    $("#SpanCurrentValueErrorMsg").css("display", "none");
    //}
}
function OnChangeValidUpto(ACID) {
    var ValidUpto = parseFloat($('#ValidUpto').val());
    if (isNaN(ValidUpto)) {
        ValidUpto = 0;
        $('#ValidUpto').val(0);
    }
    if ($('#ValidUpto').val().trim() != '') {
        $("#ValidUpto").attr("style", "border-color: #ced4da;");
        $("#SpanValidUptoErrorMsg").css("display", "none");
    }
    var DisAmt = $('#ValidUpto').val().trim();
    if (DisAmt == "" || DisAmt == null) {
        DisAmt = 0;
    }
    $('#ValidUpto').val(parseInt(DisAmt));
}
function OnChangeCurrentValue(ACID) {
    var CurrentValue = parseFloat($('#CurrentValue').val());
    if (isNaN(CurrentValue)) {
        CurrentValue = 0;
        $('#CurrentValue').val(0);
    }
    if ($('#CurrentValue').val().trim() != '') {
        $("#CurrentValue").attr("style", "border-color: #ced4da;");
        $("#SpanCurrentValueErrorMsg").css("display", "none");
    }
    var DisAmt = $('#CurrentValue').val().trim();
    if (DisAmt == "" || DisAmt == null) {
        DisAmt = 0;
    }
    $('#CurrentValue').val(parseFloat(DisAmt).toFixed(ValDecDigit));

    var ProcuredValue = parseFloat($('#ProcuredValue').val());
    if (ProcuredValue == "" || isNaN(ProcuredValue)) {
        ProcuredValue = 0;
    }
    //var CurrentValue = parseFloat($('#CurrentValue').val());
    var Diff = ProcuredValue - CurrentValue;
    $("#AccumulatedDepreciation").val(parseFloat(Diff).toFixed(ValDecDigit));
    if (CurrentValue > ProcuredValue) {
        $("#CurrentValue").css("border-color", "red");
        $("#SpanCurrentValueErrorMsg").css("display", "none");
        swal("", $("#Currentvaluecantbegreaterthanprocuredvalue").text(), "warning");
    }
}
function OnChangeAssetLife(ACID) {
    debugger;
    var AssetLife = parseFloat($('#AssetLife').val());
    if (isNaN(AssetLife)) {
        AssetLife = 0;
        $('#AssetLife').val(0);
    }
    if ($('#AssetLife').val().trim() != '') {
        $("#AssetLife").attr("style", "border-color: #ced4da;");
        $("#SpanAssetLifeErrorMsg").css("display", "none");
    }
    var DisAmt = $('#AssetLife').val().trim();
    if (DisAmt == "" || DisAmt == null) {
        DisAmt = 0;
    }
    $('#AssetLife').val(parseInt(DisAmt).toFixed(0));
}
function OnChangeProcurementDate(ACID) {
    if ($('#ProcurementDate').val().trim() != '') {
        $("#ProcurementDate").attr("style", "border-color: #ced4da;");
        $("#SpanProcurementDateErrorMsg").css("display", "none");
    }
}
function OnChangeSupplierName(ACID) {
    if ($('#SupplierName').val() != '') {
        $("#SupplierName").attr("style", "border-color: #ced4da;");
        $("#SpanSupplierNameErrorMsg").css("display", "none");
    }
}
function OnChangeBillNumber(ACID) {
    if ($('#BillNumber').val() != '') {
        $("#BillNumber").attr("style", "border-color: #ced4da;");
        $("#SpanBillNumberErrorMsg").css("display", "none");
    }
}
function OnChangeBillDate(ACID) {
    if ($('#BillDate').val() != '') {
        $("#BillDate").attr("style", "border-color: #ced4da;");
        $("#SpanBillDateErrorMsg").css("display", "none");
    }
}
function OnChangeAsOn(ACID) {
    if ($('#AsOn').val().trim() != '') {
        $("#AsOn").attr("style", "border-color: #ced4da;");
        $("#SpanAsOnErrorMsg").css("display", "none");
    }
}
function OnChangeddlAssignedRequirementArea(ACID) {
    if ($('#ddlAssignedRequirementArea').val().trim() != '') {
        $('[aria-labelledby="select2-ddlAssignedRequirementArea-container"]').css("border-color", "#ced4da");
        $("#SpanddlAssignedRequirementAreaErrorMsg").css("display", "none");
        $("#Hdn_AssignedRequirementAreaId").val($('#ddlAssignedRequirementArea').val().trim());
        var ratype = $("#ddlAssignedRequirementArea option:selected")[0].dataset.ratype
        $("#Hdn_AssignedRequirementAreaType").val(ratype);
    }
}
function OnChangeAssetWorkingDate(ACID) {
    if ($('#AssetWorkingDate').val().trim() != '') {
        $("#AssetWorkingDate").attr("style", "border-color: #ced4da;");
        $("#SpanAssetWorkingDateErrorMsg").css("display", "none");
        debugger;
        const finYrStartStr = $("#CurrentYearFirstDate").val();
        const finYrStart = new Date(finYrStartStr);  // Convert string to Date object
        const assetDate = new Date($('#AssetWorkingDate').val());
        const month = assetDate.getMonth();
        let depStartDate;
        if ($("#Depreciationfreq").val() == "Half-yearly" || $("#Depreciationfreq").val() == "Yearly") {
            if (finYrStart.getMonth() === 3) {  // If financial year starts on April 1st
                depStartDate = new Date(finYrStart);
                if (month >= 3 && month < 9) {
                    depStartDate = new Date(assetDate);
                    depStartDate.setDate(1);
                    depStartDate.setMonth(3);
                } else {//if (month >= 10 && month < 3) {  // Between January (0) and March (2)
                    if ((month == 0) || (month == 1) || (month == 2)) {
                        depStartDate = new Date(assetDate);
                        depStartDate.setFullYear(depStartDate.getFullYear() - 1);
                    }
                    else {
                        depStartDate = new Date(assetDate);
                    }
                    depStartDate.setDate(1);
                    depStartDate.setMonth(9);
                }
            }
            else if (finYrStart.getMonth() === 0) {  // If financial year starts on April 1st
                depStartDate = new Date(finYrStart);
                if (month >= 0 && month < 5) {
                    depStartDate = new Date(assetDate);
                    depStartDate.setDate(1);
                    depStartDate.setMonth(0);
                } else {
                    depStartDate = new Date(assetDate);
                    depStartDate.setDate(1);
                    depStartDate.setMonth(6);
                }
            }
            const formattedDepStartDate = depStartDate.toISOString().split('T')[0];
            $('#DepreciationStartDate').val(formattedDepStartDate);
        }
        else if ($("#Depreciationfreq").val() == "Monthly") {
            depStartDate = new Date(assetDate);
            depStartDate.setDate(1);
            depStartDate.setMonth(month);
            const formattedDepStartDate = depStartDate.toISOString().split('T')[0];
            $('#DepreciationStartDate').val(formattedDepStartDate);
        }
        else if ($("#Depreciationfreq").val() == "Quarterly") {
            depStartDate = new Date(assetDate);
            depStartDate.setDate(1);
            if (finYrStart.getMonth() === 3) {  // If financial year starts on April 1st
                if (month >= 3 && month < 6) {
                    depStartDate.setMonth(3);
                }
                else if (month >= 6 && month < 9) {
                    depStartDate.setMonth(6);
                }
                else if (month >= 9 && month < 12) {
                    depStartDate.setMonth(9);
                }
                else if (month >= 0 && month < 3) {
                    depStartDate.setMonth(0);
                }
            }
            else if (finYrStart.getMonth() === 0) {  // If financial year starts on Jan 1st
                if (month >= 0 && month < 3) {
                    depStartDate.setMonth(0);
                }
                else if (month >= 3 && month < 6) {
                    depStartDate.setMonth(3);
                }
                else if (month >= 6 && month < 9) {
                    depStartDate.setMonth(6);
                }
                else if (month >= 9 && month < 12) {
                    depStartDate.setMonth(9);
                }
            }

            const formattedDepStartDate = depStartDate.toISOString().split('T')[0];
            $('#DepreciationStartDate').val(formattedDepStartDate);
        }
    }
}
function formatDate(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are 0-indexed
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
}
function GetAssignedRequirementArea() {
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetRegistration/GetAssignedRequirementArea",
            success: function (data) {
                if (data === 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                if (data && data !== "{}") {
                    if (typeof data === 'string') {
                        try {
                            data = JSON.parse(data);
                        } catch (e) {
                            console.error("Error parsing data:", e);
                        }
                    }
                    if (data && data.Table && data.Table.length > 0) {
                        $("#ddlAssignedRequirementArea").empty();
                        var uniqueOptGroupId = "Textddl_" + new Date().getTime();
                        $('#ddlAssignedRequirementArea').append(`<optgroup class='def-cursor' id="${uniqueOptGroupId}" label='${$("#span_RequirementArea").text()}' data-ratype='${$("#span_Type").text()}'></optgroup>`);
                        // Append options to the dropdown
                        for (var i = 0; i < data.Table.length; i++) {
                            $('#' + uniqueOptGroupId).append(`<option data-invdate="${data.Table[i].acc_id}" data-ratype="${data.Table[i].RAType}" value="${data.Table[i].acc_id}">${data.Table[i].acc_name}</option>`);
                        }
                        $('#ddlAssignedRequirementArea').select2({
                            templateResult: function (data) {
                                var PInvDate = $(data.element).data('ratype');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-3 col-xs-6' + classAttr + '">' + PInvDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                            }
                        });
                        debugger;
                        var rq_id = $("#Hdn_AssignedRequirementAreaId").val();
                        if (rq_id != 'undefined' && rq_id != '0' && rq_id != '')
                            $('#ddlAssignedRequirementArea').val(rq_id).trigger("change");
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: ", error); // Log AJAX errors
            }
        });
    } catch (err) {
        console.log("GetAssignedRequirementArea Error: " + err.message); // Handle any JavaScript errors
    }
}
function CheckAR_Validations() {
    var ErrorFlag = "N";
    debugger;
    if ($("#ddlAssetDescription").val() === "0") {
        $('#SpanddlAssetDescriptionErrorMsg').text($("#valueReq").text());
        $("#ddlAssetDescription").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlAssetDescription-container']").css("border-color", "red");
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlAssetDescription-container']").css("border-color", "#ced4da");
        $("#ddlAssetDescription").css("border-color", "#ced4da");
    }
    if ($("#AssetLabel").val() == null || $("#AssetLabel").val() == "") {
        $("#AssetLabel").css("border-color", "Red");
        $('#SpanAssetLabelErrorMsg').text($("#valueReq").text());
        $("#SpanAssetLabelErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanAssetLabelErrorMsg").css("display", "none");
        $("#AssetLabel").css("border-color", "#ced4da");
    }
    if ($("#ddlAssetSerialNo").val() == "0") {
        $('#SpanSerialNumberErrorMsg').text($("#valueReq").text());
        $("#SpanSerialNumberErrorMsg").css("display", "block");
        $("#ddlAssetSerialNo").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlAssetSerialNo-container']").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSerialNumberErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlAssetSerialNo-container']").css("border-color", "#ced4da");
        $("#ddlAssetSerialNo").css("border-color", "#ced4da");
    }
    if ($("#ddlAssetDescription").val() === "0") {
        $('#SpanddlAssetDescriptionErrorMsg').text($("#valueReq").text());
        $("#ddlAssetDescription").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlAssetDescription-container']").css("border-color", "red");
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlAssetDescription-container']").css("border-color", "#ced4da");
        $("#ddlAssetDescription").css("border-color", "#ced4da");
    }
    if ($("#ddlAssetGroup").val() === "0") {
        $('#SpanddlAssetGroupErrorMsg').text($("#valueReq").text());
        $("#ddlAssetGroup").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlAssetGroup-container']").css("border-color", "red");
        $("#SpanddlAssetGroupErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanddlAssetGroupErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlAssetGroup-container']").css("border-color", "#ced4da");
        $("#ddlAssetGroup").css("border-color", "#ced4da");
    }
    //if ($("#Category").val() == "") {
    //    $('#SpanCategoryErrorMsg').text($("#valueReq").text());
    //    $("#SpanCategoryErrorMsg").css("display", "block");
    //    $("#Category").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanCategoryErrorMsg").css("display", "none");
    //    $("#Category").css("border-color", "#ced4da");
    //}
    //if ($("#ProcurementDate").val() == "") {
    //    $('#SpanProcurementDateErrorMsg').text($("#valueReq").text());
    //    $("#SpanProcurementDateErrorMsg").css("display", "block");
    //    $("#ProcurementDate").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanProcurementDateErrorMsg").css("display", "none");
    //    $("#ProcurementDate").css("border-color", "#ced4da");
    //}
    //if ($("#SupplierName").val() == "") {
    //    $('#SpanSupplierNameErrorMsg').text($("#valueReq").text());
    //    $("#SpanSupplierNameErrorMsg").css("display", "block");
    //    $("#SupplierName").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanSupplierNameErrorMsg").css("display", "none");
    //    $("#SupplierName").css("border-color", "#ced4da");
    //}
    //if ($("#BillNumber").val() == "") {
    //    $('#SpanBillNumberErrorMsg').text($("#valueReq").text());
    //    $("#SpanBillNumberErrorMsg").css("display", "block");
    //    $("#BillNumber").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanBillNumberErrorMsg").css("display", "none");
    //    $("#BillNumber").css("border-color", "#ced4da");
    //}
    //if ($("#BillDate").val() == "") {
    //    $('#SpanBillDateErrorMsg').text($("#valueReq").text());
    //    $("#SpanBillDateErrorMsg").css("display", "block");
    //    $("#BillDate").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanBillDateErrorMsg").css("display", "none");
    //    $("#BillDate").css("border-color", "#ced4da");
    //}
    if ($("#ProcuredValue").val() == "" || $("#ProcuredValue").val() == "0" || $("#ProcuredValue").val() == "0.00") {
        $('#SpanProcuredValueErrorMsg').text($("#valueReq").text());
        $("#SpanProcuredValueErrorMsg").css("display", "block");
        $("#ProcuredValue").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanProcuredValueErrorMsg").css("display", "none");
        $("#ProcuredValue").css("border-color", "#ced4da");
    }
    debugger;
    var ProcuredValue = parseFloat(cmn_ReplaceCommas($('#ProcuredValue').val()));
    var CurrentValue = parseFloat(cmn_ReplaceCommas($('#CurrentValue').val()));
    if (CurrentValue > ProcuredValue) {
        $("#CurrentValue").css("border-color", "red");
        $("#SpanCurrentValueErrorMsg").css("display", "block");
        //$("#SpanCurrentValueErrorMsg").text("Current Value can't be greater than Procured Value");
        swal("", $("#Currentvaluecantbegreaterthanprocuredvalue").text(), "warning");
        ErrorFlag = "Y";
    }
    //if ($("#CurrentValue").val() == "" || $("#CurrentValue").val() == "0" || $("#CurrentValue").val() == "0.00") {
    //    $('#SpanCurrentValueErrorMsg').text($("#valueReq").text());
    //    $("#SpanCurrentValueErrorMsg").css("display", "block");
    //    $("#CurrentValue").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanCurrentValueErrorMsg").css("display", "none");
    //    $("#CurrentValue").css("border-color", "#ced4da");
    //}
    //if ($("#AsOn").val() == "") {
    //    $('#SpanAsOnErrorMsg').text($("#valueReq").text());
    //    $("#SpanAsOnErrorMsg").css("display", "block");
    //    $("#AsOn").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanAsOnErrorMsg").css("display", "none");
    //    $("#AsOn").css("border-color", "#ced4da");
    //}
    if ($("#AssetLife").val() == "" || $("#AssetLife").val() == "0") {
        $('#SpanAssetLifeErrorMsg').text($("#valueReq").text());
        $("#SpanAssetLifeErrorMsg").css("display", "block");
        $("#AssetLife").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanAssetLifeErrorMsg").css("display", "none");
        $("#AssetLife").css("border-color", "#ced4da");
    }
    //if ($("#DepreciationPer").val() == "") {
    //    $('#SpanDepreciationPerErrorMsg').text($("#valueReq").text());
    //    $("#SpanDepreciationPerErrorMsg").css("display", "block");
    //    $("#DepreciationPer").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanDepreciationPerErrorMsg").css("display", "none");
    //    $("#DepreciationPer").css("border-color", "#ced4da");
    //}
    //if ($("#Depreciationfreq").val() == "") {
    //    $('#SpanDepreciationfreqErrorMsg').text($("#valueReq").text());
    //    $("#SpanDepreciationfreqErrorMsg").css("display", "block");
    //    $("#Depreciationfreq").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanDepreciationfreqErrorMsg").css("display", "none");
    //    $("#Depreciationfreq").css("border-color", "#ced4da");
    //}
    //if ($("#DepreciationMethod").val() == "") {
    //    $('#SpanDepreciationMethodErrorMsg').text($("#valueReq").text());
    //    $("#SpanDepreciationMethodErrorMsg").css("display", "block");
    //    $("#DepreciationMethod").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanDepreciationMethodErrorMsg").css("display", "none");
    //    $("#DepreciationMethod").css("border-color", "#ced4da");
    //}
    if ($("#AddDepreciationPer").val() == "") {// || $("#AddDepreciationPer").val() == "0" || $("#AddDepreciationPer").val() == "0.00") {
        $('#SpanAddDepreciationPerMsg').text($("#valueReq").text());
        $("#SpanAddDepreciationPerMsg").css("display", "block");
        $("#AddDepreciationPer").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanAddDepreciationPerMsg").css("display", "none");
        $("#AddDepreciationPer").css("border-color", "#ced4da");
    }
    debugger;


    var AddDepreciationPer = $('#AddDepreciationPer').val().trim();
    if (AddDepreciationPer == "" || AddDepreciationPer == null) {
        AddDepreciationPer = 0;
    }
    if (AddDepreciationPer > 0) {
        if ($("#ValidUpto").val() == "" || $("#ValidUpto").val() == "0") {
            $('#SpanValidUptoErrorMsg').text($("#valueReq").text());
            $("#SpanValidUptoErrorMsg").css("display", "block");
            $("#ValidUpto").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        else {
            $("#SpanValidUptoErrorMsg").css("display", "none");
            $("#ValidUpto").css("border-color", "#ced4da");
        }
    }
    if ($("#AssetWorkingDate").val() == "") {
        $('#SpanAssetWorkingDateErrorMsg').text($("#valueReq").text());
        $("#SpanAssetWorkingDateErrorMsg").css("display", "block");
        $("#AssetWorkingDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanAssetWorkingDateErrorMsg").css("display", "none");
        $("#AssetWorkingDate").css("border-color", "#ced4da");
    }

    if ($("#ddlAssignedRequirementArea").val() === "0") {
        $('#SpanddlAssignedRequirementAreaErrorMsg').text($("#valueReq").text());
        $("#ddlAssignedRequirementArea").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlAssignedRequirementArea-container']").css("border-color", "red");
        $("#SpanddlAssignedRequirementAreaErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanddlAssignedRequirementAreaErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlAssignedRequirementArea-container']").css("border-color", "#ced4da");
        $("#ddlAssignedRequirementArea").css("border-color", "#ced4da");
    }
    debugger;
    // Get the values from the inputs
    //var procurementDate = new Date(document.getElementById("ProcurementDate").value);
    //var workingDate = new Date(document.getElementById("AssetWorkingDate").value);
    //var asonDate = new Date(document.getElementById("AsOn").value);

    //// Check if both dates are valid
    //if (!isNaN(procurementDate) && !isNaN(workingDate)) {
    //    if (procurementDate > workingDate) {
    //        //swal("", "#ProcurmentDatecannotbeafterAssetWorkingdate", "warning");
    //        swal("", $("#ProcurmentDatecannotbeafterAssetWorkingdate").text(), "warning");
    //        ErrorFlag = "Y";
    //    }
    //}
    //if (!isNaN(procurementDate) && !isNaN(asonDate)) {
    //    if (procurementDate > asonDate) {
    //        // swal("", "#AsOnDatecannotbebeforeProcurementDate.", "warning");
    //        swal("", $("#AsOnDatecannotbebeforeProcurementDate").text(), "warning");
    //        ErrorFlag = "Y";
    //    }
    //}

    var AssetLifeValue = parseFloat($('#AssetLife').val());
    var ValidUptoValue = parseFloat($('#ValidUpto').val());
    if (ValidUptoValue > AssetLifeValue) {
        $("#ValidUpto").css("border-color", "red");
        $("#SpanValidUptoErrorMsg").css("display", "block");
        $("#SpanValidUptoErrorMsg").text("Additional Depreciation Validation");
        //swal("", $("#Currentvaluecantbegreaterthanprocuredvalue").text(), "warning");
        ErrorFlag = "Y";
    }
    debugger;
    if (ErrorFlag == "Y") {
        return false;
    }
    else {

        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
    }
}

function InsertAssetRegistration() {
    var HeaderValidation = CheckAR_Validations();
    if (HeaderValidation == false) {
        return false;
    }
    debugger;
    var OCDetail = [];
    OCDetail = GetFA_ProcurementDetails();
    var str_OCDetail = JSON.stringify(OCDetail);
    $('#hdn_PD_DetailList').val(str_OCDetail);
}

function GetFA_ProcurementDetails() {
    debugger;
    var PI_OCList = [];
    if ($("#tblAssetProcurementDetail >tbody >tr").length > 0) {
        $("#tblAssetProcurementDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var pd_src = "M";
            var BillNumber = currentRow.find("#BillNumber").text();
            var BillDate = currentRow.find("#BillDate").text();
            var Supplier = currentRow.find("#SupplierName").text();
            var GST = currentRow.find("#GSTNumber").text();
            var CurrencyId = currentRow.find("#CurrencyId").text();
            var PurchasePrice = cmn_ReplaceCommas(currentRow.find("#PurchasePrice").text());
            var TaxAmount = cmn_ReplaceCommas(currentRow.find("#TaxAmount").text());
            var OtherCharges = cmn_ReplaceCommas(currentRow.find("#OtherCharges").text());
            var TotalCost = cmn_ReplaceCommas(currentRow.find("#TotalCost").text());
            var CapitalizedValue = 0;// currentRow.find("#CaptAmount").text();
            PI_OCList.push({
                src_type: pd_src, billno: BillNumber, bill_dt: BillDate, supplier: Supplier, supplierid: 0, gst: GST, currencyid: CurrencyId
                , purchase_price: PurchasePrice, taxamount: TaxAmount, othercharges: OtherCharges, totalcost: TotalCost, capVal: CapitalizedValue
            });
        });
    }
    return PI_OCList;
};
function ResetDepreciationHistory() {
    $('#AssetDescriptionHis').val("");
    $('#AssetLabelHis').val("");
    $('#SerialNumberHis').val("");
    $('#ProcuredValueHis').val("");
}
function AssetDepreciationHistory(el, evt) {
    debugger;
    var AssetDescriptionHis = $("#ddlAssetDescription option:selected").text();//$("#AssetDescription").val()
    var AssetLabelHis = $("#AssetLabel").val();
    var SerialNumberHis = $("#ddlAssetSerialNo option:selected").text();//$("#SerialNumber").val();
    var ProcuredValueHis = $("#ProcuredValue").val();

    ResetDepreciationHistory();
    if (AssetDescriptionHis != null && AssetDescriptionHis != "" && AssetDescriptionHis != "0") {
        $('#AssetDescriptionHis').val(AssetDescriptionHis);
        $('#AssetLabelHis').val(AssetLabelHis);
        $('#SerialNumberHis').val(SerialNumberHis);

        if (ProcuredValueHis != "" && parseFloat(ProcuredValueHis) != 0 && ProcuredValueHis != null) {
            $('#ProcuredValueHis').val(ProcuredValueHis);
        }
        else {
            $("#ProcuredValueHis").val("");
        }
        //$("#DPHListTbody >tbody >tr").remove();
        $("#DPHListTbody tbody tr").remove();
        var rowIdx = 0;
        var Count = $("#DepreciationHistoryDetailsTbl tbody tr").length;
        if (Count != null && Count != 0) {
            if (Count > 0) {
                //  $("#DPHListTbody >tbody >tr").remove();
                $("#DepreciationHistoryDetailsTbl tbody tr").each(function () {
                    var SaveBatchRow = $(this);
                    var SRowID = SaveBatchRow.find("#RowSNo").val();
                    var Src_Doc_no = SaveBatchRow.find("#Src_Doc_no").val();
                    var Src_Doc_date = SaveBatchRow.find("#Src_Doc_date").val();
                    var DHcurr_val = SaveBatchRow.find("#DHcurr_val").val();
                    var DHdep_val = SaveBatchRow.find("#DHdep_val").val();
                    var DHAdddep_val = SaveBatchRow.find("#DHAdddep_val").val();
                    var DHrevised_val = SaveBatchRow.find("#DHrevised_val").val();
                    var DHason_date_val = SaveBatchRow.find("#DHAsonDt").val();
                    if (Src_Doc_no != null && Src_Doc_no != "") {
                        $('#DPHListTbody tbody').append(`<tr id="R${++rowIdx}">
                            <td id="RowSNo" >${SRowID}</td>
                            <td id="Src_Doc_no" >${Src_Doc_no}</td>
                            <td>${Src_Doc_date}</td>
                            <td id="DHcurr_val" class="num_right">${DHdep_val}</td>
                            <td id="DHAdddep_val" class="num_right">${DHAdddep_val}</td>
                            <td id="DHrevised_val" class="num_right">${DHrevised_val}</td>
                            <td id="DHAsonDt">${DHason_date_val}</td>
                            </tr>`);
                    }
                    /* <td id="DHcurr_val" class="num_right">${DHcurr_val}</td>*/
                });
            }
        }
    }
}
function TransferDetail() {
    debugger;
    var AssetDescriptionTD = $('#ddlAssetDescription').val().trim();
    var AssetLabelTD = $("#AssetLabel").val();
    var SerialNumberTD = $('#ddlAssetSerialNo').val().trim()
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/AssetRegistration/GetAssetTransferDetails",
        data: {
            AssetDescriptionTD: AssetDescriptionTD, SerialNumberTD: SerialNumberTD,
        },
        success: function (data) {
            $("#TransferDetailDetailPartial").html(data);
            //replaceDatatablesIds("tbl_AsssetHistory");
            cmn_apply_datatable("#tblAssetTransferDetail");
            $('#AssetDescriptionTD').val($("#ddlAssetDescription option:selected").text());
            $('#AssetLabelTD').val(AssetLabelTD);
            $('#SerialNumberTD').val(SerialNumberTD);
        }
    })
}
function ProcurementDetail() {
    debugger;
    var AssetDescriptionPD = $('#ddlAssetDescription').val().trim();
    var AssetLabelPD = $("#AssetLabel").val();
    var SerialNumberPD = $('#ddlAssetSerialNo').val().trim();
    var CurrId = $('#Hdn_CurrecyID').val().trim();
    var DisableFlag = $("#txtdisable").val();
    var NewArr = new Array();
    const ValDecDigit = $("#ValDigit").text();

    $("#tblAssetProcurementDetail > tbody > tr").each(function () {
        var currentrow = $(this);
        var List = {};
        List.hdnSrno = currentrow.find("#Srno").text();
        List.BillNumber = currentrow.find("#BillNumber").text();
        List.BillDate = currentrow.find("#BillDate").text();
        List.BillDt = currentrow.find("#BillDt").text();
        List.Supplier = currentrow.find("#SupplierName").text();
        List.GST = currentrow.find("#GSTNumber").text();
        List.Currency = currentrow.find("#Currency").text();
        List.CurrencyId = currentrow.find("#CurrencyId").text();
        List.PurchasePrice = parseFloat(cmn_ReplaceCommas(currentrow.find("#PurchasePrice").text())).toFixed(ValDecDigit);
        List.TaxAmount = parseFloat(cmn_ReplaceCommas(currentrow.find("#TaxAmount").text())).toFixed(ValDecDigit);
        List.OtherCharges = parseFloat(cmn_ReplaceCommas(currentrow.find("#OtherCharges").text())).toFixed(ValDecDigit);
        List.TotalCost = parseFloat(cmn_ReplaceCommas(currentrow.find("#TotalCost").text())).toFixed(ValDecDigit);
        List.CapitalizedValue = parseFloat(cmn_ReplaceCommas(currentrow.find("#CaptAmount").text())).toFixed(ValDecDigit);
        NewArr.push(List);
    });
    if (SerialNumberPD == "0") {
        $("#btnProcurementDetail").attr("data-target", "")
        return false;
    } else {
        $("#btnProcurementDetail").attr("data-target", "#AssetProcrumentDetail")
    }
    //else {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/AssetRegistration/GetAssetProcurementDetails",
        data: {
            AssetDescriptionTD: AssetDescriptionPD, DisableFlag: DisableFlag, PD_rowdata: JSON.stringify(NewArr),
        },
        success: function (data) {
            $("#ProcrumentDetailPartial").html(data);
            //replaceDatatablesIds("tbl_AsssetHistory");
            // cmn_apply_datatable("#tblAssetProcurementDetail");
            $('#AssetDescriptionPD').val($("#ddlAssetDescription option:selected").text());
            $('#AssetLabelPD').val(AssetLabelPD);
            $('#SerialNumberPD').val(SerialNumberPD);
            //$("#curr").val(CurrId);
            $("#PDBillNumber").focus();
        }
    })
    //}
}
function AmountFloatQty(el, evt) {
    let QtyDecDigit = "#QtyDigit";
    if (Cmn_FloatValueonly(el, evt, QtyDecDigit) == false) {
        return false;
    }
    return true;
}

function CheckPD_Validations() {
    var ErrorFlag = "N";
    if ($("#PDBillNumber").val() == null || $("#PDBillNumber").val() == "") {
        $('#SpanPDBillNumberErrorMsg').text($("#valueReq").text());
        $("#PDBillNumber").css("border-color", "Red");
        $("#SpanPDBillNumberErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanPDBillNumberErrorMsg").css("display", "none");
        $("#PDBillNumber").css("border-color", "#ced4da");
    }
    if ($("#PDBillDate").val() == null || $("#PDBillDate").val() == "") {
        $('#SpanPDBillDateErrorMsg').text($("#valueReq").text());
        $("#PDBillDate").css("border-color", "Red");
        $("#SpanPDBillDateErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanPDBillDateErrorMsg").css("display", "none");
        $("#PDBillDate").css("border-color", "#ced4da");
    }
    if ($("#PDSupplierName").val() == "") {
        $('#SpanPDSupplierNameErrorMsg').text($("#valueReq").text());
        $("#SpanPDSupplierNameErrorMsg").css("display", "block");
        $("#PDSupplierName").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanPDSupplierNameErrorMsg").css("display", "none");
        $("#PDSupplierName").css("border-color", "#ced4da");
    }
    //if ($("#PDGSTNumber").val() == "") {
    //    $('#SpanPDGSTNumberErrorMsg').text($("#valueReq").text());
    //    $("#SpanPDGSTNumberErrorMsg").css("display", "block");
    //    $("#PDGSTNumber").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanPDGSTNumberErrorMsg").css("display", "none");
    //    $("#PDGSTNumber").css("border-color", "#ced4da");
    //}

    //if ($("#PDPurchasePrice").val() == "" || $("#PDPurchasePrice").val() == "0" || $("#PDPurchasePrice").val() == "0.00") {
    if ($("#PDPurchasePrice").val() == "") {
        $('#SpanPDPurchasePriceErrorMsg').text($("#valueReq").text());
        $("#SpanPDPurchasePriceErrorMsg").css("display", "block");
        $("#PDPurchasePrice").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanPDPurchasePriceErrorMsg").css("display", "none");
        $("#PDPurchasePrice").css("border-color", "#ced4da");
    }

    //if ($("#PDTaxAmount").val() == "" || $("#PDTaxAmount").val() == "0" || $("#PDTaxAmount").val() == "0.00") {
    if ($("#PDTaxAmount").val() == "") {
        $('#SpanPDTaxAmountErrorMsg').text($("#valueReq").text());
        $("#SpanPDTaxAmountErrorMsg").css("display", "block");
        $("#PDTaxAmount").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanPDTaxAmountErrorMsg").css("display", "none");
        $("#PDTaxAmount").css("border-color", "#ced4da");
    }
    //if ($("#PDOtherCharges").val() == "" || $("#PDOtherCharges").val() == "0" || $("#PDOtherCharges").val() == "0.00") {
    if ($("#PDOtherCharges").val() == "") {
        $('#SpanPDOtherChargesErrorMsg').text($("#valueReq").text());
        $("#SpanPDOtherChargesErrorMsg").css("display", "block");
        $("#PDOtherCharges").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanPDOtherChargesErrorMsg").css("display", "none");
        $("#PDOtherCharges").css("border-color", "#ced4da");
    }
    if ($("#PDTotalCost").val() == "" || $("#PDTotalCost").val() == "0" || $("#PDTotalCost").val() == "0.00") {
        $('#SpanPDTotalCostErrorMsg').text($("#valueReq").text());
        $("#SpanPDTotalCostErrorMsg").css("display", "block");
        $("#PDTotalCost").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanPDTotalCostErrorMsg").css("display", "none");
        $("#PDTotalCost").css("border-color", "#ced4da");
    }
    //if (parseFloat($("#PDTotalCost").val()) !==
    //    (
    //        parseFloat($("#PDOtherCharges").val()) +
    //        parseFloat($("#PDTaxAmount").val()) +
    //        parseFloat($("#PDPurchasePrice").val())
    //    )) {
    //    $('#SpanPDTotalCostErrorMsg').text($("#span_InvalidPrice").text());
    //    $("#SpanPDTotalCostErrorMsg").css("display", "block");
    //    $("#PDTotalCost").css("border-color", "Red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#SpanPDTotalCostErrorMsg").css("display", "none");
    //    $("#PDTotalCost").css("border-color", "#ced4da");
    //}
    debugger;
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function AddProcurementDetail() {
    var HeaderValidation = CheckPD_Validations();
    if (HeaderValidation == false) {
        return false;
    }
    ProcurementDetailAdd();
}
function ProcurementDetailAdd() {
    debugger;
    var Flag = 'N';
    var BillNumber = $("#PDBillNumber").val().trim();
    var BillDate = $("#PDBillDate").val();
    var Supplier = $("#PDSupplierName").val();
    var GST = $("#PDGSTNumber").val();
    var Currency = $("#curr").val();
    var CurrencyId = $("#currId").val();
    var PurchasePrice = $("#PDPurchasePrice").val();
    var TaxAmount = $("#PDTaxAmount").val();
    var OtherCharges = $("#PDOtherCharges").val();
    var TotalCost = $("#PDTotalCost").val();
    var CapitalizedValue = $("#PDCapitalizedValue").val();
    var rowCount = $('#tblAssetProcurementDetail >tbody >tr').length;
    var rowspn = parseInt(0);

    $("#tblAssetProcurementDetail >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        if (rowCount == 0) {
            rowspn = 1;
        }
    });
    ProcurementDetailAddRows(BillNumber, BillDate, Supplier, GST, Currency, CurrencyId, PurchasePrice, TaxAmount, OtherCharges, TotalCost, CapitalizedValue);
    debugger;
    PDClearControl(); ProcVal();
}
function ProcurementDetailAddRows(BillNumber, BillDate, Supplier, GST, Currency, CurrencyId, PurchasePrice, TaxAmount, OtherCharges, TotalCost, CapitalizedValue) {
    var ccrows = "";
    var len = $("#tblAssetProcurementDetail tr").length;
    if (len == 0) {
        var FCCAmount1 = parseFloat(0);
    }
    var SrnoCounter = len > 0 ? len : 1;
    /*end code add by Hina Sharma on 04-07-2025 to show total amount*/
    var Span_Delete_Title = $("#Span_Delete_Title").text();
    var isDuplicate = false;
    $("#tblAssetProcurementDetail tr").each(function () {
        var existingBillNumber = $(this).find("#BillNumber").text();  // Find the BillNumber in the existing rows
        if (existingBillNumber === BillNumber) {
            isDuplicate = true;  // Duplicate found
            return false;  // Exit the loop
        }
    });
    if (isDuplicate) {
        // alert();
        $("#SpanAddErrorMsg").val("Duplicate Bill Number detected! Please enter a unique Bill Number.");
        $("#PDBillNumber").css("border-color", "Red");
        $("#SpanAddErrorMsg").css("display", "block");
        return;
    } else {
        ccrows += `<tr id='tr'>
                        <td width="20" class="red center bom_width_td"> <i class="fa fa-trash" aria-hidden="true" title="${Span_Delete_Title}" onclick="PD_DeleteChild(this)"></i></td>
                        <td class="bom_width_td"><i class="fa fa-edit" aria-hidden="true" id='hdn_editRowId' title="" onclick ="PD_editRow(event)"></i></td>
                        <td id="Srno">${SrnoCounter}</td>
                        <td id="BillNumber">${BillNumber}</td>
                        <td id="BillDate">${BillDate}</td>
                        <td id="SupplierName">${Supplier}</td>
                        <td id="GSTNumber">${GST}</td>
                        <td id="Currency" class="center">${Currency}</td>
                        <td id="CurrencyId" class="center" style="display:none">${CurrencyId}</td>
                        <td id="PurchasePrice" class="num_right">${PurchasePrice}</td>
                        <td id="TaxAmount" class="num_right">${TaxAmount}</td>
                        <td id="OtherCharges" class="num_right">${OtherCharges}</td>
                        <td id="TotalCost" class="num_right">${TotalCost}</td>
                        <td id="CapitalizedValue" class="num_right" style="display: none">${CapitalizedValue}</td>
            </tr>`
        $("#tblAssetProcurementDetail").append(ccrows);
        SrnoCounter++;
    }
}

function PD_DeleteChild(e) {
    var $tbody = $('#tblAssetProcurementDetail tbody');
    var rowCount = $tbody.find('tr').length;
    if (rowCount === 1) {
        $tbody.find('tr').remove();
    } else {
        $(e).closest('tr').remove();
    }
    ProcVal();
};
function PDClearControl() {
    $("#PDBillNumber").val("").prop("disabled", false);
    $("#PDBillDate").val("");
    $("#PDSupplierName").val("");
    $("#PDGSTNumber").val("");
    $("#ddl_CC_Name option:selected").text();
    $("#PDPurchasePrice").val("");
    $("#PDTaxAmount").val("");
    $("#PDOtherCharges").val("");
    $("#PDTotalCost").val("");
    $("#PDCapitalizedValue").val("");
    $("#ProcrumentDetailPartial #divUpdate").css("display", "none");
    $("#ProcrumentDetailPartial #divAdd").css("display", "block");
}
function PD_editRow(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var BillNumber = clickedrow.find("#BillNumber").text();
    var BillDate = clickedrow.find("#BillDt").text();
    var SupplierName = clickedrow.find("#SupplierName").text();
    var GSTNumber = clickedrow.find("#GSTNumber").text();
    var ddlCst = clickedrow.find("#Currency").text();
    var PurchasePrice = clickedrow.find("#PurchasePrice").text();
    var TaxAmount = clickedrow.find("#TaxAmount").text();
    var OCAmount = clickedrow.find("#OtherCharges").text();
    var TotalCost = clickedrow.find("#TotalCost").text();
    var CaptAmount = clickedrow.find("#CapitalizedValue").text();
    var currentrow = clickedrow.closest('table').closest('tr');
    $("#PDBillNumber").val(BillNumber).prop("disabled", true);
    $("#PDBillDate").val(BillDate);
    $("#PDSupplierName").val(SupplierName);
    $("#PDGSTNumber").val(GSTNumber);
    $("#PDPurchasePrice").val(PurchasePrice);
    $("#PDTaxAmount").val(TaxAmount);
    $("#PDOtherCharges").val(OCAmount);
    $("#PDTotalCost").val(TotalCost);
    $("#PDCapitalizedValue").val(CaptAmount);
    $("#ProcrumentDetailPartial #divUpdate").css("display", "block");
    $("#ProcrumentDetailPartial #divAdd").css("display", "none");
}

function UpdateProcurementDetail(e) {
    debugger;
    var BillNumber = $("#PDBillNumber").val();
    var BillDate = $("#PDBillDate").val();
    var Supplier = $("#PDSupplierName").val();
    var GST = $("#PDGSTNumber").val();
    var Currency = $("#curr").val();
    var CurrencyId = $("#currId").val();
    var PurchasePrice = $("#PDPurchasePrice").val();
    var TaxAmount = $("#PDTaxAmount").val();
    var OtherCharges = $("#PDOtherCharges").val();
    var TotalCost = $("#PDTotalCost").val();
    var CapitalizedValue = $("#PDCapitalizedValue").val();

    //$("#tblAssetProcurementDetail > tbody > tr").each(function () {
    //    var currentrow = $(this);
    //    var TblBillNumber = currentrow.find("#BillNumber").text();

    $("#tblAssetProcurementDetail tbody > tr").each(function () {
        var CurrRow = $(this);
        var TblBillNumber = CurrRow.find("#BillNumber").text();
        if (TblBillNumber == BillNumber) {
            CurrRow.find("#BillDate").text(BillDate);
            CurrRow.find("#SupplierName").text(Supplier);
            CurrRow.find("#GSTNumber").text(GST);
            CurrRow.find("#Currency").text(Currency);
            CurrRow.find("#CurrencyId").text(CurrencyId);
            CurrRow.find("#PurchasePrice").text(cmn_addCommas(PurchasePrice));
            CurrRow.find("#TaxAmount").text(cmn_addCommas(TaxAmount));
            CurrRow.find("#OtherCharges").text(cmn_addCommas(OtherCharges));
            CurrRow.find("#TotalCost").text(cmn_addCommas(TotalCost));
            CurrRow.find("#CapitalizedValue").text(cmn_addCommas(CapitalizedValue));
            //CurrRow.find("#hdntbl_CstAmt").val(cmn_addCommas(CC_Amount));
        }
    });
    //});
    PDClearControl();
    ProcVal();
}

function OnChangePDBillNumber(ACID) {
    if ($('#PDBillNumber').val() != '') {
        $("#PDBillNumber").attr("style", "border-color: #ced4da;");
        $("#SpanPDBillNumberErrorMsg").css("display", "none");
    }
}
function OnChangePDBillDate(ACID) {
    if ($('#PDBillDate').val() != '') {
        $("#PDBillDate").attr("style", "border-color: #ced4da;");
        $("#SpanPDBillDateErrorMsg").css("display", "none");
    }
}
function OnChangePDSupplierName(ACID) {
    if ($('#PDSupplierName').val() != '') {
        $("#PDSupplierName").attr("style", "border-color: #ced4da;");
        $("#SpanPDSupplierNameErrorMsg").css("display", "none");
    }
}
function OnChangePDGSTNumber(ACID) {
    if ($('#PDGSTNumber').val() != '') {
        $("#PDGSTNumber").attr("style", "border-color: #ced4da;");
        $("#SpanPDGSTNumberErrorMsg").css("display", "none");
    }
}
function OnChangePDPurchasePrice(ACID) {
    if ($('#PDPurchasePrice').val() != '') {
        $("#PDPurchasePrice").attr("style", "border-color: #ced4da;");
        $("#SpanPDPurchasePriceErrorMsg").css("display", "none");
        var total = (parseFloat(cmn_ReplaceCommas($("#PDOtherCharges").val())) || 0) + (parseFloat(cmn_ReplaceCommas($("#PDTaxAmount").val())) || 0) + (parseFloat(cmn_ReplaceCommas($("#PDPurchasePrice").val())) || 0);
        $("#PDTotalCost").attr("style", "border-color: #ced4da;");
        $("#SpanPDTotalCostErrorMsg").css("display", "none");
        $("#PDTotalCost").val(total.toFixed(2));

        //Total Cost
        var TotalCostAmt = cmn_ReplaceCommas($('#PDTotalCost').val());
        if (AvoidDot(TotalCostAmt) == false) {
            TotalCostAmt = "";
            $('#PDTotalCost').val("");
        }
        else {
            $('#PDTotalCost').val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(TotalCostAmt), ValDigit)));
        }
        //PurchasePrice
        var PurchasePriceAmt = cmn_ReplaceCommas($('#PDPurchasePrice').val());
        if (AvoidDot(PurchasePriceAmt) == false) {
            PurchasePriceAmt = "";
            $('#PDPurchasePrice').val("");
        }
        else {
            $('#PDPurchasePrice').val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(PurchasePriceAmt), ValDigit)));
        }
    }
}
function OnChangePDTaxAmount(ACID) {
    if ($('#PDTaxAmount').val() != '') {
        $("#PDTaxAmount").attr("style", "border-color: #ced4da;");
        $("#SpanPDTaxAmountErrorMsg").css("display", "none");
        var total = (parseFloat(cmn_ReplaceCommas($("#PDOtherCharges").val())) || 0) + (parseFloat(cmn_ReplaceCommas($("#PDTaxAmount").val())) || 0) + (parseFloat(cmn_ReplaceCommas($("#PDPurchasePrice").val())) || 0);
        $("#PDTotalCost").val(total.toFixed(2));
        $("#PDTotalCost").attr("style", "border-color: #ced4da;");
        $("#SpanPDTotalCostErrorMsg").css("display", "none");
        var TotalCostAmt = cmn_ReplaceCommas($('#PDTotalCost').val());
        if (AvoidDot(TotalCostAmt) == false) {
            TotalCostAmt = "";
            $('#PDTotalCost').val("");
        }
        else {
            $('#PDTotalCost').val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(TotalCostAmt), ValDigit)));
        }

        //tax Amount
        var TaxAmount = cmn_ReplaceCommas($('#PDTaxAmount').val());
        if (AvoidDot(TaxAmount) == false) {
            //TaxAmount = "";
            $('#PDTaxAmount').val(TaxAmount);
        }
        else {
            $('#PDTaxAmount').val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(TaxAmount), ValDigit)));
        }
    }
}
function OnChangePDOtherCharges(ACID) {
    debugger;
    if ($('#PDOtherCharges').val() != '') {
        $("#PDOtherCharges").attr("style", "border-color: #ced4da;");
        $("#SpanPDOtherChargesErrorMsg").css("display", "none");
        var total = (parseFloat(cmn_ReplaceCommas($("#PDOtherCharges").val())) || 0) + (parseFloat(cmn_ReplaceCommas($("#PDTaxAmount").val())) || 0) + (parseFloat(cmn_ReplaceCommas($("#PDPurchasePrice").val())) || 0);
        $("#PDTotalCost").val(total.toFixed(2));
        $("#PDTotalCost").attr("style", "border-color: #ced4da;");
        $("#SpanPDTotalCostErrorMsg").css("display", "none");
        var TotalCostAmt = cmn_ReplaceCommas($('#PDTotalCost').val());
        if (AvoidDot(TotalCostAmt) == false) {
            TotalCostAmt = "";
            $('#PDTotalCost').val("");
        }
        else {
            $('#PDTotalCost').val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(TotalCostAmt), ValDigit)));
        }

        //OtherCharges
        var OtherCharges = cmn_ReplaceCommas($('#PDOtherCharges').val());
        if (AvoidDot(OtherCharges) == false) {
            //OtherCharges = "";
            $('#PDOtherCharges').val(OtherCharges);
        }
        else {
            $('#PDOtherCharges').val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(OtherCharges), ValDigit)));
        }
    }
}
function OnChangePDTotalCost(ACID) {
    if ($('#PDTotalCost').val() != '') {
        $("#PDTotalCost").attr("style", "border-color: #ced4da;");
        $("#SpanPDTotalCostErrorMsg").css("display", "none");
    }
}
function PDOnClickResetButton() {
    var rowCount = $('#tblAssetProcurementDetail >tbody >tr').length;

    if (rowCount > 0) {
        $("#tblAssetProcurementDetail > tbody > tr").remove();
    }
    else {
        //$("#CCAmount").val(null);
        //$("#spn_CCAmount").css("display", "none");
        //$("#CCAmount").css("border-color", "#ced4da");
    }
    PDClearControl();
    ProcVal();
}
function ProcVal() {
    var FTotalAmount = parseFloat(0);
    var Length = $("#tblAssetProcurementDetail > tbody > tr").length;
    if (Length > 0) {
        $("#tblAssetProcurementDetail > tbody > tr").each(function () {
            var currentrow = $(this);
            var ddl_Type_Id = currentrow.find("#TotalCost").text()
            //var TotalAmount = parseFloat(Cmn_CalculateAmtCC(ddl_Type_Id)).toFixed(cmn_ValDecDigit);
            var TotalAmount = parseFloat(cmn_ReplaceCommas(ddl_Type_Id)).toFixed(cmn_ValDecDigit);
            FTotalAmount = parseFloat(FTotalAmount) + parseFloat(TotalAmount);
        })
        $("#ProcuredValue").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(FTotalAmount), ValDigit)));
    }
    else {
        $("#ProcuredValue").val(0);
    }
    OnChangeProcuredValue(1);
}

//    function PD_onclickSaveandExitBtn() { 
//    debugger;
//    var Flag = $("#txtdisable").val();
//    var ErrorFlag = "Y";
//    var Tbllen = $("#tblAssetProcurementDetail > tbody > tr").length;
//    //var hdnTbllen = $("#tblpdhdn > tbody >tr").length;
//    //if (Flag == "Y") {
//    //    if (Tbllen != hdnTbllen) {
//    //        return false;
//    //    }
//    //}
//    //$("#tblpdhdn > tbody >tr").remove();
//    var Length = $("#tblAssetProcurementDetail > tbody > tr").length;
//    var FTotalAmount = parseFloat(0);
//    if (Length == 0) {
//        $("#ProcuredValue").val(0);
//        OnChangeProcuredValue(1);
//        $("#PDSaveAndExitBtn").attr("data-dismiss", "modal");
//    }
//    else {
//        if (Length > 0) {
//            $("#tblAssetProcurementDetail > tbody > tr").each(function () {
//                var currentrow = $(this);
//                var ddl_Type_Id = currentrow.find("#TotalCost").text()
//                //var TotalAmount = parseFloat(Cmn_CalculateAmtCC(ddl_Type_Id)).toFixed(cmn_ValDecDigit);
//                var TotalAmount = parseFloat(cmn_ReplaceCommas(ddl_Type_Id)).toFixed(cmn_ValDecDigit);
//                FTotalAmount = parseFloat(FTotalAmount) + parseFloat(TotalAmount);
//                $("#ProcuredValue").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(FTotalAmount), ValDigit)));
//                OnChangeProcuredValue(1);
//            })

//            if (ErrorFlag == "N") {
//                $("#ProcuredValue").val(0);
//                OnChangeProcuredValue(1);
//                return false;
//            }
//            if (ErrorFlag == "Y") {

//                //$("#tblAssetProcurementDetail > tbody > tr").each(function () {
//                //    var currentrow = $(this);
//                //    var hdnSrno = currentrow.find("#Srno").text();
//                //    var BillNumber = currentrow.find("#BillNumber").text();
//                //    var BillDate = currentrow.find("#BillDate").text();
//                //    var Supplier = currentrow.find("#SupplierName").text();
//                //    var GST = currentrow.find("#GSTNumber").text();
//                //    var Currency = currentrow.find("#Currency").text();
//                //    var CurrencyId = currentrow.find("#CurrencyId").text();
//                //    var PurchasePrice = cmn_ReplaceCommas(currentrow.find("#PurchasePrice").text());
//                //    var TaxAmount = cmn_ReplaceCommas(currentrow.find("#TaxAmount").text());
//                //    var OCAmount = cmn_ReplaceCommas(currentrow.find("#OtherCharges").text());
//                //    var TotalCost = cmn_ReplaceCommas(currentrow.find("#TotalCost").text());
//                //    var CaptAmount = cmn_ReplaceCommas(currentrow.find("#CaptAmount").text());

//                //    $("#tblpdhdn > tbody").append(`<tr>
//                //        <td id="hdnSrno">${hdnSrno}</td>
//                //        <td id="hdnBillNumber">${BillNumber}</td>
//                //        <td id="hdnBillDate">${BillDate}</td>
//                //        <td id="hdnSupplierName">${Supplier}</td>
//                //        <td id="hdnGSTNumber">${GST}</td>
//                //        <td id="hdnCurrency" class="center">${Currency}</td>
//                //        <td id="hdnCurrencyId" class="center" style="display:none">${CurrencyId}</td>
//                //        <td id="hdnPurchasePrice" class="num_right">${PurchasePrice}</td>
//                //        <td id="hdnTaxAmount" class="num_right">${TaxAmount}</td>
//                //        <td id="hdnOCAmount" class="num_right">${OCAmount}</td>
//                //        <td id="hdnTotalCost" class="num_right">${TotalCost}</td>
//                //        <td id="hdnCaptAmount" class="num_right" style="display: none">${CaptAmount}</td>
//                //            </tr>`)

//                //});
//                $("#PDSaveAndExitBtn").attr("data-dismiss", "modal");
//            }
//            $("#ProcuredValue").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(FTotalAmount), ValDigit)));
//            OnChangeProcuredValue(1);
//        }
//        else {
//            $("#PDSaveAndExitBtn").attr("data-dismiss", "");
//            $("#ProcuredValue").val(0);
//            OnChangeProcuredValue(1);
//            ErrorFlag = "N";
//        }
//    }
//    if (ErrorFlag == "Y") {
//        return true;
//    }
//    else {
//        return false;
//    }
//}
/*Upload Code Starts*/
function FetchCustomerData() {
    debugger;
    var isfileexist = $('#assetregfile').val();
    if (isfileexist != null && isfileexist != "") {
        FetchAndValidateData('0');
    }
    else {
        swal("", "Please choose file", "warning");
    }
}
function FetchAndValidateData(uploadStatus) {
    $(".loader1").show();
    var formData = new FormData();
    var file = $("#assetregfile").get(0).files[0];
    formData.append("file", file);
    $('#btnassregImportData').prop('disabled', true);
    var PName = window.location.pathname.split('/')
    var addr = '/' + PName[1] + '/' + PName[2]
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ValidateExcelFile?uploadStatus=' + uploadStatus);
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#PartialImportAssetRegDetail').html(xhr.response);
            cmn_apply_datatable("#DttblBtnsassreg")
            $(".loader1").hide();
            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            if (responseMessage == "Excel file is empty. Please fill data in excel file and try again" || responseMessage == "ErrorPage") {
                $('#btnassregImportData').prop('disabled', true); // Keep the button disabled
                $('#btnassregImportData').css('background-color', '#D3D3D3')
            } else {
                $('#btnassregImportData').prop('disabled', false); // Enable the button
                $('#btnassregImportData').css('background-color', '#007bff')
            }
        }
    }
}
function OnClickErrorDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var customerName = "";
    assetname = clickedrow.find("#tdassetdesc").text();
    serial_number = clickedrow.find("#serial_no").text();
    asset_label = clickedrow.find("#asset_label").text();
    var formData = new FormData();
    var file = $("#assetregfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?AssetDes=' + encodeURIComponent(assetname) + '&AssetSerialNo=' + encodeURIComponent(serial_number) + '&Assetlabel=' + encodeURIComponent(asset_label));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#divErrorDetail').html(xhr.response);
        }
    }
}
function ImportDataFromExcel() {
    debugger;
    $(".loader1").show();
    if ($('#hdnNotOk').val() != "0") {
        $('.loader1').hide();
        swal("", 'Invalid data in excel file. Please modify the data and try again..!', "warning");
    }
    else {
        var formData = new FormData();
        var file = $("#assetregfile").get(0).files[0];
        formData.append("file", file);

        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest();
        xhr.open('POST', addr + '/ImportAssetRegistrationDetailFromExcel');
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            $(".loader1").hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                //alert(xhr.responseText);
                if (xhr.responseText.toLowerCase().includes("Success")) {
                    swal("", xhr.responseText, "success");
                    $('.loader1').hide();
                }
                else {
                    var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
                    if (responseMessage == "Data saved successfully") {
                        swal("", xhr.responseText, "success");
                        $('#btnassregImportData').prop('disabled', true);
                        $('#btnassregImportData').css('background-color', '#D3D3D3')
                        $('#DttblBtnsassreg').DataTable().destroy();
                        $('#DttblBtnsassreg tbody').empty()
                        $('#tblstatus_id').empty();
                        $('#assetregfile').val('');
                        $('#PartialImportAssRegData').modal('hide');
                        $('.loader1').hide();
                    }
                    else {
                        swal("", xhr.responseText, "warning");
                        $('.loader1').hide();
                    }
                }
            }
        }
    }
}
function Closemodal() {
    $('#DttblBtnsassreg').DataTable().destroy();
    $('#DttblBtnsassreg tbody').empty()
    $('#tblstatus_id').empty();
    $('#assetregfile').val('');
    $('#btnassregImportData').prop('disabled', true);
    $('#btnassregImportData').css('background-color', '#D3D3D3')
}
function popClose_modal() {
    debugger;
    $("#PartialCustomerAddressDetail").css("display", "none")
    $(".modal-backdrop").css("display", "none")
}
function popClose_Brmodal() {
    debugger;
    $("#BranchMapping").css("display", "none")
    $(".modal-backdrop").css("display", "none")
}
function SearchByUploadStatus() {
    var uploadStatus = $('#item_Status').val();
    if ($('#DttblBtnsassreg tbody tr').length > 0) {
        FetchAndValidateData(uploadStatus);
    }
}

function ProcurementDetailList(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var AssetDescriptionHis = currentrow.find("#tdassetdesc").text();
    var AssetLabelHis = currentrow.find("#asset_label").text();
    var SerialNumberHis = currentrow.find("#serial_no").text();

    var DisableFlag = "Y";
    var NewArr = new Array();
    const ValDecDigit = $("#ValDigit").text();

    $("#tblpdhdn > tbody > tr").each(function () {
        var currentrow = $(this);
        var List = {};
        List.hdnSrno = currentrow.find("#Srno").text();
        List.AssetDesc = currentrow.find("#hdnAssetDesc").text();
        List.AssetLabel = currentrow.find("#hdnAssetLabel").text();
        List.SerialNumber = currentrow.find("#hdnSerialNumber").text();
        List.BillNumber = currentrow.find("#hdnBillNumber").text();
        List.BillDate = currentrow.find("#hdnBillDate").text();
        List.BillDt = currentrow.find("#hdnBillDt").text();
        List.Supplier = currentrow.find("#hdnSupplierName").text();
        List.GST = currentrow.find("#hdnGSTNumber").text();
        List.Currency = currentrow.find("#hdnCurrency").text();
        List.CurrencyId = currentrow.find("#hdnCurrencyId").text();
        List.PurchasePrice = parseFloat(cmn_ReplaceCommas(currentrow.find("#hdnPurchasePrice").text())).toFixed(ValDecDigit);
        List.TaxAmount = parseFloat(cmn_ReplaceCommas(currentrow.find("#hdnTaxAmount").text())).toFixed(ValDecDigit);
        List.OtherCharges = parseFloat(cmn_ReplaceCommas(currentrow.find("#hdnOCAmount").text())).toFixed(ValDecDigit);
        List.TotalCost = parseFloat(cmn_ReplaceCommas(currentrow.find("#hdnTotalCost").text())).toFixed(ValDecDigit);
        List.CapitalizedValue = parseFloat(cmn_ReplaceCommas(currentrow.find("#hdnCaptAmount").text())).toFixed(ValDecDigit);
        //if (List.AssetDesc == AssetDescriptionHis && List.SerialNumber == SerialNumberHis && List.AssetLabel == AssetLabelHis) {
        if (List.AssetDesc == AssetDescriptionHis && List.SerialNumber == SerialNumberHis) {
            NewArr.push(List);
        }
    });
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/AssetRegistration/GetAssetProcurementDetailsList",
        data: {
            AssetDescriptionTD: AssetDescriptionHis, DisableFlag: DisableFlag, PD_rowdata: JSON.stringify(NewArr),
        },
        success: function (data) {
            $("#ProcrumentDetailPartial").html(data);
            $('#AssetDescriptionPD').val(AssetDescriptionHis);
            $('#AssetLabelPD').val(AssetLabelHis);
            $('#SerialNumberPD').val(SerialNumberHis);
        }
    })
}
function PDClosemodal(btn) {
    debugger;
     if ($.fn.DataTable.isDataTable('#tblAssetProcurementDetail')) {
        $('#tblAssetProcurementDetail').DataTable().clear().destroy();
    }
    //$('#tblstatus_id').empty();
    //$('#ChooseSerialNofile').val('');
    //$('#btnImportData').prop('disabled', true);
    //$('#btnImportData').css('background-color', '#D3D3D3')
   // $("#ClosemodalPartialImportData").attr("data-dismiss", "modal");

    $(btn).closest('.modal').modal('hide');
}
/*Upload Code End*/