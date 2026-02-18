using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.TDSDetail
{
   public class TDSDetailModel
    {
        public string Title { get; set; }
        public List<TDS_Name_List> tds_name_list { get; set; }
        public List<Sec_Code_List> seccode_list { get; set; }
        public List<Tax_Type_List> tax_type_list { get; set; } = new List<Tax_Type_List>()
        { new Tax_Type_List { tax_type="0",tax_type_name="All"},
        new Tax_Type_List { tax_type = "tds", tax_type_name = "TDS" },
        new Tax_Type_List { tax_type="tcs",tax_type_name="TCS"}
         };//Added by Suraj Maurya on 06-02-2025 to Add Default Values to the List
        public string tds_id { get; set; }
        public string Supp_id { get; set; }
        public string From_dt { get; set; }
        public string To_dt { get; set; }
        public string SearchStatus { get; set; }
        public string SuppName { get; set; }
        public string tax_type { get; set; }
        public string searchValue { get; set; }
        public string hdnCSVPrint { get; set; }
        public string TDSDetailData { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<TDS_Detail_List> _TDS_Detail_List { get; set; }
    }
    public class TDS_Detail_List
    {
        public Int64 SrNo { get; set; }
        public string Deduction_Date {get;set;}
        public string TDS_Name {get;set;}
        public string Section_Code { get; set; }
        public string Supplier_Name {get;set;}
        public string TAN_Number {get;set;}
        public string PAN_number {get;set;}
        public string City_State {get;set;}
        public string Bill_Number {get;set;}
        public string Bill_Date {get;set;}
        public string Taxable_Amount {get;set;}
        public string TDS_Percentage { get; set; }
        public string TDS_Amount {get;set;}
        public string Debit_Note_Number { get;set;}
        public string Debit_Note_Date { get;set;}
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class Search_model
    {
        public Search_Parmeters search_prm { get; set; }
    }
    public class Search_Parmeters
    {
        public string CompId { get; set; }
        public string BrId { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string GroupName { get; set; }
        public string Supp_id { get; set; }
        public string Supp_Name { get; set; }
        public string Inv_type { get; set; }
        public string Uom { get; set; }
        public string Qty { get; set; }
        public string currency { get; set; }
        public string Item_PortFolio { get; set; }
        public string From_dt { get; set; }
        public string To_dt { get; set; }
        public string Inv_no { get; set; }
        public string Inv_dt { get; set; }
        public string Group { get; set; }
        public string category { get; set; }
        public string portFolio { get; set; }
        public string flag { get; set; }

    }
    public class Tax_Type_List
    {
        public string tax_type { get; set; }
        public string tax_type_name { get; set; }
    }
    public class TDS_Name_List
    {
        public string tds_id { get; set; }
        public string tds_name { get; set; }
        public string tds_acc_id { get; set; }
    }
    public class Sec_Code_List
    {
        public string seccode_id { get; set; }
        public string seccode_name { get; set; }
        
    }
}


