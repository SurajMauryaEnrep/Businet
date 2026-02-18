using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.SalesDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.MIS.SalesDetail
{
   
    public class SalesDetail_SERVICES : SalesDetail_ISERVICES
    {
        public DataSet GetSales_Detail(string CompID, string BrID, string userid, string cust_id,string reg_name, string sale_type, string curr_id, string productGrp,
            string Product_Id, string productPort, string custCat, string CustPort, string inv_no, 
            string inv_dt, string sale_per,string From_dt, string To_dt, string Flag,string HSN_code,string custzone, string custgroup,string custstate,string custcity,string brlist,string uom_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,BrID),
                 objProvider.CreateInitializedParameter("@userid",DbType.Int32,userid),
                 objProvider.CreateInitializedParameter("@cust_id",DbType.String,cust_id),
                 objProvider.CreateInitializedParameter("@reg_name",DbType.String,reg_name),
                 objProvider.CreateInitializedParameter("@sale_type",DbType.String,sale_type),
                 objProvider.CreateInitializedParameter("@curr_id",DbType.String,curr_id),
                 objProvider.CreateInitializedParameter("@productGrp",DbType.String,productGrp),
                 objProvider.CreateInitializedParameter("@productPort",DbType.String,productPort),
                 objProvider.CreateInitializedParameter("@Product_Id",DbType.String,Product_Id),
                 objProvider.CreateInitializedParameter("@custCat",DbType.String,custCat),
                 objProvider.CreateInitializedParameter("@CustPort",DbType.String,CustPort),
                 objProvider.CreateInitializedParameter("@inv_no",DbType.String,inv_no),
                 objProvider.CreateInitializedParameter("@inv_dt",DbType.String,inv_dt),
                 objProvider.CreateInitializedParameter("@sale_per",DbType.String,sale_per),
                 objProvider.CreateInitializedParameter("@from_dt",DbType.String,From_dt),
                 objProvider.CreateInitializedParameter("@to_dt",DbType.String,To_dt),
                 objProvider.CreateInitializedParameter("@Flag",DbType.String,Flag),
                 objProvider.CreateInitializedParameter("@HSN_code",DbType.String,HSN_code),
                 objProvider.CreateInitializedParameter("@custzone",DbType.String,custzone),
                 objProvider.CreateInitializedParameter("@custgroup",DbType.String,custgroup),
                 objProvider.CreateInitializedParameter("@cust_state",DbType.String,custstate),
                 objProvider.CreateInitializedParameter("@cust_city",DbType.String,custcity),
                 objProvider.CreateInitializedParameter("@brlist",DbType.String,brlist),
                   objProvider.CreateInitializedParameter("@uom_id",DbType.String,uom_id),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$MIS_Get$SalesDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustomerList(string CompID, string SuppName, string BranchID, string CustType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$AllMIS$cust$detail_GetCustList", prmContentGetDetails);

                //if (PARQusData.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                //    {
                //        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["cust_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_name"].ToString());
                //    }
                //}
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
           
        }

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
        public DataSet BindProductNameInDDL(string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$MIS$ProductionAnalysisProductName", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet PaidAmountDetail(string compId, string brId, string InVNo, string InvDate, string Curr_id, string Fromdate, string Todate, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@InVNo",DbType.String, InVNo),
                                                        objProvider.CreateInitializedParameter("@InvDate",DbType.String, InvDate),
                                                        objProvider.CreateInitializedParameter("@Curr_id",DbType.String, Curr_id),
                                                         objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Sp_SLS$MIS$SlsDtl_GetPaidAmountDtl", prmContentGetDetails);
                return GetInvoiceDetailList;
                //return GetInvoiceDetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> ItemSetupHSNDAL(string CompID, string HSNName)
        {
            Dictionary<string, string> ddlcountryDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@HSN_code",DbType.String, HSNName),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$hsn_GetAllItemhsn_new", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlcountryDictionary.Add(PARQusData.Tables[0].Rows[i]["setup_id"].ToString(), PARQusData.Tables[0].Rows[i]["setup_val"].ToString());
                    }
                }
                return ddlcountryDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            //return null;
        }
        public DataSet GetCustCommonDropdownDAL(string CompID,string SearchVal,string Stateid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@StateName",DbType.String, SearchVal),
                    objProvider.CreateInitializedParameter("@cityName",DbType.String, SearchVal),
                    objProvider.CreateInitializedParameter("@state_id",DbType.String, Stateid),
                                                    };
                DataSet GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$cust$common$ddl", prmContentGetDetails);
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
