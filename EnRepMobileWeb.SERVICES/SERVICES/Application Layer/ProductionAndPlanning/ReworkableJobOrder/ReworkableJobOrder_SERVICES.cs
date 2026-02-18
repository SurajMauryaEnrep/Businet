using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ReworkableJobOrder;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ReworkableJobOrder;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.ReworkableJobOrder
{
   public class ReworkableJobOrder_SERVICES: ReworkableJobOrder_ISERVICES
    {
        public DataSet AllDDLBind_OnPageLOad(string CompID, string BrID,string SearchName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, SearchName),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllDDLBind_OnPageLoad", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetWorkStationDAL(string CompID, string br_id, int shfl_id)
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
        public DataSet GetRewrkWHAvalStk(string CompID, string BrchID, string ItemID, string WarehouseID,string src_type,string accodian_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrchID),
                                                         objProvider.CreateInitializedParameter("@Item_id",DbType.String, ItemID),
                                                         objProvider.CreateInitializedParameter("@WarehouseID",DbType.String, WarehouseID),
                                                         objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                                                         objProvider.CreateInitializedParameter("@accodian_type",DbType.String, accodian_type),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$PPL$GetRewrkWHAvalStk", prmContentGetDetails);
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
        public DataSet GetNewBatchNo(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrchID),
                                                         //objProvider.CreateInitializedParameter("@Item_id",DbType.String, ItemID),
                                                         //objProvider.CreateInitializedParameter("@WarehouseID",DbType.String, WarehouseID),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$PPL$GetRewrkNewbatchNo", prmContentGetDetails);
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

        public DataSet GetMaterialNameByMtrlTyp(string CompID, string BrchID, string ddl_MaterialTyp)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrchID),
                                                         objProvider.CreateInitializedParameter("@MatrialTyp",DbType.String, ddl_MaterialTyp)
                                                        };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$PPL$GetMaterialNameByMtrlTyp", prmContentGetDetails);
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
        public DataSet GetItemUOM( string CompId, string br_id, string MaterialID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                       objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, MaterialID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_PPL$Rewrk_GetItemUOMDetails", prmContentGetDetails);
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
        
        public DataSet GetReworkQtyDetails(string CompID, string BrchID, string ItemID, string WHID,string src_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@Compid",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Brid",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemID),
                    objProvider.CreateInitializedParameter("@WarehouseId",DbType.String, WHID),
                    objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_PPL$Reworkdetail_GetItemStockforReworkQty", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemReworkQtyAfterInsert(string CompID, string BrID, string RJO_No, string RJO_Date, string ItemId, string WHID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@RJONo",DbType.String,  RJO_No),
            objProvider.CreateInitializedParameter("@RJODate",DbType.String,  RJO_Date),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            objProvider.CreateInitializedParameter("@WhId",DbType.String,  WHID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$GetDetailStockRewrkQtywiseAftrInsert",/*Done*/ prmContentGetDetails);
            return DS;
        }

        public string InsertReworkJO_Details(DataTable DtblHDetail, DataTable DtblReqMatrlDetail, DataTable ReworkQtyItemDetails, DataTable DTAttachmentDetail, DataTable DtblConsumeMatrlDetail, DataTable CMItemBatchDetails, DataTable CMItemSerialDetails, string hdnJobCmplted, DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DtblHDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ReqMtrlItemDetail",SqlDbType.Structured, DtblReqMatrlDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ReworkQtyDetail",SqlDbType.Structured, ReworkQtyItemDetails),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DTAttachmentDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ConsMtrlItemDetail",SqlDbType.Structured,DtblConsumeMatrlDetail),
                                                       objprovider.CreateInitializedParameterTableType("@CMBatchDetails",SqlDbType.Structured,CMItemBatchDetails),
                                                       objprovider.CreateInitializedParameterTableType("@CMSerialDetails",SqlDbType.Structured,CMItemSerialDetails),
                                                       objprovider.CreateInitializedParameter("@JobCmplted",DbType.String,hdnJobCmplted),
                                                        objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem ),

                                                    };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp$PPL$ReworkJO_InsertDetails",/*Done*/ prmcontentaddupdate).ToString();
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

            finally
            {
            }
        }

        public DataSet GetRJOListandSrchDetail(string CompId, string BrchID, RJOListModel _RJOListModel, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String,_RJOListModel.ItemID),
                                                       objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_RJOListModel.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_RJOListModel.ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _RJOListModel.Status),
                                                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                             objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$GetRewrkJOListandSrchDetail", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
       
        public DataSet GetRewrkJODetailEditUpdate(string CompId, string BrchID, string JobCard_NO, string JobCard_Date, string UserID, string DocID)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@RJONo",DbType.String, JobCard_NO),
                                                        objProvider.CreateInitializedParameter("@RJODate",DbType.String, JobCard_Date),
                                                        //objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp$PPL$GetRewrkJOEditUpdtDetails]",/*Done*/ prmContentGetDetails);
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
        public string RewrkJO_DeleteDetail(ReworkableJobOrder_Model _ReworkableJobOrder_Model, string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@RJONo",DbType.String,_ReworkableJobOrder_Model.ReworkJO_No),
                                                        objProvider.CreateInitializedParameter("@RJODate",DbType.String,_ReworkableJobOrder_Model.ReworkJO_Date),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure,"sp$PPL$RewrkJODeleteAllSectionDetails", prmContentInsert).ToString();
                return POId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string RewrkJOApproveDetails(string CompID, string BrchID, string RJO_No, string RJO_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                              objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                              objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                              objProvider.CreateInitializedParameter("@docno",DbType.String, RJO_No),
                                                              objProvider.CreateInitializedParameter("@docdate",DbType.String, RJO_Date),
                                                              objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                              objProvider.CreateInitializedParameter("@DocMenuId",DbType.String, MenuID),
                                                              objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                              objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                              objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                              objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),

                };


                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp$PPL$RewrkJOApproveDetails", prmContentGetDetails).ToString();
                return POId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet RewrkJOCancel(ReworkableJobOrder_Model _ReworkableJobOrder_Mode, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@RJO_no",DbType.String,  _ReworkableJobOrder_Mode.ReworkJO_No),
                    objProvider.CreateInitializedParameter("@RJO_dt",DbType.Date,  _ReworkableJobOrder_Mode.ReworkJO_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _ReworkableJobOrder_Mode.Created_by ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_PPL$RewrkJOCancel", prmContentGetDetails);

            return DS;
        }
        public DataSet ChkRJOagainstMRS(string CompID, string BrID, string RJONo, string RJODate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@RJO_no",DbType.String, RJONo),
                    objProvider.CreateInitializedParameter("@RJO_dt",DbType.String, RJODate),

                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$RewrkJO_ChckRJODetailAgainstMRS", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetDetailsOfRequiredMaterialTbl(string Comp_ID, string Br_ID, string RJO_No, string RJO_Date, string ShopfloorId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.Int32, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.Int32, Br_ID),
                                                         objProvider.CreateInitializedParameter("@RJO_No",DbType.String, RJO_No),
                                                         objProvider.CreateInitializedParameter("@RJO_Date",DbType.String, RJO_Date),
                                                         objProvider.CreateInitializedParameter("@ShfloorId",DbType.String, ShopfloorId),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$GetDetailsOfRequiredMaterial", prmContentGetDetails);
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
        public DataSet getCMItemStockBatchSerialWise(string ItemId, string ShpfloorId, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
                objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@ShpfloorId",DbType.Int32, ShpfloorId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp$PPL$RewrkJO_GetCnsumMatrialStockBatchSerialwise]", prmContentGetDetails);
            return DS;
        }
        public DataSet getCMStockBatchWiseAfterInsert(string CompID, string BrID, string RJO_No, string RJO_Date, string MtrlTypId, string ItemId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@RJONo",DbType.String,  RJO_No),
            objProvider.CreateInitializedParameter("@RJODate",DbType.String,  RJO_Date),
            objProvider.CreateInitializedParameter("@MatrlType",DbType.String,  MtrlTypId),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$RewrkJO_GetCMItemStockBatchwiseAfterInsert", prmContentGetDetails);
            return DS;
        }
        public DataSet getCMStockSerialWiseAfterInsert(string CompID, string BrID, string RJO_No, string RJO_Date, string MtrlTypId, string ItemId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@RJONo",DbType.String,  RJO_No),
            objProvider.CreateInitializedParameter("@RJODate",DbType.String,  RJO_Date),
            objProvider.CreateInitializedParameter("@MatrlType",DbType.String,  MtrlTypId),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$RewrkJO_GetCMItemStockSerialwiseAfterInsert", prmContentGetDetails);
            return DS;
        }
        public DataSet RJO_GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag, string Shfl_Id)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                    objProvider.CreateInitializedParameter("@Shfl_Id",DbType.String, Shfl_Id),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "RJO_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet QCAcptRejRewkQty_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "RJO_GetSubItemDetailsAfterQC", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCMSubItemShflAvlstockDetails(string comp_ID, string br_ID, string doc_no, string doc_dt, string Shfl_Id, string item_id, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                     objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Shfl_id",DbType.Int32, Shfl_Id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "RJO_GetCMSubItemAvlStockDetailsBYShopfloor", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataSet RJO_GetCMSubItemDetailsAfterSave(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
        //{
        //    try
        //    {

        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
        //            objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
        //            objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
        //            objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
        //            objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
        //            objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
        //            objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
        //                                             };
        //        DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "RJO_GetSubItemDetailsAfterApprove", prmContentGetDetails);
        //        return Getsuppport;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet ItemList(string GroupName, string CompID, string BrchID, string ddl_MaterialTyp, string ddl_HedrItemId)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@MatrialTyp",DbType.String, ddl_MaterialTyp),
                    objProvider.CreateInitializedParameter("@ddl_HedrItemId",DbType.String, ddl_HedrItemId),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$PPL$GetMaterialNameByMtrlTyp$RWK", prmContentGetDetails);
                //DataRow dr;
                //dr = PARQusData.Tables[0].NewRow();
                //dr[0] = "0";
                //dr[1] = "---Select---";
                //PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                //if (PARQusData.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                //    {
                //        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["Item_id"].ToString(), PARQusData.Tables[0].Rows[i]["Item_name"].ToString());
                //    }
                //}
                //return ddlItemNameDictionary;
                return PARQusData;
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
        public DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id, string UomId, string flag1,string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.Int32, wh_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
                    objProvider.CreateInitializedParameter("@flag1",DbType.String, flag1),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "RWK$wh$shflGetSubItemAvlStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
