using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.MIS.PurchaseTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.GoodsReceiptNote;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.MISOrderDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.PurchaseTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.OrderDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.MIS.PurchaseTracking
{
    public class PurchaseTrackingController : Controller
    {
        string DocumentMenuId = "105101155110", title;
        string CompId, BrId, language = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly Purchasetracking_IService _poTracking;
        private readonly ItemList_ISERVICES _itemSetup;
        private readonly GoodsReceiptNote_ISERVICE _GoodsReceiptNote_ISERVICE;
        private readonly OrderDetail_IService _OrderISERVICES;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        private readonly MISOrderDetail_IService _MisOrderDetailsRepo;
        public PurchaseTrackingController(Common_IServices _Common_IServices, Purchasetracking_IService poTracking,
            ItemList_ISERVICES itemSetup, GoodsReceiptNote_ISERVICE GoodsReceiptNote_ISERVICE, OrderDetail_IService OrderISERVICES
            , GeneralLedger_ISERVICE GeneralLedger_ISERVICE, MISOrderDetail_IService MisOrderDetailsRepo)
        {
            this._Common_IServices = _Common_IServices;
            _poTracking = poTracking;
            _itemSetup = itemSetup;
            _GoodsReceiptNote_ISERVICE = GoodsReceiptNote_ISERVICE;
            _OrderISERVICES = OrderISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
            _MisOrderDetailsRepo = MisOrderDetailsRepo;
        }
        // GET: ApplicationLayer/PurchaseTracking
        public ActionResult PurchaseTracking()
        {
            string CompID = string.Empty;
            string userid = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
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
            ViewBag.MenuPageName = getDocumentName();
            PurchaseTrackingModel objModel = new PurchaseTrackingModel();
            objModel.Title = title;
            DateTime dtnow = DateTime.Now;
            //string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            // string FromDate = new DateTime(dtnow.Year, 4, 1).ToString("yyyy-MM-dd"); /*Modified Month By Nitesh 05-12-2023 add Fincacial Year in From Date*/
            DataSet dttbl = new DataSet();
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
            // objModel.FromDate = FromDate;
            objModel.ToDate = ToDate;
            //objModel.ItemsList = GetItemsList();
            //objModel.SuppliersList = GetSuppliersList();/*Commented by Hina on 05-12-2024*/
            GetAllDropDownData(objModel);/*Add by Hina on 05-12-2024*/
            objModel.ItemsList = GetItemsList();
            objModel.Currencylist = GetCurrencyList();
            List<OrderNumberListModel> LstOrder = new List<OrderNumberListModel>();
            OrderNumberListModel objOrder = new OrderNumberListModel()
            {
                OrderNumber = "---All---",
                po_Value = "0"
            };
            LstOrder.Add(objOrder);
            objModel.OrderType1 = "A";
            objModel.PoNumberList = LstOrder;
            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(CompID, userid);
            ViewBag.br_list = br_list;
            objModel.categoryLists = SuppCategoryList();
            objModel.portFolioLists = SuppPortFolioLists();
            //ViewBag.POTrackingDetails = GetPOTrackingDetails("", "", "", "", "", FromDate, ToDate,"", objModel.OrderType1);
            return View("~/Areas/ApplicationLayer/Views/Procurement/MIS/PurchaseTracking/PurchaseTracking.cshtml", objModel);
        }
        private void GetAllDropDownData(PurchaseTrackingModel _Model)/*Created by Hina sharma on 05-12-2024 */
        {
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string SupplierName = string.Empty;

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
            if (string.IsNullOrEmpty(_Model.SuppId))
            {
                SupplierName = "0";
            }
            else
            {
                SupplierName = _Model.SuppId;
            }

            DataSet ds = _MisOrderDetailsRepo.GetAllDDLData(Comp_ID, Br_ID, SupplierName);
            //------Bind Supplier List------------
            List<SupplierModel> _SuppList = new List<SupplierModel>();
            foreach (DataRow data in ds.Tables[0].Rows)
            {
                SupplierModel _SuppDetail = new SupplierModel();
                _SuppDetail.SuppId = data["supp_id"].ToString();
                _SuppDetail.SuppName = data["supp_name"].ToString();
                _SuppList.Add(_SuppDetail);
            }
           // _SuppList.Insert(0, new SupplierModel() { SuppId = "0", SuppName = "All" });
            _Model.SuppliersList = _SuppList;
        }

        public ActionResult ShowReportFromDashBoard(string FromDt, string ToDt)
        {
            string CompID = string.Empty;
            string userid = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
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
            ViewBag.MenuPageName = getDocumentName();
            PurchaseTrackingModel objModel = new PurchaseTrackingModel();
            objModel.Title = title;
            objModel.FromDate = FromDt;
            objModel.ToDate = ToDt;
            GetAllDropDownData(objModel);
            objModel.ItemsList = GetItemsList();
            objModel.Currencylist = GetCurrencyList();
            List<OrderNumberListModel> LstOrder = new List<OrderNumberListModel>();
            OrderNumberListModel objOrder = new OrderNumberListModel()
            {
                OrderNumber = "---All---",
                po_Value = "0"
            };
            LstOrder.Add(objOrder);
            objModel.OrderType1 = "A";
            objModel.PoNumberList = LstOrder;
            objModel.categoryLists = SuppCategoryList();
            objModel.portFolioLists = SuppPortFolioLists();
            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(CompID, userid);
            ViewBag.br_list = br_list;

            return View("~/Areas/ApplicationLayer/Views/Procurement/MIS/PurchaseTracking/PurchaseTracking.cshtml", objModel);
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
        public DataTable GetPOTrackingDetails(string poNo, string suppId, string orderType, string itemId, string currId, string fromDate, string toDate,string ItemType
            ,string NotFillterOrderType)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                DataTable dt = _poTracking.GetPoTrackingDetailsMIS(CompId, BrId, poNo, suppId, orderType, itemId, currId
                    , fromDate, toDate, ItemType, NotFillterOrderType,0,25,"",null,"0","0",null).Tables[0];
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

                //DataTable dt = _itemSetup.BindGetItemList("", compId, BrId);
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
        private List<SupplierModel> GetSuppliersList()
        {
            string compId = string.Empty;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            SuppList = _GoodsReceiptNote_ISERVICE.GetSupplierList(compId, "0", BrId);
            SuppList.Remove("0");

            List<SupplierModel> _SuppList = new List<SupplierModel>();

            SupplierModel _SuppDetail1 = new SupplierModel();
            _SuppDetail1.SuppId = "0";
            _SuppDetail1.SuppName = "---All---";
            _SuppList.Add(_SuppDetail1);


            foreach (var data in SuppList)
            {
                SupplierModel _SuppDetail = new SupplierModel();
                _SuppDetail.SuppId = data.Key;
                _SuppDetail.SuppName = data.Value;
                _SuppList.Add(_SuppDetail);
            }

            return _SuppList;
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
        public JsonResult GetOrderNumberListByOrder(string orderType, string suppId, string currId,string SearchName,string SuppCat,string SuppPort)
        {
            JsonResult DataRows = null;
            try
            {
                if (Session["CompId"] != null)
                    CompId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                DataTable dt = _poTracking.GetOrderNumberList(CompId, BrId, orderType, suppId, currId, SuppCat, SuppPort,SearchName);

                DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult SearchPoTrackingDetails(string poNo, string suppId, string orderType, string itemId, string currId, string fromDate, string toDate,string ItemType
            , string NotFillterOrderType)
        {
            try
            {
                PurchaseTrackingModel objModel = new PurchaseTrackingModel();
                objModel.SearchStatus = "SEARCH";
                objModel.OrderType1 = NotFillterOrderType;
                DataTable dt = new DataTable();
                 dt = GetPOTrackingDetails(poNo, suppId, orderType, itemId, currId, fromDate, toDate,ItemType, NotFillterOrderType);
                if (NotFillterOrderType == "O")
                {
                    ViewBag.POTrackingDetailsOverdue = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMisPOTrackingOverdue.cshtml", objModel);
                }
                else if(NotFillterOrderType == "A")
                {
                    ViewBag.POTrackingDetails = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMisPOTrackingDetails.cshtml", objModel);
                }
                else  if(NotFillterOrderType == "P")
                {
                    ViewBag.POTrackingDetailspending = dt;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMisPOTrackingDetailsPending.cshtml", objModel);
                }
                else
                {
                    return null;
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
            ViewBag.ItemDelSchdetails = _Common_IServices.Cmn_GetDeliverySchudule(CompId, br_id, OrderType, orderNo, item_id, DocumentMenuId);
            return PartialView("~/Areas/Common/Views/Comn_PartialMISOrderDeliverySchedule.cshtml", null);
        }

        public JsonResult LoadPoTrackingDetailsData(string ItemListFilter)
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

                List<POTrackingDataModel> _ItemListModel = new List<POTrackingDataModel>();

                (_ItemListModel, recordsTotal) = getDtList(ItemListFilter, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                //}

                //Search
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    searchValue = searchValue.ToUpper();
                //    ItemListData = ItemListData.Where(m => m.supp_name.ToUpper().Contains(searchValue)
                //    || m.HSNCode.ToUpper().Contains(searchValue)
                //    || m.uomname.ToUpper().Contains(searchValue) || m.brname.ToUpper().Contains(searchValue) || m.opening.ToUpper().Contains(searchValue)
                //    || m.receipts.ToUpper().Contains(searchValue) || m.issued.ToUpper().Contains(searchValue) || m.reserved.ToUpper().Contains(searchValue)
                //    || m.rejected.ToUpper().Contains(searchValue) || m.reworkabled.ToUpper().Contains(searchValue) || m.totalstk.ToUpper().Contains(searchValue)
                //    || m.totalstkval.ToUpper().Contains(searchValue) || m.avlstk.ToUpper().Contains(searchValue) || m.avlstkvalue.ToUpper().Contains(searchValue)
                //    || m.sub_item_id.ToUpper().Contains(searchValue) || m.sub_item_name.ToUpper().Contains(searchValue) || m.exp_dt.ToUpper().Contains(searchValue)
                //    || m.min_stk_lvl.ToUpper().Contains(searchValue)
                //    );
                //}

                //total number of rows count     
                //recordsTotal = ItemListData.Count();

                //Paging     
                var data = ItemListData.ToList();//.Skip(skip).Take(pageSize).ToList();
                //var data = ItemListData.Take(pageSize).ToList();
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
        private (List<POTrackingDataModel>,int) getDtList(string ItemListFilter, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir,string Flag="")
        {
            List<POTrackingDataModel> _ItemListModel = new List<POTrackingDataModel>();
            CommonController cmn = new CommonController();
            int Total_Records = 0;
            try
            {
                string User_ID = string.Empty;
                string SuppCat, SuppPort = string.Empty;
                string poNo, suppId, orderType, itemId, currId, fromDate, toDate, ItemType, NotFillterOrderType = string.Empty, bridlist;
                if (Session["CompId"] != null)
                {
                    CompId = Session["CompId"].ToString();
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
                if (ItemListFilter != null)
                {
                    if (ItemListFilter != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = ItemListFilter;
                        fdata = Fstring.Split('_');

                        // NotFillterOrderType = fdata[0];
                        NotFillterOrderType = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        orderType = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        ItemType = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        suppId = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        poNo = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        itemId = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));
                        currId = cmn.ReplaceDefault(fdata[6].Trim('[', ']'));
                        fromDate = fdata[7].Trim('[', ']');
                        toDate = fdata[8].Trim('[', ']');
                        SuppCat = cmn.ReplaceDefault(fdata[9].Trim('[', ']'));
                        SuppPort = cmn.ReplaceDefault(fdata[10].Trim('[', ']'));
                        bridlist = cmn.ReplaceDefault(fdata[11].Trim('[', ']'));

                        DSet = _poTracking.GetPoTrackingDetailsMIS(CompId, BrId, poNo, suppId, orderType, itemId, currId
                            , fromDate, toDate, ItemType, NotFillterOrderType,skip,pageSize,searchValue, sortColumn, sortColumnDir, SuppCat, SuppPort,Flag, bridlist);
                    }
                    else
                    {
                       
                    }
                }
                else
                {
                   
                }
                if (DSet.Tables[0].Rows.Count > 0)
                {
                    if (NotFillterOrderType == "P")
                    {
                        _ItemListModel = DSet.Tables[0].AsEnumerable()
           .Select((row, index) => new POTrackingDataModel
           {
               SrNo = row.Field<Int64>("SrNo"),
               supp_name = row.Field<string>("supp_name"),
               city_name = row.Field<string>("city_name"),
               app_po_no = row.Field<string>("app_po_no"),
               po_dt = row.Field<string>("po_dt"),
               curr_logo = row.Field<string>("curr_logo"),
               item_id = row.Field<string>("item_id"),
               order_type = row.Field<string>("order_type"),
               item_name = row.Field<string>("item_name"),
               uom_name = row.Field<string>("uom_name"),
               item_type = row.Field<string>("item_type"),
               ForceClose = row.Field<string>("ForceClose"),
               ord_qty_base = row.Field<string>("ord_qty_base"),
               pending_qty = row.Field<string>("pending_qty"),
               sch_date = row.Field<string>("sch_date"),
               overduedays = Convert.ToInt32(row.Field<string>("overduedays")),
               supp_cat_name = row.Field<string>("supp_cat_name"),
               supp_port_name = row.Field<string>("supp_port_name"),
               br_id = row.Field<int>("br_id")
           }).ToList();
                    }
                    else if (NotFillterOrderType == "O")
                    {
                        _ItemListModel = DSet.Tables[0].AsEnumerable()
        .Select((row, index) => new POTrackingDataModel
        {
            SrNo = Convert.ToInt32(row.Field<Int64>("SrNo")),
            supp_name = row.Field<string>("supp_name"),
            city_name = row.Field<string>("city_name"),
            app_po_no = row.Field<string>("app_po_no"),
            po_dt = row.Field<string>("po_dt"),
            curr_logo = row.Field<string>("curr_logo"),
            item_id = row.Field<string>("item_id"),
            order_type = row.Field<string>("order_type"),
            item_name = row.Field<string>("item_name"),
            uom_name = row.Field<string>("uom_name"),
            item_type = row.Field<string>("item_type"),
            ForceClose = row.Field<string>("ForceClose"),
            ord_qty_base = row.Field<string>("delv_qty"),
            pending_qty = row.Field<string>("pending_qty"),
            sch_date = row.Field<string>("sch_date"),
            overduedays = row.Field<int>("overduedays"),
            supp_cat_name = row.Field<string>("supp_cat_name"),
            supp_port_name = row.Field<string>("supp_port_name"),
             br_id = row.Field<int>("br_id")
        }).ToList();
                    }
                    else
                    {
                        _ItemListModel = DSet.Tables[0].AsEnumerable()
           .Select((row, index) => new POTrackingDataModel
           {
               SrNo = row.Field<Int64>("SrNo"),
               supp_name = row.Field<string>("supp_name"),
               city_name = row.Field<string>("city_name"),
               app_po_no = row.Field<string>("app_po_no"),
               po_dt = row.Field<string>("po_dt"),
               bill_no = row.Field<string>("bill_no"),
               bill_dt = row.Field<string>("bill_dt"),
               curr_logo = row.Field<string>("curr_logo"),
               item_id = row.Field<string>("item_id"),
               order_type = row.Field<string>("order_type"),
               item_name = row.Field<string>("item_name"),
               uom_name = row.Field<string>("uom_name"),
               item_type = row.Field<string>("item_type"),
               ForceClose = row.Field<string>("ForceClose"),
               ord_qty_base = row.Field<string>("ord_qty_base"),
               pending_qty = row.Field<string>("pending_qty"),
               sch_date = row.Field<string>("sch_date"),
               overduedays =Convert.ToInt32(row.Field<string>("overduedays")),

               supp_cat_name = row.Field<string>("supp_cat_name"),
               supp_port_name = row.Field<string>("supp_port_name"),

               dn_no = row.Field<string>("dn_no"),
               dn_dt = row.Field<string>("dn_dt"),
               dnQty = row.Field<string>("dnQty"),
               qc_no = row.Field<string>("qc_no"),
               qc_dt = row.Field<string>("qc_dt"),
               accept_qty = row.Field<string>("accept_qty"),
               reject_qty = row.Field<string>("reject_qty"),
               rework_qty = row.Field<string>("rework_qty"),
               short_qty = row.Field<string>("short_qty"),
               sample_qty = row.Field<string>("sample_qty"),
               mr_no = row.Field<string>("mr_no"),
               mr_dt = row.Field<string>("mr_dt"),
               mr_qty = row.Field<string>("mr_qty"),
               inv_no = row.Field<string>("inv_no"),
               inv_dt = row.Field<string>("inv_dt"),
               inv_qty = row.Field<string>("inv_qty"),
               br_id = row.Field<int>("br_id"),
               prt_no = row.Field<string>("prt_no"),
               prt_dt = row.Field<string>("prt_dt"),
               prt_qty = row.Field<string>("PrtQty")
           }).ToList();
                    }


                }
                if (DSet.Tables.Count >= 2 )
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
        [HttpPost]
        public ActionResult ExportCsv([System.Web.Http.FromBody] DataTableRequest request)
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

            List<POTrackingDataModel> _ItemListModel = new List<POTrackingDataModel>();

            (_ItemListModel, recordsTotal) = getDtList(request.ItemListFilter, 0, request.length, keyword, sortColumn, sortColumnDir,"CSV");
            
            var data = _ItemListModel.ToList(); // All filtered & sorted rows
            var commonController = new CommonController(_Common_IServices);
            return commonController.Cmn_GetDataToCsv(request, data);


        }
        public ActionResult PurchaseTrackingActionCommands(PurchaseTrackingModel _Model, string Command)
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
        public FileResult GenrateCsvFile(PurchaseTrackingModel _Model)
        {
            try
            {
                string User_ID = string.Empty;
                string SuppCat, SuppPort = string.Empty;
                string poNo, suppId, orderType, itemId, currId, fromDate, toDate, ItemType, NotFillterOrderType = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompId = Session["CompId"].ToString();
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
                if (!string.IsNullOrEmpty(_Model.Filters))
                {
                    string Fstring = string.Empty;
                    string[] fdata;
                    Fstring = _Model.Filters;
                    fdata = Fstring.Split(',');

                    NotFillterOrderType = fdata[0];
                    orderType = fdata[1];
                    ItemType = fdata[2];
                    suppId = fdata[3];
                    poNo = fdata[4];
                    itemId = fdata[5];
                    currId = fdata[6];
                    fromDate = fdata[7];
                    toDate = fdata[8];
                    SuppCat = fdata[9];
                    SuppPort = fdata[10];

                    var SearchValue = IsNull(_Model.SearchValue, "");
                    var Direction = IsNull(_Model.sortColumnDir, "asc");
                    var sortColumnNm = IsNull(_Model.sortColumn, "SrNo");
                    DSet = _poTracking.GetPoTrackingDetailsMIS(CompId, BrId, poNo, suppId, orderType, itemId, currId
                        , fromDate, toDate, ItemType, NotFillterOrderType, 0, 50, SearchValue, sortColumnNm, Direction, SuppCat,SuppPort, "CSV");
                }
                DataTable newTable = new DataTable();

                newTable.Columns.Add("Sr.No.", typeof(string));
                newTable.Columns.Add("Supplier Name", typeof(string));
                newTable.Columns.Add("City Name", typeof(string));
                newTable.Columns.Add("Order Number", typeof(string));
                newTable.Columns.Add("Order Date", typeof(string));
                if (NotFillterOrderType == "A")
                {
                    newTable.Columns.Add("Bill Number", typeof(string));
                    newTable.Columns.Add("Bill Date", typeof(string));
                }
                newTable.Columns.Add("Currency", typeof(string));
                newTable.Columns.Add("Item Name", typeof(string));
                newTable.Columns.Add("UOM", typeof(string));
                newTable.Columns.Add("Item Type", typeof(string));
                newTable.Columns.Add("Force Closed", typeof(string));
                newTable.Columns.Add("Order Quantity", typeof(string));
                newTable.Columns.Add("Pending Quantity", typeof(string));
                //if (NotFillterOrderType == "O")
                //{
                    newTable.Columns.Add("Due Date", typeof(string));
                    newTable.Columns.Add("Overdue Days", typeof(string));
               // }
                if (NotFillterOrderType == "A")
                {
                    newTable.Columns.Add("Gate Entry Number", typeof(string));
                    newTable.Columns.Add("Deliver Date", typeof(string));
                    newTable.Columns.Add("Delivery Quantity", typeof(string));
                    newTable.Columns.Add("QC Number", typeof(string));
                    newTable.Columns.Add("QC Date", typeof(string));
                    newTable.Columns.Add("  Quantity", typeof(string));
                    newTable.Columns.Add("Rejected Quantity", typeof(string));
                    newTable.Columns.Add("Reworkable Quantity", typeof(string));
                    newTable.Columns.Add("Short Quantity", typeof(string));
                    newTable.Columns.Add("Sample Quantity", typeof(string));
                    newTable.Columns.Add("Receipt Number", typeof(string));
                    newTable.Columns.Add("Receipt Date", typeof(string));
                    newTable.Columns.Add("Receipt Quantity", typeof(string));
                    newTable.Columns.Add("Invoice Number", typeof(string));
                    newTable.Columns.Add("Invoice Date", typeof(string));
                    newTable.Columns.Add("Invoice Quantity", typeof(string));
                }
                

                // Copy relevant data from the original table
                foreach (DataRow row in DSet.Tables[0].Rows)
                {
                    if (NotFillterOrderType == "P")
                    {
                        newTable.Rows.Add(row["SrNo"], row["supp_name"], row["city_name"], row["app_po_no"], row["po_dt"]
                        , row["curr_logo"], row["item_name"], row["uom_name"]
                        , row["item_type"], row["ForceClose"], row["ord_qty_base"]
                        , row["pending_qty"], row["sch_date"], row["overduedays"]); // Selecting only needed columns
                    }
                    else if (NotFillterOrderType == "O")
                    {
                        newTable.Rows.Add(row["SrNo"], row["supp_name"], row["city_name"], row["app_po_no"], row["po_dt"]
                        , row["curr_logo"], row["item_name"], row["uom_name"]
                        , row["item_type"], row["ForceClose"], row["delv_qty"]
                        , row["pending_qty"], row["sch_date"], row["overduedays"]); // Selecting only needed columns
                    }
                    else
                    {
                        newTable.Rows.Add(row["SrNo"], row["supp_name"], row["city_name"], row["app_po_no"], row["po_dt"]
                        , row["bill_no"], row["bill_dt"], row["curr_logo"], row["item_name"], row["uom_name"]
                        , row["item_type"], row["ForceClose"], row["ord_qty_base"]
                        , row["pending_qty"], row["sch_date"], row["overduedays"], row["dn_no"], row["dn_dt"], row["dnQty"], row["qc_no"], row["qc_dt"], row["accept_qty"]
                        , row["reject_qty"], row["rework_qty"], row["short_qty"], row["sample_qty"], row["mr_no"]
                        , row["mr_dt"], row["mr_qty"], row["inv_no"], row["inv_dt"], row["inv_qty"]); // Selecting only needed columns
                    }
                        
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Purchase Tracking", newTable);
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
        private List<SuppCategoryList> SuppCategoryList()
        {
            if (Session["CompId"] != null)
                CompId = Session["CompId"].ToString();
            DataSet dt = _MisOrderDetailsRepo.GetcategoryPortfolioDAL(CompId);

            List<SuppCategoryList> lists = new List<SuppCategoryList>();
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                SuppCategoryList list = new SuppCategoryList();
                list.Cat_id = dr["setup_id"].ToString();
                list.Cat_val = dr["setup_val"].ToString();
                lists.Add(list);
            }
            return lists;
        }
        private List<SuppPortFolioList> SuppPortFolioLists()
        {
            if (Session["CompId"] != null)
                CompId = Session["CompId"].ToString();
            DataSet dt1 = _MisOrderDetailsRepo.GetcategoryPortfolioDAL(CompId);

            List<SuppPortFolioList> portFolioLists = new List<SuppPortFolioList>();
            foreach (DataRow dr in dt1.Tables[1].Rows)
            {
                SuppPortFolioList custPortFolio = new SuppPortFolioList();
                custPortFolio.Port_id = dr["setup_id"].ToString();
                custPortFolio.Port_val = dr["setup_val"].ToString();
                portFolioLists.Add(custPortFolio);
            }
            return portFolioLists;
        }
    }

}