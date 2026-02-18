using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MIS.MaterialTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.MIS.MaterialTracking
{
   public interface MaterialTracking_ISERVICE
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet BindProductNameInDDL(string CompID, string BrID, string ItmName);//Bind product name
        DataTable GetJobORDDocList(string Comp_ID, string Br_ID);
        DataTable GetMTTrackDetailList_Onload(string CompId, string BranchID, string SuppId, string OutOPProdctID, string JobOrdNo, MaterialTracking_Model _MaterialTracking_Model);
        DataTable GetMaterialTrackDetail(string CompId, string BranchID, string SuppId, string OutOPProdctID, string JobOrdNO, string FromDate, string ToDate);
        DataSet GetMTAllQtyItemDetailList(string Comp_ID, string Branch, string Type, string JobOrdNo, string hdnJobOrdDt,string MaterialID);

    }
}
