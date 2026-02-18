using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ProductionPlan;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionPlan;
using EnRepMobileWeb.UTILITIES;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.ProductionPlan
{
   public class ProductionPlan_SERVICES: ProductionPlan_ISERVICES
    {
        public String insertJCDetail(DataTable HeaderTable, DataTable jcitemdetails)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@ppl$jc$header",SqlDbType.Structured, HeaderTable),
                 objprovider.CreateInitializedParameterTableType("@ppl$jc$item",SqlDbType.Structured, jcitemdetails),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_ppl$jc$details_insert", prmcontentaddupdate).ToString();
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
        //public DataSet BindProductNameInDDL(string CompID, string BrID, string @ItmName)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
        //            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
        //            objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
        //            objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
        //                                             };
        //        DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$jc$Bind$prdname", prmContentGetDetails);
        //        return Getsuppport;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet BindFinancialYear(int CompID, int BrID, string StartDate, string Period, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@StartDate",DbType.String, StartDate),
                     objProvider.CreateInitializedParameter("@trtype",DbType.String, Period),
                     objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$mrp$get$fy", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindProductList(int CompID, int BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$bom$detail_GetProductList", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPlannedMaterialDetails(int CompID, int BrID,string ProductID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                     objProvider.CreateInitializedParameter("@productid",DbType.String, ProductID),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$bom$detail_GetMaterialDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindDateRangeCal(int CompID, int BrID, string start_year, string end_year, Int32 months)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@start_year",DbType.String, start_year),
                    objProvider.CreateInitializedParameter("@end_year",DbType.String, end_year),
                    objProvider.CreateInitializedParameter("@months",DbType.Int32, months),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$mrp$get$daterange", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetProductionDetailsData(int CompID, int BrID, string ProductionID, string hdn_FromDate
            , string hdn_ToDate, string PP_no,string PP_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, ProductionID),
                    objProvider.CreateInitializedParameter("@hdn_FromDate",DbType.String, hdn_FromDate),
                    objProvider.CreateInitializedParameter("@hdn_ToDate",DbType.String, hdn_ToDate),
                    objProvider.CreateInitializedParameter("@pp_no",DbType.String, PP_no),
                    objProvider.CreateInitializedParameter("@pp_dt",DbType.String, PP_dt),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$adv$detail_GetProductionDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertUpdatePPDetails(DataTable PPHeader, DataTable PPItemDetails, DataTable SOItemDetails, DataTable dtSubItem,DataTable dtSubItemProc,string req_area)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, PPHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, PPItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SOItemDetail",SqlDbType.Structured, SOItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                 objprovider.CreateInitializedParameterTableType("@SubItemProcDetail",SqlDbType.Structured, dtSubItemProc ),
                 //objprovider.CreateInitializedParameter("@req_area",DbType.Int32, req_area ),
                 objprovider.CreateInitializedParameter("@req_area",DbType.String, req_area ),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$pp$detail_InsertUpdatePP_Details", prmcontentaddupdate).ToString();

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
        public DataSet GetPPDetailByNo(string CompID, string PP_no, string BrchID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@pp_no",DbType.String, PP_no),
                    objProvider.CreateInitializedParameter("@UserID",DbType.Int32, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),

            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$pp$detail_GetPPDetailByNo", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPPList(string Source, string Fromdate, string Todate, string Status, string CompID, string BrchID,string wfstatus, string UserID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@Source",DbType.String, Source),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@UserID",DbType.Int32, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[ppl$pp$detail_GetPPDetailsList]", prmContentGetDetails);
            return dt;
        }
        public DataSet Delete_PPDetails(ProductionPlan_Model _ProductionPlan_Model, string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@pp_no",DbType.String, _ProductionPlan_Model.PPNumber),
                    objProvider.CreateInitializedParameter("@pp_dt",DbType.String,  _ProductionPlan_Model.PPDate),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$pp$detail_DeletePPDetails", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approved_PPDetails(string comp_id, string br_id, string user_id, string PP_no, 
            string PP_date, string DocumentMenuID, string mac_id, string A_Status, string A_Level, string A_Remarks
            , string AutoGen_Remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@pp_no",DbType.String, PP_no),
                    objProvider.CreateInitializedParameter("@pp_dt",DbType.String, PP_date),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, user_id),
                    objProvider.CreateInitializedParameter("@menuid",DbType.String, DocumentMenuID),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@ppstatus",DbType.String,""),
                    objProvider.CreateInitializedParameter("@wf_status",DbType.String,A_Status),
                    objProvider.CreateInitializedParameter("@wf_level",DbType.String,A_Level),
                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String,A_Remarks),
                    objProvider.CreateInitializedParameter("@AutoGen_Remarks",DbType.String,AutoGen_Remarks),
                };
                prmContentGetDetails[7].Size = 100;
                prmContentGetDetails[7].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$pp$detail_Approved_PPDetail", prmContentGetDetails).ToString();

                string msg = string.Empty;
                if (prmContentGetDetails[7].Value != DBNull.Value) // status
                {
                    msg = prmContentGetDetails[7].Value.ToString();
                }

                return msg;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public string Cancelled_PPDetail(string comp_id, string br_id, string pp_no, string pp_date, string userid, string mac_id, string MenuDocid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16,comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                    objProvider.CreateInitializedParameter("@pp_no",DbType.String,pp_no),
                    objProvider.CreateInitializedParameter("@pp_dt",DbType.String,pp_date),
                    objProvider.CreateInitializedParameter("@userid",DbType.String,userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@menudocid_id",DbType.String,MenuDocid),
                     };
                string ppno = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "ppl$pp$detail_Cancel_PP", prmContentGetDetails).ToString();
                return ppno; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet AddForeCastItemDetail(string CompID, string Br_ID, string F_Fy, string F_Period, string FromDate, string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16,CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String,Br_ID),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String,F_Fy),
                    objProvider.CreateInitializedParameter("@fperiod",DbType.String,F_Period),
                    objProvider.CreateInitializedParameter("@fromDate",DbType.String,FromDate),
                    objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                     };
                DataSet ppno = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "AddProductionPlanByForecast", prmContentGetDetails);
                return ppno; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet AddSOItemDetail(string Comp_ID, string Br_ID, string CustID, string OrderNo, string OrderDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16,Comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String,Br_ID),
                    objProvider.CreateInitializedParameter("@CustID",DbType.String,CustID),
                    objProvider.CreateInitializedParameter("@OrderNo",DbType.String,OrderNo),
                    objProvider.CreateInitializedParameter("@OrderDate",DbType.String,OrderDate),
                     };
                DataSet ppno = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "AddProductionPlanBySO", prmContentGetDetails);
                return ppno; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID, string CustType)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
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
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["cust_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_namewithCity"].ToString());
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

        }
        public Dictionary<string, string> GetOrderNumberList(string CompID, string BrchID, string CustID, string OrderNumber)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                    objProvider.CreateInitializedParameter("@OrderNumber",DbType.String, OrderNumber),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_OrderNumberListForPP", prmContentGetDetails);
                //DataRow dr;
                //dr = PARQusData.Tables[0].NewRow();
                //dr[0] = "0";
                //dr[1] = "---Select---";
                //PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["app_so_no"].ToString(), PARQusData.Tables[0].Rows[i]["so_dt"].ToString()+"_"+ PARQusData.Tables[0].Rows[i]["so_date"].ToString());
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

        }

        public DataSet PendingStockItemWise(string CompID, string BrID,string ItemId,string UomId, string StockType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
                    objProvider.CreateInitializedParameter("@StockType",DbType.String, StockType),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl_PendingStockItemWise", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetBOMDetailsItemWise(string CompID, string BrID,string ItemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$bom$detail_GetMaterialDetail", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet PP_GetSubItemDetails(string CompID, string Br_id, string ItemId, string pp_no, string pp_dt,string Flag)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@pp_no",DbType.String, pp_no),
                    objProvider.CreateInitializedParameter("@pp_dt",DbType.String, pp_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PP_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetRequirmentreaList(string CompId, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[prc$pr$GetRequirmentreaList]", prmContentGetDetails).Tables[0];
                return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckDocAgainstPP(string Comp_ID, string Br_ID, string DocNo, string DocDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, Comp_ID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, Br_ID),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                    objProvider.CreateInitializedParameter("@DocDate",DbType.String, DocDate),

                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "CheckDocAgainstPP", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckDocAgainstPR(string Comp_ID, string Br_ID, string DocNo, string DocDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, Comp_ID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, Br_ID),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                    objProvider.CreateInitializedParameter("@DocDate",DbType.String, DocDate),

                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "CheckDocAgainstPR", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemDetailsFromForecast(string CompID, string BrchID, string Item_id,string Fy, Int32 F_period)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@Fy",DbType.String, Fy),
                    objProvider.CreateInitializedParameter("@F_period",DbType.Int32, F_period),
                    //objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PP_GetSubItemDetailsFromForecast", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemDetailsFromSO(string CompID, string BrchID, string Item_id,string hdn_SOOrderNO, string Status,string pp_no)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@OrderNo",DbType.String, hdn_SOOrderNO),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@pp_no",DbType.String, pp_no),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PP_GetSubItemDetailsFromSO", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetProductionPlanPrintDeatils(string CompID, string BrchID, string pp_No, string PP_Date)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                    objProvider.CreateInitializedParameter("@pp_no",DbType.String, pp_No),
                    objProvider.CreateInitializedParameter("@pp_date",DbType.String, PP_Date)
                    //objProvider.CreateInitializedParameter("@F_period",DbType.Int32, F_period),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PP_GetProductionPlanPrintDeatils", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetPPTrackingDetails(string compId, string brId, string ppNo, string ppDate)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int32, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@PpNo",DbType.String, ppNo),
                    objProvider.CreateInitializedParameter("@PpDate",DbType.String, ppDate)
                    //objProvider.CreateInitializedParameter("@F_period",DbType.Int32, F_period),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetProdPlantrackingDetails", prmContentGetDetails);
                return Getsuppport.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
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
        public DataTable GetintimationPrintDeatils(string CompID, string BrchID, string pp_No, string PP_Date)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                    objProvider.CreateInitializedParameter("@pp_no",DbType.String, pp_No),
                    objProvider.CreateInitializedParameter("@pp_date",DbType.String, PP_Date)
                    //objProvider.CreateInitializedParameter("@F_period",DbType.Int32, F_period),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl_pp_GetIntimationDetail", prmContentGetDetails);
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
