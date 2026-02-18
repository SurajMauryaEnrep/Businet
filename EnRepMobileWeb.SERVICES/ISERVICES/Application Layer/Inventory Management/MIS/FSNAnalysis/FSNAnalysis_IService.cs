using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.FSNAnalysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.FSNAnalysis
{
    public interface FSNAnalysis_IService
    {
        DataSet GetFSNAnalysisPageLoad(string compID,string BrId, string userId);
        DataSet GetFSNAnalysisMISData(string compID, string brId, string userId, string reportType, string itemGrpId, string itemPrtFloId,string fromDate, string upToDate, int skip, int pageSize, string searchValue, string sortColumn, string sortColumnDir, string flag);
        string SaveFSNRange(string compID, string userId, FSNRanges model);
        DataSet GetFSN_InSightDatail(string compID, string br_id, string ReportType
                    , string FromDt, string upToDate, string ItmCode, string flag);
    }
}
