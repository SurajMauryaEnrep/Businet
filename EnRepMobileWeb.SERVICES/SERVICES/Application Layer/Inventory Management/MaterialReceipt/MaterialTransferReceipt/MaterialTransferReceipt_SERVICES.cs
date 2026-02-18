using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.MaterialTransferReceipt;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.MaterialTransferReceipt;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialReceipt.MaterialTransferReceipt
{
   public class MaterialTransferReceipt_SERVICES : MaterialTransferReceipt_ISERVICES
    {
        //public DataTable GetWhList(string CompId, string BrchID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //             objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrchID),

        //        };
        //        DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetfromWarehouseList", prmContentGetDetails).Tables[0];
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
        //public DataTable GetToBranchList(string CompId)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //        };
        //        DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetToBranchList", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllDDLListAndListPageData(string CompId, string BrchID,string flag, string PageName, /*string From_br, string From_wh, string Fromdate, string Todate, string Status,*/ string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrchID),
                     objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                     objProvider.CreateInitializedParameter("@PageName",DbType.String, PageName),
                     //objProvider.CreateInitializedParameter("@From_br",DbType.String, From_br),
                     //                                    objProvider.CreateInitializedParameter("@From_wh",DbType.String, From_wh),
                     //                                   objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                     //                                   objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        //objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId)
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$MTR$GetBranchAndfromWarehouseList", prmContentGetDetails);
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
        public DataTable GetToWhList(string CompId, string Tobranch)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, Tobranch),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, "105102115110"),

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetToWarehouseList", prmContentGetDetails).Tables[0];
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
        public DataSet getMTRNOList(string CompID, string BrchID, string MTRNo, string SourceBR, string SourceWH, string TransferType, string TOWH, string TOBR)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@trf_type",DbType.String, TransferType),
                     objProvider.CreateInitializedParameter("@from_br",DbType.Int64, SourceBR),
                    objProvider.CreateInitializedParameter("@from_wh",DbType.Int64, SourceWH),
                    objProvider.CreateInitializedParameter("@trf_no",DbType.String, MTRNo),
                     objProvider.CreateInitializedParameter("@to_wh",DbType.Int64, TOWH),
                      objProvider.CreateInitializedParameter("@to_br",DbType.Int64, TOBR),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$GetMTRListForReceipt", prmContentGetDetails);
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
        public DataSet getMINOList(string CompID, string BrchID, string SourceBR, string SourceWH, string MTRNo, string MTRDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int32, BrchID),
                     objProvider.CreateInitializedParameter("@from_br",DbType.Int64, SourceBR),
                    objProvider.CreateInitializedParameter("@from_wh",DbType.Int64, SourceWH),
                    objProvider.CreateInitializedParameter("@trf_no",DbType.String, MTRNo),
                     objProvider.CreateInitializedParameter("@MTRDate",DbType.String, MTRDate),
                     
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$GetMTIListForReceipt", prmContentGetDetails);
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
        public DataSet GetMaterialTransferItemDetail(string CompID, string BrchID, string TRFDate, string TRFNo, string TRFType,string MINo, string MIDate,string frombranch)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@TRFDate",DbType.DateTime, TRFDate),
                    objProvider.CreateInitializedParameter("@TRFNo",DbType.String, TRFNo),
                    objProvider.CreateInitializedParameter("@trf_type",DbType.String, TRFType),
                    objProvider.CreateInitializedParameter("@frombranch",DbType.String, frombranch),
                      objProvider.CreateInitializedParameter("@MINo",DbType.String, MINo),
                    objProvider.CreateInitializedParameter("@MIDate",DbType.String, MIDate),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mtr$GetMTRItemDetailForReceipt", prmContentGetDetails);

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
        public DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo, string IssueDate, string ItemID, string frombranch)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@IssueType",DbType.String, IssueType),
            objProvider.CreateInitializedParameter("@IssueNo",DbType.String, IssueNo),
            objProvider.CreateInitializedParameter("@IssueDate",DbType.String, IssueDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
             objProvider.CreateInitializedParameter("@frombranch",DbType.String, frombranch),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_MIItem_StockBatchwisedetail", prmContentGetDetails);
            return DS;
        }
        public string InsertUpdateMaterialTransferReceipt(DataTable MaterialTransferReceiptHeader, DataTable MaterialTransferReceiptItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable Attachments)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, MaterialTransferReceiptHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, MaterialTransferReceiptItemDetails ),
                 //objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,MaterialIssueAttachments ),
                 objprovider.CreateInitializedParameterTableType("@BatchDetail",SqlDbType.Structured,ItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@SerialDetail",SqlDbType.Structured,ItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,Attachments),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$tmr$detail_InsertTMR_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet MaterialTransferReceiptCancel(MaterialTransferReceiptModel _MTRModel, string CompID, string br_id, string mac_id, string MTI_date)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@mr_no",DbType.String,  _MTRModel.MaterialReceiptNo),
                    objProvider.CreateInitializedParameter("@mr_dt",DbType.Date,  MTI_date),
                    //objProvider.CreateInitializedParameter("@issue_type",DbType.Date, MTI_Type),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MTRModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                      objProvider.CreateInitializedParameter("@frombr_id",DbType.String, _MTRModel.from_brid),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MaterialTransferReceiptCancel", prmContentGetDetails);

            return DS;
        }
        public DataSet GetTMRDetailByNo(string CompID, string BrchID, string TMR_no, string TMR_date, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    //objProvider.CreateInitializedParameter("@trf_type",DbType.String, trf_type),
                    objProvider.CreateInitializedParameter("@TMR_no",DbType.String, TMR_no),
                    objProvider.CreateInitializedParameter("@TMR_date",DbType.String, TMR_date),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetTMR_Detail", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public string Approve_TMR(MaterialTransferReceiptModel _MTRModel, string Branch, string CompID, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String,Branch),
                                                        objProvider.CreateInitializedParameter("@TMR_no",DbType.String,_MTRModel.MaterialReceiptNo),
                                                        objProvider.CreateInitializedParameter("@TMR_date",DbType.String,_MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd")),
                                                        objProvider.CreateInitializedParameter("@frombr_id",DbType.String,_MTRModel.from_brid ),
                                                        objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MTRModel.CreatedBy ),
                                                        //objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                         objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                         objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                         objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                                                         objProvider.CreateInitializedParameter("@DocID",DbType.String, DocumentMenuId),
                };
                DataSet TMRDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$tmr$detail_Approved_TMR_Details", prmContentInsert);
                string DocuMTRNo = string.Empty;
                DocuMTRNo = TMRDetail.Tables[1].Rows[0]["mtrd_detail"].ToString();
                return DocuMTRNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet MTRDelete(MaterialTransferReceiptModel _MTRModel, string CompID, string br_id,string MaterialReceiptNo)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                   objProvider.CreateInitializedParameter("@mr_no",DbType.String,  _MTRModel.MaterialReceiptNo),
                    objProvider.CreateInitializedParameter("@mr_dt",DbType.Date,  _MTRModel.MaterialReceiptDate),
                   // objProvider.CreateInitializedParameter("@trf_type",DbType.String,  _MTOModel.trf_type),
                                                     };
                DataSet Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$TMRDelete", prmContentGetDetails);
                return Deatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet MTRcpt_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag,string frombranch)
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
                    objProvider.CreateInitializedParameter("@frombranch",DbType.String, frombranch),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MTRcpt_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetMTRDeatilsForPrint(string CompID, string BrchID, string MTRNo, string MTRDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@MTR_No",DbType.String, MTRNo),
                     objProvider.CreateInitializedParameter("@MTR_Dt",DbType.String, MTRDate),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetMTReceiptDeatils_ForPrint", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
    }
    public class TMRList_SERVICES : TMRList_ISERVICES
    {
        public DataSet GetTMRDetailList(string CompId, string BrchID, string From_br, string From_wh, string Fromdate, string Todate,string Status,string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@From_br",DbType.String, From_br),
                                                         objProvider.CreateInitializedParameter("@From_wh",DbType.String, From_wh),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),                                                      
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId)
                                                      };
                DataSet GetTMRList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetTMRList", prmContentGetDetails);
                return GetTMRList;
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

