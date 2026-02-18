using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionConfirmation;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.ProductionConfirmation
{
    public class ProductionConfirmation_Service : ProductionConfirmation_IService
    {
        public DataSet GetOrderList(string Product_id, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@ProductID",DbType.String, Product_id),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$jc$detail_GetOrderList", prmContentGetDetails);
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
        public DataSet GetAllData(int CompID, int br_id, string UserID, string wfstatus, string DocumentMenuId, string SrcType, string ffy, string Period, string SearchName,
            string txtFromdate, string txtToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                     objProvider.CreateInitializedParameter("@UserID",DbType.Int32, UserID),
                     objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                     objProvider.CreateInitializedParameter("@srctype",DbType.String, SrcType),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String, ffy),
                    objProvider.CreateInitializedParameter("@period",DbType.String, Period),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, SearchName),
                    objProvider.CreateInitializedParameter("@Fromdate",DbType.String, txtFromdate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, txtToDate),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ppl$prdcnf$GetAllData", prmContentGetDetails);
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
        public DataSet GetOrderDetails(string CompId, string BrID, string OrderNo, string OrderDate,string Flag, string Shflid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@OrderNo",DbType.String, OrderNo),
                    objProvider.CreateInitializedParameter("@OrderDate",DbType.String, OrderDate),
                     objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                      objProvider.CreateInitializedParameter("@Shfl_Id",DbType.String, Shflid),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$jc$detail_GetOrderDetails", prmContentGetDetails);
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
        public DataSet getItemStockBatchWise(string CompId, string BranchId,string ItemId, string ShflID,string uom_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@ShflID",DbType.Int32, ShflID),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@uom_id",DbType.String,  uom_id),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$shfl$stk$detail_GetItemStockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string PCNo, string PCDate, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@PCNo",DbType.String, PCNo),
            objProvider.CreateInitializedParameter("@PCDate",DbType.String, PCDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$cnf$in_item$bt$detail_GetStockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockSerialWise(string CompId, string BranchId,string ItemId, string ShflId)
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
        public DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string PCNo, string PCDate, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@PCNo",DbType.String, PCNo),
            objProvider.CreateInitializedParameter("@PCDate",DbType.String, PCDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$cnf$in_item$sr$detail_GetStockSerialwise", prmContentGetDetails);
            return DS;
        }
        public DataSet Delete_ProductionConfirmation(string comp_id, string br_id, string Cnf_No, string Cnf_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                    objProvider.CreateInitializedParameter("@cnf_no",DbType.String, Cnf_No),
                    objProvider.CreateInitializedParameter("@cnf_dt",DbType.String,Cnf_Date),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$cnf$detail_Delete", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertUpdate_ProductionConfirmation(DataTable Cnf_HeaderDetails, DataTable Cnf_CnfItemDetails, DataTable Cnf_OutputItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable CnfAttachments,DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, Cnf_HeaderDetails ),
                 objprovider.CreateInitializedParameterTableType("@InputItemDetail",SqlDbType.Structured, Cnf_CnfItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@OutputItemDetail",SqlDbType.Structured, Cnf_OutputItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemBatchDetail",SqlDbType.Structured,ItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemSerialDetail",SqlDbType.Structured,ItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, CnfAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$prd$cnf$detail_InsertUpdate", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[6].Value != DBNull.Value) 
                {
                    DocNo = prmcontentaddupdate[6].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet Cancel_ProductionConfirmation(string CompID, string br_id, string Cnf_No, string Cnf_Date, string UserID,  string DocumentMenuId,  string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@MenuDocId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@cnf_no",DbType.String, Cnf_No),
                    objProvider.CreateInitializedParameter("@cnf_dt",DbType.Date, Cnf_Date),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$cnf$detail_Cancel", prmContentGetDetails);

            return DS;
        }
        public DataSet CheckQCAgainstCnf(string CompID, string br_id, string Cnf_No, string Cnf_Date)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@cnf_no",DbType.String, Cnf_No),
                    objProvider.CreateInitializedParameter("@cnf_dt",DbType.Date, Cnf_Date),
               };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_CheckQCAgainstCnf", prmContentGetDetails);

            return DS;
        }
        public string Approve_ProductionConfirmation(string comp_id, string br_id, string DocumentMenuID,string cnf_no, string cnf_date, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),//@menuid
                    objProvider.CreateInitializedParameter("@menuid",DbType.String, DocumentMenuID),
                    objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                    objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                    objProvider.CreateInitializedParameter("@cnf_no",DbType.String,cnf_no),
                    objProvider.CreateInitializedParameter("@cnf_dt",DbType.Date,  cnf_date),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@stkstatus",DbType.String,""),
                };
                prmContentGetDetails[10].Size = 100;
                prmContentGetDetails[10].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$prd$cnf$detail_Approve", prmContentGetDetails).ToString();

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
        public DataSet GetProductionConfirmationDetailByNo(string CompID, string BrchID, string Cnf_No, string Cnf_Date, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@cnf_no",DbType.String, Cnf_No),
                    objProvider.CreateInitializedParameter("@cnf_dt",DbType.String, Cnf_Date),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$cnf$detail_DetailByBNo", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetProductionConfirmationList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                         objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                         objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$cnf$detail_ProductionCnfList", prmContentGetDetails);
            return dt;
        }
        public DataTable GetProductionConfirmationFilter(string ProductionID, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId
            ,string OPID, string ShflID, string WSID, string ShftID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@ProductionID",DbType.String, ProductionID),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
                objProvider.CreateInitializedParameter("@OPID",DbType.String, OPID),
                objProvider.CreateInitializedParameter("@ShflID",DbType.String, ShflID),
                objProvider.CreateInitializedParameter("@WSID",DbType.String, WSID),
                objProvider.CreateInitializedParameter("@ShftID",DbType.String, ShftID),
                                                     };
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$cnf$detail_Filter", prmContentGetDetails).Tables[0];
            return dt;
        }
        public DataTable Bind_ProductList1(string CompID, string BrID, string SrcType, string ffy, string Period, string SearchName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@srctype",DbType.String, SrcType),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String, ffy),
                    objProvider.CreateInitializedParameter("@period",DbType.String, Period),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, SearchName),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "BOMAndPplan_GetProductList_forProAdvice", prmContentGetDetails).Tables[0];
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
        public DataSet PC_GetSubItemDetails(string CompID, string br_id, string Item_id,string Uom_id, string cnf_no, string cnf_dt,string Flag, string Shfl_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                     objProvider.CreateInitializedParameter("@Uom_id",DbType.String, Uom_id),
                      objProvider.CreateInitializedParameter("@cnf_no",DbType.String, cnf_no),
                      objProvider.CreateInitializedParameter("@cnf_dt",DbType.String, cnf_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                      objProvider.CreateInitializedParameter("@Shfl_id",DbType.String, Shfl_id),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[PC_GetSubItemDetailsAfterApprove]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetProductionConfirmationPrintDeatils(string CompID, string br_id, string cnf_no, string cnf_dt)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                      objProvider.CreateInitializedParameter("@cnf_no",DbType.String, cnf_no),
                      objProvider.CreateInitializedParameter("@cnf_dt",DbType.String, cnf_dt),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetProductionConfirmationPrintDetails]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetConsumeSubItemShflAvlstockDetails(string CompID, string BrID, string Item_id,string Uom_id, String Shfl_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@Uom_id",DbType.String, Uom_id),
                    objProvider.CreateInitializedParameter("@Shfl_id",DbType.String, Shfl_id),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetSfhlSubItemAvlStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetConsumeSubItemDetails(string CompID, string BrID, string Item_id, string Shfl_id, string Doc_no, string Doc_dt,string Uom_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@Shfl_id",DbType.String, Shfl_id),
                    objProvider.CreateInitializedParameter("@Prod_no",DbType.String, Doc_no),
                      objProvider.CreateInitializedParameter("@Prod_dt",DbType.String, Doc_dt),
                      objProvider.CreateInitializedParameter("@Uom_id",DbType.String, Uom_id)
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSubItemDetailwhithProductionOrder", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetQcDetail(string CompId, string BranchId, string ItemId,string UOMId, string cnf_no, string cnf_dt)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32, CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String, BranchId),
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@uom_id",DbType.Int64, UOMId),
            objProvider.CreateInitializedParameter("@cnf_no",DbType.String, cnf_no),
            objProvider.CreateInitializedParameter("@cnf_dt",DbType.String,  cnf_dt),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetItemWiseQcDetail", prmContentGetDetails);
            return DS;
        }
        public DataSet GetAllData(int CompID, int br_id, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                     objProvider.CreateInitializedParameter("@UserID",DbType.Int32, UserID),
                     objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ppl$jc$GetAllData", prmContentGetDetails);
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
