using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.CurrencySetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.CurrencySetup
{
    public class CurrencySetup_SERVICE: CurrencySetup_ISERVICE
    {
        public DataSet GetCurrancyTable(string comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_CurrancyTable", prmContentGetDetails);
                return GetBrList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCurrencyList(string CompId, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[CostCurrencyList]", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllData(string CompId, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$All$Data$Currency]", prmContentGetDetails);
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
        public string InsertCurr(string CompID, int currencyid, string Price, string Date, string transtype)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@comp_id",DbType.Int64,CompID),
                    objprovider.CreateInitializedParameter("@curr_id",DbType.Int64, currencyid),
                    objprovider.CreateInitializedParameter("@conv_date",DbType.Date,Date),
                    objprovider.CreateInitializedParameter("@conv_rate",DbType.String,Price),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,transtype),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")
                };
                prmContent[5].Size = 100;
                prmContent[5].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[insertCurrSetupDetail]", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[5].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeleteCurrDetail(string CompID, int currencyid,string Date, string transtype)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@comp_id",DbType.Int64,CompID),
                    objprovider.CreateInitializedParameter("@curr_id",DbType.Int64,currencyid),
                    objprovider.CreateInitializedParameter("@conv_date",DbType.Date,Date),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,transtype),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[DeletecurrData]", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[4].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[4].Value.ToString();
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
