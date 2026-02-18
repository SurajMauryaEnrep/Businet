using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesReturn;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesReturn;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.SalesReturn
{
   public class SalesReturn_SERVICES : SalesReturn_ISERVICES
    {
        public Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID,string CustType,string src_type)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@CustName",DbType.String, Cust_Name),                            
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                             objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                             //objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                                                             };

                //DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, /*"stp$cust$detail_GetCustListForReturn"*/ "stp$cust$detail_GetCustList", prmContentGetDetails);
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetCustList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["cust_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_name"].ToString());
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
        public DataSet GetAllData(string CompID, string Cust_Name, string BrchID,string CustType
            , string CustId, string Fromdate, string Todate, string Status, string wfstatus, string UserID, string DocumentMenuId)
        {
           

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@CustName",DbType.String, Cust_Name),                            
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                             objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                                objProvider.CreateInitializedParameter("@CustId",DbType.String, CustId),
                            objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                            objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                            objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                              objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                            objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                            objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$Sales$Return$List", prmContentGetDetails);
                
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
        public DataSet GetSalesInvoiceNo(string CompID, string BrchID, string CustomerId, string DocumentNumber,string Src_Type)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@CustomerId",DbType.String, CustomerId),
                      objProvider.CreateInitializedParameter("@DocumentNumber",DbType.String, DocumentNumber),
                      objProvider.CreateInitializedParameter("@Src_Type",DbType.String, Src_Type),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetSalesInvoiceForReturn]", prmContentGetDetails);

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
        public DataSet GetSIItemDetail(string CompID, string BrchID, string SourDocumentNo,string src_type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@SourDocumentNo",DbType.String, SourDocumentNo),
                     objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$sinv$GetSalesInvoiceItemDetailByDocumentNo]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetShipmentItemDetail(string CompID, string BrchID, string ItemID, string ShipNumber, string SrcDocNumber, string RT_Status,string src_type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                      objProvider.CreateInitializedParameter("@ShipNumber",DbType.String, ShipNumber),
                      objProvider.CreateInitializedParameter("@SrcDocNo",DbType.String, SrcDocNumber),
                      objProvider.CreateInitializedParameter("@RT_Status",DbType.String, RT_Status),
                      objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),

                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetShipment_itemLotBatchSerialDetail]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetSalesInvoiceItemDetail(string CompID, string BrchID, string ItemID, string InvoiceNo, string ShipNumber,string src_type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                      objProvider.CreateInitializedParameter("@InvoiceNo",DbType.String, InvoiceNo),
                       objProvider.CreateInitializedParameter("@ShipNumber",DbType.String, ShipNumber),
                       objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetSalesitemInvoiceCalculationDetail]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetTaxAmountDetail(string CompID, string BrchID, string ItmCode, string InvoiceNo, string ShipNumber, string ReturnQuantity,string src_type)
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
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_SR$GetTaxAmountDetail]", prmContentGetDetails);
            return ds;
        }

        public String InsertSalesReturnDetail(DataTable SalesReturnHeader, DataTable SalesReturnItemDetails, DataTable SalesReturnLotBatchSerial,DataTable SalesReturnVoudetail,DataTable dtSubItem,DataTable CRCostCenterDetails,DataTable DtblTaxDetail, string Src_Type,string InvBillNumber,string InvBillDate, string Payment_term, string Delivery_term,DataTable DtblVouGLDetail,DataTable DtblOCDetail,DataTable DtblOCTaxDetail,string oc_amt)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, SalesReturnHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, SalesReturnItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemLotBatchSerialDetail",SqlDbType.Structured,SalesReturnLotBatchSerial ),
               objprovider.CreateInitializedParameterTableType("@SalesReturnVoudetail",SqlDbType.Structured,SalesReturnVoudetail ),
                    objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                    objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                    objprovider.CreateInitializedParameterTableType("@CCDetail",SqlDbType.Structured, CRCostCenterDetails ),
                    objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DtblTaxDetail ),
                    objprovider.CreateInitializedParameterTableType("@Src_Type",SqlDbType.NVarChar, Src_Type ),
                    objprovider.CreateInitializedParameterTableType("@InvBillNumber",SqlDbType.NVarChar, InvBillNumber ),
                    objprovider.CreateInitializedParameterTableType("@InvBillDate",SqlDbType.NVarChar, InvBillDate ),
                    objprovider.CreateInitializedParameterTableType("@pay_term",SqlDbType.NVarChar, Payment_term),
                    objprovider.CreateInitializedParameterTableType("@del_term",SqlDbType.NVarChar, Delivery_term),
                    objprovider.CreateInitializedParameterTableType("@DtblVouGLDetail",SqlDbType.Structured, DtblVouGLDetail),
                    objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured, DtblOCDetail),
                    objprovider.CreateInitializedParameterTableType("@OC_TaxDetail",SqlDbType.Structured, DtblOCTaxDetail),
                    objprovider.CreateInitializedParameterTableType("@oc_amt",SqlDbType.NVarChar, oc_amt),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_InsertSalesReturn_Details", prmcontentaddupdate).ToString();

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
        public DataSet GetSalesReturnDetail(string Srt_no, string Srt_dt, string CompID, string BrchID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Srt_no",DbType.String, Srt_no),
                    objProvider.CreateInitializedParameter("@Srt_dt",DbType.String, Srt_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_SalesReturn$DetailView", prmContentGetDetails);
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
        public DataSet GetSalesReturnListAll(string CustId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string wfstatus, string UserID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    
                     
                                                         objProvider.CreateInitializedParameter("@CustId",DbType.String, CustId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                          objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetSalesReturnList$details]", prmContentGetDetails);
            return dt;//.Tables[0];
        }
        public DataSet SalesReturnCancel(SalesReturn_Model _SalesReturn_Model, string CompID, string br_id, string mac_id,string Src_Type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@srt_no",DbType.String,  _SalesReturn_Model.srt_no),
                    objProvider.CreateInitializedParameter("@srt_dt",DbType.Date,  _SalesReturn_Model.srt_dt),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _SalesReturn_Model.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@Narr",DbType.String, _SalesReturn_Model.SvNarr),
                     objProvider.CreateInitializedParameter("@src_type",DbType.String, Src_Type),
                     objProvider.CreateInitializedParameter("@cancel_remarks",DbType.String, _SalesReturn_Model.CancelledRemarks),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$SalesReturnCancel", prmContentGetDetails);

            return DS;
        }
        public DataSet SalesReturnDelete(SalesReturn_Model _SalesReturn_Model, string CompID, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                   objProvider.CreateInitializedParameter("@srt_no",DbType.String, _SalesReturn_Model.srt_no),
                    objProvider.CreateInitializedParameter("@srt_dt",DbType.Date,  _SalesReturn_Model.srt_dt),

                                                     };
                DataSet Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$SalesReturnDelete", prmContentGetDetails);
                return Deatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SalesReturnApprove(string SRTNo, string SRTDate, string userid, string wf_status, string wf_level, string wf_remarks,string CnNarr, string comp_id, string br_id, string mac_id, string DocID,string JVNurr)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@srt_no",DbType.String, SRTNo),
                    objProvider.CreateInitializedParameter("@srt_dt",DbType.Date,  SRTDate),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, userid ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                         objProvider.CreateInitializedParameter("@Narr",DbType.String, CnNarr),
                         objProvider.CreateInitializedParameter("@JVNurr",DbType.String, JVNurr),
                     };
                DataSet SRTDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$SalesReturnApprove", prmContentGetDetails);

                string DocNo = string.Empty;
                DocNo = SRTDeatils.Tables[0].Rows[0]["srt_detail"].ToString();
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

        public DataSet SR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag,string src_doc_no,string src_type)
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
                    objProvider.CreateInitializedParameter("@flag",DbType.String, Flag),
                    objProvider.CreateInitializedParameter("@src_doc_no",DbType.String, src_doc_no),
                    objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SalRtrn_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Shipment_GetSubItemDetails(string CompID, string Br_id, string ShipNo, string ItemId, string doc_no, string doc_dt, string Flag,string src_type)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@Ship_No",DbType.String, ShipNo),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                    //objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ByShipment_GetSubItemDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id, string flag, string ShNo, string ShDt)
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
                    objProvider.CreateInitializedParameter("@ShipNo",DbType.String, ShNo),
                    objProvider.CreateInitializedParameter("@ShipDate",DbType.String, ShDt),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SRT_GetSubItemAvlStockDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemDetailsFromSinv(string CompID, string BrchID, string SinvNo, string ShNo, string ShDt, string Item_id,string src_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@sinv_no",DbType.String, SinvNo),
                    objProvider.CreateInitializedParameter("@ship_no",DbType.String, ShNo),
                    objProvider.CreateInitializedParameter("@ship_dt",DbType.String, ShDt),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, Item_id),
                    objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                    //objProvider.CreateInitializedParameter("@rev_no",DbType.String, "PPlan"),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SRT_GetSubItemDetailsFromPInv", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet SRT_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string flag, string ShNo, string ShDt, string wh_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@srt_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@srt_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                      objProvider.CreateInitializedParameter("@ShipNo",DbType.String, ShNo),
                    objProvider.CreateInitializedParameter("@ShipDate",DbType.String, ShDt),
                    objProvider.CreateInitializedParameter("@wh_id",DbType.Int32, wh_id),
                      //objProvider.CreateInitializedParameter("@SrcDocNo",DbType.String, Srcdoc_no)
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SRTGetSubItemDetailsAftrApprove]", prmContentGetDetails);
            return ds;
        } 
        public DataSet GetSalesOrderDeatilsForPrint(string CompID, string br_id, string doc_no, string doc_dt,string Src_Type)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, br_id),                   
                      objProvider.CreateInitializedParameter("@srt_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@srt_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Src_Type",DbType.String, Src_Type),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sls$sales$return$print]", prmContentGetDetails);
            return ds;
        }
        public DataTable GetTransportDetails(string compId, string transId, string transType, string transMode)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@TransId",DbType.String,transId),
                     objProvider.CreateInitializedParameter("@Transtype",DbType.String,transType),
                     objProvider.CreateInitializedParameter("@TransMode",DbType.String,transMode),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Get$stp$trans$detail", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
