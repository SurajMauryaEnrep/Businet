using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesForecast
{
   public class SalesForecastModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string WF_Status1 { get; set; }
        public string f_period { get; set; }
        public string f_fy { get; set; }
        public string f_freq { get; set; }
        public string SaveUpd { get; set; }
        public string err { get; set; }
        public string dbclickfc { get; set; }
        public string WF_Status { get; set; }
        public string DocumentStatusfc { get; set; }
        public string TransTypefc { get; set; }
        public string Commandfc { get; set; }
        public string Messagefc { get; set; }
        public string BtnNamefc { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public string sf_id { get; set; }
        public string TransType { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedOn { get; set; }
        public string Sforca_no { get; set; }
        public string CreatedDate { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string WFBarStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string WFStatus { get; set; }
        public string Createid { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string mac_id { get; set; }
        public string DeleteCommand { get; set; }
        public Boolean CancelFlag { get; set; }
        public string f_status { get; set; }
        public string f_statusId { get; set; }
        public string ddl_f_frequency { get; set; }
        public List<f_frequency> ddl_f_frequencyList { get; set; }
        public string ddl_financial_year { get; set; }
        public List<financial_year> ddl_financial_yearList { get; set; }
        public string ddl_period { get; set; }
        public List<period> ddl_periodList { get; set; }
        public string txtFromDate { get; set; }
        public string txtToDate { get; set; }
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string uom_Name { get; set; }
        public string uom_id { get; set; }
        public string txtPreviousYearSalesInQuantity { get; set; }
        public string txtPreviousYearSalesInAmount { get; set; } 
        public string txtSalesPrice { get; set; }
        public string txtTargetSalesValue { get; set; }
        public string txtIncreaseByInPercentage { get; set; }
        public string txtReducedByInPercentage { get; set; }
        public string txtTargetSaleQuantity { get; set; }
        public string hdn_actual_sale_qty { get; set; }
        public string hdn_actual_sale_value { get; set; }
        public string footer_PreviousYearSalesInValue { get; set; }
        public string footer_TargetSalesInValue { get; set; }
        public string footer_ActualSaleInValue { get; set; }
        public string SFCItemdetails { get; set; }
        public List<FC_Item_details> FC_Item_Details_List { get; set; }

    }
    public class UrlModel
    {
        public string BtnNamefc { get; set; }
        public string Commandfc { get; set; }
        public string sf_id { get; set; }
        public string dbclickfc { get; set; }
        public string TransTypefc { get; set; }
        public string WF_Status1 { get; set; }
        public string AppStatus { get; set; }
        public string DocumentMenuId { get; set; }
        public string DocumentStatus { get; set; }
    }
    public class FC_Item_details
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string sub_item { get; set; }
        public string uom_id { get; set; }
        public string uom_alias { get; set; }
        public string pys_q { get; set; }
        public string pys_v { get; set; }
        public string inc_by { get; set; }
        public string red_by { get; set; }
        public string tgt_qty { get; set; }
        public string sale_price { get; set; }
        public string tgt_val { get; set; }
        public string actual_sale_q { get; set; }
        public string actual_sale_v { get; set; }
    }
    public class period
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class f_frequency
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class financial_year
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
