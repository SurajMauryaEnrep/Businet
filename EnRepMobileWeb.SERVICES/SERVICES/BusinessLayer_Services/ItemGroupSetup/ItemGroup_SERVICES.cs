using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.MODELS.BusinessLayer.ItemGroupSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemGroupSetup;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.ItemGroupSetup
{
   public class ItemGroup_SERVICES: ItemGroup_ISERVICES
    {
        public DataSet GetAllItemGrp(ItemMenuSearchModel ObjItemMenuSearchModel)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, ObjItemMenuSearchModel.Comp_ID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$item$grp", prmContentGetDetails);
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
        public DataSet GetItemDetail(string ItemGrpId, int CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@item_grp_id",DbType.String, ItemGrpId),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$item$grp$detail", prmContentGetDetails);
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
        public DataSet GetItemGroupDetail(string ItemGrpId, int CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@item_grp_id",DbType.String, ItemGrpId),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$item$grp$detail$byname", prmContentGetDetails);
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

        public DataSet GetDefaultItemDetail(int CompId,string GroupID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                         objProvider.CreateInitializedParameter("@grp_id",DbType.String, GroupID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$default$item$grp", prmContentGetDetails);
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
        public DataTable GetItemGroupSetup(int CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                         objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$item$parent$grp", prmContentGetDetails);
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
        public Dictionary<string, string> GetLocalSaleAccount(string AccName, string CompID)
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

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$LocalSalAccount", prmContentGetDetails);
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
        public Dictionary<string, string> GetProvisionalPayableAccount(string AccName, string CompID)
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

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$ProvisionalPayableAccount", prmContentGetDetails);
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
        public string InsertitemGroupDetail(ItemGroupModel ObjAddItemGroupSetupBOL)
        {
            var sls = "";
            var pur = "";
            var wip = "";
            var capg = "";
            var stk = "";
            var qc = "";
            var service = "";
            var cons = "";
            var serial = "";
            var sample = "";
            var batch = "";
            var expiry = "";
            var pack = "";
            var catalog = "";

            if (ObjAddItemGroupSetupBOL.i_sls)
            {
                sls = "Y";
            }
            else
            {
                sls = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_pur)
            {
                pur = "Y";
            }
            else
            {
                pur = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_wip)
            {
                wip = "Y";
            }
            else
            {
                wip = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_capg)
            {
                capg = "Y";
            }
            else
            {
                capg = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_stk)
            {
                stk = "Y";
            }
            else
            {
                stk = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_qc)
            {
                qc = "Y";
            }
            else
            {
                qc = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_srvc)
            {
                service = "Y";
            }
            else
            {
                service = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_cons)
            {
                cons = "Y";
            }
            else
            {
                cons = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_serial)
            {
                serial = "Y";
            }
            else
            {
                serial = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_sam)
            {
                sample = "Y";
            }
            else
            {
                sample = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_batch)
            {
                batch = "Y";
            }
            else
            {
                batch = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_exp)
            {
                expiry = "Y";
            }
            else
            {
                expiry = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_pack)
            {
                pack = "Y";
            }
            else
            {
                pack = "N";
            }
            if (ObjAddItemGroupSetupBOL.i_catalog)
            {
                catalog = "Y";
            }
            else
            {
                catalog = "N";
            }
            ObjAddItemGroupSetupBOL.act_stat = "Y";
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentAddUpdate = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, ObjAddItemGroupSetupBOL.comp_id ),
                                                        objProvider.CreateInitializedParameter("@item_grp_id",DbType.Int64, ObjAddItemGroupSetupBOL. item_grp_id),
                                                        objProvider.CreateInitializedParameter("@item_group_name",DbType.String, ObjAddItemGroupSetupBOL.item_group_name ),
                                                        objProvider.CreateInitializedParameter("@item_grp_struc",DbType.String, ObjAddItemGroupSetupBOL.item_grp_par_id ),
                                                        objProvider.CreateInitializedParameter("@item_grp_par_id",DbType.String, ObjAddItemGroupSetupBOL.item_grp_par_id ),
                                                        objProvider.CreateInitializedParameter("@issue_method",DbType.String, ObjAddItemGroupSetupBOL.issue_method ),
                                                        objProvider.CreateInitializedParameter("@cost_method",DbType.String, ObjAddItemGroupSetupBOL.cost_method ),
                                                        objProvider.CreateInitializedParameter("@It_Remarks",DbType.String, ObjAddItemGroupSetupBOL.It_Remarks ),
                                                        objProvider.CreateInitializedParameter("@i_sls",DbType.String,sls ),
                                                        objProvider.CreateInitializedParameter("@i_pur",DbType.String, pur ),
                                                        objProvider.CreateInitializedParameter("@i_wip",DbType.String, wip ),
                                                        objProvider.CreateInitializedParameter("@i_capg",DbType.String, capg ),
                                                        objProvider.CreateInitializedParameter("@i_stk",DbType.String, stk ),
                                                        objProvider.CreateInitializedParameter("@sal_ret_coa",DbType.String, ObjAddItemGroupSetupBOL.sal_ret_coa ),
                                                        objProvider.CreateInitializedParameter("@i_qc",DbType.String, qc ),
                                                        objProvider.CreateInitializedParameter("@i_srvc",DbType.String, service ),
                                                        objProvider.CreateInitializedParameter("@i_cons",DbType.String, cons ),
                                                        objProvider.CreateInitializedParameter("@i_serial",DbType.String, serial ),
                                                        objProvider.CreateInitializedParameter("@i_sam",DbType.String, sample ),
                                                        objProvider.CreateInitializedParameter("@i_batch",DbType.String, batch ),
                                                        objProvider.CreateInitializedParameter("@i_exp",DbType.String, expiry ),
                                                        objProvider.CreateInitializedParameter("@i_pack",DbType.String, pack ),
                                                         objProvider.CreateInitializedParameter("@i_catalog",DbType.String, catalog ),
                                                        objProvider.CreateInitializedParameter("@act_stat",DbType.String, ObjAddItemGroupSetupBOL.act_stat ),
                                                        objProvider.CreateInitializedParameter("@loc_sls_coa",DbType.String, ObjAddItemGroupSetupBOL.loc_sls_coa ),
                                                        objProvider.CreateInitializedParameter("@exp_sls_coa",DbType.String, ObjAddItemGroupSetupBOL.exp_sls_coa ),
                                                        objProvider.CreateInitializedParameter("@loc_pur_coa",DbType.Int64, ObjAddItemGroupSetupBOL.loc_pur_coa ),
                                                        objProvider.CreateInitializedParameter("@imp_pur_coa",DbType.Int64, ObjAddItemGroupSetupBOL.imp_pur_coa ),
                                                        objProvider.CreateInitializedParameter("@stk_coa",DbType.Int64, ObjAddItemGroupSetupBOL.stk_coa ),
                                                        objProvider.CreateInitializedParameter("@disc_coa",DbType.Int64, ObjAddItemGroupSetupBOL.disc_coa ),
                                                        objProvider.CreateInitializedParameter("@pur_ret_coa",DbType.Int64, ObjAddItemGroupSetupBOL.pur_ret_coa ),
                                                        objProvider.CreateInitializedParameter("@prov_pay_coa",DbType.Int64, 0),
                                                        objProvider.CreateInitializedParameter("@cogs_coa",DbType.Int64, 0),
                                                        objProvider.CreateInitializedParameter("@stk_adj_coa",DbType.Int64, 0 ),
                                                        objProvider.CreateInitializedParameter("@dep_coa",DbType.Int64, ObjAddItemGroupSetupBOL.dep_coa ),
                                                        objProvider.CreateInitializedParameter("@asset_coa",DbType.Int64, ObjAddItemGroupSetupBOL.asset_coa ),
                                                        objProvider.CreateInitializedParameter("@create_id",DbType.Int64, ObjAddItemGroupSetupBOL.create_id ),
                                                        objProvider.CreateInitializedParameter("@mod_id",DbType.Int64, ObjAddItemGroupSetupBOL.mod_id ),
                                                        objProvider.CreateInitializedParameter("@InterBranch_sls_coa",DbType.Int64, ObjAddItemGroupSetupBOL.InterBranch_sls_coa ),
                                                        objProvider.CreateInitializedParameter("@InterBranch_pur_coa",DbType.Int64, ObjAddItemGroupSetupBOL.InterBranch_pur_coa ),

                                                    };
                string Result = string.Empty;
                if (ObjAddItemGroupSetupBOL.FormMode == "1")
                {
                    Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_UpdGrpDetail", prmContentAddUpdate));
                }
                else
                {
                    Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_InsGrpDetail", prmContentAddUpdate));
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

        public string DeleteItemGroup(int item_grp_id, int comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentAddUpdate = {
                                        objProvider.CreateInitializedParameter("@item_grp_id",DbType.Int64, item_grp_id),
                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comp_id),
                                      };
                string Result = string.Empty;
                Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_DelItemParentGrp", prmContentAddUpdate));
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
        public string ChkPGroupDependency(int item_grp_id, int comp_id)
        {
            try
            {
                string Result = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentCheck = {
                                        objProvider.CreateInitializedParameter("@item_grp_id",DbType.Int64, item_grp_id),
                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comp_id),
                                      };
               
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ChkItemParentGrpDependency", prmContentCheck);
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

        public DataSet GetSelectedParentDetail(string item_grp_struc, int CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@item_grp_struc",DbType.String, item_grp_struc),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$item$per$detail", prmContentGetDetails);
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


        public JObject GetAllItemGrpBl(ItemMenuSearchModel ObjItemMenuSearchModel)
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

                ParentNode ParentNod = new ParentNode();
                childrenNode ChildNod = new childrenNode();
                SubchildrenNode SubChildNod = new SubchildrenNode();
                SubSubchildrenNode SubSubChildNod = new SubSubchildrenNode();
                SubSubSubchildrenNode SubSubSubChildNod = new SubSubSubchildrenNode();
                Header Headertree = new Header();

                if (PresentNode.Rows.Count > 0)
                {
                    //string HTMLString = "";
                    for (int x = 0; x < PresentNode.Rows.Count; x++)
                    //for (int x = 0; x < 1; x++)
                    {
                        JObject NewObj = new JObject();
                        var Finaldata = string.Empty;

                        List<childrenNode> NodeChild = new List<childrenNode>();
                        string PNodeName = string.Empty;
                        PNodeName = PresentNode.Rows[x]["item_grp_id"].ToString();

                        DataView DV = new DataView(ChildNode);
                        DV.RowFilter = "item_grp_par_id='" + PNodeName + "'";
                        DataTable PreNode = new DataTable();
                        PreNode = DV.ToTable();
                        if (PreNode.Rows.Count > 0)
                        {
                            for (int y = 0; y < PreNode.Rows.Count; y++)
                            {
                                List<SubchildrenNode> NodeSubChild = new List<SubchildrenNode>();
                                DataView DV1 = new DataView(SubChildNode);
                                DV1.RowFilter = "item_grp_par_id='" + PreNode.Rows[y]["item_grp_id"].ToString() + "'";
                                DataTable ChihNode = new DataTable();
                                ChihNode = DV1.ToTable();
                                if (ChihNode.Rows.Count > 0)
                                {
                                    for (int za = 0; za < ChihNode.Rows.Count; za++)
                                    {
                                        List<SubSubchildrenNode> NodeSubSubChild = new List<SubSubchildrenNode>();
                                        DataView DV2 = new DataView(SubSubChildNode);
                                        DV2.RowFilter = "item_grp_par_id='" + ChihNode.Rows[za]["item_grp_id"].ToString() + "'";
                                        DataTable SubSubChihNode = new DataTable();
                                        SubSubChihNode = DV2.ToTable();
                                        if (SubSubChihNode.Rows.Count > 0)
                                        {
                                            for (int ay = 0; ay < SubSubChihNode.Rows.Count; ay++)
                                            {
                                                List<SubSubSubchildrenNode> NodeSubSubSubChild = new List<SubSubSubchildrenNode>();
                                                DataView DV3 = new DataView(SubSubSubChildNode);
                                                DV3.RowFilter = "item_grp_par_id='" + SubSubChihNode.Rows[ay]["item_grp_id"].ToString() + "'";
                                                DataTable SubSubSubChihNode = new DataTable();
                                                SubSubSubChihNode = DV3.ToTable();
                                                if (SubSubSubChihNode.Rows.Count > 0)
                                                {
                                                    for (int az = 0; az < SubSubSubChihNode.Rows.Count; az++)
                                                    {
                                                        NodeSubSubSubChild.Add(new SubSubSubchildrenNode
                                                        {
                                                            label = SubSubSubChihNode.Rows[az]["item_group_name"].ToString(),
                                                            value = SubSubSubChihNode.Rows[az]["item_grp_id"].ToString(),
                                                        });
                                                    }
                                                }
                                                NodeSubSubChild.Add(new SubSubchildrenNode
                                                {
                                                    label = SubSubChihNode.Rows[ay]["item_group_name"].ToString(),
                                                    value = SubSubChihNode.Rows[ay]["item_grp_id"].ToString(),
                                                    children = NodeSubSubSubChild
                                                });
                                            }
                                        }
                                        NodeSubChild.Add(new SubchildrenNode()
                                        {
                                            label = ChihNode.Rows[za]["item_group_name"].ToString(),
                                            value = ChihNode.Rows[za]["item_grp_id"].ToString(),
                                            children = NodeSubSubChild
                                        });
                                    }
                                }
                                NodeChild.Add(new childrenNode()
                                {
                                    label = PreNode.Rows[y]["item_group_name"].ToString(),
                                    value = PreNode.Rows[y]["item_grp_id"].ToString(),
                                    children = NodeSubChild
                                });
                            }
                        }

                        ParentNod.children = NodeChild;
                        ParentNod.label = PresentNode.Rows[x]["item_group_name"].ToString();
                        ParentNod.value = PresentNode.Rows[x]["item_grp_id"].ToString();
                        Headertree.TreeStr = ParentNod;

                        //Parent DataF = new Parent();
                        //DataF = PresentNode;
                        //var Fdata = string.Empty;
                        var Fdata = JsonConvert.SerializeObject(Headertree);
                        //JObject Objc = JObject.Parse(Fdata);
                        //Fdata = PresentNode.ToString();

                        Finaldata = Fdata;

                        if (!string.IsNullOrEmpty(Finaldata))
                        {
                            if (x != 0)
                            {
                                //Finaldata = Finaldata.Replace("TreeStr", x.ToString());
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

                    //FinalList = "";
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
