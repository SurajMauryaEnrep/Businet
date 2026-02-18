using System;
using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesReturn
{
    public class SalesReturn_Model
    {
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string DocumentNo { get; set; }
        public string CustomerID { get; set; }
        public string sr_id { get; set; }
        public string MenuDocumentId { get; set; }
        public string CreatedBy { get; set; }
        public string create_id { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string Status { get; set; }
        public string srt_status { get; set; }
        public string srt_no { get; set; }
        public DateTime srt_dt { get; set; }
        public string inv_value { get; set; }
        public string OcAmt { get; set; }
        public string srt_value { get; set; }
        public string cust_id { get; set; }
        public string CustomerName { get; set; }
        public string src_doc_date { get; set; }
        public string src_doc_no { get; set; }
        public string DocumentId { get; set; }
        public Boolean CancelFlag { get; set; }
        public string WFBarStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string WFStatus { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public List<DocumentNumber> DocumentNumberList { get; set; }
        public List<ItemDetails> ItemDetailsList { get; set; }
        public string SRItemdetails { get; set; }
        public string SRItemBatchSerialDetail { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string ship_dt { get; set; }
        public string ship_no { get; set; }
        public string CreateBy { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string DeleteCommand { get; set; }

        public string VouType { get; set; }
        public string VouNo { get; set; }
        public string VouDt { get; set; }
        public string SvNarr { get; set; }
        public string CnNarr { get; set; }
        public string Voudetails { get; set; }
        public string CustomVouGlDetails { get; set; }
        public string ListFilterData1 { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string SRSearch { get; set; }
        public string AppStatus { get; set; }
        public string SalesReturnNo { get; set; }
        public string SalesReturnDate { get; set; }
        public string WF_status1 { get; set; }
        public string DocumentMenuId { get; set; }
        public string cust_acc_id { get; set; }
        public string curr { get; set; }
        public string curr_id { get; set; }
        public string conv_rate { get; set; }
        public string CC_DetailList { get; set; }
        public string TaxDetail { get; set; }
        public string ItemOCdetails { get; set; }
        public string ItemOCTaxdetails { get; set; }
        public string Src_Type { get; set; }
        public string BatchItemDeatilData { get; set; }
        public string SerialItemDeatilData { get; set; }
        public string BatchCommand { get; set; }
        public string Ship_StateCode { get; set; }
        public string IRNNumber { get; set; }//Added by Suraj Maurya on 11-04-2025
        public string InvBillNumber { get; set; }//Added by Shubham Maurya on 02-05-2025
        public string InvBillDate { get; set; }//Added by Shubham Maurya on 02-05-2025
        public Boolean RoundOffFlag { get; set; }
        public string pmflagval { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        // Code start Added by Hina sharma on 15-05-2025
        public string Customer_Reference { get; set; }
        public string Payment_term { get; set; }
        public string Delivery_term { get; set; }
        public string Invoice_Heading { get; set; }
        public string PlaceOfSupply { get; set; }
        public string SlsRemarks { get; set; }
        // Code End Added by Hina sharma on 15-05-2025

        /*------------------------Transport detail start--------------------*/
        public string GR_No { get; set; }
        public string HdnGRNumber { get; set; }
        public string GR_Dt { get; set; }
        public string HdnGRDate { get; set; }

        public string No_Of_Packages { get; set; }
        public string hdnNumberOfPacks { get; set; }
        //public string Transpt_Id { get; set; }
        public string Transpt_NameID { get; set; }
        public string HdnTrnasportName { get; set; }

        public string Veh_Number { get; set; }

        public string Driver_Name { get; set; }
        public string Mob_No { get; set; }
        public string Tot_Tonnage { get; set; }
        public string TransId { get; set; }
        public List<TransListModel> TransList { get; set; }
        /*----------------------Print Detail Start-------------------------------*/
        public string ShowProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string CustomerAliasName { get; set; } = "N";
        public string ShowTotalQty { get; set; } = "Y";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string PrintFormat { get; set; } = "F1"; 
        public string HdnPrintOptons { get; set; }
        public string GstApplicable { get; set; }
        public string ShowWithoutSybbol { get; set; } = "Y";
        public string showInvHeading { get; set; } = "N";
        public string PrintRemarks { get; set; } = "N";
        public string JVNurr { get; set; }
        public string GLVoucherType { get; set; }
        public string GLVoucherNo { get; set; }
        public string GLVoucherDt { get; set; }
    }
    public class TransListModel
    {
        public string TransId { get; set; }
        public string TransName { get; set; }
    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class UrlModel
    {
        public string BtnName { get; set; }
        public string cmd { get; set; }
        public string InvType { get; set; }
        public string SRN { get; set; }
        public string SRD { get; set; }
        public string Trp { get; set; }
        public string WFS1 { get; set; }
        public string APS { get; set; }
        public string Docid { get; set; }
        public string DMS { get; set; }
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
    public class CustomerName
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
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
}
