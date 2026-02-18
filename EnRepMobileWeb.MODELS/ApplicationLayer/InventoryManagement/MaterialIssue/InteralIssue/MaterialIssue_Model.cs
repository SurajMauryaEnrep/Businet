using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialIssue
{
    public class GetModelDetails
    {
        public MaterialIssue_Model _MIModeldata { get; set; }
        public string MdataToDetail { get; set; }
        public string MRS_type { get; set; }
        public string MaterialIssueNo { get; set; }
        public string MaterialIssueDate { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string _mdlCommand { get; set; }
        public string Message { get; set; }
        public string WF_Status1 { get; set; }

    }
    public class URLModelDetails
    {
        public string MRS_type { get; set; }
        public string Tnstyp { get; set; }
        public string CMD { get; set; }
        public string Docid { get; set; }
        public string BtnName { get; set; }
        public string Doc_dt { get; set; }
        public string WF_sts { get; set; }
    }
    public class MaterialIssue_Model
    {
        public Boolean ReturnableFlag { get; set; }
        public string CancelledRemarks { get; set; }
        public string Ship_StateCode { get; set; }
        public string Ship_Gst_number { get; set; }
        public string bill_add_id { get; set; }
        public string Address { get; set; }
        public string sr_type { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string MISearch { get; set; }
        public DataTable Issuedtotabledata { get; set; }
        public string Issuedby { get; set; }
        public string issuetoIcon_partial { get; set; }
        public string WF_Status1 { get; set; }
        public string Title { get; set; }
        public string SearchIssueToName { get; set; }
        public string ListPendingCreateDocument { get; set; }
        public string DocumentMenuId { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string BtnName { get; set; }
        public string _mdlCommand { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string StockItemWiseMessage { get; set; }
        public string DeleteCommand { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string mod_id { get; set; }
        public string mod_dt { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string MRS_type { get; set; }
        public string ListFilterData1 { get; set; }
        public string SrcType { get; set; }
        public string MaterialIssueDate { get; set; }

        public string MaterialIssueNo { get; set; }
        public string CompId { get; set; }
        public string BrchID { get; set; }
        public string MaterialIssueRemarks { get; set; }
        public List<RequiredArea> RequiredAreaList { get; set; }
       

        public string RequiredArea { get; set; }

        public List<MRS_NO> MRS_NO_List { get; set; }

        public string MRS_No { get; set; }
        public string MRS_Dt { get; set; }
        public string FilterArea { get; set; }
        public string FilterMRSNo { get; set; }

        public string FilterRequisitionType { get; set; }

        public string MRS_Date { get; set; }

        public List<EntityType> EntityTypelist { get; set; }
        public List<Warehouse> WarehouseList { get; set; }

        public string IssueToCode { get; set; }
        public string IssueToName { get; set; }
        public string HDN_Issuedby { get; set; }
        public string EntityType { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string MaterialIssueItemDetails { get; set; }
        public bool CancelFlag { get; set; }
        public string ItmRWKJOFlag { get; set; }
        public string hidenRequiredArea { get; set; }
        public string hiddenMRS_No { get; set; }
        public string hiddenMRS_Date { get; set; }
        public string Dispatchthrough { get; set; }
        public string CheckDependcySampleIssue { get; set; }
        public string VehicleNo { get; set; }
        public List<Issuedby> IssuedbyList { get; set; }
    }
    public class Issuedby
    {
        public string Issuedby_id { get; set; }
        public string Issuedby_Name { get; set; }
    }
    public class EntityType
    {
        public string Entity_ID { get; set; }
        public string Entity_Name { get; set; }

        public string Entity_Type { get; set; }
    }
    public class MRS_NO
    {
        public string MaterialIssueNo { get; set; }
        public string MaterialIssueDate { get; set; }
    }
    public class RequiredArea
    {
        public int ReqArea_ID { get; set; }
        public string ReqArea_Name { get; set; }
    }
  
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class MaterialIssueList
    {
        public string WF_Status { get; set; }
        public string DocumentMenuId { get; set; }
        public string MISearch { get; set; }
        public string Title { get; set; }
        public List<EntityType> EntityTypelist { get; set; }
        public string EntityType { get; set; }
        public List<RequiredArea> RequiredAreaList { get; set; }
        public string RequiredArea { get; set; }
        public string MRS_type { get; set; }
        public string ListFilterData { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<Status> StatusList { get; set; }
        public string StatusCode { get; set; }
        public List<MaterialIssueDetail> MaterialIssueDetailList { get; set; }
        public string MaterialIssueTo { get; set; }
        public List<MaterialIssueTo> MaterialIssueToList { get; set; }

    }
    public class MaterialIssueTo
    {
        public string issue_to_id { get; set; }
        public string issue_to_name { get; set; }
    }
    public class Status
    {
        public string status_code { get; set; }
        public string status_name { get; set; }

    }
    public class MaterialIssueDetail
    {
        public string issue_type { get; set; }
        public string issuetype { get; set; }
        public string issue_no { get; set; }
        public string issue_dt { get; set; }
        public string issue_date { get; set; }
        public string issue_by { get; set; }
        public string issue_to { get; set; }
        public string requisition_no { get; set; }
        public string requisition_date { get; set; }
        public string entity_type { get; set; }
        public string app_dt { get; set; }
        public string create_dt { get; set; }
        public string mod_dt { get; set; }
        public string issue_status { get; set; }
        public string create_by { get; set; }
        public string mod_by { get; set; }

    }
   
}
