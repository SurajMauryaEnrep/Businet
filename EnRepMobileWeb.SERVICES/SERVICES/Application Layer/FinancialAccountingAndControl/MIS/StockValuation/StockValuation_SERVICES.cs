using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.StockValuation;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.StockValuation
{
   public class StockValuation_SERVICES : StockValuation_ISERVICES
    {
        public DataTable BindGetGroupList(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$grp_GetAllItemGroupNoodChilds_ItemList", prmContentGetDetails).Tables[0];
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
        public DataTable GetFinYearList(string CompID, string brId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] GetfinyrList = {
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int16, brId),
                    //objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                };
                DataTable YearList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Fin$MIS$StkValutn_GetFinYear", GetfinyrList).Tables[0];
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
        public DataSet BindFinancialYearMonths(int CompID, int BrID, string fin_sfy)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@fin_sfy",DbType.String, fin_sfy),
                    
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Fin$MIS$StkVal$GetAllMonthsOfFY", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetStkValDetailsMIS(string CompID, string BrID, string ItmGrpID, string ReportType
            , string costbase, string inc_shfl, string inc_zero,string acc_id, string from_dt
            , string to_dt, string ftype,string sp_uom_id,string priceList)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompId",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@ItmGrpID",DbType.String,ItmGrpID),
                 //objProvider.CreateInitializedParameter("@FinYear",DbType.String,FinYear),
                 //objProvider.CreateInitializedParameter("@Month",DbType.String,Month),
                 objProvider.CreateInitializedParameter("@ReportType",DbType.String,ReportType),
                 objProvider.CreateInitializedParameter("@CostBase",DbType.String,costbase),
                 objProvider.CreateInitializedParameter("@Inc_Shfl",DbType.String,inc_shfl),
                 objProvider.CreateInitializedParameter("@Inc_zero",DbType.String,inc_zero),
                 objProvider.CreateInitializedParameter("@acc_id",DbType.String,acc_id),
                 objProvider.CreateInitializedParameter("@fromdt",DbType.String,from_dt),
                 objProvider.CreateInitializedParameter("@todt",DbType.String,to_dt),
                 objProvider.CreateInitializedParameter("@filter_type",DbType.String,ftype),
                 objProvider.CreateInitializedParameter("@sp_uom_id",DbType.String,sp_uom_id),
                 objProvider.CreateInitializedParameter("@priceList",DbType.String,priceList),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Fin$MIS_GetStkValuationDetail", prmContentGetDetails);
                //return ds.Tables[0];
                return ds;

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

        public DataSet Get_rcptIssueDetail(string CompID, string BrID, string id, string flag, string rpt_type
            , string shfl_flag, string from_dt, string to_dt,string cost_type,string priceList)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@search_id",DbType.String,id),
                 objProvider.CreateInitializedParameter("@from_dt",DbType.String,from_dt),
                 objProvider.CreateInitializedParameter("@to_dt",DbType.String,to_dt),
                 objProvider.CreateInitializedParameter("@flag",DbType.String,flag),
                 objProvider.CreateInitializedParameter("@rpt_type",DbType.String,rpt_type),
                 objProvider.CreateInitializedParameter("@shfl_flag",DbType.String,shfl_flag),
                 objProvider.CreateInitializedParameter("@cost_base",DbType.String,cost_type),
                 objProvider.CreateInitializedParameter("@priceList",DbType.String,priceList),

                 };
                DataSet dtbl = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_getstkval_rcptissuedetail", prmContentGetDetails);
                return dtbl;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Get_priceList(string comp_ID, string br_ID, string searchValue, string fromDt, string toDt)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_ID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,br_ID),
                 objProvider.CreateInitializedParameter("@searchValue",DbType.String,searchValue),
                 //objProvider.CreateInitializedParameter("@fromDt",DbType.String,fromDt),
                 //objProvider.CreateInitializedParameter("@toDt",DbType.String,toDt)
                 
                 };
                DataSet dtbl = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_getPriceList", prmContentGetDetails);
                return dtbl;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
