using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using System;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TermAndConditionTemplate;
using System.Data;
using EnRepMobileWeb.MODELS.BusinessLayer.TermAndConditionTemplate;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.TermAndConditionTemplate
{
    public class TermAndConditionTemplateController : Controller
    {
        string CompID, language, userid = String.Empty;
        string DocumentMenuId = "103158", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable tmpltList;
        TermAndConditionTemplate_Model termAndConditionTemplate_Model;
        Common_IServices _Common_IServices;
        TermAndConditionTemplate_ISERVICE _TermAndConditionTemplate_ISERVICE;
        public TermAndConditionTemplateController(Common_IServices _Common_IServices, TermAndConditionTemplate_ISERVICE _TermAndConditionTemplate_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._TermAndConditionTemplate_ISERVICE = _TermAndConditionTemplate_ISERVICE;
        }
        // GET: BusinessLayer/TermAndConditionTemplate
        public ActionResult TermAndConditionTemplate()
        {
            try
            {
                TermAndConditionTemplate_Model model = new TermAndConditionTemplate_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                tmpltList = new DataTable();
                tmpltList = _TermAndConditionTemplate_ISERVICE.tmpltList(CompID);
                ViewBag.tamplateList = tmpltList;
                
                ViewBag.VBRoleList = GetRoleList();
                ViewBag.MenuPageName = getDocumentName();
                model.Title = title;
                return View("~/Areas/BusinessLayer/Views/TermAndConditionTemplate/TermAndConditionTemplateList.cshtml",model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

        }
        public ActionResult TermAndConditionTemplateDetail(string TemplateIdURL, string TransType, string BtnName, string Command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                var termAndConditionTemplate_Model = TempData["Modeldata"] as TermAndConditionTemplate_Model;
                if (termAndConditionTemplate_Model != null)
                {
                    termAndConditionTemplate_Model.TransType = "Save";
                    if (termAndConditionTemplate_Model.BtnName == null)
                    {
                        termAndConditionTemplate_Model.BtnName = "AddNew";
                    }
                    if (termAndConditionTemplate_Model.Command == null)
                    {
                        termAndConditionTemplate_Model.Command = "Add";
                    }
                    if (termAndConditionTemplate_Model.Message == null)
                    {
                        termAndConditionTemplate_Model.Message = "New";
                    }
                    var other = new CommonController(_Common_IServices);
                    termAndConditionTemplate_Model.CustomerBranchList = other.GetBranchList(CompID);
                    if (termAndConditionTemplate_Model.TemplateId != null)
                    {
                        termAndConditionTemplate_Model.TransType = "Update";
                        string Templateid = termAndConditionTemplate_Model.TemplateId.ToString();
                        DataSet ds = _TermAndConditionTemplate_ISERVICE.GetViewDetails(CompID, Templateid);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewBag.termtamplate = ds.Tables[1];
                            termAndConditionTemplate_Model.TemplateName = ds.Tables[0].Rows[0]["tmplt_name"].ToString();
                            termAndConditionTemplate_Model.TemplateId = ds.Tables[0].Rows[0]["tmplt_id"].ToString();
                            termAndConditionTemplate_Model.create_id = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            termAndConditionTemplate_Model.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            termAndConditionTemplate_Model.mod_id = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            termAndConditionTemplate_Model.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();

                            string act_status = ds.Tables[0].Rows[0]["act_status"].ToString();
                            if (act_status == "Y")
                            {
                                termAndConditionTemplate_Model.TemplateStatus = true;
                            }
                            else
                            {
                                termAndConditionTemplate_Model.TemplateStatus = false;
                            }
                        }
                        termAndConditionTemplate_Model.CustomerBranchList = ds.Tables[2];
                    }
                    else
                    {
                        
                        termAndConditionTemplate_Model.TemplateId = null;
                        termAndConditionTemplate_Model.TemplateStatus = true;
                    }
                    ViewBag.VBRoleList = GetRoleList();
                    ViewBag.MenuPageName = getDocumentName();
                    termAndConditionTemplate_Model.Title = title;
                    return View("~/Areas/BusinessLayer/Views/TermAndConditionTemplate/TermAndConditionTemplateDetail.cshtml", termAndConditionTemplate_Model);
                }
                else
                {
                    TermAndConditionTemplate_Model termAndConditionTemplate_Model1 = new TermAndConditionTemplate_Model();
                    //if (termAndConditionTemplate_Model1.BtnName == null)
                    //{
                    //    termAndConditionTemplate_Model1.BtnName = "AddNew";
                    //    termAndConditionTemplate_Model1.Command = "Add";
                    //    termAndConditionTemplate_Model1.TransType = "Save";
                    //    termAndConditionTemplate_Model1.Message = "New";
                    //}
                    if (BtnName != null)
                    {
                        termAndConditionTemplate_Model1.BtnName = BtnName;
                    }
                    else
                    {
                        termAndConditionTemplate_Model1.BtnName = "AddNew";
                    }
                    if (TemplateIdURL != null)
                    {
                        termAndConditionTemplate_Model1.TemplateId = TemplateIdURL;
                    }
                    else
                    {
                        termAndConditionTemplate_Model1.TemplateId = null;
                    }
                    if (TransType != null)
                    {
                        termAndConditionTemplate_Model1.TransType = TransType;
                    }
                    else
                    {
                        termAndConditionTemplate_Model1.TransType = "Save";
                    }
                    if (Command != null)
                    {
                        termAndConditionTemplate_Model1.Command = Command;
                    }
                    else
                    {
                        termAndConditionTemplate_Model1.Command = "Add";
                    }
                    var other = new CommonController(_Common_IServices);
                    termAndConditionTemplate_Model1.CustomerBranchList = other.GetBranchList(CompID);
                    if (termAndConditionTemplate_Model1.TemplateId != null)
                    {
                        termAndConditionTemplate_Model1.TransType = "Update";
                        string Templateid = termAndConditionTemplate_Model1.TemplateId.ToString();
                        DataSet ds = _TermAndConditionTemplate_ISERVICE.GetViewDetails(CompID, Templateid);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewBag.termtamplate = ds.Tables[1];
                            termAndConditionTemplate_Model1.TemplateName = ds.Tables[0].Rows[0]["tmplt_name"].ToString();
                            termAndConditionTemplate_Model1.TemplateId = ds.Tables[0].Rows[0]["tmplt_id"].ToString();
                            termAndConditionTemplate_Model1.create_id = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            termAndConditionTemplate_Model1.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            termAndConditionTemplate_Model1.mod_id = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            termAndConditionTemplate_Model1.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();

                            string act_status = ds.Tables[0].Rows[0]["act_status"].ToString();
                            if (act_status == "Y")
                            {
                                termAndConditionTemplate_Model1.TemplateStatus = true;
                            }
                            else
                            {
                                termAndConditionTemplate_Model1.TemplateStatus = false;
                            }
                        }
                        termAndConditionTemplate_Model1.CustomerBranchList = ds.Tables[2];
                    }
                    else
                    {
                        //Session["Templateid"] = null;
                        termAndConditionTemplate_Model1.TemplateId = null;
                    }
                    termAndConditionTemplate_Model1.TemplateStatus = true;
                    ViewBag.VBRoleList = GetRoleList();
                    ViewBag.MenuPageName = getDocumentName();
                    termAndConditionTemplate_Model1.Title = title;
                    return View("~/Areas/BusinessLayer/Views/TermAndConditionTemplate/TermAndConditionTemplateDetail.cshtml", termAndConditionTemplate_Model1);
                }     
                
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult SaveTermAndConditionTemplate(TermAndConditionTemplate_Model termAndConditionTemplate_Model, string Command)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            try
            {
                if (termAndConditionTemplate_Model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "Save":
                        SaveOrUpdateDetails(termAndConditionTemplate_Model, Command);
                        if(termAndConditionTemplate_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (termAndConditionTemplate_Model.Message == "Duplicate")
                        {
                            var other = new CommonController(_Common_IServices);
                            termAndConditionTemplate_Model.CustomerBranchList = other.GetBranchList(CompID);
                            if (termAndConditionTemplate_Model.TemplateId != null && termAndConditionTemplate_Model.TemplateId != "0")
                            {
                                string TemplateId = termAndConditionTemplate_Model.TemplateId.ToString();
                                DataSet ds = _TermAndConditionTemplate_ISERVICE.GetViewDetails(CompID, TemplateId);
                                termAndConditionTemplate_Model.create_id = ds.Tables[0].Rows[0]["create_nm"].ToString();
                                termAndConditionTemplate_Model.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                termAndConditionTemplate_Model.mod_id = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                                termAndConditionTemplate_Model.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                ViewBag.termtamplate = ds.Tables[1];
                                termAndConditionTemplate_Model.CustomerBranchList = ds.Tables[2];
                            }

                            termAndConditionTemplate_Model.TemplateId = null;
                            termAndConditionTemplate_Model.TransType = "Save";
                            termAndConditionTemplate_Model.BtnName = "AddNew";
                            if (termAndConditionTemplate_Model.TemplateStatus == false)
                            {
                                termAndConditionTemplate_Model.TemplateStatus = true;
                            }
                            DataSet DuplicateData = termAndConditionTemplate_Model.DuplicateData as DataSet;
                            termAndConditionTemplate_Model.DuplicateData = null;
                            ViewBag.termtamplate = DuplicateData.Tables[1];
                            ViewBag.VBRoleList = GetRoleList();
                            ViewBag.MenuPageName = getDocumentName();
                            termAndConditionTemplate_Model.Title = title;
                            termAndConditionTemplate_Model.hdnSavebtn = null;
                            return View("~/Areas/BusinessLayer/Views/TermAndConditionTemplate/TermAndConditionTemplateDetail.cshtml", termAndConditionTemplate_Model);
                        }
                            termAndConditionTemplate_Model.Command = Command;
                        var TemplateIdURL = termAndConditionTemplate_Model.TemplateId;
                        var TransType = termAndConditionTemplate_Model.TransType;
                        var BtnName = termAndConditionTemplate_Model.BtnName;
                        TempData["Modeldata"] = termAndConditionTemplate_Model;
                        return (RedirectToAction("TermAndConditionTemplateDetail", new { TemplateIdURL = TemplateIdURL, TransType, BtnName, Command }));
                    case "Update":
                        SaveOrUpdateDetails(termAndConditionTemplate_Model, Command);
                        termAndConditionTemplate_Model.Command = Command;
                        if (termAndConditionTemplate_Model.Message == "Duplicate")
                        {
                            var other = new CommonController(_Common_IServices);
                            termAndConditionTemplate_Model.CustomerBranchList = other.GetBranchList(CompID);
                            if (termAndConditionTemplate_Model.TemplateId != null && termAndConditionTemplate_Model.TemplateId != "0")
                            {
                                string TemplateId = termAndConditionTemplate_Model.TemplateId.ToString();
                                DataSet ds = _TermAndConditionTemplate_ISERVICE.GetViewDetails(CompID, TemplateId);
                                termAndConditionTemplate_Model.create_id = ds.Tables[0].Rows[0]["create_nm"].ToString();
                                termAndConditionTemplate_Model.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                termAndConditionTemplate_Model.mod_id = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                                termAndConditionTemplate_Model.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                ViewBag.termtamplate = ds.Tables[1];
                                termAndConditionTemplate_Model.CustomerBranchList = ds.Tables[2];
                            }

                            termAndConditionTemplate_Model.TemplateId = null;
                            termAndConditionTemplate_Model.TransType = "Save";
                            termAndConditionTemplate_Model.BtnName = "AddNew";
                            if (termAndConditionTemplate_Model.TemplateStatus == false)
                            {
                                termAndConditionTemplate_Model.TemplateStatus = true;
                            }
                            DataSet DuplicateData = termAndConditionTemplate_Model.DuplicateData as DataSet;
                            termAndConditionTemplate_Model.DuplicateData = null;
                            ViewBag.termtamplate = DuplicateData.Tables[1];
                            ViewBag.VBRoleList = GetRoleList();
                            ViewBag.MenuPageName = getDocumentName();
                            termAndConditionTemplate_Model.Title = title;
                            termAndConditionTemplate_Model.hdnSavebtn = null;
                            return View("~/Areas/BusinessLayer/Views/TermAndConditionTemplate/TermAndConditionTemplateDetail.cshtml", termAndConditionTemplate_Model);
                        }
                        TemplateIdURL = termAndConditionTemplate_Model.TemplateId;
                        TransType = termAndConditionTemplate_Model.TransType;
                        BtnName = termAndConditionTemplate_Model.BtnName;
                        TempData["Modeldata"] = termAndConditionTemplate_Model;
                        return (RedirectToAction("TermAndConditionTemplateDetail", new { TemplateIdURL = TemplateIdURL, TransType, BtnName, Command }));
                    case "Edit":
                        termAndConditionTemplate_Model.TemplateId = termAndConditionTemplate_Model.TemplateId;
                        termAndConditionTemplate_Model.Command = Command;
                        termAndConditionTemplate_Model.hdnSavebtn = null;
                        termAndConditionTemplate_Model.BtnName = "Edit";
                        TemplateIdURL = termAndConditionTemplate_Model.TemplateId;
                        TransType = termAndConditionTemplate_Model.TransType;
                        BtnName = termAndConditionTemplate_Model.BtnName;
                        TempData["Modeldata"] = termAndConditionTemplate_Model;
                        return (RedirectToAction("TermAndConditionTemplateDetail", new { TemplateIdURL = TemplateIdURL, TransType, BtnName, Command }));
                    case "Add":
                        termAndConditionTemplate_Model.Command = Command;
                        termAndConditionTemplate_Model.hdnSavebtn = null;
                        termAndConditionTemplate_Model.BtnName = "AddNew";
                        termAndConditionTemplate_Model.TemplateId = null;
                        TempData["Modeldata"] = termAndConditionTemplate_Model;
                        return RedirectToAction("TermAndConditionTemplateDetail");
                    case "Delete":
                        termAndConditionTemplate_Model.Command = Command;
                        termAndConditionTemplate_Model.hdnSavebtn = null;
                        termAndConditionTemplate_Model.BtnName = "Delete";
                        termAndConditionTemplate_Model.TemplateId = termAndConditionTemplate_Model.TemplateId;
                        DeleteTermAndConditionTemplate(termAndConditionTemplate_Model);
                        TempData["Modeldata"] = termAndConditionTemplate_Model;
                        return RedirectToAction("TermAndConditionTemplateDetail");
                    case "Refresh"://BacktoList
                        termAndConditionTemplate_Model.TemplateId = null;
                        termAndConditionTemplate_Model.hdnSavebtn = null;
                        termAndConditionTemplate_Model.Message = null;
                        termAndConditionTemplate_Model.TemplateName = null;
                        termAndConditionTemplate_Model.TermsAndConditions = null;
                        //termAndConditionTemplate_Model.TaxCalculatorTbl = null;
                        termAndConditionTemplate_Model.Command = Command;
                        termAndConditionTemplate_Model.BtnName = "Refresh";
                        TempData["Modeldata"] = termAndConditionTemplate_Model;
                        return RedirectToAction("TermAndConditionTemplateDetail");
                    case "BacktoList":
                        termAndConditionTemplate_Model.TemplateId = null;
                        termAndConditionTemplate_Model.hdnSavebtn = null;
                        termAndConditionTemplate_Model.Message = null;
                        termAndConditionTemplate_Model.Command = null;
                        return RedirectToAction("TermAndConditionTemplate");
                }
                return RedirectToAction("TermAndConditionTemplate");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private ActionResult SaveOrUpdateDetails(TermAndConditionTemplate_Model termAndConditionTemplate_Model, string Command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                if (termAndConditionTemplate_Model.TemplateId != null)
                {
                    var tmplt_id = termAndConditionTemplate_Model.TemplateId;
                }
                else
                {
                    termAndConditionTemplate_Model.TemplateId = "";
                }
                DataTable dtTermsConditions = ToTermsConditionsTable(termAndConditionTemplate_Model.TermsConditions, termAndConditionTemplate_Model.TemplateId);

                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("TransType", typeof(string));
                dtHeader.Columns.Add("comp_id", typeof(string));
                dtHeader.Columns.Add("tmplt_id", typeof(string));
                dtHeader.Columns.Add("tmplt_name", typeof(string));
                dtHeader.Columns.Add("act_status", typeof(string));
                dtHeader.Columns.Add("mac_id", typeof(string)); 
                dtHeader.Columns.Add("UserId", typeof(string));

                DataRow dtHeaderRow = dtHeader.NewRow();
                dtHeaderRow["TransType"] = Command;
                dtHeaderRow["comp_id"] = CompID;
                dtHeaderRow["tmplt_id"] = termAndConditionTemplate_Model.TemplateId;
                dtHeaderRow["tmplt_name"] = termAndConditionTemplate_Model.TemplateName;
                string tmplStatus = "";
                if (termAndConditionTemplate_Model.TemplateStatus == true)
                {
                    tmplStatus = "Y";
                }
                else
                {
                    tmplStatus = "N";
                }
                dtHeaderRow["act_status"] = tmplStatus;    
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtHeaderRow["mac_id"] = mac_id;
                dtHeaderRow["UserId"] = userid;
                dtHeader.Rows.Add(dtHeaderRow);

                DataTable TaxBranch = new DataTable();
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(string));
                dtBranch.Columns.Add("tmplt_id", typeof(string));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(termAndConditionTemplate_Model.BranchMapping);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    if (termAndConditionTemplate_Model.TemplateId != null || termAndConditionTemplate_Model.TemplateId != "")
                    {
                        dtrowBrdetails["tmplt_id"] = termAndConditionTemplate_Model.TemplateId;
                    }
                    else
                    {
                        dtrowBrdetails["tmplt_id"] = "";
                    }                   
                    dtrowBrdetails["br_id"] = jObject[i]["brid"].ToString();
                    dtrowBrdetails["act_status"] = jObject[i]["ActiveStatus"].ToString();

                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                TaxBranch = dtBranch;
                string Result = _TermAndConditionTemplate_ISERVICE.SaveAndUpdateDetails(dtHeader, dtTermsConditions, TaxBranch);
                string Templateid = Result.Split(',')[1].Trim();
                string Message = Result.Split(',')[0].Trim();
                if (Message == "Data_Not_Found")
                {
                    ViewBag.MenuPageName = getDocumentName();
                    termAndConditionTemplate_Model.Title = title;
                    termAndConditionTemplate_Model.hdnSavebtn = null;
                    //var a = Templateid.Split('-');
                    var msg = Message.Replace("_", " ") + " " + Templateid+" in " + termAndConditionTemplate_Model.Title;
                    //var msg = "Data Not Found" +" "+ a[0].Trim();
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    termAndConditionTemplate_Model.Message = Message.Replace("_", "");
                    return RedirectToAction("TermAndConditionTemplateDetail");
                }
                if (Message == "Save" || Message == "Update")
                {
                    termAndConditionTemplate_Model.Message = "Save";
                    termAndConditionTemplate_Model.BtnName = "ToViewDetail";
                    termAndConditionTemplate_Model.TemplateId= Templateid;
                    termAndConditionTemplate_Model.TransType = Message;
                }
                if (Message == "Duplicate")
                {

                    termAndConditionTemplate_Model.hdnSavebtn = null;
                    termAndConditionTemplate_Model.Message = "Duplicate";
                    DataSet DuplicateData = new DataSet();
                    JArray jObject1 = JArray.Parse(termAndConditionTemplate_Model.TermAndConditionForDuplicate);
                    dtTermsConditions.Columns.Add("term_desc1", typeof(string));
                    for (int i = 0; i < jObject1.Count; i++)
                    {
                        dtTermsConditions.Rows[i]["term_desc1"] = jObject1[i]["TermCndsn"].ToString();
                    }
                    DuplicateData.Tables.AddRange(new DataTable[] { dtHeader, dtTermsConditions, TaxBranch});
                    //Session["DuplicateData"] = DuplicateData;
                    termAndConditionTemplate_Model.DuplicateData = DuplicateData;
                }
                //Session["Templateid"] = Templateid;
                termAndConditionTemplate_Model.TemplateId = Templateid;
                TempData["Modeldata"] = termAndConditionTemplate_Model;
                return RedirectToAction("TermAndConditionTemplateDetail");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public JsonResult SaveTermTemplate(string DetailList)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string userid = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataTable TermDetails = new DataTable();
                DataTable dtBranch = new DataTable();
                var tmplt_name = "";
                JArray jObject = JArray.Parse(DetailList);
                for (int i = 0; i < jObject.Count; i++)
                {
                    tmplt_name = jObject[i]["tmplt_name"].ToString();
                    TermDetails = ToTermsConditionsTable(jObject[i]["TermDetails"].ToString(),"");
                    dtBranch = ToBranchTable(jObject[i]["BranchDetail"].ToString());
                }

                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("TransType", typeof(string));
                dtHeader.Columns.Add("comp_id", typeof(string));
                dtHeader.Columns.Add("tmplt_id", typeof(int));
                dtHeader.Columns.Add("tmplt_name", typeof(string));
                dtHeader.Columns.Add("act_status", typeof(string));
                dtHeader.Columns.Add("mac_id", typeof(string));
                dtHeader.Columns.Add("UserId", typeof(string));

                DataRow dtHeaderRow = dtHeader.NewRow();
                dtHeaderRow["TransType"] = "Save";
                dtHeaderRow["comp_id"] = Comp_ID;
                dtHeaderRow["tmplt_id"] = "0";
                dtHeaderRow["tmplt_name"] = tmplt_name;
                string tmplStatus = "Y";
                dtHeaderRow["act_status"] = tmplStatus;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtHeaderRow["mac_id"] = mac_id;
                dtHeaderRow["UserId"] = userid;
                dtHeader.Rows.Add(dtHeaderRow);
                string Result = _TermAndConditionTemplate_ISERVICE.SaveAndUpdateDetails(dtHeader, TermDetails, dtBranch);
                string Message = Result.Split(',')[0].Trim();
                DataRows = Json(JsonConvert.SerializeObject(Message));/*Result convert into Json Format for javasript*/
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public DataTable ToBranchTable(string BranchMapping)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("comp_id", typeof(string));
                dt.Columns.Add("tmplt_id", typeof(string));
                dt.Columns.Add("br_id", typeof(int));
                dt.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(BranchMapping);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtRow = dt.NewRow();
                    dtRow["comp_id"] = Comp_ID;
                    dtRow["tmplt_id"] = "";
                    dtRow["br_id"] = jObject[i]["brid"].ToString();
                    dtRow["act_status"] = jObject[i]["ActiveStatus"].ToString();
                    dt.Rows.Add(dtRow);
                }
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }

        }
        public ActionResult DblClickToDetail(TermAndConditionTemplate_Model termAndConditionTemplate_Model, string tmplt_id)
        {
            //TaxTemplate_Model template_Model = new TaxTemplate_Model();
            //Session["TTMessage"] = null;
            termAndConditionTemplate_Model.Message = null;
            //Session["Templateid"] = tmplt_id;
            termAndConditionTemplate_Model.TemplateId = tmplt_id;
            termAndConditionTemplate_Model.BtnName = "ToViewDetail";
            TempData["Modeldata"] = termAndConditionTemplate_Model;
            var TemplateIdURL = Convert.ToInt32(tmplt_id);
            var TransType = "Update";
            var BtnName = "ToViewDetail";
            var Command = "Add";
            return (RedirectToAction("TermAndConditionTemplateDetail", new { TemplateIdURL = TemplateIdURL, TransType, BtnName, Command }));
        }
        public ActionResult DeleteTermAndConditionTemplate(TermAndConditionTemplate_Model termAndConditionTemplate_Model)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string Message = _TermAndConditionTemplate_ISERVICE.TamplateDetailDelete(termAndConditionTemplate_Model, CompID);
                if (Message == "Deleted")
                {
                    termAndConditionTemplate_Model.Message = "Deleted";
                    termAndConditionTemplate_Model.Command = "Refresh";
                    termAndConditionTemplate_Model.BtnName = "Delete";
                    termAndConditionTemplate_Model.TransType = "Refresh";
                    termAndConditionTemplate_Model.TemplateId = null;


                }
                return RedirectToAction("TaxTemplateDetail", termAndConditionTemplate_Model);
                //return RedirectToAction("TermAndConditionTemplateDetail");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private DataTable GetRoleList()
        {
            try
            {
                string UserID = "";
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
        public DataTable converttable(DataTable dt1, DataTable dt2, string ToCompair1, string ToChange1, string ToCompair2, string ToChange2)
        {
            try
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i][ToCompair1].ToString() == dt2.Rows[i][ToCompair2].ToString())
                    {
                        dt1.Rows[i][ToChange1] = dt2.Rows[i][ToChange2];
                    }
                }
                return dt1;
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

        public DataTable ToTermsConditionsTable(string TermsConditions,string TemplateId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("comp_id", typeof(int));
                dt.Columns.Add("tmplt_id", typeof(string));
                dt.Columns.Add("term_desc", typeof(string));

                JArray jObject = JArray.Parse(TermsConditions);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtRow = dt.NewRow();
                    dtRow["comp_id"] = Session["CompId"].ToString();
                    if (TemplateId == "")
                    {
                        dtRow["tmplt_id"] = "";
                    }
                    else
                    {
                        dtRow["tmplt_id"] = TemplateId;
                    }
                    dtRow["term_desc"] = jObject[i]["TermCndsn"].ToString();
                    dt.Rows.Add(dtRow);
                }
                return dt;
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














