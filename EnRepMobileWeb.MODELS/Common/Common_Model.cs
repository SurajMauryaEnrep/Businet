using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.Common
{
    public class Common_Model
    {
        public List<statusLists1> statusListsC { get; set; }
        public string ddlCityName { get; set; }
        public string cmn_data { get; set; }
    }
    public class BindItemList
    {
        public string SearchName { get; set; }
    }
    public class SearchParams
    {
        public string SearchValue { get; set; }
    }

    public class statusLists1
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }

    public class SubItemPopupDt
    {
        public string Flag { get; set; }
        public string Flag1 { get; set; }
        public string _subitemPageName { get; set; }
        public DataTable dt_SubItemDetails { get; set; }
        public string IsDisabled { get; set; }
        public string ShowStock { get; set; }
        public string decimalAllowed { get; set; }
        public string stqty { get; set; }
        public string show_SrcDocNo { get; set; }
        public string show_SrcDocDt { get; set; }
    }

    public class CostCenterDt
    {
        public List<CostcntrType> costcntrtype { get; set; }
        public string cc_type_id { get; set; }
        public DataTable _CCItemDetails { get; set; }

        public string disflag { get; set; }

    }

    public class CostcntrType
    {
        public string cc_id { get; set; }
        public string cc_name { get; set; }
    }
    public class CommonTDS_Model
    {
        public List<TDS_Tmplt_List> tds_tmplt_list { get; set; }
        public List<TDS_Name_List> tds_name_list { get; set; }
        public string tmplt_id { get; set; }
        public string tds_id { get; set; }
        public string Disable { get; set; }

    }
    public class TDS_Tmplt_List
    {
        public string tmplt_id { get; set; }
        public string tmplt_name { get; set; }
    }
    public class TDS_Name_List
    {
        public string tds_id { get; set; }
        public string tds_name { get; set; }
        public string tds_acc_id { get; set; }
    }
    public class CommonAddress_Detail
    {
        public List<Country> countryList { get; set; }
        public List<State> stateList { get; set; }
        public string City { get; set; }
        public int City_Id { get; set; }
        public string City_Name { get; set; }
        public int? District_Id { get; set; }
        public string District_Name { get; set; }
        public string Dist { get; set; }
        public string State { get; set; }
        public int State_Id { get; set; }
        public string State_Name { get; set; }
        public int Country_Id { get; set; }
        public int Country_Name { get; set; }
        public string Country { get; set; }
        public string Pin { get; set; }
        public string PANNumber { get; set; }
        public string GSTNumber { get; set; }


    }
    public class Country
    {
        public string country_id { get; set; }
        public string country_name { get; set; }

    }
    public class State
    {
        public string state_id { get; set; }
        public string state_name { get; set; }

    }
    public class Cmn_OC_Detail
    { 
        public string oc_name { get; set; }
        public string curr_name { get; set; }
        public string curr_id { get; set; }
        public string supp_name { get; set; }
        public string supp_id { get; set; }
        public string conv_rate { get; set; }
        public string oc_id { get; set; }
        public string oc_val { get; set; }
        public string OCValBs { get; set; }
        public string tax_amt { get; set; }
        public string total_amt { get; set; }
    }
    public class Cmn_TP_Entity_List
    {
        public string id { get; set; }
        public string text { get; set; }
        public string acc_type { get; set; }
        public string supp_acc_id { get; set; }
    }
    public class Cmn_GL_Detail
    {
        public string comp_id { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string doctype { get; set; }
        public string Value { get; set; }
        public string ValueInBase { get; set; }
        public string DrAmt { get; set; }
        public string CrAmt { get; set; }
        public string TransType { get; set; }
        public string gl_type { get; set; }
        public string parent { get; set; }
        public string DrAmtInBase { get; set; } = "0";
        public string CrAmtInBase { get; set; } = "0";
        public string curr_id { get; set; }
        public string conv_rate { get; set; }
        public string bill_no { get; set; }
        public string bill_date { get; set; }

    }
    public class Cmn_GL_Detail1/*Added by Suraj on 07-08-2024 to add new parameter acc_id*/
    {
        public string comp_id { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string doctype { get; set; }
        public string Value { get; set; }
        public string ValueInBase { get; set; }
        public string DrAmt { get; set; }
        public string CrAmt { get; set; }
        public string TransType { get; set; }
        public string gl_type { get; set; }
        public string parent { get; set; }
        public string DrAmtInBase { get; set; } = "0";
        public string CrAmtInBase { get; set; } = "0";
        public string curr_id { get; set; }
        public string conv_rate { get; set; }
        public string bill_no { get; set; }
        public string bill_date { get; set; }
        public string acc_id { get; set; }

    }

    public class Cmn_PrintOptions
    {
        public string PrintFormat { get; set; } = "F1";
        public string ShowProdDesc { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string ItemAliasName { get; set; } = "N";
        public string PrintRemarks { get; set; } = "N";
        public string CustAliasName { get; set; } = "N";
        public string ShowDeliverySchedule { get; set; } = "Y";
        public string ShowHsnNumber { get; set; } = "Y";
        public string ShowRemarksBlwItm { get; set; } = "N";
        public string ShowPayTerms { get; set; } = "N";
        
        public string ShowTotalQty { get; set; } = "Y";//add by shubham Maurya on 02-04-2025
        public string ShowMRP { get; set; } = "N";//add by shubham Maurya on 02-04-2025
        public string ShowPackSize { get; set; } = "N";//add by shubham Maurya on 02-04-2025
        public string SupplierAliasName { get; set; } = "N";
        /*Add Start all below columns by Hina Sharma on 25-07-2025 only for Sales Quotation*/
        public string ReferenceNo { get; set; } = "N";
        public string CatalogueNo { get; set; } = "N";
        public string OEMNo { get; set; } = "N";
        public string Discount { get; set; } = "Y";
        public string ShipTo { get; set; } = "Y";
        public string BillTo { get; set; } = "Y";
        public string BankDetail { get; set; } = "Y";
        public string CompAddress { get; set; } = "Y";
        public string PrintImage { get; set; } = "N";
        /*End all below columns by Hina Sharma on 25-07-2025 only for Sales Quotation*/
    }
    public class Cmn_pdfGenerate_model
    {
        public string localLogoPath { get; set; }
        public string localDigiSignPath { get; set; }
    }
    public class DateRange
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    // Models For Csv
    public class DataTableRequest
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
        public List<Column> columns { get; set; }
        public string ItemListFilter { get; set; }
        public List<ExportColumn> ColumnsExport { get; set; }
    }

    public class Search { public string value { get; set; } }
    public class Order { public int column { get; set; } public string dir { get; set; } }
    public class Column { public string data { get; set; } public string name { get; set; } }
    public class ExportColumn { public string data { get; set; } public string title { get; set; } }

    // Models For Csv End
}
