using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.QuotationAnalysis;
using System;
using System.Web.Mvc;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.QuotationAnalysis;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.QuotationAnalysis
{
    public class QuotationAnalysisController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105101125", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        QuotationAnalysis_ISERVICES _QuotationAnalysis_ISERVICES;
        public QuotationAnalysisController(Common_IServices _Common_IServices, QuotationAnalysis_ISERVICES _quotationAnalysis_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._QuotationAnalysis_ISERVICES = _quotationAnalysis_ISERVICES;
        }
        // GET: ApplicationLayer/QuotationAnalysis
        public ActionResult QuotationAnalysis(string wfStatus)
        {
            try
            {
                GetCompDeatil();
                CommonPageDetails();
                QAList_Model pQList_Model = new QAList_Model();
                if (wfStatus != null)
                {
                    pQList_Model.WF_status = wfStatus;
                    pQList_Model.ListFilterData = "0,0,0,0" + "," + wfStatus;
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                List<statusLists> statusLists = new List<statusLists>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    statusLists list = new statusLists();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                pQList_Model.statusLists = statusLists;
                pQList_Model.Command = "Add";
                pQList_Model.TransType = "Save";
                pQList_Model.BtnName = "BtnAddNew";

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    if (ListFilterData != null && ListFilterData != "")
                    {
                        var a = ListFilterData.Split(',');
                        var SupplierID = a[0].Trim();
                        var Fromdate = a[1].Trim();
                        var Todate = a[2].Trim();
                        var Status = a[3].Trim();
                        if (Status == "0")
                        {
                            Status = null;
                        }
                        pQList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                        pQList_Model.PQA_FromDate = Fromdate;
                        pQList_Model.PQA_ToDate = Todate;
                        pQList_Model.PQA_status = Status;
                    }
                }
                else
                {
                    pQList_Model.PQA_FromDate = startDate;

                }
                GetAllListData(pQList_Model);
                pQList_Model.title = title;
                pQList_Model.PQASearch = "0";
                return View("~/Areas/ApplicationLayer/Views/Procurement/QuotationAnalysis/QuotationAnalysisList.cshtml", pQList_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void GetAllListData(QAList_Model _QAList_Model)
        {
            try
            {
                string SupplierName = string.Empty;
                CommonPageDetails();

                DataSet CustList = _QuotationAnalysis_ISERVICES.GetAllData(CompID, BrchID, UserID, _QAList_Model.PQA_FromDate,
                    _QAList_Model.PQA_ToDate, _QAList_Model.PQA_status, "105101125", _QAList_Model.WF_status);
                ViewBag.ListDetail = CustList.Tables[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        public ActionResult PQAListSearch(string Fromdate, string Todate, string Status, string wfStatus)
        {
            try
            {
                CommonPageDetails();
                DataSet ds = new DataSet();
                ds = _QuotationAnalysis_ISERVICES.GetAllData(CompID, BrchID, UserID, Fromdate, Todate, Status, "105101125", wfStatus);
                ViewBag.ListDetail = ds.Tables[0];
                ViewBag.ListSearch = "ListSearch";
                ViewBag.ListFilterData1 = Fromdate + "," + Todate + "," + Status + "," + wfStatus;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialQuotationAnalysisList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult AddQuotationAnalysisDetail(string DocNo, string DocDate, string ListFilterData)
        {
            try
            {
                UrlData urlData = new UrlData();
                /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (DocNo == null)
                {
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {

                        TempData["Message"] = "Financial Year not Exist";
                        SetUrlData(urlData, "", "", "", "Financial Year not Exist", DocNo, DocDate, ListFilterData);
                        return RedirectToAction("QuotationAnalysisDetail", "QuotationAnalysis", urlData);
                    }
                }

                /*End to chk Financial year exist or not*/
                string BtnName = DocNo == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = DocNo == null ? "Save" : "Update";
                SetUrlData(urlData, "Add", TransType, BtnName, null, DocNo, DocDate, ListFilterData);
                return RedirectToAction("QuotationAnalysisDetail", "QuotationAnalysis", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
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
        private void CommonPageDetails()
        {
            try
            {
                GetCompDeatil();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
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

        public ActionResult ApprovPQADetails(QuotationAnalysis_Model _model, string Inv_No, string Inv_Date
             , string A_Status, string A_Level, string A_Remarks, string VoucherNarr, string FilterData
             , string docid, string WF_Status1, string Bp_Nurr, string Dn_Nurration)
        {
            try
            {
                UrlData urlData = new UrlData();
                GetCompDeatil();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Result = _QuotationAnalysis_ISERVICES.InsertPQApproveDetails(Inv_No, Inv_Date, CompID, BrchID, DocumentMenuId,
                    UserID, mac_id, A_Status, A_Level, A_Remarks, VoucherNarr, "");
                try
                {
                    //string fileName = "IBP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    //var filePath = SavePdfDocToSendOnEmailAlert(Inv_No, Inv_Date, fileName, null);
                    //_Common_IServices.SendAlertEmail(CompID, BrchID, "105101153", _model.Inv_no, "AP", UserID, "", filePath);

                    string fileName = "QuotationAnalysis_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(Inv_No, Inv_Date, fileName, DocumentMenuId, "AP");
                    _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, Inv_No, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                if (Result.Split(',')[1] == "A")
                {
                    _model.Message = _model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                _model.Message = Result.Split(',')[1] == "O" ? "OrderRaised" : "Error";
                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, Result.Split(',')[0], Result.Split(',')[2], FilterData);
                return RedirectToAction("QuotationAnalysisDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType)
        {
            QuotationAnalysis_Model _purchaseQuotation_Model = new QuotationAnalysis_Model();
            //Session["Message"] = "";
            var a = TrancType.Split(',');
            _purchaseQuotation_Model.QA_No = a[0].Trim();
            _purchaseQuotation_Model.QA_Date = a[1].Trim();
            _purchaseQuotation_Model.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _purchaseQuotation_Model;
            TempData["ListFilterData"] = ListFilterData1;
            TempData["WF_status1"] = WF_status1;
            return RedirectToAction("QuotationAnalysisDetail");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuotationAnalysisDetailsActionCommands(QuotationAnalysis_Model _model, string Command)
        {
            try
            {
                var commCont = new CommonController(_Common_IServices);
                UrlData urlData = new UrlData();
                if (_model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "AddNew":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            //if (!string.IsNullOrEmpty(_model.Inv_no))
                            //{
                            //    SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", "Financial Year not Exist", _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                            //    return RedirectToAction("QuotationAnalysisDetail", _model);
                            //}
                            //return RedirectToAction("AddQuotationAnalysisDetail", new { DocNo = _model.SPO_No, DocDate = _model.SPO_Date, ListFilterData = _model.ListFilterData1, WF_status = _Model.WFStatus });
                            //else
                            //{
                            //    SetUrlData(urlData, "Refresh", "Refresh", "Refresh", "Financial Year not Exist", null, null, _model.ListFilterData1);
                            //    return RedirectToAction("QuotationAnalysisDetail", _model);
                            //}
                        }
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, null, _model.ListFilterData1);
                        return RedirectToAction("QuotationAnalysisDetail", urlData);
                    case "Save":
                        SavePQADetails(_model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.QA_No, _model.QA_Date, _model.ListFilterData1);
                        return RedirectToAction("QuotationAnalysisDetail", urlData);
                    case "Approve":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        //string Invdt1 = _model.Inv_dt;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, Invdt1) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", _model.Message, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                        //    return RedirectToAction("QuotationAnalysisDetail", urlData);
                        //}
                        ApprovPQADetails(_model, _model.QA_No, _model.QA_Date, "", "", "", "", "", "", "", "", "");
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.QA_No, _model.QA_Date, _model.ListFilterData1);
                        return RedirectToAction("QuotationAnalysisDetail", urlData);
                    case "Edit":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        GetCompDeatil();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _model.QA_No, DocDate = _model.QA_Date, ListFilterData = _model.ListFilterData1, WF_status = _model.WFStatus });
                        }
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string invdt = _model.QA_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _model.QA_No, _model.QA_Date, _model.ListFilterData1);
                            return RedirectToAction("QuotationAnalysisDetail", urlData);
                        }
                        /*End to chk Financial year exist or not*/
                        if (_model.Status == "O")
                        {
                            string checkforCancle = CheckPQAForCancellation(_model.QA_No, _model.QA_Date);
                            if (checkforCancle != "")
                            {
                                //Session["Message"] = checkforCancle;
                                _model.Message = checkforCancle;
                                _model.BtnName = "BtnToDetailPage";
                                TempData["ModelData"] = _model;
                                _model.Command = "Add";
                                _model.TransType = "Update";
                                //TempData["FilterData"] = _model.FilterData1;
                            }
                            else
                            {
                                _model.TransType = "Update";
                                //_model.Command = command;
                                _model.BtnName = "BtnEdit";
                                _model.Message = null;
                                TempData["ModelData"] = _model;
                                //TempData["FilterData"] = _model.FilterData1;
                            }
                        }
                        else
                        {
                            _model.TransType = "Update";
                            _model.BtnName = "BtnEdit";
                            _model.Message = null;
                            Command = _model.Command;
                            TempData["ModelData"] = _model;
                            // TempData["FilterData"] = _model.FilterData1;
                        }
                        SetUrlData(urlData, _model.Command, "Update", _model.BtnName, _model.Message, _model.QA_No, _model.QA_Date, _model.ListFilterData1);
                        return RedirectToAction("QuotationAnalysisDetail", urlData);
                    case "Print":
                        return GenratePdfFile(_model);
                    case "Delete":
                        DeleteDPIDetail(_model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, null, _model.ListFilterData1);
                        return RedirectToAction("QuotationAnalysisDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _model.ListFilterData1);
                        return RedirectToAction("QuotationAnalysisDetail", urlData);
                    case "BacktoList":
                        TempData["WF_status"] = _model.WF_status1;
                        TempData["ListFilterData"] = _model.ListFilterData1;
                        SetUrlData(urlData, "", "", "", null, null, null, _model.ListFilterData1);
                        return RedirectToAction("QuotationAnalysis");
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("QuotationAnalysisDetail", urlData);
                }
                //return RedirectToAction("PurchaseQuotationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public string CheckPQAForCancellation(string DocNo, string DocDate)
        {
            string Result = "";
            try
            {
                GetCompDeatil();
                DataSet Deatils = _QuotationAnalysis_ISERVICES.CheckPQADetail(CompID, BrchID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = "Used";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            return Result;
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string Inv_no = null, string Inv_dt = null, string ListFilterData1 = null)
        {
            try
            {
                urlData.Command = Command;
                urlData.TransType = TransType;
                urlData.BtnName = BtnName;
                urlData.Inv_no = Inv_no;
                urlData.Inv_dt = Inv_dt;
                urlData.ListFilterData1 = ListFilterData1;
                TempData["UrlData"] = urlData;
                TempData["Message"] = Message;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }

        public FileResult GenratePdfFile(QuotationAnalysis_Model _model)
        {
            // return null;
            return File(GetPdfData(_model.QA_No, _model.QA_Date), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        }
        public byte[] GetPdfData(string invNo, string invDt)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                GetCompDeatil();
                DataSet Details = _QuotationAnalysis_ISERVICES.GetQuotationAnalysisDetailForPrint(CompID, BrchID, invNo, invDt);
                // DataSet Details = _QuotationAnalysis_ISERVICES.GetQuotationAnalysisDetailForPrint(CompID, BrchID, "FL/08/25/PDI0000052", "2025-08-28");
                ViewBag.PageName = "PI";
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["analy_status"].ToString().Trim();
                //ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                ViewBag.Title = "Quotation Analysis";
                htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/QuotationAnalysis/QuotationAnalysisPrint.cshtml"));

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4.Rotate(), 20f, 20f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    string draftImage = string.Empty;
                    if (ViewBag.DocStatus == "C")
                    {
                        draftImage = Server.MapPath("~/Content/Images/cancelled.png");
                    }
                    else
                    {
                        draftImage = Server.MapPath("~/Content/Images/draft.png");
                    }
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(0, 10);
                                draftimg.ScaleAbsolute(750f, 580f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F" || ViewBag.DocStatus == "C")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
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

        private void DeleteDPIDetail(QuotationAnalysis_Model _model)
        {
            try
            {
                GetCompDeatil();
                string Result = _QuotationAnalysis_ISERVICES.DeletePQDetails(CompID, BrchID, _model.QA_No, _model.QA_Date);
                _model.Message = Result.Split(',')[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
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
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        public DataSet GetDPIDetailEdit(string Inv_no, string Inv_dt, string DocumentMenuId)
        {
            try
            {
                GetCompDeatil();
                DataSet result = _QuotationAnalysis_ISERVICES.GetPQADetailDAL(CompID, BrchID, Inv_no, Inv_dt, UserID, DocumentMenuId);
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult QuotationAnalysisDetail(UrlData urlData)
        {
            try
            {
                CommonPageDetails();
                QuotationAnalysis_Model model = new QuotationAnalysis_Model();
                GetCompDeatil();
                /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, urlData.Inv_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                if (TempData["UrlData"] != null)
                {
                    urlData = TempData["UrlData"] as UrlData;
                    model.Message = TempData["Message"] != null ? TempData["Message"].ToString() : null;
                }
                model.Command = urlData.Command;
                model.TransType = urlData.TransType;
                model.BtnName = urlData.BtnName;
                model.QA_No = urlData.Inv_no;
                model.QA_Date = urlData.Inv_dt;
                model.ListFilterData1 = urlData.ListFilterData1;

                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.ValDigit = ValDigit;
                ViewBag.QtyDigit = QtyDigit;
                ViewBag.RateDigit = RateDigit;
                if (model.QA_No != null && model.QA_Date != null)
                {
                    GetAutoCompleteSearchRFQList(model, "E");
                }
                else
                {
                    GetAutoCompleteSearchRFQList(model, "");
                }

                if (model.QA_No != null && model.QA_Date != null)
                {
                    DataSet ds = GetDPIDetailEdit(model.QA_No, model.QA_Date, DocumentMenuId);
                    ViewBag.AttechmentDetails = ds.Tables[5];
                    ViewBag.PODetails = ds.Tables[6];
                    model.QA_No = ds.Tables[0].Rows[0]["analy_no"].ToString();
                    model.QA_Date = ds.Tables[0].Rows[0]["analy_dt"].ToString();
                    model.rfqID = ds.Tables[0].Rows[0]["rfq_no"].ToString();
                    model.rfqdt = ds.Tables[0].Rows[0]["rfq_dt"].ToString();
                    model.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                    model.Create_by = ds.Tables[0].Rows[0]["CreateName"].ToString();
                    model.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                    model.Approved_by = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                    model.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                    model.Amended_by = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                    model.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                    model.StatusName = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                    model.CancelledRemarks = ds.Tables[0].Rows[0]["Cancel_remarks"].ToString();
                    string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                    string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                    string doc_status = ds.Tables[0].Rows[0]["analy_status"].ToString().Trim();

                    model.DocumentStatus = doc_status;
                    model.Status = doc_status;
                    if (doc_status == "C")
                    {
                        model.Cancelled = true;
                        model.BtnName = "Refresh";
                    }
                    else
                    {
                        model.Cancelled = false;
                    }
                    model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                    model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                    if (doc_status != "D" && doc_status != "F")
                    {
                        ViewBag.AppLevel = ds.Tables[4];
                    }
                    if (ViewBag.AppLevel != null && model.Command != "Edit")
                    {
                        var sent_to = "";
                        var nextLevel = "";
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                        }
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                        }
                        if (doc_status == "D")
                        {
                            if (create_id != UserID)
                            {
                                model.BtnName = "Refresh";
                            }
                            else
                            {
                                if (nextLevel == "0")
                                {
                                    if (create_id == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                    }
                                    model.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    ViewBag.Approve = "N";
                                    ViewBag.ForwardEnbl = "Y";
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (UserID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                model.BtnName = "BtnToDetailPage";
                            }
                            if (nextLevel == "0")
                            {
                                if (sent_to == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    model.BtnName = "BtnToDetailPage";
                                }
                            }
                        }
                        if (doc_status == "F")
                        {
                            if (UserID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                model.BtnName = "BtnToDetailPage";
                            }
                            if (nextLevel == "0")
                            {
                                if (sent_to == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                }
                                model.BtnName = "BtnToDetailPage";
                            }
                        }
                        if (doc_status == "A")
                        {
                            if (create_id == UserID || approval_id == UserID)
                            {
                                model.BtnName = "BtnToDetailPage";
                            }
                            else
                            {
                                model.BtnName = "Refresh";
                            }
                        }
                    }
                    if (ViewBag.AppLevel.Rows.Count == 0)
                    {
                        ViewBag.Approve = "Y";
                    }
                    ViewBag.ItemDetailsList = ds.Tables[1];
                }

                List<RFQList> BillNoLists = new List<RFQList>();
                BillNoLists.Add(new RFQList { RFQ_id = model.rfqID, RFQ_value = model.rfqID });
                model.rfqLists = BillNoLists;

                model.title = title;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = model.TransType;
                ViewBag.DocumentStatus = model.DocumentStatus;
                ViewBag.Command = model.Command;
                return View("~/Areas/ApplicationLayer/Views/Procurement/QuotationAnalysis/QuotationAnalysisDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public void GetCompDeatil()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            if (Session["UserID"] != null)
            {
                UserID = Session["UserID"].ToString();
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
        }
        public ActionResult GetAutoCompleteSearchRFQList(QuotationAnalysis_Model model, string status)
        {
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            try
            {
                GetCompDeatil();
                CustList = _QuotationAnalysis_ISERVICES.GetRFQList(CompID, BrchID, status);
                List<RFQList> _SuppList = new List<RFQList>();
                foreach (var data in CustList)
                {
                    RFQList _SuppDetail = new RFQList();
                    _SuppDetail.RFQ_id = data.Key;
                    _SuppDetail.RFQ_value = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                model.rfqLists = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRFQList()
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet SO_OC = _QuotationAnalysis_ISERVICES.GetRFQListJS(CompID, BrchID, "");
                DataRows = Json(JsonConvert.SerializeObject(SO_OC));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult GetRFQDetail(string invno)
        {
            DataTable dt = new DataTable();
            try
            {
                PQItemDetail model = new PQItemDetail();
                QuotationAnalysis_Model model1 = new QuotationAnalysis_Model();
                GetCompDeatil();
                if (invno != null)
                {
                    DataSet Details = _QuotationAnalysis_ISERVICES.GetRFQDetail(CompID, BrchID, invno);
                    ViewBag.ItemDetailsList = Details.Tables[0];
                    ViewBag.TransType = "Save";
                    ViewBag.DocumentStatus = "";
                    ViewBag.Command = "Add";
                }
                return View("~/Areas/ApplicationLayer/Views/Shared/PartialQuotationAnalysisItemDetails.cshtml", model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public ActionResult SavePQADetails(QuotationAnalysis_Model _model)
        {
            try
            {
                string SaveMessage = "";
                string PageName = _model.title.Replace(" ", "");
                GetCompDeatil();

                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();

                DtblHDetail = ToDtblHeaderDetail(_model);
                DtblItemDetail = ToDtblItemDetail(_model.Itemdetails);
                DtblAttchDetail = ToDtblAttachmentDetail(_model);

                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as QuotationAnalysisattch;
                TempData["ModelDataattch"] = null;
                string Nurr = "";
                if (_model.Cancelled)
                {
                    // Nurr = _model. + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                }

                SaveMessage = _QuotationAnalysis_ISERVICES.InsertQTATransactionDetails(DtblHDetail, DtblItemDetail, DtblAttchDetail);
                if (SaveMessage == "DocModify")
                {
                    _model.Message = "DocModify";
                    _model.BtnName = "Refresh";
                    _model.Command = "Refresh";
                    return RedirectToAction("QuotationAnalysisDetail");
                }
                else
                {
                    string[] FDetail = SaveMessage.Split(',');
                    string Message = FDetail[5].ToString();
                    string Inv_no = FDetail[0].ToString();
                    string Inv_DATE = FDetail[6].ToString();
                    string Cansal = FDetail[1].ToString();
                    if (Message == "DataNotFound")
                    {
                        var msg = "Data Not found" + " " + Inv_DATE + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _model.Message = Message;
                        return RedirectToAction("QuotationAnalysisDetail");
                    }
                    else if (Message == "Save")
                    {
                        string Guid = "";
                        if (_DirectPurchaseInvoiceattch != null)
                        {
                            if (_DirectPurchaseInvoiceattch.Guid != null)
                            {
                                Guid = _DirectPurchaseInvoiceattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, _model.TransType, DtblAttchDetail);
                    }
                    else if (Cansal == "C" && Message == "Update")
                    {
                        try
                        {
                            string fileName = "QuotationAnalysis_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_model.QA_No, _model.QA_Date, fileName, DocumentMenuId, "C");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _model.QA_No, "C", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _model.Message = _model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";

                        _model.Command = "Update";
                        _model.QA_No = Inv_no;
                        _model.QA_Date = Inv_DATE;
                        _model.AppStatus = "D";
                        _model.BtnName = "Refresh";
                        _model.TransType = "Update";
                    }
                    if (Message == "Update" || Message == "Save")
                    {
                        _model.Message = "Save";
                        _model.Command = "Update";
                        _model.QA_No = Inv_no;
                        _model.QA_Date = Inv_DATE;
                        _model.AppStatus = "D";
                        _model.BtnName = "BtnSave";
                        _model.TransType = "Update";
                    }
                    return RedirectToAction("QuotationAnalysisDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private DataTable ToDtblHeaderDetail(QuotationAnalysis_Model _model)
        {
            try
            {
                DataTable dtheaderdeatil = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("MenuID", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(string));
                dtheader.Columns.Add("br_id", typeof(string));
                dtheader.Columns.Add("inv_no", typeof(string));
                dtheader.Columns.Add("inv_dt", typeof(string));
                dtheader.Columns.Add("bill_no", typeof(string));
                dtheader.Columns.Add("bill_dt", typeof(string));
                dtheader.Columns.Add("analy_status", typeof(string));
                dtheader.Columns.Add("user_id", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("Cancel_remarks", typeof(string));
                DataRow dtrowHeader = dtheader.NewRow();
                if (_model.QA_No != null)
                {
                    _model.TransType = "Update";
                    dtrowHeader["TransType"] = _model.TransType;
                }
                else
                {
                    _model.TransType = "Save";
                    dtrowHeader["TransType"] = _model.TransType;
                }
                dtrowHeader["MenuID"] = DocumentMenuId;
                string cancelflag = _model.Cancelled.ToString();
                if (cancelflag == "False")
                    dtrowHeader["Cancelled"] = "N";
                else
                    dtrowHeader["Cancelled"] = "Y";

                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                dtrowHeader["inv_no"] = _model.QA_No;
                dtrowHeader["inv_dt"] = _model.QA_Date;
                dtrowHeader["bill_no"] = _model.rfqID;
                dtrowHeader["bill_dt"] = _model.rfqdt;
                dtrowHeader["user_id"] = Session["UserId"].ToString();
                if (_model.AppStatus == null)
                {
                    _model.AppStatus = "D";
                    dtrowHeader["analy_status"] = _model.AppStatus;
                }
                else
                {
                    dtrowHeader["analy_status"] = _model.AppStatus;
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["Cancel_remarks"] = _model.CancelledRemarks;
                dtheader.Rows.Add(dtrowHeader);
                dtheaderdeatil = dtheader;
                return dtheaderdeatil;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblItemDetail(string ItemDetails)
        {
            try
            {
                DataTable dtItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("item_type", typeof(string));
                dtItem.Columns.Add("qt_qty", typeof(decimal));
                dtItem.Columns.Add("item_remarks", typeof(string));
                dtItem.Columns.Add("supp_id", typeof(string));
                dtItem.Columns.Add("supp_pros_type", typeof(string));
                dtItem.Columns.Add("supp_rating", typeof(string));
                dtItem.Columns.Add("qt_no", typeof(string));
                dtItem.Columns.Add("qt_dt", typeof(DateTime));
                dtItem.Columns.Add("item_rate", typeof(decimal));
                dtItem.Columns.Add("item_disc_perc", typeof(decimal));
                dtItem.Columns.Add("net_rate", typeof(decimal));
                dtItem.Columns.Add("item_net_val_bs", typeof(decimal));
                dtItem.Columns.Add("payment_terms_days", typeof(int));
                dtItem.Columns.Add("delivery_date", typeof(DateTime));
                dtItem.Columns.Add("order_for", typeof(string));

                if (ItemDetails != null)
                {
                    JArray jObject = JArray.Parse(ItemDetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["ItemID"]?.ToString();
                        dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["Uomid"]);
                        dtrowLines["item_type"] = jObject[i]["ItemType"]?.ToString();
                        dtrowLines["qt_qty"] = Convert.ToDecimal(jObject[i]["qt_qty"]);
                        dtrowLines["item_remarks"] = jObject[i]["ItemRemarks"]?.ToString();
                        dtrowLines["supp_id"] = jObject[i]["SuppID"]?.ToString();
                        dtrowLines["supp_pros_type"] = jObject[i]["SuppPros_type"]?.ToString();
                        dtrowLines["supp_rating"] = jObject[i]["suppRating"]?.ToString();
                        dtrowLines["qt_no"] = jObject[i]["qt_no"]?.ToString();
                        dtrowLines["qt_dt"] = Convert.ToDateTime(jObject[i]["qt_dt"]);
                        dtrowLines["item_rate"] = Convert.ToDecimal(jObject[i]["item_rate"]);
                        dtrowLines["item_disc_perc"] = Convert.ToDecimal(jObject[i]["item_disc_perc"]);
                        dtrowLines["net_rate"] = Convert.ToDecimal(jObject[i]["net_rate"]);
                        dtrowLines["item_net_val_bs"] = Convert.ToDecimal(jObject[i]["item_net_val_bs"]);
                        dtrowLines["payment_terms_days"] = Convert.ToInt32(jObject[i]["PaymentTermsInDays"]);
                        dtrowLines["delivery_date"] = Convert.ToDateTime(jObject[i]["DeliveryDate"]);
                        dtrowLines["order_for"] = jObject[i]["OrderFor"]?.ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                }
                dtItemDetail = dtItem;
                return dtItemDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        private DataTable ToDtblAttachmentDetail(QuotationAnalysis_Model _model)
        {
            try
            {
                string PageName = _model.title.Replace(" ", "");
                DataTable dtAttachment = new DataTable();
                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as DirectPurchaseInvoiceattch;

                if (_model.attatchmentdetail != null)
                {
                    if (_DirectPurchaseInvoiceattch != null)
                    {
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (_DirectPurchaseInvoiceattch.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _DirectPurchaseInvoiceattch.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }
                    else
                    {
                        if (_model.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _model.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }

                    JArray jObject1 = JArray.Parse(_model.attatchmentdetail);
                    for (int i = 0; i < jObject1.Count; i++)
                    {
                        string flag = "Y";
                        foreach (DataRow dr in dtAttachment.Rows)
                        {
                            string drImg = dr["file_name"].ToString();
                            string ObjImg = jObject1[i]["file_name"].ToString();
                            if (drImg == ObjImg)
                            {
                                flag = "N";
                            }
                        }
                        if (flag == "Y")
                        {
                            DataRow dtrowAttachment1 = dtAttachment.NewRow();
                            if (!string.IsNullOrEmpty(_model.QA_No))
                            {
                                dtrowAttachment1["id"] = _model.QA_Date;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y";
                            dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_model.TransType == "Update")
                    {
                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_model.QA_No))
                            {
                                ItmCode = _model.QA_No;
                            }
                            else
                            {
                                ItmCode = "0";
                            }
                            string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + ItmCode.Replace("/", "") + "*");

                            foreach (var fielpath in filePaths)
                            {
                                string flag = "Y";
                                foreach (DataRow dr in dtAttachment.Rows)
                                {
                                    string drImgPath = dr["file_path"].ToString();
                                    if (drImgPath == fielpath.Replace("/", @"\"))
                                    {
                                        flag = "N";
                                    }
                                }
                                if (flag == "Y")
                                {
                                    System.IO.File.Delete(fielpath);
                                }
                            }
                        }
                    }
                }
                return dtAttachment;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                DirectPurchaseInvoiceattch _ScrapSIModelattch = new DirectPurchaseInvoiceattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                string branchID = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _ScrapSIModelattch.Guid = DocNo;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    branchID = Session["BranchId"].ToString();
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + branchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _ScrapSIModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _ScrapSIModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _ScrapSIModelattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult ErrorPage()
        {
            try
            {
                throw new Exception("JsError");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPOItemDetail(string PO_No, string PO_Date)
        {
            DataTable dt = new DataTable();
            try
            {
                PQItemDetail model = new PQItemDetail();
                QuotationAnalysis_Model model1 = new QuotationAnalysis_Model();
                GetCompDeatil();
                var podt = Convert.ToDateTime(PO_Date).ToString("d");
                if (PO_No != null)
                {
                    DataSet Details = _QuotationAnalysis_ISERVICES.GetPOItemDetailDAL(CompID, BrchID, PO_No, podt);
                    ViewBag.POItemDetailsList = Details.Tables[0];
                    ViewBag.POItemTotalList = Details.Tables[1];
                    ViewBag.OtherChargeDetails = Details.Tables[2];
                    ViewBag.TransType = "Save";
                    ViewBag.DocumentStatus = "";
                    ViewBag.Command = "Add";
                    ViewBag.POSupplier = Details.Tables[0].Rows[0]["supp_name"].ToString();
                    ViewBag.PONO = Details.Tables[0].Rows[0]["po_no"].ToString();
                    ViewBag.PODate = Details.Tables[0].Rows[0]["po_date"].ToString();
                }
                return View("~/Areas/ApplicationLayer/Views/Shared/PartialQAOrderValueDetail.cshtml", model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public JsonResult GetPOItemDetailJson(string PO_No, string PO_Date)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                var podt = Convert.ToDateTime(PO_Date).ToString("d");
                DataSet Details = _QuotationAnalysis_ISERVICES.GetPOItemDetailDAL(CompID, BrchID, PO_No, podt);
                DataRows = Json(JsonConvert.SerializeObject(Details));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        //Added by Nidhi on 08-07-2025
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            GetCompDeatil();
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, Doc_dt);
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return "ErrorPage";
            }
            return null;
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            var WF_status1 = "";
            QuotationAnalysis_Model _Model = new QuotationAnalysis_Model();
            var a = TrancType.Split(',');
            _Model.QA_No = a[0].Trim();
            _Model.QA_Date = a[1].Trim();
            _Model.TransType = a[2].Trim();
            if (a[3].Trim() != "" && a[3].Trim() != null)
            {
                WF_status1 = a[3].Trim();
                _Model.WF_status1 = WF_status1;
            }
            var docId = a[4].Trim();
            _Model.Message = Mailerror;
            _Model.BtnName = "BtnToDetailPage";
            _Model.DocumentMenuId = docId;
            TempData["ModelData"] = _Model;
            TempData["WF_status1"] = WF_status1; ;
            TempData["ListFilterData"] = ListFilterData1;
            UrlData URLModel = new UrlData();
            URLModel.Inv_no = a[0].Trim();
            URLModel.Inv_dt = a[01].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.DocumentMenuId = docId;
            URLModel.BtnName = "BtnToDetailPage";
            return RedirectToAction("QuotationAnalysisDetail", URLModel);
        }
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, string QtNo, string Doc_no, string Doc_dt)
        {
            try
            {
                GetCompDeatil();
                DataTable dt = new DataTable();
                dt = _QuotationAnalysis_ISERVICES.GetSubItemDetailsAfterApprove(CompID, BrchID, Item_id, Doc_no, Doc_dt, "QTy").Tables[0];
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = "PRFQQtQty",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y"
                };
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }

}

