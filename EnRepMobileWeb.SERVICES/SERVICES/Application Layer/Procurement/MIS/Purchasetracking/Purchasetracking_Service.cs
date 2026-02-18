using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.PurchaseTracking;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.MIS.Purchasetracking
{
    public class Purchasetracking_Service : Purchasetracking_IService
    {
        public DataTable GetOrderNumberList(string compId, string brId, string orderType, string suppId, string currId, string SuppCat, string SuppPort,string SearchName)
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
                 objProvider.CreateInitializedParameter("@SuppId",DbType.String,suppId),
                 objProvider.CreateInitializedParameter("@CurrId",DbType.String,currId),
                 objProvider.CreateInitializedParameter("@Search",DbType.String,SearchName),
                 objProvider.CreateInitializedParameter("@cat_id",DbType.String,SuppCat),
                 objProvider.CreateInitializedParameter("@port_id",DbType.String,SuppPort),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetApprovedPOList", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetPoTrackingDetailsMIS(string compId, string brId, string poNo, string suppId, string orderType, string itemId, string currId, 
            string fromDate, string toDate, string ItemType, string NotFillterOrderType,int skip ,int pageSize,string searchValue
            ,string sortColumn,string sortColumnDir, string SuppCat, string SuppPort, string flag,string bridlist)
        {
            try
            {
                if (poNo.ToUpper().Contains("ALL"))
                    poNo = "0";
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@PoNo",DbType.String,poNo),
                 objProvider.CreateInitializedParameter("@SuppId",DbType.String,suppId),
                 objProvider.CreateInitializedParameter("@OrderType",DbType.String,orderType),
                 objProvider.CreateInitializedParameter("@ItemId",DbType.String,itemId),
                 objProvider.CreateInitializedParameter("@CurrId",DbType.String,currId),
                 objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                 objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                 objProvider.CreateInitializedParameter("@ItemType",DbType.String,ItemType),
                 objProvider.CreateInitializedParameter("@NotFillterOrderType",DbType.String,NotFillterOrderType),
                 objProvider.CreateInitializedParameter("@Skip",DbType.String,skip),
                 objProvider.CreateInitializedParameter("@PageSize",DbType.String,pageSize),
                 objProvider.CreateInitializedParameter("@Search",DbType.String,searchValue),
                 objProvider.CreateInitializedParameter("@sortColumn",DbType.String,sortColumn),
                 objProvider.CreateInitializedParameter("@sortColumnDir",DbType.String,sortColumnDir),
                 objProvider.CreateInitializedParameter("@Flag",DbType.String,flag),
                 objProvider.CreateInitializedParameter("@cat_id",DbType.String,SuppCat),
                 objProvider.CreateInitializedParameter("@port_id",DbType.String,SuppPort),
                 objProvider.CreateInitializedParameter("@brid_list",DbType.String,bridlist)
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetPoTrackingMISReport", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
