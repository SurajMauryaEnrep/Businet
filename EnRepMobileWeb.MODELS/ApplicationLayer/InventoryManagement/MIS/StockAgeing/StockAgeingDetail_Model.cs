using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockAgeing
{
    public class StockAgeingDetail_Model
    {
        public string Title { get; set; }
        public AgeingRanges ageingRanges { get; set; }
        public string ItemGroupId { get; set; }
        public string ItemPortFolioId { get; set; }
        public string HsnCode { get; set; }
        public string BranchId { get; set; }
        public string AsOnDate { get; set; }
        public List<ItemGroup> itemGroupsList { get; set; } = new List<ItemGroup>() { new ItemGroup {item_group_id="0",item_group_name="---Select---" } };
        public List<ItemPortFolio> itemPortFoliosList { get; set; }= new List<ItemPortFolio>() { new ItemPortFolio { portfolio_id = "0", portfolio_name = "---Select---" } };
        public List<HsnCode> hsnCodesList { get; set; }= new List<HsnCode>() { new HsnCode { hsn_code = "0",hsn_code_name="---Select---" } };
        public List<Branch> BranchList { get; set; }= new List<Branch>() { new Branch { br_id = "0",br_name="---Select---" } };
    }
    public class AgeingRanges
    {
        public int range_1 { get; set; } = 0;
        public int range_2 { get; set; } = 0;
        public int range_3 { get; set; } = 0;
        public int range_4 { get; set; } = 0;
        public int range_5 { get; set; } = 0;
    }
    public class ItemGroup
    {
        public string item_group_id { get; set; }
        public string item_group_name { get; set; }
    }
    public class ItemPortFolio
    {
        public string portfolio_id { get; set; }
        public string portfolio_name { get; set; }
    }
    public class HsnCode
    {
        public string hsn_code { get; set; }
        public string hsn_code_name { get; set; }
    }
    public class Branch
    {
        public string br_id { get; set; }
        public string br_name { get; set; }
    }
}
