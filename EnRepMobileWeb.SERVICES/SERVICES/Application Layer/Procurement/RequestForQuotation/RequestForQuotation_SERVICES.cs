using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.RequestForQuotation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.RequestForQuotation;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.RequestForQuotation
{
    public  class RequestForQuotation_SERVICES : RequestForQuotation_ISERVICES
    {
        public DataSet GetSuppList(string Comp_ID, string branchID, string SearchName, string SuppPros_type)
        {
            try
            {
               // Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, Comp_ID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.Int64, branchID),
                    objProvider.CreateInitializedParameter("@SearchName",DbType.String, SearchName),
                     objProvider.CreateInitializedParameter("@SuppPros_type",DbType.String,SuppPros_type),
                     objProvider.CreateInitializedParameter("@Flag",DbType.String, "SuppList"),
                };
                DataSet searchresult = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$rfq$supp$list", prmContentGetDetails);
                return searchresult;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public  DataSet GetSuppDetails(string Comp_ID, string branchID, string Supp_id, string SuppPros_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, Comp_ID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.Int64, branchID),
                     objProvider.CreateInitializedParameter("@supp_id",DbType.String, Supp_id),
                        objProvider.CreateInitializedParameter("@SuppPros_type",DbType.String,SuppPros_type),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, "SuppDetail"),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$rfq$supp$list", prmContentGetDetails);
                return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public String RFQCancel(RequestForQuotation_Model _RFQModel, string CompID, string br_id, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@rfq_no",DbType.String, _RFQModel.RFQ_no),
                    objProvider.CreateInitializedParameter("@rfq_dt",DbType.Date,  _RFQModel.RFQ_date),
                     objProvider.CreateInitializedParameter("@pr_no",DbType.String, _RFQModel.src_doc_no),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.Date,  _RFQModel.src_doc_dt),
                    objProvider.CreateInitializedParameter("@rfq_type",DbType.String,  _RFQModel.SourceType),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _RFQModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),

                    };

                string mrs_no = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_prc$RFQ$Cancel", prmContentGetDetails).ToString();
                return mrs_no;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet getDetailBySourceDocumentNo(string CompID, string BrchID, string SourDocumentNo)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@SourDocumentNo",DbType.String, SourDocumentNo),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[prc$rfq$GetDetailBySourceDocumentNo]", prmContentGetDetails);
            return ds;
        }
        public DataSet getPRDocumentNo(string CompID, string BrchID, string DocumentNumber)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     //objProvider.CreateInitializedParameter("@SupplierId",DbType.String, SupplierId),
                      objProvider.CreateInitializedParameter("@DocumentNumber",DbType.String, DocumentNumber),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[prc$rfq$PRDocNoList]", prmContentGetDetails);

                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
           
        }
        public string RFQApprove(RequestForQuotation_Model _RFQModel, string CompID, string br_id, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@rfq_no",DbType.String, _RFQModel.RFQ_no),
                    objProvider.CreateInitializedParameter("@rfq_dt",DbType.Date,  _RFQModel.RFQ_date),
                    objProvider.CreateInitializedParameter("@pr_no",DbType.String, _RFQModel.src_doc_no),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.Date,  _RFQModel.src_doc_dt),
                    objProvider.CreateInitializedParameter("@rfq_type",DbType.String,_RFQModel.SourceType),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String,_RFQModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                     };
                
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_prc$RFQApprove]", prmContentGetDetails);
                string DocNo = string.Empty;
                DocNo = ImageDeatils.Tables[0].Rows[0]["rfq_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            
        }
        public string InsertUpdateRFQ(DataTable RFQHeader, DataTable RFQItemDetails, DataTable RFQSuppDetails, DataTable RFQDeleveryShedDetail, DataTable RFQTermAndConDetail, DataTable dtSubItem, DataTable Attachments)
        {
            try
            {
                SqlDataProvider sqlDataProvider = new SqlDataProvider();
                SqlParameter[] sqlParameters =
                {
                sqlDataProvider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,RFQHeader),
                sqlDataProvider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured,RFQItemDetails),
                sqlDataProvider.CreateInitializedParameterTableType("@SuppDetail",SqlDbType.Structured,RFQSuppDetails),
                sqlDataProvider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, Attachments ),
                sqlDataProvider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                sqlDataProvider.CreateInitializedParameterTableType("@DeleveryShedDetail",SqlDbType.Structured,RFQDeleveryShedDetail),
                sqlDataProvider.CreateInitializedParameterTableType("@TermAndConDetail",SqlDbType.Structured,RFQTermAndConDetail),
                 sqlDataProvider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem),

            };
                sqlParameters[4].Size = 100;
                sqlParameters[4].Direction = ParameterDirection.Output;
                string Result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "prc$rfq$InsertUpdate", sqlParameters).ToString();
                string DocNo = string.Empty;
                if (sqlParameters[4].Value != DBNull.Value)
                {
                    DocNo = sqlParameters[4].Value.ToString();
                }
                return DocNo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public DataSet GetRequestForQuotationDeatils(string CompId, string BrchID, string DocNo, string DocDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                      objProvider.CreateInitializedParameter("@rfq_no",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@rfq_date",DbType.String,DocDate),
                                                     };
                DataSet GetMRSList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetRequestForQuotationDeatils_ForPrint", prmContentGetDetails);
                return GetMRSList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetRFQDetailList(string rfq_no, string CompId, string BrchID, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                         objProvider.CreateInitializedParameter("@rfq_no",DbType.String, rfq_no),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                      //  objProvider.CreateInitializedParameter("@reqArea",DbType.String,reqArea),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId)
                                                      };
                DataSet GetMRSList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetRFQList]", prmContentGetDetails);
                return GetMRSList;
            }
            catch (SqlException ex)
            {
                throw ex;
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
        public string RFQDelete(RequestForQuotation_Model _RFQModel, string CompID, string br_id,string RFQ_NO)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@rfq_no",DbType.String, _RFQModel.RFQ_no),
                    objProvider.CreateInitializedParameter("@rfq_dt",DbType.Date,  _RFQModel.RFQ_date),
                    objProvider.CreateInitializedParameter("@rfq_type",DbType.String,  _RFQModel.SourceType),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                     };
                prmContentGetDetails[5].Size = 100;
                prmContentGetDetails[5].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_prc$RFQDelete", prmContentGetDetails).ToString();
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
        public DataSet CheckPQAgainstRFQ(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$rfq$detail_CheckPQAgainstRFQ", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getdetailsRFQ(string CompId, string BranchId, string rfq_no,string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64,BranchId),
                    objProvider.CreateInitializedParameter("@rfq_no",DbType.String,rfq_no),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$rfq$getDetails", prmContentGetDetails);
                return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "RFQ_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetRFQTrackingDetail(string CompId, string BrID, string RFQ_no, string RFQ_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@RFQ_no",DbType.String, RFQ_no),
                                                        objProvider.CreateInitializedParameter("@RFQ_dt",DbType.String, RFQ_dt),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetRFQTrackingView", prmContentGetDetails);
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


    }
}
