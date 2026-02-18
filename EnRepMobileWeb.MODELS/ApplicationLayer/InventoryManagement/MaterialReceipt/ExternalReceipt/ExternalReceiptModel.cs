using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.ExternalReceipt
{
    public class ExternalReceiptModel
    {
        public List<EntityNameList1> EntityNameList1 { get; set; }
        public string EntityID { get; set; }
        public string EntityName { get; set; }
        public string entity_type { get; set; }
        public string EntityType { get; set; }
        public string Search { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public string ListFilterData { get; set; }
        public string Title { get; set; }
        public string WF_Status { get; set; }

        public List<StatusList> Status_list { get; set; }
    }
    public class StatusList
    {
        public string StatusID { get; set; }
        public string Status_Name { get; set; }
    }
    public class UrlModel
    {
        public string Msg { get; set; }
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string recp_no { get; set; }
        public string recpt_dt { get; set; }
        public string DMS { get; set; }

    }
    public class ExternalReceiptDeatilModel
    {
        public string ImportExcelFileFlag { get; set; }
        public string Qty_pari_Command { get; set; }
        public string hdnSaveDataBatchSerialLotDeatil { get; set; }
        public string DocumentDate { get; set; }
        public string SrcDocNo { get; set; }
        public List<SrcDocNoList> docNoLists { get; set; }
        public string SourceType { get; set; }
        public string CancelledRemarks { get; set; }
        public string A_Remarks { get; set; }
        public string A_Level { get; set; }
        public string A_Status { get; set; }
        public string BatchCommand { get; set; }
        public string WFBarStatus { get; set; }
        public string doc_status { get; set; }
        public string StatusCode { get; set; }
        public string entity_type { get; set; }
        public string wh_id { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string Title { get; set; }
        public string DocumentStatus { get; set; }
        public string DeleteCommand { get; set; }
        public string Create_id { get; set; }
        public string Status_Code { get; set; }
        public string StatusName { get; set; }
        public string Amended_on { get; set; }
        public string Amended_by { get; set; }
        public string Approved_on { get; set; }
        public string Approved_by { get; set; }
        public string Created_on { get; set; }
        public string Created_by { get; set; }
        public string ReceivedFrom { get; set; }
        public string CheckedBy { get; set; }
        public string remarks { get; set; }
        public string ReceiptDate { get; set; }
        public string ReceiptNumber { get; set; }
        public string Massage { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public string ListFilterData1 { get; set; }
        public string WF_Status1 { get; set; }
        public string WFStatus { get; set; }
        public string EntityTypeID { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public string EntityID { get; set; }
        public string Guid { get; set; }
        public string ItemDeatilData { get; set; }
        public string SerialItemDeatilData { get; set; }
        public string BatchItemDeatilData { get; set; }
        public Boolean CancelFlag { get; set; }
        public string attatchmentdetail { get; set; }
        public List<EntityNameList> EntityNameList { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
    }
    public class SrcDocNoList
    {
        public string SrcDocnoId { get; set; }
        public string SrcDocnoVal { get; set; }
    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class EntityNameList1
    {
        public string entity_id { get; set; }
        public string entity_name { get; set; }
    }
    public class EntityNameList
    {
        public string entity_id { get; set; }
        public string entity_name { get; set; }
    }
    public class GatePassattchment
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
       
    }
}
