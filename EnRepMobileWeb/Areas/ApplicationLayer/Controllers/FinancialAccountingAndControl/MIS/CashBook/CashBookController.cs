using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.CashBook;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.CashBook;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.CashBook
{
    public class CashBookController : Controller
    {
        string CompID, BrID, title, language = String.Empty;
        string DocumentMenuId = "105104135135";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        Common_IServices _Common_IServices;
        CashBook_ISERVICE _CashBook_ISERVICE;
        CashBookModel _CashBookModel;
        public CashBookController(Common_IServices _Common_IServices, CashBook_ISERVICE CashBook_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._CashBook_ISERVICE = CashBook_ISERVICE;
        }
        // GET: ApplicationLayer/CashBook
        public ActionResult CashBook()
        {
            _CashBookModel = new CashBookModel();

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
                _CashBookModel.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();
                ViewBag.ToFyMindate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();/*Add by Hina on 13-08-2024 for current date*/
                ViewBag.fylist = dttbl.Tables[1];
            }
            _CashBookModel.currList = currList;

            ViewBag.MenuPageName = getDocumentName();
            _CashBookModel.Title = title;
            ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/CashBook/CashBook.cshtml", _CashBookModel);
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
                DataTable dt = _CashBook_ISERVICE.GetCurrList(Comp_ID, Currtype);
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
                DataSet dt = _CashBook_ISERVICE.Get_FYList(Comp_ID, BrID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult CashBookdetail(CashBookModel CashBookModel, string command)
        {
            try
            {
                if (CashBookModel.hdnPDFPrint == "Print")
                {
                    command = "Print";
                }
                if (CashBookModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "Print":
                        return GenratePdfFile(CashBookModel);
                    case "CsvPrint":
                        return ExportCashBookData(CashBookModel);
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
        public FileResult ExportCashBookData(CashBookModel CashBookModel)
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

                //DataTable Details = new DataTable();
                //Details = ToDataTable(CashBookModel.PrintData);
                /* ---------- Added by Suraj Maurya on 20-06-2025 ----------- */
                Search_Parmeters model1 = new Search_Parmeters();
                model1.CompId = CompID;
                model1.BrId = BrID;
                model1.AccID = CashBookModel.AccId;
                model1.CurrId = CashBookModel.curr;
                model1.From_dt = CashBookModel.FromDate;
                model1.To_dt = CashBookModel.ToDate;
                model1.Flag = "CSV";
                var SearchValue = IsNull(CashBookModel.searchValue, "");
                var Direction = IsNull(CashBookModel.sortColumnDir, "asc");
                var sortColumnNm = IsNull(CashBookModel.sortColumn, "SrNo");
                sortColumnNm = "acc_name";

                DataSet DSet = _CashBook_ISERVICE.GetCashBookDetails(model1, 0, 50, SearchValue, sortColumnNm, Direction);
                /* ---------- Added by Suraj Maurya on 20-06-2025 End ----------- */
                DataTable dt = new DataTable();
                //List<ARList> _ARListDetail = new List<ARList>();
                List<CashBookList> _CB_Detail_List = new List<CashBookList>();
                dt = DSet.Tables[1]/*Details*/;
                if (dt.Rows.Count > 0)
                {
                    //int rowno = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        CashBookList _CurrList = new CashBookList();
                        //_CurrList.SrNo = rowno + 1;
                        _CurrList.SrNo =Convert.ToInt64(dr["SrNo"].ToString());
                        _CurrList.acc_name = dr["acc_name"].ToString();
                        _CurrList.doc_no = dr["doc_no"].ToString();
                        _CurrList.doc_dt = dr["doc_dt"].ToString();
                        _CurrList.narr = dr["narr"].ToString();
                        _CurrList.dr_amt = dr["dr_amt"].ToString();
                        _CurrList.cr_amt = dr["cr_amt"].ToString();
                        _CurrList.cloasing_amt = dr["closing_bal"].ToString();
                        _CurrList.closing_type = dr["closing_bal_type"].ToString();
                        //_CurrList.Acc_Name = dr["Acc_Name"].ToString();
                        //_CurrList.Vou_No = dr["Vou_No"].ToString();
                        //_CurrList.Vou_Dt = dr["Vou_Dt"].ToString();
                        //_CurrList.Narr = dr["Narr"].ToString();
                        //_CurrList.Dr_Amt = dr["Dr_Amt"].ToString();
                        //_CurrList.Cr_Amt = dr["Cr_Amt"].ToString();
                        //_CurrList.Closing_Bal = dr["Closing_Bal"].ToString();
                        //_CurrList.Bal_Type = dr["Bal_Type"].ToString();
                        _CB_Detail_List.Add(_CurrList);
                        //rowno = rowno + 1;
                    }
                }
                //var ItemListData = (from tempitem in _CB_Detail_List select tempitem);
                //string searchValue = CashBookModel.searchValue;
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    searchValue = searchValue.ToUpper();
                //    ItemListData = ItemListData.Where(m => m.Acc_Name.ToUpper().Contains(searchValue) || m.Vou_No.ToUpper().Contains(searchValue)
                //    || m.Vou_Dt.ToUpper().Contains(searchValue) || m.Narr.ToUpper().Contains(searchValue) || m.Dr_Amt.ToUpper().Contains(searchValue)
                //    || m.Cr_Amt.ToUpper().Contains(searchValue) || m.Closing_Bal.ToUpper().Contains(searchValue) || m.Bal_Type.ToUpper().Contains(searchValue));
                //}
                //var data = ItemListData.ToList();
                CashBookModel.hdnCSVPrint = null;
                DataTable dt1 = new DataTable();
                dt1 = ToCashBookDetailExl(_CB_Detail_List);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Cash Book", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToCashBookDetailExl(List<CashBookList> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr. No.", typeof(int));
            dataTable.Columns.Add("GL Account", typeof(string));
            dataTable.Columns.Add("GL Voucher Number", typeof(string));
            dataTable.Columns.Add("GL Voucher Date", typeof(string));
            dataTable.Columns.Add("Narration", typeof(string));
            dataTable.Columns.Add("Debit Amount", typeof(decimal));
            dataTable.Columns.Add("Credit Amount", typeof(decimal));
            dataTable.Columns.Add("Closing Balance", typeof(decimal));
            dataTable.Columns.Add("Balance Type", typeof(string));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr. No."] = item.SrNo;
                rows["GL Account"] = item.acc_name;
                rows["GL Voucher Number"] = item.doc_no;
                rows["GL Voucher Date"] = item.doc_dt;
                rows["Narration"] = item.narr;
                rows["Debit Amount"] = item.dr_amt;
                rows["Credit Amount"] = item.cr_amt;
                rows["Closing Balance"] = item.cloasing_amt;
                rows["Balance Type"] = item.closing_type;
                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        public ActionResult GetAutoCompleteAccList(CashBookModel CashBookModel)
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
                if (string.IsNullOrEmpty(CashBookModel.AccName))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = CashBookModel.AccName;
                }
                GLList = _CashBook_ISERVICE.GetCashBookAcc_List(Comp_ID, BrID, AccName);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(GLList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchCashBookDetail(string acc_id, string curr_id, string Fromdate, string Todate)
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
                _CashBookModel = new CashBookModel();
                List<CashBookList> _CB_Detail_List = new List<CashBookList>();
                //DataSet dtdata = _CashBook_ISERVICE.GetCashBookDetails(CompID, BrID, acc_id, curr_id, Fromdate, Todate);

                //if (dtdata.Tables[0].Rows.Count > 0)
                //{
                //    _CashBookModel.Opening_Bal = dtdata.Tables[0].Rows[0]["opening_bal"].ToString();
                //    _CashBookModel.Opening_BalType = dtdata.Tables[0].Rows[0]["opening_bal_type"].ToString();
                //    _CashBookModel.Closing_Bal = dtdata.Tables[0].Rows[0]["closing_bal"].ToString();
                //    _CashBookModel.Closing_BalType = dtdata.Tables[0].Rows[0]["closing_bal_type"].ToString();
                //}

                //if (dtdata.Tables[1].Rows.Count > 0)
                //{
                //    foreach (DataRow row in dtdata.Tables[1].Rows)
                //    {
                //        CashBookList cb_list = new CashBookList();
                //        cb_list.Acc_Name = row["acc_name"].ToString();
                //        cb_list.Vou_No = row["doc_no"].ToString();
                //        cb_list.Vou_Dt = row["doc_dt"].ToString();
                //        cb_list.VouDt = row["voudt"].ToString();
                //        cb_list.Narr = row["narr"].ToString();
                //        cb_list.Dr_Amt = row["dr_amt"].ToString();
                //        cb_list.Cr_Amt = row["cr_amt"].ToString();
                //        cb_list.Closing_Bal = row["cloasing_amt"].ToString();
                //        cb_list.Bal_Type = row["closing_type"].ToString();
                //        _CB_Detail_List.Add(cb_list);
                //    }
                //    _CashBookModel.CashBook_List = _CB_Detail_List;
                //    /*Code start add by Hina sharma on 05-05-2025, also commented on 16-05-2025*/
                //    var len = _CashBookModel.CashBook_List.Count;
                //    if (len > 0)
                //    {
                //        var Flen = len - 1;
                //        _CashBookModel.Closing_Bal = _CashBookModel.CashBook_List[Flen].Closing_Bal;
                //        _CashBookModel.Closing_BalType = _CashBookModel.CashBook_List[Flen].Bal_Type;
                //    }
                //    /*Code End add by Hina sharma on 05-05-2025,also commented on 16-05-2025*/
                //}

                //Session["CB_listByFilter"] = "CBlistByFilter";
                ViewBag.MenuPageName = getDocumentName();
                _CashBookModel.Title = title;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialCashBook.cshtml", _CashBookModel);
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
        public FileResult GenratePdfFile(CashBookModel _model)
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

                //DataTable Details = new DataTable();
                //Details = ToDataTable(_model.PrintData);

                /* ---------- Added by Suraj Maurya on 20-06-2025 ----------- */
                Search_Parmeters model1 = new Search_Parmeters();
                model1.CompId = CompID;
                model1.BrId = BrID;
                model1.AccID = _model.AccId;
                model1.CurrId = _model.curr;
                model1.From_dt = _model.FromDate;
                model1.To_dt = _model.ToDate;
                model1.Flag = "CSV";
                DataSet DSet = _CashBook_ISERVICE.GetCashBookDetails(model1, 0, 25, IsNull(_model.searchValue, ""), "SrNo", "asc");
                /* ---------- Added by Suraj Maurya on 20-06-2025 End ----------- */

                _model.FromDate = Convert.ToDateTime(_model.FromDate).ToString("dd-MM-yyyy");
                _model.ToDate = Convert.ToDateTime(_model.ToDate).ToString("dd-MM-yyyy");
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, BrID);
                ViewBag.CompLogoDtl = dtlogo;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.DocName = "Cash Book";
                ViewBag.DocumentMenuId = DocumentMenuId;
                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4.Rotate(), 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    ViewBag.PrintCashBook = DSet.Tables[1];
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/CashBook/CashBookPrint.cshtml", _model));
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
                    return File(bytes.ToArray(), "application/pdf", "CashBook.pdf");
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

                List<CashBookList> _DetailedModel = new List<CashBookList>();
                List<CashBookModel> _TotalModel = new List<CashBookModel>();

                (_DetailedModel, _TotalModel, recordsTotal) = getDtList(model, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _DetailedModel select tempitem);

                //Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                //}

                //Paging     
                var data = ItemListData.ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data,footer= _TotalModel });

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        private (List<CashBookList>, List<CashBookModel>, int) getDtList(Search_model search_Model, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir,string Flag = "")
        {
            List<CashBookList> _ItemListModel = new List<CashBookList>();
            List<CashBookModel> _TotalModel = new List<CashBookModel>();
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
                model1.Flag = Flag;
                DSet = _CashBook_ISERVICE.GetCashBookDetails(model1, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                if (DSet.Tables.Count >= 2)
                {
                    if (DSet.Tables[0].Rows.Count > 0)
                    {
                                _TotalModel = DSet.Tables[0].AsEnumerable()
                        .Select((row, index) => new CashBookModel
                        {
                            Opening_Bal = row.Field<string>("opening_bal"),
                            Opening_BalType = row.Field<string>("opening_bal_type"),
                            Closing_Bal = row.Field<string>("closing_bal"),
                            Closing_BalType = row.Field<string>("closing_bal_type"),

                        }).ToList();
                    }
                    if (DSet.Tables[1].Rows.Count > 0)
                    {
                        if (Flag == "CSV")
                        {
                            _ItemListModel = DSet.Tables[1].AsEnumerable()
                            .Select((row, index) => new CashBookList
                            {
                                SrNo = row.Field<Int64>("SrNo"),
                                acc_name = row.Field<string>("acc_name"),
                                doc_no = row.Field<string>("doc_no"),
                                doc_dt = row.Field<string>("doc_dt"),
                                voudt = row.Field<string>("voudt"),
                                narr = row.Field<string>("narr"),
                                dr_amt = row.Field<string>("dr_amt").Replace(",",""),
                                cr_amt = row.Field<string>("cr_amt").Replace(",", ""),
                                cloasing_amt = row.Field<string>("cloasing_amt").Replace(",", ""),
                                closing_type = row.Field<string>("closing_type")
                            }).ToList();
                        }
                        else
                        {
                            _ItemListModel = DSet.Tables[1].AsEnumerable()
                            .Select((row, index) => new CashBookList
                            {
                                SrNo = row.Field<Int64>("SrNo"),
                                acc_name = row.Field<string>("acc_name"),
                                doc_no = row.Field<string>("doc_no"),
                                doc_dt = row.Field<string>("doc_dt"),
                                voudt = row.Field<string>("voudt"),
                                narr = row.Field<string>("narr"),
                                dr_amt = row.Field<string>("dr_amt"),
                                cr_amt = row.Field<string>("cr_amt"),
                                cloasing_amt = row.Field<string>("cloasing_amt"),
                                closing_type = row.Field<string>("closing_type")
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
        string IsNull(string InStr,string OutStr)
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
                sortColumn = request.columns[colIndex].data;

            }
            List<CashBookList> _DetailedModel = new List<CashBookList>();
            List<CashBookModel> _TotalModel = new List<CashBookModel>();
            // 🔹 Fetch data same as LoadData but ignore paging
            (_DetailedModel, _TotalModel, recordsTotal) = getDtList(model, 0, request.length, keyword, sortColumn, sortColumnDir,"CSV");


            var data = _DetailedModel.ToList(); // All filtered & sorted rows
            var commonController = new CommonController(_Common_IServices);
            return commonController.Cmn_GetDataToCsv(request, data);
        }
        /*---------------------------------------DataTable Functionality End-------------------------------------------*/
    }

}