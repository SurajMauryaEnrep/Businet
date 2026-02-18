using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesForecast;
using EnRepMobileWeb.UTILITIES;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.SalesForecast
{
   public class SalesForecast_SERVICES : SalesForecast_ISERVICES
    {
        public String insertFCDetail(DataTable HeaderTable, DataTable jcitemdetails, DataTable dtSubItem)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@sls$forecast$header",SqlDbType.Structured, HeaderTable),
                 objprovider.CreateInitializedParameterTableType("@sls$forecast$item",SqlDbType.Structured, jcitemdetails),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                  objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_sls$forecast$insert", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[2].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[2].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public String cancelFCDetail(int CompID, int BrID, string f_freq, string f_fy, string f_period, string create_id,string transtype)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@comp_id",SqlDbType.Int, CompID),
                  objprovider.CreateInitializedParameterTableType("@br_id",SqlDbType.Int, BrID),
                   objprovider.CreateInitializedParameterTableType("@f_freq",SqlDbType.NVarChar, f_freq),
                    objprovider.CreateInitializedParameterTableType("@f_fy",SqlDbType.NVarChar, f_fy),
                     objprovider.CreateInitializedParameterTableType("@f_period",SqlDbType.NVarChar, f_period),
                 objprovider.CreateInitializedParameterTableType("@create_id",SqlDbType.NVarChar, create_id),
                 objprovider.CreateInitializedParameterTableType("@TransType",SqlDbType.NVarChar, transtype),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[7].Size = 100;
                prmcontentaddupdate[7].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_sls$forecast$cancel", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[7].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[7].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindFinancialYear(int CompID, int BrID, string f_freq, string StartDate,string Period)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@f_freq",DbType.String, f_freq),
                    objProvider.CreateInitializedParameter("@StartDate",DbType.String, StartDate),
                     objProvider.CreateInitializedParameter("@trtype",DbType.String, Period),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$forecast$get$fy", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetFCList(int CompID, int BrID, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$forecast$getlist", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindPeriod(int CompID, int BrID, string f_freq, string StartDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@f_freq",DbType.String, f_freq),
                    objProvider.CreateInitializedParameter("@StartDate",DbType.String, StartDate),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$forecast$get$fy", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindDateRangeCal(int CompID, int BrID, string f_frequency, string start_year, string end_year, Int32 months, string fy_datefrom, string fy_dateto, string ItmName, string f_fy_full)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@f_freq",DbType.String, f_frequency),
                    objProvider.CreateInitializedParameter("@start_year",DbType.String, start_year),
                    objProvider.CreateInitializedParameter("@end_year",DbType.String, end_year),
                    objProvider.CreateInitializedParameter("@months",DbType.Int32, months),
                    objProvider.CreateInitializedParameter("@fy_datefrom",DbType.String, fy_datefrom),
                    objProvider.CreateInitializedParameter("@fy_dateto",DbType.String, fy_dateto),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String,ItmName),
                    objProvider.CreateInitializedParameter("@f_fy_full",DbType.String, f_fy_full),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$forecast$get$period", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindItemNameInDDL(int CompID, int BrID,string itemname)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, itemname),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$forecast$ItemList", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSOItemUOMDL(string comp_id, string BrID,string Itm_id,string fromdate, string todate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_id),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@Itm_id",DbType.String, Itm_id),
                                                        objProvider.CreateInitializedParameter("@fromdate",DbType.Date, fromdate),
                                                        objProvider.CreateInitializedParameter("@todate",DbType.Date, todate),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$forecast$PreviousYearSales", prmContentGetDetails);
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
        public DataSet BinddbClick(int CompID, int BrID, /*string f_freq, string f_fy,Int32 f_period //Commented by Suraj */ string sf_id, string UserID, string DocumentMenuId)
        {
            //string f_status,
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@f_freq",DbType.String, ""),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String, ""),
                    objProvider.CreateInitializedParameter("@f_period",DbType.Int32, 0),
                    objProvider.CreateInitializedParameter("@sf_id",DbType.String, sf_id),
                    //objProvider.CreateInitializedParameter("@f_status",DbType.String, f_status),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$forecast$db$edit", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approve_SalesForecast(string comp_id, string br_id,string sf_id, /*string sfc_no,*/ string sfc_date, string A_Status, string A_Level, string A_Remarks, string CreatedBy, string mac_id, string f_fy, string status, string period,  string DocumentMenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@sf_id",DbType.String,sf_id),
                    //objProvider.CreateInitializedParameter("@sfc_no",DbType.String,sfc_no), //Commented by Suraj
                    objProvider.CreateInitializedParameter("@sfc_dt",DbType.Date,  sfc_date),
                    objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                    objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                    objProvider.CreateInitializedParameter("@CreatedBy",DbType.String, CreatedBy),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@f_status",DbType.String,status),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String,f_fy ),
                    objProvider.CreateInitializedParameter("@f_period",DbType.String, period),
                    //objProvider.CreateInitializedParameter("@TransType",DbType.String, ""),
                     objProvider.CreateInitializedParameter("@DocID",DbType.String, DocumentMenuID),

                };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_sls$forecast$Approve]", prmContentGetDetails);
                string DocuSFRNo = string.Empty;
                DocuSFRNo = ImageDeatils.Tables[0].Rows[0]["sfrc_detail"].ToString();
                return DocuSFRNo; 
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
         public DataSet PreviousSalQty_GetSubItemDetails(string CompID, string BrchID, string Item_id, string fromdate2, string todate2)
         {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@fromdate",DbType.String,fromdate2),
                    objProvider.CreateInitializedParameter("@todate",DbType.String, todate2),

                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PreviousSaleQty_GetSubItemDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet SF_GetSubItemDetailsAftrApprove(string CompID, string BrchID, string Item_id, string Doc_no, string Doc_dt)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,Doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, Doc_dt),

                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SF_GetSubItemDetailsAftrApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckSalesForecastAgainstPP(string CompId, string BrchID, string financial_year, string Period, string FromDate, string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@f_fy",DbType.String, financial_year),
                                                        objProvider.CreateInitializedParameter("@Period",DbType.String, Period),
                                                        objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                                                        objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$forecast$detail_CheckSalesForecastAgainstPP", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
