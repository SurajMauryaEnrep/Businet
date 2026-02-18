using System;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.MIS.MISAssetRegistration;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.MIS.MISAssetRegistration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;


namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FixedAssetManagement.MIS.MISAssetRegister
{
    public class MISAssetRegisterController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105106125101", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        MISAssetRegistration_ISERVICES _MISAssetRegistration_ISERVICES;
        public MISAssetRegisterController(Common_IServices _Common_IServices, MISAssetRegistration_ISERVICES _MISAssetRegistration_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._MISAssetRegistration_ISERVICES = _MISAssetRegistration_ISERVICES;
        }

        public ActionResult MISAssetRegister()
        {
            try
            {
                CommonPageDetails();
                MISAssetRegistration_Model _model = new MISAssetRegistration_Model();
                ViewBag.MenuPageName = getDocumentName();
                GetAssetGroup(_model);
                GetAssetCategory("0", _model);
                GetAssignedRequirementArea(_model);
                _model.Title = title;
                APListSearch("0", "0", "0", "0");
                ViewBag.ListSearch = "";
                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/MIS/MISAssetRegister/MISAssetRegister.cshtml", _model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
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
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public ActionResult GetAssetGroup(MISAssetRegistration_Model _model)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _MISAssetRegistration_ISERVICES.GetAssetGroupListPage(CompID, "0");
                List<AssetsGroup> _COACList = new List<AssetsGroup>();
                foreach (var data in AGAList)
                {
                    AssetsGroup _COADetail = new AssetsGroup();
                    _COADetail.assetgrp_id = data.Key;
                    _COADetail.assetgrp_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _model.AssetsGroupList = _COACList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(AGAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAssignedRequirementArea(MISAssetRegistration_Model _model)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet ds = _MISAssetRegistration_ISERVICES.GetAssignedRequirementArea(CompID, BrchID, "MIS");

                List<AssignedRequirementArea> Lists = new List<AssignedRequirementArea>();
                foreach (DataRow data in ds.Tables[0].Rows)
                {
                    AssignedRequirementArea _COADetail = new AssignedRequirementArea();
                    _COADetail.AssignedRequirementArea_id = data.Table.Columns["acc_id"].ToString();
                    _COADetail.AssignedRequirementArea_name = data.Table.Columns["acc_name"].ToString();
                    Lists.Add(_COADetail);
                }
                _model.AssignedRequirementAreaList = Lists;
                DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        public ActionResult GetAssetCategory(string AssetGroupId, MISAssetRegistration_Model _AssetRegistrationGroupDetail)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _MISAssetRegistration_ISERVICES.GetAssetCategory(CompID, AssetGroupId);
                List<AssetsGroupCat> _COACList = new List<AssetsGroupCat>();
                foreach (var data in AGAList)
                {
                    AssetsGroupCat _COADetail = new AssetsGroupCat();
                    _COADetail.assetgrpcat_id = data.Key;
                    _COADetail.assetgrpcat_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssetsGroupCatList = _COACList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(AGAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public void GetCompDeatil()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            if (Session["UserID"] != null)
            {
                UserID = Session["UserID"].ToString();
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
        }
        private void CommonPageDetails()
        {
            try
            {
                GetCompDeatil();
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
        public ActionResult APListSearch(string Group, string Category, string RequirementArea, string WorkingStatus)
        {
            try
            {
                CommonPageDetails();
                DataSet ds = new DataSet();
                ds = _MISAssetRegistration_ISERVICES.GetAllData(CompID, BrchID, Group, Category, RequirementArea, WorkingStatus, "");
                ViewBag.ListDetail = ds.Tables[0];
                ViewBag.ListSearch = "ListSearch";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISAssetRegisterList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public ActionResult GetAssetRegHistory(string RegId)
        {
            try
            {
                ViewBag.OrderItemDetailList = GetRegistrationHistory(RegId);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDepreciationHistory.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public DataTable GetRegistrationHistory(string RegId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                GetCompDeatil();
                ds = _MISAssetRegistration_ISERVICES.GetRegistrationHistory(CompID, BrchID, RegId);
                dt = ds.Tables[0];
                ViewBag.DepreciationHistory = ds.Tables[0];
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private DataTable Getcurr()
        {
            try
            {
                CommonPageDetails();
                DataTable dt = _MISAssetRegistration_ISERVICES.GetCurrList(BrchID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public ActionResult GetAssetProcDetail(string RegId)
        {
            try
            {
                ViewBag.PD_DisableFlag = "Y";
                ViewBag.ProcrumentDetailData = GetAssetProcurmentDetail(RegId);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAssetProcrumentDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public DataTable GetAssetProcurmentDetail(string RegId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                GetCompDeatil();
                ds = _MISAssetRegistration_ISERVICES.GetAssetProcurmentDetail(CompID, BrchID, RegId);
                dt = ds.Tables[0];
                ViewBag.ProcrumentDetailData = ds.Tables[0];
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
