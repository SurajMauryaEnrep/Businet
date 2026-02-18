using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MaterialRequirementPlan;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MaterialRequirementPlan;
using EnRepMobileWeb.UTILITIES;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.MaterialRequirementPlan
{
   public class MaterialRequirementPlan_SERVICES: MaterialRequirementPlan_ISERVICES
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
        public DataSet GetPlannedMaterialDetails(int CompID, int BrID,string ProductID,string plannedqty)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                     objProvider.CreateInitializedParameter("@productid",DbType.String, ProductID),
                     objProvider.CreateInitializedParameter("@plannedqty",DbType.String, plannedqty),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$bom$detail_GetMaterialDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetRMPendingOrderQuantityDetails(int CompID, int BrID, string ProductID,string UomId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                     objProvider.CreateInitializedParameter("@Itemid",DbType.String, ProductID),
                     objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get_MRP_RM_PendingOrderQuantityDetails]", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet ProductMaterialDetails(string comp_id, string br_id, string product_id, string qty)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                     objProvider.CreateInitializedParameter("@product_id",DbType.String, product_id),
                     objProvider.CreateInitializedParameter("@qty",DbType.String, qty),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[ppl$mrp$detail_GetProductMaterialDetail]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSfAndRmDataByProductList(string comp_ID, string br_ID, DataTable productListDt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                     objProvider.CreateInitializedParameterTableType("@tbl_bom_tree",SqlDbType.Structured, productListDt),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[AddSfAndRmDetailsByProductList]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPlannedRowMaterialDetails(int CompID, int BrID,string ProductID,string SF_Item_id,string Req_Qty)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                     objProvider.CreateInitializedParameter("@productid",DbType.String, ProductID),
                     objProvider.CreateInitializedParameter("@SF_Item_id",DbType.String, SF_Item_id),
                     objProvider.CreateInitializedParameter("@Req_Qty",DbType.String, Req_Qty),
                     objProvider.CreateInitializedParameter("@Flag",DbType.String, "RMbySF"),
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
        public string InsertUpdateMRPDetails(DataTable MRPHeader, DataTable MRPItemDetails, DataTable MRPInputMaterialDetails,
            DataTable SFMaterialDetails,DataTable HdnSFmaterial,DataTable HdnRMdetails,DataTable dtSubItem/*,DataTable dtSFSubItem*/
            ,DataTable dtSubItem_precure)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, MRPHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, MRPItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@InputMaterialDetail",SqlDbType.Structured, MRPInputMaterialDetails ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SFMaterialDetail",SqlDbType.Structured, SFMaterialDetails ),
                 objprovider.CreateInitializedParameterTableType("@HdnSFMaterialDetail",SqlDbType.Structured, HdnSFmaterial ),
                 objprovider.CreateInitializedParameterTableType("@HdnRMDetail",SqlDbType.Structured, HdnRMdetails ),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail_prec",SqlDbType.Structured, dtSubItem_precure ),
                 //objprovider.CreateInitializedParameterTableType("@SFSubItemDetail",SqlDbType.Structured, dtSFSubItem ),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$mrp$detail_InsertUpdateMRP_Details", prmcontentaddupdate).ToString();

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
        public DataSet GetMRPDetailByNo(string CompID, string mrp_no, string BrchID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@mrp_no",DbType.String, mrp_no),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$mrp$detail_GetDetailByBNo", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetMRPList(string Source, string Fromdate, string Todate, string Status, string CompID, string BrchID,string wfstatus,string UserID,string DocumentMenuId,string Req_area)
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
                 objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                  objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                  objProvider.CreateInitializedParameter("@req_area_id",DbType.String, Req_area),
                                                     };
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[ppl$mrp$detail_GetMRPDetailsList]", prmContentGetDetails).Tables[0];
            return dt;
        }
        public DataSet Delete_MRPDetails(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model, string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@mrp_no",DbType.String, _MaterialRequirementPlan_Model.MRPNumber),
                    objProvider.CreateInitializedParameter("@mrp_dt",DbType.String,  _MaterialRequirementPlan_Model.MRPDate),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$mrp$rm$detail_DeleteMRPDetails", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approved_MRPDetails(DataTable MRPHeader, DataTable MRPItemDetails, DataTable MRPInputMaterialDetails
            , DataTable SFMaterialDetails,DataTable dtSubItem, string A_Status, string A_Level, string A_Remarks,DataTable dtSubItem_precu)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                      objProvider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, MRPHeader ),
                 objProvider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, MRPItemDetails ),
                 objProvider.CreateInitializedParameterTableType("@InputMaterialDetail",SqlDbType.Structured, MRPInputMaterialDetails ),
                 objProvider.CreateInitializedParameterTableType("@SFMaterialDetail",SqlDbType.Structured, SFMaterialDetails ),
             
                 //objProvider.CreateInitializedParameterTableType("@HdnSFMaterialDetail",SqlDbType.Structured, HdnSFmaterial ),
                 //objProvider.CreateInitializedParameterTableType("@HdnRMDetail",SqlDbType.Structured, HdnRMdetails ),
                 
                    objProvider.CreateInitializedParameter("@wf_status",DbType.String,A_Status),
                    objProvider.CreateInitializedParameter("@wf_level",DbType.String,A_Level),
                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String,A_Remarks),
                    objProvider.CreateInitializedParameter("@mrpstatus",DbType.String,""),
                    objProvider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                        objProvider.CreateInitializedParameterTableType("@SubItemDetail_prec",SqlDbType.Structured, dtSubItem_precu ),
                };
                prmContentGetDetails[7].Size = 100;
                prmContentGetDetails[7].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$mrp$detail_Approved_MRPDetail", prmContentGetDetails).ToString();

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
        //public string Approved_MRPDetails(string comp_id, string br_id, string user_id, string mrp_no, string mrp_date, string DocumentMenuID, string mac_id, string A_Status, string A_Level, string A_Remarks)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //            objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
        //            objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
        //            objProvider.CreateInitializedParameter("@mrp_no",DbType.String, mrp_no),
        //            objProvider.CreateInitializedParameter("@mrp_dt",DbType.String, mrp_date),
        //            objProvider.CreateInitializedParameter("@userid",DbType.String, user_id),
        //            objProvider.CreateInitializedParameter("@menuid",DbType.String, DocumentMenuID),
        //            objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
        //            objProvider.CreateInitializedParameter("@mrpstatus",DbType.String,""),
        //            objProvider.CreateInitializedParameter("@wf_status",DbType.String,A_Status),
        //            objProvider.CreateInitializedParameter("@wf_level",DbType.String,A_Level),
        //            objProvider.CreateInitializedParameter("@wf_remarks",DbType.String,A_Remarks),

        //        };
        //        prmContentGetDetails[7].Size = 100;
        //        prmContentGetDetails[7].Direction = ParameterDirection.Output;

        //        string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "ppl$mrp$detail_Approved_MRPDetail", prmContentGetDetails).ToString();

        //        string msg = string.Empty;
        //        if (prmContentGetDetails[7].Value != DBNull.Value) // status
        //        {
        //            msg = prmContentGetDetails[7].Value.ToString();
        //        }

        //        return msg;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }

        //}
        public string Cancelled_MRPDetail(string comp_id, string br_id, string mrp_no, string mrp_date, string userid,  string mac_id, string MenuDocid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16,comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                    objProvider.CreateInitializedParameter("@mrp_no",DbType.String,mrp_no),
                    objProvider.CreateInitializedParameter("@mrp_dt",DbType.String,mrp_date),
                    objProvider.CreateInitializedParameter("@userid",DbType.String,userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@menudocid_id",DbType.String,MenuDocid),
                     };
                string mrpno = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "ppl$mrp$detail_Cancel_MRP", prmContentGetDetails).ToString();
                return mrpno; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet AddPRoductionPlanItemDetail(string Comp_ID, string Br_ID, string F_Fy, string F_Period
            , string FromDate, string ToDate, string P_Number, string P_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, Comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                    objProvider.CreateInitializedParameter("@f_fy",DbType.String, F_Fy),
                    objProvider.CreateInitializedParameter("@fperiod",DbType.String,  F_Period),
                    objProvider.CreateInitializedParameter("@fromDate",DbType.String,  FromDate),
                    objProvider.CreateInitializedParameter("@ToDate",DbType.String,  ToDate),
                    objProvider.CreateInitializedParameter("@PP_Number",DbType.String,  P_Number),
                    objProvider.CreateInitializedParameter("@PP_Date",DbType.String,  P_Date),
                    //objProvider.CreateInitializedParameter("@Flag",DbType.String,  Flag),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "AddMRPByProductionPlan", prmContentGetDetails);
                return ds;
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
            finally
            {
            }
        }

        public DataSet CheckPRAgainstMRP(string CompId, string BrchID, string DocNo, string DocDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, DocDate),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "CheckPRAgainstMRP", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GetPPNumberList(string CompID, string BrchID, string PPNumber,string RequisitionArea)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@PPNumber",DbType.String, PPNumber),
                    objProvider.CreateInitializedParameter("@ReqArea",DbType.String, RequisitionArea),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_PPNumberListForMRP", prmContentGetDetails);
                
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["pp_no"].ToString(), 
                            PARQusData.Tables[0].Rows[i]["pp_dt"].ToString() + "|" + PARQusData.Tables[0].Rows[i]["pp_date"].ToString() + "|" + PARQusData.Tables[0].Rows[i]["setup_val"].ToString());
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
        public DataSet GetPPNumberDetail(string CompID, string BrchID, string PP_Number, string PP_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@PPNo",DbType.String, PP_Number),
                                                        objProvider.CreateInitializedParameter("@PPDate",DbType.String, PP_Date),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPPNo_Detail", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSFBOMDetailsItemWise(string CompID, string BrID, string ItemId,string SFItemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                    objProvider.CreateInitializedParameter("@sf_ItemId",DbType.String, SFItemId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$bom$detail_GetMaterialDetail", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetProducedQuantityDetail(string Comp_ID,string Br_ID,string mrp_no,string mrp_dt,string product_Id,string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, Comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_ID),
                    objProvider.CreateInitializedParameter("@mrp_no",DbType.String, mrp_no),
                    objProvider.CreateInitializedParameter("@mrp_dt",DbType.String, mrp_dt),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, product_Id),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "mrp_GetProducedQuantityDetail", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetProcuredQuantityDetail(string Comp_ID,string Br_ID,string mrp_no,string mrp_dt,string product_Id
            ,string Flag,string UomId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, Comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_ID),
                    objProvider.CreateInitializedParameter("@mrp_no",DbType.String, mrp_no),
                    objProvider.CreateInitializedParameter("@mrp_dt",DbType.String, mrp_dt),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, product_Id),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, Flag),
                    objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get_ProcuredQtyDetailsFromMrp", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet MRP_GetSubItemDetails(string CompID,string br_id, string Item_id, string mrp_no, string mrp_dt, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@mrp_no",DbType.String, mrp_no),
                    objProvider.CreateInitializedParameter("@mrp_dt",DbType.String, mrp_dt),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MRP_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataSet MRP_SFGetSubItemDetails(string CompID, string br_id, string Item_id, string mrp_no, string mrp_dt, string Flag)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
        //            objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
        //            objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
        //            objProvider.CreateInitializedParameter("@item_id",DbType.String, Item_id),
        //            objProvider.CreateInitializedParameter("@mrp_no",DbType.String, mrp_no),
        //            objProvider.CreateInitializedParameter("@mrp_dt",DbType.String, mrp_dt),
        //            objProvider.CreateInitializedParameter("@flag",DbType.String, Flag),
        //                                             };
        //        DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MRP_GetSubItemDetailsAfterApprove", prmContentGetDetails);
        //        return Getsuppport;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}


        public DataSet GetMRPPrintDeatils(string CompID, string br_id, string mrp_no, string mrp_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                    objProvider.CreateInitializedParameter("@mrp_no",DbType.String, mrp_no),
                    objProvider.CreateInitializedParameter("@mrp_dt",DbType.String, mrp_dt),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMRPPrintDeatils", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
