using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.PaymentAdvice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.PaymentAdvice;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.PaymentAdvice
{
    public class PaymentAdvice_SERVICES :PaymentAdvice_ISERVICES
    {
        public DataSet GetFinYearDates(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] bnkReGetFinYearDates = {
                    objProvider.CreateInitializedParameter("@Comp_Id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, BrchID),
                };
                DataSet FinYear = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$GetFinYearDate", bnkReGetFinYearDates);
                return FinYear;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
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
        public DataSet SearchPayAdvItemDetails(string comp_id, string br_id,string FromDate, string ToDate, string InsType, string BankAcc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                     objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                     objProvider.CreateInitializedParameter("@InsType",DbType.String, InsType),
                     objProvider.CreateInitializedParameter("@BankAcc_id",DbType.String, BankAcc_id),
                };
                DataSet GetAdv_BillDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$PayAdv$SearchItemDetail", prmContentGetDetails);
                return GetAdv_BillDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public String InsertPayAdvDetail(DataTable PayAdviceHeaderDetail, DataTable PayAdviceItemDetail)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, PayAdviceHeaderDetail ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, PayAdviceItemDetail ),
                 
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string BP_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_Fin$PayAdv$InsertUpdate", prmcontentaddupdate).ToString();

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
        public DataSet GetPayAdvDetailOnView(string CompId, string BrID, string PAdv_No, string PAdv_Date, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@PAdv_No",DbType.String, PAdv_No),
                    objProvider.CreateInitializedParameter("@PAdv_Date",DbType.String, PAdv_Date),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocID),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SP_Fin$PayAdv$GetViewItemDetail]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet GetPayAdvListDetail(string CompID, string BrchID, string Fromdate, string Todate, string Status, string wfstatus, string UserID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                                                   
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                       
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$PayAdv$GetPAListDetail", prmContentGetDetails);
            return dt;
        }

        public string PADelete(PaymentAdviceModel _PaymentAdviceModel, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@PAdv_no",DbType.String, _PaymentAdviceModel.AdviceNo),
                    objProvider.CreateInitializedParameter("@PAdv_dt",DbType.Date,  _PaymentAdviceModel.AdviceDate),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,DocumentMenuId),
                                                     };
                prmContentGetDetails[4].Size = 100;
                prmContentGetDetails[4].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[SP_fin$PayAdv$Delete]", prmContentGetDetails).ToString();
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
        public string ApprovePADetail(string PAdv_No, string PAdv_Date, string MenuDocId, string Branch, string CompID, string UserID
           , string mac_id, string wf_status, string wf_level, string wf_remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String,Branch),
                                                        objProvider.CreateInitializedParameter("@PAdvno",DbType.String, PAdv_No),
                                                        objProvider.CreateInitializedParameter("@PAdvdt",DbType.String, PAdv_Date),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, MenuDocId),
                                                        objProvider.CreateInitializedParameter("@CreateBy",DbType.String,UserID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                         objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                                                         

                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SP_Fin$PayAdv$Approve]", prmContentInsert);
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

        public DataSet GetPAPrintDeatils(string Comp_ID, string BrchID, string PAdvNo, string PAdvDate, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@PAdv_No",DbType.String, PAdvNo),
                                                        objProvider.CreateInitializedParameter("@PAdv_Date",DbType.String, PAdvDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$PayAdv$GetPrintDetail", prmContentGetDetails);
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
