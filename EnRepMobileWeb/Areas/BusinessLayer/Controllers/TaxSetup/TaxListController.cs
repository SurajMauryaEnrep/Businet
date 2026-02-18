using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.BusinessLayer.TaxDetail;
//***Modifyed by shubham maurya on 12-12-2022 12:10 remove all session and using Model to Transfer Data***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class TaxListController : Controller
    {
        DataTable TaxListDs;
        string CompID, language = String.Empty;
        string DocumentMenuId = "103155",title;
        Common_IServices _Common_IServices;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        TaxList_ISERVICES _TaxList_ISERVICES;
        public TaxListController(Common_IServices _Common_IServices,TaxList_ISERVICES _TaxList_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._TaxList_ISERVICES = _TaxList_ISERVICES;
        }
        // GET: BusinessLayer/TaxList
        public ActionResult TaxList()
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                TaxDetailModel _TaxDetailModel = new TaxDetailModel();
                _TaxDetailModel.Title = title;
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
                GetAllData(_TaxDetailModel);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ItemListFilter = TempData["ListFilterData"].ToString();
                    if (ItemListFilter != "" && ItemListFilter != null)
                    {
                        _TaxDetailModel.ListFilterData = ItemListFilter;
                        var a = ItemListFilter.Split(',');
                        var TaxID = a[0].Trim();
                        var ActStatus = a[1].Trim();
                        var Taxtype = a[2].Trim();
                        DataTable HoCompData = _TaxList_ISERVICES.GetTaxListFilterDAL(Comp_ID, TaxID, ActStatus, Taxtype).Tables[0];
                        ViewBag.VBTaxlist = HoCompData;
                        _TaxDetailModel.TaxListDDL = TaxID;
                        _TaxDetailModel.tax_type = Taxtype;
                        _TaxDetailModel.tax_act_stat = ActStatus;
                    }
                    //else
                    //{
                    //    TaxListDs = new DataTable();
                    //    TaxListDs = _TaxList_ISERVICES.GetTaxListDAL(Comp_ID);
                    //    ViewBag.VBTaxList = TaxListDs;
                    //}
                }
                //else
                //{
                //    TaxListDs = new DataTable();
                //    TaxListDs = _TaxList_ISERVICES.GetTaxListDAL(Comp_ID);
                //    ViewBag.VBTaxList = TaxListDs;
                //}
                //Session["TaxSearch"] = "0";
                _TaxDetailModel.TaxSearch = "0";
                CommonPageDetails();
                return View("~/Areas/BusinessLayer/Views/TaxSetup/TaxList.cshtml", _TaxDetailModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void GetAllData(TaxDetailModel _TaxDetailModel)
        {
            string Br_ID = string.Empty;
            string UserID = string.Empty;
            string GroupName = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (string.IsNullOrEmpty(_TaxDetailModel.TaxListDDL))
            {
                GroupName = "0";
            }
            else
            {
                GroupName = _TaxDetailModel.TaxListDDL;
            }
          //  DataTable dt = _TaxList_ISERVICES.GetTaxNameList(GroupName, CompID);
            DataSet DataTable = _TaxList_ISERVICES.GetAllData(GroupName, CompID);

            List<TaxName> _TaxName = new List<TaxName>();
            DataTable dt = GetTaxNameList(_TaxDetailModel);
            foreach (DataRow dr in DataTable.Tables[0].Rows)
            {
                TaxName ddlTaxName = new TaxName();
                ddlTaxName.ID = dr["tax_id"].ToString();
                ddlTaxName.Name = dr["tax_name"].ToString();
                _TaxName.Add(ddlTaxName);
            }
            _TaxName.Insert(0, new TaxName() { ID = "0", Name = "All" });
            _TaxDetailModel.TaxList = _TaxName;

            ViewBag.VBTaxList = DataTable.Tables[1];
        }
        private void CommonPageDetails()
        {
            try
            {
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                string UserID = string.Empty;
                string language = string.Empty;
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
                    UserID = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
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
        private DataTable GetTaxNameList(TaxDetailModel _TaxDetailModel)
        {
            try
            {
                string GroupName = string.Empty;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_TaxDetailModel.TaxListDDL))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _TaxDetailModel.TaxListDDL;
                }
                DataTable dt = _TaxList_ISERVICES.GetTaxNameList(GroupName, Comp_ID);
                //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
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
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult TaxListFilter(string CompID, string TaxID, string ActStatus, string Taxtype)
        {
            TaxDetailModel _TaxDetailModel = new TaxDetailModel();
            ViewBag.VBCustomerList = null;
            string Comp_ID = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            try
            {
                DataTable HoCompData = _TaxList_ISERVICES.GetTaxListFilterDAL(Comp_ID, TaxID, ActStatus, Taxtype).Tables[0];
                ViewBag.VBTaxlist = HoCompData;
                //Session["TaxSearch"] = "Tax_Search";
                _TaxDetailModel.TaxSearch = "Tax_Search";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

            }
            return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialTaxList.cshtml", _TaxDetailModel);
        }
        public ActionResult GetAutoCompleteTaxList(SearchSupp queryParameters)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string GroupName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> TaxList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ddlGroup;
                }
                TaxList = _TaxList_ISERVICES.TaxSetupGroupDAL(GroupName, Comp_ID);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(TaxList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
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
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult AddNewTax()
        {
            TaxDetailModel _TaxDetailModel = new TaxDetailModel();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";

            _TaxDetailModel.Message = "New";
            _TaxDetailModel.Command = "Add";
            _TaxDetailModel.AppStatus = "D";
            _TaxDetailModel.TransType = "Save";
            _TaxDetailModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = _TaxDetailModel;
            TempData["ListFilterData"] = null;
            return RedirectToAction("TaxDetail", "TaxDetail");
        }
        public ActionResult EditTax(string TaxId,string ActSts,string manual_calc,string recov,string ListFilterData)
        {
            try
            {
                TaxDetailModel _TaxDetailModel = new TaxDetailModel();
                //Session["Message"] = "New";
                //Session["Command"] = "Add";
                //Session["TaxCode"] = TaxId;
                //Session["TransType"] = "Update";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnToDetailPage";

                _TaxDetailModel.Message = "New";
                _TaxDetailModel.Command = "Add";
                _TaxDetailModel.TaxCode = Convert.ToInt32(TaxId);
                _TaxDetailModel.AppStatus = "D";
                _TaxDetailModel.TransType = "Update";
                _TaxDetailModel.BtnName = "BtnToDetailPage";
                if (ActSts != "N")
                {
                    _TaxDetailModel.act_status = true;
                }
                else
                {
                    _TaxDetailModel.act_status = false;
                }
                if (manual_calc != "N")
                {
                    _TaxDetailModel.manual_calc = true;
                }
                else
                {
                    _TaxDetailModel.manual_calc = false;
                }
                if (recov != "N")
                {
                    _TaxDetailModel.recov = true;
                }
                else
                {
                    _TaxDetailModel.recov = false;
                }
                var TaxCodeURL = Convert.ToInt32(TaxId);
                var TransType = "Update";
                var BtnName = "BtnToDetailPage";
                var command = "Add";
                TempData["ModelData"] = _TaxDetailModel;
                TempData["ListFilterData"] = ListFilterData;
                //TaxDetailModel _TaxDetailM = new TaxDetailModel();
                //_TaxDetailM.BtnName= "BtnToDetailPage";
                //_TaxDetailM.TaxCode = Convert.ToInt32(TaxId);
                //_TaxDetailM.TransType = "Update";
                return (RedirectToAction("TaxDetail", "TaxDetail", new { TaxCodeURL = TaxCodeURL, TransType, BtnName, command }));
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
    }
}