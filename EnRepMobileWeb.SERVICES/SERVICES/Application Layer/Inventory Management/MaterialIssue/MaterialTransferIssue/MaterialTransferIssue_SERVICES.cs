using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialIssue.MaterialTransferIssue;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialIssue.MaterialTransferIssue;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialIssue.MaterialTransferIssue
{
    public class MaterialTransferIssue_SERVICES : MaterialTransferIssue_ISERVICES
    {
        public DataTable GetWhList(string CompId, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrchID),

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetfromWarehouseList", prmContentGetDetails).Tables[0];
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
        public DataTable GetToBranchList(string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetToBranchList", prmContentGetDetails).Tables[0];
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

        public DataSet MTI_GetAllDDLListAndListPageData(string CompId, string BrchID, string flag,
             string startDate, string CurrentDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrchID),
                     objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                     objProvider.CreateInitializedParameter("@Fromdate",DbType.String, startDate),
                     objProvider.CreateInitializedParameter("@Todate",DbType.String, CurrentDate),

                };
                DataSet TblDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SP$MTI$GetAllDDLListAndListPageData]", prmContentGetDetails);
                return TblDetails;
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

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetDestWarehouseList", prmContentGetDetails).Tables[0];
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
        public DataSet GetMTODetailByNo(string CompID, string BrchID, string MTI_no, string MTI_date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    //objProvider.CreateInitializedParameter("@trf_type",DbType.String, trf_type),
                    objProvider.CreateInitializedParameter("@MTI_no",DbType.String, MTI_no),
                    objProvider.CreateInitializedParameter("@MTI_date",DbType.String, MTI_date),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetMTI_Detail", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$GetMTRList", prmContentGetDetails);
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
        public DataSet GetMaterialTransferItemDetail(string CompID, string BrchID, string TRFDate, string TRFNo, string TRFType, string SessionBrchID)
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
                    objProvider.CreateInitializedParameter("@SessionBrchID",DbType.String, SessionBrchID),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mtr$GetMTRItemDetail", prmContentGetDetails);

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
        public string InsertUpdateMaterialTransferIssue(DataTable MaterialTransferIssuetHeader, DataTable MaterialTransferIssueItemDetails
            , DataTable ItemBatchDetails, DataTable ItemSerialDetails,DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, MaterialTransferIssuetHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, MaterialTransferIssueItemDetails ),
                 //objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,MaterialIssueAttachments ),
                 objprovider.CreateInitializedParameterTableType("@ItemBatchDetails",SqlDbType.Structured,ItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemSerialDetails",SqlDbType.Structured,ItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem ),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$InsertUpdateMaterialTransferIssue", prmcontentaddupdate).ToString();

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
        public DataSet MaterialTransferIssueCancel(MaterialTransferIssueModel _MTIModel, string CompID, string br_id, string mac_id, string MTI_date, string MTI_Type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@issue_no",DbType.String,  _MTIModel.MaterialIssueNo),
                    objProvider.CreateInitializedParameter("@issue_dt",DbType.Date,  MTI_date),
                    objProvider.CreateInitializedParameter("@issue_type",DbType.Date, MTI_Type),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MTIModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                      objProvider.CreateInitializedParameter("@tobr_id",DbType.String, _MTIModel.hdto_brid),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MaterialTransferIssueCancel", prmContentGetDetails);

            return DS;
        }
      
        public DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo, string IssueDate, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@IssueType",DbType.String, IssueType),
            objProvider.CreateInitializedParameter("@IssueNo",DbType.String, IssueNo),
            objProvider.CreateInitializedParameter("@IssueDate",DbType.String, IssueDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mti$item$bt$detail_Get_MIItem_StockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet CheckTransferRecieve(string CompID, string br_id, string txtMaterialIssueNo, string txtMaterialIssueDate)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, br_id),
            objProvider.CreateInitializedParameter("@MaterialIssueNo",DbType.String, txtMaterialIssueNo),
            objProvider.CreateInitializedParameter("@MaterialIssueDate",DbType.String, txtMaterialIssueDate),
       
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mti$item$bt$detail_CheckTransferRecieve", prmContentGetDetails);
            return DS;
        }

        public DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo, string IssueDate, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@IssueType",DbType.String, IssueType),
            objProvider.CreateInitializedParameter("@IssueNo",DbType.String, IssueNo),
            objProvider.CreateInitializedParameter("@IssueDate",DbType.String, IssueDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mti$item$sr$detail_Get_MIItem_StockSerialwise", prmContentGetDetails);
            return DS;
        }
        public DataSet MTI_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MTI_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetMaterialTransferIssuePrint(string CompID, string BrchID, string Doc_No, string Doc_dt)
        {
            try
            {
                SqlDataProvider dataobject = new SqlDataProvider();
                SqlParameter[] miobject ={
                      dataobject.CreateInitializedParameter("@comid",DbType.Int16,CompID),
                      dataobject.CreateInitializedParameter("@brid",DbType.Int64,BrchID),
                      dataobject.CreateInitializedParameter("@DocNo",DbType.String,Doc_No),
                      dataobject.CreateInitializedParameter("@Doc_dt",DbType.DateTime,Doc_dt),
                };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$MTI$Deatils_ForPrint]", miobject);
                return dt;

            }
            catch (SqlException EX)
            {
                throw EX;
            }
            return null;
        }
        public DataSet MTI_getItemstockWarehouseWise(string ItemId, string UomId, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[MTI_GetItemStockWareHousewise]", prmContentGetDetails);
            return DS;
        }

    }
    public class MTIList_SERVICES : MTIList_ISERVICES
    {
        public DataTable GetMTIDetailList(string CompId, string BrchID, int To_WH, int To_BR, string Fromdate, string Todate, string TRF_Type, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@To_WH",DbType.String, To_WH),
                                                         objProvider.CreateInitializedParameter("@To_BR",DbType.String, To_BR),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                         objProvider.CreateInitializedParameter("@TRFType",DbType.String, TRF_Type),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                      };
                DataSet GetMTOList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMTIList", prmContentGetDetails);
                return GetMTOList.Tables[0];
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
