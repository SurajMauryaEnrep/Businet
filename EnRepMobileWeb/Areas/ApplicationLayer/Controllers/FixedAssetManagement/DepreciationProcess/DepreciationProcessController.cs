using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.DepreciationProcess;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.DepreciationProcess;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FixedAssetManagement.DepreciationProcess
{
    public class DepreciationProcessController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105106105", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        DepreciationProcess_ISERVICES _DepreciationProcess_ISERVICES;
        public DepreciationProcessController(Common_IServices _Common_IServices, DepreciationProcess_ISERVICES _DepreciationProcess_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._DepreciationProcess_ISERVICES = _DepreciationProcess_ISERVICES;
        }
        // GET: ApplicationLayer/DepreciationProcess
        public ActionResult DepreciationProcess(string wfStatus)
        {
            ViewBag.MenuPageName = getDocumentName();
            GetCompDeatil();
            CommonPageDetails();
            DepreciationProcessList_Model _model = new DepreciationProcessList_Model();
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
                    _model.DP_status = Status;
                }
            }
            _model.title = title;
            _model.PQASearch = "0";
            GetAssetGroupDetailListView(_model);
            GetAllListData(_model);
            return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/DepreciationProcess/DepreciationProcessList.cshtml", _model);
        }
        public ActionResult DPListSearch(string Group, string Status)
        {
            try
            {
                CommonPageDetails();
                DataSet ds = new DataSet();
                ds = _DepreciationProcess_ISERVICES.GetAllData(CompID, BrchID, Group, Status, "", UserID);
                ViewBag.ListDetail = ds.Tables[0];
                ViewBag.ListSearch = "ListSearch";
                ViewBag.ListFilterData1 = Group + "," + Status;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDepreciationProcessList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private void GetAllListData(DepreciationProcessList_Model _List_Model)
        {
            try
            {
                string SupplierName = string.Empty;
                CommonPageDetails();
                DataSet CustList = _DepreciationProcess_ISERVICES.GetAllData(CompID, BrchID, _List_Model.AssetsGroupId, _List_Model.DP_status, _List_Model.WF_status, UserID);
                ViewBag.ListDetail = CustList.Tables[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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

        public ActionResult AddDepreciationProcessDetail(string DocNo, string DocDt, string ListFilterData)
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
                        return RedirectToAction("DepreciationProcessDetail", "DepreciationProcess", urlData);
                    }
                }
                /*End to chk Financial year exist or not*/
                string BtnName = DocNo == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = DocNo == null ? "Save" : "Update";
                SetUrlData(urlData, "Add", TransType, BtnName, null, DocNo, DocDt, ListFilterData);
                return RedirectToAction("DepreciationProcessDetail", "DepreciationProcess", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
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
        public ActionResult DepreciationProcessDetail(UrlData urlData)
        {
            try
            {
                DepreciationProcess_Model model = new DepreciationProcess_Model();
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
                //GetAssetDescription(model);
                GetAssetGroupDetailView(model);

                model.Doc_no = urlData.Doc_No;
                model.Doc_date = urlData.Doc_date;

                if (urlData.Doc_No != null)
                {
                    DataSet ds = _DepreciationProcess_ISERVICES.GetDepreciationProcessDetail(CompID, BrchID, model.Doc_no, model.Doc_date, UserID, model.DocumentMenuId);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {


                            model.Doc_no = ds.Tables[0].Rows[0]["Doc_no"].ToString(); //int.Parse(ds.Tables[0].Rows[0]["Asset_grp_id"].ToString());
                            model.Doc_date = ds.Tables[0].Rows[0]["Doc_date"].ToString();
                            model.AssetsGroupId = ds.Tables[0].Rows[0]["asset_grp_id"].ToString();
                            model.Depreciationfreq = ds.Tables[0].Rows[0]["dep_freq"].ToString();
                            model.DepreciationfreqId = ds.Tables[0].Rows[0]["dep_freq1"].ToString();
                            BindDDLOnPageLoad(model);

                            model.ddl_financial_year = ds.Tables[0].Rows[0]["f_fy"].ToString();
                            BindPeriodList(model, model.DepreciationfreqId, model.ddl_financial_year, model.AssetsGroupId);
                            model.ddl_periodList.Add(new period
                            {
                                id = ds.Tables[0].Rows[0]["f_period1"].ToString(), // ID
                                name = ds.Tables[0].Rows[0]["f_period"].ToString()  // Name
                            });
                            model.ddl_period = Convert.ToString(ds.Tables[0].Rows[0]["f_period1"]);
                            model.txtFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                            model.txtToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                            model.StatusName = ds.Tables[0].Rows[0]["DPStatus"].ToString();
                            model.DocumentStatus = ds.Tables[0].Rows[0]["dp_status"].ToString().Trim();
                            model.Create_by = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            model.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            model.Create_on = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            model.Amended_by = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            model.Amended_on = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                            model.Approved_by = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                            model.Approved_on = ds.Tables[0].Rows[0]["app_on"].ToString();
                            model.TransType = "Update";
                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ItemDetailsListTotal = ds.Tables[2];
                            ViewBag.AttechmentDetails = ds.Tables[3];
                            ViewBag.GLAccount = ds.Tables[7];
                            ViewBag.CostCenterData = ds.Tables[8];
                            model.VouGlDetails = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                            string doc_status = ds.Tables[0].Rows[0]["dp_status"].ToString().Trim();
                            string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                            model.DocumentStatus = doc_status;
                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                if (doc_status == "A" || doc_status == "C")
                                {
                                    model.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                                }
                                model.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                                model.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                                ViewBag.GLVoucherNo = model.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
                            }
                            if (doc_status == "C")
                            {
                                model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                model.CancelFlag = true;
                                model.BtnName = "Refresh";
                            }
                            else
                            {
                                model.CancelFlag = false;
                            }
                            model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                            if (doc_status != "D" && doc_status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[6];
                            }
                            if (ViewBag.AppLevel != null && model.Command != "Edit")
                            {
                                var sent_to = "";
                                var nextLevel = "";
                                if (ds.Tables[4].Rows.Count > 0)
                                {
                                    sent_to = ds.Tables[4].Rows[0]["sent_to"].ToString();
                                }
                                if (ds.Tables[5].Rows.Count > 0)
                                {
                                    nextLevel = ds.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                                }
                                if (doc_status == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        model.BtnName = "Refresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                            }
                                            model.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            model.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        model.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            model.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (doc_status == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        model.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                        }
                                        model.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (doc_status == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        model.BtnName = "Refresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                        }
                        else
                        {
                            BindDDLOnPageLoad(model);
                        }
                    }
                    else
                    {
                        BindDDLOnPageLoad(model);
                    }
                }
                else
                {
                    BindDDLOnPageLoad(model);
                }
                model.title = title;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = model.TransType;
                ViewBag.DocumentStatus = model.DocumentStatus;
                ViewBag.Command = model.Command;
                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/DepreciationProcess/DepreciationProcessDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BindDDLOnPageLoad(DepreciationProcess_Model _DepreciationProcessModel)
        {
            try
            {
                GetCompDeatil();
                string f_freq = ""; string StartDate = "";
                DataSet ds = _DepreciationProcess_ISERVICES.BindFinancialYear(Convert.ToInt32(CompID), Convert.ToInt32(BrchID), _DepreciationProcessModel.DepreciationfreqId, StartDate, "", _DepreciationProcessModel.AssetsGroupId);
                List<financial_year> fyList = new List<financial_year>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    financial_year fyObj1 = new financial_year();
                    fyObj1.id = "0";
                    fyObj1.name = "---Select---";
                    fyList.Add(fyObj1);
                    foreach (DataRow data in ds.Tables[0].Rows)
                    {
                        financial_year fyObj = new financial_year();
                        fyObj.id = data["id"].ToString();
                        fyObj.name = data["name"].ToString();
                        fyList.Add(fyObj);
                    }
                }
                _DepreciationProcessModel.ddl_financial_yearList = fyList;

                //List<period> plist = new List<period>();
                //period pObj = new period();
                //pObj.id = "0";
                //pObj.name = "---Select---";
                //plist.Add(pObj);
                //_DepreciationProcessModel.ddl_periodList = plist;
                //if (ds.Tables[1].Rows.Count == 0)
                //{
                //    _DepreciationProcessModel.ddl_f_frequency = "0";
                //}
                //_DepreciationProcessModel.ddl_f_frequency = Convert.ToString(ds.Tables[1].Rows[0]["param_stat"]);
                //_DepreciationProcessModel.ddl_f_frequency = Convert.ToString(ds.Tables[1].Rows[0]["name"]);
                period pObj = new period();
                List<period> PList = new List<period>();
                pObj.id = "0";
                pObj.name = "---Select---";
                PList.Add(pObj);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow data in ds.Tables[1].Rows)
                        {
                            period fyObjPeriod = new period();
                            fyObjPeriod.id = data["id"].ToString();
                            fyObjPeriod.name = data["name"].ToString();
                            PList.Add(fyObjPeriod);
                        }
                    }
                }
                _DepreciationProcessModel.ddl_periodList = PList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetAssetGroupDetailView(DepreciationProcess_Model _AssetRegistrationGroupDetail)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _DepreciationProcess_ISERVICES.GetAssetGroup(CompID, "0");
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
        public ActionResult GetAssetGroupDetailListView(DepreciationProcessList_Model _AssetRegistrationGroupDetail)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _DepreciationProcess_ISERVICES.GetAssetGroup(CompID, "0");
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
        public JsonResult GetAssetCategoryDetails(string AssetGroupId)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DepreciationProcess_Model model = new DepreciationProcess_Model();
                DataSet Details = _DepreciationProcess_ISERVICES.GetAssetCategoryDetails(CompID, BrchID, AssetGroupId);
                List<period> ddl_periodList = new List<period>();
                ddl_periodList.Add(new period { id = model.ddl_period, name = model.ddl_period });
                model.ddl_periodList = ddl_periodList;

                //List<RFQList> BillNoLists = new List<RFQList>();
                //BillNoLists.Add(new RFQList { RFQ_id = model.rfqID, RFQ_value = model.rfqID });
                //model.rfqLists = BillNoLists;
                if (Details.Tables.Count > 0)
                {
                    model.DepreciationfreqId = Details.Tables[0].Rows[0]["dep_freq"].ToString();
                    BindDDLOnPageLoad(model);
                    DataRows = Json(JsonConvert.SerializeObject(Details));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult GetAssetRegGroupDetail(string AssetGroup, string fin_yr, string Period)
        {
            DataTable dt = new DataTable();
            try
            {
                DPtemDetail model = new DPtemDetail();
                // DepreciationProcess_Model model1 = new DepreciationProcess_Model();
                GetCompDeatil();
                if (AssetGroup != null)
                {
                    DataSet Details = _DepreciationProcess_ISERVICES.GetAssetRegGroupDetail(CompID, BrchID, AssetGroup, fin_yr, Period);
                    ViewBag.ItemDetailsList = Details.Tables[0];
                    ViewBag.ItemDetailsListTotal = Details.Tables[1];
                    ViewBag.TransType = "Save";
                    ViewBag.DocumentStatus = "";
                    ViewBag.Command = "Add";
                }
                return View("~/Areas/ApplicationLayer/Views/Shared/PartialDepreciationProcessItemDetails.cshtml", model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepreciationProcessDetailsActionCommands(DepreciationProcess_Model _model, string Command)
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
                            //    return RedirectToAction("DepreciationProcessDetail", _model);
                            //}
                            //return RedirectToAction("AddDepreciationProcessDetail", new { DocNo = _model.SPO_No, DocDate = _model.SPO_Date, ListFilterData = _model.ListFilterData, WF_status = _Model.WFStatus });
                            //else
                            //{
                            //    SetUrlData(urlData, "Refresh", "Refresh", "Refresh", "Financial Year not Exist", null, null, _model.ListFilterData);
                            //    return RedirectToAction("DepreciationProcessDetail", _model);
                            //}
                        }
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, null, _model.ListFilterData);
                        return RedirectToAction("DepreciationProcessDetail", urlData);
                    case "Save":
                        SaveDepreciationProcessDetail(_model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Doc_no, _model.Doc_date, _model.ListFilterData);
                        return RedirectToAction("DepreciationProcessDetail", urlData);
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
                        //    return RedirectToAction("DepreciationProcessDetail", urlData);
                        //}
                        ApproveDPDetails(_model, _model.Doc_no, _model.Doc_date, "", "", "", "", "", "", "", "", "");
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Doc_no, _model.Doc_date, _model.ListFilterData);
                        return RedirectToAction("DepreciationProcessDetail", urlData);
                    case "Edit":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        GetCompDeatil();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _model.Doc_no, DocDate = _model.Doc_date, ListFilterData = _model.ListFilterData, WF_status = _model.WFStatus });
                        }
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string invdt = _model.Doc_date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _model.Doc_no, _model.Doc_date, _model.ListFilterData);
                            return RedirectToAction("DepreciationProcessDetail", urlData);
                        }
                        /*End to chk Financial year exist or not*/
                        if (_model.DocumentStatus == "O")
                        {
                            //string checkforCancle = CheckPQAForCancellation(_model.Doc_no, _model.Doc_date);
                            //if (checkforCancle != "")
                            //{
                            //    //Session["Message"] = checkforCancle;
                            //    _model.Message = checkforCancle;
                            //    _model.BtnName = "BtnToDetailPage";
                            //    TempData["ModelData"] = _model;
                            //    _model.Command = "Add";
                            //    _model.TransType = "Update";
                            //    //TempData["FilterData"] = _model.FilterData1;
                            //}
                            //else
                            //{
                            //    _model.TransType = "Update";
                            //    //_model.Command = command;
                            //    _model.BtnName = "BtnEdit";
                            //    _model.Message = null;
                            //    TempData["ModelData"] = _model;
                            //    //TempData["FilterData"] = _model.FilterData1;
                            //}
                        }
                        else
                        {
                            _model.TransType = "Update";
                            _model.BtnName = "BtnEdit";
                            _model.Message = null;
                            Command = _model.Command;
                            TempData["ModelData"] = _model;
                            // TempData["FilterData"] = _model.FilterData1;
                        }
                        SetUrlData(urlData, _model.Command, "Update", _model.BtnName, _model.Message, _model.Doc_no, _model.Doc_date, _model.ListFilterData);
                        return RedirectToAction("DepreciationProcessDetail", urlData);
                    case "Print":
                    ///return GenratePdfFile(_model);
                    case "Delete":
                        DeleteDPDetail(_model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, null, _model.ListFilterData);
                        return RedirectToAction("DepreciationProcessDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _model.ListFilterData);
                        return RedirectToAction("DepreciationProcessDetail", urlData);
                    case "BacktoList":
                        TempData["WF_status"] = _model.WFStatus;
                        TempData["ListFilterData"] = _model.ListFilterData;
                        SetUrlData(urlData, "", "", "", null, null, null, _model.ListFilterData);
                        return RedirectToAction("DepreciationProcess");
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("DepreciationProcessDetail", urlData);
                }
                //return RedirectToAction("PurchaseQuotationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ApproveDPDetails(DepreciationProcess_Model _model, string Inv_No, string Inv_Date, string A_Status, string A_Level, string A_Remarks, string VoucherNarr, string FilterData, string docid, string WF_Status1, string Bp_Nurr, string Dn_Nurration)
        {
            try
            {
                UrlData urlData = new UrlData();
                GetCompDeatil();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
               // string Dn_Nurr = ""; _model.DN_Nurration == null ? Dn_Nurration : _model.DN_Nurration;
                string Result = _DepreciationProcess_ISERVICES.ApproveDPDetail(Inv_No, Inv_Date, DocumentMenuId, BrchID, CompID, UserID, mac_id, A_Status, A_Level, A_Remarks, VoucherNarr);
                //ViewBag.PrintFormat = PrintFormatDataTable(_Model);
                //try
                //{
                //    //string fileName = "DPI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                //    string fileName = Inv_No.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                //    var filePath = SavePdfDocToSendOnEmailAlert(Inv_No, Inv_Date, fileName, DocumentMenuId, "AP");
                //    _Common_IServices.SendAlertEmail(CompID, BrchID, "105101154", Inv_No, "AP", UserID, "", filePath);
                //}
                //catch (Exception exMail)
                //{
                //    _model.Message = "ErrorInMail";
                //    string path = Server.MapPath("~");
                //    Errorlog.LogError(path, exMail);
                //}
                if (Result.Split(',')[1] == "A")
                {
                    _model.Message = _model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                //_model.Message = Result.Split(',')[1] == "A" ? "Approved" : "Error";
                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, Result.Split(',')[0], Result.Split(',')[7], FilterData);
                return RedirectToAction("DepreciationProcessDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private void DeleteDPDetail(DepreciationProcess_Model _model)
        {
            try
            {
                GetCompDeatil();
                string Result = _DepreciationProcess_ISERVICES.DeleteDPetails(CompID, BrchID, _model.Doc_no, _model.Doc_date);
                _model.Message = Result.Split(',')[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblDetail(string HeaderDetail, DepreciationProcess_Model _DP_model)
        {
            try
            {
                CommonPageDetails();
                DataTable dtItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("TransType", typeof(string));
                dtItem.Columns.Add("MenuID", typeof(string));
                dtItem.Columns.Add("Cancelled", typeof(string));
                dtItem.Columns.Add("Cancel_remarks", typeof(string));
                dtItem.Columns.Add("compid", typeof(string));
                dtItem.Columns.Add("brId", typeof(string));
                dtItem.Columns.Add("DocNo", typeof(string));
                dtItem.Columns.Add("DocDate", typeof(string));
                dtItem.Columns.Add("asset_grp_id", typeof(string));
                dtItem.Columns.Add("f_fy", typeof(string));
                dtItem.Columns.Add("f_period", typeof(string));
                dtItem.Columns.Add("from_date", typeof(string));
                dtItem.Columns.Add("to_date", typeof(string));
                dtItem.Columns.Add("dp_status", typeof(string));
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
                        string cancelflag = _DP_model.CancelFlag.ToString();
                        if (cancelflag == "False")
                            dtrowLines["Cancelled"] = "N";
                        else
                            dtrowLines["Cancelled"] = "Y";
                        dtrowLines["compid"] = CompID;
                        dtrowLines["brId"] = BrchID;
                        dtrowLines["DocNo"] = jObject[i]["DocNo"]?.ToString();
                        dtrowLines["DocDate"] = jObject[i]["DocDate"]?.ToString();
                        dtrowLines["asset_grp_id"] = jObject[i]["asset_grp_id"]?.ToString();
                        dtrowLines["f_fy"] = jObject[i]["f_fy"]?.ToString();
                        dtrowLines["f_period"] = jObject[i]["f_period"]?.ToString();
                        string dateFrom = _DP_model.txtFromDate;
                        DateTime dateFrom1 = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        _DP_model.txtFromDate = dateFrom1.ToString("yyyy-MM-dd");
                        dtrowLines["from_date"] = _DP_model.txtFromDate;
                        string dateTo = _DP_model.txtToDate;
                        DateTime dateTo1 = DateTime.ParseExact(dateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        _DP_model.txtToDate = dateTo1.ToString("yyyy-MM-dd");
                        dtrowLines["to_date"] = _DP_model.txtToDate;

                        //dtrowLines["from_date"] = jObject[i]["from_date"]?.ToString();
                        //dtrowLines["to_date"] = jObject[i]["to_date"]?.ToString();
                        dtrowLines["dp_status"] = jObject[i]["dp_status"]?.ToString();
                        dtrowLines["create_id"] = UserID;
                        dtrowLines["app_id"] = UserID;
                        dtrowLines["mod_id"] = UserID;
                        dtrowLines["mac_id"] = mac_id;
                        dtrowLines["Cancel_remarks"] = _DP_model.CancelledRemarks;
                        // dtrowLines["DocMenuId"] = DocumentMenuId;
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
        private DataTable ToDtblAttachmentDetail(DepreciationProcess_Model _model)
        {
            try
            {
                string PageName = _model.title.Replace(" ", "");
                DataTable dtAttachment = new DataTable();
                GetCompDeatil();
                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as DepreciationProcessattch;

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
        public ActionResult SaveDepreciationProcessDetail(DepreciationProcess_Model ObjAddItemGroupSetupBOL)
        {
            try
            {
                DPtemDetail Asset = new DPtemDetail();
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
                DataTable DtblAssetDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable DtblVouGLDetail = new DataTable();
                DataTable CostCenterDetails = new DataTable();

                DtblHeaderDetail = ToDtblDetail(ObjAddItemGroupSetupBOL.Headerdetails, ObjAddItemGroupSetupBOL);
                DtblAssetDetail = ToDtblItemDetail(ObjAddItemGroupSetupBOL.Itemdetails);
                ObjAddItemGroupSetupBOL.title = title;
                DtblAttchDetail = ToDtblAttachmentDetail(ObjAddItemGroupSetupBOL);
                DtblVouGLDetail = ToDtblvouDetail(ObjAddItemGroupSetupBOL.VouGlDetails);
                CostCenterDetails = ToDtblccDetail(ObjAddItemGroupSetupBOL.CC_DetailList);

                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as DepreciationProcessattch;
                TempData["ModelDataattch"] = null;
                if (ObjAddItemGroupSetupBOL.ValidUpto == null)
                {
                    ObjAddItemGroupSetupBOL.ValidUpto = "0";
                }
                if (ObjAddItemGroupSetupBOL.AssetsGroupId.ToString() != "0")
                {
                    string SaveMessage = _DepreciationProcess_ISERVICES.InsertDepreciationProcessDetail(ObjAddItemGroupSetupBOL, DtblHeaderDetail, DtblAssetDetail, DtblAttchDetail, DtblVouGLDetail, CostCenterDetails);
                    //string AssetRegId = status.Substring(status.IndexOf('-') + 1);
                    //string Message = status.Substring(0, status.IndexOf("-"));
                    //if (Message == "Update" || Message == "Save")
                    //{
                    //    ObjAddItemGroupSetupBOL.Message = "Save";
                    //    ObjAddItemGroupSetupBOL.AssetRegId = AssetRegId;
                    //    ObjAddItemGroupSetupBOL.TransType = "Update";
                    //    ObjAddItemGroupSetupBOL.Doc_no = "Update";
                    //    ObjAddItemGroupSetupBOL.Doc_date = "Update";

                    //    string Guid = "";
                    //    if (_DirectPurchaseInvoiceattch != null)
                    //    {
                    //        if (_DirectPurchaseInvoiceattch.Guid != null)
                    //        {
                    //            Guid = _DirectPurchaseInvoiceattch.Guid;
                    //        }
                    //    }
                    //    string guid = Guid;
                    //    var comCont = new CommonController(_Common_IServices);
                    //    comCont.ResetImageLocation(CompID, BrchID, guid, title, AssetRegId, ObjAddItemGroupSetupBOL.TransType, DtblAttchDetail);
                    //}
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
                        //return RedirectToAction("DepreciationProcessAdd");
                    }
                    else
                    {
                        string[] FDetail = SaveMessage.Split(',');
                        string Message = FDetail[5].ToString();
                        string Inv_no = FDetail[0].ToString();
                        string Inv_DATE = FDetail[6].ToString();
                        string Cancel = FDetail[1].ToString();
                        if (Message == "DataNotFound")
                        {
                            var msg = "Data Not found" + " " + Inv_DATE + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            ObjAddItemGroupSetupBOL.Message = Message;
                            //return RedirectToAction("DepreciationProcessAdd");
                        }
                        if (Message == "Save")
                        {
                            //string Guid = "";
                            //if (_DirectPurchaseInvoiceattch != null)
                            //{
                            //    if (_DirectPurchaseInvoiceattch.Guid != null)
                            //    {
                            //        Guid = _DirectPurchaseInvoiceattch.Guid;
                            //    }
                            //}
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
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, ObjAddItemGroupSetupBOL.TransType, DtblAttchDetail);
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
                        else if (Message == "StNtAvl")
                        {
                            ObjAddItemGroupSetupBOL.Message = "StNtAvl";
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
                        //return RedirectToAction("DepreciationProcessAdd");
                    }
                    //if (Message == "Duplicate")
                    //{
                    //    ObjAddItemGroupSetupBOL.TransType = "Duplicate";
                    //    ObjAddItemGroupSetupBOL.Message = "Duplicate";
                    //    ObjAddItemGroupSetupBOL.AssetRegId = AssetRegId;
                    //    ObjAddItemGroupSetupBOL.BtnName = AssetRegId;
                    //    TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                    //    return RedirectToAction("AssetRegistration_DuplicateCase", "AssetRegistration");
                    //}
                    //ObjAddItemGroupSetupBOL.BtnName = "BtnSave";
                    //TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                    //return RedirectToAction("DepreciationProcessAdd", "DepreciationProcess");
                }
                TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                return RedirectToAction("DepreciationProcessAdd", "DepreciationProcess");
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
                DepreciationProcessattch _ScrapSIModelattch = new DepreciationProcessattch();
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
            DepreciationProcess_Model _Model = new DepreciationProcess_Model();
            var a = TrancType.Split(',');
            _Model.Doc_no = a[0].Trim();
            _Model.Doc_date = a[1].Trim();
            _Model.TransType = a[2].Trim();
            if (a[3].Trim() != "" && a[3].Trim() != null)
            {
                WF_status1 = a[3].Trim();
                _Model.WFStatus = WF_status1;
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
            return RedirectToAction("DepreciationProcessDetail", URLModel);
        }
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            GetCompDeatil();
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData("105106105", Doc_no, Doc_dt);
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return "ErrorPage";
            }
            return null;
        }
        /***---------------------------For PDF Start-----------------------------------------***/
        public FileResult GenratePdfFile(DepreciationProcess_Model _model)
        {
            return File(GetPdfData(_model.DocumentMenuId, _model.Doc_no, _model.Doc_date), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        }
        public byte[] GetPdfData(string docId, string invNo, string invDt)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet Details = new DataSet();// _DirectPurchaseInvoice_ISERVICE.GetDirectPurchaseInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt);
                ViewBag.PageName = "PI";
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                ViewBag.Title = "Direct Purchase Invoice";
                htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/DirectPurchaseInvoice/DirectPurchaseInvoicePrint.cshtml"));

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            finally
            {

            }
        }
        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }

        public ActionResult BindDateRange(string f_frequency, string financial_year, string period)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                string start_year = "";
                string end_year = "";
                int months = 0;
                string[] splitPeriod = period.Split(',');
                if (splitPeriod.Length > 1)
                {
                    int start_year1 = Convert.ToDateTime(splitPeriod[0]).Year;
                    start_year = Convert.ToString(start_year1);
                    int end_year1 = Convert.ToDateTime(splitPeriod[1]).Year;
                    end_year = Convert.ToString(end_year1);
                }
                else
                {
                    string[] split_fy_year = financial_year.Split(',');
                    int start_year1 = Convert.ToDateTime(split_fy_year[0]).Year;
                    start_year = Convert.ToString(start_year1);
                    int end_year1 = Convert.ToDateTime(split_fy_year[1]).Year;
                    end_year = Convert.ToString(end_year1);
                    months = Convert.ToInt32(period);
                }
                DataSet ds = _DepreciationProcess_ISERVICES.BindDateRangeCal(Convert.ToInt32(CompID), Convert.ToInt32(BrchID), f_frequency, start_year, end_year, months);
                DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public ActionResult BindPeriod(string f_frequency, string financial_year, string AssetGroupId)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    string[] splitFY = financial_year.Split(',');
                    DataSet ds = _DepreciationProcess_ISERVICES.BindFinancialYear(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), f_frequency, splitFY[0], "", AssetGroupId);
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public DataTable BindPeriodList(DepreciationProcess_Model _model, string f_frequency, string financial_year, string AssetGroupId)
        {
            DataTable dt = new DataTable();
            try
            {
                //JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                string[] splitFY = financial_year.Split(',');
                DataSet ds = _DepreciationProcess_ISERVICES.BindFinancialYear(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), f_frequency, splitFY[0], "", AssetGroupId);
                dt = ds.Tables[1];
                List<period> requirementAreaLists = new List<period>();
                foreach (DataRow dr in dt.Rows)
                {
                    period periodList = new period();
                    periodList.id = dr["id"].ToString();
                    periodList.name = dr["name"].ToString();
                    requirementAreaLists.Add(periodList);
                }
                requirementAreaLists.Insert(0, new period() { id = "0", name = "---Select---" });
                _model.ddl_periodList = requirementAreaLists;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
            return dt;
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
                DataSet GlDt = _DepreciationProcess_ISERVICES.GetAllGLDetails(DtblGLDetail);
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
                DepreciationProcess_Model _Model = new DepreciationProcess_Model();
                string Comp_ID = string.Empty;
                string AssetGroupId = GLDetail.First().id;// string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet GlDt = _DepreciationProcess_ISERVICES.GetTaxRecivableAcc(Comp_ID, AssetGroupId);
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
    }
}