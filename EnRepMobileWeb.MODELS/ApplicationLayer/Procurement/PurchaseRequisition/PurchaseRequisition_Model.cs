using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.PurchaseRequisition
{
    public class PurchaseRequisition_Model
    {
        public string SourceDocumentDate { get; set; }
        public string SourceDocumentNumber { get; set; }
        public string wfDisableAmnd { get; set; }
        public string OrdStatus { get; set; }
        public string Amend { get; set; }
        public string DocumentMenuId { get; set; }
        public string Amendment { get; set; }
        public string UserID { get; set; }
        public string ForAmmendendBtn { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string PRData1 { get; set; }
        public string PRData { get; set; }
        public int req_area { get; set; }
        public string Title { get; set; }
        public string PR_No { get; set; }
        public string Pr_type { get; set; }
        public string doc_status { get; set; }
        public bool CancelFlag { get; set; }
        public bool ForceClose { get; set; }
        public string DeleteCommand { get; set; }
        public string SourceType { get; set; }
        public string Req_number{get;set;}
        public DateTime Req_date { get; set; }
        public string PR_FromDate { get; set; }
        public string PR_ToDate { get; set; }
        public string SubItemDetailsDt { get; set; }
        
        public string cancelled { get; set; }
        public string ForceClosed { get; set; }
        public string QtyDigit { get; set; }/*Add by Hina on 23-09-2024 to show aval qty*/
        public string Req_By { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string PR_status { get; set; }
        public string Itemdetails { get; set; }
        public string PR_ItemName { get; set; }
        public string attatchmentdetail { get; set; }
        public List<RequirementAreaList> _requirementAreaLists { get; set; }
        public List<StatusList> statusLists { get; set; }
      
        public string DocumentStatus { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string PRSearch { get; set; }
        public string WF_status { get; set; }
        public string WF_status1 { get; set; }
        public string PR_Number { get; set; }
        public string PR_Date { get; set; }
        public string AppStatus { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
    }
    public class RequirementAreaList
    {
        public int req_id { get; set; }
        public string req_val { get; set; }
    }
    public class StatusList
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class PurchaseRequisitionattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
}
