using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TransporterSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.TransporterSetup
{
    public class TransporterSetup_SERVICES : TransporterSetup_ISERVICES
    {
        public DataTable GetCountryBehfOf_HOD_Organisation(string CompID, string TransModetype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@TransModetype",DbType.String,TransModetype),
                };
                DataTable GetCurronSuppType = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Country_onTransModeType", prmContentGetDetails).Tables[0];
                return GetCurronSuppType;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetstateOnCountryDDL(string ddlCountryID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CountryID",DbType.String,ddlCountryID),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getState", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetDistrictOnStateDDL(string ddlStateID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@StateID",DbType.String,ddlStateID),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getDistrict", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCityOnDistrictDDL(string ddlDistrictID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@DistrictID",DbType.String,ddlDistrictID),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getCity", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertTransport_Details(TransporterSetupModel _TransporterSetupModel,DataTable ItemAttachments)
        {
            try
            {

                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameter("@Action",DbType.String, _TransporterSetupModel.TransType),
                 objprovider.CreateInitializedParameter("@TransId",DbType.String, _TransporterSetupModel.TransId),
                 objprovider.CreateInitializedParameter("@CompId",DbType.String, _TransporterSetupModel.CompId ),
                 objprovider.CreateInitializedParameter("@TransType",DbType.String, _TransporterSetupModel.TransportType ),
                 objprovider.CreateInitializedParameter("@TransName",DbType.String, _TransporterSetupModel.TransName ),
                 objprovider.CreateInitializedParameter("@TransMode",DbType.String, _TransporterSetupModel.TransMode ),
                 objprovider.CreateInitializedParameter("@TransAdd",DbType.String, _TransporterSetupModel.TransAdd ),
                 objprovider.CreateInitializedParameter("@TransCntry",DbType.String, _TransporterSetupModel.TransCntry ),
                 objprovider.CreateInitializedParameter("@TransState",DbType.String, _TransporterSetupModel.TransState ),
                 objprovider.CreateInitializedParameter("@TransCity",DbType.String, _TransporterSetupModel.TransCity ),
                 objprovider.CreateInitializedParameter("@TransDist",DbType.String, _TransporterSetupModel.TransDist ),
                 objprovider.CreateInitializedParameter("@TransPin",DbType.String, _TransporterSetupModel.TransPin ),
                 objprovider.CreateInitializedParameter("@TransGstNo",DbType.String, _TransporterSetupModel.transGstNo ),
                 objprovider.CreateInitializedParameter("@TransPanNo",DbType.String, _TransporterSetupModel.transPanNo ),
                 objprovider.CreateInitializedParameter("@CreateModId",DbType.String, _TransporterSetupModel.createModId ),
                 objprovider.CreateInitializedParameter("@TransStatus",DbType.String, _TransporterSetupModel.TransStatus ),
                 objprovider.CreateInitializedParameter("@MacId",DbType.String, _TransporterSetupModel.MacId ),
                 objprovider.CreateInitializedParameter("@Onhold",DbType.String, _TransporterSetupModel.TransOnHold ),
                 objprovider.CreateInitializedParameter("@Remarks",DbType.String, _TransporterSetupModel.Remarks ),
                 objprovider.CreateInitializedParameterTableType("@Attachment",SqlDbType.Structured,ItemAttachments ),
                 objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                 objprovider.CreateInitializedParameter("@GST_Cat",DbType.String, _TransporterSetupModel.Gst_Cat ),
                };
                prmcontentaddupdate[20].Size = 100;
                prmcontentaddupdate[20].Direction = ParameterDirection.Output;

                string DocNo = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertTransporter_Details", prmcontentaddupdate).ToString();

                if (prmcontentaddupdate[20].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[20].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataTable GetTransportDetails(string compId, string transId, string transType, string transMode)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@TransId",DbType.String,transId),
                     objProvider.CreateInitializedParameter("@Transtype",DbType.String,transType),
                     objProvider.CreateInitializedParameter("@TransMode",DbType.String,transMode),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Get$stp$trans$detail", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDeatilTrasportData(string compId, string transId, string transType, string transMode)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@TransId",DbType.String,transId),
                     objProvider.CreateInitializedParameter("@Transtype",DbType.String,transType),
                     objProvider.CreateInitializedParameter("@TransMode",DbType.String,transMode),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Get$stp$trans$detail", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetStateCode(string stateId)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@StateId",DbType.String,stateId),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetStateCode", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeleteTransDetails(string compId, string transId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int32, compId),
                    objProvider.CreateInitializedParameter("@TransId",DbType.String, transId),
                                                     };
                //string SuppDetails = string.Empty;
                string SuppDetails = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "SP_Delete_stp$trans$detail", prmContentGetDetails));
                return SuppDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
