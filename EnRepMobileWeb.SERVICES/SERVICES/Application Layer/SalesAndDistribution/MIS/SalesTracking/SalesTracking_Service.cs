using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.SalesTracking;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.MIS.SalesTracking
{
    public class SalesTracking_Service : SalesTracking_IService
    {
        public DataTable GetPONumberList(string compId, string brId, string orderType, string suppId, string currId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@OrderType",DbType.String,orderType),
                 objProvider.CreateInitializedParameter("@CustId",DbType.String,suppId),
                 objProvider.CreateInitializedParameter("@CurrId",DbType.String,currId),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetApprovedSOList", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetSalesPersonList(string compId, string brId,string userid)
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

        public DataTable GetSalesTrackingList(string compId, string brId,string userid, string orderno, string custId, string slsPersId, string currId, string orderType, string itemId, string fromDate, string toDate,string NotFillterOrderType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@userid",DbType.Int32,userid),
                 objProvider.CreateInitializedParameter("@SoNo",DbType.String,orderno),
                 objProvider.CreateInitializedParameter("@CustId",DbType.String,custId),
                 objProvider.CreateInitializedParameter("@OrderType",DbType.String,orderType),
                 objProvider.CreateInitializedParameter("@ItemId",DbType.String,itemId),
                 objProvider.CreateInitializedParameter("@CurrId",DbType.String,currId),
                 objProvider.CreateInitializedParameter("@SlsPersId",DbType.String,slsPersId),
                 objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                 objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                 objProvider.CreateInitializedParameter("@NotFillterOrderType",DbType.String,NotFillterOrderType)
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSoTrackingMISReport", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesTrackingList(string compId, string brId,string userid, string orderno, string custId, string slsPersId, string currId, string orderType, string itemId,
            string fromDate, string toDate, string NotFillterOrderType, string custCat, string CustPort, string custzone, string custgroup, string custstate, string custcity,string brlist,
            string skip, string pageSize, string searchValue, string sortColumn, string sortColumnDir, string Flag)
        
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@userid",DbType.Int32,userid),
                 objProvider.CreateInitializedParameter("@SoNo",DbType.String,orderno),
                 objProvider.CreateInitializedParameter("@CustId",DbType.String,custId),
                 objProvider.CreateInitializedParameter("@OrderType",DbType.String,orderType),
                 objProvider.CreateInitializedParameter("@ItemId",DbType.String,itemId),
                 objProvider.CreateInitializedParameter("@CurrId",DbType.String,currId),
                 objProvider.CreateInitializedParameter("@SlsPersId",DbType.String,slsPersId),
                 objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                 objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                 objProvider.CreateInitializedParameter("@NotFillterOrderType",DbType.String,NotFillterOrderType),
                 objProvider.CreateInitializedParameter("@Skip",DbType.String,skip),
                 objProvider.CreateInitializedParameter("@PageSize",DbType.String,pageSize),
                 objProvider.CreateInitializedParameter("@Search",DbType.String,searchValue),
                 objProvider.CreateInitializedParameter("@sortColumn",DbType.String,sortColumn),
                 objProvider.CreateInitializedParameter("@sortColumnDir",DbType.String,sortColumnDir),
                 objProvider.CreateInitializedParameter("@Flag",DbType.String,Flag),
                 objProvider.CreateInitializedParameter("@custCat",DbType.String,custCat),
                 objProvider.CreateInitializedParameter("@CustPort",DbType.String,CustPort),
                 objProvider.CreateInitializedParameter("@custzone",DbType.String,custzone),
                 objProvider.CreateInitializedParameter("@custgroup",DbType.String,custgroup),
                 objProvider.CreateInitializedParameter("@cust_state",DbType.String,custstate),
                 objProvider.CreateInitializedParameter("@cust_city",DbType.String,custcity),
                  objProvider.CreateInitializedParameter("@brlist",DbType.String,brlist)
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSoTrackingMISReport", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
