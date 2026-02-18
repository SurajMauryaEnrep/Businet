using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesEnquiry
{
   public class SalesEnquiryModel
    {
        ///*------All Detail Page Data-------------*/
        //public string SEType { get; set; }
        public string Enquiry_type { get; set; }
        public string ProspectFromEnquiry { get; set; }
        public string CustPros_type { get; set; }
        public string CustType { get; set; }
        public string EnquiryNo { get; set; }
        public string EnquiryDt { get; set; }
        public string EnquirySource { get; set; }
        public string Cust_id { get; set; }
        public string SE_CustName { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public string BillingAddress { get; set; }
        public int Billing_id { get; set; }
        public string cont_pers { get; set; }
        public string cont_email { get; set; }
        public string cont_num { get; set; }
        public string cont_web { get; set; }
        public string Currency { get; set; }
        public string curr_id { get; set; }
        public string curr_name { get; set; }
        public List<Currency> CurrencyList { get; set; }
        public string convrate { get; set; }
        public string SE_SalePerson { get; set; }
        public string SE_SalePersonID { get; set; }
        public string SE_SalePersonName { get; set; }
        public List<SalePerson> SalePersonList { get; set; }
        public string Remarks { get; set; }

        public string SE_ItemDetail { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string Communicationdetails { get; set; }
        public string hdnQuationCreated { get; set; }
        public Boolean QuotationCreated { get; set; }
        public string QuotationNumDt { get; set; }
        
        /*------End All Detail Page Data-------------*/
        /*------Start All Common Data-------------*/
        public string Title { get; set; }
        public string DocumentMenuId { get; set; }
        public string GstApplicable { get; set; }
        public string UserID { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string DocumentStatus { get; set; }
        public string DeleteCommand { get; set; }
        public string SEStatus { get; set; }
        public string Createid { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string AppStatus { get; set; }
        public string attatchmentdetail { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }

        /*--------For COMMUNICATION DETAIL SECTION-------------------*/
        public string CommunicatnTyp { get; set; }
        public string CallDate { get; set; }
        public string ContactedBy { get; set; }
        
        public string ContactedTo { get; set; }
        public string ContactDetail { get; set; }
        public string DiscussRemarks { get; set; }
        
        
    }
    public class CustomerName
    {
        public string Cust_id { get; set; }
        public string Cust_name { get; set; }
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
    public class SalesEnquiryModelattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class URLDetailModel
    {
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string TransType { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string DocumentMenuId { get; set; }
        public string EnquiryType { get; set; }
        public string CustType { get; set; }
    }
    public class SEListModel
    {
        public string DocumentMenuId { get; set; }
        public string EnqryTyp { get; set; }
        public string CustTyp { get; set; }
        public string EnqSrc { get; set; }
        public string SlsPrsn { get; set; }
        public string SESearch { get; set; }
        public string Title { get; set; }
        public string CustName { get; set; }
        public string CustID { get; set; }
        public string ListFilterData { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Catgry { get; set; }
        public string Portfolio { get; set; }
        public string Region { get; set; }
        public string attatchmentdetail { get; set; }
        public List<CustNameOnList> CustNameList { get; set; }
        //public string SalePerson { get; set; }
        //public string SE_SalePersonID { get; set; }
        //public string SE_SalePersonName { get; set; }
        public List<SalePersonOnList> SlsPrsnOnList { get; set; }
        public List<SlsEnqryList> SalesEnquiryList { get; set; }
        public List<Category> CategoryList { get; set; }

        public List<PortFolio> PortFolioList { get; set; }
        public List<Region> RegionList { get; set; }
        public List<Status> StatusList { get; set; }
        
    }
    
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }

    public class CustNameOnList
    {
        public string Cust_id { get; set; }
        public string Cust_name { get; set; }
    }
    public class Category
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }
    }
    public class PortFolio
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }
    }
    public class Region
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }

    }
    public class SalePersonOnList
    {
        public string salep_id { get; set; }
        public string salep_name { get; set; }

    }

    public class SlsEnqryList
    {
        public string FinStDt { get; set; }
        public string SENumber { get; set; }
        public string SEDate { get; set; }
        public string SE_Dt { get; set; }
        public string Enqry_Src { get; set; }
        public string Enqry_Typ { get; set; }
        public string Cust_Typ { get; set; }
        public string CustName { get; set; }
        public string Region { get; set; }
        public string Portfolio { get; set; }
        public string Category { get; set; }
        public string Sls_Persn { get; set; }
        public string QuotaionNumDate { get; set; }
        public string SE_StatusList { get; set; }
        public string CreatedON { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        //public string ApprovedOn { get; set; }


    }
}
