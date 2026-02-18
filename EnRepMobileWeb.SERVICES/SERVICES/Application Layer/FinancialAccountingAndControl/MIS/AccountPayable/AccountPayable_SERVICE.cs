using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.AccountPayable;
using EnRepMobileWeb.UTILITIES;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.AccountPayable
{
    public class AccountPayable_SERVICE: AccountPayable_ISERVICE
    {

        public DataSet GetUserRangeDetail(string CompID, string UserID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_Ap$UserAgingBucket$View]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID, string Doc_id)
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
                    objProvider.CreateInitializedParameter("@Doc_id",DbType.String, Doc_id),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetSuppList", prmContentGetDetails);
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
        public DataTable GetcategoryDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getcategory = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$category_Getcategory", prmContentGetDetails).Tables[0];
                return Getcategory;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetsuppportDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp$portfolio_Getportfolio", prmContentGetDetails).Tables[0];
                return Getsuppport;
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

                string rangedt = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "fin$gl$Ap$UserAgingBucket$InsertUpdate", prmcontentaddupdate).ToString();
                return rangedt;
            }
            catch (SqlException ex)
            {
                throw ex;

            }


        }
        public DataSet GetAgingDetailList(string CompId, string BranchID, string UserID, string Supp_id, string Cat_id, string Prf_id, string Basis, string AsDate, int Curr_Id, string Flag, int Acc_Id,string PayableType,string ReportType, string brlist)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@supp_id",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@cate_id",DbType.String, Cat_id),
                                                        objProvider.CreateInitializedParameter("@prf_id",DbType.String, Prf_id),
                                                        objProvider.CreateInitializedParameter("@basis",DbType.String, Basis),
                                                        objProvider.CreateInitializedParameter("@date",DbType.String, AsDate),
                                                         objProvider.CreateInitializedParameter("@user_id",DbType.String, UserID),
                                                         objProvider.CreateInitializedParameter("@curr",DbType.Int32, Curr_Id),
                                                         objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                         objProvider.CreateInitializedParameter("@Acc_Id",DbType.String, Acc_Id),
                                                         objProvider.CreateInitializedParameter("@PayableType",DbType.String, PayableType),
                                                         objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                         objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist),
                                                      };
                DataSet GetAgingDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSupplierAgingDetails", prmContentGetDetails);
                //return GetAgingDetailList.Tables[0];
                return GetAgingDetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetInvoiceDetailList(string CompId, string BranchID, string Supp_id, string lrange, string urange, string Basis, string AsDate, int CurrId,string PayableType,string ReportType,string inv_no,string inv_dt, string brlist)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@supp_id",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@lrange",DbType.String, lrange),
                                                        objProvider.CreateInitializedParameter("@urange",DbType.String, urange),
                                                        objProvider.CreateInitializedParameter("@basis",DbType.String, Basis),
                                                        objProvider.CreateInitializedParameter("@asondate",DbType.String, AsDate),
                                                        objProvider.CreateInitializedParameter("@CurrId",DbType.Int32, CurrId),
                                                        objProvider.CreateInitializedParameter("@PayableType",DbType.String, PayableType),
                                                        objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, inv_no),
                                                        objProvider.CreateInitializedParameter("@inv_dt",DbType.String, inv_dt),
                                                        objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist),
                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetSupplierAgingHistory", prmContentGetDetails);
                return GetInvoiceDetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataTable SearchAdvanceAmountDetail(string compId, string brId, string accId, int CurrId)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
        //                                                objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
        //                                                objProvider.CreateInitializedParameter("@AccId",DbType.String, accId),
        //                                                objProvider.CreateInitializedParameter("@CurrId",DbType.String, CurrId),
        //                                                //objProvider.CreateInitializedParameter("@urange",DbType.String, urange),
        //                                                //objProvider.CreateInitializedParameter("@basis",DbType.String, Basis),
        //                                                //objProvider.CreateInitializedParameter("@asondate",DbType.String, AsDate),
        //                                              };
        //        DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Sp_GetAccPayableAdvanceAmtDetail", prmContentGetDetails);
        //        return GetInvoiceDetailList.Tables[0];
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet SearchAdvanceAmountDetail(string compId, string brId, string accId, int CurrId,string AsDate, string Basis, string PayableType, string brlist)
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
                                                        objProvider.CreateInitializedParameter("@PayableType",DbType.String, PayableType),
                                                        objProvider.CreateInitializedParameter("@br_list",DbType.String, brlist),
                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Sp_GetAccPayableAdvanceAmtDetail", prmContentGetDetails);
                return GetInvoiceDetailList;
                //return GetInvoiceDetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet SearchPaidAmountDetail(string compId, string brId, string InVNo, string InvDate, string AsOnDate,string supp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@InVNo",DbType.String, InVNo),
                                                        objProvider.CreateInitializedParameter("@InvDate",DbType.String, InvDate),
                                                        objProvider.CreateInitializedParameter("@AsOnDate",DbType.String, AsOnDate),
                                                        objProvider.CreateInitializedParameter("@supp_id",DbType.String, supp_id),
                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_getpaidvou_detail", prmContentGetDetails);
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
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_AccPayble_GetInvoiceDeatilsForPopup", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetGLAccountPayablePrintData(string compId, string brId, string accId, string currId, string asOnDate,string userId, string brlist)
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
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetGLAccountPayablePrintData", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
