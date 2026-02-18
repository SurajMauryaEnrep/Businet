using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetRegistration;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetRegistration;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using System.Data;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FixedAssetManagement.AssetRegistration
{
    public class AssetRegsitration_SERVICES : AssetRegistration_ISERVICES
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
                // DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetgrpddl", prmContentGetDetails);
                //DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_parentgrp", prmContentGetDetails);
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
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetitemddl", prmContentGetDetails);
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
        public Dictionary<string, string> GetSerialNo(string CompID, string BrId, string ItemId, string ShowFor, int RegId)
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
                objProvider.CreateInitializedParameter("@ass_reg_id", DbType.Int64, RegId),};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetserialddl", prmContentGetDetails);
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
        public DataSet GetSerialNoJs(string CompId, string BrId, string ItemId, string ShowFor)
        {
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompId),
                objProvider.CreateInitializedParameter("@BrId", DbType.String, BrId),
                objProvider.CreateInitializedParameter("@item_id", DbType.String, ItemId),
                objProvider.CreateInitializedParameter("@ShowFor", DbType.String, ShowFor),};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetserialddl", prmContentGetDetails);
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
                dr[1] = "All";
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
                dr[1] = "---Select---";
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
        public DataSet GetAssignedRequirementArea(string CompId, string BrId)
        {
            // Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@CompID", DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrId", DbType.String, BrId), };
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
        public Dictionary<string, string> GetRequirmentArea(string CompId, string BrId)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@CompID", DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrId", DbType.String, BrId), };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_AssignedRequirementArea", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
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
        public string InsertAssetRegDetail(AssetRegistration_Model ObjAddItemGroupSetupBOL, string mac_id, DataTable Attachments, DataTable ProcurmentDetail)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentAddUpdate = {
                objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, ObjAddItemGroupSetupBOL.CompId),
                objProvider.CreateInitializedParameter("@br_id", DbType.Int32, ObjAddItemGroupSetupBOL.BrdId),
                objProvider.CreateInitializedParameter("@ass_reg_id", DbType.String, ObjAddItemGroupSetupBOL.AssetRegId),
                objProvider.CreateInitializedParameter("@asset_id", DbType.String, ObjAddItemGroupSetupBOL.AssetItemsId),
                objProvider.CreateInitializedParameter("@asset_label", DbType.String, ObjAddItemGroupSetupBOL.AssetLabel),
                objProvider.CreateInitializedParameter("@serial_no", DbType.String, ObjAddItemGroupSetupBOL.SerialNumber1),
                objProvider.CreateInitializedParameter("@asset_grp_id", DbType.Int32, ObjAddItemGroupSetupBOL.AssetsGroupId),
                objProvider.CreateInitializedParameter("@asset_cat_id", DbType.Int32, ObjAddItemGroupSetupBOL.AssetCategoryId),
                //objProvider.CreateInitializedParameter("@procur_dt", DbType.DateTime, ObjAddItemGroupSetupBOL.ProcurementDate),
                //objProvider.CreateInitializedParameter("@supp_name", DbType.String, ObjAddItemGroupSetupBOL.SupplierName),
                //objProvider.CreateInitializedParameter("@bill_no", DbType.String, ObjAddItemGroupSetupBOL.BillNumber),
                //objProvider.CreateInitializedParameter("@bill_dt", DbType.DateTime, ObjAddItemGroupSetupBOL.BillDate),
                objProvider.CreateInitializedParameter("@proc_val", DbType.Double, ObjAddItemGroupSetupBOL.ProcuredValue),
                objProvider.CreateInitializedParameter("@curr_val", DbType.Double, ObjAddItemGroupSetupBOL.CurrentValue),
                objProvider.CreateInitializedParameter("@as_on_dt", DbType.DateTime, ObjAddItemGroupSetupBOL.AsOn),
                objProvider.CreateInitializedParameter("@asset_life", DbType.Int64, ObjAddItemGroupSetupBOL.AssetLife),
                objProvider.CreateInitializedParameter("@add_dep_freq", DbType.Double, ObjAddItemGroupSetupBOL.AddDepreciationPer),
                objProvider.CreateInitializedParameter("@validupto", DbType.Int32, ObjAddItemGroupSetupBOL.ValidUpto),
                objProvider.CreateInitializedParameter("@asset_working_dt", DbType.DateTime, ObjAddItemGroupSetupBOL.AssetWorkingDate),
                objProvider.CreateInitializedParameter("@dep_start_dt", DbType.DateTime, ObjAddItemGroupSetupBOL.DepreciationStartDate),
                objProvider.CreateInitializedParameter("@assign_req_area", DbType.Int32, ObjAddItemGroupSetupBOL.AssignedRequirementAreaId),
                objProvider.CreateInitializedParameter("@assign_req_area_type", DbType.String, ObjAddItemGroupSetupBOL.AssignedRequirementAreaType),
                objProvider.CreateInitializedParameter("@accumulated_dep", DbType.String, ObjAddItemGroupSetupBOL.AccumulatedDepreciation),
                objProvider.CreateInitializedParameter("@working_status", DbType.String, ObjAddItemGroupSetupBOL.WorkingStatusId),
                objProvider.CreateInitializedParameter("@create_id", DbType.Int32, ObjAddItemGroupSetupBOL.Create_id),
                objProvider.CreateInitializedParameter("@mod_id", DbType.Int32, ObjAddItemGroupSetupBOL.Create_id),
                objProvider.CreateInitializedParameter("@mac_id", DbType.String, mac_id),
                objProvider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,Attachments),
                objProvider.CreateInitializedParameterTableType("@FA_AR_ProcurmentDetails",SqlDbType.Structured, ProcurmentDetail),
                objProvider.CreateInitializedParameter("@ImpType",DbType.String, "D"),
                };
                string Result = string.Empty;
                if (ObjAddItemGroupSetupBOL.FormMode == "1")
                {
                    Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "fa_sp_Upd_AssetRegDetail", prmContentAddUpdate));
                }
                else
                {
                    Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "fa_sp_Ins_AssetRegDetail", prmContentAddUpdate));
                }
                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetAssetRegistrationDetail(string CompId, string BrId, int RegId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id", DbType.String, BrId),
                                                        objProvider.CreateInitializedParameter("@ass_reg_id", DbType.String, RegId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetreg", prmContentGetDetails);
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_get_arlist", prmContentGetDetails);
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
        public string DeleteARetails(string CompID, string BrchID, string AssetRegId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@AssetRegId",DbType.String, AssetRegId),
                                                        objProvider.CreateInitializedParameter("@Result",DbType.String,""),
                                                    };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_DelAssetRegister", prmcontentaddupdate).ToString();
                string Result = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
                {
                    Result = prmcontentaddupdate[3].Value.ToString();
                }

                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string ApproveAssetRegistration(string AssetRegId, string CompID, string BrchID, string doc_no, string UserID, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@ass_reg_id",DbType.String, AssetRegId),
                    objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                    objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                };
                prmContentInsert[3].Size = 100;
                prmContentInsert[3].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ApproveAssetRegister", prmContentInsert);
                string DocNo = string.Empty;
                if (prmContentInsert[3].Value != DBNull.Value)
                {
                    DocNo = prmContentInsert[3].Value.ToString();
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
        public DataSet Get_FYList(string Compid, string Brid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Compid),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String,Brid),
                };
                DataSet Getfy_list = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$fy_GetList", prmContentGetDetails);
                return Getfy_list;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetTransferDetail(string CompID, string BrID, string AssetDescriptionTD, string SerialNumberTD)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@asset_id",DbType.String,AssetDescriptionTD),
                 objProvider.CreateInitializedParameter("@serial_no",DbType.String,SerialNumberTD),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assettrans", prmContentGetDetails);
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
        public DataSet GetMasterDropDownList(string Comp_id, string Br_ID)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Comp_id",DbType.String,Comp_id),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,Br_ID),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Master$Asset$Registration$data", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVerifiedDataOfExcel(string compId, string brId, DataTable CustomerDetail, DataTable CustomerBranch)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@AssetRegData",SqlDbType.Structured, CustomerDetail ),
                    objProvider.CreateInitializedParameterTableType("@ProcurmentDetail",SqlDbType.Structured, CustomerBranch ),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                    objProvider.CreateInitializedParameter("@brId", DbType.String,brId),

                };
                DataSet GetCustomerList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateAssetRegistrationExceFile", prmContentGetDetails);
                return GetCustomerList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable ShowExcelErrorDetail(string compId, string brId, DataTable CustomerDetail, DataTable CustomerBranch)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameterTableType("@AssetRegData",SqlDbType.Structured, CustomerDetail),
                    objProvider.CreateInitializedParameterTableType("@ProcurmentDetail",SqlDbType.Structured,CustomerBranch),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                    objProvider.CreateInitializedParameter("@brId", DbType.String,brId),
                  };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ShowExcelAssRegErrorDetail", prmContentGetDetails);
                return GetItemList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string BulkImportAssetRegistrationDetail(string compId, string UserID, string BranchName, DataTable CustomerDetail, DataTable CustomerBranch)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                     objProvider.CreateInitializedParameterTableType("@AssetRegistrationData",SqlDbType.Structured, CustomerDetail ),
                    objProvider.CreateInitializedParameterTableType("@ProcDetail",SqlDbType.Structured, CustomerBranch ),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                    objProvider.CreateInitializedParameter("@BrchId", DbType.String,BranchName),
                    objProvider.CreateInitializedParameter("@userId",DbType.String,UserID),
                 objProvider.CreateInitializedParameterTableType("@OutPut",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BulkImportAssetRegister", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[5].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[5].Value.ToString();
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
