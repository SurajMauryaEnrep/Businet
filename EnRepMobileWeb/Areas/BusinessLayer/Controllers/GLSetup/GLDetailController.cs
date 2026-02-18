using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.MODELS.BusinessLayer.GLDetail;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
//*** All Session Removed by shubham maurya on 21-12-2022 16:05***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class GLDetailController : Controller
    {
        string comp_id, Br_ID, userid, GLId = string.Empty;
        string CompID, language = String.Empty;
        string DocumentMenuId = "103170",title;
        Common_IServices _Common_IServices;
        GLDetailModel _GLDetail;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        GLDetail_ISERVICES _GLDetail_ISERVICES;
        // GET: BusinessLayer/GLDetail
        public GLDetailController(Common_IServices _Common_IServices,GLDetail_ISERVICES _GLDetail_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._GLDetail_ISERVICES = _GLDetail_ISERVICES;
        }
        public ActionResult GLDetail(/*GLDetailModel _GLDetail1*/string GL_Code,string TransType, string Command,string BtnName)
        {
            try
            {
                var _GLDetail=TempData["ModelData"] as GLDetailModel;
                 string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                if (_GLDetail != null)
                {
                    ViewBag.MenuPageName = getDocumentName();
                    //_GLDetail = new GLDetailModel();
                    _GLDetail.Title = title;
                    string Comp_ID = string.Empty;
                    //_GLDetail.acc_id = 0;
                    dt = GetGLBranchList();
                    //ViewBag.CustomerBranchList = dt;
                    _GLDetail.CustomerBranchList = dt;
                  DataTable  dt1 = GetAccountGroup(_GLDetail);
                    List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                    foreach (DataRow Row1 in dt1.Rows)
                    {
                        AccountGroup _AccountGroup = new AccountGroup();
                        _AccountGroup.acc_grp_id = Row1["acc_grp_id"].ToString();
                        _AccountGroup.AccGroupChildNood = Row1["AccGroupChildNood"].ToString();
                        _AccountGroupList.Add(_AccountGroup);
                    }
                    _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                    _GLDetail.AccountGroupList = _AccountGroupList;
                    //List<curr> _CustcurrList = new List<curr>();
                    //dt = Getcurr();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    curr _Custcurr = new curr();
                    //    _Custcurr.curr_id = dr["curr_id"].ToString();
                    //    _Custcurr.curr_name = dr["curr_name"].ToString();
                    //    _CustcurrList.Add(_Custcurr);

                    //}
                    //_CustcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                    _GLDetail.currList = Currlist();
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _GLDetail.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                   
                    //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                    if (_GLDetail.TransType == "Update" || _GLDetail.Command == "Edit")
                    {
                        //string GLCode = Session["GLCode"].ToString();
                        string GLCode = _GLDetail.GLCode;
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = Session["CompId"].ToString();
                        }
                        Boolean act_status_tr;
                        Boolean od_allow;
                        DataSet ds = _GLDetail_ISERVICES.GetviewGLdetailDAL(GLCode, Comp_ID);
                        if (ds.Tables[0].Rows[0]["act_status_tr"].ToString() == "Y")
                            act_status_tr = true;
                        else
                            act_status_tr = false;
                        if (ds.Tables[0].Rows[0]["od_allow"].ToString() == "Y")
                        {
                            od_allow = true;
                        }
                        else
                        {
                            od_allow = false;
                        }
                        ViewBag.BranchList = ds.Tables[2];
                        _GLDetail.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
                        _GLDetail.acc_name = ds.Tables[0].Rows[0]["acc_name"].ToString();
                        _GLDetail.inact_reason = ds.Tables[0].Rows[0]["inact_reason"].ToString();
                        _GLDetail.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        _GLDetail.acc_type = int.Parse(ds.Tables[0].Rows[0]["acc_type"].ToString());
                        _GLDetail.cf_type = ds.Tables[0].Rows[0]["cf_type"].ToString();                        
                        _GLDetail.act_status_tr = act_status_tr;
                        _GLDetail.roa = ToggleResultToBoolean(ds.Tables[0].Rows[0]["roa"].ToString());
                        _GLDetail.plr = ToggleResultToBoolean(ds.Tables[0].Rows[0]["plr"].ToString());
                        _GLDetail.ibt = ToggleResultToBoolean(ds.Tables[0].Rows[0]["ibt"].ToString());
                        _GLDetail.iwt = ToggleResultToBoolean(ds.Tables[0].Rows[0]["iwt"].ToString());
                        _GLDetail.egl = ToggleResultToBoolean(ds.Tables[0].Rows[0]["egl"].ToString());
                        _GLDetail.sta = ToggleResultToBoolean(ds.Tables[0].Rows[0]["sta"].ToString());
                        _GLDetail.tr = ToggleResultToBoolean(ds.Tables[0].Rows[0]["tr"].ToString());
                        _GLDetail.tp = ToggleResultToBoolean(ds.Tables[0].Rows[0]["tp"].ToString());                       
                        _GLDetail.bra = ToggleResultToBoolean(ds.Tables[0].Rows[0]["bra"].ToString());                       
                        _GLDetail.createby = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _GLDetail.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _GLDetail.modby = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _GLDetail.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _GLDetail.GLReportingGroup = ds.Tables[0].Rows[0]["gl_rpt_des"].ToString();
                        _GLDetail.GLReportingGroup_ID = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                        //ViewBag.CustomerBranchList = ds.Tables[1];
                        _GLDetail.CustomerBranchList = ds.Tables[1];
                        _GLDetail.curr = int.Parse(ds.Tables[0].Rows[0]["curr_id"].ToString());                     
                        _GLDetail.TAp = od_allow;
                        _GLDetail.D_InActive = ds.Tables[3].Rows[0]["disableInActive"].ToString();
                        DataTable  dt2 = GetAccountGroup(_GLDetail);
                    List<AccountGroup> _AccountGroupLists = new List<AccountGroup>();
                    foreach (DataRow Row12 in dt2.Rows)
                    {
                        AccountGroup _AccountGroup = new AccountGroup();
                        _AccountGroup.acc_grp_id = Row12["acc_grp_id"].ToString();
                        _AccountGroup.AccGroupChildNood = Row12["AccGroupChildNood"].ToString();
                            _AccountGroupLists.Add(_AccountGroup);
                    }
                        _AccountGroupLists.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                        _GLDetail.AccountGroupList = _AccountGroupLists;

                        if (ds.Tables[4].Rows[0]["disenbcurr"].ToString() != null && ds.Tables[4].Rows[0]["disenbcurr"].ToString() != "")
                        {
                            _GLDetail.curr_depncy = ds.Tables[4].Rows[0]["disenbcurr"].ToString();
                        }
                        else
                        {
                            _GLDetail.curr_depncy = null;
                        }

                        //_GLDetail.Overdraft_Limit = ds.Tables[0].Rows[0]["od_limit"].ToString();
                        if ((ds.Tables[0].Rows[0]["od_limit"] != null) && (ds.Tables[0].Rows[0]["od_limit"].ToString() != "") && (od_allow != false))
                        {
                            _GLDetail.Overdraft_Limit = Convert.ToDecimal(ds.Tables[0].Rows[0]["od_limit"]).ToString(RateDigit);
                        }
                        else
                        {
                            _GLDetail.Overdraft_Limit = ds.Tables[0].Rows[0]["od_limit"].ToString();
                        }
                        ViewBag.Acctypedt = int.Parse(ds.Tables[0].Rows[0]["acc_type"].ToString());
                        _GLDetail.bank_add = ds.Tables[0].Rows[0]["bank_addr"].ToString();
                        _GLDetail.acc_no = ds.Tables[0].Rows[0]["acc_no"].ToString();
                        _GLDetail.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();
                        _GLDetail.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                    }
                    else
                    {
                        _GLDetail.act_status_tr = true;
                    }
                    //_GLDetail.TransType = Session["TransType"].ToString();
                    _GLDetail.TransType = _GLDetail.TransType;
                    ViewBag.DocumentId = DocumentMenuId;
                    CommonPageDetails();
                    return View("~/Areas/BusinessLayer/Views/GLSetup/GLDetail.cshtml", _GLDetail);
                }
                else
                {
                    GLDetailModel _GLDetail1 = new GLDetailModel();
                    ViewBag.MenuPageName = getDocumentName();
                    //GLDetailModel _GLDetail1 = new GLDetailModel();
                    _GLDetail1.Title = title;
                    string Comp_ID = string.Empty;
                    //_GLDetail1.acc_id = 0;
                    dt = GetGLBranchList();
                    //ViewBag.CustomerBranchList = dt;
                    _GLDetail1.CustomerBranchList = dt;
                 DataTable   dt1 = GetAccountGroup(_GLDetail1);
                    List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                    foreach (DataRow Row1 in dt1.Rows)
                    {
                        AccountGroup _AccountGroup = new AccountGroup();
                        _AccountGroup.acc_grp_id = Row1["acc_grp_id"].ToString();
                        _AccountGroup.AccGroupChildNood = Row1["AccGroupChildNood"].ToString();
                        _AccountGroupList.Add(_AccountGroup);
                    }
                    _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                    _GLDetail1.AccountGroupList = _AccountGroupList;
                    //List<curr> _CustcurrList = new List<curr>();
                    //dt = Getcurr();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    curr _Custcurr = new curr();
                    //    _Custcurr.curr_id = dr["curr_id"].ToString();
                    //    _Custcurr.curr_name = dr["curr_name"].ToString();
                    //    _CustcurrList.Add(_Custcurr);

                    //}
                    //_CustcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                    _GLDetail1.currList = Currlist();
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    _GLDetail1.TransType = TransType;
                    _GLDetail1.Command = Command;
                    _GLDetail1.BtnName = BtnName;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString()!="")
                    {
                        _GLDetail1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                    if (_GLDetail1.TransType == "Update" || _GLDetail1.Command == "Edit")
                    {
                        //string GLCode = Session["GLCode"].ToString();
                        string GLCode = GL_Code;// _GLDetail1.GLCode;
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = Session["CompId"].ToString();
                        }
                        Boolean act_status_tr;
                        Boolean od_allow;
                        DataSet ds = _GLDetail_ISERVICES.GetviewGLdetailDAL(GLCode, Comp_ID);
                        if (ds.Tables[0].Rows[0]["act_status_tr"].ToString() == "Y")
                            act_status_tr = true;
                        else
                            act_status_tr = false;
                        if (ds.Tables[0].Rows[0]["od_allow"].ToString() == "Y")
                        {
                            od_allow = true;
                        }
                        else
                        {
                            od_allow = false;
                        }
                        ViewBag.BranchList = ds.Tables[2];
                        _GLDetail1.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
                        _GLDetail1.GLCode = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _GLDetail1.acc_name = ds.Tables[0].Rows[0]["acc_name"].ToString();
                        _GLDetail1.inact_reason = ds.Tables[0].Rows[0]["inact_reason"].ToString();
                        _GLDetail1.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        _GLDetail1.acc_type = int.Parse(ds.Tables[0].Rows[0]["acc_type"].ToString());
                        _GLDetail1.cf_type = ds.Tables[0].Rows[0]["cf_type"].ToString();                    
                        _GLDetail1.act_status_tr = act_status_tr;
                        _GLDetail1.roa = ToggleResultToBoolean(ds.Tables[0].Rows[0]["roa"].ToString());
                        _GLDetail1.plr = ToggleResultToBoolean(ds.Tables[0].Rows[0]["plr"].ToString());
                        _GLDetail1.ibt = ToggleResultToBoolean(ds.Tables[0].Rows[0]["ibt"].ToString());
                        _GLDetail1.iwt = ToggleResultToBoolean(ds.Tables[0].Rows[0]["iwt"].ToString());
                        _GLDetail1.egl = ToggleResultToBoolean(ds.Tables[0].Rows[0]["egl"].ToString());
                        _GLDetail1.sta = ToggleResultToBoolean(ds.Tables[0].Rows[0]["sta"].ToString());
                        _GLDetail1.tr = ToggleResultToBoolean(ds.Tables[0].Rows[0]["tr"].ToString());
                        _GLDetail1.tp = ToggleResultToBoolean(ds.Tables[0].Rows[0]["tp"].ToString());
                        _GLDetail1.bra = ToggleResultToBoolean(ds.Tables[0].Rows[0]["bra"].ToString());
                        _GLDetail1.createby = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _GLDetail1.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _GLDetail1.modby = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _GLDetail1.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _GLDetail1.GLReportingGroup = ds.Tables[0].Rows[0]["gl_rpt_des"].ToString();
                        _GLDetail1.GLReportingGroup_ID = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                        //ViewBag.CustomerBranchList = ds.Tables[1];
                        _GLDetail1.CustomerBranchList = ds.Tables[1];
                        _GLDetail1.curr = int.Parse(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _GLDetail1.TAp = od_allow;
                        _GLDetail1.D_InActive = ds.Tables[3].Rows[0]["disableInActive"].ToString();
                       DataTable dt2 = GetAccountGroup(_GLDetail1);
                        List<AccountGroup> _AccountGroupLists = new List<AccountGroup>();
                        foreach (DataRow Row12 in dt2.Rows)
                        {
                            AccountGroup _AccountGroup = new AccountGroup();
                            _AccountGroup.acc_grp_id = Row12["acc_grp_id"].ToString();
                            _AccountGroup.AccGroupChildNood = Row12["AccGroupChildNood"].ToString();
                            _AccountGroupLists.Add(_AccountGroup);
                        }
                        _AccountGroupLists.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                        _GLDetail1.AccountGroupList = _AccountGroupLists;

                        if (ds.Tables[4].Rows[0]["disenbcurr"].ToString() != null && ds.Tables[4].Rows[0]["disenbcurr"].ToString() != "")
                        {
                            _GLDetail1.curr_depncy = ds.Tables[4].Rows[0]["disenbcurr"].ToString();
                        }
                        else
                        {
                            _GLDetail1.curr_depncy = null;
                        }

                        //_GLDetail1.TAp = ToggleResultToBoolean(ds.Tables[0].Rows[0]["od_allow"].ToString());
                        //_GLDetail1.Overdraft_Limit = ds.Tables[0].Rows[0]["od_limit"].ToString();
                        if (ds.Tables[0].Rows[0]["od_limit"] != null && ds.Tables[0].Rows[0]["od_limit"].ToString() != "" && od_allow!=false)
                        {
                            _GLDetail1.Overdraft_Limit = Convert.ToDecimal(ds.Tables[0].Rows[0]["od_limit"]).ToString(RateDigit);
                        }
                        else
                        {
                            _GLDetail1.Overdraft_Limit = ds.Tables[0].Rows[0]["od_limit"].ToString();
                        }
                        if (_GLDetail1.BtnName==null)
                        {
                            _GLDetail1.BtnName = "BtnEdit";
                        }
                        ViewBag.Acctypedt = int.Parse(ds.Tables[0].Rows[0]["acc_type"].ToString());
                        //ViewBag.TPAToggel = ToggleResultToBoolean(ds.Tables[0].Rows[0]["od_allow"].ToString());
                        _GLDetail1.bank_add = ds.Tables[0].Rows[0]["bank_addr"].ToString();
                        _GLDetail1.acc_no = ds.Tables[0].Rows[0]["acc_no"].ToString();
                        _GLDetail1.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();
                        _GLDetail1.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                    }
                    else
                    {
                        _GLDetail1.act_status_tr = true;
                    }
                    //_GLDetail1.TransType = Session["TransType"].ToString();
                    _GLDetail1.TransType = _GLDetail1.TransType;
                    if (_GLDetail1.BtnName == null)
                    {
                        _GLDetail1.BtnName = "BtnRefresh";
                    }
                    CommonPageDetails();
                    ViewBag.DocumentId = DocumentMenuId;
                    return View("~/Areas/BusinessLayer/Views/GLSetup/GLDetail.cshtml", _GLDetail1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public string ToFixDecimal(int number)
        {
            try
            {
                string str = "0.";
                for (int i = 0; i < number; i++)
                {
                    str += "0";
                }
                return str;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, userid, DocumentMenuId, language);
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
        private List<curr> Currlist()
        {
            try
            {
                List<curr> _CustcurrList = new List<curr>();
                dt = Getcurr();
                foreach (DataRow dr in dt.Rows)
                {
                    curr _Custcurr = new curr();
                    _Custcurr.curr_id = dr["curr_id"].ToString();
                    _Custcurr.curr_name = dr["curr_name"].ToString();
                    _CustcurrList.Add(_Custcurr);
                }
                return _CustcurrList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null; ;
            }

        }
        public ActionResult GLSave(GLDetailModel _GLDetail, string command, int acc_id)
        {
            try
            {
                //var Objects=new { };
                if (_GLDetail.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "Edit":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["GLCode"] = _GLDetail.acc_id;
                        //Session["TransType"] = "Update";
                        //Session["BtnName"] = "BtnEdit";

                        //_GLDetail.Message = "";
                        //_GLDetail.Command = command;
                        //_GLDetail.GLCode = _GLDetail.acc_id.ToString();
                        //_GLDetail.GLCode = _GLDetail.GLCode;
                        //_GLDetail.TransType = "Update";
                        //_GLDetail.BtnName = "BtnEdit";
                        //TempData["ModelData"] = _GLDetail;
                        _GLDetail.hdnSavebtn = null;
                        TempData["ListFilterData"] = _GLDetail.ListFilterData1;
                        return RedirectToAction("GLDetail", "GLDetail", new { GL_Code = _GLDetail.GLCode, Command = command, TransType = "Update", BtnName = "BtnEdit" });
                    case "Add":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["GLCode"] = "";
                        //Session["AppStatus"] = "D";
                        ////_GLDetail = null;
                        //Session["TransType"] = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        //_GLDetail.Message = "";
                        //_GLDetail.Command = command;
                        //_GLDetail.GLCode = "";
                        //_GLDetail.AppStatus = "D";
                        //_GLDetail = null;
                        //_GLDetail.TransType = "Save";
                        //_GLDetail.BtnName = "BtnAddNew";
                        //TempData["ModelData"] = _GLDetail;
                        _GLDetail.hdnSavebtn = null;
                        TempData["ListFilterData"] = null;
                       var Objects = new { GL_Code = "", Command = command, TransType = "Save", BtnName = "BtnAddNew", Message = _GLDetail.Message };
                        return RedirectToAction("GLDetail", "GLDetail", Objects);
                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Delete";
                        _GLDetail.Command = command;
                        _GLDetail.BtnName = "Delete";
                        _GLDetail.DeleteCommand = null;
                        _GLDetail.hdnSavebtn = null;
                        //acc_id = _GLDetail.acc_id;
                        GLDetailDelete(_GLDetail, command);
                        //TempData["ModelData"] = _GLDetail;
                        TempData["ListFilterData"] = _GLDetail.ListFilterData1;
                        var GL_Code = "";
                        if (_GLDetail.Message == "Used")
                        {
                            GL_Code = _GLDetail.GLCode;
                        }
                        Objects = new { GL_Code = GL_Code, Command = command, TransType = _GLDetail.TransType, BtnName = "Delete", Message=_GLDetail.Message };
                        return RedirectToAction("GLDetail", Objects);
                    case "Save":
                        //Session["Command"] = command;
                        _GLDetail.Command = command;
                        if (ModelState.IsValid)
                        {
                            InsertGLDetail(_GLDetail);
                            //Session["GLCode"] = Session["GLCode"].ToString();
                            _GLDetail.GLCode = _GLDetail.GLCode;
                            if (_GLDetail.Message == "DataNotFound")
                            {
                                _GLDetail.hdnSavebtn = null;
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            //if (Session["Message"].ToString() == "Duplicate")
                            if (_GLDetail.Message == "Duplicate")
                            {
                                _GLDetail.hdnSavebtn = null;
                                dt = GetGLBranchList();
                                //ViewBag.CustomerBranchList = dt;
                                _GLDetail.CustomerBranchList = dt;
                                if (_GLDetail.GLCode != "0")
                                {
                                    string Comp_ID = string.Empty;
                                    string GLCode = _GLDetail.GLCode;
                                    if (Session["CompId"] != null)
                                    {
                                        Comp_ID = Session["CompId"].ToString();
                                    }
                                    DataSet ds = _GLDetail_ISERVICES.GetviewGLdetailDAL(GLCode, Comp_ID);
                                    _GLDetail.CustomerBranchList = ds.Tables[1];
                                    _GLDetail.createby = ds.Tables[0].Rows[0]["create_id"].ToString();
                                    _GLDetail.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                    _GLDetail.modby = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                    _GLDetail.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                }
                                ViewBag.Acctypedt = _GLDetail.acc_type;
                                dt = GetAccountGroup(_GLDetail);
                                List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AccountGroup _AccountGroup = new AccountGroup();
                                    _AccountGroup.acc_grp_id = dt["acc_grp_id"].ToString();
                                    _AccountGroup.AccGroupChildNood = dt["AccGroupChildNood"].ToString();
                                    _AccountGroupList.Add(_AccountGroup);
                                }
                                _GLDetail.AccountGroupList = _AccountGroupList;
                                _GLDetail.currList = Currlist();
                               
                                //Session["GLCode"] = "";
                                //Session["AppStatus"] = "D";
                                //Session["TransType"] = "Save";
                                //Session["BtnName"] = "BtnAddNew";
                                //ViewBag.Message = Session["Message"].ToString();

                                _GLDetail.GLCode = "";
                                _GLDetail.AppStatus = "D";
                                _GLDetail.TransType = "Save";
                                _GLDetail.BtnName = "BtnAddNew";
                                _GLDetail.Message = _GLDetail.Message;
                                if (_GLDetail.act_status_tr == false)
                                {
                                    _GLDetail.act_status_tr = true;
                                }
                                TempData["ListFilterData"] = _GLDetail.ListFilterData1;
                                return View("~/Areas/BusinessLayer/Views/GLSetup/GLDetail.cshtml", _GLDetail);
                            }
                            else
                            {
                                TempData["ListFilterData"] = _GLDetail.ListFilterData1;
                                //TempData["ModelData"] = _GLDetail;
                                Objects = new { GL_Code = _GLDetail.GLCode, Command = command, TransType = _GLDetail.TransType, BtnName = _GLDetail.BtnName, Message= _GLDetail.Message };
                                return RedirectToAction("GLDetail", Objects);
                            }
                        }
                        else
                        {
                            TempData["ListFilterData"] = _GLDetail.ListFilterData1;
                            //_GLDetail = null;
                            return View("~/Areas/BusinessLayer/Views/GLSetup/GLDetail.cshtml", _GLDetail);
                        }
                    case "Forward":
                        return new EmptyResult();
                    //case "Approve":
                    //    //Session["Command"] = command;
                    //    //Session["GLCode"] = acc_id;
                    //    acc_id = _GLDetail.acc_id;
                    //    _GLDetail.Command = command;
                    //    _GLDetail.GLCode = acc_id.ToString();
                    //    //SupplierApprove(_GLDetail, command);
                    //    TempData["ModelData"] = _GLDetail;
                    //    Objects = new { GL_Code = _GLDetail.GLCode, Command = command, TransType = _GLDetail.TransType, BtnName = _GLDetail.BtnName, Message = _GLDetail.Message };
                    //    return RedirectToAction("GLDetail");
                    case "Refresh":
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Refresh";
                        //Session["Message"] = "";
                        //Session["AppStatus"] = "";
                        //_GLDetail = null;
                        // _GLDetail.BtnName = "BtnRefresh";
                        //_GLDetail.Command = command;
                        // _GLDetail.TransType = "Refresh";
                        // _GLDetail.Message = "";
                        // _GLDetail.AppStatus = "";
                        // _GLDetail.acc_type = 0;
                        // _GLDetail.acc_grp_id = 0;
                        // _GLDetail.acc_name = null;
                        // _GLDetail.cf_type = null;
                        // _GLDetail.roa = false;
                        // _GLDetail.iwt = false;
                        // _GLDetail.plr = false;
                        // _GLDetail.egl = false;
                        // _GLDetail.ibt = false;
                        // _GLDetail.sta = false;
                        // _GLDetail.tr = false;
                        // _GLDetail.tp = false;
                        // TempData["ModelData"] = _GLDetail;
                        _GLDetail.hdnSavebtn = null;
                        Objects = new { GL_Code = "", Command = command, TransType = "Refresh", BtnName = "BtnRefresh", Message = "" };
                        //_GLDetail = null;
                        TempData["ListFilterData"] = _GLDetail.ListFilterData1;
                        return RedirectToAction("GLDetail", Objects);
                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        TempData["ListFilterData"] = _GLDetail.ListFilterData1;
                        return RedirectToAction("GLList", "GLList");
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
        public ActionResult InsertGLDetail(GLDetailModel _GLDetail)
        {
            try
            {
                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataTable GLDetail = new DataTable();
                DataTable GLBranch = new DataTable();

                DataTable GLDt = new DataTable();
                GLDt.Columns.Add("TransType", typeof(string));
                GLDt.Columns.Add("acc_id", typeof(int));
                GLDt.Columns.Add("acc_name", typeof(string));
                GLDt.Columns.Add("acc_type", typeof(int));
                GLDt.Columns.Add("acc_grp_id", typeof(int));
                GLDt.Columns.Add("cf_type", typeof(string));
                GLDt.Columns.Add("UserMacaddress", typeof(string));
                GLDt.Columns.Add("UserSystemName", typeof(string));
                GLDt.Columns.Add("UserIP", typeof(string));
                GLDt.Columns.Add("act_status_tr", typeof(string));
                GLDt.Columns.Add("roa", typeof(string));
                GLDt.Columns.Add("plr", typeof(string));
                GLDt.Columns.Add("ibt", typeof(string));
                GLDt.Columns.Add("iwt", typeof(string));
                GLDt.Columns.Add("egl", typeof(string));
                GLDt.Columns.Add("sta", typeof(string));
                GLDt.Columns.Add("tr", typeof(string));
                GLDt.Columns.Add("tp", typeof(string));            
                GLDt.Columns.Add("create_id", typeof(int));
                GLDt.Columns.Add("mod_id", typeof(int));
                GLDt.Columns.Add("comp_id", typeof(int));
                GLDt.Columns.Add("inact_reason", typeof(string));
                GLDt.Columns.Add("curr_id", typeof(int));
                GLDt.Columns.Add("od_allow", typeof(string));
                GLDt.Columns.Add("od_limit", typeof(string));
                GLDt.Columns.Add("bra", typeof(string));             
             
                GLDt.Columns.Add("acc_no", typeof(string));
                GLDt.Columns.Add("ifsc_code", typeof(string));
                GLDt.Columns.Add("swift_code", typeof(string));
                GLDt.Columns.Add("bank_addr", typeof(string));
       
               
                //GLDt.Columns.Add("od_allow", typeof(string));


                DataRow GLDtrow = GLDt.NewRow();
                //GLDtrow["TransType"] = Session["TransType"].ToString();

                GLDtrow["TransType"] = _GLDetail.TransType;
                //GLDtrow["acc_id"] = _GLDetail.acc_id;
                if (_GLDetail.GLCode == null)
                {
                    GLDtrow["acc_id"] = "0";
                }
                else
                {
                    GLDtrow["acc_id"] = _GLDetail.GLCode;
                }
                GLDtrow["acc_name"] = _GLDetail.acc_name;
                GLDtrow["acc_type"] = _GLDetail.acc_type;
                GLDtrow["acc_grp_id"] = _GLDetail.acc_grp_id;
                GLDtrow["cf_type"] = _GLDetail.cf_type;
                GLDtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                GLDtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                GLDtrow["UserIP"] = Session["UserIP"].ToString();
                //if (Session["TransType"].ToString() == "Save")
                if (_GLDetail.TransType == "Save")
                {
                    GLDtrow["act_status_tr"] = "Y";
                }
                else
                {
                    GLDtrow["act_status_tr"] = ToggleResultToString(_GLDetail.act_status_tr);
                   
                }
                GLDtrow["roa"] = ToggleResultToString(_GLDetail.roa);
                GLDtrow["plr"] = ToggleResultToString(_GLDetail.plr);
                GLDtrow["ibt"] = ToggleResultToString(_GLDetail.ibt);
                GLDtrow["iwt"] = ToggleResultToString(_GLDetail.iwt);
                GLDtrow["egl"] = ToggleResultToString(_GLDetail.egl);
                GLDtrow["sta"] = ToggleResultToString(_GLDetail.sta);
                GLDtrow["tr"] = ToggleResultToString(_GLDetail.tr);
                GLDtrow["tp"] = ToggleResultToString(_GLDetail.tp);
                GLDtrow["create_id"] = Session["UserId"].ToString();
                GLDtrow["mod_id"] = Session["UserId"].ToString();
                GLDtrow["comp_id"] = comp_id;
                GLDtrow["inact_reason"] = _GLDetail.inact_reason;
                if (_GLDetail.curr == null)   // modified By Nitesh 16-11-2023 for curr_id not save Zero (handel it Procedure Save base_currid)
                {
                    GLDtrow["curr_id"] = 0;
                }
                else
                {
                    GLDtrow["curr_id"] = _GLDetail.curr;
                }
                GLDtrow["od_allow"] = ToggleResultToString(_GLDetail.TAp);
                GLDtrow["od_limit"] = _GLDetail.Overdraft_Limit;
                GLDtrow["bra"] = ToggleResultToString(_GLDetail.bra);
                if(_GLDetail.acc_type ==7)
                {
                    GLDtrow["bank_addr"] = _GLDetail.bank_add;
                    GLDtrow["acc_no"] = _GLDetail.acc_no;
                    GLDtrow["swift_code"] = _GLDetail.swift_code;
                    GLDtrow["ifsc_code"] = _GLDetail.ifsc_code;
                }
                else
                {
                    GLDtrow["bank_addr"] = null;
                    GLDtrow["acc_no"] = null;
                    GLDtrow["swift_code"] = null;
                    GLDtrow["ifsc_code"] = null;
                }
           
              
                GLDt.Rows.Add(GLDtrow);
                GLDetail = GLDt;
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("acc_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(_GLDetail.GLBranchDetail);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();                   
                     dtrowBrdetails["acc_id"] = _GLDetail.acc_id;   
                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();
                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                GLBranch = dtBranch;
                String SaveMessage = _GLDetail_ISERVICES.insertGLDetail(GLDetail, GLBranch);
                string GLCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "DataNotFound")
                {
                    ViewBag.MenuPageName = getDocumentName();
                    _GLDetail.Title = title;
                    var a = GLCode.Split('-');
                    var msg = "Data Not found" +" "+ a[0].Trim()+" in " + _GLDetail.Title;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _GLDetail.Message = Message;
                    return RedirectToAction("GLDetail");
                }
                if (Message == "Update" || Message == "Save")
                {
                    //Session["Message"] = "Save";
                    //Session["GLCode"] = GLCode;
                    //Session["TransType"] = "Update";

                    _GLDetail.Message = "Save";
                    _GLDetail.GLCode = GLCode;
                    _GLDetail.TransType = "Update";
                }
                if (Message == "Duplicate")
                {
                    //Session["TransType"] = "Duplicate";
                    //Session["Message"] = "Duplicate";
                    //Session["GLCode"] = GLCode;

                    _GLDetail.TransType = "Duplicate";
                    _GLDetail.Message = "Duplicate";
                    //_GLDetail.GLCode = null;
                    //_GLDetail.GLCode = GLCode;
                    _GLDetail.GLCode = GLCode;
                }
                //Session["BtnName"] = "BtnSave";
                _GLDetail.BtnName = "BtnSave";
                TempData["ModelData"] = _GLDetail;
                return RedirectToAction("GLDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GLDetailDelete(GLDetailModel _GLDetail, string command)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string GLCode = _GLDetail.GLCode;
                string DeleteMassage = _GLDetail_ISERVICES.GLDetailDelete(GLCode, Comp_ID);
                if (DeleteMassage == "Used")
                {
                    _GLDetail.Message = "Used";
                    _GLDetail.BtnName = "BtnToDetailPage";
                    _GLDetail.GLCode = GLCode;

                }
                if(DeleteMassage== "Deleted")
                {
                    _GLDetail.BtnName = "Delete";
                    _GLDetail.Message = "Delete";
                    _GLDetail.TransType = "Refresh";
                    _GLDetail.GLCode = null;
                }
                //_GLDetail.BtnName = "Delete";
                TempData["ModelData"] = _GLDetail;
                return RedirectToAction("GLDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private string ToggleResultToString(Boolean _Bool)
        {
            if (_Bool)
            {
                return "Y";
            }
            else
            {
                return "N";
            }
        }
        private Boolean ToggleResultToBoolean(string result)
        {
            if (result.Trim()=="Y")
            {
                return true;
            }
            else
            {
                return false;
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
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }   
        }
        private DataTable GetGLBranchList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _GLDetail_ISERVICES.GetBrList(Comp_ID).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetBrList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _GLDetail_ISERVICES.GetBrListDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetAccountGroup(GLDetailModel _GLDetail1)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                var acc_groupid = "";
                if (_GLDetail1.acc_id != 0)
                {
                    var acc_id = _GLDetail1.acc_id.ToString();
                     var a = acc_id.Split('0');

                    //var groupid_sign = acc_id[0];
                    var groupid_sign = a[0].Trim();
                    if (groupid_sign == "1")
                    {
                        acc_groupid = "101";
                    }

                    else if (groupid_sign == "2")
                    {
                        acc_groupid = "201";
                    }
                    else if (groupid_sign =="3")
                    {
                        acc_groupid = "301";
                    }
                    else if (groupid_sign == "4")
                    {
                        acc_groupid = "401";
                    }
                }
                else
                {
                    acc_groupid = null;
                }
                DataTable dt1;
                DataTable dt2;
              
                if (_GLDetail1.Command=="Edit" && _GLDetail1.TransType == "Update")
                {
                     dt2 = _GLDetail_ISERVICES.GetAccountGroupList(Comp_ID, acc_groupid);
                    return dt2;
                }
                else
                {
                     dt1 = _GLDetail_ISERVICES.GetAccountGroupDAL(Comp_ID);
                    return dt1;
                }
             
              
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [NonAction]
        private DataTable Getcurr()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _GLDetail_ISERVICES.GetCurr(Comp_ID);
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