using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.GoodReceiptNote
{
  public  class GoodReceiptNoteSCModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string WF_Status1 { get; set; }
        public string DocumentMenuId { get; set; }
        public string DocumentStatus { get; set; }
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string SubItemDetailsDt { get; set; }//For sub-item
        public string ConsScrapbySubItemDetailsDt { get; set; }//For sub-item
        public string AppStatus { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public Boolean Cancelled { get; set; }
        public string DeleteCommand { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string Status { get; set; }
        public string doc_status { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string GRNStatus { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string TranstypAttach { get; set; }
        public string ListFilterData1 { get; set; }
        public string DocNoAttach { get; set; }
        public string attatchmentdetail { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        
      
        public string Title { get; set; }
        public string GRNNumber { get; set; }
        public string GRNDate { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public string DeliveryNoteDate { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public string DeliveryNote_Number { get; set; }
        public string FinishProduct { get; set; }
        public string FinishProductId { get; set; }
        public string FinishUom { get; set; }
        public string FinishUomId { get; set; }
        public string Bill_No { get; set; }
        public string Bill_Dt { get; set; }
        public string Remarks { get; set; }
        public string JobOrdTyp { get; set; }
        public string ConvRate { get; set; }
        public string CurrId { get; set; }
        public string GRNSCItemdetails { get; set; }
        public string BatchDetail { get; set; }
        public string SerialDetail { get; set; }
        public List<DeliveryNoteNo> DeliveryNoteNoList { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
       
        
        public string GRNSCConsumeItemdetails { get; set; }
        public string GRNSCScrapItemdetails { get; set; }
        public string ConsumeItemBatchWiseDetail { get; set; }
        public string ScrapItemBatchWiseDetail { get; set; }
        // public string ConsumeBatchDetail { get; set; }
        public string CompId { get; set; }
        public string BrchID { get; set; }
        public string item_id { get; set; }
        public int uom_id { get; set; }
        public string wh_id { get; set; }
        public string GRNSCwhname { get; set; }
        public string wh_id_scrap { get; set; }
        public string rec_qty { get; set; }
        public int reject_wh_id { get; set; }
        public string reject_qty { get; set; }
        public int rework_wh_id { get; set; }
        public string rework_qty { get; set; }
        public string item_rate { get; set; }
        public string GrossValue { get; set; }
        public string TaxAmountNonRecoverable { get; set; }
        public string TaxAmountRecoverable { get; set; }
        public string OtherCharges { get; set; }
        public string OCWithoutTax { get; set; }
        public string NetMRValue { get; set; }
        public string NetLandedValue { get; set; }
        public string ConsumptionValue { get; set; }
        public string CostingDetailItmDt { get; set; }
        public string CostingDetailItmTaxDt { get; set; }
        public string CostingDetailItmOCTaxDt { get; set; }
        public string CostingDetailOcDt { get; set; }
        public string CostingDetailConItmDt { get; set; }
        public string Show { get; set; }
        public string ewb_no { get; set; }

    }

    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_id_scrap { get; set; }
        public string wh_name { get; set; }
    }
    public class DeliveryNoteNo
    {
        public string DNnoId { get; set; }
        public string DNnoVal { get; set; }
    }
    public class GRNDetailsattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        
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
    public class GRNListModel
    {

        public string WF_Status { get; set; }
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

        public string ToDate { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string mr_no { get; set; }
        public string mr_dt { get; set; }
        public string GRNSCSearch { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<GoodsReceiptNoteSCList> GRNSCList { get; set; }
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
    public class GoodsReceiptNoteSCList
    {
        public string WFBarStatus { get; set; }
        public string GRNNo { get; set; }
        public string GRNDate { get; set; }
        public string GRN_Date { get; set; }
        public string OrderType { get; set; }
        public string DeliveryNoteNo { get; set; }
        public string DeliveryNoteDate { get; set; }
        public string SuppName { get; set; }
        public string Currency { get; set; }
        public string Stauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string Create_By { get; set; }
        public string Mod_By { get; set; }
        public string App_By { get; set; }
    }

}
