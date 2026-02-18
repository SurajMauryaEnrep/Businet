/*Modified by Suraj on 12-03-2024 in Function 'OnClickSaveAndExit' To Save All Parameters even without value*/

$(document).ready(function () {
    debugger;
    inSupplierName();
    BindRejectionReasonBind();
    $("#ddlSupplierName").select2()/*add by Hina on 21-8-2024*/
    $("#SourceShfl").select2()/*add by Hina on 21-8-2024*/
    var Src_Type = $("#hdQCtype").val();
    var Src_Type = $("#qc_type  option:selected").val();
    if (Src_Type == "PRD") {
        BindProductNameDDL();
    }
    RemoveSessionData();
    var Doc_no = $("#InspectionNumber").val();
    $("#hdDoc_No").val(Doc_no);
    var message = $("#hdn_massage").val();
    if (message == "DocModify") {

    }
    else {
        BindDocumentNumberList();
    }
    debugger;
    SetSessionParam();
    $("#qc_type").change();

    var srno = 1;
    $("#QcItmDetailsTbl > tbody > tr").each(function () {
        debugger;
        var currentRow = $(this);
        var itemID = currentRow.find("#hdItemId").val();
        debugger;
        //BindItemList("#ItemName_", srno, "#QcItmDetailsTbl", "#hdRowID", "", "QC");/*Commented and add below line by HIna on 31-08-2024 under guidance suraj */
        //DynamicSerchableItemDDLForQc("#QcItmDetailsTbl", "#ItemName_", srno, "#hdRowID", "", "RndmQC", srcwh, LocationTyp);
        var Src_Type = $("#qc_type  option:selected").val();/*Add By Hina on 28-08-2024 to random qc for shopfloor*/
        if (Src_Type == "RQC") {
            /*------------------------------*/
            
            var srcwh = "";
            var LocationTyp = $("#hdnLocation_type").val();/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
            if (LocationTyp == 'WH') {
                srcwh = $("#SourceWH").val();
            }
            else {
                srcwh = $("#hdnSourceSF").val();/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
            }
            DynamicSerchableItemDDLForQc("#QcItmDetailsTbl", "#ItemName_", srno, "#hdRowID", "", "RndmQC", srcwh, LocationTyp);
            /*------------------------------*/
            //var Location_type = $("#hdnLocation_type").val();
            if (Src_Type == "RQC" && LocationTyp == "SF") {
                GetShflAvlStock(currentRow, itemID);
            }
            else {
                GetAvlStock(currentRow, itemID);
            }
        }
        else {
            GetAvlStock(currentRow, itemID);
        }
        //GetAvlStock(currentRow, itemID);/*commented By Hina on 28-08-2024 to random qc for shopfloor*/
        srno++;
    });
    $('#QcItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        debugger
        $(this).closest('tr').remove();
        var ItemID = $(this).closest('tr').find("#hdItemId").val();
        DeleteItemLotBatchDetails(ItemID);
        DeleteItemParamDetails(ItemID);
        Cmn_DeleteSubItemQtyDetail(ItemID);
        ResetSerialNo();
        var status = $("#hdQCstatuscode").val();
        if (status != "" && status != null && status != "0") {
            $("#ddlSupplierName").val("0");
            $("#Div_SupplierName").css("display", "none");
        }
        else {
            var counttable = $('#QcItmDetailsTbl tbody tr').length;
            var QCtype = $("#hdQCtype").val();

            if (counttable == 0 && (QCtype === "RQC" || QCtype === "PUR")) {
                $("#ddlSupplierName").attr("disabled", false);
            } else {
                $("#ddlSupplierName").attr("disabled", true);
            }
        }
       
    });
    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#QcItmDetailsTbl > tbody > tr').each(function () {
            var cellText = $(this).find('#ItemName_' + $(this).find('#hdRowID').val().trim()).val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
});

var TempItemID = "";
var ItemuomID = "";
var QtyDecDigit = $("#QtyDigit").text();
function BindProductNameDDL() {
    debugger;
    var Itm_ID = $("#ddl_ProductName_PRD").val();
    if (Itm_ID == null) {
        BindItemList("#ddl_ProductName_PRD", "", "", "", "", "PRDQCDetail");
    }
}
function onChangeSupplierName()
{
    var supp_id = $("#ddlSupplierName").val();
    $("#hdnSupplierID").val(supp_id);
    BindDocumentNumberList();
    if ($("#qc_type").val() == "RQC") {
        debugger;
        var counttable = $('#QcItmDetailsTbl tbody tr').length;
        if (counttable > 0) {
            var srcwh = "";
            var LocationTyp = $("#hdnLocation_type").val();
            srcwh = $("#SourceWH").val();
            var RndmQC = "RndmQC" + ',' + srcwh
            DynamicSerchableItemDDLForQc("#QcItmDetailsTbl", "#ItemName_", srno, "#hdRowID", "", "RndmQC", srcwh, LocationTyp);
        }
       
    }
}
function ProductNameItemInfo() {
    var item_id = $("#ddl_ProductName_PRD").val();
    ItemInfoBtnClick(item_id);
}
function onchangeddlProductName() {
    debugger;
    BindDocumentNumberList();
}
/*------------------------------WorkFlow JS-------------------------------------*/
function CmnGetWorkFlowDetails(e) {
    var clickedrow = $(e.target).closest("tr");
    var QC_No = clickedrow.children("#tbl_Qc_no").text();
    var QC_Date = clickedrow.children("#tbl_Qc_dt").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(Doc_id);

    var Doc_Status = clickedrow.children("#Doc_Status").text();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    GetWorkFlowDetails(QC_No, QC_Date, Doc_id, Doc_Status);
    //var a = 1;
}
function ReplicateFirstRowInAllSamples() {
    debugger;
    var TotalTh = (parseFloat($("#TotalTh").val())-1);
    for (var i = 1; i <= TotalTh; i++) {
        var sapnRow = 0;
        var flag = "N";
        var TdData = "";
        var checked = "Unchecked";
        $("#QCPrmEvalutionTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);            
            var td = currentRow.find("#td_" + i).val();
            sapnRow = sapnRow + 1;
            if (sapnRow == 2) {
                if (td != null || td != "") {
                    flag = "Y";
                    if (td == "on") {
                        if (currentRow.find('#td_' + i).is(":checked")) {
                            checked = "checked"
                        }                        
                        TdData = checked;
                    }
                    else {
                        TdData = td;
                    }                   
                }
            }
            if (flag == "Y") {
                if (td == "on") {
                    if (TdData == "checked") {
                        currentRow.find("#td_" + i).prop("checked", true)
                    }
                    else {
                        currentRow.find("#td_" + i).prop("checked", false)
                    }
                }
                else {
                    currentRow.find("#td_" + i).val(TdData);
                    validateParametersVal("td_" + i, TdData, currentRow);
                }           
            }
        });
    }
}
function validateParametersVal(id, valParam, currentRow) {
    var ParametersVal = $("#QCPrmEvalutionTbl > tbody> tr:eq(0)").find("#" + id).text().trim().split(" ");
    if (ParametersVal == "Observative") {

    }
    else if ((parseFloat(ParametersVal[2]) >= parseFloat(valParam)) && (parseFloat(valParam) >= parseFloat(ParametersVal[0]))) {
        currentRow.find("#" + id).closest("input").css("color", "");
    }
    else {
        currentRow.find("#" + id).css("color", "red");
    }

    if (valParam > 0) {
        currentRow.find("#" + id).closest("input").css("border", "none");
        currentRow.find("#" + id).closest("input").parent().find("#ErrMsg_" + id).text("");
        currentRow.find("#" + id).closest("input").parent().find("#ErrMsg_" + id).css("display", "none");
    }
}
function ReplicateWithAllSamples(e) {
    debugger;
    var clickedrow = $(e.target).closest("th");
    var Srno = clickedrow.find("#ThDeatilSrNo").val();
    var data = $("#th_" + Srno).text().trim();
    var ParamType = $("#QCPrmEvalutionTbl > tbody> tr:eq(0)").find("#td_" + Srno + " #ParamTypeValue").val();
    var tdHeader = $("#td_" + Srno).text().trim();
    $("#thReplicateDeatilCount").text(Srno);
    if (ParamType == "O") {
        //$("#ObservativeId").css("display", "block");
        $("#ObservativeId").removeAttr("style");
        $("#thHeading1").text(data);
        $("#tdHeaderData1").text(tdHeader);
        $("#thReplicateDeatil").text(ParamType);
        $("#td_Observative").val("");
    }
    else {
        $("#ObservativeId").css("display", "none");
    }
    if (ParamType == "N") {
        //$("#QuantitativeId").css("display", "block");
        $("#QuantitativeId").removeAttr("style");
        $("#thHeading2").text(data);
        $("#tdHeaderData2").text(tdHeader);
        $("#thReplicateDeatil").text(ParamType);
        $("#td_ParamVal").val("");

    }
    else {
        $("#QuantitativeId").css("display", "none");
    }
    if (ParamType == "L") {
        //$("#QualitativeId").css("display", "block");
        $("#QualitativeId").removeAttr("style");
        $("#thHeading").text(data);
        $("#tdHeaderData").text(tdHeader);
        $("#thReplicateDeatil").text(ParamType);
        $("#td_Checkd").prop("checked", true)
    }
    else {
        $("#QualitativeId").css("display", "none");
    }
}
function OnClickSaveAndExitReplicateBtn() {
    debugger;
    var checked = "Unchecked";
    var td_Observative = "";
    var td_ParamVal = "";
    var type = $("#thReplicateDeatil").text();
    if (type == "O") {
        td_Observative = $("#td_Observative").val();
    }
    else if (type == "L") {
        if ($('#td_Checkd').is(":checked")) {
            checked = "checked"
        }
    }
    else {
        td_ParamVal = $("#td_ParamVal").val();
    }
    var count = $("#thReplicateDeatilCount").text();
    var rowid = 1;
    $("#QCPrmEvalutionTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        if (type == "O") {
            currentRow.find("#td_" + count).val(td_Observative);
        }
        else if (type == "L") {
            if (checked == "checked") {
                currentRow.find("#td_" + count).prop("checked", true)
            }
            else {
                currentRow.find("#td_" + count).prop("checked", false)
            }
        }
        else {
            currentRow.find("#td_" + count).val(td_ParamVal);
            if (rowid != 1) {
                validateParametersVal("td_" + count, td_ParamVal, currentRow);
            }
        }
        rowid = rowid + 1;
    });
    $("#thReplicateDeatil").text("");
    $("#SaveExitReplicateBtn").attr("data-dismiss", "modal");
}
function ForwardBtnClick() {
    debugger;
    //var Doc_Status = "";
    //Doc_Status = $('#hdQCstatuscode').val().trim();
    //if (Doc_Status === "D" || Doc_Status === "F") {

    //    //if (CheckQCItemParamsValidations() == false) {
    //    //    $("#btn_forward").attr("data-target", "");
    //    //    return false;
    //    //} else {

    //        if ($("#hd_nextlevel").val() === "0") {
    //            $("#btn_forward").attr("data-target", "");
    //        }
    //        else {
    //            $("#btn_forward").attr("data-target", "#Forward_Pop");
    //            $("#Btn_Approve").attr("data-target", "");
    //        }
    //        var Doc_Menu_ID = $("#DocumentMenuId").val();
    //        $("#OKBtn_FW").attr("data-dismiss", "modal");

    //        Cmn_GetForwarderList(Doc_Menu_ID);
    //    //}


    //}
    //else {
    //    $("#btn_forward").attr("data-target", "");
    //    $("#btn_forward").attr('onclick', '');
    //    $("#btn_forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var QcDt = $("#txtQCDate").val();
        $.ajax({
            type: "POST",
            /* url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: QcDt
            },
            success: function (data) {
                /* if (data == "Exist") { *//*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var Doc_Status = "";
                    Doc_Status = $('#hdQCstatuscode').val().trim();
                    if (Doc_Status === "D" || Doc_Status === "F") {

                        //if (CheckQCItemParamsValidations() == false) {
                        //    $("#btn_forward").attr("data-target", "");
                        //    return false;
                        //} else {

                        if ($("#hd_nextlevel").val() === "0") {
                            $("#btn_forward").attr("data-target", "");
                        }
                        else {
                            $("#btn_forward").attr("data-target", "#Forward_Pop");
                            $("#Btn_Approve").attr("data-target", "");
                        }
                        var Doc_Menu_ID = $("#DocumentMenuId").val();
                        $("#OKBtn_FW").attr("data-dismiss", "modal");

                        Cmn_GetForwarderList(Doc_Menu_ID);
                        //}


                    }
                    else {
                        $("#btn_forward").attr("data-target", "");
                        $("#btn_forward").attr('onclick', '');
                        $("#btn_forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                }
                else {/* to chk Financial year exist or not*/
                    /*  swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                    /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Btn_Approve").attr("data-target", "");
                    $("#Forward_Pop").attr("data-target", "");

                }
            }
        });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    /*End to chk Financial year exist or not*/
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var QC_No = "";
    var QC_Date = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var mailerror = "";

    Remarks = $("#fw_remarks").val();
    QC_No = $("#InspectionNumber").val();
    QC_Date = $("#txtQCDate").val();
    docid = $("#DocumentMenuId").val();
    qc_type = $('#qc_type').val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var WF_status1 = $("#WF_status1").val();
    var qc_type = $("#qc_type").val();
    var Loc_type = $("#hdnLocation_type").val();
    var TrancType = (QC_No + ',' + qc_type + ',' + docid + ',' + WF_status1)
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("td:eq(3)").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("td:eq(3)").text();
            level = currentRow.find("td:eq(4)").text();
        }
    });
    //Added by Nidhi on 09-07-2025
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "QualityInspection_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await GetPdfFilePathToSendonEmailAlert(QC_No, QC_Date, qc_type, pdfAlertEmailFilePath, docid, fwchkval);
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && QC_No != "" && QC_Date != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(QC_No, QC_Date, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/QualityInspection/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ qc_type: $("#qc_type").val(), Loc_type: Loc_type, qc_no: QC_No, qc_dt: QC_Date, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks, DocMenuId: docid }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/QualityInspection/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&docid=" + docid + "&qc_type=" + qc_type + "&Loc_type=" + Loc_type;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && QC_No != "" && QC_Date != "") {
            await Cmn_InsertDocument_ForwardedDetail1(QC_No, QC_Date, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/QualityInspection/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && QC_No != "" && QC_Date != "") {
            await Cmn_InsertDocument_ForwardedDetail1(QC_No, QC_Date, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/QualityInspection/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
//Added by Nidhi on 09-07-2025
async function GetPdfFilePathToSendonEmailAlert(QC_No, QC_Date, qc_type, fileName, docid, docstatus) {
    var filepath = "";
    debugger;
    filepath = await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/QualityInspection/SavePdfDocToSendOnEmailAlert",
        data: { Doc_no: QC_No, Doc_dt: QC_Date, qc_type: qc_type, fileName: fileName, docid: docid, docstatus: docstatus },
        /*dataType: "json",*/
        success: function (data) {
            debugger;
        }
       
    });
    return filepath;
}
function OtherFunctions(StatusC, StatusName) {

}
function ForwardHistoryBtnClick() {
    var Doc_Menu_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InspectionNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_Menu_ID != "" && Doc_Menu_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_Menu_ID);
    return false;
}
/*---------------------------------------WorkFlow JS End---------------------------------------*/
//function DirecrtBtnClick() {
//    if (CheckQCItemParamsValidations() == false) {
//        return false;
//    } else {
//        return true;
//    }
//}
function DeleteItemLotBatchDetails(ItemID) {
    var LBDetails = sessionStorage.getItem("ItemLBQCDetails");
    let arrey = [];
    if (LBDetails != null) {
        if (LBDetails.length > 0) {
            arrey.push({ ItmCode: ItemID });
            LBDetails = JSON.parse(LBDetails);
            LBDetails = LBDetails.filter(j => !JSON.stringify(arrey).includes(j.ItmCode));
            sessionStorage.removeItem("ItemLBQCDetails");
            sessionStorage.setItem("ItemLBQCDetails", JSON.stringify(LBDetails));
        }
    }
}
function DeleteItemParamDetails(ItemID) {
    var LBDetails = sessionStorage.getItem("ItemQCParamDetails");
    let arrey = [];
    if (LBDetails != null) {
        if (LBDetails.length > 0) {
            arrey.push({ ItmCode: ItemID });
            LBDetails = JSON.parse(LBDetails);
            LBDetails = LBDetails.filter(j => !JSON.stringify(arrey).includes(j.ItmCode));
            sessionStorage.removeItem("ItemQCParamDetails");
            sessionStorage.setItem("ItemQCParamDetails", JSON.stringify(LBDetails));
        }
    }
}
function SetSessionParam() {

    if ($("#hdQCLotBatchDetailList").val() != null && $("#hdQCLotBatchDetailList").val() != "") {
        var arr2 = $("#hdQCLotBatchDetailList").val();
        var arr = JSON.parse(arr2);
        $("#hdQCLotBatchDetailList").val("");
        if (arr.length > 0) {

            sessionStorage.removeItem("ItemLBQCDetails");
            sessionStorage.setItem("ItemLBQCDetails", JSON.stringify(arr));
        }
    }
    if ($("#hdTblQCItemParamDetailList tbody tr").length > 0) {
        let NewArr = [];
        var j = 0;

        $("#hdTblQCItemParamDetailList tbody tr").each(function () {
            var CurrentRow = $(this);

            var ItmCode = CurrentRow.find("#item_id").text().trim();
            var ItmName = CurrentRow.find("#item_name").text().trim();
            var ItemUOMID = CurrentRow.find("#uom_id").text().trim();
            var UOMName = CurrentRow.find("#uom_name").text().trim();
            var ItemSampSize = CurrentRow.find("#sam_size").text().trim();
            var ParamName = CurrentRow.find("#param_name").text().trim();
            var ParamID = CurrentRow.find("#param_Id").text().trim();
            var param_uom_Id = CurrentRow.find("#param_uom_Id").text().trim();
            var ParamTypeCode = CurrentRow.find("#param_type").text().trim();
            var ParamType = CurrentRow.find("#paramtype").text().trim();
            var ParamUOM = CurrentRow.find("#uom_alias").text().trim();
            var UpperRange = CurrentRow.find("#upper_val").text().trim();
            var LowerRange = CurrentRow.find("#lower_val").text().trim();
            var Result = CurrentRow.find("#Result").text().trim();
            var Action = CurrentRow.find("#param_action").text().trim();
            var ToggleResult = CurrentRow.find("#Result").text().trim();
            var SRNumber = CurrentRow.find("#sr_no").text().trim();
            NewArr.push({
                ItmCode: ItmCode, ItmName: ItmName, ItemUOMID: ItemUOMID, UOMName: UOMName, ItemSampSize: ItemSampSize,
                ParamName: ParamName, ParamID: ParamID, ParamUOMID: param_uom_Id, ParamTypeCode: ParamTypeCode,
                ParamType: ParamType, ParamUOM: ParamUOM, UpperRange: UpperRange,
                LowerRange: LowerRange, Result: Result, ToggleResult: ToggleResult,
                Action: Action, Remarks: "", SRNumber: SRNumber, td_no: j
            })
            j++;
        });
        sessionStorage.removeItem("ItemQCParamDetails");
        sessionStorage.setItem("ItemQCParamDetails", JSON.stringify(NewArr));
    }

    //    if ($("#hdQCItemParamDetailList").val() != null && $("#hdQCItemParamDetailList").val() != "") {
    //        var arr2 = $("#hdQCItemParamDetailList").val();
    //        var arr = JSON.parse(arr2);
    //        $("#hdQCItemParamDetailList").val("");
    //        let NewArr = [];
    //        if (arr.length > 0) {
    //            for (var j = 0; j < arr.length; j++) {

    //                var ItmCode = arr[j].item_id.trim();
    //                var ItemUOMID = arr[j].uom_id.trim();
    //                var ItemSampSize = arr[j].sam_size.trim();
    //                var ParamName = arr[j].param_name.trim();
    //                var ParamID = arr[j].param_Id.trim();
    //                var ParamTypeCode = arr[j].param_type.trim();
    //                var ParamType = arr[j].paramtype.trim();
    //                var ParamUOM = arr[j].uom_alias.trim();
    //                var UpperRange = arr[j].upper_val.trim();
    //                var LowerRange = arr[j].lower_val.trim();
    //                var Result = arr[j].Result.trim();
    //                var Action = arr[j].param_action.trim();
    //                var ToggleResult = arr[j].Result.trim();
    //                var SRNumber = arr[j].sr_no.trim();
    //                NewArr.push({
    //                    ItmCode: ItmCode, ItemUOMID: ItemUOMID, ItemSampSize: ItemSampSize,
    //                    ParamName: ParamName, ParamID: ParamID, ParamTypeCode: ParamTypeCode,
    //                    ParamType: ParamType, ParamUOM: ParamUOM, UpperRange: UpperRange,
    //                    LowerRange: LowerRange, Result: Result, ToggleResult: ToggleResult,
    //                    Action: Action, Remarks: "", SRNumber: SRNumber, td_no: j
    //                })

    //            }
    //            sessionStorage.removeItem("ItemQCParamDetails");
    //            sessionStorage.setItem("ItemQCParamDetails", JSON.stringify(NewArr));
    //        }
    //    }
}

function CheckFormValidation() {
    debugger;
    var rowcount = $('#QcItmDetailsTbl tr').length;
    var ValidationFlag = true;
    var DocumentNumber = $('#ddlDocumentNumber option:selected').text();
    var DocDate = $('#txtsrcdocdate').val();
    var ReworkWH = $('#ReworkWH').val();
    var RejectWH = $('#RejectWH').val();
    var Src_Type = $("#qc_type  option:selected").val();
    if (Src_Type != "RQC") {
        if (DocumentNumber == "" || DocumentNumber == "0" || DocumentNumber == "---Select---" || DocumentNumber == null) {
            document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
            $("#ddlDocumentNumber").css("border-color", "red");
            $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
            ValidationFlag = false;
        }
    }



    if (Src_Type == "PUR" || Src_Type == "RQC" || Src_Type == "SCQ") {
        var RewQtyStatus = "N";
        $("#QcItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var ReworkQty = "";
            var currentRow = $(this);
            ReworkQty = currentRow.find("#ReworkableQty").val();
            if (parseFloat(ReworkQty) > parseFloat(0)) {
                RewQtyStatus = "Y";
            }
        });
        if (Src_Type == "RQC") {/*Add By Hina on 21-08-2024 to random qc for shopfloor*/
            var LocationTyp = $("#hdnLocation_type").val();/*Add By Hina on 23-08-2024 to random qc for shopfloor*/
            var SrcShfl = $("#hdnSourceSF").val();
            if (LocationTyp == "WH") {
                if (RewQtyStatus == "Y") {
                    if (ReworkWH == "0" || ReworkWH == "") {
                        document.getElementById("vmReworkWH").innerHTML = $("#valueReq").text();
                        $("#ReworkWH").css("border-color", "red");
                        ValidationFlag = false;
                    }
                    else {
                        document.getElementById("vmReworkWH").innerHTML = null;
                        $("#ReworkWH").css("border-color", "#ced4da");
                    }
                }
                else {
                    $("#ReworkWH").attr("disabled", true);
                    $("#spanReWReq").attr("style", "display: none;");
                    document.getElementById("vmReworkWH").innerHTML = null;
                    $("#ReworkWH").css("border-color", "#ced4da");
                }
            }
            else {
                if (SrcShfl == "" || SrcShfl == "0" || SrcShfl == null) {
                    //$("#SourceShfl").css("border-color", "red");
                    $('[aria-labelledby="select2-SourceShfl-container"]').css("border-color", "red");
                    
                    document.getElementById("vmSourceSF").innerHTML = $("#valueReq").text();
                    ValidationFlag = false;
                }
                else {
                    //$("#SourceShfl").css("border-color", "#ced4da");
                    $('[aria-labelledby="select2-SourceShfl-container"]').css("border-color", "#ced4da");
                    document.getElementById("vmSourceSF").innerHTML = null;
                }
            }
        }

        else {
            if (RewQtyStatus == "Y") {
                if (ReworkWH == "0" || ReworkWH == "") {
                    document.getElementById("vmReworkWH").innerHTML = $("#valueReq").text();
                    $("#ReworkWH").css("border-color", "red");
                    ValidationFlag = false;
                }
                else {
                    document.getElementById("vmReworkWH").innerHTML = null;
                    $("#ReworkWH").css("border-color", "#ced4da");
                }
            }
            else {
                $("#ReworkWH").attr("disabled", true);
                $("#spanReWReq").attr("style", "display: none;");
                document.getElementById("vmReworkWH").innerHTML = null;
                $("#ReworkWH").css("border-color", "#ced4da");
            }
        }
        

        var RejQtyStatus = "N";
        var AccQtyStatus = "N";
        $("#QcItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var RejectQty = "";
            var currentRow = $(this);
            RejectQty = currentRow.find("#RejectQty").val();
            var AcceptQty = currentRow.find("#AcceptQty").val();
            if (parseFloat(CheckNullNumber(RejectQty)) > parseFloat(0)) {
                RejQtyStatus = "Y";
            }
            if (parseFloat(CheckNullNumber(AcceptQty)) > parseFloat(0)) {
                AccQtyStatus = "Y";
            }
        });
        if (Src_Type == "RQC") {
            var SourceWH = $("#SourceWH").val();
            var LocationTyp = $("#hdnLocation_type").val();/*Add By Hina on 21-08-2024 to random qc for shopfloor*/
            var SrcShfl = $("#hdnSourceSF").val();
            if (LocationTyp == "WH") {
                if (SourceWH == "" || SourceWH == "0" || SourceWH == null) {
                    $("#SourceWH").css("border-color", "red");
                    document.getElementById("vmSourceWH").innerHTML = $("#valueReq").text();
                    ValidationFlag = false;
                }
                else {
                    $("#SourceWH").css("border-color", "#ced4da");
                    document.getElementById("vmSourceWH").innerHTML = null;
                }
            }
            else {
                if (SrcShfl == "" || SrcShfl == "0" || SrcShfl == null) {
                    //$("#SourceShfl").css("border-color", "red");/*for non serachable dropdown*/
                    
                    $('[aria-labelledby="select2-SourceShfl-container"]').css("border-color", "red");/*for serachable drop down*/
                    document.getElementById("vmSourceSF").innerHTML = $("#valueReq").text();
                    ValidationFlag = false;
                }
                else {
                    //$("#SourceShfl").css("border-color", "#ced4da");
                    $('[aria-labelledby="select2-SourceShfl-container"]').css("border-color", "#ced4da");/*for serachable drop down*/
                    document.getElementById("vmSourceSF").innerHTML = null;
                }
            }
            if (LocationTyp == "WH") {
                if (AccQtyStatus == "Y") {
                    var AcceptedWH = $("#AcceptedWH").val();
                    if (AcceptedWH == "" || AcceptedWH == "0" || AcceptedWH == null) {
                        $("#AcceptedWH").css("border-color", "red");
                        document.getElementById("vmAcceptedWH").innerHTML = $("#valueReq").text();
                        ValidationFlag = false;
                    } else {
                        $("#AcceptedWH").css("border-color", "#ced4da");
                        document.getElementById("vmAcceptedWH").innerHTML = null;
                    }
                }
                else {
                    $("#AcceptedWH").attr("disabled", true);
                    $("#AcceptedWH").css("border-color", "#ced4da");
                    document.getElementById("vmAcceptedWH").innerHTML = null;
                }
            }
            

        }
        if (Src_Type == "RQC") {/*Add by Hina on 23-08-2024 to random qc for shopfloor*/
            if (LocationTyp == "WH") {
                if (RejQtyStatus == "Y") {
                    if (RejectWH == "0" || RejectWH == "") {
                        document.getElementById("vmRejectWH").innerHTML = $("#valueReq").text();
                        $("#RejectWH").css("border-color", "red");
                        ValidationFlag = false;
                    }
                    else {
                        document.getElementById("vmRejectWH").innerHTML = null;
                        $("#RejectWH").css("border-color", "#ced4da");
                    }
                }
                else {
                    $("#RejectWH").attr("disabled", true);
                    $("#spanRejReq").attr("style", "display: none;");
                    document.getElementById("vmRejectWH").innerHTML = null;
                    $("#RejectWH").css("border-color", "#ced4da");
                }
            }
            
        }
        else {
            if (RejQtyStatus == "Y") {
                if (RejectWH == "0" || RejectWH == "") {
                    document.getElementById("vmRejectWH").innerHTML = $("#valueReq").text();
                    $("#RejectWH").css("border-color", "red");
                    ValidationFlag = false;
                }
                else {
                    document.getElementById("vmRejectWH").innerHTML = null;
                    $("#RejectWH").css("border-color", "#ced4da");
                }
            }
            else {
                $("#RejectWH").attr("disabled", true);
                $("#spanRejReq").attr("style", "display: none;");
                document.getElementById("vmRejectWH").innerHTML = null;
                $("#RejectWH").css("border-color", "#ced4da");
            }
        }
        
    }
    //else if (Src_Type == "") {

    //}
    if (ValidationFlag == true) {
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        if (rowcount > 1) {
          
            return CheckItemValidation();
            var flag = CheckQCResultValidations();
            if (flag == true) {
                debugger
                //var QCNo = $("hdQCNo").val();
                //$("#InspectionNunber").val(QCNo);

                //var QCDate = $('#txtQCDate').val();
                //$("#txtQCDate").val(QCDate);


                var QCItemDetailList = new Array();
                $("#QcItmDetailsTbl TBODY TR").each(function () {
                    debugger;
                    var row = $(this);
                    var rownumber = row.find("#SpanRowId").text();
                    var ItemList = {};
                    ItemList.ItemId = row.find('#hdItemId').val();
                    ItemList.UOMId = row.find('#UOMID').val();
                    ItemList.RecievedQty = row.find("#RecivedQty").val();
                    ItemList.AcceptedQty = row.find("#AcceptQty").val();
                    ItemList.RejectedQty = row.find("#RejectQty").val();
                    ItemList.ReworkableQty = row.find("#ReworkableQty").val();
                    ItemList.ShortQty = row.find("#ShortQty").val();
                    ItemList.SampleQty = row.find("#SampleQty").val();
                    ItemList.ItemRemarks = row.find('#txtItemRemarks').val();
                    if (Src_Type == "PRD") {
                        ItemList.ProdQty = row.find('#ProdQty').val();
                        ItemList.PendQty = row.find('#PendQty').val();
                    }
                    QCItemDetailList.push(ItemList);
                    debugger;
                });

                var str = JSON.stringify(QCItemDetailList);
                $('#hdQCItemDetailList').val(str);
                return true;
            }
            else {
                return false;
            }
        }
        else {

            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }
    }
    else {
        return false;
    }

}
function CheckQCLotBatchValidations() {
    debugger
    var ErrorFlag = "N";
    var LBDetails = sessionStorage.getItem("ItemLBQCDetails");
    if (LBDetails != null) {
        if (LBDetails.length > 0) {
            $("#QcItmDetailsTbl >tbody >tr").each(function () {

                var currentRow = $(this);
                var ItemId = currentRow.find("#hdItemId").val();
                //var ItemName = currentRow.find("#ItemName").val();
                var RecvdQty = currentRow.find("#RecivedQty").val();
                var filteredValue = JSON.parse(LBDetails).filter(v => v.ItmCode == ItemId);
                if (filteredValue != null) {
                    if (filteredValue.length > 0) {
                        var chkLotQty = 0;
                        filteredValue.map((item) => {
                            chkLotQty = parseFloat(chkLotQty) + parseFloat(item.LBAccQty) + parseFloat(item.LBRejQty) + parseFloat(item.LBRewQty) + parseFloat(item.LBSampleQty) + parseFloat(item.LBShortQty);
                        })
                        if (parseFloat(chkLotQty).toFixed(QtyDecDigit) != parseFloat(RecvdQty).toFixed(QtyDecDigit)) {
                            ErrorFlag = "Y";
                            ValidateEyeColor(currentRow, "LotBatchDetailsBtn", "Y");
                        }
                    }
                    else {

                        ErrorFlag = "Y";
                        ValidateEyeColor(currentRow, "LotBatchDetailsBtn", "Y");
                        //return false;
                    }
                }
                else {
                    ErrorFlag = "Y";
                    ValidateEyeColor(currentRow, "LotBatchDetailsBtn", "Y");
                    //return false;
                }
            })
            //ErrorFlag = "Y";
        }
    }

    if (ErrorFlag == "N") {
        return true;
    }
    else {
        return false;
    }
}
function CheckQCInsightbtnValidations() {
    var ErrorFlag = "N";
    $("#QcItmDetailsTbl >tbody >tr").each(function () {
        var QtyDecDigit = $("#QtyDigit").text();
        var currentRow = $(this);
        var AcceptQty = parseFloat(CheckNullNumber(currentRow.find("#AcceptQty").val())).toFixed(QtyDecDigit);
        var RejectQty = parseFloat(CheckNullNumber(currentRow.find("#RejectQty").val())).toFixed(QtyDecDigit);
        var ReworkableQty = parseFloat(CheckNullNumber(currentRow.find("#ReworkableQty").val())).toFixed(QtyDecDigit);
        var ReasonForReject = currentRow.find("#ReasonForReject").val();
        var ReasonForRework = currentRow.find("#ReasonForRework").val();
        var ReasonForAccecpt = currentRow.find("#ReasonForAccecpt").val();
      
        if (parseFloat(RejectQty) > 0) {
            if ((ReasonForReject == null ? "" : ReasonForReject).trim().length > 0) {
                currentRow.find("#BtnReasonForReject").css("border", "none")
            } else {
                ErrorFlag = "Y";
                currentRow.find("#BtnReasonForReject").css("border", "1px solid red");
            }
        }
        else {
            currentRow.find("#BtnReasonForReject").css("border", "none")
        }
        if (parseFloat(ReworkableQty) > 0) {
            if ((ReasonForRework == null ? "" : ReasonForRework).trim().length > 0) {
                currentRow.find("#BtnReasonForRework").css("border", "none")
            } else {
                ErrorFlag = "Y";
                currentRow.find("#BtnReasonForRework").css("border", "1px solid red");
            }
        } else {
            currentRow.find("#BtnReasonForRework").css("border", "none")
        }
        //if (parseFloat(AcceptQty) > 0) {
        //    if ((ReasonForAccecpt == null ? "" : ReasonForAccecpt).trim().length > 0) {
        //        currentRow.find("#BtnReasonForAccecpt").css("border", "none")
        //    } else {
        //        ErrorFlag = "Y";
        //        currentRow.find("#BtnReasonForAccecpt").css("border", "1px solid red");
        //    }
        //} else {
        //    currentRow.find("#BtnReasonForAccecpt").css("border", "none")
        //}
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckQCItemValidations() {
    var qc_type = $("#qc_type").val();
    var ErrorFlag = "N";
    $("#QcItmDetailsTbl >tbody >tr").each(function () {

        var QtyDecDigit = $("#QtyDigit").text();
        var currentRow = $(this);
        var ReceivedQty = parseFloat(CheckNullNumber(currentRow.find("#RecivedQty").val())).toFixed(QtyDecDigit);
        var AcceptQty = parseFloat(CheckNullNumber(currentRow.find("#AcceptQty").val())).toFixed(QtyDecDigit);
        var RejectQty = parseFloat(CheckNullNumber(currentRow.find("#RejectQty").val())).toFixed(QtyDecDigit);
        var ReworkableQty = parseFloat(CheckNullNumber(currentRow.find("#ReworkableQty").val())).toFixed(QtyDecDigit);
        var ShortQty = parseFloat(CheckNullNumber(currentRow.find("#ShortQty").val())).toFixed(QtyDecDigit);
        var SampleQty = parseFloat(CheckNullNumber(currentRow.find("#SampleQty").val())).toFixed(QtyDecDigit);
        if (qc_type == "PRD") {
            if (ReceivedQty == 0) {
                currentRow.find("#RecivedQty").css("border-color", "red");
                currentRow.find("#PRDRecivedQty_specError").text($("#valueReq").text());
                currentRow.find("#PRDRecivedQty_specError").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#RecivedQty").css("border-color", "#ced4da");
                currentRow.find("#PRDRecivedQty_specError").text("");
                currentRow.find("#PRDRecivedQty_specError").css("display", "none");
            }
        }
        var ItemName = currentRow.find("#ItemName").val();
        var ItemID = currentRow.find("#hdItemId").val();
        if (qc_type != "PRD") {
            var ReasonForReject = currentRow.find("#ReasonForReject").val();
            var ReasonForRework = currentRow.find("#ReasonForRework").val();
            if (parseFloat(RejectQty) > 0) {
                if ((ReasonForReject == null ? "" : ReasonForReject).trim().length > 0) {
                    currentRow.find("#BtnReasonForReject").css("border", "none")
                } else {
                    ErrorFlag = "Y";
                    currentRow.find("#BtnReasonForReject").css("border", "1px solid red");
                }
            }
            else {
                currentRow.find("#BtnReasonForReject").css("border", "none")
            }
            if (parseFloat(ReworkableQty) > 0) {
                if ((ReasonForRework == null ? "" : ReasonForRework).trim().length > 0) {
                    currentRow.find("#BtnReasonForRework").css("border", "none")
                } else {
                    ErrorFlag = "Y";
                    currentRow.find("#BtnReasonForRework").css("border", "1px solid red");
                }
            } else {
                currentRow.find("#BtnReasonForRework").css("border", "none")
            }
        }
        if ((parseFloat(ReceivedQty) == parseFloat((parseFloat(AcceptQty) + parseFloat(RejectQty) + parseFloat(ReworkableQty)
            + parseFloat(CheckNullNumber(ShortQty)) + parseFloat(CheckNullNumber(SampleQty)))).toFixed(QtyDecDigit))) {
        }
        else {
            if ($("#qc_type").val() == "RQC") {
                var arr = sessionStorage.getItem("ItemLBQCDetails");
                if (arr != null) {

                    if (arr.length > 0) {
                        if (arr.match(ItemID) != null) {
                            ErrorFlag = "RY";
                            //currentRow.find("#LotBatchDetailsBtn").css("border-color", "red");
                            ValidateEyeColor(currentRow, "LotBatchDetailsBtn", "Y");
                        } else {
                            //currentRow.find("#LotBatchDetailsBtn").css("border-color", "red");
                            ValidateEyeColor(currentRow, "LotBatchDetailsBtn", "Y");
                            ErrorFlag = "RY";
                            //swal("", $("#InspectedQty").text() + " " + ItemName, "warning");
                            //return false;
                        }
                    }

                    //return false;
                } else {
                    //currentRow.find("#LotBatchDetailsBtn").css("border-color", "red");
                    //swal("", $("#InspectedQty").text() + " " + ItemName, "warning");
                    ErrorFlag = "RY";
                    //currentRow.find("#LotBatchDetailsBtn").css("border-color", "red");
                    ValidateEyeColor(currentRow, "LotBatchDetailsBtn", "Y");
                    // return false;
                }
            } else {
                swal("", $("#InspectedQty").text() + " " + ItemName, "warning");
                ErrorFlag = "Y";
                return false;
            }

        }

    })
    debugger;
    debugger;
    debugger;
    if (ErrorFlag == "Y") {
        return false;
    }
    else if (ErrorFlag == "RY") {
        swal("", $("#LotBatchDetailsNotFound").text(), "warning");
        return false;
    }
    else {
        return true;
    }
}
function CheckQCItemParamsValidations() {
    debugger;
    var ErrorFlag = "N";
    var countNoErr = 0;
    var items = '';
    $("#QcItmDetailsTbl >tbody >tr").each(function () {
        debugger;

        var QtyDecDigit = $("#QtyDigit").text();
        var currentRow = $(this);
        var ItemName = currentRow.find("#ItemName").val();
        var ItemID = currentRow.find("#hdItemId").val();
        if (items == '') {
            items += ItemID;
        } else {
            items += ',' + ItemID;
        }
    });
    var hdQCstatuscode = $("#hdQCstatuscode").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/QualityInspection/GetAllItemQCParamDetail",
        data: {
            ItemID: items,
            qc_no: "",
            qc_dt: "",
            TransType: "",
            Command: "",
            DocumentStatus: hdQCstatuscode
        },
        async: false,
        success: function (data) {
            var arr = JSON.parse(data);
            var FResultDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));

            for (var i = 0; i < arr.Table.length; i++) {
                var item_id = arr.Table[i].item_id;
                var param_id = arr.Table[i].param_Id;
                var Itemparamdata = FResultDetails.filter(v => v.ItmCode == item_id).filter(v => v.ParamID == param_id);
                if (Itemparamdata.length == 0) {
                    var Crt_item_row = $("#QcItmDetailsTbl tbody tr #hdItemId[value='" + item_id + "']")
                        .closest('tr');
                    var item_nm = Crt_item_row.find("#ItemName").val();
                    //Crt_item_row.find("#ParamEvluationImg").css("border-color", "1px solid red");
                    ErrorFlag = "Y"
                    //swal("", $("#ParameterEvaluationIsIncompleteForItem").text() + " - " + item_nm, "warning");

                    //return false;
                } else {
                    countNoErr++;
                }

            }
            if (ErrorFlag == "Y" && countNoErr == 0) {
                swal("", $("#ParameterEvaluationIsIncompleteForItem").text() + " - " + item_nm, "warning");

                return false;
            }
        }
    });
    if (ErrorFlag == "Y" && countNoErr == 0) {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemValidation() {
    debugger;
    var ErrorFlag = "N";
    var qc_type = $("#qc_type").val();
    var ItemName;
    $("#QcItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var ItemId = currentRow.find("#hdItemId").val();
        ItemName = currentRow.find("#ItemName").val();
        if (qc_type == "RQC") {
            var hdRowID = currentRow.find("#hdRowID").val();
            ItemId = currentRow.find("#ItemName_" + hdRowID).val();
            if (ItemId == "" || ItemId == "0" || ItemId == null) {
                currentRow.find('[aria-labelledby="select2-ItemName_' + hdRowID + '-container"]').css("border-color", "red");
                currentRow.find("#ItemNameError").text($("#valueReq").text());
                currentRow.find("#ItemNameError").css("display", "block");
                ErrorFlag = "RY";
            } else {
                currentRow.find('[aria-labelledby="select2-ItemName_' + hdRowID + '-container"]').css("border-color", "#ced4da");
                currentRow.find("#ItemNameError").text("");
                currentRow.find("#ItemNameError").css("display", "none");
            }
            var RecivedQty = currentRow.find("#RecivedQty").val();
            if (parseFloat(CheckNullNumber(RecivedQty)) == 0) {
                currentRow.find("#RecivedQty").css("border-color", "red");
                currentRow.find("#RecivedQty_specError").text($("#valueReq").text());
                currentRow.find("#RecivedQty_specError").css("display", "block");
                ErrorFlag = "RY";
            } else {
                currentRow.find("#RecivedQty").css("border-color", "#ced4da");
                currentRow.find("#RecivedQty_specError").text("");
                currentRow.find("#RecivedQty_specError").css("display", "none");
            }
        }

    })
    if (ErrorFlag == "RY") {
        return false;
    }
    else {
        return true;
    }
}
function CheckQCResultValidations() {
    debugger
    var ErrorFlag = "N";
    var Src_Type = $("#qc_type  option:selected").val();
    var ItemName;
    $("#QcItmDetailsTbl >tbody >tr").each(function () {

        var currentRow = $(this);
        var ItemId = currentRow.find("#hdItemId").val();
        ItemName = currentRow.find("#ItemName").val();


        var FQCDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
        if (FQCDetails != null) {
            var filteredValue = FQCDetails.filter(function (item) {
                return item.ItmCode == ItemId;
            });
            if (filteredValue != null) {
                if (filteredValue.length > 0) {
                }
                else {

                    ErrorFlag = "Y";
                    currentRow.find("#ParameterEvlsnPopUp").css("outline", "1px solid red");
                    swal("", $("#InsepetionResultWarning").text() + " " + ItemName, "warning");
                    return false;
                }
            }
            else {
                ErrorFlag = "Y";
                currentRow.find("#ParameterEvlsnPopUp").css("outline", "1px solid red");
                swal("", $("#InsepetionResultWarning").text() + " " + ItemName, "warning");
                return false;
            }
        }
        else {
            ErrorFlag = "Y";
            currentRow.find("#ParameterEvlsnPopUp").css("outline", "1px solid red");

            swal("", $("#InsepetionResultWarning").text() + " " + ItemName, "warning");
            return false;
        }
    })

    if (ErrorFlag == "Y") {

        swal("", $("#InsepetionResultWarning").text() + " " + ItemName, "warning");
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
        confirmButtonText: $("#yesdeleteit").text() + "!",
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
function BindDocumentNumberList() {
    debugger;
    var suppid = "0";
    var item_id = $("#ddl_ProductName_PRD").val();
    var Src_Type = $("#qc_type  option:selected").val();
    var hdQCstatuscode = $("#hdQCstatuscode").val();
    if (hdQCstatuscode == "") {
        if (Src_Type == "PUR")
        {
            suppid = $("#hdnSupplierID").val();
            $("#ddlDocumentNumber").select2({
                ajax: {
                    url: "/ApplicationLayer/QualityInspection/GetQCSourceDocumentNoList",
                    data: function (params) {
                        return {
                            DocumentNo: "",
                            Src_Type: Src_Type,
                            itemId: item_id,
                            suppid: suppid,
                            page: params.page || 1
                        };
                    },
                    cache: true,
                    processResults: function (data, params) {
                        const pageSize = 2000;
                        const page = params.page || 1;

                        if ($(".select2-search__field").parent().find("ul.select2-results__options").length === 0) {
                            $(".select2-search__field").parent().append(`
                    <ul class="select2-results__options">
                        <li class="select2-results__option">
                            <strong class="select2-results__group">
                                <div class="row">
                                    <div class="col-md-4 col-xs-12 def-cursor">${$("#DocNo").text()}</div>
                                    <div class="col-md-4 col-xs-12 def-cursor">${$("#DocDate").text()}</div>
                                    <div class="col-md-4 col-xs-12 def-cursor">${$("#span_SupplierName").text()}</div>
                                </div>
                            </strong>
                        </li>
                    </ul>`);
                        }

                        data = data.slice((page - 1) * pageSize, page * pageSize);
                        debugger;
                        return {
                            results: $.map(data, function (val) {
                               // const parts = val.Name.split(",");
                                return {
                                    id: val.ID,
                                    text: val.Name,      // Document No
                                    document: val.ID,   // Date
                                    Supp_Name: val.Supp_Name,   // Customer Name
                                };
                            }),
                            pagination: {
                                more: data.length === pageSize
                            }
                        };
                    }
                },
                templateResult: function (data) {
                    if (!data.id) return data.text; // Handles the "placeholder" or empty result

                    const classAttr = $(data.element).attr('class') || '';
                    return $(
                        `<div class="row">
                <div class="col-md-4 col-xs-12${classAttr}">${data.text}</div>
                <div class="col-md-4 col-xs-12${classAttr}">${data.document}</div>
                <div class="col-md-4 col-xs-12${classAttr}">${data.Supp_Name}</div>
            </div>`
                    );
                }
            });

            
          
        }
        else {
            $.ajax({
                url: "/ApplicationLayer/QualityInspection/GetQCSourceDocumentNoList",
                data: {
                    DocumentNo: "",
                    Src_Type: Src_Type,
                    itemId: item_id,
                    suppid: suppid
                },
                success: function (data) {
                    debugger;
                    arr = data;//JSON.parse(data);
                    if (arr.length > 0) {
                        $("#ddlDocumentNumber option").remove();
                        $("#ddlDocumentNumber optgroup").remove();
                        $('#ddlDocumentNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr[i].ID}" value="${arr[i].ID}">${arr[i].Name}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#ddlDocumentNumber').select2({
                            templateResult: function (data) {
                                var DocDate = $(data.element).data('date');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });





                    }
                    else {

                        $("#ddlDocumentNumber option").remove();
                        $("#ddlDocumentNumber optgroup").remove();
                        $('#ddlDocumentNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        $('#Textddl').append(`<option data-date="0" value="0">---Select---</option>`);

                    }
                },
            })
        }

    }
}
function ddlDocumentNumberSelect() {
    debugger;

    var SourceDocumentDate = $('#ddlDocumentNumber').val().trim();
    var date = SourceDocumentDate.split("-");

    var fdate = date[2] + "-" + date[1] + "-" + date[0];
    $("#txtsrcdocdate").val(fdate);

    var DocumentNumber = $('#ddlDocumentNumber option:selected').text();
    $('#hd_doc_no').val(DocumentNumber);
    if (DocumentNumber != "0") {
        document.getElementById("vmsrc_doc_no").innerHTML = null;
        $("#ddlDocumentNumber").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-ddlDocumentNumber-container"]').css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
        $("#ddlDocumentNumber").css("border-color", "red");
        $('[aria-labelledby="select2-ddlDocumentNumber-container"]').css("border-color", "red");
    }

    var Src_Type = $("#qc_type  option:selected").val();
    if (Src_Type == 'PRD') {
        OnchangePrdNo();
    }

}
function OnchangePrdNo() {
    var DocumentNumber = $('#ddlDocumentNumber option:selected').text();
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/QualityInspection/GetBatchNo",
            data: {
                DocumentNumber: DocumentNumber
            },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);

                    $("#BatchNumber").val(arr.Table[0].batch_no);

                }

            },
        });

    } catch (err) {
    }
}

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    // var Sno = clickedrow.find("#SpanRowId").val();
    var ItmCode = clickedrow.find("#hdItemId").val();
    //var ItmCode = "";
    //var ItmName = "";
    //if (Sno == "1") {
    //    ItmCode = clickedrow.find("#SOItemListName").val();
    //    ItmName = clickedrow.find("#SOItemListName option:selected").text()
    //}
    //else {
    //    ItmCode = clickedrow.find("#SOItemListName" + Sno).val();
    //    ItmName = clickedrow.find("#SOItemListName" + Sno + " option:selected").text()
    //}
    ItemInfoBtnClick(ItmCode);
    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/LSODetail/GetSOItemDetail",
    //                data: { ItemID: ItmCode },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                        ErrorPage();
    //                        return false;
    //                    }
    //                    if (data !== null && data !== "") {
    //                        var arr = [];
    //                        arr = JSON.parse(data);
    //                        if (arr.Table.length > 0) {
    //                            $("#Txt_ItmOEMNo").val(arr.Table[0].item_oem_no);
    //                            $("#Txt_ItmSampCode").val(arr.Table[0].item_sam_cd);
    //                            $("#Txt_ItmRefNo").val(arr.Table[0].RefNo);
    //                            $("#Txt_ItmHSNCode").val(arr.Table[0].HSN_code);
    //                            var ImgFlag = "N";
    //                            for (var i = 0; i < arr.Table.length; i++) {
    //                                if (arr.Table[i].item_img_name != "" && arr.Table[i].item_img_name != null) {
    //                                    ImgFlag = "Y";
    //                                }
    //                            }
    //                            if (ImgFlag == "Y") {
    //                                var OL = '<ol class="carousel-indicators">';
    //                                var Div = '<div class="carousel-inner">';
    //                                for (var i = 0; i < arr.Table.length; i++) {
    //                                    var ImgName = arr.Table[i].item_img_name;
    //                                    var origin = window.location.origin + "/Attachment/ItemSetup/" + ImgName;
    //                                    if (i === 0) {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '" class="active"> <img src="' + origin + '" />'
    //                                        Div += '<div class="carousel-item active"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                    else {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '"><img src="' + origin + '" /></li>'
    //                                        Div += '<div class="carousel-item"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                }
    //                                OL += '</ol>'
    //                                Div += '</div>'

    //                                var Ach = '<a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev"><span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="sr-only">Previous</span></a>';
    //                                var Ach1 = '<a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next"><span class="carousel-control-next-icon" aria-hidden="true"></span><span class="sr-only">Next</span></a>';

    //                                $("#myCarousel").html(OL + Div + Ach + Ach1);
    //                            }
    //                            else {
    //                                $("#myCarousel").html("");
    //                            }
    //                        }
    //                    }
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}
function OnClickReceiveQtybtn(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    //var Sno = clickedrow.find("#SpanRowId").text();
    var ItemName = "";
    var Itemuom = "";
    /* if (Sno == "1") {*/
    ItemName = clickedrow.find("#ItemName").val();
    TempItemID = clickedrow.find("#hdItemId").val();
    Itemuom = clickedrow.find("#UOM").val()
    ItemuomID = clickedrow.find("#UOMID").val()
    var RecivedQty = clickedrow.find("#RecivedQty").val()
    //if (parseFloat(CheckNullNumber(RecivedQty)) <= 0) {
    //    clickedrow.find("#ParameterEvlsnPopUp").attr("data-target", "");
    //    return false;
    //} else {
    //    clickedrow.find("#ParameterEvlsnPopUp").attr("data-target", "#RecQuant");
    //}

    //samsize = "1";
    //$("#SaveExitClose").attr("data-dismiss", "");

    //$("#QC_ItemName").val(ItemName);
    //$("#UOM").val(Itemuom);
    var TransType = $("#hdn_TransType").val();
    var Command = $("#Command").val();
    var DocumentStatus = $("#DocumentStatus").val();
    var StatusCode = $("#hdQCstatuscode").val();
    var qc_no = $("#InspectionNumber").val();
    var qc_dt = $("#txtQCDate").val();

    if ((StatusCode == "" || StatusCode == null || StatusCode == "D")) {
        sessionStorage.removeItem("DisableQCparamtable");
        sessionStorage.setItem("DisableQCparamtable", "false");
    }
    else {
        sessionStorage.removeItem("DisableQCparamtable");
        sessionStorage.setItem("DisableQCparamtable", "true");
    }

    OnViewRecivedIbtn();
    // AddItemQcParamDetail(TempItemID);
    var ItemID = TempItemID;
    $.ajax(
        {
            type: "Post",
            url: "/ApplicationLayer/QualityInspection/GetItemQCParamDetail",
            data: {
                ItemID: ItemID,
                qc_no: qc_no,
                qc_dt: qc_dt,
                TransType: TransType,
                Command: Command,
                DocumentStatus: DocumentStatus == "New" ? "" : DocumentStatus
            },
            success: function (data) {
                debugger;
                $("#RecQuantPopUp").html(data);
                //samsize = "1";
                $("#SaveExitClose").attr("data-dismiss", "");
                $("#QC_ItemName").val(ItemName);
                $("#UOM").val(Itemuom);
                $("#ItemCode").val(TempItemID);
                $("#hdUOMId").val(ItemuomID);
                $("#HdnItemSampleSize").val(RecivedQty);
                BindQCParam(TempItemID);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
                //   alert("some error");
            }
        });

}
function BindQCParam(ItmCode) {
    debugger;
    //let NewArr = [];
    var FQCDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
    debugger;
    var NewArr = FQCDetails.filter(v => v.ItmCode === ItmCode);
    NewArr.sort(FQCDetails.SRNumber);
    if (FQCDetails != null) {
        if (NewArr != null) {
            if (NewArr.length > 0) {


                $("#ItmSampSize").val(NewArr[0].ItemSampSize)
                var sampleSize = $("#ItmSampSize").val();
                var disablePage = $("#disablePage").val();
                var ArrSrNo = 0;
                for (var k = 1; k <= sampleSize; k++) {
                    var td1 = "";
                    var i = 0;

                    $("#QCPrmEvalutionTbl > tbody > tr:eq(0) >td").each(function () {
                        var CurrentRow = $(this);
                        var Param_Type = CurrentRow.find("#ParamTypeValue").val();
                        var Param_Id = CurrentRow.find("#ParamId").val();

                        if (Param_Type == "N") {
                            var rejult = "";
                            if (NewArr[ArrSrNo] != null) {
                                if (Param_Id == NewArr[ArrSrNo].ParamID) {
                                    rejult = NewArr[ArrSrNo] == null ? "" : NewArr[ArrSrNo].Result;
                                    ArrSrNo = ArrSrNo + 1;
                                }
                            }



                            td1 += `<td><div class="lpo_form">
    <input class="form-control center" autocomplete="off" ${disablePage} style="border: none;" id="td_${i}" value="${rejult}" onchange="OnchangeParamVal(this)" onkeypress="return OnClickParamVal(this,event);" onkeyup="OnKeyUpParamVal(this, this.value)" value="" />
    <span id="ErrMsg_td_${i}" class="error-message is-visible"></span>
    </div> </td>`;

                        }
                        if (Param_Type == "L") {
                            var checked = "checked";
                            if (NewArr[ArrSrNo] != null) {
                                if (Param_Id == NewArr[ArrSrNo].ParamID) {
                                    if (NewArr[ArrSrNo].ToggleResult == "Y") {
                                        checked = "checked";
                                    }
                                    else {
                                        checked = "";
                                    }
                                    ArrSrNo = ArrSrNo + 1;
                                }
                            }

                            td1 += `<td class="center">
                <label class="switch">
                    <input id="td_${i}" ${disablePage} type="checkbox" ${checked}>
                    <span class="slider round"></span>
                </label>
            </td>`;

                        }
                        if (Param_Type == "O") {
                            var rejult = "";
                            if (NewArr[ArrSrNo] != null) {
                                if (Param_Id == NewArr[ArrSrNo].ParamID && k == NewArr[ArrSrNo].SRNumber) {
                                    rejult = NewArr[ArrSrNo] == null ? "" : NewArr[ArrSrNo] == null ? "" : NewArr[ArrSrNo].Result;
                                    ArrSrNo = ArrSrNo + 1;
                                }
                            }

                            td1 += `<td ><div class="lpo_form">
    <input class="form-control" autocomplete="off" maxlength="50" id="td_${i}" ${disablePage} placeholder="${$("#span_Observative").text()}" onchange="OnchangeObservativeVal(this)" title="${$("#span_Observative").text()}" value="${rejult}" />
    <span id="ErrMsgg_td_${i}" class="error-message is-visible"></span>
    </div> </td>`;
                        }
                        i = i + 1;
                    });
                    // ArrSrNo = ArrSrNo - 1;
                    $("#QCPrmEvalutionTbl tbody").append(`
        <tr>
            <td class="center">${k}</td>
                ${td1}
        </tr>
       `)
                }

                $("#ItmSampSize").attr("disabled", true);
                $("#BtnAddItem").parent().css("display", "none");
                if ($("#Disable").val() == "N") {
                    $("#ReplicateFirstRow").attr("disabled", false);
                }
                else {
                    $("#ReplicateFirstRow").attr("disabled", true);
                }
                $("#ReplicateWithAll").attr("disabled", false);

                var a = 0;
                $("#QCPrmEvalutionTbl > tbody > tr").each(function () {
                    if (a > 0) {
                        a = 0;
                        $(this).find("td").each(function () {
                            if (a > 0) {
                                var CurrentColumn = $(this);
                                OnKeyUpParamVal(CurrentColumn.find("#td_" + a)[0], CurrentColumn.find("#td_" + a).val());
                            }
                            a++;
                        });
                    }
                    a++;
                });


            }
        }

    }

}
function DisableQCParam() {
    $("#QcParamDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        currentRow.find("#Result" + Sno).attr("disabled", true);
        currentRow.find("#ok" + Sno).attr("disabled", true);
        currentRow.find("#remarks" + Sno).attr("disabled", true);

    });
}
function EnableQCParam() {
    sessionStorage.removeItem("DisableQCparamtable");
    sessionStorage.setItem("DisableQCparamtable", "false");
    $("#QcParamDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var ParamType = currentRow.find("#paramtype" + Sno).val();

        if (ParamType == 'L') {
            currentRow.find("#Result" + Sno).attr("disabled", true);
            currentRow.find("#ok" + Sno).attr("disabled", false);
        }
        else {
            currentRow.find("#Result" + Sno).attr("disabled", false);
            currentRow.find("#ok" + Sno).attr("disabled", true);
        }
        //currentRow.find("#Result" + Sno).attr("disabled", false);
        //currentRow.find("#ok" + Sno).attr("disabled", false);
        currentRow.find("#remarks" + Sno).attr("disabled", false);

    });
}
function AddEnableQCParam() {
    debugger;
    sessionStorage.removeItem("ItemQCParamDetails");
    sessionStorage.removeItem("EditQCparamtable");
    sessionStorage.removeItem("DisableQCparamtable");
    sessionStorage.setItem("DisableQCparamtable", "false");
    $("#QcParamDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var ParamType = currentRow.find("#paramtype" + Sno).val();

        if (ParamType == 'L') {
            currentRow.find("#Result" + Sno).attr("disabled", true);
            currentRow.find("#ok" + Sno).attr("disabled", false);
        }
        else {
            currentRow.find("#Result" + Sno).attr("disabled", false);
            currentRow.find("#ok" + Sno).attr("disabled", true);
        }

        //currentRow.find("#Result" + Sno).attr("disabled", false);
        //currentRow.find("#ok" + Sno).attr("disabled", false);
        currentRow.find("#remarks" + Sno).attr("disabled", false);

    });
}
function EditBtnClick() {
    debugger;
    sessionStorage.removeItem("DisableQCparamtable");
    sessionStorage.setItem("DisableQCparamtable", "false");
    sessionStorage.removeItem("EditQCparamtable");
    sessionStorage.setItem("EditQCparamtable", "Edit");

    $("#QcParamDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var ParamType = currentRow.find("#paramtype" + Sno).val();

        if (ParamType == 'L') {
            currentRow.find("#Result" + Sno).attr("disabled", true);
            currentRow.find("#ok" + Sno).attr("disabled", false);
        }
        else {
            currentRow.find("#Result" + Sno).attr("disabled", false);
            currentRow.find("#ok" + Sno).attr("disabled", true);
        }
        currentRow.find("#remarks" + Sno).attr("disabled", false);

    });
    debugger
    var Status = "";
    //Status = $('#hdQCstatuscode').val();
    //if (Status === "A") {
    //    debugger
    //    var ddlDocumentNumber = $("#ddlDocumentNumber").val();
    //    var txtsrcdocdate = $("#txtsrcdocdate").val();
    //    $.ajax({
    //        type: "POST",
    //        url: "/ApplicationLayer/QualityInspection/CheckGRNAgainstQC",
    //        dataType: "json",
    //        data: { DocNo: ddlDocumentNumber, DocDate: txtsrcdocdate },
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                //LSO_ErrorPage();
    //                return false;
    //            }

    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {

    //                    swal("", $("#GRNHasBnPrcsdCanNotBeMdfd").text(), "warning");
    //                    $("#hdnForEdit").attr("name", "");
    //                    return false;
    //                }
    //                else {
    //                    $("#hdnForEdit").attr("name", "Command");
    //                    $('form').submit();
    //                }
    //            }
    //            else {
    //                $("#hdnForEdit").attr("name", "Command");
    //                $('form').submit();
    //            }
    //        },
    //        error: function (Data) {

    //        }
    //    });
    //}
    //else {
    //    $("#hdnForEdit").attr("name", "Command");
    //    $('form').submit();
    //}
}
function RemoveSessionData() {
    sessionStorage.removeItem("DisableQCparamtable");
    sessionStorage.removeItem("ItemQCParamDetails");
    sessionStorage.removeItem("EditQCparamtable");
    sessionStorage.removeItem("ItemLBQCDetails");

}
function OnClickResult() {
    debugger;
    $("#QcParamDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        currentRow.find("#SpanQCResult").css("display", "none");
        currentRow.find("#Result" + Sno).css("border-color", "#ced4da");
        currentRow.find("#SpanQCResult").text($("").text());
    });
}
function OnViewRecivedIbtn() {
    debugger;
    //var ItmCode = $("#ItemCode").val();
    //var ItemUOMID = $("#UOMID").val();
    //var ItemSampSize = $("#ItmSampSize").val(); 
    let NewArr = [];
    var FQCDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
    if (FQCDetails != null) {
        if (FQCDetails.length > 0) {

        }
        else {
            SetSessionData();

        }
    }
    else {
        SetSessionData();

    }

}
//function OnClickSaveAndExitOld() {
//    debugger;
//    var DecDigit = $("#ValDigit").text();
//    //var TotalTaxAmount = $('#TotalTaxAmount').text();
//    //var RowSNo = $("#HiddenRowSNo").val();
//    var ItmCode = $("#ItemCode").val();  
//    var ItemUOMID = $("#UOMID").val(); 
//    var ItemSampSize = $("#ItmSampSize").val(); 
//    var UserID = $("#UserID").text();
//    var error = "N";
//    $("#QcParamDetailsTbl >tbody >tr").each(function () {
//        var currentRow = $(this);
//        var ToggleResult = "";
//        debugger;
//        var Sno = currentRow.find("#SNohiddenfiled").val();      
//        var ParamType = currentRow.find("#Quantitative" + Sno).val();      
//        var Result = currentRow.find("#Result" + Sno).val();
//        if (ParamType == "Quantitative" && Result == "" || Result == "NaN" || Result == "0" || Result == parseFloat("0").toFixed(DecDigit)) {
//           // $('#SpanQCResult').text($("#valueReq").text());
//            currentRow.find("#SpanQCResult").text($("#valueReq").text());
//            currentRow.find("#SpanQCResult").css("display", "block");
//            currentRow.find("#Result" + Sno).css("border-color", "red");
//            error = "Y";
//            return false;
//        }
//        else {
//            currentRow.find("#SpanQCResult").css("display", "none");
//            currentRow.find("#Result" + Sno).css("border-color", "#ced4da");
//        }       
//    });
//    if (error == "Y") {
//        return false
//    }
//    else {
//        debugger;
//        $("#SaveExitClose").attr("data-dismiss", "modal");
//    }
//    let NewArr = [];
//    var FQCDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
//    if (FQCDetails != null) {
//        if (FQCDetails.length > 0) {
//            for (i = 0; i < FQCDetails.length; i++) {

//                var ItemID = FQCDetails[i].ItmCode;

//                if (ItemID == ItmCode ) {
//                }
//                else {
//                    NewArr.push(FQCDetails[i]);
//                }
//            }
//            $("#QcParamDetailsTbl >tbody >tr").each(function () {
//                var currentRow = $(this);
//                var ToggleResult = "";
//                var Action = "";

//                debugger;

//                var Sno = currentRow.find("#SNohiddenfiled").val();
//                var ParamName = currentRow.find("#Parameter" + Sno).val();
//                var ParamID = currentRow.find("#param_id" + Sno).val();
//                var ParamType = currentRow.find("#Quantitative" + Sno).val();
//                var ParamTypeCode = currentRow.find("#paramtype" + Sno).val();
//                var ParamUOM = currentRow.find("#ParamUOM" + Sno).val();
//                var UpperRange = currentRow.find("#UpperRange" + Sno).val();
//                var LowerRange = currentRow.find("#LowerRange" + Sno).val();
//                var Result = currentRow.find("#Result" + Sno).val();
//               // var Action = currentRow.find("#hdAction" + Sno).val();
//                if (currentRow.find("#ok" + Sno).is(":checked")) {
//                    ToggleResult = 'Y';
//                }
//                else {
//                    ToggleResult = 'N';

//                }
//                if (ParamTypeCode == "L" && ToggleResult == "Y") {
//                    Action = "Pass";
//                }
//                if (ParamTypeCode == "L" && ToggleResult == "N") {
//                    Action = "Fail";
//                }
//                if (ParamTypeCode == "N") {
//                    //Action = currentRow.find("#hdAction" + Sno).val();
//                    Action = "Pass";
//                }

//                var Remarks = currentRow.find("#remarks" + Sno).val();

//                NewArr.push({ ItmCode: ItmCode, ItemUOMID: ItemUOMID, ItemSampSize: ItemSampSize, ParamName: ParamName, ParamID: ParamID, ParamTypeCode: ParamTypeCode, ParamType: ParamType, ParamUOM: ParamUOM, UpperRange: UpperRange, LowerRange: LowerRange, Result: Result, ToggleResult: ToggleResult, Action: Action, Remarks: Remarks })
//            });

//            sessionStorage.removeItem("ItemQCParamDetails"); 
//            sessionStorage.setItem("ItemQCParamDetails", JSON.stringify(NewArr));
//        }
//        else {
//            var ItemQCList = [];
//            $("#QcParamDetailsTbl >tbody >tr").each(function () {
//                var currentRow = $(this);
//                var ToggleResult = "";
//                var Action = "";
//                debugger;
//                var Sno = currentRow.find("#SNohiddenfiled").val();

//                var ParamName = currentRow.find("#Parameter" + Sno).val();
//                var ParamID = currentRow.find("#param_id" + Sno).val();
//                var ParamType = currentRow.find("#Quantitative" + Sno).val();
//                var ParamTypeCode = currentRow.find("#paramtype" + Sno).val();
//                var ParamUOM = currentRow.find("#ParamUOM" + Sno).val();
//                var UpperRange = currentRow.find("#UpperRange" + Sno).val();
//                var LowerRange = currentRow.find("#LowerRange" + Sno).val();
//                var Result = currentRow.find("#Result" + Sno).val();
//                //var Action = currentRow.find("#hdAction" + Sno).val();
//                if (currentRow.find("#ok" + Sno).is(":checked")) {
//                    ToggleResult = 'Y';
//                }
//                else {
//                    ToggleResult = 'N';

//                }
//                if (ParamTypeCode == "L" && ToggleResult == "Y") {
//                    Action = "Pass";
//                }
//                if (ParamTypeCode == "L" && ToggleResult == "N") {
//                    Action = "Fail";
//                }
//                if (ParamTypeCode == "N") {
//                    //Action = currentRow.find("#hdAction" + Sno).val();
//                    Action = "Pass";
//                }

//                var Remarks = currentRow.find("#remarks" + Sno).val();

//                ItemQCList.push({ ItmCode: ItmCode, ItemUOMID: ItemUOMID, ItemSampSize: ItemSampSize, ParamName: ParamName, ParamID: ParamID, ParamTypeCode: ParamTypeCode, ParamType: ParamType, ParamUOM: ParamUOM, UpperRange: UpperRange, LowerRange: LowerRange, Result: Result, ToggleResult: ToggleResult, Action: Action, Remarks: Remarks })
//            });

//            sessionStorage.removeItem("ItemQCParamDetails");
//            sessionStorage.setItem("ItemQCParamDetails", JSON.stringify(ItemQCList));
//        }
//    }
//    else {
//        var ItemQCList = [];
//        $("#QcParamDetailsTbl >tbody >tr").each(function () {
//            var currentRow = $(this);
//            var ToggleResult = "";
//            var Action = "";
//            debugger;
//            var Sno = currentRow.find("#SNohiddenfiled").val();
//            var ParamName = currentRow.find("#Parameter" + Sno).val();
//            var ParamID = currentRow.find("#param_id" + Sno).val();
//            var ParamType = currentRow.find("#Quantitative" + Sno).val();
//            var ParamTypeCode = currentRow.find("#paramtype" + Sno).val();
//            var ParamUOM = currentRow.find("#ParamUOM" + Sno).val();
//            var UpperRange = currentRow.find("#UpperRange" + Sno).val();
//            var LowerRange = currentRow.find("#LowerRange" + Sno).val();
//            var Result = currentRow.find("#Result" + Sno).val();
//            //var Action = currentRow.find("#hdAction" + Sno).val();
//            if (currentRow.find("#ok" + Sno).is(":checked")) {
//                ToggleResult = 'Y';
//            }
//            else {
//                ToggleResult = 'N';

//            }
//            if (ParamTypeCode == "L" && ToggleResult == "Y") {
//                Action = "Pass";
//            }
//            if (ParamTypeCode == "L" && ToggleResult == "N") {
//                Action = "Fail";
//            }
//            if (ParamTypeCode == "N") {
//                //Action = currentRow.find("#hdAction" + Sno).val();
//                Action = "Pass";
//            }

//            var Remarks = currentRow.find("#remarks" + Sno).val();

//            ItemQCList.push({ ItmCode: ItmCode, ItemUOMID: ItemUOMID, ItemSampSize: ItemSampSize, ParamName: ParamName, ParamID: ParamID, ParamTypeCode: ParamTypeCode, ParamType: ParamType, ParamUOM: ParamUOM, UpperRange: UpperRange, LowerRange: LowerRange, Result: Result, ToggleResult: ToggleResult, Action: Action, Remarks: Remarks })
//        });

//        sessionStorage.removeItem("ItemQCParamDetails");
//        sessionStorage.setItem("ItemQCParamDetails", JSON.stringify(ItemQCList));
//    }


//        var FQCDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
//        if (FQCDetails != null) {
//            if (FQCDetails.length > 0) {
//                for (i = 0; i < FQCDetails.length; i++) {
//                    var FQCItemID = FQCDetails[i].ItmCode;
//                    $("#QcItmDetailsTbl >tbody >tr").each(function () {
//                        var currentRow = $(this);
//                        //var Sno = currentRow.find("#SNohiddenfiled").val();
//                        debugger;
//                        var QCItemID = currentRow.find("#hdItemId").val(); 
//                    if (QCItemID == FQCItemID) {
//                        debugger;
//                        currentRow.find("#AcceptQty").attr("readonly", false);
//                        currentRow.find("#RejectQty").attr("readonly", false);
//                        currentRow.find("#ReworkableQty").attr("readonly", false);
//                    }
//                    //else {
//                    //    debugger;
//                    //    currentRow.find("#AcceptQty").attr("readonly", true);
//                    //    currentRow.find("#RejectQty").attr("readonly", true);
//                    //    currentRow.find("#ReworkableQty").attr("readonly", true);
//                    //}
//                    });
//                }

//            }

//        }


//}
function AddAttribute() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var docno = $('#ddlDocumentNumber option:selected').text();
    $("#hd_doc_no").val(docno);
    var Src_Type = $("#qc_type  option:selected").val();
    $("#hdQCtype").val(Src_Type);
    var span_SubItemDetail = $("#span_SubItemDetail").text();
    if ($('#ddlDocumentNumber').val() != "0" && $('#ddlDocumentNumber').val() != "") {
        var text = $('#ddlDocumentNumber').val();
        $(".plus_icon1").css('display', 'none');
        //debugger;
        //$("#qc_type").attr("readonly", true);
        $("#qc_type").prop("disabled", true);
        $("#ddlDocumentNumber").prop("disabled", true);
        var hdSelectedSourceDocument = null;
        var SourceDocumentDate = $('#ddlDocumentNumber').val().trim();
        var SourDocumentNo = $('#ddlDocumentNumber option:selected').text();
        hdSelectedSourceDocument = SourDocumentNo;
        $("#hdSelectedSourceDocument").val(hdSelectedSourceDocument);
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/QualityInspection/GetItemDetailBySourceDocumentNo",
                data: {
                    SourDocumentNo: hdSelectedSourceDocument,
                    Src_Type: Src_Type
                },
                success: function (data) {
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var img = new Image();
                                var image = new Image();
                                img.src = "~/Content/Images/qc1.png"
                                image = "qc1.png"
                                var origin = window.location.origin + "/Content/Images/qc1.png";
                                var sub_item = arr.Table[i].sub_item;
                                var subitemdisable = "";
                                if (sub_item == "Y") {
                                    subitemdisable = "";
                                } else {
                                    subitemdisable = "disabled";
                                }
                                var recd_qty;
                                if (arr.Table[i].recd_qty == "0") {
                                    recd_qty = "";
                                }
                                else {
                                    recd_qty = parseFloat(arr.Table[i].recd_qty).toFixed(QtyDecDigit);
                                }
                                var Cnf_Prod_qty;
                                if (arr.Table[i].prod_qty == "0") {
                                    Cnf_Prod_qty = "";
                                }
                                else {
                                    Cnf_Prod_qty = parseFloat(arr.Table[i].prod_qty).toFixed(QtyDecDigit);
                                }
                                var accept_qty;
                                if (arr.Table[i].accept_qty == "0") {
                                    accept_qty = "";
                                }
                                else {
                                    accept_qty = parseFloat(arr.Table[i].accept_qty).toFixed(QtyDecDigit);
                                }
                                var reject_qty;
                                if (arr.Table[i].reject_qty == "0") {
                                    reject_qty = "";
                                }
                                else {
                                    reject_qty = parseFloat(arr.Table[i].reject_qty).toFixed(QtyDecDigit);
                                }
                                var rework_qty;
                                if (arr.Table[i].rework_qty == "0") {
                                    rework_qty = "";
                                }
                                else {
                                    rework_qty = parseFloat(arr.Table[i].rework_qty).toFixed(QtyDecDigit);
                                }
                                let ForPDR = '';
                                let QcQtyDisable = '';
                                if (Src_Type == "PRD") {
                                    ForPDR = `<td>
                                                 <div class=" col-sm-10 lpo_form" style="padding:0px;">
                                                         <input id="ProdQty" value="${Cnf_Prod_qty}" readonly onchange="OnChangeShortQty(event)" onkeypress="return OnKeyPressShortQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="ProdQty" placeholder="0000.00" >
                                                 </div>
                                                 <div class=" col-sm-2 i_Icon" id="div_SubItemPRDQty" style="padding:0px;">
                                                     <button type="button" id="SubItemPRDProdQty" ${subitemdisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('PRDProdQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title='${span_SubItemDetail}'> </button>
                                                 </div>
                                             </td>
                                             <td>
                                                <div class=" col-sm-10 lpo_form" style="padding:0px;">
                                                        <input id="PendQty" value="${recd_qty}"  readonly onchange="OnChangeSampleQty(event)" onkeypress="return OnKeyPressSampleQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="PendQty" placeholder="0000.00" >
                                                </div>
                                                <div class=" col-sm-2 i_Icon" id="div_SubItemSampleQty" style="padding:0px;">
                                                    <button type="button" id="SubItemPRDPendQty" ${subitemdisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('PRDPendQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title='${span_SubItemDetail}'> </button>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="col-sm-8 lpo_form" style="padding:0px;" id="multiWrapper">
                                                     <input id="RecivedQty" value="${recd_qty}" class="form-control num_right" onchange="OnChangeQCQty(event)" onkeypress="return Cmn_IntValueonly(this, event)" onpaste="return NOanycopypastedata(event)" autocomplete="off" type="text" name="RecivedQty" placeholder="0000.00">
<input hidden id="sub_item" value="${arr.Table[i].sub_item}" />
                                                     <span id="PRDRecivedQty_specError" class="error-message is-visible"></span>
                                                </div>
                                                                            <div class=" col-sm-2 i_Icon" id="div_SubItemQCQty">
                                                                                <button type="button" id="SubItemQCQty" ${subitemdisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('PRDProduceQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                                            </div>
                                                                                            <div class="col-sm-2 received_quantity" style="padding:0px;">
                                                                                                <a onclick="OnClickReceiveQtybtn(event);" id="ParameterEvlsnPopUp" data-toggle="modal" data-target="#RecQuant" data-backdrop="static" data-keyboard="false" class="calculator">
                                                                                                    <img src="${origin}" height="22" width="22" id="ParamEvluationImg" aria-hidden="true" title="${$(" #Span_ParameterEvaluatione_Title").text()}" />
                                                                                                </a>
                                                                                            </div>
                                                                                        </td>`
                                }
                                else {
                                    QcQtyDisable = ` <td>
                                                                                            <div class="col-sm-8" style="padding:0px;" id="multiWrapper">
                                                                                                <input id="RecivedQty" value="${recd_qty}" class="form-control num_right" onpaste="return NOanycopypastedata(event)" autocomplete="" type="text" name="RecivedQty" placeholder="0000.00" disabled>
                                                                            </div>
                                                                            <input hidden id="sub_item" value="${arr.Table[i].sub_item}" />
                                                                            <div class=" col-sm-2 i_Icon" id="div_SubItemQCQty">
                                                                                <button type="button" id="SubItemQCQty" ${subitemdisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('ProduceQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                                            </div>
                                                                                            <div class="col-sm-2 received_quantity" style="padding:0px;">
                                                                                                <a onclick="OnClickReceiveQtybtn(event);" id="ParameterEvlsnPopUp" data-toggle="modal" data-target="#RecQuant" data-backdrop="static" data-keyboard="false" class="calculator">
                                                                                                    <img src="${origin}" height="22" width="22" id="ParamEvluationImg" aria-hidden="true" title="${$(" #Span_ParameterEvaluatione_Title").text()}" />
                                                                                                </a>
                                                                                            </div>
                                                                                        </td>`;
                                }
                                let ShortAndSampleRow = '';
                                //if (Src_Type == "PUR" || Src_Type == "RQC") {//commanted by shubham maurya on 21-02-2025
                                if (Src_Type == "PUR") {
                                    ShortAndSampleRow = `<td>
                                                 <div class=" col-sm-10 lpo_form" style="padding:0px;">
                                                         <input id="ShortQty" value="" readonly onchange="OnChangeShortQty(event)" onkeypress="return OnKeyPressShortQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="ShortQty" placeholder="0000.00" >
                                                 </div>
                                                 <div class=" col-sm-2 i_Icon" id="div_SubItemShortQty" style="padding:0px;">
                                                     <button type="button" id="SubItemShortQty" ${subitemdisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCShortQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title='${span_SubItemDetail}'> </button>
                                                 </div>
                                             </td>
                                             <td>
                                                <div class=" col-sm-10 lpo_form" style="padding:0px;">
                                                        <input id="SampleQty" value=""  readonly onchange="OnChangeSampleQty(event)" onkeypress="return OnKeyPressSampleQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="SampleQty" placeholder="0000.00" >
                                                </div>
                                                <div class=" col-sm-2 i_Icon" id="div_SubItemSampleQty" style="padding:0px;">
                                                    <button type="button" id="SubItemSampleQty" ${subitemdisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCSampleQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title='${span_SubItemDetail}'> </button>
                                                </div>
                                            </td>`;
                                }


                                $('#QcItmDetailsTbl tbody').append(`<tr id="@rowIdx">
                                                                                        <td style="display:none;"></td>
                                                                                        <td class="sr_padding"><span id="SpanRowId">${i + 1}</span></td>
                                                                                        <td class="ItmNameBreak itmStick tditemfrz">
                                                                                            <div class=" col-sm-11" style="padding:0px;">
                                                                                                <input id="ItemName" class="form-control" autocomplete="off" type="text" name="ItemName" placeholder="${$("#ItemName").text()}" disabled value='${arr.Table[i].item_name}'>

                                                                                                <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                                                                            </div>
                                                                                            <div class="col-sm-1 i_Icon received_quantity">

                                                                                                <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td>
                                                                                            <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled value="${arr.Table[i].uom_name}">
                                                                                            <input type="hidden" id="UOMID" value="${arr.Table[i].uom_id}" style="display: none;" />
                                                                                        </td>
                                                                                        <td style="display:none;">
                                                                                          
                                                                                        </td>
                                                                                        `+ ForPDR + `
                                                                                       `+ QcQtyDisable +`
                                                                                        <td style="display:none;">
                                                                                           

                                                                                        </td>


                                                                                        <td>
                                                                                            <div class="col-sm-8 lpo_form no-padding">
                                                                                                <input id="AcceptQty"  value="${accept_qty}" class="form-control num_right" onkeypress="return OnKeyPressAcceptQty(this,event);" onchange="OnChangeAcceptQty(this,event)" onkeypress="return OnKeyPressShortQty(this,event);" onpaste="return NOanycopypastedata(event)" autocomplete="off" type="text" name="AcceptQty" placeholder="0000.00" readonly>
                                                                                                <span id="ord_qty_specError" class="error-message is-visible"></span>
                                                                                            </div>
                                                                            <div class=" col-sm-2 i_Icon no-padding" id="div_SubItemAccQty">
                                                                                <button type="button" id="SubItemAccQty" ${subitemdisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCAccQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                                            </div>
                                                                       <div class="col-sm-2 i_Icon">
                                                                        <button type="button" class="calculator" id="BtnReasonForAccecpt" onclick="return onClickReasonRemarks(event,'Accept')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_ReasonForAccept_Remarks").text()}">  </button>
                                                                             <input  type="hidden" id="ReasonForAccecpt" />
                                                                               </div>
                                                                                        </td>
                                                                                        <td>
                                                                                          <div class="col-sm-8 lpo_form no-padding">
                                                                                          <input id="RejectQty" value="${reject_qty}" class="form-control num_right" autocomplete="off" type="text" name="RejectQty" onkeypress="return OnKeyPressRejectQty(this,event);" onchange="OnChangeRejectQty(this,event)" onkeypress="return OnKeyPressShortQty(this,event);" onpaste="return NOanycopypastedata(event)" placeholder="0000.00" readonly>
                                                                                          </div>
                                                                                          <div class="col-sm-2 i_Icon no-padding" id="div_SubItemRejQty">
                                                                                              <button type="button" id="SubItemRejQty" ${subitemdisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCRejQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                                                          </div>
                                                                                          <div class="col-sm-2 i_Icon">
                                                                                              <button type="button" class="calculator" id="BtnReasonForReject" onclick="return onClickReasonRemarks(event,'reject')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_ReasonForRejection").text()}">  </button>
                                                                                              <input hidden id="ReasonForReject"  />

                                                                                                <input type="hidden" id="ReasonForReject_ID" />
                                                                                                 <input type="hidden" id="ReasonForReject_Name" />
                                                                                          </div>
                                                                                        </td>
                                                                                        <td>
                                                                                            <div class="col-sm-8 lpo_form no-padding">
                                                                                            <input id="ReworkableQty" value="${rework_qty}" class="form-control num_right" autocomplete="off" type="text" name="ReworkableQty" placeholder="0000.00" onkeypress="return OnKeyPressReworkQty(this,event);" onkeypress="return OnKeyPressShortQty(this,event);" onpaste="return NOanycopypastedata(event)" onchange="OnChangeReworkQty(this,event)" readonly>
                                                                                            </div>
                                                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemRewQty">
                                                                                                <button type="button" id="SubItemRewQty" ${subitemdisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCRewQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                                                            </div>
                                                                                            <div class="col-sm-2 i_Icon">
                                                                                                <button type="button" class="calculator" id="BtnReasonForRework" onclick="return onClickReasonRemarks(event,'rework')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_Reworkremarks").text()}">  </button>
                                                                                                <input hidden id="ReasonForRework" />
                                                                                                
                                                                                            
                                                                                            </div>
                                                                                       </td>
                                                                                        `+ ShortAndSampleRow + `
                                                                                        <td>

                                                                                            <textarea id="txtItemRemarks" value="${arr.Table[i].it_remarks}" class="form-control" name="txtItemRemarks" maxlength="200" onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}" onblur="this.placeholder='${$("#span_remarks").text()}'" data-parsley-validation-threshold="10"></textarea>

                                                                                        </td>
                                                                                    </tr>`);
                            }

                        }
                        $("#ddl_ProductName_PRD").attr("disabled", true);
                        //commented By shubham Maurya 17-10-2023 ----------------Start--------------
                        //if (arr.Table1.length > 1) {
                        //-----------------------------------------------------End------------------
                        //        for (var i = 0; i < arr.Table1.length; i++) {
                        //            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                        //    <td><input type="text" id="ItemId" value='${arr.Table1[i].item_id}'></td>
                        //    <td><input type="text" id="subItemId" value='${arr.Table1[i].sub_item_id}'></td>
                        //    <td><input type="text" id="subItemQty" value='${arr.Table1[i].prod_qty}'></td>
                        //    <td><input type="text" id="subItemAccQty" value='${arr.Table1[i].accept_qty == null ? "0" : arr.Table1[i].accept_qty}'></td>
                        //    <td><input type="text" id="subItemRejQty" value='${arr.Table1[i].reject_qty == null ? "0" : arr.Table1[i].reject_qty}'></td>
                        //    <td><input type="text" id="subItemRewQty" value='${arr.Table1[i].rework_qty == null ? "0" : arr.Table1[i].rework_qty}'></td>
                        //</tr>`);
                        //        }
                        //}

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

        var Str = $("#valueReq").text();
        document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
    }
    inSupplierName("Disabled");
}
function OnChangeQCQty(e) {
    debugger;
    var ErrorFlag = "N";
    var clickedrow = $(e.target).closest("tr");
    var ReceivedQty = parseFloat(CheckNullNumber(clickedrow.find("#RecivedQty").val())).toFixed(QtyDecDigit);
    if (ReceivedQty == 0) {
        clickedrow.find("#RecivedQty").css("border-color", "red");
        clickedrow.find("#PRDRecivedQty_specError").text($("#valueReq").text());
        clickedrow.find("#PRDRecivedQty_specError").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        clickedrow.find("#RecivedQty").css("border-color", "#ced4da");
        clickedrow.find("#PRDRecivedQty_specError").text("");
        clickedrow.find("#PRDRecivedQty_specError").css("display", "none");
    }
    var PendQty = parseFloat(CheckNullNumber(clickedrow.find("#PendQty").val())).toFixed(QtyDecDigit);
    if (parseFloat(PendQty) < parseFloat(ReceivedQty)) {
        clickedrow.find("#RecivedQty").css("border-color", "red");
        clickedrow.find("#PRDRecivedQty_specError").text($("#ExceedingQty").text());
        clickedrow.find("#PRDRecivedQty_specError").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        clickedrow.find("#RecivedQty").css("border-color", "#ced4da");
        clickedrow.find("#PRDRecivedQty_specError").text("");
        clickedrow.find("#PRDRecivedQty_specError").css("display", "none");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
}
function AddItemQcParamDetail(ItmID) {
    debugger;
    //var clickedrow = $(e.target).closest("tr");
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var ItemID = TempItemID;
    var UomID = ItemuomID;
    var samsize = 1;
    //if ($('#hdQCItemParamDetailList').val() == "") {
    if ($('#hdItemId').val() != "") {
        //debugger;       
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/QualityInspection/GetItemQCParamDetail",
                data: {
                    ItemID: ItemID
                },
                success: function (data) {
                    debugger;
                    $("#RecQuantPopUp").html(data);

                    //                        if (data !== null && data !== "") {
                    //                            var arr = [];
                    //                            arr = JSON.parse(data);                          
                    //                            $('#QcParamDetailsTbl tbody tr').remove();
                    //                            if (arr.Table.length > 0) {
                    //                                var rowIdx = 0;
                    //                                for (var i = 0; i < arr.Table.length; i++) {
                    //                                    ++rowIdx;
                    //                                    var param_result = "";
                    //                                    var param_ok = "";
                    //                                    var action = "";
                    //                                    if (arr.Table[i].param_type == 'L') {
                    //                                        param_result = `<div class="lpo_form"><input id="Result${rowIdx}" class="form-control num_right" onchange="OnchangeParamResult(event);" onkeypress = "return OnKeyPressParamResult(this,event);" onclick="OnClickResult();" type="text" name="Result${rowIdx}" disabled>  <span id="SpanQCResult" class="error-message is-visible"></span>
                    //                        </div>`;
                    //                                        action = `<i class="fa fa-check text-success" id="ResultIcon${rowIdx}" aria-hidden="true"></i>`
                    //                                    }
                    //                                    else {
                    //                                        param_result = `<div class="lpo_form"><input id="Result${rowIdx}" class="form-control num_right" onchange="OnchangeParamResult(event);" onclick="OnClickResult();" onkeypress = "return OnKeyPressParamResult(this,event);" type="text" name="Result${rowIdx}">  <span id="SpanQCResult" class="error-message is-visible"></span>
                    //                        </div>`;
                    //                                        action = `<i class="" id="ResultIcon${rowIdx}" aria-hidden="true"></i>`;
                    //                                    }

                    //                                    if (arr.Table[i].param_type == 'N') {
                    //                                        param_ok = `<div class="custom-control custom-switch sample_issue" hidden><input type="checkbox"  onclick="OnclickParamResultToggle(event);" class="custom-control-input margin-switch" id="ok${rowIdx}" checked disabled><label class="custom-control-label col-md-8 col-sm-12" for="ok${rowIdx}"></label></div>`;
                    //                                    }
                    //                                    else {
                    //                                        param_ok = `<div class="custom-control custom-switch sample_issue" ><input type="checkbox" onclick="OnclickParamResultToggle(event);" class="custom-control-input margin-switch" id="ok${rowIdx}" checked><label class="custom-control-label col-md-8 col-sm-12" for="ok${rowIdx}"></label></div>`;
                    //                                    }                           

                    //                                    //debugger;
                    //                                    $('#QcParamDetailsTbl tbody').append(` <tr id="${rowIdx}">
                    //                    <td>

                    //                        <input id="Parameter${rowIdx}" value="${arr.Table[i].param_name}"  class="form-control" autocomplete="off" type="text" name="Parameter${rowIdx}"  placeholder="Parameter"  onblur="this.placeholder='Parameter'" disabled>
                    //                     <input type="hidden" id="SNohiddenfiled" value="${rowIdx}" />
                    //                     <input type="hidden" id="param_id${rowIdx}" value="${arr.Table[i].param_Id}" />
                    //                     <input type="hidden" id="Item_id${rowIdx}" value="${arr.Table[i].item_id}" />
                    //                     <input type="hidden" id="uom_id${rowIdx}" value="${UomID}" />
                    //                     <input type="hidden" id="sam_id${rowIdx}" value="${samsize}" />
                    //</td>
                    //                    <td><input id="Quantitative${rowIdx}" value="${arr.Table[i].paramtype}"  class="form-control" autocomplete="off" type="text" name="Quantitative${rowIdx}"   placeholder="Quantitative"  onblur="this.placeholder='Quantitative'" disabled>
                    //                    <input type="hidden" id="paramtype${rowIdx}" value="${arr.Table[i].param_type}"  /></td>
                    //            <td><input id="ParamUOM${rowIdx}" class="form-control" type="text" name="ParamUOM" value="${arr.Table[i].uom_alias}" placeholder="${$("#ItemUOM").text()}" disabled ></td>
                    //                        <td>
                    //                        <input id="UpperRange${rowIdx}" value="${parseFloat(arr.Table[i].upper_val).toFixed(QtyDecDigit)}" class="form-control col-md-12 col-sm-12 num_right" autocomplete="off" type="text" name="UpperRange${rowIdx}"  placeholder="1"  onblur="this.placeholder='1'" disabled>
                    //                    </td>
                    //                    <td><input id="LowerRange${rowIdx}" value="${parseFloat(arr.Table[i].lower_val).toFixed(QtyDecDigit)}" class="form-control col-md-12 col-sm-12 num_right" autocomplete="off" type="text" name="LowerRange${rowIdx}"  placeholder="5"  onblur="this.placeholder='5'" disabled></td>
                    //                        <td>

                    //                        `
                    //                                        + param_result +
                    //                                        `

                    //                        </td>
                    //                    <td>

                    //                         `
                    //                                        + param_ok +
                    //                                        `
                    //                    </td>
                    //                    <td class="center">  
                    //                         `
                    //                                        + action +
                    //                                        `
                    // <input type="hidden" id="hdAction${rowIdx}" value="" />
                    //                    </td>
                    // <td>
                    //                                <textarea id="remarks${rowIdx}" value="${arr.Table[i].param_remarks}" class="form-control" name="remarks${rowIdx}"  placeholder="Remarks" onblur="this.placeholder='Remarks'" data-parsley-validation-threshold="10" ></textarea>

                    //                            </td>

                    //                </tr>`);
                    //                                }
                    //                                debugger;
                    //                                var ValDecDigit = $("#ValDigit").text();
                    //                                var FQCDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
                    //                                if (FQCDetails != null) {
                    //                                    if (FQCDetails.length > 0) {
                    //                                        //$("#QcParamDetailsTbl >tbody >tr").remove();
                    //                                        for (j = 0; j < FQCDetails.length; j++) {
                    //                                            var ItemID = FQCDetails[j].ItmCode;
                    //                                            var ParamName = FQCDetails[j].ParamName;
                    //                                            var ParamType = FQCDetails[j].ParamTypeCode;
                    //                                            var Result;
                    //                                            if (FQCDetails[j].Result == null || FQCDetails[j].Result == "") {
                    //                                                Result = "";
                    //                                            }
                    //                                            else {
                    //                                                Result = parseFloat(FQCDetails[j].Result).toFixed(ValDecDigit);
                    //                                            }
                    //                                            var ToggleResult = FQCDetails[j].ToggleResult;
                    //                                            var Remarks = FQCDetails[j].Remarks;
                    //                                            var Action = FQCDetails[j].Action;
                    //                                            var UpperRange = parseFloat(FQCDetails[j].UpperRange).toFixed(ValDecDigit);
                    //                                            var LowerRange = parseFloat(FQCDetails[j].LowerRange).toFixed(ValDecDigit);
                    //                                            var ParamUOM = FQCDetails[j].ParamUOM;


                    //                                            $("#QcParamDetailsTbl >tbody >tr").each(function () {
                    //                                                var currentRow = $(this);
                    //                                                debugger;
                    //                                                var Sno = currentRow.find("#SNohiddenfiled").val();
                    //                                                var param_name = currentRow.find("#Parameter" + Sno).val();
                    //                                                var paramtype = currentRow.find("#paramtype" + Sno).val();

                    //                                                if (ItemID == ItmID && ParamName == param_name && ParamType == paramtype) {
                    //                                                    debugger;
                    //                                                    $("#Result" + Sno).val(Result);
                    //                                                    $("#remarks" + Sno).val(Remarks);
                    //                                                    $("#ParamUOM" + Sno).val(ParamUOM);
                    //                                                    if (ToggleResult == 'Y') {
                    //                                                        $("#ok" + Sno).prop("checked", true)
                    //                                                    }
                    //                                                    else {
                    //                                                        $("#ok" + Sno).prop("checked", false)
                    //                                                    }

                    //                                                    if (ParamType == "L" && ToggleResult == "Y") {
                    //                                                        debugger;
                    //                                                        $("#ResultIcon" + Sno).removeClass('fa fa-times-circle text-danger').addClass('fa fa-check text-success');
                    //                                                    }
                    //                                                    if (ParamType == "L" && ToggleResult == "N") {
                    //                                                        debugger;
                    //                                                        $("#ResultIcon" + Sno).removeClass('fa fa-check text-success').addClass('fa fa-times-circle text-danger');
                    //                                                    }
                    //                                                    if (ParamType == "N" && parseFloat(Result) <= parseFloat(UpperRange) && parseFloat(Result) >= parseFloat(LowerRange)) {
                    //                                                        debugger;
                    //                                                        $("#ResultIcon" + Sno).removeClass('fa fa-times-circle text-danger').addClass('fa fa-check text-success');
                    //                                                    }
                    //                                                    if (ParamType == "N" && (parseFloat(Result) > parseFloat(UpperRange) || parseFloat(Result) < parseFloat(LowerRange))) {
                    //                                                        debugger;
                    //                                                        $("#ResultIcon" + Sno).removeClass('fa fa-check text-success').addClass('fa fa-times-circle text-danger');
                    //                                                    }

                    //                                                }
                    //                                            });
                    //                                        }
                    //                                    }
                    //                                }
                    //                                debugger;
                    //                                var QCDisable = sessionStorage.getItem("DisableQCparamtable");
                    //                                var EditQCDisable = sessionStorage.getItem("EditQCparamtable");
                    //                                var Status = $("#hdQCstatuscode").val();

                    //                                if (QCDisable != null) {
                    //                                    if (QCDisable == "true") {
                    //                                        DisableQCParam();
                    //                                    }
                    //                                    else {
                    //                                        EnableQCParam();
                    //                                    }
                    //                                }
                    //                                else {
                    //                                    EnableQCParam();
                    //                                }
                    //                                if (EditQCDisable != 'Edit' && Status == "D") {
                    //                                    DisableQCParam();
                    //                                }
                    //                                if (EditQCDisable == 'Edit' && Status == "D") {
                    //                                    EnableQCParam();
                    //                                }

                    //                                //sessionStorage.removeItem("EditQCparamtable");
                    //                            }
                    //                        }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

    }
    // }
}
function OnchangeParamResult(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var rowIdx = clickedrow.find("#SNohiddenfiled").val();
    var ValDecDigit = $("#ValDigit").text();

    var UpperRange = parseFloat($('#UpperRange' + rowIdx).val()).toFixed(ValDecDigit);
    var LowerRange = parseFloat($('#LowerRange' + rowIdx).val()).toFixed(ValDecDigit);
    var TestResult = parseFloat($('#Result' + rowIdx).val()).toFixed(ValDecDigit);
    if (TestResult == "NaN") {
        TestResult = 0;
    }
    if (AvoidDot(TestResult) == false) {
        TestResult = 0;
    }
    var FinalTestResult = parseFloat(TestResult).toFixed(parseFloat(ValDecDigit));
    var test = parseFloat(parseFloat(FinalTestResult)).toFixed(parseFloat(ValDecDigit));
    clickedrow.find('#Result' + rowIdx).val(test);

    if (parseFloat(FinalTestResult) <= parseFloat(UpperRange) && parseFloat(FinalTestResult) >= parseFloat(LowerRange)) {
        $("#ResultIcon" + rowIdx).removeClass('fa fa-times-circle text-danger').addClass('fa fa-check text-success');
        clickedrow.find("#hdAction" + rowIdx).val("Pass");
    }
    else {
        $("#ResultIcon" + rowIdx).removeClass('fa fa-check text-success').addClass('fa fa-times-circle text-danger');
        clickedrow.find("#hdAction" + rowIdx).val("Fail");
    }
}
function OnKeyPressParamResult(el, evt) {

    var clickedrow = $(el.target).closest("tr");
    var rowIdx = clickedrow.find("#SNohiddenfiled").val();
    var ValDecDigit = $("#ValDigit").text();

    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find('#Result' + rowIdx).css("border-color", "#ced4da");


    return true;
}
function OnclickParamResultToggle(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var rowIdx = clickedrow.find("#SNohiddenfiled").val();
    //var ToggleValue = $('#ok' + rowIdx).is(":checked");
    if ($('#ok' + rowIdx).is(":checked")) {
        $("#ResultIcon" + rowIdx).removeClass('fa fa-times-circle text-danger').addClass('fa fa-check text-success');

        clickedrow.find("#hdAction" + rowIdx).val("Pass");
    }
    else {
        $("#ResultIcon" + rowIdx).removeClass('fa fa-check text-success').addClass('fa fa-times-circle text-danger');
        clickedrow.find("#hdAction" + rowIdx).val("Fail");
    }

}
function OnChangeAcceptWareHouse() {
    var AcceptedWH = $("#AcceptedWH").val();
    if (AcceptedWH != null && AcceptedWH != "" && AcceptedWH !== "0") {
        $("#AcceptedWH").css("border-color", "#ced4da");
        document.getElementById("vmAcceptedWH").innerHTML = null;
    } else {
        $("#AcceptedWH").css("border-color", "red");
        document.getElementById("vmAcceptedWH").innerHTML = $("#valueReq").text();
    }
}
function ResetQCDetail() {
    debugger;

    $("#ItmSampSize").attr("disabled", false);
    $("#ItmSampSize").val("");
    $("#BtnAddItem").parent().css("display", "block");
    $("#ReplicateFirstRow").attr("disabled", true);
    var rowno = 0;
    var R1 = $("#QCPrmEvalutionTbl > tbody > tr:eq(0)").html();
    $("#QCPrmEvalutionTbl > tbody tr").remove();
    $("#QCPrmEvalutionTbl > tbody").append("<tr>" + R1 + "</tr>");
    //$("#QCPrmEvalutionTbl > tbody > tr").each(function () {
    //    if (rowno > 0) {
    //        $(this).remove();
    //    }
    //    rowno = rowno + 1;
    //})
}
function InsertInspectionDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");
    //var POStatus = "";
    OnApproveclickValidation();
    var qc_type = $("#qc_type").val();
    if (CheckFormValidation() == false) {
        return false;
    }
    if (CheckQCResultValidations() == false) {
        return false;
    }
    if (CheckQCItemValidations() == false) {
        return false;
    }
    if (CheckQCItemParamsValidations() == false) {
        return false;
    }
    if ($("#qc_type").val() == "RQC") {
        if (CheckQCLotBatchValidations() == false) {
            return false;
        }
    }

    //if ($("#qc_type").val() == "PRD" || $("#qc_type").val() == "RQC" || $("#qc_type").val() == "RWK" || $("#qc_type").val() == "SMR") {
    if (CheckValidations_forSubItems() == false) {
        return false;
    }
    if (qc_type == "PRD") {
        if (CheckQCInsightbtnValidations() == false) {
            return false;
        }
    }

    //}
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
        debugger;
        var TransType = "";
        var Inspection_No = "";
        var Inspection_Date = "";
        var Src_Type = "";
        var SrcDocNo = "";
        var SrcDocDate = "";
        var BatchNo = "";
        var Remarks = "";
        var CompID = "";
        var BranchID = "";
        var UserID = "";
        var SystemDetail = "";
        var ReworkWH = "";
        var RejectWH = "";

        if (INSDTransType === 'Update') {
            TransType = 'Update';
        }
        else {
            TransType = 'Save';
        }
        debugger;
        Inspection_No = $("#InspectionNumber").val();
        Inspection_Date = $("#txtQCDate").val();
        Src_Type = $("#qc_type  option:selected").val();
        SrcDocNo = $("#ddlDocumentNumber option:selected").text()
        SrcDocDate = $("#txtsrcdocdate").val();
        BatchNo = $("#BatchNumber").val();
        ItemSerialNo = $("#SerialNumber").val();
        Remarks = $("#remarks").val();
        ReworkWH = $('#ReworkWH').val();
        RejectWH = $('#RejectWH').val();

        var FinalInspectionDetail = [];
        var FinalInspectionItemDetail = [];
        var FinalInspectionResultDetail = [];
        var FinalInspectionAttachDetail = [];

        FinalInspectionDetail.push({ TransType: TransType, Inspection_No: Inspection_No, Inspection_Date: Inspection_Date, Src_Type: Src_Type, SrcDocNo: SrcDocNo, SrcDocDate: SrcDocDate, BatchNo: BatchNo, Remarks: Remarks, CompID: CompID, BranchID: BranchID, UserID: UserID, SystemDetail: SystemDetail, ReworkWH: ReworkWH, RejectWH: RejectWH, ItemSerialNo: ItemSerialNo });

        var editqcparam = sessionStorage.getItem("EditQCparamtable");
        if (editqcparam == 'Edit') {
            // SetSessionData();
        }

        FinalInspectionItemDetail = InsertInspectionItemDetails();
        FinalInspectionResultDetail = InsertInspectionResultDetails();


        var QCItemDt = JSON.stringify(FinalInspectionItemDetail);
        $('#hdQCItemDetailList').val(QCItemDt);

        var QCItemParamDt = JSON.stringify(FinalInspectionResultDetail);
        $('#hdQCItemParamDetailList').val(QCItemParamDt);

        var QCSubItemDt = JSON.stringify(QCSubItemList());
        $('#SubItemDetailsDt').val(QCSubItemDt);


        if ($("#qc_type").val() == "RQC") {
            var LBDetails = sessionStorage.getItem("ItemLBQCDetails");
            $('#hdQCLotBatchDetailList').val(LBDetails);
            $("#SourceWH").prop("disabled", false);
            
        }

        RemoveSessionData();
        $("#qc_type").prop("disabled", true);
        $("#SourceWH").prop("disabled", false);
        
        var DocumentNumber = $('#ddlDocumentNumber option:selected').text();
        $('#hd_doc_no').val(DocumentNumber);
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;


    }
    else {
        //alert("Check network");
        return false;
    }
};

function SetSessionData() {
    debugger;
    var ItemQCList = [];
    $("#QcParamDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var ToggleResult = "";
        var Action = "";

        var Sno = currentRow.find("#SNohiddenfiled").val();
        var ItmCode = currentRow.find("#Item_id" + Sno).val();
        var ItemUOMID = currentRow.find("#uom_id" + Sno).val();
        var ItemSampSize = currentRow.find("#sam_id" + Sno).val();
        var ParamName = currentRow.find("#Parameter" + Sno).val();
        var ParamID = currentRow.find("#param_id" + Sno).val();
        var ParamType = currentRow.find("#Quantitative" + Sno).val();
        var ParamTypeCode = currentRow.find("#paramtype" + Sno).val();
        var ParamUOM = currentRow.find("#ParamUOM" + Sno).val();
        var UpperRange = currentRow.find("#UpperRange" + Sno).val();
        var LowerRange = currentRow.find("#LowerRange" + Sno).val();
        var Result = currentRow.find("#Result" + Sno).val();
        //var Action = currentRow.find("#hdAction" + Sno).val();
        if (currentRow.find("#ok" + Sno).is(":checked")) {
            ToggleResult = 'Y';
        }
        else {
            ToggleResult = 'N';

        }
        if (ParamTypeCode == "L" && ToggleResult == "Y") {
            Action = "Pass";
        }
        if (ParamTypeCode == "L" && ToggleResult == "N") {
            Action = "Fail";
        }
        if (ParamTypeCode == "N") {
            //Action = currentRow.find("#hdAction_" + Sno).val();
            Action = "Pass";
        }

        var Remarks = currentRow.find("#remarks" + Sno).val();

        ItemQCList.push({ ItmCode: ItmCode, ItemUOMID: ItemUOMID, ItemSampSize: ItemSampSize, ParamName: ParamName, ParamID: ParamID, ParamTypeCode: ParamTypeCode, ParamType: ParamType, ParamUOM: ParamUOM, UpperRange: UpperRange, LowerRange: LowerRange, Result: Result, ToggleResult: ToggleResult, Action: Action, Remarks: Remarks })
    });
    sessionStorage.removeItem("ItemQCParamDetails");
    sessionStorage.setItem("ItemQCParamDetails", JSON.stringify(ItemQCList));
}
function InsertInspectionItemDetails() {
    debugger;
    var Src_Type = $("#qc_type  option:selected").val();
    let InspectionItemList = [];
    $("#QcItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        let ItemID = "";
        let ItemName = "";
        let UOMName = "";
        let UOMID = "";
        let sub_item = "";
        let RecivedQty = "";
        let AcceptQty = "";
        let RejectQty = "";
        let ReasonRej = "";
        let ReworkableQty = "";
        let ReasonRwk = "";
        let ShortQty = "";
        let SampleQty = "";
        let Remarks = "";
        var ProdQty = "";
        var PendQty = "";
        var ReasonRej_ID = "";
        var ReasonAccept = "";

        let currentRow = $(this);
        ItemID = currentRow.find("#hdItemId").val();
        ItemName = currentRow.find("#ItemName").val();
        UOMName = currentRow.find("#UOM").val();
        UOMID = currentRow.find("#UOMID").val();
        sub_item = currentRow.find("#sub_item").val();
        RecivedQty = currentRow.find("#RecivedQty").val();
        AcceptQty = currentRow.find("#AcceptQty").val();
        RejectQty = currentRow.find("#RejectQty").val();
        ReasonRej = parseFloat(CheckNullNumber(RejectQty)) > 0 ? currentRow.find("#ReasonForReject").val() : "";
        ReasonRej_ID = parseFloat(CheckNullNumber(RejectQty)) > 0 ? currentRow.find("#ReasonForReject_ID").val() : "0";
        ReworkableQty = currentRow.find("#ReworkableQty").val();
        ReasonRwk = parseFloat(CheckNullNumber(ReworkableQty)) > 0 ? currentRow.find("#ReasonForRework").val() : "";
        ReasonAccept = parseFloat(CheckNullNumber(AcceptQty)) > 0 ? currentRow.find("#ReasonForAccecpt").val() : "";
        ShortQty = CheckNullNumber(currentRow.find("#ShortQty").val());
        SampleQty = CheckNullNumber(currentRow.find("#SampleQty").val());
        Remarks = currentRow.find("#txtItemRemarks").val();
        if (Src_Type == "PRD") {
            ProdQty = CheckNullNumber(currentRow.find('#ProdQty').val());
            PendQty = CheckNullNumber(currentRow.find('#PendQty').val());
        } 
        InspectionItemList.push({
            ItemID: ItemID, ItemName: ItemName, UOMName: UOMName, UOMID: UOMID, sub_item: sub_item
            , RecivedQty: RecivedQty, AcceptQty: AcceptQty, RejectQty: RejectQty, ReasonRej: ReasonRej
            , ReworkableQty: ReworkableQty, ReasonRwk: ReasonRwk,
            ShortQty: ShortQty, SampleQty: SampleQty, Remarks: Remarks, ProdQty: ProdQty, PendQty: PendQty, ReasonRej_ID: ReasonRej_ID, ReasonAccept: ReasonAccept
        });
    });

    return InspectionItemList;
};
function InsertInspectionResultDetails() {
    debugger;
    var FResultDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
    var InspectionResultList = [];

    if (FResultDetails != null) {
        if (FResultDetails.length > 0) {
            debugger;
            for (i = 0; i < FResultDetails.length; i++) {
                var ItemID = FResultDetails[i].ItmCode;
                var ItemName = FResultDetails[i].ItmName;
                var UOMID = FResultDetails[i].ItemUOMID;
                var UOMName = FResultDetails[i].UOMName;
                var SamSize = FResultDetails[i].ItemSampSize;
                var ParameterName = FResultDetails[i].ParamID;
                var Parameter_Name = FResultDetails[i].ParamName;
                var param_uom_Id = FResultDetails[i].ParamUOMID == "" ? "0" : FResultDetails[i].ParamUOMID;
                var ParameterType = FResultDetails[i].ParamTypeCode;
                var ParameterTypeName = FResultDetails[i].ParamType;
                var ParamUOM = FResultDetails[i].ParamUOM;
                var UpperRange = FResultDetails[i].UpperRange;
                var LowerRange = FResultDetails[i].LowerRange;
                var Result = FResultDetails[i].Result;
                var Action = FResultDetails[i].Action;
                var ToggleResult = FResultDetails[i].ToggleResult;
                var Remarks = FResultDetails[i].Remarks;
                var SRNumber = FResultDetails[i].SRNumber;

                InspectionResultList.push({
                    ItemID: ItemID, ItemName: ItemName, UOMID: UOMID, UOMName: UOMName, SamSize: SamSize, ParameterName: ParameterName, Parameter_Name: Parameter_Name, param_uom_Id: IsNull(param_uom_Id, '0'),
                    ParameterType: ParameterType, ParameterTypeName: ParameterTypeName, ParamUOM: ParamUOM, UpperRange: UpperRange, LowerRange: LowerRange,
                    Result: Result, ToggleResult: ToggleResult, Action: Action, Remarks: Remarks, SRNumber: SRNumber
                });
            }
        }
    }
    debugger;
    return InspectionResultList;


};

function OnApproveclickValidation() {

    let NewArr = [];
    var FQCDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
    if (FQCDetails != null) {
        if (FQCDetails.length > 0) {

        }
        else {
            //SetSessionData();

        }
    }
    else {
        //SetSessionData();

    }


}
function OnKeyPressAcceptQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");

    clickedrow.find("#ord_qty_specError").css("display", "none");
    clickedrow.find("#AcceptQty").css("border-color", "#ced4da");


    return true;
}
function OnKeyPressQCQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");

    clickedrow.find("#RecivedQty_specError").css("display", "none");
    clickedrow.find("#RecivedQty").css("border-color", "#ced4da");


    return true;
}
function OnChangeAcceptQty(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity   
    var Src_Type = $("#qc_type  option:selected").val();
    var clickedrow = $(evt.target).closest("tr");
    var Accqty = clickedrow.find("#AcceptQty").val();
    if (AvoidDot(Accqty) == false) {
        Accqty = 0;
    }
    var AcceptQty = parseFloat(Accqty).toFixed(parseFloat(QtyDecDigit));
    var test = parseFloat(parseFloat(AcceptQty)).toFixed(parseFloat(QtyDecDigit));
    if (parseFloat(AcceptQty).toFixed() == parseFloat(0).toFixed()) {
        clickedrow.find("#AcceptQty").val("");
    }
    else {
        if (Src_Type == "PRD") {
            clickedrow.find("#AcceptQty").val(parseFloat(Accqty));
        }
        else {
            clickedrow.find("#AcceptQty").val(test);
        }
        clickedrow.find("#ord_qty_specError").attr("disabled", true);
    }



}
function OnKeyPressRejectQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#RejectQty").css("border-color", "#ced4da");


    return true;
}
function OnChangeRejectQty(el, evt) {
    debugger;
    var Src_Type = $("#qc_type  option:selected").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity   
    var clickedrow = $(evt.target).closest("tr");
    var Rejqty = clickedrow.find("#RejectQty").val();
    if (AvoidDot(Rejqty) == false) {
        Rejqty = 0;
    }
    if (parseFloat(CheckNullNumber(Rejqty)) == 0) {
        clickedrow.find("#ReasonForReject").val("");
    }
    var RejectQuantity = parseFloat(Rejqty).toFixed(parseFloat(QtyDecDigit));
    var test = parseFloat(parseFloat(RejectQuantity)).toFixed(parseFloat(QtyDecDigit));
    if (test > 0) {
        $("#spanRejReq").css("display", "block");
    }
    else {
        $("#spanRejReq").css("display", "none");
    }
    if (parseFloat(RejectQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
        clickedrow.find("#RejectQty").val("");
    }
    else {
        if (Src_Type == "PRD") {
            clickedrow.find("#RejectQty").val(parseFloat(Rejqty));
        }
        else {
            clickedrow.find("#RejectQty").val(test);
        }
    }

    CheckRejectQty();
}

function CheckRejectQty() {
    debugger;
    var RejQtyStatus = "N";
    $("#QcItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var RejectQty = "";
        var currentRow = $(this);
        RejectQty = currentRow.find("#RejectQty").val();
        if (parseFloat(RejectQty) > parseFloat(0)) {
            RejQtyStatus = "Y";
        }
    });
    if (RejQtyStatus == "Y") {
        $("#RejectWH").attr("disabled", false);
        $("#spanRejReq").css("display", "inherit");
    }
    else {
        $("#RejectWH").attr("disabled", true);
        $("#RejectWH").val("0");
        $("#hdRejectWH").val("0");
        $("#spanRejReq").attr("style", "display: none;");
        $("#vmRejectWH").val("");/*Add by Hina under guidance suraj on 28-08-2024 to random qc for shopfloor*/
        /*document.getElementById("vmRejectWH").innerHTML = null;*//*Commented by Hina on 28-08-2024 to random qc for shopfloor*/
        $("#RejectWH").css("border-color", "#ced4da");
    }
}
function OnKeyPressReworkQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#ReworkableQty").css("border-color", "#ced4da");
    return true;
}
function OnKeyPressShortQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#ShortQty").css("border-color", "#ced4da");
    return true;
}
function OnKeyPressSampleQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#SampleQty").css("border-color", "#ced4da");
    return true;
}
function OnChangeReworkQty(el, evt) {
    debugger;
    var Src_Type = $("#qc_type  option:selected").val();

    var QtyDecDigit = $("#QtyDigit").text();///Quantity   
    var clickedrow = $(evt.target).closest("tr");
    var Rewqty = clickedrow.find("#ReworkableQty").val();
    if (AvoidDot(Rewqty) == false) {
        Rewqty = 0;
    }

    if (parseFloat(CheckNullNumber(Rewqty)) == 0) {
        clickedrow.find("#ReasonForRework").val("");
    }
    var ReworkQuantity = parseFloat(Rewqty).toFixed(parseFloat(QtyDecDigit));


    var test = parseFloat(parseFloat(ReworkQuantity)).toFixed(parseFloat(QtyDecDigit));
    if (test > 0) {
        $("#spanReWReq").css("display", "block");
    }
    else {
        $("#spanReWReq").css("display", "none");
    }
    if (parseFloat(ReworkQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
        clickedrow.find("#ReworkableQty").val("");
    }
    else {
        if (Src_Type == "PRD") {
            clickedrow.find("#ReworkableQty").val(parseFloat(Rewqty));
        }
        else {
            clickedrow.find("#ReworkableQty").val(test);
        }
    }

    CheckReworkQty();
}
function CheckReworkQty() {
    debugger;
    var RewQtyStatus = "N";
    $("#QcItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var ReworkQty = "";
        var currentRow = $(this);
        ReworkQty = currentRow.find("#ReworkableQty").val();
        if (parseFloat(ReworkQty) > parseFloat(0)) {
            RewQtyStatus = "Y";
        }
    });
    if (RewQtyStatus == "Y") {
        $("#ReworkWH").attr("disabled", false);
        $("#spanReWReq").css("display", "inherit");
    }
    else {
        $("#ReworkWH").attr("disabled", true);
        $("#ReworkWH").val("0");
        $("#hdReworkWH").val("0");
        $("#spanReWReq").attr("style", "display: none;");
        $("#vmReworkWH").val("");/*Add by Hina under guidance suraj on 28-08-2024 to random qc for shopfloor*/
        //document.getElementById("vmReworkWH").innerHTML = null;/*Commented by Hina on 28-08-2024 to random qc for shopfloor*/
        $("#ReworkWH").css("border-color", "#ced4da");
    }
}
function CheckedCancelled() {
    debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertInspectionDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
function onChangeRejectWH() {
    debugger;
    //$("#vmRejectWH").val("");/*Add by Hina under guidance suraj on 28-08-2024 to random qc for shopfloor*/
    document.getElementById("vmRejectWH").innerHTML = null;
    $("#RejectWH").css("border-color", "#ced4da");
    var rj_wh = $("#RejectWH").val();
    $("#hdRejectWH").val(rj_wh);

}
function onChangeReworkWH() {
    debugger
    //$("#vmReworkWH").val("");/*Add by Hina under guidance suraj on 28-08-2024 to random qc for shopfloor*/
    document.getElementById("vmReworkWH").innerHTML = null;
    $("#ReworkWH").css("border-color", "#ced4da");
    var rw_wh = $("#ReworkWH").val();
    $("#hdReworkWH").val(rw_wh);
}
function inSupplierName(flag) {
    var Src_Type = $("#qc_type  option:selected").val();
   // $("#hdQCtype").val(Src_Type);
    if (Src_Type == "RQC" || Src_Type == "PUR")
    {
        if (flag == "NotShow") {
            $("#ddlSupplierName").val("0");
            $("#Div_SupplierName").css("display", "none");
        }
        else {
            var status = $("#hdQCstatuscode").val();
            if (status != "" && status != null && status != "0") {
                $("#ddlSupplierName").val("0");
                $("#Div_SupplierName").css("display", "none");
            }
            else {
                if (status == "D" || status == "A" || status == "C" || status == "F") {
                    $("#ddlSupplierName").val("0");
                    $("#Div_SupplierName").css("display", "none");
                }
                else {
                    if (flag == "Disabled") {
                        $("#Div_SupplierName").css("display", "block");
                        $("#ddlSupplierName").attr("disabled", true);
                        //$("#ddlSupplierName").val("0").trigger('change');
                    }
                    else {
                        $("#ddlSupplierName").attr("disabled", false);
                        $("#Div_SupplierName").css("display", "block");
                        $("#ddlSupplierName").val("0").trigger('change');
                    }

                }

            }
        }
        
       
      
       
    }
    else {
        $("#ddlSupplierName").val("0");
        $("#Div_SupplierName").css("display", "none");
    }
   
}
function OnchangeQcType() {
    debugger;
    inSupplierName("Enable");
    var Src_Type = $("#qc_type  option:selected").val();
    $("#hdQCtype").val(Src_Type);
    if (Src_Type == "RQC") {/*add by Hina on 28-08-2024 to random qc for shopfloor*/
       
        var loc_Type = $("#hdnLocation_type").val();
        if (loc_Type == "SF") {
          
        }
        else {
         
            document.getElementById("vmsrc_doc_no").innerHTML = null;
            $("#ddlDocumentNumber").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
        }
    }
    else {
       
        document.getElementById("vmsrc_doc_no").innerHTML = null;
        $("#ddlDocumentNumber").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
    }
    //document.getElementById("vmsrc_doc_no").innerHTML = null;/*Commented by Hina on 28-08-2024 to random qc for shopfloor*/
    //$("#ddlDocumentNumber").css("border-color", "#ced4da");
    //$("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
    
    if (Src_Type == "PRD") {
       
        $("#RejectwhID").css('display', 'none');
        $("#ReworkwhID").css('display', 'none'); 
        $("#SourceShflID").css('display', 'none');/*Add By Hina on 21-08-2024 for random qc to shopfloor*/
        $("#div_Location").css('display', 'none');/*Add By Hina on 21-08-2024 for random qc to shopfloor*/
        $("#location_type").val("");
    }

    if (Src_Type == "PUR" || Src_Type == "SCQ") {
       
        $("#RejectwhID").css('display', 'block');
        $("#ReworkwhID").css('display', 'block');
        $("#div_Location").css('display', 'none');/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
        $("#SourceShflID").css('display', 'none');/*Add By Hina on 21-08-2024 for random qc to shopfloor*/
        $("#location_type").val("");/*Add By Hina on 21-08-2024 for random qc to shopfloor*/
    }
    //if (Src_Type == "PUR" || Src_Type == "RQC") {//commanted by shubham maurya on 21-02-2025
    if (Src_Type == "PUR") {
       
       
        $("#th_ShortQty,#th_SampleQty").attr("hidden", false);
    } else {
       
        $("#th_ShortQty,#th_SampleQty").attr("hidden", true);
    }
    if (Src_Type == "RQC") {
       
        $("#div_Location").css('display', 'block');/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
        var status = $("#hdQCstatuscode").val();
        if (status == null || status == "") {
            $("#location_type").val("WH");/*Add By Hina on 21-08-2024 for random qc to shopfloor*/
        }
        OnchangeLocationType();/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
        
    }
    //else if (Src_Type == "SCQ") {

    //}
    else {
        $("#SourcewhID").css('display', 'none');
        $("#AcceptedwhID,#PlusBtnAddItem,#th_Delete,#th_avlQty,#th_LotBatchDetail,#spanQCQtyReq,#spanItemReq").css('display', 'none');
        $("#div_BatchNumber,#Doc_AddIcon,#div_DocDate,#div_DocNumber").css('display', 'block');

        if (Src_Type == "SMR") {
          
            $("#RejectwhID,#ReworkwhID").css('display', 'none');
            $("#div_Location").css('display', 'none');/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
            $("#SourceShflID").css('display', 'none');/*Add By Hina on 21-08-2024 for random qc to shopfloor*/
            $("#location_type").val("");/*Add By Hina on 21-08-2024 for random qc to shopfloor*/
        }
        if (Src_Type == "RWK" || Src_Type == "PJO" || Src_Type == "FGR") {
          
            $("#RejectwhID").css('display', 'none');
            $("#ReworkwhID").css('display', 'none');
            $("#div_BatchNumber").css('display', 'none');
        }
        //$('#QcItmDetailsTbl tbody > tr').find("")
    }
    if (Src_Type != "PRD") {
        $("#ProducedQuantity").css('display', 'none');
        $("#PendingQCQuality").css('display', 'none');
        $("#ProductDetail").css('display', 'none');
    }
    else {
        $("#ProducedQuantity").css('display', '');
        $("#PendingQCQuality").css('display', '');
        $("#ProductDetail").css('display', '');
    }
    $.ajax({
        url: "/ApplicationLayer/QualityInspection/SetDocMenuID",
        data: { Src_Type },
        success: function (data) {
            var arr = data.split(",");
            $("#DocumentMenuId").val(arr[0]);
            if ($("#hdQCstatuscode").val() == "D" || $("#hdQCstatuscode").val() == "" || $("#hdQCstatuscode").val() == null) {
                var WFBar = '<ul class="wizard_steps">';
                if (arr[1] > 0) {
                    for (var i = 1; i <= arr[1]; i++) {
                        WFBar += `
 <li>
                <a href="#step-1" class="disabled" isdone="1" onclick="ForwardBarHistoryClick()" data-toggle="modal" data-target="#WorkflowInformation" data-backdrop="static" data-keyboard="false" rel="1" id="a_${i}">
                    <span class="step_no">${i}</span>
                </a>
            </li>
`;
                    }

                }
                WFBar += '</ul>'
                WFBar += '<input type="hidden" id="hdDoc_No" value="" />';
                $("#wizard").html(WFBar);
            }

        }
    })
    var message = $("#hdn_massage").val();
    if (message == "DocModify") {

    }
    else {
        BindDocumentNumberList();
    }
    /* BindDocumentNumberList();*/
}
/*For shopfloor and Warehouse */
function OnchangeLocationType() {/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
    debugger;
    var hdnCommand = $("#Command").val();
    var Location_Type = $("#location_type  option:selected").val();
    if (Location_Type == "WH") {/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
        $("#hdnLocation_type").val(Location_Type);/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
        $("#SourcewhID,#RejectwhID,#ReworkwhID").css('display', 'block');
        $("#SourceShflID").css('display', 'none');/*Add By Hina on 21-08-2024 for random qc to shopfloor*/
        $("#AcceptedwhID,#th_avlQty,#th_LotBatchDetail,#spanQCQtyReq,#spanItemReq").css('display', '');
        $("#div_BatchNumber,#Doc_AddIcon,#div_DocDate,#div_DocNumber").css('display', 'none');
        if ($("#SourceWH").val() == "0" && hdnCommand != "Refresh") {
            $("#SourceWH").prop('disabled', false);
            
        }
        else {
            $("#SourceWH,#AcceptedWH").prop('disabled', true);
        }
        $("#SourceWH").css("border-color", "#ced4da");
        $("#vmSourceWH").text("");
        $("#AcceptedWH").prop('disabled', true);
        $("#th_Delete").css("display", "");
        inSupplierName("show");
    }
    else {/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
        $("#hdnLocation_type").val(Location_Type);
        $("#SourcewhID,#AcceptedwhID,#RejectwhID,#ReworkwhID").css('display', 'none');
        $("#SourceShflID").css('display', 'block');
        inSupplierName("NotShow");
    }
    
}
function OnChangeSourceShopfloor() {
    debugger;
    var Sourceshfl =  $("#SourceShfl  option:selected").val();
    if (Sourceshfl != null && Sourceshfl != "" && Sourceshfl != "0") {
        $("#PlusBtnAddItem").css("display", '');
        $("#hdnSourceSF").val(Sourceshfl);/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
    }
    else {
        $("#PlusBtnAddItem").css("display", 'none');
    }
    if (Sourceshfl != null && Sourceshfl != "" && Sourceshfl !== "0") {
        $("#hdnSourceSF").val(Sourceshfl);/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
        $("[aria-labelledby='select2-SourceShfl-container']").css("border-color", "#ced4da");
        //$("#SourceShfl").css("border-color", "#ced4da");
        document.getElementById("vmSourceSF").innerHTML = null;
    } else {
        //$("#SourceShfl").css("border-color", "red");
        $("[aria-labelledby='select2-SourceShfl-container']").css("border-color", "red");
        document.getElementById("vmSourceSF").innerHTML = $("#valueReq").text();
    }


}
function AddSampleRows() {
    debugger;
    var sampleSize = $("#ItmSampSize").val();
    var td1 = "";
    var i = 0;
    if (parseFloat(CheckNullNumber(sampleSize)) == 0) {
        $("#ItmSampSize").css("border-color", "red");
        document.getElementById("ItmSampSize_Error").innerHTML = $("#valueReq").text();
        return false;
    }
    else {
        $("#ItmSampSize").css("border-color", "#ced4da");
        document.getElementById("ItmSampSize_Error").innerHTML = null;
    }
    if (OnChangeSampleSize() == false) {
        return false;
    }
    $("#QCPrmEvalutionTbl > tbody > tr >td").each(function () {
        var CurrentRow = $(this);
        var Param_Type = CurrentRow.find("#ParamTypeValue").val();
        if (Param_Type == "N") {
            td1 += `<td ><div class="lpo_form">
    <input class="form-control center" autocomplete="off" style="border: none;" id="td_${i}" onchange="OnchangeParamVal(this)" onkeypress="return OnClickParamVal(this,event);" onkeyup="OnKeyUpParamVal(this, this.value)" value="" />
    <span id="ErrMsg_td_${i}" class="error-message is-visible"></span>
    </div> </td>`;
        }
        if (Param_Type == "O") {
            td1 += `<td ><div class="lpo_form">
    <input class="form-control" autocomplete="off" maxlength="50" id="td_${i}" placeholder="${$("#span_ObFvative").text()}" onchange="OnchangeObservativeVal(this)" title="${$("#span_Observative").text()}" value="" />
    <span id="ErrMsgg_td_${i}" class="error-message is-visible"></span>
    </div> </td>`;
        }
        if (Param_Type == "L") {
            td1 += `<td class="center">
                <label class="switch">
                    <input id="td_${i}" type="checkbox" checked>
                    <span class="slider round"></span>
                </label>
            </td>`;
        }
        i = i + 1;
    });
    var tr = "";
    for (var i = 1; i <= sampleSize; i++) {
        tr += `
        <tr>
            <td class="center">${i}</td>
                ${td1}
        </tr>
       `
    }
    $("#QCPrmEvalutionTbl tbody").append(tr);
    //for (var i = 1; i <= sampleSize; i++) {
    //    $("#QCPrmEvalutionTbl tbody").append(`
    //    <tr>
    //        <td>${i}</td>
    //            ${td1}
    //    </tr>
    //   `)
    //}
    $("#ItmSampSize").attr("disabled", true);
    if ($("#Disable").val() == "N") {
        $("#ReplicateFirstRow").attr("disabled", false);
    }
    else {
        $("#ReplicateFirstRow").attr("disabled", true);
    }
    $("#BtnAddItem").parent().css("display", "none");

}
function AddNewRow() {
    var srno = $('#QcItmDetailsTbl tbody > tr').length;
    srno = parseFloat(srno) + 1;
    var deletetext = $("#Span_Delete_Title").text();
    var span_SubItemDetail = $("#span_SubItemDetail").text();
    var origin = window.location.origin + "/Content/Images/qc1.png";
    let Src_Type = $("#qc_type").val();
    let ShortAndSampleRow = '';
    //if (Src_Type == "PUR" || Src_Type == "RQC") {//commanted by shubham maurya on 21-02-2025
    if (Src_Type == "PUR") {
        ShortAndSampleRow = `<td>
                                 <div class=" col-sm-10 lpo_form" style="padding:0px;">
                                         <input id="ShortQty" value="" readonly onchange="OnChangeShortQty(event)" onkeypress="return OnKeyPressShortQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="ShortQty" placeholder="0000.00">
                                 </div>
                                 <div class=" col-sm-2 i_Icon" id="div_SubItemShortQty" style="padding:0px;">
                                     <button type="button" id="SubItemShortQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('QCShortQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title='${span_SubItemDetail}'> </button>
                                 </div>
                             </td>
                             <td>
                                <div class=" col-sm-10 lpo_form" style="padding:0px;">
                                        <input id="SampleQty" value="" readonly onchange="OnChangeSampleQty(event)" onkeypress="return OnKeyPressSampleQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="SampleQty" placeholder="0000.00">
                                </div>
                                <div class=" col-sm-2 i_Icon" id="div_SubItemSampleQty" style="padding:0px;">
                                    <button type="button" id="SubItemSampleQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('QCSampleQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title='${span_SubItemDetail}'> </button>
                                </div>
                            </td>`;
    }

    $('#QcItmDetailsTbl tbody').append(`<tr id="R1">
                                   <td class="red center bom_width_td"><i class="fa fa-trash red deleteIcon" aria-hidden="true" title="${deletetext}"></i></td>
                                   <td class="sr_padding"><span id="SpanRowId">${srno}</span></td>
                                   <td class="ItmNameBreak itmStick tditemfrz">
                                       <div class=" col-sm-11 lpo_form " style="padding:0px;">
                                           <select id="ItemName_${srno}" class="form-control" onchange="OnchangeItemName(event)">
                                               <option value='0'>---Select---</option>
                                           </select>
                                           <input type="hidden" id="ItemName" value="0" />
                                           <input type="hidden" id="hdItemId" value="" />
                                           <input type="hidden" id="hdRowID" value="${srno}" />
                                           <span id="ItemNameError" class="error-message is-visible"></span>
                                       </div>
                                       <div class="col-sm-1 i_Icon received_quantity">

                                           <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$(" #Span_ItemInformation_Title").text()}"> </button>
                                       </div>
                                   </td>
                                   <td>
                                       <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$(" #ItemUOM").text()}" disabled value="">
                                       <input type="hidden" id="UOMID" value="" style="display: none;" />
                                   </td>
                                   <td>
                                   <div class="lpo_form col-sm-10 no-padding" >
                                       <input id="AvailableStockInBase" value="0.000" class="form-control num_right" autocomplete="off" type="text" name="AvlQty" placeholder="0000.00" disabled="">
                                   </div>
                                         <input hidden id="sub_item" value="" />
                                     <div class=" col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty" >
                                         <button type="button" id="SubItemAvlQty" disabled class="calculator subItmImg" onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                     </div>
                                   </td>
                                   <td>
                                       <div class="lpo_form col-sm-8 no-padding"  id="multiWrapper">
                                           <input id="RecivedQty" onchange="OnchangeQCQuantity(event)" onkeyup="OnKeyUpQCQuantity(event)" autocomplete="off" onkeypress="return OnKeyPressQCQty(this,event);" value="" class="form-control num_right" autocomplete="" type="text" name="RecivedQty" placeholder="0000.00">
                                           <span id="RecivedQty_specError" class="error-message is-visible"></span>
                                      </div>
                                     <div class=" col-sm-2 i_Icon no-padding" id="div_SubItemQCQty" >
                                         <button type="button" id="SubItemQCQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('ProduceQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                     </div>
                                       <div class="col-sm-2 received_quantity no-padding" >
                                           <a onclick="" id="ParameterEvlsnPopUp" data-toggle="modal" data-target="" data-backdrop="static" data-keyboard="false" class="calculator">
                                               <img src="${origin}" height="22" width="22" aria-hidden="true" title="${$(" #Span_ParameterEvaluatione_Title").text()}" />
                                           </a>
                                       </div>
                                   </td>
                                   <td class="center">
                                       <button type="button" id="LotBatchDetailsBtn" onclick="return OnclickLotBatchDetailsBtn(event)" disabled class="calculator" data-toggle="modal" data-target="#QCAdhocDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                   </td>
                                   <td>
                                       <div class="lpo_form col-sm-8 no-padding">
                                           <input id="AcceptQty" value="" class="form-control num_right" onkeypress="return OnKeyPressAcceptQty(this,event);" onchange="OnChangeAcceptQty(this,event)" autocomplete="" type="text" name="AcceptQty" placeholder="0000.00" readonly>
                                           <span id="ord_qty_specError" class="error-message is-visible"></span>
                                       </div>
                           <div class="col-sm-2 i_Icon" id="div_SubItemAccQty" style="padding:0px;">
                               <button type="button" id="SubItemAccQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCAccQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                           </div>
                                           <div class="col-sm-2 i_Icon">
                                           <button type="button" class="calculator" id="BtnReasonForAccecpt" onclick="return onClickReasonRemarks(event,'Accept')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_ReasonForAccept_Remarks").text()}">  </button>
                                           <input  type="hidden" id="ReasonForAccecpt" />
                                       </div>
                                   </td>
                                   <td>
                                       <div class="col-sm-8 lpo_form no-padding" >
                                       <input id="RejectQty" value="" class="form-control num_right" autocomplete="" type="text" name="RejectQty" onkeypress="return OnKeyPressRejectQty(this,event);" onchange="OnChangeRejectQty(this,event)" placeholder="0000.00" readonly>
                                       </div>
                                       <div class=" col-sm-2 i_Icon" id="div_SubItemRejQty" style="padding:0px;">
                                           <button type="button" id="SubItemRejQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCRejQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                       </div>
                                       <div class="col-sm-2 i_Icon">
                                           <button type="button" class="calculator" id="BtnReasonForReject" onclick="return onClickReasonRemarks(event,'reject')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_ReasonForRejection").text()}">  </button>
                                           <input hidden id="ReasonForReject"  />
                                          <input type="hidden" id="ReasonForReject_ID" />
                                           <input type="hidden" id="ReasonForReject_Name" />
                                       </div>
                                   </td>
                                   <td>
                                       <div class="col-sm-8 lpo_form no-padding" >
                                       <input id="ReworkableQty" value="" class="form-control num_right" autocomplete="" type="text" name="ReworkableQty" placeholder="0000.00" onkeypress="return OnKeyPressReworkQty(this,event);" onchange="OnChangeReworkQty(this,event)" readonly>
                                       </div>
                                       <div class=" col-sm-2 i_Icon" id="div_SubItemRewQty" style="padding:0px;">
                                       <button type="button" id="SubItemRewQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCRewQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                       </div>
                                       <div class="col-sm-2 i_Icon">
                                           <button type="button" class="calculator" id="BtnReasonForRework" onclick="return onClickReasonRemarks(event,'rework')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_Reworkremarks").text()}">  </button>
                                           <input hidden id="ReasonForRework" value=''/>
                                       </div>
                                   </td>
                                      `+ ShortAndSampleRow + `
                                   <td>
                                       <textarea id="txtItemRemarks" value="" class="form-control" name="txtItemRemarks" placeholder="${$("#span_remarks").text()}" onblur="this.placeholder='${$("#span_remarks").text()}'" data-parsley-validation-threshold="10"></textarea>
                                   </td>
                               </tr>`);
    if ($("#qc_type").val() == "RQC") {
        debugger;
        var srcwh = "";
        var LocationTyp = $("#hdnLocation_type").val();/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
        if (LocationTyp == 'WH') {
            srcwh = $("#SourceWH").val();
        }
        else {
            srcwh = $("#hdnSourceSF").val();/*Add By Hina on 17-08-2024 for random qc to shopfloor*/
        }
        var RndmQC = "RndmQC" + ',' + srcwh 
        DynamicSerchableItemDDLForQc("#QcItmDetailsTbl", "#ItemName_", srno, "#hdRowID", "", "RndmQC", srcwh, LocationTyp);
    }
    else {
        var srcwh = "";
       var LocationTyp = "";/*Add By Hina on 21-08-2024 to random qc by Shopfloor*/
        DynamicSerchableItemDDLForQc("#QcItmDetailsTbl", "#ItemName_", srno, "#hdRowID", "", "QC", srcwh, LocationTyp);
    }
    $("#ddlSupplierName").prop("disabled", true);
    $("#qc_type").prop("disabled", true);
    $("#SourceWH").prop("disabled", true);
    $("#location_type").prop("disabled", true);/*Add By Hina on 21-08-2024 to random qc by Shopfloor*/
    $("#SourceShfl").prop("disabled", true);/*Add By Hina on 21-08-2024 to random qc by Shopfloor*/
}
function DynamicSerchableItemDDLForQc(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName, srcwh, LocationTyp) {
    debugger;
    var suppid = "0";
    if (PageName != "RndmQC") {
        //var srcwh = $("#SourceWH").val();
        $("#ItemName_" + RowID).select2({
            ajax: {
                url: "/ApplicationLayer/QualityInspection/GetItemList",
                data: function (params) {
                    var queryParameters = {
                        SearchName: params.term,
                        PageName: PageName,
                        SrcWh_Id: srcwh,
                        SrcShfl_Id: srcshfl,/*Add By Hina on 21-08-2024 to random qc by Shopfloor*/
                        LocationTyp: LocationTyp,
                        suppid: suppid,
                        page: params.page || 1
                    };
                    return queryParameters;
                },
                multiple: true,
                cache: true,
                processResults: function (data, params) {
                    var pageSize,
                        pageSize = 20;//50000; // or whatever pagesize
                    ConvertIntoArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField);
                    var ItemListArrey = [];
                    if (sessionStorage.getItem("selecteditemlist").length != null) {
                        ItemListArrey = JSON.parse(sessionStorage.getItem("selecteditemlist"));
                    }
                    let selected = [];
                    selected.push({ id: $(ItmDDLName + RowID).val() });
                    selected = JSON.stringify(selected);

                    var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));


                    data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));

                    if ($(".select2-search__field").parent().find("ul").length == 0) {
                        $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div></div>
</strong></li></ul>`)
                    }
                    var page = params.page || 1;
                    Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                    if (page == 1) {
                        if (Fdata[0] != null) {
                            if (Fdata[0].Name.trim() != "---Select---") {
                                var select = { ID: "0_0", Name: " ---Select---" };
                                Fdata.unshift(select);
                            }
                        }
                    }

                    return {
                        results: $.map(Fdata, function (val, Item) {
                            return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
                        }),
                        pagination: {
                            more: (page * pageSize) < data.length
                        }
                    };
                },
                cache: true
            },
            templateResult: function (data) {

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
    else {
        suppid = $("#hdnSupplierID").val();
        //var srcwh = "";
        $("#ItemName_" + RowID).select2({
            ajax: {
                url: "/ApplicationLayer/QualityInspection/GetItemList",
                data: function (params) {
                    var queryParameters = {
                        SearchName: params.term,
                        PageName: PageName,
                        SrcWh_Id: srcwh,
                        LocationTyp: LocationTyp,/*Add By Hina on 21-08-2024 to random qc by Shopfloor*/
                        suppid: suppid,
                        page: params.page || 1
                    };
                    return queryParameters;
                },
                multiple: true,
                cache: true,
                processResults: function (data, params) {
                    debugger
                    data = JSON.parse(data).Table;
                    var pageSize,
                        pageSize = 20;//50000; // or whatever pagesize
                    ConvertIntoArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField);
                    var ItemListArrey = [];
                    if (sessionStorage.getItem("selecteditemlist").length != null) {
                        ItemListArrey = JSON.parse(sessionStorage.getItem("selecteditemlist"));
                    }
                    let selected = [];
                    selected.push({ id: $(ItmDDLName + RowID).val() });
                    selected = JSON.stringify(selected);

                    var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));


                    //data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));
                    data = data.filter(j => !JSON.stringify(NewArrey).includes(j.Item_id));

                    if ($(".select2-search__field").parent().find("ul").length == 0) {
                        $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div></div>
</strong></li></ul>`)
                    }
                    var page = params.page || 1;
                    Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                    if (page == 1) {
                        if (Fdata[0] != null) {
                            if (Fdata[0].item_name.trim() != "---Select---") {
                                //var select = { ID: "0_0", Name: " ---Select---" };//Commented by Suraj Maurya on 13-03-2025
                                var select = { Item_id: "0", item_name: " ---Select---", uom_name: "", avlstk:0 };
                                Fdata.unshift(select);
                            }
                        }
                    }

                    return {
                        results: $.map(Fdata, function (val, Item) {
                            //return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
                            return { id: val.Item_id, text: val.item_name, UOM: val.uom_name };
                        }),
                        pagination: {
                            more: (page * pageSize) < data.length
                        }
                    };
                },
                cache: true
            },
            templateResult: function (data) {

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
}
function OnChangeSourceWareHouse() {
    debugger;
    var SourceWH = $("#SourceWH").val();
    if (SourceWH != null && SourceWH != "" && SourceWH != "0") {
        $("#PlusBtnAddItem").css("display", '');
    }
    else {
        $("#PlusBtnAddItem").css("display", 'none');
    }
    if (SourceWH != null && SourceWH != "" && SourceWH !== "0") {
        $("#SourceWH").css("border-color", "#ced4da");
        document.getElementById("vmSourceWH").innerHTML = null;
    } else {
        $("#SourceWH").css("border-color", "red");
        document.getElementById("vmSourceWH").innerHTML = $("#valueReq").text();
    }


}
function ResetSerialNo() {
    debugger;
    var Srno = 1;
    if ($('#QcItmDetailsTbl tbody tr').length > 0) {
        $('#QcItmDetailsTbl tbody tr').each(function () {
            $(this).find("#SpanRowId").text(Srno);
            Srno++;
        });
    }
    else {
        var status = $("#hdQCstatuscode").val();
        if (status == "" || status == null) {
            $("#SourceWH").prop("disabled", true);
        }
        else {
            $("#SourceWH").prop("disabled", false);
        }
        $("#location_type").prop("disabled", false);/*Add By Hina on 21-08-2024 to random qc for shopfloor*/
        $("#SourceShfl").prop("disabled", false);
    }

}
function OnchangeItemName(e) {
    var currentRow = $(e.target).closest("tr");
    var RowNo = currentRow.find("#hdRowID").val();
    var hdItemId = currentRow.find("#hdItemId").val();
    DeleteItemLotBatchDetails(hdItemId);
    DeleteItemParamDetails(hdItemId);
    Cmn_DeleteSubItemQtyDetail(hdItemId);
    var itemName = currentRow.find("#ItemName_" + RowNo + " option:selected").text();
    var itemID = currentRow.find("#ItemName_" + RowNo + " option:selected").val();
    currentRow.find("#ItemName").val(itemName);
    currentRow.find("#hdItemId").val(itemID);
    currentRow.find('[aria-labelledby="select2-ItemName_' + RowNo + '-container"]').css("border-color", "#ced4da");
    currentRow.find("#ItemNameError").text("");
    currentRow.find("#ItemNameError").css("display", "none");
    if (itemID != "0" && itemID != null && itemID != "") {
        currentRow.find("#ParameterEvlsnPopUp").attr("data-target", "#RecQuant");
        currentRow.find("#ParameterEvlsnPopUp").attr("onclick", "OnClickReceiveQtybtn(event);");
    }
    else {
        currentRow.find("#ParameterEvlsnPopUp").attr("data-target", "");
        currentRow.find("#ParameterEvlsnPopUp").attr("onclick", "");
    }
    ClearRowData(currentRow);
    Cmn_BindUOM(currentRow, itemID, "", "", "");
    debugger;
    var QCType = $("#hdQCtype").val();/*Add by Hina on 21-08-2024 to randome qc for shopfloor*/
    var Location_type = $("#hdnLocation_type").val();
    if (QCType == "RQC" && Location_type == "SF") {
        GetShflAvlStock(currentRow, itemID);
    }
    else {
        GetAvlStock(currentRow, itemID);
    }
    

}
function GetShflAvlStock(currentRow, itemID) {/*Add by Hina on 21-08-2024 to randome qc for shopfloor*/
    debugger;
    var SourceShfl = $("#SourceShfl").val();
    var MaterialType = "AC";

    $.ajax({
        type: "Post",
        url: "/Common/Common/GetItemAvlStockShopfloor",
        data: {
            Itm_ID: itemID,
            MaterialType: MaterialType,
            SourceShopfloor: SourceShfl
        },
        success: function (data) {
            debugger
            var avaiableStock = JSON.parse(data);
            var parseavaiableStock = parseFloat(avaiableStock.Table[0].avl_stock_shfl).toFixed(QtyDecDigit)
            currentRow.find("#AvailableStockInBase").val(parseavaiableStock);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
        }
    })
}
function OnKeyUpQCQuantity(e) {
    var currentRow = $(e.target).closest("tr");
    var RecivedQty = currentRow.find("#RecivedQty").val();
    if (parseFloat(CheckNullNumber(RecivedQty)) > 0) {
        currentRow.find("#RecivedQty").css("border-color", "#ced4da");
        currentRow.find("#RecivedQty_specError").text("");
        currentRow.find("#RecivedQty_specError").css("display", "none");
    }


}
function GetAvlStock(currentRow, itemID) {
    debugger;
    var SourceWH = $("#SourceWH").val();
    
    $.ajax({
        type: "Post",
        url: "/Common/Common/getWarehouseWiseItemStock",
        data: {
            ItemId: itemID,
            WarehouseId: SourceWH
           
        },
        success: function (data) {
            debugger
            var avaiableStock = JSON.parse(data);
            var parseavaiableStock = parseFloat(avaiableStock).toFixed(QtyDecDigit)
            currentRow.find("#AvailableStockInBase").val(parseavaiableStock);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
        }
    })
}
function PLBindItemBatchDetail() {
    var batchrowcount = $('#SaveItemBatchTbl tr').length;
    if (batchrowcount > 1) {
        var ItemBatchList = new Array();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            debugger;
            batchList.LotNo = row.find('#PD_BatchLotNo').val();
            batchList.ItemId = row.find('#PD_BatchItemId').val();
            batchList.UOMId = row.find('#PD_BatchUOMId').val();
            batchList.BatchNo = row.find('#PD_BatchBatchNo').val();
            batchList.BatchAvlStock = row.find('#PD_BatchBatchAvlStk').val();
            batchList.IssueQty = row.find('#PD_BatchIssueQty').val();

            var ExDate = row.find('#PD_BatchExpiryDate').val().trim();
            var FDate = "";
            if (ExDate == "") {
                FDate = "";
            }
            else {
                var date = ExDate.split("-");
                FDate = date[2] + '-' + date[1] + '-' + date[0];
            }
            batchList.ExpiryDate = FDate;
            ItemBatchList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwise").val(str1);
    }
}
function OnclickLotBatchDetailsBtn(evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var Src_Type = $("#qc_type  option:selected").val();
        var SourceWH = $("#SourceWH").val();
        var Sourceshfl = $("#hdnSourceSF").val();
        var Location_type = $("#hdnLocation_type").val();
        var ItemId = clickedrow.find("#hdItemId").val();;
        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var UOMId = clickedrow.find("#UOMID").val();
        var PL_Status = $("#hdQCstatuscode").val();//.trim();
        var RecivedQty = clickedrow.find("#RecivedQty").val();
        var TransType = $("#hdn_TransType").val();
        var Command = $("#Command").val();
        var DocumentStatus = $("#DocumentStatus").val();
        var DocumentMenuId = $("#DocumentMenuId").val();
        var SelectedItemdetail = sessionStorage.getItem("ItemLBQCDetails");
        if (RecivedQty != null && RecivedQty != "" && Location_type=="SF") {/*Add By Hina on 21-08-2024 to random qc for shopfloor*/
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/QualityInspection/getItemStockBatchWiseOfShopFloor",
                data: {
                    ItemId: ItemId,
                    ShflID: Sourceshfl,
                    Status: PL_Status,
                    SelectedItemdetail: SelectedItemdetail,
                    TransType: TransType,
                    Command: Command,
                    DocumentStatus: DocumentStatus,
                    DocumentMenuId: DocumentMenuId,
                    UOMId: UOMId,
                    Src_Type: Src_Type
                },
                success: function (data) {
                    debugger;
                    $('#QCLotBatchPopUp').html(data);
                    $("#ItemNameBatchWise").val(ItemName);
                    $("#UOMBatchWise").val(UOMName);
                    $("#QCQuantity").val(RecivedQty);
                    $("#HDItemNameBatchWise").val(ItemId);
                    $("#HDUOMBatchWise").val(UOMId);

                    //var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

                    //if ($("#SaveItemBatchTbl TBODY TR").length > 0) {
                    //    $("#SaveItemBatchTbl TBODY TR").each(function () {
                    //        var row = $(this)
                    //        var BtItemId = row.find("#PD_BatchItemId").val();
                    //        if (BtItemId === ItemId) {
                    //            TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#PD_BatchIssueQty").val());
                    //        }
                    //    });
                    //}

                    //$("#BatchWiseItemStockTbl TBODY TR").each(function () {
                    //    var row = $(this)
                    //    var issueQty = row.find("#IssuedQuantity").val();
                    //    if (issueQty != null && issueQty != "") {
                    //        row.find("#IssuedQuantity").val(parseFloat(issueQty).toFixed(QtyDecDigit))
                    //    }
                    //});

                    //$("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));


                },
            });
        }
        else if (RecivedQty != null && RecivedQty != "") {
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/QualityInspection/getItemStockBatchWise",
                data: {
                    ItemId: ItemId,
                    WarehouseId: SourceWH,
                    Status: PL_Status,
                    SelectedItemdetail: SelectedItemdetail,
                    TransType: TransType,
                    Command: Command,
                    DocumentStatus: DocumentStatus,
                    DocumentMenuId: DocumentMenuId,
                    Src_Type: Src_Type

                },
                success: function (data) {
                    debugger;
                    $('#QCLotBatchPopUp').html(data);
                    $("#ItemNameBatchWise").val(ItemName);
                    $("#UOMBatchWise").val(UOMName);
                    $("#QCQuantity").val(RecivedQty);
                    $("#HDItemNameBatchWise").val(ItemId);
                    $("#HDUOMBatchWise").val(UOMId);

                    //var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

                    //if ($("#SaveItemBatchTbl TBODY TR").length > 0) {
                    //    $("#SaveItemBatchTbl TBODY TR").each(function () {
                    //        var row = $(this)
                    //        var BtItemId = row.find("#PD_BatchItemId").val();
                    //        if (BtItemId === ItemId) {
                    //            TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#PD_BatchIssueQty").val());
                    //        }
                    //    });
                    //}

                    //$("#BatchWiseItemStockTbl TBODY TR").each(function () {
                    //    var row = $(this)
                    //    var issueQty = row.find("#IssuedQuantity").val();
                    //    if (issueQty != null && issueQty != "") {
                    //        row.find("#IssuedQuantity").val(parseFloat(issueQty).toFixed(QtyDecDigit))
                    //    }
                    //});

                    //$("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));


                },
            });
        }
        else {
            return false;
        }

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnChangeLBQty(evt) {
    debugger
    var ClickedRow = $(evt.target).closest("tr");
    var LBAccQty = ClickedRow.find("#LBAccQuantity").val();
    var LBRejQty = ClickedRow.find("#LBRejQuantity").val();
    var LBRewQty = ClickedRow.find("#LBRewQuantity").val();
    var LBShortQty = ClickedRow.find("#LBShortQuantity").val();
    var LBSampleQty = ClickedRow.find("#LBSampleQuantity").val();
    var totalQcQty = parseFloat(CheckNullNumber(LBAccQty)) + parseFloat(CheckNullNumber(LBRejQty)) + parseFloat(CheckNullNumber(LBRewQty))
        + parseFloat(CheckNullNumber(LBShortQty)) + parseFloat(CheckNullNumber(LBSampleQty));
    var BLAvlQty = ClickedRow.find("#AvailableQuantity").val();
    if (parseFloat(CheckNullNumber(evt.currentTarget.value)).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
        evt.currentTarget.value = "";
    }
    else {
        evt.currentTarget.value = parseFloat(CheckNullNumber(evt.currentTarget.value)).toFixed(QtyDecDigit);
    }

    LBAccQty = 0;
    LBRejQty = 0;
    LBRewQty = 0;
    LBShortQty = 0;
    LBSampleQty = 0;

    $("#BatchWiseItemStockTbl > tbody > tr").each(function () {
        var CurrentRow = $(this);
        LBAccQty += parseFloat(CheckNullNumber(CurrentRow.find("#LBAccQuantity").val()));
        LBRejQty += parseFloat(CheckNullNumber(CurrentRow.find("#LBRejQuantity").val()));
        LBRewQty += parseFloat(CheckNullNumber(CurrentRow.find("#LBRewQuantity").val()));
        LBShortQty += parseFloat(CheckNullNumber(CurrentRow.find("#LBShortQuantity").val()));
        LBSampleQty += parseFloat(CheckNullNumber(CurrentRow.find("#LBSampleQuantity").val()));
    })
    var Tfoot = $("#BatchWiseItemStockTbl > tfoot > tr");
    Tfoot.find("#TotalLBAccQty").text(parseFloat(LBAccQty).toFixed(QtyDecDigit));
    Tfoot.find("#TotalLBRejQty").text(parseFloat(LBRejQty).toFixed(QtyDecDigit));
    Tfoot.find("#TotalLBRewQty").text(parseFloat(LBRewQty).toFixed(QtyDecDigit));
    Tfoot.find("#TotalLBShortQty").text(parseFloat(LBShortQty).toFixed(QtyDecDigit));
    Tfoot.find("#TotalLBSampleQty").text(parseFloat(LBSampleQty).toFixed(QtyDecDigit));
    if (parseFloat(totalQcQty) > parseFloat(BLAvlQty)) {
        $(evt.currentTarget).css("border-color", "red");
        $(evt.currentTarget).parent().find('#LBQuantity_Error').html($("#ExceedingQty").text());
        $(evt.currentTarget).parent().find('#LBQuantity_Error').css("display", "block");
        //swal("", "QC Quantity Exceeding", "warning");
    }
    else {
        $(evt.currentTarget).css("border-color", "#ced4da");
        $(evt.currentTarget).parent().find('#LBQuantity_Error').html("");
        $(evt.currentTarget).parent().find('#LBQuantity_Error').css("display", "none");
        //$(evt.currentTarget).parent().find('<span class="field-validation-valid" style="color:red;!important">Value required.</span>').remove()
    }



}
function OnkeyUpLBQty(evt) {
    $(evt.currentTarget).css("border-color", "#ced4da");
    $(evt.currentTarget).parent().find('#LBQuantity_Error').html("");
    $(evt.currentTarget).parent().find('#LBQuantity_Error').css("display", "none");
}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var Flag = "N";
    var Tfoot = $("#BatchWiseItemStockTbl > tfoot > tr");
    var LBAccQty = Tfoot.find("#TotalLBAccQty").text();
    var LBRejQty = Tfoot.find("#TotalLBRejQty").text();
    var LBRewQty = Tfoot.find("#TotalLBRewQty").text();
    var LBShortQty = Tfoot.find("#TotalLBShortQty").text();
    var LBSampleQty = Tfoot.find("#TotalLBSampleQty").text();
    var Total = parseFloat(parseFloat(CheckNullNumber(LBAccQty)) + parseFloat(CheckNullNumber(LBRejQty)) + parseFloat(CheckNullNumber(LBRewQty))
        + parseFloat(CheckNullNumber(LBShortQty)) + parseFloat(CheckNullNumber(LBSampleQty))).toFixed(QtyDecDigit);

    var QCQuantity = $("#QCQuantity").val();
    if (CheckNullNumber(Total) == CheckNullNumber(QCQuantity)) {

        var ItemUOMID = $("#HDUOMBatchWise").val();
        var ItemId = $("#HDItemNameBatchWise").val();
        var QCQuantity = $("#QCQuantity").val();

        var NewArr = [];
        var LBQCDetails = sessionStorage.getItem("ItemLBQCDetails");
        if (LBQCDetails != null) {
            LBQCDetails = JSON.parse(LBQCDetails);
            for (var j = 0; j < LBQCDetails.length; j++) {
                if (LBQCDetails[j].ItmCode != ItemId) {
                    NewArr.push(LBQCDetails[j]);
                }
            }
        }
        var TtlAccQty = 0;
        var chk_LBAccQty = 0;
        var chk_LBRejQty = 0;
        var chk_LBRewQty = 0;
        var chk_LBShortQty = 0;
        var chk_LBSampleQty = 0;
        $("#BatchWiseItemStockTbl TBODY TR").each(function () {
            debugger;
            var row = $(this);
            var LBAccQty = row.find("#LBAccQuantity").val();
            var LBRejQty = row.find("#LBRejQuantity").val();
            var LBRewQty = row.find("#LBRewQuantity").val();
            var LBShortQty = row.find("#LBShortQuantity").val();
            var LBSampleQty = row.find("#LBSampleQuantity").val();
            TtlAccQty = TtlAccQty + parseFloat(CheckNullNumber(LBAccQty));
            var totalQcQty = parseFloat(CheckNullNumber(LBAccQty)) + parseFloat(CheckNullNumber(LBRejQty)) + parseFloat(CheckNullNumber(LBRewQty)) + parseFloat(CheckNullNumber(LBShortQty)) + parseFloat(CheckNullNumber(LBSampleQty));
            var BLAvlQty = row.find("#AvailableQuantity").val();
            chk_LBAccQty = parseFloat(chk_LBAccQty) + parseFloat(CheckNullNumber(LBAccQty));
            chk_LBRejQty = parseFloat(chk_LBRejQty) + parseFloat(CheckNullNumber(LBRejQty));
            chk_LBRewQty = parseFloat(chk_LBRewQty) + parseFloat(CheckNullNumber(LBRewQty));
            chk_LBShortQty = parseFloat(chk_LBShortQty) + parseFloat(CheckNullNumber(LBShortQty));
            chk_LBSampleQty = parseFloat(chk_LBSampleQty) + parseFloat(CheckNullNumber(LBSampleQty));

            if (parseFloat(CheckNullNumber(BLAvlQty)) < parseFloat(CheckNullNumber(totalQcQty))) {
                Flag = "Y";
                if (parseFloat(CheckNullNumber(LBRewQty)) > 0) {
                    row.find("#LBRewQuantity").css("border-color", "red");
                    row.find("#LBRewQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBRewQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                else if (parseFloat(CheckNullNumber(LBRejQty)) > 0) {
                    row.find("#LBRejQuantity").css("border-color", "red");
                    row.find("#LBRejQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBRejQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                else if (parseFloat(CheckNullNumber(LBAccQty)) > 0) {
                    row.find("#LBAccQuantity").css("border-color", "red");
                    row.find("#LBAccQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBAccQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                else if (parseFloat(CheckNullNumber(LBShortQty)) > 0) {
                    row.find("#LBShortQuantity").css("border-color", "red");
                    row.find("#LBShortQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBShortQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                else if (parseFloat(CheckNullNumber(LBSampleQty)) > 0) {
                    row.find("#LBSampleQuantity").css("border-color", "red");
                    row.find("#LBSampleQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBSampleQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                //return false;
            }
            if (Flag == "N") {
                if (parseFloat(CheckNullNumber(BLAvlQty)) >= parseFloat(CheckNullNumber(totalQcQty))
                    && parseFloat(CheckNullNumber(totalQcQty)) != 0) {

                    var ItemBatchNo = row.find("#BatchNumber").val();
                    var ItemSerialNo = row.find("#SerialNumber").val();
                    var ItemBatchAvlstock = row.find("#AvailableQuantity").val();
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    var LotNo = row.find("#Lot").val();
                    var mfg_name = row.find("#BtMfgName").val();
                    var mfg_mrp = row.find("#BtMfgMrp").val();
                    var mfg_date = row.find("#BtMfgDate").val();
                    NewArr.push({
                        ItmCode: ItemId, ItemUOMID: ItemUOMID, ItemBatchNo: ItemBatchNo, ItemSerialNo: ItemSerialNo,
                        ItemBatchAvlstock: ItemBatchAvlstock, ItemExpiryDate: ItemExpiryDate, LotNo: LotNo,
                        LBAccQty: CheckNullNumber(LBAccQty), LBRejQty: CheckNullNumber(LBRejQty)
                        , LBRewQty: CheckNullNumber(LBRewQty), LBShortQty: CheckNullNumber(LBShortQty)
                        , LBSampleQty: CheckNullNumber(LBSampleQty), QCQuantity: QCQuantity
                        , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                    });
                }
            }


        });
        var checkLBAccQty = 0, checkLBRejQty = 0, checkLRewQty = 0;
        NewArr.map((item) => {
            checkLBAccQty = parseFloat(checkLBAccQty) + parseFloat(item.LBAccQty);
            checkLBRejQty = parseFloat(checkLBRejQty) + parseFloat(item.LBRejQty);
            checkLRewQty = parseFloat(checkLRewQty) + parseFloat(item.LBRewQty);
        });
        //if (CheckNullNumber(LBAccQty) > 0) {
        if (CheckNullNumber(checkLBAccQty) > 0) {
            $("#AcceptedWH").attr("disabled", false);
            $("#spanAccReq").css("display", '');
        }
        else {
            $("#AcceptedWH").val(0).attr("disabled", true);
            $("#spanAccReq").css("display", 'none');
        }
        //if (CheckNullNumber(LBRejQty) > 0) {
        if (CheckNullNumber(checkLBRejQty) > 0) {
            $("#RejectWH").attr("disabled", false);
        }
        else {
            $("#RejectWH").val(0).attr("disabled", true);
            $("#hdRejectWH").val(0)

        }
        //if (CheckNullNumber(LBRewQty) > 0) {
        if (CheckNullNumber(checkLRewQty) > 0) {
            $("#ReworkWH").attr("disabled", false);
        }
        else {
            $("#ReworkWH").val(0).attr("disabled", true);
            $("#hdReworkWH").val(0);
        }
        //if (CheckNullNumber(TtlAccQty) > 0) {
        //    $("#AcceptedWH").attr("disabled", false);
        //    $("#spanAccReq").css("display", '');
        //} else {
        //    $("#AcceptedWH").attr("disabled", true);
        //    $("#spanAccReq").css("display", 'none');
        //}
        if (Flag == "Y") {
            return false;
        }

        var ItemId = $("#HDItemNameBatchWise").val();
        var BatchClickedRow = $("#QcItmDetailsTbl> tbody > tr  #hdItemId[value=" + ItemId + "]").closest("tr");
        BatchClickedRow.find("#AcceptQty").val(LBAccQty);
        if (parseFloat(CheckNullNumber(LBRejQty)) > 0) {
            BatchClickedRow.find("#RejectQty").val(LBRejQty).trigger("change");
            //$("#RejectWH").prop("disabled", false);
        }
        else {
            BatchClickedRow.find("#RejectQty").val(LBRejQty).trigger("change");
            //$("#RejectWH").val(0);
            //$("#RejectWH").prop("disabled", true);
        }
        if (parseFloat(CheckNullNumber(LBRewQty)) > 0) {
            BatchClickedRow.find("#ReworkableQty").val(LBRewQty).trigger("change");
            //$("#ReworkWH").prop("disabled", false);
        } else {
            BatchClickedRow.find("#ReworkableQty").val(LBRewQty).trigger("change");
            //$("#ReworkWH").val(0);
            //$("#ReworkWH").prop("disabled", true);
        }
        if (parseFloat(CheckNullNumber(LBShortQty)) > 0) {
            BatchClickedRow.find("#ShortQty").val(LBShortQty).trigger("change");
        } else {
            BatchClickedRow.find("#ShortQty").val(LBShortQty).trigger("change");
        }
        if (parseFloat(CheckNullNumber(LBSampleQty)) > 0) {
            BatchClickedRow.find("#SampleQty").val(LBSampleQty).trigger("change");
        } else {
            BatchClickedRow.find("#SampleQty").val(LBSampleQty).trigger("change");
        }
        $("#QcItmDetailsTbl >tbody >tr").each(function () {
            var ItmCode = $(this).find("#hdItemId").val();
            if (ItmCode == ItemId) {
                //$(this).find("#LotBatchDetailsBtn").css("border", "");
                ValidateEyeColor($(this), "LotBatchDetailsBtn", "N");
                return false;
            }
        });

        sessionStorage.removeItem("ItemLBQCDetails");
        sessionStorage.setItem("ItemLBQCDetails", JSON.stringify(NewArr));
        $("#QCAdhocDetail").modal('hide');
    }
    else {
        Flag = "N";
        $("#BatchWiseItemStockTbl TBODY TR").each(function () {
            var row = $(this);
            var LBAccQty = row.find("#LBAccQuantity").val();
            var LBRejQty = row.find("#LBRejQuantity").val();
            var LBRewQty = row.find("#LBRewQuantity").val();
            var LBShortQty = row.find("#LBShortQuantity").val();
            var LBSampleQty = row.find("#LBSampleQuantity").val();
            var totalQcQty = parseFloat(CheckNullNumber(LBAccQty)) + parseFloat(CheckNullNumber(LBRejQty)) + parseFloat(CheckNullNumber(LBRewQty))
                + parseFloat(CheckNullNumber(LBShortQty)) + parseFloat(CheckNullNumber(LBSampleQty));
            var BLAvlQty = row.find("#AvailableQuantity").val();
            if (parseFloat(CheckNullNumber(BLAvlQty)) < parseFloat(CheckNullNumber(totalQcQty))) {
                Flag = "Y";
                if (parseFloat(CheckNullNumber(LBRewQty)) > 0) {
                    row.find("#LBRewQuantity").css("border-color", "red");
                    row.find("#LBRewQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBRewQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                else if (parseFloat(CheckNullNumber(LBRejQty)) > 0) {
                    row.find("#LBRejQuantity").css("border-color", "red");
                    row.find("#LBRejQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBRejQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                else if (parseFloat(CheckNullNumber(LBAccQty)) > 0) {
                    row.find("#LBAccQuantity").css("border-color", "red");
                    row.find("#LBAccQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBAccQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                else if (parseFloat(CheckNullNumber(LBShortQty)) > 0) {
                    row.find("#LBShortQuantity").css("border-color", "red");
                    row.find("#LBShortQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBShortQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                else if (parseFloat(CheckNullNumber(LBSampleQty)) > 0) {
                    row.find("#LBSampleQuantity").css("border-color", "red");
                    row.find("#LBSampleQuantity").parent().find("#LBQuantity_Error").html($("#ExceedingQty").text());
                    row.find("#LBSampleQuantity").parent().find("#LBQuantity_Error").css("display", "block");
                }
                //return false;
            }
            else {
                row.find("#LBRewQuantity").css("border-color", "#ced4da");
                row.find("#LBRewQuantity").parent().find("#LBQuantity_Error").html("").text();
                row.find("#LBRewQuantity").parent().find("#LBQuantity_Error").css("display", "none");
                row.find("#LBRejQuantity").css("border-color", "#ced4da");
                row.find("#LBRejQuantity").parent().find("#LBQuantity_Error").html("").text();
                row.find("#LBRejQuantity").parent().find("#LBQuantity_Error").css("display", "none");
                row.find("#LBAccQuantity").css("border-color", "#ced4da");
                row.find("#LBAccQuantity").parent().find("#LBQuantity_Error").html("").text();
                row.find("#LBAccQuantity").parent().find("#LBQuantity_Error").css("display", "none");
                row.find("#LBShortQuantity").css("border-color", "#ced4da");
                row.find("#LBShortQuantity").parent().find("#LBQuantity_Error").html("").text();
                row.find("#LBShortQuantity").parent().find("#LBQuantity_Error").css("display", "none");
                row.find("#LBSampleQuantity").css("border-color", "#ced4da");
                row.find("#LBSampleQuantity").parent().find("#LBQuantity_Error").html("").text();
                row.find("#LBSampleQuantity").parent().find("#LBQuantity_Error").css("display", "none");
            }

        });
        if (Flag == "N") {
            swal("", $("#QCQuantityDoesNotMatchWithTotalQuantity").text(), "warning");
        }
        return false;
    }
}
function OnKeyPressSampleSize(el, evt) {
    if (Cmn_IntValueonly(el, evt) == false) {
        return false;
    }
}

function OnchangeQCQuantity(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    AvoidChar(e.currentTarget, "QtyDigit");
    var AvlQty = currentRow.find("#AvailableStockInBase").val();
    var QcQty = currentRow.find("#RecivedQty").val();
    currentRow.find("#RecivedQty").val(parseFloat(CheckNullNumber(QcQty)).toFixed(QtyDecDigit));
    if (parseFloat(QcQty) > parseFloat(AvlQty)) {
        currentRow.find("#RecivedQty").css("border-color", "red");
        currentRow.find("#RecivedQty_specError").text($("#ExceedingQty").text());
        currentRow.find("#RecivedQty_specError").css("display", "block");
    } else {
        currentRow.find("#RecivedQty").css("border-color", "#ced4da");
        currentRow.find("#RecivedQty_specError").text("");
        currentRow.find("#RecivedQty_specError").css("display", "none");
        currentRow.find("#LotBatchDetailsBtn").prop("disabled", false);
    }
}
function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl > tbody > tr").each(function () {
        var Row = $(this);
        Row.find("#LBAccQuantity").val("");
        Row.find("#LBRejQuantity").val("");
        Row.find("#LBRewQuantity").val("");
        Row.find("#LBShortQuantity").val("");
        Row.find("#LBSampleQuantity").val("");
        Row.find("#LBAccQuantity").css("border-color", "#ced4da");
        Row.find("#LBRejQuantity").css("border-color", "#ced4da");
        Row.find("#LBRewQuantity").css("border-color", "#ced4da");
        Row.find("#LBShortQuantity").css("border-color", "#ced4da");
        Row.find("#LBSampleQuantity").css("border-color", "#ced4da");
        Row.find("#LBAccQuantity").parent().find('#LBQuantity_Error').html("");
        Row.find("#LBAccQuantity").parent().find('#LBQuantity_Error').css("display", "none");
        Row.find("#LBRejQuantity").parent().find('#LBQuantity_Error').html("");
        Row.find("#LBRejQuantity").parent().find('#LBQuantity_Error').css("display", "none");
        Row.find("#LBRewQuantity").parent().find('#LBQuantity_Error').html("");
        Row.find("#LBRewQuantity").parent().find('#LBQuantity_Error').css("display", "none");
        Row.find("#LBShortQuantity").parent().find('#LBQuantity_Error').html("");
        Row.find("#LBShortQuantity").parent().find('#LBQuantity_Error').css("display", "none");
        Row.find("#LBSampleQuantity").parent().find('#LBQuantity_Error').html("");
        Row.find("#LBSampleQuantity").parent().find('#LBQuantity_Error').css("display", "none");
    });
    var Tfoot = $("#BatchWiseItemStockTbl > tfoot > tr");
    Tfoot.find("#TotalLBAccQty").text(parseFloat(0).toFixed(QtyDecDigit));
    Tfoot.find("#TotalLBRejQty").text(parseFloat(0).toFixed(QtyDecDigit));
    Tfoot.find("#TotalLBRewQty").text(parseFloat(0).toFixed(QtyDecDigit));
}
function OnClickParamVal(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    debugger
    var key = evt.key;
    var number = el.value.split('.');
    var valParam = 0;
    if (number.length == 1) {
        valParam = number[0] + '' + key;
    }
    else {
        valParam = number[0] + '.' + key;
    }
    OnKeyUpParamVal(el, valParam);
}
function OnClickParamValForReplicate(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    debugger
    var key = evt.key;
    var number = el.value.split('.');
    var valParam = 0;
    if (number.length == 1) {
        valParam = number[0] + '' + key;
    }
    else {
        valParam = number[0] + '.' + key;
    }
    OnKeyUpParamValForReplicate(el, valParam);
}

function OnChangeShortQty(e) {
    let currentRow = $(e.target).closest("tr");
    let ShortQty = currentRow.find("#ShortQty").val();
    if (parseFloat(ShortQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit) || ShortQty == "" || ShortQty == null) { /***Added By Nitesh 01-03-2024 For Paceholder***/
        currentRow.find("#ShortQty").val("");
    }
    else {
        currentRow.find("#ShortQty").val(parseFloat(CheckNullNumber(ShortQty)).toFixed(QtyDecDigit));
    }


}
function OnChangeSampleQty(e) {
    debugger;
    let currentRow = $(e.target).closest("tr");
    let SampleQty = currentRow.find("#SampleQty").val();
    if (parseFloat(SampleQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit) || SampleQty == "" || SampleQty == null) { /***Added By Nitesh 01-03-2024 For Paceholder***/
        currentRow.find("#SampleQty").val("");
    }
    else {
        currentRow.find("#SampleQty").val(parseFloat(CheckNullNumber(SampleQty)).toFixed(QtyDecDigit));
    }

}

function OnChangeSampleSize() {
    debugger
    var ItmSampSize = $("#ItmSampSize").val();
    var HdnItemSampleSize = $("#HdnItemSampleSize").val();

    //if (parseFloat(CheckNullNumber(ItmSampSize)) > parseFloat(CheckNullNumber(HdnItemSampleSize))) {
    if (parseFloat(CheckNullNumber(HdnItemSampleSize))<=0) {
        $("#ItmSampSize").css("border-color", "red");
        document.getElementById("ItmSampSize_Error").innerHTML = $("#ExceedingQty").text();
        $("#ItmSampSize").val(CheckNullNumber(ItmSampSize));
        return false;
    }
    else {
        if (CheckNullNumber(ItmSampSize) > 0) {
            $("#ItmSampSize").val(CheckNullNumber(ItmSampSize));
        } else {
            $("#ItmSampSize").val("");
        }

        $("#ItmSampSize").css("border-color", "#ced4da");
        document.getElementById("ItmSampSize_Error").innerHTML = null;
    }
}
function OnchangeObservativeVal(el) {
    debugger
    var currentrow = $(el.target).closest('tr');
    $(el).closest("input").css("border", "none");
    $(el).closest("input").parent().find("#ErrMsgg_" + el.id).text("");
    $(el).closest("input").parent().find("#ErrMsgg_" + el.id).css("display", "none");
}
function OnchangeParamVal(el) {
    debugger

    if (AvoidChar(el, "QtyDigit") == false) {
        el.value = parseFloat(0).toFixed($("#ValDigit").text());
        return false;
    }
    else {
        el.value = parseFloat(el.value).toFixed($("#ValDigit").text());
    }

}
function OnKeyUpParamVal(el, valParam) {
    debugger;
    var ParametersVal = $("#QCPrmEvalutionTbl > tbody> tr:eq(0)").find("#" + el.id).text().trim().split(" ");
    if (ParametersVal == "Observative") {

    }
    else if ((parseFloat(ParametersVal[2]) >= parseFloat(valParam)) && (parseFloat(valParam) >= parseFloat(ParametersVal[0]))) {
        $(el).closest("input").css("color", "");
    }
    else {
        $(el).closest("input").css("color", "red");
    }

    if (valParam > 0) {
        $(el).closest("input").css("border", "none");
        $(el).closest("input").parent().find("#ErrMsg_" + el.id).text("");
        $(el).closest("input").parent().find("#ErrMsg_" + el.id).css("display", "none");
    }


}
function OnKeyUpParamValForReplicate(el, valParam) {
    debugger;
    if ($("#thReplicateDeatil").text() == "N") {
        var ParametersVal = $("#tdHeaderData2").text().trim().split(" ");
    }
    //else {
    //    var ParametersVal = $("#QCPrmEvalutionTbl > tbody> tr:eq(0)").find("#" + el.id).text().trim().split(" ");
    //}
    if (ParametersVal == "Observative") {

    }
    else if ((parseFloat(ParametersVal[2]) >= parseFloat(valParam)) && (parseFloat(valParam) >= parseFloat(ParametersVal[0]))) {
        $(el).closest("input").css("color", "");
    }
    else {
        $(el).closest("input").css("color", "red");
    }
    if (valParam > 0) {
        $(el).closest("input").css("border", "none");
        $(el).closest("input").parent().find("#ErrMsg_" + el.id).text("");
        $(el).closest("input").parent().find("#ErrMsg_" + el.id).css("display", "none");
    }
}
function SetParamDataInSession() {

}
function OnClickSaveAndExit() {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var ItmCode = $("#ItemCode").val();
    var ItemUOMID = $("#hdUOMId").val();
    var ItmName = $("#QC_ItemName").val();
    var UOMName = $("#UOM").val();
    var ItemSampSize = $("#ItmSampSize").val();
    var error = "N";
    var td_id = "#td_"
    var srno = 0;
    var SRNumber = 1;
    let NewArr = [];
    var countNoErr = 0;
    if ($("#QCPrmEvalutionTbl >tbody >tr").length > 1) {
        var FQCDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
        if (FQCDetails != null) {
            if (FQCDetails.length > 0) {
                for (i = 0; i < FQCDetails.length; i++) {

                    var ItemID = FQCDetails[i].ItmCode;

                    if (ItemID == ItmCode) {
                    }
                    else {
                        NewArr.push(FQCDetails[i]);
                    }
                }
            }

            $("#QCPrmEvalutionTbl >tbody >tr").each(function () {
                var currentRow = $(this);

                debugger;
                if (srno > 0) {
                    srno = 0;
                    let ErFlgRow = "Y";
                    let FlgRowSno = 0
                    currentRow.find("td").each(function () {
                        if (FlgRowSno > 0) {
                            let Crw = $(this);
                            var ParamType = $("#QCPrmEvalutionTbl > tbody> tr:eq(0)").find(td_id + FlgRowSno + " #ParamTypeValue").val();
                            var Result = Crw.find(td_id + FlgRowSno).val();
                            if (ParamType == "N" && Result == "" || Result == "NaN" || Result == "0" || Result == parseFloat("0").toFixed(DecDigit)) {

                            }
                            else if (ParamType == "O" && Result == "" || Result == "NaN" || Result == "0" || Result == parseFloat("0").toFixed(DecDigit)) {

                            } else {
                                ErFlgRow = "N";
                            }
                        }
                        FlgRowSno++;

                    });
                    let RowChecked = "N";
                    currentRow.find("td").each(function () {
                        debugger;
                        if (srno > 0) {
                            var CurrentColumn = $(this);
                            var ParamType = $("#QCPrmEvalutionTbl > tbody> tr:eq(0)").find(td_id + srno + " #ParamTypeValue").val();
                            var Result = CurrentColumn.find(td_id + srno).val();
                            if (ParamType == "N" && Result == "" || Result == "NaN" || Result == "0" || Result == parseFloat("0").toFixed(DecDigit)) {
                                if (ErFlgRow == "Y") {
                                    CurrentColumn.find(td_id + srno).css("border", "1px solid red");
                                    CurrentColumn.find("#ErrMsg_td_" + srno).css("display", "block");
                                    CurrentColumn.find("#ErrMsg_td_" + srno).text($("#valueReq").text());
                                }
                                else {
                                    CurrentColumn.find(td_id + srno).css("border-color", "#ced4da");
                                    CurrentColumn.find("#ErrMsg_td_" + srno).css("display", "none");
                                    CurrentColumn.find("#ErrMsg_td_" + srno).text("");
                                }
                                error = "Y";
                            }
                            else if (ParamType == "O" && Result == "" || Result == "NaN" || Result == "0" || Result == parseFloat("0").toFixed(DecDigit)) {
                                if (ErFlgRow == "Y") {
                                    CurrentColumn.find(td_id + srno).css("border", "1px solid red");
                                    CurrentColumn.find("#ErrMsgg_td_" + srno).css("display", "block");
                                    CurrentColumn.find("#ErrMsgg_td_" + srno).text($("#valueReq").text());
                                }
                                else {
                                    CurrentColumn.find(td_id + srno).css("border-color", "#ced4da");
                                    CurrentColumn.find("#ErrMsgg_td_" + srno).css("display", "none");
                                    CurrentColumn.find("#ErrMsgg_td_" + srno).text("");
                                }
                                error = "Y";
                            }
                            else {
                                if (RowChecked == "N") {//For Count Only One filled entry row 
                                    countNoErr++;
                                    RowChecked = "Y";
                                }
                            }

                            if (ParamType == "O") {
                                CurrentColumn.find(td_id + srno).parent().parent().css("border-color", "#ced4da");
                                CurrentColumn.find("#ErrMsgg_td_" + srno).css("display", "none");
                                CurrentColumn.find("#ErrMsgg_td_" + srno).text("");
                            }
                            CurrentColumn.find(td_id + srno).parent().parent().css("border-color", "#ced4da");
                            CurrentColumn.find("#ErrMsg_td_" + srno).css("display", "none");
                            CurrentColumn.find("#ErrMsg_td_" + srno).text("");

                            var ParamName = $("#th_" + srno).text().trim();
                            var ParamID = $("#thPId_" + srno).val().trim();
                            var ParamTypeCode = ParamType;
                            var ParamType = "";
                            var ParamArr = $("#QCPrmEvalutionTbl > tbody> tr:eq(0)").find("#td_" + srno).text().trim().split(" ");
                            var ParamUOM = ParamArr[3];
                            var ParamUOMID = $("#QCPrmEvalutionTbl > tbody> tr:eq(0)").find("#td_" + srno).find("#ParamUomId").val();
                            var UpperRange = ParamArr[2];
                            var LowerRange = ParamArr[0];
                            var Result = "";
                            if (ParamTypeCode == "N") {
                                Result = CurrentColumn.find(td_id + srno).val();
                            }
                            if (ParamTypeCode == "O") {
                                Result = CurrentColumn.find(td_id + srno).val();
                                UpperRange = 0;
                                LowerRange = 0;
                                ParamUOM = 0;
                            }
                            var Action = "";
                            var ToggleResult = "";
                            if (ParamTypeCode == "L") {
                                if (CurrentColumn.find(td_id + srno).is(":checked")) {
                                    ToggleResult = 'Y';
                                    UpperRange = 0;
                                    LowerRange = 0;
                                    /***Commented by Shubham Maurya on 18-10-2023 for ParamTypeCode="L" so save Uom '0'****/
                                    //ParamUOM = 101;
                                    ParamUOM = 0;
                                }
                                else {
                                    ToggleResult = 'N';
                                    UpperRange = 0;
                                    LowerRange = 0;
                                    /***Commented by Shubham Maurya on 18-10-2023 for ParamTypeCode="L" so save Uom '0'****/
                                    //ParamUOM = 101;
                                    ParamUOM = 0;
                                }
                            }

                            if (ParamTypeCode == "L" && ToggleResult == "Y") {
                                Action = "Pass";
                            }
                            if (ParamTypeCode == "L" && ToggleResult == "N") {
                                Action = "Fail";
                            }
                            if (ParamTypeCode == "N") {
                                Action = "Pass";
                            }
                            if (ParamTypeCode == "O") {
                                Action = "Pass";
                            }
                            NewArr.push({
                                ItmCode: ItmCode, ItmName: ItmName, ItemUOMID: ItemUOMID, UOMName: UOMName, ItemSampSize: ItemSampSize,
                                ParamName: ParamName, ParamID: ParamID, ParamTypeCode: ParamTypeCode,
                                ParamType: ParamType, ParamUOM: ParamUOM, ParamUOMID: ParamUOMID, UpperRange: UpperRange,
                                LowerRange: LowerRange, Result: Result, ToggleResult: ToggleResult,
                                Action: Action, Remarks: "", SRNumber: SRNumber, td_no: srno
                            })

                        }
                        srno = srno + 1;
                    });
                    SRNumber = SRNumber + 1;
                }
                srno = srno + 1;

            });
        }
        var SampleLength = $("#QCPrmEvalutionTbl tbody tr").length - 1;
        if (error == "Y" && countNoErr != SampleLength) {
            return false
        }
        else {
            debugger;
            $("#QcItmDetailsTbl >tbody >tr").each(function () {
                var itemId = $(this).find("#hdItemId").val();
                if (ItmCode == itemId) {
                    $(this).find("#ParameterEvlsnPopUp").css("outline", "");
                    return false;
                }
            });
            $("#SaveExitClose").attr("data-dismiss", "modal");
            sessionStorage.removeItem("ItemQCParamDetails");
            sessionStorage.setItem("ItemQCParamDetails", JSON.stringify(NewArr));

        }

        var qc_type = $("#qc_type").val();
        if (qc_type != "RQC") {
            var FQCDetails = JSON.parse(sessionStorage.getItem("ItemQCParamDetails"));
            if (FQCDetails != null) {
                if (FQCDetails.length > 0) {
                    for (i = 0; i < FQCDetails.length; i++) {
                        var FQCItemID = FQCDetails[i].ItmCode;
                        $("#QcItmDetailsTbl >tbody >tr").each(function () {
                            var currentRow = $(this);
                            //var Sno = currentRow.find("#SNohiddenfiled").val();
                            debugger;
                            var QCItemID = currentRow.find("#hdItemId").val();
                            if (QCItemID == FQCItemID) {
                                debugger;
                                currentRow.find("#AcceptQty").attr("readonly", false);
                                currentRow.find("#RejectQty").attr("readonly", false);
                                if (qc_type != "SMR") {
                                    currentRow.find("#ReworkableQty").attr("readonly", false);
                                }
                                //if (qc_type == "PUR" || qc_type == "RQC") {
                                if (qc_type == "PUR") {//commanted by shubham maurya on 21-02-2025
                                    currentRow.find("#ShortQty").attr("readonly", false);
                                    currentRow.find("#SampleQty").attr("readonly", false);
                                }

                            }
                            else {
                                //debugger;
                                //currentRow.find("#AcceptQty").attr("readonly", true);
                                //currentRow.find("#RejectQty").attr("readonly", true);
                                //currentRow.find("#ReworkableQty").attr("readonly", true);
                            }
                        });
                    }

                }

            }
        }
    }
    else {
        swal("", $("#InvalidParameterData").text(), "warning");
        return false;
    }




}
function OnKeyPressNumField(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }

}
function ClearRowData(row) {
    row.find("#RecivedQty").val("");
    row.find("#AcceptQty").val("");
    row.find("#RejectQty").val("");
    row.find("#ReworkableQty").val("");
    row.find("#ShortQty").val("");
    row.find("#SampleQty").val("");
}



/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemQCQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAccQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemRejQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemRewQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemShortQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemSampleQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlQty",);
    //Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemPlanQty",);
}
function SubItemDetailsPopUp(flag, e) {
    debugger
    var hdStatus = $("#hdQCstatuscode").val();
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#hfsno").val();
    var ProductNm = clickdRow.find("#ItemName").val();
    var ProductId = clickdRow.find("#hdItemId").val();
    var UOM = clickdRow.find("#UOM").val();
    var Doc_no = $("#ddlDocumentNumber option:selected").text();//$("#InspectionNumber").val();
    var Doc_dt = $("#txtsrcdocdate").val();//$("#txtQCDate").val();
    var qc_type = $("#qc_type").val();//$("#txtQCDate").val();
    //var UOMID = clickdRow.find("#UOMID").val();
    var src_wh_id = 0;
    var src_shfl_id = 0;/*Add by Hina on 23-08-2024 to random qc for shopfloor*/
    var location_type = "";
    
    if (flag == "ProduceQty") {
        $("#SubItemPopUp").css("max-width", "600px");
    }
    else {
        $("#SubItemPopUp").css("max-width", "900px");
    }
    if (qc_type == "RQC") {
        Doc_no = $("#InspectionNumber").val();
        Doc_dt = $("#txtQCDate").val(); 
        location_type = $("#hdnLocation_type").val();/*Add by Hina on 23-08-2024 to random qc for shopfloor*/
        if (location_type == "WH") {
            src_wh_id = $("#SourceWH").val();
        }
        else {
            src_shfl_id = $("#SourceShfl").val();
        }
        
    }
    if (qc_type == "SMR") {
        if (flag == "ProduceQty" || (flag == "QCAccQty" && hdStatus == "") || (flag == "QCRejQty" && hdStatus == "") || (flag == "QCRewQty" && hdStatus == "")) {
            var Doc_no = $("#ddlDocumentNumber option:selected").text();
            var Doc_dt = $("#txtsrcdocdate").val();
        }
        else {
            Doc_no = $("#InspectionNumber").val();
            Doc_dt = $("#txtQCDate").val();
        }

    }

    var Sub_Quantity = 0;
    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        //List.ProdQty = row.find('#subItemProdQty').val();
        List.ProdQty = row.find('#subItemOrdQty').val();
        List.PendQty = row.find('#subItemPendQty').val();
        if (qc_type == "PRD") {
            List.qty = row.find('#subItemQcsQty').val();
        }
        else {
            List.qty = row.find('#subItemQty').val();
        }
        //List.qty = row.find('#subItemQty').val();
        List.Accqty = row.find('#subItemAccQty').val();
        List.Rejqty = row.find('#subItemRejQty').val();
        List.Rewqty = row.find('#subItemRewQty').val();
        List.Shortqty = CheckNullNumber(row.find('#subItemShortQty').val());
        List.Sampleqty = CheckNullNumber(row.find('#subItemSampleQty').val());
        debugger;
        if (qc_type == "RWK" || qc_type == "PJO"  ) {
            List.avaiableStock = row.find('#subItemAvlStk').val();
        }
        NewArr.push(List);
    });
    if (flag == "ProduceQty" || flag == "Quantity") {
        Sub_Quantity = clickdRow.find("#RecivedQty").val();
    } else if (flag == "QCAccQty") {
        Sub_Quantity = clickdRow.find("#AcceptQty").val();
    } else if (flag == "QCRejQty") {
        Sub_Quantity = clickdRow.find("#RejectQty").val();
    } else if (flag == "QCRewQty") {
        Sub_Quantity = clickdRow.find("#ReworkableQty").val();
    } else if (flag == "QCShortQty") {
        Sub_Quantity = clickdRow.find("#ShortQty").val();
    } else if (flag == "QCSampleQty") {
        Sub_Quantity = clickdRow.find("#SampleQty").val();
    } else if (flag == "PRDProdQty") {
        Sub_Quantity = clickdRow.find("#ProdQty").val();
    } else if (flag == "PRDPendQty") {
        Sub_Quantity = clickdRow.find("#PendQty").val();
    } else if (flag == "PRDProduceQty") {
        Sub_Quantity = clickdRow.find("#RecivedQty").val();
    }

    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hdQCstatuscode").val();
    hd_Status = IsNull(hd_Status, "").trim();

    if (hd_Status != "" && hd_Status != "D" && hd_Status != "F") {
        Doc_no = $("#InspectionNumber").val();
        Doc_dt = $("#txtQCDate").val();

    }

    if (qc_type == "RQC") {
        location_type = $("#hdnLocation_type").val();/*Add by Hina on 23-08-2024 to random qc for shopfloor*/
        if (location_type == "WH") {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/QualityInspection/GetSubItemDetails",
                data: {
                    Item_id: ProductId,
                    SubItemListwithPageData: JSON.stringify(NewArr),
                    IsDisabled: IsDisabled,
                    Flag: flag,
                    Status: hd_Status,
                    Doc_no: Doc_no,
                    Doc_dt: Doc_dt,
                    src_wh_id: src_wh_id,
                    qc_type: qc_type,
                    location_type: location_type
                },
                success: function (data) {
                    debugger;
                    $("#SubItemPopUp").html(data);
                    $("#Sub_ProductlName").val(ProductNm);
                    $("#Sub_ProductlId").val(ProductId);
                    $("#Sub_serialUOM").val(UOM);
                    $("#Sub_Quantity").val(Sub_Quantity);
                }

            })
        }
        else {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/QualityInspection/GetSubItemDetails",
                data: {
                    Item_id: ProductId,
                    SubItemListwithPageData: JSON.stringify(NewArr),
                    IsDisabled: IsDisabled,
                    Flag: flag,
                    Status: hd_Status,
                    Doc_no: Doc_no,
                    Doc_dt: Doc_dt,
                    src_wh_id: src_shfl_id,
                    qc_type: qc_type,
                    location_type: location_type
                },
                success: function (data) {
                    debugger;
                    $("#SubItemPopUp").html(data);
                    $("#Sub_ProductlName").val(ProductNm);
                    $("#Sub_ProductlId").val(ProductId);
                    $("#Sub_serialUOM").val(UOM);
                    $("#Sub_Quantity").val(Sub_Quantity);
                }

            })
        }
    }
    else {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/QualityInspection/GetSubItemDetails",
            data: {
                Item_id: ProductId,
                SubItemListwithPageData: JSON.stringify(NewArr),
                IsDisabled: IsDisabled,
                Flag: flag,
                Status: hd_Status,
                Doc_no: Doc_no,
                Doc_dt: Doc_dt,
                src_wh_id: src_wh_id,
                qc_type: qc_type,
                location_type: location_type
            },
            success: function (data) {
                debugger;
                $("#SubItemPopUp").html(data);
                $("#Sub_ProductlName").val(ProductNm);
                $("#Sub_ProductlId").val(ProductId);
                $("#Sub_serialUOM").val(UOM);
                $("#Sub_Quantity").val(Sub_Quantity);
            }

        })
    }
   
    if (flag == 'enable') {

    }
    else if (flag = 'readonly') {

    }
}

function CheckValidations_forSubItems() {
    //return Cmn_CheckValidations_forSubItems("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y");
    return QC_CheckValidations_forSubItems("QcItmDetailsTbl", "", "hdItemId", "RecivedQty", "SubItemQCQty", "Y");
}

function QC_CheckValidations_forSubItems(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage) {
    debugger;
    var qc_type = $("#qc_type").val();
    var flag = "N";
    $("#" + Main_table + " tbody tr").each(function () {
        var PPItemRow = $(this);
        var item_id;
        if (Row_Id != "" && Row_Id != null) {
            var Sno = PPItemRow.find("#" + Row_Id).val();
            item_id = PPItemRow.find("#" + Item_field_id + Sno).val();
        } else {
            item_id = PPItemRow.find("#" + Item_field_id).val();
        }

        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
        var item_PrdAccQty = PPItemRow.find("#" + "AcceptQty").val();
        var item_PrdRejQty = PPItemRow.find("#" + "RejectQty").val();
        var item_PrdRewQty = PPItemRow.find("#" + "ReworkableQty").val();
        var item_PrdShortQty = PPItemRow.find("#" + "ShortQty").val();
        var item_PrdSampleQty = PPItemRow.find("#" + "SampleQty").val();
        var item_PrdQcQty = PPItemRow.find("#" + "RecivedQty").val();
        item_PrdShortQty = CheckNullNumber(item_PrdShortQty);
        item_PrdSampleQty = CheckNullNumber(item_PrdSampleQty);

        var sub_item = PPItemRow.find("#sub_item").val();
        var Sub_Quantity = 0;
        var Sub_AccQuantity = 0;
        var Sub_RejQuantity = 0;
        var Sub_RewQuantity = 0;
        var Sub_ShortQuantity = 0;
        var Sub_SampleQuantity = 0;
        var Sub_subItemQcsQty = 0;
        var Sub_AvailQuantity = 0;
        $("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr').each(function () {
            var Crow = $(this).closest("tr");
            //var subItemId = Crow.find("#subItemId").val();
            var subItemQty = Crow.find("#subItemQty").val();
            Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
            var subItemAccQty = Crow.find("#subItemAccQty").val();
            Sub_AccQuantity = parseFloat(Sub_AccQuantity) + parseFloat(CheckNullNumber(subItemAccQty));
            var subItemRejQty = Crow.find("#subItemRejQty").val();
            Sub_RejQuantity = parseFloat(Sub_RejQuantity) + parseFloat(CheckNullNumber(subItemRejQty));
            var subItemRewQty = Crow.find("#subItemRewQty").val();
            Sub_RewQuantity = parseFloat(Sub_RewQuantity) + parseFloat(CheckNullNumber(subItemRewQty));
            var subItemShortQty = Crow.find("#subItemShortQty").val();
            Sub_ShortQuantity = parseFloat(Sub_ShortQuantity) + parseFloat(CheckNullNumber(subItemShortQty));
            var subItemSampleQty = Crow.find("#subItemSampleQty").val();
            Sub_SampleQuantity = parseFloat(Sub_SampleQuantity) + parseFloat(CheckNullNumber(subItemSampleQty));           
            var subItemQcsQty = Crow.find("#subItemQcsQty").val();
            Sub_subItemQcsQty = parseFloat(Sub_subItemQcsQty) + parseFloat(CheckNullNumber(subItemQcsQty));
            //debugger;
            //if (Src_Type == "RWK") {
            //    var avaiableStock = Crow.find('#subItemAvlQty').val();
            //    Sub_AvailQuantity = parseFloat(Sub_AvailQuantity) + parseFloat(CheckNullNumber(avaiableStock));
            //}
        });
        if (sub_item == "Y") {
            var qc_type = $("#qc_type").val();
            if (qc_type == "RWK" || qc_type == "PJO" || qc_type == "FGR" ) {
                if (item_PrdQty != Sub_Quantity) {
                    PPItemRow.find("#" + SubItemButton).css("border", "");
                }
            }
            else if (qc_type == "SMR") {
                if (item_PrdQty != Sub_Quantity) {
                    PPItemRow.find("#" + SubItemButton).css("border", "");
                }
            }
            else {
                if (qc_type != "PRD") {//add by shubham Maurya on 01-02-2025 for PRD
                    if (item_PrdQty != Sub_Quantity) {
                        flag = "Y";
                        PPItemRow.find("#" + SubItemButton).css("border", "1px solid red");
                    } else {
                        PPItemRow.find("#" + SubItemButton).css("border", "");
                    }
                }                
            }
            if (item_PrdAccQty != Sub_AccQuantity) {
                flag = "Y";
                PPItemRow.find("#" + "SubItemAccQty").css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + "SubItemAccQty").css("border", "");
            }
            if (item_PrdRejQty != Sub_RejQuantity) {
                flag = "Y";
                PPItemRow.find("#" + "SubItemRejQty").css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + "SubItemRejQty").css("border", "");
            }
            if (item_PrdRewQty != Sub_RewQuantity) {
                flag = "Y";
                PPItemRow.find("#" + "SubItemRewQty").css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + "SubItemRewQty").css("border", "");
            }
            if (item_PrdShortQty != Sub_ShortQuantity) {
                flag = "Y";
                PPItemRow.find("#" + "SubItemShortQty").css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + "SubItemShortQty").css("border", "");
            }
            if (item_PrdSampleQty != Sub_SampleQuantity) {
                flag = "Y";
                PPItemRow.find("#" + "SubItemSampleQty").css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + "SubItemSampleQty").css("border", "");
            }
            if (qc_type=="PRD") {
                if (item_PrdQcQty != Sub_subItemQcsQty) {
                    flag = "Y";
                    PPItemRow.find("#" + "SubItemQCQty").css("border", "1px solid red");
                } else {
                    PPItemRow.find("#" + "SubItemQCQty").css("border", "");
                }
            }   
        }
    });

    if (flag == "Y") {
        if (ShowMessage == "Y") {

            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }

        return false;
    }
    else {
        return true;
    }
}

function GetSubItemAvlStock(e) {
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#hdRowID").val();
    var ProductNm = Crow.find("#ItemName_" + rowNo + " option:selected").text();
    var ProductId = Crow.find("#ItemName_" + rowNo + " option:selected").val();
    var UOM = Crow.find("#UOM").val();
    var QCtype = $("#hdQCtype").val(); /*Add by Hina on 23-08-2024 only for random qc by shopfloor*/
    /*Add by Hina on 23-08-2024 only for random qc by shopfloor*/
    if (QCtype == "RQC") {
        var Location_type = $("#hdnLocation_type").val();
        if (Location_type == "WH") {
            var SourceWH = $("#SourceWH").val();
        }
        else {
            var SourceShopfloor = $("#SourceShfl").val();
        }
    }
    else {
        var SourceWH = $("#SourceWH").val();
    }
    var AvlStk = Crow.find("#AvailableStockInBase").val();
    //Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, SourceWH, AvlStk, "wh");
    if (QCtype == "RQC") {
        var Location_type = $("#hdnLocation_type").val();
        if (Location_type == "WH") {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/QualityInspection/GetSubItemWhAvlstockDetails",
                data: {
                    Wh_id: SourceWH,
                    Item_id: ProductId,
                    flag: "wh"
                },
                success: function (data) {
                    debugger;
                    $("#SubItemStockPopUp").html(data);
                    $("#Stk_Sub_ProductlName").val(ProductNm);
                    $("#Stk_Sub_ProductlId").val(ProductId);
                    $("#Stk_Sub_serialUOM").val(UOM);
                    $("#Stk_Sub_Quantity").val(AvlStk);
                }

            });
        }
        else {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/QualityInspection/GetSubItemShflAvlstockDetails",
                data: {
                    Shfl_id: SourceShopfloor,
                    Item_id: ProductId,
                    flag: "shfl"
                },
                success: function (data) {
                    debugger;
                    $("#SubItemStockPopUp").html(data);
                    $("#Stk_Sub_ProductlName").val(ProductNm);
                    $("#Stk_Sub_ProductlId").val(ProductId);
                    $("#Stk_Sub_serialUOM").val(UOM);
                    $("#Stk_Sub_Quantity").val(AvlStk);
                }

            });
        }
    }
    else {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/QualityInspection/GetSubItemWhAvlstockDetails",
            data: {
                Wh_id: SourceWH,
                Item_id: ProductId,
                flag: "wh"
            },
            success: function (data) {
                debugger;
                $("#SubItemStockPopUp").html(data);
                $("#Stk_Sub_ProductlName").val(ProductNm);
                $("#Stk_Sub_ProductlId").val(ProductId);
                $("#Stk_Sub_serialUOM").val(UOM);
                $("#Stk_Sub_Quantity").val(AvlStk);
            }

        });
    }
}
function ResetWorningBorderColor() {
    debugger;
    return QC_CheckValidations_forSubItems("QcItmDetailsTbl", "", "hdItemId", "RecivedQty", "SubItemQCQty", "N");
}
function QCSubItemList() {
    var NewArr = new Array();
    debugger;
    var qc_type = $("#qc_type").val();
    var QCSrc_Type = $("#hdQCtype").val();
    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        if (qc_type == "PRD") {
            var Qty = row.find("#subItemQcsQty").val();
        }
        else {
            var Qty = row.find('#subItemQty').val();
        }
        debugger;
        if (parseFloat(CheckNullNumber(Qty)) > 0) {
            //if (row.find('#subItemQty').val() != "" && row.find('#subItemQty').val() != "0") {
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();           
            List.accept_qty = row.find('#subItemAccQty').val();
            List.reject_qty = row.find('#subItemRejQty').val();
            List.rework_qty = row.find('#subItemRewQty').val();
            List.short_qty = CheckNullNumber(row.find('#subItemShortQty').val());
            List.sample_qty = CheckNullNumber(row.find('#subItemSampleQty').val());
            if (qc_type == "PRD") {
                //List.ProdQty = CheckNullNumber(row.find('#subItemProdQty').val());
                List.ProdQty = CheckNullNumber(row.find('#subItemOrdQty').val());
                List.PendQty = CheckNullNumber(row.find('#subItemPendQty').val());
                List.qty = row.find('#subItemQcsQty').val();
            }
            else {
                List.qty = row.find('#subItemQty').val();
            }
            debugger;
            if (QCSrc_Type == "RWK") {
                List.avaiableStock = row.find('#subItemAvlStk').val();
            }
            NewArr.push(List);
        }
    });
    return NewArr;
}
/***--------------------------------Sub Item Section End-----------------------------------------***/
/****======------------Reason For Reject And Rework section Start-------------======****/
function onClickReasonRemarks(e, flag) {
    debugger;

    if (flag == "reject") {

        $("#RR_Remarks_Dropdown").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-RR_Remarks_Dropdown-container']").css("border-color", "#ced4da");
        $("#DropDown_vmRR_Remarks").css("display", "none");
        $("#DropDown_vmRR_Remarks").text("");

        $("#RR_Remarks").css("border-color", "#ced4da");
        $("#vmRR_Remarks").css("display", "none");
        $("#vmRR_Remarks").text("");
    }
    else {
        $("#RR_Remarks").css("border-color", "#ced4da");
        $("#vmRR_Remarks").css("display", "none");
        $("#vmRR_Remarks").text("");
    }


    let Row = $(e.target).closest('tr');
    let ItemName = Row.find("#ItemName").val();
    let ItemId = Row.find("#hdItemId").val();
    let UOM = Row.find("#UOM").val();
    let ReasonRemarks = "";
    let IsDisabled = $("#DisableSubItem").val();
    if (IsDisabled == "Y") {
        $("#RR_Remarks").attr("readonly", true);
        $("#RR_Remarks_Dropdown").attr("disabled", true);
        $("#RR_btnClose").attr("hidden", false);
        $("#RR_btnSaveAndExit,#RR_btnDiscardAndExit").attr("hidden", true);

    } else {
        $("#RR_Remarks").attr("readonly", false);
        $("#RR_Remarks_Dropdown").attr("disabled", false);
        $("#RR_btnClose").attr("hidden", true);
        $("#RR_btnSaveAndExit,#RR_btnDiscardAndExit").attr("hidden", false);
    }
    if (flag == "reject") {
        ReasonRemarks = Row.find("#ReasonForReject").val();
        $("#Div_DropdownReason").show();
        $("#reasonrequired").show();
        $("reasonrequired").addClass("required");
        let rejQty = Row.find("#RejectQty").val();
        if (parseFloat(CheckNullNumber(rejQty)) > 0) {
            $("#RR_Quantity").val(rejQty);

            var ReasonForReject_ID = Row.find("#ReasonForReject_ID").val();
            var ReasonForReject_Name = Row.find("#ReasonForReject_Name").val().trim();
            if (ReasonForReject_ID != "0" && ReasonForReject_ID != "" && ReasonForReject_ID != null) {
                $("#RR_Remarks_Dropdown").append(`<option value="${ReasonForReject_ID}"> ${ReasonForReject_Name}</option>`)

                $("#RR_Remarks_Dropdown").val(ReasonForReject_ID).trigger('change')
            }
            else {
                $("#RR_Remarks_Dropdown").val(0).trigger('change')
                $("#RR_Remarks_Dropdown").css("border-color", "#ced4da");
                $("[aria-labelledby='select2-RR_Remarks_Dropdown-container']").css("border-color", "#ced4da");
                $("#DropDown_vmRR_Remarks").css("display", "none");
                $("#DropDown_vmRR_Remarks").text("");
            }
          
        }
        else {
            Row.find("#ReasonForReject").val("");
            ReasonRemarks = "";
        }
        var abc = $("#reasonrequired").text();
       //abc= abc.css("border-color", "red");
        $("#RR_ReasonRemarksLabel_1").text($("#span_ReasonForRejection").text() + abc);
        $("#RR_ReasonRemarksLabel").text($("#span_ReasonForRejection_Remarks").text());
    }
    else if (flag == "rework") {
        ReasonRemarks = Row.find("#ReasonForRework").val();
        $("#Div_DropdownReason").hide();
        let rwkQty = Row.find("#ReworkableQty").val();
        if (parseFloat(CheckNullNumber(rwkQty)) > 0) {
            $("#RR_Quantity").val(rwkQty);
        } else {
            Row.find("#ReasonForRework").val("");
            ReasonRemarks = "";
        }
        $("#RR_ReasonRemarksLabel").text($("#span_Reworkremarks").text());
    }
    else if (flag == "Accept") {
        ReasonRemarks = Row.find("#ReasonForAccecpt").val();
        $("#Div_DropdownReason").hide();
        let AcceptQty = Row.find("#AcceptQty").val();
        if (parseFloat(CheckNullNumber(AcceptQty)) > 0) {
            $("#RR_Quantity").val(AcceptQty);
        } else {
            Row.find("#ReasonForAccecpt").val("");
            ReasonRemarks = "";
        }
        $("#RR_ReasonRemarksLabel").text($("#span_ReasonForAccept_Remarks").text());
    }
    $("#RR_ItemName").val(ItemName);        
    $("#RR_ItemId").val(ItemId);        
    $("#RR_Uom").val(UOM);
    $("#RR_Flag").val(flag);

    $("#RR_Remarks").val(ReasonRemarks);
   /// BindRejectionReasonBind();
}
function onchangeReason() {
    debugger;
    var RR_RemarksID = $("#RR_Remarks_Dropdown").val();
    var RR_Remarks_text = $("#RR_Remarks_Dropdown option:selected").text().trim();
    if (RR_RemarksID == "0" || RR_RemarksID == "") {
        CheckVallidation("RR_Remarks_Dropdown", "DropDown_vmRR_Remarks");
    }
    else {
        CheckVallidation("RR_Remarks_Dropdown", "DropDown_vmRR_Remarks");
   
        $("#RR_Remarks").text(RR_Remarks_text);      
        $("#hdn_reasonRemarks_Name").val(RR_Remarks_text);
        $("#RR_Remarks").val(RR_Remarks_text);
        $("#hdnreasonRemarksID").val(RR_RemarksID);
       
    }
}
function BindRejectionReasonBind() {
    //$("#RR_Remarks_Dropdown").empty();
    //$("#RR_Remarks_Dropdown").append(`<option value="0">---Select---</option>`);
    debugger;
    $("#RR_Remarks_Dropdown").html('').append(`<option value="0">---Select---</option>`);

    debugger
    $('#RR_Remarks_Dropdown').select2({
       // allowClear: true,
        ajax: {        
            url: "/ApplicationLayer/QualityInspection/GetRejectionReason",  // Fetch data from Laravel Controller
            type: "post",
            dataType: "json",        
            data: function (params) {
                return {
                    SearchName: params.term,  // User input text
                    page: params.page || 1
                };
            },
            processResults: function (data, params) {
                var pageSize = 20;  // Set pagination limit
                var page = params.page || 1;
                data = JSON.parse(data);
                // Ensure `data` is an array before slicing
                var paginatedData = data.slice((page - 1) * pageSize, page * pageSize); 
                if (page == 1) {
                    if (paginatedData[0] != null) {
                        if (paginatedData[0].rej_reason.trim() != "---Select---") {
                            var select = { rej_id: "0", rej_reason: " ---Select---" };
                            paginatedData.unshift(select);
                        }
                    }
                }
                debugger;
                return {
                    results: $.map(paginatedData, function (item) {
                        debugger;
                        return { id: item.rej_id, text: item.rej_reason };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length  // Check if more pages exist
                    }
                };
            },
           
            cache: true
        },
        templateResult: function (data) {
            debugger;
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
               /* '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.id + '</div>' +*/
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
   // $("#RR_Remarks_Dropdown").val(0).trigger("change");
   
}
function onClick_RR_SaveAndExit() {
    debugger;
    var ErrorFlag = "N";
    let ItemId = $("#RR_ItemId").val();
   let RR_Remarks = $("#RR_Remarks").val();
    let RR_Remarks_ID = $("#hdnreasonRemarksID").val();
    let reason_rej_ID = "";
    let RR_Remarks_Name = $("#hdn_reasonRemarks_Name").val();
    let RR_Flag = $("#RR_Flag").val();
    let RR_HidenFieldId = "";
    if (RR_Flag == "rework") {
        RR_HidenFieldId = "ReasonForRework";
        //RR_HidenField_ID = "ReasonForRework_ID";
        //RR_HidenField_Name = "ReasonForRework_Name";
    } else if (RR_Flag == "reject") {
        RR_HidenFieldId = "ReasonForReject";
        RR_HidenField_ID = "ReasonForReject_ID";
        RR_HidenField_Name = "ReasonForReject_Name";
    }
    else if (RR_Flag == "Accept") {
        RR_HidenFieldId = "ReasonForAccecpt";
       
    }
   

    if (RR_Remarks.trim() == "" && parseFloat(CheckNullNumber($("#RR_Quantity").val())) > 0) {
        $("#RR_btnSaveAndExit").attr("data-dismiss", "");
        if (RR_Flag == "reject") {           
            $("[aria-labelledby='select2-RR_Remarks_Dropdown-container']").css("border-color", "red");
            $("#RR_Remarks_Dropdown").css("border-color", "red");
            $("#DropDown_vmRR_Remarks").css("display", "block");
            $("#DropDown_vmRR_Remarks").text($("#valueReq").text());
            ErrorFlag = "Y";
        }
        if (RR_Flag == "Accept") {
            
            $("#RR_Remarks_Dropdown").css("border-color", "#ced4da");
            $("#DropDown_vmRR_Remarks").css("display", "none");
            $("#DropDown_vmRR_Remarks").text("");
            $("#RR_btnSaveAndExit").attr("data-dismiss", "modal");
        }
        else {
             $("#RR_Remarks").css("border-color", "red");          
            $("#vmRR_Remarks").css("display", "block");
            $("#vmRR_Remarks").text($("#valueReq").text());
            ErrorFlag = "Y";
        }
      
    }
    else {
        reason_rej_ID = $("#RR_Remarks_Dropdown").val();
      
        if (RR_Flag == "reject") {
            if (reason_rej_ID == "0" || reason_rej_ID == "" || reason_rej_ID == null) {
                $("#RR_btnSaveAndExit").attr("data-dismiss", "");
                $("[aria-labelledby='select2-RR_Remarks_Dropdown-container']").css("border-color", "red");
                $("#RR_Remarks_Dropdown").css("border-color", "red");
                $("#DropDown_vmRR_Remarks").css("display", "block");
                $("#DropDown_vmRR_Remarks").text($("#valueReq").text());
                ErrorFlag = "Y";
            }
            else {
                $("#RR_Remarks_Dropdown").css("border-color", "#ced4da");
                $("[aria-labelledby='select2-RR_Remarks_Dropdown-container']").css("border-color", "#ced4da");
                $("#DropDown_vmRR_Remarks").css("display", "none");
                $("#DropDown_vmRR_Remarks").text("");
                $("#RR_btnSaveAndExit").attr("data-dismiss", "modal");
            }
            
        }
        else {
            $("#RR_btnSaveAndExit").attr("data-dismiss", "modal");
            $("#RR_Remarks").css("border-color", "#ced4da");           
            $("#vmRR_Remarks").css("display", "none");
            $("#vmRR_Remarks").text("");
        }
      
       
    }
    if (RR_HidenFieldId != "" && ErrorFlag == "N") {
        let row = $("#QcItmDetailsTbl tbody tr #hdItemId[value = '" + ItemId + "']").closest('tr');
        row.find("#" + RR_HidenFieldId).val(RR_Remarks);
        if (RR_Flag == "reject") {
            row.find("#" + RR_HidenField_ID).val(RR_Remarks_ID);
            row.find("#" + RR_HidenField_Name).val(RR_Remarks_Name);
        }
        row.find("#Btn" + RR_HidenFieldId).css("border", "none");
    }

}
function checkValidationRR_remarks() {
   var flag= $("#RR_Flag").val()
    if (flag == "Accept") {

    }
    else {
        CheckVallidation("RR_Remarks", "vmRR_Remarks");
    }
   
}
/****======------------Reason For Reject And Rework section End-------------======****/

function approveonclick() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
    else {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#hdnsavebtn").val("AllreadyclickApprove");
    }

}
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "QcItmDetailsTbl", [{ "FieldId": "ItemName", "FieldType": "input" }]);
}






