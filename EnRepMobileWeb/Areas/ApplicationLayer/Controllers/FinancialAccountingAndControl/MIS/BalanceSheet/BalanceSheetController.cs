using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BalanceSheet;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.BalanceSheet
{
    public class BalanceSheetController : Controller
    {
        string Comp_id,Br_id, User_id, language, title = String.Empty;
        string DocumentMenuId = "105104135125";
        DataTable br_list = new DataTable();
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        BalanceSheet_IService _BalanceSheet_IService;

        public BalanceSheetController(Common_IServices _Common_IServices, BalanceSheet_IService _BalanceSheet_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._BalanceSheet_IService = _BalanceSheet_IService;
        }
        // GET: ApplicationLayer/BalanceSheet
        public ActionResult BalanceSheet()
        {
            DataSet fy_list = new DataSet();
          
            if (Session["CompId"] != null)
            {
                Comp_id = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_id = Session["BranchId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                User_id = Session["UserId"].ToString();
            }
            br_list = _Common_IServices.Cmn_GetBrList(Comp_id, Br_id, User_id,"S");
            fy_list= _Common_IServices.Get_FYList(Comp_id, Br_id);

            if (fy_list.Tables[0].Rows.Count > 0 && fy_list.Tables[1].Rows.Count > 0)
            {
                ViewBag.FromDate = fy_list.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = fy_list.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = fy_list.Tables[0].Rows[0]["fy_enddt"].ToString();
                ViewBag.ToFyMindate = fy_list.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.fylist = fy_list.Tables[1];
            }
            if (Session["BranchId"] != null)
            {
                ViewBag.vb_br_id = Session["BranchId"].ToString();
            }
            DateTime to_dt = DateTime.Now;
            ViewBag.ToDate = to_dt.ToString("yyyy-MM-dd");
            ViewBag.br_list = br_list;
            ViewBag.MenuPageName = getDocumentName();
            ViewBag.Title = title;
            ViewBag.doc_id = DocumentMenuId;
            ViewBag.rpt_type = null;

            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/BalanceSheet/BalanceSheetDetail.cshtml");
        }
        private string getDocumentName()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_id = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(Comp_id, DocumentMenuId, language);
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
        public ActionResult SearchBalanceSheetDetail(string from_dt, string to_dt, string br_list, string rpt_type)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_id = Session["BranchId"].ToString();
                }
                ViewBag.doc_id = DocumentMenuId;

                DataSet dtdata = _BalanceSheet_IService.Get_BalSheet_Detail(Comp_id, Br_id, br_list, from_dt, to_dt, rpt_type);

                ViewBag.capi_assetdata = null;
                ViewBag.curr_assetdata = null;
                ViewBag.ncurr_assetdata = null;
                ViewBag.capi_libadata = null;
                ViewBag.curr_libadata = null;
                ViewBag.ncurr_libadata = null;
                ViewBag.PL_amt = null;

                ViewBag.rpt_type = null;
                ViewBag.ass_capflag = null;
                ViewBag.ass_currflag = null;
                ViewBag.ass_ncurrflag = null;
                ViewBag.lib_capflag = null;
                ViewBag.lib_currflag = null;
                ViewBag.lib_ncurrflag = null;
                ViewBag.curr_fdt_todt = null;
                ViewBag.prev_fdt_todt = null;

                //if (rpt_type == "V")
                //{
                    DateTime fdt = new DateTime();
                    DateTime todt = new DateTime();

                    fdt = Convert.ToDateTime(from_dt);
                    todt = Convert.ToDateTime(to_dt);

                    string cftodt = string.Empty;
                    string pftodt = string.Empty;
                    cftodt = fdt.ToString("dd-MM-yyyy") + " to " + todt.ToString("dd-MM-yyyy");
                    pftodt = fdt.AddYears(-1).ToString("dd-MM-yyyy") + " to " + todt.AddYears(-1).ToString("dd-MM-yyyy");

                    ViewBag.curr_fdt_todt = cftodt;
                    ViewBag.prev_fdt_todt = pftodt;

                //}

                if (dtdata.Tables[0].Rows.Count > 0)
                {
                    ViewBag.capi_assetdata = dtdata.Tables[0];
                    ViewBag.ass_capflag = "ca";
                }
                if (dtdata.Tables[1].Rows.Count > 0)
                {
                    ViewBag.curr_assetdata = dtdata.Tables[1];
                    ViewBag.ass_currflag = "cu";
                }
                if (dtdata.Tables[2].Rows.Count > 0)
                {
                    ViewBag.ncurr_assetdata = dtdata.Tables[2];
                    ViewBag.ass_ncurrflag = "nc";
                }
                if (dtdata.Tables[3].Rows.Count > 0)
                {
                    ViewBag.capi_libadata = dtdata.Tables[3];
                    ViewBag.lib_capflag = "ca";
                }
                if (dtdata.Tables[4].Rows.Count > 0)
                {
                    ViewBag.curr_libadata = dtdata.Tables[4];
                    ViewBag.lib_currflag = "cu";
                }
                if (dtdata.Tables[5].Rows.Count > 0)
                {
                    ViewBag.ncurr_libadata = dtdata.Tables[5];
                    ViewBag.lib_ncurrflag = "nc";
                }
                if (dtdata.Tables[12].Rows.Count > 0)
                {
                    ViewBag.PL_amt = dtdata.Tables[12];
                }
                ViewBag.rpt_type = rpt_type;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialBalanceSheetDetailList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        /*-----------------------Print Section Begin--------------------*/
        public FileResult GenrateBLSheetPdfFile1(string RptType, string from_dt, string to_dt, string br_list)
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
                    Comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_id = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    User_id = Session["userid"].ToString();
                }
                ViewBag.doc_id = DocumentMenuId;
                DataSet dtdata = _BalanceSheet_IService.Get_BalSheet_Detail(Comp_id, Br_id, br_list, from_dt, to_dt, RptType);

                DataTable dt = new DataTable();
                DataTable dtlogo = _Common_IServices.GetCompLogo(Comp_id, Br_id);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.DocName = "Balance Sheet Statement";
                if (dtdata.Tables[13].Rows.Count > 0)
                {
                    ViewBag.Branches = dtdata.Tables[13].Rows[0]["comp_nm"].ToString();
                }
                ViewBag.FromDate = Convert.ToDateTime(from_dt).ToString("dd-MM-yyyy"); 
                ViewBag.ToDate = Convert.ToDateTime(to_dt).ToString("dd-MM-yyyy"); 

                ViewBag.capi_assetdata = null;
                ViewBag.curr_assetdata = null;
                ViewBag.ncurr_assetdata = null;
                ViewBag.capi_libadata = null;
                ViewBag.curr_libadata = null;
                ViewBag.ncurr_libadata = null;
                ViewBag.PL_amt = null;

                ViewBag.rpt_type = null;
                ViewBag.ass_capflag = null;
                ViewBag.ass_currflag = null;
                ViewBag.ass_ncurrflag = null;
                ViewBag.lib_capflag = null;
                ViewBag.lib_currflag = null;
                ViewBag.lib_ncurrflag = null;
                ViewBag.curr_fdt_todt = null;
                ViewBag.prev_fdt_todt = null;

                //if (rpt_type == "V")
                //{
                DateTime fdt = new DateTime();
                DateTime todt = new DateTime();

                fdt = Convert.ToDateTime(from_dt);
                todt = Convert.ToDateTime(to_dt);

                string cftodt = string.Empty;
                string pftodt = string.Empty;
                cftodt = fdt.ToString("dd-MM-yyyy") + " to " + todt.ToString("dd-MM-yyyy");
                pftodt = fdt.AddYears(-1).ToString("dd-MM-yyyy") + " to " + todt.AddYears(-1).ToString("dd-MM-yyyy");

                ViewBag.curr_fdt_todt = cftodt;
                ViewBag.prev_fdt_todt = pftodt;

                //}

                if (dtdata.Tables[0].Rows.Count > 0)
                {
                    ViewBag.capi_assetdata = dtdata.Tables[0];
                    ViewBag.ass_capflag = "ca";
                }
                if (dtdata.Tables[1].Rows.Count > 0)
                {
                    ViewBag.curr_assetdata = dtdata.Tables[1];
                    ViewBag.ass_currflag = "cu";
                }
                if (dtdata.Tables[2].Rows.Count > 0)
                {
                    ViewBag.ncurr_assetdata = dtdata.Tables[2];
                    ViewBag.ass_ncurrflag = "nc";
                }
                if (dtdata.Tables[3].Rows.Count > 0)
                {
                    ViewBag.capi_libadata = dtdata.Tables[3];
                    ViewBag.lib_capflag = "ca";
                }
                if (dtdata.Tables[4].Rows.Count > 0)
                {
                    ViewBag.curr_libadata = dtdata.Tables[4];
                    ViewBag.lib_currflag = "cu";
                }
                if (dtdata.Tables[5].Rows.Count > 0)
                {
                    ViewBag.ncurr_libadata = dtdata.Tables[5];
                    ViewBag.lib_ncurrflag = "nc";
                }
                if (dtdata.Tables[12].Rows.Count > 0)
                {
                    ViewBag.PL_amt = dtdata.Tables[12];
                }
                ViewBag.rpt_type = RptType;


                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    if (RptType == "H")
                    {
                        pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 30f, 80f);
                    }
                    else
                        {
                        pdfDoc = new Document(PageSize.A4, 10f, 10f, 30f, 80f);
                    }
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    
                    pdfDoc.Open();
                    if(RptType=="H")
                    {
                        htmlcontent = ConvertPartialViewToString1(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/BalanceSheet/BalanceSheetHorizontalPrint.cshtml"));
                    }
                    else
                    {
                        htmlcontent = ConvertPartialViewToString1(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/BalanceSheet/BalanceSheetVerticalPrint.cshtml"));

                    }

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

                    return File(bytes.ToArray(), "application/pdf", "BalanceSheet.pdf");
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