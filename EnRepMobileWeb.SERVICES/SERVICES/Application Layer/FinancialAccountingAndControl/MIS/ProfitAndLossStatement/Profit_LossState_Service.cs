using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.ProfitAndLossStatement;
using EnRepMobileWeb.UTILITIES;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.ProfitAndLossStatement
{
    public class Profit_LossState_Service:Profit_LossState_IService
    {
        public DataSet Get_PL_Statement(string comp_id, string br_id, string br_list, string Fromdate, string Todate, string rpt_type)
        {
            try
            {
                DataSet GetBankCashDetails = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@br_list",DbType.String,br_list),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String,Fromdate),
                     objProvider.CreateInitializedParameter("@to_dt",DbType.String,Todate),
                };
                    GetBankCashDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetPL_StateDetails", prmContentGetDetails);
                    return GetBankCashDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Get_FYList(string Compid, string Brid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Compid),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String,Brid),
                };
                DataSet Getfy_list = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$fy_GetList", prmContentGetDetails);
                return Getfy_list;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
