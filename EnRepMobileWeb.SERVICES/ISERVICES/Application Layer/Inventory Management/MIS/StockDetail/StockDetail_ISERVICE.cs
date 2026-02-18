using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetail
{
    public interface StockDetail_ISERVICE
    {
        Dictionary<string, string> ItemList(string GroupName, string CompID);
        Dictionary<string, string> ItemGroupList(string GroupName, string CompID);
        DataSet GetAllBrchList(string CompID,string User_id);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataTable GetStockDetailList(string CompId, string BranchID, string IncludeZeroStockFlag, string StockBy, string ItemId, string ItemGrpId
            , string WarehouseID, string UpToDate,string PortfolioId, string hsnCode,string ExpiredItems,string StockoutItems
            ,string NearExpiryItm,string StkGlAccId,string ItemAlias, string BranchIDLis,string  Supp_Name);
        DataSet GetItemReceivedList(string CompId, string BranchID, string TransType, string StockBy, string ItemId, string WarehouseID, string Lot, string BatchNo, string SerialNo, string UpToDate);
        Dictionary<string, string> ItemPortfolioList(string GroupName, string CompID);
        DataSet GetSubItemStkList(string CompId, string BranchID, string StockBy, string ItemId, string WarehouseID, string UpToDate,string IncludeZero);
        DataSet Get_SuppNameDetails(string comp_id, string Br_ID);
    }
}
