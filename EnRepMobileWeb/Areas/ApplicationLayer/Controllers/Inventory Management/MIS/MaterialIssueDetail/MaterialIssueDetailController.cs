using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MaterialIssueDetail;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialIssue;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialIssue.MaterialTransferIssue;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MaterialIssueDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.MaterialIssueDetail
{
    public class MaterialIssueDetailController : Controller
    {
        string CompID, BrId, language = String.Empty;
        string DocumentMenuId = "105102180125", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly MaterialIssueDetail_IService _mtiIservice;
        private readonly MaterialIssue_IServices _MaterialIssue_IServices;
        private readonly MaterialTransferIssue_ISERVICES _MTI_ISERVICES;
        private readonly ItemList_ISERVICES _itemSetup;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public MaterialIssueDetailController(Common_IServices _Common_IServices, MaterialIssue_IServices MaterialIssue_IServices,
            MaterialIssueDetail_IService mtiIservice, MaterialTransferIssue_ISERVICES MTI_ISERVICES, ItemList_ISERVICES itemSetup, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            _MaterialIssue_IServices = MaterialIssue_IServices;
            _mtiIservice = mtiIservice;
            _MTI_ISERVICES = MTI_ISERVICES;
            _itemSetup = itemSetup;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/MaterialIssueDetail
        public ActionResult MaterialIssueDetail()
        {
            getDocumentName();
            MaterialIssueDetail_Model objModel = new MaterialIssueDetail_Model();
            objModel.title = title;
            DateTime dtnow = DateTime.Now;
            //string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            DataSet dttbl = new DataSet();
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                objModel.fromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }
            string ToDate = dtnow.ToString("yyyy-MM-dd");
          //  objModel.fromDate = FromDate;
            objModel.toDate = ToDate;
            ViewBag.MenuPageName = getDocumentName();
            ViewBag.DocumentMenuId = DocumentMenuId;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/MaterialIssueDetail/MISMaterialIssueDetail.cshtml", objModel);
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
        public DataTable GetMisMaterialIssueDetails(string action, string itemId,
            string reqArea, string fromDate, string toDate, string transferType, string destinationBranch,
            string destinationWarehouse, string issueTo)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            DataTable dt = _mtiIservice.GetMaterialIssueReport(action, CompID, BrId, itemId, reqArea, fromDate, toDate, transferType, destinationBranch,
                destinationWarehouse, issueTo);
            return dt;
        }
        public JsonResult GetReqAreaList(string issueType)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            DataTable dt = new DataTable();
            DataSet ds = _MaterialIssue_IServices.GetRequirmentreaList(CompID, BrId, issueType);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return Json(JsonConvert.SerializeObject(dt));
        }
        public JsonResult GetToBranchList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();

            DataTable dt = _MTI_ISERVICES.GetToBranchList(CompID);
            return Json(JsonConvert.SerializeObject(dt));
        }
        public JsonResult GetToWarehouseList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();

            DataTable dt = _MTI_ISERVICES.GetToWhList(CompID, BrId);
            return Json(JsonConvert.SerializeObject(dt));
        }
        public JsonResult GetIssueToList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            DataTable dt = new DataTable();
            DataSet ds = _mtiIservice.IssueToList(CompID, "0", BrId);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return Json(JsonConvert.SerializeObject(dt));
        }
        public JsonResult GetItemsList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            DataTable dt = _itemSetup.BindGetItemList("", CompID, BrId);
            return Json(JsonConvert.SerializeObject(dt));
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
        public ActionResult Get_S_I_E_issueReport(string issueType, string itemId, string reqArea, string fromDate, string toDate, string issueTo)
        {
            ViewBag.Issuetype = issueType;
            //set issueto as 0 for interna issue
            if (issueType == "I")
                issueTo = "0";
            ViewBag.IESissueReport = GetMisMaterialIssueDetails(issueType, itemId, reqArea, fromDate, toDate, "0", "0", "0", issueTo);
            MaterialIssueDetail_Model objModel = new MaterialIssueDetail_Model();
            objModel.SearchStatus = "1";
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISInternalIssue.cshtml", objModel);
        }
        public ActionResult GetMaterialTransferIssueReport(string itemId,
             string fromDate, string toDate, string transferType, string destinationBranch,
            string destinationWarehouse, string issueTo)
        {
            ViewBag.MaterialTransferIssueReport = GetMisMaterialIssueDetails("M", itemId, "0", fromDate, toDate, transferType, destinationBranch,
                destinationWarehouse, issueTo);
            MaterialIssueDetail_Model objModel = new MaterialIssueDetail_Model();
            objModel.SearchStatus = "1";
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISMaterialTransferIssue.cshtml", objModel);
        }
        public ActionResult GetSubItemDetails(string act, string issueNo, string issueDate, string itemId)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                DataTable dt = new DataTable();
                DataSet ds = _mtiIservice.GetSubItemDetails(act, CompID, BrId, issueNo, issueDate, itemId);
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];

                var flag = "";
                if (act == "ExterPendingQty" || act == "ExterRec_qty")
                {
                    flag = act;
                }
                else
                {
                    flag = "MTIMIS";
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {

                    Flag = flag,
                    dt_SubItemDetails = dt,
                    ShowStock = "Y",
                    _subitemPageName = "MI",
                    IsDisabled = "Y",
                    decimalAllowed = "Y",
                };

                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
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