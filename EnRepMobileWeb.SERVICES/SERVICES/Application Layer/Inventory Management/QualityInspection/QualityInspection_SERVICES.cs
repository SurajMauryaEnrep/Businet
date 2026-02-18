using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnRepMobileWeb.UTILITIES;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.QualityInspection;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.QualityInspection;


namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.QualityInspection
{
   public class QualityInspection_SERVICES : QualityInspection_ISERVICES
    {
        public DataSet getQCInsSourceDocumentNo(string CompID, string BrchID, string DocumentNumber,string Src_type,string itemId,string supp_id)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@DocumentNumber",DbType.String, DocumentNumber),
                    objProvider.CreateInitializedParameter("@Src_type",DbType.String, Src_type),
                    objProvider.CreateInitializedParameter("@itemId",DbType.String, itemId),
                    objProvider.CreateInitializedParameter("@supp_id",DbType.String, supp_id),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$QCIns$GetSourceDocument]", prmContentGetDetails);
               
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            
        }

        public DataSet GetItemDetailBySourceDocumentNo(string CompID, string BrchID, string SourDocumentNo,string Src_Type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@SourDocumentNo",DbType.String, SourDocumentNo),
                     objProvider.CreateInitializedParameter("@Src_Type",DbType.String, Src_Type),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetitemDetailBySourceDocumentNo]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetItemQCParamDetail(string CompID,string br_id,  string ItemID,string qc_no,string qc_dt,string status)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                     objProvider.CreateInitializedParameter("@qc_no",DbType.String, qc_no),
                     objProvider.CreateInitializedParameter("@qc_dt",DbType.String, qc_dt),
                     objProvider.CreateInitializedParameter("@status",DbType.String, status),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetitemQCParamDetail]", prmContentGetDetails);
            return ds;
        }
        public String InsertQualityInspectionDetail(DataTable QualityInspectionHeader, DataTable QualityInspectionItemDetail,
            DataTable QualityInspectionItemParamDetail, DataTable Attachments, DataTable QualityInspectionLotBatchDetails,DataTable dtSubItem)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, QualityInspectionHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, QualityInspectionItemDetail ),
                   objprovider.CreateInitializedParameterTableType("@ItemParamDetail",SqlDbType.Structured, QualityInspectionItemParamDetail ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,Attachments ),
                 objprovider.CreateInitializedParameterTableType("@ItemLotBatchDetail",SqlDbType.Structured,QualityInspectionLotBatchDetails),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem),
                };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;

                string insp_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertQualityInspection_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[5].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }


        }

        public DataSet GetQcInspectionDetail(string CompID, string qc_no, string BrchID, string userid, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@qc_no",DbType.String, qc_no),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, userid),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_QC$Insp$DetailView", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        } public DataTable GetRejectionReason(string Comp_ID, string Br_ID, string search)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                    objProvider.CreateInitializedParameter("@search",DbType.String, search),
                  
            };
                DataTable ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Rejection$Reason", prmContentGetDetails).Tables[0];
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet QCInspectionDelete(QualityInspectionModel _QualityInspectionModel, string comp_id, string br_id,string DocumentMenuID,string qc_no)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@qc_no",DbType.String, _QualityInspectionModel.qc_no),
                     objProvider.CreateInitializedParameter("@qc_dt",DbType.Date,  _QualityInspectionModel.qc_dt),
                     objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.Date,  DocumentMenuID),
                };
                DataSet QCdelete = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$QCInspectionDelete", prmContentGetDetails);
                return QCdelete;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet QCInspectionApprove(QualityInspectionModel _QualityInspectionModel, string comp_id, string br_id, string mac_id,string DocumentMenuId,string Location_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@qc_no",DbType.String,  _QualityInspectionModel.qc_no),
                    objProvider.CreateInitializedParameter("@qc_dt",DbType.String,  _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd")),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _QualityInspectionModel.CreatedBy ),
                       objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@Src_Type",DbType.String, _QualityInspectionModel.qc_type ),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, _QualityInspectionModel.A_Status ),
                         objProvider.CreateInitializedParameter("@wf_level",DbType.String, _QualityInspectionModel.A_Level ),
                         objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, _QualityInspectionModel.A_Remarks ),
                         objProvider.CreateInitializedParameter("@menuid",DbType.String,DocumentMenuId ),
                         objProvider.CreateInitializedParameter("@Loc_type",DbType.String,Location_type ),
                     };
                DataSet QcApprove = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$QCInspectionApprove", prmContentGetDetails);
                return QcApprove; 
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet AfterApproveItemStockDetailBatchLotWise(QualityInspectionModel _QualityInspectionModel, string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@src_wh_id",DbType.String, _QualityInspectionModel.SourceWH),
                    objProvider.CreateInitializedParameter("@qc_no",DbType.String,  _QualityInspectionModel.qc_no),
                    objProvider.CreateInitializedParameter("@qc_dt",DbType.Date,  _QualityInspectionModel.qc_dt),
                    objProvider.CreateInitializedParameter("@ItemId",DbType.String, _QualityInspectionModel.item_id ),
                     };
                DataSet QcApprove = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "QC_GetLotBatchDetailsAfterSave", prmContentGetDetails);
                return QcApprove; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public String QCInspectionCancel(QualityInspectionModel _QualityInspectionModel, string comp_id, string userid, string br_id,string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@qc_no",DbType.String,  _QualityInspectionModel.qc_no),
                    objProvider.CreateInitializedParameter("@qc_dt",DbType.Date,  _QualityInspectionModel.qc_dt),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, userid),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                      objProvider.CreateInitializedParameter("@Src_Type",DbType.Date,  _QualityInspectionModel.qc_type),
                      objProvider.CreateInitializedParameter("@Loc_Type",DbType.Date,  _QualityInspectionModel.Location_type),
                     };
                string qc_no = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_stp$QCInspectionCancel", prmContentGetDetails).ToString();
                return qc_no; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet CheckGRNAgainstQC(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$qc$detail_CheckGRNAgainstQC", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetRejectWHList(string CompId, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_RejectWarehouseList", prmContentGetDetails).Tables[0];
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
        //public DataTable GetSourceAndAcceptWHList(string CompId, string BrchID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),

        //        };
        //        DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_AcceptWarehouseList", prmContentGetDetails).Tables[0];
        //        return searchmenu;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}
        public DataSet GetSourceAndAcceptWHList(string CompId, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_AcceptWarehouseList", prmContentGetDetails);
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
        public DataTable GetReworkWHList(string CompId, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                       objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ReworkWarehouseList", prmContentGetDetails).Tables[0];
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
        public DataSet GetBatchNo(string CompId, string BrID, string DocumentNumber)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {objProvider.CreateInitializedParameter("@CompId",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BranchId",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@DocumentNumber",DbType.String, DocumentNumber),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetProdBatchDetail]", prmContentGetDetails);
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
        public DataSet QC_GetSubItemDetails(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag)
        {
            if (Flag == "PUR"||Flag== "ApprvPUR" || Flag=="A" || Flag == "C"||Flag== "PRDApproved")
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@doc_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[QC_GetSubItemDetails]", prmContentGetDetails);
                return ds;
            }
            else
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@cnf_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@cnf_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[PC_GetSubItemDetailsAfterApprove]", prmContentGetDetails);
                return ds;
            }
            
        }
      
        public DataSet QCRWK_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@doc_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_RwkGetSubItemDetailsAfterApprove]", prmContentGetDetails);
            return ds;
        }
        public DataSet QC_GetSubItemDetailsAftrApprov(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@doc_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_QCDnscSubItemDetailsAfterApprove]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetItmListDL(string CompID, string BrID, string ItmName, string PageName,string SrcWh_Id,string LocationTyp,string suppid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@PageName",DbType.String, PageName),
                    objProvider.CreateInitializedParameter("@srcwh",DbType.String, SrcWh_Id),
                    //objProvider.CreateInitializedParameter("@srcshfl",DbType.String, SrcShfl_Id),
                    objProvider.CreateInitializedParameter("@LocationTyp",DbType.String, LocationTyp),
                    objProvider.CreateInitializedParameter("@suppid",DbType.String, suppid),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetQC_ItemList", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet GetQualityInspectionPrintDetails(string compId, string brId, string qcNo, string qcDate, string qcType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                    objProvider.CreateInitializedParameter("@QcNo",DbType.String, qcNo),
                    objProvider.CreateInitializedParameter("@QcDate",DbType.String, qcDate),
                    objProvider.CreateInitializedParameter("@QcType",DbType.String, qcType),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetQualityInspectionForPrint", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemStockBatchWiseofShopFloor(string CompId, string BranchId, string ItemId, string ShflID, string UOMId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@ShflID",DbType.Int32, ShflID),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@uom_id",DbType.String,  UOMId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$shfl$stk$detail_GetItemStockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet AfterApproveItemStockDetailBatchLotWiseforShopfloor(QualityInspectionModel _QualityInspectionModel, string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@src_shfl_id",DbType.String, _QualityInspectionModel.SourceSF),
                    objProvider.CreateInitializedParameter("@qc_no",DbType.String,  _QualityInspectionModel.qc_no),
                    objProvider.CreateInitializedParameter("@qc_dt",DbType.Date,  _QualityInspectionModel.qc_dt),
                    objProvider.CreateInitializedParameter("@ItemId",DbType.String, _QualityInspectionModel.item_id ),
                     };
                DataSet QcApprove = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Sp_QCGetLotBatchDetails_forShopFloor", prmContentGetDetails);
                return QcApprove; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
    public class QualityInspectionList_SERVICES : QualityInspectionList_ISERVICES
    {
        public DataSet GetQCDetailList(string CompId, string BrchID,  string FromDate, string Todate, string QC_Type, string Status
            , string userid, string wfstatus, string DocumentMenuId, string ItemID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),                                                       
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                         objProvider.CreateInitializedParameter("@QCType",DbType.String, QC_Type),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String, userid),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                      };
                DataSet GetQCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetQCInspectionList", prmContentGetDetails);
                return GetQCList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetStatusList(string MenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@menu_id",DbType.String, MenuID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$DocStatus", prmContentGetDetails);
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
       

    }
}

