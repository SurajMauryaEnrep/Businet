using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.DocApproval;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.DocApproval
{
    public  class DocApproval_SERVICES: DocApproval_ISERVICES
    {
       public DataSet getAppDocDetails(string flag, string CompID, string create_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@flag",DbType.String,flag),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@groupname",DbType.String,""),
                      objProvider.CreateInitializedParameter("@create_id",DbType.String,create_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$app$doc$getDetails", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getDocAppList(string CompID, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/
                    
                    objProvider.CreateInitializedParameter("@flag",DbType.String,"GetDocAppList"),               
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                      objProvider.CreateInitializedParameter("@branch_id",DbType.String,br_id),

                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$app$doc$getDetails", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getDocAppListFilter(string CompID, string br_id,string doc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/
                    
                    objProvider.CreateInitializedParameter("@flag",DbType.String,"GetDocAppListFilter"),
                    objProvider.CreateInitializedParameter("@branch_id",DbType.String,br_id),
                    objProvider.CreateInitializedParameter("@doc_id",DbType.String,doc_id),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),

                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$app$doc$getDetails", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getDocAppEditDetails(string Br_ID, string Doc_ID,string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/
                    
                    objProvider.CreateInitializedParameter("@flag",DbType.String,"GetDocAppEditDetails"),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@branch_id",DbType.String,Br_ID),
                     objProvider.CreateInitializedParameter("@doc_id",DbType.String,Doc_ID),

                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$app$doc$getDetails", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> doc_list(string GroupName,string CompID,string branch_id)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentdetails =
                {
                    objProvider.CreateInitializedParameter("@flag",DbType.String,"ddlbranch"),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objProvider.CreateInitializedParameter("@groupname",DbType.String,GroupName),
                    objProvider.CreateInitializedParameter("@branch_id",DbType.String,branch_id),
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$app$doc$getDetails", prmcontentdetails);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].NewRow();
                    dr["doc_id"] = "0";
                    dr["doc_name_eng"] = "---select---";
                    ds.Tables[1].Rows.InsertAt(dr, 0);

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(ds.Tables[1].Rows[i]["doc_id"].ToString(), ds.Tables[1].Rows[i]["doc_name_eng"].ToString());
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return ddlItemNameDictionary;
        }
        public Dictionary<string, string> user_list(string GroupName, string CompID, string branch_id,string DocName)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentdetails =
                {
                    objProvider.CreateInitializedParameter("@flag",DbType.String,"ddlbranch"),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objProvider.CreateInitializedParameter("@groupname",DbType.String,GroupName),
                    objProvider.CreateInitializedParameter("@branch_id",DbType.String,branch_id),
                    objProvider.CreateInitializedParameter("@DocName",DbType.String,DocName),
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$app$doc$getDetails", prmcontentdetails);
                if (ds.Tables[2].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[2].NewRow();
                    dr["user_id"] = "0";
                    dr["user_nm"] = "---select---";
                    ds.Tables[2].Rows.InsertAt(dr, 0);

                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(ds.Tables[2].Rows[i]["user_id"].ToString(), ds.Tables[2].Rows[i]["user_nm"].ToString());
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return ddlItemNameDictionary;
        }

        public String InsertDocAppDetails(DataTable DocAppHeader, DataTable DocAppUserList)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@DocAppHeader",SqlDbType.Structured, DocAppHeader ),
                 objprovider.CreateInitializedParameterTableType("@DocAppUserList",SqlDbType.Structured, DocAppUserList ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string insp_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Insert_Doc_Approval", prmcontentaddupdate).ToString();

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
        public string DeleteDocAddDetails(string br_id, string doc_id, string CompID)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameter("@br_id",DbType.String, br_id ),
                 objprovider.CreateInitializedParameter("@doc_id",DbType.String, doc_id ),
                 objprovider.CreateInitializedParameter("@comp_id",DbType.String, CompID ),
                 objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string insp_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Delete_Doc_Approval", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public string checkEditClick(string CompID,string br_id, string doc_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameter("@comp_id",DbType.String, CompID ),
                 objprovider.CreateInitializedParameter("@br_id",DbType.String, br_id ),
                 objprovider.CreateInitializedParameter("@doc_id",DbType.String, doc_id ),
                 objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string insp_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "EditBtnClickDocApproval", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[3].Value.ToString();
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
