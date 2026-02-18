using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.Shipment;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.Shipment;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.Shipment
{
    public class Shipment_SERVICES : Shipment_ISERVICES
    {
        public Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID, string CustType)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@CustName",DbType.String, Cust_Name),
                             objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                             };

                //DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$cust$detail_GetCustList", prmContentGetDetails);
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
        public DataTable GetCustomer_List(string CompID, string Cust_Name, string BrchID, string CustType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@CustName",DbType.String, Cust_Name),
                             objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                      };
                //DataTable Get_Cust_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$cust$detail_GetCustList", prmContentGetDetails).Tables[0];
                DataTable Get_Cust_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetCustList", prmContentGetDetails).Tables[0];
                return Get_Cust_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllData(string CompID, string Cust_Name, string BrchID, string CustType,
             string CustID, string Fromdate, string Todate, string Status, string DocumentMenuId, string UserID, string wfstatus,string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@CustName",DbType.String, Cust_Name),
                             objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                               objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.DateTime, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.DateTime, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),              
                 objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                   objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                         objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                         objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                      };
                DataSet Get_Cust_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$shipment$list$data", prmContentGetDetails);
                return Get_Cust_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomerAddress(string comp_id, string CustID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_id),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$cust$detail_GetCustAddrDetails]", prmContentGetDetails);
            return ds;
        }
        public DataSet getShipmentPackingList(string CompID, string BrchID, string Cust_id, string PackNumber)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@Cust_id",DbType.String, Cust_id),
                    objProvider.CreateInitializedParameter("@PackNumber",DbType.String, PackNumber),                         };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$Shipment$GetPackingListNumber]", prmContentGetDetails);
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
        public DataSet getDetailPckingListByPackNo(string CompID, string BrchID, string Pack_NO, string Pack_date, string DocumentMenuId)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = 
                    {/*Passing perameter to sotore procedure*/   
                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@Pack_NO",DbType.String, Pack_NO),
                        objProvider.CreateInitializedParameter("@Pack_date",DbType.Date,Convert.ToDateTime(Pack_date)),
                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                    };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$Shipment$GetPackingListItemDetail]", prmContentGetDetails);
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
        public DataTable getcurr_Detail(string CompID, string BrchID, string Pack_NO, string Pack_date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                    {/*Passing perameter to sotore procedure*/   
                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@Pack_NO",DbType.String, Pack_NO),
                        objProvider.CreateInitializedParameter("@Pack_date",DbType.Date,Convert.ToDateTime(Pack_date)),
                    };
                DataTable curr_Data = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$Shipment$GetcurrDetail", prmContentGetDetails).Tables[0];
                return curr_Data;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet ShipmentApprove(ShipmentDetail_MODEL _ShipmentDetail_MODEL, string CompID, string br_id, string mac_id,string menuid, string A_Status, string A_Level, string A_Remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16,CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String,br_id),
                    objProvider.CreateInitializedParameter("@ship_no",DbType.String,_ShipmentDetail_MODEL.ship_no),
                    objProvider.CreateInitializedParameter("@ship_dt",DbType.Date,_ShipmentDetail_MODEL.ship_dt),
                    objProvider.CreateInitializedParameter("@ship_type",DbType.Date,_ShipmentDetail_MODEL.ship_type),
                    objProvider.CreateInitializedParameter("@menuid",DbType.String,menuid ),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String,_ShipmentDetail_MODEL.CreatedBy ),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String,mac_id),
                    objProvider.CreateInitializedParameter("@wf_status",DbType.String,A_Status),
                    objProvider.CreateInitializedParameter("@wf_level",DbType.String,A_Level),
                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String,A_Remarks),
                    //objProvider.CreateInitializedParameter("@Narr",DbType.String,Narr),
                };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$ShipmentApprove", prmContentGetDetails);
                return ImageDeatils; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckSaleInvoiceAgainstShipment(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ship$detail_CheckSaleInvoiceAgainstShipment", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet ShipmentDelete(ShipmentDetail_MODEL _ShipmentDetail_MODEL, string CompID, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@ship_no",DbType.String, _ShipmentDetail_MODEL.ship_no),
                    objProvider.CreateInitializedParameter("@ship_dt",DbType.Date,  _ShipmentDetail_MODEL.ship_dt),
                    objProvider.CreateInitializedParameter("@ship_type",DbType.String,  _ShipmentDetail_MODEL.ship_type),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$ShipmentDelete", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertUpdateShipment(DataTable ShipmentHeader, DataTable ShipmentItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable dtSubItem, DataTable ShipmentAttachments,DataTable ShipmentTranspoterDetails)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, ShipmentHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, ShipmentItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemBatchDetails",SqlDbType.Structured,ItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemSerialDetails",SqlDbType.Structured,ItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,ShipmentAttachments ),
                 //objprovider.CreateInitializedParameterTableType("@Voudetail",SqlDbType.Structured,Voudetail ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                  objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem ),
                  objprovider.CreateInitializedParameterTableType("@TranspoterDetail",SqlDbType.Structured,ShipmentTranspoterDetails ),
                };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$shipment$InsertUpdateShipment", prmcontentaddupdate).ToString();

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
        public DataSet getDocumentStatus(string MenuDocumentId)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@menu_id",DbType.String, MenuDocumentId),                         };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[fct$DocStatus]", prmContentGetDetails);
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
        public DataSet GetShipmentListAll(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId,
            string FromDate, string ToDate)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                         objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                         objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                         objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                         objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                         objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate)
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$GetShipmentListAll$details", prmContentGetDetails);
            return ds;
        }
        public DataTable GetShipmentListByFilter(string CustID, DateTime Fromdate, DateTime Todate, string Status, string CompID, string BrchID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.DateTime, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.DateTime, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                 objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                     };
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$ShipmentList$detailByFilter", prmContentGetDetails).Tables[0];
            return dt;
        }
        public DataTable GetCustNameList(string CompId, string br_id, string CustomerName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetCustList]", prmContentGetDetails).Tables[0];
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
        public DataSet getShipmentDetailByShipmentNo(string CompID, string BrchID, string UserID, string ShipmentNumber, DateTime ShipmentDate, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                objProvider.CreateInitializedParameter("@ShipmentNumber",DbType.String,ShipmentNumber),
                objProvider.CreateInitializedParameter("@ShipmentDate",DbType.Date,ShipmentDate),
                objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
        };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$ShipmentDetail$detailByNo", prmContentGetDetails);
            return ds;
        }   
        public string getNextDocumentNumber(string CompID, string BrchID, string MenuDocumentId, string Prefix)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@MenuDocumentId",DbType.String,MenuDocumentId),
                objProvider.CreateInitializedParameter("@Prefix",DbType.String,Prefix),
               };

            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$GetDocumentNextNUmber]", prmContentGetDetails).Tables[0];
            string documentNo = dt.Rows[0]["Column1"].ToString();
            return documentNo;
        }
        public DataSet ShipmentCancel(ShipmentDetail_MODEL _ShipmentDetail_MODEL, string CompID, string br_id, string mac_id,string DocMenuID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                 objProvider.CreateInitializedParameter("@ship_no",DbType.String,  _ShipmentDetail_MODEL.ship_no),
                 objProvider.CreateInitializedParameter("@ship_dt",DbType.Date,  _ShipmentDetail_MODEL.ship_dt),
                 objProvider.CreateInitializedParameter("@ship_type",DbType.Date,  _ShipmentDetail_MODEL.ship_type),
                 objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _ShipmentDetail_MODEL.CreatedBy ),
                 objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                 objProvider.CreateInitializedParameter("@DocMenuid",DbType.String, DocMenuID),
                 objProvider.CreateInitializedParameter("@cancel_remarks",DbType.String, _ShipmentDetail_MODEL.CancelledRemarks),
            };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$ShipmentCancel", prmContentGetDetails);

            return DS;
        }
        public DataSet GetShipmentDeatilsForPrint(string CompId, string BrchID, string DocNo, string DocDate,string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@ship_no",DbType.String, DocNo),
                    objProvider.CreateInitializedParameter("@ship_date",DbType.Date,  DocDate),
                    objProvider.CreateInitializedParameter("@flag",DbType.String,  flag),
                    //objProvider.CreateInitializedParameter("@ship_type",DbType.String,  ),
                                                     };
                DataSet Deatils = new DataSet();
                //if (CompId == "1")
                //{
                    //Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "AE_GetShipmentDeatils_ForPrint", prmContentGetDetails);
                //}
                //else
                //{
                    Deatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetShipmentDeatils_ForPrint", prmContentGetDetails);
                //}
                 
                return Deatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Shipment_GetSubItemDetails(string CompID, string Br_id, string ItemId, string SrcDoc_no, string SrcDoc_dt, string doc_no, string doc_dt, string Flag,string DocumentMenuId)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@Pack_no",DbType.String,SrcDoc_no),
                    objProvider.CreateInitializedParameter("@Pack_dt",DbType.String, SrcDoc_dt),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Shipment_GetSubItemDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Shipment_GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string DocumentMenuId)
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
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Ship_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable PortOfLoadingList()
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            //SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                
                                                     //};
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPortOfLoadingList").Tables[0];
            return dt;
        }
        public DataTable PlOfReceiptByPreCarrierList()
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            //SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 

            //};
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPlOfReceiptByPreCarrierList").Tables[0];
            return dt;
        }
        public DataTable GetShipmentItemsToExportExcel(string compId, string brId, string shipmentNo, string shipmentDate)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompId",DbType.Int32, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int32, brId),
                    objProvider.CreateInitializedParameter("@ShipmentNo",DbType.String, shipmentNo),
                    objProvider.CreateInitializedParameter("@ShipmentDate",DbType.String,shipmentDate),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetShipmentItemsToExportExcel", prmContentGetDetails);
                return Getsuppport.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string GetCstmInvNo(string CompID, string BrchID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
               };
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetCustomeInvoiceNumber]", prmContentGetDetails).Tables[0];
            string documentNo = dt.Rows[0]["CustInvNo"].ToString();
            return documentNo;
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
        //public DataSet GetRoundOffGLDetails(string comp_id, string br_id)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //            objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
        //            objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
        //             };
        //        DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetRoundOffGLDetail", prmContentGetDetails);
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

        //public DataSet GetWarehouseList(string CompId, string BrID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //            objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
        //        };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse_GetWarehouseList", prmContentGetDetails);
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
        //public string getWarehouseWiseItemStock(string CompID, string BrID, string Wh_ID, string ItemID, string LotID, string BatchNo)
        //{
        //    string AvaiableStock="0";
        //    SqlDataProvider objProvider = new SqlDataProvider();
        //    SqlParameter[] prmContentGetDetails = {
        //    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
        //    objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
        //    objProvider.CreateInitializedParameter("@Wh_ID",DbType.Int32,  Wh_ID),
        //    objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemID),
        //    objProvider.CreateInitializedParameter("@LotID",DbType.String,  LotID),
        //    objProvider.CreateInitializedParameter("@BatchNo",DbType.String,  BatchNo),
        //    };
        //    DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetAvlStock_wh]", prmContentGetDetails);
        //    if(DS.Tables[0].Rows.Count>0)
        //     AvaiableStock = DS.Tables[0].Rows[0]["wh_avl_stk_bs"].ToString();

        //    return AvaiableStock;
        //}
        //public DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId)
        //{
        //    SqlDataProvider objProvider = new SqlDataProvider();
        //    SqlParameter[] prmContentGetDetails = {
        //    objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
        //    objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
        //    objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
        //    objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),         
        //    };
        //    DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockBatchwise]", prmContentGetDetails);
        //    return DS;
        //}
        //public DataSet getItemStockBatchWiseAfterInsert(string CompID, string BrID, string Sh_Type, string Sh_No, string Sh_Date, string ItemId)
        //{
        //    SqlDataProvider objProvider = new SqlDataProvider();
        //    SqlParameter[] prmContentGetDetails = {
        //    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
        //    objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
        //    objProvider.CreateInitializedParameter("@ShipType",DbType.String,  Sh_Type),
        //    objProvider.CreateInitializedParameter("@ShipNo",DbType.String,  Sh_No),
        //    objProvider.CreateInitializedParameter("@ShipDate",DbType.String,  Sh_Date),
        //    objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
        //    };
        //    DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ship$item$bt$detail_Get_ShipItem_StockBatchwise", prmContentGetDetails);
        //    return DS;
        //}
        //public DataSet getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId)
        //{
        //    SqlDataProvider objProvider = new SqlDataProvider();
        //    SqlParameter[] prmContentGetDetails = {
        //    objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
        //    objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
        //    objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
        //    objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
        //    };
        //    DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockSerialwise]", prmContentGetDetails);
        //    return DS;
        //}
        //public DataSet getItemstockSerialWiseAfterInsert(string CompID, string BrID, string Sh_Type, string Sh_No, string Sh_Date, string ItemId)
        //{
        //    SqlDataProvider objProvider = new SqlDataProvider();
        //    SqlParameter[] prmContentGetDetails = {
        //    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
        //    objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
        //    objProvider.CreateInitializedParameter("@ShipType",DbType.String,  Sh_Type),
        //    objProvider.CreateInitializedParameter("@ShipNo",DbType.String,  Sh_No),
        //    objProvider.CreateInitializedParameter("@ShipDate",DbType.String,  Sh_Date),
        //    objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemId),
        //    };
        //    DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ship$item$sr$detail_Get_ShipItem_StocksSerialwise", prmContentGetDetails);
        //    return DS;
        //}
        //public DataSet getItemstockWarehouseWise(string ItemId, string CompId, string BranchId)
        //{
        //    SqlDataProvider objProvider = new SqlDataProvider();
        //    SqlParameter[] prmContentGetDetails = {
        //    objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
        //    objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
        //    objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
        //    };
        //    DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockWareHousewise]", prmContentGetDetails);
        //    return DS;
        //}
    }

}
