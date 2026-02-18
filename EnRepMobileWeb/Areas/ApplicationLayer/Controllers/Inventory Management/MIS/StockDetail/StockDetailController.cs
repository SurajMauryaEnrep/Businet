using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.MiscellaneousSetup;
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
using System.Reflection;
using System.Text;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.StockDetail
{
    public class StockDetailController : Controller
    {
        //DataTable ItemListDs;
        string DocumentMenuId = "105102180101",title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        StockDetail_ISERVICE _StockDetail_ISERVICE;
        Common_IServices _Common_IServices;
        private readonly HSNCode_ISERVICES _HSNNCode_Iservices;
        public StockDetailController(StockDetail_ISERVICE _StockDetail_ISERVICE, Common_IServices _Common_IServices, HSNCode_ISERVICES HSNNCode_Iservices)
        {
            this._StockDetail_ISERVICE = _StockDetail_ISERVICE;
            this._Common_IServices = _Common_IServices;
            this._HSNNCode_Iservices = HSNNCode_Iservices;
        }
        // GET: ApplicationLayer/StockDetail
        public ActionResult StockDetail(StockDetail_Model _StockDetail_Model)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string Language = string.Empty;
                //StockDetail_Model _StockDetail_Model = new StockDetail_Model();
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
                GetItemList(_StockDetail_Model);
                GetItemGrpList(_StockDetail_Model);
                GetItemPortfList(_StockDetail_Model);
                GetWarehouseList(_StockDetail_Model);
                GetBrachList(_StockDetail_Model);
                _StockDetail_Model.UptoDate = DateTime.Now.ToString("yyyy-MM-dd"); //last_Date.ToString("yyyy-MM-dd");
                _StockDetail_Model.BranchName = Br_ID;
                //_StockDetail_Model.StockDetailList = SearchStockDetail_load(_StockDetail_Model);
                if (_StockDetail_Model.ExpiredItm == "Y" || _StockDetail_Model.NearExpiryItm == "Y")
                {
                    _StockDetail_Model.StockBy = "LotBatchwisestock";
                }
                else
                {
                    _StockDetail_Model.StockBy = "Branchwisestock";
                }
                    
                _StockDetail_Model.HsnCodeList = GetHsnCodeList(Comp_ID);
                _StockDetail_Model.SuppName_List = GetSuppName(Comp_ID, Br_ID);
                _StockDetail_Model.StockGlAccList = GetStockGlAccList(Comp_ID);
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _StockDetail_Model.Title = title;
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/StockDetail/StockDetail.cshtml", _StockDetail_Model);
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
                StockDetail_Model _StockDetail_Model = new StockDetail_Model();
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
                GetItemList(_StockDetail_Model);
                GetItemGrpList(_StockDetail_Model);
                GetItemPortfList(_StockDetail_Model);
                GetWarehouseList(_StockDetail_Model);
                GetBrachList(_StockDetail_Model);
                _StockDetail_Model.SuppName_List = GetSuppName(Comp_ID, Br_ID);
                _StockDetail_Model.UptoDate = ToDt;// DateTime.Now.ToString("yyyy-MM-dd"); //last_Date.ToString("yyyy-MM-dd");
                _StockDetail_Model.BranchName = Br_ID;
                //_StockDetail_Model.StockDetailList = SearchStockDetail_load(_StockDetail_Model);
                if (_StockDetail_Model.ExpiredItm == "Y" || _StockDetail_Model.NearExpiryItm == "Y")
                {
                    _StockDetail_Model.StockBy = "LotBatchwisestock";
                }
                else
                {
                    _StockDetail_Model.StockBy = "Branchwisestock";
                }
                _StockDetail_Model.HsnCodeList = GetHsnCodeList(Comp_ID);
                _StockDetail_Model.StockGlAccList = GetStockGlAccList(Comp_ID);
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _StockDetail_Model.Title = title;
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/StockDetail/StockDetail.cshtml", _StockDetail_Model);
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
           // hsnList.Add(new HsnCodeModel { hsn_code = "0", hsn_desc = "---Select---" });
            DataSet ds = _HSNNCode_Iservices.Get_HsnDetails(compId);
            if(ds.Tables.Count > 0)
            {
                foreach(DataRow dtr in ds.Tables[0].Rows)
                {
                    hsnList.Add(new HsnCodeModel { hsn_code = dtr["hsn_no"].ToString(),hsn_desc = dtr["hsn_no"].ToString() });
                }
            }
            return hsnList;
        } 
        private List<SuppNameList> GetSuppName(string compId,string Br_ID)
        {
            List<SuppNameList> hsnList = new List<SuppNameList>();
          
            DataSet ds = _StockDetail_ISERVICE.Get_SuppNameDetails(compId, Br_ID);
            if(ds.Tables.Count > 0)
            {
                foreach(DataRow dtr in ds.Tables[0].Rows)
                {
                    hsnList.Add(new SuppNameList { Supp_id = dtr["supp_id"].ToString(),Supp_Name = dtr["supp_name"].ToString() });
                }
            }
            return hsnList;
        }
        private List<StockGlAccount> GetStockGlAccList(string compId)
        {
            List<StockGlAccount> _StockGlAccList = new List<StockGlAccount>();
            //_StockGlAccList.Add(new StockGlAccount { acc_id = "0", acc_name = "---Select---" });
            DataSet ds = _Common_IServices.Cmn_Get_StockGlAccountList(compId);
            if(ds.Tables.Count > 0)
            {
                foreach(DataRow dtr in ds.Tables[0].Rows)
                {
                    _StockGlAccList.Add(new StockGlAccount { acc_id = dtr["acc_id"].ToString(),acc_name = dtr["acc_name"].ToString() });
                }
            }
            return _StockGlAccList;
        }
        
        public ActionResult GetItemList(StockDetail_Model queryParameters)
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
                ItemList = _StockDetail_ISERVICE.ItemList(ItemName, Comp_ID);

                List<ItemName> _ItemList = new List<ItemName>();
                _ItemList.Add(new ItemName
                {
                    Item_Id = "0",
                    Item_Name = "---Select---"
                });
                //foreach (var data in ItemList)
                //{
                //    ItemName _ItemDetail = new ItemName();
                //    _ItemDetail.Item_Id = data.Key;
                //    _ItemDetail.Item_Name = data.Value;
                //    _ItemList.Add(_ItemDetail);
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
        public ActionResult GetStockDetailFromDashBoard(string ExpiredItm, string StockOutItm,string NearExpiryItm)
        {
            StockDetail_Model _StockDetail_Model = new StockDetail_Model();
            _StockDetail_Model.ExpiredItm = ExpiredItm;
            _StockDetail_Model.StockOutItm = StockOutItm;
            _StockDetail_Model.NearExpiryItm = NearExpiryItm;
            return RedirectToAction("StockDetail", _StockDetail_Model);
        }
        public ActionResult GetItemGrpList(StockDetail_Model queryParameters)
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
                ItemGroupList = _StockDetail_ISERVICE.ItemGroupList(GroupName, Comp_ID);

                List<ItemGroupName> _ItemGroupList = new List<ItemGroupName>();
                _ItemGroupList.Add(new ItemGroupName { Group_Id="0",Group_Name="---Select---" });
                //foreach (var data in ItemGroupList)
                //{
                //    ItemGroupName _ItemGroupDetail = new ItemGroupName();
                //    _ItemGroupDetail.Group_Id = data.Key;
                //    _ItemGroupDetail.Group_Name = data.Value;
                //    _ItemGroupList.Add(_ItemGroupDetail);
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
        public ActionResult GetItemPortfList(StockDetail_Model queryParameters)
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
                ItemPortfolioList = _StockDetail_ISERVICE.ItemPortfolioList(PortfolioName, Comp_ID);

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

        public void GetWarehouseList(StockDetail_Model queryParameters)
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
                DataSet Wh_List = _StockDetail_ISERVICE.GetWarehouseList(Comp_ID, Br_ID);

                DataRow Drow = Wh_List.Tables[0].NewRow();
               // Drow[1] = "---ALL---";
               // Drow[0] = "0";
               // Wh_List.Tables[0].Rows.InsertAt(Drow, 0);

                List<WarehouseName> _Whlist = new List<WarehouseName>();

                foreach (DataRow dr in Wh_List.Tables[0].Rows)
                {
                    WarehouseName _WhDetail = new WarehouseName();
                    _WhDetail.Wh_Id = dr["wh_id"].ToString();
                    _WhDetail.Wh_Name = dr["wh_name"].ToString();
                    _Whlist.Add(_WhDetail);
                }
                queryParameters.WarehouseNameList = _Whlist;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }

        public void GetBrachList(StockDetail_Model queryParameters)
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
                DataSet BranchList = _StockDetail_ISERVICE.GetAllBrchList(Comp_ID, User_id);

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
        public ActionResult SearchStockDetail(string IncludeZeroStockFlag, string StockBy, string ItemId, string GroupId, string BrachID
            , string WarehouseID, string UpToDate, string PortfolioId, string hsnCode,string ExpiredItems,string StockoutItems,string NearExpiryItm
            ,string StkGlAccId)
        {
            try
            {
                List<StockListDetail> _StockListDetail = new List<StockListDetail>();
                StockDetail_Model _StockDetail_Model = new StockDetail_Model();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
                DataTable dt = new DataTable();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string ReturnPartial = "";
                StringBuilder filters = new StringBuilder();
                if(StockBy== "ItemGroupWise_Popup" || StockBy == "StockGlAccountWise_Popup" || StockBy == "ItemAliasWiseStock_Popup")
                {
                    ReturnPartial = "Popup";
                    //ViewBag.PartialTableId = StockBy == "ItemGroupWise_Popup" ? "IG_stockDetailDtList" : StockBy == "StockGlAccountWise_Popup" ? "SGA_stockDetailDtList" : "";
                    StockBy = "Branchwisestock";

                    //_StockDetail_Model.GroupID = GroupId;
                    //_StockDetail_Model.stock_gl_acc_id = StkGlAccId;
                    
                }
                //dt = _StockDetail_ISERVICE.GetStockDetailList(CompID, BrachID, IncludeZeroStockFlag, StockBy, ItemId, GroupId, WarehouseID, UpToDate, PortfolioId, hsnCode, ExpiredItems, StockoutItems, NearExpiryItm, StkGlAccId);


                //Session["StockBy"] = StockBy;
                _StockDetail_Model.StockBy = StockBy;
                //Session["StockByFilter"] = StockBy;
                _StockDetail_Model.StockByFilter = StockBy;
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        StockListDetail _StockDetailList = new StockListDetail();
                //        _StockDetailList.ItemName = dr["itemname"].ToString();
                //        _StockDetailList.Hsn_Code = dr["HsnCode"].ToString();
                //        _StockDetailList.SubItem = dr["sub_item"].ToString();
                //        _StockDetailList.ItemID = dr["itemid"].ToString();
                //        _StockDetailList.UOM = dr["uomname"].ToString();
                //        _StockDetailList.Branch = dr["brname"].ToString();
                //        _StockDetailList.BranchID = dr["brid"].ToString();
                //        _StockDetailList.sub_item_id = dr["sub_item_id"].ToString();
                //        _StockDetailList.sub_item_name = dr["sub_item_name"].ToString();
                //        _StockDetailList.Warehouse = dr["whname"].ToString();
                //        _StockDetailList.WarehouseID = dr["whid"].ToString();
                //        _StockDetailList.Lot = dr["lotno"].ToString();
                //        _StockDetailList.Batch = dr["batchno"].ToString();
                //        _StockDetailList.exp_dt = dr["exp_dt"].ToString();
                //        _StockDetailList.Serial = dr["serialno"].ToString();
                //        _StockDetailList.OpeningStock = dr["opening"].ToString();
                //        _StockDetailList.Receipts = dr["receipts"].ToString();
                //        _StockDetailList.Issued = dr["issued"].ToString();
                //        _StockDetailList.ReservedStock = dr["reserved"].ToString();
                //        _StockDetailList.RejectedStock = dr["rejected"].ToString();
                //        _StockDetailList.ReworkableStock = dr["reworkabled"].ToString();
                //        _StockDetailList.TotalStock = dr["totalstk"].ToString();
                //        _StockDetailList.TotalStockVal = dr["totalstkval"].ToString();
                //        _StockDetailList.AvailableStock = dr["avlstk"].ToString();
                //        _StockDetailList.StockValue = dr["avlstkvalue"].ToString();                   
                //        _StockDetailList.min_stk_lvl = dr["min_stk_lvl"].ToString();                   
                //        _StockDetailList.Saleable = dr["Saleable"].ToString();                   
                      
                //        _StockListDetail.Add(_StockDetailList);
                //    }
                //}

                //_StockDetail_Model.StockDetailList = _StockListDetail;
                CommonPageDetails();
                if (ReturnPartial == "Popup")
                {
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/_PartialStockDetailsItemGroupAndStockAccWise.cshtml", _StockDetail_Model);
                }
                else
                {
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialStockDetailListNew.cshtml", _StockDetail_Model);
                }
                
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        //private List<StockListDetail> SearchStockDetail_load(StockDetail_Model _StockDetail_Model)
        //{
        //    try
        //    {
        //        List<StockListDetail> _StockListDetail = new List<StockListDetail>();
        //        string CompID = string.Empty;
        //        string Br_ID = string.Empty;
        //        DataTable dt = new DataTable();

        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        var ExpiredItm = _StockDetail_Model.ExpiredItm;
        //        var StockOutItm = _StockDetail_Model.StockOutItm;
        //        var NearExpiryItm = _StockDetail_Model.NearExpiryItm;
        //        if(ExpiredItm == "Y")
        //            dt = _StockDetail_ISERVICE.GetStockDetailList(CompID, Br_ID, "N", "LotBatchwisestock", "0", "0", "0", _StockDetail_Model.UptoDate, "0", "0", "Y", "N", NearExpiryItm);
        //        else if (StockOutItm == "Y")
        //            dt = _StockDetail_ISERVICE.GetStockDetailList(CompID, Br_ID, "Y", "Branchwisestock", "0", "0", "0", _StockDetail_Model.UptoDate, "0", "0", "N", "Y", NearExpiryItm);
        //        else if (NearExpiryItm == "Y")
        //            dt = _StockDetail_ISERVICE.GetStockDetailList(CompID, Br_ID, "N", "LotBatchwisestock", "0", "0", "0", _StockDetail_Model.UptoDate, "0", "0", "N", "N", NearExpiryItm);
        //        else
        //            dt = _StockDetail_ISERVICE.GetStockDetailList(CompID, Br_ID, "N", "Branchwisestock", "0", "0", "0", _StockDetail_Model.UptoDate, "0", "0", "N", "N", NearExpiryItm);
        //        if (ExpiredItm == "Y"|| NearExpiryItm == "Y")
        //        {
        //            _StockDetail_Model.StockBy = "LotBatchwisestock";
        //            _StockDetail_Model.StockByFilter = "LotBatchwisestock";
        //        }
        //        else
        //        {
        //            _StockDetail_Model.StockBy = "Branchwisestock";
        //            _StockDetail_Model.StockByFilter = "Branchwisestocktbl";
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
                                       
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                StockListDetail _StockDetailList = new StockListDetail();
        //                _StockDetailList.ItemName = dr["itemname"].ToString();
        //                _StockDetailList.Hsn_Code = dr["HsnCode"].ToString();
        //                _StockDetailList.SubItem = dr["sub_item"].ToString();
        //                _StockDetailList.ItemID = dr["itemid"].ToString();
        //                _StockDetailList.UOM = dr["uomname"].ToString();
        //                _StockDetailList.Branch = dr["brname"].ToString();
        //                _StockDetailList.BranchID = dr["brid"].ToString();
        //                _StockDetailList.Warehouse = dr["whname"].ToString();
        //                _StockDetailList.WarehouseID = dr["whid"].ToString();
        //                _StockDetailList.Lot = dr["lotno"].ToString();
        //                _StockDetailList.Batch = dr["batchno"].ToString();
        //                _StockDetailList.Serial = dr["serialno"].ToString();
        //                _StockDetailList.OpeningStock = dr["opening"].ToString();
        //                _StockDetailList.Receipts = dr["receipts"].ToString();
        //                _StockDetailList.Issued = dr["issued"].ToString();
        //                _StockDetailList.ReservedStock = dr["reserved"].ToString();
        //                _StockDetailList.RejectedStock = dr["rejected"].ToString();
        //                _StockDetailList.ReworkableStock = dr["reworkabled"].ToString();
        //                _StockDetailList.TotalStock = dr["totalstk"].ToString();
        //                _StockDetailList.TotalStockVal = dr["totalstkval"].ToString();
        //                _StockDetailList.AvailableStock = dr["avlstk"].ToString();
        //                _StockDetailList.StockValue = dr["avlstkvalue"].ToString();
        //                _StockDetailList.sub_item_id = dr["sub_item_id"].ToString();
        //                _StockDetailList.sub_item_name = dr["sub_item_name"].ToString();
        //                _StockDetailList.min_stk_lvl = dr["min_stk_lvl"].ToString();
        //                _StockDetailList.Saleable = dr["Saleable"].ToString();
        //                _StockListDetail.Add(_StockDetailList);
        //            }
        //        }
        //        return _StockListDetail;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}

        [HttpPost]
        public JsonResult GetItemReceivedDetailList(string StockBy, string ItemID, string Branch,string TransType, string WareHouse, string LotNo, string BatchNo, string SerialNo, string UpToDate)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _StockDetail_ISERVICE.GetItemReceivedList(Comp_ID, Branch, TransType, StockBy, ItemID, WareHouse, LotNo, BatchNo, SerialNo, UpToDate);

                DataView dv =new DataView(result.Tables[0]);
                dv.Sort = "CDate desc";
                DataTable dt = new DataTable();
                dt = dv.ToTable();
                DataSet dset =new DataSet();
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
        //        DataSet result = _StockDetail_ISERVICE.GetSubItemStkList(Comp_ID, Branch, StockBy, ItemID, WareHouse,UpToDate);

        //        //DataView dv = new DataView(result.Tables[0]);
        //        //dv.Sort = "CDate desc";
        //        //DataTable dt = new DataTable();
        //        //dt = dv.ToTable();
        //        //DataSet dset = new DataSet();
        //        //dset.Tables.Add(dt);
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
        [HttpPost]
        public ActionResult GetSubItemStkDetailList(string StockBy, string ItemID, string Branch, string WareHouse, string UpToDate,string IncludeZero)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _StockDetail_ISERVICE.GetSubItemStkList(Comp_ID, Branch, StockBy, ItemID, WareHouse, UpToDate, IncludeZero);
               
                ViewBag.ResultData = result;
                ViewBag.FlagWhShfl = "Wh";
               
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSubItemStockDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult LoadData(string ItemListFilter/*, string IncludeZeroStockFlag, string StockBy, string ItemId, string GroupId, string BrachID, string WarehouseID, string UpToDate, string PortfolioId, string hsnCode*/)
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

                List<StockDetailsDataModel> _ItemListModel = new List<StockDetailsDataModel>();

                _ItemListModel = getStockDtList(ItemListFilter);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    ItemListData = FilteredList(ItemListData, searchValue);/* Added by Suraj Maurya on 25-09-2025 under ticket id 2829 */
                    //searchValue = searchValue.ToUpper();
                    //ItemListData = ItemListData.Where(m => m.itemname.ToUpper().Contains(searchValue) || m.HSNCode.ToUpper().Contains(searchValue)
                    //|| m.uomname.ToUpper().Contains(searchValue) || m.brname.ToUpper().Contains(searchValue) || m.opening.ToUpper().Contains(searchValue)
                    //|| m.receipts.ToUpper().Contains(searchValue) || m.issued.ToUpper().Contains(searchValue) || m.reserved.ToUpper().Contains(searchValue)
                    //|| m.rejected.ToUpper().Contains(searchValue) || m.reworkabled.ToUpper().Contains(searchValue) || m.totalstk.ToUpper().Contains(searchValue)
                    //|| m.totalstkval.ToUpper().Contains(searchValue) || m.avlstk.ToUpper().Contains(searchValue) || m.avlstkvalue.ToUpper().Contains(searchValue)
                    //|| m.sub_item_id.ToUpper().Contains(searchValue) || m.sub_item_name.ToUpper().Contains(searchValue)|| m.exp_dt.ToUpper().Contains(searchValue)
                    //|| m.min_stk_lvl.ToUpper().Contains(searchValue)|| m.Saleable.ToUpper().Contains(searchValue)
                    //);
                }

                //total number of rows count     
                recordsTotal = ItemListData.Count();
                //Paging     
                var data = ItemListData.Skip(skip).Take(pageSize).ToList();
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
        private List<StockDetailsDataModel> getStockDtList(string ItemListFilter)
        {
            List<StockDetailsDataModel> _ItemListModel = new List<StockDetailsDataModel>();
            CommonController cmn = new CommonController();
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                string IncludeZeroStockFlag, StockBy, ItemId, GroupId, BrachID, WarehouseID, UpToDate, PortfolioId, hsnCode
                    , ExpiredItems, StockoutItems, NearExpiryItm, StkGlAccId, ItemAlias, BranchIDList, Supp_Name;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataTable DSet = new DataTable();
                if (ItemListFilter != null)
                {
                    if (ItemListFilter != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = ItemListFilter;
                        fdata = Fstring.Split('_');


                        //IncludeZeroStockFlag = fdata[0];
                        //StockBy = fdata[1];
                        //ItemId = fdata[2];
                        //GroupId = fdata[3];
                        //BrachID = fdata[4];
                        //WarehouseID = fdata[5];
                        //UpToDate = fdata[6];
                        //PortfolioId = fdata[7];
                        //hsnCode = fdata[8];
                        //ExpiredItems = fdata[9];
                        //StockoutItems = fdata[10];
                        //NearExpiryItm = fdata[11];
                        //StkGlAccId = fdata[12];
                        //ItemAlias = fdata[13];
                        //Added by Nidhi on 19-11-2025
                        IncludeZeroStockFlag = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        StockBy = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        ItemId = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        GroupId = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        //BrachID = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        WarehouseID = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        UpToDate = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));
                        PortfolioId = cmn.ReplaceDefault(fdata[6].Trim('[', ']'));
                        hsnCode = cmn.ReplaceDefault(fdata[7].Trim('[', ']'));
                        ExpiredItems = cmn.ReplaceDefault(fdata[8].Trim('[', ']'));
                        StockoutItems = cmn.ReplaceDefault(fdata[9].Trim('[', ']'));
                        NearExpiryItm = cmn.ReplaceDefault(fdata[10].Trim('[', ']'));
                        StkGlAccId = cmn.ReplaceDefault(fdata[11].Trim('[', ']'));
                        ItemAlias = cmn.ReplaceDefault(fdata[12].Trim('[', ']'));
                        BranchIDList = cmn.ReplaceDefault(fdata[13].Trim('[', ']'));
                        Supp_Name= cmn.ReplaceDefault(fdata[14].Trim('[', ']'));

                        DSet = _StockDetail_ISERVICE.GetStockDetailList(CompID, Br_ID, IncludeZeroStockFlag, StockBy, ItemId, GroupId, 
                            WarehouseID, UpToDate, PortfolioId, hsnCode, ExpiredItems, StockoutItems, NearExpiryItm, StkGlAccId, ItemAlias, BranchIDList, Supp_Name);
                    }
                    else
                    {
                        //DSet = _StockDetail_ISERVICE.GetStockDetailList(CompID, BrachID, IncludeZeroStockFlag, StockBy, ItemId, GroupId, WarehouseID, UpToDate, PortfolioId, hsnCode);
                        //Session.Remove("ILSearch");
                        //Session.Remove("IL_SSearch");
                    }
                }
                else
                {
                    //DSet = _StockDetail_ISERVICE.GetStockDetailList(CompID, BrachID, IncludeZeroStockFlag, StockBy, ItemId, GroupId, WarehouseID, UpToDate, PortfolioId, hsnCode);
                    //Session.Remove("ILSearch");
                    //Session.Remove("IL_SSearch");
                }
                
                if (DSet.Rows.Count > 0)
                {

                    //int rowno = 0;
                    _ItemListModel = DSet.AsEnumerable()
               .Select((row, index) =>  new StockDetailsDataModel
               {
                   SrNo = index + 1,
                   compid = row.Field<Int32>("compid").ToString(),
                   brid = row.Field<Int32>("brid").ToString(),
                   brname = row.Field<string>("brname").ToString(),
                   itemid = row.Field<string>("itemid").ToString(),
                    itemname = row.Field<string>("itemname").ToString(),
                    HSNCode = row.Field<string>("HSNCode").ToString(),
                    sub_item = row.Field<string>("sub_item").ToString(),
                   uom = row.Field<Int32>("uom").ToString(),
                   uomname = row.Field<string>("uomname").ToString(),
                    whid = row.Field<Int32>("whid").ToString(),
                    whname = row.Field<string>("whname").ToString(),
                    lotno = row.Field<string>("lotno").ToString(),
                    batchno = IsNull(row.Field<string>("batchno"),""),
                    exp_dt = IsNull(row.Field<string>("exp_dt"),""),
                    serialno = IsNull(row.Field<string>("serialno"),""),
                   opening = Convert.ToDouble(row.Field<string>("opening")),
                   receipts = Convert.ToDouble(row.Field<string>("receipts")),
                   issued = Convert.ToDouble(row.Field<string>("issued")),
                   reserved = Convert.ToDouble(row.Field<string>("reserved")),
                   unreserved = Convert.ToDouble(row.Field<string>("unreserved")),
                   rejected = Convert.ToDouble(row.Field<string>("rejected")),
                   reworkabled = Convert.ToDouble(row.Field<string>("reworkabled")),
                   totalstk = Convert.ToDouble(row.Field<string>("totalstk")),
                   totalstk_in_sp = row.Field<string>("totalstk_in_sp_uom"),
                   totalstkval = Convert.ToDouble(row.Field<string>("totalstkval")),
                   avlstk = Convert.ToDouble(row.Field<string>("avlstk")),
                   avlstk_in_sp = row.Field<string>("avlstk_in_sp_uom"),
                   avlstkvalue = Convert.ToDouble(row.Field<string>("avlstkvalue")),
                   sub_item_id = row.Field<string>("sub_item_id").ToString(),
                   sub_item_name = row.Field<string>("sub_item_name").ToString(),
                   min_stk_lvl = Convert.ToDouble(row.Field<string>("min_stk_lvl")),
                   Saleable = row.Field<string>("Saleable").ToString(),
                   mfg_name = row.Field<string>("mfg_name") == null ? "" : row.Field<string>("mfg_name").ToString(),
                   mfg_mrp = row.Field<string>("mfg_mrp")==null ? "" : row.Field<string>("mfg_mrp").ToString(),
                   mfg_date = row.Field<string>("mfg_date") == null ? "" : row.Field<string>("mfg_date").ToString(),
                   supp_name = row.Field<string>("supp_name") == null ? "" : row.Field<string>("supp_name").ToString(),
               }).ToList();
                    //foreach (DataRow dr in DSet.Rows)
                    //{

                    //    StockDetailsDataModel _Item_List = new StockDetailsDataModel();
                    //    _Item_List.SrNo = rowno + 1;
                    //    _Item_List.compid = dr["compid"].ToString();
                    //    _Item_List.brid = dr["brid"].ToString();
                    //    _Item_List.brname = dr["brname"].ToString();
                    //    _Item_List.itemid = dr["itemid"].ToString();
                    //    _Item_List.itemname = dr["itemname"].ToString();
                    //    _Item_List.HSNCode = dr["HSNCode"].ToString();
                    //    _Item_List.sub_item = dr["sub_item"].ToString();
                    //    _Item_List.uom = dr["uom"].ToString();
                    //    _Item_List.uomname = dr["uomname"].ToString();
                    //    _Item_List.whid = dr["whid"].ToString();
                    //    _Item_List.whname = dr["whname"].ToString();
                    //    _Item_List.lotno = dr["lotno"].ToString();
                    //    _Item_List.batchno = dr["batchno"].ToString();
                    //    _Item_List.exp_dt = dr["exp_dt"].ToString();
                    //    _Item_List.serialno = dr["serialno"].ToString();
                    //    _Item_List.opening = Convert.ToDouble(dr["opening"]);
                    //    _Item_List.receipts = Convert.ToDouble(dr["receipts"]);
                    //    _Item_List.issued = Convert.ToDouble(dr["issued"]);
                    //    _Item_List.reserved = Convert.ToDouble(dr["reserved"]);
                    //    _Item_List.unreserved = Convert.ToDouble(dr["unreserved"]);
                    //    _Item_List.rejected = Convert.ToDouble(dr["rejected"]);
                    //    _Item_List.reworkabled = Convert.ToDouble(dr["reworkabled"]);
                    //    _Item_List.totalstk = Convert.ToDouble(dr["totalstk"]);
                    //    _Item_List.totalstk_in_sp = dr["totalstk_in_sp_uom"].ToString();
                    //    _Item_List.totalstkval = Convert.ToDouble(dr["totalstkval"]);
                    //    _Item_List.avlstk = Convert.ToDouble(dr["avlstk"]);
                    //    _Item_List.avlstk_in_sp = dr["avlstk_in_sp_uom"].ToString();
                    //    _Item_List.avlstkvalue = Convert.ToDouble(dr["avlstkvalue"]);
                    //    _Item_List.sub_item_id = dr["sub_item_id"].ToString();
                    //    _Item_List.sub_item_name = dr["sub_item_name"].ToString();
                    //    _Item_List.min_stk_lvl = Convert.ToDouble(dr["min_stk_lvl"]);
                    //    _Item_List.Saleable = dr["Saleable"].ToString();
                    //    _ItemListModel.Add(_Item_List);
                    //    rowno = rowno + 1;
                    //}
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                //return View("~/Views/Shared/Error.cshtml");
            }
            return _ItemListModel;
        }

        private string IsNull(object obj, string outStr)
        {
            string result = obj == null ? outStr : obj.ToString();
            return result;
        }

        // code wrote by - Suraj Maurya on 14-12-2023 (Use - Export Items and Serialization details to Excel)
        public FileResult ExportStkDtlData(string searchValue, string filters)
        {
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
                
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                var sortColumn = "itemname";// Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = "asc";// Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                
                List<StockDetailsDataModel> _ItemListModel = new List<StockDetailsDataModel>();

                _ItemListModel = getStockDtList(filters);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    ItemListData = FilteredList(ItemListData, searchValue);/* Added by Suraj Maurya on 25-09-2025 under ticket id 2829 */
                    //searchValue = searchValue.ToUpper();
                    //ItemListData = ItemListData.Where(m => m.itemname.ToUpper().Contains(searchValue) || m.HSNCode.ToUpper().Contains(searchValue)
                    //|| m.uomname.ToUpper().Contains(searchValue) || m.brname.ToUpper().Contains(searchValue) || m.opening.ToUpper().Contains(searchValue)
                    //|| m.receipts.ToUpper().Contains(searchValue) || m.issued.ToUpper().Contains(searchValue) || m.reserved.ToUpper().Contains(searchValue)
                    //|| m.rejected.ToUpper().Contains(searchValue) || m.reworkabled.ToUpper().Contains(searchValue) || m.totalstk.ToUpper().Contains(searchValue)
                    //|| m.totalstkval.ToUpper().Contains(searchValue) || m.avlstk.ToUpper().Contains(searchValue)|| m.avlstkvalue.ToUpper().Contains(searchValue)
                    // || m.sub_item_id.ToUpper().Contains(searchValue) || m.sub_item_name.ToUpper().Contains(searchValue)|| m.exp_dt.ToUpper().Contains(searchValue)
                    // || m.min_stk_lvl.ToUpper().Contains(searchValue)|| m.Saleable.ToUpper().Contains(searchValue));
                }
  
                var data = ItemListData.ToList();
                //var data = ItemListData.Take(pageSize).ToList();
                //Returning Json Data    
                //return Json(new { data = data });

                string Fstring = string.Empty;
                string[] fdata;
                Fstring = filters;
                fdata = Fstring.Split(',');


                DataTable dt = new DataTable();

                dt = ToBrWiseStockDetailExl(data, fdata[1]);
                
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Stock Detail", dt);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
           
        }
        public DataTable ToBrWiseStockDetailExl(List<StockDetailsDataModel> _ItemListModel,string StockBy)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr No.",typeof(int));
            dataTable.Columns.Add("Item Name",typeof(string));
            dataTable.Columns.Add("HSN Code", typeof(string));
            dataTable.Columns.Add("UOM",typeof(string));
            dataTable.Columns.Add("Minimum Stock Level", typeof(string));
            dataTable.Columns.Add("Branch",typeof(string));
            if (StockBy == "Warehousewisestock")
            {
                dataTable.Columns.Add("Warehouse", typeof(string));
            }
            else if (StockBy == "LotBatchwisestock")
            {
                dataTable.Columns.Add("Warehouse", typeof(string));
                dataTable.Columns.Add("Supplier Name", typeof(string));
                dataTable.Columns.Add("Lot#", typeof(string));
                dataTable.Columns.Add("Batch#", typeof(string));
                dataTable.Columns.Add("Saleable/Non Saleable", typeof(string));
                dataTable.Columns.Add("Serial#", typeof(string));
            }
            else if (StockBy == "SubitemWise")
            {
                dataTable.Columns.Add("Warehouse", typeof(string));
                dataTable.Columns.Add("Sub-Item Name", typeof(string));
            }
            dataTable.Columns.Add("Opening Stock",typeof(decimal));
            dataTable.Columns.Add("Reciept",typeof(decimal));
            dataTable.Columns.Add("Issued",typeof(decimal));
            dataTable.Columns.Add("Reserved Stock", typeof(decimal));
            dataTable.Columns.Add("Rejected Stock", typeof(decimal));
            dataTable.Columns.Add("Reworkable Stock", typeof(decimal));
            dataTable.Columns.Add("Total Stock", typeof(decimal));
            dataTable.Columns.Add("Total Stock (In Specific)", typeof(string));
            dataTable.Columns.Add("Total Stock Value", typeof(decimal));
            dataTable.Columns.Add("Available Stock", typeof(decimal));
            dataTable.Columns.Add("Available Stock (In Specific)", typeof(string));
            dataTable.Columns.Add("Available Stock Value", typeof(decimal));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr No."] = item.SrNo;
                rows["Item Name"] = item.itemname;
                rows["HSN Code"] = item.HSNCode;
                rows["UOM"] = item.uomname;
                rows["Minimum Stock Level"] = item.min_stk_lvl;
                rows["Branch"] = item.brname;
                if (StockBy == "Warehousewisestock")
                {
                    rows["Warehouse"] = item.whname;
                }
                else if (StockBy == "LotBatchwisestock")
                {
                    rows["Warehouse"] = item.whname;
                    rows["Supplier Name"] = item.supp_name;
                    rows["Lot#"] = item.lotno;
                    rows["Batch#"] = item.batchno;
                    rows["Saleable/Non Saleable"] = item.Saleable;
                    rows["Serial#"] = item.serialno;
                }
                else if (StockBy == "SubitemWise")
                {
                    rows["Warehouse"] = item.whname;
                    rows["Sub-Item Name"] = item.sub_item_name;
                }
                rows["Opening Stock"] = item.opening;
                rows["Reciept"] = item.receipts;
                rows["Issued"] = item.issued;
                rows["Reserved Stock"] = item.reserved;
                rows["Rejected Stock"] = item.rejected;
                rows["Reworkable Stock"] = item.reworkabled;
                rows["Total Stock"] = item.totalstk;            
                rows["Total Stock (In Specific)"] =item.totalstk_in_sp;
                rows["Total Stock Value"] = item.totalstkval;
                rows["Available Stock"] = item.avlstk;
                rows["Available Stock (In Specific)"] = item.avlstk_in_sp;
                rows["Available Stock Value"] = item.avlstkvalue;

                dataTable.Rows.Add(rows);
            }

            return dataTable;
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        private IEnumerable<StockDetailsDataModel> FilteredList(IEnumerable<StockDetailsDataModel> ItemListData, string searchValue)
        {
            searchValue = searchValue.ToUpper();
            ItemListData = ItemListData.Where(m => m.SrNo.ToString().ToUpper().Contains(searchValue) || m.itemname.ToUpper().Contains(searchValue) || m.HSNCode.ToUpper().Contains(searchValue)
            || m.uomname.ToUpper().Contains(searchValue) || m.brname.ToUpper().Contains(searchValue)
            || m.whname.ToUpper().Contains(searchValue) || m.opening.ToString().ToUpper().Contains(searchValue)
            || m.receipts.ToString().ToUpper().Contains(searchValue) || m.issued.ToString().ToUpper().Contains(searchValue) || m.reserved.ToString().ToUpper().Contains(searchValue)
            || m.rejected.ToString().ToUpper().Contains(searchValue) || m.reworkabled.ToString().ToUpper().Contains(searchValue)
            || m.totalstk.ToString().ToUpper().Contains(searchValue) || m.totalstk_in_sp.ToString().ToUpper().Contains(searchValue)
            || m.totalstkval.ToString().ToUpper().Contains(searchValue) || m.avlstk.ToString().ToUpper().Contains(searchValue)
            || m.avlstk_in_sp.ToString().ToUpper().Contains(searchValue) || m.avlstkvalue.ToString().ToUpper().Contains(searchValue)
            || m.sub_item_name.ToUpper().Contains(searchValue) || m.exp_dt.ToUpper().Contains(searchValue)
            || m.min_stk_lvl.ToString().ToUpper().Contains(searchValue) || m.Saleable.ToUpper().Contains(searchValue)
            || m.lotno.ToUpper().Contains(searchValue) || m.batchno.ToUpper().Contains(searchValue) || m.serialno.ToUpper().Contains(searchValue)
             || m.mfg_name.ToUpper().Contains(searchValue) || m.mfg_mrp.ToUpper().Contains(searchValue) || m.mfg_date.ToUpper().Contains(searchValue) ||
              m.supp_name.ToUpper().Contains(searchValue) 
            );
            return ItemListData;
        }
        [HttpPost]
        public ActionResult ExportCsv([System.Web.Http.FromBody] DataTableRequest request)
        {
            string keyword = "";
            // 🔹 Fetch data same as LoadData but ignore paging
            var query = getStockDtList(request.ItemListFilter);

            // Apply search filter
            if (!string.IsNullOrEmpty(request.search?.value))
            {
                keyword = request.search.value.ToLower();
            }
            IEnumerable<StockDetailsDataModel> _ItemListModelList = FilteredList(query, keyword);
            
           
            if (request.order != null && request.order.Any())
            {
                var colIndex = request.order[0].column;
                var dir = request.order[0].dir;
                var sortColumn = request.columns[colIndex].data;

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(dir)))
                {
                    _ItemListModelList = _ItemListModelList.OrderBy(sortColumn + " " + dir);
                }
            }
              
            var data = _ItemListModelList.ToList(); // All filtered & sorted rows
            var commonController = new CommonController(_Common_IServices);
            return commonController.Cmn_GetDataToCsv(request, data);
        }

    }
}