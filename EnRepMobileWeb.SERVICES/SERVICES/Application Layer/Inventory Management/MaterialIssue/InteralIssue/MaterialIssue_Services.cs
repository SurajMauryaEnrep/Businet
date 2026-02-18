using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.UTILITIES;
using System.Data.SqlClient;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialIssue;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialIssue;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialIssue
{
    public class MaterialIssue_Services : MaterialIssue_IServices
    {
        public DataSet GetAllDDLandListPageData(string CompId, string br_id, string flag
            , string startDate, string CurrentDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                    objProvider.CreateInitializedParameter("@FromDate",DbType.String, startDate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, CurrentDate),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$MtrlIssu$GetReqAreaAndIssueToList", prmContentGetDetails);
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
        public DataSet GetRequirmentreaList(string CompId, string br_id, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ivt$mrs$GetRequirmentreaList", prmContentGetDetails);
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
        public DataSet GetIssuedByData(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, BrchID)
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$emp$issuedby$data$materialIssue", prmContentGetDetails);
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
        public DataSet getMRSNOList(string CompID, string BrchID, string MRSNo, string Area, string RequisitionType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@mrs_type",DbType.String, RequisitionType),
                    objProvider.CreateInitializedParameter("@req_area",DbType.Int64, Area),
                    objProvider.CreateInitializedParameter("@mrs_no",DbType.String, MRSNo),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$MaterialIssue$GetMaterialRequestNumberList", prmContentGetDetails);
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
        public DataSet GetIssueToList(string CompId, string IssueTo, string BranchId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@IssueTo",DbType.String, IssueTo),
                    objProvider.CreateInitializedParameter("@branchId",DbType.String, BranchId),                         };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ivt$mrs$detail_CustomerSupplerList", prmContentGetDetails);
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
        public DataSet GetMaterialRequisitionIssueTo(string CompID, string BrchID, string MRSDate, string MRSNo, string mrs_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@MRSDate",DbType.DateTime, MRSDate),
                objProvider.CreateInitializedParameter("@MRSNo",DbType.String, MRSNo),
                objProvider.CreateInitializedParameter("@mrs_type",DbType.String, mrs_type),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ivt$mrs$GetMaterialRequisitionIssueTo", prmContentGetDetails);

                //return PARQusData.Tables[0].Rows[0]["entityname"].ToString();
               // return PARQusData.Tables[0];
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
        public DataSet GetSuppAddrDetailDAL(string CompID, string BrchID, string MRSDate, string MRSNo, string mrs_type)
        {
            try
            {
                DataSet searchmenu = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                          objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@MRSDate",DbType.String, MRSDate),
                objProvider.CreateInitializedParameter("@MRSNo",DbType.String, MRSNo),
                objProvider.CreateInitializedParameter("@mrs_type",DbType.String, mrs_type),
                                                      };
                searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppAndCustAddrDetails", prmContentGetDetails);


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
        public DataSet GetMaterialRequisitionItemDetailByNO(string CompID, string BrchID, string MRSDate, string MRSNo, string MRSType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@MRSDate",DbType.DateTime, MRSDate),
                    objProvider.CreateInitializedParameter("@MRSNo",DbType.String, MRSNo),
                    objProvider.CreateInitializedParameter("@mrs_type",DbType.String, MRSType),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ivt$mrs$GetMaterialRequisitionItemDetailByNO", prmContentGetDetails);

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
        public string InsertUpdateMaterialIssue(DataTable MaterialIssuetHeader, DataTable MaterialIssueItemDetails
            , DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, MaterialIssuetHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, MaterialIssueItemDetails ),
                 //objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,MaterialIssueAttachments ),
                 objprovider.CreateInitializedParameterTableType("@ItemBatchDetails",SqlDbType.Structured,ItemBatchDetails ),
                 objprovider.CreateInitializedParameterTableType("@ItemSerialDetails",SqlDbType.Structured,ItemSerialDetails ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetals",SqlDbType.Structured,dtSubItem ),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$MaterialIssue$InsertUpdateMaterialIssue", prmcontentaddupdate).ToString();

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
        public DataSet GetWarehouseList1(string CompId, string BrID, string doc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, doc_id),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetToWarehouseList", prmContentGetDetails);//commented By Shubham Maurya 22-07-2024
                //DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse_GetWarehouseList", prmContentGetDetails);//commented By Shubham Maurya 22-07-2024
                //DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetDestWarehouseList", prmContentGetDetails);
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
        public DataSet getDocumentStatus(string MenuDocumentId)
        {

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
        public DataSet GettMaterialIssueListAll(string CompID, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@IssueType", DbType.String, "0"),
                    objProvider.CreateInitializedParameter("@ReqArea", DbType.String, "0"),
                    objProvider.CreateInitializedParameter("@IssueTo", DbType.String, "0"),
                    objProvider.CreateInitializedParameter("@Fromdate", DbType.String, ""),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, ""),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, "0"),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$MaterialIssue$GetAllMaterialIssueList", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataSet GetMaterialIssueToList(string CompID, string BranchId)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
        //            objProvider.CreateInitializedParameter("@BrId",DbType.String, BranchId), };
        //        DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$MaterialIssue$GetAllMIIssueToList]", prmContentGetDetails);
        //        return PARQusData;

        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }

        //}
        public DataTable GetMaterialIssueDetailByFilter(string CompID, string br_id, string RequisitionTyp, string RequiredArea, string MaterialIssueTo, string Fromdate, string Todate, string Status,string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@IssueType", DbType.String,RequisitionTyp),
                    objProvider.CreateInitializedParameter("@ReqArea", DbType.String, RequiredArea),
                    objProvider.CreateInitializedParameter("@IssueTo", DbType.String, MaterialIssueTo),
                    objProvider.CreateInitializedParameter("@Fromdate", DbType.String, Fromdate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                };
                DataTable PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$MaterialIssue$GetAllMaterialIssueList", prmContentGetDetails).Tables[0];
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetMatrialIssueDetailByNo(string CompID, string BrchID, string MIssue_type, string MIssue_no, string MIssue_date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@MIssueType",DbType.String, MIssue_type),
                    objProvider.CreateInitializedParameter("@MIssueNo",DbType.String, MIssue_no),
                    objProvider.CreateInitializedParameter("@MIssueDate",DbType.String, MIssue_date),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mi$detail_GetMI_Detail", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet getItemStockBatchWise(string ItemId,string UomId, string WarehouseId, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[MTI_GetItemStockBatchwise]", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemStockBatchWisefromRwkJO(string ItemId, string UomId, string WarehouseId, string CompId, string BranchId, string MRSNo)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
                objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
             objProvider.CreateInitializedParameter("@MRSNo",DbType.String,  MRSNo),
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId)

            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[ivt$mrs$detail_GetItemStockBatchwisefromRwkJO]", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo
            , string IssueDate, string ItemID,string UomId = null)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@IssueType",DbType.String, IssueType),
            objProvider.CreateInitializedParameter("@IssueNo",DbType.String, IssueNo),
            objProvider.CreateInitializedParameter("@IssueDate",DbType.String, IssueDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            //objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mi$item$bt$detail_Get_MIItem_StockBatchwise", prmContentGetDetails);
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
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MTI_GetItemStockSerialwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemStockSerialWisefromRwkJO(string ItemId, string WarehouseId, string CompId, string BranchId, string MRSNo)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            objProvider.CreateInitializedParameter("@MRSNo",DbType.String,  MRSNo),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ivt$mrs$detail_GetItemStockSerialwisefromRwkJO", prmContentGetDetails);
            return DS;
        }

        public DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo, string IssueDate, string ItemID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@IssueType",DbType.String, IssueType),
            objProvider.CreateInitializedParameter("@IssueNo",DbType.String, IssueNo),
            objProvider.CreateInitializedParameter("@IssueDate",DbType.String, IssueDate),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mi$item$sr$detail_Get_MIItem_StockSerialwise", prmContentGetDetails);
            return DS;
        }

        public DataSet MI_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
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
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MI_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, string> IssueToList(string CompID, string Entity, string BrchID,string sr_type)
        {
            Dictionary<string, string> ddlcountryDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Entity",DbType.String, Entity),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@sr_type",DbType.String, sr_type),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$EntityList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlcountryDictionary.Add(PARQusData.Tables[0].Rows[i]["id"].ToString(), PARQusData.Tables[0].Rows[i]["val"].ToString());
                    }
                }
                return ddlcountryDictionary;

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
        public string MaterialIssueCancel(MaterialIssue_Model _MaterialIssue_Model, string DocumentMenuId, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@MenuDocumentId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@issue_no",DbType.String,  _MaterialIssue_Model.MaterialIssueNo),
                    objProvider.CreateInitializedParameter("@issue_dt",DbType.Date,  _MaterialIssue_Model.MaterialIssueDate),
                    objProvider.CreateInitializedParameter("@issue_type",DbType.Date,  _MaterialIssue_Model.MRS_type),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MaterialIssue_Model.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String,_MaterialIssue_Model.CancelledRemarks),
               };

            prmContentGetDetails[8].Size = 100;
            prmContentGetDetails[8].Direction = ParameterDirection.Output;

            string exmsg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$mi$detail_MaterialIssueCancel", prmContentGetDetails).ToString();

            string Msg = string.Empty;
            if (prmContentGetDetails[8].Value != DBNull.Value) // status
            {
                Msg = prmContentGetDetails[8].Value.ToString();
            }
            return Msg;

            //DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mi$detail_MaterialIssueCancel", prmContentGetDetails);

            //return DS;
        }
        public DataSet GetMRSDeatilsForPrint(string CompID, string BrchID, string Doc_No, string Doc_dt,string IssueType)
        {
        try{
                SqlDataProvider dataobject = new SqlDataProvider();
                SqlParameter[] miobject ={
        dataobject.CreateInitializedParameter("@comid",DbType.Int16,CompID),
        dataobject.CreateInitializedParameter("@brid",DbType.Int64,BrchID),
        dataobject.CreateInitializedParameter("@DocNo",DbType.String,Doc_No),
        dataobject.CreateInitializedParameter("@Doc_dt",DbType.DateTime,Doc_dt),
        dataobject.CreateInitializedParameter("@issue_type",DbType.String,IssueType)
                };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$MI$Deatils_ForPrint]", miobject);
                return dt;

        }
        catch(SqlException EX)
            {
                throw EX;
            }
            

        return null;
        }
        public DataSet GetPendingDocumentData(string CompID, string BrchID, string Docid,string language
            , string ItemID, string flag)
        {
            try
            {
                SqlDataProvider dataobject = new SqlDataProvider();
                SqlParameter[] miobject ={
                 dataobject.CreateInitializedParameter("@comid",DbType.String,CompID),
                dataobject.CreateInitializedParameter("@brid",DbType.String,BrchID),
                 dataobject.CreateInitializedParameter("@Docid",DbType.String,Docid),
                 dataobject.CreateInitializedParameter("@language",DbType.String,language),
                 dataobject.CreateInitializedParameter("@ItemID",DbType.String,ItemID),
                 dataobject.CreateInitializedParameter("@flag",DbType.String,flag),
                };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$Pending$Document$materialIssue]", miobject);
                return dt;
            }
            catch (SqlException EX)
            {
                throw EX;
            }
        }
        public DataSet GetPendingDocumentDataitemdetail(string CompID, string BrchID, string doc_no, string doc_dt)
        {
            try
            {
                SqlDataProvider dataobject = new SqlDataProvider();
                SqlParameter[] miobject ={
                 dataobject.CreateInitializedParameter("@comid",DbType.String,CompID),
                dataobject.CreateInitializedParameter("@brid",DbType.String,BrchID),
                 dataobject.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                 dataobject.CreateInitializedParameter("@doc_dt",DbType.String,doc_dt),
                };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$Pending$Document$mi$item$detail]", miobject);
                return dt;
            }
            catch (SqlException EX)
            {
                throw EX;
            }
        }
        public DataSet checkDependency(string CompID, string BrchID, string issue_no, string issue_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@issue_no",DbType.String, issue_no),
                     objProvider.CreateInitializedParameter("@issue_dt",DbType.Date,  issue_dt),
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_Material$Issue$Check$Dependency]", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
