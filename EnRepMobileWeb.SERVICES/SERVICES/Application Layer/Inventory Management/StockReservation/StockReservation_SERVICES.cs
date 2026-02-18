using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.StockReservation;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.StockReservation;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.StockReservation
{
   public class StockReservation_SERVICES:StockReservation_ISERVICES
    {
        public DataSet BindItemName(string CompID, string BrID, string ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_BindItemStockReserve", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataTable GetCustNameList(string CompId, string br_id, string CustomerName)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //            objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, br_id),
        //            objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
        //        };
        //        DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetCustList]", prmContentGetDetails).Tables[0];
        //        return searchmenu;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}
        //public DataTable GetWhList(string CompId, string branch)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //             objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, branch),

        //        };
        //        DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetToWarehouseList", prmContentGetDetails).Tables[0];
        //        return searchmenu;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        public DataSet GetWHAndCustNameAndStkRsrvList(string CompId, string br_id, string CustomerName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SP$StkResrv_GetWHAndCustList]", prmContentGetDetails);
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
        //public Dictionary<string, string> GetCustomerList(string CompID, string EntityName, string BranchID)
        //{
        //    Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
        //            objProvider.CreateInitializedParameter("@CustName",DbType.String, EntityName),                  
        //            objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
        //                                             };
        //        DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetCustList", prmContentGetDetails);
        //        DataRow dr;
        //        dr = PARQusData.Tables[0].NewRow();
        //        dr[0] = "0";
        //        dr[1] = "---Select---";
        //        PARQusData.Tables[0].Rows.InsertAt(dr, 0);

        //        if (PARQusData.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
        //            {
        //                ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["cust_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_name"].ToString());
        //            }
        //        }
        //        return ddlSuppListDic;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}
        public DataSet GetDocumentNo(string CompID, string BrchID, string Entity_id, string Itm_ID,string wh_id, string Type)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Entity_id",DbType.String, Entity_id),
                     objProvider.CreateInitializedParameter("@Itm_ID",DbType.String, Itm_ID),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.String, wh_id),
                    objProvider.CreateInitializedParameter("@Type",DbType.String, Type),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetDocNoOnStockReserve]", prmContentGetDetails);
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
        public DataSet GetStock(string CompID, string BrchID, string item_id, string wh_id)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Item",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@Warehouse",DbType.String, wh_id),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetStockDetailforStockReserve]", prmContentGetDetails);
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
        public DataSet GetBatchSerialDetail(string CompID, string BrchID, string item_id, string wh_id, string lot_id)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Item",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@Warehouse",DbType.String, wh_id),
                      objProvider.CreateInitializedParameter("@lot_id",DbType.String, lot_id),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetBatchDetailLotwise]", prmContentGetDetails);
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
        public DataSet GetBatchSerialAvalStock(string CompID, string BrchID, string item_id, string wh_id, string lot_id, string BatchNo)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Item",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@Warehouse",DbType.String, wh_id),
                      objProvider.CreateInitializedParameter("@lot_id",DbType.String, lot_id),
                      objProvider.CreateInitializedParameter("@BatchNo",DbType.String, BatchNo),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetBatchSerailAvalilableStock]", prmContentGetDetails);
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
        public DataSet GetDocdetail(string CompID, string BrchID, string WhID, string item_id, string Docno)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int32, BrchID),
                    objProvider.CreateInitializedParameter("@Whid",DbType.Int32, WhID),
                    objProvider.CreateInitializedParameter("@Item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@Docno",DbType.String, Docno),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetItemDetailByDocNo]", prmContentGetDetails);
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
        public string InsertStockReserve(DataTable StockReserve,DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, StockReserve ),              
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),              
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string stkres = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_InsertStockReserve_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[2].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[2].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetStockReservationList(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                      objProvider.CreateInitializedParameter("@BrchID",DbType.String,BrchID),                      
                                                     };
                DataTable GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetStockReserveList", prmContentGetDetails).Tables[0];
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
        public DataSet GetReservedItemDetail(string CompID, string BrchID, string ItemID, string wh_id,string flag, string entity_id, string docno)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int32, BrchID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                      objProvider.CreateInitializedParameter("@wh_id",DbType.Int32, wh_id),
                      objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                      objProvider.CreateInitializedParameter("@entityid",DbType.String, entity_id),
                      objProvider.CreateInitializedParameter("@docno",DbType.String, docno),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetReserveditemDetail]", prmContentGetDetails);
            return ds;
        }

        public DataSet StockRes_GetSubItemDetails(string compID, string brchID, string item_id, string wh_id, string doc_type
            , string cust_id, string doc_no, string doc_dt, string flag, string transType)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brchID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.String,wh_id),
                    objProvider.CreateInitializedParameter("@doc_type",DbType.String, doc_type),
                    objProvider.CreateInitializedParameter("@cust_id",DbType.String, cust_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                    objProvider.CreateInitializedParameter("@transType",DbType.String, transType),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stockreservation_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id, string uomId, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.Int32, wh_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@UomId",DbType.String, uomId),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MISStk_GetSubItemAvlStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
