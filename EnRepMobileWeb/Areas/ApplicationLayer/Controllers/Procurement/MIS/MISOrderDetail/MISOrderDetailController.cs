using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.MIS.MISOrderDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.GoodsReceiptNote;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.MISOrderDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.OrderDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.MIS.MISOrderDetail
{
    public class MISOrderDetailController : Controller
    {
        string DocumentMenuId = "105101155105", title;
        string CompID, BrId, language = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly MISOrderDetail_IService _MisOrderDetailsRepo;
        private readonly ItemList_ISERVICES _itemSetup;
        private readonly GoodsReceiptNote_ISERVICE _GoodsReceiptNote_ISERVICE;
        private readonly OrderDetail_IService _OrderISERVICES;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public MISOrderDetailController(Common_IServices _Common_IServices, MISOrderDetail_IService MisOrderDetailsRepo,
            ItemList_ISERVICES itemSetup, GoodsReceiptNote_ISERVICE GoodsReceiptNote_ISERVICE, OrderDetail_IService OrderISERVICES, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            _MisOrderDetailsRepo = MisOrderDetailsRepo;
            _itemSetup = itemSetup;
            _GoodsReceiptNote_ISERVICE = GoodsReceiptNote_ISERVICE;
            _OrderISERVICES = OrderISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/MISOrderDetail
        public ActionResult MISOrderDetail()
        {
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
            MISOrderDetail_Model objModel = new MISOrderDetail_Model();
            DateTime dtnow = DateTime.Now;
            // string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
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
            // string FromDate = new DateTime(dtnow.Year, 4, 1).ToString("yyyy-MM-dd"); /*Commented Month  By Nitesh 05-12-2023 add Fincacial Year in From Date*/
            string FromDate = objModel.FromDate;
            string ToDate = dtnow.ToString("yyyy-MM-dd");
            objModel.Title = title;
            //  objModel.FromDate = FromDate;
            objModel.ToDate = ToDate;
            //  objModel.ItemsList = GetItemsList();
            //objModel.SuppliersList = GetSuppliersList();/*Commented by Hina on 04-12-2024*/
            GetAllDropDownData(objModel);/*Add by Hina on 04-12-2024*/
            objModel.StatusList = GetStatusList();
            //  objModel.ItemsList = GetItemsList();
            List<ItemsModel> _ItemList1 = new List<ItemsModel>();
            ItemsModel _ItemList = new ItemsModel();
            _ItemList.Item_ID = "0";
            _ItemList.Item_Name = "All";
            _ItemList1.Add(_ItemList);
            objModel.ItemsList = _ItemList1;
            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(CompID, userid);
            ViewBag.br_list = br_list;
            objModel.Currencylist = GetCurrencyList();
            objModel.categoryLists = SuppCategoryList();
            objModel.portFolioLists = SuppPortFolioLists();
            GetMisOrderSummary(FromDate, ToDate, "", "", "", "", "", "", "S","0","0",BrId);

            return View("~/Areas/ApplicationLayer/Views/Procurement/MIS/MISOrderDetail/MISOrderDetail.cshtml", objModel);
        }
        private void GetAllDropDownData(MISOrderDetail_Model _Model)/*Created by Hina sharma on 05-12-2024 */
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
            _SuppList.Insert(0, new SupplierModel() { SuppId = "0", SuppName = "All" });
            _Model.SuppliersList = _SuppList;
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

        //private List<ItemsModel> GetItemsList()
        //{
        //    try
        //    {
        //        string compId = string.Empty;
        //        if (Session["CompId"] != null)
        //            compId = Session["CompId"].ToString();
        //        if (Session["BranchId"] != null)
        //            BrId = Session["BranchId"].ToString();

        //        DataTable dt = _itemSetup.BindGetItemList("", compId, BrId);
        //        List<ItemsModel> itemsList = new List<ItemsModel>();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            ItemsModel itmLst = new ItemsModel
        //            {
        //                Item_ID = dr["item_id"].ToString(),
        //                Item_Name = dr["item_name"].ToString()
        //            };
        //            itemsList.Add(itmLst);
        //        }
        //        itemsList.Insert(0, new ItemsModel { Item_ID = "0", Item_Name = "---All---" });
        //        return itemsList;
        //    }
        //    catch (Exception exc)
        //    {
        //        throw exc;
        //    }
        //}
        private List<SupplierModel> GetSuppliersList()
        {
            string compId = string.Empty;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            SuppList = _GoodsReceiptNote_ISERVICE.GetSupplierList(compId, "0", BrId);

            List<SupplierModel> _SuppList = new List<SupplierModel>();
            foreach (var data in SuppList)
            {
                SupplierModel _SuppDetail = new SupplierModel();
                _SuppDetail.SuppId = data.Key;
                _SuppDetail.SuppName = data.Value;
                _SuppList.Add(_SuppDetail);
            }
            return _SuppList;
        }
        private List<StatusModel> GetStatusList()
        {
            List<StatusModel> StatusLst = new List<StatusModel>();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrId, "0", "105101130", language);
            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables.Contains("Table4"))
            {
                foreach (DataRow dr in ds.Tables[4].Rows)
                {
                    StatusModel status = new StatusModel()
                    {
                        StatusCode = dr["status_code"].ToString(),
                        StatusName = dr["status_name"].ToString()
                    };
                    if (dr["status_code"].ToString() != "C" && dr["status_code"].ToString() != "D" && dr["status_code"].ToString() != "F" && dr["status_code"].ToString() != "0")
                        StatusLst.Add(status);
                }
                //StatusLst.Insert(0, new StatusModel { StatusCode = "0", StatusName = "---All---" });
                return StatusLst;
            }
            else
            {
                return null;
            }
        }
        public DataTable GetMisPurchaseOrderData(string fromDate, string toDate, string showAs, string suppId,
            string itemId, string currId, string srctype, string orderType, string Status, string poNo, string poDate, string SuppCat, string SuppPort,string brid_list)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            ds = _MisOrderDetailsRepo.GetMISOrderDetail(CompID, BrId, fromDate, toDate, showAs, suppId, itemId, currId, srctype, orderType, Status, poNo, poDate, SuppCat, SuppPort, brid_list);
            if (showAs == "S")
            {
                ViewBag.TotalSummeryOrderWise = ds.Tables[1];
            }
            if (showAs == "D")
            {
                ViewBag.TotalDetailOrderWise = ds.Tables[1];
            }
            if (showAs == "SD")
            {
                ViewBag.TotalDetailOrderItemWise = ds.Tables[1];
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                return dt;
            }
            else
            {
                return null;
            }
        }
        public   ActionResult GetMisOrderSummary(string fromDate, string toDate, string suppId,
             string itemId, string currId, string srctype, string orderType, string Status, string ShowAs,string custCat,string custPort,string brid_list)
        {
            MISOrderDetail_Model objModel = new MISOrderDetail_Model();
            objModel.SearchStatus = "SEARCH";
            ViewBag.flag = ShowAs;
            ViewBag.OrderSummary = GetMisPurchaseOrderData(fromDate, toDate, ShowAs, suppId, itemId, currId, srctype, orderType, Status, "", "", custCat, custPort, brid_list);
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISOrderSummaryDetail.cshtml", objModel);
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
                //currencyList.Insert(0, new CurrencyList { curr_id = "0", curr_name = "---All---" });
                return currencyList;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public ActionResult GetMisOrderdeliveryShudule(string orderNo, string item_id, string OrderType)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrId = Session["BranchId"].ToString();
            MISOrderDetail_Model objModel = new MISOrderDetail_Model();
            //objModel.SearchStatus = "SEARCH";
            ViewBag.ItemDelSchdetails = _Common_IServices.Cmn_GetDeliverySchudule(CompID, BrId, OrderType, orderNo, item_id, DocumentMenuId);
            return PartialView("~/Areas/Common/Views/Comn_PartialMISOrderDeliverySchedule.cshtml", objModel);
        }

        public ActionResult GetMisOrderSummaryDetails(string poNo, string poDate,string SuppCat, string SuppPort,string brid_list)
        {
            MISOrderDetail_Model objModel = new MISOrderDetail_Model();
            objModel.SearchStatus = "SEARCH";
            ViewBag.OrderSummaryDetails = GetMisPurchaseOrderData("", "", "SD", "", "", "", "", "", "", poNo, poDate, SuppCat, SuppPort, brid_list);
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISOrderSummary.cshtml", objModel);
        }
        public ActionResult GetMisOrderDetails(string fromDate, string toDate, string suppId,
             string itemId, string currId, string srctype, string orderType, string Status)
        {
            MISOrderDetail_Model objModel = new MISOrderDetail_Model();
            objModel.SearchStatus = "SEARCH";
            ViewBag.OrderDetails = GetMisPurchaseOrderData(fromDate, toDate, "D", suppId, itemId, currId, srctype, orderType, Status, "", "","0","0","");
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISOrderDetail.cshtml", objModel);
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
        //public FileResult OrderDetailExporttoExcelDt(string ShowAs, string fromDate, string toDate, string suppId, string itemId, string currId, string srctype, string orderType, string Status, string poNo, string poDate)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        DataSet ds = new DataSet();
        //        if (Session["CompId"] != null)
        //            CompID = Session["CompId"].ToString();
        //        if (Session["BranchId"] != null)
        //            BrId = Session["BranchId"].ToString();
        //        ds = _MisOrderDetailsRepo.GetMISOrderDetail(CompID, BrId, fromDate, toDate, ShowAs, suppId, itemId, currId, srctype, orderType, Status, poNo, poDate);
        //        if (ShowAs == "S" || ShowAs == "D" || ShowAs == "SD")
        //        {
        //            dt.Columns.Add("Sr.No", typeof(string));
        //            if (ShowAs == "D" || ShowAs == "SD")
        //            {
        //                dt.Columns.Add("Item Name", typeof(string));//Item Name
        //                dt.Columns.Add("UOM", typeof(string));//Item Name
        //                if (ShowAs == "D")
        //                {
        //                    dt.Columns.Add("Supplier Name", typeof(string));//Item Name
        //                    dt.Columns.Add("Order Number", typeof(string));
        //                    dt.Columns.Add("Order Date", typeof(string));
        //                    dt.Columns.Add("Order Type", typeof(string));
        //                    dt.Columns.Add("Currency", typeof(string));
        //                    dt.Columns.Add("Source Type", typeof(string));
        //                    dt.Columns.Add("Source Document", typeof(string));
        //                }
        //                dt.Columns.Add("Order Quantity", typeof(decimal));
        //                dt.Columns.Add("Price", typeof(decimal));
        //                dt.Columns.Add("Discount Per Unit", typeof(decimal));
        //                dt.Columns.Add("Discount Value", typeof(decimal));
        //                dt.Columns.Add("Gross Value", typeof(decimal));
        //                dt.Columns.Add("Tax Amount", typeof(decimal));
        //                dt.Columns.Add("Other Charges", typeof(decimal));
        //                dt.Columns.Add("Net Value (In Specific)", typeof(decimal));
        //                dt.Columns.Add("Net Value (In Base)", typeof(decimal));
        //                if (ShowAs == "D")
        //                {
        //                    dt.Columns.Add("Status", typeof(string));
        //                    dt.Columns.Add("Force Closed", typeof(string));
        //                }
        //            }
        //            else
        //            {
        //                dt.Columns.Add("Order Number", typeof(string));
        //                dt.Columns.Add("Order Date", typeof(string));
        //                dt.Columns.Add("Order Type", typeof(string));
        //                dt.Columns.Add("Source Type", typeof(string));
        //                dt.Columns.Add("Source Document", typeof(string));
        //                dt.Columns.Add("Supplier Name", typeof(string));
        //                dt.Columns.Add("Currency", typeof(string));
        //                dt.Columns.Add("Order Value", typeof(decimal));
        //                dt.Columns.Add("Status", typeof(string));
        //                dt.Columns.Add("Created By", typeof(string));
        //                dt.Columns.Add("Created On", typeof(string));
        //                dt.Columns.Add("Approved By", typeof(string));
        //                dt.Columns.Add("Approved On", typeof(string));
        //                dt.Columns.Add("Amended By", typeof(string));
        //                dt.Columns.Add("Amended On", typeof(string));
        //            }

        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                int rowno = 0;
        //                foreach (DataRow dr in ds.Tables[0].Rows)
        //                {
        //                    DataRow dtrowLines = dt.NewRow();
        //                    dtrowLines["Sr.No"] = rowno + 1;
        //                    if (ShowAs == "D" || ShowAs == "SD")
        //                    {
        //                        dtrowLines["Item Name"] = dr["item_name"].ToString();
        //                        dtrowLines["UOM"] = dr["uom_name"].ToString();
        //                        if (ShowAs == "D")
        //                        {
        //                            dtrowLines["Supplier Name"] = dr["supp_name"].ToString();
        //                            dtrowLines["Order Number"] = dr["po_no"].ToString();
        //                            dtrowLines["Order Date"] = dr["po_dt1"].ToString();
        //                            dtrowLines["Order Type"] = dr["order_type"].ToString();
        //                            dtrowLines["Currency"] = dr["curr_logo"].ToString();
        //                            dtrowLines["Source Type"] = dr["src_type"].ToString();
        //                            dtrowLines["Source Document"] = dr["src_doc_number"].ToString();
        //                        }
        //                        dtrowLines["Order Quantity"] = dr["ord_qty_base"].ToString();
        //                        dtrowLines["Price"] = dr["item_rate"].ToString();
        //                        dtrowLines["Discount Per Unit"] = dr["DiscPerUnit"].ToString();
        //                        dtrowLines["Discount Value"] = dr["item_disc_val"].ToString();
        //                        dtrowLines["Gross Value"] = dr["item_gr_val"].ToString();
        //                        dtrowLines["Tax Amount"] = dr["item_tax_amt"].ToString();
        //                        dtrowLines["Other Charges"] = dr["item_oc_amt"].ToString();
        //                        dtrowLines["Net Value (In Specific)"] = dr["item_net_val_spec"].ToString();
        //                        dtrowLines["Net Value (In Base)"] = dr["item_net_val_bs"].ToString();
        //                        if (ShowAs == "D")
        //                        {
        //                            dtrowLines["Status"] = dr["status_name"].ToString();
        //                            dtrowLines["Force Closed"] = dr["force_close"].ToString();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dtrowLines["Order Number"] = dr["po_no"].ToString();
        //                        dtrowLines["Order Date"] = dr["po_dt1"].ToString();
        //                        dtrowLines["Order Type"] = dr["order_type"].ToString();
        //                        dtrowLines["Source Type"] = dr["src_type"].ToString();
        //                        dtrowLines["Source Document"] = dr["src_doc_number"].ToString();
        //                        dtrowLines["Supplier Name"] = dr["supp_name"].ToString();
        //                        dtrowLines["Currency"] = dr["curr_logo"].ToString();
        //                        dtrowLines["Order Value"] = dr["net_val_bs"].ToString();
        //                        dtrowLines["Status"] = dr["status_name"].ToString();
        //                        dtrowLines["Created By"] = dr["CreatedBy"].ToString();
        //                        dtrowLines["Created On"] = dr["create_dt"].ToString();
        //                        dtrowLines["Approved By"] = dr["ApprovedBy"].ToString();
        //                        dtrowLines["Approved On"] = dr["app_dt"].ToString();
        //                        dtrowLines["Amended By"] = dr["AmendedBy"].ToString();
        //                        dtrowLines["Amended On"] = dr["mod_dt"].ToString();
        //                    }
        //                    dt.Rows.Add(dtrowLines);
        //                    rowno = rowno + 1;
        //                }
        //            }
        //        }
        //        var commonController = new CommonController(_Common_IServices);
        //        return commonController.ExportDatatableToExcel("OrderDetail", dt);
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}
        private List<SuppCategoryList> SuppCategoryList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            DataSet dt = _MisOrderDetailsRepo.GetcategoryPortfolioDAL(CompID);

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
                CompID = Session["CompId"].ToString();
            DataSet dt1 = _MisOrderDetailsRepo.GetcategoryPortfolioDAL(CompID);

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