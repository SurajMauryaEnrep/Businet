using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.CostCenterSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.CostCenterSetup
{
    public class CostCenterSetup_SERVICES : CostCenterSetup_ISERVICES
    {
        public DataSet GetCCSetupTable(string comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_CCSetupTable", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SaveCCSetupData(string TransType, string comp_id, string user_id, string cc_id, string cc_name)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@user_id",DbType.String,user_id),
                    objprovider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                    objprovider.CreateInitializedParameter("@cc_name",DbType.String,cc_name),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[5].Size = 100;
                prmContent[5].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertAndUpdateCCSetup", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[5].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SaveCCSetupValueData(string TransType, string comp_id, string user_id, string cc_id, string cc_val_id, string cc_val_name)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@user_id",DbType.String,user_id),
                    objprovider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                    objprovider.CreateInitializedParameter("@cc_val_id",DbType.String,cc_val_id),
                    objprovider.CreateInitializedParameter("@cc_val_name",DbType.String,cc_val_name),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[6].Size = 100;
                prmContent[6].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertAndUpdateCCSetupVal_Name", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[6].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[6].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SaveCCSetup_branch_ueData(string TransType, string comp_id, string user_id, string cc_id, string Branch_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@user_id",DbType.String,user_id),
                    objprovider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                    objprovider.CreateInitializedParameter("@br_id",DbType.String,Branch_id),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[5].Size = 100;
                prmContent[5].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "insert_branch", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[5].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SaveCCSetup_Module(string TransType, string comp_id, string user_id, string cc_id, string module_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@user_id",DbType.String,user_id),
                    objprovider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                    objprovider.CreateInitializedParameter("@module_id",DbType.String,module_id),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[5].Size = 100;
                prmContent[5].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "insert_Module", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[5].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCostCenterList(string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[CostCenterList]", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllDropDown(string flag, string CompID, string create_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                      objProvider.CreateInitializedParameter("@flag",DbType.String,flag),
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                     objProvider.CreateInitializedParameter("@groupname",DbType.String,""),
                      objProvider.CreateInitializedParameter("@create_id",DbType.String,create_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Gt$All$DropDrown$Cost$Center]", prmContentGetDetails);
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
        public string DeleteCCSetup(string TransType, string cc_id,string comp_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")
                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "deleteCCSetup", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[3].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeleteCC_Setup_val(string TransType, string cc_val_id, string comp_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    //objprovider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                    objprovider.CreateInitializedParameter("@cc_val_id",DbType.String,cc_val_id),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")
                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "deleteCCSetup_val", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[3].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string deleteCC_branchSetup_val(string TransType, string cc_id,string br_id, string comp_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")
                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "deleteCC_BranchList", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[4].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string deleteCC_module(string TransType, string cc_id, string module_id, string comp_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@cc_id",DbType.String,cc_id),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@module_id",DbType.String,module_id),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")
                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "deleteCC_Module", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[4].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetModulDetails(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetModuleTaxTemplate", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getAppDocDetails(string flag, string CompID, string create_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@flag",DbType.String,flag),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@groupname",DbType.String,""),
                      objProvider.CreateInitializedParameter("@create_id",DbType.String,create_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$app$doc$getDetails", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetBranchOnchangeCC(string ddlcc_id, string CompID, string create_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@ddlcc_id",DbType.String,ddlcc_id),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objProvider.CreateInitializedParameter("@create_id",DbType.String,create_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetDDL_cc_branchOnChangeCCId", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetModuleOnchangeCC(string DDLModulecc_id, string CompID, string create_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@DDLModulecc_id",DbType.String,DDLModulecc_id),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objProvider.CreateInitializedParameter("@create_id",DbType.String,create_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetDDLcc_ModuleOnChangeCCId", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
