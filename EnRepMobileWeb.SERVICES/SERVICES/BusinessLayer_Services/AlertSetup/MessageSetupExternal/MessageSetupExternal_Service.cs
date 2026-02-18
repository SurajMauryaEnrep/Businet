using EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.MessageSetupExternal;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AlertSetup.MessageSetupExternal;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.AlertSetup.MessageSetupExternal
{
    public class MessageSetupExternal_Service : MessageSetupExternal_IService
    {
        public int AddUpdateAlertSetup(AlertMessageSetupExternal asModel)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] procParams = {

                 objprovider.CreateInitializedParameterTableType("@action",SqlDbType.NVarChar, asModel.action ),
                 objprovider.CreateInitializedParameterTableType("@alertType",SqlDbType.NVarChar, asModel.alertType ),
                 objprovider.CreateInitializedParameterTableType("@compId",SqlDbType.Int, asModel.compId ),
                 objprovider.CreateInitializedParameterTableType("@brId",SqlDbType.Int, asModel.brId ),
                 objprovider.CreateInitializedParameterTableType("@docId",SqlDbType.NVarChar, asModel.docId ),
                 objprovider.CreateInitializedParameterTableType("@event",SqlDbType.NVarChar, asModel.events.Trim()),
                 objprovider.CreateInitializedParameterTableType("@msg",SqlDbType.NVarChar, asModel.msg ),
                  objprovider.CreateInitializedParameterTableType("@MsgSubject",SqlDbType.NVarChar, asModel.msgSubject ),
                 objprovider.CreateInitializedParameterTableType("@MsgHeader",SqlDbType.NVarChar, asModel.msgHeader),
                 objprovider.CreateInitializedParameterTableType("@MsgBody",SqlDbType.NVarChar, asModel.msgBody),
                 objprovider.CreateInitializedParameterTableType("@MsgFooter",SqlDbType.NVarChar, asModel.msgFooter),
                 objprovider.CreateInitializedParameterTableType("@CrModId",SqlDbType.NVarChar, asModel.crtOrModId),
                };
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsUpdAlertMsgExt", procParams);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllData(string compId, string brId, string langFlag, string docId, string events)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, compId),
                    objProvider.CreateInitializedParameter("@brId",DbType.Int64, brId),
                    objProvider.CreateInitializedParameter("@LangFlag",DbType.String, langFlag),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, docId),
                    objProvider.CreateInitializedParameter("@Event",DbType.String, events),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$All$Data$ListAndDropdown$Message$Setup$Ext", prmContentGetDetails);
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
        public DataTable GetDocumentList(string compId, string brId, string langFlag, string alert_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, compId),
                    objProvider.CreateInitializedParameter("@LangFlag",DbType.String, langFlag),
                    objProvider.CreateInitializedParameter("@alert_type",DbType.String, alert_type),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetDocumentList", prmContentGetDetails).Tables[0];
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
        public DataTable GetAlertMsg(string compId, string brId, string docId, string events, string langFlag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int64,  compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int64,  brId),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, docId),
                    objProvider.CreateInitializedParameter("@Event",DbType.String, events),
                    objProvider.CreateInitializedParameter("@LangFlag",DbType.String, langFlag),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetAlertMsgExtDetail", prmContentGetDetails).Tables[0];
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
        public DataTable GetDocumentEvents(string docId, string ddlType, string alertType, string compId, string brId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@Action",DbType.String, ddlType),
                    objProvider.CreateInitializedParameter("@alertType",DbType.String, alertType),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, docId),
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Get$stp$alert$status$ext", prmContentGetDetails).Tables[0];
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
        public DataTable GetDocumentFieldName(string compId, string docId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int64,  compId),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, docId),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetDocFieldNameExt", prmContentGetDetails).Tables[0];
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
        public int DeleteAlertSetup(string compId, string brId, string docId, string events, string alertType)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] procParams = {

                 objprovider.CreateInitializedParameterTableType("@CompId",SqlDbType.Int, compId ),
                 objprovider.CreateInitializedParameterTableType("@BrId",SqlDbType.Int, brId ),
                 objprovider.CreateInitializedParameterTableType("@AlertType",SqlDbType.VarChar, alertType),
                 objprovider.CreateInitializedParameterTableType("@DocId",SqlDbType.VarChar, docId ),
                 objprovider.CreateInitializedParameterTableType("@Event",SqlDbType.VarChar, events ),
                };
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DeleteAlertMsgExt", procParams);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
