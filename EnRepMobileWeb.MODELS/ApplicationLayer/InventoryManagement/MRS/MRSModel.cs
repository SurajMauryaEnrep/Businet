using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MRS
{
    public class MRSModel
    {
        public string sr_type { get; set; }
        public List<SrcDocNoList> docNoLists { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string hdnCommand { get; set; }
        public string src_type { get; set; }
        public string Out_PutItm { get; set; }
        public string Pro_order_dt { get; set; }
        public string Pro_order_Num { get; set; }
        public string ListFilterData1 { get; set; }
        public string Title { get; set; }
        public string DocumentNo { get; set; }
        public string MenuDocumentId { get; set; }
        public string ddlissueto { get; set; }
        public string mrs_no { get; set; }
        public DateTime mrs_dt { get; set; }
        public string mrs_type { get; set; }
        public string Search { get; set; }
        public int req_area { get; set; }
        public string issue_to { get; set; }
        public string mrs_rem { get; set; }
        public string SrcType { get; set; }
        public string SrcDocNo { get; set; }
        public string SrcDocDt { get; set; }
        public string mrs_status { get; set; }
        public string entity_type { get; set; }
        public string item_id { get; set; }
        public string uom_id { get; set; }
        public float mrs_qty { get; set; }
        public float issue_qty { get; set; }
        public string it_remarks { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string mac_id { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string AppStatus { get; set; }
        public string create_id { get; set; }
        public string create_dt { get; set; }
        public string app_id { get; set; }
        public string app_dt { get; set; }
        public string mod_dt { get; set; }
        public string mod_id { get; set; }

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
        public string StatusCode { get; set; }
        public string MRSStatus { get; set; }
        public string Itemdetails { get; set; }
        public string DeleteCommand { get; set; }
        public Boolean CancelFlag { get; set; }
        public Boolean ForceClose { get; set; }
        public string Mrs_ItemName { get; set; }
        public string attatchmentdetail { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string MRSNo { get; set; }
        public string WF_status1 { get; set; }
        public string Guid { get; set; }
        public string docid { get; set; }
        public DateTime MRSDate { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public List<RequirementArea> RequirementAreaList { get; set; }
        public List<IssueID> IssueList { get; set; }

    }
    public class SrcDocNoList
    {
        public string SrcDocnoId { get; set; }
        public string SrcDocnoVal { get; set; }
    }
    public class MTSModelAttch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class RequirementArea
    {
        public int req_id { get; set; }
        public string req_val { get; set; }
    }
    public class IssueID
    {
        public string issue_id { get; set; }
        public string issue_val { get; set; }
    }
    public class UrlModel {
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string MRSNo { get; set; }
        public DateTime MRSDate { get; set; }
        public string TransType { get; set; }
        public string WF_status1 { get; set; }
        public string AppStatus { get; set; }
    }

}
