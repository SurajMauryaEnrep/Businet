using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.DomesticPurchaseInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.DomesticPurchaseInvoiceIService;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.DomesticPurchaseInvoiceService
{
    public class DomesticPurchaseInvoiceService : DomesticPurchaseInvoiceIService
    {
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID, string SuppType)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),          
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                     objProvider.CreateInitializedParameter("@SuppType",DbType.String, SuppType),
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
           
        }
        public DataSet GetPITaxListDAL(string CompId, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$tax$setup_GetTaxTypes", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet PI_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brchID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PI_GetSubItemDetails", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetGoodReceiptNoteList(string Supp_id, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mr$detail_GetGoodReceiptNoteList", prmContentGetDetails);
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
        public DataSet GetGoodReceiptNoteDetail(string GRNNo, string GRNDate, string CompId, string BrID,string supp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@GRNNo",DbType.String, GRNNo),
                    objProvider.CreateInitializedParameter("@GRNDate",DbType.String, GRNDate),
                    objProvider.CreateInitializedParameter("@supp_id",DbType.Int64, supp_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mr$detail_GetGoodReceiptNoteDetail", prmContentGetDetails);
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
        public DataTable CheckRoundOffAcc(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                };
                DataTable acc_flag = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_checkroundoffacc", prmContentGetDetails).Tables[0];
                return acc_flag;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string InsertPI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            ,DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            ,DataTable CRCostCenterDetails, DataTable DtblSubItemDetail,string Nurr, DataTable DtblTdsDetail
            , DataTable DtblOcTdsDetail,string TDS_Amount,DataTable DtblVarDetail, DataTable DtblVarTaxDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                    objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                    objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                    objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                    objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DtblIOCDetail),                                                        
                    objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                    objprovider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouDetail),
                    objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                    objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DtblOCTaxDetail),
                    objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CRCostCenterDetails),
                    objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, DtblSubItemDetail),
                    objprovider.CreateInitializedParameterTableType("@DtblTdsDetail",SqlDbType.Structured, DtblTdsDetail),
                    objprovider.CreateInitializedParameterTableType("@DtblOcTdsDetail",SqlDbType.Structured, DtblOcTdsDetail),
                    objprovider.CreateInitializedParameterTableType("@Nurr",SqlDbType.NVarChar, Nurr),
                    objprovider.CreateInitializedParameterTableType("@TDS_Amount",SqlDbType.NVarChar, TDS_Amount),
                    objprovider.CreateInitializedParameterTableType("@DtblVarDetail",SqlDbType.Structured, DtblVarDetail),/*Added by Suraj Maurya on 31-03-2025 for variance quantity details */
                    objprovider.CreateInitializedParameterTableType("@DtblVarTaxDetail",SqlDbType.Structured, DtblVarTaxDetail),/*Added by Suraj Maurya on 31-03-2025 for variance quantity tax details */
                                                    };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "prc$pinv$detail_InsertPI_Details", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[6].Value != DBNull.Value)
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
        public string Delete_PI_Detail(PI_ListModel _PI_ListModel, string InvType, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@Inv_No",DbType.String, _PI_ListModel.PI_inv_no ),
                                                        objProvider.CreateInitializedParameter("@Inv_Date",DbType.String,_PI_ListModel.PI_inv_dt),
                                                        objProvider.CreateInitializedParameter("@Inv_Type",DbType.String,InvType),
                };
                string GRNId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "prc$pinv$detail_DeletePI_Details", prmContentInsert).ToString();
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
        public string Approve_PI(string Inv_No, string Inv_Date, string Inv_Type, string MenuDocId, string Branch, string CompID
            , string UserID,string mac_id, string wf_status, string wf_level, string wf_remarks,string VoucherNarr,string Bp_Nurr
            ,string Dn_Nurr,string DN_VarianceNarr)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@invno",DbType.String, Inv_No),
                                                        objProvider.CreateInitializedParameter("@invdate",DbType.String, Inv_Date),
                                                        objProvider.CreateInitializedParameter("@invtype",DbType.String, Inv_Type ),
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
                                                         objProvider.CreateInitializedParameter("@Bp_Narr",DbType.String, Bp_Nurr),
                                                         objProvider.CreateInitializedParameter("@Dn_Narr",DbType.String, Dn_Nurr),
                                                         objProvider.CreateInitializedParameter("@DN_VarNarr",DbType.String, DN_VarianceNarr),

                };
                prmContentInsert[7].Size = 100;
                prmContentInsert[7].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$pinv$detail_Approved_PI_Details", prmContentInsert);

                string DocNo = string.Empty;
                if (prmContentInsert[7].Value != DBNull.Value)
                {
                    DocNo = prmContentInsert[7].Value.ToString();
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
        public DataSet Edit_PIDetail(string CompId, string BrID,string VouType, string PINo, string PIDate, string Type, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
                                                        objProvider.CreateInitializedParameter("@VouType",DbType.String, VouType),
                                                        objProvider.CreateInitializedParameter("@PINo",DbType.String, PINo),
                                                        objProvider.CreateInitializedParameter("@PIDate",DbType.String, PIDate),
                                                        objProvider.CreateInitializedParameter("@PIType",DbType.String, Type),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$pinv$detail_GetPI_Details", prmContentGetDetails);
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
        public DataSet CheckPIDetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckReturnAgainstPI", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVarienceDetails(string CompId, string BrchID, string GRNNo, string GRNDate,string ItmCode,string flag = "")
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@GRNNo",DbType.String, GRNNo),
                                                        objProvider.CreateInitializedParameter("@GRNDate",DbType.String, GRNDate),
                                                        objProvider.CreateInitializedParameter("@ItmCode",DbType.String, ItmCode),
                                                      };
                if (flag == "QtyDetails")
                {
                    DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$Pinv$GetVarienceQuantityDetail", prmContentGetDetails);
                    return GetDt;
                }
                else if (flag == "TaxDetails")
                {
                    DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$Pinv$GetVarienceQuantityTaxDetail", prmContentGetDetails);
                    return GetDt;
                }
                else
                {
                    DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$Pinv$GetVarienceDetail", prmContentGetDetails);
                    return GetDt;
                }
                
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPIAttatchDetailEdit(string CompID, string BrchID, string PI_No, string PI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@PI_No",DbType.String, PI_No),
                                                        objProvider.CreateInitializedParameter("@PI_Date",DbType.String, PI_Date),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$pi$getAttatchmentDetail", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPurchaseInvoiceDeatilsForPrint(string CompID, string BrchID, string PI_No, string PI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, PI_No),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, PI_Date),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPurchaseInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_OC_List;
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

                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PI_GetGLDetail", prmcontentaddupdate);
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
        public DataSet getEinvoiceno_ewbNo(string CompID, string BrchID, string grnno, string GRN_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@grnno",DbType.String, grnno),
                                                        objProvider.CreateInitializedParameter("@GRN_Date",DbType.String, GRN_Date),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get$einvoiceno$ewbno", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPurchaseInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date, string inv_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, SI_No),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, SI_Date),
                                                        objProvider.CreateInitializedParameter("@flag",DbType.String, inv_type),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPurchasInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
    public class DomesticPurchaseInvoiceListService : DomesticPurchaseInvoiceListIService
    {
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
        public DataSet GetPI_DetailList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus)
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
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$pinv$detail_GetPI_DeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
