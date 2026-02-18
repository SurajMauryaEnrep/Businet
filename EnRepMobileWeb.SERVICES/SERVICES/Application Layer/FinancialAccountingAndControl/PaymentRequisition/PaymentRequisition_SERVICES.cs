using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.PaymentRequisition;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.PaymentRequisition;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.PaymentRequisition
{
    public class PaymentRequisition_SERVICES : PaymentRequisition_ISERVICES
    {
        public DataSet GetAllData(string CompId, string br_id, string reqArea, string Fromdate, string Todate, string Status, 
            string UserID, string wfstatus, string DocumentMenuId, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),          
                    objProvider.CreateInitializedParameter("@reqArea",DbType.String,reqArea),
                    objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[fin$paym$req$list$All$data]", prmContentGetDetails);
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

        public string InsertUpdatePR(DataTable PRHeader, DataTable Attachments)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, PRHeader ),               
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, Attachments ),               
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                   
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[fin$Pay$req$InsertUpdate]", prmcontentaddupdate).ToString();

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
        public string DeleteData(PaymentRequisitionModel DeleteModel, string CompID, string BrchID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameter("@CompID",DbType.String, CompID ),               
                 objprovider.CreateInitializedParameter("@BrchID",DbType.String, BrchID ),               
                 objprovider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId ),               
                 objprovider.CreateInitializedParameter("@RequisitionNumber",DbType.String, DeleteModel.RequisitionNumber ),               
                 objprovider.CreateInitializedParameter("@Req_date",DbType.String, DeleteModel.Req_date ),               
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                   
                };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[fin$Pay$Req$Delete]", prmcontentaddupdate).ToString();

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
      public  DataSet GetDetailData(string CompID, string BrchID, string pr_no, string pr_dt, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@pr_no",DbType.String,pr_no),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.String,pr_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),                 
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[fin$pay$req$Get$Details]", prmContentGetDetails);
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
        public string DocumentApprove(PaymentRequisitionModel ApproveModel, string CompID, string br_id, string PR_Date,  string wf_status, string wf_level, string wf_remarks, string mac_id, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@pr_no",DbType.String, ApproveModel.RequisitionNumber),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.Date,PR_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String,ApproveModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[fin$Pay$Req$Detail$Approve]", prmContentGetDetails);
                string DocNo = string.Empty;
                DocNo = ImageDeatils.Tables[0].Rows[0]["pr_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        } 
        public string CancelDocument(PaymentRequisitionModel CancelModel, string CompID, string BrchID, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@pr_no",DbType.String, CancelModel.RequisitionNumber),
                    objProvider.CreateInitializedParameter("@pr_dt",DbType.Date,CancelModel.Req_date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String,CancelModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),                       
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String, CancelModel.CancelledRemarks),                       
                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[fin$Pay$Req$Detail$Cancelled]", prmContentGetDetails);
                string DocNo = string.Empty;
                DocNo = ImageDeatils.Tables[0].Rows[0]["pr_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
    }
}
