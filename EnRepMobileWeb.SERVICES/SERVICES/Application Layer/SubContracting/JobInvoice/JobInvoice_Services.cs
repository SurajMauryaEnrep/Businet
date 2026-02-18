using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.JobInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.JobInvoice;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SubContracting.JobInvoice
{
   public class JobInvoice_Services : JobInvoice_IServices
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
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, "D"),
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
            //return null;
        }

        public DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppAddrDetails", prmContentGetDetails);
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
        public DataSet GetGoodReceiptNoteList(string Supp_id, string CompId, string BrID,string DocumentNumber)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                    objProvider.CreateInitializedParameter("@DocumentNumber",DbType.String, DocumentNumber),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$JobInv$detail_GetGoodReceiptNoteSCList", prmContentGetDetails);
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
        public DataSet GetJOandGoodReceiptNoteSCDetails(string GRNNo, string GRNDate, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@GRNNo",DbType.String, GRNNo),
                    objProvider.CreateInitializedParameter("@GRNDate",DbType.String, GRNDate),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$JobInv$detail_JOandGRNSCDetail", prmContentGetDetails);
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

        public DataSet GetAllGLDetails(DataTable GLDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured,GLDetail),
                                                    };

                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetGLDetail", prmcontentaddupdate);
                return GetGlDt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }

        public string InsertJI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            ,DataTable CRCostCenterDetails, DataTable DtblTdsDetail,DataTable DtblOcTdsDetail, string Tds_amt)
        {
            try
            { 
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DtblOCTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DtblIOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouDetail),
                                                        objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CRCostCenterDetails),
                                                        objprovider.CreateInitializedParameterTableType("@DtblOcTdsDetail",SqlDbType.Structured, DtblOcTdsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TDS_Details",SqlDbType.Structured, DtblTdsDetail),
                                                        objprovider.CreateInitializedParameter("@TDS_Amt",DbType.String, Tds_amt),
                                                    };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sc$jobinv$detail_InsertJI_Details]", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[6].Value != DBNull.Value)
                {
                    DocNo = prmcontentaddupdate[6].Value.ToString();
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
        public DataSet GetJoInvListandSrchDetail(string CompId, string BrchID, JI_ListModel _JI_ListModel, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String,_JI_ListModel.SuppID),
                                                       objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_JI_ListModel.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_JI_ListModel.ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _JI_ListModel.Status),
                                                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                             objProvider.CreateInitializedParameter("@Docid",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$JoInv$GetJISC_ListDeatil", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobInvDetailEditUpdate(string CompId, string BrchID, string JISC_NO, string JISC_Date, string UserID, string DocID,string VouType)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@JINo",DbType.String, JISC_NO),
                                                        objProvider.CreateInitializedParameter("@JIDate",DbType.String, JISC_Date),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                        objProvider.CreateInitializedParameter("@VouType",DbType.String, VouType),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$jobinv$GetJISC_DetailsOnDblClk", prmContentGetDetails);
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
        public string JInv_DeleteDetail(JobInvoiceModel _JobInvoiceModel, string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@Inv_No",DbType.String,_JobInvoiceModel.JInv_No),
                                                        objProvider.CreateInitializedParameter("@Inv_Date",DbType.String,_JobInvoiceModel.JInv_Dt),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$jobinv$DeleteJISC_Details", prmContentInsert).ToString();
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
        public string JIApproveDetails(string CompID, string BrchID, string JI_No, string JI_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks, string VoucherNarr, string BP_Nurration, string DN_Nurration)
        {
            
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                              objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                              objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                              objProvider.CreateInitializedParameter("@invno",DbType.String, JI_No),
                                                              objProvider.CreateInitializedParameter("@invdate",DbType.String, JI_Date),
                                                              objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                              objProvider.CreateInitializedParameter("@menuid",DbType.String, MenuID),
                                                              objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                              objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                              objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                              objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                                                              objProvider.CreateInitializedParameter("@VoucherNarr",DbType.String, VoucherNarr),
                                                              objProvider.CreateInitializedParameter("@Bp_Narr",DbType.String, BP_Nurration ),
                                                              objProvider.CreateInitializedParameter("@Dn_Narr",DbType.String, DN_Nurration),
                                                                //objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),

                                                              
                                                         
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$jobinv$Approved_JISC_Details", prmContentInsert).ToString();
                return POId;

                //prmContentInsert[10].Size = 100;
                //prmContentInsert[10].Direction = ParameterDirection.Output;
                //DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$pinv$detail_Approved_PI_Details", prmContentInsert);

                //string DocNo = string.Empty;
                //if (prmContentInsert[10].Value != DBNull.Value)
                //{
                //    DocNo = prmContentInsert[10].Value.ToString();
                //}

                //return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet JobInvCancel(JobInvoiceModel _JobInvoiceModel, string CompID, string br_id, string mac_id,string Nurr)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@compid",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@brid",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@invno",DbType.String,  _JobInvoiceModel.JInv_No),
                    objProvider.CreateInitializedParameter("@invdate",DbType.Date,  _JobInvoiceModel.JInv_Dt),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _JobInvoiceModel.CreatedBy ),
                    objProvider.CreateInitializedParameter("@Narration",DbType.String, _JobInvoiceModel.Narration ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@Nurr",DbType.String, Nurr),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$jobinv$Cancelled_JISCDetails", prmContentGetDetails);

            return DS;
        }
        public DataSet GetJobInvoiceDeatilsForPrint(string CompID, string BrchID, string invNo, string invDt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, invNo),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, invDt),
                                                        //objProvider.CreateInitializedParameter("@flag",DbType.String, inv_type),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetJobInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckJIDetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SC$JobInv$CheckReturnAgainstJI", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
