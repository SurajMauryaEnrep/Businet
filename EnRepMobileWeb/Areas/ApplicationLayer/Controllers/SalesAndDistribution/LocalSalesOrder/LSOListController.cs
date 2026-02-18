using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesAndDistributionIServices;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesAndDistributionModels;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution
{
    public class LSOListController : Controller
    {
        string FromDate, title, crm = "Y";
        List<LocalSalesOrder> _LocalSalesOrderList;
        string CompID, UserID, language, BrchID = string.Empty;
        //DataTable dt;
        string DocumentMenuId;
        Common_IServices _Common_IServices;
        //DataTable SOListDT;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();

        LSOList_ISERVICE _LSOList_ISERVICE;

        public LSOListController(Common_IServices _Common_IServices, LSOList_ISERVICE _LSOList_ISERVICE)
        {
            this._LSOList_ISERVICE = _LSOList_ISERVICE;
            this._Common_IServices = _Common_IServices;
        }

        // GET: ApplicationLayer/LSOList
        public ActionResult LSOList(LSOListModel _LSOList_Model)
        {
            try
            {

                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    Session["MenuDocumentId"] = "105103125";
                DocumentMenuId = "105103125";
                //if (Session["MenuDocumentId"].ToString() == "105103125")
                //{
                //    DocumentMenuId = "105103125";
                //}
                //}
                //LSOListModel _LSOList_Model = new LSOListModel();
                CommonPageDetails();
                var CustType = "D";
                //DateTime dtnow = DateTime.Now;

                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                //string endDate = dtnow.ToString("yyyy-MM-dd");

                _LSOList_Model.CustType = CustType;
                _LSOList_Model.MenuDocumentId = DocumentMenuId;
                _LSOList_Model.DocumentMenuId = DocumentMenuId;
                //GetAutoCompleteSearchCustList(_LSOList_Model);
               
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    _LSOList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _LSOList_Model.SO_CustID = a[0].Trim();
                    _LSOList_Model.SO_FromDate = a[1].Trim();
                    _LSOList_Model.SO_ToDate = a[2].Trim();
                    _LSOList_Model.SO_Status = a[3].Trim();
                    _LSOList_Model.SQ_SalePerson = a[4].Trim();
                    if (_LSOList_Model.SO_Status == "0")
                    {
                        _LSOList_Model.SO_Status = null;
                    }
                }
              //  _LSOList_Model.LocalSalesOrderList = getLSOList(_LSOList_Model, startDate);
                if (_LSOList_Model.SO_FromDate == null)
                {
                    _LSOList_Model.FromDate = startDate;
                    _LSOList_Model.SO_FromDate = startDate;
                    _LSOList_Model.SO_ToDate = CurrentDate;
                }
                else
                {
                    _LSOList_Model.FromDate = _LSOList_Model.SO_FromDate;
                }
                // GetStatusList(_LSOList_Model);
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
                else
                {
                    statusLists.Insert(0, new Status() { status_id = "0", status_name = "All" });
                }
                
                _LSOList_Model.StatusList = statusLists;

                GetAllData(_LSOList_Model);
             //   ViewBag.MenuPageName = getDocumentName(_LSOList_Model);
                _LSOList_Model.Title = title;
                //Session["LSOSearch"] = "0";
                _LSOList_Model.LSOSearch = "0";
              //  ViewBag.VBRoleList = GetRoleList();
                var other = new CommonController(_Common_IServices);
              //  ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                _LSOList_Model.DocumentMenuId = DocumentMenuId;
                if (Session["crm"] != null)
                {
                    crm = Session["crm"].ToString();
                }
                if (UserID == "1001")
                {
                    crm = "Y";
                }
                ViewBag.crm = crm;
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSalesOrder/LSOList.cshtml", _LSOList_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(LSOListModel _LSOListModel)/**Added By Nitesh 04-04-2024 for All data one Procedure**/
        {
            DateTime dtnow = DateTime.Now;
            string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string CustType = string.Empty;
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
                if (string.IsNullOrEmpty(_LSOListModel.SO_CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _LSOListModel.SO_CustName;
                }
                if (_LSOListModel.DocumentMenuId != null)
                {
                    if (_LSOListModel.DocumentMenuId == "105103125")
                    {
                        CustType = "D";
                    }
                    else if (_LSOListModel.DocumentMenuId == "105103145110")
                    {
                        CustType = "E";
                    }
                }


                string wfstatus = "";
                if (_LSOListModel.SO_FromDate == null)
                {
                    if (startDate != null)
                    {
                        _LSOListModel.SO_FromDate = startDate;
                    }
                }
                if (_LSOListModel.WF_status != null)
                {
                    wfstatus = _LSOListModel.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
               
                DataSet dt = new DataSet();

              DataSet  AllData = _LSOList_ISERVICE.GetAllData(Comp_ID, CustomerName, Br_ID, CustType
                  , _LSOListModel.SO_CustID, _LSOListModel.SO_FromDate, _LSOListModel.SO_ToDate, _LSOListModel.SO_Status, UserID, DocumentMenuId, wfstatus, "", _LSOListModel.SQ_SalePerson);

                List<CustomerName> _CustList = new List<CustomerName>();
                foreach (DataRow data in AllData.Tables[0].Rows)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.cust_id = data["cust_id"].ToString();
                    _CustDetail.cust_name = data["cust_name"].ToString();
                    _CustList.Add(_CustDetail);
                }
                _CustList.Insert(0,new CustomerName() { cust_id="0",cust_name="All"});
                _LSOListModel.CustomerNameList = _CustList;

                List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                foreach (DataRow data in AllData.Tables[4].Rows)
                {
                    SalePersonList _SlPrsnDetail = new SalePersonList();
                    _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                    _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                    _SlPrsnList.Add(_SlPrsnDetail);
                }
                _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                _LSOListModel.SalePersonList = _SlPrsnList;

                SetAlldata(AllData, _LSOListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
                
            }
        }
        private void SetAlldata(DataSet dt, LSOListModel _LSOListModel)
        {
            List<LocalSalesOrder> _LocalSalesOrderList = new List<LocalSalesOrder>();
            if (dt.Tables[2].Rows.Count > 0)
            {
                //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
            }
            if (dt.Tables[1].Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    LocalSalesOrder _LocalSalesOrder = new LocalSalesOrder();
                    _LocalSalesOrder.OrderNo = dr["OrderNo"].ToString();
                    _LocalSalesOrder.OrderDate = dr["OrderDate"].ToString();
                    _LocalSalesOrder.SalesPerson = dr["SalesPerson"].ToString();
                    _LocalSalesOrder.OrderDt = dr["OrderDt"].ToString();
                    _LocalSalesOrder.OrderType = dr["OrderType"].ToString();
                    _LocalSalesOrder.SourceType = dr["SourceType"].ToString();
                    _LocalSalesOrder.src_doc_number = dr["SrcDocNoDt"].ToString();
                    _LocalSalesOrder.cust_name = dr["cust_name"].ToString();
                    _LocalSalesOrder.cust_alias = dr["cust_alias"].ToString();
                    _LocalSalesOrder.curr_logo = dr["curr_logo"].ToString();
                    _LocalSalesOrder.net_val_bs = dr["net_val_bs"].ToString();
                    _LocalSalesOrder.net_val_spec = dr["net_val_spec"].ToString();
                    _LocalSalesOrder.OrderStauts = dr["OrderStauts"].ToString();
                    _LocalSalesOrder.CreateDate = dr["CreateDate"].ToString();
                    _LocalSalesOrder.ApproveDate = dr["ApproveDate"].ToString();
                    _LocalSalesOrder.ModifyDate = dr["ModifyDate"].ToString();
                    _LocalSalesOrder.create_by = dr["create_by"].ToString();
                    _LocalSalesOrder.app_by = dr["app_by"].ToString();
                    _LocalSalesOrder.mod_by = dr["mod_by"].ToString();
                    _LocalSalesOrder.ref_doc_no = dr["ref_doc_no"].ToString();
                    _LocalSalesOrderList.Add(_LocalSalesOrder);
                }
            }
            _LSOListModel.LocalSalesOrderList = _LocalSalesOrderList;
            _LSOListModel.FinStDt = dt.Tables[3].Rows[0]["findate"].ToString();
        }
        private void CommonPageDetails()
        {
            try
            {
                string BrchID = string.Empty;
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
        public ActionResult ESOList(LSOListModel _LSOList_Model)
        {
            try
            {
                //if (Session["MenuDocumentId"] != null)
                //{
                DocumentMenuId = "105103145110";
                //Session["MenuDocumentId"] = DocumentMenuId;
                //if (Session["MenuDocumentId"].ToString() == "105103145110")
                //{
                //    DocumentMenuId = "105103145110";
                //}
                //}
                CommonPageDetails();
                var CustType = "E";
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");


                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                //string endDate = dtnow.ToString("yyyy-MM-dd");
                // LSOListModel _LSOList_Model = new LSOListModel();
                _LSOList_Model.CustType = CustType;
                _LSOList_Model.MenuDocumentId = DocumentMenuId;
                _LSOList_Model.DocumentMenuId = DocumentMenuId;
               // GetAutoCompleteSearchCustList(_LSOList_Model);
                if (TempData["ListFilterData"] != null)
                {
                    _LSOList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _LSOList_Model.SO_CustID = a[0].Trim();
                    _LSOList_Model.SO_FromDate = a[1].Trim();
                    _LSOList_Model.SO_ToDate = a[2].Trim();
                    _LSOList_Model.SO_Status = a[3].Trim();
                    if (_LSOList_Model.SO_Status == "0")
                    {
                        _LSOList_Model.SO_Status = null;
                    }
                }
               
                if (_LSOList_Model.SO_FromDate == null)
                {
                    _LSOList_Model.FromDate = startDate;
                    _LSOList_Model.SO_FromDate = startDate;
                    _LSOList_Model.SO_ToDate = CurrentDate;
                }
                else
                {
                    _LSOList_Model.FromDate = _LSOList_Model.SO_FromDate;
                }
                _LSOList_Model.LocalSalesOrderList = getLSOList(_LSOList_Model, startDate);
                //GetStatusList(_LSOList_Model);

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
                else
                {
                    statusLists.Insert(0, new Status() { status_id = "0", status_name = "All" });
                }
                _LSOList_Model.StatusList = statusLists;

                GetAllData(_LSOList_Model);


               // ViewBag.MenuPageName = getDocumentName(_LSOList_Model);
                _LSOList_Model.Title = title;
                //Session["LSOSearch"] = "0";
                _LSOList_Model.LSOSearch = "0";
             //   ViewBag.VBRoleList = GetRoleList();
                //ViewBag.AppLevel = GetApprovalLevel();
                var other = new CommonController(_Common_IServices);
               // ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                _LSOList_Model.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSalesOrder/LSOList.cshtml", _LSOList_Model);

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
                //    if (Session["MenuDocumentId"].ToString() == "105103125")
                //    {
                //        DocumentMenuId = "105103125";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145110")
                //    {
                //        DocumentMenuId = "105103145110";
                //    }
                //}
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
        public void GetStatusList(LSOListModel _LSOList_Model)
        {
            try
            {
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103125")
                //    {
                //        DocumentMenuId = "105103125";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145110")
                //    {
                //        DocumentMenuId = "105103145110";
                //    }
                //}
                if (_LSOList_Model.DocumentMenuId != null)
                {
                    if (_LSOList_Model.DocumentMenuId == "105103125")
                    {
                        DocumentMenuId = "105103125";
                    }
                    if (_LSOList_Model.DocumentMenuId == "105103145110")
                    {
                        DocumentMenuId = "105103145110";
                    }
                }
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _LSOList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private string getDocumentName(LSOListModel _LSOList_Model)
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
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103125")
                //    {
                //        DocumentMenuId = "105103125";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145110")
                //    {
                //        DocumentMenuId = "105103145110";
                //    }
                //}
                if (_LSOList_Model.MenuDocumentId != null)
                {
                    if (_LSOList_Model.MenuDocumentId == "105103125")
                    {
                        DocumentMenuId = "105103125";
                    }
                    if (_LSOList_Model.MenuDocumentId == "105103145110")
                    {
                        DocumentMenuId = "105103145110";
                    }
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
        public ActionResult GetAutoCompleteSearchCustList(LSOListModel _LSOListModel)
        {
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string CustType = string.Empty;
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
                if (string.IsNullOrEmpty(_LSOListModel.SO_CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _LSOListModel.SO_CustName;
                }
                //if (Session["MenuDocumentId"] != null)
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103125")
                //    {
                //        CustType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145110")
                //    {
                //        CustType = "E";
                //    }
                //}
                if (_LSOListModel.DocumentMenuId != null)
                {
                    if (_LSOListModel.DocumentMenuId == "105103125")
                    {
                        CustType = "D";
                    }
                    else if (_LSOListModel.DocumentMenuId == "105103145110")
                    {
                        CustType = "E";
                    }
                }
                CustList = _LSOList_ISERVICE.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustType, _LSOListModel.DocumentMenuId);

                List<CustomerName> _CustList = new List<CustomerName>();
                foreach (var data in CustList)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.cust_id = data.Key;
                    _CustDetail.cust_name = data.Value;
                    _CustList.Add(_CustDetail);
                }
                _LSOListModel.CustomerNameList = _CustList;
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
        //public ActionResult ErrorPage()
        //{
        //    try
        //    {
        //        return PartialView("~/Views/Shared/Error.cshtml");
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //}
        public ActionResult GetSaleOrderList(string docid, string status)
        {
            LSOListModel _LSOList_Model = new LSOListModel();
            //_LSOList_Model.MenuDocumentId = docid;
            //Session["MenuDocumentId"] = docid;
            //Session["WF_status"] = status;
            _LSOList_Model.WF_status = status;
            if (docid == "105103125")
            {
                return RedirectToAction("LSOList", _LSOList_Model);
            }
            else
            {
                return RedirectToAction("ESOList", _LSOList_Model);
            }

        }
        private List<LocalSalesOrder> getLSOList(LSOListModel _lSOListModel,string startDate)
        {
            try
            {
                string SO_type = string.Empty;
                _LocalSalesOrderList = new List<LocalSalesOrder>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                string wfstatus = "";
                if (_lSOListModel.SO_FromDate == null)
                {
                    if (startDate != null)
                    {
                        _lSOListModel.SO_FromDate = startDate;
                    }
                }
                if (_lSOListModel.WF_status != null)
                {
                    wfstatus = _lSOListModel.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }     
                if (_lSOListModel.MenuDocumentId != null)
                {
                    if (_lSOListModel.MenuDocumentId == "105103125")
                    {
                        SO_type = "D";
                    }
                    if (_lSOListModel.MenuDocumentId == "105103145110")
                    {
                        SO_type = "E";
                    }
                }
                DataSet dt = new DataSet();
                dt = _LSOList_ISERVICE.GetSODetailListDAL(CompID, BrchID, _lSOListModel.SO_CustID, _lSOListModel.SO_FromDate, _lSOListModel.SO_ToDate, _lSOListModel.SO_Status, UserID, DocumentMenuId, wfstatus, SO_type, _lSOListModel.SQ_SalePerson);
                if (dt.Tables[1].Rows.Count > 0)
                {
                    //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        LocalSalesOrder _LocalSalesOrder = new LocalSalesOrder();
                        _LocalSalesOrder.OrderNo = dr["OrderNo"].ToString();
                        _LocalSalesOrder.OrderDate = dr["OrderDate"].ToString();
                        _LocalSalesOrder.SalesPerson = dr["SalesPerson"].ToString();
                        _LocalSalesOrder.OrderDt = dr["OrderDt"].ToString();
                        _LocalSalesOrder.OrderType = dr["OrderType"].ToString();
                        _LocalSalesOrder.SourceType = dr["SourceType"].ToString();
                        _LocalSalesOrder.cust_name = dr["cust_name"].ToString();
                        _LocalSalesOrder.cust_alias = dr["cust_alias"].ToString();
                        _LocalSalesOrder.curr_logo = dr["curr_logo"].ToString();
                        _LocalSalesOrder.net_val_bs = dr["net_val_bs"].ToString();
                        _LocalSalesOrder.net_val_spec = dr["net_val_spec"].ToString();
                        _LocalSalesOrder.OrderStauts = dr["OrderStauts"].ToString();
                        _LocalSalesOrder.CreateDate = dr["CreateDate"].ToString();
                        _LocalSalesOrder.ApproveDate = dr["ApproveDate"].ToString();
                        _LocalSalesOrder.ModifyDate = dr["ModifyDate"].ToString();
                        _LocalSalesOrder.create_by = dr["create_by"].ToString();
                        _LocalSalesOrder.app_by = dr["app_by"].ToString();
                        _LocalSalesOrder.mod_by = dr["mod_by"].ToString();
                        _LocalSalesOrder.ref_doc_no = dr["ref_doc_no"].ToString();
                        _LocalSalesOrderList.Add(_LocalSalesOrder);
                    }
                }
                _lSOListModel.FinStDt = dt.Tables[2].Rows[0]["findate"].ToString();
                return _LocalSalesOrderList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult AddNewLSO(string DocumentMenuId)
        {
            LSODetailModel _LSODetailAddModel = new LSODetailModel();
            LSOListModel _LSOList_Model = new LSOListModel();
            //Session["Message"] = "New";
            //Session["Command"] = "AddNew";
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["SO_No"] = null;
            //Session["SO_Date"] = null;
            _LSODetailAddModel.Message = "New";
            _LSODetailAddModel.Command = "AddNew";
            _LSODetailAddModel.TransType = "Save";
            _LSODetailAddModel.BtnName = "BtnAddNew";
            _LSODetailAddModel.DocumentMenuId = DocumentMenuId;
            _LSODetailAddModel.DocumentStatus = "D";
            _LSODetailAddModel.CustType = _LSOList_Model.CustType;
            _LSODetailAddModel.WF_status1 = _LSOList_Model.WF_status;
            TempData["ModelData"] = _LSODetailAddModel;
            UrlModel _urlModel = new UrlModel();
            _urlModel.Command = "AddNew";
            _urlModel.TransType = "Save";
            _urlModel.BtnName = "BtnAddNew";
            _urlModel.DocumentMenuId = DocumentMenuId;
            _urlModel.DocumentStatus = _LSODetailAddModel.DocumentStatus;
            _urlModel.CustType = _LSOList_Model.CustType;
            _urlModel.WF_status1 = _LSOList_Model.WF_status;
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
                if (DocumentMenuId == "105103125")
                {
                    return RedirectToAction("LSOList");
                }
                else
                {
                    return RedirectToAction("ESOList");
                }

            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("LSODetail", "LSODetail", _urlModel);
        }
        public ActionResult SearchLSODetail(string CustId, string Fromdate, string Todate, string Status, string DocumentMenuId,string sales_person)
        {
            try
            {
                LSOListModel _LSOList_Model = new LSOListModel();
                _LSOList_Model.WF_status = null;
                string SO_type = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (_LSOList_Model.DocumentMenuId != null && _LSOList_Model.DocumentMenuId != null)
                {
                    if (_LSOList_Model.DocumentMenuId == "105103125")
                    {
                        SO_type = "D";
                    }
                    if (_LSOList_Model.DocumentMenuId == "105103145110")
                    {
                        SO_type = "E";
                    }
                }
                else
                {
                    if (DocumentMenuId == "105103125")
                    {
                        SO_type = "D";
                    }
                    if (DocumentMenuId == "105103145110")
                    {
                        SO_type = "E";
                    }
                    _LSOList_Model.DocumentMenuId = DocumentMenuId;
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                _LocalSalesOrderList = new List<LocalSalesOrder>();
                DataSet dt = new DataSet();
                dt = _LSOList_ISERVICE.GetSODetailListDAL(CompID, BrchID, CustId, Fromdate, Todate, Status, UserID, DocumentMenuId, "", SO_type, sales_person);

                _LSOList_Model.LSOSearch = "LSO_Search";
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    LocalSalesOrder _LocalSalesOrder = new LocalSalesOrder();
                    _LocalSalesOrder.OrderNo = dr["OrderNo"].ToString();
                    _LocalSalesOrder.OrderDate = dr["OrderDate"].ToString();
                    _LocalSalesOrder.SalesPerson = dr["SalesPerson"].ToString();
                    _LocalSalesOrder.OrderDt = dr["OrderDt"].ToString();
                    _LocalSalesOrder.OrderType = dr["OrderType"].ToString();
                    _LocalSalesOrder.SourceType = dr["SourceType"].ToString();
                    //_LocalSalesOrder.src_doc_number = dr["src_doc_number"].ToString();
                    _LocalSalesOrder.cust_name = dr["cust_name"].ToString();
                    _LocalSalesOrder.cust_alias = dr["cust_alias"].ToString();
                    _LocalSalesOrder.curr_logo = dr["curr_logo"].ToString();
                    _LocalSalesOrder.net_val_bs = dr["net_val_bs"].ToString();
                    _LocalSalesOrder.net_val_spec = dr["net_val_spec"].ToString();
                    _LocalSalesOrder.OrderStauts = dr["OrderStauts"].ToString();
                    _LocalSalesOrder.CreateDate = dr["CreateDate"].ToString();
                    _LocalSalesOrder.ApproveDate = dr["ApproveDate"].ToString();
                    _LocalSalesOrder.ModifyDate = dr["ModifyDate"].ToString();
                    //_LocalSalesOrder.src_doc_date = dr["src_doc_date"].ToString();
                    _LocalSalesOrder.create_by = dr["create_by"].ToString();
                    _LocalSalesOrder.app_by = dr["app_by"].ToString();
                    _LocalSalesOrder.mod_by = dr["mod_by"].ToString();
                    _LocalSalesOrder.ref_doc_no = dr["ref_doc_no"].ToString();
                    _LocalSalesOrderList.Add(_LocalSalesOrder);
                }
                _LSOList_Model.FinStDt = dt.Tables[2].Rows[0]["findate"].ToString();
                _LSOList_Model.LocalSalesOrderList = _LocalSalesOrderList;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSOList.cshtml", _LSOList_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");

            }
        }
        [HttpPost]
        public JsonResult GetSO_Detail(string SO_No, string SO_Date)
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
                DataSet result = _LSOList_ISERVICE.GetSO_Detail(Comp_ID, BranchID, SO_No, SO_Date);
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

        //[HttpPost]
        public ActionResult GetSOTrackingDetail(string SO_No, string SO_Date)
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
                DataSet result = _LSOList_ISERVICE.GetSOTrackingDetail(Comp_ID, BranchID, SO_No, SO_Date);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.SOTrackingList = result.Tables[0];

                return View("~/Areas/ApplicationLayer/Views/Shared/_SalesOrderTracking.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetProductionTrackingDetail(string SO_No, string SO_Date)
        {
            try
            {
                //JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string BranchID = string.Empty;
                if (Session["CompId"] != null)
                    Comp_ID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BranchID = Session["BranchId"].ToString();

                DataSet result = _LSOList_ISERVICE.GetProductionTrackingDetail(Comp_ID, BranchID, SO_No, SO_Date);
                //DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.ProductionTrackingList = result.Tables[0];

                return View("~/Areas/ApplicationLayer/Views/Shared/_ProductionTracking.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetProductionPlanQC_DetailsOnClick(string Item_id, string flag, string Item_Name, string UomAlias, string qty, string Plan_no, string Plan_dt)
        {
            try
            {
               
                //objModel.opId = opName;
                //objModel.shflId = shflName;
               ViewBag.ItemName = Item_Name;
                ViewBag.UOM = UomAlias;
                ViewBag.Qty = qty;
                ViewBag.InsightType = flag;
                ViewBag.ProductionAnalysisDetails = GetSalesOrderTracking_DetailsOnClickInfo(Item_id, Plan_no, Plan_dt, flag);
              
                return PartialView("~/Areas/Common/Views/Cmn_ProductionPlanTrackingQC.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

        }
        public DataTable GetSalesOrderTracking_DetailsOnClickInfo(string Item_id, string Plan_no, string Plan_dt, string flag)
        {
            try
            {
                string BrID = string.Empty;
                string CompID = string.Empty;
                DataTable dt = new DataTable();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                dt = _LSOList_ISERVICE.GetProductionPlan_DetailsInfo(CompID, BrID, Item_id, Plan_no, Plan_dt, flag);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetItemQCParamDetail(string ItemID, string qc_no, string qc_dt)
        {
            try
            {
                string BrID = string.Empty;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    BrID = Session["BranchId"].ToString();
                }
                DataSet ds = _LSOList_ISERVICE.GetItemQCParamDetail(CompID, BrID, ItemID, qc_no, qc_dt);
                ViewBag.ItemDetailsQc = ds.Tables[0];
                ViewBag.ItemDetailsQcList = ds.Tables[1];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_OnlyShowQCParameterEvalutionDetail.cshtml");
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