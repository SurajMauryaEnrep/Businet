using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.PurchaseReturn;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.PurchaseReturn;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.PurchaseReturn
{
   public class PurchaseReturn_SERVICES : PurchaseReturn_ISERVICES
    {
        public Dictionary<string, string> AutoGetSupplierListALl(string CompID, string SuppName, string BrchID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetDomesticSuppList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["supp_id"].ToString(), PARQusData.Tables[0].Rows[i]["supp_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            
        }
        public DataSet GetAllData(string CompID, string SuppName, string BrchID,
            string SuppId, string Fromdate, string Todate, string Status, string wfstatus, string UserID, string DocumentMenuId)
        {


            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),

                                                          objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$Pur$Return$List", prmContentGetDetails);

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
        public DataSet GetPurchaseInvoiceNo(string CompID, string BrchID, string SupplierId, string DocumentNumber,string Src_Type)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@SupplierId",DbType.String, SupplierId),
                      objProvider.CreateInitializedParameter("@DocumentNumber",DbType.String, DocumentNumber),
                      objProvider.CreateInitializedParameter("@Src_Type",DbType.String, Src_Type),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$GetPurchaseInvoice]", prmContentGetDetails);

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
        public DataSet GetPIItemDetail(string CompID, string BrchID, string SourDocumentNo,string src_type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@SourDocumentNo",DbType.String, SourDocumentNo),
                     objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$pinv$GetInvoiceItemDetailByDocumentNo]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetGRNItemDetail(string CompID, string BrchID, string ItemID, string GRNNumber,string SrcDocNumber, string RT_Status,string src_type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                      objProvider.CreateInitializedParameter("@GRNNumber",DbType.String, GRNNumber),
                      objProvider.CreateInitializedParameter("@SrcDocNo",DbType.String, SrcDocNumber),
                      objProvider.CreateInitializedParameter("@RT_Status",DbType.String, RT_Status),
                      objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),

                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetitemLotSerialDetail]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetInvoiceItemDetail(string CompID, string BrchID, string ItemID, string InvoiceNo, string GRNNumber,string src_type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                      objProvider.CreateInitializedParameter("@InvoiceNo",DbType.String, InvoiceNo),
                       objProvider.CreateInitializedParameter("@GRNNumber",DbType.String, GRNNumber),
                       objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetitemInvoiceCalculationDetail]", prmContentGetDetails);
            return ds;
        }
        public String InsertPurchaseReturnDetail(DataTable PurchaseReturnHeader, DataTable PurchaseReturnItemDetails
            ,DataTable PurchaseReturnLotBatchSerial,DataTable PurchaseReturnVoudetail, DataTable dtSubItem
            , DataTable PRCostCenterDetails,DataTable DtblTaxDetail, DataTable DtblOCDetail, DataTable DtblOCTaxDetail, string src_type,string AdHocBill_no,string AdHocBill_dt,string oc_amt)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, PurchaseReturnHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, PurchaseReturnItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemLotBatchSerialDetail",SqlDbType.Structured,PurchaseReturnLotBatchSerial ),
                 objprovider.CreateInitializedParameterTableType("@PurchaseReturnVoudetail",SqlDbType.Structured,PurchaseReturnVoudetail ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                  objprovider.CreateInitializedParameterTableType("@SubItemDetals",SqlDbType.Structured,dtSubItem ),
                  objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured,PRCostCenterDetails ),
                  objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured,DtblTaxDetail ),
                  objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DtblOCDetail ),
                  objprovider.CreateInitializedParameterTableType("@OC_TaxDetail",SqlDbType.Structured,DtblOCTaxDetail ),
                  objprovider.CreateInitializedParameterTableType("@src_type",SqlDbType.NVarChar,src_type ),
                  objprovider.CreateInitializedParameterTableType("@AdHocBill_no",SqlDbType.NVarChar,AdHocBill_no ),
                  objprovider.CreateInitializedParameterTableType("@AdHocBill_dt",SqlDbType.NVarChar,AdHocBill_dt ),
                  objprovider.CreateInitializedParameterTableType("@oc_amt",SqlDbType.NVarChar,oc_amt ),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_InsertPurchaseReturn_Details", prmcontentaddupdate).ToString();

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
        public DataSet GetPurchaseReturnDetail(string Prt_no, string Prt_dt, string CompID, string BrchID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Prt_no",DbType.String, Prt_no),
                    objProvider.CreateInitializedParameter("@Prt_dt",DbType.String, Prt_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_PurchaseReturn$DetailView", prmContentGetDetails);
                return ds;
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
        public DataSet GetPurchaseReturnListAll(string SuppId, string Fromdate, string Todate, string Status,string CompID, string BrchID,string wfstatus, string UserID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    
                     
                                                         objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),                                                        
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                          objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetPurchaseReturnList$details]", prmContentGetDetails);
            return dt;//.Tables[0]; 
        }
        public DataSet PurchaseReturnCancel(PurchaseReturn_Model _PurchaseReturn_Model, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@prt_no",DbType.String,  _PurchaseReturn_Model.prt_no),
                    objProvider.CreateInitializedParameter("@prt_dt",DbType.Date,  _PurchaseReturn_Model.prt_dt),                   
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _PurchaseReturn_Model.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@Narr",DbType.String, _PurchaseReturn_Model.PvNarr),
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String, _PurchaseReturn_Model.CancelledRemarks),
                     objProvider.CreateInitializedParameter("@src_type",DbType.String, _PurchaseReturn_Model.Src_Type),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_prc$PurchaseReturnCancel", prmContentGetDetails);

            return DS;
        }
        public DataSet PurchaseReturnDelete(PurchaseReturn_Model _PurchaseReturn_Model, string CompID, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                   objProvider.CreateInitializedParameter("@prt_no",DbType.String, _PurchaseReturn_Model.prt_no),
                    objProvider.CreateInitializedParameter("@prt_dt",DbType.Date,  _PurchaseReturn_Model.prt_dt),
                    
                                                     };
                DataSet Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_prc$PurchaseReturnDelete]", prmContentGetDetails);
                return Deatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string PurchaseReturnApprove(string PRTNo, string PRTDate, string userid, string wf_status, string wf_level, string wf_remarks, string DnNarr, string comp_id, string br_id, string mac_id,  string DocID,string src_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@prt_no",DbType.String, PRTNo),
                    objProvider.CreateInitializedParameter("@prt_dt",DbType.Date,  PRTDate),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, userid ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                               objProvider.CreateInitializedParameter("@Narr",DbType.String, DnNarr),
                               objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                     };
                DataSet PRTDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_prc$PurchaseReturnApprove", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = PRTDeatils.Tables[0].Rows[0]["prt_detail"].ToString();
                return DocNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet GetAllGLDetails(DataTable GLDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured,GLDetail),
                                                    };

                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetGLDetail", prmcontentaddupdate);
                return GetGlDt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet GetRoundOffGLDetails(string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     };
                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetRoundOffGLDetail", prmContentGetDetails);
                return GetGlDt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id, string flag, string GRNNo, string GRNDt,string src_type,int UOMID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.Int32, wh_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    objProvider.CreateInitializedParameter("@GRNNo",DbType.String, GRNNo),
                    objProvider.CreateInitializedParameter("@GRNDate",DbType.String, GRNDt),
                    objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                    objProvider.CreateInitializedParameter("@UOMID",DbType.Int32, UOMID),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PRT_GetSubItemAvlStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet PRT_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string flag, string GRNNo, string GRNDt, string wh_id,string src_type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@prt_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@prt_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                      objProvider.CreateInitializedParameter("@GRNNo",DbType.String, GRNNo),
                    objProvider.CreateInitializedParameter("@GRNDate",DbType.String, GRNDt),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.Int32, wh_id),
                      objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type)
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[PRTGetSubItemDetailsAftrApprove]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetSubItemDetailsFromPinv(string CompID, string BrchID, string PinvNo, string GRNNo, string GRNDt, string Item_id,string src_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@pinv_no",DbType.String, PinvNo),
                    objProvider.CreateInitializedParameter("@mr_no",DbType.String, GRNNo),
                    objProvider.CreateInitializedParameter("@mr_dt",DbType.String, GRNDt),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PRT_GetSubItemDetailsFromPInv", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPurchaseReturnDeatils(string Comp_ID, string Br_ID, string prt_no, string prt_dt,string src_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@prt_no",DbType.String, prt_no),
                                                        objProvider.CreateInitializedParameter("@prt_dt",DbType.String, prt_dt),
                                                        objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPurchaseReturnDeatils_ForPrint", prmContentGetDetails);
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
        public DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId,string Doclist)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@DocNos",DbType.String,  Doclist),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetDocItemStockBatchwise]", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemStockBatchWiseAfterInsert(string CompID, string BrID, string PL_No, string PL_Date, string ItemId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@PrtNo",DbType.String,  PL_No),
            objProvider.CreateInitializedParameter("@PrtDate",DbType.String,  PL_Date),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$prt$item$bt$detail_Get_Item_StockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet GetTaxAmountDetail(string CompID, string BrchID, string ItmCode, string InvoiceNo, string ShipNumber, string ReturnQuantity, string src_type,DataTable GlTaxDetailInsight)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItmCode),
                      objProvider.CreateInitializedParameter("@InvoiceNo",DbType.String, InvoiceNo),
                       objProvider.CreateInitializedParameter("@ShipNumber",DbType.String, ShipNumber),
                       objProvider.CreateInitializedParameter("@RetrnQty",DbType.String, ReturnQuantity),
                       objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                       objProvider.CreateInitializedParameterTableType("@GlTaxDetailInsight",SqlDbType.Structured, GlTaxDetailInsight),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_prt$GetTaxAmountDetail]", prmContentGetDetails);
            return ds;
        }
    }
}
