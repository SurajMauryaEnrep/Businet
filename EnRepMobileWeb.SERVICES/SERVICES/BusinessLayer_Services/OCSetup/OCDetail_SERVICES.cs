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
using EnRepMobileWeb.MODELS.BusinessLayer.OCDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
   public class OCDetail_SERVICES :OCDetail_ISERVICES
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
        public DataSet GetallData(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$OtherDeatil", prmContentGetDetails);
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

        public DataTable GetThirdPartyDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetThirdParty = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Get$ThirdParty", prmContentGetDetails).Tables[0];
                return GetThirdParty;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetOCcoaDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetOCcoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$oc$Coa", prmContentGetDetails).Tables[0];
                return GetOCcoa;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetviewOCdetailDAL(string OCcode, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@OC_id",DbType.String, OCcode),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                     };
                DataSet GetviewOCdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$OC$ViewDetail]", prmContentGetDetails);
                return GetviewOCdetail;

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
        public string insertOCDetail(DataTable OCDetail, DataTable OCBranch)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured, OCDetail ),
                 objprovider.CreateInitializedParameterTableType("@BranchDetail",SqlDbType.Structured, OCBranch ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertOC_Details", prmcontentaddupdate).ToString();

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
        //public string InsertOCDetailDAL(OCDetailModel _OCDetailModel, string CompID, string UserID)
        //{
        //    string ActiveFlag;

        //    if (_OCDetailModel.act_status)
        //        ActiveFlag = "Y";
        //    else
        //        ActiveFlag = "N";
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentAddUpdate = {

        //                                                //stp$item$detail
        //                                                objProvider.CreateInitializedParameter("@TransType",DbType.String, _OCDetailModel.TransType ),
        //                                                objProvider.CreateInitializedParameter("@oc_id",DbType.Int32, _OCDetailModel.oc_id ),
        //                                                objProvider.CreateInitializedParameter("@oc_name",DbType.String, _OCDetailModel.oc_name ),
        //                                                objProvider.CreateInitializedParameter("@acc_id",DbType.Int32, _OCDetailModel.acc_id ),
        //                                                objProvider.CreateInitializedParameter("@tp_id",DbType.Int32, _OCDetailModel.tp_id ),
        //                                                objProvider.CreateInitializedParameter("@oc_type",DbType.String, _OCDetailModel.oc_type ),
        //                                                objProvider.CreateInitializedParameter("@app_date",DbType.String, _OCDetailModel.app_date ),
        //                                                objProvider.CreateInitializedParameter("@act_status",DbType.String, ActiveFlag ),
        //                                                objProvider.CreateInitializedParameter("@create_id",DbType.Int32, Convert.ToInt32(UserID)),
        //                                                objProvider.CreateInitializedParameter("@mod_id",DbType.Int32, Convert.ToInt32(UserID)),
        //                                                objProvider.CreateInitializedParameter("@mac_id",DbType.String, _OCDetailModel.mac_id ),
        //                                                objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),

        //                                            };
        //        string CompanyId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_InsertOCDetails", prmContentAddUpdate).ToString();
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
        //public DataSet InsertBrDetailDAL(string Comp_ID, string OCcode, string BrID, string Flag, string TransType)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //             objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
        //             objProvider.CreateInitializedParameter("@OCID",DbType.String,OCcode),
        //             objProvider.CreateInitializedParameter("@BrID",DbType.String,BrID),
        //              objProvider.CreateInitializedParameter("@ActStat",DbType.String,Flag),
        //              objProvider.CreateInitializedParameter("@TransType",DbType.String,TransType),

        //             };
        //        DataSet InsertBrDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_InsertOCBranchDetails", prmContentGetDetails);
        //        return InsertBrDetail;

        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
