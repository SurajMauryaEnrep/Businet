using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher
{
    public class JournalVoucher_SERVICES : JournalVoucher_ISERVICES
    {

        public DataSet GetCstCntrtype(string CompId, string BrchId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] getCstDetails = {
                                                    objProvider.CreateInitializedParameter("@CompId",DbType.String,CompId),
                                                    objProvider.CreateInitializedParameter("@BrchId",DbType.String,BrchId),

                                                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Usp_GetCostCentretype]", getCstDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string InsertUpdateJV(DataTable JVHeader, DataTable JVAccountDetails, DataTable JVAttachments,DataTable JVCostCenterDetails)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, JVHeader ),
                 objprovider.CreateInitializedParameterTableType("@AccountDetail",SqlDbType.Structured, JVAccountDetails ),
                  objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, JVAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured,JVCostCenterDetails),

                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[fin$gl$JV$InsertUpdateJV]", prmcontentaddupdate).ToString();

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

        public DataSet GetJVDetailList(string CompId, string BrchID, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId, string searchValue)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@searchValue",DbType.String, searchValue),
                                                      };
                DataSet GetJVoucList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetJVList]", prmContentGetDetails);
                return GetJVoucList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet getdetailsJV(string CompId, string BranchId, string Doc_no, string Doc_date, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64,BranchId),
                   objProvider.CreateInitializedParameter("@DocNo",DbType.String, Doc_no),
                   objProvider.CreateInitializedParameter("@DocDate",DbType.String, Doc_date),
                    objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[fin$gl$vou$getDetailsEdit]", prmContentGetDetails);
                return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string JVDelete(JournalVoucher_Model _JVModel, string CompID, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, _JVModel.JV_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _JVModel.JV_Date),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                     };
                prmContentGetDetails[4].Size = 100;
                prmContentGetDetails[4].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_fin$gl$vou$JVDelete]", prmContentGetDetails).ToString();
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

        //public DataSet JVApprove(JournalVoucher_Model _JVModel, string CompID, string br_id, string app_id, string mac_id)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //            objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
        //            objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
        //            objProvider.CreateInitializedParameter("@vou_no",DbType.String, _JVModel.JV_No),
        //            objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _JVModel.JV_Date),
        //            objProvider.CreateInitializedParameter("@CreateBy",DbType.String, app_id ),
        //             objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
        //             };
        //        DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_fin$get$vou$JVApprove]", prmContentGetDetails);
        //        return ImageDeatils; ;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet GetGLVoucherPrintDeatils(string CompID, string br_id, string JVNo, string JVDate, string Vou_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, JVNo),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  JVDate),
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

        public string JVCancel(JournalVoucher_Model _JVModel, string CompID, string br_id, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@vou_no",DbType.String,_JVModel.JV_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,_JVModel.JV_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _JVModel.Create_by),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String, _JVModel.CancelledRemarks),

                    };

                string mrs_no = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_fin$gl$vou$JV$Cancel]", prmContentGetDetails).ToString();
                return mrs_no;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public string InsertJornlVoucApproveDetails(string JVNo, string JVDate, string CompID, string BrchID, string DocumentMenuId, string UserID, string mac_id, string A_Status, string A_Level, string A_Remarks, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@docno",DbType.String, JVNo),
                                                        objProvider.CreateInitializedParameter("@docdate",DbType.String, JVDate),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String,DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                        objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                        objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                };
                string JvoucId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "fin$gl$vou$JVApproveDetails", prmContentInsert).ToString();
                return JvoucId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet CheckJVDetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckVouAgainstJV", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet getReplicateWith(string CompID, string br_id, string OrderType, string SarchValue) // Added By Nitesh 26-10-2023 11:02 for Bind Shopfloore data
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@order_type", DbType.String, OrderType),
                     objProvider.CreateInitializedParameter("@SarchValue", DbType.String, SarchValue)
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fin$gl$vou$detail$replicate", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetReplicateWithItemdata(string CompID, string br_id, string Vou_no, string vou_dt,string vou_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Vou_no", DbType.String, Vou_no),
                     objProvider.CreateInitializedParameter("@vou_dt", DbType.String, vou_dt),
                     objProvider.CreateInitializedParameter("@vou_type", DbType.String, vou_type)
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fin$gl$vou_Replicate", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
