using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.CashBook;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.CashBook;
using EnRepMobileWeb.UTILITIES;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.CashBook
{
    public class CashBook_SERVICE: CashBook_ISERVICE
    {
        public Dictionary<string, string> GetCashBookAcc_List(string Comp_Id, string Br_Id, string AccName)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, Comp_Id),
                             objProvider.CreateInitializedParameter("@AccName",DbType.String, AccName),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, Br_Id),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$gl$detail_GetCashListDetail", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["cash_acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["cash_acc_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCashBookDetails(string comp_id, string br_id, string acc_id, string curr_id, string Fromdate, string Todate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@acc_id",DbType.String,acc_id),
                     objProvider.CreateInitializedParameter("@curr_id",DbType.String,curr_id),
                     objProvider.CreateInitializedParameter("@fromdt",DbType.String,Fromdate),
                     objProvider.CreateInitializedParameter("@todt",DbType.String,Todate),
                };
                DataSet Get_CashBookDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetBankAndCash_Book_Details", prmContentGetDetails);
                return Get_CashBookDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCashBookDetails(Search_Parmeters model, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir)
        {
            /* Added by Suraj Maurya 19-06-2025 */
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,model.CompId),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,model.BrId),
                     objProvider.CreateInitializedParameter("@acc_id",DbType.String,model.AccID),
                     objProvider.CreateInitializedParameter("@curr_id",DbType.String,model.CurrId),
                     objProvider.CreateInitializedParameter("@fromdt",DbType.String,model.From_dt),
                     objProvider.CreateInitializedParameter("@todt",DbType.String,model.To_dt),
                     objProvider.CreateInitializedParameter("@Skip",DbType.String,skip),
                     objProvider.CreateInitializedParameter("@PageSize",DbType.String,pageSize),
                     objProvider.CreateInitializedParameter("@Search",DbType.String,searchValue),
                     objProvider.CreateInitializedParameter("@sortColumn",DbType.String,sortColumn),
                     objProvider.CreateInitializedParameter("@sortColumnDir",DbType.String,sortColumnDir),
                     objProvider.CreateInitializedParameter("@forCsv",DbType.String,model.Flag)
                };
                DataSet Get_CashBookDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetBankAndCash_Book_Details", prmContentGetDetails);
                return Get_CashBookDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCurrList(string CompID, string Supptype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@Supptype",DbType.String,Supptype),
                };
                DataTable GetCurr_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp_CurronType", prmContentGetDetails).Tables[0];
                return GetCurr_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Get_FYList(string Compid, string Brid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Compid),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String,Brid),
                };
                DataSet Getfy_list = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$fy_GetList", prmContentGetDetails);
                return Getfy_list;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
