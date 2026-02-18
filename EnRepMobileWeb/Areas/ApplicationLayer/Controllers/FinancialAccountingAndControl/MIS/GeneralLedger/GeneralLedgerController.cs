using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.AccountPayable
{
    public class GeneralLedgerController : Controller
    {
        string CompID, BrID, UserID, language = String.Empty;
        string DocumentMenuId = "105104135105", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        Common_IServices _Common_IServices;
        GeneralLedger_ISERVICE GeneralLedger_ISERVICE;
        public GeneralLedgerController(Common_IServices _Common_IServices, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/GeneralLedger
        public ActionResult GeneralLedger()
        {
            GeneralLedgerModel _GeneralLedgerModel = new GeneralLedgerModel();
            string Comp_ID = string.Empty;
            string User_id = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                User_id = Session["UserId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
                ViewBag.vb_br_id = Session["BranchId"].ToString();
            }
            List<curr> currList = new List<curr>();
            dt = Getcurr("A");
            foreach (DataRow dr in dt.Rows)
            {
                curr curr = new curr();
                curr.curr_id = dr["curr_id"].ToString();
                curr.curr_name = dr["curr_name"].ToString();
                currList.Add(curr);
            }
            currList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });

            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(Comp_ID, User_id);
            ViewBag.br_list = br_list;

            DataSet dttbl = new DataSet();
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                _GeneralLedgerModel.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString(); 
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();
                ViewBag.ToFyMindate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();/*Add by Hina on 14-08-2024 for current date*/
                ViewBag.fylist = dttbl.Tables[1];
            }
            

                _GeneralLedgerModel.currList = currList;
            ViewBag.MenuPageName = getDocumentName();
            _GeneralLedgerModel.Title = title;
            _GeneralLedgerModel.GL_listByFilter = null;
            ViewBag.VBRoleList = GetRoleList();
            ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/GeneralLedger/GeneralLedger.cshtml", _GeneralLedgerModel);
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
                DataTable dt = GeneralLedger_ISERVICE.GetCurrList(Comp_ID, Currtype);
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
                DataSet dt = GeneralLedger_ISERVICE.Get_FYList(Comp_ID, BrID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult SearchGeneralLedgerDetail(string acc_id, string acc_group, string acc_type, string curr, string Fromdate, string Todate,string Rpt_As,string brlist)
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
                GeneralLedgerModel _GeneralLedgerModel = new GeneralLedgerModel();
                List<GLDetail_List> _GLDetail_List = new List<GLDetail_List>();
                DataSet dtdata = new DataSet();
               
                if (Rpt_As == "MW")
                {
                    dtdata = GeneralLedger_ISERVICE.GetGernalLedgerDetails(CompID, BrID, acc_id, acc_group, acc_type, curr, Fromdate, Todate, Rpt_As, brlist);
                    _GeneralLedgerModel.GL_listByFilter = "GLlistByFilter";

                    if (dtdata.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.mnthwise = dtdata.Tables[0];
                    }
                    if (dtdata.Tables[1].Rows.Count > 0)
                    {
                        ViewBag.glwise = dtdata.Tables[1];
                    }
                    if (dtdata.Tables[2].Rows.Count > 0)
                    {

                        ViewBag.docwise = dtdata.Tables[2];
                    }

                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialGeneralLedgerMonthWise.cshtml");
                }
                else
                {                  
                    //if (dtdata.Tables[0].Rows.Count > 0) // Commented by Suraj Maurya on 25-06-2025
                    //{
                    //    ViewBag.GL_Details = dtdata.Tables[0];
                    //}

                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialGeneralLedger.cshtml", _GeneralLedgerModel);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public ActionResult GetAutoCompleteGLList(GeneralLedgerModel _GeneralLedgerModel)
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
                if (string.IsNullOrEmpty(_GeneralLedgerModel.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _GeneralLedgerModel.ddlGroup;
                }
                GLList = GeneralLedger_ISERVICE.GLSetupGroupDAL(GroupName, Comp_ID);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(GLList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteAccGrp(GeneralLedgerModel _GeneralLedgerModel)
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
                if (string.IsNullOrEmpty(_GeneralLedgerModel.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _GeneralLedgerModel.ddlGroup;
                }
                suggestions = GeneralLedger_ISERVICE.AccGrpListGroupDAL(GroupName, Comp_ID);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(suggestions.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult GeneralLedgerPrintCsv(GeneralLedgerModel _GeneralLedgerModel, string command)
        {
            try
            {
                if (_GeneralLedgerModel.hdnPDFPrint == "Print")
                {
                    command = "Print";
                }
                if (_GeneralLedgerModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "Print":
                        _GeneralLedgerModel.hdnPDFPrint = null;
                        return GenratePdfFile(_GeneralLedgerModel);
                    case "CsvPrint":
                        _GeneralLedgerModel.hdnCSVPrint = null;
                        return ExportTrialBalanceData(_GeneralLedgerModel);

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
        /*--------------------------For PDF Print Start--------------------------*/

        //[HttpPost]
        public FileResult GenratePdfFile(GeneralLedgerModel _model)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                var model = new Search_Parmeters();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    model.CompId = CompID;
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                    model.BrId = BrID;
                }
                _model.hdnPDFPrint = null;
                JArray jObject = JArray.Parse(_model.PrintData);
                string acc_id = jObject[0]["acc_id"].ToString();
                string acc_group = jObject[0]["acc_group"].ToString();
                string acc_type = jObject[0]["acc_type"].ToString();
                string curr = jObject[0]["curr"].ToString();
                string Fromdate = jObject[0]["Fromdate"].ToString();
                string Todate = jObject[0]["Todate"].ToString();
                string Rpt_As = jObject[0]["Rpt_As"].ToString();
                string BrId_List = jObject[0]["brid_list"].ToString();
                model.AccID = acc_id;
                model.AccGrp = acc_group;
                model.AccType = acc_type;
                model.CurrId = curr;
                model.From_dt = Fromdate;
                model.To_dt = Todate;
                model.Rpt_As = Rpt_As;
                model.Flag = "CSV";
                model.br_ids = BrId_List;
                DataSet dtdata = GeneralLedger_ISERVICE.GetGernalLedgerDetails(model, 0, 25, "", "SrNo", "asc");
                //DataSet dtdata = GeneralLedger_ISERVICE.GetGernalLedgerDetails(CompID, BrID, acc_id, acc_group, acc_type
                //    , curr, Fromdate, Todate, Rpt_As);
                DataTable Details = new DataTable();
                Details = dtdata.Tables[0]; //ToDataTable(_model.PrintData);
                DataView data = new DataView();
                data = Details.DefaultView;
                //data.Sort = "vou_no";
                //data.Sort = "vou_dt";
                //Details = data.ToTable();
                DataTable dt = new DataTable();
                dt = data.ToTable(true, "acc_name");
                //dt.Columns.Add("acc_name", typeof(string));
                //string acc_name = "";
                //foreach(DataRow dr in Details.Rows)
                //{
                //    if (acc_name == "")
                //    {
                //        DataRow dtrow = dt.NewRow();
                //        dtrow["acc_name"] = dr["acc_name"].ToString();
                //        dt.Rows.Add(dtrow);
                //    }
                //    else
                //    {
                //        if (acc_name != dr["acc_name"].ToString())
                //        {
                //            DataRow dtrow = dt.NewRow();
                //            dtrow["acc_name"] = dr["acc_name"].ToString();
                //            dt.Rows.Add(dtrow);
                //        }
                //    }
                //    acc_name= dr["acc_name"].ToString();
                //}
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, BrID, BrId_List);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.Details = Details;
                ViewBag.FromDate = Convert.ToDateTime(_model.FromDate).ToString("dd-MM-yyyy");
                ViewBag.ToDate = Convert.ToDateTime(_model.ToDate).ToString("dd-MM-yyyy");
                //ViewBag.CompanyName = "Alaska Exports";
                ViewBag.DocName = "General Ledger";
                ViewBag.DocumentMenuId = DocumentMenuId;
                if (string.IsNullOrEmpty(_model.curr_Name))
                {
                    ViewBag.Currency = "INR";
                }
                else
                {
                    ViewBag.Currency = _model.curr_Name;
                }
                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4.Rotate(), 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    data = Details.DefaultView;
                    DataTable PrintGL = new DataTable();
                    pdfDoc.Open();
                    string Start = "Y";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Start == "N")
                        {
                            pdfDoc.NewPage();
                        }
                        else
                        {
                            Start = "N";
                        }
                        
                        //data = Details.DefaultView;
                        data.RowFilter = "acc_name='" + dr["acc_name"].ToString()+"'";
                        PrintGL = data.ToTable();
                        ViewBag.PrintGL = PrintGL;
                        //ViewBag.GLAccountName = PrintGL.Rows[0]["op_bal"].ToString();
                        ViewBag.GLAccountName = PrintGL.Rows[0]["br_op_bal_bs"].ToString();
                        //ViewBag.GLAccountName = PrintGL.Rows[PrintGL.Rows.Count-1]["closing_bal"].ToString();
                        ViewBag.GLAccountName = PrintGL.Rows[PrintGL.Rows.Count-1]["br_acc_bal_bs"].ToString();
                        htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/GeneralLedger/GeneralLedgerPrint.cshtml"));
                        reader = new StringReader(htmlcontent);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    }
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
                    return File(bytes.ToArray(), "application/pdf", "GeneralLedger.pdf");
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

                dtItem.Columns.Add("acc_name", typeof(string));
                dtItem.Columns.Add("acc_id", typeof(string));
                dtItem.Columns.Add("vou_no", typeof(string));
                dtItem.Columns.Add("vou_dt", typeof(string));
                dtItem.Columns.Add("narr", typeof(string));
                dtItem.Columns.Add("op_bal", typeof(string));
                dtItem.Columns.Add("op_type", typeof(string));
                dtItem.Columns.Add("dr_amt", typeof(string));
                dtItem.Columns.Add("cr_amt", typeof(string));
                dtItem.Columns.Add("closing_bal", typeof(string));
                dtItem.Columns.Add("closing_type", typeof(string));
              

                if (Details != null)
                {
                    JArray jObject = JArray.Parse(Details);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["acc_name"] = jObject[i]["acc_name"].ToString();
                        dtrowLines["acc_id"] = jObject[i]["acc_id"].ToString();
                        dtrowLines["vou_no"] = jObject[i]["vou_no"].ToString();
                        dtrowLines["vou_dt"] = jObject[i]["vou_dt"].ToString();
                        dtrowLines["narr"] = jObject[i]["narr"].ToString();
                        dtrowLines["op_bal"] = jObject[i]["op_bal"].ToString();
                        dtrowLines["op_type"] = jObject[i]["op_type"].ToString();
                        dtrowLines["dr_amt"] = jObject[i]["dr_amt"].ToString();
                        dtrowLines["cr_amt"] = jObject[i]["cr_amt"].ToString();
                        dtrowLines["closing_bal"] = jObject[i]["closing_bal"].ToString();
                        dtrowLines["closing_type"] = jObject[i]["closing_type"].ToString();

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
        public FileResult ExportTrialBalanceData(GeneralLedgerModel _model)
        {
            try
            {
                List<GLDetail_List> _GLDetail_ListData = new List<GLDetail_List>();
                var model = new Search_Parmeters();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    model.CompId = CompID;// Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                    model.BrId = BrID;// Session["BranchId"].ToString();
                }                
                JArray jObject = JArray.Parse(_model.PrintData);
                string acc_id = jObject[0]["acc_id"].ToString();
                string acc_group = jObject[0]["acc_group"].ToString();
                string acc_type = jObject[0]["acc_type"].ToString();
                string curr = jObject[0]["curr"].ToString();
                string Fromdate = jObject[0]["Fromdate"].ToString();
                string Todate = jObject[0]["Todate"].ToString();
                string Rpt_As = jObject[0]["Rpt_As"].ToString();
                model.AccID = acc_id;
                model.AccGrp = acc_group;
                model.AccType = acc_type;
                model.CurrId = curr;
                model.From_dt = Fromdate;
                model.To_dt = Todate;
                model.Rpt_As = Rpt_As;
                model.Flag = "CSV";
                model.br_ids = _model.BrList;

                //var sortcolumn = "acc_name";
                //var sortcolumndir = "asc";

                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                //ViewBag.ValDigit = ValDigit;

                DataSet dtdata = GeneralLedger_ISERVICE.GetGernalLedgerDetails(model, 0, 25, IsNull(_model.searchValue,""), "SrNo", "asc");
                //GeneralLedger_ISERVICE.GetGernalLedgerDetails(CompID, BrID, acc_id, acc_group, acc_type, curr, Fromdate, Todate, Rpt_As);

                if (dtdata.Tables[0].Rows.Count > 0)
                {
                _GLDetail_ListData = dtdata.Tables[0].AsEnumerable()
               .Select((row, index) => new GLDetail_List
               {
                   SrNo = row.Field<Int64>("SrNo"),
                   acc_name = row.Field<string>("acc_name"),
                   acc_id = row.Field<string>("acc_id"),
                   src_doc_no = row.Field<string>("src_doc_no"),
                   src_doc_dt = row.Field<string>("src_doc_dt"),
                   narr = row.Field<string>("narr"),
                   br_op_bal_bs = row.Field<string>("br_op_bal_bs"),
                   br_op_bal_type = row.Field<string>("br_op_bal_type"),
                   dr_amt = row.Field<string>("dr_amt"),
                   cr_amt = row.Field<string>("cr_amt"),
                   br_acc_bal_bs = row.Field<string>("br_acc_bal_bs"),
                   br_acc_bal_type = row.Field<string>("br_acc_bal_type")
               }).ToList();
                //    int rowno = 0;
                //    foreach (DataRow dr in dtdata.Tables[0].Rows)
                //    {
                //        GLDetail_List _GLDetail_List = new GLDetail_List();
                //        _GLDetail_List.SrNo = rowno + 1;
                //        _GLDetail_List.acc_name = dr["acc_name"].ToString();
                //        _GLDetail_List.acc_id = dr["acc_id"].ToString();
                //        _GLDetail_List.vou_no = dr["src_doc_no"].ToString();
                //        _GLDetail_List.vou_dt = dr["src_doc_dt"].ToString();
                //        _GLDetail_List.narr = dr["narr"].ToString();
                //        _GLDetail_List.op_bal = Convert.ToDecimal(dr["br_op_bal_bs"]).ToString(ValDigit);
                //        _GLDetail_List.op_type = dr["br_op_bal_type"].ToString();
                //        _GLDetail_List.dr_amt = Convert.ToDecimal(dr["dr_amt"]).ToString(ValDigit);
                //        _GLDetail_List.cr_amt = Convert.ToDecimal(dr["cr_amt"]).ToString(ValDigit);
                //        _GLDetail_List.closing_bal = Convert.ToDecimal(dr["br_acc_bal_bs"]).ToString(ValDigit);
                //        _GLDetail_List.closing_type = dr["br_acc_bal_type"].ToString();
                //        _GLDetail_ListData.Add(_GLDetail_List);
                //        rowno = rowno + 1;
                //    }
                }
                //var ItemListData = (from tempitem in _GLDetail_ListData select tempitem);
                //Sorting
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                    //ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                    //ItemListData.OrderBy(p => p.acc_name);
                //}
                //string searchValue = _model.searchValue;
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    searchValue = searchValue.ToUpper();
                //    ItemListData = ItemListData.Where(m => m.acc_name.ToUpper().Contains(searchValue) || m.acc_id.ToUpper().Contains(searchValue)
                //    || m.vou_no.ToUpper().Contains(searchValue) || m.vou_dt.ToUpper().Contains(searchValue) || m.narr.ToUpper().Contains(searchValue)
                //    || m.op_bal.ToUpper().Contains(searchValue) || m.op_type.ToUpper().Contains(searchValue) || m.dr_amt.ToUpper().Contains(searchValue)
                //    || m.cr_amt.ToUpper().Contains(searchValue) || m.closing_bal.ToUpper().Contains(searchValue) || m.closing_type.ToUpper().Contains(searchValue));
                //}
                var data = _GLDetail_ListData.ToList();
                _model.hdnCSVPrint = null;
                DataTable dt = new DataTable();
                dt = ToGernalLedgerDetailExl(data);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("General Ledger", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToGernalLedgerDetailExl(List<GLDetail_List> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr No.", typeof(int));
            dataTable.Columns.Add("GL Name", typeof(string));
            dataTable.Columns.Add("GL Voucher Number", typeof(string));
            dataTable.Columns.Add("GL Voucher Date", typeof(string));
            dataTable.Columns.Add("Narration", typeof(string));
            dataTable.Columns.Add("Opening Balance", typeof(decimal));
            dataTable.Columns.Add("Balance Type", typeof(string));
            dataTable.Columns.Add("Debit Amount", typeof(decimal));
            dataTable.Columns.Add("Credit Amount", typeof(decimal));
            dataTable.Columns.Add("Closing Balance", typeof(decimal));
            dataTable.Columns.Add("Balance  Type", typeof(string));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr No."] = item.SrNo;
                rows["GL Name"] = item.acc_name;
                rows["GL Voucher Number"] = item.src_doc_no;
                rows["GL Voucher Date"] = item.src_doc_dt;
                rows["Narration"] = item.narr;
                rows["Opening Balance"] = item.br_op_bal_bs;
                rows["Balance Type"] = item.br_op_bal_type;
                rows["Debit Amount"] = item.dr_amt;
                rows["Credit Amount"] = item.cr_amt;
                rows["Closing Balance"] = item.br_acc_bal_bs;
                rows["Balance  Type"] = item.br_acc_bal_type;
                dataTable.Rows.Add(rows);
            }
            return dataTable;
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

                List<GLDetail_List> _DetailedModel = new List<GLDetail_List>();
                List<GLDetail_List> _DataTotal = new List<GLDetail_List>();
                (_DetailedModel, recordsTotal, _DataTotal) = getDtList(model, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _DetailedModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                }

                //Paging     
                var data = ItemListData.ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data, footer = _DataTotal });

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        //Added by Suraj Maurya on 20-06-2026
        private (List<GLDetail_List>, int, List<GLDetail_List>) getDtList(Search_model search_Model, int skip, int pageSize, string searchValue, string sortColumn, string sortColumnDir)
        {
            List<GLDetail_List> _ItemListModel = new List<GLDetail_List>();
            List<GLDetail_List> _DataTotal = new List<GLDetail_List>();
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

                DSet = GeneralLedger_ISERVICE.GetGernalLedgerDetails(model1, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                if (DSet.Tables.Count >= 2)
                {
                    
                    if (DSet.Tables[0].Rows.Count > 0)
                    {
                        _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new GLDetail_List
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    acc_name = row.Field<string>("acc_name"),
                    acc_id = row.Field<string>("acc_id"),
                    src_doc_no = row.Field<string>("src_doc_no"),
                    src_doc_dt = row.Field<string>("src_doc_dt"),
                    srcdocdt = row.Field<string>("srcdocdt"),
                    narr = row.Field<string>("narr"),
                    br_op_bal_bs = row.Field<string>("br_op_bal_bs"),
                    br_op_bal_type = row.Field<string>("br_op_bal_type"),
                    dr_amt = row.Field<string>("dr_amt"),
                    cr_amt = row.Field<string>("cr_amt"),
                    br_acc_bal_bs = row.Field<string>("br_acc_bal_bs"),
                    br_acc_bal_type = row.Field<string>("br_acc_bal_type")
                }).ToList();

                    }
                    if (DSet.Tables[1].Rows.Count > 0)
                    {
                        Total_Records = Convert.ToInt32(DSet.Tables[1].Rows[0]["total_rows"]);
                    }
                    if (DSet.Tables[0].Rows.Count > 0)
                    {
                        _DataTotal = DSet.Tables[2].AsEnumerable()
                .Select((row, index) => new GLDetail_List
                {
                    br_op_bal_bs = row.Field<string>("br_op_bal_bs"),
                    dr_amt = row.Field<string>("dr_amt"),
                    cr_amt = row.Field<string>("cr_amt"),
                    br_acc_bal_bs = row.Field<string>("br_acc_bal_bs")
                    
                }).ToList();
                    }
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
            return (_ItemListModel, Total_Records, _DataTotal);
        }

        string IsNull(string InStr, string OutStr)
        {
            return string.IsNullOrEmpty(InStr) ? OutStr : InStr;
        }
        /*---------------------------------------DataTable Functionality End-------------------------------------------*/
    }
}