using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.ShipmentDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.ShipmentDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesAndDistributionIServices;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.Shipment;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.MIS.MISShipmentDetail
{
    public class MISShipmentDetailController : Controller
    {
        string DocumentMenuId = "105103190110", title;
        string CompID, language = String.Empty, BrID;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        private readonly ShipmentDetail_IService _shipIService;
        private readonly Shipment_ISERVICES _Shipment_ISERVICES;
        private readonly LSODetail_ISERVICE _LSODetail_ISERVICE;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public MISShipmentDetailController(Common_IServices _Common_IServices, ShipmentDetail_IService shipIService, Shipment_ISERVICES Shipment_ISERVICES,
            LSODetail_ISERVICE LSODetail_ISERVICE, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            _shipIService = shipIService;
            _Shipment_ISERVICES = Shipment_ISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;

        }
        // GET: ApplicationLayer/MISShipmentDetail
        public ActionResult MISShipmentDetail()
        {
            ShipmentMISDetail_Model objModel = new ShipmentMISDetail_Model();
            ViewBag.MenuPageName = getDocumentName();
            DateTime dtnow = DateTime.Now;
           // string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
     

            DataSet dttbl = new DataSet();
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                objModel.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }
            string FromDate = objModel.FromDate;
            string ToDate = dtnow.ToString("yyyy-MM-dd");
            //objModel.FromDate = FromDate;
            objModel.ToDate = ToDate;
            objModel.Title = title;
            objModel.CustomersList = GetCustomerList();
            objModel.itemsList = GetItemsList();
            GetShipmentItemWiseSummary(FromDate,ToDate,"","","");
            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/MISShipmentDetail/MISShipmentDetail.cshtml", objModel);
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
        public List<Customers> GetCustomerList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            DataTable dtCust = _Shipment_ISERVICES.GetCustomer_List(CompID, "0", BrID, "");
            List<Customers> CustList = new List<Customers>();
            foreach (DataRow dr in dtCust.Rows)
            {
                Customers cust = new Customers
                {
                    CustId = dr["cust_id"].ToString(),
                    CustName = dr["cust_name"].ToString()
                };
                CustList.Add(cust);
            }
           // CustList.Insert(0, new Customers { CustId = "0", CustName = "---All---" });

            return CustList;
        }
        public List<Items> GetItemsList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            DataTable dtitems = _shipIService.GetItemsList(CompID);
            List<Items> LstItems = new List<Items>();
            foreach (DataRow drItem in dtitems.Rows)
            {
                Items item = new Items
                {
                    ItemId = drItem["item_id"].ToString(),
                    ItemName = drItem["item_name"].ToString()
                };
                LstItems.Add(item);
            }
            LstItems.Insert(0, new Items { ItemId = "0", ItemName = "---All---" });
            return LstItems;
        }
        private DataTable GetShipmentMisReport(string fromDate, string toDate, string shipmentType,
             string customerId, string ItemId, string showAs)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            DataTable dtMisReport = _shipIService.GetShipmentMISDetailsReport(CompID, BrID, fromDate, toDate, shipmentType, customerId, ItemId, showAs);
            if (dtMisReport.Rows.Count > 0)
            {
                return dtMisReport;
            }
            else
            {
                return null;
            }
        }
        public ActionResult GetShipmentItemWiseSummary(string fromDate, string toDate, string shipmentType,
             string customerId, string ItemId)
        {
            //IWS
            ShipmentMISDetail_Model objModel = new ShipmentMISDetail_Model();
            objModel.SearchStatus = "SEARCH";
            DataTable dt = GetShipmentMisReport(fromDate, toDate, shipmentType, customerId, ItemId, "S");
            ViewBag.ItemWiseSummary = dt;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISShipmentItemSummary.cshtml", objModel);
        }
        public ActionResult GetShipmentSummary(string fromDate, string toDate, string shipmentType,
             string customerId, string ItemId)
        {
            //S
            ShipmentMISDetail_Model objModel = new ShipmentMISDetail_Model();
            objModel.SearchStatus = "SEARCH";
            DataTable dt = GetShipmentMisReport(fromDate, toDate, shipmentType, customerId, ItemId, "D");
            ViewBag.ShipmentSummary = dt;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISShipmentSummary.cshtml",objModel);
        }
        public ActionResult GetShipmentDetails(string fromDate, string toDate, string shipmentType,
             string customerId, string ItemId)
        {
            ShipmentMISDetail_Model objModel = new ShipmentMISDetail_Model();
            objModel.SearchStatus = "SEARCH";
            //D
            DataTable dt = GetShipmentMisReport(fromDate, toDate, shipmentType, customerId, ItemId, "D");
            ViewBag.ShipmentDetails = dt;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISShipmentDetail.cshtml",objModel);
        }
    }

}