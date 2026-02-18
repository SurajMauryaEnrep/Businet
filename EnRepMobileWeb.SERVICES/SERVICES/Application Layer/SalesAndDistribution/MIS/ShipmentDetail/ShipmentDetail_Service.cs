using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.ShipmentDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.MIS.ShipmentDetail
{
    public class ShipmentDetail_Service : ShipmentDetail_IService
    {
        public DataTable GetItemsList(string compId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),

                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetItemsList", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetShipmentMISDetailsReport(string compId, string brId, string fromDate, string toDate, string shipmentType,
            string customerId, string ItemId, string showAs)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                 objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                 objProvider.CreateInitializedParameter("@ShipmentType",DbType.String,shipmentType),
                 objProvider.CreateInitializedParameter("@CustomerId",DbType.String,customerId),
                 objProvider.CreateInitializedParameter("@ItemId",DbType.String,ItemId),
                 objProvider.CreateInitializedParameter("@ShowAs",DbType.String,showAs),

                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetShipmentMISReport", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
