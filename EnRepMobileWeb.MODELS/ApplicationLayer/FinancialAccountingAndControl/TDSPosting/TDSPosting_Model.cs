using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.TDSPosting
{
    public class TDSPosting_Model
    {
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public List<MonthList> monthList { get; set; }
        public string MonthNo { get; set; }
        public List<YearList> yearList { get; set; }
        public string Year { get; set; }
        public string tds_id { get; set; }
        public string ListFilterData1 { get; set; }
        public List<GlAccList> glAccList { get; set; }
        public string acc_id { get; set; }
        public string cal_on { get; set; }
        public string PrePeriod { get; set; }
        public string Period { get; set; }
        public string from_dt { get; set; }
        public string to_dt { get; set; }
        public string TdsDetails { get; set; }
        public string TdsSlabDetails { get; set; }
        public string TdsSuppInvDetails { get; set; }
        public string TdsSuppInvSlabDetails { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentMenuId { get; set; }
        public string Tds_dt { get; set; }
        public string DocStatus { get; set; }
        public string BtnCommand { get; set; }
        public string searchValue { get; set; }
        public string TransType { get; set; }
        public string DeleteCommand { get; set; }
        public string Create_dt { get; set; }
        public string Create_by { get; set; }
        public string App_dt { get; set; }
        public string App_By { get; set; }
        public string PreStDt { get; set; }
        public string PreEndDt { get; set; }
        public string CurrStDt { get; set; }
        public string CurrEndDt { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string create_id { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string CC_DetailList { get; set; }
        public string vouDetail { get; set; }
    }
    public class MonthList
    {
        public string month_no { get; set; }
        public string month_name { get; set; }

    }
    public class YearList
    {
        public string Year { get; set; }
        public string YearVal { get; set; }
    }
    public class GlAccList
    {
        public string acc_id { get; set; }
        public string acc_name { get; set; }
    }
    public class UrlData
    {
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string tds_id { get; set; }
        public string ListFilterData1 { get; set; }
    }
    public class TdsListModel
    {
        public string Title { get; set; }
        public string DocumentMenuId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Status { get; set; }
        public List<MonthList> monthList { get; set; }
        public string MonthNo { get; set; }
        public List<YearList> yearList { get; set; }
        public List<statusLists> statusLists { get; set; }
    }
    public class statusLists
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }


}
