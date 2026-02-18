using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ShopfloorSetup
{
   public class ShopfloorSetupModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string SaveUpd { get; set; }
        public string BtnName { get; set; }
        public string  Message { get; set; }
        public string Command { get; set; }
        public int Branchid { get; set; }
        public string sh_id { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public int shfl_id { get; set; }
        public string shfl_name { get; set; }
        public string shfl_loc { get; set; }
        public string shfl_remarks { get; set; }
       // public List<OpType> OpTypeList { get; set; }

        public string create_id { get; set; }
        public string create_dt { get; set; }
        public string mod_id { get; set; }
        public string mod_dt { get; set; }
        public string TransType { get; set; }
        public string DeleteCommand { get; set; }
        public string SO_ItemName { get; set; }

        //public string ItemAttrDetail { get; set; }
        public string shopfloorattrdetails { get; set; }
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string uom_id { get; set; }
        public string uom_alias { get; set; }
        public string optm_qty { get; set; }
        public string per_unit_val { get; set; }
        public string per_unit { get; set; }
        public string Title { get; set; }
        public string Guid { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public List<ProductionCapacity> ProductionCapacityList { get; set; }
        public List<ProductionHistory> ProductionHistoryList { get; set; }
        public string attatchmentdetail { get; set; }
    }
    public class ProductionHistory   /**Add by Nitesh 13102023 1014 for Production History Tabel **/
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string uom_id { get; set; }
        public string uom_alias { get; set; }
        public string reject_qty { get; set; }
        public string prod_qty { get; set; }
        public string cnf_dt { get; set; }
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
    public class Shopflore_Model
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class UrlModel
    {
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string Shp_id { get; set; }
        public string SV_No { get; set; }
        public string SV_DT { get; set; }
        public string DMS { get; set; }

    }
}
