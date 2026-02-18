using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetRetirement;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetRetirement;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
using System;
using System.Web.Mvc;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using System.Globalization;
using EnRepMobileWeb.Resources;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Configuration;
using ZXing;
using System.Text;
using System.ComponentModel;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FixedAssetManagement.AssetRetirement
{
    public class AssetRetirementController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105106120", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        AssetRetirement_ISERVICES _AssetRetirement_ISERVICES;
        public AssetRetirementController(Common_IServices _Common_IServices, AssetRetirement_ISERVICES _AssetRetirement_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._AssetRetirement_ISERVICES = _AssetRetirement_ISERVICES;
        }
        // GET: ApplicationLayer/AssetRetirement
        public ActionResult AssetRetirement(string wfStatus)
        {
            ViewBag.MenuPageName = getDocumentName();
            GetCompDeatil();
            CommonPageDetails();
            AssetRetirementList_Model _model = new AssetRetirementList_Model();
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
            _model.DocumentMenuId = DocumentMenuId;
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                if (ListFilterData != null && ListFilterData != "")
                {
                    var a = ListFilterData.Split(',');
                    var Status = a[1].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    _model.AssetsGroupId = a[0].Trim();
                    _model.ListFilterData = TempData["ListFilterData"].ToString();
                    _model.AR_status = Status;
                }
            }
            _model.title = title;
            _model.PQASearch = "0";
            GetAssetGroupDetailListView(_model);
            GetAllListData(_model);
            return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetRetirement/AssetRetirementList.cshtml", _model);
        }

        //public ActionResult AddAssetRetirementDetail()
        //{
        //    Session["Message"] = "New";
        //    Session["Command"] = "Add";
        //    Session["AppStatus"] = 'D';
        //    Session["TransType"] = "Save";
        //    Session["BtnName"] = "BtnAddNew";
        //    ViewBag.MenuPageName = getDocumentName();
        //    return RedirectToAction("AssetRetirementDetail", "AssetRetirement");
        //}
        //public ActionResult AssetRetirementDetail()
        //{
        //    try
        //    {
        //        ViewBag.MenuPageName = getDocumentName();
        //        return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetRetirement/AssetRetirementDetail.cshtml");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        public ActionResult AddAssetRetirementDetail(string DocNo, string DocDt, string ListFilterData)
        {
            try
            {
                UrlData urlData = new UrlData();
                /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                GetCompDeatil();
                var commCont = new CommonController(_Common_IServices);
                if (DocNo == null)
                {
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        SetUrlData(urlData, "", "", "", "Financial Year not Exist", DocNo, DocDt, ListFilterData);
                        return RedirectToAction("AssetRetirementDetail", "AssetRetirement", urlData);
                    }
                }
                /*End to chk Financial year exist or not*/
                string BtnName = DocNo == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = DocNo == null ? "Save" : "Update";
                SetUrlData(urlData, "Add", TransType, BtnName, null, DocNo, DocDt, ListFilterData);
                return RedirectToAction("AssetRetirementDetail", "AssetRetirement", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
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
        public ActionResult AssetRetireListSearch(string Group, string Status)
        {
            try
            {
                CommonPageDetails();
                DataSet ds = new DataSet();
                ds = _AssetRetirement_ISERVICES.GetAllData(CompID, BrchID, Group, Status, UserID);
                ViewBag.ListDetail = ds.Tables[0];
                ViewBag.ListSearch = "ListSearch";
                ViewBag.ListFilterData1 = Group + "," + Status;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialRetirementList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private void GetAllListData(AssetRetirementList_Model _List_Model)
        {
            try
            {
                string SupplierName = string.Empty;
                CommonPageDetails();
                DataSet CustList = _AssetRetirement_ISERVICES.GetAllData(CompID, BrchID, _List_Model.AssetsGroupId, _List_Model.AR_status, UserID);
                ViewBag.ListDetail = CustList.Tables[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
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
        public ActionResult AssetRetirementDetail(UrlData urlData)
        {
            try
            {
                AssetRetirement_Model model = new AssetRetirement_Model();
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
                model.ListFilterData = urlData.ListFilterData;
                ViewBag.MenuPageName = getDocumentName();
                model.title = title;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = model.TransType;
                ViewBag.DocumentStatus = model.DocumentStatus;
                ViewBag.Command = model.Command;
                GetAssetGroupDetailView(model);
                GetAssetDescription(model, "A");
                GetSerialNo(model, "A", "0");

                model.Doc_no = urlData.Doc_No;
                model.Doc_date = urlData.Doc_date;

                if (urlData.Doc_No != null)
                {
                    DataSet ds = _AssetRetirement_ISERVICES.GetDepreciationProcessDetail(CompID, BrchID, model.Doc_no, model.Doc_date, UserID, model.DocumentMenuId);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            model.Doc_no = ds.Tables[0].Rows[0]["Doc_no"].ToString(); //int.Parse(ds.Tables[0].Rows[0]["Asset_grp_id"].ToString());
                            model.Doc_date = ds.Tables[0].Rows[0]["Doc_date"].ToString();
                            model.AssetsGroupId = ds.Tables[0].Rows[0]["asset_grp_id"].ToString();
                            model.AssetItemsId = ds.Tables[0].Rows[0]["asset_id"].ToString();
                            GetSerialNo(model, "A", model.AssetsGroupId);
                            model.SerialNumber = ds.Tables[0].Rows[0]["serial_no"].ToString();

                            model.AssetDesDetails = ds.Tables[0].Rows[0]["item_name"].ToString();
                            model.AssetRegId = ds.Tables[0].Rows[0]["ass_reg_id"].ToString();
                            model.SerialNumberDetails = ds.Tables[0].Rows[0]["serial_no"].ToString();
                            model.AssetLabel = ds.Tables[0].Rows[0]["asset_label"].ToString();
                            model.GroupDetails = ds.Tables[0].Rows[0]["asset_group_name"].ToString();
                            model.CategoryDetails = ds.Tables[0].Rows[0]["Category"].ToString();
                            model.ProcuredValue = ds.Tables[0].Rows[0]["proc_val"].ToString();
                            model.AssetLife = ds.Tables[0].Rows[0]["assetlife"].ToString();
                            model.AssetWorkingDate = ds.Tables[0].Rows[0]["asset_working_dt"].ToString();
                            model.DepreciationStartDate = ds.Tables[0].Rows[0]["DepStartDate"].ToString();
                            model.AccumulatedDepreciation = ds.Tables[0].Rows[0]["accumulated_dep"].ToString();
                            model.CurrentValue = ds.Tables[0].Rows[0]["curr_val"].ToString();
                            model.AsOn = ds.Tables[0].Rows[0]["ason_dt"].ToString();
                            model.AssignedRequirementArea = ds.Tables[0].Rows[0]["assign_req_area"].ToString();

                            model.ScrapValue = ds.Tables[0].Rows[0]["scrap_val"].ToString();
                            model.RetDate = ds.Tables[0].Rows[0]["retire_dt"].ToString();
                            model.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            model.StatusName = ds.Tables[0].Rows[0]["ARStatus"].ToString();
                            model.DocumentStatus = ds.Tables[0].Rows[0]["status"].ToString().Trim();
                            model.Create_by = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            model.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            model.Create_on = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            model.Approved_by = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                            model.Approved_on = ds.Tables[0].Rows[0]["app_on"].ToString();
                            model.TransType = "Update";

                            ViewBag.GLAccount = ds.Tables[1];
                            ViewBag.CostCenterData = ds.Tables[2];
                            model.VouGlDetails = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                            string doc_status = ds.Tables[0].Rows[0]["status"].ToString().Trim();
                            string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                            model.DocumentStatus = doc_status;
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (doc_status == "A" || doc_status == "C")
                                {
                                    model.GLVoucherType = ds.Tables[1].Rows[0]["vou_type"].ToString();
                                }
                                model.GLVoucherNo = ds.Tables[1].Rows[0]["vou_no"].ToString();
                                model.GLVoucherDt = ds.Tables[1].Rows[0]["vou_dt"].ToString();
                                ViewBag.GLVoucherNo = model.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
                            }
                            if (doc_status == "C")
                            {
                                //model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                //model.CancelFlag = true;
                                model.BtnName = "Refresh";
                            }
                            else
                            {
                                //model.CancelFlag = false;
                            }

                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                        }
                        else
                        {
                            // BindDDLOnPageLoad(model);
                        }
                    }
                    else
                    {
                        // BindDDLOnPageLoad(model);
                    }
                }
                else
                {
                    // BindDDLOnPageLoad(model);
                }
                model.title = title;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = model.TransType;
                ViewBag.DocumentStatus = model.DocumentStatus;
                ViewBag.Command = model.Command;
                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetRetirement/AssetRetirementDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetAssetDescription(AssetRetirement_Model _AssetRegistrationGroupDetail, string ShowFor)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _AssetRetirement_ISERVICES.GetAssetItem(CompID, BrchID, ShowFor);
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
        public ActionResult GetSerialNo(AssetRetirement_Model _AssetRegistrationGroupDetail, string ShowFor, string RegId)
        {
            CommonPageDetails();
            Dictionary<string, string> SerialNoList = new Dictionary<string, string>();
            try
            {
                SerialNoList = _AssetRetirement_ISERVICES.GetSerialNo(CompID, BrchID, _AssetRegistrationGroupDetail.AssetItemsId, ShowFor, RegId);
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
        public JsonResult GetSerialNoJS(AssetRetirement_Model _AssetTransfer_Model, string AssetDescriptionId, string GrpId)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet ds = _AssetRetirement_ISERVICES.GetSerialNoJs(CompID, BrchID, AssetDescriptionId, "A", GrpId);

                List<AssetSerialNoLists> Lists = new List<AssetSerialNoLists>();
                foreach (DataRow data in ds.Tables[0].Rows)
                {
                    AssetSerialNoLists _COADetail = new AssetSerialNoLists();
                    _COADetail.SerialNo_id = data.Table.Columns["serial_noid"].ToString();
                    _COADetail.SerialNo_name = data.Table.Columns["serial_no"].ToString();
                    Lists.Add(_COADetail);
                }
                _AssetTransfer_Model.AssetSerialNo = Lists;
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
        public JsonResult GetAssetDesc(string AssetGroupId)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet Details = _AssetRetirement_ISERVICES.GetAssetDescDetails(CompID, BrchID, AssetGroupId);
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
        [HttpPost]
        public JsonResult GetRetirmentData(AssetRetirement_Model _AssetTransfer_Model, string AssetDescriptionId, string SerialNo)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet ds = _AssetRetirement_ISERVICES.GetRetirmentData(CompID, BrchID, AssetDescriptionId, SerialNo);
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
        public ActionResult GetAssetGroupDetailView(AssetRetirement_Model _AssetRegistrationGroupDetail)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _AssetRetirement_ISERVICES.GetAssetGroup(CompID, "0");
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
        public ActionResult GetAssetGroupDetailListView(AssetRetirementList_Model _AssetRegistrationGroupDetail)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _AssetRetirement_ISERVICES.GetAssetGroup(CompID, "0");
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
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string Doc_No = null, string Doc_dt = null,
            string ListFilterData = null)
        {
            try
            {
                urlData.Command = Command;
                urlData.TransType = TransType;
                urlData.BtnName = BtnName;
                urlData.Doc_No = Doc_No;
                urlData.Doc_date = Doc_dt;
                urlData.ListFilterData = ListFilterData;
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
        public ActionResult AssetRetirementDetailsActionCommands(AssetRetirement_Model _model, string Command)
        {
            try
            {
                var commCont = new CommonController(_Common_IServices);
                UrlData urlData = new UrlData();
                if (_model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "AddNew":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            //if (!string.IsNullOrEmpty(_model.Inv_no))
                            //{
                            //    SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", "Financial Year not Exist", _model.Inv_no, _model.Inv_dt, _model.ListFilterData);
                            //    return RedirectToAction("AssetRetirementDetail", _model);
                            //}
                            //return RedirectToAction("AddAssetRetirementDetail", new { DocNo = _model.SPO_No, DocDate = _model.SPO_Date, ListFilterData = _model.ListFilterData, WF_status = _Model.WFStatus });
                            //else
                            //{
                            //    SetUrlData(urlData, "Refresh", "Refresh", "Refresh", "Financial Year not Exist", null, null, _model.ListFilterData);
                            //    return RedirectToAction("AssetRetirementDetail", _model);
                            //}
                        }
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, null, _model.ListFilterData);
                        return RedirectToAction("AssetRetirementDetail", urlData);
                    case "Save":
                        SaveAssetRetirementDetail(_model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Doc_no, _model.Doc_date, _model.ListFilterData);
                        return RedirectToAction("AssetRetirementDetail", urlData);
                    case "Approve":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        //string Invdt1 = _model.Inv_dt;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, Invdt1) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", _model.Message, _model.Inv_no, _model.Inv_dt, _model.ListFilterData);
                        //    return RedirectToAction("AssetRetirementDetail", urlData);
                        //}
                        ApproveDPDetails(_model, _model.Doc_no, _model.Doc_date, "", "", "", "", "", "", "", "", "");
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Doc_no, _model.Doc_date, _model.ListFilterData);
                        return RedirectToAction("AssetRetirementDetail", urlData);
                    case "Edit":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        GetCompDeatil();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _model.Doc_no, DocDate = _model.Doc_date, ListFilterData = _model.ListFilterData });
                        }
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string invdt = _model.Doc_date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _model.Doc_no, _model.Doc_date, _model.ListFilterData);
                            return RedirectToAction("AssetRetirementDetail", urlData);
                        }
                        /*End to chk Financial year exist or not*/

                        _model.TransType = "Update";
                        _model.BtnName = "BtnEdit";
                        _model.Message = null;
                        Command = _model.Command;
                        TempData["ModelData"] = _model;

                        SetUrlData(urlData, _model.Command, "Update", _model.BtnName, _model.Message, _model.Doc_no, _model.Doc_date, _model.ListFilterData);
                        return RedirectToAction("AssetRetirementDetail", urlData);
                    case "Print":
                    ///return GenratePdfFile(_model);
                    case "Delete":
                        DeleteAssRetDetail(_model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, null, _model.ListFilterData);
                        return RedirectToAction("AssetRetirementDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _model.ListFilterData);
                        return RedirectToAction("AssetRetirementDetail", urlData);
                    case "BacktoList":
                        TempData["ListFilterData"] = _model.ListFilterData;
                        SetUrlData(urlData, "", "", "", null, null, null, _model.ListFilterData);
                        return RedirectToAction("AssetRetirement");
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("AssetRetirementDetail", urlData);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ApproveDPDetails(AssetRetirement_Model _model, string Inv_No, string Inv_Date, string A_Status, string A_Level, string A_Remarks, string VoucherNarr, string FilterData, string docid, string WF_Status1, string Bp_Nurr, string Dn_Nurration)
        {
            try
            {
                UrlData urlData = new UrlData();
                GetCompDeatil();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Result = _AssetRetirement_ISERVICES.ApproveDPDetail(Inv_No, Inv_Date, DocumentMenuId, BrchID, CompID, UserID, mac_id, VoucherNarr);

                if (Result.Split(',')[1] == "A")
                {
                    _model.Message = _model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                //_model.Message = Result.Split(',')[1] == "A" ? "Approved" : "Error";
                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, Result.Split(',')[0], Result.Split(',')[4], FilterData);
                return RedirectToAction("AssetRetirementDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private void DeleteAssRetDetail(AssetRetirement_Model _model)
        {
            try
            {
                GetCompDeatil();
                string Result = _AssetRetirement_ISERVICES.DeleteAssRetDetail(CompID, BrchID, _model.Doc_no, _model.Doc_date);
                _model.Message = Result.Split(',')[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblDetail(string HeaderDetail, AssetRetirement_Model _DP_model)
        {
            try
            {
                CommonPageDetails();
                DataTable dtItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("TransType", typeof(string));
                dtItem.Columns.Add("MenuID", typeof(string));
                dtItem.Columns.Add("compid", typeof(string));
                dtItem.Columns.Add("brId", typeof(string));
                dtItem.Columns.Add("DocNo", typeof(string));
                dtItem.Columns.Add("DocDate", typeof(string));
                dtItem.Columns.Add("asset_grp_id", typeof(string));
                dtItem.Columns.Add("asset_description", typeof(string));
                dtItem.Columns.Add("serial_no", typeof(string));
                dtItem.Columns.Add("scrapVal", typeof(string));
                dtItem.Columns.Add("ret_date", typeof(string));
                dtItem.Columns.Add("Remarks", typeof(string));
                dtItem.Columns.Add("status", typeof(string));
                dtItem.Columns.Add("create_id", typeof(string));
                dtItem.Columns.Add("app_id", typeof(string));
                dtItem.Columns.Add("mod_id", typeof(string));
                dtItem.Columns.Add("mac_id", typeof(string));

                //dtItem.Columns.Add("DocMenuId", typeof(string));
                if (HeaderDetail != null)
                {
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    JArray jObject = JArray.Parse(HeaderDetail);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        if (_DP_model.Doc_no != null)
                        {
                            _DP_model.TransType = "Update";
                            dtrowLines["TransType"] = _DP_model.TransType;
                        }
                        else
                        {
                            _DP_model.TransType = "Save";
                            dtrowLines["TransType"] = _DP_model.TransType;
                        }
                        dtrowLines["MenuID"] = DocumentMenuId;
                        dtrowLines["compid"] = CompID;
                        dtrowLines["brId"] = BrchID;
                        dtrowLines["DocNo"] = jObject[i]["DocNo"]?.ToString();
                        dtrowLines["DocDate"] = jObject[i]["DocDate"]?.ToString();
                        dtrowLines["asset_grp_id"] = jObject[i]["asset_grp_id"]?.ToString();
                        dtrowLines["asset_description"] = jObject[i]["asset_description"]?.ToString();
                        dtrowLines["serial_no"] = jObject[i]["serial_no"]?.ToString();
                        dtrowLines["scrapVal"] = jObject[i]["scrapVal"]?.ToString();
                        dtrowLines["ret_date"] = jObject[i]["ret_date"]?.ToString();
                        dtrowLines["Remarks"] = jObject[i]["remarks"]?.ToString();
                        dtrowLines["status"] = jObject[i]["status"]?.ToString();
                        dtrowLines["create_id"] = UserID;
                        dtrowLines["app_id"] = UserID;
                        dtrowLines["mod_id"] = UserID;
                        dtrowLines["mac_id"] = mac_id;
                        dtItem.Rows.Add(dtrowLines);
                    }
                }
                dtItemDetail = dtItem;
                return dtItemDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblItemDetail(string ItemDetails)
        {
            try
            {
                DataTable dtItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("ass_reg_id", typeof(string));
                dtItem.Columns.Add("asset_id", typeof(string));
                dtItem.Columns.Add("asset_label", typeof(string));
                dtItem.Columns.Add("curr_val", typeof(string));
                dtItem.Columns.Add("dep_method", typeof(string));
                dtItem.Columns.Add("dep_val", typeof(string));
                dtItem.Columns.Add("add_dep_val", typeof(string));
                dtItem.Columns.Add("total_dep_val", typeof(string));
                dtItem.Columns.Add("revised_dep_val", typeof(string));
                dtItem.Columns.Add("dep_per", typeof(string));
                dtItem.Columns.Add("add_dep_per", typeof(string));
                dtItem.Columns.Add("serial_no", typeof(string));
                if (ItemDetails != null)
                {
                    JArray jObject = JArray.Parse(ItemDetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["ass_reg_id"] = jObject[i]["ass_reg_id"]?.ToString();
                        dtrowLines["asset_id"] = jObject[i]["AssetId"]?.ToString();
                        dtrowLines["asset_label"] = jObject[i]["AssetLabel"]?.ToString();
                        dtrowLines["curr_val"] = jObject[i]["CurrentValue"]?.ToString();
                        dtrowLines["dep_method"] = jObject[i]["DepreciationMethod"]?.ToString();
                        dtrowLines["dep_val"] = jObject[i]["DepreciationValue"]?.ToString();
                        dtrowLines["add_dep_val"] = jObject[i]["AdditionalDepreciationValue"]?.ToString();
                        dtrowLines["total_dep_val"] = jObject[i]["TotalDepreciationValue"]?.ToString();
                        dtrowLines["revised_dep_val"] = jObject[i]["RevisedAssetValue"]?.ToString();
                        dtrowLines["dep_per"] = jObject[i]["DepreciationPercentage"]?.ToString();
                        dtrowLines["add_dep_per"] = jObject[i]["AdditionalDepreciationPercentage"]?.ToString();
                        dtrowLines["serial_no"] = jObject[i]["SerialNumber"]?.ToString();
                        //dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["Uomid"]);
                        dtItem.Rows.Add(dtrowLines);
                    }
                }
                dtItemDetail = dtItem;
                return dtItemDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public DataTable dtVoudetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("comp_id", typeof(string));
            dtItem.Columns.Add("acc_id", typeof(string));
            dtItem.Columns.Add("acc_name", typeof(string));
            dtItem.Columns.Add("type", typeof(string));
            dtItem.Columns.Add("doctype", typeof(string));
            dtItem.Columns.Add("Value", typeof(string));
            dtItem.Columns.Add("dr_amt_sp", typeof(string));
            dtItem.Columns.Add("cr_amt_sp", typeof(string));
            dtItem.Columns.Add("TransType", typeof(string));
            dtItem.Columns.Add("Gltype", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowVoudetails = dtItem.NewRow();
                dtrowVoudetails["comp_id"] = jObject[i]["comp_id"].ToString();
                dtrowVoudetails["acc_id"] = jObject[i]["id"].ToString();
                dtrowVoudetails["acc_name"] = jObject[i]["acc_name"].ToString();
                dtrowVoudetails["type"] = jObject[i]["type"].ToString();
                dtrowVoudetails["doctype"] = jObject[i]["doctype"].ToString();
                dtrowVoudetails["Value"] = jObject[i]["Value"].ToString();
                dtrowVoudetails["dr_amt_sp"] = jObject[i]["DrAmt"].ToString();
                dtrowVoudetails["cr_amt_sp"] = jObject[i]["CrAmt"].ToString();
                dtrowVoudetails["TransType"] = jObject[i]["TransType"].ToString();
                dtrowVoudetails["Gltype"] = jObject[i]["Gltype"].ToString();
                dtItem.Rows.Add(dtrowVoudetails);
            }
            return dtItem;
        }
        private DataTable ToDtblvouDetail(string vouDetail)
        {
            try
            {
                DataTable vou_Details = new DataTable();
                vou_Details.Columns.Add("comp_id", typeof(string));
                vou_Details.Columns.Add("vou_sr_no", typeof(string));
                vou_Details.Columns.Add("gl_sr_no", typeof(string));
                vou_Details.Columns.Add("id", typeof(string));
                vou_Details.Columns.Add("type", typeof(string));
                vou_Details.Columns.Add("doctype", typeof(string));
                vou_Details.Columns.Add("Value", typeof(string));
                vou_Details.Columns.Add("ValueInBase", typeof(string));
                vou_Details.Columns.Add("DrAmt", typeof(string));
                vou_Details.Columns.Add("CrAmt", typeof(string));
                vou_Details.Columns.Add("TransType", typeof(string));
                vou_Details.Columns.Add("Gltype", typeof(string));
                vou_Details.Columns.Add("parent", typeof(string));
                vou_Details.Columns.Add("DrAmtInBase", typeof(string));
                vou_Details.Columns.Add("CrAmtInBase", typeof(string));
                vou_Details.Columns.Add("curr_id", typeof(string));
                vou_Details.Columns.Add("conv_rate", typeof(string));
                vou_Details.Columns.Add("vou_type", typeof(string));
                vou_Details.Columns.Add("bill_no", typeof(string));
                vou_Details.Columns.Add("bill_date", typeof(string));
                vou_Details.Columns.Add("gl_narr", typeof(string));
                if (vouDetail != null)
                {
                    JArray jObjectVOU = JArray.Parse(vouDetail);
                    for (int i = 0; i < jObjectVOU.Count; i++)
                    {
                        DataRow dtrowVouDetailsLines = vou_Details.NewRow();
                        dtrowVouDetailsLines["comp_id"] = jObjectVOU[i]["comp_id"].ToString();
                        dtrowVouDetailsLines["vou_sr_no"] = jObjectVOU[i]["VouSrNo"].ToString();
                        dtrowVouDetailsLines["gl_sr_no"] = jObjectVOU[i]["GlSrNo"].ToString();
                        dtrowVouDetailsLines["id"] = jObjectVOU[i]["id"].ToString();
                        dtrowVouDetailsLines["type"] = jObjectVOU[i]["type"].ToString();
                        dtrowVouDetailsLines["doctype"] = jObjectVOU[i]["doctype"].ToString();
                        dtrowVouDetailsLines["Value"] = jObjectVOU[i]["Value"].ToString();
                        dtrowVouDetailsLines["ValueInBase"] = jObjectVOU[i]["ValueInBase"].ToString();
                        dtrowVouDetailsLines["DrAmt"] = jObjectVOU[i]["DrAmt"].ToString();
                        dtrowVouDetailsLines["CrAmt"] = jObjectVOU[i]["CrAmt"].ToString();
                        dtrowVouDetailsLines["TransType"] = jObjectVOU[i]["TransType"].ToString();
                        dtrowVouDetailsLines["Gltype"] = jObjectVOU[i]["Gltype"].ToString();
                        dtrowVouDetailsLines["parent"] = "0";// jObjectVOU[i]["Gltype"].ToString();
                        dtrowVouDetailsLines["DrAmtInBase"] = jObjectVOU[i]["DrAmtInBase"].ToString();
                        dtrowVouDetailsLines["CrAmtInBase"] = jObjectVOU[i]["CrAmtInBase"].ToString();
                        dtrowVouDetailsLines["curr_id"] = jObjectVOU[i]["curr_id"].ToString();
                        dtrowVouDetailsLines["conv_rate"] = jObjectVOU[i]["conv_rate"].ToString();
                        dtrowVouDetailsLines["vou_type"] = jObjectVOU[i]["vou_type"].ToString();
                        dtrowVouDetailsLines["bill_no"] = jObjectVOU[i]["bill_no"].ToString();
                        dtrowVouDetailsLines["bill_date"] = jObjectVOU[i]["bill_date"].ToString();
                        dtrowVouDetailsLines["gl_narr"] = jObjectVOU[i]["gl_narr"].ToString();
                        vou_Details.Rows.Add(dtrowVouDetailsLines);
                    }
                    ViewData["VouDetail"] = dtVoudetail(jObjectVOU);
                }
                return vou_Details;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public DataTable dtCCdetail(JArray JAObj)
        {
            DataTable CC_Details = new DataTable();

            CC_Details.Columns.Add("acc_id", typeof(string));
            CC_Details.Columns.Add("cc_id", typeof(int));
            CC_Details.Columns.Add("cc_val_id", typeof(int));
            CC_Details.Columns.Add("cc_amt", typeof(string));
            CC_Details.Columns.Add("cc_name", typeof(string));
            CC_Details.Columns.Add("cc_val_name", typeof(string));
            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = CC_Details.NewRow();

                dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                dtrowLines["cc_name"] = JAObj[i]["ddl_CC_Name"].ToString();
                dtrowLines["cc_val_name"] = JAObj[i]["ddl_CC_Type"].ToString();

                CC_Details.Rows.Add(dtrowLines);
            }
            return CC_Details;
        }
        private DataTable ToDtblVouGlDetail1(string VouGlDetails)
        {
            try
            {
                DataTable DtblVouGlDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("id", typeof(double));
                dtItem.Columns.Add("type", typeof(int));
                dtItem.Columns.Add("doctype", typeof(string));
                dtItem.Columns.Add("Value", typeof(int));
                dtItem.Columns.Add("DrAmt", typeof(string));
                dtItem.Columns.Add("CrAmt", typeof(string));
                dtItem.Columns.Add("TransType", typeof(string));
                dtItem.Columns.Add("Gltype", typeof(string));

                JArray jObject = JArray.Parse(VouGlDetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["comp_id"] = jObject[i]["comp_id"].ToString();
                    dtrowLines["id"] = jObject[i]["id"].ToString();
                    dtrowLines["type"] = 'I';
                    dtrowLines["doctype"] = jObject[i]["doctype"].ToString();
                    dtrowLines["Value"] = jObject[i]["Value"].ToString();
                    dtrowLines["DrAmt"] = jObject[i]["DrAmt"].ToString();
                    dtrowLines["CrAmt"] = jObject[i]["CrAmt"].ToString();
                    dtrowLines["TransType"] = jObject[i]["TransType"].ToString();
                    dtrowLines["Gltype"] = jObject[i]["Gltype"].ToString();

                    dtItem.Rows.Add(dtrowLines);
                }
                DtblVouGlDetail = dtItem;
                ViewData["VouDetails"] = dtVoudetail(jObject);
                return DtblVouGlDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblccDetail(string CC_DetailList)
        {
            try
            {
                DataTable CC_Details = new DataTable();
                CC_Details.Columns.Add("vou_sr_no", typeof(string));
                CC_Details.Columns.Add("gl_sr_no", typeof(string));
                CC_Details.Columns.Add("acc_id", typeof(string));
                CC_Details.Columns.Add("cc_id", typeof(int));
                CC_Details.Columns.Add("cc_val_id", typeof(int));
                CC_Details.Columns.Add("cc_amt", typeof(string));

                if (CC_DetailList != null)
                {
                    JArray JAObj = JArray.Parse(CC_DetailList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = CC_Details.NewRow();
                        dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                        dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();
                        dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                        dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                        dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                        dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                        CC_Details.Rows.Add(dtrowLines);
                    }
                    ViewData["CCdetail"] = dtCCdetail(JAObj);
                }
                return CC_Details;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult SaveAssetRetirementDetail(AssetRetirement_Model ObjAddItemGroupSetupBOL)
        {
            try
            {

                // ObjAddItemGroupSetupBOL.title = title;
                string PageName = ObjAddItemGroupSetupBOL.title.Replace(" ", "");
                if (ObjAddItemGroupSetupBOL.TransType == "Update")
                {
                    ObjAddItemGroupSetupBOL.FormMode = "1";
                }
                if (Session["CompId"] != null)
                {

                    if (ObjAddItemGroupSetupBOL.FormMode == "1")
                    {
                        ObjAddItemGroupSetupBOL.Create_id = UserID;
                    }
                }
                DataTable DtblHeaderDetail = new DataTable();
                DataTable DtblVouGLDetail = new DataTable();
                DataTable CostCenterDetails = new DataTable();

                DtblHeaderDetail = ToDtblDetail(ObjAddItemGroupSetupBOL.Headerdetails, ObjAddItemGroupSetupBOL);

                ObjAddItemGroupSetupBOL.title = title;

                DtblVouGLDetail = ToDtblvouDetail(ObjAddItemGroupSetupBOL.VouGlDetails);
                CostCenterDetails = ToDtblccDetail(ObjAddItemGroupSetupBOL.CC_DetailList);

                if (ObjAddItemGroupSetupBOL.AssetsGroupId.ToString() != "0")
                {
                    string SaveMessage = _AssetRetirement_ISERVICES.InsertARDetail(ObjAddItemGroupSetupBOL, DtblHeaderDetail, DtblVouGLDetail, CostCenterDetails);

                    if (SaveMessage == "DocModify")
                    {
                        ObjAddItemGroupSetupBOL.Message = "DocModify";
                        ObjAddItemGroupSetupBOL.BtnName = "Refresh";
                        ObjAddItemGroupSetupBOL.Command = "Refresh";
                    }
                    else if (SaveMessage == "Cancelled")
                    {
                        try
                        {
                            //string fileName = ObjAddItemGroupSetupBOL.Doc_no.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            //var filePath = SavePdfDocToSendOnEmailAlert(ObjAddItemGroupSetupBOL.Doc_no, ObjAddItemGroupSetupBOL.Doc_date, fileName, DocumentMenuId, "C");
                            //_Common_IServices.SendAlertEmail(CompID, BrchID, "105106105", ObjAddItemGroupSetupBOL.Doc_no, "Cancel", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            ObjAddItemGroupSetupBOL.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        ObjAddItemGroupSetupBOL.Message = ObjAddItemGroupSetupBOL.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        //_model.Message = "Cancelled";
                        ObjAddItemGroupSetupBOL.Command = "Update";
                        ObjAddItemGroupSetupBOL.DocumentStatus = "D";
                        ObjAddItemGroupSetupBOL.BtnName = "Refresh";
                        ObjAddItemGroupSetupBOL.TransType = "Update";
                        //return RedirectToAction("AssetRetirementAdd");
                    }
                    else
                    {
                        string[] FDetail = SaveMessage.Split(',');
                        string Message = FDetail[3].ToString();
                        string Inv_no = FDetail[0].ToString();
                        string Inv_DATE = FDetail[4].ToString();
                        string Cancel = FDetail[1].ToString();
                        if (Message == "DataNotFound")
                        {
                            var msg = "Data Not found" + " " + Inv_DATE + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            ObjAddItemGroupSetupBOL.Message = Message;
                        }

                        if (Cancel == "C" && Message == "Update")
                        {
                            ObjAddItemGroupSetupBOL.Message = "Cancelled";
                            ObjAddItemGroupSetupBOL.Command = "Update";
                            ObjAddItemGroupSetupBOL.Doc_no = Inv_no;
                            ObjAddItemGroupSetupBOL.Doc_date = Inv_DATE;
                            ObjAddItemGroupSetupBOL.DocumentStatus = "D";
                            ObjAddItemGroupSetupBOL.BtnName = "Refresh";
                            ObjAddItemGroupSetupBOL.TransType = "Update";
                        }
                        else
                        {
                            if (Message == "Update" || Message == "Save")
                            {
                                ObjAddItemGroupSetupBOL.Message = "Save";
                                ObjAddItemGroupSetupBOL.Command = "Update";
                                ObjAddItemGroupSetupBOL.Doc_no = Inv_no;
                                ObjAddItemGroupSetupBOL.Doc_date = Inv_DATE;
                                ObjAddItemGroupSetupBOL.DocumentStatus = "D";
                                ObjAddItemGroupSetupBOL.BtnName = "BtnSave";
                                ObjAddItemGroupSetupBOL.TransType = "Update";
                            }
                        }
                    }
                }
                TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                return RedirectToAction("AddAssetRetirementDetail", "AssetRetirement");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            var WF_status1 = "";
            //Session["Message"] = "";
            AssetRetirement_Model _Model = new AssetRetirement_Model();
            var a = TrancType.Split(',');
            _Model.Doc_no = a[0].Trim();
            _Model.Doc_date = a[1].Trim();
            _Model.TransType = a[2].Trim();
            if (a[3].Trim() != "" && a[3].Trim() != null)
            {
                WF_status1 = a[3].Trim();
                //_Model.WFStatus = WF_status1;
            }
            var docId = a[4].Trim();
            _Model.Message = Mailerror;
            _Model.BtnName = "BtnToDetailPage";
            _Model.DocumentMenuId = docId;
            TempData["ModelData"] = _Model;
            TempData["WF_status1"] = WF_status1; ;
            TempData["ListFilterData"] = ListFilterData1;
            UrlData URLModel = new UrlData();
            URLModel.Doc_No = a[0].Trim();
            URLModel.Doc_date = a[1].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.ListFilterData = ListFilterData1;
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnToDetailPage";
            TempData["UrlData"] = URLModel;
            TempData["Message"] = _Model.Message;
            return RedirectToAction("AssetRetirementDetail", URLModel);
        }


        [HttpPost]
        public JsonResult GetGLDetails(List<GL_Detail> GLDetail)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                DataTable DtblGLDetail = new DataTable();

                if (GLDetail != null)
                {
                    DtblGLDetail = ToDataTable(GLDetail);
                }
                DataSet GlDt = _AssetRetirement_ISERVICES.GetAllGLDetails(DtblGLDetail);
                Validate = Json(GlDt);
                JsonResult DataRows = null;
                DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }

            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Validate;
        }

        [HttpPost]
        public JsonResult GetTaxRecivable(List<GL_Detail> GLDetail)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                AssetRetirement_Model _Model = new AssetRetirement_Model();
                string Comp_ID = string.Empty;
                string AssetGroupId = GLDetail.First().id;// string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet GlDt = _AssetRetirement_ISERVICES.GetTaxRecivableAcc(Comp_ID, AssetGroupId);
                Validate = Json(GlDt);
                JsonResult DataRows = null;
                DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);

                return DataRows;
            }

            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            //return Validate;
        }
        public DataTable ToDataTable<T>(IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                object[] values = new object[props.Count];
                using (DataTable table = new DataTable())
                {
                    long _pCt = props.Count;
                    for (int i = 0; i < _pCt; ++i)
                    {
                        PropertyDescriptor prop = props[i];
                        table.Columns.Add(prop.Name, prop.PropertyType);
                    }
                    foreach (T item in data)
                    {
                        long _vCt = values.Length;
                        for (int i = 0; i < _vCt; ++i)
                        {
                            values[i] = props[i].GetValue(item);
                        }
                        table.Rows.Add(values);
                    }
                    return table;
                }
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
                ds = _AssetRetirement_ISERVICES.GetAssetProcurmentDetail(CompID, BrchID, RegId);
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
        public ActionResult GetAssetRegHistory(string RegId)
        {
            try
            {
                ViewBag.DepreciationHistory = GetRegistrationHistory(RegId);
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
                ds = _AssetRetirement_ISERVICES.GetRegistrationHistory(CompID, BrchID, RegId);
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
    }
}