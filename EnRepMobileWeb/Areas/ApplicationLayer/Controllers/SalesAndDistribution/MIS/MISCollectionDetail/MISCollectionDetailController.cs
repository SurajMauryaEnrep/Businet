using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.MISCollectionDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.MISCollectionDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.MIS.MISCollectionDetail
{
    public class MISCollectionDetailController : Controller
    {
        string CompID, BrId, userid, language = String.Empty;
        string DocumentMenuId = "105103190102", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        MISCollectionDetail_IService _MISCollectionDetail_IService;
        public MISCollectionDetailController(Common_IServices _Common_IServices, MISCollectionDetail_IService _MISCollectionDetail_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._MISCollectionDetail_IService = _MISCollectionDetail_IService;
        }
        // GET: ApplicationLayer/MISCollectionDetail
        public ActionResult MISCollectionDetail()
        {
            MISCollectionDetail_Model model = new MISCollectionDetail_Model();
            CommonPageDetails();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrId = Session["BranchId"].ToString();
                ViewBag.vb_br_id = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                userid = Session["userid"].ToString();
            }
            model.categoryLists = custCategoryList();
            model.portFolioLists = custPortFolioLists();
            model.SalesPersons = GetSalesPersonList();
            model.regionLists = regionLists();
            GetCustomerDropdown(model);
            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(CompID, userid);
            br_list = _Common_IServices.Cmn_GetBrList(CompID, userid);
            ViewBag.br_list = br_list;
            SearchCollectionDetail("0","0","0","0", DateTime.Now.ToString("yyyy-MM-dd"), "A", "S",BrId,"0","0","0","0","N","0");
            model.Title = title;
            ViewBag.flag = "S";
            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/MISCollectionDetail/MISCollectionDetail.cshtml", model);
        }
        [HttpPost]
        public ActionResult SearchCollectionDetail(string Cust_id, string Cat_id, string Prf_id, string Reg_id, string AsDate, string ReceivableType, string ReportType, string brlist, string customerZone, string CustomerGroup, string state_id, string city_id,string includeZero, string sales_per)
        {
            try
            {
                MISCollectionDetail_Model model = new MISCollectionDetail_Model();
                string Partial_View = string.Empty; 
                DataSet ds = new DataSet();
                if (Cust_id == "")
                {
                    Cust_id = "0";
                }
                if (Cat_id == "")
                {
                    Cat_id = "0";
                }
                if (Prf_id == "")
                {
                    Prf_id = "0";
                }
                if (Reg_id == "")
                {
                    Reg_id = "0";
                }
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                ds = _MISCollectionDetail_IService.GetCollectionDetailList(CompID, BrId, userid, Cust_id, Cat_id, Prf_id, Reg_id, AsDate, 0, "List", 0, ReceivableType, ReportType, brlist, customerZone, CustomerGroup, state_id, city_id, includeZero, sales_per);
                model.ColAging = "Search";
                ViewBag.flag = ReportType;

                model.ReceivableType = ReceivableType;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.CollectionDetails = ds.Tables[0];
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ViewBag.CollectionDetailsTotal = ds.Tables[1];
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISCollectionDetail.cshtml", model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public ActionResult GetSalesAmtDetails(string Cust_id, string AsDate, int CurrId, string ReceivableType, string ReportType, string inv_no, string inv_dt, string brlist,string includeZero)
        {
            try
            {
                string CompID = string.Empty;
                DataSet ds = new DataSet();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                ds = _MISCollectionDetail_IService.GetSalesAmtDetails(CompID, BrId, Cust_id, AsDate, CurrId, ReceivableType, ReportType, inv_no, inv_dt, brlist, userid, includeZero);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.SalesAmountDetail = ds.Tables[0];
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSalesAmountDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult SearchPaidAmountDetail(string Cust_id, string curr_id, string AsDate, string ReceivableType, string ReportType, string brlist, string includeZero)/*Add by Hina sharma on 10-12-2024*/
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataSet dt = _MISCollectionDetail_IService.SearchPaidAmountDetail(CompID, BrId, userid, Cust_id, curr_id, AsDate, ReceivableType, ReportType, brlist, includeZero);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    ViewBag.PaidAmountDetail = dt.Tables[0];
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    ViewBag.PaidAmountDetailTotal = dt.Tables[1];
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPaymentDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public ActionResult CollectionDtl(MISCollectionDetail_Model model, string command)
        {
            try
            {
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                if (model.hdnPDFPrint == "Print")
                {
                    command = "Print";
                }            
                switch (command)
                {
                    case "Print":
                        return GenratePdfFile(model);
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
         [HttpPost]
        public FileResult GenratePdfFile(MISCollectionDetail_Model model)
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
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                //Added by Nidhi on 06-01-2026
                if(model.hdnPDFPrint == "Print")
                {
                    //_AccRecModel.supp_id = _AccRecModel.Hdnsupp_id;
                    //_AccRecModel.category = _AccRecModel.Hdncatg_id;
                    //_AccRecModel.portFolio = _AccRecModel.Hdnport_id;
                }
                model.hdnPDFPrint = null;
                DataTable Details = new DataTable();
                DataSet ds = new DataSet();
                //ds = _AccountPayable_ISERVICE.GetAgingDetailList(CompID, BrId, userid, _AccRecModel.supp_id, _AccRecModel.category, _AccRecModel.portFolio, _AccRecModel.Basis, _AccRecModel.To_dt, 0, "List", 0, _AccRecModel.PayableType,_AccRecModel.ReportType,_AccRecModel.hdnbr_ids);
                //DataTable dtItem = new DataTable();
                //if (_AccRecModel.PayableType == "A")
                //{
                //    ViewBag.PaybleType = "All";
                //}
                //else if (_AccRecModel.PayableType == "O")
                //{
                //    ViewBag.PaybleType = " Overdue";
                //}
                //else
                //{
                //    ViewBag.PaybleType = " Upcoming Due ";
                //}
                //ViewBag.ReportType = _AccRecModel.ReportType;
             
                Details = ds.Tables[0];
                //Details = ToDataTable(_AccRecModel.PrintPDFData,"PDF");
                //ViewBag.RangeList = _AccRecModel.RangeList;
                DataView data = new DataView();
                data = Details.DefaultView;
                data.Sort = "suppname";
                //data.Sort = "supp_name";
                Details = data.ToTable();
                DataTable dt = new DataTable();
                //dt = data.ToTable(true, "supp_name");
                dt = data.ToTable(true, "suppname");

                //DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, Br_ID);
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, BrId);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                //ViewBag.CompLogoDtl = dtlogo;
                ViewBag.Details = Details;
                //ViewBag.TotalDetails = ds.Tables[1];
                ViewBag.DocName = "Account Payable";
                //ViewBag.basis = _AccRecModel.hdnBasis;
                //ViewBag.AsonDate = _AccRecModel.hdnAsonDate;
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
        private void CommonPageDetails()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrId = Session["BranchId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                userid = Session["UserId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            ViewBag.DocumentMenuId = "105103190125";
            DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrId, userid, DocumentMenuId, language);
            ViewBag.AppLevel = ds.Tables[0];
            ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
            string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
            ViewBag.VBRoleList = ds.Tables[3];
            ViewBag.StatusList = ds.Tables[4];
            ViewBag.PackSerialization = ds.Tables[6].Rows.Count > 0 ? ds.Tables[6].Rows[0]["param_stat"].ToString() : "";
            string[] Docpart = DocumentName.Split('>');
            int len = Docpart.Length;
            if (len > 1)
            {
                title = Docpart[len - 1].Trim();
            }
            ViewBag.MenuPageName = DocumentName;
        }
        private List<RegionList> regionLists()
        {
            List<RegionList> regionLists = new List<RegionList>();
            DataTable dt = GetRegion();
            foreach (DataRow dr in dt.Rows)
            {
                RegionList list = new RegionList();
                list.region_id = dr["setup_id"].ToString();
                list.region_val = dr["setup_val"].ToString();
                regionLists.Add(list);
            }
            // regionLists.Insert(0, new RegionList() { region_id = "0", region_val = "---All---" });
            return regionLists;
        }
        private List<CustCategoryList> custCategoryList()
        {
            List<CustCategoryList> lists = new List<CustCategoryList>();
            DataTable dt = GetCustomerCategory();
            foreach (DataRow dr in dt.Rows)
            {
                CustCategoryList list = new CustCategoryList();
                list.Cat_id = dr["setup_id"].ToString();
                list.Cat_val = dr["setup_val"].ToString();
                lists.Add(list);
            }
            //lists.Insert(0, new CustCategoryList() { Cat_id = "0", Cat_val = "---All---" });
            return lists;
        }
        private List<CustPortFolioList> custPortFolioLists()
        {
            List<CustPortFolioList> portFolioLists = new List<CustPortFolioList>();
            DataTable dt1 = GetCustomerPortfolio();
            foreach (DataRow dr in dt1.Rows)
            {
                CustPortFolioList custPortFolio = new CustPortFolioList();
                custPortFolio.CatPort_id = dr["setup_id"].ToString();
                custPortFolio.CatPort_val = dr["setup_val"].ToString();
                portFolioLists.Add(custPortFolio);
            }
            // portFolioLists.Insert(0, new CustPortFolioList() { CatPort_id = "0", CatPort_val = "---All---" });
            return portFolioLists;
        }
        private List<SalesPersList> GetSalesPersonList()
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                    userid = Session["UserId"].ToString();
                DataTable dt = _MISCollectionDetail_IService.GetSalesPersonList(CompID, BrId, userid = Session["UserId"].ToString());
                List<SalesPersList> slsperslist = new List<SalesPersList>();
                foreach (DataRow dr in dt.Rows)
                {
                    SalesPersList slspers = new SalesPersList
                    {
                        sls_pers_id = dr["sls_pers_id"].ToString(),
                        sls_pers_name = dr["sls_pers_name"].ToString()
                    };
                    slsperslist.Add(slspers);
                }
                //slsperslist.Insert(0, new SalesPersList { sls_pers_id = "0", sls_pers_name = "---All---" });
                return slsperslist;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DataTable GetRegion()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _MISCollectionDetail_IService.GetRegionDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable GetCustomerCategory()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _MISCollectionDetail_IService.GetcategoryDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable GetCustomerPortfolio()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _MISCollectionDetail_IService.GetCustportDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private void GetCustomerDropdown(MISCollectionDetail_Model model)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            DataSet datalist = _MISCollectionDetail_IService.GetCustomerDropdowns(CompID, "0", "0");

            List<customerZoneList> _SuppList = new List<customerZoneList>();
            foreach (DataRow data in datalist.Tables[0].Rows)
            {
                customerZoneList _SuppDetail = new customerZoneList();
                _SuppDetail.cust_zone_id = data["setup_id"].ToString();
                _SuppDetail.cust_zone_name = data["setup_val"].ToString();
                _SuppList.Add(_SuppDetail);
            }
            model.customerZoneLists = _SuppList;

            List<CustomerGroupList> _CustomerGroupList = new List<CustomerGroupList>();
            foreach (DataRow data in datalist.Tables[1].Rows)
            {
                CustomerGroupList _SuppDetail = new CustomerGroupList();
                _SuppDetail.cust_grp_id = data["setup_id"].ToString();
                _SuppDetail.cust_grp_name = data["setup_val"].ToString();
                _CustomerGroupList.Add(_SuppDetail);
            }
            model.CustomerGroupLists = _CustomerGroupList;

            List<CityList> _CityList = new List<CityList>();
            model.CityLists = _CityList;

            List<StateList> _StateList = new List<StateList>();
            model.StateLists = _StateList;
        }
        public ActionResult BindStateListData(MISCollectionDetail_Model model)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                    {
                        BrId = Session["BranchId"].ToString();
                    }
                    if (model.SearchState == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = model.SearchState;
                    }
                    DataSet ProductList = _MISCollectionDetail_IService.BindStateListData(CompID, BrId, SarchValue);
                    if (ProductList.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[2].Rows.Count; i++)
                        {
                            string state_id = ProductList.Tables[2].Rows[i]["state_id"].ToString();
                            string state_name = ProductList.Tables[2].Rows[i]["state_name"].ToString();
                            string country_name = ProductList.Tables[2].Rows[i]["country_name"].ToString();
                            ItemList.Add(state_id + ',' + country_name, state_name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindCityListdata(MISCollectionDetail_Model model)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                    {
                        BrId = Session["BranchId"].ToString();
                    }
                    if (model.SearchCity == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = model.SearchCity;
                    }
                    DataSet ProductList = _MISCollectionDetail_IService.BindCityListdata(CompID, BrId, SarchValue, model.state_id);
                    if (ProductList.Tables[3].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[3].Rows.Count; i++)
                        {
                            string city_id = ProductList.Tables[3].Rows[i]["city_id"].ToString();
                            string city_name = ProductList.Tables[3].Rows[i]["city_name"].ToString();
                            string state_name = ProductList.Tables[3].Rows[i]["state_name"].ToString();
                            string country_name = ProductList.Tables[3].Rows[i]["country_name"].ToString();
                            ItemList.Add(city_id + ',' + state_name + ',' + country_name, city_name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
    }

}