using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetRetirement;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetRetirement;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using System.Data;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FixedAssetManagement.AssetRetirement
{
    public class AssetRetirement_SERVICES : AssetRetirement_ISERVICES
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
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$asset$grp_GetAllAssetGroup_LastLevel_ART", prmContentGetDetails);
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
        public Dictionary<string, string> GetAssetItem(string CompID, string BrId, string ShowFor)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId", DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@ShowFor", DbType.String, ShowFor),};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assettrans_assetitemddl", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        dtList.Add(PARQusData.Tables[0].Rows[i]["item_id"].ToString(), PARQusData.Tables[0].Rows[i]["item_name"].ToString());
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
        public DataSet GetSerialNoJs(string CompId, string BrId, string ItemId, string ShowFor,string grp_id)
        {
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompId),
                objProvider.CreateInitializedParameter("@BrId", DbType.String, BrId),
                objProvider.CreateInitializedParameter("@item_id", DbType.String, ItemId),
                objProvider.CreateInitializedParameter("@ShowFor", DbType.String, ShowFor),
                objProvider.CreateInitializedParameter("@grp_id", DbType.String, grp_id)};
            DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetret_assetserialddl", prmContentGetDetails);
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
        public DataSet GetRetirmentData(string CompId, string BrId, string ItemId, string SerialNo)
        {
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                objProvider.CreateInitializedParameter("@CompID", DbType.String, CompId),
                objProvider.CreateInitializedParameter("@BrchID", DbType.String, BrId),
                objProvider.CreateInitializedParameter("@AssetID", DbType.String, ItemId),
                objProvider.CreateInitializedParameter("@SerialNo", DbType.String, SerialNo),};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetretdetails", prmContentGetDetails);
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
        public DataSet GetRegistrationHistory(string CompID, string BrID, string RegId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@ass_reg_id",DbType.Int32,RegId),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_mis_assetreg_his", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GetSerialNo(string CompID, string BrId, string ItemId, string ShowFor, string grp_id)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId", DbType.String, BrId),
                objProvider.CreateInitializedParameter("@item_id", DbType.String, ItemId),
                objProvider.CreateInitializedParameter("@ShowFor", DbType.String, ShowFor),
                objProvider.CreateInitializedParameter("@grp_id", DbType.String, grp_id),};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetret_assetserialddl", prmContentGetDetails);
                //DataRow dr;
                //dr = PARQusData.Tables[0].NewRow();
                //dr[0] = "0";
                //dr[1] = "---Select---";
                //PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        dtList.Add(PARQusData.Tables[0].Rows[i]["serial_noid"].ToString(), PARQusData.Tables[0].Rows[i]["serial_no"].ToString());
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
        public DataSet GetAssetDescDetails(string CompId, string BrId, string AssetGroupId)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@CompID", DbType.String, CompId),
                     objProvider.CreateInitializedParameter("@BrchID", DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@GroupId", DbType.String, AssetGroupId), };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetret_assetitemddl", prmContentGetDetails);
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
        public string InsertARDetail(AssetRetirement_Model ObjAddItemGroupSetupBOL, DataTable HeaderDetail, DataTable DtblVouGLDetail, DataTable CRCostCenterDetails)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentAddUpdate = {
                objProvider.CreateInitializedParameterTableType("@Result",SqlDbType.NVarChar,""),
                objProvider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,HeaderDetail),
                objProvider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouGLDetail),
                objProvider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CRCostCenterDetails),
                };
                prmContentAddUpdate[0].Size = 100;
                prmContentAddUpdate[0].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "fa_sp_Ins_AssRet", prmContentAddUpdate).ToString();
                string DocNo = string.Empty;
                if (prmContentAddUpdate[0].Value != DBNull.Value)
                {
                    DocNo = prmContentAddUpdate[0].Value.ToString();
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assret", prmContentGetDetails);
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
        public DataSet GetAllData(string CompID, string BranchID, string GroupId, string Status, string UserID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@GroupId",DbType.String, GroupId),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_get_assretlist", prmContentGetDetails);
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
        public string DeleteAssRetDetail(string CompID, string BrchID, string DocNo, string DocDate)
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
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_DelAssRet", prmcontentaddupdate).ToString();
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
           , string UserID, string mac_id, string VoucherNarr)
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
                                                         objProvider.CreateInitializedParameter("@VoucherNarr",DbType.String, VoucherNarr),


                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_Approve_AssRet", prmContentInsert);

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
        public DataSet GetAssetProcurmentDetail(string CompID, string BrID, string RegId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@ass_reg_id",DbType.Int32,RegId),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_mis_assetproc_detail", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
