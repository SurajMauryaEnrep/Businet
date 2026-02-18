using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BudgetAnalysis;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BudgetAnalysis
{
    public class BudgetAnalysis_Service : BudgerAnalysis_IService
    {
        public DataSet GetBudgerAnalysisReport(string action,string compId, string brId, string glAccId, string finYear, string quarter, string month)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Action",DbType.String,action),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                     objProvider.CreateInitializedParameter("@GlAccount",DbType.String,glAccId),
                     objProvider.CreateInitializedParameter("@FinYear",DbType.String,finYear),
                     objProvider.CreateInitializedParameter("@Quarter",DbType.String,quarter),
                     objProvider.CreateInitializedParameter("@Month",DbType.String,month),
                };
                DataSet GetCurr_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetBudgetAnalysisReport", prmContentGetDetails);
                return GetCurr_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetBudgetAllocationReport(string action, string compId, string brId, string glAccId, string finYear, string qtr, string mnth,int rev_no)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Action",DbType.String,action),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                     objProvider.CreateInitializedParameter("@AccId",DbType.String,glAccId),
                     objProvider.CreateInitializedParameter("@FinYear",DbType.String,finYear),
                     objProvider.CreateInitializedParameter("@Quarter",DbType.String,qtr),
                     objProvider.CreateInitializedParameter("@Month",DbType.String,mnth),
                     objProvider.CreateInitializedParameter("@Rev_no",DbType.String,rev_no)
                };
                DataSet GetCurr_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetBudgetAllocationByAccountId", prmContentGetDetails);
                return GetCurr_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetFinancialYearsList(string compId, string brId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                };
                DataTable GetCurr_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetFinancialYears", prmContentGetDetails).Tables[0];
                return GetCurr_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetfyLedgerDetails(string compId, string brId, string accId, string fromDate, string toDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                     objProvider.CreateInitializedParameter("@AccId",DbType.String,accId),
                     objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                     objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                };
                DataTable GetCurr_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetGlAccLedgerDetails", prmContentGetDetails).Tables[0];
                return GetCurr_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetfyLedgerDetailsCostCentr(string compId, string brId, string accId, string fromDate, string toDate, int CCtypeId, int CCNameId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                     objProvider.CreateInitializedParameter("@AccId",DbType.String,accId),
                     objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                     objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                     objProvider.CreateInitializedParameter("@CCtypeId",DbType.Int32,CCtypeId),
                     objProvider.CreateInitializedParameter("@CCNameId",DbType.Int32,CCNameId),
                };
                DataTable GetCurr_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetGlAccLedgerDetailsCostCentr", prmContentGetDetails).Tables[0];
                return GetCurr_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetFyMonthOrQuarterList(string action, string compId, string brId, string budgetType, string finYear, string qtrName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Action",DbType.String,action),
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                     objProvider.CreateInitializedParameter("@BgtType",DbType.String,budgetType),
                     objProvider.CreateInitializedParameter("@FinYear",DbType.String,finYear),
                     objProvider.CreateInitializedParameter("@QtrName",DbType.String,qtrName),
                };
                DataTable GetCurr_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetFYQuarterOrMonthList", prmContentGetDetails).Tables[0];
                return GetCurr_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
