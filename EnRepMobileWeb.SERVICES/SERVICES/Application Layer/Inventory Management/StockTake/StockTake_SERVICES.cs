using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.StockTake;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.StockTake;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.StockTake
{
   public class StockTake_SERVICES : StockTake_ISERVICES
    {

        public DataSet GetItemList(string CompId, string BranchId, string ItmName,string GroupID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@Branch",DbType.Int64, BranchId),
                     objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                       objProvider.CreateInitializedParameter("@GroupID",DbType.String, GroupID),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllItemNameList_ForStockTake", prmContentGetDetails);
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

        public DataSet GetItemListForAddNewPopup(string CompId, string BranchId, string ItmName, string PageName,int Wh_Id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BranchId),
                     objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                     objProvider.CreateInitializedParameter("@PageName",DbType.String, PageName),
                       objProvider.CreateInitializedParameter("@Wh_Id",DbType.Int64, Wh_Id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetAllItemNameList_ForAddNewPopup", prmContentGetDetails);
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

        public Dictionary<string, string> ItemGroupList(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetItemGroupStockTake]", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["item_grp_id"].ToString(), PARQusData.Tables[0].Rows[i]["ItemGroupChildNood"].ToString());
                    }
                }
                return ddlItemNameDictionary;

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
        public DataSet GetWarehouseListPopUp(string CompId, string BrID,string WareHouseName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                      objProvider.CreateInitializedParameter("@WarehouseName",DbType.String, WareHouseName),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetWarehouseListStockTake", prmContentGetDetails);
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
        public Dictionary<string, string> GetWarehouseList(string WarehouseName, string CompID,string BrID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@WarehouseName",DbType.String, WarehouseName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetWarehouseListStockTake", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["wh_id"].ToString(), PARQusData.Tables[0].Rows[i]["wh_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

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
        public DataSet GetStockItemDetail(string CompID, string BrchID, string WHID, string GRPID, string ItemID,string ListItems)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@WHID",DbType.String, WHID),
                     objProvider.CreateInitializedParameter("@GRPID",DbType.String, GRPID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                     objProvider.CreateInitializedParameter("@ListItems",DbType.String, ListItems),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetStockTakeItemDetail]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetStockItemLotBatchSerialDetail(string CompID, string BrchID, string ItemID, string WHID, string SrcDocNumber, string RT_Status,string flag,string hdFlagForAddNewStk)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                      objProvider.CreateInitializedParameter("@WHID",DbType.String, WHID),
                      objProvider.CreateInitializedParameter("@SrcDocNo",DbType.String, SrcDocNumber),
                      objProvider.CreateInitializedParameter("@RT_Status",DbType.String, RT_Status),
                       objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                       objProvider.CreateInitializedParameter("@hdFlagForAddNewStk",DbType.String, hdFlagForAddNewStk),

           };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetStockTakeitemLotBatchSerialDetail]", prmContentGetDetails);
            return ds;
        }
        public String InsertStockTakeDetail(DataTable StockTakeHeader, DataTable StockTakeItemDetails
            , DataTable StockTakeLotBatchSerial,DataTable dtSubItem)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, StockTakeHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, StockTakeItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemLotBatchSerialDetail",SqlDbType.Structured,StockTakeLotBatchSerial ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem ),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string stk_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_InsertStockTake_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }


        }
        public DataSet GetStockTakeDetail(string Stk_no, string Stk_dt, string CompID, string BrchID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Stk_no",DbType.String, Stk_no),
                    objProvider.CreateInitializedParameter("@Stk_dt",DbType.String, Stk_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_StockTake$DetailView", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet GetStatusList(string MenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@menu_id",DbType.String, MenuID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$DocStatus", prmContentGetDetails);
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
        public DataSet GetStockTakeListAll(string Fromdate, string Todate, string Status, string CompID, string BrchID, string wfstatus, string UserID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                                                   
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                          objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetStockTakeList$details]", prmContentGetDetails);
            return dt;
        }
        public DataSet StockTakeDelete(StockTake_Model _StockTake_Model, string CompID, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                   objProvider.CreateInitializedParameter("@stk_no",DbType.String, _StockTake_Model.stktake_no),
                    objProvider.CreateInitializedParameter("@stk_dt",DbType.Date,  _StockTake_Model.stktake_dt),

                                                     };
                DataSet Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_prc$StockTakeDelete]", prmContentGetDetails);
                return Deatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string StockTakeApprove(string STKNo, string STKDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@stk_no",DbType.String, STKNo),
                    objProvider.CreateInitializedParameter("@stk_dt",DbType.Date,  STKDate),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, userid ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                     };
                DataSet STKDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk_take$detail_Approve_Stock_take_Details", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = STKDeatils.Tables[0].Rows[0]["stk_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet StockTake_GetSubItemDetailsAfterApprove(string compID, string brchID, string item_id, string doc_no, string doc_dt, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brchID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                                                     };
                DataSet Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "StockTake_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Deatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
    }
}
