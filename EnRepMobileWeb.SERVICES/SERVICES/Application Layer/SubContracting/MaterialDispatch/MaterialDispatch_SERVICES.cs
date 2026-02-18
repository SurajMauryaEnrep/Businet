using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MaterialDispatch;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.MaterialDispatch;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SubContracting.MaterialDispatch
{
   public class MaterialDispatch_SERVICES : MaterialDispatch_ISERVICES
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
        public DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppAddrDetails", prmContentGetDetails);
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
        public DataSet GetJobORDDocList(string Supp_IdNm, string Comp_ID, string Br_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                          objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_IdNm),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_JobORDNoList", prmContentGetDetails);
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
        public DataSet getWarehouseWiseItemStock(string CompID, string BrID, string Wh_ID, string ItemID,string JONo, string JODate, string OrdQty, string DispatchQty, string UomId, string LotID, string BatchNo)
        {
            string AvaiableStock = "0";
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
            objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
            objProvider.CreateInitializedParameter("@Wh_ID",DbType.Int32,  Wh_ID),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String,  ItemID),
            objProvider.CreateInitializedParameter("@JONo",DbType.String,  JONo),
            objProvider.CreateInitializedParameter("@JODate",DbType.String,  JODate),
            objProvider.CreateInitializedParameter("@OrdQty",DbType.String,  OrdQty),
            objProvider.CreateInitializedParameter("@DispatchQty",DbType.String,  DispatchQty),
            objProvider.CreateInitializedParameter("@UomId",DbType.String,  UomId),
            objProvider.CreateInitializedParameter("@LotID",DbType.String,  LotID),
            objProvider.CreateInitializedParameter("@BatchNo",DbType.String,  BatchNo),
            
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Sp$MDSC_GetAvlStock_wh]", prmContentGetDetails);
            if (DS.Tables[0].Rows.Count > 0)
                AvaiableStock = DS.Tables[0].Rows[0]["wh_avl_stk_bs"].ToString();

            return DS;
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
        public DataSet getMaterialInputItemDetailByJONumber(string CompID, string BrchID, string JODate, string JONo, string OrdQty, string DispatchQty)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@JODate",DbType.DateTime, JODate),
                    objProvider.CreateInitializedParameter("@JONo",DbType.String, JONo),
                    objProvider.CreateInitializedParameter("@OrdQty",DbType.String, OrdQty),
                    objProvider.CreateInitializedParameter("@DispatchQty",DbType.String, DispatchQty),
                    //objProvider.CreateInitializedParameter("@mrs_type",DbType.String, MRSType),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$md$GetMaterialDispatchItemDetailByJONO", prmContentGetDetails);

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
        public DataSet GetDetailofJobOrdNoList(string CompID, string BrchID, string JobordNo, string JobOrddate, string Disp_No)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@JONo",DbType.String, JobordNo),
                    objProvider.CreateInitializedParameter("@JODate",DbType.DateTime, JobOrddate),
                    objProvider.CreateInitializedParameter("@DisptchNo",DbType.String, Disp_No)

                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$md$GetJobOrdItemDetailByJONO", prmContentGetDetails);

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
        public string InsertMD_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemBatchDetails",SqlDbType.Structured,ItemBatchDetails ),
                                                        objprovider.CreateInitializedParameterTableType("@ItemSerialDetails",SqlDbType.Structured,ItemSerialDetails ),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@SubItemDetails",SqlDbType.Structured,dtSubItem ),

                                                    };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sc$MD$detail_InsertMDDetails", prmcontentaddupdate).ToString();
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
        public DataSet GetMDListandSrchDetail(string CompId, string BrchID, MDListModel _MDListModel, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String,_MDListModel.SuppID),
                                                        objProvider.CreateInitializedParameter("@ProdctID",DbType.String,_MDListModel.Product_id),
                                                        objProvider.CreateInitializedParameter("@FinishProdctID",DbType.String,_MDListModel.FinishProdct_Id),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_MDListModel.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_MDListModel.ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _MDListModel.Status),
                                                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                             objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$Mdispatch$GetListandSrchDetail", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindProductNameInDDL(string CompID, string BrID, string ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_SC$Job$BindItmListName", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetMDDetailEditUpdate(string CompId, string BrchID, string MDSC_NO, string MDSC_Date, string UserID, string DocID)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@MDNo",DbType.String, MDSC_NO),
                                                        objProvider.CreateInitializedParameter("@MDDate",DbType.String, MDSC_Date),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$Mdispatch$detail_GetMDSCEditUpdtDetails", prmContentGetDetails);
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
        public string MD_DeleteDetail(MaterialDispatchModel _MaterialDispatchModel, string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@MDNo",DbType.String,_MaterialDispatchModel.MDIssue_No),
                                                        objProvider.CreateInitializedParameter("@MDDate",DbType.String,_MaterialDispatchModel.MDIssue_Date),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$Mdispatch$_DeleteAllSectionDetails", prmContentInsert).ToString();
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
        public string MDApproveDetails(string CompID, string BrchID, string MDNo, string MDDate, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                              objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                              objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                              objProvider.CreateInitializedParameter("@docno",DbType.String, MDNo),
                                                              objProvider.CreateInitializedParameter("@docdate",DbType.String, MDDate),
                                                              objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                              objProvider.CreateInitializedParameter("@DocMenuId",DbType.String, MenuID),
                                                              objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                              objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                              objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                              objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                                                              objProvider.CreateInitializedParameter("@stkstatus",DbType.String,""),
                };
                prmContentGetDetails[10].Size = 100;
                prmContentGetDetails[10].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sc$Mdispatch$MDApproveDetails", prmContentGetDetails).ToString();

                string msg = string.Empty;
                if (prmContentGetDetails[10].Value != DBNull.Value) // status
                {
                    msg = prmContentGetDetails[10].Value.ToString();
                }

                //DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$PackingListApprove", prmContentGetDetails);
                return msg;
                //string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$Mdispatch$MDApproveDetails", prmContentInsert).ToString();
                //return POId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public string MDCancel(MaterialDispatchModel _MaterialDispatchModel, string CompID, string br_id, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@dispatch_no",DbType.String,  _MaterialDispatchModel.MDIssue_No),
                    objProvider.CreateInitializedParameter("@dispatch_dt",DbType.String,  _MaterialDispatchModel.MDIssue_Date),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MaterialDispatchModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };
                string disptchno = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_Mdispatch$MDCancel", prmContentGetDetails).ToString();
                return disptchno;
            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }
        public DataSet ChkDNSCDagainstMDSC(string CompID, string BrID, string MDNo, string MDDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@md_no",DbType.String, MDNo),
                    objProvider.CreateInitializedParameter("@md_dt",DbType.String, MDDate),

                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$DNSC$ChckDtlDNSCAgainstMDSC", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockBatchwise]", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string IssueNo, string IssueDate, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            //objProvider.CreateInitializedParameter("@IssueType",DbType.String, IssueType),
            objProvider.CreateInitializedParameter("@IssueNo",DbType.String, IssueNo),
            objProvider.CreateInitializedParameter("@IssueDate",DbType.String, IssueDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$md$item$bt$detail_Get_MDItem_StockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$stk$detail_GetItemStockSerialwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string IssueNo, string IssueDate, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            //objProvider.CreateInitializedParameter("@IssueType",DbType.String, IssueType),
            objProvider.CreateInitializedParameter("@IssueNo",DbType.String, IssueNo),
            objProvider.CreateInitializedParameter("@IssueDate",DbType.String, IssueDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$md$item$sr$detail_Get_MDItem_StockSerialwise", prmContentGetDetails);
            return DS;
        }
        public DataSet GetMaterialDispatchDeatils(string Comp_ID, string Br_ID, string OrderNo, string OrderDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@issue_no",DbType.String, OrderNo),
                                                        objProvider.CreateInitializedParameter("@Issue_date",DbType.String, OrderDate),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMaterialDispacthDeatils_ForPrint", prmContentGetDetails);
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
        public DataSet MD_GetSubItemDetails(string CompID, string br_id, string Item_id, string JobOrdNo, string JobOrdDt, string Flag, 
            string OrdQty, string DispatchQty, string Wh_id, string flagwh, string DocNo,string Status)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@JobOrdNo",DbType.String, JobOrdNo),
                      objProvider.CreateInitializedParameter("@JobOrdDt",DbType.String, JobOrdDt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                      objProvider.CreateInitializedParameter("@OrdQty",DbType.String, OrdQty),
                    objProvider.CreateInitializedParameter("@DispatchQty",DbType.String, DispatchQty),
                   objProvider.CreateInitializedParameter("@Wh_id",DbType.String, Wh_id),
                    objProvider.CreateInitializedParameter("@flagwh",DbType.String, flagwh),
                   
                      objProvider.CreateInitializedParameter("@DisptchNo",DbType.String, DocNo),
                      objProvider.CreateInitializedParameter("@Status",DbType.String, Status),

                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[MD_GetSubItemDetails]", prmContentGetDetails);
            return ds;
        }

        public DataSet MD_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string DocNo, string DocDt, string JobOrdNo, string JobOrdDt, string Flag,
            string OrdQty, string DispatchQty, string Wh_id, string flagwh, string JOTyp)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                     objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                      objProvider.CreateInitializedParameter("@DocDt",DbType.String, DocDt),
                      objProvider.CreateInitializedParameter("@JobOrdNo",DbType.String, JobOrdNo),
                      objProvider.CreateInitializedParameter("@JobOrdDt",DbType.String, JobOrdDt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                      objProvider.CreateInitializedParameter("@OrdQty",DbType.String, OrdQty),
                    objProvider.CreateInitializedParameter("@DispatchQty",DbType.String, DispatchQty),
                   objProvider.CreateInitializedParameter("@Wh_id",DbType.String, Wh_id),
                    objProvider.CreateInitializedParameter("@flagwh",DbType.String, flagwh),
                    objProvider.CreateInitializedParameter("@JOTyp",DbType.String, JOTyp)
                   //objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                      
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[MD_GetSubItemDetailsAfterApprov]", prmContentGetDetails);
            return ds;
        }


    }
}
