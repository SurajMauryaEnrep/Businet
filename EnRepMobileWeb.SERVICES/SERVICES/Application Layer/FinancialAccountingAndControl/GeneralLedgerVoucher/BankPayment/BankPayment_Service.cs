using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment
{
     public class BankPayment_Service : BankPayment_IService
    {
        public Dictionary<string, string> AutoGetBankAccList(string CompID, string AccName, string BrchID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@AccName",DbType.String, AccName),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$gl$detail_GetBankListDetail", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["bank_acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["bank_acc_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

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
        public String InsertBankPaymentDetail(DataTable BankPaymentHeader, DataTable BankPaymentGLDetails, DataTable BankPaymentBillAdjDetail, DataTable BPAttachments,DataTable CRCostCenterDetails, string PDC, string InterBrch)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, BankPaymentHeader ),
                 objprovider.CreateInitializedParameterTableType("@AccountDetail",SqlDbType.Structured, BankPaymentGLDetails ),
                 objprovider.CreateInitializedParameterTableType("@BillAdjDetail",SqlDbType.Structured, BankPaymentBillAdjDetail ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, BPAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@Costcenterdetail",SqlDbType.Structured,CRCostCenterDetails),
                 objprovider.CreateInitializedParameter("@PDC",DbType.String, PDC),
                     objprovider.CreateInitializedParameter("@InterBrch",DbType.String, InterBrch),

                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string BP_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "fin$gl$BP$InsertUpdate", prmcontentaddupdate).ToString();

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
        public DataTable GetCurrList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                };
                DataTable GetCurr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetCurrOpBal", prmContentGetDetails).Tables[0];
                return GetCurr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetGLVoucherDtForPDC(string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String,BrID),
                };
                DataTable GetGLDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetGLVouDtforPDC", prmContentGetDetails).Tables[0];
                return GetGLDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetBranchList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                };
                DataTable GetGLDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_GetBranchList", prmContentGetDetails).Tables[0];
                return GetGLDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAccCurrOnChange(string acc_id, string CompId, string Br_ID, string Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@acc_id",DbType.String, acc_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                          objProvider.CreateInitializedParameter("@Date",DbType.String, Date),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAccCurrId", prmContentGetDetails);
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
        public DataSet GetBankAccIDDetail(string CompID, string BrchID, string BankAccID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@BankAccID",DbType.String, BankAccID),         
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetBankAccDetail]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetBankPaymentListAll(string SrcType, string BankId, string Fromdate, string Todate, string Status
            , string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId
            , string Currency, string InsType, string RecoStatus
            , string skip, string pageSize, string searchValue, string sortColumn, string sortColumnDir, string Flag)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                        objProvider.CreateInitializedParameter("@SrcType",DbType.String,SrcType),
                        objProvider.CreateInitializedParameter("@BankId",DbType.String, BankId),
                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                        objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@VouType",DbType.String, VouType),
                        objProvider.CreateInitializedParameter("@CurrId",DbType.String, Currency),
                        objProvider.CreateInitializedParameter("@InsType",DbType.String, InsType),
                        objProvider.CreateInitializedParameter("@Recostatus",DbType.String, RecoStatus),
                        objProvider.CreateInitializedParameter("@Skip",DbType.String,skip),
                        objProvider.CreateInitializedParameter("@PageSize",DbType.String,pageSize),
                        objProvider.CreateInitializedParameter("@Search",DbType.String,searchValue),
                        objProvider.CreateInitializedParameter("@sortColumn",DbType.String,sortColumn),
                        objProvider.CreateInitializedParameter("@sortColumnDir",DbType.String,sortColumnDir),
                        objProvider.CreateInitializedParameter("@Flag",DbType.String,Flag),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetBankPaymentList", prmContentGetDetails);
            return dt;
        }

        public DataSet GetBankPaymentDetail(string VouType,string VouNo, string Voudt, string CompID, string BrchID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@VouType",DbType.String, VouType),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, VouNo),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.String, Voudt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_BankPayment$DetailView]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public string BPDelete(BankPayment_Model _BankPayment_Model, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, _BankPayment_Model.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _BankPayment_Model.Vou_Date),
                      objProvider.CreateInitializedParameter("@Doc_id",DbType.String,  DocumentMenuId),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                     };
                prmContentGetDetails[5].Size = 100;
                prmContentGetDetails[5].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_fin$gl$vou$Delete]", prmContentGetDetails).ToString();
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
        public string BankPaymentApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level
            , string wf_remarks, string comp_id, string br_id, string mac_id, string DocID,string int_br_nurr)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, VouNo),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  VouDate),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, userid ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                        objProvider.CreateInitializedParameter("@int_br_nurr",DbType.String, int_br_nurr),
                     };
                DataSet VouDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fin$gl$vou$Approve", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = VouDeatils.Tables[0].Rows[0]["vou_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataSet BankPaymentCancel(BankPayment_Model _BankPayment_Model, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String,  _BankPayment_Model.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _BankPayment_Model.Vou_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _BankPayment_Model.Create_by ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String, _BankPayment_Model.CancelledRemarks),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fin$BankPaymentCancel", prmContentGetDetails);

            return DS;
        }
        public DataSet GetBillDetail(string CompID, string BrchID, string AccId,string fromdt,string todt,string flag, string DocumentNumber, string Status,string Curr)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@AccId",DbType.String, AccId),
                      objProvider.CreateInitializedParameter("@fromdt",DbType.String, fromdt),
                       objProvider.CreateInitializedParameter("@todt",DbType.String, todt),
                       objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                       objProvider.CreateInitializedParameter("@DocumentNumber",DbType.String, DocumentNumber),
                       objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                objProvider.CreateInitializedParameter("@Curr",DbType.String, Curr),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetARAPBillsDetail]", prmContentGetDetails);
            return ds;
        }

        public DataSet CheckAdvancePayment(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckAdvancePaymentForBP_BR", prmContentGetDetails);
                return Get_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet SearchAdjustedAmountDetail(string compId, string brId, string InVNo, string InvDate, string Accid, string VouNo, int AccTyp)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@InVNo",DbType.String, InVNo),
                                                        objProvider.CreateInitializedParameter("@InvDate",DbType.String, InvDate),
                                                        objProvider.CreateInitializedParameter("@Accid",DbType.String, Accid),
                                                        objProvider.CreateInitializedParameter("@VouNo",DbType.String, VouNo),
                                                        objProvider.CreateInitializedParameter("@AccTyp",DbType.String, AccTyp),

                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Sp_CMN_Fin$AdjustedAmountDetail", prmContentGetDetails);
                return GetInvoiceDetailList;
                //return GetInvoiceDetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
