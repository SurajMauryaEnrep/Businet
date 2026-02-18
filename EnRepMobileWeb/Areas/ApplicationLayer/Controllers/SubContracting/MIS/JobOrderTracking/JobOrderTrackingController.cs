using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MIS.JobOrderTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.MIS.JobOrderTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SubContracting.MIS.JobOrderTracking
{
    public class JobOrderTrackingController : Controller
    {
        string CompID, language = String.Empty;
        string DocumentMenuId = "105108125105", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        JobOrderTracking_ISERVICE _JobOrderTracking_ISERVICE;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public JobOrderTrackingController(Common_IServices _Common_IServices, JobOrderTracking_ISERVICE _JobOrderTracking_ISERVICE
            , GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._JobOrderTracking_ISERVICE = _JobOrderTracking_ISERVICE;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/JobOrderTracking
        public ActionResult JobOrderTracking()
        {
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string Language = string.Empty;
            JobOrderTracking_Model _JobOrderTracking_Model = new JobOrderTracking_Model();
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                Language = Session["Language"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            CommonPageDetails();
            DateTime dtnow = DateTime.Now;
          //  string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            string ToDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");

            DataSet dttbl = new DataSet();
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                _JobOrderTracking_Model.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }

           //_JobOrderTracking_Model.FromDate = startDate;
            _JobOrderTracking_Model.ToDate = ToDate;
            GetAutoCompleteSearchSuppList(_JobOrderTracking_Model);
           
            _JobOrderTracking_Model.JobOrderTrackList = SearchJOTrackDetail_Onload(_JobOrderTracking_Model);
           
            List<Status> statusLists = new List<Status>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                Status list = new Status();
                list.status_id = dr["status_code"].ToString();
                list.status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            _JobOrderTracking_Model.StatusList = statusLists;

            _JobOrderTracking_Model.Title = title;
            //Session["JOSearch"] = "0";
            _JobOrderTracking_Model.JOTrackSearch = "";
            //ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/ApplicationLayer/Views/SubContracting/MIS/JobOrderTracking/JobOrderTrackingDetail.cshtml", _JobOrderTracking_Model);
        }
        private DataSet GetFyList()
        {
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            try
            {
                string Comp_ID = string.Empty;
                string Br_Id = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_Id = Session["BranchId"].ToString();
                }
                DataSet dt = _GeneralLedger_ISERVICE.Get_FYList(Comp_ID, Br_Id);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }


        private List<JobOrderTrackListDetail> SearchJOTrackDetail_Onload(JobOrderTracking_Model _JobOrderTracking_Model)
        {
            try
            {
                List<JobOrderTrackListDetail> _JobOrderListDetail = new List<JobOrderTrackListDetail>();
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                DataTable dt = new DataTable();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                dt = _JobOrderTracking_ISERVICE.GetJOTrackDetailList_Onload(CompID, Br_ID, "0", "0", "0", _JobOrderTracking_Model, "0");

                
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        JobOrderTrackListDetail _JobOrderTrackList = new JobOrderTrackListDetail();
                        _JobOrderTrackList.OrderNo = dr["JobOrdNo"].ToString();
                        _JobOrderTrackList.OrderDate = dr["JobOrdDate"].ToString();
                        _JobOrderTrackList.OrderDt = dr["JobOrdDT"].ToString();
                        _JobOrderTrackList.SupplierId = dr["supp_id"].ToString();
                        _JobOrderTrackList.SupplierName = dr["supp_name"].ToString();
                        _JobOrderTrackList.FinishProductId = dr["fg_product_id"].ToString();
                        _JobOrderTrackList.FinishProductName = dr["FinishPrdctName"].ToString();
                        _JobOrderTrackList.FinishProducUOMId = dr["fg_uom_id"].ToString();
                        _JobOrderTrackList.FinishProducUOM = dr["FinishPrdctUOM"].ToString();
                        _JobOrderTrackList.OperationId = dr["op_id"].ToString();
                        _JobOrderTrackList.OperationName = dr["op_name"].ToString();
                        _JobOrderTrackList.OpOutProductID = dr["OpOutPrdctId"].ToString();
                        _JobOrderTrackList.OpOutProductName = dr["OpOutPrdctName"].ToString();
                        _JobOrderTrackList.OpOutUOMId = dr["OpOutUomId"].ToString();
                        _JobOrderTrackList.OpOutUOM = dr["OpOutUOM"].ToString();
                        _JobOrderTrackList.OrderQuantity = dr["OrdQty"].ToString();
                        _JobOrderTrackList.DispatchQuantity = dr["DispQty"].ToString();
                        _JobOrderTrackList.ReceviedQuantity = dr["RecevQty"].ToString();
                        _JobOrderTrackList.AcceptedQuantity = dr["AccptQty"].ToString();
                        _JobOrderTrackList.RejectedQuantity = dr["RejQty"].ToString();
                        _JobOrderTrackList.ReworkableQuantity = dr["ReworkQty"].ToString();
                        _JobOrderTrackList.Status = dr["JobStatus"].ToString();
                        //_JobOrderTrackList.StatusName = dr["issued"].ToString();
                        
                        _JobOrderListDetail.Add(_JobOrderTrackList);
                    }
                }
                return _JobOrderListDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchJOTrackDetail(string SuppId, string FinishProdctID, string OutOPProdctID, string Fromdate, string Todate, string Status)
        {
            try
            {
                List<JobOrderTrackListDetail> _JOTrackListDetail = new List<JobOrderTrackListDetail>();
                JobOrderTracking_Model _JobOrderTracking_Model = new JobOrderTracking_Model();
                string CompID = string.Empty;
                string BrachID = string.Empty;
                string Partial_View = string.Empty;
                DataTable dt = new DataTable();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrachID = Session["BranchId"].ToString();
                }
                dt = _JobOrderTracking_ISERVICE.GetJOTrackDetail(CompID, BrachID, SuppId, FinishProdctID, OutOPProdctID, Fromdate, Todate, Status);

                if (dt.Rows.Count > 0)
                {
                    //_StockDetailShopfloor_Model.StockBy = StockBy;
                    //_StockDetailShopfloor_Model.StockByFilter = StockBy;

                    foreach (DataRow dr in dt.Rows)
                    {
                        JobOrderTrackListDetail _JobOrderTrackList = new JobOrderTrackListDetail();
                        _JobOrderTrackList.OrderNo = dr["JobOrdNo"].ToString();
                        _JobOrderTrackList.OrderDate = dr["JobOrdDate"].ToString();
                        _JobOrderTrackList.OrderDt = dr["JobOrdDT"].ToString();
                        _JobOrderTrackList.SupplierId = dr["supp_id"].ToString();
                        _JobOrderTrackList.SupplierName = dr["supp_name"].ToString();
                        _JobOrderTrackList.FinishProductId = dr["fg_product_id"].ToString();
                        _JobOrderTrackList.FinishProductName = dr["FinishPrdctName"].ToString();
                        _JobOrderTrackList.FinishProducUOMId = dr["fg_uom_id"].ToString();
                        _JobOrderTrackList.FinishProducUOM = dr["FinishPrdctUOM"].ToString();
                        _JobOrderTrackList.OperationId = dr["op_id"].ToString();
                        _JobOrderTrackList.OperationName = dr["op_name"].ToString();
                        _JobOrderTrackList.OpOutProductID = dr["OpOutPrdctId"].ToString();
                        _JobOrderTrackList.OpOutProductName = dr["OpOutPrdctName"].ToString();
                        _JobOrderTrackList.OpOutUOMId = dr["OpOutUomId"].ToString();
                        _JobOrderTrackList.OpOutUOM = dr["OpOutUOM"].ToString();
                        _JobOrderTrackList.OrderQuantity = dr["OrdQty"].ToString();
                        _JobOrderTrackList.DispatchQuantity = dr["DispQty"].ToString();
                        _JobOrderTrackList.ReceviedQuantity = dr["RecevQty"].ToString();
                        _JobOrderTrackList.AcceptedQuantity = dr["AccptQty"].ToString();
                        _JobOrderTrackList.RejectedQuantity = dr["RejQty"].ToString();
                        _JobOrderTrackList.ReworkableQuantity = dr["ReworkQty"].ToString();
                        _JobOrderTrackList.Status = dr["JobStatus"].ToString();

                        _JOTrackListDetail.Add(_JobOrderTrackList);
                    }
                }
                _JobOrderTracking_Model.JTFilter = "JTFilter";
                _JobOrderTracking_Model.JobOrderTrackList = _JOTrackListDetail;
                CommonPageDetails();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMisJOTrackingDetails.cshtml", _JobOrderTracking_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        [HttpPost]
        public JsonResult GetAllQtyItemDetailList(string Type, string JobOrdNo, string hdnJobOrdDt)
        {
             try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string Branch = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Branch = Session["BranchId"].ToString();
                }
                DataSet result = _JobOrderTracking_ISERVICE.GetAllQtyItemDetailList(Comp_ID, Branch,Type, JobOrdNo, hdnJobOrdDt);

                //DataView dv = new DataView(result.Tables[0]);
                //dv.Sort = "CDate desc";
                //DataTable dt = new DataTable();
                //dt = dv.ToTable();
                //DataSet dset = new DataSet();
                //dset.Tables.Add(dt);
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

        [HttpPost]
        public JsonResult GetMtrlDispRawMaterialDetailList(string JobOrdNo, string JobOrdrDate, string DispatchNo, string hdnDispatchDt)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string Branch = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Branch = Session["BranchId"].ToString();
                }
                //_JobOrderTracking_Model.JOTrackSearch = "JoTrack_Search";
                DataSet result = _JobOrderTracking_ISERVICE.GetMtrlDispRawMaterialDetailList(Comp_ID, Branch,JobOrdNo, JobOrdrDate, DispatchNo, hdnDispatchDt);
               
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

        public ActionResult GetAutoCompleteSearchSuppList(JobOrderTracking_Model _JobOrderTracking_Model)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(_JobOrderTracking_Model.Supp_Name))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _JobOrderTracking_Model.Supp_Name;
                }
                CustList = _JobOrderTracking_ISERVICE.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _JobOrderTracking_Model.SupplierNameList = _SuppList;
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
        [HttpPost]
        public ActionResult GetProductNameInDDLListPage(JobOrderTracking_Model _JobOrderTracking_Model)
        {
            JsonResult DataRows = null;
            string JOItmName = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(_JobOrderTracking_Model.JO_ItemName))
                    {
                        JOItmName = "0";
                    }
                    else
                    {
                        JOItmName = _JobOrderTracking_Model.JO_ItemName;
                    }

                    DataSet ProductList = _JobOrderTracking_ISERVICE.BindProductNameInDDL(Comp_ID, Br_ID, JOItmName);
                    DataRow DRow = ProductList.Tables[0].NewRow();

                    DRow[0] = "0";
                    DRow[1] = "All";
                    DRow[2] = "0";
                    ProductList.Tables[0].Rows.InsertAt(DRow, 0);
                    DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/

                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        private void CommonPageDetails()
        {
            try
            {
                string CompID = string.Empty;
                string BrchID = string.Empty;
                string UserID = string.Empty;
                string Language = string.Empty;
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
                    Language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, Language);
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