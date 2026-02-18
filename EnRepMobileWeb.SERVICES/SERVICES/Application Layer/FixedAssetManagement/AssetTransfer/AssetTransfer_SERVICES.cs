using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetTransfer;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetTransfer;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using System.Data;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FixedAssetManagement.AssetTransfer
{
    public class AssetTransfer_SERVICES : AssetTransfer_ISERVICES
    {
      
       
        
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
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assettrans_assetserialddl", prmContentGetDetails);
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
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assettrans_assetserialddl", prmContentGetDetails);
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
        public DataSet GetLabelJs(string CompId, string BrId, string ItemId, string SerialNo, string ShowFor)
        {
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompId),
                objProvider.CreateInitializedParameter("@BrId", DbType.String, BrId),
                objProvider.CreateInitializedParameter("@item_id", DbType.String, ItemId),
                objProvider.CreateInitializedParameter("@serialno", DbType.String, SerialNo),};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assettrans_assetserialddl", prmContentGetDetails);
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
        public DataSet GetLabelJs(string CompId, string BrId, string ItemId, string SerialNo)
        {
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = { 
                objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompId),
                objProvider.CreateInitializedParameter("@BrId", DbType.String, BrId),
                objProvider.CreateInitializedParameter("@item_id", DbType.String, ItemId),
                objProvider.CreateInitializedParameter("@serial_no", DbType.String, SerialNo),};
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assettrans_assetlabel", prmContentGetDetails);
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
        public string InsertAssetRegDetail(AssetTransfer_Model ObjAddItemGroupSetupBOL, string mac_id, DataTable Attachments)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentAddUpdate = {
                objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, ObjAddItemGroupSetupBOL.CompId),
                objProvider.CreateInitializedParameter("@br_id", DbType.Int32, ObjAddItemGroupSetupBOL.BrdId),
                objProvider.CreateInitializedParameter("@asset_id", DbType.String, ObjAddItemGroupSetupBOL.AssetItemsId),
                objProvider.CreateInitializedParameter("@serial_no", DbType.String, ObjAddItemGroupSetupBOL.SerialNumber),
                objProvider.CreateInitializedParameter("@transfer_dt", DbType.String, ObjAddItemGroupSetupBOL.TransferDate),
                objProvider.CreateInitializedParameter("@src_assign_req_area", DbType.Int32, ObjAddItemGroupSetupBOL.AssignedRequirementAreaId),
                objProvider.CreateInitializedParameter("@src_assign_req_area_type", DbType.String, ObjAddItemGroupSetupBOL.AssignedRequirementAreaType),
                objProvider.CreateInitializedParameter("@assign_req_area", DbType.Int32, ObjAddItemGroupSetupBOL.DestinationAssignedRequirementAreaId),
                objProvider.CreateInitializedParameter("@assign_req_area_type", DbType.String, ObjAddItemGroupSetupBOL.DestinationAssignedRequirementAreaType),
                objProvider.CreateInitializedParameter("@remarks", DbType.String, ObjAddItemGroupSetupBOL.Remarks),
                objProvider.CreateInitializedParameter("@TransType", DbType.String, ObjAddItemGroupSetupBOL.TransType),
                objProvider.CreateInitializedParameter("@MenuID", DbType.String, ObjAddItemGroupSetupBOL.DocumentMenuId),
                objProvider.CreateInitializedParameter("@create_id", DbType.Int32, ObjAddItemGroupSetupBOL.Create_id),
                objProvider.CreateInitializedParameter("@mod_id", DbType.Int32, ObjAddItemGroupSetupBOL.Create_id),
                objProvider.CreateInitializedParameter("@mac_id", DbType.String, mac_id),
                objProvider.CreateInitializedParameter("@Docdt", DbType.String, ObjAddItemGroupSetupBOL.DocDate),
                objProvider.CreateInitializedParameter("@DocNo", DbType.String, ObjAddItemGroupSetupBOL.DocNo),
                objProvider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,Attachments),
                objProvider.CreateInitializedParameterTableType("@Result",SqlDbType.NVarChar,""),
                };
                prmContentAddUpdate[18].Size = 100;
                prmContentAddUpdate[18].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "fa_sp_Ins_AssetTransDetail", prmContentAddUpdate).ToString();
                string DocNo = string.Empty;
                if (prmContentAddUpdate[18].Value != DBNull.Value)
                {
                    DocNo = prmContentAddUpdate[18].Value.ToString();
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

        public DataSet GetAssetTransferDetail(string CompId, string BrId, string DocNo, string DocDate, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id", DbType.String, BrId),
                                                        objProvider.CreateInitializedParameter("@AT_DocNo", DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@AT_DocDate", DbType.String, DocDate),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assettransfer", prmContentGetDetails);
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
        public DataSet GetAllData(string CompID, string BranchID, string UserID, string AssetId, string ReqAreaId, 
            string Status, string Fromdate, string Todate, string wfstatus, string Docid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@AssetId",DbType.String, AssetId),
                    objProvider.CreateInitializedParameter("@ReqAreaId",DbType.String, ReqAreaId),
                    objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_get_atlist", prmContentGetDetails);
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
        public string DeleteATDetails(string CompID, string BrchID, string DocNo, string DocDate)
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
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_DelAssetTransfer", prmcontentaddupdate).ToString();
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
        public string ApproveATDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID
           , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks)
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
                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_Approve_AssetTransfer", prmContentInsert);

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
    }
}
