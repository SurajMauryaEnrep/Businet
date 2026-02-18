using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.DASHBOARD;
using EnRepMobileWeb.SERVICES.SERVICES;
using System.Data;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerDetails;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using OfficeOpenXml;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Drawing;
//***All Session Remove By Shubham Maurya on 03-01-2023 Using Model to Pass Data***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    
    public class CustomerSetupController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "103125",title,cust_id; 
        Common_IServices _Common_IServices;
        DataTable CustomerListDs;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();

        CustomerSetup_ISERVICES _CustomerSetup_ISERVICES;
        CustomerDetails_ISERVICES _CustomerDetails_ISERVICES;
        private object _GLList_ISERVICES;

        public CustomerSetupController(Common_IServices _Common_IServices,CustomerSetup_ISERVICES _CustomerSetup_ISERVICES, CustomerDetails_ISERVICES _CustomerDetails_ISERVICES)
        {
            this._CustomerSetup_ISERVICES = _CustomerSetup_ISERVICES;
            this._Common_IServices = _Common_IServices;
            this._CustomerDetails_ISERVICES = _CustomerDetails_ISERVICES;
    }
        // GET: BusinessLayer/CustomerSetup
        
        
        public ActionResult CustomerList()
        {
            try
            {
                CustomerDetails _CustomerDetails = new CustomerDetails();
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                _CustomerDetails.AttachMentDetailItmStp = null;
                _CustomerDetails.Guid = null;     
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _CustomerDetails.Title = title;
                GetStatusList(_CustomerDetails);
                DASHBOARD_MODEL _DASHBOARD_MODEL = new DASHBOARD_MODEL();
                string Comp_ID = string.Empty;
                string Language = string.Empty;
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                GetAllDropDownList(_CustomerDetails);
                //GetAutoCompleteCustList(SearchSupp queryParameters)
                //string GroupName = string.Empty;
                ////string ErrorMessage = "success";
                //Dictionary<string, string> SuppList = new Dictionary<string, string>();
                //if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                //{
                //    GroupName = "0";
                //}
                //else
                //{
                //    GroupName = queryParameters.ddlGroup;
                //}


                //CustomerListDs = new DataTable();
                //CustomerListDs = _CustomerSetup_ISERVICES.GetCustListDAL(cust_id,Comp_ID);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    if (PRData != "" && PRData != null)
                    {
                        var a = PRData.Split(',');
                        var CustID = a[0].Trim();
                        var Custtype = a[1].Trim();
                        var Custcat = a[2].Trim();
                        var Custport = a[3].Trim();
                        var CustAct = a[4].Trim();
                        var CustStatus = a[5].Trim();
                        var gl_id = a[6].Trim();
                        var gl_Name = a[7].Trim();
                        if (CustStatus == "0")
                        {
                            CustStatus = null;
                        }
                        _CustomerDetails.ListFilterData = TempData["ListFilterData"].ToString();
                        DataSet HoCompData = _CustomerSetup_ISERVICES.GetCustomerListFilterDAL(Comp_ID, CustID, Custtype, Custcat, Custport, CustAct, CustStatus, gl_id);
                        ViewBag.VBCustomerList = HoCompData.Tables[0];
                        _CustomerDetails.cust_name = CustID;
                        _CustomerDetails._item_cust_type = Custtype;
                        _CustomerDetails.cust_categ = Custcat;
                        _CustomerDetails.cust_ports = Custport;
                        _CustomerDetails.item_ActStatus = CustAct;
                        _CustomerDetails.Status = CustStatus;
                        if(gl_id != "" && gl_id !="0" && gl_id != null)
                        {
                            List<GlReportingGroup> _ItemList = new List<GlReportingGroup>();

                            GlReportingGroup Glrpt = new GlReportingGroup();
                            Glrpt.Gl_rpt_id = gl_id;
                            Glrpt.Gl_rpt_Name = gl_Name;
                            _ItemList.Add(Glrpt);

                            _CustomerDetails.GlReportingGroupList = _ItemList;
                        }
                       
                    }
                    //else
                    //{
                    //    _CustomerDetails.CSSearch = "0";
                    //    TempData["ProspectDetail"] = null;
                    //    DataSet ds = _CustomerSetup_ISERVICES.GetCustListDAL(cust_id, Comp_ID);
                    //    ViewBag.VBCustomerList = ds.Tables[0];
                    //    ViewBag.AttechmentDetails = ds.Tables[1];
                    //}
                }
                //else
                //{
                    _CustomerDetails.CSSearch = "0";
                    TempData["ProspectDetail"] = null;
                    //DataSet ds = _CustomerSetup_ISERVICES.GetCustListDAL(cust_id, Comp_ID);
                    //ViewBag.VBCustomerList = ds.Tables[0];
                    //ViewBag.AttechmentDetails = ds.Tables[1];
             //   }
               
                //ViewBag.VBCustomerList = CustomerListDs;
               
                //NOTE--Comment by Hina on 01-12-2022 10:10
                //Comment Session and do it by viewBag bywhich it will be work for prospect and this page
                //Session["FinStDt"] = ds.Tables[2].Rows[0]["findate"];
                //ViewBag.VBRoleList = GetRoleList();
                //string CustID, string Custtype, string Custcat, string Custport, string CustAct, string CustStatus
                //Session["CSSearch"] = "0";
               
               
                return View("CustomerList", _CustomerDetails);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
           
        }
        private void GetAllDropDownList(CustomerDetails _CustomerDetails)
        {
            string GroupName = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (string.IsNullOrEmpty(_CustomerDetails.cust_name))
            {
                GroupName = "0";
            }
            else
            {
                GroupName = _CustomerDetails.cust_name;
            }
            DataSet dt = _CustomerSetup_ISERVICES.GetDataAllDropDown(GroupName, CompID, cust_id);
            List<cust> _cust = new List<cust>();
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                cust ddlcust = new cust();
                ddlcust.cust_id = dr["cust_id"].ToString();
                ddlcust.cust_name = dr["cust_name"].ToString();
                _cust.Add(ddlcust);
            }
            _cust.Insert(0, new cust() { cust_id = "0", cust_name = "All" });
            _CustomerDetails.custList = _cust;

         /*******************************GEt Category DropDownList*******************************************/
            List<Category> _Category = new List<Category>();

            foreach (DataRow dr in dt.Tables[1].Rows)
            {
                Category ddlcategory = new Category();
                ddlcategory.setup_id = dr["setup_id"].ToString();
                ddlcategory.setup_val = dr["setup_val"].ToString();
                _Category.Add(ddlcategory);
            }
            _Category.Insert(0, new Category() { setup_id = "0", setup_val = "All" });
            _CustomerDetails.CategoryList = _Category;
            /*****************************************End******************************************************/
            /******************PortFolio DropDown Bind****************/
            List<PortFolio> _PortFolioList = new List<PortFolio>();
          //  DataTable dt3 = GetCustport();
            foreach (DataRow dr in dt.Tables[2].Rows)
            {
                PortFolio _PortFolio = new PortFolio();
                _PortFolio.setup_id = dr["setup_id"].ToString();
                _PortFolio.setup_val = dr["setup_val"].ToString();
                _PortFolioList.Add(_PortFolio);
            }
            _PortFolioList.Insert(0, new PortFolio() { setup_id = "0", setup_val = "All" });
            _CustomerDetails.PortFolioList = _PortFolioList;
            /**************************End**********************/
            /*********************************Bind Item List Table**********************************************/
            ViewBag.VBCustomerList = dt.Tables[3];
            ViewBag.AttechmentDetails = dt.Tables[4];
            List<GlReportingGroup> _ItemList = new List<GlReportingGroup>();
          
                GlReportingGroup Glrpt = new GlReportingGroup();
                Glrpt.Gl_rpt_id = "0";
                Glrpt.Gl_rpt_Name = "All";
                _ItemList.Add(Glrpt);
           
            _CustomerDetails.GlReportingGroupList = _ItemList;
            /***********************************End**********************/
        }
        [HttpPost]
        public JsonResult GetGlReportingGrp(CustomerDetails _CustomerDetails)
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
                if (string.IsNullOrEmpty(_CustomerDetails.GlRepoting_Group))
                {
                    gl_repoting = "0";
                }
                else
                {
                    gl_repoting = _CustomerDetails.GlRepoting_Group;
                }
                DataTable GlReporting = _CustomerSetup_ISERVICES.GetGlReportingGrp(Comp_ID, Br_ID, gl_repoting);
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
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
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
        public void GetStatusList(CustomerDetails _CustomerDetails)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });               
                _CustomerDetails.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult GetCustomerListFilter(string CustID, string Custtype, string Custcat, 
            string Custport, string CustAct, string CustStatus, string Glrtp_id)
        {
            CustomerDetails _CustomerDetails = new CustomerDetails();
            ViewBag.VBCustomerList = null;
            string Comp_ID = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            try
            {
                
                DataSet HoCompData = _CustomerSetup_ISERVICES.GetCustomerListFilterDAL(Comp_ID, CustID, Custtype, Custcat, Custport, CustAct, CustStatus, Glrtp_id);
                ViewBag.VBCustomerList = HoCompData.Tables[0];
                //Session["FinStDt"] = HoCompData.Tables[1].Rows[0]["findate"];
                //Session["CSSearch"] = "CS_Search";
                _CustomerDetails.FinStDt = HoCompData.Tables[1].Rows[0]["findate"].ToString();
                _CustomerDetails.CSSearch = "CS_Search";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
              
            }
            return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialCutomerList.cshtml", _CustomerDetails);
        }
        private DataTable GetAutoCompleteCust(CustomerDetails _CustomerDetails)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string GroupName = string.Empty;
                if (string.IsNullOrEmpty(_CustomerDetails.cust_name))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _CustomerDetails.cust_name;
                }
                //DataTable dt = _CustomerSetup_ISERVICES.Bind_custList1(GroupName, Comp_ID);
                DataTable dt=_CustomerSetup_ISERVICES.Bind_custList1(GroupName, Comp_ID);
               
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable GetAutoCompletecat(CustomerDetails _CustomerDetails)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string GroupName = string.Empty;
                if (string.IsNullOrEmpty(_CustomerDetails.cust_categ))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _CustomerDetails.cust_categ;
                }
                //DataTable dt = _CustomerSetup_ISERVICES.Bind_custList1(GroupName, Comp_ID);
                DataTable HoCompData = _CustomerSetup_ISERVICES.Getcategory(Comp_ID);

                return HoCompData;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetCustport()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _CustomerSetup_ISERVICES.GetCustport(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        //public ActionResult GetAutoCompleteCustList(SearchSupp queryParameters)
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
        //        SuppList = _CustomerSetup_ISERVICES.CustNameListDAL(GroupName, Comp_ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
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
        //        DataSet HoCompData = _CustomerSetup_ISERVICES.GetcategoryDAL(Comp_ID);
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
        //public JsonResult GetCustport()
        //{
        //    JsonResult DataRows = null;
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        DataSet HoCompData = _CustomerSetup_ISERVICES.GetCustportDAL(Comp_ID);
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
                return Json("ErrorPage");
            }
        }
        public ActionResult AddNewCustomer()
        {
            CustomerDetails _CustomerDetails = new CustomerDetails();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";

            _CustomerDetails.Message = "New";
            _CustomerDetails.Command = "Add";
            _CustomerDetails.AppStatus = "D";
            _CustomerDetails.TransType = "Save";
            _CustomerDetails.BtnName = "BtnAddNew";
            TempData["ModelData"] = _CustomerDetails;
            TempData["ListFilterData"] = null;

            return RedirectToAction("CustomerDetails", "CustomerDetails");
        }
        public ActionResult EditCustomer(string CustId, string act_status, string cust_hold,string ListFilterData)
        {
            try
            {
                CustomerDetails _CustomerDetails = new CustomerDetails();
                //Session["Message"] = "New";
                //Session["Command"] = "Add";
                //Session["CustCode"] = CustId;
                //Session["TransType"] = "Update";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnToDetailPage";

                _CustomerDetails.Message = "New";
                _CustomerDetails.Command = "Add";
                _CustomerDetails.CustCode = CustId;
                _CustomerDetails.TransType = "Update";
                _CustomerDetails.AppStatus = "D";
                _CustomerDetails.BtnName = "BtnToDetailPage";
                if (act_status != "N")
                {
                    _CustomerDetails.act_stats = true;
                }
                else
                {
                    _CustomerDetails.act_stats = false;
                }
                if (cust_hold != "N")
                {
                    _CustomerDetails.cust_hold = true;
                }
                else
                {
                    _CustomerDetails.cust_hold = false;
                }
                var custCodeURL = CustId;
                var TransType = "Update";
                var BtnName = "BtnToDetailPage";
                var command = "Add";
                TempData["ModelData"] = _CustomerDetails;
                TempData["ListFilterData"] = ListFilterData;
                return (RedirectToAction("CustomerDetails", "CustomerDetails", new { custCodeURL = custCodeURL, TransType, BtnName, command }));
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
                string compId = "0";
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                CommonController com_obj = new CommonController();
                DataTable CustomerDetail = new DataTable();
                DataTable Address = new DataTable();
                DataTable BrDetail = new DataTable();
                DataSet obj_ds = new DataSet();
                CustomerDetail.Columns.Add("Customer Type*", typeof(string));
                CustomerDetail.Columns.Add("TCS Applicable*", typeof(string));
                CustomerDetail.Columns.Add("Customer Name*(max 100 characters)", typeof(string));
                CustomerDetail.Columns.Add("Customer Alias(max 50 characters)", typeof(string));
                CustomerDetail.Columns.Add("GL Account Name*(max 100 characters)", typeof(string));
                CustomerDetail.Columns.Add("GL Account Group*", typeof(string));
                CustomerDetail.Columns.Add("GL Reporting Group", typeof(string));
                CustomerDetail.Columns.Add("Default Currency*", typeof(string)); 
                CustomerDetail.Columns.Add("GST Category*", typeof(string));
                CustomerDetail.Columns.Add("Category*", typeof(string));
                CustomerDetail.Columns.Add("Portfolio*", typeof(string));
                CustomerDetail.Columns.Add("Customer Group", typeof(string));
                CustomerDetail.Columns.Add("Customer Zone", typeof(string));
                CustomerDetail.Columns.Add("Price Policy", typeof(string));
                CustomerDetail.Columns.Add("Price Group", typeof(string));
                CustomerDetail.Columns.Add("Sales Region*", typeof(string));
                CustomerDetail.Columns.Add("Registration Number(max 50 characters)", typeof(string));
                CustomerDetail.Columns.Add("TAN Number(max 50 characters)", typeof(string));
                CustomerDetail.Columns.Add("PAN number(max 50 characters)", typeof(string));
                CustomerDetail.Columns.Add("Contact Person(max 50 characters)", typeof(string));
                CustomerDetail.Columns.Add("Contact Email ID(max 50 characters)", typeof(string));
                CustomerDetail.Columns.Add("Contact Number(max 50 characters)", typeof(string));
                CustomerDetail.Columns.Add("Payment Alert (In Days)", typeof(string));
                CustomerDetail.Columns.Add("Remarks(max 200 characters)", typeof(string));
                CustomerDetail.Columns.Add("Default Transporter", typeof(string));
                CustomerDetail.Columns.Add("Credit Limit", typeof(string));
                CustomerDetail.Columns.Add("Credit Days", typeof(string));
                CustomerDetail.Columns.Add("Applicable On", typeof(string));
                CustomerDetail.Columns.Add("Bank Name(max 50 characters)", typeof(string)); 
                CustomerDetail.Columns.Add("Bank Branch(max 50 characters)", typeof(string));
                CustomerDetail.Columns.Add("Address(max 100 characters)", typeof(string));
                CustomerDetail.Columns.Add("Account Number(max 50 characters)", typeof(string));
                CustomerDetail.Columns.Add("IFSC Code(max 25 characters)", typeof(string));
                CustomerDetail.Columns.Add("SWIFT Code(max 25 characters)", typeof(string));

                Address.Columns.Add("Customer Name*", typeof(string));
                Address.Columns.Add("Address*(max 200 characters)", typeof(string));
                Address.Columns.Add("Country*", typeof(string));
                Address.Columns.Add("State*", typeof(string));
                Address.Columns.Add("District*", typeof(string));
                Address.Columns.Add("City*", typeof(string));
                Address.Columns.Add("Pin(max 15 characters)", typeof(string));
                Address.Columns.Add("GST Number*(max 15 characters)", typeof(string));
                Address.Columns.Add("Contact Person(max 50 characters)", typeof(string));
                Address.Columns.Add("Contact Number(max 50 characters)", typeof(string));
                Address.Columns.Add("Shipping Address", typeof(string));
                Address.Columns.Add("Billing Address", typeof(string)); 

                BrDetail.Columns.Add("Customer Name", typeof(string));
                BrDetail.Columns.Add("Branch Name", typeof(string));
                BrDetail.Columns.Add("Active", typeof(string));
               
                obj_ds.Tables.Add(CustomerDetail);
                obj_ds.Tables.Add(Address);
                obj_ds.Tables.Add(BrDetail);

                DataSet ds = _CustomerSetup_ISERVICES.GetMasterDropDownList(compId, Br_ID);
                string filePath = com_obj.CreateExcelFile("ImportCustomerTemplate", Server);
                com_obj.AppendExcel(filePath, ds, obj_ds, "CustomerSetup");
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
                    DataTable CustomerDetail = new DataTable();
                    DataTable CustomerBranch = new DataTable();
                    DataTable CustomerAddress = new DataTable();
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

                                string CustomerDetailQuery = "SELECT DISTINCT * FROM [CustomerDetail$] WHERE LEN([Customer Name*(max 100 characters)]) > 0;";
                                string CustomerBranchQuery = "SELECT DISTINCT * From [BranchMapping$] ; ";
                                string CustomerAddressQuery = "SELECT DISTINCT * From [Address$]; ";

                                connExcel.Open();
                                cmdExcel.CommandText = CustomerDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerDetail);

                                cmdExcel.CommandText = CustomerBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerBranch);

                                cmdExcel.CommandText = CustomerAddressQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerAddress);
                                connExcel.Close();

                                ds.Tables.Add(CustomerDetail);
                                ds.Tables.Add(CustomerBranch);
                                ds.Tables.Add(CustomerAddress);
                                DataSet dts = VerifyData(ds, uploadStatus);
                                if (dts == null)
                                    return Json("Excel file is empty. Please fill data in excel file and try again");
                                ViewBag.ImportItemsPreview = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialImportCustomerDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet VerifyData(DataSet dscustomer, string uploadStatus)
        {
            string compId = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            DataSet dts = PrepareDataset(dscustomer);
            if (dscustomer.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dscustomer.Tables[0].Rows[0].ToString()))
            {
                DataSet result = _CustomerSetup_ISERVICES.GetVerifiedDataOfExcel(compId, dts.Tables[0], dts.Tables[1], dts.Tables[2]);
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
        public DataSet PrepareDataset(DataSet dscustomer)
        {
            DataTable CustomerDetail = new DataTable();
            DataTable CustomerBranch = new DataTable();
            DataTable CustomerAddress = new DataTable();
            CustomerDetail.Columns.Add("cust_type", typeof(string));
            CustomerDetail.Columns.Add("tcs_posting", typeof(string));
            CustomerDetail.Columns.Add("cust_name", typeof(string));
            CustomerDetail.Columns.Add("cust_alias", typeof(string));
            CustomerDetail.Columns.Add("acc_name", typeof(string));
            CustomerDetail.Columns.Add("acc_grp_id", typeof(string));
            CustomerDetail.Columns.Add("gl_rpt_id", typeof(string));
            CustomerDetail.Columns.Add("def_curr_id", typeof(string));
            CustomerDetail.Columns.Add("gst_cat", typeof(string));
            CustomerDetail.Columns.Add("cust_catg", typeof(string));
            CustomerDetail.Columns.Add("cust_port", typeof(string));
            CustomerDetail.Columns.Add("cust_pr_pol", typeof(string));
            CustomerDetail.Columns.Add("cust_pr_grp", typeof(string));
            CustomerDetail.Columns.Add("cust_region", typeof(string));
            CustomerDetail.Columns.Add("cust_regn_no", typeof(string));
            CustomerDetail.Columns.Add("cust_tan_no", typeof(string));
            CustomerDetail.Columns.Add("cust_pan_no", typeof(string));
            CustomerDetail.Columns.Add("cont_pers", typeof(string));
            CustomerDetail.Columns.Add("cont_email", typeof(string));
            CustomerDetail.Columns.Add("cont_num1", typeof(string));
            CustomerDetail.Columns.Add("paym_alrt", typeof(string));
            CustomerDetail.Columns.Add("cust_remarks", typeof(string));
            CustomerDetail.Columns.Add("def_trns_name", typeof(string));
            CustomerDetail.Columns.Add("cre_limit", typeof(string));
            CustomerDetail.Columns.Add("cre_days", typeof(string));
            CustomerDetail.Columns.Add("app_on", typeof(string));
            CustomerDetail.Columns.Add("bank_name", typeof(string));
            CustomerDetail.Columns.Add("bank_branch", typeof(string));
            CustomerDetail.Columns.Add("bank_add", typeof(string));
            CustomerDetail.Columns.Add("bank_acc_no", typeof(string));
            CustomerDetail.Columns.Add("ifsc_code", typeof(string));
            CustomerDetail.Columns.Add("swift_code", typeof(string));
            CustomerDetail.Columns.Add("cust_group", typeof(string));
            CustomerDetail.Columns.Add("cust_zone", typeof(string));
            for (int i = 0; i< dscustomer.Tables[0].Rows.Count; i++)
            {
                DataTable dtcustomerdetail = dscustomer.Tables[0];
                DataRow dtr = CustomerDetail.NewRow();
                if(dtcustomerdetail.Rows[i][0].ToString() == "")
                {
                    dtr["cust_type"] = dtcustomerdetail.Rows[i][0].ToString().Trim();
                }
                else
                {
                    dtr["cust_type"] = dtcustomerdetail.Rows[i][0].ToString().Trim().Substring(0, 1);
                }
                if (dtcustomerdetail.Rows[i][1].ToString() == "")
                {
                    dtr["tcs_posting"] = dtcustomerdetail.Rows[i][1].ToString().Trim();
                }
                else
                {
                    dtr["tcs_posting"] = dtcustomerdetail.Rows[i][1].ToString().Trim().Substring(0, 1);
                }
                //dtr["cust_name"] = dtcustomerdetail.Rows[i][2].ToString().Trim().Replace(",_", " ");
                dtr["cust_name"] = dtcustomerdetail.Rows[i][2].ToString().Trim();
                dtr["cust_alias"] = dtcustomerdetail.Rows[i][3].ToString().Trim();
                dtr["acc_name"] = dtcustomerdetail.Rows[i][4].ToString().Trim();
                dtr["acc_grp_id"] = dtcustomerdetail.Rows[i][5].ToString().Trim();
                dtr["gl_rpt_id"] = dtcustomerdetail.Rows[i][6].ToString().Trim();
                dtr["def_curr_id"] = dtcustomerdetail.Rows[i][7].ToString().Trim();
                if (dtcustomerdetail.Rows[i][8].ToString() == "")
                {
                    
                    dtr["gst_cat"] = dtcustomerdetail.Rows[i][8].ToString().Trim();
                }
                else
                {
                    if (dtcustomerdetail.Rows[i][8].ToString() == "Registered")
                    {
                        dtr["gst_cat"] = "RR";
                    }
                    else if(dtcustomerdetail.Rows[i][8].ToString() == "Un-Registered")
                    {
                        dtr["gst_cat"] = "UR";
                    }
                    else
                    {
                        dtr["gst_cat"] = dtcustomerdetail.Rows[i][8].ToString().Trim().Substring(0, 2).ToUpper();
                    }
                }
                dtr["cust_catg"] = dtcustomerdetail.Rows[i][9].ToString().Trim();
                dtr["cust_port"] = dtcustomerdetail.Rows[i][10].ToString().Trim();
                dtr["cust_group"] = dtcustomerdetail.Rows[i][11].ToString().Trim();
                dtr["cust_zone"] = dtcustomerdetail.Rows[i][12].ToString().Trim();
                if (dtcustomerdetail.Rows[i][13].ToString() == "")
                {
                    dtr["cust_pr_pol"] = dtcustomerdetail.Rows[i][13].ToString().Trim();
                }
                else
                {
                    dtr["cust_pr_pol"] = dtcustomerdetail.Rows[i][13].ToString().Trim().Substring(0, 1);
                }
                dtr["cust_pr_grp"] = dtcustomerdetail.Rows[i][14].ToString().Trim();
                dtr["cust_region"] = dtcustomerdetail.Rows[i][15].ToString().Trim();
                dtr["cust_regn_no"] = dtcustomerdetail.Rows[i][16].ToString().Trim();
                dtr["cust_tan_no"] = dtcustomerdetail.Rows[i][17].ToString().Trim();
                dtr["cust_pan_no"] = dtcustomerdetail.Rows[i][18].ToString().Trim();
                dtr["cont_pers"] = dtcustomerdetail.Rows[i][19].ToString().Trim();
                dtr["cont_email"] = dtcustomerdetail.Rows[i][20].ToString().Trim();
                dtr["cont_num1"] = dtcustomerdetail.Rows[i][21].ToString().Trim();
                int paymalrt = 0;
                int.TryParse(dtcustomerdetail.Rows[i][22].ToString().Trim(), out paymalrt);
                dtr["paym_alrt"] = paymalrt.ToString();
                dtr["cust_remarks"] = dtcustomerdetail.Rows[i][23].ToString().Trim(); 
                dtr["def_trns_name"] = dtcustomerdetail.Rows[i][24].ToString().Trim(); 
                float crelimit = 0;
                int creday = 0;
                float.TryParse(dtcustomerdetail.Rows[i][25].ToString().Trim(), out crelimit);
                dtr["cre_limit"] = crelimit.ToString();
                int.TryParse(dtcustomerdetail.Rows[i][26].ToString().Trim(), out creday);
                dtr["cre_days"] = creday.ToString();
                dtr["app_on"] = dtcustomerdetail.Rows[i][27].ToString().Trim();
                dtr["bank_name"] = dtcustomerdetail.Rows[i][28].ToString().Trim();
                dtr["bank_branch"] = dtcustomerdetail.Rows[i][29].ToString().Trim();
                dtr["bank_add"] = dtcustomerdetail.Rows[i][30].ToString().Trim();
                dtr["bank_acc_no"] = dtcustomerdetail.Rows[i][31].ToString().Trim();
                dtr["ifsc_code"] = dtcustomerdetail.Rows[i][32].ToString().Trim();
                dtr["swift_code"] = dtcustomerdetail.Rows[i][33].ToString().Trim();
                CustomerDetail.Rows.Add(dtr);
            }
            //-------------------------------Branch Detail-------------------------
            CustomerBranch.Columns.Add("cust_name", typeof(string));
            CustomerBranch.Columns.Add("branch_name", typeof(string));
            CustomerBranch.Columns.Add("act_status", typeof(string));
            for(int i=0; i< dscustomer.Tables[1].Rows.Count; i++)
            {
                DataTable dtbranchdetail = dscustomer.Tables[1];
                DataRow dtr = CustomerBranch.NewRow();
                dtr["cust_name"] = dtbranchdetail.Rows[i][0].ToString().Trim();
                dtr["branch_name"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                if (!string.IsNullOrEmpty(dtbranchdetail.Rows[i][2].ToString()))
                {
                    dtr["act_status"] = dtbranchdetail.Rows[i][2].ToString().Trim().Substring(0, 1);
                }
                else
                {
                    dtr["act_status"] = "N";
                }
                //dtr["act_status"] = dtbranchdetail.Rows[i][1].ToString().Trim().Substring(0, 1);
                CustomerBranch.Rows.Add(dtr);
            }
            //---------------------------End-------------------------------------
            //-----------------------------Address-------------------------------
            CustomerAddress.Columns.Add("cust_name", typeof(string));
            CustomerAddress.Columns.Add("cust_add", typeof(string));
            CustomerAddress.Columns.Add("cust_cntry", typeof(string));
            CustomerAddress.Columns.Add("cust_state", typeof(string));
            CustomerAddress.Columns.Add("cust_dist", typeof(string));
            CustomerAddress.Columns.Add("cust_city", typeof(string));
            CustomerAddress.Columns.Add("cust_pin", typeof(string));
            CustomerAddress.Columns.Add("cust_gst_no", typeof(string));
            CustomerAddress.Columns.Add("Cont_Person", typeof(string));
            CustomerAddress.Columns.Add("Cont_Number", typeof(string));
            CustomerAddress.Columns.Add("def_ship_add", typeof(string));
            CustomerAddress.Columns.Add("def_bill_add", typeof(string));
            for(int i= 0; i< dscustomer.Tables[2].Rows.Count; i++)
            {

                DataTable dtaddress = dscustomer.Tables[2];
                DataRow dtr = CustomerAddress.NewRow();
                dtr["cust_name"] = dtaddress.Rows[i][0].ToString().Trim();
                dtr["cust_add"] = dtaddress.Rows[i][1].ToString().Trim();
                dtr["cust_cntry"] = dtaddress.Rows[i][2].ToString().Trim();
                dtr["cust_state"] = dtaddress.Rows[i][3].ToString().Trim();
                dtr["cust_dist"] = dtaddress.Rows[i][4].ToString().Trim();
                dtr["cust_city"] = dtaddress.Rows[i][5].ToString().Trim();
                dtr["cust_pin"] = dtaddress.Rows[i][6].ToString().Trim();
                dtr["cust_gst_no"] = dtaddress.Rows[i][7].ToString().Trim();
                dtr["Cont_Person"] = dtaddress.Rows[i][8].ToString().Trim();
                dtr["Cont_Number"] = dtaddress.Rows[i][9].ToString().Trim();
                    if (dtaddress.Rows[i][10] == null || dtaddress.Rows[i][10].ToString().Trim() == "")
                    {
                        dtr["def_ship_add"] = "";
                    }
                    else
                    {
                        dtr["def_ship_add"] = dtaddress.Rows[i][10].ToString().Trim().Substring(0, 1);
                    }

                    if (dtaddress.Rows[i][11] == null || dtaddress.Rows[i][11].ToString().Trim() == "")
                    {
                        dtr["def_bill_add"] = "";
                    }
                    else
                    {
                        dtr["def_bill_add"] = dtaddress.Rows[i][11].ToString().Trim().Substring(0, 1);
                    }
                CustomerAddress.Rows.Add(dtr);
            }
            //---------------------------End-------------------------------------
            DataSet dts = new DataSet();
            dts.Tables.Add(CustomerDetail);
            dts.Tables.Add(CustomerBranch);
            dts.Tables.Add(CustomerAddress);
            return dts;
        }
        public ActionResult BindCustomerAddress(string customerName)
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

                    DataTable dtcustomerdetail = new DataTable();
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
                                string subcustomerQuery = "SELECT [Customer Name*], [Address*(max 200 characters)], [Country*], [State*], [District*], [City*], [Pin(max 15 characters)], [GST Number*(max 15 characters)],[Contact Person(max 50 characters)],[Contact Number(max 50 characters)] ,MAX([Billing Address]) AS [Billing Address],MAX([Shipping Address]) AS [Shipping Address] " +
                              "FROM [Address$] " +
                              "WHERE [Customer Name*] = '" + customerName + "' " +
                              "GROUP BY [Customer Name*], [Address*(max 200 characters)], [Country*], [State*], [District*], [City*], [Pin(max 15 characters)],[Contact Person(max 50 characters)],[Contact Number(max 50 characters)], " +
                              "[GST Number*(max 15 characters)];";
                                cmdExcel.CommandText = subcustomerQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtcustomerdetail);

                                DataTable CustomerAddress = new DataTable();
                                CustomerAddress.Columns.Add("cust_name", typeof(string));
                                CustomerAddress.Columns.Add("cust_add", typeof(string));
                                CustomerAddress.Columns.Add("cust_cntry", typeof(string));
                                CustomerAddress.Columns.Add("cust_state", typeof(string));
                                CustomerAddress.Columns.Add("cust_dist", typeof(string));
                                CustomerAddress.Columns.Add("cust_city", typeof(string));
                                CustomerAddress.Columns.Add("cust_pin", typeof(string));
                                CustomerAddress.Columns.Add("cust_gst_no", typeof(string));
                                CustomerAddress.Columns.Add("Cont_Person", typeof(string));
                                CustomerAddress.Columns.Add("Cont_Number", typeof(string));
                                CustomerAddress.Columns.Add("def_ship_add", typeof(string));
                                CustomerAddress.Columns.Add("def_bill_add", typeof(string));

                                for (int i = 0; i < dtcustomerdetail.Rows.Count; i++)
                                {
                                    DataTable dtaddress = dtcustomerdetail;
                                    DataRow dtr = CustomerAddress.NewRow();
                                    dtr["cust_name"] = dtaddress.Rows[i][0].ToString().Trim();
                                    dtr["cust_add"] = dtaddress.Rows[i][1].ToString().Trim();
                                    dtr["cust_cntry"] = dtaddress.Rows[i][2].ToString().Trim();
                                    dtr["cust_state"] = dtaddress.Rows[i][3].ToString().Trim();
                                    dtr["cust_dist"] = dtaddress.Rows[i][4].ToString().Trim();
                                    dtr["cust_city"] = dtaddress.Rows[i][5].ToString().Trim();
                                    dtr["cust_pin"] = dtaddress.Rows[i][6].ToString().Trim();
                                    dtr["cust_gst_no"] = dtaddress.Rows[i][7].ToString().Trim();
                                    dtr["Cont_Person"] = dtaddress.Rows[i][8].ToString().Trim();
                                    dtr["Cont_Number"] = dtaddress.Rows[i][9].ToString().Trim();
                                    dtr["def_ship_add"] = dtaddress.Rows[i][10].ToString().Trim();
                                    dtr["def_bill_add"] = dtaddress.Rows[i][11].ToString().Trim();
                                    CustomerAddress.Rows.Add(dtr);
                                }
                                ViewBag.CustomerAddressDetail = CustomerAddress;
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
        public ActionResult BindCustomerBranch(string customerName)
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

                    DataTable dtcustomerdetail = new DataTable();
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
                                string customerQuery = "SELECT DISTINCT * From [BranchMapping$] where [Customer Name] = '" + customerName + "' ; ";
                                cmdExcel.CommandText = customerQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtcustomerdetail);

                                DataTable CustomerBranch = new DataTable();
                                CustomerBranch.Columns.Add("cust_name", typeof(string));
                                CustomerBranch.Columns.Add("branch_name", typeof(string));
                                CustomerBranch.Columns.Add("act_status", typeof(string));
                                for (int i = 0; i < dtcustomerdetail.Rows.Count; i++)
                                {
                                    DataTable dtbranchdetail = dtcustomerdetail;
                                    DataRow dtr = CustomerBranch.NewRow();
                                    dtr["cust_name"] = dtbranchdetail.Rows[i][0].ToString().Trim();
                                    dtr["branch_name"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                                    dtr["act_status"] = dtbranchdetail.Rows[i][2].ToString().Trim();
                                    CustomerBranch.Rows.Add(dtr);
                                }
                                ViewBag.BranchDetail = CustomerBranch;
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
        public ActionResult ShowValidationError(string customerName)
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
                    DataTable CustomerDetail = new DataTable();
                    DataTable CustomerBranch = new DataTable();
                    DataTable CustomerAddress = new DataTable();
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
                                string CustomerDetailQuery = "";
                                if (string.IsNullOrWhiteSpace(customerName))
                                {
                                    CustomerDetailQuery = "SELECT DISTINCT * FROM [CustomerDetail$] WHERE [Customer Name*(max 100 characters)] IS NULL OR LTRIM(RTRIM([Customer Name*(max 100 characters)])) = '';";
                                }
                                else
                                {
                                    CustomerDetailQuery = "SELECT DISTINCT * FROM [CustomerDetail$] WHERE [Customer Name*(max 100 characters)] = '" + customerName + "';";
                                }
                                //string CustomerDetailQuery = "SELECT DISTINCT * From [CustomerDetail$] where [Customer Name*_(max 100 characters)] = '" + customerName + "' ; ";
                                string CustomerBranchQuery = "SELECT DISTINCT * From [BranchMapping$] where [Customer Name] = '" + customerName + "' ; ";
                                string CustomerAddressQuery = "SELECT DISTINCT * From [Address$] where [Customer Name*] = '" + customerName + "' ; ";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = CustomerDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerDetail);

                                cmdExcel.CommandText = CustomerBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerBranch);

                                cmdExcel.CommandText = CustomerAddressQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerAddress);
                                connExcel.Close();
                               
                                ds.Tables.Add(CustomerDetail);
                                ds.Tables.Add(CustomerBranch);
                                ds.Tables.Add(CustomerAddress);
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
            DataTable result = _CustomerSetup_ISERVICES.ShowExcelErrorDetail(compId, dts.Tables[0], dts.Tables[1], dts.Tables[2]);
            return result;
        }
        public JsonResult ImportCustomerDetailFromExcel()
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
                    DataTable CustomerDetail = new DataTable();
                    DataTable CustomerBranch = new DataTable();
                    DataTable CustomerAddress = new DataTable();
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
                                string CustomerDetailQuery = "SELECT DISTINCT * FROM [CustomerDetail$] WHERE LEN([Customer Name*(max 100 characters)]) > 0;";
                                string CustomerBranchQuery = "SELECT DISTINCT * From [BranchMapping$] ; ";
                                string CustomerAddressQuery = "SELECT [Customer Name*], [Address*(max 200 characters)], [Country*], [State*], [District*], [City*], [Pin(max 15 characters)]," +
                                    " [GST Number*(max 15 characters)],[Contact Person(max 50 characters)],[Contact Number(max 50 characters)] ,MAX([Shipping Address]) AS [Shipping Address], MAX([Billing Address]) AS [Billing Address] " +
                              "FROM [Address$] " +
                              "GROUP BY [Customer Name*], [Address*(max 200 characters)], [Country*], [State*], [District*], [City*], [Pin(max 15 characters)], [GST Number*(max 15 characters)],[Contact Person(max 50 characters)],[Contact Number(max 50 characters)];";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = CustomerDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerDetail);

                                cmdExcel.CommandText = CustomerBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerBranch);

                                cmdExcel.CommandText = CustomerAddressQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(CustomerAddress);
                                connExcel.Close();

                                ds.Tables.Add(CustomerDetail);
                                ds.Tables.Add(CustomerBranch);
                                ds.Tables.Add(CustomerAddress);
                                string msg = SaveCustomerFromExcel(ds);
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
        private string SaveCustomerFromExcel(DataSet dsCustomer)
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
            DataSet dts = PrepareDataset(dsCustomer);
            string result = _CustomerSetup_ISERVICES.BulkImportCustomerDetail(compId, UserID, BranchName, dts.Tables[0], dts.Tables[1], dts.Tables[2]);
            return result;
        }
        [HttpPost]
        public JsonResult GetLedgerDetail(string Cust_id, string CustAcc_id, string Curr_id)
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
                DataSet result = _CustomerSetup_ISERVICES.GetCustLedgerDtl(Comp_ID, Br_ID, Cust_id, CustAcc_id, Curr_id);
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