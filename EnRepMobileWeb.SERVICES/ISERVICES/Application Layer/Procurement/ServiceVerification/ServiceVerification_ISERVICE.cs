using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ServiceVerification;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ServiceVerification
{
   public interface ServiceVerification_ISERVICE
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
       DataSet GetAlldata(string CompID, string SuppName, string BranchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        DataSet GetServicePOList(string Supp_id, string CompId, string BrID);
        DataSet GetServicePODetail(string SPONo, string SPODate, string CompId, string BrID); String InsertSrVerificationDetail(DataTable SrVerificationHeader, DataTable SrVerificationItemDetails, DataTable SrVerAttachments);
        DataSet GetVerificationList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        DataSet GetServiceVerificationDetail(string CompId, string BrID, string VerNo, string VerDate, string UserID, string DocID);
        string InsertServiceVerificationApproveDetail(string DocNo, string DocDate, string Branch, string MenuID, string CompID, string ApproveID, string mac_id, string wf_status, string wf_level, string wf_remarks);
        string ServiceVerificationDelete(ServiceVerificationModel _SrVerModel, string CompID, string br_id, string DocumentMenuId);
        DataSet ServiceVerificationCancel(ServiceVerificationModel _SrVerModel, string CompID, string br_id, string mac_id);
    }
}
