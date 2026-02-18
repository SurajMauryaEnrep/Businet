using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BalanceSheet;
using EnRepMobileWeb.UTILITIES;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BalanceSheet
{
    public class BalanceSheet_Service: BalanceSheet_IService
    {
        public DataSet Get_BalSheet_Detail (string comp_id, string br_id, string br_list, string Fromdate, string Todate, string rpt_type)
        {
            try
            {
                DataSet GetBalSheetDetails = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@br_list",DbType.String,br_list),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String,Fromdate),
                     objProvider.CreateInitializedParameter("@to_dt",DbType.String,Todate),
                };
                GetBalSheetDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetBalanceSheet_Details", prmContentGetDetails);
                return GetBalSheetDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
