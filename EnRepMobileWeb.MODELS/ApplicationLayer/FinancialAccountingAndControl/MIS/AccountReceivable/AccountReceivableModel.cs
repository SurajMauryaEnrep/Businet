using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.AccountReceivable
{
    public class AccountReceivableModel
    {
        public string ARAging { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string Sales_by { get; set; }
        public string cust_id { get; set; }
        public string AccRcvablPDFData { get; set; }
        public string GstApplicable { get; set; }
        public string SlsPersId { get; set; }
        public string RegionName { get; set; }
        public string Basis { get; set; }
        public string sales_type { get; set; }
        public string Sales_per { get; set; }
        public string From_dt { get; set; }
        public string To_dt { get; set; }
        public string Inv_no { get; set; }
        public string Inv_dt { get; set; }
        public string Group { get; set; }
        public string Range1 { get; set; }
        public string Range2 { get; set; }
        public string Range3 { get; set; }
        public string Range4 { get; set; }
        public string Range5 { get; set; }
        public string ARAging_basis { get; set; }
        public string ARReceivableType { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string AppStatus { get; set; }
        public string BtnName { get; set; }
        public string SearchCity { get; set; }
        public string SearchState { get; set; }
        public string cust_catg { get; set; }
        public string cust_port { get; set; }
        public string cust_regin { get; set; }
        public string cust_zone { get; set; }
        public string cust_grp { get; set; }
        public string stateLData { get; set; }
        public string cityLdata { get; set; }
        public string state_id { get; set; }

        public List<RegionList> regionLists { get; set; }
        public List<customerZoneList> customerZoneLists { get; set; }
        public List<CustomerGroupList> CustomerGroupLists { get; set; }
        public List<CityList> CityLists { get; set; }
        public List<StateList> StateLists { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string Region { get; set; }
        public string customerZone { get; set; }
        public string CustomerGroup { get; set; }
        public List<CustCategoryList> categoryLists { get; set; }
        public string category { get; set; }
        public List<CustPortFolioList> portFolioLists { get; set; }
        public string portFolio { get; set; }
        public List<ARList> AccountRecivableList { get; set; }
        public List<InvoiceList> InvoiceList { get; set; }
        public string hdnPDFPrint { get; set; }
        public string searchValue { get; set; }
        public string hdnCSVPrint { get; set; }
        public string hdnCSVInsight { get; set; }
        public string hdnCSVInsightData { get; set; }
        public string hdnPaidAmtCSVInsight { get; set; }
        public string hdnPaidAmtCSVInsightData { get; set; }
        public string hdnAdvAmtCSVInsight { get; set; }
        public string ReceivableType { get; set; }
        public string ReportType { get; set; }
        public string HiddenCustId { get; set; }
        public string Hidcategory { get; set; }
        public string HidportFolioLists { get; set; }
        public string HidRegionName { get; set; }
        public string HidcustomerZone { get; set; }
        public string HidCustomerGroup { get; set; }
        public string Hidcity { get; set; }
        public string Hidstate { get; set; }
        public string hdnbr_ids { get; set; }
        public List<SalesPersList> SalesPersons { get; set; }
    }
    public class SalesPersList
    {
        public string sls_pers_id { get; set; }
        public string sls_pers_name { get; set; }
    }
    public class RegionList
    {
        public string region_id { get; set; }
        public string region_val { get; set; }

    }
    public class StateList
    {
        public string state_id { get; set; }
        public string state_name { get; set; }

    }
    public class CityList
    {
        public string city_id { get; set; }
        public string city_name { get; set; }

    }
    public class CustomerGroupList
    {
        public string cust_grp_id { get; set; }
        public string cust_grp_name { get; set; }

    }
    public class customerZoneList
    {
        public string cust_zone_id { get; set; }
        public string cust_zone_name { get; set; }

    }
    public class CustCategoryList
    {
        public string Cat_id { get; set; }
        public string Cat_val { get; set; }

    }
    public class CustPortFolioList
    {
        public string CatPort_id { get; set; }
        public string CatPort_val { get; set; }

    }
    public class ARList
    {
        public Int64 SrNo { get; set; }
        public string CustName { get; set; }
        public string CustId { get; set; }
        public string AccId { get; set; }
        public string Curr { get; set; }
        public string CurrId { get; set; }
        public string AmtRange1 { get; set; }
        public string AmtRange2 { get; set; }
        public string AmtRange3 { get; set; }
        public string AmtRange4 { get; set; }
        public string AmtRange5 { get; set; }
        public string AmtRange6 { get; set; }
        public string totamt_sp { get; set; }
        public string totamt_bs { get; set; }
        public string AdvanceAmount { get; set; }
        public string advamt_bs { get; set; }
        public string TotalAmt { get; set; }
        public string totnetamt_bs { get; set; }
        public string Inv_no { get; set; }
        public string Inv_dt { get; set; }
        public string salesPerson { get; set; }
        public string Invoice_Amt { get; set; }
        public string Paid_amt { get; set; }
        public string Balance_Amt { get; set; }
        public string Due_date { get; set; }
        public string due_days { get; set; }
        public string VouNo { get; set; }
        public string VouDate { get; set; }
        public string VouType { get; set; }
        public string cust_catg { get; set; }
        public string cust_port { get; set; }
        public string cust_regin { get; set; }
        public string cust_zone { get; set; }
        public string cust_grp { get; set; }
        public string stateLData { get; set; }
        public string cityLdata { get; set; }
    }

    public class InvoiceList
    {
        public string due_Date { get; set; }
        public string due_days { get; set; }
        public string Invoice_No { get; set; }
        public string Invoice_Dt { get; set; }
        public string Invoice_Date { get; set; }
        public string Invoice_Amt { get; set; }
        public string Paid_Amt { get; set; }
        public string Balance_Amt { get; set; }
        public string Pay_term { get; set; }
        public string Total_Invoice_Amt { get; set; }
        public string Total_Paid_Amt { get; set; }
        public string Total_Balance_Amt { get; set; }
        public string DocCode { get; set; }
        public string salesPerson { get; set; }

    }
}
