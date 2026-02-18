using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.MISCostCenterAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.MISCostCenterAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.MISCostCenterAnalysis
{
    //Developed by Suraj Maurya on 10-10-2024
    public class MISCostCenterAnalysisController : Controller
    {
        string CompID, BrID, language = String.Empty;
        string DocumentMenuId = "105104135170", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        private readonly MISCostCenterAnalysis_IService _mis_CC_Analysis_IService;
        public MISCostCenterAnalysisController(Common_IServices _Common_IServices, MISCostCenterAnalysis_IService mis_CC_Analysis_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._mis_CC_Analysis_IService = mis_CC_Analysis_IService;
        }
        // GET: ApplicationLayer/MISCostCenterAnalysis
        public ActionResult MISCostCenterAnalysis()
        {
            ViewBag.MenuPageName = getDocumentName();
            MISCostCenterAnalysis_Model cc_analysis_model = new MISCostCenterAnalysis_Model();
            cc_analysis_model.Title = title;
            DataSet ds = new DataSet();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
            }
            ds = _mis_CC_Analysis_IService.GetCCExpRevPageLoadDetail(CompID, BrID);

            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<CostCenterList> cc_list = new List<CostCenterList>();
                    cc_list.Add(new CostCenterList { cc_id = "0", cc_name = "---Select---" });
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cc_list.Add(new CostCenterList { cc_id = dr["cc_id"].ToString(), cc_name = dr["cc_name"].ToString() });
                    }
                    cc_analysis_model.costCenterLists = cc_list;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    List<CostCenterValueList> cc_val_list = new List<CostCenterValueList>();
                    cc_val_list.Add(new CostCenterValueList { cc_val_id = "0", cc_val_name = "---Select---" });
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        cc_val_list.Add(new CostCenterValueList { cc_val_id = dr["cc_val_id"].ToString(), cc_val_name = dr["cc_val_name"].ToString() });
                    }
                    cc_analysis_model.costCenterValueLists = cc_val_list;
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    cc_analysis_model.from_dt = ds.Tables[2].Rows[0]["from_dt"].ToString();
                    cc_analysis_model.to_dt = ds.Tables[2].Rows[0]["to_dt"].ToString();
                }
            }
            ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 20-11-2024 for loader (work on _footer.cshtml)*/

            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/MISCostCenterAnalysis/MISCostCenterAnalysis.cshtml", cc_analysis_model);
        }
        public ActionResult GetCostCenterAnalysisReport(string cc_id, string cc_val_id, string from_dt, string to_dt)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                ds = _mis_CC_Analysis_IService.GetExpanceAndRevanueDs(CompID, BrID, cc_id, cc_val_id, from_dt, to_dt);
                ViewBag.ExpenceDt = ds.Tables[0];
                ViewBag.RevenueDt = ds.Tables[1];
                ViewBag.ProfitAndLoss = ds.Tables[2];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISCostCenterAnalysisList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult GetCostCenterTransactionDetails(string cc_id, string cc_val_id, string from_dt, string to_dt,string acc_id)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                ds = _mis_CC_Analysis_IService.GetCostCenterTransactionDetails(CompID, BrID, cc_id, cc_val_id, from_dt, to_dt, acc_id);
                ViewBag.CostCenterTransactionDetails = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.AccName = ds.Tables[0].Rows[0]["acc_name"].ToString();
                }
                ViewBag.CostCenterTransactionDetailsTotal = ds.Tables[1];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/Partial_MISCostCenterTransactionDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult GetCostCenterValueList(string cc_id)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                ds = _mis_CC_Analysis_IService.GetCostCenterValueListByCostCenter(CompID, BrID, cc_id);
                return Json(JsonConvert.SerializeObject(ds.Tables[0]), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        [HttpPost]
        public ActionResult MISCostCenterAnalysisDetail(MISCostCenterAnalysis_Model cc_analysis_model, string command)
        {
            try
            {
                if (cc_analysis_model.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "CsvPrint":
                        return ExportCostCenterAnalysisData(cc_analysis_model);
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
        public FileResult ExportCostCenterAnalysisData(MISCostCenterAnalysis_Model cc_analysis_model)
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

                DataTable Details = new DataTable();

                Details = ToDataTable(cc_analysis_model.cc_TransactionDetail);
                DataTable dt = new DataTable();
                //List<ARList> _ARListDetail = new List<ARList>();
                List<cc_TransactionDetail> _BB_Detail_List = new List<cc_TransactionDetail>();
                dt = Details;
                if (dt.Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        cc_TransactionDetail bb_list = new cc_TransactionDetail();
                        bb_list.SrNo = rowno + 1;
                        bb_list.Vou_No = row["Vou_No"].ToString();
                        bb_list.Vou_dt = row["Vou_Dt"].ToString();
                        bb_list.cc_vou_amt_bs = row["cc_vou_amt_bs"].ToString();
                        bb_list.cc_vou_amt_sP = row["cc_vou_amt_sp"].ToString();
                        bb_list.amt_type = row["amt_type"].ToString();
                        bb_list.curr_logo = row["curr_logo"].ToString();
                        bb_list.conv_rate = row["conv_rate"].ToString();
                        bb_list.nurr = row["narr"].ToString();
                        _BB_Detail_List.Add(bb_list);
                        rowno = rowno + 1;
                    }
                }
                var ItemListData = (from tempitem in _BB_Detail_List select tempitem);
                string searchValue = cc_analysis_model.searchValue;
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    ItemListData = ItemListData.Where(m => m.Vou_No.ToUpper().Contains(searchValue) || m.Vou_dt.ToUpper().Contains(searchValue)
                    || m.cc_vou_amt_bs.ToUpper().Contains(searchValue) || m.cc_vou_amt_sP.ToUpper().Contains(searchValue) || m.amt_type.ToUpper().Contains(searchValue)
                    || m.curr_logo.ToUpper().Contains(searchValue) || m.conv_rate.ToUpper().Contains(searchValue) || m.nurr.ToUpper().Contains(searchValue));
                }
                var data = ItemListData.ToList();
                cc_analysis_model.hdnCSVPrint = null;
                DataTable dt1 = new DataTable();
                dt1 = ToCostCenterAnalysisDetailExl(data);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("CostCenterAnalysis", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToCostCenterAnalysisDetailExl(List<cc_TransactionDetail> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr. No.", typeof(int));
            dataTable.Columns.Add("Voucher Number", typeof(string));
            dataTable.Columns.Add("Voucher Date", typeof(string));
            dataTable.Columns.Add("Amount (In Base)", typeof(decimal));
            dataTable.Columns.Add("Amount (In Specific)", typeof(decimal));
            dataTable.Columns.Add("Amount Type", typeof(string));
            dataTable.Columns.Add("Currency", typeof(string));
            dataTable.Columns.Add("Conversion Rate", typeof(decimal));
            dataTable.Columns.Add("Narration", typeof(string));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr. No."] = item.SrNo;
                rows["Voucher Number"] = item.Vou_No;
                rows["Voucher Date"] = item.Vou_dt;
                rows["Amount (In Base)"] = item.cc_vou_amt_bs;
                rows["Amount (In Specific)"] = item.cc_vou_amt_sP;
                rows["Amount Type"] = item.amt_type;
                rows["Currency"] = item.curr_logo;
                rows["Conversion Rate"] = item.conv_rate;
                rows["Narration"] = item.nurr;
                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        private DataTable ToDataTable(string Details)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("Vou_No", typeof(string));
                dtItem.Columns.Add("Vou_Dt", typeof(string));
                dtItem.Columns.Add("cc_vou_amt_bs", typeof(string));
                dtItem.Columns.Add("cc_vou_amt_sp", typeof(string));
                dtItem.Columns.Add("amt_type", typeof(string));
                dtItem.Columns.Add("curr_logo", typeof(string));
                dtItem.Columns.Add("conv_rate", typeof(string));
                dtItem.Columns.Add("narr", typeof(string));

                if (Details != null)
                {
                    JArray jObject = JArray.Parse(Details);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["Vou_No"] = jObject[i]["vou_no"].ToString();
                        dtrowLines["Vou_Dt"] = jObject[i]["vou_dt"].ToString();
                        dtrowLines["cc_vou_amt_bs"] = jObject[i]["cc_vou_amt_bs"].ToString();
                        dtrowLines["cc_vou_amt_sp"] = jObject[i]["cc_vou_amt_sp"].ToString();
                        dtrowLines["amt_type"] = jObject[i]["amt_type"].ToString();
                        dtrowLines["curr_logo"] = jObject[i]["curr_logo"].ToString();
                        dtrowLines["conv_rate"] = jObject[i]["conv_rate"].ToString();
                        dtrowLines["narr"] = jObject[i]["narr"].ToString();

                        dtItem.Rows.Add(dtrowLines);
                    }
                }
                DtblItemTaxDetail = dtItem;
                return DtblItemTaxDetail;
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

















