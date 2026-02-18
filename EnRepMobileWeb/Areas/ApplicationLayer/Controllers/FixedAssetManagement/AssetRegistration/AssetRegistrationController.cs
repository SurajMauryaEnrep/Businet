using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetRegistration;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetRegistration;
using System;
using System.Web.Mvc;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using System.Configuration;
using System.Data.OleDb;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FixedAssetManagement.AssetRegistration
{
    public class AssetRegistrationController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105106101", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        AssetRegistration_ISERVICES _AssetRegistration_ISERVICES;
        public AssetRegistrationController(Common_IServices _Common_IServices, AssetRegistration_ISERVICES _AssetRegistration_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._AssetRegistration_ISERVICES = _AssetRegistration_ISERVICES;
        }
        public ActionResult AssetRegistration(string wfStatus)
        {
            try
            {
                GetCompDeatil();
                CommonPageDetails();
                ARList_Model _model = new ARList_Model();
                if (wfStatus != null)
                {
                    _model.WF_status = wfStatus;
                    _model.ListFilterData = "0,0,0,0" + "," + wfStatus;
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                List<statusLists> statusLists = new List<statusLists>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    statusLists list = new statusLists();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _model.statusLists = statusLists;
                _model.Command = "Add";
                _model.TransType = "Save";
                _model.BtnName = "BtnAddNew";
                _model.title = title;
                _model.PQASearch = "0";
                GetAssetGroup(_model);
                GetAssetCategory("0", _model);
                List<RequirementArea> Lists = new List<RequirementArea>();
                Lists.Add(new RequirementArea { RequirementArea_name = "---Select---", RequirementArea_id = "0" });
                _model.RequirementAreaList = Lists;
                _model.WorkingStatusId = "I";
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    if (ListFilterData != null && ListFilterData != "")
                    {
                        var a = ListFilterData.Split(',');
                        //var SupplierID = a[0].Trim();
                        //var Fromdate = a[1].Trim();
                        //var Todate = a[2].Trim();
                        //var Status = 
                        //if (Status == "0")
                        //{
                        //    Status = null;
                        //}
                        _model.AssetsGroupId = a[0].Trim();
                        _model.AssetsGroupCatId = a[1].Trim();
                        _model.RequirementAreaId = a[2].Trim();
                        _model.WorkingStatusId = a[3].Trim();
                        _model.statusId = a[4].Trim();
                        _model.AR_status = a[4].Trim();
                        _model.ListFilterData = TempData["ListFilterData"].ToString();
                    }
                }
                GetAllListData(_model);
                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetRegistration/AssetRegistrationList.cshtml", _model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private DataSet GetFyList()
        {
            try
            {
                string Comp_ID = string.Empty, BrID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                DataSet dt = _AssetRegistration_ISERVICES.Get_FYList(Comp_ID, BrID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult APListSearch(string Group, string Category, string RequirementArea, string WorkingStatus, string Status)
        {
            try
            {
                CommonPageDetails();
                DataSet ds = new DataSet();
                ds = _AssetRegistration_ISERVICES.GetAllData(CompID, BrchID, Group, Category, RequirementArea, WorkingStatus, Status);
                ViewBag.ListDetail = ds.Tables[0];
                ViewBag.ListSearch = "ListSearch";
                ViewBag.ListFilterData1 = Group + "," + Category + "," + RequirementArea + "," + WorkingStatus + "," + Status;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAssetRegistrationList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private void GetAllListData(ARList_Model _List_Model)
        {
            try
            {
                string SupplierName = string.Empty;
                CommonPageDetails();

                string assetGroupId = "0";// _List_Model.AssetsGroupId ?? 0;
                if (_List_Model.AssetsGroupId == null)
                {
                    assetGroupId = "0";
                }
                else
                {
                    assetGroupId = _List_Model.AssetsGroupId;
                }
                DataSet CustList = _AssetRegistration_ISERVICES.GetAllData(CompID, BrchID, assetGroupId,
                    _List_Model.AssetsGroupCatId, _List_Model.RequirementAreaId, _List_Model.WorkingStatusId, _List_Model.AR_status);
                ViewBag.ListDetail = CustList.Tables[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        public ActionResult AddAssetRegistrationDetail(string RegId, string ListFilterData)
        {
            try
            {
                UrlData urlData = new UrlData();
                /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                GetCompDeatil();
                var commCont = new CommonController(_Common_IServices);
                if (RegId == null)
                {
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        SetUrlData(urlData, "", "", "", "Financial Year not Exist", RegId, ListFilterData);
                        return RedirectToAction("AssetRegistrationDetail", "AssetRegistration", urlData);
                    }
                }
                /*End to chk Financial year exist or not*/
                string BtnName = RegId == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = RegId == null ? "Save" : "Update";
                SetUrlData(urlData, "Add", TransType, BtnName, null, RegId, ListFilterData);
                return RedirectToAction("AssetRegistrationDetail", "AssetRegistration", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult AssetRegistrationDetail(UrlData urlData)
        {
            try
            {
                AssetRegistration_Model model = new AssetRegistration_Model();
                CommonPageDetails();
                GetCompDeatil();
                /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, urlData.Asset_reg_Id) == "TransNotAllow")
                //{
                //    //TempData["Message2"] = "TransNotAllow";
                //    ViewBag.Message = "TransNotAllow";
                //}
                if (TempData["UrlData"] != null)
                {
                    urlData = TempData["UrlData"] as UrlData;
                    model.Message = TempData["Message"] != null ? TempData["Message"].ToString() : null;
                }
                model.Command = urlData.Command;
                model.TransType = urlData.TransType;
                model.BtnName = urlData.BtnName;
                model.ListFilterData = urlData.ListFilterData1;
                ViewBag.MenuPageName = getDocumentName();
                model.title = title;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = model.TransType;
                ViewBag.DocumentStatus = model.DocumentStatus;
                ViewBag.Command = model.Command;
                GetAssetDescription(model, "A");
                GetSerialNo(model, "A", 0);
                GetAssetGroupDetailView(model);

                DataSet dttbl = new DataSet();
                dttbl = GetFyList();
                if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
                {
                    ViewBag.CFYStartDateAR = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                    ViewBag.FromFyMindateAR = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                    ViewBag.ToFyMaxdateAR = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();
                    //ViewBag.fylist = dttbl.Tables[1];
                }
                //List<Currency> Currency = new List<Currency>();
                //Currency.Insert(0, new Currency() { curr_id = "0", curr_val = model.ProcurementDetail.CurrencyId });
                //model.ProcurementDetail.CurrencyList = Currency;

                List<AssignedRequirementArea> Lists = new List<AssignedRequirementArea>();
                Lists.Add(new AssignedRequirementArea { AssignedRequirementArea_name = "---Select---", AssignedRequirementArea_id = "0" });
                model.AssignedRequirementAreaList = Lists;
                model.CurrentYearFirstDate = ViewBag.CFYStartDateAR;
                Getcurr();

                if (urlData.Asset_reg_Id != null)
                {
                    GetAssetDescription(model, "A");

                    DataSet ds = GetAssetRegDetailEdit(urlData.Asset_reg_Id);
                    ViewBag.AttechmentDetails = ds.Tables[1];
                    ViewBag.DepCalendar = ds.Tables[3];
                    model.AssetRegId = ds.Tables[0].Rows[0]["ass_reg_id"].ToString(); //int.Parse(ds.Tables[0].Rows[0]["Asset_grp_id"].ToString());
                    model.AssetItemsId = ds.Tables[0].Rows[0]["asset_id"].ToString();
                    GetSerialNo(model, "E", int.Parse(model.AssetRegId));
                    model.AssetLabel = ds.Tables[0].Rows[0]["asset_label"].ToString();
                    model.SerialNumber = ds.Tables[0].Rows[0]["serial_no"].ToString();
                    model.SerialNumber1 = ds.Tables[0].Rows[0]["serial_no"].ToString();
                    model.AssetsGroupId = ds.Tables[0].Rows[0]["asset_grp_id"].ToString();
                    model.AssetCategory = ds.Tables[0].Rows[0]["setup_val"].ToString();
                    model.AssetCategoryId = ds.Tables[0].Rows[0]["asset_cat_id"].ToString();
                    model.ProcurementDate = ds.Tables[0].Rows[0]["procur_dt"].ToString();
                    model.SupplierName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                    model.BillNumber = ds.Tables[0].Rows[0]["bill_no"].ToString();
                    model.BillDate = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                    model.ProcuredValue = ds.Tables[0].Rows[0]["proc_val"].ToString();
                    model.CurrentValue = ds.Tables[0].Rows[0]["curr_val"].ToString();
                    model.AsOn = ds.Tables[0].Rows[0]["ason_dt"].ToString();
                    model.AssetLife = ds.Tables[0].Rows[0]["assetlife"].ToString();
                    model.DepreciationPer = ds.Tables[0].Rows[0]["dep_per"].ToString();
                    model.Depreciationfreq = ds.Tables[0].Rows[0]["dep_frequency"].ToString();
                    model.AssetLifeLabel = "(in " + ds.Tables[0].Rows[0]["dep_frequency"].ToString() + ")";
                    model.DepreciationMethod = ds.Tables[0].Rows[0]["DepreciationMethod"].ToString();
                    model.AddDepreciationPer = ds.Tables[0].Rows[0]["add_dep_per"].ToString(); //int.Parse(ds.Tables[0].Rows[0]["asset_cat"].ToString());
                    model.ValidUpto = ds.Tables[0].Rows[0]["validupto"].ToString();
                    model.AssetWorkingDate = ds.Tables[0].Rows[0]["asset_working_dt"].ToString();
                    model.DepreciationStartDate = ds.Tables[0].Rows[0]["DepStartDate"].ToString();
                    model.AssignedRequirementAreaId = ds.Tables[0].Rows[0]["assign_req_area"].ToString();
                    model.AssignedRequirementAreaType = ds.Tables[0].Rows[0]["assign_req_area_type"].ToString();
                    model.AccumulatedDepreciation = ds.Tables[0].Rows[0]["accumulated_dep"].ToString();
                    model.WorkingStatusId = ds.Tables[0].Rows[0]["working_status"].ToString().Trim();
                    model.Create_by = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    model.Create_on = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                    model.Amended_by = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    model.Amended_on = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                    model.Approved_by = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                    model.Approved_on = ds.Tables[0].Rows[0]["app_on"].ToString();
                    model.DocumentStatus = ds.Tables[0].Rows[0]["ar_status"].ToString().Trim();
                    model.StatusName = ds.Tables[0].Rows[0]["ARStatus"].ToString();
                    ViewBag.AddDepreciationPer = ds.Tables[0].Rows[0]["add_dep_per"].ToString();
                    ViewBag.UsedInDP = ds.Tables[0].Rows[0]["UsedInDP"].ToString();
                    ViewBag.DepreciationHistory = ds.Tables[2];
                    ViewBag.PD_DisableFlag = "N";
                    ViewBag.ProcrumentDetailData = ds.Tables[4];
                    model.CurrecyID = ds.Tables[0].Rows[0]["bs_curr_id"].ToString().Trim();

                }
                model.TransType = urlData.TransType;
                model.BtnName = urlData.BtnName;
                model.Command = urlData.Command;
                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetRegistration/AssetRegistrationDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAssetRegDetailEdit(string regId)
        {
            try
            {
                GetCompDeatil();
                DataSet result = _AssetRegistration_ISERVICES.GetAssetRegistrationDetail(CompID, BrchID, Convert.ToInt32(regId));
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
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
        public ActionResult ErrorPage()
        {
            try
            {
                throw new Exception("JsError");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage", JsonRequestBehavior.AllowGet);
            }
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string Asset_reg_Id = null,
            string ListFilterData1 = null)
        {
            try
            {
                urlData.Command = Command;
                urlData.TransType = TransType;
                urlData.BtnName = BtnName;
                urlData.Asset_reg_Id = Asset_reg_Id;
                urlData.ListFilterData1 = ListFilterData1;
                TempData["UrlData"] = urlData;
                TempData["Message"] = Message;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FA_AR_ActionCommands(AssetRegistration_Model _model, string Command)
        {
            try
            {
                var commCont = new CommonController(_Common_IServices);
                UrlData urlData = new UrlData();
                if (_model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                GetCompDeatil();
                switch (Command)
                {
                    case "AddNew":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            //if (!string.IsNullOrEmpty(_model.Inv_no))
                            //{
                            //    SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", "Financial Year not Exist", _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                            //    return RedirectToAction("AssetRegistrationDetail", _model);
                            //}
                            //return RedirectToAction("AddAssetRegistrationDetail", new { DocNo = _model.SPO_No, DocDate = _model.SPO_Date, ListFilterData = _model.ListFilterData1, WF_status = _Model.WFStatus });
                            //else
                            //{
                            //    SetUrlData(urlData, "Refresh", "Refresh", "Refresh", "Financial Year not Exist", null, null, _model.ListFilterData1);
                            //    return RedirectToAction("AssetRegistrationDetail", _model);
                            //}
                        }
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, _model.ListFilterData);
                        return RedirectToAction("AssetRegistrationDetail", urlData);
                    case "Save":
                        SaveAssetRegDetail(_model);
                        if (_model.Message == "Duplicate")
                        {
                            return RedirectToAction("AssetRegistration_DuplicateCase", _model);
                        }
                        else
                        {
                            SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.AssetRegId, _model.ListFilterData);
                            return RedirectToAction("AssetRegistrationDetail", urlData);
                        }
                    case "Approve":
                        ApprovARDetails(_model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.AssetRegId, _model.ListFilterData);
                        return RedirectToAction("AssetRegistrationDetail", urlData);
                    case "Edit":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            //return RedirectToAction("DoubleClickOnList", new { DocNo = _model.QA_No, DocDate = _model.QA_Date, ListFilterData = _model.ListFilterData1, WF_status = _model.WFStatus });
                        }
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        //string invdt = _model.QA_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invdt) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    //SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _model.QA_No, _model.QA_Date, _model.ListFilterData1);
                        //    return RedirectToAction("AssetRegistrationDetail", urlData);
                        //}
                        /*End to chk Financial year exist or not*/

                        _model.TransType = "Update";
                        _model.BtnName = "BtnEdit";
                        _model.Message = null;
                        Command = _model.Command;
                        TempData["ModelData"] = _model;
                        SetUrlData(urlData, _model.Command, "Update", _model.BtnName, _model.Message, _model.AssetRegId, _model.ListFilterData);
                        return RedirectToAction("AssetRegistrationDetail", urlData);

                    case "Delete":
                        DeleteARDetail(_model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, _model.ListFilterData);
                        return RedirectToAction("AssetRegistrationDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, _model.ListFilterData);
                        return RedirectToAction("AssetRegistrationDetail", urlData);
                    case "BacktoList":
                        //TempData["WF_status"] = _model.DocumentStatus;
                        TempData["ListFilterData"] = _model.ListFilterData;
                        SetUrlData(urlData, "", "", "", null, null, _model.ListFilterData);
                        return RedirectToAction("AssetRegistration");
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("AssetRegistrationDetail", urlData);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ApprovARDetails(AssetRegistration_Model _model)
        {
            try
            {
                UrlData urlData = new UrlData();
                GetCompDeatil();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Result = _AssetRegistration_ISERVICES.ApproveAssetRegistration(_model.AssetRegId, CompID, BrchID, "",
                    UserID, mac_id);
                try
                {//string fileName = "QuotationAnalysis_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    //var filePath = SavePdfDocToSendOnEmailAlert(Inv_No, Inv_Date, fileName, DocumentMenuId, "AP");
                    //_Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, Inv_No, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                if (Result.Split(',')[1] == "A")
                {
                    _model.Message = _model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                _model.Message = Result.Split(',')[1] == "A" ? "Approved" : "Error";
                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, Result.Split(',')[0], "");
                return RedirectToAction("AssetRegistrationDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private void DeleteARDetail(AssetRegistration_Model _model)
        {
            try
            {
                GetCompDeatil();
                string Result = _AssetRegistration_ISERVICES.DeleteARetails(CompID, BrchID, _model.AssetRegId);
                _model.Message = Result.Split(',')[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult GetAssetGroup(ARList_Model _AssetRegistrationGroupDetail)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _AssetRegistration_ISERVICES.GetAssetGroupListPage(CompID, "0");
                List<AssetsGroup> _COACList = new List<AssetsGroup>();
                foreach (var data in AGAList)
                {
                    AssetsGroup _COADetail = new AssetsGroup();
                    _COADetail.assetgrp_id = data.Key;
                    _COADetail.assetgrp_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssetsGroupList = _COACList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(AGAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAssetCategory(string AssetGroupId, ARList_Model _AssetRegistrationGroupDetail)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _AssetRegistration_ISERVICES.GetAssetCategory(CompID, AssetGroupId);
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

        public ActionResult GetAssetDescription(AssetRegistration_Model _AssetRegistrationGroupDetail, string ShowFor)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _AssetRegistration_ISERVICES.GetAssetItem(CompID, BrchID, ShowFor);
                List<AssetItemsLists> _COACList = new List<AssetItemsLists>();
                foreach (var data in AGAList)
                {
                    AssetItemsLists _COADetail = new AssetItemsLists();
                    _COADetail.AssetItem_id = data.Key;
                    _COADetail.AssetItem_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssetItems = _COACList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(AGAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSerialNo(AssetRegistration_Model _AssetRegistrationGroupDetail, string ShowFor, int RegId)
        {
            CommonPageDetails();
            Dictionary<string, string> SerialNoList = new Dictionary<string, string>();
            try
            {
                SerialNoList = _AssetRegistration_ISERVICES.GetSerialNo(CompID, BrchID, _AssetRegistrationGroupDetail.AssetItemsId, ShowFor, RegId);
                List<AssetSerialNoLists> _COACList = new List<AssetSerialNoLists>();
                foreach (var data in SerialNoList)
                {
                    AssetSerialNoLists _COADetail = new AssetSerialNoLists();
                    _COADetail.SerialNo_id = data.Key;
                    _COADetail.SerialNo_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssetSerialNo = _COACList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(SerialNoList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSerialNoJS(AssetRegistration_Model _AssetRegistrationGroupDetail, string AssetDescriptionId)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                ARList_Model model = new ARList_Model();
                DataSet ds = _AssetRegistration_ISERVICES.GetSerialNoJs(CompID, BrchID, AssetDescriptionId, "A");

                List<AssetSerialNoLists> Lists = new List<AssetSerialNoLists>();
                foreach (DataRow data in ds.Tables[0].Rows)
                {
                    AssetSerialNoLists _COADetail = new AssetSerialNoLists();
                    _COADetail.SerialNo_id = data.Table.Columns["serial_noid"].ToString();
                    _COADetail.SerialNo_name = data.Table.Columns["serial_no"].ToString();
                    Lists.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssetSerialNo = Lists;
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
        public ActionResult GetAssetGroupDetailView(AssetRegistration_Model _AssetRegistrationGroupDetail)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _AssetRegistration_ISERVICES.GetAssetGroup(CompID, "0");
                List<AssetsGroup> _COACList = new List<AssetsGroup>();
                foreach (var data in AGAList)
                {
                    AssetsGroup _COADetail = new AssetsGroup();
                    _COADetail.assetgrp_id = data.Key;
                    _COADetail.assetgrp_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssetsGroupList = _COACList;
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
        public JsonResult GetAssignedRequirementArea(AssetRegistration_Model _AssetRegistrationGroupDetail)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                ARList_Model model = new ARList_Model();
                DataSet ds = _AssetRegistration_ISERVICES.GetAssignedRequirementArea(CompID, BrchID);

                List<AssignedRequirementArea> Lists = new List<AssignedRequirementArea>();
                foreach (DataRow data in ds.Tables[0].Rows)
                {
                    AssignedRequirementArea _COADetail = new AssignedRequirementArea();
                    _COADetail.AssignedRequirementArea_id = data.Table.Columns["acc_id"].ToString();
                    _COADetail.AssignedRequirementArea_name = data.Table.Columns["acc_name"].ToString();
                    Lists.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssignedRequirementAreaList = Lists;
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
        [HttpPost]
        public JsonResult GetAssetCategoryDetails(string AssetGroupId)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet Details = _AssetRegistration_ISERVICES.GetAssetCategoryDetails(CompID, BrchID, AssetGroupId);
                DataRows = Json(JsonConvert.SerializeObject(Details));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public DataTable dtPDdetail(JArray JAObj)
        {
            DataTable PD_Details = new DataTable();
            PD_Details.Columns.Add("src_type", typeof(string));
            PD_Details.Columns.Add("bill_no", typeof(string));
            PD_Details.Columns.Add("bill_dt", typeof(DateTime));
            PD_Details.Columns.Add("supplier", typeof(string));
            PD_Details.Columns.Add("supp_id", typeof(string));
            PD_Details.Columns.Add("GSTIN", typeof(string));
            PD_Details.Columns.Add("Curr_Id", typeof(string));
            PD_Details.Columns.Add("pur_val", typeof(string));
            PD_Details.Columns.Add("tax_val", typeof(string));
            PD_Details.Columns.Add("oc_val", typeof(string));
            PD_Details.Columns.Add("total_val", typeof(string));
            PD_Details.Columns.Add("capitalized_val", typeof(string));
            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = PD_Details.NewRow();

                //dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                //dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                //dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                //dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                //dtrowLines["cc_name"] = JAObj[i]["ddl_CC_Name"].ToString();
                //dtrowLines["cc_val_name"] = JAObj[i]["ddl_CC_Type"].ToString();

                dtrowLines["src_type"] = JAObj[i]["src_type"].ToString();
                dtrowLines["bill_no"] = JAObj[i]["billno"].ToString();
                dtrowLines["bill_dt"] = JAObj[i]["bill_dt"].ToString();
                dtrowLines["supplier"] = JAObj[i]["supplier"].ToString();
                dtrowLines["supp_id"] = JAObj[i]["supplierid"].ToString();
                dtrowLines["GSTIN"] = JAObj[i]["gst"].ToString();
                dtrowLines["Curr_Id"] = JAObj[i]["currencyid"].ToString();
                dtrowLines["pur_val"] = JAObj[i]["purchase_price"].ToString();
                dtrowLines["tax_val"] = JAObj[i]["taxamount"].ToString();
                dtrowLines["oc_val"] = JAObj[i]["othercharges"].ToString();
                dtrowLines["total_val"] = JAObj[i]["totalcost"].ToString();
                dtrowLines["capitalized_val"] = JAObj[i]["capVal"].ToString();
                PD_Details.Rows.Add(dtrowLines);
            }
            return PD_Details;
        }
        private DataTable ToDtblProcDetail(string PD_DetailList)
        {
            try
            {
                DataTable PD_Details = new DataTable();
                PD_Details.Columns.Add("src_type", typeof(string));
                PD_Details.Columns.Add("bill_no", typeof(string));
                PD_Details.Columns.Add("bill_dt", typeof(DateTime));
                PD_Details.Columns.Add("supplier", typeof(string));
                PD_Details.Columns.Add("supp_id", typeof(string));
                PD_Details.Columns.Add("GSTIN", typeof(string));
                PD_Details.Columns.Add("Curr_Id", typeof(string));
                PD_Details.Columns.Add("pur_val", typeof(string));
                PD_Details.Columns.Add("tax_val", typeof(string));
                PD_Details.Columns.Add("oc_val", typeof(string));
                PD_Details.Columns.Add("total_val", typeof(string));
                PD_Details.Columns.Add("capitalized_val", typeof(string));

                if (PD_DetailList != null)
                {
                    JArray JAObj = JArray.Parse(PD_DetailList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = PD_Details.NewRow();
                        dtrowLines["src_type"] = JAObj[i]["src_type"].ToString();
                        dtrowLines["bill_no"] = JAObj[i]["billno"].ToString();
                        //dtrowLines["bill_dt"] = JAObj[i]["bill_dt"].ToString();
                        DateTime parsedDate;
                        if (DateTime.TryParse(JAObj[i]["bill_dt"].ToString(), out parsedDate))
                        {
                            dtrowLines["bill_dt"] = parsedDate;
                        }
                        else
                        {
                            dtrowLines["bill_dt"] = DBNull.Value;  // Handle invalid date
                        }
                        dtrowLines["supplier"] = JAObj[i]["supplier"].ToString();
                        dtrowLines["supp_id"] = JAObj[i]["supplierid"].ToString();
                        dtrowLines["GSTIN"] = JAObj[i]["gst"].ToString();
                        dtrowLines["Curr_Id"] = JAObj[i]["currencyid"].ToString();
                        dtrowLines["pur_val"] = JAObj[i]["purchase_price"].ToString();
                        dtrowLines["tax_val"] = JAObj[i]["taxamount"].ToString();
                        dtrowLines["oc_val"] = JAObj[i]["othercharges"].ToString();
                        dtrowLines["total_val"] = JAObj[i]["totalcost"].ToString();
                        dtrowLines["capitalized_val"] = JAObj[i]["capVal"].ToString();
                        PD_Details.Rows.Add(dtrowLines);
                    }
                    ViewData["PDdetail"] = dtPDdetail(JAObj);
                }
                return PD_Details;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult SaveAssetRegDetail(AssetRegistration_Model ObjAddItemGroupSetupBOL)
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
                    ObjAddItemGroupSetupBOL.CompId = CompID;
                    ObjAddItemGroupSetupBOL.BrdId = BrchID;
                    ObjAddItemGroupSetupBOL.Create_id = UserID;
                    //ObjAddItemGroupSetupBOL.AccumulatedDepreciation = "0";
                    //var type = ObjAddItemGroupSetupBOL.AssignedRequirementAreaType;
                    //ObjAddItemGroupSetupBOL.AssignedRequirementAreaType = type.ToLower() == "customer" ? "C" :
                    //  type.ToLower() == "supplier" ? "S" :
                    //  type.ToLower() == "employee" ? "E" :
                    //  type.ToLower() == "requirment area" ? "R" : type.ToUpper();

                    var src_ra = ObjAddItemGroupSetupBOL.AssignedRequirementAreaId;
                    string[] parts = src_ra.Split('_');
                    ObjAddItemGroupSetupBOL.AssignedRequirementAreaId = parts[0];
                    ObjAddItemGroupSetupBOL.AssignedRequirementAreaType = parts[1];

                    if (ObjAddItemGroupSetupBOL.FormMode == "1")
                    {
                        ObjAddItemGroupSetupBOL.Create_id = UserID;
                    }
                }
                DataTable DtblAttchDetail = new DataTable();
                ObjAddItemGroupSetupBOL.title = title;
                DtblAttchDetail = ToDtblAttachmentDetail(ObjAddItemGroupSetupBOL);
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as AssetRegistrationattch;
                // ObjAddItemGroupSetupBOL.AssetLife =(ObjAddItemGroupSetupBOL.AssetLife);
                TempData["ModelDataattch"] = null;
                if (ObjAddItemGroupSetupBOL.ValidUpto == null)
                {
                    ObjAddItemGroupSetupBOL.ValidUpto = "0";
                }
                if (ObjAddItemGroupSetupBOL.AccumulatedDepreciation == null)
                {
                    ObjAddItemGroupSetupBOL.AccumulatedDepreciation = "0";
                }
                if (ObjAddItemGroupSetupBOL.CurrentValue == null)
                {
                    ObjAddItemGroupSetupBOL.CurrentValue = "0";
                }
                ObjAddItemGroupSetupBOL.ProcuredValue = ObjAddItemGroupSetupBOL.ProcuredValue.Replace(",", "");
                ObjAddItemGroupSetupBOL.CurrentValue = ObjAddItemGroupSetupBOL.CurrentValue.Replace(",", "");
                ObjAddItemGroupSetupBOL.AccumulatedDepreciation = ObjAddItemGroupSetupBOL.AccumulatedDepreciation.Replace(",", "");

                DataTable DtblProcurmentDetail = new DataTable();
                DtblProcurmentDetail = ToDtblProcDetail(ObjAddItemGroupSetupBOL.PD_DetailList);

                if (ObjAddItemGroupSetupBOL.AssetItemsId.ToString() != "0")
                {
                    string status = _AssetRegistration_ISERVICES.InsertAssetRegDetail(ObjAddItemGroupSetupBOL, mac_id, DtblAttchDetail, DtblProcurmentDetail);
                    string AssetRegId = status.Substring(status.IndexOf('-') + 1);
                    string Message = status.Substring(0, status.IndexOf("-"));
                    if (Message == "Update" || Message == "Save")
                    {
                        ObjAddItemGroupSetupBOL.Message = "Save";
                        ObjAddItemGroupSetupBOL.AssetRegId = AssetRegId;
                        ObjAddItemGroupSetupBOL.TransType = "Update";
                        string Guid = "";
                        if (_DirectPurchaseInvoiceattch != null)
                        {
                            if (_DirectPurchaseInvoiceattch.Guid != null)
                            {
                                Guid = _DirectPurchaseInvoiceattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, title, AssetRegId, ObjAddItemGroupSetupBOL.TransType, DtblAttchDetail);
                    }
                    if (Message == "Duplicate")
                    {
                        ObjAddItemGroupSetupBOL.TransType = "Duplicate";
                        ObjAddItemGroupSetupBOL.Message = "Duplicate";
                        ObjAddItemGroupSetupBOL.AssetRegId = AssetRegId;
                        ObjAddItemGroupSetupBOL.BtnName = AssetRegId;
                        TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                        return RedirectToAction("AssetRegistration_DuplicateCase", "AssetRegistration");
                    }
                    ObjAddItemGroupSetupBOL.BtnName = "BtnSave";
                    TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                    return RedirectToAction("AddAssetRegistrationDetail", "AssetRegistration");
                }
                TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                return RedirectToAction("AddAssetRegistrationDetail", "AssetRegistration");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult AssetRegistration_DuplicateCase(AssetRegistration_Model _Model)
        {
            try
            {
                var AssetRegistration_Model = TempData["ModelData"] as AssetRegistration_Model;
                UrlData urlData = new UrlData();
                if (AssetRegistration_Model != null)
                {
                    if (Session["compid"] != null)
                    {
                        CommonPageDetails();
                        GetCompDeatil();
                        /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        var commCont = new CommonController(_Common_IServices);
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, urlData.Asset_reg_Id) == "TransNotAllow")
                        {
                            //TempData["Message2"] = "TransNotAllow";
                            ViewBag.Message = "TransNotAllow";
                        }
                        if (TempData["UrlData"] != null)
                        {
                            urlData = TempData["UrlData"] as UrlData;
                            _Model.Message = TempData["Message"] != null ? TempData["Message"].ToString() : null;
                        }
                        _Model.Command = urlData.Command;
                        _Model.TransType = urlData.TransType;
                        _Model.BtnName = urlData.BtnName;
                        _Model.ListFilterData = urlData.ListFilterData1;

                        ViewBag.MenuPageName = getDocumentName();
                        _Model.title = title;
                        _Model.DocumentMenuId = DocumentMenuId;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        ViewBag.TransType = _Model.TransType;
                        ViewBag.DocumentStatus = _Model.DocumentStatus;
                        ViewBag.Command = _Model.Command;
                        GetAssetDescription(_Model, "A");
                        GetSerialNo(_Model, "A", 0);
                        GetAssetGroupDetailView(_Model);
                        DataSet dttbl = new DataSet();
                        dttbl = GetFyList();
                        if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
                        {
                            ViewBag.CFYStartDateAR = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                            ViewBag.FromFyMindateAR = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                            ViewBag.ToFyMaxdateAR = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();
                            //ViewBag.fylist = dttbl.Tables[1];
                        }
                        //_Model(model);
                        //_Model.AsOn = ViewBag.CFYStartDateAR;
                        List<AssignedRequirementArea> Lists = new List<AssignedRequirementArea>();
                        Lists.Add(new AssignedRequirementArea { AssignedRequirementArea_name = "---Select---", AssignedRequirementArea_id = "0" });
                        _Model.AssignedRequirementAreaList = Lists;
                        _Model.AssetItemsId = _Model.AssetItemsId;
                        _Model.AssetsGroupId = _Model.AssetsGroupId;
                        _Model.AssignedRequirementAreaId = _Model.AssignedRequirementAreaId;
                        _Model.TransType = "Save";
                        _Model.BtnName = "BtnAddNew";
                        _Model.Command = "Add";
                        return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetRegistration/AssetRegistrationDetail.cshtml", _Model);
                    }
                    else
                    {
                        return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetRegistration/AssetRegistrationDetail.cshtml", _Model);
                    }
                }
                else
                {
                    if (Session["compid"] != null)
                    {
                        DataSet ds = _AssetRegistration_ISERVICES.GetAssetRegistrationDetail(CompID, BrchID, Convert.ToInt32(_Model.AssetRegId));
                        if (ds.Tables.Count > 0)
                        {
                            _Model.AssetRegId = ds.Tables[0].Rows[0]["ass_reg_id"].ToString(); //int.Parse(ds.Tables[0].Rows[0]["Asset_grp_id"].ToString());
                            _Model.AssetItemsId = ds.Tables[0].Rows[0]["asset_id"].ToString();
                            _Model.AssetLabel = ds.Tables[0].Rows[0]["asset_label"].ToString();
                            _Model.SerialNumber = ds.Tables[0].Rows[0]["serial_no"].ToString();
                            _Model.AssetsGroupId = ds.Tables[0].Rows[0]["asset_grp_id"].ToString();
                            _Model.AssetCategory = ds.Tables[0].Rows[0]["setup_val"].ToString();
                            _Model.AssetCategoryId = ds.Tables[0].Rows[0]["asset_cat_id"].ToString();
                            _Model.ProcurementDate = ds.Tables[0].Rows[0]["procur_dt"].ToString();
                            _Model.SupplierName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _Model.BillNumber = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _Model.BillDate = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                            _Model.ProcuredValue = ds.Tables[0].Rows[0]["proc_val"].ToString();
                            _Model.CurrentValue = ds.Tables[0].Rows[0]["curr_val"].ToString();
                            _Model.AsOn = ds.Tables[0].Rows[0]["ason_dt"].ToString();
                            _Model.AssetLife = ds.Tables[0].Rows[0]["assetlife"].ToString();
                            _Model.AssetLifeLabel = "(in " + ds.Tables[0].Rows[0]["dep_frequency"].ToString() + ")";
                            _Model.DepreciationPer = ds.Tables[0].Rows[0]["dep_per"].ToString();
                            _Model.Depreciationfreq = ds.Tables[0].Rows[0]["dep_frequency"].ToString();
                            _Model.AssetLifeLabel = "(in " + ds.Tables[0].Rows[0]["dep_frequency"].ToString() + ")";
                            _Model.DepreciationMethod = ds.Tables[0].Rows[0]["DepreciationMethod"].ToString();
                            _Model.AddDepreciationPer = ds.Tables[0].Rows[0]["add_dep_per"].ToString(); //int.Parse(ds.Tables[0].Rows[0]["asset_cat"].ToString());
                            _Model.ValidUpto = ds.Tables[0].Rows[0]["validupto"].ToString();
                            _Model.AssetWorkingDate = ds.Tables[0].Rows[0]["asset_working_dt"].ToString();
                            _Model.DepreciationStartDate = ds.Tables[0].Rows[0]["DepStartDate"].ToString();
                            _Model.AssignedRequirementAreaId = ds.Tables[0].Rows[0]["assign_req_area"].ToString();
                            _Model.AssignedRequirementAreaId = ds.Tables[0].Rows[0]["assign_req_area"].ToString();
                            _Model.AssignedRequirementAreaType = ds.Tables[0].Rows[0]["assign_req_area_type"].ToString();
                            _Model.WorkingStatusId = ds.Tables[0].Rows[0]["working_status"].ToString();
                            _Model.Create_by = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            _Model.Create_on = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            _Model.Amended_by = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            _Model.Amended_on = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                            _Model.Approved_by = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                            _Model.Approved_on = ds.Tables[0].Rows[0]["app_on"].ToString();
                            ViewBag.AttechmentDetails = ds.Tables[1];
                            ViewBag.DepCalendar = ds.Tables[3];
                            ViewBag.AddDepreciationPer = ds.Tables[0].Rows[0]["add_dep_per"].ToString();
                            ViewBag.UsedInDP = ds.Tables[0].Rows[0]["UsedInDP"].ToString();
                            // _Model.AsOn = ViewBag.CFYStartDateAR;
                        }
                    }
                    _Model.DocumentMenuId = DocumentMenuId;
                    return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetRegistration/AssetRegistrationDetail.cshtml", _Model);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblAttachmentDetail(AssetRegistration_Model _model)
        {
            try
            {
                string PageName = _model.title.Replace(" ", "");
                DataTable dtAttachment = new DataTable();
                GetCompDeatil();
                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as AssetRegistrationattch;

                if (_model.attatchmentdetail != null)
                {
                    if (_DirectPurchaseInvoiceattch != null)
                    {
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (_DirectPurchaseInvoiceattch.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _DirectPurchaseInvoiceattch.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }
                    else
                    {
                        if (_model.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _model.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }
                    JArray jObject1 = JArray.Parse(_model.attatchmentdetail);
                    for (int i = 0; i < jObject1.Count; i++)
                    {
                        string flag = "Y";
                        foreach (DataRow dr in dtAttachment.Rows)
                        {
                            string drImg = dr["file_name"].ToString();
                            string ObjImg = jObject1[i]["file_name"].ToString();
                            if (drImg == ObjImg)
                            {
                                flag = "N";
                            }
                        }
                        if (flag == "Y")
                        {
                            DataRow dtrowAttachment1 = dtAttachment.NewRow();
                            if (!string.IsNullOrEmpty(_model.SerialNumber))
                            {
                                dtrowAttachment1["id"] = _model.SerialNumber;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y";
                            dtrowAttachment1["comp_id"] = CompID;
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_model.TransType == "Update")
                    {
                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_model.SerialNumber))
                            {
                                ItmCode = _model.SerialNumber;
                            }
                            else
                            {
                                ItmCode = "0";
                            }
                            string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + ItmCode.Replace("/", "") + "*");

                            foreach (var fielpath in filePaths)
                            {
                                string flag = "Y";
                                foreach (DataRow dr in dtAttachment.Rows)
                                {
                                    string drImgPath = dr["file_path"].ToString();
                                    if (drImgPath == fielpath.Replace("/", @"\"))
                                    {
                                        flag = "N";
                                    }
                                }
                                if (flag == "Y")
                                {
                                    System.IO.File.Delete(fielpath);
                                }
                            }
                        }
                    }
                }
                return dtAttachment;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                AssetRegistrationattch _ScrapSIModelattch = new AssetRegistrationattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                string branchID = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _ScrapSIModelattch.Guid = DocNo;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    branchID = Session["BranchId"].ToString();
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + branchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _ScrapSIModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _ScrapSIModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _ScrapSIModelattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult GetAssetTransferDetails(string AssetDescriptionTD, string SerialNumberTD)
        {
            try
            {
                ViewBag.AssetTransferDetails = GetTransferDetail(AssetDescriptionTD, SerialNumberTD);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialTransferDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public DataTable GetTransferDetail(string AssetDescriptionTD, string SerialNumberTD)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                GetCompDeatil();
                ds = _AssetRegistration_ISERVICES.GetTransferDetail(CompID, BrchID, AssetDescriptionTD, SerialNumberTD);
                dt = ds.Tables[0];
                ViewBag.AssetTransferDetails = ds.Tables[0];
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetAssetProcurementDetails(string AssetDescriptionTD, string DisableFlag, string PD_rowdata)
        {
            try
            {
                if (AssetDescriptionTD != "0")
                {
                    Getcurr();
                    ViewBag.PD_DisableFlag = DisableFlag;

                    if (PD_rowdata.ToString() != null && PD_rowdata.ToString() != "" && PD_rowdata.ToString() != "[]")
                    {
                        DataTable PDDetails = new DataTable();
                        PDDetails.Columns.Add("BillNumber", typeof(string));
                        PDDetails.Columns.Add("BillDate", typeof(string));
                        PDDetails.Columns.Add("BillDt", typeof(string));
                        PDDetails.Columns.Add("SupplierName", typeof(string));
                        PDDetails.Columns.Add("GSTNumber", typeof(string));
                        PDDetails.Columns.Add("Currency", typeof(string));
                        PDDetails.Columns.Add("CurrencyId", typeof(string));
                        PDDetails.Columns.Add("PurchasePrice", typeof(string));
                        PDDetails.Columns.Add("TaxAmount", typeof(string));
                        PDDetails.Columns.Add("OtherCharges", typeof(string));
                        PDDetails.Columns.Add("TotalCost", typeof(string));
                        PDDetails.Columns.Add("CapitalizedValue", typeof(string));
                        JArray arr = JArray.Parse(PD_rowdata);
                        for (int i = 0; i < arr.Count; i++)
                        {
                            DataRow dtrowLines = PDDetails.NewRow();
                            dtrowLines["BillNumber"] = arr[i]["BillNumber"].ToString();
                            dtrowLines["BillDate"] = arr[i]["BillDate"].ToString();
                            dtrowLines["BillDt"] = arr[i]["BillDt"].ToString();
                            dtrowLines["SupplierName"] = arr[i]["Supplier"].ToString();
                            dtrowLines["GSTNumber"] = arr[i]["GST"].ToString();
                            dtrowLines["Currency"] = arr[i]["Currency"].ToString();
                            dtrowLines["CurrencyId"] = arr[i]["CurrencyId"].ToString();
                            dtrowLines["PurchasePrice"] = arr[i]["PurchasePrice"].ToString();
                            dtrowLines["TaxAmount"] = arr[i]["TaxAmount"].ToString();
                            dtrowLines["OtherCharges"] = arr[i]["OtherCharges"].ToString();
                            dtrowLines["TotalCost"] = arr[i]["TotalCost"].ToString();
                            dtrowLines["CapitalizedValue"] = arr[i]["CapitalizedValue"].ToString();
                            PDDetails.Rows.Add(dtrowLines);
                        }
                        ViewBag.ProcrumentDetailData = PDDetails;
                    }

                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAssetProcrumentDetail.cshtml");
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        //[NonAction]
        private DataTable Getcurr()
        {
            try
            {
                CommonPageDetails();
                DataTable dt = _AssetRegistration_ISERVICES.GetCurrList(BrchID);
                ViewBag.BrCurrId = dt.Rows[0]["curr_id"].ToString();
                ViewBag.BrCurrName = dt.Rows[0]["curr_name"].ToString();
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        #region File Upload 
        public ActionResult DownloadFile()
        {
            try
            {
                string compId = "0";
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                CommonController com_obj = new CommonController();
                DataTable AssetRegDetail = new DataTable();
                DataTable CaptVal = new DataTable();

                DataSet obj_ds = new DataSet();
                AssetRegDetail.Columns.Add("Asset Description*", typeof(string));
                AssetRegDetail.Columns.Add("Asset Label*(max 100 characters)", typeof(string));
                AssetRegDetail.Columns.Add("Serial Number*", typeof(string));
                AssetRegDetail.Columns.Add("Group*", typeof(string));
                //AssetRegDetail.Columns.Add("Capitalized Value*", typeof(string));
                AssetRegDetail.Columns.Add("Additional Depreciation(in %)", typeof(string));
                AssetRegDetail.Columns.Add("Valid Upto (As per Depreciation Frequency)", typeof(string));
                AssetRegDetail.Columns.Add("Asset Working Date*(DD/MM/YYYY)", typeof(string));
                AssetRegDetail.Columns.Add("Assigned Requirement Area*", typeof(string));

                CaptVal.Columns.Add("Asset Description*", typeof(string));
                //CaptVal.Columns.Add("Asset Label*(max 100 characters)", typeof(string));
                CaptVal.Columns.Add("Serial Number*", typeof(string));
                CaptVal.Columns.Add("Bill Number*(max 20 characters)", typeof(string));
                CaptVal.Columns.Add("Bill Date*(DD/MM/YYYY)", typeof(string));
                CaptVal.Columns.Add("Supplier Name*(max 100 characters)", typeof(string));
                CaptVal.Columns.Add("GST Number(max 20 characters)", typeof(string));
                CaptVal.Columns.Add("Purchase Amount (In Base)*", typeof(string));
                CaptVal.Columns.Add("Tax Amount (In Base)*", typeof(string));
                CaptVal.Columns.Add("Other Charges(In Base)*", typeof(string));

                DataSet ds = _AssetRegistration_ISERVICES.GetMasterDropDownList(compId, Br_ID);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = AssetRegDetail.NewRow();
                        row["Asset Description*"] = ds.Tables[0].Rows[i]["AssetDescription"]?.ToString();
                        row["Serial Number*"] = ds.Tables[0].Rows[i]["SerialNumber"]?.ToString();
                        AssetRegDetail.Rows.Add(row);
                    }
                }

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = CaptVal.NewRow();
                        row["Asset Description*"] = ds.Tables[0].Rows[i]["AssetDescription"]?.ToString();
                        row["Serial Number*"] = ds.Tables[0].Rows[i]["SerialNumber"]?.ToString();
                        CaptVal.Rows.Add(row);
                    }
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    ds.Tables.RemoveAt(0);
                }
                obj_ds.Tables.Add(AssetRegDetail);
                obj_ds.Tables.Add(CaptVal);
                string filePath = com_obj.CreateExcelFile("ImportAsssetRegistrationTemplate", Server);
                com_obj.AppendExcel(filePath, ds, obj_ds, "AsssetRegistrationSetup");
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
                    DataTable AssetDetail = new DataTable();
                    DataTable CapitalizedValue = new DataTable();
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
                                string AssetDetailQuery = "SELECT DISTINCT * FROM [AssetDetail$];";

                                //string AssetDetailQuery = "SELECT DISTINCT * FROM [AssetDetail$];";
                                string CapitalizedValueQuery = "SELECT * From [CapitalizedValue$]; ";

                                connExcel.Open();
                                cmdExcel.CommandText = AssetDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(AssetDetail);

                                cmdExcel.CommandText = CapitalizedValueQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CapitalizedValue);

                                connExcel.Close();

                                ds.Tables.Add(AssetDetail);
                                ds.Tables.Add(CapitalizedValue);
                                DataSet dts = VerifyData(ds, uploadStatus);
                                if (dts == null)
                                    return Json("Excel file is empty. Please fill data in excel file and try again");
                                ViewBag.ImportProcDetailPreview = dts;
                                ViewBag.ProcrumentDetailData = dts.Tables[1];
                            }
                        }
                    }
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialImportAssetRegistrationDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet VerifyData(DataSet dscustomer, string uploadStatus)
        {
            string compId = "", BrchID ="";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            DataSet dts = PrepareDataset(dscustomer);
            if (dscustomer.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dscustomer.Tables[0].Rows[0].ToString()))
            {
                DataSet result = _AssetRegistration_ISERVICES.GetVerifiedDataOfExcel(compId, BrchID, dts.Tables[0], dts.Tables[1]);
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
        public DataSet PrepareDataset(DataSet dscustomer)
        {
            DataTable AssetDetail = new DataTable();
            DataTable CapitalizedValue = new DataTable();

            AssetDetail.Columns.Add("asset_description", typeof(string));
            AssetDetail.Columns.Add("asset_label", typeof(string));
            AssetDetail.Columns.Add("serial_no", typeof(string));
            AssetDetail.Columns.Add("group", typeof(string));
            //AssetDetail.Columns.Add("capitalized_value", typeof(string));
            AssetDetail.Columns.Add("add_Depreciation", typeof(string));
            AssetDetail.Columns.Add("valid_upto", typeof(string));
            AssetDetail.Columns.Add("asset_working_dt", typeof(string));
            AssetDetail.Columns.Add("assigned_req_area", typeof(string));

            for (int i = 0; i < dscustomer.Tables[0].Rows.Count; i++)
            {
                DataTable dtcustomerdetail = dscustomer.Tables[0];
                DataRow dtr = AssetDetail.NewRow();

                dtr["asset_description"] = dtcustomerdetail.Rows[i][0].ToString().Trim();
                dtr["asset_label"] = dtcustomerdetail.Rows[i][1].ToString().Trim();
                dtr["serial_no"] = dtcustomerdetail.Rows[i][2].ToString().Trim();
                dtr["group"] = dtcustomerdetail.Rows[i][3].ToString().Trim();
                //dtr["capitalized_value"] = dtcustomerdetail.Rows[i][4].ToString().Trim();
                dtr["add_Depreciation"] = dtcustomerdetail.Rows[i][4].ToString().Trim();
                dtr["valid_upto"] = dtcustomerdetail.Rows[i][5].ToString().Trim();
                dtr["asset_working_dt"] = dtcustomerdetail.Rows[i][6].ToString().Trim();
                dtr["assigned_req_area"] = dtcustomerdetail.Rows[i][7].ToString().Trim();
                AssetDetail.Rows.Add(dtr);
            }
            //-------------------------------Capitalized Value Detail-------------------------

            CapitalizedValue.Columns.Add("asset_description", typeof(string));
            //CapitalizedValue.Columns.Add("asset_label", typeof(string));
            CapitalizedValue.Columns.Add("serial_no", typeof(string));
            CapitalizedValue.Columns.Add("bill_no", typeof(string));
            CapitalizedValue.Columns.Add("bill_dt", typeof(string));
            CapitalizedValue.Columns.Add("supplier_name", typeof(string));
            CapitalizedValue.Columns.Add("gst", typeof(string));
            CapitalizedValue.Columns.Add("purchase_amt", typeof(string));
            CapitalizedValue.Columns.Add("tax_amt", typeof(string));
            CapitalizedValue.Columns.Add("oc_amt", typeof(string));

            for (int i = 0; i < dscustomer.Tables[1].Rows.Count; i++)
            {
                DataTable dtbranchdetail = dscustomer.Tables[1];
                DataRow dtr = CapitalizedValue.NewRow();

                dtr["asset_description"] = dtbranchdetail.Rows[i][0].ToString().Trim();
               // dtr["asset_label"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                dtr["serial_no"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                dtr["bill_no"] = dtbranchdetail.Rows[i][2].ToString().Trim();
                dtr["bill_dt"] = dtbranchdetail.Rows[i][3].ToString().Trim();
                dtr["supplier_name"] = dtbranchdetail.Rows[i][4].ToString().Trim();
                dtr["gst"] = dtbranchdetail.Rows[i][5].ToString().Trim();
                dtr["purchase_amt"] = dtbranchdetail.Rows[i][6].ToString().Trim();
                dtr["tax_amt"] = dtbranchdetail.Rows[i][7].ToString().Trim();
                dtr["oc_amt"] = dtbranchdetail.Rows[i][8].ToString().Trim();
                CapitalizedValue.Rows.Add(dtr);
            }
            //---------------------------End-------------------------------------

            DataSet dts = new DataSet();
            dts.Tables.Add(AssetDetail);
            dts.Tables.Add(CapitalizedValue);
            return dts;
        }
        
        public ActionResult ShowValidationError(string AssetDes, string Assetlabel, string AssetSerialNo)
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
                    DataTable CustomerDetail = new DataTable();
                    DataTable CustomerBranch = new DataTable();
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
                                string CustomerDetailQuery = "";
                                AssetDes = AssetDes.Trim();
                                AssetSerialNo = AssetSerialNo.Trim();
                                Assetlabel = string.IsNullOrWhiteSpace(Assetlabel) ? "" : Assetlabel.Trim();
                                if (string.IsNullOrWhiteSpace(Assetlabel))
                                {
                                    CustomerDetailQuery = "SELECT DISTINCT * FROM [AssetDetail$] WHERE TRIM([Asset Description*]) = '" + AssetDes + "' and TRIM([Serial Number*]) = '" + AssetSerialNo + "';";
                                }
                                else
                                {
                                    CustomerDetailQuery = "SELECT DISTINCT * FROM [AssetDetail$] WHERE [Asset Description*] = '" + AssetDes + "' and [Asset Label*(max 100 characters)] = '" + Assetlabel + "' and [Serial Number*] = '" + AssetSerialNo + "';";
                                }
                                 string CustomerBranchQuery = "SELECT * From [CapitalizedValue$] where [Asset Description*] = '" + AssetDes + "' and [Serial Number*] = '" + AssetSerialNo + "' ; ";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = CustomerDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerDetail);

                                cmdExcel.CommandText = CustomerBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerBranch);

                                connExcel.Close();

                                ds.Tables.Add(CustomerDetail);
                                ds.Tables.Add(CustomerBranch);
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
                return Json("ErrorPage");
            }
        }
        private DataTable VerifySingleData(DataSet ds)
        {
            try
            {
            string compId = "", BrchID = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            DataSet dts = PrepareDataset(ds);
            DataTable result = _AssetRegistration_ISERVICES.ShowExcelErrorDetail(compId, BrchID, dts.Tables[0], dts.Tables[1]);
            return result;
            }
            catch(Exception ex) {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return new DataTable();
            }
        }
        public JsonResult ImportAssetRegistrationDetailFromExcel()
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
                    DataTable CustomerDetail = new DataTable();
                    DataTable CustomerBranch = new DataTable();
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
                                string CustomerDetailQuery = "SELECT DISTINCT * FROM [AssetDetail$] WHERE LEN([Group*]) > 0; ";
                                string CustomerBranchQuery = "SELECT DISTINCT * From [CapitalizedValue$]; ";
                                
                                connExcel.Open();
                                cmdExcel.CommandText = CustomerDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerDetail);

                                cmdExcel.CommandText = CustomerBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerBranch);
                                connExcel.Close();

                                ds.Tables.Add(CustomerDetail);
                                ds.Tables.Add(CustomerBranch);
                                string msg = SaveAssetRegistrationFromExcel(ds);
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
        private string SaveAssetRegistrationFromExcel(DataSet dsCustomer)
        {
            string compId = "";
            string UserID = "";
            string BranchName = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            if (Session["BranchName"] != null)
                BranchName = Session["BranchName"].ToString();
            DataSet dts = PrepareDataset(dsCustomer);
            string result = _AssetRegistration_ISERVICES.BulkImportAssetRegistrationDetail(compId, UserID, BrchID, dts.Tables[0], dts.Tables[1]);
            return result;
        }

        public ActionResult GetAssetProcurementDetailsList(string AssetDescriptionTD, string DisableFlag, string PD_rowdata)
        {
            try
            {
                if (AssetDescriptionTD != "0")
                {
                    Getcurr();
                    ViewBag.PD_DisableFlag = DisableFlag;

                    if (PD_rowdata.ToString() != null && PD_rowdata.ToString() != "" && PD_rowdata.ToString() != "[]")
                    {
                        DataTable PDDetails = new DataTable();
                        PDDetails.Columns.Add("BillNumber", typeof(string));
                        PDDetails.Columns.Add("BillDate", typeof(string));
                        PDDetails.Columns.Add("BillDt", typeof(string));
                        PDDetails.Columns.Add("SupplierName", typeof(string));
                        PDDetails.Columns.Add("GSTNumber", typeof(string));
                        PDDetails.Columns.Add("Currency", typeof(string));
                        PDDetails.Columns.Add("CurrencyId", typeof(string));
                        PDDetails.Columns.Add("PurchasePrice", typeof(string));
                        PDDetails.Columns.Add("TaxAmount", typeof(string));
                        PDDetails.Columns.Add("OtherCharges", typeof(string));
                        PDDetails.Columns.Add("TotalCost", typeof(string));
                        PDDetails.Columns.Add("CapitalizedValue", typeof(string));
                        JArray arr = JArray.Parse(PD_rowdata);
                        for (int i = 0; i < arr.Count; i++)
                        {
                            DataRow dtrowLines = PDDetails.NewRow();
                            dtrowLines["BillNumber"] = arr[i]["BillNumber"].ToString();
                            dtrowLines["BillDate"] = arr[i]["BillDate"].ToString();
                            dtrowLines["BillDt"] = arr[i]["BillDt"].ToString();
                            dtrowLines["SupplierName"] = arr[i]["Supplier"].ToString();
                            dtrowLines["GSTNumber"] = arr[i]["GST"].ToString();
                            dtrowLines["Currency"] = arr[i]["Currency"].ToString();
                            dtrowLines["CurrencyId"] = arr[i]["CurrencyId"].ToString();
                            dtrowLines["PurchasePrice"] = arr[i]["PurchasePrice"].ToString();
                            dtrowLines["TaxAmount"] = arr[i]["TaxAmount"].ToString();
                            dtrowLines["OtherCharges"] = arr[i]["OtherCharges"].ToString();
                            dtrowLines["TotalCost"] = arr[i]["TotalCost"].ToString();
                            dtrowLines["CapitalizedValue"] = arr[i]["CapitalizedValue"].ToString();
                            PDDetails.Rows.Add(dtrowLines);
                        }
                        ViewBag.ProcrumentDetailData = PDDetails;
                    }

                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAssetProcrumentDetail.cshtml");
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        #endregion
    }
}