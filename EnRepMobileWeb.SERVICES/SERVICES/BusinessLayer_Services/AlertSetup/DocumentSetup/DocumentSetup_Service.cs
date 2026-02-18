using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AlertSetup.DocumentSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.AlertSetup.DocumentSetup
{
    public class DocumentSetup_Service : DocumentSetup_IService
    {
        public DataTable CheckIfDocumentTypeAlreadySet(string compid, string brId, string docId, string eventId, string alertType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@compid",DbType.String, compid),
                    objProvider.CreateInitializedParameter("@brId",DbType.String, brId),
                    objProvider.CreateInitializedParameter("@docId",DbType.String, docId),
                    objProvider.CreateInitializedParameter("@eventId",DbType.String, eventId),
                    objProvider.CreateInitializedParameter("@alertType",DbType.String, alertType),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_CheckIfDocUserAlreadySet", prmContentGetDetails).Tables[0];
                return searchmenu;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public int DeleteAlertDocSetup(string rowId)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] procParams = {

                 objprovider.CreateInitializedParameterTableType("@RowId",SqlDbType.Int, rowId),
                };
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_Delete$stp$alert$doc", procParams);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetAlertDocData(string compId, string branchId, string alertType, string docId, string events, string receipientType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@compId",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@BranchId",DbType.String, branchId),
                    objProvider.CreateInitializedParameter("@alertType",DbType.String, alertType),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, docId),
                    objProvider.CreateInitializedParameter("@Event",DbType.String, events),
                    objProvider.CreateInitializedParameter("@receptType",DbType.String, receipientType),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Get_stp$alert$doc_Data", prmContentGetDetails).Tables[0];
                return searchmenu;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        } 
        public DataSet GetAlldata(string compId, string branchId,string language)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, branchId),      
                    objProvider.CreateInitializedParameter("@lang",DbType.String, language),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$All$Alert$SP_GetAlert$DocSetupDocuments", prmContentGetDetails);
                return searchmenu;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataSet GetDocumentEvents(string compId, string brId, string alertType, string docId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int64, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int64, brId),
                    objProvider.CreateInitializedParameter("@AlertType",DbType.String, alertType),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, docId),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetAlertDocSetupEvents", prmContentGetDetails);
                return searchmenu;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataSet GetDocumentList(string compId, string brId, string alertType, string lang)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int64, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int64, brId),
                    objProvider.CreateInitializedParameter("@AlertType",DbType.String, alertType),
                    objProvider.CreateInitializedParameter("@lang",DbType.String, lang),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetAlertDocSetupDocuments", prmContentGetDetails);
                return searchmenu;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataTable GetUsersList(string compId, string branchId, string docId, string receipientType, string events, string alertType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@compId",DbType.Int64, compId),
                    objProvider.CreateInitializedParameter("@branchId",DbType.Int64, branchId),
                    objProvider.CreateInitializedParameter("@docId",DbType.String, docId),
                    objProvider.CreateInitializedParameter("@UserType",DbType.String, receipientType),
                    objProvider.CreateInitializedParameter("@event",DbType.String, events),
                    objProvider.CreateInitializedParameter("@alertType",DbType.String, alertType),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetUsersToSendAlert", prmContentGetDetails).Tables[0];
                return searchmenu;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public int SaveDocSetupDetails(string compId, string branchId, string alertType, string docId, string events, string rcptType, string rcptId)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] procParams = {

                 objprovider.CreateInitializedParameterTableType("@CompId",SqlDbType.NVarChar, compId ),
                 objprovider.CreateInitializedParameterTableType("@BranchId",SqlDbType.Int, branchId ),
                 objprovider.CreateInitializedParameterTableType("@AlertType",SqlDbType.NVarChar, alertType ),
                 objprovider.CreateInitializedParameterTableType("@DocId",SqlDbType.NVarChar, docId),
                 objprovider.CreateInitializedParameterTableType("@Event",SqlDbType.NVarChar, events ),
                 objprovider.CreateInitializedParameterTableType("@ReceptType",SqlDbType.NVarChar, rcptType),
                 objprovider.CreateInitializedParameterTableType("@ReceptId",SqlDbType.NVarChar, rcptId),
                };
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_Save_stp$alert$doc", procParams);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
