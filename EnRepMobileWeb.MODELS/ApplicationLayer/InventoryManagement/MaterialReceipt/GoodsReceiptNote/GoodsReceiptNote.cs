using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.GoodsReceiptNote
{
    public class GoodsReceiptNoteModel
    {
        public string ImportExcelFileFlag { get; set; }
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string EWBNNumber { get; set; }
        public string EInvoive { get; set; }
        public string veh_load { get; set; }
        public string VehicleNumber { get; set; }
        public string TransporterName { get; set; }
        public string FreightAmount { get; set; }
        public string GRNumber { get; set; }
        public DateTime? GRDate { get; set; }
        public string Title { get; set; }
        public string SuppName { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string attatchmentdetail { get; set; }
        public string grn_no { get; set; }
        public string DeleteCommand { get; set; }
        public string grn_dt { get; set; }
        public string ListFilterData1 { get; set; }
        public string supp_id { get; set; }
        public bool CancelFlag { get; set; }
        public string DeliveryNoteDate { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public string bill_no { get; set; }
        public string bill_dt { get; set; }
        public string GRNItemdetails { get; set; }
        public string CreatedBy { get; set; }
        public string Createdon { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string Status { get; set; }
        public string doc_status { get; set; }
        public string BatchDetail { get; set; }
        public string SerialDetail { get; set; }
        public string wh_id { get; set; }
        public string GRNwhname { get; set; }
        public string GrossValue { get; set; }
        public string GrossValueInBase { get; set; }
        public string TaxAmountNonRecoverable { get; set; }
        public string TaxAmountRecoverable { get; set; }
        public string OtherCharges { get; set; }
        public string NetMRValue { get; set; }
        public string NetLandedValue { get; set; }
        public List<DocumentNumber> DeliveryNoteDateList { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public string VouType { get; set; }
        public string VouNo { get; set; }
        public string VouDt { get; set; }
        public string Narr { get; set; }
        public string Voudetails { get; set; }
        public string WF_status1 { get; set; }
        public string duplicateBillNo { get; set; } = "N";
        public string Command { get; set; }
        public string Message { get; set; }
        public string StockItemWiseMessage { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string AppStatus { get; set; }
        public string TransType { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public DataSet DtSet { get; set; }
        public string Guid { get; set; }
        public string SubItemDetailsDt { get; set; }//For sub-item
        public string CostingDetailItmDt { get; set; }
        public string CostingDetailItmTaxDt { get; set; }
        public string CostingDetailItmOCTaxDt { get; set; }
        public string CostingDetailOcDt { get; set; }
        public string ConvRate { get; set; }
        public string CurrId { get; set; }
        public string BsCurrId { get; set; }
        public string CurrName { get; set; }
        public string OrderType { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string ExchDigit { get; set; }
    }
    public class DocumentNumber
    {
        public string dn_no { get; set; }
        public string dn_dt { get; set; }

    }
    public class URLModelDetails
    {
        public string TransType { get; set; }
        public string Command { get; set; }
        public string grn_no { get; set; }
        public string BtnName { get; set; }
        public string grn_dt { get; set; }
    }
    public class GoodsReceiptNoteModelAttch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class GoodsReceiptNoteHeaderDetail
    {
       
        public string TransType { get; set; }
        public string MenuID { get; set; }
        public string Cancelled { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public string mr_type { get; set; }
        public string mr_no { get; set; }

        public string mr_dt { get; set; }
        public string doc_no { get; set; }
        public string doc_dt { get; set; }
        public int supp_id { get; set; }
        public int user_id { get; set; }
        public string mr_status { get; set; }
        public string mac_id { get; set; }
        public string gr_val { get; set; }
        public string tax_amt_nrecov { get; set; }
        public string tax_amt_recov { get; set; }
        public string oc_amt { get; set; }
        public string net_val { get; set; }
        public string landed_val { get; set; }


    }
    public class GoodsReceiptNoteItemDetail
    {
        public string item_id { get; set; }
        public int uom_id { get; set; }
        public string rec_qty { get; set; }
        public int wh_id { get; set; }       
        public int reject_wh_id { get; set; }
        public string reject_qty { get; set; }
        public int rework_wh_id { get; set; }
        public string rework_qty { get; set; }
        public string item_rate { get; set; }
        public string item_gross_val { get; set; }
        public string item_tax_amt_recov { get; set; }
        public string item_tax_amt_nrecov { get; set; }
        public string item_oc_amt { get; set; }
        public string item_net_val { get; set; }
        public string item_landed_rate { get; set; }
        public string item_landed_val { get; set; }
        public string it_remarks { get; set; }
    }
    public class GoodsReceiptNoteItemBatchDetail
    {
        public string item_id { get; set; }
        public string batch_no { get; set; }
        public string batch_qty { get; set; }
        public string reject_batch_qty { get; set; }
        public string rework_batch_qty { get; set; }
        public string exp_dt { get; set; }
    }
    public class GoodsReceiptNoteItemSerialDetail
    {
        public string item_id { get; set; }
        public string serial_no { get; set; }
        public string QtyType { get; set; }
    }


    public class GRN_ListModel
    {

        public string SupplierID { get; set; }
        public string wfdocid { get; set; }
        public string wfstatus { get; set; }
        public string doc_status { get; set; }
        public string Title { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public string ListFilterData { get; set; }
        public string GRN_FromDate { get; set; }

        public string GRN_ToDate { get; set; }
        public string DocumentMenuId { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }

        public DateTime ToDate { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string mr_no { get; set; }
        public string mr_dt { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<GoodsReceiptNoteList> GRNList { get; set; }
        public string GRNSearch { get; set; }
        public string WF_status { get; set; }
        public string DocumentStatus { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class GoodsReceiptNoteList
    {
        public string WFBarStatus { get; set; }
        public string GRNNo { get; set; }
        public string GRNDate { get; set; }
        public string MrDate { get; set; }
        public string OrderType { get; set; }
        public string DeliveryNoteNo { get; set; }
        public string DeliveryNoteDate { get; set; }
        public string SuppName { get; set; }
        public string Currency { get; set; }
        public string Stauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string mod_by { get; set; }
        public string app_by { get; set; }
        public string bill_no { get; set; }
        public string bill_dt { get; set; }
    }
    public class GL_Detail
    {
        public string comp_id { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string doctype { get; set; }
        public float Value { get; set; }
        public string DrAmt { get; set; }
        public string CrAmt { get; set; }
        public string TransType { get; set; }

    }
}
