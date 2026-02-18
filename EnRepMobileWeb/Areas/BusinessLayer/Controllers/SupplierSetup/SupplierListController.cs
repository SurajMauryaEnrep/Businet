using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.DASHBOARD;
using EnRepMobileWeb.SERVICES.SERVICES;
using System.Data;
using Newtonsoft.Json;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.BusinessLayer.SupplierDetail;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.IO;
using OfficeOpenXml;
using System.Configuration;
using System.Data.OleDb;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class SupplierListController : Controller
    {
        DataTable SupplierListDs;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string CompID, UserID, BrchID, language = String.Empty;
        string DocumentMenuId = "103135", title, supp_id;
        Common_IServices _Common_IServices;
        SupplierList_ISERVICES _SupplierList_ISERVICES;
        SupplierDetail_ISERVICES _SupplierDetail_ISERVICES;
        CustomerSetup_ISERVICES _CustomerSetup_ISERVICES;
        public SupplierListController(Common_IServices _Common_IServices, SupplierList_ISERVICES _SupplierList_ISERVICES, SupplierDetail_ISERVICES _SupplierDetail_ISERVICES, CustomerSetup_ISERVICES _CustomerSetup_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._SupplierList_ISERVICES = _SupplierList_ISERVICES;
            this._SupplierDetail_ISERVICES = _SupplierDetail_ISERVICES;
            this._CustomerSetup_ISERVICES = _CustomerSetup_ISERVICES;
        }
        // GET: BusinessLayer/SupplierList
        public ActionResult SupplierList()
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                SupplierDetail _SupplierDetail = new SupplierDetail();
                _SupplierDetail.Title = title;
                string Comp_ID = string.Empty;
                string Language = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }
                //        public class Supplier
                //{
                //    public string SuppID { get; set; }
                //    public string SuppName { get; set; }

                //}


                GetAllDropdownlist(_SupplierDetail);

                //SupplierListDs = new DataTable();
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    if (PRData != "" && PRData != null)
                    {
                        var a = PRData.Split(',');
                        var SuppID = a[0].Trim();
                        var Supptype = a[1].Trim();
                        var Suppcat = a[2].Trim();
                        var Suppport = a[3].Trim();
                        var SuppAct = a[4].Trim();
                        var SuppStatus = a[5].Trim();
                        var gl_rpt_id = a[6].Trim();
                        var gl_rpt_name = a[7].Trim();
                        if (SuppStatus == "0")
                        {
                            SuppStatus = null;
                        }
                        DataTable HoCompData = _SupplierList_ISERVICES.GetSupplierListFilterDAL(Comp_ID, SuppID, Supptype, Suppcat, Suppport, SuppAct, SuppStatus, gl_rpt_id).Tables[0];
                        ViewBag.VBSupplierlist = HoCompData;
                        _SupplierDetail.SupplName = SuppID;
                        _SupplierDetail.typ = Supptype;
                        _SupplierDetail.suppcatg = Suppcat;
                        _SupplierDetail.suppport = Suppport;
                        _SupplierDetail.item_ActStatus = SuppAct;
                        _SupplierDetail.Status = SuppStatus;
                        if (gl_rpt_id != "" && gl_rpt_id != "0" && gl_rpt_id != null)
                        {
                            List<GlReportingGroup> _ItemList = new List<GlReportingGroup>();

                            GlReportingGroup Glrpt = new GlReportingGroup();
                            Glrpt.Gl_rpt_id = gl_rpt_id;
                            Glrpt.Gl_rpt_Name = gl_rpt_name;
                            _ItemList.Add(Glrpt);

                            _SupplierDetail.GlReportingGroupList = _ItemList;
                        }
                        _SupplierDetail.ListFilterData = TempData["ListFilterData"].ToString();
                    }
                    //else
                    //{
                    //    DataSet ds = _SupplierList_ISERVICES.GetSuppListDAL(supp_id, Comp_ID);
                    //    ViewBag.VBSupplierlist = ds.Tables[0];
                    //    ViewBag.AttechmentDetails = ds.Tables[1];
                    //    _SupplierDetail.SSearch = "0";
                    //}

                }
                //else
                //{

                // }


                //ViewBag.VBSupplierlist = SupplierListDs;
                ViewBag.VBRoleList = GetRoleList();
                GetStatusList(_SupplierDetail);
                //Session["SSearch"] = "0";
                _SupplierDetail.DocumentMenuId = DocumentMenuId;
                TempData["ProspectDetail"] = null;
                return View("~/Areas/BusinessLayer/Views/SupplierSetup/SupplierList.cshtml", _SupplierDetail);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllDropdownlist(SupplierDetail _SupplierDetail)
        {
            string Comp_ID = string.Empty;
            string Language = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                Language = Session["Language"].ToString();
            }
            string GroupName = string.Empty;
            if (string.IsNullOrEmpty(_SupplierDetail.SupplName))
            {
                GroupName = "0";
            }
            else
            {
                GroupName = _SupplierDetail.SupplName;
            }
            DataSet dt = _SupplierList_ISERVICES.GetAllDropdown(GroupName, Comp_ID, supp_id);
            List<Supplier> _SupplierList = new List<Supplier>();
            //DataTable dt = GetSupplierList(_SupplierDetail);
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                Supplier _supplier = new Supplier();
                _supplier.SuppID = dr["supp_id"].ToString();
                _supplier.SuppName = dr["supp_name"].ToString();
                _SupplierList.Add(_supplier);
            }
            _SupplierList.Insert(0, new Supplier() { SuppID = "0", SuppName = "All" });
            _SupplierDetail.SupplierList = _SupplierList;

            List<SuppCategory> _CategoryList = new List<SuppCategory>();
            // DataTable dt1 = Getcategory();
            foreach (DataRow dr in dt.Tables[1].Rows)
            {
                SuppCategory _Category = new SuppCategory();
                _Category.setup_id = dr["setup_id"].ToString();
                _Category.setup_val = dr["setup_val"].ToString();
                _CategoryList.Add(_Category);
            }
            _CategoryList.Insert(0, new SuppCategory() { setup_id = "-1", setup_val = "All" });
            _SupplierDetail.CategoryList = _CategoryList;

            List<SuppPortFolio> _PortFolioList = new List<SuppPortFolio>();
            // DataTable dt3 = GetSuppport();
            foreach (DataRow dr in dt.Tables[2].Rows)
            {
                SuppPortFolio _PortFolio = new SuppPortFolio();
                _PortFolio.setup_id = dr["setup_id"].ToString();
                _PortFolio.setup_val = dr["setup_val"].ToString();
                _PortFolioList.Add(_PortFolio);
            }
            _PortFolioList.Insert(0, new SuppPortFolio() { setup_id = "-1", setup_val = "All" });
            _SupplierDetail.PortFolioList = _PortFolioList;

            // DataSet ds = _SupplierList_ISERVICES.GetSuppListDAL(supp_id, Comp_ID);
            ViewBag.VBSupplierlist = dt.Tables[3];
            ViewBag.AttechmentDetails = dt.Tables[4];
            _SupplierDetail.SSearch = "0";

            List<GlReportingGroup> _ItemList = new List<GlReportingGroup>();

            GlReportingGroup Glrpt = new GlReportingGroup();
            Glrpt.Gl_rpt_id = "0";
            Glrpt.Gl_rpt_Name = "All";
            _ItemList.Add(Glrpt);
            _SupplierDetail.GlReportingGroupList = _ItemList;
        }
        [HttpPost]
        public JsonResult GetGlReportingGrp(SupplierDetail _SupplierDetail)
        {
            JsonResult DataRows = null;
            try
            {

                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string gl_repoting = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_SupplierDetail.GlRepoting_Group))
                {
                    gl_repoting = "0";
                }
                else
                {
                    gl_repoting = _SupplierDetail.GlRepoting_Group;
                }
                DataTable GlReporting = _SupplierList_ISERVICES.GetGlReportingGrp(Comp_ID, Br_ID, gl_repoting);
                //List<GlReportingGroup> _ItemList = new List<GlReportingGroup>();
                //foreach (DataRow data in GlReporting.Rows)
                //{
                //    GlReportingGroup Glrpt = new GlReportingGroup();
                //    Glrpt.Gl_rpt_id = data["ID"].ToString();
                //    Glrpt.Gl_rpt_Name = data["Val"].ToString();
                //    _ItemList.Add(Glrpt);
                //}
                //_CustomerDetails.GlReportingGroupList = _ItemList;
                return Json(JsonConvert.SerializeObject(GlReporting), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;//Json("ErrorPage");
            }

        }
        public void GetStatusList(SupplierDetail _SupplierDetail)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _SupplierDetail.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult GetSupplierListFilter(string SuppID, string Supptype, string Suppcat,
            string Suppport, string SuppAct, string SuppStatus, string Glrtp_id)
        {
            SupplierDetail _SupplierDetail = new SupplierDetail();
            ViewBag.VBSupplierList = null;
            string Comp_ID = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            try
            {

                DataTable HoCompData = _SupplierList_ISERVICES.GetSupplierListFilterDAL(Comp_ID, SuppID, Supptype, Suppcat, Suppport, SuppAct, SuppStatus, Glrtp_id).Tables[0];
                ViewBag.VBSupplierlist = HoCompData;
                //Session["SSearch"] = "S_Search";
                _SupplierDetail.SSearch = "S_Search";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialSupplierList.cshtml", _SupplierDetail);
        }

        //[HttpPost]
        //public JsonResult Getcategory()
        //{
        //    JsonResult DataRows = null;
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        DataSet HoCompData = _SupplierList_ISERVICES.GetcategoryDAL(Comp_ID);
        //        DataRows = Json(JsonConvert.SerializeObject(HoCompData));/*Result convert into Json Format for javasript*/
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //    return DataRows;
        //}
        //[HttpPost]
        //public JsonResult Getsuppport()
        //{
        //    JsonResult DataRows = null;
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        DataSet HoCompData = _SupplierList_ISERVICES.GetsuppportDAL(Comp_ID);
        //        DataRows = Json(JsonConvert.SerializeObject(HoCompData));/*Result convert into Json Format for javasript*/
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //    return DataRows;
        //}
        [NonAction]
        private DataTable GetSupplierList(SupplierDetail _SupplierDetail)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string GroupName = string.Empty;
                if (string.IsNullOrEmpty(_SupplierDetail.SupplName))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _SupplierDetail.SupplName;
                }
                DataTable dt = _SupplierList_ISERVICES.Getsuppplier(GroupName, Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [NonAction]
        private DataTable Getcategory()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _SupplierList_ISERVICES.GetcategoryDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetSuppport()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _SupplierList_ISERVICES.GetsuppportDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        //public ActionResult GetAutoCompleteSuppList(SearchSupp queryParameters)
        //{
        //    string Comp_ID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }
        //    string GroupName = string.Empty;
        //    //string ErrorMessage = "success";
        //    Dictionary<string, string> SuppList = new Dictionary<string, string>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(queryParameters.ddlGroup))
        //        {
        //            GroupName = "0";
        //        }
        //        else
        //        {
        //            GroupName = queryParameters.ddlGroup;
        //        }
        //        SuppList = _SupplierList_ISERVICES.SuppSetupGroupDAL(GroupName, Comp_ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ErrorPage()
        {
            try
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult AddNewSupplier()
        {
            SupplierDetail _SupplierDetail = new SupplierDetail();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";

            _SupplierDetail.Message = "New";
            _SupplierDetail.Command = "Add";
            _SupplierDetail.AppStatus = "D";
            _SupplierDetail.TransType = "Save";
            _SupplierDetail.BtnName = "BtnAddNew";
            TempData["ModelData"] = _SupplierDetail;
            TempData["ListFilterData"] = null;

            return RedirectToAction("SupplierDetail", "SupplierDetail");
        }
        public ActionResult EditSupplier(string SuppId, string act_status, string on_hold, string ListFilterData)
        {
            try
            {
                SupplierDetail _SupplierDetail = new SupplierDetail();
                //Session["Message"] = "New";
                //Session["Command"] = "Add";
                //Session["SuppCode"] = SuppId;
                //Session["TransType"] = "Update";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnToDetailPage";

                _SupplierDetail.Message = "New";
                _SupplierDetail.Command = "Add";
                _SupplierDetail.SuppCode = SuppId;
                _SupplierDetail.TransType = "Update";
                //_SupplierDetail.AppStatus = "D";
                _SupplierDetail.BtnName = "BtnToDetailPage";
                if (act_status != "N")
                {
                    _SupplierDetail.act_status = true;
                }
                else
                {
                    _SupplierDetail.act_status = false;
                }
                if (on_hold != "N")
                {
                    _SupplierDetail.on_hold = true;
                }
                else
                {
                    _SupplierDetail.on_hold = false;
                }
                var SuppCodeURL = SuppId;
                var TransType = "Update";
                var BtnName = "BtnToDetailPage";
                var command = "Add";
                TempData["ModelData"] = _SupplierDetail;
                TempData["ListFilterData"] = ListFilterData;
                return (RedirectToAction("SupplierDetail", "SupplierDetail", new { SuppCodeURL = SuppCodeURL, TransType, BtnName, command }));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult DownloadFile()
        {
            try
            {
                string compId = "0";string Br_Id = string.Empty;
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                {
                    Br_Id = Session["BranchId"].ToString();
                }
                CommonController com_obj = new CommonController();
                DataTable SupplierDetail = new DataTable();
                DataTable Address = new DataTable();
                DataTable BrDetail = new DataTable();
                DataSet obj_ds = new DataSet();
                SupplierDetail.Columns.Add("Supplier Type*", typeof(string));
                SupplierDetail.Columns.Add("TDS Applicable*", typeof(string));
                SupplierDetail.Columns.Add("Applicable On*", typeof(string));
                SupplierDetail.Columns.Add("Supplier Name*(max 100 characters)", typeof(string));
                SupplierDetail.Columns.Add("Category*", typeof(string));
                SupplierDetail.Columns.Add("Portfolio*", typeof(string));
                SupplierDetail.Columns.Add("Payment Terms(In Days)(Numeric only)", typeof(string));
                SupplierDetail.Columns.Add("Payment Alert", typeof(string));
                SupplierDetail.Columns.Add("GL Account Name*(max 100 characters)", typeof(string));
                SupplierDetail.Columns.Add("GL Account Group*", typeof(string));
                SupplierDetail.Columns.Add("GL Reporting Group", typeof(string));
                SupplierDetail.Columns.Add("Default Currency*", typeof(string));
                SupplierDetail.Columns.Add("GST Category*", typeof(string));
                SupplierDetail.Columns.Add("Registration Number(max 50 characters)", typeof(string));
                SupplierDetail.Columns.Add("TAN Number(max 50 characters)", typeof(string));
                SupplierDetail.Columns.Add("PAN number(max 50 characters)", typeof(string));
                SupplierDetail.Columns.Add("Contact Person(max 50 characters)", typeof(string));
                SupplierDetail.Columns.Add("Contact Email ID(max 50 characters)", typeof(string));
                SupplierDetail.Columns.Add("Contact Number(max 50 characters)", typeof(string));
                SupplierDetail.Columns.Add("Bank Name(max 50 characters)", typeof(string));
                SupplierDetail.Columns.Add("Bank Branch(max 50 characters)", typeof(string));
                SupplierDetail.Columns.Add("Address(max 100 characters)", typeof(string));
                SupplierDetail.Columns.Add("Account Number(max 50 characters)", typeof(string));
                SupplierDetail.Columns.Add("IFSC Code(max 25 characters)", typeof(string));
                SupplierDetail.Columns.Add("SWIFT Code(max 25 characters)", typeof(string));

                Address.Columns.Add("Supplier Name*", typeof(string));
                Address.Columns.Add("Address*(max 200 characters)", typeof(string));
                Address.Columns.Add("Country*", typeof(string));
                Address.Columns.Add("State*", typeof(string));
                Address.Columns.Add("District*", typeof(string));
                Address.Columns.Add("City*", typeof(string));
                Address.Columns.Add("Pin(max 15 characters)", typeof(string));
                Address.Columns.Add("GST Number*(max 15 characters)", typeof(string));
                Address.Columns.Add("Billing Address*", typeof(string));

                BrDetail.Columns.Add("Supplier Name", typeof(string));
                BrDetail.Columns.Add("Branch Name", typeof(string));
                BrDetail.Columns.Add("Active", typeof(string));

                obj_ds.Tables.Add(SupplierDetail);
                obj_ds.Tables.Add(Address);
                obj_ds.Tables.Add(BrDetail);

                DataSet ds = _SupplierList_ISERVICES.GetMasterDropDownList(compId, Br_Id);
                string filePath = com_obj.CreateExcelFile("ImportSupplierTemplate", Server);
                com_obj.AppendExcel(filePath, ds, obj_ds, "SupplierSetup");
                string fileName = Path.GetFileName(filePath);
                return File(filePath, "application/octet-stream", fileName);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ValidateExcelFile(string uploadStatus)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            conString = "Invalid File";
                            break;
                    }
                    if (conString == "Invalid File")
                        return Json("Invalid File. Please upload a valid file", JsonRequestBehavior.AllowGet);
                    DataSet ds = new DataSet();
                    DataTable SupplierDetail = new DataTable();
                    DataTable SupplierAddress = new DataTable();
                    DataTable SupplierBranch = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();

                                string SupplierDetailQuery = "SELECT DISTINCT * FROM [SupplierDetail$] WHERE LEN([Supplier Name*(max 100 characters)]) > 0;";
                                string SupplierAddressQuery = "SELECT DISTINCT * From [Address$]; ";
                                string SupplierBranchQuery = "SELECT DISTINCT * From [BranchMapping$] ; ";

                                connExcel.Open();
                                cmdExcel.CommandText = SupplierDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SupplierDetail);

                                cmdExcel.CommandText = SupplierAddressQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SupplierAddress);

                                cmdExcel.CommandText = SupplierBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SupplierBranch);
                                connExcel.Close();

                                ds.Tables.Add(SupplierDetail);
                                ds.Tables.Add(SupplierBranch);
                                ds.Tables.Add(SupplierAddress);
                                DataSet dts = VerifyData(ds, uploadStatus);
                                if (dts == null)
                                    return Json("Excel file is empty. Please fill data in excel file and try again");
                                ViewBag.ImportItemsPreview = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialImportSupplierDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet VerifyData(DataSet ds_supplier, string uploadStatus)
        {
            string compId = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            DataSet dts = PrepareDataset(ds_supplier);
            if (ds_supplier.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(ds_supplier.Tables[0].Rows[0].ToString()))
            {
                DataSet result = _SupplierList_ISERVICES.GetVerifiedDataOfExcel(compId, dts.Tables[0], dts.Tables[1], dts.Tables[2]);
                if (uploadStatus.Trim() == "0")
                    return result;

                var filteredRows = result.Tables[0].AsEnumerable().Where(x => x.Field<string>("UploadStatus").ToUpper() == uploadStatus.ToUpper()).ToList();
                DataTable newDataTable = filteredRows.Any() ? filteredRows.CopyToDataTable() : result.Tables[0].Clone();
                result.Tables[0].Clear();

                for (int i = 0; i < newDataTable.Rows.Count; i++)
                {
                    result.Tables[0].ImportRow(newDataTable.Rows[i]);
                }
                result.Tables[0].AcceptChanges();
                return result;
            }
            else
            {
                return null;
            }
        }
        public DataSet PrepareDataset(DataSet ds_supplier)
        {
            DataTable SupplierDetail = new DataTable();
            DataTable SupplierBranch = new DataTable();
            DataTable SupplierAddress = new DataTable();
            SupplierDetail.Columns.Add("supp_type", typeof(string));
            SupplierDetail.Columns.Add("tds_posting", typeof(string));
            SupplierDetail.Columns.Add("tds_app_on", typeof(string));
            SupplierDetail.Columns.Add("supp_name", typeof(string));
            SupplierDetail.Columns.Add("supp_catg", typeof(string));
            SupplierDetail.Columns.Add("supp_port", typeof(string));
            SupplierDetail.Columns.Add("paym_term", typeof(string));
            SupplierDetail.Columns.Add("paym_alrt", typeof(string));
            SupplierDetail.Columns.Add("acc_name", typeof(string));
            SupplierDetail.Columns.Add("acc_grp_id", typeof(string));
            SupplierDetail.Columns.Add("gl_rpt_id", typeof(string));
            SupplierDetail.Columns.Add("def_curr_id", typeof(string));
            SupplierDetail.Columns.Add("gst_cat", typeof(string));
            SupplierDetail.Columns.Add("supp_regn_no", typeof(string));
            SupplierDetail.Columns.Add("supp_tan_no", typeof(string));
            SupplierDetail.Columns.Add("supp_pan_no", typeof(string));
            SupplierDetail.Columns.Add("cont_pers", typeof(string));
            SupplierDetail.Columns.Add("cont_email", typeof(string));
            SupplierDetail.Columns.Add("cont_num1", typeof(string));
            SupplierDetail.Columns.Add("bank_name", typeof(string));
            SupplierDetail.Columns.Add("bank_branch", typeof(string));
            SupplierDetail.Columns.Add("bank_add", typeof(string));
            SupplierDetail.Columns.Add("bank_acc_no", typeof(string));
            SupplierDetail.Columns.Add("ifsc_code", typeof(string));
            SupplierDetail.Columns.Add("swift_code", typeof(string));
            for (int i = 0; i < ds_supplier.Tables[0].Rows.Count; i++)
            {
                DataTable dtsupplierdetail = ds_supplier.Tables[0];
                DataRow dtr = SupplierDetail.NewRow();
                if (dtsupplierdetail.Rows[i][0].ToString() == "")
                {
                    dtr["supp_type"] = dtsupplierdetail.Rows[i][0].ToString().Trim();
                }
                else
                {
                    dtr["supp_type"] = dtsupplierdetail.Rows[i][0].ToString().Trim().Substring(0, 1);
                }
                if (dtsupplierdetail.Rows[i][1].ToString() == "")
                {
                    dtr["tds_posting"] = "N";
                }
                else
                {
                    dtr["tds_posting"] = dtsupplierdetail.Rows[i][1].ToString().Trim().Substring(0, 1);
                }
                if (!string.IsNullOrEmpty(dtsupplierdetail.Rows[i][2].ToString()))
                {
                    dtr["tds_app_on"] = dtsupplierdetail.Rows[i][2].ToString().Trim().Substring(0, 1);
                }
                else
                {
                    dtr["tds_app_on"] = "";
                }
                dtr["supp_name"] = dtsupplierdetail.Rows[i][3].ToString().Trim();
                dtr["supp_catg"] = dtsupplierdetail.Rows[i][4].ToString().Trim();
                dtr["supp_port"] = dtsupplierdetail.Rows[i][5].ToString().Trim();
                int paymterm = 0;
                int.TryParse(dtsupplierdetail.Rows[i][6].ToString().Trim(), out paymterm);
                dtr["paym_term"] = paymterm.ToString();
                int paymalrt = 0;
                int.TryParse(dtsupplierdetail.Rows[i][7].ToString().Trim(), out paymalrt);
                dtr["paym_alrt"] = paymalrt.ToString();
                dtr["acc_name"] = dtsupplierdetail.Rows[i][8].ToString().Trim();
                dtr["acc_grp_id"] = dtsupplierdetail.Rows[i][9].ToString().Trim();
                dtr["gl_rpt_id"] = dtsupplierdetail.Rows[i][10].ToString().Trim();
                dtr["def_curr_id"] = dtsupplierdetail.Rows[i][11].ToString().Trim();
                if (dtsupplierdetail.Rows[i][12].ToString() == "")
                {
                    dtr["gst_cat"] = dtsupplierdetail.Rows[i][12].ToString().Trim();
                }
                else
                {
                    if (dtsupplierdetail.Rows[i][12].ToString() == "Registered")
                    {
                        dtr["gst_cat"] = "RR";
                    }
                    else if (dtsupplierdetail.Rows[i][12].ToString() == "Un-Registered")
                    {
                        dtr["gst_cat"] = "UR";
                    }
                    else
                    {
                        dtr["gst_cat"] = dtsupplierdetail.Rows[i][12].ToString().Trim().Substring(0, 2).ToUpper();
                    }
                }
                dtr["supp_regn_no"] = dtsupplierdetail.Rows[i][13].ToString().Trim();
                dtr["supp_tan_no"] = dtsupplierdetail.Rows[i][14].ToString().Trim();
                dtr["supp_pan_no"] = dtsupplierdetail.Rows[i][15].ToString().Trim();
                dtr["cont_pers"] = dtsupplierdetail.Rows[i][16].ToString().Trim();
                dtr["cont_email"] = dtsupplierdetail.Rows[i][17].ToString().Trim();
                dtr["cont_num1"] = dtsupplierdetail.Rows[i][18].ToString().Trim();
                dtr["bank_name"] = dtsupplierdetail.Rows[i][19].ToString().Trim();
                dtr["bank_branch"] = dtsupplierdetail.Rows[i][20].ToString().Trim();
                dtr["bank_add"] = dtsupplierdetail.Rows[i][21].ToString().Trim();
                dtr["bank_acc_no"] = dtsupplierdetail.Rows[i][22].ToString().Trim();
                dtr["ifsc_code"] = dtsupplierdetail.Rows[i][23].ToString().Trim();
                dtr["swift_code"] = dtsupplierdetail.Rows[i][24].ToString().Trim();
                SupplierDetail.Rows.Add(dtr);
            }
            //-------------------------------Branch Detail-------------------------
            SupplierBranch.Columns.Add("supp_name", typeof(string));
            SupplierBranch.Columns.Add("branch_name", typeof(string));
            SupplierBranch.Columns.Add("act_status", typeof(string));
            for (int i = 0; i < ds_supplier.Tables[1].Rows.Count; i++)
            {
                DataTable dtbranchdetail = ds_supplier.Tables[1];
                DataRow dtr = SupplierBranch.NewRow();
                dtr["supp_name"] = dtbranchdetail.Rows[i][0].ToString().Trim();
                dtr["branch_name"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                if (!string.IsNullOrEmpty(dtbranchdetail.Rows[i][2].ToString()))
                {
                    dtr["act_status"] = dtbranchdetail.Rows[i][2].ToString().Trim().Substring(0, 1);
                }
                else
                {
                    dtr["act_status"] = "N";
                }
                SupplierBranch.Rows.Add(dtr);
            }
            //---------------------------End-------------------------------------
            //-----------------------------Address-------------------------------
            SupplierAddress.Columns.Add("supp_name", typeof(string));
            SupplierAddress.Columns.Add("supp_add", typeof(string));
            SupplierAddress.Columns.Add("supp_cntry", typeof(string));
            SupplierAddress.Columns.Add("supp_state", typeof(string));
            SupplierAddress.Columns.Add("supp_dist", typeof(string));
            SupplierAddress.Columns.Add("supp_city", typeof(string));
            SupplierAddress.Columns.Add("supp_pin", typeof(string));
            SupplierAddress.Columns.Add("supp_gst_no", typeof(string));
            SupplierAddress.Columns.Add("def_bill_add", typeof(string));
            for (int i = 0; i < ds_supplier.Tables[2].Rows.Count; i++)
            {

                DataTable dtaddress = ds_supplier.Tables[2];
                DataRow dtr = SupplierAddress.NewRow();
                dtr["supp_name"] = dtaddress.Rows[i][0].ToString().Trim();
                dtr["supp_add"] = dtaddress.Rows[i][1].ToString().Trim();
                dtr["supp_cntry"] = dtaddress.Rows[i][2].ToString().Trim();
                dtr["supp_state"] = dtaddress.Rows[i][3].ToString().Trim();
                dtr["supp_dist"] = dtaddress.Rows[i][4].ToString().Trim();
                dtr["supp_city"] = dtaddress.Rows[i][5].ToString().Trim();
                dtr["supp_pin"] = dtaddress.Rows[i][6].ToString().Trim();
                dtr["supp_gst_no"] = dtaddress.Rows[i][7].ToString().Trim();
                if (dtaddress.Rows[i][8] == null || dtaddress.Rows[i][8].ToString().Trim() == "")
                {
                    dtr["def_bill_add"] = "";
                }
                else
                {
                    dtr["def_bill_add"] = dtaddress.Rows[i][8].ToString().Trim().Substring(0, 1);
                }
                SupplierAddress.Rows.Add(dtr);
            }
            //---------------------------End-------------------------------------
            DataSet dts = new DataSet();
            dts.Tables.Add(SupplierDetail);
            dts.Tables.Add(SupplierBranch);
            dts.Tables.Add(SupplierAddress);
            return dts;
        }
        public ActionResult BindSupplierAddress(string supplierName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();

                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": // Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": // Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            throw new Exception("Invalid file type");
                    }

                    DataTable dtsupplierdetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                //string supplieraddressQuery = "SELECT DISTINCT [Supplier Name*],[Address*_(max 200 characters)],[Country*],[State*],[District*],[City*],[Pin*_(max 15 characters)],[GST Number*_(max 15 characters)] From [Address$] where [Supplier Name*] = '" + supplierName + "' ; ";
                                string supplieraddressQuery = "SELECT [Supplier Name*], [Address*(max 200 characters)], [Country*], [State*], [District*], [City*], [Pin(max 15 characters)], [GST Number*(max 15 characters)], MAX([Billing Address*]) AS [Billing Address*] " +
                              "FROM [Address$] " +
                              "WHERE [Supplier Name*] = '" + supplierName + "' " +
                              "GROUP BY [Supplier Name*], [Address*(max 200 characters)], [Country*], [State*], [District*], [City*], [Pin(max 15 characters)], [GST Number*(max 15 characters)];";
                                cmdExcel.CommandText = supplieraddressQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtsupplierdetail);

                                DataTable SupplierAddress = new DataTable();
                                SupplierAddress.Columns.Add("supp_name", typeof(string));
                                SupplierAddress.Columns.Add("supp_add", typeof(string));
                                SupplierAddress.Columns.Add("supp_cntry", typeof(string));
                                SupplierAddress.Columns.Add("supp_state", typeof(string));
                                SupplierAddress.Columns.Add("supp_dist", typeof(string));
                                SupplierAddress.Columns.Add("supp_city", typeof(string));
                                SupplierAddress.Columns.Add("supp_pin", typeof(string));
                                SupplierAddress.Columns.Add("supp_gst_no", typeof(string));
                                SupplierAddress.Columns.Add("def_bill_add", typeof(string));

                                for (int i = 0; i < dtsupplierdetail.Rows.Count; i++)
                                {
                                    DataTable dtaddress = dtsupplierdetail;
                                    DataRow dtr = SupplierAddress.NewRow();
                                    dtr["supp_name"] = dtaddress.Rows[i][0].ToString().Trim();
                                    dtr["supp_add"] = dtaddress.Rows[i][1].ToString().Trim();
                                    dtr["supp_cntry"] = dtaddress.Rows[i][2].ToString().Trim();
                                    dtr["supp_state"] = dtaddress.Rows[i][3].ToString().Trim();
                                    dtr["supp_dist"] = dtaddress.Rows[i][4].ToString().Trim();
                                    dtr["supp_city"] = dtaddress.Rows[i][5].ToString().Trim();
                                    dtr["supp_pin"] = dtaddress.Rows[i][6].ToString().Trim();
                                    dtr["supp_gst_no"] = dtaddress.Rows[i][7].ToString().Trim();
                                    dtr["def_bill_add"] = dtaddress.Rows[i][8].ToString().Trim();
                                    SupplierAddress.Rows.Add(dtr);
                                }
                                ViewBag.CustomerAddressDetail = SupplierAddress;
                                ViewBag.SupplierSetup = "Supplier";
                            }
                        }
                    }
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialCustomerAddressDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult BindSupplierBranch(string supplierName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();

                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": // Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": // Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            throw new Exception("Invalid file type");
                    }

                    DataTable dtsupplierdetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string supplierQuery = "SELECT DISTINCT * From [BranchMapping$] where [Supplier Name] = '" + supplierName + "' ; ";
                                cmdExcel.CommandText = supplierQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtsupplierdetail);

                                DataTable SupplierBranch = new DataTable();
                                SupplierBranch.Columns.Add("supp_name", typeof(string));
                                SupplierBranch.Columns.Add("branch_name", typeof(string));
                                SupplierBranch.Columns.Add("act_status", typeof(string));
                                for (int i = 0; i < dtsupplierdetail.Rows.Count; i++)
                                {
                                    DataTable dtbranchdetail = dtsupplierdetail;
                                    DataRow dtr = SupplierBranch.NewRow();
                                    dtr["supp_name"] = dtbranchdetail.Rows[i][0].ToString().Trim();
                                    dtr["branch_name"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                                    dtr["act_status"] = dtbranchdetail.Rows[i][2].ToString().Trim();
                                    SupplierBranch.Rows.Add(dtr);
                                }
                                ViewBag.BranchDetail = SupplierBranch;
                                ViewBag.PageName = "Supplier";
                            }
                        }
                    }
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialBranchMapping.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ShowValidationError(string supplierName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable SupplierDetail = new DataTable();
                    DataTable SupplierBranch = new DataTable();
                    DataTable SupplierAddress = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string SupplierDetailQuery = "";
                                if (string.IsNullOrWhiteSpace(supplierName))
                                {
                                    SupplierDetailQuery = "SELECT DISTINCT * FROM [SupplierDetail$] WHERE [Supplier Name*(max 100 characters)] IS NULL OR LTRIM(RTRIM([Supplier Name*(max 100 characters)])) = '';";
                                }
                                else
                                {
                                    SupplierDetailQuery = "SELECT DISTINCT * FROM [SupplierDetail$] WHERE [Supplier Name*(max 100 characters)] = '" + supplierName + "';";
                                }
                                string SupplierBranchQuery = "SELECT DISTINCT * From [BranchMapping$] where [Supplier Name] = '" + supplierName + "' ; ";
                                string SupplierAddressQuery = "SELECT DISTINCT * From [Address$] where [Supplier Name*] = '" + supplierName + "' ; ";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = SupplierDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SupplierDetail);

                                cmdExcel.CommandText = SupplierBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SupplierBranch);

                                cmdExcel.CommandText = SupplierAddressQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SupplierAddress);
                                connExcel.Close();

                                ds.Tables.Add(SupplierDetail);
                                ds.Tables.Add(SupplierBranch);
                                ds.Tables.Add(SupplierAddress);
                                DataTable dts = VerifySingleData(ds);
                                ViewBag.ErrorDetails = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialExportErrorDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataTable VerifySingleData(DataSet ds)
        {
            string compId = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            DataSet dts = PrepareDataset(ds);
            DataTable result = _SupplierList_ISERVICES.ShowExcelErrorDetail(compId, dts.Tables[0], dts.Tables[1], dts.Tables[2]);
            return result;
        }
        public JsonResult ImportSupplierDetailFromExcel()
        {
            try
            {
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable SupplierDetail = new DataTable();
                    DataTable SupplierBranch = new DataTable();
                    DataTable SupplierAddress = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string SupplierDetailQuery = "SELECT DISTINCT * FROM [SupplierDetail$] WHERE LEN([Supplier Name*(max 100 characters)]) > 0;";
                                string SupplierBranchQuery = "SELECT DISTINCT * From [BranchMapping$] ; ";
                                string SupplierAddressQuery = "SELECT [Supplier Name*], [Address*(max 200 characters)], [Country*], [State*], [District*], [City*], [Pin(max 15 characters)], [GST Number*(max 15 characters)], MAX([Billing Address*]) AS [Billing Address*] " +
                               "FROM [Address$] " +
                               "GROUP BY [Supplier Name*], [Address*(max 200 characters)], [Country*], [State*], [District*], [City*], [Pin(max 15 characters)], [GST Number*(max 15 characters)];";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = SupplierDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SupplierDetail);

                                cmdExcel.CommandText = SupplierBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SupplierBranch);

                                cmdExcel.CommandText = SupplierAddressQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SupplierAddress);
                                connExcel.Close();

                                ds.Tables.Add(SupplierDetail);
                                ds.Tables.Add(SupplierBranch);
                                ds.Tables.Add(SupplierAddress);
                                string msg = SaveSupplierFromExcel(ds);
                                return Json(msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                }
                else
                    return Json("No file selected", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("cannot insert duplicate"))
                    return Json("something went wrong", JsonRequestBehavior.AllowGet);
                else
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private string SaveSupplierFromExcel(DataSet dsSupplier)
        {
            string compId = "";
            string UserID = "";
            string BranchName = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            if (Session["BranchName"] != null)
                BranchName = Session["BranchName"].ToString();
            DataSet dts = PrepareDataset(dsSupplier);
            string result = _SupplierList_ISERVICES.BulkImportSupplierDetail(compId, UserID, BranchName, dts.Tables[0], dts.Tables[1], dts.Tables[2]);
            return result;
        }
        [HttpPost]
        public JsonResult GetLedgerDetail(string Supp_id, string SuppAcc_id, string Curr_id) 
                    
        {
            try
            {
                JsonResult DataRows = null;
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
                DataSet result = _SupplierList_ISERVICES.GetSuppLedgerDtl(Comp_ID, Br_ID, Supp_id, SuppAcc_id, Curr_id);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }
    }
}