using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.Assembly_Kit;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.Assembly_Kit
{
    public class AssemblyKit_SERVICES : AssemblyKit_ISERVICES
    {
        public DataSet BindItemList(string ItemName, string CompID, string BranchId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@ItemName",DbType.String,ItemName),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BranchId",DbType.String,BranchId),


                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Bind$AssemblyLit$DropDown$List", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetItemStockBatchWise(string CompId, string BranchId, string ItemId, string Wh_ID, string uom_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, Wh_ID),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@UomId",DbType.String,  uom_id),
            };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetItemStockBatchwise", prmContentGetDetails);
                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string Wh_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, Wh_ID),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockSerialwise]", prmContentGetDetails);
                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string Doc_no, string Doc_dt, string ItemID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@Doc_no",DbType.String, Doc_no),
            objProvider.CreateInitializedParameter("@Doc_dt",DbType.String, Doc_dt),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$ass$Kit_item$sr$detail_GetStockSerialwise", prmContentGetDetails);
                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertUpdate(DataTable HeaderTable, DataTable ItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable InputdtSubItem,DataTable Attachments)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, HeaderTable ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, ItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemBatchDetail",SqlDbType.Structured,ItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemSerialDetail",SqlDbType.Structured,ItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@SubItem",SqlDbType.Structured, InputdtSubItem ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, Attachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 
                };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$Ass$kit$_InsertUpdate", prmcontentaddupdate).ToString();

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
        public DataTable SearchDataFilter(string AssemblyProduct, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId, string Userid, string wf_status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@AssemblyProduct",DbType.String, AssemblyProduct),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
                objProvider.CreateInitializedParameter("@Userid",DbType.String, Userid),
                objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wf_status),
                                                     };
                DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$Ass$kit$list_Filter", prmContentGetDetails).Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDeatilData(string CompID, string BrchID, string Doc_no, string Doc_dt, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, Doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, Doc_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$ass$kit$deatil$data", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet DeleteData(string comp_id, string br_id, string Doc_no, string Doc_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, Doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String,Doc_dt),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$Ass$kit$detail_Delete]", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approve_Detail(string comp_id, string br_id, string DocumentMenuID, string Doc_no, string Doc_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks)
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
                    objProvider.CreateInitializedParameter("@Doc_no",DbType.String,Doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.Date,  Doc_dt),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@stkstatus",DbType.String,""),
                };
                prmContentGetDetails[10].Size = 100;
                prmContentGetDetails[10].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Inv$AssemblyKit$Approve", prmContentGetDetails).ToString();

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
        public DataSet GetDeatilSubitm(string CompID, string BrID, string Item_id, string Uom_id, string WhID, string Doc_no, string Doc_dt, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@Uom_id",DbType.String, Uom_id),
                    objProvider.CreateInitializedParameter("@WhID",DbType.String, WhID),
                    objProvider.CreateInitializedParameter("@Doc_no",DbType.String, Doc_no),
                    objProvider.CreateInitializedParameter("@Doc_dt",DbType.String, Doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$subitem$ass$kit", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string doc_no, string doc_dt, string ItemID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@doc_no",DbType.String, doc_no),
            objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$ass$kit_item$bt$detail_GetStockBatchwise", prmContentGetDetails);
                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Cancel_Document(string CompID, string br_id, string Doc_no, string Doc_dt, string UserID, string DocumentMenuId, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@MenuDocId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@Doc_no",DbType.String, Doc_no),
                    objProvider.CreateInitializedParameter("@Doc_dt",DbType.Date, Doc_dt),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$ass$kit$detail_Cancel", prmContentGetDetails);

                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPurchaseAKDeatilsPDF(string Comp_ID, string Br_ID, string DocumentNumber, string DocumentDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@Doc_no",DbType.String, DocumentNumber),
                                                        objProvider.CreateInitializedParameter("@Doc_dt",DbType.String, DocumentDate),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAssemblyKitDeatils_ForPrint", prmContentGetDetails);
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
