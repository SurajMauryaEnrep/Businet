using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.FinanceBudget;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.FinanceBudget
{
    public interface Financebudget_Iservices
    {
        DataTable GetFinYearList(string CompID, string brId,string flag);
        DataTable GetFinYearListpage(string CompID, string brId,string flag);
        DataTable GlList(string CompID);
        string FinBudDelete(string CompID, string BrId,FinanceBudgetModel financeBudgetModel);
        String InsertFinbudDetails(DataTable FinBudHeader, DataTable FinBugGlDetails, DataTable FinMonQtrDetails,DataTable FBCostCenterDetails);
        //,DataTable FinBugGlDetails, DataTable FinMonQtrDetails,DataTable FinCostCenter

        DataSet GetFinanBudList(string CompId, string BrId,string UserID,string DocumentMenuId,string WF_Status);
        DataSet GetFinBudgetDetail(string CompID, string BrId, string Finyear, string Revno,string UserID,string Period, string DocumentMenuId,string RevMess);

        DataSet SerachListFinBudget(string compId, string BrId, string Finyear, string Status);
        string ApproveFinanceBudgetDetails(string FY, string Revno, string FB_Date, string CompID, string BrchID, string UserID, string A_Status, string A_Level, string A_Remarks, string mac_id, string DocumentMenuId);
        //FY, RevNo, FB_Date, Comp_ID, BranchID, UserID, A_Status, A_Level, A_Remarks, mac_id, DocumentMenuId
        string FinanceBudRevised(string Revno,string Finyr,string Period,string Buddate,string CompID,string BrId,string UserID,string mac_id);

    }
}
