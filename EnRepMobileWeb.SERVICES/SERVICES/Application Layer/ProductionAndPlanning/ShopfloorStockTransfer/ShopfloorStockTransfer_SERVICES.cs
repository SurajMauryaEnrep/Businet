using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ShopfloorStockTransfer;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.ShopfloorStockTransfer
{
    public class ShopfloorStockTransfer_SERVICES: ShopfloorStockTransfer_ISERVICES
    {
        public DataSet Delete_ShopfloorStockTransfer(string comp_id, string br_id, string trf_No, string trf_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                    objProvider.CreateInitializedParameter("@trf_no",DbType.String, trf_No),
                    objProvider.CreateInitializedParameter("@trf_dt",DbType.String,trf_Date),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$shfl$trf$detail_Delete", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approve_ShopfloorStockTransfer(string comp_id, string br_id, string DocumentMenuID, string trf_no, string trf_date, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),//@menuid
                    objProvider.CreateInitializedParameter("@menuid",DbType.String, DocumentMenuID),
                    objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                    objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                    objProvider.CreateInitializedParameter("@trf_no",DbType.String,trf_no),
                    objProvider.CreateInitializedParameter("@trf_dt",DbType.String,  trf_date),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@stkstatus",DbType.String,""),
                };
                prmContentGetDetails[10].Size = 100;
                prmContentGetDetails[10].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$shfl$trf$detail_Approve", prmContentGetDetails).ToString();

                string msg = string.Empty;
                if (prmContentGetDetails[10].Value != DBNull.Value) // status
                {
                    msg = prmContentGetDetails[10].Value.ToString();
                }

                return msg;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSourceAndDestinationList(string CompId, string BrID, string TransferType,string MaterialType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/   
                objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompId),
                objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                objProvider.CreateInitializedParameter("@TransferType",DbType.String, TransferType),      
                objProvider.CreateInitializedParameter("@MaterialType",DbType.String, MaterialType),      
                };
                DataSet GetDT = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSourceAndDestinitionShopFloorStockTransferList", prmContentGetDetails);
                return GetDT;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public DataSet getItemStockBatchWise(string ItemId, string ShflID, string CompId, string BranchId,string MaterialType)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@ShflID",DbType.Int32, ShflID),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@trans_type",DbType.String,  MaterialType),

            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$shfl$stk$detail_GetItemStockBatchwise_shfl$stk$trf]", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemStockBatchWiseAfterSavedDetail(string ItemId, string trf_no, string trf_dt, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            
            objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@trf_no",DbType.String, trf_no),
            objProvider.CreateInitializedParameter("@trf_dt",DbType.String, trf_dt),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),

            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$shfl$stk$detail_GetItemStockBatchwise_afterSaveDT]", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string ShflId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@ShflId",DbType.Int32, ShflId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$shfl$stk$detail_GetItemStockSerialwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockSerialWiseAfterSavedDetail(string CompId, string BranchId, string trf_no, string trf_dt)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@trf_no",DbType.String, trf_no),
            objProvider.CreateInitializedParameter("@trf_dt",DbType.String, trf_dt),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$shfl$stk$detail_GetItemStockSerialwise_afterSaveDT", prmContentGetDetails);
            return DS;
        }
        public DataSet GetSHFLTransferList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId,
            string FromDate, string ToDate)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                         objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                         objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                         objProvider.CreateInitializedParameter("@Fromdate",DbType.String, FromDate),
                         objProvider.CreateInitializedParameter("@Todate",DbType.String, ToDate),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$shfl$trf$details_List", prmContentGetDetails);
            return dt;
        }
        public DataTable GetSHFLTransferListFiltered(string transferType, string materialType, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/ 
                objProvider.CreateInitializedParameter("@transferType",DbType.String, transferType),
                objProvider.CreateInitializedParameter("@materialType",DbType.String, materialType),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
                                                     };
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$shfl$trf$detail_Filter", prmContentGetDetails).Tables[0];
            return dt;
        }
        public DataSet GetShopfloorStockTransferDetailByNo(string CompID, string BrchID, string trf_No, string trf_Date, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@trf_no",DbType.String, trf_No),
                    objProvider.CreateInitializedParameter("@trf_dt",DbType.String, trf_Date),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$shfl$trf$detail_DetailByBNo", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertUpdate_ShopfloorStockTransfer(DataTable trff_HeaderDetails, DataTable trf_ItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails,DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, trff_HeaderDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, trf_ItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemBatchDetail",SqlDbType.Structured,ItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemSerialDetail",SqlDbType.Structured,ItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$shfl$trf$detail_InsertUpdate", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value)
                {
                    DocNo = prmcontentaddupdate[4].Value.ToString();
                }
                return DocNo;
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "TRF_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetShflStkTrfDeatilsForPrint(string CompID, string BrchID, string TRFNo, string TRFDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@TRF_No",DbType.String, TRFNo),
                     objProvider.CreateInitializedParameter("@TRF_Dt",DbType.String, TRFDate),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetShflStkTrfDeatils_ForPrint", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
    }
}
