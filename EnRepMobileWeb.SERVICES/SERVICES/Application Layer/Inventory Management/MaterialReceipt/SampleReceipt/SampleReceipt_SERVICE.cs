using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.SampleReceipt;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.SampleReceipt;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialReceipt.SampleReceipt
{
   public class SampleReceipt_SERVICE: SampleReceipt_ISERVICE
    {
        public DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType,string source_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
              
                if (source_type == "A")  //Modified By Nitesh on 10-10-2023 16:42  add Condtion accoding to Source_type and item for supplier and Customer 
                {
                    SqlParameter[] prmContentGetDetailss = {/*Passing perameter to sotore procedure*/  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@itemName",DbType.String, EntityType),                   
                      objProvider.CreateInitializedParameter("@source_type",DbType.String, source_type),
                     };
                    DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Against$issue$Supp$CustList", prmContentGetDetailss);
                    return PARQusData;
                }
                else
                {
                    SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@EntityName",DbType.String, EntityName),
                      objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),
                     };
                    DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_Supp_CustList", prmContentGetDetails);
                    return PARQusData;
                }
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

        public DataSet GetWarehouseList(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse_GetWarehouseList", prmContentGetDetails);
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
        public DataSet getSampleRcptDetail(string CompID, string BrchID, string SampleRcptNumber, string SampleRcptDate)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@SR_No",DbType.String,SampleRcptNumber),
                objProvider.CreateInitializedParameter("@SR_Date",DbType.String,SampleRcptDate),
        };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$sr$detail_GetSampleReceipt_Details", prmContentGetDetails);
            return ds;
        }
       
        public DataSet GetSampleRcptItmList(string CompID, string BrID, string ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetSR_ItemList", prmContentGetDetails);
                return PARQusData;
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
        public DataSet GetItemUOMDAL(string Item_id, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, Item_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetItemUOMDetails", prmContentGetDetails);
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
        public string Delete_SR_Detail(SampleReceiptModel _SRModel, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@SR_No",DbType.String,_SRModel.srcpt_no ),
                                                        objProvider.CreateInitializedParameter("@SR_Date",DbType.Date,_SRModel.srcpt_dt),
                };
                string SRId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "inv$sr$detail_DeleteSampleReceipt_Details", prmContentInsert).ToString();
                return SRId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string Approve_SampleReceipt(SampleReceiptModel _SRModel, string CompID, string br_id, string SR_Date,string wf_status, string wf_level, string wf_remarks, string mac_id, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    
                            objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
                            objProvider.CreateInitializedParameter("@brid",DbType.String, br_id),
                            objProvider.CreateInitializedParameter("@srno",DbType.String, _SRModel.srcpt_no),
                            objProvider.CreateInitializedParameter("@srdate",DbType.String,SR_Date),
                            objProvider.CreateInitializedParameter("@userid",DbType.String,_SRModel.CreatedBy ),
                            objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                            objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                            objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                            objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                            objProvider.CreateInitializedParameter("@menuid",DbType.String, DocID),
                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$sr$detail_Approved_SampleReceipt_Details", prmContentGetDetails);
                string SRDetail = ds.Tables[0].Rows[0]["Result"].ToString();
                return SRDetail; 
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        //public string Approve_SampleReceipt(string SR_No, string SR_Date, string MenuID, string Branch, string CompID, string ApproveID, string mac_id, string wf_status, string wf_level, string wf_remarks)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentInsert = {
        //                                                objProvider.CreateInitializedParameter("@srno",DbType.String, SR_No),
        //                                                objProvider.CreateInitializedParameter("@srdate",DbType.String, SR_Date),
        //                                                objProvider.CreateInitializedParameter("@menuid",DbType.String,MenuID),
        //                                                objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
        //                                                objProvider.CreateInitializedParameter("@brid",DbType.String,Branch),
        //                                                objProvider.CreateInitializedParameter("@userid",DbType.String,ApproveID),
        //                                                 objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
        //                                                   objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
        //                                                objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
        //                                                objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
        //        };
        //        //DataSet SR_Detail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$sr$detail_Approved_SampleReceipt_Details", prmContentInsert);
        //        //return SR_Detail;
        //        DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$sr$detail_Approved_SampleReceipt_Details", prmContentInsert);
        //        string SRDetail = ds.Tables[0].Rows[0]["Result"].ToString();

        //        return SRDetail;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //    }
        //}
        public string Check_SampleReceiptItemStock(string CompID, string Branch, string SR_No, string SR_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentcheck = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Branch),
                                                        objProvider.CreateInitializedParameter("@Sr_No",DbType.String, SR_No),
                                                        objProvider.CreateInitializedParameter("@Sr_Date",DbType.String, SR_Date),
                                                        objProvider.CreateInitializedParameter("@StockChangeFlag",DbType.String,""),
                };
                prmContentcheck[4].Size = 1;
                prmContentcheck[4].Direction = ParameterDirection.Output;
                string SRDetail = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$stk$detail_CheckedSampleReceipt_StockQty", prmContentcheck).ToString();
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
        public string InsertUpdateSR_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, /*DataTable dtSubItem,*/ DataTable DtblAttchDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                       // objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),

                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$sr$detail_InsertSampleReceipt_Details", prmcontentaddupdate).ToString();
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

            finally
            {
            }
        }
        public DataSet GetSRDetailList(string CompId, string BrchID, string UserID, string EntityType, string EntityName, string Fromdate, string Todate, string Status, string Docid, string wfstatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),
                                                        objProvider.CreateInitializedParameter("@EntityName",DbType.String, EntityName),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                         objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                         objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$sr$detail_GetSR_DeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAttatchDetailEdit(string CompID, string BrchID, string SR_No, string SR_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@sr_no",DbType.String, SR_No),
                                                        objProvider.CreateInitializedParameter("@sr_date",DbType.String, SR_Date),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$sr$getAttatchmentDetail", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataSet Edit_SampleReceiptDetail(string CompId, string BrID, string SRNo, string SRDate, string UserID, string DocID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
        //        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
        //        objProvider.CreateInitializedParameter("@SR_No",DbType.String,SRNo),
        //        objProvider.CreateInitializedParameter("@SR_Date",DbType.String,SRDate),
        //        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
        //        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
        //                                              };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$sr$detail_GetSampleReceipt_Details", prmContentGetDetails);
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

        public DataSet DblClickgetdetailsSR(string CompId, string BrchID, string SRNo, string SRDate, string Userid, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                        objProvider.CreateInitializedParameter("@SR_No",DbType.String,SRNo),
                                        objProvider.CreateInitializedParameter("@SR_Date",DbType.String,SRDate),
                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, Userid),
                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocumentMenuId),
                                                     };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$sr$detail_GetSampleReceipt_Details", prmContentGetDetails);
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
     
        public DataSet SR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag, string src_docdate, string src_doc_no,string Status)
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
                    objProvider.CreateInitializedParameter("@srcdocno",DbType.String, src_docdate),
                    objProvider.CreateInitializedParameter("@srcdocdt",DbType.String, src_doc_no),
                    objProvider.CreateInitializedParameter("@status",DbType.String, Status),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SR_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        } 
        public DataSet getMIdata(string CompID, string BrchID, string sample_name, string srcdocno, string srcdocdt,string entity_type,string entityname,string SR_Number)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, sample_name),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,srcdocno),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, srcdocdt),
                    objProvider.CreateInitializedParameter("@entitytype",DbType.String, entity_type),
                          objProvider.CreateInitializedParameter("@entityname",DbType.String, entityname),
                          objProvider.CreateInitializedParameter("@SR_Number",DbType.String, SR_Number),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$get$mi$item$detaildata", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSourceDocList(string Comp_ID, string Br_ID, string Itm_ID, string SuppID, string entity_type,string sr_number)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@item",DbType.String, Itm_ID),
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, SuppID),
                                                        objProvider.CreateInitializedParameter("@entitytype",DbType.String, entity_type), 
                                                       objProvider.CreateInitializedParameter("@sr_number",DbType.String, sr_number), 
                                                           
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMIsrcdocnoList", prmContentGetDetails);
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
        public DataSet GetItemList(string CompID, string BrID)//, string src_type, string EntityName, string EntityType)
        {
            

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    //  objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),

                    //objProvider.CreateInitializedParameter("@EntityName",DbType.String, EntityName),
                    //objProvider.CreateInitializedParameter("@src_type",DbType.String, src_type),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail$_GetSR_ItemList", prmContentGetDetails);
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
           // return null;
        }
        public DataSet CheckRFQAgainstPR(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$Smp$detail_Check$Dependcy$AgainstQC", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
