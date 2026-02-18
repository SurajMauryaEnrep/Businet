using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MaterialRequirementPlan
{
   public class MaterialRequirementPlan_Model
    {
        public string hdnsaveApprovebtn { get; set; }
        public string docmodified { get; set; }
        public string WF_Status { get; set; }
        public string WF_Status1 { get; set; }
        public string DocumentStatusfc { get; set; }
        public string MRPNumberfc { get; set; }
        public string MRPDatefc { get; set; }
        public string MRPSearch { get; set; }
        public string BtnNamefc { get; set; }
        public string Messagefc { get; set; }
        public string Commandfc { get; set; }
        public string TransTypefc { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public string MRPNumber { get; set; }
        public string RequisitionNumber { get; set; }
        public string MRPDate { get; set; }
        public string ActionCommand { get; set; }
        public string Title { get; set; }
        public string TransType { get; set; }
        public string CreatedBy { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string create_id { get; set; }
        public string SubItemDetailsDt_prcure { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string SFSubItemDetailsDt { get; set; }
        public int req_area { get; set; }
        public int reqarea { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string ddl_financial_year { get; set; }
        public string ddlfinancialyear { get; set; }
        public List<financial_year> ddl_financial_yearList { get; set; }
        public string ddl_period { get; set; }
        public string hdnddl_period { get; set; }
        public string ddlperiod { get; set; }
        public List<period> ddl_periodList { get; set; }
        public string txtFromDate { get; set; }
        public string txtToDate { get; set; }
        public string AdHocFromDate { get; set; }
        public string AdHocToDate { get; set; }
        public string hfFromDate { get; set; }
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string hfToDate { get; set; }
        public string ddl_src_type { get; set; }
        public string src_doc_type { get; set; }
        public Boolean CancelFlag { get; set; }
        public string ddl_srctype { get; set; }
        public string ddlsrctype { get; set; }
        public List<sourcetype> ddl_src_typeList { get; set; }
        public List<Status> StatusList { get; set; }
        public string ProductMaterialDetail { get; set; }
        public string ProductDetail { get; set; }
        public string hdnSFMaterialDetail { get; set; }
        public string hdnInputMaterialDetail { get; set; }
        public string SFMaterialDetail { get; set; }
        public string InputMaterialDetail { get; set; }
        public List<MRP_List> MRP_ListDetail { get; set; }
        public List<RequirementAreaList> _requirementAreaLists { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public List<PP_NumberList> PP_NoLists { get; set; }
        public string PP_Number { get; set; }
        public string PP_Date { get; set; }
        public string UserId { get; set; }
        public string ForAmmendendBtn { get; set; }
        public string Amendment { get; set; }
        public string ForDeleteBtn { get; set; }
        public string Amend { get; set; }
    }
    public class UrlModel
    {
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string MRP_No { get; set; }
        public string MRP_Date { get; set; }
        public string DMS { get; set; }
        public string Amend { get; set; }

    }
    public class PP_NumberList
    {
        public string PP_id { get; set; }
        public string PP_val { get; set; }
    }
    public class RequirementAreaList
    {
        public int req_id { get; set; }
        public string req_val { get; set; }
    }
    public class sourcetype
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class period
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class financial_year
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class MRP_List
    {
       
        public string mrp_no { get; set; }
        public string mrp_date { get; set; }
        public string source { get; set; }
        public string Req_area { get; set; }
        public string src_doc_no { get; set; }
        public string src_doc_date { get; set; }
        public string fy { get; set; }
        public string period { get; set; }
        public string daterange { get; set; }
        public string status { get; set; }
        public string createon { get; set; }
        public string approvedon { get; set; }
        public string amendedon { get; set; }
        public string createdby { get; set; }
        public string approvedby { get; set; }
        public string amendby { get; set; }
    }
}
