using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MRS
{
    public class MRSList_Model
    {
        public List<ItemName_List> ItemNameList { get; set; }
        public string ddl_ItemName { get; set; }
        public string DocumentMenuId { get; set; }
        public string Title { get; set; }
        public int req_area { get; set; }
        public string ListFilterData { get; set; }
        public string issue_to { get; set; }
        public string MRS_FromDate { get; set; }
        public string MRS_ToDate { get; set; }
        public string MRS_Type { get; set; }
        public string SRC_Type { get; set; }
        public string FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string MRSStatus { get; set; }
        public string ddlissueto { get; set; }
        public string entity_type { get; set; }
        public string MRSSearch { get; set; }
        public string WF_status { get; set; }


        public List<RequirementAreaList> RequirementAreaList { get; set; }
        public List<IssueIDList> IssueList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<MRSList> BindMRSList { get; set; }
    }
    public class ItemName_List
    {
        public string Item_ID { get; set; }
        public string Item_Name { get; set; }
    }
    public class RequirementAreaList
    {
        public int req_id { get; set; }
        public string req_val { get; set; }
    }
    public class IssueIDList
    {
        public string issue_id { get; set; }
        public string issue_val { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class MRSList
    {
        public string MRSNo { get; set; }
        public string MRSDate { get; set; }
        public string MRS_Dt { get; set; }
        public string MRSType { get; set; }
        public string ReqArea { get; set; }
        public string IssueTo { get; set; }
        public string SrcType { get; set; }
        public string SrcDocNo { get; set; }
        public string SrcDocDate { get; set; }
        public string SrcDocDt { get; set; }
        public string MRSList_Stauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
    }

}
