using EnRepMobileWeb.SERVICES.ISERVICES.PushNotifications;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.PushNotifications
{
    public class Notification_Service : Notification_IService
    {
        public DataTable GetAllUnreadNotifications(int companyId, int branchId, int userId)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompId",DbType.Int32,companyId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int32,branchId),
                    objProvider.CreateInitializedParameter("@UserId",DbType.Int32,userId),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetUnreadNotification", prmContentGetDetails).Tables[0];
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
        public int UpdateReadStatus(string rowId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@rowNo",DbType.Int32,rowId),
                };
                int searchmenu = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UpdateReadStatus", prmContentGetDetails);
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
