using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MIS.JobOrderTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.MIS.JobOrderTracking
{
   public interface JobOrderTracking_ISERVICE
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet BindProductNameInDDL(string CompID, string BrID, string ItmName);//Bind product name
        DataTable GetJOTrackDetailList_Onload(string CompId, string BranchID, string SuppId, string FinishProdctID, string OutOPProdctID, JobOrderTracking_Model _JobOrderTracking_Model, string Status);
        DataTable GetJOTrackDetail(string CompId, string BranchID, string SuppId, string FinishProdctID, string OutOPProdctID, string FromDate, string ToDate, string Status);
        DataSet GetAllQtyItemDetailList(string Comp_ID, string Branch, string Type, string JobOrdNo, string hdnJobOrdDt);
        DataSet GetMtrlDispRawMaterialDetailList(string Comp_ID, string Branch, string JobOrdNo, string JobOrdrDate, string DispatchNo, string hdnDispatchDt);
        
    }
}
