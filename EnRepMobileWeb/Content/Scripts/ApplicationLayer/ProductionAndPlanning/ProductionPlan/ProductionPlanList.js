$(document).ready(function () {
    debugger;
    $("#tbodyPPList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var WF_Status = $('#WF_Status').val();
            var clickedrow = $(e.target).closest("tr");
            var PPNumber = clickedrow.children("#PP_no").text();
            var PPDate = clickedrow.children("#PP_date").text();
            if (PPNumber != null && PPNumber != "") {
                window.location.href = "/ApplicationLayer/ProductionPlan/EditDomesticPPDetails/?PPNumber=" + PPNumber + "&PPDate=" + PPDate + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    //$("#datatable-buttons >tbody").bind("click", function (e) {
    //    debugger;
    //    var clickedrow = $(e.target).closest("tr");
    //    var PPNumber = clickedrow.children("#PP_no").text();
    //    var PPDate = clickedrow.children("#PP_date").text();
    //    var date = PPDate.split("-");
    //    var FDate = date[2] + '-' + date[1] + '-' + date[0];
    //    var Doc_id = $("#DocumentMenuId").val();
    //    debugger;
    //    GetWorkFlowDetails(PPNumber, FDate, Doc_id);
    //    $("#hdDoc_No").val(PPNumber);
    //});
});
function BtnSearch() {
    debugger;
    FilterPPListData();
    ResetWF_Level();
}
function CmnGetWorkFlowDetails(e) {
    var clickedrow = $(e.target).closest("tr");
    var PP_No = clickedrow.children("#PP_no").text();
    var PP_Date = clickedrow.children("#PP_date").text();
    var PP_Status = clickedrow.children("#PP_Status").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(PP_No);
    if (PP_Date.split('-')[2].length == 4) {
        PP_Date = PP_Date.split('-').reverse().join('-');
    }
    PP_Status = PP_Status == "Drafted" ? "D" : (PP_Status == "Forwarded" ? "F" : "N");
    GetWorkFlowDetails(PP_No, PP_Date, Doc_id, PP_Status);
    var a = 1;
}
function ResetWF_Level() {
    var li_count = $("#wizard ul li").length;
    if (li_count > 0) {
        for (var y = 0; y < li_count; y++) {
            var id = parseInt(y) + 1;
            $("#a_" + id).removeClass("done");
            $("#a_" + id).removeClass("selected");
            $("#a_" + id).addClass("disabled");
        }
    }
}
function FilterPPListData() {
    debugger;
    try {
        var Source = $("#ddl_src_type").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ProductionPlan/PPListSearch",
            data: {
                Source: Source,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodyPPList').html(data);
                $('#ListFilterData').val(Source + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("PP Error : " + err.message);

    }
}
function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    if ((FromDate != null || FromDate == "") && (ToDate != null || ToDate == "")) {
        if (FromDate > ToDate) {

            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            $("#txtTodate").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");


        }
    }

}
function BindPPTrackingDetails(e) {
    var clickedrow = $(e.target).closest("tr");
    var ppNo = clickedrow.children("#PP_no").text();
    var ppDate = clickedrow.children("#PP_date").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ProductionPlan/GetPpTrackingDetails",
        data: {
            ppNo: ppNo,
            ppDate: ppDate
        },
        success: function (data) {
            debugger;
            $('#PPTracking').html(data);
            $('#txtPpTracking').val(ppNo);
            $('#OrderDate').val(ppDate);
        },
        error: function OnError(xhr, errorType, exception) {
            debugger;
        }
    });
}
function BindItemInfoBtnClick(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemId = clickedrow.find("#hdnItemId").text();
    ItemInfoBtnClick(itemId);
}
function OnClickOrderedQty(e, flag) {
    debugger;
    var ItemID = $(e.target).closest("tr").find("#hdnItemId").text();
    //var hdnShflId = $(e.target).closest("tr").find("#hdnShflId").text();
    //var hdnOpId = $(e.target).closest("tr").find("#hdnOpId").text();
    //var shflName = $(e.target).closest("tr").find("#hdnShflName1").text();
    //var opName = $(e.target).closest("tr").find("#hdnOpName1").text();
    //var txtFromdate = $("#txtFromdate").val();
    //var txtTodate = $("#txtTodate").val();
    //var txtShowAs = $("#txtShowAs").val();
    var Item_Name = $(e.target).closest("tr").find("#TDItemName").text().trim();
    var UomAlias = $(e.target).closest("tr").find("#UOM").text().trim();
    var confir_no = $(e.target).closest("tr").find("#Prod_Confir_no").text().trim();
    // Get the text content of the date cell and trim it
    var confir_dt = $(e.target).closest("tr").find("#Prod_Confir_dt").text().trim();
    if (confir_no != "" && confir_no != null) {
        let day, month, year;

        if (confir_dt.includes('/')) {
            [day, month, year] = confir_dt.split('/');
        } else if (confir_dt.includes('-')) {
            [day, month, year] = confir_dt.split('-');
        }
        const confir_date = `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
        var qty = 0;
        //if (flag == "orderqty") {
        //    qty = $(e.target).closest("tr").find("#orderqty").text().trim();
        //}
        //else if (flag == "produceqty") {
        //    qty = $(e.target).closest("tr").find("#txtProduceQty").text().trim();
        //}
        //else if (flag == "AnalysisQcQty") {
        //    qty = $(e.target).closest("tr").find("#qcqty").text().trim();
        //}
        if (flag == "accept_qty") {
            qty = $(e.target).closest("tr").find("#AcceptQty").text().trim();
        }
        else if (flag == "rework_qty") {
            qty = $(e.target).closest("tr").find("#ReworkableQty").text().trim();
        }
        else if (flag == "reject_qty") {
            qty = $(e.target).closest("tr").find("#RejectQty").text().trim();
        }

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ProductionPlan/GetProductionPlanQC_DetailsOnClick",
            data: {
                Item_id: ItemID, flag: flag, Item_Name: Item_Name, UomAlias: UomAlias, qty: qty, Plan_no: confir_no,
                Plan_dt: confir_date
            },
            success: function (data) {
                $("#ProductionPlanQCDetailsPopUp").html(data);

                cmn_apply_datatable("#tblProdPlanQCDetail");
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ProductionPlan/GetProductionPlanQC_DetailsOnClick",
            data: {
                Item_id: ItemID, flag: flag, Item_Name: Item_Name, UomAlias: UomAlias, qty: qty, Plan_no: "",
                Plan_dt: ""
            },
            success: function (data) {
                $("#ProductionPlanQCDetailsPopUp").html(data);

                cmn_apply_datatable("#tblProdPlanQCDetail");
            }
        });
    }
   
    //$('#plShflName1').val(shflName);
    //$('#plOpName1').val(opName);
}
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
    let ItemName = Row.find("#itm_nm").text();
    let ItemId = Row.find("#Hdn_product_id").text();
    let UOM = Row.find("#ProdAnlyUom").text();

    let ReasonRemarks = "";
    //ReasonRemarks = Row.find("#ReasonForReject").val();
    //let IsDisabled = "Disabled";//$("#DisableSubItem").val();
    //if (IsDisabled == "Y") {
    $("#RR_Remarks").attr("readonly", true);
    $("#RR_Remarks_Dropdown").attr("disabled", true);
    $("#RR_btnClose").attr("hidden", false);
    $("#RR_btnSaveAndExit,#RR_btnDiscardAndExit").attr("hidden", true);

    // }
    if (flag == "reject") {
        ReasonRemarks = Row.find("#ReasonForReject").val();
        $("#Div_DropdownReason").show();
        $("#reasonrequired").show();
        $("reasonrequired").addClass("required");
        let rejQty = Row.find("#RejectQty").text().trim();
        if (parseFloat(CheckNullNumber(rejQty)) > 0) {
            $("#RR_Quantity").val(rejQty);

            var ReasonForReject_ID = Row.find("#ReasonForReject_ID").val();
            var ReasonForReject_Name = Row.find("#ReasonForReject_Name").val().trim();
            if (ReasonForReject_ID != "0" && ReasonForReject_ID != "" && ReasonForReject_ID != null) {
                $("#RR_Remarks_Dropdown").empty();
                $("#RR_Remarks_Dropdown").append(`<option value="${ReasonForReject_ID}"> ${ReasonForReject_Name}</option>`)
                $//("#RR_Remarks_Dropdown").val(ReasonForReject_ID).trigger('change')
            }
            else {
                //$("#RR_Remarks_Dropdown").val(0).trigger('change')
                $("#RR_Remarks_Dropdown").css("border-color", "#ced4da");
                $("[aria-labelledby='select2-RR_Remarks_Dropdown-container']").css("border-color", "#ced4da");
                $("#DropDown_vmRR_Remarks").css("display", "none");
                $("#DropDown_vmRR_Remarks").text("");
            }

        }
        else {
            $("#RR_Remarks_Dropdown").empty();
            $("#RR_Remarks_Dropdown").append(`<option value="0"> ---Select---</option>`)
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
        let rwkQty = Row.find("#ReworkableQty").text().trim();
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
        let AcceptQty = Row.find("#AcceptQty").text().trim();
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
}
function OnClickReceiveQtybtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemName = "";
    var Itemuom = "";
    ItemName = clickedrow.find("#itm_nm").text();
    Itemuom = clickedrow.find("#ProdAnlyUom").text();
    var qc_no = clickedrow.find("#hdn_qc_no").text();
    var qc_dt = clickedrow.find("#hdn_qc_dt").text();
    var ItemID = clickedrow.find("#Hdn_product_id").text();

    $.ajax(
        {
            type: "Post",
            url: "/ApplicationLayer/ProductionPlan/GetItemQCParamDetail",
            data: {
                ItemID: ItemID,
                qc_no: qc_no,
                qc_dt: qc_dt
            },
            success: function (data) {
                debugger;
                $("#RecQuantPopUp").html(data);
                $("#SaveExitClose").attr("data-dismiss", "");
                $("#QC_ItemName").val(ItemName);
                $("#UOM").val(Itemuom);
                setQcParamData();
                BindQCParam(ItemID);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
            }
        });
}
function setQcParamData() {
    if ($("#hdTblQCItemParamDetail tbody tr").length > 0) {
        let NewArr = [];
        var j = 0;
        var uom = $("#UOM").val();
        $("#hdTblQCItemParamDetail tbody tr").each(function () {
            var CurrentRow = $(this);

            var ItmCode = CurrentRow.find("#item_id").text().trim();
            var ItmName = CurrentRow.find("#item_name").text().trim();
            var ItemUOMID = CurrentRow.find("#uom_id").text().trim();
            var UOMName = CurrentRow.find("#uom_name").text().trim();
            if (UOMName == "" || UOMName == null) {
                UOMName = uom;
            }
           
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