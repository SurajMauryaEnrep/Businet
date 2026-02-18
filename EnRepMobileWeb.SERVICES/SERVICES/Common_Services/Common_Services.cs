using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.UTILITIES;
using System.Data;
using System.Net.Mail;
using System.Security.Cryptography;
using System.IO;

namespace EnRepMobileWeb.SERVICES.SERVICES.Common_Services
{
    public class Common_Services : Common_IServices
    {
        public Dictionary<string, string> SuppCityDAL(string GroupName)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CityName",DbType.String, GroupName),
                                  };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$SuppCity$detail_GetSuppCityList", prmContentGetDetails);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["city_id"].ToString(), PARQusData.Tables[0].Rows[i]["city_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string GetPageNameByDocumentMenuId(string CompID, string DocumentMenuId, string language)
        {
            string PageName = string.Empty;
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            objProvider.CreateInitializedParameter("@language",DbType.String, language)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$GetPageNameByDocumentId]", prmContentGetDetails);
            PageName = ds.Tables[0].Rows[0]["pagename"].ToString();
            return PageName;
        }
        public DataSet GetApprovalLevel(string CompID, string BrID, string DocumentMenuId)
        {
            string PageName = string.Empty;
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@doc_no",DbType.String, DocumentMenuId)
            };
            DataSet DS_ApprovalLevel = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$doc$app_GetApprovalLevel", prmContentGetDetails);

            return DS_ApprovalLevel;
        }
        public DataSet GetCommonPageDetails(string CompID, string BrID, string UserID, string DocumentMenuId, string language)
        {
            string PageName = string.Empty;
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@UserID",DbType.Int32, UserID),
            objProvider.CreateInitializedParameter("@doc_no",DbType.String, DocumentMenuId),
            objProvider.CreateInitializedParameter("@language",DbType.String, language)
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetCommonPageDetails", prmContentGetDetails);

            return DS;
        }
        public DataSet GetItemGstDetails(string CompID, string BrID, string ItemIDs, string gst_number)
        {
            string PageName = string.Empty;
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@ItemIDs",DbType.String, ItemIDs),
            objProvider.CreateInitializedParameter("@gst_number",DbType.String, gst_number)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Common_GetItemGstDetails", prmContentGetDetails);

            return ds;
        }
        public DataSet GetGSTApplicable(string CompID, string BrID, string param_id)
        {
            string PageName = string.Empty;
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@param_id",DbType.String, param_id)
            };
            DataSet DS_ApprovalLevel = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct_GetGSTApplicable", prmContentGetDetails);

            return DS_ApprovalLevel;
        }
        public DataSet GetBrList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$BrList$detail", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetBaseCurrency(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_getBaseCurrency", prmContentGetDetails);
                return GetBrList;

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
        public DataTable GetRole_List(string CompID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                      objProvider.CreateInitializedParameter("@UserID",DbType.String,UserID),
                      objProvider.CreateInitializedParameter("@DocID",DbType.String,DocumentMenuId),
                                                     };
                DataTable GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sec$role$menu$detail_GetRoleList", prmContentGetDetails).Tables[0];
                return GetItemList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }

        }
        public string InsertForwardDetails(string compid, string brid, string docid, string docno, string docdate, string status, string forwarededto, string forwardedby, string level, string remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                       objProvider.CreateInitializedParameter("@comp_id",DbType.String, compid),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, brid ),
                                                        objProvider.CreateInitializedParameter("@doc_id",DbType.String, docid),
                                                        objProvider.CreateInitializedParameter("@doc_no",DbType.String,docno),
                                                        objProvider.CreateInitializedParameter("@doc_date",DbType.String,docdate),
                                                        objProvider.CreateInitializedParameter("@status",DbType.String, status),
                                                        objProvider.CreateInitializedParameter("@level",DbType.String, level),
                                                        objProvider.CreateInitializedParameter("@forwarded_to",DbType.String, forwarededto ),
                                                        objProvider.CreateInitializedParameter("@forwarded_by",DbType.String, forwardedby),
                                                        objProvider.CreateInitializedParameter("@remarks",DbType.String,remarks),
                                                        objProvider.CreateInitializedParameter("@fStatus",DbType.String,""),
                                                    };
                prmcontentaddupdate[10].Size = 100;
                prmcontentaddupdate[10].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "stp$app$wf$detail_InsertForwardDetail", prmcontentaddupdate).ToString();
                string fstatus = string.Empty;
                if (prmcontentaddupdate[10].Value != DBNull.Value) // status
                {
                    fstatus = prmcontentaddupdate[10].Value.ToString();
                }

                return fstatus;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet GetWFLevel_Detail(string CompId, string BrID, string DocNo, string DocDate, string DocID, string DocStatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, DocDate),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                        objProvider.CreateInitializedParameter("@DocStatus",DbType.String, DocStatus),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$app$wf$detail_GetWF_LevelDetails", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetItemDetailDL(string ItemID, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetItemDetails", prmContentGetDetails);
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
        public DataTable GetAddressdetail(string CustID, string CompId, string CustPros_type, string BranchID,string Add_type,string add_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                          objProvider.CreateInitializedParameter("@CustPros_type",DbType.String, CustPros_type),
                                                           objProvider.CreateInitializedParameter("@BranchID",DbType.String, BranchID),
                                                           objProvider.CreateInitializedParameter("@Add_type",DbType.String, Add_type),
                                                           objProvider.CreateInitializedParameter("@add_id",DbType.String, add_id),
                                                      };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetAddressDetail", prmContentGetDetails).Tables[0];
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
        public DataTable GetSuppAddressdetail(string SuppID, string CompId, string BranchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, SuppID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BranchID",DbType.Int64, BranchID),
                                                      };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetSupplierAddressDetail", prmContentGetDetails).Tables[0];
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
        public DataSet GetItemCustomerInfo(string ItemID, string CustID, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                         objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetCustInfoDetails", prmContentGetDetails);
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
        public DataSet GetItemSupplierInfo(string ItemID, string SuppID, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                         objProvider.CreateInitializedParameter("@SuppID",DbType.String, SuppID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetSuppInfoDetails", prmContentGetDetails);
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
        public DataSet GetItmListDL(string CompID, string BrID, string ItmName, string PageName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@PageName",DbType.String, PageName),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetSO_ItemList", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet GetItmListDL1(string CompID, string BrID, string ItmName, string PageName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@PageName",DbType.String, PageName),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetPO_ItemList", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet GetAccountListDDL(string CompID, string BrID, string AccName, string DocMenuID, string gl_curr_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@AccName",DbType.String, AccName),
                    objProvider.CreateInitializedParameter("@DocumentID",DbType.String, DocMenuID),
                    objProvider.CreateInitializedParameter("@gl_curr_id",DbType.String, gl_curr_id),
                                                     };
                DataSet JornlVoucData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_BindAccountJournlVouc", prmContentGetDetails);
                return JornlVoucData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAccGrpDDL(string CompId, string Acc_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Acc_ID",DbType.String, Acc_ID),
                                                        objProvider.CreateInitializedParameter("@Comp_id",DbType.Int64, CompId),
                                                        //objProvider.CreateInitializedParameter("@AccGroupType",DbType.String, AccGroupType),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Common_GetAccountGroupDetails", prmContentGetDetails);
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
        public DataSet GetAccBalance(string CompId, string BrID, string Acc_ID,string VouDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@acc_id",DbType.String, Acc_ID),
                                                        objProvider.CreateInitializedParameter("@VouDate",DbType.String, VouDate),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetAccountBalanceByAccId", prmContentGetDetails);
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
        public string getWarehouseWiseItemStock(string CompID, string BrID, string Wh_ID, string ItemID, string UomId
            , string LotID, string BatchNo, string DocumentMenuId)
        {
            string AvaiableStock = "0";
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@Wh_ID",DbType.Int32,  Wh_ID),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemID),
            objProvider.CreateInitializedParameter("@UomId",DbType.String,  UomId),
            objProvider.CreateInitializedParameter("@LotID",DbType.String,  LotID),
            objProvider.CreateInitializedParameter("@BatchNo",DbType.String,  BatchNo),
            objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String,  DocumentMenuId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetAvlStock_wh]", prmContentGetDetails);
            if (DS.Tables[0].Rows.Count > 0)
                AvaiableStock = DS.Tables[0].Rows[0]["wh_avl_stk_bs"].ToString();

            return AvaiableStock;
        }
        public DataSet Getfw_List(string CompID, string BrID, string UserID, string DocumentMenuId)
        {
            string PageName = string.Empty;
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@user_id",DbType.Int32, UserID),
            objProvider.CreateInitializedParameter("@doc_no",DbType.String, DocumentMenuId)
            };
            DataSet DS_ApprovalLevel = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$doc$app$dt_GetAndChkApprovalLevel", prmContentGetDetails);

            return DS_ApprovalLevel;
        }
        public DataSet GetfwHistory_List(string CompID, string BrID, string DocNo, string DocId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
            objProvider.CreateInitializedParameter("@DocID",DbType.String, DocId)
            };
            DataSet DS_ApprovalLevel = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$app$wf$detail_GetWorkflowHistory", prmContentGetDetails);

            return DS_ApprovalLevel;
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
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockBatchwise]", prmContentGetDetails);
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
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockSerialwise]", prmContentGetDetails);
            return DS;
        }
        public string getWarehouseWiseItemStock(string CompID, string BrID, string Wh_ID, string ItemID)
        {
            string AvaiableStock = "0";
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@Wh_ID",DbType.Int32,  Wh_ID),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemID),
            objProvider.CreateInitializedParameter("@LotID",DbType.String,  ""),
            objProvider.CreateInitializedParameter("@BatchNo",DbType.String,  ""),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetAvlStock_wh]", prmContentGetDetails);
            if (DS.Tables[0].Rows.Count > 0)
                AvaiableStock = DS.Tables[0].Rows[0]["wh_avl_stk_bs"].ToString();

            return AvaiableStock;
        }
        public DataSet getItemstockWarehouseWise(string ItemId, string UomId, string CompId, string BranchId, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String,  DocumentMenuId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockWareHousewise]", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockShopFloorWise(string ItemId, string UomId, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$shfl$stk$detail_GetItemStockShopfloorWise]", prmContentGetDetails);
            return DS;
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse_GetWarehouseList", prmContentGetDetails);
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
        public DataTable GetItemSOHistory(string ItemID, string CustID, string FinStDt, string Date12, string CompId, string BranchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                        objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                                                             objProvider.CreateInitializedParameter("@FinStDt",DbType.String, FinStDt),
                                                        objProvider.CreateInitializedParameter("@Date12",DbType.String, Date12),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                         objProvider.CreateInitializedParameter("@BranchID",DbType.Int64, BranchID),
                                                      };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetItemSOHistory", prmContentGetDetails).Tables[0];
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
        public DataTable GetCustomerSalesHistory(string CustID, string FinStDt, string Date12, string CompId, string BranchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                                                             objProvider.CreateInitializedParameter("@FinStDt",DbType.String, FinStDt),
                                                        objProvider.CreateInitializedParameter("@Date12",DbType.String, Date12),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                         objProvider.CreateInitializedParameter("@BranchID",DbType.Int64, BranchID),
                                                      };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetCustomerSalesHistory", prmContentGetDetails).Tables[0];
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
        public DataTable GetItemPOHistory(string ItemID, string SuppID, string FinStDt, string Date12, string DMenuId, string CompId, string BranchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, SuppID),
                                                         objProvider.CreateInitializedParameter("@FinStDt",DbType.String, FinStDt),
                                                        objProvider.CreateInitializedParameter("@Date12",DbType.String, Date12),
                                                         objProvider.CreateInitializedParameter("@DocID",DbType.String, DMenuId),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                         objProvider.CreateInitializedParameter("@BranchID",DbType.Int64, BranchID),
                                                      };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetItemPOHistory", prmContentGetDetails).Tables[0];
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
        public DataSet GetItemUOMDL(string Item_id, string CompId, string br_id, string ItemUomType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, Item_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                                                        objProvider.CreateInitializedParameter("@ItemUomType",DbType.String, ItemUomType),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Common_GetItemUOMDetails", prmContentGetDetails);
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
        public DataSet GetItemAvlStock(string CompId, string BrID, string Item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {objProvider.CreateInitializedParameter("@CompId",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BranchId",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, Item_id),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetItemStock_BranchWise", prmContentGetDetails);
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
        public DataSet GetItemAvlStockShopfloor(string CompId, string BrID, string Item_id, string MaterialType, string SourceShopfloor)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {objProvider.CreateInitializedParameter("@CompId",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BranchId",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, Item_id),
                                                        objProvider.CreateInitializedParameter("@MaterialType",DbType.String, MaterialType),
                                                        objProvider.CreateInitializedParameter("@SourceShopfloor",DbType.Int64, SourceShopfloor),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetItemStock_BranchWise_shopfloor", prmContentGetDetails);
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
        public DataSet GetOtherChargeDAL(string CompId, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$oc$setup_GetOtherChargeList", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Get_VoucherDetails(string compid, string brid, string vouno, string voudt, string flag, string narr)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, compid),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, brid),
                                                        objProvider.CreateInitializedParameter("@vou_no",DbType.String, vouno),
                                                        objProvider.CreateInitializedParameter("@vou_dt",DbType.String, voudt),
                                                        objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                                                        objProvider.CreateInitializedParameter("@narr",DbType.String, narr),
                                                      };
                DataSet get_voudetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetGLVoucherDetails", prmContentGetDetails);
                return get_voudetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet get_Costcenterdetails(string compid, string brid, string vouno, string voudt, string acc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, compid),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, brid),
                                                        objProvider.CreateInitializedParameter("@vou_no",DbType.String, vouno),
                                                        objProvider.CreateInitializedParameter("@vou_dt",DbType.String, voudt),
                                                        objProvider.CreateInitializedParameter("@acc_id",DbType.String, acc_id),
                                                      };
                DataSet get_ccdetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_getglcostcenterdetails", prmContentGetDetails);
                return get_ccdetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetTaxListDAL(string CompId, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$tax$setup_GetTaxTypes", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetTaxPercentageDAL(string CompId, string BrchID, string TaxID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@TaxID",DbType.String, TaxID),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$tax$setup_GetTaxPercent", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindTaxSlablist(string Comp_ID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_TaxSlabList", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindTaxTemplatelist(string Comp_ID, string DocumentMenuId, string Tmplt_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@tax_type",DbType.String, Tmplt_type),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_TaxTemplateList", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindTaxTemplateData(string Comp_ID, string tmplt_id, string TaxSlab, string Br_ID, string ItemId, string GSTNo)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, tmplt_id),
                                                        objProvider.CreateInitializedParameter("@TaxBySlab",DbType.String, TaxSlab),
                                                        objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@ItemIDs",DbType.String, ItemId),
                                                        objProvider.CreateInitializedParameter("@gst_number",DbType.String, GSTNo),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_TaxTemplateData", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindTermsTemplatelist(string Comp_ID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_TermsTemplateList", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindTermsTemplateData(string Comp_ID, string tmplt_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, tmplt_id),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_TermsTemplateData", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetTaxTemplateByOC(string Comp_ID, string OC_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@OC_id",DbType.String, OC_id),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_OC_TaxTemplateData", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckUserRolePageAccess(string Comp_ID, string Br_ID, string UserID, string MaterPagetype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@MaterPagetype",DbType.String, MaterPagetype),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Check$User$Role$PageAccess", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CmnGetOcTpEntityList(string comp_ID, string ocId, string currId, string DocId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_ID),
                                                        objProvider.CreateInitializedParameter("@oc_id",DbType.String, ocId),
                                                        objProvider.CreateInitializedParameter("@curr_id",DbType.String, currId),
                                                        objProvider.CreateInitializedParameter("@DocId",DbType.String,DocId),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_OC_TP_EntityList", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCompLogo(string Comp_ID, string Br_id, string BrId_List = null)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@Br_id",DbType.String, Br_id),
                                                        objProvider.CreateInitializedParameter("@br_list",DbType.String, BrId_List),
                                                      };
                DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_CompanyLogo", prmContentGetDetails).Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        /*----------------------------Sub Item---------------------------------*/
        public DataSet GetSubItemWIPstockDetails(string CompID, string br_ID, string shfl_id, string ItemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                    objProvider.CreateInitializedParameter("@shfl_id",DbType.Int32, shfl_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetSubItemWIPStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemShflAvlstockDetails(string CompID, string br_ID, string shfl_id, string ItemId, string stkType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                    objProvider.CreateInitializedParameter("@shfl_id",DbType.Int32, shfl_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@stkType",DbType.String, stkType),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetSubItemShflAvlStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id, string UomId, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.Int32, wh_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetSubItemAvlStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemDetails(string CompID, string ItemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetSubItemDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        /*----------------------------Sub Item---------------------------------*/
        public DataSet GetCstCntrData(string CompId, string BrchId, string CC_Id, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@BrchId",DbType.String,BrchId),
                                                    objProvider.CreateInitializedParameter("@CC_Id",DbType.String,CC_Id),
                                                    objProvider.CreateInitializedParameter("@Flag",DbType.String,Flag),

                                                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Usp_GetCostCentretype]", getCstDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAccountDetail(string acc_id, string CompId, string BrchId, string Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] GetAccountDetails = {
                                                    objProvider.CreateInitializedParameter("@acc_id",DbType.String,acc_id),
                                                    objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@br_id",DbType.String,BrchId),
                                                    objProvider.CreateInitializedParameter("@Date",DbType.String,Date),

                                                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Prc_GetGlAccDetails]", GetAccountDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Cmn_GetTDSDetail(string CompId, string BrchId, string doc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] GetAccountDetails = {
                                                    objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@br_id",DbType.String,BrchId),
                                                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String,doc_id),

                                                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[proc_Cmn_GetTDSDetail]", GetAccountDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Cmn_GetTDSTempltDetail(string CompId, string BrchId, string tmplt_id, string doc_id, string AmtForTds)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] GetAccountDetails = {
                                                    objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@br_id",DbType.String,BrchId),
                                                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String,doc_id),
                                                    objProvider.CreateInitializedParameter("@tmplt_id",DbType.String,tmplt_id),
                                                    objProvider.CreateInitializedParameter("@AmtForTds",DbType.String,AmtForTds),

                                                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[proc_Cmn_GetTDSTmpletDetail]", GetAccountDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public void SendAlertEmail(string compId, string brId, string docId, string docNo, string status, string sentBy, string sentTo, string docPdfFilePath)
        {
            try
            {
                if (status == "Forward")
                    status = "FW";
                else if (status == "Revert")
                    status = "RV";
                else if (status == "Reject")
                    status = "RJ";
                else if (status == "Amend")
                    status = "AM";
                else if (status == "Approve")
                    status = "AP";
                else if (status == "Cancel")
                    status = "C";
                else if (status.ToLower().Contains("force"))
                    status = "FC";
                else if (status.ToLower().Contains("qc"))
                    status = "QP";
                //else
                //    return;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                                                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,docNo),
                                                    objProvider.CreateInitializedParameter("@Status",DbType.String,status),
                                                    objProvider.CreateInitializedParameter("@DocId",DbType.String,docId),
                                                    objProvider.CreateInitializedParameter("@AlertType",DbType.String,"mail"),
                                                    objProvider.CreateInitializedParameter("@SentBy",DbType.String,sentBy),
                                               };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetUsersDetailToSendAlertmail", getCstDetails);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count < 1 || ds.Tables[1].Rows.Count < 1 || ds.Tables[2].Rows.Count < 1)
                        return;
                    string msgSubject = ds.Tables[1].Rows[0]["mailSubject"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "");// modified by Suraj on 08-10-2024 to replace <br /> to '' for {Line Break}
                    string msgHeader = ds.Tables[1].Rows[0]["header"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgBody = ds.Tables[1].Rows[0]["body"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgFooter = ds.Tables[1].Rows[0]["footer"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgAttachment = ds.Tables[1].Rows[0]["hasAttachment"].ToString();

                    bool enableSsl = false;
                    bool useDefaultCrediential = false;
                    int portNo = 25;

                    string sendFrom = ds.Tables[2].Rows[0]["sender_email"].ToString();
                    string senderPwd = ds.Tables[2].Rows[0]["pwd"].ToString();
                    bool.TryParse(ds.Tables[2].Rows[0]["ssl_flag"].ToString(), out enableSsl);
                    bool.TryParse(ds.Tables[2].Rows[0]["use_deflt_cred"].ToString(), out useDefaultCrediential);
                    string smtpHostName = ds.Tables[2].Rows[0]["host_server"].ToString();
                    int.TryParse(ds.Tables[2].Rows[0]["port"].ToString(), out portNo);
                    // -- Approve, Force Close, Cancel -- Main table else Workflow table
                    MailMessage mail = new MailMessage();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sendTo = ds.Tables[0].Rows[i]["user_email"].ToString();
                        //if (string.IsNullOrEmpty(sendTo))
                        //sendTo = "sanjay.prasad@enrep.biz";
                        if (!string.IsNullOrEmpty(sendTo))
                            mail.To.Add(new MailAddress(sendTo));
                    }
                    if (mail.To.Count < 1)
                        return;
                    mail.From = new MailAddress(sendFrom);
                    mail.Subject = msgSubject;
                    string msgBodyHtml = "<b>" + msgHeader + "</b>" + "<br /><br />" + msgBody + "<br /><br />" + msgFooter;
                    mail.Body = msgBodyHtml;

                    if (msgAttachment.ToUpper().Trim() == "YES")
                    {
                        msgAttachment = docPdfFilePath;
                        mail.Attachments.Add(new Attachment(msgAttachment));
                    }
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = smtpHostName;
                    smtp.Port = portNo;
                    smtp.UseDefaultCredentials = useDefaultCrediential;
                    smtp.Credentials = new System.Net.NetworkCredential(sendFrom, senderPwd);
                    smtp.EnableSsl = enableSsl;
                    if (!string.IsNullOrEmpty(mail.Subject))
                        smtp.Send(mail);
                    //mail sent
                }
                //Delete PDF after sent on email
                // System.IO.File.Delete(docPdfFilePath);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public void SendAlertEmail(string compId, string brId, string docId, string docNo, string status, string sentBy, string sentTo)
        {
            try
            {
                if (status == "Forward")
                    status = "FW";
                else if (status == "Revert")
                    status = "RV";
                else if (status == "Reject")
                    status = "RJ";
                else if (status == "Amend")
                    status = "AM";
                else if (status == "Approve")
                    status = "AP";
                else if (status == "Cancel")
                    status = "C";
                else if (status.ToLower().Contains("force"))
                    status = "FC";
                else if (status.ToLower().Contains("qc"))
                    status = "QP";
                //else
                //    return;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                                                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,docNo),
                                                    objProvider.CreateInitializedParameter("@Status",DbType.String,status),
                                                    objProvider.CreateInitializedParameter("@DocId",DbType.String,docId),
                                                    objProvider.CreateInitializedParameter("@AlertType",DbType.String,"mail"),
                                                    objProvider.CreateInitializedParameter("@SentBy",DbType.String,sentBy),
                                               };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetUsersDetailToSendAlertmail", getCstDetails);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count < 1 || ds.Tables[1].Rows.Count < 1 || ds.Tables[2].Rows.Count < 1)
                        return;
                    string msgSubject = ds.Tables[1].Rows[0]["mailSubject"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgHeader = ds.Tables[1].Rows[0]["header"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgBody = ds.Tables[1].Rows[0]["body"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgFooter = ds.Tables[1].Rows[0]["footer"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgAttachment = ds.Tables[1].Rows[0]["hasAttachment"].ToString();

                    bool enableSsl = false;
                    bool useDefaultCrediential = false;
                    int portNo = 25;

                    string sendFrom = ds.Tables[2].Rows[0]["sender_email"].ToString();
                    string senderPwd = ds.Tables[2].Rows[0]["pwd"].ToString();
                    bool.TryParse(ds.Tables[2].Rows[0]["ssl_flag"].ToString(), out enableSsl);
                    bool.TryParse(ds.Tables[2].Rows[0]["use_deflt_cred"].ToString(), out useDefaultCrediential);
                    string smtpHostName = ds.Tables[2].Rows[0]["host_server"].ToString();
                    int.TryParse(ds.Tables[2].Rows[0]["port"].ToString(), out portNo);
                    // -- Approve, Force Close, Cancel -- Main table else Workflow table
                    MailMessage mail = new MailMessage();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sendTo = ds.Tables[0].Rows[i]["user_email"].ToString();
                        //if (string.IsNullOrEmpty(sendTo))
                        //sendTo = "sanjay.prasad@enrep.biz";
                        if (!string.IsNullOrEmpty(sendTo))
                            mail.To.Add(new MailAddress(sendTo));
                    }
                    if (mail.To.Count < 1)
                        return;
                    mail.From = new MailAddress(sendFrom);
                    mail.Subject = msgSubject;
                    string msgBodyHtml = "<b>" + msgHeader + "</b>" + "<br /><br />" + msgBody + "<br /><br />" + msgFooter;
                    mail.Body = msgBodyHtml;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = smtpHostName;
                    smtp.Port = portNo;
                    smtp.UseDefaultCredentials = useDefaultCrediential;
                    smtp.Credentials = new System.Net.NetworkCredential(sendFrom, senderPwd);
                    smtp.EnableSsl = enableSsl;
                    if (!string.IsNullOrEmpty(mail.Subject))
                        smtp.Send(mail);
                    //mail sent
                }
                //Delete PDF after sent on email
                // System.IO.File.Delete(docPdfFilePath);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataSet Cmn_GetGLVoucherPrintDeatils(string CompID, string br_id, string JVNo, string JVDate, string Vou_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, JVNo),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  JVDate),
                    objProvider.CreateInitializedParameter("@vou_type",DbType.Date,  Vou_type),
                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_fin$get$vou$PrintsDetail]", prmContentGetDetails);
                return ImageDeatils; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //Added by Suraj on 17-10-2023
        public DataSet Cmn_getAlternateItemDetalInfo(string compID, string br_ID, string product_id, string op_id, string item_type_id, string item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_ID),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, product_id),
                    objProvider.CreateInitializedParameter("@op_id",DbType.String,  op_id),
                    objProvider.CreateInitializedParameter("@item_type_id",DbType.String,  item_type_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String,  item_id),
                     };
                DataSet Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[usp_GetBomAlternateItemInfo]", prmContentGetDetails);
                return Deatils; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable curFYdt(string compId, string brId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int32, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                     };
                DataSet details = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_CheckCompCurrentFY", prmContentGetDetails);
                if (details.Tables.Count > 0)
                    return details.Tables[0];
                else
                    return null;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string curFYdtAndPreviousFYdt(string compId, string brId, string DocDate)
        {

            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompID",DbType.Int32, compId),
            objProvider.CreateInitializedParameter("@BrID",DbType.Int32, brId),
            objProvider.CreateInitializedParameter("@DocDate",DbType.String,  DocDate)

            };
            string sr = string.Empty;
            DataTable detail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_CMN_CheckCompCurrentFYAndPreviousFY", prmContentGetDetails).Tables[0];
            sr = detail.Rows[0]["Result"].ToString();
            return sr;
            //DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SP_CMN_CheckCompCurrentFYAndPreviousFY]", prmContentGetDetails).Tables[0];
            //return searchmenu;
        }
        public DataSet Fin_curFYdt(string compId, string brId, string VouDt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int32, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.String, VouDt),
                     };
                DataSet details = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin_CheckCompCurrentFY", prmContentGetDetails);
                if (details.Tables.Count > 0)
                    return details;
                else
                    return null;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable Cmn_GetDeliverySchudule(string compId, string brId, string orderType, string orderNo, string itemId, string docId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int32, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                    objProvider.CreateInitializedParameter("@OrderType",DbType.String, orderType),
                    objProvider.CreateInitializedParameter("@Orderno",DbType.String, orderNo),
                    objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, docId),
                     };
                DataSet details = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_CmnGetDeliverySchudule", prmContentGetDetails);
                if (details.Tables.Count > 0)
                    return details.Tables[0];
                else
                    return null;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Get_rcptpatmhist_details(string CompId, string BrchId, string AccId, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@BrchId",DbType.String,BrchId),
                                                    objProvider.CreateInitializedParameter("@AccId",DbType.String,AccId),
                                                    objProvider.CreateInitializedParameter("@Flag",DbType.String,Flag),

                                                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc_get_chequedeatil", getDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetStockUomWise(string comp_ID, string brId, string itemId, string uomId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,comp_ID),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                                                    objProvider.CreateInitializedParameter("@ItemId",DbType.String,itemId),
                                                    objProvider.CreateInitializedParameter("@UomId",DbType.String,uomId),
                                                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get_stockUomWise", getDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Cmn_GetBrList(string comp_id, string userid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,comp_id),
                 objProvider.CreateInitializedParameter("@User_id",DbType.String,userid)
                 };
                DataSet br_list = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$detail_GetBrchList", prmContentGetDetails);
                return br_list.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet CmnGetOcTpEntityList(string comp_ID, string br_ID, string ocId, string currId, string DocId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getDetails = {
                                                    objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_ID),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,br_ID),
                                                    objProvider.CreateInitializedParameter("@oc_id",DbType.String,ocId),
                                                    objProvider.CreateInitializedParameter("@curr_id",DbType.String,currId),
                                                    objProvider.CreateInitializedParameter("@DocId",DbType.String,DocId),
                                                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_OC_TP_EntityList", getDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllGLDetails(DataTable GLDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                             objprovider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured,GLDetail),
                        };

                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetGLDetail", prmcontentaddupdate);
                return GetGlDt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet GetAllGLDetails1(DataTable GLDetail, string BrId)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                             objprovider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured,GLDetail),
                             objprovider.CreateInitializedParameterTableType("@BrId",SqlDbType.NVarChar,BrId),
                        };

                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetGLDetail1", prmcontentaddupdate);
                return GetGlDt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet Cmn_GetTdsDetails(string comp_ID, string br_ID, string SuppId, string GrossVal, string tax_type)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                             objprovider.CreateInitializedParameterTableType("@comp_id",SqlDbType.VarChar,comp_ID),
                             objprovider.CreateInitializedParameterTableType("@br_id",SqlDbType.VarChar,br_ID),
                             objprovider.CreateInitializedParameterTableType("@entity_id",SqlDbType.VarChar,SuppId),
                             objprovider.CreateInitializedParameterTableType("@gr_val",SqlDbType.VarChar,GrossVal),
                             objprovider.CreateInitializedParameterTableType("@tax_type",SqlDbType.VarChar,tax_type),
                        };

                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetTdsDetail", prmcontentaddupdate);
                return GetGlDt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }

        public DataSet Get_FYList(string Compid, string Brid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Compid),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String,Brid),
                };
                DataSet Getfy_list = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$fy_GetList", prmContentGetDetails);
                return Getfy_list;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable Cmn_GetBrList(string comp_id, string br_id, string userid, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,comp_id),
                 objProvider.CreateInitializedParameter("@BrID",DbType.String,br_id),
                  objProvider.CreateInitializedParameter("@User_id",DbType.String,userid),
                 objProvider.CreateInitializedParameter("@flag",DbType.String,flag)
                 };
                DataSet br_list = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$detail_GetBrchList", prmContentGetDetails);
                return br_list.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet Cmn_GetGernalLedgerDetails(string comp_id, string br_id, string acc_id, string from_dt, string to_dt, string doc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@gl_acc",DbType.String,acc_id),
                     objProvider.CreateInitializedParameter("@acc_grp",DbType.String,"0"),
                     objProvider.CreateInitializedParameter("@acc_type",DbType.String,"0"),
                     objProvider.CreateInitializedParameter("@curr",DbType.String,"0"),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String,from_dt),
                     objProvider.CreateInitializedParameter("@to_dt",DbType.String,to_dt),
                     objProvider.CreateInitializedParameter("@rpt_as",DbType.String,"TW"),
                     objProvider.CreateInitializedParameter("@doc_id",DbType.String,doc_id),
                };
                DataSet GetGLDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetGLAccDetails", prmContentGetDetails);
                return GetGLDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet Cmn_Get_GernalLedger_Details(string comp_id, string br_id, string acc_id, string from_dt, string to_dt, string doc_id, string curr_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@gl_acc",DbType.String,acc_id),
                     objProvider.CreateInitializedParameter("@acc_grp",DbType.String,"0"),
                     objProvider.CreateInitializedParameter("@acc_type",DbType.String,"0"),
                     objProvider.CreateInitializedParameter("@curr",DbType.String,curr_id),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String,from_dt),
                     objProvider.CreateInitializedParameter("@to_dt",DbType.String,to_dt),
                     objProvider.CreateInitializedParameter("@rpt_as",DbType.String,"TW"),
                     objProvider.CreateInitializedParameter("@doc_id",DbType.String,doc_id),
                };
                DataSet GetGLDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetGLAccDetails", prmContentGetDetails);
                return GetGLDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Cmn_GetSFBOMDetailsItemWise(string CompID, string BrID, string ItemId, string SFItemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                    objProvider.CreateInitializedParameter("@sf_ItemId",DbType.String, SFItemId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_ppl$bom$detail_GetMaterialDetail", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetItemName(string CompID, string BranchId, string itemNameSearch)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BranchId",DbType.String,BranchId),
                     objProvider.CreateInitializedParameter("@itemNameSearch",DbType.String,itemNameSearch),
                                                     };
                DataTable GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_Get$Item$Name$Search", prmContentGetDetails).Tables[0];
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //Added by Nidhi on 30-05-2025 15:05
        public string SendAlertEmailExternal(string compId, string brId, string UserId, string docId, string docNo, string status, string mail_id, string docPdfFilePath)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                                                    objProvider.CreateInitializedParameter("@UserId",DbType.String,UserId),
                                                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,docNo),
                                                    objProvider.CreateInitializedParameter("@Status",DbType.String,status),
                                                    objProvider.CreateInitializedParameter("@DocId",DbType.String,docId),
                                               };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetUsersDetailToSendAlertmailExt", getCstDetails);
                if (ds.Tables.Count > 0)
                {
                    string msgBody = "";
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return "invalidemail" + "," + msgBody;
                    }
                    if (ds.Tables[0].Rows.Count < 1 || ds.Tables[1].Rows.Count < 1)
                        return "invalid" + "," + msgBody;
                    string msgSubject = ds.Tables[0].Rows[0]["mailSubject"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "");// modified by Suraj on 08-10-2024 to replace <br /> to '' for {Line Break}
                    string msgHeader = ds.Tables[0].Rows[0]["header"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    msgBody = ds.Tables[0].Rows[0]["body"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgFooter = ds.Tables[0].Rows[0]["footer"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgAttachment = ds.Tables[0].Rows[0]["hasAttachment"].ToString();

                    bool enableSsl = false;
                    bool useDefaultCrediential = false;
                    int portNo;

                    string sendFrom = ds.Tables[1].Rows[0]["sender_email"].ToString();
                    string senderPwd = Decrypt(ds.Tables[1].Rows[0]["mail_pwd"].ToString());
                    string ssl = (ds.Tables[1].Rows[0]["ssl_flag"].ToString());
                    if (ssl == "Y")
                    {
                        enableSsl = true;
                    }
                    else
                    {
                        enableSsl = false;
                    }
                    string usedefltcredsl = (ds.Tables[1].Rows[0]["use_deflt_cred"].ToString());
                    if (usedefltcredsl == "Y")
                    {
                        useDefaultCrediential = true;
                    }
                    else
                    {
                        useDefaultCrediential = false;
                    }
                    string smtpHostName = ds.Tables[1].Rows[0]["host_server"].ToString();
                    int.TryParse(ds.Tables[1].Rows[0]["port"].ToString(), out portNo);
                    MailMessage mail = new MailMessage();
                    foreach (var address in mail_id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.To.Add(new MailAddress(address.Trim()));
                    }

                    // Handle attachment
                    msgAttachment = docPdfFilePath;
                    if (!string.IsNullOrEmpty(msgAttachment))
                    {
                        mail.Attachments.Add(new Attachment(msgAttachment));
                    }
                    mail.From = new MailAddress(sendFrom);
                    mail.Subject = msgSubject;
                    string msgBodyHtml = "<b>" + msgHeader + "</b>" + "<br /><br />" + msgBody + "<br /><br />" + msgFooter;
                    mail.Body = msgBodyHtml;

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = smtpHostName;
                    smtp.Port = portNo;
                    smtp.UseDefaultCredentials = useDefaultCrediential;
                    smtp.Credentials = new System.Net.NetworkCredential(sendFrom, senderPwd);
                    smtp.EnableSsl = enableSsl;
                    if (!string.IsNullOrEmpty(mail.Subject))
                        smtp.Send(mail);
                    status = "success" + "," + msgBody;
                }
                return status;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public string SendAlertEmailExternal1(string compId, string brId, string UserId, string docId, string docNo, string status, string mail_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                                                    objProvider.CreateInitializedParameter("@UserId",DbType.String,UserId),
                                                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,docNo),
                                                    objProvider.CreateInitializedParameter("@Status",DbType.String,status),
                                                    objProvider.CreateInitializedParameter("@DocId",DbType.String,docId),
                                               };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetUsersDetailToSendAlertmailExt", getCstDetails);

                if (ds.Tables.Count > 0)
                {
                    string msgBody = "";
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return "invalidemail";
                    }
                    if (ds.Tables[0].Rows.Count < 1 || ds.Tables[1].Rows.Count < 1)
                        return "invalid" + "," + msgBody;
                    string msgSubject = ds.Tables[0].Rows[0]["mailSubject"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "");// modified by Suraj on 08-10-2024 to replace <br /> to '' for {Line Break}
                    string msgHeader = ds.Tables[0].Rows[0]["header"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    msgBody = ds.Tables[0].Rows[0]["body"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgFooter = ds.Tables[0].Rows[0]["footer"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgAttachment = ds.Tables[0].Rows[0]["hasAttachment"].ToString();

                    bool enableSsl = false;
                    bool useDefaultCrediential = false;
                    int portNo;

                    string sendFrom = ds.Tables[1].Rows[0]["sender_email"].ToString();
                    string senderPwd = Decrypt(ds.Tables[1].Rows[0]["mail_pwd"].ToString());
                    string ssl = (ds.Tables[1].Rows[0]["ssl_flag"].ToString());
                    if (ssl == "Y")
                    {
                        enableSsl = true;
                    }
                    else
                    {
                        enableSsl = false;
                    }
                    string usedefltcredsl = (ds.Tables[1].Rows[0]["use_deflt_cred"].ToString());
                    if (usedefltcredsl == "Y")
                    {
                        useDefaultCrediential = true;
                    }
                    else
                    {
                        useDefaultCrediential = false;
                    }
                    string smtpHostName = ds.Tables[1].Rows[0]["host_server"].ToString();
                    int.TryParse(ds.Tables[1].Rows[0]["port"].ToString(), out portNo);
                    MailMessage mail = new MailMessage();
                    foreach (var address in mail_id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.To.Add(new MailAddress(address.Trim()));
                    }
                    mail.From = new MailAddress(sendFrom);
                    mail.Subject = msgSubject;
                    string msgBodyHtml = "<b>" + msgHeader + "</b>" + "<br /><br />" + msgBody + "<br /><br />" + msgFooter;
                    mail.Body = msgBodyHtml;

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = smtpHostName;
                    smtp.Port = portNo;
                    smtp.UseDefaultCredentials = useDefaultCrediential;
                    smtp.Credentials = new System.Net.NetworkCredential(sendFrom, senderPwd);
                    smtp.EnableSsl = enableSsl;
                    if (!string.IsNullOrEmpty(mail.Subject))
                        smtp.Send(mail);
                    status = "success" + "," + msgBody;
                }
                return status;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public string ViewEmailAlert(string compId, string brId, string UserId, string docId, string docNo, string status, string mail, string docPdfFilePath)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                                                    objProvider.CreateInitializedParameter("@UserId",DbType.String,UserId),
                                                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,docNo),
                                                    objProvider.CreateInitializedParameter("@Status",DbType.String,status),
                                                    objProvider.CreateInitializedParameter("@DocId",DbType.String,docId),
                                               };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetUsersDetailToSendAlertmailExt", getCstDetails);
                string msgBodyHtml = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string from = ds.Tables[0].Rows[0]["mail_from"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgSubject = ds.Tables[0].Rows[0]["mailSubject"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "");// modified by Suraj on 08-10-2024 to replace <br /> to '' for {Line Break}
                    string msgHeader = ds.Tables[0].Rows[0]["header"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgBody = ds.Tables[0].Rows[0]["body"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    string msgFooter = ds.Tables[0].Rows[0]["footer"].ToString().Replace("\r", "").Replace("\n", "").Replace("{Line Break}", "<br />");
                    //string msgAttachment = "";
                    //msgAttachment = docPdfFilePath;
                    if (status == "A")
                    {
                        string fileName = Path.GetFileName(docPdfFilePath);

                        string Scheme = System.Web.HttpContext.Current.Request.Url.Scheme.ToLower();
                        string Host = System.Web.HttpContext.Current.Request.Url.Host.ToLower();
                        string LocalServerURL = "";
                        string localsysip = System.Configuration.ConfigurationManager.AppSettings["LocalServerip"];
                        if (Host == localsysip)
                            LocalServerURL = System.Configuration.ConfigurationManager.AppSettings["LocalServerURL"];
                        else if (Host == "localhost")
                            LocalServerURL = System.Configuration.ConfigurationManager.AppSettings["LocalServerURL"];
                        else
                            LocalServerURL = System.Configuration.ConfigurationManager.AppSettings["LiveServerURL"];
                        string basePath = LocalServerURL + "/LogsFile/ExternalEmailAlertPDFs/" + fileName;

                        //string path = System.Web.HttpContext.Current.Server.MapPath("~");
                        //mail.Subject = msgSubject;
                        //msgBodyHtml = "<b>" +"From: " + "</b>" + from  + "<br /><br />" + "<b>" + "Recipients: " + "</b>" + mail + "<br /><br />" + "<b>" + "Subject: " + "</b>" + msgSubject + "<br /><br />" + msgHeader + "<br /><br />" + msgBody + "<br /><br />" + msgFooter;
                        msgBodyHtml = "<b>From: </b>" + from + "<br /><br />" +
                   "<b>Recipients: </b>" + mail + "<br /><br />" +
                   "<b>Subject: </b>" + msgSubject + "<br /><br />" +

                   // Email-style attachment block using icon and link
                   "<div style='border:1px solid #ccc; padding:10px; display:inline-block; margin:10px 0;'>" +
                       "<img src='https://cdn-icons-png.flaticon.com/512/337/337946.png' alt='PDF' style='width:24px;height:24px;vertical-align:middle;margin-right:10px;' />" +
                       "<a href='" + basePath + "' target='_blank' style='text-decoration:none;color:#0066cc;font-weight:bold;vertical-align:middle;'>" + fileName + "</a>" +
                   "</div><br /><br />" + msgHeader + "<br /><br />" + msgBody + "<br /><br />" + msgFooter + "<br /><br />";
                    }

                    else
                    {
                        msgBodyHtml = "<b>" + "From: " + "</b>" + from + "<br /><br />" + "<b>" + "Recipients: " + "</b>" + mail + "<br /><br />" + "<b>" + "Subject: " + "</b>" + msgSubject + "<br /><br />" + msgHeader + "<br /><br />" + msgBody + "<br /><br />" + msgFooter;
                    }
                }
                return msgBodyHtml;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public void SendAlertlog(string compId, string brId, string Alert_type, string docNo, string docDate, string docId, string doc_status, string send_dt, char sent_status, string mail_ID, string mail_cont,string file_path)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@compId",DbType.String,compId),
                                                    objProvider.CreateInitializedParameter("@brId",DbType.String,brId),
                                                     objProvider.CreateInitializedParameter("@Alert_type",DbType.String,Alert_type),
                                                    objProvider.CreateInitializedParameter("@docNo",DbType.String,docNo),
                                                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String,docDate),
                                                    objProvider.CreateInitializedParameter("@docId",DbType.String,docId),
                                                    objProvider.CreateInitializedParameter("@doc_status",DbType.String,doc_status),
                                                    objProvider.CreateInitializedParameter("@send_dt",DbType.String,send_dt),
                                                    objProvider.CreateInitializedParameter("@sent_status",DbType.String,sent_status),
                                                    objProvider.CreateInitializedParameter("@mail_ID",DbType.String,mail_ID),
                                                    objProvider.CreateInitializedParameter("@mail_cont",DbType.String,mail_cont),
                                                     objProvider.CreateInitializedParameter("@filepath",DbType.String,file_path),
                                               };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Insert$Ext$Alert$Log", getCstDetails);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataTable EmailAlertLogDetails(string compId, string brId, string docId, string docNo)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                                                    objProvider.CreateInitializedParameter("@DocId",DbType.String,docId),
                                                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,docNo),
                                               };
                DataTable ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Sp_Get$Ext$Alert$Log", getCstDetails).Tables[0];
                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212012";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        //END
        //Added by Nidhi on 01-08-2025
        public DataTable GetSupplierEmail(string CompId, string BrID, string docid, string id, string type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@DocId",DbType.String, docid),
                                                        objProvider.CreateInitializedParameter("@Id",DbType.String, id),
                                                        objProvider.CreateInitializedParameter("@type",DbType.String, type),
                                                      };
                DataTable SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSupplierEmail", prmContentGetDetails).Tables[0];
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet GetOCHSNDL(string Oc_id, string CompId, string br_id, string SuppStateId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@OC_id",DbType.String, Oc_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                                                        objProvider.CreateInitializedParameter("@suppstate",DbType.String, SuppStateId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Common_GetOCHSNDetails", prmContentGetDetails);
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

        public string CheckMailAttch(string compId, string brId, string docid, string mail_event)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                                                    objProvider.CreateInitializedParameter("@Docid",DbType.String,docid),
                                                    objProvider.CreateInitializedParameter("@Event",DbType.String,mail_event),
                                               };
                string attach = string.Empty;
                DataTable ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$email$alert$msg", getCstDetails).Tables[0];
                if(ds.Rows.Count > 0)
                {
                    attach = (ds.Rows[0]["attach"].ToString());
                    return attach;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataSet Cmn_Get_StockGlAccountList(string compId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                                               };
                string attach = string.Empty;
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_Get_stockGlAccountList", getCstDetails);
                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataSet Cmn_Get_ItemAliasList(string compId,string Search)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                                                    objProvider.CreateInitializedParameter("@Search",DbType.String,Search),
                                               };
                string attach = string.Empty;
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_sp_Get_ItemAliasList", getCstDetails);
                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataSet Cmn_GetParameterValues(string comp_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,comp_ID),
                                               };
                string attach = string.Empty;
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_sp_GetParameterValues", getCstDetails);
                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataTable GetItemStockWhLotBatchSerialWise(string compID, string branchID, string itemID)/* Added by Suraj Maurya on 29-12-2025 */
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompID",DbType.String,compID),
                                                    objProvider.CreateInitializedParameter("@BrID",DbType.String,branchID),
                                                    objProvider.CreateInitializedParameter("@Item",DbType.String,itemID),
                                               };
                string attach = string.Empty;
                DataTable ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetItemStockLotBatchSerialwise", getCstDetails).Tables[0];
                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataSet GetCustCommonDropdownDAL(string CompID, string SearchVal, string Stateid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@StateName",DbType.String, SearchVal),
                    objProvider.CreateInitializedParameter("@cityName",DbType.String, SearchVal),
                    objProvider.CreateInitializedParameter("@state_id",DbType.String, Stateid),
                                                    };
                DataSet GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$cust$common$ddl", prmContentGetDetails);
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVerifiedDataOfExcel(string compId, string BrchID, DataTable dt, string ItemName,string Flag)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                SqlDataProvider objProvider = new SqlDataProvider();

                SqlParameter[] prm =
                {
            objProvider.CreateInitializedParameter("@comp_id", DbType.String, compId),
            objProvider.CreateInitializedParameter("@br_id", DbType.String, BrchID),
            objProvider.CreateInitializedParameter("@ItemName", DbType.String, ItemName),
            objProvider.CreateInitializedParameterTableType("@SerialData", SqlDbType.Structured, dt),
            objProvider.CreateInitializedParameter("@Flag", DbType.String, Flag)
        };
                if(Flag== "GRNSerialExcel")
                {
                    return SqlHelper.ExecuteDataset(
                                    CommandType.StoredProcedure,
                                    "Verifydata$Excel$GRN",
                                    prm
                                );
                }
                else if(Flag == "DPISerialExcel" || Flag == "StockTakeSerialExcel" || Flag == "ExtRcptSerialExcel" || Flag == "OpeningRcptSerialExcel")
                {
                    return SqlHelper.ExecuteDataset(
                                   CommandType.StoredProcedure,
                                   "Cmn$Verify$data$ExcelImport",
                                   prm
                               );
                }
                else
                {
                    return null;
                }
            
            }
            catch (SqlException sqlEx)
            {
                // Optional: log sqlEx
                throw;   // preserve stack trace
            }
            catch (Exception ex)
            {
                // Optional: log ex
                throw;   // preserve stack trace
            }
        }

        public DataSet Cmn_getSchemeFocDetail(string compId, string brchID, string item_id, string cust_id, string order_qty, string order_value, string ord_dt)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();

                SqlParameter[] prm =
                {
                    objProvider.CreateInitializedParameter("@comp_id", DbType.String, compId),
                    objProvider.CreateInitializedParameter("@br_id", DbType.String, brchID),
                    objProvider.CreateInitializedParameter("@item_id", DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@order_qty", DbType.String, order_qty),
                    objProvider.CreateInitializedParameter("@order_value", DbType.String, order_value),
                    objProvider.CreateInitializedParameter("@cust_id", DbType.String, cust_id),
                    objProvider.CreateInitializedParameter("@ord_dt", DbType.String, ord_dt)
                };
                return SqlHelper.ExecuteDataset(
                                        CommandType.StoredProcedure,
                                        "Cmn_getSchemeFocDetails",
                                        prm
                                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
