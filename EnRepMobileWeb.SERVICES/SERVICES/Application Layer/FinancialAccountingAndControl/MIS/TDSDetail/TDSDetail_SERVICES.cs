using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.TDSDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.TDSDetail
{
   public class TDSDetail_SERVICES : TDSDetail_ISERVICES
    {
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID,string tax_type)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    //objProvider.CreateInitializedParameter("@SuppType",DbType.String, SuppType),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@tax_type",DbType.String, tax_type),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetSuppList_TDSTCSDetails", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["supp_id"].ToString(), PARQusData.Tables[0].Rows[i]["supp_name"].ToString());
                    }
                }
                return ddlSuppListDic;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
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
        public DataSet GetTDSDetailsMIS(string CompID, string BrID, string TDSId, string suppId, string fromDate, string toDate,string tax_type,string sec_code)
        {
            try
            {
                string EntityId="0", EntityType="";
                if (!string.IsNullOrEmpty(suppId) && suppId != "0")
                {
                    var entity_info = suppId.Split('_');
                    EntityId = entity_info[0];
                    EntityType = entity_info[1];
                }
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@TDSId",DbType.String,TDSId),
                 objProvider.CreateInitializedParameter("@EntityId",DbType.String,EntityId),
                 objProvider.CreateInitializedParameter("@EntityType",DbType.String,EntityType),
                 objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                 objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                 objProvider.CreateInitializedParameter("@TaxType",DbType.String,tax_type),
                 objProvider.CreateInitializedParameter("@SecCode",DbType.String,sec_code),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$MIS_GetTDSDetail", prmContentGetDetails);
                return ds;//.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetTDSNameList(string compID, string br_ID, string tax_type)
        {
            try
            {
                
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,compID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,br_ID),
                 objProvider.CreateInitializedParameter("@tax_type",DbType.String,tax_type),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_TaxNameList_TDSDetail", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
