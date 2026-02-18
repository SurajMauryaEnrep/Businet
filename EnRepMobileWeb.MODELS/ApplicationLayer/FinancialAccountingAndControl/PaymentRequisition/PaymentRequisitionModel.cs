using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.PaymentRequisition
{
    public class PaymentRequisitionModel
    {
        public bool CancelFlag { get; set; }
      
        public string CancelledRemarks { get; set; }
        public string DashbordPendingStatus { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string FilterDataList { get; set; }
        public string SearchFlag { get; set; }
        public string UserID { get; set; }
        public string DisabledPage { get; set; }
        public string ListFilterData1 { get; set; }
        public string DocumentMenuId { get; set; }
        public string Create_id { get; set; }
        public string WFStatus { get; set; }
        public string WFBarStatus { get; set; }
        public string DocumentStatus { get; set; }
        public string DeleteCommand { get; set; }
        public string Curren_name { get; set; }
        public string Currency_id { get; set; }
        public string Currency_name { get; set; }
        public string attatchmentdetail { get; set; }
        public string VoucherNumber { get; set; }
        public string PaidOn { get; set; }
        public string PaymentMode { get; set; }
        public string doc_status { get; set; }
        public string PR_status { get; set; }
        public string AmmendedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string AppStatus { get; set; }
        public string BtnName { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string PaidAmount { get; set; }
        public string remarks { get; set; }
        public string RequestedAmount { get; set; }
        public string Purpose { get; set; }
        public string Req_By { get; set; }
        public string Req_date { get; set; }
        public string RequisitionNumber { get; set; }
       
        public string WF_status { get; set; }
        public string Title { get; set; }
        public string req_area { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public List<Status> StatusList { get; set; }
        public List<RequirementAreaList> _requirementAreaLists { get; set; }
        public List<CurrencyNameLIst> _currencyNameList { get; set; }

    }
    public class PaymentRequisitionattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    public class CurrencyNameLIst
    {
        public int curr_id { get; set; }
        public string curr_name { get; set; }
    }
    public class RequirementAreaList
    {
        public int req_id { get; set; }
        public string req_val { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
}
