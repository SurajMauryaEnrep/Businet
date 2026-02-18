using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
    public class CustomerSetup_SERVICES : CustomerSetup_ISERVICES
    {
        

        public DataSet GetCustListDAL(string cust_id, string CompID)
        {
            try
            {
                
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                      objProvider.CreateInitializedParameter("@cust_id",DbType.String,cust_id),
                                                     };
                //DataTable GetCustList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$CustListPage$detail", prmContentGetDetails).Tables[0];
                //return GetCustList;
                DataSet GetCustList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$CustListPage$detail]", prmContentGetDetails);
                return GetCustList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetStatusList(string MenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@menu_id",DbType.String, MenuID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$DocStatus", prmContentGetDetails);
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
        public DataSet GetCustomerListFilterDAL(string CompID, string CustID, string Custtype, string Custcat, string Custport, string CustAct, string CustStatus, string Glrtp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@CustID",DbType.String,CustID),
                     objProvider.CreateInitializedParameter("@Custtype",DbType.String,Custtype),
                     objProvider.CreateInitializedParameter("@Custcat",DbType.String,Custcat),
                     objProvider.CreateInitializedParameter("@Custport",DbType.String,Custport),
                     objProvider.CreateInitializedParameter("@CustAct",DbType.String,CustAct),
                     objProvider.CreateInitializedParameter("@CustStatus",DbType.String,CustStatus),
                     objProvider.CreateInitializedParameter("@Glrtp_id",DbType.String,Glrtp_id)

                     };
                DataSet GetCustList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$CustomerList$_GetCustomerFilter", prmContentGetDetails);
                return GetCustList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataSet GetcategoryDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataSet Getcategory = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$category", prmContentGetDetails);
                return Getcategory;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable Getcategory(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getcategor = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$category", prmContentGetDetails).Tables[0];
                return Getcategor;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustportDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataSet GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$portfolio", prmContentGetDetails);
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustport(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$portfolio", prmContentGetDetails).Tables[0];
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> CustNameListDAL(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$CustList$detail", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["cust_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable Bind_custList1(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, GroupName),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$CustList$detail", prmContentGetDetails).Tables[0];
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
        public DataSet GetDataAllDropDown(string GroupName, string CompID,string cust_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@cust_id",DbType.String, cust_id),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$All$List$Dropdown", prmContentGetDetails);
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
        public DataTable GetGlReportingGrp(string Comp_ID, string Br_id, string gl_repoting)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                    objProvider.CreateInitializedParameter("@Br_id",DbType.String,Br_id),
                     objProvider.CreateInitializedParameter("@gl_repoting",DbType.String,gl_repoting),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Data$Drp$Dwn$GlReporting", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVerifiedDataOfExcel(string compId, DataTable CustomerDetail, DataTable CustomerBranch, DataTable CustomerAddress)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@CustomerData",SqlDbType.Structured, CustomerDetail ),
                    objProvider.CreateInitializedParameterTableType("@CustomerAddress",SqlDbType.Structured,CustomerAddress),
                    objProvider.CreateInitializedParameterTableType("@BranchData",SqlDbType.Structured, CustomerBranch ),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),

                };
                DataSet GetCustomerList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateCustomerExceFile", prmContentGetDetails);
                return GetCustomerList;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable ShowExcelErrorDetail(string compId, DataTable CustomerDetail, DataTable CustomerBranch, DataTable CustomerAddress)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameterTableType("@CustomerData",SqlDbType.Structured, CustomerDetail ),
                    objProvider.CreateInitializedParameterTableType("@CustomerAddress",SqlDbType.Structured,CustomerAddress),
                    objProvider.CreateInitializedParameterTableType("@BranchData",SqlDbType.Structured, CustomerBranch ),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                  };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ShowExcelCustomerErrorDetail", prmContentGetDetails);
                return GetItemList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
            public string BulkImportCustomerDetail(string compId,string UserID,string BranchName,DataTable CustomerDetail, DataTable CustomerBranch, DataTable CustomerAddress)
            {

                try
                {
                    SqlDataProvider objProvider = new SqlDataProvider();
                    SqlParameter[] prmcontentaddupdate = {
                     objProvider.CreateInitializedParameterTableType("@CustomerData",SqlDbType.Structured, CustomerDetail ),
                    objProvider.CreateInitializedParameterTableType("@CustomerAddress",SqlDbType.Structured,CustomerAddress),
                    objProvider.CreateInitializedParameterTableType("@BranchData",SqlDbType.Structured, CustomerBranch ),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                    objProvider.CreateInitializedParameter("@userId",DbType.String,UserID),
                    objProvider.CreateInitializedParameter("@BranchName",DbType.String,BranchName),
                 objProvider.CreateInitializedParameterTableType("@OutPut",SqlDbType.NVarChar,""),
                };
                    prmcontentaddupdate[6].Size = 100;
                    prmcontentaddupdate[6].Direction = ParameterDirection.Output;

                    string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BulkImportCustomer", prmcontentaddupdate).ToString();

                    string DocNo = string.Empty;
                    if (prmcontentaddupdate[6].Value != DBNull.Value) // status
                    {
                        DocNo = prmcontentaddupdate[6].Value.ToString();
                    }
                    return DocNo;
                }
                catch (SqlException ex)
                {
                    throw ex;

                }
            }
        public DataSet GetMasterDropDownList(string Comp_id, string Br_ID)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Comp_id",DbType.String,Comp_id),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Br_ID),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Master$Cust$data", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustLedgerDtl(string CompID, string Br_ID, string Cust_id, string CustAcc_id, string Curr_id)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String,Br_ID),
                     objProvider.CreateInitializedParameter("@Cust_id",DbType.String,Cust_id),
                      objProvider.CreateInitializedParameter("@CustAcc_id",DbType.String,CustAcc_id),
                      objProvider.CreateInitializedParameter("@Curr_id",DbType.String,Curr_id),
                                                     };
                //DataTable GetCustList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$CustListPage$detail", prmContentGetDetails).Tables[0];
                //return GetCustList;
                DataSet GetCustList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$CustListLedgerdetail]", prmContentGetDetails);
                return GetCustList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        
    }
}
