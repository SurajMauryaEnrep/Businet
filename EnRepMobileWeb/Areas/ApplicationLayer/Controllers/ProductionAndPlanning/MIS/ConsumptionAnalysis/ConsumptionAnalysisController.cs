using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MIS.ConsumptionAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.ConsumptionAnalysis;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.MIS.ConsumptionAnalysis
{
    public class ConsumptionAnalysisController : Controller
    {
        string CompID, language, BrID, title = String.Empty;
        string DocumentMenuId = "105105155110";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ConsumptionAnalysis_ISERVICES analysis_ISERVICES;
        private readonly ShopfloorSetup_ISERVICES _ShopfloorSetup_ISERVICES;
        private readonly OperationSetup_ISERVICES _OperationSetup_ISERVICES;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public ConsumptionAnalysisController(Common_IServices _Common_IServices, ConsumptionAnalysis_ISERVICES analysis_ISERVICES,
            ShopfloorSetup_ISERVICES ShopfloorSetup_ISERVICES, OperationSetup_ISERVICES OperationSetup_ISERVICES, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.analysis_ISERVICES = analysis_ISERVICES;
            _ShopfloorSetup_ISERVICES = ShopfloorSetup_ISERVICES;
            _OperationSetup_ISERVICES = OperationSetup_ISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/ConsumptionAnalysis
        public ActionResult ConsumptionAnalysis()
        {
            ViewBag.MenuPageName = getDocumentName();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
            }
            ConsumptionAnalysis_Model analysis_Model = new ConsumptionAnalysis_Model();
            DateTime dtnow = DateTime.Now;
            /*Commented By Nitesh 06-12-2023 from_date fincial start Date not month start date*/
            //string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
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
            string ToDate = dtnow.ToString("yyyy-MM-dd");
            analysis_Model.To_dt = ToDate;
           // analysis_Model.From_dt = FromDate;
            analysis_Model.ShopFloorList = GetShflList(Convert.ToInt32(CompID), Convert.ToInt32(BrID));
            analysis_Model.OperationList = GetOperationList(Convert.ToInt32(CompID));
            ViewBag.ConsumptionMaterialList = GetConsumption_Details("", FromDate, ToDate, "", "Material List","","");
            //Session["CAFilter"] = null;
            analysis_Model.CAFilter = null;
            analysis_Model.Title = title;
            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/ConsumptionAnalysis/ConsumptionAnalysis.cshtml", analysis_Model);
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
        public List<ShopFloor> GetShflList(int compId, int brId)
        {
            List<ShopFloor> shflList = new List<ShopFloor>();
            ShopFloor shfl = new ShopFloor()
            {
                shfl_id = "0",
                shfl_name = "----ALL----"
            };
            shflList.Add(shfl);
            DataTable dt = _ShopfloorSetup_ISERVICES.GetShopFloorDetailsDAL(compId,brId);
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
        public DataTable GetConsumption_Details(string ProductID, string From_dt, string To_dt, string Group, string Flag, string shflId, string opId)
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

                dt = analysis_ISERVICES.GetConsumption_Details(CompID, BrID, ProductID, From_dt, To_dt, Group, Flag, shflId,opId).Tables[0];
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public ActionResult GetLotDetail(string cnf_no, string cnf_dt, string Product_Id, string Material_item_id)
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
                ViewBag.ConsumptionLotDetail = analysis_ISERVICES.GetLotDetail(CompID, BrID, cnf_no, cnf_dt, Product_Id, Material_item_id).Tables[0];
                return PartialView("~/Areas/Common/Views/PartialLotDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public ActionResult GetConsumedMeterialDetail(string ItemId, string From_dt, string To_dt, string Group,string shflId, string opId)
        {
            try
            {

                ViewBag.ConsumptionMaterialDetails = GetConsumption_Details(ItemId, From_dt, To_dt, Group, "Material Detail",shflId,opId);
                return PartialView("~/Areas/Common/Views/PartialConsumptionDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        public ActionResult GetConsumption_DetailsByFilter(string ItemName, string txtFromdate, string txtTodate, string Groupname, string shflId, string opId)
        {
            try
            {
                ConsumptionAnalysis_Model analysis_Model = new ConsumptionAnalysis_Model();
                ViewBag.ConsumptionMaterialList = GetConsumption_Details(ItemName, txtFromdate, txtTodate, Groupname, "Material List", shflId,opId);
                //Session["CAFilter"] = "CAFilter";
                analysis_Model.CAFilter = "CAFilter";
                // DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/ConsumptionAnalysisDetailList.cshtml", analysis_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

        }
        public JsonResult GetGroupNameInDDL()
        {
            try
            {
                //JsonResult DataRows = null;
                Dictionary<string, string> ItemList = new Dictionary<string, string>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataTable GroupList = analysis_ISERVICES.ItemGroupListDAL("", CompID);
                // DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/
                DataRow dr;
                dr = GroupList.NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                GroupList.Rows.InsertAt(dr, 0);
                if (GroupList.Rows.Count > 0)
                {
                    for (int i = 0; i < GroupList.Rows.Count; i++)
                    {
                        ItemList.Add(GroupList.Rows[i]["item_grp_id"].ToString(), GroupList.Rows[i]["ItemGroupChildNood"].ToString());
                    }
                }
                return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
                // return DataRows;
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
        //            DataSet ProductList = analysis_ISERVICES.BindProductNameInDDL(CompID, BrID);
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

    }

}