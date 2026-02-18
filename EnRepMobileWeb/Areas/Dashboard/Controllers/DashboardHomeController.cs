using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using EnRepMobileWeb.SERVICES.ISERVICES.Dashboard_ISERVICE;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Globalization;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;

namespace EnRepMobileWeb.Areas.Dashboard.Controllers
{
    [NoDirectAccess]
    public class DashboardHomeController : Controller
    {
        string compId = "", brId = "", userId = "";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataSet MenuDs, BranchDs;
        MENU_ISERVICES _MENU_ISERVICES;
        Dashboard_ISERVICE _Dashboard_ISERVICE;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public DashboardHomeController(MENU_ISERVICES _MENU_ISERVICES, Dashboard_ISERVICE _Dashboard_ISERVICE, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._MENU_ISERVICES = _MENU_ISERVICES;
            this._Dashboard_ISERVICE = _Dashboard_ISERVICE;
            this._GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        public ActionResult Logout()
        {
            try
            {
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }            
        }
        public ActionResult Index()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);

                Session["MenuPageName"] = "HomePage";
                Session["Message"] = "New";
                DASHBOARD_MODEL _DASHBOARD_MODEL = new DASHBOARD_MODEL();

                MenuDs = new DataSet();
                string Comp_ID = string.Empty;
                string Language = string.Empty;
                string userid = string.Empty;
                if (Session["CompId"] != null)
                    Comp_ID = Session["CompId"].ToString();
                if (Session["Language"] != null)
                    Language = Session["Language"].ToString();
                if (Session["userid"] != null)
                    userid = Session["userid"].ToString();
                MenuDs = _MENU_ISERVICES.GetAllMenuDAL(Comp_ID, Language, userid);

                Session["Menu1"] = MenuDs.Tables[0];
                Session["Menu2"] = MenuDs.Tables[1];
                Session["Menu3"] = MenuDs.Tables[2];
                Session["Menu4"] = MenuDs.Tables[3];
                ViewBag.UserPermision = MenuDs.Tables[4];
                if (MenuDs.Tables[5].Rows.Count > 0)
                {
                    ViewBag.DefaultBrch = MenuDs.Tables[5].Rows[0]["br_id"].ToString();
                    if (MenuDs.Tables[5].Rows[0]["br_id"].ToString() != "" && MenuDs.Tables[5].Rows[0]["br_id"].ToString() != null)
                    {
                        if (Session["BranchId"] == null)
                        {
                            Session["BranchId"] = MenuDs.Tables[5].Rows[0]["br_id"].ToString();
                        }
                        if (Session["BranchId"] != null)
                        {
                            if (Session["BranchId"].ToString() == "")
                            {
                                Session["BranchId"] = MenuDs.Tables[5].Rows[0]["br_id"].ToString();
                            }
                        }
                    }
                }
                if (Session["BranchId"] != null)
                {
                    brId = Session["BranchId"].ToString();
                }
                BranchDs = _MENU_ISERVICES.GetAllTopNavBrchList(Comp_ID, userid, brId);
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (DataRow dr in BranchDs.Tables[0].Rows)
                {
                    list.Add(new SelectListItem
                    {
                        Text = dr["comp_nm"].ToString(),
                        Value = dr["Comp_id"].ToString()
                    });
                }
                DataSet dttbl = new DataSet();
                dttbl = GetFyList();
                if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
                {
                    ViewBag.CFYStartDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                    ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                    ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();
                    ViewBag.fylist = dttbl.Tables[1];
                }
                Session["BranchList"] = list;
                Session["Available"] = BranchDs.Tables[1].Rows[0]["user_avl"].ToString();

             
                Session["User_Img"] = BranchDs.Tables[1].Rows[0]["user_img"].ToString();
                Session["CompLogo"] = BranchDs.Tables[2].Rows[0]["comp_logo"].ToString();
                ViewBag.br_list = BranchDs.Tables[0];
                ViewBag.vb_br_id = Session["BranchId"];
                ViewBag.MenuPageName = Session["MenuPageName"].ToString();
                return View("Index");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }            
        }
        public void OnBranchChange(string Branch, string BranchName)
        {
            try
            {
                Session["BranchId"] = Branch;
                Session["BranchName"] = BranchName;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        public void SetSessionMenuDocumentId(string MenuDocumentId, string MenuPageName)
        {
            try
            {
                Session["MenuDocumentId"] = MenuDocumentId;
                Session["MenuPageName"] = MenuPageName;
                Session["WF_status"] = null;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        [HttpPost]
        public JsonResult Get_AllDashboardData(string Dateflag, string Fromdt, string Todt, string Top, string Charttype, string Flag)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string User_ID = string.Empty;
                string Br_ID = string.Empty;
                string Lang = string.Empty;
                Session.Remove("WF_status");
                if (Session["CompId"] != null)
                    Comp_ID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                    User_ID = Session["UserId"].ToString();
                if (Session["Language"] != null)
                    Lang = Session["Language"].ToString();
                DataSet result = _Dashboard_ISERVICE.GetDashboardData(Comp_ID, Br_ID, Dateflag, Fromdt, Todt, Top, Charttype, User_ID, Lang, Flag);
                int ctbl = result.Tables.Count - 2;//add by shubham maurya on 21-01-2025 for curr_format table 
                if (Flag== "def_frmt")//Added by Suraj Maurya on 23-12-2024 for getting only default currency format
                    Session["curr_format"] = result.Tables[0].Rows[0]["curr_format"].ToString();
                else
                    Session["curr_format"] = result.Tables[ctbl].Rows[0]["curr_format"].ToString();
                //result.Tables[0].Columns.Add("currformat", typeof(string));
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
        [HttpPost]
        public void SetSession_FromDateAndToDt(string DBFromDt, string DBToDt)
        {
            try
            {
                Session["DBFromDt"] = DBFromDt;
                Session["DBToDt"] = DBToDt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        public void togglesave(string value)
        {
            try
            {
                string User_ID = string.Empty;
                if (Session["UserId"] != null)
                    User_ID = Session["UserId"].ToString();
                DataSet result = _Dashboard_ISERVICE.Updateavlvalue(User_ID, value);
                var avl = result.Tables[0].Rows[0]["user_avl"].ToString();
                Session["Available"] = avl.ToString();
                // return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        [HttpPost]
        public JsonResult GetPendingDocument()/*Added this Function by Nitesh 05-12-2023 For Pending Document when Dashbord user role is not Enable*/
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string User_ID = string.Empty;
                string Br_ID = string.Empty;
                string Lang = string.Empty;
                Session.Remove("WF_status");
                if (Session["CompId"] != null)
                    Comp_ID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                    User_ID = Session["UserId"].ToString();
                if (Session["Language"] != null)
                    Lang = Session["Language"].ToString();
                DataSet result = _Dashboard_ISERVICE.GetPendingDocument(Comp_ID, Br_ID, User_ID, Lang);
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
        public JsonResult SaveUpdateFavouriteMenuDetails(string act, string docId)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                int result = _MENU_ISERVICES.SaveUpdateMyFavouriteMenu(act, compId, userId, docId);
                if (result > 0)
                    return Json("success");
                else
                    return Json("Already exist");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public JsonResult CheckMyFavMenu(string docId)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                DataSet dsResult = _MENU_ISERVICES.GetMyFavMenuDetails(compId, userId, docId);
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                        return Json("exist");
                    else
                        return Json("not exist");
                }
                else
                {
                    return Json("not exist");
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }            
        }
        public JsonResult GetMyFavLiItems()
        {
            try
            {
                string Comp_ID = string.Empty;
                string Language = string.Empty;
                string userid = string.Empty;
                if (Session["CompId"] != null)
                    Comp_ID = Session["CompId"].ToString();
                if (Session["Language"] != null)
                    Language = Session["Language"].ToString();
                if (Session["userid"] != null)
                    userid = Session["userid"].ToString();
                RefreshMenuPageSession();
                DataSet ds = _MENU_ISERVICES.GetMyFavListItems(Comp_ID, Language, userid);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return Json(JsonConvert.SerializeObject(ds.Tables[0]), JsonRequestBehavior.AllowGet);
                    }
                }
                return null;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }            
        }
        public void RefreshMenuPageSession()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);

                Session["MenuPageName"] = "HomePage";
                Session["Message"] = "New";
                DASHBOARD_MODEL _DASHBOARD_MODEL = new DASHBOARD_MODEL();

                MenuDs = new DataSet();
                string Comp_ID = string.Empty;
                string Language = string.Empty;
                string userid = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                MenuDs = _MENU_ISERVICES.GetAllMenuDAL(Comp_ID, Language, userid);

                Session["Menu1"] = MenuDs.Tables[0];
                Session["Menu2"] = MenuDs.Tables[1];
                Session["Menu3"] = MenuDs.Tables[2];
                Session["Menu4"] = MenuDs.Tables[3];
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
                DataSet dt = _GeneralLedger_ISERVICE.Get_FYList(Comp_ID, BrID);
                return dt;
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