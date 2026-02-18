using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.ShopfloorTransferDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.MIS.ShopfloorTransferDetail_Service
{
    public class ShopfloorTransferDetail_Service : ShopfloorTransferDetail_IService
    {
        public DataTable GetShflTrfReport(string compId, string brId, string transactionType, string materialType,
            string itemId, string fromDate, string toDate, string itemGroupName, string shopfloorId, string status)
        {
            try
            {
                
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@TransferType",DbType.String,transactionType),
                 objProvider.CreateInitializedParameter("@Materialtype",DbType.String,materialType),
                 objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                 objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                 objProvider.CreateInitializedParameter("@ItemId",DbType.String,itemId),
                 objProvider.CreateInitializedParameter("@ItemGroup",DbType.String,itemGroupName),
                 objProvider.CreateInitializedParameter("@ShopFloor",DbType.String,shopfloorId),
                 objProvider.CreateInitializedParameter("@Status",DbType.String,status),
                 };
                return SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetShopfloorStockTransferMIS", prmContentGetDetails).Tables[0];
                
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetShflStatusList(string compId, string brId)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 };
                return SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetShflTrfStatusList", prmContentGetDetails).Tables[0];

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetShflStktrfBatchDetail(string compId, string brId, string trfNo, string trfDt, string itemId)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@Trfno",DbType.String,trfNo),
                 objProvider.CreateInitializedParameter("@Trfdt",DbType.String,trfDt),
                 objProvider.CreateInitializedParameter("@ItemId",DbType.String,itemId),
                 };
                return SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetShflStktrfBatchDetail", prmContentGetDetails).Tables[0];

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet TRF_GetSubItemDetails(string CompID, string Br_id, string ItemId, string trf_no, string trf_dt, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, trf_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, trf_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "STDetail_GetSubItemDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
