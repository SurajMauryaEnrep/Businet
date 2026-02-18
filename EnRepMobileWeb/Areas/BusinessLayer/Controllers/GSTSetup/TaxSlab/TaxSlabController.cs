using EnRepMobileWeb.MODELS.BusinessLayer.TaxSlab;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.GSTSetup.TaxSlab;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.GSTSetup.TaxSlab
{
    public class TaxSlabController : Controller
    {
        string CompID, branchId, userId, language, title = String.Empty;
        string DocumentMenuId = "103156001";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        TaxSlab_ISERVICE _TaxSlab_ISERVICE;
        public TaxSlabController(Common_IServices _Common_IServices, TaxSlab_ISERVICE _TaxSlab_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._TaxSlab_ISERVICE = _TaxSlab_ISERVICE;
        }
        // GET: BusinessLayer/TaxSlab
        public ActionResult TaxSlab()
        {
            try
            {
                CommonPageDetails();
                var _taxSlab_Model = TempData["Modeldata"] as TaxSlab_Model;
                if (_taxSlab_Model != null)
                {
                    TaxSlab_Model _Model = new TaxSlab_Model();

                    if (_taxSlab_Model.Message == "Save")
                    {
                        _Model.Message = "Save";
                    }
                    if (_taxSlab_Model.Message == "Duplicate")
                    {
                        _Model.Message = "Duplicate";
                    }
                    if (_taxSlab_Model.Message == "Delete")
                    {
                        _Model.Message = "Delete";
                    }
                    if (_taxSlab_Model.Message == "Used")
                    {
                        _Model.Message = "Used";
                    }
                    string Comp_ID = string.Empty;

                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }


                    getdatadropdown(_Model);
                    //ViewBag.MenuPageName = getDocumentName();
                    if(_taxSlab_Model.Message== "Duplicate")
                    {
                        _Model.tax_per = _taxSlab_Model.tax_per;
                    }
                    _Model.Title = title;
                    return View("~/Areas/BusinessLayer/Views/GSTSetup/TaxSlab/TaxSlab.cshtml", _Model);
                }
                else
                {
                    TaxSlab_Model _Model = new TaxSlab_Model();
                    string Comp_ID = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    string Language = string.Empty;
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    getdatadropdown(_Model);
                    //ViewBag.MenuPageName = getDocumentName();
                    _Model.Title = title;
                    return View("~/Areas/BusinessLayer/Views/GSTSetup/TaxSlab/TaxSlab.cshtml", _Model);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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
                    branchId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userId = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, branchId, userId, DocumentMenuId, language);
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
        private void getdatadropdown(TaxSlab_Model _Model)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string Language = string.Empty;
            if (Session["Language"] != null)
            {
                Language = Session["Language"].ToString();
            }
            DataSet ds1 = _TaxSlab_ISERVICE.GetDataDropDownList(Comp_ID);
            List<HSNNumbarList> hSNNumbars = new List<HSNNumbarList>();
            hSNNumbars.Add(new HSNNumbarList { HSNNumber = "---Select---", HSNNumberId = "0" });
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                HSNNumbarList numbarList = new HSNNumbarList();
                numbarList.HSNNumberId = dr["hsn_no"].ToString();
                numbarList.HSNNumber = dr["hsn_no"].ToString();
                hSNNumbars.Add(numbarList);
            }
            _Model.hSNNumbars = hSNNumbars;

            List<taxPerlist> _attrlists = new List<taxPerlist>();
            foreach (DataRow dr in ds1.Tables[1].Rows)
            {
                taxPerlist _Attribute = new taxPerlist();
                _Attribute.taxPer_id = dr["taxPer_id"].ToString();
                _Attribute.taxPer = dr["taxPer"].ToString();
                _attrlists.Add(_Attribute);
            }
            _Model.TaxPerlist = _attrlists;
            #region Commented By Nitesh 08-04-2024 For All List Page In One Procedure
            //DataSet ds = _TaxSlab_ISERVICE.GetTaxSlabDetail(Comp_ID);
            #endregion
            ViewBag.TaxSlabList = ds1.Tables[3];
            ViewBag.HSNList = ds1.Tables[4];
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveTaxSlab(TaxSlab_Model _TaxSlab_Model, string command)
        {
            try
            {
                if (_TaxSlab_Model.hdnAction == "Delete")
                {
                    command = "Delete";
                }
                if(_TaxSlab_Model.hdnAction == "DeleteHSN")
                {
                    command = "DeleteHSN";
                }
                switch (command)
                {
                    case "Delete":                   
                       // Session["Command"] = command;
                       // Session["BtnName"] = "Refresh";
                        string CompID = string.Empty;
                        //string tex = _TaxSlab_Model.hdntexper.ToString();
                        string tex = _TaxSlab_Model.hdntexper;
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }
                        string transType = _TaxSlab_Model.hdnAction;
                        string ds = _TaxSlab_ISERVICE.DeleteTaxSlab(CompID, tex,"", transType);
                        if(ds=="Delete")
                        {
                            _TaxSlab_Model.Message = "Delete";
                            _TaxSlab_Model.hdnSavebtn = "";
                        }
                        if (ds == "Duplicate")
                        {
                            _TaxSlab_Model.Message = "Duplicate";
                            _TaxSlab_Model.hdnSavebtn = "";
                        }
                        if (ds == "Used")
                        {                            
                            _TaxSlab_Model.Message = "Used";
                            _TaxSlab_Model.hdnSavebtn = "";
                        }
                        if (ds == "UsedInSturcture")
                        {
                            _TaxSlab_Model.Message = "Used";
                            _TaxSlab_Model.hdnSavebtn = "";
                        }
                        TempData["ModelData"] = _TaxSlab_Model;
                        return RedirectToAction("TaxSlab");
                    case "DeleteHSN":
                         CompID = string.Empty;
                         string texhsn = _TaxSlab_Model.hdnlistTaxHsn.ToString();
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }
                         transType = _TaxSlab_Model.hdnAction;
                        string ds1 = _TaxSlab_ISERVICE.DeleteTaxSlab(CompID, "", texhsn, transType);
                        if (ds1 == "DeleteHSN")
                        {
                            _TaxSlab_Model.Message = "Delete";
                            _TaxSlab_Model.hdnSavebtn = "";
                        }
                        TempData["ModelData"] = _TaxSlab_Model;
                        return RedirectToAction("TaxSlab");
                    case "TaxPerSave":
                        //Session["Command"] = command;
                        SaveTaxslab(_TaxSlab_Model, command);
                        _TaxSlab_Model.hdnSavebtn = "";
                        TempData["ModelData"] = _TaxSlab_Model;
                        return RedirectToAction("TaxSlab");

                    case "Save":
                       // Session["Command"] = command;
                        SaveHsnDetails(_TaxSlab_Model);
                        _TaxSlab_Model.hdnSavebtn = "";
                        TempData["ModelData"] = _TaxSlab_Model;
                        return RedirectToAction("TaxSlab");
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
        public ActionResult SaveHsnDetails(TaxSlab_Model _TaxSlab_Model)
        {
            try
            {
                var transtype = "Save";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                var listTaxPer = _TaxSlab_Model.listTaxPer.ToString();
                var HSN_Number = _TaxSlab_Model.HSN_Number.ToString();
                string ds = _TaxSlab_ISERVICE.InsertHsnDetails(CompID, listTaxPer, HSN_Number, transtype);

                if (ds == "Save")
                {
                    _TaxSlab_Model.Message = "Save";
                }
                return RedirectToAction("TaxSlab");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult SaveTaxslab(TaxSlab_Model _TaxSlab_Model, string command)
        {
            try
            {
                var goods = "";
                var services = "";
                var transtype = "Save";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (_TaxSlab_Model.goods != false)
                {
                     goods = "Y";
                }
                else
                {
                     goods = "N";
                }
                if (_TaxSlab_Model.services != false)
                {
                    services = "Y";
                }
                else
                {
                    services = "N";
                }
                var taxper = _TaxSlab_Model.tax_per.ToString();
                string ds = _TaxSlab_ISERVICE.InsertTaxSlabDetail(CompID, taxper, goods, services,transtype);

                if (ds == "Save")
                {
                    _TaxSlab_Model.Message = "Save";
                }
                if(ds== "Duplicate")
                {
                    _TaxSlab_Model.Message = "Duplicate";
                }
                return RedirectToAction("TaxSlab",_TaxSlab_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }      
        }
    }
}