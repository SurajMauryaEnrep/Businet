using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetGroup;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetGroup;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FixedAssetManagement.AssetGroup
{
    public class AssetGroupController : Controller
    {
        DataTable dt;
        string CompID, language = String.Empty;
        string DocumentMenuId = "105106100", title, UserID;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        AssetGroup_ISERVICES _AssetGroup_ISERVICES;

        public AssetGroupController(Common_IServices _Common_IServices, AssetGroup_ISERVICES _AssetGroup_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._AssetGroup_ISERVICES = _AssetGroup_ISERVICES;
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
                ViewBag.Pagetitle = title;
                ViewBag.MenuPageName = DocumentName;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

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

        private string getDocumentName()
        {
            try
            {
                CommonPageDetails();
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
                string[] Docpart = DocumentName.Split('>');
                int len = Docpart.Length;
                if (len > 1)
                {
                    title = Docpart[len - 1].Trim();
                }
                return DocumentName;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public ActionResult AssetGroup()
        {
            try
            {
                CommonPageDetails();
                var _AssetGroupModel = TempData["ModelData"] as AssetGroupModel;
                ViewBag.MenuPageName = getDocumentName();
                string GroupID = string.Empty;
                if (_AssetGroupModel != null)
                {
                    _AssetGroupModel.Title = title;
                    _AssetGroupModel.Asset_grp_id = null;
                    if (_AssetGroupModel.Command != null)
                    {
                        if (_AssetGroupModel.Command == "Delete")
                        {
                            _AssetGroupModel.TransType = "Refresh";
                        }
                        else
                        {
                            _AssetGroupModel.TransType = "Save";
                        }
                    }
                    else
                    {
                        _AssetGroupModel.TransType = "Save";
                    }
                    _AssetGroupModel.Command = "Test";
                    _AssetGroupModel.BtnName = "BtnToDetailPage";
                    CommonAssetGroupCode(_AssetGroupModel);
                    if (_AssetGroupModel.GroupID != null)
                    {
                        if (_AssetGroupModel.GroupID == "")
                        {
                            GroupID = "0";
                        }
                        else
                        {
                            GroupID = _AssetGroupModel.GroupID;
                        }
                    }
                    else
                    {
                        GroupID = "0";
                    }

                    DataSet ds = _AssetGroup_ISERVICES.GetAssetDetail(GroupID, CompID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _AssetGroupModel.Asset_grp_id = int.Parse(ds.Tables[0].Rows[0]["Asset_grp_id"].ToString());
                        _AssetGroupModel.GroupID = ds.Tables[0].Rows[0]["Asset_grp_id"].ToString();
                        _AssetGroupModel.Asset_group_name = ds.Tables[0].Rows[0]["Asset_group_name"].ToString();
                        _AssetGroupModel.Asset_grp_par_id = ds.Tables[0].Rows[0]["asset_grp_par_id"].ToString();
                        _AssetGroupModel.Dep_method = ds.Tables[0].Rows[0]["DepreciationMethod"].ToString();
                        _AssetGroupModel.Dep_method = ds.Tables[0].Rows[0]["Dep_method"].ToString();
                        _AssetGroupModel.Dep_per = ds.Tables[0].Rows[0]["Dep_per"].ToString();
                        _AssetGroupModel.Dep_freq = ds.Tables[0].Rows[0]["Dep_freq"].ToString();
                        _AssetGroupModel.Assetcat_coa = int.Parse(ds.Tables[0].Rows[0]["asset_cat"].ToString());
                        _AssetGroupModel.Dep_coa = int.Parse(ds.Tables[0].Rows[0]["Dep_coa"].ToString());
                        _AssetGroupModel.Asset_coa = int.Parse(ds.Tables[0].Rows[0]["Asset_coa"].ToString());
                        _AssetGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                        _AssetGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        _AssetGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                        _AssetGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        _AssetGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                        var transtype = _AssetGroupModel.TransType;
                        if (_AssetGroupModel.TransType == "Save")
                        {
                            _AssetGroupModel.Message = "New";
                        }
                        else
                        {
                            _AssetGroupModel.Message = _AssetGroupModel.Message;
                        }
                        if ((GroupID == "0" || GroupID == "") && _AssetGroupModel.Asset_grp_id == null)
                        {
                            _AssetGroupModel.BtnName = "BtnRefresh";
                        }
                        _AssetGroupModel.DeleteCommand = null;
                        TempData["ModelData"] = _AssetGroupModel;
                    }
                    else
                    {
                        _AssetGroupModel.Asset_grp_id = 0;
                        _AssetGroupModel.Asset_group_name = string.Empty;
                        _AssetGroupModel.Asset_grp_par_id = string.Empty;
                        _AssetGroupModel.Dep_method = string.Empty;
                        _AssetGroupModel.Dep_per = string.Empty;
                        _AssetGroupModel.Dep_freq = string.Empty;
                        _AssetGroupModel.Assetcat_coa = 0;
                        _AssetGroupModel.Dep_coa = 0;
                        _AssetGroupModel.Asset_coa = 0;
                        _AssetGroupModel.Delete_Dependcy = string.Empty;
                        _AssetGroupModel.CreatedBy = string.Empty;
                        _AssetGroupModel.CreatedOn = string.Empty;
                        _AssetGroupModel.AmmendedBy = string.Empty;
                        _AssetGroupModel.AmmendedOn = string.Empty;
                        _AssetGroupModel.Onetimeclick = null;
                        _AssetGroupModel.Message = "";
                        //_AssetGroupModel.Command = null;
                        _AssetGroupModel.GroupID = "0";
                        //if (_AssetGroupModel.Command != null)
                        //{
                        //    if (_AssetGroupModel.Command == "Delete")
                        //    {
                        //        //Session["TransType"] = "Refresh";
                        //        _AssetGroupModel.TransType = "Refresh";
                        //    }
                        //    else
                        //    {
                        //        _AssetGroupModel.TransType = "Save";
                        //    }
                        //}
                        //else
                        //{
                        //    _AssetGroupModel.TransType = "Save";
                        //}

                        _AssetGroupModel.TransType = "Refresh";
                        //_AssetGroupModel.BtnName = "BtnAddNew";
                        _AssetGroupModel.Command = "Test";
                        _AssetGroupModel.BtnName = "BtnRefresh";
                        _AssetGroupModel.Asset_grp_par_id = null;
                        TempData["ModelData"] = _AssetGroupModel;
                    }
                    
                }
                else
                {
                    _AssetGroupModel = new AssetGroupModel();
                    _AssetGroupModel.Title = title;
                    _AssetGroupModel.Asset_grp_id = null;
                    if (_AssetGroupModel.Command != null)
                    {
                        if (_AssetGroupModel.Command == "Delete")
                        {
                            _AssetGroupModel.TransType = "Refresh";
                        }
                        else
                        {
                            _AssetGroupModel.TransType = "Save";
                        }
                    }
                    else
                    {
                        _AssetGroupModel.TransType = "Save";
                    }
                    _AssetGroupModel.Command = "Test";
                    _AssetGroupModel.BtnName = "BtnToDetailPage";
                    CommonAssetGroupCode(_AssetGroupModel);
                    if (_AssetGroupModel.GroupID != null)
                    {
                        if (_AssetGroupModel.GroupID == "")
                        {
                            GroupID = "0";
                        }
                        else
                        {
                            GroupID = _AssetGroupModel.GroupID;
                        }
                    }
                    else
                    {
                        GroupID = "0";
                    }
                    DataSet ds = _AssetGroup_ISERVICES.GetAssetDetail(GroupID, CompID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _AssetGroupModel.Asset_grp_id = int.Parse(ds.Tables[0].Rows[0]["Asset_grp_id"].ToString());
                        _AssetGroupModel.GroupID = ds.Tables[0].Rows[0]["Asset_grp_id"].ToString();
                        _AssetGroupModel.Asset_group_name = ds.Tables[0].Rows[0]["Asset_group_name"].ToString();
                        _AssetGroupModel.Asset_grp_par_id = ds.Tables[0].Rows[0]["asset_grp_par_id"].ToString();
                        _AssetGroupModel.Dep_method = ds.Tables[0].Rows[0]["DepreciationMethod"].ToString();
                        _AssetGroupModel.Dep_method = ds.Tables[0].Rows[0]["Dep_method"].ToString();
                        _AssetGroupModel.Dep_per = ds.Tables[0].Rows[0]["Dep_per"].ToString();
                        _AssetGroupModel.Dep_freq = ds.Tables[0].Rows[0]["Dep_freq"].ToString();
                        _AssetGroupModel.Assetcat_coa = int.Parse(ds.Tables[0].Rows[0]["asset_cat"].ToString());
                        _AssetGroupModel.Dep_coa = int.Parse(ds.Tables[0].Rows[0]["Dep_coa"].ToString());
                        _AssetGroupModel.Asset_coa = int.Parse(ds.Tables[0].Rows[0]["Asset_coa"].ToString());
                        _AssetGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                        _AssetGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        _AssetGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                        _AssetGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        _AssetGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                    }
                    var transtype = _AssetGroupModel.TransType;
                    if (_AssetGroupModel.TransType == "Save")
                    {
                        _AssetGroupModel.Message = "New";
                    }
                    else
                    {
                        _AssetGroupModel.Message = _AssetGroupModel.Message;
                    }
                    if ((GroupID == "0" || GroupID == "") && _AssetGroupModel.Asset_grp_id == null)
                    {
                        _AssetGroupModel.BtnName = "BtnRefresh";
                    }
                }
                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("Error");
                throw Ex;
            }
        }

        private void CommonAssetGroupCode(AssetGroupModel _AssetGroupModel)
        {
            dt = GetGroupParentList();
            List<AccountGroup_AG> _ItemAccountGroupList = new List<AccountGroup_AG>();
            foreach (DataRow dt in dt.Rows)
            {
                AccountGroup_AG _AccountGroupList = new AccountGroup_AG();
                _AccountGroupList.item_grp_struc = dt["asset_grp_struc"].ToString();
                _AccountGroupList.par_grp_name = dt["par_grp_name"].ToString();
                _ItemAccountGroupList.Add(_AccountGroupList);
            }
            _ItemAccountGroupList.Insert(0, new AccountGroup_AG() { item_grp_struc = "-1", par_grp_name = "---Select---" });
            _AssetGroupModel.ParentItemGroup = _ItemAccountGroupList;
            GetDepreciationAccount(_AssetGroupModel);
            GetAssetAccount(_AssetGroupModel);
            GetAssetCategoryAccount(_AssetGroupModel);
        }
        public ActionResult AssetGroupAdd(string GroupID, string TransType, string BtnName, string command, string ChkPgroup)
        {
            try
            {
                CommonPageDetails();
                var _AssetGroupModel = TempData["ModelData"] as AssetGroupModel;
                if (_AssetGroupModel != null)
                {
                    _AssetGroupModel.Title = title;
                    _AssetGroupModel.Asset_grp_id = Convert.ToInt32(_AssetGroupModel.GroupID);
                    CommonAssetGroupCode(_AssetGroupModel);

                    var transtype = _AssetGroupModel.TransType;
                    if (transtype == "Edit" || transtype == "Update")
                    {
                        GroupID = _AssetGroupModel.GroupID;
                        string ItemGrpId = (_AssetGroupModel.Asset_grp_id).ToString();
                        DataSet ds = _AssetGroup_ISERVICES.GetAssetDetail(ItemGrpId, CompID);
                        if (ds.Tables.Count > 0)
                        {
                            _AssetGroupModel.Asset_grp_id = int.Parse(ds.Tables[0].Rows[0]["Asset_grp_id"].ToString());
                            _AssetGroupModel.GroupID = ds.Tables[0].Rows[0]["Asset_grp_id"].ToString();
                            _AssetGroupModel.Asset_group_name = ds.Tables[0].Rows[0]["Asset_group_name"].ToString();
                            _AssetGroupModel.Asset_grp_par_id = ds.Tables[0].Rows[0]["AssetParentId"].ToString();
                            _AssetGroupModel.Dep_method = ds.Tables[0].Rows[0]["Dep_method"].ToString();
                            _AssetGroupModel.Dep_per = ds.Tables[0].Rows[0]["Dep_per"].ToString();
                            _AssetGroupModel.Dep_freq = ds.Tables[0].Rows[0]["Dep_freq"].ToString();
                            _AssetGroupModel.Assetcat_coa = int.Parse(ds.Tables[0].Rows[0]["asset_cat"].ToString());
                            _AssetGroupModel.Dep_coa = int.Parse(ds.Tables[0].Rows[0]["Dep_coa"].ToString());
                            _AssetGroupModel.Asset_coa = int.Parse(ds.Tables[0].Rows[0]["Asset_coa"].ToString());
                            _AssetGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                            _AssetGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            _AssetGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            _AssetGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            _AssetGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                        }
                    }
                    else if (transtype == "Save")
                    {
                        _AssetGroupModel.Asset_grp_id = 0;
                        _AssetGroupModel.Asset_group_name = string.Empty;
                        _AssetGroupModel.Asset_grp_par_id = string.Empty;
                        _AssetGroupModel.Dep_method = string.Empty;
                        _AssetGroupModel.Dep_per = string.Empty;
                        _AssetGroupModel.Dep_freq = string.Empty;
                        _AssetGroupModel.Assetcat_coa = 0;
                        _AssetGroupModel.Dep_coa = 0;
                        _AssetGroupModel.Asset_coa = 0;
                        _AssetGroupModel.Delete_Dependcy = string.Empty;
                        _AssetGroupModel.CreatedBy = string.Empty;
                        _AssetGroupModel.CreatedOn = string.Empty;
                        _AssetGroupModel.AmmendedBy = string.Empty;
                        _AssetGroupModel.AmmendedOn = string.Empty;
                        GroupID = string.Empty;
                        _AssetGroupModel.BtnName = "BtnAddNew";
                    }
                   
                    _AssetGroupModel.TransType = transtype;
                    _AssetGroupModel.Command = "Save";
                    //_AssetGroupModel.BtnName = "BtnAddNew";
                    return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel);
                }

                else
                {
                    AssetGroupModel _AssetGroupModel1 = new AssetGroupModel();
                    if (_AssetGroupModel1.GroupID == null)
                    {
                        _AssetGroupModel1.GroupID = GroupID;
                    }
                    if (_AssetGroupModel1.TransType == null)
                    {
                        _AssetGroupModel1.TransType = TransType;
                    }
                    if (_AssetGroupModel1.BtnName == null)
                    {
                        _AssetGroupModel1.BtnName = BtnName;
                    }
                    if (_AssetGroupModel1.Command == null)
                    {
                        _AssetGroupModel1.Command = command;
                    }
                    if (_AssetGroupModel1.ChkPgroup == null)
                    {
                        _AssetGroupModel1.ChkPgroup = ChkPgroup;
                    }
                    _AssetGroupModel1.Asset_grp_id = Convert.ToInt32(_AssetGroupModel1.GroupID);
                    ViewBag.MenuPageName = getDocumentName();
                    _AssetGroupModel1.Title = title;
                    var transtype = _AssetGroupModel1.TransType;
                    CommonAssetGroupCode(_AssetGroupModel1);
                    if (transtype == "Update" || transtype == "Edit")
                    {
                        GroupID = _AssetGroupModel1.GroupID;
                        string ItemGrpId = (_AssetGroupModel1.Asset_grp_id).ToString();
                        DataSet ds = _AssetGroup_ISERVICES.GetAssetDetail(ItemGrpId, CompID);
                        if (ds.Tables.Count > 0)
                        {
                            _AssetGroupModel1.Asset_grp_id = int.Parse(ds.Tables[0].Rows[0]["Asset_grp_id"].ToString());
                            _AssetGroupModel1.GroupID = ds.Tables[0].Rows[0]["Asset_grp_id"].ToString();
                            _AssetGroupModel1.Asset_group_name = ds.Tables[0].Rows[0]["Asset_group_name"].ToString();
                            _AssetGroupModel1.Asset_grp_par_id = ds.Tables[0].Rows[0]["AssetParentId"].ToString();
                            _AssetGroupModel1.Dep_method = ds.Tables[0].Rows[0]["Dep_method"].ToString();
                            _AssetGroupModel1.Dep_per = ds.Tables[0].Rows[0]["Dep_per"].ToString();
                            _AssetGroupModel1.Dep_freq = ds.Tables[0].Rows[0]["Dep_freq"].ToString();
                            _AssetGroupModel1.Assetcat_coa = int.Parse(ds.Tables[0].Rows[0]["asset_cat"].ToString());
                            _AssetGroupModel1.Dep_coa = int.Parse(ds.Tables[0].Rows[0]["Dep_coa"].ToString());
                            _AssetGroupModel1.Asset_coa = int.Parse(ds.Tables[0].Rows[0]["Asset_coa"].ToString());
                            _AssetGroupModel1.Delete_Dependcy = ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                            _AssetGroupModel1.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            _AssetGroupModel1.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            _AssetGroupModel1.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            _AssetGroupModel1.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                        }
                    }
                    if (_AssetGroupModel1.BtnName == null)
                    {
                        _AssetGroupModel1.BtnName = "BtnAddNew";
                    }
                    _AssetGroupModel1.TransType = "Update";
                    _AssetGroupModel1.Command = "Save";
                    //_AssetGroupModel1.BtnName = "BtnSave";
                    //_AssetGroupModel1.BtnName = "BtnAddNew";
                    return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel1);
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
                AssetGroupModel _AssetGroupModel = new AssetGroupModel();
                _AssetGroupModel.TransType = "Save";
                _AssetGroupModel.Command = "Test";
                _AssetGroupModel.BtnName = "BtnToDetailPage";
                ViewBag.MenuPageName = getDocumentName();
                _AssetGroupModel.Title = title;

                CommonAssetGroupCode(_AssetGroupModel);

                DataSet ds = _AssetGroup_ISERVICES.GetAssetDetail(ItemGrpId, CompID);
                if (ds.Tables.Count > 0)
                {
                    _AssetGroupModel.Asset_grp_id = int.Parse(ds.Tables[0].Rows[0]["Asset_grp_id"].ToString());
                    _AssetGroupModel.GroupID = ds.Tables[0].Rows[0]["Asset_grp_id"].ToString();
                    _AssetGroupModel.Asset_group_name = ds.Tables[0].Rows[0]["Asset_group_name"].ToString();
                    _AssetGroupModel.Asset_grp_par_id = ds.Tables[0].Rows[0]["AssetParentId"].ToString();
                    _AssetGroupModel.Dep_method = ds.Tables[0].Rows[0]["Dep_method"].ToString();
                    _AssetGroupModel.Dep_per = ds.Tables[0].Rows[0]["Dep_per"].ToString();
                    _AssetGroupModel.Dep_freq = ds.Tables[0].Rows[0]["Dep_freq"].ToString();
                    _AssetGroupModel.Assetcat_coa = int.Parse(ds.Tables[0].Rows[0]["asset_cat"].ToString());
                    _AssetGroupModel.Dep_coa = int.Parse(ds.Tables[0].Rows[0]["Dep_coa"].ToString());
                    _AssetGroupModel.Asset_coa = int.Parse(ds.Tables[0].Rows[0]["Asset_coa"].ToString());
                    _AssetGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Dependcy_delete"].ToString();
                    _AssetGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    _AssetGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                    _AssetGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    _AssetGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                }
                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("Error");
                throw Ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssetGroupSave(AssetGroupModel _AssetGroupModel, string command, string item_grp_id)
        {
            try
            {
                if (_AssetGroupModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                var GroupID = _AssetGroupModel.Asset_grp_id.ToString();
                var TransType = "";
                var BtnName = "";
                _AssetGroupModel.Title = ViewBag.Pagetitle;
                switch (command)
                {
                    case "Edit":
                        _AssetGroupModel.Onetimeclick = null;
                        _AssetGroupModel.Message = "";
                        _AssetGroupModel.Command = command;
                        _AssetGroupModel.GroupID = _AssetGroupModel.Asset_grp_id.ToString();
                        _AssetGroupModel.FormMode = "1";
                        _AssetGroupModel.TransType = "Update";
                        _AssetGroupModel.BtnName = "BtnEdit";
                        GroupID = _AssetGroupModel.Asset_grp_id.ToString();
                        TransType = "Update";
                        BtnName = "BtnEdit";
                        string status = Check_GroupDependency(GroupID);
                        if (status == "Y")
                        {
                            _AssetGroupModel.ChkPgroup = "Y";
                        }
                        else
                        {
                            _AssetGroupModel.ChkPgroup = null;
                        }
                        var ChkPgroup = _AssetGroupModel.ChkPgroup;
                        TempData["ModelData"] = _AssetGroupModel;
                        CommonAssetGroupCode(_AssetGroupModel);
                        //return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel);
                        return (RedirectToAction("AssetGroupAdd", new { GroupID = GroupID, TransType, BtnName, command }));
                    case "Add":
                        _AssetGroupModel.Onetimeclick = null;
                        _AssetGroupModel.Message = "";
                        _AssetGroupModel.Command = command;
                        _AssetGroupModel.GroupID = "0";
                        _AssetGroupModel.TransType = "Save";
                        _AssetGroupModel.BtnName = "BtnAddNew";
                        _AssetGroupModel.Asset_grp_par_id = null;
                        TempData["ModelData"] = _AssetGroupModel;
                        return RedirectToAction("AssetGroupAdd", "AssetGroup");

                    case "Delete":
                        _AssetGroupModel.Onetimeclick = null;
                        _AssetGroupModel.Command = command;
                        _AssetGroupModel.BtnName = "Delete";
                        item_grp_id = Convert.ToInt32(_AssetGroupModel.Asset_grp_id).ToString();
                        DeleteItemGroup(_AssetGroupModel);
                        TempData["ModelData"] = _AssetGroupModel;
                        return RedirectToAction("AssetGroup");

                    case "Save":
                        _AssetGroupModel.Command = command;
                        if (ModelState.IsValid)
                        {
                            InsertItemGroupDetail(_AssetGroupModel);
                            CommonPageDetails();
                            if (_AssetGroupModel.Message == "Duplicate")
                            {
                                _AssetGroupModel.Onetimeclick = null;
                                CommonAssetGroupCode(_AssetGroupModel);
                                _AssetGroupModel.GroupID = "";
                                _AssetGroupModel.AppStatus = "D";
                                //_AssetGroupModel.TransType = "Save";
                                //_AssetGroupModel.BtnName = "BtnAddNew";
                                //_AssetGroupModel.Command = "Add";
                                _AssetGroupModel.TransType = "Update";
                                _AssetGroupModel.BtnName = "BtnAddNew";
                                _AssetGroupModel.Command = "Edit";
                                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel);
                            }
                            if (_AssetGroupModel.Message == "ValidationForSameName")
                            {
                                _AssetGroupModel.Onetimeclick = null;
                                CommonAssetGroupCode(_AssetGroupModel);
                                _AssetGroupModel.GroupID = "";
                                _AssetGroupModel.AppStatus = "D";
                                _AssetGroupModel.TransType = "Save";
                                _AssetGroupModel.BtnName = "BtnAddNew";
                                _AssetGroupModel.Command = "Add";
                                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel);
                            }
                            else
                                GroupID = _AssetGroupModel.GroupID;
                            TransType = _AssetGroupModel.TransType;
                            BtnName = _AssetGroupModel.BtnName;
                            TempData["ModelData"] = _AssetGroupModel;

                            return (RedirectToAction("AssetGroupAdd", new { GroupID = GroupID, TransType, BtnName, command }));
                            // return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel);
                        }
                        else
                        {
                            _AssetGroupModel = null;
                            return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel);
                        }

                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        _AssetGroupModel.Command = command;
                        item_grp_id = Convert.ToInt32(_AssetGroupModel.Asset_grp_id).ToString();
                        _AssetGroupModel.GroupID = item_grp_id;
                        TempData["ModelData"] = _AssetGroupModel;
                        return RedirectToAction("AssetGroupAdd");

                    case "Refresh":
                        _AssetGroupModel.Asset_grp_id = 0;
                        _AssetGroupModel.Asset_group_name = string.Empty;
                        _AssetGroupModel.Asset_grp_par_id = string.Empty;
                        _AssetGroupModel.Dep_method = string.Empty;
                        _AssetGroupModel.Dep_per = string.Empty;
                        _AssetGroupModel.Dep_freq = string.Empty;
                        _AssetGroupModel.Assetcat_coa = 0;
                        _AssetGroupModel.Dep_coa = 0;
                        _AssetGroupModel.Asset_coa = 0;
                        _AssetGroupModel.Delete_Dependcy = string.Empty;
                        _AssetGroupModel.CreatedBy = string.Empty;
                        _AssetGroupModel.CreatedOn = string.Empty;
                        _AssetGroupModel.AmmendedBy = string.Empty;
                        _AssetGroupModel.AmmendedOn = string.Empty;
                        _AssetGroupModel.Onetimeclick = null;
                        _AssetGroupModel.BtnName = "BtnRefresh";
                        _AssetGroupModel.Command = command;
                        _AssetGroupModel.TransType = "Refresh";
                        _AssetGroupModel.Message = "";
                        _AssetGroupModel.AppStatus = "";
                        CommonAssetGroupCode(_AssetGroupModel);
                        TempData["ModelData"] = _AssetGroupModel;
                        GroupID = _AssetGroupModel.GroupID;
                        return RedirectToAction("AssetGroup");
                         //return (RedirectToAction("AssetGroupAdd", new { GroupID = GroupID, TransType, BtnName, command }));
                        //return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _AssetGroupModel);

                    case "Print":
                        return new EmptyResult();

                    case "BacktoList":
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

        public JsonResult GetAllItemGrp(ItemMenuSearchModel_AG ObjItemMenuSearchModel)
        {
            try
            {
                CommonPageDetails();
                ObjItemMenuSearchModel.Comp_ID = CompID;
                JsonResult DataRows = null;
                string FinalData = string.Empty;
                Newtonsoft.Json.Linq.JObject FData = new Newtonsoft.Json.Linq.JObject();
                FData = _AssetGroup_ISERVICES.GetAllItemGrpBl(ObjItemMenuSearchModel);
                DataRows = Json(JsonConvert.SerializeObject(FData), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public JsonResult GetItemDetail(string ItemGrpId)
        {
            try
            {
                CommonPageDetails();
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    DataSet result = _AssetGroup_ISERVICES.GetAssetDetail(ItemGrpId, CompID);
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
                    CommonPageDetails();
                    DataSet result = _AssetGroup_ISERVICES.GetAssetDetail(ItemGrpId, CompID);
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
        public ActionResult GetDepreciationAccount(AssetGroupModel _ItemGroupDetail)
        {
            CommonPageDetails();
            string AccName = string.Empty;
            Dictionary<string, string> COAList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.Ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.Ddlcoa_name;
                }
                COAList = _AssetGroup_ISERVICES.GetLocalPurchaseAccount(AccName, CompID);
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
        public ActionResult GetAssetAccount(AssetGroupModel _ItemGroupDetail)
        {
            CommonPageDetails();
            string AccName = string.Empty;
            Dictionary<string, string> COAList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.Ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.Ddlcoa_name;
                }
                COAList = _AssetGroup_ISERVICES.GetStockAccount(AccName, CompID);
                List<AssetsCOA_AG> _COAList = new List<AssetsCOA_AG>();
                foreach (var data in COAList)
                {
                    AssetsCOA_AG _COADetail = new AssetsCOA_AG();
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

        public ActionResult GetAssetCategoryAccount(AssetGroupModel _ItemGroupDetail)
        {
            CommonPageDetails();
            string AccName = string.Empty;
            Dictionary<string, string> COAList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(_ItemGroupDetail.Ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemGroupDetail.Ddlcoa_name;
                }
                COAList = _AssetGroup_ISERVICES.GetAssetCategory(CompID);
                List<AssetsCategoryCOA_AG> _COACList = new List<AssetsCategoryCOA_AG>();
                foreach (var data in COAList)
                {
                    AssetsCategoryCOA_AG _COADetail = new AssetsCategoryCOA_AG();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _ItemGroupDetail.AssetsCategoryCOAList = _COACList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertItemGroupDetail(AssetGroupModel ObjAddItemGroupSetupBOL)
        {
            try
            {
                CommonPageDetails();
                if (ObjAddItemGroupSetupBOL.TransType == "Update")
                {
                    ObjAddItemGroupSetupBOL.FormMode = "1";
                }
                if (Session["CompId"] != null)
                {
                    ObjAddItemGroupSetupBOL.Comp_id = int.Parse(CompID);
                    ObjAddItemGroupSetupBOL.Create_id = int.Parse(UserID);
                    if (ObjAddItemGroupSetupBOL.FormMode == "1")
                    {
                        ObjAddItemGroupSetupBOL.Mod_id = int.Parse(UserID);
                    }
                }
                string ParGrpID;
                if (ObjAddItemGroupSetupBOL.Asset_grp_par_id == "-1")
                {
                    ParGrpID = "0";
                }
                else
                {
                    ParGrpID = ObjAddItemGroupSetupBOL.Asset_grp_par_id;
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

                if (ObjAddItemGroupSetupBOL.Asset_grp_id.ToString() == "0" || ObjAddItemGroupSetupBOL.Asset_grp_id.ToString() != GrpID)
                {
                    string status = _AssetGroup_ISERVICES.InsertAssetGroupDetail(ObjAddItemGroupSetupBOL);
                    string GroupID = status.Substring(status.IndexOf('-') + 1);
                    string Message = status.Substring(0, status.IndexOf("-"));

                    if (Message == "Update" || Message == "Save")
                    {
                        ObjAddItemGroupSetupBOL.Message = "Save";
                        ObjAddItemGroupSetupBOL.GroupID = GroupID;
                        ObjAddItemGroupSetupBOL.TransType = "Update";
                    }
                    if (Message == "Duplicate")
                    {
                        ObjAddItemGroupSetupBOL.TransType = "Duplicate";
                        ObjAddItemGroupSetupBOL.Message = "Duplicate";
                        ObjAddItemGroupSetupBOL.GroupID = GroupID;
                    }
                    ObjAddItemGroupSetupBOL.BtnName = "BtnSave";
                    TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                    return RedirectToAction("AssetGroupAdd", "AssetGroup");
                }
                if (ObjAddItemGroupSetupBOL.Asset_grp_id.ToString() == GrpID)
                {
                    ObjAddItemGroupSetupBOL.Message = "ValidationForSameName";
                }
                TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                return RedirectToAction("AssetGroup", "AssetGroup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                throw Ex;
            }
        }
        public ActionResult DeleteItemGroup(AssetGroupModel _ItemGroupDetail)
        {
            try
            {
                JsonResult Result = Json("");
                CommonPageDetails();
                int item_grp_id = int.Parse(_ItemGroupDetail.Asset_grp_id.ToString());
                string status = _AssetGroup_ISERVICES.DeleteItemGroup(item_grp_id, CompID);
                if (status == "Deleted")
                {
                    _ItemGroupDetail.Message = "Deleted";
                    _ItemGroupDetail.Command = "Delete";
                    _ItemGroupDetail.GroupID = "";
                    _ItemGroupDetail.TransType = "Refresh";
                    _ItemGroupDetail.BtnName = "Delete";
                    return RedirectToAction("AssetGroup", "AssetGroup");
                }
                else
                {
                    _ItemGroupDetail.GroupID = item_grp_id.ToString();
                    _ItemGroupDetail.Message = "Dependency";
                    _ItemGroupDetail.Command = "Delete";
                    _ItemGroupDetail.DeleteCommand = null;
                    return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroup.cshtml", _ItemGroupDetail);
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
                int item_grp_id = 0;
                CommonPageDetails();
                if (GroupID != "")
                {
                    item_grp_id = int.Parse(GroupID);
                }
                string status = _AssetGroup_ISERVICES.ChkPGroupDependency(item_grp_id, CompID);
                return status;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }

        public string Check_ChildGroupDependency(string GroupID)
        {
            try
            {
                int item_grp_id = 0;
                CommonPageDetails();
                if (GroupID != "")
                {
                    item_grp_id = int.Parse(GroupID);
                }
                string status = _AssetGroup_ISERVICES.ChkChildGroupDependency(item_grp_id, CompID);
                return status;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public JsonResult GetSelectedParentDetail(string item_grp_struc)
        {
            try
            {
                JsonResult DataRows = null;
                CommonPageDetails();
                if (ModelState.IsValid)
                {
                    DataSet result = _AssetGroup_ISERVICES.GetSelectedParentDetail(item_grp_struc, CompID);
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
        private DataTable GetGroupParentList()
        {
            try
            {
                CommonPageDetails();
                DataTable dt = _AssetGroup_ISERVICES.GetAssetGroupSetup(CompID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }

        [HttpPost]
        public JsonResult UpdateAssetGroupHeaderAndDetail(string AssetGroupId)
        {
            try
            {
                CommonPageDetails();
                // Get data from DB
                DataSet ds = _AssetGroup_ISERVICES.GetAssetDetail(AssetGroupId, CompID);
                DataSet dsHeader = ds;
                DataSet dsDetails = ds;
                // Convert to model
                var HeaderModel = new AssetGroupModel();
                if (dsHeader != null && dsHeader.Tables.Count > 0 && dsHeader.Tables[0].Rows.Count > 0)
                {
                    var row = dsHeader.Tables[0].Rows[0];
                    HeaderModel = new AssetGroupModel
                    {
                        TransType = "Save",
                        Command = "Test",
                        BtnName = "BtnToDetailPage",
                        Title = title,
                        Asset_group_name = row["Asset_group_name"].ToString(),
                        Asset_grp_id = int.Parse(row["Asset_grp_id"].ToString()),
                        GroupID = row["Asset_grp_id"].ToString(),
                        Delete_Dependcy = row["Dependcy_delete"].ToString(),
                        CreatedBy = row["CreatedBy"].ToString(),
                        CreatedOn = row["CreatedOn"].ToString(),
                        AmmendedBy = row["ModifiedBy"].ToString(),
                        AmmendedOn = row["ModifiedOn"].ToString(),
                        Asset_grp_par_id = row["AssetParentId"].ToString(),
                        Dep_method = row["Dep_method"].ToString(),
                        Dep_per = row["Dep_per"].ToString(),
                        Dep_freq = row["Dep_freq"].ToString(),
                        Assetcat_coa = int.Parse(row["asset_cat"].ToString()),
                        Dep_coa = int.Parse(row["Dep_coa"].ToString()),
                        Asset_coa = int.Parse(row["Asset_coa"].ToString())
                    };
                }
                TempData["ModelData"] = HeaderModel;
                CommonAssetGroupCode(HeaderModel);
                // Render views
                string HeaderHtml = RenderPartialViewToString("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroupHeader.cshtml", HeaderModel);
                string DetailsHtml = RenderPartialViewToString("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroupDetails.cshtml", HeaderModel);

                return Json(new
                {
                    success = true,
                    HeaderHtmlResponse = HeaderHtml,
                    DetailsHtmlResponse = DetailsHtml
                });
            }
            catch (Exception ex)
            {
                Errorlog.LogError(Server.MapPath("~"), ex);
                return Json(new { success = false, message = "Error updating partials." });
            }
        }
        protected string RenderPartialViewToString(string viewPath, object model)
        {
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewPath);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        public JsonResult EditAssetGroupHeaderAndDetail(string AssetGroupId)
        {
            try
            {
                CommonPageDetails();
                // Get data from DB
                DataSet ds = _AssetGroup_ISERVICES.GetAssetDetail(AssetGroupId, CompID);
                DataSet dsHeader = ds;
                DataSet dsDetails = ds;
                // Convert to model
                var HeaderModel = new AssetGroupModel();
                if (dsHeader != null && dsHeader.Tables.Count > 0 && dsHeader.Tables[0].Rows.Count > 0)
                {
                    var row = dsHeader.Tables[0].Rows[0];
                    var GroupID = row["asset_grp_id"].ToString(); var ChkPgroup = "N"; var ChkChildGroup = "N";
                    string status = Check_GroupDependency(GroupID);
                    if (status == "Y")
                    {
                        ChkPgroup = "Y";
                    }
                    else
                    {
                        ChkPgroup = null;
                    }
                    string ChkChildGroupstatus = Check_GroupDependency(GroupID);
                    if (ChkChildGroupstatus == "Y")
                    {
                        ChkChildGroup = "Y";
                    }
                    else
                    {
                        ChkChildGroup = "N";
                    }
                    HeaderModel = new AssetGroupModel
                    {
                        TransType = "Update",
                        Command = "Edit",
                        BtnName = "BtnEdit",
                        Title = title,
                        Asset_grp_id = int.Parse(row["Asset_grp_id"].ToString()),
                        Asset_group_name = row["Asset_group_name"].ToString(),
                        GroupID = row["Asset_grp_id"].ToString(),
                        Delete_Dependcy = row["Dependcy_delete"].ToString(),
                        CreatedBy = row["CreatedBy"].ToString(),
                        CreatedOn = row["CreatedOn"].ToString(),
                        AmmendedBy = row["ModifiedBy"].ToString(),
                        AmmendedOn = row["ModifiedOn"].ToString(),
                        Asset_grp_par_id = row["AssetParentId"].ToString(),
                        Dep_method = row["Dep_method"].ToString(),
                        Dep_per = row["Dep_per"].ToString(),
                        Dep_freq = row["Dep_freq"].ToString(),
                        Assetcat_coa = int.Parse(row["asset_cat"].ToString()),
                        Dep_coa = int.Parse(row["Dep_coa"].ToString()),
                        Asset_coa = int.Parse(row["Asset_coa"].ToString()),
                        Onetimeclick = null,
                        Message = "",
                        ChkPgroup = ChkPgroup,
                        ChkChildGroup = ChkChildGroup
                    };
                }
                CommonAssetGroupCode(HeaderModel);
                // Render views
                string HeaderHtml = RenderPartialViewToString("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroupHeader.cshtml", HeaderModel);
                string DetailsHtml = RenderPartialViewToString("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetGroup/AssetGroupDetails.cshtml", HeaderModel);

                return Json(new
                {
                    success = true,
                    HeaderHtmlResponse = HeaderHtml,
                    DetailsHtmlResponse = DetailsHtml
                });
            }
            catch (Exception ex)
            {
                Errorlog.LogError(Server.MapPath("~"), ex);
                return Json(new { success = false, message = "Error updating partials." });
            }
        }
    }
}






