using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.QualityControlSetup.ItemParameterSetup
{
    public class ItemParameterController : Controller
    {
        string CompID, branchID, user_id, language = String.Empty, UserID = String.Empty;
        string DocumentMenuId = "103200105", title;
        Common_IServices _Common_IServices;
        List<getViewModelItemList> _ViewModelItemList;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        QCItemParameterSetup_ISERVICES _qcItemParameterSetup_ISERVICES;
        // GET: BusinessLayer/ItemParameterSetup
        public ItemParameterController(Common_IServices _Common_IServices, QCItemParameterSetup_ISERVICES qcItemParameterSetup_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._qcItemParameterSetup_ISERVICES = qcItemParameterSetup_ISERVICES;
        }

        public ActionResult ItemParameterList()
        {
            try
            {
                CommonPageDetails();
                QCItemParameterSetupViewModel _QCItemParameterSetupViewModel = new QCItemParameterSetupViewModel();
                //if (Session["BtnNameIPS"] == null)
                if (_QCItemParameterSetupViewModel.BtnNameIPS == null)
                {
                    //Session["BtnNameIPS"] = "BtnAddNew";
                    _QCItemParameterSetupViewModel.BtnNameIPS = "BtnAddNew";
                }
                //ViewBag.MenuPageName = getDocumentName();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, "0", DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                //QCItemParameterSetupViewModel _QCItemParameterSetupViewModel = new QCItemParameterSetupViewModel();
                GetAllData(_QCItemParameterSetupViewModel);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    if (PRData != null && PRData != "")
                    {
                        var a = PRData.Split(',');
                        var ItemID = a[0].Trim();
                        var ItemGrpID = a[1].Trim();
                        DataTable dt = _qcItemParameterSetup_ISERVICES.GetItemParamListFilterDAL(CompID, ItemID, ItemGrpID).Tables[0];
                        List<getViewModelItemList> modelList = new List<getViewModelItemList>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            getViewModelItemList _ParameterItemList = new getViewModelItemList();
                            _ParameterItemList.item_Name = dr["ItemName"].ToString();
                            _ParameterItemList.item_id = dr["ItemID"].ToString();
                            _ParameterItemList.group = dr["GroupName"].ToString();
                            _ParameterItemList.UOM = dr["UOM"].ToString();
                            _ParameterItemList.OEM_No = dr["OEM_no"].ToString();
                            _ParameterItemList.Sample_code = dr["SampleCode"].ToString();
                            _ParameterItemList.Status = dr["StatusName"].ToString();
                            _ParameterItemList.create_by = dr["create_by"].ToString();
                            _ParameterItemList.CreatedOn = dr["CreateOn"].ToString();
                            _ParameterItemList.app_by = dr["app_by"].ToString();
                            _ParameterItemList.ApprovedOn = dr["approvedOn"].ToString();
                            _ParameterItemList.mod_by = dr["mod_by"].ToString();
                            _ParameterItemList.AmendedOn = dr["amendedOn"].ToString();
                            modelList.Add(_ParameterItemList);
                        }
                        _QCItemParameterSetupViewModel.ListFilterData = TempData["ListFilterData"].ToString();
                        _QCItemParameterSetupViewModel.getViewModelItemList = modelList;
                        _QCItemParameterSetupViewModel.item_parList = ItemID;
                        _QCItemParameterSetupViewModel.item_Group = ItemGrpID;
                    }
                    //else
                    //{
                    //    _QCItemParameterSetupViewModel.getViewModelItemList = getITMListAll();
                    //}
                }
                //else
                //{
                //    _QCItemParameterSetupViewModel.getViewModelItemList = getITMListAll();
                //}
                _QCItemParameterSetupViewModel.Title = title;
                _QCItemParameterSetupViewModel.IPSearch = "0";
                return View("~/Areas/BusinessLayer/Views/QualityControlSetup/ItemParameterSetup/ItemParameterList.cshtml", _QCItemParameterSetupViewModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private void GetAllData(QCItemParameterSetupViewModel _QCItemParameterSetupViewModel)
        {
            string GroupName = string.Empty;
            string item_Group = string.Empty;
            string Comp_ID = string.Empty;
            string BrchID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (string.IsNullOrEmpty(_QCItemParameterSetupViewModel.item_parList))
            {
                GroupName = "0";
            }
            else
            {
                GroupName = _QCItemParameterSetupViewModel.item_parList;
            }
            if (string.IsNullOrEmpty(_QCItemParameterSetupViewModel.item_Group))
            {
                item_Group = "0";
            }
            else
            {
                item_Group = _QCItemParameterSetupViewModel.item_Group;
            }
            //  DataTable dt = _qcItemParameterSetup_ISERVICES.GetITMlist(GroupName, Comp_ID, BrchID);
            DataSet Data = _qcItemParameterSetup_ISERVICES.GetAllData(GroupName, Comp_ID, BrchID, item_Group);
            // DataTable dt2 = GetOCList(_QCItemParameterSetupViewModel);
            List<ItemParaList> _ITMList = new List<ItemParaList>();
            foreach (DataRow dr in Data.Tables[0].Rows)
            {
                ItemParaList ddlITMList = new ItemParaList();
                ddlITMList.Itm_id = dr["Item_id"].ToString();
                ddlITMList.Itm_name = dr["Item_name"].ToString();
                _ITMList.Add(ddlITMList);
            }
            _ITMList.Insert(0, new ItemParaList() { Itm_id = "0", Itm_name = "All" });
            _QCItemParameterSetupViewModel.getperaItemList = _ITMList;
            //  DataTable dt3 = GetGroupList(_QCItemParameterSetupViewModel);
            List<ItemGroupList> _ItemGroupList = new List<ItemGroupList>();
            foreach (DataRow dr in Data.Tables[1].Rows)
            {
                ItemGroupList ddlITMList = new ItemGroupList();
                ddlITMList.gr_id = dr["item_grp_id"].ToString();
                ddlITMList.gr_name = dr["ItemGroupChildNood"].ToString();
                _ItemGroupList.Add(ddlITMList);
            }
            _ItemGroupList.Insert(0, new ItemGroupList() { gr_id = "0", gr_name = "All" });
            _QCItemParameterSetupViewModel.getItemGroupList = _ItemGroupList;

            List<getViewModelItemList> _ViewModelItemList = new List<getViewModelItemList>();
            List<getViewModelItemList> modelList = new List<getViewModelItemList>();
            foreach (DataRow dr in Data.Tables[2].Rows)
            {
                getViewModelItemList _ParameterItemList = new getViewModelItemList();
                _ParameterItemList.item_Name = dr["ItemName"].ToString();
                _ParameterItemList.item_id = dr["ItemID"].ToString();
                //_ParameterItemList.param_Id = dr["param_Id"].ToString();
                _ParameterItemList.group = dr["GroupName"].ToString();
                _ParameterItemList.UOM = dr["UOM"].ToString();
                _ParameterItemList.OEM_No = dr["OEM_no"].ToString();
                _ParameterItemList.Sample_code = dr["SampleCode"].ToString();
                _ParameterItemList.Status = dr["StatusName"].ToString();
                _ParameterItemList.create_by = dr["create_by"].ToString();
                _ParameterItemList.CreatedOn = dr["CreateOn"].ToString();
                _ParameterItemList.app_by = dr["app_by"].ToString();
                _ParameterItemList.ApprovedOn = dr["approvedOn"].ToString();
                _ParameterItemList.mod_by = dr["mod_by"].ToString();
                _ParameterItemList.AmendedOn = dr["amendedOn"].ToString();

                _ViewModelItemList.Add(_ParameterItemList);

            }
            _QCItemParameterSetupViewModel.getViewModelItemList = _ViewModelItemList;
            // return _ViewModelItemList;
        }
        public ActionResult BindReplicateWithlist(QCItemParameterSetup _QCItemParameterSetup)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    if (_QCItemParameterSetup.item == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _QCItemParameterSetup.item;
                    }
                    DataSet ProductList = _qcItemParameterSetup_ISERVICES.getReplicateWith(CompID, SarchValue);
                    if (ProductList.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[0].Rows.Count; i++)
                        {
                            string ItemID = ProductList.Tables[0].Rows[i]["ItemID"].ToString();
                            string ItemName = ProductList.Tables[0].Rows[i]["ItemName"].ToString();
                            string UOM = ProductList.Tables[0].Rows[i]["UOM"].ToString();
                            ItemList.Add(ItemID, ItemName + ',' + UOM);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReplicateWithItemId(string ItemId)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataSet result = _qcItemParameterSetup_ISERVICES.GetReplicateWithItemdata(CompID, ItemId);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [NonAction]
        private DataTable GetOCList(QCItemParameterSetupViewModel _QCItemParameterSetupViewModel)
        {
            try
            {
                string GroupName = string.Empty;
                string Comp_ID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_QCItemParameterSetupViewModel.item_parList))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _QCItemParameterSetupViewModel.item_parList;
                }
                DataTable dt = _qcItemParameterSetup_ISERVICES.GetITMlist(GroupName, Comp_ID, BrchID);
                return dt;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable GetGroupList(QCItemParameterSetupViewModel _QCItemParameterSetupViewModel)
        {
            try
            {
                string GroupName = string.Empty;
                string Comp_ID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_QCItemParameterSetupViewModel.item_Group))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _QCItemParameterSetupViewModel.item_Group;
                }
                DataTable dt = _qcItemParameterSetup_ISERVICES.ItemGroupList(GroupName, Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        private List<getViewModelItemList> getITMListAll()
        {
            try
            {
                _ViewModelItemList = new List<getViewModelItemList>();
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet dt = _qcItemParameterSetup_ISERVICES.GetItemParameterSetupList(Convert.ToInt32(Comp_ID)); //GetItemListFromParameter();

                //_QCItemParameter.Title = title;
                List<getViewModelItemList> modelList = new List<getViewModelItemList>();
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    getViewModelItemList _ParameterItemList = new getViewModelItemList();
                    _ParameterItemList.item_Name = dr["ItemName"].ToString();
                    _ParameterItemList.item_id = dr["ItemID"].ToString();
                    //_ParameterItemList.param_Id = dr["param_Id"].ToString();
                    _ParameterItemList.group = dr["GroupName"].ToString();
                    _ParameterItemList.UOM = dr["UOM"].ToString();
                    _ParameterItemList.OEM_No = dr["OEM_no"].ToString();
                    _ParameterItemList.Sample_code = dr["SampleCode"].ToString();
                    _ParameterItemList.Status = dr["StatusName"].ToString();
                    _ParameterItemList.create_by = dr["create_by"].ToString();
                    _ParameterItemList.CreatedOn = dr["CreateOn"].ToString();
                    _ParameterItemList.app_by = dr["app_by"].ToString();
                    _ParameterItemList.ApprovedOn = dr["approvedOn"].ToString();
                    _ParameterItemList.mod_by = dr["mod_by"].ToString();
                    _ParameterItemList.AmendedOn = dr["amendedOn"].ToString();

                    _ViewModelItemList.Add(_ParameterItemList);

                }
                return _ViewModelItemList;
            }
            catch (Exception Ex)

            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult ItemParameterDetailListTbl(string itemID, string ListFilterData)
        {

            QCItemParameterSetup dblclick = new QCItemParameterSetup();
            var Comp_ID = Session["CompId"].ToString();
            dblclick.ItemId = itemID;
            dblclick.Action = "Update";
            dblclick.Commandfc = "Update";
            //Session["ItemId"] = itemID;
            //Session["Action"] = "Update";
            //Session["Commandfc"] = "Update";
            TempData["ModelData"] = dblclick;
            var ItemId = dblclick.ItemId;
            var Actions = dblclick.Action;
            var Commandfc = dblclick.Commandfc;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("ItemParameterDetail", "ItemParameter", new { ItemId, Actions, Commandfc });
        }
        public ActionResult ItemParameterDetailsNew()
        {


            var Comp_ID = Session["CompId"].ToString();
            //// Session["ItemId"] = itemID;
            //Session["BtnNameIPS"] = "BtnAddNew";
            //Session["Action"] = "AddNew";
            //Session["Commandfc"] = "";
            QCItemParameterSetup AddNew_Model = new QCItemParameterSetup();
            AddNew_Model.Action = "AddNew";
            AddNew_Model.Commandfc = "";
            AddNew_Model.BtnNameIPS = "BtnAddNew";
            TempData["ModelData"] = AddNew_Model;
            var Actions = AddNew_Model.Action;
            var Commandfc = AddNew_Model.Commandfc;
            var BtnNameIPS = AddNew_Model.BtnNameIPS;
            return RedirectToAction("ItemParameterDetail", "ItemParameter", new { Actions, Commandfc, BtnNameIPS });
        }
        public ActionResult ItemParameterDetail(string ItemId, string Actions, string Action, string Commandfc, string BtnNameIPS, string BtnEditName)
        {
            try
            {
                CommonPageDetails();
                var _QCsetup1 = TempData["ModelData"] as QCItemParameterSetup;
                if (_QCsetup1 != null)
                {
                    //QCItemParameterSetup _QCsetup1 = new QCItemParameterSetup();
                    CompID = Session["CompId"].ToString();
                    if (Actions != null)
                    {
                        _QCsetup1.Action = Actions;
                    }
                    else
                    {
                        _QCsetup1.Action = Action;
                    }

                    _QCsetup1.ItemId = ItemId;
                    _QCsetup1.Commandfc = Commandfc;
                    _QCsetup1.BtnNameIPS = BtnNameIPS;
                    _QCsetup1.BtnEditName = BtnEditName;
                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, "0", DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.VBRoleList = GetRoleList();

                    List<ItemNameList> modelList = new List<ItemNameList>();

                    ItemNameList _ItemDetail = new ItemNameList();
                    _ItemDetail.item_id = "0";
                    _ItemDetail.item_name = "---Select---";
                    modelList.Add(_ItemDetail);

                    if (_QCsetup1.MessageIPS != null)
                    {
                        ViewBag.Message = _QCsetup1.MessageIPS;
                        _QCsetup1.MessageIPS = null;
                    }
                    _QCsetup1.SerialNumber = 1;
                    BindReplicateWithlist(_QCsetup1);
                    ViewBag.BtnSaveCommand = "Save";
                    if (_QCsetup1.Action != null && _QCsetup1.ItemId != null)
                    {
                        ViewBag.BtnSaveCommand = "Update";
                        //Session["BtnNameIPS"] = "BtnEdit";
                        _QCsetup1.BtnNameIPS = "BtnEdit";
                        //QCItemParameterSetup QCsetupModel = new QCItemParameterSetup();
                        if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                        {
                            _QCsetup1.ListFilterData1 = TempData["ListFilterData"].ToString();
                        }
                        //string Itm_no = Session["ItemId"].ToString();
                        string Itm_no = _QCsetup1.ItemId;
                        DataTable value = _qcItemParameterSetup_ISERVICES.GetItemSetupListOnVIew(Convert.ToInt32(CompID), Itm_no);

                        ViewBag.DocumentCode = value.Rows[0]["status"].ToString().Trim();

                        _QCsetup1.ParameterNameList = ParameterDefinitionValue("0");
                        _QCsetup1.UomList = GetUomList();

                        ViewBag.datatbl = value;

                        _QCsetup1.comp_id = value.Rows[0]["comp_id"].ToString();
                        _QCsetup1.item_id = value.Rows[0]["ItemID"].ToString();
                        _QCsetup1.item_Name = value.Rows[0]["item_name"].ToString();
                        _QCsetup1.UOMName = value.Rows[0]["UOM"].ToString();
                        _QCsetup1.RefNo = value.Rows[0]["RefNo"].ToString();
                        _QCsetup1.OEMNo = value.Rows[0]["OemNo"].ToString();
                        _QCsetup1.SampleCode = value.Rows[0]["SampleCode"].ToString();
                        _QCsetup1.create_name = value.Rows[0]["CreatedBy"].ToString();
                        _QCsetup1.create_dt = value.Rows[0]["CreateOn"].ToString();
                        _QCsetup1.mod_name = value.Rows[0]["ModName"].ToString();
                        _QCsetup1.mod_dt = value.Rows[0]["AmendedOn"].ToString();
                        _QCsetup1.app_name = value.Rows[0]["ApproveName"].ToString();
                        _QCsetup1.app_dt = value.Rows[0]["approvedOn"].ToString();
                        _QCsetup1.status = value.Rows[0]["StatusName"].ToString();
                        _QCsetup1.CheckDependcy = value.Rows[0]["CheckDependcy"].ToString();

                        ItemNameList _ItemDetail1 = new ItemNameList();
                        _ItemDetail1.item_id = _QCsetup1.item_id;
                        _ItemDetail1.item_name = _QCsetup1.item_Name;
                        modelList.Add(_ItemDetail1);
                        _QCsetup1.ItemList = modelList;

                        //ViewBag.MenuPageName = getDocumentName();
                        _QCsetup1.Title = title;
                        return View("~/Areas/BusinessLayer/Views/QualityControlSetup/ItemParameterSetup/ItemParameterDetail.cshtml", _QCsetup1);

                    }
                    else
                    {
                        _QCsetup1.SerialNumber = 1;
                        // QCItemParameterSetup _QCItemParameterSetup = new QCItemParameterSetup();
                        if (TempData["ListFilterData"] != null)
                        {
                            _QCsetup1.ListFilterData1 = TempData["ListFilterData"].ToString();
                        }
                        _QCsetup1.ItemList = modelList;
                        _QCsetup1.ParameterNameList = ParameterDefinitionValue("0");
                        _QCsetup1.UomList = GetUomList();

                        if (Session["UserName"] != null)
                        {
                            UserID = Session["UserName"].ToString();
                        }

                        //ViewBag.MenuPageName = getDocumentName();
                        _QCsetup1.Title = title;
                        return View("~/Areas/BusinessLayer/Views/QualityControlSetup/ItemParameterSetup/ItemParameterDetail.cshtml", _QCsetup1);

                    }
                }
                else
                {
                    QCItemParameterSetup QCsetup = new QCItemParameterSetup();
                    CompID = Session["CompId"].ToString();
                    if (Actions != null)
                    {
                        QCsetup.Action = Actions;
                    }
                    else
                    {
                        QCsetup.Action = Action;
                    }
                    QCsetup.SerialNumber = 1;
                    QCsetup.ItemId = ItemId;
                    QCsetup.Commandfc = Commandfc;
                    QCsetup.BtnNameIPS = BtnNameIPS;
                    QCsetup.BtnEditName = BtnEditName;
                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, "0", DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.VBRoleList = GetRoleList();

                    List<ItemNameList> modelList = new List<ItemNameList>();

                    ItemNameList _ItemDetail = new ItemNameList();
                    _ItemDetail.item_id = "0";
                    _ItemDetail.item_name = "---Select---";
                    modelList.Add(_ItemDetail);
                    if (QCsetup.MessageIPS != null)
                    {
                        ViewBag.Message = QCsetup.MessageIPS;
                        QCsetup.MessageIPS = null;

                    }
                    BindReplicateWithlist(QCsetup);
                    ViewBag.BtnSaveCommand = "Save";
                    if (QCsetup.Action != null && QCsetup.ItemId != null)
                    {
                        ViewBag.BtnSaveCommand = "Update";
                        //Session["BtnNameIPS"] = "BtnEdit";
                        QCsetup.BtnNameIPS = "BtnEdit";
                        //QCItemParameterSetup QCsetupModel = new QCItemParameterSetup();
                        if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                        {
                            QCsetup.ListFilterData1 = TempData["ListFilterData"].ToString();
                        }
                        //string Itm_no = Session["ItemId"].ToString();
                        string Itm_no = QCsetup.ItemId;
                        DataTable value = _qcItemParameterSetup_ISERVICES.GetItemSetupListOnVIew(Convert.ToInt32(CompID), Itm_no);

                        ViewBag.DocumentCode = value.Rows[0]["status"].ToString().Trim();

                        QCsetup.ParameterNameList = ParameterDefinitionValue("0");
                        QCsetup.UomList = GetUomList();

                        ViewBag.datatbl = value;

                        QCsetup.comp_id = value.Rows[0]["comp_id"].ToString();
                        QCsetup.item_id = value.Rows[0]["ItemID"].ToString();
                        QCsetup.item_Name = value.Rows[0]["item_name"].ToString();
                        QCsetup.UOMName = value.Rows[0]["UOM"].ToString();
                        QCsetup.RefNo = value.Rows[0]["RefNo"].ToString();
                        QCsetup.OEMNo = value.Rows[0]["OemNo"].ToString();
                        QCsetup.SampleCode = value.Rows[0]["SampleCode"].ToString();
                        QCsetup.create_name = value.Rows[0]["CreatedBy"].ToString();
                        QCsetup.create_dt = value.Rows[0]["CreateOn"].ToString();
                        QCsetup.mod_name = value.Rows[0]["ModName"].ToString();
                        QCsetup.mod_dt = value.Rows[0]["AmendedOn"].ToString();
                        QCsetup.app_name = value.Rows[0]["ApproveName"].ToString();
                        QCsetup.app_dt = value.Rows[0]["approvedOn"].ToString();
                        QCsetup.status = value.Rows[0]["StatusName"].ToString();
                        QCsetup.CheckDependcy = value.Rows[0]["CheckDependcy"].ToString();
                        ItemNameList _ItemDetail1 = new ItemNameList();
                        _ItemDetail1.item_id = QCsetup.item_id;
                        _ItemDetail1.item_name = QCsetup.item_Name;
                        modelList.Add(_ItemDetail1);
                        QCsetup.ItemList = modelList;

                        //ViewBag.MenuPageName = getDocumentName();
                        QCsetup.Title = title;
                        return View("~/Areas/BusinessLayer/Views/QualityControlSetup/ItemParameterSetup/ItemParameterDetail.cshtml", QCsetup);

                    }
                    else
                    {
                        // QCItemParameterSetup _QCItemParameterSetup = new QCItemParameterSetup();
                        if (TempData["ListFilterData"] != null)
                        {
                            QCsetup.ListFilterData1 = TempData["ListFilterData"].ToString();
                        }
                        QCsetup.ItemList = modelList;
                        QCsetup.ParameterNameList = ParameterDefinitionValue("0");
                        QCsetup.UomList = GetUomList();

                        if (Session["UserName"] != null)
                        {
                            UserID = Session["UserName"].ToString();
                        }

                        //ViewBag.MenuPageName = getDocumentName();
                        QCsetup.Title = title;
                        return View("~/Areas/BusinessLayer/Views/QualityControlSetup/ItemParameterSetup/ItemParameterDetail.cshtml", QCsetup);

                    }
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void CommonPageDetails()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    user_id = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, branchID, user_id, DocumentMenuId, language);
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
        public List<ParameterNameList> ParameterDefinitionValue(string Parmid)
        {

            try
            {
                DataTable dt = GetItemList(Parmid);
                QCParameterDefinition _QCParameterDefinition = new QCParameterDefinition();
                List<ParameterNameList> modelList = new List<ParameterNameList>();
                ParameterNameList _ParameterList1 = new ParameterNameList();
                if (Parmid == "0")
                {
                    _ParameterList1.param_Id = "0";
                    _ParameterList1.param_name = "---Select---";
                    modelList.Add(_ParameterList1);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    ParameterNameList _ParameterList = new ParameterNameList();
                    _ParameterList.param_Id = dr["param_Id"].ToString();
                    if (Parmid == "0")
                    {
                        _ParameterList.param_name = dr["param_name"].ToString();
                    }
                    else
                    {
                        _ParameterList.param_name = dr["param_type"].ToString();
                    }

                    modelList.Add(_ParameterList);
                }
                return modelList;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }

        public ActionResult ParameterDefinitionType(string Parmid)
        {
            JsonResult Validate = Json("Please fill all mandatory field");
            try
            {
                DataTable dt = GetItemList(Parmid);

                //DataSet value = _qcItemParameterSetup_ISERVICES.GetItemSetupToDelete(itemId, Comp_ID);
                Validate = Json(JsonConvert.SerializeObject(dt.Rows[0]["param_type"].ToString()), JsonRequestBehavior.AllowGet);
                return Validate;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }


        }
        [NonAction]
        private DataTable GetItemList(string Parmid)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _qcItemParameterSetup_ISERVICES.GetItemParaMList(Comp_ID, Parmid);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult QCParameterSetupSave(QCItemParameterSetup qCItem, string Command)
        {
            try
            {
                var Actions = "";
                var BtnNameIPS = "";
                var Commandfc = "";
                var BtnEditName = "";
                var ItemId = "";
                switch (Command)
                {
                    case "AddNew":
                        //Session["Action"] = "AddNew";
                        ////Session["Action"] = null;
                        //Session["ItemId"] = null;
                        //Session["BtnNameIPS"] = "BtnAddNew";
                        QCItemParameterSetup AddNewModel = new QCItemParameterSetup();
                        AddNewModel.hdnSavebtn = null;
                        AddNewModel.Action = "AddNew";
                        AddNewModel.BtnNameIPS = "BtnAddNew";
                        TempData["ModelData"] = AddNewModel;
                        Actions = AddNewModel.Action;
                        BtnNameIPS = AddNewModel.BtnNameIPS;
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("ItemParameterDetail", new { Actions, BtnNameIPS });
                    case "Edit":
                        //Session["BtnEditName"] = "Update";
                        //Session["Commandfc"] = "Edit";
                        qCItem.hdnSavebtn = null;
                        qCItem.BtnEditName = "Update";
                        qCItem.Commandfc = "Edit";
                        qCItem.Action = "Edit";
                        qCItem.BtnNameIPS = "BtnAddNew";
                        qCItem.ItemId = qCItem.item_id;
                        Actions = "Edit";
                        TempData["ModelData"] = qCItem;
                        ItemId = qCItem.ItemId;
                        BtnEditName = qCItem.BtnEditName;
                        Commandfc = qCItem.Commandfc;
                        BtnNameIPS = qCItem.BtnNameIPS;
                        TempData["ListFilterData"] = qCItem.ListFilterData1;
                        return RedirectToAction("ItemParameterDetail", new { BtnEditName, Commandfc, BtnNameIPS, ItemId });
                    case "Save":
                        QCParameterSetupDetailSave(qCItem, qCItem.hdnItemList, "Create");
                        TempData["ModelData"] = qCItem;
                        ItemId = qCItem.ItemId;
                        Actions = qCItem.Action;
                        //Session["Commandfc"] = "Update";
                        qCItem.Commandfc = "Update";
                        Commandfc = qCItem.Commandfc;
                        TempData["ListFilterData"] = qCItem.ListFilterData1;
                        return RedirectToAction("ItemParameterDetail", new { ItemId, Actions, Commandfc });
                    case "Update":
                        QCParameterSetupDetailSave(qCItem, qCItem.hdnItemList, "Update");
                        // Session["Commandfc"] = "Update";
                        TempData["ModelData"] = qCItem;
                        qCItem.Commandfc = "Update";
                        Commandfc = qCItem.Commandfc;
                        ItemId = qCItem.item_id;
                        Actions = qCItem.Action;
                        qCItem.hdnSavebtn = null;
                        TempData["ListFilterData"] = qCItem.ListFilterData1;
                        return RedirectToAction("ItemParameterDetail", new { ItemId, Actions, Commandfc });
                    case "Delete":
                        QCParameterDelete(qCItem, qCItem.item_id);

                        //Session["ItemId"] = null;
                        //Session["BtnNameIPS"] = "BtnRefresh";
                        QCItemParameterSetup Delete_Model = new QCItemParameterSetup();
                        Delete_Model.BtnNameIPS = "BtnRefresh";
                        Delete_Model.MessageIPS = "Delete";
                        qCItem.hdnSavebtn = null;
                        TempData["ModelData"] = Delete_Model;
                        BtnNameIPS = Delete_Model.BtnNameIPS;
                        TempData["ListFilterData"] = qCItem.ListFilterData1;
                        return RedirectToAction("ItemParameterDetail", new { BtnNameIPS });
                    case "Approve":
                        QCParameterApprove(qCItem, qCItem.item_id, "Approve", "");
                        TempData["ModelData"] = qCItem;
                        ItemId = qCItem.item_id;
                        qCItem.hdnSavebtn = null;
                        Actions = qCItem.Action;
                        TempData["ListFilterData"] = qCItem.ListFilterData1;
                        return RedirectToAction("ItemParameterDetail", new { ItemId, Actions, Commandfc });
                    case "Refresh":
                        QCItemParameterSetup Refresh_Model = new QCItemParameterSetup();
                        Refresh_Model.BtnNameIPS = "BtnRefresh";
                        Refresh_Model.hdnSavebtn = null;
                        TempData["ModelData"] = Refresh_Model;
                        BtnNameIPS = Refresh_Model.BtnNameIPS;
                        //Session["BtnNameIPS"] = "BtnRefresh";
                        //Session["Action"] = null;
                        //Session["ItemId"] = null;
                        TempData["ListFilterData"] = qCItem.ListFilterData1;
                        return RedirectToAction("ItemParameterDetail", new { BtnNameIPS });
                    case "BacktoList":
                        //Session["Action"] = null;
                        //Session["ItemId"] = null;
                        //Session["BtnNameIPS"] = "BtnAddNew";
                        QCItemParameterSetup Back_List = new QCItemParameterSetup();
                        Back_List.BtnNameIPS = "BtnAddNew";
                        TempData["ListFilterData"] = qCItem.ListFilterData1;
                        return RedirectToAction("ItemParameterList");
                }
                TempData["ListFilterData "] = qCItem.ListFilterData1;
                return RedirectToAction("ItemParameterDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult QCParameterSetupDetailSave(QCItemParameterSetup qCItem, string Items, string TransType)
        {
            try
            {
                string Message = string.Empty;
                string Comp_ID = string.Empty;
                string UserID = string.Empty;

                if (Comp_ID == "" || Comp_ID == null)
                {
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["UserId"] != null)
                    {
                        UserID = Session["UserId"].ToString();
                    }
                    string SystemDetail = string.Empty;

                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();

                    DataTable QCItemDetail = new DataTable();

                    DataTable dtParaDetail = new DataTable();
                    dtParaDetail.Columns.Add("TransType", typeof(string));
                    dtParaDetail.Columns.Add("comp_id", typeof(Int32));
                    dtParaDetail.Columns.Add("User_id", typeof(Int32));
                    dtParaDetail.Columns.Add("SystemInfo", typeof(string));
                    dtParaDetail.Columns.Add("item_id", typeof(string));
                    dtParaDetail.Columns.Add("param_Id", typeof(Int32));
                    dtParaDetail.Columns.Add("upper_val", typeof(string));
                    dtParaDetail.Columns.Add("lower_val", typeof(string));
                    dtParaDetail.Columns.Add("uom_id", typeof(Int32));
                    dtParaDetail.Columns.Add("remarks", typeof(string));
                    dtParaDetail.Columns.Add("sr_no", typeof(int));

                    JArray jObject = JArray.Parse(Items);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtParaDetail.NewRow();

                        dtrowLines["TransType"] = TransType;
                        dtrowLines["comp_id"] = Comp_ID;
                        dtrowLines["User_id"] = UserID;
                        dtrowLines["SystemInfo"] = SystemDetail;
                        dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                        dtrowLines["param_Id"] = jObject[i]["param_Id"].ToString();
                        dtrowLines["upper_val"] = IsNull(jObject[i]["upper_val"].ToString(), "0");
                        dtrowLines["lower_val"] = IsNull(jObject[i]["lower_val"].ToString(), "0");
                        dtrowLines["uom_id"] = IsNull(jObject[i]["uom_id"].ToString(), "0");
                        dtrowLines["remarks"] = jObject[i]["remarks"].ToString();
                        dtrowLines["sr_no"] = Convert.ToInt32(jObject[i]["sr_no"].ToString());
                        dtParaDetail.Rows.Add(dtrowLines);
                    }

                    QCItemDetail = dtParaDetail;

                    Message = _qcItemParameterSetup_ISERVICES.insertQCDetails(QCItemDetail);

                    //Session["ItemId"] = Message.Split('-')[0].Trim();
                    //Session["Action"] = Message.Split('-')[1].Trim();
                    qCItem.ItemId = Message.Split('-')[0].Trim();
                    qCItem.Action = Message.Split('-')[1].Trim();
                    if (Message.Split('-')[1].Trim() == "Save" || Message.Split('-')[1].Trim() == "Update")
                    {
                        //Session["MessageIPS"] = "Save";
                        qCItem.MessageIPS = "Save";
                    }

                }
                return Json(Message);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //   return Json("ErrorPage");
            }
        }

        public ActionResult QCParameterApprove(QCItemParameterSetup qCItem, string itemId, string Status, string ListFilterData1)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                string UserId = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserId = Session["UserId"].ToString();
                }

                DataSet value = _qcItemParameterSetup_ISERVICES.GetItemSetupToApprove(itemId, Comp_ID, UserId, Status);
                if (value.Tables[0].Rows[0]["Result"].ToString() == itemId)
                {
                    //Session["MessageIPS"] = "Approved";
                    qCItem.MessageIPS = "Approved";
                    qCItem.Action = "Approved";
                    qCItem.ItemId = value.Tables[0].Rows[0]["Result"].ToString();
                }
                TempData["ListFilterData "] = ListFilterData1;
                DataRows = Json(value.Tables[0].Rows[0]["Result"].ToString());
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

            return DataRows;
        }
        private string IsNull(string Str, string Str2)
        {
            if (!string.IsNullOrEmpty(Str) && Str != "null")
            {
            }
            else
                Str = Str2;
            return Str;
        }
        public List<Uom> GetUomList()
        {
            try
            {
                CompID = Session["CompId"].ToString();
                DataTable dt = _qcItemParameterSetup_ISERVICES.GetUomIdList(CompID);

                List<Uom> modelList = new List<Uom>();
                Uom _UomList1 = new Uom();
                _UomList1.uom_id = "0";
                _UomList1.uom_name = "Select";
                modelList.Add(_UomList1);

                foreach (DataRow dr in dt.Rows)
                {
                    Uom _UomList = new Uom();
                    _UomList.uom_id = dr["uom_id"].ToString();
                    _UomList.uom_name = dr["uom_name"].ToString();

                    modelList.Add(_UomList);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        private DataTable GetRoleList()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        [HttpGet]
        public JsonResult ItemParaListDetails(string Value)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataTable dt = _qcItemParameterSetup_ISERVICES.GetItemSetupList(Convert.ToInt32(Comp_ID), BrchID, Value);

                List<BindItemNameList> modelList = new List<BindItemNameList>();
                DataRow Drow = dt.NewRow();
                Drow[1] = "---Select---";
                Drow[2] = "0";
                dt.Rows.InsertAt(Drow, 0);
                foreach (DataRow dr in dt.Rows)
                {

                    BindItemNameList _ItemDetailModel = new BindItemNameList();
                    _ItemDetailModel.item_id = dr["item_id"].ToString();
                    _ItemDetailModel.item_Name = dr["item_name"].ToString();
                    _ItemDetailModel.UOMName = dr["uom_name"].ToString();
                    _ItemDetailModel.OEMNo = dr["item_oem_no"].ToString();
                    _ItemDetailModel.SampleCode = dr["item_sam_cd"].ToString();
                    _ItemDetailModel.RefNo = dr["item_leg_cd"].ToString();


                    modelList.Add(_ItemDetailModel);

                }
                return Json(modelList, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        private string getDocumentName()
        {
            try
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
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult getQCItemValues(QCItemParameterSetup qCItem, string ItemID)
        {
            JsonResult DataRows = null;
            try
            {
                //Session["Action"] = "Update";
                qCItem.Action = "Update";
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet result = _qcItemParameterSetup_ISERVICES.GetQCItemDetailLIst(ItemID, Comp_ID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public string QCParameterDelete(QCItemParameterSetup qCItem, string itemId)
        {
            string Result = string.Empty;
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet value = _qcItemParameterSetup_ISERVICES.GetItemSetupToDelete(itemId, Comp_ID);
                Result = value.Tables[0].Rows[0]["Result"].ToString();
                if (Result == "Delete")
                {
                    //Session["MessageIPS"] = "Delete";
                    qCItem.MessageIPS = "Delete";
                }
                return Result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }


        }
        [HttpPost]
        //[OutputCache(Duration = 0)]
        public ActionResult GetItemParamListFilter(string ItemID, string ItemGrpID)
        {
            ViewBag.VBSupplierList = null;
            string Comp_ID = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            QCItemParameterSetupViewModel _QCItemParameterSetupViewModel = new QCItemParameterSetupViewModel();
            try
            {
                DataTable dt = _qcItemParameterSetup_ISERVICES.GetItemParamListFilterDAL(Comp_ID, ItemID, ItemGrpID).Tables[0];
                //Session["IPSearch"] = "IP_Search";
                _QCItemParameterSetupViewModel.IPSearch = "IP_Search";
                List<getViewModelItemList> modelList = new List<getViewModelItemList>();
                //  var value = _QCItemParameter_ISERVICES.GetItemParameterList();
                foreach (DataRow dr in dt.Rows)
                {

                    getViewModelItemList _ParameterItemList = new getViewModelItemList();
                    _ParameterItemList.item_Name = dr["ItemName"].ToString();
                    _ParameterItemList.item_id = dr["ItemID"].ToString();
                    // _ParameterItemList.param_Id = dr["param_Id"].ToString();
                    _ParameterItemList.group = dr["GroupName"].ToString();
                    _ParameterItemList.UOM = dr["UOM"].ToString();
                    _ParameterItemList.OEM_No = dr["OEM_no"].ToString();
                    _ParameterItemList.Sample_code = dr["SampleCode"].ToString();
                    _ParameterItemList.Status = dr["StatusName"].ToString();
                    _ParameterItemList.create_by = dr["create_by"].ToString();
                    _ParameterItemList.CreatedOn = dr["CreateOn"].ToString();
                    _ParameterItemList.app_by = dr["app_by"].ToString();
                    _ParameterItemList.ApprovedOn = dr["approvedOn"].ToString();
                    _ParameterItemList.mod_by = dr["mod_by"].ToString();
                    _ParameterItemList.AmendedOn = dr["amendedOn"].ToString();

                    modelList.Add(_ParameterItemList);

                }
                _QCItemParameterSetupViewModel.getViewModelItemList = modelList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

            }

            return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialItemParamList.cshtml", _QCItemParameterSetupViewModel);
        }

        public ActionResult DownloadFile()
        {
            try
            {
                string compId = string.Empty, Br_Id = string.Empty;
                if (Session["CompID"] != null)
                    compId = Session["CompID"].ToString();
                if (Session["BranchId"] != null)
                {
                    Br_Id = Session["BranchId"].ToString();
                }
                CommonController com_obj = new CommonController();
                DataTable QCParaDetail = new DataTable();
                DataSet obj_ds = new DataSet();

                QCParaDetail.Columns.Add("Item Name*(max 100 characters)", typeof(string));
                QCParaDetail.Columns.Add("Parameter Name*(max 50 characters)", typeof(string));
                QCParaDetail.Columns.Add("Parameter Type*", typeof(string));
                QCParaDetail.Columns.Add("Lower Range", typeof(string));
                QCParaDetail.Columns.Add("Upper Range", typeof(string));
                QCParaDetail.Columns.Add("UOM", typeof(string));
                QCParaDetail.Columns.Add("Remarks(max 100 characters)", typeof(string));

                obj_ds.Tables.Add(QCParaDetail);
                DataSet ds = _qcItemParameterSetup_ISERVICES.GetMasterDropDownList(compId, Br_Id);
                CommonController obj = new CommonController();
                string filePath = obj.CreateExcelFile("ImportQCItemTemplate", Server);
                //UpdateExcel(filePath, ds, QCParaDetail);
                com_obj.AppendExcel(filePath, ds, obj_ds, "QCItemParameter");
                string fileName = Path.GetFileName(filePath);
                return File(filePath, "application/octet-stream", fileName);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ValidateExcelFile(string uploadStatus)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            conString = "Invalid File";
                            break;
                    }
                    if (conString == "Invalid File")
                        return Json("Invalid File. Please upload a valid file", JsonRequestBehavior.AllowGet);
                    DataSet ds = new DataSet();
                    DataTable QCDetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string DetailQuery = "SELECT DISTINCT * FROM [ParameterDetail$] WHERE LEN([Item Name*(max 100 characters)]) > 0;";

                                connExcel.Open();
                                cmdExcel.CommandText = DetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(QCDetail);

                                ds.Tables.Add(QCDetail);
                                DataSet dts = VerifyData(ds, uploadStatus);
                                if (dts == null)
                                    return Json("Excel file is empty. Please fill data in excel file and try again");
                                ViewBag.ImportQCdata = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialImportQCItemDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet VerifyData(DataSet ds, string uploadStatus)
        {
            string compId = ""; string br_id = string.Empty;
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
            {
                br_id = Session["BranchId"].ToString();
            }
            DataTable dts = PrepareDataset(ds);
            if (ds.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[0].Rows[0].ToString()))
            {
                DataSet result = _qcItemParameterSetup_ISERVICES.GetVerifiedDataOfExcel(compId, br_id, dts);
                if (uploadStatus.Trim() == "0")
                    return result;

                var filteredRows = result.Tables[0].AsEnumerable().Where(x => x.Field<string>("UploadStatus").ToUpper() == uploadStatus.ToUpper()).ToList();
                DataTable newDataTable = filteredRows.Any() ? filteredRows.CopyToDataTable() : result.Tables[0].Clone();
                result.Tables[0].Clear();

                for (int i = 0; i < newDataTable.Rows.Count; i++)
                {
                    result.Tables[0].ImportRow(newDataTable.Rows[i]);
                }
                result.Tables[0].AcceptChanges();
                return result;
            }
            else
            {
                return null;
            }
        }
        public DataTable PrepareDataset(DataSet ds)
        {
            DataTable QCItemDetail = new DataTable();
            QCItemDetail.Columns.Add("Item_name", typeof(string));
            QCItemDetail.Columns.Add("param_name", typeof(string));
            QCItemDetail.Columns.Add("param_type", typeof(string));
            QCItemDetail.Columns.Add("lower_val", typeof(string));
            QCItemDetail.Columns.Add("upper_val", typeof(string));
            QCItemDetail.Columns.Add("uom", typeof(string));
            QCItemDetail.Columns.Add("remarks", typeof(string));

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataTable dspd = ds.Tables[0];
                DataRow dtr = QCItemDetail.NewRow();
                dtr["item_name"] = dspd.Rows[i][0].ToString().Trim();
                dtr["param_name"] = dspd.Rows[i][1].ToString().Trim();
                dtr["param_type"] = dspd.Rows[i][2].ToString().Trim();

                double lowval = 0;
                double.TryParse(dspd.Rows[i][3].ToString().Trim(), out lowval);
                dtr["lower_val"] = lowval.ToString();

                double upval = 0;
                double.TryParse(dspd.Rows[i][4].ToString().Trim(), out upval);
                dtr["upper_val"] = upval.ToString();
                if (dspd.Rows[i][5].ToString().Trim() == null || dspd.Rows[i][5].ToString().Trim() == "")
                {
                    dtr["uom"] = 0;
                }
                else
                {
                    dtr["uom"] = dspd.Rows[i][5].ToString().Trim();
                }
                dtr["remarks"] = dspd.Rows[i][6].ToString().Trim();
                QCItemDetail.Rows.Add(dtr);
            }
            return QCItemDetail;
        }
        public ActionResult ShowValidationError(string ItemName, string paramname)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable QCDetail = new DataTable();
                    conString = string.Format(conString, filePath);
                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                //string DetailQuery = "SELECT DISTINCT * FROM [ParameterDetail$] WHERE [Item Name*(max 100 characters)] = '" + ItemName + "' and Parameter Name*(max 50 characters) = '" + paramname+"'; ";
                                string DetailQuery = "SELECT DISTINCT * FROM [ParameterDetail$] WHERE [Item Name*(max 100 characters)] = '" + ItemName + "' AND [Parameter Name*(max 50 characters)] = '" + paramname + "';";
                                connExcel.Open();
                                cmdExcel.CommandText = DetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(QCDetail);

                                ds.Tables.Add(QCDetail);
                                DataTable dts = VerifySingleData(ds);
                                ViewBag.ErrorDetails = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialExportErrorDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (!string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("Cannot find table 0"))
                {
                    return Json("Please fill all mandatory fields.");
                }
                else
                {
                    return Json("ErrorPage");
                }

            }
        }
        private DataTable VerifySingleData(DataSet ds)
        {
            string compId = ""; string br_id = string.Empty;
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
            {
                br_id = Session["BranchId"].ToString();
            }
            DataTable dts = PrepareDataset(ds);
            DataTable result = _qcItemParameterSetup_ISERVICES.ShowExcelErrorDetail(compId, br_id, dts);
            return result;
        }
        public JsonResult ImportQCItemDetailFromExcel()
        {
            try
            {
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable QCDetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string DetailQuery = "SELECT DISTINCT * FROM [ParameterDetail$] WHERE LEN([Item Name*(max 100 characters)]) > 0;";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = DetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(QCDetail);

                                ds.Tables.Add(QCDetail);
                                string msg = SavePDFromExcel(ds);
                                return Json(msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                }
                else
                    return Json("No file selected", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("cannot insert duplicate"))
                    return Json("something went wrong", JsonRequestBehavior.AllowGet);
                else
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private string SavePDFromExcel(DataSet ds)
        {
            string compId = "";
            string UserID = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            DataTable dts = PrepareDataset(ds);
            string result = _qcItemParameterSetup_ISERVICES.BulkImportQCItemDetail(compId, UserID, dts);
            return result;
        }

        [HttpPost]
        public ActionResult PageLoadData(string ItemID, string ItemGrpID)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn1 = Request.Form.GetValues("columns[" + Request.Form.GetValues
                    ("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToUpper();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
              //  List<getViewModelItemList> modelList = new List<getViewModelItemList>();
                List<getViewModelItemList> _ItemListModel = new List<getViewModelItemList>();

                _ItemListModel = GetDataSR(ItemID, ItemGrpID);
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn1) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn1 + " " + sortColumnDir);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    ItemListData = ItemListData.Where(m => m.sr_no.ToString().ToUpper().Contains(searchValue) || m.item_id.ToUpper().Contains(searchValue)
                    || m.item_Name.ToUpper().Contains(searchValue) || m.group.ToUpper().Contains(searchValue)
                    || m.UOM.ToUpper().Contains(searchValue)
                    || m.OEM_No.ToUpper().Contains(searchValue) || m.Sample_code.ToUpper().Contains(searchValue)
                    || m.Status.ToUpper().Contains(searchValue) || m.create_by.ToUpper().Contains(searchValue)
                    || m.CreatedOn.ToUpper().Contains(searchValue) || m.create_by.ToUpper().Contains(searchValue)
                    || m.app_by.ToUpper().Contains(searchValue) || m.mod_by.ToUpper().Contains(searchValue)
                    || m.AmendedOn.ToUpper().Contains(searchValue) || m.ApprovedOn.ToUpper().Contains(searchValue)
                    );
                }

                recordsTotal = ItemListData.Count();

                var data = ItemListData.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }

        private List<getViewModelItemList> GetDataSR(string ItemID, string ItemGrpID)
        {

            try
            {
                DataSet dt = new DataSet();
                string User_ID = string.Empty;
                string CompID = string.Empty;
                List<getViewModelItemList> QCList = new List<getViewModelItemList>();
              //  getViewModelItemList _QcListModel = new getViewModelItemList();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                //  DataTable DSet = new DataTable();
                DataSet DSet = new DataSet();

                DSet = _qcItemParameterSetup_ISERVICES.GetItemParamListFilterDAL(CompID, ItemID, ItemGrpID);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    int rowno1 = 0;
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        rowno1++;   // Increment the counter

                        getViewModelItemList _ParameterItemList = new getViewModelItemList();
                        _ParameterItemList.sr_no = rowno1;
                        _ParameterItemList.item_Name = dr["ItemName"].ToString();
                        _ParameterItemList.item_id = dr["ItemID"].ToString();
                        // _ParameterItemList.param_Id = dr["param_Id"].ToString();
                        _ParameterItemList.group = dr["GroupName"].ToString();
                        _ParameterItemList.UOM = dr["UOM"].ToString();
                        _ParameterItemList.OEM_No = dr["OEM_no"].ToString();
                        _ParameterItemList.Sample_code = dr["SampleCode"].ToString();
                        _ParameterItemList.Status = dr["StatusName"].ToString();
                        _ParameterItemList.create_by = dr["create_by"].ToString();
                        _ParameterItemList.CreatedOn = dr["CreateOn"].ToString();
                        _ParameterItemList.app_by = dr["app_by"].ToString();
                        _ParameterItemList.ApprovedOn = dr["approvedOn"].ToString();
                        _ParameterItemList.mod_by = dr["mod_by"].ToString();
                        _ParameterItemList.AmendedOn = dr["amendedOn"].ToString();

                        QCList.Add(_ParameterItemList);
                    }
                }

               


                return QCList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public FileResult ExcelDownload(string searchValue,string ItemID, string ItemGrpID )
        {
            try
            {
                string ExcelName = string.Empty;
                string User_ID = string.Empty;
                string CompID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                var sortColumn = "sr_no";// Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = "asc";// Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                DataTable dt = new DataTable();


                List<getViewModelItemList> _ItemListModel = new List<getViewModelItemList>();

                _ItemListModel = GetDataSR(ItemID, ItemGrpID);
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    ItemListData = ItemListData.Where(m => m.sr_no.ToString().ToUpper().Contains(searchValue) || m.item_id.ToUpper().Contains(searchValue)
                    || m.item_Name.ToUpper().Contains(searchValue) || m.group.ToUpper().Contains(searchValue)
                    || m.UOM.ToUpper().Contains(searchValue)
                    || m.OEM_No.ToUpper().Contains(searchValue) || m.Sample_code.ToUpper().Contains(searchValue)
                    || m.Status.ToUpper().Contains(searchValue) || m.create_by.ToUpper().Contains(searchValue)
                    || m.CreatedOn.ToUpper().Contains(searchValue) || m.create_by.ToUpper().Contains(searchValue)
                    || m.app_by.ToUpper().Contains(searchValue) || m.mod_by.ToUpper().Contains(searchValue)
                    || m.AmendedOn.ToUpper().Contains(searchValue) || m.ApprovedOn.ToUpper().Contains(searchValue)
                    );
                }

                var data = ItemListData.ToList();


                dt = ItemParameterExcel(data);
                ExcelName = "ItemParameter";
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel(ExcelName, dt);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable ItemParameterExcel(List<getViewModelItemList> _ItemListModel)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Sr No.", typeof(int));
                dataTable.Columns.Add("Item Name", typeof(string));
                dataTable.Columns.Add("Group", typeof(string));

                dataTable.Columns.Add("UOM", typeof(string));
                dataTable.Columns.Add("OEM No.", typeof(string));
                dataTable.Columns.Add("Sample Code", typeof(string));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("Created By", typeof(string));
                dataTable.Columns.Add("Created On", typeof(string));
                dataTable.Columns.Add("Approved By", typeof(string));
                dataTable.Columns.Add("Approved On", typeof(string));
                dataTable.Columns.Add("Amended By", typeof(string));
                dataTable.Columns.Add("Amended On", typeof(string));


                foreach (var item in _ItemListModel)
                {
                    DataRow rows = dataTable.NewRow();
                    rows["Sr No."] = item.sr_no;
                    rows["Item Name"] = item.item_Name;
                    rows["Group"] = item.group;
                    rows["UOM"] = item.UOM;
                    rows["OEM No."] = item.OEM_No;
                    rows["Sample Code"] = item.Sample_code;
                    rows["Status"] = item.Status;
                    rows["Created By"] = item.create_by;
                    rows["Created On"] = item.CreatedOn;
                    rows["Approved By"] = item.app_by;
                    rows["Approved On"] = item.ApprovedOn;
                    rows["Amended By"] = item.mod_by;
                    rows["Amended On"] = item.AmendedOn;
                    dataTable.Rows.Add(rows);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
    }
}