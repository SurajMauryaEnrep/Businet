using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.FinanceBudget;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.FinanceBudget;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.FinanceBudget
{
    public class Financebudget_Services : Financebudget_Iservices
    {
        public DataTable GetFinYearList(string CompID, string brId,string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] GetfinyrList = {
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int16, brId),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                };
                DataTable YearList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$FinanceBudget$getFinyr", GetfinyrList).Tables[0];
                return YearList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable GetFinYearListpage(string CompID, string brId, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] GetfinyrList = {
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int16, brId),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                };
                DataTable YearList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$FinanceBudget$Lispage", GetfinyrList).Tables[0];
                return YearList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable GlList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] GetfinyrList = {
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int16, CompID),
                };
                DataTable YearList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Usp$GetFinanceBudgetGlList", GetfinyrList).Tables[0];
                return YearList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string FinBudDelete(string CompID,string BrId, FinanceBudgetModel financeBudgetModel)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@compId",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@brId",DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@finYear",DbType.String, financeBudgetModel.FinYears),
                    objProvider.CreateInitializedParameter("@revno",DbType.Int16, financeBudgetModel.Revno),
                    objProvider.CreateInitializedParameter("@FinancialYear",DbType.String,""),
                                                     };
                prmContentGetDetails[4].Size = 100;
                prmContentGetDetails[4].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[Usp$FinanceBudget$Delete]", prmContentGetDetails).ToString();
                //return ActionDeatils;
                string Finyrs = string.Empty;
                if (prmContentGetDetails[4].Value != DBNull.Value) // status
                {
                    Finyrs = prmContentGetDetails[4].Value.ToString();
                }
                return Finyrs;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        public String InsertFinbudDetails(DataTable FinBudHeader, DataTable FinBugGlDetails, DataTable FinMonQtrDetails,DataTable FBCostCenterDetails)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@Headerdetail",SqlDbType.Structured, FinBudHeader ),
                 objprovider.CreateInitializedParameterTableType("@AccountDetails",SqlDbType.Structured, FinBugGlDetails ),
                 objprovider.CreateInitializedParameterTableType("@QuaterMonthDetail",SqlDbType.Structured, FinMonQtrDetails ),
                 objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, FBCostCenterDetails ),
                 //objprovider.CreateInitializedParameterTableType("@CostcenterAllocation",SqlDbType.Structured, FinCostCenter ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string Fin_Bud = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Usp$FinanceBudget$insertUpdate", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }


        }

        public DataSet GetFinBudgetDetail(string CompID, string BrId, string Finyear, string Revno, string UserID,string Period, string DocumentMenuId,string RevMess)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@compId",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@Finyear",DbType.String, Finyear),
                    objProvider.CreateInitializedParameter("@Revno",DbType.String, Revno),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@Period",DbType.String, Period),
                    objProvider.CreateInitializedParameter("@Message",DbType.String, RevMess),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[FinanceBudget$Detailview]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataSet GetFinanBudList(string CompId, string BrId, string UserID, string DocumentMenuId, string WF_Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetFinBudDetails = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, WF_Status),
                   
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Usp$GetFinbudget$list]", prmContentGetFinBudDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataSet SerachListFinBudget(string compId, string BrId, string Finyear, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmGetSearchFinBudList = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@FinYear",DbType.String, Finyear),
                    objProvider.CreateInitializedParameter("@FbStatus",DbType.String, Status),

            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Usp$FinbudList$OnSearch]", prmGetSearchFinBudList);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string ApproveFinanceBudgetDetails(string Finyear, string Revno, string FB_Date, string CompID, string BrchID, string UserID,  string A_Status, string A_Level, string A_Remarks,string mac_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@FinYear",DbType.String, Finyear ),
                                                        objProvider.CreateInitializedParameter("@RevNo",DbType.String, Revno),
                                                        objProvider.CreateInitializedParameter("@BudDate",DbType.String,FB_Date),
                                                        objProvider.CreateInitializedParameter("@DocumentId",DbType.String,DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@UserId",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                                                        objProvider.CreateInitializedParameter("@macid",DbType.String, mac_id),
                };
                string FinBud = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "FinBud$FB$ApproveBudget", prmContentInsert).ToString();
                return FinBud;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }

        public string FinanceBudRevised(string Revno, string Finyr, string Period, string Buddate, string CompID, string BrId, string UserID, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmGetFinBudgetRevise = {/*Passing perameter to sotore procedure*/   
                    
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, BrId),
                    objProvider.CreateInitializedParameter("@UserId",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@macId",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@RevNo",DbType.String, Revno),
                    objProvider.CreateInitializedParameter("@BudDate",DbType.String, Buddate),
                    objProvider.CreateInitializedParameter("@Finyear",DbType.String, Finyr),
                    objProvider.CreateInitializedParameter("@period",DbType.String, Period),


            };
                string Revised = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "[Usp$FinanceBudget$Revise]", prmGetFinBudgetRevise).ToString();
                return Revised;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }
}
