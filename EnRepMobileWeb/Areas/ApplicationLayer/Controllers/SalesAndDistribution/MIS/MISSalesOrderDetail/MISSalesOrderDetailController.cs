using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.OrderDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.OrderDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.SalesDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.MIS.MISSalesOrderDetail
{
    public class MISSalesOrderDetailController : Controller
    {
        string CompID, BrID, language, userid = String.Empty;
        string DocumentMenuId = "105103190105", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly SalesDetail_ISERVICES Sales_ISERVICES;
        private readonly OrderDetail_IService _OrderISERVICES;
        private readonly ItemList_ISERVICES _itemSetup;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public MISSalesOrderDetailController(Common_IServices _Common_IServices, SalesDetail_ISERVICES Sales_ISERVICES,
            OrderDetail_IService OrderISERVICES, ItemList_ISERVICES itemSetup, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.Sales_ISERVICES = Sales_ISERVICES;
            _OrderISERVICES = OrderISERVICES;
            _itemSetup = itemSetup;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE; ;
        }
        // GET: ApplicationLayer/MISSalesOrderDetail
        public ActionResult MISSalesOrderDetail()
        {
            try
            {
                //Session["SDFilter"] = null;
                OrderDetail_Model _Model = new OrderDetail_Model();
                ViewBag.MenuPageName = getDocumentName();
                DateTime dtnow = DateTime.Now;
                // string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                string ToDate = dtnow.ToString("yyyy-MM-dd");

                DataSet dttbl = new DataSet();
                #region Added By Nitesh  02-01-2024 for Financial Year 
                #endregion
                dttbl = GetFyList();
                if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
                {
                    _Model.From_dt = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                    ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                    ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                    ViewBag.fylist = dttbl.Tables[1];
                }
                string FromDate = _Model.From_dt;
                // _Model.From_dt = FromDate;
                _Model.To_dt = ToDate;

                _Model.categoryLists = custCategoryList();
                _Model.portFolioLists = custPortFolioLists();
                _Model.custzoneList = CustZoneLists();
                _Model.custgroupList = CustGroupLists();
                List<CityList> _CityList = new List<CityList>();
                _Model.CityLists = _CityList;

                List<StateList> _StateList = new List<StateList>();
                _Model.StateLists = _StateList;
                _Model.regionLists = regionLists();
                _Model.Title = title;
                _Model.Currencylist = GetCurrencyList();
                _Model.ItemsList = GetItemList();
                _Model.SoNumberList = GetSoNumberList();
                try
                {
                    OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                    //ViewBag.OrderDetailList = GetOrderDetails("", "", "", "", "", "", "", "", "", "", "", "", FromDate, ToDate, "ORDERWISE");
                    ViewBag.flag = "OrderWiseSummary";
                    ViewBag.OrderDetailList = GetOrderDetails("", "", "", "", "", "", "", "", "", "", "", "", FromDate, ToDate, "OrderWiseSummary","","","","");
                    // Session["SDFilter"] = "SDFilter";

                }
                catch (Exception Ex)
                {
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, Ex);
                    throw Ex;
                }
                //ViewBag.SalesDetailList = GetSales_Details("","", "", "0","", "", "", "","","","", FromDate, ToDate, "Summary");
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/MISSalesOrderDetail/OrderDetail.cshtml", _Model);

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
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

        private List<ItemList> GetItemList()
        {
            try
            {
                string compId = string.Empty;
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrID = Session["BranchId"].ToString();

                DataTable dt = _itemSetup.BindGetItemList("", compId, BrID);
                List<ItemList> itemsList = new List<ItemList>();
                foreach (DataRow dr in dt.Rows)
                {
                    ItemList itmLst = new ItemList
                    {
                        itemId = dr["item_id"].ToString(),
                        itemName = dr["item_name"].ToString()
                    };
                    itemsList.Add(itmLst);
                }
                itemsList.Insert(0, new ItemList { itemId = "0", itemName = "---All---" });
                return itemsList;
            }
            catch (Exception exc)
            {
                throw exc;
            }
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
           // lists.Insert(0, new CustCategoryList() { Cat_id = "0", Cat_val = "---All---" });
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
        public List<CurrencyList> GetCurrencyList()
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
        private List<SoNumberList> GetSoNumberList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            List<SoNumberList> soList = new List<SoNumberList>();
            SoNumberList objsolst = new SoNumberList
            {
                so_no = "---All---",
                so_dt = "---All---"
            };
            soList.Add(objsolst);
            DataTable dtsolst = _OrderISERVICES.GetSONumberList(CompID, BrID);
            if (dtsolst.Rows.Count > 0)
            {
                for (int i = 0; i < dtsolst.Rows.Count; i++)
                {

                     objsolst = new SoNumberList
                    {
                        so_no = dtsolst.Rows[i]["so_no"].ToString(),
                        so_dt = dtsolst.Rows[i]["so_dt"].ToString()
                    };
                    soList.Add(objsolst);
                }
            }
            return soList;
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
                DataTable dt = Sales_ISERVICES.GetRegionDAL(Comp_ID);
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
                DataTable dt = Sales_ISERVICES.GetCustportDAL(Comp_ID);
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
                DataTable dt = Sales_ISERVICES.GetcategoryDAL(Comp_ID);
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
        public ActionResult GetAutoCompleteSearchCustList()
        {
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string CustType = string.Empty;
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
                CustType = "";
                DataSet dt = Sales_ISERVICES.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustType);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        CustList.Add(dt.Tables[0].Rows[i]["cust_id"].ToString(), dt.Tables[0].Rows[i]["cust_name"].ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public DataTable GetOrderDetails(string cust_id, string reg_name, string sale_type, string curr_id, string productGrp, string Product_Id, string productPort,
                  string custCat, string CustPort, string inv_no, string inv_dt, string sale_per, string From_dt, string To_dt, string Flag, string custzone, string custgroup, string custstate, string custcity)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                    userid = Session["UserId"].ToString();
                //dt = _OrderISERVICES.GetOrder_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag).Tables[0];
                ds = _OrderISERVICES.GetOrder_Detail(CompID, BrID, userid, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag, custzone, custgroup, custstate, custcity);
                dt = ds.Tables[0];
                if(Flag == "OrderWiseSummary")
                {
                    ViewBag.OrderWiseSummaryTotal = ds.Tables[1];
                }
                if (Flag == "OrderWiseDetail")
                {
                    ViewBag.OrderWiseDetailTotal = ds.Tables[1];
                }
                if (Flag == "SOCostomerWise")
                {
                    ViewBag.SOCostomerWiseTotal = ds.Tables[1];
                }
                if (Flag == "CustomerWiseDetail")
                {
                    ViewBag.OrderWiseDetailTotal = ds.Tables[1];
                }               
                if (Flag == "ODProductWise")
                {
                    ViewBag.ODProductWiseTotal = ds.Tables[1];
                }
                if (Flag == "ProductWiseDetail")
                {
                    ViewBag.ProductWiseDetailTotal = ds.Tables[1];
                }
                if(Flag == "ODProductGroupWise")
                {
                    ViewBag.ODProductGroupWiseTotal = ds.Tables[1];
                }
                if (Flag == "ProductGroupWiseDetail")
                {
                   ViewBag.ProductGroupWiseDetailTotal = ds.Tables[1];
                }
                if(Flag== "ODRegionWise")
                {
                    ViewBag.ODRegionWiseTotal = ds.Tables[1];
                }
                if (Flag == "RegionWiseDetail")
                {
                    ViewBag.RegionWiseDetailTotal = ds.Tables[1];
                }
                if(Flag == "SOSalePerWise")
                {
                    ViewBag.SOSalePerWiseTotal = ds.Tables[1];
                }
                if(Flag == "SalePersonWiseDetail")
                {
                    ViewBag.SalePersonWiseDetailTotal = ds.Tables[1];
                }
                if(Flag== "PendingOrders")
                {
                    ViewBag.PendingOrdersTotal = ds.Tables[1];
                }
                if (Flag == "PendingOrdDtl")
                {
                    ViewBag.PendingOrdDtlTotal = ds.Tables[1];
                }
                if(Flag== "ItemDetails")
                {
                    ViewBag.TotalItemDetails = ds.Tables[1];
                }
                if(Flag== "CW_Ord_Details")
                {
                    ViewBag.TotalCW_Ord_Details = ds.Tables[1];
                }
                if(Flag== "CW_Ord_Item_Details")
                {
                    ViewBag.TotalCW_Ord_Item_Details = ds.Tables[1];
                }
                if(Flag== "ODProductWiseInvoce")
                {
                    ViewBag.TotalODProductWiseInvoce = ds.Tables[1];
                } 
                if(Flag== "PendingOrdersItemWise")
                {
                    ViewBag.TotalPendingOrdersItemWise = ds.Tables[1];
                }
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetMisOrderDeliverySchedule(string Sono, string Sodt, string item_id)
        {
            #region added By Nitesh 17-01-2024 for Show Delivery Shudule
            #endregion
            OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
            ViewBag.DocumentMenuId = DocumentMenuId;
            //objModel.SearchStatus = "SEARCH";
            ViewBag.ItemDelSchdetails = GetOrderDetails("", "", "", "", "", item_id, "", "", "", Sono, Sodt, "", "", "", "DeliverySchedule", "", "", "", "");
            return PartialView("~/Areas/Common/Views/Comn_PartialMISOrderDeliverySchedule.cshtml", _SalesDetail_Model);
        }

        public ActionResult GetOrderDetailsByFilter(string cust_id, string reg_name, string sale_type, string product_id, string sale_per, string From_dt, string To_dt,
         string currId, string ShowAs, string custCat, string custPort, string cust_zone, string cust_group, string state, string city)
        {
            try
            {
                OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                var flag = "OrderWiseSummary";
                if (ShowAs == "S")
                {
                    flag = "OrderWiseSummary";
                }
                else if (ShowAs == "D")
                {
                    flag = "OrderWiseDetail";
                }
                ViewBag.flag = flag;
                //ViewBag.OrderDetailList = GetOrderDetails(cust_id, reg_name, sale_type, currId, product_id, "", "", "", "", "", "", sale_per, From_dt, To_dt, "ORDERWISE");
                ViewBag.OrderDetailList = GetOrderDetails(cust_id, reg_name, sale_type, currId, product_id, "", "", custCat, custPort, "", "", sale_per, From_dt, To_dt, flag, cust_zone,cust_group, state, city);
                // Session["SDFilter"] = "SDFilter";
                _SalesDetail_Model.ODFilter = "ODFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesOrderDetailsOrderWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public ActionResult GetOrderItemDetails(string inv_no, string inv_dt, string curr_id, string from_dt, string to_dt)
        {
            try
            {
                ViewBag.OrderItemDetailList = GetOrderDetails("", "", "", curr_id, "0", "", "", "", "", inv_no, inv_dt, "", from_dt, to_dt, "ItemDetails", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetOrderDetailsCostomer(string cust_id, string reg_name, string sale_type, string curr_id,
       string custCat, string custPort, string From_dt, string To_dt,string ShowAs,string cust_zone, string cust_group, string state, string city)
        {
            try
            {
                OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                var flag = "SOCostomerWise";
                if (ShowAs == "S")
                {
                    flag = "SOCostomerWise";
                }
                else if (ShowAs == "D")
                {
                    flag = "CustomerWiseDetail";
                }
                ViewBag.flag = flag;
                ViewBag.OrderDetailCostomerWiseList = GetOrderDetails(cust_id, reg_name, sale_type, curr_id, "", "", "", custCat, custPort, "", "", "", From_dt, To_dt, flag, cust_zone, cust_group, state, city);
                //Session["SDFilter"] = "SDFilter";
                _SalesDetail_Model.ODFilter = "ODFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesOrderDetailsCustomerWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetCW_Sales_Invoice_Item_Details(string Cust_Id, string inv_no, string inv_dt, string curr_id, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.CWInvoiceItemDetailList = GetOrderDetails(Cust_Id, "", "", curr_id, "0", "", "", "", "", inv_no, inv_dt, "", From_dt, To_dt, "CW_Ord_Item_Details", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/CustomerOrderInvoiceProductDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetCW_Sales_Order_Details(string Cust_Id, string curr_id, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.CWOrderDetailList = GetOrderDetails(Cust_Id, "", "", curr_id, "0", "", "", "", "", "", "", "", From_dt, To_dt, "CW_Ord_Details", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderCustomerInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public ActionResult GetOrder_DetailsProductWiseByFilter(string sale_type, string curr_id, string productgrp,
        string productPort, string From_dt, string To_dt, string itemId,string ShowAs)
        {
            try
            {
                OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                var flag = "ODProductWise";
                if (ShowAs == "S")
                {
                    flag = "ODProductWise";
                }
                else if (ShowAs == "D")
                {
                    flag = "ProductWiseDetail";
                }
                ViewBag.flag = flag;
                //ViewBag.OrderDetailProductWiseList = GetOrderDetails(sale_type, "", sale_type, curr_id, productgrp, itemId, productPort, "", "", "", "", "", From_dt, To_dt, "ODProductWise");
                ViewBag.OrderDetailProductWiseList = GetOrderDetails(sale_type, "", sale_type, curr_id, productgrp, itemId, productPort, "", "", "", "", "", From_dt, To_dt, flag, "", "", "", "");
                //TempData["SDFilter"] = "SDFilter";
                _SalesDetail_Model.ODFilter = "ODFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesOrderDetailsProductWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetPW_Sales_Order_Details(string Product_Id, string sale_type, string curr_id, string productgrp, string productPort, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.OrderDetailProductWiseInvoiceList = GetOrderDetails("", "", sale_type, curr_id, productgrp, Product_Id, productPort, "", "", "", "", "", From_dt, To_dt, "ODProductWiseInvoce", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderProductWiseInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public ActionResult GetOrder_DetailsProductGroupWiseByFilter(string sale_type, string curr_id, string productgrp, string productPort, string From_dt, string To_dt,string ShowAs)
        {
            try
            {
                OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                var flag = "ODProductGroupWise";
                if (ShowAs == "S")
                {
                    flag = "ODProductGroupWise";
                }
                else if (ShowAs == "D")
                {
                    flag = "ProductGroupWiseDetail";
                }
                ViewBag.flag = flag;
                //ViewBag.OrderDetailProductGroupWiseList = GetOrderDetails(sale_type, "", sale_type, curr_id, productgrp, "", productPort, "", "", "", "", "", From_dt, To_dt, "ODProductGroupWise");
                ViewBag.OrderDetailProductGroupWiseList = GetOrderDetails(sale_type, "", sale_type, curr_id, productgrp, "", productPort, "", "", "", "", "", From_dt, To_dt, flag, "", "", "", "");
                //TempData["SDFilter"] = "SDFilter";
                _SalesDetail_Model.ODFilter = "ODFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesOrderDetailsProductGroupWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetPGW_Sales_Invoice_product_Details(string sale_type, string curr_id, string productgrp, string productPort,
   string From_dt, string To_dt)
        {
            try
            {
                ViewBag.SalesDetailProductGrpWisePrdctList = GetOrderDetails("", "", sale_type, curr_id, productgrp, "", productPort, "", "", "", "", "", From_dt, To_dt, "ODProductWise", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderProductGroupWiseProductDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public ActionResult GetPGW_Sales_Invoice_Details(string Product_Id, string sale_type, string curr_id, string productgrp, string productPort,
      string From_dt, string To_dt)
        {
            try
            {
                ViewBag.SalesDetailProductWiseInvoiceList = GetOrderDetails("", "", sale_type, curr_id, productgrp, Product_Id, productPort, "", "", "", "", "", From_dt, To_dt, "ODProductWiseInvoce", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderProductGroupWiseInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        /*---------------------------------- Region Wise Sales Details ------------------------------------------ */
        public ActionResult GetSales_DetailsRegionWiseByFilter(string sale_type, string curr_id, string RegionName,
        string From_dt, string To_dt,string ShowAs)
        {
            try
            {
                var flag = "ODRegionWise";
                if (ShowAs == "S")
                {
                    flag = "ODRegionWise";
                }
                else if (ShowAs == "D")
                {
                    flag = "RegionWiseDetail";
                }
                ViewBag.flag = flag;
                ViewBag.OrderDetailRegionWiseList = GetOrderDetails(sale_type, RegionName, sale_type, curr_id, "", "", "", "", "", "", "", "", From_dt, To_dt, flag, "", "", "", "");
                //TempData["SDFilter"] = "SDFilter";
                OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                _SalesDetail_Model.ODFilter = "ODFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesOrderDetailsRegionWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetRegionWise_Customer_Details(string sale_type, string curr_id, string Region_Id, string productPort, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.OrderDetailRegionWiseCustomerList = GetOrderDetails("", Region_Id, sale_type, curr_id, "0", "", productPort, "", "", "", "", "", From_dt, To_dt, "SOCostomerWise", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderRegionWiseCustomerDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetRegionWise_Sales_Invoice_Details(string Cust_Id, string Region_Id, string sale_type, string curr_id, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.RWOrderDetailList = GetOrderDetails(Cust_Id, Region_Id, sale_type, curr_id, "0", "", "", "", "", "", "", "", From_dt, To_dt, "CW_Ord_Details", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderRegionWiseInvoiceSummary.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        public ActionResult GetRegionWise_Sales_Invoice_Product_Details(string Cust_Id, string Region_Id, string sale_type, string curr_id,
          string invoice_no, string invoice_dt, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.RWOrderItemDetailList = GetOrderDetails(Cust_Id, Region_Id, sale_type, curr_id, "0", "", "", "", "", invoice_no, invoice_dt, "", From_dt, To_dt, "CW_Ord_Item_Details", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderRegionWiseInvoiceProductDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        /*---------------------------------- Region Wise Sales Details End------------------------------------------ */

        /*---------------------------------- Sales Representative Wise Sales Details End------------------------------------------ */
        public ActionResult GetSales_DetailsSalePersonWiseByFilter(string sale_type, string curr_id, string sale_per,
       string From_dt, string To_dt,string ShowAs)
        {
            try
            {
                var flag = "SOSalePerWise";
                if (ShowAs == "S")
                {
                    flag = "SOSalePerWise";
                }
                else if (ShowAs == "D")
                {
                    flag = "SalePersonWiseDetail";
                }
                ViewBag.flag = flag;
                ViewBag.OrderDetailSalePerWiseList = GetOrderDetails("", "", sale_type, curr_id, "", "", "", "", "", "", "", sale_per, From_dt, To_dt, flag, "", "", "", "");
                //TempData["SDFilter"] = "SDFilter";               
                OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                _SalesDetail_Model.ODFilter = "ODFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesOrderDetailsSalesExecutiveWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetSalePersonWise_Customer_Details(string sale_type, string curr_id, string sale_per, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.OrderDetailSalePersonWiseCustomerList = GetOrderDetails("", "", sale_type, curr_id, "0", "", "", "", "", "", "", sale_per, From_dt, To_dt, "SOCostomerWise", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderSalesExecutiveWiseCustomerDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetSalePersonWise_Customer_Invoice_Details(string sale_type, string curr_id, string sale_per, string cust_id, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.OrderDetailSalePersonWiseCustomerInvoiceList = GetOrderDetails(cust_id, "", sale_type, curr_id, "0", "", "", "", "", "", "", sale_per, From_dt, To_dt, "CW_Ord_Details", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderSalesExecutiveWiseInvoiceSummary.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetSalePersonWise_Customer_Invoice_Item_Details(string inv_no, string inv_dt, string curr_id, string From_dt, string To_dt, string ShowAs)
        {
            try
            {
                ViewBag.OrderDetailSalePersonWiseCustomerInvoiceItemList = GetOrderDetails("", "", "", curr_id, "0", "", "", "", "", inv_no, inv_dt, "", From_dt, To_dt, "CW_Ord_Item_Details", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderSalesExecutiveWiseProductDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }

        /*---------------------------------- Sales Representative Wise Sales Details End------------------------------------------ */
        public ActionResult GetPendingOrdersDetail(string orderType, string salesRepresentatives, string fromDate, string toDate, string currId, string custId,string ShowAs, string custCat, string custPort, string cust_zone, string cust_group, string state, string city)
        {
            try
            {
                //var flag = "PendingOrdersSummary";
                //if (ShowAs == "S")
                //{
                //    flag = "PendingOrdersSummary";
                //}
                //else if (ShowAs == "D")
                //{
                //    flag = "PendingOrdersDetail";
                //}
                //ViewBag.flag = flag;
                //ViewBag.PendingOrderDetails = GetOrderDetails(custId, "", orderType, currId, "", "", "", "", "", "", "", salesRepresentatives, fromDate, toDate, "PendingOrders");
                var flag = "PendingOrders";
                if (ShowAs == "S")
                {
                    flag = "PendingOrders";
                }
                else if (ShowAs == "D")
                {
                    flag = "PendingOrdDtl";
                }
                ViewBag.flag = flag;
                ViewBag.PendingOrderDetails = GetOrderDetails(custId, "", orderType, currId, "", "", "", custCat, custPort, "", "", salesRepresentatives, fromDate, toDate, flag, cust_zone, cust_group, state, city);
                //TempData["SDFilter"] = "SDFilter";               
                OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                _SalesDetail_Model.ODFilter = "ODFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesOrderDetailsPendingOrderWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetPendingOrdersItemWiseDetail(string so_no, string so_dt)
        {
            try
            {
                ViewBag.PendingOrderItemWiseDetails = GetOrderDetails("", "", "", "", "", "", "", "", "", so_no, so_dt, "", "", "", "PendingOrdersItemWise", "", "", "", "");
                //TempData["SDFilter"] = "SDFilter";               
                OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                _SalesDetail_Model.ODFilter = "ODFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPendingOrderDetail.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult Pending_Sales_Order_Details(string Cust_Id, string curr_id, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.CWOrderDetailList = GetOrderDetails(Cust_Id, "", "", curr_id, "0", "", "", "", "", "", "", "", From_dt, To_dt, "PendingOrdDetails", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPendingOrderCustomerInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        //public ActionResult GetPending_Sales_Order_Details(string Cust_Id, string curr_id, string From_dt, string To_dt)
        //{
        //    try
        //    {
        //        ViewBag.CWOrderDetailList = GetOrderDetails(Cust_Id, "", "", curr_id, "0", "", "", "", "", "", "", "", From_dt, To_dt, "CW_Pending_Ord_Details");
        //        return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOrderCustomerInvoiceDetail.cshtml");
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        throw Ex;
        //    }

        //}
        public ActionResult GetCW_Pending_Sales_Invoice_Item_Details(string Cust_Id, string inv_no, string inv_dt, string curr_id, string From_dt, string To_dt)
        {
            try
            {
                ViewBag.CWInvoiceItemDetailList = GetOrderDetails(Cust_Id, "", "", curr_id, "0", "", "", "", "", inv_no, inv_dt, "", From_dt, To_dt, "CW_Pending_Ord_Item_Details", "", "", "", "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/CustomerOrderInvoiceProductDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetPendingOrderDetails(string custId, string currId, string orderNo, string orderDate,string fromDate, string toDate,string ShowAs)
        {
            try
            {
                var flag = "PendingOrderItemSummary";
                if (ShowAs == "S")
                {
                    flag = "PendingOrderItemSummary";
                }
                else if (ShowAs == "D")
                {
                    flag = "PendingOrderItemDetail";
                }
                ViewBag.flag = flag;
                ViewBag.PendingOrderDetail = GetOrderDetails(custId,"","",currId,"","","","","",orderNo,orderDate,"",fromDate,toDate, "PendingOrdDtl", "", "", "", "");
                //TempData["SDFilter"] = "SDFilter";               
                OrderDetail_Model _SalesDetail_Model = new OrderDetail_Model();
                _SalesDetail_Model.ODFilter = "ODFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesOrderDetailsPendingOrderWiseDetailedList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult DeliveryShudule(string orderNo, string itemId)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            //objModel.SearchStatus = "SEARCH";
            ViewBag.DocumentMenuId = DocumentMenuId;
            ViewBag.ItemDelSchdetails = _Common_IServices.Cmn_GetDeliverySchudule(CompID, BrID, "", orderNo.Trim(), itemId.Trim(), DocumentMenuId);
            return PartialView("~/Areas/Common/Views/Comn_PartialMISOrderDeliverySchedule.cshtml", null);
        }
        public FileResult OrderDetailExporttoExcelDt(string cust_id, string reg_name, string DataShow, string sale_type, string product_id, string sale_per, string From_dt, string To_dt, string ShowAs, string curr_id, string productGrp, string productPort, string custCat, string CustPort, string inv_no, string inv_dt)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                var Flag = "";
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrID = Session["BranchId"].ToString();
                if(DataShow== "OrderWise")
                    if (ShowAs == "S")
                        Flag = "OrderWiseSummary";
                    else if (ShowAs == "D")
                        Flag = "OrderWiseDetail";
                if (DataShow == "ItemDetails")
                        Flag = "ItemDetails";
                if (DataShow == "CW_Ord_Details")
                        Flag = "CW_Ord_Details";
                if (DataShow == "CustomerWise")
                    if (ShowAs == "S")
                        Flag = "SOCostomerWise";
                    else if (ShowAs == "D")
                        Flag = "CustomerWiseDetail";
                if (DataShow == "CW_Ord_Item_Details")
                    Flag = "CW_Ord_Item_Details";
                if (DataShow == "ProductWise")
                    if (ShowAs == "S")
                        Flag = "ODProductWise";
                    else if (ShowAs == "D")
                        Flag = "ProductWiseDetail";
                if (DataShow == "ODProductWiseInvoce")
                    Flag = "ODProductWiseInvoce";
                if (DataShow == "ProductGroupWise")
                    if (ShowAs == "S")
                        Flag = "ODProductGroupWise";
                    else if (ShowAs == "D")
                        Flag = "ProductGroupWiseDetail";
                if (DataShow == "ODProductWise")
                    Flag = "ODProductWise";
                if (DataShow == "RegionWise")
                    if (ShowAs == "S")
                        Flag = "ODRegionWise";
                    else if (ShowAs == "D")
                        Flag = "RegionWiseDetail";
                if (DataShow == "SOCostomerWise")
                    Flag = "SOCostomerWise";
                if (DataShow == "SalePersonWise")
                    if (ShowAs == "S")
                        Flag = "SOSalePerWise";
                    else if (ShowAs == "D")
                        Flag = "SalePersonWiseDetail";
                if (DataShow == "PendingOrdersWise")
                    if (ShowAs == "S")
                        Flag = "PendingOrders";
                    else if (ShowAs == "D")
                        Flag = "PendingOrdDtl";
                if (DataShow == "PendingOrdersItemWise")
                    Flag = "PendingOrdersItemWise";
                if (Session["UserId"] != null)
                    userid = Session["UserId"].ToString();
                ds = _OrderISERVICES.GetOrder_Detail(CompID, BrID, userid, cust_id, reg_name, sale_type, curr_id, productGrp, product_id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag,"","","","");
                if (DataShow == "OrderWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Order Number", typeof(string));
                    dt.Columns.Add("Order Date", typeof(string));
                    dt.Columns.Add("Customer Name", typeof(string));
                    dt.Columns.Add("Region Name", typeof(string));
                    dt.Columns.Add("Order Type", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Sales Representative", typeof(string));
                    if (Flag == "OrderWiseDetail")
                    {
                        dt.Columns.Add("Item Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Order Quantity", typeof(decimal));
                        dt.Columns.Add("Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Price (In Base)", typeof(decimal));
                    }
                    dt.Columns.Add("Gross Value (In Specific)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Base)", typeof(decimal));
                    if (Flag == "OrderWiseSummary")
                    {
                        dt.Columns.Add("Approved On", typeof(string));
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Order Number"] = dr["app_so_no"].ToString();
                            dtrowLines["Order Date"] = dr["so_dt1"].ToString();
                            dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                            dtrowLines["Region Name"] = dr["cust_region"].ToString();
                            dtrowLines["Order Type"] = dr["sale_type"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Sales Representative"] = dr["sale_per"].ToString();
                            if (Flag == "OrderWiseDetail")
                            {
                                dtrowLines["Item Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["Order Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();
                                dtrowLines["Gross Value (In Specific)"] = dr["item_gr_val_spec"].ToString();
                                dtrowLines["Gross Value (In Base)"] = dr["item_gr_val_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                                dtrowLines["Order Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                                dtrowLines["Order Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            }
                            else
                            {
                                dtrowLines["Gross Value (In Specific)"] = dr["sale_amount_spec"].ToString();
                                dtrowLines["Gross Value (In Base)"] = dr["sale_amount_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                                dtrowLines["Order Amount (In Specific)"] = dr["Order_amt_spec"].ToString();
                                dtrowLines["Order Amount (In Base)"] = dr["Order_amt_bs"].ToString();
                                dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if(DataShow== "ItemDetails"||DataShow== "CW_Ord_Item_Details")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Item Name", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("Order Quantity", typeof(decimal));
                    dt.Columns.Add("Price (In Specific)", typeof(decimal));
                    dt.Columns.Add("Price (In Base)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Specific)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Item Name"] = dr["item_name"].ToString();
                            dtrowLines["UOM"] = dr["uom_alias"].ToString();
                            dtrowLines["Order Quantity"] = dr["item_qty"].ToString();
                            dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                            dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();
                            dtrowLines["Gross Value (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Gross Value (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Order Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Order Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if(DataShow== "CustomerWise"||DataShow== "SOCostomerWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Customer Name", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    if(DataShow != "SOCostomerWise")
                    {
                        dt.Columns.Add("Region Name", typeof(string));
                        dt.Columns.Add("Order Type", typeof(string));
                    }               
                    if (Flag == "CustomerWiseDetail")
                    {
                        dt.Columns.Add("Order Number", typeof(string));
                        dt.Columns.Add("Order Date", typeof(string));
                        dt.Columns.Add("Sales Representative", typeof(string));
                    }
                    dt.Columns.Add("Gross Value (In Specific)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Base)", typeof(decimal));
                    if (Flag == "CustomerWiseDetail")
                    {
                        dt.Columns.Add("Approved On", typeof(string));
                    }
                        if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            if (DataShow != "SOCostomerWise")
                            {
                                dtrowLines["Region Name"] = dr["cust_region"].ToString();
                                dtrowLines["Order Type"] = dr["sale_type"].ToString();
                            }
                            if (Flag == "CustomerWiseDetail")
                            {
                                dtrowLines["Order Number"] = dr["app_so_no"].ToString();
                                dtrowLines["Order Date"] = dr["so_dt1"].ToString();
                                dtrowLines["Sales Representative"] = dr["sale_per"].ToString();
                            }
                            dtrowLines["Gross Value (In Specific)"] = dr["sale_amount_spec"].ToString();
                            dtrowLines["Gross Value (In Base)"] = dr["sale_amount_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                            dtrowLines["Order Amount (In Specific)"] = dr["Order_amt_spec"].ToString();
                            dtrowLines["Order Amount (In Base)"] = dr["Order_amt_bs"].ToString();
                            if (Flag == "CustomerWiseDetail")
                            {
                                dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            }
                                dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "CW_Ord_Details"|| DataShow == "ODProductWiseInvoce")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Order Number", typeof(string));
                    dt.Columns.Add("Order Date", typeof(string));
                    if(DataShow == "ODProductWiseInvoce")
                    {
                        dt.Columns.Add("Customer Name", typeof(string));
                        dt.Columns.Add("Region Name", typeof(string));
                        dt.Columns.Add("Quantity", typeof(decimal));
                        dt.Columns.Add("Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Price (In Base)", typeof(decimal));
                    }
                    else
                    {
                        dt.Columns.Add("Sales Representative", typeof(string));
                    }
                    dt.Columns.Add("Gross Value (In Specific)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Approved On", typeof(string));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Order Number"] = dr["app_so_no"].ToString();                           
                            if (DataShow == "ODProductWiseInvoce")
                            {
                                dtrowLines["Order Date"] = dr["so_dt"].ToString();
                                dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                                dtrowLines["Region Name"] = dr["cust_region"].ToString();
                                dtrowLines["Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();
                                dtrowLines["Gross Value (In Specific)"] = dr["item_gr_val_spec"].ToString();
                                dtrowLines["Gross Value (In Base)"] = dr["item_gr_val_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                                dtrowLines["Order Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                                dtrowLines["Order Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            }
                            else
                            {
                                dtrowLines["Order Date"] = dr["so_dt1"].ToString();
                                dtrowLines["Sales Representative"] = dr["sale_per"].ToString();
                                dtrowLines["Gross Value (In Specific)"] = dr["sale_amount_spec"].ToString();
                                dtrowLines["Gross Value (In Base)"] = dr["sale_amount_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                                dtrowLines["Order Amount (In Specific)"] = dr["Order_amt_spec"].ToString();
                                dtrowLines["Order Amount (In Base)"] = dr["Order_amt_bs"].ToString();
                            }                                                           
                            dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if(DataShow== "ProductWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Product Name", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Order Type", typeof(string));
                    if(Flag== "ProductWiseDetail")
                    {
                        dt.Columns.Add("Order Number", typeof(string));
                        dt.Columns.Add("Order Date", typeof(string));
                        dt.Columns.Add("Customer Name", typeof(string));
                        dt.Columns.Add("Region Name", typeof(string));
                    }
                    dt.Columns.Add("Quantity", typeof(decimal));
                    if (Flag == "ProductWiseDetail")
                    {
                        dt.Columns.Add("Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Price (In Base)", typeof(decimal));
                    }
                    else
                    {
                        dt.Columns.Add("Average Price", typeof(decimal));
                        dt.Columns.Add("Discount", typeof(decimal));
                    }     
                    dt.Columns.Add("Gross Value (In Specific)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Product Name"] = dr["item_name"].ToString();
                            dtrowLines["UOM"] = dr["uom_alias"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Order Type"] = dr["sale_type"].ToString();
                            if (Flag == "ProductWiseDetail")
                            {
                                dtrowLines["Order Number"] = dr["app_so_no"].ToString();
                                dtrowLines["Order Date"] = dr["so_dt1"].ToString();
                                dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                                dtrowLines["Region Name"] = dr["cust_region"].ToString();
                            }
                            dtrowLines["Quantity"] = dr["item_qty"].ToString();
                            if (Flag == "ProductWiseDetail")
                            {
                                dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }
                            else
                            {
                                dtrowLines["Average Price"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Discount"] = dr["disc_amt"].ToString();
                            }                           
                            dtrowLines["Gross Value (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Gross Value (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Order Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Order Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "ProductGroupWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Group Structure", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Order Type", typeof(string));
                    if (Flag == "ProductGroupWiseDetail")
                    {
                        dt.Columns.Add("Product Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Quantity", typeof(decimal));
                        dt.Columns.Add("Average Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Average Price (In Base)", typeof(decimal));
                    }  
                    dt.Columns.Add("Gross Value (In Specific)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Base)", typeof(decimal));
                    //dt.Columns.Add("Approved On", typeof(string));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Group Structure"] = dr["group_str"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Order Type"] = dr["sale_type"].ToString();
                            if (Flag == "ProductGroupWiseDetail")
                            {
                                dtrowLines["Product Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Average Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Average Price (In Base)"] = dr["item_rate_bs"].ToString();
                                
                            }                            
                            dtrowLines["Gross Value (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Gross Value (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Order Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Order Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "ODProductWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Product Name", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));                    
                    dt.Columns.Add("Quantity", typeof(decimal));                    
                    dt.Columns.Add("Average Price (In Specific)", typeof(decimal));
                    dt.Columns.Add("Average Price (In Base)", typeof(decimal));                    
                    dt.Columns.Add("Gross Value (In Specific)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Product Name"] = dr["item_name"].ToString();
                            dtrowLines["UOM"] = dr["uom_alias"].ToString();
                            dtrowLines["Quantity"] = dr["item_qty"].ToString();
                            dtrowLines["Average Price (In Specific)"] = dr["item_rate_spec"].ToString();
                            dtrowLines["Average Price (In Base)"] = dr["item_rate_bs"].ToString();
                            dtrowLines["Gross Value (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Gross Value (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Order Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Order Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "RegionWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Region Name", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Order Type", typeof(string));
                    if(Flag == "RegionWiseDetail")
                    {
                        dt.Columns.Add("Customer Name", typeof(string));
                        dt.Columns.Add("Sales Representative", typeof(string));
                        dt.Columns.Add("Order Number", typeof(string));
                        dt.Columns.Add("Order Date", typeof(string));
                        dt.Columns.Add("Item Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Order Quantity", typeof(decimal));
                        dt.Columns.Add("Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Price (In Base)", typeof(decimal));
                    }
                    dt.Columns.Add("Gross Value (In Specific)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Region Name"] = dr["cust_region"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Order Type"] = dr["sale_type"].ToString();
                            if (Flag == "RegionWiseDetail")
                            {
                                dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                                dtrowLines["Sales Representative"] = dr["sale_per"].ToString();
                                dtrowLines["Order Number"] = dr["app_so_no"].ToString();
                                dtrowLines["Order Date"] = dr["so_dt1"].ToString();
                                dtrowLines["Item Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["Order Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }
                            dtrowLines["Gross Value (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Gross Value (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Order Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Order Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "SalePersonWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Sales Executive Name", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Order Type", typeof(string));
                    if (Flag == "SalePersonWiseDetail")
                    {
                        dt.Columns.Add("Customer Name", typeof(string));
                        dt.Columns.Add("Region Name", typeof(string));
                        dt.Columns.Add("Order Number", typeof(string));
                        dt.Columns.Add("Order Date", typeof(string));
                        dt.Columns.Add("Item Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Order Quantity", typeof(decimal));
                        dt.Columns.Add("Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Price (In Base)", typeof(decimal));
                    }
                    dt.Columns.Add("Gross Value (In Specific)", typeof(decimal));
                    dt.Columns.Add("Gross Value (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Order Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Sales Executive Name"] = dr["sale_per"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Order Type"] = dr["sale_type"].ToString();
                            if (Flag == "SalePersonWiseDetail")
                            {
                                dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                                dtrowLines["Region Name"] = dr["cust_region"].ToString();
                                dtrowLines["Order Number"] = dr["app_so_no"].ToString();
                                dtrowLines["Order Date"] = dr["so_dt1"].ToString();
                                dtrowLines["Item Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["Order Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }
                            dtrowLines["Gross Value (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Gross Value (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Order Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Order Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "PendingOrdersWise")
                {
                    if(Flag == "PendingOrders")
                    {
                        dt.Columns.Add("Sr.No", typeof(string));
                    }   
                    dt.Columns.Add("Order Number", typeof(string));
                    dt.Columns.Add("Order Date", typeof(string));
                    if (Flag == "PendingOrders")
                    {
                        dt.Columns.Add("Order Type", typeof(string));
                    }                      
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Customer Name", typeof(string));
                    if (Flag == "PendingOrdDtl")
                    {
                        dt.Columns.Add("Sr.No", typeof(string));
                        dt.Columns.Add("Item Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Order Quantity", typeof(decimal));
                        dt.Columns.Add("Order Price", typeof(decimal));
                        dt.Columns.Add("Order Value", typeof(decimal));
                        dt.Columns.Add("Shipped Quantity", typeof(decimal));
                        dt.Columns.Add("Shipped Value", typeof(decimal));
                        dt.Columns.Add("Pending Quantity", typeof(decimal));
                        dt.Columns.Add("Pending Value", typeof(decimal));
                    }
                    else
                    {
                        dt.Columns.Add("Order Quantity", typeof(decimal));
                        dt.Columns.Add("Order Value", typeof(decimal));
                        dt.Columns.Add("Shipped Quantity", typeof(decimal));
                        dt.Columns.Add("Pending Quantity", typeof(decimal));
                        dt.Columns.Add("Pending Value", typeof(decimal));
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            if (Flag == "PendingOrders")
                            {
                                dtrowLines["Sr.No"] = rowno + 1;
                            }
                                
                            dtrowLines["Order Number"] = dr["app_so_no"].ToString();
                            dtrowLines["Order Date"] = dr["so_dt1"].ToString();
                            if (Flag == "PendingOrders")
                            {
                                dtrowLines["Order Type"] = dr["sale_type"].ToString();
                                dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            }
                            if (Flag == "PendingOrdDtl")
                            {
                                dtrowLines["Currency"] = dr["curr_name"].ToString();
                            }
                                dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                            if (Flag == "PendingOrdDtl")
                            {
                                dtrowLines["Sr.No"] = dr["srNo"].ToString();
                                dtrowLines["Item Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_name"].ToString();
                                dtrowLines["Order Quantity"] = dr["OrderQuantity"].ToString();
                                dtrowLines["Order Price"] = dr["OrderPrice"].ToString();
                                dtrowLines["Order Value"] = dr["OrderValue"].ToString();
                                dtrowLines["Shipped Quantity"] = dr["Ship_qty"].ToString();
                                dtrowLines["Shipped Value"] = dr["ShipValue"].ToString();
                                dtrowLines["Pending Quantity"] = dr["PendingQty"].ToString();
                                dtrowLines["Pending Value"] = dr["PendingVal"].ToString();
                            }
                            else
                            {
                                dtrowLines["Order Quantity"] = dr["ord_qty_base"].ToString();
                                dtrowLines["Order Value"] = dr["OrderValue"].ToString();
                                dtrowLines["Shipped Quantity"] = dr["ship_qty"].ToString();
                                dtrowLines["Pending Quantity"] = dr["PendingQty"].ToString();
                                dtrowLines["Pending Value"] = dr["PendingValue"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "PendingOrdersItemWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Item Name", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("Order Quantity", typeof(decimal));
                    dt.Columns.Add("Order Price", typeof(decimal));                    
                    dt.Columns.Add("Order Value", typeof(decimal));
                    dt.Columns.Add("Shipped Quantity", typeof(decimal));
                    dt.Columns.Add("Pending Quantity", typeof(decimal));
                    dt.Columns.Add("Pending Value", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Item Name"] = dr["item_name"].ToString();
                            dtrowLines["UOM"] = dr["uom_alias"].ToString();
                            dtrowLines["Order Quantity"] = dr["ord_qty_base"].ToString();
                            dtrowLines["Order Price"] = dr["OrderPrice"].ToString();
                            dtrowLines["Order Value"] = dr["OrderValue"].ToString();
                            dtrowLines["Shipped Quantity"] = dr["ship_qty"].ToString();
                            dtrowLines["Pending Quantity"] = dr["PendingQty"].ToString();
                            dtrowLines["Pending Value"] = dr["PendingVal"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("OrderDetail", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private List<CustZoneList> CustZoneLists()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            List<CustZoneList> custzoneList = new List<CustZoneList>();
            //CommonController comn = new CommonController(_Common_IServices);
            DataSet ds = _Common_IServices.GetCustCommonDropdownDAL(CompID, "0", "0");
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
            //CommonController comn = new CommonController(_Common_IServices);
            DataSet ds = _Common_IServices.GetCustCommonDropdownDAL(CompID, "0", "0");
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                CustGroupList custgroup = new CustGroupList();
                custgroup.CustGrp_id = dr["setup_id"].ToString();
                custgroup.CustGrp_val = dr["setup_val"].ToString();
                custgroupList.Add(custgroup);
            }
            return custgroupList;
        }
        public ActionResult BindStateListData(OrderDetail_Model _OrderDetail_Model)
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
                        BrID = Session["BranchId"].ToString();
                    }
                    if (_OrderDetail_Model.SearchState == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _OrderDetail_Model.SearchState;
                    }
                    DataSet ProductList = _Common_IServices.GetCustCommonDropdownDAL(CompID, SarchValue, "0");
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
        public ActionResult BindCityListdata(OrderDetail_Model _OrderDetail_Model)
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
                        BrID = Session["BranchId"].ToString();
                    }
                    if (_OrderDetail_Model.SearchCity == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _OrderDetail_Model.SearchCity;
                    }
                    var state_id = _OrderDetail_Model.state_id;
                    DataSet ProductList = _Common_IServices.GetCustCommonDropdownDAL(CompID, SarchValue, state_id);
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

