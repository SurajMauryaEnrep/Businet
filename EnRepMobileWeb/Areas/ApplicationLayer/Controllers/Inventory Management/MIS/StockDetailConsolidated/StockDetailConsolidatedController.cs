using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockDetailConsolidated;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetailConsolidated;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.MiscellaneousSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using EnRepMobileWeb.Areas.Common.Controllers.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.StockDetailConsolidated
{
    public class StockDetailConsolidatedController : Controller
    {
        //DataTable ItemListDs;
        string compId = "", brId = "", language = "";
        string DocumentMenuId = "105102180104", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        StockDetailConsolidated_ISERVICE _StockDetailConsolidated_ISERVICE;
        Common_IServices _Common_IServices;
        private readonly HSNCode_ISERVICES _HSNNCode_Iservices;
        public StockDetailConsolidatedController(StockDetailConsolidated_ISERVICE _StockDetail_ISERVICE, Common_IServices _Common_IServices, HSNCode_ISERVICES HSNNCode_Iservices)
        {
            this._StockDetailConsolidated_ISERVICE = _StockDetail_ISERVICE;
            this._Common_IServices = _Common_IServices;
            this._HSNNCode_Iservices = HSNNCode_Iservices;
        }
        // GET: ApplicationLayer/StockDetailConsolidated
        [HttpGet]
        public ActionResult StockDetailConsolidated()
        {
            try
            {
                StockDetailConsolidated_Model _StockDetailConsolidated_Model = new StockDetailConsolidated_Model();
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["Language"] != null)
                    language = Session["Language"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                CommonPageDetails();

                //ViewBag.MenuPageName = getDocumentName();
                _StockDetailConsolidated_Model.Title = title;
                _StockDetailConsolidated_Model.AsOnDate = DateTime.Now.ToString("yyyy-MM-dd");
                _StockDetailConsolidated_Model.ItemsList = GetItemsList();
                _StockDetailConsolidated_Model.ItemsGroupList = GetItemsGroiupList();
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/StockDetailConsolidated/StockDetailConsolidated.cshtml", _StockDetailConsolidated_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataTable GetStockDetails(string itemId, string itemGroupId, string asonDate, string flag)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            DataTable StkTrfConsData = _StockDetailConsolidated_ISERVICE.GetStockDetailConsolidatedList(compId, brId, itemId, itemGroupId, asonDate, flag).Tables[0];
            return StkTrfConsData;
        }
        public ActionResult showReportFromDashBoard(string ToDt)
        {
            try
            {
                StockDetailConsolidated_Model _StockDetailConsolidated_Model = new StockDetailConsolidated_Model();
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["Language"] != null)
                    language = Session["Language"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                CommonPageDetails();

                //ViewBag.MenuPageName = getDocumentName();
                _StockDetailConsolidated_Model.Title = title;
                _StockDetailConsolidated_Model.AsOnDate = ToDt;
                _StockDetailConsolidated_Model.ItemsList = GetItemsList();
                _StockDetailConsolidated_Model.ItemsGroupList = GetItemsGroiupList();
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/StockDetailConsolidated/StockDetailConsolidated.cshtml", _StockDetailConsolidated_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private List<ItemListModel> GetItemsList()
        {
            #region Code Modified by Suraj Maurya on 04-03-2025 under ticket No. 1536 
            #endregion
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            DataTable dt = _StockDetailConsolidated_ISERVICE.GetItemsList("", compId);
            List<ItemListModel> ItemsList = new List<ItemListModel>();
            ItemListModel objitem = new ItemListModel()
            {
                Item_Id = "0",
                Item_Name = "---All---"
            };
            ItemsList.Add(objitem);
            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow dtr in dt.Rows)
            //    {
            //        objitem = new ItemListModel()
            //        {
            //            Item_Id = dtr["item_id"].ToString(),
            //            Item_Name = dtr["item_name"].ToString()
            //        };
            //        ItemsList.Add(objitem);
            //    }
            //}
            return ItemsList;
        }
        private List<ItemGroupListModel> GetItemsGroiupList()
        {
            #region Code Modified by Suraj Maurya on 04-03-2025 under ticket No. 1536 
            #endregion
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            DataTable dt = _StockDetailConsolidated_ISERVICE.GetItemsGroupList("", compId);
            List<ItemGroupListModel> ItemsGroupList = new List<ItemGroupListModel>();
            ItemGroupListModel objitemgrp = new ItemGroupListModel()
            {
                Group_Id = "0",
                Group_Name = "---All---"
            };
            //ItemsGroupList.Add(objitemgrp);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dtr in dt.Rows)
                {
                    objitemgrp = new ItemGroupListModel()
                    {
                        Group_Id = dtr["item_grp_id"].ToString(),
                        Group_Name = dtr["ItemGroupChildNood"].ToString()
                    };
                    ItemsGroupList.Add(objitemgrp);
                }
            }
            return ItemsGroupList;
        }
        [HttpPost]
        public ActionResult SearchStockDetailsConsolidated(string itemId, string itemGroupId, string asonDate)
        {
            //DataTable StkTrfConsData = GetStockDetails(itemId, itemGroupId, asonDate, "SUMMARY");
            CommonPageDetails();
            //ViewBag.StkTrfConsData = StkTrfConsData;
            StockDetailConsolidated_Model objmodel = new StockDetailConsolidated_Model();
            //objmodel.SearchStatus = "Searched";
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialStockDetailConsolidatedList.cshtml", objmodel);
        }
        public ActionResult SearchStockOnIBtnClick(string itemId, string asonDate, string flag)
        {
            DataTable StkTrfConsData = GetStockDetails(itemId, "0", asonDate, flag);
            ViewBag.StkTrfPopupData = StkTrfConsData;
            StockDetailConsolidated_Model objmodel = new StockDetailConsolidated_Model();
            objmodel.SearchStatus = "Searched";
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialStockDetailConsolidatedDetail.cshtml", objmodel);
        }
        public ActionResult GetShopfloorWIPStockDetail(string itemId, string asonDate, string flag)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            DataSet StkTrfConsData = _StockDetailConsolidated_ISERVICE.GetWipStockDetailConsolidatedList(compId, brId, itemId, "0", asonDate, flag);
            ViewBag.WIPDetail = StkTrfConsData.Tables[0];
            StockDetailConsolidated_Model objmodel = new StockDetailConsolidated_Model();
            objmodel.SearchStatus = "Searched";
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialWIPDetail.cshtml", objmodel);
        }
        public ActionResult GetSubItemStockDetails(string Item_id, string flag,string AsOnDate)/*Add by Hina on 04-09-2024 to add sub item stock detail*/
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
                
                DataSet ds = _StockDetailConsolidated_ISERVICE.GetSubItemStockDetails(Comp_ID, Br_ID, Item_id, flag, AsOnDate);
                ViewBag.SubitemAvlStockDetail = ds.Tables[0];
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.Flag = "";
                
                return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
                
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
        public JsonResult LoadStockDetailConsolidatedData(string ItemListFilter)
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

                List<StockDetailsConsolidatedList> _ItemListModel = new List<StockDetailsConsolidatedList>();

                (_ItemListModel, recordsTotal) = getDtList(ItemListFilter, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                }
   
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
        private (List<StockDetailsConsolidatedList>, int) getDtList(string ItemListFilter, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir)
        {
            List<StockDetailsConsolidatedList> _ItemListModel = new List<StockDetailsConsolidatedList>();
            CommonController cmn = new CommonController();
            int Total_Records = 0;
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string BrId = string.Empty;
                string itemId, itemGroupId, asonDate, flag;
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
                if (ItemListFilter != null)
                {
                    if (ItemListFilter != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = ItemListFilter;
                        //fdata = Fstring.Split(',');
                        fdata = Fstring.Split('_');

                        //itemId = fdata[0];
                        //itemGroupId = fdata[1];
                        //asonDate = fdata[2];
                        //flag = fdata[3];

                        itemId = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        itemGroupId = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        asonDate = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        flag = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));

                        DSet = _StockDetailConsolidated_ISERVICE
                            .GetStockDetailConsolidatedList(CompID, BrId, itemId, itemGroupId, asonDate, flag
                            , skip, pageSize, searchValue, sortColumn, sortColumnDir);
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
               .Select((row, index) => new StockDetailsConsolidatedList
               {
                   SrNo = Convert.ToInt32(row.Field<Int64>("SrNo")),
                   itemid = row.Field<string>("itemid").ToString(),
                   itemname = row.Field<string>("itemname").ToString(),
                   sub_item = row.Field<string>("sub_item").ToString(),
                   hsncode = row.Field<string>("hsncode").ToString(),
                   uom = row.Field<string>("uom").ToString(),
                   uomid = row.Field<int>("uomid").ToString(),
                   itemgrpid = row.Field<string>("itemgrpid").ToString(),
                   itemgroup = row.Field<string>("itemgroup").ToString(),
                   WhAvlStk = row.Field<string>("WhAvlStk").ToString(),
                   WhReservedStk = row.Field<string>("WhReservedStk").ToString(),
                   WhRejectStk = row.Field<string>("WhRejectStk").ToString(),
                   WhReworkStk = row.Field<string>("WhReworkStk").ToString(),
                   WhTotalStk = row.Field<string>("WhTotalStk").ToString(),
                   WhTotalStkVal = row.Field<string>("WhTotalStkVal").ToString(),
                   ShflAvlStk = row.Field<string>("ShflAvlStk").ToString(),
                   ShflRejectStk = row.Field<string>("ShflRejectStk").ToString(),
                   ShflReworkStk = row.Field<string>("ShflReworkStk").ToString(),
                   WIPStock = row.Field<string>("WIPStock").ToString(),
                   ShflTotalStk = row.Field<string>("ShflTotalStk").ToString(),
                   ShflTotalStkVal = row.Field<string>("ShflTotalStkVal").ToString(),
                   TotalAvlStk = row.Field<string>("TotalAvlStk").ToString(),
                   TotalReservedStk = row.Field<string>("TotalReservedStk").ToString(),
                   TotalRejectStk = row.Field<string>("TotalRejectStk").ToString(),
                   TotalReworkStk = row.Field<string>("TotalReworkStk").ToString(),
                   TotalStk = row.Field<string>("TotalStk").ToString(),
                   TotalStkVal = row.Field<string>("TotalStkVal").ToString()
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

        public ActionResult StockDetailsConsolidatedActions(StockDetailConsolidated_Model _Model, string Command)
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
        public FileResult GenrateCsvFile(StockDetailConsolidated_Model _Model)
        {
            #region this function is Created by Suraj Maurya on 05-03-2025 to Genarete CSV Files
            #endregion
            try
            {
                CommonController cmn = new CommonController();
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string BrId = string.Empty;
                string itemId, itemGroupId, asonDate, flag;
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
                if (!string.IsNullOrEmpty(_Model.Filters))
                {
                    string Fstring = string.Empty;
                    string[] fdata;
                    Fstring = _Model.Filters;
                    //fdata = Fstring.Split(',');
                    fdata = Fstring.Split('_');

                    //itemId = fdata[0];
                    //itemGroupId = fdata[1];
                    //asonDate = fdata[2];
                    //flag = fdata[3];

                    itemId = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                    itemGroupId = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                    asonDate = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                    flag = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));

                    DSet = _StockDetailConsolidated_ISERVICE
                        .GetStockDetailConsolidatedList(CompID, BrId, itemId, itemGroupId, asonDate, flag
                        , 0, 25, "", "SrNo", "ASC","CSV");
                }
                //Creating First Header 
                DataTable newTableHeader = new DataTable();

                for(int i = 0; i < 24; i++)
                {
                    newTableHeader.Columns.Add("", typeof(string));
                }
                
                newTableHeader.Rows.Add("", "", "", "", ""
                        , "Warehouse Stock Detail", "", "", "", "", ""
                        , "Shopfloor Stock Detail", "", "", "", "", ""
                        , "Consolidated Stock Detail", "", "", "", "", "", "");

                //Creating Main Header
                DataTable newTable = new DataTable();
                newTable.Columns.Add("Sr.No.", typeof(string));
                newTable.Columns.Add("Item Name", typeof(string));
                newTable.Columns.Add("HSN/SAC Code", typeof(string));
                newTable.Columns.Add("UOM", typeof(string));
                newTable.Columns.Add("Item Group", typeof(string));
                //------------
                newTable.Columns.Add("Available Stock", typeof(string));
                newTable.Columns.Add("Reserved Stock", typeof(string));
                newTable.Columns.Add("Rejected Stock", typeof(string));
                newTable.Columns.Add("Rework Stock", typeof(string));
                newTable.Columns.Add("Total Stock", typeof(string));
                newTable.Columns.Add("Total Stock Value", typeof(string));
                //------------
                newTable.Columns.Add("Available Stock ", typeof(string));//Extra space is given to this column due to duplicate columns
                newTable.Columns.Add("Rejected Stock ", typeof(string));
                newTable.Columns.Add("Rework Stock ", typeof(string));
                newTable.Columns.Add("WIP Stock ", typeof(string));
                newTable.Columns.Add("Total Stock ", typeof(string));
                newTable.Columns.Add("Total Stock Value ", typeof(string));
                //------------
                newTable.Columns.Add("Available Stock  ", typeof(string));//Extra space is given to this column due to duplicate columns
                newTable.Columns.Add("Reserved Stock  ", typeof(string));
                newTable.Columns.Add("Rejected Stock  ", typeof(string));
                newTable.Columns.Add("Rework Stock  ", typeof(string));
                newTable.Columns.Add("WIP Stock  ", typeof(string));
                newTable.Columns.Add("Total Stock  ", typeof(string));
                newTable.Columns.Add("Total Stock Value  ", typeof(string));

                // Adding Data
                foreach (DataRow row in DSet.Tables[0].Rows)
                {
                    newTable.Rows.Add(row["SrNo"], row["itemname"], row["hsncode"], row["uom"], row["itemgroup"]
                        , row["WhAvlStk"], row["WhReservedStk"], row["WhRejectStk"]
                        , row["WhReworkStk"], row["WhTotalStk"], row["WhTotalStkVal"]
                        , row["ShflAvlStk"], row["ShflRejectStk"], row["ShflReworkStk"], row["WIPStock"]
                        , row["ShflTotalStk"], row["ShflTotalStkVal"], row["TotalAvlStk"]
                        , row["TotalReservedStk"], row["TotalRejectStk"], row["TotalReworkStk"]
                        , row["WIPStock"], row["TotalStk"], row["TotalStkVal"]
                        );

                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Stock Detail Consolidated", newTable, newTableHeader);
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