using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.PurchaseRequisition;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.PurchaseRequisition;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.PurchaseRequisition
{
    public class PurchaseRequisition_SERVICES: PurchaseRequisition_ISERVICES
    {
        public DataSet GetPRDetailList(string pr_no, string CompId, string BrchID, int reqArea, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@pr_no",DbType.String, pr_no),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@reqArea",DbType.String,reqArea),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetMRSList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetPRList]", prmContentGetDetails);
                return GetMRSList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOTrackingDetail(string CompId, string BrID, string PRNo, string PRDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@PRNo",DbType.String, PRNo),
                                                        objProvider.CreateInitializedParameter("@PRdt",DbType.String, PRDate),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPRTrackingView", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetStatusList(string MenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@menu_id",DbType.String, MenuID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$DocStatus", prmContentGetDetails);
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
        public DataTable GetRequirmentreaList(string CompId, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[prc$pr$GetRequirmentreaList]", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllData(string CompId, string br_id, string pr_no, int reqArea, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),                     
                    objProvider.CreateInitializedParameter("@pr_no",DbType.Int64, pr_no),                     
                    objProvider.CreateInitializedParameter("@reqArea",DbType.String,reqArea),
                    objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetAllData$Pur$Req$List]", prmContentGetDetails);
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
        public string PRDelete(PurchaseRequisition_Model _PRModel, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@pr_no",DbType.String, _PRModel.PR_No),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.Date,  _PRModel.Req_date),
                    objProvider.CreateInitializedParameter("@pr_type",DbType.String,  _PRModel.SourceType),
                    //objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,DocumentMenuId),
                                                     };
                prmContentGetDetails[5].Size = 100;
                prmContentGetDetails[5].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_prc$PRDelete]", prmContentGetDetails).ToString();
                //return ActionDeatils;
                 string DocNo = string.Empty;
                if (prmContentGetDetails[5].Value != DBNull.Value) // status
                {
                    DocNo = prmContentGetDetails[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getdetailsPR(string CompId, string BranchId, string pr_no, string pr_dt, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64,BranchId),
                    objProvider.CreateInitializedParameter("@pr_no",DbType.String,pr_no),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.String, pr_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$pr$getDetails", prmContentGetDetails);
                return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string PRListApprove(PurchaseRequisition_Model _PRModel, string CompID, string br_id,string PR_Date,string SrcType, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@pr_no",DbType.String, _PRModel.PR_No),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.Date,PR_Date),
                    objProvider.CreateInitializedParameter("@pr_type",DbType.String,SrcType),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String,_PRModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_prc$PRApprove]", prmContentGetDetails);
                string DocNo = string.Empty;
                DocNo = ImageDeatils.Tables[0].Rows[0]["pr_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public string InsertUpdatePR(DataTable PRHeader, DataTable PRItemDetails, DataTable dtSubItem, DataTable Attachments)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, PRHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, PRItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, Attachments ),
                 //objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,MRSAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                    objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[prc$PR$InsertUpdatePR]", prmcontentaddupdate).ToString();

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
        public String PRCancel(PurchaseRequisition_Model _PRModel, string CompID, string br_id, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@pr_no",DbType.String, _PRModel.PR_No),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.Date,  _PRModel.Req_date),
                    objProvider.CreateInitializedParameter("@pr_type",DbType.String,  _PRModel.SourceType),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _PRModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),

                    };

                string mrs_no = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_prc$PR$Cancel", prmContentGetDetails).ToString();
                return mrs_no;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public String PRForceClose(PurchaseRequisition_Model _PRModel, string CompID, string br_id, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@pr_no",DbType.String, _PRModel.PR_No),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.Date,  _PRModel.Req_date),
                    objProvider.CreateInitializedParameter("@pr_type",DbType.String,  _PRModel.SourceType),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _PRModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),

                    };

                string mrs_no = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_prc$pr$ForceClose", prmContentGetDetails).ToString();
                return mrs_no;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet CheckRFQAgainstPR(string CompId, string BrchID, string DocNo, string DocDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, DocDate),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$pr$detail_CheckRFQAgainstPR", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet PR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PR_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPurchaseRequisitionDeatils(string Comp_ID, string Br_ID, string PR_No, string PR_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@pr_no",DbType.String, PR_No),
                                                        objProvider.CreateInitializedParameter("@pr_date",DbType.String, PR_Date),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPurchaseRequisitionDeatils_ForPrint", prmContentGetDetails);
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
        public DataSet GetDataReorderLevel(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),                                                       
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetData$ReOrderLevel", prmContentGetDetails);
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
    }
}
