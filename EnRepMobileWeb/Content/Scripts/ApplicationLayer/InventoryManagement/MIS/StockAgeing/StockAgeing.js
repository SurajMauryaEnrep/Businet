$(document).ready(function () {
   // $("#ddlItemGroupName").select2();
    //$("#ItemPortfolio").select2();
    //$("#ddlHsnCode").select2();
    //$("#ddlBranch").select2();

    StockAgeingDtList();
    Cmn_initializeMultiselect(['#ddlItemGroupName', '#ItemPortfolio', '#ddlHsnCode', '#ddlBranch']);
});
function BtnSearch() {
    StockAgeingDtList();
}
function StockAgeingDtList() {
    debugger;
    let ReportType = "D";/*$("#RT_Summary").is("checked") ? "S" : $("#RT_Detail").is("checked") ? "D" : "S";//Default value is 'S';*/
    //let itemGrpId = $("#ddlItemGroupName option:selected").val();
    //let ItemPrtFloId = $("#ItemPortfolio option:selected").val();
    //let hsnCode = $("#ddlHsnCode option:selected").val();
    //let BrnchId = $("#ddlBranch option:selected").val();

    let itemGrpId = cmn_getddldataasstring("#ddlItemGroupName");
    let ItemPrtFloId = cmn_getddldataasstring("#ItemPortfolio");
    let hsnCode = cmn_getddldataasstring("#ddlHsnCode");
    var BrnchId = cmn_getddldataasstring("#ddlBranch");
    let upToDate = $("#calUpTodate").val();

    //let filters = "";
    //[ReportType, itemGrpId, ItemPrtFloId, hsnCode, BrnchId, upToDate].map((item, index, data) => {
    //    filters += data.length - 1 == index ? item : item + ",";
    //});

    var filterArray = [ReportType, itemGrpId, ItemPrtFloId, hsnCode, upToDate, BrnchId];
    var filters = filterArray.map(x => `[${x}]`).join('_');

    $("#Tbl_StockAgeingDetail").DataTable().destroy();
    $("#Tbl_StockAgeingDetail").DataTable({
        dom: "Blfrtip",
        "pageLength": 50,
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/ApplicationLayer/StockAgeing/LoadData",
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
                    var csvUrl = "/ApplicationLayer/StockAgeing/ExportCsv";
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
        { "data": "itemname", "name": "itemname", "autoWidth": true, "id": "tditem", className: "ItmNameBreak itmStick tditemfrz" },
        { "data": "uomname", "name": "uomname", "autoWidth": true, className: "" },
        { "data": "brname", "name": "brname", "autoWidth": true },
        { "data": "brid", "name": "brid", "autoWidth": true, className: "hidden" },
        { "data": "HSNCode", "name": "HSNCode", "autoWidth": true },
        { "data": "item_grp_name", "name": "item_grp_name", "autoWidth": true },
        { "data": "item_grp_id", "name": "item_grp_id", "autoWidth": true, className: "hidden" },
        { "data": "whname", "name": "whname", "autoWidth": true },
        { "data": "lotno", "name": "lotno", "autoWidth": true },
        { "data": "batchno", "name": "batchno", "autoWidth": true },
        { "data": "exp_dt", "name": "exp_dt", "autoWidth": true },
        { "data": "serialno", "name": "serialno", "autoWidth": true },
        { "data": "Inword_dt", "name": "Inword_dt", "autoWidth": true },
        { "data": "Inword_type", "name": "Inword_type", "autoWidth": true },
        { "data": "range_1", "name": "range_1", "autoWidth": true, className: "num_right" },
        { "data": "range_2", "name": "range_2", "autoWidth": true, className: "num_right" },
        { "data": "range_3", "name": "range_3", "autoWidth": true, className: "num_right" },
        { "data": "range_4", "name": "range_4", "autoWidth": true, className: "num_right" },
        { "data": "range_5", "name": "range_5", "autoWidth": true, className: "num_right" },
        { "data": "range_6", "name": "range_6", "autoWidth": true, className: "num_right" },
        { "data": "avlstk", "name": "avlstk", "autoWidth": true, className: "num_right" },
        { "data": "avlstk_in_sp_uom", "name": "avlstk_in_sp_uom", "autoWidth": true, className: "" },
        { "data": "avlstkvalue", "name": "avlstkvalue", "autoWidth": true, className: "num_right" },
    ]
    return Array;
}
function StkDtlDataColumnRenders() {
    debugger;
    var array = [];
    array = [
        {
            targets: 1,//SrNo
            render: (data, type, row) => `<div class="col-sm-11 no-padding" id="multiWrapper"><span id="ItmNameSpan">${data}</span></div>
                <div class="col-sm-1 i_Icon">
                    <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">  </button>
                    <input type="text" hidden id="hfItemID" value="${row.itemid}" />
                </div>`,
        },
        {
            targets: 11,
            render: (data, type, row) => (data == '01-Jan-1900' || data == '01-01-1900') ? `` : data
        },
        {
            targets: [15, 16, 17, 18, 19, 20, 21, 23],
            render: (data, type, row) => parseFloat(CheckNullNumber(data)).toFixed(cmn_ValDecDigit)
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