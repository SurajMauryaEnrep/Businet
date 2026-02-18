using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.MODELS.BusinessLayer.GLDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.SecurityLayer.UserSetup;
using EnRepMobileWeb.MODELS.SecurityLayer.UserSetup;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EnRepMobileWeb.SERVICES.SERVICES.SecurityLayer.UserSetup
{
   public class UserList_SERVICES : UserList_ISERVICES
    {
        public DataTable GetUserListDAL()
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                                                     };
                DataTable GetUserList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$UserList", prmContentGetDetails).Tables[0];
                return GetUserList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GettopUser(string Comp_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
                                                      };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sec$$User$detail$get$tree$data$top", prmContentGetDetails).Tables[0];
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
