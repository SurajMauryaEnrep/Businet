using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.JournalBook;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.JournalBook
{
   public class JournalBook_SERVICE : JournalBook_ISERVICE
    {
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
                dr[1] = "---Select---";
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
        // public DataSet GetGLAccountList(string Comp_ID, string Br_ID, string GLGroupId)
        public Dictionary<string, string> GetGLAccountList(string Comp_ID, string Br_ID, string GLGroupId, string GroupName)
        {
            try
            {
                Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
                string firstItem = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                objProvider.CreateInitializedParameter("@GLGroupId",DbType.String , GLGroupId),
                objProvider.CreateInitializedParameter("@AccName",DbType.String, GroupName),};
                //DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$MIS$JournlBook$BindGLAccount", prmContentGetDetails);
                //return Getsuppport;

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$MIS$JournlBook$BindGLAccount", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
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
        public DataSet GetAllDDLDetails(string CompID, string BrID, string UserID, string DocumentMenuId, string language)
        {
            string PageName = string.Empty;
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@UserID",DbType.Int32, UserID),
            objProvider.CreateInitializedParameter("@doc_no",DbType.String, DocumentMenuId),
            objProvider.CreateInitializedParameter("@language",DbType.String, language)
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$MIS$JourlBook_GetAllDDLData", prmContentGetDetails);

            return DS;
        }
        public DataTable GetJournalBookDetailsMIS(string CompID, string BrID, string FromDate, string ToDate, string GroupId, string AccountID, string AmtFrom,
            string AmtTo, string VouTyp, string CreatBy, string CreatOn, string AppBy, string AppOn, string Narr, string Status)
        {
            try
            {
                //string EntityId = "0", EntityType = "";
                //if (!string.IsNullOrEmpty(suppId) && suppId != "0")
                //{
                //    var entity_info = suppId.Split('_');
                //    EntityId = entity_info[0];
                //    EntityType = entity_info[1];
                //}
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@FromDate",DbType.String,FromDate),
                 objProvider.CreateInitializedParameter("@ToDate",DbType.String,ToDate),
                 objProvider.CreateInitializedParameter("@GroupId",DbType.String,GroupId),
                 objProvider.CreateInitializedParameter("@AccountID",DbType.String,AccountID),
                 objProvider.CreateInitializedParameter("@AmtFrom",DbType.String,AmtFrom),
                 objProvider.CreateInitializedParameter("@AmtTo",DbType.String,AmtTo),
                 objProvider.CreateInitializedParameter("@VouTyp",DbType.String,VouTyp),
                 objProvider.CreateInitializedParameter("@CreatBy",DbType.String,CreatBy),
                 objProvider.CreateInitializedParameter("@CreatOn",DbType.String,CreatOn),
                 objProvider.CreateInitializedParameter("@AppBy",DbType.String,AppBy),
                 objProvider.CreateInitializedParameter("@AppOn",DbType.String,AppOn),
                 objProvider.CreateInitializedParameter("@Narr",DbType.String,Narr),
                 objProvider.CreateInitializedParameter("@Status",DbType.String,Status),

                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$MIS$JournalBook_GetAllVouchersDetail", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCostCenterData(string Comp_ID, string Br_ID, string Vou_No, string Vou_Dt, string GLAcc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/     
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Br_ID),
                   
                 objProvider.CreateInitializedParameter("@Vou_No",DbType.String,Vou_No),
                 objProvider.CreateInitializedParameter("@Vou_Dt",DbType.String,Vou_Dt),
                 objProvider.CreateInitializedParameter("@GLAcc_id",DbType.String,GLAcc_id),
               };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$MIS$JB$GetCostCenterDetails", prmContentGetDetails);
                return ds;
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
                dr[1] = "---Select---";
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
    }
}
