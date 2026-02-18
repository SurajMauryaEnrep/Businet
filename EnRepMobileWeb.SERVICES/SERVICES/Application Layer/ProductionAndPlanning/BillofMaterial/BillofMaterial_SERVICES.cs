using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.BillofMaterial;
using EnRepMobileWeb.UTILITIES;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.BillofMaterial
{
   public class BillofMaterial_SERVICES: BillofMaterial_ISERVICES
    {
        public (string, string) insertBOMDetail(DataTable bomproductdetail, DataTable bomitemdetails, DataTable AlternateItemDt, string A_Status,string A_Level,string A_Remarks,string menuid)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@bomproductdetail",SqlDbType.Structured, bomproductdetail),
                 objprovider.CreateInitializedParameterTableType("@ppl$bom$itm$details",SqlDbType.Structured, bomitemdetails),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@ProductNameList",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@BOMAlternateItemDetail",SqlDbType.Structured, AlternateItemDt),
                 objprovider.CreateInitializedParameter("@wf_status",DbType.String,A_Status),
                 objprovider.CreateInitializedParameter("@wf_level",DbType.String,A_Level),
                 objprovider.CreateInitializedParameter("@wf_remarks",DbType.String,A_Remarks),
                 objprovider.CreateInitializedParameter("@menuid",DbType.String,menuid),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                DataSet Ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$details_insert", prmcontentaddupdate);

                string DocNo = string.Empty;
                if (prmcontentaddupdate[2].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[2].Value.ToString();
                }
                string ProductNameList = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
                {
                    ProductNameList = prmcontentaddupdate[3].Value.ToString();
                }
                return (DocNo, ProductNameList);
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
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$Bind$prdname", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> BindProductNameInDDLList(string CompID, string BrID, string ItmName)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$Bind$prdname", prmContentGetDetails);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["item_id"].ToString(), PARQusData.Tables[0].Rows[i]["item_name"].ToString());
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
            return null;
        }
        public DataSet GetSOItemUOMDL(string Item_id, string CompId)
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
        public DataTable GetOperationNameList(int CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),                             };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$Bind$opname", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GeItemNameList(string CompID, string BrID, string ItmType, string wip, string Pack,string ItmName, string ProductId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmType",DbType.String, ItmType),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@wip",DbType.String, wip),
                    objProvider.CreateInitializedParameter("@Pack",DbType.String, Pack),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@ProductId",DbType.String, ProductId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$Bind$itemname", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetItemCost(Int32 CompID, Int32 BrID, string ItmId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItmId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$bind$item$cost", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet HidePrNameAfterSave(Int32 CompID, Int32 BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$detail$hide$pr$name", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetBOMList(Int32 CompID, Int32 BrID, string UserID, string wfstatus, string DocumentMenuId, string Act, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                objProvider.CreateInitializedParameter("@Act",DbType.String, Act),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$list", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet BindBOMdbEdit(Int32 CompID, Int32 BrID,string product_id,Int32 rev_no,string UserID,string DocumentMenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, product_id),
                    objProvider.CreateInitializedParameter("@rev_no",DbType.String, rev_no),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    //objProvider.CreateInitializedParameter("@DocNo",DbType.String, rev_no),
                    //objProvider.CreateInitializedParameter("@DocDate",DbType.String, rev_no),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuID),
                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ppl$bom$db$edit", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillofMaterialPrintDeatils(string CompID, string Br_ID, string product_id, int rev_no)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_ID),
                    objProvider.CreateInitializedParameter("@product_id",DbType.String, product_id),
                    objProvider.CreateInitializedParameter("@rev_no",DbType.String, rev_no),
                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_BillofMaterialPrintDeatils", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetShopFloorDetailsDAL(int CompID, int br_id) // Added By Nitesh 26-10-2023 11:02 for Bind Shopfloore data
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),                               };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_pp$jc$BindShfl", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        } 
        public DataSet getrepicateitem(int CompID, int br_id, string SOItmName) // Added By Nitesh 26-10-2023 11:02 for Bind Shopfloore data
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),                              
                     objProvider.CreateInitializedParameter("@itemname", DbType.String, SOItmName)                               };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_bom$replicate$item", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet getreplicateitemdata(string comp_id, string br_id, string replicate_item) // Added By Nitesh 26-10-2023 11:02 for Bind Shopfloore data
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_id),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),                            
                     objProvider.CreateInitializedParameter("@replicate_item", DbType.String, replicate_item),    
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_repli_item$detail$", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getUomConvRate(string comp_ID, string br_ID, string itemId, string uomId)// Added By Suraj on 18-01-2024 for Get Conversion Rate
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_ID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_ID),                            
                     objProvider.CreateInitializedParameter("@item_id", DbType.String, itemId),    
                     objProvider.CreateInitializedParameter("@uom_id", DbType.String, uomId),     
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetItemUomConvRate", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
