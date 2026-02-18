$(document).ready(function () {
    $("#ddlAssetGroup").select2();
    $("#ddlAssetGroupCategory").select2();
    $("#ddlWorkingStatus").select2();
    $("#ddlAssignedRequirementArea").select2();
    GetAssignedRequirementArea();
});
function FilterARList() {
    debugger;
    try {
        var Group = $("#ddlAssetGroup").val();
        var Category = $("#ddlAssetGroupCategory").val();
        var RequirementArea = $("#ddlAssignedRequirementArea").val();
        var WorkingStatus = $("#ddlWorkingStatus").val();
        $("#Hdn_AssignedRequirementAreaId").val(RequirementArea);
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MISAssetRegister/APListSearch",
            data: {
                Group: Group, Category: Category, RequirementArea: RequirementArea, WorkingStatus: WorkingStatus//, Status: Status
            },
            success: function (data) {
                debugger;
                $('#Tbl_list_FA_MIS_RA').html(data);
                $('#ListFilterData').val(Group + ',' + Category + ',' + RequirementArea + ',' + WorkingStatus);
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
function GetAssignedRequirementArea() {
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MISAssetRegister/GetAssignedRequirementArea",
            success: function (data) {
                if (data === 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                debugger;
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
function AssetDepreciationHistory(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var regId = currentrow.find("#AssetRegId").text();
    var AssetDescriptionHis = currentrow.find("#AssetDescriptionNo").text();
    var AssetLabelHis = currentrow.find("#AssetLabel").text();
    var SerialNumberHis = currentrow.find("#SerialNo").text();
    var ProcuredValueHis = currentrow.find("#ProcuredValue").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISAssetRegister/GetAssetRegHistory",
        data: {
            RegId: regId,
        },
        success: function (data) {
            $("#DepreciationHistoryPartial").html(data);
            //replaceDatatablesIds("tbl_AsssetHistory");
            cmn_apply_datatable("#tbl_AsssetHistory");
            $('#AssetDescriptionHis').val(AssetDescriptionHis);
            $('#AssetLabelHis').val(AssetLabelHis);
            $('#SerialNumberHis').val(SerialNumberHis);
            $('#ProcuredValueHis').val(ProcuredValueHis);
        }
    })
}
function AssetProcurementDetail(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var regId = currentrow.find("#AssetRegId").text();
    var AssetDescriptionHis = currentrow.find("#AssetDescriptionNo").text();
    var AssetLabelHis = currentrow.find("#AssetLabel").text();
    var SerialNumberHis = currentrow.find("#SerialNo").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISAssetRegister/GetAssetProcDetail",
        data: {
            RegId: regId,
        },
        success: function (data) {
            $("#ProcrumentDetailPartial").html(data);
            //replaceDatatablesIds("tbl_AsssetHistory");
            //cmn_apply_datatable("#tbl_AsssetHistory");
            $('#AssetDescriptionPD').val(AssetDescriptionHis);
            $('#AssetLabelPD').val(AssetLabelHis);
            $('#SerialNumberPD').val(SerialNumberHis);
        }
    })
}