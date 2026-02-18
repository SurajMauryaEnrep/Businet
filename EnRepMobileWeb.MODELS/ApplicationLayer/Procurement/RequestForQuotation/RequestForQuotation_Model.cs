using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.RequestForQuotation
{
    public class RequestForQuotation_Model
    {
        public string hdnsaveApprovebtn { get; set; }
        public string PRData1 { get; set; }
        public string PRData { get; set; }
        public string Title { get; set; }
        public string RFQ_FromDate { get; set; }
        public string RFQ_ToDate { get; set; }
        public string SearchName { get; set; }
        public string SuppPros_type { get; set; }
        public string SourceType { get; set; }
        public string src_doc_no { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string doc_status { get; set; }
        public string RFQ_status { get; set; }
        public string RFQ_no { get; set; }
        public string RFQ_date { get; set; }
        public string PR_date { get; set; }
        public string src_doc_dt { get; set; }
        public string Valid_upto { get; set; }
        public string Delevery_Date { get; set; }
        public bool CancelFlag { get; set; }
        public string reqNo { get; set; }
        public string Remarks { get; set; }
        public string Itemdetails { get; set; }
        public string Suppdetail_Save { get; set; }
        public string DelShedDetailList { get; set; }
        public string DelsheduleColValHide { get; set; }
        public string TermAndConDetailsList { get; set; }
        public List<supplist> _supplist { get; set; }
        public List<StatusLists> statusLists { get; set; }
        public List<PRReqList> pRReqLists { get; set; }
        public string hdnAction { get; set; }
        public string attatchmentdetail { get; set; }
        public string DocumentStatus { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string RFQSearch { get; set; }
        public string AppStatus { get; set; }
        public string RFQ_Number { get; set; }
        public string ProspectFromRFQ { get; set; }
        public string ProspectFromQuot { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string WF_status { get; set; }
        public string WF_status1 { get; set; }
    }
    public class RequestForQuotationattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class PRReqList
    {
        public string PrNo { get; set; }
        public string PrDt { get; set; }
    }
    public class StatusLists
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class supplist
    {
        public string Supp_id { get; set; }
        public string Supp_Name { get; set; }
    }
}
