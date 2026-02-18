using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.SERVICES.ISERVICES.SecurityLayer.UserSetup;
using EnRepMobileWeb.MODELS.SecurityLayer.UserSetup;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace EnRepMobileWeb.SERVICES.SERVICES.SecurityLayer.UserSetup
{
   public class UserDetail_SERVICES: UserDetail_ISERVICES
    {
        public DataSet BindLAng()
        {
                    
            SqlParameter[] prmContentGetDetails = {          
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$langList]", prmContentGetDetails);       
            return ds;
        }
        public DataSet BindHeadOffice()
        {
            SqlParameter[] prmContentGetDetails = {
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[fct$Comp$detail_GetHoListAll]", prmContentGetDetails);
            return ds;
        } 
        public DataSet BindReportingTo()
        {
            SqlParameter[] prmContentGetDetails = {
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sec$repoting$user]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetRoleName(string HO_ID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
                objProvider.CreateInitializedParameter("@HOName",DbType.String,HO_ID),
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$RoleList]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetBranchName(string HO_ID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
                objProvider.CreateInitializedParameter("@Ho",DbType.String,HO_ID),
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$GetBranch]", prmContentGetDetails);
            return ds;
        }
        public DataSet getUserSetUpDt(string user_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
                objProvider.CreateInitializedParameter("@User_ID",DbType.String,user_id),
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_sec$GetUserSetup$details]", prmContentGetDetails);
            return ds;
        }
        public string InsertUpdateUserSetup(DataTable Userdetail, DataTable UserRoleDetail, DataTable UserBranchAccDetail)
        {
            try
            {
                SqlDataProvider sqlDataProvider = new SqlDataProvider();
                SqlParameter[] sqlParameters =
                {
                sqlDataProvider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,Userdetail),
                sqlDataProvider.CreateInitializedParameterTableType("@RoleDetail",SqlDbType.Structured,UserRoleDetail),
                sqlDataProvider.CreateInitializedParameterTableType("@BranchDetail",SqlDbType.Structured,UserBranchAccDetail),
                sqlDataProvider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
           
            };
                sqlParameters[3].Size = 100;
                sqlParameters[3].Direction = ParameterDirection.Output;
                string Result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sec$UserSetup$InsertUpdate", sqlParameters).ToString();
                string DocNo = string.Empty;
                if (sqlParameters[3].Value != DBNull.Value)
                {
                    DocNo = sqlParameters[3].Value.ToString();
                }
                return DocNo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string DeleteUserSetup(string User_ID)
        {
            try
            {
                SqlDataProvider sqlDataProvider = new SqlDataProvider();
                SqlParameter[] sqlParameters =
                {
                sqlDataProvider.CreateInitializedParameter("@user_id",DbType.String,User_ID),
               //sqlDataProvider.CreateInitializedParameter("@user_id",DbType.String,User_ID),
                sqlDataProvider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),

            };
                sqlParameters[1].Size = 100;
                sqlParameters[1].Direction = ParameterDirection.Output;
                string Result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sec$user$delete", sqlParameters).ToString();
                string DocNo = string.Empty;
                if (sqlParameters[1].Value != DBNull.Value)
                {
                    DocNo = sqlParameters[1].Value.ToString();
                }
                return DocNo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetUserTree(ItemMenuSearchModel ObjItemMenuSearchModel)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, ObjItemMenuSearchModel.Comp_ID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sec$$User$detail$get$tree$data", prmContentGetDetails);
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
