using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ServiceVerification;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ServiceVerification;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.ServiceVerification
{
    public class ServiceVerification_SERVICE: ServiceVerification_ISERVICE
    {
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, 'D'),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["supp_id"].ToString(), PARQusData.Tables[0].Rows[i]["supp_name"].ToString());
                    }
                }
                return ddlSuppListDic;

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
        public DataSet GetAlldata(string CompID, string SuppName, string BranchID, string UserID,
            string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                 objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                 objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                 objProvider.CreateInitializedParameter("@SuppType",DbType.String, 'D'),
                 objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                 objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                 objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                 objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                 objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
            };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$Ser$Ver", prmContentGetDetails);
                return PARQusData;
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
        public DataSet GetServicePOList(string Supp_id, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                  

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetServicePO", prmContentGetDetails);
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
        public DataSet GetServicePODetail(string SPONo, string SPODate, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@SPONo",DbType.String, SPONo),
                    objProvider.CreateInitializedParameter("@SPODate",DbType.String, SPODate),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetServicePODetail", prmContentGetDetails);
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
        public String InsertSrVerificationDetail(DataTable SrVerificationHeader, DataTable SrVerificationItemDetails, DataTable SrVerAttachments)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, SrVerificationHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, SrVerificationItemDetails ),               
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, SrVerAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string SrVerNo = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_InsertServiceVerificationDetail", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }


        }
        public DataSet GetVerificationList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),

                                                      };
                DataSet GetVerificationList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetServiceVerificationList", prmContentGetDetails);
                return GetVerificationList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetServiceVerificationDetail(string CompId, string BrID, string VerNo, string VerDate, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@VerNo",DbType.String, VerNo),
                                                        objProvider.CreateInitializedParameter("@VerDate",DbType.String, VerDate),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetServiceVerificationAllDetail", prmContentGetDetails);
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
        public string InsertServiceVerificationApproveDetail(string DocNo, string DocDate, string Branch, string MenuID, string CompID, string ApproveID, string mac_id, string wf_status, string wf_level, string wf_remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@docno",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@docdate",DbType.String, DocDate),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String, Branch),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String,MenuID),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,ApproveID),
                                                        objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                        objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "[sp_AppoveServiceVerificationDetail]", prmContentInsert).ToString();
                return POId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }

        public string ServiceVerificationDelete(ServiceVerificationModel _SrVerModel, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, _SrVerModel.SrVerNo),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.Date,  _SrVerModel.SrVerDate),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                     };
                prmContentGetDetails[4].Size = 100;
                prmContentGetDetails[4].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_Delete_ServiceVerification_Detail]", prmContentGetDetails).ToString();
                //return ActionDeatils;
                string DocNo = string.Empty;
                if (prmContentGetDetails[4].Value != DBNull.Value) // status
                {
                    DocNo = prmContentGetDetails[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet ServiceVerificationCancel(ServiceVerificationModel _SrVerModel, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,  _SrVerModel.SrVerNo),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.Date,  _SrVerModel.SrVerDate),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _SrVerModel.Create_by ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ServiceVerificationCancel", prmContentGetDetails);

            return DS;
        }

    }
}
