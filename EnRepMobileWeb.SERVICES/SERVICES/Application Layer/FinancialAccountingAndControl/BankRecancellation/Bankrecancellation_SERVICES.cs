using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.BankRecancellation;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.BankRecancellation
{
    public class Bankrecancellation_SERVICES : BankRecancellation_Iservices
    {
        public DataSet GetBankLists(string Comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] bnkReGetBankList = {
                    objProvider.CreateInitializedParameter("@Comp_Id",DbType.Int16, Comp_id),
                };
                DataSet BankList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$Bnkrecncl_GetbankList", bnkReGetBankList);
                return BankList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet GetBankCurr(string CompID, string Br_Id, string BankId,string FromDate,string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] bnkRecnclGetCurr = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, Br_Id),
                    objProvider.CreateInitializedParameter("@Banlk_Id",DbType.String, BankId),
                    objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                    objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                };
                DataSet GetCurrency = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$BnkRecnl_Curr", bnkRecnclGetCurr);
                return GetCurrency;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet GetFinYearDates(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] bnkReGetFinYearDates = {
                    objProvider.CreateInitializedParameter("@Comp_Id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, BrchID),
                };
                DataSet FinYear = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$GetFinYearDate", bnkReGetFinYearDates);
                return FinYear;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet GetFyToDate(string CompID, string BrchID,string ToDate,string FromDate,string Year)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] bnkReCnclFyToDate = {
                    objProvider.CreateInitializedParameter("@Comp_Id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                    
                    objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                    objProvider.CreateInitializedParameter("@Year",DbType.String, Year),
                };
                DataSet FinYear = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$Get_FyToDate", bnkReCnclFyToDate);
                return FinYear;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet GetSearchedData(string CompID, string BrchID, string acc_id, string FromDate, string ToDate,string TransType,string Status,string searchValue)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] bnkReCnclgetData = {
                    objProvider.CreateInitializedParameter("@Comp_Id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Acc_id",DbType.String, acc_id),
                    objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                    objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                    objProvider.CreateInitializedParameter("@TranType",DbType.String, TransType),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@searchValue",DbType.String, searchValue),
                    
                };
                DataSet Result = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$BnkRecncl_Searchdetails", bnkReCnclgetData);
                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string InsertUpdateBankreco(DataTable BanrecoList)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] BankRecontentaddupdate = {
                    objProvider.CreateInitializedParameterTableType("@BankRecoList",SqlDbType.Structured, BanrecoList),
                };
                string Result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[Usp$SaveInsert_BankReco]", BankRecontentaddupdate).ToString();
                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet CheckAdvancePayment(string CompId, string BrchID, string DocNo, string DocDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, DocDate),
                                                      };
                DataSet Get_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckAdvancePaymentForBP_BR", prmContentGetDetails);
                return Get_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckBeforeInsertUpdateBankreco(DataTable BanrecoList)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] BankRecontentaddupdate = {
                    objProvider.CreateInitializedParameterTableType("@BankRecoList",SqlDbType.Structured, BanrecoList),
                };
                //string Result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[Usp$SaveInsert_BankReco]", BankRecontentaddupdate).ToString();
                ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "CheckBefore_Insert_BankReco", BankRecontentaddupdate);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
