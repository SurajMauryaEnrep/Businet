using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SchemeSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SchemeSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.SchemeSetup
{
    public class SchemeSetup_SERVICES: SchemeSetup_ISERVICES
    {
        public DataSet GetAllPageLoadData(string compID, string brId,string user_id, string scheme_id)
        {
            try
            {
                string PageName = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@user_id",DbType.Int32, user_id),
                    objProvider.CreateInitializedParameter("@scheme_id",DbType.String, scheme_id),
                 };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_getSchemeDetailPageLoadData", prmContentGetDetails);

                return DS;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SchemeDataList(string compID, string brId, string prod_grp, string cust_price_grp, string status, string act_status)
        {
            try
            {
                string PageName = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@prod_grp_id",DbType.Int32, prod_grp),
                    objProvider.CreateInitializedParameter("@cust_prc_grp_id",DbType.Int32, cust_price_grp),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, status),
                    objProvider.CreateInitializedParameter("@act_status",DbType.String, act_status),
                 };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_getSchemeListPageLoad", prmContentGetDetails);

                return DS;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SchemeListWithFilter(string compID, string brId, string prod_grp_id, string cust_prc_grp_id, string status, string act_status)
        {
            try
            {
                string PageName = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@prod_grp_id",DbType.Int32, prod_grp_id),
                    objProvider.CreateInitializedParameter("@cust_prc_grp_id",DbType.Int32, cust_prc_grp_id),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, status),
                    objProvider.CreateInitializedParameter("@act_status",DbType.String, act_status),
                 };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_getSchemeList", prmContentGetDetails);

                return DS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet ProductGrpList(string compID, string brId, string fromDt, string uptoDt, string search)
        {
            try
            {
                string PageName = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@fromDt",DbType.String, fromDt),
                    objProvider.CreateInitializedParameter("@upToDt",DbType.String, uptoDt),
                    objProvider.CreateInitializedParameter("@search",DbType.String, search),
                 };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_getSchemeProductGrpList", prmContentGetDetails);

                return DS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet CustPriceGrpList(string compID, string brId, string fromDt, string uptoDt, string search)
        {
            try
            {
                string PageName = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@fromDt",DbType.String, fromDt),
                    objProvider.CreateInitializedParameter("@upToDt",DbType.String, uptoDt),
                    objProvider.CreateInitializedParameter("@search",DbType.String, search),
                 };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_getSchemeCustPriceGrpList", prmContentGetDetails);

                return DS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet ChkGrpsAlrdyAddedInRange(string compID, string brId, string fromDt, string uptoDt, string scheme_id, string prodGrps, string cstPrcGrps)
        {
            try
            {
                string PageName = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@fromDt",DbType.String, fromDt),
                    objProvider.CreateInitializedParameter("@upToDt",DbType.String, uptoDt),
                    objProvider.CreateInitializedParameter("@schm_id",DbType.String, scheme_id),
                    objProvider.CreateInitializedParameter("@prodGrps",DbType.String, prodGrps),
                    objProvider.CreateInitializedParameter("@cstPrcGrps",DbType.String, cstPrcGrps),
                 };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_chkSchemeGrpsAlrdyAddedInRange", prmContentGetDetails);

                return DS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPctGrpAndCstPrcGrpList(string compID, string brId, string fromDt, string uptoDt, string scheme_id)
        {
            try
            {
                string PageName = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@fromDt",DbType.String, fromDt),
                    objProvider.CreateInitializedParameter("@upToDt",DbType.String, uptoDt),
                    objProvider.CreateInitializedParameter("@schm_id",DbType.String, scheme_id)
                 };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_getSchemePctGrpAndCstPrcGrpList", prmContentGetDetails);

                return DS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SchemeDelete(SchemeSetup_Model schemeSetup_Model, string compID, string brId)
        {
            try
            {
                string PageName = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@scheme_id",DbType.Int32, schemeSetup_Model.scheme_id)
                 };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_SchemeDelete", prmContentGetDetails);

                return DS;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SchemeApprove(SchemeSetup_Model schemeSetup_Model, string compID, string brId, string macId)
        {
            try
            {
                string PageName = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@user_id",DbType.Int32, schemeSetup_Model.UserId),
                    objProvider.CreateInitializedParameter("@scheme_id",DbType.Int32, schemeSetup_Model.scheme_id),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, macId)
                 };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_SchemeApprove", prmContentGetDetails);

                return DS;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public string SaveSchemeData(DataTable schemeSetupHeader, DataTable schemeSetupSlabDetail, DataTable schemeSetupProductGroup, DataTable schemeSetupCustomerPriceGroup)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@schemeHeaderDetail",SqlDbType.Structured, schemeSetupHeader ),
                 objprovider.CreateInitializedParameterTableType("@schemeSetupSlabDetail",SqlDbType.Structured, schemeSetupSlabDetail ),
                 objprovider.CreateInitializedParameterTableType("@schemeSetupProductGroup",SqlDbType.Structured,schemeSetupProductGroup ),
                 objprovider.CreateInitializedParameterTableType("@schemeSetupCustomerPriceGroup",SqlDbType.Structured,schemeSetupCustomerPriceGroup ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,"")
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "usp_SaveSchemeDetail", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[4].Value.ToString();
                }
                return DocNo;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
