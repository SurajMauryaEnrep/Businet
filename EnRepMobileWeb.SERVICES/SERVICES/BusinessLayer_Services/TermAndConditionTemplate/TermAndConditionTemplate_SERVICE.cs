using EnRepMobileWeb.MODELS.BusinessLayer.TermAndConditionTemplate;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TermAndConditionTemplate;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.TermAndConditionTemplate
{
    public class TermAndConditionTemplate_SERVICE: TermAndConditionTemplate_ISERVICE
    {
        public DataTable tmpltList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataTable GetGLList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$GetTermConditionList", prmContentGetDetails).Tables[0];
                return GetGLList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetViewDetails(string CompID, string Templateid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, Templateid),

                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetViewDetailsTermAndCondication", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SaveAndUpdateDetails(DataTable dtHeader, DataTable dtTermsConditions, DataTable TaxBranch)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,dtHeader),
                                                        objprovider.CreateInitializedParameterTableType("@termstemplate",SqlDbType.Structured, dtTermsConditions),
                                                        objprovider.CreateInitializedParameterTableType("@BranchDetail",SqlDbType.Structured,TaxBranch),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                    };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertTermAndConditionTemplate", prmcontentaddupdate).ToString();
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
        public string TamplateDetailDelete(TermAndConditionTemplate_Model termAndConditionTemplate_Model, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, termAndConditionTemplate_Model.TemplateId),
                                                     };
                //string SuppDetails = string.Empty;
                string TampDetails = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "DeleteTermAndConditionTemplate", prmContentGetDetails));
                return TampDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
