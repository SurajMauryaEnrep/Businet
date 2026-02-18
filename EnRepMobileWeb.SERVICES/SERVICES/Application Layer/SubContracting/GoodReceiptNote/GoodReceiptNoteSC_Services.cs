using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.GoodReceiptNote;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.GoodReceiptNote;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SubContracting.GoodReceiptNote
{
  public  class GoodReceiptNoteSC_Services : GoodReceiptNoteSC_IServices
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
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, "D"),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppList", prmContentGetDetails);
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
            //return null;
        }

        public DataSet GetDeliveryNoteList(string Supp_id, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SC$GRN$detail_GetDeliveryNoteList", prmContentGetDetails);
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$GRN$GetDeliveryNoteDetail", prmContentGetDetails);
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
                //DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse_GetWarehouseList", prmContentGetDetails);
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
        public DataSet GetGRNSCDetails(string DNNo, string DNDate, string CompId, string BrID)
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SC$dn$item$detail_GetGRNSC_ItemDetails", prmContentGetDetails);
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
        public string InsertGRNSC_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTItemBatchDetail, 
            DataTable DTItemSerialDetail, DataTable DtblAttchDetail, DataTable DtblConsumeItemDetail, 
            DataTable DtblConsumeItemBatchDetails, DataTable DtblScrapItemDetail, DataTable DtblScrapItemBatchDetail, DataTable DtblSubItemDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@BatchDetail",SqlDbType.Structured, DTItemBatchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@SerialDetail",SqlDbType.Structured,DTItemSerialDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ConsumeItemDetail",SqlDbType.Structured,DtblConsumeItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ConsumeBatchDetail",SqlDbType.Structured,DtblConsumeItemBatchDetails),
                                                        objprovider.CreateInitializedParameterTableType("@ScrapItemDetail",SqlDbType.Structured,DtblScrapItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ScrapBatchDetail",SqlDbType.Structured,DtblScrapItemBatchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,DtblSubItemDetail),



                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SC$mr$InsertGRN_Details", prmcontentaddupdate).ToString();
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
        public string InsertGRNCosting_Details(DataTable DTHeaderDetail, DataTable DTItemDetail,DataTable CostingDetailConItmConList, DataTable DTItemTaxDetail, DataTable DTItemOCDetail, DataTable DTItemOCTaxDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ConItemDetail",SqlDbType.Structured, CostingDetailConItmConList),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTItemTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OcDetail",SqlDbType.Structured,DTItemOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DTItemOCTaxDetail),
                                                    };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sc$mr$detail_InsertGRNCosting_Details", prmcontentaddupdate).ToString();
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
            finally
            {
            }
        }
        public DataSet GetGRNSCListandSrchDetail(string CompId, string BrchID, GRNListModel _GRNListModel, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String,_GRNListModel.SuppID),
                                                       objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_GRNListModel.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_GRNListModel.ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _GRNListModel.Status),
                                                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                             objProvider.CreateInitializedParameter("@Docid",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$mr$GetGRNDeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetGRNDetailEditUpdate(string CompId, string BrchID, string GRNSC_NO, string GRNSC_Date, string UserID, string DocID)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@GRNNo",DbType.String, GRNSC_NO),
                                                        objProvider.CreateInitializedParameter("@GRNDate",DbType.String, GRNSC_Date),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SC$mr$GetGRN_DetailsOnDblClick", prmContentGetDetails);
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
        public string GRN_DeleteDetail(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel, string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@GRN_No",DbType.String,_GoodReceiptNoteSCModel.GRNNumber),
                                                        objProvider.CreateInitializedParameter("@GRN_Date",DbType.String,_GoodReceiptNoteSCModel.GRNDate),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$mr$DeleteGRNSC_Details", prmContentInsert).ToString();
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
         public string GRNApproveDetails(string CompID, string BrchID, string GRN_No, string GRN_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                              objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                              objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                              objProvider.CreateInitializedParameter("@docno",DbType.String, GRN_No),
                                                              objProvider.CreateInitializedParameter("@docdate",DbType.String, GRN_Date),
                                                              objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                              objProvider.CreateInitializedParameter("@DocMenuId",DbType.String, MenuID),
                                                              objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                              objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                              objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                              objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                                                              //objProvider.CreateInitializedParameter("@stkstatus",DbType.String,""),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$mr$Approved_GRNSC_Details", prmContentGetDetails).ToString();
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
        public string GRNCancel(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@GRN_No",DbType.String,  _GoodReceiptNoteSCModel.GRNNumber),
                    objProvider.CreateInitializedParameter("@GRN_DT",DbType.Date,  _GoodReceiptNoteSCModel.GRNDate),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _GoodReceiptNoteSCModel.CreatedBy ),
                      objProvider.CreateInitializedParameter("@TransType",DbType.String, _GoodReceiptNoteSCModel.TransType ),

                objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };

            //DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$DNSC$DNCancel", prmContentGetDetails);

            //return DS;
            string GRNMsg = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$GRNSCDetailCancel", prmContentGetDetails).ToString();
            return GRNMsg;
        }

        public DataSet getOrderResrvItemStockBatchWise(string ItemId, string CompID, string BrchID, string DN_NO)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            //objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompID),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BrchID),
            objProvider.CreateInitializedParameter("@DNNO",DbType.String,  DN_NO),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sc$stk$detail_GetOrderConsumeResItemStockBatchwise]", prmContentGetDetails);
            return DS;
        }
        public DataSet getOrderResrvItemStockBatchWiseOnDblClk(string CompID, string BrID, string GRNNo, string GRNDate, string ItemID, string DN_NO)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            //objProvider.CreateInitializedParameter("@IssueType",DbType.String, IssueType),
            objProvider.CreateInitializedParameter("@GRNNo",DbType.String, GRNNo),
            objProvider.CreateInitializedParameter("@GRNDate",DbType.String, GRNDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            objProvider.CreateInitializedParameter("@DNNO",DbType.String, DN_NO),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$mr$detail_ConsumeItemBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet GetSubItemDetailsFromDNSC(string CompID, string br_id, string Item_id, string DNNo, string doc_no, string doc_dt, string Flag, string Status)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                     objProvider.CreateInitializedParameter("@DNNo",DbType.String, DNNo),
                      objProvider.CreateInitializedParameter("@doc_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                       objProvider.CreateInitializedParameter("@Status",DbType.String, Status)


                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetSubItemDetailsFromDNSC]", prmContentGetDetails);
            return ds;
        }
        public DataSet CheckGRNAgainstInvoic(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$jobinv$detail_CheckGRNAgainstInovice", prmContentGetDetails);
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_SC$GRNSC$GetGRNSCDeatils_ForPrint", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
