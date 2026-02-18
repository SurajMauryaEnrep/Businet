using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.MISCostCenterAnalysis;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.MISCostCenterAnalysis
{
    public class MISCostCenterAnalysis_Service: MISCostCenterAnalysis_IService
    {
        public DataSet GetCCExpRevPageLoadDetail(string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                };
                DataSet Ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MIS_CostCenterAnalysisPageLoad", prmContentGetDetails);
                return Ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetExpanceAndRevanueDs(string comp_id, string br_id, string cc_id, string cc_val_id, string from_dt, string to_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String,from_dt),
                     objProvider.CreateInitializedParameter("@to_dt",DbType.String,to_dt),
                     objProvider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                     objProvider.CreateInitializedParameter("@cc_val_id",DbType.String,cc_val_id),
                };
                DataSet Ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MIS_CostCenterAnalysis", prmContentGetDetails);
                return Ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCostCenterTransactionDetails(string comp_id, string br_id, string cc_id, string cc_val_id, string from_dt, string to_dt,string acc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String,from_dt),
                     objProvider.CreateInitializedParameter("@to_dt",DbType.String,to_dt),
                     objProvider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                     objProvider.CreateInitializedParameter("@cc_val_id",DbType.String,cc_val_id),
                     objProvider.CreateInitializedParameter("@acc_id",DbType.String,acc_id),
                };
                DataSet Ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MIS_CostCenterTransactionDetails", prmContentGetDetails);
                return Ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
       
        public DataSet GetCostCenterValueListByCostCenter(string comp_id, string br_id, string cc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                };
                DataSet Ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MIS_CostCenterValueListByCostCenter", prmContentGetDetails);
                return Ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
