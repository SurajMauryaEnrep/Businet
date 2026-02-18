using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockDetailShopfloor;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetailShopfloor;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.MiscellaneousSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.StockDetailShopfloor
{
    public class StockDetailShopfloorController : Controller
    {
        //DataTable ItemListDs;
        string DocumentMenuId = "105102180103", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        StockDetailShopfloor_ISERVICE _StockDetailShopfloor_ISERVICE;
        Common_IServices _Common_IServices;
        private readonly HSNCode_ISERVICES _HSNNCode_Iservices;
        public StockDetailShopfloorController(StockDetailShopfloor_ISERVICE _StockDetailShopfloor_ISERVICE, Common_IServices _Common_IServices, HSNCode_ISERVICES HSNNCode_Iservices)
        {
            this._StockDetailShopfloor_ISERVICE = _StockDetailShopfloor_ISERVICE;
            this._Common_IServices = _Common_IServices;
            this._HSNNCode_Iservices = HSNNCode_Iservices;
        }
        // GET: ApplicationLayer/StockDetailShopfloor
        public ActionResult StockDetailShopfloor()
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string Language = string.Empty;
                StockDetailShopfloor_Model _StockDetailShopfloor_Model = new StockDetailShopfloor_Model();
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                GetItemList(_StockDetailShopfloor_Model);
                GetItemGrpList(_StockDetailShopfloor_Model);
                GetItemPortfList(_StockDetailShopfloor_Model);
                GetShopfloorList(_StockDetailShopfloor_Model);
                GetBrachList(_StockDetailShopfloor_Model);
                _StockDetailShopfloor_Model.UptoDate = DateTime.Now.ToString("yyyy-MM-dd"); //last_Date.ToString("yyyy-MM-dd");
                _StockDetailShopfloor_Model.BranchName = Br_ID;
                _StockDetailShopfloor_Model.StockDetailShopfloorList = SearchStockDetailShopfloor_load(_StockDetailShopfloor_Model);
                _StockDetailShopfloor_Model.HsnCodeList = GetHsnCodeList(Comp_ID);
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _StockDetailShopfloor_Model.Title = title;
                _StockDetailShopfloor_Model.StockBy = "Branchwisestock";
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/StockDetailShopfloor/StockDetailShopfloorDetail.cshtml", _StockDetailShopfloor_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void CommonPageDetails()
        {
            try
            {
                string CompID = string.Empty;
                string BrchID = string.Empty;
                string UserID = string.Empty;
                string Language = string.Empty;
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
                    Language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, Language);
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
        public ActionResult showReportFromDashBoard(string ToDt)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string Language = string.Empty;
                StockDetailShopfloor_Model _StockDetailShopfloor_Model = new StockDetailShopfloor_Model();
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                GetItemList(_StockDetailShopfloor_Model);
                GetItemGrpList(_StockDetailShopfloor_Model);
                GetItemPortfList(_StockDetailShopfloor_Model);
                GetShopfloorList(_StockDetailShopfloor_Model);
                GetBrachList(_StockDetailShopfloor_Model);
                _StockDetailShopfloor_Model.UptoDate = ToDt; //last_Date.ToString("yyyy-MM-dd");
                _StockDetailShopfloor_Model.BranchName = Br_ID;
                _StockDetailShopfloor_Model.StockDetailShopfloorList = SearchStockDetailShopfloor_load(_StockDetailShopfloor_Model);
                _StockDetailShopfloor_Model.HsnCodeList = GetHsnCodeList(Comp_ID);
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _StockDetailShopfloor_Model.Title = title;
                _StockDetailShopfloor_Model.StockBy = "Branchwisestock";
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/StockDetailShopfloor/StockDetailShopfloorDetail.cshtml", _StockDetailShopfloor_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private string getDocumentName()
        {
            try
            {
                string CompID = string.Empty;
                string language = string.Empty;
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
        private List<HsnCodeModel> GetHsnCodeList(string compId)
        {
            List<HsnCodeModel> hsnList = new List<HsnCodeModel>();
            //hsnList.Add(new HsnCodeModel { hsn_code = "0", hsn_desc = "---Select---" });
            DataSet ds = _HSNNCode_Iservices.Get_HsnDetails(compId);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dtr in ds.Tables[0].Rows)
                {
                    hsnList.Add(new HsnCodeModel { hsn_code = dtr["hsn_no"].ToString(), hsn_desc = dtr["hsn_no"].ToString() });
                }
            }
            return hsnList;
        }
        public ActionResult GetItemList(StockDetailShopfloor_Model queryParameters)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.ItemName))
                {
                    ItemName = "0";
                }
                else
                {
                    ItemName = queryParameters.ItemName;
                }
                ItemList = _StockDetailShopfloor_ISERVICE.ItemList(ItemName, Comp_ID);

                List<ItemName> _ItemList = new List<ItemName>();
                //foreach (var data in ItemList)
                //{
                //    ItemName _ItemDetail = new ItemName();
                //    _ItemDetail.Item_Id = data.Key;
                //    _ItemDetail.Item_Name = data.Value;
                    _ItemList.Add(new ItemName { Item_Id = "0", Item_Name = "---Select---" });
                //}
                queryParameters.ItemNameList = _ItemList;
                

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemGrpList(StockDetailShopfloor_Model queryParameters)
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
                ItemGroupList = _StockDetailShopfloor_ISERVICE.ItemGroupList(GroupName, Comp_ID);

                List<ItemGroupName> _ItemGroupList = new List<ItemGroupName>();
                //foreach (var data in ItemGroupList)
                //{
                //    ItemGroupName _ItemGroupDetail = new ItemGroupName();
                //    _ItemGroupDetail.Group_Id = data.Key;
                //    _ItemGroupDetail.Group_Name = data.Value;
                    _ItemGroupList.Add(new ItemGroupName { Group_Id="0",Group_Name="---Select---"});
                //}
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
        public ActionResult GetItemPortfList(StockDetailShopfloor_Model queryParameters)
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
                ItemPortfolioList = _StockDetailShopfloor_ISERVICE.ItemPortfolioList(PortfolioName, Comp_ID);

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
        public void GetShopfloorList(StockDetailShopfloor_Model queryParameters)
        {
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
                DataSet Wh_List = _StockDetailShopfloor_ISERVICE.GetShopfloorList(Comp_ID, Br_ID);

                DataRow Drow = Wh_List.Tables[0].NewRow();
              //  Drow[1] = "---All---";
               // Drow[0] = "0";
               // Wh_List.Tables[0].Rows.InsertAt(Drow, 0);

                List<Shopfloor> _Shfllist = new List<Shopfloor>();

                foreach (DataRow dr in Wh_List.Tables[0].Rows)
                {
                    Shopfloor _ShflDetail = new Shopfloor();
                    _ShflDetail.shfl_Id = dr["shfl_id"].ToString();
                    _ShflDetail.shfl_Name = dr["shfl_name"].ToString();
                    _Shfllist.Add(_ShflDetail);
                }
                queryParameters.ShopfloorList = _Shfllist;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        public void GetBrachList(StockDetailShopfloor_Model queryParameters)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string User_id = string.Empty;
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
                    User_id = Session["UserId"].ToString();
                }
                DataSet BranchList = _StockDetailShopfloor_ISERVICE.GetAllBrchList(Comp_ID, User_id);

                List<Branch> _Branchlist = new List<Branch>();

                foreach (DataRow dr in BranchList.Tables[0].Rows)
                {
                    Branch _BranchDetail = new Branch();
                    _BranchDetail.Br_Id = dr["Comp_Id"].ToString();
                    _BranchDetail.Br_Name = dr["comp_nm"].ToString();
                    _Branchlist.Add(_BranchDetail);
                }
                queryParameters.BranchList = _Branchlist;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        [HttpPost]
        public ActionResult SearchStockDetailShopfloor(string IncludeZeroStockFlag, string StockBy, string ItemId, string GroupId, string BrachID, string ShflID, string UpToDate, string PortfolioId, string hsnCode)
        {
            #region Code Modified by Suraj Maurya on 03-03-2025 under ticket id 1536 for performance issue
            #endregion
            try
            {
                List<StockListDetailShopfloor> _StockListDetail = new List<StockListDetailShopfloor>();
                StockDetailShopfloor_Model _StockDetailShopfloor_Model = new StockDetailShopfloor_Model();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
                DataTable dt = new DataTable();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                //dt = _StockDetailShopfloor_ISERVICE.GetStockDetailShopfloorList(CompID, BrachID, IncludeZeroStockFlag, StockBy
                //    , ItemId, GroupId, ShflID, UpToDate, PortfolioId, hsnCode,0,0,null,null,null).Tables[0];
                _StockDetailShopfloor_Model.StockBy = StockBy;
                _StockDetailShopfloor_Model.StockByFilter = StockBy;
                //if (dt.Rows.Count > 0)
                //{

                //    //Session["StockBy"] = StockBy;
                //    //Session["StockByFilter"] = StockBy;
                   

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        StockListDetailShopfloor _StockDetailShopfloorList = new StockListDetailShopfloor();
                //        _StockDetailShopfloorList.ItemName = dr["itemname"].ToString();
                //        _StockDetailShopfloorList.HsnCode = dr["HsnCode"].ToString();
                //        _StockDetailShopfloorList.subItem = dr["sub_item"].ToString();
                //        _StockDetailShopfloorList.ItemID = dr["itemid"].ToString();
                //        _StockDetailShopfloorList.UOM = dr["uomname"].ToString();
                //        _StockDetailShopfloorList.Branch = dr["brname"].ToString();
                //        _StockDetailShopfloorList.BranchID = dr["brid"].ToString();
                //        _StockDetailShopfloorList.Shopfloor = dr["shflname"].ToString();
                //        _StockDetailShopfloorList.ShopfloorID = dr["shflid"].ToString();
                //        _StockDetailShopfloorList.Lot = dr["lotno"].ToString();
                //        _StockDetailShopfloorList.Batch = dr["batchno"].ToString();
                //        _StockDetailShopfloorList.Serial = dr["serialno"].ToString();
                //        _StockDetailShopfloorList.OpeningStock = dr["opening"].ToString();
                //        _StockDetailShopfloorList.Receipts = dr["receipts"].ToString();
                //        _StockDetailShopfloorList.Issued = dr["issued"].ToString();
                //        //_StockDetailShopfloorList.ReservedStock = dr["reserved"].ToString();
                //        _StockDetailShopfloorList.RejectedStock = dr["rejected"].ToString();
                //        _StockDetailShopfloorList.ReworkableStock = dr["reworkabled"].ToString();
                //        _StockDetailShopfloorList.WIPStock = dr["WIPStock"].ToString();
                //        _StockDetailShopfloorList.TotalStock = dr["totalstk"].ToString();
                //        _StockDetailShopfloorList.TotalStockVal = dr["totalstkval"].ToString();
                //        _StockDetailShopfloorList.AvailableStock = dr["avlstk"].ToString();
                //        _StockDetailShopfloorList.StockValue = dr["avlstkvalue"].ToString();
                //        _StockDetailShopfloorList.sub_item_id = dr["sub_item_id"].ToString();
                //        _StockDetailShopfloorList.sub_item_name = dr["sub_item_name"].ToString();
                //        _StockDetailShopfloorList.ReservedStock = dr["reserved"].ToString();

                //        _StockListDetail.Add(_StockDetailShopfloorList);
                //    }
                //}

                //_StockDetailShopfloor_Model.StockDetailShopfloorList = _StockListDetail;
                CommonPageDetails();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialStockDetailShopfloorList.cshtml", _StockDetailShopfloor_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private List<StockListDetailShopfloor> SearchStockDetailShopfloor_load(StockDetailShopfloor_Model _StockDetailShopfloor_Model)
        {
            try
            {
                List<StockListDetailShopfloor> _StockListDetail = new List<StockListDetailShopfloor>();
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                DataTable dt = new DataTable();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                dt = _StockDetailShopfloor_ISERVICE.GetStockDetailShopfloorList(CompID, Br_ID, "N", "Branchwisestock", "0", "0", "0"
                    , _StockDetailShopfloor_Model.UptoDate, "0","0",0,25,null,null,null, "").Tables[0];
                //Session["StockBy"] = "Branchwisestock";
                _StockDetailShopfloor_Model.StockBy = "Branchwisestock";
                //Session["StockByFilter"] = "Branchwisestocktbl";
                _StockDetailShopfloor_Model.StockByFilter = "Branchwisestocktbl";
                if (dt.Rows.Count > 0)
                {
                    ////Session["StockBy"] = "Branchwisestock";
                    //_StockDetailShopfloor_Model.StockBy = "Branchwisestock";
                    ////Session["StockByFilter"] = "Branchwisestocktbl";
                    //_StockDetailShopfloor_Model.StockByFilter = "Branchwisestocktbl";
                    foreach (DataRow dr in dt.Rows)
                    {
                        StockListDetailShopfloor _StockDetailShopfloorList = new StockListDetailShopfloor();
                        _StockDetailShopfloorList.ItemName = dr["itemname"].ToString();
                        _StockDetailShopfloorList.HsnCode = dr["HsnCode"].ToString();
                        _StockDetailShopfloorList.subItem = dr["sub_item"].ToString();
                        _StockDetailShopfloorList.ItemID = dr["itemid"].ToString();
                        _StockDetailShopfloorList.UOM = dr["uomname"].ToString();
                        _StockDetailShopfloorList.Branch = dr["brname"].ToString();
                        _StockDetailShopfloorList.BranchID = dr["brid"].ToString();
                        _StockDetailShopfloorList.Shopfloor = dr["shflname"].ToString();
                        _StockDetailShopfloorList.ShopfloorID = dr["shflid"].ToString();
                        _StockDetailShopfloorList.Lot = dr["lotno"].ToString();
                        _StockDetailShopfloorList.Batch = dr["batchno"].ToString();
                        _StockDetailShopfloorList.Serial = dr["serialno"].ToString();
                        _StockDetailShopfloorList.OpeningStock = dr["opening"].ToString();
                        _StockDetailShopfloorList.Receipts = dr["receipts"].ToString();
                        _StockDetailShopfloorList.Issued = dr["issued"].ToString();
                        //_StockDetailShopfloorList.ReservedStock = dr["reserved"].ToString();
                        _StockDetailShopfloorList.RejectedStock = dr["rejected"].ToString();
                        _StockDetailShopfloorList.ReworkableStock = dr["reworkabled"].ToString();
                        _StockDetailShopfloorList.WIPStock = dr["WIPStock"].ToString();
                        _StockDetailShopfloorList.TotalStock = dr["totalstk"].ToString();
                        _StockDetailShopfloorList.TotalStockVal = dr["totalstkval"].ToString();
                        _StockDetailShopfloorList.AvailableStock = dr["avlstk"].ToString();
                        _StockDetailShopfloorList.StockValue = dr["avlstkvalue"].ToString();
                        _StockDetailShopfloorList.sub_item_id = dr["sub_item_id"].ToString();
                        _StockDetailShopfloorList.sub_item_name = dr["sub_item_name"].ToString();
                        _StockDetailShopfloorList.ReservedStock = dr["reserved"].ToString();

                        _StockListDetail.Add(_StockDetailShopfloorList);
                    }
                }
                return _StockListDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetItemReceivedDetailList(string StockBy, string ItemID, string Branch, string TransType, string ShflID, string LotNo, string BatchNo, string SerialNo, string UpToDate)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _StockDetailShopfloor_ISERVICE.GetItemReceivedList(Comp_ID, Branch, TransType, StockBy, ItemID, ShflID, LotNo, BatchNo, SerialNo, UpToDate);

                DataView dv = new DataView(result.Tables[0]);
                dv.Sort = "CDate desc";
                DataTable dt = new DataTable();
                dt = dv.ToTable();
                DataSet dset = new DataSet();
                dset.Tables.Add(dt);
                DataRows = Json(JsonConvert.SerializeObject(dset), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        //[HttpPost]
        //public JsonResult GetSubItemStkDetailList(string StockBy, string ItemID, string Branch, string WareHouse, string UpToDate)
        //{
        //    try
        //    {
        //        JsonResult DataRows = null;
        //        string Comp_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        DataSet result = _StockDetailShopfloor_ISERVICE.GetSubItemStkList(Comp_ID, Branch, StockBy, ItemID, WareHouse, UpToDate);

        //        DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

        //        return DataRows;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //}        
        public ActionResult GetSubItemStkDetailList(string StockBy, string ItemID, string Branch, string WareHouse, string UpToDate,string IncludeZero)
        {
            try
            {
                //JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _StockDetailShopfloor_ISERVICE.GetSubItemStkList(Comp_ID, Branch, StockBy, ItemID, WareHouse, UpToDate, IncludeZero);
                ViewBag.ResultData = result;
                ViewBag.FlagWhShfl = "Shfl";
                //DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSubItemStockDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public JsonResult LoadDataTable(string ItemListFilter)
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

                List<StockListDetailShopfloor> _ItemListModel = new List<StockListDetailShopfloor>();

                (_ItemListModel, recordsTotal) = getDtList(ItemListFilter, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                ////Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                //}
                var data = ItemListData.ToList();
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
        private (List<StockListDetailShopfloor>, int) getDtList(string ItemListFilter, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir)
        {
            List<StockListDetailShopfloor> _ItemListModel = new List<StockListDetailShopfloor>();
            CommonController cmn = new CommonController();
            int Total_Records = 0;
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                string StockBy, ItemId , GroupId, ShopfloorID, hsnCode , UpToDate , PortfolioId, IncludeZeroStock, BranchIDList;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
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

                        //StockBy = fdata[0];
                        //ItemId = fdata[1];
                        //GroupId = fdata[2];
                        //BrachID = fdata[3];
                        //ShopfloorID = fdata[4];
                        //hsnCode = fdata[5];
                        //UpToDate = fdata[6];
                        //PortfolioId = fdata[7];
                        //IncludeZeroStock = fdata[8
                        //
                        StockBy = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        ItemId = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        GroupId = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        ShopfloorID = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        hsnCode = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        UpToDate = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));
                        PortfolioId = cmn.ReplaceDefault(fdata[6].Trim('[', ']'));
                        IncludeZeroStock = cmn.ReplaceDefault(fdata[7].Trim('[', ']'));
                        BranchIDList = cmn.ReplaceDefault(fdata[8].Trim('[', ']'));
                        DSet = _StockDetailShopfloor_ISERVICE.GetStockDetailShopfloorList(CompID, Br_ID, IncludeZeroStock, StockBy
                            ,ItemId,GroupId, ShopfloorID
                            , UpToDate, PortfolioId, hsnCode, skip, pageSize, searchValue, sortColumn, sortColumnDir, BranchIDList);
                    }
                    else
                    {

                    }
                }
                else
                {

                }
                if (DSet.Tables.Count >= 2)
                {
                    if (DSet.Tables[0].Rows.Count > 0)
                    {
                        _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new StockListDetailShopfloor
                {
                    SrNo = Convert.ToInt32(row.Field<Int64>("SrNo")),
                    ItemName = row.Field<string>("itemname"),
                    HsnCode = row.Field<string>("HSNCode"),
                    subItem = row.Field<string>("sub_item"),
                    ItemID = row.Field<string>("itemid"),
                    UOM = row.Field<string>("uomname"),
                    Branch = row.Field<string>("brname"),
                    BranchID = row.Field<Int32>("brid").ToString(),
                    Shopfloor = row.Field<string>("shflname"),
                    ShopfloorID = row.Field<Int32>("shflid").ToString(),
                    Lot = row.Field<string>("lotno"),
                    Batch = row.Field<string>("batchno"),
                    Serial = row.Field<string>("serialno"),
                    OpeningStock = row.Field<string>("opening"),
                    Receipts = row.Field<string>("receipts"),
                    Issued = row.Field<string>("issued"),
                    ReservedStock = row.Field<string>("reserved"),
                    RejectedStock = row.Field<string>("rejected"),
                    ReworkableStock = row.Field<string>("reworkabled"),
                    WIPStock = row.Field<string>("WIPStock"),
                    TotalStock = row.Field<string>("totalstk"),
                    TotalStockVal = row.Field<string>("totalstkval"),
                    AvailableStock = row.Field<string>("avlstk"),
                    StockValue = row.Field<string>("avlstkvalue"),
                    sub_item_id = row.Field<string>("sub_item_id"),
                    sub_item_name = row.Field<string>("sub_item_name")
                }).ToList();

                    }
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

        public ActionResult StockDetailShopfloorActions(StockDetailShopfloor_Model _Model, string Command)
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
        public FileResult GenrateCsvFile(StockDetailShopfloor_Model _Model)
        {
            try
            {
                CommonController cmn = new CommonController();
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                string StockBy="", ItemId, GroupId, ShopfloorID, hsnCode, UpToDate, PortfolioId, IncludeZeroStock = string.Empty, BranchIDList;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
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
                    //fdata = Fstring.Split(',');
                    fdata = Fstring.Split('_');

                    //StockBy = fdata[0];
                    //ItemId = fdata[1];
                    //GroupId = fdata[2];
                    //BrachID = fdata[3];
                    //ShopfloorID = fdata[4];
                    //hsnCode = fdata[5];
                    //UpToDate = fdata[6];
                    //PortfolioId = fdata[7];
                    //IncludeZeroStock = fdata[8];

                    StockBy = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                    ItemId = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                    GroupId = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                    ShopfloorID = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                    hsnCode = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                    UpToDate = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));
                    PortfolioId = cmn.ReplaceDefault(fdata[6].Trim('[', ']'));
                    IncludeZeroStock = cmn.ReplaceDefault(fdata[7].Trim('[', ']'));
                    BranchIDList = cmn.ReplaceDefault(fdata[8].Trim('[', ']'));

                    DSet = _StockDetailShopfloor_ISERVICE.GetStockDetailShopfloorList(CompID, Br_ID, IncludeZeroStock, StockBy
                        , ItemId, GroupId, ShopfloorID
                        , UpToDate, PortfolioId, hsnCode, 0, 25, "", "SrNo", "Asc", BranchIDList,"CSV");
                }
                DataTable newTable = new DataTable();

                newTable.Columns.Add("Sr.No.", typeof(string));
                newTable.Columns.Add("Item Name", typeof(string));
                newTable.Columns.Add("HSN/SAC Code", typeof(string));
                newTable.Columns.Add("UOM", typeof(string));
                newTable.Columns.Add("Branch", typeof(string));
                if (StockBy == "Warehousewisestock")
                {
                    newTable.Columns.Add("Shopfloor", typeof(string));
                }
                else if (StockBy == "LotBatchwisestock")
                {
                    newTable.Columns.Add("Shopfloor", typeof(string));
                    newTable.Columns.Add("Lot#", typeof(string));
                    newTable.Columns.Add("Batch#", typeof(string));
                    newTable.Columns.Add("Serial#", typeof(string));
                }
                else if (StockBy == "SubitemWise")
                {
                    newTable.Columns.Add("Shopfloor", typeof(string));
                    newTable.Columns.Add("Sub-Item Name", typeof(string));
               
                }
                newTable.Columns.Add("Receipts", typeof(string));
                newTable.Columns.Add("Issued", typeof(string));
                if (StockBy == "SubitemWise")
                {
                    newTable.Columns.Add("Reserved Stock", typeof(string));
                }
                newTable.Columns.Add("Rejected Stock", typeof(string));
                newTable.Columns.Add("Reworkable Stock", typeof(string));
                if (StockBy != "SubitemWise")
                {
                    newTable.Columns.Add("WIP Stock", typeof(string));
                }
                newTable.Columns.Add("Total Stock", typeof(string));
                newTable.Columns.Add("Total Stock Value", typeof(string));
                newTable.Columns.Add("Available Stock", typeof(string));
                newTable.Columns.Add("Available Stock Value", typeof(string));
                
                // Copy relevant data from the original table
                foreach (DataRow row in DSet.Tables[0].Rows)
                {
                    if (StockBy == "Branchwisestock")
                    {
                        newTable.Rows.Add(row.Field<Int64>("SrNo").ToString(), row["itemname"], row["HSNCode"]
                            , row["uomname"], row["brname"], row["receipts"], row["issued"]
                            , row["rejected"], row["reworkabled"]
                        , row["WIPStock"], row["totalstk"], row["totalstkval"], row["avlstk"], row["avlstkvalue"]); // Selecting only needed columns
                    }
                    else if (StockBy == "Warehousewisestock")
                    {
                        newTable.Rows.Add(row.Field<Int64>("SrNo").ToString(), row["itemname"], row["HSNCode"]
                           , row["uomname"], row["brname"], row["shflname"], row["receipts"], row["issued"], row["rejected"], row["reworkabled"]
                       , row["WIPStock"], row["totalstk"], row["totalstkval"], row["avlstk"], row["avlstkvalue"]); // Selecting only needed columns
                    }
                    else if (StockBy == "LotBatchwisestock")
                    {
                        newTable.Rows.Add(row.Field<Int64>("SrNo").ToString(), row["itemname"], row["HSNCode"]
                           , row["uomname"], row["brname"], row["shflname"], row["lotno"], row["batchno"], row["serialno"]
                           , row["receipts"], row["issued"], row["rejected"], row["reworkabled"]
                       , row["WIPStock"], row["totalstk"], row["totalstkval"], row["avlstk"], row["avlstkvalue"]); // Selecting only needed columns
                    }
                    else if (StockBy == "SubitemWise")
                    {
                        newTable.Rows.Add(Convert.ToInt32(row.Field<Int64>("SrNo")).ToString(), row["itemname"], row["HSNCode"]
                           , row["uomname"], row["brname"], row["shflname"], row["sub_item_name"]
                           , row["receipts"], row["issued"], row["reserved"], row["rejected"], row["reworkabled"]
                           , row["totalstk"], row["totalstkval"], row["avlstk"], row["avlstkvalue"]); // Selecting only needed columns
                    }

                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("StockDetailShopfloor", newTable);
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