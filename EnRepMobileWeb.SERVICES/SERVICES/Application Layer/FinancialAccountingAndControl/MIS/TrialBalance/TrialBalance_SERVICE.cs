using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.TrialBalance;
using EnRepMobileWeb.UTILITIES;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.TrialBalance
{
    public class TrialBalance_SERVICE : TrialBalance_ISERVICE
    {
        public Dictionary<string, string> GLSetupGroupDAL(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@AccName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$GLName", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["acc_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> AccGrpListGroupDAL(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Trial_Balance_stp$Acc$grp_GetAllAccGroup", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["acc_grp_id"].ToString(), PARQusData.Tables[0].Rows[i]["AccGroupChildNood"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllBrchList(string CompID, string User_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@User_id",DbType.String,User_id)
                 };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$detail_GetBrchList", prmContentGetDetails);
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
        public DataSet GetBalanceDetailList(string CompId, string BranchID, string UserID, string IncludeZeroStockFlag, string BalanceBy, string AccId, string AccGrpId, string AccType, string RptType, string Branch, string UptoDate, string curr_filter)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@Br_Id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@IncludeZeroBalFlag",DbType.String, IncludeZeroStockFlag),
                                                        objProvider.CreateInitializedParameter("@BalanceBy",DbType.String, BalanceBy),
                                                        objProvider.CreateInitializedParameter("@Acc_Id",DbType.String, AccId),
                                                        objProvider.CreateInitializedParameter("@AccGrpId",DbType.String,AccGrpId),
                                                        objProvider.CreateInitializedParameter("@AccType",DbType.String, AccType),
                                                        objProvider.CreateInitializedParameter("@RptType",DbType.String, RptType),
                                                        objProvider.CreateInitializedParameter("@Branch",DbType.String, Branch),
                                                        objProvider.CreateInitializedParameter("@UptoDate",DbType.String, UptoDate),
                                                         objProvider.CreateInitializedParameter("@user_Id",DbType.String, UserID),
                                                          objProvider.CreateInitializedParameter("@curr_filter",DbType.String, curr_filter),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fin$gl$detail_GetTrialBalanceDeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetTrialBalHisList(string CompId, string BranchID,string AccId, string Type, string UptoDate, string BalType, string CurrType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@Br_Id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@Acc_Id",DbType.String, AccId),
                                                         objProvider.CreateInitializedParameter("@Type",DbType.String, Type),
                                                        objProvider.CreateInitializedParameter("@UptoDate",DbType.String, UptoDate),
                                                        objProvider.CreateInitializedParameter("@BalType",DbType.String, BalType),
                                                        objProvider.CreateInitializedParameter("@CurrType",DbType.String, CurrType),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fin$gl$detail_GetTrialBalancehistoryList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
