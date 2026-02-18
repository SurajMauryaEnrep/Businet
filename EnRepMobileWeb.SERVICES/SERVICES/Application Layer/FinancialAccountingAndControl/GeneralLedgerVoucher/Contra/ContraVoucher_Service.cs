using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.Contra;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.Contra;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.Contra
{
   public class ContraVoucher_Service:ContraVoucher_IService
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

        public DataSet GetAccCLBal(string acc_id, string CompId, string Br_ID, string Date)
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
        
        public DataSet GetContraVoucherListAll(string Fromdate, string Todate, string Status, string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/  
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                          objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                       objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                                                       objProvider.CreateInitializedParameter("@VouType",DbType.String, VouType),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetContraList", prmContentGetDetails);
            return dt;
        }

        public DataSet GetContraVoucherDetail(string VouNo, string Voudt, string CompID, string BrchID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, VouNo),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.String, Voudt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_ContraVoucher$DetailView]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public string ContraVoucherDelete(ContraVoucherModel _ContraVoucherModel, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, _ContraVoucherModel.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _ContraVoucherModel.Vou_Date),
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
        public string ContraVoucherApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID)
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

        public DataSet ContraVoucherCancel(ContraVoucherModel _ContraVoucherModel, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String,  _ContraVoucherModel.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _ContraVoucherModel.Vou_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _ContraVoucherModel.Create_by ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String, _ContraVoucherModel.CancelledRemarks),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fin$ContraVoucherCancel", prmContentGetDetails);

            return DS;
        }
        public String InsertContraDetail(DataTable ContraHeader, DataTable ContraGLDetails,DataTable ContraAttachments)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, ContraHeader ),
                 objprovider.CreateInitializedParameterTableType("@AccountDetail",SqlDbType.Structured, ContraGLDetails ),                 
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, ContraAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string Contra_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "fin$gl$Contra$InsertUpdate", prmcontentaddupdate).ToString();

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


    }
}
