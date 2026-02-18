using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.ProfitAndLossStatement;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.ProfitAndLossStatement
{
    public class ProfitAndLossStatementController : Controller
    {
        string CompID, BrID, language, title = String.Empty;
        string DocumentMenuId = "105104135130";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        Profit_LossState_IService _profit_LoseState_IService;
        public ProfitAndLossStatementController(Common_IServices _Common_IServices, Profit_LossState_IService _profit_LoseState_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._profit_LoseState_IService = _profit_LoseState_IService;
        }
        // GET: ApplicationLayer/ProfitAndLossStatement
        public ActionResult ProfitAndLossStatement()
        {
            DataSet dttbl = new DataSet();
            DataTable br_list = new DataTable();
            //br_list = GetBrachList();
            string Comp_ID = string.Empty;
            string User_id = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                User_id = Session["UserId"].ToString();
            }
            br_list = _Common_IServices.Cmn_GetBrList(Comp_ID, User_id);

            dttbl = _Common_IServices.Get_FYList(Comp_ID, BrID);

            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                ViewBag.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();
                ViewBag.ToFyMindate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.fylist = dttbl.Tables[1];
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
            ViewBag.rpt_type = null;
            ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/ProfitAndLossStatement/ProfitAndLossStatement.cshtml");
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
        public ActionResult SearchProfitAndLossDetail(string from_dt,string to_dt,string br_list,string rpt_type)
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

               

                DataSet dtdata = _profit_LoseState_IService.Get_PL_Statement(CompID, BrID, br_list, from_dt, to_dt, rpt_type);

                ViewBag.opening_tmpdata = null;
                ViewBag.direxp_tmpdata = null;
                ViewBag.indiexp_tmpdata = null;
                ViewBag.dirinc_tmpdata = null;
                ViewBag.closing_tmpdata = null;
                ViewBag.indirinc_tmpdata = null;
                ViewBag.totamt_tmpdata = null;
                ViewBag.rpt_type = null;
                ViewBag.curr_fdt_todt = null;
                ViewBag.prev_fdt_todt = null;

                if (rpt_type=="V")
                {
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

                }

                if (dtdata.Tables[0].Rows.Count > 0)
                {
                    ViewBag.opening_tmpdata = dtdata.Tables[0];
                }
                if (dtdata.Tables[2].Rows.Count > 0)
                {
                    ViewBag.direxp_tmpdata = dtdata.Tables[2];
                }
                if (dtdata.Tables[3].Rows.Count > 0)
                {
                    ViewBag.indiexp_tmpdata = dtdata.Tables[3];
                }
                if (dtdata.Tables[4].Rows.Count > 0)
                {
                    ViewBag.dirinc_tmpdata = dtdata.Tables[4];
                }
                if (dtdata.Tables[1].Rows.Count > 0)
                {
                    ViewBag.closing_tmpdata = dtdata.Tables[1];
                }
                if (dtdata.Tables[5].Rows.Count > 0)
                {
                    ViewBag.indirinc_tmpdata = dtdata.Tables[5];
                }
                if (dtdata.Tables[6].Rows.Count > 0)
                {
                    ViewBag.totamt_tmpdata = dtdata.Tables[6];
                }
                ViewBag.rpt_type = rpt_type;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProfitAndLossStatementList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        /*-----------------------Print Section Begin--------------------*/
        public FileResult GenratePAndLPdfFile1(string RptType, string from_dt, string to_dt, string br_list)
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
                    BrID = Session["BranchId"].ToString();
                }

                ViewBag.doc_id = DocumentMenuId;

                DataSet dtdata = _profit_LoseState_IService.Get_PL_Statement(CompID, BrID, br_list, from_dt, to_dt, RptType);

                DataTable dt = new DataTable();
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, BrID);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.DocName = "Profit And Loss Statement";
                if (dtdata.Tables[7].Rows.Count > 0)
                {
                    ViewBag.Branches = dtdata.Tables[7].Rows[0]["comp_nm"].ToString();
                }
                ViewBag.FromDate = Convert.ToDateTime(from_dt).ToString("dd-MM-yyyy");
                ViewBag.ToDate = Convert.ToDateTime(to_dt).ToString("dd-MM-yyyy");

                ViewBag.opening_tmpdata = null;
                ViewBag.direxp_tmpdata = null;
                ViewBag.indiexp_tmpdata = null;
                ViewBag.dirinc_tmpdata = null;
                ViewBag.closing_tmpdata = null;
                ViewBag.indirinc_tmpdata = null;
                ViewBag.totamt_tmpdata = null;
                ViewBag.rpt_type = null;
                ViewBag.curr_fdt_todt = null;
                ViewBag.prev_fdt_todt = null;

                if (RptType == "V")
                {
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

                }

                if (dtdata.Tables[0].Rows.Count > 0)
                {
                    ViewBag.opening_tmpdata = dtdata.Tables[0];
                }
                if (dtdata.Tables[2].Rows.Count > 0)
                {
                    ViewBag.direxp_tmpdata = dtdata.Tables[2];
                }
                if (dtdata.Tables[3].Rows.Count > 0)
                {
                    ViewBag.indiexp_tmpdata = dtdata.Tables[3];
                }
                if (dtdata.Tables[4].Rows.Count > 0)
                {
                    ViewBag.dirinc_tmpdata = dtdata.Tables[4];
                }
                if (dtdata.Tables[1].Rows.Count > 0)
                {
                    ViewBag.closing_tmpdata = dtdata.Tables[1];
                }
                if (dtdata.Tables[5].Rows.Count > 0)
                {
                    ViewBag.indirinc_tmpdata = dtdata.Tables[5];
                }
                if (dtdata.Tables[6].Rows.Count > 0)
                {
                    ViewBag.totamt_tmpdata = dtdata.Tables[6];
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
                    if (RptType == "H")
                    {
                        htmlcontent = ConvertPartialViewToString1(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/ProfitAndLossStatement/ProfitAndLossHorizontalPrint.cshtml"));
                    }
                    else
                    {
                        htmlcontent = ConvertPartialViewToString1(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/ProfitAndLossStatement/ProfitAndLossVerticalPrint.cshtml"));

                    }
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

                    return File(bytes.ToArray(), "application/pdf", "ProfitAndLoss.pdf");
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










