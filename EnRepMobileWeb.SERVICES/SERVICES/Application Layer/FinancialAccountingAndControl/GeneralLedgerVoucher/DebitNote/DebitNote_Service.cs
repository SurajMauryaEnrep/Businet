using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.DebitNote;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.DebitNote;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.DebitNote
{
    public class DebitNote_Service : DebitNote_IService
    {
        public Dictionary<string, string> AutoGetEntityList(string CompID, string AccName, string BrchID, string Entitytype, string flag)
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
                              objProvider.CreateInitializedParameter("@Entitytype",DbType.String, Entitytype),
                              objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$gl$detail_GetEntityListDetail]", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["entity_acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["entity_acc_name"].ToString());
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

        public DataSet CheckPaymentAgainstDebitNote(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckPaymentAgainstDebitNote", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string DebitNoteApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID)
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
                DataSet VouDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fin$gl$DebitNote$Approve", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = VouDeatils.Tables[0].Rows[0]["vou_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataSet DebitNoteCancel(DebitNote_Model _DebitNote_Model, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String,  _DebitNote_Model.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _DebitNote_Model.Vou_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _DebitNote_Model.Create_by ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String, _DebitNote_Model.CancelledRemarks),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fin$DebitNoteCancel", prmContentGetDetails);

            return DS;
        }

        public string DNDelete(DebitNote_Model _DebitNote_Model, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, _DebitNote_Model.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _DebitNote_Model.Vou_Date),
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
        public DataTable AutoGetEntityList1(string CompID, string AccName, string BrchID, string Entitytype, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@AccName",DbType.String, AccName),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                              objProvider.CreateInitializedParameter("@Entitytype",DbType.String, Entitytype),
                              objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                                                    };
                DataTable GetEntity = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$gl$detail_GetEntityListDetail", prmContentGetDetails).Tables[0];
                return GetEntity;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDebitNoteDetail(string VouNo, string Voudt,string VouType, string CompID, string BrchID, string UserID, string DocumentMenuId)
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
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_DebitNote$DetailView]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataSet GetDebitNoteListAll(string EntityType, string EntityId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                                                   
                                                       objProvider.CreateInitializedParameter("@EntityType",DbType.String,EntityType),
                                                        objProvider.CreateInitializedParameter("@EntityId",DbType.String, EntityId),
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
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetDebitNoteList", prmContentGetDetails);
            return dt;
        }

        public DataSet GetEntityIDDetail(string CompID, string BrchID, string EntityID,string VouDate)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@EntityID",DbType.String, EntityID),
                     objProvider.CreateInitializedParameter("@VouDate",DbType.String, VouDate),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetEntityDetail]", prmContentGetDetails);
            return ds;
        }

        public string InsertDebitNoteDetail(DataTable DebitNoteHeader, DataTable DebitNoteGLDetails
            , DataTable DbNoAttachments,DataTable CRCostCenterDetails,DataTable DebitNoteBillAdj,string conv_rate, string sls_per)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, DebitNoteHeader ),
                 objprovider.CreateInitializedParameterTableType("@AccountDetail",SqlDbType.Structured, DebitNoteGLDetails ),
                    objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, DbNoAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@Costcenterdetail",SqlDbType.Structured,CRCostCenterDetails),
                 objprovider.CreateInitializedParameterTableType("@BillAdjDetail",SqlDbType.Structured,DebitNoteBillAdj),
                 objprovider.CreateInitializedParameterTableType("@Conv_rate",SqlDbType.NVarChar,conv_rate),
                 objprovider.CreateInitializedParameterTableType("@sls_per",SqlDbType.NVarChar,sls_per),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string DN_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "fin$gl$DN$InsertUpdate", prmcontentaddupdate).ToString();

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
