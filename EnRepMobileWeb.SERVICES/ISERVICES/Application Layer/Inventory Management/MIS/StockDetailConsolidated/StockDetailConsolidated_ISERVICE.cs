using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetailConsolidated
{
    public interface StockDetailConsolidated_ISERVICE
    {
        DataTable GetItemsList(string GroupName, string CompID);
        DataTable GetItemsGroupList(string GroupName, string CompID);
        DataSet GetStockDetailConsolidatedList(string compId, string brId, string itemId, string itemGroupId, string asOnDate, string flag
            , int skip=0, int pageSiz=25, string searchValue="", string sortColumn="SrNo", string sortColumnDir="Asc", string CsvFlag="");
        DataSet GetWipStockDetailConsolidatedList(string compId, string brId, string itemId, string itemGroupId, string asOnDate, string flag);
        DataSet GetSubItemStockDetails(string comp_ID, string br_ID, string item_id, string flag, string AsOnDate);
    }
}
