using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemGroupSetup;
using EnRepMobileWeb.MODELS.BusinessLayer.ItemGroupSetup;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
//***All Session Remove By Shubham Maurya on 11-01-2023 ***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.ItemGroupSetup
{
    public class ItemGroupSetupController : Controller
    {
        string UserID,userid,title;
        int Comp_Id;
        string CompID, language = String.Empty;
        string DocumentMenuId = "103101";
        Common_IServices _Common_IServices;
        SearchItemBOL _SearchItemBOL;
        ItemGroupModel _ItemGroupModel;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        ItemGroup_ISERVICES _ItemGroup_ISERVICES;
      
        // GET: BusinessLayer/ItemGroupSetup
        public ItemGroupSetupController(Common_IServices _Common_IServices,ItemGroup_ISERVICES _ItemGroup_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._ItemGroup_ISERVICES = _ItemGroup_ISERVICES;
        }
        public ActionResult ItemGroupSetup()
        {
            try
            {
                CommonPageDetails();
                var _ItemGroupModel = TempData["ModelData"] as ItemGroupModel;
                if (_ItemGroupModel != null)
                {
                    _SearchItemBOL = new SearchItemBOL();
                    //_ItemGroupModel = new ItemGroupModel();
                    int Comp_ID = 0;
                    _ItemGroupModel.item_grp_id = null;
                    ViewBag.MenuPageName = getDocumentName();
                    _ItemGroupModel.Title = title;
                    //if(Session["Command"]!=null)
                    if (_ItemGroupModel.Command != null)
                    {
                        //if (Session["Command"].ToString() == "Delete")
                        if (_ItemGroupModel.Command == "Delete")
                        {
                            //Session["TransType"] = "Refresh";
                            _ItemGroupModel.TransType = "Refresh";
                        }
                        else
                        {
                            //Session["TransType"] = "Save";
                            _ItemGroupModel.TransType = "Save";
                        }
                    }
                    else
                    {
                        //Session["TransType"] = "Save";
                        _ItemGroupModel.TransType = "Save";
                    }


                    //Session["Command"] = "Test";
                    _ItemGroupModel.Command = "Test";
                    //Session["BtnName"] = "BtnToDetailPage";
                    _ItemGroupModel.BtnName = "BtnToDetailPage";

                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }

                    dt = GetGroupParentList();
                    List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                    foreach (DataRow dt in dt.Rows)
                    {
                        AccountGroup _AccountGroupList = new AccountGroup();
                        _AccountGroupList.item_grp_struc = dt["item_grp_struc"].ToString();
                        _AccountGroupList.par_grp_name = dt["par_grp_name"].ToString();
                        _ItemAccountGroupList.Add(_AccountGroupList);
                    }
                    _ItemAccountGroupList.Insert(0, new AccountGroup() { item_grp_struc = "-1", par_grp_name = "---Select---" });
                    _ItemGroupModel.ParentItemGroup = _ItemAccountGroupList;

                    GetLocalSaleAccount(_ItemGroupModel);
                    GetExportSaleAccount(_ItemGroupModel);
                    GetLocalPurchaseAccount(_ItemGroupModel);
                    GetImportPurchaseAccount(_ItemGroupModel);
                    GetSaleReturnAccount(_ItemGroupModel);
                    GetPurchaseReturnAccount(_ItemGroupModel);
                    //GetProvisionalPayableAccount(_ItemGroupModel);
                    //GetStockAccount(_ItemGroupModel);
                    //GetStockAdjustmentAccount(_ItemGroupModel);
                    GetDepreciationAccount(_ItemGroupModel);
                    GetDiscountAccount(_ItemGroupModel);
                    //GetcostOfGoodsSoldAccount(_ItemGroupModel);
                    GetAssetAccount(_ItemGroupModel);
                    Boolean i_batch, i_capg, i_cons, i_exp, i_pur, i_qc, i_Sam, i_serial, i_sls, i_srvc, i_stk, i_wip, i_pack, i_catalog;

                    string GroupID = string.Empty;
                    //if (Session["GroupID"] != null)
                    if (_ItemGroupModel.GroupID != null)
                    {
                        //if (Session["GroupID"].ToString() == "")
                        if (_ItemGroupModel.GroupID == "")
                        {

                            GroupID = "0";
                        }
                        else
                        {
                            //GroupID = Session["GroupID"].ToString();
                            GroupID = _ItemGroupModel.GroupID;
                        }

                    }
                    else
                    {
                        GroupID = "0";
                    }

                    DataSet ds = _ItemGroup_ISERVICES.GetDefaultItemDetail(Comp_ID, GroupID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["i_batch"].ToString() == "Y")
                            i_batch = true;
                        else
                            i_batch = false;
                        if (ds.Tables[0].Rows[0]["i_capg"].ToString() == "Y")
                            i_capg = true;
                        else
                            i_capg = false;
                        if (ds.Tables[0].Rows[0]["i_cons"].ToString() == "Y")
                            i_cons = true;
                        else
                            i_cons = false;
                        if (ds.Tables[0].Rows[0]["i_exp"].ToString() == "Y")
                            i_exp = true;
                        else
                            i_exp = false;
                        if (ds.Tables[0].Rows[0]["i_pur"].ToString() == "Y")
                            i_pur = true;
                        else
                            i_pur = false;
                        if (ds.Tables[0].Rows[0]["i_qc"].ToString() == "Y")
                            i_qc = true;
                        else
                            i_qc = false;
                        if (ds.Tables[0].Rows[0]["i_Sam"].ToString() == "Y")
                            i_Sam = true;
                        else
                            i_Sam = false;
                        if (ds.Tables[0].Rows[0]["i_serial"].ToString() == "Y")
                            i_serial = true;
                        else
                            i_serial = false;
                        if (ds.Tables[0].Rows[0]["i_sls"].ToString() == "Y")
                            i_sls = true;
                        else
                            i_sls = false;
                        if (ds.Tables[0].Rows[0]["i_srvc"].ToString() == "Y")
                            i_srvc = true;
                        else
                            i_srvc = false;
                        if (ds.Tables[0].Rows[0]["i_stk"].ToString() == "Y")
                            i_stk = true;
                        else
                            i_stk = false;
                        if (ds.Tables[0].Rows[0]["i_wip"].ToString() == "Y")
                            i_wip = true;
                        else
                            i_wip = false;
                        if (ds.Tables[0].Rows[0]["i_pack"].ToString() == "Y")
                            i_pack = true;
                        else
                            i_pack = false;
                        if (ds.Tables[0].Rows[0]["i_catalog"].ToString() == "Y")
                            i_catalog = true;
                        else
                            i_catalog = false;
                        _ItemGroupModel.item_grp_id = int.Parse(ds.Tables[0].Rows[0]["item_grp_id"].ToString());
                        _ItemGroupModel.item_group_name = ds.Tables[0].Rows[0]["item_group_name"].ToString();
                        _ItemGroupModel.item_grp_par_id = ds.Tables[0].Rows[0]["ItemParId"].ToString();
                        _ItemGroupModel.issue_method = ds.Tables[0].Rows[0]["issue_method"].ToString();
                        _ItemGroupModel.cost_method = ds.Tables[0].Rows[0]["cost_method"].ToString();
                        _ItemGroupModel.It_Remarks = ds.Tables[0].Rows[0]["It_Remarks"].ToString();

                        _ItemGroupModel.i_batch = i_batch;
                        _ItemGroupModel.i_capg = i_capg;
                        _ItemGroupModel.i_cons = i_cons;
                        _ItemGroupModel.i_exp = i_exp;
                        _ItemGroupModel.i_pur = i_pur;
                        _ItemGroupModel.i_qc = i_qc;
                        _ItemGroupModel.i_sam = i_Sam;
                        _ItemGroupModel.i_serial = i_serial;
                        _ItemGroupModel.i_sls = i_sls;
                        _ItemGroupModel.i_srvc = i_srvc;
                        _ItemGroupModel.i_stk = i_stk;
                        _ItemGroupModel.i_wip = i_wip;
                        _ItemGroupModel.i_pack = i_pack;
                        _ItemGroupModel.i_catalog = i_catalog;

                        _ItemGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        _ItemGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                        _ItemGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        _ItemGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();

                        _ItemGroupModel.loc_sls_coa = int.Parse(ds.Tables[0].Rows[0]["loc_sls_coa"].ToString());
                        _ItemGroupModel.sal_ret_coa = int.Parse(ds.Tables[0].Rows[0]["sal_ret_coa"].ToString());
                        _ItemGroupModel.exp_sls_coa = int.Parse(ds.Tables[0].Rows[0]["exp_sls_coa"].ToString());
                        _ItemGroupModel.loc_pur_coa = int.Parse(ds.Tables[0].Rows[0]["loc_pur_coa"].ToString());
                        _ItemGroupModel.imp_pur_coa = int.Parse(ds.Tables[0].Rows[0]["imp_pur_coa"].ToString());
                        _ItemGroupModel.stk_coa = int.Parse(ds.Tables[0].Rows[0]["stk_coa"].ToString());
                        _ItemGroupModel.disc_coa = int.Parse(ds.Tables[0].Rows[0]["Disc_coa"].ToString());
                        _ItemGroupModel.pur_ret_coa = int.Parse(ds.Tables[0].Rows[0]["pur_ret_coa"].ToString());
                        //_ItemGroupModel.prov_pay_coa = int.Parse(ds.Tables[0].Rows[0]["prov_pay_coa"].ToString());
                        //_ItemGroupModel.cogs_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_coa"].ToString());
                        //_ItemGroupModel.stk_adj_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_adj_coa"].ToString());
                        _ItemGroupModel.dep_coa = int.Parse(ds.Tables[0].Rows[0]["dep_coa"].ToString());
                        _ItemGroupModel.asset_coa = int.Parse(ds.Tables[0].Rows[0]["asset_coa"].ToString());
                        _ItemGroupModel.InterBranch_sls_coa = int.Parse(ds.Tables[0].Rows[0]["interbr_sls_coa"].ToString());
                        _ItemGroupModel.InterBranch_pur_coa = int.Parse(ds.Tables[0].Rows[0]["interbr_pur_coa"].ToString());
                        _ItemGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                    }
                    //var transtype = Session["TransType"].ToString();
                    var transtype = _ItemGroupModel.TransType;


                    //_ItemGroupModel.TransType = Session["TransType"].ToString();
                    //_ItemGroupModel.TransType = _ItemGroupModel.TransType;

                    //if (Session["TransType"].ToString() == "Save")
                    if (_ItemGroupModel.TransType == "Save")
                    {
                        //ViewBag.Message = "New";
                        _ItemGroupModel.Message = "New";
                    }
                    else
                    {
                        //ViewBag.Message = Session["Message"].ToString();
                        _ItemGroupModel.Message = _ItemGroupModel.Message;
                    }
                    if ((GroupID == "0" || GroupID == "") && _ItemGroupModel.item_grp_id == null)
                    {
                        _ItemGroupModel.BtnName = "BtnRefresh";
                    }
                    _ItemGroupModel.DeleteCommand = null;
                    return View("~/Areas/BusinessLayer/Views/ItemGroupSetup/ItemGroupSetup.cshtml", _ItemGroupModel);
                }
                else
                {
                    _SearchItemBOL = new SearchItemBOL();
                    _ItemGroupModel = new ItemGroupModel();
                    int Comp_ID = 0;
                    _ItemGroupModel.item_grp_id = null;
                    ViewBag.MenuPageName = getDocumentName();
                    _ItemGroupModel.Title = title;
                    //if(Session["Command"]!=null)
                    if (_ItemGroupModel.Command != null)
                    {
                        //if (Session["Command"].ToString() == "Delete")
                        if (_ItemGroupModel.Command == "Delete")
                        {
                            //Session["TransType"] = "Refresh";
                            _ItemGroupModel.TransType = "Refresh";
                        }
                        else
                        {
                            //Session["TransType"] = "Save";
                            _ItemGroupModel.TransType = "Save";
                        }
                    }
                    else
                    {
                        //Session["TransType"] = "Save";
                        _ItemGroupModel.TransType = "Save";
                    }


                    //Session["Command"] = "Test";
                    _ItemGroupModel.Command = "Test";
                    //Session["BtnName"] = "BtnToDetailPage";
                    _ItemGroupModel.BtnName = "BtnToDetailPage";

                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }
                    dt = GetGroupParentList();
                    List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                    foreach (DataRow dt in dt.Rows)
                    {
                        AccountGroup _AccountGroupList = new AccountGroup();
                        _AccountGroupList.item_grp_struc = dt["item_grp_struc"].ToString();
                        _AccountGroupList.par_grp_name = dt["par_grp_name"].ToString();
                        _ItemAccountGroupList.Add(_AccountGroupList);
                    }
                    _ItemAccountGroupList.Insert(0, new AccountGroup() { item_grp_struc = "-1", par_grp_name = "---Select---" });
                    _ItemGroupModel.ParentItemGroup = _ItemAccountGroupList;

                    GetLocalSaleAccount(_ItemGroupModel);
                    GetExportSaleAccount(_ItemGroupModel);
                    GetLocalPurchaseAccount(_ItemGroupModel);
                    GetImportPurchaseAccount(_ItemGroupModel);
                    GetSaleReturnAccount(_ItemGroupModel);
                    GetPurchaseReturnAccount(_ItemGroupModel);
                    //GetProvisionalPayableAccount(_ItemGroupModel);
                    //GetStockAccount(_ItemGroupModel);
                    //GetStockAdjustmentAccount(_ItemGroupModel);
                    GetDepreciationAccount(_ItemGroupModel);
                    GetDiscountAccount(_ItemGroupModel);
                    //GetcostOfGoodsSoldAccount(_ItemGroupModel);
                    GetAssetAccount(_ItemGroupModel);
                    Boolean i_batch, i_capg, i_cons, i_exp, i_pur, i_qc, i_Sam, i_serial, i_sls, i_srvc, i_stk, i_wip, i_pack, i_catalog;

                    string GroupID = string.Empty;
                    //if (Session["GroupID"] != null)
                    if (_ItemGroupModel.GroupID != null)
                    {
                        //if (Session["GroupID"].ToString() == "")
                        if (_ItemGroupModel.GroupID == "")
                        {
                            GroupID = "0";
                        }
                        else
                        {
                            //GroupID = Session["GroupID"].ToString();
                            GroupID = _ItemGroupModel.GroupID;
                        }
                    }
                    else
                    {
                        GroupID = "0";
                    }
                    DataSet ds = _ItemGroup_ISERVICES.GetDefaultItemDetail(Comp_ID, GroupID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["i_batch"].ToString() == "Y")
                            i_batch = true;
                        else
                            i_batch = false;
                        if (ds.Tables[0].Rows[0]["i_capg"].ToString() == "Y")
                            i_capg = true;
                        else
                            i_capg = false;
                        if (ds.Tables[0].Rows[0]["i_cons"].ToString() == "Y")
                            i_cons = true;
                        else
                            i_cons = false;
                        if (ds.Tables[0].Rows[0]["i_exp"].ToString() == "Y")
                            i_exp = true;
                        else
                            i_exp = false;
                        if (ds.Tables[0].Rows[0]["i_pur"].ToString() == "Y")
                            i_pur = true;
                        else
                            i_pur = false;
                        if (ds.Tables[0].Rows[0]["i_qc"].ToString() == "Y")
                            i_qc = true;
                        else
                            i_qc = false;
                        if (ds.Tables[0].Rows[0]["i_Sam"].ToString() == "Y")
                            i_Sam = true;
                        else
                            i_Sam = false;
                        if (ds.Tables[0].Rows[0]["i_serial"].ToString() == "Y")
                            i_serial = true;
                        else
                            i_serial = false;
                        if (ds.Tables[0].Rows[0]["i_sls"].ToString() == "Y")
                            i_sls = true;
                        else
                            i_sls = false;
                        if (ds.Tables[0].Rows[0]["i_srvc"].ToString() == "Y")
                            i_srvc = true;
                        else
                            i_srvc = false;
                        if (ds.Tables[0].Rows[0]["i_stk"].ToString() == "Y")
                            i_stk = true;
                        else
                            i_stk = false;
                        if (ds.Tables[0].Rows[0]["i_wip"].ToString() == "Y")
                            i_wip = true;
                        else
                            i_wip = false;
                        if (ds.Tables[0].Rows[0]["i_pack"].ToString() == "Y")
                            i_pack = true;
                        else
                            i_pack = false;
                        if (ds.Tables[0].Rows[0]["i_catalog"].ToString() == "Y")
                            i_catalog = true;
                        else
                            i_catalog = false;
                        _ItemGroupModel.item_grp_id = int.Parse(ds.Tables[0].Rows[0]["item_grp_id"].ToString());
                        _ItemGroupModel.item_group_name = ds.Tables[0].Rows[0]["item_group_name"].ToString();
                        _ItemGroupModel.item_grp_par_id = ds.Tables[0].Rows[0]["ItemParId"].ToString();
                        _ItemGroupModel.issue_method = ds.Tables[0].Rows[0]["issue_method"].ToString();
                        _ItemGroupModel.cost_method = ds.Tables[0].Rows[0]["cost_method"].ToString();
                        _ItemGroupModel.It_Remarks = ds.Tables[0].Rows[0]["It_Remarks"].ToString();

                        _ItemGroupModel.i_batch = i_batch;
                        _ItemGroupModel.i_capg = i_capg;
                        _ItemGroupModel.i_cons = i_cons;
                        _ItemGroupModel.i_exp = i_exp;
                        _ItemGroupModel.i_pur = i_pur;
                        _ItemGroupModel.i_qc = i_qc;
                        _ItemGroupModel.i_sam = i_Sam;
                        _ItemGroupModel.i_serial = i_serial;
                        _ItemGroupModel.i_sls = i_sls;
                        _ItemGroupModel.i_srvc = i_srvc;
                        _ItemGroupModel.i_stk = i_stk;
                        _ItemGroupModel.i_wip = i_wip;
                        _ItemGroupModel.i_pack = i_pack;
                        _ItemGroupModel.i_catalog = i_catalog;

                        _ItemGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        _ItemGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                        _ItemGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        _ItemGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();

                        _ItemGroupModel.loc_sls_coa = int.Parse(ds.Tables[0].Rows[0]["loc_sls_coa"].ToString());
                        _ItemGroupModel.sal_ret_coa = int.Parse(ds.Tables[0].Rows[0]["sal_ret_coa"].ToString());
                        _ItemGroupModel.exp_sls_coa = int.Parse(ds.Tables[0].Rows[0]["exp_sls_coa"].ToString());
                        _ItemGroupModel.loc_pur_coa = int.Parse(ds.Tables[0].Rows[0]["loc_pur_coa"].ToString());
                        _ItemGroupModel.imp_pur_coa = int.Parse(ds.Tables[0].Rows[0]["imp_pur_coa"].ToString());
                        _ItemGroupModel.stk_coa = int.Parse(ds.Tables[0].Rows[0]["stk_coa"].ToString());
                        _ItemGroupModel.disc_coa = int.Parse(ds.Tables[0].Rows[0]["Disc_coa"].ToString());
                        _ItemGroupModel.pur_ret_coa = int.Parse(ds.Tables[0].Rows[0]["pur_ret_coa"].ToString());
                        //_ItemGroupModel.prov_pay_coa = int.Parse(ds.Tables[0].Rows[0]["prov_pay_coa"].ToString());
                        //_ItemGroupModel.cogs_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_coa"].ToString());
                        //_ItemGroupModel.stk_adj_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_adj_coa"].ToString());
                        _ItemGroupModel.dep_coa = int.Parse(ds.Tables[0].Rows[0]["dep_coa"].ToString());
                        _ItemGroupModel.asset_coa = int.Parse(ds.Tables[0].Rows[0]["asset_coa"].ToString());
                        _ItemGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                        _ItemGroupModel.InterBranch_sls_coa = int.Parse(ds.Tables[0].Rows[0]["interbr_sls_coa"].ToString());
                        _ItemGroupModel.InterBranch_pur_coa = int.Parse((ds.Tables[0].Rows[0]["interbr_pur_coa"].ToString()));
                    }
                    //var transtype = Session["TransType"].ToString();
                    var transtype = _ItemGroupModel.TransType;


                    //_ItemGroupModel.TransType = Session["TransType"].ToString();
                    //_ItemGroupModel.TransType = _ItemGroupModel.TransType;

                    //if (Session["TransType"].ToString() == "Save")
                    if (_ItemGroupModel.TransType == "Save")
                    {
                        //ViewBag.Message = "New";
                        _ItemGroupModel.Message = "New";
                    }
                    else
                    {
                        //ViewBag.Message = Session["Message"].ToString();
                        _ItemGroupModel.Message = _ItemGroupModel.Message;
                    }
                    if((GroupID =="0" || GroupID=="") && _ItemGroupModel.item_grp_id==null)
                    {
                        _ItemGroupModel.BtnName = "BtnRefresh";
                    }
                    return View("~/Areas/BusinessLayer/Views/ItemGroupSetup/ItemGroupSetup.cshtml", _ItemGroupModel);
                }              
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);              
                return View("Error");
                throw Ex;
            }                    
        }
        private void CommonPageDetails()
        {
            try
            {
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
                ViewBag.VBRoleList = ds.Tables[3];
                ViewBag.StatusList = ds.Tables[4];

                string[] Docpart = DocumentName.Split('>');
                int len = Docpart.Length;
                if (len > 1)
                {
                    title = Docpart[len - 1].Trim();
                }
                ViewBag.MenuPageName = DocumentName;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ItemGroupSetupAdd(string GroupID, string TransType, string BtnName, string command,string ChkPgroup)
        {
            try
            {
                CommonPageDetails();
                var _ItemGroupModel = TempData["ModelData"] as ItemGroupModel;
                if (_ItemGroupModel != null)
                {
                    _SearchItemBOL = new SearchItemBOL();
                    //_ItemGroupModel = new ItemGroupModel();
                    int Comp_ID = 0;
                    //_ItemGroupModel.item_grp_id = Convert.ToInt32(Session["GroupID"].ToString());
                    _ItemGroupModel.item_grp_id = Convert.ToInt32(_ItemGroupModel.GroupID);
                    //ViewBag.MenuPageName = getDocumentName();
                    _ItemGroupModel.Title = title;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }

                    dt = GetGroupParentList();
                    List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                    foreach (DataRow dt in dt.Rows)
                    {
                        AccountGroup _AccountGroupList = new AccountGroup();
                        _AccountGroupList.item_grp_struc = dt["item_grp_struc"].ToString();
                        _AccountGroupList.par_grp_name = dt["par_grp_name"].ToString();
                        _ItemAccountGroupList.Add(_AccountGroupList);
                    }
                    _ItemAccountGroupList.Insert(0, new AccountGroup() { item_grp_struc = "-1", par_grp_name = "---Select---" });
                    _ItemGroupModel.ParentItemGroup = _ItemAccountGroupList;

                    GetLocalSaleAccount(_ItemGroupModel);
                    GetExportSaleAccount(_ItemGroupModel);
                    GetLocalPurchaseAccount(_ItemGroupModel);
                    GetImportPurchaseAccount(_ItemGroupModel);
                    GetSaleReturnAccount(_ItemGroupModel);
                    GetPurchaseReturnAccount(_ItemGroupModel);
                    //GetProvisionalPayableAccount(_ItemGroupModel);
                    GetStockAccount(_ItemGroupModel);
                    //GetStockAdjustmentAccount(_ItemGroupModel);
                    GetDepreciationAccount(_ItemGroupModel);
                    GetDiscountAccount(_ItemGroupModel);
                    //GetcostOfGoodsSoldAccount(_ItemGroupModel);
                    GetAssetAccount(_ItemGroupModel);
                    Boolean i_batch, i_capg, i_cons, i_exp, i_pur, i_qc, i_Sam, i_serial, i_sls, i_srvc, i_stk, i_wip, i_pack, i_catalog;


                    //var transtype = Session["TransType"].ToString();
                    var transtype = _ItemGroupModel.TransType;

                    if (transtype == "Update" || transtype == "Edit")
                    {

                        //string GroupID = Session["GroupID"].ToString();
                        GroupID = _ItemGroupModel.GroupID;
                        string ItemGrpId = (_ItemGroupModel.item_grp_id).ToString();
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = int.Parse(Session["CompId"].ToString());
                        }
                        DataSet ds = _ItemGroup_ISERVICES.GetItemDetail(ItemGrpId, Comp_ID);

                        if (ds.Tables[0].Rows[0]["i_batch"].ToString() == "Y")
                            i_batch = true;
                        else
                            i_batch = false;
                        if (ds.Tables[0].Rows[0]["i_capg"].ToString() == "Y")
                            i_capg = true;
                        else
                            i_capg = false;
                        if (ds.Tables[0].Rows[0]["i_cons"].ToString() == "Y")
                            i_cons = true;
                        else
                            i_cons = false;
                        if (ds.Tables[0].Rows[0]["i_exp"].ToString() == "Y")
                            i_exp = true;
                        else
                            i_exp = false;
                        if (ds.Tables[0].Rows[0]["i_pur"].ToString() == "Y")
                            i_pur = true;
                        else
                            i_pur = false;
                        if (ds.Tables[0].Rows[0]["i_qc"].ToString() == "Y")
                            i_qc = true;
                        else
                            i_qc = false;
                        if (ds.Tables[0].Rows[0]["i_Sam"].ToString() == "Y")
                            i_Sam = true;
                        else
                            i_Sam = false;
                        if (ds.Tables[0].Rows[0]["i_serial"].ToString() == "Y")
                            i_serial = true;
                        else
                            i_serial = false;
                        if (ds.Tables[0].Rows[0]["i_sls"].ToString() == "Y")
                            i_sls = true;
                        else
                            i_sls = false;
                        if (ds.Tables[0].Rows[0]["i_srvc"].ToString() == "Y")
                            i_srvc = true;
                        else
                            i_srvc = false;
                        if (ds.Tables[0].Rows[0]["i_stk"].ToString() == "Y")
                            i_stk = true;
                        else
                            i_stk = false;
                        if (ds.Tables[0].Rows[0]["i_wip"].ToString() == "Y")
                            i_wip = true;
                        else
                            i_wip = false;
                        if (ds.Tables[0].Rows[0]["i_pack"].ToString() == "Y")
                            i_pack = true;
                        else
                            i_pack = false;
                        if (ds.Tables[0].Rows[0]["i_catalog"].ToString() == "Y")
                            i_catalog = true;
                        else
                            i_catalog = false;
                        _ItemGroupModel.item_grp_id = int.Parse(ds.Tables[0].Rows[0]["item_grp_id"].ToString());
                        _ItemGroupModel.item_group_name = ds.Tables[0].Rows[0]["item_group_name"].ToString();
                        _ItemGroupModel.item_grp_par_id = ds.Tables[0].Rows[0]["ItemParId"].ToString();
                        _ItemGroupModel.issue_method = ds.Tables[0].Rows[0]["issue_method"].ToString();
                        _ItemGroupModel.cost_method = ds.Tables[0].Rows[0]["cost_method"].ToString();
                        _ItemGroupModel.It_Remarks = ds.Tables[0].Rows[0]["It_Remarks"].ToString();

                        _ItemGroupModel.i_batch = i_batch;
                        _ItemGroupModel.i_capg = i_capg;
                        _ItemGroupModel.i_cons = i_cons;
                        _ItemGroupModel.i_exp = i_exp;
                        _ItemGroupModel.i_pur = i_pur;
                        _ItemGroupModel.i_qc = i_qc;
                        _ItemGroupModel.i_sam = i_Sam;
                        _ItemGroupModel.i_serial = i_serial;
                        _ItemGroupModel.i_sls = i_sls;
                        _ItemGroupModel.i_srvc = i_srvc;
                        _ItemGroupModel.i_stk = i_stk;
                        _ItemGroupModel.i_wip = i_wip;
                        _ItemGroupModel.i_pack = i_pack;
                        _ItemGroupModel.i_catalog = i_catalog;

                        _ItemGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        _ItemGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                        _ItemGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        _ItemGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();

                        _ItemGroupModel.loc_sls_coa = int.Parse(ds.Tables[0].Rows[0]["loc_sls_coa"].ToString());
                        _ItemGroupModel.sal_ret_coa = int.Parse(ds.Tables[0].Rows[0]["sal_ret_coa"].ToString());
                        _ItemGroupModel.exp_sls_coa = int.Parse(ds.Tables[0].Rows[0]["exp_sls_coa"].ToString());
                        _ItemGroupModel.loc_pur_coa = int.Parse(ds.Tables[0].Rows[0]["loc_pur_coa"].ToString());
                        _ItemGroupModel.imp_pur_coa = int.Parse(ds.Tables[0].Rows[0]["imp_pur_coa"].ToString());
                        _ItemGroupModel.stk_coa = int.Parse(ds.Tables[0].Rows[0]["stk_coa"].ToString());
                        _ItemGroupModel.disc_coa = int.Parse(ds.Tables[0].Rows[0]["Disc_coa"].ToString());
                        _ItemGroupModel.pur_ret_coa = int.Parse(ds.Tables[0].Rows[0]["pur_ret_coa"].ToString());
                        //_ItemGroupModel.prov_pay_coa = int.Parse(ds.Tables[0].Rows[0]["prov_pay_coa"].ToString());
                        //_ItemGroupModel.cogs_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_coa"].ToString());
                        //_ItemGroupModel.stk_adj_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_adj_coa"].ToString());
                        _ItemGroupModel.dep_coa = int.Parse(ds.Tables[0].Rows[0]["dep_coa"].ToString());
                        _ItemGroupModel.asset_coa = int.Parse(ds.Tables[0].Rows[0]["asset_coa"].ToString());
                        _ItemGroupModel.Delete_Dependcy =ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                        _ItemGroupModel.InterBranch_sls_coa = int.Parse(ds.Tables[0].Rows[0]["interbr_sls_coa"].ToString());
                        _ItemGroupModel.InterBranch_pur_coa = int.Parse((ds.Tables[0].Rows[0]["interbr_pur_coa"].ToString()));

                    }
                    else
                    {
                        _ItemGroupModel.i_sls = true;
                        _ItemGroupModel.i_pur = true;
                        _ItemGroupModel.act_stat = "Y";
                    }
                    //_ItemGroupModel.TransType = Session["TransType"].ToString();
                    //Session["ChkPgroup"] = null;
                    //_ItemGroupModel.ChkPgroup = null;
                    //ViewBag.Message = Session["Message"].ToString();
                    //_ItemGroupModel.Message = _ItemGroupModel.Message;
                    return View("~/Areas/BusinessLayer/Views/ItemGroupSetup/ItemGroupSetup.cshtml", _ItemGroupModel);
                }
                else
                {
                    _SearchItemBOL = new SearchItemBOL();
                    _ItemGroupModel = new ItemGroupModel();
                    int Comp_ID = 0;
                    //_ItemGroupModel.item_grp_id = Convert.ToInt32(Session["GroupID"].ToString());
                    if (_ItemGroupModel.GroupID == null)
                    {
                        _ItemGroupModel.GroupID = GroupID;
                    }
                    if (_ItemGroupModel.TransType == null)
                    {
                        _ItemGroupModel.TransType = TransType;
                    }
                    if (_ItemGroupModel.BtnName == null)
                    {
                        _ItemGroupModel.BtnName = BtnName;
                    }
                    if (_ItemGroupModel.Command == null)
                    {
                        _ItemGroupModel.Command = command;
                    }
                    if (_ItemGroupModel.ChkPgroup == null)
                    {
                        _ItemGroupModel.ChkPgroup = ChkPgroup;
                    }
                    _ItemGroupModel.item_grp_id = Convert.ToInt32(_ItemGroupModel.GroupID);
                    ViewBag.MenuPageName = getDocumentName();
                    _ItemGroupModel.Title = title;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }

                    dt = GetGroupParentList();
                    List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                    foreach (DataRow dt in dt.Rows)
                    {
                        AccountGroup _AccountGroupList = new AccountGroup();
                        _AccountGroupList.item_grp_struc = dt["item_grp_struc"].ToString();
                        _AccountGroupList.par_grp_name = dt["par_grp_name"].ToString();
                        _ItemAccountGroupList.Add(_AccountGroupList);
                    }
                    _ItemAccountGroupList.Insert(0, new AccountGroup() { item_grp_struc = "-1", par_grp_name = "---Select---" });
                    _ItemGroupModel.ParentItemGroup = _ItemAccountGroupList;

                    GetLocalSaleAccount(_ItemGroupModel);
                    GetExportSaleAccount(_ItemGroupModel);
                    GetLocalPurchaseAccount(_ItemGroupModel);
                    GetImportPurchaseAccount(_ItemGroupModel);
                    GetSaleReturnAccount(_ItemGroupModel);
                    GetPurchaseReturnAccount(_ItemGroupModel);
                    //GetProvisionalPayableAccount(_ItemGroupModel);
                    GetStockAccount(_ItemGroupModel);
                    //GetStockAdjustmentAccount(_ItemGroupModel);
                    GetDepreciationAccount(_ItemGroupModel);
                    GetDiscountAccount(_ItemGroupModel);
                    //GetcostOfGoodsSoldAccount(_ItemGroupModel);
                    GetAssetAccount(_ItemGroupModel);
                    Boolean i_batch, i_capg, i_cons, i_exp, i_pur, i_qc, i_Sam, i_serial, i_sls, i_srvc, i_stk, i_wip, i_pack, i_catalog;


                    //var transtype = Session["TransType"].ToString();
                    var transtype = _ItemGroupModel.TransType;

                    if (transtype == "Update" || transtype == "Edit")
                    {

                        //string GroupID = Session["GroupID"].ToString();
                         GroupID = _ItemGroupModel.GroupID;
                        string ItemGrpId = (_ItemGroupModel.item_grp_id).ToString();
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = int.Parse(Session["CompId"].ToString());
                        }
                        DataSet ds = _ItemGroup_ISERVICES.GetItemDetail(ItemGrpId, Comp_ID);

                        if (ds.Tables[0].Rows[0]["i_batch"].ToString() == "Y")
                            i_batch = true;
                        else
                            i_batch = false;
                        if (ds.Tables[0].Rows[0]["i_capg"].ToString() == "Y")
                            i_capg = true;
                        else
                            i_capg = false;
                        if (ds.Tables[0].Rows[0]["i_cons"].ToString() == "Y")
                            i_cons = true;
                        else
                            i_cons = false;
                        if (ds.Tables[0].Rows[0]["i_exp"].ToString() == "Y")
                            i_exp = true;
                        else
                            i_exp = false;
                        if (ds.Tables[0].Rows[0]["i_pur"].ToString() == "Y")
                            i_pur = true;
                        else
                            i_pur = false;
                        if (ds.Tables[0].Rows[0]["i_qc"].ToString() == "Y")
                            i_qc = true;
                        else
                            i_qc = false;
                        if (ds.Tables[0].Rows[0]["i_Sam"].ToString() == "Y")
                            i_Sam = true;
                        else
                            i_Sam = false;
                        if (ds.Tables[0].Rows[0]["i_serial"].ToString() == "Y")
                            i_serial = true;
                        else
                            i_serial = false;
                        if (ds.Tables[0].Rows[0]["i_sls"].ToString() == "Y")
                            i_sls = true;
                        else
                            i_sls = false;
                        if (ds.Tables[0].Rows[0]["i_srvc"].ToString() == "Y")
                            i_srvc = true;
                        else
                            i_srvc = false;
                        if (ds.Tables[0].Rows[0]["i_stk"].ToString() == "Y")
                            i_stk = true;
                        else
                            i_stk = false;
                        if (ds.Tables[0].Rows[0]["i_wip"].ToString() == "Y")
                            i_wip = true;
                        else
                            i_wip = false;
                        if (ds.Tables[0].Rows[0]["i_pack"].ToString() == "Y")
                            i_pack = true;
                        else
                            i_pack = false;
                        if (ds.Tables[0].Rows[0]["i_catalog"].ToString() == "Y")
                            i_catalog = true;
                        else
                            i_catalog = false;
                        _ItemGroupModel.item_grp_id = int.Parse(ds.Tables[0].Rows[0]["item_grp_id"].ToString());
                        _ItemGroupModel.item_group_name = ds.Tables[0].Rows[0]["item_group_name"].ToString();
                        _ItemGroupModel.item_grp_par_id = ds.Tables[0].Rows[0]["ItemParId"].ToString();
                        _ItemGroupModel.issue_method = ds.Tables[0].Rows[0]["issue_method"].ToString();
                        _ItemGroupModel.cost_method = ds.Tables[0].Rows[0]["cost_method"].ToString();
                        _ItemGroupModel.It_Remarks = ds.Tables[0].Rows[0]["It_Remarks"].ToString();

                        _ItemGroupModel.i_batch = i_batch;
                        _ItemGroupModel.i_capg = i_capg;
                        _ItemGroupModel.i_cons = i_cons;
                        _ItemGroupModel.i_exp = i_exp;
                        _ItemGroupModel.i_pur = i_pur;
                        _ItemGroupModel.i_qc = i_qc;
                        _ItemGroupModel.i_sam = i_Sam;
                        _ItemGroupModel.i_serial = i_serial;
                        _ItemGroupModel.i_sls = i_sls;
                        _ItemGroupModel.i_srvc = i_srvc;
                        _ItemGroupModel.i_stk = i_stk;
                        _ItemGroupModel.i_wip = i_wip;
                        _ItemGroupModel.i_pack = i_pack;
                        _ItemGroupModel.i_catalog = i_catalog;

                        _ItemGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        _ItemGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                        _ItemGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        _ItemGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();

                        _ItemGroupModel.loc_sls_coa = int.Parse(ds.Tables[0].Rows[0]["loc_sls_coa"].ToString());
                        _ItemGroupModel.sal_ret_coa = int.Parse(ds.Tables[0].Rows[0]["sal_ret_coa"].ToString());
                        _ItemGroupModel.exp_sls_coa = int.Parse(ds.Tables[0].Rows[0]["exp_sls_coa"].ToString());
                        _ItemGroupModel.loc_pur_coa = int.Parse(ds.Tables[0].Rows[0]["loc_pur_coa"].ToString());
                        _ItemGroupModel.imp_pur_coa = int.Parse(ds.Tables[0].Rows[0]["imp_pur_coa"].ToString());
                        _ItemGroupModel.stk_coa = int.Parse(ds.Tables[0].Rows[0]["stk_coa"].ToString());
                        _ItemGroupModel.disc_coa = int.Parse(ds.Tables[0].Rows[0]["Disc_coa"].ToString());
                        _ItemGroupModel.pur_ret_coa = int.Parse(ds.Tables[0].Rows[0]["pur_ret_coa"].ToString());
                        //_ItemGroupModel.prov_pay_coa = int.Parse(ds.Tables[0].Rows[0]["prov_pay_coa"].ToString());
                        //_ItemGroupModel.cogs_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_coa"].ToString());
                        //_ItemGroupModel.stk_adj_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_adj_coa"].ToString());
                        _ItemGroupModel.dep_coa = int.Parse(ds.Tables[0].Rows[0]["dep_coa"].ToString());
                        _ItemGroupModel.asset_coa = int.Parse(ds.Tables[0].Rows[0]["asset_coa"].ToString());
                        _ItemGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                        _ItemGroupModel.InterBranch_sls_coa = int.Parse(ds.Tables[0].Rows[0]["interbr_sls_coa"].ToString());
                        _ItemGroupModel.InterBranch_pur_coa = int.Parse((ds.Tables[0].Rows[0]["interbr_pur_coa"].ToString()));
                    }
                    else
                    {
                        _ItemGroupModel.i_sls = true;
                        _ItemGroupModel.i_pur = true;
                        _ItemGroupModel.act_stat = "Y";
                    }
                    //_ItemGroupModel.TransType = Session["TransType"].ToString();
                    //Session["ChkPgroup"] = null;
                    //_ItemGroupModel.ChkPgroup = null;
                    if (_ItemGroupModel.BtnName == null)
                    {
                        _ItemGroupModel.BtnName= "BtnAddNew";
                    }
                    //ViewBag.Message = Session["Message"].ToString();
                    //_ItemGroupModel.Message = _ItemGroupModel.Message;
                    return View("~/Areas/BusinessLayer/Views/ItemGroupSetup/ItemGroupSetup.cshtml", _ItemGroupModel);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("Error");
                throw Ex;
            }


        }
        public ActionResult ItemGroupSetupView(string ItemGrpId)
        {
            try
            {
                CommonPageDetails();
                _SearchItemBOL = new SearchItemBOL();
                _ItemGroupModel = new ItemGroupModel();
                //Session["TransType"] = "Save";
                //Session["Command"] = "Test";
                //Session["BtnName"] = "BtnToDetailPage";

                _ItemGroupModel.TransType = "Save";
                _ItemGroupModel.Command = "Test";
                _ItemGroupModel.BtnName = "BtnToDetailPage";

                int Comp_ID = 0;
                ////_ItemGroupModel.item_grp_id = Convert.ToInt32(Session["GroupID"].ToString());
                ViewBag.MenuPageName = getDocumentName();
                _ItemGroupModel.Title = title;

                if (Session["CompId"] != null)
                {
                    Comp_ID = int.Parse(Session["CompId"].ToString());
                }

                dt = GetGroupParentList();
                List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                foreach (DataRow dt in dt.Rows)
                {
                    AccountGroup _AccountGroupList = new AccountGroup();
                    _AccountGroupList.item_grp_struc = dt["item_grp_struc"].ToString();
                    _AccountGroupList.par_grp_name = dt["par_grp_name"].ToString();
                    _ItemAccountGroupList.Add(_AccountGroupList);
                }
                _ItemAccountGroupList.Insert(0, new AccountGroup() { item_grp_struc = "-1", par_grp_name = "---Select---" });
                _ItemGroupModel.ParentItemGroup = _ItemAccountGroupList;

                GetLocalSaleAccount(_ItemGroupModel);
                GetExportSaleAccount(_ItemGroupModel);
                GetLocalPurchaseAccount(_ItemGroupModel);
                GetImportPurchaseAccount(_ItemGroupModel);
                GetSaleReturnAccount(_ItemGroupModel);
                GetPurchaseReturnAccount(_ItemGroupModel);
                //GetProvisionalPayableAccount(_ItemGroupModel);
                GetStockAccount(_ItemGroupModel);
                //GetStockAdjustmentAccount(_ItemGroupModel);
                GetDepreciationAccount(_ItemGroupModel);
                GetDiscountAccount(_ItemGroupModel);
                //GetcostOfGoodsSoldAccount(_ItemGroupModel);
                GetAssetAccount(_ItemGroupModel);
                Boolean i_batch, i_capg, i_cons, i_exp, i_pur, i_qc, i_Sam, i_serial, i_sls, i_srvc, i_stk, i_wip,i_pack, i_catalog;

                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }
                    DataSet ds = _ItemGroup_ISERVICES.GetItemGroupDetail(ItemGrpId, Comp_ID);

                    if (ds.Tables[0].Rows[0]["i_batch"].ToString() == "Y")
                        i_batch = true;
                    else
                        i_batch = false;
                    if (ds.Tables[0].Rows[0]["i_capg"].ToString() == "Y")
                        i_capg = true;
                    else
                        i_capg = false;
                    if (ds.Tables[0].Rows[0]["i_cons"].ToString() == "Y")
                        i_cons = true;
                    else
                        i_cons = false;
                    if (ds.Tables[0].Rows[0]["i_exp"].ToString() == "Y")
                        i_exp = true;
                    else
                        i_exp = false;
                    if (ds.Tables[0].Rows[0]["i_pur"].ToString() == "Y")
                        i_pur = true;
                    else
                        i_pur = false;
                    if (ds.Tables[0].Rows[0]["i_qc"].ToString() == "Y")
                        i_qc = true;
                    else
                        i_qc = false;
                    if (ds.Tables[0].Rows[0]["i_Sam"].ToString() == "Y")
                        i_Sam = true;
                    else
                        i_Sam = false;
                    if (ds.Tables[0].Rows[0]["i_serial"].ToString() == "Y")
                        i_serial = true;
                    else
                        i_serial = false;
                    if (ds.Tables[0].Rows[0]["i_sls"].ToString() == "Y")
                        i_sls = true;
                    else
                        i_sls = false;
                    if (ds.Tables[0].Rows[0]["i_srvc"].ToString() == "Y")
                        i_srvc = true;
                    else
                        i_srvc = false;
                    if (ds.Tables[0].Rows[0]["i_stk"].ToString() == "Y")
                        i_stk = true;
                    else
                        i_stk = false;
                    if (ds.Tables[0].Rows[0]["i_wip"].ToString() == "Y")
                        i_wip = true;
                    else
                        i_wip = false;
                if (ds.Tables[0].Rows[0]["i_pack"].ToString() == "Y")
                    i_pack = true;
                else
                    i_pack = false;
                if (ds.Tables[0].Rows[0]["i_catalog"].ToString() == "Y")
                    i_catalog = true;
                else
                    i_catalog = false;

                _ItemGroupModel.item_grp_id = int.Parse(ds.Tables[0].Rows[0]["item_grp_id"].ToString());
                    _ItemGroupModel.item_group_name = ds.Tables[0].Rows[0]["item_group_name"].ToString();
                    _ItemGroupModel.item_grp_par_id = ds.Tables[0].Rows[0]["ItemParId"].ToString();
                    _ItemGroupModel.issue_method = ds.Tables[0].Rows[0]["issue_method"].ToString();
                    _ItemGroupModel.cost_method = ds.Tables[0].Rows[0]["cost_method"].ToString();
                    _ItemGroupModel.It_Remarks = ds.Tables[0].Rows[0]["It_Remarks"].ToString();

                    _ItemGroupModel.i_batch = i_batch;
                    _ItemGroupModel.i_capg = i_capg;
                    _ItemGroupModel.i_cons = i_cons;
                    _ItemGroupModel.i_exp = i_exp;
                    _ItemGroupModel.i_pur = i_pur;
                    _ItemGroupModel.i_qc = i_qc;
                    _ItemGroupModel.i_sam = i_Sam;
                    _ItemGroupModel.i_serial = i_serial;
                    _ItemGroupModel.i_sls = i_sls;
                    _ItemGroupModel.i_srvc = i_srvc;
                    _ItemGroupModel.i_stk = i_stk;
                    _ItemGroupModel.i_wip = i_wip;
                    _ItemGroupModel.i_pack = i_pack;
                _ItemGroupModel.i_catalog = i_catalog;

                _ItemGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    _ItemGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                    _ItemGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    _ItemGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();

                    _ItemGroupModel.loc_sls_coa = int.Parse(ds.Tables[0].Rows[0]["loc_sls_coa"].ToString());
                    _ItemGroupModel.sal_ret_coa = int.Parse(ds.Tables[0].Rows[0]["sal_ret_coa"].ToString());
                    _ItemGroupModel.exp_sls_coa = int.Parse(ds.Tables[0].Rows[0]["exp_sls_coa"].ToString());
                    _ItemGroupModel.loc_pur_coa = int.Parse(ds.Tables[0].Rows[0]["loc_pur_coa"].ToString());
                    _ItemGroupModel.imp_pur_coa = int.Parse(ds.Tables[0].Rows[0]["imp_pur_coa"].ToString());
                    _ItemGroupModel.stk_coa = int.Parse(ds.Tables[0].Rows[0]["stk_coa"].ToString());
                    _ItemGroupModel.disc_coa = int.Parse(ds.Tables[0].Rows[0]["Disc_coa"].ToString());
                    _ItemGroupModel.pur_ret_coa = int.Parse(ds.Tables[0].Rows[0]["pur_ret_coa"].ToString());
                    //_ItemGroupModel.prov_pay_coa = int.Parse(ds.Tables[0].Rows[0]["prov_pay_coa"].ToString());
                    //_ItemGroupModel.cogs_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_coa"].ToString());
                    //_ItemGroupModel.stk_adj_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_adj_coa"].ToString());
                    _ItemGroupModel.dep_coa = int.Parse(ds.Tables[0].Rows[0]["dep_coa"].ToString());
                    _ItemGroupModel.asset_coa = int.Parse(ds.Tables[0].Rows[0]["asset_coa"].ToString());
                    _ItemGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                _ItemGroupModel.InterBranch_sls_coa = int.Parse(ds.Tables[0].Rows[0]["interbr_sls_coa"].ToString());
                _ItemGroupModel.InterBranch_pur_coa = int.Parse((ds.Tables[0].Rows[0]["interbr_pur_coa"].ToString()));


                //_ItemGroupModel.TransType = Session["TransType"].ToString();
                //_ItemGroupModel.TransType = _ItemGroupModel.TransType;
                return View("~/Areas/BusinessLayer/Views/ItemGroupSetup/ItemGroupSetup.cshtml", _ItemGroupModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("Error");
                throw Ex;
            }
        }
        private string getDocumentName()
        {

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
            string[] Docpart = DocumentName.Split('>');
            int len = Docpart.Length;
            if (len > 1)
            {
                title = Docpart[len - 1].Trim();
            }
            return DocumentName;
        }
        [HttpPost]
       [ValidateAntiForgeryToken]

        public ActionResult ItemGroupSave(ItemGroupModel _ItemGroupModel, string command, string item_grp_id)
        {
            try
            {
                if (_ItemGroupModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "Edit":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["GroupID"] = _ItemGroupModel.item_grp_id;
                        //_ItemGroupModel.FormMode = "1";
                        //Session["TransType"] = "Update";
                        //Session["BtnName"] = "BtnEdit";

                        _ItemGroupModel.onetimeclick = null;
                        _ItemGroupModel.Message = "";
                        _ItemGroupModel.Command = command;
                        _ItemGroupModel.GroupID = _ItemGroupModel.item_grp_id.ToString();
                        _ItemGroupModel.FormMode = "1";
                        _ItemGroupModel.TransType = "Update";
                        _ItemGroupModel.BtnName = "BtnEdit";
                        var GroupID= _ItemGroupModel.item_grp_id.ToString();
                        var TransType = "Update";
                        var BtnName= "BtnEdit";
                        string status= Check_GroupDependency(GroupID);
                        if (status == "Y")
                        {
                            //Session["ChkPgroup"] = "Y";
                            _ItemGroupModel.ChkPgroup = "Y";
                        }
                        else
                        {
                            //Session["ChkPgroup"] =null;
                            _ItemGroupModel.ChkPgroup = null;
                        }
                        var ChkPgroup = _ItemGroupModel.ChkPgroup;
                        TempData["ModelData"] = _ItemGroupModel;
                        return( RedirectToAction("ItemGroupSetupAdd", "ItemGroupSetup", new { GroupID = GroupID, TransType, BtnName, command, ChkPgroup }));

                    case "Add":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["GroupID"] = 0;
                        ////Session["AppStatus"] = "D";
                        //_ItemGroupModel = null;
                        //Session["TransType"] = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        _ItemGroupModel.onetimeclick = null;
                        _ItemGroupModel.Message = "";
                        _ItemGroupModel.Command = command;
                        _ItemGroupModel.GroupID = "0";
                        ////_ItemGroupModel.AppStatus = "D";
                        //_ItemGroupModel = null;
                        _ItemGroupModel.TransType = "Save";
                        _ItemGroupModel.BtnName = "BtnAddNew";
                        _ItemGroupModel.item_grp_par_id = null;
                        TempData["ModelData"] = _ItemGroupModel;
                        return RedirectToAction("ItemGroupSetupAdd", "ItemGroupSetup");


                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Delete";
                        _ItemGroupModel.onetimeclick = null;
                        _ItemGroupModel.Command = command;
                        _ItemGroupModel.BtnName = "Delete";
                        item_grp_id = Convert.ToInt32(_ItemGroupModel.item_grp_id).ToString();
                         DeleteItemGroup(_ItemGroupModel);
                        TempData["ModelData"] = _ItemGroupModel;
                        return RedirectToAction("ItemGroupSetup");

                    case "Save":
                        //Session["Command"] = command;
                        _ItemGroupModel.Command = command;
                        if (ModelState.IsValid)
                        {
                            InsertItemGroupDetail(_ItemGroupModel);
                            //Session["GroupID"] = Session["GroupID"].ToString();
                            //_ItemGroupModel.GroupID = _ItemGroupModel.GroupID;

                            //if (Session["Message"].ToString() == "Duplicate")
                            CommonPageDetails();
                            if (_ItemGroupModel.Message == "Duplicate")
                            {                               
                                dt = GetGroupParentList();
                                _ItemGroupModel.onetimeclick = null;
                                List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AccountGroup _AccountGroupList = new AccountGroup();
                                    _AccountGroupList.item_grp_struc = dt["item_grp_struc"].ToString();
                                    _AccountGroupList.par_grp_name = dt["par_grp_name"].ToString();
                                    _ItemAccountGroupList.Add(_AccountGroupList);
                                }
                                _ItemAccountGroupList.Insert(0, new AccountGroup() { item_grp_struc = "-1", par_grp_name = "---Select---" });
                                _ItemGroupModel.ParentItemGroup = _ItemAccountGroupList;

                                GetLocalSaleAccount(_ItemGroupModel);
                                GetExportSaleAccount(_ItemGroupModel);
                                GetLocalPurchaseAccount(_ItemGroupModel);
                                GetImportPurchaseAccount(_ItemGroupModel);
                                GetSaleReturnAccount(_ItemGroupModel);
                                GetPurchaseReturnAccount(_ItemGroupModel);
                                GetDepreciationAccount(_ItemGroupModel);
                                GetDiscountAccount(_ItemGroupModel);
                                GetAssetAccount(_ItemGroupModel);

                                _ItemGroupModel.GroupID = "";
                                _ItemGroupModel.AppStatus = "D";
                                _ItemGroupModel.TransType = "Save";
                                _ItemGroupModel.BtnName = "BtnAddNew";
                                _ItemGroupModel.Command = "Add";
                                return View("~/Areas/BusinessLayer/Views/ItemGroupSetup/ItemGroupSetup.cshtml", _ItemGroupModel);
                            }
                            if (_ItemGroupModel.Message == "ValidationForSameName")
                            {

                                dt = GetGroupParentList();
                                _ItemGroupModel.onetimeclick = null;
                                List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AccountGroup _AccountGroupList = new AccountGroup();
                                    _AccountGroupList.item_grp_struc = dt["item_grp_struc"].ToString();
                                    _AccountGroupList.par_grp_name = dt["par_grp_name"].ToString();
                                    _ItemAccountGroupList.Add(_AccountGroupList);
                                }
                                _ItemAccountGroupList.Insert(0, new AccountGroup() { item_grp_struc = "-1", par_grp_name = "---Select---" });
                                _ItemGroupModel.ParentItemGroup = _ItemAccountGroupList;

                                GetLocalSaleAccount(_ItemGroupModel);
                                GetExportSaleAccount(_ItemGroupModel);
                                GetLocalPurchaseAccount(_ItemGroupModel);
                                GetImportPurchaseAccount(_ItemGroupModel);
                                GetSaleReturnAccount(_ItemGroupModel);
                                GetPurchaseReturnAccount(_ItemGroupModel);
                                GetDepreciationAccount(_ItemGroupModel);
                                GetDiscountAccount(_ItemGroupModel);
                                GetAssetAccount(_ItemGroupModel);

                                _ItemGroupModel.GroupID = "";
                                _ItemGroupModel.AppStatus = "D";
                                _ItemGroupModel.TransType = "Save";
                                _ItemGroupModel.BtnName = "BtnAddNew";
                                _ItemGroupModel.Command = "Add";
                                return View("~/Areas/BusinessLayer/Views/ItemGroupSetup/ItemGroupSetup.cshtml", _ItemGroupModel);
                            }
                            else
                                GroupID = _ItemGroupModel.GroupID;
                            TransType = _ItemGroupModel.TransType;
                            BtnName = _ItemGroupModel.BtnName;
                            TempData["ModelData"] = _ItemGroupModel;
                                return( RedirectToAction("ItemGroupSetupAdd", new { GroupID = GroupID, TransType, BtnName, command }));
                        }
                        else
                        {
                            _ItemGroupModel = null;
                            return View("~/Areas/BusinessLayer/Views/ItemGroupSetup/ItemGroupSetup.cshtml", _ItemGroupModel);
                        }

                    case "Forward":
                        return new EmptyResult();
                    case "Approve":
                        //Session["Command"] = command;
                        _ItemGroupModel.Command = command;
                        item_grp_id = Convert.ToInt32(_ItemGroupModel.item_grp_id).ToString();
                        //Session["GroupID"] = item_grp_id;
                        _ItemGroupModel.GroupID = item_grp_id;
                        // ItemApprove(_ItemGroupModel, command);
                        TempData["ModelData"] = _ItemGroupModel;
                        return RedirectToAction("ItemGroupSetupAdd");


                    case "Refresh":
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Refresh";                   
                        //Session["Message"] = "";
                        //Session["AppStatus"] = "";
                        //_ItemGroupModel = null;
                        _ItemGroupModel.onetimeclick = null;
                        _ItemGroupModel.BtnName = "BtnRefresh";
                        _ItemGroupModel.Command = command;
                        _ItemGroupModel.TransType = "Refresh";
                        _ItemGroupModel.Message = "";
                        _ItemGroupModel.AppStatus = "";
                        TempData["ModelData"] = _ItemGroupModel;
                        return RedirectToAction("ItemGroupSetup");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        return RedirectToAction("Index", "DashboardHome", new { area = "Dashboard" });

                    default:
                        return new EmptyResult();

                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public JsonResult GetAllItemGrp(ItemMenuSearchModel ObjItemMenuSearchModel)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    ObjItemMenuSearchModel.Comp_ID = Session["CompId"].ToString();
                }
                JsonResult DataRows = null;
                string FinalData = string.Empty;
                Newtonsoft.Json.Linq.JObject FData = new Newtonsoft.Json.Linq.JObject();
                FData = _ItemGroup_ISERVICES.GetAllItemGrpBl(ObjItemMenuSearchModel);
                DataRows = Json(JsonConvert.SerializeObject(FData), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                //this.LogError(Ex);
                ////return Json("ErrorPage"); 
                //throw Ex;
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return View("Error");
                throw Ex;
            }
        }
        public JsonResult GetItemDetail(string ItemGrpId)
        {
            try
            {
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    if (Session["CompId"] != null)
                    {
                        Comp_Id = int.Parse(Session["CompId"].ToString());
                    }
                    DataSet result = _ItemGroup_ISERVICES.GetItemDetail(ItemGrpId, Comp_Id);
                    DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                }
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                throw Ex;
            }
        }
        public JsonResult GetItemGroupDetail(string ItemGrpId)
        {
            try
            {
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    if (Session["CompId"] != null)
                    {
                        Comp_Id = int.Parse(Session["CompId"].ToString());
                    }
                    DataSet result = _ItemGroup_ISERVICES.GetItemGroupDetail(ItemGrpId, Comp_Id);
                    DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                }
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                throw Ex;
            }
        }
        //public JsonResult GetDefaultItemDetail()
        //{
        //    try
        //    {
        //        JsonResult DataRows = null;
        //        if (ModelState.IsValid)
        //        {
        //            if (Session["Comp_Id"] != null)
        //            {
        //                Comp_Id = int.Parse(Session["CompId"].ToString());
        //            }
        //            DataSet result = _ItemGroup_ISERVICES.GetDefaultItemDetail(Comp_Id);
        //            DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

        //        }
        //        return DataRows;
        //    }
        //    catch (Exception Ex)
        //    {
        //        this.LogError(Ex);
        //        return Json("ErrorPage");
        //        throw Ex;
        //    }
        //}
        public ActionResult GetLocalSaleAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetLocalSaleAccount(AccName, Comp_ID);

                List<IncomeCOA> _COAList = new List<IncomeCOA>();
                foreach (var data in COAList)
                {
                    IncomeCOA _COADetail = new IncomeCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.IncomeCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExportSaleAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetLocalSaleAccount(AccName, Comp_ID);
                List<IncomeCOA> _COAList = new List<IncomeCOA>();
                foreach (var data in COAList)
                {
                    IncomeCOA _COADetail = new IncomeCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.IncomeCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLocalPurchaseAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetImportPurchaseAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStockAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetStockAccount(AccName, Comp_ID);
                List<AssetsCOA> _COAList = new List<AssetsCOA>();
                foreach (var data in COAList)
                {
                    AssetsCOA _COADetail = new AssetsCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.AssetsCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSaleReturnAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetLocalSaleAccount(AccName, Comp_ID);
                List<IncomeCOA> _COAList = new List<IncomeCOA>();
                foreach (var data in COAList)
                {
                    IncomeCOA _COADetail = new IncomeCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.IncomeCOAList = _COAList;
                
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDiscountAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPurchaseReturnAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);
                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetProvisionalPayableAccount(ItemGroupModel _ItemGroupDetail)
        //{
        //    string Comp_ID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }
        //    string AccName = string.Empty;
        //    //string ErrorMessage = "success";
        //    Dictionary<string, string> COAList = new Dictionary<string, string>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
        //        {
        //            AccName = "0";
        //        }
        //        else
        //        {
        //            AccName = _ItemGroupDetail.ddlcoa_name;
        //        }
        //        COAList = _ItemGroup_ISERVICES.GetProvisionalPayableAccount(AccName, Comp_ID);

        //        List<LiabilityCOA> _COAList = new List<LiabilityCOA>();
        //        foreach (var data in COAList)
        //        {
        //            LiabilityCOA _COADetail = new LiabilityCOA();
        //            _COADetail.coa_id = data.Key;
        //            _COADetail.coa_name = data.Value;
        //            _COAList.Add(_COADetail);
        //        }
        //        _ItemGroupDetail.LiabilityCOAList = _COAList;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetcostOfGoodsSoldAccount(ItemGroupModel _ItemGroupDetail)
        //{
        //    string Comp_ID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }
        //    string AccName = string.Empty;
        //    //string ErrorMessage = "success";
        //    Dictionary<string, string> COAList = new Dictionary<string, string>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
        //        {
        //            AccName = "0";
        //        }
        //        else
        //        {
        //            AccName = _ItemGroupDetail.ddlcoa_name;
        //        }
        //        COAList = _ItemGroup_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

        //        List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
        //        foreach (var data in COAList)
        //        {
        //            ExpenseCOA _COADetail = new ExpenseCOA();
        //            _COADetail.coa_id = data.Key;
        //            _COADetail.coa_name = data.Value;
        //            _COAList.Add(_COADetail);
        //        }
        //        _ItemGroupDetail.ExpenseCOAList = _COAList;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetStockAdjustmentAccount(ItemGroupModel _ItemGroupDetail)
        //{
        //    string Comp_ID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }
        //    string AccName = string.Empty;
        //    //string ErrorMessage = "success";
        //    Dictionary<string, string> COAList = new Dictionary<string, string>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
        //        {
        //            AccName = "0";
        //        }
        //        else
        //        {
        //            AccName = _ItemGroupDetail.ddlcoa_name;
        //        }
        //        COAList = _ItemGroup_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

        //        List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
        //        foreach (var data in COAList)
        //        {
        //            ExpenseCOA _COADetail = new ExpenseCOA();
        //            _COADetail.coa_id = data.Key;
        //            _COADetail.coa_name = data.Value;
        //            _COAList.Add(_COADetail);
        //        }
        //        _ItemGroupDetail.ExpenseCOAList = _COAList;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetDepreciationAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAssetAccount(ItemGroupModel _ItemGroupDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.ddlcoa_name;
                }
                COAList = _ItemGroup_ISERVICES.GetStockAccount(AccName, Comp_ID);
                List<AssetsCOA> _COAList = new List<AssetsCOA>();
                foreach (var data in COAList)
                {
                    AssetsCOA _COADetail = new AssetsCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemGroupDetail.AssetsCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertItemGroupDetail(ItemGroupModel ObjAddItemGroupSetupBOL)
        {
            try
            {
               
                //if(Session["TransType"].ToString() == "Update")
                if(ObjAddItemGroupSetupBOL.TransType == "Update")
                {
                    ObjAddItemGroupSetupBOL.FormMode = "1";
                }
                
                if (Session["CompId"] != null)
                {
                    ObjAddItemGroupSetupBOL.comp_id = int.Parse(Session["CompId"].ToString());
                    ObjAddItemGroupSetupBOL.create_id = int.Parse(Session["UserId"].ToString());
                    if (ObjAddItemGroupSetupBOL.FormMode == "1")
                    {
                        ObjAddItemGroupSetupBOL.mod_id = int.Parse(Session["UserId"].ToString());
                    }
                }
                string ParGrpID;
                if (ObjAddItemGroupSetupBOL.item_grp_par_id=="-1")
                {
                     ParGrpID = "0";
                }
                else
                {
                     ParGrpID = ObjAddItemGroupSetupBOL.item_grp_par_id;
                }
                string GrpID;
                if (ParGrpID == "0")
                {
                    GrpID = "0";
                }
                else
                {
                     GrpID = ParGrpID.Substring(ParGrpID.Length - 5, 5);
                }         
                
                if (ObjAddItemGroupSetupBOL.item_grp_id.ToString() == "0" || ObjAddItemGroupSetupBOL.item_grp_id.ToString() != GrpID)                   
                {
                    string status = _ItemGroup_ISERVICES.InsertitemGroupDetail(ObjAddItemGroupSetupBOL);

                    string GroupID = status.Substring(status.IndexOf('-') + 1);

                    string Message = status.Substring(0, status.IndexOf("-"));

                    if (Message == "Update" || Message == "Save")
                    {

                        //Session["Message"] = "Save";
                        //Session["GroupID"] = GroupID;
                        //Session["TransType"] = "Update";
                        //ViewBag.Message = Session["Message"].ToString();

                        ObjAddItemGroupSetupBOL.Message = "Save";
                        ObjAddItemGroupSetupBOL.GroupID = GroupID;
                        ObjAddItemGroupSetupBOL.TransType = "Update";
                        //ViewBag.Message = ObjAddItemGroupSetupBOL.Message.ToString();
                    }
                    if (Message == "Duplicate")
                    {
                        //Session["TransType"] = "Duplicate";
                        //Session["Message"] = "Duplicate";
                        //Session["GroupID"] = GroupID;
                        //ViewBag.Message = Session["Message"].ToString();

                        ObjAddItemGroupSetupBOL.TransType = "Duplicate";
                        ObjAddItemGroupSetupBOL.Message = "Duplicate";
                        ObjAddItemGroupSetupBOL.GroupID = GroupID;
                        //ViewBag.Message = ObjAddItemGroupSetupBOL.Message.ToString();
                    }
                    //Session["BtnName"] = "BtnSave";
                    ObjAddItemGroupSetupBOL.BtnName = "BtnSave";
                    TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                    return RedirectToAction("ItemGroupSetupAdd", "ItemGroupSetup");
                }
                if (ObjAddItemGroupSetupBOL.item_grp_id.ToString() == GrpID)
                {
                    //Session["Message"] = "ValidationForSameName";
                    //ViewBag.Message = Session["Message"].ToString();

                    ObjAddItemGroupSetupBOL.Message = "ValidationForSameName";
                    //ViewBag.Message = ObjAddItemGroupSetupBOL.Message.ToString();
                }
                TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                return RedirectToAction("ItemGroupSetup", "ItemGroupSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                throw Ex;
            }
        }
        public ActionResult DeleteItemGroup(ItemGroupModel _ItemGroupDetail)
        {
            try
            {
                JsonResult Result = Json("");
                //ViewBag.PageHeader = PageHeader;
                int comp_id = 0;
                if (Session["CompId"] != null)
                {
                    comp_id = int.Parse(Session["CompId"].ToString());
                }
                int item_grp_id = int.Parse(_ItemGroupDetail.item_grp_id.ToString());
                string status = _ItemGroup_ISERVICES.DeleteItemGroup(item_grp_id, comp_id);
                if (status == "Deleted")
                {
                    //Session["Message"] = "Deleted";
                    //Session["Command"] = "Delete";
                    //Session["GroupID"] = "";
                    //_ItemGroupDetail = null;
                    //Session["TransType"] = "Refresh";
                    //Session["BtnName"] = "Delete";
                    //ViewBag.Message = Session["Message"].ToString();

                    _ItemGroupDetail.Message = "Deleted";
                    _ItemGroupDetail.Command = "Delete";
                    _ItemGroupDetail.GroupID = "";
                    //_ItemGroupDetail = null;
                    _ItemGroupDetail.TransType = "Refresh";
                    _ItemGroupDetail.BtnName = "Delete";
                    //ViewBag.Message = _ItemGroupDetail.Message.ToString();
                    return RedirectToAction("ItemGroupSetup", "ItemGroupSetup");
                }
                else
                {
                    //Session["GroupID"] = item_grp_id;
                    //Session["Message"] = "Dependency";
                    //Session["Command"] = "Delete";
                    //ViewBag.Message = Session["Message"].ToString();
                    CommonPageDetails();
                    _ItemGroupDetail.GroupID = item_grp_id.ToString();
                    _ItemGroupDetail.Message = "Dependency";
                    _ItemGroupDetail.Command = "Delete";
                    //ViewBag.Message = _ItemGroupDetail.Message.ToString();
                    _ItemGroupDetail.DeleteCommand = null;
                    return View("~/Areas/BusinessLayer/Views/ItemGroupSetup/ItemGroupSetup.cshtml", _ItemGroupDetail);
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                throw Ex;
            }
        }
        public string Check_GroupDependency(string GroupID)
        {
            try
            {
                //_ItemGroupModel = new ItemGroupModel();
                //JsonResult Result = Json("");
                int comp_id = 0;
                int item_grp_id = 0;
                if (Session["CompId"] != null)
                {
                    comp_id = int.Parse(Session["CompId"].ToString());
                }
                if(GroupID!="")
                {
                    item_grp_id = int.Parse(GroupID);
                }
                
                string status = _ItemGroup_ISERVICES.ChkPGroupDependency(item_grp_id, comp_id);
                //if(status=="Y")
                //{
                //    //Session["ChkPgroup"] = "Y";
                //    _ItemGroupModel.ChkPgroup = "Y";
                //}
                //else
                //{
                //    //Session["ChkPgroup"] =null;
                //    _ItemGroupModel.ChkPgroup =null;
                //}
                ////return Json(status);
                return status;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                throw Ex;
            }
        }

        public JsonResult GetSelectedParentDetail(string item_grp_struc)
        {
            try
            {
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    if (Session["CompId"] != null)
                    {
                        Comp_Id = int.Parse(Session["CompId"].ToString());
                    }
                    DataSet result = _ItemGroup_ISERVICES.GetSelectedParentDetail(item_grp_struc, Comp_Id);
                    DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                }
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                throw Ex;
            }
        }

        //public void LogError(Exception ex)
        //{
        //    try
        //    {
        //        string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        message += Environment.NewLine;
        //        message += "-----------------------------------------------------------";
        //        message += Environment.NewLine;
        //        message += string.Format("Message: {0}", ex.Message);
        //        message += Environment.NewLine;
        //        message += string.Format("StackTrace: {0}", ex.StackTrace);
        //        message += Environment.NewLine;
        //        message += string.Format("Source: {0}", ex.Source);
        //        message += Environment.NewLine;
        //        message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
        //        message += Environment.NewLine;
        //        message += "-----------------------------------------------------------";
        //        message += Environment.NewLine;
        //        string path = Server.MapPath("~/Log/ErrorLog.txt");
        //        using (StreamWriter writer = new StreamWriter(path, true))
        //        {
        //            writer.WriteLine(message);
        //            writer.Close();
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        throw Ex;
        //    }
        //}
        public ActionResult ErrorPage()
        {
            try
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private DataTable GetGroupParentList()
        {
            try
            {
                int Comp_ID = 0;
                if (Session["CompId"] != null)
                {
                    Comp_ID = int.Parse(Session["CompId"].ToString());
                }
                DataTable dt = _ItemGroup_ISERVICES.GetItemGroupSetup(Comp_ID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
    }
}