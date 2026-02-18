using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.SalesVoucher;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.SalesVoucher;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.SalesVoucher
{
    public class SalesVoucher_Service : SalesVoucher_IService
    {
        public Dictionary<string, string> AutoGetCustAccList(string CompID, string AccName, string BrchID)
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

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$gl$detail_GetCustListDetail", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["cust_acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_acc_name"].ToString());
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
        public String InsertSalesVoucherDetail(DataTable SalesVoucherHeader, DataTable SalesVoucherGLDetails, DataTable SVAttachments,DataTable CRCostCenterDetails,string sls_per)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, SalesVoucherHeader ),
                 objprovider.CreateInitializedParameterTableType("@AccountDetail",SqlDbType.Structured, SalesVoucherGLDetails ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, SVAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@Costcenterdetail",SqlDbType.Structured, CRCostCenterDetails ),
                 objprovider.CreateInitializedParameterTableType("@sls_per",SqlDbType.NVarChar, sls_per ),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string SV_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Cmn_fin$gl$vou$InsertUpdate", prmcontentaddupdate).ToString();

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

        public DataSet GetAccCurrOnChange(string acc_id, string CompId, string br_id, string date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@acc_id",DbType.String, acc_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                                                        objProvider.CreateInitializedParameter("@Date",DbType.String, date),

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
        public DataSet GetCustAccIDDetail(string CompID, string BrchID, string CustAccID,string VouDate)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@CustAccID",DbType.String, CustAccID),
                     objProvider.CreateInitializedParameter("@VouDate",DbType.String, VouDate),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetCustAccDetail]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetSalesVoucherListAll( int curr,string CustId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                                                                                                            
                                                        objProvider.CreateInitializedParameter("@CustId",DbType.String, CustId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                          objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                       objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                                                       objProvider.CreateInitializedParameter("@VouType",DbType.String, VouType),
                                                        objProvider.CreateInitializedParameter("@curr",DbType.String, curr),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetSalesVoucherList", prmContentGetDetails);
            return dt;
        }

        public DataSet GetSalesVoucherDetail(string VouNo, string Voudt,string VouType, string CompID, string BrchID, string UserID, string DocumentMenuId)
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
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_SalesVoucher$DetailView]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public string SVDelete(SalesVoucher_Model _SalesVoucher_Model, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, _SalesVoucher_Model.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _SalesVoucher_Model.Vou_Date),
                    objProvider.CreateInitializedParameter("@Doc_id",DbType.String,DocumentMenuId),
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
        public string SalesVoucherApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID)
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
                DataSet VouDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fin$gl$sales$vou$Approve", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = VouDeatils.Tables[0].Rows[0]["vou_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataSet SalesVoucherCancel(SalesVoucher_Model _SalesVoucher_Model, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String,  _SalesVoucher_Model.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _SalesVoucher_Model.Vou_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _SalesVoucher_Model.Create_by ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String, _SalesVoucher_Model.CancelledRemarks),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fin$SalesVoucherCancel", prmContentGetDetails);

            return DS;
        }
        public DataSet CheckSVDetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckPaymentAgainstSV", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetGLVoucherPrintDeatils(string CompID, string br_id, string SVNo, string SVDate, string Vou_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, SVNo),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  SVDate),
                    objProvider.CreateInitializedParameter("@vou_type",DbType.Date,  Vou_type),
                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_fin$get$vou$PrintsDetail]", prmContentGetDetails);
                return ImageDeatils; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable getSalesPersonList(string CompID, string br_id, string userid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@SPersonName",DbType.String,"0"),
                      objProvider.CreateInitializedParameter("@BrchID",DbType.String,br_id),
                      objProvider.CreateInitializedParameter("@user_id",DbType.String,"1001"),
                                                     };
                DataTable GetOpbalFinList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$sls$person$comp_GetSalesPersonList", prmContentGetDetails).Tables[0];
                return GetOpbalFinList;

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
