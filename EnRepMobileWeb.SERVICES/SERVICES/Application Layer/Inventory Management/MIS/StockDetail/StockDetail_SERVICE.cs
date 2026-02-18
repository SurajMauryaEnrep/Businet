using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.StockDetail
{
    public class StockDetail_SERVICE: StockDetail_ISERVICE
    {
        public Dictionary<string, string> ItemList(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetAllItemNameList_ForStock", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["Item_id"].ToString(), PARQusData.Tables[0].Rows[i]["Item_name"].ToString());
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

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$grp_GetAllItemGroupNoodChilds", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
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
        public DataSet GetAllBrchList(string CompID,string User_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@User_id",DbType.String,User_id)
                 };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$detail_GetBrchList", prmContentGetDetails);
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
        public DataTable GetStockDetailList(string CompId, string BranchID, string IncludeZeroStockFlag, string StockBy, string ItemId
            , string ItemGrpId, string WarehouseID, string UpToDate,string PortfolioId, string hsnCode,string ExpiredItems
            ,string StockoutItems,string NearExpiryItm,string StkGlAccId,string ItemAlias,string BranchIDList, string Supp_Name)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@IncludeZeroStockFlag",DbType.String, IncludeZeroStockFlag),
                                                        objProvider.CreateInitializedParameter("@StockBy",DbType.String, StockBy),
                                                        objProvider.CreateInitializedParameter("@Item",DbType.String, ItemId),
                                                        objProvider.CreateInitializedParameter("@ItemGroup",DbType.String,ItemGrpId),
                                                        objProvider.CreateInitializedParameter("@Warehouse",DbType.String, WarehouseID),
                                                        objProvider.CreateInitializedParameter("@UpTodate",DbType.String, UpToDate),
                                                        objProvider.CreateInitializedParameter("@PortfolioId",DbType.String, PortfolioId),
                                                        objProvider.CreateInitializedParameter("@HsnCode",DbType.String, hsnCode),
                                                        objProvider.CreateInitializedParameter("@ExpiredItems",DbType.String, ExpiredItems),
                                                        objProvider.CreateInitializedParameter("@StockoutItems",DbType.String, StockoutItems),
                                                        objProvider.CreateInitializedParameter("@NearExpiryItm",DbType.String, NearExpiryItm),
                                                        objProvider.CreateInitializedParameter("@StkGlAccId",DbType.String, StkGlAccId),
                                                        objProvider.CreateInitializedParameter("@ItemAlias",DbType.String, ItemAlias),
                                                        objProvider.CreateInitializedParameter("@BrList",DbType.String, BranchIDList),
                                                        objProvider.CreateInitializedParameter("@Supp_Name",DbType.String, Supp_Name),
                                                      };
                //DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetStockDeatilList", prmContentGetDetails);
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetStockDeatilList1", prmContentGetDetails);
                return GetPODetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetItemReceivedList(string CompId, string BranchID, string TransType, string StockBy, string ItemId, string WarehouseID, string Lot, string BatchNo, string SerialNo, string UpToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@TransType",DbType.String, TransType),
                     objProvider.CreateInitializedParameter("@StockBy",DbType.String, StockBy),
                    objProvider.CreateInitializedParameter("@Item",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@Warehouse",DbType.String, WarehouseID),
                    objProvider.CreateInitializedParameter("@LotNo",DbType.String, Lot),
                    objProvider.CreateInitializedParameter("@BatchNo",DbType.String, BatchNo),
                    objProvider.CreateInitializedParameter("@SerialNo",DbType.String, SerialNo),
                    objProvider.CreateInitializedParameter("@UpToDate",DbType.String, UpToDate),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetStockReceivedDeatilList", prmContentGetDetails);
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
        public Dictionary<string, string> ItemPortfolioList(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@PortfName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetItemPortfolioList]", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["setup_id"].ToString(), PARQusData.Tables[0].Rows[i]["setup_val"].ToString());
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
        public DataSet GetSubItemStkList(string CompId, string BranchID,string StockBy, string ItemId, string WarehouseID,string UpToDate,string IncludeZero)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BranchID),
                     objProvider.CreateInitializedParameter("@StockBy",DbType.String, StockBy),
                    objProvider.CreateInitializedParameter("@Item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@Warehouse",DbType.String, WarehouseID),
                    objProvider.CreateInitializedParameter("@UpTodate",DbType.String, UpToDate),
                    objProvider.CreateInitializedParameter("@IncludeZero",DbType.String, IncludeZero),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail$subitem_GetSubitemStockDeatilList", prmContentGetDetails);
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
        public DataSet Get_SuppNameDetails(string comp_id, string Br_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Br_ID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_SuppNameData", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
