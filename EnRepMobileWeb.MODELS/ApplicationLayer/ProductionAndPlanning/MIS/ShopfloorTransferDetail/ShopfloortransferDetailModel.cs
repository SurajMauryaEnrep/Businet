using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MIS.ShopfloorTransferDetail
{
    public class ShopfloortransferDetailModel
    {
        public string Title { get; set; }
        public string TransactionType { get; set; }
        public string MaterialType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ItemId { get; set; }
        public string ItemGroupId { get; set; }
        public string shflId { get; set; }
        public string Status { get; set; }
        public string STFilter { get; set; }
        public List<ItemListModel> ItemNameList { get; set; }
        public List<ItemGroupModel> ItemGroupNameList { get; set; }
        public List<ShopFloor> ShopFloorList { get; set; }
        public List<StatuslistModel> StatusList { get; set; }
    }
    
    public class ItemListModel
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
    }
    public class ItemGroupModel
    {
        public string item_grp_id { get; set; }
        public string GroupName { get; set; }
    }
    public class ShopFloor
    {
        public string shfl_id { get; set; }
        public string shfl_name { get; set; }
    }
    public class StatuslistModel
    {
        public string status_code { get; set; }
        public string status_name { get; set; }
    }
}
