using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
    public class QCParameterDefinition_SERVICES : QCParameterDefinition_ISERVICES
    {

        public DataSet QCItemParameterSave(int Comp_ID, int userid, string param_name, string param_type, string TransType, string SystemDetail, int paramId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32,Comp_ID),
                    objProvider.CreateInitializedParameter("@ParaName",DbType.String, param_name),
                    objProvider.CreateInitializedParameter("@ParaType",DbType.String,param_type),
                    objProvider.CreateInitializedParameter("@CreateID",DbType.Int32, userid),
                    objProvider.CreateInitializedParameter("@MacID",DbType.String, SystemDetail),
                    objProvider.CreateInitializedParameter("@TransType",DbType.String, TransType),
                    objProvider.CreateInitializedParameter("@ParamID",DbType.String, paramId),

             };
                DataSet QCParameterDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$param_InsertParameterDetail", prmContentGetDetails);
                return QCParameterDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            DataSet Ds = new DataSet();
            return Ds;
        }
        public DataTable GetItemParameterList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getparamlist = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$param_ParameterDetailList", prmContentGetDetails).Tables[0];
                return Getparamlist;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetItemParaMList(string CompID, string Parmid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@paraid",DbType.Int32, Parmid),
                                                    };
                DataTable GetParam = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$param_ParamDetailList", prmContentGetDetails).Tables[0];
                return GetParam;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetUomIdList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetUom = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$UomList", prmContentGetDetails).Tables[0];
                return GetUom;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVerifiedDataOfExcel(string compId, DataTable PDDetail)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                     objProvider.CreateInitializedParameterTableType("@ParamData", SqlDbType.Structured, PDDetail),
                    objProvider.CreateInitializedParameter("@CompID", DbType.String, compId)
                };
                DataSet GetCustomerList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateParameterDefinitionExceFile", prmContentGetDetails);
                return GetCustomerList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable ShowExcelErrorDetail(string compId, DataTable PDDetail)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                     objProvider.CreateInitializedParameterTableType("@ParamData", SqlDbType.Structured, PDDetail),
                    objProvider.CreateInitializedParameter("@CompID", DbType.String, compId)
                };
                DataSet List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ShowExcelPDErrorDetail", prmContentGetDetails);
                return List.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string BulkImportPDDetail(string compId, string UserID,DataTable PDDetail)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                    objProvider.CreateInitializedParameterTableType("@ParamData",SqlDbType.Structured, PDDetail ),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String,UserID),
                    objProvider.CreateInitializedParameter("@CompID", DbType.String,compId),
                    objProvider.CreateInitializedParameterTableType("@OutPut",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BulkImportParameterDefinition", prmcontentaddupdate).ToString();

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
    }
}
