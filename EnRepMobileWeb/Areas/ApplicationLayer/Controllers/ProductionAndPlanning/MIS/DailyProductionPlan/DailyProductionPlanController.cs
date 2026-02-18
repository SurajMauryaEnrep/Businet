using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.DailyProductionPlan;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MIS.DailyProductionPlan;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.Areas.Common.Controllers.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.MIS.DailyProductionPlan
{
    public class DailyProductionPlanController : Controller
    {
        string DocumentMenuId = "105105155130", title;
        string CompID,BrId, language = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        DailyProductionPlan_ISERVICES _DailyProductionPlan_ISERVICES;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public DailyProductionPlanController(Common_IServices _Common_IServices, DailyProductionPlan_ISERVICES dailyProductionPlan_ISERVICES, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._DailyProductionPlan_ISERVICES = dailyProductionPlan_ISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/DailyProductionPlan
        public ActionResult DailyProductionPlan()
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                DataSet ds = new DataSet();
                DailyProductionPlan_Model _model = new DailyProductionPlan_Model();
                _model.Title = title;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                BindOperationNameList(_model);
                DateTime date = DateTime.Now;
                /*Commented By Nitesh 06-12-2023 from_date fincial start Date not month start date*/
                //string FromDate = new DateTime(date.Year,date.Month,1).ToString("yyyy-MM-dd");
                string ToDate = date.ToString("yyyy-MM-dd");
                DataSet dttbl = new DataSet();
                #region Added By Nitesh  02-01-2024 for Financial Year 
                #endregion
                dttbl = GetFyList();
                if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
                {
                    _model.DR_fromDt = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                    ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                    ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                    ViewBag.fylist = dttbl.Tables[1];
                }
                string FromDate = _model.DR_fromDt;

                _model.DR_ToDt = ToDate;
                ds = _DailyProductionPlan_ISERVICES.GetDailyProductPlanDetails(CompID, BrId, FromDate, ToDate, null, null);
                ViewBag.DailyPPDetailDates = ds.Tables[0];
                ViewBag.DailyPPDetailProducts = ds.Tables[1];
                ViewBag.DailyPPDetailScheduleDetail = ds.Tables[2];
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/DailyProductionPlan/DailyProductionPlan.cshtml", _model);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
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
        public ActionResult GetFilteredDailyProductionPlan(string ProductId,string DR_fromDt,string DR_ToDt,string ddlOpId)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                //BindOperationNameList(_model);
                DataSet ds = _DailyProductionPlan_ISERVICES.GetDailyProductPlanDetails(CompID, BrId, DR_fromDt, DR_ToDt,ProductId, ddlOpId);
                ViewBag.DailyPPDetailDates = ds.Tables[0];
                ViewBag.DailyPPDetailProducts = ds.Tables[1];
                ViewBag.DailyPPDetailScheduleDetail = ds.Tables[2];
                ViewBag.Search = true;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDailyProductionPlan.cshtml");
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public ActionResult GetDPP_PlannedQtyDetail(string ProductId, string PlanDt)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                DataSet ds = _DailyProductionPlan_ISERVICES.GetDailyProductPlanPlannedQtyDetails(CompID, BrId, PlanDt, ProductId);
                ViewBag.DailyPP_PlannedQtyDetail = ds.Tables[0];
                ViewBag.Search = true;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDailyPlannedDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public ActionResult GetDPP_ProducedQtyDetail(string ProductId, string ProducedDt)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                DataSet ds = _DailyProductionPlan_ISERVICES.GetDailyProductPlanProducedQtyDetails(CompID, BrId, ProducedDt, ProductId);
                ViewBag.DailyPP_ProducedQtyDetail = ds.Tables[0];
                ViewBag.Search = true;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDailyProductionDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public void BindOperationNameList(DailyProductionPlan_Model _model)
        {
            DataTable dt = new DataTable();
            try
            {
                string Comp_ID = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    dt = _DailyProductionPlan_ISERVICES.GetOperationNameList(Convert.ToInt32(Comp_ID));
                    List<OperationName> _Status = new List<OperationName>();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow data in dt.Rows)
                        {
                            OperationName _Statuslist = new OperationName();
                            _Statuslist.op_id = data["op_id"].ToString();
                            _Statuslist.op_name = data["op_name"].ToString();
                            _Status.Add(_Statuslist);
                        }
                    }
                    _Status.Insert(0, new OperationName() { op_id = "0", op_name = "---Select---" });/*Add by Hina on 13-09-2024*/

                    _model.OperationNameList = _Status;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }

        public ActionResult DailyProductionPlanPrintCSV(DailyProductionPlan_Model _Model, string Command)/*add by Hina Sharma on 24-09-2025*/
        {
            try
            {

                if (_Model.hdnCSVPrint == "CSV")
                {
                    Command = "CSV";
                }
                switch (Command)
                {
                    case "CSV":
                        return GenrateCsvFile(_Model);
                }
                return RedirectToAction("DailyProductionPlan");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public FileResult GenrateCsvFile(DailyProductionPlan_Model _Model)/*add by Hina Sharma on 24-09-2025*/
        {

            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string BrId = string.Empty;
                string ProductId, DR_fromDt, DR_ToDt, ddlOpId;
                int DtRange = 0; // initialize
                //DateTime fromDate="2025-04-01", 
                DateTime toDate;
                DateTime fromDate = DateTime.Parse("2025-04-01");
                //DateTime toDate = DateTime.Now;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                

                if (!string.IsNullOrEmpty(_Model.Filters))
                {
                    string[] fdata = _Model.Filters.Split(',');

                    if (fdata.Length >= 4)
                    {
                        ProductId = fdata[0];
                        DR_fromDt = fdata[1];
                        DR_ToDt = fdata[2];
                        ddlOpId = fdata[3];

                        if (DateTime.TryParse(DR_fromDt, out fromDate) && DateTime.TryParse(DR_ToDt, out toDate))
                        {
                            DtRange = (toDate - fromDate).Days;   // difference in days
                        }

                        DataSet ds = _DailyProductionPlan_ISERVICES.GetDailyProductPlanDetails(
                                        CompID, BrId, DR_fromDt, DR_ToDt, ProductId, ddlOpId);

                        ViewBag.DailyPPDetailDates = ds.Tables[0];
                        ViewBag.DailyPPDetailProducts = ds.Tables[1];
                        ViewBag.DailyPPDetailScheduleDetail = ds.Tables[2];
                    }
                }

                // Creating First Header Table
                DataTable FirstTableHeader = new DataTable();

                FirstTableHeader.Columns.Add("", typeof(string));
                FirstTableHeader.Columns.Add("", typeof(string));
                FirstTableHeader.Columns.Add("", typeof(string));
                
                if (DtRange > 0 && fromDate != DateTime.MinValue)
                {
                    for (int i = 0; i <= DtRange; i++)   // include both start & end date
                    {
                        string colName = fromDate.AddDays(i).ToString("dd-MM-yyyy");
                        FirstTableHeader.Columns.Add(colName, typeof(string)); // string safer for display
                        FirstTableHeader.Columns.Add("", typeof(string)); // string safer for display
                    }
                    DataRow newRow = FirstTableHeader.NewRow();
                    foreach(DataColumn clm in FirstTableHeader.Columns)
                    {
                        if (clm.ColumnName.StartsWith("Col"))
                        {
                            newRow[clm.ColumnName] = "";
                        }
                        else
                        {
                            newRow[clm.ColumnName] = clm.ColumnName;
                        }
                        
                    }
                    FirstTableHeader.Rows.Add(newRow);

                }
                // Creating Second Header Table
                DataTable SecondTableHeader = new DataTable();
                SecondTableHeader.Columns.Add("Sr.No.", typeof(string));
                SecondTableHeader.Columns.Add("Product Name", typeof(string));
                SecondTableHeader.Columns.Add("UOM", typeof(string));
                if (ViewBag.DailyPPDetailDates != null)
                {
                    for (int i = 0; i <= DtRange; i++)   // include both start & end date
                    {
                        string spaces = new string(' ', i + 1);
                        SecondTableHeader.Columns.Add("Planned" + spaces, typeof(string));   // internal unique name
                        SecondTableHeader.Columns.Add("Produced" + spaces, typeof(string));  // internal unique name
                        foreach (DataColumn col in SecondTableHeader.Columns)
                        {
                            if (col.ColumnName.StartsWith("Planned"))
                                col.Caption = "Planned";
                            else if (col.ColumnName.StartsWith("Produced"))
                                col.Caption = "Produced";
                        }
                    }
                }
                // Adding Data
                if (ViewBag.DailyPPDetailProducts != null)
                {
                    DataTable scheduleDetail = ViewBag.DailyPPDetailScheduleDetail as DataTable;
                    DataTable dateTable = ViewBag.DailyPPDetailDates as DataTable;

                    // ========================
                    // 1. Define Columns First
                    // ========================
                    if (SecondTableHeader.Columns.Count == 0)   // avoid duplicate add
                    {
                        SecondTableHeader.Columns.Add("Sr.No.", typeof(string));
                        SecondTableHeader.Columns.Add("Product Name", typeof(string));
                        SecondTableHeader.Columns.Add("UOM", typeof(string));

                        if (dateTable != null)
                        {
                            int colIndex = 0;
                            foreach (DataRow drDate in dateTable.Rows)
                            {
                                string spaces = new string(' ', colIndex + 1); // change +1 to more if you want more spaces
                                SecondTableHeader.Columns.Add("Planned" + spaces, typeof(string));
                                SecondTableHeader.Columns.Add("Produced" + spaces, typeof(string));
                                //SecondTableHeader.Columns.Add("Planned " + colIndex, typeof(string));
                                //SecondTableHeader.Columns.Add("Produced " + colIndex, typeof(string));
                                colIndex++;
                            }
                        }
                    }

                    // ========================
                    // 2. Fill Data
                    // ========================
                    foreach (DataRow dr in ViewBag.DailyPPDetailProducts.Rows)
                    {
                        DataRow newRow = SecondTableHeader.NewRow();
                        newRow["Sr.No."] = dr["SrNo"].ToString();
                        newRow["Product Name"] = dr["item_name"].ToString();
                        newRow["UOM"] = dr["uom_alias"].ToString();

                        if (dateTable != null && scheduleDetail != null)
                        {
                            int colIndex = 0;
                            foreach (DataRow drDate in dateTable.Rows)
                            {
                                string currentDate = drDate["date"].ToString();

                                // filter only on item_id with date (safe fetch)
                                var sched = scheduleDetail.AsEnumerable()
                                    .FirstOrDefault(r =>r["op_out_item_id"].ToString() == dr["op_out_item_id"].ToString()
                                   && r["date"].ToString() == currentDate);

                                if (sched != null)
                                {
                                    string spaces = new string(' ', colIndex + 1); // change +1 to more if you want more spaces
                                    newRow["Planned" + spaces] = sched["prod_qty"].ToString();
                                    newRow["Produced" + spaces] = sched["produced_qty"].ToString();
                                    //newRow["Planned " + colIndex] = sched["prod_qty"].ToString();
                                    //newRow["Produced " + colIndex] = sched["produced_qty"].ToString();
                                }

                                colIndex++;
                            }
                        }
                        
                        SecondTableHeader.Rows.Add(newRow);
                        
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Daily Production Plan", SecondTableHeader, FirstTableHeader);
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
    }

}