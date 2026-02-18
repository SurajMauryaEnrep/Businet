using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.MISOrderDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.MIS.MISOrderDetail
{
    public class MISOrderDetail_Service : MISOrderDetail_IService
    {
        public DataSet GetMISOrderDetail(string compId,string brId,string fromDate, string toDate, string showAs, string suppId, 
            string itemId, string currId, string srctype, string orderType, string Status, string poNo, string poDate, string SuppCat, string SuppPort,string brid_list)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int32, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@FromDate",DbType.String, fromDate),
                    objProvider.CreateInitializedParameter("@ToDate",DbType.String, toDate),
                    objProvider.CreateInitializedParameter("@ShowAs",DbType.String, showAs),
                    objProvider.CreateInitializedParameter("@SuppId",DbType.String, suppId),
                    objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                    objProvider.CreateInitializedParameter("@CurrId",DbType.String, currId),
                    objProvider.CreateInitializedParameter("@SrcType",DbType.String, srctype),
                    objProvider.CreateInitializedParameter("@OrderType",DbType.String, orderType),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@PoNo",DbType.String, poNo),
                    objProvider.CreateInitializedParameter("@PoDate",DbType.String, poDate),
                    objProvider.CreateInitializedParameter("@cat_id",DbType.String, SuppCat),
                    objProvider.CreateInitializedParameter("@port_id",DbType.String, SuppPort),
                    objProvider.CreateInitializedParameter("@brid_list",DbType.String, brid_list),
                                                    };
                
                DataSet Getcategory = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetMISOrderDetails", prmContentGetDetails);
                return Getcategory;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllDDLData(string Comp_ID, string Br_ID, string SupplierName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,Br_ID),
                 objProvider.CreateInitializedParameter("@SuppName",DbType.String,SupplierName),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_$MIS_GetSupplierDropdownListData", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetcategoryPortfolioDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataSet Getcategory = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$supp$categoryPortfolio", prmContentGetDetails);
                return Getcategory;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
