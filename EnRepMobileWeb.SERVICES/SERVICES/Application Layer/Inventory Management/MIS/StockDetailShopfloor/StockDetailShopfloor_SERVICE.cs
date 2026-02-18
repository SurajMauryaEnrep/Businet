using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetailShopfloor;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.StockDetailShopfloor
{
    public class StockDetailShopfloor_SERVICE: StockDetailShopfloor_ISERVICE
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
        public DataSet GetShopfloorList(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$shfl$setup_GetShopfloorList", prmContentGetDetails);
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
        public DataSet GetStockDetailShopfloorList(string CompId, string BranchID, string IncludeZeroStockFlag, string StockBy
            , string ItemId, string ItemGrpId, string ShflID, string UpToDate,string PortfolioId, string hsnCode
            , int skip, int pageSize, string searchValue, string sortColumn, string sortColumnDir, string BranchIDList, string flag)
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
                    objProvider.CreateInitializedParameter("@Shfl_ID",DbType.String,ShflID),
                    objProvider.CreateInitializedParameter("@UpTodate",DbType.String, UpToDate),
                    objProvider.CreateInitializedParameter("@PortfolioId",DbType.String, PortfolioId),
                    objProvider.CreateInitializedParameter("@HsnCode",DbType.String, hsnCode),
                    objProvider.CreateInitializedParameter("@BrList",DbType.String,BranchIDList),
                    objProvider.CreateInitializedParameter("@Skip",DbType.String,skip),
                    objProvider.CreateInitializedParameter("@PageSize",DbType.String,pageSize),
                    objProvider.CreateInitializedParameter("@Search",DbType.String,searchValue),
                    objProvider.CreateInitializedParameter("@sortColumn",DbType.String,sortColumn),
                    objProvider.CreateInitializedParameter("@sortColumnDir",DbType.String,sortColumnDir),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String,flag)
                                                      };
                //DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$shfl$stk$detail_GetStockDeatilList", prmContentGetDetails);
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$shfl$stk$detail_GetStockDeatilList1", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetItemReceivedList(string CompId, string BranchID, string TransType, string StockBy, string ItemId, string @ShflID, string Lot, string BatchNo, string SerialNo, string UpToDate)
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
                    objProvider.CreateInitializedParameter("@ShflID",DbType.String, @ShflID),
                    objProvider.CreateInitializedParameter("@LotNo",DbType.String, Lot),
                    objProvider.CreateInitializedParameter("@BatchNo",DbType.String, BatchNo),
                    objProvider.CreateInitializedParameter("@SerialNo",DbType.String, SerialNo),
                    objProvider.CreateInitializedParameter("@UpToDate",DbType.String, UpToDate),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$shfl$stk$detail_GetStockReceivedDeatilList", prmContentGetDetails);
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
        public DataSet GetSubItemStkList(string CompId, string BranchID, string StockBy, string ItemId, string WarehouseID, string UpToDate,string IncludeZero)
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$shfl$stk$detail$subitem_GetSubitemStockDeatilList", prmContentGetDetails);
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
