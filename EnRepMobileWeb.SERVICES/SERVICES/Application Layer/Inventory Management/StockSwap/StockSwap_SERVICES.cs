using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.Stock_Swap;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.Stock_Swap;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.Stock_Swap
{
    public class StockSwap_SERVICES: StockSwap_ISERVICES 
    {
        public DataTable GetProductNameLists(string CompId, string br_id,string itemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.Int64, itemId),

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetItemNameListForStockSwap]", prmContentGetDetails).Tables[0];
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
        public DataTable GetWarehouseNameLists(string CompId, string br_id, string wh_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@WarehouseName",DbType.Int64, wh_id),

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetWarehouseListStockTake]", prmContentGetDetails).Tables[0];
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
        public DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockBatchwiseSwapstock]", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string SwapNumber, string SwapDate, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@swp_no",DbType.String, SwapNumber),
            objProvider.CreateInitializedParameter("@swp_dt",DbType.String, SwapDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_SwpStkItem_StockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetItemStockSerialwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID,  string SwapNumber, string SwapDate, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@swp_no",DbType.String, SwapNumber),
            objProvider.CreateInitializedParameter("@swp_dt",DbType.String, SwapDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_SwpStkItem_StockSerialwise", prmContentGetDetails);
            return DS;
        }
        public string InsertUpdateMaterialIssue(StockSwap_Model _StockSwap_Model,string CompID,string BrchID,string Userid,string mac_id,string DocumentMenuId, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objprovider.CreateInitializedParameter("@br_id",DbType.String,BrchID),
                 objprovider.CreateInitializedParameter("@swp_no",DbType.String, _StockSwap_Model.SwapNumber),
                 objprovider.CreateInitializedParameter("@swp_dt",DbType.String, _StockSwap_Model.SwapDate),
                 objprovider.CreateInitializedParameter("@src_prod_id",DbType.String, _StockSwap_Model.ProductName),
                 objprovider.CreateInitializedParameter("@src_uom",DbType.String, _StockSwap_Model.Src_UomID),
                 objprovider.CreateInitializedParameter("@src_wh_id",DbType.String, _StockSwap_Model.Warehouse),
                 objprovider.CreateInitializedParameter("@swp_qty",DbType.String, _StockSwap_Model.SwapQuantity),
                 objprovider.CreateInitializedParameter("@dest_prod_id",DbType.String, _StockSwap_Model.ProductName1),
                 objprovider.CreateInitializedParameter("@dest_uom",DbType.String, _StockSwap_Model.Dest_UomID),
                 objprovider.CreateInitializedParameter("@dest_wh_id",DbType.String, _StockSwap_Model.Warehouse1),
                 objprovider.CreateInitializedParameter("@src_avl_stk",DbType.String, _StockSwap_Model.AvailableQuantitySrc),
                 objprovider.CreateInitializedParameter("@dest_avl_stk",DbType.String, _StockSwap_Model.AvailableQuantityDest),
                 objprovider.CreateInitializedParameter("@create_id",DbType.String, Userid),
                 objprovider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                 objprovider.CreateInitializedParameter("@MenuID",DbType.String, DocumentMenuId),
                 objprovider.CreateInitializedParameter("@TransType",DbType.String, _StockSwap_Model.TransType),
                 objprovider.CreateInitializedParameter("@swp_type",DbType.String, _StockSwap_Model.swp_type),
                 objprovider.CreateInitializedParameter("@dest_swp_qty",DbType.String, _StockSwap_Model.DestSwapQuantity),
                 objprovider.CreateInitializedParameterTableType("@ItemBatchDetails",SqlDbType.Structured,ItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemSerialDetails",SqlDbType.Structured,ItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetals",SqlDbType.Structured,dtSubItem ),
                };
                prmcontentaddupdate[21].Size = 100;
                prmcontentaddupdate[21].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$stk$swap$detail_InsertAndUpdateSwapStock", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[21].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[21].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet Edit_SwapStockDetail(string CompId, string BrID, string SwpNumber, string SwpDate, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
                                                        objProvider.CreateInitializedParameter("@swp_no",DbType.String, SwpNumber),
                                                        objProvider.CreateInitializedParameter("@swp_dt",DbType.String, SwpDate),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$swap$detail_GetSwapStockDetails", prmContentGetDetails);
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
        public string ApproveSwapStockDetails(string compID, string brId, string swp_no,string swp_dt, string a_Status, string a_Level, string a_Remarks, string userID, string mac_id, string documentMenuId,string SwapType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, brId),
                    objProvider.CreateInitializedParameter("@swp_no",DbType.String, swp_no),
                    objProvider.CreateInitializedParameter("@swp_dt",DbType.String, swp_dt),
                    objProvider.CreateInitializedParameter("@CreatedBy",DbType.String, userID ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, a_Status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, a_Level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, a_Remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, documentMenuId),
                        objProvider.CreateInitializedParameter("@SwapType",DbType.String, SwapType),
                     };
                DataSet VouDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stockSwapApprove", prmContentGetDetails);

                string DocNo = string.Empty;
                if (VouDeatils.Tables[0].Rows.Count > 0)
                {
                    DocNo = VouDeatils.Tables[0].Rows[0]["SwapStockDetail"].ToString();
                }
                
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeleteSwapStockDetails(string compID, string brId, string swp_no, string swp_dt)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                 sqlDataProvider.CreateInitializedParameter("@compId",DbType.String, compID ),
                 sqlDataProvider.CreateInitializedParameter("@brId",DbType.String, brId ),
                 sqlDataProvider.CreateInitializedParameter("@swp_no",DbType.String, swp_no ),
                 sqlDataProvider.CreateInitializedParameter("@swp_dt",DbType.String, swp_dt ),
                sqlDataProvider.CreateInitializedParameter("@DocNo",DbType.String,""),

            };
            sqlParameters[4].Direction = ParameterDirection.Output;
            sqlParameters[4].Size = 100;
            string dataSet = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SwapStockDeleteDetails", sqlParameters).ToString();
            string DocNo = string.Empty;
            if (sqlParameters[4].Value != DBNull.Value) // status
            {
                DocNo = sqlParameters[4].Value.ToString();
            }
            return DocNo;
        }
        public DataSet GetSwapStockList(string compID, string brId, string src_Prod_id, string dest_prod_id, string status, string DmenuId, string wfStatus, string Userid, string FromDate, string ToDate)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@comp_id",DbType.String,compID),
                sqlDataProvider.CreateInitializedParameter("@br_id",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@src_Prod_id",DbType.String,src_Prod_id),
                sqlDataProvider.CreateInitializedParameter("@dest_prod_id",DbType.String,dest_prod_id),
                sqlDataProvider.CreateInitializedParameter("@status",DbType.String,status),
                sqlDataProvider.CreateInitializedParameter("@DmenuId",DbType.String,DmenuId),
                sqlDataProvider.CreateInitializedParameter("@wfStatus",DbType.String,wfStatus),
                sqlDataProvider.CreateInitializedParameter("@UserId",DbType.String,Userid),
                sqlDataProvider.CreateInitializedParameter("@FromDate",DbType.String,FromDate),
                sqlDataProvider.CreateInitializedParameter("@ToDate",DbType.String,ToDate)
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_SwapStockList", sqlParameters);
            return dataSet;
        }
        public DataSet GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag,string Type)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                    objProvider.CreateInitializedParameter("@Type",DbType.String, Type),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SwapStkGetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string item_id, string flag,string swp_no,string swp_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    objProvider.CreateInitializedParameter("@swp_no",DbType.String, swp_no),
                    objProvider.CreateInitializedParameter("@swp_dt",DbType.String, swp_dt),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "swpStk_GetSubItemAvlStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataitemList(string CompID, string BrchID, string ItmName, string Itemid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, Itemid),
                };
                DataSet GetData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$item$dest$stock$swap", prmContentGetDetails);
                return GetData;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }
    }
}
