using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.MISCollectionDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.MIS.MISCollectionDetail
{
    public class MISCollectionDetail_Service: MISCollectionDetail_IService
    {
        public DataTable GetRegionDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetRegion = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$region", prmContentGetDetails).Tables[0];
                return GetRegion;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetcategoryDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getcategory = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$category", prmContentGetDetails).Tables[0];
                return Getcategory;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCustportDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$portfolio", prmContentGetDetails).Tables[0];
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetSalesPersonList(string compId, string brId, string userid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@SPersonName",DbType.String,"0"),
                 objProvider.CreateInitializedParameter("@BrchID",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@user_id",DbType.String,userid),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$sls$person$comp_GetSalesPersonList", prmContentGetDetails);
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustomerDropdowns(string compId, string StateName, string CityName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@StateName",DbType.String, StateName),
                    objProvider.CreateInitializedParameter("@cityName",DbType.String, CityName),
                    objProvider.CreateInitializedParameter("@state_id",DbType.String, "0"),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$cust$common$ddl", prmContentGetDetails);
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
        public DataSet BindStateListData(string CompID, string br_id, string SarchValue)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@StateName",DbType.String, SarchValue),
                    objProvider.CreateInitializedParameter("@cityName",DbType.String, "0"),
                    objProvider.CreateInitializedParameter("@state_id",DbType.String, "0"),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$cust$common$ddl", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindCityListdata(string CompID, string br_id, string SarchValue, string State_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@StateName",DbType.String, "0"),
                    objProvider.CreateInitializedParameter("@cityName",DbType.String, SarchValue),
                    objProvider.CreateInitializedParameter("@state_id",DbType.Int32, State_id),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$cust$common$ddl", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCollectionDetailList(string CompId, string BranchID, string UserID, string Cust_id, string Cat_id, string Prf_id, string Reg_id, string AsDate, int Curr_Id, string Flag, int Acc_Id, string ReceivableType, string ReportType, string brlist, string customerZone, string CustomerGroup, string state_id, string city_id,string includeZero,string sales_per)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@user_id",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@cust_id",DbType.String, Cust_id),
                                                        objProvider.CreateInitializedParameter("@cate_id",DbType.String, Cat_id),
                                                        objProvider.CreateInitializedParameter("@prf_id",DbType.String, Prf_id),
                                                        objProvider.CreateInitializedParameter("@region_id",DbType.String,Reg_id),
                                                        objProvider.CreateInitializedParameter("@date",DbType.String, AsDate),
                                                         objProvider.CreateInitializedParameter("@curr",DbType.Int32, Curr_Id),
                                                         objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                         objProvider.CreateInitializedParameter("@Acc_Id",DbType.String, Acc_Id),
                                                         objProvider.CreateInitializedParameter("@ReceivableType",DbType.String, ReceivableType),
                                                         objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                         objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist),
                                                         objProvider.CreateInitializedParameter("@customerZone",DbType.String, customerZone),
                                                         objProvider.CreateInitializedParameter("@CustomerGroup",DbType.String, CustomerGroup),
                                                         objProvider.CreateInitializedParameter("@state_id",DbType.String, state_id),
                                                         objProvider.CreateInitializedParameter("@city_id",DbType.String, city_id),
                                                         objProvider.CreateInitializedParameter("@includeZero",DbType.String, includeZero),
                                                         objProvider.CreateInitializedParameter("@sales_per",DbType.String, sales_per),
                                                      };
                DataSet GetAgingDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetCollectionDetails", prmContentGetDetails);
                //return GetAgingDetailList.Tables[0];
                return GetAgingDetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesAmtDetails(string CompId, string BranchID, string Cust_id, string AsDate, int CurrId, string ReceivableType, string ReportType, string inv_no, string inv_dt, string brlist, string user_id,string includeZero)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@cust_id",DbType.String, Cust_id),         
                                                        objProvider.CreateInitializedParameter("@asondate",DbType.String, AsDate),
                                                        objProvider.CreateInitializedParameter("@CurrId",DbType.Int32, CurrId),
                                                        objProvider.CreateInitializedParameter("@ReceivableType",DbType.String, ReceivableType),
                                                        objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, inv_no),
                                                        objProvider.CreateInitializedParameter("@inv_dt",DbType.String, inv_dt),
                                                        objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist),
                                                        objProvider.CreateInitializedParameter("@user_id",DbType.String, user_id),
                                                        objProvider.CreateInitializedParameter("@includeZero",DbType.String, includeZero),
                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetSalesAmtDetailsList", prmContentGetDetails);
                return GetInvoiceDetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet SearchPaidAmountDetail(string CompId, string BranchID, string UserID, string Cust_id, string curr_id, string AsDate, string ReceivableType, string ReportType, string brlist, string includeZero)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                         objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@user_id",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@cust_id",DbType.String, Cust_id),
                                                        objProvider.CreateInitializedParameter("@curr_id",DbType.String, curr_id),
                                                        objProvider.CreateInitializedParameter("@date",DbType.String, AsDate),
                                                         objProvider.CreateInitializedParameter("@ReceivableType",DbType.String, ReceivableType),
                                                         objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                         objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist),
                                                         objProvider.CreateInitializedParameter("@includeZero",DbType.String, includeZero),
                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetCollection$PaidAmountDetail", prmContentGetDetails);
                return GetInvoiceDetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
