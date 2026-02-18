using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.MiscellaneousSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.MiscellaneousSetup
{
    public class MISDetail_SERVICES : MISDetail_ISERVICES
    {

        public string SaveMISData(string TransType, string setup_type_id, string setup_type_name, string setup_id, string setup_val, string mac_id, string comp_id, string setup_flag, string br_id, string user_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                   // objprovider.CreateInitializedParameter("@flag",DbType.String,"Insertstate"),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@setup_type_id",DbType.String,setup_type_id),
                    objprovider.CreateInitializedParameter("@setup_type_name",DbType.String,setup_type_name),
                    objprovider.CreateInitializedParameter("@setup_id",DbType.String,setup_id),
                    objprovider.CreateInitializedParameter("@setup_val",DbType.String,setup_val),
                    objprovider.CreateInitializedParameter("@mac_id",DbType.String,mac_id),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@setup_flag",DbType.String,setup_flag),

                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                            objprovider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                    objprovider.CreateInitializedParameter("@user_id",DbType.String,user_id),

                };
                prmContent[8].Size = 100;
                prmContent[8].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertAndUpdateMISData", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[8].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[8].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable checkSalesExecutiveBranchStatus(string compId, string branchId, string seId)
        {
            //SP_CheckSalesExecutiveBranchStatus
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,branchId),
                     objProvider.CreateInitializedParameter("@SeId",DbType.String,seId),
                                                     };
                DataTable GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_CheckSalesExecutiveBranchStatus", prmContentGetDetails).Tables[0];
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable checkBranchStatus(string compId, string branchId, string Doc_id, string flag)
        {
          
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,branchId),
                     objProvider.CreateInitializedParameter("@Doc_id",DbType.String,Doc_id),
                     objProvider.CreateInitializedParameter("@flag",DbType.String,flag),
                                                     };
                DataTable GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_CheckBranchStatus", prmContentGetDetails).Tables[0];
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SaveSalesExecutiveData(string action, string compId, string slsPersId, string slsPersName, string slsContno, string slsEmail, DataTable cstBranchList,string SalesRegion)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@Action",DbType.String,action),
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,compId),
                    objprovider.CreateInitializedParameter("@SlsPersId",DbType.String,slsPersId),
                    objprovider.CreateInitializedParameter("@SlsPersName",DbType.String,slsPersName),
                    objprovider.CreateInitializedParameter("@SlsContNo",DbType.String,slsContno),
                    objprovider.CreateInitializedParameter("@SlsEmail",DbType.String,slsEmail),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),                    
                    objprovider.CreateInitializedParameterTableType("@SlsPersBranches",SqlDbType.Structured,cstBranchList),
                    objprovider.CreateInitializedParameter("@SalesRegion",DbType.String,SalesRegion),
                };
                prmContent[6].Size = 100;
                prmContent[6].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SaveUpdate_stp$sls$person", prmContent).ToString();
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
        public string SaveEmployeeData(string action, string compId, string Emp_Id, string Emp_PersName, string Emp_Contno, string Emp_Email, DataTable cstBranchList)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@Action",DbType.String,action),
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,compId),
                    objprovider.CreateInitializedParameter("@emp_id",DbType.String,Emp_Id),
                    objprovider.CreateInitializedParameter("@emp_Name",DbType.String,Emp_PersName),
                    objprovider.CreateInitializedParameter("@EmpContNo",DbType.String,Emp_Contno),
                    objprovider.CreateInitializedParameter("@EmpEmail",DbType.String,Emp_Email),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objprovider.CreateInitializedParameterTableType("@SlsPersBranches",SqlDbType.Structured,cstBranchList),
                };
                prmContent[6].Size = 100;
                prmContent[6].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[stp_saveupdate_stp$emp$setup]", prmContent).ToString();
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
        public string SaveWastageReasonDetails(string action, string compId, string wastage_id, string wastage_reason, DataTable cstBranchList)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@Action",DbType.String,action),
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,compId),
                    objprovider.CreateInitializedParameter("@wastage_id",DbType.String,wastage_id),
                    objprovider.CreateInitializedParameter("@wastage_reason",DbType.String,wastage_reason),
             
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objprovider.CreateInitializedParameterTableType("@SlsPersBranches",SqlDbType.Structured,cstBranchList),
                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[STP_SaveUpdate_stp$wastage$reason]", prmContent).ToString();
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
        public string SaveRejectionReasonDetails(string action, string compId, string Rejection_id, string Rejection_reason, DataTable cstBranchList)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@Action",DbType.String,action),
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,compId),
                    objprovider.CreateInitializedParameter("@rej_id",DbType.String,Rejection_id),
                    objprovider.CreateInitializedParameter("@rej_reason",DbType.String,Rejection_reason),
             
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objprovider.CreateInitializedParameterTableType("@SlsPersBranches",SqlDbType.Structured,cstBranchList),
                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[STP_SaveUpdate_stp$rej$reason]", prmContent).ToString();
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
        public string SaveGLReport(string action, string compId, string GL_ID, string GL_Reporting, DataTable cstBranchList)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@Action",DbType.String,action),
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,compId),
                    objprovider.CreateInitializedParameter("@gl_id",DbType.String,GL_ID),
                    objprovider.CreateInitializedParameter("@gl_report_des",DbType.String,GL_Reporting),
             
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objprovider.CreateInitializedParameterTableType("@SlsPersBranches",SqlDbType.Structured,cstBranchList),
                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[STP_SaveUpdate_stp$gl$rpt$group]", prmContent).ToString();
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
        public string DeleteMISData(string setup_type_id, string setup_id, string setup_val, string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@setup_type_id",DbType.String,setup_type_id),
                    objprovider.CreateInitializedParameter("@setup_id",DbType.String,setup_id),
                    objprovider.CreateInitializedParameter("@setup_val",DbType.String,setup_val),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objprovider.CreateInitializedParameter("@br_id",DbType.String,br_id),

                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteMISData", prmContent).ToString();
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
        public string DeleteSeDetail(string compId, string brId, string seId)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,compId),
                    objprovider.CreateInitializedParameter("@BrId",DbType.String,brId),
                    objprovider.CreateInitializedParameter("@SeId",DbType.String,seId),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteSalesExecutive", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[3].Value != DBNull.Value)
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
        public string DeleteWastageDetail(string CompID, string br_id, string wastage_id)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@BrId",DbType.String,br_id),
                    objprovider.CreateInitializedParameter("@wast_id",DbType.String,wastage_id),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Delete$wastage$reason", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[3].Value != DBNull.Value)
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
        public string DeleteRejectionDetail(string CompID, string br_id, string rej_id)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@BrId",DbType.String,br_id),
                    objprovider.CreateInitializedParameter("@rej_id",DbType.String,rej_id),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Delete$Rejection$reason", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[3].Value != DBNull.Value)
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
        public string DeleteEmployeeSetup(string CompID, string br_id, string emp_id)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@BrId",DbType.String,br_id),
                    objprovider.CreateInitializedParameter("@emp_id",DbType.String,emp_id),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Delete$Employee$Setup", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[3].Value != DBNull.Value)
                {
                    DocNo = prmContent[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        } public string DeleteGLrpt_grp(string CompID, string br_id, string gl_id)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@CompId",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@BrId",DbType.String,br_id),
                    objprovider.CreateInitializedParameter("@gl_id",DbType.String,gl_id),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Delete$gl_rpt$grp", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[3].Value != DBNull.Value)
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
        public DataSet Get_MISAllTables(string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     //objProvider.CreateInitializedParameter("@flag",DbType.String,"getaddressstructuredetails"),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_MISAllTables", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable getstatelist(string countryid,string hdnstate_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@countryid",DbType.String,countryid),                  
                     objProvider.CreateInitializedParameter("@hdnstate_id",DbType.String,hdnstate_id),                  
                    
                                                     };
                DataTable GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get$sate$list", prmContentGetDetails).Tables[0];
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string Saveportdata(string Country, string portid, string PortDescription, string PinCode, string state,string Command,string hdnprot_id, string Porttype)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@Country",DbType.String,Country),
                    objprovider.CreateInitializedParameter("@portid",DbType.String,portid),
                    objprovider.CreateInitializedParameter("@PortDescription",DbType.String,PortDescription),
                    objprovider.CreateInitializedParameter("@PinCode",DbType.String,PinCode),                   
                    objprovider.CreateInitializedParameter("@state",DbType.String,state),
                    objprovider.CreateInitializedParameter("@Command",DbType.String,Command),
                    objprovider.CreateInitializedParameter("@hdnprot_id",DbType.String,hdnprot_id),
                    objprovider.CreateInitializedParameter("@Porttype",DbType.String,Porttype),
                     objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[8].Size = 100;
                prmContent[8].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "save$port$fct", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[8].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[8].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeletePortDetail(string Portid)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@Portid",DbType.String,Portid),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                };
                prmContent[1].Size = 100;
                prmContent[1].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "delete$port$detail", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[1].Value != DBNull.Value)
                {
                    DocNo = prmContent[1].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public DataTable checkdependcyport(string compId, string brId, string tblport_id)
        //{

        //    try
        //    {
        //        SqlDataProvider objprovider = new SqlDataProvider();
        //        SqlParameter[] prmContent =
        //        {
        //            objprovider.CreateInitializedParameter("@comp_id",DbType.String,compId),
        //            objprovider.CreateInitializedParameter("@br_id",DbType.String,brId),
        //            objprovider.CreateInitializedParameter("@tblport_id",DbType.String,tblport_id),

        //        };
        //        //prmContent[3].Size = 100;
        //        //prmContent[3].Direction = ParameterDirection.Output;
        //        DataTable dn_no = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "check$dependcy$port$deatil", prmContent).Tables[0];
        //        //string DocNo = string.Empty;
        //        //if (prmContent[3].Value != DBNull.Value)
        //        //{
        //        //    DocNo = prmContent[3].Value.ToString();
        //        //}
        //        return dn_no;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
