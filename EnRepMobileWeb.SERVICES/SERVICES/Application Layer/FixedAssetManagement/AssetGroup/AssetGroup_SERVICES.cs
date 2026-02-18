using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetGroup;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetGroup;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FixedAssetManagement.AssetGroup
{
    public class AssetGroup_SERVICES : AssetGroup_ISERVICES
    {
        public DataSet GetAllItemGrp(ItemMenuSearchModel_AG ObjItemMenuSearchModel)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, ObjItemMenuSearchModel.Comp_ID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_allassetgrp", prmContentGetDetails);
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
        public DataSet GetAssetDetail(string ItemGrpId, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@grp_id", DbType.String, ItemGrpId),
                                                        objProvider.CreateInitializedParameter("@comp_id", DbType.String, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetgrp", prmContentGetDetails);
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
        public DataTable GetAssetGroupSetup(string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                         objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_parentgrp", prmContentGetDetails);
                return searchmenu.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        
        public Dictionary<string, string> GetLocalPurchaseAccount(string AccName, string CompID)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@AccName",DbType.String, AccName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$LocalPurchaseAccount", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        dtList.Add(PARQusData.Tables[0].Rows[i]["acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["acc_name"].ToString());
                    }
                }
                return dtList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public Dictionary<string, string> GetAssetCategory(string CompID)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetcategory", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        dtList.Add(PARQusData.Tables[0].Rows[i]["acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["acc_name"].ToString());
                    }
                }
                return dtList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public Dictionary<string, string> GetStockAccount(string AccName, string CompID)
        {
            Dictionary<string, string> dtList = new Dictionary<string, string>();
            string firstItem = string.Empty;
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@AccName",DbType.String, AccName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$StockAccount", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        dtList.Add(PARQusData.Tables[0].Rows[i]["acc_id"].ToString(), PARQusData.Tables[0].Rows[i]["acc_name"].ToString());
                    }
                }
                return dtList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
       
        public string InsertAssetGroupDetail(AssetGroupModel ObjAddItemGroupSetupBOL)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentAddUpdate = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, ObjAddItemGroupSetupBOL.Comp_id),
                                                        objProvider.CreateInitializedParameter("@item_grp_id",DbType.Int64, ObjAddItemGroupSetupBOL. Asset_grp_id),
                                                        objProvider.CreateInitializedParameter("@item_group_name",DbType.String, ObjAddItemGroupSetupBOL.Asset_group_name),
                                                        objProvider.CreateInitializedParameter("@item_grp_struc",DbType.String, ObjAddItemGroupSetupBOL.Asset_grp_par_id),
                                                        objProvider.CreateInitializedParameter("@item_grp_par_id",DbType.String, ObjAddItemGroupSetupBOL.Asset_grp_par_id),
                                                        objProvider.CreateInitializedParameter("@dep_met",DbType.String, ObjAddItemGroupSetupBOL.Dep_method),
                                                        objProvider.CreateInitializedParameter("@dep_per",DbType.String, ObjAddItemGroupSetupBOL.Dep_per),
                                                        objProvider.CreateInitializedParameter("@asset_cat_coa",DbType.String, ObjAddItemGroupSetupBOL.Assetcat_coa),
                                                        objProvider.CreateInitializedParameter("@dep_freq",DbType.String, ObjAddItemGroupSetupBOL.Dep_freq),
                                                        objProvider.CreateInitializedParameter("@dep_coa",DbType.Int64, ObjAddItemGroupSetupBOL.Dep_coa),
                                                        objProvider.CreateInitializedParameter("@asset_coa",DbType.Int64, ObjAddItemGroupSetupBOL.Asset_coa),
                                                        objProvider.CreateInitializedParameter("@create_id",DbType.Int64, ObjAddItemGroupSetupBOL.Create_id),
                                                        objProvider.CreateInitializedParameter("@mod_id",DbType.Int64, ObjAddItemGroupSetupBOL.Mod_id),
                                                    };
                string Result = string.Empty;
                if (ObjAddItemGroupSetupBOL.FormMode == "1")
                {
                    Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "fa_sp_Upd_AssetGrpDetail", prmContentAddUpdate));
                }
                else
                {
                    Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "fa_sp_Ins_AssetGrpDetail", prmContentAddUpdate));
                }
                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string DeleteItemGroup(int item_grp_id, string comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentAddUpdate = {
                                        objProvider.CreateInitializedParameter("@Asset_grp_id",DbType.Int64, item_grp_id),
                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_id),
                                      };
                string Result = string.Empty;
                Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "fa_sp_Del_AssetGrp", prmContentAddUpdate));
                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string ChkPGroupDependency(int item_grp_id, string comp_id)
        {
            try
            {
                string Result = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentCheck = {
                                        objProvider.CreateInitializedParameter("@Asset_grp_id",DbType.Int64, item_grp_id),
                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_id),
                                      };

                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_chkassetparentgrpdependency", prmContentCheck);
                Result = ds.Tables[0].Rows[0]["Flag"].ToString();
                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string ChkChildGroupDependency(int item_grp_id, string comp_id)
        {
            try
            {
                string Result = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentCheck = {
                                        objProvider.CreateInitializedParameter("@Asset_grp_id",DbType.Int64, item_grp_id),
                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_id),
                                      };

                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_chkassetchildgrpdependency", prmContentCheck);
                Result = ds.Tables[0].Rows[0]["Flag"].ToString();
                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet GetSelectedParentDetail(string item_grp_struc, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@item_grp_struc",DbType.String, item_grp_struc),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fa_sp_get_assetgrp_parent_details", prmContentGetDetails);
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


        public JObject GetAllItemGrpBl(ItemMenuSearchModel_AG ObjItemMenuSearchModel)
        {
            JObject FinalList = new JObject();
            try
            {
                //objItemSetupDL = new ItemSetupDAL();
                DataSet GetItemList = GetAllItemGrp(ObjItemMenuSearchModel);
                DataTable PresentNode = new DataTable();
                DataTable ChildNode = new DataTable();
                DataTable SubChildNode = new DataTable();
                DataTable SubSubChildNode = new DataTable();
                DataTable SubSubSubChildNode = new DataTable();

                PresentNode = GetItemList.Tables[0];
                ChildNode = GetItemList.Tables[1];
                SubChildNode = GetItemList.Tables[2];
                SubSubChildNode = GetItemList.Tables[3];
                SubSubSubChildNode = GetItemList.Tables[4];

                ParentNode_AG ParentNod = new ParentNode_AG();
                childrenNode_AG ChildNod = new childrenNode_AG();
                SubchildrenNode_AG SubChildNod = new SubchildrenNode_AG();
                SubSubchildrenNode_AG SubSubChildNod = new SubSubchildrenNode_AG();
                SubSubSubchildrenNode_AG SubSubSubChildNod = new SubSubSubchildrenNode_AG();
                Header_AG Headertree = new Header_AG();

                if (PresentNode.Rows.Count > 0)
                {
                    for (int x = 0; x < PresentNode.Rows.Count; x++)
                    {
                        JObject NewObj = new JObject();
                        var Finaldata = string.Empty;

                        List<childrenNode_AG> NodeChild = new List<childrenNode_AG>();
                        string PNodeName = string.Empty;
                        PNodeName = PresentNode.Rows[x]["Asset_grp_id"].ToString();

                        DataView DV = new DataView(ChildNode);
                        DV.RowFilter = "asset_grp_par_id='" + PNodeName + "'";
                        DataTable PreNode = new DataTable();
                        PreNode = DV.ToTable();
                        if (PreNode.Rows.Count > 0)
                        {
                            for (int y = 0; y < PreNode.Rows.Count; y++)
                            {
                                List<SubchildrenNode_AG> NodeSubChild = new List<SubchildrenNode_AG>();
                                DataView DV1 = new DataView(SubChildNode);
                                DV1.RowFilter = "asset_grp_par_id='" + PreNode.Rows[y]["Asset_grp_id"].ToString() + "'";
                                DataTable ChihNode = new DataTable();
                                ChihNode = DV1.ToTable();
                                if (ChihNode.Rows.Count > 0)
                                {
                                    for (int za = 0; za < ChihNode.Rows.Count; za++)
                                    {
                                        List<SubSubchildrenNode_AG> NodeSubSubChild = new List<SubSubchildrenNode_AG>();
                                        DataView DV2 = new DataView(SubSubChildNode);
                                        DV2.RowFilter = "asset_grp_par_id='" + ChihNode.Rows[za]["Asset_grp_id"].ToString() + "'";
                                        DataTable SubSubChihNode = new DataTable();
                                        SubSubChihNode = DV2.ToTable();
                                        if (SubSubChihNode.Rows.Count > 0)
                                        {
                                            for (int ay = 0; ay < SubSubChihNode.Rows.Count; ay++)
                                            {
                                                List<SubSubSubchildrenNode_AG> NodeSubSubSubChild = new List<SubSubSubchildrenNode_AG>();
                                                DataView DV3 = new DataView(SubSubSubChildNode);
                                                DV3.RowFilter = "asset_grp_par_id='" + SubSubChihNode.Rows[ay]["Asset_grp_id"].ToString() + "'";
                                                DataTable SubSubSubChihNode = new DataTable();
                                                SubSubSubChihNode = DV3.ToTable();
                                                if (SubSubSubChihNode.Rows.Count > 0)
                                                {
                                                    for (int az = 0; az < SubSubSubChihNode.Rows.Count; az++)
                                                    {
                                                        NodeSubSubSubChild.Add(new SubSubSubchildrenNode_AG
                                                        {
                                                            label = SubSubSubChihNode.Rows[az]["asset_group_name"].ToString(),
                                                            value = SubSubSubChihNode.Rows[az]["Asset_grp_id"].ToString(),
                                                        });
                                                    }
                                                }
                                                NodeSubSubChild.Add(new SubSubchildrenNode_AG
                                                {
                                                    label = SubSubChihNode.Rows[ay]["asset_group_name"].ToString(),
                                                    value = SubSubChihNode.Rows[ay]["Asset_grp_id"].ToString(),
                                                    children = NodeSubSubSubChild
                                                });
                                            }
                                        }
                                        NodeSubChild.Add(new SubchildrenNode_AG()
                                        {
                                            label = ChihNode.Rows[za]["asset_group_name"].ToString(),
                                            value = ChihNode.Rows[za]["Asset_grp_id"].ToString(),
                                            children = NodeSubSubChild
                                        });
                                    }
                                }
                                NodeChild.Add(new childrenNode_AG()
                                {
                                    label = PreNode.Rows[y]["asset_group_name"].ToString(),
                                    value = PreNode.Rows[y]["Asset_grp_id"].ToString(),
                                    children = NodeSubChild
                                });
                            }
                        }

                        ParentNod.children = NodeChild;
                        ParentNod.label = PresentNode.Rows[x]["asset_group_name"].ToString();
                        ParentNod.value = PresentNode.Rows[x]["Asset_grp_id"].ToString();
                        Headertree.TreeStr = ParentNod;
                        var Fdata = JsonConvert.SerializeObject(Headertree);
                        Finaldata = Fdata;
                        if (!string.IsNullOrEmpty(Finaldata))
                        {
                            if (x != 0)
                            {
                                Finaldata = Finaldata.Replace("TreeStr", "TreeStr" + x.ToString());
                            }
                        }

                        NewObj = JObject.Parse(Finaldata);
                        if (x == 0)
                        {
                            FinalList = NewObj;
                        }
                        else
                        {
                            FinalList.Merge(NewObj);
                        }
                    }
                }
                //FinalList = JObject.Parse(Finaldata);
                return FinalList;
            }
            catch (Exception ex)
            {
                JObject Obj = new JObject();
                return Obj;
                //throw;
            }
        }

        //public void GetItemGroupSetup(int CompID)
        //{
        //    try
        //    {
        //        DataSet ds = GetItemGroupSetup(CompID);
        //        ParentItemGroup = ds.Tables[0];
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
    }
}
