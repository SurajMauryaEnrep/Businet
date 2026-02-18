using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.FinishedGoodsReceipt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialReceipt.FinishedGoodsReceipt
{
    public class FinishedGoodsReceipt_SERVICE : FinishedGoodsReceipt_ISERVICE
    {
        public DataSet ItemList(string GroupName, string CompID, string BrchID)
        {
          //  Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "FGR$Bind$productName", prmContentGetDetails);
                
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        public DataSet OperationData(string GroupName, string CompID, string BrchID)
        {
          //  Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Fgr$Get$op$name", prmContentGetDetails);
                
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        public DataSet GetBomDeatilData(string itemID, string operation_ID, string shop_floor, string CompID, string brnchID,string flag)
        {
          //  Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@itemID",DbType.String, itemID),
                    objProvider.CreateInitializedParameter("@operation_ID",DbType.String, operation_ID),
                    objProvider.CreateInitializedParameter("@shop_floor",DbType.String, shop_floor),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, brnchID),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Fgr$Get$BillofMaterial", prmContentGetDetails);
                
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        public DataTable ItemListDeatil(string GroupName, string CompID, string BrchID)
        {
           // Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                     };

                DataTable PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "FGR$Item$DropDown$Bind", prmContentGetDetails).Tables[0];
              
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        public DataSet GetAllDropDownList(string comid,string brid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                objProvider.CreateInitializedParameter("@Compid",DbType.String,comid),
                objProvider.CreateInitializedParameter("@brid", DbType.String, brid),
                 };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$All$DropDownList$finish$Good$Recipt", prmContentGetDetails);

                return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {

            }          
        } 
        public DataSet GetConsumeSubItemShflAvlstockDetails(string CompID, string BrID, string Item_id, string Uom_id, string Shfl_id)
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
        public DataSet GetOutputDeatilSubitm(string CompID, string BrID, string Item_id, string Uom_id, string shfl_id, string rcpt_no, string rcpt_dt,string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@Uom_id",DbType.String, Uom_id),
                    objProvider.CreateInitializedParameter("@Shfl_id",DbType.String, shfl_id),
                    objProvider.CreateInitializedParameter("@rcpt_no",DbType.String, rcpt_no),
                    objProvider.CreateInitializedParameter("@rcpt_dt",DbType.String, rcpt_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$subitem$fgr$input$output", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemStockBatchWise(string CompId, string BranchId, string ItemId, string ShflID, string uom_id)
        {
            try
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
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string FGR_No, string FGR_dt, string ItemID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@FGR_No",DbType.String, FGR_No),
            objProvider.CreateInitializedParameter("@FGR_dt",DbType.String, FGR_dt),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$fgr$in_item$bt$detail_GetStockBatchwise", prmContentGetDetails);
            return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }  
        public DataSet GetAvlStock(string CompID, string brnchID, string itemid, string shopflore)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@BrID",DbType.String, brnchID),
            objProvider.CreateInitializedParameter("@Item_id",DbType.String, itemid),
            objProvider.CreateInitializedParameter("@Shfl_id",DbType.String, shopflore),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetSfhlItemAvlStockDetails", prmContentGetDetails);
            return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertUpdate_FinishGoodRecipt(DataTable FGRHeader, DataTable InputFGRItemDetails, DataTable OutputItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable OutputdtSubItem, DataTable InputdtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, FGRHeader ),
                 objprovider.CreateInitializedParameterTableType("@InputItemDetail",SqlDbType.Structured, InputFGRItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@OutputItemDetail",SqlDbType.Structured, OutputItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemBatchDetail",SqlDbType.Structured,ItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemSerialDetail",SqlDbType.Structured,ItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, OutputdtSubItem ),
                 objprovider.CreateInitializedParameterTableType("@InputdtSubItem",SqlDbType.Structured, InputdtSubItem ),
                };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$prd$FGR$detail_InsertUpdate", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[5].Value != DBNull.Value)
                {
                    DocNo = prmcontentaddupdate[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet GetFGRDeatilData(string CompID, string BrchID, string rcpt_no, string rcpt_dt, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@rcpt_no",DbType.String, rcpt_no),
                    objProvider.CreateInitializedParameter("@rcpt_dt",DbType.String, rcpt_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$fgr$detail_DetailByBNo", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string ShflId)
        {
            try
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
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string rcpt_no, string rcpt_dt, string ItemID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@rcpt_no",DbType.String, rcpt_no),
            objProvider.CreateInitializedParameter("@rcpt_dt",DbType.String, rcpt_dt),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$fgr$in_item$sr$detail_GetStockSerialwise", prmContentGetDetails);
                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetFGRList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId,
           string FromDate, string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                         objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                         objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                         objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                         objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                                                     };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$fgr$detail_FGRList", prmContentGetDetails);
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }    
        public DataSet Delete_FinishGoodsRecipt(string comp_id, string br_id, string rcpt_no, string rcpt_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                    objProvider.CreateInitializedParameter("@rcpt_no",DbType.String, rcpt_no),
                    objProvider.CreateInitializedParameter("@rcpt_dt",DbType.String,rcpt_dt),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[ppl$prd$fgr$detail_Delete]", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approve_FinishedGoodsReceipt(string comp_id, string br_id, string DocumentMenuID, string rcpt_no, string rcpt_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks)
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
                    objProvider.CreateInitializedParameter("@rcpt_no",DbType.String,rcpt_no),
                    objProvider.CreateInitializedParameter("@rcpt_dt",DbType.Date,  rcpt_dt),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@stkstatus",DbType.String,""),
                };
                prmContentGetDetails[10].Size = 100;
                prmContentGetDetails[10].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$prd$fgr$detail_Approve", prmContentGetDetails).ToString();

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
        public DataTable SearchDataFilter(string shopfloreid, string opertionid, string Fromdate, string Todate, string Status, 
            string CompID, string BrchID, string DocumentMenuId, string Source_type, string Item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@shopfloreid",DbType.String, shopfloreid),
                objProvider.CreateInitializedParameter("@opertionid",DbType.String, opertionid),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
                objProvider.CreateInitializedParameter("@Source_type",DbType.String, Source_type),
                objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                                                     };
                DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$fgr$detail_Filter", prmContentGetDetails).Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Cancel_FinishGoodsReceipt(string CompID, string br_id, string Rcpt_no, string rcpt_Date, string UserID, string DocumentMenuId, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@MenuDocId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@rcpt_no",DbType.String, Rcpt_no),
                    objProvider.CreateInitializedParameter("@rcpt_dt",DbType.Date, rcpt_Date),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$fgr$detail_Cancel", prmContentGetDetails);

                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetProductionConfirmationPrintDeatils(string CompID, string br_id, string cnf_no, string cnf_dt)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                      objProvider.CreateInitializedParameter("@rcpt_no",DbType.String, cnf_no),
                      objProvider.CreateInitializedParameter("@rcpt_dt",DbType.String, cnf_dt),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$FGR$PrintDetails]", prmContentGetDetails);
            return ds;
        }
    }
   
}
