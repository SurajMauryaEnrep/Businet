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
using EnRepMobileWeb.MODELS.BusinessLayer.AccountGroup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AccountGroupSetup;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.AccountGroupSetup
{
   public class AccountGroup_SERVICES : AccountGroup_ISERVICES
    {
        public DataSet GetAllAccGrp(AccMenuSearchModel ObjAccMenuSearchModel)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, ObjAccMenuSearchModel.Comp_ID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Acc$Grp$detail", prmContentGetDetails);
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
        public DataTable GetAccGroupSetup(int CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                         objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Acc$parent$grp", prmContentGetDetails).Tables[0];
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
        public string InsertAccGrpDetail(AccountGroupModel ObjAddAccGroupSetupBOL)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentAddUpdate = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, ObjAddAccGroupSetupBOL.comp_id ),
                                                        objProvider.CreateInitializedParameter("@acc_grp_id",DbType.Int64, ObjAddAccGroupSetupBOL.acc_grp_id),
                                                        objProvider.CreateInitializedParameter("@acc_group_name",DbType.String, ObjAddAccGroupSetupBOL.acc_group_name ),
                                                        objProvider.CreateInitializedParameter("@acc_grp_struc",DbType.String, ObjAddAccGroupSetupBOL.parent_acc_grp_id ),
                                                        objProvider.CreateInitializedParameter("@parent_acc_grp_id",DbType.String, ObjAddAccGroupSetupBOL.parent_acc_grp_id ),
                                                        objProvider.CreateInitializedParameter("@alt_grp_id",DbType.String, ObjAddAccGroupSetupBOL.alt_grp_id ),
                                                        objProvider.CreateInitializedParameter("@grp_seq_no",DbType.String, ObjAddAccGroupSetupBOL.grp_seq_no ),
                                                        objProvider.CreateInitializedParameter("@grp_type",DbType.String, ObjAddAccGroupSetupBOL.grp_type ),
                                                        objProvider.CreateInitializedParameter("@create_id",DbType.Int64, ObjAddAccGroupSetupBOL.create_id ),
                                                        objProvider.CreateInitializedParameter("@mod_id",DbType.Int64, ObjAddAccGroupSetupBOL.mod_id ),

                                                    };
                string Result = string.Empty;
                if (ObjAddAccGroupSetupBOL.FormMode == "1")
                {
                    Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_Update$AccGrpDetail", prmContentAddUpdate));
                }
                else
                {
                    Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_Insert$AccGrp$Detail", prmContentAddUpdate));
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
        public DataSet GetDefaultAccGrp(int CompId,string GroupID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                          objProvider.CreateInitializedParameter("@grp_id",DbType.String, GroupID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_default$Acc$Grp", prmContentGetDetails);
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
        public DataSet GetAccDetail(string AccGrpId, int CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@acc_grp_id",DbType.String, AccGrpId),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Get$Acc$GrpDetail", prmContentGetDetails);
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
        public DataSet GetAccViewDetail(string AccGrpId, int CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@acc_grp_id",DbType.String, AccGrpId),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Get$Acc$GrpDetail$byname", prmContentGetDetails);
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
        public string DeleteAccGroup(int AccGrpID, int comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] param = {
                                        objProvider.CreateInitializedParameter("@acc_grp_id",DbType.Int64, AccGrpID),
                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comp_id),
                                      };
                string Result = string.Empty;
                Result = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_Del$AccGrp$detail", param));
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
        public JObject GetAllAccGrpBl(AccMenuSearchModel ObjAccMenuSearchModel)
        {
            JObject FinalList = new JObject();
            try
            {
                DataSet GetAccList = GetAllAccGrp(ObjAccMenuSearchModel);

                DataTable PresentNode = new DataTable();
                DataTable ChildNode = new DataTable();
                DataTable SubChildNode = new DataTable();
                DataTable SubSubChildNode = new DataTable();

                PresentNode = GetAccList.Tables[0];
                ChildNode = GetAccList.Tables[1];
                SubChildNode = GetAccList.Tables[2];
                SubSubChildNode = GetAccList.Tables[3];

                ParentNode ParentNod = new ParentNode();
                childrenNode ChildNod = new childrenNode();
                SubchildrenNode SubChildNod = new SubchildrenNode();
                SubSubchildrenNode SubSubChildNod = new SubSubchildrenNode();
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
                        PNodeName = PresentNode.Rows[x]["acc_grp_id"].ToString();

                        DataView DV = new DataView(ChildNode);
                        DV.RowFilter = "parent_acc_grp_id='" + PNodeName + "'";
                        DataTable PreNode = new DataTable();
                        PreNode = DV.ToTable();
                        if (PreNode.Rows.Count > 0)
                        {
                            for (int y = 0; y < PreNode.Rows.Count; y++)
                            {
                                List<SubchildrenNode> NodeSubChild = new List<SubchildrenNode>();
                                DataView DV1 = new DataView(SubChildNode);
                                DV1.RowFilter = "parent_acc_grp_id='" + PreNode.Rows[y]["acc_grp_id"].ToString() + "'";
                                DataTable ChihNode = new DataTable();
                                ChihNode = DV1.ToTable();
                                if (ChihNode.Rows.Count > 0)
                                {
                                    for (int za = 0; za < ChihNode.Rows.Count; za++)
                                    {
                                        List<SubSubchildrenNode> NodeSubSubChild = new List<SubSubchildrenNode>();
                                        DataView DV2 = new DataView(SubSubChildNode);
                                        DV2.RowFilter = "parent_acc_grp_id='" + ChihNode.Rows[za]["acc_grp_id"].ToString() + "'";
                                        DataTable SubSubChihNode = new DataTable();
                                        SubSubChihNode = DV2.ToTable();
                                        if (SubSubChihNode.Rows.Count > 0)
                                        {
                                            for (int ay = 0; ay < SubSubChihNode.Rows.Count; ay++)
                                            {
                                                NodeSubSubChild.Add(new SubSubchildrenNode
                                                {
                                                    label = SubSubChihNode.Rows[ay]["acc_group_name"].ToString(),
                                                    value = SubSubChihNode.Rows[ay]["acc_grp_id"].ToString(),
                                                });
                                            }
                                        }
                                        NodeSubChild.Add(new SubchildrenNode()
                                        {
                                            label = ChihNode.Rows[za]["acc_group_name"].ToString(),
                                            value = ChihNode.Rows[za]["acc_grp_id"].ToString(),
                                            children = NodeSubSubChild
                                        });
                                    }
                                }
                                NodeChild.Add(new childrenNode()
                                {
                                    label = PreNode.Rows[y]["acc_group_name"].ToString(),
                                    value = PreNode.Rows[y]["acc_grp_id"].ToString(),
                                    children = NodeSubChild
                                });
                            }
                        }

                        ParentNod.children = NodeChild;
                        ParentNod.label = PresentNode.Rows[x]["acc_group_name"].ToString();
                        ParentNod.value = PresentNode.Rows[x]["acc_grp_id"].ToString();
                        Headertree.TreeStr = ParentNod;

                        var Fdata = JsonConvert.SerializeObject(Headertree);
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
        public string ChkPGroupDependency(int acc_grp_id, int comp_id)
        {
            try
            {
                string Result = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentCheck = {
                                        objProvider.CreateInitializedParameter("@Acc_grp_id",DbType.Int64, acc_grp_id),
                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comp_id),
                                      };

                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_ChkAccParentGrpDependency", prmContentCheck);
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
        public string get_grptype( int comp_id, string acc_grp_id)
        {
            try
            {
                string Result = string.Empty;
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentCheck = {
                                        objProvider.CreateInitializedParameter("@Acc_grpstr_id",DbType.String, acc_grp_id),
                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comp_id),
                                      };

                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get_grptype", prmContentCheck);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0]["grp_type"].ToString();
                }
                else
                {
                    Result = "0";
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
    }
}
