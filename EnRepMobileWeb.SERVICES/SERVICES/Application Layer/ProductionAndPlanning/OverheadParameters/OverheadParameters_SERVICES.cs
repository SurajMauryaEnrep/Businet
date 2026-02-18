using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.OverheadParameters;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.OverheadParameters;
using EnRepMobileWeb.UTILITIES;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.OverheadParameters
{
    public class OverheadParameters_SERVICES : OverheadParameters_ISERVICES
    {
        public DataTable GetOverheadExpParamDetailsDAL(int Comp_ID)
        {
            try
                {
                    SqlDataProvider objProvider = new SqlDataProvider();
                    SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, Comp_ID),
                                                    };
                    DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$ohd$param$setup_GetOverheadExpParamDetails", prmContentGetDetails).Tables[0];
                    return Getsuppport;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            
        }

        public string InsertUpdateOverheadParam(int comp_id, int ohd_exp_id, string ohd_exp_name, int uom_id, string ohd_exp_remarks, int create_id, string transtype)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@comp_id",SqlDbType.Int, comp_id),
                 objprovider.CreateInitializedParameterTableType("@ohd_exp_id",SqlDbType.Int, ohd_exp_id ),
                 objprovider.CreateInitializedParameterTableType("@ohd_exp_name",SqlDbType.NVarChar,ohd_exp_name),
                 objprovider.CreateInitializedParameterTableType("@uom_id",SqlDbType.Int,uom_id),
                 objprovider.CreateInitializedParameterTableType("@ohd_exp_remarks",SqlDbType.NVarChar,ohd_exp_remarks ),
                 objprovider.CreateInitializedParameterTableType("@create_id",SqlDbType.Int,create_id),
                 objprovider.CreateInitializedParameterTableType("@TransType",SqlDbType.Char,transtype),
                 objprovider.CreateInitializedParameterTableType("@Status",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[7].Size = 100;
                prmcontentaddupdate[7].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_ppl$ohdExp$parameter_insert", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[7].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[7].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataTable GetUOMDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataTable GetUOM = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$uom$detail_Getuom", prmContentGetDetails).Tables[0];
                return GetUOM;
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
        public DataSet getOHDdetailsEdit(string CompId,  string ohd_exp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                   
                    objProvider.CreateInitializedParameter("@ohd_exp_id",DbType.String,ohd_exp_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$ohd$getDetailsEdit", prmContentGetDetails);
                return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string OHDDelete(OverheadParametersModel OhdParamModelDEL, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    
                    objProvider.CreateInitializedParameter("@ohd_exp_id",DbType.Int32, OhdParamModelDEL.ohd_exp_id),
                    //objProvider.CreateInitializedParameter("@ohd_exp_name",DbType.String,  OhdParamModelDEL.ohd_exp_name),
                    //objProvider.CreateInitializedParameter("@uom_id",DbType.Int32,  OhdParamModelDEL.uom_id),
                    objProvider.CreateInitializedParameter("@Status",DbType.String,""),
                                                     };
                prmContentGetDetails[2].Size = 100;
                prmContentGetDetails[2].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_ppl$OHDParamDelete]", prmContentGetDetails).ToString();
                //return ActionDeatils;
                string DocNo = string.Empty;
                if (prmContentGetDetails[2].Value != DBNull.Value) // status
                {
                    DocNo = prmContentGetDetails[2].Value.ToString();
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
