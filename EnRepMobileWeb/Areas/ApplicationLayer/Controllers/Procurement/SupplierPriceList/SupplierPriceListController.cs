using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.SupplierPriceList;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.SupplierPriceList;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.SupplierPriceList
{
    public class SupplierPriceListController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105101101", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SupplierPriceList_ISERVICE _SupplierPriceList_ISERVICE;
        public SupplierPriceListController(Common_IServices _Common_IServices, SupplierPriceList_ISERVICE _SupplierPriceList_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._SupplierPriceList_ISERVICE = _SupplierPriceList_ISERVICE;
        }
        // GET: ApplicationLayer/SupplierPriceList
        public ActionResult SupplierPriceList(string wfStatus)
        {
            SupplierPriceListModel _model = new SupplierPriceListModel();
            CommonPageDetails();
            if (wfStatus != null)
            {
                _model.wf_status = wfStatus;
                _model.ListFilterData = "0,0,0,0" + "," + wfStatus;
            }
            if (TempData["UrlData"] != null)
            {
                if (TempData["UrlData"].ToString() != "")
                {
                    UrlData urlData = TempData["UrlData"] as UrlData;
                    if (urlData.ListFilterData1 != null&& urlData.ListFilterData1 != "undefined")
                    {
                        var arr = urlData.ListFilterData1.Split(',');
                        _model.catalog = arr[0];
                        _model.portfolio = arr[1];
                        _model.ActStatus = arr[2];
                        _model.wf_status = arr[3];
                        if (wfStatus == null)
                        {
                            wfStatus = arr[3];
                        }

                        _model.ListFilterData = _model.catalog + "," + _model.portfolio + "," + _model.ActStatus + "," + wfStatus;
                    }

                }
            }
        GetAllListData(_model);
            _model.Title = title;
            _model.DocumentMenuId = "105101101";
            return View("~/Areas/ApplicationLayer/Views/Procurement/SupplierPriceList/SupplierPriceList.cshtml", _model);
        }
        private void GetAllListData(SupplierPriceListModel _List_Model)
        {
            try
            {
                CommonPageDetails();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                var range = CommonController.Comman_GetFutureDateRange();
                string todate = range.ToDate;
                _List_Model.ValidUpto = todate;
                //_List_Model.ActStatus = "0";
                //_List_Model.catalog = "0";
                //_List_Model.portfolio = "0";
              DataSet CustList = _SupplierPriceList_ISERVICE.GetAllData(CompID, BrchID, UserID, _List_Model.catalog, _List_Model.portfolio, _List_Model.ActStatus, _List_Model.ValidUpto, "105101101", _List_Model.wf_status);
                ViewBag.ListDetail = CustList.Tables[0];

                List<catalogName> Lists = new List<catalogName>();
                foreach (DataRow data in CustList.Tables[3].Rows)
                {
                    catalogName _COADetail = new catalogName();
                    _COADetail.catalog_id = data["setup_id"].ToString();
                    _COADetail.catalog_name = data["setup_val"].ToString();
                    Lists.Add(_COADetail);
                }
                Lists.Insert(0, new catalogName() { catalog_id = "0", catalog_name = "All" });
                _List_Model.CatalogList = Lists;

                List<PortfolioName> List = new List<PortfolioName>();
                foreach (DataRow data in CustList.Tables[4].Rows)
                {
                    PortfolioName _COADetails = new PortfolioName();
                    _COADetails.Portfolio_id = data["setup_id"].ToString();
                    _COADetails.Portfolio_name = data["setup_val"].ToString();
                    List.Add(_COADetails);
                }
                List.Insert(0, new PortfolioName() { Portfolio_id = "0", Portfolio_name = "All" });
                _List_Model.portfolioList = List;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        public ActionResult SPListSearch(string catalog, string portfolio, string supp_act, string AsDate, string wfStatus)
        {
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
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataSet ds = new DataSet();
                //ds = _TDSPosting_ISERVICES.GetTdsPostngList(CompID, BrchID, InCase(Year, "0", null), InCase(month, "0", null), InCase(status, "0", null), DocumentMenuId, null, UserID);
                DataSet CustList = _SupplierPriceList_ISERVICE.GetAllData(CompID, BrchID, UserID, catalog, portfolio, supp_act, AsDate, "105101101", wfStatus);
                ViewBag.ListDetail = CustList.Tables[0];
                ViewBag.ListSearch = "SPLListSearch";
                ViewBag.ListFilterData1 = catalog + "," + portfolio + "," + supp_act + "," + AsDate + "," + wfStatus;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSupplierPriceList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult AddSupplierPriceListDetail(string DocNo, string ListFilterData)
        {
            UrlData urlData = new UrlData();
            /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (DocNo == null)
            {
                if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                {

                    TempData["Message"] = "Financial Year not Exist";
                    SetUrlData(urlData, "", "", "", "Financial Year not Exist", DocNo, ListFilterData);
                    return RedirectToAction("DirectPurchaseInvoice", "DirectPurchaseInvoice", urlData);
                }
            }
            string BtnName = DocNo == null ? "BtnAddNew" : "BtnToDetailPage";
            string TransType = DocNo == null ? "Save" : "Update";
            SetUrlData(urlData, "Add", TransType, BtnName, null, DocNo, ListFilterData);
            return RedirectToAction("SupplierPriceListDetail", "SupplierPriceList", urlData);
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string supp_id = null, string ListFilterData1 = null)
        {
            try
            {
                urlData.Command = Command;
                urlData.TransType = TransType;
                urlData.BtnName = BtnName;
                urlData.Inv_no = supp_id;
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
        public ActionResult SupplierPriceListDetail(UrlData urlData)
        {
            try
            {
                //ViewBag.MenuPageName = getDocumentName();
                CommonPageDetails();
                SupplierPriceList_Model model = new SupplierPriceList_Model();
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                if (Session["userid"] != null)
                    UserID = Session["userid"].ToString();
                /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, urlData.Inv_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                if (TempData["UrlData"] != null)
                {
                    urlData = TempData["UrlData"] as UrlData;
                    model.Message = TempData["Message"] != null ? TempData["Message"].ToString() : null;
                    TempData["UrlData"] = urlData;
                }
                model.Command = urlData.Command;
                model.TransType = urlData.TransType;
                model.BtnName = urlData.BtnName;
                model.hdnSuppId = urlData.Inv_no;
                model.ListFilterData1 = urlData.ListFilterData1;
                model.UserID = UserID;

                DataSet ds1 = GetSupplierAndItemList();

                List<SupplierName> Lists = new List<SupplierName>();
                foreach (DataRow data in ds1.Tables[0].Rows)
                {
                    SupplierName _COADetail = new SupplierName();
                    _COADetail.supp_id = data["supp_id"].ToString();
                    _COADetail.supp_name = data["supp_name"].ToString();
                    Lists.Add(_COADetail);
                }
                Lists.Insert(0, new SupplierName() { supp_id = "0", supp_name = "---Select---" });
                model.SupplierNameList = Lists;

                if (model.hdnSuppId != null)
                {
                    DataSet ds = GetDPIDetailEdit(model.hdnSuppId, DocumentMenuId);
                    ViewBag.ItemDetail = ds.Tables[1];
                    //ViewBag.RevItemDetail = ds.Tables[2];
                    model.hdnSuppId = ds.Tables[0].Rows[0]["hdnSupp_id"].ToString();
                    model.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                    model.suppname = ds.Tables[0].Rows[0]["supp_name"].ToString();
                    model.Category = ds.Tables[0].Rows[0]["supp_catg_name"].ToString();
                    model.HdnCategory = ds.Tables[0].Rows[0]["supp_catg"].ToString();
                    model.HdnPortfolio = ds.Tables[0].Rows[0]["supp_port"].ToString();
                    model.Portfolio = ds.Tables[0].Rows[0]["supp_port_name"].ToString();
                    model.ValidUpto = ds.Tables[0].Rows[0]["valid_upto1"].ToString();
                    model.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                    model.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                    model.Createdon = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                    model.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                    model.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                    model.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                    model.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                    model.Status_name = ds.Tables[0].Rows[0]["status_name"].ToString();
                    model.DocumentStatus = ds.Tables[0].Rows[0]["status"].ToString();
                    string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                    ViewBag.Approve_id = approval_id;
                    string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                    string doc_status = ds.Tables[0].Rows[0]["status"].ToString().Trim();
                    var ActFlag = ds.Tables[0].Rows[0]["act_status"].ToString();

                    List<SupplierName> List1 = new List<SupplierName>();
                    List1.Insert(0, new SupplierName() { supp_id = model.hdnSuppId, supp_name = model.suppname });
                    model.SupplierNameList = List1;

                    if (ActFlag == "Y")
                    {
                        model.ActiveStatus = true;
                    }
                    else
                    {
                        model.ActiveStatus = false;                       
                    }
                    model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                    model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                    if (doc_status != "D" && doc_status != "F")
                    {
                        ViewBag.AppLevel = ds.Tables[5];
                    }
                    if (ViewBag.AppLevel != null && model.Command != "Edit")
                    {
                        var sent_to = "";
                        var nextLevel = "";
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            sent_to = ds.Tables[3].Rows[0]["sent_to"].ToString();
                        }
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            nextLevel = ds.Tables[4].Rows[0]["nextlevel"].ToString().Trim();
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
                model.Title = title;
                model.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/Procurement/SupplierPriceList/SupplierPriceDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionSPLDeatils(SupplierPriceList_Model _model, string Command)
        {
            try
            {/*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                UrlData urlData = new UrlData();
                if (_model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "AddNew":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_model.hdnSuppId))
                            {
                                SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", "Financial Year not Exist", _model.hdnSuppId, _model.ListFilterData1);
                                return RedirectToAction("SupplierPriceListDetail", _model);
                            }
                            else
                            {
                                SetUrlData(urlData, "Refresh", "Refresh", "Refresh", "Financial Year not Exist", null, _model.ListFilterData1);
                                return RedirectToAction("SupplierPriceListDetail", _model);

                            }
                        }
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, _model.ListFilterData1);
                        return RedirectToAction("SupplierPriceListDetail", urlData);
                    case "Edit":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        string invdt = _model.Createdon;
                        DateTime date = DateTime.ParseExact(
    invdt,
    "dd-MM-yyyy HH:mm",
    CultureInfo.InvariantCulture
);

                        string formattedDate = date.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, formattedDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _model.hdnSuppId, _model.ListFilterData1);
                            return RedirectToAction("SupplierPriceListDetail", urlData);
                        }                        
                        SetUrlData(urlData, "Edit", "Update", "BtnEdit", null, _model.hdnSuppId, _model.ListFilterData1);                        
                        return RedirectToAction("SupplierPriceListDetail", urlData);
                    case "Save":
                        SaveSPLDetails(_model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.hdnSuppId, _model.ListFilterData1);
                        return RedirectToAction("SupplierPriceListDetail", urlData);
                    case "Approve":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        string Invdt1 = _model.Createdon;
                        DateTime date1 = DateTime.ParseExact(
Invdt1,
"dd-MM-yyyy HH:mm",
CultureInfo.InvariantCulture
);

                        string formattedDate1 = date1.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, formattedDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", _model.Message, _model.hdnSuppId, _model.ListFilterData1);
                            return RedirectToAction("SupplierPriceListDetail", urlData);
                        }
                        ApproveSPLDetails(_model, _model.hdnSuppId, "", "", "", "", "", "");
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.hdnSuppId, _model.ListFilterData1);
                        return RedirectToAction("SupplierPriceListDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, _model.ListFilterData1);
                        return RedirectToAction("SupplierPriceListDetail", urlData);
                    case "Delete":
                        DeleteSPLDetail(_model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, _model.ListFilterData1);
                        return RedirectToAction("SupplierPriceListDetail", urlData);
                    case "BacktoList":
                        SetUrlData(urlData, "", "", "", null, null, _model.ListFilterData1);
                        return RedirectToAction("SupplierPriceList");
                    case "Print":
                        return null;
                        //return GenratePdfFile(_model);
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("SupplierPriceListDetail", urlData);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult SaveSPLDetails(SupplierPriceList_Model _model)
        {
            try
            {
                string SaveMessage = "";
                string PageName = _model.Title.Replace(" ", "");

                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                if (Session["userid"] != null)
                    UserID = Session["userid"].ToString();

                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblRevisionDetail = new DataTable();

                DtblHDetail = ToDtblHeaderDetail(_model);
                DtblItemDetail = ToDtblItemDetail(_model.ItemDetails, _model.hdnSuppId);
                DtblRevisionDetail = ToDtblRevItem(_model.RevItemDetails);
               
                SaveMessage = _SupplierPriceList_ISERVICE.InsertDPI_Details(DtblHDetail, DtblItemDetail, DtblRevisionDetail);
                if (SaveMessage == "DocModify")
                {
                    _model.Message = "DocModify";
                    _model.BtnName = "Refresh";
                    _model.Command = "Refresh";
                    return RedirectToAction("DirectPurchaseInvoiceDetail");
                }              
                else
                {
                    string[] FDetail = SaveMessage.Split(',');
                    string supp_id = FDetail[0].ToString();
                   
                        string Message = FDetail[5].ToString();
                        string Cansal = FDetail[1].ToString();

                        if (Message == "Update" || Message == "Save")
                        {
                            _model.Message = "Save";
                            _model.Command = "Update";
                            _model.hdnSuppId = supp_id;
                            _model.BtnName = "BtnSave";
                            _model.TransType = "Update";
                        }
                    }
                    return RedirectToAction("DirectPurchaseInvoiceDetail");
               
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private DataTable ToDtblHeaderDetail(SupplierPriceList_Model _model)
        {
            try
            {
                DataTable dtheaderdeatil = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("MenuID", typeof(string));
                dtheader.Columns.Add("act_status", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("user_id", typeof(int));
                dtheader.Columns.Add("supp_id", typeof(string));
                dtheader.Columns.Add("supp_catg", typeof(string));
                dtheader.Columns.Add("supp_port", typeof(string));
                dtheader.Columns.Add("valid_upto", typeof(string));
                dtheader.Columns.Add("status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                if (_model.DocumentStatus != null)
                {
                    _model.TransType = "Update";
                    dtrowHeader["TransType"] = _model.TransType;
                }
                else
                {
                    _model.TransType = "Save";
                    dtrowHeader["TransType"] = _model.TransType;
                }
                dtrowHeader["MenuID"] = "105101101";
                string ActiveStatus = _model.ActiveStatus.ToString();
                if (ActiveStatus == "False")
                    dtrowHeader["act_status"] = "N";
                else
                    dtrowHeader["act_status"] = "Y";
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                dtrowHeader["user_id"] = Session["userid"].ToString();
                dtrowHeader["supp_id"] = _model.hdnSuppId;
                dtrowHeader["supp_catg"] = _model.HdnCategory;
                dtrowHeader["supp_port"] = _model.HdnPortfolio;
                dtrowHeader["valid_upto"] = _model.ValidUpto;
                dtrowHeader["status"] = IsNull(_model.DocumentStatus,"D");
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;              
                dtheader.Rows.Add(dtrowHeader);

                dtheaderdeatil = dtheader;
                return dtheaderdeatil;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblItemDetail(string ItemDetails,string supp_id)
        {
            try
            {
                DataTable dtItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("br_id", typeof(int));
                dtItem.Columns.Add("supp_id", typeof(int));
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("PackSize", typeof(string));
                dtItem.Columns.Add("mrp", typeof(string));
                dtItem.Columns.Add("price", typeof(string));
                dtItem.Columns.Add("item_disc_perc", typeof(string));
                dtItem.Columns.Add("effective_price", typeof(string));

                if (ItemDetails != null)
                {
                    JArray jObject = JArray.Parse(ItemDetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["comp_id"] = Session["CompId"].ToString();
                        dtrowLines["br_id"] = Session["BranchId"].ToString();
                        dtrowLines["supp_id"] = supp_id;
                        dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                        dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["uom_id"].ToString());
                        dtrowLines["PackSize"] = jObject[i]["PackSize"].ToString();
                        dtrowLines["mrp"] = IsNull(jObject[i]["MRP"].ToString(),"0");
                        dtrowLines["price"] = jObject[i]["Price"].ToString();
                        dtrowLines["item_disc_perc"] = jObject[i]["item_disc_perc"].ToString();
                        dtrowLines["effective_price"] = jObject[i]["EffectivePrice"].ToString();                       
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
        private DataTable ToDtblRevItem(string RevItemDetails)
        {
            try
            {
                DataTable dtItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("rev_dt", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("pack_size", typeof(string));
                dtItem.Columns.Add("mrp", typeof(string));
                dtItem.Columns.Add("price", typeof(string));
                dtItem.Columns.Add("item_disc_perc", typeof(string));
                dtItem.Columns.Add("effective_price", typeof(string));
                dtItem.Columns.Add("rev_no", typeof(string));

                if (RevItemDetails != null)
                {
                    JArray jObject = JArray.Parse(RevItemDetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();                                             
                        dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                        dtrowLines["rev_dt"] = jObject[i]["rev_dt"].ToString();
                        dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["uom_id"].ToString());
                        dtrowLines["PackSize"] = jObject[i]["PackSize"].ToString();
                        dtrowLines["mrp"] = jObject[i]["mrp"].ToString();
                        dtrowLines["price"] = jObject[i]["price"].ToString();
                        dtrowLines["item_disc_perc"] = jObject[i]["item_disc_perc"].ToString();
                        dtrowLines["effective_price"] = jObject[i]["effective_price"].ToString();
                        dtrowLines["rev_no"] = jObject[i]["rev_no"].ToString();
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
        private void DeleteSPLDetail(SupplierPriceList_Model _model)
        {
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
                string Result = _SupplierPriceList_ISERVICE.DeleteSPLDetails(CompID, BrchID, _model.hdnSuppId);
                _model.Message = Result.Split(',')[0];

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public DataSet GetSupplierAndItemList()
        {
            DataSet dt = new DataSet();
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
                DataSet result = _SupplierPriceList_ISERVICE.GetSupplierAndItemList(Comp_ID, Br_ID);
                dt = result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
            return dt;
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
        public DataSet GetDPIDetailEdit(string supp_id, string DocumentMenuId)
        {
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                DataSet result = _SupplierPriceList_ISERVICE.GetDPIDetailDAL(CompID, BrchID, supp_id, UserID, DocumentMenuId);
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
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
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        public ActionResult ApproveSPLDetails(SupplierPriceList_Model _model, string supp_id
           , string A_Status, string A_Level, string A_Remarks, string FilterData
           , string docid, string WF_Status1)
        {
            try
            {
                UrlData urlData = new UrlData();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Result = _SupplierPriceList_ISERVICE.ApproveSPLDetail(supp_id, docid, BrchID
                    , CompID, UserID, mac_id, A_Status, A_Level, A_Remarks);
                try
                {
                    //string fileName = Inv_No.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    //var filePath = SavePdfDocToSendOnEmailAlert(Inv_No, Inv_Date, fileName, DocumentMenuId, "AP");
                    //_Common_IServices.SendAlertEmail(CompID, BrchID, "105101154", Inv_No, "AP", UserID, "", filePath);
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
                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, Result.Split(',')[0], FilterData);
                return RedirectToAction("SupplierPriceListDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            var WF_status1 = "";
            //Session["Message"] = "";
            SupplierPriceList_Model _Model = new SupplierPriceList_Model();
            var a = TrancType.Split(',');
            _Model.hdnSuppId = a[0].Trim();
            _Model.TransType = a[2].Trim();
            if (a[3].Trim() != "" && a[3].Trim() != null)
            {
                WF_status1 = a[3].Trim();
                _Model.WF_Status1 = WF_status1;
            }
            var docId = a[4].Trim();
            _Model.Message = Mailerror;
            _Model.BtnName = "BtnToDetailPage";
            _Model.DocumentMenuId = docId;
            TempData["ModelData"] = _Model;
            TempData["WF_status1"] = WF_status1; ;
            TempData["ListFilterData"] = ListFilterData1;
            UrlData URLModel = new UrlData();
            URLModel.Inv_no = a[0].Trim();
            URLModel.Inv_dt = a[1].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.ListFilterData1 = ListFilterData1;
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnToDetailPage";
            TempData["UrlData"] = URLModel;
            TempData["Message"] = _Model.Message;
            return RedirectToAction("SupplierPriceListDetail", URLModel);
        }
        public ActionResult BindReplicateWithlist(SupplierPriceList_Model MRSModel)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                    {
                        BrchID = Session["BranchId"].ToString();
                    }
                    if (MRSModel.Search == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = MRSModel.Search;
                    }
                    DataSet ProductList = _SupplierPriceList_ISERVICE.getReplicateWith(CompID, BrchID, SarchValue);
                    if (ProductList.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[0].Rows.Count; i++)
                        {
                            string supp_id = ProductList.Tables[0].Rows[i]["supp_id"].ToString();
                            string supp_name = ProductList.Tables[0].Rows[i]["supp_name"].ToString();
                            ItemList.Add(supp_name, supp_id);
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
        public JsonResult GetReplicateWithSUpplierPriceList(string Supp_id)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet result = _SupplierPriceList_ISERVICE.GetReplicateWithItemdata(CompID, BrchID, Supp_id);
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
        public ActionResult GetRevPriceHistoryDetails(string supp_id, string Item_id)
        {
            try
            {
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
                DataTable dt = new DataTable();
                dt = _SupplierPriceList_ISERVICE.GetCustPriceHistryDtl(Comp_ID, Br_ID, supp_id, Item_id);

                if (dt.Rows.Count > 0)
                {
                    ViewBag.supplierHistryItemDetails = dt;
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialRevisionHistoryDetail.cshtml");
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