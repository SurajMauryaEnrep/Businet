using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.BusinessLayer.CurrencySetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.CurrencySetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.CurrencySetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.CurrencySetup
{
    public class CurrencySetupController : Controller
    {
        string CompID, branchID, user_id, language = String.Empty;
        string DocumentMenuId = "103120", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        CurrencySetup_ISERVICE CurrencySetup_ISERVICE;
        public CurrencySetupController(Common_IServices _Common_IServices ,CurrencySetup_SERVICE CurrencySetup_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.CurrencySetup_ISERVICE = CurrencySetup_ISERVICE;
        }
        // GET: BusinessLayer/CurrencySetup
        public ActionResult CurrencySetup()
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
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                var CRLIST_Model = TempData["Modeldata"] as CurrencySetup_Model;
                if (CRLIST_Model != null)
                {
                    if (CRLIST_Model.Message == "Save")
                    {
                        CRLIST_Model.Message = "Save";
                    }if (CRLIST_Model.Message == "Update")
                    {
                        CRLIST_Model.Message = "Save";
                    }
                    if (CRLIST_Model.Message == "Duplicate")
                    {
                        CRLIST_Model.Message = "Duplicate";
                    }
                   // DataSet ds = CurrencySetup_ISERVICE.GetCurrancyTable(CompID);
                    DataSet Alldata = CurrencySetup_ISERVICE.GetAllData(CompID, branchID);
                    ViewBag.CurrConvList = Alldata.Tables[1];
                    ViewBag.currancysetup = Alldata.Tables[0];
                    DataTable dt = new DataTable();
                    List<CurrencyNameLIst> currencyNameLIst = new List<CurrencyNameLIst>();
                    //dt = GetCurrencySetup();
                    foreach (DataRow dr in Alldata.Tables[2].Rows)
                    {
                        CurrencyNameLIst _CLList = new CurrencyNameLIst();
                        _CLList.curr_id = Convert.ToInt32(dr["curr_id"]);
                        _CLList.curr_name = dr["curr_name"].ToString();
                        currencyNameLIst.Add(_CLList);
                    }
                    currencyNameLIst.Insert(0, new CurrencyNameLIst() { curr_id = 0, curr_name = "---Select---" });
                    CRLIST_Model._currencyNameList = currencyNameLIst;
                    CRLIST_Model.hdnAction = null;
                    CRLIST_Model.Curr_name = 0;
                    CRLIST_Model.Price = null;
                    CRLIST_Model.Eff_Date = null;
                    CRLIST_Model.Curr_id = 0;
                    CRLIST_Model.Title = title;
                    return View("~/Areas/BusinessLayer/Views/CurrencySetup/CurrencySetup.cshtml", CRLIST_Model);
                }
                else
                {
                    CurrencySetup_Model CRLIST_Model1 = new CurrencySetup_Model();
                  //  DataSet ds = CurrencySetup_ISERVICE.GetCurrancyTable(CompID);
                    DataSet Alldata = CurrencySetup_ISERVICE.GetAllData(CompID, branchID);
                    ViewBag.CurrConvList = Alldata.Tables[1];
                    ViewBag.currancysetup = Alldata.Tables[0];
                    DataTable dt = new DataTable();
                    List<CurrencyNameLIst> currencyNameLIst = new List<CurrencyNameLIst>();
                    //dt = GetCurrencySetup();
                    foreach (DataRow dr in Alldata.Tables[2].Rows)
                    {
                        CurrencyNameLIst _CLList = new CurrencyNameLIst();
                        _CLList.curr_id = Convert.ToInt32(dr["curr_id"]);
                        _CLList.curr_name = dr["curr_name"].ToString();
                        currencyNameLIst.Add(_CLList);
                    }
                    currencyNameLIst.Insert(0, new CurrencyNameLIst() { curr_id = 0, curr_name = "---Select---" });
                    CRLIST_Model1._currencyNameList = currencyNameLIst;
                    CRLIST_Model1.Title = title;
                    return View("~/Areas/BusinessLayer/Views/CurrencySetup/CurrencySetup.cshtml", CRLIST_Model1);
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
        private DataTable GetCurrencySetup()
        {
            try
            {
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
                DataTable dt = CurrencySetup_ISERVICE.GetCurrencyList(CompID, BrchID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult SaveCurrencySetup(CurrencySetup_Model CRLIST_Model, string command)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            try
            {
                if (CRLIST_Model.hdnAction == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "Delete":
                        //Session["Command"] = command;
                        CRLIST_Model.Command = command;
                        DeleteCurrencyDetails(CRLIST_Model, command);
                        TempData["ModelData"] = CRLIST_Model;
                        return RedirectToAction("CurrencySetup");
                    case "Save":
                        //Session["Command"] = command;
                        CRLIST_Model.Command = command;
                        SaveCurrencyDetails(CRLIST_Model,command);
                        TempData["ModelData"] = CRLIST_Model;
                        return RedirectToAction("CurrencySetup");
                    case "Update":
                        //Session["Command"] = command;
                        CRLIST_Model.Command = command;
                        Updatevalue(CRLIST_Model, command);
                        TempData["Modeldata"] = CRLIST_Model;
                        return RedirectToAction("CurrencySetup");
                    default:
                        return new EmptyResult();
                }  
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private ActionResult Updatevalue(CurrencySetup_Model CRLIST_Model, string Command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                var transtype = "Update";
                var currencyid = CRLIST_Model.Curr_name;
                var Price = CRLIST_Model.Price.ToString();
                var Date = CRLIST_Model.Eff_Date.ToString();
                string ds = CurrencySetup_ISERVICE.InsertCurr(CompID, currencyid, Price, Date, transtype);
              
                if (ds == "Update" || ds == "Save")
                {
                    CRLIST_Model.Message = "Save";
                }
                TempData["Modeldata"] = CRLIST_Model;
                return RedirectToAction("CostCenterSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult SaveCurrencyDetails(CurrencySetup_Model CRLIST_Model, string command)
        {
            try
            {
                var transtype = "Save";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                var currencyid = CRLIST_Model.Curr_name;
                var Price = CRLIST_Model.Price.ToString();
                var Date = CRLIST_Model.Eff_Date.ToString();
                string ds = CurrencySetup_ISERVICE.InsertCurr(CompID, currencyid, Price, Date, transtype);
                if(ds == "Save")
                {
                    CRLIST_Model.Message = "Save";
                }
                if (ds == "Duplicate")
                {
                    CRLIST_Model.Message = "Duplicate";
                }
                TempData["ModelData"] = CRLIST_Model;
                return RedirectToAction("CurrencySetup", CRLIST_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult DeleteCurrencyDetails(CurrencySetup_Model CRLIST_Model, string command)
        {
            try
            { 
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                var transtype = CRLIST_Model.hdnAction;
                var currencyid = CRLIST_Model.Curr_id;
                var Date = CRLIST_Model.hdnConv_date.ToString();
                string ds = CurrencySetup_ISERVICE.DeleteCurrDetail(CompID, currencyid, Date, transtype);
                if (ds == "Delete")
                {
                    CRLIST_Model.Message = "Delete";
                }
                TempData["ModelData"] = CRLIST_Model;
                return RedirectToAction("CurrencySetup");
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
    }
}