using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ServiceVerification
{
    public class ServiceVerificationModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string DocumentMenuId { get; set; }
        public string Title { get; set; }
        //public string documentStatus { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public string Delete { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string ItemName { get; set; }
        public string TransType { get; set; }
        public string SrVerNo { get; set; }
        public string SrVerDate { get; set; }
        public string Src_Type { get; set; }
        public string SrcDocNo { get; set; }
        public string SrcDocDate { get; set; }
        public Boolean Cancelled { get; set; }
        public List<SourceDoc> SourceDocList { get; set; }
        public string SrVerStatus { get; set; }
        public string Address { get; set; }
        public int bill_add_id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string PriceBasis { get; set; }
        public string SystemDetail { get; set; }
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string StatusName { get; set; }
        public string Itemdetails { get; set; }
        public string attatchmentdetail { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string WFStatus1 { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string ListFilterData1 { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string AppStatus { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string WF_status1 { get; set; }
        //public string WF_Docid1 { get; set; }
        public string ILSearch { get; set; }

    }
    public class RequestForServiceattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class SourceDoc
    {
        public string doc_no { get; set; }
        public string doc_dt { get; set; }
    }

    public class ServiceVerificationListModel
    {

        public string Title { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public string ListFilterData  { get; set; }
        public string Ver_FromDate { get; set; }
        public string Ver_ToDate { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string wfdocid { get; set; }
        public string wfstatus { get; set; }
        public string SrVerNo { get; set; }
        public string SrVerDate { get; set; }
        public string attatchmentdetail { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<ServiceVerificationList> VerificationList { get; set; }
        public string VerSearch { get; set; }
        public string BtnName { get; set; }
        public DateTime FinStDt { get; set; }
        public string WF_status { get; set; }
        //public string WF_Docid { get; set; }
        public string WF_Stat { get; set; }
        public string Message { get; set; }

    }

    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }   
    public class ServiceVerificationList
    {
        public string VerNo { get; set; }
        public string VerDate { get; set; }
        public string VerDt { get; set; }       
        public string SourceDocNo { get; set; }
        public string SourceDocDt { get; set; }
        public string SuppName { get; set; }       
        public string VerStauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
    }
}
