using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.OpeningBalance;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.OpeningBalance;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.OpeningBalance
{
   public class OpeningBalance_SERVICES :OpeningBalance_ISERVICES
    {
        public DataTable Getcoa(string CompID,string BrchID, int acc_type,string Transtype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                     objProvider.CreateInitializedParameter("@acc_type",DbType.Int32, acc_type),
                       objProvider.CreateInitializedParameter("@Transtype",DbType.String, Transtype),
                                                    };
                DataTable GetCustcoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Coa$opbal", prmContentGetDetails).Tables[0];
                return GetCustcoa;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetAccGroup(string acc_id, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@acc_id",DbType.String, acc_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                       
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAccGrpDetail", prmContentGetDetails);
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

        public DataSet GetOpeningDate(int CompID, int BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                                                     };
                DataSet GetFY = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Get$FY$Openingdetail", prmContentGetDetails);
                return GetFY;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetCurrList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),                    
                };
                DataTable GetCurr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetCurrOpBal", prmContentGetDetails).Tables[0];
                return GetCurr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCurrConvRate(string curr_id, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@curr_id",DbType.String, curr_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetCurrConvRate", prmContentGetDetails);
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
        public String InsertOpeningBalanceDetail(DataTable OpeningBalanceHeader, DataTable OpeningBalanceBillWiseDetail)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, OpeningBalanceHeader ),
                 objprovider.CreateInitializedParameterTableType("@BillWiseDetail",SqlDbType.Structured, OpeningBalanceBillWiseDetail ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string OpBalDt = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertOpeningBalanceDetail", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[2].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[2].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }


        }
        public DataTable GetOpeningBalDetailList(string CompID, string BrchID, string FinYear,string searchValue)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                      objProvider.CreateInitializedParameter("@BrchID",DbType.String,BrchID),
                      objProvider.CreateInitializedParameter("@FinYear",DbType.String,FinYear),
                      objProvider.CreateInitializedParameter("@searchValue",DbType.String,searchValue),
                                                     };
                DataTable GetOpbalList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetOpeningBalanceDetailList]", prmContentGetDetails).Tables[0];
                return GetOpbalList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }

        }
        public DataSet GetBillwiseOpeningDetail(string CompID, string BrchID, string AccId, string FinYear)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@AccId",DbType.String, AccId),
                      objProvider.CreateInitializedParameter("@FinYear",DbType.String, FinYear),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetOpeningBillWiseDetail]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetBillwiseOpeningDetail1(string CompID, string BrchID, string AccId, string FinYear)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@AccId",DbType.String, AccId),
                      objProvider.CreateInitializedParameter("@FinYear",DbType.String, FinYear),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetOpeningBillWiseDetail1]", prmContentGetDetails);
            return ds;
        }
        public DataSet OpeningBalanceDelete(string Acc_id, string Fin_year, string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@acc_id",DbType.String, Acc_id),
                     objProvider.CreateInitializedParameter("@fin_year",DbType.String,  Fin_year),
                };
                DataSet OPdelete = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_DeleteOpbalance", prmContentGetDetails);
                return OPdelete;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetOpeningBalFinYearList(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                      objProvider.CreateInitializedParameter("@BrchID",DbType.String,BrchID),
                                                     };
                DataTable GetOpbalFinList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetOpeningBalanceFinYearList]", prmContentGetDetails).Tables[0];
                return GetOpbalFinList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }

        }
        public DataSet GetOpeningBalanceDeatils(string CompID, string BrchID, string FinYear)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    //objProvider.CreateInitializedParameter("@acc_id",DbType.String, FinYear),
                     objProvider.CreateInitializedParameter("@FinYear",DbType.String,  FinYear),
                };
                DataSet OPdelete = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetOpeningBalanceDetailListPrint", prmContentGetDetails);
                return OPdelete;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVerifiedDataOfExcel(DataTable OPBalDetail, DataTable BillDetail, string compId,string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@OPBalDetail",SqlDbType.Structured, OPBalDetail ),
                    objProvider.CreateInitializedParameterTableType("@BillDetail",SqlDbType.Structured,BillDetail),
                    objProvider.CreateInitializedParameter("@CompId", DbType.String,compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                };
                DataSet GetOpeningReceiptList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateOPBalExceFile", prmContentGetDetails);
                return GetOpeningReceiptList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable ShowExcelErrorDetail(DataTable OPBalDetail, DataTable BillDetail, string compId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@OPBalDetail",SqlDbType.Structured, OPBalDetail ),
                    objProvider.CreateInitializedParameterTableType("@BillDetail",SqlDbType.Structured,BillDetail),
                    objProvider.CreateInitializedParameter("@CompId", DbType.String,compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ShowExcelOPBalErrorDetail", prmContentGetDetails);
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
        public string BulkImportOPBalDetail(DataTable OPBalDetail, DataTable BillDetail, string BrID, string UserID, string compId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                    objProvider.CreateInitializedParameterTableType("@OPBalDetail",SqlDbType.Structured, OPBalDetail ),
                    objProvider.CreateInitializedParameterTableType("@BillDetail",SqlDbType.Structured,BillDetail),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                   objProvider.CreateInitializedParameter("@UserId",DbType.String,UserID),
                   objProvider.CreateInitializedParameter("@CompId", DbType.String,compId),
                 objProvider.CreateInitializedParameterTableType("@OutPut",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BulkImportOpeningBalance", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[5].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet GetMasterDataOPBal(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompId",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.Int32, BrchID),
                                                    };
                DataSet GetCustcoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetMasterDataOPBal", prmContentGetDetails);
                return GetCustcoa;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable getSalesPersonList(string CompID,string br_id, string userid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@SPersonName",DbType.String,"0"),
                      objProvider.CreateInitializedParameter("@BrchID",DbType.String,br_id),
                      objProvider.CreateInitializedParameter("@user_id",DbType.String,"1001"),
                                                     };
                DataTable GetOpbalFinList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$sls$person$comp_GetSalesPersonList", prmContentGetDetails).Tables[0];
                return GetOpbalFinList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }

        }
    }
}
