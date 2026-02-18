using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.InterBranchPurchase
{
    public class InterBranchPurchase_Model
    {
        public string Title { get; set; }
        public string DeleteCommand { get; set; }
        public string Message { get; set; }
        public string DocumentMenuId { get; set; }
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Inv_no { get; set; }
        public string Inv_dt { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string Hd_GstType { get; set; }
        public string Hd_GstCat { get; set; }
        public string AppStatus { get; set; }
        public string WF_Status1 { get; set; }
        public string CreatedBy { get; set; }
        public string Createdon { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string Status_name { get; set; }
        public string doc_status { get; set; }
        public Boolean RCMApplicable { get; set; }
        public Boolean CancelFlag { get; set; }
        public string IBP_SuppID { get; set; }
        public string interbrnchid { get; set; }
        public string supp_acc_id { get; set; }
        public string FilterData1 { get; set; }
        public string Address { get; set; }
        public string bill_add_id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string bill_no { get; set; }
        public string bill_date { get; set; }
        public string EInvoive { get; set; }
        public string EWBNNumber { get; set; }
        public string remarks { get; set; }
        public string ItemDetails { get; set; }
        public string TaxDetail { get; set; }
        public string OC_TaxDetail { get; set; }
        public string OCDetail { get; set; }
        public string GLVoucherType { get; set; }
        public string GLVoucherNo { get; set; }
        public string GLVoucherDt { get; set; }
        public string vouDetail { get; set; }
        public string Narration { get; set; }
        public string GrossValue { get; set; }
        public string GrossValueInBase { get; set; }
        public string TaxAmount { get; set; }
        public string OtherCharges { get; set; }
        public string DocSuppOtherCharges { get; set; }
        public string NetAmount { get; set; }
        public Boolean RoundOffFlag { get; set; }
        public string pmflagval { get; set; }
        public string NetAmountInBase { get; set; }
        public string TDS_Amount { get; set; }
        public string NetLandedValue { get; set; }
        public string IBP_SuppName { get; set; }
        public string SerialItemDeatilData { get; set; }
        public string BatchItemDeatilData { get; set; }
        public string BatchCommand { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string ListFilterData1 { get; set; }
        public string PriceBasis { get; set; }
        public string FreightType { get; set; }
        public string ModeOfTransport { get; set; }
        public string Destination { get; set; }
        public string Nurration { get; set; }
        public string BP_Nurration { get; set; }
        public string DN_Nurration { get; set; }
        public string CN_Nurration { get; set; }
        public string curr_id { get; set; }
        public string bs_curr_id { get; set; }
        public string conv_rate { get; set; }
        public string GstApplicable { get; set; }
        public string SubItemDetails { get; set; }
        public string tds_details { get; set; }
        public string oc_tds_details { get; set; }
        public string CC_DetailList { get; set; }
        public string attatchmentdetail { get; set; }
        public string wh_id { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public List<ibp_Branch> ibp_BranchList { get; set; }
        public List<ibp_BillNo> ibp_BillNoList { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string TblItemOCdetails { get; set; }
        public string tcs_details { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string ShowProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string PrintShipFromAddress { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string CustomerAliasName { get; set; } = "N";
        public string ShowTotalQty { get; set; } = "Y";
        public string PrintFormat { get; set; } = "F";
        public string ShowWithoutSybbol { get; set; } = "Y";
        public string showDeclare1 { get; set; } = "N";
        public string showDeclare2 { get; set; } = "N";
        public string showInvHeading { get; set; } = "N";
        public int NumberofCopy { get; set; } = 1;
        public string _ModelCommand { get; set; }
        //public string WF_status1 { get; set; }
        //public string Itemdetails { get; set; }
        public string ItemTaxdetails { get; set; }
        public string ItemOCdetails { get; set; }
        public string EditCommand { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string StatusName { get; set; }
        public string SSIStatus { get; set; }
        public string ExRate { get; set; }
    }
    public class IBPListModel
    {
        public string Title { get; set; }
        public string SuppID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string IBP_ToDate { get; set; }
        public string IBP_FromDate { get; set; }
        public string Status { get; set; }
        public string ListFilterData { get; set; }
        public string DocumentMenuId { get; set; }
        public string SuppName { get; set; }
        public string wfdocid { get; set; }
        public string wfstatus { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Status> StatusList { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }

    public class ibp_Branch
    {
        public string ibp_brch_id { get; set; }
        public string ibp_brch_name { get; set; }
    }
    
    public class ibp_BillNo
    {
        public string ibp_billno_id { get; set; }
        public string ibp_billno { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class UrlData
    {
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Inv_no { get; set; }
        public string Inv_dt { get; set; }
        public string ListFilterData1 { get; set; }
    }
    public class DirectPurchaseInvoiceattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
}
