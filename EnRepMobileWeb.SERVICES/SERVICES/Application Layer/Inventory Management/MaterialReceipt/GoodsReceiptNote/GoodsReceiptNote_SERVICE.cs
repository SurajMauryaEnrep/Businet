using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.GoodsReceiptNote;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.GoodsReceiptNote;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialReceipt.GoodsReceiptNote
{
    public class GoodsReceiptNote_SERVICE : GoodsReceiptNote_ISERVICE
    {
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSupplierList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["supp_id"].ToString(), PARQusData.Tables[0].Rows[i]["supp_name"].ToString());
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
            return null;
        }
        public DataSet GetAllData(string CompID, string SuppName, string BranchID, string User_ID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfstatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                     objProvider.CreateInitializedParameter("@UserID",DbType.String, User_ID),
                    objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                    objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$inv$GRN$List", prmContentGetDetails);
               
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
        public DataSet GetDeliveryNoteList(string Supp_id, string CompId,string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$dn$detail_GetDeliveryNoteList", prmContentGetDetails);
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
        public DataSet GetGRNDetails(string DNNo,string DNDate, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@DNNo",DbType.String, DNNo),
                    objProvider.CreateInitializedParameter("@DNDate",DbType.String, DNDate),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$dn$item$detail_GetGRN_ItemDetails", prmContentGetDetails);
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
        public DataSet GetWarehouseList(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrID),
                };
     //DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse_GetWarehouseList", prmContentGetDetails);//commented By Shubham Maurya 22-07-2024
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetDestWarehouseList", prmContentGetDetails);
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
        public DataSet GetPIAttatchDetailEdit(string CompID, string BrchID, string GRNNo, string GRNDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@grn_no",DbType.String, GRNNo),
                                                        objProvider.CreateInitializedParameter("@grn_date",DbType.String, GRNDate),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$grn$getAttatchmentDetail", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDeliveryNoteDetail(string DnNo, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@DnNo",DbType.String, DnNo),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$dn$detail_GetDeliveryNoteDetail", prmContentGetDetails);
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
        public string InsertGRN_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTItemBatchDetail, DataTable DTItemSerialDetail, DataTable DtblAttchDetail, DataTable DtblSubItemDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@BatchDetail",SqlDbType.Structured, DTItemBatchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@SerialDetail",SqlDbType.Structured,DTItemSerialDetail),
                                                        //objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DTItemSerialDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                         objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                         objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,DtblSubItemDetail),
                                                         //objprovider.CreateInitializedParameterTableType("@Voudetail",SqlDbType.Structured,Voudetail),
                                                    };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$mr$detail_InsertGRN_Details", prmcontentaddupdate).ToString();
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

            finally
            {
            }
        }
        public string InsertGRNCosting_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTItemTaxDetail, DataTable DTItemOCDetail,DataTable DTItemOCTaxDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTItemTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OcDetail",SqlDbType.Structured,DTItemOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DTItemOCTaxDetail),
                                                    };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$mr$detail_InsertGRNCosting_Details", prmcontentaddupdate).ToString();
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

            finally
            {
            }
        }
        public string Delete_GRN_Detail(GoodsReceiptNoteModel GoodsReceiptNote, string MrType, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@GRN_No",DbType.String, GoodsReceiptNote.grn_no ),
                                                        objProvider.CreateInitializedParameter("@GRN_Date",DbType.String,GoodsReceiptNote.grn_dt),
                                                        objProvider.CreateInitializedParameter("@Mr_Type",DbType.String,MrType),
                };
                string GRNId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "inv$mr$detail_DeleteGRN_Details", prmContentInsert).ToString();
                return GRNId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string Approve_GRN(string grn_no,string grn_dt, string Mr_Type, string MenuID, string Branch, string CompID, string ApproveID, string mac_id, string wf_status, string wf_level, string wf_remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@grnno",DbType.String,grn_no),          
                                                        objProvider.CreateInitializedParameter("@grndate",DbType.String,grn_dt),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String,MenuID),                                                       
                                                        objProvider.CreateInitializedParameter("@mrtype",DbType.String, Mr_Type ),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,ApproveID),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String,Branch),                                                       
                                                         objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                                                        //objProvider.CreateInitializedParameter("@Narr",DbType.String,Narr),
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mr$detail_Approved_GRN_Details", prmContentInsert);
                string DocNo = string.Empty;
                DocNo = ds.Tables[0].Rows[0]["grn_detail"].ToString();
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
        public DataSet Edit_GRNDetail( string CompId, string BrID, string GRNNo, string GRNDate, string UserID, string DocID, string Type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
                                                        objProvider.CreateInitializedParameter("@GRNNo",DbType.String, GRNNo),
                                                        objProvider.CreateInitializedParameter("@GRNDate",DbType.String, GRNDate),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                        objProvider.CreateInitializedParameter("@MrType",DbType.String, Type),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mr$detail_GetGRN_Details", prmContentGetDetails);
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
        public string Check_GRNItemStock(string CompID, string Branch, string MR_No, string MR_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentcheck = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Branch),
                                                        objProvider.CreateInitializedParameter("@Mr_No",DbType.String, MR_No),
                                                        objProvider.CreateInitializedParameter("@Mr_Date",DbType.String, MR_Date),
                                                        objProvider.CreateInitializedParameter("@StockChangeFlag",DbType.String,""),
                };
                prmContentcheck[4].Size = 1;
                prmContentcheck[4].Direction = ParameterDirection.Output;
                string SRDetail = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$stk$detail_CheckedGRN_StockQty", prmContentcheck).ToString();
                string StockChangeFlag = string.Empty;
                if (prmContentcheck[4].Value != DBNull.Value) // status
                {
                    StockChangeFlag = prmContentcheck[4].Value.ToString();
                }
                return StockChangeFlag;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet CheckInvoiceAgainstGRN(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mr$detail_CheckInvoiceAgainstGRN", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetGRNDeatilsForPrint(string CompID, string Branch, string GRN_No, string GRN_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, Branch),
                                                        objProvider.CreateInitializedParameter("@grn_no",DbType.String, GRN_No),
                                                        objProvider.CreateInitializedParameter("@grn_dt",DbType.String, GRN_Date),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetGRNDeatils_ForPrint", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataSet GetAllGLDetails(DataTable GLDetail)
        //{
        //    try
        //    {
        //        SqlDataProvider objprovider = new SqlDataProvider();
        //        SqlParameter[] prmcontentaddupdate = {
        //                                                objprovider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured,GLDetail),
        //                                            };

        //        DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetGLDetail", prmcontentaddupdate);
        //        return GetGlDt;

        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //    }
        //}
     
    }
    public class GoodsReceiptNoteList_SERVICE : GoodsReceiptNoteList_ISERVICE
    {
        public DataSet GetGRNDetailList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfstatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mr$detail_GetGRNDeatilList", prmContentGetDetails);
                return GetPODetailList;
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
