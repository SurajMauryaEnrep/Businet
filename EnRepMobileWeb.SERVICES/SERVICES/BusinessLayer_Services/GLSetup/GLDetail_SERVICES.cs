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
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
    public class GLDetail_SERVICES: GLDetail_ISERVICES
    {
        public DataSet GetBrList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$BrList$detail", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetBrListDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataTable GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$BrList$detail", prmContentGetDetails).Tables[0];
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetAccountGroupList(string CompID, string acc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@acc_id",DbType.String, acc_id),
                                                    };
                DataTable GetAccountGroup = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetAllAccGroups]", prmContentGetDetails).Tables[0];
                return GetAccountGroup;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetAccountGroupDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),                 
                                                    };
                DataTable GetAccountGroup = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetAllAccGroup", prmContentGetDetails).Tables[0];
                return GetAccountGroup;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string insertGLDetail(DataTable GLDetail, DataTable GLBranch)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured, GLDetail ),
                 objprovider.CreateInitializedParameterTableType("@BranchDetail",SqlDbType.Structured, GLBranch ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertGL_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[2].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[2].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string GLDetailDelete(string GLCode, string CompID)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 //objprovider.CreateInitializedParameterTableType("@acc_id",SqlDbType.Structured, GLCode ),
                 //objprovider.CreateInitializedParameterTableType("@comp_id",SqlDbType.Structured, CompID ),
                 objprovider.CreateInitializedParameter("@acc_id",DbType.String, GLCode),
                     objprovider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteGL_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[2].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[2].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }


        }
        public DataSet GetviewGLdetailDAL(string GLCode, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@acc_id",DbType.String, GLCode),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                     };
                DataSet GetviewGLdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$GL$ViewDetail]", prmContentGetDetails);
                return GetviewGLdetail;

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
        public DataTable GetCurr(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),             
                };
                DataTable GetCurr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetCurronGL", prmContentGetDetails).Tables[0];
                return GetCurr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
