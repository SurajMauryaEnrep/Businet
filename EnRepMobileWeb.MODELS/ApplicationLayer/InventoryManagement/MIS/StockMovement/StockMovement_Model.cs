using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockMovement
{
    public class StockMovement_Model
    {
       public string Title { get; set; }
        public string ItemDescription { get; set; }
        public string UOM { get; set; }
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string MovementBy { get; set; }
        public List<ItemName> itemNameList { get; set; }
        public List<Batch> batcheList { get; set; }
        public List<Serial> SerialList { get; set; }
        public List<MovementDetailsList> MovDetailsLists { get; set; }
    }

    public class ItemName
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Batch
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Serial
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class MovementDetailsList
    {
        public string itemname { get; set; }
        public string uom { get; set; }
        public string branch { get; set; }
        public string warehouse { get; set; }
        public string lot { get; set; }
        public string batch { get; set; }
        public string serial_no { get; set; }
        public string transdate { get; set; }
        public string sourcetype { get; set; }
        public string docno { get; set; }
        public string src_doc_code { get; set; }
        public string billno { get; set; }
        public string billdt { get; set; }
        public string openingstock { get; set; }
        public string inwardqty { get; set; }
        public string outwardqty { get; set; }
        public string closingstock { get; set; }
        public string costprice { get; set; }
        public string closingvalue { get; set; }
        public string mfg_name { get; set; } = "";
        public string mfg_mrp { get; set; } = "";
        public string mfg_date { get; set; } = "";
    }
}
