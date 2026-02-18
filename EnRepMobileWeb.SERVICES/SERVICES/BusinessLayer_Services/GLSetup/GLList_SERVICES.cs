using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
   public class GLList_SERVICES :GLList_ISERVICES
    {
        public DataTable BindGetGLNameList(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@AccName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$GLName", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllDropDownGL(string GroupName, string CompID,string Br_ID, string ddlGLGroup, string GLID, string GRPID, string GLAct, string GLAcctype,
           string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@AccName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                    objProvider.CreateInitializedParameter("@ddlGLGroup",DbType.String, ddlGLGroup),
                    objProvider.CreateInitializedParameter("@GLID",DbType.String,GLID),
                    objProvider.CreateInitializedParameter("@GRPID",DbType.String,GRPID),
                    objProvider.CreateInitializedParameter("@GLAct",DbType.String,GLAct),
                    objProvider.CreateInitializedParameter("@GLAcctype",DbType.String,GLAcctype),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String,Flag),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$All$DropDown$list$GL", prmContentGetDetails);
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
        public DataTable BindGetGLGroupList(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Acc$grp_GetAllAccGroup", prmContentGetDetails).Tables[0];
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
        public DataTable GetGLListDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataTable GetGLList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$GLList$detail", prmContentGetDetails).Tables[0];
                return GetGLList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GLSetupGroupDAL(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@AccName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$GLName", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["acc_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> AccGrpListGroupDAL(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Acc$grp_GetAllAccGroup", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["acc_grp_id"].ToString(), PARQusData.Tables[0].Rows[i]["AccGroupChildNood"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetGLListFilterDAL(string CompID, string GLID, string GRPID, string GLAct, string GLAcctype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@GLID",DbType.String,GLID),
                     objProvider.CreateInitializedParameter("@GRPID",DbType.String,GRPID),
                      objProvider.CreateInitializedParameter("@GLAct",DbType.String,GLAct),
                      objProvider.CreateInitializedParameter("@GLAcctype",DbType.String,GLAcctype),
              };
                DataSet GetGLList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetGLFilterList", prmContentGetDetails);
                return GetGLList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //added By Nitesh (GetGLSetup_data) 03-11-2023 for get Opning and closing Balence and Currency
        public DataSet GetGLSetup_data(string CompID, string BranchID, string acc_id, string acc_grpid, string acc_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@acc_id",DbType.String,acc_id),
                     objProvider.CreateInitializedParameter("@acc_grpid",DbType.String,acc_grpid),
                      objProvider.CreateInitializedParameter("@acc_type",DbType.String,acc_type),
                      objProvider.CreateInitializedParameter("@BranchID",DbType.String,BranchID),
              };
                DataSet GetGLList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetGLFilterLists", prmContentGetDetails);
                return GetGLList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetMasterDataForExcelFormat(string compId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId)
        };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetMasterDataforGLImportFormat", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVerifiedDataOfExcel(string CompID, DataTable GLDetail, DataTable GLBranch)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured, GLDetail ),
                    objProvider.CreateInitializedParameterTableType("@GLBranch",SqlDbType.Structured,GLBranch),
                    objProvider.CreateInitializedParameter("@CompID", DbType.String,CompID),

                };
                DataSet GetGLList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateGLExceFile", prmContentGetDetails);
                return GetGLList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable ShowExcelErrorDetail(string CompID, DataTable GLDetail, DataTable GLBranch)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured, GLDetail ),
                    objProvider.CreateInitializedParameterTableType("@GLBranch",SqlDbType.Structured,GLBranch),
                    objProvider.CreateInitializedParameter("@CompID", DbType.String,CompID),
                };
                DataSet GetGLList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ShowExcelGLErrorDetail", prmContentGetDetails);
                return GetGLList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string BulkImportGLDetail(string CompID, string Userid, string BranchName, DataTable GLDetail, DataTable GLBranch)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                     objProvider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured, GLDetail ),
                    objProvider.CreateInitializedParameterTableType("@GLBranch",SqlDbType.Structured,GLBranch),
                    objProvider.CreateInitializedParameter("@CompID", DbType.String,CompID),
                    objProvider.CreateInitializedParameter("@userId",DbType.String,Userid),
                    objProvider.CreateInitializedParameter("@BranchName",DbType.String,BranchName),
                 objProvider.CreateInitializedParameterTableType("@OutPut",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BulkImportGL", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[5].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
    }
}
