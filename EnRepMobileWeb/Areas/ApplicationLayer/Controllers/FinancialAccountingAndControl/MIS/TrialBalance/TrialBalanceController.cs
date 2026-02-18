using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.TrialBalance;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.TrialBalance;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.TrialBalance
{
    public class TrialBalanceController : Controller
    {
        string CompID, BrID, UserID, language, title = string.Empty;
        string DocumentMenuId = "105104135101";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        TrialBalanceModel _TrialBalanceModel;
        TrialBalance_ISERVICE _TrialBalance_ISERVICE;
        public TrialBalanceController(Common_IServices _Common_IServices, TrialBalance_ISERVICE _TrialBalance_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._TrialBalance_ISERVICE = _TrialBalance_ISERVICE;
        }
        // GET: ApplicationLayer/TrialBalance
        public ActionResult TrialBalance()
        {
            _TrialBalanceModel = new TrialBalanceModel();
            
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
            }
            var today = DateTime.Today;         
            _TrialBalanceModel.Branch = BrID;
            _TrialBalanceModel.UpToDate = today.ToString("yyyy-MM-dd");
            GetBrachList(_TrialBalanceModel);
            //_TrialBalanceModel.TrialBalanceListDetails = SearchStockDetail_load(_TrialBalanceModel);//Commented by Suraj Maurya on 13-02-2026
            ViewBag.MenuPageName = getDocumentName();
            _TrialBalanceModel.Title = title;
            ViewBag.VBRoleList = GetRoleList();
            ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/TrialBalance/TrialBalanceDetail.cshtml", _TrialBalanceModel);
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
        public ActionResult GetAutoCompleteGLList(TrialBalanceModel queryParameters)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string GroupName = string.Empty;
            Dictionary<string, string> GLList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ddlGroup;
                }
                GLList = _TrialBalance_ISERVICE.GLSetupGroupDAL(GroupName, Comp_ID);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(GLList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteAccGrp(TrialBalanceModel queryParameters)
        {
            string GroupName = string.Empty;
            Dictionary<string, string> suggestions = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ddlGroup;
                }
                suggestions = _TrialBalance_ISERVICE.AccGrpListGroupDAL(GroupName, Comp_ID);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(suggestions.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public void GetBrachList(TrialBalanceModel queryParameters)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string User_id = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_id = Session["UserId"].ToString();
                }
                DataSet BranchList = _TrialBalance_ISERVICE.GetAllBrchList(Comp_ID, User_id);
                List<Branch> _Branchlist = new List<Branch>();

                foreach (DataRow dr in BranchList.Tables[0].Rows)
                {
                    Branch _BranchDetail = new Branch();
                    _BranchDetail.br_id = dr["Comp_Id"].ToString();
                    _BranchDetail.br_name = dr["comp_nm"].ToString();
                    _Branchlist.Add(_BranchDetail);
                }
                queryParameters.BranchList = _Branchlist;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        [HttpPost]
        public ActionResult TrialBalanceSave(TrialBalanceModel _TrialBalanceModel, string command)
        {
            try
            {
                if (_TrialBalanceModel.hdnPDFPrint == "Print")
                {
                    command = "Print";
                }
                if (_TrialBalanceModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                if (_TrialBalanceModel.InsightButton == "InsightButtonCsvPrint")
                {
                    command = "InsightButtonCsvPrint";
                }
                switch (command)
                {
                    case "Print":
                        return GenratePdfFile(_TrialBalanceModel);
                    case "CsvPrint":
                        return ExportTrialBalanceData(_TrialBalanceModel);
                    case "InsightButtonCsvPrint":
                        return ExportTrialBalanceInsightData(_TrialBalanceModel);
                    default:
                        return new EmptyResult();
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public FileResult GenratePdfFile(TrialBalanceModel _TrialBalanceModel)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                string Br_ID = string.Empty;
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
                DataSet dset = new DataSet();

                DataTable Details = new DataTable();
                //DataSet Details = new DataSet();
                Details = ToDataTable(_TrialBalanceModel.PrintPDFData,"", _TrialBalanceModel.PrintBrIdListPDFData, _TrialBalanceModel.PrintTotalPDFData);
                /*start Add _TrialBalanceModel.PrintBrIdListPDFData also by Hina on 22-05-2025 for multiple branch name */
                JArray jObject = JArray.Parse(_TrialBalanceModel.PrintBrIdListPDFData);
                JArray jObjecttotal = JArray.Parse(_TrialBalanceModel.PrintTotalPDFData);
                DataTable DtblTotalDetail = new DataTable();
                DataTable dttotal = new DataTable();
                if (jObjecttotal != null)
                {
                    dttotal.Columns.Add("TotalOpnBal_Dr", typeof(string));
                    dttotal.Columns.Add("TotalOpnBal_Cr", typeof(string));
                    dttotal.Columns.Add("TotalDebit", typeof(string));
                    dttotal.Columns.Add("TotalCredit", typeof(string));
                    dttotal.Columns.Add("TotalClsBal_Dr", typeof(string));
                    dttotal.Columns.Add("TotalClsBal_Cr", typeof(string));

                    //JArray jObjecttotal1 = JArray.Parse(_TrialBalanceModel.PrintTotalPDFData);
                    for (int i = 0; i < jObjecttotal.Count; i++)
                    {
                        DataRow dtrowLines = dttotal.NewRow();
                        
                        dtrowLines["TotalOpnBal_Dr"] = jObjecttotal[i]["TotalOpnBalDebit"].ToString();
                        dtrowLines["TotalOpnBal_Cr"] = jObjecttotal[i]["TotalOpnBalCredit"].ToString();
                        dtrowLines["TotalDebit"] = jObjecttotal[i]["TotalDebit"].ToString();
                        dtrowLines["TotalCredit"] = jObjecttotal[i]["TotalCredit"].ToString();
                        dtrowLines["TotalClsBal_Dr"] = jObjecttotal[i]["TotalClsBalDebit"].ToString();
                        dtrowLines["TotalClsBal_Cr"] = jObjecttotal[i]["TotalClsBalCredit"].ToString();


                        dttotal.Rows.Add(dtrowLines);
                    }
                }
                DtblTotalDetail = dttotal;
                
                string BrId_List = jObject[0]["Br_ID"].ToString();
                /*End Add by Hina on 22-05-2025 for multiple branch name */
                DataView data = new DataView();
                data = Details.DefaultView;
                data.Sort = "AccName";
                Details = data.ToTable();
                DataTable dt = new DataTable();
                dt = data.ToTable(true, "AccName");

                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, Br_ID, BrId_List);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.Details = Details;
                ViewBag.TotalDetails = DtblTotalDetail;
                ViewBag.DocName = "Trial Balance";
                ViewBag.BalType = _TrialBalanceModel.BalanceType;
                ViewBag.AccountGroup = _TrialBalanceModel.AccountGroupName;
                ViewBag.AccountType = _TrialBalanceModel.AccountTypeName;
                string formattedDate = string.Join("-", _TrialBalanceModel.UpToDate.Split('-').Reverse());
                ViewBag.UpToDate = formattedDate;
                ViewBag.Currency = _TrialBalanceModel.Currency;
                ViewBag.DocumentMenuId = DocumentMenuId;
                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 20f);
                    
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    DataTable PrintGL = new DataTable();
                    pdfDoc.Open();
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/TrialBalance/TrialBalancePrint.cshtml"));
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
                    return File(bytes.ToArray(), "application/pdf", "TrialBalance.pdf");
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
        //private DataTable ToDataTable(string Details,string InsightCSV,string BrIdListDetails, string totaldetail)
        private DataTable ToDataTable(string Details, string InsightCSV, string BrIdListDetails, string totaldetail)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                if (InsightCSV== "InsightButtonData")
                {
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
                else
                {
                    dtItem.Columns.Add("AccName", typeof(string));
                    dtItem.Columns.Add("Acc_Id", typeof(string));
                    dtItem.Columns.Add("Acc_typ", typeof(string));
                    dtItem.Columns.Add("Acc_grp", typeof(string));
                    dtItem.Columns.Add("Branch", typeof(string));
                    dtItem.Columns.Add("curr_name", typeof(string));
                    //dtItem.Columns.Add("op_bal_bs", typeof(string));
                    //dtItem.Columns.Add("op_bal_type", typeof(string));
                    //dtItem.Columns.Add("totdebitbs", typeof(string));
                    //dtItem.Columns.Add("totcreditbs", typeof(string));
                    //dtItem.Columns.Add("closingbalbs", typeof(string));
                    //dtItem.Columns.Add("closingbaltype", typeof(string));/*commented and modify by Hina sharma on 19-08-2025*/
                    dtItem.Columns.Add("Op_Bal_Debit", typeof(decimal));
                    dtItem.Columns.Add("Op_Bal_Credit", typeof(decimal));
                    dtItem.Columns.Add("Total_Debit", typeof(decimal));
                    dtItem.Columns.Add("Total_Credit", typeof(decimal));
                    dtItem.Columns.Add("Closing_Bal_Debit", typeof(decimal));
                    dtItem.Columns.Add("Closing_Bal_Credit", typeof(decimal));

                    if (Details != null)
                    {
                        JArray jObject = JArray.Parse(Details);
                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();
                            dtrowLines["AccName"] = jObject[i]["AccName"].ToString();
                            dtrowLines["Acc_Id"] = jObject[i]["Acc_Id"].ToString();
                            dtrowLines["Acc_typ"] = jObject[i]["Acc_typ"].ToString();
                            dtrowLines["Acc_grp"] = jObject[i]["Acc_grp"].ToString();
                            dtrowLines["Branch"] = jObject[i]["Branch"].ToString();
                            dtrowLines["curr_name"] = jObject[i]["curr_name"].ToString();

                            dtrowLines["Op_Bal_Debit"] = jObject[i]["op_bal_bs_dr"].ToString();
                            dtrowLines["Op_Bal_Credit"] = jObject[i]["op_bal_bs_cr"].ToString();
                            dtrowLines["Total_Debit"] = jObject[i]["totdebitbs"].ToString();
                            dtrowLines["Total_Credit"] = jObject[i]["totcreditbs"].ToString();
                            dtrowLines["Closing_Bal_Debit"] = jObject[i]["closingbalbs_dr"].ToString();
                            dtrowLines["Closing_Bal_Credit"] = jObject[i]["closingbalbs_cr"].ToString();
                            //dtrowLines["op_bal_bs"] = jObject[i]["op_bal_bs"].ToString();/*commented and modify above by Hina sharma on 19-08-2025*/
                            //dtrowLines["op_bal_type"] = jObject[i]["op_bal_type"].ToString();
                            //dtrowLines["totdebitbs"] = jObject[i]["totdebitbs"].ToString();
                            //dtrowLines["totcreditbs"] = jObject[i]["totcreditbs"].ToString();
                            //dtrowLines["closingbalbs"] = jObject[i]["closingbalbs"].ToString();
                            //dtrowLines["closingbaltype"] = jObject[i]["closingbaltype"].ToString();

                            dtItem.Rows.Add(dtrowLines);
                        }
                    }
                    DtblItemTaxDetail = dtItem;
                    return DtblItemTaxDetail;
                }
               
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private List<TrialBalanceList> SearchStockDetail_load(TrialBalanceModel _TrialBalanceModel)
        {
            try
            {
                List<TrialBalanceList> _TrialBalanceListDetail = new List<TrialBalanceList>();
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                DataSet dset = new DataSet();

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
                dset = _TrialBalance_ISERVICE.GetBalanceDetailList(CompID, Br_ID, UserID, "Y", "BW", "0", "0", "0", "SU", Br_ID, _TrialBalanceModel.UpToDate,"BS");

                _TrialBalanceModel.BalanceBy = "BranchwiseBalance";
                if (dset.Tables.Count > 0)
                {
                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dset.Tables[0].Rows)
                        {
                            TrialBalanceList _TrialBalanceList = new TrialBalanceList();
                            _TrialBalanceList.AccountName = dr["acc_name"].ToString();
                            _TrialBalanceList.AccountId = dr["acc_id"].ToString();
                            _TrialBalanceList.AccountType = dr["acc_typename"].ToString();
                            _TrialBalanceList.AccountGroup = dr["acc_group"].ToString();
                            _TrialBalanceList.Branch = dr["branch"].ToString();
                            _TrialBalanceList.BranchId = dr["br_id"].ToString();
                            _TrialBalanceList.curr_name = dr["curr_name"].ToString();
                            _TrialBalanceList.curr_id = dr["curr_id"].ToString();
                            _TrialBalanceList.Ho_op_dr_bs = dr["ho_dr_op_bs"].ToString();
                            _TrialBalanceList.Ho_op_cr_bs = dr["ho_cr_op_bs"].ToString();
                            _TrialBalanceList.Ho_op_dr_sp = dr["ho_dr_op_sp"].ToString();
                            _TrialBalanceList.Ho_op_cr_sp = dr["ho_cr_op_sp"].ToString();
                            _TrialBalanceList.br_op_dr_bs = dr["br_dr_op_bs"].ToString();
                            _TrialBalanceList.br_op_cr_bs = dr["br_cr_op_bs"].ToString();
                            _TrialBalanceList.br_op_dr_sp = dr["br_dr_op_sp"].ToString();
                            _TrialBalanceList.br_op_cr_sp = dr["br_cr_op_sp"].ToString();
                            _TrialBalanceList.HoTotalDebit_bs = dr["ho_totaldebitbs"].ToString();
                            _TrialBalanceList.HoTotalDebit_sp = dr["ho_totaldebitsp"].ToString();
                            _TrialBalanceList.HoTotalCredit_bs = dr["ho_totalcreditbs"].ToString();
                            _TrialBalanceList.HoTotalCredit_sp = dr["ho_totalcreditsp"].ToString();
                            _TrialBalanceList.BrTotalDebit_bs = dr["br_totaldebitbs"].ToString();
                            _TrialBalanceList.BrTotalDebit_sp = dr["br_totaldebitsp"].ToString();
                            _TrialBalanceList.BrTotalCredit_bs = dr["br_totalcreditbs"].ToString();
                            _TrialBalanceList.BrTotalCredit_sp = dr["br_totalcreditsp"].ToString();

                            _TrialBalanceList.Ho_cl_dr_bs = dr["ho_dr_cl_bs"].ToString();
                            _TrialBalanceList.Ho_cl_cr_bs = dr["ho_cr_cl_bs"].ToString();
                            _TrialBalanceList.Ho_cl_dr_sp = dr["ho_dr_cl_sp"].ToString();
                            _TrialBalanceList.Ho_cl_cr_sp = dr["ho_cr_cl_sp"].ToString();
                            _TrialBalanceList.br_cl_dr_bs = dr["br_dr_cl_bs"].ToString();
                            _TrialBalanceList.br_cl_cr_bs = dr["br_cr_cl_bs"].ToString();
                            _TrialBalanceList.br_cl_dr_sp = dr["br_dr_cl_sp"].ToString();
                            _TrialBalanceList.br_cl_cr_sp = dr["br_cr_cl_sp"].ToString();
                            _TrialBalanceListDetail.Add(_TrialBalanceList);
                        }

                        ViewBag.L1Data = dset.Tables[1];
                        ViewBag.L2Data = dset.Tables[2];
                        ViewBag.L3Data = dset.Tables[3];
                        ViewBag.L4Data = dset.Tables[4];
                        ViewBag.DetailsData = dset.Tables[5];
                        ViewBag.TotalAmt = dset.Tables[6];
                        ViewBag.TotalAmtTrialBalList = dset.Tables[7];/*add by Hina Sharma on 31-07-2025*/
                    }
                }
                return _TrialBalanceListDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        [HttpPost]
        public JsonResult PrintTrialBalDetail(string IncludeZeroBlaFlag, string BalanceBy, string AccId, string AccGroupId, string Acctype, string RptType, string BrachID, string Uptodt,string filter_curr)
        {
            try
            {
                JsonResult DataRows = null;
                DataSet dset = new DataSet();

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
                    UserID = Session["userid"].ToString();
                }
                dset = _TrialBalance_ISERVICE.GetBalanceDetailList(CompID, BrID, UserID, IncludeZeroBlaFlag, BalanceBy, AccId, AccGroupId, Acctype, RptType, BrachID, Uptodt, filter_curr);
                DataRows = Json(JsonConvert.SerializeObject(dset), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public ActionResult SearchTrialBalDetail(string IncludeZeroBlaFlag, string BalanceBy, string AccId, string AccGroupId, string Acctype, string RptType, string BrachID, string Uptodt, string filter_curr)
        {
            try
            {
                List<TrialBalanceList> _TrialBalanceListDetail = new List<TrialBalanceList>();
                TrialBalanceModel _TrialBalanceModel = new TrialBalanceModel();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
                DataSet dset = new DataSet();

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
                    UserID = Session["userid"].ToString();
                }
                dset = _TrialBalance_ISERVICE.GetBalanceDetailList(CompID, BrID, UserID, IncludeZeroBlaFlag, BalanceBy, AccId, AccGroupId, Acctype, RptType, BrachID, Uptodt, filter_curr);

                    if (BalanceBy == "BW")
                    {
                   _TrialBalanceModel.BalanceBy= "BranchwiseBalance";
                    _TrialBalanceModel.BalanceByFilter = "BranchwiseBalancetbl";
                }
                    if (BalanceBy == "HW")
                    {
                    _TrialBalanceModel.BalanceBy = "HowiseBalance";
                    _TrialBalanceModel.BalanceByFilter = "HowiseBalancetbl";
                }
              
                if (dset.Tables.Count > 0)
                {
                    if (dset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dset.Tables[0].Rows)
                        {
                            TrialBalanceList _TrialBalanceList = new TrialBalanceList();
                            _TrialBalanceList.AccountName = dr["acc_name"].ToString();
                            _TrialBalanceList.AccountId = dr["acc_id"].ToString();
                            _TrialBalanceList.AccountType = dr["acc_typename"].ToString();
                            _TrialBalanceList.AccountGroup = dr["acc_group"].ToString();
                            _TrialBalanceList.Branch = dr["branch"].ToString();
                            _TrialBalanceList.BranchId = dr["br_id"].ToString();
                            _TrialBalanceList.curr_name = dr["curr_name"].ToString();
                            _TrialBalanceList.curr_id = dr["curr_id"].ToString();
                            _TrialBalanceList.Ho_op_dr_bs = dr["ho_dr_op_bs"].ToString();
                            _TrialBalanceList.Ho_op_cr_bs = dr["ho_cr_op_bs"].ToString();
                            _TrialBalanceList.Ho_op_dr_sp = dr["ho_dr_op_sp"].ToString();
                            _TrialBalanceList.Ho_op_cr_sp = dr["ho_cr_op_sp"].ToString();
                            _TrialBalanceList.br_op_dr_bs = dr["br_dr_op_bs"].ToString();
                            _TrialBalanceList.br_op_cr_bs = dr["br_cr_op_bs"].ToString();
                            _TrialBalanceList.br_op_dr_sp = dr["br_dr_op_sp"].ToString();
                            _TrialBalanceList.br_op_cr_sp = dr["br_cr_op_sp"].ToString();
                            _TrialBalanceList.HoTotalDebit_bs = dr["ho_totaldebitbs"].ToString();
                            _TrialBalanceList.HoTotalDebit_sp = dr["ho_totaldebitsp"].ToString();
                            _TrialBalanceList.HoTotalCredit_bs = dr["ho_totalcreditbs"].ToString();
                            _TrialBalanceList.HoTotalCredit_sp = dr["ho_totalcreditsp"].ToString();
                            _TrialBalanceList.BrTotalDebit_bs = dr["br_totaldebitbs"].ToString();
                            _TrialBalanceList.BrTotalDebit_sp = dr["br_totaldebitsp"].ToString();
                            _TrialBalanceList.BrTotalCredit_bs = dr["br_totalcreditbs"].ToString();
                            _TrialBalanceList.BrTotalCredit_sp = dr["br_totalcreditsp"].ToString();
                            _TrialBalanceList.Ho_cl_dr_bs = dr["ho_dr_cl_bs"].ToString();
                            _TrialBalanceList.Ho_cl_cr_bs = dr["ho_cr_cl_bs"].ToString();
                            _TrialBalanceList.Ho_cl_dr_sp = dr["ho_dr_cl_sp"].ToString();
                            _TrialBalanceList.Ho_cl_cr_sp = dr["ho_cr_cl_sp"].ToString();
                            _TrialBalanceList.br_cl_dr_bs = dr["br_dr_cl_bs"].ToString();
                            _TrialBalanceList.br_cl_cr_bs = dr["br_cr_cl_bs"].ToString();
                            _TrialBalanceList.br_cl_dr_sp = dr["br_dr_cl_sp"].ToString();
                            _TrialBalanceList.br_cl_cr_sp = dr["br_cr_cl_sp"].ToString();

                            _TrialBalanceListDetail.Add(_TrialBalanceList);
                        }
                       
                            ViewBag.L1Data = dset.Tables[1];
                            ViewBag.L2Data = dset.Tables[2];
                            ViewBag.L3Data = dset.Tables[3];
                            ViewBag.L4Data = dset.Tables[4];
                            ViewBag.DetailsData = dset.Tables[5];
                            ViewBag.TotalAmt = dset.Tables[6];
                            ViewBag.TotalAmtTrialBalList = dset.Tables[7];/*add by Hina Sharma on 31-07-2025*/

                    }
                }
                _TrialBalanceModel.TrialBalanceListDetails = _TrialBalanceListDetail;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialTrialBalanceList.cshtml", _TrialBalanceModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public ActionResult GetTrialBalHistoryDetail(string AccId, string BrachID, string Uptodt, string Flag, string Accname, string bs_amt, string sp_amt, string type, string baltype, string currtype)
        {
            try
            {
                DataSet ds = new DataSet();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                ds = _TrialBalance_ISERVICE.GetTrialBalHisList(CompID, BrachID, AccId, Flag, Uptodt, baltype, currtype);
                ViewBag.tbtype = Flag;

                ViewBag.AccName = Accname;
                ViewBag.DCCbal_bs = bs_amt;
                ViewBag.DCCbal_sp = sp_amt;
                ViewBag.DCCbal_type = type;
                ViewBag.TrialBalTransHistory = ds.Tables[0] ;
                ViewBag.TotalTrialBalTransHistory = ds.Tables[1];
                return PartialView("~/Areas/Common/Views/PartialTrialBalanceClosingBalanceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private DataTable GetRoleList()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public FileResult ExportTrialBalanceData(TrialBalanceModel _TrialBalanceModel)
        {
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                
                List<TrialBalanceList> _TrialBalanceListDetail = new List<TrialBalanceList>();

                _TrialBalanceListDetail = getTrialBalanceList(_TrialBalanceModel.Allfilters, _TrialBalanceModel.PrintBrIdListPDFData);
                var ItemListData = (from tempitem in _TrialBalanceListDetail select tempitem);

                string searchValue = _TrialBalanceModel.searchValue;
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    ItemListData = ItemListData.Where(m => m.AccountName.ToUpper().Contains(searchValue) || m.AccountId.ToUpper().Contains(searchValue)
                    || m.AccountType.ToUpper().Contains(searchValue) || m.AccountGroup.ToUpper().Contains(searchValue) || m.Branch.ToUpper().Contains(searchValue)
                    || m.BranchId.ToUpper().Contains(searchValue) || m.Ho_op_dr_bs.ToUpper().Contains(searchValue) || m.Ho_op_cr_bs.ToUpper().Contains(searchValue)
                    || m.Ho_op_dr_sp.ToUpper().Contains(searchValue) || m.Ho_op_cr_sp.ToUpper().Contains(searchValue) || m.br_op_dr_bs.ToUpper().Contains(searchValue)
                    || m.br_op_cr_bs.ToUpper().Contains(searchValue) || m.br_op_dr_sp.ToUpper().Contains(searchValue) || m.br_op_cr_sp.ToUpper().Contains(searchValue)
                    || m.HoTotalDebit_bs.ToUpper().Contains(searchValue) || m.HoTotalDebit_sp.ToUpper().Contains(searchValue)|| m.HoTotalCredit_bs.ToUpper().Contains(searchValue)
                    || m.HoTotalCredit_sp.ToUpper().Contains(searchValue) || m.BrTotalDebit_bs.ToUpper().Contains(searchValue) || m.BrTotalDebit_sp.ToUpper().Contains(searchValue)
                    || m.BrTotalCredit_bs.ToUpper().Contains(searchValue) || m.BrTotalCredit_sp.ToUpper().Contains(searchValue) || m.Ho_cl_dr_bs.ToUpper().Contains(searchValue)
                    || m.Ho_cl_cr_bs.ToUpper().Contains(searchValue) || m.Ho_cl_dr_sp.ToUpper().Contains(searchValue) || m.Ho_cl_cr_sp.ToUpper().Contains(searchValue)
                    || m.br_cl_dr_bs.ToUpper().Contains(searchValue) || m.br_cl_cr_bs.ToUpper().Contains(searchValue) || m.br_cl_dr_sp.ToUpper().Contains(searchValue)
                    || m.br_cl_cr_sp.ToUpper().Contains(searchValue));
                }
                var data = ItemListData.ToList();

                string Fstring = string.Empty;
                string[] fdata;
                Fstring = _TrialBalanceModel.Allfilters;
                fdata = Fstring.Split(',');
                DataTable dt = new DataTable();

                dt = ToBrWiseStockDetailExl(data, fdata[1]);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Trial Balance", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        private List<TrialBalanceList> getTrialBalanceList(string filters,string BRList)
        {
            List<TrialBalanceList> _TrialBalanceListDetail = new List<TrialBalanceList>();
            try
            {
                string IncludeZeroStock, BalanceBy, AccId, AccGroupId, Acctype, Rpttype, Br_ID, Uptodate,curr_filter;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                DataSet dset = new DataSet();
                if (filters != null)
                {
                    if (filters != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = filters;
                        fdata = Fstring.Split(',');

                        IncludeZeroStock = fdata[0];
                        BalanceBy = fdata[1];
                        AccId = fdata[2];
                        AccGroupId = fdata[3];
                        Acctype = fdata[4];
                        Rpttype = fdata[5];
                        //Br_ID = fdata[6];
                        //Uptodate = fdata[7];
                        //curr_filter = fdata[8];
                        Br_ID = BRList;
                        Uptodate = fdata[6];
                        curr_filter = fdata[7];

                        dset = _TrialBalance_ISERVICE.GetBalanceDetailList(CompID, BrID, UserID, IncludeZeroStock, BalanceBy, AccId, AccGroupId, Acctype, Rpttype, Br_ID, Uptodate, curr_filter);
                    }
                }
                if (dset.Tables[0].Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in dset.Tables[0].Rows)
                    {
                        TrialBalanceList _TrialBalanceList = new TrialBalanceList();
                        _TrialBalanceList.SrNo = rowno + 1;
                        _TrialBalanceList.AccountName = dr["acc_name"].ToString();
                        _TrialBalanceList.AccountId = dr["acc_id"].ToString();
                        _TrialBalanceList.AccountType = dr["acc_typename"].ToString();
                        _TrialBalanceList.AccountGroup = dr["acc_group"].ToString();
                        _TrialBalanceList.Branch = dr["branch"].ToString();
                        _TrialBalanceList.BranchId = dr["br_id"].ToString();
                        _TrialBalanceList.curr_name = dr["curr_name"].ToString();
                        _TrialBalanceList.curr_id = dr["curr_id"].ToString();
                        _TrialBalanceList.Ho_op_dr_bs = dr["ho_dr_op_bs"].ToString();
                        _TrialBalanceList.Ho_op_cr_bs = dr["ho_cr_op_bs"].ToString();
                        _TrialBalanceList.Ho_op_dr_sp = dr["ho_dr_op_sp"].ToString();
                        _TrialBalanceList.Ho_op_cr_sp = dr["ho_cr_op_sp"].ToString();
                        _TrialBalanceList.br_op_dr_bs = dr["br_dr_op_bs"].ToString();
                        _TrialBalanceList.br_op_cr_bs = dr["br_cr_op_bs"].ToString();
                        _TrialBalanceList.br_op_dr_sp = dr["br_dr_op_sp"].ToString();
                        _TrialBalanceList.br_op_cr_sp = dr["br_cr_op_sp"].ToString();
                        _TrialBalanceList.HoTotalDebit_bs = dr["ho_totaldebitbs"].ToString();
                        _TrialBalanceList.HoTotalDebit_sp = dr["ho_totaldebitsp"].ToString();
                        _TrialBalanceList.HoTotalCredit_bs = dr["ho_totalcreditbs"].ToString();
                        _TrialBalanceList.HoTotalCredit_sp = dr["ho_totalcreditsp"].ToString();
                        _TrialBalanceList.BrTotalDebit_bs = dr["br_totaldebitbs"].ToString();
                        _TrialBalanceList.BrTotalDebit_sp = dr["br_totaldebitsp"].ToString();
                        _TrialBalanceList.BrTotalCredit_bs = dr["br_totalcreditbs"].ToString();
                        _TrialBalanceList.BrTotalCredit_sp = dr["br_totalcreditsp"].ToString();
                        _TrialBalanceList.Ho_cl_dr_bs = dr["ho_dr_cl_bs"].ToString();
                        _TrialBalanceList.Ho_cl_cr_bs = dr["ho_cr_cl_bs"].ToString();
                        _TrialBalanceList.Ho_cl_dr_sp = dr["ho_dr_cl_sp"].ToString();
                        _TrialBalanceList.Ho_cl_cr_sp = dr["ho_cr_cl_sp"].ToString();
                        _TrialBalanceList.br_cl_dr_bs = dr["br_dr_cl_bs"].ToString();
                        _TrialBalanceList.br_cl_cr_bs = dr["br_cr_cl_bs"].ToString();
                        _TrialBalanceList.br_cl_dr_sp = dr["br_dr_cl_sp"].ToString();
                        _TrialBalanceList.br_cl_cr_sp = dr["br_cr_cl_sp"].ToString();
                        _TrialBalanceListDetail.Add(_TrialBalanceList);
                        rowno = rowno + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            return _TrialBalanceListDetail;
        }
        public DataTable ToBrWiseStockDetailExl(List<TrialBalanceList> _ItemListModel, string StockBy)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr No.", typeof(int));
            dataTable.Columns.Add("Account Name", typeof(string));
            dataTable.Columns.Add("Account Type", typeof(string));
            dataTable.Columns.Add("Account Group", typeof(string));
            if (StockBy == "BW")
            {
                dataTable.Columns.Add("Branch", typeof(string));
            }
            dataTable.Columns.Add("curr_name", typeof(string));
            dataTable.Columns.Add("Opening Balance Debit", typeof(decimal));
            dataTable.Columns.Add("Opening Balance Credit", typeof(decimal));
            dataTable.Columns.Add("Total Debit", typeof(decimal));
            dataTable.Columns.Add("Total Credit", typeof(decimal));
            dataTable.Columns.Add("Closing Balance Debit", typeof(decimal));
            dataTable.Columns.Add("Closing Balance  Credit", typeof(decimal));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr No."] = item.SrNo;
                rows["Account Name"] = item.AccountName;
                rows["Account Type"] = item.AccountType;
                rows["Account Group"] = item.AccountGroup;
                if (StockBy == "BW")
                {
                    rows["Branch"] = item.Branch;
                    rows["curr_name"] = item.curr_name;
                    rows["Opening Balance Debit"] = item.br_op_dr_bs;
                    rows["Opening Balance Credit"] = item.br_op_cr_bs;
                    rows["Total Debit"] = item.BrTotalDebit_bs;
                    rows["Total Credit"] = item.BrTotalCredit_bs;
                    rows["Closing Balance Debit"] = item.br_cl_dr_bs;
                    rows["Closing Balance  Credit"] = item.br_cl_cr_bs;
                }
                else
                {
                    rows["curr_name"] = item.curr_name;
                    rows["Opening Balance Debit"] = item.Ho_op_dr_bs;
                    rows["Opening Balance Credit"] = item.Ho_op_cr_bs;
                    rows["Total Debit"] = item.HoTotalDebit_bs;
                    rows["Total Credit"] = item.HoTotalCredit_bs;
                    rows["Closing Balance Debit"] = item.Ho_cl_dr_bs;
                    rows["Closing Balance  Credit"] = item.Ho_cl_cr_bs;
                }

                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        public FileResult ExportTrialBalanceInsightData(TrialBalanceModel _TrialBalanceModel)
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

                Details = ToDataTable(_TrialBalanceModel.InsightButtonData, "InsightButtonData","","");
                DataTable dt = new DataTable();
                List<TrialBalanceList> _BB_Detail_List = new List<TrialBalanceList>();
                dt = Details;
                if (dt.Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        TrialBalanceList bb_list = new TrialBalanceList();
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
                string searchValue = _TrialBalanceModel.searchValue;
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    ItemListData = ItemListData.Where(m => m.Vou_No.ToUpper().Contains(searchValue) || m.Vou_dt.ToUpper().Contains(searchValue)
                    || m.cc_vou_amt_bs.ToUpper().Contains(searchValue) || m.cc_vou_amt_sP.ToUpper().Contains(searchValue) || m.amt_type.ToUpper().Contains(searchValue)
                    || m.curr_logo.ToUpper().Contains(searchValue) || m.conv_rate.ToUpper().Contains(searchValue) || m.nurr.ToUpper().Contains(searchValue));
                }
                var data = ItemListData.ToList();
                _TrialBalanceModel.hdnCSVPrint = null;
                DataTable dt1 = new DataTable();
                dt1 = ToTrialBalanceInsightDetailExl(data);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("TrialBalance", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToTrialBalanceInsightDetailExl(List<TrialBalanceList> _ItemListModel)
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
        /*-----------------------Print Section Begin--------------------*/
        public FileResult GenratePdfFilebySummary(string IncludeZeroStock, string curr_filt, string BalanceBy, string AccId, string AccGroupId, string Acctype, string Rpttype, string Uptodate, string Br_ID )
        {
            //string curr, string r1, string r2, string r3, string r4, string r5, string r6,
            //string rv1, string rv2, string rv3, string rv4, string rv5, string rv6, string advPmt, string totalAmt
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            DataSet dset = new DataSet();
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
                    UserID = Session["userid"].ToString();
                }
                ViewBag.doc_id = DocumentMenuId;

                dset = _TrialBalance_ISERVICE.GetBalanceDetailList(CompID, BrID, UserID, IncludeZeroStock, BalanceBy, AccId, AccGroupId, Acctype, Rpttype, Br_ID, Uptodate, curr_filt);

                if (dset.Tables.Count > 0)
                {
                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.L1Data = dset.Tables[1];
                        ViewBag.L2Data = dset.Tables[2];
                        ViewBag.L3Data = dset.Tables[3];
                        ViewBag.L4Data = dset.Tables[4];
                        ViewBag.DetailsData = dset.Tables[5];
                        ViewBag.TotalAmt = dset.Tables[6];
                        ViewBag.TotalAmtTrialBalList = dset.Tables[7];/*add by Hina Sharma on 31-07-2025*/
                    }
                }
                
                DataTable dt = new DataTable();
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, BrID, Br_ID);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.DocName = "Trial Balance";
                ViewBag.DocumentMenuId = DocumentMenuId;
                //if (dtdata.Tables[13].Rows.Count > 0)
                //{
                //    ViewBag.Branches = dtdata.Tables[13].Rows[0]["comp_nm"].ToString();
                //}






                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 30f, 80f);
                    
                    writer = PdfWriter.GetInstance(pdfDoc, stream);

                    pdfDoc.Open();

                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/TrialBalance/TrialBalancePrintSummary.cshtml"));



                    //htmlcontent = ConvertPartialViewToString1(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/BalanceSheet/BalanceSheetHorizontalPrint.cshtml"));
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

                    return File(bytes.ToArray(), "application/pdf", "TrialBalance.pdf");
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