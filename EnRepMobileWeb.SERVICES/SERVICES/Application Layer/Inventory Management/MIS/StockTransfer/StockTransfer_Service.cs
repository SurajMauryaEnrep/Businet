using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockTransfer;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.StockTransfer
{
    public class StockTransfer_Service : StockTransfer_IService
    {
       
        public DataTable GetItemsList(string compId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, compId)
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetItemsListCompanyWise", prmContentGetDetails);
                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
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
        }
        public DataTable GetStockTransferReport(string compId, string brId, string itemId, string mtType, string srcBranch, string dstnBranch, string srcWarehouse, string dstnWarehouse, string fromDate, string toDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                     objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                     objProvider.CreateInitializedParameter("@MTType",DbType.String, mtType),
                     objProvider.CreateInitializedParameter("@SrcBranch",DbType.String, srcBranch),
                     objProvider.CreateInitializedParameter("@DstnBranch",DbType.String, dstnBranch),
                     objProvider.CreateInitializedParameter("@SrcWareHouse",DbType.String, srcWarehouse),
                     objProvider.CreateInitializedParameter("@DstnWareHouse",DbType.String, dstnWarehouse),
                     objProvider.CreateInitializedParameter("@FromDate",DbType.String, fromDate),
                     objProvider.CreateInitializedParameter("@ToDate",DbType.String, toDate),
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetStockTransferMIS", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetBranchAndWarehouseList(string compId, string brId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, brId)
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSrcBranchAndSrcWareHouse", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable GetUomByItemId(string compId, string itemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                     objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId)
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetUomByItemId", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable GetStockTransferPopupData(string actflag,string compId, string brId, string itemId, string mtType, string srcBranch, string dstnBranch, string srcWarehouse, string dstnWarehouse, string fromDate, string toDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@Action",DbType.String, actflag),
                     objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                     objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                     objProvider.CreateInitializedParameter("@MTType",DbType.String, mtType),
                     objProvider.CreateInitializedParameter("@SrcBranch",DbType.String, srcBranch),
                     objProvider.CreateInitializedParameter("@DstnBranch",DbType.String, dstnBranch),
                     objProvider.CreateInitializedParameter("@SrcWareHouse",DbType.String, srcWarehouse),
                     objProvider.CreateInitializedParameter("@DstnWareHouse",DbType.String, dstnWarehouse),
                     objProvider.CreateInitializedParameter("@FromDate",DbType.String, fromDate),
                     objProvider.CreateInitializedParameter("@ToDate",DbType.String, toDate),
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetStockTransferMISPopupData", prmContentGetDetails);
                return ds.Tables[0];
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
