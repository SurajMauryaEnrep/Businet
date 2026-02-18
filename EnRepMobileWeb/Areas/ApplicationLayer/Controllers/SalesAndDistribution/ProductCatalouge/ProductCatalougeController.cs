using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ProductCatalouge;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.ProductCatalouge;
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
using System.Text;
using System.Web.Mvc;
using static EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ProductCatalouge.ProductCatalouge_Model;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.ProductCatalouge
{
    public class ProductCatalougeController : Controller
    {
        //DataTable dt;
        string CompID, UserID, BrchID, language = String.Empty;
        string DocumentMenuId = "105103185", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ProductCatalouge_ISERVICES _ProductCatalouge_ISERVICES;

        public ProductCatalougeController(Common_IServices _Common_IServices, ProductCatalouge_ISERVICES _ProductCatalouge_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._ProductCatalouge_ISERVICES = _ProductCatalouge_ISERVICES;
           
        }
        // GET: ApplicationLayer/ProductCatalouge
        public ActionResult ProductCatalouge(ProductCatalouge_Model _ProdCataModel)
        {
            try
            {
                //ProductCatalouge_Model _ProdCataModel = new ProductCatalouge_Model();
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
                CommonPageDetails();
                var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;


                //List<CustNameList> custLists = new List<CustNameList>();
                //custLists.Add(new CustNameList { Cust_id = "0", Cust_name = "---Select---" });
                //_ProdCataModel._CustNameList = custLists;

                #region Commented By Nitesh 06-04-2024 for All DropdownList in One Procedure 
                #endregion
                //DataTable dt1 = new DataTable();
                //List<CustomerName> CustLists = new List<CustomerName>();
                //dt1 = GetCustNameList(_ProdCataModel);
                //foreach (DataRow dr in dt1.Rows)
                //{
                //    CustomerName _RAList = new CustomerName();
                //    _RAList.cust_id = dr["cust_id"].ToString();
                //    _RAList.cust_name = dr["cust_name"].ToString();
                //    CustLists.Add(_RAList);
                //}
                //CustLists.Insert(0, new CustomerName() { cust_id = "0", cust_name = "---Select---" });
                //_ProdCataModel.CustomerNameList = CustLists;
                #region EndRegion
                #endregion
                List<StatusList> statusList = new List<StatusList>();
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new StatusList { status_id = x.status_id, status_name = x.status_name });
                _ProdCataModel._statusList = listOfStatus;
                _ProdCataModel.Command = "Add";
                _ProdCataModel.TransType = "Save";
                _ProdCataModel.BtnName = "BtnAddNew";
               //ViewBag.MenuPageName = getDocumentName();
               //ViewBag.VBRoleList = GetRoleList();
                _ProdCataModel.Title = title;

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    var CustID = a[0].Trim();
                    var Fromdate = a[1].Trim();
                    var Todate = a[2].Trim();
                    var Status = a[3].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    _ProdCataModel.ListFilterData = TempData["ListFilterData"].ToString();
                    _ProdCataModel.Catl_FromDate = Fromdate;
                    _ProdCataModel.Catl_ToDate = Todate;
                    _ProdCataModel.Catl_status = Status;
                    _ProdCataModel.CustID = CustID;
                    #region Commented By Nitesh 06-04-2024 for All DropdownList in One Procedure 
                    #endregion
                    // ViewBag.PCatList = _ProductCatalouge_ISERVICES.GetSearchListOfProdCatalogDetails(CompID, BrchID, CustID, Fromdate, Todate, Status, "", "", "").Tables[0];
                    #region EndRegion
                    #endregion
                
                }
                else
                {
                    #region Commented By Nitesh 06-04-2024 for All DropdownList in One Procedure 
                    #endregion
                    //  DataSet ds = _ProductCatalouge_ISERVICES.GetListOfProdCatalogDetails(CompID, BrchID, UserID, wfstatus, DocumentMenuId);

                    // ViewBag.PCatList = ds.Tables[0];
                    #region EndRegion
                    #endregion
                  
                    _ProdCataModel.Catl_FromDate = startDate;
                    _ProdCataModel.Catl_ToDate = CurrentDate;
                  
                }
                GetAllData(_ProdCataModel);
                //DateTime dt = DateTime.Now;
                //DateTime dtBegin = dt.AddDays(-(dt.Day - 1));
                //Session["CatalogSearch"] = "0";
                _ProdCataModel.CatalogSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ProductCatalouge/ProductCatalougeList.cshtml", _ProdCataModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private void GetAllData(ProductCatalouge_Model _ProdCataModel)
        {
            string wfstatus = string.Empty;
            string CustomerName = string.Empty;
            string CompID = string.Empty;
            string BrchID = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (string.IsNullOrEmpty(_ProdCataModel.CustID))
            {
                CustomerName = "0";
            }
            else
            {
                CustomerName = _ProdCataModel.CustID;
            }
            if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
            {
                _ProdCataModel.WF_status = TempData["WF_status"].ToString();
                if (_ProdCataModel.WF_status != null)
                {
                    wfstatus = _ProdCataModel.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
            }
            else
            {
                if (_ProdCataModel.WF_status != null)
                {
                    wfstatus = _ProdCataModel.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
            }
            DataSet GetAllData = _ProductCatalouge_ISERVICES.GetAllData(CompID, BrchID, CustomerName, UserID, wfstatus, DocumentMenuId,
                 _ProdCataModel.CustID, _ProdCataModel.Catl_FromDate, _ProdCataModel.Catl_ToDate, _ProdCataModel.Catl_status);
         
            List<CustomerName> CustLists = new List<CustomerName>();
          
            foreach (DataRow dr in GetAllData.Tables[0].Rows)
            {
                CustomerName _RAList = new CustomerName();
                _RAList.cust_id = dr["cust_id"].ToString();
                _RAList.cust_name = dr["cust_name"].ToString();
                CustLists.Add(_RAList);
            }
            CustLists.Insert(0, new CustomerName() { cust_id = "0", cust_name = "---Select---" });
            _ProdCataModel.CustomerNameList = CustLists;

            ViewBag.PCatList = GetAllData.Tables[1];
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
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
        private DataTable GetCustNameList(ProductCatalouge_Model _ProdCataModel)
        {
            try
            {
                string CustomerName = string.Empty;
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_ProdCataModel.CustID))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _ProdCataModel.CustID;
                }
                DataTable dt = _ProductCatalouge_ISERVICES.GetCustNameList(CompID, BrchID, CustomerName);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetPRoductCatalogueList(string docid, string status)
        {
            ProductCatalouge_Model _ProdCataModel = new ProductCatalouge_Model();
            //Session["WF_status"] = status;
            _ProdCataModel.WF_status = status;
            return RedirectToAction("ProductCatalouge", _ProdCataModel);
        }
        public ActionResult AddProductCatalougeDetail(string flag)
        {
           try
            {
            ProductCatalouge_Model _ProdCataModel = new ProductCatalouge_Model();
               if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                CommonPageDetails();
                if(flag=="Y")
                {
                    _ProdCataModel.propectflag = "Y";
                }
           // var other = new CommonController(_Common_IServices);
           //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                _ProdCataModel.Message = "New";
                _ProdCataModel.Command = "Add";

                _ProdCataModel.AppStatus = "D";
                _ProdCataModel.TransType = "Save";
                _ProdCataModel.BtnName = "BtnAddNew";
                _ProdCataModel.DocumentStatus = "D";
                _ProdCataModel.Title = title;
                TempData["ModelData"] = _ProdCataModel;
                //Session["Message"] = "New";
                //Session["Command"] = "Add";
                //Session["AppStatus"] = 'D';
                //Session["TransType"] = "Save";
                //Session["BtnName"] = "BtnAddNew";
                //    Session["CTLNo"] = null;
                //    Session["CTLDate"] = null;
                //    Session["DocumentStatus"] = "D";
                //ViewBag.MenuPageName = getDocumentName();
                //ViewBag.VBRoleList = GetRoleList();
                /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                {
                    if (flag == "Delet")
                    {
                        _ProdCataModel.Command = "Refresh";
                        _ProdCataModel.TransType = "Refresh";
                        _ProdCataModel.BtnName = "BtnRefresh";
                        _ProdCataModel.DocumentStatus = null;
                        TempData["ModelData"] = _ProdCataModel;
                        TempData["Message"] = "Financial Year not Exist";
                        return RedirectToAction("ProductCatalougeDetail", _ProdCataModel);
                    }
                    else
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        return RedirectToAction("ProductCatalouge");
                    }
                }
                /*End to chk Financial year exist or not*/
                return RedirectToAction("ProductCatalougeDetail", "ProductCatalouge");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public ActionResult ProductCatalougeDetail(URLDetailModel URLModelSave,string doc_no, string doc_date,string ListFilterData,string WF_status)
        {
            ViewBag.DocumentMenuId = DocumentMenuId;
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
            try
            {
                var _ProdCataModel = TempData["ModelData"] as ProductCatalouge_Model;
                if (_ProdCataModel != null)
                {
                   
                    DataTable dt = new DataTable();
                    CommonPageDetails();
                    _ProdCataModel.Title = title;
                    //List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                    //dt = GetRequirmentreaList();
                    if(_ProdCataModel.propectflag =="Y")
                    {
                        _ProdCataModel.propectflag = "SuccefullyCreate";
                    }

                    List<CustNameList> CustNameList = new List<CustNameList>();
                    dt = GetCustomerListtoEdit(_ProdCataModel,"C");
                    foreach (DataRow dr in dt.Rows)
                    {
                        CustNameList CustName = new CustNameList();
                        CustName.Cust_id = dr["cust_id"].ToString();
                        CustName.Cust_name = dr["cust_name"].ToString();
                        CustNameList.Add(CustName);
                        //CustNameList CustName = new CustNameList();
                        //CustName.Cust_id = "0";
                        //CustName.Cust_name = "--- Select ---";
                        //CustNameList.Add(CustName);
                    }
                    CustNameList.Insert(0, new CustNameList() { Cust_id = "0", Cust_name = "---Select---" });
                    _ProdCataModel._CustNameList = CustNameList;


                    //GetCustomerListtoEdit(_ProdCataModel);
                    //  List<CustNameList> CustNameList = new List<CustNameList>();
                    //  CustNameList CustName = new CustNameList();
                    //  CustName.Cust_id = "0";
                    //  CustName.Cust_name = "--- Select ---";
                    //  CustNameList.Add(CustName);
                    //  _ProdCataModel._CustNameList = CustNameList;

                    List<GroupNameList> _GroupNameList = new List<GroupNameList>();
                    GroupNameList _GroupName = new GroupNameList();
                    _GroupName.ItemGroupChildNood = "--- Select ---";
                    _GroupName.item_grp_id = "0";
                    _GroupNameList.Add(_GroupName);
                    _ProdCataModel.GroupList = _GroupNameList;

                    //var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    if (doc_date != null && doc_date != null)
                    {
                        _ProdCataModel.CTLNo = doc_no;
                        _ProdCataModel.CTLDate = doc_date;
                        _ProdCataModel.TransType = "Update";
                        _ProdCataModel.Command = "New";
                        _ProdCataModel.Message = "New";
                        _ProdCataModel.BtnName = "BtnToDetailPage";

                        //Session["CTLNo"] = doc_no;
                        //Session["CTLDate"] = doc_date;
                        //Session["TransType"] = "Update";
                        //Session["Command"] = "New";
                        //Session["Message"] = "New";
                        //Session["BtnName"] = "BtnToDetailPage";
                    }
                    var create_id = "";
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ProdCataModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _ProdCataModel.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if(WF_status!=null&& WF_status.ToString() != "")
                    {
                        _ProdCataModel.WF_status1 = WF_status;
                    }
                    //if (Session["CTLNo"] != null && Session["TransType"] != null)
                    if (_ProdCataModel.CTLNo != null && _ProdCataModel.TransType != null)
                    {
                        //if (Session["TransType"].ToString() == "Update")
                        if (_ProdCataModel.TransType == "Update")
                        {
                            //string Doc_no = Session["CTLNo"].ToString();
                            string Doc_no = _ProdCataModel.CTLNo;
                            //string Doc_date = Session["CTLDate"].ToString();
                            string Doc_date = _ProdCataModel.CTLDate;
                            DataSet ds = _ProductCatalouge_ISERVICES.GetProdCatalogueDetails(CompID, BrchID, Doc_no, Doc_date, UserID, DocumentMenuId);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                _ProdCataModel.CTLNo = ds.Tables[0].Rows[0]["cat_no"].ToString();
                                _ProdCataModel.CTLDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["cat_dt"].ToString()).ToString("yyyy-MM-dd");
                                _ProdCataModel.CustPros_type = ds.Tables[0].Rows[0]["cust_type"].ToString();
                                string CustPros_type = _ProdCataModel.CustPros_type;
                                if (_ProdCataModel.CustPros_type == "P")
                                {
                                    _ProdCataModel.ProspectFromProduct = "Y";
                                }
                                else
                                {
                                    _ProdCataModel.ProspectFromProduct = "N";
                                }
                                List<CustNameList> CustNameList1 = new List<CustNameList>();
                                dt = GetCustomerListtoEdit(_ProdCataModel, CustPros_type);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    CustNameList CustName = new CustNameList();
                                    CustName.Cust_id = dr["cust_id"].ToString();
                                    CustName.Cust_name = dr["cust_name"].ToString();
                                    CustNameList1.Add(CustName);
                                    //CustNameList CustName = new CustNameList();
                                    //CustName.Cust_id = "0";
                                    //CustName.Cust_name = "--- Select ---";
                                    //CustNameList.Add(CustName);
                                }
                                CustNameList1.Insert(0, new CustNameList() { Cust_id = "0", Cust_name = "---Select---" });
                                _ProdCataModel._CustNameList = CustNameList1;

                                _ProdCataModel.CustID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                                _ProdCataModel.CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();
                                _ProdCataModel.Remarks = ds.Tables[0].Rows[0]["cat_rem"].ToString();
                                _ProdCataModel.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                                _ProdCataModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                _ProdCataModel.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                _ProdCataModel.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                _ProdCataModel.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                _ProdCataModel.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                                _ProdCataModel.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                                _ProdCataModel.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();
                                _ProdCataModel.Status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                             
                                create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                ViewBag.PCatItemDetails = ds.Tables[1];

                                _ProdCataModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                                _ProdCataModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);//Cancelled
                            }
                            string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                            string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                            //Session["DocumentStatus"] = Statuscode;
                            _ProdCataModel.DocumentStatus = Statuscode;

                            if (Statuscode != "D" && Statuscode != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[4];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _ProdCataModel.Command != "Edit")
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
                                if (Statuscode == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _ProdCataModel.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/

                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProdCataModel.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProdCataModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ProdCataModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProdCataModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ProdCataModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ProdCataModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ProdCataModel.BtnName = "BtnToDetailPage";

                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _ProdCataModel.BtnName = "BtnRefresh";
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Session["DocumentStatus"] = "D";
                            _ProdCataModel.DocumentStatus = "D";
                        }
                    }
                    if (ViewBag.AppLevel.Rows.Count == 0)
                    {
                        ViewBag.Approve = "Y";
                    }
                    if (TempData["ListFilterData"] == null)
                    {
                        _ProdCataModel.ListFilterData1 = ListFilterData;
                    }
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ProductCatalouge/ProductCatalougeDetail.cshtml", _ProdCataModel);
                }
                else
                {/*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    var commCont = new CommonController(_Common_IServices);
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {
                        TempData["Message1"] = "Financial Year not Exist";
                    }
                    /*End to chk Financial year exist or not*/
                    //ViewBag.MenuPageName = getDocumentName();
                    ProductCatalouge_Model _ProdCataModel1 = new ProductCatalouge_Model();
                    CommonPageDetails();
                    _ProdCataModel1.Title = title;
                    DataTable dt = new DataTable();
                    CommonPageDetails();
                    //List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                    //dt = GetRequirmentreaList();


                    List<CustNameList> CustNameList = new List<CustNameList>();
                    dt = GetCustomerListtoEdit(_ProdCataModel1,"C");
                    foreach (DataRow dr in dt.Rows)
                    {
                        CustNameList CustName = new CustNameList();
                        CustName.Cust_id = dr["cust_id"].ToString();
                        CustName.Cust_name = dr["cust_name"].ToString();
                        CustNameList.Add(CustName);
                        //CustNameList CustName = new CustNameList();
                        //CustName.Cust_id = "0";
                        //CustName.Cust_name = "--- Select ---";
                        //CustNameList.Add(CustName);
                    }
                    CustNameList.Insert(0, new CustNameList() { Cust_id = "0", Cust_name = "---Select---" });
                    _ProdCataModel1._CustNameList = CustNameList;

                    if (URLModelSave.DocNo != null && URLModelSave.DocDate!=null)
                    {
                        _ProdCataModel1.CTLNo = URLModelSave.DocNo;
                        _ProdCataModel1.CTLDate = URLModelSave.DocDate;
                    }
                    if (URLModelSave.TransType != null)
                    {
                        _ProdCataModel1.TransType = URLModelSave.TransType;
                    }
                    else
                    {
                        _ProdCataModel1.TransType = "New";
                    }
                    if (URLModelSave.BtnName != null)
                    {
                        _ProdCataModel1.BtnName = URLModelSave.BtnName;
                    }
                    else
                    {
                        _ProdCataModel1.BtnName = "BtnRefresh";
                    }
                    if (URLModelSave.Command != null)
                    {
                        _ProdCataModel1.Command = URLModelSave.Command;
                    }
                    else
                    {
                        _ProdCataModel1.Command = "Refresh";
                    }
                    //GetCustomerListtoEdit(_ProdCataModel1);
                    //  List<CustNameList> CustNameList = new List<CustNameList>();
                    //  CustNameList CustName = new CustNameList();
                    //  CustName.Cust_id = "0";
                    //  CustName.Cust_name = "--- Select ---";
                    //  CustNameList.Add(CustName);
                    //  _ProdCataModel1._CustNameList = CustNameList;

                    List<GroupNameList> _GroupNameList = new List<GroupNameList>();
                    GroupNameList _GroupName = new GroupNameList();
                    _GroupName.ItemGroupChildNood = "--- Select ---";
                    _GroupName.item_grp_id = "0";
                    _GroupNameList.Add(_GroupName);
                    _ProdCataModel1.GroupList = _GroupNameList;

                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    if (doc_date != null && doc_date != null)
                    {
                        _ProdCataModel1.CTLNo = doc_no;
                        _ProdCataModel1.CTLDate = doc_date;
                        _ProdCataModel1.TransType = "Update";
                        _ProdCataModel1.Command = "New";
                        _ProdCataModel1.Message = "New";
                        _ProdCataModel1.BtnName = "BtnToDetailPage";

                        //Session["CTLNo"] = doc_no;
                        //Session["CTLDate"] = doc_date;
                        //Session["TransType"] = "Update";
                        //Session["Command"] = "New";
                        //Session["Message"] = "New";
                        //Session["BtnName"] = "BtnToDetailPage";
                    }
                    var create_id = "";
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ProdCataModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _ProdCataModel1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (WF_status != null && WF_status.ToString() != "")
                    {
                        _ProdCataModel1.WF_status1 = WF_status;
                    }
                    //if (Session["CTLNo"] != null && Session["TransType"] != null)
                    if (_ProdCataModel1.CTLNo != null && _ProdCataModel1.TransType != null)
                    {
                        //if (Session["TransType"].ToString() == "Update")
                        if (_ProdCataModel1.TransType == "Update")
                        {
                            //string Doc_no = Session["CTLNo"].ToString();
                            string Doc_no = _ProdCataModel1.CTLNo;
                            //string Doc_date = Session["CTLDate"].ToString();
                            string Doc_date = _ProdCataModel1.CTLDate;
                            DataSet ds = _ProductCatalouge_ISERVICES.GetProdCatalogueDetails(CompID, BrchID, Doc_no, Doc_date, UserID, DocumentMenuId);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                _ProdCataModel1.CTLNo = ds.Tables[0].Rows[0]["cat_no"].ToString();
                                _ProdCataModel1.CTLDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["cat_dt"].ToString()).ToString("yyyy-MM-dd");

                                _ProdCataModel1.CustPros_type = ds.Tables[0].Rows[0]["cust_type"].ToString();
                                string CustPros_type = _ProdCataModel1.CustPros_type;
                                if (_ProdCataModel1.CustPros_type == "P")
                                {
                                    _ProdCataModel1.ProspectFromProduct = "Y";
                                }
                                else
                                {
                                    _ProdCataModel1.ProspectFromProduct = "N";
                                }
                                List<CustNameList> CustNameList2 = new List<CustNameList>();
                                dt = GetCustomerListtoEdit(_ProdCataModel1, CustPros_type);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    CustNameList CustName = new CustNameList();
                                    CustName.Cust_id = dr["cust_id"].ToString();
                                    CustName.Cust_name = dr["cust_name"].ToString();
                                    CustNameList2.Add(CustName);
                                }
                                CustNameList2.Insert(0, new CustNameList() { Cust_id = "0", Cust_name = "---Select---" });
                                _ProdCataModel1._CustNameList = CustNameList2;

                                _ProdCataModel1.CustID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                                _ProdCataModel1.CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();
                                _ProdCataModel1.Remarks = ds.Tables[0].Rows[0]["cat_rem"].ToString();
                                _ProdCataModel1.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                                _ProdCataModel1.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                _ProdCataModel1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                _ProdCataModel1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                _ProdCataModel1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                _ProdCataModel1.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                                _ProdCataModel1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                                _ProdCataModel1.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();
                                _ProdCataModel1.Status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                                create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                ViewBag.PCatItemDetails = ds.Tables[1];
                                

                                _ProdCataModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                                _ProdCataModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);//Cancelled
                            }
                            string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                            string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                            //Session["DocumentStatus"] = Statuscode;
                            _ProdCataModel1.DocumentStatus = Statuscode;

                            if (Statuscode != "D" && Statuscode != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[4];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _ProdCataModel1.Command != "Edit")
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
                                if (Statuscode == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _ProdCataModel1.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProdCataModel1.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProdCataModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ProdCataModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProdCataModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ProdCataModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ProdCataModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ProdCataModel1.BtnName = "BtnToDetailPage";

                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _ProdCataModel1.BtnName = "BtnRefresh";
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Session["DocumentStatus"] = "D";
                            _ProdCataModel1.DocumentStatus = "D";
                        }
                    }
                    if (ViewBag.AppLevel.Rows.Count == 0)
                    {
                        ViewBag.Approve = "Y";
                    }
                    if (TempData["ListFilterData"] == null)
                    {
                        _ProdCataModel1.ListFilterData1 = ListFilterData;
                    }
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ProductCatalouge/ProductCatalougeDetail.cshtml", _ProdCataModel1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

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
        public ActionResult ProdCatalogActionCommands(ProductCatalouge_Model _ProdCataModel, string Command)
        {

            try
            {/*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ProdCataModel.Delete == "Delete")
                {
                    Command = "Delete";
                    DeleteProdCataDetails(_ProdCataModel);
                }

                switch (Command)
                {
                    case "AddNew":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_ProdCataModel.CTLNo))
                                return RedirectToAction("ProductCatalougeDetail", new { doc_no = _ProdCataModel.CTLNo, doc_date = _ProdCataModel.CTLDate, ListFilterData = _ProdCataModel.ListFilterData1, WF_status = _ProdCataModel.WFStatus });
                            else
                                return RedirectToAction("AddProductCatalougeDetail", new { flag = "Delet"});
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("AddProductCatalougeDetail");
                    case "Save":
                        //_ProdCataModel.TransType = Command;
                        URLDetailModel URLModelSave = new URLDetailModel();
                        SavePRodCatalogDetails(_ProdCataModel,Command);
                        if (_ProdCataModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        //Session["Command"] = Command;
                        //Session["BtnName"] = "BtnSave";
                        _ProdCataModel.Command = Command;
                        _ProdCataModel.BtnName = "BtnSave";
                        URLModelSave.TransType = _ProdCataModel.TransType;
                        URLModelSave.DocNo = _ProdCataModel.CTLNo;
                        URLModelSave.DocDate = _ProdCataModel.CTLDate;
                        URLModelSave.BtnName = _ProdCataModel.BtnName;
                        URLModelSave.Command = _ProdCataModel.Command;
                        TempData["ModelData"] = _ProdCataModel;
                        TempData["ListFilterData"] = _ProdCataModel.ListFilterData1;
                        return RedirectToAction("ProductCatalougeDetail", URLModelSave);
                    case "Update":
                        URLDetailModel URLModelUpdate = new URLDetailModel();
                        SavePRodCatalogDetails(_ProdCataModel, Command);
                        //Session["Command"] = Command;
                        //Session["BtnName"] = "BtnSave";
                        if (_ProdCataModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        _ProdCataModel.Command = Command;
                        _ProdCataModel.BtnName = "BtnSave";
                        URLModelUpdate.TransType = _ProdCataModel.TransType;
                        URLModelUpdate.DocNo = _ProdCataModel.CTLNo;
                        URLModelUpdate.DocDate = _ProdCataModel.CTLDate;
                        URLModelUpdate.BtnName = _ProdCataModel.BtnName;
                        URLModelUpdate.Command = _ProdCataModel.Command;
                        TempData["ModelData"] = _ProdCataModel;
                        TempData["ListFilterData"] = _ProdCataModel.ListFilterData1;
                        return RedirectToAction("ProductCatalougeDetail", URLModelUpdate);
                    case "Approve":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("ProductCatalougeDetail", new { doc_no = _ProdCataModel.CTLNo, doc_date = _ProdCataModel.CTLDate, ListFilterData = _ProdCataModel.ListFilterData1, WF_status = _ProdCataModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        URLDetailModel URLModel = new URLDetailModel();
                        _ProdCataModel.TransType = Command;
                        ApproveProductCatalogDetails(_ProdCataModel, _ProdCataModel.CTLNo, _ProdCataModel.CTLDate, "Direct Approve", "", "", "","","");
                        //Session["Command"] = Command;//
                        //Session["BtnName"] = "BtnApprove";
                        _ProdCataModel.Command = Command;//
                        _ProdCataModel.BtnName = "BtnApprove";
                        URLModel.TransType = _ProdCataModel.TransType;
                        URLModel.DocNo = _ProdCataModel.CTLNo;
                        URLModel.DocDate = _ProdCataModel.CTLDate;
                        URLModel.BtnName = _ProdCataModel.BtnName;
                        URLModel.Command = _ProdCataModel.Command;
                        TempData["ModelData"] = _ProdCataModel;
                        TempData["ListFilterData"] = _ProdCataModel.ListFilterData1;
                        return RedirectToAction("ProductCatalougeDetail", URLModel);
                    case "Edit":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("ProductCatalougeDetail", new { doc_no = _ProdCataModel.CTLNo, doc_date = _ProdCataModel.CTLDate, ListFilterData = _ProdCataModel.ListFilterData1, WF_status = _ProdCataModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        URLDetailModel URLModelEdit = new URLDetailModel();
                        _ProdCataModel.Message = "New";
                        _ProdCataModel.Command = Command;
                        _ProdCataModel.TransType = "Update";
                        _ProdCataModel.BtnName = "BtnEdit";
                        _ProdCataModel.DocumentStatus = "D";
                        TempData["ModelData"] = _ProdCataModel;
                        URLModelEdit.TransType = _ProdCataModel.TransType;
                        URLModelEdit.DocNo = _ProdCataModel.CTLNo;
                        URLModelEdit.DocDate = _ProdCataModel.CTLDate;
                        URLModelEdit.BtnName = _ProdCataModel.BtnName;
                        URLModelEdit.Command = _ProdCataModel.Command;
                        //    Session["Message"] = "New";
                        //    Session["Command"] = Command;
                        //    Session["TransType"] = "Update";
                        //    Session["BtnName"] = "BtnEdit";
                        //Session["DocumentStatus"] = "D";
                        TempData["ListFilterData"] = _ProdCataModel.ListFilterData1;
                        return RedirectToAction("ProductCatalougeDetail", URLModelEdit);
                    case "Delete":
                        ProductCatalouge_Model _ProdCataModelDelete = new ProductCatalouge_Model();
                        _ProdCataModelDelete.Message = "Deleted";
                        _ProdCataModelDelete.Command = "Refresh";
                        _ProdCataModelDelete.TransType = "New";
                        _ProdCataModelDelete.BtnName = "BtnRefresh";
                        TempData["ModelData"] = _ProdCataModelDelete;
                        //Session["Command"] = "Refresh";
                        //Session["TransType"] = "New";
                        //Session["BtnName"] = "BtnRefresh";
                        TempData["ListFilterData"] = _ProdCataModel.ListFilterData1;
                        return RedirectToAction("ProductCatalougeDetail");
                    case "Refresh":
                        ProductCatalouge_Model _ProdCataModelRefresh = new ProductCatalouge_Model();
                        _ProdCataModelRefresh.Message = "New";
                        _ProdCataModelRefresh.Command = Command;
                        _ProdCataModelRefresh.TransType = "New";
                        _ProdCataModelRefresh.BtnName = "BtnRefresh";
                        TempData["ModelData"] = _ProdCataModelRefresh;
                        //Session["Message"] = "New";
                        //Session["Command"] = Command;
                        //Session["TransType"] = "New";
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["CTLNo"] = null;
                        //Session["CTLDate"] = null;
                        TempData["ListFilterData"] = _ProdCataModel.ListFilterData1;
                        return RedirectToAction("ProductCatalougeDetail");
                    case "BacktoList":
                        //Session["Message"] = "New";
                        //Session["CTLNo"] = null;
                        //Session["CTLDate"] = null;
                        TempData["WF_status"] = _ProdCataModel.WF_status1;
                        TempData["ListFilterData"] = _ProdCataModel.ListFilterData1;
                        return RedirectToAction("ProductCatalouge");
                    case "Print":
                        return GenratePdfFile(_ProdCataModel,Command);
                }
                return RedirectToAction("ProductCatalougeDetail");
             }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private ActionResult SavePRodCatalogDetails(ProductCatalouge_Model _ProdCataModel, string Command)
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
                string catal_No = _ProdCataModel.CTLNo;
                string catal_Date =_ProdCataModel.CTLDate;
                
                string cust_Id = _ProdCataModel.CustID;
                string remark = _ProdCataModel.Remarks;
                string Cust_type = _ProdCataModel.CustPros_type;
                string create_id = Session["UserId"].ToString();
                string Transtype = Command;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

               
                DataTable CatalogItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("item_grp_id", typeof(string));
                dtItem.Columns.Add("prf_id", typeof(string));
                dtItem.Columns.Add("item_ref_no", typeof(string)); 
                dtItem.Columns.Add("item_tech_spec", typeof(string));
                dtItem.Columns.Add("veh_id", typeof(string));
                dtItem.Columns.Add("model_no", typeof(string));
                dtItem.Columns.Add("veh_oem_no", typeof(string));
                dtItem.Columns.Add("item_cat_no", typeof(string));

                JArray jObject = JArray.Parse(_ProdCataModel.Itemdetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["uom_id"] = jObject[i]["uom_id"].ToString();
                    dtrowLines["item_grp_id"] = jObject[i]["item_grp_id"];
                    dtrowLines["prf_id"] = jObject[i]["prf_id"].ToString();
                    dtrowLines["item_ref_no"] = jObject[i]["item_ref_no"].ToString();
                    dtrowLines["item_tech_spec"] = jObject[i]["item_tech_spec"].ToString();
                    dtrowLines["veh_id"] = jObject[i]["veh_id"].ToString();
                    dtrowLines["model_no"] = jObject[i]["model_no"].ToString();
                    dtrowLines["veh_oem_no"] = jObject[i]["veh_oem_no"].ToString();
                    dtrowLines["item_cat_no"] = jObject[i]["item_cat_no"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
               
                CatalogItemDetail = dtItem;
                string SaveMessage = "";
                //DataTable DtblItemDetail = new DataTable();
                //DtblItemDetail = ToDtblItemDetail(_ProdCataModel.Itemdetails);
                SaveMessage = _ProductCatalouge_ISERVICES.InsertUpdateProdCatalogDetail(Convert.ToInt32(CompID),Convert.ToInt32(BrchID), catal_No,catal_Date,cust_Id,remark,create_id, Transtype, DocumentMenuId,mac_id, CatalogItemDetail, Cust_type);
                string CTLNo = SaveMessage.Split(',')[1].Trim();
                string Message = SaveMessage.Split(',')[0].Trim();
                string CTLDate = SaveMessage.Split(',')[2].Trim();
                if (Message == "Data_Not_Found")
                {
                    ViewBag.MenuPageName = getDocumentName();
                    _ProdCataModel.Title = title;
                    var msg = Message.Replace("_", " ") + " " + SaveMessage.Split(',')[1]+" in "+ _ProdCataModel.Title;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _ProdCataModel.Message = Message.Split(',')[0].Replace("_", "");
                    return RedirectToAction("ProductCatalougeDetail");
                }
                if (SaveMessage != null)
                {        
                    _ProdCataModel.TransType = "Update";
                }

                if (!string.IsNullOrEmpty(CTLNo))
                {
                    //Session["CTLNo"] = CTLNo;
                    _ProdCataModel.CTLNo = CTLNo;
                }
                if (Message == "Save" || Message == "Update")
                {
                    //Session["Message"] = "Save";
                    _ProdCataModel.Message = "Save";
                }
                if (!string.IsNullOrEmpty(CTLDate))
                {
                    //Session["CTLDate"] = CTLDate;
                    _ProdCataModel.CTLDate = CTLDate;
                }
                return RedirectToAction("ProductCatalougeDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        private DataTable GetCustomerListtoEdit(ProductCatalouge_Model _ProdCataModel,string CustPros_type)
        {
            string CustomerName = string.Empty;
            //Dictionary<string, string> CustListPC = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_ProdCataModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _ProdCataModel.CustName;
                }


                DataTable dt = _ProductCatalouge_ISERVICES.GetCustomerListtoEdit(Comp_ID, Br_ID, CustomerName, CustPros_type);
                //DataTable dt = _CustomerPriceList_ISERVICES.GetCustPriceGrpDAL(Comp_ID);
                return dt;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
            
        }
        public ActionResult GetAutoCompleteSearchCustList(ProductCatalouge_Model _ProdCataModel, string CustPros_type)
        {
            string CustomerName = string.Empty;
            Dictionary<string, string> CustListPC = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_ProdCataModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _ProdCataModel.CustName;
                }


                CustListPC = _ProductCatalouge_ISERVICES.GetCustomerListProdCata(Comp_ID, Br_ID, CustomerName,CustPros_type);

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustListPC.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemName(ProductCatalouge_Model _ProdCataModel)
        {
            JsonResult DataRows = null;
            string ItmName = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(_ProdCataModel.item_Name))
                    {
                        ItmName = "0";
                    }
                    else
                    {
                        ItmName = _ProdCataModel.item_Name;
                    }
                    DataSet ProductList = _ProductCatalouge_ISERVICES.BindItemName(Comp_ID, Br_ID, ItmName);
                    DataRow dr;
                    dr = ProductList.Tables[0].NewRow();
                    dr[0] = "0";
                    dr[1] = "---Select---";
                    dr[2] = "0";
                    ProductList.Tables[0].Rows.InsertAt(dr, 0);
                    DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public ActionResult GetAutoCompleteSearchGroupList(ProductCatalouge_Model _ProdCataModel)
        {
            string GroupName = string.Empty;
            Dictionary<string, string> GroupList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_ProdCataModel.ddlgroup_name))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _ProdCataModel.ddlgroup_name;
                }
                GroupList = _ProductCatalouge_ISERVICES.GetGroupList(Comp_ID, GroupName);
               
                List<GroupNameList> _GroupList = new List<GroupNameList>();
                foreach (var data in GroupList)
                {
                    GroupNameList _GroupDetail = new GroupNameList();
                    _GroupDetail.item_grp_id = data.Key;
                    _GroupDetail.ItemGroupChildNood = data.Value;
                    _GroupList.Add(_GroupDetail);
                }
                _ProdCataModel.GroupList = _GroupList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(GroupList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPortfolioListAuto(ProductCatalouge_Model _ProdCataModel)
        {
            JsonResult DataRows = null;
            string PortfolioName = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_ProdCataModel.PortfName))
                {
                    PortfolioName = "0";
                }
                else
                {
                    PortfolioName = _ProdCataModel.PortfName;
                }
                DataSet prf = _ProductCatalouge_ISERVICES.GetPortFolioList(Comp_ID, PortfolioName);
                DataRow dr;
                dr = prf.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                prf.Tables[0].Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(prf));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public ActionResult GetVehicleListAuto(ProductCatalouge_Model _ProdCataModel)
        {
            JsonResult DataRows = null;
            string VehicleName = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_ProdCataModel.VehicleName))
                {
                    VehicleName = "0";
                }
                else
                {
                    VehicleName = _ProdCataModel.VehicleName;
                }
                DataSet VehicleNameList = _ProductCatalouge_ISERVICES.GetVehicleList(Comp_ID, VehicleName);
                DataRow dr;
                dr = VehicleNameList.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                VehicleNameList.Tables[0].Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(VehicleNameList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        public ActionResult GetVehOEMNoAuto(ProductCatalouge_Model _ProdCataModel)
        {
            JsonResult DataRows = null;
            string VehOEM_No = string.Empty;
            string Comp_ID = string.Empty;
            
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                
                if (string.IsNullOrEmpty(_ProdCataModel.VehOEM_No))
                {
                    VehOEM_No = "0";
                }
                else
                {
                    VehOEM_No = _ProdCataModel.VehOEM_No;
                }
                DataSet Oem_No = _ProductCatalouge_ISERVICES.GetVehOEMNoDetail(Comp_ID, VehOEM_No);
                DataRow dr;
                dr = Oem_No.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                Oem_No.Tables[0].Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(Oem_No));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public ActionResult GetReferenceNoAuto(ProductCatalouge_Model _ProdCataModel)
        {
            JsonResult DataRows = null;
            string RefNo = string.Empty;
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                if (string.IsNullOrEmpty(_ProdCataModel.ReferenceNo))
                {
                    RefNo = "0";
                }
                else
                {
                    RefNo = _ProdCataModel.ReferenceNo;
                }
                DataSet refno = _ProductCatalouge_ISERVICES.GetRefNoDetail(Comp_ID, RefNo);
                DataRow dr;
                dr = refno.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                refno.Tables[0].Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(refno));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public ActionResult GetTechnSpecAuto(ProductCatalouge_Model _ProdCataModel)
        {
            JsonResult DataRows = null;
            string Techspec = string.Empty;
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                if (string.IsNullOrEmpty(_ProdCataModel.TechSpecific))
                {
                    Techspec = "0";
                }
                else
                {
                    Techspec = _ProdCataModel.TechSpecific;
                }
                DataSet techspeci = _ProductCatalouge_ISERVICES.GetTechSpecDetail(Comp_ID, Techspec);
                DataRow dr;
                dr = techspeci.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                techspeci.Tables[0].Rows.InsertAt(dr, 0);

                DataRows = Json(JsonConvert.SerializeObject(techspeci));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public ActionResult GetFilterItemDetail(string fltrvalue,string fltrtype)
        {
            JsonResult DataRows = null;
            
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
               DataSet FiltrItemData = _ProductCatalouge_ISERVICES.GetFilterItem(Comp_ID, fltrvalue, fltrtype);
                DataRows = Json(JsonConvert.SerializeObject(FiltrItemData));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public ActionResult SearchProdCataList(string CustID, string Fromdate, string Todate, string Status)
        {
            try
            {
                ProductCatalouge_Model _ProdCataModel = new ProductCatalouge_Model();
               // Session.Remove("WF_status");
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                ViewBag.PCatList = _ProductCatalouge_ISERVICES.GetSearchListOfProdCatalogDetails(CompID, BrchID, CustID, Fromdate, Todate, Status,"","","").Tables[0];
                //Session["CatalogSearch"] = "Catalog_Search";
                _ProdCataModel.CatalogSearch = "Catalog_Search";
                ViewBag.CatalogSearch = "Catalog_Search";
                ViewBag.VBRoleList = GetRoleList();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProductCatalougeList.cshtml", _ProdCataModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private string DeleteProdCataDetails(ProductCatalouge_Model _ProdCataModel)
        {
            try
            {
                string result = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                string doc_no = _ProdCataModel.CTLNo;
                string doc_date = _ProdCataModel.CTLDate;
                result = _ProductCatalouge_ISERVICES.DeleteProdCatlogDetails(CompID, BrchID, doc_no, doc_date);
                if (result == "Deleted")
                {
                    //Session["Message"] = "Deleted";
                    _ProdCataModel.Message = "Deleted";
                }
                return result;
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
            //Session["Message"] = "";
            ProductCatalouge_Model _ProdCataModel = new ProductCatalouge_Model();
            var a = TrancType.Split(',');
            _ProdCataModel.CTLNo = a[0].Trim();
            _ProdCataModel.CTLDate = a[1].Trim();
            var WF_status1 = a[2].Trim();
            _ProdCataModel.TransType = "Update";
            _ProdCataModel.WF_status1 = WF_status1;
            _ProdCataModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _ProdCataModel;
            TempData["WF_status1"] = WF_status1; ;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.DocNo = a[0].Trim();
            URLModel.DocDate = a[01].Trim();
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ProductCatalougeDetail", URLModel);
        }

        public ActionResult ApproveProdCatalogDetails(string CTLNo, string CTLDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1,string WF_status1)
        {
            URLDetailModel URLModel = new URLDetailModel();
            ProductCatalouge_Model _ProdCataModel = new ProductCatalouge_Model();
            if(A_Status != "Approve")
            {
                A_Status = "Approve";
            }
            ApproveProductCatalogDetails(_ProdCataModel,CTLNo, CTLDate, A_Status, A_Level, A_Remarks, "", ListFilterData1, WF_status1);
            URLModel.TransType = _ProdCataModel.TransType;
            URLModel.DocNo = _ProdCataModel.CTLNo;
            URLModel.DocDate = _ProdCataModel.CTLDate;
            URLModel.BtnName = _ProdCataModel.BtnName;
            URLModel.Command = _ProdCataModel.Command;
            TempData["ModelData"] = _ProdCataModel;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ProductCatalougeDetail", URLModel);
        }

        public ActionResult ApproveProductCatalogDetails(ProductCatalouge_Model _ProdCataModel,string CTLNo, string CTLDate, string A_Status, string A_Level, string A_Remarks, string Flag,string ListFilterData1,string WF_status1)
        {
            try
            {
                URLDetailModel URLModel = new URLDetailModel();
                //ProductCatalouge_Model _ProdCataModel = new ProductCatalouge_Model();
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
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string SaveMessage = _ProductCatalouge_ISERVICES.InsertProdCatalogApproveDetails(CTLNo, CTLDate, CompID, BrchID, DocumentMenuId, UserID, mac_id, A_Status, A_Level, A_Remarks, Flag);
                CTLNo = SaveMessage.Split(',')[1].Trim();
                string Message = SaveMessage.Split(',')[0].Trim();
                CTLDate = SaveMessage.Split(',')[2].Trim();
                if (!string.IsNullOrEmpty(CTLNo))
                {
                    //Session["CTLNo"] = CTLNo;
                    _ProdCataModel.CTLNo = CTLNo;
                }
                if (Message == "Approved")
                {
                    //Session["Message"] = "Approved";
                    _ProdCataModel.Message = "Approved";
                }
                if (!string.IsNullOrEmpty(CTLDate))
                {
                    //Session["CTLDate"] = CTLDate;
                    _ProdCataModel.CTLDate = CTLDate;
                }
                _ProdCataModel.Command = "Approve";//
                _ProdCataModel.BtnName = "BtnApprove";
                _ProdCataModel.TransType = "Update";
                if (WF_status1!=null && WF_status1.ToString() != "")
                {
                    _ProdCataModel.WF_status1 = WF_status1;
                }
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("ProductCatalougeDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        //public FileResult GenratePdfFile(ProductCatalouge_Model _ProdCataModel)
        //{
        //    return File(GetPdfData(_ProdCataModel.CTLNo, _ProdCataModel.CTLDate), "application/pdf", "ProductCatalogue.pdf");
        //}
        [HttpPost]
        public FileResult GenratePdfFile(ProductCatalouge_Model _ProdCataModel, string Command)
        {
            
            
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintItemImage", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowUOM", typeof(string));
            dt.Columns.Add("ItemAliasName", typeof(string));
            dt.Columns.Add("ShowOEMNo", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowProdTechSpec", typeof(string));
            dt.Columns.Add("CatalogueNo", typeof(string));
            dt.Columns.Add("ReferenceNo", typeof(string));
            dt.Columns.Add("VehicleName", typeof(string));
            dt.Columns.Add("ModelNo", typeof(string));
            /*Code start add by Hina on 01-09-2025*/
            dt.Columns.Add("LblProdDesc", typeof(string));
            dt.Columns.Add("LblUOM", typeof(string));
            dt.Columns.Add("LblItemAlias", typeof(string));
            dt.Columns.Add("LblOEMNo", typeof(string));
            dt.Columns.Add("LblProdTechDesc", typeof(string));
            dt.Columns.Add("LblProdTechSpec", typeof(string));
            dt.Columns.Add("LblCatalogueNo", typeof(string));
            dt.Columns.Add("LblRefNo", typeof(string));
            dt.Columns.Add("LblVehicleName", typeof(string));
            dt.Columns.Add("LblModelNo", typeof(string));
            /*Code end add by Hina on 01-09-2025*/
            DataRow dtr = dt.NewRow();
            dtr["PrintItemImage"] = _ProdCataModel.PrintItemImage;
            dtr["ShowProdDesc"] = _ProdCataModel.ShowProdDesc;
            dtr["ShowUOM"] = _ProdCataModel.ShowUOM;
            dtr["ItemAliasName"] = _ProdCataModel.ShowItemAliasName;
            dtr["ShowOEMNo"] = _ProdCataModel.ShowOEMNumber;
            dtr["ShowProdTechDesc"] = _ProdCataModel.ShowProdTechDesc;
            dtr["ShowProdTechSpec"] = _ProdCataModel.ShowProdTechSpec;
            dtr["CatalogueNo"] = _ProdCataModel.ShowCatalougeNumber;
            dtr["ReferenceNo"] = _ProdCataModel.ShowRefNumber;
            dtr["VehicleName"] = _ProdCataModel.ShowVehicleName;
            dtr["ModelNo"] = _ProdCataModel.ShowModelNumber;
            /*Code start add by Hina on 01-09-2025*/
            dtr["LblProdDesc"] = _ProdCataModel.LblProdDesc;
            dtr["LblUOM"] = _ProdCataModel.LblUOM;
            dtr["LblItemAlias"] = _ProdCataModel.LblItemAlias;
            dtr["LblOEMNo"] = _ProdCataModel.LblOEMNumber;
            dtr["LblProdTechDesc"] = _ProdCataModel.LblProdTechDesc;
            dtr["LblProdTechSpec"] = _ProdCataModel.LblProdTechSpec;
            dtr["LblCatalogueNo"] = _ProdCataModel.LblCatalougeNum;
            dtr["LblRefNo"] = _ProdCataModel.LblRefNumber;
            dtr["LblVehicleName"] = _ProdCataModel.LblVehicleName;
            dtr["LblModelNo"] = _ProdCataModel.LblModelNumber;
            /*Code end add by Hina on 01-09-2025*/
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            
            ViewBag.DocumentMenuId = _ProdCataModel.DocumentMenuId;
            return File(GetPdfData(_ProdCataModel.CTLNo, _ProdCataModel.CTLDate), "application/pdf", "ProductCatalogue.pdf");

        }
        public byte[] GetPdfData(string CTLNo, string CTLDate)
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
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet Details = _ProductCatalouge_ISERVICES.GetCatalogueDeatils(CompID, BrchID, CTLNo, CTLDate);
                //ViewBag.PageName = "PR";
                ViewBag.Title = "Product Catalouge";
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ProductCatalouge/ProductCatalougePrint.cshtml"));


                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
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
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
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
    }

}