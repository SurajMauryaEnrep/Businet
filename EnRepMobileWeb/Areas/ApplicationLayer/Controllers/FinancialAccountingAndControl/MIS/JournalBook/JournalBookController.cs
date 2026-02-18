using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.JournalBook;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.JournalBook;
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
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.JournalBook
{
    public class JournalBookController : Controller
    {
        string CompID, BrID, UserID, title, language = String.Empty;
        string DocumentMenuId = "105104135133";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        JournalBook_ISERVICE _JournalBook_ISERVICE;
        List<JournalBookList> _JournalBookList;
        public JournalBookController(Common_IServices _Common_IServices, JournalBook_ISERVICE _JournalBook_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._JournalBook_ISERVICE = _JournalBook_ISERVICE;
        }
        // GET: ApplicationLayer/JournalBook
        public ActionResult JournalBook()
        {
            CommonPageDetails();
            JournalBookModel _JournalBookModel = new JournalBookModel();

            DataSet dttbl = new DataSet();
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                _JournalBookModel.FromVouDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();
                ViewBag.ToFyMindate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.fylist = dttbl.Tables[1];
            }
            /*--------To Bind GL Group ---------*/
            List<GlGroupName> _GroupList = new List<GlGroupName>();
            _GroupList.Insert(0, new GlGroupName() { acc_grp_id = "0", acc_grp_name = "---Select---" });
            _JournalBookModel.GlGroupNameList = _GroupList;

            /*--------To Bind GL Account ---------*/
            List<GLAccount> glacc = new List<GLAccount>();
            glacc.Insert(0, new GLAccount() { glacc_id = "0", glacc_name = "---Select---" });
            _JournalBookModel.GLAccountList = glacc;
            
            /*--------To Bind Status List ---------*/
            List<Status> statusLists = new List<Status>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                Status list = new Status();
                list.status_id = dr["status_code"].ToString();
                list.status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            statusLists.Insert(0, new Status() { status_id = "0", status_name = "---Select---" });
            _JournalBookModel.StatusList = statusLists;
            /*--------To Bind Creator List ---------*/
            List<CreatorName> crtList = new List<CreatorName>();
            foreach (DataRow dr in ViewBag.CreatorList.Rows) 
            
            {
                CreatorName Clist = new CreatorName();
                Clist.creator_id = dr["create_id"].ToString();
                Clist.creator_name = dr["creator_nm"].ToString();
                crtList.Add(Clist);
            }
            crtList.Insert(0, new CreatorName() { creator_id = "0", creator_name = "---Select---" });
            _JournalBookModel.CreatorList = crtList;
            /*--------To Bind Approver List ---------*/
            List<ApproverName> appList = new List<ApproverName>();
            foreach (DataRow dr in ViewBag.ApproverList.Rows)
            {
                ApproverName Aplist = new ApproverName();
                Aplist.approver_id = dr["app_id"].ToString();
                Aplist.approver_name = dr["app_nm"].ToString();
                appList.Add(Aplist);
            }
            appList.Insert(0, new ApproverName() { approver_id = "0", approver_name = "---Select---" });
            _JournalBookModel.ApproverList = appList;
            ViewBag.MenuPageName = getDocumentName();
            _JournalBookModel.Title = title;
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/JournalBook/JournalBook.cshtml", _JournalBookModel);
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
                DataSet dt = _JournalBook_ISERVICE.Get_FYList(Comp_ID, BrID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult GetAutoCompleteAccGrp(JournalBookModel _JournalBookModel)
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
                if (string.IsNullOrEmpty(_JournalBookModel.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _JournalBookModel.ddlGroup;
                }
                suggestions = _JournalBook_ISERVICE.AccGrpListGroupDAL(GroupName, Comp_ID);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(suggestions.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteGLList(JournalBookModel _JournalBookModel,string  GLGroupId/*, string type*/)
        {

            string GroupName = string.Empty;
            Dictionary<string, string> GLList = new Dictionary<string, string>();
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
               
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_JournalBookModel.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _JournalBookModel.ddlGroup;
                   
                }
                //if(type== "Onchange")
                //{
                    GLList = _JournalBook_ISERVICE.GetGLAccountList(Comp_ID, Br_ID, GLGroupId, GroupName);
                //}
                //else
                //{
                //    GLList = _JournalBook_ISERVICE.GLSetupGroupDAL(GroupName, Comp_ID);
                //}
                

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(GLList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult BindGLAccountList(JournalBookModel _JournalBookModel, int GLGroupId)
        //{
        //    JsonResult DataRows = null;
        //    string product_id = string.Empty;
        //    string Comp_ID = string.Empty;
        //    string Br_ID = string.Empty;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();

        //            if (Session["BranchId"] != null)
        //            {
        //                Br_ID = Session["BranchId"].ToString();
        //            }

        //            DataSet Data = _JournalBook_ISERVICE.GetGLAccountList(Comp_ID, Br_ID, GLGroupId);
        //            List<GLAccount> glacc = new List<GLAccount>();

        //            foreach (DataRow dr in Data.Tables[0].Rows)
        //            {
        //                GLAccount wrk = new GLAccount();
        //                wrk.glacc_id = dr["acc_id"].ToString();
        //                wrk.glacc_name = dr["acc_name"].ToString();
        //                glacc.Add(wrk);

        //            }
        //            glacc.Insert(0, new GLAccount() { glacc_id = "0", glacc_name = "---Select---" });
        //            _JournalBookModel.GLAccountList = glacc;
        //            DataRows = Json(JsonConvert.SerializeObject(Data));/*Result convert into Json Format for javasript*/

        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    return DataRows;
        //}
        public ActionResult SearchJournalBookDetails(string FromDate, string ToDate, string GroupId, string AccountID, string AmtFrom,
            string AmtTo, string VouTyp, string CreatBy, string CreatOn, string AppBy, string AppOn, string Narr, string Status)
        {

            try
            {
                JournalBookModel objModel = new JournalBookModel();
                objModel.SearchStatus = "SEARCH";

                ViewBag.JournalBookDetails = GetJournalBookListDetails(FromDate, ToDate, GroupId, AccountID, AmtFrom, AmtTo,
                VouTyp, CreatBy, CreatOn, AppBy, AppOn, Narr, Status);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialJournalBook.cshtml", objModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public DataTable GetJournalBookListDetails(string FromDate, string ToDate, string GroupId, string AccountID, string AmtFrom,
            string AmtTo, string VouTyp, string CreatBy, string CreatOn, string AppBy, string AppOn, string Narr, string Status)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrID = Session["BranchId"].ToString();
                DataTable dt = _JournalBook_ISERVICE.GetJournalBookDetailsMIS(CompID, BrID, FromDate, ToDate, GroupId, AccountID, AmtFrom, AmtTo,
                VouTyp, CreatBy, CreatOn, AppBy, AppOn, Narr, Status);
                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                //return Json("ErrorPage");
                //  return View("~/Views/Shared/Error.cshtml");
                return null;
            }
        }
        public ActionResult JB_GetCostCenterData(string Vou_No, string Vou_Dt, string GLAcc_id)
        {
            try
            {
                CostCenterDt _CC_Model = new CostCenterDt();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }

                DataSet ds = _JournalBook_ISERVICE.GetCostCenterData(CompID, BrID, Vou_No, Vou_Dt, GLAcc_id);
                //ViewBag.CostCenterReAllocationDetail = ds;
                ViewBag.CC_type = ds.Tables[0];

                ViewBag.CC_Item = ds.Tables[1];
               
               // DataSet ds1 = _Common_IServices.GetCstCntrData(CompID, BrID, "0", Flag);

                List<CostcntrType> Cctypelist = new List<CostcntrType>();
                Cctypelist.Insert(0, new CostcntrType() { cc_id = "0", cc_name = "---Select---" });
                _CC_Model.costcntrtype = Cctypelist;
                ViewBag.CCTotalAmt = ds.Tables[2].Rows[0]["total_cc_Amount"];//add by sm 11-12-2024
                ViewBag.DocId = DocumentMenuId;//add by sm 11-12-2024
                _CC_Model.disflag = "Y";
                return PartialView("~/Areas/Common/Views/Cmn_PartialCostCenterDetail.cshtml", _CC_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }

        }

        public ActionResult JournalBookPrintCsv(JournalBookModel _JournalBookModel, string command)
        {
            try
            {
                if (_JournalBookModel.hdnPDFPrint == "Print")
                {
                    command = "Print";
                }
                if (_JournalBookModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "Print":
                        _JournalBookModel.hdnPDFPrint = null;
                        return GenratePdfFile(_JournalBookModel);
                    case "CsvPrint":
                        _JournalBookModel.hdnCSVPrint = null;
                        return ExportJournalBookData(_JournalBookModel);
                    //return ExportTrialBalanceData(_JournalBookModel);

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
        public FileResult ExportJournalBookData(JournalBookModel _JournalBookModel)
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

                List<JournalBookList> _JournalBookListDetail = new List<JournalBookList>();

                _JournalBookListDetail = getJournalBookList(_JournalBookModel.Allfilters);
                var ItemListData = (from tempitem in _JournalBookListDetail select tempitem);

                string searchValue = _JournalBookModel.searchValue;
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    ItemListData = ItemListData.Where(m => m.Vou_contain_row.ToUpper().Contains(searchValue)
                    || m.Vou_No.ToUpper().Contains(searchValue) || m.Vou_Dt.ToUpper().Contains(searchValue)
                    || m.Vou_Type.ToUpper().Contains(searchValue) || m.Vou_TypeName.ToUpper().Contains(searchValue) || m.Instrument_Type.ToUpper().Contains(searchValue)
                    || m.Instrument_TypeName.ToUpper().Contains(searchValue) || m.Instrument_No.ToUpper().Contains(searchValue) || m.StatusId.ToUpper().Contains(searchValue)
                    || m.StatusName.ToUpper().Contains(searchValue) || m.CurrId.ToUpper().Contains(searchValue) || m.CurrName.ToUpper().Contains(searchValue)
                    || m.Acc_Id.ToUpper().Contains(searchValue) || m.Acc_Name.ToUpper().Contains(searchValue) || m.Dr_Amt.ToUpper().Contains(searchValue)
                    || m.Cr_Amt.ToUpper().Contains(searchValue) || m.Narr.ToUpper().Contains(searchValue)) ;
                }
                var data = ItemListData.ToList();

                string Fstring = string.Empty;
                string[] fdata;
                Fstring = _JournalBookModel.Allfilters;
                fdata = Fstring.Split(',');
                DataTable dt = new DataTable();

                dt = ToJournalBookDetailExl(data);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Journal Book", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        private List<JournalBookList> getJournalBookList(string filters)
        {
            List<JournalBookList> _JournalBookListDetail = new List<JournalBookList>();
            try
            {
                string FromDate, ToDate, GroupId, AccountID, AmtFrom, AmtTo,
                VouTyp, CreatBy, CreatOn, AppBy, AppOn, Narr, Status;
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
                DataTable dt = new DataTable();
                if (filters != null)
                {
                    if (filters != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = filters;
                        fdata = Fstring.Split(',');

                        FromDate = fdata[0]; 
                        ToDate = fdata[1];  
                        GroupId = fdata[2];  
                        AccountID = fdata[3]; 
                        AmtFrom = fdata[4]; 
                        AmtTo = fdata[5]; 
                        VouTyp = fdata[6]; 
                        CreatBy = fdata[7]; 
                        CreatOn = fdata[8]; 
                        AppBy = fdata[9];
                        AppOn = fdata[10];
                        Narr = fdata[11];
                        Status = fdata[12];

                         dt = _JournalBook_ISERVICE.GetJournalBookDetailsMIS(CompID, BrID,FromDate, ToDate, GroupId, AccountID, AmtFrom, AmtTo,
                VouTyp, CreatBy, CreatOn, AppBy, AppOn, Narr, Status);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        JournalBookList _JournalBookList = new JournalBookList();
                        _JournalBookList.SrNo = rowno + 1;
                        //_JournalBookList.sono_total = dr["sno3"].ToString();
                        _JournalBookList.Vou_contain_row = dr["Sno_vou_no"].ToString();
                        _JournalBookList.Vou_No = dr["vou_no"].ToString();
                        _JournalBookList.Vou_Dt = dr["vou_date"].ToString();
                        _JournalBookList.Vou_Type = dr["vou_type"].ToString();
                        _JournalBookList.Vou_TypeName = dr["VouTypeName"].ToString();
                        _JournalBookList.Instrument_Type = dr["InstrumentTyp"].ToString();
                        _JournalBookList.Instrument_TypeName = dr["InstrumentTypName"].ToString();
                        _JournalBookList.Instrument_No = dr["InstrumentNo"].ToString();
                        _JournalBookList.StatusId = dr["vou_status"].ToString();
                        _JournalBookList.StatusName = dr["VouStatusName"].ToString();
                        _JournalBookList.CurrId = dr["curr_id"].ToString();
                        _JournalBookList.CurrName = dr["curr_name"].ToString();
                        _JournalBookList.Acc_Id = dr["acc_id"].ToString();
                        _JournalBookList.Acc_Name = dr["acc_name"].ToString();
                        _JournalBookList.Dr_Amt = dr["dr_amt_bs"].ToString();
                        _JournalBookList.Cr_Amt = dr["cr_amt_bs"].ToString();
                        _JournalBookList.Narr = dr["narr"].ToString();
                        _JournalBookListDetail.Add(_JournalBookList);
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
            return _JournalBookListDetail;
        }
        public DataTable ToJournalBookDetailExl(List<JournalBookList> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr. No.", typeof(int));
            dataTable.Columns.Add("Voucher Number", typeof(string));
            dataTable.Columns.Add("Voucher Date", typeof(string));
            dataTable.Columns.Add("Voucher Type", typeof(string));
            dataTable.Columns.Add("Instrument Type", typeof(string));
            dataTable.Columns.Add("Instrument Number", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("Currency", typeof(string));
            dataTable.Columns.Add("GL Account", typeof(string));
            dataTable.Columns.Add("Debit Amount", typeof(decimal));
            dataTable.Columns.Add("Credit Amount", typeof(decimal));
            dataTable.Columns.Add("Narration", typeof(string));
            

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr. No."] = item.SrNo;
                rows["Voucher Number"] = item.Vou_No;
                rows["Voucher Date"] = item.Vou_Dt;
                rows["Voucher Type"] = item.Vou_TypeName;
                rows["Instrument Type"] = item.Instrument_TypeName;
                rows["Instrument Number"] = item.Instrument_No;
                rows["Status"] = item.StatusName;
                rows["Currency"] = item.CurrName;
                rows["GL Account"] = item.Acc_Name;
                rows["Debit Amount"] = item.Dr_Amt;
                rows["Credit Amount"] = item.Cr_Amt;
                rows["Narration"] = item.Narr;
                
                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        /*--------------------------For PDF Print Start--------------------------*/

        //[HttpPost]
        public FileResult GenratePdfFile(JournalBookModel _JournalBookModel)
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
                _JournalBookModel.hdnPDFPrint = null;
                JArray jObject = JArray.Parse(_JournalBookModel.PrintData);
                string FromDate = jObject[0]["FromDate"].ToString();
                string ToDate = jObject[0]["ToDate"].ToString();
                string GroupId = jObject[0]["GroupId"].ToString();
                string AccountID = jObject[0]["AccountID"].ToString();
                string AmtFrom = jObject[0]["AmtFrom"].ToString();
                string AmtTo = jObject[0]["AmtTo"].ToString();
                string VouTyp = jObject[0]["VouTyp"].ToString();
                string CreatBy = jObject[0]["CreatBy"].ToString();
                string CreatOn = jObject[0]["CreatOn"].ToString();
                string AppBy = jObject[0]["AppBy"].ToString();
                string AppOn = jObject[0]["AppOn"].ToString();
                string Narr = jObject[0]["Narr"].ToString();
                string Status = jObject[0]["Status"].ToString();
                DataTable Details = _JournalBook_ISERVICE.GetJournalBookDetailsMIS(CompID, BrID, FromDate, ToDate, GroupId, AccountID, AmtFrom, AmtTo,
                VouTyp, CreatBy, CreatOn, AppBy, AppOn, Narr, Status);
                
                if(_JournalBookModel.ddlGroup=="0")
                {
                    _JournalBookModel.ddlGroupName = "All";
                }
                if (_JournalBookModel.GLAccount == "0")
                {
                    _JournalBookModel.GLAccountName = "All";
                }
                if (_JournalBookModel.VouType == "0")
                {
                    _JournalBookModel.VouTypeName = "All";
                }
                if (_JournalBookModel.CreatedBy == "0")
                {
                    _JournalBookModel.CreatorName = "All";
                }
                if (_JournalBookModel.ApprovedBy == "0")
                {
                    _JournalBookModel.ApproverName = "All";
                }
                if (_JournalBookModel.Status == "0")
                {
                    _JournalBookModel.StatusName = "All";
                }
                ViewBag.Details = Details;
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, BrID);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.Title = "Journal Book";
                //ViewBag.FromDate = Convert.ToDateTime(_JournalBookModel.FromVouDate).ToString("dd-MM-yyyy");
                //ViewBag.ToDate = Convert.ToDateTime(_JournalBookModel.ToVouDate).ToString("dd-MM-yyyy");

                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4.Rotate(), 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    
                    pdfDoc.Open();
                    
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/JournalBook/JournalBookPrint.cshtml", _JournalBookModel));
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
                    return File(bytes.ToArray(), "application/pdf", "JournalBook.pdf");
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

        //public ActionResult SearchJournalBookDetails(string FromDate, string ToDate, string GroupId, string AccountID, string AmtFrom,
        //    string AmtTo, string VouTyp, string CreatBy, string CreatOn, string AppBy, string AppOn, string Narr, string Status)
        //{
        //    _JournalBookList = new List<JournalBookList>();
        //    JournalBookModel _JournalBookModel = new JournalBookModel();

        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }

        //    if (Session["BranchId"] != null)
        //    {
        //        BrID = Session["BranchId"].ToString();
        //    }
        //    DataTable dt = new DataTable();
        //    dt = _JournalBook_ISERVICE.GetJournalBookDetailsMIS(CompID, BrID, FromDate, ToDate, GroupId, AccountID, AmtFrom, AmtTo,
        //        VouTyp, CreatBy, CreatOn, AppBy, AppOn, Narr, Status);


        //    _JournalBookModel.SearchStatus = "SEARCH";
        //    if (dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            JournalBookList _VouList = new JournalBookList();
        //            _VouList.Vou_No = dr["vou_no"].ToString();
        //            _VouList.Vou_Dt = dr["vou_dt"].ToString();
        //            _VouList.hdnVouDate = dr["vou_date"].ToString();
        //            _VouList.Vou_TypeName = dr["VouTypeName"].ToString();
        //            _VouList.Vou_Type = dr["vou_type"].ToString();
        //            _VouList.Instrument_TypeName = dr["InstrumentTypName"].ToString();
        //            _VouList.Instrument_No = dr["InstrumentNo"].ToString();
        //           _VouList.StatusName = dr["VouStatusName"].ToString();
        //            _VouList.StatusId = dr["vou_status"].ToString();
        //            _VouList.CurrName = dr["curr_name"].ToString();
        //            _VouList.CurrId = dr["curr_id"].ToString();
        //            _VouList.Acc_Name = dr["acc_name"].ToString();
        //            _VouList.AccTyp_Id = dr["acc_id"].ToString();
        //            _VouList.Dr_Amt = dr["dr_amt_bs"].ToString();
        //            _VouList.Cr_Amt = dr["cr_amt_bs"].ToString();
        //            _VouList.Narr = dr["narr"].ToString();

        //            _JournalBookList.Add(_VouList);
        //        }
        //    }
        //    _JournalBookModel.JournalBook_List = _JournalBookList;
        //    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialJournalBook.cshtml", _JournalBookModel);
        //}

        //public ActionResult JB_GetBillDetail(string AccId, string fromdt, string todt, string flag, string DocumentNumber
        //    , string Status, string Curr, string GlBrID)
        //{
        //    try
        //    {
        //        JsonResult DataRows = null;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        string BrchID = Session["BranchId"].ToString();
        //        if (!string.IsNullOrEmpty(GlBrID) /*&& Status!="A"*/)
        //        {
        //            BrchID = GlBrID;
        //        }
        //        DataSet ds = _BankPayment_IService.GetBillDetail(CompID, BrchID, AccId, fromdt, todt, flag, DocumentNumber, Status, Curr);

        //        DataRows = Json(JsonConvert.SerializeObject(ds));
        //        return DataRows;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }

        //}

        private void CommonPageDetails()
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _JournalBook_ISERVICE.GetAllDDLDetails(CompID, BrID, UserID, DocumentMenuId, language);
                string DocumentName = ds.Tables[0].Rows[0]["pagename"].ToString();
                ViewBag.StatusList = ds.Tables[1];
                ViewBag.CreatorList = ds.Tables[2];
                ViewBag.ApproverList = ds.Tables[3];

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