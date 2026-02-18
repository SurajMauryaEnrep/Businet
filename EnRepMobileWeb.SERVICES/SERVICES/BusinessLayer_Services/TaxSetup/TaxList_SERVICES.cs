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
    public class TaxList_SERVICES :TaxList_ISERVICES
    {
        public DataTable GetTaxNameList(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@TaxName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$TaxName", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllData(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@TaxName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$TaxSetup", prmContentGetDetails);
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
        public DataSet GetTaxListFilterDAL(string CompID, string TaxID, string ActStatus, string Taxtype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@TaxID",DbType.String,TaxID),
                     objProvider.CreateInitializedParameter("@ActStatus",DbType.String,ActStatus),
                      objProvider.CreateInitializedParameter("@Taxtype",DbType.String,Taxtype),
              };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetTaxFilterList", prmContentGetDetails);
                return GetTaxList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> TaxSetupGroupDAL(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlTaxNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@TaxName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet taxname = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$TaxName", prmContentGetDetails);
                DataRow dr;
                dr = taxname.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                taxname.Tables[0].Rows.InsertAt(dr, 0);

                if (taxname.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < taxname.Tables[0].Rows.Count; i++)
                    {
                        ddlTaxNameDictionary.Add(taxname.Tables[0].Rows[i]["tax_id"].ToString(), taxname.Tables[0].Rows[i]["tax_name"].ToString());
                    }
                }
                return ddlTaxNameDictionary;

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
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataTable GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$TaxList$detail", prmContentGetDetails).Tables[0];
                return GetTaxList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
