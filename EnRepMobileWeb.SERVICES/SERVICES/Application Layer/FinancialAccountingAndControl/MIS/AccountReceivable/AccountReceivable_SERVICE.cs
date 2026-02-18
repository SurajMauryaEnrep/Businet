using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.AccountReceivable;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.AccountReceivable
{
   public class AccountReceivable_SERVICE : AccountReceivable_ISERVICE
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

        public DataSet GetUserRangeDetail(string CompID,string UserID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_UserAgingBucket$View]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public string InsertUserRangeDetail(string CompID, string user_id, string range1, string range2, string range3, string range4, string range5)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                               objprovider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                 objprovider.CreateInitializedParameter("@user_id",DbType.String, user_id ),
                 objprovider.CreateInitializedParameter("@range1",DbType.String, range1 ),
                 objprovider.CreateInitializedParameter("@range2",DbType.String, range2 ),
                 objprovider.CreateInitializedParameter("@range3",DbType.String, range3 ),
                 objprovider.CreateInitializedParameter("@range4",DbType.String, range4 ),
                 objprovider.CreateInitializedParameter("@range5",DbType.String, range5 ),
                 //objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };

                string rangedt = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "fin$gl$UserAgingBucket$InsertUpdate", prmcontentaddupdate).ToString();
                return rangedt;
            }
            catch (SqlException ex)
            {
                throw ex;

            }


        }
        public DataSet GetAgingDetailList(string CompId, string BranchID, string UserID, string Cust_id, string Cat_id, string Prf_id, string Reg_id, string Basis, string AsDate,int Curr_Id, string Flag,int Acc_Id,string ReceivableType,string ReportType, string brlist,string sales_per,string customerZone,string CustomerGroup,string state_id,string city_id)
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
                                                        objProvider.CreateInitializedParameter("@basis",DbType.String, Basis),
                                                        objProvider.CreateInitializedParameter("@date",DbType.String, AsDate),
                                                         
                                                         objProvider.CreateInitializedParameter("@curr",DbType.Int32, Curr_Id),
                                                         objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                         objProvider.CreateInitializedParameter("@Acc_Id",DbType.String, Acc_Id),
                                                         objProvider.CreateInitializedParameter("@ReceivableType",DbType.String, ReceivableType),
                                                         objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                         objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist),
                                                         objProvider.CreateInitializedParameter("@sales_per",DbType.String, sales_per),
                                                         objProvider.CreateInitializedParameter("@customerZone",DbType.String, customerZone),
                                                         objProvider.CreateInitializedParameter("@CustomerGroup",DbType.String, CustomerGroup),
                                                         objProvider.CreateInitializedParameter("@state_id",DbType.String, state_id),
                                                         objProvider.CreateInitializedParameter("@city_id",DbType.String, city_id),
                                                         objProvider.CreateInitializedParameter("@db_flag",DbType.String, ""),
                                                      };
                DataSet GetAgingDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetCustomerAgingDetails", prmContentGetDetails);
                //return GetAgingDetailList.Tables[0];
                return GetAgingDetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetInvoiceDetailList(string CompId, string BranchID, string Cust_id, string lrange, string urange,  string Basis, string AsDate, int CurrId, string ReceivableType,string ReportType,string inv_no,string inv_dt, string brlist,string sls_per,string user_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@cust_id",DbType.String, Cust_id),
                                                        objProvider.CreateInitializedParameter("@lrange",DbType.String, lrange),
                                                        objProvider.CreateInitializedParameter("@urange",DbType.String, urange),
                                                        objProvider.CreateInitializedParameter("@basis",DbType.String, Basis),
                                                        objProvider.CreateInitializedParameter("@asondate",DbType.String, AsDate),
                                                        objProvider.CreateInitializedParameter("@CurrId",DbType.Int32, CurrId),
                                                        objProvider.CreateInitializedParameter("@ReceivableType",DbType.String, ReceivableType),
                                                        objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, inv_no),
                                                        objProvider.CreateInitializedParameter("@inv_dt",DbType.String, inv_dt),
                                                        objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist),
                                                        objProvider.CreateInitializedParameter("@sales_per",DbType.String, sls_per),
                                                        objProvider.CreateInitializedParameter("@user_id",DbType.String, user_id),
                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetCustomerAgingHistory", prmContentGetDetails);
                return GetInvoiceDetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet SearchAdvanceAmountDetail(string compId, string brId, string accId, int CurrId, string AsDate, string Basis, string ReceivableType, string brlist)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@AccId",DbType.String, accId),
                                                        objProvider.CreateInitializedParameter("@CurrId",DbType.Int32, CurrId),
                                                        //objProvider.CreateInitializedParameter("@urange",DbType.String, urange),
                                                        //objProvider.CreateInitializedParameter("@basis",DbType.String, Basis),
                                                        objProvider.CreateInitializedParameter("@asondate",DbType.Date, AsDate),
                                                        objProvider.CreateInitializedParameter("@Basis",DbType.String, Basis),
                                                        objProvider.CreateInitializedParameter("@ReceivableType",DbType.String, ReceivableType),
                                                        objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist),
                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Sp_GetAccReceivableAdvanceAmtDetail", prmContentGetDetails);
                return GetInvoiceDetailList;
                //return GetInvoiceDetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet SearchPaidAmountDetail(string compId, string brId, string InVNo, string InvDate,string cust_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@InVNo",DbType.String, InVNo),
                                                        objProvider.CreateInitializedParameter("@InvDate",DbType.String, InvDate),
                                                        objProvider.CreateInitializedParameter("@cust_id",DbType.String, cust_id),

                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Sp_CMN_Fin$MIS$GetAR_AP$PaidAmountDetail", prmContentGetDetails);
                return GetInvoiceDetailList;
                //return GetInvoiceDetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceDeatilsForPrint(string CompID, string BrchID, string invNo, string invDate, string dataType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, invNo),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, invDate),
                                                        objProvider.CreateInitializedParameter("@dataType",DbType.String, dataType),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_AccRecevabl_GetInvoiceDeatilsForPopup", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date, string inv_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, SI_No),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, SI_Date),
                                                        objProvider.CreateInitializedParameter("@flag",DbType.String, inv_type),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSalesInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSlsInvGstDtlForPrint(string compId, string brchId, string siNo, string siDate, string invType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                
                    SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brchId),
                                                        objProvider.CreateInitializedParameter("@InvNo",DbType.String, siNo),
                                                        objProvider.CreateInitializedParameter("@InvDate",DbType.String, siDate),
                                                        objProvider.CreateInitializedParameter("@Flag",DbType.String, invType),
                                                        
                                                      };
                    DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSlsInvGSTDtl_ForPrint", prmContentGetDetails);
                    return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetGLAccountReceivablePrintData(string compId, string brId, string accId, string currId, string asOnDate, string userId, string brlist)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@AccId",DbType.String, accId),
                                                        objProvider.CreateInitializedParameter("@CurrId",DbType.String, currId),
                                                        objProvider.CreateInitializedParameter("@AsOnDate",DbType.String, asOnDate),
                                                        objProvider.CreateInitializedParameter("@UserId",DbType.String, userId),
                                                        objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist)
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetGLAccountReceivablePrintData", prmContentGetDetails);
                return Get_SI_List;
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
        public DataSet GetCustomerDropdowns(string compId,string StateName, string CityName)
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
        public DataSet BindCityListdata(string CompID, string br_id, string SarchValue,string State_id)
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
    }
}
