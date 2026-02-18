using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.MIS.ProcurementDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.ProcurementDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Newtonsoft.Json;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.MIS.ProcurementDetail
{
    public class ProcurementDetailController : Controller
    {
        string CompID, BrID, language = String.Empty;
        string DocumentMenuId = "105101155101", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ProcurementDetail_ISERVICES detail_ISERVICES;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public ProcurementDetailController(Common_IServices _Common_IServices, ProcurementDetail_ISERVICES detail_ISERVICES,
             GeneralLedger_ISERVICE _GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.detail_ISERVICES = detail_ISERVICES;
            this._GeneralLedger_ISERVICE = _GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/ProcurementDetail
        public ActionResult ProcurementDetail()
        {
            string CompID = string.Empty;
            string BrID = string.Empty;
            string userid = string.Empty;

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
                ViewBag.vb_br_id = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                userid = Session["userid"].ToString();
            }

            ProcurementDetail_Model _Model = new ProcurementDetail_Model();
            ViewBag.MenuPageName = getDocumentName();
            DateTime dtnow = DateTime.Now;
            // string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            //string FromDate = new DateTime(dtnow.Year, 4, 1).ToString("yyyy-MM-dd"); /*Modified Month  By Nitesh 05-12-2023 add Fincacial Year in From Date*/
            DataSet dttbl = new DataSet();
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                _Model.From_dt = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }
            string ToDate = dtnow.ToString("yyyy-MM-dd");
            //_Model.From_dt = FromDate;
            _Model.To_dt = ToDate;
            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(CompID, userid);
            ViewBag.br_list = br_list;
            GetAutoCompleteSearchSuppList(_Model);/*add by Hina on 15-11-2024 to multiselect dropdown*/
            GetItemPortfList(_Model);/*add by Hina on 15-11-2024 to multiselect dropdown*/
            GetItemGrpList(_Model);/*add by Hina on 15-11-2024 to multiselect dropdown*/
            //_Model.categoryLists = suppCategoryList();
            //_Model.portFolioLists = suppPortFolioLists();
            GetAllDropDownData(_Model);/**Get Data Supplier And List Table Data in One Procedure**/
            _Model.Title = title;
            GetAutoCompleteSearchHSN(_Model);
            ViewBag.flag = "Summary";
            return View("~/Areas/ApplicationLayer/Views/Procurement/MIS/ProcurementDetail/ProcurementDetail.cshtml", _Model);
        }
        private void GetAllDropDownData(ProcurementDetail_Model _Model)/*Created by Hina sharma on 16-11-2024 to multiselect dropdown*/
        {
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string SupplierName = string.Empty;
            string GroupName = string.Empty;
            string PortfolioName = string.Empty;
            string User_ID = string.Empty;
            
            
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
                User_ID = Session["UserId"].ToString();
            }
            if (string.IsNullOrEmpty(_Model.SuppName))
            {
                SupplierName = "0";
            }
            else
            {
                SupplierName = _Model.SuppName;
            }
            if (string.IsNullOrEmpty(_Model.ItemGroupName))
            {
                GroupName = "0";
            }
            else
            {
                GroupName = _Model.ItemGroupName;
            }
            if (string.IsNullOrEmpty(_Model.Item_PortFolio))
            {
                PortfolioName = "0";
            }
            else
            {
                PortfolioName = _Model.Item_PortFolio;
            }
            DataSet ds = detail_ISERVICES.GetAllDDLData(Comp_ID, Br_ID, SupplierName, GroupName, PortfolioName);
            //------Bind Supplier List------------
            List<SupplierName> _SuppList = new List<SupplierName>();
            foreach (DataRow data in ds.Tables[0].Rows)
            {
                SupplierName _SuppDetail = new SupplierName();
                _SuppDetail.supp_id = data["supp_id"].ToString();
                _SuppDetail.supp_name = data["supp_name"].ToString();
                _SuppList.Add(_SuppDetail);
            }
            //_SuppList.Insert(0, new SupplierName() { supp_id = "0", supp_name = "All" });
            _Model.SupplierNameList = _SuppList;

            //------Bind Supplier Category List------------
            List<SuppCategoryList> catgrylist = new List<SuppCategoryList>();

            foreach (DataRow data in ds.Tables[1].Rows)
            {
                SuppCategoryList list = new SuppCategoryList();
                list.Cat_id = data["setup_id"].ToString();
                list.Cat_val = data["setup_val"].ToString();
                catgrylist.Add(list);
            }
            //catgrylist.Insert(0, new SuppCategoryList() { Cat_id = "0", Cat_val = "---All---" });/*Commented by hina on 15-11-2024 to modify multile select value*/
            _Model.categoryLists = catgrylist;

            //------Bind Supplier Portfolio List------------
            List<SuppPortFolioList> portFolioLists = new List<SuppPortFolioList>();

            foreach (DataRow data in ds.Tables[2].Rows)
            {
                SuppPortFolioList custPortFolio = new SuppPortFolioList();
                custPortFolio.Portfolio_id = data["setup_id"].ToString();
                custPortFolio.Portfolio_val = data["setup_val"].ToString();
                portFolioLists.Add(custPortFolio);
            }
           // portFolioLists.Insert(0, new SuppPortFolioList() { Portfolio_id = "0", Portfolio_val = "---All---" });/*Commented by hina on 15-11-2024 to modify multile select value*/
            _Model.portFolioLists = portFolioLists;

            //------Bind Item Group List------------
            List<ItemGroupName> _ItemGroupList = new List<ItemGroupName>();
            foreach (DataRow data in ds.Tables[3].Rows)
            {
                ItemGroupName _ItemGroupDetail = new ItemGroupName();
                _ItemGroupDetail.Group_Id = data["item_grp_id"].ToString();
                _ItemGroupDetail.Group_Name = data["ItemGroupChildNood"].ToString();
                _ItemGroupList.Add(_ItemGroupDetail);
            }
            _Model.ItemGroupNameList = _ItemGroupList;

            //------Bind Item Portfolio List------------
            List<ItemPortfolio> _ItemPortfolioList = new List<ItemPortfolio>();
            foreach (DataRow data in ds.Tables[4].Rows)
            {
                ItemPortfolio _ItemPortfolioDetail = new ItemPortfolio();
                _ItemPortfolioDetail.Prf_Id = data["setup_id"].ToString();
                _ItemPortfolioDetail.Prf_Name = data["setup_val"].ToString();
                _ItemPortfolioList.Add(_ItemPortfolioDetail);
            }
            _Model.ItemPortfolioList = _ItemPortfolioList;

        }
        public ActionResult GetAutoCompleteSearchHSN(ProcurementDetail_Model _Model)
        {
            string HSNCode = string.Empty;
            Dictionary<string, string> HSNList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_Model.ddlhsncode))
                {
                    HSNCode = "0";
                }
                else
                {
                    HSNCode = _Model.ddlhsncode;
                }
                HSNList = detail_ISERVICES.ItemSetupHSNDAL(Comp_ID, HSNCode);

                List<HSNno> _HSNList = new List<HSNno>();
                foreach (var data in HSNList)
                {
                    HSNno _HsnDetail = new HSNno();
                    _HsnDetail.setup_id = data.Key;
                    _HsnDetail.setup_val = data.Value;
                    _HSNList.Add(_HsnDetail);
                }
                _Model.HSNList = _HSNList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(HSNList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProcurementDetailCommands(ProcurementDetail_Model _Model,string command)
        {
            try
            {
                if (_Model.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "CsvPrint":
                        return ProcurementDetailExporttoExcelDt(_Model);
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
        private DataSet GetFyList()
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_Id = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_Id = Session["BranchId"].ToString();
                }
                DataSet dt = _GeneralLedger_ISERVICE.Get_FYList(Comp_ID, Br_Id);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult showReportFromDashBoard(string purchaseBy, string P_type, string FromDt,string ToDt)
        {
            string CompID = string.Empty;
            string BrID = string.Empty;
            string userid = string.Empty;

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
                ViewBag.vb_br_id = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                userid = Session["userid"].ToString();
            }
            ProcurementDetail_Model _Model = new ProcurementDetail_Model();
            ViewBag.MenuPageName = getDocumentName();
            _Model.From_dt = FromDt;
            _Model.To_dt = ToDt;

            _Model.Inv_type = P_type;
            _Model.purchase_by = purchaseBy;
            //GetAutoCompleteSearchSuppList(_Model);/*add by Hina on 15-11-2024 to multiselect dropdown*/
            //_Model.categoryLists = suppCategoryList();
            //_Model.portFolioLists = suppPortFolioLists();
            GetAllDropDownData(_Model);/**Get Data Supplier And List Table Data in One Procedure**/
            _Model.Title = title;
            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(CompID, userid);
            ViewBag.br_list = br_list;
            GetAutoCompleteSearchHSN(_Model);//add by shubham maurya on 04-04-2025 
            return View("~/Areas/ApplicationLayer/Views/Procurement/MIS/ProcurementDetail/ProcurementDetail.cshtml", _Model);
        }
        public ActionResult GetAutoCompleteSearchSuppList(ProcurementDetail_Model _Model)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string SuppType = string.Empty;
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
                if (string.IsNullOrEmpty(_Model.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _Model.SuppName;
                }
                CustList = detail_ISERVICES.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _Model.SupplierNameList = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemGrpList(ProcurementDetail_Model queryParameters)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string GroupName = string.Empty;
            Dictionary<string, string> ItemGroupList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(queryParameters.GroupName))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.GroupName;
                }
                ItemGroupList = detail_ISERVICES.ItemGroupList(GroupName, Comp_ID);

                List<ItemGroupName> _ItemGroupList = new List<ItemGroupName>();
                foreach (var data in ItemGroupList)
                {
                    ItemGroupName _ItemGroupDetail = new ItemGroupName();
                    _ItemGroupDetail.Group_Id = data.Key;
                    _ItemGroupDetail.Group_Name = data.Value;
                    _ItemGroupList.Add(_ItemGroupDetail);
                }
                queryParameters.ItemGroupNameList = _ItemGroupList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemGroupList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemPortfList(ProcurementDetail_Model queryParameters)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string PortfolioName = string.Empty;
            Dictionary<string, string> ItemPortfolioList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(queryParameters.PortfolioName))
                {
                    PortfolioName = "0";
                }
                else
                {
                    PortfolioName = queryParameters.PortfolioName;
                }
                ItemPortfolioList = detail_ISERVICES.ItemPortfolioList(PortfolioName, Comp_ID);

                List<ItemPortfolio> _ItemPortfolioList = new List<ItemPortfolio>();
                foreach (var data in ItemPortfolioList)
                {
                    ItemPortfolio _ItemPortfolioDetail = new ItemPortfolio();
                    _ItemPortfolioDetail.Prf_Id = data.Key;
                    _ItemPortfolioDetail.Prf_Name = data.Value;
                    _ItemPortfolioList.Add(_ItemPortfolioDetail);
                }
                queryParameters.ItemPortfolioList = _ItemPortfolioList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemPortfolioList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public DataTable GetPurchaseDetailsData(Search_Parmeters search_Model)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                if (Session["CompId"] != null)
                {
                    search_Model.CompId = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    search_Model.BrId = Session["BranchId"].ToString();
                }
                ds = detail_ISERVICES.GetPrcFilteredReport(search_Model);
                if(search_Model.flag== "InvoiceWiseItem")
                {
                    ViewBag.InvoiceWiseItemTotal = ds.Tables[2];
                }
                if (search_Model.flag == "SupplierWiseInvoice")
                {
                    ViewBag.SupplierWiseInvoiceTotal = ds.Tables[1];
                }
                if(search_Model.flag== "SupplierWiseInvoiceItem")
                {
                    ViewBag.SupplierWiseInvoiceItemTotal = ds.Tables[1];
                }
                if (search_Model.flag == "ItemWiseInvoice")
                {
                    ViewBag.ItemWiseInvoiceTotal = ds.Tables[1];
                }
                //if(search_Model.flag== "ItemWiseSummery")
                if(search_Model.flag== "ItemGroupWiseItemSummery")
                {
                    ViewBag.ItemWiseSummeryTotal = ds.Tables[1];
                }
                dt = ds.Tables[0];

                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetPurchase_DetailsSummery(Search_model model,string PurchaseBy)
        {
            try
            {
                var model1 = model.search_prm;
                DataTable dt = new DataTable();// GetPurchaseDetailsData(model1);
                ViewBag.flag = model1.flag;
                TempData["SDFilter"] = "SDFilter";
                if (PurchaseBy == "I")
                {
                    if (model1.flag == "Detail")/* Added by Suraj Maurya on 17-09-2025 for dynamic tax columns */
                    {
                        if (Session["CompId"] != null)
                        {
                            model1.CompId = Session["CompId"].ToString();
                        }
                        if (Session["BranchId"] != null)
                        {
                            model1.BrId = Session["BranchId"].ToString();
                        }
                        ViewBag.TaxColumns = detail_ISERVICES.GetPrcDynamicTaxColumns(model1).Tables[0];
                        ViewBag.TaxColumnsJson = JsonConvert.SerializeObject(ViewBag.TaxColumns);
                        //string json2 = Json(JsonConvert.SerializeObject(ViewBag.TaxColumns), JsonRequestBehavior.AllowGet); ;
                        //ViewBag.TaxColumnsJson = json2;
                    }
                    ViewBag.PrcInvoiceWiseDetail = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_ProcurementDetailInvoiceWiseList.cshtml");
                }
                else if (PurchaseBy == "S")
                {
                    ViewBag.PrcSupplierWiseDetail = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_ProcurementDetailSupplierWiseList.cshtml");
                }
                else if (PurchaseBy == "T")
                {
                    ViewBag.PrcItemWiseDetail = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_ProcurementDetailItemWiseList.cshtml");
                }
                else if (PurchaseBy == "G")
                {
                    ViewBag.PrcItemGroupWiseDetail = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_ProcurementDetailItemGroupWiseList.cshtml");
                }
                else if (PurchaseBy == "B")
                {
                    if (model1.flag == "InterBranchPurchaseDetail")/* Added by Suraj Maurya on 20-09-2025 for dynamic tax columns */
                    {
                        if (Session["CompId"] != null)
                        {
                            model1.CompId = Session["CompId"].ToString();
                        }
                        if (Session["BranchId"] != null)
                        {
                            model1.BrId = Session["BranchId"].ToString();
                        }
                        ViewBag.TaxColumns = detail_ISERVICES.GetPrcDynamicTaxColumns(model1).Tables[0];
                        ViewBag.TaxColumnsJson = JsonConvert.SerializeObject(ViewBag.TaxColumns);
                    }
                    ViewBag.PrcInterBranchPurchaseWiseDetail = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_ProcurementDetailInterBranchPurchaseWiseList.cshtml");
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_ProcurementDetailInvoiceWiseList.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return PartialView("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }

        }

        /*****-------------------------------------Invoice Wise Deatils----------------------------------------****/
        public ActionResult GetInvoiceWiseItemDetail(Search_model model)
        {
            try
            {
                var model1 = model.search_prm;
                ViewBag.PrcInvoiceWiseItemDetail = GetPurchaseDetailsData(model1);
                ViewBag.modeldt = model1;
                //Session["proc_invdetails"]= "inv_details";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProcurementInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }
        }

        /*****-------------------------------------Invoice Wise Deatils End----------------------------------------****/

        /*****-------------------------------------Supplier Wise Deatils----------------------------------------****/
        public ActionResult GetSupplierWiseInvoice(Search_model model)
        {
            try
            {
                var model1 = model.search_prm;
                ViewBag.PrcSupplierWiseInvoiceDetail = GetPurchaseDetailsData(model1);
                ViewBag.modeldt = model1;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProcurementSupplierWiseInvoiceSummary.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }
        }
        public ActionResult GetSupplierWiseInvoiceItem(Search_model model)
        {
            try
            {
                var model1 = model.search_prm;
                ViewBag.PrcSupplierWiseInvoiceItemDetail = GetPurchaseDetailsData(model1);
                ViewBag.modeldt = model1;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProcurementSupplierWiseInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }
        }

        /*****-------------------------------------Supplier Wise Deatils End----------------------------------------****/

        /*****-------------------------------------Item Wise Deatils----------------------------------------****/
        public ActionResult GetItemWiseInvoice(Search_model model)
        {
            try
            {
                var model1 = model.search_prm;
                ViewBag.PrcItemWiseInvoiceDetail = GetPurchaseDetailsData(model1);
                ViewBag.modeldt = model1;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/ProcurementItemWiseInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }
        }

        /*****-------------------------------------Item Wise Deatils End----------------------------------------****/

        /*****-------------------------------------Item Group Wise Deatils----------------------------------------****/
        public ActionResult GetItemGroupWiseItem(Search_model model)
        {
            try
            {
                var model1 = model.search_prm;
                ViewBag.PrcItemGroupWiseItemDetail = GetPurchaseDetailsData(model1);
                ViewBag.modeldt = model1;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/ProcurementItemGroupWiseItemDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }
        }
        public ActionResult GetItemGroupWiseItemInvoice(Search_model model)
        {
            try
            {
                var model1 = model.search_prm;
                ViewBag.PrcItemGroupWiseItemDetail = GetPurchaseDetailsData(model1);
                ViewBag.modeldt = model1;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/ProcurementItemGroupWiseInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }
        }
        /*****-------------------------------------Item Group Wise Deatils End----------------------------------------****/
       

        //private List<SuppCategoryList> suppCategoryList()
        //{
        //    List<SuppCategoryList> lists = new List<SuppCategoryList>();
        //    DataTable dt = GetSuppCategory();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        SuppCategoryList list = new SuppCategoryList();
        //        list.Cat_id = dr["setup_id"].ToString();
        //        list.Cat_val = dr["setup_val"].ToString();
        //        lists.Add(list);
        //    }
        //    //lists.Insert(0, new SuppCategoryList() { Cat_id = "0", Cat_val = "---All---" });/*Commented by hina on 15-11-2024 to modify multile select value*/
        //    return lists;
        //}
        //private List<SuppPortFolioList> suppPortFolioLists()
        //{
        //    List<SuppPortFolioList> portFolioLists = new List<SuppPortFolioList>();
        //    DataTable dt1 = GetSuppPortfolio();
        //    foreach (DataRow dr in dt1.Rows)
        //    {
        //        SuppPortFolioList custPortFolio = new SuppPortFolioList();
        //        custPortFolio.Portfolio_id = dr["setup_id"].ToString();
        //        custPortFolio.Portfolio_val = dr["setup_val"].ToString();
        //        portFolioLists.Add(custPortFolio);
        //    }
        //    //portFolioLists.Insert(0, new SuppPortFolioList() { Portfolio_id = "0", Portfolio_val = "---All---" });/*Commented by hina on 15-11-2024 to modify multile select value*/
        //    return portFolioLists;
        //}
        public DataTable GetSuppCategory()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = detail_ISERVICES.GetcategoryDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable GetSuppPortfolio()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = detail_ISERVICES.GetsuppportDAL(Comp_ID);
                return dt;
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
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public FileResult ProcurementDetailExporttoExcelDt(ProcurementDetail_Model _Model)
        {
            try
            {
                Search_Parmeters search_Model = new Search_Parmeters();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();               
                if (Session["CompId"] != null)
                {
                    search_Model.CompId = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    search_Model.BrId = Session["BranchId"].ToString();
                }
                var Flag = "";
                var PurchaseBy = "";
                if(_Model.hdnInsightBtn == null|| _Model.hdnInsightBtn == "")
                {
                    if (_Model.CsvData != null)
                    {
                        JArray jObject = JArray.Parse(_Model.CsvData);
                        Flag = jObject[0]["flag"].ToString();
                        search_Model.Supp_id = jObject[0]["Supp_id"].ToString();
                        search_Model.Inv_type = jObject[0]["Inv_type"].ToString();
                        search_Model.category = jObject[0]["category"].ToString();
                        search_Model.portFolio = jObject[0]["portFolio"].ToString();
                        search_Model.Group = jObject[0]["Group"].ToString();
                        search_Model.Item_PortFolio = jObject[0]["Item_PortFolio"].ToString();
                        search_Model.From_dt = jObject[0]["From_dt"].ToString();
                        search_Model.To_dt = jObject[0]["To_dt"].ToString();
                        search_Model.flag = jObject[0]["flag"].ToString();
                        search_Model.HSN_code = jObject[0]["HSN_code"].ToString();
                        search_Model.RCMApp = jObject[0]["RCMApp"].ToString();/* Added by Suraj Maurya on 21-07-2025 */
                        PurchaseBy = jObject[0]["PurchaseBy"].ToString();
                    }
                }                
                else if(_Model.hdnInsightBtn== "InvoiceWiseItemDetail")
                {
                    if (_Model.hdnInvoiceWiseItemDetail != null)
                    {
                        JArray jObject = JArray.Parse(_Model.hdnInvoiceWiseItemDetail);
                        PurchaseBy = jObject[0]["flag"].ToString();
                        search_Model.Inv_no = jObject[0]["Inv_no"].ToString();
                        search_Model.Inv_dt = jObject[0]["Inv_dt"].ToString();
                        search_Model.From_dt = jObject[0]["From_dt"].ToString();
                        search_Model.To_dt = jObject[0]["To_dt"].ToString();
                        search_Model.Supp_Name = jObject[0]["Supp_Name"].ToString();
                        search_Model.flag = jObject[0]["flag"].ToString();
                    }
                }
                else if (_Model.hdnInsightBtn == "SupplierWiseInvoice")
                {
                    if (_Model.hdnSupplierWiseInvoiceDetail != null)
                    {
                        JArray jObject = JArray.Parse(_Model.hdnSupplierWiseInvoiceDetail);
                        PurchaseBy = jObject[0]["flag"].ToString();
                        search_Model.currency = jObject[0]["currency"].ToString();
                        search_Model.Supp_id = jObject[0]["Supp_id"].ToString();
                        search_Model.From_dt = jObject[0]["From_dt"].ToString();
                        search_Model.To_dt = jObject[0]["To_dt"].ToString();
                        search_Model.Supp_Name = jObject[0]["Supp_Name"].ToString();
                        search_Model.flag = jObject[0]["flag"].ToString();
                    }
                }
                else if (_Model.hdnInsightBtn == "SupplierWiseInvoiceItem")
                {
                    if (_Model.hdnSupplierWiseInvoiceItemDetail != null)
                    {
                        JArray jObject = JArray.Parse(_Model.hdnSupplierWiseInvoiceItemDetail);
                        PurchaseBy = jObject[0]["flag"].ToString();
                        search_Model.currency = jObject[0]["currency"].ToString();
                        search_Model.From_dt = jObject[0]["From_dt"].ToString();
                        search_Model.To_dt = jObject[0]["To_dt"].ToString();
                        search_Model.Supp_Name = jObject[0]["Supp_Name"].ToString();
                        search_Model.flag = jObject[0]["flag"].ToString();
                        search_Model.Inv_no = jObject[0]["Inv_no"].ToString();
                        search_Model.Inv_dt = jObject[0]["Inv_dt"].ToString();
                    }
                }
                else if (_Model.hdnInsightBtn == "ItemWiseInvoice")
                {
                    if (_Model.hdnItemWiseInvoiceDetail != null)
                    {
                        JArray jObject = JArray.Parse(_Model.hdnItemWiseInvoiceDetail);
                        PurchaseBy = jObject[0]["flag"].ToString();
                        search_Model.currency = jObject[0]["currency"].ToString();
                        search_Model.ItemID = jObject[0]["ItemID"].ToString();
                        search_Model.ItemName = jObject[0]["ItemName"].ToString();
                        search_Model.From_dt = jObject[0]["From_dt"].ToString();
                        search_Model.To_dt = jObject[0]["To_dt"].ToString();
                        search_Model.flag = jObject[0]["flag"].ToString();
                        search_Model.Uom = jObject[0]["Uom"].ToString();
                        search_Model.Qty = jObject[0]["Qty"].ToString();
                    }
                }
                else if (_Model.hdnInsightBtn == "ItemGroupWiseItem")
                {
                    if (_Model.hdnItemGroupWiseItemDetail != null)
                    {
                        JArray jObject = JArray.Parse(_Model.hdnItemGroupWiseItemDetail);
                        PurchaseBy = jObject[0]["flag"].ToString();
                        search_Model.currency = jObject[0]["currency"].ToString();
                        search_Model.Group = jObject[0]["Group"].ToString();
                        search_Model.GroupName = jObject[0]["GroupName"].ToString();
                        search_Model.From_dt = jObject[0]["From_dt"].ToString();
                        search_Model.To_dt = jObject[0]["To_dt"].ToString();
                        search_Model.flag = jObject[0]["flag"].ToString();
                    }
                }
                else if (_Model.hdnInsightBtn == "ItemGroupWiseItemInvoice")
                {
                    if (_Model.hdnItemGroupWiseItemInvoiceDetail != null)
                    {
                        JArray jObject = JArray.Parse(_Model.hdnItemGroupWiseItemInvoiceDetail);
                        PurchaseBy = jObject[0]["flag"].ToString();
                        search_Model.currency = jObject[0]["currency"].ToString();
                        search_Model.ItemID = jObject[0]["ItemID"].ToString();
                        search_Model.ItemName = jObject[0]["ItemName"].ToString();
                        search_Model.Uom = jObject[0]["Uom"].ToString();
                        search_Model.Qty = jObject[0]["Qty"].ToString();
                        search_Model.From_dt = jObject[0]["From_dt"].ToString();
                        search_Model.To_dt = jObject[0]["To_dt"].ToString();
                        search_Model.flag = jObject[0]["flag"].ToString();
                    }
                }
                else if (_Model.hdnInsightBtn == "ItemGroupWiseItemInvoice")
                {
                    if (_Model.hdnItemGroupWiseItemInvoiceDetail != null)
                    {
                        JArray jObject = JArray.Parse(_Model.hdnItemGroupWiseItemInvoiceDetail);
                        PurchaseBy = jObject[0]["flag"].ToString();
                        search_Model.currency = jObject[0]["currency"].ToString();
                        search_Model.ItemID = jObject[0]["ItemID"].ToString();
                        search_Model.ItemName = jObject[0]["ItemName"].ToString();
                        search_Model.Uom = jObject[0]["Uom"].ToString();
                        search_Model.Qty = jObject[0]["Qty"].ToString();
                        search_Model.From_dt = jObject[0]["From_dt"].ToString();
                        search_Model.To_dt = jObject[0]["To_dt"].ToString();
                        search_Model.flag = jObject[0]["flag"].ToString();
                    }
                }
                ds = detail_ISERVICES.GetPrcFilteredReport(search_Model);
                if (PurchaseBy == "I")
                {
                    dt.Columns.Add("Sr.No.", typeof(int));
                    dt.Columns.Add("Supplier Name", typeof(string));
                    dt.Columns.Add("GST Category", typeof(string));
                    if (Flag == "Summary")/* Add by Hina on 29-07-2025 */
                    {
                        dt.Columns.Add("GST Number", typeof(string));
                    }
                    dt.Columns.Add("Purchase Type", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Invoice Number", typeof(string));
                    dt.Columns.Add("Invoice Date", typeof(string));
                    //if (Flag == "Summary")/* Added by Suraj Maurya on 21-07-2025 */
                    //{
                    dt.Columns.Add("RCM Applicable", typeof(string));
                //}
                    dt.Columns.Add("Bill Number", typeof(string));
                    dt.Columns.Add("Bill Date", typeof(string));
                    if(Flag== "Detail")
                    {
                        dt.Columns.Add("Item Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("HSN/SAC Code", typeof(string));
                        dt.Columns.Add("Purchase Quantity", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Base)", typeof(decimal));
                    }
                    dt.Columns.Add("Purchase Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Purchase Amount (In Base)", typeof(decimal));
                    if (Flag == "Summary")
                    {
                        dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                        dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    }
                    if (Flag == "Detail")
                    {
                        if (_Model.TaxColumnJson_forCsv != null)
                        {
                            JArray jObject_tax = JArray.Parse(_Model.TaxColumnJson_forCsv);
                            foreach(var row in jObject_tax)
                            {
                                dt.Columns.Add(row["tax_name"].ToString().Replace("_","."), typeof(string));
                            }
                        }
                        //dt.Columns.Add("IGSTPercent", typeof(string));
                        //dt.Columns.Add("IGST Amount", typeof(string));
                        //dt.Columns.Add("CGSTPercent", typeof(string));
                        //dt.Columns.Add("CGST Amount", typeof(string));
                        //dt.Columns.Add("SGSTPercent", typeof(string));
                        //dt.Columns.Add("SGST Amount", typeof(string));
                        dt.Columns.Add("Tax Amount", typeof(decimal));
                    }
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (Flag == "Summary")
                    {
                        dt.Columns.Add("TDS Amount", typeof(string));
                        dt.Columns.Add("Approved On", typeof(string));
                    }                        

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No."] = rowno + 1;
                            dtrowLines["Supplier Name"] = dr["supp_name"].ToString();
                            dtrowLines["GST Category"] = dr["gst_cat"].ToString();
                            if (Flag == "Summary")/* Added by Hina on 29-07-2025 */
                            {
                                dtrowLines["GST Number"] = dr["supp_gst_no"].ToString();
                            }
                            dtrowLines["Purchase Type"] = dr["inv_type"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Invoice Number"] = dr["inv_no"].ToString();
                            dtrowLines["Invoice Date"] = dr["inv_dt1"].ToString();
                            //if (Flag == "Summary")/* Added by Suraj Maurya on 21-07-2025 */
                            //{
                                dtrowLines["RCM Applicable"] = dr["rcm_app"].ToString() == "Y" ? "Yes" : "No";
                            //}
                            dtrowLines["Bill Number"] = dr["bill_no"].ToString();
                            dtrowLines["Bill Date"] = dr["bill_dt"].ToString();
                            if (Flag == "Detail")
                            {
                                dtrowLines["Item Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["HSN/SAC Code"] = dr["HSN_code"].ToString();
                                dtrowLines["Purchase Quantity"] = dr["mr_qty"].ToString();
                                dtrowLines["Cost Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Cost Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }
                            dtrowLines["Purchase Amount (In Specific)"] = dr["purchase_amount_spec"].ToString();
                            dtrowLines["Purchase Amount (In Base)"] = dr["purchase_amount_bs"].ToString();
                            if (Flag == "Summary")/* Added by Hina on 29-07-2025 */
                            {
                                dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                            }
                            if (Flag == "Detail")/* Added by Hina on 29-07-2025 */
                            {
                                if (_Model.TaxColumnJson_forCsv != null)
                                {
                                    JArray jObject_tax = JArray.Parse(_Model.TaxColumnJson_forCsv);
                                    foreach (var row in jObject_tax)
                                    {
                                        dtrowLines[row["tax_name"].ToString().Replace("_",".")] = dr[row["tax_name"].ToString()].ToString().Replace(",","");
                                    }
                                }
                                //dtrowLines["IGSTPercent"] = dr["igstRate"].ToString();
                                //dtrowLines["IGST Amount"] = dr["igst"].ToString();
                                //dtrowLines["CGSTPercent"] = dr["cgstRate"].ToString();
                                //dtrowLines["CGST Amount"] = dr["cgst"].ToString();
                                //dtrowLines["SGSTPercent"] = dr["sgstRate"].ToString();
                                //dtrowLines["SGST Amount"] = dr["sgst"].ToString();

                                dtrowLines["Tax Amount"] = dr["tax_amt_bs"].ToString();
                            }
                            dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            if (Flag == "Summary")
                            {
                                dtrowLines["TDS Amount"] = dr["tds_amt"].ToString();/* Added by Hina on 29-07-2025 */
                                dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (PurchaseBy == "InvoiceWiseItem" || PurchaseBy == "SupplierWiseInvoiceItem" || PurchaseBy == "G")
                {
                    dt.Columns.Add("Sr.No.", typeof(int));
                    if(PurchaseBy == "G")
                    {
                        dt.Columns.Add("Group Name", typeof(string));
                        dt.Columns.Add("Currency", typeof(string));
                        if (Flag == "ItemGroupWiseDetail")
                        {
                            dt.Columns.Add("Item Name", typeof(string));
                            dt.Columns.Add("UOM", typeof(string));
                            dt.Columns.Add("Quantity", typeof(decimal));
                            dt.Columns.Add("Average Cost Price (In Specific)", typeof(decimal));
                            dt.Columns.Add("Average Cost Price (In Base)", typeof(decimal));
                        }
                    }
                    else
                    {
                        dt.Columns.Add("Item Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Purchase Quantity", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Base)", typeof(decimal));
                    }
                    dt.Columns.Add("Purchase Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Purchase Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No."] = rowno + 1;
                            if (PurchaseBy == "G")
                            {
                                dtrowLines["Group Name"] = dr["item_group_name"].ToString();
                                dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                                if (Flag == "ItemGroupWiseDetail")
                                {
                                    dtrowLines["Item Name"] = dr["item_name"].ToString();
                                    dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                    dtrowLines["Quantity"] = dr["item_qty"].ToString();
                                    dtrowLines["Average Cost Price (In Specific)"] = dr["avg_item_rate_spec"].ToString();
                                    dtrowLines["Average Cost Price (In Base)"] = dr["avg_item_rate_bs"].ToString();
                                }
                            }
                            else
                            {
                                dtrowLines["Item Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["Purchase Quantity"] = dr["mr_qty"].ToString();
                                dtrowLines["Cost Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Cost Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }                            
                            dtrowLines["Purchase Amount (In Specific)"] = dr["purchase_amount_spec"].ToString();
                            dtrowLines["Purchase Amount (In Base)"] = dr["purchase_amount_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (PurchaseBy == "S")
                {
                    {
                        dt.Columns.Add("Sr.No.", typeof(int));
                        dt.Columns.Add("Supplier Name", typeof(string));
                        dt.Columns.Add("Currency", typeof(string));
                        if (Flag == "SupplierWiseDetail")
                        {
                            dt.Columns.Add("Invoice Number", typeof(string));
                            dt.Columns.Add("Invoice Date", typeof(string));
                        }
                        dt.Columns.Add("Purchase Amount (In Specific)", typeof(decimal));
                        dt.Columns.Add("Purchase Amount (In Base)", typeof(decimal));
                        dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                        dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                        dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                        dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                        dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                        dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                        if (Flag == "SupplierWiseDetail")
                        {
                            dt.Columns.Add("Approved On", typeof(string));
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int rowno = 0;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DataRow dtrowLines = dt.NewRow();
                                dtrowLines["Sr.No."] = rowno + 1;
                                dtrowLines["Supplier Name"] = dr["supp_name"].ToString();
                                dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                                if (Flag == "SupplierWiseDetail")
                                {
                                    dtrowLines["Invoice Number"] = dr["inv_no"].ToString();
                                    dtrowLines["Invoice Date"] = dr["inv_dt1"].ToString();
                                }
                                dtrowLines["Purchase Amount (In Specific)"] = dr["purchase_amount_spec"].ToString();
                                dtrowLines["Purchase Amount (In Base)"] = dr["purchase_amount_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                                if (Flag == "SupplierWiseDetail")
                                {
                                    dtrowLines["Approved On"] = dr["app_dt"].ToString();
                                }
                                dt.Rows.Add(dtrowLines);
                                rowno = rowno + 1;
                            }
                        }
                    }
                }
                else if (PurchaseBy == "SupplierWiseInvoice" ||PurchaseBy == "ItemWiseInvoice")
                {
                    dt.Columns.Add("Sr.No.", typeof(int));
                    dt.Columns.Add("Invoice Number", typeof(string));
                    dt.Columns.Add("Invoice Date", typeof(string));
                    if (PurchaseBy == "ItemWiseInvoice")
                    {
                        dt.Columns.Add("Supplier Name", typeof(string));
                        dt.Columns.Add("Quantity", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Base)", typeof(decimal));
                    }
                    dt.Columns.Add("Purchase Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Purchase Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Approved On", typeof(string));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No."] = rowno + 1;
                            dtrowLines["Invoice Number"] = dr["inv_no"].ToString();
                            dtrowLines["Invoice Date"] = dr["inv_dt1"].ToString();
                            if (PurchaseBy == "ItemWiseInvoice")
                            {
                                dtrowLines["Supplier Name"] = dr["supp_name"].ToString();
                                dtrowLines["Quantity"] = dr["mr_qty"].ToString();
                                dtrowLines["Cost Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Cost Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }
                            dtrowLines["Purchase Amount (In Specific)"] = dr["purchase_amount_spec"].ToString();
                            dtrowLines["Purchase Amount (In Base)"] = dr["purchase_amount_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (PurchaseBy == "T"|| PurchaseBy == "ItemWiseSummery")
                {
                    dt.Columns.Add("Sr.No.", typeof(int));
                    dt.Columns.Add("Item Name", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("HSN/SAC Code", typeof(string));
                    if(PurchaseBy != "ItemWiseSummery")
                    {
                        dt.Columns.Add("Currency", typeof(string));
                    }                    
                    if (Flag == "ItemWiseDetail")
                    {
                        dt.Columns.Add("Invoice Number", typeof(string));
                        dt.Columns.Add("Invoice Date", typeof(string));
                        dt.Columns.Add("Supplier Name", typeof(string));
                        dt.Columns.Add("Quantity", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Base)", typeof(decimal));
                    }
                    else
                    {
                        dt.Columns.Add("Quantity", typeof(decimal));
                        dt.Columns.Add("Average Cost Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Average Cost Price (In Base)", typeof(decimal));
                    }
                    dt.Columns.Add("Purchase Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Purchase Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (Flag == "ItemWiseDetail")
                    {
                        dt.Columns.Add("Approved On", typeof(string));
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No."] = rowno + 1;
                            dtrowLines["Item Name"] = dr["item_name"].ToString();
                            dtrowLines["UOM"] = dr["uom_alias"].ToString();
                            dtrowLines["HSN/SAC Code"] = dr["HSN_code"].ToString();
                            if (PurchaseBy != "ItemWiseSummery")
                            {
                                dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            }                               
                            if (Flag == "ItemWiseDetail")
                            {
                                dtrowLines["Invoice Number"] = dr["inv_no"].ToString();
                                dtrowLines["Invoice Date"] = dr["inv_dt1"].ToString();
                                dtrowLines["Supplier Name"] = dr["supp_name"].ToString();
                                dtrowLines["Quantity"] = dr["mr_qty"].ToString();
                                dtrowLines["Cost Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Cost Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }
                            else
                            {
                                dtrowLines["Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Average Cost Price (In Specific)"] = dr["avg_item_rate_spec"].ToString();
                                dtrowLines["Average Cost Price (In Base)"] = dr["avg_item_rate_bs"].ToString();
                            }
                            dtrowLines["Purchase Amount (In Specific)"] = dr["purchase_amount_spec"].ToString();
                            dtrowLines["Purchase Amount (In Base)"] = dr["purchase_amount_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            if (Flag == "ItemWiseDetail")
                            {
                                dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (PurchaseBy == "B")
                {
                    dt.Columns.Add("Sr.No.", typeof(int));
                    dt.Columns.Add("Supplier Name", typeof(string));
                    dt.Columns.Add("GST Category", typeof(string));
                     if (Flag == "InterBranchPurchaseSummary")/* Add by Hina on 29-07-2025 */
                    {
                        dt.Columns.Add("GST Number", typeof(string));
                    }
                    dt.Columns.Add("Purchase Type", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Invoice Number", typeof(string));
                    dt.Columns.Add("Invoice Date", typeof(string));
                    //if (Flag == "Summary")/* Added by Suraj Maurya on 21-07-2025 */
                    //{
                    dt.Columns.Add("RCM Applicable", typeof(string));
                    //}
                    dt.Columns.Add("Bill Number", typeof(string));
                    dt.Columns.Add("Bill Date", typeof(string));
                    if (Flag == "InterBranchPurchaseDetail")
                    {
                        dt.Columns.Add("Item Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("HSN/SAC Code", typeof(string));
                        dt.Columns.Add("Purchase Quantity", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Cost Price (In Base)", typeof(decimal));
                    }
                    dt.Columns.Add("Purchase Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Purchase Amount (In Base)", typeof(decimal));
                    if (Flag == "InterBranchPurchaseSummary")
                    {
                        dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                        dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    }
                    if (Flag == "InterBranchPurchaseDetail")
                    {
                        if (_Model.TaxColumnJson_forCsv != null)
                        {
                            JArray jObject_tax = JArray.Parse(_Model.TaxColumnJson_forCsv);
                            foreach (var row in jObject_tax)
                            {
                                dt.Columns.Add(row["tax_name"].ToString(), typeof(string));
                            }
                        }
                        //dt.Columns.Add("IGSTPercent", typeof(string));
                        //dt.Columns.Add("IGST Amount", typeof(string));
                        //dt.Columns.Add("CGSTPercent", typeof(string));
                        //dt.Columns.Add("CGST Amount", typeof(string));
                        //dt.Columns.Add("SGSTPercent", typeof(string));
                        //dt.Columns.Add("SGST Amount", typeof(string));
                        dt.Columns.Add("Tax Amount", typeof(decimal));
                    }
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (Flag == "InterBranchPurchaseSummary")
                    {
                        dt.Columns.Add("TDS Amount", typeof(string));
                        dt.Columns.Add("Approved On", typeof(string));
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No."] = rowno + 1;
                            dtrowLines["Supplier Name"] = dr["supp_name"].ToString();
                            dtrowLines["GST Category"] = dr["gst_cat"].ToString();
                            if (Flag == "InterBranchPurchaseSummary")/* Added by Hina on 29-07-2025 */
                            {
                                dtrowLines["GST Number"] = dr["supp_gst_no"].ToString();
                            }
                            dtrowLines["Purchase Type"] = dr["inv_type"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Invoice Number"] = dr["inv_no"].ToString();
                            dtrowLines["Invoice Date"] = dr["inv_dt1"].ToString();
                            //if (Flag == "Summary")/* Added by Suraj Maurya on 21-07-2025 */
                            //{
                            dtrowLines["RCM Applicable"] = dr["rcm_app"].ToString() == "Y" ? "Yes" : "No";
                            //}
                            dtrowLines["Bill Number"] = dr["bill_no"].ToString();
                            dtrowLines["Bill Date"] = dr["bill_dt"].ToString();
                            if (Flag == "InterBranchPurchaseDetail")
                            {
                                dtrowLines["Item Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["HSN/SAC Code"] = dr["HSN_code"].ToString();
                                dtrowLines["Purchase Quantity"] = dr["mr_qty"].ToString();
                                dtrowLines["Cost Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Cost Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }
                            dtrowLines["Purchase Amount (In Specific)"] = dr["purchase_amount_spec"].ToString();
                            dtrowLines["Purchase Amount (In Base)"] = dr["purchase_amount_bs"].ToString();
                            if (Flag == "InterBranchPurchaseSummary")/* Added by Hina on 29-07-2025 */
                            {
                                dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                            }
                            if (Flag == "InterBranchPurchaseDetail")/* Added by Hina on 29-07-2025 */
                            {
                                if (_Model.TaxColumnJson_forCsv != null)
                                {
                                    JArray jObject_tax = JArray.Parse(_Model.TaxColumnJson_forCsv);
                                    foreach (var row in jObject_tax)
                                    {
                                        dtrowLines[row["tax_name"].ToString().Replace("_",".")] = dr[row["tax_name"].ToString()].ToString().Replace(",", "");
                                    }
                                }
                                //dtrowLines["IGSTPercent"] = dr["igstRate"].ToString();
                                //dtrowLines["IGST Amount"] = dr["igst"].ToString();
                                //dtrowLines["CGSTPercent"] = dr["cgstRate"].ToString();
                                //dtrowLines["CGST Amount"] = dr["cgst"].ToString();
                                //dtrowLines["SGSTPercent"] = dr["sgstRate"].ToString();
                                //dtrowLines["SGST Amount"] = dr["sgst"].ToString();
                                dtrowLines["Tax Amount"] = dr["tax_amt_bs"].ToString();
                            }
                            dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            if (Flag == "InterBranchPurchaseSummary")
                            {
                                dtrowLines["TDS Amount"] = dr["tds_amt"].ToString();/* Added by Hina on 29-07-2025 */
                                dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("ProcurementDetail", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        /*---------------------------------------DataTable Functionality-------------------------------------------*/
        public JsonResult LoadPrcDetailsDataTable(Search_model model, string PurchaseBy,string ShowAs)
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

                List<PrcDtlInvWiseSummaryModel> _SummaryModel = new List<PrcDtlInvWiseSummaryModel>();
                List<PrcDtlInvWiseDetailedModel> _DetailedModel = new List<PrcDtlInvWiseDetailedModel>();
                List<PrcDtlInvWiseDetailedModel> _DataTotal = new List<PrcDtlInvWiseDetailedModel>();
                DataSet ds = new DataSet();
                (_DetailedModel, _DataTotal, recordsTotal,ds) = getDtInvWiseSummeryList(model, skip, pageSize, searchValue, sortColumn, sortColumnDir);


                if (model.search_prm.flag == "Detail" || model.search_prm.flag == "InterBranchPurchaseDetail")
                {
                    //var ItemListData2 = (from tempitem in dt.AsEnumerable() select tempitem);

                    //Paging     
                    //var data = ItemListData2.ToList();
                    var model2 = ToDynamicList(ds.Tables[0]);
                    var total = ToDynamicList(ds.Tables[2]);

                    //Returning Json Data    
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = model2, footer = total });

                }
                else
                {
                    // Getting all Customer data    
                    var ItemListData = (from tempitem in _DetailedModel select tempitem);
                    //Sorting    
                    //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    //{
                    //    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                    //}

                    //Paging     
                    var data = ItemListData.ToList();
                    //Returning Json Data    
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data, footer = _DataTotal });

                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        public List<Dictionary<string, string>> ToDynamicList(DataTable dt)
        {
            var list = new List<Dictionary<string, string>>();

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col].ToString();
                }
                list.Add(dict);
            }

            return list;
        }

        private (List<PrcDtlInvWiseDetailedModel>, List<PrcDtlInvWiseDetailedModel>, int,DataSet) getDtInvWiseSummeryList(Search_model search_Model, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir)
        {
            List<PrcDtlInvWiseDetailedModel> _ItemListModel = new List<PrcDtlInvWiseDetailedModel>();
            List<PrcDtlInvWiseDetailedModel> _DataTotal = new List<PrcDtlInvWiseDetailedModel>();
            
            int Total_Records = 0;
            CommonController cmn = new CommonController();
            try
            {
                DataSet DSet = new DataSet();
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string ShowAs = string.Empty, ReportType = string.Empty, SupplierName = string.Empty
                    , PurchaseType = string.Empty, SupplierCategory = string.Empty, SupplierPortfolio = string.Empty
                    , FromDate = string.Empty, ToDate = string.Empty;

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
                    User_ID = Session["UserId"].ToString();
                }

                
                if (Session["CompId"] != null)
                {
                    search_Model.search_prm.CompId = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    search_Model.search_prm.BrId = Session["BranchId"].ToString();
                }
                var model1 = search_Model.search_prm;
                //--------Added By Nidhi on 23-10-25----------
                model1.Supp_id = cmn.ReplaceDefault(model1.Supp_id);
                model1.category = cmn.ReplaceDefault(model1.category);
                model1.portFolio = cmn.ReplaceDefault(model1.portFolio);
                model1.RCMApp = cmn.ReplaceDefault(model1.RCMApp);
                model1.Group = cmn.ReplaceDefault(model1.Group);
                model1.brid_list = cmn.ReplaceDefault(model1.brid_list);
                // ---------------End 23-10-25---------------------
                DSet = detail_ISERVICES.GetPrcFilteredReport(model1, skip, pageSize, searchValue, sortColumn, sortColumnDir);
                
                if (DSet.Tables.Count >= 2)
                {
                    if (DSet.Tables[0].Rows.Count > 0)
                    {
                        if (model1.flag == "Summary")
                        {

                            _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new PrcDtlInvWiseDetailedModel
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    supp_name = row.Field<string>("supp_name"),
                    gst_cat = row.Field<string>("gst_cat"),/*add supp_gst_category by Hina sharma on 04-09-2025*/
                    supp_gst_no = row.Field<string>("supp_gst_no"),
                    inv_type = row.Field<string>("inv_type"),
                    curr_symbol = row.Field<string>("curr_symbol"),
                    inv_no = row.Field<string>("inv_no"),
                    inv_dt1 = row.Field<string>("inv_dt1"),
                    inv_dt = row.Field<string>("inv_dt"),
                    //rcm_app = row.Field<string>("rcm_app"),
                    rcm_app = row.Field<string>("rcm_app") == "Y" ? "Yes" : "No",

                    bill_no = row.Field<string>("bill_no"),
                    bill_dt = row.Field<string>("bill_dt"),
                    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                    tax_amt_spec = row.Field<string>("tax_amt_spec"),
                    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                    tds_amt = row.Field<string>("tds_amt"),
                    app_dt = row.Field<string>("app_dt"),
                    roundoff_diff = row.Field<string>("roundoff_diff")
                }).ToList();

                        }
                        else if (model1.flag == "Detail")
                        {
                            /* Commented by Suraj Maurya on 20-09-2025 due to method changed for dynamic tax details */
                            //            _ItemListModel = DSet.Tables[0].AsEnumerable()
                            //.Select((row, index) => new PrcDtlInvWiseDetailedModel
                            //{
                            //    SrNo = row.Field<Int64>("SrNo"),
                            //    supp_name = row.Field<string>("supp_name"),
                            //    gst_cat = row.Field<string>("gst_cat"),/*add supp_gst_category by Hina sharma on 04-09-2025*/
                            //    inv_type = row.Field<string>("inv_type"),
                            //    curr_symbol = row.Field<string>("curr_symbol"),
                            //    inv_no = row.Field<string>("inv_no"),
                            //    inv_dt1 = row.Field<string>("inv_dt1"),
                            //    inv_dt = row.Field<string>("inv_dt"),
                            //    rcm_app = row.Field<string>("rcm_app") == "Y" ? "Yes" : "No",/*add supp_gst_category by Hina sharma on 04-09-2025*/
                            //    bill_no = row.Field<string>("bill_no"),
                            //    bill_dt = row.Field<string>("bill_dt"),
                            //    item_name = row.Field<string>("item_name"),
                            //    item_id = row.Field<string>("item_id"),
                            //    uom_alias = row.Field<string>("uom_alias"),
                            //    HSN_code = row.Field<string>("HSN_code"),
                            //    mr_qty = row.Field<string>("mr_qty"),
                            //    item_rate_spec = row.Field<string>("item_rate_spec"),
                            //    item_rate_bs = row.Field<string>("item_rate_bs"),
                            //    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                            //    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                            //    //igstRate = row.Field<string>("igstRate"),
                            //    //igst = row.Field<string>("igst"),/*add igst,cgst,sgst by Hina sharma on 01-07-2025*/
                            //    //cgstRate = row.Field<string>("cgstRate"),
                            //    //cgst = row.Field<string>("cgst"),
                            //    //sgstRate = row.Field<string>("sgstRate"),
                            //    //sgst = row.Field<string>("sgst"),
                            //    //tax_amt_spec = row.Field<string>("tax_amt_spec"),
                            //    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                            //    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                            //    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                            //    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                            //    invoice_amt_bs = row.Field<string>("invoice_amt_bs")
                            //}).ToList();
                        }
                        else if (model1.flag == "SupplierWiseSummery")
                        {
                            _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new PrcDtlInvWiseDetailedModel
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    supp_name = row.Field<string>("supp_name"),
                    supp_id = row.Field<int>("supp_id"),
                    curr_symbol = row.Field<string>("curr_symbol"),
                    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                    tax_amt_spec = row.Field<string>("tax_amt_spec"),
                    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                    roundoff_diff = row.Field<string>("roundoff_diff")
                }).ToList();
                        }
                        else if (model1.flag == "SupplierWiseDetail")
                        {
                            _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new PrcDtlInvWiseDetailedModel
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    supp_name = row.Field<string>("supp_name"),
                    supp_id = row.Field<int>("supp_id"),
                    br_id = row.Field<int>("br_id"),
                    curr_symbol = row.Field<string>("curr_symbol"),
                    inv_no = row.Field<string>("inv_no"),
                    inv_dt1 = row.Field<string>("inv_dt1"),
                    inv_dt = row.Field<string>("inv_dt"),
                    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                    tax_amt_spec = row.Field<string>("tax_amt_spec"),
                    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                    app_dt = row.Field<string>("app_dt")
                }).ToList();
                        }
                        else if (model1.flag == "ItemWiseSummery")
                        {
                            _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new PrcDtlInvWiseDetailedModel
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    item_name = row.Field<string>("item_name"),
                    item_id = row.Field<string>("item_id"),
                    uom_alias = row.Field<string>("uom_alias"),
                    curr_symbol = row.Field<string>("curr_symbol"),
                    mr_qty = row.Field<string>("item_qty"),
                    item_rate_spec = row.Field<string>("avg_item_rate_spec"),
                    item_rate_bs = row.Field<string>("avg_item_rate_bs"),
                    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                    tax_amt_spec = row.Field<string>("tax_amt_spec"),
                    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                    HSN_code = row.Field<string>("HSN_code")
                }).ToList();
                        }
                        else if (model1.flag == "ItemWiseDetail")
                        {
                            _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new PrcDtlInvWiseDetailedModel
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    item_name = row.Field<string>("item_name"),
                    item_id = row.Field<string>("item_id"),
                    uom_alias = row.Field<string>("uom_alias"),
                    curr_symbol = row.Field<string>("curr_symbol"),
                    inv_no = row.Field<string>("inv_no"),
                    inv_dt1 = row.Field<string>("inv_dt1"),
                    inv_dt = row.Field<string>("inv_dt"),
                    supp_name = row.Field<string>("supp_name"),
                    mr_qty = row.Field<string>("mr_qty"),
                    item_rate_spec = row.Field<string>("item_rate_spec"),
                    item_rate_bs = row.Field<string>("item_rate_bs"),
                    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                    tax_amt_spec = row.Field<string>("tax_amt_spec"),
                    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                    app_dt = row.Field<string>("app_dt"),
                    HSN_code = row.Field<string>("HSN_code")
                }).ToList();
                        }

                        else if (model1.flag == "ItemGroupWiseSummery")
                        {
                            _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new PrcDtlInvWiseDetailedModel
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    item_grp_id = row.Field<int>("item_grp_id"),
                    item_group_name = row.Field<string>("item_group_name"),
                    curr_symbol = row.Field<string>("curr_symbol"),
                    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                    tax_amt_spec = row.Field<string>("tax_amt_spec"),
                    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                    //bill_no = row.Field<string>("bill_no"),
                    //bill_dt = row.Field<string>("bill_dt")
                }).ToList();
                        }
                        else if (model1.flag == "ItemGroupWiseDetail")
                        {
                            _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new PrcDtlInvWiseDetailedModel
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    item_grp_id = row.Field<int>("item_grp_id"),
                    item_group_name = row.Field<string>("item_group_name"),
                    item_name = row.Field<string>("item_name"),
                    item_id = row.Field<string>("item_id"),
                    uom_alias = row.Field<string>("uom_alias"),
                    curr_symbol = row.Field<string>("curr_symbol"),
                    mr_qty = row.Field<string>("item_qty"),
                    item_rate_spec = row.Field<string>("avg_item_rate_spec"),
                    item_rate_bs = row.Field<string>("avg_item_rate_bs"),
                    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                    tax_amt_spec = row.Field<string>("tax_amt_spec"),
                    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                    bill_no = row.Field<string>("bill_no"),
                    bill_dt = row.Field<string>("bill_dt")
                }).ToList();
                        }
                        else if (model1.flag == "InterBranchPurchaseSummary")/*add InterBranchPurchaseSummary by Hina sharma on 04-09-2025*/
                        {

                            _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new PrcDtlInvWiseDetailedModel
                {
                    SrNo = row.Field<Int64>("SrNo"),
                    supp_name = row.Field<string>("supp_name"),
                    gst_cat = row.Field<string>("gst_cat"),/*add supp_gst_category by Hina sharma on 04-09-2025*/
                    supp_gst_no = row.Field<string>("supp_gst_no"),
                    inv_type = row.Field<string>("inv_type"),
                    curr_symbol = row.Field<string>("curr_symbol"),
                    inv_no = row.Field<string>("inv_no"),
                    inv_dt1 = row.Field<string>("inv_dt1"),
                    inv_dt = row.Field<string>("inv_dt"),
                    //rcm_app = row.Field<string>("rcm_app"),
                    rcm_app = row.Field<string>("rcm_app") == "Y" ? "Yes" : "No",

                    bill_no = row.Field<string>("bill_no"),
                    bill_dt = row.Field<string>("bill_dt"),
                    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                    tax_amt_spec = row.Field<string>("tax_amt_spec"),
                    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                    tds_amt = row.Field<string>("tds_amt"),
                    app_dt = row.Field<string>("app_dt"),
                    roundoff_diff = row.Field<string>("roundoff_diff"),
                }).ToList();

                        }
                        else if (model1.flag == "InterBranchPurchaseDetail")
                        {
                            /* Commented by Suraj Maurya on 20-09-2025 due to method changed for dynamic tax details */
                            //            _ItemListModel = DSet.Tables[0].AsEnumerable()
                            //.Select((row, index) => new PrcDtlInvWiseDetailedModel
                            //{
                            //    SrNo = row.Field<Int64>("SrNo"),
                            //    supp_name = row.Field<string>("supp_name"),
                            //    gst_cat = row.Field<string>("gst_cat"),/*add supp_gst_category by Hina sharma on 04-09-2025*/
                            //    inv_type = row.Field<string>("inv_type"),
                            //    curr_symbol = row.Field<string>("curr_symbol"),
                            //    inv_no = row.Field<string>("inv_no"),
                            //    inv_dt1 = row.Field<string>("inv_dt1"),
                            //    inv_dt = row.Field<string>("inv_dt"),
                            //    rcm_app = row.Field<string>("rcm_app") == "Y" ? "Yes" : "No",/*add supp_gst_category by Hina sharma on 04-09-2025*/
                            //    bill_no = row.Field<string>("bill_no"),
                            //    bill_dt = row.Field<string>("bill_dt"),
                            //    item_name = row.Field<string>("item_name"),
                            //    item_id = row.Field<string>("item_id"),
                            //    uom_alias = row.Field<string>("uom_alias"),
                            //    HSN_code = row.Field<string>("HSN_code"),
                            //    mr_qty = row.Field<string>("mr_qty"),
                            //    item_rate_spec = row.Field<string>("item_rate_spec"),
                            //    item_rate_bs = row.Field<string>("item_rate_bs"),
                            //    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                            //    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                            //    igstRate = row.Field<string>("igstRate"),
                            //    igst = row.Field<string>("igst"),/*add igst,cgst,sgst by Hina sharma on 01-07-2025*/
                            //    cgstRate = row.Field<string>("cgstRate"),
                            //    cgst = row.Field<string>("cgst"),
                            //    sgstRate = row.Field<string>("sgstRate"),
                            //    sgst = row.Field<string>("sgst"),
                            //    //tax_amt_spec = row.Field<string>("tax_amt_spec"),
                            //    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                            //    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                            //    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                            //    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                            //    invoice_amt_bs = row.Field<string>("invoice_amt_bs")
                            //}).ToList();
                        }
                        /*---------------------------------------------------------*/
                        /*-----------------Total of All rows-----------------------*/
                        /*---------------------------------------------------------*/
                        if(model1.csvFlag != "CSV")
                        {
                            if (model1.flag == "Summary") /*add by Hina sharma on 30-06-2025*/
                            {
                                _DataTotal = DSet.Tables[2].AsEnumerable()
                    .Select((row, index) => new PrcDtlInvWiseDetailedModel
                    {
                        purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                        purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                        tax_amt_spec = row.Field<string>("tax_amt_spec"),
                        tax_amt_bs = row.Field<string>("tax_amt_bs"),
                        oc_amt_spec = row.Field<string>("oc_amt_spec"),
                        oc_amt_bs = row.Field<string>("oc_amt_bs"),
                        invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                        invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                        tds_amt = row.Field<string>("tds_amt"),/*Add by Hina Sharma on 27-06-2025*/
                        roundoff_diff = row.Field<string>("roundoff_diff")
                    }).ToList();
                            }
                            else if (model1.flag == "Detail") /*add by Hina sharma on 30-06-2025*/
                            {
                                /* Commented by Suraj Maurya on 20-09-2025 due to method changed for dynamic tax details */
                                //            _DataTotal = DSet.Tables[2].AsEnumerable()
                                //.Select((row, index) => new PrcDtlInvWiseDetailedModel
                                //{
                                //    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                                //    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                                //    //igst = row.Field<string>("igst"),/*add igst,cgst,sgst by Hina sharma on 01-07-2025*/
                                //    //cgst = row.Field<string>("cgst"),
                                //    //sgst = row.Field<string>("sgst"),
                                //    //tax_amt_spec = row.Field<string>("tax_amt_spec"),
                                //    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                                //    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                                //    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                                //    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                                //    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),

                                //}).ToList();
                            }
                            else if (model1.flag == "InterBranchPurchaseSummary") /*add by Hina sharma on 04-09-2025*/
                            {
                                _DataTotal = DSet.Tables[2].AsEnumerable()
                    .Select((row, index) => new PrcDtlInvWiseDetailedModel
                    {
                        purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                        purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                        tax_amt_spec = row.Field<string>("tax_amt_spec"),
                        tax_amt_bs = row.Field<string>("tax_amt_bs"),
                        oc_amt_spec = row.Field<string>("oc_amt_spec"),
                        oc_amt_bs = row.Field<string>("oc_amt_bs"),
                        invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                        invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                        tds_amt = row.Field<string>("tds_amt"),/*Add by Hina Sharma on 27-06-2025*/
                        roundoff_diff = row.Field<string>("roundoff_diff"),
                    }).ToList();
                            }
                            else if (model1.flag == "InterBranchPurchaseDetail") /*add by Hina sharma on 04-09-2025*/
                            {
                                /* Commented by Suraj Maurya on 20-09-2025 due to method changed for dynamic tax details */
                                //            _DataTotal = DSet.Tables[2].AsEnumerable()
                                //.Select((row, index) => new PrcDtlInvWiseDetailedModel
                                //{
                                //    purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                                //    purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                                //    igst = row.Field<string>("igst"),/*add igst,cgst,sgst by Hina sharma on 01-07-2025*/
                                //    cgst = row.Field<string>("cgst"),
                                //    sgst = row.Field<string>("sgst"),
                                //    //tax_amt_spec = row.Field<string>("tax_amt_spec"),
                                //    tax_amt_bs = row.Field<string>("tax_amt_bs"),
                                //    oc_amt_spec = row.Field<string>("oc_amt_spec"),
                                //    oc_amt_bs = row.Field<string>("oc_amt_bs"),
                                //    invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                                //    invoice_amt_bs = row.Field<string>("invoice_amt_bs"),

                                //}).ToList();
                            }
                            else if (model1.flag == "Summary" || model1.flag == "SupplierWiseSummery")
                            {
                                _DataTotal = DSet.Tables[2].AsEnumerable()
                                               .Select((row, index) => new PrcDtlInvWiseDetailedModel
                                               {
                                                   purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                                                   purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                                                   tax_amt_spec = row.Field<string>("tax_amt_spec"),
                                                   tax_amt_bs = row.Field<string>("tax_amt_bs"),
                                                   oc_amt_spec = row.Field<string>("oc_amt_spec"),
                                                   oc_amt_bs = row.Field<string>("oc_amt_bs"),
                                                   invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                                                   invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                                                   roundoff_diff = row.Field<string>("roundoff_diff"),
                                               }).ToList();
                            }
                            else
                            {
                                _DataTotal = DSet.Tables[2].AsEnumerable()
                                               .Select((row, index) => new PrcDtlInvWiseDetailedModel
                                               {
                                                   purchase_amount_spec = row.Field<string>("purchase_amount_spec"),
                                                   purchase_amount_bs = row.Field<string>("purchase_amount_bs"),
                                                   tax_amt_spec = row.Field<string>("tax_amt_spec"),
                                                   tax_amt_bs = row.Field<string>("tax_amt_bs"),
                                                   oc_amt_spec = row.Field<string>("oc_amt_spec"),
                                                   oc_amt_bs = row.Field<string>("oc_amt_bs"),
                                                   invoice_amt_spec = row.Field<string>("invoice_amt_spec"),
                                                   invoice_amt_bs = row.Field<string>("invoice_amt_bs"),
                                               }).ToList();
                            }
                        }
                        
                        /*---------------------------------------------------------*/
                        /*-----------------Total of All rows-----------------------*/
                        /*---------------------------------------------------------*/
                    }
                    if (model1.csvFlag != "CSV")
                    {
                        if (DSet.Tables[1].Rows.Count > 0)
                        {
                            Total_Records = Convert.ToInt32(DSet.Tables[1].Rows[0]["total_rows"]);
                        }
                    }
                
                }

                return (_ItemListModel, _DataTotal, Total_Records, DSet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }


        [HttpPost]
        public ActionResult ExportCsv([System.Web.Http.FromBody] DataTableRequest request, Search_model model, string PurchaseBy, string ShowAs)
        {
            string keyword = "";
            // Apply search filter
            if (!string.IsNullOrEmpty(request.search?.value))
            {
                keyword = request.search.value;//.ToLower();
            }
            int recordsTotal = 0;
            string sortColumn = "SrNo";
            string sortColumnDir = "asc";
            if (request.order != null && request.order.Any())
            {
                var colIndex = request.order[0].column;
                sortColumnDir = request.order[0].dir;
                sortColumn = request.columns[colIndex].data;

            }

            List<PrcDtlInvWiseDetailedModel> _DetailedModel = new List<PrcDtlInvWiseDetailedModel>();
            List<PrcDtlInvWiseDetailedModel> _DataTotal = new List<PrcDtlInvWiseDetailedModel>();
            DataSet ds = new DataSet();
            // 🔹 Fetch data same as LoadData but ignore pagingDataSet ds = new DataSet();
            model.search_prm.csvFlag = "CSV";
            (_DetailedModel, _DataTotal, recordsTotal, ds) = getDtInvWiseSummeryList(model, 0, request.length, keyword, sortColumn, sortColumnDir);

            if (model.search_prm.flag == "Detail" || model.search_prm.flag == "InterBranchPurchaseDetail")
            {
                var model2 = ToDynamicList(ds.Tables[0]);
                var data = model2.ToList(); // All filtered & sorted rows
                var commonController = new CommonController(_Common_IServices);
                return commonController.Cmn_GetDataToCsv(request, data);
            }
            else
            {
                var data = _DetailedModel.ToList(); // All filtered & sorted rows
                var commonController = new CommonController(_Common_IServices);
                return commonController.Cmn_GetDataToCsv(request, data);
            }

           
        }
        /*---------------------------------------DataTable Functionality End-------------------------------------------*/
    }

}