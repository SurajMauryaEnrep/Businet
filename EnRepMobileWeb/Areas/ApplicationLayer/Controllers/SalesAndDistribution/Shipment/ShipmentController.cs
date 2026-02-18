using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.Shipment;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.Shipment;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System.Threading;
using System.Text;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TransporterSetup;
using EnRepMobileWeb.Areas.BusinessLayer.Controllers.TransporterSetup;
using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using System.Configuration;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution
{
    public class ShipmentController : Controller
    {
        string DocumentMenuId = "", title;
        ShipmentDetail_MODEL _ShipmentDetail_MODEL;
        Shipment_ISERVICES _Shipment_ISERVICES;
        private readonly TransporterSetup_ISERVICES _trptIService;
        // ShipmentList_MODEL _ShipmentList_MODEL;
        string CompID, BrchID, ship_no, userid, language = String.Empty;

        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataSet ds;
        Common_IServices _Common_IServices;
        public ShipmentController(Shipment_ISERVICES _Shipment_ISERVICES, Common_IServices _Common_IServices,
            TransporterSetup_ISERVICES trptIService)
        {
            this._Shipment_ISERVICES = _Shipment_ISERVICES;
            this._Common_IServices = _Common_IServices;
            this._trptIService = trptIService;
        }
        public List<TransListModel> GetTransporterList()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            var transController = new TransporterSetupController(_Common_IServices, _trptIService);
            var transList = transController.GetTransporterList(CompID);
            transList.RemoveAt(0);
            transList.Insert(0, new TransListModel { TransId = "0", TransName = "---Select--" });
            return transList;
        }
        private DataTable GetRoleList()
        {
            try
            {
                string UserID = string.Empty;
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
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private void CommonPageDetails(string DocumentMenuId)
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
                    userid = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, userid, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];

                ViewBag.VBRoleList = ds.Tables[3];
                #region Commented this Code by Nitesh 24-05-2024 for param_id and table Name Changed GstApplicable And ExpComrcPrintOptions
                #endregion
                //foreach (DataRow Row in ds.Tables[1].Rows)
                //{
                //    if (Row["param_id"].ToString() == "101")
                //    {
                //        ViewBag.GstApplicable = Row["param_stat"].ToString();
                //    }
                //    else if (Row["param_id"].ToString() == "107")
                //    {
                //        ViewBag.ExpComrcPrintOptions = Row["param_stat"].ToString();
                //    }
                //}
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                ViewBag.ExpComrcPrintOptions = ds.Tables[9].Rows.Count > 0 ? ds.Tables[9].Rows[0]["param_stat"].ToString() : "";

                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
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
        public ActionResult ShipmentListD(ShipmentList_MODEL _ShipmentList_MODEL)
        {
            try
            {
                DocumentMenuId = "105103135";
                _ShipmentList_MODEL.ShipMent_type = "D";
                _ShipmentList_MODEL.DocumentMenuId = DocumentMenuId;
                _ShipmentList_MODEL.MenuDocumentId = DocumentMenuId;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);
                //_ShipmentList_MODEL = new ShipmentList_MODEL();
                GetStatus(_ShipmentList_MODEL);
                DataTable dt = new DataTable();
                List<ListCustomerName> CustLists = new List<ListCustomerName>();
                dt = GetCustNameList(_ShipmentList_MODEL);
                foreach (DataRow dr in dt.Rows)
                {
                    ListCustomerName _RAList = new ListCustomerName();
                    _RAList.cust_id = dr["cust_id"].ToString();
                    _RAList.cust_name = dr["cust_name"].ToString();
                    CustLists.Add(_RAList);
                }
                CustLists.Insert(0, new ListCustomerName() { cust_id = "0", cust_name = "---Select---" });
                _ShipmentList_MODEL.CustomerNameList = CustLists;

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    var CustID = a[0].Trim();
                    var Date = a[1].Trim();
                    var tdt = a[2].Trim();
                    DateTime Fromdate = Convert.ToDateTime(a[1].Trim());
                    DateTime Todate = Convert.ToDateTime(a[2].Trim());
                    var Status = a[3].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    List<ShipmentDetails> _ShipmentDetailsList = new List<ShipmentDetails>();
                    DataTable DetailDatable = _Shipment_ISERVICES.GetShipmentListByFilter(CustID, Fromdate, Todate, Status, CompID, BrchID, DocumentMenuId);
                    Session["SCSearch"] = "SC_Search";
                    _ShipmentList_MODEL.FromDate = Date;
                    _ShipmentList_MODEL.cust_id = CustID;
                    _ShipmentList_MODEL.StatusCode = Status;
                    _ShipmentList_MODEL.ToDate = tdt;
                    _ShipmentList_MODEL.ListFilterData = TempData["ListFilterData"].ToString();
                    //ShipmentList_MODEL _ShipmentList_MODEL = new ShipmentList_MODEL();
                    foreach (DataRow dr in DetailDatable.Rows)
                    {
                        ShipmentDetails _ShipmentDetails = new ShipmentDetails();
                        _ShipmentDetails.ship_no = dr["ship_no"].ToString();
                        _ShipmentDetails.finstrdate = dr["finstrdate"].ToString();
                        _ShipmentDetails.ship_dt = dr["ship_dt"].ToString();
                        _ShipmentDetails.ship_date = dr["ship_date"].ToString();
                        _ShipmentDetails.ship_status = dr["ship_status"].ToString();
                        _ShipmentDetails.pack_no = dr["pack_no"].ToString();
                        _ShipmentDetails.pack_dt = dr["pack_dt"].ToString();
                        _ShipmentDetails.cust_name = dr["cust_name"].ToString();
                        _ShipmentDetails.create_dt = dr["create_dt"].ToString();
                        _ShipmentDetails.ship_type = dr["ship_type"].ToString();
                        _ShipmentDetails.mod_dt = dr["mod_dt"].ToString();
                        _ShipmentDetails.app_dt = dr["app_dt"].ToString();
                        _ShipmentDetails.create_by = dr["create_by"].ToString();
                        _ShipmentDetails.app_by = dr["app_by"].ToString();
                        _ShipmentDetails.mod_by = dr["mod_by"].ToString();
                        _ShipmentDetails.custom_inv_no = dr["custom_inv_no"].ToString();
                        _ShipmentDetails.custom_inv_dt = dr["custom_inv_dt"].ToString();
                        _ShipmentDetails.GRNumber = dr["gr_no"].ToString();
                        _ShipmentDetails.GRDate = dr["gr_dt"].ToString();
                        _ShipmentDetails.TransporterName = dr["trans_name"].ToString();
                        _ShipmentDetailsList.Add(_ShipmentDetails);
                    }
                    _ShipmentList_MODEL.ShipmentDetailsList = _ShipmentDetailsList;
                }
                else
                {
                    _ShipmentList_MODEL.FromDate = startDate;
                    _ShipmentList_MODEL.ToDate = CurrentDate;
                    _ShipmentList_MODEL.ShipmentDetailsList = GetShipmenListAllDetail(_ShipmentList_MODEL);
                }
                //List<ShipmentListCustomerName> _CustomerNameList = new List<ShipmentListCustomerName>();
                //ShipmentListCustomerName _CustomerName = new ShipmentListCustomerName();
                //_CustomerName.cust_name = "---Select---";
                //_CustomerName.cust_id = "0";
                //_CustomerNameList.Add(_CustomerName);
                //_ShipmentList_MODEL.CustomerNameList = _CustomerNameList;

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                //string endDate = dtnow.ToString("yyyy-MM-dd");
                //ViewBag.MenuPageName = getDocumentName();
                CommonPageDetails(DocumentMenuId);
                _ShipmentList_MODEL.Title = title;
                //Session["SCSearch"] = "0";
                _ShipmentList_MODEL.SCSearch = "0";
                //ViewBag.VBRoleList = GetRoleList();
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);

                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentList.cshtml", _ShipmentList_MODEL);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ShipmentListE(ShipmentList_MODEL _ShipmentList_MODEL)
        {
            try
            {
                DocumentMenuId = "105103145120";
                _ShipmentList_MODEL.ShipMent_type = "E";
                _ShipmentList_MODEL.DocumentMenuId = DocumentMenuId;
                _ShipmentList_MODEL.MenuDocumentId = DocumentMenuId;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);
                //_ShipmentList_MODEL = new ShipmentList_MODEL();
                GetStatus(_ShipmentList_MODEL);
                DataTable dt = new DataTable();
                List<ListCustomerName> CustLists = new List<ListCustomerName>();
                dt = GetCustNameList(_ShipmentList_MODEL);
                foreach (DataRow dr in dt.Rows)
                {
                    ListCustomerName _RAList = new ListCustomerName();
                    _RAList.cust_id = dr["cust_id"].ToString();
                    _RAList.cust_name = dr["cust_name"].ToString();
                    CustLists.Add(_RAList);
                }
                CustLists.Insert(0, new ListCustomerName() { cust_id = "0", cust_name = "---Select---" });
                _ShipmentList_MODEL.CustomerNameList = CustLists;

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    var CustID = a[0].Trim();
                    var Date = a[1].Trim();
                    var tdt = a[2].Trim();
                    DateTime Fromdate = Convert.ToDateTime(a[1].Trim());
                    DateTime Todate = Convert.ToDateTime(a[2].Trim());
                    var Status = a[3].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    List<ShipmentDetails> _ShipmentDetailsList = new List<ShipmentDetails>();
                    DataTable DetailDatable = _Shipment_ISERVICES.GetShipmentListByFilter(CustID, Fromdate, Todate, Status, CompID, BrchID, DocumentMenuId);
                    Session["SCSearch"] = "SC_Search";
                    _ShipmentList_MODEL.FromDate = Date;
                    _ShipmentList_MODEL.cust_id = CustID;
                    _ShipmentList_MODEL.StatusCode = Status;
                    _ShipmentList_MODEL.ToDate = tdt;
                    _ShipmentList_MODEL.ListFilterData = TempData["ListFilterData"].ToString();
                    //ShipmentList_MODEL _ShipmentList_MODEL = new ShipmentList_MODEL();
                    foreach (DataRow dr in DetailDatable.Rows)
                    {
                        ShipmentDetails _ShipmentDetails = new ShipmentDetails();
                        _ShipmentDetails.ship_no = dr["ship_no"].ToString();
                        _ShipmentDetails.finstrdate = dr["finstrdate"].ToString();
                        _ShipmentDetails.ship_dt = dr["ship_dt"].ToString();
                        _ShipmentDetails.ship_date = dr["ship_date"].ToString();
                        _ShipmentDetails.ship_status = dr["ship_status"].ToString();
                        _ShipmentDetails.pack_no = dr["pack_no"].ToString();
                        _ShipmentDetails.pack_dt = dr["pack_dt"].ToString();
                        _ShipmentDetails.cust_name = dr["cust_name"].ToString();
                        _ShipmentDetails.create_dt = dr["create_dt"].ToString();
                        _ShipmentDetails.ship_type = dr["ship_type"].ToString();
                        _ShipmentDetails.mod_dt = dr["mod_dt"].ToString();
                        _ShipmentDetails.app_dt = dr["app_dt"].ToString();
                        _ShipmentDetails.create_by = dr["create_by"].ToString();
                        _ShipmentDetails.app_by = dr["app_by"].ToString();
                        _ShipmentDetails.mod_by = dr["mod_by"].ToString();
                        _ShipmentDetails.custom_inv_no = dr["custom_inv_no"].ToString();
                        _ShipmentDetails.custom_inv_dt = dr["custom_inv_dt"].ToString();
                        _ShipmentDetails.GRNumber = dr["gr_no"].ToString();
                        _ShipmentDetails.GRDate = dr["gr_dt"].ToString();
                        _ShipmentDetails.TransporterName = dr["trans_name"].ToString();
                        _ShipmentDetailsList.Add(_ShipmentDetails);
                    }
                    _ShipmentList_MODEL.ShipmentDetailsList = _ShipmentDetailsList;
                }
                else
                {
                    _ShipmentList_MODEL.FromDate = startDate;
                    _ShipmentList_MODEL.ToDate = CurrentDate;
                    _ShipmentList_MODEL.ShipmentDetailsList = GetShipmenListAllDetail(_ShipmentList_MODEL);
                }
                CommonPageDetails(DocumentMenuId);
                _ShipmentList_MODEL.Title = title;
                _ShipmentList_MODEL.SCSearch = "0";
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentList.cshtml", _ShipmentList_MODEL);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [NonAction]
        public List<ShipmentDetails> GetShipmenListAllDetail(ShipmentList_MODEL _ShipmentList_MODEL)
        {
            try
            {
                string CompID = string.Empty;
                string br_id = string.Empty;
                List<ShipmentDetails> _ShipmentDetailsList = new List<ShipmentDetails>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string UserID = "";
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                if (_ShipmentList_MODEL.MenuDocumentId != null)
                {
                    if (_ShipmentList_MODEL.MenuDocumentId == "105103135")
                    {
                        DocumentMenuId = "105103135";
                    }
                    else if (_ShipmentList_MODEL.MenuDocumentId == "105103145120")
                    {
                        DocumentMenuId = "105103145120";
                    }

                }
                string wfstatus = "";
                if (_ShipmentList_MODEL.WF_Status != null)
                {
                    wfstatus = _ShipmentList_MODEL.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }

                DataSet DetailDatable = _Shipment_ISERVICES.GetShipmentListAll(CompID, br_id, UserID, wfstatus, DocumentMenuId, _ShipmentList_MODEL.FromDate, _ShipmentList_MODEL.ToDate);
                //FromDate = DetailDatable.Tables[1].Rows[0]["finstrdate"].ToString();
                if (DetailDatable.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in DetailDatable.Tables[0].Rows)
                    {
                        ShipmentDetails _ShipmentDetails = new ShipmentDetails();
                        _ShipmentDetails.ship_no = dr["ship_no"].ToString();
                        _ShipmentDetails.finstrdate = DetailDatable.Tables[1].Rows[0]["finstrdate"].ToString();
                        _ShipmentDetails.ship_dt = dr["ship_dt"].ToString();
                        _ShipmentDetails.ship_date = dr["ship_date"].ToString();
                        _ShipmentDetails.ship_status = dr["ship_status"].ToString();
                        _ShipmentDetails.pack_no = dr["pack_no"].ToString();
                        _ShipmentDetails.pack_dt = dr["pack_dt"].ToString();
                        _ShipmentDetails.cust_name = dr["cust_name"].ToString();
                        _ShipmentDetails.create_dt = dr["create_dt"].ToString();
                        _ShipmentDetails.ship_type = dr["ship_type"].ToString();
                        _ShipmentDetails.mod_dt = dr["mod_dt"].ToString();
                        _ShipmentDetails.app_dt = dr["app_dt"].ToString();
                        _ShipmentDetails.create_by = dr["create_by"].ToString();
                        _ShipmentDetails.app_by = dr["app_by"].ToString();
                        _ShipmentDetails.mod_by = dr["mod_by"].ToString();
                        _ShipmentDetails.custom_inv_no = dr["custom_inv_no"].ToString();
                        _ShipmentDetails.custom_inv_dt = dr["custom_inv_dt"].ToString();
                        _ShipmentDetails.GRNumber = dr["gr_no"].ToString();
                        _ShipmentDetails.GRDate = dr["gr_dt"].ToString();
                        _ShipmentDetails.TransporterName = dr["trans_name"].ToString();
                        _ShipmentDetailsList.Add(_ShipmentDetails);
                    }
                }
                return _ShipmentDetailsList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult ShipmentListSearch(string CustID, DateTime Fromdate, DateTime Todate, string Status, string Docid)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                DocumentMenuId = Docid;

                ShipmentList_MODEL _ShipmentList_MODEL = new ShipmentList_MODEL();
                _ShipmentList_MODEL.WF_Status = null;
                List<ShipmentDetails> _ShipmentDetailsList = new List<ShipmentDetails>();
                DataTable DetailDatable = _Shipment_ISERVICES.GetShipmentListByFilter(CustID, Fromdate, Todate, Status, CompID, BrchID, DocumentMenuId);
                //Session["SCSearch"] = "SC_Search";
                _ShipmentList_MODEL.SCSearch = "SC_Search";
                foreach (DataRow dr in DetailDatable.Rows)
                {
                    ShipmentDetails _ShipmentDetails = new ShipmentDetails();
                    _ShipmentDetails.ship_no = dr["ship_no"].ToString();
                    _ShipmentDetails.finstrdate = dr["finstrdate"].ToString();
                    _ShipmentDetails.ship_dt = dr["ship_dt"].ToString();
                    _ShipmentDetails.ship_date = dr["ship_date"].ToString();
                    _ShipmentDetails.ship_status = dr["ship_status"].ToString();
                    _ShipmentDetails.pack_no = dr["pack_no"].ToString();
                    _ShipmentDetails.pack_dt = dr["pack_dt"].ToString();
                    _ShipmentDetails.cust_name = dr["cust_name"].ToString();
                    _ShipmentDetails.create_dt = dr["create_dt"].ToString();
                    _ShipmentDetails.ship_type = dr["ship_type"].ToString();
                    _ShipmentDetails.mod_dt = dr["mod_dt"].ToString();
                    _ShipmentDetails.app_dt = dr["app_dt"].ToString();
                    _ShipmentDetails.create_by = dr["create_by"].ToString();
                    _ShipmentDetails.app_by = dr["app_by"].ToString();
                    _ShipmentDetails.mod_by = dr["mod_by"].ToString();
                    _ShipmentDetails.custom_inv_no = dr["custom_inv_no"].ToString();
                    _ShipmentDetails.custom_inv_dt = dr["custom_inv_dt"].ToString();
                    _ShipmentDetails.GRNumber = dr["gr_no"].ToString();
                    _ShipmentDetails.GRDate = dr["gr_dt"].ToString();
                    _ShipmentDetails.TransporterName = dr["trans_name"].ToString();
                    _ShipmentDetailsList.Add(_ShipmentDetails);
                }
                _ShipmentList_MODEL.ShipmentDetailsList = _ShipmentDetailsList;
                _ShipmentList_MODEL.DocumentMenuId = Docid;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/ShipmentListDetail.cshtml", _ShipmentList_MODEL);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private DataTable GetCustNameList(ShipmentList_MODEL _ShipmentList_MODEL)
        {
            try
            {
                string CustName = string.Empty;
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_ShipmentList_MODEL.cust_id))
                {
                    CustName = "0";
                }
                else
                {
                    CustName = _ShipmentList_MODEL.cust_id;
                }
                var CustType = _ShipmentList_MODEL.ShipMent_type;
                DataTable CustList = _Shipment_ISERVICES.GetCustomer_List(CompID, CustName, BrchID, CustType);
                //DataTable dt = _Shipment_ISERVICES.GetCustNameList(CompID, BrchID, CustomerName);
                return CustList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult EditShipment(string ShipmentNumber, String ShipmentDate, string ListFilterData, string WF_Status, string Docid, string ShipMent_type)
        {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            string UserID = string.Empty;
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            ShipmentDetail_MODEL DblClick = new ShipmentDetail_MODEL();
            UrlModel _urlModel = new UrlModel();
            DblClick.Command = "Update";
            DblClick.ShipmentNumber = ShipmentNumber;
            DblClick.ShipmentDate = ShipmentDate;
            DblClick.TransType = "Update";
            DblClick.AppStatus = "Update";
            DblClick.BtnName = "BtnEdit";
            DblClick.UserID = UserID;
            if (WF_Status != null && WF_Status != null)
            {
                DblClick.WF_Status1 = WF_Status;
                _urlModel.WF_sts1 = WF_Status;
            }
            DblClick.DocumentMenuId = Docid;
            DblClick.ShipMent_type = ShipMent_type;
            _urlModel.Trns_Typ = "Update";
            _urlModel.Btn = DblClick.BtnName;
            _urlModel.Spmt_no = ShipmentNumber;
            _urlModel.Spmt_dt = ShipmentDate;
            _urlModel.DocId = Docid;
            _urlModel.Sp_typ = ShipMent_type;
            TempData["ModelData"] = DblClick;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("ShipmentDetail", "Shipment", _urlModel);
        }
        public ActionResult AddNewShipment(string DocumentMenuId)
        {
            ShipmentDetail_MODEL AddNew_Model = new ShipmentDetail_MODEL();
            UrlModel _urlModel = new UrlModel();
            var sp_typ = "";
            AddNew_Model.AppStatus = "D";
            AddNew_Model.BtnName = "BtnAddNew";
            AddNew_Model.DocumentMenuId = DocumentMenuId;
            AddNew_Model.TransType = "Save";
            AddNew_Model.Command = "New";
            if (DocumentMenuId != null)
            {
                if (DocumentMenuId == "105103135")
                {
                    sp_typ = "D";
                }
                else if (DocumentMenuId == "105103145120")
                {
                    sp_typ = "E";
                }
                AddNew_Model.ShipMent_type = sp_typ;
                _urlModel.Sp_typ = sp_typ;
            }
            _urlModel.Apsts = "D";
            _urlModel.Btn = "BtnAddNew";
            _urlModel.DocId = DocumentMenuId;
            _urlModel.Trns_Typ = "Save";
            _urlModel.Cmd = "New";
            TempData["ModelData"] = AddNew_Model;
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                if (DocumentMenuId == "105103135")
                {
                    return RedirectToAction("ShipmentListD");
                }
                else
                {
                    return RedirectToAction("ShipmentListE");
                }

            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("ShipmentDetail", "Shipment", _urlModel);
        }
        private string GetCstmInvNo()
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
                string RoleList = _Shipment_ISERVICES.GetCstmInvNo(CompID, BrchID);
                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
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
        public ActionResult ShipmentDetail(UrlModel _urlModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string UserID = "";
                if (Session["UserID"] != null)
                {
                    UserID = Session["UserID"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                /*Add by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.Spmt_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var _Shipment_Detail_Model = TempData["ModelData"] as ShipmentDetail_MODEL;
               
                if (_Shipment_Detail_Model != null)
                {
                    DocumentMenuId = _Shipment_Detail_Model.DocumentMenuId;
                    _Shipment_Detail_Model.CompId = CompID;
                    var date = DateTime.Now;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);
                    List<CustomerName> _CustList = new List<CustomerName>();

                    _CustList.Insert(0, new CustomerName() { cust_name = "---Select---", cust_id = "0" });
                    _Shipment_Detail_Model.CustomerNameList = _CustList;

                    string CstmInvNo = GetCstmInvNo();
                    _Shipment_Detail_Model.custom_inv_no = CstmInvNo;
                    // for bind Trade term
                    List<trade_termList> _TermLists = new List<trade_termList>();
                    _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                    _Shipment_Detail_Model.TradeTermsList = _TermLists;

                    List<PortOfLoadingListModel> _PortOfLoadingListModel = new List<PortOfLoadingListModel>();
                    PortOfLoadingListModel PortOfLoadingLis = new PortOfLoadingListModel();
                    PortOfLoadingLis.POL_Name = "---Select---";
                    PortOfLoadingLis.POL_id = "0";
                    _PortOfLoadingListModel.Add(PortOfLoadingLis);
                    _Shipment_Detail_Model.PortOfLoadingList = _PortOfLoadingListModel;

                    List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
                    pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
                    _pi_rcpt_carrLis.Pi_Name = "---Select---";
                    _pi_rcpt_carrLis.Pi_id = "0";
                    _pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
                    _Shipment_Detail_Model.pi_rcpt_carrList = _pi_rcpt_carrListModel;

                    _Shipment_Detail_Model.ship_dt = DateTime.Now.ToString("yyyy-MM-dd");

                    List<PackListNumber> _PackListNumber = new List<PackListNumber>();
                    PackListNumber _PackList_Number = new PackListNumber();
                    _PackList_Number.packing_no = "---Select---";
                    _PackList_Number.packing_dt = "0";
                    _PackListNumber.Add(_PackList_Number);
                    _Shipment_Detail_Model.PackListNumberList = _PackListNumber;
                    _Shipment_Detail_Model.BrchID = BrchID;
                    _Shipment_Detail_Model.TransList = GetTransporterList();
                    CommonPageDetails(DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _Shipment_Detail_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    string QtyDigit = "";
                    if (_Shipment_Detail_Model.DocumentMenuId == "105103145120")
                    {
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"]));
                    }
                    else
                    {
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                    }
                    var WeightDigit = ToFixDecimal(Convert.ToInt32(Session["WeightDigit"]));
                    if (_Shipment_Detail_Model.TransType == "Update")
                    {
                        string ShipmentNumber = _Shipment_Detail_Model.ShipmentNumber;
                        string ShipmentDate = _Shipment_Detail_Model.ShipmentDate;
                        DataSet ds = _Shipment_ISERVICES.getShipmentDetailByShipmentNo(CompID, BrchID, UserID, ShipmentNumber, Convert.ToDateTime(ShipmentDate), DocumentMenuId);
                        _Shipment_Detail_Model.ship_type = ds.Tables[0].Rows[0]["ship_type"].ToString();
                        _Shipment_Detail_Model.custom_inv_no = ds.Tables[0].Rows[0]["custom_inv_no"].ToString();

                        GetAutoCompleteCustomerName(_Shipment_Detail_Model);
                        ShipmentPackingListView(_Shipment_Detail_Model);

                        if (_Shipment_Detail_Model.PackListNumberList.Find(obj => obj.packing_no == ds.Tables[0].Rows[0]["pack_no"].ToString()) == null)
                        {
                            PackListNumber _IPackListListNumber = new PackListNumber();
                            _IPackListListNumber.packing_no = ds.Tables[0].Rows[0]["pack_no"].ToString();
                            _IPackListListNumber.packing_dt = ds.Tables[0].Rows[0]["pack_no"].ToString();
                            _Shipment_Detail_Model.PackListNumberList.Add(_IPackListListNumber);
                        }
                        _Shipment_Detail_Model.curr_des = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        _Shipment_Detail_Model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _Shipment_Detail_Model.CustomerID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _Shipment_Detail_Model.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _Shipment_Detail_Model.ship_no = ds.Tables[0].Rows[0]["ship_no"].ToString();
                        _Shipment_Detail_Model.ship_dt = ds.Tables[0].Rows[0]["so_dt"].ToString();
                        _Shipment_Detail_Model.pack_num = ds.Tables[0].Rows[0]["pack_no"].ToString();
                        _Shipment_Detail_Model.pack_dte = ds.Tables[0].Rows[0]["pack_dt"].ToString();
                        _Shipment_Detail_Model.bill_address = ds.Tables[0].Rows[0]["bill_address_name"].ToString();
                        _Shipment_Detail_Model.ship_address = ds.Tables[0].Rows[0]["ship_address_name"].ToString();
                        _Shipment_Detail_Model.bill_add_id = ds.Tables[0].Rows[0]["bill_address"].ToString();
                        _Shipment_Detail_Model.ship_add_id = ds.Tables[0].Rows[0]["ship_address"].ToString();
                        _Shipment_Detail_Model.so_remarks = ds.Tables[0].Rows[0]["so_remarks"].ToString();
                        _Shipment_Detail_Model.cntry_dest = ds.Tables[0].Rows[0]["cntry_dest"].ToString();
                        _Shipment_Detail_Model.cntry_origin = ds.Tables[0].Rows[0]["cntry_origin"].ToString();
                        _Shipment_Detail_Model.loading_port = ds.Tables[0].Rows[0]["loading_port"].ToString();
                        _Shipment_Detail_Model.local_port = ds.Tables[0].Rows[0]["local_port"].ToString();
                        _Shipment_Detail_Model.destination_port = ds.Tables[0].Rows[0]["destination_port"].ToString();
                        _Shipment_Detail_Model.discharge_port = ds.Tables[0].Rows[0]["discharge_port"].ToString();
                        _Shipment_Detail_Model.carrier_name = ds.Tables[0].Rows[0]["carrier_name"].ToString();
                        _Shipment_Detail_Model.carrier_no = ds.Tables[0].Rows[0]["carrier_no"].ToString();
                        _Shipment_Detail_Model.container_no = ds.Tables[0].Rows[0]["container_no"].ToString();
                        _Shipment_Detail_Model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _Shipment_Detail_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _Shipment_Detail_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _Shipment_Detail_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _Shipment_Detail_Model.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _Shipment_Detail_Model.mod_id = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _Shipment_Detail_Model.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _Shipment_Detail_Model.StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _Shipment_Detail_Model.create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _Shipment_Detail_Model.LineSealNumber = ds.Tables[0].Rows[0]["line_seal_no"].ToString();
                        _Shipment_Detail_Model.SelfSealNumber = ds.Tables[0].Rows[0]["self_seal_no"].ToString();
                        _Shipment_Detail_Model.ContainerNetWeight = ds.Tables[0].Rows[0]["cntr_net_wt"].ToString();
                        _Shipment_Detail_Model.ContainerGrossWeight = ds.Tables[0].Rows[0]["cntr_gross_wt"].ToString();
                        _Shipment_Detail_Model.EWBNNumber = ds.Tables[0].Rows[0]["gst_ewb_no"].ToString();
                        // Add On Other Detail Added by Suraj on 03-11-2023
                        _Shipment_Detail_Model.pre_carr_by = ds.Tables[0].Rows[0]["pre_carr_by"].ToString();
                        _Shipment_Detail_Model.pi_rcpt_carr = ds.Tables[0].Rows[0]["pi_rcpt_carr"].ToString();
                        _Shipment_Detail_Model.ves_fli_no = ds.Tables[0].Rows[0]["ves_fli_no"].ToString();
                        _Shipment_Detail_Model.other_ref = ds.Tables[0].Rows[0]["other_ref"].ToString();
                        _Shipment_Detail_Model.term_del_pay = ds.Tables[0].Rows[0]["term_del_pay"].ToString();
                        _Shipment_Detail_Model.des_good = ds.Tables[0].Rows[0]["des_good"].ToString();
                        _Shipment_Detail_Model.prof_detail = ds.Tables[0].Rows[0]["prof_detail"].ToString();
                        _Shipment_Detail_Model.declar = ds.Tables[0].Rows[0]["declar"].ToString();
                        _Shipment_Detail_Model.ExportersReference = ds.Tables[0].Rows[0]["ext_ref"].ToString();
                        _Shipment_Detail_Model.BuyerIfOtherThenConsignee = ds.Tables[0].Rows[0]["buyer_consig"].ToString();
                        _Shipment_Detail_Model.trade_term = ds.Tables[0].Rows[0]["trade_term"].ToString();
                        _Shipment_Detail_Model.ConsigneeAddress = ds.Tables[0].Rows[0]["consig_addr"].ToString();
                        _Shipment_Detail_Model.BuyersOrderNumberAndDate = ds.Tables[0].Rows[0]["buyer_ord_no_dt"].ToString();
                        // Add On Other Detail Added by Suraj on 03-11-2023 End
                        _Shipment_Detail_Model.TotalGrossWgt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_gr_wght"]).ToString(WeightDigit);
                        _Shipment_Detail_Model.TotalNetWgt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_net_wght"]).ToString(WeightDigit);
                        _Shipment_Detail_Model.TotalCBM = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_cbm"]).ToString(QtyDigit);
                        _Shipment_Detail_Model.Cust_term_del_pay = ds.Tables[0].Rows[0]["cust_term_deli_pay"].ToString();
                        _Shipment_Detail_Model.custom_local_port = ds.Tables[0].Rows[0]["Cust_Local_Port"].ToString();
                        _Shipment_Detail_Model.ExporterAddress = ds.Tables[0].Rows[0]["exp_addr"].ToString();
                        _Shipment_Detail_Model.ConsigneeName = ds.Tables[0].Rows[0]["consig_name"].ToString();
                        _Shipment_Detail_Model.CustomInvDate = ds.Tables[0].Rows[0]["custom_inv_dt"].ToString();
                        _Shipment_Detail_Model.PvtMark = ds.Tables[0].Rows[0]["pvt_mark"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string StatusCode = string.Empty;
                        StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();

                        if (StatusCode == "C")
                        {
                            _Shipment_Detail_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString(); 
                            _Shipment_Detail_Model.CancelFlag = true;
                            _Shipment_Detail_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _Shipment_Detail_Model.CancelFlag = false;
                        }
                        _Shipment_Detail_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _Shipment_Detail_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        if (ViewBag.AppLevel != null && _Shipment_Detail_Model.Command != "Edit")
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

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _Shipment_Detail_Model.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _Shipment_Detail_Model.BtnName = "BtnEdit";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _Shipment_Detail_Model.BtnName = "BtnEdit";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _Shipment_Detail_Model.BtnName = "BtnEdit";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _Shipment_Detail_Model.BtnName = "BtnEdit";
                                    }


                                }
                            }
                            if (StatusCode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _Shipment_Detail_Model.BtnName = "BtnEdit";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _Shipment_Detail_Model.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "SH")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _Shipment_Detail_Model.BtnName = "BtnEdit";
                                }
                                else
                                {
                                    _Shipment_Detail_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.ItemDetails = ds.Tables[1];
                        _Shipment_Detail_Model.DocumentStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _Shipment_Detail_Model.DocumentStatusCode = StatusCode;
                        ViewBag.DocumentCode = StatusCode;
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //var commCont = new CommonController(_Common_IServices);
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    if (ViewBag.DocumentCode == "SH" || ViewBag.DocumentCode == "IN")
                        //    {
                        //        ViewBag.Message = "Financial Year not Exist";
                        //    }
                        //}
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.Spmt_dt) == "TransNotAllow")
                        {
                            if (ViewBag.DocumentCode == "SH" || ViewBag.DocumentCode == "IN")
                            {
                                ViewBag.Message = "TransNotAllow";
                            }
                            ViewBag.Message = "TransNotAllow";
                        }
                        /*End to chk Financial year exist or not*/
                        _Shipment_Detail_Model.Title = title;
                        ViewBag.ItemStockBatchWise = ds.Tables[2];
                        ViewBag.ItemStockSerialWise = ds.Tables[3];
                        ViewBag.AttechmentDetails = ds.Tables[7];
                        ViewBag.SubItemDetails = ds.Tables[8];
                        ViewBag.TranspoterDetails = ds.Tables[9];
                        _Shipment_Detail_Model.hdnNumberOfPacks = ds.Tables[10].Rows[0]["TotalPackages"].ToString();
                        _Shipment_Detail_Model.DeleteCommand = null;
                        _Shipment_Detail_Model.MenuDocumentId = _Shipment_Detail_Model.DocumentMenuId;
                        _Shipment_Detail_Model.CMN_Command = _Shipment_Detail_Model.Command;
                        ViewBag.DocID = _Shipment_Detail_Model.DocumentMenuId;
                        ViewBag.TransType = _Shipment_Detail_Model.TransType;
                        ViewBag.Command = _Shipment_Detail_Model.Command;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentDetail.cshtml", _Shipment_Detail_Model);
                    }
                    else
                    {
                        ViewBag.DocumentCode = "0";
                        _Shipment_Detail_Model.DeleteCommand = null;
                        _Shipment_Detail_Model.DocumentStatus = "New";
                        _Shipment_Detail_Model.Title = title;
                        CommonPageDetails(DocumentMenuId);
                        _Shipment_Detail_Model.MenuDocumentId = _Shipment_Detail_Model.DocumentMenuId;
                        _Shipment_Detail_Model.CMN_Command = _Shipment_Detail_Model.Command;
                        ViewBag.DocID = _Shipment_Detail_Model.DocumentMenuId;
                        ViewBag.TransType = _Shipment_Detail_Model.TransType;
                        ViewBag.Command = _Shipment_Detail_Model.Command;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentDetail.cshtml", _Shipment_Detail_Model);
                    }
                }
                else
                {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    var date = DateTime.Now;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);
                    ShipmentDetail_MODEL _ShipmentDetail_MODEL = new ShipmentDetail_MODEL();
                    _ShipmentDetail_MODEL.UserID = UserID;
                    if (_urlModel != null)
                    {
                        _ShipmentDetail_MODEL.WF_Status1 = _urlModel.WF_sts1;
                        _ShipmentDetail_MODEL.Command = _urlModel.Cmd;
                        if (_urlModel.Cmd == "UpdateTransPortDetail")
                        {
                            _ShipmentDetail_MODEL.EditCommand = _urlModel.Cmd;
                        }
                        _ShipmentDetail_MODEL.BtnName = _urlModel.Btn;
                        _ShipmentDetail_MODEL.TransType = _urlModel.Trns_Typ;
                        _ShipmentDetail_MODEL.ShipmentNumber = _urlModel.Spmt_no;
                        _ShipmentDetail_MODEL.ShipmentDate = _urlModel.Spmt_dt;
                        _ShipmentDetail_MODEL.ShipMent_type = _urlModel.Sp_typ;
                        _ShipmentDetail_MODEL.DocumentMenuId = _urlModel.DocId;
                        DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                        _ShipmentDetail_MODEL.MenuDocumentId = _ShipmentDetail_MODEL.DocumentMenuId;
                    }
                    string QtyDigit = "";
                    if (_ShipmentDetail_MODEL.DocumentMenuId == "105103145120")
                    {
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"]));
                    }
                    else
                    {
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                    }

                    List<PortOfLoadingListModel> _PortOfLoadingListModel1 = new List<PortOfLoadingListModel>();
                    PortOfLoadingListModel PortOfLoadingLis = new PortOfLoadingListModel();
                    PortOfLoadingLis.POL_Name = "---Select---";
                    PortOfLoadingLis.POL_id = "0";
                    _PortOfLoadingListModel1.Add(PortOfLoadingLis);
                    _ShipmentDetail_MODEL.PortOfLoadingList = _PortOfLoadingListModel1;

                    List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
                    pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
                    _pi_rcpt_carrLis.Pi_Name = "---Select---";
                    _pi_rcpt_carrLis.Pi_id = "0";
                    _pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
                    _ShipmentDetail_MODEL.pi_rcpt_carrList = _pi_rcpt_carrListModel;

                    List<CustomerName> _CustomerNameList = new List<CustomerName>();
                    CustomerName _CustomerName = new CustomerName();
                    _CustomerName.cust_name = "---Select---";
                    _CustomerName.cust_id = "0";
                    _CustomerNameList.Add(_CustomerName);
                    _ShipmentDetail_MODEL.CustomerNameList = _CustomerNameList;

                    string CstmInvNo = GetCstmInvNo();
                    _ShipmentDetail_MODEL.custom_inv_no = CstmInvNo;

                    // for bind Trade term
                    List<trade_termList> _TermLists = new List<trade_termList>();
                    _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                    _ShipmentDetail_MODEL.TradeTermsList = _TermLists;

                    _ShipmentDetail_MODEL.TransList = GetTransporterList();
                    List<PackListNumber> _PackListListNumberList = new List<PackListNumber>();
                    PackListNumber _PackListListNumber = new PackListNumber();
                    _PackListListNumber.packing_no = "---Select---";
                    _PackListListNumber.packing_dt = "0";
                    _PackListListNumberList.Add(_PackListListNumber);
                    _ShipmentDetail_MODEL.PackListNumberList = _PackListListNumberList;
                    _ShipmentDetail_MODEL.ship_dt = DateTime.Now.ToString("yyyy-MM-dd");

                    _ShipmentDetail_MODEL.BrchID = BrchID;
                    var other = new CommonController(_Common_IServices);
                    CommonPageDetails(DocumentMenuId);
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ShipmentDetail_MODEL.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_ShipmentDetail_MODEL.TransType == "Update")
                    {
                        string ShipmentNumber = _ShipmentDetail_MODEL.ShipmentNumber;
                        string ShipmentDate = _ShipmentDetail_MODEL.ShipmentDate;
                        DataSet ds = _Shipment_ISERVICES.getShipmentDetailByShipmentNo(CompID, BrchID, UserID, ShipmentNumber, Convert.ToDateTime(ShipmentDate), DocumentMenuId);
                        _ShipmentDetail_MODEL.ship_type = ds.Tables[0].Rows[0]["ship_type"].ToString();
                        _ShipmentDetail_MODEL.custom_inv_no = ds.Tables[0].Rows[0]["custom_inv_no"].ToString();

                        GetAutoCompleteCustomerName(_ShipmentDetail_MODEL);
                        ShipmentPackingListView(_ShipmentDetail_MODEL);

                        if (_ShipmentDetail_MODEL.PackListNumberList.Find(obj => obj.packing_no == ds.Tables[0].Rows[0]["pack_no"].ToString()) == null)
                        {
                            PackListNumber _IPackListListNumber = new PackListNumber();
                            _IPackListListNumber.packing_no = ds.Tables[0].Rows[0]["pack_no"].ToString();
                            _IPackListListNumber.packing_dt = ds.Tables[0].Rows[0]["pack_no"].ToString();
                            _ShipmentDetail_MODEL.PackListNumberList.Add(_IPackListListNumber);
                        }
                        _ShipmentDetail_MODEL.curr_des = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        _ShipmentDetail_MODEL.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _ShipmentDetail_MODEL.CustomerID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _ShipmentDetail_MODEL.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _ShipmentDetail_MODEL.ship_no = ds.Tables[0].Rows[0]["ship_no"].ToString();
                        _ShipmentDetail_MODEL.ship_dt = ds.Tables[0].Rows[0]["so_dt"].ToString();
                        _ShipmentDetail_MODEL.pack_num = ds.Tables[0].Rows[0]["pack_no"].ToString();
                        _ShipmentDetail_MODEL.pack_dte = ds.Tables[0].Rows[0]["pack_dt"].ToString();
                        _ShipmentDetail_MODEL.bill_address = ds.Tables[0].Rows[0]["bill_address_name"].ToString();
                        _ShipmentDetail_MODEL.ship_address = ds.Tables[0].Rows[0]["ship_address_name"].ToString();
                        _ShipmentDetail_MODEL.bill_add_id = ds.Tables[0].Rows[0]["bill_address"].ToString();
                        _ShipmentDetail_MODEL.ship_add_id = ds.Tables[0].Rows[0]["ship_address"].ToString();
                        _ShipmentDetail_MODEL.so_remarks = ds.Tables[0].Rows[0]["so_remarks"].ToString();
                        _ShipmentDetail_MODEL.cntry_dest = ds.Tables[0].Rows[0]["cntry_dest"].ToString();
                        _ShipmentDetail_MODEL.cntry_origin = ds.Tables[0].Rows[0]["cntry_origin"].ToString();
                        _ShipmentDetail_MODEL.loading_port = ds.Tables[0].Rows[0]["loading_port"].ToString();
                        _ShipmentDetail_MODEL.local_port = ds.Tables[0].Rows[0]["local_port"].ToString();
                        _ShipmentDetail_MODEL.destination_port = ds.Tables[0].Rows[0]["destination_port"].ToString();
                        _ShipmentDetail_MODEL.discharge_port = ds.Tables[0].Rows[0]["discharge_port"].ToString();
                        _ShipmentDetail_MODEL.carrier_name = ds.Tables[0].Rows[0]["carrier_name"].ToString();
                        _ShipmentDetail_MODEL.carrier_no = ds.Tables[0].Rows[0]["carrier_no"].ToString();
                        _ShipmentDetail_MODEL.container_no = ds.Tables[0].Rows[0]["container_no"].ToString();
                        _ShipmentDetail_MODEL.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _ShipmentDetail_MODEL.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _ShipmentDetail_MODEL.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _ShipmentDetail_MODEL.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _ShipmentDetail_MODEL.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _ShipmentDetail_MODEL.mod_id = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _ShipmentDetail_MODEL.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _ShipmentDetail_MODEL.StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _ShipmentDetail_MODEL.create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _ShipmentDetail_MODEL.LineSealNumber = ds.Tables[0].Rows[0]["line_seal_no"].ToString();
                        _ShipmentDetail_MODEL.SelfSealNumber = ds.Tables[0].Rows[0]["self_seal_no"].ToString();
                        _ShipmentDetail_MODEL.ContainerNetWeight = ds.Tables[0].Rows[0]["cntr_net_wt"].ToString();
                        _ShipmentDetail_MODEL.ContainerGrossWeight = ds.Tables[0].Rows[0]["cntr_gross_wt"].ToString();
                        _ShipmentDetail_MODEL.EWBNNumber = ds.Tables[0].Rows[0]["gst_ewb_no"].ToString();
                        // Add On Other Detail Added by Suraj on 03-11-2023
                        _ShipmentDetail_MODEL.pre_carr_by = ds.Tables[0].Rows[0]["pre_carr_by"].ToString();
                        _ShipmentDetail_MODEL.pi_rcpt_carr = ds.Tables[0].Rows[0]["pi_rcpt_carr"].ToString();
                        _ShipmentDetail_MODEL.ves_fli_no = ds.Tables[0].Rows[0]["ves_fli_no"].ToString();
                        _ShipmentDetail_MODEL.other_ref = ds.Tables[0].Rows[0]["other_ref"].ToString();
                        _ShipmentDetail_MODEL.term_del_pay = ds.Tables[0].Rows[0]["term_del_pay"].ToString();
                        _ShipmentDetail_MODEL.des_good = ds.Tables[0].Rows[0]["des_good"].ToString();
                        _ShipmentDetail_MODEL.prof_detail = ds.Tables[0].Rows[0]["prof_detail"].ToString();
                        _ShipmentDetail_MODEL.declar = ds.Tables[0].Rows[0]["declar"].ToString();
                        _ShipmentDetail_MODEL.ExportersReference = ds.Tables[0].Rows[0]["ext_ref"].ToString();
                        _ShipmentDetail_MODEL.BuyerIfOtherThenConsignee = ds.Tables[0].Rows[0]["buyer_consig"].ToString();
                        _ShipmentDetail_MODEL.trade_term = ds.Tables[0].Rows[0]["trade_term"].ToString();
                        _ShipmentDetail_MODEL.ConsigneeAddress = ds.Tables[0].Rows[0]["consig_addr"].ToString();
                        _ShipmentDetail_MODEL.BuyersOrderNumberAndDate = ds.Tables[0].Rows[0]["buyer_ord_no_dt"].ToString();
                        // Add On Other Detail Added by Suraj on 03-11-2023 End
                        _ShipmentDetail_MODEL.TotalGrossWgt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_gr_wght"]).ToString(QtyDigit);
                        _ShipmentDetail_MODEL.TotalNetWgt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_net_wght"]).ToString(QtyDigit);
                        _ShipmentDetail_MODEL.TotalCBM = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_cbm"]).ToString(QtyDigit);
                        _ShipmentDetail_MODEL.Cust_term_del_pay = ds.Tables[0].Rows[0]["cust_term_deli_pay"].ToString();
                        _ShipmentDetail_MODEL.custom_local_port = ds.Tables[0].Rows[0]["Cust_Local_Port"].ToString();
                        _ShipmentDetail_MODEL.ExporterAddress = ds.Tables[0].Rows[0]["exp_addr"].ToString();
                        _ShipmentDetail_MODEL.ConsigneeName = ds.Tables[0].Rows[0]["consig_name"].ToString();
                        _ShipmentDetail_MODEL.CustomInvDate = ds.Tables[0].Rows[0]["custom_inv_dt"].ToString();
                        _ShipmentDetail_MODEL.PvtMark = ds.Tables[0].Rows[0]["pvt_mark"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string StatusCode = string.Empty;
                        ViewBag.Approve_id = approval_id;
                        StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (StatusCode == "C")
                        {
                            _ShipmentDetail_MODEL.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _ShipmentDetail_MODEL.CancelFlag = true;
                            _ShipmentDetail_MODEL.BtnName = "Refresh";
                        }
                        else
                        {
                            _ShipmentDetail_MODEL.CancelFlag = false;
                        }
                        _ShipmentDetail_MODEL.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _ShipmentDetail_MODEL.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        if (ViewBag.AppLevel != null && _ShipmentDetail_MODEL.Command != "Edit")
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

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _ShipmentDetail_MODEL.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                                    }


                                }
                            }
                            if (StatusCode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "SH")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _ShipmentDetail_MODEL.BtnName = "BtnEdit";

                                }
                                else
                                {
                                    _ShipmentDetail_MODEL.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.ItemDetails = ds.Tables[1];
                        _ShipmentDetail_MODEL.DocumentStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _ShipmentDetail_MODEL.DocumentStatusCode = StatusCode;
                        ViewBag.DocumentCode = StatusCode;
                        CommonPageDetails(DocumentMenuId);
                        _ShipmentDetail_MODEL.Title = title;
                        ViewBag.ItemStockBatchWise = ds.Tables[2];
                        ViewBag.ItemStockSerialWise = ds.Tables[3];
                        ViewBag.AttechmentDetails = ds.Tables[7];
                        ViewBag.SubItemDetails = ds.Tables[8];
                        ViewBag.TranspoterDetails = ds.Tables[9];
                        _ShipmentDetail_MODEL.hdnNumberOfPacks = ds.Tables[10].Rows[0]["TotalPackages"].ToString();

                        _ShipmentDetail_MODEL.DeleteCommand = null;
                        _ShipmentDetail_MODEL.CMN_Command = _ShipmentDetail_MODEL.Command;
                        ViewBag.DocID = _ShipmentDetail_MODEL.DocumentMenuId;
                        ViewBag.DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                        ViewBag.TransType = _ShipmentDetail_MODEL.TransType;
                        ViewBag.Command = _ShipmentDetail_MODEL.Command;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentDetail.cshtml", _ShipmentDetail_MODEL);
                    }
                    else
                    {
                        ViewBag.DocumentCode = "0";
                        _ShipmentDetail_MODEL.DeleteCommand = null;
                        _ShipmentDetail_MODEL.DocumentStatus = "New";
                        CommonPageDetails(DocumentMenuId);
                        _ShipmentDetail_MODEL.Title = title;
                        _ShipmentDetail_MODEL.CMN_Command = _ShipmentDetail_MODEL.Command;
                        ViewBag.DocID = _ShipmentDetail_MODEL.DocumentMenuId;
                        ViewBag.TransType = _ShipmentDetail_MODEL.TransType;
                        ViewBag.Command = _ShipmentDetail_MODEL.Command;
                        ViewBag.DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentDetail.cshtml", _ShipmentDetail_MODEL);
                    }
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetPortOfLoadingList()
        {
            try
            {
                string OrderType = string.Empty;
                JsonResult DataRows = null;
                DataTable PackListNumberDs = new DataTable();
                PackListNumberDs = _Shipment_ISERVICES.PortOfLoadingList();
                DataRow Drow = PackListNumberDs.NewRow();
                Drow[1] = "---Select---";
                Drow[0] = "0";
                Drow[2] = "0";
                PackListNumberDs.Rows.InsertAt(Drow, 0);
                DataRows = Json(JsonConvert.SerializeObject(PackListNumberDs));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetPlOfReceiptByPreCarrierList()
        {
            try
            {
                string OrderType = string.Empty;
                JsonResult DataRows = null;
                DataTable PackListNumberDs = new DataTable();
                PackListNumberDs = _Shipment_ISERVICES.PlOfReceiptByPreCarrierList();
                DataRow Drow = PackListNumberDs.NewRow();
                Drow[1] = "---Select---";
                Drow[0] = "0";
                Drow[2] = "0";
                PackListNumberDs.Rows.InsertAt(Drow, 0);
                DataRows = Json(JsonConvert.SerializeObject(PackListNumberDs));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType,string Mailerror)
        {
            //Session["Message"] = "";
            ShipmentDetail_MODEL _ShipmentDetail_MODEL = new ShipmentDetail_MODEL();
            UrlModel URLModel = new UrlModel();
            _ShipmentDetail_MODEL.Message = "";
            var a = TrancType.Split(',');
            _ShipmentDetail_MODEL.ShipmentNumber = a[0].Trim();
            _ShipmentDetail_MODEL.ShipmentDate = a[1].Trim();
            _ShipmentDetail_MODEL.DocumentMenuId = a[2].Trim();
            var WF_status1 = a[3].Trim();
            if (WF_status1 != null && WF_status1 != "")
            {
                _ShipmentDetail_MODEL.WF_Status1 = a[3].Trim();
                URLModel.WF_sts1 = a[3].Trim();
            }
            _ShipmentDetail_MODEL.ShipMent_type = a[4].Trim();
            _ShipmentDetail_MODEL.TransType = "Update";
            _ShipmentDetail_MODEL.Message = Mailerror;
            _ShipmentDetail_MODEL.BtnName = "BtnEdit";
            TempData["ModelData"] = _ShipmentDetail_MODEL;
            URLModel.Spmt_no = a[0].Trim();
            URLModel.Spmt_dt = a[1].Trim();
            URLModel.Trns_Typ = "Update";
            URLModel.Btn = _ShipmentDetail_MODEL.BtnName;
            URLModel.DocId = a[2].Trim();
            URLModel.Sp_typ = a[4].Trim();
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ShipmentDetail", URLModel);
        }
        public ActionResult GetShipmentList(string docid, string status)
        {
            ShipmentList_MODEL _DashBord = new ShipmentList_MODEL();
            _DashBord.DocumentMenuId = docid;
            _DashBord.WF_Status = status;
            if (_DashBord.DocumentMenuId == "105103135")
            {
                return RedirectToAction("ShipmentListD", _DashBord);
            }
            else if (_DashBord.DocumentMenuId == "105103145120")
            {
                return RedirectToAction("ShipmentListE", _DashBord);
            }
            else
            {
                return RedirectToAction("ShipmentList", _DashBord);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShipmentSave(ShipmentDetail_MODEL _ShipmentDetail_MODEL, string dn_no, string command, HttpPostedFileBase[] ShipmentFiles)
        {
            try
            {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ShipmentDetail_MODEL.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                if (_ShipmentDetail_MODEL.DeleteCommand == "Edit")
                {
                    command = "Edit";
                }
                if (command == null)
                {
                    if (_ShipmentDetail_MODEL.EditCommand == "UpdateTransPortDetail")
                    {
                        command = "UpdateTransPortDetail";
                    }
                }
                switch (command)
                {
                    case "AddNewDeliveryNote":
                        UrlModel URLModel = new UrlModel();
                        ShipmentDetail_MODEL _ShipmentDetailAddNew_MODEL = new ShipmentDetail_MODEL();
                        _ShipmentDetailAddNew_MODEL.Message = "New";
                        _ShipmentDetailAddNew_MODEL.DocumentStatus = "D";
                        _ShipmentDetailAddNew_MODEL.BtnName = "BtnAddNew";
                        _ShipmentDetailAddNew_MODEL.TransType = "Save";
                        _ShipmentDetailAddNew_MODEL.Command = "New";
                        _ShipmentDetailAddNew_MODEL.ShipMent_type = _ShipmentDetail_MODEL.ShipMent_type;
                        _ShipmentDetailAddNew_MODEL.DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                        TempData["ModelData"] = _ShipmentDetailAddNew_MODEL;
                        URLModel.DocId = _ShipmentDetail_MODEL.DocumentMenuId;
                        URLModel.Trns_Typ = "Save";
                        URLModel.Btn = "BtnAddNew";
                        URLModel.Cmd = "New";
                        URLModel.Sp_typ = _ShipmentDetailAddNew_MODEL.ShipMent_type;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_ShipmentDetail_MODEL.ship_no))
                                return RedirectToAction("EditShipment", new { ShipmentNumber = _ShipmentDetail_MODEL.ship_no, ShipmentDate = _ShipmentDetail_MODEL.ship_dt, ListFilterData = _ShipmentDetail_MODEL.ListFilterData1, WF_Status = _ShipmentDetail_MODEL.WFStatus, Docid = _ShipmentDetail_MODEL.DocumentMenuId, ShipMent_type = _ShipmentDetail_MODEL.ship_type });
                            else
                                _ShipmentDetailAddNew_MODEL.Command = "Refresh";
                            _ShipmentDetailAddNew_MODEL.TransType = "Refresh";
                            _ShipmentDetailAddNew_MODEL.BtnName = "Refresh";
                            _ShipmentDetailAddNew_MODEL.DocumentStatus = null;
                            _ShipmentDetailAddNew_MODEL.DocId = _ShipmentDetail_MODEL.DocumentMenuId;
                            TempData["ModelData"] = _ShipmentDetailAddNew_MODEL;
                            return RedirectToAction("ShipmentDetail", _ShipmentDetailAddNew_MODEL);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ShipmentDetail", URLModel);
                    case "UpdateTransPortDetail":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditShipment", new { ShipmentNumber = _ShipmentDetail_MODEL.ship_no, ShipmentDate = _ShipmentDetail_MODEL.ship_dt, ListFilterData = _ShipmentDetail_MODEL.ListFilterData1, WF_Status = _ShipmentDetail_MODEL.WFStatus, Docid = _ShipmentDetail_MODEL.DocumentMenuId, ShipMent_type = _ShipmentDetail_MODEL.ship_type });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string ShpDate = _ShipmentDetail_MODEL.ship_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, ShpDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditShipment", new { ShipmentNumber = _ShipmentDetail_MODEL.ship_no, ShipmentDate = _ShipmentDetail_MODEL.ship_dt, ListFilterData = _ShipmentDetail_MODEL.ListFilterData1, WF_Status = _ShipmentDetail_MODEL.WFStatus, Docid = _ShipmentDetail_MODEL.DocumentMenuId, ShipMent_type = _ShipmentDetail_MODEL.ship_type });
                        }
                        /*End to chk Financial year exist or not*/
                        UrlModel URLeditModel2 = new UrlModel();
                        _ShipmentDetail_MODEL.TransType = "Update";
                        _ShipmentDetail_MODEL.Command = command;
                        _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                        _ShipmentDetail_MODEL.ShipmentNumber = _ShipmentDetail_MODEL.ship_no;
                        _ShipmentDetail_MODEL.ShipmentDate = _ShipmentDetail_MODEL.ship_dt;
                        _ShipmentDetail_MODEL.DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                        _ShipmentDetail_MODEL.ShipMent_type = _ShipmentDetail_MODEL.ShipMent_type;
                        //_ShipmentDetail_MODEL.CustType = _ShipmentDetail_MODEL.CustType;
                        TempData["ModelData"] = _ShipmentDetail_MODEL;
                        URLeditModel2.Trns_Typ = _ShipmentDetail_MODEL.TransType;
                        URLeditModel2.Cmd = command;
                        URLeditModel2.Btn = "BtnEdit";
                        URLeditModel2.Spmt_no = _ShipmentDetail_MODEL.ShipmentNumber;
                        URLeditModel2.DocId = _ShipmentDetail_MODEL.DocumentMenuId;
                        URLeditModel2.Sp_typ = _ShipmentDetail_MODEL.ShipMent_type;
                        URLeditModel2.Spmt_dt = _ShipmentDetail_MODEL.ShipmentDate;
                        TempData["ListFilterData"] = _ShipmentDetail_MODEL.ListFilterData1;
                        return RedirectToAction("ShipmentDetail", URLeditModel2);

                    case "Edit":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditShipment", new { ShipmentNumber = _ShipmentDetail_MODEL.ship_no, ShipmentDate = _ShipmentDetail_MODEL.ship_dt, ListFilterData = _ShipmentDetail_MODEL.ListFilterData1, WF_Status = _ShipmentDetail_MODEL.WFStatus, Docid = _ShipmentDetail_MODEL.DocumentMenuId, ShipMent_type = _ShipmentDetail_MODEL.ship_type });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string ShpDate1 = _ShipmentDetail_MODEL.ship_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, ShpDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditShipment", new { ShipmentNumber = _ShipmentDetail_MODEL.ship_no, ShipmentDate = _ShipmentDetail_MODEL.ship_dt, ListFilterData = _ShipmentDetail_MODEL.ListFilterData1, WF_Status = _ShipmentDetail_MODEL.WFStatus, Docid = _ShipmentDetail_MODEL.DocumentMenuId, ShipMent_type = _ShipmentDetail_MODEL.ship_type });
                        }
                        /*End to chk Financial year exist or not*/
                        UrlModel URLeditModel = new UrlModel();
                        if (CheckSaleInvoiceAgainstShipment(_ShipmentDetail_MODEL.ship_no, _ShipmentDetail_MODEL.ship_dt) == "Used")
                        {
                            //Session["Message"] = "Used";
                            _ShipmentDetail_MODEL.Message = "Used";
                            _ShipmentDetail_MODEL.TransType = "Update";
                            _ShipmentDetail_MODEL.Command = "Refresh";
                            _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                            _ShipmentDetail_MODEL.ShipmentNumber = _ShipmentDetail_MODEL.ship_no;
                            _ShipmentDetail_MODEL.ShipmentDate = _ShipmentDetail_MODEL.ship_dt;
                            _ShipmentDetail_MODEL.DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                            _ShipmentDetail_MODEL.ShipMent_type = _ShipmentDetail_MODEL.ShipMent_type;
                            //_ShipmentDetail_MODEL.CustType = _ShipmentDetail_MODEL.CustType;
                            TempData["ModelData"] = _ShipmentDetail_MODEL;
                            URLeditModel.Trns_Typ = _ShipmentDetail_MODEL.TransType;
                            URLeditModel.Cmd = "Refresh";
                            URLeditModel.Btn = "BtnEdit";
                            URLeditModel.Spmt_no = _ShipmentDetail_MODEL.ShipmentNumber;
                            URLeditModel.DocId = _ShipmentDetail_MODEL.DocumentMenuId;
                            URLeditModel.Sp_typ = _ShipmentDetail_MODEL.ShipMent_type;
                            URLeditModel.Spmt_dt = _ShipmentDetail_MODEL.ShipmentDate;
                        }
                        else
                        {
                            _ShipmentDetail_MODEL.TransType = "Update";
                            _ShipmentDetail_MODEL.Command = command;
                            _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                            _ShipmentDetail_MODEL.ShipmentNumber = _ShipmentDetail_MODEL.ship_no;
                            _ShipmentDetail_MODEL.ShipmentDate = _ShipmentDetail_MODEL.ship_dt;
                            _ShipmentDetail_MODEL.DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                            _ShipmentDetail_MODEL.ShipMent_type = _ShipmentDetail_MODEL.ShipMent_type;
                            //_ShipmentDetail_MODEL.CustType = _ShipmentDetail_MODEL.CustType;
                            TempData["ModelData"] = _ShipmentDetail_MODEL;
                            URLeditModel.Trns_Typ = _ShipmentDetail_MODEL.TransType;
                            URLeditModel.Cmd = command;
                            URLeditModel.Btn = "BtnEdit";
                            URLeditModel.Spmt_no = _ShipmentDetail_MODEL.ShipmentNumber;
                            URLeditModel.DocId = _ShipmentDetail_MODEL.DocumentMenuId;
                            URLeditModel.Sp_typ = _ShipmentDetail_MODEL.ShipMent_type;
                            URLeditModel.Spmt_dt = _ShipmentDetail_MODEL.ShipmentDate;
                        }
                        TempData["ListFilterData"] = _ShipmentDetail_MODEL.ListFilterData1;
                        return RedirectToAction("ShipmentDetail", URLeditModel);

                    case "Delete":
                        _ShipmentDetail_MODEL.Command = command;
                        ShipmentDelete(_ShipmentDetail_MODEL, command);
                        ShipmentDetail_MODEL _Modeldelete = new ShipmentDetail_MODEL();
                        _Modeldelete.Command = "Refresh";
                        _Modeldelete.Message = "Deleted";
                        _Modeldelete.TransType = "New";
                        _Modeldelete.BtnName = "BtnDelete";
                        _Modeldelete.DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                        _Modeldelete.ShipMent_type = _ShipmentDetail_MODEL.ShipMent_type;
                        UrlModel URLModeldelete = new UrlModel();
                        URLModeldelete.Cmd = _Modeldelete.Command;
                        URLModeldelete.Trns_Typ = _Modeldelete.TransType;
                        URLModeldelete.DocId = _Modeldelete.DocumentMenuId;
                        URLModeldelete.Sp_typ = _Modeldelete.ShipMent_type;
                        URLModeldelete.Btn = _Modeldelete.BtnName;
                        TempData["ModelData"] = _Modeldelete;
                        TempData["ListFilterData"] = _ShipmentDetail_MODEL.ListFilterData1;
                        return RedirectToAction("ShipmentDetail", URLModeldelete);

                    case "Save":
                        _ShipmentDetail_MODEL.Command = command;
                        SaveUpdateShipment(_ShipmentDetail_MODEL, ShipmentFiles);
                        if (_ShipmentDetail_MODEL.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_ShipmentDetail_MODEL.Message == "Error")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_ShipmentDetail_MODEL.Message == "DocModify")
                        {
                            List<CustomerName> _CustList = new List<CustomerName>();

                            _CustList.Insert(0, new CustomerName() { cust_name = _ShipmentDetail_MODEL.SO_CustName, cust_id = _ShipmentDetail_MODEL.CustomerID });
                            _ShipmentDetail_MODEL.CustomerNameList = _CustList;


                            _ShipmentDetail_MODEL.ship_dt = DateTime.Now.ToString("yyyy-MM-dd");

                            List<PackListNumber> _PackListNumber = new List<PackListNumber>();
                            PackListNumber _PackList_Number = new PackListNumber();
                            _PackList_Number.packing_no = _ShipmentDetail_MODEL.pack_num;
                            _PackList_Number.packing_dt = "0";
                            _PackListNumber.Add(_PackList_Number);
                            _ShipmentDetail_MODEL.PackListNumberList = _PackListNumber;
                            _ShipmentDetail_MODEL.BrchID = BrchID;
                            CommonPageDetails(DocumentMenuId);
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            _ShipmentDetail_MODEL.MenuDocumentId = DocumentMenuId;
                            _ShipmentDetail_MODEL.TransList = GetTransporterList();

                            List<trade_termList> _TermLists = new List<trade_termList>();
                            _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                            _ShipmentDetail_MODEL.TradeTermsList = _TermLists;

                            List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
                            pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
                            _pi_rcpt_carrLis.Pi_Name = "---Select---";
                            _pi_rcpt_carrLis.Pi_id = "0";
                            _pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
                            _ShipmentDetail_MODEL.pi_rcpt_carrList = _pi_rcpt_carrListModel;

                            List<PortOfLoadingListModel> _PortOfLoadingListModel = new List<PortOfLoadingListModel>();
                            PortOfLoadingListModel PortOfLoadingLis = new PortOfLoadingListModel();
                            PortOfLoadingLis.POL_Name = "---Select---";
                            PortOfLoadingLis.POL_id = "0";
                            _PortOfLoadingListModel.Add(PortOfLoadingLis);
                            _ShipmentDetail_MODEL.PortOfLoadingList = _PortOfLoadingListModel;

                            _ShipmentDetail_MODEL.pack_num = _ShipmentDetail_MODEL.pack_num;
                            _ShipmentDetail_MODEL.pack_dte = _ShipmentDetail_MODEL.pack_dte;
                            _ShipmentDetail_MODEL.curr_des = _ShipmentDetail_MODEL.curr_des;
                            _ShipmentDetail_MODEL.curr_id = _ShipmentDetail_MODEL.curr_id;
                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.ItemStockBatchWise = ViewData["BatchDetails"];
                            ViewBag.ItemStockSerialWise = ViewData["SerialDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetails"];
                            ViewBag.TranspoterDetails = ViewData["TranspoterDetails"];
                            _ShipmentDetail_MODEL.BtnName = "Refresh";
                            _ShipmentDetail_MODEL.Command = "Refresh";
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentDetail.cshtml", _ShipmentDetail_MODEL);
                        }
                        else if (_ShipmentDetail_MODEL.Message == "DuplicateCustomInvNo")
                        {
                            List<CustomerName> _CustList = new List<CustomerName>();

                            _CustList.Insert(0, new CustomerName() { cust_name = _ShipmentDetail_MODEL.SO_CustName, cust_id = _ShipmentDetail_MODEL.CustomerID });
                            _ShipmentDetail_MODEL.CustomerNameList = _CustList;

                            string UserID = "";
                            if (Session["UserID"] != null)
                            {
                                UserID = Session["UserID"].ToString();
                            }
                            //_ShipmentDetail_MODEL.ship_dt = DateTime.Now.ToString("yyyy-MM-dd");

                            List<PackListNumber> _PackListNumber = new List<PackListNumber>();
                            PackListNumber _PackList_Number = new PackListNumber();
                            _PackList_Number.packing_no = _ShipmentDetail_MODEL.pack_num;
                            _PackList_Number.packing_dt = _ShipmentDetail_MODEL.pack_num;
                            _PackListNumber.Add(_PackList_Number);
                            _ShipmentDetail_MODEL.PackListNumberList = _PackListNumber;
                            _ShipmentDetail_MODEL.BrchID = BrchID;
                            CommonPageDetails(DocumentMenuId);
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            _ShipmentDetail_MODEL.MenuDocumentId = DocumentMenuId;
                            _ShipmentDetail_MODEL.TransList = GetTransporterList();

                            List<trade_termList> _TermLists = new List<trade_termList>();
                            _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                            _ShipmentDetail_MODEL.TradeTermsList = _TermLists;

                            List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
                            pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
                            _pi_rcpt_carrLis.Pi_Name = "---Select---";
                            _pi_rcpt_carrLis.Pi_id = "0";
                            _pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
                            _ShipmentDetail_MODEL.pi_rcpt_carrList = _pi_rcpt_carrListModel;

                            List<PortOfLoadingListModel> _PortOfLoadingListModel = new List<PortOfLoadingListModel>();
                            PortOfLoadingListModel PortOfLoadingLis = new PortOfLoadingListModel();
                            PortOfLoadingLis.POL_Name = "---Select---";
                            PortOfLoadingLis.POL_id = "0";
                            _PortOfLoadingListModel.Add(PortOfLoadingLis);
                            _ShipmentDetail_MODEL.PortOfLoadingList = _PortOfLoadingListModel;

                            if (_ShipmentDetail_MODEL.ship_no != null && _ShipmentDetail_MODEL.ship_dt != null)
                            {
                                string ShipmentNumber = _ShipmentDetail_MODEL.ship_no;
                                string ShipmentDate = _ShipmentDetail_MODEL.ship_dt;
                                DataSet ds = _Shipment_ISERVICES.getShipmentDetailByShipmentNo(CompID, BrchID, UserID, ShipmentNumber, Convert.ToDateTime(ShipmentDate), DocumentMenuId);
                                _ShipmentDetail_MODEL.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                                _ShipmentDetail_MODEL.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                _ShipmentDetail_MODEL.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                                _ShipmentDetail_MODEL.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                                _ShipmentDetail_MODEL.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                _ShipmentDetail_MODEL.mod_id = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                _ShipmentDetail_MODEL.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                                _ShipmentDetail_MODEL.StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                                _ShipmentDetail_MODEL.create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                                if (_ShipmentDetail_MODEL.PackListNumberList.Find(obj => obj.packing_no == ds.Tables[0].Rows[0]["pack_no"].ToString()) == null)
                                {
                                    PackListNumber _IPackListListNumber = new PackListNumber();
                                    _IPackListListNumber.packing_no = ds.Tables[0].Rows[0]["pack_no"].ToString();
                                    _IPackListListNumber.packing_dt = ds.Tables[0].Rows[0]["pack_no"].ToString();
                                    _ShipmentDetail_MODEL.PackListNumberList.Add(_IPackListListNumber);
                                }
                                _ShipmentDetail_MODEL.CustomerID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                                _ShipmentDetail_MODEL.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                                _ShipmentDetail_MODEL.ship_no = ds.Tables[0].Rows[0]["ship_no"].ToString();
                                _ShipmentDetail_MODEL.ship_dt = ds.Tables[0].Rows[0]["so_dt"].ToString();
                                _ShipmentDetail_MODEL.pack_num = ds.Tables[0].Rows[0]["pack_no"].ToString();
                                _ShipmentDetail_MODEL.pack_dte = ds.Tables[0].Rows[0]["pack_dt"].ToString();
                            }
                            //_PackList_Number.packing_no = _ShipmentDetail_MODEL.pack_num;
                            //_PackList_Number.packing_dt = "0";
                            //_PackListNumber.Add(_PackList_Number);
                            //_ShipmentDetail_MODEL.PackListNumberList = _PackListNumber;
                            //_ShipmentDetail_MODEL.pack_num = _ShipmentDetail_MODEL.pack_num;
                            //_ShipmentDetail_MODEL.pack_dte = _ShipmentDetail_MODEL.pack_dte;
                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.ItemStockBatchWise = ViewData["BatchDetails"];
                            ViewBag.ItemStockSerialWise = ViewData["SerialDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetails"];
                            ViewBag.TranspoterDetails = ViewData["TranspoterDetails"];
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentDetail.cshtml", _ShipmentDetail_MODEL);
                        }
                        else
                        {
                            UrlModel URL_SaveModel = new UrlModel();
                            TempData["ModelData"] = _ShipmentDetail_MODEL;
                            URL_SaveModel.Cmd = _ShipmentDetail_MODEL.Command;
                            URL_SaveModel.Trns_Typ = _ShipmentDetail_MODEL.TransType;
                            URL_SaveModel.DocId = _ShipmentDetail_MODEL.DocumentMenuId;
                            URL_SaveModel.Sp_typ = _ShipmentDetail_MODEL.ShipMent_type;
                            URL_SaveModel.Btn = _ShipmentDetail_MODEL.BtnName;
                            URL_SaveModel.Spmt_no = _ShipmentDetail_MODEL.ShipmentNumber;
                            URL_SaveModel.Spmt_dt = _ShipmentDetail_MODEL.ShipmentDate;
                            TempData["ModelData"] = _ShipmentDetail_MODEL;
                            TempData["ListFilterData"] = _ShipmentDetail_MODEL.ListFilterData1;
                            return RedirectToAction("ShipmentDetail", URL_SaveModel);
                        }
                    case "Forward":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditShipment", new { ShipmentNumber = _ShipmentDetail_MODEL.ship_no, ShipmentDate = _ShipmentDetail_MODEL.ship_dt, ListFilterData = _ShipmentDetail_MODEL.ListFilterData1, WF_Status = _ShipmentDetail_MODEL.WFStatus, Docid = _ShipmentDetail_MODEL.DocumentMenuId, ShipMent_type = _ShipmentDetail_MODEL.ship_type });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string ShpDate3 = _ShipmentDetail_MODEL.ship_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, ShpDate3) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditShipment", new { ShipmentNumber = _ShipmentDetail_MODEL.ship_no, ShipmentDate = _ShipmentDetail_MODEL.ship_dt, ListFilterData = _ShipmentDetail_MODEL.ListFilterData1, WF_Status = _ShipmentDetail_MODEL.WFStatus, Docid = _ShipmentDetail_MODEL.DocumentMenuId, ShipMent_type = _ShipmentDetail_MODEL.ship_type });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditShipment", new { ShipmentNumber = _ShipmentDetail_MODEL.ship_no, ShipmentDate = _ShipmentDetail_MODEL.ship_dt, ListFilterData = _ShipmentDetail_MODEL.ListFilterData1, WF_Status = _ShipmentDetail_MODEL.WFStatus, Docid = _ShipmentDetail_MODEL.DocumentMenuId, ShipMent_type = _ShipmentDetail_MODEL.ship_type });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string ShpDate4 = _ShipmentDetail_MODEL.ship_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, ShpDate4) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditShipment", new { ShipmentNumber = _ShipmentDetail_MODEL.ship_no, ShipmentDate = _ShipmentDetail_MODEL.ship_dt, ListFilterData = _ShipmentDetail_MODEL.ListFilterData1, WF_Status = _ShipmentDetail_MODEL.WFStatus, Docid = _ShipmentDetail_MODEL.DocumentMenuId, ShipMent_type = _ShipmentDetail_MODEL.ship_type });
                        }
                        /*End to chk Financial year exist or not*/
                        _ShipmentDetail_MODEL.Command = command;
                        ship_no = _ShipmentDetail_MODEL.ship_no;
                        ShipmentApprove(_ShipmentDetail_MODEL, _ShipmentDetail_MODEL.ship_no, _ShipmentDetail_MODEL.ship_dt, _ShipmentDetail_MODEL.ship_type,
                           "", "", "", command, "", "", "");
                        TempData["ModelData"] = _ShipmentDetail_MODEL;
                        UrlModel URLModelApprove = new UrlModel();
                        URLModelApprove.Cmd = _ShipmentDetail_MODEL.Command;
                        URLModelApprove.Trns_Typ = _ShipmentDetail_MODEL.TransType;
                        URLModelApprove.Spmt_no = _ShipmentDetail_MODEL.ShipmentNumber;
                        URLModelApprove.Spmt_dt = _ShipmentDetail_MODEL.ShipmentDate;
                        URLModelApprove.DocId = _ShipmentDetail_MODEL.DocumentMenuId;
                        URLModelApprove.Sp_typ = _ShipmentDetail_MODEL.ShipMent_type;
                        TempData["ListFilterData"] = _ShipmentDetail_MODEL.ListFilterData1;
                        return RedirectToAction("ShipmentDetail", URLModelApprove);

                    case "Refresh":
                        UrlModel URLModelRefresh = new UrlModel();
                        ShipmentDetail_MODEL ___ShipmentDetail_MODELRefresh = new ShipmentDetail_MODEL();
                        ___ShipmentDetail_MODELRefresh.BtnName = "Refresh";
                        ___ShipmentDetail_MODELRefresh.Command = command;
                        ___ShipmentDetail_MODELRefresh.TransType = "Save";
                        ___ShipmentDetail_MODELRefresh.DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                        ___ShipmentDetail_MODELRefresh.ShipMent_type = _ShipmentDetail_MODEL.ShipMent_type;
                        TempData["ModelData"] = ___ShipmentDetail_MODELRefresh;
                        URLModelRefresh.DocId = ___ShipmentDetail_MODELRefresh.DocumentMenuId;
                        URLModelRefresh.Btn = ___ShipmentDetail_MODELRefresh.BtnName;
                        URLModelRefresh.Cmd = ___ShipmentDetail_MODELRefresh.Command;
                        URLModelRefresh.Trns_Typ = ___ShipmentDetail_MODELRefresh.TransType;
                        URLModelRefresh.Sp_typ = _ShipmentDetail_MODEL.ShipMent_type;
                        TempData["ListFilterData"] = _ShipmentDetail_MODEL.ListFilterData1;
                        return RedirectToAction("ShipmentDetail", URLModelRefresh);

                    case "Print":
                        return GenratePdfFile(_ShipmentDetail_MODEL);
                    case "BacktoList":
                        ShipmentList_MODEL _ShipmentList_MODEL = new ShipmentList_MODEL();
                        _ShipmentList_MODEL.WF_Status = _ShipmentDetail_MODEL.WF_Status1;
                        TempData["ListFilterData"] = _ShipmentDetail_MODEL.ListFilterData1;
                        if (_ShipmentDetail_MODEL.DocumentMenuId != null)
                        {
                            if (_ShipmentDetail_MODEL.DocumentMenuId == "105103135")
                            {
                                return RedirectToAction("ShipmentListD", _ShipmentList_MODEL);
                            }
                            else if (_ShipmentDetail_MODEL.DocumentMenuId == "105103145120")
                            {
                                return RedirectToAction("ShipmentListE", _ShipmentList_MODEL);
                            }
                        }
                        return RedirectToAction("ShipmentListD", _ShipmentList_MODEL);
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
        public void GetStatus(ShipmentList_MODEL _ShipmentList_MODEL)
        {
            try
            {
                DocumentMenuId = _ShipmentList_MODEL.DocumentMenuId;
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103135")
                //    {
                //        DocumentMenuId = "105103135";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145120")
                //    {
                //        DocumentMenuId = "105103145120";
                //    }
                //}
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_code = x.status_id, status_name = x.status_name });
                _ShipmentList_MODEL.StatusList = listOfStatus;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

            }
        }
        public ActionResult GetAutoCompleteCustomerName(ShipmentDetail_MODEL _Shipment_MODEL)
        {
            string CustName = string.Empty;
            string CustType = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_Shipment_MODEL.filterCustomerName))
                {
                    CustName = "0";
                }
                else
                {
                    CustName = _Shipment_MODEL.filterCustomerName;
                    //  _DomesticPackingDetail_Model.Customer_type;
                }
                if (Session["BranchId"] == null)
                {
                    RedirectToAction("Index", "Home");
                }
                else
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (_Shipment_MODEL.DocumentMenuId == "105103135")
                {
                    CustType = "D";
                }
                else if (_Shipment_MODEL.DocumentMenuId == "105103145120")
                {
                    CustType = "E";
                }
                else if (_Shipment_MODEL.ShipMent_type != null)
                {
                    CustType = _Shipment_MODEL.ShipMent_type;
                }

                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103135")
                //    {
                //        CustType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145120")
                //    {
                //        CustType = "E";
                //    }
                //}

                CustList = _Shipment_ISERVICES.GetCustomerList(Comp_ID, CustName, BrchID, CustType);

                List<CustomerName> _CustomerNameList = new List<CustomerName>();
                foreach (var dr in CustList)
                {
                    CustomerName _CustomerName = new CustomerName();
                    _CustomerName.cust_id = dr.Key;
                    _CustomerName.cust_name = dr.Value;
                    _CustomerNameList.Add(_CustomerName);
                }
                _Shipment_MODEL.CustomerNameList = _CustomerNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerAddress(string CustID)
        {
            try
            {
                _ShipmentDetail_MODEL = new ShipmentDetail_MODEL();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                ds = _Shipment_ISERVICES.getCustomerAddress(CompID, CustID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _ShipmentDetail_MODEL.ship_address = ds.Tables[0].Rows[0]["ShippingAddress"].ToString();
                    _ShipmentDetail_MODEL.bill_address = ds.Tables[0].Rows[0]["BillingAddress"].ToString();
                    _ShipmentDetail_MODEL.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                    _ShipmentDetail_MODEL.ship_add_id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                }
                else
                {


                    _ShipmentDetail_MODEL.ship_address = string.Empty;
                    _ShipmentDetail_MODEL.bill_address = string.Empty;

                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    _ShipmentDetail_MODEL.cust_trnsport_id = ds.Tables[1].Rows[0]["def_trns_id"].ToString();
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialShipmentCustomerAddress.cshtml", _ShipmentDetail_MODEL);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public ActionResult ShipmentPackingList(string CustID)
        {
            try
            {
                string OrderType = string.Empty;
                JsonResult DataRows = null;
                DataSet PackListNumberDs = new DataSet();
                //string Cust_id = string.Empty;
                // List<PackListNumber> _PackListNumberList = new List<PackListNumber>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                //Cust_id = _ShipmentDetail_MODEL.filterCustomerName;             
                string BrchID = Session["BranchId"].ToString();
                PackListNumberDs = _Shipment_ISERVICES.getShipmentPackingList(CompID, BrchID, CustID, null);
                // if (PackListNumberDs.Tables[0].Rows.Count > 0)
                //   {
                DataRow Drow = PackListNumberDs.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";
                PackListNumberDs.Tables[0].Rows.InsertAt(Drow, 0);

                //foreach (DataRow dr in PackListNumberDs.Tables[0].Rows)
                //{
                //    PackListNumber _PackListNumber = new PackListNumber();
                //    _PackListNumber.pack_no = dr["pack_no"].ToString();
                //    _PackListNumber.pack_dt = dr["pack_dt"].ToString();
                //    _PackListNumberList.Add(_PackListNumber);
                //}
                //  }

                //_ShipmentDetail_MODEL.PackListNumberList = _PackListNumberList;
                //return Json(_PackListNumberList.Select(c => new { Name = c.pack_no, ID = c.pack_dt }).ToList(), JsonRequestBehavior.AllowGet);
                DataRows = Json(JsonConvert.SerializeObject(PackListNumberDs));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }


        }
        public ActionResult getDetailPckingListByPackNo(string Pack_NO, string Pack_date, string DocumentMenuId)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _Shipment_ISERVICES.getDetailPckingListByPackNo(CompID, BrchID, Pack_NO, Pack_date, DocumentMenuId);
                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult getcurr_Detail(string Pack_NO, string Pack_date)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataTable dt = _Shipment_ISERVICES.getcurr_Detail(CompID, BrchID, Pack_NO, Pack_date);
                DataRows = Json(JsonConvert.SerializeObject(dt));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        private ActionResult ShipmentDelete(ShipmentDetail_MODEL _ShipmentDetail_MODEL, string command)
        {
            DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();

                string shipNo = _ShipmentDetail_MODEL.ship_no;
                string shipNumber = shipNo.Replace("/", "");

                DataSet Message = _Shipment_ISERVICES.ShipmentDelete(_ShipmentDetail_MODEL, CompID, br_id);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(shipNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, shipNumber, Server);
                }
                /*---------Attachments Section End----------------*/
                return RedirectToAction("ShipmentDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        //[NonAction]
        public ActionResult ShipmentApprove(ShipmentDetail_MODEL _ShipmentDetail_MODEL, string DocNo, string DocDate, string ShipType, string A_Status, string A_Level, string A_Remarks, string command, string ListFilterData1, string docid, string WF_Status1)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                    _ShipmentDetail_MODEL.DocumentMenuId = docid;
                }
                else
                {
                    DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                }
                string br_id = Session["BranchId"].ToString();
                _ShipmentDetail_MODEL.CreatedBy = Session["UserId"].ToString();
                _ShipmentDetail_MODEL.ship_no = DocNo;
                _ShipmentDetail_MODEL.ship_dt = DocDate;
                _ShipmentDetail_MODEL.ship_type = ShipType;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                DataSet Message = _Shipment_ISERVICES.ShipmentApprove(_ShipmentDetail_MODEL, CompID, br_id, mac_id, DocumentMenuId, A_Status, A_Level, A_Remarks);
                var Messag = Message.Tables[1].Rows[0]["result"].ToString();
                if (Messag == "StkNotAvl")
                {
                    _ShipmentDetail_MODEL.Command = "Save";
                    _ShipmentDetail_MODEL.Message = "StockNotAvail";
                    _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                    _ShipmentDetail_MODEL.AppStatus = "D";
                    _ShipmentDetail_MODEL.ShipMent_type = ShipType;
                }
                else
                {
                    _ShipmentDetail_MODEL.Command = command;
                    _ShipmentDetail_MODEL.Message = "Approved";
                    _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                    _ShipmentDetail_MODEL.AppStatus = "D";
                    _ShipmentDetail_MODEL.ShipMent_type = ShipType;
                }
                _ShipmentDetail_MODEL.TransType = "Update";
                _ShipmentDetail_MODEL.ShipmentNumber = _ShipmentDetail_MODEL.ship_no;
                _ShipmentDetail_MODEL.ShipmentDate = _ShipmentDetail_MODEL.ship_dt.ToString();
                try
                {
                    //string fileName = "SHP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "Shipment_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(_ShipmentDetail_MODEL,_ShipmentDetail_MODEL.ship_no, _ShipmentDetail_MODEL.ship_dt.ToString(), fileName, DocumentMenuId,"AP");
                    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _ShipmentDetail_MODEL.ship_no, "AP", Session["UserId"].ToString(), "", filePath);
                }
                catch (Exception exMail)
                {
                    _ShipmentDetail_MODEL.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                _ShipmentDetail_MODEL.Message = _ShipmentDetail_MODEL.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                TempData["ModelData"] = _ShipmentDetail_MODEL;
                UrlModel URLModelApprove = new UrlModel();
                URLModelApprove.Cmd = _ShipmentDetail_MODEL.Command;
                URLModelApprove.Trns_Typ = _ShipmentDetail_MODEL.TransType;
                URLModelApprove.Spmt_no = _ShipmentDetail_MODEL.ShipmentNumber;
                URLModelApprove.Spmt_dt = _ShipmentDetail_MODEL.ShipmentDate;
                URLModelApprove.DocId = _ShipmentDetail_MODEL.DocumentMenuId;
                URLModelApprove.Sp_typ = _ShipmentDetail_MODEL.ShipMent_type;
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("ShipmentDetail", URLModelApprove);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        [NonAction]
        public ActionResult SaveUpdateShipment(ShipmentDetail_MODEL _ShipmentDetail_MODEL, HttpPostedFileBase[] ShipmentFiles)
        {
            string SaveMessage = "";
            string PageName = "";

            try
            {
                if (_ShipmentDetail_MODEL.CancelFlag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BrchID = Session["BranchId"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }

                    DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                    getDocumentName(); /* To set Title*/
                    PageName = title.Replace(" ", "");
                    DataTable ShipmentHeader = new DataTable();
                    DataTable ShipmentItemDetails = new DataTable();
                    DataTable ShipmentTranspoterDetails = new DataTable();
                    DataTable ItemBatchDetails = new DataTable();
                    DataTable ItemSerialDetails = new DataTable();

                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("ship_type", typeof(string));
                    dtheader.Columns.Add("ship_no", typeof(string));
                    dtheader.Columns.Add("ship_dt", typeof(string));
                    dtheader.Columns.Add("cust_id", typeof(int));
                    dtheader.Columns.Add("bill_address", typeof(string));
                    dtheader.Columns.Add("ship_address", typeof(string));
                    dtheader.Columns.Add("trpt_name", typeof(string));
                    dtheader.Columns.Add("veh_number", typeof(string));
                    dtheader.Columns.Add("driver_name", typeof(string));
                    dtheader.Columns.Add("tot_tonnage", typeof(string));
                    dtheader.Columns.Add("gr_no", typeof(string));
                    dtheader.Columns.Add("gr_dt", typeof(string));
                    dtheader.Columns.Add("mob_no", typeof(string));
                    dtheader.Columns.Add("cntry_dest", typeof(string));
                    dtheader.Columns.Add("cntry_origin", typeof(string));
                    dtheader.Columns.Add("local_port", typeof(string));
                    dtheader.Columns.Add("loading_port", typeof(string));
                    dtheader.Columns.Add("discharge_port", typeof(string));
                    dtheader.Columns.Add("destination_port", typeof(string));
                    dtheader.Columns.Add("carrier_name", typeof(string));
                    dtheader.Columns.Add("carrier_no", typeof(string));
                    dtheader.Columns.Add("container_no", typeof(string));
                    dtheader.Columns.Add("so_remarks", typeof(string));
                    dtheader.Columns.Add("pack_no", typeof(string));
                    dtheader.Columns.Add("pack_dt", typeof(DateTime));
                    dtheader.Columns.Add("curr_id", typeof(int));
                    dtheader.Columns.Add("create_id", typeof(string));
                    dtheader.Columns.Add("ship_status", typeof(string));
                    dtheader.Columns.Add("UserMacaddress", typeof(string));
                    dtheader.Columns.Add("UserSystemName", typeof(string));
                    dtheader.Columns.Add("UserIP", typeof(string));
                    dtheader.Columns.Add("tot_gr_wght", typeof(string));
                    dtheader.Columns.Add("tot_net_wght", typeof(string));
                    dtheader.Columns.Add("tot_cbm", typeof(string));
                    dtheader.Columns.Add("line_seal_no", typeof(string));
                    dtheader.Columns.Add("self_seal_no", typeof(string));
                    dtheader.Columns.Add("cntr_net_wt", typeof(string));
                    dtheader.Columns.Add("cntr_gross_wt", typeof(string));
                    // Add On Other Detail section
                    dtheader.Columns.Add("pre_carr_by", typeof(string));
                    dtheader.Columns.Add("pi_rcpt_carr", typeof(string));
                    dtheader.Columns.Add("ves_fli_no", typeof(string));
                    dtheader.Columns.Add("other_ref", typeof(string));
                    dtheader.Columns.Add("term_del_pay", typeof(string));
                    dtheader.Columns.Add("des_good", typeof(string));
                    dtheader.Columns.Add("prof_detail", typeof(string));
                    dtheader.Columns.Add("declar", typeof(string));
                    dtheader.Columns.Add("ext_ref", typeof(string));
                    dtheader.Columns.Add("trade_term", typeof(string));
                    dtheader.Columns.Add("buyer_consig", typeof(string));
                    dtheader.Columns.Add("consig_addr", typeof(string));
                    dtheader.Columns.Add("buyers_ord_and_dt", typeof(string));
                    //Add On Other Detail section End
                    dtheader.Columns.Add("cust_term_deli_pay", typeof(string));
                    dtheader.Columns.Add("cust_lol_port", typeof(string));
                    dtheader.Columns.Add("custom_inv_no", typeof(string));
                    dtheader.Columns.Add("exp_addr", typeof(string));
                    dtheader.Columns.Add("consig_name", typeof(string));
                    dtheader.Columns.Add("custom_inv_dt", typeof(string));
                    dtheader.Columns.Add("pvt_mark", typeof(string));

                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    if (_ShipmentDetail_MODEL.ship_no != null)
                    {
                        dtrowHeader["TransType"] = "Update";
                    }
                    else
                    {
                        dtrowHeader["TransType"] = "Save";
                    }
                    dtrowHeader["comp_id"] = Session["CompId"].ToString();
                    dtrowHeader["br_id"] = BrchID;
                    dtrowHeader["ship_type"] = _ShipmentDetail_MODEL.ship_type;
                    dtrowHeader["ship_no"] = _ShipmentDetail_MODEL.ship_no;
                    dtrowHeader["ship_dt"] = _ShipmentDetail_MODEL.ship_dt;
                    dtrowHeader["cust_id"] = _ShipmentDetail_MODEL.CustomerID;
                    dtrowHeader["bill_address"] = _ShipmentDetail_MODEL.bill_add_id;
                    dtrowHeader["ship_address"] = _ShipmentDetail_MODEL.ship_add_id;
                    dtrowHeader["trpt_name"] = "";
                    dtrowHeader["veh_number"] = "";
                    dtrowHeader["driver_name"] = "";
                    dtrowHeader["tot_tonnage"] = "";
                    dtrowHeader["gr_no"] = "";
                    dtrowHeader["gr_dt"] = "";
                    dtrowHeader["mob_no"] = "";
                    dtrowHeader["cntry_dest"] = _ShipmentDetail_MODEL.cntry_dest;
                    dtrowHeader["cntry_origin"] = _ShipmentDetail_MODEL.cntry_origin;
                    dtrowHeader["local_port"] = _ShipmentDetail_MODEL.local_port;
                    dtrowHeader["loading_port"] = _ShipmentDetail_MODEL.loading_port;
                    dtrowHeader["discharge_port"] = _ShipmentDetail_MODEL.discharge_port;
                    dtrowHeader["destination_port"] = _ShipmentDetail_MODEL.destination_port;
                    dtrowHeader["carrier_name"] = _ShipmentDetail_MODEL.carrier_name;
                    dtrowHeader["carrier_no"] = _ShipmentDetail_MODEL.carrier_no;
                    dtrowHeader["container_no"] = _ShipmentDetail_MODEL.container_no;
                    dtrowHeader["so_remarks"] = _ShipmentDetail_MODEL.so_remarks;
                    dtrowHeader["pack_no"] = _ShipmentDetail_MODEL.pack_num;
                    dtrowHeader["pack_dt"] = _ShipmentDetail_MODEL.pack_dte;
                    if (!string.IsNullOrEmpty(_ShipmentDetail_MODEL.curr_id))
                    {
                        dtrowHeader["curr_id"] = _ShipmentDetail_MODEL.curr_id;
                    }
                    else
                    {
                        dtrowHeader["curr_id"] = 0;
                    }

                    dtrowHeader["tot_gr_wght"] = _ShipmentDetail_MODEL.TotalGrossWgt;
                    dtrowHeader["tot_net_wght"] = _ShipmentDetail_MODEL.TotalNetWgt;
                    dtrowHeader["tot_cbm"] = _ShipmentDetail_MODEL.TotalCBM;
                    dtrowHeader["create_id"] = Session["UserId"].ToString();
                    //dtrowHeader["ship_status"] = Session["AppStatus"].ToString();
                    dtrowHeader["ship_status"] = "D";
                    dtrowHeader["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrowHeader["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrowHeader["UserIP"] = Session["UserIP"].ToString();
                    dtrowHeader["line_seal_no"] = _ShipmentDetail_MODEL.LineSealNumber;
                    dtrowHeader["self_seal_no"] = _ShipmentDetail_MODEL.SelfSealNumber;
                    dtrowHeader["cntr_net_wt"] = _ShipmentDetail_MODEL.ContainerNetWeight;
                    dtrowHeader["cntr_gross_wt"] = _ShipmentDetail_MODEL.ContainerGrossWeight;

                    //----------------Other Detail section
                    dtrowHeader["pre_carr_by"] = _ShipmentDetail_MODEL.pre_carr_by;
                    dtrowHeader["pi_rcpt_carr"] = _ShipmentDetail_MODEL.pi_rcpt_carr;
                    dtrowHeader["ves_fli_no"] = _ShipmentDetail_MODEL.ves_fli_no;
                    dtrowHeader["other_ref"] = _ShipmentDetail_MODEL.other_ref;
                    dtrowHeader["term_del_pay"] = _ShipmentDetail_MODEL.term_del_pay;
                    dtrowHeader["des_good"] = _ShipmentDetail_MODEL.des_good;
                    dtrowHeader["prof_detail"] = _ShipmentDetail_MODEL.prof_detail;
                    dtrowHeader["declar"] = _ShipmentDetail_MODEL.declar;
                    dtrowHeader["ext_ref"] = _ShipmentDetail_MODEL.ExportersReference;
                    dtrowHeader["trade_term"] = _ShipmentDetail_MODEL.trade_term;
                    dtrowHeader["buyer_consig"] = _ShipmentDetail_MODEL.BuyerIfOtherThenConsignee;
                    dtrowHeader["consig_addr"] = _ShipmentDetail_MODEL.ConsigneeAddress;
                    dtrowHeader["buyers_ord_and_dt"] = _ShipmentDetail_MODEL.BuyersOrderNumberAndDate;

                    dtrowHeader["cust_lol_port"] = _ShipmentDetail_MODEL.custom_local_port;
                    dtrowHeader["cust_term_deli_pay"] = _ShipmentDetail_MODEL.Cust_term_del_pay;
                    dtrowHeader["custom_inv_no"] = _ShipmentDetail_MODEL.custom_inv_no;
                    dtrowHeader["exp_addr"] = _ShipmentDetail_MODEL.ExporterAddress;
                    dtrowHeader["consig_name"] = _ShipmentDetail_MODEL.ConsigneeName;
                    if (DocumentMenuId == "105103145120")
                    {
                        dtrowHeader["custom_inv_dt"] = _ShipmentDetail_MODEL.CustomInvDate;
                    }
                    else
                    {
                        dtrowHeader["custom_inv_dt"] = _ShipmentDetail_MODEL.ship_dt;
                    }
                    dtrowHeader["pvt_mark"] = _ShipmentDetail_MODEL.PvtMark;
                    //----------------Other Detail section End
                    dtheader.Rows.Add(dtrowHeader);
                    ShipmentHeader = dtheader;


                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("pack_qty", typeof(string));
                    dtItem.Columns.Add("pack_nos", typeof(string));
                    dtItem.Columns.Add("gr_wght", typeof(string));
                    dtItem.Columns.Add("net_wght", typeof(string));
                    dtItem.Columns.Add("tot_cbm", typeof(string));
                    dtItem.Columns.Add("ship_qty", typeof(string));
                    dtItem.Columns.Add("wh_id", typeof(int));
                    dtItem.Columns.Add("avl_stock", typeof(string));
                    dtItem.Columns.Add("lot_no", typeof(string));
                    dtItem.Columns.Add("inv_qty", typeof(string));
                    dtItem.Columns.Add("item_rate", typeof(string));
                    dtItem.Columns.Add("item_ass_val", typeof(string));
                    dtItem.Columns.Add("item_tax_amt_recov", typeof(string));
                    dtItem.Columns.Add("item_tax_amt_nrecov", typeof(string));
                    dtItem.Columns.Add("item_oc_amt", typeof(string));
                    dtItem.Columns.Add("item_net_val_spec", typeof(string));
                    dtItem.Columns.Add("item_net_val_bs", typeof(string));
                    dtItem.Columns.Add("item_landed_rate", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));
                    dtItem.Columns.Add("PackSize", typeof(string));

                    JArray jObject = JArray.Parse(_ShipmentDetail_MODEL.ShipmentItemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                        dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                        dtrowLines["pack_qty"] = jObject[i]["PackedQuantity"].ToString();
                        dtrowLines["pack_nos"] = jObject[i]["NumberOfPacks"].ToString();
                        dtrowLines["gr_wght"] = jObject[i]["GrossWeight"].ToString();
                        dtrowLines["net_wght"] = jObject[i]["NetWeight"].ToString();
                        dtrowLines["tot_cbm"] = jObject[i]["CBM"].ToString();
                        dtrowLines["ship_qty"] = jObject[i]["ShippedQuantity"].ToString();
                        dtrowLines["wh_id"] = jObject[i]["WareHouseId"].ToString();
                        dtrowLines["avl_stock"] = jObject[i]["AvailableStock"].ToString();
                        dtrowLines["lot_no"] = 0;
                        dtrowLines["inv_qty"] = jObject[i]["InvoicedQuantity"].ToString();
                        dtrowLines["item_rate"] = 0;
                        dtrowLines["item_ass_val"] = 0;
                        dtrowLines["item_tax_amt_recov"] = 0;
                        dtrowLines["item_tax_amt_nrecov"] = 0;
                        dtrowLines["item_oc_amt"] = 0;
                        dtrowLines["item_net_val_spec"] = 0;
                        dtrowLines["item_net_val_bs"] = 0;
                        dtrowLines["item_landed_rate"] = 0;
                        dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                        dtrowLines["PackSize"] = jObject[i]["PackSize"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    ShipmentItemDetails = dtItem;
                    ViewData["ItemDetails"] = dtitemdetail(jObject);

                    DataTable dtTranspoter = new DataTable();
                    dtTranspoter.Columns.Add("gr_no", typeof(string));
                    dtTranspoter.Columns.Add("gr_dt", typeof(string));
                    dtTranspoter.Columns.Add("trpt_name", typeof(string));
                    dtTranspoter.Columns.Add("veh_number", typeof(string));
                    dtTranspoter.Columns.Add("driver_name", typeof(string));
                    dtTranspoter.Columns.Add("mob_no", typeof(string));
                    dtTranspoter.Columns.Add("tot_tonnage", typeof(string));
                    dtTranspoter.Columns.Add("no_of_pkgs", typeof(string));

                    JArray jObject6 = JArray.Parse(_ShipmentDetail_MODEL.Transpoterdetails);
                    for (int i = 0; i < jObject6.Count; i++)
                    {
                        DataRow dtrowLines6 = dtTranspoter.NewRow();
                        dtrowLines6["gr_no"] = jObject6[i]["GrNumber"].ToString();
                        dtrowLines6["gr_dt"] = jObject6[i]["GRDt"].ToString();
                        dtrowLines6["trpt_name"] = jObject6[i]["trpt_ID"].ToString();
                        dtrowLines6["veh_number"] = jObject6[i]["veh_number"].ToString();
                        dtrowLines6["driver_name"] = jObject6[i]["driver_name"].ToString();
                        dtrowLines6["mob_no"] = jObject6[i]["Mobile_no"].ToString();
                        dtrowLines6["tot_tonnage"] = jObject6[i]["tot_tonnage"].ToString();
                        dtrowLines6["no_of_pkgs"] = jObject6[i]["no_of_pkgs"].ToString();
                        dtTranspoter.Rows.Add(dtrowLines6);
                    }
                    ShipmentTranspoterDetails = dtTranspoter;
                    ViewData["TranspoterDetails"] = dtTransPoterdetail(jObject6);

                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("item_id", typeof(string));
                    Batch_detail.Columns.Add("lot_no", typeof(string));
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                    Batch_detail.Columns.Add("uom_id", typeof(int));
                    Batch_detail.Columns.Add("ship_qty", typeof(string));
                    Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
                    Batch_detail.Columns.Add("expiry_date", typeof(string));
                    Batch_detail.Columns.Add("issue_qty", typeof(string));
                    Batch_detail.Columns.Add("mfg_name", typeof(string));
                    Batch_detail.Columns.Add("mfg_mrp", typeof(string));
                    Batch_detail.Columns.Add("mfg_date", typeof(string));
                    if (_ShipmentDetail_MODEL.ItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_ShipmentDetail_MODEL.ItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                            dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                            dtrowBatchDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
                            dtrowBatchDetailsLines["ship_qty"] = "0";
                            dtrowBatchDetailsLines["avl_batch_qty"] = jObjectBatch[i]["BatchAvlStock"].ToString();

                            if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null)
                            {
                                dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                            }
                            else
                            {
                                dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                            }

                            //dtrowBatchDetailsLines["expiry_date"] = DateTime.Parse(jObjectBatch[i]["ExpiryDate"].ToString());
                            dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            dtrowBatchDetailsLines["mfg_name"] = IsBlank(jObjectBatch[i]["mfg_name"].ToString(),null);
                            dtrowBatchDetailsLines["mfg_mrp"] = IsBlank(jObjectBatch[i]["mfg_mrp"].ToString(),null);
                            dtrowBatchDetailsLines["mfg_date"] = IsBlank(jObjectBatch[i]["mfg_date"].ToString(),null);
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                        ViewData["BatchDetails"] = dtBatchdetail(jObjectBatch);
                    }
                    ItemBatchDetails = Batch_detail;


                    DataTable Serial_detail = new DataTable();
                    Serial_detail.Columns.Add("item_id", typeof(string));
                    Serial_detail.Columns.Add("uom_id", typeof(int));
                    Serial_detail.Columns.Add("ship_qty", typeof(string));
                    Serial_detail.Columns.Add("lot_no", typeof(string));
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    Serial_detail.Columns.Add("issue_qty", typeof(string));
                    Serial_detail.Columns.Add("mfg_name", typeof(string));
                    Serial_detail.Columns.Add("mfg_mrp", typeof(string));
                    Serial_detail.Columns.Add("mfg_date", typeof(string));

                    if (_ShipmentDetail_MODEL.ItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_ShipmentDetail_MODEL.ItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                            dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                            dtrowSerialDetailsLines["ship_qty"] = "0";
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                            dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["mfg_name"] = IsBlank(jObjectSerial[i]["mfg_name"].ToString(),null);
                            dtrowSerialDetailsLines["mfg_mrp"] = IsBlank(jObjectSerial[i]["mfg_mrp"].ToString(),null);
                            dtrowSerialDetailsLines["mfg_date"] = IsBlank(jObjectSerial[i]["mfg_date"].ToString(),null);
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                        ViewData["SerialDetails"] = dtSerialdetail(jObjectSerial);
                    }
                    ItemSerialDetails = Serial_detail;

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("ship_qty", typeof(string));
                    if (_ShipmentDetail_MODEL.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_ShipmentDetail_MODEL.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["ship_qty"] = jObject2[i]["qty"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubItemDetails"] = dtSubitemdetail(jObject2);
                    }

                    /*------------------Sub Item end----------------------*/
                    /*-----------------Attachment Section Start------------------------*/
                    DataTable ShipmentAttachments = new DataTable();
                    DataTable shipdtAttachment = new DataTable();
                    var _ShipMentModelattch = TempData["Model_attch"] as ShipMentModelattch;
                    TempData["Model_attch"] = null;
                    if (_ShipmentDetail_MODEL.attatchmentdetail != null)
                    {
                        if (_ShipMentModelattch != null)
                        {
                            //if (Session["AttachMentDetailItmStp"] != null)
                            if (_ShipMentModelattch.AttachMentDetailItmStp != null)
                            {
                                //shipdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                shipdtAttachment = _ShipMentModelattch.AttachMentDetailItmStp as DataTable;

                            }
                            else
                            {
                                shipdtAttachment.Columns.Add("id", typeof(string));
                                shipdtAttachment.Columns.Add("file_name", typeof(string));
                                shipdtAttachment.Columns.Add("file_path", typeof(string));
                                shipdtAttachment.Columns.Add("file_def", typeof(char));
                                shipdtAttachment.Columns.Add("comp_id", typeof(Int32));
                            }
                        }
                        else
                        {
                            if (_ShipmentDetail_MODEL.AttachMentDetailItmStp != null)
                            {
                                shipdtAttachment = _ShipmentDetail_MODEL.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                shipdtAttachment.Columns.Add("id", typeof(string));
                                shipdtAttachment.Columns.Add("file_name", typeof(string));
                                shipdtAttachment.Columns.Add("file_path", typeof(string));
                                shipdtAttachment.Columns.Add("file_def", typeof(char));
                                shipdtAttachment.Columns.Add("comp_id", typeof(Int32));
                            }
                        }
                        JArray jObject1 = JArray.Parse(_ShipmentDetail_MODEL.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in shipdtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = shipdtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_ShipmentDetail_MODEL.ship_no).ToString()))
                                {
                                    dtrowAttachment1["id"] = _ShipmentDetail_MODEL.ship_no;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                shipdtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_ShipmentDetail_MODEL.TransType == "Update")
                        {
                            //string branch_id = dtrowHeader["br_id"].ToString();
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string SHIP_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_ShipmentDetail_MODEL.ship_no).ToString()))
                                {
                                    SHIP_CODE = (_ShipmentDetail_MODEL.ship_no).ToString();

                                }
                                else
                                {
                                    SHIP_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + SHIP_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in shipdtAttachment.Rows)
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
                        ShipmentAttachments = shipdtAttachment;
                    }

                    SaveMessage = _Shipment_ISERVICES.InsertUpdateShipment(ShipmentHeader, ShipmentItemDetails, ItemBatchDetails, ItemSerialDetails, dtSubItem, ShipmentAttachments, ShipmentTranspoterDetails);
                    if (SaveMessage == "DocModify")
                    {
                        _ShipmentDetail_MODEL.Message = "DocModify";
                        _ShipmentDetail_MODEL.BtnName = "Refresh";
                        _ShipmentDetail_MODEL.Command = "Refresh";
                        TempData["ModelData"] = _ShipmentDetail_MODEL;
                        return RedirectToAction("ShipmentDetail");
                    }
                    if (SaveMessage == "DuplicateCustomInvNo")
                    {
                        _ShipmentDetail_MODEL.Message = "DuplicateCustomInvNo";
                        _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                        _ShipmentDetail_MODEL.hdnsaveApprovebtn = "Duplicate";
                        _ShipmentDetail_MODEL.Command = "Save";
                        _ShipmentDetail_MODEL.TransType = "Save";
                        _ShipmentDetail_MODEL.DuplicateCustmInvNo = "DuplicateCustomInvNo";
                        TempData["ModelData"] = _ShipmentDetail_MODEL;
                        return RedirectToAction("ShipmentDetail");
                    }
                    else
                    {
                        string[] FDetail = SaveMessage.Split(',');

                        string Message = FDetail[0].ToString();
                        string ShipmentNumber = FDetail[1].ToString();
                        if (Message == "Data_Not_Found")
                        {
                            var a = ShipmentNumber.Split('-');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _ShipmentDetail_MODEL.Message = Message.Replace("_", "");
                            return RedirectToAction("ShipmentDetail");
                        }
                        string ShipmentDate = FDetail[2].ToString();
                        string SHIP_Number = ShipmentNumber.Replace("/", "");

                        /*-----------------Attachment Section Start------------------------*/
                        if (Message == "Save")

                        {
                            string Guid = string.Empty;
                            //if (Session["Guid"] != null)
                            if (_ShipMentModelattch != null)
                            {
                                if (_ShipMentModelattch.Guid != null)
                                {
                                    //string Guid = Session["Guid"].ToString();
                                    Guid = _ShipMentModelattch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, SHIP_Number, _ShipmentDetail_MODEL.TransType, ShipmentAttachments);

                        }
                        /*-----------------Attachment Section End------------------------*/


                        if (Message == "StockNotAvail")
                        {
                            _ShipmentDetail_MODEL.Message = "StockNotAvail";
                            _ShipmentDetail_MODEL.BtnName = "Refresh";
                            _ShipmentDetail_MODEL.Command = "Refresh";
                            _ShipmentDetail_MODEL.TransType = "Save";
                            _ShipmentDetail_MODEL.ShipmentDate = ShipmentDate;
                            _ShipmentDetail_MODEL.ShipmentNumber = ShipmentNumber;
                        }

                        if (Message == "Update" || Message == "Save")
                        {
                            _ShipmentDetail_MODEL.Message = "Save";
                            _ShipmentDetail_MODEL.BtnName = "BtnEdit";
                            _ShipmentDetail_MODEL.TransType = "Update";
                            _ShipmentDetail_MODEL.ShipmentDate = ShipmentDate;
                            _ShipmentDetail_MODEL.ShipmentNumber = ShipmentNumber;

                        }
                        return RedirectToAction("ShipmentDetail");
                    }
                }
                else
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }

                    string br_id = Session["BranchId"].ToString();
                    _ShipmentDetail_MODEL.CreatedBy = userid;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DocumentMenuId = _ShipmentDetail_MODEL.DocumentMenuId;
                    DataSet message = _Shipment_ISERVICES.ShipmentCancel(_ShipmentDetail_MODEL, CompID, br_id, mac_id
                        , DocumentMenuId);
                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //Session["ShipmentNumber"] = _ShipmentDetail_MODEL.ship_no;
                    //Session["ShipmentDate"] = _ShipmentDetail_MODEL.ship_dt.ToString(); ;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    try
                    {
                        //string fileName = "SHP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "Shipment_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_ShipmentDetail_MODEL,_ShipmentDetail_MODEL.ship_no, _ShipmentDetail_MODEL.ship_dt.ToString(), fileName, DocumentMenuId,"C");
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _ShipmentDetail_MODEL.ship_no, "C", Session["UserId"].ToString(), "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _ShipmentDetail_MODEL.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    //_ShipmentDetail_MODEL.Message = "Cancelled";
                    _ShipmentDetail_MODEL.Message = _ShipmentDetail_MODEL.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    _ShipmentDetail_MODEL.BtnName = "Refresh";
                    _ShipmentDetail_MODEL.Command = "Update";
                    _ShipmentDetail_MODEL.TransType = "Update";
                    _ShipmentDetail_MODEL.ShipmentNumber = _ShipmentDetail_MODEL.ship_no;
                    _ShipmentDetail_MODEL.ShipmentDate = _ShipmentDetail_MODEL.ship_dt.ToString();
                    _ShipmentDetail_MODEL.AppStatus = "D";
                    return RedirectToAction("ShipmentDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*---------------Attachment Section start-------------------*/
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ShipmentDetail_MODEL.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ShipmentDetail_MODEL.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ShipmentDetail_MODEL.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                        _ShipmentDetail_MODEL.Message = "Error";
                    }
                }
                /*-----------------Attachment Section end------------------*/
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public DataTable dtitemdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("base_uom_id", typeof(int));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("pack_qty", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("pack_nos", typeof(string));

            dtItem.Columns.Add("gr_wght", typeof(string));
            dtItem.Columns.Add("net_wght", typeof(string));
            dtItem.Columns.Add("tot_cbm", typeof(string));
            dtItem.Columns.Add("ship_qty", typeof(string));
            dtItem.Columns.Add("wh_id", typeof(int));
            dtItem.Columns.Add("wh_name", typeof(string));
            dtItem.Columns.Add("availale_qty", typeof(string));
            dtItem.Columns.Add("lot_no", typeof(string));
            dtItem.Columns.Add("invoice_qty", typeof(string));
            dtItem.Columns.Add("i_batch", typeof(string));
            dtItem.Columns.Add("i_serial", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("item_ass_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt_recov", typeof(string));
            dtItem.Columns.Add("item_tax_amt_nrecov", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val_spec", typeof(string));
            dtItem.Columns.Add("item_net_val_bs", typeof(string));
            dtItem.Columns.Add("item_landed_rate", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("PackSize", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["base_uom_id"] = jObject[i]["UOMId"].ToString();
                dtrowLines["uom_name"] = jObject[i]["UOM"].ToString();
                dtrowLines["pack_qty"] = jObject[i]["PackedQuantity"].ToString();
                dtrowLines["sub_item"] = jObject[i]["sub_item"].ToString();
                dtrowLines["pack_nos"] = jObject[i]["NumberOfPacks"].ToString();

                dtrowLines["gr_wght"] = jObject[i]["GrossWeight"].ToString();
                dtrowLines["net_wght"] = jObject[i]["NetWeight"].ToString();
                dtrowLines["tot_cbm"] = jObject[i]["CBM"].ToString();

                dtrowLines["ship_qty"] = jObject[i]["ShippedQuantity"].ToString();
                dtrowLines["wh_id"] = jObject[i]["WareHouseId"].ToString();
                dtrowLines["wh_name"] = jObject[i]["WareHouseName"].ToString();
                dtrowLines["availale_qty"] = jObject[i]["AvailableStock"].ToString();
                dtrowLines["lot_no"] = 0;
                dtrowLines["invoice_qty"] = jObject[i]["InvoicedQuantity"].ToString();
                dtrowLines["i_batch"] = jObject[i]["i_batch"].ToString();
                dtrowLines["i_serial"] = jObject[i]["i_serial"].ToString();
                dtrowLines["item_rate"] = 0;
                dtrowLines["item_ass_val"] = 0;
                dtrowLines["item_tax_amt_recov"] = 0;
                dtrowLines["item_tax_amt_nrecov"] = 0;
                dtrowLines["item_oc_amt"] = 0;
                dtrowLines["item_net_val_spec"] = 0;
                dtrowLines["item_net_val_bs"] = 0;
                dtrowLines["item_landed_rate"] = 0;
                dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                dtrowLines["PackSize"] = jObject[i]["PackSize"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtTransPoterdetail(JArray jObject6)
        {
            DataTable dtTransPoter = new DataTable();

            dtTransPoter.Columns.Add("gr_no", typeof(string));
            dtTransPoter.Columns.Add("gr_dt", typeof(string));
            dtTransPoter.Columns.Add("gr_Date", typeof(string));
            dtTransPoter.Columns.Add("trpm_id", typeof(string));
            dtTransPoter.Columns.Add("trans_name", typeof(string));
            dtTransPoter.Columns.Add("veh_number", typeof(string));
            dtTransPoter.Columns.Add("driver_name", typeof(string));
            dtTransPoter.Columns.Add("mob_no", typeof(string));
            dtTransPoter.Columns.Add("tot_tonnage", typeof(string));
            dtTransPoter.Columns.Add("no_of_pkgs", typeof(string));

            for (int i = 0; i < jObject6.Count; i++)
            {
                DataRow dtrowLines = dtTransPoter.NewRow();
                dtrowLines["gr_no"] = jObject6[i]["GrNumber"].ToString();
                dtrowLines["gr_dt"] = jObject6[i]["GRDt"].ToString();
                dtrowLines["gr_Date"] = jObject6[i]["GRDt"].ToString();
                dtrowLines["trpm_id"] = jObject6[i]["trpt_ID"].ToString();
                dtrowLines["trans_name"] = jObject6[i]["trpt_name"].ToString();
                dtrowLines["veh_number"] = jObject6[i]["veh_number"].ToString();
                dtrowLines["driver_name"] = jObject6[i]["driver_name"].ToString();
                dtrowLines["mob_no"] = jObject6[i]["Mobile_no"].ToString();
                dtrowLines["tot_tonnage"] = jObject6[i]["tot_tonnage"].ToString();
                dtrowLines["no_of_pkgs"] = jObject6[i]["no_of_pkgs"].ToString();
                dtTransPoter.Rows.Add(dtrowLines);
            }

            return dtTransPoter;
        }
        public DataTable dtBatchdetail(JArray jObjectBatch)
        {
            DataTable Batch_detail = new DataTable();
            Batch_detail.Columns.Add("item_id", typeof(string));
            Batch_detail.Columns.Add("lot_id", typeof(string));
            Batch_detail.Columns.Add("batch_no", typeof(string));
            Batch_detail.Columns.Add("uom_id", typeof(int));
            Batch_detail.Columns.Add("ship_qty", typeof(string));
            Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
            Batch_detail.Columns.Add("expiry_date", typeof(string));
            Batch_detail.Columns.Add("exp_dt", typeof(string));
            Batch_detail.Columns.Add("issue_qty", typeof(string));

            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                dtrowBatchDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();
                dtrowBatchDetailsLines["lot_id"] = jObjectBatch[i]["LotNo"].ToString();
                dtrowBatchDetailsLines["ship_qty"] = "0";
                dtrowBatchDetailsLines["avl_batch_qty"] = jObjectBatch[i]["BatchAvlStock"].ToString();

                if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null)
                {
                    dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                }
                else
                {
                    dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                }
                dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["ExpiryDate"].ToString();
                //dtrowBatchDetailsLines["expiry_date"] = DateTime.Parse(jObjectBatch[i]["ExpiryDate"].ToString());
                dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                Batch_detail.Rows.Add(dtrowBatchDetailsLines);
            }

            return Batch_detail;
        }
        public DataTable dtSerialdetail(JArray jObjectSerial)
        {
            DataTable Serial_detail = new DataTable();
            Serial_detail.Columns.Add("item_id", typeof(string));
            Serial_detail.Columns.Add("uom_id", typeof(int));
            Serial_detail.Columns.Add("ship_qty", typeof(string));
            Serial_detail.Columns.Add("lot_no", typeof(string));
            Serial_detail.Columns.Add("serial_no", typeof(string));
            Serial_detail.Columns.Add("issue_qty", typeof(string));

            for (int i = 0; i < jObjectSerial.Count; i++)
            {
                DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                dtrowSerialDetailsLines["ship_qty"] = "0";
                dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                Serial_detail.Rows.Add(dtrowSerialDetailsLines);
            }

            return Serial_detail;
        }
        public DataTable dtSubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("ship_qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["ship_qty"] = jObject2[i]["qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }

            return dtSubItem;
        }
        public ActionResult ShipmentPackingListView(ShipmentDetail_MODEL _ShipmentDetail_MODEL)
        {
            try
            {
                string PackNumber, OrderType = string.Empty;
                DataSet PackListNumberDs = new DataSet();
                string Cust_id = string.Empty;
                List<PackListNumber> _PackListNumberList = new List<PackListNumber>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                Cust_id = _ShipmentDetail_MODEL.filterCustomerName;
                PackNumber = _ShipmentDetail_MODEL.FilterPackNumber;
                string BrchID = Session["BranchId"].ToString();
                PackListNumberDs = _Shipment_ISERVICES.getShipmentPackingList(CompID, BrchID, Cust_id, PackNumber);
                foreach (DataRow dr in PackListNumberDs.Tables[0].Rows)
                {
                    PackListNumber _PackListNumber = new PackListNumber();
                    _PackListNumber.packing_no = dr["pack_no"].ToString();
                    _PackListNumber.packing_dt = dr["pack_no"].ToString();
                    _PackListNumberList.Add(_PackListNumber);
                }
                _ShipmentDetail_MODEL.PackListNumberList = _PackListNumberList;
                return Json(_PackListNumberList.Select(c => new { Name = c.packing_no, ID = c.packing_dt }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }


        }

        [NonAction]
        private string getNextDocumentNumber()
        {
            try
            {
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103135")
                //    {
                //        DocumentMenuId = "105103135";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145120")
                //    {
                //        DocumentMenuId = "105103145120";
                //    }
                //}
                string MenuDocumentId = DocumentMenuId;
                string CompId = Session["CompId"].ToString();
                string BranchId = Session["BranchId"].ToString();
                string Prefix = "SH";
                string NextDocumentNumber = _Shipment_ISERVICES.getNextDocumentNumber(CompID, BranchId, MenuDocumentId, Prefix);
                return NextDocumentNumber;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        //[NonAction]
        //private void getWarehouse(ShipmentDetail_MODEL _ShipmentDetail_MODEL)
        //{
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        string Br_ID = string.Empty;
        //        List<Warehouse> _WarehouseList = new List<Warehouse>();
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        DataSet result = _Shipment_ISERVICES.GetWarehouseList(Comp_ID, Br_ID);
        //        foreach (DataRow dr in result.Tables[0].Rows)
        //        {
        //            Warehouse _Warehouse = new Warehouse();
        //            _Warehouse.wh_id = dr["wh_id"].ToString();
        //            _Warehouse.wh_name = dr["wh_name"].ToString();
        //            _WarehouseList.Add(_Warehouse);
        //        }
        //        Warehouse _oWarehouse = new Warehouse();
        //        _oWarehouse.wh_id = "0";
        //        _oWarehouse.wh_name = "---Select---";
        //        _WarehouseList.Insert(0, _oWarehouse);
        //        _ShipmentDetail_MODEL.WarehouseList = _WarehouseList;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);

        //    }
        //}
        //public ActionResult getWarehouseWiseItemStock(string ItemId, string WarehouseId, string CompId, string BranchId)
        //{
        //    try
        //    {
        //        string CompID, BrID, Wh_ID, ItemID, LotID, BatchNo = string.Empty;
        //        CompID = CompId;
        //        BrID = BranchId;
        //        Wh_ID = WarehouseId;
        //        ItemID = ItemId;
        //        LotID = null;
        //        BatchNo = null;
        //        string Stock = _Shipment_ISERVICES.getWarehouseWiseItemStock(CompID, BrID, Wh_ID, ItemID, LotID, BatchNo);
        //        return Json(Stock, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }

        //}
        public ActionResult getItemStockBatchWise(string ItemId, string SelectedItemdetail, string docid, string CMD, string typ)
        {
            try
            {
                //ds = _Shipment_ISERVICES.getItemStockBatchWise(ItemId, WarehouseId, CompId, BranchId);
                //if (SelectedItemdetail != null && SelectedItemdetail != "")
                //{
                //    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        foreach (JObject item in jObjectBatch.Children())
                //        {
                //            if (item.GetValue("LotNo").ToString() == ds.Tables[0].Rows[i]["lot_id"].ToString() && item.GetValue("BatchNo").ToString() == ds.Tables[0].Rows[i]["batch_no"].ToString())
                //            {
                //                ds.Tables[0].Rows[i]["IssueQty"] = item.GetValue("IssueQty");
                //            }
                //        }
                //    }
                //}
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103135")
                //    {
                //        DocumentMenuId = "105103135";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145120")
                //    {
                DocumentMenuId = docid;
                //    }
                //}
                DataTable DTableBatchDetail = new DataTable();
                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    DataTable dtbatchdtl = new DataTable();
                    dtbatchdtl.Columns.Add("lot_id", typeof(string));
                    dtbatchdtl.Columns.Add("ItemId", typeof(string));
                    dtbatchdtl.Columns.Add("UOMId", typeof(string));
                    dtbatchdtl.Columns.Add("batch_no", typeof(string));
                    dtbatchdtl.Columns.Add("avl_batch_qty", typeof(string));
                    dtbatchdtl.Columns.Add("issue_qty", typeof(string));
                    dtbatchdtl.Columns.Add("expiry_date", typeof(string));
                    dtbatchdtl.Columns.Add("exp_dt", typeof(string));
                    dtbatchdtl.Columns.Add("mfg_name", typeof(string));
                    dtbatchdtl.Columns.Add("mfg_mrp", typeof(string));
                    dtbatchdtl.Columns.Add("mfg_date", typeof(string));

                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);

                    foreach (JObject item in jObjectBatch.Children())
                    {
                        if (item.GetValue("ItemId").ToString() == ItemId.ToString())
                        {
                            DataRow dtbatchdtlrow = dtbatchdtl.NewRow();
                            dtbatchdtlrow["lot_id"] = item.GetValue("LotNo").ToString();
                            dtbatchdtlrow["ItemId"] = item.GetValue("ItemId").ToString();
                            dtbatchdtlrow["UOMId"] = item.GetValue("UOMId").ToString();
                            dtbatchdtlrow["batch_no"] = item.GetValue("BatchNo").ToString();
                            dtbatchdtlrow["avl_batch_qty"] = item.GetValue("BatchAvlStock").ToString();
                            dtbatchdtlrow["issue_qty"] = item.GetValue("IssueQty").ToString();
                            if (item.GetValue("ExpiryDate").ToString().Trim() != "")
                            {
                                dtbatchdtlrow["expiry_date"] = Convert.ToDateTime(item.GetValue("ExpiryDate").ToString()).ToString("dd-MM-yyyy");
                            }
                            else
                            {
                                dtbatchdtlrow["expiry_date"] = item.GetValue("ExpiryDate").ToString();
                            }
                            dtbatchdtlrow["exp_dt"] = item.GetValue("ExpiryDate").ToString();
                            dtbatchdtlrow["mfg_name"] = item.GetValue("mfg_name").ToString();
                            dtbatchdtlrow["mfg_mrp"] = item.GetValue("mfg_mrp").ToString();
                            dtbatchdtlrow["mfg_date"] = item.GetValue("mfg_date").ToString();

                            dtbatchdtl.Rows.Add(dtbatchdtlrow);
                        }
                    }
                    DTableBatchDetail = dtbatchdtl;
                }

                ViewBag.DocID = DocumentMenuId;
                ViewBag.Command = CMD;
                ViewBag.TransType = typ;
                if (DTableBatchDetail.Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = DTableBatchDetail;

                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise_New.cshtml");

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        //public ActionResult getItemStockBatchWiseAfterInsert(string Sh_Type, string Sh_No, string Sh_Date, string ItemId)
        //{
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        string Br_ID = string.Empty;

        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        ds = _Shipment_ISERVICES.getItemStockBatchWiseAfterInsert(Comp_ID, Br_ID, Sh_Type, Sh_No, Sh_Date, ItemId);

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            ViewBag.ItemStockBatchWise = ds.Tables[0];
        //        }
        //        return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialShipmenItemStockBatchWise.cshtml");
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //}
        public ActionResult getItemstockSerialWise(string ItemId, string SelectedItemSerial, string docid, string CMD, string typ)
        {
            try
            {
                //ds = _Shipment_ISERVICES.getItemstockSerialWise(ItemId, WarehouseId, CompId, BranchId);
                //if (SelectedItemSerial != null && SelectedItemSerial != "")
                //{
                //    JArray jObjectBatch = JArray.Parse(SelectedItemSerial);
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        foreach (JObject item in jObjectBatch.Children())
                //        {
                //            if (item.GetValue("LOTId").ToString() == ds.Tables[0].Rows[i]["lot_id"].ToString() && item.GetValue("SerialNO").ToString() == ds.Tables[0].Rows[i]["serial_no"].ToString())
                //            {
                //                ds.Tables[0].Rows[i]["SerailSelected"] = "Y";
                //            }

                //        }
                //    }
                //}
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103135")
                //    {
                //        DocumentMenuId = "105103135";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145120")
                //    {
                //        DocumentMenuId = "105103145120";
                //    }
                //}
                DocumentMenuId = docid;
                DataTable DTableSerialDetail = new DataTable();
                if (SelectedItemSerial != null && SelectedItemSerial != "")
                {
                    DataTable dtbatchdtl = new DataTable();
                    dtbatchdtl.Columns.Add("ItemId", typeof(string));
                    dtbatchdtl.Columns.Add("UOMId", typeof(string));
                    dtbatchdtl.Columns.Add("lot_id", typeof(string));
                    dtbatchdtl.Columns.Add("IssuedQuantity", typeof(string));
                    dtbatchdtl.Columns.Add("serial_no", typeof(string));
                    dtbatchdtl.Columns.Add("SerailSelected", typeof(string));

                    JArray jObjectBatch = JArray.Parse(SelectedItemSerial);

                    foreach (JObject item in jObjectBatch.Children())
                    {
                        if (item.GetValue("ItemId").ToString() == ItemId.ToString())
                        {
                            DataRow dtbatchdtlrow = dtbatchdtl.NewRow();
                            dtbatchdtlrow["ItemId"] = item.GetValue("ItemId").ToString();
                            dtbatchdtlrow["UOMId"] = item.GetValue("UOMId").ToString();
                            dtbatchdtlrow["lot_id"] = item.GetValue("LOTId").ToString();
                            dtbatchdtlrow["IssuedQuantity"] = item.GetValue("IssuedQuantity").ToString();
                            dtbatchdtlrow["serial_no"] = item.GetValue("SerialNO").ToString();
                            dtbatchdtlrow["SerailSelected"] = "Y";

                            dtbatchdtl.Rows.Add(dtbatchdtlrow);
                        }
                    }
                    DTableSerialDetail = dtbatchdtl;
                }

                ViewBag.DocID = DocumentMenuId;
                ViewBag.Command = CMD;
                ViewBag.TransType = typ;
                if (DTableSerialDetail.Rows.Count > 0)
                    ViewBag.ItemStockSerialWise = DTableSerialDetail;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        //public ActionResult getItemstockSerialWiseAfterInsert(string Sh_Type, string Sh_No, string Sh_Date, string ItemId)
        //{
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        string Br_ID = string.Empty;

        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }

        //        ds = _Shipment_ISERVICES.getItemstockSerialWiseAfterInsert(Comp_ID, Br_ID, Sh_Type, Sh_No, Sh_Date, ItemId);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            ViewBag.ItemStockSerialWise = ds.Tables[0];
        //        }
        //        return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialShipmenItemStockSerialWise.cshtml");
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //}
        //public ActionResult getItemstockWareHouselWise(string ItemId, string CompId, string BranchId)
        //{
        //    try
        //    {
        //        ds = _Shipment_ISERVICES.getItemstockWarehouseWise(ItemId, CompId, BranchId);
        //        if (ds.Tables[0].Rows.Count > 0)
        //            ViewBag.ItemStockWareHouselWise = ds.Tables[0];
        //        return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialShipmenItemStockWareHouseWise.cshtml");
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }

        //}

        //[HttpPost]
        //public JsonResult CheckSaleInvoiceAgainstShipment(string DocNo, string DocDate)
        //{
        //    JsonResult DataRows = null;
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        string Br_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        DataSet Deatils = _Shipment_ISERVICES.CheckSaleInvoiceAgainstShipment(Comp_ID, Br_ID, DocNo, DocDate);
        //        DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //    return DataRows;
        //}

        public string CheckSaleInvoiceAgainstShipment(string DocNo, string DocDate)
        {
            string str = "";
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
                DataSet Deatils = _Shipment_ISERVICES.CheckSaleInvoiceAgainstShipment(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            return str;
        }

        /*--------------------------For PDF Print Start--------------------------*/
        public FileResult GenratePdfFile(ShipmentDetail_MODEL _model)
        {
            PrintOptionsList ProntOption = new PrintOptionsList();
            if (_model.HdnPrintOptons == "Y")
            {
                ProntOption.PrtOpt_catlog_number = _model.PrtOpt_catlog_number == "Y" ? true : false;
                ProntOption.PrtOpt_item_code = _model.PrtOpt_item_code == "Y" ? true : false;
                ProntOption.PrtOpt_item_desc = _model.PrtOpt_item_desc == "Y" ? true : false;
            }
            else
            {
                ProntOption.PrtOpt_catlog_number = true;
                ProntOption.PrtOpt_item_code = true;
                ProntOption.PrtOpt_item_desc = true;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("ItemAliasName", typeof(string));/*Add by Hina on 13-11-2024*/
            dt.Columns.Add("ShowItemRemarks", typeof(string));/*Add by Hina on 13-11-2024*/
            dt.Columns.Add("ShowPackSize", typeof(string));

            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = _model.PrintFormat;
            dtr["ShowProdDesc"] = _model.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
            dtr["ShowSubItem"] = _model.ShowSubItem;
            dtr["CustAliasName"] = _model.CustomerAliasName;
            dtr["ItemAliasName"] = _model.ItemAliasName;/*Add by Hina on 13-11-2024*/
            dtr["ShowItemRemarks"] = _model.ShowItemRemarks;/*Add by Hina on 13-11-2024*/
            dtr["ShowPackSize"] = _model.ShowPackSize;
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;

            string PrintName = "";
            if (_model.print_type == "Commercial")
            {
                PrintName = "CommercialShipmentList";
                return File(GetPdfData(_model.ship_no, _model.ship_dt, _model.print_type, ProntOption), "application/pdf", PrintName + ".pdf");
            }
            else if (_model.print_type == "Custom")
            {
                PrintName = "ExportPackingList";
                return File(GetPdfData(_model.ship_no, _model.ship_dt, _model.print_type, ProntOption), "application/pdf", PrintName + ".pdf");
            }
            else
            {
                PrintName = "Shipment";
                //    return File(GetPdfData(_model.ship_no, _model.ship_dt, _model.print_type,ProntOption), "application/pdf", PrintName + ".pdf");
                return File(GetPdfData(_model.ship_no, _model.ship_dt, "Common", ProntOption), "application/pdf", PrintName + ".pdf");

            }

        }
        public byte[] GetPdfData(string shipmentNo, string shipmentDate, string printType, PrintOptionsList ProntOption)
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
                string ReportType = "custom";//for customer(ERP User Company) specific Report
                DataSet Details = new DataSet();
                if (printType == "Commercial")
                {
                    Details = _Shipment_ISERVICES.GetShipmentDeatilsForPrint(CompID, BrchID, shipmentNo, shipmentDate, "Commercial");
                }
                else if (printType == "Common")
                {
                    Details = _Shipment_ISERVICES.GetShipmentDeatilsForPrint(CompID, BrchID, shipmentNo, shipmentDate, "Common");
                    ReportType = "common";
                    ViewBag.Title = "Delivery Challan";
                    DocumentMenuId = "105103135";
                    ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                }
                else
                {
                    Details = _Shipment_ISERVICES.GetShipmentDeatilsForPrint(CompID, BrchID, shipmentNo, shipmentDate, ReportType);
                }

                ViewBag.PageName = "S";

                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "";
                ViewBag.printType = printType;
                var docstatus = Details.Tables[0].Rows[0]["ship_status"].ToString().Trim();
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
                ViewBag.DocStatus = docstatus;// Details.Tables[0].Rows[0]["ship_status"].ToString().Trim();
                string htmlcontent = "";// ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentPrint.cshtml"));
                string PageTitle = "";
                ViewBag.ProntOption = ProntOption;
                ViewBag.d = DocumentMenuId;
                if (ReportType == "common")
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentPrint.cshtml"));
                }
                else
                {
                    if (printType == "Commercial")
                    {
                        PageTitle = "Commercial Packing List";
                        htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Reports/CustomReports/AlaskaExports/AE_CommercialPackingPrint.cshtml"));
                    }
                    else
                    {
                        PageTitle = "Export Packing List";
                        htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Reports/CustomReports/AlaskaExports/AE_ShipmentPrint.cshtml"));
                    }
                    /*Commented bY Hina on 15-03-2024 to work as per Common print*/
                    //else if (printType == "Custom")
                    //{
                    //    PageTitle = "Export Packing List";
                    //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Reports/CustomReports/AlaskaExports/AE_ShipmentPrint.cshtml"));
                    //}
                    //else
                    //{
                    //    PageTitle = "Shipment";

                    //    //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Reports/CustomReports/AlaskaExports/AE_ShipmentPrint.cshtml"));
                    //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/Shipment/ShipmentPrint.cshtml"));

                    //}
                }

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    if (ReportType == "common")
                    {
                        pdfDoc = new Document(PageSize.A4, 5f, 5f, 20f, 20f);
                    }
                    else
                    {
                        pdfDoc = new Document(PageSize.A4, 20f, 20f, 60f, 90f);
                    }
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = HeaderFooterPagination(bytes, Details, ReportType, PageTitle, printType);
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
        private Byte[] HeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType, string PageTitle, string printType)
        {
            var docstatus = Details.Tables[0].Rows[0]["ship_status"].ToString().Trim();
            var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.NORMAL);
            Font font1 = new Font(bf, 8, Font.NORMAL);
            Font fontb = new Font(bf, 9, Font.NORMAL);
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 13, Font.BOLD);
            fonttitle.SetStyle("underline");
            //string logo = Server.MapPath(Details.Tables[0].Rows[0]["logo"].ToString());
            //string logo =  ConfigurationManager.AppSettings["LocalServerURL"].ToString() +Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            //string logo =  Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string draftImage = string.Empty;
            if (docstatus == "C")
            {
                 draftImage = Server.MapPath("~/Content/Images/cancelled.png");
            }
            else
            {
                 draftImage = Server.MapPath("~/Content/Images/draft.png");
            }
               

            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        if (ReportType == "common")
                        {
                            var draftimg = Image.GetInstance(draftImage);
                            draftimg.SetAbsolutePosition(20, 220);
                            draftimg.ScaleAbsolute(550f, 600f);

                            int PageCount = reader1.NumberOfPages;
                            for (int i = 1; i <= PageCount; i++)
                            {
                                var content = stamper.GetUnderContent(i);
                                if (docstatus == "D" || docstatus == "F" || docstatus == "C")
                                {
                                    content.AddImage(draftimg);
                                }
                                Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                            }

                        }
                        else
                        {
                            var draftimg = Image.GetInstance(draftImage);
                            draftimg.SetAbsolutePosition(20, 220);
                            draftimg.ScaleAbsolute(550f, 600f);

                            int PageCount = reader1.NumberOfPages;
                            for (int i = 1; i <= PageCount; i++)
                            {
                                var content = stamper.GetUnderContent(i);
                                if (docstatus == "D" || docstatus == "F")
                                {
                                    content.AddImage(draftimg);
                                }
                                try
                                {
                                    //var image = Image.GetInstance(logo);
                                    //image.SetAbsolutePosition(31, 794);
                                    //image.ScaleAbsolute(68f, 15f);
                                    //content.AddImage(image);
                                }
                                catch { }

                                content.Rectangle(34.5, 28, 526, 60);
                                string strdate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
                                Phrase pdate = new Phrase(String.Format(strdate, i, PageCount), font);

                                Phrase ptitle = new Phrase(String.Format(PageTitle, i, PageCount), fonttitle);
                                Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                Phrase p4 = new Phrase(String.Format("Declaration :", i, PageCount), fontb);
                                Phrase p8 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontb);
                                Phrase p7 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontb);
                                //----------------------Declaration-----------------------------

                                Phrase p1, p2, p3;
                                //var txt = "";
                                if (printType == "Commercial")
                                {
                                    p1 = new Phrase(String.Format("We certify that the present invoice is true and correct and that is the only one issue", i, PageCount), font1);
                                    p2 = new Phrase(String.Format("by ourselves for the goods described on it and that it's value is exact, without any deduction of", i, PageCount), font1);
                                    p3 = new Phrase(String.Format("advance payment and that goods are exclusively of indian origin", i, PageCount), font1);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
                                }
                                else if (printType == "Custom")
                                {

                                    //-----------------Adding Declaration--------------
                                    BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                                    content.BeginText();
                                    content.SetColorFill(CMYKColor.BLACK);
                                    content.SetFontAndSize(baseFont1, 9);
                                    content.CreateTemplate(20, 10);
                                    content.SetLineWidth(25);

                                    var txt = Details.Tables[0].Rows[0]["declar"].ToString();
                                    string[] stringSeparators = new string[] { "\r\n" };
                                    string text = txt;
                                    string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

                                    var y = 65;
                                    for (var j = 0; j < lines.Length; j++)
                                    {
                                        content.ShowTextAlignedKerned(PdfContentByte.ALIGN_LEFT, lines[j], 40, y, 0);
                                        y = y - 10;
                                    }
                                    //-----------------Adding Declaration--------------

                                    //p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
                                    //p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
                                    //p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);
                                }
                                else
                                {
                                    p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
                                    p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
                                    p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);

                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
                                }


                                //----------------------Declaration End-----------------------------
                                /*------------------Header ---------------------------*/
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, pdate, 560, 794, 0);
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 785, 0);

                                /*------------------Header end---------------------------*/

                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p4, 40, 75, 0);
                                //----------------------Declaration-----------------------------

                                //----------------------Declaration End-----------------------------
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p7, 555, 45, 0);
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p8, 555, 70, 0);
                            }
                        }



                    }
                    bytes = ms.ToArray();
                }
            }

            return bytes;
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
        public ActionResult GetSelectPrintTypePopup(string PrintOptions)
        {
            try
            {
                ViewBag.PrintCommand = "Y";
                ViewBag.PrintOptions = PrintOptions;
                return PartialView("~/Areas/Common/Views/Cmn_SelectPrintType.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }

        /*--------------------------For PDF Print End--------------------------*/
        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            //if (Session["MenuDocumentId"] != null)
            //{
            //    if (Session["MenuDocumentId"].ToString() == "105103135")
            //    {
            //        DocumentMenuId = "105103135";
            //    }
            //    if (Session["MenuDocumentId"].ToString() == "105103145120")
            //    {
            //        DocumentMenuId = "105103145120";
            //    }
            //}

            try
            {
                ShipMentModelattch _ShipMentModelattch = new ShipMentModelattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string shipCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["ShipmentNumber"] != null)
                //{
                //    shipCode = Session["ShipmentNumber"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                _ShipMentModelattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                // getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _ShipMentModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _ShipMentModelattch.AttachMentDetailItmStp = null;
                }
                TempData["Model_attch"] = _ShipMentModelattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        /*-----------------Attachment Section End------------------------*/

        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
   , string Flag, string Status, string SrcDoc_no, string SrcDoc_dt, string Doc_no, string Doc_dt, string DocumentMenuId)
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
                ViewBag.DocumentMenuId = DocumentMenuId;
                DataTable dt = new DataTable();
                if (Flag == "ShipPacked" || Flag == "Shipped")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        dt = _Shipment_ISERVICES.Shipment_GetSubItemDetails(CompID, BrchID, Item_id, SrcDoc_no, SrcDoc_dt, Doc_no, Doc_dt, Flag, DocumentMenuId).Tables[0];
                    }
                    else
                    {
                        dt = _Shipment_ISERVICES.Shipment_GetSubItemDetailsAfterApprove(CompID, BrchID, Item_id, Doc_no, Doc_dt, DocumentMenuId).Tables[0];
                    }
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    //_subitemPageName = "MTO",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y"

                };

                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag == "Quantity" ? Flag : "MTO";
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }


        /*-----------------GL Voucher Posting Start-----------------------*/

        //public DataTable ToDataTable<T>(IList<T> data)
        //{
        //    try
        //    {
        //        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        //        object[] values = new object[props.Count];
        //        using (DataTable table = new DataTable())
        //        {
        //            long _pCt = props.Count;
        //            for (int i = 0; i < _pCt; ++i)
        //            {
        //                PropertyDescriptor prop = props[i];
        //                table.Columns.Add(prop.Name, prop.PropertyType);
        //            }
        //            foreach (T item in data)
        //            {
        //                long _vCt = values.Length;
        //                for (int i = 0; i < _vCt; ++i)
        //                {
        //                    values[i] = props[i].GetValue(item);
        //                }
        //                table.Rows.Add(values);
        //            }
        //            return table;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
        //[HttpPost]
        //public JsonResult GetGLDetails(List<GL_Detail> GLDetail)
        //{
        //    JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
        //    try
        //    {

        //        DataTable DtblGLDetail = new DataTable();

        //        if (GLDetail != null)
        //        {
        //            DtblGLDetail = ToDataTable(GLDetail);
        //        }
        //        DataSet GlDt = _Shipment_ISERVICES.GetAllGLDetails(DtblGLDetail);
        //        Validate = Json(GlDt);
        //        JsonResult DataRows = null;
        //        DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);

        //        return DataRows;
        //    }

        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Validate;
        //}

        //[HttpPost]
        //public JsonResult GetRoundOffGLDetails()
        //{
        //    JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
        //    try
        //    {

        //        string Comp_ID = string.Empty;
        //        string BranchID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            BranchID = Session["BranchId"].ToString();
        //        }
        //        DataSet GlDt = _Shipment_ISERVICES.GetRoundOffGLDetails(Comp_ID, BranchID);
        //        Validate = Json(GlDt);
        //        JsonResult DataRows = null;
        //        DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);

        //        return DataRows;
        //    }

        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Validate;
        //}
        /*-----------------GL Voucher Posting End-----------------------*/
        public string SavePdfDocToSendOnEmailAlert(ShipmentDetail_MODEL _model, string Doc_no, string Doc_dt, string fileName,string docid, string docstatus)
        {
            PrintOptionsList ProntOption = new PrintOptionsList()
            {
                PrtOpt_catlog_number = true,
                PrtOpt_item_code = true,
                PrtOpt_item_desc = true
            };
            DataTable dt = new DataTable();
            var commonCont = new CommonController(_Common_IServices);
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("ItemAliasName", typeof(string));
            dt.Columns.Add("ShowItemRemarks", typeof(string));
            dt.Columns.Add("ShowPackSize", typeof(string));

            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = _model.PrintFormat;
            dtr["ShowProdDesc"] = _model.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
            dtr["ShowSubItem"] = _model.ShowSubItem;
            dtr["CustAliasName"] = _model.CustomerAliasName;
            dtr["ItemAliasName"] = _model.ItemAliasName;
            dtr["ShowItemRemarks"] = _model.ShowItemRemarks;
            dtr["ShowPackSize"] = _model.ShowPackSize;
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, Convert.ToDateTime(Doc_dt).ToString(), "Common", ProntOption);
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "ErrorPage";
            }
            return null;
        }
        public FileResult ExportItemsToExcel(string shipmentNo, string shipmentDate)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            string UserID = "";
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            DocumentMenuId = "105103130";
            //ITEMDETAILS
            DataTable DtItems = _Shipment_ISERVICES.GetShipmentItemsToExportExcel(CompID, BrchID, shipmentNo, shipmentDate);
            var commonController = new CommonController(_Common_IServices);
            return commonController.ExportDatatableToExcel("Shipment_Items", DtItems);
        }
        //Added by Nidhi on 20-08-2025
        public string SavePdfDocToSendOnEmailAlert_Ext(string shpNo, string shpDt, string fileName, string PrintFormat)
        {
            var printOptionsList = JsonConvert.DeserializeObject<List<ShipmentDetail_MODEL>>(PrintFormat);
            var printOptions = printOptionsList.FirstOrDefault();

            PrintOptionsList ProntOption = new PrintOptionsList()
            {
                PrtOpt_catlog_number = true,
                PrtOpt_item_code = true,
                PrtOpt_item_desc = true
            };

            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("ItemAliasName", typeof(string));
            dt.Columns.Add("ShowItemRemarks", typeof(string));
            dt.Columns.Add("ShowPackSize", typeof(string));

            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = printOptions?.PrintFormat;
            dtr["ShowProdDesc"] = printOptions?.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = printOptions?.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = printOptions?.ShowProdTechDesc;
            dtr["ShowSubItem"] = printOptions?.ShowSubItem;
            dtr["CustAliasName"] = printOptions?.CustomerAliasName;
            dtr["ItemAliasName"] = printOptions?.ItemAliasName;
            dtr["ShowItemRemarks"] = printOptions?.ShowItemRemarks;
            dtr["ShowPackSize"] = printOptions?.ShowPackSize;
            dt.Rows.Add(dtr);

            ViewBag.PrintOption = dt;
            var data = GetPdfData(shpNo, shpDt, "Common", ProntOption);
            var commonCont = new CommonController(_Common_IServices);
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
        public ActionResult SendEmailAlert(ShipmentDetail_MODEL _model,string mail_id, string status, string docid, string Doc_no, string Doc_dt, string statusAM, string filepath)
        {
            try
            {
                string UserID = string.Empty;
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
                var commonCont = new CommonController(_Common_IServices);
                //var _Model = TempData["ModelData"] as PODetailsModel;
                DataTable dt = new DataTable();
                string message = "";
                string mail_cont = "";
                string file_path = "";
                if (status == "SH")
                {
                    try
                    {
                        if ((filepath == "" || filepath == null) && (docid == "105103135"))
                        {
                            PrintOptionsList ProntOption = new PrintOptionsList()
                            {
                                PrtOpt_catlog_number = true,
                                PrtOpt_item_code = true,
                                PrtOpt_item_desc = true
                            };
                            dt.Columns.Add("PrintFormat", typeof(string));
                            dt.Columns.Add("ShowProdDesc", typeof(string));
                            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
                            dt.Columns.Add("ShowProdTechDesc", typeof(string));
                            dt.Columns.Add("ShowSubItem", typeof(string));
                            dt.Columns.Add("CustAliasName", typeof(string));
                            dt.Columns.Add("ItemAliasName", typeof(string));
                            dt.Columns.Add("ShowItemRemarks", typeof(string));
                            dt.Columns.Add("ShowPackSize", typeof(string));

                            DataRow dtr = dt.NewRow();
                            dtr["PrintFormat"] = _model.PrintFormat;
                            dtr["ShowProdDesc"] = _model.ShowProdDesc;
                            dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
                            dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
                            dtr["ShowSubItem"] = _model.ShowSubItem;
                            dtr["CustAliasName"] = _model.CustomerAliasName;
                            dtr["ItemAliasName"] = _model.ItemAliasName;
                            dtr["ShowItemRemarks"] = _model.ShowItemRemarks;
                            dtr["ShowPackSize"] = _model.ShowPackSize;
                            dt.Rows.Add(dtr);
                            ViewBag.PrintOption = dt;
                            var data = GetPdfData(Doc_no, Doc_dt, "Common", ProntOption);
                            string fileName = "SHP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            filepath = commonCont.SaveAlertDocument_MailExt(data, fileName);
                        }
                        string keyword = @"\ExternalEmailAlertPDFs\";
                        int index = filepath.IndexOf(keyword);
                        file_path = (index >= 0) ? filepath.Substring(index) : filepath;
                        message = _Common_IServices.SendAlertEmailExternal(CompID, BrchID, UserID, docid, Doc_no, "A", mail_id, filepath);
                        if (message.Contains(","))
                        {
                            var a = message.Split(',');
                            message = a[0];
                            mail_cont = a[1];
                        }
                        if (message == "success")
                        {
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            if (message == "invalidemail")
                            {
                                mail_cont = "Invalid email body configuration";
                            }
                            if (message == "invalid")
                            {
                                mail_cont = "Invalid sender email configuration";
                            }
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                    catch (Exception exMail)
                    {
                        message = "ErrorInMail";
                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                }
                if (status == "C" || status == "FC" || status == "AM")
                {
                    try
                    {
                        message = _Common_IServices.SendAlertEmailExternal1(CompID, BrchID, UserID, docid, Doc_no, status, mail_id);
                        if (message.Contains(","))
                        {
                            var a = message.Split(',');
                            message = a[0];
                            mail_cont = a[1];
                        }
                        if (message == "success")
                        {
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            if (message == "invalidemail")
                            {
                                mail_cont = "Invalid email body configuration";
                            }
                            if (message == "invalid")
                            {
                                mail_cont = "Invalid sender email configuration";
                            }
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                    catch (Exception exMail)
                    {
                        message = "ErrorInMail";
                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }

                }
                return Json(message);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private object IsBlank(string input, object output)//Added by Suraj Maurya on 27-11-2025
        {
            return input == "" ? output : input;
        }
    }

}

