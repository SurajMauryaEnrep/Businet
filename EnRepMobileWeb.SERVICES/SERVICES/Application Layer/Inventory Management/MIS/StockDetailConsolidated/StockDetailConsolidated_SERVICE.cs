using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetailConsolidated;
using EnRepMobileWeb.UTILITIES;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.StockDetailConsolidated
{
    public class StockDetailConsolidated_SERVICE : StockDetailConsolidated_ISERVICE
    {
        public DataTable GetItemsList(string GroupName, string CompID)
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
                DataSet dsitemgroup = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetAllItemNameList_ForStock", prmContentGetDetails);
                if (dsitemgroup.Tables.Count > 0)
                    return dsitemgroup.Tables[0];
                else
                    return new DataTable();

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
        public DataTable GetItemsGroupList(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataSet dsitemgroup = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$grp_GetAllItemGroupNoodChilds", prmContentGetDetails);
                if (dsitemgroup.Tables.Count > 0)
                    return dsitemgroup.Tables[0];
                else
                    return new DataTable();
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
        public DataSet GetStockDetailConsolidatedList(string compId, string brId, string itemId, string itemGroupId, string asOnDate
            , string flag, int skip, int pageSize, string searchValue, string sortColumn, string sortColumnDir,string CsvFlag)
        {
            try
            {
                #region code modified by sanjay pant for query optimization 
                #endregion
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                        objProvider.CreateInitializedParameter("@ItemGroupId",DbType.String, itemGroupId),
                        objProvider.CreateInitializedParameter("@AsOnDate",DbType.String, asOnDate),
                        objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                        objProvider.CreateInitializedParameter("@Skip",DbType.String,skip),
                        objProvider.CreateInitializedParameter("@PageSize",DbType.String,pageSize),
                        objProvider.CreateInitializedParameter("@Search",DbType.String,searchValue),
                        objProvider.CreateInitializedParameter("@sortColumn",DbType.String,sortColumn),
                        objProvider.CreateInitializedParameter("@sortColumnDir",DbType.String,sortColumnDir),
                        objProvider.CreateInitializedParameter("@CsvFlag",DbType.String,CsvFlag),
                                                      };
                //DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetStockDetailsconsolidated", prmContentGetDetails);
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetStockDetailsconsolidated1", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetWipStockDetailConsolidatedList(string compId, string brId, string itemId, string itemGroupId, string asOnDate, string flag)
        {
            try
            {
                #region code modified by sanjay pant for query optimization 
                #endregion
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                                                        objProvider.CreateInitializedParameter("@ItemGroupId",DbType.String, itemGroupId),
                                                        objProvider.CreateInitializedParameter("@AsOnDate",DbType.String, asOnDate),
                                                        objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                                                      };
                //DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetStockDetailsconsolidated", prmContentGetDetails);
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetStockDetailsconsolidated1", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemStockDetails(string comp_ID, string br_ID, string item_id, string flag, string AsOnDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    //objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    objProvider.CreateInitializedParameter("@AsOnDate",DbType.String,AsOnDate ),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_StkCons_GetSubItemStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


    }
}
