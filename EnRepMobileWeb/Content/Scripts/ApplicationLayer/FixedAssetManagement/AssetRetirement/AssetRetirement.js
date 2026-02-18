var ValDecDigit = $("#ExpImpValDigit").text();
const RateDecDigit = $("#RateDigit").text();
$(document).ready(function () {
    $("#ddlAssetGroup").select2();
    $("#ddlAssetDescription").select2();
    $("#ddlAssetSerialNo").select2();
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#Doc_no").val() == "" || $("#Doc_no").val() == null) {
        $("#Doc_date").val(CurrentDate);
    }
    $("#Tbl_list_FA_ART #datatable-buttons tbody").bind("dblclick", function (e) {
        var WF_status = $("#WF_status").val();
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var DocNo = clickedrow.children("#DPDocNo").text();
        var DocDt = clickedrow.children("#SSIDt").text();
        if (DocNo != "" && DocNo != null) {
            window.location.href = "/ApplicationLayer/AssetRetirement/AddAssetRetirementDetail?DocNo=" + DocNo + "&DocDt=" + DocDt + "&ListFilterData=" + ListFilterData;
        }
    });
    $("#Tbl_list_FA_ART #datatable-buttons tbody").bind("click", function (e) { });
    var InvoiceNo = $("#Doc_no").val();
    $("#hdDoc_No").val(InvoiceNo);
    SetRetDateRange();
    BindDDLAccountList();
});
function FilterARList() {
    try {
        var Group = $("#ddlAssetGroup").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetRetirement/AssetRetireListSearch",
            data: {
                Group: Group, Status: Status
            },
            success: function (data) {
                $('#Tbl_list_FA_ART').html(data);
                $('#ListFilterData').val(Group + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
            }
        });
    }
    catch (err) {
        debugger;
        console.log("DP Error : " + err.message);
    }
}
function SetRetDateRange() {
    var today = new Date();
    var pastDate = new Date();
    pastDate.setFullYear(today.getFullYear() - 2);
    var minDate = pastDate.toISOString().split('T')[0];
    var maxDate = today.toISOString().split('T')[0];
    $('#RetDate').attr('min', minDate);
    $('#RetDate').attr('max', maxDate);
}
function OnChangeddlAssetGroup(assetgrp) {
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
        $("#vm_ddlAssetGroup").css("display", "none");
        $("[aria-labelledby='select2-ddlAssetGroup-container']").css("border-color", "#ced4da");
        GetAssetSerialNo();
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/AssetRetirement/GetAssetDesc",
                data: { AssetGroupId: assetgrp },
                success: function (data) {
                    if (typeof data === 'string') {
                        try {
                            var arr = [];
                            debugger;
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                var tableCount = arr.Table.length;
                                $("#ddlAssetDescription").empty().append('<option value="0" selected="selected">---Select---</option>');
                                for (var i = 0; i < tableCount; i++) {
                                    var item = arr.Table[i];
                                    if (item.item_id != null && item.item_id != 0) {
                                        $('#ddlAssetDescription').append('<option value="' + item.item_id + '">' + item.item_name + '</option>');
                                    }
                                    else {
                                        $('#ddlAssetDescription').empty().append('<option value="0" selected="selected">---Select---</option>');
                                    }
                                }
                                $("#Doc_AddIcon").show();
                            }
                            else {
                                $('#ddlAssetDescription').empty().append('<option value="0" selected="selected">---Select---</option>');
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
function OnChangeddlAssetDescription(ACID) {
    if ($('#ddlAssetDescription').val() != '') {
        $('[aria-labelledby="select2-ddlAssetDescription-container"]').css("border-color", "#ced4da");
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "none");
        $("#Hdn_AssetItemsId").val($('#ddlAssetDescription').val());
    }
    GetAssetSerialNo();
}
function AmountFloatQty(el, evt) {
    let QtyDecDigit = "#QtyDigit";
    if (Cmn_FloatValueonly(el, evt, QtyDecDigit) == false) {
        return false;
    }
    return true;
}
function OnChangeScrapValue(ACID) {
    var CurrentValue = parseFloat($('#ScrapValue').val());
    if (isNaN(CurrentValue)) {
        CurrentValue = 0;
        $('#ScrapValue').val(0);
    }
    if ($('#ScrapValue').val().trim() != '') {
        $("#ScrapValue").attr("style", "border-color: #ced4da;");
        $("#SpanScrapValueErrorMsg").css("display", "none");
    }
    var DisAmt = $('#ScrapValue').val().trim();
    if (DisAmt == "" || DisAmt == null) {
        DisAmt = 0;
    }
    $('#ScrapValue').val(parseFloat(DisAmt).toFixed(RateDecDigit));
    BindDDLAccountList();
    GetAllGLID();
}
function OnChangeRetDate(ACID) {
    if ($('#RetDate').val() != '') {
        $('#RetDate').css("border-color", "#ced4da");
        $("#SpanRetDateErrorMsg").css("display", "none");
    }
}
function GetAssetSerialNo() {
    try {
        var AssetDescriptionId = $('#Hdn_AssetItemsId').val();
        var AssetGrpId = $('#Hdn_AssetsGroupId').val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetRetirement/GetSerialNoJS",
            data: {
                AssetDescriptionId: AssetDescriptionId, GrpId: AssetGrpId,
            },
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
        var ddlAssetDescription = $("#ddlAssetDescription").val();
        if (ddlAssetDescription != "0") {
            $("#SpanddlAssetDescriptionErrorMsg").css("display", "none");
            $("#ddlAssetDescription").css("border-color", "#ced4da");
        }
        else {
            $('#SpanddlAssetDescriptionErrorMsg').text($("#valueReq").text());
            $("#SpanddlAssetDescriptionErrorMsg").css("display", "block");
            $("#ddlAssetDescription").css("border-color", "red");
            return;
        }
        var ddlAssetSerialNo = $("#ddlAssetSerialNo").val();
        if (ddlAssetSerialNo != "0") {
            $("#SpanSerialNumberErrorMsg").css("display", "none");
            $("#ddlAssetSerialNo").css("border-color", "#ced4da");
        }
        else {
            $('#SpanSerialNumberErrorMsg').text($("#valueReq").text());
            $("#SpanSerialNumberErrorMsg").css("display", "block");
            $("#ddlAssetSerialNo").css("border-color", "red");
            return;
        }
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetRetirement/GetRetirmentData",
            data: { AssetDescriptionId: $("#Hdn_AssetItemsId").val(), SerialNo: $("#Hdn_SerialNumber").val() },
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
                        for (var i = 0; i < data.Table.length; i++) {
                            $('#AssetDesc').val(data.Table[i].item_name);
                            $('#Hdn_AssRegID').val(data.Table[i].ass_reg_id);
                            $('#AssetSerialNumber').val(data.Table[i].serial_no);
                            $('#AssetLabel').val(data.Table[i].asset_label);
                            $('#AssetGroup').val(data.Table[i].asset_group_name);
                            $('#CategoryDetails').val(data.Table[i].Category);
                            $('#ProcuredValue').val(data.Table[i].proc_val);
                            $('#AssetLife').val(data.Table[i].assetlife);
                            $('#AssetWorkingDate').val(data.Table[i].asset_working_dt);
                            $('#DepreciationStartDate').val(data.Table[i].DepStartDate);
                            $('#AccumulatedDepreciation').val(data.Table[i].accumulated_dep);
                            $('#CurrentValue').val(data.Table[i].curr_val);
                            $('#AsOn').val(data.Table[i].ason_dt);
                            $("#AssignedRequirementArea").val(data.Table[i].assign_req_area);
                            $('#ScrapValue').val(data.Table[i].curr_val);
                            $("#RetDate").val(moment().format('YYYY-MM-DD'));
                            $('#ddlAssetGroup').prop('disabled', true);
                            $('#ddlAssetDescription').prop('disabled', true);
                            $('#ddlAssetSerialNo').prop('disabled', true);
                            $("#Doc_AddIcon").hide();

                            $("#curr_id").val(data.Table[0].curr_id);
                            $("#conv_rate").val(data.Table[0].curr_rate);
                            $("#asset_coa").val(data.Table[0].asset_coa);
                            $("#AssetAccount").val(data.Table[0].AssetAccount);
                            $("#dep_coa").val(data.Table[0].dep_coa);
                            $("#DepreciationAccount").val(data.Table[0].DepreciationAccount);
                            //Set Range of Retirement Date
                            var depreciationStartDate = $('#DepreciationStartDate').val();
                            if (depreciationStartDate) {
                                $('#RetDate').attr('min', depreciationStartDate);
                            }
                        }
                    }
                    else {
                        $('#ddlAssetGroup').prop('disabled', false);
                        $('#ddlAssetDescription').prop('disabled', false);
                        $('#ddlAssetSerialNo').prop('disabled', false);
                    }
                }
                debugger;
                BindDDLAccountList();
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

    if (HeaderValidations() == false) {
        return false;
    }
    debugger;
    var FinalHeaderDetail = [];
    FinalHeaderDetail = InsertDPHeaderDetails();
    var HeaderDt = JSON.stringify(FinalHeaderDetail);
    $('#hdHeaderDetailList').val(HeaderDt);

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
    HeaderList.asset_description = $("#Hdn_AssetItemsId").val();
    HeaderList.serial_no = $("#Hdn_SerialNumber").val();
    HeaderList.scrapVal = cmn_ReplaceCommas($("#ScrapValue").val());
    HeaderList.ret_date = $("#RetDate").val();
    HeaderList.remarks = $("#Remarks").val();
    HeaderList.status = "D";
    HeaderList.create_id = "";
    HeaderList.app_id = "";
    HeaderList.mod_id = "";
    HeaderList.mac_id = "";
    HeaderDetailList.push(HeaderList);
    return HeaderDetailList;
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
    var ddl_f_frequency = $("#ddlAssetSerialNo").val();
    if (ddl_f_frequency != "0") {
        $("#SpanSerialNumberErrorMsg").css("display", "none");
        $('[aria-labelledby="select2-ddlAssetSerialNo-container"]').css("border-color", "#ced4da");
    }
    else {
        $('#SpanSerialNumberErrorMsg').text($("#valueReq").text());
        $("#SpanSerialNumberErrorMsg").css("display", "block");
        $('[aria-labelledby="select2-ddlAssetSerialNo-container"]').css("border-color", "red");
        ErrorFlag = "Y";
    }
    var ddl_period = $("#ddlAssetDescription").val();
    if (ddl_period != "0") {
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "none");
        $('[aria-labelledby="select2-ddlAssetDescription-container"]').css("border-color", "#ced4da");
    }
    else {
        $('#SpanddlAssetDescriptionErrorMsg').text($("#valueReq").text());
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "block");
        $('[aria-labelledby="select2-ddlAssetDescription-container"]').css("border-color", "red");
        ErrorFlag = "Y";
    }
    if ($("#ScrapValue").val() == "") {
        $('#SpanScrapValueErrorMsg').text($("#valueReq").text());
        $("#SpanScrapValueErrorMsg").css("display", "block");
        $("#ScrapValue").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanScrapValueErrorMsg").css("display", "none");
        $("#ScrapValue").css("border-color", "#ced4da");
    }
    if ($("#RetDate").val() == "") {
        $('#SpanRetDateErrorMsg').text($("#valueReq").text());
        $("#SpanRetDateErrorMsg").css("display", "block");
        $("#RetDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanRetDateErrorMsg").css("display", "none");
        $("#RetDate").css("border-color", "#ced4da");
    }

    //var ScrapValue = parseFloat(cmn_ReplaceCommas($('#ScrapValue').val()));
    //var CurrentValue = parseFloat(cmn_ReplaceCommas($('#CurrentValue').val()));
    //if (ScrapValue > CurrentValue) {
    //    $("#ScrapValue").css("border-color", "red");
    //    $("#SpanScrapValueErrorMsg").css("display", "block");
    //    //$("#SpanCurrentValueErrorMsg").text("Current Value can't be greater than Procured Value");
    //    swal("", $("#Scrapvaluecantbegreaterthanprocuredvalue").text(), "warning");
    //    ErrorFlag = "Y";
    //}
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
function FilterItemDetail(e) {//added by Suraj Maurya on 13-02-2025 to filter item details
    Cmn_FilterTableData(e, "FADPAssetDetailsTbl", [{ "FieldId": "AssetDescription", "FieldType": "input", "SrNo": "" }]);
}

/***---------------------------GL Voucher Entry Start-----------------------------***/
function GetAllGLID() {
    GetAllGL_WithMultiSupplier();
}
function BindDDLAccountList() {
    debugger;
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105106120");
}
function BindData() {
    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#VoucherDetail >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                //rowid = parseFloat(rowid) + 1;
                if (rowid > $("#VoucherDetail >tbody >tr").length) {
                    return false;
                }
                $("#Acc_name_" + rowid).empty();
                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="VouTextddl${rowid}" label='${$("#AccName").text()}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#VouTextddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#Acc_name_" + rowid).select2({
                    templateResult: function (data) {
                        var selected = $("#Acc_name_" + rowid).val();
                        if (check(data, selected, "#VoucherDetail", "#SNohf", "#Acc_name_") == true) {
                            /* var UOM = $(data.element).data('uom');*/
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                /*'<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +*/
                                /*'<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +*/
                                '</div>'
                            );
                            return $result;
                        }
                        firstEmptySelect = false;
                    }
                });
            });
        }
    }

    $("#VoucherDetail >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        if (AccID != '0' && AccID != "") {
            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
        }
    });
}
function OnChangeAccountName(RowID, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val();
    var Acc_Name = clickedrow.find("#Acc_name_" + SNo + " option:selected").text();
    var hdn_acc_id = clickedrow.find("#hfAccID").val();
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    if (hdn_acc_id != Acc_ID || hdn_acc_id == "") {
        var Len = $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
        if (Len > 0) {
            $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').each(function () {
                var row = $(this);
                var vouSrNo = row.find("#hdntbl_vou_sr_no").text();
                if (vouSrNo == vou_sr_no) {
                    row.remove();
                }
            });
        }
        //$("#POInvItmDetailsTbl >tbody >tr #hdn_item_gl_acc[value='" + hdn_acc_id + "']").closest('tr').each(function () {
        //    var row = $(this);
        //    row.find("#hdn_item_gl_acc").val(Acc_ID);
        //});
    }
    if (Acc_ID.substring(0, 1) == "3" || Acc_ID.substring(0, 1) == "4") {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", false);
    }
    else {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", true);
    }
    $("#VoucherDetail > tbody > tr #hfAccID[value=" + hdn_acc_id + "]").closest('tr').each(function () {
        let row = $(this);
        let hf_acc_id = row.find("#hfAccID").val();
        let gltype = row.find("#type").val();
        if (gltype == "VItm" && hf_acc_id == hdn_acc_id) {
            row.find("#td_GlAccName").text(Acc_Name);
            row.find("#tdhdn_GlAccId").text(Acc_ID);
            row.find("#hfAccID").val(Acc_ID);
            row.find("#txthfAccID").val(Acc_Name);
        }

    });
    clickedrow.find("#hfAccID").val(Acc_ID);
    clickedrow.find("#txthfAccID").val(Acc_Name);
    $("#hdnAccID").val(Acc_ID);
}
function CheckDP_AssetValidations(VoucherValidate) {
    var ErrorFlag = "N";
    if ($("#ScrapValue").val() == "") {
        $('#SpanScrapValueErrorMsg').text($("#valueReq").text());
        $("#SpanScrapValueErrorMsg").css("display", "block");
        $("#ScrapValue").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanScrapValueErrorMsg").css("display", "none");
        $("#ScrapValue").css("border-color", "#ced4da");
        ErrorFlag = "N";
    }
    if ($("#RetDate").val() == "") {
        $('#SpanRetDateErrorMsg').text($("#valueReq").text());
        $("#SpanRetDateErrorMsg").css("display", "block");
        $("#RetDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanRetDateErrorMsg").css("display", "none");
        $("#RetDate").css("border-color", "#ced4da");
        ErrorFlag = "N";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
async function GetAllGL_WithMultiSupplier() {

    if (CheckDP_AssetValidations("Y") == false) {
        return false;
    }
    $('#VoucherDetail tbody').empty();
    var NetInvValue = 0;
    NetInvValue = cmn_ReplaceCommas($("#ScrapValue").val());
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
    var bill_no = $('#ddlAssetGroup option:selected').text().trim();
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

    Cmn_GlTableRenderHtml(1, GlRowNo, rowSpan, "FAR", depacc_name, depacc_id, NetInvValue, NetInvValue, 0, 0, vouType, curr_id, conv_rate, bill_no, "0", "0");
    GlRowNo = GlRowNo + 1;
    //rowSpan = rowSpan + 1;
    Cmn_GlTableRenderHtml(rowSpan, GlRowNo, rowSpan, "AST", ast_name, ast_id, 0, 0, NetInvValue, NetInvValue, vouType, curr_id, conv_rate, bill_no, "0", "0");
    CalculateVoucherTotalAmount();
}


async function CalculateVoucherTotalAmount() {
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
    NetInvValue = cmn_ReplaceCommas($("#ScrapValue").val());
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

    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = cmn_ReplaceCommas(clickedrow.find("#dramtInBase").text());
    var CstCrtAmt = cmn_ReplaceCommas(clickedrow.find("#cramtInBase").text());
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    var gl_sr_no = clickedrow.find("#td_GlSrNo").text();
    var RowNo = clickedrow.find("#SNohf").val();

    var GLAcc_Name = clickedrow.find("#Acc_name_" + RowNo + " option:selected").text();
    var GLAcc_id = clickedrow.find("#Acc_name_" + RowNo + " option:selected").val();
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
function AssetProcurementDetail(e) {
    debugger;
    var regId = $("#Hdn_AssRegID").val();
    var AssetDescriptionHis = $("#AssetDesc").val();
    var AssetLabelHis = $("#AssetLabel").val();
    var SerialNumberHis = $("#AssetSerialNumber").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/AssetRetirement/GetAssetProcDetail",
        data: {
            RegId: regId,
        },
        success: function (data) {
            $("#ProcrumentDetailPartial").html(data);
            $('#AssetDescriptionPD').val(AssetDescriptionHis);
            $('#AssetLabelPD').val(AssetLabelHis);
            $('#SerialNumberPD').val(SerialNumberHis);
        }
    })
}
function AssetDepreciationHistory(e) {
    debugger;
    var regId = $("#Hdn_AssRegID").val();
    var AssetDescriptionHis = $("#AssetDesc").val();
    var AssetLabelHis = $("#AssetLabel").val();
    var SerialNumberHis = $("#AssetSerialNumber").val();
    var ProcuredValueHis = $("#ProcuredValue").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/AssetRetirement/GetAssetRegHistory",
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