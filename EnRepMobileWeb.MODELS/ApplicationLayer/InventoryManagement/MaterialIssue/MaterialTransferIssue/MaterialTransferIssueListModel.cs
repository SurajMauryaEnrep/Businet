using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialIssue.MaterialTransferIssue
{
   public class MaterialTransferIssueListModel
    {
        public string ListFilterData { get; set; }
        public string Title { get; set; }
        public int to_br { get; set; }
        public int to_wh { get; set; }
        public string MTO_FromDate { get; set; }
        public string MTO_ToDate { get; set; }
        public string TRF_Type { get; set; }
        public string FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string MTIStatus { get; set; }
        public string MTISearch { get; set; }
        public List<Status> StatusList { get; set; }
        public List<MTIList> BindMTIList { get; set; }
        public List<ToWharehouseList> ToWharehouseList { get; set; }
        public List<ToBranchList> ToBranchList { get; set; }

    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class ToWharehouseList
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }
    public class ToBranchList
    {
        public int br_id { get; set; }
        public string br_val { get; set; }
    }
    public class MTIList
    {
        public string Title { get; set; }
        public string MTINo { get; set; }
        public string MTIDate { get; set; }
        public string issue_date { get; set; }
        public string TRFType { get; set; }
        public string trfType { get; set; }
        public string ReqNo { get; set; }
        public string ReqDate { get; set; }
        public string FromWH { get; set; }
        public string ToBranch { get; set; }       
        public string ToWH { get; set; }
        public string MTIList_Stauts { get; set; }
        public string create_by { get; set; }      
        public string CreateDate { get; set; }      
        public string mod_dt { get; set; }      
        public string mod_by { get; set; }      
      
    }
}
