using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.ConsumptionAnalysis;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.MIS.ConsumptionAnalysis
{
    public class ConsumptionAnalysis_SERVICES: ConsumptionAnalysis_ISERVICES
    {
        public DataSet GetConsumption_Details(string CompID, string BrID, string ProductID, string From_dt, string To_dt, string Group, string Flag, string shflId, string opId)
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
                 objProvider.CreateInitializedParameter("@Group",DbType.String,Group),
                 objProvider.CreateInitializedParameter("@Flag",DbType.String,Flag),
                 objProvider.CreateInitializedParameter("@opId",DbType.String,opId),
                 objProvider.CreateInitializedParameter("@ShflId",DbType.String,shflId),

                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$mis_GetConsumption_Details", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetLotDetail(string CompID, string BrID, string cnf_no, string cnf_dt, string Product_Id, string Material_item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@cnf_no",DbType.String,cnf_no),
                 objProvider.CreateInitializedParameter("@cnf_dt",DbType.String,cnf_dt),
                 objProvider.CreateInitializedParameter("@Product_Id",DbType.String,Product_Id),
                 objProvider.CreateInitializedParameter("@Material_item_id",DbType.String,Material_item_id),
                 //objProvider.CreateInitializedParameter("@Flag",DbType.String,Flag),

                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$mis_GetConsumptionLot_Details", prmContentGetDetails);
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
        public DataTable ItemGroupListDAL(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataTable PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$grp_GetAllItemGroupNoodChilds_ItemList", prmContentGetDetails).Tables[0];
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            
        }

    }
}
