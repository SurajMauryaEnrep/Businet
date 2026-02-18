using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;


namespace EnRepMobileWeb.SERVICES.SERVICES
{
    public class LOGIN_SERVICES : LOGIN_ISERVICES
    {
        public int ChangePassword(string userName, string newPassword)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@UserName",DbType.String, userName),
                                                        objProvider.CreateInitializedParameter("@NewPassword",DbType.String, newPassword),
                                                    };
                int searchmenu = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ChangePassword", prmContentGetDetails);
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

        public DataSet GetValidUser(string userName,string Comp_id,string BranchId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@userName",DbType.String, userName),
                                                        objProvider.CreateInitializedParameter("@Comp_id",DbType.String, Comp_id),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BranchId),
                                                    };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "spGetValidUser", prmContentGetDetails);
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
        public DataSet ValidateUserToForgetPassword(string userName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    objProvider.CreateInitializedParameter("@UserName",DbType.String,userName)
                 };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateUserToForgetPassword", prmContentGetDetails);
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
