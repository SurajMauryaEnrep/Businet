using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockMovement;
using EnRepMobileWeb.UTILITIES;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.StockMovement
{
    public class StockMovement_SERVICE: StockMovement_ISERVICE
    {
        public DataSet BindAllDropdownLists(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                };

                DataSet alldata_Dset = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetItem_List", prmContentGetDetails);
                return alldata_Dset;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetBatch_Lists(string CompID,string ItemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                };
                DataSet BatchList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetBatch_List", prmContentGetDetails);
                return BatchList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable GetMovementList(string CompID, string BrID, string MoveType,string ItemId,string BatchNo,string Serial_no, string Fromdt,string Todt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                     objProvider.CreateInitializedParameter("@move_type",DbType.String, MoveType),
                     objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                     objProvider.CreateInitializedParameter("@batch_no",DbType.String, BatchNo),
                     objProvider.CreateInitializedParameter("@Serial_no",DbType.String, Serial_no),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String, Fromdt),
                     objProvider.CreateInitializedParameter("@to_dt",DbType.String, Todt),
                     objProvider.CreateInitializedParameter("@lang",DbType.String, ""),
                };
                DataTable BatchList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetItemMovementDeatil", prmContentGetDetails).Tables[0];
                return BatchList;
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
