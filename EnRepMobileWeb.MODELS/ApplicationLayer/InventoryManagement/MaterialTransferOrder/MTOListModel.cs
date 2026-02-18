using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialTransferOrder
{
   public class MTOListModel
    {
        public string DocumentMenuId { get; set; }
        public string ListFilterData { get; set; }
        public string Title { get; set; }
        public string to_br { get; set; }
        public string to_wh { get; set; }
        public string MTO_FromDate { get; set; }
        public string MTO_ToDate { get; set; }
        public string TRF_Type { get; set; }
        public string FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string TRFStatus { get; set; }
        public string MTOSearch { get; set; }
        public string WF_status { get; set; }
        public List<Status> StatusList { get; set; }
        public List<MTOList> BindMTOList { get; set; }
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
    public class MTOList
    {
        public string TRFNo { get; set; }
        public string TRFDate { get; set; }
        public string TRFType { get; set; }
        public string FromBranch { get; set; }
        public string ToBranch { get; set; }
        public string FromWH { get; set; }
        public string ToWH { get; set; }
        public string TRFList_Stauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string trf_date { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
    }
}
