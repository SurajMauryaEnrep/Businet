$(document).ready(function () {
   // $("#ddlItemGroupName").select2();
    //$("#ItemPortfolio").select2();
    Cmn_initializeMultiselect(['#ddlItemGroupName', '#ItemPortfolio']);
    //FSNAnalysisDtList();
});
function BtnSearchFSNAnalytics() {
    debugger;
    $("#Tbl_FSNAnalysis").DataTable().destroy();
    FSNAnalysisDtList();
}
function FSNAnalysisDtList() {
    debugger;
    let ReportType = $("#TransactionType").val();
   // let ItemGrpId = $("#ddlItemGroupName option:selected").val();
    let ItemGrpId = cmn_getddldataasstring("#ddlItemGroupName");
    //let ItemPrtFloId = $("#ItemPortfolio option:selected").val();
    let ItemPrtFloId = cmn_getddldataasstring("#ItemPortfolio");
    let FromDt = $("#FromDate").val();
    let upToDate = $("#calUpTodate").val();

    var filterArray = [ReportType, ItemGrpId, ItemPrtFloId, FromDt, upToDate];
    var filters = filterArray.map(x => `[${x}]`).join('_');
    
    $("#Tbl_FSNAnalysis").DataTable({
        dom: "Blfrtip",
        "pageLength": 50,
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/ApplicationLayer/FSNAnalysis/LoadData",
            "data": {
                ItemListFilter: filters,
            },
            "type": "POST",
            "datatype": "json",
        },
        error: function (err) {
            console.log(err);
        },
        "columns": StkDtldataColumns(),
        columnDefs: StkDtlDataColumnRenders(),
        buttons: [
            {
                extend: "csv",
                className: "btn-sm",
                action: function (e, dt, button, config) {
                    var csvUrl = "/ApplicationLayer/FSNAnalysis/ExportCsv";
                    let CsvName = $("title").text() + "_" + moment().format('YYYYMMDDhhmmss');
                    Cmn_importDataToCSV(e, dt, button, config, csvUrl, CsvName);

                }

            }],
    });
}
function StkDtldataColumns() {
    var Array = [];

    Array = [
        { "data": "SrNo", "name": "SrNo", "autoWidth": true },
        { "data": "item_name", "name": "item_name", "autoWidth": true, "id": "tditem", className: "ItmNameBreak itmStick tditemfrz" },
        { "data": "uom_alias", "name": "uom_alias", "autoWidth": true, className: "" },
        { "data": "fst_op_stk", "name": "fst_op_stk", "autoWidth": true, className: "num_right" },
        { "data": "fst_inword_qty", "name": "fst_inword_qty", "autoWidth": true, className: "num_right" },
        { "data": "fst_sold_qty", "name": "fst_sold_qty", "autoWidth": true, className: "num_right" },
        { "data": "fst_cl_stk", "name": "fst_cl_stk", "autoWidth": true, className: "num_right" },
        { "data": "slw_op_stk", "name": "slw_op_stk", "autoWidth": true, className: "num_right" },
        { "data": "slw_inword_qty", "name": "slw_inword_qty", "autoWidth": true, className: "num_right" },
        { "data": "slw_sold_qty", "name": "slw_sold_qty", "autoWidth": true, className: "num_right" },
        { "data": "slw_cl_stk", "name": "slw_cl_stk", "autoWidth": true, className: "num_right" },
        { "data": "nn_op_stk", "name": "nn_op_stk", "autoWidth": true, className: "num_right" },
        { "data": "nn_inword_qty", "name": "nn_inword_qty", "autoWidth": true, className: "num_right" },
        { "data": "nn_sold_qty", "name": "nn_sold_qty", "autoWidth": true, className: "num_right" },
        { "data": "nn_cl_stk", "name": "nn_cl_stk", "autoWidth": true, className: "num_right" }
    ]
    return Array;
}
function StkDtlDataColumnRenders() {
    debugger;
    var array = [];
    array = [
        {
            targets: 1,
            render: (data, type, row) => `<div class="col-sm-11 no-padding" id="multiWrapper"><span id="ItmNameSpan">${data}</span></div>
                <div class="col-sm-1 i_Icon">
                    <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">  </button>
                    <input type="text" hidden id="hfItemID" value="${row.item_id}" />
                </div>`,
        },
        {
            targets: 2,
            render: (data, type, row) => `<span id="ItmUomSpan">${data}</span>`,
        },
        {
            targets: [4,8,12],
            render: (data, type, row) => parseFloat(CheckNullNumber(data)).toFixed(5) != "0.00000" ? `<div class="col-sm-11 lpo_form no-padding num_right" id="">${parseFloat(CheckNullNumber(data)).toFixed(cmn_ValDecDigit)}</div>
                <div class="col-sm-1 i_Icon">
                    <button type="button" id="FSNInwardDetail" class="calculator" data-toggle="modal" onclick="FSN_InSightDatail(event,'InwordDetail')" data-target="#FSNInwardDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_InwardDetail").text()}">  </button>
                </div>`:'',
        },
        {
            targets: [5, 9, 13],
            render: (data, type, row) => parseFloat(CheckNullNumber(data)).toFixed(5) != "0.00000" ? `<div class="col-sm-11 lpo_form no-padding num_right" id="">${parseFloat(CheckNullNumber(data)).toFixed(cmn_ValDecDigit)}</div>
                <div class="col-sm-1 i_Icon">
                    <button type="button" id="FSNSaleDetail" class="calculator" data-toggle="modal" onclick="FSN_InSightDatail(event,'SaleDetail')" data-target="#FSNSaleDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_SalesDetail").text()}">  </button>
                </div>`: '',
        },
        {
            targets: [6, 10, 14],
            render: (data, type, row) => parseFloat(CheckNullNumber(data)).toFixed(5) != "0.00000" ? `<div class="col-sm-11 lpo_form no-padding num_right" id="">${parseFloat(CheckNullNumber(data)).toFixed(cmn_ValDecDigit)}</div>
                 <div class="col-sm-1 i_Icon">
                    <button type="button" id="FSNStockDetail" class="calculator" data-toggle="modal" onclick="FSN_InSightDatail(event,'WhStockDetail')" data-target="#FSNStockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_StockDetail").text()}">  </button>
                </div>`: '',
        },
        {
            targets: [3,7,11],
            render: (data, type, row) => parseFloat(CheckNullNumber(data)).toFixed(5) != "0.00000" ? parseFloat(CheckNullNumber(data)).toFixed(cmn_ValDecDigit) : ''
        }
    ]
    return array;
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function onChangeFSNRange1() {

    $("#rg_slow_from").val("");
    onChangeFSNRange();

}
function onChangeFSNRange() {
    let fast_from = $("#rg_fast_from").val();
    let slow_from = $("#rg_slow_from").val();
    if (SaveFSNRange() == true) {
        let slow_to = parseFloat(CheckNullNumber(fast_from)) - 1;
        $("#rg_slow_to").val(slow_to);
        if (parseFloat(CheckNullNumber(slow_from)) > 0) {
            let non_from = parseFloat(CheckNullNumber(slow_from)) - 1;
            $("#rg_non_from").val(non_from);
        } else {
            $("#rg_non_from").val("");
        }
    }
}
function FSN_InSightDatail(e,flag) {
    try {
        debugger
        let row = $(e.target).closest("tr");
        let ReportType = $("#TransactionType").val();
        let FromDt = $("#FromDate").val();
        let upToDate = $("#calUpTodate").val();
        let ItmCode = row.find("#hfItemID").val();
        let ItmName = row.find("#ItmNameSpan").text();
        let ItmUom = row.find("#ItmUomSpan").text();
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/FSNAnalysis/GetFSN_InSightData",
                data: {
                    ReportType: ReportType
                    , FromDt: FromDt, upToDate: upToDate
                    , ItmCode: ItmCode, flag: flag
                },
                success: function (data) {
                    if (flag == "InwordDetail") {
                        $("#PopUp_FSNInwardDetail").html(data);
                        $("#Inst_InW_ItemName").val(ItmName);
                        $("#Inst_InW_UOM").val(ItmUom);
                        cmn_apply_datatable("#InstTbl_InwordDetails");
                    }

                    if (flag == "SaleDetail") {
                        $("#PopUp_FSNSaleDetail").html(data);
                        $("#Inst_Sl_ItemName").val(ItmName);
                        $("#Inst_Sl_UOM").val(ItmUom);
                        cmn_apply_datatable("#InstTbl_SalesDetails");
                    }

                    if (flag == "WhStockDetail") {
                        $("#PopUp_FSNStockDetail").html(data);
                        $("#Inst_Stk_ItemName").val(ItmName);
                        $("#Inst_Stk_UOM").val(ItmUom);
                        cmn_apply_datatable("#InstTbl_StkDetails");
                    }
                },
            });
    } catch (err) {
        console.log("Error : " + err.message);
    }
}

function SaveFSNRange() {
    let fast_from = $("#rg_fast_from").val();
    let slow_from = $("#rg_slow_from").val();
    let flagResult = true;
    if (parseFloat(slow_from) >= parseFloat(fast_from)) {
        $("#vmRange_2").text($("#InvalidRange").text());
        $("#vmRange_2").css("display", "block");
        $("#rg_slow_from").css("border", "1px solid red");
        flagResult = false;
    } else {
        $("#vmRange_2").text("");
        $("#vmRange_2").css("display", "none");
        $("#rg_slow_from").css("border-color", "#ced4da");
    }
    return flagResult;
}
function NumericRangeValue(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#PerDigit") == false) {
        return false;
    }
    return true;
}