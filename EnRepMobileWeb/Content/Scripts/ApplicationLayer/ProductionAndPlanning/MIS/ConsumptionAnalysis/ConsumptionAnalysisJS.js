
$(document).ready(function () {
    BindProductNameDDL();
    BindGroupName();
    $('#ddlShopfloor').select2();
    $('#ddloperation').select2();
    var fromDate = $('#txtFromdate').val();
    $('#txtTodate').attr('min', fromDate);
});

function validateToDate() {
    var fromDate = $('#txtFromdate').val();
    $('#txtTodate').attr('min', fromDate);
}
function BindProductNameDDL() {
    debugger;
    DynamicSerchableItemDDL("", "#Ddl_MaterialName", "", "", "", "PA");
}
function BindGroupName() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ConsumptionAnalysis/GetGroupNameInDDL",
            dataType: "json",
            success: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                var s = "";
                $.map(data, function (val, item) {
                    s += "<option value=" + val.ID + ">" + val.Name + "</option>";
                });
                $("#Ddl_GroupName").html(s);
                $("#Ddl_GroupName").select2();
            },
        });
}
function BtnSearch() {
    debugger;
    var ItemName = $("#Ddl_MaterialName").val();
    var txtFromdate = $("#txtFromdate").val();
    var txtTodate = $("#txtTodate").val();
    var Groupname = $("#Ddl_GroupName").val();
    var shflId = $("#ddlShopfloor").val();
    var opId = $("#ddloperation").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ConsumptionAnalysis/GetConsumption_DetailsByFilter",
        data: {
            ItemName: ItemName, txtFromdate: txtFromdate, txtTodate: txtTodate, Groupname: Groupname, shflId: shflId, opId: opId
        },
        success: function (data) {
            $("#Tbl_ConsumptionDetails").html(data);
        }
    })
}

function OnclickTotalConsumedQty(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    var ItemID = currentRow.find("#item_id").text();
    var shflId = currentRow.find("#hdnShflId").text();
    var opId = currentRow.find("#hdnOpId").text();
    var ItemName = currentRow.find("#item_name").text();
    var UOM = currentRow.find("#uom_alias").text();
    var ConsumptionQuantity = currentRow.find("#Cons_qty").text().trim();
    var txtFromdate = $("#txtFromdate").val();
    var txtTodate = $("#txtTodate").val();
    var txtGroup = $("#txtShowAs").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ConsumptionAnalysis/GetConsumedMeterialDetail",
        data: {
            ItemId: ItemID, From_dt: txtFromdate, To_dt: txtTodate, Group: txtGroup, shflId: shflId, opId: opId
        },
        success: function (data) {
            $("#ConsumptionDetailPopUp").html(data);

            cmn_apply_datatable("#tblConsumpDetail");

            $("#MaterialName").val(ItemName);
            $("#Material_id").val(ItemID);
            $("#UOM").val(UOM);
            $("#ConsumptionQuantity").val(ConsumptionQuantity);
           
        }
    })
}

function OnClickConsuptionQty(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    var cnf_no = currentRow.find("#cnf_no").text().trim();
    var cnf_dt = currentRow.find("#cnf_dt").text().trim();
    var Product_Id = currentRow.find("#product_id").text().trim();
    var ConsumptionQuantity = currentRow.find("#Cons_Qty").text().trim();
    var Material_item_id = $("#Material_id").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ConsumptionAnalysis/GetLotDetail",
        data: {
            cnf_no: cnf_no, cnf_dt: cnf_dt, Product_Id: Product_Id, Material_item_id: Material_item_id
        },
        success: function (data) {
            $("#LotDetailPopUp").html(data);
            cmn_apply_datatable("#tbl_LotDetail");

            var ItemName = $("#MaterialName").val();
            var UOM = $("#UOM").val();
           
            $("#Lot_MaterialName").val(ItemName);
            $("#Lot_UOM").val(UOM);
            $("#Lot_Quantity").val(ConsumptionQuantity);
        }
    })
}

