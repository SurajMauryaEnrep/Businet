using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.MaterialTransferReceipt
{
   public class MaterialTransferReceiptModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string DocumentNo { get; set; }
        public string MenuDocumentId { get; set; }
        public DateTime MaterialReceiptDate { get; set; }
        public string MaterialReceiptNo { get; set; }
        public string CompId { get; set; }
        public string BrchID { get; set; }
        public string trf_type { get; set; }
        public string hdtrf_type { get; set; }
        public string from_br { get; set; }
        public string from_brid { get; set; }
        public string to_br { get; set; }
        public string hdto_brid { get; set; }
        public int from_wh { get; set; }
        public int hdfrom_whid { get; set; }
        public int to_wh { get; set; }
        public int hdto_whid { get; set; }
        public string issue_rem { get; set; }
        public string mr_status { get; set; }
        public string item_id { get; set; }
        public string uom_id { get; set; }
        public float trf_qty { get; set; }
        public float rec_qty { get; set; }
        public string it_remarks { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string mac_id { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string create_id { get; set; }
        public string create_dt { get; set; }
        public string app_id { get; set; }
        public string app_dt { get; set; }
        public string mod_dt { get; set; }
        public string mod_id { get; set; }
        public string FilterSourceWH { get; set; }
        public string FilterMTRNo { get; set; }
        public string FilterToWH { get; set; }
        public string FilterFromBR { get; set; }
        public string FilterTransferType { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string WFBarStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string WFStatus { get; set; }
        public string Createid { get; set; }
        public string MTIStatus { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string Itemdetails { get; set; }
        public string DeleteCommand { get; set; }
        public Boolean CancelFlag { get; set; }      
        public string Req_No { get; set; }
        public DateTime? Req_Date { get; set; }
        public string attatchmentdetail { get; set; }
        public string Issue_No { get; set; }
        public DateTime? Issue_Date { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string MaterialIssueItemDetails { get; set; }
        public string ListFilterData1 { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string AppStatus { get; set; }
        public string TMR_Number { get; set; }
        public string TMR_Date { get; set; }
        public string TMR_Type { get; set; }
        public string Guid { get; set; }
        public string WF_status1 { get; set; }
        public string docid { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        

        public List<Req_NO> Req_NO_List { get; set; }
        public List<MI_NO> MI_NO_List { get; set; }
        public List<FromWharehouse> FromWharehouseList { get; set; }       
        public List<FromBranch> FromBranchList { get; set; }
        public List<ToWharehouse> ToWharehouseList { get; set; }
        public string hdIssue_No { get; set; }
        public string hdIssue_dt { get; set; }
        public string hdReq_No { get; set; }
        public string hdReq_Dt { get; set; }
        public string hdto_bridName { get; set; }

    }
    public class URLModelDetails
    {
        public string TransType { get; set; }
        public string Command { get; set; }
        public string TMR_Number { get; set; }
        public string BtnName { get; set; }
        public string TMR_Date { get; set; }
        public string WF_status1 { get; set; }
    }
    public class MTRCModelAttch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }

    public class FromWharehouse
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }    
    public class Req_NO
    {
        public string RequirementNo { get; set; }
        public string RequirementDate { get; set; }
    }
    public class MI_NO
    {
        public string MINo { get; set; }
        public string MIDate { get; set; }
    }
    public class FromBranch
    {
        public int br_id { get; set; }
        public string br_val { get; set; }
    }
    public class ToWharehouse
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }

    public class TMR_ListModel
    {
        public string Title { get; set; }            
        public string TMR_FromDate { get; set; }
        public string TMR_ToDate { get; set; }
        public string TrfType { get; set; }
        public string from_br { get; set; }       
        public string from_wh { get; set; }
        public string Status { get; set; }
        public string ListFilterData { get; set; }
        public string FromDate { get; set; }
        public string MTRSearch { get; set; }
        public string WF_status { get; set; }
        public DateTime ToDate { get; set; }        
        public List<Status> StatusList { get; set; }
        public List<FromWharehouseList> FromWharehouseListPage { get; set; }
        public List<FromBranchList> FromBranchListPage { get; set; }
        public List<TMRList> TMRList { get; set; }

    }

    public class FromBranchList
    {
        public int br_id { get; set; }
        public string br_val { get; set; }
    }
    public class FromWharehouseList
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class TMRList
    {
        public string MRNo { get; set; }
        public string MRDate { get; set; }
        public string MR_Dt { get; set; }
        public string TrfType { get; set; }
        public string Trf_Type { get; set; }
        public string SourceBranch { get; set; }
        public string SourceWH { get; set; }      
        public string Stauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string mod_by { get; set; }
        public string app_by { get; set; }
        public string create_by { get; set; }
    }
}
