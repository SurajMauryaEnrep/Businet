using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.BusinessLayer.TaxTemplate;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TaxTemplate;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.TaxTemplate
{
    public class TaxTemplateController : Controller
    {
        string CompID, language = String.Empty;
        string DocumentMenuId = "103157", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        TaxTemplate_Model template_Model;
        Common_IServices _Common_IServices;
        TaxTemplate_ISERVICES _taxTemplate_ISERVICES;
        public TaxTemplateController(Common_IServices _Common_IServices, TaxTemplate_ISERVICES _taxTemplate_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._taxTemplate_ISERVICES = _taxTemplate_ISERVICES;
        }
        // GET: BusinessLayer/TaxTemplate
        public ActionResult TaxTemplate()
        {
            try
            {
                RemoveSession();
                ViewBag.MenuPageName = getDocumentName();
                TaxTemplate_Model template_Model = new TaxTemplate_Model();
                template_Model.Title = title;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                ViewBag.VBRoleList = GetRoleList();
                //Session["TTBtnName"] = "AddNew";
                template_Model.TTBtnName = "AddNew";
                DataTable dt = _taxTemplate_ISERVICES.GetTaxTemplateList(CompID).Tables[0];
                //ViewBag.TaxTemplateList = dt;
                template_Model.TaxTemplateList = dt;
                return View("~/Areas/BusinessLayer/Views/TaxTemplate/TaxTemplateList.cshtml", template_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");

            }

        }
        public void RemoveSession()
        {
            //TaxTemplate_Model template_Model = new TaxTemplate_Model();
            //Session["TTBtnName"] = null;
            //template_Model.TTBtnName = null;
            //Session["TTMessage"] = null;
            //Session["Templateid"] = null;
            //Session["TransType"] = null;
            //Session["Command"] = null;
            //Session["DuplicateData"] = null;
        }
        public ActionResult DblClickToDetail(TaxTemplate_Model template_Model, string tmplt_id)
        {
            try
            {
                //TaxTemplate_Model template_Model = new TaxTemplate_Model();
                //Session["TTMessage"] = null;
                template_Model.Message = null;
                //Session["Templateid"] = tmplt_id;
                template_Model.TemplateId = Convert.ToInt32(tmplt_id);
                template_Model.TTBtnName = "ToViewDetail";
                TempData["Modeldata"] = template_Model;
                var TemplateIdURL = Convert.ToInt32(tmplt_id);
                var TransType = "Update";
                var BtnName = "ToViewDetail";
                var Command = "Add";
                return (RedirectToAction("TaxTemplateDetail", new { TemplateIdURL = TemplateIdURL, TransType, BtnName, Command }));
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");

            }

        }
        public ActionResult TaxTemplateDetail(string TemplateIdURL, string TransType, string BtnName, string Command)
        {
            try
            {
                var template_Model = TempData["Modeldata"] as TaxTemplate_Model;
                if (template_Model != null)
                {
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    //if (Session["Command"] == null)
                    if (template_Model.Command == null)
                    {
                        //Session["Command"] = "Add";
                        template_Model.Command = "Add";
                    }
                    if (template_Model.TTBtnName == null)
                    {
                        template_Model.TTBtnName = "AddNew";
                    }
                    //Session["TransType"] = "Save";
                    template_Model.TransType = "Save";
                    if (template_Model.Message == null)
                    {
                        template_Model.Message = "New";
                    }
                    
                    //if (Session["TTMessage"] != null)
                    //if (template_Model.TTMessage != null)
                    //{
                    //    template_Model.Message = template_Model.TTMessage.ToString();
                    //    //ViewBag.Message = Session["TTMessage"].ToString();
                    //    //Session["TTMessage"] = null;
                    //    template_Model.TTMessage = null;
                    //}
                    ViewBag.VBRoleList = GetRoleList();
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    //TaxTemplate_Model template_Model = new TaxTemplate_Model();
                    ViewBag.MenuPageName = getDocumentName();
                    template_Model.Title = title;
                    var other = new CommonController(_Common_IServices);
                    //ViewBag.CustomerBranchList = other.GetBranchList(CompID);
                    template_Model.CustomerBranchList = other.GetBranchList(CompID);
                    DataSet ds1 = _taxTemplate_ISERVICES.GetModulAndHsnDetails(CompID);
                    //ViewBag.ModuleList = ds1.Tables[0];
                    template_Model.ModuleList = ds1.Tables[0];

                    List<HSNNumbarList> hSNNumbars = new List<HSNNumbarList>();
                    hSNNumbars.Add(new HSNNumbarList { HSNNumber = "---Select---", HSNNumberId = "0" });
                    foreach (DataRow dr in ds1.Tables[1].Rows)
                    {
                        HSNNumbarList numbarList = new HSNNumbarList();
                        numbarList.HSNNumberId = dr["hsn_no"].ToString();
                        numbarList.HSNNumber = dr["hsn_no"].ToString();
                        hSNNumbars.Add(numbarList);
                    }
                    template_Model.hSNNumbars = hSNNumbars;

                    template_Model.BaseAmount = Convert.ToDecimal(100).ToString(ValDigit);
                    template_Model.TemplateStatus = true;

                    //if (Session["Templateid"] != null)
                    if (template_Model.TemplateId != 0)
                    {
                        //Session["TransType"] = "Update";
                        template_Model.TransType = "Update";
                        //string Templateid = Session["Templateid"].ToString();
                        string Templateid = template_Model.TemplateId.ToString();
                        DataSet ds = _taxTemplate_ISERVICES.GetViewDetails(CompID, Templateid);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //ViewBag.TaxDetailList = ds.Tables[1];
                            template_Model.TaxDetailList = ds.Tables[1];
                            //ViewBag.CustomerBranchList = ds.Tables[2];
                            template_Model.CustomerBranchList = ds.Tables[2];
                            //ViewBag.ModuleList = ds.Tables[3];
                            template_Model.ModuleList = ds.Tables[3];
                            //ViewBag.HSNList = ds.Tables[4];
                            template_Model.HSNList = ds.Tables[4];

                            template_Model.BaseAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["base_amt"]).ToString(ValDigit);
                            template_Model.TemplateName = ds.Tables[0].Rows[0]["tmplt_name"].ToString();
                            template_Model.TemplateId = Convert.ToInt32(ds.Tables[0].Rows[0]["tmplt_id"].ToString());
                            template_Model.create_id = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            template_Model.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            template_Model.mod_id = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            template_Model.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            template_Model.tax_type = ds.Tables[0].Rows[0]["tax_type"].ToString();


                            string act_status = ds.Tables[0].Rows[0]["act_status"].ToString();
                            if (act_status == "Y")
                            {
                                template_Model.TemplateStatus = true;
                            }
                            else
                            {
                                template_Model.TemplateStatus = false;
                            }
                            //ViewBag.ValDigit = ValDigit;
                        }
                        else
                        {
                            //Session["Templateid"] = null;
                            template_Model.TemplateId = 0;
                        }
                    }
                    if (template_Model.Message == "Duplicate")
                    {
                        //DataSet DuplicateData = Session["DuplicateData"] as DataSet;
                        DataSet DuplicateData = template_Model.DuplicateData as DataSet;
                        //Session["DuplicateData"] = null;
                        template_Model.DuplicateData = null;
                        //ViewBag.TaxDetailList = DuplicateData.Tables[1];
                        template_Model.TaxDetailList = DuplicateData.Tables[1];
                        //ViewBag.CustomerBranchList = converttable(ViewBag.CustomerBranchList, DuplicateData.Tables[2], "comp_id", "BrActStatus", "br_id", "act_status");

                        template_Model.CustomerBranchList = converttable(template_Model.CustomerBranchList, DuplicateData.Tables[2], "comp_id", "BrActStatus", "br_id", "act_status");
                        //ViewBag.ModuleList = converttable(ds1.Tables[0], DuplicateData.Tables[3], "doc_id", "act_status", "doc_id", "act_status");
                        template_Model.ModuleList = converttable(ds1.Tables[0], DuplicateData.Tables[3], "doc_id", "act_status", "doc_id", "act_status");

                        //ViewBag.HSNList = DuplicateData.Tables[4];
                        template_Model.HSNList = DuplicateData.Tables[4];
                        template_Model.BaseAmount = Convert.ToDecimal(DuplicateData.Tables[1].Rows[0]["base_amt"]).ToString(ValDigit);
                        template_Model.TemplateName = DuplicateData.Tables[0].Rows[0]["tmplt_name"].ToString();
                        template_Model.TemplateId = Convert.ToInt32(DuplicateData.Tables[0].Rows[0]["tmplt_id"].ToString());

                    }
                    ViewBag.ValDigit = ValDigit;
                    return View("~/Areas/BusinessLayer/Views/TaxTemplate/TaxTemplate.cshtml", template_Model);
                }
                else
                {
                    TaxTemplate_Model template_Model1 = new TaxTemplate_Model();
                    if (template_Model1.TemplateId == 0)
                    {
                        template_Model1.TemplateId =Convert.ToInt32(TemplateIdURL);
                    }
                    if (template_Model1.TransType == null)
                    {
                        template_Model1.TransType = TransType;
                    }
                    if (template_Model1.TTBtnName == null)
                    {
                        template_Model1.TTBtnName = BtnName;
                    }
                    if (template_Model1.Command == null)
                    {
                        template_Model1.Command = Command;
                    }
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    //if (Session["Command"] == null)
                    //if (template_Model1.Command == null)
                    //{
                    //    //Session["Command"] = "Add";
                    //    template_Model1.Command = "Add";
                    //}
                    //if (template_Model1.TTBtnName == null)
                    //{
                        //template_Model1.TTBtnName = "Refresh";
                    //}
                    //Session["TransType"] = "Save";

                    ViewBag.Message = "New";
                    //if (Session["TTMessage"] != null)
                    //if (template_Model1.TTMessage != null)
                    //{
                    //    template_Model1.TTMessage = template_Model1.TTMessage.ToString();
                    //    //ViewBag.Message = Session["TTMessage"].ToString();
                    //    //Session["TTMessage"] = null;
                    //    template_Model1.TTMessage = null;
                    //}
                    ViewBag.VBRoleList = GetRoleList();
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    //TaxTemplate_Model template_Model = new TaxTemplate_Model();
                    ViewBag.MenuPageName = getDocumentName();
                    template_Model1.Title = title;
                    var other = new CommonController(_Common_IServices);
                    //ViewBag.CustomerBranchList = other.GetBranchList(CompID);
                    template_Model1.CustomerBranchList = other.GetBranchList(CompID);
                    DataSet ds1 = _taxTemplate_ISERVICES.GetModulAndHsnDetails(CompID);
                    //ViewBag.ModuleList = ds1.Tables[0];
                    template_Model1.ModuleList = ds1.Tables[0];

                    List<HSNNumbarList> hSNNumbars = new List<HSNNumbarList>();
                    hSNNumbars.Add(new HSNNumbarList { HSNNumber = "---Select---", HSNNumberId = "0" });
                    foreach (DataRow dr in ds1.Tables[1].Rows)
                    {
                        HSNNumbarList numbarList = new HSNNumbarList();
                        numbarList.HSNNumberId = dr["hsn_no"].ToString();
                        numbarList.HSNNumber = dr["hsn_no"].ToString();
                        hSNNumbars.Add(numbarList);
                    }

                    template_Model1.hSNNumbars = hSNNumbars;



                    template_Model1.BaseAmount = Convert.ToDecimal(100).ToString(ValDigit);
                    template_Model1.TemplateStatus = true;

                    //if (Session["Templateid"] != null)
                        if (template_Model1.TemplateId != 0)
                        {
                        //Session["TransType"] = "Update";
                        template_Model1.TransType = "Update";
                        //string Templateid = Session["Templateid"].ToString();
                        string Templateid = template_Model1.TemplateId.ToString();
                        DataSet ds = _taxTemplate_ISERVICES.GetViewDetails(CompID, Templateid);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //ViewBag.TaxDetailList = ds.Tables[1];
                            template_Model1.TaxDetailList = ds.Tables[1];
                            //ViewBag.CustomerBranchList = ds.Tables[2];
                            template_Model1.CustomerBranchList = ds.Tables[2];
                            //ViewBag.ModuleList = ds.Tables[3];
                            template_Model1.ModuleList = ds.Tables[3];
                            //ViewBag.HSNList = ds.Tables[4];
                            template_Model1.HSNList = ds.Tables[4];

                            template_Model1.BaseAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["base_amt"]).ToString(ValDigit);
                            template_Model1.TemplateName = ds.Tables[0].Rows[0]["tmplt_name"].ToString();
                            template_Model1.TemplateId = Convert.ToInt32(ds.Tables[0].Rows[0]["tmplt_id"].ToString());
                            template_Model1.create_id = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            template_Model1.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            template_Model1.mod_id = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            template_Model1.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            template_Model1.tax_type = ds.Tables[0].Rows[0]["tax_type"].ToString();


                            string act_status = ds.Tables[0].Rows[0]["act_status"].ToString();
                            if (act_status == "Y")
                            {
                                template_Model1.TemplateStatus = true;
                            }
                            else
                            {
                                template_Model1.TemplateStatus = false;
                            }
                            //ViewBag.ValDigit = ValDigit;
                        }
                        else
                        {
                            //Session["Templateid"] = null;
                            template_Model1.TemplateId = 0;
                        }
                    }
                    if (ViewBag.Message == "Duplicate")
                    {
                        //DataSet DuplicateData = Session["DuplicateData"] as DataSet;
                        DataSet DuplicateData = template_Model1.DuplicateData as DataSet;
                        //Session["DuplicateData"] = null;
                        template_Model1.DuplicateData = null;
                        //ViewBag.TaxDetailList = DuplicateData.Tables[1];
                        template_Model1.TaxDetailList = DuplicateData.Tables[1];
                        //ViewBag.CustomerBranchList = converttable(ViewBag.CustomerBranchList, DuplicateData.Tables[2], "comp_id", "BrActStatus", "br_id", "act_status");

                        template_Model1.CustomerBranchList = converttable(ViewBag.CustomerBranchList, DuplicateData.Tables[2], "comp_id", "BrActStatus", "br_id", "act_status");
                        //ViewBag.ModuleList = converttable(ds1.Tables[0], DuplicateData.Tables[3], "doc_id", "act_status", "doc_id", "act_status");
                        template_Model1.ModuleList = converttable(ds1.Tables[0], DuplicateData.Tables[3], "doc_id", "act_status", "doc_id", "act_status");
                        //ViewBag.HSNList = DuplicateData.Tables[4];
                        template_Model1.HSNList = DuplicateData.Tables[4];
                        template_Model1.BaseAmount = Convert.ToDecimal(DuplicateData.Tables[1].Rows[0]["base_amt"]).ToString(ValDigit);
                        template_Model1.TemplateName = DuplicateData.Tables[0].Rows[0]["tmplt_name"].ToString();
                        template_Model1.TemplateId = Convert.ToInt32(DuplicateData.Tables[0].Rows[0]["tmplt_id"].ToString());

                    }
                    ViewBag.ValDigit = ValDigit;
                    if(template_Model1.TTBtnName == null){
                        template_Model1.TTBtnName= "AddNew";
                        template_Model1.Command="Add";
                        template_Model1.TransType = "Save";
                    }
                    return View("~/Areas/BusinessLayer/Views/TaxTemplate/TaxTemplate.cshtml", template_Model1);
                }
                
            }
            catch(Exception Ex)
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
        public DataTable converttable( DataTable dt1 ,DataTable dt2,string ToCompair1,string ToChange1, string ToCompair2, string ToChange2)
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
                return null;
            }

        }
        public ActionResult SaveTaxTemplate(TaxTemplate_Model template_Model,string Command)
        {
            try
            {
                if (template_Model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "Save":
                        SaveOrUpdateDetails(template_Model, Command);
                        //Session["Command"] = Command;
                        template_Model.Command = Command;
                        if (template_Model.Message == "DataNotFound")
                        {
                            template_Model.hdnSavebtn = null;
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        template_Model.hdnSavebtn = null;
                        var TemplateIdURL = template_Model.TemplateId;
                        var TransType = template_Model.TransType;
                        var BtnName = template_Model.TTBtnName;
                        TempData["Modeldata"] = template_Model;

                        return( RedirectToAction("TaxTemplateDetail", new { TemplateIdURL = TemplateIdURL, TransType, BtnName, Command }));
                    case "Update":
                        SaveOrUpdateDetails(template_Model, Command);
                        //Session["Command"] = Command;
                        template_Model.Command = Command;
                        if (template_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TemplateIdURL = template_Model.TemplateId;
                        TransType = template_Model.TransType;
                        BtnName = template_Model.TTBtnName;
                        TempData["Modeldata"] = template_Model;
                        return( RedirectToAction("TaxTemplateDetail", new { TemplateIdURL = TemplateIdURL, TransType, BtnName, Command }));
                    case "Edit":
                        //SaveOrUpdateDetails(template_Model, Command);
                        //Session["Templateid"] = template_Model.TemplateId;
                        template_Model.TemplateId = template_Model.TemplateId;
                        //Session["Command"] = Command;
                        template_Model.Command = Command;
                        //Session["TTBtnName"] = "Edit";
                        template_Model.TTBtnName = "Edit";

                        TemplateIdURL = template_Model.TemplateId;
                        TransType = template_Model.TransType;
                        BtnName = template_Model.TTBtnName;
                        TempData["Modeldata"] = template_Model;
                        return( RedirectToAction("TaxTemplateDetail", new { TemplateIdURL = TemplateIdURL, TransType, BtnName, Command }));
                    case "Add":
                        //SaveOrUpdateDetails(template_Model, Command);
                        //Session["Command"] = Command;
                        template_Model.Command = Command;
                        //Session["TTBtnName"] = "AddNew";
                        template_Model.TTBtnName = "AddNew";
                        //Session["Templateid"] = null;
                        template_Model.TemplateId = 0;
                        TempData["Modeldata"] = template_Model;
                        return RedirectToAction("TaxTemplateDetail");
                    case "Delete":
                        template_Model.Command = Command;
                        template_Model.TTBtnName = "Delete";
                        template_Model.TemplateId = template_Model.TemplateId;
                        TamplateDetailDelete(template_Model);
                        TempData["Modeldata"] = template_Model;
                        return RedirectToAction("TaxTemplateDetail");
                    case "Refresh"://BacktoList
                        //Session["Templateid"] = null;
                        template_Model.TemplateId = 0;
                        //Session["TTMessage"] = null;
                        template_Model.Message = null;
                        //Session["Command"] = Command;
                        template_Model.Command = Command;
                        //Session["TTBtnName"] = "Refresh";
                        template_Model.TTBtnName = "Refresh";
                        TempData["Modeldata"] = template_Model;
                        return RedirectToAction("TaxTemplateDetail");
                    case "BacktoList":
                        //Session["Templateid"] = null;
                        template_Model.TemplateId = 0;
                        //Session["TTMessage"] = null;
                        template_Model.Message = null;
                        //Session["Command"] = null;
                        template_Model.Command = null;
                        return RedirectToAction("TaxTemplate");
                }
                return RedirectToAction("TaxTemplateDetail");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");

            }
         
        }
        private ActionResult SaveOrUpdateDetails(TaxTemplate_Model template_Model, string Command)
        {

            try
            {
                string userid = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataTable dtTax= ToTaxTable(template_Model.TaxDetails,template_Model.BaseAmount);
                DataTable dtBranch= ToBranchTable(template_Model.BranchMapping);
                DataTable dtModule = ToModuleTable(template_Model.ModuleMapping);
                DataTable dtHSNNumber = ToHSNNumberTable(template_Model.HSNDetail);
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("TransType", typeof(string));
                dtHeader.Columns.Add("tmplt_id", typeof(int));
                dtHeader.Columns.Add("tmplt_name", typeof(string));
                dtHeader.Columns.Add("UserId", typeof(string));
                dtHeader.Columns.Add("act_status", typeof(string));
                dtHeader.Columns.Add("comp_id", typeof(string));
                dtHeader.Columns.Add("mac_id", typeof(string));
                dtHeader.Columns.Add("tax_type", typeof(string));

                DataRow dtHeaderRow = dtHeader.NewRow();
                dtHeaderRow["TransType"] = Command;
                dtHeaderRow["tmplt_id"] = template_Model.TemplateId;
                dtHeaderRow["tmplt_name"] = template_Model.TemplateName; 
                dtHeaderRow["UserId"] = userid;
                string tmplStatus = "";
                if (template_Model.TemplateStatus == true)
                {
                    tmplStatus = "Y";
                }
                else
                {
                    tmplStatus = "N";
                }
                dtHeaderRow["act_status"] = tmplStatus;
                dtHeaderRow["comp_id"] = CompID;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtHeaderRow["mac_id"] = mac_id;
                dtHeaderRow["tax_type"] = template_Model.tax_type;
                dtHeader.Rows.Add(dtHeaderRow);
                string Result = _taxTemplate_ISERVICES.SaveAndUpdateDetails(dtHeader,dtTax,dtBranch, dtModule, dtHSNNumber,"");
                string Templateid = Result.Split(',')[1].Trim();
                string Message = Result.Split(',')[0].Trim();
                ViewBag.MenuPageName = getDocumentName();
                 var Title = title;
                if (Message == "DataNotFound")
                {
                    template_Model.hdnSavebtn = null;
                    var msg = ("Data Not found" + " " + Templateid + " in " + Title);
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    template_Model.Message = Message;
                    return RedirectToAction("TaxTemplateDetail");
                }
                if(Message=="Save"|| Message == "Update")
                {
                    template_Model.hdnSavebtn = null;
                    //Session["TTMessage"] = "Save";
                    template_Model.Message= "Save";
                    //Session["TTBtnName"] = "ToViewDetail";
                    template_Model.TTBtnName= "ToViewDetail";
                }
                if (Message == "Duplicate")
                {
                    template_Model.hdnSavebtn = null;
                    //Session["TTMessage"] = "Duplicate";
                    template_Model.Message= "Duplicate";
                    //Session["TTBtnName"] = "ToViewDetail";
                    DataSet DuplicateData = new DataSet();
                    JArray jObject = JArray.Parse(template_Model.TaxDetailsForDuplicate);
                    dtTax.Columns.Add("tax_name", typeof(string));
                    dtTax.Columns.Add("TaxApplyOnName", typeof(string));
                    for (int i = 0; i < jObject.Count; i++)
                    {                      
                        dtTax.Rows[i]["tax_name"] = jObject[i]["TaxName"].ToString();
                        dtTax.Rows[i]["TaxApplyOnName"] = jObject[i]["TaxApplyOnName"].ToString();
                        dtTax.Rows[i]["tax_rate"] = jObject[i]["TaxRate"].ToString();
                    }
                        DuplicateData.Tables.AddRange(new DataTable[] { dtHeader, dtTax, dtBranch, dtModule, dtHSNNumber });
                    //Session["DuplicateData"] = DuplicateData;
                    template_Model.DuplicateData = DuplicateData;

                }
                template_Model.hdnSavebtn = null;
                //Session["Templateid"] = Templateid;
                template_Model.TemplateId =Convert.ToInt32(Templateid);
                TempData["Modeldata"] = template_Model;
                // string tmpltName = template_Model.TemplateName;
                return RedirectToAction("TaxTemplateDetail");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }         
        }
        public ActionResult TamplateDetailDelete(TaxTemplate_Model template_Model)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string Message = _taxTemplate_ISERVICES.TamplateDetailDelete(template_Model, CompID);
                if (Message == "Deleted")
                {
                    template_Model.Message = "Deleted";
                    template_Model.Command = "Delete";
                    template_Model.TTBtnName = "Delete";
                    template_Model.TransType = "Refresh";
                    return RedirectToAction("TaxTemplateDetail", template_Model);
                }
                else
                {
                    Session["Command"] = "Delete";
                    return View("~/Areas/BusinessLayer/Views/TaxTemplate/TaxTemplate.cshtml", template_Model);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public DataTable ToTaxTable(string TaxDetails,string baseAmount)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("tax_id", typeof(int));
                dt.Columns.Add("base_amt", typeof(string));
                dt.Columns.Add("tax_rate", typeof(string));
                dt.Columns.Add("tax_val", typeof(string));
                dt.Columns.Add("tax_level", typeof(int));
                dt.Columns.Add("tax_apply_on", typeof(string));


                JArray jObject = JArray.Parse(TaxDetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtRow = dt.NewRow();
                    dtRow["tax_id"] = jObject[i]["TaxID"].ToString();
                    dtRow["base_amt"] = baseAmount;
                    dtRow["tax_rate"] = jObject[i]["TaxRate"].ToString();
                    dtRow["tax_val"] = jObject[i]["TaxValue"].ToString();
                    dtRow["tax_level"] = jObject[i]["TaxLevel"].ToString();
                    dtRow["tax_apply_on"] = jObject[i]["TaxApplyOn"].ToString();
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
        public DataTable ToBranchTable(string BranchMapping)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("br_id", typeof(int));
                dt.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(BranchMapping);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtRow = dt.NewRow();
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
        public DataTable ToModuleTable(string ModuleMapping)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("doc_id", typeof(string));
                dt.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(ModuleMapping);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtRow = dt.NewRow();
                    dtRow["doc_id"] = jObject[i]["moduleid"].ToString();
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
        public DataTable ToHSNNumberTable(string BranchMapping)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("hsn_no", typeof(string));

                JArray jObject = JArray.Parse(BranchMapping);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtRow = dt.NewRow();
                    dtRow["hsn_no"] = jObject[i]["HSNNumber"].ToString();
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
        public JsonResult SaveTaxTemplateFromTaxCalc(string DetailList,string tmplt_type)
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
                DataTable dtTax=new DataTable();
                DataTable dtBranch = new DataTable();
                DataTable dtModule = new DataTable();
                DataTable dtHSNNumber = new DataTable();
                var tmplt_name = "";
                JArray jObject = JArray.Parse(DetailList);
                for (int i = 0; i < jObject.Count; i++)
                {
                    tmplt_name = jObject[i]["tmplt_name"].ToString();
                    dtTax = ToTaxTable(jObject[i]["TaxDetails"].ToString(), jObject[i]["BaseAmount"].ToString());
                    dtBranch = ToBranchTable(jObject[i]["BranchDetail"].ToString());
                    dtModule = ToModuleTable(jObject[i]["ModuleMapping"].ToString());
                    dtHSNNumber = ToHSNNumberTable(jObject[i]["HSNDetail"].ToString());
                }

                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("TransType", typeof(string));
                dtHeader.Columns.Add("tmplt_id", typeof(int));
                dtHeader.Columns.Add("tmplt_name", typeof(string));
                dtHeader.Columns.Add("UserId", typeof(string));
                dtHeader.Columns.Add("act_status", typeof(string));
                dtHeader.Columns.Add("comp_id", typeof(string));
                dtHeader.Columns.Add("mac_id", typeof(string));
                dtHeader.Columns.Add("tax_type", typeof(string));

                DataRow dtHeaderRow = dtHeader.NewRow();
                dtHeaderRow["TransType"] = "Save";
                dtHeaderRow["tmplt_id"] = "0";
                dtHeaderRow["tmplt_name"] = tmplt_name;
                dtHeaderRow["UserId"] = userid;
                string tmplStatus = "Y";
                dtHeaderRow["act_status"] = tmplStatus;
                dtHeaderRow["comp_id"] = Comp_ID;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtHeaderRow["mac_id"] = mac_id;
                dtHeaderRow["tax_type"] = tmplt_type;
                dtHeader.Rows.Add(dtHeaderRow);
                string Result =  _taxTemplate_ISERVICES.SaveAndUpdateDetails(dtHeader, dtTax, dtBranch, dtModule, dtHSNNumber,"FromModule");
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


    }

}