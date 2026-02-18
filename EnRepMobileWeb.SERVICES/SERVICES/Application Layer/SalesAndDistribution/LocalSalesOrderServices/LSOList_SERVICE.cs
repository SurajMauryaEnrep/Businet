using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesAndDistributionIServices;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.SalesAndDistributionServices
{
   public class LSOList_SERVICE:LSOList_ISERVICE
    {
        public DataSet GetStatusList(string MenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@menu_id",DbType.String, MenuID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$DocStatus", prmContentGetDetails);
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
        public DataSet GetSODetailListDAL(string CompId, string BrchID, string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId,string wfstatus ,string SO_type,string sales_person)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@CustId",DbType.String, CustId),
                                                        objProvider.CreateInitializedParameter("@OrderType",DbType.String, SO_type),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@sls_per",DbType.String, sales_person),
      
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$so$detail_GetSoDeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID,string CustType,string DocId)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String,CustType),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, DocId),
                                                     };
                //DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$cust$detail_GetCustList", prmContentGetDetails);
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetCustList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["cust_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_name"].ToString());
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
        public DataSet GetAllData(string CompID, string SuppName, string BranchID,string CustType
            , string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus, string SO_type,string sls_per)
        {
           
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String,CustType),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                      objProvider.CreateInitializedParameter("@CustId",DbType.String, CustId),
                     objProvider.CreateInitializedParameter("@OrderType",DbType.String, CustType),
                     objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                     objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                     objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                     objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                     objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                     objProvider.CreateInitializedParameter("@sls_per",DbType.String, sls_per),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$sales$Order$Detail", prmContentGetDetails);
              
                return PARQusData;

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
        public DataSet GetSO_Detail( string CompId, string BrID, string SONo, string SODate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, SONo),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, SODate),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$so$detail_GetSaleOrderRelatedDetails", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetSOTrackingDetail(string CompId, string BrID, string SONo, string SODate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@SoNo",DbType.String, SONo),
                                                        objProvider.CreateInitializedParameter("@Sodt",DbType.String, SODate),
                                                      };
                DataSet SOTrackingData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSOTrackingView", prmContentGetDetails);
                return SOTrackingData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet GetProductionTrackingDetail(string compId, string brId, string soNo, string soDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.Int64, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.Int64, brId),
                                                        objProvider.CreateInitializedParameter("@SoNo",DbType.String, soNo),
                                                        objProvider.CreateInitializedParameter("@SoDate",DbType.String, soDate),
                                                      };
                DataSet SOTrackingData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSoProductionTrackingDetails", prmContentGetDetails);
                return SOTrackingData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable GetProductionPlan_DetailsInfo(string CompID, string BrID, string Item_id, string Plan_no, string Plan_dt, string flag)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    objProvider.CreateInitializedParameter("@PpNo",DbType.String, Plan_no),
                    objProvider.CreateInitializedParameter("@PpDate",DbType.String, Plan_dt)

                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl_GetProductionPlanQC_DetailsOnClickInfo", prmContentGetDetails);
                return Getsuppport.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetItemQCParamDetail(string CompID, string br_id, string ItemID, string qc_no, string qc_dt)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                     objProvider.CreateInitializedParameter("@qc_no",DbType.String, qc_no),
                     objProvider.CreateInitializedParameter("@qc_dt",DbType.String, qc_dt),
                     objProvider.CreateInitializedParameter("@status",DbType.String, "A"),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetitemQCParamDetail]", prmContentGetDetails);
            return ds;
        }
    }
}
