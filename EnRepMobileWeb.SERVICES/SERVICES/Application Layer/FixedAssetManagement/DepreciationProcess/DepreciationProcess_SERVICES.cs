using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.DepreciationProcess;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.DepreciationProcess;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using System.Data;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FixedAssetManagement.DepreciationProcess
{
    public class DepreciationProcess_SERVICES : DepreciationProcess_ISERVICES
    {
        public Dictionary<string, string> GetAssetGroup(string CompID, string GroupId)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@CompID", DbType.String, CompID),
                objProvider.CreateInitializedParameter("@GroupName", DbType.String, GroupId)};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$asset$grp_GetAllAssetGroup_LastLevel", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        dtList.Add(PARQusData.Tables[0].Rows[i]["item_grp_id"].ToString(), PARQusData.Tables[0].Rows[i]["ItemGroupChildNood"].ToString());
                    }
                }
                return dtList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetAssetCategoryDetails(string CompId, string BrId, string AssetGroupId)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@CompID", DbType.String, CompId),
                     objProvider.CreateInitializedParameter("@BrchID", DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@GroupId", DbType.String, AssetGroupId), };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetgroup_details", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet BindFinancialYear(int CompID, int BrID, string f_freq, string StartDate, string Period, string AssetGroupId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@f_freq",DbType.String, f_freq),
                    objProvider.CreateInitializedParameter("@StartDate",DbType.String, StartDate),
                     objProvider.CreateInitializedParameter("@trtype",DbType.String, Period),
                     objProvider.CreateInitializedParameter("@AssetGroupId",DbType.String, AssetGroupId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_deppro_get_fy", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAssetRegGroupDetail(string CompId, string BrID, string AssetGroup, string fin_yr, string Period)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@asset_grp_id",DbType.String, AssetGroup),
                    objProvider.CreateInitializedParameter("@fin_yr",DbType.String, fin_yr),
                    objProvider.CreateInitializedParameter("@Period",DbType.String, Period),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetregon_asset_grp", prmContentGetDetails);
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
        public string InsertDepreciationProcessDetail(DepreciationProcess_Model ObjAddItemGroupSetupBOL, DataTable HeaderDetail, DataTable AssetDetails, DataTable Attachments, DataTable DtblVouGLDetail, DataTable CRCostCenterDetails)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentAddUpdate = {
                objProvider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,HeaderDetail),
                objProvider.CreateInitializedParameterTableType("@AssetDetail",SqlDbType.Structured,AssetDetails),
                objProvider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,Attachments),
                objProvider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouGLDetail),
                objProvider.CreateInitializedParameterTableType("@Result",SqlDbType.NVarChar,""),
                objProvider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CRCostCenterDetails),
                };
                prmContentAddUpdate[4].Size = 100;
                prmContentAddUpdate[4].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "fa_sp_Ins_DepProcess", prmContentAddUpdate).ToString();
                string DocNo = string.Empty;
                if (prmContentAddUpdate[4].Value != DBNull.Value)
                {
                    DocNo = prmContentAddUpdate[4].Value.ToString();
                }
                return DocNo;
                //return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetDepreciationProcessDetail(string CompId, string BrId, string DocNo, string DocDate, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id", DbType.String, BrId),
                                                        objProvider.CreateInitializedParameter("@DP_DocNo", DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@DP_DocDate", DbType.String, DocDate),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_depprocess", prmContentGetDetails);
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
        public DataSet GetAllData(string CompID, string BranchID, string GroupId, string Status, string wfstatus, string UserID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@GroupId",DbType.String, GroupId),
                   // objProvider.CreateInitializedParameter("@PeriodId",DbType.String, PeriodId),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_get_dplist", prmContentGetDetails);
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
        public string DeleteDPetails(string CompID, string BrchID, string DocNo, string DocDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, DocDate),
                                                        objProvider.CreateInitializedParameter("@Result",DbType.String,""),
                                                    };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_DelDepProcess", prmcontentaddupdate).ToString();
                string Result = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value) // status
                {
                    Result = prmcontentaddupdate[4].Value.ToString();
                }

                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string ApproveDPDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID
           , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@invno",DbType.String, Inv_No),
                                                        objProvider.CreateInitializedParameter("@invdate",DbType.String, Inv_Date),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String, MenuDocId),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String,Branch),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                         objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                                                         objProvider.CreateInitializedParameter("@VoucherNarr",DbType.String, VoucherNarr),
                                                         

                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_Approve_DepProcess", prmContentInsert);

                string DocNo = string.Empty;
                if (prmContentInsert[6].Value != DBNull.Value)
                {
                    DocNo = prmContentInsert[6].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
       
        public DataSet BindPeriod(int CompID, int BrID, string f_freq, string StartDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@f_freq",DbType.String, f_freq),
                    objProvider.CreateInitializedParameter("@StartDate",DbType.String, StartDate),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$forecast$get$fy", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindDateRangeCal(int CompID, int BrID, string f_frequency, string start_year, string end_year, Int32 months)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@f_freq",DbType.String, f_frequency),
                    objProvider.CreateInitializedParameter("@start_year",DbType.String, start_year),
                    objProvider.CreateInitializedParameter("@end_year",DbType.String, end_year),
                    objProvider.CreateInitializedParameter("@months",DbType.Int32, months),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fa$get$periodrange", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllGLDetails(DataTable GLDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured,GLDetail),
                                                    };

                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetGLDetail", prmcontentaddupdate);
                return GetGlDt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet GetTaxRecivableAcc(string comp_id, string assetId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@assetId",DbType.String, assetId),
                     };
                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_getassetacc", prmContentGetDetails);
                return GetGlDt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
    }
}
