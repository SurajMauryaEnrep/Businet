using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.PackagingJobOrder;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.PackagingJobOrder;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.PackagingJobOrder
{
   public class PackagingJobOrder_SERVICES: PackagingJobOrder_ISERVICES
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllDDLBind_OnPageLoad",/*Done*/ prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllDataDeatil(string CompID, string BrID,string SearchName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, SearchName),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllDDLBind_OnPageLoad$pkg",/*Done*/ prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }  
        public DataSet GetAllData(string CompID, string BrID, PJOListModel _PJOListModel, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrID),
                     objProvider.CreateInitializedParameter("@ItemId",DbType.String,_PJOListModel.ItemID),
                                                       objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_PJOListModel.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_PJOListModel.ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _PJOListModel.Status),
                                                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                             objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$PckingJobOrder",/*Done*/ prmContentGetDetails);
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_pp$jc$BindWorkStation", /*Done*/prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetRewrkWHAvalStk(string CompID, string BrchID, string ItemID, string WarehouseID,string src_type)
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

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$PPL$GetPkgJobOrderWHAvalStk", prmContentGetDetails);
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$PPL$GetRewrkNewbatchNo ",/*Done**/ prmContentGetDetails);
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$PPL$GetMaterialNameByMtrlTyp",/*Done*/ prmContentGetDetails);
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_PPL$Rewrk_GetItemUOMDetails" /*Done*/, prmContentGetDetails);
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
               // DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_PPL$pkg$detail_GetItemStockforPkgQty",/*Done*/ prmContentGetDetails);
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_PPL$pkg$detail_GetItem$shfl$warehouse",/*Done*/ prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemReworkQtyAfterInsert(string CompID, string BrID, string PJO_No, string PJO_Date, string ItemId, string WHID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@PJONo",DbType.String,  PJO_No),
            objProvider.CreateInitializedParameter("@PJODate",DbType.String,  PJO_Date),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            objProvider.CreateInitializedParameter("@WhId",DbType.String,  WHID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$GetDetailStock$packingQtywiseAftrInsert",/*Done*/ prmContentGetDetails);
            return DS;
        }
        public DataSet getItempkgQtyAfterInsert(string CompID, string BrID, string PJO_No, string PJO_Date, string ItemId, string shfl_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@PJONo",DbType.String,  PJO_No),
            objProvider.CreateInitializedParameter("@PJODate",DbType.String,  PJO_Date),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            objProvider.CreateInitializedParameter("@shfl_id",DbType.String,  shfl_id),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$GetDetailStock$packingQtywiseAftrInsert$shfl",/*Done*/ prmContentGetDetails);
            return DS;
        }

        public string InsertPackingJO_Details(DataTable DtblHDetail, DataTable DtblReqMatrlDetail, DataTable PackingQtyItemDetails, DataTable DTAttachmentDetail, DataTable DtblConsumeMatrlDetail, DataTable CMItemBatchDetails, DataTable CMItemSerialDetails, string hdnJobCmplted, DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DtblHDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ReqMtrlItemDetail",SqlDbType.Structured, DtblReqMatrlDetail),
                                                        objprovider.CreateInitializedParameterTableType("@packingQtyDetail",SqlDbType.Structured, PackingQtyItemDetails),
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
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp$PPL$PackingJO_InsertDetails",/*Done*/ prmcontentaddupdate).ToString();
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

        public DataSet GetRJOListandSrchDetail(string CompId, string BrchID, PJOListModel _PJOListModel, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String,_PJOListModel.ItemID),
                                                       objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_PJOListModel.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_PJOListModel.ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _PJOListModel.Status),
                                                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                             objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$Get$PJOListandSrchDetail",/*Done*/ prmContentGetDetails);
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
                                                        objProvider.CreateInitializedParameter("@PJONo",DbType.String, JobCard_NO),
                                                        objProvider.CreateInitializedParameter("@PJODate",DbType.String, JobCard_Date),
                                                        //objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp$PPL$GetPackingJOEditUpdtDetails]",/**Done*/ prmContentGetDetails);
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
        public string PackingJO_DeleteDetail(PackagingJobOrder_Model _PackagingJobOrder_Model, string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@PJONo",DbType.String,_PackagingJobOrder_Model.PJO_No),
                                                        objProvider.CreateInitializedParameter("@PJODate",DbType.String,_PackagingJobOrder_Model.PJO_Date),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp$PPL$packingJODeleteAllSectionDetails",/*Done*/ prmContentInsert).ToString();
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
        public string PackingJOApproveDetails(string CompID, string BrchID, string PJO_No, string PJO_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                              objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                              objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                              objProvider.CreateInitializedParameter("@docno",DbType.String, PJO_No),
                                                              objProvider.CreateInitializedParameter("@docdate",DbType.String, PJO_Date),
                                                              objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                              objProvider.CreateInitializedParameter("@DocMenuId",DbType.String, MenuID),
                                                              objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                              objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                              objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                              objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),

                };


                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp$PPL$PackingJOApproveDetails",/*Done*/ prmContentGetDetails).ToString();
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
        public DataSet packingJOCancel(PackagingJobOrder_Model _PackagingJobOrder_Mode, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@PJO_no",DbType.String,  _PackagingJobOrder_Mode.PJO_No),
                    objProvider.CreateInitializedParameter("@PJO_dt",DbType.Date,  _PackagingJobOrder_Mode.PJO_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _PackagingJobOrder_Mode.Created_by ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_PPL$PackingJOCancel",/*Done*/ prmContentGetDetails);

            return DS;
        }
        public DataSet ChkPJOagainstMRS(string CompID, string BrID, string PJONo, string PJODate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@PJO_no",DbType.String, PJONo),
                    objProvider.CreateInitializedParameter("@PJO_dt",DbType.String, PJODate),

                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$PkgJO_ChckRJODetailAgainstMRS",/*Done*/ prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetDetailsOfRequiredMaterialTbl(string Comp_ID, string Br_ID, string PJO_No, string PJO_Date, string ShopfloorId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.Int32, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.Int32, Br_ID),
                                                         objProvider.CreateInitializedParameter("@PJO_No",DbType.String, PJO_No),
                                                         objProvider.CreateInitializedParameter("@PJO_Date",DbType.String, PJO_Date),
                                                         objProvider.CreateInitializedParameter("@ShfloorId",DbType.String, ShopfloorId),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$pkg$GetDetailsOfRequiredMaterial",/*Done*/ prmContentGetDetails);
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
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp$PPL$RewrkJO_GetCnsumMatrialStockBatchSerialwise]", /*Done*/prmContentGetDetails);
            return DS;
        }
        public DataSet getCMStockBatchWiseAfterInsert(string CompID, string BrID, string PJO_No, string PJO_Date, string MtrlTypId, string ItemId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@PJONo",DbType.String,  PJO_No),
            objProvider.CreateInitializedParameter("@PJODate",DbType.String,  PJO_Date),
            objProvider.CreateInitializedParameter("@MatrlType",DbType.String,  MtrlTypId),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$pkgJO_GetCMItemStockBatchwiseAfterInsert",/*Done*/ prmContentGetDetails);
            return DS;
        }
        public DataSet getCMStockSerialWiseAfterInsert(string CompID, string BrID, string PJO_No, string PJO_Date, string MtrlTypId, string ItemId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@PJONo",DbType.String,  PJO_No),
            objProvider.CreateInitializedParameter("@PJODate",DbType.String,  PJO_Date),
            objProvider.CreateInitializedParameter("@MatrlType",DbType.String,  MtrlTypId),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$PPL$pkgJO_GetCMItemStockSerialwiseAfterInsert",/*Done*/ prmContentGetDetails);
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PkgJO_GetSubItemDetailsAfterApprove",/*Done*/ prmContentGetDetails);
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PkgJO_GetSubItemDetailsAfterQC",/*Done*/ prmContentGetDetails);
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PJO_GetCMSubItemAvlStockDetailsBYShopfloor",/*Done*/ prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id, string UomId, string flag)
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
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "pkgGetSubItemAvlStockDetails", prmContentGetDetails);

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
    }
}
