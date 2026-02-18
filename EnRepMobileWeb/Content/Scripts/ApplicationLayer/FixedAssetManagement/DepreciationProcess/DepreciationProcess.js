var ValDecDigit = $("#ExpImpValDigit").text();
$(document).ready(function () {
    $("#ddlAssetGroup").select2();
    $("#ddl_financial_year").prop("disabled", true);
    $("#ddl_period").prop("disabled", true);
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#Doc_no").val() == "" || $("#Doc_no").val() == null) {
        $("#Doc_date").val(CurrentDate);
    }
    $("#Tbl_list_FA_DP #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        var WF_status = $("#WF_status").val();
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var DocNo = clickedrow.children("#DPDocNo").text();
        var DocDt = clickedrow.children("#SSIDt").text();
        if (DocNo != "" && DocNo != null) {
            window.location.href = "/ApplicationLayer/DepreciationProcess/AddDepreciationProcessDetail?DocNo=" + DocNo + "&DocDt=" + DocDt + "&ListFilterData=" + ListFilterData;
        }
    });
    $("#Tbl_list_FA_DP #datatable-buttons tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        debugger;
        var SSINo = clickedrow.children("#DPDocNo").text();
        var SSIDt = clickedrow.children("#SSIDt").text();
        // var Doc_Status = clickedrow.children("#Doc_Status").text();
        var Doc_Status = clickedrow.children("#FADP_Status").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(SSINo);
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");

        GetWorkFlowDetails(SSINo, SSIDt, Doc_id, Doc_Status);
    });
    var InvoiceNo = $("#Doc_no").val();
    $("#hdDoc_No").val(InvoiceNo);
    CancelledRemarks("#Cancelled", "Disabled");
});
function FilterDPList() {
    debugger;
    try {
        var Group = $("#ddlAssetGroup").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DepreciationProcess/DPListSearch",
            data: {
                Group: Group, Status: Status
            },
            success: function (data) {
                debugger;
                $('#Tbl_list_FA_DP').html(data);
                $('#ListFilterData').val(Group + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    }
    catch (err) {
        debugger;
        console.log("DP Error : " + err.message);
    }
}
function OnChangeddlAssetGroup(assetgrp) {
    var assetgrp = assetgrp.value;
    if (assetgrp == "" || assetgrp == null || assetgrp == undefined) {
        assetgrp = assetgrp;
    }
    $("#Depreciationfreq").val("");
    $("#hdn_dep_frequencyId").val("");
    if (assetgrp == "") {
        $("#ddlAssetGroup").val("");
        $('#SpanddlAssetGroupErrorMsg').text($("#valueReq").text());
        $("#SpanddlAssetGroupErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlAssetGroup-container']").css("border-color", "red");
        $("#Hdn_AssetsGroupId").val('');
    }
    else {
        $("#Hdn_AssetsGroupId").val($('#ddlAssetGroup').val().trim());
        $("#vm_ddlAssetGroup").css("display", "none");
        $("[aria-labelledby='select2-ddlAssetGroup-container']").css("border-color", "#ced4da");

        var ddl_f_frequency = $("#ddl_financial_year").val();
        if (ddl_f_frequency = "0") {
            $("#vm_ddl_financial_year").css("display", "none");
            $("#ddl_financial_year").css("border-color", "#ced4da");
        }
        var ddl_period = $("#ddl_period").val();
        $("#txtFromDate").val('');
        $("#txtToDate").val('');
        if (ddl_period == "0") {
            $("#vm_ddl_period").css("display", "none");
            $("#ddl_period").css("border-color", "#ced4da");
        }

        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/DepreciationProcess/GetAssetCategoryDetails",
                data: { AssetGroupId: assetgrp },
                success: function (data) {
                    if (typeof data === 'string') {
                        try {
                            var arr = [];
                            debugger;
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                $("#Depreciationfreq").val(arr.Table[0].dep_Frequency);
                                $("#Hdn_AssetsGroupName").val(arr.Table[0].CategoryName);
                                $("#hdn_dep_frequencyId").val(arr.Table[0].dep_freq);
                                $("#curr_id").val(arr.Table[0].curr_id);
                                $("#conv_rate").val(arr.Table[0].curr_rate);
                                $("#asset_coa").val(arr.Table[0].asset_coa);
                                $("#AssetAccount").val(arr.Table[0].AssetAccount);
                                $("#dep_coa").val(arr.Table[0].dep_coa);
                                $("#DepreciationAccount").val(arr.Table[0].DepreciationAccount);
                                $("#ddl_financial_year").prop("disabled", true);
                                $("#ddl_period").prop("disabled", true);
                                //ddl_financial_year_onchange();
                                if (arr.Table1.length > 0) 
                                $("#ddl_financial_year").val('0');
                                $("#ddl_period").val('0');
                                $("#txtFromDate").val(arr.Table1[0].from_date);
                                $("#txtToDate").val(arr.Table1[0].to_date);
                                if (arr.Table1[0].f_fy_Id != null && arr.Table1[0].f_fy_Id != 0) {
                                    $('#ddl_financial_year').empty().append('<option value=' + arr.Table1[0].f_fy_Id + ' selected="selected">' + arr.Table1[0].f_fy + '</option>');
                                    $("#ddl_financial_year").attr('disabled', true);
                                    var ddl_financial_year = $("#ddl_financial_year").val();
                                    $("#hdn_ddl_financial_year").val(ddl_financial_year);
                                } else {
                                    $('#ddl_financial_year').empty().append('<option value="0" selected="selected">---Select---</option>');
                                    $("#ddl_financial_year").attr('disabled', true);
                                }
                                if (arr.Table1[0].PeriodId != null && arr.Table1[0].PeriodId != 0) {
                                    $('#ddl_period').empty().append('<option value=' + arr.Table1[0].PeriodId + ' selected="selected">' + arr.Table1[0].Period + '</option>');
                                    $("#ddl_period").attr('disabled', true);
                                    var ddl_period = $("#ddl_period").val();
                                    $("#hdn_ddl_period").val(ddl_period);
                                } else {
                                    $('#ddl_period').empty().append('<option value="0" selected="selected">---Select---</option>');
                                    $("#ddl_period").attr('disabled', true);
                                }
                                $("#Doc_AddIcon").show();
                                }
                            
                            else {
                                $("#Depreciationfreq").val("");
                                $("#hdn_dep_frequencyId").val("");
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
function ddl_financial_year_onchange(e) {
    var ddl_f_frequency = $("#ddl_financial_year").val();
    if (ddl_f_frequency != "0") {
        $("#vm_ddl_financial_year").css("display", "none");
        $("#ddl_financial_year").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_financial_year').text($("#valueReq").text());
        $("#vm_ddl_financial_year").css("display", "block");
        $("#vm_ddl_financial_year").css("border-color", "red");
    }

    var f_frequency = $("#hdn_dep_frequencyId").val();
    var ddl_financial_year = $("#ddl_financial_year").val();
    var ddl_period = $("#ddl_period").val();
    var ddl_AssetGroupId = $("#ddlAssetGroup").val();
    $("#hdn_ddl_financial_year").val(ddl_financial_year);
    var financial_year = $("#hdn_ddl_financial_year").val();
    $("#txtFromDate").val('');
    $("#txtToDate").val('');

    if (ddl_period == "0") {
        $("#vm_ddl_period").css("display", "none");
        $("#ddl_period").css("border-color", "#ced4da");
    }
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/DepreciationProcess/BindPeriod",
            data: {
                f_frequency: f_frequency,
                financial_year: financial_year,
                AssetGroupId: ddl_AssetGroupId,
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    $('#ddl_period').empty();
                    arr = JSON.parse(data);
                    if (arr.Table1 && arr.Table1.length > 0) {
                        $('#ddl_period').append(`<option value=0>---Select---</option>`);
                        for (var i = 0; i < arr.Table1.length; i++) {
                            if (arr.Table1[i].id != "null" && arr.Table1[i].id != null) {
                                $('#ddl_period').append(`<option value="${arr.Table1[i].id}">${arr.Table1[i].name}</option>`);
                            }
                        }
                    }
                    else {
                        $('#ddl_period').append(`<option value=0>---Select---</option>`);
                    }
                }
            },
        });
};
function ddl_period_onchange(e) {
    var ddl_f_frequency = $("#ddl_period").val();
    if (ddl_f_frequency != "0") {
        $("#vm_ddl_period").css("display", "none");
        $("#ddl_period").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_period').text($("#valueReq").text());
        $("#vm_ddl_period").css("display", "block");
        $("#vm_ddl_period").css("border-color", "red");
    }

    var f_frequency = $("#hdn_dep_frequencyId").val();

    var ddl_financial_year = $("#ddl_financial_year").val();
    $("#hdn_ddl_financial_year").val(ddl_financial_year);
    var financial_year = $("#hdn_ddl_financial_year").val();

    var ddl_period = $("#ddl_period").val();
    if (ddl_period != "0") {
        $("#ddl_ItemName").attr('disabled', false);
    }
    else {
        $("#ddl_ItemName").attr('disabled', true);
    }
    $("#hdn_ddl_period").val(ddl_period);
    var period = $("#hdn_ddl_period").val();
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/DepreciationProcess/BindDateRange",
            data: {
                f_frequency: f_frequency,
                financial_year: financial_year,
                period: period,
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    $('#txtFromDate').val('');
                    $('#txtToDate').val('');
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#txtFromDate').val(arr.Table[i].StartDT);
                            $('#txtToDate').val(arr.Table[i].EndDT);
                        }
                    }
                }
                else {
                    $('#txtFromDate').val('');
                    $('#txtToDate').val('');
                }
            },
        });
};
function OnClickAddButton() {
    try {
        debugger;
        var ddl_Asset_Group = $("#ddlAssetGroup").val();
        if (ddl_Asset_Group != "0") {
            $("#vm_ddlAssetGroup").css("display", "none");
            $("#ddlAssetGroup").css("border-color", "#ced4da");
            $('[aria-labelledby="select2-ddlAssetGroup-container"]').css("border-color", "#ced4da");
        }
        else {
            $('#vm_ddlAssetGroup').text($("#valueReq").text());
            $("#vm_ddlAssetGroup").css("display", "block");
            $('[aria-labelledby="select2-ddlAssetGroup-container"]').css("border-color", "red");
            return;
        }
        var ddl_f_frequency = $("#ddl_financial_year").val();
        if (ddl_f_frequency != "0") {
            $("#vm_ddl_financial_year").css("display", "none");
            $("#ddl_financial_year").css("border-color", "#ced4da");
        }
        else {
            $('#vm_ddl_financial_year').text($("#valueReq").text());
            $("#vm_ddl_financial_year").css("display", "block");
            $("#ddl_financial_year").css("border-color", "red");
            return;
        }
        var ddl_period = $("#ddl_period").val();
        if (ddl_period != "0") {
            $("#vm_ddl_period").css("display", "none");
            $("#ddl_period").css("border-color", "#ced4da");
        }
        else {
            $('#vm_ddl_period').text($("#valueReq").text());
            $("#vm_ddl_period").css("display", "block");
            $("#ddl_period").css("border-color", "red");
            return;
        }
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DepreciationProcess/GetAssetRegGroupDetail",
            data: { AssetGroup: $("#Hdn_AssetsGroupId").val(), fin_yr: $("#hdn_ddl_financial_year").val(), Period: $("#hdn_ddl_period").val() },
            success: function (html) {
                $("#Tbl_list_ItemDetails").html(html);
                $("#Doc_AddIcon").hide();
                $("#ddlAssetGroup").attr("disabled", true);
                $("#ddl_financial_year").attr("disabled", true);
                $("#ddl_period").attr("disabled", true);
                GetAllGLID();
            },
        });
    } catch (err) {
        console.log("DepreciationProcessError : " + err.message);
    }
}
function SaveBtnClick() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition By NItesh 08-01-2024 for Disable Save Button**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    //var length = $("#FADPAssetDetailsTbl >tbody >tr>td").length;
    //if (length == 1) {
    //    if (HeaderValidations() == false) {
    //        return false;
    //    }
    //}
    if (HeaderValidations() == false) {
        return false;
    }
    var ItemValidation = Check_ItemValidations();
    if (ItemValidation == false) {
        swal("", $("#noitemselectedmsg").text(), "warning");
        return false;
    }
    var FinalHeaderDetail = [];
    FinalHeaderDetail = InsertDPHeaderDetails();
    var HeaderDt = JSON.stringify(FinalHeaderDetail);
    $('#hdHeaderDetailList').val(HeaderDt);

    var FinalItemDetail = [];
    FinalItemDetail = InsertDPItemDetails();
    var ItemDt = JSON.stringify(FinalItemDetail);

    var FinalVoucherDetail = [];
    FinalVoucherDetail = InsertDPVoucherDetails();
    var VoucherDt = JSON.stringify(FinalVoucherDetail);

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);

    try {
        if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramtInBase1", "cramtInBase1", "hfPIStatus") == false) {
            return false;
        }
    }
    catch (ex) {
        console.log(ex);
        return false;
    }

    $('#hdItemDetailList').val(ItemDt);
    $('#hdVouGlDetailList').val(VoucherDt);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
}
function InsertDPHeaderDetails() {
    debugger;
    var HeaderDetailList = new Array();
    var HeaderList = {};
    HeaderList.DocNo = $("#Doc_no").val();
    HeaderList.DocDate = $("#Doc_date").val();
    HeaderList.asset_grp_id = $("#ddlAssetGroup").val();
    HeaderList.f_fy = $("#hdn_ddl_financial_year").val();
    HeaderList.f_period = $("#hdn_ddl_period").val();
    HeaderList.from_date = $("#txtFromDate").val();
    HeaderList.to_date = $("#txtToDate").val();
    HeaderList.dp_status = "D";
    HeaderList.create_id = "";
    HeaderList.app_id = "";
    HeaderList.mod_id = "";
    HeaderList.mac_id = "";
    HeaderDetailList.push(HeaderList);
    return HeaderDetailList;
};
function InsertDPItemDetails() {
    debugger;
    var ItemDetailList = new Array();
    $("#FADPAssetDetailsTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SrNo").val();
        var ItemList = {};
        ItemList.ass_reg_id = currentRow.find("#Hdn_ass_reg_id").val();
        ItemList.AssetId = currentRow.find("#Hdn_ass_id").val();
        ItemList.SerialNumber = currentRow.find("#SerialNumber").text();
        ItemList.AssetLabel = currentRow.find("#AssetLabel").text();
        ItemList.CurrentValue = cmn_ReplaceCommas(currentRow.find("#CurrentValue").text());
        ItemList.DepreciationMethod = currentRow.find("#Hdn_dep_method").val();
        ItemList.DepreciationPercentage = currentRow.find("#DepreciationPercentage").text();
        ItemList.DepreciationValue = cmn_ReplaceCommas(currentRow.find("#DepreciationValue").text());
        ItemList.AdditionalDepreciationPercentage = currentRow.find("#AdditionalDepreciationPercentage").text();
        ItemList.AdditionalDepreciationValue = cmn_ReplaceCommas(currentRow.find("#AdditionalDepreciationValue").text());
        ItemList.TotalDepreciationValue = cmn_ReplaceCommas(currentRow.find("#Hdn_TotalDepreciationValue").val());
        ItemList.RevisedAssetValue = cmn_ReplaceCommas(currentRow.find("#RevisedAssetValue").text());
        ItemDetailList.push(ItemList);
        debugger;
    });
    return ItemDetailList;
};
function InsertDPVoucherDetails1() {
    debugger;
    var SI_VouList = [];
    var CustVal = 0;
    var Compid = $("#CompID").text();
    var _SIType = "E";

    var TransType = "Sal";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var acc_id = "";
            var acc_name = "";
            var dr_amt = "";
            var cr_amt = "";
            var Gltype = "";
            acc_id = currentRow.find("#hfAccID").val();
            acc_name = currentRow.find("#CInv_txthfAccID").text().trim();
            dr_amt = currentRow.find("#dramt").text();
            cr_amt = currentRow.find("#cramt").text();
            Gltype = currentRow.find("#type").val();
            SI_VouList.push({ comp_id: Compid, id: acc_id, acc_name: acc_name, type: "I", doctype: _SIType, Value: CustVal, DrAmt: dr_amt, CrAmt: cr_amt, TransType: TransType, Gltype: Gltype });

        });
    }
    return SI_VouList;
};
function InsertDPVoucherDetails() {
    debugger;
    var PI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "D";
    var TransType = "Pur";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            let VouSrNo = currentRow.find("#td_vou_sr_no").text();
            let GlSrNo = currentRow.find("#td_GlSrNo").text();
            var acc_id = currentRow.find("#hfAccID").val();
            var acc_name = currentRow.find("#txthfAccID").val();
            var dr_amt = currentRow.find("#dramt").text();
            var cr_amt = currentRow.find("#cramt").text();
            var dr_amt_bs = currentRow.find("#dramtInBase").text();
            var cr_amt_bs = currentRow.find("#cramtInBase").text();
            var Gltype = currentRow.find("#type").val();
            var curr_id = currentRow.find("#gl_curr_id").val();
            var conv_rate = currentRow.find("#gl_conv_rate").val();
            var vou_type = currentRow.find("#glVouType").val();
            var bill_no = currentRow.find("#gl_bill_no").val();
            var bill_date = currentRow.find("#gl_bill_date").val();
            var gl_narr = currentRow.find("#gl_narr").text();
            PI_VouList.push({
                comp_id: Compid, VouSrNo: VouSrNo, GlSrNo: GlSrNo, id: IsNull(acc_id, '0'), acc_name: acc_name
                , type: "I", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
                , DrAmt: dr_amt, CrAmt: cr_amt, DrAmtInBase: dr_amt_bs, CrAmtInBase: cr_amt_bs
                , Gltype: IsNull(Gltype, '0'), TransType: TransType, curr_id: curr_id, conv_rate: conv_rate
                , vou_type: vou_type, bill_no: bill_no, bill_date: bill_date, gl_narr: gl_narr
            });
        });
    }
    return PI_VouList;
};
//-------------------------Vallidation----------------------------------------//
function HeaderValidations() {
    debugger;
    var ErrorFlag = "N";
    var ddl_Asset_Group = $("#ddlAssetGroup").val();
    if (ddl_Asset_Group != "0") {
        $("#vm_ddlAssetGroup").css("display", "none");
        $("#ddlAssetGroup").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-ddlAssetGroup-container"]').css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddlAssetGroup').text($("#valueReq").text());
        $("#vm_ddlAssetGroup").css("display", "block");
        $('[aria-labelledby="select2-ddlAssetGroup-container"]').css("border-color", "red");
        ErrorFlag = "Y";
    }
    var ddl_f_frequency = $("#ddl_financial_year").val();
    if (ddl_f_frequency != "0") {
        $("#vm_ddl_financial_year").css("display", "none");
        $("#ddl_financial_year").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_financial_year').text($("#valueReq").text());
        $("#vm_ddl_financial_year").css("display", "block");
        $("#ddl_financial_year").css("border-color", "red");
        ErrorFlag = "Y";
    }
    var ddl_period = $("#ddl_period").val();
    if (ddl_period != "0") {
        $("#vm_ddl_period").css("display", "none");
        $("#ddl_period").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_period').text($("#valueReq").text());
        $("#vm_ddl_period").css("display", "block");
        $("#ddl_period").css("border-color", "red");
        ErrorFlag = "Y";
    }

    if ($("#Cancelled").is(":checked")) {
        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }
    }
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
function Check_ItemValidations() {
    debugger;
    var ErrorFlag = "N";
    //if ($("#FADPAssetDetailsTbl >tbody >tr").length > 0) { }
    //else {
    //    swal("", $("#noitemselectedmsg").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //}
    if ($("#FADPAssetDetailsTbl tbody tr").length > 0) {
        $("#FADPAssetDetailsTbl tbody tr").each(function () {
            debugger
            var currentRow = $(this);
            var number = currentRow.find("#SrNo").text();
            if (number == "" || number == null || number == "0") {
                swal("", $("#noitemselectedmsg").text(), "warning");
                ErrorFlag = "Y";
            }
        });
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
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
function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#ForceClosed").attr("disabled", true);
        $("#ForceClosed").prop("checked", false);
        $("#btn_save").attr('onclick', 'return SaveBtnClick()');
        $("#btn_save").attr('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr('onclick', '');
        $("#btn_save").attr('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}
function onchangeCancelledRemarks() {
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}
/***-------------------For Workflow End----------------***/
function ForwardBtnClick() {
    debugger;
    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var PQDate = $("#Doc_date").val();
    $.ajax({
        type: "POST",
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: PQDate
        },
        success: function (data) {
            /* if (data == "Exist") { *//*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var OrderStatus = "";
                OrderStatus = $('#hfStatus').val().trim();
                if (OrderStatus === "D" || OrderStatus === "F") {

                    if ($("#hd_nextlevel").val() === "0") {
                        $("#Btn_Forward").attr("data-target", "");
                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                        $("#Btn_Approve").attr("data-target", "");
                    }
                    var Doc_ID = $("#DocumentMenuId").val();
                    $("#OKBtn_FW").attr("data-dismiss", "modal");

                    Cmn_GetForwarderList(Doc_ID);

                }
                else {
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Btn_Forward").attr('onclick', '');
                    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                }
            }
            else {/* to chk Financial year exist or not*/
                swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                $("#Btn_Forward").attr("data-target", "");
                $("#Btn_Approve").attr("data-target", "");
                $("#Forward_Pop").attr("data-target", "");
            }
        }
    });
    /*End to chk Financial year exist or not*/
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var PQNo = "";
    var PQDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var mailerror = "";

    Remarks = $("#fw_remarks").val();
    PQNo = $("#Doc_no").val();
    PQDate = $("#Doc_date").val();
    docid = $("#DocumentMenuId").val();
    var WF_status1 = $("#WF_status1").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var TrancType = (PQNo + ',' + PQDate + ',' + "Update" + ',' + WF_status1)
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    //Added by Nidhi on 08-07-2025
    var pdfAlertEmailFilePath = 'DP_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    //GetPdfFilePathToSendonEmailAlert(PQNo, PQDate, pdfAlertEmailFilePath);
    if (fwchkval != "Approve") {
        //var pdfAlertEmailFilePath = "DepreciationProcess_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        //var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(PQNo, PQDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/DepreciationProcess/SavePdfDocToSendOnEmailAlert");
        //if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
        //    pdfAlertEmailFilePath = "";
        //}
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && PQNo != "" && PQDate != "" && level != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            //mailerror = $("#MailError").val();
            //showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/DepreciationProcess/ApproveDPDetails?Inv_No=" + PQNo + "&Inv_Date=" + PQDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&FilterData=" + ListFilterData1 + "&WF_Status1=" + WF_status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && PQNo != "" && PQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && PQNo != "" && PQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            showLoader();
            window.location.reload();
        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#Doc_no").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function OtherFunctions(StatusC, StatusName) {
    //window.location.reload();
}
//----------------------------WorkFlow JS End-------------------------------------//
function FilterItemDetail(e) {//added by Suraj Maurya on 13-02-2025 to filter item details
    Cmn_FilterTableData(e, "FADPAssetDetailsTbl", [{ "FieldId": "AssetDescription", "FieldType": "input", "SrNo": "" }]);
}

/***---------------------------GL Voucher Entry Start-----------------------------***/
function GetAllGLID() {
    GetAllGL_WithMultiSupplier();
}
function CheckDP_AssetValidations(VoucherValidate) {
    var ErrorFlag = "N";
    var count = 0;
    if ($("#FADPAssetDetailsTbl >tbody >tr").length > 0) {
        let isFocused = false;
        var len = $("#FADPAssetDetailsTbl > tbody > tr").length;
        $("#FADPAssetDetailsTbl >tbody >tr").each(function () {

            if (VoucherValidate == "Y") {
                if (parseFloat(len) == parseFloat(count)) {
                    var currentRow = $(this);
                    var Sno = currentRow.find("#SNohiddenfiled").val();
                    var POItemListName = currentRow.find("#POItemListName" + Sno).val();
                    if (POItemListName == "" || POItemListName == null || POItemListName == "0") {
                        currentRow.find("#SpanPOItemListName_Error" + Sno).text($("#valueReq").text());
                        currentRow.find("#SpanPOItemListName_Error" + Sno).css("display", "block");
                        currentRow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid red");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#SpanPOItemListName_Error" + Sno).css("display", "none");
                        currentRow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
                    }
                }
                count = count + 1;
            }

        })
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
async function GetAllGL_WithMultiSupplier() {
    debugger;
    if ($("#FADPAssetDetailsTbl > tbody > tr").length == 0) {
        $("#VoucherDetail tbody tr").remove();
        $("#tbladdhdn tbody tr").remove();
        $("#DrTotalInBase").text("");
        $("#CrTotalInBase").text("");
        return false;
    }
    if (CheckDP_AssetValidations("Y") == false) {
        return false;
    }

    var NetInvValue = 0;
    $("#FADPAssetDetailsTbl >tfoot >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        NetInvValue = cmn_ReplaceCommas(currentRow.find("#Totdep_val").text());
    });
    var conv_rate = $("#conv_rate").val();
    var supp_id = $("#Hdn_AssetsGroupId").val();
    var supp_acc_id = $("#supp_acc_id").val();
    var depacc_name = $("#DepreciationAccount").val();
    var depacc_id = $("#dep_coa").val();
    var ast_name = $("#AssetAccount").val();
    var ast_id = $("#asset_coa").val();
    var SuppVal = 0;
    var SuppValInBase = 0;
    SuppValInBase = (parseFloat(NetInvValue)).toFixed(ValDecDigit);
    SuppVal = (parseFloat(SuppValInBase) / parseFloat(conv_rate)).toFixed(ValDecDigit)

    var Compid = $("#CompID").text();
    var InvType = "D";
    var curr_id = $("#curr_id").val();
    var bill_no = $('#ddlAssetGroup option:selected').text().trim();//$("#Hdn_AssetsGroupName").val();// $("#Bill_No").val();
    var bill_dt = "";// $("#Bill_Date").val();
    var TransType = 'Pur';
    var vouType = "JV";
    var GLDetail = [];
    var TxaExantedItemList = [];
    GLDetail.push({
        comp_id: Compid, id: supp_id, type: "FA", doctype: InvType, Value: SuppValInBase, ValueInBase: SuppValInBase
        , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp", parent: 0, Entity_id: supp_acc_id
        , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
    });
    let rowSpan = 1;//arrDetailDr.length + 1;
    let GlRowNo = 1;
    Cmn_GlTableRenderHtml(1, GlRowNo, rowSpan, "Dep", depacc_name, depacc_id, NetInvValue, NetInvValue, 0, 0, vouType, curr_id, conv_rate, bill_no, "0", "0");
    GlRowNo = GlRowNo + 1;
    //rowSpan = rowSpan + 1;
    Cmn_GlTableRenderHtml(rowSpan, GlRowNo, rowSpan, "AST", ast_name, ast_id, 0, 0, NetInvValue, NetInvValue, vouType, curr_id, conv_rate, bill_no, "0", "0");
    CalculateVoucherTotalAmount();
}

async function CalculateVoucherTotalAmount() {
    debugger;
    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    $("#VoucherDetail >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
        }
    });
    var NetInvValue = 0;
    $("#FADPAssetDetailsTbl >tfoot >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        NetInvValue = currentRow.find("#Totdep_val").text();
    });
    $("#DrTotal").text(NetInvValue);
    $("#DrTotalInBase").text(NetInvValue);
    $("#CrTotal").text(NetInvValue);
    $("#CrTotalInBase").text(NetInvValue);
    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
        debugger;
        if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
            await AddRoundOffGL();
        }
    }
}
function Get_Gl_Narration(VouType, bill_no, bill_date) {
    return ($("#hdn_Nurration").val()).replace("_Assets_Group_Name", bill_no);
}
//-------------Cost Center Section----------//
function Onclick_CCbtn(flag, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = cmn_ReplaceCommas(clickedrow.find("#dramtInBase").text());
    var CstCrtAmt = cmn_ReplaceCommas(clickedrow.find("#cramtInBase").text());
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    var gl_sr_no = clickedrow.find("#td_GlSrNo").text();
    var RowNo = clickedrow.find("#SNohf").val();

    //var GLAcc_Name = clickedrow.find("#Acc_name_" + RowNo + " option:selected").text();
    var GLAcc_Name = clickedrow.find("td_GlAccName").text();
    //var GLAcc_id = clickedrow.find("#Acc_name_" + RowNo + " option:selected").val();
    var GLAcc_id = clickedrow.find("#tdhdn_GlAccId").val();
    if (GLAcc_Name == null || GLAcc_Name == "") {
        GLAcc_Name = clickedrow.find("#txthfAccID").val();
        GLAcc_id = clickedrow.find("#hfAccID").val();
    }
    var disableflag = ($("#txtdisable").val());
    var NewArr = new Array();
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        let cc_vou_sr_no = row.find("#hdntbl_vou_sr_no").text();
        if (cc_vou_sr_no == vou_sr_no) {
            var List = {};
            List.GlAccount = row.find("#hdntbl_GlAccountId").text();
            List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
            List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
            List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
            List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
            List.vou_sr_no = row.find('#hdntbl_vou_sr_no').text();
            List.gl_sr_no = row.find('#hdntbl_gl_sr_no').text();
            var amount = row.find("#hdntbl_CstAmt").text();
            List.CC_Amount = parseFloat(cmn_ReplaceCommas(amount)).toFixed(ValDecDigit);
            NewArr.push(List);
        }
    })
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
        Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt;
    }
    var DocMenuId = $("#DocumentMenuId").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalSaleInvoice/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
            DocumentMenuId: DocMenuId
        },
        success: function (data) {
            debugger;
            $("#CostCenterDetailPopup").html(data);
            $("#CC_vou_sr_no").val(vou_sr_no);
            $("#CC_gl_sr_no").val(gl_sr_no);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(Amt);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("VoucherDetail");
        },
    })
}
//-------------------Cost Center Section End-------------------//
/***---------------------------GL Voucher Entry End-----------------------------***/