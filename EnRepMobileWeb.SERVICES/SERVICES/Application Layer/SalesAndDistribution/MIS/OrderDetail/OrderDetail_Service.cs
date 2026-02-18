using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.OrderDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.MIS.OrderDetail
{
    public class OrderDetail_Service : OrderDetail_IService
    {
        public DataTable GetCurrencyList(string compId, string currencyId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@CurrId",DbType.String,currencyId),
                 
                 };
                DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Get_stp$curr", prmContentGetDetails).Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetOrder_Detail(string CompID, string BrID,string userid, string cust_id, string reg_name, string sale_type, string curr_id, string productGrp, string Product_Id, string productPort, string custCat, string CustPort, string inv_no, string inv_dt, string sale_per, string From_dt, string To_dt, string Flag, string custzone, string custgroup, string custstate, string custcity)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@userid",DbType.Int32,userid),
                 objProvider.CreateInitializedParameter("@cust_id",DbType.String,cust_id),
                 objProvider.CreateInitializedParameter("@reg_name",DbType.String,reg_name),
                 objProvider.CreateInitializedParameter("@sale_type",DbType.String,sale_type),
                 objProvider.CreateInitializedParameter("@curr_id",DbType.String,curr_id),
                 objProvider.CreateInitializedParameter("@productGrp",DbType.String,productGrp),
                 objProvider.CreateInitializedParameter("@productPort",DbType.String,productPort),
                 objProvider.CreateInitializedParameter("@Product_Id",DbType.String,Product_Id),
                 objProvider.CreateInitializedParameter("@custCat",DbType.String,custCat),
                 objProvider.CreateInitializedParameter("@CustPort",DbType.String,CustPort),
                 objProvider.CreateInitializedParameter("@so_no",DbType.String,inv_no),
                 objProvider.CreateInitializedParameter("@so_dt",DbType.String,inv_dt),
                 objProvider.CreateInitializedParameter("@sale_per",DbType.String,sale_per),
                 objProvider.CreateInitializedParameter("@from_dt",DbType.String,From_dt),
                 objProvider.CreateInitializedParameter("@to_dt",DbType.String,To_dt),
                 objProvider.CreateInitializedParameter("@Action",DbType.String,Flag),
                 objProvider.CreateInitializedParameter("@custzone",DbType.String,custzone),
                 objProvider.CreateInitializedParameter("@custgroup",DbType.String,custgroup),
                 objProvider.CreateInitializedParameter("@cust_state",DbType.String,custstate),
                 objProvider.CreateInitializedParameter("@cust_city",DbType.String,custcity),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$MIS_Get$SalesOrderDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetSONumberList(string compId, string branchId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,branchId),

                 };
                DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSONumberList", prmContentGetDetails).Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
