using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.ProductionAnalysis;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.MIS.ProductionAnalysis
{
    public class ProductionAnalysis_SERVICES: ProductionAnalysis_ISERVICES
    {
        public DataSet GetProductionMIS_Details(string CompID, string BrID, string ProductID, string From_dt, string To_dt, string ShowAs, string shflId, string opId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@product_id",DbType.String,ProductID),
                 objProvider.CreateInitializedParameter("@from_dt",DbType.String,From_dt),
                 objProvider.CreateInitializedParameter("@to_dt",DbType.String,To_dt),
                 objProvider.CreateInitializedParameter("@ShowAs",DbType.String,ShowAs),
                 objProvider.CreateInitializedParameter("@shflId",DbType.String,shflId),
                 objProvider.CreateInitializedParameter("@OpId",DbType.String,opId),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$mis_GetProductionMIS_Details", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetProductionMIS_DetailsInfo(string CompID, string BrID, string ProductID, string From_dt, string To_dt, string ShowAs, string shflId, string opId,string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@product_id",DbType.String,ProductID),
                 objProvider.CreateInitializedParameter("@from_dt",DbType.String,From_dt),
                 objProvider.CreateInitializedParameter("@to_dt",DbType.String,To_dt),
                 objProvider.CreateInitializedParameter("@ShowAs",DbType.String,ShowAs),
                 objProvider.CreateInitializedParameter("@shflId",DbType.String,shflId),
                 objProvider.CreateInitializedParameter("@OpId",DbType.String,opId),
                 objProvider.CreateInitializedParameter("@flag",DbType.String,flag),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$mis_GetProductionMIS_DetailsOnClickInfo", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataSet BindProductNameInDDL(string CompID, string BrID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
        //            objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
        //                                             };
        //        DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$MIS$ProductionAnalysisProductName", prmContentGetDetails);
        //        return ds;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet GetProductionMIS_EstimateAndActualValueDetails(string CompID, string BrID, string ProductID,string ProduceQty,
            string JcNo, string JcDt, string Cnf_no, string Cnf_dt,string From_dt,string To_dt,string Flag, string shflId, string opId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@product_id",DbType.String,ProductID),
                 objProvider.CreateInitializedParameter("@ProduceQty",DbType.String,ProduceQty),
                 objProvider.CreateInitializedParameter("@jc_no",DbType.String,JcNo),
                 objProvider.CreateInitializedParameter("@jc_dt",DbType.String,JcDt),
                  objProvider.CreateInitializedParameter("@Cnf_no",DbType.String,Cnf_no),
                 objProvider.CreateInitializedParameter("@Cnf_dt",DbType.String,Cnf_dt),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String,From_dt),
                 objProvider.CreateInitializedParameter("@to_dt",DbType.String,To_dt),
                 objProvider.CreateInitializedParameter("@Flag",DbType.String,Flag),
                 objProvider.CreateInitializedParameter("@shflId",DbType.String,shflId),
                 objProvider.CreateInitializedParameter("@OpId",DbType.String,opId),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$mis_GetEstimatedCostDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetItemQCParamDetail(string CompID, string br_id, string ItemID, string qc_no, string qc_dt)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                     objProvider.CreateInitializedParameter("@qc_no",DbType.String, qc_no),
                     objProvider.CreateInitializedParameter("@qc_dt",DbType.String, qc_dt),
                     objProvider.CreateInitializedParameter("@status",DbType.String, "A"),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetitemQCParamDetail]", prmContentGetDetails);
            return ds;
        }
    }
}
