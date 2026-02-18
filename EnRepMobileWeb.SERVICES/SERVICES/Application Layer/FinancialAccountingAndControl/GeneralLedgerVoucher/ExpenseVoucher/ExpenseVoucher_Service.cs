using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.ExpenseVoucher;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.ExpenseVoucher
{
    public class ExpenseVoucher_Service : ExpenseVoucher_ISERVICE
    {
        public DataTable GetGLAccList(string CompId,string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getGlList = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                                                };
                DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Usp_GetLiabilityGlList]", getGlList).Tables[0];
                return dt;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetClosingBal(string CompId, string BrID,string acc_id, string Vou_Dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getVoulist = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                                                    objProvider.CreateInitializedParameter("@Acc_id",DbType.String,acc_id),
                                                    objProvider.CreateInitializedParameter("@Vou_Dt",DbType.String,Vou_Dt),
                                                };
                DataTable dt_closingbal = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc_getAccountClosingBal", getVoulist).Tables[0];
                return dt_closingbal;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable Getgl_accgroup(string CompId, string BrID, string acc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getVoulist = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                                                    objProvider.CreateInitializedParameter("@Acc_id",DbType.String,acc_id),
                                                };
                DataTable dt_closingbal = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc_getAccountgroup", getVoulist).Tables[0];
                return dt_closingbal;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable Get_VouDetails(string compid, string brid, string acc_id, string vou_no, string vou_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getVoulist = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,compid),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,brid),
                                                    objProvider.CreateInitializedParameter("@Acc_id",DbType.String,acc_id),
                                                    objProvider.CreateInitializedParameter("@Vou_no",DbType.String,vou_no),
                                                    objProvider.CreateInitializedParameter("@Vou_dt",DbType.String,vou_dt),
                                                };
                DataTable dt_closingbal = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc_getVouDetails", getVoulist).Tables[0];
                return dt_closingbal;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVouList(string CompId, string BrID, string acc_id, string searchval)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getVoulist = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                                                    objProvider.CreateInitializedParameter("@Acc_id",DbType.String,acc_id),
                                                    objProvider.CreateInitializedParameter("@searchval",DbType.String,searchval),
                                                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Usp_ExpeseVou$GetPayeeAccVouList]", getVoulist);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetExpenseAcc(string CompId, string BrID,string searchval)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getExpenseLst = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                                                    objProvider.CreateInitializedParameter("@searchval",DbType.String,searchval),
                                                };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Usp_GetExpenseAccount]", getExpenseLst);
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string InsertExpenseVouDetail(DataTable ExpenseVoucherHeader, DataTable ExpVouPaymentDetail, DataTable ExpVouExpenseDesc, DataTable BPAttachments, DataTable DtblVouGLDetail, DataTable CostCenterDetails)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, ExpenseVoucherHeader ),
                 objprovider.CreateInitializedParameterTableType("@PaymentDetail",SqlDbType.Structured, ExpVouPaymentDetail ),
                 objprovider.CreateInitializedParameterTableType("@ExpenseDesc",SqlDbType.Structured, ExpVouExpenseDesc ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, BPAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                  objprovider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured, DtblVouGLDetail ),
                 objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CostCenterDetails ),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string EV_No = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Usp$fin$gl$EV$InsertUpdate", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet GetExpenseVouDetails(string compId, string brId, string UserID, string vou_no, string vou_dt)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@comp_id",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@br_id",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@vou_no",DbType.String,vou_no),
                sqlDataProvider.CreateInitializedParameter("@vou_dt",DbType.String,vou_dt),
                sqlDataProvider.CreateInitializedParameter("@UserID",DbType.String,UserID),
                
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$fin$gl$EV$Detail", sqlParameters);
            return dataSet;
        }

        public string DeleteExpenseDetail(string compID, string brId, string Vou_No, string Vou_Dt)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                 sqlDataProvider.CreateInitializedParameter("@compId",DbType.String, compID ),
                 sqlDataProvider.CreateInitializedParameter("@brId",DbType.String, brId ),
                 sqlDataProvider.CreateInitializedParameter("@Vou_No",DbType.String, Vou_No ),
                 sqlDataProvider.CreateInitializedParameter("@Vou_Dt",DbType.String, Vou_Dt ),
                sqlDataProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
               

            };
            sqlParameters[4].Direction = ParameterDirection.Output;
            sqlParameters[4].Size = 100;
            string dataSet = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Usp$FinExpenseVou$Delete", sqlParameters).ToString();
            string DocNo = string.Empty;
            if (sqlParameters[4].Value != DBNull.Value) // status
            {
                DocNo = sqlParameters[4].Value.ToString();
            }
            return DocNo;
        }

        public DataSet GetExpenseVouList(string CompId, string BrId,string FromDate,string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetExpVouList = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, ToDate),

            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Usp$ExpenseVoucher$ListPageDetail]", prmContentGetExpVouList);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataSet SerachListExpenseVoucher(string compId, string BrId, string Acc_Id, string FormDate, string Todate, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmGetSearchExpenseVou = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@Acc_Id",DbType.String, Acc_Id),
                    objProvider.CreateInitializedParameter("@FromDate",DbType.String, FormDate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),

            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Usp$ExpenseVou$OnSearchList]", prmGetSearchExpenseVou);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public string ApproveExpVouDt(string compID, string brId, string vou_no, string vou_dt, string a_Status, string a_Level, string a_Remarks, string userID, string mac_id, string documentMenuId, string narr)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@compid",DbType.Int16, compID),
                    objProvider.CreateInitializedParameter("@brid",DbType.String, brId),
                    objProvider.CreateInitializedParameter("@Vou_No",DbType.String, vou_no),
                    objProvider.CreateInitializedParameter("@Vou_Date",DbType.String, vou_dt),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, userID ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, a_Status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, a_Level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, a_Remarks),
                        objProvider.CreateInitializedParameter("@menuid",DbType.String, documentMenuId),
                         objProvider.CreateInitializedParameter("@paygl_narr",DbType.String, narr),
                     };
                DataSet VouDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$ExpenseVoucher$Approve", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = VouDeatils.Tables[0].Rows[0]["VouDetail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public string CancelExpVouDt(string compID, string brId, string vou_no, string vou_dt, 
            string userID, string mac_id,string CancelledRemarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@compId",DbType.Int16, compID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                    objProvider.CreateInitializedParameter("@Vou_No",DbType.String, vou_no),
                    objProvider.CreateInitializedParameter("@Vou_Dt",DbType.String, vou_dt),
                    objProvider.CreateInitializedParameter("@mac_Id",DbType.String, mac_id ),
                     objProvider.CreateInitializedParameter("@UserId",DbType.String, userID),
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String, CancelledRemarks),
                        
                     };
                DataSet VouDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$ExpenseVou$Cancel", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = VouDeatils.Tables[0].Rows[0]["VouDetail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
