using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.BankBook;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BankBook;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.BankBook
{
    public class BankBookController : Controller
    {
        string CompID, BrID, title, language = String.Empty;
        string DocumentMenuId = "105104135140";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        BankBook_ISERVICE BankBook_ISERVICE;
        BankBookModel _BankBookModel;
        public BankBookController(Common_IServices _Common_IServices, BankBook_ISERVICE BankBook_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.BankBook_ISERVICE = BankBook_ISERVICE;
        }
        // GET: ApplicationLayer/BankBook
        public ActionResult BankBook()
        {
            _BankBookModel = new BankBookModel();
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
            }

            List<curr> currList = new List<curr>();
            DataTable dt = Getcurr("A");
            foreach (DataRow dr in dt.Rows)
            {
                curr curr = new curr();
                curr.curr_id = dr["curr_id"].ToString();
                curr.curr_name = dr["curr_name"].ToString();
                currList.Add(curr);
            }
            currList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });

            DataSet dttbl = new DataSet();
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                _BankBookModel.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();
                ViewBag.ToFyMindate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();/*Add by Hina on 13-08-2024 for current date*/

                ViewBag.fylist = dttbl.Tables[1];
            }
            _BankBookModel.currList = currList;
            ViewBag.MenuPageName = getDocumentName();
            _BankBookModel.Title = title;
            ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/BankBook/BankBook.cshtml", _BankBookModel);
        }
        [HttpPost]
        public ActionResult BankBookDetail(BankBookModel _BankBookModel, string command)
        {
            try
            {
                if (_BankBookModel.hdnPDFPrint == "Print")
                {
                    command = "Print";
                }
                if (_BankBookModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "Print":
                        return GenratePdfFile(_BankBookModel);
                    case "CsvPrint":
                        return ExportBankBookData(_BankBookModel);
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
        public FileResult ExportBankBookData(BankBookModel _BankBookModel)
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

                //DataTable Details = new DataTable();//Commented by Suraj Maurya on 20-06-2025
                //Details = ToDataTable(_BankBookModel.PrintData);

                /*Added By Suraj Maurya on 20-06-2025*/
                Search_Parmeters model1 = new Search_Parmeters();
                model1.CompId = CompID;
                model1.BrId = BrID;
                model1.AccID = _BankBookModel.BankAcc_Id;
                model1.CurrId = _BankBookModel.curr;
                model1.From_dt = _BankBookModel.FromDate;
                model1.To_dt = _BankBookModel.ToDate;
                model1.Flag = "CSV";

                var SearchValue = IsNull(_BankBookModel.searchValue, "");
                var Direction = IsNull(_BankBookModel.sortColumnDir, "asc");
                var sortColumnNm = IsNull(_BankBookModel.sortColumn, "SrNo");

                DataSet DSet = BankBook_ISERVICE.GetBankBookDetails(model1,0, 50, SearchValue, sortColumnNm, Direction);
                /*Added By Suraj Maurya on 20-06-2025 End*/
                DataTable dt = new DataTable();
                //List<ARList> _ARListDetail = new List<ARList>();
                List<BankBookList> _BB_Detail_List = new List<BankBookList>();
                //var sortColumn = "itemname";
                //var sortColumnDir = "asc";
                dt = DSet.Tables[1]/*Details*/;/*Modified by Suraj Maurya on 20-06-2025*/
                if (dt.Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        BankBookList bb_list = new BankBookList();
                        //bb_list.SrNo = rowno+1;
                        bb_list.SrNo =Convert.ToInt64(row["SrNo"].ToString());
                        bb_list.Acc_Name = row["acc_name"].ToString();
                        bb_list.Vou_No = row["doc_no"].ToString();
                        bb_list.Vou_Dt = row["doc_dt"].ToString();
                        bb_list.Ins_Type = row["ins_type"].ToString();
                        bb_list.Ins_No = row["ins_no"].ToString();
                        bb_list.Reconciled = row["reconciled"].ToString();
                        bb_list.Narr = row["narr"].ToString();
                        bb_list.Dr_Amt = row["dr_amt"].ToString();
                        bb_list.Cr_Amt = row["cr_amt"].ToString();
                        bb_list.Closing_Bal = row["closing_bal"].ToString();
                        bb_list.Bal_Type = row["closing_bal_type"].ToString();
                        //bb_list.Acc_Name = row["Acc_Name"].ToString();
                        //bb_list.Vou_No = row["Vou_No"].ToString();
                        //bb_list.Vou_Dt = row["Vou_Dt"].ToString();
                        //bb_list.Ins_Type = row["Ins_Type"].ToString();
                        //bb_list.Ins_No = row["Ins_No"].ToString();
                        //bb_list.Reconciled = row["Reconciled"].ToString();
                        //bb_list.Narr = row["Narr"].ToString();
                        //bb_list.Dr_Amt = row["Dr_Amt"].ToString();
                        //bb_list.Cr_Amt = row["Cr_Amt"].ToString();
                        //bb_list.Closing_Bal = row["Closing_Bal"].ToString();
                        //bb_list.Bal_Type = row["Bal_Type"].ToString();
                        _BB_Detail_List.Add(bb_list);
                        //rowno = rowno + 1;
                    }
                }
                //var ItemListData = (from tempitem in _BB_Detail_List select tempitem);//Commented by Suraj Maurya on 20-06-2025

                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    ItemListData = ItemListData.OrderBy(p=>p.Acc_Name);
                //}

                //string searchValue = _BankBookModel.searchValue;
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    searchValue = searchValue.ToUpper();
                //    ItemListData = ItemListData.Where(m => m.Acc_Name.ToUpper().Contains(searchValue) || m.Vou_No.ToUpper().Contains(searchValue)
                //    || m.Vou_Dt.ToUpper().Contains(searchValue) || m.Ins_Type.ToUpper().Contains(searchValue) || m.Ins_No.ToUpper().Contains(searchValue)
                //    || m.Reconciled.ToUpper().Contains(searchValue) || m.Narr.ToUpper().Contains(searchValue) || m.Dr_Amt.ToUpper().Contains(searchValue)
                //    || m.Cr_Amt.ToUpper().Contains(searchValue) || m.Closing_Bal.ToUpper().Contains(searchValue) || m.Bal_Type.ToUpper().Contains(searchValue));
                //}
                var data = _BB_Detail_List.ToList();
                _BankBookModel.hdnCSVPrint = null;
                DataTable dt1 = new DataTable();
                dt1 = ToBankBookDetailExl(data);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Bank Book", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToBankBookDetailExl(List<BankBookList> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr. No.", typeof(int));
            dataTable.Columns.Add("GL Account", typeof(string));
            dataTable.Columns.Add("GL Voucher Number", typeof(string));
            dataTable.Columns.Add("GL Voucher Date", typeof(string));
            dataTable.Columns.Add("Instrument Type", typeof(string));
            dataTable.Columns.Add("Instrument  Number", typeof(string));
            dataTable.Columns.Add("Reconciled", typeof(string));
            dataTable.Columns.Add("Narration", typeof(string));
            dataTable.Columns.Add("Debit Amount", typeof(decimal));
            dataTable.Columns.Add("Credit Amount", typeof(decimal));
            dataTable.Columns.Add("Closing Balance", typeof(decimal));
            dataTable.Columns.Add("Balance Type", typeof(string));
            //int i = 1;
            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                //rows["Sr. No."] = i;
                rows["Sr. No."] = item.SrNo;
                rows["GL Account"] = item.Acc_Name.Replace('\n',' ').Replace('\t',' ');
                rows["GL Voucher Number"] = item.Vou_No;
                rows["GL Voucher Date"] = item.Vou_Dt;
                rows["Instrument Type"] = item.Ins_Type;
                rows["Instrument  Number"] = item.Ins_No;
                rows["Reconciled"] = item.Reconciled;
                rows["Narration"] = item.Narr.Replace('\n', ' ').Replace('\t', ' ');
                rows["Debit Amount"] = item.Dr_Amt;
                rows["Credit Amount"] = item.Cr_Amt;
                rows["Closing Balance"] = item.Closing_Bal;
                rows["Balance Type"] = item.Bal_Type;
                dataTable.Rows.Add(rows);
                //i++;
            }
            return dataTable;
        }
        private DataTable Getcurr(string Currtype)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = BankBook_ISERVICE.GetCurrList(Comp_ID, Currtype);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataSet GetFyList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                DataSet dt = BankBook_ISERVICE.Get_FYList(Comp_ID, BrID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult GetAutoCompleteAcc_List(BankBookModel BankBookModel)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            BrID = Session["BranchId"].ToString();
            string AccName = string.Empty;
            Dictionary<string, string> GLList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(BankBookModel.Acc_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = BankBookModel.Acc_name;
                }
                GLList = BankBook_ISERVICE.BB_AccList(Comp_ID, BrID, AccName);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(GLList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchBankBookDetail(string acc_id, string curr_id, string Fromdate, string Todate)
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
                _BankBookModel = new BankBookModel();
                List<BankBookList> _BB_Detail_List = new List<BankBookList>();

                //DataSet dtdata = BankBook_ISERVICE.GetBankBookDetails(CompID, BrID, acc_id, curr_id, Fromdate, Todate);
                //Commented by Suraj Maurya on 19-06-2025
                //if (dtdata.Tables[0].Rows.Count > 0)
                //{
                //    _BankBookModel.Opening_Bal = dtdata.Tables[0].Rows[0]["opening_bal"].ToString();
                //    _BankBookModel.Opening_BalType = dtdata.Tables[0].Rows[0]["opening_bal_type"].ToString();
                //    _BankBookModel.Closing_Bal = dtdata.Tables[0].Rows[0]["closing_bal"].ToString();
                //    _BankBookModel.Closing_BalType = dtdata.Tables[0].Rows[0]["closing_bal_type"].ToString();
                //}

                //    if (dtdata.Tables[1].Rows.Count > 0)
                //    {
                //    foreach (DataRow row in dtdata.Tables[1].Rows)
                //    {
                //        BankBookList bb_list = new BankBookList();
                //        bb_list.Acc_Name = row["acc_name"].ToString();
                //        bb_list.Vou_No = row["doc_no"].ToString();
                //        bb_list.Vou_Dt = row["doc_dt"].ToString();
                //        bb_list.VouDt = row["voudt"].ToString();
                //        bb_list.Ins_Type = row["ins_type"].ToString();
                //        bb_list.Ins_No = row["ins_no"].ToString();
                //        bb_list.Reconciled = row["reconciled"].ToString();
                //        bb_list.Narr = row["narr"].ToString();
                //        bb_list.Dr_Amt = row["dr_amt"].ToString();
                //        bb_list.Cr_Amt = row["cr_amt"].ToString();
                //        bb_list.Closing_Bal = row["cloasing_amt"].ToString();
                //        bb_list.Bal_Type = row["closing_type"].ToString();
                //        _BB_Detail_List.Add(bb_list);
                //    }
                //    _BankBookModel.BankBook_List = _BB_Detail_List;
                //    ///*Code start add by Hina sharma on 05-05-2025, also commented on 16-05-2025*/
                //    //var len = _BankBookModel.BankBook_List.Count;
                //    //if (len > 0)
                //    //{
                //    //    var Flen = len - 1;
                //    //    _BankBookModel.Closing_Bal = _BankBookModel.BankBook_List[Flen].Closing_Bal;
                //    //    _BankBookModel.Closing_BalType = _BankBookModel.BankBook_List[Flen].Bal_Type;
                //    //}
                //    ///*Code End add by Hina sharma on 05-05-2025, also commented on 16-05-2025*/
                //}

                //ViewBag.MenuPageName = getDocumentName();
                //_BankBookModel.Title = title;
                //Session["BB_listByFilter"] = "BBlistByFilter";
                //Commented by Suraj Maurya on 19-06-2025 End
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialBankBook.cshtml", _BankBookModel);
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

        /*--------------------------For PDF Print Start--------------------------*/

        [HttpPost]
        public FileResult GenratePdfFile(BankBookModel _model)
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
                    BrID = Session["BranchId"].ToString();
                }

                //DataTable Details = new DataTable();//Commented by Suraj Maurya on 20-06-2025
                //Details = ToDataTable(_model.PrintData);

                /*---- Added by Suraj Maurya on 20-06-2025 ----*/
                Search_Parmeters model1 = new Search_Parmeters();
                model1.CompId = CompID;
                model1.BrId = BrID;
                model1.AccID = _model.BankAcc_Id;
                model1.CurrId = _model.curr;
                model1.From_dt = _model.FromDate;
                model1.To_dt = _model.ToDate;
                model1.Flag = "CSV";
                DataSet DSet = BankBook_ISERVICE.GetBankBookDetails(model1, 0, 25, "", "SrNo", "asc");
                /*---- Added by Suraj Maurya on 20-06-2025 End ----*/
                _model.FromDate = Convert.ToDateTime(_model.FromDate).ToString("dd-MM-yyyy");
                _model.ToDate = Convert.ToDateTime(_model.ToDate).ToString("dd-MM-yyyy");
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, BrID);
                ViewBag.CompLogoDtl = dtlogo;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.DocName = "Bank Book";
                ViewBag.DocumentMenuId = DocumentMenuId;
                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4.Rotate(), 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    ViewBag.PrintBankBook = DSet.Tables[1]/*Details*/;/*Modified by Suraj Maurya 20-06-2025*/
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/BankBook/BankBookPrint.cshtml", _model));
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
                    return File(bytes.ToArray(), "application/pdf", "BankBook.pdf");
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
        private DataTable ToDataTable(string Details)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("Acc_Name", typeof(string));
                dtItem.Columns.Add("Vou_No", typeof(string));
                dtItem.Columns.Add("Vou_Dt", typeof(string));
                dtItem.Columns.Add("Ins_Type", typeof(string));
                dtItem.Columns.Add("Ins_No", typeof(string));
                dtItem.Columns.Add("Reconciled", typeof(string));
                dtItem.Columns.Add("Narr", typeof(string));
                dtItem.Columns.Add("Dr_Amt", typeof(string));
                dtItem.Columns.Add("Cr_Amt", typeof(string));
                dtItem.Columns.Add("Closing_Bal", typeof(string));
                dtItem.Columns.Add("Bal_Type", typeof(string));

                if (Details != null)
                {
                    JArray jObject = JArray.Parse(Details);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["Acc_Name"] = jObject[i]["Acc_Name"].ToString();
                        dtrowLines["Vou_No"] = jObject[i]["Vou_No"].ToString();
                        dtrowLines["Vou_Dt"] = jObject[i]["Vou_Dt"].ToString();
                        dtrowLines["Ins_Type"] = jObject[i]["Ins_Type"].ToString();
                        dtrowLines["Ins_No"] = jObject[i]["Ins_No"].ToString();
                        dtrowLines["Reconciled"] = jObject[i]["Reconciled"].ToString();
                        dtrowLines["Narr"] = jObject[i]["Narr"].ToString();
                        dtrowLines["Dr_Amt"] = jObject[i]["Dr_Amt"].ToString();
                        dtrowLines["Cr_Amt"] = jObject[i]["Cr_Amt"].ToString();
                        dtrowLines["Closing_Bal"] = jObject[i]["Closing_Bal"].ToString();
                        dtrowLines["Bal_Type"] = jObject[i]["Bal_Type"].ToString();

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
        /*--------------------------For PDF Print End--------------------------*/
        /*---------------------------------------DataTable Functionality-------------------------------------------*/
        //Added by Suraj Maurya on 20-06-2026
        public JsonResult LoadDetailsDataTable(Search_model model)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToUpper();

                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                List<BankBookList> _DetailedModel = new List<BankBookList>();
                List<BankBookModel> _TotalModel = new List<BankBookModel>(); 

                (_DetailedModel, _TotalModel, recordsTotal) = getDtList(model, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                // Getting all Customer data    
                //var ItemListData = (from tempitem in _DetailedModel select tempitem);

                //Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                //}

                //Paging     
                //var data = ItemListData.ToList();
                var data = _DetailedModel.ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data, footer= _TotalModel });

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        //Added by Suraj Maurya on 20-06-2026
        private (List<BankBookList>, List<BankBookModel>, int) getDtList(Search_model search_Model, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir)
        {
            List<BankBookList> _ItemListModel = new List<BankBookList>();
            List<BankBookModel> _TotalModel = new List<BankBookModel>();
            int Total_Records = 0;
            try
            {
                DataSet DSet = new DataSet();
                if (Session["CompId"] != null)
                {
                    search_Model.search_prm.CompId = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    search_Model.search_prm.BrId = Session["BranchId"].ToString();
                }
                var model1 = search_Model.search_prm;

                DSet = BankBook_ISERVICE.GetBankBookDetails(model1, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                if (DSet.Tables.Count >= 2)
                {
                    if (DSet.Tables[0].Rows.Count > 0)
                    {
                        _TotalModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new BankBookModel
                {
                    Opening_Bal = row.Field<string>("opening_bal"),
                    Opening_BalType = row.Field<string>("opening_bal_type"),
                    Closing_Bal = row.Field<string>("closing_bal"),
                    Closing_BalType = row.Field<string>("closing_bal_type"),
                    
                }).ToList();
                    }
                    if (DSet.Tables[1].Rows.Count > 0)
                    {
                        if (model1.Flag == "CSV")
                        {
                            _ItemListModel = DSet.Tables[1].AsEnumerable()
                .Select((row, index) => new BankBookList
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    Acc_Name = row.Field<string>("acc_name"),
                    Vou_No = row.Field<string>("doc_no"),
                    Vou_Dt = row.Field<string>("doc_dt"),
                    VouDt = row.Field<string>("voudt"),
                    Ins_Type = row.Field<string>("ins_type"),
                    Ins_No = row.Field<string>("ins_no"),
                    Reconciled = row.Field<string>("reconciled"),
                    Narr = row.Field<string>("narr"),
                    Dr_Amt = row.Field<string>("dr_amt").Replace(",",""),
                    Cr_Amt = row.Field<string>("cr_amt").Replace(",", ""),
                    Closing_Bal = row.Field<string>("cloasing_amt").Replace(",", ""),
                    Bal_Type = row.Field<string>("closing_type")
                }).ToList();
                        }
                        else
                        {
                            _ItemListModel = DSet.Tables[1].AsEnumerable()
                .Select((row, index) => new BankBookList
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    Acc_Name = row.Field<string>("acc_name"),
                    Vou_No = row.Field<string>("doc_no"),
                    Vou_Dt = row.Field<string>("doc_dt"),
                    VouDt = row.Field<string>("voudt"),
                    Ins_Type = row.Field<string>("ins_type"),
                    Ins_No = row.Field<string>("ins_no"),
                    Reconciled = row.Field<string>("reconciled"),
                    Narr = row.Field<string>("narr"),
                    Dr_Amt = row.Field<string>("dr_amt"),
                    Cr_Amt = row.Field<string>("cr_amt"),
                    Closing_Bal = row.Field<string>("cloasing_amt"),
                    Bal_Type = row.Field<string>("closing_type")
                }).ToList();
                        }
                    }
                    if (DSet.Tables[2].Rows.Count > 0)
                    {
                        Total_Records = Convert.ToInt32(DSet.Tables[2].Rows[0]["total_rows"]);
                    }
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
            return (_ItemListModel, _TotalModel, Total_Records);
        }

        string IsNull(string InStr, string OutStr)
        {
            return string.IsNullOrEmpty(InStr) ? OutStr : InStr;
        }

        [HttpPost]
        public ActionResult ExportCsv([System.Web.Http.FromBody] DataTableRequest request, Search_model model)
        {
            string keyword = "";
            // Apply search filter
            if (!string.IsNullOrEmpty(request.search?.value))
            {
                keyword = request.search.value;//.ToLower();
            }
            int recordsTotal = 0;
            string sortColumn = "SrNo";
            string sortColumnDir = "asc";
            if (request.order != null && request.order.Any())
            {
                var colIndex = request.order[0].column;
                sortColumnDir = request.order[0].dir;
                sortColumn = request.columns[colIndex].name;

            }
            List<BankBookList> _DetailedModel = new List<BankBookList>();
            List<BankBookModel> _TotalModel = new List<BankBookModel>();
            // 🔹 Fetch data same as LoadData but ignore paging
            model.search_prm.Flag = "CSV";
            (_DetailedModel, _TotalModel, recordsTotal) = getDtList(model, 0, request.length, keyword, sortColumn, sortColumnDir);


            var data = _DetailedModel.ToList(); // All filtered & sorted rows
            var commonController = new CommonController(_Common_IServices);
            return commonController.Cmn_GetDataToCsv(request, data);
        }
        /*---------------------------------------DataTable Functionality End-------------------------------------------*/
    }

}