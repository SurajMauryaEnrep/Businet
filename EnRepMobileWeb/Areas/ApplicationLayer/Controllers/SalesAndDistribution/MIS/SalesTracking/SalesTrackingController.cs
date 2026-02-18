using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.SalesTracking;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.GoodsReceiptNote;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSaleInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.OrderDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.SalesTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.MIS.SalesTracking
{
    public class SalesTrackingController : Controller
    {
        string DocumentMenuId = "105103190120";
        string FromDate, title;

        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly SalesTracking_IService _salesTrackingRepo;
        private readonly DomesticSaleInvoice_ISERVICE _DomesticSaleInvoice_ISERVICE;
        private readonly OrderDetail_IService _OrderISERVICES;
        private readonly ItemList_ISERVICES _itemSetup;
        string CompId, BrId, ship_no, userid, language = String.Empty;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public SalesTrackingController(Common_IServices _Common_IServices, SalesTracking_IService salesTrackingRepo,
             ItemList_ISERVICES itemSetup, DomesticSaleInvoice_ISERVICE DomesticSaleInvoice_ISERVICE, 
             OrderDetail_IService OrderISERVICES, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            _salesTrackingRepo = salesTrackingRepo;
            _itemSetup = itemSetup;
            _DomesticSaleInvoice_ISERVICE = DomesticSaleInvoice_ISERVICE;
            _OrderISERVICES = OrderISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/SalesTracking
        public ActionResult SalesTracking()
        {
            if (Session["UserId"] != null)
                userid = Session["UserId"].ToString();
            if (Session["BranchId"] != null)
            {
                ViewBag.vb_br_id = Session["BranchId"].ToString();
            }
            ViewBag.MenuPageName = getDocumentName();
            SalestrackingModel objModel = new SalestrackingModel();
            objModel.Title = title;
            DateTime dtnow = DateTime.Now;
            //string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            DataSet dttbl = new DataSet();
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                objModel.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }
            string FromDate = objModel.FromDate;
            string ToDate = dtnow.ToString("yyyy-MM-dd");
            //objModel.FromDate = FromDate;
            objModel.ToDate = ToDate;
            //objModel.ItemsList = GetItemsList();
            //objModel.CustomersList = GetCustomersList();/*commented by Hina on 04-12-2024*/
            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(CompId, userid);
            ViewBag.br_list = br_list;

            objModel.ItemsList = GetItemsList();
            objModel.Currencylist = GetCurrencyList();
            objModel.categoryLists = custCategoryList();
            objModel.portFolioLists = custPortFolioLists();
            objModel.custzoneList = CustZoneLists();
            objModel.custgroupList = CustGroupLists();
            List<CityList> _CityList = new List<CityList>();
            objModel.CityLists = _CityList;

            List<StateList> _StateList = new List<StateList>();
            objModel.StateLists = _StateList;
            List<OrderNumberListModel> LstOrder = new List<OrderNumberListModel>();
            OrderNumberListModel objOrder = new OrderNumberListModel()
            {
                OrderNumber = "---All---",
                po_Value = "0"
            };
            LstOrder.Add(objOrder);
            objModel.PoNumberList = LstOrder;
            objModel.SalesPersons = GetSalesPersonList();
            objModel.OrderType1 = "A";
            ViewBag.SOTrackingDetails = GetSOTrackingDetails("", "", "", "", "", "", FromDate, ToDate, objModel.OrderType1);

            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/SalesTracking/SalesTracking.cshtml", objModel);

        }
        private DataSet GetFyList()
        {
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
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
        public ActionResult ShowReportFromDashBoard(string FromDt, string ToDt)
        {

            if (Session["UserId"] != null)
                userid = Session["UserId"].ToString();
            if (Session["BranchId"] != null)
            {
                ViewBag.vb_br_id = Session["BranchId"].ToString();
            }
            ViewBag.MenuPageName = getDocumentName();
            SalestrackingModel objModel = new SalestrackingModel();
            objModel.Title = title;
            
            objModel.FromDate = FromDt;
            objModel.ToDate = ToDt;
            
            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(CompId, userid);
            ViewBag.br_list = br_list;

            objModel.ItemsList = GetItemsList();
            objModel.Currencylist = GetCurrencyList();
            objModel.categoryLists = custCategoryList();
            objModel.portFolioLists = custPortFolioLists();
            objModel.custzoneList = CustZoneLists();
            objModel.custgroupList = CustGroupLists();
            List<CityList> _CityList = new List<CityList>();
            objModel.CityLists = _CityList;

            List<StateList> _StateList = new List<StateList>();
            objModel.StateLists = _StateList;
            List<OrderNumberListModel> LstOrder = new List<OrderNumberListModel>();
            OrderNumberListModel objOrder = new OrderNumberListModel()
            {
                OrderNumber = "---All---",
                po_Value = "0"
            };
            LstOrder.Add(objOrder);
            objModel.PoNumberList = LstOrder;
            objModel.SalesPersons = GetSalesPersonList();
            objModel.OrderType1 = "A";
            ViewBag.SOTrackingDetails = GetSOTrackingDetails("", "", "", "", "", "", FromDt, ToDt, objModel.OrderType1);

            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/SalesTracking/SalesTracking.cshtml", objModel);

        }
        public DataTable GetSOTrackingDetails(string soNo, string custId, string slsPersId, string orderType, string itemId, string currId, string fromDate, string toDate,string NotFillterOrderType)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                    userid = Session["UserId"].ToString();
                DataTable dt = _salesTrackingRepo.GetSalesTrackingList(CompId, BrId, userid, soNo, custId, slsPersId, currId, orderType, itemId, fromDate, toDate, NotFillterOrderType);
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
        private List<ItemsModel> GetItemsList()
        {
            try
            {
                string compId = string.Empty;
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();

                DataTable dt = _itemSetup.BindGetItemList("", compId, BrId);
                List<ItemsModel> itemsList = new List<ItemsModel>();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    ItemsModel itmLst = new ItemsModel
                //    {
                //        ItemId = dr["item_id"].ToString(),
                //        ItemName = dr["item_name"].ToString()
                //    };
                //    itemsList.Add(itmLst);
                //}
                itemsList.Insert(0, new ItemsModel { ItemId = "0", ItemName = "---All---" });
                return itemsList;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        private List<CustomerModel> GetCustomersList()
        {
            string compId = string.Empty;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            SuppList = _DomesticSaleInvoice_ISERVICE.GetCustomerList(compId, "0", BrId, "0", "105103190120");
            SuppList.Remove("0");

            List<CustomerModel> _custList = new List<CustomerModel>();

            CustomerModel _custDetail1 = new CustomerModel();
            _custDetail1.CustId = "0";
            _custDetail1.CustName = "---All---";
            _custList.Add(_custDetail1);


            foreach (var data in SuppList)
            {
                CustomerModel _custDetail = new CustomerModel();
                _custDetail.CustId = data.Key;
                _custDetail.CustName = data.Value;
                _custList.Add(_custDetail);
            }

            return _custList;
        }
        private List<CurrencyList> GetCurrencyList()
        {
            try
            {
                string compId = string.Empty;
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                DataTable dt = _OrderISERVICES.GetCurrencyList(compId, "0");
                List<CurrencyList> currencyList = new List<CurrencyList>();
                foreach (DataRow dr in dt.Rows)
                {
                    CurrencyList crList = new CurrencyList
                    {
                        curr_id = dr["curr_id"].ToString(),
                        curr_name = dr["curr_name"].ToString()
                    };
                    currencyList.Add(crList);
                }
               // currencyList.Insert(0, new CurrencyList { curr_id = "0", curr_name = "---All---" });
                return currencyList;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        private List<SalesPersList> GetSalesPersonList()
        {
            try
            {
                if (Session["CompId"] != null)
                    CompId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    userid = Session["UserId"].ToString();
                }

                DataTable dt = _salesTrackingRepo.GetSalesPersonList(CompId, BrId, userid);
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
        public JsonResult GetOrderNumberListByOrder(string orderType, string custId, string currId)
        {
            JsonResult DataRows = null;
            try
            {
                if (Session["CompId"] != null)
                    CompId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                DataTable dt = _salesTrackingRepo.GetPONumberList(CompId, BrId, orderType, custId, currId);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult SearchSoTrackingDetails(string soNo, string custId, string slsPersId, string orderType, string itemId, string currId, string fromDate, string toDate
            ,string NotFillterOrderType, string custCat, string custPort,string cust_zone, string cust_group, string state, string city)
        {
            try
            {
                SalestrackingModel objModel = new SalestrackingModel();
                objModel.SearchStatus = "SEARCH";
                DataTable dt = new DataTable();
                //dt = GetSOTrackingDetails(soNo, custId, slsPersId, orderType, itemId, currId, fromDate, toDate, NotFillterOrderType);

                if (NotFillterOrderType == "O")
                {
                    ViewBag.SOTrackingDetailsOverdue = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMisSOTrackingOverdues.cshtml", objModel);
                }
                else if (NotFillterOrderType == "P")
                {
                    ViewBag.SOTrackingDetailspending = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMisSOTrackingDetailsPending.cshtml", objModel);
                }
                else
                {
                    ViewBag.SOTrackingDetails = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMisSOTrackingDetails.cshtml", objModel);
                }          
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private string getDocumentName()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompId = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompId, DocumentMenuId, language);
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
        public ActionResult DeliveryShudule(string orderNo, string item_id, string OrderType,string br_id)
        {
            if (Session["CompId"] != null)
                CompId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            //objModel.SearchStatus = "SEARCH";
            ViewBag.DocumentMenuId = DocumentMenuId;
            ViewBag.ItemDelSchdetails = _Common_IServices.Cmn_GetDeliverySchudule(CompId, br_id, OrderType.Trim(), orderNo.Trim(), item_id.Trim(), DocumentMenuId);
            return PartialView("~/Areas/Common/Views/Comn_PartialMISOrderDeliverySchedule.cshtml", null);
        }

        public JsonResult LoadSoTrackingDetailsData(string soNo, string custId, string slsPersId, string orderType, string itemId
            , string currId, string fromDate, string toDate
            , string NotFillterOrderType, string custCat, string custPort, string cust_zone, string cust_group, string state, string city,string brlist)
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

                List<SOTrackingDataModel> _ItemListModel = new List<SOTrackingDataModel>();

                //Get Data Filtered,with Paging,Shorted
                (_ItemListModel, recordsTotal) = getDtList(soNo, custId, slsPersId, currId, orderType, itemId, fromDate, toDate
                            , NotFillterOrderType, custCat, custPort, cust_zone, cust_group, state, city, brlist,skip, pageSize, searchValue, sortColumn, sortColumnDir);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Paging     
                var data = ItemListData.ToList();//.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        private (List<SOTrackingDataModel>, int) getDtList(string soNo, string custId, string slsPersId, string currId, string orderType, string itemId, string fromDate, string toDate
            , string NotFillterOrderType, string custCat, string custPort, string cust_zone, string cust_group, string state, string city,string brlist, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir,string Flag="")
        {
            List<SOTrackingDataModel> _ItemListModel = new List<SOTrackingDataModel>();
            int Total_Records = 0;
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
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
                    User_ID = Session["UserId"].ToString();
                }

                DataSet DSet = new DataSet();
                DSet = _salesTrackingRepo.GetSalesTrackingList(CompID, BrId, User_ID, soNo, custId, slsPersId, currId, orderType, itemId, fromDate, toDate
                             , NotFillterOrderType,custCat, custPort, cust_zone, cust_group, state, city, brlist,skip.ToString(), pageSize.ToString(), searchValue, sortColumn, sortColumnDir, Flag);
                if (DSet.Tables[0].Rows.Count > 0)
                {
                    if (NotFillterOrderType == "P")
                    {
                        _ItemListModel = DSet.Tables[0].AsEnumerable()
           .Select((row, index) => new SOTrackingDataModel
           {
               SrNo = row.Field<Int64>("SrNo"),
               app_so_no = row.Field<string>("app_so_no"),
               so_dt = row.Field<string>("so_dt"),
               cust_name = row.Field<string>("cust_name"),
               OrderType = row.Field<string>("OrderType"),
               curr_logo = row.Field<string>("curr_logo"),
               sls_pers_name = row.Field<string>("sls_pers_name"),
               item_id = row.Field<string>("item_id"),
               item_name = row.Field<string>("item_name"),
               uom_alias = row.Field<string>("uom_alias"),
               ord_qty_base = row.Field<string>("ord_qty_base"),
               Pending_qty = row.Field<string>("Pending_qty"),
               ForceClosed = row.Field<string>("ForceClosed"),
               ref_doc_no = row.Field<string>("ref_doc_no"),
               cust_catg = row.Field<string>("cust_catg"),
               cust_port = row.Field<string>("cust_port"),
               cust_zone = row.Field<string>("cust_zone"),
               cust_group = row.Field<string>("cust_group"),
               state = row.Field<string>("state"),
               city = row.Field<string>("city"),
               br_id = row.Field<int>("br_id")
           }).ToList();
                    }
                    else if (NotFillterOrderType == "O")
                    {
                        _ItemListModel = DSet.Tables[0].AsEnumerable()
        .Select((row, index) => new SOTrackingDataModel
        {
            SrNo = row.Field<Int64>("SrNo"),
            app_so_no = row.Field<string>("app_so_no"),
            so_dt = row.Field<string>("so_dt"),
            cust_name = row.Field<string>("cust_name"),
            OrderType = row.Field<string>("OrderType"),
            curr_logo = row.Field<string>("curr_logo"),
            sls_pers_name = row.Field<string>("sls_pers_name"),
            item_id = row.Field<string>("item_id"),
            item_name = row.Field<string>("item_name"),
            uom_alias = row.Field<string>("uom_alias"),
            ord_qty_base = row.Field<string>("ord_qty_base"),
            Pending_qty = row.Field<string>("Pending_qty"),
            ForceClosed = row.Field<string>("ForceClosed"),
            sch_date = row.Field<string>("sch_date"),
            overduedays = row.Field<int>("overduedays"),
            ref_doc_no = row.Field<string>("ref_doc_no"),
            cust_catg = row.Field<string>("cust_catg"),
            cust_port = row.Field<string>("cust_port"),
            cust_zone = row.Field<string>("cust_zone"),
            cust_group = row.Field<string>("cust_group"),
            state = row.Field<string>("state"),
            city = row.Field<string>("city"),
        }).ToList();
                    }
                    else
                    {
                        _ItemListModel = DSet.Tables[0].AsEnumerable()
           .Select((row, index) => new SOTrackingDataModel
           {
               SrNo = row.Field<Int64>("SrNo"),
               app_so_no = row.Field<string>("app_so_no"),
               so_dt = row.Field<string>("so_dt"),
               cust_name = row.Field<string>("cust_name"),
               OrderType = row.Field<string>("OrderType"),
               curr_logo = row.Field<string>("curr_logo"),
               sls_pers_name = row.Field<string>("sls_pers_name"),
               item_id = row.Field<string>("item_id"),
               item_name = row.Field<string>("item_name"),
               uom_alias = row.Field<string>("uom_alias"),
               ord_qty_base = row.Field<string>("ord_qty_base"),
               Pending_qty = row.Field<string>("Pending_qty"),
               ForceClosed = row.Field<string>("ForceClosed"),
               pack_no = row.Field<string>("pack_no"),
               pack_dt = row.Field<string>("pack_dt"),
               pack_qty = row.Field<string>("pack_qty"),
               ship_no = row.Field<string>("ship_no"),
               ship_dt = row.Field<string>("ship_dt"),
               ship_qty = row.Field<string>("ship_qty"),
               app_inv_no = row.Field<string>("app_inv_no"),
               inv_dt = row.Field<string>("inv_dt"),
               invQty = row.Field<string>("invQty"),
               srt_no = row.Field<string>("srt_no"),
               srt_dt = row.Field<string>("srt_dt"),
               srt_qty = row.Field<string>("srt_qty"),
               ref_doc_no = row.Field<string>("ref_doc_no"),
               cust_catg = row.Field<string>("cust_catg"),
               cust_port = row.Field<string>("cust_port"),
               cust_zone = row.Field<string>("cust_zone"),
               cust_group = row.Field<string>("cust_group"),
               state = row.Field<string>("state"),
               city = row.Field<string>("city"),
               br_id = row.Field<int>("br_id")
           }).ToList();
                    }
                }
                if (DSet.Tables.Count >= 2)
                {
                    
                    if (DSet.Tables[1].Rows.Count > 0)
                    {
                        Total_Records = Convert.ToInt32(DSet.Tables[1].Rows[0]["total_rows"]);
                    }
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
            return (_ItemListModel, Total_Records);
        }
        public ActionResult SalesTrackingActionCommands(SalestrackingModel _Model, string Command)
        {
            try
            {

                switch (Command)
                {
                    case "CSV":
                        return GenrateCsvFile(_Model);
                }
                return RedirectToAction("PurchaseTracking");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public FileResult GenrateCsvFile(SalestrackingModel _Model)
        {
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
                
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
                    User_ID = Session["UserId"].ToString();
                }
                var SearchValue = IsNull(_Model.SearchValue,"");
                var Direction = IsNull(_Model.sortColumnDir, "asc");
                var sortColumnNm = IsNull(_Model.sortColumn, "SrNo");
                DataSet DSet = new DataSet();
                DSet = _salesTrackingRepo.GetSalesTrackingList(CompID, BrId, User_ID, _Model.OrderNo, _Model.CustId
                    , _Model.SlsPersId, _Model.CurrId, _Model.OrderType, _Model.ItemId, _Model.FromDate, _Model.ToDate
                             , _Model.TrackingOrderType,"","","","","","","", "0", "0", SearchValue, sortColumnNm, Direction, "CSV");
                             //, _Model.TrackingOrderType, "0", "0", "", "SrNo", "asc", "CSV");

                DataTable newTable = new DataTable();
                if (_Model.TrackingOrderType == "A")
                {

                    newTable.Columns.Add(" ", typeof(string));
                    newTable.Columns.Add("  ", typeof(string));
                    newTable.Columns.Add("   ", typeof(string));
                    newTable.Columns.Add("Order Details", typeof(string));
                    newTable.Columns.Add("    ", typeof(string));
                    newTable.Columns.Add("     ", typeof(string));
                    newTable.Columns.Add("      ", typeof(string));
                    newTable.Columns.Add("       ", typeof(string));
                    newTable.Columns.Add("        ", typeof(string));
                    newTable.Columns.Add("         ", typeof(string));
                    newTable.Columns.Add("          ", typeof(string));
                    newTable.Columns.Add("           ", typeof(string));
                    newTable.Columns.Add("            ", typeof(string));
                    newTable.Columns.Add("             ", typeof(string));
                    newTable.Columns.Add("              ", typeof(string));
                    newTable.Columns.Add("Shipment Detail", typeof(string));
                    newTable.Columns.Add("                ", typeof(string));
                    newTable.Columns.Add("                 ", typeof(string));
                    newTable.Columns.Add("Invoice Detail", typeof(string));
                    newTable.Columns.Add("                  ", typeof(string));
                    newTable.Columns.Add("                   ", typeof(string));
                    newTable.Columns.Add("Return Detail", typeof(string));
                    newTable.Columns.Add("                    ", typeof(string));
                    newTable.Columns.Add("                     ", typeof(string));

                    newTable.Rows.Add("Sr.No.", "Order Number", "Order Date", "Customer Name", "Sale Type"
                    , "Currency", "Sales Representative", "Product Name"
                    , "UOM", "Quantity", "Pending Quantity"
                    , "Force Closed", "Pack List Number", "Pack Date", "Quantity", "Shipment Number", "Shipment Date", "Quantity"
                    , "Invoice Number", "Invoice Date", "Quantity", "Return Number", "Return Date"
                    , "Quantity"); // Selecting only needed columns
                }
                else
                {
                    newTable.Columns.Add("Sr.No.", typeof(string));
                    newTable.Columns.Add("Order Number", typeof(string));
                    newTable.Columns.Add("Order Date", typeof(string));
                    newTable.Columns.Add("Customer Name", typeof(string));
                    newTable.Columns.Add("Sale Type", typeof(string));
                    newTable.Columns.Add("Currency", typeof(string));
                    newTable.Columns.Add("Sales Representative", typeof(string));
                    newTable.Columns.Add("Product Name", typeof(string));
                    newTable.Columns.Add("UOM", typeof(string));
                    newTable.Columns.Add("Quantity", typeof(string));
                    newTable.Columns.Add("Pending Quantity", typeof(string));
                    newTable.Columns.Add("Force Closed", typeof(string));

                    if (_Model.TrackingOrderType == "O")
                    {
                        newTable.Columns.Add("Due Date", typeof(string));
                        newTable.Columns.Add("Overdue Days", typeof(string));
                    }
                }
                // Copy relevant data from the original table
                foreach (DataRow row in DSet.Tables[0].Rows)
                {
                    if (_Model.TrackingOrderType == "P")
                    {
                        newTable.Rows.Add(row["SrNo"], row["app_so_no"], row["so_dt"], row["cust_name"], row["OrderType"]
                        , row["curr_logo"], row["sls_pers_name"], row["item_name"]
                        , row["uom_alias"], row["ord_qty_base"], row["Pending_qty"]
                        , row["ForceClosed"]); // Selecting only needed columns
                    }
                    else if (_Model.TrackingOrderType == "O")
                    {
                        newTable.Rows.Add(row["SrNo"], row["app_so_no"], row["so_dt"], row["cust_name"], row["OrderType"]
                        , row["curr_logo"], row["sls_pers_name"], row["item_name"]
                        , row["uom_alias"], row["ord_qty_base"], row["Pending_qty"]
                        , row["ForceClosed"], row["sch_date"], row["overduedays"]); // Selecting only needed columns
                    }
                    else
                    {
                        newTable.Rows.Add(row["SrNo"], row["app_so_no"], row["so_dt"], row["cust_name"], row["OrderType"]
                        , row["curr_logo"], row["sls_pers_name"], row["item_name"]
                        , row["uom_alias"], row["ord_qty_base"], row["Pending_qty"]
                        , row["ForceClosed"], row["pack_no"], row["pack_dt"], row["pack_qty"], row["ship_no"], row["ship_dt"], row["ship_qty"]
                        , row["app_inv_no"], row["inv_dt"], row["invQty"], row["srt_no"], row["srt_dt"]
                        , row["srt_qty"]); // Selecting only needed columns
                    }

                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Sales Tracking", newTable);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public static string IsNull(string In, string Out)
        {
            return string.IsNullOrEmpty(In) ? Out : In;
        }
        [HttpPost]
        public ActionResult ExportCsv([System.Web.Http.FromBody] DataTableRequest request
            , string soNo, string custId, string slsPersId, string orderType, string itemId
            , string currId, string fromDate, string toDate
            , string NotFillterOrderType, string custCat, string custPort, string cust_zone, string cust_group, string state, string city,string brlist)
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
            List<SOTrackingDataModel> _ItemListModel = new List<SOTrackingDataModel>();
            (_ItemListModel, recordsTotal) = getDtList(soNo, custId, slsPersId, currId, orderType, itemId, fromDate, toDate
                            , NotFillterOrderType, custCat, custPort, cust_zone, cust_group, state, city, brlist, 0, request.length, keyword, sortColumn, sortColumnDir,"CSV");
            var data = _ItemListModel.ToList(); // All filtered & sorted rows
            var commonController = new CommonController(_Common_IServices);
            return commonController.Cmn_GetDataToCsv(request, data);
        }
        private List<CustPortFolioList> custPortFolioLists()
        {
            List<CustPortFolioList> custcatgList = new List<CustPortFolioList>();
            DataSet ds = _Common_IServices.GetCustCommonDropdownDAL(CompId, "0", "0");
            foreach (DataRow dr in ds.Tables[5].Rows)
            {
                CustPortFolioList custcatg = new CustPortFolioList();
                custcatg.CatPort_id = dr["setup_id"].ToString();
                custcatg.CatPort_val = dr["setup_val"].ToString();
                custcatgList.Add(custcatg);
            }
            return custcatgList;
        }
        private List<CustCategoryList> custCategoryList()
        {
            List<CustCategoryList> custcatgList = new List<CustCategoryList>();
            DataSet ds = _Common_IServices.GetCustCommonDropdownDAL(CompId, "0", "0");
            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                CustCategoryList custcatg = new CustCategoryList();
                custcatg.Cat_id = dr["setup_id"].ToString();
                custcatg.Cat_val = dr["setup_val"].ToString();
                custcatgList.Add(custcatg);
            }
            return custcatgList;
        }
        private List<CustZoneList> CustZoneLists()
        {
            List<CustZoneList> custzoneList = new List<CustZoneList>();
            DataSet ds = _Common_IServices.GetCustCommonDropdownDAL(CompId, "0", "0");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CustZoneList custzone = new CustZoneList();
                custzone.custzone_id = dr["setup_id"].ToString();
                custzone.custzone_val = dr["setup_val"].ToString();
                custzoneList.Add(custzone);
            }
            return custzoneList;
        }
        private List<CustGroupList> CustGroupLists()
        {
            List<CustGroupList> custgroupList = new List<CustGroupList>();
            DataSet ds = _Common_IServices.GetCustCommonDropdownDAL(CompId, "0", "0");
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                CustGroupList custgroup = new CustGroupList();
                custgroup.CustGrp_id = dr["setup_id"].ToString();
                custgroup.CustGrp_val = dr["setup_val"].ToString();
                custgroupList.Add(custgroup);
            }
            return custgroupList;
        }
        public ActionResult BindStateListData(SalestrackingModel _Model)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    if (_Model.SearchState == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _Model.SearchState;
                    }
                    DataSet ProductList = _Common_IServices.GetCustCommonDropdownDAL(CompId, SarchValue, "0");
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
        public ActionResult BindCityListdata(SalestrackingModel _Model)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    if (_Model.SearchCity == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _Model.SearchCity;
                    }
                    var state_id = _Model.state_id;
                    DataSet ProductList = _Common_IServices.GetCustCommonDropdownDAL(CompId, SarchValue, state_id);
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