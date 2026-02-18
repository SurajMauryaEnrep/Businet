using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.BudgetAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.FinanceBudget;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BudgetAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.BudgetAnalysis
{
    public class BudgetAnalysisController : Controller
    {
        string DocumentMenuId = "105104135155", title;
        string CompID, BrId, language = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly BudgerAnalysis_IService _budgetAnalysisIservice;
        private readonly Financebudget_Iservices _financebudget_Iservices;
        public BudgetAnalysisController(Common_IServices _Common_IServices,
            BudgerAnalysis_IService budgetAnalysisIservice, Financebudget_Iservices financebudget_Iservices)
        {
            this._Common_IServices = _Common_IServices;
            _budgetAnalysisIservice = budgetAnalysisIservice;
            _financebudget_Iservices = financebudget_Iservices;
        }
        // GET: ApplicationLayer/BudgetAnalysis
        public DataSet GetBudgetAnalysisReport(string action, string glAccId, string finYear, string quarter, string month)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                DataSet dt = _budgetAnalysisIservice.GetBudgerAnalysisReport(action, CompID, BrId, glAccId, finYear, quarter, month);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataSet GetBudgetAllocationReport(string action, string glAccId, string finYear, string qtr, string mnth,int rev_no)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                DataSet ds = _budgetAnalysisIservice.GetBudgetAllocationReport(action, CompID, BrId, glAccId, finYear, qtr, mnth,rev_no);
                return ds;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult BudgetAnalysisDetail(BudgetAnalysisModel _bgtAnalysisModel, string command)
        {
            try
            {
                if (_bgtAnalysisModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "CsvPrint":
                        return ExportBudgetAnalysisData(_bgtAnalysisModel);
                    default:
                        return new EmptyResult();
                        //return new EmptyResult();

                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public FileResult ExportBudgetAnalysisData(BudgetAnalysisModel _bgtAnalysisModel)
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
                DataTable dt1 = new DataTable();
                JArray jObject = JArray.Parse(_bgtAnalysisModel.BudgetAnalysisData);
                string action = jObject[0]["action"].ToString();
                string glAccId = jObject[0]["glAccId"].ToString();
                string finYear = jObject[0]["finYear"].ToString();
                string quarter = jObject[0]["qtrName"].ToString();
                string month = jObject[0]["mnthName"].ToString();

                DataSet Ds = _budgetAnalysisIservice.GetBudgerAnalysisReport(action, CompID, BrId, glAccId, finYear, quarter, month);

                DataTable dt = new DataTable();
                //List<ARList> _ARListDetail = new List<ARList>();
                if(action == "M")
                {
                    dt = Ds.Tables[1];
                }
                else
                {
                    dt = Ds.Tables[0];
                }
                if (action == "Y")
                {
                    List<BudgetAnalysisListYearly> _BB_Detail_List = new List<BudgetAnalysisListYearly>();
                    if (dt.Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            BudgetAnalysisListYearly bb_list = new BudgetAnalysisListYearly();
                            bb_list.SrNo = rowno + 1;
                            bb_list.acc_name = row["acc_name"].ToString();
                            bb_list.acc_Group_name = row["AccGroupName"].ToString();
                            bb_list.Budget_amount = row["BudgetAmount"].ToString();
                            bb_list.Actual_amount = row["ActualAmount"].ToString();
                            bb_list.Variance = row["Variance"].ToString();
                            _BB_Detail_List.Add(bb_list);
                            rowno = rowno + 1;
                        }
                    }
                    var ItemListData = (from tempitem in _BB_Detail_List select tempitem);
                    string searchValue = _bgtAnalysisModel.searchValue;
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData = ItemListData.Where(m => m.acc_name.ToUpper().Contains(searchValue) || m.acc_Group_name.ToUpper().Contains(searchValue)
                        || m.Budget_amount.ToUpper().Contains(searchValue) || m.Actual_amount.ToUpper().Contains(searchValue) || m.Variance.ToUpper().Contains(searchValue));
                    }
                    var data = ItemListData.ToList();
                    _bgtAnalysisModel.hdnCSVPrint = null;
                    dt1 = ToBudgetAnalysisDetailExl(data);
                }
                else if(action == "Q")
                {
                    List<BudgetAnalysisListQuarterly> _BB_Detail_List = new List<BudgetAnalysisListQuarterly>();
                    if (dt.Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            BudgetAnalysisListQuarterly bb_list = new BudgetAnalysisListQuarterly();
                            bb_list.SrNo = rowno + 1;
                            bb_list.acc_name = row["acc_name"].ToString();
                            bb_list.acc_Group_name = row["AccGroupName"].ToString();
                            bb_list.Budget_amountQ1 = row["Q1BudgetAmount"].ToString();
                            bb_list.Actual_amountQ1 = row["Q1Amt"].ToString();
                            bb_list.VarianceQ1 = row["Q1Variance"].ToString();
                            bb_list.Budget_amountQ2 = row["Q2BudgetAmount"].ToString();
                            bb_list.Actual_amountQ2 = row["Q2Amt"].ToString();
                            bb_list.VarianceQ2 = row["Q2Variance"].ToString();
                            bb_list.Budget_amountQ3 = row["Q3BudgetAmount"].ToString();
                            bb_list.Actual_amountQ3 = row["Q3Amt"].ToString();
                            bb_list.VarianceQ3 = row["Q3Variance"].ToString();
                            bb_list.Budget_amountQ4 = row["Q4BudgetAmount"].ToString();
                            bb_list.Actual_amountQ4 = row["Q4Amt"].ToString();
                            bb_list.VarianceQ4 = row["Q4Variance"].ToString();
                            _BB_Detail_List.Add(bb_list);
                            rowno = rowno + 1;
                        }
                    }
                    var ItemListData = (from tempitem in _BB_Detail_List select tempitem);
                    string searchValue = _bgtAnalysisModel.searchValue;
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData = ItemListData.Where(m => m.acc_name.ToUpper().Contains(searchValue) || m.acc_Group_name.ToUpper().Contains(searchValue)
                        || m.Budget_amountQ1.ToUpper().Contains(searchValue) || m.Actual_amountQ1.ToUpper().Contains(searchValue) || m.VarianceQ1.ToUpper().Contains(searchValue)
                        || m.Budget_amountQ2.ToUpper().Contains(searchValue) || m.Actual_amountQ2.ToUpper().Contains(searchValue) || m.VarianceQ2.ToUpper().Contains(searchValue)
                        || m.Budget_amountQ3.ToUpper().Contains(searchValue) || m.Actual_amountQ3.ToUpper().Contains(searchValue) || m.VarianceQ3.ToUpper().Contains(searchValue)
                        || m.Budget_amountQ4.ToUpper().Contains(searchValue) || m.Actual_amountQ4.ToUpper().Contains(searchValue) || m.VarianceQ4.ToUpper().Contains(searchValue));
                    }
                    var data = ItemListData.ToList();
                    _bgtAnalysisModel.hdnCSVPrint = null;
                    dt1 = ToBudgetAnalysisDetailQuarterlyExl(data);
                }
                else if(action == "M")
                {
                    List<BudgetAnalysisListMonthly> _BB_Detail_List = new List<BudgetAnalysisListMonthly>();
                    if (dt.Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            BudgetAnalysisListMonthly bb_list = new BudgetAnalysisListMonthly();
                            bb_list.SrNo = rowno + 1;
                            bb_list.acc_name = row["acc_name"].ToString();
                            bb_list.acc_Group_name = row["AccGroupName"].ToString();
                            bb_list.Budget_amount_M1 = row["M1BudgetAmount"].ToString();
                            bb_list.Actual_amount_M1 = row["M1Amt"].ToString();
                            bb_list.Variance_M1 = row["M1Variance"].ToString();
                            bb_list.Budget_amount_M2 = row["M2BudgetAmount"].ToString();
                            bb_list.Actual_amount_M2 = row["M2Amt"].ToString();
                            bb_list.Variance_M2 = row["M2Variance"].ToString();
                            bb_list.Budget_amount_M3 = row["M3BudgetAmount"].ToString();
                            bb_list.Actual_amount_M3 = row["M3Amt"].ToString();
                            bb_list.Variance_M3 = row["M3Variance"].ToString();
                            bb_list.Budget_amount_M4 = row["M4BudgetAmount"].ToString();
                            bb_list.Actual_amount_M4 = row["M4Amt"].ToString();
                            bb_list.Variance_M4 = row["M4Variance"].ToString();
                            bb_list.Budget_amount_M5 = row["M5BudgetAmount"].ToString();
                            bb_list.Actual_amount_M5 = row["M5Amt"].ToString();
                            bb_list.Variance_M5 = row["M5Variance"].ToString();
                            bb_list.Budget_amount_M6 = row["M6BudgetAmount"].ToString();
                            bb_list.Actual_amount_M6 = row["M6Amt"].ToString();
                            bb_list.Variance_M6 = row["M6Variance"].ToString();
                            bb_list.Budget_amount_M7 = row["M7BudgetAmount"].ToString();
                            bb_list.Actual_amount_M7 = row["M7Amt"].ToString();
                            bb_list.Variance_M7 = row["M7Variance"].ToString();
                            bb_list.Budget_amount_M8 = row["M8BudgetAmount"].ToString();
                            bb_list.Actual_amount_M8 = row["M8Amt"].ToString();
                            bb_list.Variance_M8 = row["M8Variance"].ToString();
                            bb_list.Budget_amount_M9 = row["M9BudgetAmount"].ToString();
                            bb_list.Actual_amount_M9 = row["M9Amt"].ToString();
                            bb_list.Variance_M9 = row["M9Variance"].ToString();
                            bb_list.Budget_amount_M10 = row["M10BudgetAmount"].ToString();
                            bb_list.Actual_amount_M10 = row["M10Amt"].ToString();
                            bb_list.Variance_M10 = row["M10Variance"].ToString();
                            bb_list.Budget_amount_M11 = row["M11BudgetAmount"].ToString();
                            bb_list.Actual_amount_M11 = row["M11Amt"].ToString();
                            bb_list.Variance_M11 = row["M11Variance"].ToString();
                            bb_list.Budget_amount_M12 = row["M12BudgetAmount"].ToString();
                            bb_list.Actual_amount_M12 = row["M12Amt"].ToString();
                            bb_list.Variance_M12 = row["M12Variance"].ToString();
                            _BB_Detail_List.Add(bb_list);
                            rowno = rowno + 1;
                        }
                    }
                    var ItemListData = (from tempitem in _BB_Detail_List select tempitem);
                    string searchValue = _bgtAnalysisModel.searchValue;
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData = ItemListData.Where(m => m.acc_name.ToUpper().Contains(searchValue) || m.acc_Group_name.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M1.ToUpper().Contains(searchValue) || m.Actual_amount_M1.ToUpper().Contains(searchValue) || m.Variance_M1.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M2.ToUpper().Contains(searchValue) || m.Actual_amount_M2.ToUpper().Contains(searchValue) || m.Variance_M2.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M3.ToUpper().Contains(searchValue) || m.Actual_amount_M3.ToUpper().Contains(searchValue) || m.Variance_M3.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M4.ToUpper().Contains(searchValue) || m.Actual_amount_M4.ToUpper().Contains(searchValue) || m.Variance_M4.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M5.ToUpper().Contains(searchValue) || m.Actual_amount_M5.ToUpper().Contains(searchValue) || m.Variance_M5.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M6.ToUpper().Contains(searchValue) || m.Actual_amount_M6.ToUpper().Contains(searchValue) || m.Variance_M6.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M7.ToUpper().Contains(searchValue) || m.Actual_amount_M7.ToUpper().Contains(searchValue) || m.Variance_M7.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M8.ToUpper().Contains(searchValue) || m.Actual_amount_M8.ToUpper().Contains(searchValue) || m.Variance_M8.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M9.ToUpper().Contains(searchValue) || m.Actual_amount_M9.ToUpper().Contains(searchValue) || m.Variance_M9.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M10.ToUpper().Contains(searchValue) || m.Actual_amount_M10.ToUpper().Contains(searchValue) || m.Variance_M10.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M11.ToUpper().Contains(searchValue) || m.Actual_amount_M11.ToUpper().Contains(searchValue) || m.Variance_M11.ToUpper().Contains(searchValue)
                        || m.Budget_amount_M12.ToUpper().Contains(searchValue) || m.Actual_amount_M12.ToUpper().Contains(searchValue) || m.Variance_M12.ToUpper().Contains(searchValue));
                    }
                    var data = ItemListData.ToList();
                    _bgtAnalysisModel.hdnCSVPrint = null;
                    dt1 = ToBudgetAnalysisDetailMonthlyExl(data);
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Budget Analysis", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToBudgetAnalysisDetailExl(List<BudgetAnalysisListYearly> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr No.", typeof(int));
            dataTable.Columns.Add("GL Account", typeof(string));
            dataTable.Columns.Add("GL Group", typeof(string));
            dataTable.Columns.Add("Budget Amount", typeof(decimal));
            dataTable.Columns.Add("Actual Amount", typeof(decimal));
            dataTable.Columns.Add("Variance (in %)", typeof(decimal));   

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr No."] = item.SrNo;
                rows["GL Account"] = item.acc_name;
                rows["GL Group"] = item.acc_Group_name;
                rows["Budget Amount"] = item.Budget_amount;
                rows["Actual Amount"] = item.Actual_amount;
                rows["Variance (in %)"] = item.Variance;
                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        public DataTable ToBudgetAnalysisDetailQuarterlyExl(List<BudgetAnalysisListQuarterly> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr No.", typeof(int));
            dataTable.Columns.Add("GL Account", typeof(string));
            dataTable.Columns.Add("GL Group", typeof(string));
            dataTable.Columns.Add("Budget Amount (Quarter 1)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (Quarter 1)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (Quarter 1)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (Quarter 2)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (Quarter 2)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (Quarter 2)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (Quarter 3)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (Quarter 3)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (Quarter 3)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (Quarter 4)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (Quarter 4)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (Quarter 4)", typeof(decimal));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr No."] = item.SrNo;
                rows["GL Account"] = item.acc_name;
                rows["GL Group"] = item.acc_Group_name;
                rows["Budget Amount (Quarter 1)"] = item.Budget_amountQ1;
                rows["Actual Amount (Quarter 1)"] = item.Actual_amountQ1;
                rows["Variance (in %) (Quarter 1)"] = item.VarianceQ1;
                rows["Budget Amount (Quarter 2)"] = item.Budget_amountQ2;
                rows["Actual Amount (Quarter 2)"] = item.Actual_amountQ2;
                rows["Variance (in %) (Quarter 2)"] = item.VarianceQ2;
                rows["Budget Amount (Quarter 3)"] = item.Budget_amountQ3;
                rows["Actual Amount (Quarter 3)"] = item.Actual_amountQ3;
                rows["Variance (in %) (Quarter 3)"] = item.VarianceQ3;
                rows["Budget Amount (Quarter 4)"] = item.Budget_amountQ4;
                rows["Actual Amount (Quarter 4)"] = item.Actual_amountQ4;
                rows["Variance (in %) (Quarter 4)"] = item.VarianceQ4;
                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        public DataTable ToBudgetAnalysisDetailMonthlyExl(List<BudgetAnalysisListMonthly> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr No.", typeof(int));
            dataTable.Columns.Add("GL Account", typeof(string));
            dataTable.Columns.Add("GL Group", typeof(string));
            dataTable.Columns.Add("Budget Amount (April)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (April)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (April)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (May)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (May)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (May)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (June)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (June)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (June)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (July)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (July)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (July)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (August)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (August)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (August)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (September)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (September)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (September)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (October)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (October)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (October)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (November)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (November)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (November)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (December)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (December)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (December)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (January)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (January)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (January)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (February)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (February)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (February)", typeof(decimal));
            dataTable.Columns.Add("Budget Amount (March)", typeof(decimal));
            dataTable.Columns.Add("Actual Amount (March)", typeof(decimal));
            dataTable.Columns.Add("Variance (in %) (March)", typeof(decimal));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr No."] = item.SrNo;
                rows["GL Account"] = item.acc_name;
                rows["GL Group"] = item.acc_Group_name;
                rows["Budget Amount (April)"] = item.Budget_amount_M1;
                rows["Actual Amount (April)"] = item.Actual_amount_M1;
                rows["Variance (in %) (April)"] = item.Variance_M1;
                rows["Budget Amount (May)"] = item.Budget_amount_M2;
                rows["Actual Amount (May)"] = item.Actual_amount_M2;
                rows["Variance (in %) (May)"] = item.Variance_M2;
                rows["Budget Amount (June)"] = item.Budget_amount_M3;
                rows["Actual Amount (June)"] = item.Actual_amount_M3;
                rows["Variance (in %) (June)"] = item.Variance_M3;
                rows["Budget Amount (July)"] = item.Budget_amount_M4;
                rows["Actual Amount (July)"] = item.Actual_amount_M4;
                rows["Variance (in %) (July)"] = item.Variance_M4;
                rows["Budget Amount (August)"] = item.Budget_amount_M5;
                rows["Actual Amount (August)"] = item.Actual_amount_M5;
                rows["Variance (in %) (August)"] = item.Variance_M5;
                rows["Budget Amount (September)"] = item.Budget_amount_M6;
                rows["Actual Amount (September)"] = item.Actual_amount_M6;
                rows["Variance (in %) (September)"] = item.Variance_M6;
                rows["Budget Amount (October)"] = item.Budget_amount_M7;
                rows["Actual Amount (October)"] = item.Actual_amount_M7;
                rows["Variance (in %) (October)"] = item.Variance_M7;
                rows["Budget Amount (November)"] = item.Budget_amount_M8;
                rows["Actual Amount (November)"] = item.Actual_amount_M8;
                rows["Variance (in %) (November)"] = item.Variance_M8;
                rows["Budget Amount (December)"] = item.Budget_amount_M9;
                rows["Actual Amount (December)"] = item.Actual_amount_M9;
                rows["Variance (in %) (December)"] = item.Variance_M9;
                rows["Budget Amount (January)"] = item.Budget_amount_M10;
                rows["Actual Amount (January)"] = item.Actual_amount_M10;
                rows["Variance (in %) (January)"] = item.Variance_M10;
                rows["Budget Amount (February)"] = item.Budget_amount_M11;
                rows["Actual Amount (February)"] = item.Actual_amount_M11;
                rows["Variance (in %) (February)"] = item.Variance_M11;
                rows["Budget Amount (March)"] = item.Budget_amount_M12;
                rows["Actual Amount (March)"] = item.Actual_amount_M12;
                rows["Variance (in %) (March)"] = item.Variance_M12;
                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        public ActionResult BudgetAnalysis()
        {
            BudgetAnalysisModel _bgtAnalysisModel = new BudgetAnalysisModel();

            _bgtAnalysisModel.GlAccList = GetGlAccList();
            _bgtAnalysisModel.FinYearList = GetFinYearList();
            List<FyMonthsModel> MnthList = new List<FyMonthsModel>();
            MnthList.Add(new FyMonthsModel { Key = "0", Value = "--Select--" });
            _bgtAnalysisModel.FyMonthslist = MnthList;
            List<FYQuarterModel> QtrList = new List<FYQuarterModel>();
            QtrList.Add(new FYQuarterModel { Key = "0", Value = "--Select--" });
            _bgtAnalysisModel.FYQuarterList = QtrList;
            ViewBag.MenuPageName = getDocumentName();
            _bgtAnalysisModel.Title = title;
            ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/BudgetAnalysis/BudgetAnalysis.cshtml", _bgtAnalysisModel);
        }
        public ActionResult BudgetAnalysisDsahboard(string FromDt, string ToDt)
        {
            BudgetAnalysisModel _bgtAnalysisModel = new BudgetAnalysisModel();

            _bgtAnalysisModel.GlAccList = GetGlAccList();
            _bgtAnalysisModel.FinYearList = GetFinYearList();
           
            var query = _bgtAnalysisModel.FinYearList.Where(arr =>
            {               
                var DT = FromDt.Split('-');
                var DT2 = arr.Value.Split('-');
                var DT1 = DT[2] +"-"+ DT2[1];
                return arr.Value == DT1;
            }).ToList();
            var data = query.Select(x=>x.Key).ToList() ;
            var dataID = query.Select(x => x.Value).ToList();
            var data1 = query.Select(x=>x.Key).ToList() ;
            _bgtAnalysisModel.FinYear = data[0].Trim();
            _bgtAnalysisModel.PeriodDashBoad = data[0].Trim();
            List<FyMonthsModel> MnthList = new List<FyMonthsModel>();
            MnthList.Add(new FyMonthsModel { Key = "0", Value = "--Select--" });
            _bgtAnalysisModel.FyMonthslist = MnthList;
            List<FYQuarterModel> QtrList = new List<FYQuarterModel>();
            QtrList.Add(new FYQuarterModel { Key = "0", Value = "--Select--" });
            _bgtAnalysisModel.FYQuarterList = QtrList;
            ViewBag.MenuPageName = getDocumentName();
            _bgtAnalysisModel.Title = title;

            ViewBag.YearlyBAReport = GetBudgetAnalysisReport("Y", "0", dataID[0], "0", "0").Tables[0];

            ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/


            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/BudgetAnalysis/BudgetAnalysis.cshtml", _bgtAnalysisModel);
        }
        public ActionResult GetYearlyBudgerAnalysisReport(string glAccId, string finYear, string quarter, string month)
        {
            try
            {
                  BudgetAnalysisModel objModel = new BudgetAnalysisModel();
                objModel.Search = "SEARCH";
                ViewBag.YearlyBAReport = GetBudgetAnalysisReport("Y", glAccId, finYear, quarter, month).Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_BudgetAnalysisYearly.cshtml", objModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetQuarterlyBudgerAnalysisReport(string glAccId, string finYear, string quarter, string month)
        {
            try
            {
                ViewBag.Qtr = quarter;
                BudgetAnalysisModel objModel = new BudgetAnalysisModel();
                objModel.Search = "SEARCH";
                ViewBag.QuarterlyBAReport = GetBudgetAnalysisReport("Q", glAccId, finYear, quarter, month).Tables[0];
                if (ViewBag.QuarterlyBAReport.Rows.Count > 0 && quarter == "0" && month != "0")
                    ViewBag.Qtr = ViewBag.QuarterlyBAReport.Rows[0]["Qtr"].ToString();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_BudgetAnalysisQuarterly.cshtml", objModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetMonthlyBudgerAnalysisReport(string glAccId, string finYear, string quarter, string month)
        {
            try
            {
                BudgetAnalysisModel objModel = new BudgetAnalysisModel();
                objModel.Search = "SEARCH";
                DataSet dts = GetBudgetAnalysisReport("M", glAccId, finYear, quarter, month);
                ViewBag.MonthlyBAReport = dts.Tables[1];
                ViewBag.MonthName = dts.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_BudgetAnalysisMonthly.cshtml", objModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetBudgerAnalysisReport(string action, string glAccId, string finYear, string quarter, string month)
        {
            try
            {
                if (action == "Y")
                {
                    return GetYearlyBudgerAnalysisReport(glAccId, finYear, quarter, month);
                }
                else if (action == "Q")
                {
                    return GetQuarterlyBudgerAnalysisReport(glAccId, finYear, quarter, month);
                }
                else if (action == "M")
                {
                    return GetMonthlyBudgerAnalysisReport(glAccId, finYear, quarter, month);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetMonthlyAllocationBudget(string glAccId, string finYear, string qtr, string mnth,int rev_no)/*Add by Hina sharma on 08-10-2024 to remove duplicate data*/
        {
            try
            {
                ViewBag.MonthlyAllocation = GetBudgetAllocationReport("M", glAccId, finYear, qtr, mnth,rev_no).Tables[0];/*Add by Hina sharma on 08-10-2024 to remove duplicate data*/
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISMonthlyAllocation.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetQuarterlyAllocationBudget(string finYear, string glAccId, string qtr, string mnth,int rev_no)
        {
            try
            {
                ViewBag.QuarterlyAllocation = GetBudgetAllocationReport("Q", glAccId, finYear, qtr, mnth,rev_no).Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISQuarterlyAllocation.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetCostCenterAllocationBudget(string finYear, string glAccId,int rev_no)
        {
            try
            {
                DataSet ds = GetBudgetAllocationReport("CC", glAccId, finYear, "", "", rev_no);
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    ViewBag.ccdetail = ds.Tables[0];
                    ViewBag.ccvaldetail = ds.Tables[1];
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISCostCenterAllocation.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
       public ActionResult GetFYLedgerReport(string accId, string fromDate, string toDate)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                DataTable dt = _budgetAnalysisIservice.GetfyLedgerDetails(CompID, BrId, accId, fromDate, toDate);
                ViewBag.LedgerDetails = dt;
                ViewBag.Flag = "List";/*Add by Hina sharma on 10-10-2024*/
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISExpenseDetail.cshtml");
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult GetFYLedgerReportCostCenter(string accId, string fromDate, string toDate,int CCtypeId,int CCNameId)/*Add by Hina sharma on 10-10-2024*/
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                DataTable dt = _budgetAnalysisIservice.GetfyLedgerDetailsCostCentr(CompID, BrId, accId, fromDate, toDate, CCtypeId,  CCNameId);
                ViewBag.LedgerDetails = dt;
                ViewBag.Flag = "CostCenter";/*Add by Hina sharma on 10-10-2024*/
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISExpenseDetail.cshtml");
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
        private List<FinYearModel> GetFinYearList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            List<FinYearModel> fyList = new List<FinYearModel>();
            fyList.Add(new FinYearModel { Key = "0", Value = "---Select---" });
            DataTable dtFY = _budgetAnalysisIservice.GetFinancialYearsList(CompID, BrId);
            foreach (DataRow dtr in dtFY.Rows)
            {
                fyList.Add(new FinYearModel() { Key = dtr["Period"].ToString(), Value = dtr["FinYear"].ToString() });
            }
            return fyList;
        }
        private List<FYQuarterModel> GetQuarterList(string finYear)
        {
            List<FYQuarterModel> QtrList = new List<FYQuarterModel>();
            QtrList.Add(new FYQuarterModel { Key = "0", Value = "--Select--" });
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();

            DataTable dt = _budgetAnalysisIservice.GetFyMonthOrQuarterList("All", CompID, BrId, "Q", finYear, "");
            foreach (DataRow dtr in dt.Rows)
            {
                QtrList.Add(new FYQuarterModel() { Key = dtr["mnth_qtr_name"].ToString(), Value = dtr["mnth_qtr_name"].ToString() });
            }
            return QtrList;
        }
        public JsonResult GetQtrList(string finYear)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();

            DataTable dt = _budgetAnalysisIservice.GetFyMonthOrQuarterList("All", CompID, BrId, "Q", finYear, "");
            return Json(JsonConvert.SerializeObject(dt));
        }
        private List<FyMonthsModel> GetMonthsList(string finYear, string qtrName)
        {
            List<FyMonthsModel> QtrList = new List<FyMonthsModel>();
            QtrList.Add(new FyMonthsModel { Key = "0", Value = "--Select--" });
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            //default action all to get all months of financial year
            string action = "All";
            //change action to quarter wise if quarter wise filter is applied by user
            if (qtrName != "0" && !string.IsNullOrEmpty(qtrName))
                action = "QTRWISE";
            DataTable dt = _budgetAnalysisIservice.GetFyMonthOrQuarterList(action, CompID, BrId, "M", finYear, "");
            foreach (DataRow dtr in dt.Rows)
            {
                QtrList.Add(new FyMonthsModel() { Key = dtr["mnth_qtr_name"].ToString(), Value = dtr["mnth_qtr_name"].ToString() });
            }
            return QtrList;
        }
        public JsonResult GetMnthsList(string finYear, string qtrName)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            //default action all to get all months of financial year
            string action = "All";
            //change action to quarter wise if quarter wise filter is applied by user
            if (qtrName != "0" && !string.IsNullOrEmpty(qtrName))
                action = "QTRWISE";
            DataTable dt = _budgetAnalysisIservice.GetFyMonthOrQuarterList(action, CompID, BrId, "M", finYear, qtrName);

            return Json(JsonConvert.SerializeObject(dt));
        }
        private List<GlAccountModel> GetGlAccList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            List<GlAccountModel> GlAccList = new List<GlAccountModel>();
            GlAccList.Add(new GlAccountModel { acc_id = "0", acc_name = "---Select---" });
            DataTable dt = _financebudget_Iservices.GlList(CompID);
            foreach (DataRow dtr in dt.Rows)
            {
                GlAccList.Add(new GlAccountModel() { acc_id = dtr["acc_id"].ToString(), acc_name = dtr["acc_name"].ToString() });
            }
            return GlAccList;
        }
    }
}