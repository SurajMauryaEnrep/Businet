using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialTransferOrder;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialTransferOrder;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialTransferOrder
{
   public class MTO_SERVICES : MTO_ISERVICES
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
        //public DataTable GetToBranchList(string CompId,string BrchID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //                  objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrchID),
        //        };
        //        //DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetToBranchList", prmContentGetDetails).Tables[0];
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
        public DataTable GetToWhList(string CompId, string SrcBrId, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, SrcBrId),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.Int64, DocumentMenuId),
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

        public DataSet MTO_GetAllDDLListAndListPageData(string CompId, string Tobranch,string flag, string PageName, string UserID, string wfstatus, string DocumentMenuId
            , string startDate, string CurrentDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, Tobranch),
                      objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                       objProvider.CreateInitializedParameter("@PageName",DbType.String, PageName),
                        objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, startDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, CurrentDate),

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
        public string InsertMTO(DataTable MRSHeader, DataTable MRSItemDetails,DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, MRSHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, MRSItemDetails ),                               
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$mtr$InsertUpdateMTO", prmcontentaddupdate).ToString();

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
        public DataSet GetMTODetail(string CompID, string trf_no, string BrchID,string UserID,string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@trf_no",DbType.String, trf_no),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_MTO$DetailView", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet MTOApprove(MTOModel _MTOModel, string CompID, string br_id,string app_id, string mac_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@trf_no",DbType.String, _MTOModel.trf_no),
                    objProvider.CreateInitializedParameter("@trf_dt",DbType.Date,  _MTOModel.trf_dt),
                    objProvider.CreateInitializedParameter("@trf_type",DbType.String,  _MTOModel.trf_type),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, app_id ),
                     //objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, _MTOModel.A_Status ),
                         objProvider.CreateInitializedParameter("@wf_level",DbType.String, _MTOModel.A_Level ),
                         objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, _MTOModel.A_Remarks ),
                         objProvider.CreateInitializedParameter("@DocID",DbType.String,DocumentMenuId ),

                     };
                DataSet Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$MTOApprove", prmContentGetDetails);
                return Deatils; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet MTODelete(MTOModel _MTOModel, string CompID, string br_id, string DocumentMenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                   objProvider.CreateInitializedParameter("@trf_no",DbType.String, _MTOModel.trf_no),
                    objProvider.CreateInitializedParameter("@trf_dt",DbType.Date,  _MTOModel.trf_dt),
                    objProvider.CreateInitializedParameter("@trf_type",DbType.String,  _MTOModel.trf_type),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuID),
                                                     };
                DataSet Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$MTODelete", prmContentGetDetails);
                return Deatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public String MTOCancel(MTOModel _MTOModel, string CompID, string br_id, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@trf_no",DbType.String, _MTOModel.trf_no),
                    objProvider.CreateInitializedParameter("@trf_dt",DbType.Date,  _MTOModel.trf_dt),
                    objProvider.CreateInitializedParameter("@trf_type",DbType.String,  _MTOModel.trf_type),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MTOModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),

                    };

                string mrs_no = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Inv$MTO$Cancel", prmContentGetDetails).ToString();
                return mrs_no;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public String MTOForceClose(MTOModel _MTOModel, string CompID, string br_id, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@trf_no",DbType.String, _MTOModel.trf_no),
                    objProvider.CreateInitializedParameter("@trf_dt",DbType.Date,  _MTOModel.trf_dt),
                    objProvider.CreateInitializedParameter("@trf_type",DbType.String,  _MTOModel.trf_type),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MTOModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),

                    };

                string mrs_no = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Inv$MTO$ForceClose", prmContentGetDetails).ToString();
                return mrs_no;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet GetItemList(string CompId, string BranchId, string ItmName, string ToBranchId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@Branch",DbType.Int64, BranchId),
                     objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                      objProvider.CreateInitializedParameter("@ToBranchId",DbType.String, ToBranchId),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ivt$mtr$detail_ItemList", prmContentGetDetails);
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
        //For Sub-Item Data fetch
        public DataSet MTR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MTR_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSourceAndDestinationList(string CompId, string BrID, string wh_id,string doc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/   
                objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompId),
                objProvider.CreateInitializedParameter("@BrchID",DbType.Int32, BrID),
                objProvider.CreateInitializedParameter("@wh_id",DbType.String, wh_id),
                objProvider.CreateInitializedParameter("@doc_id",DbType.String, doc_id),
                };
                DataSet GetDT = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$MTR$GetWarehouseList", prmContentGetDetails);
                return GetDT;
            }
            catch (Exception Ex)
            {
                throw Ex;
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
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$MTR$Deatils_ForPrint", miobject);
                return dt;

            }
            catch (SqlException EX)
            {
                throw EX;
            }
            return null;
        }
    }
    public class MTOList_SERVICES : MTOList_ISERVICES
    {
        public DataSet GetMTODetailList(string CompId, string BrchID, string To_WH, string To_BR, string Fromdate, string Todate, string TRF_Type, string Status, string UserID, string wfstatus, string DocumentMenuId)
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
                                                         objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId)
                                                      };
                DataSet GetMTOList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMTOList", prmContentGetDetails);
                return GetMTOList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSerchDetailList(string CompId, string BrchID, int To_WH, int To_BR, string Fromdate, string Todate, string TRF_Type, string Status, string UserID, string DocumentMenuId)
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
                                                         objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, ""),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId)
                                                      };
                DataSet GetMTOList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMTOList", prmContentGetDetails);
                return GetMTOList;
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
