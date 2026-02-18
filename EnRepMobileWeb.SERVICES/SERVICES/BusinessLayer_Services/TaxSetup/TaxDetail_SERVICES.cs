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
using EnRepMobileWeb.MODELS.BusinessLayer.TaxDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
   public class TaxDetail_SERVICES : TaxDetail_ISERVICES
    {
        public DataTable GetAccountGroupDAL(string CompID, string type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@type",DbType.String, type),
                                                    };
                DataTable GetAccountGroup = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetHierAccGroupList", prmContentGetDetails).Tables[0];
                return GetAccountGroup;
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
                DataTable GetAccountGroup = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetAllAccGroups", prmContentGetDetails).Tables[0];
                return GetAccountGroup;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetviewTaxdetailDAL(string Taxcode, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@tax_id",DbType.String, Taxcode),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                     };
                DataSet GetviewTaxdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Tax$ViewDetail", prmContentGetDetails);
                return GetviewTaxdetail;

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
        public DataTable GetTaxcoaDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),                     
                                                    };
                DataTable GetTaxcoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$tax$Coa", prmContentGetDetails).Tables[0];
                return GetTaxcoa;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable  deletetaxdata(string CompID, string BrchID, int tax_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameter("@comid",DbType.Int32,CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int32,BrchID),
                    objProvider.CreateInitializedParameter("@tax_id",DbType.Int32,tax_id),

                };
                DataTable deletetax = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$delete$tax$setup", prmContentGetDetails).Tables[0];
                return deletetax;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetTaxAuthrityCoaDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),                 
                                                    };
                DataTable GetTaxAuthrityCoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$TaxAuthority$Coa", prmContentGetDetails).Tables[0];
                return GetTaxAuthrityCoa;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

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
        public DataSet GetAllData(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$All$Data$DropDown$Tax$Setup$Detail", prmContentGetDetails);
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
        public string insertTaxDetail(DataTable TaxDetail, DataTable TaxBranch)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, TaxDetail ),
                 objprovider.CreateInitializedParameterTableType("@BranchDetail",SqlDbType.Structured, TaxBranch ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertTax_Details", prmcontentaddupdate).ToString();

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
        //public string InsertTaxDetailDAL(TaxDetailModel _TaxDetailModel, string CompID, string UserID)
        //{
        //    string ActiveFlag, RecovFlag, ManualCalcFlag;

        //    if (_TaxDetailModel.act_status)
        //        ActiveFlag = "Y";
        //    else
        //        ActiveFlag = "N";
        //    if (_TaxDetailModel.recov)
        //        RecovFlag = "Y";
        //    else
        //        RecovFlag = "N";
        //    if (_TaxDetailModel.manual_calc)
        //        ManualCalcFlag = "Y";
        //    else
        //        ManualCalcFlag = "N";
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentAddUpdate = {

        //                                                //stp$item$detail
        //                                                objProvider.CreateInitializedParameter("@TransType",DbType.String, _TaxDetailModel.TransType ),
        //                                                objProvider.CreateInitializedParameter("@tax_id",DbType.Int32, _TaxDetailModel.tax_id ),
        //                                                objProvider.CreateInitializedParameter("@tax_name",DbType.String, _TaxDetailModel.tax_name ),
        //                                                objProvider.CreateInitializedParameter("@acc_id",DbType.Int32, _TaxDetailModel.acc_id ),
        //                                                objProvider.CreateInitializedParameter("@tax_auth_id",DbType.Int32, _TaxDetailModel.tax_auth_id ),
        //                                                objProvider.CreateInitializedParameter("@tax_perc",DbType.Double, _TaxDetailModel.tax_perc ),
        //                                                objProvider.CreateInitializedParameter("@recov",DbType.String, RecovFlag ),
        //                                                objProvider.CreateInitializedParameter("@app_date",DbType.String, _TaxDetailModel.app_date ),
        //                                                objProvider.CreateInitializedParameter("@act_status",DbType.String, ActiveFlag ),
        //                                                objProvider.CreateInitializedParameter("@man_calc",DbType.String, ManualCalcFlag ),
        //                                                objProvider.CreateInitializedParameter("@tax_type",DbType.String, _TaxDetailModel.tax_type ),
        //                                                objProvider.CreateInitializedParameter("@create_id",DbType.Int32, Convert.ToInt32(UserID)),
        //                                                objProvider.CreateInitializedParameter("@mod_id",DbType.Int32, Convert.ToInt32(UserID)),
        //                                                objProvider.CreateInitializedParameter("@mac_id",DbType.String, _TaxDetailModel.mac_id ),
        //                                                objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),

        //                                            };
        //        string CompanyId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_InsertTaxDetails", prmContentAddUpdate).ToString();
        //        return CompanyId;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //    }
        //}
        //public DataSet InsertBrDetailDAL(string Comp_ID, string Taxcode, string BrID, string Flag, string TransType)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //             objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
        //             objProvider.CreateInitializedParameter("@taxID",DbType.String,Taxcode),
        //             objProvider.CreateInitializedParameter("@BrID",DbType.String,BrID),
        //              objProvider.CreateInitializedParameter("@ActStat",DbType.String,Flag),
        //              objProvider.CreateInitializedParameter("@TransType",DbType.String,TransType),

        //             };
        //        DataSet InsertBrDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_InsertTaxBranchDetails", prmContentGetDetails);
        //        return InsertBrDetail;

        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
