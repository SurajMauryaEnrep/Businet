using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.InterBranchSale
{
    public class InterBranchSale_Model
    {
        public string Title { get; set; }
        public string DeleteCommand { get; set; }
        public string Inv_no { get; set; }
        public string Sinv_no { get; set; }
        public string Inv_dt { get; set; }
        public string cust_id { get; set; }
        public string CustName { get; set; }
        public string Message { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string _ModelCommand { get; set; }
        public string BtnName { get; set; }
        public string cust_trnsport_id { get; set; }
        public string DocumentMenuId { get; set; }
        public Boolean CancelFlag { get; set; }
        public string GLVoucherDt { get; set; }
        public string GLVoucherNo { get; set; }
        public string GLVoucherType { get; set; }
        public string WFBarStatus { get; set; }
        public string doc_status { get; set; }
        public string pmflagval { get; set; }
        public string conv_rate { get; set; }
        public string cust_acc_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string StatusName { get; set; }
        public string bill_add_id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string Address { get; set; }
        public string GrossValue { get; set; }
        public string TaxAmount { get; set; }
        public string OtherCharges { get; set; }
        public string NetAmountInBase { get; set; }
        public string curr_id { get; set; }
        public string bs_curr_id { get; set; }
        public string ExRate { get; set; }
        public string Currency { get; set; }
        public string DocSuppOtherCharges { get; set; }
        public string GR_No { get; set; }
        public string GR_Dt { get; set; }
        public string HdnGRDate { get; set; }
        public string Transpt_NameID { get; set; }
        public string HdnTrnasportName { get; set; }
        public string attatchmentdetail { get; set; }
        public string Veh_Number { get; set; }
        public string Driver_Name { get; set; }
        public string Mob_No { get; set; }
        public string Tot_Tonnage { get; set; }
        public string Remarks { get; set; }
        public string CustRefNo { get; set; }
        public string CustRefDt { get; set; }
        public string IRNNumber { get; set; }
        public string slprsn_id { get; set; }
        public string Ship_Add_Id { get; set; }
        public string ShippingAddress { get; set; }
        public string PlaceOfSupply { get; set; }
        public string No_Of_Packages { get; set; }
        public string Invoice_Heading { get; set; }
        public string ShipFromAddress { get; set; }
        public string ddlPayment_term { get; set; }
        public string ddlDelivery_term { get; set; }
        public string Declaration_1 { get; set; }
        public string Declaration_2 { get; set; }
        public Boolean RoundOffFlag { get; set; }
        public Boolean RCMApplicable { get; set; }
        public Boolean nontaxable { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string SSIStatus { get; set; }
        public string DocumentStatus { get; set; }
        public string Nurration { get; set; }
        public string BP_Nurration { get; set; }
        public string DN_Nurration { get; set; }
        public string CC_DetailList { get; set; }
        public string TblItemOCdetails { get; set; }
        public string ListFilterData1 { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string Hd_GstType { get; set; }
        public string Hd_GstCat { get; set; }
        public string WF_status1 { get; set; }
        public string EditCommand { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string ShowProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string PrintShipFromAddress { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string CustomerAliasName { get; set; } = "N";
        public string ShowTotalQty { get; set; } = "Y";
        public string PrintFormat { get; set; } = "F";
        public string GstApplicable { get; set; }
        public string ShowWithoutSybbol { get; set; } = "Y";
        public string showDeclare1 { get; set; } = "N";
        public string showDeclare2 { get; set; } = "N";
        public string showInvHeading { get; set; } = "N";
        public string ShowPackSize { get; set; } = "N";
        public int NumberofCopy { get; set; } = 1;
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string SalePerson { get; set; }
        public string PV_Narration { get; set; }
        public string BP_Narration { get; set; }
        public string CN_Narration { get; set; }
        public string DN_Narration { get; set; }
        public string DN_Narration_Tcs { get; set; }
        public string tcs_details { get; set; }
        public string oc_tds_details { get; set; }
        public string AppStatus { get; set; }
        public string Narration { get; set; }
        public string Itemdetails { get; set; }
        public string ItemTaxdetails { get; set; }
        public string ItemOCdetails { get; set; }
        public string OC_TaxDetail { get; set; }
        public string vouDetail { get; set; }
        public string Guid { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string SaleVouMsg { get; set; }
        /*----------------------Print Detail Start-------------------------------*/
        public string HdnPrintOptons { get; set; }
        public string PrtOpt_catlog_number { get; set; }
        public string PrtOpt_item_code { get; set; }
        public string PrtOpt_item_desc { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public List<SalesPersonName> SalesPersonNameList { get; set; }
        public List<TransListModel> TransList { get; set; }
    }
    public class IBSListModel
    {
        public string Title { get; set; }
        public string wfstatus { get; set; }
        public string SSI_FromDate { get; set; }
        public string SSI_ToDate { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string wfdocid { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string attatchmentdetail { get; set; }
        public string CustName { get; set; }
        public string CustID { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public List<Status> StatusList { get; set; }
        //public List<ScrapSaleInvoiceList> SSIList { get; set; }
        public string ListFilterData { get; set; }
        public string WF_status { get; set; }
        public string SSISearch { get; set; }
    }
    public class IBSAttch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
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
    public class CustomerName
    {
        public string Cust_id { get; set; }
        public string Cust_name { get; set; }
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
    public class SalesPersonName
    {
        public string slprsn_id { get; set; }
        public string slprsn_name { get; set; }
    }
    //public class TransListModel
    //{
    //    public string TransId { get; set; }
    //    public string TransName { get; set; }
    //}
}
