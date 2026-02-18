using EnRepMobileWeb.SERVICES.ISERVICES.SecurityLayer.UserRoleSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.SecurityLayer.UserRoleSetup
{
   public class UserRoleSetup_SERVICE: UserRoleSetup_ISERVICE
    {
        public DataSet GetMenuList_UserRoleSetup(string CompID)
        {
            string PageName = string.Empty;
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
            };
            DataSet RoleMenuList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_GetRoleMenuList", prmContentGetDetails);
            return RoleMenuList;

        }
        public DataTable GetHoDetail()
        {
            try
            {
                DataSet HoList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$Comp$detail_GetHoListAll");
                return HoList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable GetUserRoleList_Detail(string comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompID",DbType.String, comp_id),
            };
                DataSet HoList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sec$role$detail_GetUserRoleListAll", prmContentGetDetails);
                return HoList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string InsertUserRoleSetupDetails(DataTable UserRoleSetupDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameterTableType("@UserRoleSetupDetail",SqlDbType.Structured, UserRoleSetupDetail ),
                 objprovider.CreateInitializedParameterTableType("@RoleID",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[1].Size = 100;
                prmcontentaddupdate[1].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sec$role$detail_InsertUserRoleSetup_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[1].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[1].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet GetUserRoleDetail(string CompID, string userrole_no)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@userrole_no",DbType.String, userrole_no),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sec$role$detail_GetUserRoleSetupDetail", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public string Delete_UserRoleDetail(string CompID, string userrole_no)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameter("@comp_id",DbType.String, CompID ),
                 objprovider.CreateInitializedParameter("@role_id",DbType.String, userrole_no ),
                 objprovider.CreateInitializedParameter("@DelStatus",DbType.String,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string DeleteStatus = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sec$role$detail_DeleteUserRoleSetupDetail", prmcontentaddupdate).ToString();

                string Delete_Status = string.Empty;
                if (prmcontentaddupdate[2].Value != DBNull.Value) // status
                {
                    Delete_Status = prmcontentaddupdate[2].Value.ToString();
                }
                return Delete_Status;   
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        //public string CheckDuplicate_UserRoleName(string CompID, string role_name)
        //{
        //    try
        //    {
        //        SqlDataProvider objprovider = new SqlDataProvider();
        //        SqlParameter[] prmcontentaddupdate = {
        //         objprovider.CreateInitializedParameter("@comp_id",DbType.String, CompID ),
        //         objprovider.CreateInitializedParameter("@role_name",DbType.String, role_name ),
        //         objprovider.CreateInitializedParameter("@DulipcateStatus",DbType.String,""),
        //        };
        //        prmcontentaddupdate[2].Size = 100;
        //        prmcontentaddupdate[2].Direction = ParameterDirection.Output;

        //        string DeleteStatus = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sec$role$detail_CheckDuplicateUserRoleName", prmcontentaddupdate).ToString();

        //        string Delete_Status = string.Empty;
        //        if (prmcontentaddupdate[2].Value != DBNull.Value) // status
        //        {
        //            Delete_Status = prmcontentaddupdate[2].Value.ToString();
        //        }
        //        return Delete_Status;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }

        //}
    }
}
