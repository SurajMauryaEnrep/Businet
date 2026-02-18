using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
    public class ItemList_SERVICES : ItemList_ISERVICES
    {
        public DataTable BindGetItemList(string GroupName, string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetItemNameList", prmContentGetDetails).Tables[0];
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
        public DataSet BindGetAllDropDownList(string GroupName, string Comp_ID, string BrchID, string Item_ID, string Itemportfolio, string AttributeName, string User_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                        objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, Comp_ID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, Item_ID),
                    objProvider.CreateInitializedParameter("@Itemportfolio",DbType.String, Itemportfolio),
                     objProvider.CreateInitializedParameter("@AttrName",DbType.String, "0"),
                     objProvider.CreateInitializedParameter("@AttributeName",DbType.String, AttributeName),
                     objProvider.CreateInitializedParameter("@User_ID",DbType.String, User_ID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$Item$all$list$dataBind", prmContentGetDetails);
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
        //public DataTable BindGetGroupList(string GroupName, string CompID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
        //            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
        //        };
        //        DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$grp_GetAllItemGroupNoodChilds_ItemList", prmContentGetDetails).Tables[0];
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
        //public DataTable BindGetPortfNameList(string GroupName, string CompID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@PortfName",DbType.String, GroupName),
        //            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
        //        };
        //        DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetItemPortfolioList", prmContentGetDetails).Tables[0];
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
        //public DataTable BindAttributeNameList(string CompID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //           objProvider.CreateInitializedParameter("@AttrName",DbType.String, "0"),
        //             objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
        //        };
        //        DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$attr_GetattrList", prmContentGetDetails).Tables[0];
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
        public DataTable BindGetAttributeValue(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@AttrID",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$attr$val_GetattrValList", prmContentGetDetails).Tables[0];
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
        public DataSet GetItemListDAL(string CompID, string UserID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                      objProvider.CreateInitializedParameter("@UserID",DbType.String,UserID),
                                                     };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetItemList", prmContentGetDetails);
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
        public DataSet GetItemListFilterDAL(string CompID, string ItmID, string ItmGrpID, string AttrName, string AttrValue, string ActStatus, string ItmStatus, string ItemPrfID, string Imagefilter)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@ItmID",DbType.String,ItmID),
                     objProvider.CreateInitializedParameter("@ItmGrpID",DbType.String,ItmGrpID),
                      objProvider.CreateInitializedParameter("@AttrName",DbType.String,AttrName),
                     objProvider.CreateInitializedParameter("@AttrValue",DbType.String,AttrValue),
                      objProvider.CreateInitializedParameter("@ActStatus",DbType.String,ActStatus),
                     objProvider.CreateInitializedParameter("@ItmStatus",DbType.String,ItmStatus),
                        objProvider.CreateInitializedParameter("@ItemPrfID",DbType.String,ItemPrfID),
                        objProvider.CreateInitializedParameter("@Imagefilter",DbType.String,Imagefilter),
                                                     };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetItemFilterList", prmContentGetDetails);
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
        public Dictionary<string, string> ItemSetupGroupDAL(string GroupName, string CompID, string BrchID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetItemNameList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["Item_id"].ToString(), PARQusData.Tables[0].Rows[i]["Item_name"].ToString());
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
        public Dictionary<string, string> ItemGroupListDAL(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$grp_GetAllItemGroupNoodChilds_ItemList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["item_grp_id"].ToString(), PARQusData.Tables[0].Rows[i]["ItemGroupChildNood"].ToString());
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
        public Dictionary<string, string> ItemPortfolioList(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@PortfName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetItemPortfolioList]", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["setup_id"].ToString(), PARQusData.Tables[0].Rows[i]["setup_val"].ToString());
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
        public DataSet GetAttributeListDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@AttrName",DbType.String, "0"),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataSet GetAtrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$attr_GetattrList", prmContentGetDetails);
                return GetAtrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAttributeValueDAL(string CompID, string AttributeID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     //objProvider.CreateInitializedParameter("@AttrName",DbType.String, GroupName),
                     objProvider.CreateInitializedParameter("@AttrID",DbType.String, AttributeID),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataSet GetAttrVal = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$attr$val_GetattrValList", prmContentGetDetails);
                return GetAttrVal;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetItemImageListDAL(string CompID, string ItmCode)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItmCode),
                                                     };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$image_GetItemImageList", prmContentGetDetails);
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
        public DataSet GetItem_OrdersDetails(string CompID, string ItmCode)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItmCode),
                                                     };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$image_GetItem_OrdersDetails", prmContentGetDetails);
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
        public DataSet GetItemsAvailableStock(string compId, string brId, string itemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                    objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                                                     };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetItemSetupStockDetails", prmContentGetDetails);
                return GetItemList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetMasterDataForExcelFormat(string compId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId)
        };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetMasterDataforItemsImportFormat", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string BulkImportItemsDetail(string compId, string brId, DataTable dtItems, DataTable dtSubItems, DataTable dtItemAttributes, DataTable dtItemBranch, DataTable dtItemPortfolio, string BranchName, string createdBy)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                        
                        objprovider.CreateInitializedParameterTableType("@ItemsData",SqlDbType.Structured, dtItems ),
                        objprovider.CreateInitializedParameterTableType("@SubItemsData",SqlDbType.Structured, dtSubItems ),
                        objprovider.CreateInitializedParameterTableType("@ItemAttrData",SqlDbType.Structured,dtItemAttributes),
                        objprovider.CreateInitializedParameterTableType("@BranchData",SqlDbType.Structured,dtItemBranch),
                        objprovider.CreateInitializedParameterTableType("@Portfolio",SqlDbType.Structured,dtItemPortfolio),
                        objprovider.CreateInitializedParameter("@BranchName",DbType.String, BranchName),
                        objprovider.CreateInitializedParameter("@CompId",DbType.String, compId),
                        objprovider.CreateInitializedParameter("@BrId",DbType.String, brId),
                        objprovider.CreateInitializedParameter("@CreatedBy",DbType.String, createdBy),
                        objprovider.CreateInitializedParameterTableType("@OutPut",SqlDbType.NVarChar,"")
                };
                prmcontentaddupdate[9].Size = 100;
                prmcontentaddupdate[9].Direction = ParameterDirection.Output;
                string result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BulkImportItems", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[9].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[9].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetVerifiedDataOfExcel(string compId, DataTable dtItems, DataTable dtSubItems, DataTable dtItemAttributes, DataTable dtItemBranch, DataTable dtItemPortfolio)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameterTableType("@ItemsData",SqlDbType.Structured, dtItems ),
                    objProvider.CreateInitializedParameterTableType("@SubItemsData",SqlDbType.Structured, dtSubItems ),
                    objProvider.CreateInitializedParameterTableType("@ItemAttrData",SqlDbType.Structured,dtItemAttributes),
                    objProvider.CreateInitializedParameterTableType("@BranchData",SqlDbType.Structured,dtItemBranch),
                    objProvider.CreateInitializedParameterTableType("@Portfolio",SqlDbType.Structured,dtItemPortfolio)
                  };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ValidateItemsExceFile", prmContentGetDetails);
                return GetItemList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable ShowExcelErrorDetail(string compId, DataTable dtItems, DataTable dtSubItems, DataTable dtItemAttributes, DataTable dtItemBranch, DataTable dtItemPortfolio)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameterTableType("@ItemsData",SqlDbType.Structured, dtItems ),
                    objProvider.CreateInitializedParameterTableType("@SubItemsData",SqlDbType.Structured, dtSubItems ),
                    objProvider.CreateInitializedParameterTableType("@ItemAttrData",SqlDbType.Structured,dtItemAttributes),
                    objProvider.CreateInitializedParameterTableType("@BranchData",SqlDbType.Structured,dtItemBranch),
                    objProvider.CreateInitializedParameterTableType("@Portfolio",SqlDbType.Structured,dtItemPortfolio)
                  };
                DataSet GetItemList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ShowExcelItemErrorDetail", prmContentGetDetails);
                return GetItemList.Tables[0];
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
}
