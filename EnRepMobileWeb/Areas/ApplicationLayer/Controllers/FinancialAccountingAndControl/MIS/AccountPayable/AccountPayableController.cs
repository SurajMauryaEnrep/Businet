using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.AccountPayable;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.AccountPayable;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;
using iTextSharp.tool.xml;
using System.Configuration;
using EnRepMobileWeb.Areas.Common.Controllers.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.AccountPayable
{
    public class AccountPayableController : Controller
    {
        string CompID, language = String.Empty;
        string Comp_ID, Br_ID, Language, title, UserID = String.Empty;
        string DocumentMenuId = "105104135120";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        AccountPayable_ISERVICE _AccountPayable_ISERVICE;
        public AccountPayableController(Common_IServices _Common_IServices, AccountPayable_ISERVICE _AccountPayable_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._AccountPayable_ISERVICE = _AccountPayable_ISERVICE;
        }
        // GET: ApplicationLayer/AccountPayable
        public ActionResult AccountPayable()
        {
            try
            {
                AccountPayableModel _AccPayModel = new AccountPayableModel();
                ViewBag.MenuPageName = getDocumentName();
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
                    ViewBag.vb_br_id = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                _AccPayModel.categoryLists = suppCategoryList();
                _AccPayModel.portFolioLists = suppPortFolioLists();
                _AccPayModel.Title = title;

                DataSet ds = _AccountPayable_ISERVICE.GetUserRangeDetail(Comp_ID, UserID);
                DataTable br_list = new DataTable();
                br_list = _Common_IServices.Cmn_GetBrList(Comp_ID, UserID);
                ViewBag.br_list = br_list;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    _AccPayModel.Range1 = ds.Tables[0].Rows[0]["range_1"].ToString();
                    _AccPayModel.Range2 = ds.Tables[0].Rows[0]["range_2"].ToString();
                    _AccPayModel.Range3 = ds.Tables[0].Rows[0]["range_3"].ToString();
                    _AccPayModel.Range4 = ds.Tables[0].Rows[0]["range_4"].ToString();
                    _AccPayModel.Range5 = ds.Tables[0].Rows[0]["range_5"].ToString();
                }
                if (Session["APAging_basis"] != null)
                {
                    string basis = Session["APAging_basis"].ToString();
                    _AccPayModel.Basis = basis;

                    _AccPayModel.AccountPayableList = GetAccountReceivable_Detail("0", "0", "0", basis, DateTime.Now.ToString("yyyy-MM-dd"),"A","S", Br_ID);
                }
                else if (Session["APPayableType"] != null)
                {
                    string PayableType = Session["APPayableType"].ToString();
                    _AccPayModel.PayableType = PayableType;
                    _AccPayModel.Basis = "S";
                    _AccPayModel.AccountPayableList = GetAccountReceivable_Detail("0", "0", "0", "S", DateTime.Now.ToString("yyyy-MM-dd"), PayableType,"S", Br_ID);
                }
                else
                {
                    _AccPayModel.AccountPayableList = GetAccountReceivable_Detail("0", "0", "0", "S", DateTime.Now.ToString("yyyy-MM-dd"), "A","S", Br_ID);
                    _AccPayModel.PayableType = "A";
                    _AccPayModel.Basis = "S";
                    _AccPayModel.PayablePdf = "A";
                }
                _AccPayModel.To_dt = DateTime.Now.ToString("yyyy-MM-dd");
                Session["APAging"] = "0";
                Session["APAging_basis"] = null;
                Session["APPayableType"] = null;
                ViewBag.MenuPageName = getDocumentName();
                ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/
                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/AccountPayable/AccountPayable.cshtml", _AccPayModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult ShowReportFromDashBoard(string ToDt)
        {
            AccountPayableModel _AccPayModel = new AccountPayableModel();
            ViewBag.MenuPageName = getDocumentName();
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
                ViewBag.vb_br_id = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            _AccPayModel.categoryLists = suppCategoryList();
            _AccPayModel.portFolioLists = suppPortFolioLists();
            _AccPayModel.Title = title;

            DataSet ds = _AccountPayable_ISERVICE.GetUserRangeDetail(Comp_ID, UserID);
            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(Comp_ID, UserID);
            ViewBag.br_list = br_list;

            if (ds.Tables[0].Rows.Count > 0)
            {
                _AccPayModel.Range1 = ds.Tables[0].Rows[0]["range_1"].ToString();
                _AccPayModel.Range2 = ds.Tables[0].Rows[0]["range_2"].ToString();
                _AccPayModel.Range3 = ds.Tables[0].Rows[0]["range_3"].ToString();
                _AccPayModel.Range4 = ds.Tables[0].Rows[0]["range_4"].ToString();
                _AccPayModel.Range5 = ds.Tables[0].Rows[0]["range_5"].ToString();
            }

            _AccPayModel.AccountPayableList = GetAccountReceivable_Detail("0", "0", "0", "S", ToDt, "A", "S", Br_ID);
            _AccPayModel.PayableType = "A";
            _AccPayModel.Basis = "S";
            _AccPayModel.PayablePdf = "A";
            _AccPayModel.To_dt = ToDt;

            Session["APAging"] = "0";
            Session["APAging_basis"] = null;
            Session["APPayableType"] = null;
            ViewBag.MenuPageName = getDocumentName();
            ViewBag.DocumentMenuId = DocumentMenuId;
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/AccountPayable/AccountPayable.cshtml", _AccPayModel);
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
        public ActionResult GetAutoCompleteSearchSuppList(AccountPayableModel _AccPayModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string SuppType = string.Empty;
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
                if (string.IsNullOrEmpty(_AccPayModel.supp_id))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _AccPayModel.supp_id;
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105101130")
                //    {
                //        SuppType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105101140101")
                //    {
                //        SuppType = "I";
                //    }
                //}
                CustList = _AccountPayable_ISERVICE.GetSupplierList(Comp_ID, SupplierName, Br_ID , "105104135120");

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _AccPayModel.SupplierNameList = _SuppList;
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
        private List<SuppCategoryList> suppCategoryList()
        {
            List<SuppCategoryList> lists = new List<SuppCategoryList>();
            DataTable dt = GetSuppCategory();
            foreach (DataRow dr in dt.Rows)
            {
                SuppCategoryList list = new SuppCategoryList();
                list.Cat_id = dr["setup_id"].ToString();
                list.Cat_val = dr["setup_val"].ToString();
                lists.Add(list);
            }
           // lists.Insert(0, new SuppCategoryList() { Cat_id = "0", Cat_val = "---All---" });
            return lists;
        }
        private List<SuppPortFolioList> suppPortFolioLists()
        {
            List<SuppPortFolioList> portFolioLists = new List<SuppPortFolioList>();
            DataTable dt1 = GetSuppPortfolio();
            foreach (DataRow dr in dt1.Rows)
            {
                SuppPortFolioList SuppPortFolio = new SuppPortFolioList();
                SuppPortFolio.Portfolio_id = dr["setup_id"].ToString();
                SuppPortFolio.Portfolio_val = dr["setup_val"].ToString();
                portFolioLists.Add(SuppPortFolio);
            }
            //portFolioLists.Insert(0, new SuppPortFolioList() { Portfolio_id = "0", Portfolio_val = "---All---" });
            return portFolioLists;
        }
        public DataTable GetSuppCategory()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _AccountPayable_ISERVICE.GetcategoryDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable GetSuppPortfolio()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _AccountPayable_ISERVICE.GetsuppportDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UserRangeSave(AccountPayableModel _AccRecModel, string command)
        {
            try
            {
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (_AccRecModel.CmdPDFPrint == "Print")
                {
                    command = "Print";
                    // DeletePQDetails(_Model);
                }
                if (_AccRecModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                } 
                if (_AccRecModel.hdnInsightCSVPrint == "InsightCsvPrint")
                {
                    command = "InsightCsvPrint";
                }
                if (_AccRecModel.hdnPaidAmtInsightCSVPrint == "PaidAmtInsightCsvPrint")
                {
                    command = "PaidAmtInsightCsvPrint";
                }
                if (_AccRecModel.hdnAdvAmtInsightCSVPrint == "AdvAmtInsightCsvPrint")
                {
                    command = "AdvAmtInsightCsvPrint";
                }
                switch (command)
                {

                    case "Save":
                        Session["Command"] = command;
                        SaveUserRange(_AccRecModel);
                        Session["UserID"] = UserID;
                        return RedirectToAction("AccountPayable");
                    case "Print":
                        return GenratePdfFile(_AccRecModel);
                    case "CsvPrint":
                        return ExportAccountPayableData(_AccRecModel, command);
                    case "InsightCsvPrint":
                        return ExportAccountPayableData(_AccRecModel, command);
                    case "PaidAmtInsightCsvPrint":
                        return ExportAccountPayableData(_AccRecModel, command);
                    case "AdvAmtInsightCsvPrint":
                        return ExportAccountPayableData(_AccRecModel, command);
                    default:
                        return new EmptyResult();

                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult SaveUserRange(AccountPayableModel _AccRecModel)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {

                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }

                string user_id = Session["UserId"].ToString();
                string range1 = _AccRecModel.Range1;
                string range2 = _AccRecModel.Range2;
                string range3 = _AccRecModel.Range3;
                string range4 = _AccRecModel.Range4;
                string range5 = _AccRecModel.Range5;


                SaveMessage = _AccountPayable_ISERVICE.InsertUserRangeDetail(CompID, user_id, range1, range2, range3, range4, range5);


                Session["Message"] = "Save";
                Session["Command"] = "Update";
                Session["UserID"] = UserID;
                Session["TransType"] = "Update";
                Session["AppStatus"] = 'D';
                Session["BtnName"] = "BtnToDetailPage";
                return RedirectToAction("AccountPayable");



            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        [HttpPost]
        public ActionResult SearchAccountPayableDetail(string Supp_id, string Cat_id, string Prf_id, string Basis, string AsDate,string PayableType,string ReportType,string brlist)
        {
            try
            {
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                List<APList> _APListDetail = new List<APList>();
                AccountPayableModel _AccountPayModel = new AccountPayableModel();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
                //DataTable dt = new DataTable();/*Coomented and modify by Hina sharma on 25-12-2024 for dataset instead of datatable*/
                DataSet ds = new DataSet();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                //Session["APAging"] = "Search";
                _AccountPayModel.APAging = "Search";
                ds = _AccountPayable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, Supp_id, Cat_id, Prf_id, Basis, AsDate,0,"List",0, PayableType, ReportType, brlist);
                ViewBag.flag = ReportType;
                //if (dt.Rows.Count > 0)/*Coomented and modify by Hina sharma on 25-12-2024 for dataset instead of datatable*/
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //foreach (DataRow dr in dt.Rows)/*Coomented and modify by Hina sharma on 25-12-2024 for dataset instead of datatable*/
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        APList _APList = new APList();
                        _APList.SuppName = dr["suppname"].ToString();
                        _APList.SuppId = dr["suppid"].ToString();
                        _APList.Curr = dr["curr"].ToString();
                        _APList.CurrId = dr["curr_id"].ToString();/*add by Hina sharma on 18-12-2024 for filter*/
                        //_APList.AmtRange1 = Convert.ToDecimal(dr["r1"]).ToString(ValDigit);
                        //_APList.AmtRange2 = Convert.ToDecimal(dr["r2"]).ToString(ValDigit);
                        //_APList.AmtRange3 = Convert.ToDecimal(dr["r3"]).ToString(ValDigit);
                        //_APList.AmtRange4 = Convert.ToDecimal(dr["r4"]).ToString(ValDigit);
                        //_APList.AmtRange5 = Convert.ToDecimal(dr["r5"]).ToString(ValDigit);
                        //_APList.AmtRange6 = Convert.ToDecimal(dr["gtr5"]).ToString(ValDigit);
                        //_APList.TotalAmt = Convert.ToDecimal(dr["tamt"]).ToString(ValDigit);
                        if (ReportType == "D")
                        {
                            _APList.inv_no = dr["inv_no"].ToString();
                            _APList.inv_dt = dr["inv_dt"].ToString();
                            _APList.inv_amt = dr["inv_amt"].ToString();
                            _APList.bill_no = dr["bill_no"].ToString();
                            _APList.bill_dt = dr["bill_dt"].ToString();
                        }
                        if (ReportType == "S")
                        {
                            _APList.totamt_sp = dr["tot_amt"].ToString();
                            _APList.totamt_bs = dr["tot_amt_bs"].ToString();
                            _APList.advamt_bs = dr["Adv_Amt_bs"].ToString();
                            _APList.totnetamt_bs = dr["tamt_bs"].ToString();
                        }
                        _APList.AmtRange1 = dr["r1"].ToString();
                        _APList.AmtRange2 = dr["r2"].ToString();
                        _APList.AmtRange3 = dr["r3"].ToString();
                        _APList.AmtRange4 = dr["r4"].ToString();
                        _APList.AmtRange5 = dr["r5"].ToString();
                        _APList.AmtRange6 = dr["gtr5"].ToString();
                        _APList.AdvanceAmount = dr["AdvanceAmount"].ToString();
                        _APList.AccId = dr["acc_id"].ToString();
                        _APList.TotalAmt = dr["tamt"].ToString();
                        _AccountPayModel.Range1 = dr["range1"].ToString();
                        _AccountPayModel.Range2 = dr["range2"].ToString();
                        _AccountPayModel.Range3 = dr["range3"].ToString();
                        _AccountPayModel.Range4 = dr["range4"].ToString();
                        _AccountPayModel.Range5 = dr["range5"].ToString();
                        _APListDetail.Add(_APList);
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)/*Add by Hina Sharma on 19-07-2025 to show bucket wise total*/
                {
                    ViewBag.BucketWiseTotal = ds.Tables[1];
                }
                _AccountPayModel.AccountPayableList = _APListDetail;
                _AccountPayModel.PayablePdf = PayableType;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAccountPayable.cshtml", _AccountPayModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public ActionResult GetInvoiceListDetail(string Supp_id, string lrange, string urange, string Basis, string AsDate,int CurrId,string PayableType,string ReportType,string inv_no,string inv_dt, string brlist)
        {
            try
            {
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                List<InvoiceList> _InvoiceListDetail = new List<InvoiceList>();
                AccountPayableModel _AccountRecModel = new AccountPayableModel();
                string CompID = string.Empty;
                DataTable dt = new DataTable();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                dt = _AccountPayable_ISERVICE.GetInvoiceDetailList(CompID, Br_ID, Supp_id, lrange, urange, Basis, AsDate, CurrId, PayableType, ReportType, inv_no, inv_dt, brlist);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {   
                        //var inv_no= dr["inv_no"].ToString();
                        //var Invno = inv_no.Split('/');
                        //var INo = Invno[3];
                        //var code = INo.Split('0');
                        //var Doccode = code[0];
                        InvoiceList _InvoiceList = new InvoiceList();
                        _InvoiceList.Bill_No = dr["bill_no"].ToString();
                        _InvoiceList.Bill_Dt = dr["bill_dt"].ToString();
                        
                        _InvoiceList.Invoice_No = dr["inv_no"].ToString();
                        _InvoiceList.Invoice_Dt = dr["inv_dt"].ToString();
                        _InvoiceList.Invoice_Date = dr["inv_date"].ToString();
                        //_InvoiceList.Invoice_Amt = Convert.ToDecimal(dr["inv_amt_sp"]).ToString(ValDigit);
                        //_InvoiceList.Paid_Amt = Convert.ToDecimal(dr["paid_amt"]).ToString(ValDigit);
                        //_InvoiceList.Balance_Amt = Convert.ToDecimal(dr["pend_amt"]).ToString(ValDigit);
                        //_InvoiceList.Total_Invoice_Amt = Convert.ToDecimal(dr["totalInvAmt"]).ToString(ValDigit);
                        //_InvoiceList.Total_Paid_Amt = Convert.ToDecimal(dr["totalPaidAmt"]).ToString(ValDigit);
                        //_InvoiceList.Total_Balance_Amt = Convert.ToDecimal(dr["totalPendAmt"]).ToString(ValDigit);
                        _InvoiceList.Invoice_Amt = dr["inv_amt_sp"].ToString();
                        _InvoiceList.Paid_Amt = dr["paid_amt"].ToString();
                        _InvoiceList.Balance_Amt = dr["pend_amt"].ToString();
                        _InvoiceList.Total_Invoice_Amt = dr["totalInvAmt"].ToString();
                        _InvoiceList.Total_Paid_Amt = dr["totalPaidAmt"].ToString();
                        _InvoiceList.Total_Balance_Amt = dr["totalPendAmt"].ToString();
                        _InvoiceList.due_days = dr["due_days"].ToString();
                        _InvoiceList.due_Date = dr["due_date"].ToString();
                        _InvoiceList.Payment_Terms = dr["paym_term"].ToString();/*Add paym_term by Hina sharma on 25-12-2024*/
                        _InvoiceList.DocCode = dr["src_type"].ToString();/*Add paym_term by Hina sharma on 17-04-2025*/
                        _InvoiceListDetail.Add(_InvoiceList);
                    }
                }

                _AccountRecModel.InvoiceList = _InvoiceListDetail;
                ViewBag.Basis = Basis;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAccountPayableInvoiceDetails.cshtml", _AccountRecModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetOverDuePaymentList(string Basis, string AsDate,string PayableType)
        {
            if (Basis == "S")
            {
                Session["APPayableType"] = PayableType;
            }
            else
            {
                Session["APAging_basis"] = Basis;
            }
            return RedirectToAction("AccountPayable");

        }
        private List<APList> GetAccountReceivable_Detail(string Supp_id, string Cat_id, string Prf_id, string Basis, string AsDate,string PayableType,string ReportType, string brlist)
        {
            try
            {
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                List<APList> _APListDetail = new List<APList>();
                AccountPayableModel _AccountPayModel = new AccountPayableModel();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
                //DataTable dt = new DataTable();/*Coomented and modify by Hina sharma on 25-12-2024 for dataset instead of datatable*/
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                Session["APAging"] = "Search";
                ds = _AccountPayable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, Supp_id, Cat_id, Prf_id, Basis, AsDate,0,"List",0, PayableType, ReportType, brlist);
                ViewBag.flag = ReportType;
                //if (dt.Rows.Count > 0)/*Coomented and modify by Hina sharma on 25-12-2024 for dataset instead of datatable*/
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //foreach (DataRow dr in dt.Rows)/*Coomented and modify by Hina sharma on 25-12-2024 for dataset instead of datatable*/
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        APList _APList = new APList();
                        _APList.SuppName = dr["suppname"].ToString();
                        _APList.SuppId = dr["suppid"].ToString();
                        _APList.Curr = dr["curr"].ToString();
                        _APList.CurrId = dr["curr_id"].ToString();/*add by Hina sharma on 18-12-2024 for filter*/
                        //_APList.AmtRange1 = Convert.ToDecimal(dr["r1"]).ToString(ValDigit);
                        //_APList.AmtRange2 = Convert.ToDecimal(dr["r2"]).ToString(ValDigit);
                        //_APList.AmtRange3 = Convert.ToDecimal(dr["r3"]).ToString(ValDigit);
                        //_APList.AmtRange4 = Convert.ToDecimal(dr["r4"]).ToString(ValDigit);
                        //_APList.AmtRange5 = Convert.ToDecimal(dr["r5"]).ToString(ValDigit);
                        //_APList.AmtRange6 = Convert.ToDecimal(dr["gtr5"]).ToString(ValDigit);
                        //_APList.TotalAmt = Convert.ToDecimal(dr["tamt"]).ToString(ValDigit);
                        _APList.AmtRange1 = dr["r1"].ToString();
                        _APList.AmtRange2 = dr["r2"].ToString();
                        _APList.AmtRange3 = dr["r3"].ToString();
                        _APList.AmtRange4 = dr["r4"].ToString();
                        _APList.AmtRange5 = dr["r5"].ToString();
                        _APList.AmtRange6 = dr["gtr5"].ToString();
                        _APList.totamt_sp = dr["tot_amt"].ToString();
                        _APList.totamt_bs = dr["tot_amt_bs"].ToString();
                        _APList.AdvanceAmount = dr["AdvanceAmount"].ToString();
                        _APList.advamt_bs = dr["Adv_Amt_bs"].ToString();
                        _APList.AccId = dr["acc_id"].ToString();
                        _APList.TotalAmt = dr["tamt"].ToString();
                        _APList.totnetamt_bs = dr["tamt_bs"].ToString();
                        _AccountPayModel.Range1 = dr["range1"].ToString();
                        _AccountPayModel.Range2 = dr["range2"].ToString();
                        _AccountPayModel.Range3 = dr["range3"].ToString();
                        _AccountPayModel.Range4 = dr["range4"].ToString();
                        _AccountPayModel.Range5 = dr["range5"].ToString();
                        _APListDetail.Add(_APList);
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)/*Add by Hina Sharma on 21-07-2025 to show bucket wise total*/
                {
                    ViewBag.BucketWiseTotal = ds.Tables[1];
                }
                return _APListDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public FileResult GenratePdfFile(AccountPayableModel _AccRecModel)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                //Added by Nidhi on 06-01-2026
                if(_AccRecModel.CmdPDFPrint == "Print")
                {
                    _AccRecModel.supp_id = _AccRecModel.Hdnsupp_id;
                    _AccRecModel.category = _AccRecModel.Hdncatg_id;
                    _AccRecModel.portFolio = _AccRecModel.Hdnport_id;
                }
                _AccRecModel.CmdPDFPrint = null;
                DataTable Details = new DataTable();
                DataSet ds = new DataSet();
                //_AccRecModel.AccountPayableList = GetAccountReceivable_Detail(_AccRecModel.supp_id, _AccRecModel.category, _AccRecModel.portFolio, _AccRecModel.Basis, _AccRecModel.To_dt, _AccRecModel.PayableType);
                ds = _AccountPayable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, _AccRecModel.supp_id, _AccRecModel.category, _AccRecModel.portFolio, _AccRecModel.Basis, _AccRecModel.To_dt, 0, "List", 0, _AccRecModel.PayableType,_AccRecModel.ReportType,_AccRecModel.hdnbr_ids);
                DataTable dtItem = new DataTable();
                if (_AccRecModel.PayableType == "A")
                {
                    ViewBag.PaybleType = "All";
                }
                else if (_AccRecModel.PayableType == "O")
                {
                    ViewBag.PaybleType = " Overdue";
                }
                else
                {
                    ViewBag.PaybleType = " Upcoming Due ";
                }
                ViewBag.ReportType = _AccRecModel.ReportType;
                //dtItem.Columns.Add("supp_name", typeof(string));
                //dtItem.Columns.Add("supp_id", typeof(string));
                //dtItem.Columns.Add("curr", typeof(string));
                //dtItem.Columns.Add("Range1", typeof(string));
                //dtItem.Columns.Add("Range2", typeof(string));
                //dtItem.Columns.Add("Range3", typeof(string));
                //dtItem.Columns.Add("Range4", typeof(string));
                //dtItem.Columns.Add("Range5", typeof(string));
                //dtItem.Columns.Add("Range6", typeof(string));
                //dtItem.Columns.Add("TotalAmt", typeof(string));
                //dtItem.Columns.Add("AdvanceAmount", typeof(string));

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        DataRow dtrowLines = dtItem.NewRow();
                //        dtrowLines["supp_name"] = dr["suppname"].ToString();
                //        dtrowLines["supp_id"] = dr["suppid"].ToString();
                //        dtrowLines["curr"] = dr["curr"].ToString();
                //        dtrowLines["Range1"] = dr["r1"].ToString();
                //        dtrowLines["Range2"] = dr["r2"].ToString();
                //        dtrowLines["Range3"] = dr["r3"].ToString();
                //        dtrowLines["Range4"] = dr["r4"].ToString();
                //        dtrowLines["Range5"] = dr["r5"].ToString();
                //        dtrowLines["Range6"] = dr["gtr5"].ToString();
                //        dtrowLines["TotalAmt"] = dr["tamt"].ToString();
                //        dtrowLines["AdvanceAmount"] = dr["AdvanceAmount"].ToString();
                //        dtItem.Rows.Add(dtrowLines);
                //    }
                //}
                //Details = ds.Tables[0];
                Details = ds.Tables[0];
                //Details = ToDataTable(_AccRecModel.PrintPDFData,"PDF");
                ViewBag.RangeList = _AccRecModel.RangeList;
                DataView data = new DataView();
                data = Details.DefaultView;
                data.Sort = "suppname";
                //data.Sort = "supp_name";
                Details = data.ToTable();
                DataTable dt = new DataTable();
                //dt = data.ToTable(true, "supp_name");
                dt = data.ToTable(true, "suppname");

                //DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, Br_ID);
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, Br_ID);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                //ViewBag.CompLogoDtl = dtlogo;
                ViewBag.Details = Details;
                //ViewBag.TotalDetails = ds.Tables[1];
                ViewBag.DocName = "Account Payable";
                ViewBag.basis = _AccRecModel.hdnBasis;
                ViewBag.AsonDate = _AccRecModel.hdnAsonDate;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TotalDetails = ds.Tables[1];/*Add by Hina Sharma on 21-07-2025 for show bucket wise total*/
                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 18f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    data = Details.DefaultView;
                    DataTable PrintGL = new DataTable();
                    pdfDoc.Open();
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/AccountPayable/AccountPayablePrint.cshtml"));
                    reader = new StringReader(htmlcontent);
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font font = new Font(bf, 9, Font.BOLDITALIC, BaseColor.BLACK);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return File(bytes.ToArray(), "application/pdf", "AccountPayable.pdf");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
            finally
            {

            }
        }
        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }
        private DataTable ToDataTable(string Details,string command)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("supp_name", typeof(string));
                dtItem.Columns.Add("supp_id", typeof(string));
                dtItem.Columns.Add("curr", typeof(string));
                dtItem.Columns.Add("Range1", typeof(string));
                dtItem.Columns.Add("Range2", typeof(string));
                dtItem.Columns.Add("Range3", typeof(string));
                dtItem.Columns.Add("Range4", typeof(string));
                dtItem.Columns.Add("Range5", typeof(string));
                dtItem.Columns.Add("Range6", typeof(string));
                dtItem.Columns.Add("TotalAmt", typeof(string));
                dtItem.Columns.Add("AdvanceAmount", typeof(string));
                if (Details != null)
                {
                    JArray jObject = JArray.Parse(Details);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["supp_name"] = jObject[i]["Supp_name"].ToString();
                        dtrowLines["supp_id"] = jObject[i]["Supp_id"].ToString();
                        dtrowLines["curr"] = jObject[i]["Curr"].ToString();
                        dtrowLines["Range1"] = jObject[i]["multiWrapperRange1"].ToString();
                        dtrowLines["Range2"] = jObject[i]["multiWrapperRange2"].ToString();
                        dtrowLines["Range3"] = jObject[i]["multiWrapperRange3"].ToString();
                        dtrowLines["Range4"] = jObject[i]["multiWrapperRange4"].ToString();
                        dtrowLines["Range5"] = jObject[i]["multiWrapperRange5"].ToString();
                        dtrowLines["Range6"] = jObject[i]["multiWrapperRange6"].ToString();
                        dtrowLines["TotalAmt"] = jObject[i]["TotalAmt"].ToString();
                        dtrowLines["AdvanceAmount"] = jObject[i]["AdvanceAmount"].ToString();
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
        public FileResult ExportAccountPayableData(AccountPayableModel _AccRecModel, string command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                _AccRecModel.CmdPDFPrint = null;
                DataTable Details = new DataTable();
                DataTable dt = new DataTable();
                 DataTable dt1 = new DataTable();
                DataSet ds = new DataSet();

                if (command == "InsightCsvPrint")
                {
                    //Details = ToDataTable(_AccRecModel.PrintPDFData, command);
                    dt1.Columns.Add("Sr.No", typeof(string));
                    dt1.Columns.Add("Bill Number", typeof(string));
                    dt1.Columns.Add("Bill date", typeof(string));
                    dt1.Columns.Add("Invoice No.", typeof(string));
                    dt1.Columns.Add("Invoice date", typeof(string));
                    dt1.Columns.Add("Invoice Amount", typeof(decimal));
                    dt1.Columns.Add("Paid Amount", typeof(decimal));
                    dt1.Columns.Add("Balance Amount", typeof(decimal));
                    dt1.Columns.Add("Due Date", typeof(string));
                    dt1.Columns.Add("Payment Terms (in Days)", typeof(string));
                    dt1.Columns.Add("Overdue Days", typeof(string));

                    if (_AccRecModel.PrintPDFData != null)
                    {
                        int rowno = 0;
                        JArray jObject = JArray.Parse(_AccRecModel.PrintPDFData);
                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dt1.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Bill Number"] = jObject[i]["Ap_Bill_No"].ToString();
                            dtrowLines["Bill date"] = jObject[i]["Ap_Bill_Dt"].ToString();
                            dtrowLines["Invoice No."] = jObject[i]["Hdn_InvNo"].ToString();
                            dtrowLines["Invoice date"] = jObject[i]["Inv_Dt"].ToString();
                            dtrowLines["Invoice Amount"] = jObject[i]["AP_Invoice_Amt"].ToString();
                            dtrowLines["Paid Amount"] = jObject[i]["paid_amt"].ToString();
                            dtrowLines["Balance Amount"] = jObject[i]["AP_Balance_Amt"].ToString();
                            dtrowLines["Due Date"] = jObject[i]["AP_due_Date"].ToString();
                            dtrowLines["Payment Terms (in Days)"] = jObject[i]["AP_Payment_Terms"].ToString();
                            dtrowLines["Overdue Days"] = jObject[i]["AP_due_days"].ToString();
                            dt1.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (command == "PaidAmtInsightCsvPrint")
                {
                    dt1.Columns.Add("Sr.No", typeof(string));
                    dt1.Columns.Add("Voucher Number", typeof(string));
                    dt1.Columns.Add("Voucher Date", typeof(string));
                    dt1.Columns.Add("Voucher Type", typeof(string));
                    dt1.Columns.Add("Amount", typeof(decimal));

                    if (_AccRecModel.PrintPDFData != null)
                    {
                        int rowno = 0;
                        JArray jObject = JArray.Parse(_AccRecModel.PrintPDFData);
                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dt1.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Voucher Number"] = jObject[i]["AR_VouNo"].ToString();
                            dtrowLines["Voucher Date"] = jObject[i]["AR_VouDate"].ToString();
                            dtrowLines["Voucher Type"] = jObject[i]["AR_VouType"].ToString();
                            dtrowLines["Amount"] = jObject[i]["AR_paid_amt"].ToString();
                            dt1.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (command == "AdvAmtInsightCsvPrint")
                {
                    dt1.Columns.Add("Sr.No", typeof(string));
                    dt1.Columns.Add("Voucher Number", typeof(string));
                    dt1.Columns.Add("Voucher Date", typeof(string));
                    dt1.Columns.Add("Voucher Type", typeof(string));
                    dt1.Columns.Add("Amount", typeof(decimal));
                    dt1.Columns.Add("Adjusted Amount", typeof(decimal));
                    dt1.Columns.Add("Pending Amount", typeof(decimal));

                    if (_AccRecModel.PrintPDFData != null)
                    {
                        int rowno = 0;
                        JArray jObject = JArray.Parse(_AccRecModel.PrintPDFData);
                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dt1.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Voucher Number"] = jObject[i]["AR_VouNo"].ToString();
                            dtrowLines["Voucher Date"] = jObject[i]["AR_VouDate"].ToString();
                            dtrowLines["Voucher Type"] = jObject[i]["AR_VouType"].ToString();
                            dtrowLines["Amount"] = jObject[i]["AR_paid_amt"].ToString();
                            dtrowLines["Adjusted Amount"] = jObject[i]["Adj_amt"].ToString();
                            dtrowLines["Pending Amount"] = jObject[i]["Pend_amt"].ToString();
                            dt1.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else
                {
                    ds = _AccountPayable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, _AccRecModel.supp_id, _AccRecModel.category, _AccRecModel.portFolio, _AccRecModel.Basis, _AccRecModel.To_dt, 0, "List", 0, _AccRecModel.PayableType,_AccRecModel.ReportType,_AccRecModel.hdnbr_ids);

                    //Details = ToDataTable(_AccRecModel.PrintPDFData, command);
                    //List<APList> _APListDetail = new List<APList>();
                    //dt = Details;
                    //if (dt.Rows.Count > 0)
                    //{
                    //    int rowno = 0;
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        APList _ARList = new APList();
                    //        _ARList.SrNo = rowno + 1;
                    //        _ARList.SuppName = dr["supp_name"].ToString();
                    //        _ARList.SuppId = dr["supp_id"].ToString();
                    //        _ARList.Curr = dr["curr"].ToString();
                    //        _ARList.AmtRange1 = dr["Range1"].ToString();
                    //        _ARList.AmtRange2 = dr["Range2"].ToString();
                    //        _ARList.AmtRange3 = dr["Range3"].ToString();
                    //        _ARList.AmtRange4 = dr["Range4"].ToString();
                    //        _ARList.AmtRange5 = dr["Range5"].ToString();
                    //        _ARList.AmtRange6 = dr["Range6"].ToString();
                    //        _ARList.TotalAmt = dr["TotalAmt"].ToString();
                    //        _ARList.AdvanceAmount = dr["AdvanceAmount"].ToString();
                    //        _AccRecModel.Range1 = dr["range1"].ToString();
                    //        _AccRecModel.Range2 = dr["range2"].ToString();
                    //        _AccRecModel.Range3 = dr["range3"].ToString();
                    //        _AccRecModel.Range4 = dr["range4"].ToString();
                    //        _AccRecModel.Range5 = dr["range5"].ToString();
                    //        _APListDetail.Add(_ARList);
                    //        rowno = rowno + 1;
                    //    }
                    //}
                    //var ItemListData = (from tempitem in _APListDetail select tempitem);
                    //string searchValue = _AccRecModel.searchValue;
                    //if (!string.IsNullOrEmpty(searchValue))
                    //{
                    //    searchValue = searchValue.ToUpper();
                    //    ItemListData = ItemListData.Where(m => m.SuppName.ToUpper().Contains(searchValue) || m.SuppId.ToUpper().Contains(searchValue)
                    //    || m.Curr.ToUpper().Contains(searchValue) || m.AmtRange1.ToUpper().Contains(searchValue) || m.AmtRange2.ToUpper().Contains(searchValue)
                    //    || m.AmtRange3.ToUpper().Contains(searchValue) || m.AmtRange4.ToUpper().Contains(searchValue) || m.AmtRange5.ToUpper().Contains(searchValue)
                    //    || m.AmtRange6.ToUpper().Contains(searchValue) || m.TotalAmt.ToUpper().Contains(searchValue) || m.AdvanceAmount.ToUpper().Contains(searchValue));
                    //}
                    //var data = ItemListData.ToList();
                    _AccRecModel.hdnCSVPrint = null;

                    //dt1 = ToAccountPayableDetailExl(data, _AccRecModel);

                    string Range1="";
                    string Range2="";
                    string Range3="";
                    string Range4="";
                    string Range5="";
                    string Range6 = "";
                    JArray jObject = JArray.Parse(_AccRecModel.RangeList);
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                        Range1 = jObject[0]["Range1"].ToString();
                        Range2 = jObject[0]["Range2"].ToString();
                        Range3 = jObject[0]["Range3"].ToString();
                        Range4 = jObject[0]["Range4"].ToString();
                        Range5 = jObject[0]["Range5"].ToString();
                        Range6 = jObject[0]["Range6"].ToString();
                    //}
                        

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Sr.No.", typeof(int));
                    dataTable.Columns.Add("Supplier Name", typeof(string));
                    if (_AccRecModel.ReportType == "D")
                    {
                        dataTable.Columns.Add("Invoice No.", typeof(string));
                        dataTable.Columns.Add("Invoice Date", typeof(string));
                        dataTable.Columns.Add("Bill Number", typeof(string));
                        dataTable.Columns.Add("Bill Date", typeof(string));
                    }
                    dataTable.Columns.Add("Currency", typeof(string));
                    if (_AccRecModel.ReportType == "D")
                    {
                        dataTable.Columns.Add("Invoice Amount", typeof(decimal));
                    }
                    dataTable.Columns.Add(Range1, typeof(decimal));
                    dataTable.Columns.Add(Range2, typeof(decimal));
                    dataTable.Columns.Add(Range3, typeof(decimal));
                    dataTable.Columns.Add(Range4, typeof(decimal));
                    dataTable.Columns.Add(Range5, typeof(decimal));
                    dataTable.Columns.Add(Range6, typeof(decimal));
                    dataTable.Columns.Add("Advance Payment", typeof(decimal));
                    dataTable.Columns.Add("Total", typeof(decimal));

                    //foreach (var item in _ItemListModel)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var SrNo = 1;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow rows = dataTable.NewRow();
                            rows["Sr.No."] = SrNo;
                            rows["Supplier Name"] = dr["suppname"].ToString();
                            if (_AccRecModel.ReportType == "D")
                            {
                                rows["Invoice No."] = dr["inv_no"].ToString();
                                rows["Invoice Date"] = dr["inv_dt"].ToString();
                                rows["Bill Number"] = dr["bill_no"].ToString();
                                rows["Bill Date"] = dr["bill_dt"].ToString();
                            }
                            rows["Currency"] = dr["curr"].ToString();
                            if (_AccRecModel.ReportType == "D")
                            {
                                rows["Invoice Amount"] = dr["inv_amt"].ToString();
                            }
                            rows[Range1] = dr["r1"].ToString();
                            rows[Range2] = dr["r2"].ToString();
                            rows[Range3] = dr["r3"].ToString();
                            rows[Range4] = dr["r4"].ToString();
                            rows[Range5] = dr["r5"].ToString();
                            rows[Range6] = dr["gtr5"].ToString();
                            if (_AccRecModel.ReportType != "D")
                            {
                                rows["Advance Payment"] = dr["AdvanceAmount"].ToString();
                            }      
                            rows["Total"] = dr["tamt"].ToString();
                            dataTable.Rows.Add(rows);
                            SrNo = SrNo + 1;
                        }
                        dt1 = dataTable;
                    }
                }
                

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Account Payable", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToAccountPayableDetailExl(List<APList> _ItemListModel, AccountPayableModel _AccRecModel)
        {

            JArray jObject = JArray.Parse(_AccRecModel.RangeList);
            string Range1 = jObject[0]["Range1"].ToString();
            string Range2 = jObject[0]["Range2"].ToString();
            string Range3 = jObject[0]["Range3"].ToString();
            string Range4 = jObject[0]["Range4"].ToString();
            string Range5 = jObject[0]["Range5"].ToString();
            string Range6 = jObject[0]["Range6"].ToString();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr.No.", typeof(int));
            dataTable.Columns.Add("Supplier Name", typeof(string));
            dataTable.Columns.Add("Currency", typeof(string));
            dataTable.Columns.Add(Range1, typeof(decimal));
            dataTable.Columns.Add(Range2, typeof(decimal));
            dataTable.Columns.Add(Range3, typeof(decimal));
            dataTable.Columns.Add(Range4, typeof(decimal));
            dataTable.Columns.Add(Range5, typeof(decimal));
            dataTable.Columns.Add(Range6, typeof(decimal));
            dataTable.Columns.Add("Advance Payment", typeof(decimal));
            dataTable.Columns.Add("Total", typeof(decimal));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr.No."] = item.SrNo;
                rows["Supplier Name"] = item.SuppName;
                rows["Currency"] = item.Curr;
                rows[Range1] = item.AmtRange1;
                rows[Range2] = item.AmtRange2;
                rows[Range3] = item.AmtRange3;
                rows[Range4] = item.AmtRange4;
                rows[Range5] = item.AmtRange5;
                rows[Range6] = item.AmtRange6;
                rows["Advance Payment"] = item.AdvanceAmount;
                rows["Total"] = item.TotalAmt;
                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        public ActionResult SearchAdvanceAmountDetail(string accId,int CurrId,string AsDate,string Basis, string PayableType, string brlist)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                //if (Session["userid"] != null)
                //    UserID = Session["userid"].ToString();
                DataSet ds = _AccountPayable_ISERVICE.SearchAdvanceAmountDetail(CompID, Br_ID, accId, CurrId, AsDate, Basis, PayableType, brlist);
                ViewBag.AdvanceAmountDetail = ds.Tables[0];
                ViewBag.AdvanceAmountDetailTotal = ds.Tables[1];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAdvancePaymentDetails.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult SearchPaidAmountDetail(string InVNo, string InvDate,string asondate,string supp_id)/*Add by Hina sharma on 11-12-2024*/
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                //if (Session["userid"] != null)
                //    UserID = Session["userid"].ToString();
                //DataTable dt = _AccountReceivable_ISERVICE.SearchAdvanceAmountDetail(CompID, Br_ID, accId);
                DataSet dt = _AccountPayable_ISERVICE.SearchPaidAmountDetail(CompID, Br_ID, InVNo, InvDate, asondate, supp_id);
                ViewBag.PaidAmountDetail = dt.Tables[0];
                ViewBag.PaidAmountDetailTotal = dt.Tables[1];
                //ViewBag.AdvanceAmountDetail = dt;
                return PartialView("~/Areas/Common/Views/Cmn_PartialPaidAmountDetails.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        /*-----------For get Invoice detail Popup Pdf by particular Invoice no------  */
        public FileResult GenerateInvoiceDetails(string invNo, string invDate, /*string invType,*/ string dataType)
        {
            if (dataType == "DPI" || dataType == "CIN")
            {
                ViewBag.Title = "Purchase Invoice";
            }
            else if (dataType == "SPI")
            {
                ViewBag.Title = "Service Purchase Invoice";
            }
            else if (dataType == "SJI")
            {
                ViewBag.Title = "Job Invoice";
            }
            else if (dataType == "IPI")
            {
                ViewBag.Title = "Import Invoice";
            }
            else if (dataType == "PDI")
            {
                ViewBag.Title = "Direct Purchase Invoice";
            }
            else if (dataType == "SCI")
            {
                ViewBag.Title = "Direct Sale Invoice";
            }
            else if (dataType == "DSI")
            {
                ViewBag.Title = "Sale Invoice";
            }
            else if (dataType == "SSI")
            {
                ViewBag.Title = "Service Sale Invoice";
            }
            //ViewBag.Title = "EInvoicePreview";
            return File(GetIvDtlPopupPrintData(invNo, invDate, /*invType,*/ dataType), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        }
        public byte[] GetIvDtlPopupPrintData(string invNo, string invDate, string dataType)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet Details = new DataSet();
                if (dataType == "DPI" || dataType == "CIN" || dataType == "SJI" || dataType == "SPI" || dataType == "IPI" || dataType == "PDI"|| dataType == "SCI" || dataType == "DSI" || dataType == "SSI")
                {
                    Details = _AccountPayable_ISERVICE.GetInvoiceDeatilsForPrint(CompID, Br_ID, invNo, invDate, dataType);
                    //ViewBag.PageName = "PI";
                    if (dataType == "DPI" || dataType == "SJI" || dataType == "IPI"|| dataType == "PDI")
                    {
                        string path1 = Server.MapPath("~") + "..\\Attachment\\";
                        string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                        ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                    }
                    else
                    {
                        string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                        string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                        if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                            serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                        else
                            serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                        ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                        ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
                    }
                }
                ViewBag.Details = Details;
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.InvoiceTo = "";

                string htmlcontent = "";
                if (dataType == "DPI" || dataType == "IPI")
                {
                    ViewBag.PageName = "PI";
                    if (dataType == "DPI")
                    {
                        ViewBag.Title = "Purchase Invoice";
                    }
                    else
                    {
                        ViewBag.Title = "Import Invoice";
                    }

                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/PurchaseInvoicePrint.cshtml"));
                }
                else if (dataType == "CIN")
                {
                    ViewBag.PageName = "PI";
                    ViewBag.Title = "Consumble Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/ConsumableInvoice/ConsumbleInvoicePrint.cshtml"));
                }
                else if (dataType == "SJI")
                {
                    ViewBag.PageName = "JI";
                    ViewBag.Title = "Job Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SubContracting/JobInvoice/JobInvoicePrint.cshtml"));
                }
                else if (dataType == "SPI")
                {
                    ViewBag.PageName = "PI";
                    ViewBag.Title = "Service Purchase Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseInvoice/ServicePurchaseInvoicePrint.cshtml"));
                }
                else if (dataType == "PDI")
                {
                    ViewBag.PageName = "PDI";
                    ViewBag.Title = "Direct Purchase Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/DirectPurchaseInvoice/DirectPurchaseInvoicePrint.cshtml"));
                }
                else if (dataType == "SCI")
                {
                    ViewBag.PageName = "SCI";
                    ViewBag.Title = "Direct Sale Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceWithGSTPrint.cshtml"));
                }
                else if (dataType == "DSI")
                {
                    ViewBag.PageName = "DSI";
                    ViewBag.Title = "Sale Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrint.cshtml"));
                }
                else if (dataType == "SSI")
                {
                    ViewBag.PageName = "SSI";
                    ViewBag.Title = "Service Sale Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceWithGSTPrint.cshtml"));
                }

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            finally
            {

            }
        }
        /*-----------------------Print Section Begin--------------------*/
        public FileResult GenratePdfFile1(string accId, string currId, string asOndate, int Curr_Id, int Acc_Id, string Basis,string ReportType, string brlist)
        {
            //string curr, string r1, string r2, string r3, string r4, string r5, string r6,
            //string rv1, string rv2, string rv3, string rv4, string rv5, string rv6, string advPmt, string totalAmt
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataSet dtdata = _AccountPayable_ISERVICE.GetGLAccountPayablePrintData(CompID, Br_ID, accId,currId, asOndate, UserID, brlist);

                ViewBag.RangeValues = dtdata.Tables[1];
                /*commented and Modify by Hina sharma on 25-12-2024 to also show pending invoces and advance payments details*/
                /*Commented and modify by Hina sharma on 25-12-2024 to change as dataset instead of datatable*/
                //DataTable dtAccPbl = _AccountPayable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, accId, "0", "0", "I", asOndate);
                //DataView dv = new DataView(dtAccPbl);
                //dv.RowFilter = "curr='" + currId + "' ";
                //ViewBag.AccPbl = dv.ToTable();
                DataSet dsAllTbl = _AccountPayable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, accId, "0", "0", Basis, asOndate, Curr_Id, "Print", Acc_Id,"A", ReportType, brlist);
                //ViewBag.AccPbl = dsAllTbl.Tables[0];
                DataView dv = new DataView(dsAllTbl.Tables[0]);
                dv.RowFilter = "curr='" + currId + "' ";
                ViewBag.AccPbl = dv.ToTable();
                ViewBag.PendingInvoicesDetail = dsAllTbl.Tables[1];
                ViewBag.AdvancePaymentsDetail = dsAllTbl.Tables[2];
                ViewBag.TotalAdvancePaymentsDetail = dsAllTbl.Tables[3];/*Add by Hina sharma on 06-01-2025*/
                ViewBag.TotalPendingInvoicesDetail = dsAllTbl.Tables[4];/*Add by Hina sharma on 06-01-2025*/

                DataTable Details = new DataTable();
                Details = dtdata.Tables[0]; //ToDataTable(_model.PrintData);
                DataView data = new DataView();
                data = Details.DefaultView;
                DataTable dt = new DataTable();
                dt = data.ToTable(true, "acc_name");

                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, Br_ID);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.Details = Details;
                ViewBag.FromDate = dtdata.Tables[2].Rows[0][0].ToString();
                ViewBag.ToDate = Convert.ToDateTime(asOndate).ToString("dd-MM-yyyy");
                //ViewBag.CompanyName = "Alaska Exports";
                ViewBag.DocName = "Supplier Statement";
                ViewBag.Currency = currId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 30f, 80f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    data = Details.DefaultView;
                    DataTable PrintGL = new DataTable();
                    pdfDoc.Open();
                    string Start = "Y";

                    //data = Details.DefaultView;
                    PrintGL = data.ToTable();
                    ViewBag.PrintGL = PrintGL;
                    //ViewBag.GLAccountName = PrintGL.Rows[0]["op_bal"].ToString();
                    ViewBag.GLAccountName = "";
                    if (PrintGL.Rows.Count > 0)
                        ViewBag.GLAccountName = PrintGL.Rows[0]["br_op_bal_bs"].ToString();
                    //ViewBag.GLAccountName = PrintGL.Rows[PrintGL.Rows.Count-1]["closing_bal"].ToString();

                    ViewBag.GLAccountName = "";
                    if (PrintGL.Rows.Count > 0)
                        ViewBag.GLAccountName = PrintGL.Rows[PrintGL.Rows.Count - 1]["br_acc_bal_bs"].ToString();
                    htmlcontent = ConvertPartialViewToString1(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/AccountPayable/AccountPayablePrintLedger.cshtml"));
                    reader = new StringReader(htmlcontent);
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);

                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font font = new Font(bf, 9, Font.BOLDITALIC, BaseColor.BLACK);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return File(bytes.ToArray(), "application/pdf", "SupplierStatement.pdf");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
            finally
            {

            }
        }
        protected string ConvertPartialViewToString1(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }
        /*-----------------------Print Section END--------------------*/
    }
}





















