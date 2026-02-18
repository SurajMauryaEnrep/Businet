using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.TDSPosting;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.TDSPosting
{
    public class TDSPosting_SERVICES: TDSPosting_ISERVICES
    {
        /*------------------------Tds Posting List-------------------------*/
        public DataSet GetTdsPostngList(string compId, string brId, string Year, string month, string status
            , string documentMenuId, string wfStatus, string UserID,string searchValue)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@comp_id",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@br_id",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@Year",DbType.String,Year),
                sqlDataProvider.CreateInitializedParameter("@month",DbType.String,month),
                sqlDataProvider.CreateInitializedParameter("@status",DbType.String,status),
                sqlDataProvider.CreateInitializedParameter("@doc_no",DbType.String,documentMenuId),
                sqlDataProvider.CreateInitializedParameter("@wfStatus",DbType.String,wfStatus),
                sqlDataProvider.CreateInitializedParameter("@UserId",DbType.String,UserID),
                sqlDataProvider.CreateInitializedParameter("@searchValue",DbType.String,searchValue)
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_TdsPostingList", sqlParameters);
            return dataSet;
        }

        /*------------------------Tds Posting List-------------------------*/
        public DataSet GetTdsPostngDetail(string compId, string brId,string UserID, string month,string year,string tds_id)
        {
            try
            {
                SqlDataProvider sqlDataProvider = new SqlDataProvider();
                SqlParameter[] sqlParameters =
                {
                sqlDataProvider.CreateInitializedParameter("@compId",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@brId",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@UserID",DbType.String,UserID),
                sqlDataProvider.CreateInitializedParameter("@month",DbType.String,month),
                sqlDataProvider.CreateInitializedParameter("@year",DbType.String,year),
                sqlDataProvider.CreateInitializedParameter("@tds_id",DbType.String,tds_id),
            };
                DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_TdsPostingDetailPageLoad", sqlParameters);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        public DataSet GetTdsPostngDetailToAdd(string compID, string brId, string from_dt
            , string to_dt)
        {
            try
            {
                SqlDataProvider sqlDataProvider = new SqlDataProvider();
                SqlParameter[] sqlParameters =
                {
                sqlDataProvider.CreateInitializedParameter("@comp_id",DbType.String,compID),
                sqlDataProvider.CreateInitializedParameter("@br_id",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@from_dt",DbType.String,from_dt),
                sqlDataProvider.CreateInitializedParameter("@to_dt",DbType.String,to_dt)
            };
                DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get_TDS_Posting_Details", sqlParameters);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
         
        }
        public string InsertTdsPostingDetails(DataTable tdsPostingHeader, DataTable tdsPostingDetail, DataTable tdsPostingSlabDetail
            ,DataTable TdsPostingGLDetail, DataTable TdsPostingGLDetailCC, DataTable TdsPostingSuppInvoice, DataTable TdsPostingSuppInvSlab)
        {
            try
            {
                SqlDataProvider sqlDataProvider = new SqlDataProvider();
                SqlParameter[] sqlParameters =
                {
                 sqlDataProvider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, tdsPostingHeader ),
                 sqlDataProvider.CreateInitializedParameterTableType("@tdsPostingDetail",SqlDbType.Structured, tdsPostingDetail ),
                 sqlDataProvider.CreateInitializedParameterTableType("@tdsPostingSlabDetail",SqlDbType.Structured, tdsPostingSlabDetail ),
                 sqlDataProvider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 sqlDataProvider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured, TdsPostingGLDetail ),
                 sqlDataProvider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, TdsPostingGLDetailCC ),
                 sqlDataProvider.CreateInitializedParameterTableType("@SuppInvDetals",SqlDbType.Structured, TdsPostingSuppInvoice ),
                 sqlDataProvider.CreateInitializedParameterTableType("@SuppInvSlabDetals",SqlDbType.Structured, TdsPostingSuppInvSlab ),
            };
                sqlParameters[3].Size = 100;
                sqlParameters[3].Direction = ParameterDirection.Output;

                string dataSet = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "TDS_PostingInsertDetails", sqlParameters).ToString();
                string DocNo = string.Empty;

                if (sqlParameters[3].Value != DBNull.Value) // status
                {
                    DocNo = sqlParameters[3].Value.ToString();
                }
                return DocNo;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        public string DeleteTdsPostingDetails(string compID, string brId, string monthNo, string year)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                 sqlDataProvider.CreateInitializedParameter("@compId",DbType.String, compID ),
                 sqlDataProvider.CreateInitializedParameter("@brId",DbType.String, brId ),
                 sqlDataProvider.CreateInitializedParameter("@month",DbType.String, monthNo ),
                sqlDataProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                sqlDataProvider.CreateInitializedParameter("@year",DbType.String, year ),
                 
            };
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Size = 100;
            string dataSet = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "TDS_PostingDeleteDetails", sqlParameters).ToString();
            string DocNo = string.Empty;
            if (sqlParameters[3].Value != DBNull.Value) // status
            {
                DocNo = sqlParameters[3].Value.ToString();
            }
            return DocNo;
        }
        public DataSet GetMonthOnBehalfYear(string compID, string brId, string tds_year)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@compId",DbType.String,compID),
                sqlDataProvider.CreateInitializedParameter("@brId",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@year",DbType.String,tds_year),
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get_TDS_MonthByYear", sqlParameters);
            return dataSet;
        }
        public DataSet GetTdsSuppWiseInvoiceDetails(string compID, string brId, string supp_id, string preVlStD, string preVlEdD, string status, string tds_id)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@comp_id",DbType.String,compID),
                sqlDataProvider.CreateInitializedParameter("@br_id",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@supp_id",DbType.String,supp_id),
                sqlDataProvider.CreateInitializedParameter("@start_dt",DbType.String,preVlStD),
                sqlDataProvider.CreateInitializedParameter("@end_dt",DbType.String,preVlEdD),
                sqlDataProvider.CreateInitializedParameter("@statusFlg",DbType.String,status),
                sqlDataProvider.CreateInitializedParameter("@tds_id",DbType.String,tds_id),
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_TdsValuePInvoiceDetails", sqlParameters);
            return dataSet;
        }
        public DataSet GetTdsSuppWiseTaxableValueDetails(string compID, string brId, string Year, string Month, string suppId
            , string StartDate, string EndDate)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@compId",DbType.String,compID),
                sqlDataProvider.CreateInitializedParameter("@brId",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@year",DbType.String,Year),
                sqlDataProvider.CreateInitializedParameter("@month",DbType.String,Month),
                sqlDataProvider.CreateInitializedParameter("@supp_id",DbType.String,suppId),
                sqlDataProvider.CreateInitializedParameter("@start_dt",DbType.String,StartDate),
                sqlDataProvider.CreateInitializedParameter("@end_dt",DbType.String,EndDate),
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_TdsPostingTaxableValueDetail", sqlParameters);
            return dataSet;
        }
        public string ApproveTdsPostingDetails(string compID, string brId, string tds_id, string a_Status, string a_Level, string a_Remarks, string userID, string mac_id, string documentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, brId),
                    objProvider.CreateInitializedParameter("@tds_id",DbType.String, tds_id),
                    objProvider.CreateInitializedParameter("@CreatedBy",DbType.String, userID ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, a_Status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, a_Level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, a_Remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, documentMenuId),
                     };
                DataSet VouDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Tds_PostingApprove", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = VouDeatils.Tables[0].Rows[0]["tdsTransDetail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
