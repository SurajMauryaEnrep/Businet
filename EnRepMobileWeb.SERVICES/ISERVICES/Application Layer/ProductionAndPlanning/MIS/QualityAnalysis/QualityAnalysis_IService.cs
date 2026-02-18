using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.QualityAnalysis
{
    public interface QualityAnalysis_IService
    {
        DataTable GetQualityAnalysisReport(string compId, string brId, string srcType, string itemId,
        string fromDate, string toDate, string showAs,string DocId);
        DataTable GetQADetailsByItemId(string compId, string brId, string srcType, string itemId,
      string fromDate, string toDate, string showAs, string DocId);
        DataTable GetItemsDetails(string compId, string brId);
        
    }
}
