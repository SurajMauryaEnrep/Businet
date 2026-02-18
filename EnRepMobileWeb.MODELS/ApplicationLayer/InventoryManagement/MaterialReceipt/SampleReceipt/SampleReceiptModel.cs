using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.SampleReceipt
{
    public class SampleReceiptModel
    {
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string src_type { get; set; }
        public string uom { get; set; }
        public string Sample_id { get; set; }
        public string ST_Item { get; set; }
        public string SrcDocNo { get; set; }
        public string SrcDocDate { get; set; }
        public List<listitem> SelectList { get; set; }
        public List<uomitemlist> uomlist { get; set; }
        public List<SrcDocNoList> docNoLists { get; set; }
        public string sample_name { get; set; }
        public List<SampleName> SampleNamesList { get; set; }
        public string source_type { get; set; }
        public string attatchmentdetail { get; set; }
        public string Title { get; set; }
        public string SR_Item { get; set; }
        public string srcpt_id { get; set; }
        public string MenuDocumentId { get; set; }
        public string entity_type { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string srcpt_status { get; set; }
        public string srcpt_no { get; set; }
        public string ListFilterData1 { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string srcpt_dt { get; set; }
        public string srcpt_rem { get; set; }
        public string EntityID { get; set; }
        public string EntityType { get; set; }
        public string entity_id { get; set; }
        public string entity_name { get; set; }
        public string EntityName { get; set; }
        public List<EntityNameList> EntityNameList { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public Boolean CancelFlag { get; set; }
        //public string CancelFlag { get; set; }
        //public List<EntityType> EntityTypeList { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public string DeleteCommand { get; set; }
        public string SampleRcptItemDetail { get; set; }
        public string SR_ItemDetail { get; set; }
        public string SR_Val { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string WF_status1 { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string DocumentStatus { get; set; }
        public string AppStatus { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
    }
    public class SampleReceiptModelAttch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class URLModelDetails
    {
        public string TransType { get; set; }
        public string Command { get; set; }
        public string srcpt_no { get; set; }
        public string BtnName { get; set; }
        public string srcpt_dt { get; set; }
    }
    public class EntityNameList
    {
        public string entity_id { get; set; }
        public string entity_name { get; set; }
    }   
    public class listitem
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
    }
    public class uomitemlist
    {
        public string uomid { get; set; }
        public string uom_name { get; set; }
    }
    public class EntityType
    {
        public string EntityTypeID { get; set; }
        public string EntityTypeName { get; set; }
    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class SrcDocNoList
    {
        public string SrcDocnoId { get; set; }
        public string SrcDocnoVal { get; set; }
    }
    public class SampleName
    {
        public string sample_id { get; set; }
        public string sample_name { get; set; }
    }
    public class ItemDetails
    {
        public string ItemID { get; set; }
        public string UOM { get; set; }
        public string ItemCost { get; set; }
        public string RecievedQty { get; set; }
        public string Warehouse { get; set; }
        public string LotNo { get; set; }
        public string Remarks { get; set; }
    }

    public class SampleReceiptHeaderDetail
    {

        public string TransType { get; set; }
        public string MenuID { get; set; }
        public string Cancelled { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public string sr_no { get; set; }
        public string sr_dt { get; set; }
        public string entity_name { get; set; }
        public int entity_id { get; set; }
        public int user_id { get; set; }
        public string sr_status { get; set; }
        public string mac_id { get; set; }
        public string sr_val { get; set; }
        public string remarks { get; set; }

    }
    public class SampleReceiptItemDetail
    {
        public string item_id { get; set; }
        public int uom_id { get; set; }
        public string rec_qty { get; set; }
        public string lot_id { get; set; }
        public int wh_id { get; set; }
        public string item_rate { get; set; }
        public string it_remarks { get; set; }
    }
    public class SampleReceiptItemBatchDetail
    {
        public string item_id { get; set; }
        public string batch_no { get; set; }
        public string batch_qty { get; set; }
        public string exp_dt { get; set; }
    }
    public class SampleReceiptItemSerialDetail
    {
        public string item_id { get; set; }
        public string serial_no { get; set; }
    }

    public class SampleReceipt_ListModel
    {
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string sr_no { get; set; }
        public string sr_dt { get; set; }
        public string wfdocid { get; set; }
        public string wfstatus { get; set; }
        public string doc_status { get; set; }
        public string Title { get; set; }
        public string EntityType { get; set; }
        public string ListFilterData { get; set; }
        public string EntityName { get; set; }
        public string EntityID { get; set; }
        public string SR_FromDate { get; set; }
        public string SR_ToDate { get; set; }
        public string OrderType { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<EntityNameList> EntityNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<SampleReceiptList> SR_List { get; set; }
        public string WF_status { get; set; }
        public string SRSearch { get; set; }
        public string DocumentStatus { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class SampleReceiptList
    {
        public int sr_no { get; set; }
        public string src_dt { get; set; }
        public string src_no { get; set; }
        public string SRNo { get; set; }
        public string SRDate { get; set; }
        public string SR_DT { get; set; }
        public string OrderType { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public string Stauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
    }
}
