using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.OpeningBalance;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.OpeningBalance
{
   public interface OpeningBalance_ISERVICES
    {
        DataTable Getcoa(string CompID,string BrchID, int acc_type, string Transtype);
        DataSet GetAccGroup(string acc_id, string CompId);
        DataSet GetOpeningDate(int CompID, int BrID);
        DataTable GetCurrList(string CompID);
        DataSet GetCurrConvRate(string curr_id, string CompId);
        String InsertOpeningBalanceDetail(DataTable OpeningBalanceHeader, DataTable OpeningBalanceBillWiseDetail);
        DataTable GetOpeningBalDetailList(string CompID, string BrchID, string FinYear,string searchValue);
        DataSet GetBillwiseOpeningDetail(string CompID, string BrchID, string AccId, string FinYear);
        DataSet GetBillwiseOpeningDetail1(string CompID, string BrchID, string AccId, string FinYear);
        DataSet OpeningBalanceDelete(string Acc_id, string Fin_year,string comp_id, string br_id);
        DataTable GetOpeningBalFinYearList(string CompID, string BrchID);
        DataSet GetOpeningBalanceDeatils(string CompID, string BrchID, string FinYear);
        DataSet GetVerifiedDataOfExcel(DataTable OPBalDetail, DataTable BillDetail, string compId, string BrchID);
        DataTable ShowExcelErrorDetail(DataTable OPBalDetail, DataTable BillDetail, string compId,string BrchID);
        string BulkImportOPBalDetail(DataTable OPBalDetail, DataTable BillDetail,string BrID,string UserID,string compId);
        DataSet GetMasterDataOPBal(string CompID, string BrchID);
        DataTable getSalesPersonList(string CompID,string br_id, string userid);
    }
}
