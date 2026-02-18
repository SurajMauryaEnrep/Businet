using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.UTILITIES;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger
{
    public class GeneralLedger_SERVICE: GeneralLedger_ISERVICE
    {
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
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_getaccgroup_list", prmContentGetDetails);
                //DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Acc$grp_GetAllAccGroup", prmContentGetDetails);
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
        public DataTable GetCurrList(string CompID,string Supptype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@Supptype",DbType.String,Supptype),
                };
                DataTable getcurr_list = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp_CurronType", prmContentGetDetails).Tables[0];
                return getcurr_list;
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
        public DataSet GetGernalLedgerDetails(string comp_id, string br_id, string acc_id, string acc_group, string acc_type, string curr, string Fromdate, string Todate, string Rpt_As, string brlist)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                     objProvider.CreateInitializedParameter("@gl_acc",DbType.String,acc_id),
                     objProvider.CreateInitializedParameter("@acc_grp",DbType.String,acc_group),
                     objProvider.CreateInitializedParameter("@acc_type",DbType.String,acc_type),
                     objProvider.CreateInitializedParameter("@curr",DbType.String,curr),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String,Fromdate),
                     objProvider.CreateInitializedParameter("@to_dt",DbType.String,Todate),
                     objProvider.CreateInitializedParameter("@rpt_as",DbType.String,Rpt_As),
                     objProvider.CreateInitializedParameter("@br_list",DbType.String,brlist),
                };
                DataSet GetGLDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetGLAccDetails", prmContentGetDetails);
                return GetGLDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetGernalLedgerDetails(Search_Parmeters model1, int skip, int pageSize, string searchValue, string sortColumn, string sortColumnDir)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,model1.CompId),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String,model1.BrId),
                     objProvider.CreateInitializedParameter("@gl_acc",DbType.String,model1.AccID),
                     objProvider.CreateInitializedParameter("@acc_grp",DbType.String,model1.AccGrp),
                     objProvider.CreateInitializedParameter("@acc_type",DbType.String,model1.AccType),
                     objProvider.CreateInitializedParameter("@curr",DbType.String,model1.CurrId),
                     objProvider.CreateInitializedParameter("@from_dt",DbType.String,model1.From_dt),
                     objProvider.CreateInitializedParameter("@to_dt",DbType.String,model1.To_dt),
                     objProvider.CreateInitializedParameter("@rpt_as",DbType.String,model1.Rpt_As),
                 objProvider.CreateInitializedParameter("@Skip",DbType.String,skip),
                 objProvider.CreateInitializedParameter("@PageSize",DbType.String,pageSize),
                 objProvider.CreateInitializedParameter("@Search",DbType.String,searchValue),
                 objProvider.CreateInitializedParameter("@sortColumn",DbType.String,sortColumn),
                 objProvider.CreateInitializedParameter("@sortColumnDir",DbType.String,sortColumnDir),
                 objProvider.CreateInitializedParameter("@forCsv",DbType.String,model1.Flag),
                 objProvider.CreateInitializedParameter("@br_list",DbType.String,model1.br_ids)
                };
                DataSet GetGLDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetGLAccDetails", prmContentGetDetails);
                return GetGLDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
