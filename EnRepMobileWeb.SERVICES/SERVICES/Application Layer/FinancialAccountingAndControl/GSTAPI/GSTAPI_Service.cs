using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GSTAPI;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.GSTAPI
{
    public class GSTAPI_Service : GSTAPI_IService
    {
        public DataSet GetGSTAPIPostingDetails(string compId, string brId, string fromDate, string toDate, string dataType
            , string docStatus,string GSTR_DateOption,string GstCat)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                sqlDataProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                sqlDataProvider.CreateInitializedParameter("@DataType",DbType.String,dataType),
                sqlDataProvider.CreateInitializedParameter("@DocStatus",DbType.String,docStatus),
                sqlDataProvider.CreateInitializedParameter("@GSTR_DateOption",DbType.String,GSTR_DateOption),
                sqlDataProvider.CreateInitializedParameter("@GstCat",DbType.String,GstCat)
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetGstApiReport", sqlParameters);
            return dataSet;
        }
        public DataSet GetSalesInvoiceData(string compId, string brId, string invNo, string invDt, string invType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                         objProvider.CreateInitializedParameter("@InvNo",DbType.String, invNo),
                                                        objProvider.CreateInitializedParameter("@InvDt",DbType.String, invDt),
                                                        objProvider.CreateInitializedParameter("@InvType",DbType.String, invType),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_TeleIntg_GetSaleInvData", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetServiceSalesInvoiceData(string compId, string brId, string invNo, string invDt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                         objProvider.CreateInitializedParameter("@InvNo",DbType.String, invNo),
                                                        objProvider.CreateInitializedParameter("@InvDt",DbType.String, invDt),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_TeleIntg_GetServiceSaleInvData", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetSalesItemDetails(string compId, string brId, string dataType, string invNo, string invDt)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@DataType",DbType.String,dataType),
                sqlDataProvider.CreateInitializedParameter("@InvNo",DbType.String,invNo),
                sqlDataProvider.CreateInitializedParameter("@InvDt",DbType.String,invDt)
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetGstSalesItemsDetails", sqlParameters);
            return dataSet.Tables[0];
        }
        public int SaveApiRequestLogs(string reqBody, string apiResponse, DateTime reqTimeStamp, DateTime respTimeStamp, string status, string compId, string brId,
            string docno, string docDate, string errorMsg)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] procParams = {
                 objprovider.CreateInitializedParameter("@ReqBody",DbType.String, reqBody),
                 objprovider.CreateInitializedParameter("@ApiResponse",DbType.String, apiResponse),
                 objprovider.CreateInitializedParameter("@ReqTimeStamp",DbType.String, reqTimeStamp),
                 objprovider.CreateInitializedParameter("@RespTimeStamp",DbType.String, respTimeStamp),
                 objprovider.CreateInitializedParameter("@Status",DbType.String, status),
                 objprovider.CreateInitializedParameter("@CompId",DbType.String, compId),
                 objprovider.CreateInitializedParameter("@BrId",DbType.String, brId),
                 objprovider.CreateInitializedParameter("@DocNo",DbType.String, docno),
                 objprovider.CreateInitializedParameter("@DocDate",DbType.String, docDate),
                 objprovider.CreateInitializedParameter("@ErrorMsg",DbType.String, errorMsg),
                };
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_GenerateApiLogs", procParams);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public int UpdateGstInvApiDetails(string action, string compId, string brId, string gstInvno, string invDt,
            string ackNo, string ackDt, string irnNo, string status, string invType, string signedQrCode)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] procParams = {
                 objprovider.CreateInitializedParameter("@Action",DbType.String, action),
                 objprovider.CreateInitializedParameter("@CompId",DbType.String, compId),
                 objprovider.CreateInitializedParameter("@BrId",DbType.String, brId),
                 objprovider.CreateInitializedParameter("@GstInvNo",DbType.String, gstInvno),
                 objprovider.CreateInitializedParameter("@InvDt",DbType.String, invDt),
                 objprovider.CreateInitializedParameter("@AckNo",DbType.String, ackNo),
                 objprovider.CreateInitializedParameter("@AckDate",DbType.String, ackDt),
                 objprovider.CreateInitializedParameter("@IrnNo",DbType.String, irnNo),
                 objprovider.CreateInitializedParameter("@Status",DbType.String, status),
                 objprovider.CreateInitializedParameter("@InvType",DbType.String, invType),
                 objprovider.CreateInitializedParameter("@SignedQrCode",DbType.String, signedQrCode),
                };
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_sls$inv$detail_UpdateGstInvApiDetails", procParams);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public int UpdateGstInvAttachmentDetails(string invTbl, string compId, string brId, string gstInvno, string invDt, string docName, string docPath, string docType)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] procParams = {
                 objprovider.CreateInitializedParameter("@InvTbl",DbType.String, invTbl),
                 objprovider.CreateInitializedParameter("@CompId",DbType.String, compId),
                 objprovider.CreateInitializedParameter("@BrId",DbType.String, brId),
                 objprovider.CreateInitializedParameter("@AppOrGstInvNo",DbType.String, gstInvno),
                 objprovider.CreateInitializedParameter("@InvDt",DbType.String, Convert.ToDateTime(invDt).ToString("yyyy-MM-dd")),
                 objprovider.CreateInitializedParameter("@DocName",DbType.String, docName),
                 objprovider.CreateInitializedParameter("@DocPath",DbType.String, docPath),
                 objprovider.CreateInitializedParameter("@DocType",DbType.String, docType),

                };
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_Insert_Sls$Inv$Gst$Doc", procParams);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string CheckGstInvAttachmentDetails(string invTbl, string compId, string brId, string gstInvno, string invDt, string docName, string docPath, string docType)
        {
            try
            {
                string result = "";
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] sqlParameters =
                {objprovider.CreateInitializedParameter("@InvTbl",DbType.String, invTbl),
                 objprovider.CreateInitializedParameter("@CompId",DbType.String, compId),
                 objprovider.CreateInitializedParameter("@BrId",DbType.String, brId),
                 objprovider.CreateInitializedParameter("@AppOrGstInvNo",DbType.String, gstInvno),
                 objprovider.CreateInitializedParameter("@InvDt",DbType.String, Convert.ToDateTime(invDt).ToString("yyyy-MM-dd")),
                 objprovider.CreateInitializedParameter("@DocName",DbType.String, docName),
                 objprovider.CreateInitializedParameter("@DocPath",DbType.String, docPath),
                 objprovider.CreateInitializedParameter("@DocType",DbType.String, docType),
                 objprovider.CreateInitializedParameter("@Flag",DbType.String, "Check"),
            };
                DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Insert_Sls$Inv$Gst$Doc", sqlParameters);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    result = dataSet.Tables[0].Rows[0]["result"].ToString();
                }
                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetApiErrorDetails(string compId, string brId, string invNo)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
               // sqlDataProvider.CreateInitializedParameter("@InvTbl",DbType.String,InvTbl),
                sqlDataProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@DocNo",DbType.String,invNo),
               // sqlDataProvider.CreateInitializedParameter("@DocDate",DbType.String,invDt)
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Sp_GetApiErrorLog", sqlParameters);
            return dataSet.Tables[0];
        }
        public DataTable GetApiDocsDetails(string InvTbl, string compId, string brId, string invNo, string invDt, string docName)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@InvTbl",DbType.String,InvTbl),
                sqlDataProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@DocNo",DbType.String,invNo),
                sqlDataProvider.CreateInitializedParameter("@DocDate",DbType.String,invDt),
                sqlDataProvider.CreateInitializedParameter("@DocName",DbType.String,docName)
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetGstApiDocs", sqlParameters);
            return dataSet.Tables[0];
        }
        public DataTable GetApiClientDetails(string compId, string brId)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetGstApiClientDetail", sqlParameters);
            return dataSet.Tables[0];
        }
        public DataTable GetSaleRegisterDetails(string action, string compId, string brId, string fromDate, string toDate, string GSTR_DateOption)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@Action",DbType.String,action),
                sqlDataProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                sqlDataProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                sqlDataProvider.CreateInitializedParameter("@GSTR_DateOption",DbType.String,GSTR_DateOption)
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSlsPrcDataToSendOnEMGPortal", sqlParameters);
            return dataSet.Tables[0];
        }
        public DataTable GetFinMonthYearForGSTR(string compId, string brId)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_MonthYearForSaleRegister", sqlParameters);
            return dataSet.Tables[0];
        }
        public DataTable GetExcelFileNameForEMPReconsile(string gstr1Or2,string compId, string brId, string fromDate)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                sqlDataProvider.CreateInitializedParameter("@GSTR1Or2",DbType.String,gstr1Or2),
                sqlDataProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                sqlDataProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                sqlDataProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate)
            };
            DataSet dataSet = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetExcelFileNameForEMGRecon", sqlParameters);
            return dataSet.Tables[0];
        }
    }
}