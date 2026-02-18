using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.MISOrderIntimation;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.MIS.MISOrderIntimation
{
    public class OrderIntimation_Service: OrderIntimation_IService
    {
        public DataSet GetOrderIntimationDetail(string comp_id, string br_id,string userid, string cust_id, string From_dt, string To_dt, string OrderType, string OrderNumber,string SalesPerson,string ItemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                 objProvider.CreateInitializedParameter("@userid",DbType.Int32,userid),
                 objProvider.CreateInitializedParameter("@cust_id",DbType.String,cust_id),
                 objProvider.CreateInitializedParameter("@From_dt",DbType.String,From_dt),
                 objProvider.CreateInitializedParameter("@To_dt",DbType.String,To_dt),
                 objProvider.CreateInitializedParameter("@OrderType",DbType.String,OrderType),
                 objProvider.CreateInitializedParameter("@OrderNumber",DbType.String,OrderNumber),
                 objProvider.CreateInitializedParameter("@SalesPerson",DbType.String,SalesPerson),
                 objProvider.CreateInitializedParameter("@ItemId",DbType.String,ItemId),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$MIS_Get$OrderIntimationDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetIntimationDetail(string CompID, string br_id,DataTable SodataTable)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameterTableType("@SodataTable",SqlDbType.Structured, SodataTable),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sls$MIS_Get$OrderIntimation]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetoOrderIntimationSONoList(string CompID, string BrchID, string Cust_id, string curr_Id,string doc_id, string From_dt, string To_dt, string OrderType)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Cust_id",DbType.String, Cust_id),
                    objProvider.CreateInitializedParameter("@Curr_Id",DbType.String, curr_Id),
                    objProvider.CreateInitializedParameter("@SONumber",DbType.String, ""),
                    objProvider.CreateInitializedParameter("@doc_id",DbType.String, doc_id),
                    objProvider.CreateInitializedParameter("@From_dt",DbType.Date, From_dt),
                    objProvider.CreateInitializedParameter("@To_dt",DbType.Date, To_dt),
                    objProvider.CreateInitializedParameter("@OrderType",DbType.String, OrderType),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetoOrderIntimationSalesOrderNumber]", prmContentGetDetails);
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable GetSalesPersonList(string compId, string brId, string userid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@SPersonName",DbType.String,"0"),
                 objProvider.CreateInitializedParameter("@BrchID",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@user_id",DbType.String,userid),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$sls$person$comp_GetSalesPersonList", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable BindGetItemList(string GroupName, string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetItemNameList", prmContentGetDetails).Tables[0];
                return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
    }
}
