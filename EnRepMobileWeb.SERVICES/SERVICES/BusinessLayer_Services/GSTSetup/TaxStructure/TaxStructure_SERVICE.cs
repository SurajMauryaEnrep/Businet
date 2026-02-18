using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.GSTSetup.TaxStructure;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.GSTSetup.TaxStructure
{
    public class TaxStructure_SERVICE : TaxStructure_ISERVICE
    {
        public DataTable GetTaxStructureDetail(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/  
                   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                     };
                DataTable GetviewGLdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$TaxStructure$ListDetail", prmContentGetDetails).Tables[0];
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
        public DataSet GetTaxStructureViewDetail(string TaxCode,string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/  
                   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@TaxCode",DbType.String, TaxCode),
                                                     };
                DataSet GetviewGLdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$TaxStructure$ViewDetail", prmContentGetDetails);
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
        public DataTable GetTaxPercDetail(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetTaxcoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$taxperc", prmContentGetDetails).Tables[0];
                return GetTaxcoa;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllData(string CompID,string Flag,string TaxCode)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                     objProvider.CreateInitializedParameter("@TaxCode",DbType.String, TaxCode),
                                                    };
                DataSet GetTaxcoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$Tax$Structure", prmContentGetDetails);
                return GetTaxcoa;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetTaxListDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetTaxcoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$tax$detail", prmContentGetDetails).Tables[0];
                return GetTaxcoa;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public String InsertGstTaxStructureDetail(DataTable TaxStructureDt)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@TaxStructureDt",SqlDbType.Structured, TaxStructureDt ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[1].Size = 100;
                prmcontentaddupdate[1].Direction = ParameterDirection.Output;

                string OpBalDt = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertGstStructureDetail", prmcontentaddupdate).ToString();

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
        public string DeleteTaxDetail(string tax_perc, string CompID)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {

                    objprovider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@tax_perc",DbType.String,tax_perc),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")

                };
                prmContent[2].Size = 100;
                prmContent[2].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteGstTaxDetail", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[2].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[2].Value.ToString();
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
