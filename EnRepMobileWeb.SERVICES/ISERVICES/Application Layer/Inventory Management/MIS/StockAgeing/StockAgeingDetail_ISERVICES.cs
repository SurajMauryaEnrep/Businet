using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockAgeing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockAgeing
{
    public interface StockAgeingDetail_ISERVICES
    {
        string SaveAgeingRange(string CompId,string UserId, AgeingRanges ageingRanges);
        DataSet GetAgiengPageLoad(string CompId, string UserId);
        DataSet GetStockAgeingMISData(string compID, string br_id,string UserId, string ReportType, string itemGrpId, string itemPrtFloId
            , string hsnCode, string brnchId, string upToDate, int skip, int pageSize, string searchValue, string sortColumn, string sortColumnDir, string flag);
    }
}
