using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISStockReservation;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.MISStockReservation
{
    public class MISStockReservation_Service : MISStockReservation_IService
    {
        public DataTable GetStockReservationReport(string compId, string brId, string itemId, string itemGroupId, string warehouseId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                                                        objProvider.CreateInitializedParameter("@ItemGroup",DbType.String, itemGroupId),
                                                        objProvider.CreateInitializedParameter("@WareHouse",DbType.String, warehouseId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_MIS_GetStockReservationReport", prmContentGetDetails);
                return GetPODetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
