using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.InvoiceAdjustment;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.InvoiceAdjustment;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.InvoiceAdjustment
{
    public class InvoiceAdjustment_SERVICE: InvoiceAdjustment_ISERVICE
    {
        public DataTable GetEntity(string CompID, string BrchID, string entity_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                     objProvider.CreateInitializedParameter("@entity_type",DbType.String, entity_type),
                                                    };
                DataTable GetEntity = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Entity$Detail", prmContentGetDetails).Tables[0];
                return GetEntity;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAdv_Inv_Details(string comp_id, string br_id, string entity_id, string entity_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@entity_id",DbType.String,entity_id),
                     objProvider.CreateInitializedParameter("@entity_type",DbType.String,entity_type),
                };
                DataSet GetAdv_BillDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetAdv_Bills_Detail", prmContentGetDetails);
                return GetAdv_BillDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public String InsertInvoiceAdjustmentDetail(DataTable InvoiceAdjustmentHeader, DataTable InvoiceAdjustmentAdvDetails,DataTable InvoiceAdjustmentBillsDetail)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, InvoiceAdjustmentHeader ),
                 objprovider.CreateInitializedParameterTableType("@AdvanceDetail",SqlDbType.Structured, InvoiceAdjustmentAdvDetails ),
                 objprovider.CreateInitializedParameterTableType("@BillAdjDetail",SqlDbType.Structured, InvoiceAdjustmentBillsDetail ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string BP_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Fin$InvAdj$InsertUpdate", prmcontentaddupdate).ToString();

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
        public DataSet GetInvoiceAdjListAll(string EntityType, string EntityId, string Fromdate, string Todate, string Status, string CompID, string BrchID,  string wfstatus, string UserID, string DocumentMenuId)
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
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetInvoiceAdjustmentList", prmContentGetDetails);
            return dt;
        }
        public DataSet GetInvoiceAdjustmentDetail(string VouNo, string Voudt, string CompID, string BrchID, string UserID, string DocumentMenuId)
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
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_InvoiceAdj$DetailView]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public string InvAdjDelete(InvoiceAdjustmentModel _InvoiceAdjustmentModel, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String, _InvoiceAdjustmentModel.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _InvoiceAdjustmentModel.Vou_Date),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,DocumentMenuId),
                                                     };
                prmContentGetDetails[4].Size = 100;
                prmContentGetDetails[4].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_fin$gl$InvAdj$Delete]", prmContentGetDetails).ToString();
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

        public string InvoiceAdjustmentApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID)
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
                DataSet VouDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fin$gl$InvAdj$Approve", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = VouDeatils.Tables[0].Rows[0]["vou_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet InvoiceAdjustmentCancel(InvoiceAdjustmentModel _InvoiceAdjustmentModel, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@vou_no",DbType.String,  _InvoiceAdjustmentModel.Vou_No),
                    objProvider.CreateInitializedParameter("@vou_dt",DbType.Date,  _InvoiceAdjustmentModel.Vou_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _InvoiceAdjustmentModel.Create_by ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_fin$InvoiceAdjustmentCancel", prmContentGetDetails);

            return DS;
        }

    }
}
