using EnRepMobileWeb.MODELS.BusinessLayer.TaxTemplate;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TaxTemplate;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.TaxTemplate
{
    public class TaxTemplate_SERVICES: TaxTemplate_ISERVICES
    {
        public DataSet GetTaxTemplateList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        //objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, Templateid),

                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetTaxTemplateList", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SaveAndUpdateDetails(DataTable dtHeader, DataTable dtTax, DataTable dtBranch, DataTable dtModule, DataTable dtHSNNumber,string Method)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,dtHeader),                                                        
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, dtTax),
                                                        objprovider.CreateInitializedParameterTableType("@BranchDetail",SqlDbType.Structured,dtBranch),                                                                                                           
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@ModuleDetail",SqlDbType.Structured,dtModule),
                                                        objprovider.CreateInitializedParameterTableType("@HSNDetail",SqlDbType.Structured,dtHSNNumber),
                                                        objprovider.CreateInitializedParameter("@Method",DbType.String,Method),
                                                    };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertTaxTemplateTransactionDetails", prmcontentaddupdate).ToString();
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
        public DataSet GetViewDetails(string CompID, string Templateid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, Templateid),
                                              
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetViewDetailsTaxTemplate", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetModulAndHsnDetails(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        //objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, Templateid),

                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetModuleTaxTemplate", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string TamplateDetailDelete(TaxTemplate_Model template_Model, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, template_Model.TemplateId),
                                                     };
                //string SuppDetails = string.Empty;
                string TampDetails = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "stp$tax$tmplt_DeleteTaxTemplate", prmContentGetDetails));
                return TampDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
