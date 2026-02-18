using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.OpeningMaterialReceipt;
using EnRepMobileWeb.UTILITIES;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.OpeningMaterialReceipt;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialReceipt.OpeningMaterialReceipt
{
    public class OpeningMaterialReceipt_SERVICE : OpeningMaterialReceipt_ISERVICE
    {
        public DataSet GetOpeningRcptItmList(string CompID, string BrID, string ItmName, string wh_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                     objProvider.CreateInitializedParameter("@wh_id",DbType.String, wh_id),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetSR_ItemList", prmContentGetDetails);
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
        public DataSet GetItemUOMDAL(string Item_id, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, Item_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetItemUOMDetails", prmContentGetDetails);
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
        public string InsertOP_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTItemBatchDetail, DataTable DTItemSerialDetail,DataTable dtSubItem,string op_rno)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                      objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@BatchDetail",SqlDbType.Structured, DTItemBatchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@SerialDetail",SqlDbType.Structured,DTItemSerialDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem),
                                                        objprovider.CreateInitializedParameterTableType("@RId",SqlDbType.BigInt,op_rno),
                                                    };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$op$detail_InsertOpeningMaterial_Details", prmcontentaddupdate).ToString();
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

            finally
            {
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
        public DataSet GetOPRDetailList(string CompId, string BrchID,string Fromdate, string wfstatus, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),                                                      
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String,UserID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String,DocumentMenuId),
                                                      };
                DataSet GetOPList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$opstk$detail_GetOP_DeatilList", prmContentGetDetails);
                return GetOPList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetOpeningDate(int CompID, int BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),                    
                                                     };
                DataSet GetFY = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Get$FY$Openingdetail", prmContentGetDetails);
                return GetFY;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Edit_OpeningDetail(string CompId, string BrID, string OPDate, string wh_id,string UserID, string DocID, string id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),               
                objProvider.CreateInitializedParameter("@OP_Date",DbType.String,OPDate),
                 objProvider.CreateInitializedParameter("@wh_id",DbType.String,wh_id),
                  objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                 objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                 objProvider.CreateInitializedParameter("@RID",DbType.String, id),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$opstk$detail_GetOpening_Details", prmContentGetDetails);
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
        public string Delete_OPR_Detail(OpeningMaterialReceiptModel model, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),                                                       
                    objProvider.CreateInitializedParameter("@OPR_Date",DbType.String,model.Opening_dt),
                      objProvider.CreateInitializedParameter("@wh_id",DbType.String,model.wh_id),
                       objProvider.CreateInitializedParameter("@id",DbType.String,model.opstk_rno),
                };
                string SRId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "inv$opstk$detail_DeleteOpeningMaterial_Details", prmContentInsert).ToString();
                return SRId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string Approve_OpeningMaterialReceipt(string id,string wh_id, string OPRDate, string Branch, string CompID, string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks,string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    
                                                        objProvider.CreateInitializedParameter("@Oprdate",DbType.String, OPRDate),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String,Branch),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                         objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                          objProvider.CreateInitializedParameter("@wh_id",DbType.String,wh_id),
                                                            objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                                                          objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                          objProvider.CreateInitializedParameter("@id",DbType.String, id),
                };
                DataSet OPR_Detail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$opstk$detail_Approve_OpeningMaterial_Details", prmContentInsert);
                string DocNo = string.Empty;
                DocNo = OPR_Detail.Tables[1].Rows[0]["op_detail"].ToString();
                return DocNo; ;

                //return OPR_Detail;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet GetWarehouseList(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse_GetOpeningReceiptWarehouseList", prmContentGetDetails);
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
        public DataSet GetOpeningQtyDetauls(string comp_id, string br_id,string op_dt, string wh_id, string Item_id, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comp_id),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                                                        objProvider.CreateInitializedParameter("@wh_id",DbType.Int64, wh_id),
                                                        objProvider.CreateInitializedParameter("@op_dt",DbType.String, op_dt),
                                                        objProvider.CreateInitializedParameter("@itme_id",DbType.String, Item_id),
                                                         objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                                                      };
                DataSet opqty_details = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetOpeningItemQtyDetails", prmContentGetDetails);
                return opqty_details;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet OMR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string wh_id, string OP_dt, string Flag)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.String,wh_id),
                    objProvider.CreateInitializedParameter("@OP_dt",DbType.String, OP_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "OMR_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        public DataSet GetMasterDataForExcelFormat(string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetMasterDataforOPRImportFormat", prmContentGetDetails);
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
        public DataSet GetVerifiedDataOfExcel(string Warehouse, string BrchID, DataTable ItemDetail, DataTable BatchDetail, DataTable SerialDetail, DataTable SubItemDetail, string compId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, ItemDetail ),
                    objProvider.CreateInitializedParameterTableType("@BatchDetail",SqlDbType.Structured,BatchDetail),
                    objProvider.CreateInitializedParameterTableType("@SerialDetail",SqlDbType.Structured, SerialDetail ),
                       objProvider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, SubItemDetail ),
                       objProvider.CreateInitializedParameter("@whid", DbType.String,Warehouse),
                       objProvider.CreateInitializedParameter("@brid", DbType.String,BrchID),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),

                };
                DataSet GetOpeningReceiptList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateOpeningMaterialRecepitExceFile", prmContentGetDetails);
                return GetOpeningReceiptList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable ShowExcelErrorDetail(string Warehouse, string BrchID, DataTable ItemDetail, DataTable BatchDetail, DataTable SerialDetail, DataTable SubItemDetail, string compId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, ItemDetail ),
                    objProvider.CreateInitializedParameterTableType("@BatchDetail",SqlDbType.Structured,BatchDetail),
                    objProvider.CreateInitializedParameterTableType("@SerialDetail",SqlDbType.Structured, SerialDetail ),
                       objProvider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, SubItemDetail ),
                       objProvider.CreateInitializedParameter("@whid", DbType.String,Warehouse),
                       objProvider.CreateInitializedParameter("@brid", DbType.String,BrchID),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ShowExcelOpeningMaterialRecepitErrorDetail", prmContentGetDetails);
                return GetItemList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
               
        public string BulkImportOMRDetail(string compId, string UserID, string brId, string Warehouse, string op_dt, float op_val1, DataTable ItemDetail, DataTable BatchDetail, DataTable SerialDetail, DataTable SubItemDetail)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                    objProvider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, ItemDetail ),
                    objProvider.CreateInitializedParameterTableType("@BatchDetail",SqlDbType.Structured,BatchDetail),
                    objProvider.CreateInitializedParameterTableType("@SerialDetail",SqlDbType.Structured, SerialDetail ),
                    objProvider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, SubItemDetail ),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                    objProvider.CreateInitializedParameter("@userId",DbType.String,UserID),
                    objProvider.CreateInitializedParameter("@brId", DbType.String,brId),
                    objProvider.CreateInitializedParameter("@whid",DbType.String,Warehouse),
                    objProvider.CreateInitializedParameter("@op_dt",DbType.String,op_dt),
                    objProvider.CreateInitializedParameter("@op_val",DbType.String,op_val1),
                    objProvider.CreateInitializedParameterTableType("@OutPut",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[10].Size = 100;
                prmcontentaddupdate[10].Direction = ParameterDirection.Output;

               string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BulkImportOpeningMaterialRecepit", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[10].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[10].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
    }
}
