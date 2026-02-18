using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionAdvice;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.ProductionAdvice
{
    public class ProductionAdvice_Service : ProductionAdvice_IService
    {
        public DataTable Bind_ProductList1(string CompID, string BrID, string SrcType, string ffy, string Period, string SearchName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@srctype",DbType.String, SrcType),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String, ffy),
                    objProvider.CreateInitializedParameter("@period",DbType.String, Period),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, SearchName),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "BOMAndPplan_GetProductList_forProAdvice", prmContentGetDetails).Tables[0];
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
        public DataSet Bind_ProductList(string CompID, string BrID,string SrcType, string ffy, string Period, string SearchName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@srctype",DbType.String, SrcType),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String, ffy),
                    objProvider.CreateInitializedParameter("@period",DbType.String, Period),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, SearchName),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "BOMAndPplan_GetProductList_forProAdvice", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindFinancialYear(string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetFyList", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Bind_PeriodList(string CompID, string BrID, string ffy)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String, ffy),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$pp$detail_GetPeriodList", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Bind_PeriodRAndProductList(string CompID, string BrID, string ffy,string Period)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String, ffy),
                    objProvider.CreateInitializedParameter("@period",DbType.String, Period),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$pp$detail_GetPeriodRAndProductList", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Bind_RevisionNoList(string CompID, string BrID, string productid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, productid),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$bom$detail_GetRevisionNoList", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Get_MaterialDetail(string CompID, string BrID, string productid, string revno)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, productid),
                    objProvider.CreateInitializedParameter("@rev_no",DbType.String, revno),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$bom$detail_GetMaterialDetail", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertUpdateProductionAdvice_Details(DataTable PAdviceHeader, DataTable PAdviceItemDetails
            , DataTable ProdAdvAttachments,DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, PAdviceHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, PAdviceItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, ProdAdvAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$prd$adv$detail_InsertUpdateProductionAdvice_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }

        }
        public string Cancelled_ProductionAdviceDetail(string comp_id, string br_id, string adv_no, string adv_date, string userid, string mac_id, string MenuDocid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16,comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                    objProvider.CreateInitializedParameter("@adv_no",DbType.String,adv_no),
                    objProvider.CreateInitializedParameter("@adv_dt",DbType.String,adv_date),
                    objProvider.CreateInitializedParameter("@userid",DbType.String,userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@menudocid_id",DbType.String,MenuDocid),
                     };
                string mrpno = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "ppl$prd$adv$detail_Cancel_ProductionAdvice", prmContentGetDetails).ToString();
                return mrpno; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAdviceDetailByNo(string CompID, string BrchID, string Adv_no, string Adv_Date, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@adv_no",DbType.String, Adv_no),
                    objProvider.CreateInitializedParameter("@adv_dt",DbType.String, Adv_Date),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$adv$detail_GetAdviceDetailByBNo", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetAdviceList(string ProductId, string Source, string Fromdate, string Todate, string Status, string CompID, string BrchID, string UserID, string DocumentMenuId,string wfstatus)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@ProductId",DbType.String, ProductId),
                objProvider.CreateInitializedParameter("@Source",DbType.String, Source),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@status",DbType.String, Status),
                //objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                     };
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$adv$detail_GetAdviceDetailsList", prmContentGetDetails).Tables[0];
            return dt;
        }
        public DataSet Delete_PAdviceDetails(string comp_id, string br_id, string adviceno, string advicedt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@advice_no",DbType.String, adviceno),
                    objProvider.CreateInitializedParameter("@advice_dt",DbType.String,advicedt),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$adv$detail_DeleteAdviceDetails", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approved_PAdviceDetails(string comp_id, string br_id, string user_id, string adviceno, string advicedt, string DocumentMenuID, string mac_id,string A_Status,string A_Level,string A_Remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@advice_no",DbType.String, adviceno),
                    objProvider.CreateInitializedParameter("@advice_dt",DbType.String, advicedt),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, user_id),
                    objProvider.CreateInitializedParameter("@menuid",DbType.String, DocumentMenuID),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               
                    objProvider.CreateInitializedParameter("@mrpstatus",DbType.String,""),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String,A_Status),
                    objProvider.CreateInitializedParameter("@wf_level",DbType.String,A_Level),
                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String,A_Remarks),

                };
                prmContentGetDetails[7].Size = 100;
                prmContentGetDetails[7].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$prd$adv$detail_Approved_AdviceDetail", prmContentGetDetails).ToString();

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
        public DataSet GetProdAdv_Detail(string CompId, string BrID, string AdvNo, string AdvDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@AdvNo",DbType.String, AdvNo),
                                                        objProvider.CreateInitializedParameter("@AdvDate",DbType.String, AdvDate),
                                                      };
                DataSet AdvData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$adv$detail_GetProdAdv_RelDetails", prmContentGetDetails);
                return AdvData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet ADV_GetSubItemDetails(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@adv_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@adv_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[ADV_GetSubItemDetailsAfterApprove]", prmContentGetDetails);
            return ds;
        }



    }
}
