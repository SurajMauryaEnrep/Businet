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
    public class SupplierList_SERVICES : SupplierList_ISERVICES
    {
        public DataSet GetSuppListDAL(string supp_id, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@supp_id",DbType.String,supp_id),
                                                     };
                //DataTable GetSuppList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$SuppList$detail_GetSuppList", prmContentGetDetails).Tables[0];
                //return GetSuppList;
                DataSet GetSuppList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$SuppList$detail_GetSuppList]", prmContentGetDetails);
                return GetSuppList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSupplierListFilterDAL(string CompID, string SuppID, string Supptype, string Suppcat, string Suppport, 
            string SuppAct, string SuppStatus,string Glrtp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@SuppID",DbType.String,SuppID),
                     objProvider.CreateInitializedParameter("@Supptype",DbType.String,Supptype),
                      objProvider.CreateInitializedParameter("@Suppcat",DbType.String,Suppcat),
                      objProvider.CreateInitializedParameter("@Suppport",DbType.String,Suppport),
                       objProvider.CreateInitializedParameter("@SuppAct",DbType.String,SuppAct),
                      objProvider.CreateInitializedParameter("@SuppStatus",DbType.String,SuppStatus),
                      objProvider.CreateInitializedParameter("@Glrtp_id",DbType.String,Glrtp_id)

                     };
                DataSet GetSuppList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$SupplierList$detail_GetSupplierFilterList", prmContentGetDetails);
                return GetSuppList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetcategoryDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getcategory = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$category_Getcategory", prmContentGetDetails).Tables[0];
                return Getcategory;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetsuppportDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp$portfolio_Getportfolio", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable Getsuppplier(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@SuppName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                    };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$SuppList$detail_GetSuppNameList", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        } 
        public DataSet GetAllDropdown(string GroupName, string CompID,string supp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@SuppName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@supp_id",DbType.String, supp_id),
                                                    };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$supplier$list$All$Dropdowns", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
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
        //public Dictionary<string, string> SuppSetupGroupDAL(string GroupName, string CompID)
        //{
        //    Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
        //    string firstItem = string.Empty;

        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //            objProvider.CreateInitializedParameter("@SuppName",DbType.String, GroupName),
        //            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
        //                                             };

        //        DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$SuppList$detail_GetSuppNameList", prmContentGetDetails);
        //        DataRow dr;
        //        dr = PARQusData.Tables[0].NewRow();
        //        dr[0] = "0";
        //        dr[1] = "---All---";
        //        PARQusData.Tables[0].Rows.InsertAt(dr, 0);

        //        if (PARQusData.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
        //            {
        //                ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["supp_id"].ToString(), PARQusData.Tables[0].Rows[i]["supp_name"].ToString());
        //            }
        //        }
        //        return ddlItemNameDictionary;

        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet GetVerifiedDataOfExcel(string compId, DataTable SupplierDetail, DataTable SupplierBranch, DataTable SupplierAddress)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@SupplierData",SqlDbType.Structured, SupplierDetail ),
                    objProvider.CreateInitializedParameterTableType("@SupplierAddress",SqlDbType.Structured,SupplierAddress),
                    objProvider.CreateInitializedParameterTableType("@BranchData",SqlDbType.Structured, SupplierBranch ),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                };
                DataSet GetCustomerList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateSupplierExceFile", prmContentGetDetails);
                return GetCustomerList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable ShowExcelErrorDetail(string compId, DataTable SupplierDetail, DataTable SupplierBranch, DataTable SupplierAddress)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameterTableType("@SupplierData",SqlDbType.Structured, SupplierDetail ),
                    objProvider.CreateInitializedParameterTableType("@SupplierAddress",SqlDbType.Structured,SupplierAddress),
                    objProvider.CreateInitializedParameterTableType("@BranchData",SqlDbType.Structured, SupplierBranch ),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                  };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ShowExcelSupplierErrorDetail", prmContentGetDetails);
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
        public string BulkImportSupplierDetail(string compId, string UserID, string BranchName, DataTable SupplierDetail, DataTable SupplierBranch, DataTable SupplierAddress)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                     objProvider.CreateInitializedParameterTableType("@SupplierData",SqlDbType.Structured, SupplierDetail ),
                    objProvider.CreateInitializedParameterTableType("@SupplierAddress",SqlDbType.Structured,SupplierAddress),
                    objProvider.CreateInitializedParameterTableType("@BranchData",SqlDbType.Structured, SupplierBranch ),
                    objProvider.CreateInitializedParameter("@compId", DbType.String,compId),
                    objProvider.CreateInitializedParameter("@userId",DbType.String,UserID),
                    objProvider.CreateInitializedParameter("@BranchName",DbType.String,BranchName),
                 objProvider.CreateInitializedParameterTableType("@OutPut",SqlDbType.NVarChar,""),
                };
               prmcontentaddupdate[6].Size = 100;
               prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BulkImportSupplier", prmcontentaddupdate).ToString();
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
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_id),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Br_ID),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Master$Supp$data", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSuppLedgerDtl(string Comp_id, string Br_ID, string Supp_id, string SuppAcc_id, string Curr_id)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_id),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String,Br_ID),
                     objProvider.CreateInitializedParameter("@Supp_id",DbType.String,Supp_id),
                      objProvider.CreateInitializedParameter("@SuppAcc_id",DbType.String,SuppAcc_id),
                      objProvider.CreateInitializedParameter("@Curr_id",DbType.String,Curr_id),
                };
                DataSet GetsuppDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$SuppListLedgerdetail", prmContentGetDetails);
                return GetsuppDetail;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
