using EnRepMobileWeb.MODELS.BusinessLayer.MiscellaneousSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.MiscellaneousSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.MiscellaneousSetup.HSNCode
{
    public class HSNCodeController : Controller
    {
        string CompID, branchID, user_id, language = String.Empty;
        string DocumentMenuId = "103205103", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        HSNCode_ISERVICES _HSNNCode_Iservices;
        public HSNCodeController(Common_IServices _Common_IServices, HSNCode_ISERVICES _HSNNCode_Iservices)
        {
            this._Common_IServices = _Common_IServices;
            this._HSNNCode_Iservices = _HSNNCode_Iservices;
        }
        // GET: BusinessLayer/HSNCode
        public ActionResult HSNCode()
        {
            try
            {
                CommonPageDetails();
                ViewBag.MenuPageName = getDocumentName();
                HSNCode_Model _HSNCodeModel = TempData["ModelData"] as HSNCode_Model;
                if (_HSNCodeModel != null)
                {
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    DataSet ds = _HSNNCode_Iservices.Get_HsnDetails(CompID);
                    ViewBag.HsnList = ds.Tables[0];
                    //if (Session["MessageHsn"] != null)
                    //if (Session["MessageHsn"] != null)
                    if (_HSNCodeModel.MessageHsn != null)
                    {
                        //ViewBag.Message = Session["MessageHsn"].ToString();
                        _HSNCodeModel.Message = _HSNCodeModel.MessageHsn;
                        //Session["MessageHsn"] = null;
                        _HSNCodeModel.MessageHsn = null;
                        //if (ViewBag.Message == "Duplicate")
                        if (_HSNCodeModel.Message == "Duplicate")
                        {

                            //_HSNCodeModel.Remarks = Session["hsn_rem"].ToString();
                            //_HSNCodeModel.DBKCode = Session["dbk_code"].ToString();
                            //_HSNCodeModel.HSNNumber = Session["hsn_val"].ToString();
                            //_HSNCodeModel.HSN_id = Session["hsn_id"].ToString();
                            //_HSNCodeModel.BtnName = "Update";

                            _HSNCodeModel.Remarks = _HSNCodeModel.hsn_rem;
                            _HSNCodeModel.DBKCode = _HSNCodeModel.dbk_code;
                            _HSNCodeModel.HSNNumber = _HSNCodeModel.hsn_val;
                            _HSNCodeModel.HSN_id = _HSNCodeModel.HSN_id;
                            _HSNCodeModel.HSNDescription = _HSNCodeModel.hsn_desc;
                            _HSNCodeModel.BtnName = "Update";
                            //Session["hsn_rem"] = null;
                            //Session["dbk_code"] = null;
                            //Session["hsn_val"] = null;
                            //Session["hsn_id"] = null;

                            _HSNCodeModel.hsn_rem = null;
                            _HSNCodeModel.dbk_code = null;
                            _HSNCodeModel.hsn_val = null;
                            _HSNCodeModel.hsn_desc = null;
                            //_HSNCodeModel.HSN_id = null;
                        }
                        else
                        {
                            _HSNCodeModel.BtnName = "Save";
                        }
                        _HSNCodeModel.MessageHsn = null;

                    }
                    else
                    {
                        _HSNCodeModel.BtnName = "Save";
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    _HSNCodeModel.Title = title;
                    return View("~/Areas/BusinessLayer/Views/MiscellaneousSetup/HSNCode/HSNCodeDetail.cshtml", _HSNCodeModel);
                }
                else
                {
                    HSNCode_Model _HSNCodeModel1 = new HSNCode_Model();
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    DataSet ds = _HSNNCode_Iservices.Get_HsnDetails(CompID);
                    ViewBag.HsnList = ds.Tables[0];
                    //if (Session["MessageHsn"] != null)
                    //if (Session["MessageHsn"] != null)
                    if (_HSNCodeModel1.MessageHsn != null)
                    {
                        //ViewBag.Message = Session["MessageHsn"].ToString();
                        //_HSNCodeModel.Message = _HSNCodeModel.MessageHsn.ToString();
                        ////Session["MessageHsn"] = null;
                        //_HSNCodeModel.MessageHsn = null;
                        ////if (ViewBag.Message == "Duplicate")
                        //if (_HSNCodeModel.Message == "Duplicate")
                        //{

                        //    //_HSNCodeModel.Remarks = Session["hsn_rem"].ToString();
                        //    //_HSNCodeModel.DBKCode = Session["dbk_code"].ToString();
                        //    //_HSNCodeModel.HSNNumber = Session["hsn_val"].ToString();
                        //    //_HSNCodeModel.HSN_id = Session["hsn_id"].ToString();
                        //    //_HSNCodeModel.BtnName = "Update";

                        //    _HSNCodeModel.Remarks = _HSNCodeModel.hsn_rem;
                        //    _HSNCodeModel.DBKCode = _HSNCodeModel.dbk_code;
                        //    _HSNCodeModel.HSNNumber = _HSNCodeModel.hsn_val;
                        //    _HSNCodeModel.HSN_id = _HSNCodeModel.hsn_id;
                        //    _HSNCodeModel.BtnName = "Update";
                        //    //Session["hsn_rem"] = null;
                        //    //Session["dbk_code"] = null;
                        //    //Session["hsn_val"] = null;
                        //    //Session["hsn_id"] = null;

                        //    _HSNCodeModel.hsn_rem = null;
                        //    _HSNCodeModel.dbk_code = null;
                        //    _HSNCodeModel.hsn_val = null;
                        //    _HSNCodeModel.hsn_id = null;
                        //}
                        //else
                        //{
                        //    _HSNCodeModel.BtnName = "Save";
                        //}
                        //_HSNCodeModel.MessageHsn = null;
                    }
                    else
                    {
                        _HSNCodeModel1.BtnName = "Save";
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    _HSNCodeModel1.Title = title;
                    return View("~/Areas/BusinessLayer/Views/MiscellaneousSetup/HSNCode/HSNCodeDetail.cshtml", _HSNCodeModel1);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
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
        private string IsNull(string Str, string Str2)
        {
            if (!string.IsNullOrEmpty(Str))
            {
            }
            else
                Str = Str2;
            return Str;
        }
    
        public JsonResult GetTaxDetailAgainstHSN(string HsnNumber)
        {

            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;               
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                
                DataSet result = _HSNNCode_Iservices.GetTaxDetailAgainstHSN( HsnNumber, Comp_ID);
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
        public ActionResult SaveDetail(HSNCode_Model _HSNCodeModel, string Command)
        {
            try
            {
                switch (Command)
                {
                    case "Save":
                        //Session["BtnNameMIS"] = Command;
                        _HSNCodeModel.BtnNameMIS = Command;
                        SaveHSNCodeDetail("", _HSNCodeModel.DBKCode, _HSNCodeModel.HSNNumber, _HSNCodeModel.Remarks, _HSNCodeModel.HSNDescription, "Save");
                        return RedirectToAction("HSNCode");
                    case "Update":
                        //Session["BtnNameMIS"] = Command;
                        _HSNCodeModel.BtnNameMIS = Command;
                        SaveHSNCodeDetail(_HSNCodeModel.HSN_id, _HSNCodeModel.DBKCode, _HSNCodeModel.HSNNumber, _HSNCodeModel.Remarks, _HSNCodeModel.HSNDescription ,"Update");
                        return RedirectToAction("HSNCode");
                }

                return RedirectToAction("HSNCode");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            
        }
        private ActionResult SaveHSNCodeDetail(string hsn_id, string dbk_code,string hsn_val,string remarks,string hsn_desc, string Command)
        {
            try
            {
                HSNCode_Model _HSNCodeMode = new HSNCode_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (hsn_id == null)
                {
                    hsn_id = "";
                }
                string SystemDetail = string.Empty;
                SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                string mac_id = SystemDetail;
                var hsndes = _HSNCodeMode.HSNDescription;
                string SaveMessage = _HSNNCode_Iservices.SaveHSNDetails(Command, hsn_id, dbk_code, hsn_val, remarks, hsn_desc, mac_id, CompID);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));

                if (Message == "Update" || Message == "Save")
                {
                    //Session["TransTypehsn"] = null;
                    //Session["MessageHsn"] = "Save";
                    _HSNCodeMode.TransTypehsn = null;
                    _HSNCodeMode.MessageHsn = "Save";
                }
                if (Message == "Duplicate")
                {
                    //Session["MessageHsn"] = "Duplicate";
                    //Session["hsn_rem"] = IsNull(remarks,"");
                    //Session["dbk_code"] = IsNull(dbk_code, "");
                    //Session["hsn_val"] = IsNull(hsn_val,"");
                    //Session["hsn_id"] = IsNull(hsn_id,"");

                    _HSNCodeMode.MessageHsn = "Duplicate";
                    _HSNCodeMode.hsn_rem = IsNull(remarks, "");
                    _HSNCodeMode.dbk_code = IsNull(dbk_code, "");
                    _HSNCodeMode.hsn_val = IsNull(hsn_val, "");
                    _HSNCodeMode.HSN_id = IsNull(hsn_id, "");
                    _HSNCodeMode.hsn_desc = IsNull(hsn_desc, "");
                }
                TempData["ModelData"] = _HSNCodeMode;
                return RedirectToAction("HSNCode");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        [HttpPost]
        public string DeleteHSNDetail(string HsnNumber)
        {
            try
            {
                HSNCode_Model _HSNCodeMode = new HSNCode_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string Message = "";
                if (HsnNumber != "")
                {
                    string SaveMessage = _HSNNCode_Iservices.DeleteHSNDetail(HsnNumber, CompID);
                    Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "Deleted")
                    {

                        //Session["MessageHsn"] = "Deleted";
                    }
                    if (Message == "Used")
                    {
                        //Session["MessageHsn"] = "Used";
                    }
                }

                return Message;
                //return RedirectToAction("HSNCode");
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
    }

}












