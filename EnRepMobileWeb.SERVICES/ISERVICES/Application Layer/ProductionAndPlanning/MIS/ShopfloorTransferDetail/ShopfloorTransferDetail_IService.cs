using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.ShopfloorTransferDetail
{
    public interface ShopfloorTransferDetail_IService
    {
        DataTable GetShflTrfReport(string compId, string brId, string transactionType, string materialType, string itemId,
        string fromDate, string toDate, string itemGroupName, string shopfloorId, string status);
        DataTable GetShflStatusList(string compId, string brId);
        DataTable GetShflStktrfBatchDetail(string compId, string brId, string trfNo, string trfDt, string itemId);
        DataSet TRF_GetSubItemDetails(string CompID, string Br_id, string ItemId, string trf_no, string trf_dt, string Flag);
    }
}
