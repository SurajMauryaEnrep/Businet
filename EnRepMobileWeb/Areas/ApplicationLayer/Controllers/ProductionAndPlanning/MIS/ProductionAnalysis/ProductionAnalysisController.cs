using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MIS.ProductionAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.ProductionAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.OperationSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ShopfloorSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.MIS.ProductionAnalysis
{
    public class ProductionAnalysisController : Controller
    {
        string CompID, language, userid, BrID, title = String.Empty;
        string DocumentMenuId = "105105155105";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ProductionAnalysis_ISERVICES analysis_ISERVICES;
        private readonly ShopfloorSetup_ISERVICES _ShopfloorSetup_ISERVICES;
        private readonly OperationSetup_ISERVICES _OperationSetup_ISERVICES;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public ProductionAnalysisController(Common_IServices _Common_IServices, ProductionAnalysis_ISERVICES analysis_ISERVICES,
              ShopfloorSetup_ISERVICES ShopfloorSetup_ISERVICES, OperationSetup_ISERVICES OperationSetup_ISERVICES, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.analysis_ISERVICES = analysis_ISERVICES;

            _ShopfloorSetup_ISERVICES = ShopfloorSetup_ISERVICES;
            _OperationSetup_ISERVICES = OperationSetup_ISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/ProductionAnalysis
        public ActionResult ProductionAnalysis()
        {
            try
            {
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                ProductionAnalysis_Model analysis_Model = new ProductionAnalysis_Model();
                DateTime dtnow = DateTime.Now;
                /*Commented By Nitesh 06-12-2023 from_date fincial start Date not month start date*/
                //string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            
                string ToDate = dtnow.ToString("yyyy-MM-dd");
                analysis_Model.To_dt = ToDate;
                DataSet dttbl = new DataSet();
                #region Added By Nitesh  02-01-2024 for Financial Year 
                #endregion
                dttbl = GetFyList();
                if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
                {
                    analysis_Model.From_dt = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                    ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                    ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                    ViewBag.fylist = dttbl.Tables[1];
                }
                string FromDate = analysis_Model.From_dt;
               //analysis_Model.From_dt = FromDate;
                ViewBag.ProductionAnalysisList = GetProductionMIS_Details("", FromDate, ToDate, "","","");
                //Session["PAFilter"] = null;
                analysis_Model.PAFilter = null;
                analysis_Model.Title = title;
                analysis_Model.ShopFloorList = GetShflList(Convert.ToInt32(CompID), Convert.ToInt32(BrID));
                analysis_Model.OperationList = GetOperationList(Convert.ToInt32(CompID));
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/ProductionAnalysis/ProductionAnalysis.cshtml", analysis_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
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
                    BrID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrID, userid, DocumentMenuId, language);
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
        public ActionResult showReportFromDashBoard(string FromDt,string ToDt)
        {
            try
            {
                //ViewBag.MenuPageName = getDocumentName();
                CommonPageDetails();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                ProductionAnalysis_Model analysis_Model = new ProductionAnalysis_Model();
                analysis_Model.To_dt = ToDt;
                analysis_Model.From_dt = FromDt;
                ViewBag.ProductionAnalysisList = GetProductionMIS_Details("", FromDt, ToDt, "","","");
                //Session["PAFilter"] = null;
                analysis_Model.PAFilter = null;
                analysis_Model.Title = title;
                analysis_Model.ShopFloorList = GetShflList(Convert.ToInt32(CompID), Convert.ToInt32(BrID));
                analysis_Model.OperationList = GetOperationList(Convert.ToInt32(CompID));
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/ProductionAnalysis/ProductionAnalysis.cshtml", analysis_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public List<ShopFloor> GetShflList(int compId, int brId)
        {
            List<ShopFloor> shflList = new List<ShopFloor>();
            ShopFloor shfl = new ShopFloor()
            {
                shfl_id = "0",
                shfl_name = "----ALL----"
            };
            shflList.Add(shfl);
            DataTable dt = _ShopfloorSetup_ISERVICES.GetShopFloorDetailsDAL(compId, brId);
            foreach (DataRow dtr in dt.Rows)
            {
                shfl = new ShopFloor()
                {
                    shfl_id = dtr["shfl_id"].ToString(),
                    shfl_name = dtr["shfl_name"].ToString()
                };
                shflList.Add(shfl);
            }
            return shflList;
        }
        public List<Operation> GetOperationList(int compId)
        {
            List<Operation> opList = new List<Operation>();
            Operation shfl = new Operation()
            {
                op_id = "0",
                op_name = "----ALL----"
            };
            opList.Add(shfl);
            DataTable dt = _OperationSetup_ISERVICES.GetOperationDetailsDAL(compId);
            foreach (DataRow dtr in dt.Rows)
            {
                shfl = new Operation()
                {
                    op_id = dtr["op_id"].ToString(),
                    op_name = dtr["op_name"].ToString()
                };
                opList.Add(shfl);
            }
            return opList;
        }
        public DataTable GetProductionMIS_Details(string ProductID,string From_dt,string To_dt,string Summary, string shflId, string opId)
        {
            try
            {
                DataTable dt = new DataTable();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                dt = analysis_ISERVICES.GetProductionMIS_Details(CompID, BrID, ProductID, From_dt, To_dt, Summary, shflId,opId).Tables[0];
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
           
        }
        public DataTable GetProductionMIS_DetailsOnClickInfo(string ProductID, string From_dt, string To_dt, string Summary, string shflId, string opId,string flag)
        {
            try
            {
                DataTable dt = new DataTable();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                dt = analysis_ISERVICES.GetProductionMIS_DetailsInfo(CompID, BrID, ProductID, From_dt, To_dt, Summary, shflId,opId, flag).Tables[0];
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetProductionMIS_DetailsByFilter(string ItemName,string txtFromdate,string txtTodate,string txtShowAs, string shflId, string opId)
        {
            try
            {
                CommonPageDetails();
                ProductionAnalysis_Model analysis_Model = new ProductionAnalysis_Model();
                ViewBag.ProductionAnalysisList = GetProductionMIS_Details(ItemName, txtFromdate, txtTodate, txtShowAs,shflId,opId);
                //Session["PAFilter"] = "PAFilter";
                analysis_Model.PAFilter = "PAFilter";
                // DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProductionAnalysisList.cshtml", analysis_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
           
        }
        public ActionResult GetProductionMIS_DetailsOnClick(string ItemName, string txtFromdate, string txtTodate, string txtShowAs, string shflId, string opId, string shflName,string opName,string flag,string Item_Name,string UomAlias,string qty)
        {
            try
            {
                ProductionAnalysis_Model objModel = new ProductionAnalysis_Model();
                objModel.opId = opName;
                objModel.shflId = shflName;
                objModel.ItemName = Item_Name;
                objModel.UOM = UomAlias;
                objModel.Qty = qty;
                ViewBag.ProductionAnalysisDetails = GetProductionMIS_DetailsOnClickInfo(ItemName, txtFromdate, txtTodate, txtShowAs,shflId,opId, flag);
                objModel.InsightType = flag;
               // Session["PAFilter"] = "PAFilter";
                // DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return PartialView("~/Areas/Common/Views/PartialProductionAnalysisDetail.cshtml", objModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

        }
        public ActionResult GetProductionMIS_EstimateValueDetails(string ItemName,string ProduceQty
            , string JcNo,string JcDt, string Cnf_no, string Cnf_dt
            , string From_dt,string To_dt, string shflId, string opId)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                ViewBag.EstimateValueDetails = analysis_ISERVICES.GetProductionMIS_EstimateAndActualValueDetails(CompID,BrID,ItemName,ProduceQty, JcNo, JcDt, Cnf_no,
                    Cnf_dt, From_dt,To_dt,"Estimated", shflId,opId).Tables[0];
                // Session["PAFilter"] = "PAFilter";
                // DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialEstimatedCostList.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetProductionMIS_ActualValueDetails(string ItemName, string ProduceQty,string JcNo, string JcDt, string Cnf_no, string Cnf_dt,
            string From_dt, string To_dt, string shflId, string opId)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                ViewBag.ActualValueDetails = analysis_ISERVICES.GetProductionMIS_EstimateAndActualValueDetails(CompID,BrID,ItemName, 
                    ProduceQty, JcNo, JcDt, Cnf_no, Cnf_dt, From_dt, To_dt,"Actual",shflId,opId).Tables[0];
                // Session["PAFilter"] = "PAFilter";
                // DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialActualCostList.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        //public JsonResult GetProductNameInDDL()
        //{
        //    try
        //    {
        //        JsonResult DataRows = null;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //            if (Session["BranchId"] != null)
        //            {
        //                BrID = Session["BranchId"].ToString();
        //            }
          //          DataSet ProductList = analysis_ISERVICES.BindProductNameInDDL(CompID, BrID);
        //            DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/
        //        }
        //        return DataRows;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
           
        //}
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
        public ActionResult GetItemQCParamDetail(string ItemID, string qc_no, string qc_dt)
        {
            try
            {
                //JsonResult DataRows = null;
                //_QualityInspectionModel = new QualityInspectionModel();
                //if (TransType != null)
                //{
                //    _QualityInspectionModel.TransType = TransType;
                //}
                //if (Command != null)
                //{
                //    _QualityInspectionModel.Command = Command;
                //}
                //if (DocumentStatus != null)
                //{
                //    _QualityInspectionModel.DocumentStatus = DocumentStatus;
                //}
                //List<ItemDetailsQc> _ItemDetailsList = new List<ItemDetailsQc>();
                //List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    BrID = Session["BranchId"].ToString();
                }
                DataSet ds = analysis_ISERVICES.GetItemQCParamDetail(CompID, BrID, ItemID, qc_no, qc_dt);
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