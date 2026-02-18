using EnRepMobileWeb.UTILITIES;
using System;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.OperationSetup;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.OperationSetup
{
    public class OperationSetup_SERVICES : OperationSetup_ISERVICES
    {
        public String insertOperationDetail(int comp_id, int op_id, string op_name, string op_type, 
            string op_remarks, int create_id,string action, string shfl_id, string wrkstn_id, string supervisor)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@comp_id",SqlDbType.Int, comp_id),
                 objprovider.CreateInitializedParameterTableType("@op_id",SqlDbType.Int, op_id ),
                 objprovider.CreateInitializedParameterTableType("@op_name",SqlDbType.NVarChar,op_name),
                 objprovider.CreateInitializedParameterTableType("@op_type",SqlDbType.NVarChar,op_type),
                 objprovider.CreateInitializedParameterTableType("@op_remarks",SqlDbType.NVarChar,op_remarks ),
                 objprovider.CreateInitializedParameterTableType("@create_id",SqlDbType.Int,create_id),
                 objprovider.CreateInitializedParameterTableType("@TransType",SqlDbType.Char,action),
                 objprovider.CreateInitializedParameterTableType("@supervisor",SqlDbType.NVarChar,supervisor),
                 objprovider.CreateInitializedParameterTableType("@wrkstn_id",SqlDbType.Int,wrkstn_id),
                 objprovider.CreateInitializedParameterTableType("@shfl_id",SqlDbType.Int,shfl_id),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[10].Size = 100;
                prmcontentaddupdate[10].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_ppl$op$setup_insert", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[10].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[10].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataTable GetOperationDetailsDAL(int CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$op$setup_GetOperationDetails", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindShopFloore(string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                   
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Shop$Flore$Dropdown", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetWorkStationDAL(string CompID, string br_id, int shfl_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                objProvider.CreateInitializedParameter("@shfl_id",DbType.Int32, shfl_id),};
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_pp$jc$BindWorkStation", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
