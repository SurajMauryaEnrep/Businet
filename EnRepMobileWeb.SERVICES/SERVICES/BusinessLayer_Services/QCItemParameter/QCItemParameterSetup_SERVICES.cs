using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
    public class QCItemParameterSetup_SERVICES : QCItemParameterSetup_ISERVICES
    {

        public DataSet GetItemParameterSetupList(int CompID)  // patameter setup list table values //
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
            };
                DataSet GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$param_ParameterSetupList", prmContentGetDetails);
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetITMlist(string GroupName, string CompID, string BrchID) // patameter setup list table values //
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                       objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
            };
                DataTable GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetItemNameList", prmContentGetDetails).Tables[0];
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllData(string GroupName, string CompID, string BrchID, string item_Group) // patameter setup list table values //
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                       objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                       objProvider.CreateInitializedParameter("@item_Group",DbType.String, item_Group),
            };
                DataSet GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAll$Data$QCItem$Perameter", prmContentGetDetails);
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable ItemGroupList(string GroupName, string CompID) // patameter setup list table values //
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),

            };
                DataTable GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$grp_GetAllItemGroupNoodChilds_ItemList", prmContentGetDetails).Tables[0];
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getQCItemParameterSave(string TransType, int Comp_ID, int userid, string para_type, string item_id, string paramId, string UpperValue, string LowerValue, string uomId, string SystemDetail)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/  
                      objProvider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32,Comp_ID),
                    objProvider.CreateInitializedParameter("@ParamID",DbType.Int32, paramId),
                    objProvider.CreateInitializedParameter("@ItemID",DbType.String, item_id),
               //     objProvider.CreateInitializedParameter("@ParaType",DbType.String,para_type),
                    objProvider.CreateInitializedParameter("@CreateID",DbType.Int32, userid),
                    objProvider.CreateInitializedParameter("@MacID",DbType.String, SystemDetail),
                   objProvider.CreateInitializedParameter("@UpperValue",DbType.Int64, UpperValue),
                   objProvider.CreateInitializedParameter("@LowerValue",DbType.Int64, LowerValue),
                    objProvider.CreateInitializedParameter("@uomId",DbType.Int64, uomId),

                                 };
                DataSet QCParameterDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$item$param_InsertSetupParameterDetail", prmContentGetDetails);
                return QCParameterDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            DataSet Ds = new DataSet();
            return Ds;
        }
        public DataTable GetItemSetupParameterList(int CompID)   // list of stored value by parameter creation setup
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
            };
                DataTable GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$param_ParameterSetupDetailList", prmContentGetDetails).Tables[0];
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetItemSetupList(int CompID, string BrchID, string Value)   // Item LIST //
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int32, BrchID),
                     objProvider.CreateInitializedParameter("@ItemId",DbType.String, Value),
            };
                DataTable GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$item$param_ItemParameterSetup", prmContentGetDetails).Tables[0];
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public String insertQCDetails(DataTable QCItemDetail)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@QCParameterDetails",SqlDbType.Structured, QCItemDetail ),
                // objprovider.CreateInitializedParameterTableType("@QCParameterHeader",SqlDbType.Structured, QCItemDetail ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[1].Size = 100;
                prmcontentaddupdate[1].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "stp$qc$item$param_InsertQCParameterDetail", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[1].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[1].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }


        }
        public DataTable GetItemSetupListOnVIew(int CompID, string itemID)   // Item LIST on Detail page //
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemID),
            };
                DataTable GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$param_SetupListOnView", prmContentGetDetails).Tables[0];
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetItemSetupToDelete(string itemId, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {     /*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@itemId",DbType.String, itemId),
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID)
            };
                DataSet GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$item$param_SetupDetete", prmContentGetDetails);
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetQCItemDetailLIst(string ItemID, string CompId)
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
        public DataSet GetItemSetupToApprove(string itemId, string CompID, string UserId, string status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {     /*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@itemId",DbType.String, itemId),
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@UserId",DbType.Int32, UserId),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, status)
            };
                DataSet GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$item$param_SetupApproved", prmContentGetDetails);
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetItemParamListFilterDAL(string CompID, string ItmID, string ItmGrpID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@ItmID",DbType.String,ItmID),
                     objProvider.CreateInitializedParameter("@ItmGrpID",DbType.String,ItmGrpID),
                                                     };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetItemParamFilterList", prmContentGetDetails);
                return GetItemList;

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
        public DataTable GetUomIdList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetUom = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$UomList", prmContentGetDetails).Tables[0];
                return GetUom;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetItemParaMList(string CompID, string Parmid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@paraid",DbType.Int32, Parmid),
                                                    };
                DataTable GetParam = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$param_ParamDetailList", prmContentGetDetails).Tables[0];
                return GetParam;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getReplicateWith(string CompID, string SarchValue) // Added By Nitesh 26-10-2023 11:02 for Bind Shopfloore data
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@SarchValue", DbType.String, SarchValue)
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetItemParam$replicate", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetReplicateWithItemdata(string CompID, string ItemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$qc$param_SetupListOnView", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetMasterDropDownList(string CompID, string Br_Id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, Br_Id),
                };
                DataSet GetList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Get$QC$Master$Dropdown", prmContentGetDetails);
                return GetList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVerifiedDataOfExcel(string compId, string Br_Id, DataTable QCDetail)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                     objProvider.CreateInitializedParameterTableType("@ItemParamData", SqlDbType.Structured, QCDetail),
                    objProvider.CreateInitializedParameter("@CompID", DbType.String, compId),
                    objProvider.CreateInitializedParameter("@Br_Id", DbType.String, Br_Id)
                };
                DataSet GetCustomerList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateItemParameterExceFile", prmContentGetDetails);
                return GetCustomerList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable ShowExcelErrorDetail(string compId, string Br_Id, DataTable QCDetail)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                {
                    objProvider.CreateInitializedParameterTableType("@ItemParamData", SqlDbType.Structured, QCDetail),
                    objProvider.CreateInitializedParameter("@CompID", DbType.String, compId),
                    objProvider.CreateInitializedParameter("@Br_Id", DbType.String, Br_Id)
                };
                DataSet ErrorLsit = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ShowItemParameterErrorDetail", prmContentGetDetails);
                return ErrorLsit.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string BulkImportQCItemDetail(string compId, string UserID, DataTable QCDetail)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                    objProvider.CreateInitializedParameterTableType("@ItemParamData",SqlDbType.Structured, QCDetail ),
                    objProvider.CreateInitializedParameter("@UserID", DbType.String,UserID),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String,compId),
                 objProvider.CreateInitializedParameterTableType("@OutPut",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BulkImportQCItemParam", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
    }
}
