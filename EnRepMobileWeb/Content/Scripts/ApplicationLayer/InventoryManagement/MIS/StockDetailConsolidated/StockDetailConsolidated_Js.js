$(document).ready(function () {
    DynamicSerchableItemDDL("", "#ddlItemList", "", "", "", "AllItems");
    //$('#ddlItemList').select2();//Commented by Suraj Maurya on 05-03-2025
   // $('#ddlItemGroupList').select2();
    Cmn_initializeMultiselect(["#ddlItemGroupList"]);
    //GetStockDetailsConsolidatedData();//Commented by Suraj Maurya on 05-03-2025
});

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hfItemID").text();
    ItemInfoBtnClick(ItmCode);
}
function GetStockDetailsConsolidatedData() {
    debugger;
    var itemId = $('#ddlItemList').val();
    var itemGroupId = $('#ddlItemGroupList').val();
    //var itemGroupId = cmn_getddldataasstring('#ddlItemGroupList');
    var asonDate = $('#txtAsOnDate').val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/StockDetailConsolidated/SearchStockDetailsConsolidated",
        data: {
            itemId: itemId,
            itemGroupId: itemGroupId,
            asonDate: asonDate
        },
        cache: false,
        async: true,
        success: function (data) {
            $('#PartialStockDetailConsolidatedList').html(data);
            //hideLoader();
            cmn_apply_datatable("#DataTable1");
        }
    });
}
function GetStockDetailsOnClickIbtn(e, whOrShfl) {
    debugger;
    if (whOrShfl == "WIPDetail") {
        var clickedrow = $(e.target).closest("tr");
        var ItmCode = clickedrow.find("#hfItemID").text();
        var tdItemName = clickedrow.find("#tdItemName").text();
        var tdUomName = clickedrow.find("#tdUomName").text();
        var asOnDate = $("#txtAsOnDate").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/StockDetailConsolidated/GetShopfloorWIPStockDetail",
            data: {
                itemId: ItmCode,
                asonDate: asOnDate,
                flag: whOrShfl
            },
            success: function (data) {
                $('#PartialWIPDetail').html(data);
                $('#txtItemName11').val(tdItemName);
                $('#txtUom11').val(tdUomName);
                hideLoader();
                cmn_apply_datatable("#DataTable2");
            }
        });
    }
    else {
        var clickedrow = $(e.target).closest("tr");
        var ItmCode = clickedrow.find("#hfItemID").text();
        var tdItemName = clickedrow.find("#tdItemName").text();
        var tdUomName = clickedrow.find("#tdUomName").text();
        var asOnDate = $("#txtAsOnDate").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/StockDetailConsolidated/SearchStockOnIBtnClick",
            data: {
                itemId: ItmCode,
                asonDate: asOnDate,
                flag: whOrShfl
            },
            success: function (data) {
                $('#StockDetailConsolidatedPopUp').html(data);
                $('#txtItemName').val(tdItemName);
                $('#txtUom').val(tdUomName);
                hideLoader();
                cmn_apply_datatable("#DataTable2");
            }
        });
    }    
}

/*------------Add By Hina on 04-09-2024 to work for Sub Item---------------------*/
/***--------------------------------Sub Item Section-----------------------------------------***/

function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var ProductId = clickdRow.find("#hfItemID").text();
    var ProductNm = clickdRow.find("#tdItemName").text();
    var UOM = clickdRow.find("#tdUomName").text();
    var AsOnDate = $("#txtAsOnDate").val();
    var ItemQty = 0;
   /* -----WAREHOUSE DATA-----------*/
    if (flag == "Wh_AvlStkQty") {
        ItemQty = clickdRow.find("#WhAvlStk").text();
    }
    else if (flag == "Wh_ResrvStkQty") {
        ItemQty = clickdRow.find("#WhReservedStk").text();
    }
    else if (flag == "Wh_RejStkQty") {
        ItemQty = clickdRow.find("#WhRejectStk").text();
    }
    else if (flag == "Wh_RewrkStkQty") {
        ItemQty = clickdRow.find("#WhReworkStk").text();
    }
    else if (flag == "Wh_TotalStkQty") {
        ItemQty = clickdRow.find("#WhTotalStk").text();
    }
    /* -----SHOPFLOOR DATA-----------*/
    else if (flag == "Sf_AvlStkQty") {
        ItemQty = clickdRow.find("#ShflAvlStk").text();
    }
    else if (flag == "Sf_RejStkQty") {
        ItemQty = clickdRow.find("#ShflRejectStk").text();
    }
    else if (flag == "Sf_RewrkStkQty") {
        ItemQty = clickdRow.find("#ShflReworkStk").text();
    }
    else if (flag == "Sf_WIPStkQty") {
        ItemQty = clickdRow.find("#WIPStock").text();
    }
    else if (flag == "Sf_TotalStkQty") {
        ItemQty = clickdRow.find("#ShflTotalStk").text();
    }
    /* -----CONSOLIDATED DATA-----------*/
    else if (flag == "Cons_AvlStkQty") {
        ItemQty = clickdRow.find("#ConsTotalAvlStk").text();
    }
    else if (flag == "Cons_ResrvStkQty") {
        ItemQty = clickdRow.find("#ConsTotalResrvStk").text();
    }
    else if (flag == "Cons_RejStkQty") {
        ItemQty = clickdRow.find("#ConsTotalRejctStk").text();
    }
    else if (flag == "Cons_RewrkStkQty") {
        ItemQty = clickdRow.find("#ConsTotalRewrkStk").text();
    }
    else if (flag == "Cons_WIPStkQty") {
        ItemQty = clickdRow.find("#ConsTotalWIPStk").text();
    }
    else if (flag == "Cons_TotalStkQty") {
        ItemQty = clickdRow.find("#ConsTotalStk").text();
    }
    ItemQty = ItemQty.trim();
$.ajax({
        type: "POST",
        url: "/ApplicationLayer/StockDetailConsolidated/GetSubItemStockDetails",
        data: {
            Item_id: ProductId,
            Flag: flag,
            AsOnDate: AsOnDate
        },
        success: function (data) {
            debugger;
            $("#SubItemStockPopUp").html(data);
            $("#Stk_Sub_ProductlName").val(ProductNm);
            $("#Stk_Sub_ProductlId").val(ProductId);
            $("#Stk_Sub_serialUOM").val(UOM);
            $("#Stk_Sub_Quantity").val(ItemQty);
        }
    });

}



/***--------------------------------Sub Item Section End-----------------------------------------***/