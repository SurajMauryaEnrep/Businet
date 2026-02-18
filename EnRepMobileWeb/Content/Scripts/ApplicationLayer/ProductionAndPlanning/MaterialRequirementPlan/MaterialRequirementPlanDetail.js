//Modified by Suraj Maurya on 18-11-2024 Flag Name changed OrdrToPrecurQuantity1->OrdrToProduceQuantity
let CountLoad=0;
let TotalLoad = 0;
$(document).ready(function () {
    debugger;
    ResetSessionStorage();
    if ($("#ddl_src_type").val() == "D" || $("#ddl_src_type").val() == "A") {
        $("#ProductItemDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var sno = currentRow.find("#hfsno").val();

            BindProductItmList(sno, "bdind");
        });
        //BindProductItmList(1, "");
    }
    $('#ProductItemDetailsTbl tbody').on('click', '.deleteIcon', function () {
        debugger;
        // Getting all the rows next to the row 
        // containing the clicked button 
        var child = $(this).closest('tr').nextAll();
        // Iterating across all the rows  
        // obtained to change the index 
        child.each(function () {
            // Getting <tr> id. 
            var id = $(this).attr('id');
            // Getting the <p> inside the .row-index class. 
            var idx = $(this).children('.row-index').children('p');
            // Gets the row number from <tr> id. 
            var dig = parseInt(id.substring(1));
            // Modifying row index. 
            idx.html(`Row ${dig - 1}`);
            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });
        // Removing the current row. 
        $(this).closest('tr').remove();
        //debugger;
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#hfproductID").val();
        
        DeleteMaterialAndInputItemDetails(ItemCode);
        //ResetSerialNo();
    });
    //HideForecastfields();
    HideShowAddProPlanBtn();
    $("#hdDoc_No").val($("#txt_MRPNumber").val());
   
    if ($("#hd_Status").val() == "" || $("#hd_Status").val() == null) {
        $("#BtnAddItem").closest(".plus_icon1").css("display", "none");
        $("#ProductItemDetailsTbl tbody tr select ,#ItmInfoBtnIcon ,#StockDetail ,#PlannedQuantity ,#PlannedQuantityDetail ,#remarks").attr("disabled", true);
    }
    $("#MRP_MaterialDetailTbl >tbody").bind("click", function (e) {
            var clickedrow = $(e.target).closest("tr");
        $("#MRP_MaterialDetailTbl >tbody >tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");

    });    
    
});
const QtyDecDigit = $("#QtyDigit").text();///Quantity
const span_StockDetail = $("#span_StockDetail").text();
const span_ProductionDetail = $("#span_ProductionDetail").text();

function ResetSessionStorage() {
    var hdnSfData = $("#hdnSFMaterialDetail").val();
    $("#hdnSFMaterialDetail").val("");
    var hdnRmData = $("#hdnInputMaterialDetail").val();
    $("#hdnInputMaterialDetail").val("");
    if (hdnSfData != null && hdnSfData != "") {
        sessionStorage.setItem("ArrSfData_forCalculation", hdnSfData);
    } else {
        sessionStorage.removeItem("ArrSfData_forCalculation");
    }
    if (hdnRmData != null && hdnRmData != "") {
        sessionStorage.setItem("ArrRmData_forCalculation", hdnRmData);
    } else {
        sessionStorage.removeItem("ArrRmData_forCalculation");
    }
}
function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var PQ_No = clickedrow.children("#mrp_no").text();
    var PQ_Date = clickedrow.children("#mrp_date").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(PQ_No);
    GetWorkFlowDetails(PQ_No, PQ_Date, Doc_id);
    var a = 1;
}
function ProcurementCompletionDateReplicate() {
    debugger;
    var date = $("#ProcurementCompletionDt").val();
    $("#SFMaterialDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Qty = currentRow.find("#OrderToProcureQuantity").val();
        if (parseFloat(CheckNullNumber(Qty)) > 0) {
            currentRow.find("#ProcurementCompletionDate").val(date);
            currentRow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
            currentRow.find("#ProcurementCompletionDateError").text("");
            currentRow.find("#ProcurementCompletionDateError").css("display", "none");
        }
    });
}
function ProductionCompletionDateReplicate() {
    debugger;
    var date = $("#ProductionCompletionDt").val();
    $("#SFMaterialDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Qty = currentRow.find("#OrderToProduceQuantity").val();
        if (parseFloat(CheckNullNumber(Qty)) > 0) {
            currentRow.find("#ProductionCompletionDate").val(date);
            currentRow.find("#ProductionCompletionDate").css("border-color", "#ced4da");
            currentRow.find("#ProductionCompletionDateError").text("");
            currentRow.find("#ProductionCompletionDateError").css("display", "none");
        }
    });
}
function ProductionCompletionInputDateReplicate() {
    debugger;
    var date = $("#ProcurementInputCompletionDate").val();
    $("#InputMaterialDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Qty = currentRow.find("#RequisitionQuantity").val();
        if (parseFloat(CheckNullNumber(Qty)) > 0) {
            currentRow.find("#ProcurementCompletionDate").val(date);
            currentRow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
            currentRow.find("#ProcurementCompletionDateError").text("");
            currentRow.find("#ProcurementCompletionDateError").css("display", "none");
        }
    });
}
function ForwardBtnClick() {
    debugger;
    //var DocStatus = "";
    //DocStatus = $('#hd_Status').val().trim();
    //if (DocStatus === "D" || DocStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");
    //    }
    //    var Doc_ID = $("#DocumentMenuId").val();
    //    $("#OKBtn_FW").attr("data-dismiss", "modal");

    //    Cmn_GetForwarderList(Doc_ID);

    //}
    //else {
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var MRPDate = $("#txt_MRPDate").val();
    $.ajax({
        type: "POST",
        /*  url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: MRPDate
        },
        success: function (data) {
            /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var DocStatus = "";
                DocStatus = $('#hd_Status').val().trim();
                if (DocStatus === "D" || DocStatus === "F") {

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
                /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
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
    var MRPNo = "";
    var MRPDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";
    var mailerror = "";

    Remarks = $("#fw_remarks").val(); 
    MRPNo = $("#txt_MRPNumber").val();
    MRPDate = $("#txt_MRPDate").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (MRPNo + ',' + WF_Status1);
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "MaterialRequirementPlan_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(MRPNo, MRPDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/MaterialRequirementPlan/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && MRPNo != "" && MRPDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(MRPNo, MRPDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/MaterialRequirementPlan/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
        
    }
    if (fwchkval === "Approve") {
        $("#A_Status").val("Approve");
        $("#A_Level").val($("#hd_currlevel").val());
        $("#A_Remarks").val(Remarks);
        $("#HdActionCommand").val("Approve");
        if (CheckFormValidation() == true) {
            $("form").submit();
        }
        
        //var list = [{ MRPNo: MRPNo, MRPDate: MRPDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        //var AppDtList = JSON.stringify(list);

        //window.location.href = "/ApplicationLayer/MaterialRequirementPlan/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && MRPNo != "" && MRPDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(MRPNo, MRPDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/MaterialRequirementPlan/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
        
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && MRPNo != "" && MRPDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(MRPNo, MRPDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/MaterialRequirementPlan/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
        
    }
}
//function GetPdfFilePathToSendonEmailAlert(poNo, poDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/MaterialRequirementPlan/SavePdfDocToSendOnEmailAlert",
//        data: { poNo: poNo, poDate: poDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function OtherFunctions(StatusC, StatusName) {

}
function OnChangeAHToDate() {
    CheckVallidation("AH_txtToDate", "vm_AH_txtToDate");
    var Todt = $("#AH_txtToDate").val();
    $("#AH_hdn_ToDate").val(Todt);
    CheckAdHocDateRange();
}
function CheckAdHocDateRange() {
    var fromdt = $("#AH_txtFromDate").val();
    var todt = $("#AH_txtToDate").val();
    if (fromdt != "" && todt != "") {
        let fromdt1 = new Date(fromdt).getTime();
        let todt1 = new Date(todt).getTime();
        if (fromdt1 > todt1) {
            $("#vm_AH_txtToDate").text($("#JC_InvalidDate").text());
            $("#vm_AH_txtToDate").css("display", "block");
            $("#AH_txtToDate").css("border-color", "red");
            return false;
        } else {
            $("#vm_AH_txtToDate").text("");
            $("#vm_AH_txtToDate").css("display", "none");
            $("#AH_txtToDate").css("border-color", "#ced4da");
            return true;
        }
    }

}
function OnChangeAHFromdate() {
    CheckVallidation("AH_txtFromDate", "vm_AH_txtFromDate")
    var fromdt = $("#AH_txtFromDate").val();
    $("#AH_hdn_FromDate").val(fromdt);
    CheckAdHocDateRange();
}
function OnClickAdHocToDate() {
    debugger;
    var fromdt = $("#AH_txtFromDate").val();
    if (fromdt != "" && fromdt != null) {
        $("#AH_txtToDate").attr("min", fromdt);
    }
}

function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#txt_MRPNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function AddPRoductionPlanItemDetail() {
    debugger;
    var F_Fy = $("#ddl_financial_year").val();
    var F_Period = $("#ddl_period").val();
    var FromDate = $("#hdn_FromDate").val();
    var ToDate = $("#hdn_ToDate").val();
    var P_Number = "";
    var P_Date = "";
    var ErrorFlag = "N";
    var ddl_src_type = $("#ddl_src_type").val();
    if (ddl_src_type== "P") {
        var PP_Number = $("#PP_Number").val();
        if (PP_Number != "0") {
            P_Number = PP_Number.split("|")[0];
            P_Date = PP_Number.split("|")[1];
        }
        
    } else {
        var val_ffy = CheckVallidation("ddl_financial_year", "vm_ddl_financial_year");
        var val_fPeriod = CheckVallidation("ddl_period", "vm_ddl_period");
        if (val_ffy == false || val_fPeriod == false) {
            ErrorFlag = "Y";
        }
    }
    let startTime = moment();
    let duration;
    if (ErrorFlag == "N") {
        debugger;
        if (ddl_src_type == "P") {
            showLoader();
            try {
                $.ajax({
                    type: "POST",
                    url: "/ApplicationLayer/MaterialRequirementPlan/AddPRoductionPlanItemDetail",
                    data: {
                        F_Fy: F_Fy,
                        F_Period: F_Period,
                        FromDate: FromDate,
                        ToDate: ToDate,
                        P_Number: P_Number,
                        P_Date: P_Date
                    },
                    success: function (data) {
                        debugger;
                        try {
                            let responceTime = moment();
                            duration = moment.duration(responceTime.diff(startTime));
                            console.log(duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');

                            var arr = JSON.parse(data);
                            if (arr != null) {
                                debugger;

                                if (arr.Table.length > 0) {
                                    $("#ddl_src_type").attr("disabled", true);
                                    $("#ddl_financial_year").attr("disabled", true);
                                    $("#ddl_period").attr("disabled", true);
                                    //$("#ddlRequiredArea").attr('disabled', true);
                                    $("#PP_Number").attr('disabled', true);
                                    var QtyDigit = $("#QtyDigit").text();
                                    for (var i = 0; i < arr.Table.length; i++) {
                                        AddnewRowOnly(i + 1);
                                        HideShowAddProPlanBtn();
                                        //AddNewProduct_OnClick();
                                        $("#ProductItemDetailsTbl >tbody >tr").each(function (i, row) {
                                            debugger
                                            var currentRow = $(this);
                                            if (currentRow.find("#ProductName" + (i + 1)).val() == null) {
                                                HideShowPageWise(arr.Table[i].sub_item, currentRow);
                                                currentRow.find("#ProductName" + (i + 1)).append(`
                                        <option value="${arr.Table[i].item_id}">${arr.Table[i].item_name}</option>`).attr("disabled", true);
                                                currentRow.find("#hfproductID").val(arr.Table[i].item_id);
                                                currentRow.find("#UOM").val(arr.Table[i].uom_alias);
                                                currentRow.find("#UOMID").val(arr.Table[i].uom_id);
                                                //currentRow.find("#ForecastQuantity").val(parseFloat(arr.Table[i].tgt_qty).toFixed(QtyDigit));
                                                currentRow.find("#AvailableStockInBase").val(parseFloat(arr.Table[i].Avlstock).toFixed(QtyDigit));
                                                currentRow.find("#PlannedQuantity").val(parseFloat(arr.Table[i].plan_qty).toFixed(QtyDigit)).attr("disabled", true);//s.trigger("change");
                                                //currentRow.find("#remarks").val(arr.Table[i].plan_qty);

                                            }


                                        });
                                    }

                                    for (var i = 0; i < arr.Table1.length; i++) {
                                        $("#hdn_Sub_ItemDetailTbl tbody").append(
                                            `<tr>
                                                <td><input type="text" id="ItemId" value='${arr.Table1[i].item_id}'></td>
                                                <td><input type="text" id="subItemId" value='${arr.Table1[i].sub_item_id}'></td>
                                                <td><input type="text" id="subItemQty" value='${arr.Table1[i].Qty}'></td>
                                                <td><input type="text" id="ItemType" value='${arr.Table1[i].ItemType}'></td>
                                            </tr>`)
                                    }
                                    $("#DisableSubItem").val("Y");
                                    $(".deleteIcon").css("display", "none");
                                    $("#AddBtnIconMRP").css("display", "none");
                                    AddSFAndRM_ItemDetails(data)//To Add SF And RM Details

                                    let completeTime = moment();
                                    duration = moment.duration(completeTime.diff(startTime));
                                    console.log(duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');
                                    //hideLoader();
                                }
                            }
                        }
                        catch (ex) {
                            hideLoader();
                            console.log(ex);
                        }
                        
                    }
                });
            }
            catch (ex) {
                hideLoader();
                console.log(ex);
            }
           
        }
        else {
            showLoader();
            try {
                $.ajax({
                    type: "POST",
                    url: "/ApplicationLayer/ProductionPlan/AddForeCastItemDetail",
                    data: {
                        F_Fy: F_Fy,
                        F_Period: F_Period,
                        FromDate: FromDate,
                        ToDate: ToDate
                    },
                    success: function (data) {
                        debugger;
                        try {
                            var arr = JSON.parse(data);
                            if (arr != null) {
                                if (arr.Table.length > 0) {
                                    $("#ddl_src_type").attr("disabled", true);
                                    $("#ddl_financial_year").attr("disabled", true);
                                    $("#ddl_period").attr("disabled", true);
                                    //$("#ddlRequiredArea").attr('disabled', true);
                                    $("#PP_Number").attr('disabled', true);

                                    var QtyDigit = $("#QtyDigit").text();
                                    for (var i = 0; i < arr.Table.length; i++) {
                                        var BOM_Avl = arr.Table[i].BOM_Avl;
                                        AddnewRowOnly((i + 1),BOM_Avl);
                                        HideShowAddProPlanBtn()
                                        //AddNewProduct_OnClick();
                                        $("#ProductItemDetailsTbl >tbody >tr").each(function (i, row) {
                                            debugger
                                            var currentRow = $(this);
                                            if (currentRow.find("#ProductName" + (i + 1)).val() == null) {
                                                HideShowPageWise(arr.Table[i].sub_item, currentRow);
                                                currentRow.find("#ProductName" + (i + 1)).append(`
                                        <option value="${arr.Table[i].item_id}">${arr.Table[i].item_name}</option>`).attr("disabled", true);
                                                currentRow.find("#hfproductID").val(arr.Table[i].item_id);
                                                currentRow.find("#UOM").val(arr.Table[i].uom_alias);
                                                currentRow.find("#UOMID").val(arr.Table[i].uom_id);
                                                currentRow.find("#tdforecastqty").css("display", "");
                                                currentRow.find("#ForecastQuantity").val(parseFloat(arr.Table[i].tgt_qty).toFixed(QtyDigit));
                                                currentRow.find("#AvailableStockInBase").val(parseFloat(arr.Table[i].Avlstock).toFixed(QtyDigit));
                                                var PlanQty = parseFloat(arr.Table[i].tgt_qty) - parseFloat(arr.Table[i].Avlstock);
                                                PlanQty = PlanQty > 0 ? PlanQty : 0;
                                                currentRow.find("#PlannedQuantity").val(parseFloat(PlanQty).toFixed(QtyDigit)).attr("disabled", false);//.trigger("change");
                                                //currentRow.find("#remarks").val(arr.Table[i].plan_qty);
                                            }
                                        });
                                    }
                                    $(".deleteIcon").css("display", "none");
                                    $("#AddBtnIconMRP").css("display", "none");

                                    AddSFAndRM_ItemDetails(data)//To Add SF And RM Details
                                    let completeTime = moment();
                                    duration = moment.duration(completeTime.diff(startTime));
                                    console.log(duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');

                                }
                            }
                        }
                        catch (ex) {
                            console.log(ex);
                            hideLoader();
                        }
                    }

                });
            }
            catch (ex) {
                console.log(ex);
                hideLoader();
            }
            
        }   
    }
}
function ddl_src_type_onchange() {
    debugger;
    HideShowAddProPlanBtn();
    var srcval = $("#ddl_src_type").val();
    $("#ddl_financial_year").val(0);//.trigger('change');
    $("#ddl_period").val(0).trigger('change');
    $("#vm_ddl_financial_year").css("display", "none");
    $("#ddl_financial_year").css("border-color", "#ced4da");
    $("#vm_ddl_period").css("display", "none");
    $("#ddl_period").css("border-color", "#ced4da");
    if (srcval === "P") {
        $("#div_ppNo").attr("hidden", false);
        $("#div_ppDt").attr("hidden", false);
        $("#AdHocdiv_ppDtRg").css("display", "none");
        $("#div_ppfy").css("display", "none");
        $("#P_div_ppDtRg").css("display", "");
        $("#div_ppPeriod").css("display", "none");
        $("#div_ppDtRg").css("display", "none");

        $("#ddl_financial_year").attr("disabled", true);
        $("#ddl_period").attr("disabled", true);
        $("#thforecast").css("display", "none");
        $("#thproduced").css("display", "none");
        BindPPNumberList();
        $("#ProductItemDetailsTbl tbody tr").remove();
        $("#BtnAddItem").closest("div .plus_icon1").css("display", "none");
        $("#BtnAddItem").closest(".plus_icon1").css("display", "none");
    }
    else if (srcval === "D") {
        $("#div_ppfy").css("display", "block");
        $("#div_ppPeriod").css("display", "block");
        $("#div_ppDtRg").css("display", "block");
        $("#P_div_ppDtRg").css("display", "");
        $("#AdHocdiv_ppDtRg").css("display", "none");
        $("#div_ppNo").attr("hidden", true);
        $("#div_ppDt").attr("hidden", true);
        $("#ddl_financial_year").attr("disabled", false);
        $("#ddl_period").attr("disabled", false);
        $("#thforecast").css("display", "none");
        $("#thproduced").css("display", "");
        $("#BtnAddItem").closest(".plus_icon1").css("display", "none");
        // $("#BtnAddItem").closest("div .plus_icon1").css("display", "");
        $("#ProductItemDetailsTbl tbody tr").remove();
        AddNewProduct_OnClick();
        $("#ProductItemDetailsTbl tbody tr select ,#ItmInfoBtnIcon ,#StockDetail ,#PlannedQuantity ,#PlannedQuantityDetail ,#remarks").attr("disabled", true);
    }
    else if (srcval === "A") {
        $("#div_ppfy").css("display", "none");
        $("#div_ppPeriod").css("display", "none");
        $("#P_div_ppDtRg").css("display", "none");
        $("#AdHocdiv_ppDtRg").css("display", "block");
        $("#div_ppNo").attr("hidden", true);
        $("#div_ppDt").attr("hidden", true);
        $("#ddl_financial_year").attr("disabled", false);
        $("#ddl_period").attr("disabled", false);
        $("#thforecast").css("display", "none");
        $("#thproduced").css("display", "");
        $("#BtnAddItem").closest(".plus_icon1").css("display", "");
        // $("#BtnAddItem").closest("div .plus_icon1").css("display", "");
        $("#ProductItemDetailsTbl tbody tr").remove();
        AddNewProduct_OnClick();
        $("#ProductItemDetailsTbl tbody tr select ,#ItmInfoBtnIcon ,#StockDetail ,#PlannedQuantity ,#PlannedQuantityDetail ,#remarks").attr("disabled", false);
    }
    else {
        $("#div_ppfy").css("display", "block");
        $("#div_ppPeriod").css("display", "block");
        $("#div_ppDtRg").css("display", "block");
        $("#P_div_ppDtRg").css("display", "");
        $("#AdHocdiv_ppDtRg").css("display", "none");
        $("#div_ppNo").attr("hidden", true);
        $("#div_ppDt").attr("hidden", true);
        $("#ddl_financial_year").attr("disabled", false);
        $("#ddl_period").attr("disabled", false);
        $("#AddBtnIconMRP").css("display", "block");
        $("#ProductItemDetailsTbl tbody tr").remove();
        $("#thforecast").css("display", "");
        $("#thproduced").css("display", "");
        $("#BtnAddItem").closest(".plus_icon1").css("display", "none");
        //AddNewProduct_OnClick();
        $("#ProductItemDetailsTbl tbody tr select ,#ItmInfoBtnIcon ,#StockDetail ,#PlannedQuantity ,#PlannedQuantityDetail ,#remarks").attr("disabled", true);
    }

};

function ResetSerialNo() {
    var SoNo = 0;
    $("#ProductItemDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        SoNo = parseInt(SoNo) + 1;
        currentRow.find("#sno").text(SoNo);
    });
    SoNo = 0;
    $("#SFMaterialDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        SoNo = parseInt(SoNo) + 1;
        currentRow.find("td:eq(0)").text(SoNo);
    });
    SoNo = 0;
    $("#InputMaterialDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        SoNo = parseInt(SoNo) + 1;
        currentRow.find("td:eq(0)").text(SoNo);
    });

}

function HideShowAddProPlanBtn() {
    var srcval = $("#ddl_src_type").val();
    if (srcval === "P") {
        $("#AddBtnIconMRP").removeAttr("style");

        //ShowForecastfields();
    }
    else {
        $("#AddBtnIconMRP").css("display", "none");
        //HideForecastfields();
    }

    $("#hdn_ddl_src_type").val(srcval);
}
function ddl_financial_year_onchange() {
    debugger;
    var ddl_f_frequency = $("#ddl_financial_year").val();
    if (ddl_f_frequency != "0") {
        $("#vm_ddl_financial_year").css("display", "none");
        $("#ddl_financial_year").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_financial_year').text($("#valueReq").text());
        $("#vm_ddl_financial_year").css("display", "block");
        $("#ddl_financial_year").css("border-color", "red");
    }
    var ddl_financial_year = $("#ddl_financial_year").val();
    $("#hdn_ddl_financial_year").val(ddl_financial_year);
    var financial_year = $("#hdn_ddl_financial_year").val();
    $("#txtFromDate").val('');
    $("#txtToDate").val('');
    $('#hdn_FromDate').val('');
    $('#hdn_ToDate').val('');
    var Flag = $("#ddl_src_type").val();
    if (Flag == "P") {
        Flag = "PP";
    }
    if (Flag == "D") {
        Flag = "MRP_D";
    }
    if (Flag == "F") {
        Flag = "MRP_SF";
    }
    if (ddl_f_frequency != "0") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialRequirementPlan/BindPeriod",
            data: {
                financial_year: financial_year,
                Flag: Flag
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    $('#ddl_period').empty();
                    arr = JSON.parse(data);
                    if (arr.Table1.length > 0) {
                        $('#ddl_period').append(`<option value=0>---Select---</option>`);
                        for (var i = 0; i < arr.Table1.length; i++) {
                            $('#ddl_period').append(`<option value="${arr.Table1[i].id}">${arr.Table1[i].name}</option>`);
                        }
                    }
                    else {
                        $('#ddl_period').append(`<option value=0>---Select---</option>`);
                    }
                    if ($("#PP_Number").val()!="0") {
                        SetPeriodOnchangePPNo();
                    }
                }
            },
        });
    }
};
function onChangeRequiredArea() {
    debugger;
    var RequisitionArea = $('#ddlRequiredArea').val();
    if (RequisitionArea != "0") {
        document.getElementById("vmRequiredArea").innerHTML = null;
        $("#ddlRequiredArea").css('border-color', '#ced4da');
        $("#hdRequiredArea").val(RequisitionArea);
        BindPPNumberList();
    }
 

}
function ddl_period_onchange(e) {
    debugger;
    var ddl_financial_year = $("#ddl_financial_year").val();
    $("#hdn_ddl_financial_year").val(ddl_financial_year);
    var financial_year = $("#hdn_ddl_financial_year").val();
     
    var ddl_period = $("#ddl_period").val();
    if (ddl_period == "0" || ddl_period == null) {
        $("#vm_ddl_period").text($("#valueReq").text());
        $("#vm_ddl_period").css("display", "block");
        $("#ddl_period").css("border-color", "red");
        ValidationFlag = false;
    }
    else {
        $("#vm_ddl_period").css("display", "none");
        $("#ddl_period").css("border-color", "#ced4da");
    }
    $("#hdn_ddl_period").val(ddl_period);
    var period = $("#hdn_ddl_period").val();
    resetitemdetail();
    if (ddl_period != "0") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialRequirementPlan/BindDateRange",
            data: {
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
                    $('#hdn_FromDate').val('');
                    $('#hdn_ToDate').val('');
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#txtFromDate').val(arr.Table[i].StartDT);
                            $('#txtToDate').val(arr.Table[i].EndDT);

                            $('#hdn_FromDate').val(arr.Table[i].StartDTyf);
                            $('#hdn_ToDate').val(arr.Table[i].EndDTyf);
                        }
                    }
                    if ($("#ddl_src_type").val()=="D")
                    $("#BtnAddItem").closest(".plus_icon1").css("display", "");

                    $("#ProductItemDetailsTbl tbody tr select ,#ItmInfoBtnIcon ,#StockDetail ,#PlannedQuantity ,#PlannedQuantityDetail ,#remarks").attr("disabled", false);
                }
                else {
                    $('#txtFromDate').val('');
                    $('#txtToDate').val('');

                    $('#hdn_FromDate').val('');
                    $('#hdn_ToDate').val('');
                }
            },
        });
    }
};
function resetitemdetail() {
    $('#txtFromDate').val('');
    $('#txtToDate').val('');
    $('#hdn_FromDate').val('');
    $('#hdn_ToDate').val('');
};
function AddNewProduct_OnClick() {
    var rowIdx = 0;
    var rowCount = $('#ProductItemDetailsTbl >tbody >tr').length - 1;
    var RowNo = 0;

    $("#ProductItemDetailsTbl >tbody >tr:eq(" + rowCount +")").each(function (i, row) {
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#hfsno").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    AddnewRowOnly(RowNo);
    //$('#ProductItemDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
    //        <td class="red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
    //        <td class="sr_padding"><span id="sno">${rowCount}</span><input type="hidden" id="hfsno" value="${RowNo}" /></td>
    //        <td><div class="col-sm-11 lpo_form" style="padding:0px;" ><select class="form-control" id="ProductName${RowNo}" onchange="OnChangeProductName(event)" name="ProductName"></select>
    //        <span id="productnameError" class="error-message is-visible"></span><input type="hidden" id="hfproductID" /></div><div class="col-sm-1 i_Icon">
    //        <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <i class="fa fa-info" aria-hidden="true" title="${$("#Span_ItemInformation_Title").text()}"></i> </button></div></td>
    //        <td><input id="UOM" class="form-control" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" /></td>
    //        <td id="tdforecastqty" style="display:none"><input id="ForecastQuantity" class="form-control num_right" type="text" name="ForecastQuantity" placeholder="${$("#span_ForecastQuantity").text()}" disabled></td>
    //        <td><div class="col-sm-11" style="padding:0px;"><input id="AvailableStockInBase" class="form-control num_right" type="text" name="AvailableStock" placeholder="${$("#span_AvailableStock").text()}" disabled></div>
    //        <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="ItemStockWareHouseWise(event,'P')" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"> <i class="fa fa-info" aria-hidden="true" title="${$("#span_StockDetail").text()}"></i> </button></div></td>
    //        <td width="10%"><div class="col-sm-10 lpo_form" style="padding:0px;" id=''>
    //        <input id="PlannedQuantity" class="form-control num_right" autocomplete="off" onkeypress="return OnKeyPressPlannedQty(this,event);" onchange="OnChangePlannedQty(this,event)" type="text" name="PlannedQuantity" placeholder="0000.00"><span id="PlannedQtyError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon">
    //        <button type="button" id="PlannedQuantityDetail" class="calculator" onclick="OnClickPlannedQtyIconBtn(event);" data-toggle="modal" data-target="#MaterialDetail" data-backdrop="static" data-keyboard="false"> <i class="fa fa-info" aria-hidden="true" title="${$("#span_MaterialDetail").text()}"></i> </button></div></td>
    //        <td><textarea id="remarks" class="form-control" name="remarks" maxlength="100" data-parsley-maxlength="100" placeholder="${$("#span_remarks").text()}" ></textarea></td>
    //        </tr>`);
    BindProductItmList(RowNo, "");
    HideShowAddProPlanBtn();
    //ddl_src_type_onchange();
}
function AddnewRowOnly(RowNo, BOM_Avl) {
    var rowIdx = 0;
    var rowCount = $('#ProductItemDetailsTbl >tbody >tr').length + 1;
    var span_subitemdetail = $("#span_SubItemDetail").text();
    var td_produced = "";
    if ($("#ddl_src_type").val() != "P") {
        td_produced = `<td>
            <div class="col-sm-8 lpo_form no-padding">
                <input id="ProducedQuantity" class="form-control num_right" autocomplete="off" value="" type="text" name="PlannedQuantity" placeholder="0000.00" disabled>
            </div>
            <div class="col-sm-2 i_Icon">
                <button type="button" id="ProducedQuantityDetail" class="calculator" onclick="OnClickProducedQtyIconBtn(event,'FG');" data-toggle="modal" data-target="#ProducedQuantityDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_ProductionDetail}"></button>
            </div>
            <div class="col-sm-2 i_Icon" id="div_SubItemProdQty" style="padding:0px; ">
            <button type="button" id="SubItemProdQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('ProduceQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
            </div> 
            </td>`;
    }
    var disableBOM = BOM_Avl == "N" ? "disabled" : "";

    $('#ProductItemDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
            <td class="red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
            <td class="sr_padding"><span id="sno">${rowCount}</span><input type="hidden" id="hfsno" value="${RowNo}" /></td>
            <td><div class="col-sm-10 lpo_form no-padding"><select class="form-control" id="ProductName${RowNo}" onchange="OnChangeProductName(event)" name="ProductName"></select>
            <span id="productnameError" class="error-message is-visible"></span><input type="hidden" id="hfproductID" />
<input type="hidden" id="ItemtypeFlag" value="" />
</div>
            <div class="col-sm-1 i_Icon">
            <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button></div> <div class="col-sm-1 i_Icon">
            <button type="button" id="BillOfMaterial" ${disableBOM} class="calculator no-padding subItmImg" onclick="OnClickIconBtnBOMDetail(event);" data-toggle="modal" data-target="#BillOfMaterial" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/BillOfMaterial.png" alt="" title="${$("#span_BillOfMaterial").text()}"></button>
             </div></td>
            <td><input id="UOM" class="form-control" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" /></td>
            <td id="tdforecastqty" style="display:none"><input id="ForecastQuantity" class="form-control num_right" type="text" name="ForecastQuantity" placeholder="${$("#span_ForecastQuantity").text()}" disabled></td>
            <td><div class="col-sm-8 no-padding"><input id="AvailableStockInBase" class="form-control num_right" type="text" name="AvailableStock" placeholder="0000.00" disabled></div>
            <div class="col-sm-2 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="ItemStockWareHouseWise(event,'P')" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
            <div class="col-sm-2 i_Icon"><button type="button" id="SubItemAvlStockBtn" disabled class="calculator subItmImg" onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"></button></div>
            </td>
            <td width="10%">
            <div class="col-sm-8 lpo_form no-padding" id=''>
            <input id="PlannedQuantity" class="form-control num_right" autocomplete="off" onkeypress="return OnKeyPressPlannedQty(this,event);" onchange="OnChangePlannedQty(this,event)" type="text" name="PlannedQuantity" placeholder="0000.00"><span id="PlannedQtyError" class="error-message is-visible"></span></div>
             <input hidden id="sub_item" />
            <div class="col-sm-2 i_Icon">
            <button type="button" id="PlannedQuantityDetail" class="calculator" onclick="OnClickPlannedQtyIconBtn(event);" data-toggle="modal" data-target="#MaterialDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_MaterialDetail").text()}"> </button></div>
            <div class="col-sm-2 i_Icon" id="div_SubItemPlanQty" style="padding:0px; ">
            <button type="button" id="SubItemPlanQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
            </div>
<input hidden type="text" id="HdnFGsubitemFlag" value="Quantity" />
            </td>
            ${td_produced}
            <td><textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength="100" placeholder="${$("#span_remarks").text()}" ></textarea></td>
</tr>`);

}
function CheckFormValidation() {
    debugger;
    var btn = $("#hdnsavebtn").val(); /**Added this Condition by Nitesh 11-01-2024 for Disable Approve btn after one Click**/
    var date = $("#AH_txtFromDate").val();
    var todate = moment().format('YYYY-MM-DD');
    if (date < todate) {
        $("#btn_save").attr("disabled", false);
        //$("#btn_save").css("filter", "grayscale(100%)");

        $("#btn_approve").attr("disabled", false);
       // $("#btn_approve").css("filter", "grayscale(100%)")
        $("#hdnsavebtn").val("");
    }
    else {
        if (btn == "Allreadyclick") {
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");

            $("#btn_approve").attr("disabled", true);
            $("#btn_approve").css("filter", "grayscale(100%)");
            return false;
        }
    }

    var rowcount = $('#ProductItemDetailsTbl tbody tr').length;
    var ValidationFlag = true;
    var ddl_src_type = $("#ddl_src_type").val();
    var ddlsrctype = $('#ddl_src_type option:selected').text();
    $('#hdnddl_src_type').val(ddlsrctype);
    
    var fy = $("#ddl_financial_year").val();
    var period = $("#ddl_period").val();

    var RequirementArea = $('#ddlRequiredArea').val();
    if (RequirementArea == "" || RequirementArea == "0") {
        document.getElementById("vmRequiredArea").innerHTML = $("#valueReq").text();
        $("#ddlRequiredArea").css("border-color", "red");
        ValidationFlag = false;
    }
    else {
        $("#vmRequiredArea").css("display", "none");
        $("#ddlRequiredArea").css("border-color", "#ced4da");
    }
    if (ddl_src_type == "D") {
        if (fy == "0" || fy == null) {
            $("#vm_ddl_financial_year").text($("#valueReq").text());
            $("#vm_ddl_financial_year").css("display", "block");
            $("#ddl_financial_year").css("border-color", "red");
            ValidationFlag = false;
        }
        else {
            $("#vm_ddl_financial_year").css("display", "none");
            $("#ddl_financial_year").css("border-color", "#ced4da");
        }
        if (period == "0" || period == null) {
            $("#vm_ddl_period").text($("#valueReq").text());
            $("#vm_ddl_period").css("display", "block");
            $("#ddl_period").css("border-color", "red");
            ValidationFlag = false;
        }
        else {
            $("#vm_ddl_period").css("display", "none");
            $("#ddl_period").css("border-color", "#ced4da");
        }
    }
    else if (ddl_src_type == "A") {
        if (CheckVallidation("AH_txtFromDate", "vm_AH_txtFromDate") == false) {
            ValidationFlag = false;
        }
        if (CheckVallidation("AH_txtToDate", "vm_AH_txtToDate") == false) {
            ValidationFlag = false;
        }
        if (CheckAdHocDateRange() == false) {
            ValidationFlag = false;
        }
    }
    if (ValidationFlag == true) {
        if (rowcount > 0) {
            var flagProductname = CheckMaterialValidations_ProductName();
            if (flagProductname == false) {
                return false;
            }
            var flagPlannedQty = CheckMaterialValidations_PlannedQty();
            if (flagPlannedQty == false) {
                return false;
            }
            var flagProcureAndProduce = CheckMaterialValidations_ProcureAndProduce();
            if (flagProcureAndProduce == false) {
                return false;
            }
            debugger;
            var flagsubItemVald = CheckValidations_forSubItems();
            if (flagsubItemVald == false) {
                return false;
            }
            
            debugger;
            if (flagPlannedQty == true && flagProductname == true) {
                debugger
                var srctype = $("#ddl_src_type").val();
                var fy = $("#ddl_financial_year").val();
                var period = $("#ddl_period").val();
                var hdn_period = $("#ddl_period  option:selected").text();
                $("#hdn_ddl_src_type").val(srctype);
                $("#hdn_ddl_financial_year").val(fy);
                $("#hdn_ddl_period").val(period);
                $('#hd_ddl_period').val(hdn_period);

                var MRPProductItemDetailList = new Array();
                debugger;
                $("#ProductItemDetailsTbl TBODY TR").each(function () {
                    var row = $(this);
                    var ItemList = {};
                    var sno = row.find("#hfsno").val();
                   
                    ItemList.SrNo = row.find("#sno").text();
                    ItemList.ProductId = row.find("#hfproductID").val();
                    ItemList.item_name = row.find("#ProductName" + sno).text().trim();
                    ItemList.UOMId = row.find('#UOMID').val();
                    ItemList.uom_name = row.find('#UOM').val();
                    ItemList.PlannedQuantity = row.find('#PlannedQuantity').val();
                    ItemList.remarks = row.find('#remarks').val();
                    ItemList.avl_stk = row.find('#AvailableStockInBase').val();                    
                    ItemList.ForecastQuantity = row.find('#ForecastQuantity').val();
                    ItemList.sub_item = row.find('#sub_item').val();
                    if ($("#ddl_src_type").val() != "P") {
                        ItemList.ProduceQuantity = row.find('#ProducedQuantity').val();
                    }
                    ItemList.ProduceQuantity = "0";
                    MRPProductItemDetailList.push(ItemList);
                });

                var str = JSON.stringify(MRPProductItemDetailList);
                $('#HDProductDetails').val(str);

                var SubItemsListArr = Cmn_SubItemList();
                var str2 = JSON.stringify(SubItemsListArr);
                $('#SubItemDetailsDt').val(str2);

                var SFPecureSubitem = GetDataSubitem_SFPrecure();
                var str3 = JSON.stringify(SFPecureSubitem);
                $('#SubItemDetailsDt_precure').val(str3);

                BindhdnSFMaterialDetails();
                BindhdnRMDetails();
                BindSFMaterialDetails();
                BindInputMaterialDetails();
                //var date = $("#AH_txtFromDate").val();
                //var todate = moment().format('YYYY-MM-DD');
                var flagCheckDates = CheckValidation_forAllDateOnPage();//added by Suraj Maurya on 15-11-2024
                //var doc_status = $("#hd_Status").val();
                //if (date < todate) {
                if (flagCheckDates == false) {
                    $("#hdnsavebtn").val("");

                }
                else {
                    $("#btn_approve").css("filter", "grayscale(100%)"); /**Added this Condition by Nitesh 11-01-2024 for Disable Approve btn after one Click**/
                    $("#hdnsavebtn").val("Allreadyclick");
                }
              
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
function CheckValidation_forAllDateOnPage() {//Created by Suraj Maurya on 15-11-2024 to validate all dates togather
    var txtFromDate = $("#AH_txtFromDate").val();
    var txtToDate = $("#AH_txtToDate").val();
   
    var currentdate = moment().format('YYYY-MM-DD');

    if (txtFromDate < currentdate || txtToDate < currentdate) {
        return false;
    }
    let flagCheckDate = "N";
    $("#SFMaterialDetailsTbl tbody tr").each(function () {
        var proc_dt = $(this).find("#ProcurementCompletionDate").val();
        var prod_dt = $(this).find("#ProductionCompletionDate").val();
        if (proc_dt < currentdate || prod_dt < currentdate) {
            flagCheckDate = "Y";
            //break;
        }
    });
    $("#InputMaterialDetailsTbl tbody tr").each(function () {
        var proc_dt = $(this).find("#ProcurementCompletionDate").val();
        if (proc_dt < currentdate) {
            flagCheckDate = "Y";
            //break;
        }
    });
    if (flagCheckDate == "Y") {
        return false;
    }
    return true;
}
function GetDataSubitem_SFPrecure() {
    debugger;
    var NewArr = new Array();
    $("#hdn_SubItemDetailTblMRPSF_Procure tbody tr").each(function () {
        var row = $(this);
        if (parseFloat(CheckNullNumber(row.find('#subItemQty').val())) > 0) {
            debugger;
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.ItemType = "SF";//Added by Suraj on 
            NewArr.push(List);
        }
    });
    return NewArr;
}
function CheckMaterialValidations_ProductName() {
    debugger;
    var ErrorFlag = "N";
    $("#ProductItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var sno = currentRow.find("#hfsno").val();
        var ProductName = currentRow.find("#ProductName" + sno).val();

        if (ProductName == "0") {
            currentRow.find("#productnameError").text($("#valueReq").text());
            currentRow.find("#productnameError").css("display", "block");
            currentRow.find(".select2-container--default .select2-selection--single").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#productnameError").css("display", "none");
            currentRow.find(".select2-container--default .select2-selection--single").css("border-color", "#ced4da");
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckMaterialValidations_PlannedQty() {
    debugger;
    var ErrorFlag = "N";
    $("#ProductItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var plannedQty = currentRow.find("#PlannedQuantity").val();
        if (plannedQty != "") {
            if (parseFloat(plannedQty) == parseFloat(0)) {
                currentRow.find("#PlannedQtyError").text($("#valueReq").text());
                currentRow.find("#PlannedQtyError").css("display", "block");
                currentRow.find("#PlannedQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#PlannedQtyError").css("display", "none");
                currentRow.find("#PlannedQuantity").css("border-color", "#ced4da");
            }
        }
        else {
            currentRow.find("#PlannedQtyError").text($("#valueReq").text());
            currentRow.find("#PlannedQtyError").css("display", "block");
            currentRow.find("#PlannedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
            
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckMaterialValidations_ProcureAndProduce() {
    debugger;
    var ErrorFlag = "N";
    //var ErrorMismatchFlag = "N";
    //var ReqAreaError = "N"; //Commented By Suraj on 25-01-2023 
    $("#SFMaterialDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var ProduceQty = currentRow.find("#OrderToProduceQuantity").val();
        var ProcureQty = currentRow.find("#OrderToProcureQuantity").val();
        if (parseFloat(CheckNullNumber(ProcureQty)) > 0) {
            if (currentRow.find("#ProcurementCompletionDate").val() == "") {
                currentRow.find("#ProcurementCompletionDate").css("border-color", "red");
                currentRow.find("#ProcurementCompletionDateError").text($("#valueReq").text());
                currentRow.find("#ProcurementCompletionDateError").css("display", "block");
                if (ErrorFlag == "N") {
                    currentRow.find("#ProcurementCompletionDate").focus();
                }
                ErrorFlag = "Y";
            } else {
                currentRow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
                currentRow.find("#ProcurementCompletionDateError").text("");
                currentRow.find("#ProcurementCompletionDateError").css("display", "none");
            }
        } else {
            currentRow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
            currentRow.find("#ProcurementCompletionDateError").text("");
            currentRow.find("#ProcurementCompletionDateError").css("display", "none");
        }
        if (parseFloat(CheckNullNumber(ProduceQty)) > 0) {
            if (currentRow.find("#ProductionCompletionDate").val() == "") {
                currentRow.find("#ProductionCompletionDate").css("border-color", "red");
                currentRow.find("#ProductionCompletionDateError").text($("#valueReq").text());
                currentRow.find("#ProductionCompletionDateError").css("display", "block");
                if (ErrorFlag == "N") {
                    currentRow.find("#ProductionCompletionDate").focus();
                }
                ErrorFlag = "Y";
            } else {
                currentRow.find("#ProductionCompletionDate").css("border-color", "#ced4da");
                currentRow.find("#ProductionCompletionDateError").text("");
                currentRow.find("#ProductionCompletionDateError").css("display", "none");
            }
        } else {
            currentRow.find("#ProductionCompletionDate").css("border-color", "#ced4da");
            currentRow.find("#ProductionCompletionDateError").text("");
            currentRow.find("#ProductionCompletionDateError").css("display", "none");
        }
    });
    if ($("#InputMaterialDetailsTbl >tbody >tr").length > 0) {
        $("#InputMaterialDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var ProcureQty = currentRow.find("#RequisitionQuantity").val();
            if (parseFloat(CheckNullNumber(ProcureQty)) > 0) {
                if (currentRow.find("#ProcurementCompletionDate").val() == "") {
                    currentRow.find("#ProcurementCompletionDate").css("border-color", "red");
                    currentRow.find("#ProcurementCompletionDateError").text($("#valueReq").text());
                    currentRow.find("#ProcurementCompletionDateError").css("display", "block");
                    if (ErrorFlag == "N") {
                        currentRow.find("#ProcurementCompletionDate").focus();
                    }
                    ErrorFlag = "Y";
                } else {
                    currentRow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
                    currentRow.find("#ProcurementCompletionDateError").text("");
                    currentRow.find("#ProcurementCompletionDateError").css("display", "none");
                }
            }

        });
    } else {
        ErrorFlag = "Y";
        swal("", $("#span_RawMaterialDetailNotFound").text(), "warning");
    }
    
   
    if (ErrorFlag == "Y") {
        //swal("", $("#span_RawMaterialDetailNotFound").text(), "warning");//commented by suraj maurya on 15-02-2025
        return false;
    }
    else {
        return true;
    }
}
function OnChangeProcureDate(evt) {
    var currentRow = $(evt.target).closest("tr");
    currentRow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
    currentRow.find("#ProcurementCompletionDateError").text("");
    currentRow.find("#ProcurementCompletionDateError").css("display", "none");
    
}
function OnChangeSFProduceDate(evt) {
    var currentRow = $(evt.target).closest("tr");
    currentRow.find("#ProductionCompletionDate").css("border-color", "#ced4da");
    currentRow.find("#ProductionCompletionDateError").text("");
    currentRow.find("#ProductionCompletionDateError").css("display", "none");
    
}
function BindInputMaterialDetails() {
    var MRPInputMaterialDetailList = new Array();
    debugger;
    $("#InputMaterialDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var currentRow = $(this);
        var InputMaterialItemList = {};
        InputMaterialItemList.materialid = currentRow.find("#hf_materialID").val();
        InputMaterialItemList.MaterialName = currentRow.find("#MaterialName").val();
        InputMaterialItemList.uomid = currentRow.find("#UOMID").val();
        InputMaterialItemList.uom_name = currentRow.find("#UOM").val();
        InputMaterialItemList.bomqty = currentRow.find("#BOMQuantity").val();
        InputMaterialItemList.minrevstk = currentRow.find("#MinimumStockReserve").val();
        InputMaterialItemList.requiredqty = currentRow.find("#RequiredQuantity").val();
        InputMaterialItemList.pendingorederqty = currentRow.find("#PendingOrderQuantity").val();
        InputMaterialItemList.requisitionqty = currentRow.find("#RequisitionQuantity").val();
        InputMaterialItemList.procurementdate = currentRow.find("#ProcurementCompletionDate").val();
        InputMaterialItemList.avl_stk = currentRow.find("#AvailableStock").val();
        InputMaterialItemList.ShopFloorstock = currentRow.find("#ShopFloorstock").val();
        InputMaterialItemList.INtransit = currentRow.find("#INtransit").val();
        InputMaterialItemList.ProcuredQty = currentRow.find("#ProcuredQty").val();

        MRPInputMaterialDetailList.push(InputMaterialItemList);
    });
    var str = JSON.stringify(MRPInputMaterialDetailList);
    $('#HDInputMaterialDetails').val(str);
}
function BindSFMaterialDetails() {
    var MRPSFMaterialDetailList = new Array();
    $("#SFMaterialDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var SFMaterialItemList = {};
        SFMaterialItemList.sr_no = currentRow.find("#SFSrNo").text();
        SFMaterialItemList.materialid = currentRow.find("#hf_materialID").val();
        SFMaterialItemList.MaterialName = currentRow.find("#MaterialName").val().trim();
        SFMaterialItemList.bom_item_id = currentRow.find("#hf_FGProductID").val();
        SFMaterialItemList.fg_item_id = currentRow.find("#hf_FGID").val();
        SFMaterialItemList.item_type = currentRow.find("#hf_MaterialType").val();
        SFMaterialItemList.uomid = currentRow.find("#UOMID").val();
        SFMaterialItemList.uom_name = currentRow.find("#UOM").val();
        SFMaterialItemList.bomqty = currentRow.find("#BOMQuantity").val();
        SFMaterialItemList.AvlStock = currentRow.find("#AvailableStock").val();
        SFMaterialItemList.WIPstock = currentRow.find("#WIPstock").val();
        SFMaterialItemList.ShopFloorstock = currentRow.find("#ShopFloorstock").val();
        SFMaterialItemList.INtransit = currentRow.find("#INtransit").val();
        SFMaterialItemList.RequiredQuantity = currentRow.find("#RequiredQuantity").val();
        SFMaterialItemList.pend_ord_qty = currentRow.find("#PendingOrderQuantity").val();
        SFMaterialItemList.OrderToProcureQuantity = currentRow.find("#OrderToProcureQuantity").val();
        SFMaterialItemList.ProcurementCompletionDate = currentRow.find("#ProcurementCompletionDate").val();
        SFMaterialItemList.OrderToProduceQuantity = currentRow.find("#OrderToProduceQuantity").val();
        SFMaterialItemList.ProductionCompletionDate = currentRow.find("#ProductionCompletionDate").val();
        SFMaterialItemList.ProcuredQty = currentRow.find("#ProcuredQty").val();
        SFMaterialItemList.ProducedQty = currentRow.find("#ProducedQty").val();

        MRPSFMaterialDetailList.push(SFMaterialItemList);
    });
    var str = JSON.stringify(MRPSFMaterialDetailList);
    $('#HDSFMaterialDetails').val(str);
}
function BindhdnSFMaterialDetails() {
    var MRPhdnSFMaterialDetailList = new Array();
    //$("#Hdn_SFItemsFGWise >tbody >tr").each(function (i, row) {//Commented by Suraj Maurya on 17-01-2025
    //    var currentRow = $(this);
    //    var SFMaterialItemList = {};
    //    SFMaterialItemList.fg_ItemId = currentRow.find("#fg_ItemId").text();
    //    SFMaterialItemList.fg_rowno = currentRow.find("#fg_rowno").text();
    //    SFMaterialItemList.fg_Sf_ItemID = currentRow.find("#fg_Sf_ItemID").text();
    //    SFMaterialItemList.fg_BomQty = currentRow.find("#fg_BomQty").text();
    //    SFMaterialItemList.fg_ReqQty = currentRow.find("#fg_ReqQty").text();
    //    SFMaterialItemList.parent_sf_item_id = currentRow.find("#parent_Sf_ItemID").text();
    //    SFMaterialItemList.fg_sf_item_type = currentRow.find("#fg_Sf_ItemType").text();
    //    MRPhdnSFMaterialDetailList.push(SFMaterialItemList);
    //});

    var ssn_SfData = sessionStorage.ArrSfData_forCalculation;
    var HdnSfArr = ssn_SfData != null ? JSON.parse(ssn_SfData) : [];
    HdnSfArr.map((item) => {
        var SFMaterialItemList = {};
        SFMaterialItemList.fg_ItemId = item.fg_ItemId;
        SFMaterialItemList.fg_rowno = item.fg_rowno;
        SFMaterialItemList.fg_Sf_ItemID = item.fg_Sf_ItemID;
        SFMaterialItemList.fg_BomQty = item.fg_BomQty;
        SFMaterialItemList.fg_ReqQty = item.fg_ReqQty;
        SFMaterialItemList.parent_sf_item_id = item.parent_Sf_ItemID;
        SFMaterialItemList.fg_sf_item_type = item.fg_Sf_ItemType;
        MRPhdnSFMaterialDetailList.push(SFMaterialItemList);
    })

    var str = JSON.stringify(MRPhdnSFMaterialDetailList);
    $('#hdnSFMaterialDetail').val(str);
}
function BindhdnRMDetails() {
    var MRPhdnRMDetailList = new Array();
    //$("#Hdn_RMItemsSFGWise >tbody >tr").each(function (i, row) {
    //    var currentRow = $(this);
    //    var RMItemList = {};
    //    RMItemList.fg_ItemId = currentRow.find("#fg_ItemId").text();
    //    RMItemList.fg_rowno = currentRow.find("#fg_rowno").text();
    //    RMItemList.Sf_ItemID = currentRow.find("#Sf_ItemID").text();
    //    RMItemList.RM_ItemID = currentRow.find("#RM_ItemID").text();
    //    RMItemList.RM_BomQty = currentRow.find("#RM_BomQty").text();
    //    RMItemList.RM_ReqQty = currentRow.find("#RM_ReqQty").text();
    //    MRPhdnRMDetailList.push(RMItemList);
    //});
    var ssn_RmData = sessionStorage.ArrRmData_forCalculation;
    var HdnRmArr = ssn_RmData != null ? JSON.parse(ssn_RmData) : [];
    HdnRmArr.map((item) => {
        var RMItemList = {};
        RMItemList.fg_ItemId = item.fg_ItemId;
        RMItemList.fg_rowno = item.fg_rowno;
        RMItemList.Sf_ItemID = item.Sf_ItemID;
        RMItemList.RM_ItemID = item.RM_ItemID;
        RMItemList.RM_BomQty = item.RM_BomQty;
        RMItemList.RM_ReqQty = item.RM_ReqQty;
        MRPhdnRMDetailList.push(RMItemList);
    })
    var str = JSON.stringify(MRPhdnRMDetailList);
    $('#hdnInputMaterialDetail').val(str);
}
function DeleteMaterialAndInputItemDetails(productid) {

    if (productid != "") {
        //$("#Hdn_RMItemsSFGWise tbody tr #fg_ItemId:contains(" + productid + ")").closest("tr").remove();
        //$("#Hdn_SFItemsFGWise tbody tr #fg_ItemId:contains(" + productid + ")").closest("tr").remove();
        var ssn_RmData = sessionStorage.ArrRmData_forCalculation;
        var HdnRmArr = ssn_RmData != null ? JSON.parse(ssn_RmData) : [];
        var NewHdnRmArr = HdnRmArr.filter(u => u.fg_ItemId != productid);
        sessionStorage.setItem("ArrRmData_forCalculation", JSON.stringify(NewHdnRmArr));
        var ssn_SfData = sessionStorage.ArrSfData_forCalculation;
        var HdnSfArr = ssn_SfData != null ? JSON.parse(ssn_SfData) : [];
        var NewHdnSfArr = HdnSfArr.filter(u => u.fg_ItemId != productid);
        sessionStorage.setItem("ArrSfData_forCalculation", JSON.stringify(NewHdnSfArr));

        $("#SFMaterialDetailsTbl tbody tr").each(function () {
            var clickedrow = $(this);
            var Product_id = clickedrow.find("#hf_FGID").val();
            var SF_ItemId = clickedrow.find("#hf_materialID").val();

            var AvlStock = clickedrow.find("#AvailableStock").val();
            var WIPstock = clickedrow.find("#WIPstock").val();
            var Shflstock = clickedrow.find("#ShopFloorstock").val();
            var INtransit = clickedrow.find("#INtransit").val();
            var PendOrderQty = clickedrow.find("#PendingOrderQuantity").val();

            var RequiredQuantity = 0;//clickedrow.find("#RequiredQuantity").val();
            var SFbomQty = 0;
            var SFItemRows = NewHdnSfArr.filter(u => u.fg_Sf_ItemID == SF_ItemId);//$("#Hdn_SFItemsFGWise tbody tr #fg_Sf_ItemID:contains(" + SF_ItemId + ")").closest("tr");
            if (SFItemRows.length > 0) {
                SFItemRows.map((item)=> {
                    var fg_ReqQty = 0;
                    fg_ReqQty = item.fg_ReqQty;
                    var fg_BomQty = item.fg_BomQty;
                    SFbomQty = parseFloat(SFbomQty) + parseFloat(fg_BomQty);
                    RequiredQuantity = parseFloat(parseFloat(RequiredQuantity) + parseFloat(fg_ReqQty));//.toFixed(QtyDecDigit);
                });

                if (parseFloat(RequiredQuantity) > 0) {
                    clickedrow.find("#ProductionCompletionDate").attr("disabled", false);
                } else {
                    clickedrow.find("#ProductionCompletionDate").val(""); 
                    clickedrow.find("#ProductionCompletionDate").attr("disabled", true);
                }
                //clickedrow.find("#BOMQuantity").val(parseFloat(SFbomQty).toFixed(QtyDecDigit));
                clickedrow.find("#BOMQuantity").val(parseFloat(RequiredQuantity).toFixed(QtyDecDigit));
                var ReqQty = parseFloat(RequiredQuantity) - (parseFloat(AvlStock) + parseFloat(WIPstock) + parseFloat(Shflstock) + parseFloat(INtransit));
                clickedrow.find("#RequiredQuantity").val(parseFloat(ReqQty).toFixed(QtyDecDigit));

                var OrdToProduce = parseFloat(ReqQty) - parseFloat(PendOrderQty);
                OrdToProduce = parseFloat(OrdToProduce) >= 0 ? OrdToProduce : 0;
                clickedrow.find("#OrderToProduceQuantity").val(parseFloat(OrdToProduce).toFixed(0));
            } else {
                clickedrow.remove();
            }

        });

        $("#InputMaterialDetailsTbl tbody tr").each(function () {
            var clickedrow = $(this);
            var RM_ItemId = clickedrow.find("#hf_materialID").val();
            var RM_ReqQty = 0;
            var ItemRows = NewHdnRmArr.filter(u => u.RM_ItemID == RM_ItemId);//$("#Hdn_RMItemsSFGWise tbody tr #RM_ItemID:contains(" + RM_ItemId + ")").closest("tr");
            if (ItemRows.length > 0) {
                ItemRows.map((item) => {
                    var Crow = $(this);
                    var Sf_ItemID = item.Sf_ItemID;
                    var RM_ReqQty1 = item.RM_ReqQty;
                    RM_ReqQty = parseFloat(parseFloat(RM_ReqQty) + parseFloat(RM_ReqQty1)).toFixed(QtyDecDigit);
                });
                clickedrow.find("#BOMQuantity").val(RM_ReqQty);
            } else {
                clickedrow.remove();
            }
        });


        //$("#SaveProductMaterialTbl >tbody >tr").each(function (i, row) {//Commented by Suraj maurya on 18-01-2025
        //    var currentRow = $(this);
        //    var md_ProductID = currentRow.find("#MRP_ProductID").val();
        //    if (md_ProductID === productid) {
        //        var md_MaterialID = currentRow.find("#MRP_MaterialID").val();
        //        var md_ReqQty = currentRow.find("#MRP_ReqQty").val();


        //        //$("#InputMaterialDetailsTbl >tbody >tr").each(function (i, row) {
        //        //    var icurrentRow = $(this);
        //        //    var imd_materialid = icurrentRow.find("#hf_materialID").val();
        //        //    if (imd_materialid === md_MaterialID) {
        //        //        var fval = parseFloat(0);
        //        //        var imd_bomqty = icurrentRow.find("#BOMQuantity").val();

        //        //        fval = (parseFloat(imd_bomqty) - parseFloat(md_ReqQty))
        //        //        if (parseFloat(fval) === parseFloat(0)) {
        //        //            icurrentRow.remove();
        //        //        }
        //        //        else {
        //        //            icurrentRow.find("#BOMQuantity").val(parseFloat(fval).toFixed(QtyDecDigit));
        //        //        }
        //        //    }
        //        //});

        //        currentRow.remove();
        //    }
        //});
        ResetSerialNo();
        CalculationReqAndRequisitionQty();
        Cmn_DeleteSubItemQtyDetail(productid);
    }
    
}
function BindProductItmList(RowID, btype) {

    if ($("#ddl_src_type").val() == "P") {
        $(".fa-trash").css("display", "none");
        $("#BtnAddItem").closest("div .plus_icon1").css("display", "none");
        $("#AddBtnIconMRP").css("display", "none");
        $("#ProductItemDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var sno = currentRow.find("#hfsno").val();
            var Itemid = currentRow.find("#hfproductID").val();
            if (Itemid != "") {
                currentRow.find("#ProductName" + sno).append(`
                    <option value="${currentRow.find("#hfproductID").val()}" selected>${currentRow.find("#hfproductName").val()}</option>
                `);
                currentRow.find("#ProductName" + sno).attr("disabled", true);
            }
        });
    }
    else {
        BindItemList("#ProductName", RowID, "#ProductItemDetailsTbl", "#hfsno", "ForOneRow", "MRP");
        //$.ajax(
        //    {
        //        type: "POST",
        //        url: "/ApplicationLayer/MaterialRequirementPlan/BindProductList",
        //        data: {},
        //        dataType: "json",
        //        success: function (data) {
        //            debugger;
        //            if (data !== null && data !== "") {
        //                var arr = [];
        //                arr = JSON.parse(data);
        //                if (arr.Table.length > 0) {
        //                    $('#ProductName' + RowID + " optgroup option").remove();
        //                    $('#ProductName' + RowID + " optgroup").remove();

        //                    $('#ProductName' + RowID).append(`<optgroup class='def-cursor' id="Textddl${RowID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        //                    for (var i = 0; i < arr.Table.length; i++) {
        //                        $('#Textddl' + RowID).append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].product_id}">${arr.Table[i].item_name}</option>`);
        //                    }
        //                    var firstEmptySelect = true;
        //                    $('#ProductName' + RowID).select2({
        //                        templateResult: function (data) {
        //                            var selected = $('#ProductName' + RowID).val();
        //                            if (check(data, selected, "#ProductItemDetailsTbl", "#hfsno", '#ProductName') == true) {
        //                                var UOM = $(data.element).data('uom');
        //                                var classAttr = $(data.element).attr('class');
        //                                var hasClass = typeof classAttr != 'undefined';
        //                                classAttr = hasClass ? ' ' + classAttr : '';
        //                                var $result = $(
        //                                    '<div class="row">' +
        //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
        //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
        //                                    '</div>'
        //                                );
        //                                return $result;
        //                            }
        //                            firstEmptySelect = false;
        //                        }
        //                    });

        //                    if (btype === "bdind") {
        //                        $("#ProductItemDetailsTbl >tbody >tr").each(function (i, row) {
        //                            debugger;
        //                            var currentRow = $(this);
        //                            var sno = currentRow.find("#hfsno").val();
        //                            var Itemid = currentRow.find("#hfproductID").val();
        //                            if (Itemid != "") {
        //                                currentRow.find("#ProductName" + sno).removeAttr("onchange");
        //                                currentRow.find("#ProductName" + sno).val(Itemid).trigger('change');
        //                                currentRow.find("#ProductName" + sno).attr('onchange', 'OnChangeProductName(event)');
        //                            }
        //                            else {
        //                                currentRow.find("#ProductName" + sno).removeAttr("onchange");
        //                                currentRow.find("#ProductName" + sno).val("0").trigger('change');
        //                                currentRow.find("#ProductName" + sno).attr('onchange', 'OnChangeProductName(event)');
        //                            }
        //                        });
        //                    }
        //                    //else {
        //                    //    $("#ProductName" + RowID).removeAttr("onchange");
        //                    //    $("#ProductName" + RowID).val("0").trigger('change');
        //                    //    $("#ProductName" + RowID).attr('onchange', 'OnChangeProductName(event)');
        //                    //}
        //                }
        //            }
        //        },
        //    });
    }

   
}
function OnChangeProductName(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfproductID").val();
    var Itm_ID;
    var SNo = clickedrow.find("#hfsno").val();

    Itm_ID = clickedrow.find("#ProductName" + SNo).val();
    clickedrow.find("#hfproductID").val(Itm_ID);

    if (Itm_ID == "0") {
        clickedrow.find("#productnameError").text($("#valueReq").text());
        clickedrow.find("#productnameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ProductName" + SNo + "-container']").css("border-color", "red");
    }
    else {

        clickedrow.find("#productnameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ProductName" + SNo + "-container']").css("border-color", "#ced4da");
        DeleteMaterialAndInputItemDetails(ItemID);
    }
    ClearRowDetails(e, ItemID);
    DisableHeaderField();
    try {
        Cmn_BindUOM(clickedrow, Itm_ID, "", "Y","MRPlan");

    } catch (err) {
    }
}
function ClearRowDetails(e, ItemID) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");

    clickedrow.find("#UOM").val("");
    clickedrow.find("#ForecastQuantity").val("");
    clickedrow.find("#AvailableStock").val("");
    clickedrow.find("#PlannedQuantity").val("");
    clickedrow.find("#remarks").val("");

    Cmn_DeleteSubItemQtyDetail(ItemID)

    //if (ItemID != "" && ItemID != null) {
    //    ShowItemListItm(ItemID);
    //}
}
function DisableHeaderField() {
    //debugger;
    $("#ddl_src_type").attr('disabled', true);
    $("#ddl_financial_year").attr('disabled', true);
    $("#ddl_period").attr('disabled', true);
    //$("#ddlRequiredArea").attr('disabled', true);
    
    //$("#divdocsection").css("display", "none");

}
function ItemStockWareHouseWise(evt,flag) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var sno = "";
        var ItemId = "";
        var ItemName = "";
        var UOMName = "";
        var UOMID = "";
        if (flag == "P") {
             sno = clickedrow.find("#hfsno").val();
             ItemId = clickedrow.find("#hfproductID").val();
             ItemName = clickedrow.find("#ProductName" + sno + " option:selected").text();
             UOMName = clickedrow.find("#UOM").val();
            UOMID = clickedrow.find("#UOMID").val();
        }
        if (flag == "I") {
            ItemId = clickedrow.find("#hf_materialID").val();
            ItemName = clickedrow.find("#MaterialName").val();
            UOMName = clickedrow.find("#UOM").val();
            UOMID = clickedrow.find("#UOMID").val();
        }
       
        $.ajax(
            {
                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: { ItemId: ItemId, UomId: UOMID},
                success: function (data) {
                    debugger;
                    $('#ItemStockWareHouseWise').html(data);
                    $("#WareHouseWiseItemName").val(ItemName);
                    $("#WareHouseWiseUOM").val(UOMName);
                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickPlannedQtyIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var srno = clickedrow.find("#hfsno").val();
    var productname = clickedrow.find("#ProductName" + srno +" option:selected").text();
    var uom = clickedrow.find("#UOM").val();

    var plannedqty = clickedrow.find("#PlannedQuantity").val();
    var productid = clickedrow.find("#hfproductID").val();

    if ((plannedqty == "") || (plannedqty == null)) {
        plannedqty = "0";
    }
    clickedrow.find("#PlannedQuantity").val(parseFloat(plannedqty).toFixed(QtyDecDigit));

    $('#MRP_MaterialDetailTbl tbody tr').remove();
    var sno = 0;
    //$("#SaveProductMaterialTbl >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this);
    //    var md_ProductID = currentRow.find("#MRP_ProductID").val();
    //    if (md_ProductID === productid) {
    //        var md_MatrialName = currentRow.find("#MRP_MaterialName").val();
    //        var md_uomname = currentRow.find("#MRP_UOMName").val();
    //        var md_MatrialType = currentRow.find("#MRP_MaterialType").val();
    //        var md_ReqQty = currentRow.find("#MRP_ReqQty").val();
            
    //            $('#MRP_MaterialDetailTbl tbody').append(`<tr>
    //               <td>${parseInt(sno) + 1}</td>
    //               <td>${md_MatrialName}</td>
    //               <td>${md_uomname}</td>
    //               <td>${md_MatrialType}</td>
    //               <td class="num_right">${parseFloat(md_ReqQty).toFixed(QtyDecDigit)}</td>
    //               </tr>`);
    //        sno++;
    //    }  
    //});

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialRequirementPlan/getProductMaterialDetails",
        data: { product_id: productid, qty: plannedqty},
        success: function (data) {
            if (data != null || data != "") {
                $("#PopUpMaterialDetail").html(data);
               
            }
            $("#MD_ProductName").val(productname);
            $("#hfMD_productID").val(productid);
            $("#PP_MD_UOM").val(uom);
            $("#MD_PlannedQuantity").val(parseFloat(plannedqty).toFixed(QtyDecDigit));
        }
    });

  
    
}
async function OnChangePlannedQty(el, evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var srno = clickedrow.find("#hfsno").val();
    var span_subitemdetail = $("#span_SubItemDetail").text();
    //var productname = clickedrow.find("#ProductName" + srno + " option:selected").text();
    //var uom = clickedrow.find("#UOM").val();

    var plannedqty = clickedrow.find("#PlannedQuantity").val();
    var productid = clickedrow.find("#hfproductID").val();

    if ((plannedqty == "") || (plannedqty == null) || (plannedqty == "0")) {
        clickedrow.find("#PlannedQuantity").val("");
    }
    else {
        plannedqty = CheckNullNumber(plannedqty);
        clickedrow.find("#PlannedQuantity").val(parseFloat(plannedqty).toFixed(0));
    }
    //clickedrow.find("#PlannedQuantity").val(parseFloat(plannedqty).toFixed(0));

    if (plannedqty !="0" && productid != '') {
    //    if (parseFloat(plannedqty) != parseFloat(0)) {
        let startTime = moment();
        TotalLoad = 1;
        CountLoad = TotalLoad;
        var BtList = []
        BtList.push({ sr_no: 1, product_id: productid, qty: plannedqty });
        await AsyncGetSfAndRmData(BtList, startTime, "OnProductQtyChange");
//            $.ajax(
//                {
//                    type: "POST",
//                    url: "/ApplicationLayer/MaterialRequirementPlan/GetPlannedMaterialDetails",
//                    data: { ProductID: productid, plannedqty: plannedqty },
//                    dataType: "json",
//                    success: function (data) {
//                        debugger;
//                        try {
//                            if (data !== null && data !== "") {
//                                var arr = [];
//                                arr = JSON.parse(data);
//                                //clickedrow.find("#hfproductRevNo").val();
//                                if (arr.Table1.length > 0) {
//                                    $('#Hdn_SFItemsFGWise tbody tr #fg_ItemId:contains(' + arr.Table1[0].FG_ItemId + ')').closest("tr").remove();
//                                }
//                                if (arr.Table2.length > 0) {
//                                    $('#Hdn_RMItemsSFGWise tbody tr #fg_ItemId:contains(' + arr.Table2[0].FG_ItemId + ')').closest("tr").remove();
//                                }
//                                if (arr.Table.length > 0) {

//                                    $("#SaveProductMaterialTbl >tbody >tr").each(function (i, row) {
//                                        var currentRow = $(this);
//                                        var md_ProductID = currentRow.find("#MRP_ProductID").val();
//                                        if (md_ProductID === productid) {
//                                            currentRow.remove();
//                                        }
//                                    });

//                                    for (var i = 0; i < arr.Table.length; i++) {
//                                        $('#SaveProductMaterialTbl tbody').append(`<tr>
//                                        <td><input type="text" id="MRP_ProductID" value="${productid}" /></td>
//                                        <td><input type="text" id="MRP_MaterialID" value="${arr.Table[i].item_id}" /></td>
//                                        <td><input type="text" id="MRP_MaterialName" value='${arr.Table[i].MatrialName}' /></td>
//                                        <td><input type="text" id="MRP_UOMID" value="${arr.Table[i].uom_id}" /></td>
//                                        <td><input type="text" id="MRP_UOMName" value="${arr.Table[i].uom_name}" /></td>
//                                        <td><input type="text" id="MRP_MaterialType" value="${arr.Table[i].MatrialType}" /></td>
//                                        <td><input type="text" id="MRP_ReqQty" value="${parseFloat(parseFloat(arr.Table[i].ReqQty)/* * parseFloat(plannedqty)*/).toFixed(QtyDecDigit)}" /></td>
//                                    </tr>`);
//                                    }
//                                }
//                                for (var j = 0; j < arr.Table3.length; j++) {
//                                    $('#Hdn_SFItemsFGWise tbody').append(`<tr>
//                                                                <td id="fg_ItemId">${arr.Table3[j].FG_ItemId}</td>
//                                                                <td id="fg_rowno">${arr.Table3[j].Rowno}</td>
//                                                                <td id="parent_Sf_ItemID">${arr.Table3[j].prod_id}</td>
//                                                                <td id="fg_Sf_ItemID">${arr.Table3[j].MatrialID}</td>
//                                                                <td id="fg_Sf_ItemType">${arr.Table3[j].MaterialType}</td>
//                                                                <td id="fg_BomQty">${arr.Table3[j].BOMQty}</td>
//                                                                <td id="fg_ReqQty">${arr.Table3[j].BOMQty}</td>
//                                                            </tr>`);
//                                }
//                                if (arr.Table1.length > 0) {


//                                    for (var j = 0; j < arr.Table1.length; j++) {
//                                        var subitmDisable = "";
//                                        if (arr.Table1[j].sub_item != "Y") {
//                                            subitmDisable = "disabled";
//                                        }
//                                        var ExistMaterialID = "N";
//                                        var materialid = arr.Table1[j].MatrialID;
//                                        var bomqty = arr.Table1[j].BOMQty;
//                                        $("#SFMaterialDetailsTbl >tbody >tr").each(function (i, row) {
//                                            var currentRow = $(this);
//                                            var imd_materialid = currentRow.find("#hf_materialID").val();
//                                            if (imd_materialid === materialid) {
//                                                ExistMaterialID = "Y";
//                                            }
//                                        });

//                                        if (ExistMaterialID === "Y") {
//                                            $("#SFMaterialDetailsTbl >tbody >tr #hf_materialID[value=" + materialid + "]").closest("tr").each(function (i, row) {
//                                                var currentRow = $(this);
//                                                var imdmaterialid = currentRow.find("#hf_materialID").val();

//                                                if (imdmaterialid === materialid) {
//                                                    var TotalBomQty = parseFloat(0);
//                                                    $('#Hdn_SFItemsFGWise tbody tr #fg_Sf_ItemID:contains(' + imdmaterialid + ')').closest("tr").each(function () {
//                                                        var currentRow1 = $(this);
//                                                        var fg_BomQty = currentRow1.find("#fg_ReqQty").text();
//                                                        TotalBomQty = parseFloat(TotalBomQty) + parseFloat(fg_BomQty);

//                                                    });

//                                                    currentRow.find("#BOMQuantity").val(parseFloat(TotalBomQty).toFixed(QtyDecDigit));
//                                                    var reqQty = parseFloat(TotalBomQty) - (parseFloat(arr.Table1[j].AvlStock) + parseFloat(arr.Table1[j].wip_stock) + parseFloat(arr.Table1[j].shfl_stock) + parseFloat(arr.Table1[j].Intrst_stock));

//                                                    reqQty = reqQty > 0 ? reqQty : 0;
//                                                    if (reqQty == 0) {
//                                                        currentRow.find("#ProductionCompletionDate").val("");
//                                                        currentRow.find("#ProductionCompletionDate").attr("disabled", true);
//                                                    } else {
//                                                        currentRow.find("#ProductionCompletionDate").attr("disabled", false);
//                                                    }
//                                                    currentRow.find("#RequiredQuantity").val(parseFloat(reqQty).toFixed(QtyDecDigit));
//                                                    var PendOrdQty = currentRow.find("#PendingOrderQuantity").val();
//                                                    var OrderToProd = parseFloat(reqQty) - parseFloat(CheckNullNumber(PendOrdQty));
//                                                    OrderToProd = OrderToProd >= 0 ? OrderToProd : 0;
//                                                    currentRow.find("#OrderToProduceQuantity").val(parseFloat(OrderToProd).toFixed(0));
//                                                }
//                                            });
//                                        }
//                                        else {
//                                            var len = $('#SFMaterialDetailsTbl tbody tr').length;
//                                            var BomQty = parseFloat(parseFloat(arr.Table1[j].BOMQty)/* * parseFloat(plannedqty)*/).toFixed(QtyDecDigit);
//                                            //BomQty = parseFloat(parseFloat(BomQty) + (parseFloat(BomQty) * parseFloat(changeInOrdToProd)) / 100).toFixed(QtyDecDigit);
//                                            var reqQty = parseFloat(BomQty) - (parseFloat(arr.Table1[j].AvlStock) + parseFloat(arr.Table1[j].wip_stock)
//                                                + parseFloat(arr.Table1[j].shfl_stock) + parseFloat(arr.Table1[j].Intrst_stock));
//                                            var disablecompdate = "disabled";
//                                            if (reqQty > 0) {
//                                                reqQty = parseFloat(reqQty).toFixed(QtyDecDigit);

//                                            } else {
//                                                reqQty = parseFloat(0).toFixed(QtyDecDigit);
//                                            }
//                                            var OrdToProduce = parseFloat(reqQty) - parseFloat(CheckNullNumber(arr.Table1[j].PendingOrderQty));
//                                            if (OrdToProduce > 0) {
//                                                disablecompdate = "";
//                                            } else {
//                                                OrdToProduce = 0;
//                                            }

//                                            //changeInOrdToProd = ((parseFloat(CheckNullNumber(OrdToProduce)) - parseFloat(BomQty)) / parseFloat(BomQty)) * 100;

//                                            $('#SFMaterialDetailsTbl tbody').append(`
//<tr>
//                                                                <td id="SFSrNo" class="sr_padding">${parseInt(len) + 1}</td>
//                                                                <td>
//                                                                    <!---<div class="col-sm-1">
//                                                                         <div href="#" class="upRow" style="height: 11px;"><i class="fa fa-arrow-up" style="font-size: 11px;"></i></div>
//                                                                         <div href="#" class="downRow" style="height: 11px;"><i class="fa fa-arrow-down" style="font-size: 11px;"></i></div>
//                                                                    </div>--->
//                                                                    <div class="col-sm-10 no-padding">
//                                                                        <input id="MaterialName" class="form-control" type="text" value='${arr.Table1[j].MatrialName}' name="MaterialName" disabled="">
//                                                                        <input type="hidden" id="hf_materialID" value='${arr.Table1[j].MatrialID}'>
//                                                                        <input type="hidden" id="hf_FGProductID" value='${arr.Table1[j].prod_id}'>
//                                                                        <input type="hidden" id="hf_FGID" value='${arr.Table1[j].FG_ItemId}'>
//                                                                        <input type="hidden" id="hf_MaterialType" value='${arr.Table1[j].MaterialType}'>
//                                                                    </div>
//                                                                    <div class="col-sm-1 i_Icon">
//                                                                        <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickRMIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">  </button>
//                                                                    </div>
//                                                                    <div class="col-sm-1 i_Icon">
//                                                                        <button type="button" id="BillOfMaterial" class="calculator no-padding" onclick="OnClickIconBtnSFBOMDetail(event);" data-toggle="modal" data-target="#BillOfMaterial" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/BillOfMaterial.png" alt="" title="${$("#span_BillOfMaterial").text()}"></button>
//                                                                    </div>
//                                                                </td>
//                                                                <td><input id="UOM" class="form-control" value="${arr.Table1[j].uom_name}" type="text" name="UOM" disabled=""><input id="UOMID" type="hidden" value="${arr.Table1[j].uom_id}" /></td>
//                                                                <td><input id="BOMQuantity" class="form-control num_right" type="text" value="${parseFloat(parseFloat(arr.Table1[j].BOMQty) /** parseFloat(plannedqty)*/).toFixed(QtyDecDigit)}" name="BOMQuantity" disabled=""></td>
//                                                                <td>
//                                                                    <div class="col-sm-10 no-padding"><input id="AvailableStock" class="form-control num_right" type="text" value="${parseFloat(arr.Table1[j].AvlStock).toFixed(QtyDecDigit)}" name="AvailableStock" disabled=""></div>
//                                                                    <div class="col-sm-2 i_Icon">
//                                                                        <button type="button" id="StockDetail" class="calculator" onclick="ItemStockWareHouseWise(event,'I')" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}">  </button>
//                                                                    </div>

//                                                                </td>
//                                                                <td>
//                                                                    <div class="col-sm-10 no-padding"><input id="WIPstock" class="form-control num_right" value="${parseFloat(arr.Table1[j].wip_stock).toFixed(QtyDecDigit)}" type="text" name="WIPstock" placeholder="0000.00" disabled></div>
//                                                                    <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="OnClickIconBtnWIPStock(event,'wip')" data-toggle="modal" data-target="#WIPStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
//                                                                </td>
//                                                                <td>
//                                                                    <div class="col-sm-10 no-padding"><input id="ShopFloorstock" class="form-control num_right" value="${parseFloat(arr.Table1[j].shfl_stock).toFixed(QtyDecDigit)}" type="text" name="ShopFloorstock" placeholder="0000.00" disabled></div>
//                                                                    <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="ItemStockShopFloorWise(event)" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
//                                                                </td>
//                                                                <td>
//                                                                    <div class="col-sm-10 no-padding"><input id="INtransit" class="form-control num_right" value="${parseFloat(arr.Table1[j].Intrst_stock).toFixed(QtyDecDigit)}" type="text" name="AvailableStock" placeholder="0000.00" disabled></div>
//                                                                    <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="OnClickIconBtnWIPStock(event,'intrst')" data-toggle="modal" data-target="#WIPStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
//                                                                </td>
//                                                                 <td>
//                                                                    <div class="col-sm-12 no-padding">
//                                                                        <input id="RequiredQuantity" class="form-control num_right" type="text" value="${reqQty}" name="RequiredQuantity" disabled="">
//                                                                    </div>
//                                                                </td>
//                                                                 <td>
//                                                                     <div class="col-sm-10 no-padding">
//                                                                         <input id="PendingOrderQuantity" class="form-control num_right" type="text" value='${parseFloat(arr.Table1[j].PendingOrderQty).toFixed(QtyDecDigit)}' name="PendingOrderQuantity" disabled>
//                                                                     </div>
//                                                                     <div class="col-sm-1 i_Icon">
//                                                                         <button type="button" id="" class="calculator" onclick="OnClickMaterialPendingOrderQty(event,'SF')" data-toggle="modal" data-target="#PendingOrderQuantity" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_PendingOrderQuantity").text()}"></button>
//                                                                     </div>
//                                                                </td>
//                                                                <td>
//                                                                     <div class="col-sm-10 no-padding">
//                                                                    <input id="OrderToProcureQuantity" autocomplete="off" data-initial-value="" onkeypress="return OnKeyPressProcureQty(this,event);" onchange="OnChangeProcureQty(event)" class="form-control num_right" type="text" value="0" name="OrderToProcureQuantity" >
//                                                                    </div>
//                                                                      <div class="col-sm-2 i_Icon" id="div_SubItemOrdrToPrecurQty" style="padding:0px; ">
//                                                                <input hidden type="text" id="sub_item" value="${arr.Table1[j].sub_item}" />
//                                                                <button type="button" id="SubItemOrdrToPrecurQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('OrdrToPrecurQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
//                                                                </div>
// <input hidden type="text" id="HdnSFsubitemFlag" value="OrdrToPrecurQuantity" />
//                                                                </td>
//                                                                <td><div class="lpo_form"><input id="ProcurementCompletionDate" onchange="OnChangeProcureDate(event)" class="form-control num_right" type="date" value="" name="RequisitionQuantity"  disabled=""><span id="ProcurementCompletionDateError" class="error-message is-visible"></span></div></td>
//                                                                <td>
//                                                                <div class="col-sm-10 no-padding"><input id="OrderToProduceQuantity" autocomplete="off" value="${parseFloat(OrdToProduce).toFixed(0)}" onkeypress="return OnKeyPressProduceQty(this,event);" onchange="OnChangeProduceQty(event)" class="form-control num_right" type="text" value="" name="OrderToProduceQuantity" >
//                                                                </div>
//                                                                <div class="col-sm-2 i_Icon" id="div_SubItemOrdrToProduceQty" style="padding:0px; ">
//                                                                <input hidden type="text" id="sub_item" value="${arr.Table1[j].sub_item}" />
//                                                                <button type="button" id="SubItemOrdrToProduceQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('OrdrToProduceQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
//                                                                </div>
// <input hidden type="text" id="HdnSFsubitemFlag" value="OrdrToProduceQuantity" />
//</td>
//                                                                <td><div class="lpo_form"><input id="ProductionCompletionDate" onchange="OnChangeSFProduceDate(event)" class="form-control num_right" type="date" value="" name="RequisitionQuantity" ${disablecompdate}><span id="ProductionCompletionDateError" class="error-message is-visible"></span></div></td>
//                                                                <td>
//                                                                    <div class="col-sm-10 no-padding"><input id="ProcuredQty" class="form-control num_right" value="" type="text" name="" placeholder="0000.00" disabled></div>
//                                                                    <div class="col-sm-1 i_Icon">
//                                                                        <button type="button" id="ProcuredQuantityDetail" class="calculator" onclick="OnClickProcuredQtyIconBtn(event,'SF');" data-toggle="modal" data-target="#ProcuredQuantityDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_ProductionDetail}"> </button>
//                                                                    </div>
//                                                                </td>
//                                                                <td>
//                                                                    <div class="col-sm-10 no-padding"><input id="ProducedQty" class="form-control num_right" value="" type="text" name="" placeholder="0000.00" disabled></div>
//                                                                    <div class="col-sm-1 i_Icon">
//                                                                        <button type="button" id="ProducedQuantityDetail" class="calculator" onclick="OnClickProducedQtyIconBtn(event,'SF');" data-toggle="modal" data-target="#ProducedQuantityDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_ProductionDetail}"> </button>
//                                                                    </div>
//                                                                </td>
//                                                            </tr>`);

//                                        }
//                                    }

//                                    //CalculationReqAndRequisitionQty();
//                                }
//                                for (var j = 0; j < arr.Table4.length; j++) {
//                                    $('#Hdn_RMItemsSFGWise tbody').append(` <tr>
//                                                                <td id="fg_ItemId">${arr.Table4[j].FG_ItemId}</td>
//                                                                <td id="fg_rowno">${arr.Table4[j].Rowno}</td>
//                                                                <td id="Sf_ItemID">${arr.Table4[j].prod_id}</td>
//                                                                <td id="RM_ItemID">${arr.Table4[j].MatrialID}</td>
//                                                                <td id="RM_BomQty">${arr.Table4[j].BOMQty}</td>
//                                                                <td id="RM_ReqQty">${arr.Table4[j].BOMQty}</td>
//                                                            </tr>`);
//                                }
//                                if (arr.Table2.length > 0) {

//                                    for (var j = 0; j < arr.Table2.length; j++) {
//                                        debugger;
//                                        var subitmDisable = "";
//                                        if (arr.Table2[j].sub_item != "Y") {
//                                            subitmDisable = "disabled";
//                                        }
//                                        var ExistMaterialID = "N";
//                                        var materialid = arr.Table2[j].MatrialID;
//                                        var bomqty = arr.Table2[j].BOMQty;


//                                        $("#InputMaterialDetailsTbl >tbody >tr").each(function (i, row) {
//                                            var currentRow = $(this);
//                                            var imd_materialid = currentRow.find("#hf_materialID").val();
//                                            if (imd_materialid === materialid) {
//                                                ExistMaterialID = "Y";
//                                            }
//                                        });

//                                        if (ExistMaterialID === "Y") {
//                                            $("#InputMaterialDetailsTbl >tbody >tr").each(function (i, row) {
//                                                var currentRow = $(this);
//                                                var imdmaterialid = currentRow.find("#hf_materialID").val();

//                                                //$("#SaveProductMaterialTbl >tbody >tr").each(function (i, row) {
//                                                //    var currentRow = $(this);
//                                                //    var material_id = currentRow.find("#MRP_MaterialID").val();
//                                                //    if (material_id === imdmaterialid) {

//                                                //        TotalBomQty = parseFloat(TotalBomQty) + parseFloat(currentRow.find("#MRP_ReqQty").val());
//                                                //    }
//                                                //});

//                                                if (imdmaterialid === materialid) {
//                                                    var TotalBomQty = parseFloat(0);
//                                                    $('#Hdn_RMItemsSFGWise tbody tr #RM_ItemID:contains(' + materialid + ')').closest("tr").each(function () {
//                                                        var currentRow1 = $(this);
//                                                        var fg_BomQty = currentRow1.find("#RM_ReqQty").text();
//                                                        TotalBomQty = parseFloat(TotalBomQty) + parseFloat(fg_BomQty);

//                                                    });
//                                                    //var imdbomqty = currentRow.find("#BOMQuantity").val();
//                                                    //TotalBomQty = parseFloat(parseFloat(arr.Table2[j].BOMQty) + parseFloat(imdbomqty)).toFixed(QtyDecDigit);
//                                                    //currentRow.find("#BOMQuantity").val(parseFloat((parseFloat(bomqty) * parseFloat(plannedqty)) + parseFloat(imdbomqty)).toFixed(QtyDecDigit));
//                                                    currentRow.find("#BOMQuantity").val(parseFloat(TotalBomQty).toFixed(QtyDecDigit));
//                                                    //var ReqQty = parseFloat(TotalBomQty) - (parseFloat(arr.Table2[j].AvlStock) + parseFloat(arr.Table2[j].shfl_stock) + parseFloat(arr.Table2[j].Intrst_stock));
//                                                    //if (ReqQty > 0) {
//                                                    //    currentRow.find("#RequiredQuantity").val(parseFloat(ReqQty).toFixed(QtyDecDigit));
//                                                    //}
//                                                    //else {
//                                                    //    currentRow.find("#RequiredQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
//                                                    //}
//                                                }
//                                            });
//                                        }
//                                        else {
//                                            var ReqQty = parseFloat(arr.Table2[j].BOMQty) - (parseFloat(arr.Table2[j].AvlStock) + parseFloat(arr.Table2[j].shfl_stock) + parseFloat(arr.Table2[j].Intrst_stock));
//                                            var len = $('#InputMaterialDetailsTbl tbody tr').length;

//                                            $('#InputMaterialDetailsTbl tbody').append(`<tr>
//                                        <td id="RMSrNo" class="sr_padding">${parseInt(len) + 1}</td>
                                     
//                                        <td><div class="col-sm-11 no-padding">
//                                        <input id="MaterialName" class="form-control" type="text" value='${arr.Table2[j].MatrialName}' name="MaterialName" disabled>
//                                        <input type="hidden" id="hf_materialID" value="${arr.Table2[j].MatrialID}" /></div><div class="col-sm-1 i_Icon">
//                                        <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickRMIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}"> </button></div></td>
//                                        <td><input id="UOM" class="form-control" value="${arr.Table2[j].uom_name}" type="text" name="UOM" disabled><input id="UOMID" type="hidden" value="${arr.Table2[j].uom_id}" /></td>
//                                        <td><input id="BOMQuantity" class="form-control num_right" type="text" value="${parseFloat(parseFloat(arr.Table2[j].BOMQty) /** parseFloat(plannedqty)*/).toFixed(QtyDecDigit)}" name="BOMQuantity" disabled></td>
//                                        <td><div class="col-sm-10 no-padding"><input id="AvailableStock" class="form-control num_right" type="text" value="${parseFloat(arr.Table2[j].AvlStock).toFixed(QtyDecDigit)}" name="AvailableStock" disabled></div><div class="col-sm-2 i_Icon">
//                                        <button type="button" id="StockDetail" class="calculator" onclick="ItemStockWareHouseWise(event,'I')" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"> </button></div></td>
//                                        <td>
//                                         <div class="col-sm-10 no-padding"><input id="ShopFloorstock" class="form-control num_right" value="${parseFloat(arr.Table2[j].shfl_stock).toFixed(QtyDecDigit)}" type="text" name="ShopFloorstock" placeholder="0000.00" disabled=""></div>
//                                         <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="ItemStockShopFloorWise(event)" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
//                                        </td>
//                                         <td>
//                                            <div class="col-sm-10 no-padding"><input id="INtransit" class="form-control num_right" value="${parseFloat(arr.Table2[j].Intrst_stock).toFixed(QtyDecDigit)}" type="text" name="AvailableStock" placeholder="0000.00" disabled></div>
//                                            <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="OnClickIconBtnWIPStock(event,'intrst')" data-toggle="modal" data-target="#WIPStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
//                                         </td>
//                                        <td>
//                                        <input id="MinimumStockReserve" class="form-control num_right" type="text" value="${parseFloat(arr.Table2[j].MinReqStock).toFixed(QtyDecDigit)}" name="MinimumStockReserve" disabled></td>
//                                        <td><input id="RequiredQuantity" class="form-control num_right" type="text" value="${parseFloat(ReqQty).toFixed(QtyDecDigit)}" name="RequiredQuantity" disabled></td>
//                                        <td>
//                                            <div class="col-sm-10 no-padding">
//                                            <input id="PendingOrderQuantity" class="form-control num_right" type="text" value="${parseFloat(arr.Table2[j].PendingOrderQty).toFixed(QtyDecDigit)}" name="PendingOrderQuantity" disabled>
//                                            </div>
//                                            <div class="col-sm-1 i_Icon">
//                                                <button type="button" id="" class="calculator" onclick="OnClickMaterialPendingOrderQty(event)" data-toggle="modal" data-target="#PendingOrderQuantity" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_PendingOrderQuantity").text()}"></button>
//                                            </div>
//                                        </td>
//                                        <td>
//                                            <div class="col-sm-10 no-padding">
//                                            <input id="RequisitionQuantity" class="form-control num_right" type="text" onkeypress="return OnKeyPressRequisitionQty(this,event);" onchange="OnChangeRequisitionQty(event)" value="${parseFloat(0).toFixed(QtyDecDigit)}" name="RequisitionQuantity">
//                                            </div>
//                                            <div class="col-sm-2 i_Icon" id="div_RMSubItemOrdrToPrecurQty" style="padding:0px; ">
//                                            <input hidden type="text" id="sub_item" value="${arr.Table2[j].sub_item}" />
//                                            <button type="button" id="RMSubItemOrdrToPrecurQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('RMOrdrToPrecurQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
//                                            </div>
//                                            <input hidden type="text" id="HdnRMsubitemFlag" value="RMOrdrToPrecurQuantity" />

//                                        </td>
//                                        <td><div class="lpo_form"><input id="ProcurementCompletionDate" onchange="OnChangeProcureDate(event)" class="form-control num_right" type="date" value="" name="RequisitionQuantity"  disabled=""><span id="ProcurementCompletionDateError" class="error-message is-visible"></span></div></td>
//                                        <td>
//                                              <div class="col-sm-10 no-padding"><input id="ProcuredQty" class="form-control num_right" value="" type="text" name="" placeholder="0000.00" disabled></div>
//                                              <div class="col-sm-1 i_Icon">
//                                              <button type="button" id="ProcuredQuantityDetail" class="calculator" onclick="OnClickProcuredQtyIconBtn(event,'RM');" data-toggle="modal" data-target="#ProcuredQuantityDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_ProductionDetail}"></button>
//                                              </div>
//                                         </td>
//                                    </tr>`);
//                                        }
//                                    }
//                                    CalculationReqAndRequisitionQty();
//                                }

//                                $("#SFMaterialDetailsTbl > tbody > tr #hf_FGID[value='" + productid + "']").closest('tr').each(function () {
//                                    $(this).find("#OrderToProduceQuantity").trigger("change");
//                                });
//                                /*----------------------------Row Move Up And Down--------------------------*/
//                                //$(".upRow,.downRow").click(function () {
//                                //    debugger;
//                                //    var row = $(this).closest("tr");
//                                //    if ($(this).is(".upRow")) {
//                                //        row.insertBefore(row.prev());
//                                //    } else {
//                                //        row.insertAfter(row.next());
//                                //    }
//                                //    $('#SFMaterialDetailsTbl tbody tr').each(function (index) {
//                                //        $(this).find("#SFSrNo").text(index + 1);
//                                //    });
//                                //});
//                                /*----------------------------Row Move Up And Down End--------------------------*/
//                            }
//                            //Loading();
//                        }
//                        catch (ex) {
//                            console.log(ex);
//                            //Loading();
//                        }
                        
//                    },
//                });
    //    }
    }
}

function Loading() {
    CountLoad = CountLoad - 1;
    var per = parseFloat((( TotalLoad-CountLoad )/ TotalLoad)*100).toFixed(2);
    //$("#LoadingMessage").text(per+' %');
    if (CountLoad <= 0) {
        //$("#LoadingMessage").text('');
        hideLoader();
    }
}

async function AddSFAndRM_ItemDetails(data) {
    if (data !== null && data !== "") {
        var arr = [];
        var src_type = $("#ddl_src_type").val();
        arr = JSON.parse(data);
        if (src_type == "F") {
            arr.Table = arr.Table.filter(v => v.BOM_Avl != "N");
        }
        let BtList = [];
        let limit = arr.Table.length;
        let Startlimit = 0;
        let startTime = moment();
        let len = arr.Table.length;
        TotalLoad = len == limit ? 1 : Math.floor(len / limit) + 1;
        CountLoad = TotalLoad;
        var PlanQty = 0;
        
        arr.Table.map(async (item, index) => {
            Startlimit = Startlimit + 1;
            
            if (src_type == "P") {
                PlanQty = item.plan_qty;
            } else {
                PlanQty = parseFloat(item.tgt_qty) - parseFloat(item.Avlstock);
                PlanQty = PlanQty > 0 ? PlanQty : 0;
            }
            if (index == (len - 1)) {
                BtList.push({ sr_no: Startlimit, product_id: item.item_id, qty: PlanQty });
                await AsyncGetSfAndRmData(BtList, startTime,"");
            } else {
                if (Startlimit == limit) {

                    BtList.push({ sr_no: Startlimit, product_id: item.item_id, qty: PlanQty });
                    let newBatchList = BtList;
                    BtList = [];
                    Startlimit = 0;
                    await AsyncGetSfAndRmData(newBatchList, startTime,"");
                    

                } else {
                    BtList.push({ sr_no: Startlimit, product_id: item.item_id, qty: PlanQty });
                }
            }
        })
        
    }
}
async function AsyncGetSfAndRmData(BtList, startTime, flag) {
    try {
        await $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialRequirementPlan/BatchSFAndRMItemDetailPartial",
            data: {
                productList: JSON.stringify(BtList)
            },
            success: function (responce) {
                debugger;
                $("#hdn_data_container").html(responce);
                var data = $("#cmn_data").val();
                $("#hdn_data_container").html("");
                let completeTime = moment();
                duration = moment.duration(completeTime.diff(startTime));
                console.log("Responce : "+duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');
                AddSFAndRM_ItemDetailsToTable(data);
                //$("#SFMaterialDetailsTbl > tbody > tr").on("click", function () {
                   
                //    $("#SFMaterialDetailsTbl > tbody > tr").css("border", "");
                //    $(this).css("border", "2px solid grey");
                //});
                completeTime = moment();
                duration = moment.duration(completeTime.diff(startTime));
                console.log("Added : "+duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');

                if (CountLoad == 1) {
                    //$("#SFMaterialDetailsTbl tbody tr").each(function () {
                    //    var clickedrow = $(this);
                    //    OnChangeProduceQty_row(clickedrow)
                    //});
                    var FgProductId = "";
                    if (flag == "OnProductQtyChange") {
                        var arr = JSON.parse(data);
                        FgProductId = arr.Table[0].product_id;
                    }
                   
                    var updated_sf_items = [];
                    var updated_sf_items_req = [];
                    var child_Rm_Change_items = [];
                    var changeSf = true;
                    $("#SFMaterialDetailsTbl tbody tr").each(function () {
                        var clickedrow = $(this);
                        var Product_id = clickedrow.find("#hf_FGID").val();
                        var SF_ItemId = clickedrow.find("#hf_materialID").val();
                        if (flag == "OnProductQtyChange") {
                            if (arr.Table2.findIndex(v => v.MatrialID == SF_ItemId) > -1) {
                                changeSf = true;
                            } else {
                                changeSf = false;
                            }
                        }
                        if (changeSf) {
                            var RequiredQuantity = 0;//clickedrow.find("#RequiredQuantity").val();
                            var SFbomQty = 0;
                            var ReqQty = 0;
                            var child_sf_items = [];

                            var FGItems = "";
                            ssn_SfData = sessionStorage.ArrSfData_forCalculation;

                            SFItemList = ssn_SfData != null ? JSON.parse(ssn_SfData) : [];

                            let SFItemList_Filtered = SFItemList.filter(v => v.fg_Sf_ItemID == SF_ItemId);
                            //if (flag == "OnProductQtyChange") {
                            //    SFItemList_Filtered = SFItemList_Filtered.filter(v => v.fg_ItemId == FgProductId);
                            //}
                            if (SFItemList_Filtered.length > 1) {
                                var d = SFItemList_Filtered.filter(v => updated_sf_items.includes(v.parent_Sf_ItemID));
                                updated_sf_items_req.push(...d);
                            }
                            updated_sf_items.push(SF_ItemId);
                            SFItemList_Filtered.map((item) => {
                                if (FGItems == "") {
                                    FGItems += item.fg_ItemId;
                                }
                                else {
                                    FGItems += "," + item.fg_ItemId;
                                }
                                var fg_ReqQty = 0;
                                fg_ReqQty = item.fg_ReqQty;
                                ReqQty = item.ReqQty;
                                var fg_BomQty = item.fg_BomQty;
                                SFbomQty = parseFloat(SFbomQty) + parseFloat(fg_BomQty);
                                RequiredQuantity = parseFloat(parseFloat(RequiredQuantity) + parseFloat(fg_ReqQty));//.toFixed(QtyDecDigit);
                            })

                            /*---------- Modified by Suraj Maurya on 15-11-2024 for subtract alailable stock-----*/
                            var avlSkt = CheckNullNumber(clickedrow.find("#AvailableStock").val());
                            var wipSkt = CheckNullNumber(clickedrow.find("#WIPstock").val());
                            var shflSkt = CheckNullNumber(clickedrow.find("#ShopFloorstock").val());
                            var inTransitSkt = CheckNullNumber(clickedrow.find("#INtransit").val());
                            var pendOrdQty = CheckNullNumber(clickedrow.find("#PendingOrderQuantity").val());
                            var TotalAvlSkt = parseFloat(avlSkt) + parseFloat(wipSkt) + parseFloat(shflSkt) + parseFloat(inTransitSkt);
                            var FinReqQty = parseFloat(RequiredQuantity) - parseFloat(TotalAvlSkt);
                            var OrdToProduceQty = parseFloat(FinReqQty) - parseFloat(pendOrdQty);
                            OrdToProduceQty = OrdToProduceQty > 0 ? OrdToProduceQty : 0;
                            FlagReset = "N";
                            FinReqQty = FinReqQty > 0 ? FinReqQty : 0;
                            clickedrow.find("#BOMQuantity").val(parseFloat(RequiredQuantity).toFixed(QtyDecDigit));
                            clickedrow.find("#RequiredQuantity").val(parseFloat(FinReqQty).toFixed(QtyDecDigit));


                            if (parseFloat(OrdToProduceQty).toFixed(0) > parseFloat(0)) {
                                clickedrow.find("#ProductionCompletionDate").attr("disabled", false);
                            } else {
                                clickedrow.find("#ProductionCompletionDate").val("");
                                clickedrow.find("#ProductionCompletionDate").css("border-color", "#ced4da");
                                clickedrow.find("#ProductionCompletionDateError").text("");
                                clickedrow.find("#ProductionCompletionDateError").css("display", "none");
                                clickedrow.find("#ProductionCompletionDate").attr("disabled", true);
                            }
                            clickedrow.find("#OrderToProduceQuantity").val(parseFloat(OrdToProduceQty).toFixed(0));
                            FlagResetHdnSfTable = "Y";
                            let FinOrdToProdQty = clickedrow.find("#OrderToProduceQuantity").val();
                            let ChangePerc = ((parseFloat(CheckNullNumber(FinOrdToProdQty)) - parseFloat(SFbomQty)) / parseFloat(SFbomQty)) * 100;
                            let ChangePerc_bom = ((parseFloat(CheckNullNumber(RequiredQuantity)) - parseFloat(SFbomQty)) / parseFloat(SFbomQty)) * 100;
                            //CalculateRMItems_withSession(ChangePerc, SF_ItemId, FGItems);
                            child_Rm_Change_items.push({ ChangePerc: ChangePerc, SF_ItemId: SF_ItemId, FGItems: FGItems })
                            ResetHdnSfTable_withSession(FGItems, ChangePerc, SF_ItemId, child_sf_items, null, ChangePerc_bom);
                        }
                        

                    });
                    CalculateRMItems_withSession("", "", "", child_Rm_Change_items);
                    updated_sf_items_req.filter((value, index, self) =>
                        index === self.findIndex((t) => (
                            t.fg_Sf_ItemID === value.fg_Sf_ItemID
                        ))
                    ).map((item) => {
                        $("#SFMaterialDetailsTbl tbody tr #hf_materialID[value=" + item.fg_Sf_ItemID + "]").closest('tr').each(function () {
                            var row = $(this);
                            var SFItemList = JSON.parse(sessionStorage.ArrSfData_forCalculation);
                            var RequiredQuantity = 0;
                            SFItemList.filter(v => v.fg_Sf_ItemID == item.fg_Sf_ItemID).map((item) => {
                                var fg_ReqQty = 0;
                                fg_ReqQty = item.fg_ReqQty;
                                RequiredQuantity = parseFloat(parseFloat(RequiredQuantity) + parseFloat(fg_ReqQty));//.toFixed(QtyDecDigit);
                            });
                            var avlSkt = CheckNullNumber(row.find("#AvailableStock").val());
                            var wipSkt = CheckNullNumber(row.find("#WIPstock").val());
                            var shflSkt = CheckNullNumber(row.find("#ShopFloorstock").val());
                            var inTransitSkt = CheckNullNumber(row.find("#INtransit").val());
                            var pendOrdQty = CheckNullNumber(row.find("#PendingOrderQuantity").val());
                            var TotalAvlSkt = parseFloat(avlSkt) + parseFloat(wipSkt) + parseFloat(shflSkt) + parseFloat(inTransitSkt);
                            var FinReqQty = parseFloat(RequiredQuantity) - parseFloat(TotalAvlSkt);
                            var OrdToProduceQty = parseFloat(FinReqQty) - parseFloat(pendOrdQty);
                            OrdToProduceQty = OrdToProduceQty > 0 ? OrdToProduceQty : 0;
                            FlagReset = "N";
                            FinReqQty = FinReqQty > 0 ? FinReqQty : 0;
                            row.find("#BOMQuantity").val(parseFloat(RequiredQuantity).toFixed(QtyDecDigit));
                            row.find("#RequiredQuantity").val(parseFloat(FinReqQty).toFixed(QtyDecDigit));
                            if (parseFloat(OrdToProduceQty).toFixed(0) > parseFloat(0)) {
                                row.find("#ProductionCompletionDate").attr("disabled", false);
                            } else {
                                row.find("#ProductionCompletionDate").val("");
                                row.find("#ProductionCompletionDate").css("border-color", "#ced4da");
                                row.find("#ProductionCompletionDateError").text("");
                                row.find("#ProductionCompletionDateError").css("display", "none");
                                row.find("#ProductionCompletionDate").attr("disabled", true);
                            }
                            row.find("#OrderToProduceQuantity").val(parseFloat(OrdToProduceQty).toFixed(0));
                        });
                    });
                    var changedMaterials;
                    var ssn_RmData = sessionStorage.ArrRmData_forCalculation;
                    let Hdn_RMItemsSFGWise = ssn_RmData != null ? JSON.parse(ssn_RmData) : [];
                    if (flag == "OnProductQtyChange") {
                        changedMaterials = Hdn_RMItemsSFGWise.filter((item) => {
                            if (child_Rm_Change_items.findIndex(v => v.SF_ItemId == item.Sf_ItemID) > -1) {
                                return true;
                            }
                        })
                    }
                    var changeRm = true;
                    $("#InputMaterialDetailsTbl tbody tr").each(function () {
                        var currentRow = $(this);
                        var RM_ItemId = currentRow.find("#hf_materialID").val();
                        var RM_ReqQty = 0;
                        if (flag == "OnProductQtyChange") {
                            if (changedMaterials.findIndex(v => v.RM_ItemID == RM_ItemId) > -1) {
                                changeRm = true;
                            } else {
                                changeRm = false;
                            }
                        }
                        if (changeRm) {

                            Hdn_RMItemsSFGWise.filter(v => v.RM_ItemID == RM_ItemId).map((item) => {
                                var RM_ReqQty1 = item.RM_ReqQty;
                                RM_ReqQty = parseFloat(parseFloat(RM_ReqQty) + parseFloat(RM_ReqQty1)).toFixed(QtyDecDigit);
                            });
                            currentRow.find("#BOMQuantity").val(RM_ReqQty);

                            var RequiredQty = "";
                            var RequisitionQty = "";
                            var imd_bomqty = currentRow.find("#BOMQuantity").val();
                            var imd_avlstk = currentRow.find("#AvailableStock").val();
                            var ShopFloorstock = currentRow.find("#ShopFloorstock").val();
                            var INtransit = currentRow.find("#INtransit").val();
                            var imd_minresvstk = currentRow.find("#MinimumStockReserve").val();
                            var imd_pendingordqty = currentRow.find("#PendingOrderQuantity").val();
                            RequiredQty = parseFloat((parseFloat(imd_bomqty) + parseFloat(imd_minresvstk)) - (parseFloat(imd_avlstk) + parseFloat(ShopFloorstock) + parseFloat(INtransit))).toFixed(QtyDecDigit)
                            RequisitionQty = parseFloat(parseFloat(RequiredQty) - parseFloat(imd_pendingordqty)).toFixed(QtyDecDigit)
                            debugger;
                            if (parseFloat(RequiredQty) <= parseFloat(0)) {
                                RequiredQty = "0";
                            }
                            if (parseFloat(RequisitionQty) <= parseFloat(0)) {
                                RequisitionQty = "0";
                                currentRow.find("#ProcurementCompletionDate").val("");
                                currentRow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
                                currentRow.find("#ProcurementCompletionDateError").text("");
                                currentRow.find("#ProcurementCompletionDateError").css("display", "none");
                                currentRow.find("#ProcurementCompletionDate").attr("disabled", true);
                            } else {
                                currentRow.find("#ProcurementCompletionDate").attr("disabled", false);
                            }
                            currentRow.find("#RequiredQuantity").val(parseFloat(RequiredQty).toFixed(QtyDecDigit));
                            var ItemtypeFlag = $("#InputItemtypeFlag").val();
                            if (ItemtypeFlag == "Y") {
                                currentRow.find("#RequisitionQuantity").val(parseFloat(RequisitionQty).toFixed(QtyDecDigit));
                            }
                            else {
                                currentRow.find("#RequisitionQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                            }

                        }
                        
                    });
                    let completeTime = moment();
                    duration = moment.duration(completeTime.diff(startTime));
                    console.log("Done: " + duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');
                }
            }
        });
        
        Loading();
    }
    catch (ex) {
        console.log(ex);
        Loading();
    }
    

}
function AddSFAndRM_ItemDetailsToTable(data) {

    if (data !== null && data !== "") {
        var arr = [];
        var span_subitemdetail = $("#span_SubItemDetail").text();
        arr = JSON.parse(data);
       
        var ArrSfData = arr.Table4;
        var SFItemList = sessionStorage.ArrSfData_forCalculation;
        if (SFItemList != null) {
            SFItemList = JSON.parse(SFItemList);
        } else {
            SFItemList = [];
        }
        arr.Table.map((item) => {
            SFItemList = SFItemList.filter(u => u.fg_ItemId != item.product_id);
        });

        SFItemList.push(...ArrSfData);
        sessionStorage.setItem("ArrSfData_forCalculation", JSON.stringify(SFItemList));
        var ArrRmData = arr.Table5;
        var RMItemList = sessionStorage.ArrRmData_forCalculation;
        if (RMItemList != null) {
            RMItemList = JSON.parse(RMItemList);
        } else {
            RMItemList = [];
        }
        arr.Table.map((item) => {
            RMItemList = RMItemList.filter(u => u.fg_ItemId != item.product_id);
        });
        RMItemList.push(...ArrRmData);
        sessionStorage.setItem("ArrRmData_forCalculation", JSON.stringify(RMItemList));
      
        if (arr.Table2.length > 0) {
            var ArrSFItem = arr.Table2.filter((value, index, self) => index === self.findIndex((t) => (
                t.MatrialID === value.MatrialID
            )));
            arr.Table2 = ArrSFItem;

            for (var j = 0; j < arr.Table2.length; j++) {
                var subitmDisable = "";
                if (arr.Table2[j].sub_item != "Y") {
                    subitmDisable = "disabled";
                }
                var ExistMaterialID = "N";
                var materialid = arr.Table2[j].MatrialID;
                var bomqty = arr.Table2[j].BOMQty;
                $("#SFMaterialDetailsTbl >tbody >tr #hf_materialID[value=" + materialid + "]").closest('tr').each(function (i, row) {
                    var currentRow = $(this);
                    var imd_materialid = currentRow.find("#hf_materialID").val();
                    if (imd_materialid === materialid) {
                        ExistMaterialID = "Y";
                    }
                });

                if (ExistMaterialID === "Y") {
                    //$("#SFMaterialDetailsTbl >tbody >tr #hf_materialID[value=" + materialid + "]").closest("tr").each(function (i, row) {
                    //    var currentRow = $(this);
                    //    var imdmaterialid = currentRow.find("#hf_materialID").val();

                    //    if (imdmaterialid === materialid) {
                    //        var TotalBomQty = parseFloat(0);

                    //        SFItemList.filter(v => v.fg_Sf_ItemID == imdmaterialid).map((item) => {
                    //            var fg_BomQty = item.fg_ReqQty;
                    //            TotalBomQty = parseFloat(TotalBomQty) + parseFloat(fg_BomQty);
                    //        })
                            
                    //        currentRow.find("#BOMQuantity").val(parseFloat(TotalBomQty).toFixed(QtyDecDigit));
                    //        var reqQty = parseFloat(TotalBomQty) - (parseFloat(arr.Table2[j].AvlStock) + parseFloat(arr.Table2[j].wip_stock) + parseFloat(arr.Table2[j].shfl_stock) + parseFloat(arr.Table2[j].Intrst_stock));

                    //        reqQty = reqQty > 0 ? reqQty : 0;
                    //        if (reqQty == 0) {
                    //            currentRow.find("#ProductionCompletionDate").val("");
                    //            currentRow.find("#ProductionCompletionDate").attr("disabled", true);
                    //        } else {
                    //            currentRow.find("#ProductionCompletionDate").attr("disabled", false);
                    //        }
                    //        currentRow.find("#RequiredQuantity").val(parseFloat(reqQty).toFixed(QtyDecDigit));
                    //        var PendOrdQty = currentRow.find("#PendingOrderQuantity").val();
                    //        var OrderToProd = parseFloat(reqQty) - parseFloat(CheckNullNumber(PendOrdQty));
                    //        OrderToProd = OrderToProd >= 0 ? OrderToProd : 0;
                    //        currentRow.find("#OrderToProduceQuantity").val(parseFloat(OrderToProd).toFixed(0));
                    //    }
                    //});
                }
                else {
                    var len = $('#SFMaterialDetailsTbl tbody tr').length;
                    var BomQty = parseFloat(parseFloat(arr.Table2[j].BOMQty)/* * parseFloat(plannedqty)*/).toFixed(QtyDecDigit);
                    //BomQty = parseFloat(parseFloat(BomQty) + (parseFloat(BomQty) * parseFloat(changeInOrdToProd)) / 100).toFixed(QtyDecDigit);
                    var reqQty = parseFloat(BomQty) - (parseFloat(arr.Table2[j].AvlStock) + parseFloat(arr.Table2[j].wip_stock)
                        + parseFloat(arr.Table2[j].shfl_stock) + parseFloat(arr.Table2[j].Intrst_stock));
                    var disablecompdate = "disabled";
                    if (reqQty > 0) {
                        reqQty = parseFloat(reqQty).toFixed(QtyDecDigit);

                    } else {
                        reqQty = parseFloat(0).toFixed(QtyDecDigit);
                    }
                    var OrdToProduce = parseFloat(reqQty) - parseFloat(CheckNullNumber(arr.Table2[j].PendingOrderQty));
                    if (OrdToProduce > 0) {
                        disablecompdate = "";
                    } else {
                        OrdToProduce = 0;
                    }
                    var DisabledProcureQuantity = "";
                    var subitmDisableProcureQuantity = "";
                    if (arr.Table2[j].i_pur == "Y") {
                        DisabledProcureQuantity = "Enabled";
                        if (arr.Table2[j].sub_item != "Y") {
                            subitmDisableProcureQuantity = "disabled";
                        }
                        else {
                            subitmDisableProcureQuantity = "Enabled";
                        }
                      
                    }
                    else {
                        DisabledProcureQuantity = "disabled";
                        subitmDisableProcureQuantity = "disabled";
                    }
                    $('#SFMaterialDetailsTbl tbody').append(`
<tr>
                                                                <td id="SFSrNo" class="sr_padding">${parseInt(len) + 1}</td>
                                                                <td class="ItmNameBreak itmStick tditemfrz">
                                                                    <div class="col-sm-10 no-padding">
                                                                        <input id="MaterialName" class="form-control" type="text" name="MaterialName" disabled="">
                                                                        <input type="hidden" id="hf_materialID" value='${arr.Table2[j].MatrialID}'>
                                                                        <input type="hidden" id="hf_FGProductID" value='${arr.Table2[j].prod_id}'>
                                                                        <input type="hidden" id="hf_FGID" value='${arr.Table2[j].FG_ItemId}'>
                                                                        <input type="hidden" id="hf_MaterialType" value='${arr.Table2[j].MaterialType}'>
                                                                        <input type="hidden" id="SFItemtypeFlag" value='${arr.Table2[j].i_pur}'>
                                                                    </div>
                                                                    <div class="col-sm-1 i_Icon">
                                                                        <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickRMIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">  </button>
                                                                    </div>
                                                                    <div class="col-sm-1 i_Icon">
                                                                        <button type="button" id="BillOfMaterial" class="calculator no-padding" onclick="OnClickIconBtnSFBOMDetail(event);" data-toggle="modal" data-target="#BillOfMaterial" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/BillOfMaterial.png" alt="" title="${$("#span_BillOfMaterial").text()}"></button>
                                                                    </div>
                                                                </td>
                                                                <td><input id="UOM" class="form-control" value="${arr.Table2[j].uom_name}" type="text" name="UOM" disabled=""><input id="UOMID" type="hidden" value="${arr.Table2[j].uom_id}" /></td>
                                                                <td><input id="BOMQuantity" class="form-control num_right" type="text" value="${parseFloat(parseFloat(arr.Table2[j].BOMQty) /** parseFloat(plannedqty)*/).toFixed(QtyDecDigit)}" name="BOMQuantity" disabled=""></td>
                                                                <td>
                                                                    <div class="col-sm-10 no-padding"><input id="AvailableStock" class="form-control num_right" type="text" value="${parseFloat(arr.Table2[j].AvlStock).toFixed(QtyDecDigit)}" name="AvailableStock" disabled=""></div>
                                                                    <div class="col-sm-2 i_Icon">
                                                                        <button type="button" id="StockDetail" class="calculator" onclick="ItemStockWareHouseWise(event,'I')" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}">  </button>
                                                                    </div>

                                                                </td>
                                                                <td>
                                                                    <div class="col-sm-10 no-padding"><input id="WIPstock" class="form-control num_right" value="${parseFloat(arr.Table2[j].wip_stock).toFixed(QtyDecDigit)}" type="text" name="WIPstock" placeholder="0000.00" disabled></div>
                                                                    <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="OnClickIconBtnWIPStock(event,'wip')" data-toggle="modal" data-target="#WIPStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
                                                                </td>
                                                                <td>
                                                                    <div class="col-sm-10 no-padding"><input id="ShopFloorstock" class="form-control num_right" value="${parseFloat(arr.Table2[j].shfl_stock).toFixed(QtyDecDigit)}" type="text" name="ShopFloorstock" placeholder="0000.00" disabled></div>
                                                                    <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="ItemStockShopFloorWise(event)" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
                                                                </td>
                                                                <td>
                                                                    <div class="col-sm-10 no-padding"><input id="INtransit" class="form-control num_right" value="${parseFloat(arr.Table2[j].Intrst_stock).toFixed(QtyDecDigit)}" type="text" name="AvailableStock" placeholder="0000.00" disabled></div>
                                                                    <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="OnClickIconBtnWIPStock(event,'intrst')" data-toggle="modal" data-target="#WIPStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
                                                                </td>
                                                                 <td>
                                                                    <div class="col-sm-12 no-padding">
                                                                        <input id="RequiredQuantity" class="form-control num_right" type="text" value="${reqQty}" name="RequiredQuantity" disabled="">
                                                                    </div>
                                                                </td>
                                                                 <td>
                                                                     <div class="col-sm-10 no-padding">
                                                                         <input id="PendingOrderQuantity" class="form-control num_right" type="text" value='${parseFloat(arr.Table2[j].PendingOrderQty).toFixed(QtyDecDigit)}' name="PendingOrderQuantity" disabled>
                                                                     </div>
                                                                     <div class="col-sm-1 i_Icon">
                                                                         <button type="button" id="" class="calculator" onclick="OnClickMaterialPendingOrderQty(event,'SF')" data-toggle="modal" data-target="#PendingOrderQuantity" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_PendingOrderQuantity").text()}"></button>
                                                                     </div>
                                                                </td>
                                                                <td>
                                                                     <div class="col-sm-10 no-padding">
                                                                    <input id="OrderToProcureQuantity" ${DisabledProcureQuantity} autocomplete="off" data-initial-value="" onkeypress="return OnKeyPressProcureQty(this,event);" onchange="OnChangeProcureQty(event)" class="form-control num_right" type="text" value="0" name="OrderToProcureQuantity" >
                                                                    </div>
                                                                      <div class="col-sm-2 i_Icon" id="div_SubItemOrdrToPrecurQty" style="padding:0px; " >
                                                                <input hidden type="text" id="sub_item" value="${arr.Table2[j].sub_item}" />
                                                                <button type="button" id="SubItemOrdrToPrecurQty" ${subitmDisableProcureQuantity}  class="calculator subItmImg" onclick="return SubItemDetailsPopUp('OrdrToPrecurQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
                                                                </div>
                                                                    <input hidden type="text" id="HdnSFsubitemFlag" value="OrdrToPrecurQuantity" />
                                                                </td>
                                                                <td><div class="lpo_form"><input id="ProcurementCompletionDate" ${DisabledProcureQuantity} onchange="OnChangeProcureDate(event)" class="form-control num_right" type="date" value="" name="RequisitionQuantity"  disabled=""><span id="ProcurementCompletionDateError" class="error-message is-visible"></span></div></td>
                                                                <td>
                                                                <div class="col-sm-10 no-padding"><input id="OrderToProduceQuantity" autocomplete="off" value="${parseFloat(OrdToProduce).toFixed(0)}" onkeypress="return OnKeyPressProduceQty(this,event);" onchange="OnChangeProduceQty(event)" class="form-control num_right" type="text" value="" name="OrderToProduceQuantity" >
                                                                </div>
                                                                <div class="col-sm-2 i_Icon" id="div_SubItemOrdrToProduceQty" style="padding:0px; ">
                                                                <input hidden type="text" id="sub_item" value="${arr.Table2[j].sub_item}" />
                                                                <button type="button" id="SubItemOrdrToProduceQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('OrdrToProduceQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
                                                                </div>
                                                                 <input hidden type="text" id="HdnSFsubitemFlag" value="OrdrToProduceQuantity" />
                                                                </td>
                                                                <td><div class="lpo_form"><input id="ProductionCompletionDate" onchange="OnChangeSFProduceDate(event)" class="form-control num_right" type="date" value="" name="RequisitionQuantity" ${disablecompdate}><span id="ProductionCompletionDateError" class="error-message is-visible"></span></div></td>
                                                                <td>
                                                                    <div class="col-sm-10 no-padding"><input id="ProcuredQty" class="form-control num_right" value="" type="text" name="" placeholder="0000.00" disabled></div>
                                                                    <div class="col-sm-1 i_Icon">
                                                                        <button type="button" id="ProcuredQuantityDetail" class="calculator" onclick="OnClickProcuredQtyIconBtn(event,'SF');" data-toggle="modal" data-target="#ProcuredQuantityDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_ProductionDetail}"> </button>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div class="col-sm-10 no-padding"><input id="ProducedQty" class="form-control num_right" value="" type="text" name="" placeholder="0000.00" disabled></div>
                                                                    <div class="col-sm-1 i_Icon">
                                                                        <button type="button" id="ProducedQuantityDetail" class="calculator" onclick="OnClickProducedQtyIconBtn(event,'SF');" data-toggle="modal" data-target="#ProducedQuantityDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_ProductionDetail}"> </button>
                                                                    </div>
                                                                </td>
                                                            </tr>`);
                    $('#SFMaterialDetailsTbl tbody tr #hf_materialID[value=' + arr.Table2[j].MatrialID + ']').closest('tr')
                        .find("#MaterialName").val(arr.Table2[j].MatrialName);
                }
            }

        }
        
        if (arr.Table3.length > 0) {

            for (var j = 0; j < arr.Table3.length; j++) {
                debugger;
                var subitmDisable = "";
                if (arr.Table3[j].sub_item != "Y") {
                    subitmDisable = "disabled";
                }
                var ExistMaterialID = "N";
                var materialid = arr.Table3[j].MatrialID;
                var bomqty = arr.Table3[j].BOMQty;

                $("#InputMaterialDetailsTbl >tbody >tr #hf_materialID[value=" + materialid + "]").closest('tr').each(function (i, row) {
                    var currentRow = $(this);
                    var imd_materialid = currentRow.find("#hf_materialID").val();
                    if (imd_materialid === materialid) {
                        ExistMaterialID = "Y";
                    }
                });

                if (ExistMaterialID === "Y") {
                    //$("#InputMaterialDetailsTbl >tbody >tr").each(function (i, row) {
                    //    var currentRow = $(this);
                    //    var imdmaterialid = currentRow.find("#hf_materialID").val();

                    //    if (imdmaterialid === materialid) {
                    //        var TotalBomQty = parseFloat(0);
                    //        $('#Hdn_RMItemsSFGWise tbody tr #RM_ItemID:contains(' + materialid + ')').closest("tr").each(function () {
                    //            var currentRow1 = $(this);
                    //            var fg_BomQty = currentRow1.find("#RM_ReqQty").text();
                    //            TotalBomQty = parseFloat(TotalBomQty) + parseFloat(fg_BomQty);

                    //        });
                    //        currentRow.find("#BOMQuantity").val(parseFloat(TotalBomQty).toFixed(QtyDecDigit));
                    //    }
                    //});
                }
                else {
                    var ReqQty = parseFloat(arr.Table3[j].BOMQty) - (parseFloat(arr.Table3[j].AvlStock) + parseFloat(arr.Table3[j].shfl_stock) + parseFloat(arr.Table3[j].Intrst_stock));
                    var len = $('#InputMaterialDetailsTbl tbody tr').length;

                    var DisabledProcureQuantity = "";
                    var subitmDisableProcureQuantity = "";
                    if (arr.Table3[j].i_pur === "Y") {
                        DisabledProcureQuantity = "";  // enabled (no attribute)
                        if (arr.Table3[j].sub_item !== "Y") {
                            subitmDisableProcureQuantity = "disabled";  // disable
                        } else {
                            subitmDisableProcureQuantity = ""; // enabled
                        }
                    } else {
                        DisabledProcureQuantity = "disabled";
                        subitmDisableProcureQuantity = "disabled";
                    }

                    $('#InputMaterialDetailsTbl tbody').append(`<tr>
                                        <td id="RMSrNo" class="sr_padding">${parseInt(len) + 1}</td>
                                     
                                        <td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-11 no-padding">
                                        <input id="MaterialName" class="form-control" type="text" name="MaterialName" disabled>
                                        <input type="hidden" id="hf_materialID" value="${arr.Table3[j].MatrialID}" />
<input type="hidden" id="InputItemtypeFlag" value='${arr.Table3[j].i_pur}'>
</div><div class="col-sm-1 i_Icon">
                                        <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickRMIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}"> </button></div></td>
                                        <td><input id="UOM" class="form-control" value="${arr.Table3[j].uom_name}" type="text" name="UOM" disabled><input id="UOMID" type="hidden" value="${arr.Table3[j].uom_id}" /></td>
                                        <td><input id="BOMQuantity" class="form-control num_right" type="text" value="${parseFloat(parseFloat(arr.Table3[j].BOMQty) /** parseFloat(plannedqty)*/).toFixed(QtyDecDigit)}" name="BOMQuantity" disabled></td>
                                        <td><div class="col-sm-10 no-padding"><input id="AvailableStock" class="form-control num_right" type="text" value="${parseFloat(arr.Table3[j].AvlStock).toFixed(QtyDecDigit)}" name="AvailableStock" disabled></div><div class="col-sm-2 i_Icon">
                                        <button type="button" id="StockDetail" class="calculator" onclick="ItemStockWareHouseWise(event,'I')" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"> </button></div></td>
                                        <td>
                                         <div class="col-sm-10 no-padding"><input id="ShopFloorstock" class="form-control num_right" value="${parseFloat(arr.Table3[j].shfl_stock).toFixed(QtyDecDigit)}" type="text" name="ShopFloorstock" placeholder="0000.00" disabled=""></div>
                                         <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="ItemStockShopFloorWise(event)" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
                                        </td>
                                         <td>
                                            <div class="col-sm-10 no-padding"><input id="INtransit" class="form-control num_right" value="${parseFloat(arr.Table3[j].Intrst_stock).toFixed(QtyDecDigit)}" type="text" name="AvailableStock" placeholder="0000.00" disabled></div>
                                            <div class="col-sm-1 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="OnClickIconBtnWIPStock(event,'intrst')" data-toggle="modal" data-target="#WIPStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_StockDetail}"></button></div>
                                         </td>
                                        <td>
                                        <input id="MinimumStockReserve" class="form-control num_right" type="text" value="${parseFloat(arr.Table3[j].MinReqStock).toFixed(QtyDecDigit)}" name="MinimumStockReserve" disabled></td>
                                        <td><input id="RequiredQuantity" class="form-control num_right" type="text" value="${parseFloat(ReqQty).toFixed(QtyDecDigit)}" name="RequiredQuantity" disabled></td>
                                        <td>
                                            <div class="col-sm-10 no-padding">
                                            <input id="PendingOrderQuantity" class="form-control num_right" type="text" value="${parseFloat(arr.Table3[j].PendingOrderQty).toFixed(QtyDecDigit)}" name="PendingOrderQuantity" disabled>
                                            </div>
                                            <div class="col-sm-1 i_Icon">
                                                <button type="button" id="" class="calculator" onclick="OnClickMaterialPendingOrderQty(event)" data-toggle="modal" data-target="#PendingOrderQuantity" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_PendingOrderQuantity").text()}"></button>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="col-sm-10 no-padding">
                                            <input id="RequisitionQuantity" class="form-control num_right" ${DisabledProcureQuantity} type="text" onkeypress="return OnKeyPressRequisitionQty(this,event);" onchange="OnChangeRequisitionQty(event)" value="${parseFloat(0).toFixed(QtyDecDigit)}" name="RequisitionQuantity">
                                            </div>
                                            <div class="col-sm-2 i_Icon" id="div_RMSubItemOrdrToPrecurQty" style="padding:0px; ">
                                            <input hidden type="text" id="sub_item" value="${arr.Table3[j].sub_item}" />
                                            <button type="button" id="RMSubItemOrdrToPrecurQty" ${subitmDisableProcureQuantity} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('RMOrdrToPrecurQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
                                            </div>
                                            <input hidden type="text" id="HdnRMsubitemFlag" value="RMOrdrToPrecurQuantity" />

                                        </td>
                                        <td><div class="lpo_form"><input id="ProcurementCompletionDate"   onchange="OnChangeProcureDate(event)" ${subitmDisableProcureQuantity} class="form-control num_right" type="date" value="" name="RequisitionQuantity" ><span id="ProcurementCompletionDateError" class="error-message is-visible"></span></div></td>
                                        <td>
                                              <div class="col-sm-10 no-padding"><input id="ProcuredQty" class="form-control num_right" value="" type="text" name="" placeholder="0000.00" disabled></div>
                                              <div class="col-sm-1 i_Icon">
                                              <button type="button" id="ProcuredQuantityDetail" class="calculator" onclick="OnClickProcuredQtyIconBtn(event,'RM');" data-toggle="modal" data-target="#ProcuredQuantityDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_ProductionDetail}"></button>
                                              </div>
                                         </td>
                                    </tr>`);
                    $('#InputMaterialDetailsTbl > tbody > tr #hf_materialID[value=' + arr.Table3[j].MatrialID + ']').closest('tr')
                        .find("#MaterialName").val(arr.Table3[j].MatrialName);
                }
            }
            CalculationReqAndRequisitionQty();
        }

        
        /*----------------------------Row Move Up And Down--------------------------*/
        //$(".upRow,.downRow").click(function () {
        //    debugger;
        //    var row = $(this).closest("tr");
        //    if ($(this).is(".upRow")) {
        //        row.insertBefore(row.prev());
        //    } else {
        //        row.insertAfter(row.next());
        //    }
        //    $('#SFMaterialDetailsTbl tbody tr').each(function (index) {
        //        $(this).find("#SFSrNo").text(index + 1);
        //    });
        //});
        /*----------------------------Row Move Up And Down End--------------------------*/
    }
}
function CalculationReqAndRequisitionQty() {
    debugger
    debugger;
    $("#InputMaterialDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var RequiredQty = "";
        var RequisitionQty = "";
        var imd_bomqty = currentRow.find("#BOMQuantity").val();
        var imd_avlstk = currentRow.find("#AvailableStock").val();
        var ShopFloorstock = currentRow.find("#ShopFloorstock").val();
        var INtransit = currentRow.find("#INtransit").val();
        var imd_minresvstk = currentRow.find("#MinimumStockReserve").val();
        var imd_pendingordqty = currentRow.find("#PendingOrderQuantity").val();
        RequiredQty = parseFloat((parseFloat(imd_bomqty) + parseFloat(imd_minresvstk)) - (parseFloat(imd_avlstk) + parseFloat(ShopFloorstock) + parseFloat(INtransit))).toFixed(QtyDecDigit)
        RequisitionQty = parseFloat(parseFloat(RequiredQty) - parseFloat(imd_pendingordqty)).toFixed(QtyDecDigit)
        debugger;
        if (parseFloat(RequiredQty) <= parseFloat(0)) {
            RequiredQty = "0";
        }
        if (parseFloat(RequisitionQty) <= parseFloat(0)) {
            RequisitionQty = "0";
            currentRow.find("#ProcurementCompletionDate").val("");
            currentRow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
            currentRow.find("#ProcurementCompletionDateError").text("");
            currentRow.find("#ProcurementCompletionDateError").css("display", "none");
            currentRow.find("#ProcurementCompletionDate").attr("disabled", true);
        } else {
            currentRow.find("#ProcurementCompletionDate").attr("disabled", false);
        }
        currentRow.find("#RequiredQuantity").val(parseFloat(RequiredQty).toFixed(QtyDecDigit));

        var ItemtypeFlag = $("#InputItemtypeFlag").val();
        if (ItemtypeFlag == "Y") {
            currentRow.find("#RequisitionQuantity").val(parseFloat(RequisitionQty).toFixed(QtyDecDigit));
        }
        else {
            currentRow.find("#RequisitionQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        }
       

    });
}
function OnKeyPressPlannedQty(el, evt) {
    if (Cmn_IntValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#PlannedQtyError").css("display", "none");
    clickedrow.find("#PlannedQuantity").css("border-color", "#ced4da");

    return true;
}
function OnKeyPressRequisitionQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
      return true;
}
function OnChangeRequisitionQty(evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var Requisitionqty = clickedrow.find("#RequisitionQuantity").val();

    if ((CheckNullNumber(Requisitionqty)== 0)) {
        Requisitionqty = "0";
        clickedrow.find("#ProcurementCompletionDate").val("");
        clickedrow.find("#ProcurementCompletionDate").attr("disabled", true);

    } else {
        clickedrow.find("#ProcurementCompletionDate").attr("disabled", false);
    }
    clickedrow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
    clickedrow.find("#ProcurementCompletionDateError").text("");
    clickedrow.find("#ProcurementCompletionDateError").css("display", "none");
    var ItemtypeFlag = $("#InputItemtypeFlag").val();
    if (ItemtypeFlag == "Y") {
        clickedrow.find("#RequisitionQuantity").val(parseFloat(Requisitionqty).toFixed(QtyDecDigit));
    }
    else {
        clickedrow.find("#RequisitionQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#hfsno").val();
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfproductID").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickRMIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hf_materialID").val();
    ItemInfoBtnClick(ItmCode);
}
function functionConfirm(event) {
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
            $("#HdActionCommand").val("Delete");
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function OnClickCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr('onclick', "return CheckFormValidation();");
        $("#btn_save").attr('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr('onclick', "");
        $("#btn_save").attr('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}


function BindPPNumberList() {
    debugger;
    var RequisitionArea = $('#ddlRequiredArea').val();
    if (RequisitionArea == "" || RequisitionArea == "0") {
        document.getElementById("vmRequiredArea").innerHTML = $("#valueReq").text();
        $("#vmRequiredArea").css("display", "block");
        $("#ddlRequiredArea").css("border-color", "red");
        return false;
    }
    else {
        $("#vmRequiredArea").css("display", "none");
        $("#ddlRequiredArea").css("border-color", "#ced4da");
    }
    $("#PP_Number").select2({
        ajax: {
            url: "/ApplicationLayer/MaterialRequirementPlan/GetPPNumberList",
            data: function (params) {
                var queryParameters = {
                    PP_Number: params.term,
                    RequisitionArea, RequisitionArea
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                var pageSize,
                    pageSize = 20;// or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-4 col-xs-6 def-cursor">${$("#DocNo").text()}</div>
<div class="col-md-4 col-xs-6 def-cursor">${$("#DocDate").text()}</div>
<div class="col-md-4 col-xs-6 def-cursor">${$("#span_RequirementArea").text()}</div>
</div>
</strong></li></ul>`)
                }
                var page = params.page || 1;
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        debugger;
                        if (Fdata[0].ID.trim() != "---Select---") {
                            var select = { ID: " ---Select---", Name: " ---Select---|''| ---Select---", };
                            Fdata.unshift(select);
                        }
                    }
                }

                //return {
                //    results: $.map(Fdata, function (val, Item) {
                //        debugger;
                //        return { id: val.Name == "0" ? val.Name : val.ID + '_' + val.Name.split("_")[1], text: val.ID, UOM: val.Name.split("_")[0], req: val.Name.split("_")[2] };
                //    }),
                //    pagination: {
                //        more: (page * pageSize) < data.length
                //    }
                //};
                return {
                    results: $.map(Fdata, function (val, Item) {
                        debugger; // Pauses execution for debugging

                        // Extract values from val.Name assumed to be in format "UOM_Name_req"
                        const nameParts = val.Name.split("|");

                        return {
                            id: val.Name === "0" ? val.Name : val.ID + '|' + nameParts[1], // ID + NamePart if Name is not "0"
                            text: val.ID,                   // Display text
                            UOM: nameParts[0],              // First part (e.g., Unit of Measure)
                            req: nameParts[2]               // Third part (e.g., required flag/value)
                        };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length // For infinite scrolling/pagination
                    }
                };
            }
        },
        templateResult: function (data) {

            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-4 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-4 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '<div class="col-md-4 col-xs-6' + classAttr + '">' + data.req + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },

    });

}
function OnChangePP_Number() {
    debugger;
    var pp_number = $("#PP_Number").val();
    
    debugger;
    if (pp_number == "0" || pp_number == "") {
        
    } else {
        $("#PP_Number").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-PP_Number-container']").css("border-color", "#ced4da");
        $("#vm_PP_Number").css("display", "none");
        $("#vm_PP_Number").text("");
    }
    if (pp_number != "0") {
        var number = pp_number.split('|')[0];
        var pp_Date = pp_number.split('|')[1];
        $("#HdnPP_Number").val(number);
        $("#PP_Date").val(pp_Date);
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialRequirementPlan/GetPPNumberDetail",
            data: { PP_Number: number, PP_Date: pp_Date },
            dataType: "json",
            success: function (data) {
                debugger;
                var arr = [];
                arr = JSON.parse(data);
                var fy = arr.Table[0].f_fy;
                $("#ddl_financial_year").attr("disabled", true);
                $("#ddl_period").attr("disabled", true);
                if (fy == "0") {
                    
                    $("#div_ppfy").css("display", "none");
                    $("#div_ppPeriod").css("display", "none");
                    $("#div_ppDtRg").css("display", "none");
                    
                    $("#ddl_financial_year").val(0);
                    $("#hdn_ddl_financial_year").val(0);
                    $("#ddl_period").val(0);
                    $("#hdn_ddl_period").val(0);
                    $("#txtFromDate").val("0");
                    $("#txtToDate").val("0");
                    $("#src_doc_type").val(arr.Table[0].src_type);
                    if (arr.Table[0].src_type == "A") {
                        $("#AdHocdiv_ppDtRg").css("display", "block");
                        $("#AH_txtFromDate").val(arr.Table[0].from_date).attr("readonly",true);
                        $("#AH_txtToDate").val(arr.Table[0].to_date).attr("readonly", true);;
                    }
                }
                else {
                    var f_period = arr.Table[0].f_period;
                    $("#AdHocdiv_ppDtRg").css("display", "none");
                    $("#div_ppfy").css("display", "block");
                    $("#div_ppPeriod").css("display", "block");
                    $("#div_ppDtRg").css("display", "block");
                    $("#ddl_financial_year").val(fy).change();
                    $("#hdn_ddl_period").val(f_period);
                 //.change();
                    //$("#ddl_financial_year").attr("disabled", true);
                    //$("#ddl_period").attr("disabled", true);
                }
            }
        })
    }
}
function SetPeriodOnchangePPNo() {
    var f_period = $("#hdn_ddl_period").val()
    $("#ddl_period").val(f_period).change();
}


function ItemStockShopFloorWise(evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        //var sno = "";
        var ItemId = "";
        var ItemName = "";
        var UOMName = "";

        //sno = clickedrow.find("#hfsno").val();
        ItemId = clickedrow.find("#hf_materialID").val();
        ItemName = clickedrow.find("#MaterialName").val();
        UOMName = clickedrow.find("#UOM").val();
        UOMID = clickedrow.find("#UOMID").val();

        $.ajax(
            {
                type: "Post",
                url: "/Common/Common/getItemstockShopFloorWise",
                data: { ItemId: ItemId, UomId: UOMID },
                success: function (data) {
                    debugger;
                    $('#ItemStockWareHouseWise').html(data);
                    $("#WareHouseWiseItemName").val(ItemName);
                    $("#WareHouseWiseUOM").val(UOMName);
                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickIconBtnWIPStock(e, StockType) {
    var Crow = $(e.target).closest("tr");
    $("#TablePendingOrderQty tbody tr").remove();
    var ItemId = Crow.find("#hf_materialID").val();
    //var hfsno = Crow.find("#hfsno").val();
    var ItemName = Crow.find("#MaterialName").val();
    var UOM = Crow.find("#UOM").val();
    var UOMID = Crow.find("#UOMID").val();
    //var PendingOrderQuantity = Crow.find("#PendingOrderQuantity").val();
    $("#POQ_ProductName").val(ItemName);
    $("#POQ_UOM").val(UOM);
    //$("#POQ_PendingOrderQuantity").val(PendingOrderQuantity);
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ProductionPlan/GetPendingStockItemWise",
        data: {
            ItemId: ItemId,
            ItemName: ItemName,
            UOM: UOM,
            StockType: StockType,
            UomId: UOMID
        },
        success: function (data) {
            debugger;
            $("#ItemPendingStock").html(data);

        }
    });
}
function OnClickIconBtnBOMDetail(e) {
    var Crow = $(e.target).closest("tr");
    $("#PP_BOMDetail tbody tr").remove();
    var ItemId = Crow.find("#hfproductID").val();

    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ProductionPlan/GetBOMDetailsItemWise",
            data: { ItemId: ItemId },
            //dataType: "json",
            success: function (data) {
                debugger;
                $("#PPLPopUp_BillOfMaterial").html(data);

            },
        });
}


function OnClickIconBtnSFBOMDetail(e) {
    var Crow = $(e.target).closest("tr");
    $("#PP_BOMDetail tbody tr").remove();
    var FGItemId = Crow.find("#hf_FGProductID").val();
    var ItemId = Crow.find("#hf_materialID").val();
    var Mtype = Crow.find("#hf_MaterialType").val();
    if (Mtype == "IR") {
        FGItemId = ItemId;
        ItemId = "";
    }
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/MaterialRequirementPlan/GetSFBOMDetailsItemWise",
            data: { FGItemId: FGItemId, SFItemId: ItemId },
            //dataType: "json",
            success: function (data) {
                debugger;
                $("#PPLPopUp_BillOfMaterial").html(data);

            },
        });
}
function OnChangeProduceQty(evt) {
    var clickedrow = $(evt.target).closest("tr");
    let startTime = moment();

    OnChangeProduceQty_row(clickedrow);
    let completeTime = moment();
    let duration = moment.duration(completeTime.diff(startTime));
    console.log("Data Updated in "+duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');
}
 function OnChangeProduceQty_row(clickedrow,flag) {
    debugger
    try {
        showLoader();
    var Product_id = clickedrow.find("#hf_FGID").val();
    var SF_ItemId = clickedrow.find("#hf_materialID").val();
    var ProduceQty = clickedrow.find("#OrderToProduceQuantity").val();
    var RequiredQuantity = clickedrow.find("#RequiredQuantity").val();
    //var ProcureQty = clickedrow.find("#OrderToProcureQuantity").val();
    if (AvoidDot(ProduceQty) == false) {
        
        clickedrow.find("#OrderToProduceQuantity").val(parseFloat(0).toFixed(0));
        clickedrow.find("#ProductionCompletionDate").attr("disabled", true);
        clickedrow.find("#ProductionCompletionDate").val("");
        clickedrow.find("#ProductionCompletionDate").css("border-color", "#ced4da");
        clickedrow.find("#ProductionCompletionDateError").text("");
        clickedrow.find("#ProductionCompletionDateError").css("display", "none");
    } else {

            clickedrow.find("#OrderToProduceQuantityError").text("");
            clickedrow.find("#OrderToProduceQuantityError").css("display", "none");
            clickedrow.find("#OrderToProduceQuantity").css("border-color", "#ced4da");
            clickedrow.find("#OrderToProduceQuantity").val(parseFloat(ProduceQty).toFixed(0));
            clickedrow.find("#ProductionCompletionDate").attr("disabled", false);
        
    }
        //CalcRowMaterialForSFItems(SF_ItemId, ProduceQty)
        CalcRowMaterialForSFItems_withSession(SF_ItemId, ProduceQty, flag);
        hideLoader();
    }
    catch (ex) {
        hideLoader();
    }
   
}
function checkArrLength(arr) {
    if (arr != null) {
        if (arr.length > 0) {
            return true;
        }
    }
    false;
}
function CalcRowMaterialForSFItems_withSession(SF_Item_id, ProduceQuantity,flag) {
    debugger
    //var ApplyFromRow = 0;
    var ChangePerc = 0;
    var bomQty = 0;
    var FGItems = "";
    var ssn_SfData = sessionStorage.ArrSfData_forCalculation;
    var SFItemList = ssn_SfData != null ? JSON.parse(ssn_SfData):[];
    if (checkArrLength(SFItemList)) {
        let SFItemList_Filtered = SFItemList.filter(v => v.fg_Sf_ItemID == SF_Item_id);

        SFItemList_Filtered.map((item) => {
            var fg_ItemId = item.fg_ItemId;
            var fg_rowno = item.fg_rowno;
            var fg_Sf_ItemID = item.fg_Sf_ItemID;
            var fg_BomQty = item.fg_BomQty;
            var fg_ReqQty = item.fg_ReqQty;
            if (SF_Item_id == fg_Sf_ItemID) {
                //ApplyFromRow = fg_rowno;
                if (FGItems == "") {
                    FGItems += fg_ItemId;
                }
                else {
                    FGItems += "," + fg_ItemId;
                }
                bomQty = parseFloat(bomQty) + parseFloat(fg_BomQty);
            }
        })
    }
   
    ChangePerc = ((parseFloat(CheckNullNumber(ProduceQuantity)) - parseFloat(bomQty)) / parseFloat(bomQty)) * 100;

    var child_sf_items = [];
    var child_Rm_Change_items = [];
    ResetHdnSfTable_withSession(FGItems, ChangePerc, SF_Item_id, child_sf_items);
    FlagApply = "N";
    var FlagResetHdnSfTable = "N";
    child_sf_items.filter((value, index, self) =>
        index === self.findIndex((t) => (
            t === value
        ))
    ).map((item) => {
        $("#SFMaterialDetailsTbl tbody tr #hf_materialID[value=" + item + "]").closest('tr').each(function () {
            var clickedrow = $(this);
            var Product_id = clickedrow.find("#hf_FGID").val();
            var SF_ItemId = clickedrow.find("#hf_materialID").val();
            if (!child_sf_items.includes(SF_ItemId)) {
                return;
            }
            var RequiredQuantity = 0;//clickedrow.find("#RequiredQuantity").val();
            var SFbomQty = 0;
            var ssn_SfData = sessionStorage.ArrSfData_forCalculation;
            SFItemList = ssn_SfData != null ? JSON.parse(ssn_SfData):[];
            let SFItemList_Filtered = SFItemList.filter(v => v.fg_Sf_ItemID == SF_ItemId);
            SFItemList_Filtered.map((item) => {
                var fg_ReqQty = 0;
                fg_ReqQty = item.fg_ReqQty;
                var fg_BomQty = item.fg_BomQty;
                SFbomQty = parseFloat(SFbomQty) + parseFloat(fg_BomQty);
                RequiredQuantity = parseFloat(parseFloat(RequiredQuantity) + parseFloat(fg_ReqQty));//.toFixed(QtyDecDigit);
            })
            /*---------- Modified by Suraj Maurya on 15-11-2024 for subtract alailable stock-----*/
            var avlSkt = CheckNullNumber(clickedrow.find("#AvailableStock").val());
            var wipSkt = CheckNullNumber(clickedrow.find("#WIPstock").val());
            var shflSkt = CheckNullNumber(clickedrow.find("#ShopFloorstock").val());
            var inTransitSkt = CheckNullNumber(clickedrow.find("#INtransit").val());
            var pendOrdQty = CheckNullNumber(clickedrow.find("#PendingOrderQuantity").val());
            var TotalAvlSkt = parseFloat(avlSkt) + parseFloat(wipSkt) + parseFloat(shflSkt) + parseFloat(inTransitSkt);
            //var OrdToProduceQty = parseFloat(RequiredQuantity) - parseFloat(TotalAvlSkt);
            var FinReqQty = parseFloat(RequiredQuantity) - parseFloat(TotalAvlSkt);
            var OrdToProduceQty = parseFloat(FinReqQty) - parseFloat(pendOrdQty);
            OrdToProduceQty = OrdToProduceQty > 0 ? OrdToProduceQty : 0;
            FlagReset = "N";
            if (SF_ItemId == SF_Item_id) {
                //FinReqQty = RequiredQuantity;
            } else {
                if (child_sf_items.includes(SF_ItemId)) {
                    FinReqQty = FinReqQty > 0 ? FinReqQty : 0;
                    clickedrow.find("#BOMQuantity").val(parseFloat(RequiredQuantity).toFixed(QtyDecDigit));
                    clickedrow.find("#RequiredQuantity").val(parseFloat(FinReqQty).toFixed(QtyDecDigit));


                    if (parseFloat(OrdToProduceQty).toFixed(0) > parseFloat(0)) {
                        clickedrow.find("#ProductionCompletionDate").attr("disabled", false);
                    } else {
                        clickedrow.find("#ProductionCompletionDate").val("");
                        clickedrow.find("#ProductionCompletionDate").css("border-color", "#ced4da");
                        clickedrow.find("#ProductionCompletionDateError").text("");
                        clickedrow.find("#ProductionCompletionDateError").css("display", "none");
                        clickedrow.find("#ProductionCompletionDate").attr("disabled", true);
                    }
                    clickedrow.find("#OrderToProduceQuantity").val(parseFloat(OrdToProduceQty).toFixed(0));
                    FlagResetHdnSfTable = "Y";

                }

            }
            if (child_sf_items.includes(SF_ItemId)) {
                let FinOrdToProdQty = clickedrow.find("#OrderToProduceQuantity").val();
                ChangePerc = ((parseFloat(CheckNullNumber(FinOrdToProdQty)) - parseFloat(SFbomQty)) / parseFloat(SFbomQty)) * 100;
                //CalculateRMItems_withSession(ChangePerc, SF_ItemId, FGItems);
                child_Rm_Change_items.push({ ChangePerc: ChangePerc, SF_ItemId: SF_ItemId, FGItems: FGItems})
                if (FlagResetHdnSfTable == "Y") {
                    ResetHdnSfTable_withSession(FGItems, ChangePerc, SF_ItemId, child_sf_items, SF_Item_id);
                }
                
            }
        });

    });
    CalculateRMItems_withSession("","","",child_Rm_Change_items);
    //child_Rm_Change_items.map((item) => {
    //    CalculateRMItems_withSession(item.ChangePerc, item.SF_ItemId, item.FGItems);
    //})
    //}

    FlagApply = "N";
    $("#InputMaterialDetailsTbl tbody tr").each(function () {
        var currentRow = $(this);
        var RM_ItemId = currentRow.find("#hf_materialID").val();
        var RM_ReqQty = 0;
        var ssn_RmData = sessionStorage.ArrRmData_forCalculation;
        let Hdn_RMItemsSFGWise = ssn_RmData != null ? JSON.parse(ssn_RmData):[];
        Hdn_RMItemsSFGWise.filter(v => v.RM_ItemID == RM_ItemId).map((item) => {
            var RM_ReqQty1 = item.RM_ReqQty;
            RM_ReqQty = parseFloat(parseFloat(RM_ReqQty) + parseFloat(RM_ReqQty1)).toFixed(QtyDecDigit);
        })
       
        currentRow.find("#BOMQuantity").val(RM_ReqQty);
        //--------------------------------------
        var RequiredQty = "";
        var RequisitionQty = "";
        var imd_bomqty = currentRow.find("#BOMQuantity").val();
        var imd_avlstk = currentRow.find("#AvailableStock").val();
        var ShopFloorstock = currentRow.find("#ShopFloorstock").val();
        var INtransit = currentRow.find("#INtransit").val();
        var imd_minresvstk = currentRow.find("#MinimumStockReserve").val();
        var imd_pendingordqty = currentRow.find("#PendingOrderQuantity").val();
        RequiredQty = parseFloat((parseFloat(imd_bomqty) + parseFloat(imd_minresvstk)) - (parseFloat(imd_avlstk) + parseFloat(ShopFloorstock) + parseFloat(INtransit))).toFixed(QtyDecDigit)
        RequisitionQty = parseFloat(parseFloat(RequiredQty) - parseFloat(imd_pendingordqty)).toFixed(QtyDecDigit)
        debugger;
        if (parseFloat(RequiredQty) <= parseFloat(0)) {
            RequiredQty = "0";
        }
        if (parseFloat(RequisitionQty) <= parseFloat(0)) {
            RequisitionQty = "0";
            currentRow.find("#ProcurementCompletionDate").val("");
            currentRow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
            currentRow.find("#ProcurementCompletionDateError").text("");
            currentRow.find("#ProcurementCompletionDateError").css("display", "none");
            currentRow.find("#ProcurementCompletionDate").attr("disabled", true);
        } else {
            currentRow.find("#ProcurementCompletionDate").attr("disabled", false);
        }
        currentRow.find("#RequiredQuantity").val(parseFloat(RequiredQty).toFixed(QtyDecDigit));
        var ItemtypeFlag = $("#InputItemtypeFlag").val();
        if (ItemtypeFlag == "Y") {
            currentRow.find("#RequisitionQuantity").val(parseFloat(RequisitionQty).toFixed(QtyDecDigit));
        }
        else {
            currentRow.find("#RequisitionQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        }
        //-------------------------------------------
    });

    //CalculationReqAndRequisitionQty();
    
}

function CalcRowMaterialForSFItems(SF_Item_id, ProduceQuantity) {
    debugger
    var ApplyFromRow = 0;
    
    var ChangePerc = 0;
    //var ProduceQty = clickedrow.find("#OrderToProduceQuantity").val();
    //var RequiredQuantity = clickedrow.find("#RequiredQuantity").val();

    var bomQty = 0;
    var FGItems = "";
    $("#Hdn_SFItemsFGWise tbody tr #fg_Sf_ItemID:contains(" + SF_Item_id + ")").closest("tr").each(function () {
        var Crow = $(this);
        var fg_ItemId = Crow.find("#fg_ItemId").text();
        var fg_rowno = Crow.find("#fg_rowno").text();
        var fg_Sf_ItemID = Crow.find("#fg_Sf_ItemID").text();
        var fg_BomQty = Crow.find("#fg_BomQty").text();
        var fg_ReqQty = Crow.find("#fg_ReqQty").text();
        if (SF_Item_id == fg_Sf_ItemID) {
            ApplyFromRow = fg_rowno;
            if (FGItems == "") {
                FGItems += fg_ItemId;
            }
            else {
                FGItems += "," + fg_ItemId;
            }
            bomQty = parseFloat(bomQty) + parseFloat(fg_BomQty);
        }
        
    });
    ChangePerc = ((parseFloat(CheckNullNumber(ProduceQuantity)) - parseFloat(bomQty)) / parseFloat(bomQty)) * 100;

    //var FlagApply = "N";
    //var ArrFGItems = FGItems.split(',');
    ////let NewArr = [];
    var child_sf_items = [];
    //for (var i = 0; i < ArrFGItems.length; i++) {
    //    FlagApply = "N";
    //    var NewChangePerc = ChangePerc;
    //    var checkParent = "";
    //    var checkParentType = "";
    //    $("#Hdn_SFItemsFGWise tbody tr #fg_ItemId:contains(" + ArrFGItems[i] + ")").closest("tr").each(function () {
    //        var Crow = $(this);
    //        var fg_ItemId = Crow.find("#fg_ItemId").text();
    //        var fg_rowno = Crow.find("#fg_rowno").text();
    //        var fg_Sf_ItemID = Crow.find("#fg_Sf_ItemID").text();
    //        var fg_BomQty = Crow.find("#fg_BomQty").text();
    //        var fg_ReqQty = Crow.find("#fg_ReqQty").text();
    //        var fg_Sf_ItemType = Crow.find("#fg_Sf_ItemType").text();
    //        var parent_Sf_ItemId = Crow.find("#parent_Sf_ItemID").text();
    //        if (SF_Item_id == fg_Sf_ItemID) {
    //            FlagApply = "Y";
                
    //        }
            
    //        if (FlagApply == "Y" && ArrFGItems[i] == fg_ItemId ) {
    //            if (checkParentType!="" && checkParentType != "OW") {
    //                if (checkParent == parent_Sf_ItemId) {
    //                    var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
    //                    Crow.find("#fg_ReqQty").text(parseFloat(ReqQty1));

    //                    child_sf_items.push(fg_Sf_ItemID);
    //                } else {
    //                    FlagApply = "N";
    //                }

    //            } else {
    //                var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
    //                Crow.find("#fg_ReqQty").text(parseFloat(ReqQty1));
    //                child_sf_items.push(fg_Sf_ItemID);
    //            }
               
    //            checkParent = fg_Sf_ItemID;
    //            checkParentType = fg_Sf_ItemType;
    //            /*CalculateRMItems(ChangePerc, fg_Sf_ItemID, FGItems);*/ //Shifted After Produce Qty Update in SF Table.

    //            //NewArr.push({ fg_ItemId: fg_ItemId, fg_rowno: fg_rowno, fg_Sf_ItemID: fg_Sf_ItemID, fg_BomQty: fg_BomQty, fg_ReqQty: ReqQty1 });
    //        }
    //    });
    //}
    ResetHdnSfTable(FGItems, ChangePerc, SF_Item_id, child_sf_items);
    FlagApply = "N";
    var FlagResetHdnSfTable = "N";
        $("#SFMaterialDetailsTbl tbody tr").each(function () {
            var clickedrow = $(this);
            var Product_id = clickedrow.find("#hf_FGID").val();
            var SF_ItemId = clickedrow.find("#hf_materialID").val();
            var RequiredQuantity = 0;//clickedrow.find("#RequiredQuantity").val();
            var SFbomQty = 0;
            $("#Hdn_SFItemsFGWise tbody tr #fg_Sf_ItemID:contains(" + SF_ItemId + ")").closest("tr").each(function () {
                var CurrentSFrow = $(this);
                var fg_ReqQty = 0;
                fg_ReqQty = CurrentSFrow.find("#fg_ReqQty").text();
                var fg_BomQty = CurrentSFrow.find("#fg_BomQty").text();
                SFbomQty = parseFloat(SFbomQty) + parseFloat(fg_BomQty);
                RequiredQuantity = parseFloat(parseFloat(RequiredQuantity) + parseFloat(fg_ReqQty));//.toFixed(QtyDecDigit);
            });
           // var OrdToProduceQty = parseFloat(RequiredQuantity) - parseFloat(CheckNullNumber(clickedrow.find("#PendingOrderQuantity").val()));

            /*---------- Modified by Suraj Maurya on 15-11-2024 for subtract alailable stock-----*/
            var avlSkt = CheckNullNumber(clickedrow.find("#AvailableStock").val());
            var wipSkt = CheckNullNumber(clickedrow.find("#WIPstock").val());
            var shflSkt = CheckNullNumber(clickedrow.find("#ShopFloorstock").val());
            var inTransitSkt = CheckNullNumber(clickedrow.find("#INtransit").val());
            var pendOrdQty = CheckNullNumber(clickedrow.find("#PendingOrderQuantity").val());
            var TotalAvlSkt = parseFloat(avlSkt) + parseFloat(wipSkt) + parseFloat(shflSkt) + parseFloat(inTransitSkt);
            //var OrdToProduceQty = parseFloat(RequiredQuantity) - parseFloat(TotalAvlSkt);
            var FinReqQty = parseFloat(RequiredQuantity) - parseFloat(TotalAvlSkt);
            var OrdToProduceQty = parseFloat(FinReqQty) - parseFloat(pendOrdQty);
            OrdToProduceQty = OrdToProduceQty > 0 ? OrdToProduceQty : 0;
            FlagReset = "N";
            if (SF_ItemId == SF_Item_id) {
                //FinReqQty = RequiredQuantity;
            } else {
                if (child_sf_items.includes(SF_ItemId)) {
                    FinReqQty = FinReqQty > 0 ? FinReqQty : 0;
                    clickedrow.find("#BOMQuantity").val(parseFloat(RequiredQuantity).toFixed(QtyDecDigit));
                    clickedrow.find("#RequiredQuantity").val(parseFloat(FinReqQty).toFixed(QtyDecDigit));


                    if (parseFloat(OrdToProduceQty).toFixed(0) > parseFloat(0)) {
                        clickedrow.find("#ProductionCompletionDate").attr("disabled", false);
                    } else {
                        clickedrow.find("#ProductionCompletionDate").val("");
                        clickedrow.find("#ProductionCompletionDate").css("border-color", "#ced4da");
                        clickedrow.find("#ProductionCompletionDateError").text("");
                        clickedrow.find("#ProductionCompletionDateError").css("display", "none");
                        clickedrow.find("#ProductionCompletionDate").attr("disabled", true);
                    }
                    clickedrow.find("#OrderToProduceQuantity").val(parseFloat(OrdToProduceQty).toFixed(0));
                    FlagResetHdnSfTable = "Y";
                    
                }
                
            }
            if (child_sf_items.includes(SF_ItemId)) {
                let FinOrdToProdQty = clickedrow.find("#OrderToProduceQuantity").val();
                ChangePerc = ((parseFloat(CheckNullNumber(FinOrdToProdQty)) - parseFloat(SFbomQty)) / parseFloat(SFbomQty)) * 100;
                CalculateRMItems(ChangePerc, SF_ItemId, FGItems);
                if (FlagResetHdnSfTable == "Y") {
                    ResetHdnSfTable(FGItems, ChangePerc, SF_ItemId, child_sf_items, SF_Item_id);
                }
            }
            
            
            
        });
    //}
    
    FlagApply = "N";
    $("#InputMaterialDetailsTbl tbody tr").each(function () {
        var clickedrow = $(this);
        var RM_ItemId = clickedrow.find("#hf_materialID").val();
        var RM_ReqQty = 0;
        $("#Hdn_RMItemsSFGWise tbody tr #RM_ItemID:contains(" + RM_ItemId + ")").closest("tr").each(function () {
            var Crow = $(this);
            var Sf_ItemID = Crow.find("#Sf_ItemID").text();
            var RM_ReqQty1 = Crow.find("#RM_ReqQty").text();
            RM_ReqQty = parseFloat(parseFloat(RM_ReqQty)+parseFloat(RM_ReqQty1)).toFixed(QtyDecDigit);
        });
        clickedrow.find("#BOMQuantity").val(RM_ReqQty);
    });

    CalculationReqAndRequisitionQty();
}
function ResetHdnSfTable_withSession(FGItems, ChangePerc, SF_Item_id, child_sf_items, sf_item_changed, ChangePerc_bom) {

    var FlagApply = "N";
    var ArrFGItems = FGItems.split(',');
    //let NewArr = [];
    //var child_sf_items = [];
   // for (var i = 0; i < ArrFGItems.length; i++) {
        FlagApply = "N";
        var NewChangePerc = ChangePerc;
        var checkParent = "";
    var checkParentType = "";
    var sf_changed = [];
    var ssn_SfData = sessionStorage.ArrSfData_forCalculation;
    var SFItemList = ssn_SfData!=null ? JSON.parse(ssn_SfData):[];
        if (checkArrLength(SFItemList)) {
            let SFItemList_filt = SFItemList;//.filter(v => v.fg_ItemId == ArrFGItems[i]);
            let pre_Fg_Item = "";
            let NewSfItemList = SFItemList_filt.map((item) => {
                //if (item.fg_ItemId == ArrFGItems[i]) {
                if (ArrFGItems.includes(item.fg_ItemId)) {
                    var fg_ItemId = item.fg_ItemId;
                    if (fg_ItemId != pre_Fg_Item) {
                        FlagApply = "N";
                        child_sf_items.push(...sf_changed);
                        sf_changed = [];
                    }
                var fg_rowno = item.fg_rowno;
                var fg_Sf_ItemID = item.fg_Sf_ItemID;
                var fg_BomQty = item.fg_BomQty;
                var fg_ReqQty = item.fg_ReqQty;
                var fg_Sf_ItemType = item.fg_Sf_ItemType;
                var parent_Sf_ItemId = item.parent_Sf_ItemID;
                if (sf_item_changed != null && sf_item_changed != "") {//if sf item changed between auto update of SF section
                    if (parent_Sf_ItemId == sf_item_changed || FlagApply == "Y") {
                        if (SF_Item_id == fg_Sf_ItemID) {
                            FlagApply = "Y";
                        }
                        //else if (FlagApply == "Y" && ArrFGItems[i] == fg_ItemId) {
                        else if (FlagApply == "Y" && ArrFGItems.includes(fg_ItemId)) {
                            if (checkParentType != "" && checkParentType != "OW") {
                                if (checkParent == parent_Sf_ItemId) {
                                    var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                                    item.fg_ReqQty = parseFloat(ReqQty1);

                                    sf_changed.push(fg_Sf_ItemID);
                                } else {
                                    FlagApply = "N";
                                }

                            } else {
                                var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                                item.fg_ReqQty = parseFloat(ReqQty1);
                                sf_changed.push(fg_Sf_ItemID);
                            }
                        }
                        checkParent = fg_Sf_ItemID;
                        checkParentType = checkParentType == "OW" && (fg_Sf_ItemType == "IR" || fg_Sf_ItemType == "P") ? "OW" : fg_Sf_ItemType;
                    }
                }
                else {
                    //if (SF_Item_id == fg_Sf_ItemID) {
                    //    FlagApply = "Y";
                    //} else if (sf_changed.includes(parent_Sf_ItemId)) {
                    //    FlagApply = "Y";
                    //}
                    if (sf_changed.includes(parent_Sf_ItemId)) {
                        FlagApply = "Y";
                    }
                    //if (FlagApply == "Y" && ArrFGItems[i] == fg_ItemId) {
                    if (FlagApply == "Y" && ArrFGItems.includes(fg_ItemId)) {
                        if (checkParentType != "" && checkParentType != "OW") {
                            //if (checkParent == parent_Sf_ItemId) {
                            if (sf_changed.includes(parent_Sf_ItemId)) {
                                var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                                item.fg_ReqQty = parseFloat(ReqQty1);

                                sf_changed.push(fg_Sf_ItemID);
                            } else {
                                
                            }
                            FlagApply = "N";
                        } else {
                            var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                            item.fg_ReqQty = parseFloat(ReqQty1);
                            sf_changed.push(fg_Sf_ItemID);
                            FlagApply = "N";
                        }
                        checkParent = fg_Sf_ItemID;
                        checkParentType = checkParentType == "OW" && (fg_Sf_ItemType == "IR" || fg_Sf_ItemType == "P") ? "OW" : fg_Sf_ItemType;
                    }
                     if (SF_Item_id == fg_Sf_ItemID) {
                         //FlagApply = "Y";
                         sf_changed.push(fg_Sf_ItemID);
                    }
                    }
                    pre_Fg_Item = fg_ItemId
                    return item;
                }
                else {
                    return item;
                }
            })
            child_sf_items.push(...sf_changed);
            sessionStorage.setItem("ArrSfData_forCalculation", JSON.stringify(NewSfItemList))
        }

   // }
}
function ResetHdnSfTable(FGItems, ChangePerc, SF_Item_id, child_sf_items, sf_item_changed) {

    var FlagApply = "N";
    var ArrFGItems = FGItems.split(',');
    //let NewArr = [];
    //var child_sf_items = [];
    for (var i = 0; i < ArrFGItems.length; i++) {
        FlagApply = "N";
        var NewChangePerc = ChangePerc;
        var checkParent = "";
        var checkParentType = "";
        $("#Hdn_SFItemsFGWise tbody tr #fg_ItemId:contains(" + ArrFGItems[i] + ")").closest("tr").each(function () {
            var Crow = $(this);
            var fg_ItemId = Crow.find("#fg_ItemId").text();
            var fg_rowno = Crow.find("#fg_rowno").text();
            var fg_Sf_ItemID = Crow.find("#fg_Sf_ItemID").text();
            var fg_BomQty = Crow.find("#fg_BomQty").text();
            var fg_ReqQty = Crow.find("#fg_ReqQty").text();
            var fg_Sf_ItemType = Crow.find("#fg_Sf_ItemType").text();
            var parent_Sf_ItemId = Crow.find("#parent_Sf_ItemID").text();
            if (sf_item_changed != null && sf_item_changed != "") {//if sf item changed between auto update of SF section
                if (parent_Sf_ItemId == sf_item_changed || FlagApply == "Y") {
                    if (SF_Item_id == fg_Sf_ItemID) {
                        FlagApply = "Y";
                    }
                    else if (FlagApply == "Y" && ArrFGItems[i] == fg_ItemId) {
                        if (checkParentType != "" && checkParentType != "OW") {
                            if (checkParent == parent_Sf_ItemId) {
                                var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                                Crow.find("#fg_ReqQty").text(parseFloat(ReqQty1));

                                child_sf_items.push(fg_Sf_ItemID);
                            } else {
                                FlagApply = "N";
                            }

                        } else {
                            var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                            Crow.find("#fg_ReqQty").text(parseFloat(ReqQty1));
                            child_sf_items.push(fg_Sf_ItemID);
                        }
                    }
                    checkParent = fg_Sf_ItemID;
                    checkParentType = checkParentType == "OW" && (fg_Sf_ItemType == "IR" || fg_Sf_ItemType == "P") ? "OW" : fg_Sf_ItemType;
                }
            } else {
                if (SF_Item_id == fg_Sf_ItemID) {
                    FlagApply = "Y";
                }
                if (FlagApply == "Y" && ArrFGItems[i] == fg_ItemId) {
                    if (checkParentType != "" && checkParentType != "OW") {
                        if (checkParent == parent_Sf_ItemId) {
                            var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                            Crow.find("#fg_ReqQty").text(parseFloat(ReqQty1));

                            child_sf_items.push(fg_Sf_ItemID);
                        } else {
                            FlagApply = "N";
                        }
                    } else {
                        var ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                        Crow.find("#fg_ReqQty").text(parseFloat(ReqQty1));
                        child_sf_items.push(fg_Sf_ItemID);
                    }
                    checkParent = fg_Sf_ItemID;
                    checkParentType = checkParentType == "OW" && (fg_Sf_ItemType == "IR" || fg_Sf_ItemType == "P") ? "OW" : fg_Sf_ItemType;
                }
            }
            
        });
    }
}
function CalculateRMItems(ChangePerc, SF_ItemId, FGItems) {
    var ArrFGItems = FGItems.split(',');
    for (var i = 0; i < ArrFGItems.length; i++) {
        var SFFlagApply = "N";
        $("#Hdn_RMItemsSFGWise tbody tr #fg_ItemId:contains(" + ArrFGItems[i] + ")").closest("tr").each(function () {
            var Crow = $(this);
            var Sf_ItemID = Crow.find("#Sf_ItemID").text();
            var RM_BomQty = Crow.find("#RM_BomQty").text();
            if (SF_ItemId == Sf_ItemID) {
                SFFlagApply = "Y";
            }
            if (SFFlagApply == "Y") {
                var RM_ReqQty1 = parseFloat(parseFloat(RM_BomQty) + (parseFloat(RM_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                //if (ProduceQty > 0) {
                Crow.find("#RM_ReqQty").text(RM_ReqQty1);
                //}
                //else {
                //    Crow.find("#RM_ReqQty").text(parseFloat(0).toFixed(QtyDecDigit));
                //}
                SFFlagApply = "N";
            }
        });
    }
}
function CalculateRMItems_withSession(ChangePerc, SF_ItemId, FGItems,BulkChange) {
    var ArrFGItems = FGItems.split(',');
    //for (var i = 0; i < ArrFGItems.length; i++) {
    var SFFlagApply = "N";
    let New_Hdn_RMItemsSFGWise = [];
    var ssn_RmData = sessionStorage.ArrRmData_forCalculation;
    let Hdn_RMItemsSFGWise = ssn_RmData != null ? JSON.parse(ssn_RmData):[];
    if (BulkChange != null) {
        New_Hdn_RMItemsSFGWise = Hdn_RMItemsSFGWise.map((item) => {
            //if (item.fg_ItemId == ArrFGItems[i]) {
            var arr = BulkChange.filter(v => v.SF_ItemId == item.Sf_ItemID);
            if (arr.length > 0) {
                arr = arr[0];
                if (arr.FGItems.split(',').includes(item.fg_ItemId)) {
                    var Sf_ItemID = item.Sf_ItemID;
                    var RM_BomQty = item.RM_BomQty;
                    if (arr.SF_ItemId == Sf_ItemID) {
                        var RM_ReqQty1 = parseFloat(parseFloat(RM_BomQty) + (parseFloat(RM_BomQty) * parseFloat(arr.ChangePerc)) / 100).toFixed(QtyDecDigit);
                        item.RM_ReqQty = RM_ReqQty1;
                    }
                    return item;
                }
                return item;
            }
            else {
                return item;
            }
        })
    } else {
        New_Hdn_RMItemsSFGWise = Hdn_RMItemsSFGWise.map((item) => {
            //if (item.fg_ItemId == ArrFGItems[i]) {
            if (ArrFGItems.includes(item.fg_ItemId)) {
                var Sf_ItemID = item.Sf_ItemID;
                var RM_BomQty = item.RM_BomQty;
                if (SF_ItemId == Sf_ItemID) {
                    var RM_ReqQty1 = parseFloat(parseFloat(RM_BomQty) + (parseFloat(RM_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                    item.RM_ReqQty = RM_ReqQty1;
                }
                return item;
            }
            else {
                return item;
            }
        })
    }
        sessionStorage.setItem("ArrRmData_forCalculation", JSON.stringify(New_Hdn_RMItemsSFGWise));
    //}
}

function changeInSFProduceQtyByAvlQty(FGItems, SF_Item_id, ChangePerc) {
    var FlagApply = "N";
    var ArrFGItems = FGItems.split(',');
    for (var i = 0; i < ArrFGItems.length; i++) {
        FlagApply = "N";
        $("#Hdn_SFItemsFGWise tbody tr #fg_ItemId:contains(" + ArrFGItems[i] + ")").closest("tr").each(function () {
            var Crow = $(this);
            var fg_ItemId = Crow.find("#fg_ItemId").text();
            var fg_rowno = Crow.find("#fg_rowno").text();
            var fg_Sf_ItemID = Crow.find("#fg_Sf_ItemID").text();
            var fg_BomQty = Crow.find("#fg_BomQty").text();
            //var fg_ReqQty = Crow.find("#fg_ReqQty").text();
            if (SF_Item_id == fg_Sf_ItemID) {
                FlagApply = "Y";
            }
            if (FlagApply == "Y" && ArrFGItems[i] == fg_ItemId) {
                var RM_ReqQty1 = parseFloat(parseFloat(fg_BomQty) + (parseFloat(fg_BomQty) * parseFloat(ChangePerc)) / 100).toFixed(QtyDecDigit);
                Crow.find("#fg_ReqQty").text(RM_ReqQty1);
            }
        });
    }
}
function OnChangeProcureQty(evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var ProcureQty = clickedrow.find("#OrderToProcureQuantity").val();
    var PrevProcureQty = clickedrow.find("#OrderToProcureQuantity").attr("data-initial-value");
    clickedrow.find("#OrderToProcureQuantity").attr("data-initial-value", ProcureQty);
    if (AvoidDot(ProcureQty) == false) {

        clickedrow.find("#OrderToProcureQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        clickedrow.find("#ProcurementCompletionDate").attr("disabled", true);
        clickedrow.find("#ProcurementCompletionDate").val("");
        clickedrow.find("#ProcurementCompletionDate").css("border-color", "#ced4da");
        clickedrow.find("#ProcurementCompletionDateError").text("");
        clickedrow.find("#ProcurementCompletionDateError").css("display", "none");
    } else {
        clickedrow.find("#OrderToProcureQuantityError").text("");
        clickedrow.find("#OrderToProcureQuantityError").css("display", "none");
        clickedrow.find("#OrderToProcureQuantity").css("border-color", "#ced4da");
        clickedrow.find("#OrderToProcureQuantity").val(parseFloat(ProcureQty).toFixed(QtyDecDigit));
        clickedrow.find("#ProcurementCompletionDate").attr("disabled", false);
        if (parseFloat(CheckNullNumber(ProcureQty)) > 0) {
            var ProduceQty = clickedrow.find("#OrderToProduceQuantity").val();
            var pendOrdQty = clickedrow.find("#PendingOrderQuantity").val();
            var NewProduceQty = parseFloat(CheckNullNumber(ProduceQty)) /*+ parseFloat(CheckNullNumber(PrevProcureQty))*/ - parseFloat(CheckNullNumber(ProcureQty));// - parseFloat(CheckNullNumber(pendOrdQty));

            if (parseFloat(NewProduceQty) > 0) {
                clickedrow.find("#OrderToProduceQuantity").val(parseFloat(NewProduceQty).toFixed(0)).change();
            } else {
                clickedrow.find("#OrderToProduceQuantity").val(parseFloat(0).toFixed(0)).change();
            }
        }
    }
   

}
function OnKeyPressProcureQty(el, evt) {
    debugger;
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#OrderToProcureQuantityError").css("display", "none");
    clickedrow.find("#OrderToProcureQuantity").css("border-color", "#ced4da");
    return true;
}
function OnKeyPressProduceQty(el, evt) {
    if (Cmn_IntValueonly(el, evt, "#QtyDigit") == false) {/*Modified by Suraj Changed to Int Only from FloatOnly*/
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#OrderToProduceQuantityError").css("display", "none");
    clickedrow.find("#OrderToProduceQuantity").css("border-color", "#ced4da");
    return true;
}

function OnClickProducedQtyIconBtn(e,type) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var srno = clickedrow.find("#hfsno").val();
    var productname = clickedrow.find("#ProductName" + srno + " option:selected").text();
    var productId = clickedrow.find("#ProductName" + srno + " option:selected").val();
    var txt_MRPNumber = $("#txt_MRPNumber").val();
    var txt_MRPDate = $("#txt_MRPDate").val();
    if (type == "SF") {
        productname = clickedrow.find("#MaterialName").val();
        productId = clickedrow.find("#hf_materialID").val();
    }
    var uom = clickedrow.find("#UOM").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialRequirementPlan/GetProducedQuantityDetail",
        data: {
            mrp_no: txt_MRPNumber,
            mrp_dt: txt_MRPDate,
            product_Id: productId,
            Flag: type
        },
        success: function (data) {
            $("#PopUpProducedQuantityDetail").html(data);
            $("#PD_ProductName").val(productname);
            $("#PD_UOM").val(uom);
            //$('#TblProducedQtyDtl tbody').append(`
            //            <tr>
            //                <td>01</td>
            //                <td>AE/02/23/MRP0000027</td>
            //                <td>2023-02-22</td>
            //                <td class="num_right">0000.00</td>
            //            </tr>`);
        }
    })
    
}
function OnClickProcuredQtyIconBtn(e, type) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var srno = clickedrow.find("#hfsno").val();
    var productname = '';//clickedrow.find("#ProductName" + srno + " option:selected").text();
    var productId = '';//clickedrow.find("#ProductName" + srno + " option:selected").val();
    var txt_MRPNumber = $("#txt_MRPNumber").val();
    var txt_MRPDate = $("#txt_MRPDate").val();
    if (type == "SF") {
        productname = clickedrow.find("#MaterialName").val();
        productId = clickedrow.find("#hf_materialID").val();
    }
    else if (type == "RM") {
        productname = clickedrow.find("#MaterialName").val();
        productId = clickedrow.find("#hf_materialID").val();
    }
    var uom = clickedrow.find("#UOM").val();
    var UOMID = clickedrow.find("#UOMID").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialRequirementPlan/GetProcuredQuantityDetail",
        data: {
            mrp_no: txt_MRPNumber,
            mrp_dt: txt_MRPDate,
            product_Id: productId,
            Flag: type,
            UomId: UOMID//Added by Suraj on 10-01-2024
        },
        success: function (data) {
            $("#PopUpProcuredQuantityDetail").html(data);
            $("#RMPD_ProductName").val(productname);
            $("#RMPD_UOM").val(uom);
            
        }
    })

}

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemProdQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemPlanQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlStockBtn",);
}
function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#hfsno").val();
    var SFSrNo = clickdRow.find("#SFSrNo").text();
    var ItemType = "";
    if (flag == "Quantity") {
        ItemType = "FG";
    }
    if (flag == "OrdrToPrecurQuantity") {
        ItemType = "SF";
    }
    if (flag == "RMOrdrToPrecurQuantity") {
        ItemType = "RM";
    }
    if (flag == "OrdrToProduceQuantity") {
        ItemType = "SF";
    }
    if (flag == "OrdrToPrecurQuantity" || flag == "RMOrdrToPrecurQuantity" || flag == "OrdrToProduceQuantity") {
        ProductNm = clickdRow.find("#MaterialName").val();
        ProductId = clickdRow.find("#hf_materialID").val();
        UOM = clickdRow.find("#UOM").val();
    }
    else {
        var ProductNm = clickdRow.find("#ProductName" + hfsno + " option:selected").text();
        var ProductId = clickdRow.find("#ProductName" + hfsno).val();
        var UOM = $("#UOM").val();
    }
    

    var doc_no = $("#txt_MRPNumber").val();
    var doc_dt = $("#txt_MRPDate").val();

    //var UOMID = clickdRow.find("#UOMID").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    var NewArr1 = new Array();
    if (flag == "Quantity" || flag == "OrdrToPrecurQuantity" || flag == "RMOrdrToPrecurQuantity" || flag == "OrdrToProduceQuantity") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr")
            .find("#ItemType[value='" + ItemType + "']").closest('tr').each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.ItemType = row.find('#ItemType').val();
            NewArr.push(List);
        });
        $("#hdn_SubItemDetailTblMRPSF_Procure tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr1.push(List);
        });
        if (flag == "OrdrToPrecurQuantity" || flag == "OrdrToProduceQuantity") {
            ProductNm = clickdRow.find("#MaterialName").val();
            ProductId = clickdRow.find("#hf_materialID").val();
            UOM = clickdRow.find("#UOM").val();
            
            Sub_Quantity = flag == "OrdrToPrecurQuantity" ? clickdRow.find("#OrderToProcureQuantity").val() : clickdRow.find("#OrderToProduceQuantity").val();
        }
        else if (flag == "RMOrdrToPrecurQuantity") {
            ProductNm = clickdRow.find("#MaterialName").val();
            ProductId = clickdRow.find("#hf_materialID").val();
            UOM = clickdRow.find("#UOM").val();
            Sub_Quantity = clickdRow.find("#RequisitionQuantity").val();
        }
        else {
            Sub_Quantity = clickdRow.find("#PlannedQuantity").val();
        }
        
    } else if (flag == "ProduceQty") {
        Sub_Quantity = clickdRow.find("#ProducedQuantity").val();
    } else if (flag == "OrderedQty") {
        ProductNm = clickdRow.find("#item_nm").text();
        ProductId = clickdRow.find("#output_item_id").text();
        UOM = clickdRow.find("#UOM").text();
        Sub_Quantity = clickdRow.find("#OrderedQty").text().trim();
        doc_no = clickdRow.find("#jc_no").text().trim();
        doc_dt = clickdRow.find("#jc_dt").text().trim();
    }

    debugger
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hd_Status").val();
    hd_Status = IsNull(hd_Status, "").trim();
    var src_typ = $("#hdn_ddl_src_type").val();
    if (src_typ == "P" || src_typ == "F") {
        if (flag == "OrdrToPrecurQuantity" || flag == "RMOrdrToPrecurQuantity" || flag == "OrdrToProduceQuantity") {//Modified by Suraj Maurya on 18-11-2024 Flag Name changed OrdrToPrecurQuantity1->OrdrToProduceQuantity
            var status = $("#hd_Status").val();
            if (status == "" || status == null) {
                IsDisabled = "N";
            }
            var command = $("#hdnCommand").val();
            if (status == "D" && command == "Edit") {
                IsDisabled = "N";
            }
        } else {
            IsDisabled = "Y";
        }
      
    }
   
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialRequirementPlan/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: flag == "OrdrToPrecurQuantity" ? JSON.stringify(NewArr1) : JSON.stringify(NewArr),//Modified by Suraj Maurya on 18-11-2024
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: doc_no,
            Doc_dt: doc_dt
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
    if (flag == 'enable') {

    }
    else if (flag = 'readonly') {

    }
}
function GetSubItemAvlStock(e) {
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#hfsno").val();
    var ProductNm = Crow.find("#ProductName" + rowNo + " option:selected").text();
    var ProductId = Crow.find("#ProductName" + rowNo + " option:selected").val();
    var UOM = Crow.find("#UOM").val();

    var AvlStk = Crow.find("#AvailableStockInBase").val();
    Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, "0", AvlStk, "br");

}
function CheckValidations_forSubItems() {
    debugger;
    //return Cmn_CheckValidations_forSubItems("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y");
    var ErrFlg = "";
    if (Cmn_CheckValidations_forSubItems("ProductItemDetailsTbl", "hfsno", "ProductName", "PlannedQuantity", "SubItemPlanQty", "Y")==false) {
        ErrFlg="Y"
    }
    
    if (Cmn_CheckValidations_forSubItems("SFMaterialDetailsTbl", "SFSrNo", "hf_materialID", "OrderToProduceQuantity", "SubItemOrdrToProduceQty", "Y") == false) {//Modified by Suraj Maurya on 18-11-2024
        ErrFlg = "Y"
    }
    
    if (Cmn_CheckValidations_forSubItems("SFMaterialDetailsTbl", "SFSrNo", "hf_materialID", "OrderToProcureQuantity", "SubItemOrdrToPrecurQty", "Y") == false) {//Modified by Suraj Maurya on 18-11-2024
        ErrFlg = "Y"
    }
    if (Cmn_CheckValidations_forSubItems("InputMaterialDetailsTbl", "RMSrNo", "hf_materialID", "RequisitionQuantity", "RMSubItemOrdrToPrecurQty", "Y") == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }

}
function ResetWorningBorderColor() {
    let ShowMessage = "N";
    //return Cmn_CheckValidations_forSubItems("ProductItemDetailsTbl", "hfsno", "ProductName", "PlannedQuantity", "SubItemPlanQty", "N");
    var ErrFlg = "";
    if (Cmn_CheckValidations_forSubItems("ProductItemDetailsTbl", "hfsno", "ProductName", "PlannedQuantity", "SubItemPlanQty", ShowMessage) == false) {
        ErrFlg = "Y"
    }

    if (Cmn_CheckValidations_forSubItems("SFMaterialDetailsTbl", "SFSrNo", "hf_materialID", "OrderToProduceQuantity", "SubItemOrdrToProduceQty", ShowMessage) == false) {//Modified by Suraj Maurya on 18-11-2024
        ErrFlg = "Y"
    }

    if (Cmn_CheckValidations_forSubItems("SFMaterialDetailsTbl", "SFSrNo", "hf_materialID", "OrderToProcureQuantity", "SubItemOrdrToPrecurQty", ShowMessage) == false) {//Modified by Suraj Maurya on 18-11-2024
        ErrFlg = "Y"
    }
    if (Cmn_CheckValidations_forSubItems("InputMaterialDetailsTbl", "RMSrNo", "hf_materialID", "RequisitionQuantity", "RMSubItemOrdrToPrecurQty", ShowMessage) == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

function OnClickMaterialPendingOrderQty(evt,M_type) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var productid = clickedrow.find("#hf_materialID").val();
    var PndOrdQty = clickedrow.find("#PendingOrderQuantity").val();
    var MaterialName = clickedrow.find("#MaterialName").val();
    var UOM = clickedrow.find("#UOM").val();
    var UOMID = clickedrow.find("#UOMID").val();
   
   
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/MaterialRequirementPlan/GetRMPendingOrderQuantityDetails",
            data: { ProductID: productid, PndOrdQty: PndOrdQty, MaterialName: MaterialName, UOM: UOM, UomId: UOMID},
            //dataType: "json",
            success: function (data) {
                debugger;
                $("#PopUpPendingOrderQuantity").html(data);
                
            },
        });
}
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "ProductItemDetailsTbl", [{ "FieldId": "ProductName", "FieldType": "select", "SrNo": "hfsno" }]);
}
function FilterItemDetail1(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "SFMaterialDetailsTbl", [{ "FieldId": "MaterialName", "FieldType": "input" }]);
}

function FilterItemDetail2(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "InputMaterialDetailsTbl", [{ "FieldId": "MaterialName", "FieldType": "input" }]);
}




