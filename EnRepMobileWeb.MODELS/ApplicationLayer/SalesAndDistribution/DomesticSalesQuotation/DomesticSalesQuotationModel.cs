using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSalesQuotation
{
     public class DomesticSalesQuotationModel
    {
        public List<CustomerName> CustomerNameList { get; set; }
        public Boolean FClosed { get; set; }
        public string FClosedFlag { get; set; }
        public string rev_no { get; set; }
        public string AmendmentFlag { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string DocumentMenuId { get; set; }
        public string GstApplicable { get; set; }
        public string SQ_no { get; set; }
        public string SQ_dt { get; set; }
        public string CustPros_type { get; set; }
        public string Cust_name { get; set; }
        public string SQ_CustName { get; set; }
        public string SpanCustPricePolicy { get; set; }
        public string SpanCustPriceGroup { get; set; }
        public string SalePerson { get; set; }
        public Boolean Cancelled { get; set; }
        public string QTStatus { get; set; }
        public string DtRemarks { get; set; }
        public string QtType { get; set; }
        public string Customer_type { get; set; }
        public string Currenc { get; set; }
        public string curr_id { get; set; }
        public string bs_curr_id { get; set; }
        
        public string convrate { get; set; }
        public string Cust_id { get; set; }
        public string salep_id { get; set; }
        public string SQ_SalePersonID { get; set; }
        public string SQ_SalePersonName { get; set; }
        
        public string SQ_SalePerson { get; set; }
        public string GrVal { get; set; }
        public string AssessableVal { get; set; }
        public string DiscAmt { get; set; }
        public string TaxAmt { get; set; }
        public string OcAmt { get; set; }
        public string MarginPercentage { get; set; }
        public string TotalLandedCost { get; set; }
        public string NetValBs { get; set; }
        public string NetValSpec { get; set; }
        public string BillingAddres { get; set; }
        public int Billing_id { get; set; }
        public string ShippingAddres { get; set; }
        public int Shipping_id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string SQ_ItemDetail { get; set; }
        public string ItemTaxdetails { get; set; }
        
        public string ItemOCdetails { get; set; }
        public string ItemOCTaxdetails { get; set; }
        public string ItemTermsdetails { get; set; }
        //public string Cust_type { get; set; }
        //public string SO_SalePerson { get; set; }
        //public string SO_ItemName { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public Boolean RaiseOrder { get; set; }
        public string hdnRaiseOrder { get; set; }
        
        //public string TransType { get; set; }
        //public string CompID { get; set; }
        //public string BranchID { get; set; }
        //public string qt_type { get; set; }
        //public string qt_no { get; set; }
        //public string qt_date { get; set; }
        //public string cust_type { get; set; }
        ////public string curr { get; set; }

        //public string remarks { get; set; }
        //public string UserID { get; set; }

        //public string MacID { get; set; }



        //public string Branch { get; set; }
        //public string QTNo { get; set; }
        //public string QTDate { get; set; }
        //public string ItemID { get; set; }
        //public string UOMID { get; set; }
        //public string QuotQty { get; set; }
        //public string OrderBQty { get; set; }
        //public string ItmRate { get; set; }
        //public string ItmDisPer { get; set; }
        //public string ItmDisAmt { get; set; }
        //public string DisVal { get; set; }
        //public string GrossVal { get; set; }
        //public string AssVal { get; set; }

        //public string OCAmt { get; set; }

        //public string NetValBase { get; set; }

        //public string Cust_Type { get; set; }


        public string WF_BarStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string WF_Status { get; set; }
        public string UserID { get; set; }
        public string Createid { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string Approve_id { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string DeleteCommand { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        
        public string TblItemTaxdetails { get; set; }
        public string TblItemOCdetails { get; set; }
        public string attatchmentdetail { get; set; }
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public List<CustName> CustNameList { get; set; }
        public List<Currency> CurrencyList { get; set; }
        public List<SalePerson> SalePersonList { get; set; }

        public string ForAmmendendBtn { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string DocumentStatus { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string ProspectFromQuot { get; set; }
        public string CustType { get; set; }
        public string WF_status1 { get; set; }
        public string ProspectFromRFQ { get; set; }
        public string Amend { get; set; }
        public string AppStatus { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        //public string TaxCalci_ItemID { get; set; }

        //public string TaxCalci_AssessableValue { get; set; }
        //public string TaxCalci_Tax_Template { get; set; }
        //public string TaxCalci_Tax_Type { get; set; }
        //public string TaxCalci_TaxName { get; set; }
        //public string TaxCalci_TaxNameID { get; set; }
        //public string TaxCalci_Tax_Percentage { get; set; }
        //public string TaxCalci_Level { get; set; }

        //public string TaxCalci_ApplyOn { get; set; }
        //public string TaxCalci_Tax_Amount { get; set; }
        //public List<TaxCalciTaxName> TaxCalciTaxNameList { get; set; }
        //public string OcCalci_OtherCharge { get; set; }
        //public string OcCalci_OCCurrency { get; set; }
        //public string OcCalci_OCconv_rate { get; set; }
        //public string OcCalci_OCAmount { get; set; }
        //public string OcCalci_OCAmtInBs { get; set; }
        //public List<OcCalciOtherCharge> OcCalciOtherChargeList { get; set; }

        public string ShowProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string PrintFormat { get; set; } = "F1";
        public string CustAliasName { get; set; } = "N"; 
        public string PrintRemarks { get; set; } = "N";
        public string ShowItemAliasName { get; set; } = "N";
        public string ShowReferenceNo { get; set; } = "N";
        public string ShowCatalogueNo { get; set; } = "N";
        public string ShowOEMNo { get; set; } = "N";
        public string ShowHsnCode { get; set; } = "Y";
        public string ShowDiscount { get; set; } = "Y";
        public string ShowShipTo { get; set; } = "Y";
        public string ShowBillTo { get; set; } = "Y";
        public string ShowBankDetail { get; set; } = "Y";
        public string ShowCompAddress { get; set; } = "Y";
        public string ShowPrintImage { get; set; } = "N";
        
    }
    public class URLDetailModel
    {
        public string Command { get; set; }
        public string rev_no { get; set; }
        public string AmendmentFlag { get; set; }
        public string Amend { get; set; }
        public string BtnName { get; set; }
        public string TransType { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string DocumentMenuId { get; set; }
        public string CustType { get; set; }
    }
    public class SalesQuotationModelattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class TaxCalciTaxName
    {
        public string tax_id { get; set; }
        public string tax_name { get; set; }
    }
    public class OcCalciOtherCharge
    {
        public string oc_id { get; set; }
        public string oc_name { get; set; }
    }
    public class CurrentDetail
    {
        public string CurrentUser { get; set; }
        public string CurrentDT { get; set; }
    }
    public class CustName
    {
        public string Cust_name { get; set; }

        public string Cust_id { get; set; }
    }
    public class Currency
    {
        public string curr_id { get; set; }
        public string curr_val { get; set; }

    }
    public class SalePerson
    {
        public string salep_id { get; set; }
        public string salep_name { get; set; }

    }
    public class QTTermsDeatil
    {
        public string CompID { get; set; }
        public string Branch { get; set; }
        public string QTNo { get; set; }
        public string QTDate { get; set; }
        public string TermsDescription { get; set; }
        public string QtType { get; set; }
    }
}
