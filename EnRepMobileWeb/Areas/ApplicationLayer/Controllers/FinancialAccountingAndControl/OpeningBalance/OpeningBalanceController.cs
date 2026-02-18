using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.OpeningBalance;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.OpeningBalance;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.tool.xml;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using OfficeOpenXml;
using System.Configuration;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.OpeningBalance
{
    public class OpeningBalanceController : Controller
    {
        string CompID, BrchID, language, UserID, FromDate = String.Empty;
        string DocumentMenuId = "105104101", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        OpeningBalance_ISERVICES _OpeningBalance_ISERVICES;
        //OpeningBalanceModel _OpeningBalanceModel;
        DataTable dt;
        public OpeningBalanceController(Common_IServices _Common_IServices, OpeningBalance_ISERVICES _OpeningBalance_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._OpeningBalance_ISERVICES = _OpeningBalance_ISERVICES;
        }
        // GET: ApplicationLayer/OpeningBalance
        public ActionResult OpeningBalance()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            OpeningBalanceModel _OpeningBalanceModel = new OpeningBalanceModel();
            BrchID = Session["BranchId"].ToString();
            ViewBag.MenuPageName = getDocumentName();
            DataTable dtn = new DataTable();
            dtn = GetOpeningBalFinYearList();
            ViewBag.FinYearDetail = dtn;
            //Session["Disabled"] = null;
            _OpeningBalanceModel.Disabled = null;
            ViewBag.FinYear = dtn.Rows[0]["TotalFinYear"].ToString();
            ViewBag.VBRoleList = GetRoleList();           
            _OpeningBalanceModel.Title = title;
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/OpeningBalance/OpeningBalanceList.cshtml", _OpeningBalanceModel);
        }
        public ActionResult AddOpeningBalanceDetail()
        {
            OpeningBalanceModel AddNewModel = new OpeningBalanceModel();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["Disabled"] = "Disabled";
            AddNewModel.Command = "Add";
            AddNewModel.TransType = "Save";
            AddNewModel.BtnName = "BtnAddNew";
            AddNewModel.Disabled = "Disabled";
            AddNewModel.AppStatus = "D";
            TempData["ModelData"] = AddNewModel;
            var trnstyp = "Save";
            var Btn = "BtnAddNew";
            var Disabled = "Disabled";
            var Cmd = "Add";
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("OpeningBalanceDetail", "OpeningBalance", new { trnstyp, Btn, Disabled, Cmd });
        }
        public ActionResult OpeningBalanceDetail(string trnstyp, string Btn, string Disabled, string Cmd, string AccID, string Finyear)
        {
            try
            {
                var _OpeningBalanceModel1 = TempData["ModelData"] as OpeningBalanceModel;
                if (_OpeningBalanceModel1 != null)
                {
                    // OpeningBalanceModel _OpeningBalanceModel1 = new OpeningBalanceModel();
                    DataTable dtn = new DataTable();
                    dtn = GetOpeningBalFinYearList();
                    ViewBag.FinYear = dtn.Rows[0]["TotalFinYear"].ToString();
                    // _OpeningBalanceModel1 = new OpeningBalanceModel();
                    _OpeningBalanceModel1.Hdn_fin_year = Finyear;
                    List<CoaName> _ListCoa = new List<CoaName>();
                    dt = Getcoa(_OpeningBalanceModel1);
                    foreach (DataRow dr in dt.Rows)
                    {
                        CoaName _Coa = new CoaName();
                        _Coa.acc_id = dr["acc_id"].ToString();
                        _Coa.acc_name = dr["acc_name"].ToString();
                        _ListCoa.Add(_Coa);
                    }
                    _OpeningBalanceModel1.CoaNameList = _ListCoa;

                    GetOpeningDate(_OpeningBalanceModel1);
                    // Session["OpBalFindt"] = Session["OpFindate"];
                    _OpeningBalanceModel1.OpBalFindt = _OpeningBalanceModel1.OpFindate;
                    //_OpeningBalanceModel1.fin_year = Session["OpFin_year"].ToString(); 
                    if (_OpeningBalanceModel1.Finyear != null && _OpeningBalanceModel1.Finyear != "")
                    {
                        _OpeningBalanceModel1.fin_year = _OpeningBalanceModel1.Finyear;
                    }
                    else
                    {
                        _OpeningBalanceModel1.fin_year = _OpeningBalanceModel1.OpFin_year;
                    }
                    _OpeningBalanceModel1.hd_Finyear = _OpeningBalanceModel1.fin_year;

                    List<curr> _currList = new List<curr>();
                    dt = Getcurr();
                    foreach (DataRow dr in dt.Rows)
                    {
                        curr _curr = new curr();
                        _curr.curr_id = dr["curr_id"].ToString();
                        _curr.curr_name = dr["curr_name"].ToString();
                        _currList.Add(_curr);

                    }
                    _currList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                    _OpeningBalanceModel1.currList = _currList;
                    string disabled = "";
                    //if (Session["Disabled"] != null)
                    //{
                    //    disabled = Session["Disabled"].ToString();
                    //}
                    if (_OpeningBalanceModel1.Disabled != null)
                    {
                        disabled = _OpeningBalanceModel1.Disabled;
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_OpeningBalanceModel1.TransType == "Update" || _OpeningBalanceModel1.TransType == "Edit")
                    {

                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }
                        BrchID = Session["BranchId"].ToString();
                        //string Acc_id= Session["Acc_id"].ToString();
                        //string Fin_year= Session["Fin_year"].ToString();
                        string Acc_id = _OpeningBalanceModel1.AccID;
                        string Fin_year = _OpeningBalanceModel1.Finyear;

                        DataSet ds = _OpeningBalance_ISERVICES.GetBillwiseOpeningDetail(CompID, BrchID, Acc_id, Fin_year);
                        _OpeningBalanceModel1.acc_type = Convert.ToInt32(ds.Tables[1].Rows[0]["acc_type"].ToString());
                        _OpeningBalanceModel1.acc_id = Convert.ToInt32(ds.Tables[1].Rows[0]["acc_id"].ToString());
                        _OpeningBalanceModel1.acc_grp_id = Convert.ToInt32(ds.Tables[1].Rows[0]["acc_grp_id"].ToString());
                        _OpeningBalanceModel1.acc_grp_name = ds.Tables[1].Rows[0]["acc_group_name"].ToString();
                        _OpeningBalanceModel1.fin_year = ds.Tables[1].Rows[0]["fin_year"].ToString();
                        _OpeningBalanceModel1.op_bal_sp = ds.Tables[1].Rows[0]["op_bal_sp"].ToString();
                        _OpeningBalanceModel1.op_bal_type = ds.Tables[1].Rows[0]["bal_type"].ToString();
                        _OpeningBalanceModel1.curr = Convert.ToInt32(ds.Tables[1].Rows[0]["curr"].ToString());
                        _OpeningBalanceModel1.conv_rate = ds.Tables[1].Rows[0]["conv_rate"].ToString();
                        _OpeningBalanceModel1.op_bal_bs = ds.Tables[1].Rows[0]["op_bal_bs"].ToString();
                        _OpeningBalanceModel1.creat_dt = ds.Tables[1].Rows[0]["CreateDate"].ToString();
                        _OpeningBalanceModel1.createby = ds.Tables[1].Rows[0]["create_id"].ToString();
                        ViewBag.BillDeatils = ds.Tables[0];
                        ViewBag.OpBilldetai = ds.Tables[0];
                        ViewBag.BillsTotalBal = ds.Tables[1].Rows[0]["op_bal_sp"].ToString();
                        ViewBag.BillsTotalBalType = ds.Tables[1].Rows[0]["BalType"].ToString();
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        ViewBag.ItemDetails = GetOpeningBalDetailList(_OpeningBalanceModel1);
                        ViewBag.FinYear = ds.Tables[3].Rows[0]["TotalFinYear"].ToString();
                        ViewBag.PaidAmt = ds.Tables[2].Rows[0]["paid_amt"].ToString();
                        //if (Session["Command"] != null)
                        if (_OpeningBalanceModel1.Command != null)
                        {
                            //if (Session["Command"].ToString() == "Edit")
                            if (_OpeningBalanceModel1.Command == "Edit")
                            {
                                if (Convert.ToInt32(ViewBag.PaidAmt) > 0)
                                {
                                    //Session["Message"] = "CanNotEdit";
                                    //Session["Command"] = "Update";
                                    //Session["Acc_id"] = Acc_id;
                                    //Session["Fin_year"] = Fin_year;
                                    //Session["TransType"] = "Update";
                                    //Session["AppStatus"] = 'D';
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _OpeningBalanceModel1.Message = "CanNotEdit";
                                    _OpeningBalanceModel1.Command = "Update";
                                    _OpeningBalanceModel1.AccID = Acc_id;
                                    _OpeningBalanceModel1.Finyear = Fin_year;
                                    _OpeningBalanceModel1.TransType = "Update";
                                    _OpeningBalanceModel1.AppStatus = "D";
                                    _OpeningBalanceModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                        }
                        ViewBag.MenuPageName = getDocumentName();
                        DataTable dt = getSalesPersonList();
                        List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                        foreach (DataRow data in dt.Rows)
                        {
                            SalePersonList _SlPrsnDetail = new SalePersonList();
                            _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                            _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                            _SlPrsnList.Add(_SlPrsnDetail);
                        }
                        _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                        _OpeningBalanceModel1.SalePersonList = _SlPrsnList;
                        _OpeningBalanceModel1.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/OpeningBalance/OpeningBalanceDetail.cshtml", _OpeningBalanceModel1);
                    }
                    else
                    {
                        if (disabled == "Disabled")
                        {

                        }
                        else
                        {
                            if (_OpeningBalanceModel1.BtnName != null)
                            {
                                // Session["BtnName"] = "Refresh";
                                _OpeningBalanceModel1.BtnName = "Refresh";
                            }
                        }
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        DataTable dt = getSalesPersonList();
                        List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                        foreach (DataRow data in dt.Rows)
                        {
                            SalePersonList _SlPrsnDetail = new SalePersonList();
                            _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                            _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                            _SlPrsnList.Add(_SlPrsnDetail);
                        }
                        _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                        _OpeningBalanceModel1.SalePersonList = _SlPrsnList;
                        _OpeningBalanceModel1.Title = title;
                        ViewBag.ItemDetails = GetOpeningBalDetailList(_OpeningBalanceModel1);
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/OpeningBalance/OpeningBalanceDetail.cshtml", _OpeningBalanceModel1);
                    }
                }
                else
                {
                    //string trnstyp,string Btn,string Disabled,string Cmd                    
                    OpeningBalanceModel _OpeningBalanceModel = new OpeningBalanceModel();
                    _OpeningBalanceModel.Command = Cmd;
                    _OpeningBalanceModel.TransType = trnstyp;
                    _OpeningBalanceModel.Disabled = Disabled;
                    _OpeningBalanceModel.BtnName = Btn;
                    _OpeningBalanceModel.AccID = AccID;
                    _OpeningBalanceModel.Finyear = Finyear;
                    _OpeningBalanceModel.hd_Finyear = Finyear;
                    _OpeningBalanceModel.Hdn_fin_year = Finyear;

                    DataTable dtn = new DataTable();
                    dtn = GetOpeningBalFinYearList();
                    ViewBag.FinYear = dtn.Rows[0]["TotalFinYear"].ToString();
                    //  _OpeningBalanceModel = new OpeningBalanceModel();

                    List<CoaName> _ListCoa = new List<CoaName>();
                    dt = Getcoa(_OpeningBalanceModel);
                    foreach (DataRow dr in dt.Rows)
                    {
                        CoaName _Coa = new CoaName();
                        _Coa.acc_id = dr["acc_id"].ToString();
                        _Coa.acc_name = dr["acc_name"].ToString();
                        _ListCoa.Add(_Coa);
                    }
                    _OpeningBalanceModel.CoaNameList = _ListCoa;

                    GetOpeningDate(_OpeningBalanceModel);
                    // Session["OpBalFindt"] = Session["OpFindate"];
                    _OpeningBalanceModel.OpBalFindt = _OpeningBalanceModel.OpFindate;
                    //_OpeningBalanceModel.fin_year = Session["OpFin_year"].ToString(); 
                    _OpeningBalanceModel.fin_year = _OpeningBalanceModel.OpFin_year;

                    List<curr> _currList = new List<curr>();
                    dt = Getcurr();
                    foreach (DataRow dr in dt.Rows)
                    {
                        curr _curr = new curr();
                        _curr.curr_id = dr["curr_id"].ToString();
                        _curr.curr_name = dr["curr_name"].ToString();
                        _currList.Add(_curr);

                    }
                    _currList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                    _OpeningBalanceModel.currList = _currList;
                    string disabled = "";
                    //if (Session["Disabled"] != null)
                    //{
                    //    disabled = Session["Disabled"].ToString();
                    //}
                    if (_OpeningBalanceModel.Disabled != null)
                    {
                        disabled = _OpeningBalanceModel.Disabled;
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_OpeningBalanceModel.TransType == "Update" || _OpeningBalanceModel.TransType == "Edit")
                    {

                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }
                        BrchID = Session["BranchId"].ToString();
                        //string Acc_id= Session["Acc_id"].ToString();
                        //string Fin_year= Session["Fin_year"].ToString();
                        string Acc_id = _OpeningBalanceModel.AccID;
                        string Fin_year = _OpeningBalanceModel.Finyear;

                        DataSet ds = _OpeningBalance_ISERVICES.GetBillwiseOpeningDetail(CompID, BrchID, Acc_id, Fin_year);
                        _OpeningBalanceModel.acc_type = Convert.ToInt32(ds.Tables[1].Rows[0]["acc_type"].ToString());
                        _OpeningBalanceModel.acc_id = Convert.ToInt32(ds.Tables[1].Rows[0]["acc_id"].ToString());
                        _OpeningBalanceModel.acc_grp_id = Convert.ToInt32(ds.Tables[1].Rows[0]["acc_grp_id"].ToString());
                        _OpeningBalanceModel.acc_grp_name = ds.Tables[1].Rows[0]["acc_group_name"].ToString();
                        _OpeningBalanceModel.op_bal_sp = ds.Tables[1].Rows[0]["op_bal_sp"].ToString();
                        _OpeningBalanceModel.op_bal_type = ds.Tables[1].Rows[0]["bal_type"].ToString();
                        _OpeningBalanceModel.curr = Convert.ToInt32(ds.Tables[1].Rows[0]["curr"].ToString());
                        _OpeningBalanceModel.conv_rate = ds.Tables[1].Rows[0]["conv_rate"].ToString();
                        _OpeningBalanceModel.op_bal_bs = ds.Tables[1].Rows[0]["op_bal_bs"].ToString();
                        _OpeningBalanceModel.creat_dt = ds.Tables[1].Rows[0]["CreateDate"].ToString();
                        _OpeningBalanceModel.createby = ds.Tables[1].Rows[0]["create_id"].ToString();
                        ViewBag.BillDeatils = ds.Tables[0];
                        ViewBag.OpBilldetai = ds.Tables[0];
                        ViewBag.BillsTotalBal = ds.Tables[1].Rows[0]["op_bal_sp"].ToString();
                        ViewBag.BillsTotalBalType = ds.Tables[1].Rows[0]["BalType"].ToString();
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        ViewBag.ItemDetails = GetOpeningBalDetailList(_OpeningBalanceModel);
                        ViewBag.FinYear = ds.Tables[3].Rows[0]["TotalFinYear"].ToString();
                        ViewBag.PaidAmt = ds.Tables[2].Rows[0]["paid_amt"].ToString();
                        if (_OpeningBalanceModel.Command != null)
                        {
                            if (_OpeningBalanceModel.Command == "Edit")
                            {
                                if (Convert.ToInt32(ViewBag.PaidAmt) > 0)
                                {
                                    _OpeningBalanceModel.Message = "CanNotEdit";
                                    _OpeningBalanceModel.Command = "Update";
                                    _OpeningBalanceModel.AccID = Acc_id;
                                    _OpeningBalanceModel.Finyear = Fin_year;
                                    _OpeningBalanceModel.TransType = "Update";
                                    _OpeningBalanceModel.AppStatus = "D";
                                    _OpeningBalanceModel.BtnName = "BtnToDetailPage";
                                }
                            }
                        }
                        ViewBag.MenuPageName = getDocumentName();
                        DataTable dt = getSalesPersonList();
                        List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                        foreach (DataRow data in dt.Rows)
                        {
                            SalePersonList _SlPrsnDetail = new SalePersonList();
                            _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                            _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                            _SlPrsnList.Add(_SlPrsnDetail);
                        }
                        _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                        _OpeningBalanceModel1.SalePersonList = _SlPrsnList;
                        _OpeningBalanceModel.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/OpeningBalance/OpeningBalanceDetail.cshtml", _OpeningBalanceModel);
                    }
                    else
                    {
                        if (disabled == "Disabled")
                        {

                        }
                        else
                        {
                            _OpeningBalanceModel.BtnName = "Refresh";
                        }
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        DataTable dt = getSalesPersonList();
                        List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                        foreach (DataRow data in dt.Rows)
                        {
                            SalePersonList _SlPrsnDetail = new SalePersonList();
                            _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                            _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                            _SlPrsnList.Add(_SlPrsnDetail);
                        }
                        _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                        _OpeningBalanceModel.SalePersonList = _SlPrsnList;
                        _OpeningBalanceModel.SalePerson = "0";
                        _OpeningBalanceModel.Title = title;
                        ViewBag.ItemDetails = GetOpeningBalDetailList(_OpeningBalanceModel);
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/OpeningBalance/OpeningBalanceDetail.cshtml", _OpeningBalanceModel);
                    }
                }
            }
            catch (Exception ex)
            {
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
        [NonAction]
        private DataTable Getcoa(OpeningBalanceModel _OpeningBalanceModel)
        {
            try
            {
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                //var Transtype= Session["TransType"].ToString();
                var Transtype = _OpeningBalanceModel.TransType;
                int acc_type = _OpeningBalanceModel.acc_type;
                BrchID = Session["BranchId"].ToString();
                DataTable dt = _OpeningBalance_ISERVICES.Getcoa(CompID, BrchID, acc_type, Transtype);
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
        public ActionResult Getcoa1(int acc_type, string TransType)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                //var Transtype = Session["TransType"].ToString();

                DataTable dt = _OpeningBalance_ISERVICES.Getcoa(CompID, BrchID, acc_type, TransType);
                DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetAccGroup(string acc_id)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataSet result = _OpeningBalance_ISERVICES.GetAccGroup(acc_id, CompID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }
        public void GetOpeningDate(OpeningBalanceModel _OpeningBalanceModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    string CompID = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();

                    DataSet ds = _OpeningBalance_ISERVICES.GetOpeningDate(Convert.ToInt32(CompID), Convert.ToInt32(br_id));
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        FromDate = ds.Tables[1].Rows[0]["finstrdate"].ToString();
                        string FinYear = ds.Tables[1].Rows[0]["fin_y"].ToString();
                        //Session["OpFindate"] = FromDate;
                        //Session["OpFin_year"] = FinYear;
                        _OpeningBalanceModel.OpFindate = FromDate;
                        _OpeningBalanceModel.OpFin_year = FinYear;
                    }
                    List<OPYear> fyList = new List<OPYear>();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        OPYear fyObj1 = new OPYear();
                        foreach (DataRow data in ds.Tables[0].Rows)
                        {
                            OPYear fyObj = new OPYear();
                            fyObj.id = data["id"].ToString();
                            fyObj.name = data["name"].ToString();
                            fyList.Add(fyObj);
                        }
                    }
                    _OpeningBalanceModel.op_yearList = fyList;

                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        [NonAction]
        private DataTable Getcurr()
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataTable dt = _OpeningBalance_ISERVICES.GetCurrList(CompID);
                return dt;
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
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetCurrConvRate(string curr_id)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataSet result = _OpeningBalance_ISERVICES.GetCurrConvRate(curr_id, CompID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SaveOpeningBalance(OpeningBalanceModel _OpeningBalanceModel, string command)
        {
            try
            {
                var trnstyp = "";
                var Btn = "";
                var Disabled = "";
                var Cmd = "";
                var AccID = "";
                var Finyear = "";
                if (_OpeningBalanceModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                if (command == null)
                {
                    if (_OpeningBalanceModel.HdnCsvPrint == "CsvPrint")
                    {
                        command = "CsvPrint";
                    }
                }
                switch (command)
                {
                    case "AddNew":
                        OpeningBalanceModel AddNewModel = new OpeningBalanceModel();
                        AddNewModel.Command = "Add";
                        AddNewModel.TransType = "Save";
                        AddNewModel.BtnName = "BtnAddNew";
                        AddNewModel.Disabled = "Disabled";
                        AddNewModel.AppStatus = "D";
                        AddNewModel.transtyp = null;
                        TempData["ModelData"] = AddNewModel;
                        trnstyp = "Save";
                        Btn = "BtnAddNew";
                        Disabled = "Disabled";
                        Cmd = "Add";
                        return RedirectToAction("OpeningBalanceDetail", "OpeningBalance", new { trnstyp, Btn, Disabled, Cmd });

                    case "Edit":
                        _OpeningBalanceModel.Command = command;
                        _OpeningBalanceModel.AccID = _OpeningBalanceModel.acc_id.ToString();
                        _OpeningBalanceModel.Finyear = _OpeningBalanceModel.fin_year;
                        _OpeningBalanceModel.TransType = "Update";
                        _OpeningBalanceModel.BtnName = "BtnEdit";
                        TempData["ModelData"] = _OpeningBalanceModel;
                        trnstyp = "Update";
                        Btn = "BtnEdit";
                        Cmd = command;
                        AccID = _OpeningBalanceModel.AccID;
                        Finyear = _OpeningBalanceModel.Finyear;
                        return RedirectToAction("OpeningBalanceDetail", new { trnstyp, Btn, Cmd, AccID, Finyear });

                    case "Delete":
                        OpBalDetailDelete(_OpeningBalanceModel, command);
                        if (Convert.ToInt32(ViewBag.PaidAmt) != 0)
                        {
                            OpeningBalanceModel Used_Model = new OpeningBalanceModel();
                            Used_Model.Message = "CanNotEdit";
                            Used_Model.Command = "Update";
                            Used_Model.AccID = _OpeningBalanceModel.acc_id.ToString();
                            Used_Model.Finyear = _OpeningBalanceModel.Finyear;
                            Used_Model.TransType = "Update";
                            Used_Model.BtnName = "BtnToDetailPage";
                            TempData["ModelData"] = Used_Model;
                            trnstyp = "Update";
                            Btn = "BtnEdit";
                            Cmd = command;
                            AccID = Used_Model.AccID;
                            Finyear = Used_Model.Finyear;
                            return RedirectToAction("OpeningBalanceDetail", "OpeningBalance", new { trnstyp, AccID, Finyear, Btn, Cmd });
                        }
                        else
                        {
                            OpeningBalanceModel Delete_Model = new OpeningBalanceModel();
                            Delete_Model.Message = "Deleted";
                            Delete_Model.Command = "Refresh";
                            Delete_Model.Finyear = _OpeningBalanceModel.fin_year;
                            Delete_Model.TransType = command;
                            Delete_Model.BtnName = "BtnDelete";
                            TempData["ModelData"] = Delete_Model;
                            trnstyp = command;
                            Btn = "BtnDelete";
                            Cmd = "Refresh";
                            Finyear = Delete_Model.Finyear;
                            return RedirectToAction("OpeningBalanceDetail", "OpeningBalance", new { trnstyp, Btn, Cmd, Finyear });
                        }
                    case "Save":
                        if (ModelState.IsValid)
                        {
                            SaveOpeningBalanceDetail(_OpeningBalanceModel);
                            if (_OpeningBalanceModel.Message == "Save")
                            {
                                OpeningBalanceModel Save_Model = new OpeningBalanceModel();
                                Save_Model.Message = "Save";
                                Save_Model.Command = "Refresh";
                                Save_Model.TransType = "Save";
                                Save_Model.AppStatus = "D";
                                Save_Model.BtnName = "BtnToDetailPage";
                                Save_Model.Finyear = _OpeningBalanceModel.fin_year;
                                TempData["ModelData"] = Save_Model;
                                trnstyp = Save_Model.TransType;
                                Btn = Save_Model.BtnName;
                                Cmd = Save_Model.Command;
                                Finyear = Save_Model.Finyear;
                                return RedirectToAction("OpeningBalanceDetail", new { trnstyp, Btn, Cmd, Finyear });
                            }
                            else
                            {
                                trnstyp = "Save";
                                ; Btn = "Refresh";
                                Cmd = "Refresh";
                                Finyear = _OpeningBalanceModel.fin_year;
                                return RedirectToAction("OpeningBalanceDetail", new { trnstyp, Btn, Cmd, Finyear });
                            }
                        }
                        else
                        {
                            return RedirectToAction("OpeningBalanceDetail");
                        }
                    case "Refresh":
                        OpeningBalanceModel RefreshModel = new OpeningBalanceModel();
                        RefreshModel.Command = command;
                        RefreshModel.TransType = "Save";
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.Finyear = _OpeningBalanceModel.fin_year;
                        TempData["ModelData"] = RefreshModel;
                        trnstyp = RefreshModel.TransType;
                        Btn = RefreshModel.BtnName;
                        Cmd = RefreshModel.Command;
                        Finyear = RefreshModel.Finyear;
                        return RedirectToAction("OpeningBalanceDetail", new { trnstyp, Btn, Cmd, Finyear });
                    case "Print":
                        return GenratePdfFile(_OpeningBalanceModel);
                    case "CsvPrint":
                        return OpeningBalExporttoExcelDt(_OpeningBalanceModel);
                        _OpeningBalanceModel.HdnCsvPrint = null;
                    case "BacktoList":
                        return RedirectToAction("OpeningBalance", "OpeningBalance");
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
        public FileResult GenratePdfFile(OpeningBalanceModel _OpeningBalanceModel)
        {
            string Fin_year = "";
            if (_OpeningBalanceModel.hd_Finyear != null)
            {
                Fin_year = _OpeningBalanceModel.hd_Finyear;
            }
            else
            {
                Fin_year = _OpeningBalanceModel.OpBalFindt;
            }
            return File(GetPdfData(Fin_year), "application/pdf", "OpeningBalance.pdf");
        }
        public byte[] GetPdfData(string fin_year)
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
                DataSet Deatils = _OpeningBalance_ISERVICES.GetOpeningBalanceDeatils(CompID, BrchID, fin_year);
                ViewBag.PageName = "OB";
                //ViewBag.Title = "Opening Balance";
                ViewBag.DocName = "Opening Balance";
                ViewBag.Details = Deatils.Tables[0];
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, BrchID);
                ViewBag.CompLogoDtl = dtlogo;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                //ViewBag.InvoiceTo = "Invoice to:";
                //ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["qt_status"].ToString().Trim();
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/OpeningBalance/OpeningBalancePrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 20f);
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
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
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
        public ActionResult SaveOpeningBalanceDetail(OpeningBalanceModel _OpeningBalanceModel)
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

                DataTable OpeningBalanceHeader = new DataTable();
                DataTable OpeningBalanceBillWiseDetail = new DataTable();

                DataTable dt = new DataTable();
                dt.Columns.Add("MenuDocumentId", typeof(string));
                dt.Columns.Add("TransType", typeof(string));
                dt.Columns.Add("CompID", typeof(int));
                dt.Columns.Add("br_id", typeof(int));
                dt.Columns.Add("acc_type", typeof(int));
                dt.Columns.Add("acc_id", typeof(int));
                dt.Columns.Add("acc_grp_id", typeof(int));
                dt.Columns.Add("fin_year", typeof(string));
                dt.Columns.Add("op_bal_sp", typeof(string));
                dt.Columns.Add("bal_type", typeof(string));
                dt.Columns.Add("curr", typeof(int));
                dt.Columns.Add("conv_rate", typeof(string));
                dt.Columns.Add("op_bal_bs", typeof(string));
                dt.Columns.Add("create_id", typeof(string));
                dt.Columns.Add("UserMacaddress", typeof(string));
                dt.Columns.Add("UserSystemName", typeof(string));
                dt.Columns.Add("UserIP", typeof(string));
                DataRow dtrow = dt.NewRow();

                dtrow["MenuDocumentId"] = DocumentMenuId;
                //dtrow["TransType"] = Session["TransType"].ToString();
                //if (_OpeningBalanceModel.transtyp == "Save")
                //{
                //    dtrow["TransType"] = "Update";
                //}
                //else
                //{
                //    dtrow["TransType"] = "Save";
                //}
                dtrow["TransType"] = _OpeningBalanceModel.TransType;
                dtrow["CompID"] = Session["CompId"].ToString();
                dtrow["br_id"] = Session["BranchId"].ToString();
                dtrow["acc_type"] = _OpeningBalanceModel.acc_type;
                dtrow["acc_id"] = _OpeningBalanceModel.acc_id;
                dtrow["acc_grp_id"] = _OpeningBalanceModel.acc_grp_id;
                //dtrow["fin_year"] = Session["OpFin_year"].ToString();
                dtrow["fin_year"] = _OpeningBalanceModel.OpFin_year;
                dtrow["op_bal_sp"] = _OpeningBalanceModel.op_bal_sp;
                dtrow["bal_type"] = _OpeningBalanceModel.op_bal_type;
                dtrow["curr"] = _OpeningBalanceModel.curr;
                dtrow["conv_rate"] = _OpeningBalanceModel.conv_rate;
                dtrow["op_bal_bs"] = _OpeningBalanceModel.op_bal_bs;
                dtrow["create_id"] = Session["UserId"].ToString();
                dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                dtrow["UserIP"] = Session["UserIP"].ToString();

                dt.Rows.Add(dtrow);

                OpeningBalanceHeader = dt;

                DataTable dtparam = new DataTable();

                dtparam.Columns.Add("CompID", typeof(int));
                dtparam.Columns.Add("br_id", typeof(int));
                dtparam.Columns.Add("acc_id", typeof(string));
                dtparam.Columns.Add("fin_year", typeof(string));
                dtparam.Columns.Add("bill_no", typeof(string));
                dtparam.Columns.Add("bill_dt", typeof(string));
                dtparam.Columns.Add("amt_in_sp", typeof(string));
                dtparam.Columns.Add("amt_type", typeof(string));
                dtparam.Columns.Add("sls_per", typeof(int));

                JArray JObject = JArray.Parse(_OpeningBalanceModel.OPBilldetails);
                for (int i = 0; i < JObject.Count; i++)
                {
                    DataRow dtparamrow = dtparam.NewRow();

                    dtparamrow["CompID"] = Session["CompId"].ToString();
                    dtparamrow["br_id"] = Session["BranchId"].ToString();
                    dtparamrow["acc_id"] = _OpeningBalanceModel.acc_id;
                    // dtparamrow["fin_year"] = Session["OpBalFindt"];
                    dtparamrow["fin_year"] = _OpeningBalanceModel.OpFin_year;
                    dtparamrow["bill_no"] = JObject[i]["VoucherNo"].ToString();
                    dtparamrow["bill_dt"] = JObject[i]["VoucherDt"].ToString();
                    dtparamrow["amt_in_sp"] = JObject[i]["VoucherAmt"];
                    dtparamrow["amt_type"] = JObject[i]["VoucherType"].ToString();
                    dtparamrow["sls_per"] = JObject[i]["sales_person_id"].ToString();

                    dtparam.Rows.Add(dtparamrow);
                }
                OpeningBalanceBillWiseDetail = dtparam;

                String Message = _OpeningBalance_ISERVICES.InsertOpeningBalanceDetail(OpeningBalanceHeader, OpeningBalanceBillWiseDetail);
                if (Message == "Update" || Message == "Save")
                    //Session["Message"] = "Save";
                    //Session["Command"] = "New";                  
                    //Session["TransType"] = "Save";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "BtnAddNew";
                    _OpeningBalanceModel.Message = "Save";
                //    _OpeningBalanceModel.Command = "Refresh";
                //_OpeningBalanceModel.TransType = "Save";
                //    _OpeningBalanceModel.AppStatus = "D";
                //    _OpeningBalanceModel.BtnName = "BtnAddNew";                
                return RedirectToAction("OpeningBalanceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //return View("~/Views/Shared/Error.cshtml");
            }

        }
        private DataTable GetOpeningBalDetailList(OpeningBalanceModel _OpeningBalanceModel)
        {
            try
            {
                string Fin_year = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //if (Session["Fin_year"] != null)
                //{
                //    Fin_year = Session["Fin_year"].ToString();
                //}
                //else
                //{
                //    Fin_year = Session["OpBalFindt"].ToString();
                //}
                if (_OpeningBalanceModel.Finyear != null)
                {
                    Fin_year = _OpeningBalanceModel.Finyear;
                }
                else
                {
                    Fin_year = _OpeningBalanceModel.OpBalFindt;
                }
                var searchValue = _OpeningBalanceModel.searchValue;
                if (searchValue == null)
                    searchValue = "0";
                DataTable OpBalList = _OpeningBalance_ISERVICES.GetOpeningBalDetailList(CompID, BrchID, Fin_year, searchValue);

                return OpBalList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable GetOpeningBalFinYearList()
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
                DataTable OpBalFinList = _OpeningBalance_ISERVICES.GetOpeningBalFinYearList(CompID, BrchID);

                return OpBalFinList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult GetOpeningBalBillDetail(string AccId, string FinYear)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _OpeningBalance_ISERVICES.GetBillwiseOpeningDetail(CompID, BrchID, AccId, FinYear);

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
        public ActionResult GetOpeningBalBillDetail1(string AccId, string FinYear)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _OpeningBalance_ISERVICES.GetBillwiseOpeningDetail1(CompID, BrchID, AccId, FinYear);

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
        public ActionResult EditOpeningBalDt(string Acc_id, string Fin_year)
        {
            OpeningBalanceModel Edit_Model = new OpeningBalanceModel();
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["Acc_id"] = Acc_id;
            //Session["Fin_year"] = Fin_year;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            Edit_Model.Command = "Update";
            Edit_Model.AccID = Acc_id;
            Edit_Model.Finyear = Fin_year;
            Edit_Model.TransType = "Update";
            Edit_Model.AppStatus = "D";
            Edit_Model.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = Edit_Model;
            var trnstyp = "Update";
            var AccID = Acc_id;
            var Finyear = Fin_year;
            var Btn = "BtnToDetailPage";
            var Cmd = "Update";
            return RedirectToAction("OpeningBalanceDetail", "OpeningBalance", new { trnstyp, AccID, Finyear, Btn, Cmd });
        }
        public ActionResult DBClickOpeningBalDt(string FinYear)
        {
            //Session["Message"] = "New";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            //Session["TransType"] = "Add";
            //Session["Command"] = "Refresh";
            //Session["Fin_year"] = FinYear;
            //Session["Btn"] = "disabledbtn";

            OpeningBalanceModel DblClick = new OpeningBalanceModel();
            DblClick.Command = "Refresh";
            DblClick.Finyear = FinYear;
            DblClick.TransType = "Add";
            DblClick.AppStatus = "D";
            DblClick.BtnName = "BtnToDetailPage";
            DblClick.Btn = "disabledbtn";
            TempData["ModelData"] = DblClick;
            var trnstyp = DblClick.TransType;
            var Finyear = DblClick.Finyear;
            var Btn = DblClick.BtnName;
            var Cmd = DblClick.Command;
            return RedirectToAction("OpeningBalanceDetail", "OpeningBalance", new { trnstyp, Finyear, Btn, Cmd });

        }
        private ActionResult OpBalDetailDelete(OpeningBalanceModel _OpeningBalanceModel, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                //string Acc_id = Session["Acc_id"].ToString();
                //string Fin_year = Session["Fin_year"].ToString();          
                string Acc_id = _OpeningBalanceModel.acc_id.ToString();
                string Fin_year = _OpeningBalanceModel.fin_year;
                DataSet Message = _OpeningBalance_ISERVICES.OpeningBalanceDelete(Acc_id, Fin_year, CompID, br_id);

                ViewBag.PaidAmt = Message.Tables[0].Rows[0]["paid_amt"].ToString();

                //if (Convert.ToInt32(ViewBag.PaidAmt) != 0)
                //{
                //    //Session["Message"] = "CanNotEdit";
                //    //Session["Command"] = "Update";
                //    //Session["Acc_id"] = _OpeningBalanceModel.acc_id;
                //    //Session["Fin_year"] = Fin_year;
                //    //Session["TransType"] = "Update";
                //    //Session["AppStatus"] = 'D';
                //    //Session["BtnName"] = "BtnToDetailPage";
                //    _OpeningBalanceModel.Message = "CanNotEdit";
                //    _OpeningBalanceModel.Command = "Update";
                //    _OpeningBalanceModel.AccID = _OpeningBalanceModel.acc_id.ToString();
                //    _OpeningBalanceModel.Finyear = Fin_year;
                //    _OpeningBalanceModel.TransType = "Update";
                //    _OpeningBalanceModel.BtnName = "BtnToDetailPage";
                //    return RedirectToAction("OpeningBalanceDetail", "OpeningBalance");
                //}
                //else
                //{
                //    //Session["Message"] = "Deleted";
                //    //Session["Command"] = "Refresh";
                //    //Session["Acc_id"] = _OpeningBalanceModel.acc_id;
                //    //Session["TransType"] = command;
                //    //Session["AppStatus"] = 'D';
                //    //Session["BtnName"] = "BtnDelete";
                //    _OpeningBalanceModel.Message = "Deleted";
                //    _OpeningBalanceModel.Command = "Refresh";
                //   //_OpeningBalanceModel.AccID = _OpeningBalanceModel.acc_id.ToString();           
                //    _OpeningBalanceModel.TransType = command;
                //    _OpeningBalanceModel.BtnName = "BtnDelete";
                return RedirectToAction("OpeningBalanceDetail", "OpeningBalance");
                //}
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        // public ActionResult OpeningBalExporttoExcelDt(string finYear)
        public FileResult OpeningBalExporttoExcelDt(OpeningBalanceModel _OpeningBalanceModel)
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
                DataTable dt = new DataTable();
                var searchValue = _OpeningBalanceModel.searchValue;
                if (searchValue == null)
                    searchValue = "0";

                DataTable OpBalList = _OpeningBalance_ISERVICES.GetOpeningBalDetailList(CompID, BrchID, _OpeningBalanceModel.Hdn_fin_year, searchValue);

                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("Account Name", typeof(string));
                dt.Columns.Add("Account Type", typeof(string));
                dt.Columns.Add("Account Group", typeof(string));
                dt.Columns.Add("Financial Year", typeof(string));
                dt.Columns.Add("Currency", typeof(string));
                dt.Columns.Add("Conversion Rate", typeof(decimal));
                dt.Columns.Add("Opening Balance (In Specific) Debit", typeof(decimal));
                dt.Columns.Add("Opening Balance (In Specific) Credit", typeof(decimal));
                dt.Columns.Add("Opening Balance (In Base) Debit", typeof(decimal));
                dt.Columns.Add("Opening Balance (In Base) Credit", typeof(decimal));

                if (OpBalList.Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in OpBalList.Rows)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        dtrowLines["Sr.No"] = rowno + 1;
                        dtrowLines["Account Name"] = dr["acc_name"].ToString();
                        dtrowLines["Account Type"] = dr["AccType"].ToString();
                        dtrowLines["Account Group"] = dr["acc_group_name"].ToString();
                        dtrowLines["Financial Year"] = dr["fin_year"].ToString();
                        dtrowLines["Currency"] = dr["curr_name"].ToString();
                        dtrowLines["Conversion Rate"] = dr["conv_rate"].ToString();
                        dtrowLines["Opening Balance (In Specific) Debit"] = dr["sp_bal_dr"].ToString();
                        dtrowLines["Opening Balance (In Specific) Credit"] = dr["sp_bal_cr"].ToString();
                        dtrowLines["Opening Balance (In Base) Debit"] = dr["bs_bal_dr"].ToString();
                        dtrowLines["Opening Balance (In Base) Credit"] = dr["bs_bal_cr"].ToString();
                        dt.Rows.Add(dtrowLines);
                        rowno = rowno + 1;
                    }
                }
                _OpeningBalanceModel.HdnCsvPrint = null;
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("OpeningBalance", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult DownloadFile()
        {
            try
            {
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();

                DataSet dt = _OpeningBalance_ISERVICES.GetMasterDataOPBal(CompID, BrchID);
                CommonController obj = new CommonController();
                string filePath = obj.CreateExcelFile("ImportOpeningBalanceTemplate", Server);
                UpdateExcel(filePath, dt);
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
        private void UpdateExcel(string filePath, DataSet dt)
        {
            try
            {
                // Set license context for EPPlus (only once)
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // Load the Excel file
                using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    foreach (DataTable table in dt.Tables)
                    {
                        // Rename the table and columns if necessary
                        if (table.TableName == "Table")
                        {
                            table.TableName = "OpeningBalance";
                            if (table.Columns.Contains("acc_type"))
                            {
                                table.Columns.Remove("acc_type");
                            }
                            if (table.TableName == "OpeningBalance")
                            {
                                table.Columns.Add("Opening Amount*", typeof(string));
                                table.Columns.Add("Amount Type*", typeof(string));
                                table.Columns.Add("Conversion Rate*", typeof(string));
                                
                            }
                        }
                        if (table.TableName == "Table1")
                        {
                            table.TableName = "BillDetail";
                            if (table.Columns.Contains("acc_type"))
                            {
                                table.Columns.Remove("acc_type");
                            }
                            if (table.TableName == "BillDetail")
                            {
                                table.Columns.Add("Bill Number*(max 20 characters)", typeof(string));
                                table.Columns.Add("Bill Date*", typeof(string));
                                table.Columns.Add("Amount*", typeof(string));
                                table.Columns.Add("Amount Type*", typeof(string));
                                table.Columns.Add("Sales Person", typeof(string));
                            }
                        }
                        if (table.TableName == "Table2")
                        {
                            table.TableName = "SalesPerson";                           
                        }
                        // Check if the sheet exists in the Excel file
                        var worksheet = package.Workbook.Worksheets[table.TableName] ?? package.Workbook.Worksheets.Add(table.TableName);
                        // Clear existing data in the sheet
                        worksheet.Cells.Clear();
                        // Add the DataTable to the Excel worksheet
                        worksheet.Cells.LoadFromDataTable(table, true);
                        // Auto fit columns for better visibility
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                        var headerRow = worksheet.Cells[1, 1, 1, table.Columns.Count];
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Font.Name = "Calibri";
                        headerRow.Style.Font.Size = 12;
                        headerRow.Style.Font.Italic = true;
                    }
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                       
                        if (worksheet.Name == "OpeningBalance")
                        {
                            int colIndex = FindColumnIndex(worksheet, "Amount Type*");
                            if (colIndex != -1) ApplyDataValidation(worksheet, colIndex, new[] { "Debit", "Credit" });
                           
                        }
                        if (worksheet.Name == "BillDetail")
                        {
                            int colIndex = FindColumnIndex(worksheet, "Amount Type*");
                            if (colIndex != -1) ApplyDataValidation(worksheet, colIndex, new[] { "Debit", "Credit" });
                            AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Sales Person"), "SalesPerson", "SalesPersonList");
                        }
                        // Save the changes to the file
                        ProtectSheet(package.Workbook, "SalesPerson");
                        package.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                string path = "path_to_error_logs";
                Errorlog.LogError(path, ex);
            }
        }
        public void AddDropdown(ExcelPackage package, ExcelWorksheet worksheet, int colIndex, string sheetName, string rangeName)
        {
            string errorTitle = "Invalid";
            string errorMessage = "Please select a valid record from the list.";
            var dataSheet = package.Workbook.Worksheets[sheetName];

            if (dataSheet != null)
            {
                int rowCount = dataSheet.Dimension.End.Row;
                var dataList = new List<string>();
                for (int row = 2; row <= rowCount; row++)
                {
                    var value = "";
                    if (rangeName == "AttributeValueList")
                    {
                        value = dataSheet.Cells[row, 2].Text;
                    }
                    else
                    {
                        value = dataSheet.Cells[row, 1].Text;
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        dataList.Add(value);
                    }
                }
                if (dataList.Count > 0)
                {
                    if (rangeName == "AttributeValueList")
                    {
                        var dataRange = dataSheet.Cells[2, 2, dataList.Count + 1, 2];
                        package.Workbook.Names.Add(rangeName, dataRange);
                        ApplyDropdownValidation(worksheet, colIndex, rangeName);
                    }
                    else
                    {
                        var dataRange = dataSheet.Cells[2, 1, dataList.Count + 1, 1];
                        package.Workbook.Names.Add(rangeName, dataRange);
                        ApplyDropdownValidation(worksheet, colIndex, rangeName);
                    }
                }
            }
        }
        public void ApplyDropdownValidation(ExcelWorksheet worksheet, int columnIndex, string rangeName)
        {
            string errorTitle = "Invalid Selection";
            string errorMessage = "Please select a valid value from the list.";
            if (!string.IsNullOrEmpty(rangeName))
            {
                var startRow = 2;  // Adjust as per your worksheet
                var endRow = 1048576;  // Adjust to limit rows if necessary
                var range = worksheet.Cells[startRow, columnIndex, endRow, columnIndex];
                var validation = range.DataValidation.AddListDataValidation();
                validation.ShowErrorMessage = true;
                validation.ErrorTitle = errorTitle;
                validation.Error = errorMessage;
                validation.Formula.ExcelFormula = $"={rangeName}";  // Reference to the named range
            }
        }
        private void ProtectSheet(ExcelWorkbook workbook, string sheetName)
        {
            var worksheet = workbook.Worksheets[sheetName];
            if (worksheet != null)
            {
                // Protect the worksheet with a password (optional)
                worksheet.Protection.SetPassword("enrep");
            }
        }
        private int FindColumnIndex(ExcelWorksheet worksheet, string columnName)
        {
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                if (worksheet.Cells[1, col].Text == columnName)
                {
                    return col;
                }
            }
            return -1;
        }
        private void ApplyDataValidation(ExcelWorksheet worksheet, int columnIndex, string[] listValues, string errorTitle = "Invalid Selection", string errorMessage = "Please select a valid value from the list.")
        {
            if (listValues != null && listValues.Length > 0)
            {
                var startRow = 2;
                var endRow = 1048576;
                var range = worksheet.Cells[startRow, columnIndex, endRow, columnIndex];
                var validation = range.DataValidation.AddListDataValidation();
                validation.ShowErrorMessage = true;
                validation.ErrorTitle = errorTitle;
                validation.Error = errorMessage;
                validation.Formula.ExcelFormula = $"\"{string.Join(",", listValues)}\"";
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
                    DataTable OPBalDetail = new DataTable();
                    DataTable BillDetail = new DataTable();
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

                                string OPBalDetailQuery = "SELECT DISTINCT * From [OpeningBalance$] where LEN([Opening Amount*]) > 0 or LEN([Amount Type*]) > 0 or LEN([Conversion Rate*]) > 0 ;";
                                string BillQuery = "SELECT DISTINCT * From [BillDetail$] " + "WHERE LEN([Bill Number*(max 20 characters)]) > 0 " + "OR LEN([Bill Date*]) > 0 " + "OR LEN([Amount*]) > 0 " + "OR LEN([Amount Type*]) > 0 "+ "OR LEN([Sales Person]) > 0 ;";

                                connExcel.Open();
                                cmdExcel.CommandText = OPBalDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(OPBalDetail);

                                cmdExcel.CommandText = BillQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(BillDetail);

                                ds.Tables.Add(OPBalDetail);
                                ds.Tables.Add(BillDetail);

                                DataSet dts = VerifyData(ds, uploadStatus);
                                if (dts == null)
                                    return Json("Excel file is empty. Please fill data in excel file and try again");
                                ViewBag.ImportPreview = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialImportOPBalDetail.cshtml");
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
            string compId = "", BrchID = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            DataSet dts = PrepareDataset(ds);
            if (ds.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[0].Rows[0].ToString()))
            {
                DataSet result = _OpeningBalance_ISERVICES.GetVerifiedDataOfExcel(dts.Tables[0], dts.Tables[1], compId, BrchID);
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
        public DataSet PrepareDataset(DataSet ds)
        {
            DataTable OPDetail = new DataTable();
            DataTable BillDetail = new DataTable();

            OPDetail.Columns.Add("acc_type", typeof(string));
            OPDetail.Columns.Add("acc_name", typeof(string));
            OPDetail.Columns.Add("curr", typeof(string));
            OPDetail.Columns.Add("op_bal_sp", typeof(string));
            OPDetail.Columns.Add("bal_type", typeof(string));
            OPDetail.Columns.Add("conv_rate", typeof(string));

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataTable dtopbaldetail = ds.Tables[0];
                DataRow dtr = OPDetail.NewRow();

                dtr["acc_type"] = dtopbaldetail.Rows[i][0].ToString().Trim();
                dtr["acc_name"] = dtopbaldetail.Rows[i][1].ToString().Trim();
                dtr["curr"] = dtopbaldetail.Rows[i][2].ToString().Trim();
                dtr["bal_type"] = dtopbaldetail.Rows[i][4].ToString().Trim();

                if (dtopbaldetail.Rows[i][5].ToString() == "" || string.IsNullOrEmpty(dtopbaldetail.Rows[i][5].ToString()))
                {
                    dtr["conv_rate"] = "0.0000";
                }
                else
                {
                    dtr["conv_rate"] = Convert.ToDecimal(dtopbaldetail.Rows[i][5]).ToString("F4");
                }
                string opbalString = dtopbaldetail.Rows[i][3].ToString().Trim();
                decimal qtyDecimal = 0.0m;
                if (string.IsNullOrEmpty(opbalString))
                {
                    if (dtopbaldetail.Rows[i][3] == DBNull.Value || string.IsNullOrEmpty(opbalString))
                    {
                        qtyDecimal = 0.0m;
                    }
                    else
                    {
                        if (decimal.TryParse(opbalString, out qtyDecimal))
                        {
                        }
                        else
                        {
                            qtyDecimal = 0.0m;
                        }
                    }
                    dtr["op_bal_sp"] = qtyDecimal.ToString("0000.00");
                }
                else
                {
                    double opbal = 0; /* Changed by Suraj Maurya on 29-05-2025 from float to double */
                    double.TryParse(dtopbaldetail.Rows[i][3].ToString().Trim(), out opbal);
                    dtr["op_bal_sp"] = opbal.ToString("F2");
                }
                OPDetail.Rows.Add(dtr);
            }
            BillDetail.Columns.Add("acc_type", typeof(string));
            BillDetail.Columns.Add("acc_name", typeof(string));
            BillDetail.Columns.Add("bill_no", typeof(string));
            BillDetail.Columns.Add("bill_dt", typeof(string));
            BillDetail.Columns.Add("amt_in_sp", typeof(string));
            BillDetail.Columns.Add("amt_type", typeof(string));
            BillDetail.Columns.Add("sales_per", typeof(string));

            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                DataTable dsbill = ds.Tables[1];
                DataRow dtr = BillDetail.NewRow();
                dtr["acc_type"] = dsbill.Rows[i][0].ToString().Trim();
                dtr["acc_name"] = dsbill.Rows[i][1].ToString().Trim();
                dtr["bill_no"] = dsbill.Rows[i][2].ToString().Trim();
                // dtr["bill_dt"] = dsbill.Rows[i][3].ToString().Trim();
                string inputDate = dsbill.Rows[i][3].ToString().Trim();
                string format1 = @"^\d{2}-\d{2}-\d{4}$";
                string format2 = @"^\d{4}-\d{2}-\d{2}$";
                if (!Regex.IsMatch(inputDate, format1) && !Regex.IsMatch(inputDate, format2))
                {
                    dtr["bill_dt"] = dsbill.Rows[i][3].ToString().Trim();
                }
                else
                {
                    DateTime parsedDate;
                    if (DateTime.TryParseExact(inputDate, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                    {
                        dtr["bill_dt"] = parsedDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        dtr["bill_dt"] = dsbill.Rows[i][3].ToString().Trim();
                    }
                }
                dtr["amt_type"] = dsbill.Rows[i][5].ToString().Trim();
                dtr["sales_per"] = dsbill.Rows[i][6].ToString().Trim();
                string amount = dsbill.Rows[i][4].ToString().Trim();
                decimal qtyDecimal = 0.0m;
                if (string.IsNullOrEmpty(amount) || amount == "0")
                {
                    if (dsbill.Rows[i][4] == DBNull.Value || string.IsNullOrEmpty(amount))
                    {
                        qtyDecimal = 0.0m;
                    }
                    else
                    {
                        if (decimal.TryParse(amount, out qtyDecimal))
                        {
                        }
                        else
                        {
                            qtyDecimal = 0.0m;
                        }
                    }
                    dtr["amt_in_sp"] = qtyDecimal.ToString("0000.00");
                }
                else
                {
                    double opbal = 0; /* Changed by Suraj Maurya on 29-05-2025 from float to double */
                    double.TryParse(dsbill.Rows[i][4].ToString().Trim(), out opbal);
                    dtr["amt_in_sp"] = opbal.ToString("F2");
                }
                BillDetail.Rows.Add(dtr);
            }
            DataSet dts = new DataSet();
            dts.Tables.Add(OPDetail);
            dts.Tables.Add(BillDetail);
            return dts;
        }
        public ActionResult ShowValidationError(string accounttype, string accountname)
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
                    DataTable OPBalDetail = new DataTable();
                    DataTable BillDetail = new DataTable();
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
                                string OPBalDetailQuery = "SELECT DISTINCT * FROM [OpeningBalance$] WHERE [Account Name*(max 50 characters)] = '" + accountname + "'";
                                string BillDetailQuery = "SELECT DISTINCT * From [BillDetail$] where [Account Name*(max 50 characters)] = '" + accountname + "'and [Account Type*] = '" + accounttype + "' ";

                                connExcel.Open();
                                cmdExcel.CommandText = OPBalDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(OPBalDetail);

                                cmdExcel.CommandText = BillDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(BillDetail);

                                ds.Tables.Add(OPBalDetail);
                                ds.Tables.Add(BillDetail);
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
            string compId = "", BrchID = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            DataSet dts = PrepareDataset(ds);
            DataTable result = _OpeningBalance_ISERVICES.ShowExcelErrorDetail(dts.Tables[0], dts.Tables[1], compId, BrchID);
            return result;
        }
        public ActionResult BindExcelBill(string accountname, string accounttype)
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
                    DataTable dtbilldetail = new DataTable();
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
                                string BillDetailQuery = "SELECT DISTINCT * From [BillDetail$] where [Account Name*(max 50 characters)] = '" + accountname + "'and [Account Type*] = '" + accounttype + "'";

                                cmdExcel.CommandText = BillDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtbilldetail);
                                DataTable dtbill = new DataTable();
                                double credit_amt = 0; /* Changed by Suraj Maurya on 29-05-2025 from float to double */
                                double debit_amt = 0;
                                string balancetype = "";
                                dtbill.Columns.Add("acc_type", typeof(string));
                                dtbill.Columns.Add("acc_name", typeof(string));
                                dtbill.Columns.Add("bill_no", typeof(string));
                                dtbill.Columns.Add("bill_dt", typeof(string));
                                dtbill.Columns.Add("amt_in_sp", typeof(string));
                                dtbill.Columns.Add("amt_type", typeof(string));
                                dtbill.Columns.Add("sales_per", typeof(string));
                                for (int i = 0; i < dtbilldetail.Rows.Count; i++)
                                {
                                    DataRow dtr = dtbill.NewRow();
                                    dtr["acc_type"] = dtbilldetail.Rows[i][0].ToString();
                                    dtr["acc_name"] = dtbilldetail.Rows[i][1].ToString();
                                    dtr["bill_no"] = dtbilldetail.Rows[i][2].ToString();
                                    dtr["bill_dt"] = dtbilldetail.Rows[i][3].ToString();
                                    string inputDate = dtbilldetail.Rows[i][3].ToString().Trim();
                                    if (!string.IsNullOrEmpty(inputDate))
                                    {
                                        DateTime parsedDate;
                                        if (DateTime.TryParse(inputDate, out parsedDate))
                                        {
                                            string formattedDate = parsedDate.ToString("dd-MM-yyyy");
                                            dtr["bill_dt"] = formattedDate;
                                        }
                                    }
                                        dtr["amt_in_sp"] = dtbilldetail.Rows[i][4].ToString();
                                    dtr["amt_type"] = dtbilldetail.Rows[i][5].ToString();
                                    dtr["sales_per"] = dtbilldetail.Rows[i][6].ToString();
                                    if (dtbilldetail.Rows[i][5].ToString() == "Credit")
                                    {
                                        double credit = 0; /* Changed by Suraj Maurya on 29-05-2025 from float to double */
                                        double.TryParse(dtbilldetail.Rows[i][4].ToString(), out credit);
                                        credit_amt += credit;
                                    }
                                    if (dtbilldetail.Rows[i][5].ToString() == "Debit")
                                    {
                                        double debit = 0; /* Changed by Suraj Maurya on 29-05-2025 from float to double */
                                        double.TryParse(dtbilldetail.Rows[i][4].ToString(), out debit);
                                        debit_amt += debit;
                                    }
                                    if (credit_amt > debit_amt)
                                    {
                                        balancetype = "Credit";
                                    }
                                    else
                                    {
                                        balancetype = "Debit";
                                    }
                                    dtbill.Rows.Add(dtr);
                                }
                                double balance = Math.Abs(credit_amt - debit_amt); /* Changed by Suraj Maurya on 29-05-2025 from float to double */
                                string formattedcreditamt = credit_amt.ToString("F2");
                                string formatteddebitamt = debit_amt.ToString("F2");
                                string formattedbalance = balance.ToString("F2");

                                ViewBag.BillDetail = dtbill;
                                ViewBag.CreditAmount = formattedcreditamt;
                                ViewBag.DebitAmount = formatteddebitamt;
                                ViewBag.BalanceAmount = formattedbalance;
                                ViewBag.BalanceType = balancetype;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOpeningBalanceBillDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public JsonResult ImportOpBalFromExcel()
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
                        default:
                            conString = "Invalid File";
                            break;
                    }
                    if (conString == "Invalid File")
                        return Json("Invalid File. Please upload a valid file", JsonRequestBehavior.AllowGet);
                    DataSet ds = new DataSet();
                    DataTable OPBalDetail = new DataTable();
                    DataTable BillDetail = new DataTable();
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

                                string OPBalDetailQuery = "SELECT DISTINCT * From [OpeningBalance$] where LEN([Opening Amount*]) > 0 or LEN([Amount Type*]) > 0 or LEN([Conversion Rate*]) > 0 ;";
                                string BillQuery = "SELECT DISTINCT * From [BillDetail$] " + "WHERE LEN([Bill Number*(max 20 characters)]) > 0 " + "OR LEN([Bill Date*]) > 0 " + "OR LEN([Amount*]) > 0 " + "OR LEN([Amount Type*]) > 0 ;";

                                connExcel.Open();
                                cmdExcel.CommandText = OPBalDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(OPBalDetail);

                                cmdExcel.CommandText = BillQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(BillDetail);

                                ds.Tables.Add(OPBalDetail);
                                ds.Tables.Add(BillDetail);
                                string msg = SaveOPBalFromExcel(ds);
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
        private string SaveOPBalFromExcel(DataSet dsOPBal)
        {
            string compId = "";
            string UserID = "";
            string BrchID = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            DataSet dts = PrepareDataset(dsOPBal);
            string result = _OpeningBalance_ISERVICES.BulkImportOPBalDetail(dts.Tables[0], dts.Tables[1], BrchID, UserID, compId);
            return result;
        }
        public DataTable getSalesPersonList()
        {
            if (Session["compid"] != null)
                CompID = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            DataTable dt = _OpeningBalance_ISERVICES.getSalesPersonList(CompID, BrchID, UserID);
            return dt;
        }
    }
}