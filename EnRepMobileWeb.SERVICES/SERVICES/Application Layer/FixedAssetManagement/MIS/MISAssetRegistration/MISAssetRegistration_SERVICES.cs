using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.MIS.MISAssetRegistration;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.MIS.MISAssetRegistration;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using System.Data;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FixedAssetManagement.MIS.MISAssetRegistration
{
    public class MISAssetRegsitration_SERVICES : MISAssetRegistration_ISERVICES
    {
        public Dictionary<string, string> GetAssetGroupListPage(string CompID, string GroupId)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@CompID", DbType.String, CompID),
                objProvider.CreateInitializedParameter("@GroupName", DbType.String, GroupId)};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$asset$grp_GetAllAssetGroup_AllLevel", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
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

        public Dictionary<string, string> GetAssetCategory(string CompID, string GroupId)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@CompID", DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@GroupId", DbType.String, GroupId), };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetcategoryon_GrpId", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        dtList.Add(PARQusData.Tables[0].Rows[i]["acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["acc_name"].ToString());
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
        public Dictionary<string, string> GetAssetCategory(string CompID)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetcategory", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        dtList.Add(PARQusData.Tables[0].Rows[i]["acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["acc_name"].ToString());
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
        public DataSet GetAssignedRequirementArea(string CompId, string BrId, string For)
        {
            // Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@CompID", DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrId", DbType.String, BrId),
                objProvider.CreateInitializedParameter("@For", DbType.String, For),};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_AssignedRequirementArea", prmContentGetDetails);
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
        public DataSet GetAllData(string CompID, string BranchID, string GroupId, string CategoryId, string ReqAreaId, string WorkingStatus, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@GroupId",DbType.String, GroupId),
                    objProvider.CreateInitializedParameter("@CategoryId",DbType.String, CategoryId),
                    objProvider.CreateInitializedParameter("@ReqAreaId",DbType.String, ReqAreaId),
                    objProvider.CreateInitializedParameter("@WorkingStatus",DbType.String, WorkingStatus),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_get_mis_arlist", prmContentGetDetails);
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
        public DataTable GetCurrList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                };
                DataTable GetCurr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$curr_Getcurr", prmContentGetDetails).Tables[0];
                return GetCurr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
