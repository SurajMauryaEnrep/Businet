using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.SampleTracking
{
    public class SampleTracking_Model
    {
        public string SummaryDetail { get; set; }
        public string Issuedby { get; set; }
        public string SampleWisedata { get; set; }
        public string entity_type { get; set; }
        public string Title { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ST_Item { get; set; }
        public string Entity { get; set; }
        public string EntityName { get; set; }
        public string SampleTrackingS { get; set; }
        public List<EntityNameList> EntityNameList { get; set; }
        public List<ItemList> itemLists { get; set; }
        public List<SampleTrackingList> SampleTrackingLists { get; set; }
        public List<SampleTrackingList_sampleWise> SampleTrackingLists_SampleWise { get; set; }
        public List<SampleTrackingList_BinWise> SampleTrackingLists_BinWise { get; set; }
        public List<Issuedby> IssuedbyList { get; set; }
    }
    public class Issuedby
    {
        public string Issuedby_id { get; set; }
        public string Issuedby_Name { get; set; }
    }
    public class EntityNameList
    {
        public string entity_id { get; set; }
        public string entity_name { get; set; }
    }
    public class SampleTrackingList
    {
        //public string ST_FromDate { get; set; }
        public int SrNo { get; set; }
        public string Remarks_Summ { get; set; }
        public string Issued_by_Summ { get; set; }
        public string Bin_loc_Summ { get; set; }
        public string issued_dt_Summ { get; set; }
        public string Issue_Number_Summ { get; set; }
        public string elsp_days { get; set; }
        public string issue_date { get; set; }
        public string receive_date { get; set; }
        public string other_dtl { get; set; }
        public string sr_type { get; set; }
        public string _Entity { get; set; }
        public string _EntityID { get; set; }
        public string _EntityType { get; set; }
        public string _EntityTypeCode { get; set; }
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        public string ItemID { get; set; }
        public string UOM { get; set; }
        public string uom_id { get; set; }
        //public string Cost_price { get; set; }
        public string Opening_balence { get; set; }
        public string Issued { get; set; }
        public string Receipts { get; set; }
        public string Balance { get; set; }
        //public string Value { get; set; }

    }
    public class SampleTrackingList_sampleWise
    {
        public int SrNo_Tracking { get; set; }
        public string doc_no_Tracking { get; set; }
        public string sr_dt_Tracking { get; set; }
        public string entity_type_Tracking { get; set; }
        public string bin_loc_Tracking { get; set; }
        public string sr_type_Tracking { get; set; }
        public string issued_by_Tracking { get; set; }
        public string entityName_Tracking { get; set; }
        public string issued_date_Tracking { get; set; }
        public string receive_date_Tracking { get; set; }
        public string other_dtl_Tracking { get; set; }
        public string issued_qty_Tracking { get; set; }
        public string qty_Tracking { get; set; }
        public string elsp_days_Tracking { get; set; }
        public string remarks_Tracking { get; set; }
    }
    public class SampleTrackingList_BinWise
    {
        public int SrNo_Bin { get; set; }
        public string ItemName_Bin { get; set; }
        public string ItemId_Bin { get; set; }
        public string Uom_Bin { get; set; }
        public string UomId_Bin { get; set; }
        public string doc_no_Bin { get; set; }
        public string sr_dt_Bin { get; set; }
        public string entity_type_Bin { get; set; }
        public string entity_id_Bin { get; set; }
        public string bin_loc_Bin { get; set; }
        public string sr_type_Bin { get; set; }
        public string issued_by_Bin { get; set; }
        public string entityName_Bin { get; set; }
        public string issued_date_Bin { get; set; }
        public string receive_date_Bin { get; set; }
        public string other_dtl_Bin { get; set; }
        public string issued_qty_Bin { get; set; }
        public string qty_Bin { get; set; }
        public string elsp_days_Bin { get; set; }
        public string remarks_Bin { get; set; }
    }
    public class ItemList
    {
        public string Item_id { get; set; }
        public string Item_name { get; set; }
    }
}
