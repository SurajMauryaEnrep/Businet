using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.DailyProductionPlan;
using EnRepMobileWeb.UTILITIES;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.MIS.DailyProductionPlan
{
    public class DailyProductionPlan_SERVICES: DailyProductionPlan_ISERVICES
    {
        public DataSet GetDailyProductPlanDetails(string compID, string brId, string fromDate, string toDate,string productId,string ddlOpId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,compID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@from_dt",DbType.String,fromDate),
                 objProvider.CreateInitializedParameter("@to_dt",DbType.String,toDate),
                 objProvider.CreateInitializedParameter("@product_id",DbType.String,productId),
                 objProvider.CreateInitializedParameter("@OpId",DbType.String,ddlOpId),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "DailyProductionDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDailyProductPlanPlannedQtyDetails(string compID, string brId, string planDt, string productId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,compID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@plan_dt",DbType.String,planDt),
                 objProvider.CreateInitializedParameter("@product_id",DbType.String,productId),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "DailyProductionPlannedQtyDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        public DataSet GetDailyProductPlanProducedQtyDetails(string compID, string brId, string producedDt, string productId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,compID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@produced_dt",DbType.String,producedDt),
                 objProvider.CreateInitializedParameter("@product_id",DbType.String,productId),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "DailyProductionProducedQtyDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetOperationNameList(int CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),                             };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$Bind$opname", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
