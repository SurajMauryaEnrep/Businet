using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.InterBranchPurchase;

using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.InterBranchPurchase;
using System.Data;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.InterBranchPurchase
{
    public class InterBranchPurchaseController : Controller
    {
        string CompID, language = String.Empty;
        string DocumentMenuId = "105101153", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        InterBranchPurchase_Model _model=new InterBranchPurchase_Model();
        InterBranchPurchase_IService _InterBranchPurchase_IService;
        public InterBranchPurchaseController(Common_IServices _Common_IServices,, InterBranchPurchase_IService _InterBranchPurchase_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._InterBranchPurchase_IService = _InterBranchPurchase_IService;
        }
        // GET: ApplicationLayer/InterBranchPurchase
        public ActionResult InterBranchPurchase()
        {
            try
            {
                CommonPageDetails();
                IBPListModel _DPIListModel = new IBPListModel();
                if (wfStatus != null)
                {
                    _DPIListModel.wfstatus = wfStatus;
                    _DPIListModel.ListFilterData = "0,0,0,0" + "," + wfStatus;
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                DateTime dtnow = DateTime.Now;
                string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                if (_DPIListModel.DPI_FromDate == null)
                {
                    _DPIListModel.FromDate = startDate;
                }
                else
                {
                    _DPIListModel.FromDate = _DPIListModel.DPI_FromDate;
                }
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _DPIListModel.StatusList = statusLists;
                if (TempData["UrlData"] != null)
                {
                    if (TempData["UrlData"].ToString() != "")
                    {
                        UrlData urlData = TempData["UrlData"] as UrlData;
                        if (urlData.ListFilterData1 != null)
                        {
                            var arr = urlData.ListFilterData1.Split(',');
                            _DPIListModel.SuppID = arr[0];
                            _DPIListModel.FromDate = arr[1];
                            _DPIListModel.ToDate = arr[2];
                            _DPIListModel.Status = arr[3];
                            if (wfStatus == null)
                            {
                                wfStatus = arr[3];
                            }
                            _DPIListModel.ListFilterData = _DPIListModel.SuppID + "," + _DPIListModel.FromDate + "," + _DPIListModel.ToDate + "," + _DPIListModel.Status + "," + wfStatus;
                        }

                    }
                }
                GetAllData(_DPIListModel);
                _DPIListModel.Title = title;
                return View("~/Areas/ApplicationLayer/Views/Procurement/InterBranchPurchase/InterBranchPurchaseList.cshtml");
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
        public ActionResult AddInterBranchPurchaseDetail()
        {
            Session["Message"] = "New";
            Session["Command"] = "Add";
            Session["AppStatus"] = 'D';
            Session["TransType"] = "Save";
            Session["BtnName"] = "BtnAddNew";
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("InterBranchPurchaseDetail", "InterBranchPurchase");
        }
        public ActionResult MaterialReturnSCDetail()
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/Procurement/InterBranchPurchase/InterBranchPurchaseDetail.cshtml");
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
    }

}