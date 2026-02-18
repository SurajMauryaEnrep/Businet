using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesEnquiry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesEnquiry
{
    public interface SalesEnquiry_ISERVICES
    {
        Dictionary<string, string> GetCustomerList(string CompID, string CustomerName, string BranchID, string CustProstype, string Enquiry_type);
        DataSet GetAllData(string CompID, string CustomerName, string BranchID, string CustProstype, string Enquiry_type, string SPersonName);
        string InsertSE_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblCommunicationDetail,DataTable dtSubItem, DataTable DtblAttchDetail);
        DataSet Edit_SEDetail(string CompId, string BrID, string DocumentMenuID, string UserID, string SENo);
        DataSet GetSlsEnqryListandSrchDetail(string CompId, string BrchID, SEListModel _SEListModel, string UserID, string DocumentMenuId);
        string SEdetailDelete(SalesEnquiryModel SEModel, string CompId, string BrID);
        DataSet GetPrintDeatils(string CompID, string BrchID, string EnquiryNo, string EnquiryDt);
        DataSet SE_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt/*, string Flag*/);

    }
}
