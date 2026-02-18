using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.MIS.ProcurementDetail
{
    public class ProcurementDetail_Model
    {
        public List<HSNno> HSNList { get; set; }
        public string ddlhsncode { get; set; }
        public string HSN_code { get; set; }
        public string Title { get; set; }
        public string ItemID { get; set; }
        public string purchase_by { get; set; }
        
        public string RegionName { get; set; }
        public string Inv_type { get; set; }
        
        public string From_dt { get; set; }
        public string To_dt { get; set; }
        public string Inv_no { get; set; }
        public string Inv_dt { get; set; }
        public string Group { get; set; }
        
        public string SuppName { get; set; }
        public string Supp_id { get; set; }
        public string HdnMultiselectSuppName { get; set; }
        public string ItemGroupName { get; set; }
        public string HdnMultiselectItmGrpName { get; set; }
        //public string GroupID { get; set; }
        public string Item_PortFolio { get; set; }
        //public string PrfID { get; set; }
        public string HdnMultiselectItmportfolioName { get; set; }
        public string GroupName { get; set; }
        public string PortfolioName { get; set; }
        
        public List<SupplierName> SupplierNameList { get; set; }
        public List<ItemGroupName> ItemGroupNameList { get; set; }
        public List<ItemPortfolio> ItemPortfolioList { get; set; }
        public List<SuppCategoryList> categoryLists { get; set; }
        public string category { get; set; }
        public string HdnMultiselectSuppCatgry { get; set; }

        public List<SuppPortFolioList> portFolioLists { get; set; }
        public string Supp_PortFolio { get; set; }
        public string HdnMultiselectPortfolio { get; set; }
        public string Command { get; set; }
        public string CsvData { get; set; }
        public string TaxColumnJson_forCsv { get; set; }
        public string hdnCSVPrint { get; set; }
        public string hdnInvoiceWiseItemDetail { get; set; }
        public string hdnSupplierWiseInvoiceDetail { get; set; }
        public string hdnSupplierWiseInvoiceItemDetail { get; set; }
        public string hdnItemWiseInvoiceDetail { get; set; }
        public string hdnItemGroupWiseItemDetail { get; set; }
        public string hdnItemGroupWiseItemInvoiceDetail { get; set; }
        public string hdnInsightBtn { get; set; }
        /*Code start Add by Hina Sharma on 27-06-2025*/
        public string supp_gst_no { get; set; }
        public string tds_amt { get; set; }
        public Boolean RCMApplicable { get; set; }
        public string hdnbr_id { get; set; }
        
        /*Code End Add by Hina Sharma on 27-06-2025*/

    }
    public class HSNno
    {
        public string setup_val { get; set; }
        public string setup_id { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class ItemGroupName
    {
        public string Group_Id { get; set; }
        public string Group_Name { get; set; }
    }

    public class ItemPortfolio
    {
        public string Prf_Id { get; set; }
        public string Prf_Name { get; set; }
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
        public string HSN_code { get; set; }
        public string RCMApp { get; set; }/*add by Hina sharma on 30-06-2025*/
        public string csvFlag { get; set; } = "";/*add by Suraj Maurya on 10-10-2025*/
        public string brid_list { get; set; }
    }
   
    public class SuppCategoryList
    {
        public string Cat_id { get; set; }
        public string Cat_val { get; set; }

    }
    public class SuppPortFolioList
    {
        public string Portfolio_id { get; set; }
        public string Portfolio_val { get; set; }

    }
    public class PrcDtlInvWiseSummaryModel/* Added by Suraj Maurya on 30-05-2025 */
    {
        public Int64 SrNo { get; set; }
        public string supp_name { get; set; }
        public string gst_cat { get; set; }
        public string inv_type { get; set; }
        public string curr_symbol { get; set; }
        public string inv_no { get; set; }
        public string inv_dt1 { get; set; }
        public string inv_dt { get; set; }
        public string rcm_app { get; set; }
        public string bill_no { get; set; }
        public string bill_dt { get; set; }
        public string purchase_amount_spec { get; set; }
        public string purchase_amount_bs { get; set; }
        public string tax_amt_spec { get; set; }
        public string tax_amt_bs { get; set; }
        public string oc_amt_spec { get; set; }
        public string oc_amt_bs { get; set; }
        public string invoice_amt_spec { get; set; }
        public string invoice_amt_bs { get; set; }
        public string app_dt { get; set; }
    }
    public class PrcDtlInvWiseDetailedModel/* Added by Suraj Maurya on 30-05-2025 */
    {
        public Int64 SrNo { get; set; }
        public string supp_name { get; set; }
        public int supp_id { get; set; }
        public string inv_type { get; set; }
        public string curr_symbol { get; set; }
        public string inv_no { get; set; }
        public string inv_dt1 { get; set; }
        public string inv_dt { get; set; }
        public string bill_no { get; set; }
        public string bill_dt { get; set; }
        public string item_name { get; set; }
        public string item_id { get; set; }
        public int item_grp_id { get; set; }
        public string item_group_name { get; set; }
        public string uom_alias { get; set; }
        public string HSN_code { get; set; }
        public string mr_qty { get; set; }
        public string item_rate_spec { get; set; }
        public string item_rate_bs { get; set; }
        public string purchase_amount_spec { get; set; }
        public string purchase_amount_bs { get; set; }
        public string tax_amt_spec { get; set; }
        public string tax_amt_bs { get; set; }
        public string oc_amt_spec { get; set; }
        public string oc_amt_bs { get; set; }
        public string invoice_amt_spec { get; set; }
        public string invoice_amt_bs { get; set; }
        public string app_dt { get; set; }
        /*Code start Add by Hina Sharma on 27-06-2025*/
        public string gst_cat { get; set; }
        public string supp_gst_no { get; set; }
        public string tds_amt { get; set; }
        public string rcm_app { get; set; }
        public string igst { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string igstRate { get; set; }
        public string cgstRate { get; set; }
        public string sgstRate { get; set; }
        /*Code End Add by Hina Sharma on 27-06-2025*/
        public int br_id { get; set; }
        public string roundoff_diff { get; set; }
    }


}
