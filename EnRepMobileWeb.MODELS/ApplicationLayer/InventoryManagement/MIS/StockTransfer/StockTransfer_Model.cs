using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockTransfer
{
    public class StockTransfer_Model
    {
        public string ItemId { get; set; }
        public string TransferType { get; set; }
        public string SrcBranch { get; set; }
        public string DstnBranch { get; set; }
        public string SrcWarehouse { get; set; }
        public string DstnWarehouse { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<ItemsModel> ItemsList { get; set; }
        public List<STBranchModel> SrcBranchList { get; set; }
        public List<STBranchModel> DstnBranchList { get; set; }
        public List<STWareHouseModel> SrcWarehouseList { get; set; }
        public List<STWareHouseModel> DstnWarehouseList { get; set; }
    }
    public class ItemsModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
    }
    public class STBranchModel
    {
        public string BrId { get; set; }
        public string BrName { get; set; }
    }
    public class STWareHouseModel
    {
        public string WhId { get; set; }
        public string WhName { get; set; }
    }
}
