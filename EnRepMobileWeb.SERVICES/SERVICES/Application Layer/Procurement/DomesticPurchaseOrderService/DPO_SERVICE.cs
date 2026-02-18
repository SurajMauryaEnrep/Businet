using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.DomesticPurchaseOrder;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.DomesticPurchaseOrder
{
    public class DPODetail_SERVICE: DPODetail_ISERVICE
    {
        public Dictionary<string, string> GetSupplierListDAL(string CompID, string SuppName, string BranchID, string SuppType)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, SuppType),
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
            return null;
        }
        public DataSet CheckDeliveryNoteDPO(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$dn$detail_CheckDeliveryNoteAgainstDPO", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GetPOItemListDAL(string CompID, string BrID, string ItmName)
        {
            Dictionary<string, string> ddlItmListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetItemNameList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItmListDic.Add(PARQusData.Tables[0].Rows[i]["Item_id"].ToString(), PARQusData.Tables[0].Rows[i]["Item_name"].ToString());
                    }
                }
                return ddlItmListDic;

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
        public DataSet GetPOItmListDAL(string CompID, string BrID, string ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetItemNameList", prmContentGetDetails);

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
        public DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId)
        {
            try
            {
                DataSet searchmenu = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };           
                     searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppAddrDetails", prmContentGetDetails);
             

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
        public DataSet GetSuppContectDetail(string Supp_id, string CompId)
        {
            try
            {
                DataSet searchmenu = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };               
                 searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppContectDetails", prmContentGetDetails);

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
        public DataSet GetPOItemDetailDAL(string ItemID, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetPOItemDetails", prmContentGetDetails);
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
        public DataSet GetPODetailDAL(string CompId, string BrID, string PONo, string PODate,string UserID,string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@LPONo",DbType.String, PONo),
                                                        objProvider.CreateInitializedParameter("@LPODate",DbType.String, PODate),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$po$detail_GetPOAllDetails", prmContentGetDetails);
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
        public DataSet GetSuppExRateDetailDAL(string Curr_id, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CurrID",DbType.String, Curr_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppExRateDetails", prmContentGetDetails);
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
        public DataSet GetPOItemUOMDAL(string Item_id, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, Item_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetPOItemUOMDetails", prmContentGetDetails);
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
        public DataSet GetsuppcurrDAL(string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                      };
                DataSet Getsuppcurr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$curr_GetPOcurr", prmContentGetDetails);
                return Getsuppcurr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataSet GetPOTaxListDAL(string CompId, string BrchID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
        //                                              };
        //        DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$tax$setup_GetTaxTypes", prmContentGetDetails);
        //        return GetTaxList;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public DataSet GetPODetailListDAL(string CompId, string BrchID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
        //                                              };
        //        DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$po$detail_GetPoDeatilList", prmContentGetDetails);
        //        return GetPODetailList;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public DataSet GetPOTaxPercentageDAL(string CompId, string BrchID, string TaxID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
        //                                                objProvider.CreateInitializedParameter("@TaxID",DbType.String, TaxID),
        //                                              };
        //        DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$tax$setup_GetTaxPercent", prmContentGetDetails);
        //        return GetTaxList;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public DataSet GetOtherChargeDAL(string CompId, string BrchID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
        //                                              };
        //        DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$oc$setup_GetOtherChargeList", prmContentGetDetails);
        //        return Get_OC_List;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public DataSet GetWhouseDAL(string CompId)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
        //                                              };
        //        DataSet GetWhouseList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse$detail_Getwarehouse", prmContentGetDetails);
        //        return GetWhouseList;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}

        public string InsertDPOTransactionDetails(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail, DataTable DTOCDetail, DataTable DtblOCTaxDetail, DataTable DTDeliSchDetail, DataTable DTTermsDetail, DataTable DTAttachmentDetail, DataTable DtblSubItem,string req_area)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DTOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DeliSchDetail",SqlDbType.Structured, DTDeliSchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TermsDetail",SqlDbType.Structured,DTTermsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DTAttachmentDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,DtblSubItem),
                                                        objprovider.CreateInitializedParameterTableType("@OC_TaxDetail",SqlDbType.Structured,DtblOCTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@req_area",SqlDbType.Int, req_area),
                                                    };
                prmcontentaddupdate[7].Size = 100;
                prmcontentaddupdate[7].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "prc$po$detail_InsertDPOTransactionDetails", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[7].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[7].Value.ToString();
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
        public string InsertForwardDetails(string compid,string brid,string docid,string docno,string docdate,string status,string forwarededto,string forwardedby,string level,string remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                       objProvider.CreateInitializedParameter("@comp_id",DbType.String, compid),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, brid ),
                                                        objProvider.CreateInitializedParameter("@doc_id",DbType.String, docid),
                                                        objProvider.CreateInitializedParameter("@doc_no",DbType.String,docno),
                                                        objProvider.CreateInitializedParameter("@doc_date",DbType.String,docdate),
                                                        objProvider.CreateInitializedParameter("@status",DbType.String, status),
                                                        objProvider.CreateInitializedParameter("@level",DbType.String, level),
                                                        objProvider.CreateInitializedParameter("@forwarded_to",DbType.String, forwarededto ),
                                                        objProvider.CreateInitializedParameter("@forwarded_by",DbType.String, forwardedby),
                                                        objProvider.CreateInitializedParameter("@remarks",DbType.String,remarks),
                                                        objProvider.CreateInitializedParameter("@fStatus",DbType.String,""),
                                                    };
                prmcontentaddupdate[10].Size = 100;
                prmcontentaddupdate[10].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "stp$app$wf$detail_InsertForwardDetail", prmcontentaddupdate).ToString();
                string fstatus = string.Empty;
                if (prmcontentaddupdate[10].Value != DBNull.Value) // status
                {
                    fstatus = prmcontentaddupdate[10].Value.ToString();
                }

                return fstatus;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }

        public string PO_DeleteDAL(string CompID, string BrID, string PONo, string PODate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@PONo",DbType.String, PONo ),
                                                        objProvider.CreateInitializedParameter("@PODate",DbType.String,PODate),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "prc$po$_DeleteAllSectionDetails", prmContentInsert).ToString();
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
        public string InsertPOApproveDetailsDAL(string PONo, string PODate, string Branch,string src_doc_number,string src_doc_date, string MenuID, string CompID, string ApproveID,string mac_id,string wf_status, string wf_level, string wf_remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@docno",DbType.String, PONo),
                                                        objProvider.CreateInitializedParameter("@docdate",DbType.String, PODate),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String, Branch),
                                                        objProvider.CreateInitializedParameter("@src_doc_number",DbType.String, src_doc_number),
                                                        objProvider.CreateInitializedParameter("@src_doc_date",DbType.String, src_doc_date),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String,MenuID),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,ApproveID),
                                                        objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                        objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "prc$po$detail_UpdatePOAppovedDetails", prmContentInsert).ToString();
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
        public DataTable GetSuppAddressdetail(string SuppID, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, SuppID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetSupplierAddressDetail", prmContentGetDetails).Tables[0];
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

        public DataSet GetItemSupplierInfo(string ItemID, string SuppID, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                         objProvider.CreateInitializedParameter("@SuppID",DbType.String, SuppID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetSuppInfoDetails", prmContentGetDetails);
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
        public DataSet GetSourceDocList(string Comp_ID, string Br_ID, string Flag,string SuppID,string ItemType,string RequiredArea)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, SuppID),
                                                        objProvider.CreateInitializedParameter("@ItemType",DbType.String, ItemType),
                                                        objProvider.CreateInitializedParameter("@RequiredArea",DbType.String, RequiredArea),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_QuotationAndRequesitionList", prmContentGetDetails);
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
        public DataSet GetPurchaseOrderDeatils(string Comp_ID, string Br_ID, string OrderNo, string OrderDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@po_no",DbType.String, OrderNo),
                                                        objProvider.CreateInitializedParameter("@po_date",DbType.String, OrderDate),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPurchaseOrderDeatils_ForPrint", prmContentGetDetails);
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
        public DataSet GetDetailsAgainstQuotationOrPR(string Comp_ID, string Br_ID, string Doc_no, string Doc_date, string Src_Type,string Ordertype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.Int32, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.Int32, Br_ID),
                                                         objProvider.CreateInitializedParameter("@Doc_no",DbType.String, Doc_no),
                                                         objProvider.CreateInitializedParameter("@Doc_date",DbType.String, Doc_date),
                                                        objProvider.CreateInitializedParameter("@Src_Type",DbType.String, Src_Type),
                                                        objProvider.CreateInitializedParameter("@Ordertype",DbType.String, Ordertype),
                                                       
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_DetailsAgainstQuotationOrPR", prmContentGetDetails);
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

        public DataSet CheckLPOQty_ForceClosed(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$po$detail_CheckDPOQty_Forceclosed", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOAttatchDetailEdit(string CompID, string BrchID, string PO_No, string PO_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@PO_No",DbType.String, PO_No),
                                                        objProvider.CreateInitializedParameter("@PO_Date",DbType.String, PO_Date),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$po$getAttatchmentDetail", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet PO_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, compID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, brchID),
                                                        objProvider.CreateInitializedParameter("@doc_no",DbType.String, doc_no),
                                                        objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                                                        objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                                                        objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$po$getSubitemDetailAfterApprove", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetQTandPRSubItemwithWhAvlstock(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag,string Src_Type)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@Br_ID",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@Doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@Doc_date",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                    objProvider.CreateInitializedParameter("@Src_Type",DbType.String, Src_Type),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PO_GetQTandPRSubItemwithWhAvlstock", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetQTandPRSubItemwithWhAvlstockAftrInsrt(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@Br_ID",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@Doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@Doc_date",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PO_GetQTandPRSubItemwithWhAvlstockAftrInsrt", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetRequirmentreaList(string CompId, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[prc$pr$GetRequirmentreaList]", prmContentGetDetails).Tables[0];
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
        public DataSet getReplicateWith(string CompID, string br_id, string OrderType, string SarchValue,string Item_type) 
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@order_type", DbType.String, OrderType),
                     objProvider.CreateInitializedParameter("@SarchValue", DbType.String, SarchValue),
                     objProvider.CreateInitializedParameter("@Item_type", DbType.String, Item_type)
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc_PO$replicate$item", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSPLDetails(string comp_id, string br_id, string supp_id, string ItemID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comp_id),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                                                        objProvider.CreateInitializedParameter("@supp_id",DbType.Int64, supp_id),
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSPLDetailForPO", prmContentGetDetails);
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
    public class DPOList_SERVICE: DPOList_ISERVICE
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
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID, string SuppType)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, SuppType),
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
            return null;
        }
       public DataSet GetAllData(string CompID, string SuppName, string BranchID, string SuppType,string User_ID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus, string item_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                       objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, SuppType),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, User_ID),
                      objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                     objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                   objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                   objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                   objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),                  
                   objProvider.CreateInitializedParameter("@item_type",DbType.String, item_type),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$po$List", prmContentGetDetails);
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
        public DataSet GetPODetailList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus,string SuppType,string item_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                                                          objProvider.CreateInitializedParameter("@SuppType",DbType.String, SuppType),
                                                          objProvider.CreateInitializedParameter("@item_type",DbType.String, item_type),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$po$detail_GetPoDeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPO_Detail(string CompId, string BrID, string PONo, string PODate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, PONo),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, PODate),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$po$detail_GetPurchaseOrder_RelDetails", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetWFLevel_Detail(string CompId, string BrID, string PONo, string PODate, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@PONo",DbType.String, PONo),
                                                        objProvider.CreateInitializedParameter("@PODate",DbType.String, PODate),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$app$wf$detail_GetWF_LevelDetails", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetPOTrackingDetail(string CompId, string BrID, string PONo, string PODate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@PoNo",DbType.String, PONo),
                                                        objProvider.CreateInitializedParameter("@Podt",DbType.String, PODate),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPOTrackingView", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetPendingDocumentData(string CompID, string BrchID, string Docid, string language, string SrcType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                                                        objProvider.CreateInitializedParameter("@language",DbType.String, language),
                                                        objProvider.CreateInitializedParameter("@SrcType",DbType.String, SrcType),
                                                      };
                DataSet PendingDocument = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$pending$Data$PO$List", prmContentGetDetails);
                return PendingDocument;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetPendingDocumentDataitemdetail(string CompID, string BrchID, string doc_no, string doc_dt,string flag)
        {
            try
            {
                SqlDataProvider dataobject = new SqlDataProvider();
                SqlParameter[] miobject ={
                 dataobject.CreateInitializedParameter("@comid",DbType.String,CompID),
                dataobject.CreateInitializedParameter("@brid",DbType.String,BrchID),
                 dataobject.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                 dataobject.CreateInitializedParameter("@doc_dt",DbType.String,doc_dt),
                    dataobject.CreateInitializedParameter("@flag",DbType.String,flag),
                };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$Pending$Document$PO$item$detail]", miobject);
                return dt;
            }
            catch (SqlException EX)
            {
                throw EX;
            }
        }
    }
}
