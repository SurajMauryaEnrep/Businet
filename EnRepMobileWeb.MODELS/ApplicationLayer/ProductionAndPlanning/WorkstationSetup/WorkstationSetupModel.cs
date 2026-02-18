using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.WorkstationSetup
{
   public class WorkstationSetupModel
    {
        public string hdnsaveApprovebtn;

        public string ddlgroup_name;
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public int ws_id { get; set; }
        public string ws_name { get; set; }
        public int shfl_id { get; set; }
       
        // public List<OpType> OpTypeList { get; set; }

        public string op_st_date { get; set; }
        public string op_name { get; set; }
        public string sr_no { get; set; }
        public string Make { get; set; }
        public string Model_no { get; set; }
        public string Group_name { get; set; }
        public int create_id { get; set; }
        public string create_name { get; set; }
        public string create_dt { get; set; }
        public int mod_id { get; set; }
        public string mod_name { get; set; }
        public string mod_dt { get; set; }
        public string TransType { get; set; }
        public string DeleteCommand { get; set; }
        public string SO_ItemName { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string ListFilterData { get; set; }
        public string ListFilterData1 { get; set; }
        public List<Status> StatusList { get; set; }
        public List<GroupName> GroupList { get; set; }
        public string attatchmentdetail { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string Command { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string SaveUpd { get; set; }
        public string DupName { get; set; }
        public string dbclick { get; set; }
        public string AppStatus { get; set; }
        public string DocumentMenuId { get; set; }
        public string WorkstationCap { get; set; }
        public string shopfloorattrdetails { get; set; }
        public List<ProductionCapacity> ProductionCapacityList { get; set; }

    }
    public class ProductionCapacity
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string uom_id { get; set; }
        public string uom_alias { get; set; }
        public string optm_qty { get; set; }
        public string per_unit_val { get; set; }
        public string per_unit { get; set; }
    }

    public class WorkstationSetupAttchModel
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    public class UrlModelData
    {
        public string dbclick { get; set; }
        public int ws_id { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string Msg { get; set; }
    }
    public class GroupName
    {
        public string item_grp_id { get; set; }
        public string ItemGroupChildNood { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
}
