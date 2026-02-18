using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ConsumableInvoice
{
    public class ConsumableInvoiceDetails_Model {
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string remarks { get; set; }
        public string EWBNNumber { get; set; }
        public string EInvoive { get; set; }
        public string SrcDocDate { get; set; }
        public string SrcDocNo { get; set; }
        public string Src_Type { get; set; }
        public string Title { get; set; }
        public string SuppID { get; set; }
        public string supp_acc_id { get; set; }//Added By Suraj on 04-05-2024
        public string Address { get; set; }
        public string bill_add_id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string inv_no { get; set; }
        public string inv_dt { get; set; }
        public string bill_no { get; set; }
        public string bill_date { get; set; }
        public string CreatedBy { get; set; }
        public string Createdon { get; set; }
        public string Create_id { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string Status_name { get; set; }
        public string doc_status { get; set; }
        public bool CancelFlag { get; set; }
        public string GrVal { get; set; }
        public string DiscAmt { get; set; }
        public string TaxAmt { get; set; }
        public string OcAmt { get; set; }
        public string DocSuppOtherCharges { get; set; }//Other charge for Document Supplier.
        public string NetValBs { get; set; }
        public string NetValSpec { get; set; }
        public string TDS_Amount { get; set; }
        public string tds_details { get; set; }
        public string oc_tds_details { get; set; }
        public string var_qty_dtl { get; set; }
        public string var_qty_tax_dtl { get; set; }
        public string NetAmount { get; set; }
        public string GLVoucherType { get; set; }
        public string GLVoucherNo { get; set; }
        public string GLVoucherDt { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string conv_rate { get; set; }
        public string curr_id { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string AppStatus { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string DeleteCommand { get; set; }
        public string CI_No { get; set; }
        public string attatchmentdetail { get; set; }
        public bool Cancelled { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string Itemdetails { get; set; }
        public string ItemTaxdetails { get; set; }
        public string ItemOCdetails { get; set; }
        public string vouDetail { get; set; }
        public string OC_TaxDetail { get; set; }
        public string FilterData1 { get; set; }
        public List<Supplier> SupplierNameList { get; set; }
        public List<SrcDocNoList> docNoLists { get; set; }
        public string WF_status1 { get; set; }
        public string duplicateBillNo { get; set; } = "N";
        public string CC_DetailList { get; set; }
        public string Hd_GstType { get; set; }
        public string Hd_GstCat { get; set; }
        public Boolean RoundOffFlag { get; set; }
        public Boolean pmflag { get; set; }
        public string pmflagval { get; set; }
        public string Nurration { get; set; }//Added By Suraj on 04-05-2024
        public string BP_Nurration { get; set; }//Added By Suraj on 04-05-2024
        public string DN_Nurration { get; set; }//Added By Suraj on 04-05-2024
        public string hdnbilldt { get; set; }//Added By Suraj on 04-05-2024
        public string hdnbillno { get; set; }//Added By Suraj on 04-05-2024
        public Boolean RCMApplicable { get; set; }
        public string UserID { get; set; }
    }
    public class SrcDocNoList
    {
        public string SrcDocnoId { get; set; }
        public string SrcDocnoVal { get; set; }
    }
    public class ConsumableInvoiceattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    public class ConsumableInvoice_Model
    {
        public string Title { get; set; }
        public string SuppID { get; set; }
        public string FromDate { get; set; }
        public string CI_ToDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public string FilterData { get; set; }
        public string wfdocid { get; set; }
        public string wfstatus { get; set; }
        public List<Status> StatusList { get; set; }
        public List<Supplier> SupplierNameList { get; set; }
        public string WF_status { get; set; }
        public string LPISearch { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class Supplier
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
}
