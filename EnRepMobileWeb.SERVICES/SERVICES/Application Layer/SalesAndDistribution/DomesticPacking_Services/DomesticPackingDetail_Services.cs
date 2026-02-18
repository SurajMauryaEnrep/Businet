using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.DomesticPackingIServices;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticPacking;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.DomesticPacking_Services
{
     public class DomesticPacking_Services : DomesticPacking_IServices
    {
        public Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID,string CustType,string DocId)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@CustName",DbType.String, Cust_Name),
                             objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                             objProvider.CreateInitializedParameter("@DocId",DbType.String, DocId),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetCustList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["cust_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_name"].ToString());
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
           
        }
        public DataSet getPackingListSONo(string CompID, string BrchID, string Cust_id, string curr_Id)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Cust_id",DbType.String, Cust_id),
                    objProvider.CreateInitializedParameter("@Curr_Id",DbType.String, curr_Id),
                    objProvider.CreateInitializedParameter("@SONumber",DbType.String, ""),                         
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$PackingList$GetOrderNumber]", prmContentGetDetails);
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet getDetailByOrderNo(string CompID, string BrchID, string OrderNumber,string PackingNumber,string PackType,string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@OrderNumber",DbType.String, OrderNumber),
                     objProvider.CreateInitializedParameter("@PackingNumber",DbType.String, PackingNumber),
                     objProvider.CreateInitializedParameter("@PackType",DbType.String, PackType),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$Packing$GetDetailByOrderNo]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetOrderQty(string CompID, string BrchID, string custtomerID, string ItemID, string PackType, string DocumentMenuId, string packingNo)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@custtomerID",DbType.String, custtomerID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                     objProvider.CreateInitializedParameter("@PackType",DbType.String, PackType),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                     objProvider.CreateInitializedParameter("@packingNo",DbType.String, packingNo),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$stp$Packing$GetOrderQtyAndBalanceQty]", prmContentGetDetails);
            return ds;
        }
        public DataSet getPackagingItemDetails(string CompID, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                      };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetPackagingDetail", prmContentGetDetails);
            return ds;
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

        public String InsertUpdatePackingList(DataTable PackingListHeader, DataTable PackingListItemDetails
            , DataTable PackingListSoItemDetails
            , DataTable PackingListItemBatchDetails, DataTable PackingListOrdResItemBatchDetails
            , DataTable PackingListItemSerialDetails,DataTable PL_SerializationDetails
            , DataTable dtSubItem,DataTable dtSubItemRes, DataTable dtSubItemPackRes)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, PackingListHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, PackingListItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@SoItemDetail",SqlDbType.Structured, PackingListSoItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemBatchDetail",SqlDbType.Structured, PackingListItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@OrdResItemBatchDetail",SqlDbType.Structured, PackingListOrdResItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemSerialDetail",SqlDbType.Structured, PackingListItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@PackItemSerializationDetail",SqlDbType.Structured, PL_SerializationDetails ),
                objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                objprovider.CreateInitializedParameterTableType("@SubItemResDetail",SqlDbType.Structured, dtSubItemRes ),
                objprovider.CreateInitializedParameterTableType("@SubItemPackResDetail",SqlDbType.Structured, dtSubItemPackRes ),

                };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sls$pack$InsertUpdate", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[6].Value != DBNull.Value) // status
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
        public String PackingListCancel(DomesticPackingDetail_Model _DomesticPackingDetail_Model, string comp_id, string userid, string br_id, string mac_id,string MenuDocid,string Amendment)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16,comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                    objProvider.CreateInitializedParameter("@pack_no",DbType.String,_DomesticPackingDetail_Model.pack_no),
                    objProvider.CreateInitializedParameter("@pack_dt",DbType.Date,_DomesticPackingDetail_Model.pack_dt),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String,userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@menudocid_id",DbType.String,MenuDocid),
                    objProvider.CreateInitializedParameter("@flag",DbType.String,Amendment),
                    objProvider.CreateInitializedParameter("@cancel_remarks",DbType.String,_DomesticPackingDetail_Model.CancelledRemarks),
                     };
                string pack_no = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_stp$PackingListCancel", prmContentGetDetails).ToString();
                return pack_no; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet PackingListDelete(DomesticPackingDetail_Model _DomesticPackingDetail_Model, string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@pack_no",DbType.String, _DomesticPackingDetail_Model.pack_no),
                    objProvider.CreateInitializedParameter("@pack_dt",DbType.Date,  _DomesticPackingDetail_Model.pack_dt),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$PackingListDelete", prmContentGetDetails);
                return ImageDeatils;
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
        public DataSet GetPackingListAll(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                         objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                         objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$PackingList$details]", prmContentGetDetails);
            return dt;
        }
        public DataTable GetCustNameList(string CompId, string br_id, string CustomerName,string cus_typ,string DocId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, cus_typ),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, DocId),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetCustList", prmContentGetDetails).Tables[0];
                //DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetCustList]", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllData(string CompId, string br_id, string CustomerName,string cus_typ, string CustID, string Fromdate, string Todate, string Status, string DocumentMenuId, string UserID, string wfstatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                       objProvider.CreateInitializedParameter("@CustType",DbType.String, cus_typ),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, br_id),
                            objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                  objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                 objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                         objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                       
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetAllData$Packing$list]", prmContentGetDetails);
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
        public DataTable GetPackingListFilter(string CustID, DateTime Fromdate, DateTime Todate, string Status, string CompID, string BrchID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.DateTime, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.DateTime, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
                                                     };
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$PackingList$detailsFilter]", prmContentGetDetails).Tables[0];
            return dt;
        }
        public DataSet GetPackingListDetailByNo(string CompID, string pack_no, string BrchID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@pack_no",DbType.String, pack_no),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$pack$detail$DetailByBNo", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string PackingListApprove(DomesticPackingDetail_Model _DomesticPackingDetail_Model, string comp_id,string DocumentMenuID, string br_id,string mac_id, string A_Status, string A_Level, string A_Remarks)
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
                    objProvider.CreateInitializedParameter("@pack_no",DbType.String,  _DomesticPackingDetail_Model.pack_no),
                    objProvider.CreateInitializedParameter("@pack_dt",DbType.Date,  _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd")),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _DomesticPackingDetail_Model.CreatedBy ),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@stkstatus",DbType.String,""),
                };
                prmContentGetDetails[10].Size = 100;
                prmContentGetDetails[10].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_stp$PackingListApprove", prmContentGetDetails).ToString();
                
                string msg = string.Empty;
                if (prmContentGetDetails[10].Value != DBNull.Value) // status
                {
                    msg = prmContentGetDetails[10].Value.ToString();
                }
                
                //DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$PackingListApprove", prmContentGetDetails);
                return msg; 
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet CheckShipmentAgainstPackingList(string CompId, string BrchID, string DocNo, string DocDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, DocDate),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$pack$detail_CheckShipmentAgainstPackingList", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemStockBatchWiseAfterInsert(string CompID, string BrID, string Type, string PL_No, string PL_Date, string ItemId,string docid)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@PackType",DbType.String,  Type),
            objProvider.CreateInitializedParameter("@PackNo",DbType.String,  PL_No),
            objProvider.CreateInitializedParameter("@PackDate",DbType.String,  PL_Date),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            objProvider.CreateInitializedParameter("@docid",DbType.String,  docid),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$pack$item$bt$detail_Get_Item_StockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockSerialWiseAfterInsert(string CompID, string BrID, string Pl_Type, string Pl_No, string Pl_Date, string ItemId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@PLType",DbType.String,  Pl_Type),
            objProvider.CreateInitializedParameter("@PLNo",DbType.String,  Pl_No),
            objProvider.CreateInitializedParameter("@PLDate",DbType.String,  Pl_Date),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$pack$item$sr$detail_GetItem_StocksSerialwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId, string Doclist)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@DocNos",DbType.String,  Doclist),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetDocItemStockBatchwise]", prmContentGetDetails);
            return DS;
        }
        public DataSet GetOrderResItemStockBatchWise(string CompId, string BranchId, string ItemId, string WarehouseId, string LotId, string BatchId, string Doclist)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
                 objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
           objProvider.CreateInitializedParameter("@LotNo",DbType.String, LotId),
           objProvider.CreateInitializedParameter("@BatchNo",DbType.String, BatchId),
            objProvider.CreateInitializedParameter("@DocNos",DbType.String,  Doclist),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetOrderResItemStockBatchwise", prmContentGetDetails);
            return DS;
        }

        public DataSet GetOrderResItemStockForSubItemBatchWise(string CompId, string BranchId, string ItemId
            , string WarehouseId, string Doclist,string status)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
                 objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@DocNos",DbType.String,  Doclist),
            objProvider.CreateInitializedParameter("@status",DbType.String,  status),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetOrderResItemStockForSubItemBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet GetOrderResItemStockBatchWiseAfterInsert(string CompId, string BranchId, string packtype, string Packno, string Packdt, string ItemId, string LotId, string BatchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
                 objProvider.CreateInitializedParameter("@Comp_ID",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String,  BranchId),
             objProvider.CreateInitializedParameter("@PackType",DbType.String,  packtype),
            objProvider.CreateInitializedParameter("@PackNo",DbType.String, Packno),
             objProvider.CreateInitializedParameter("@PackDate",DbType.String,  Packdt),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemId),
           objProvider.CreateInitializedParameter("@LotNo",DbType.String, LotId),
           objProvider.CreateInitializedParameter("@BatchNo",DbType.String, BatchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$pack$item$res$detail_GetOrderResItemStockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet GetPackingListDeatilsForPrint(string CompId, string BrchID, string DocNo, string DocDate,string printFormet)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@pack_no",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@pack_date",DbType.String, DocDate),
                                                        objProvider.CreateInitializedParameter("@printFormet",DbType.String, printFormet),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPackingListDeatils_ForPrint", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string SoNo, string SoDate, string Flag)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@PackingNo",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@PackingDt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@So_no",DbType.String,SoNo),
                    objProvider.CreateInitializedParameter("@So_dt",DbType.String, SoDate),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Packing_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemDetailsBySO(string CompID, string Br_id, string doc_no, string doc_dt, string SoNo, string SoDate, string ItemId, string pack_type)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@So_no",DbType.String,SoNo),
                    objProvider.CreateInitializedParameter("@So_dt",DbType.String, SoDate),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@PackingNo",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@PackingDt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@PackTyp",DbType.String, pack_type)

                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PakingList_GetSubItemDetailsBySO", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetOrdrPendSubItemDetailsBySO(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt
            , string SoNo, string SoDate,string wh_id, string flag)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@So_no",DbType.String,SoNo),
                    objProvider.CreateInitializedParameter("@So_dt",DbType.String, SoDate),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@PackingNo",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@PackingDt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.String, wh_id),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag)

                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "OrdrPending_GetSubItemDetailsBySO", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public DataSet GetOrdrPendSubItemDetailsBySOAfterApprov(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string SoNo, string SoDate, string pack_type)
        //{
        //    try
        //    {

        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
        //            objProvider.CreateInitializedParameter("@BrchID",DbType.Int32, Br_id),
        //            objProvider.CreateInitializedParameter("@So_no",DbType.String,SoNo),
        //            objProvider.CreateInitializedParameter("@So_dt",DbType.String, SoDate),
        //            objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
        //            objProvider.CreateInitializedParameter("@PackingNo",DbType.String,doc_no),
        //            objProvider.CreateInitializedParameter("@PackingDt",DbType.String, doc_dt),
        //            objProvider.CreateInitializedParameter("@PackTyp",DbType.String, pack_type)

        //                                             };
        //        DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "OrdrPending_GetSubItemDetailsBySOAfterApprov", prmContentGetDetails);
        //        return Getsuppport;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet checkorderqtymorethenpackingqty(string CompID, string BrchID)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),                 
                  // objProvider.CreateInitializedParameter("@SONumber",DbType.String, ""),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[check$parameter$packinglist]", prmContentGetDetails);
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable GetPLDetailsToExportExcel(string action, string compId, string branchId, string documentMenuId, string userId, string packNo)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    /*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Action",DbType.String, action),
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, branchId),                 
                    objProvider.CreateInitializedParameter("@pack_no",DbType.String, packNo),                 
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, userId),                 
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, documentMenuId),                 
                  
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetPackingItemsToExportExcel", prmContentGetDetails);
                return PARQusData.Tables[0];

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable GetCurrencies(string comp_ID, string currType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    /*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, comp_ID),
                    objProvider.CreateInitializedParameter("@Currtype",DbType.String, currType),                  
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Get$Currencies", prmContentGetDetails);
                return PARQusData.Tables[0];

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet GetVerifiedDataOfExcel(string compId, string BrchID, DataTable ExcelLevelData, DataTable PageLevelData)
        {
            try
            {
                if (ExcelLevelData == null || ExcelLevelData.Rows.Count == 0)
                    return null;

                SqlDataProvider objProvider = new SqlDataProvider();

                SqlParameter[] prm =
                {
            objProvider.CreateInitializedParameter("@comp_id", DbType.String, compId),
            objProvider.CreateInitializedParameter("@br_id", DbType.String, BrchID),
            objProvider.CreateInitializedParameterTableType("@ExcelLevelData", SqlDbType.Structured, ExcelLevelData),
            objProvider.CreateInitializedParameterTableType("@PageLevelData", SqlDbType.Structured, PageLevelData),
          
        };
            
                    return SqlHelper.ExecuteDataset(
                                   CommandType.StoredProcedure,
                                   "sls$Pck$Seriali$Dtl$Verify$data$ExcelImport",
                                   prm
                               );
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
    }
}
    

