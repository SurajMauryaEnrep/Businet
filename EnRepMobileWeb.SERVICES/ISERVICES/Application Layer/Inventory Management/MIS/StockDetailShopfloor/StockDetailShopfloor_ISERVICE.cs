using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetailShopfloor
{
    public interface StockDetailShopfloor_ISERVICE
    {
        Dictionary<string, string> ItemList(string GroupName, string CompID);
        Dictionary<string, string> ItemGroupList(string GroupName, string CompID);
        DataSet GetAllBrchList(string CompID, string User_id);
        DataSet GetShopfloorList(string CompId, string BrID);
        DataSet GetStockDetailShopfloorList(string CompId, string BranchID, string IncludeZeroStockFlag, string StockBy
            , string ItemId, string ItemGrpId, string ShflID, string UpToDate,string PortfolioId, string hsnCode
            , int skip, int pageSize, string searchValue, string sortColumn, string sortColumnDir, string BranchIDList, string flag ="");
        DataSet GetItemReceivedList(string CompId, string BranchID, string TransType, string StockBy, string ItemId, string @ShflID, string Lot, string BatchNo, string SerialNo, string UpToDate);
        Dictionary<string, string> ItemPortfolioList(string GroupName, string CompID);
        DataSet GetSubItemStkList(string CompId, string BranchID, string StockBy, string ItemId, string WarehouseID, string UpToDate,string IncludeZero);

    }
}
