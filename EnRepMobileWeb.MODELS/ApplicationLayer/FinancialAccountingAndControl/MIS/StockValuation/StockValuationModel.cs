using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.StockValuation
{
   public class StockValuationModel
    {
        public string Title { get; set; }
        public string ddlGroup { get; set; }
        public List<GroupName> GroupList { get; set; }
        public string FinYr { get; set; }
        public string HdnFinYr { get; set; }
        public List<Finyear> Finyrlist { get; set; }
        public string ddl_Month { get; set; }
        public List<Month> ddl_MonthList { get; set; }
        public string ReportType { get; set; }
        public string SearchStatus { get; set; }
        public string searchValue { get; set; }
        public string hdnCSVPrint { get; set; }
        public string StockValuationData { get; set; }
        public string fitler_type { get; set; }
        public string hdn_grp_id { get; set; }
        public string rpt_type { get; set; }
        public string list_no { get; set; }
        public List<StockValuationList> _StockValuation { get; set; }
        public List<PriceList> ddlPriceLists { get; set; } = new List<PriceList>() { new PriceList { list_no = 0, list_name = "---Select---" } };

    }
    public class StockValuationList
    {
        public Int64 SrNo { get; set; }
        public string Item_Name {get;set;}
        public string UOM {get;set;}
        public string Group_Name {get;set;}
        public string Group_Structure {get;set;}
        public string Opening_Quantity {get;set;}
        public string Opening_Value {get;set;}
        public string Receipt_Quantity {get;set;}
        public string Receipt_Value {get;set;}
        public string Issue_Quantity {get;set;}
        public string Issue_Value {get;set;}
        public string Closing_Quantity {get;set;}
        public string Closing_Quantity_Specific { get;set;}
        public string Closing_Value { get;set;}
        public string Acc_name { get;set;}
    }
    public class GroupName
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class Finyear
    {
        public string FinyrId { get; set; }
        public string Finyrs { get; set; }
    }
    public class Month
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class PriceList
    {
        public int list_no { get; set; }
        public string list_name { get; set; }
        public string valid_fr { get; set; }
        public string valid_to { get; set; }
    }
}
