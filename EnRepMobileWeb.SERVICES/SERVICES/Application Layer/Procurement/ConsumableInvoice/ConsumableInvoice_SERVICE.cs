using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ConsumableInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ConsumableInvoice;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.ConsumableInvoice
{
    public class ConsumableInvoice_SERVICE: ConsumableInvoice_ISERVICE
    {
        public DataTable Getsuppplier(string CompID, string GroupName,string type,string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, 'D'),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    // objProvider.CreateInitializedParameter("@SuppName",DbType.String, GroupName),
                    //objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                    };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppList", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckCIDetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckReturnAgainst_CI", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllData(string CompID, string GroupName,string type,string BrchID, string suppID, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, 'D'),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@SuppId",DbType.String,suppID),
                     objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                  objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                  objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@Docid",DbType.String, DocumentMenuId),
                                                    };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$Con$Inv$List", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertCI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DTOCDetail, DataTable DTAttachmentDetail, DataTable DtblVouDetail,DataTable DtblOCTaxDetail
            ,DataTable CRCostCenterDetails,string Nurr,DataTable DtblTdsDetail,string tds_amt, DataTable DtblOcTdsDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DTOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DTAttachmentDetail),
                                                        objprovider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DtblOCTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CRCostCenterDetails),
                                                        objprovider.CreateInitializedParameterTableType("@Narration",SqlDbType.NVarChar, Nurr),
                                                        objprovider.CreateInitializedParameterTableType("@DtblTdsDetail",SqlDbType.Structured, DtblTdsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@tds_amt",SqlDbType.NVarChar, tds_amt),
                                                        objprovider.CreateInitializedParameterTableType("@DtblOcTdsDetail",SqlDbType.Structured, DtblOcTdsDetail),
                                                    };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "prc$cinv$detail_InsertCI_Details", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[6].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[6].Value.ToString();
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
        public DataSet GetCIDetailList(string CompId, string BrchID, string suppID, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String,suppID),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@Docid",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetMRSList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[prc$cinv$detail_GetCI_DeatilList]", prmContentGetDetails);
                return GetMRSList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Edit_CIDetail(string CompId, string BrID,string VouType, string inv_no, string inv_dt, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
                                                        objProvider.CreateInitializedParameter("@VouType",DbType.String, VouType),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, inv_no),
                                                        objProvider.CreateInitializedParameter("@inv_dt",DbType.String, inv_dt),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$cinv$detail_GetCI_Details", prmContentGetDetails);
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
        public string Delete_CI_Detail(ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@Inv_No",DbType.String, _ConsumableInvoiceDetails.inv_no ),
                                                        objProvider.CreateInitializedParameter("@Inv_Date",DbType.String,_ConsumableInvoiceDetails.inv_dt),
                };
                string GRNId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "prc$cinv$detail_DeleteCI_Details", prmContentInsert).ToString();
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
        public string Approve_CI(string Inv_No, string Inv_Date, string MenuDocId, string Branch
            , string CompID, string UserID, string mac_id, string wf_status, string wf_level
            , string wf_remarks, string VoucherNarr,string curr_id,string conv_rate,string Bp_Nurr,string Dn_Narr)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@invno",DbType.String, Inv_No),
                                                        objProvider.CreateInitializedParameter("@invdate",DbType.String, Inv_Date),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String, MenuDocId),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String,Branch),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                         objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                                                         objProvider.CreateInitializedParameter("@VoucherNarr",DbType.String, VoucherNarr),
                                                         objProvider.CreateInitializedParameter("@curr_id",DbType.String, curr_id),
                                                         objProvider.CreateInitializedParameter("@conv_rate",DbType.String, conv_rate),
                                                         objProvider.CreateInitializedParameter("@Bp_Narr",DbType.String, Bp_Nurr),
                                                         objProvider.CreateInitializedParameter("@Dn_Narr",DbType.String, Dn_Narr),

                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$cinv$detail_Approved_CI_Details", prmContentInsert);

                string DocNo = string.Empty;
                if (prmContentInsert[6].Value != DBNull.Value)
                {
                    DocNo = prmContentInsert[6].Value.ToString();
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
        public DataSet GetSourceDocList(string Comp_ID, string BrchID, string SuppID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrchID),                                                       
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, SuppID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$CN$GetSourceDocument", prmContentGetDetails);
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
        public DataSet getdataPOitemtabledata(string CompID, string BrchID, string srdocNo, string srcdoc_dt, string suppid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrchID),                                                       
                                                        objProvider.CreateInitializedParameter("@srdocNo",DbType.String, srdocNo),
                                                        objProvider.CreateInitializedParameter("@srcdoc_dt",DbType.String, srcdoc_dt),
                                                        objProvider.CreateInitializedParameter("@suppid",DbType.String, suppid),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get$PO$item$table$data$Consu$invoice", prmContentGetDetails);
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
        public DataSet GetDataBillNoBillDate(string CompID, string BrchID, string srdocNo, string srcdoc_dt, string suppid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrchID),                                                       
                                                        objProvider.CreateInitializedParameter("@srdocNo",DbType.String, srdocNo),
                                                        objProvider.CreateInitializedParameter("@srcdoc_dt",DbType.String, srcdoc_dt),
                                                        objProvider.CreateInitializedParameter("@suppid",DbType.String, suppid),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Bill$no$date$Cinv", prmContentGetDetails);
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
        public DataSet GetConsumbleInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, SI_No),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, SI_Date)
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetConsubableInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
