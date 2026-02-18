using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.PurchaseReturn
{
   public class PurchaseReturn_Model
    {
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string DocumentNo { get; set; }
        public string SupplierID { get; set; }
        public string pr_id { get; set; }
        
        public string MenuDocumentId { get; set; }       
        public string CreatedBy { get; set; }
        public string create_id { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string Status { get; set; }
        public string prt_status { get; set; }
        public string prt_no { get; set; }
        public DateTime prt_dt { get; set; }      
        public string inv_value { get; set; }
        public string OcAmt { get; set; }
        public string prt_value { get; set; }   
        public string supp_id { get; set; }
        public string SupplierName { get; set; }
        public DateTime? src_doc_date { get; set; }
        public string src_doc_no { get; set; }
        public string DocumentId { get; set; }
        public Boolean CancelFlag { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string WF_status1 { get; set; }
        public string docid { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }

        public List<DocumentNumber> DocumentNumberList { get; set; }

        public List<ItemDetails> ItemDetailsList { get; set; }

        public string PRItemdetails { get; set; }
        public string PRVoudetails { get; set; }
        public string PRItemBatchSerialDetail { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }     
        public string mr_dt { get; set; }
        public string mr_no { get; set; }
        public string CreateBy { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string DeleteCommand { get; set; }

        public string VouType { get; set; }
        public string VouNo { get; set; }
        public string VouDt { get; set; }
        public string PvNarr { get; set; }
        public string DnNarr { get; set; }
        public string ListFilterData1 { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string StockItemWiseMessage { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string AppStatus { get; set; }
        public string PurchaseReturnNo { get; set; }
        public string PurchaseReturnDate { get; set; }
        public string SubItemDetailsDt { get; set; }//For sub-item
        public string CC_DetailList { get; set; }
        public string bill_no { get; set; }
        public string bill_dt { get; set; }
        public string curr_id { get; set; }
        public string conv_rate { get; set; }
        public string supp_acc_id { get; set; }
        public string Ship_StateCode { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Hd_GstType { get; set; }
        public string Hd_GstCat { get; set; }
        public string Src_Type { get; set; }
        public string AdHocBill_no { get; set; }
        public string AdHocBill_dt { get; set; }
        public string TaxDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public Boolean RoundOffFlag { get; set; }
        public string pmflagval { get; set; }
        public string ItemOCdetails { get; set; }
        public string ItemOCTaxdetails { get; set; }
        public List<Warehouse> WarehouseList { get; set; }

    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }

    public class UrlModel
    {
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string PurchaseReturnNo { get; set; }
        public string PurchaseReturnDate { get; set; }
        public string TransType { get; set; }
        public string WF_status1 { get; set; }
        public string AppStatus { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }

    public class DocumentNumber
    {
        public string inv_no { get; set; }
        public string inv_dt { get; set; }

    }

    public class ItemDetails
    {
        public string item_id { get; set; }
        public string ord_qty { get; set; }
        public string base_uom_id { get; set; }
        public string ord_qty_base { get; set; }
        public string SourceDocumentNo { get; set; }
        public string SourceDocumentDate { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public string OrderQty { get; set; }
        public string BilledQty { get; set; }
        public string RecievedQty { get; set; }       
        public string AcceptedQty { get; set; }
        public string RejectedQty { get; set; }
        public string ReworkableQty { get; set; }    
        public string it_remarks { get; set; }
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
        public string gl_type { get; set; }

    }
}
