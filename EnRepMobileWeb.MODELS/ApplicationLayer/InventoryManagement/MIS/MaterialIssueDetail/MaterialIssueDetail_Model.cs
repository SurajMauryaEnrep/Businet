using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MaterialIssueDetail
{
    public class MaterialIssueDetail_Model
    {
        public string title { get; set; }
        public string action { get; set; }
        public string itemName { get; set; }
        public string requirementArea { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string transferType { get; set; }
        public string destinationBranch { get; set; }
        public string destinationWarehouse { get; set; }
        public string issuedTo { get; set; }
        public string SearchStatus { get; set; }
        public List<DstnWarehouseModel> WarehouseList { get; set; }
        public List<DstnBranchModel> DstnBranchList { get; set; }
        public List<ItemsModel> ItemsList { get; set; }
        public List<RequirementAreaModel> ReqAreaList { get; set; }
    }
    public class DstnWarehouseModel
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class DstnBranchModel
    {
        public string br_id { get; set; }
        public string br_name { get; set; }
    }
    public class ItemsModel
    {
        public string Item_Id { get; set; }
        public string Item_Name { get; set; }
    }
    public class RequirementAreaModel
    {
        public string Item_Id { get; set; }
        public string Item_Name { get; set; }
    }
}
