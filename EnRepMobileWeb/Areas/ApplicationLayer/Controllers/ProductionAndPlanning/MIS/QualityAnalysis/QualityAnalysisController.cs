using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MIS.QualityAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.QualityAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.MIS.QualityAnalysis
{
    public class QualityAnalysisController : Controller
    {
        string compId, userId, branchId, language, title = String.Empty;
        //string DocumentMenuId = "105105155120";
        string DocumentMenuId = string.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        private readonly QualityAnalysis_IService _qaService;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public QualityAnalysisController(Common_IServices _Common_IServices, QualityAnalysis_IService qaService, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            _qaService = qaService;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/QualityAnalysis
        public ActionResult QualityAnalysisINV(QualityAnalysisModel qualityAnalysisModel)
        {
            string DocId = string.Empty;
            DocumentMenuId = "105102180145";/*By Inventory*/
            DocId = "105102180145";
            //qualityAnalysisModel.DocumentID = DocumentMenuId;
            ViewBag.DocumentMenuId = DocumentMenuId;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();
            ViewBag.MenuPageName = getDocumentName();
            DateTime dtnow = DateTime.Now;
            /*Commented By Nitesh 06-12-2023 from_date fincial start Date not month start date*/
            // string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            //string FromDate = new DateTime(dtnow.Year, 4, 1).ToString("yyyy-MM-dd");
            string ToDate = dtnow.ToString("yyyy-MM-dd");

            DataSet dttbl = new DataSet();
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                qualityAnalysisModel.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }
            string FromDate = qualityAnalysisModel.FromDate;
            qualityAnalysisModel.ToDate = ToDate;
           // qualityAnalysisModel.FromDate = FromDate;
            qualityAnalysisModel.Title = title;
            DataTable dtQaData = _qaService.GetQualityAnalysisReport(compId, branchId, qualityAnalysisModel.QcType, "", qualityAnalysisModel.FromDate, qualityAnalysisModel.ToDate, "", DocId);
            ViewBag.QualityAnalysisDetail = _qaService.GetQADetailsByItemId(compId, branchId, qualityAnalysisModel.QcType, "", qualityAnalysisModel.FromDate, qualityAnalysisModel.ToDate, "", DocId);
            ViewBag.QAReport = dtQaData;
            DataTable dt = _qaService.GetItemsDetails(compId, branchId);
            List<ItemsDetailsModel> lstItemsDetails = new List<ItemsDetailsModel>();
            ItemsDetailsModel lstItem = new ItemsDetailsModel
            {
                item_id = "0",
                item_name = "----ALL----"
            };
            lstItemsDetails.Add(lstItem);
            foreach (DataRow dr in dt.Rows)
            {
                 lstItem = new ItemsDetailsModel
                {
                    item_id = dr["item_id"].ToString(),
                    item_name = dr["item_name"].ToString()
                };
                lstItemsDetails.Add(lstItem);
            }
           
            qualityAnalysisModel.Title = title;
            qualityAnalysisModel.ItemsList = lstItemsDetails;
           

            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/QualityAnalysis/QualityAnalysis.cshtml", qualityAnalysisModel);
        }
        public ActionResult QualityAnalysisPRD(QualityAnalysisModel qualityAnalysisModel)
        {
            string DocId = string.Empty;
            DocumentMenuId = "105105155120";
            //qualityAnalysisModel.DocumentID = DocumentMenuId;
            ViewBag.DocumentMenuId = DocumentMenuId;
            DocId = "105105155120";
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();
            ViewBag.MenuPageName = getDocumentName();
            DateTime dtnow = DateTime.Now;
            /*Commented By Nitesh 06-12-2023 from_date fincial start Date not month start date*/
            // string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            //string FromDate = new DateTime(dtnow.Year, 4, 1).ToString("yyyy-MM-dd");
            string ToDate = dtnow.ToString("yyyy-MM-dd");

            DataSet dttbl = new DataSet();
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                qualityAnalysisModel.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }
            string FromDate = qualityAnalysisModel.FromDate;
            qualityAnalysisModel.ToDate = ToDate;
            // qualityAnalysisModel.FromDate = FromDate;
            qualityAnalysisModel.Title = title;
            DataTable dtQaData = _qaService.GetQualityAnalysisReport(compId, branchId, qualityAnalysisModel.QcType, "", qualityAnalysisModel.FromDate, qualityAnalysisModel.ToDate, "", DocId);
            ViewBag.QualityAnalysisDetail = _qaService.GetQADetailsByItemId(compId, branchId, qualityAnalysisModel.QcType, "", qualityAnalysisModel.FromDate, qualityAnalysisModel.ToDate, "", DocId);
            ViewBag.QAReport = dtQaData;
            DataTable dt = _qaService.GetItemsDetails(compId, branchId);
            List<ItemsDetailsModel> lstItemsDetails = new List<ItemsDetailsModel>();
            ItemsDetailsModel lstItem = new ItemsDetailsModel
            {
                item_id = "0",
                item_name = "----ALL----"
            };
            lstItemsDetails.Add(lstItem);
            foreach (DataRow dr in dt.Rows)
            {
                lstItem = new ItemsDetailsModel
                {
                    item_id = dr["item_id"].ToString(),
                    item_name = dr["item_name"].ToString()
                };
                lstItemsDetails.Add(lstItem);
            }

            qualityAnalysisModel.Title = title;
            qualityAnalysisModel.ItemsList = lstItemsDetails;


            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/QualityAnalysis/QualityAnalysis.cshtml", qualityAnalysisModel);
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

        //public ActionResult GetQualityAnalysis(QualityAnalysisModel qualityAnalysisModel)
        //{
        //    if (Session["CompId"] != null)
        //        compId = Session["CompId"].ToString();
        //    if (Session["userid"] != null)
        //        userId = Session["userid"].ToString();
        //    if (Session["BranchId"] != null)
        //        branchId = Session["BranchId"].ToString();
        //    if (Session["Language"] != null)
        //        language = Session["Language"].ToString();

        //    qualityAnalysisModel.Title = title;
        //    DataTable dtQaData = _qaService.GetQualityAnalysisReport(compId, branchId,qualityAnalysisModel.QcType,"", qualityAnalysisModel.FromDate,qualityAnalysisModel.ToDate, "").Tables[0];
        //    ViewBag.QAReport = dtQaData;
        //    ViewBag.QualityAnalysisDetail = _qaService.GetQADetailsByItemId(compId, branchId, qualityAnalysisModel.QcType, "", qualityAnalysisModel.FromDate, qualityAnalysisModel.ToDate, "").Tables[0];
        //    ViewBag.MenuPageName = getDocumentName();
        //    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/QualityAnalysis/QualityAnalysis.cshtml", qualityAnalysisModel);
        //}

        public ActionResult SearchQualityAnalysisDetailsByItemId(string itemId, string srcType, string fromDate, string toDate,string DocId)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    branchId = Session["BranchId"].ToString();
               
                QualityAnalysisModel qaModel = new QualityAnalysisModel();
                ViewBag.QualityAnalysisDetail = _qaService.GetQADetailsByItemId(compId, branchId,srcType,itemId,fromDate,toDate,"", DocId);
                qaModel.QAFilter = "QAFilter";
                return PartialView("~/Areas/Common/Views/PartialQCDetailForQualityAnalysis.cshtml", qaModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult SearchQualityAnalysisDetails(string itemId,string srcType, string fromDate, string toDate, string DocId)
        {
            try
           {
                QualityAnalysisModel qualityAnalysisModel = new QualityAnalysisModel();
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    branchId = Session["BranchId"].ToString();

                qualityAnalysisModel.Title = title;
                ViewBag.QualityAnalysisDetail = _qaService.GetQADetailsByItemId(compId, branchId, srcType,itemId, fromDate, toDate, "", DocId);
                qualityAnalysisModel.QAFilter = "QAFilter";

                // return PartialView("~/Areas/Common/Views/PartialQCDetailForQualityAnalysis.cshtml", qaModel);
                //Areas/ApplicationLayer/Views/Shared/
                return View("~/Areas/ApplicationLayer/Views/Shared/PartialQualityAnalysisDetails.cshtml", qualityAnalysisModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult SearchQualityAnalysisSummary(string itemId, string srcType, string fromDate, string toDate,string DocId)
        {
            try
            {
                QualityAnalysisModel qualityAnalysisModel = new QualityAnalysisModel();
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    branchId = Session["BranchId"].ToString();

                DataTable dtQaData = _qaService.GetQualityAnalysisReport(compId, branchId, srcType, itemId, fromDate, toDate, "", DocId);
                qualityAnalysisModel.Title = title;
                ViewBag.QAReport = dtQaData;
                qualityAnalysisModel.QAFilter = "QAFilter";
                return View("~/Areas/ApplicationLayer/Views/Shared/PartialQualityAnalysisSummary.cshtml", qualityAnalysisModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private string getDocumentName()
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["Language"] != null)
                    language = Session["Language"].ToString();
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(compId, DocumentMenuId, language);
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