using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.Purchase_Quotation;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.Purchase_Quotation;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.Purchase_Quotation
{
    public class PurchaseQuotationController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty, qt_no;         
        string DocumentMenuId = "105101120", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        PurchaseQuotation_ISERVICES _purchaseQuotation_ISERVICES;
        //PurchaseQuotation_Model _purchaseQuotation_Model;
        public PurchaseQuotationController(Common_IServices _Common_IServices, PurchaseQuotation_ISERVICES _purchaseQuotation_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._purchaseQuotation_ISERVICES = _purchaseQuotation_ISERVICES;
        }
        // GET: ApplicationLayer/PurchaseQuotation
        public ActionResult PurchaseQuotation(PQList_Model pQList_Model)
        {
            try
            {
                //PQList_Model pQList_Model = new PQList_Model();
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
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<SuppList> suppLists = new List<SuppList>();
                suppLists.Add(new SuppList { Supp_id = "0", Supp_name = "---Select---" });
                pQList_Model.suppLists = suppLists;

                List<statusLists> statusLists = new List<statusLists>();
                foreach(DataRow dr in ViewBag.StatusList.Rows)
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
                //Session["Command"] = "Add";
                //Session["TransType"] = "Save";
                //Session["BtnName"] = "BtnAddNew";
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    pQList_Model.WF_status = TempData["WF_status"].ToString();
                    //if (Session["WF_status"] != null)
                    if (pQList_Model.WF_status != null)
                    {
                        //wfstatus = Session["WF_status"].ToString();
                        wfstatus = pQList_Model.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                else
                {
                    //if (Session["WF_status"] != null)
                    if (pQList_Model.WF_status != null)
                    {
                        //wfstatus = Session["WF_status"].ToString();
                        wfstatus = pQList_Model.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                //ViewBag.MenuPageName = getDocumentName();
                //ViewBag.VBRoleList = GetRoleList();

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                #region Commanted by Nitesh 21-07-2025 For Accoding to User Change in from date
                #endregion
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
                        //DataSet ds1 = _purchaseQuotation_ISERVICES.GetSearchListOfPQDetails(CompID, BrchID, SupplierID, Fromdate, Todate, Status);
                        //ViewBag.PQList = ds1.Tables[0];
                        pQList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                        pQList_Model.PQ_FromDate = Fromdate;
                        pQList_Model.PQ_ToDate = Todate;
                       // pQList_Model.SuppID = SupplierID;
                        pQList_Model.SupplierID = SupplierID;
                        pQList_Model.PQ_status = Status;
                    }
                    //else
                    //{
                    //    pQList_Model.PQ_FromDate = startDate;
                    //    DataSet ds = _purchaseQuotation_ISERVICES.GetListOfPQDetails(CompID, BrchID, UserID, wfstatus, DocumentMenuId);
                    //    ViewBag.PQList = ds.Tables[0];
                    //    ViewBag.AttechmentDetails = ds.Tables[3];
                    //}
                }
                else
                {
                     // DataSet ds = _purchaseQuotation_ISERVICES.GetListOfPQDetails(CompID, BrchID, UserID, wfstatus, DocumentMenuId);
                    //    ViewBag.PQList = ds.Tables[0];
                    //    ViewBag.AttechmentDetails = ds.Tables[3];
                    pQList_Model.PQ_FromDate = startDate;
                 
                }
                GetAllData(pQList_Model);
                pQList_Model.title = title;
                //pQList_Model.PQ_FromDate = ds.Tables[1].Rows[0]["finstrdate"].ToString();
                //if (ds.Tables[2].Rows.Count > 0)
                //{
                //    Session["FinStDt"] = ds.Tables[2].Rows[0]["findate"].ToString();
                //}
                //Session["PQSearch"] = "0";
                pQList_Model.PQSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseQuotation/PurchaseQuotationList.cshtml", pQList_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }          
        }
        private void GetAllData(PQList_Model pQList_Model) /**Addded By  By Nitesh 30-03-2024**/
        {

            string SupplierName = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (string.IsNullOrEmpty(pQList_Model.SuppID))
            {
                SupplierName = "0";
            }
            else
            {
                SupplierName = pQList_Model.SuppID;
            }
         
         


            DataSet Data = _purchaseQuotation_ISERVICES.GetAllData(CompID, BrchID, SupplierName, UserID, pQList_Model.WF_status, DocumentMenuId, pQList_Model.SupplierID, pQList_Model.PQ_FromDate, pQList_Model.PQ_ToDate, pQList_Model.PQ_status);
            List<SuppList> SuppLists = new List<SuppList>();
           // dt = GetSuppNameList(pQList_Model);
            foreach (DataRow dr in Data.Tables[0].Rows)
            {
                SuppList _RAList = new SuppList();
                _RAList.Supp_id = dr["supp_id"].ToString();
                _RAList.Supp_name = dr["supp_name"].ToString();
                SuppLists.Add(_RAList);
            }
            SuppLists.Insert(0, new SuppList() { Supp_id = "0", Supp_name = "---Select---" });
            pQList_Model.suppLists = SuppLists;
            
            if(pQList_Model.SupplierID != null && pQList_Model.SupplierID !="")
            {
                pQList_Model.SuppID = pQList_Model.SupplierID;
            }
            ViewBag.PQList = Data.Tables[1];
          //  ViewBag.AttechmentDetails = ds.Tables[3];
        }
        private DataTable GetSuppNameList(PQList_Model pQList_Model)
        {
            try
            {
                string SupplierName = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(pQList_Model.SuppID))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = pQList_Model.SuppID;
                }
                DataTable dt = _purchaseQuotation_ISERVICES.GetSuppNameList(CompID, BrchID, SupplierName);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable GetSuppNameList(PurchaseQuotation_Model _purchaseQuotation_Model)
        {
            try
            {
                string SupplierName = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_purchaseQuotation_Model.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _purchaseQuotation_Model.SuppName;
                }
                DataTable dt = _purchaseQuotation_ISERVICES.GetSuppNameList(CompID, BrchID, SupplierName);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable GetProsSuppNameList(PurchaseQuotation_Model _purchaseQuotation_Model)
        {
            try
            {
                string SuppPros_type = string.Empty;
                string SupplierName = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_purchaseQuotation_Model.SuppID))
                {
                    SupplierName = "";
                }
                else
                {
                    SupplierName = _purchaseQuotation_Model.SuppID;
                }
                SuppPros_type = "P";
                DataTable dt1 = _purchaseQuotation_ISERVICES.GetProsSuppNameList(CompID, BrchID, SupplierName, SuppPros_type);
                return dt1;
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
        public ActionResult GetPurchaseQuotationsList(string docid, string status)
        {
            PQList_Model pQList_Model = new PQList_Model();
            //Session["WF_status"] = status;
            pQList_Model.WF_status = status;
            return RedirectToAction("PurchaseQuotation", pQList_Model);
        }
        [HttpPost]
        public ActionResult SearchPQList(string SupplierID,string Fromdate,string Todate,string Status)
        {
            try
            {
                PQList_Model pQList_Model = new PQList_Model();
                //Session.Remove("WF_status");
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds = _purchaseQuotation_ISERVICES.GetSearchListOfPQDetails(CompID, BrchID, SupplierID, Fromdate, Todate, Status);
                ViewBag.PQList = ds.Tables[0];
                //Session["PQSearch"] = "PQ_Search";
                pQList_Model.PQSearch = "PQ_Search";
                if (ds.Tables[2].Rows.Count > 0)
                {
                    //Session["FinStDt"] = ds.Tables[2].Rows[0]["findate"].ToString();
                    TempData["FinStDt"] = ds.Tables[2].Rows[0]["findate"].ToString();
                }
                
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPurchaseQuotationList.cshtml", pQList_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
          

        }
        public ActionResult AddPurchaseQuotationDetail(string ProspectFromPQ)
        {
            try
            {
                PurchaseQuotation_Model _purchaseQuotation_Model = new PurchaseQuotation_Model();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                
                CommonPageDetails();
                if (ProspectFromPQ != null)
                {
                    _purchaseQuotation_Model.Supp_Type = "P";
                }
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                DataTable dt = new DataTable();
                List<SuppList> SuppLists = new List<SuppList>();
                dt = GetSuppNameList(_purchaseQuotation_Model);
                foreach (DataRow dr in dt.Rows)
                {
                    SuppList _RAList = new SuppList();
                    _RAList.Supp_id = dr["supp_id"].ToString();
                    _RAList.Supp_name = dr["supp_name"].ToString();
                    SuppLists.Add(_RAList);
                }
                SuppLists.Insert(0, new SuppList() { Supp_id = "0", Supp_name = "---Select---" });
                _purchaseQuotation_Model.suppLists = SuppLists;

                List<ProsSuppList> ProsSuppList = new List<ProsSuppList>();
                DataTable dt1 = GetProsSuppNameList(_purchaseQuotation_Model);
                foreach (DataRow dr in dt.Rows)
                {
                    ProsSuppList _RAList = new ProsSuppList();
                    _RAList.Supp_id = dr["supp_id"].ToString();
                    _RAList.Supp_name = dr["supp_name"].ToString();
                    ProsSuppList.Add(_RAList);
                }
                ProsSuppList.Insert(0, new ProsSuppList() { Supp_id = "0", Supp_name = "---Select---" });
                _purchaseQuotation_Model.ProsSuppLists = ProsSuppList;

                ViewBag.DocumentMenuId = DocumentMenuId;
                _purchaseQuotation_Model.Message = "New";
                _purchaseQuotation_Model.Command = "Add";
                _purchaseQuotation_Model.AppStatus = "D";
                _purchaseQuotation_Model.TransType = "Save";
                _purchaseQuotation_Model.BtnName = "BtnAddNew";
                _purchaseQuotation_Model.PQNo = null;
                _purchaseQuotation_Model.PQDate = null;
                _purchaseQuotation_Model.DocumentStatus = "D";
                _purchaseQuotation_Model.SourceType = "D";
                ViewBag.DocumentStatus= "D";
                _purchaseQuotation_Model.ProspectFromRFQ = "N";
                _purchaseQuotation_Model.ProspectFromPQ = "Y";
                _purchaseQuotation_Model.ProspectFromQuot = "N";
                _purchaseQuotation_Model.title = title;
                _purchaseQuotation_Model.DocumentMenuId = DocumentMenuId;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    _purchaseQuotation_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                }
                if (TempData["FinStDt"] != null && TempData["FinStDt"].ToString()!="")
                {
                    ViewBag.FinstDt = TempData["FinStDt"];
                }
                ViewBag.ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                ViewBag.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                ViewBag.RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                {
                    PQList_Model pqModel = new PQList_Model();
                    TempData["Message"] = "Financial Year not Exist";
                    pqModel.Message = "Financial Year not Exist";
                    return RedirectToAction("PurchaseQuotation", pqModel);
                }
                /*End to chk Financial year exist or not*/
                return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseQuotation/PurchaseQuotationDetail.cshtml", _purchaseQuotation_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }      
        }
        public ActionResult PurchaseQuotationDetail(PurchaseQuotation_Model _purchaseQuotation_Model1, string PQ_Number, string PQ_Date, string TransType, string BtnName, string Command, string doc_no,string doc_date,string ListFilterData,string WF_status)
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
            //Return if financial year not defined
            var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    PQList_Model pqModel = new PQList_Model();
            //    pqModel.Message = "Financial Year not Exist";
            //    TempData["Message1"] = "Financial Year not Exist";
            //    _purchaseQuotation_Model1.Command = null;
            //    //return RedirectToAction("PurchaseQuotation", pqModel);
            //}
            /*Commented and Modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, doc_date) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                _purchaseQuotation_Model1.Command = null;
                ViewBag.Message = "TransNotAllow";
            }
            CommonPageDetails();
            try
            {
                var _purchaseQuotation_Model = TempData["ModelData"] as PurchaseQuotation_Model;
                if (_purchaseQuotation_Model != null)
                {
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    PQList_Model pqModel = new PQList_Model();
                    //    pqModel.Message = "Financial Year not Exist";
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //    _purchaseQuotation_Model.Command = null;
                    //    //return RedirectToAction("PurchaseQuotation", pqModel);
                    //}
                    /*Commented and Modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                    
                    DataTable dt = new DataTable();
                    List<SuppList> SuppLists = new List<SuppList>();
                    dt = GetSuppNameList(_purchaseQuotation_Model);
                    foreach (DataRow dr in dt.Rows)
                    {
                        SuppList _RAList = new SuppList();
                        _RAList.Supp_id = dr["supp_id"].ToString();
                        _RAList.Supp_name = dr["supp_name"].ToString();
                        SuppLists.Add(_RAList);
                    }
                    SuppLists.Insert(0, new SuppList() { Supp_id = "0", Supp_name = "---Select---" });
                    _purchaseQuotation_Model.suppLists = SuppLists;

                    List<ProsSuppList> ProsSuppList = new List<ProsSuppList>();
                    DataTable dt1 = GetProsSuppNameList(_purchaseQuotation_Model);
                    foreach (DataRow dr in dt1.Rows)
                    {
                        ProsSuppList _RAList = new ProsSuppList();
                        _RAList.Supp_id = dr["supp_id"].ToString();
                        _RAList.Supp_name = dr["supp_name"].ToString();
                        ProsSuppList.Add(_RAList);
                    }
                    ProsSuppList.Insert(0, new ProsSuppList() { Supp_id = "0", Supp_name = "---Select---" });
                    _purchaseQuotation_Model.ProsSuppLists = ProsSuppList;
                    _purchaseQuotation_Model.ListFilterData1 = ListFilterData;
                    _purchaseQuotation_Model.DocumentMenuId = DocumentMenuId;
                    if (doc_no != null && doc_date != null)
                    {
                        _purchaseQuotation_Model.PQNo = doc_no;
                        _purchaseQuotation_Model.PQDate = doc_date;
                        _purchaseQuotation_Model.TransType = "Update";
                        _purchaseQuotation_Model.Message = "New";
                        _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                    }
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    var create_id = "";
                    if (TempData["ListFilterData"] != null)
                    {
                        _purchaseQuotation_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if(TempData["WF_status1"]!=null && TempData["WF_status1"].ToString() != "")
                    {
                        _purchaseQuotation_Model.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (WF_status != null && WF_status != "")
                    {
                        _purchaseQuotation_Model.WF_status1 = WF_status;
                    }
                    if (PQ_Number != null|| PQ_Date!=null)
                    {
                        _purchaseQuotation_Model.PQNo = PQ_Number;
                        _purchaseQuotation_Model.PQDate = PQ_Date;
                    }
                    if (PQ_Number == null && _purchaseQuotation_Model.Message == "Used")
                    {
                        _purchaseQuotation_Model.PQNo = _purchaseQuotation_Model.PQ_No;
                        _purchaseQuotation_Model.PQDate = _purchaseQuotation_Model.PQ_Date;
                    }
                    //if (Session["PQNo"] != null && Session["TransType"]!=null)
                    if (_purchaseQuotation_Model.PQNo != null && _purchaseQuotation_Model.TransType != null)
                    {
                        //if (Session["TransType"].ToString() =="Update")
                        if (_purchaseQuotation_Model.TransType == "Update")
                        {

                            string Doc_no = _purchaseQuotation_Model.PQNo;
                            string Doc_date = _purchaseQuotation_Model.PQDate;
                            DataSet ds = _purchaseQuotation_ISERVICES.GetPurchaseQuotationDetails(Doc_no, Doc_date, CompID, BrchID, UserID);
                            ViewBag.UOMList = ds.Tables[14];
                            ViewBag.AttechmentDetails = ds.Tables[12];
                            ViewBag.ItemTaxDetailsList = ds.Tables[11];
                            ViewBag.SubItemDetails = ds.Tables[13];
                            if (ds.Tables[0].Rows.Count > 0)
                            {


                                // string RateDigit = "";
                                _purchaseQuotation_Model.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                                _purchaseQuotation_Model.SourceType = ds.Tables[0].Rows[0]["qt_type"].ToString();
                                _purchaseQuotation_Model.PQ_No = ds.Tables[0].Rows[0]["qt_no"].ToString();
                                _purchaseQuotation_Model.PQ_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["qt_dt"].ToString()).ToString("yyyy-MM-dd");
                                _purchaseQuotation_Model.Remarks = ds.Tables[0].Rows[0]["qt_rem"].ToString();
                                _purchaseQuotation_Model.Supp_Type = ds.Tables[0].Rows[0]["supp_type"].ToString();
                                _purchaseQuotation_Model.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                                _purchaseQuotation_Model.Currency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                                _purchaseQuotation_Model.Conv_Rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                                _purchaseQuotation_Model.Payterm = ds.Tables[0].Rows[0]["pay_term"].ToString();
                                _purchaseQuotation_Model.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                                _purchaseQuotation_Model.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                                _purchaseQuotation_Model.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                                _purchaseQuotation_Model.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                                _purchaseQuotation_Model.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["add_id"].ToString());
                                _purchaseQuotation_Model.ValidUpto = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                                _purchaseQuotation_Model.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                                _purchaseQuotation_Model.DiscAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["disc_amt"]).ToString(ValDigit);
                                _purchaseQuotation_Model.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                                _purchaseQuotation_Model.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                                _purchaseQuotation_Model.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                                _purchaseQuotation_Model.PONoAndDate = ds.Tables[0].Rows[0]["po_no_and_date"].ToString();
                                _purchaseQuotation_Model.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                                _purchaseQuotation_Model.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                _purchaseQuotation_Model.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                _purchaseQuotation_Model.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                                _purchaseQuotation_Model.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                _purchaseQuotation_Model.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                                _purchaseQuotation_Model.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                                _purchaseQuotation_Model.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();
                                _purchaseQuotation_Model.Status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                                _purchaseQuotation_Model.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString().Trim();
                                //_purchaseQuotation_Model.Itemdetails = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                                //_purchaseQuotation_Model.ItemTaxdetails = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                                //_purchaseQuotation_Model.ItemOCdetails = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                                ViewBag.ItemTaxDetails = ds.Tables[2];
                                ViewBag.OtherChargeDetails = ds.Tables[3];
                                _purchaseQuotation_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[9]);
                                _purchaseQuotation_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);//Cancelled
                                _purchaseQuotation_Model.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                                if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                                {
                                    _purchaseQuotation_Model.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                                }
                                ViewBag.ItemDelSchdetails = ds.Tables[4];
                                ViewBag.ItemTermsdetails = ds.Tables[5];
                                create_id = ds.Tables[0].Rows[0]["create_id"].ToString();   //

                                ViewBag.ItemDetailsList = ds.Tables[1];
                                ViewBag.QtyDigit = QtyDigit;
                            }
                            var RaiseOrder = ds.Tables[0].Rows[0]["raise_ord"].ToString();
                            var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                            if (Statuscode == "C")
                            {
                                _purchaseQuotation_Model.Cancelled = true;
                            }
                            else
                            {
                                _purchaseQuotation_Model.Cancelled = false;
                            }
                            //Session["DocumentStatus"] = Statuscode;
                            _purchaseQuotation_Model.DocumentStatus = Statuscode;
                            ViewBag.DocumentStatus = Statuscode;

                            if (Statuscode != "D" && Statuscode != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[9];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _purchaseQuotation_Model.Command != "Edit")
                            {

                                var sent_to = "";
                                var nextLevel = "";
                                if (ds.Tables[7].Rows.Count > 0)
                                {
                                    sent_to = ds.Tables[7].Rows[0]["sent_to"].ToString();
                                }

                                if (ds.Tables[8].Rows.Count > 0)
                                {
                                    nextLevel = ds.Tables[8].Rows[0]["nextlevel"].ToString().Trim();
                                }

                                if (Statuscode == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _purchaseQuotation_Model.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                                        }

                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                                    }


                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                                        }


                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _purchaseQuotation_Model.BtnName = "BtnToDetailPage";

                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _purchaseQuotation_Model.BtnName = "BtnRefresh";
                                    }
                                }
                            }

                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            if (RaiseOrder == "Y")/*Add by Hina sharma on 20-06-2025*/
                            {
                                _purchaseQuotation_Model.RaiseOrder = true;
                                _purchaseQuotation_Model.BtnName = "BtnRefresh";
                            }
                            else
                            {
                                _purchaseQuotation_Model.RaiseOrder = false;
                            }
                        }
                        else
                        {
                            //Session["DocumentStatus"] = "D";
                            _purchaseQuotation_Model.DocumentStatus = "D";
                            ViewBag.DocumentStatus = "D";
                        }
                    }
                    //if (Session["BtnName"].ToString() == "Refresh")
                    if (_purchaseQuotation_Model.BtnName == "Refresh")
                    {
                        //ViewBag.VBRoleList = GetRoleList();
                        //ViewBag.MenuPageName = getDocumentName();
                        _purchaseQuotation_Model.title = title;
                        return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseQuotation/PurchaseQuotationDetail.cshtml", _purchaseQuotation_Model);

                    }
                    else
                    {
                        _purchaseQuotation_Model.ProspectFromRFQ = "N";
                        _purchaseQuotation_Model.ProspectFromPQ = "Y";
                        _purchaseQuotation_Model.ProspectFromQuot = "N";
                        //Session["ProspectFromRFQ"] = "N";
                        //Session["ProspectFromPQ"] = "Y";
                        //Session["ProspectFromQuot"] = "N";
                        //ViewBag.VBRoleList = GetRoleList();
                        //ViewBag.MenuPageName = getDocumentName();
                        _purchaseQuotation_Model.title = title;
                        return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseQuotation/PurchaseQuotationDetail.cshtml", _purchaseQuotation_Model);
                    }
                }
                else
                {
                    //var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    //PurchaseQuotation_Model _purchaseQuotation_Model = new PurchaseQuotation_Model();
                    if (PQ_Number != null)
                    {
                        _purchaseQuotation_Model1.PQNo = PQ_Number;
                        _purchaseQuotation_Model1.PQDate = PQ_Date;
                    }
                    if (TransType != null)
                    {
                        _purchaseQuotation_Model1.TransType = TransType;
                    }
                    if (BtnName != null)
                    {
                        _purchaseQuotation_Model1.BtnName = BtnName;
                    }
                    if (Command != null)
                    {
                        _purchaseQuotation_Model1.Command = Command;
                    }
                    if (_purchaseQuotation_Model1.SourceType == null)
                    {
                        _purchaseQuotation_Model1.SourceType = "D";
                    }
                    DataTable dt = new DataTable();
                    List<SuppList> SuppLists = new List<SuppList>();
                    dt = GetSuppNameList(_purchaseQuotation_Model1);
                    foreach (DataRow dr in dt.Rows)
                    {
                        SuppList _RAList = new SuppList();
                        _RAList.Supp_id = dr["supp_id"].ToString();
                        _RAList.Supp_name = dr["supp_name"].ToString();
                        SuppLists.Add(_RAList);
                    }
                    SuppLists.Insert(0, new SuppList() { Supp_id = "0", Supp_name = "---Select---" });
                    _purchaseQuotation_Model1.suppLists = SuppLists;

                    List<ProsSuppList> ProsSuppList = new List<ProsSuppList>();
                    DataTable dt1 = GetProsSuppNameList(_purchaseQuotation_Model1);
                    foreach (DataRow dr in dt1.Rows)
                    {
                        ProsSuppList _RAList = new ProsSuppList();
                        _RAList.Supp_id = dr["supp_id"].ToString();
                        _RAList.Supp_name = dr["supp_name"].ToString();
                        ProsSuppList.Add(_RAList);
                    }
                    ProsSuppList.Insert(0, new ProsSuppList() { Supp_id = "0", Supp_name = "---Select---" });
                    _purchaseQuotation_Model1.ProsSuppLists = ProsSuppList;
                    _purchaseQuotation_Model1.ListFilterData1 = ListFilterData;
                    _purchaseQuotation_Model1.DocumentMenuId = DocumentMenuId;
                    if (doc_no != null && doc_date != null)
                    {
                        _purchaseQuotation_Model1.PQNo = doc_no;
                        _purchaseQuotation_Model1.PQDate = doc_date;
                        _purchaseQuotation_Model1.TransType = "Update";
                        _purchaseQuotation_Model1.Message = "New";
                        _purchaseQuotation_Model1.BtnName = "BtnToDetailPage";
                        //Session["PQNo"] = doc_no;
                        //Session["PQDate"] = doc_date;
                        //Session["TransType"] = "Update";
                        //Session["Message"] = "New";
                        //Session["BtnName"] = "BtnToDetailPage";
                    }
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    var create_id = "";
                    if (TempData["ListFilterData"] != null)
                    {
                        _purchaseQuotation_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _purchaseQuotation_Model1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (WF_status != null && WF_status != "")
                    {
                        _purchaseQuotation_Model1.WF_status1 = WF_status;
                    }
                    //if (Session["PQNo"] != null && Session["TransType"]!=null)
                    if (_purchaseQuotation_Model1.PQNo != null && _purchaseQuotation_Model1.TransType != null)
                    {
                        //if (Session["TransType"].ToString() =="Update")
                        if (_purchaseQuotation_Model1.TransType == "Update")
                        {

                            string Doc_no = _purchaseQuotation_Model1.PQNo;
                            string Doc_date = _purchaseQuotation_Model1.PQDate;
                            DataSet ds = _purchaseQuotation_ISERVICES.GetPurchaseQuotationDetails(Doc_no, Doc_date, CompID, BrchID, UserID);
                            ViewBag.UOMList = ds.Tables[14];
                            ViewBag.AttechmentDetails = ds.Tables[12];
                            ViewBag.ItemTaxDetailsList = ds.Tables[11];
                            ViewBag.SubItemDetails = ds.Tables[13];
                            if (ds.Tables[0].Rows.Count > 0)
                            {


                                // string RateDigit = "";
                                _purchaseQuotation_Model1.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                                _purchaseQuotation_Model1.SourceType = ds.Tables[0].Rows[0]["qt_type"].ToString();
                                _purchaseQuotation_Model1.PQ_No = ds.Tables[0].Rows[0]["qt_no"].ToString();
                                _purchaseQuotation_Model1.PQ_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["qt_dt"].ToString()).ToString("yyyy-MM-dd");
                                _purchaseQuotation_Model1.Remarks = ds.Tables[0].Rows[0]["qt_rem"].ToString();
                                _purchaseQuotation_Model1.Supp_Type = ds.Tables[0].Rows[0]["supp_type"].ToString();
                                _purchaseQuotation_Model1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                                _purchaseQuotation_Model1.Currency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                                _purchaseQuotation_Model1.Conv_Rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                                _purchaseQuotation_Model1.Payterm = ds.Tables[0].Rows[0]["pay_term"].ToString();
                                _purchaseQuotation_Model1.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                                _purchaseQuotation_Model1.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                                _purchaseQuotation_Model1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                                _purchaseQuotation_Model1.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                                _purchaseQuotation_Model1.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["add_id"].ToString());
                                _purchaseQuotation_Model1.ValidUpto = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                                _purchaseQuotation_Model1.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                                _purchaseQuotation_Model1.DiscAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["disc_amt"]).ToString(ValDigit);
                                _purchaseQuotation_Model1.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                                _purchaseQuotation_Model1.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                                _purchaseQuotation_Model1.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                                _purchaseQuotation_Model1.PONoAndDate = ds.Tables[0].Rows[0]["po_no_and_date"].ToString();
                                _purchaseQuotation_Model1.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                                _purchaseQuotation_Model1.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                _purchaseQuotation_Model1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                _purchaseQuotation_Model1.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                                _purchaseQuotation_Model1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                _purchaseQuotation_Model1.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                                _purchaseQuotation_Model1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                                _purchaseQuotation_Model1.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();
                                _purchaseQuotation_Model1.Status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                                _purchaseQuotation_Model1.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString().Trim();
                                
                                //_purchaseQuotation_Model.Itemdetails = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                                //_purchaseQuotation_Model.ItemTaxdetails = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                                //_purchaseQuotation_Model.ItemOCdetails = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                                ViewBag.ItemTaxDetails = ds.Tables[2];
                                ViewBag.OtherChargeDetails = ds.Tables[3];
                                _purchaseQuotation_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[9]);
                                _purchaseQuotation_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);//Cancelled
                                _purchaseQuotation_Model1.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                                if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                                {
                                    _purchaseQuotation_Model1.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                                }
                                ViewBag.ItemDelSchdetails = ds.Tables[4];
                                ViewBag.ItemTermsdetails = ds.Tables[5];
                                create_id = ds.Tables[0].Rows[0]["create_id"].ToString();   //

                                ViewBag.ItemDetailsList = ds.Tables[1];
                                ViewBag.QtyDigit = QtyDigit;
                            }
                            var RaiseOrder = ds.Tables[0].Rows[0]["raise_ord"].ToString();/*add by Hina on 20-06-2025*/
                            var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                            if (Statuscode == "C")
                            {
                                _purchaseQuotation_Model1.Cancelled = true;
                            }
                            else
                            {
                                _purchaseQuotation_Model1.Cancelled = false;
                            }
                            //Session["DocumentStatus"] = Statuscode;
                            _purchaseQuotation_Model1.DocumentStatus = Statuscode;
                            ViewBag.DocumentStatus = Statuscode;

                            if (Statuscode != "D" && Statuscode != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[9];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _purchaseQuotation_Model1.Command != "Edit")
                            {

                                var sent_to = "";
                                var nextLevel = "";
                                if (ds.Tables[7].Rows.Count > 0)
                                {
                                    sent_to = ds.Tables[7].Rows[0]["sent_to"].ToString();
                                }

                                if (ds.Tables[8].Rows.Count > 0)
                                {
                                    nextLevel = ds.Tables[8].Rows[0]["nextlevel"].ToString().Trim();
                                }

                                if (Statuscode == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _purchaseQuotation_Model1.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _purchaseQuotation_Model1.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _purchaseQuotation_Model1.BtnName = "BtnToDetailPage";
                                        }

                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _purchaseQuotation_Model1.BtnName = "BtnToDetailPage";
                                    }


                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _purchaseQuotation_Model1.BtnName = "BtnToDetailPage";
                                        }


                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _purchaseQuotation_Model1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _purchaseQuotation_Model1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _purchaseQuotation_Model1.BtnName = "BtnToDetailPage";

                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _purchaseQuotation_Model1.BtnName = "BtnRefresh";
                                    }
                                }
                            }

                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            //else
                            //{
                            //    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                            //    if (TempData["Message1"] != null)
                            //    { if (ViewBag.Approve == "Y")
                            //        {
                            //            ViewBag.ForwardEnbl = "N";
                            //        }
                            //        else
                            //        {
                            //            if (ViewBag.Approve == "N")
                            //            {
                            //                ViewBag.ForwardEnbl = "Y";
                            //            }
                            //            else
                            //            {
                            //                if (create_id == UserID)
                            //                {
                            //                    ViewBag.Approve = "Y";
                            //                    ViewBag.ForwardEnbl = "N";
                            //                }
                            //            }

                            //            //ViewBag.ForwardEnbl = "Y";
                            //        }

                            //        ViewBag.Message = TempData["Message1"];
                            //    }
                            //    /*End to chk Financial year exist or not*/
                            //}
                            if (RaiseOrder == "Y")/*add by Hina on 20-06-2025*/
                            {
                                _purchaseQuotation_Model1.RaiseOrder = true;
                                _purchaseQuotation_Model1.BtnName = "BtnRefresh";
                            }
                            else
                            {
                                _purchaseQuotation_Model1.RaiseOrder = false;
                            }
                        }
                        else
                        {
                            //Session["DocumentStatus"] = "D";
                            _purchaseQuotation_Model1.DocumentStatus = "D";
                            ViewBag.DocumentStatus = "D";
                        }
                    }
                    //if (Session["BtnName"].ToString() == "Refresh")
                    if (_purchaseQuotation_Model1.BtnName == "Refresh")
                    {
                        _purchaseQuotation_Model1.BtnName = "BtnRefresh";
                        //ViewBag.VBRoleList = GetRoleList();
                        //ViewBag.MenuPageName = getDocumentName();
                        _purchaseQuotation_Model1.title = title;

                        return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseQuotation/PurchaseQuotationDetail.cshtml", _purchaseQuotation_Model1);

                    }
                    else
                    {
                        _purchaseQuotation_Model1.ProspectFromRFQ = "N";
                        _purchaseQuotation_Model1.ProspectFromPQ = "Y";
                        _purchaseQuotation_Model1.ProspectFromQuot = "N";
                        //Session["ProspectFromRFQ"] = "N";
                        //Session["ProspectFromPQ"] = "Y";
                        //Session["ProspectFromQuot"] = "N";
                        //ViewBag.VBRoleList = GetRoleList();
                        //ViewBag.MenuPageName = getDocumentName();
                        _purchaseQuotation_Model1.title = title;
                        return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseQuotation/PurchaseQuotationDetail.cshtml", _purchaseQuotation_Model1);
                    }
                }              
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            PurchaseQuotation_Model _purchaseQuotation_Model = new PurchaseQuotation_Model();
            //Session["Message"] = "";
            var a = TrancType.Split(',');
            _purchaseQuotation_Model.PQNo = a[0].Trim();
            _purchaseQuotation_Model.PQDate = a[1].Trim();
            _purchaseQuotation_Model.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
            _purchaseQuotation_Model.Message = Mailerror;
            TempData["ModelData"] = _purchaseQuotation_Model;
            TempData["ListFilterData"] = ListFilterData1;
            TempData["WF_status1"] = WF_status1;
            // return RedirectToAction("PurchaseQuotationDetail");
            var PQ_Number = _purchaseQuotation_Model.PQNo;
            var PQ_Date = _purchaseQuotation_Model.PQDate;
            var TransType = _purchaseQuotation_Model.TransType;
            var BtnName = _purchaseQuotation_Model.BtnName;
            return RedirectToAction("PurchaseQuotationDetail", new { PQ_Number = PQ_Number, PQ_Date, TransType, BtnName });
        }
        [HttpPost]
        public JsonResult GetSuppRFQList(string Supp_id, string SuppPros_type)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet result = _purchaseQuotation_ISERVICES.GetSuppRfqList(Supp_id, Comp_ID, BrchID, SuppPros_type);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

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
        public JsonResult GetPRList()
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet result = _purchaseQuotation_ISERVICES.GetPRList( Comp_ID, BrchID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

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
        public ActionResult AddRfqOrPRItemDetailForQtsn( string doc_no,string Doc_date,string Flag)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }                              

                DataSet result = _purchaseQuotation_ISERVICES.AddRfqOrPRItemDetailForQtsn(CompID, BrchID, doc_no, Doc_date, Flag);                
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        [HttpPost]
        public JsonResult GetSuppAddrDetail(string Supp_id,string SuppPros_type)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet result = _purchaseQuotation_ISERVICES.GetSuppAddrDetailDAL(Supp_id, Comp_ID, BrchID, SuppPros_type);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for(int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                PurchaseQuotationattch _PurchaseQuotationattch = new PurchaseQuotationattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                Guid gid = new Guid();
                gid = Guid.NewGuid();
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _PurchaseQuotationattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //string br_id = Session["BranchId"].ToString();
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _PurchaseQuotationattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _PurchaseQuotationattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _PurchaseQuotationattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
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
               
        public ActionResult PQDetailsActionCommands(PurchaseQuotation_Model _purchaseQuotation_Model,string Command)
        {
          
            try
            {
                /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_purchaseQuotation_Model.Delete == "Delete")
                {
                    Command = "Delete";
                    DeletePQDetails(_purchaseQuotation_Model);
                    TempData["ListFilterData"] = _purchaseQuotation_Model.ListFilterData1;
                }
                var PQ_Number = "";
                var PQ_Date = "";
                var TransType = "";
                var BtnName = "";
                switch (Command)
                {
                    case "AddNew":
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();

                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_purchaseQuotation_Model.PQ_No))
                                return RedirectToAction("PurchaseQuotationDetail", new
                                {
                                    PQ_Number = _purchaseQuotation_Model.PQ_No,
                                    PQ_Date = _purchaseQuotation_Model.PQ_Date,
                                    TransType = _purchaseQuotation_Model.TransType,
                                    BtnName = _purchaseQuotation_Model.BtnName,
                                    Command = _purchaseQuotation_Model.Command,
                                    doc_no = _purchaseQuotation_Model.PQ_No,
                                    doc_date = _purchaseQuotation_Model.PQ_Date,
                                    ListFilterData = _purchaseQuotation_Model.ListFilterData1,
                                    WF_status = _purchaseQuotation_Model.WFStatus
                                });
                            else
                                PQ_Number = null;
                                PQ_Date = null;
                                TransType= "Refresh";
                                BtnName = "BtnRefresh";
                                Command = "Refresh";
                                TempData["ModelData"] = null;
                                return (RedirectToAction("PurchaseQuotationDetail", new { PQ_Number = PQ_Number, PQ_Date, TransType, BtnName, Command }));


                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("AddPurchaseQuotationDetail");
                    case "Save":
                        _purchaseQuotation_Model.TransType = Command;
                        SavePQDetails(_purchaseQuotation_Model);
                        if (_purchaseQuotation_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        _purchaseQuotation_Model.Command = Command;
                        _purchaseQuotation_Model.BtnName = "BtnSave";
                        PQ_Number = _purchaseQuotation_Model.PQNo;
                        PQ_Date = _purchaseQuotation_Model.PQDate;
                        TransType = _purchaseQuotation_Model.TransType;
                        if (_purchaseQuotation_Model.Message == "DocModify")
                        {
                            
                            _purchaseQuotation_Model.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            DataTable dt = new DataTable();
                            List<SuppList> SuppLists = new List<SuppList>();
                            dt = GetSuppNameList(_purchaseQuotation_Model);
                            foreach (DataRow dr in dt.Rows)
                            {
                                SuppList _RAList = new SuppList();
                                _RAList.Supp_id = dr["supp_id"].ToString();
                                _RAList.Supp_name = dr["supp_name"].ToString();
                                SuppLists.Add(_RAList);
                            }
                            SuppLists.Insert(0, new SuppList() { Supp_id = "0", Supp_name = "---Select---" });
                            _purchaseQuotation_Model.suppLists = SuppLists;

                            List<ProsSuppList> ProsSuppList = new List<ProsSuppList>();
                            DataTable dt1 = GetProsSuppNameList(_purchaseQuotation_Model);
                            foreach (DataRow dr in dt1.Rows)
                            {
                                ProsSuppList _RAList = new ProsSuppList();
                                _RAList.Supp_id = dr["supp_id"].ToString();
                                _RAList.Supp_name = dr["supp_name"].ToString();
                                ProsSuppList.Add(_RAList);
                            }
                            ProsSuppList.Insert(0, new ProsSuppList() { Supp_id = "0", Supp_name = "---Select---" });
                            _purchaseQuotation_Model.ProsSuppLists = ProsSuppList;

                            _purchaseQuotation_Model.Address = _purchaseQuotation_Model.Address;
                            _purchaseQuotation_Model.GrVal = _purchaseQuotation_Model.GrVal;
                            _purchaseQuotation_Model.TaxAmt = _purchaseQuotation_Model.TaxAmt;
                            _purchaseQuotation_Model.OcAmt = _purchaseQuotation_Model.OcAmt;
                            _purchaseQuotation_Model.NetValSpec = _purchaseQuotation_Model.NetValSpec;

                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                            _purchaseQuotation_Model.SrcDocNo = _purchaseQuotation_Model.SrcDocNo;
                            _purchaseQuotation_Model.SrcDocDate = _purchaseQuotation_Model.SrcDocDate;
                            CommonPageDetails();
                            ViewBag.GstApplicable = ViewBag.GstApplicable;
                            ViewBag.ItemDetailsList = ViewData["ItemDetails"];
                            ViewBag.ItemDelSchdetails = ViewData["DelvScheDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];
                            ViewBag.ItemTaxDetailsList = ViewData["TaxDetails"];
                            
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemTermsdetails = ViewData["TrmAndConDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _purchaseQuotation_Model.BtnName = "BtnRefresh";
                            _purchaseQuotation_Model.Command = "Refresh";
                            _purchaseQuotation_Model.DocumentStatus = "D";
                            ViewBag.VBRoleList = GetRoleList();
                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            
                            return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseQuotation/PurchaseQuotationDetail.cshtml", _purchaseQuotation_Model);


                        }
                        else
                        {
                            BtnName = "BtnSave";
                            TempData["ModelData"] = _purchaseQuotation_Model;
                            TempData["ListFilterData"] = _purchaseQuotation_Model.ListFilterData1;
                            return (RedirectToAction("PurchaseQuotationDetail", new { PQ_Number = PQ_Number, PQ_Date, TransType, BtnName, Command }));

                        }

                    case "Update":
                        _purchaseQuotation_Model.TransType = Command;
                        if (_purchaseQuotation_Model.Cancelled == true)
                        {                      
                            ApproveOrCancelPQDetails(_purchaseQuotation_Model, _purchaseQuotation_Model.PQ_No, _purchaseQuotation_Model.PQ_Date, "Cancelled", "", "", "","","");
                            PQ_Number = _purchaseQuotation_Model.PQ_No;
                            PQ_Date = _purchaseQuotation_Model.PQ_Date;
                            TransType = _purchaseQuotation_Model.TransType;
                            _purchaseQuotation_Model.Message = "Cancelled";
                            BtnName = "BtnSave";
                            TempData["ListFilterData"] = _purchaseQuotation_Model.ListFilterData1;
                        }
                        else
                        {
                            SavePQDetails(_purchaseQuotation_Model);
                            if (_purchaseQuotation_Model.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            _purchaseQuotation_Model.Command = Command;
                            _purchaseQuotation_Model.BtnName = "BtnSave";
                            TempData["ModelData"] = _purchaseQuotation_Model;
                            TempData["ListFilterData"] = _purchaseQuotation_Model.ListFilterData1;
                            PQ_Number = _purchaseQuotation_Model.PQNo;
                            PQ_Date = _purchaseQuotation_Model.PQDate;
                            TransType = _purchaseQuotation_Model.TransType;
                            BtnName = "BtnSave";
                        }
                        _purchaseQuotation_Model.Command = Command;
                        _purchaseQuotation_Model.BtnName = "BtnSave";
                        TempData["ModelData"] = _purchaseQuotation_Model;
                        return( RedirectToAction("PurchaseQuotationDetail", new { PQ_Number = PQ_Number, PQ_Date, TransType, BtnName, Command }));
                    case "Approve":
                        /*start Add by Hina on 05-05-2025 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                            string PQDate = _purchaseQuotation_Model.PQ_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PQDate) == "TransNotAllow")
                        {
                            _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                            TempData["Message1"] = "TransNotAllow";
                            TempData["ModelData"] = _purchaseQuotation_Model;
                            return RedirectToAction("PurchaseQuotationDetail", new
                            {
                                PQ_Number = _purchaseQuotation_Model.PQ_No,
                                PQ_Date = _purchaseQuotation_Model.PQ_Date,
                                TransType = _purchaseQuotation_Model.TransType,
                                BtnName = _purchaseQuotation_Model.BtnName,
                                Command = _purchaseQuotation_Model.Command,
                                doc_no = _purchaseQuotation_Model.PQ_No,
                                doc_date = _purchaseQuotation_Model.PQ_Date,
                                ListFilterData = _purchaseQuotation_Model.ListFilterData1,
                                WF_status = _purchaseQuotation_Model.WFStatus
                            });
                        }
                        /*End to chk Financial year exist or not*/
                        _purchaseQuotation_Model.TransType = Command;                      
                        ApproveOrCancelPQDetails(_purchaseQuotation_Model, _purchaseQuotation_Model.PQ_No, _purchaseQuotation_Model.PQ_Date, "Direct Approve", "", "", "","","");
                        _purchaseQuotation_Model.Command = Command;//
                        _purchaseQuotation_Model.BtnName = "BtnApprove";
                        PQ_Number = _purchaseQuotation_Model.PQNo;
                        PQ_Date = _purchaseQuotation_Model.PQDate;
                        TransType = _purchaseQuotation_Model.TransType;
                        BtnName = "BtnApprove";
                        TempData["ModelData"] = _purchaseQuotation_Model;
                        TempData["ListFilterData"] = _purchaseQuotation_Model.ListFilterData1;
                        return ( RedirectToAction("PurchaseQuotationDetail", new { PQ_Number = PQ_Number, PQ_Date, TransType, BtnName, Command }));
                    case "Edit":
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();

                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        // {
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        string PQDate1 = _purchaseQuotation_Model.PQ_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PQDate1) == "TransNotAllow")
                        {
                            _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                            //TempData["Message"] = "Financial Year not Exist";
                            TempData["Message1"] = "TransNotAllow";
                            TempData["ModelData"] = _purchaseQuotation_Model;
                            return RedirectToAction("PurchaseQuotationDetail", new
                            {
                                PQ_Number = _purchaseQuotation_Model.PQ_No,
                                PQ_Date = _purchaseQuotation_Model.PQ_Date,
                                TransType = _purchaseQuotation_Model.TransType,
                                BtnName = _purchaseQuotation_Model.BtnName,
                                Command = _purchaseQuotation_Model.Command,
                                doc_no = _purchaseQuotation_Model.PQ_No,
                                doc_date = _purchaseQuotation_Model.PQ_Date,
                                ListFilterData = _purchaseQuotation_Model.ListFilterData1,
                                WF_status = _purchaseQuotation_Model.WFStatus
                            });
                        }
                        /*End to chk Financial year exist or not*/
                        if (CheckPOAgainstPQ(_purchaseQuotation_Model.PQ_No, _purchaseQuotation_Model.PQ_Date) == "Used")
                        {
                            //Session["Message"] = "Used";
                            _purchaseQuotation_Model.Message = "Used";
                            _purchaseQuotation_Model.TransType = "Update";
                            _purchaseQuotation_Model.Command = null;
                            _purchaseQuotation_Model.DocumentStatus = _purchaseQuotation_Model.Status;
                            _purchaseQuotation_Model.BtnName = "BtnToDetailPage";
                            TempData["ListFilterData"] = _purchaseQuotation_Model.ListFilterData1;
                            TempData["ModelData"] = _purchaseQuotation_Model;
                        }
                        else
                        {
                            _purchaseQuotation_Model.Message = "New";
                            _purchaseQuotation_Model.Command = Command;
                            _purchaseQuotation_Model.TransType = "Update";
                            _purchaseQuotation_Model.BtnName = "BtnEdit";
                            PQ_Number = _purchaseQuotation_Model.PQ_No;
                            PQ_Date = _purchaseQuotation_Model.PQ_Date;
                            TransType = "Update";
                            BtnName = "BtnEdit";
                            TempData["ListFilterData"] = _purchaseQuotation_Model.ListFilterData1;
                            TempData["ModelData"] = _purchaseQuotation_Model;
                        }                       
                        return( RedirectToAction("PurchaseQuotationDetail", new { PQ_Number = PQ_Number, PQ_Date, TransType, BtnName, Command }));
                    case "Print":
                        return GenratePdfFile(_purchaseQuotation_Model);
                    case "Delete":
                        PurchaseQuotation_Model _purchaseQuotation_Modeldelete = new PurchaseQuotation_Model();
                        if (_purchaseQuotation_Model.Message != null)
                        {
                            _purchaseQuotation_Modeldelete.Message = _purchaseQuotation_Model.Message;
                        }
                        _purchaseQuotation_Modeldelete.Command = "Refresh";
                        _purchaseQuotation_Modeldelete.TransType = "New";
                        _purchaseQuotation_Modeldelete.BtnName = "BtnRefresh";
                        TempData["ModelData"] = _purchaseQuotation_Modeldelete;
                        return RedirectToAction("PurchaseQuotationDetail");
                    case "Refresh":
                        PurchaseQuotation_Model _purchaseQuotation_ModelRefresh = new PurchaseQuotation_Model();
                        _purchaseQuotation_ModelRefresh.Message = "New";
                        _purchaseQuotation_ModelRefresh.Command = Command;
                        _purchaseQuotation_ModelRefresh.TransType = "New";
                        _purchaseQuotation_ModelRefresh.BtnName = "BtnRefresh";
                        TempData["ModelData"] = _purchaseQuotation_ModelRefresh;
                        TempData["ListFilterData"] = _purchaseQuotation_Model.ListFilterData1;
                        return RedirectToAction("PurchaseQuotationDetail");
                    case "BacktoList":
                        TempData["WF_status"] = _purchaseQuotation_Model.WF_status1;
                        TempData["ListFilterData"] = _purchaseQuotation_Model.ListFilterData1;
                        return RedirectToAction("PurchaseQuotation");

                }
                return RedirectToAction("PurchaseQuotationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public FileResult GenratePdfFile(PurchaseQuotation_Model _Model)
        {
            return File(GetPdfData(_Model.PQ_No, _Model.PQ_Date), "application/pdf", "PurchaseQuotation.pdf");
        }
        public byte[] GetPdfData(string PQ_No, string PQ_Date)
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
                DataSet Deatils = _purchaseQuotation_ISERVICES.GetPurchaseQuotationDeatils(CompID, BrchID, PQ_No, PQ_Date);
                ViewBag.PageName = "PQ";
                ViewBag.Title = "Purchase Quotation";
                ViewBag.Details = Deatils;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                //ViewBag.InvoiceTo = "Invoice to:";
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["qt_status"].ToString().Trim();
                ViewBag.Website = Deatils.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/PurchaseQuotation/PurchaseQuotationPrint.cshtml"));
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
        private string DeletePQDetails(PurchaseQuotation_Model _purchaseQuotation_Model)
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
                string doc_no = _purchaseQuotation_Model.PQ_No;
                string doc_date = _purchaseQuotation_Model.PQ_Date;
                result = _purchaseQuotation_ISERVICES.DeletePQDetails(CompID, BrchID, doc_no, doc_date);

                if (!string.IsNullOrEmpty(doc_no))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, doc_no1, Server);
                }

                if (result == "Deleted")
                {
                    //Session["Message"] = "Deleted";
                    _purchaseQuotation_Model.Message = "Deleted";
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
        private ActionResult SavePQDetails(PurchaseQuotation_Model _purchaseQuotation_Model)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            try
            {            
                SaveMessage = "";
                DataTable DtblHDetail = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblDeliSchDetail = new DataTable();
                DataTable DtblTermsDetail = new DataTable();
                DataTable Attachments = new DataTable();

                DtblHDetail = ToDtblHDetail(_purchaseQuotation_Model);
                DtblItemDetail = ToDtblItemDetail(_purchaseQuotation_Model.Itemdetails);
                DtblTaxDetail = ToDtblTaxDetail(_purchaseQuotation_Model.ItemTaxdetails); 
                ViewData["TaxDetails"] = ViewData["TaxDetails"];
                DtblOCDetail = ToDtblOCDetail(_purchaseQuotation_Model.ItemOCdetails);
                //ViewData["OCTaxDetails"] = ViewData["TaxDetails"];
                DtblDeliSchDetail = ToDtblDelSchDetail(_purchaseQuotation_Model.ItemDelSchdetails);
                DtblTermsDetail = ToDtblTermsDetail(_purchaseQuotation_Model.ItemTermsdetails);

                DataTable dtAttachment = new DataTable();
                var _PurchaseQuotationattch = TempData["ModelDataattch"] as PurchaseQuotationattch;
                TempData["ModelDataattch"] = null;
               
                if (_purchaseQuotation_Model.attatchmentdetail != null)
                {
                    if (_PurchaseQuotationattch != null)
                    {
                        if(_PurchaseQuotationattch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _PurchaseQuotationattch.AttachMentDetailItmStp as DataTable;
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
                        if (_purchaseQuotation_Model.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _purchaseQuotation_Model.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_purchaseQuotation_Model.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_purchaseQuotation_Model.PQ_No))
                            {
                                dtrowAttachment1["id"] = _purchaseQuotation_Model.PQ_No;
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
                    if (_purchaseQuotation_Model.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_purchaseQuotation_Model.PQ_No))
                            {
                                ItmCode = _purchaseQuotation_Model.PQ_No;
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
                                    if (drImgPath == fielpath.Replace("/",@"\"))
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
                    Attachments = dtAttachment;
                }

                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("qty", typeof(string));
                if (_purchaseQuotation_Model.SubItemDetailsDt != null)
                {
                    JArray jObject2 = JArray.Parse(_purchaseQuotation_Model.SubItemDetailsDt);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                    ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
                }

                /*------------------Sub Item end----------------------*/
                SaveMessage = _purchaseQuotation_ISERVICES.InsertPQTransactionDetails(DtblHDetail, DtblItemDetail, DtblTaxDetail, DtblOCDetail, DtblDeliSchDetail, DtblTermsDetail, dtSubItem, Attachments);
                if (SaveMessage == "DocModify")
                {
                    _purchaseQuotation_Model.Message = "DocModify";
                    _purchaseQuotation_Model.BtnName = "BtnRefresh";
                    _purchaseQuotation_Model.Command = "Refresh";
                    TempData["ModelData"] = _purchaseQuotation_Model;
                    return RedirectToAction("PurchaseQuotationDetail");
                }
                else
                {
                    string PQNo = SaveMessage.Split(',')[1].Trim();
                    string Message = SaveMessage.Split(',')[0].Trim();
                    string PQDate = SaveMessage.Split(',')[2].Trim();
                    if (Message == "DataNotFound")
                    {
                        var msg = "Data Not found" + PQNo;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _purchaseQuotation_Model.Message = Message;
                        return RedirectToAction("PurchaseQuotationDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_PurchaseQuotationattch != null)
                        {
                            if (_PurchaseQuotationattch.Guid != null)
                            {
                                Guid = _PurchaseQuotationattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, PQNo, _purchaseQuotation_Model.TransType, Attachments);

                        //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                        //if (Directory.Exists(sourcePath))
                        //{
                        //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrchID + Guid + "_" + "*");
                        //    foreach (string file in filePaths)
                        //    {
                        //        string[] items = file.Split('\\');
                        //        string ItemName = items[items.Length - 1];
                        //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                        //        foreach (DataRow dr in Attachments.Rows)
                        //        {
                        //            string DrItmNm = dr["file_name"].ToString();
                        //            if (ItemName == DrItmNm)
                        //            {
                        //                string PQNo1 = PQNo.Replace("/", "");
                        //                string img_nm = CompID + BrchID + PQNo1 + "_" + Path.GetFileName(DrItmNm).ToString();
                        //                string doc_path = Path.Combine(Server.MapPath("~/Attachment/" + PageName + "/"), img_nm);
                        //                string DocumentPath = Server.MapPath("~/Attachment/" + PageName + "/");
                        //                if (!Directory.Exists(DocumentPath))
                        //                {
                        //                    DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
                        //                }

                        //                System.IO.File.Move(file, doc_path);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    if (SaveMessage != null)
                    {
                        //Session["TransType"] = "Update";
                        _purchaseQuotation_Model.TransType = "Update";
                    }
                    if (!string.IsNullOrEmpty(PQNo))
                    {
                        //Session["PQNo"] = PQNo;
                        _purchaseQuotation_Model.PQNo = PQNo;
                    }
                    if (Message == "Save" || Message == "Update")
                    {
                        _purchaseQuotation_Model.Message = "Save";
                        _purchaseQuotation_Model.AttachMentDetailItmStp = null;
                        _purchaseQuotation_Model.Guid = null;
                        //Session["Message"] = "Save";
                        //Session["AttachMentDetailItmStp"] = null;
                        //Session["Guid"] = null;
                    }
                    if (!string.IsNullOrEmpty(PQDate))
                    {
                        //Session["PQDate"] = PQDate;
                        _purchaseQuotation_Model.PQDate = PQDate;
                    }
                    return RedirectToAction("PurchaseQuotationDetail");
                }
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_purchaseQuotation_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_purchaseQuotation_Model.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _purchaseQuotation_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID+ BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
            
        }
        public ActionResult ApprovePQDetails(string PQNo, string PQDate, string A_Status, string A_Level, string A_Remarks,string ListFilterData1, string WF_status1)
        {
            PurchaseQuotation_Model _purchaseQuotation_Model = new PurchaseQuotation_Model();
            _purchaseQuotation_Model.PQNo = PQNo;
            _purchaseQuotation_Model.PQDate = PQDate;
            if(A_Status !="Approve")
            {
                A_Status = "Approve";
            }
            //_purchaseQuotation_Model = PQDate;
            ApproveOrCancelPQDetails(_purchaseQuotation_Model, PQNo,  PQDate,  A_Status,  A_Level,  A_Remarks,"", ListFilterData1, WF_status1);
            //Send Alert on Email
            //_Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, PQNo, "AP", UserID, "");
            var PQ_Number = _purchaseQuotation_Model.PQNo;
            var PQ_Date = _purchaseQuotation_Model.PQDate;
            var TransType = _purchaseQuotation_Model.TransType;
            var BtnName = _purchaseQuotation_Model.BtnName;
            var Command = _purchaseQuotation_Model.Command;
            TempData["ListFilterData"] = ListFilterData1;
            TempData["WF_status1"] = WF_status1;
            return( RedirectToAction("PurchaseQuotationDetail", new { PQ_Number = PQ_Number, PQ_Date, TransType, BtnName, Command }));
        }

        public ActionResult ApproveOrCancelPQDetails(PurchaseQuotation_Model _purchaseQuotation_Model,string PQNo, string PQDate, string A_Status, string A_Level, string A_Remarks,string Flag,string ListFilterData1,string WF_status1)
        {
            try
            {
                //PurchaseQuotation_Model _purchaseQuotation_Model = new PurchaseQuotation_Model();
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
                string SaveMessage = _purchaseQuotation_ISERVICES.InsertPQApproveDetails(PQNo, PQDate, CompID, BrchID, DocumentMenuId, UserID, mac_id, A_Status, A_Level, A_Remarks, Flag);
                PQNo = SaveMessage.Split(',')[1].Trim();
                string Message = SaveMessage.Split(',')[0].Trim();
                PQDate = SaveMessage.Split(',')[2].Trim();
                if (!string.IsNullOrEmpty(PQNo))
                {
                    //Session["PQNo"] = PQNo;
                    _purchaseQuotation_Model.PQNo = PQNo;
                }
                if (Message == "Approved")
                {
                    //Added by Nidhi on 08-07-2025
                    try
                    {
                        string fileName = PQNo.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(PQNo, PQDate, fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, PQNo, "AP", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _purchaseQuotation_Model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                   // _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, PQNo, "AP", UserID, "");
                    //Session["Message"] = "Approved";
                    //_purchaseQuotation_Model.Message = "Approved";
                    _purchaseQuotation_Model.Message = _purchaseQuotation_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                if (Message == "Cancelled")
                {
                    //Added by Nidhi on 08-07-2025
                    try
                    {
                        string fileName = PQNo.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(PQNo, PQDate, fileName, DocumentMenuId,"C");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, PQNo, "C", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _purchaseQuotation_Model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    //_Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, PQNo, "C", UserID, "");
                    //Session["Message"] = "Cancelled";
                    //_purchaseQuotation_Model.Message = "Cancelled";
                    _purchaseQuotation_Model.Message = _purchaseQuotation_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                }
                if (!string.IsNullOrEmpty(PQDate))
                {
                    //Session["PQDate"] = PQDate;
                    _purchaseQuotation_Model.PQDate = PQDate;
                }
                _purchaseQuotation_Model.BtnName = "BtnApprove";
                _purchaseQuotation_Model.TransType = "Update";
                _purchaseQuotation_Model.Command = "Approve";
                TempData["ModelData"] = _purchaseQuotation_Model;
                TempData["ListFilterData"] = ListFilterData1;
                TempData["WF_status1"] = WF_status1;
                return RedirectToAction("PurchaseQuotationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            
        }
        private DataTable ToDtblHDetail(PurchaseQuotation_Model _purchaseQuotation_Model)
        {
            try
            {
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
                    BrchID = Session["BranchId"].ToString();
                }
                DataTable DtblHDetail = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("MenuDocumentId", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("qt_no", typeof(string));
                dtheader.Columns.Add("qt_dt", typeof(string));
                dtheader.Columns.Add("qt_type", typeof(string));
                dtheader.Columns.Add("RaiseOrder", typeof(string));
                dtheader.Columns.Add("src_doc_number", typeof(string));
                dtheader.Columns.Add("src_doc_date", typeof(string));
                dtheader.Columns.Add("supp_type", typeof(string));
                dtheader.Columns.Add("supp_id", typeof(string));
                dtheader.Columns.Add("curr_id", typeof(string));
                dtheader.Columns.Add("curr_rate", typeof(string));
                dtheader.Columns.Add("pay_term", typeof(string));
                dtheader.Columns.Add("valid_upto", typeof(string));
                dtheader.Columns.Add("qt_rem", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("disc_amt", typeof(string));
                dtheader.Columns.Add("tax_amt", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val_bs", typeof(string));
                dtheader.Columns.Add("net_val_spec", typeof(string));
                dtheader.Columns.Add("create_id", typeof(int));
                dtheader.Columns.Add("create_dt", typeof(string));
                dtheader.Columns.Add("qt_status", typeof(string));
                dtheader.Columns.Add("add_id", typeof(int));//
                dtheader.Columns.Add("mac_id", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _purchaseQuotation_Model.TransType;
                dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                dtrowHeader["comp_id"] = CompID;
                dtrowHeader["br_id"] = BrchID;
                dtrowHeader["qt_no"] = _purchaseQuotation_Model.PQ_No;
                dtrowHeader["qt_dt"] = _purchaseQuotation_Model.PQ_Date;
                dtrowHeader["qt_type"] = _purchaseQuotation_Model.SourceType; 
                string RaiseOrderflag = _purchaseQuotation_Model.RaiseOrder.ToString();
                if (RaiseOrderflag == "False")
                {
                    dtrowHeader["RaiseOrder"] = "N";
                }
                else
                {
                    dtrowHeader["RaiseOrder"] = "Y";
                }
               
                if (_purchaseQuotation_Model.SrcDocNo == "---Select---")
                {
                    dtrowHeader["src_doc_number"] = "";
                }
                else
                {
                    dtrowHeader["src_doc_number"] = _purchaseQuotation_Model.SrcDocNo;
                }

                dtrowHeader["src_doc_date"] = _purchaseQuotation_Model.SrcDocDate;
                dtrowHeader["supp_type"] = _purchaseQuotation_Model.Supp_Type;
                dtrowHeader["supp_id"] = _purchaseQuotation_Model.SuppID;
                dtrowHeader["curr_id"] = _purchaseQuotation_Model.Currency;
                dtrowHeader["curr_rate"] = _purchaseQuotation_Model.Conv_Rate;
                var Payterm = _purchaseQuotation_Model.Payterm;
                if(Payterm==null)
                {
                    dtrowHeader["pay_term"] = 0;
                }
                else
                {
                    dtrowHeader["pay_term"] = _purchaseQuotation_Model.Payterm;
                }
                
                dtrowHeader["valid_upto"] = _purchaseQuotation_Model.ValidUpto;
                dtrowHeader["qt_rem"] = _purchaseQuotation_Model.Remarks;
                dtrowHeader["gr_val"] = IsNull(_purchaseQuotation_Model.GrVal, "0");
                dtrowHeader["disc_amt"] = IsNull(_purchaseQuotation_Model.DiscAmt, "0");
                dtrowHeader["tax_amt"] = IsNull(_purchaseQuotation_Model.TaxAmt, "0");
                dtrowHeader["oc_amt"] = IsNull(_purchaseQuotation_Model.OcAmt, "0");
                dtrowHeader["net_val_bs"] = IsNull(_purchaseQuotation_Model.NetValSpec, "0");
                dtrowHeader["net_val_spec"] = _purchaseQuotation_Model.NetValSpec;
                dtrowHeader["create_id"] = UserID;
                dtrowHeader["create_dt"] = "";
                dtrowHeader["qt_status"] = IsNull(_purchaseQuotation_Model.Status, "D");
                dtrowHeader["add_id"] = _purchaseQuotation_Model.bill_add_id;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;


                return DtblHDetail;
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            

        }
        private string IsNull(string Str,string Str2)
        {          
            if (!string.IsNullOrEmpty(Str))
            {
            }        
            else            
                Str = Str2;            
            return Str;
        }
        private DataTable ToDtblItemDetail(string pQItemDetail)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("qt_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_disc_perc", typeof(string));
                dtItem.Columns.Add("item_disc_amt", typeof(string));
                dtItem.Columns.Add("item_disc_val", typeof(string));
                dtItem.Columns.Add("net_rate", typeof(string));
                dtItem.Columns.Add("item_gr_val", typeof(string));
                dtItem.Columns.Add("item_ass_val", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val_spec", typeof(string));
                dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("it_remarks", typeof(string));
                dtItem.Columns.Add("TaxExempted", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));

                JArray jObject = JArray.Parse(pQItemDetail);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                    dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                    dtrowLines["qt_qty"] = jObject[i]["qt_qty"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                    dtrowLines["item_disc_perc"] = jObject[i]["item_disc_perc"].ToString();
                    dtrowLines["item_disc_amt"] = jObject[i]["item_disc_amt"].ToString();
                    dtrowLines["item_disc_val"] = jObject[i]["item_disc_val"].ToString();
                    dtrowLines["net_rate"] = jObject[i]["net_rate"].ToString();
                    dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                    dtrowLines["item_ass_val"] = jObject[i]["item_ass_val"].ToString();
                    dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                    dtrowLines["item_net_val_spec"] = jObject[i]["item_net_val_spec"].ToString();
                    dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                    dtrowLines["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                    dtrowLines["TaxExempted"] = jObject[i]["TaxExempted"].ToString();
                    dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                    dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemDetail = dtItem;
               ViewData["ItemDetails"]= dtitemdetail(jObject);

                return DtblItemDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            

        }
        private DataTable ToDtblTaxDetail(string TaxDetails)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("tax_id", typeof(int));
                dtItem.Columns.Add("tax_rate", typeof(string));
                dtItem.Columns.Add("tax_val", typeof(string));
                dtItem.Columns.Add("tax_level", typeof(int));
                dtItem.Columns.Add("tax_apply_on", typeof(string));

                JArray jObject = JArray.Parse(TaxDetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                    dtrowLines["tax_id"] = jObject[i]["TaxID"].ToString();
                    dtrowLines["tax_rate"] = jObject[i]["TaxRate"].ToString();
                    dtrowLines["tax_val"] = jObject[i]["TaxValue"].ToString();
                    dtrowLines["tax_level"] = jObject[i]["TaxLevel"].ToString();
                    dtrowLines["tax_apply_on"] = jObject[i]["TaxApplyOn"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemTaxDetail = dtItem;
                ViewData["TaxDetails"] = dtTaxdetail(jObject);


                return DtblItemTaxDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
           
        }
        private DataTable ToDtblOCDetail(string OCDetails)
        {
            try
            {
                DataTable DtblItemOCDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("oc_id", typeof(int));
                dtItem.Columns.Add("oc_val", typeof(string));

                JArray jObject = JArray.Parse(OCDetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["oc_id"] = jObject[i]["OC_ID"].ToString();
                    dtrowLines["oc_val"] = jObject[i]["OCValue"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemOCDetail = dtItem;
                ViewData["OCDetails"] = dtOCdetail(jObject);


                return DtblItemOCDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblDelSchDetail(string DelSchDetails)
        {
            try
            {
                DataTable DtblItemDelSchDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("sch_date", typeof(string));
                dtItem.Columns.Add("delv_qty", typeof(string));

                JArray jObject = JArray.Parse(DelSchDetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["sch_date"] = jObject[i]["sch_date"].ToString();
                    dtrowLines["delv_qty"] = jObject[i]["delv_qty"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemDelSchDetail = dtItem;
                ViewData["DelvScheDetails"] = dtdeldetail(jObject);
                return DtblItemDelSchDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
          
        }
        private DataTable ToDtblTermsDetail(string TermsDetails)
        {
            try
            {
                DataTable DtblItemTermsDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("term_desc", typeof(string));
                dtItem.Columns.Add("sno", typeof(int));
                JArray jObject = JArray.Parse(TermsDetails);
                int sno = 1;
                if (jObject != null)
                {
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["term_desc"] = jObject[i]["term_desc"].ToString();
                        dtrowLines["sno"] = sno;
                        dtItem.Rows.Add(dtrowLines);
                        sno += 1;
                    }
                }
                DtblItemTermsDetail = dtItem;
                ViewData["TrmAndConDetails"] = DtblItemTermsDetail;
                return DtblItemTermsDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
         
        }

        public DataTable dtitemdetail(JArray jObject)
        {
           
               DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            //if(_purchaseQuotation_Model.SourceType != null)
            // {
            //     if (_purchaseQuotation_Model.SourceType != "D")
            //     {
            //         dtItem.Columns.Add("sub_item", typeof(string));
            //     }
            // }
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("qt_qty", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("item_disc_perc", typeof(string));
            dtItem.Columns.Add("item_disc_amt", typeof(string));
            dtItem.Columns.Add("item_disc_val", typeof(string));
            dtItem.Columns.Add("net_rate", typeof(string));
            dtItem.Columns.Add("item_gr_val", typeof(string));
            dtItem.Columns.Add("item_ass_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val_spec", typeof(string));
            dtItem.Columns.Add("item_net_val_bs", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("tmplt_id", typeof(string));

            //JArray jObject = JArray.Parse(RFQ_Model.Itemdetails);

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["uom_name"] = jObject[i]["UOMName"].ToString();
                dtrowLines["qt_qty"] = jObject[i]["qt_qty"].ToString();
                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                if(jObject[i]["item_disc_perc"].ToString()=="")
                {
                    dtrowLines["item_disc_perc"] = "0.00";
                }
                else
                {
                    dtrowLines["item_disc_perc"] = jObject[i]["item_disc_perc"].ToString();
                }
                // dtrowLines["item_disc_perc"] = jObject[i]["item_disc_perc"].ToString();
                if (jObject[i]["item_disc_amt"].ToString() == "")
                {
                    dtrowLines["item_disc_amt"] = "0.00";
                }
                else
                {
                    dtrowLines["item_disc_amt"] = jObject[i]["item_disc_amt"].ToString();
                }
                //dtrowLines["item_disc_amt"] = jObject[i]["item_disc_amt"].ToString();
                dtrowLines["item_disc_val"] = jObject[i]["item_disc_val"].ToString();
                dtrowLines["net_rate"] = jObject[i]["net_rate"].ToString();
                dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                dtrowLines["item_ass_val"] = jObject[i]["item_ass_val"].ToString();

                if (jObject[i]["item_tax_amt"].ToString() == "")
                {
                    dtrowLines["item_tax_amt"] = "0.00";
                }
                else
                {
                    dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                }
                if (jObject[i]["item_oc_amt"].ToString() == "")
                {
                    dtrowLines["item_oc_amt"] = "0.00";
                }
                else
                {
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                }

                //dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                //dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                dtrowLines["item_net_val_spec"] = jObject[i]["item_net_val_spec"].ToString();
                dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                dtrowLines["tmplt_id"] = "0";

                dtItem.Rows.Add(dtrowLines);

            }

            return dtItem;
        }

        public DataTable dtTaxdetail(JArray jObject)
        {
            DataTable dttax = new DataTable();

            dttax.Columns.Add("item_id", typeof(string));
            dttax.Columns.Add("tax_id", typeof(int));
            dttax.Columns.Add("tax_name", typeof(string));
            dttax.Columns.Add("tax_rate", typeof(string));
            dttax.Columns.Add("tax_val", typeof(string));
            dttax.Columns.Add("tax_level", typeof(int));
            dttax.Columns.Add("tax_apply_on", typeof(string));
            dttax.Columns.Add("tax_apply_Name", typeof(string));
            dttax.Columns.Add("item_tax_amt", typeof(string));

            //JArray jObject = JArray.Parse(RFQ_Model.Itemdetails);

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dttax.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["tax_id"] = jObject[i]["TaxID"].ToString();
                dtrowLines["tax_name"] = jObject[i]["TaxName"].ToString();
                dtrowLines["tax_rate"] = jObject[i]["TaxRate"].ToString();
                dtrowLines["tax_val"] = jObject[i]["TaxValue"].ToString();
                dtrowLines["tax_level"] = jObject[i]["TaxLevel"].ToString();
                dtrowLines["tax_apply_on"] = jObject[i]["TaxApplyOn"].ToString();
                dtrowLines["tax_apply_Name"] = jObject[i]["taxapplyname"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["TotalTaxAmount"].ToString();
                dttax.Rows.Add(dtrowLines);
            }

            return dttax;
        }
        public DataTable dtOCdetail(JArray jObject)
        {
            DataTable dtOC = new DataTable();

            dtOC.Columns.Add("oc_id", typeof(int));
            dtOC.Columns.Add("oc_name", typeof(string));
            dtOC.Columns.Add("curr_name", typeof(string));
            dtOC.Columns.Add("conv_rate", typeof(float));
            dtOC.Columns.Add("oc_val", typeof(string));
            dtOC.Columns.Add("OCValBs", typeof(string));
            dtOC.Columns.Add("tax_amt", typeof(string));
            dtOC.Columns.Add("total_amt", typeof(string));



            //JArray jObject = JArray.Parse(RFQ_Model.Itemdetails);

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtOC.NewRow();

                dtrowLines["oc_id"] = jObject[i]["OC_ID"].ToString();
                dtrowLines["oc_name"] = jObject[i]["OCName"].ToString();
                dtrowLines["curr_name"] = jObject[i]["OC_Curr"].ToString();
                dtrowLines["conv_rate"] = jObject[i]["OC_Conv"].ToString();
                dtrowLines["oc_val"] = jObject[i]["OCValue"].ToString();
                dtrowLines["OCValBs"] = jObject[i]["OC_AmtBs"].ToString();
                dtrowLines["tax_amt"] = jObject[i]["OC_TaxAmt"].ToString();
                dtrowLines["total_amt"] = jObject[i]["OC_TotlAmt"].ToString();

                dtOC.Rows.Add(dtrowLines);
            }

            return dtOC;
        }
        public DataTable dtdeldetail(JArray jObject)
        {
            DataTable dtDelShed = new DataTable();

            dtDelShed.Columns.Add("item_id", typeof(string));
            dtDelShed.Columns.Add("item_name", typeof(string));
            dtDelShed.Columns.Add("sch_date", typeof(string));
            dtDelShed.Columns.Add("delv_qty", typeof(float));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtDelShed.NewRow();

                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sch_date"] = jObject[i]["sch_date"].ToString();
                dtrowLines["delv_qty"] = jObject[i]["delv_qty"].ToString();
                dtDelShed.Rows.Add(dtrowLines);
            }
            return dtDelShed;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_name", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                if (jObject2[i]["sub_item_name"]!=null)
                {
                    dtrowItemdetails["sub_item_name"] = jObject2[i]["sub_item_name"].ToString();
                }
                else
                {
                    dtrowItemdetails["sub_item_name"] = "";
                }
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
        , string Flag, string Status, string QtNo, string Doc_no, string Doc_dt)
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
                DataTable dt = new DataTable();
                int QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
                if (Flag == "Quantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];

                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));

                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _purchaseQuotation_ISERVICES.GetSubItemDetailsAfterApprove(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    }
                }
                else
                {
                    dt.Columns.Add("item_id", typeof(string));
                    dt.Columns.Add("sub_item_id", typeof(string));
                    dt.Columns.Add("sub_item_name", typeof(string));
                    dt.Columns.Add("Qty", typeof(string));

                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    foreach (JObject item in arr.Children())//
                    {
                        DataRow dRow = dt.NewRow();
                        dRow["item_id"] = item.GetValue("item_id").ToString();
                        dRow["sub_item_id"] = item.GetValue("sub_item_id").ToString();
                        dRow["sub_item_name"] = item.GetValue("sub_item_name").ToString();
                        dRow["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));

                        dt.Rows.Add(dRow);
                    }
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    //_subitemPageName = "MTO",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y"

                };

                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag == "Quantity" ? Flag : "MTO";
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
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
        public string CheckPOAgainstPQ(string DocNo, string DocDate)
        {
            string str = "";
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
                DataSet Deatils = _purchaseQuotation_ISERVICES.CheckPOAgainstPQ(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            return str;
        }
        //Added by Nidhi on 08-07-2025
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if(!string.IsNullOrEmpty(mailattch))
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
    }
}