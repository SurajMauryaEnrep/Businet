using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.QualityInspection
{
    public class QualityInspectionModel
    {
       
        public string SupplierName { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string DocumentNo { get; set; }

        public string MenuDocumentId { get; set; }
        public string qc_no { get; set; }
        public DateTime qc_dt { get; set; }
        public string qc_type { get; set; }
        public string src_doc_no { get; set; }
        public DateTime? src_doc_date { get; set; }
        public string batch_no { get; set; }
        public string qc_remarks { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string attatchmentdetail { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public int create_id { get; set; }
        public DateTime create_dt { get; set; }
        public int app_id { get; set; }

        public DateTime app_dt { get; set; }
        public int mod_id { get; set; }
        public DateTime mod_dt { get; set; }
        public string qc_status { get; set; }
        public string qc_statuscode { get; set; }
        public string mac_id { get; set; }
        public string item_id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string Src_type { get; set; }
        public List<DocumentNumber> DocumentNumberList { get; set; }

        public List<ItemDetails> ItemDetailsList { get; set; }
        public List<ItemDetailsQc> ItemDetailsQCList { get; set; }

        public string QCItemdetails { get; set; }
        public string QCItemParamdetails { get; set; }
        public string QCLotBatchdetails { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public Boolean CancelFlag { get; set; }
        public string pending_qty { get; set; }
        public string DeleteCommand { get; set; }
        public int SourceWH { get; set; }
        public int AcceptedWH { get; set; }
        public int RejectWH { get; set; }
        public int ReworkWH { get; set; }
        /*Code Start Add By Hina on 17-08-2024 for Shopfloor in case of Random QC*/
        public string Location_type { get; set; }
        public int SourceSF { get; set; }
        public List<SourceShopfloor> SourceShopfloorList { get; set; }
        /*Code End Add By Hina on 17-08-2024 for Shopfloor in case of Random QC*/
        public string ListFilterData1 { get; set; }
     
        public List<SourceWarehouse> SourceWarehouseList { get; set; }
        public List<AcceptedWarehouse> AcceptedWarehouseList { get; set; }
        public List<RejectWarehouse> RejectWarehouseList { get; set; }
        public List<ReworkWarehouse> ReworkWarehouseList { get; set; }
        public List<QCTypeList> _QCTypeList { get; set; }
        public string DocumentMenuId { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string HdnCommand { get; set; }
        public string WF_status1 { get; set; }
        public string Message { get; set; }
        public string StockItemWiseMessage { get; set; }
        public string AppStatus { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string itemId { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class SourceWarehouse
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }
    public class SourceShopfloor
    {
        public int shfl_id { get; set; }
        public string shfl_val { get; set; }
    }
    public class QCTypeList
    {
        public string QCType_id { get; set; }
        public string QCType_val { get; set; }
    }
    public class AcceptedWarehouse
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }
    public class RejectWarehouse
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }
    public class ReworkWarehouse
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }
    public class DocumentNumber
    {
        public string doc_no { get; set; }
        public string doc_dt { get; set; }
        public string supp_name { get; set; }
        public string supp_id { get; set; }
    }

    public class ItemDetails
    {
        public string item_id { get; set; }
        public int uom_id { get; set; }
        public float recd_qty { get; set; }
        public float accept_qty { get; set; }
        public float reject_qty { get; set; }
        public float rework_qty { get; set; }
        public string it_remarks { get; set; }
    }
    public class ItemDetailsQc
    {
        public string item_id { get; set; }
        public int uom_id { get; set; }
        public string sam_size { get; set; }
        public int param_Id { get; set; }
        public float upper_val { get; set; }
        public float lower_val { get; set; }
        public float param_result { get; set; }
        public string param_action { get; set; }
    }
    public class ItemName_List
    {
        public string Item_ID { get; set; }
        public string Item_Name { get; set; }
    }
    public class QCInspectionList_Model
    {
        public List<ItemName_List> ItemNameList { get; set; }
        public string ddl_ItemName { get; set; }
        public string Title { get; set; }
        public string QC_FromDate { get; set; }
        public string QC_ToDate { get; set; }
        public string QC_Type { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string MenuDocumentId { get; set; }
        public string ListFilterData { get; set; }
        public DateTime ToDate { get; set; }
        public List<Status> StatusList { get; set; }
        public List<QCInspectionList> QCList { get; set; }
        public string QISearch { get; set; }
        public string WF_status { get; set; }
        public string DocumentMenuId { get; set; }
    }
    public class URLDetailModel
    {
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string TransType { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string DocumentMenuId { get; set; }
        public string QC_Type { get; set; }
        public string WF_status1 { get; set; }
    }
    public class QualityInspectionModelAttch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class QCInspectionList
    {
        public string QCNo { get; set; }
        public string QCDate { get; set; }
        public string QC_date { get; set; }
        public string SourceType { get; set; }     
        public string QCType { get; set; }
        public string Location { get; set; }
        public string SourceDocNo { get; set; }
        public string SourceDocDate { get; set; }
        public string Stauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
    }

}

