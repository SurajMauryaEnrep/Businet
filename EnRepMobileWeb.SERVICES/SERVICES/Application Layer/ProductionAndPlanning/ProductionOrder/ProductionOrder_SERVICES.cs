using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionOrder;
using EnRepMobileWeb.UTILITIES;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.ProductionOrder
{
    public class ProductionOrder_SERVICES : ProductionOrder_ISERVICES
    {
        public String insertJCDetail(DataTable HeaderTable, DataTable jcitemdetails, DataTable PrdOrdrAttachments
            , DataTable dtSubItem, string A_Status, string A_Level, string A_Remarks,DataTable ProductionSch)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@ppl$jc$header",SqlDbType.Structured, HeaderTable),
                 objprovider.CreateInitializedParameterTableType("@ppl$jc$item",SqlDbType.Structured, jcitemdetails),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, PrdOrdrAttachments ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                 objprovider.CreateInitializedParameterTableType("@ProductionSchedule",SqlDbType.Structured, ProductionSch ),
                  objprovider.CreateInitializedParameter("@wf_status",DbType.String,A_Status),
                    objprovider.CreateInitializedParameter("@wf_level",DbType.String,A_Level),
                    objprovider.CreateInitializedParameter("@wf_remarks",DbType.String,A_Remarks),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_ppl$jc$details_insert", prmcontentaddupdate).ToString();
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
        
        public DataSet BindProductNameInDDL(string CompID, string BrID, string @ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$jc$Bind$prdname", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSOItemUOMDL(string Item_id, string CompId, int br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, Item_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                 objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),     };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetJCProduct_Details", prmContentGetDetails);
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
        public DataSet BindRevnoEdit(int CompId, int br_id, string Item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                                                 objProvider.CreateInitializedParameter("@ItemID",DbType.String, Item_id),     };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$jc$bind$revno$edit", prmContentGetDetails);
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
        public DataSet BindOPNameBaseOnRevNo(int CompId, int br_id, string Item_id, int rev_no, string ProductionOrderNumber, string Jc_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                                                 objProvider.CreateInitializedParameter("@ItemID",DbType.String, Item_id),
                 objProvider.CreateInitializedParameter("@rev_no",DbType.Int64, rev_no),
                objProvider.CreateInitializedParameter("@jc_no",DbType.String, ProductionOrderNumber),
                objProvider.CreateInitializedParameter("@jc_dt",DbType.String, Jc_Date),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$jc$BindOPNameBaseOnRevNo", prmContentGetDetails);
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
        public DataSet GetAllData(int CompID, int br_id,string UserID, string wfstatus, string DocumentMenuId,
             string txtFromdate, string txtToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                     objProvider.CreateInitializedParameter("@UserID",DbType.Int32, UserID),
                     objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                     objProvider.CreateInitializedParameter("@Fromdate",DbType.String, txtFromdate),
                     objProvider.CreateInitializedParameter("@Todate",DbType.String, txtToDate),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ppl$jc$GetAllData", prmContentGetDetails);
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

        public DataSet BindOperationNameInListPage(int CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),    };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$jc$bindop", prmContentGetDetails);
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
        public DataTable GetShopFloorDetailsDAL(int CompID, int br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),                               };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_pp$jc$BindShfl", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetWorkStationDAL(int CompID, int br_id, int shfl_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                objProvider.CreateInitializedParameter("@shfl_id",DbType.Int32, shfl_id),};
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_pp$jc$BindWorkStation", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Bind_Plus_AddAttribute(int comp_id, int br_id, string advice_no, string advice_dt, int op_id,string Shflid, string Itemid,string ProdQty,string Product_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                objProvider.CreateInitializedParameter("@advice_no",DbType.String, advice_no),
                //objProvider.CreateInitializedParameter("@rev_no",DbType.Int32, rev_no),
                objProvider.CreateInitializedParameter("@advice_dt",DbType.String, advice_dt),
                objProvider.CreateInitializedParameter("@op_id",DbType.Int64, op_id),
                objProvider.CreateInitializedParameter("@shfl_id",DbType.Int64, Shflid),
                objProvider.CreateInitializedParameter("@itemid",DbType.String, Itemid),
                objProvider.CreateInitializedParameter("@ProdQty",DbType.String, ProdQty),
                 objProvider.CreateInitializedParameter("@Product_id",DbType.String, Product_id),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$jc$Plus$AddAttribute", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet Get_ItemDetailsList(int comp_id, int br_id, string productid,string advice_no, string advice_dt, int op_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                     objProvider.CreateInitializedParameter("@productid",DbType.String, productid),
                objProvider.CreateInitializedParameter("@advice_no",DbType.String, advice_no),
                objProvider.CreateInitializedParameter("@advice_dt",DbType.String, advice_dt),
                objProvider.CreateInitializedParameter("@op_id",DbType.Int64, op_id),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$bom$item$detail_GetItemDetailsList", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetJCList(Int32 CompID, Int32 BrID,string UserID,string wfstatus,string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                objProvider.CreateInitializedParameter("@UserID",DbType.Int32, UserID),
                objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$jc$list", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataSet GetJCSearch(string CompID, string BrID, string ddl_shfl_id, string ddl_Product_id, string ddl_op_id, string txtFromdate, string txtToDate, string Status)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //    SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
        //    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
        //    objProvider.CreateInitializedParameter("@shfl_id",DbType.String, ddl_shfl_id),
        //    objProvider.CreateInitializedParameter("@Product_id",DbType.String,ddl_Product_id),
        //    objProvider.CreateInitializedParameter("@op_id",DbType.String, ddl_op_id),
        //    objProvider.CreateInitializedParameter("@txtFromdate",DbType.String, txtFromdate),
        //    objProvider.CreateInitializedParameter("@txtToDate",DbType.String, txtToDate),
        //     objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
        //    };
        //    DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$jc$searchlist", prmContentGetDetails)
        //    return ds;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet GetJCSearch(string CompID, string BrID, string ddl_shfl_id, string ddl_Product_id, string ddl_op_id,string ddl_shift_id, string ddl_workst_id,string txtFromdate, string txtToDate, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
            objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@shfl_id",DbType.String, ddl_shfl_id),
            objProvider.CreateInitializedParameter("@Product_id",DbType.String,ddl_Product_id),
            objProvider.CreateInitializedParameter("@op_id",DbType.String, ddl_op_id),
             objProvider.CreateInitializedParameter("@shift_id",DbType.String, ddl_shift_id),
               objProvider.CreateInitializedParameter("@ws_id",DbType.String, ddl_workst_id),
            objProvider.CreateInitializedParameter("@txtFromdate",DbType.String, txtFromdate),
            objProvider.CreateInitializedParameter("@txtToDate",DbType.String, txtToDate),
             objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
            };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$jc$searchlist", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BinddbClick(Int32 CompID, Int32 BrID, string jc_no, string jc_dt,string UserID ,string DocumentMenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@jc_no",DbType.String, jc_no),
                    objProvider.CreateInitializedParameter("@jc_dt",DbType.String, jc_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuID),
                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$jc$dbclick", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckPCagainstPrOrdr(string CompID, string BrID, string jc_no, string jc_dt, string prdctOrdtype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                      objProvider.CreateInitializedParameter("@jc_no",DbType.String, jc_no),
                    objProvider.CreateInitializedParameter("@jc_dt",DbType.String, jc_dt),
                    objProvider.CreateInitializedParameter("@prdctOrdType",DbType.String, prdctOrdtype),
                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "jc$detail_CheckPrdCnfAgainstPrdOrdr", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
      
        public DataSet GetAdviceDetails(string CompId, string BrID, string AdviceNo, string AdviceDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@AdviceNo",DbType.String, AdviceNo),
                    objProvider.CreateInitializedParameter("@AdviceDate",DbType.String, AdviceDate),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ppl$prd$adv$detail_GetAdviceDetails", prmContentGetDetails);
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
        public DataSet GetConfirmationDetail(string CompID, string BrchID, string ItemID, string JCNumber, string JCDate)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                      objProvider.CreateInitializedParameter("@JCNumber",DbType.String, JCNumber),
                      objProvider.CreateInitializedParameter("@JCDate",DbType.String, JCDate),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetProdOrd_Input_Output_Detail]", prmContentGetDetails);
            return ds;
        }
        public DataSet JC_GetSubItemDetails(string CompID, string br_id, string Item_id, string jc_no, string jc_dt,string Flag)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@jc_no",DbType.String, jc_no),
                      objProvider.CreateInitializedParameter("@jc_dt",DbType.String, jc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[JC_GetSubItemDetailsAfterApprove]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetProductionOrderPrintDeatils(string CompID, string br_id, string jc_no, string jc_dt)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                      objProvider.CreateInitializedParameter("@jc_no",DbType.String, jc_no),
                      objProvider.CreateInitializedParameter("@jc_dt",DbType.String, jc_dt),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetProductionOrderPrintDetails]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetAlternateItemDetails(string compID, string br_id, string product_Id, string op_Id
            , string item_Id,string shfl_id,string alt_item_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, compID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@product_Id",DbType.String, product_Id),
                     objProvider.CreateInitializedParameter("@op_Id",DbType.String, op_Id),
                     objProvider.CreateInitializedParameter("@shfl_id",DbType.String, shfl_id),
                     objProvider.CreateInitializedParameter("@item_Id",DbType.String, item_Id),
                     objProvider.CreateInitializedParameter("@alt_item_id",DbType.String, alt_item_id),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetProductionOrderAlternateItemDetails]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetSubItemWhAvlSHOPstockDetails(string comp_ID, string br_ID, string item_id, string UomId, 
            string flag1, string Doc_no, string Doc_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),                  
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),                
                    objProvider.CreateInitializedParameter("@flag1",DbType.String, flag1),                 
                    objProvider.CreateInitializedParameter("@Doc_no",DbType.String, Doc_no),                 
                    objProvider.CreateInitializedParameter("@Doc_dt",DbType.String, Doc_dt),                 
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSubItem$AvlStock$ware$shop$prod$order$Details", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
