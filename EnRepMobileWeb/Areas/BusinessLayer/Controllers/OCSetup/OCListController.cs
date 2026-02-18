using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.BusinessLayer.OCDetail;
//Modifyed by Shubham Maurya on 14-12-2022 Removed All Session//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class OCListController : Controller
    {
        DataTable OCListDs;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string CompID, branchID, user_id, language = String.Empty;
        string DocumentMenuId = "103195",title;
        Common_IServices _Common_IServices;
        OCList_ISERVICES _OCList_ISERVICES;
        public OCListController(Common_IServices _Common_IServices,OCList_ISERVICES _OCList_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._OCList_ISERVICES = _OCList_ISERVICES;
        }
        // GET: BusinessLayer/OCList
        public ActionResult OCList()
        {
            try
            {
                CommonPageDetails();
                OCDetailModel _OCDetail = new OCDetailModel();
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
                DataSet dt2 = GetOCList(_OCDetail);
                List<OTCharge> _OTCharge = new List<OTCharge>();
                foreach (DataRow dr in dt2.Tables[0].Rows)
                {
                    OTCharge ddlOTCharge = new OTCharge();
                    ddlOTCharge.OT_id = dr["oc_id"].ToString();
                    ddlOTCharge.OT_Val = dr["oc_name"].ToString();
                    _OTCharge.Add(ddlOTCharge);
                }
                //_OTCharge.Insert(0, new OTCharge() { OT_id = "0", OT_Val = "All" });
                _OCDetail.OTCLists = _OTCharge;
                /**********************************Bind HSn ********************************************/

                List<HSNno> _HSNList = new List<HSNno>();
                foreach (DataRow dr in dt2.Tables[1].Rows)
                {
                    HSNno _HsnDetail = new HSNno();
                    _HsnDetail.setup_id = dr["setup_id"].ToString();
                    _HsnDetail.setup_val = dr["setup_val"].ToString();
                    _HSNList.Add(_HsnDetail);
                }
                _HSNList.Insert(0, new HSNno() { setup_id = "0", setup_val = "All" });
                _OCDetail.HSNList = _HSNList;

                /****************************************End**************************************************/
              
                if (TempData["ListFilterData"] != null)
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    if (PRData != null && PRData != "")
                    {


                        var a = PRData.Split(',');
                        var OCID = a[0].Trim();
                        var ActStatus = a[1].Trim();
                        var OCtype = a[2].Trim();
                        var Hsn_ID = a[3].Trim();
                        DataTable HoCompData = _OCList_ISERVICES.GetOCListFilterDAL(Comp_ID, OCID, ActStatus, OCtype, Hsn_ID).Tables[0];
                        ViewBag.VBOClist = HoCompData;
                        //Session["OCSearch"] = "OC_Search";
                        _OCDetail.ListFilterData = TempData["ListFilterData"].ToString();

                        _OCDetail.OTC = OCID;
                        _OCDetail.item_ActStatus = ActStatus;
                        _OCDetail.OTCType = OCtype;
                        _OCDetail.HSN_code = Hsn_ID;

                    }
                }

                else
                {
                    OCListDs = new DataTable();
                    OCListDs = _OCList_ISERVICES.GetOCListDAL(Comp_ID);
                    ViewBag.VBOCList = OCListDs;

                }
                   
                //ViewBag.MenuPageName = getDocumentName();
                _OCDetail.Title = title;
                //Session["OCSearch"] = "0";
                _OCDetail.OCSearch = "0";
                return View("~/Areas/BusinessLayer/Views/OCSetup/OCList.cshtml", _OCDetail);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
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
        [NonAction]
        private DataSet GetOCList(OCDetailModel _OCDetail)
        {
            try
            {
                string GroupName = string.Empty;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_OCDetail.OTC))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _OCDetail.OTC;
                }
                DataSet dt = _OCList_ISERVICES.GetOTClist(GroupName, Comp_ID);
                return dt;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
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
        //public ActionResult GetAutoCompleteOCList(SearchSupp queryParameters)
        //{
        //    string Comp_ID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }
        //    string GroupName = string.Empty;
        //    //string ErrorMessage = "success";
        //    Dictionary<string, string> OCList = new Dictionary<string, string>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(queryParameters.ddlGroup))
        //        {
        //            GroupName = "0";
        //        }
        //        else
        //        {
        //            GroupName = queryParameters.ddlGroup;
        //        }
        //        OCList = _OCList_ISERVICES.OCSetupGroupDAL(GroupName, Comp_ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(OCList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult OCListFilter(string OCID, string ActStatus, string OCtype,string Hsn_ID)
        {
            CommonPageDetails();
            OCDetailModel _OCDetail = new OCDetailModel();
            ViewBag.VBOCList = null;
            string Comp_ID = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            try
            {
                DataTable HoCompData = _OCList_ISERVICES.GetOCListFilterDAL(Comp_ID, OCID, ActStatus, OCtype, Hsn_ID).Tables[0];
                ViewBag.VBOClist = HoCompData;
                //Session["OCSearch"] = "OC_Search";
                _OCDetail.OCSearch = "OC_Search";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

            }
            return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialOCList.cshtml", _OCDetail);
        }
        public ActionResult ErrorPage()
        {
            try
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult AddNewOC()
        {
            OCDetailModel _OCDetail = new OCDetailModel();
            //@Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";

            _OCDetail.Message = "New";
            _OCDetail.Command = "Add";
            _OCDetail.AppStatus = "D";
            _OCDetail.TransType = "Save";
            _OCDetail.BtnName = "BtnAddNew";
            TempData["ModelData"] = _OCDetail;
            return RedirectToAction("OCDetail", "OCDetail");
        }
        public ActionResult EditOC(string OCId,string act_status,string TaxApplicable,string ListFilterData)
        {
            OCDetailModel _OCDetail = new OCDetailModel();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["OCCode"] = OCId;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";

            _OCDetail.Message = "New";
            _OCDetail.Command = "Add";
            _OCDetail.OCCode = OCId;
            _OCDetail.TransType = "Update";
            _OCDetail.AppStatus = "D";
            _OCDetail.BtnName = "BtnToDetailPage";
            if (act_status =="Y")
            {
                _OCDetail.act_status = true;
            }
            else
            {
                _OCDetail.act_status = false;
            }
            if (TaxApplicable == "Y")
            {
                _OCDetail.TaxApplicable = true;
            }
            else
            {
                _OCDetail.TaxApplicable = false;
            }
            TempData["ModelData"] = _OCDetail;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("OCDetail", "OCDetail", _OCDetail);
        }
    }
}