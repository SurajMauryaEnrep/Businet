using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSalesQuotation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSalesQuotation;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using Newtonsoft.Json;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.DomesticSalesQuotation
{
    public class DomesticSalesQuotationListController : Controller
    {
        List<DomesticSalesQuotationList> _DomesticSalesQtList;
        string FromDate,title, crm = "Y";
        string DocumentMenuId = ""; 
        Common_IServices _Common_IServices;
        string CompID, UserID, language, BrchID = string.Empty;
       
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();

        DomesticSalesQuotationList_ISERVICES _DomesticSalesQuotationList_ISERVICES;        
        public DomesticSalesQuotationListController(Common_IServices _Common_IServices,DomesticSalesQuotationList_ISERVICES _DomesticSalesQuotationList_ISERVICES)
        {
            this._DomesticSalesQuotationList_ISERVICES = _DomesticSalesQuotationList_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }

        // GET: ApplicationLayer/DomesticSalesQuotationList
        public ActionResult DomesticSalesQuotationDList(DomesticSalesQuotationListModel _DomesticSalesQuotationListModel)
        {
            try
            {
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103120")
                //    {
                //DomesticSalesQuotationListModel _DomesticSalesQuotationListModel = new DomesticSalesQuotationListModel();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["crm"] != null)
                {
                    crm = Session["crm"].ToString();
                }
                if (UserID == "1001")
                {
                    crm = "Y";
                }
                ViewBag.crm = crm;
                DocumentMenuId = "105103120";
                var CustType = "D";
                CommonPageDetails();
                _DomesticSalesQuotationListModel.DocumentMenuId = DocumentMenuId;
                _DomesticSalesQuotationListModel.CustType = CustType;
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145105")
                //    {
                //        DocumentMenuId = "105103145105";
                //    }
                //}

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;


                //string endDate = dtnow.ToString("yyyy-MM-dd");
                //DomesticSalesQuotationListModel _DomesticSalesQuotationListModel = new DomesticSalesQuotationListModel();
                //  GetAutoCompleteSearchCustList(_DomesticSalesQuotationListModel, CustType);

                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _DomesticSalesQuotationListModel.QT_CustID = a[0].Trim();
                    _DomesticSalesQuotationListModel.QT_FromDate = a[1].Trim();
                    _DomesticSalesQuotationListModel.QT_ToDate = a[2].Trim();
                    _DomesticSalesQuotationListModel.QT_Status = a[3].Trim();
                    _DomesticSalesQuotationListModel.sls_per = a[4].Trim();
                    if (_DomesticSalesQuotationListModel.QT_Status == "0")
                    {
                        _DomesticSalesQuotationListModel.QT_Status = null;
                    }
                    _DomesticSalesQuotationListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    //_DPOListModel.ToDate =Convert.ToDateTime(_DPOListModel.PO_ToDate);
                }
              //  _DomesticSalesQuotationListModel.DomesticQuotList = getQuotationList(_DomesticSalesQuotationListModel, CustType, wfstatus);
                if (_DomesticSalesQuotationListModel.QT_FromDate == null)
                {
                    _DomesticSalesQuotationListModel.FromDate = startDate;
                    _DomesticSalesQuotationListModel.QT_FromDate = startDate;
                    _DomesticSalesQuotationListModel.QT_ToDate = CurrentDate;
                }
                else
                {
                    _DomesticSalesQuotationListModel.FromDate = _DomesticSalesQuotationListModel.QT_FromDate;
                }
               // GetStatusList(_DomesticSalesQuotationListModel);
                List<Status> statusLists = new List<Status>();
                if (ViewBag.StatusList.Rows.Count > 0)
                {
                    foreach (DataRow data in ViewBag.StatusList.Rows)
                    {
                        Status _Statuslist = new Status();
                        _Statuslist.status_id = data["status_code"].ToString();
                        _Statuslist.status_name = data["status_name"].ToString();
                        statusLists.Add(_Statuslist);
                    }
                }
                statusLists.Insert(0, new Status() { status_id = "0", status_name = "All" });
                _DomesticSalesQuotationListModel.StatusList = statusLists;
                //ViewBag.MenuPageName = getDocumentName();
                _DomesticSalesQuotationListModel.Title = title;
                //Session["DSQSearch"] = "0";
                _DomesticSalesQuotationListModel.DSQSearch = "0";
                //ViewBag.VBRoleList = GetRoleList();
                //if (Session["CompId"] != null)
                //{
                //    CompID = Session["CompId"].ToString();
                //}
                //if (Session["BranchId"] != null)
                //{
                //    BrchID = Session["BranchId"].ToString();
                //}
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                //ViewBag.DocumentMenuId = DocumentMenuId;
                GetAllData(_DomesticSalesQuotationListModel, "D");
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticSalesQuotation/DomesticSalesQuotationList.cshtml", _DomesticSalesQuotationListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(DomesticSalesQuotationListModel _DomesticSalesQuotationListModel,string CustType)
        {
            string CustomerName = string.Empty;
          
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;         
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                string UserID = Session["UserID"].ToString();
                if (string.IsNullOrEmpty(_DomesticSalesQuotationListModel.QT_CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _DomesticSalesQuotationListModel.QT_CustName;
                }
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _DomesticSalesQuotationListModel.WF_status = TempData["WF_status"].ToString();
                    if (_DomesticSalesQuotationListModel.WF_status != null)
                    {
                        wfstatus = _DomesticSalesQuotationListModel.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                else
                {
                    if (_DomesticSalesQuotationListModel.WF_status != null)
                    {
                        wfstatus = _DomesticSalesQuotationListModel.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                string SalesPersonName = string.Empty;
                if (string.IsNullOrEmpty(_DomesticSalesQuotationListModel.SQ_SalePerson))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = _DomesticSalesQuotationListModel.SQ_SalePerson;
                }
                DataSet  AllData = _DomesticSalesQuotationList_ISERVICES.GetAllData(Comp_ID, CustomerName, Br_ID, CustType,_DomesticSalesQuotationListModel,  UserID,  wfstatus,  DocumentMenuId, CustType, SalesPersonName, _DomesticSalesQuotationListModel.sls_per);

                List<CustomerName> _CustList = new List<CustomerName>();
                foreach (DataRow data in AllData.Tables[0].Rows)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.cust_id = data["cust_id"].ToString();
                    _CustDetail.cust_name = data["cust_name"].ToString();
                    _CustList.Add(_CustDetail);
                }
                _CustList.Insert(0,new CustomerName() { cust_id="0",cust_name="All"});
                _DomesticSalesQuotationListModel.CustomerNameList = _CustList;

                List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                    foreach (DataRow data in AllData.Tables[4].Rows)
                    {
                    SalePersonList _SlPrsnDetail = new SalePersonList();
                        _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                        _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                        _SlPrsnList.Add(_SlPrsnDetail);
                    }
                    _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                _DomesticSalesQuotationListModel.SalePersonList = _SlPrsnList;
                SetAllData(AllData, _DomesticSalesQuotationListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void SetAllData(DataSet dt, DomesticSalesQuotationListModel _DomesticSalesQuotationListModel)
        {
            try
            {
                //string QtType = string.Empty;
                List<DomesticSalesQuotationList> _DomesticSalesQtList = new List<DomesticSalesQuotationList>();
                if (dt.Tables[2].Rows.Count > 0)
                {
                    //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Tables[1].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[1].Rows)
                    {
                        DomesticSalesQuotationList _DomesticSalesQuotationList = new DomesticSalesQuotationList();
                        _DomesticSalesQuotationList.QuotationNo = dr["QtNo"].ToString();
                        _DomesticSalesQuotationList.rev_no = dr["rev_no"].ToString();
                        _DomesticSalesQuotationList.hdnQuotationNo = dr["Doc_no"].ToString();
                        _DomesticSalesQuotationList.QuotationDate = dr["QtDate"].ToString();
                        _DomesticSalesQuotationList.SalesPerson = dr["SalesPerson"].ToString();
                        _DomesticSalesQuotationList.Cust_type = dr["CustType"].ToString();
                        _DomesticSalesQuotationList.cust_name = dr["cust_name"].ToString();
                        _DomesticSalesQuotationList.curr_logo = dr["curr_logo"].ToString();
                        _DomesticSalesQuotationList.net_val_bs = dr["net_val_bs"].ToString();
                        _DomesticSalesQuotationList.QTStauts = dr["QtStauts"].ToString();
                        _DomesticSalesQuotationList.CreateDate = dr["CreateDate"].ToString();
                        _DomesticSalesQuotationList.ApproveDate = dr["ApproveDate"].ToString();
                        _DomesticSalesQuotationList.ModifyDate = dr["ModifyDate"].ToString();
                        _DomesticSalesQuotationList.create_by = dr["create_by"].ToString();
                        _DomesticSalesQuotationList.app_by = dr["app_by"].ToString();
                        _DomesticSalesQuotationList.mod_by = dr["mod_by"].ToString();
                        _DomesticSalesQuotationList.Amendment = dr["flag"].ToString();
                        _DomesticSalesQtList.Add(_DomesticSalesQuotationList);
                    }
                }
                _DomesticSalesQuotationListModel.DomesticQuotList = _DomesticSalesQtList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult DomesticSalesQuotationEList(DomesticSalesQuotationListModel _DomesticSalesQuotationListModel)
        {
            try
            {
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103120")
                //    {
                //        DocumentMenuId = "105103120";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145105")
                //    {
                //DomesticSalesQuotationListModel _DomesticSalesQuotationListModel = new DomesticSalesQuotationListModel();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["crm"] != null)
                {
                    crm = Session["crm"].ToString();
                }
                if (UserID == "1001")
                {
                    crm = "Y";
                }
                ViewBag.crm = crm;
                DocumentMenuId = "105103145105";
                var CustType = "E";
                CommonPageDetails();
                _DomesticSalesQuotationListModel.DocumentMenuId = DocumentMenuId;
                _DomesticSalesQuotationListModel.CustType = CustType;
                //    }
                //}
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _DomesticSalesQuotationListModel.WF_status = TempData["WF_status"].ToString();
                    if (_DomesticSalesQuotationListModel.WF_status != null)
                    {
                        wfstatus = _DomesticSalesQuotationListModel.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                else
                {
                    if (_DomesticSalesQuotationListModel.WF_status != null)
                    {
                        wfstatus = _DomesticSalesQuotationListModel.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                //string endDate = dtnow.ToString("yyyy-MM-dd");                
                //GetAutoCompleteSearchCustList(_DomesticSalesQuotationListModel, CustType);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _DomesticSalesQuotationListModel.QT_CustID = a[0].Trim();
                    _DomesticSalesQuotationListModel.QT_FromDate = a[1].Trim();
                    _DomesticSalesQuotationListModel.QT_ToDate = a[2].Trim();
                    _DomesticSalesQuotationListModel.QT_Status = a[3].Trim();
                    if (_DomesticSalesQuotationListModel.QT_Status == "0")
                    {
                        _DomesticSalesQuotationListModel.QT_Status = null;
                    }
                    _DomesticSalesQuotationListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    //_DPOListModel.ToDate =Convert.ToDateTime(_DPOListModel.PO_ToDate);
                }
             //   _DomesticSalesQuotationListModel.DomesticQuotList = getQuotationList(_DomesticSalesQuotationListModel, CustType, wfstatus);
                if (_DomesticSalesQuotationListModel.QT_FromDate == null)
                {
                    _DomesticSalesQuotationListModel.FromDate = startDate;
                    _DomesticSalesQuotationListModel.QT_FromDate = startDate;
                    _DomesticSalesQuotationListModel.QT_ToDate = CurrentDate;
                }
                else
                {
                    _DomesticSalesQuotationListModel.FromDate = _DomesticSalesQuotationListModel.QT_FromDate;
                }
                //   GetStatusList(_DomesticSalesQuotationListModel);
                List<Status> statusLists = new List<Status>();
                if (ViewBag.StatusList.Rows.Count > 0)
                {
                    foreach (DataRow data in ViewBag.StatusList.Rows)
                    {
                        Status _Statuslist = new Status();
                        _Statuslist.status_id = data["status_code"].ToString();
                        _Statuslist.status_name = data["status_name"].ToString();
                        statusLists.Add(_Statuslist);
                    }
                }
                statusLists.Insert(0, new Status() { status_id = "0", status_name = "All" });
                _DomesticSalesQuotationListModel.StatusList = statusLists;
                //ViewBag.MenuPageName = getDocumentName();
                _DomesticSalesQuotationListModel.Title = title;
                //Session["DSQSearch"] = "0";
                _DomesticSalesQuotationListModel.DSQSearch = "0";
                //ViewBag.VBRoleList = GetRoleList();
                //if (Session["CompId"] != null)
                //{
                //    CompID = Session["CompId"].ToString();
                //}
                //if (Session["BranchId"] != null)
                //{
                //    BrchID = Session["BranchId"].ToString();
                //}
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                //ViewBag.DocumentMenuId = DocumentMenuId;
                GetAllData(_DomesticSalesQuotationListModel, "E");
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticSalesQuotation/DomesticSalesQuotationList.cshtml", _DomesticSalesQuotationListModel);
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
        public ActionResult GetDomesticSalesQuotationList(string docid, string status)
        {
            //Session["MenuDocumentId"] = docid;
            DomesticSalesQuotationListModel _DomesticSalesQuotationListModel = new DomesticSalesQuotationListModel();
            if (docid == "105103120")
            {
                _DomesticSalesQuotationListModel.WF_status = status;
                return RedirectToAction("DomesticSalesQuotationDList", _DomesticSalesQuotationListModel);
            }
            else
            {
                _DomesticSalesQuotationListModel.WF_status = status;
                return RedirectToAction("DomesticSalesQuotationEList", _DomesticSalesQuotationListModel);
            }
        }
        public void GetStatusList(DomesticSalesQuotationListModel _DomesticSalesQuotationListModel)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _DomesticSalesQuotationListModel.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        //private DataTable GetRoleList()
        //{
        //    try
        //    {
        //        string UserID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            UserID = Session["userid"].ToString();
        //        }
        //        DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);
        //        return RoleList;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
        public ActionResult GetAutoCompleteSearchCustList(DomesticSalesQuotationListModel _DomesticSalesQuotationListModel,string CustType)
        {
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            //string CustType = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103120")
                //    {
                //        CustType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145105")
                //    {
                //        CustType = "E";
                //    }
                //}
                if (string.IsNullOrEmpty(_DomesticSalesQuotationListModel.QT_CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _DomesticSalesQuotationListModel.QT_CustName;
                }
                CustList = _DomesticSalesQuotationList_ISERVICES.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustType);

                List<CustomerName> _CustList = new List<CustomerName>();
                foreach (var data in CustList)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.cust_id = data.Key;
                    _CustDetail.cust_name = data.Value;
                    _CustList.Add(_CustDetail);
                }
                _DomesticSalesQuotationListModel.CustomerNameList = _CustList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ErrorPage()
        {
            try
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public ActionResult AddNewQuot(string DocumentMenuId,string CustType)
        {
            URLDetailModel URLModel = new URLDetailModel();
            DomesticSalesQuotationModel _DomesticSalesQuotationModel = new DomesticSalesQuotationModel();
            _DomesticSalesQuotationModel.Message = "New";
            _DomesticSalesQuotationModel.Command = "New";
            _DomesticSalesQuotationModel.DocumentStatus = "D";
            _DomesticSalesQuotationModel.TransType = "Save";
            _DomesticSalesQuotationModel.BtnName = "BtnAddNew";
            _DomesticSalesQuotationModel.DocumentMenuId = DocumentMenuId;
            _DomesticSalesQuotationModel.CustType = CustType;
            TempData["ModelData"] = _DomesticSalesQuotationModel;
            URLModel.DocumentMenuId = DocumentMenuId;
            URLModel.TransType = "Save";
            URLModel.BtnName = "BtnAddNew";
            URLModel.Command = "New";
            URLModel.DocDate = null;
            URLModel.DocNo = null;
            URLModel.CustType = CustType;
            //Session.Remove("ProspectFromQuot");
            //Session["Message"] = "New";
            //Session["Command"] = "New";
            //Session["DocumentStatus"] = "D";
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session.Remove("ProspectFromQuot");
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
               if( _DomesticSalesQuotationModel.DocumentMenuId=="105103120")
                {
                    return RedirectToAction("DomesticSalesQuotationDList");
                }
                else
                {
                    return RedirectToAction("DomesticSalesQuotationEList");
                }
                
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("DomesticSalesQuotationDetail", "DomesticSalesQuotation", URLModel);
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
        private List<DomesticSalesQuotationList> getQuotationList(DomesticSalesQuotationListModel _DomesticSalesQuotationListModel,string QtType, string wfstatus)
        {
            try
            {
                //string QtType = string.Empty;
                _DomesticSalesQtList = new List<DomesticSalesQuotationList>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103120")
                //    {
                //        QtType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145105")
                //    {
                //        QtType = "E";
                //    }
                //}
                string UserID = Session["UserID"].ToString();
                //string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                //else
                //{
                //    wfstatus = "";
                //}
                
                DataSet dt = new DataSet();
                dt = _DomesticSalesQuotationList_ISERVICES.GetQTDetailListDAL(CompID, BrchID, _DomesticSalesQuotationListModel, UserID, wfstatus, DocumentMenuId, QtType);
                if (dt.Tables[1].Rows.Count > 0)
                {
                    //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                    if (dt.Tables[0].Rows.Count > 0)
                {
                    
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        DomesticSalesQuotationList _DomesticSalesQuotationList = new DomesticSalesQuotationList();
                        _DomesticSalesQuotationList.QuotationNo = dr["QtNo"].ToString();
                        _DomesticSalesQuotationList.rev_no = dr["rev_no"].ToString();
                        _DomesticSalesQuotationList.hdnQuotationNo = dr["Doc_no"].ToString();
                        _DomesticSalesQuotationList.QuotationDate = dr["QtDate"].ToString();
                        _DomesticSalesQuotationList.SalesPerson = dr["SalesPerson"].ToString();
                        _DomesticSalesQuotationList.Cust_type = dr["CustType"].ToString();
                        _DomesticSalesQuotationList.cust_name = dr["cust_name"].ToString();
                        _DomesticSalesQuotationList.curr_logo = dr["curr_logo"].ToString();
                        _DomesticSalesQuotationList.net_val_bs = dr["net_val_bs"].ToString();
                        _DomesticSalesQuotationList.QTStauts = dr["QtStauts"].ToString();
                        _DomesticSalesQuotationList.CreateDate = dr["CreateDate"].ToString();
                        _DomesticSalesQuotationList.ApproveDate = dr["ApproveDate"].ToString();
                        _DomesticSalesQuotationList.ModifyDate = dr["ModifyDate"].ToString();
                        _DomesticSalesQuotationList.create_by = dr["create_by"].ToString();
                        _DomesticSalesQuotationList.app_by = dr["app_by"].ToString();
                        _DomesticSalesQuotationList.mod_by = dr["mod_by"].ToString();
                        _DomesticSalesQuotationList.Amendment = dr["flag"].ToString();
                        _DomesticSalesQtList.Add(_DomesticSalesQuotationList);
                    }
                }
                //Session["FinStDt"] = dt.Tables[2].Rows[0]["findate"];
                return _DomesticSalesQtList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult SearchQuotDetail(string CustId, string Fromdate, string Todate, string Status,string QtType,string sales_person)
        {
            try
            {
                DomesticSalesQuotationListModel _DomesticSalesQuotationListModel = new DomesticSalesQuotationListModel();
                //Session.Remove("WF_status");
                //string QtType = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103120")
                //    {
                //        QtType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145105")
                //    {
                //        QtType = "E";
                //    }
                //}
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                _DomesticSalesQtList = new List<DomesticSalesQuotationList>();
                //DomesticSalesQuotationListModel _DomesticSalesQuotationListModel = new DomesticSalesQuotationListModel();
                _DomesticSalesQuotationListModel.QT_CustID = CustId;
                _DomesticSalesQuotationListModel.QT_FromDate = Fromdate;
                _DomesticSalesQuotationListModel.QT_ToDate = Todate;
                _DomesticSalesQuotationListModel.QT_Status = Status;
                _DomesticSalesQuotationListModel.SQ_SalePerson = sales_person;
                DataSet dt= new DataSet();
                dt = _DomesticSalesQuotationList_ISERVICES.GetQTDetailListDAL(CompID, BrchID, _DomesticSalesQuotationListModel, UserID, "", "", QtType);

                //Session["DSQSearch"] = "DSQ_Search";
                _DomesticSalesQuotationListModel.DSQSearch = "DSQ_Search";

                //Session["FinStDt"] = dt.Tables[2].Rows[0]["findate"];
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    DomesticSalesQuotationList _DomesticSalesQuotationList = new DomesticSalesQuotationList();
                    _DomesticSalesQuotationList.QuotationNo = dr["QtNo"].ToString();
                    _DomesticSalesQuotationList.rev_no = dr["rev_no"].ToString();
                    _DomesticSalesQuotationList.hdnQuotationNo = dr["Doc_no"].ToString();
                    _DomesticSalesQuotationList.QuotationDate = dr["QtDate"].ToString();
                    _DomesticSalesQuotationList.SalesPerson = dr["SalesPerson"].ToString();
                    _DomesticSalesQuotationList.Cust_type = dr["CustType"].ToString();
                    _DomesticSalesQuotationList.cust_name = dr["cust_name"].ToString();
                    _DomesticSalesQuotationList.curr_logo = dr["curr_logo"].ToString();
                    _DomesticSalesQuotationList.net_val_bs = dr["net_val_bs"].ToString();
                    _DomesticSalesQuotationList.QTStauts = dr["QtStauts"].ToString();
                    _DomesticSalesQuotationList.CreateDate = dr["CreateDate"].ToString();
                    _DomesticSalesQuotationList.ApproveDate = dr["ApproveDate"].ToString();
                    _DomesticSalesQuotationList.ModifyDate = dr["ModifyDate"].ToString();
                    _DomesticSalesQuotationList.create_by = dr["create_by"].ToString();
                    _DomesticSalesQuotationList.app_by = dr["app_by"].ToString();
                    _DomesticSalesQuotationList.mod_by = dr["mod_by"].ToString();
                    _DomesticSalesQuotationList.Amendment = dr["flag"].ToString();
                    _DomesticSalesQtList.Add(_DomesticSalesQuotationList);
                }
                _DomesticSalesQuotationListModel.DomesticQuotList = _DomesticSalesQtList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialQuotationList.cshtml", _DomesticSalesQuotationListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public ActionResult GetQTTrackingDetail(string QT_No, string QT_Date)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string BranchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataSet result = _DomesticSalesQuotationList_ISERVICES.GetQT_TrackingDetail(Comp_ID, BranchID, QT_No, QT_Date);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.QTTrackingDetails = result.Tables[0];
                ViewBag.trackingSSISearch = "trackingSSISearch";
                return View("~/Areas/ApplicationLayer/Views/Shared/_QuotationTracking.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
    }

}