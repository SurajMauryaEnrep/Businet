using EnRepMobileWeb.UTILITIES;
using System;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.WorkstationSetup;
using System.Collections.Generic;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.WorkstationSetup
{
   public class WorkstationSetup_SERVICES : WorkstationSetup_ISERVICES
    {
        public String insertWorkStationDetail(int comp_id,int br_id, int ws_id, string ws_name, int shfl_id, string op_st_date, string op_name,
            string sr_no, int user_id, string TransType, string Make, string Model_no, string Grp_nm,string mac_id, DataTable WRKSAttachments,DataTable WorkstationCapacity)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@comp_id",SqlDbType.Int, comp_id),
                 objprovider.CreateInitializedParameterTableType("@br_id",SqlDbType.Int, br_id ),
                 objprovider.CreateInitializedParameterTableType("@ws_id",SqlDbType.Int,ws_id),
                 objprovider.CreateInitializedParameterTableType("@ws_name",SqlDbType.NVarChar,ws_name),
                 objprovider.CreateInitializedParameterTableType("@shfl_id",SqlDbType.Int,shfl_id ),
                 objprovider.CreateInitializedParameterTableType("@op_st_date",SqlDbType.NVarChar,op_st_date),
                 objprovider.CreateInitializedParameterTableType("@op_name",SqlDbType.NVarChar,op_name),
                  objprovider.CreateInitializedParameterTableType("@sr_no",SqlDbType.NVarChar,sr_no),
                   objprovider.CreateInitializedParameterTableType("@user_id",SqlDbType.Int,user_id),
                    objprovider.CreateInitializedParameterTableType("@TransType",SqlDbType.Char,TransType),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@Make",SqlDbType.NVarChar,Make),
                 objprovider.CreateInitializedParameterTableType("@Model_no",SqlDbType.NVarChar,Model_no),
                 objprovider.CreateInitializedParameterTableType("@Grp_nm",SqlDbType.NVarChar,Grp_nm),
                 objprovider.CreateInitializedParameterTableType("@mac_id",SqlDbType.NVarChar,mac_id),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, WRKSAttachments ),
                 objprovider.CreateInitializedParameterTableType("@itmTable",SqlDbType.Structured, WorkstationCapacity ),
                };
                prmcontentaddupdate[10].Size = 100;
                prmcontentaddupdate[10].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_pp$ws$setup_insert", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[10].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[10].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public Dictionary<string, string> GetGroupList(string CompID, string GroupName)
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

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$grp_GetAllItemGroup_LastLevel", prmContentGetDetails);

                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["item_grp_id"].ToString(), PARQusData.Tables[0].Rows[i]["ItemGroupChildNood"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
         
        }
        public DataTable GetShopFloorDetailsDAL(int CompID, int br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),                               };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_pp$ws$BindShfl", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetWSDetailsDAL(int CompID, int br_id,int shfl_id,string action)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                     objProvider.CreateInitializedParameter("@shfl_id",DbType.Int32, shfl_id),
                     objProvider.CreateInitializedParameter("@action",DbType.String, action),
                };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_pp$ws$Bind", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetWSDoubleClickEdit(Int32 comp_id, Int32 br_id, Int32 ws_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                    objProvider.CreateInitializedParameter("@ws_id",DbType.Int32, ws_id),
                                                    };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_pp$ws$Edit", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
