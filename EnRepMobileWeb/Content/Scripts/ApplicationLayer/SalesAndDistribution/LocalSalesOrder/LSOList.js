/************************************************
Javascript Name:Local Sale Order List
Created By:Prem
Created Date: 23-02-2021
Description: This Javascript use for the Local Sale Order List many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    $("#sales_person").select2();
    RemoveSession();
    $('#Btn_AddNewSO').on("click", function () {
        debugger;
        RemoveSessionNew();
    });
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var DocumentMenuId = $("#DocumentMenuId").val();
        var WF_status = $("#WF_status").val();
        var CustType = $("#CustType").val();
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var LSO_No = clickedrow.children("#OrderNo").text();
        var LSO_Date = clickedrow.children("#OrderDt").text();
        if (LSO_No != "" && LSO_No != null) {
            location.href = "/ApplicationLayer/LSODetail/DblClickLSODetail/?So_no=" + LSO_No + "&So_dt=" + LSO_Date + "&ListFilterData=" + ListFilterData + "&DocumentMenuId=" + DocumentMenuId + "&WF_status=" + WF_status + "&CustType=" + CustType;
        }
        
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var SO_No = clickedrow.children("#OrderNo").text();
        var SO_Date = clickedrow.children("#OrderDt").text();
        var CustName = clickedrow.children("#cust_name").text();
        var doc_Id = $("#DocumentMenuId").val();
        //$("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        //$(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
        $("#hdSo_no").val(SO_No);
        $("#hdso_dt").val(SO_Date);
        $("#hdcust_name").val(CustName);
        $("#hdDoc_No").val(SO_No);
        GetSalesOrderDetails(SO_No, SO_Date);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(SO_No, SO_Date, doc_Id, Doc_Status);
        //CmnGetWorkFlowDetails(e);
    });
    BindCustomerList();
    
});
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
function BindCustomerList() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    $("#ddlCustomerName").select2({
        ajax: {
            url: $("#CustNameList").val(),
            data: function (params) {
                var queryParameters = {
                    SO_CustName: params.term,
                    DocumentMenuId: DocumentMenuId // search term like "a" then "an"
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}
function RemoveSession() {
    sessionStorage.removeItem("EditLSONo");
    sessionStorage.removeItem("EditLSODate");
}
function RemoveSessionNew() {
    sessionStorage.removeItem("TaxCalcDetails");
    sessionStorage.removeItem("LSO_No");
    sessionStorage.removeItem("SOTransType"); 
    sessionStorage.removeItem("SOitemList");
}
function BtnSearch() {
    debugger;
    FilterLSOList();
    ResetWF_Level();
}
function BtnAddNew() {
    debugger;
    LSODetail();
}
function FilterLSOList() {
    debugger;
    try {
        var CustId = $("#ddlCustomerName option:selected").val();
        //var OrderType = $("#ddlOrderType").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var sales_person = $("#sales_person option:selected").val();
        var DocumentMenuId = $("#DocumentMenuId").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LSOList/SearchLSODetail",
            data: {
                CustId: CustId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                DocumentMenuId: DocumentMenuId,
                sales_person: sales_person
            },
            success: function (data) {
                debugger;
                $('#LSOListTbody').html(data);
                $('#ListFilterData').val(CustId + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + sales_person);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

    }
}
function LSODetail() {
    debugger;
    try {
        location.href = "/ApplicationLayer/LSODetail/LSODetail";
        //$.ajax(
        //    {
        //        type: "POST",
        //        url: "/ApplicationLayer/LSO/LSODetail",
        //        data: {},
        //        success: function (data) {
        //            debugger;
        //            $("#rightPageContent").empty().html(data);
        //        },
        //    });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
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
function GetSalesOrderDetails(SONo, SODate) {
    var ValDecDigit = $("#ValDigit").text();///Amount

    if (SONo != null && SONo != "" && SODate != null && SODate != "") {
        sessionStorage.setItem("ShowLoader", "N");
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LSOList/GetSO_Detail",
            data: { SO_No: SONo, SO_Date: SODate },
            dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                    var arr = [];
                    //debugger;
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        $("#TxtOrderVal").val(parseFloat(arr.Table[0].SONetVal).toFixed(ValDecDigit));
                        $("#TxtShippedVal").val(parseFloat(arr.Table[0].ShipNetVal).toFixed(ValDecDigit));
                        $("#TxtPendingOrderVal").val(parseFloat(arr.Table[0].PendingOrdVal).toFixed(ValDecDigit));
                        $("#TxtInvoiceVal").val(parseFloat(arr.Table[0].Inv_NetVal).toFixed(ValDecDigit));
                    }
                    else {
                        $("#TxtOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                        $("#TxtShippedVal").val(parseFloat(0).toFixed(ValDecDigit));
                        $("#TxtPendingOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                        $("#TxtInvoiceVal").val(parseFloat(0).toFixed(ValDecDigit));
                    }
                }
                else {
                    $("#TxtOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                    $("#TxtShippedVal").val(parseFloat(0).toFixed(ValDecDigit));
                    $("#TxtPendingOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                    $("#TxtInvoiceVal").val(parseFloat(0).toFixed(ValDecDigit));
                }
            }
        });
    }
}
function OnClickOrderTrackingBtn(e) {
    debugger;
    //var SONo = $("#hdSo_no").val();
    //var SODate = $("#hdso_dt").val();
    var CustName = $("#hdcust_name").val();
    var clickedrow = $(e.target).closest("tr");
    var SONo = clickedrow.children("#OrderNo").text();
    var SODate = clickedrow.children("#OrderDt").text();
    var cust_name = clickedrow.children("#cust_name").text();
    var cust_alias = clickedrow.children("#cust_alias").text();
    if (SONo != null && SONo != "" && SODate != null && SODate != "") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LSOList/GetSOTrackingDetail",
            data: { SO_No: SONo, SO_Date: SODate },
            //dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
                    return false;
                }
               
                $("#trackingso").html(data);

                cmn_apply_datatable("#SO_trackingTBL");

                $("#SalesOrderNumber").val(SONo);
                $("#OrderDate").val(SODate);
                $("#CustomerName").val(cust_name);
                $("#CustomerAlias").val(cust_alias);
            }
        });
    }
    
}
function OnClickProdTrackingBtn(e) {
    debugger;
    //var SONo = $("#hdSo_no").val();
    //var SODate = $("#hdso_dt").val();
    //var CustName = $("#hdcust_name").val();

    var clickedrow = $(e.target).closest("tr");
    var SONo = clickedrow.children("#OrderNo").text();
    var SODate = clickedrow.children("#OrderDt").text();
    var cust_name = clickedrow.children("#cust_name").text();
    var cust_alias = clickedrow.children("#cust_alias").text();

    if (SONo != null && SONo != "" && SODate != null && SODate != "") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LSOList/GetProductionTrackingDetail",
            data: { SO_No: SONo, SO_Date: SODate },
            //dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
                    return false;
                }

                $("#prodtracking").html(data);

                cmn_apply_datatable("#Production_trackingTBL");

                $("#txtSalesOrderNumber").val(SONo);
                $("#txtOrderDate").val(SODate);
                $("#txtCustomerName").val(cust_name);
                $("#txtCustomerAlias").val(cust_alias);
            }
        });
    }

}
function OnClickOrderedQty(e, flag) {
    debugger;
    var ItemID = $(e.target).closest("tr").find("#hdnItemId").text(); 
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
            url: "/ApplicationLayer/LSOList/GetProductionPlanQC_DetailsOnClick",
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
            url: "/ApplicationLayer/LSOList/GetProductionPlanQC_DetailsOnClick",
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