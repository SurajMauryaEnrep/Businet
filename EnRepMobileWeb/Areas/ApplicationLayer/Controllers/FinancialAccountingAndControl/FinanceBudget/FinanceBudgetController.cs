using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.FinanceBudget;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.FinanceBudget;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using static EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.FinanceBudget.FinanceBudgetListModel;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.FinanceBudget
{
    public class FinanceBudgetController : Controller
    {
        string CompID, BrId, UserID, language = String.Empty;
        string DocumentMenuId = "105104105", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();

        Common_IServices _Common_IServices;
        Financebudget_Iservices _financebudget_Iservices;
        public FinanceBudgetController(Common_IServices _Common_IServices, Financebudget_Iservices _financebudget_Iservice)
        {
            this._Common_IServices = _Common_IServices;
            this._financebudget_Iservices = _financebudget_Iservice;
        }
        // GET: ApplicationLayer/FinanceBudget
        public ActionResult DashBordtoList(string docid, string status)
        {
            var WF_status = status;
            return RedirectToAction("FinanceBudget", new { WF_status });
        }
        public ActionResult FinanceBudget(FinanceBudgetListModel _financetBudListModel, string WF_status)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                _financetBudListModel.WF_Status = WF_status;
                CommonPageDetails();
                List<StatusList> statusLists = new List<StatusList>();
                if (ViewBag.StatusList.Rows.Count > 0)
                {
                    foreach (DataRow data in ViewBag.StatusList.Rows)
                    {
                        StatusList _Statuslist = new StatusList();
                        _Statuslist.status_id = data["status_code"].ToString();
                        _Statuslist.status_name = data["status_name"].ToString();
                        statusLists.Add(_Statuslist);
                    }
                }

                _financetBudListModel.statusLists = statusLists;
                List<Finyr> _FinyrList = new List<Finyr>();
                DataTable dt = onchangeGetfinyearsGlAccs("Y");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Finyr _FinYr = new Finyr();
                        _FinYr.FinyrId = dr["id"].ToString();
                        _FinYr.Finyrs = dr["Finyear"].ToString();
                        _FinyrList.Add(_FinYr);

                    }
                }
                _FinyrList.Insert(0, new Finyr() { FinyrId = "0", Finyrs = "All" });
                _financetBudListModel.Finyrlist = _FinyrList;
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _financetBudListModel.WF_Status = TempData["WF_status"].ToString();
                    //if (Session["WF_status"] != null)
                    if (_financetBudListModel.WF_Status != null)
                    {
                        //wfstatus = Session["WF_status"].ToString();
                        wfstatus = _financetBudListModel.WF_Status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                else
                {
                    //if (Session["WF_status"] != null)
                    if (_financetBudListModel.WF_Status != null)
                    {
                        //wfstatus = Session["WF_status"].ToString();
                        wfstatus = _financetBudListModel.WF_Status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var FinancialYear = "";
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    if (ListFilterData != null && ListFilterData != "")
                    {
                        var a = ListFilterData.Split(',');
                        _financetBudListModel.Fin = a[0].Trim();
                        FinancialYear = a[2].Trim();
                        _financetBudListModel.Status = a[1].Trim();
                        if (_financetBudListModel.Status == "0")
                        {
                            //_financetBudListModel.Status = null;
                        }
                        _financetBudListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    }
                    //FinBudSearchedList(FinancialYear, "", _financetBudListModel.Status, _financetBudListModel);
                    DataSet Ds = _financebudget_Iservices.SerachListFinBudget(CompID, BrId, FinancialYear, _financetBudListModel.Status);
                    _financetBudListModel.BudSearch = "";
                    List<Budlist> budlists = new List<Budlist>();
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in Ds.Tables[0].Rows)
                        {
                            Budlist Lst = new Budlist();
                            Lst.Finyr = dr["fy"].ToString();
                            Lst.Period = dr["FbPeriod"].ToString();
                            Lst.Revno = dr["rev_no"].ToString();
                            Lst.Status = dr["Fb_Status"].ToString();
                            Lst.CreateDate = dr["create_dt"].ToString();
                            Lst.ApproveDate = dr["app_dt"].ToString();
                            Lst.ModiDate = dr["mod_dt"].ToString();
                            budlists.Add(Lst);
                        }

                    }
                    _financetBudListModel.BudgetList = budlists;

                }
                else
                {
                    DataSet Ds = _financebudget_Iservices.GetFinanBudList(CompID, BrId, UserID, DocumentMenuId, _financetBudListModel.WF_Status);
                    List<Budlist> budlists = new List<Budlist>();
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in Ds.Tables[0].Rows)
                        {
                            Budlist Lst = new Budlist();
                            Lst.Finyr = dr["fy"].ToString();
                            Lst.Period = dr["Period"].ToString();
                            Lst.Revno = dr["rev_no"].ToString();
                            Lst.Status = dr["fb_status"].ToString();
                            Lst.CreateDate = dr["CreateDate"].ToString();
                            Lst.ApproveDate = dr["ModDate"].ToString();
                            Lst.ModiDate = dr["AmendDate"].ToString();
                            budlists.Add(Lst);
                        }
                    }
                    _financetBudListModel.BudgetList = budlists;
                }
               // var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrId, DocumentMenuId);
                //ViewBag.MenuPageName = getDocumentName();
                ViewBag.DocumentMenuId = DocumentMenuId;
                _financetBudListModel.Title = title;
                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/FinanceBudget/FinanceBudgetList.cshtml", _financetBudListModel);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
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
                ViewBag.MenuPageName = DocumentName;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult AddFinanceBudgetDetail()
        {
           
            FinanceBudgetModel AddNewModel = new FinanceBudgetModel();
            AddNewModel.Command = "Add";
            AddNewModel.TransType = "Save";
            AddNewModel.BtnName = "BtnAddNew";
            AddNewModel.DocumentStatus = "D";
            TempData["ModelData"] = AddNewModel;
            UrlModel _urlModel = new UrlModel();
            _urlModel.bt = "BtnAddNew";
            _urlModel.Cmd = "Add";
            _urlModel.tp = "Save";
            ViewBag.MenuPageName = getDocumentName();
            
            TempData["ListFilterData"] = null;
            return RedirectToAction("FinanceBudgetDetail", "FinanceBudget", _urlModel);
        }

        public ActionResult FinanceBudgetDetail(UrlModel _urlModel)
        {
            try
            {
                var RevMess = string.Empty;
                FinanceBudgetModel _financeBudgetModel = new FinanceBudgetModel();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                var _FunBudget = TempData["ModelData"] as FinanceBudgetModel;
                if (_FunBudget != null)
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrId, DocumentMenuId).Tables[0];
                    List<Finyear> _FinyrList = new List<Finyear>();
                    DataTable dt = GetfinyearsGlAccs("Y");
                    if (dt.Rows.Count>0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Finyear _FinYr = new Finyear();
                            _FinYr.FinyrId = dr["id"].ToString();
                            _FinYr.Finyrs = dr["Finyear"].ToString();
                            _FinyrList.Add(_FinYr);
                        }
                    }
                    _FinyrList.Insert(0, new Finyear() { FinyrId = "0", Finyrs = "---Select---" });
                    _FunBudget.Finyrlist = _FinyrList;

                    List<GlAccounts> _GlList = new List<GlAccounts>();
                    DataTable DaTa = GetfinyearsGlAccs("");
                    foreach (DataRow dr in DaTa.Rows)
                    {
                        GlAccounts _GlAcs = new GlAccounts();
                        _GlAcs.GlaccId = dr["acc_id"].ToString();
                        _GlAcs.Glaccname = dr["acc_name"].ToString();
                        _GlList.Add(_GlAcs);

                    }
                    _GlList.Insert(0, new GlAccounts() { GlaccId = "0", Glaccname = "---Select---" });
                    _FunBudget.Glacclist = _GlList;
                    if(_FunBudget.Message == "Revised")
                    {
                        RevMess = "Revised";
                    }
                    else
                    {
                        RevMess = "";
                    }
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _FunBudget.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_FunBudget.TransType == "Update" || _FunBudget.TransType == "Edit")
                    {
                        DataSet ds = _financebudget_Iservices.GetFinBudgetDetail(CompID, BrId, _FunBudget.FinYears, _FunBudget.Revno, UserID, _FunBudget.Period, DocumentMenuId,RevMess);
                        _FunBudget.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _FunBudget.Createdon = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _FunBudget.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _FunBudget.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _FunBudget.AmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _FunBudget.AmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _FunBudget.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _FunBudget.FinYr = ds.Tables[0].Rows[0]["Finyr_Id"].ToString();
                        _FunBudget.FinYears = ds.Tables[0].Rows[0]["fy"].ToString();
                        _FunBudget.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _FunBudget.Revno = ds.Tables[0].Rows[0]["rev_no"].ToString();
                        _FunBudget.FinBudDate = ds.Tables[0].Rows[0]["FBudDate"].ToString();
                        _FunBudget.SFinYears= ds.Tables[0].Rows[0]["fy_sdt"].ToString();
                        _FunBudget.EFinYears = ds.Tables[0].Rows[0]["fy_edt"].ToString();
                        _FunBudget.BgtStatus = ds.Tables[0].Rows[0]["bgtstatus"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _FunBudget.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _FunBudget.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        _FinyrList.Insert(0, new Finyear() { FinyrId = ds.Tables[0].Rows[0]["Finyr_Id"].ToString(), Finyrs = ds.Tables[0].Rows[0]["fy"].ToString() });
                        _FunBudget.Finyrlist = _FinyrList;

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        if (ViewBag.AppLevel != null && _FunBudget.Command != "Edit")
                        {
                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _FunBudget.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                        }
                                        _FunBudget.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        _FunBudget.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    _FunBudget.BtnName = "BtnToDetailPage";
                                }

                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        _FunBudget.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    _FunBudget.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                    }
                                    _FunBudget.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _FunBudget.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _FunBudget.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.CostCenterData = ds.Tables[6];
                        ViewBag.Gldetails = ds.Tables[1];
                        ViewBag.BudQtrMonDetails = ds.Tables[5];
                        _FunBudget.DocumentStatus = Statuscode;
                        ViewBag.DocumentStatus = _FunBudget.DocumentStatus;
                        ViewBag.TransType = _FunBudget.TransType;
                        ViewBag.Command = _FunBudget.Command;
                        ViewBag.VBRoleList = GetRoleList();
                        var other = new CommonController(_Common_IServices);
                        ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrId, DocumentMenuId);
                        ViewBag.MenuPageName = getDocumentName();

                        if (_FunBudget.Message == "Revised")
                        {
                            _FunBudget.DocumentStatus = "D";
                            _FunBudget.TransType = "Save";
                            ViewBag.DocumentStatus = "D";
                        }
                        _FunBudget.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/FinanceBudget/FinanceBudgetDetail.cshtml", _FunBudget);
                    }

                    else
                    {
                        ViewBag.DocumentStatus = _FunBudget.DocumentStatus;
                        ViewBag.TransType = _FunBudget.TransType;
                        ViewBag.Command = _FunBudget.Command;
                        ViewBag.VBRoleList = GetRoleList();
                        var other = new CommonController(_Common_IServices);
                        ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrId, DocumentMenuId);
                        ViewBag.MenuPageName = getDocumentName();
                        _FunBudget.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/FinanceBudget/FinanceBudgetDetail.cshtml", _FunBudget);
                    }
                }
                else
                {
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _financeBudgetModel.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _financeBudgetModel.BtnName = _urlModel.bt;
                        }
                        _financeBudgetModel.Command = _urlModel.Cmd;
                        _financeBudgetModel.TransType = _urlModel.tp;
                        _financeBudgetModel.WF_Status1 = _urlModel.wf;
                        _financeBudgetModel.Revno = _urlModel.Rvn;
                        _financeBudgetModel.FinYears = _urlModel.Fy;
                        _financeBudgetModel.Period = _urlModel.Per;
                    }
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrId, DocumentMenuId).Tables[0];
                        List<Finyear> _FinyrList = new List<Finyear>();
                        DataTable dt = GetfinyearsGlAccs("Y");
                    if (dt.Rows.Count>0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Finyear _FinYr = new Finyear();
                            _FinYr.FinyrId = dr["id"].ToString();
                            _FinYr.Finyrs = dr["Finyear"].ToString();
                            _FinyrList.Add(_FinYr);

                        }
                    }
                        _FinyrList.Insert(0, new Finyear() { FinyrId = "0", Finyrs = "---Select---" });
                        _financeBudgetModel.Finyrlist = _FinyrList;

                        List<GlAccounts> _GlList = new List<GlAccounts>();
                        DataTable DaTa = GetfinyearsGlAccs("");
                        foreach (DataRow dr in DaTa.Rows)
                        {
                            GlAccounts _GlAcs = new GlAccounts();
                            _GlAcs.GlaccId = dr["acc_id"].ToString();
                            _GlAcs.Glaccname = dr["acc_name"].ToString();
                            _GlList.Add(_GlAcs);

                        }
                        _GlList.Insert(0, new GlAccounts() { GlaccId = "0", Glaccname = "---Select---" });
                        _financeBudgetModel.Glacclist = _GlList;
                    if (_financeBudgetModel.Message == "Revised")
                    {
                        RevMess = "Revised";
                    }
                    else
                    {
                        RevMess = "";
                    }
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _financeBudgetModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_financeBudgetModel.TransType == "Update" || _financeBudgetModel.TransType == "Edit")
                        {
                            DataSet ds = _financebudget_Iservices.GetFinBudgetDetail(CompID, BrId, _financeBudgetModel.FinYears, _financeBudgetModel.Revno, UserID, _financeBudgetModel.Period, DocumentMenuId, RevMess);
                            _financeBudgetModel.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _financeBudgetModel.Createdon = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            _financeBudgetModel.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                            _financeBudgetModel.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                            _financeBudgetModel.AmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                            _financeBudgetModel.AmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            _financeBudgetModel.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                            _financeBudgetModel.FinYr = ds.Tables[0].Rows[0]["Finyr_Id"].ToString();
                            _financeBudgetModel.FinYears = ds.Tables[0].Rows[0]["fy"].ToString();
                            _financeBudgetModel.FinBudDate = ds.Tables[0].Rows[0]["FBudDate"].ToString();
                            _financeBudgetModel.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _financeBudgetModel.Revno = ds.Tables[0].Rows[0]["rev_no"].ToString();
                        _financeBudgetModel.SFinYears = ds.Tables[0].Rows[0]["fy_sdt"].ToString();
                        _financeBudgetModel.EFinYears = ds.Tables[0].Rows[0]["fy_edt"].ToString();
                        _financeBudgetModel.BgtStatus = ds.Tables[0].Rows[0]["bgtstatus"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                            string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _financeBudgetModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _financeBudgetModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        _FinyrList.Insert(0, new Finyear() { FinyrId = ds.Tables[0].Rows[0]["Finyr_Id"].ToString(), Finyrs = ds.Tables[0].Rows[0]["fy"].ToString() });
                        _financeBudgetModel.Finyrlist = _FinyrList;

                        if (Statuscode != "D" && Statuscode != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[4];
                            }
                            if (ViewBag.AppLevel != null && _financeBudgetModel.Command != "Edit")
                            {

                                var sent_to = "";
                                var nextLevel = "";
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                                }

                                if (ds.Tables[3].Rows.Count > 0)
                                {
                                    nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                                }

                                if (Statuscode == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                    _financeBudgetModel.BtnName = "Refresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                            }
                                        _financeBudgetModel.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                        _financeBudgetModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                    _financeBudgetModel.BtnName = "BtnToDetailPage";
                                    }

                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                        _financeBudgetModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                    _financeBudgetModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                        }
                                    _financeBudgetModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                    _financeBudgetModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                    _financeBudgetModel.BtnName = "Refresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            ViewBag.CostCenterData = ds.Tables[6];
                            ViewBag.Gldetails = ds.Tables[1];
                            ViewBag.BudQtrMonDetails = ds.Tables[5];
                            ViewBag.VBRoleList = GetRoleList();
                            _financeBudgetModel.DocumentStatus = Statuscode;
                            ViewBag.DocumentStatus = _financeBudgetModel.DocumentStatus;
                            ViewBag.TransType = _financeBudgetModel.TransType;
                            ViewBag.Command = _financeBudgetModel.Command;
                        ViewBag.MenuPageName = getDocumentName();
                        _financeBudgetModel.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/FinanceBudget/FinanceBudgetDetail.cshtml", _financeBudgetModel);
                        }
                        else
                        {
                            _financeBudgetModel.DocumentStatus = "D";
                            ViewBag.TransType = _financeBudgetModel.TransType;
                            ViewBag.DocumentStatus = _financeBudgetModel.DocumentStatus;
                            ViewBag.Command = _financeBudgetModel.Command;
                            ViewBag.VBRoleList = GetRoleList();
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrId, DocumentMenuId);
                            ViewBag.MenuPageName = getDocumentName();
                            _financeBudgetModel.Title = title;
                            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/FinanceBudget/FinanceBudgetDetail.cshtml", _financeBudgetModel);
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult FinancebudgetSave(FinanceBudgetModel _financeBudgetModel, string command)
        {
            try
            {
                if (_financeBudgetModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                if (command == null)
                {
                    if (_financeBudgetModel.HdnCsvPrint == "CsvPrint")
                    {
                        command = "CsvPrint";
                    }
                }
                switch (command)
                {
                    case "AddNew":
                        FinanceBudgetModel adddnew = new FinanceBudgetModel();
                        adddnew.Command = "Add";
                        adddnew.TransType = "Save";
                        adddnew.BtnName = "BtnAddNew";
                        adddnew.DocumentStatus = "D";
                        UrlModel NewModel = new UrlModel();
                        NewModel.Cmd = "Add";
                        NewModel.tp = "Save";
                        NewModel.bt = "BtnAddNew";
                        NewModel.Fy = _financeBudgetModel.FinYears;
                        NewModel.Rvn = _financeBudgetModel.Revno;
                        NewModel.Per = _financeBudgetModel.Period;
                        TempData["ModelData"] = adddnew;
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("FinanceBudgetDetail", NewModel);

                    case "Edit":
                        _financeBudgetModel.TransType = "Update";
                        _financeBudgetModel.Command = command;
                        _financeBudgetModel.BtnName = "BtnEdit";
                        TempData["ModelData"] = _financeBudgetModel;
                        UrlModel EditModel = new UrlModel();
                        EditModel.tp = "Update";
                        EditModel.Cmd = command;
                        EditModel.bt = "BtnEdit";
                        EditModel.Fy = _financeBudgetModel.FinYears;
                        EditModel.Rvn = _financeBudgetModel.Revno;
                        EditModel.Per = _financeBudgetModel.Period;
                        TempData["ListFilterData"] = _financeBudgetModel.ListFilterData1;
                        return RedirectToAction("FinanceBudgetDetail", EditModel);
                    case "Save":
                        _financeBudgetModel.Command = command;
                        SaveFinanceBudget(_financeBudgetModel);
                        if (_financeBudgetModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        var other = new CommonController(_Common_IServices);
                        ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrId, DocumentMenuId);
                        ViewBag.VBRoleList = GetRoleList();                        
                        UrlModel _urlModel = new UrlModel();
                        _urlModel.bt = _financeBudgetModel.BtnName;
                        _urlModel.Cmd = _financeBudgetModel.Command;
                        string DocNo1 = string.Concat(_financeBudgetModel.FinYears, '_', _financeBudgetModel.Revno);
                        _urlModel.VN = DocNo1;
                        _urlModel.tp = _financeBudgetModel.TransType;
                        _urlModel.Fy = _financeBudgetModel.FinYears;
                        _urlModel.Rvn = _financeBudgetModel.Revno;
                        _urlModel.Per = _financeBudgetModel.Period;
                        TempData["ModelData"] = _financeBudgetModel;
                        TempData["ListFilterData"] = _financeBudgetModel.ListFilterData1;
                        return RedirectToAction("FinanceBudgetDetail", _urlModel);
                    case "BacktoList":
                        TempData["ListFilterData"] = _financeBudgetModel.ListFilterData1;
                        TempData["WF_status"] = _financeBudgetModel.WF_Status1;
                        return RedirectToAction("FinanceBudget", "FinanceBudget");

                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        _financeBudgetModel.Command = command;
                        FinanceBudgetApprove(_financeBudgetModel, _financeBudgetModel.FinYears, _financeBudgetModel.Revno, _financeBudgetModel.Period, _financeBudgetModel.FinBudDate, "", "", "", "", "");
                        TempData["ModelData"] = _financeBudgetModel;
                        UrlModel urlref = new UrlModel();
                        urlref.tp = "Update";
                        string DocNo = string.Concat(_financeBudgetModel.FinYears, '_', _financeBudgetModel.Revno);
                        urlref.VN = DocNo;
                        urlref.tp = _financeBudgetModel.TransType;
                        urlref.Fy = _financeBudgetModel.FinYears;
                        urlref.Rvn = _financeBudgetModel.Revno;
                        urlref.Per = _financeBudgetModel.Period;
                        urlref.bt = "BtnEdit";
                        if (_financeBudgetModel.WF_Status1 != null)
                        {
                            urlref.wf = _financeBudgetModel.WF_Status1;
                        }
                        TempData["ListFilterData"] = _financeBudgetModel.ListFilterData1;
                        return RedirectToAction("FinanceBudgetDetail", urlref);
                    case "Refresh":
                        FinanceBudgetModel RefreshModel = new FinanceBudgetModel();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh = new UrlModel();
                        refesh.tp = "Save";
                        refesh.bt = "Refresh";
                        refesh.Cmd = command;
                        TempData["ListFilterData"] = _financeBudgetModel.ListFilterData1;
                        return RedirectToAction("FinanceBudgetDetail", refesh);
                    case "Delete":
                        _financeBudgetModel.Command = command;
                        FinanceBudgetDelete(_financeBudgetModel, command);
                        FinanceBudgetModel DeleteModel = new FinanceBudgetModel();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete = new UrlModel();
                        Delete.Cmd = DeleteModel.Command;
                        Delete.tp = "Refresh";
                        Delete.bt = "BtnDelete";
                        TempData["ListFilterData"] = _financeBudgetModel.ListFilterData1;
                        return RedirectToAction("FinanceBudgetDetail", Delete);
                    case "Revision":
                        _financeBudgetModel.Command = command;
                        _financeBudgetModel.BtnName = "BtnEdit";
                        _financeBudgetModel.Command = "Edit";
                        _financeBudgetModel.Message = "Revised";
                        _financeBudgetModel.DocumentStatus = "D";
                        _financeBudgetModel.TransType = "Update";
                        UrlModel Url_Revise = new UrlModel();
                        Url_Revise.tp = "Update";
                        Url_Revise.bt = "Edit";
                        Url_Revise.Fy = _financeBudgetModel.FinYears;
                        Url_Revise.Rvn = _financeBudgetModel.Revno;
                        Url_Revise.Per = _financeBudgetModel.Period;
                        TempData["ModelData"] = _financeBudgetModel;
                        TempData["ListFilterData"] = _financeBudgetModel.ListFilterData1;
                        return RedirectToAction("FinanceBudgetDetail", Url_Revise);
                    case "CsvPrint":
                        _financeBudgetModel.HdnCsvPrint = null;
                        return financeBudgetExporttoExcelDt(_financeBudgetModel);                        
                    default:
                        return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FileResult financeBudgetExporttoExcelDt(FinanceBudgetModel _financeBudgetModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                DataTable dt = new DataTable();
                DataSet ds = _financebudget_Iservices.GetFinBudgetDetail(CompID, BrId, _financeBudgetModel.FinYears, _financeBudgetModel.Revno, UserID, _financeBudgetModel.Period, DocumentMenuId, "");

                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("GL Account", typeof(string));
                dt.Columns.Add("GL Group", typeof(string));
                dt.Columns.Add("Budget Amount", typeof(decimal));
                dt.Columns.Add("Remarks", typeof(string));

                if (ds.Tables[1].Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        dtrowLines["Sr.No"] = rowno + 1;
                        dtrowLines["GL Account"] = dr["acc_name"].ToString();
                        dtrowLines["GL Group"] = dr["acc_group_name"].ToString();
                        dtrowLines["Budget Amount"] = dr["bgt_amt"].ToString();
                        dtrowLines["Remarks"] = dr["remarks"].ToString();
                        dt.Rows.Add(dtrowLines);
                        rowno = rowno + 1;
                    }
                }
                _financeBudgetModel.HdnCsvPrint = null;
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("FinanceBudget", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        //-------------Budget save
        public ActionResult SaveFinanceBudget(FinanceBudgetModel financeBudgetModel)
        {
            string SaveMessage = "";
            try
            {
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }

                DataTable FinBudHeader = new DataTable();
                DataTable FinBugGlDetails = new DataTable();
                DataTable FinMonQtrDetails = new DataTable();
                DataTable FBCostCenterDetails = new DataTable();

                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("CompId", typeof(int));
                dtheader.Columns.Add("BrId", typeof(int));
                dtheader.Columns.Add("DocumentmenuId", typeof(string));
                dtheader.Columns.Add("Revno", typeof(int));
                dtheader.Columns.Add("FinYear", typeof(string));
                dtheader.Columns.Add("FbStatus", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("remarks", typeof(string));
                dtheader.Columns.Add("Transtype", typeof(string));
                dtheader.Columns.Add("user_id", typeof(int));
                
                dtheader.Columns.Add("StartDate", typeof(string));
                dtheader.Columns.Add("EndDate", typeof(string));
                DataRow dtrowHeader = dtheader.NewRow();

                dtrowHeader["CompId"] = CompID;
                dtrowHeader["BrId"] = BrId;
                dtrowHeader["DocumentmenuId"] = DocumentMenuId;
                if(financeBudgetModel.Revno != "" && financeBudgetModel.Revno != null)
                {
                    dtrowHeader["Revno"] = financeBudgetModel.Revno;
                }
                else
                {
                    dtrowHeader["Revno"] = 0;
                }
                dtrowHeader["FinYear"] = financeBudgetModel.FinYears;
                dtrowHeader["FbStatus"] = "D";
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["remarks"] = "";
                dtrowHeader["Transtype"] = financeBudgetModel.TransType;
                dtrowHeader["user_id"] = Convert.ToInt32(Session["UserId"]);
                dtrowHeader["StartDate"] = financeBudgetModel.SFinYears;
                dtrowHeader["EndDate"] = financeBudgetModel.EFinYears; 
                dtheader.Rows.Add(dtrowHeader);
                FinBudHeader = dtheader;

                DataTable FinbudAccDetais = new DataTable();

                FinbudAccDetais.Columns.Add("acc_id", typeof(string));
                FinbudAccDetais.Columns.Add("bud_amount", typeof(float));
                FinbudAccDetais.Columns.Add("remarks", typeof(string));

                JArray jObject = JArray.Parse(financeBudgetModel.Fin_Gldetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = FinbudAccDetais.NewRow();
                    dtrowLines["acc_id"] = jObject[i]["acc_id"].ToString();
                    dtrowLines["bud_amount"] = jObject[i]["bud_amount"].ToString();
                    dtrowLines["remarks"] = jObject[i]["remarks"].ToString();
                    FinbudAccDetais.Rows.Add(dtrowLines);
                }
                FinBugGlDetails = FinbudAccDetais;

                DataTable QtrMnthDetails = new DataTable();

                QtrMnthDetails.Columns.Add("acc_id", typeof(int));
                QtrMnthDetails.Columns.Add("bud_type", typeof(string));
                QtrMnthDetails.Columns.Add("mnth_qtr_name", typeof(string));
                QtrMnthDetails.Columns.Add("bgt_amt", typeof(string));
                QtrMnthDetails.Columns.Add("sqno", typeof(string));

                JArray BObject = JArray.Parse(financeBudgetModel.FinBudQuatMonList);

                for (int i = 0; i < BObject.Count; i++)
                {
                    DataRow dtrowLines = QtrMnthDetails.NewRow();
                    dtrowLines["acc_id"] = Convert.ToInt32(BObject[i]["acc_id"]);
                    dtrowLines["bud_type"] = BObject[i]["bud_type"].ToString();
                    dtrowLines["mnth_qtr_name"] = BObject[i]["mnth_qtr_name"].ToString();
                    dtrowLines["bgt_amt"] = BObject[i]["bgt_amt"];
                    dtrowLines["sqno"] = BObject[i]["sqno"];
                    QtrMnthDetails.Rows.Add(dtrowLines);
                }
                FinMonQtrDetails = QtrMnthDetails;

                DataTable CC_Details = new DataTable();
                /**----------------Cost Center Section--------------------*/

                CC_Details.Columns.Add("acc_id", typeof(string));
                CC_Details.Columns.Add("cc_id", typeof(int));
                CC_Details.Columns.Add("cc_val_id", typeof(int));
                CC_Details.Columns.Add("cc_amt", typeof(float));


                JArray JAObj = JArray.Parse(financeBudgetModel.CC_DetailList);
                for (int i = 0; i < JAObj.Count; i++)
                {
                    DataRow dtrowLines = CC_Details.NewRow();

                    dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                    dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                    dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                    dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();

                    CC_Details.Rows.Add(dtrowLines);
                }
                FBCostCenterDetails = CC_Details;
                

                /**----------------Cost Center Section End--------------------*/

                SaveMessage = _financebudget_Iservices.InsertFinbudDetails(FinBudHeader, FinBugGlDetails, FinMonQtrDetails, FBCostCenterDetails);

                string Message = SaveMessage.Split(',')[0].Trim();
                string revNo = SaveMessage.Split(',')[1].Trim();
                financeBudgetModel.Message = "Save";
                financeBudgetModel.Command = "Update";
                financeBudgetModel.TransType = "Update";
                financeBudgetModel.BtnName = "BtnToDetailPage";
                financeBudgetModel.Revno = revNo;
                return RedirectToAction("FinanceBudgetDetail");
                
            }
            catch (Exception ex)
            {
                throw ex;
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
                return null;
            }
        }
        public DataTable GetApprovalLevel(string Comp_ID, string Br_ID, string DocumentMenuId)
        {
            try
            {

                DataSet ApprovalLevelList = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId);
                DataTable DT_AppLevel = new DataTable();
                DT_AppLevel = ApprovalLevelList.Tables[0];
                return DT_AppLevel;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData)
        {
            FinanceBudgetModel _FinbudModel = new FinanceBudgetModel();
            UrlModel urlModel = new UrlModel();
            var a = ModelData.Split('_');
            _FinbudModel.FinYears = a[0].Trim();
            var b = a[1].Split(',');
            _FinbudModel.Revno = b[0].Trim();
            _FinbudModel.Period = string.Concat(b[1].Trim(),',',b[2].Trim());
            _FinbudModel.TransType = "Update";
            _FinbudModel.BtnName = "BtnToDetailPage";
            
            urlModel.bt = "D";
            urlModel.Fy = _FinbudModel.FinYears;
            urlModel.Rvn = _FinbudModel.Revno;
            urlModel.Per = _FinbudModel.Period;
            urlModel.Cmd = "Update";
            urlModel.tp = "Update";
            TempData["ModelData"] = _FinbudModel;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("FinanceBudgetDetail", urlModel);
        }

        // For Gl accounts and Financial year Dropdowns
        public DataTable GetfinyearsGlAccs(string flag)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                DataTable dt = _financebudget_Iservices.GetFinYearList(CompID, BrId, flag);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }

        public DataTable onchangeGetfinyearsGlAccs(string flag)
        {
            try
            {
               
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                DataTable dt = _financebudget_Iservices.GetFinYearListpage(CompID, BrId, flag);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        [HttpPost]
        public JsonResult GlAccountList()
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
                    BrId = Session["BranchId"].ToString();
                }
                DataTable result = _financebudget_Iservices.GlList(CompID);
                DataRows = Json(JsonConvert.SerializeObject(result));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public ActionResult GetMonQuatAllocation(string BudAmount, string FinYear, string Flag, string Acc_Id, string Bud_RowData, string DisFlag)
        {
            FinanceBudgetModel _financeBudgetModel = new FinanceBudgetModel();
            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));

            if (Bud_RowData != null && Bud_RowData != "" && Bud_RowData != "[]")
            {
                DataTable BudData = new DataTable();
                BudData.Columns.Add("GlAccId", typeof(string));
                BudData.Columns.Add("Type", typeof(string));
                BudData.Columns.Add("Monname", typeof(string));
                BudData.Columns.Add("period", typeof(string));
                BudData.Columns.Add("Amount", typeof(string));
                BudData.Columns.Add("Total", typeof(string));
                BudData.Columns.Add("Sqno", typeof(int));
                BudData.Columns.Add("qtr_sno", typeof(int));
                JArray arr = JArray.Parse(Bud_RowData);
                for (int i = 0; i < arr.Count; i++)
                {
                    DataRow dtrow = BudData.NewRow();
                    dtrow["GlAccId"] = arr[i]["glacc"].ToString();
                    dtrow["Type"] = arr[i]["type"].ToString();
                    dtrow["Monname"] = arr[i]["monname"].ToString();
                    dtrow["period"] = arr[i]["period"].ToString();
                    dtrow["Amount"] = arr[i]["amount"].ToString();
                    dtrow["Total"] = arr[i]["total"].ToString();
                    dtrow["Sqno"] = arr[i]["Sqno"];
                    dtrow["qtr_sno"] = arr[i]["qtr_sno"];
                    BudData.Rows.Add(dtrow);
                }
                List<ListAll> lstall = new List<ListAll>();

                for (int i = 0; i < BudData.Rows.Count; i++)
                {
                    ListAll _lst = new ListAll();
                    _lst.allmonths = BudData.Rows[i]["Monname"].ToString();
                    _lst.allperiod = BudData.Rows[i]["period"].ToString();
                    _lst.allamount = BudData.Rows[i]["Amount"].ToString();
                    _lst.alltype = BudData.Rows[i]["type"].ToString();
                    _lst.Sqno = Convert.ToInt32(BudData.Rows[i]["Sqno"].ToString());
                    _lst.qtr_sno = Convert.ToInt32(BudData.Rows[i]["qtr_sno"].ToString());
                    lstall.Add(_lst);
                }
                _financeBudgetModel.ListAlls = lstall;

                DataView dv = new DataView(BudData);

                string fflag = Flag.Trim();

                if (fflag == "M")
                {
                    dv.RowFilter = "Type = 'M'";
                }
                else
                {
                    dv.RowFilter = "Type = 'Q'";
                }
                List<monthlist> lstmont = new List<monthlist>();
                dv.Sort = "Sqno asc";
                for (int i = 0; i < dv.Count; i++)
                {
                    monthlist _lst = new monthlist();
                    _lst.month = dv[i]["Monname"].ToString();
                    _lst.monPeriod = dv[i]["period"].ToString();
                    _lst.monAmount = dv[i]["Amount"].ToString();
                    _lst.Sqn = Convert.ToInt32(dv[i]["Sqno"].ToString());
                    _lst.qtr_sno = Convert.ToInt32(dv[i]["qtr_sno"].ToString());
                    lstmont.Add(_lst);
                }
                _financeBudgetModel.Monthlists = lstmont;
                _financeBudgetModel.Acc_Id = Acc_Id;
                _financeBudgetModel.TotalAmount = BudAmount;
            }

            if ((Flag == "") && (BudAmount != null && BudAmount != "") && (FinYear != null || FinYear != "" && FinYear != "0") && (Acc_Id != "0" && Acc_Id != null))
            {
                var startDate = FinYear.Split(',').First();
                DateTime StDate = Convert.ToDateTime(startDate);
                string date_from = StDate.ToString("yyyy/dd/MM");
                DateTimeFormatInfo dtFI = new DateTimeFormatInfo();
                DateTime currentDate = Convert.ToDateTime(date_from);
                DateTime nextyearDate = currentDate.AddYears(1).AddDays(-1);
                List<ListAll> lstmont = new List<ListAll>();
                var i = 0;
                var qtrsno = 1;
                while (currentDate < nextyearDate)
                {
                    i = i + 1;
                    ListAll _new = new ListAll();
                    var EndPeriod = ((currentDate.AddMonths(1)).AddDays(-1)).ToString("dd-MM-yyyy");
                    var period = string.Concat(currentDate.ToString("dd-MM-yyyy"), " to ", EndPeriod);
                    _new.allmonths = dtFI.GetMonthName(currentDate.Month);
                    _new.allperiod = period;
                    _new.allamount = (Convert.ToDecimal(BudAmount) / 12).ToString(ValDigit);
                    _new.alltype = "M";
                    _new.Sqno = i;
                    _new.qtr_sno = qtrsno;
                    if (i % 3 == 0)
                    {
                        qtrsno = qtrsno + 1;
                    }
                    lstmont.Add(_new);
                    currentDate = currentDate.AddMonths(1);

                }
                DateTime CurrDate = Convert.ToDateTime(date_from);
                int j = 0;
                i = 0;
                while (CurrDate < nextyearDate)
                {

                    j = j + 1;
                    ListAll _new1 = new ListAll();
                    var EndPeriod = ((CurrDate.AddMonths(3)).AddDays(-1)).ToString("dd-MM-yyyy");
                    var period = string.Concat(CurrDate.ToString("dd-MM-yyyy"), " to ", EndPeriod);
                    _new1.allmonths = string.Concat("Quarter", " ", i + 1);
                    _new1.allperiod = period;
                    _new1.allamount = (Convert.ToDecimal(BudAmount) / 4).ToString(ValDigit);
                    _new1.alltype = "Q";
                    _new1.Sqno = j;
                    _new1.qtr_sno = j;
                    lstmont.Add(_new1);
                    CurrDate = CurrDate.AddMonths(3);
                    i = i + 1;

                }
                _financeBudgetModel.ListAlls = lstmont;
                _financeBudgetModel.TotalAmount = BudAmount;
                _financeBudgetModel.Acc_Id = Acc_Id;
            }

            _financeBudgetModel.DisableFlag = DisFlag;
            _financeBudgetModel.PeriodFlag = Flag;

            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMonthlyAllocation.cshtml", _financeBudgetModel);
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
        public ActionResult EditVou(string FB_Yr, string FB_Dt, string Rev_No,string FB_Per, string ListFilterData, string WF_Status)
        {
           
            FinanceBudgetModel dblclick = new FinanceBudgetModel();
            UrlModel _url = new UrlModel();
            dblclick.Command = "Update";
            dblclick.FinYears = FB_Yr;
            dblclick.Revno = Rev_No;
            dblclick.Createdon = FB_Dt;
            dblclick.TransType = "Update";
            dblclick.BtnName = "BtnToDetailPage";
            dblclick.Period = FB_Per;
            
            if (WF_Status != null && WF_Status != "")
            {
                _url.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            _url.Cmd = "Update";
            _url.tp = "Update";
            _url.bt = "D";
            _url.Fy = FB_Yr;
            _url.Rvn = Rev_No;
            _url.Per = FB_Per;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("FinanceBudgetDetail", "FinanceBudget", _url);
        }

        // Budget Delete
        private ActionResult FinanceBudgetDelete(FinanceBudgetModel _financebudget, string command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                string Message = _financebudget_Iservices.FinBudDelete(CompID, BrId, _financebudget);
                return RedirectToAction("FinanceBudgetDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
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

        // List Page Search
        public ActionResult FinBudSearchedList(string Finyear, string Revno, string Status, FinanceBudgetListModel _financeBudgetListModel)
        {
            //FinanceBudgetListModel _financeBudgetListModel = new FinanceBudgetListModel();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrId = Session["BranchId"].ToString();
            }
            DataSet Ds = _financebudget_Iservices.SerachListFinBudget(CompID, BrId, Finyear, Status);
            _financeBudgetListModel.BudSearch = "BudSearch";
            List<Budlist> budlists = new List<Budlist>();
            if (Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    Budlist Lst = new Budlist();
                    Lst.Finyr = dr["fy"].ToString();
                    Lst.Period = dr["FbPeriod"].ToString();
                    Lst.Revno = dr["rev_no"].ToString();
                    Lst.Status = dr["Fb_Status"].ToString();
                    Lst.CreateDate = dr["create_dt"].ToString();
                    Lst.ApproveDate = dr["app_dt"].ToString();
                    Lst.ModiDate = dr["mod_dt"].ToString();
                    budlists.Add(Lst);
                }

            }
            _financeBudgetListModel.BudgetList = budlists;

            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialFinanceBudgetList.cshtml", _financeBudgetListModel);
        }
        // Finance Budget Approve
        public ActionResult FinanceBudgetApprove(FinanceBudgetModel _FinanceBudgetModel, string FY, string RevNo, string Period, string FB_Date, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1)
        {
            try
            {
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _financebudget_Iservices.ApproveFinanceBudgetDetails(FY, RevNo, FB_Date, CompID, BrId, UserID, A_Status, A_Level, A_Remarks, mac_id, DocumentMenuId);
                
                UrlModel urlapp_model = new UrlModel();
                _FinanceBudgetModel.TransType = "Update";
                _FinanceBudgetModel.Message = "Approved";
                _FinanceBudgetModel.BtnName = "BtnEdit";
                _FinanceBudgetModel.FinYears = FY;
                _FinanceBudgetModel.Revno = RevNo;
                _FinanceBudgetModel.Period = Period;
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _FinanceBudgetModel.WF_Status1 = WF_Status1;
                    urlapp_model.wf = WF_Status1;
                }
                TempData["ModelData"] = _FinanceBudgetModel;
                urlapp_model.tp = "Update";
                urlapp_model.bt = "BtnEdit";
                urlapp_model.Fy = _FinanceBudgetModel.FinYears;
                urlapp_model.Rvn = _FinanceBudgetModel.Revno;
                urlapp_model.Per = _FinanceBudgetModel.Period;
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("FinanceBudgetDetail", "FinanceBudget", urlapp_model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        
        /*------------- Cost Center Section-----------------*/
        public ActionResult GetCstCntrtype(string Flag, string Disableflag, string CC_rowdata)
        {
            try
            {
                CostCenterDt _CC_Model = new CostCenterDt();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }

                if (CC_rowdata.ToString() != null && CC_rowdata.ToString() != "" && CC_rowdata.ToString() != "[]")
                {
                    DataTable Cctype = new DataTable();
                    Cctype.Columns.Add("GlAccount", typeof(string));
                    Cctype.Columns.Add("ddl_cc_type", typeof(string));
                    Cctype.Columns.Add("dd_cc_type_id", typeof(string));
                    JArray arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow dtrowLines = Cctype.NewRow();
                        dtrowLines["GlAccount"] = arr[i]["GlAccount"].ToString();
                        dtrowLines["ddl_cc_type"] = arr[i]["ddl_CC_Type"].ToString();
                        dtrowLines["dd_cc_type_id"] = arr[i]["ddl_Type_Id"].ToString();
                        Cctype.Rows.Add(dtrowLines);
                    }
                    DataView dv = new DataView();
                    dv = Cctype.DefaultView;
                    ViewBag.CC_type = dv.ToTable(true, "GlAccount", "ddl_cc_type", "dd_cc_type_id");

                    DataTable ccitem = new DataTable();
                    ccitem.Columns.Add("dd_cc_typ_id", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name_id", typeof(string));
                    ccitem.Columns.Add("cc_Amount", typeof(string));

                    JArray Arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow DtrowLines = ccitem.NewRow();
                        DtrowLines["dd_cc_typ_id"] = arr[i]["ddl_Type_Id"].ToString();
                        DtrowLines["ddl_cc_name"] = arr[i]["ddl_CC_Name"].ToString();
                        DtrowLines["ddl_cc_name_id"] = arr[i]["ddl_Name_Id"].ToString();
                        DtrowLines["cc_Amount"] = arr[i]["CC_Amount"].ToString();
                        ccitem.Rows.Add(DtrowLines);
                    }
                    ViewBag.CC_Item = ccitem;
                }

                DataSet ds = _Common_IServices.GetCstCntrData(CompID, BrId, "0", Flag);

                List<CostcntrType> Cctypelist = new List<CostcntrType>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CostcntrType Cc_type = new CostcntrType();
                    Cc_type.cc_id = dr["cc_id"].ToString();
                    Cc_type.cc_name = dr["cc_name"].ToString();
                    Cctypelist.Add(Cc_type);
                }
                Cctypelist.Insert(0, new CostcntrType() { cc_id = "0", cc_name = "---Select---" });
                _CC_Model.costcntrtype = Cctypelist;

                _CC_Model.disflag = Disableflag;

                return PartialView("~/Areas/Common/Views/Cmn_PartialCostCenterDetail.cshtml", _CC_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public JsonResult GetCstCntrName(string CCtypeid)
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
                    BrId = Session["BranchId"].ToString();
                }
                DataSet ds = _Common_IServices.GetCstCntrData(CompID, BrId, CCtypeid, "ccname");
                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        /*------------- Cost Center Section END ----------------*/
    }
}