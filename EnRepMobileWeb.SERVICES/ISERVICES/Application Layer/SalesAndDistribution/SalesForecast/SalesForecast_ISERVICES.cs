using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesForecast
{
   public interface SalesForecast_ISERVICES
    {
        string insertFCDetail(DataTable cjHeader, DataTable jcItem, DataTable dtSubItem);
        string cancelFCDetail(int CompID, int BrID, string f_freq, string f_fy, string f_period,string create_id,string transtype);
        //DataSet BindProductNameInDDL(string CompID, string BrID, string ItmName);
        DataSet BindFinancialYear(int CompID, int BrID,string f_freq,string StartDate, string Period);
        DataSet BindPeriod(int CompID, int BrID, string f_freq, string StartDate);
        DataSet BindDateRangeCal(int CompID, int BrID,string f_frequency, string start_year, string end_year, Int32 months, string fy_datefrom, string fy_dateto, string ItmName, string f_fy_full);
        DataSet BindItemNameInDDL(int CompID, int BrID,string itemname);
        DataSet GetSOItemUOMDL(string comp_id, string BrID, string Itm_id, string fromdate, string todate);// Binding for UON
        DataTable GetFCList(int CompID, int BrID, string UserID, string wfstatus, string DocumentMenuId);
        DataSet BinddbClick(int CompID, int BrID, /*string f_freq, string f_fy, int f_period, //Commented by Suraj */string sf_id,string UserID, string DocumentMenuId);
        string Approve_SalesForecast(string comp_id, string br_id, string sf_id, string sfc_date, string A_Status, string A_Level, string A_Remarks, string CreatedBy, string mac_id, string f_fy, string status, string period, string DocumentMenuID);
        //string f_status,
        DataSet PreviousSalQty_GetSubItemDetails(string CompID, string BrchID, string Item_id, string fromdate2, string todate2);
        DataSet SF_GetSubItemDetailsAftrApprove(string CompID, string BrchID, string Item_id, string Doc_no, string Doc_dt);
        DataSet CheckSalesForecastAgainstPP(string CompId, string BrchID, string financial_year, string Period, string FromDate, string ToDate);
    }
}
