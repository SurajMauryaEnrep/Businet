using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.BusinessLayer.ItemDetail;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using OfficeOpenXml;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using EnRepMobileWeb.MODELS.Common;
using ExcelDataReader;
//using DataTables.Mvc;
//***All Session Removed By Shubham Maurya 13-01-2023 ***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class ItemListController : Controller
    {
        string CompID, BrchID, UserId, language = String.Empty;
        string DocumentMenuId = "103105", title;
        Common_IServices _Common_IServices;
        List<ItemListModel> _ItemListModel;
        //DataTable ItemListDs;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        ItemList_ISERVICES _ItemList_ISERVICES;
        public ItemListController(Common_IServices _Common_IServices, ItemList_ISERVICES _ItemList_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._ItemList_ISERVICES = _ItemList_ISERVICES;
        }
        // GET: BusinessLayer/ItemList
        public ActionResult ItemList()
        {
            try
            {
                ItemDetailModel _ItemDetail = new ItemDetailModel();
                string Comp_ID = string.Empty;
                string User_ID = string.Empty;
                string Language = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    User_ID = Session["userid"].ToString();
                }
                if (TempData["ListFilterData"] != null)
                {
                    var ItemListFilter = TempData["ListFilterData"].ToString();
                    if (ItemListFilter != "" && ItemListFilter != null)
                    {
                        ViewBag.listdata = ItemListFilter;
                        _ItemDetail.ItemListFilter = ItemListFilter;
                        var a = ItemListFilter.Split(',');
                        _ItemDetail.ddl_ItemName = a[1].Trim();
                        _ItemDetail.ddlGroup = a[2].Trim();
                        _ItemDetail.ddlItemPortfolio = a[7].Trim();
                        _ItemDetail.ddlAttributeName = a[3].Trim();
                        _ItemDetail.ddlAttributeValue = a[4].Trim();
                        _ItemDetail.item_ActStatus = a[5].Trim();
                        _ItemDetail.Status = a[6].Trim();
                        _ItemDetail.ImageFilter = a[8].Trim();

                        List<AttributeValue> _AttributeValue = new List<AttributeValue>();
                        DataTable dt4 = GetAttributeValue(_ItemDetail);
                        foreach (DataRow dr in dt4.Rows)
                        {
                            AttributeValue ddlAttributeValue = new AttributeValue();
                            ddlAttributeValue.ID = dr["attr_val_id"].ToString();
                            ddlAttributeValue.Name = dr["attr_val_name"].ToString();
                            _AttributeValue.Add(ddlAttributeValue);
                        }
                        _AttributeValue.Insert(0, new AttributeValue() { ID = "0", Name = "All" });
                        _ItemDetail.AttributeValueList = _AttributeValue;
                    }
                    else
                    {
                        List<AttributeValue> _AttributeValue = new List<AttributeValue>();
                        _AttributeValue.Insert(0, new AttributeValue() { ID = "0", Name = "All" });
                        _ItemDetail.AttributeValueList = _AttributeValue;
                    }
                }
                else
                {
                    List<AttributeValue> _AttributeValue = new List<AttributeValue>();
                    _AttributeValue.Insert(0, new AttributeValue() { ID = "0", Name = "All" });
                    _ItemDetail.AttributeValueList = _AttributeValue;
                }
                /**Added By Nitesh 14-03-2024 GetAllDropdownlist**/
                GetAllDropdownlist(_ItemDetail);
                ViewBag.MenuPageName = getDocumentName();
                GetStatusList(_ItemDetail);
                _ItemDetail.Title = title;
                //Session["ILSearch"] = "0";
                _ItemDetail.ILSearch = "0";
                return View("~/Areas/BusinessLayer/Views/ItemSetup/ItemList.cshtml", _ItemDetail);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpGet]
        private void GetAllDropdownlist(ItemDetailModel _ItemDetail)
        {
            #region Added method(GetAllDropdownlist)  BY Nitesh 14-03-2024 For All List Data in One Procedure
            #endregion
            try
            {
                string GroupName = string.Empty;
                string Item_ID = string.Empty;
                string Itemportfolio = string.Empty;
                string AttributeName = string.Empty;
                string User_ID = string.Empty;
                string Comp_ID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    User_ID = Session["userid"].ToString();
                }
                if (string.IsNullOrEmpty(_ItemDetail.ddl_ItemName))
                {
                    Item_ID = "0";
                }
                else
                {
                    Item_ID = _ItemDetail.ddl_ItemName;
                }
                if (string.IsNullOrEmpty(_ItemDetail.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _ItemDetail.ddlGroup;
                }
                if (string.IsNullOrEmpty(_ItemDetail.ddlItemPortfolio))
                {
                    Itemportfolio = "0";
                }
                else
                {
                    Itemportfolio = _ItemDetail.ddlItemPortfolio;
                }
                if (string.IsNullOrEmpty(_ItemDetail.ddlAttributeName))
                {
                    AttributeName = "0";
                }
                else
                {
                    AttributeName = _ItemDetail.ddlAttributeName;
                }
                DataSet dt = new DataSet();
                dt = _ItemList_ISERVICES.BindGetAllDropDownList(GroupName, Comp_ID, BrchID, Item_ID, Itemportfolio, AttributeName, User_ID);


                //List<ItemName> _ItemName = new List<ItemName>();
                //// DataTable dt1 = GetItemList(_ItemDetail);
                ////DataSet dt1 = GetAllDropdownlist(_ItemDetail);
                //foreach (DataRow dr in dt.Tables[0].Rows)
                //{
                //    ItemName ddlItemName = new ItemName();
                //    ddlItemName.ID = dr["item_id"].ToString();
                //    ddlItemName.Name = dr["item_name"].ToString();
                //    _ItemName.Add(ddlItemName);
                //}
                //_ItemName.Insert(0, new ItemName() { ID = "0", Name = "All" });
                //_ItemDetail.ItemNameList = _ItemName;



                List<ItemName> _ItemName = new List<ItemName>();                   
                _ItemName.Insert(0, new ItemName() { Item_ID = "0", Item_Name = "All" });
                _ItemDetail.ItemNameList = _ItemName;
                List<GroupList> _GroupList = new List<GroupList>();
                //DataTable dt = GetGroupList(_ItemDetail);
                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    GroupList ddlGroupList = new GroupList();
                    ddlGroupList.ID = dr["item_grp_id"].ToString();
                    ddlGroupList.Name = dr["itemGroupChildNood"].ToString();
                    _GroupList.Add(ddlGroupList);
                }
                _GroupList.Insert(0, new GroupList() { ID = "0", Name = "All" });
                _ItemDetail.ItemGroupList = _GroupList;

                List<ItemPortfolio> _ItemPortfolio = new List<ItemPortfolio>();
                // DataTable dt2 = GetItemPortfList(_ItemDetail);
                // foreach (DataRow dr in dt2.Rows)
                foreach (DataRow dr in dt.Tables[2].Rows)
                {
                    ItemPortfolio ddlItemPortfolio = new ItemPortfolio();
                    ddlItemPortfolio.ID = dr["setup_id"].ToString();
                    ddlItemPortfolio.Name = dr["setup_val"].ToString();
                    _ItemPortfolio.Add(ddlItemPortfolio);
                }
                _ItemPortfolio.Insert(0, new ItemPortfolio() { ID = "0", Name = "All" });
                _ItemDetail.ItemPortfolioList = _ItemPortfolio;

                List<AttributeName> _ddlAttributeName = new List<AttributeName>();
                // DataTable dt3 = GetAttributeNameList(_ItemDetail);
                // foreach (DataRow dr in dt3.Rows)
                foreach (DataRow dr in dt.Tables[3].Rows)
                {
                    AttributeName ddlAttributeName = new AttributeName();
                    ddlAttributeName.ID = dr["attr_id"].ToString();
                    ddlAttributeName.Name = dr["attr_name"].ToString();
                    _ddlAttributeName.Add(ddlAttributeName);
                }
                _ddlAttributeName.Insert(0, new AttributeName() { ID = "0", Name = "All" });
                _ItemDetail.AttributeNameList = _ddlAttributeName;
                ViewBag.VBItemlist = dt.Tables[4];
                ViewBag.VBAccess = dt.Tables[5];
                var dt1 = dt.Tables[6].Rows.Count > 0 ? dt.Tables[6].Rows[0]["param_stat"].ToString() : "";
                _ItemDetail.Reference_noflag = dt1;
              
                //DataSet ItemListDs = new DataSet();
                //ItemListDs = _ItemList_ISERVICES.GetItemListDAL(Comp_ID, User_ID);
                //ViewBag.VBItemlist = ItemListDs.Tables[0];
                // ViewBag.VBAccess = ItemListDs.Tables[1];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private DataTable GetItemList(ItemDetailModel _ItemDetail)
        //{
        //    try
        //    {
        //        string GroupName = string.Empty;
        //        string Comp_ID = string.Empty;
        //        string BrchID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            BrchID = Session["BranchId"].ToString();
        //        }
        //        if (string.IsNullOrEmpty(_ItemDetail.ddl_ItemName))
        //        {
        //            GroupName = "0";
        //        }
        //        else
        //        {
        //            GroupName = _ItemDetail.ddl_ItemName;
        //        }
        //        DataTable dt = _ItemList_ISERVICES.BindGetItemList(GroupName, Comp_ID, BrchID);
        //        //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
        //        return dt;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}
        //private DataTable GetGroupList(ItemDetailModel _ItemDetail)
        //{
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        string GroupName = string.Empty;
        //        if (string.IsNullOrEmpty(_ItemDetail.ddlGroup))
        //        {
        //            GroupName = "0";
        //        }
        //        else
        //        {
        //            GroupName = _ItemDetail.ddlGroup;
        //        }
        //        DataTable dt = _ItemList_ISERVICES.BindGetGroupList(GroupName, Comp_ID);
        //        //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
        //        return dt;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;

        //    }
        //}
        //private DataTable GetItemPortfList(ItemDetailModel _ItemDetail)
        //{
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        string GroupName = string.Empty;
        //        if (string.IsNullOrEmpty(_ItemDetail.ddlItemPortfolio))
        //        {
        //            GroupName = "0";
        //        }
        //        else
        //        {
        //            GroupName = _ItemDetail.ddlItemPortfolio;
        //        }
        //        DataTable dt = _ItemList_ISERVICES.BindGetPortfNameList(GroupName, Comp_ID);
        //        //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
        //        return dt;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}
        //private DataTable GetAttributeNameList(ItemDetailModel _ItemDetail)
        //{
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        DataTable dt = _ItemList_ISERVICES.BindAttributeNameList(Comp_ID);
        //        //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
        //        return dt;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}
        private DataTable GetAttributeValue(ItemDetailModel _ItemDetail)
        {
            try
            {
                string GroupName = string.Empty;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_ItemDetail.ddlAttributeName))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _ItemDetail.ddlAttributeName;
                }
                DataTable dt = _ItemList_ISERVICES.BindGetAttributeValue(GroupName, Comp_ID);
                //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public void GetStatusList(ItemDetailModel _ItemDetail)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _ItemDetail.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult GetItemListFilter(string ItemID, string ItemGrpID, string AttrName, string AttrValue, string ActStatus, string ItmStatus, string ItemPrfID, string ImageFilter)
        {
            ItemDetailModel _ItemDetail = new ItemDetailModel();
            ViewBag.VBSupplierList = null;
            string Comp_ID = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            try
            {
                //DataTable HoCompData = _ItemList_ISERVICES.GetItemListFilterDAL(Comp_ID, ItemID, ItemGrpID, AttrName, AttrValue, ActStatus, ItmStatus, ItemPrfID).Tables[0];
                //ViewBag.VBItemlist = HoCompData;
                string Fstring = string.Empty;
                Fstring = Comp_ID + "," + ItemID + "," + ItemGrpID + "," + AttrName + "," + AttrValue + "," + ActStatus + "," + ItmStatus + "," + ItemPrfID + "," + ImageFilter;
                //Session["ILSearch"] = "IL_Search";
                _ItemDetail.ILSearch = "IL_Search";
                //Session["IL_SSearch"] = Fstring;
                _ItemDetail.IL_SSearch = Fstring;
                _ItemDetail.ItemListFilter = Fstring;
                ViewBag.listdata = Fstring;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }

            return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialItemList.cshtml", _ItemDetail);
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
        public ActionResult GetAutoCompleteItemList(SearchSupp queryParameters)
        {
            string Comp_ID = string.Empty;
            string BrchID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            string GroupName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ddlGroup;
                }
                ItemList = _ItemList_ISERVICES.ItemSetupGroupDAL(GroupName, Comp_ID, BrchID);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteItemGrpList(SearchSupp queryParameters)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string GroupName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> ItemList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ddlGroup;
                }
                ItemList = _ItemList_ISERVICES.ItemGroupListDAL(GroupName, Comp_ID);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteItemPortfList(SearchSupp queryParameters)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string GroupName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> ItemList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ddlGroup;
                }
                ItemList = _ItemList_ISERVICES.ItemPortfolioList(GroupName, Comp_ID);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemListDetails()
        {
            JsonResult DataRows = null;
            string Comp_ID = string.Empty;
            //string BrchID = string.Empty;
            string User_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            //if (Session["BranchId"] != null)
            //{
            //    BrchID = Session["BranchId"].ToString();
            //}
            if (Session["userid"] != null)
            {
                User_ID = Session["userid"].ToString();
            }
            //string GroupName = string.Empty;
            ////string ErrorMessage = "success";
            //Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                //if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                //{
                //    GroupName = "0";
                //}
                //else
                //{
                //    GroupName = queryParameters.ddlGroup;
                //}
                DataSet dset = new DataSet();
                dset = _ItemList_ISERVICES.GetItemListDAL(Comp_ID, User_ID);
                DataRows = Json(JsonConvert.SerializeObject(dset));
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
        public JsonResult GetAttributeName()
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet HoCompData = _ItemList_ISERVICES.GetAttributeListDAL(Comp_ID);
                DataRows = Json(JsonConvert.SerializeObject(HoCompData));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetItemsAvlStk(string itemId)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet HoCompData = _ItemList_ISERVICES.GetItemsAvailableStock(Comp_ID, BrchID, itemId);
                DataRows = Json(JsonConvert.SerializeObject(HoCompData));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetAttributeValue(string AttributeID)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet HoCompData = _ItemList_ISERVICES.GetAttributeValueDAL(Comp_ID, AttributeID);
                DataRows = Json(JsonConvert.SerializeObject(HoCompData));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetItemImageList(string ItmCode)
        {
            JsonResult DataRows = null;
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            try
            {
                //ItemSetupHelper = new ItemSetupHelper();
                DataSet HoCompData = _ItemList_ISERVICES.GetItemImageListDAL(Comp_ID, ItmCode);
                DataRows = Json(JsonConvert.SerializeObject(HoCompData));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetItem_OrdersDetails(string ItmCode)
        {
            JsonResult DataRows = null;
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            try
            {
                DataSet HoCompData = _ItemList_ISERVICES.GetItem_OrdersDetails(Comp_ID, ItmCode);
                DataRows = Json(JsonConvert.SerializeObject(HoCompData));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult AddNewItem()
        {
            ItemDetailModel _ItemDetail = new ItemDetailModel();
            //@Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = "D";
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";

            _ItemDetail.Message = "New";
            _ItemDetail.Command = "Add";
            _ItemDetail.AppStatus = "D";
            _ItemDetail.TransType = "Save";
            _ItemDetail.BtnName = "BtnAddNew";
            TempData["ModelData"] = _ItemDetail;
            return RedirectToAction("ItemDetail", "ItemDetail");
        }
        public ActionResult EditItem(string ItemId, string ListFilterData)
        {
            ItemDetailModel _ItemDetail = new ItemDetailModel();
            //_ItemDetail.Message = "New";
            //Session["Command"] = "Add";
            //Session["ItemCode"] = ItemId;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = "D";
            //Session["BtnName"] = "BtnToDetailPage";

            _ItemDetail.Message = "New";
            _ItemDetail.Command = "Add";
            _ItemDetail.ItemCode = ItemId;
            _ItemDetail.TransType = "Update";
            _ItemDetail.AppStatus = "D";
            _ItemDetail.BtnName = "BtnToDetailPage";
            var ItemCodeURL = ItemId;
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var command = "Add";
            TempData["ModelData"] = _ItemDetail;
            TempData["ListFilterData"] = ListFilterData;
            return (RedirectToAction("ItemDetail", "ItemDetail", new { ItemCodeURL = ItemCodeURL, TransType, BtnName, command }));
        }
        //[HttpPost]
        //public ActionResult GetItemList()
        //{
        //    JsonResult DataRows = null;
        //    string PoItmName = string.Empty;
        //    string Comp_ID = string.Empty;
        //    string User_ID = string.Empty;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            User_ID = Session["userid"].ToString();
        //        }

        //        DataSet DSet = _ItemList_ISERVICES.GetItemListDAL(Comp_ID, User_ID);
        //        //DataRows = Json(JsonConvert.SerializeObject(DSet.Tables[0]));/*Result convert into Json Format for javasript*/
        //        DataRows = Json(JsonConvert.SerializeObject(DSet.Tables[0]), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return DataRows;
        //}
        public ActionResult LoadData(string ItemListFilter)
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

                _ItemListModel = new List<ItemListModel>();

                _ItemListModel = getItemList(ItemListFilter);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                }
                //Search
                ItemListData = FilteredList(ItemListData, searchValue);
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    ItemListData = ItemListData.Where(m => m.ItemName.ToUpper().Contains(searchValue) || m.ItemID.ToUpper().Contains(searchValue)
                //    || m.Group.ToUpper().Contains(searchValue) || m.OEMNo.ToUpper().Contains(searchValue) || m.SampleCode.ToUpper().Contains(searchValue)
                //     || m.Item_leg_cd.ToUpper().Contains(searchValue) || m.Item_cat_no.ToUpper().Contains(searchValue)
                //    || m.Status.ToUpper().Contains(searchValue) || m.ItemCost.ToUpper().Contains(searchValue) || m.ItemPrice.ToUpper().Contains(searchValue)
                //    || m.Createdon.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue) || m.Active.ToUpper().Contains(searchValue) ||
                //    m.Image.ToUpper().Contains(searchValue)
                //    || m.AmendedOn.ToUpper().Contains(searchValue) || m.ApprovedOn.ToUpper().Contains(searchValue));
                //}

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
        public FileResult ExportData(string searchValue, string ItemListFilter)
        {
            try
            {
                //var draw = Request.Form.GetValues("draw").FirstOrDefault();
                //var start = Request.Form.GetValues("start").FirstOrDefault();
                //var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = "ItemName";// Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = "asc";// Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                //var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToUpper();

                //Paging Size (10,20,50,100)    
                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
                //int skip = start != null ? Convert.ToInt32(start) : 0;
                //int recordsTotal = 0;

                _ItemListModel = new List<ItemListModel>();

                _ItemListModel = getItemList(ItemListFilter);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                }
                //Search
                ItemListData = FilteredList(ItemListData, searchValue);
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    searchValue = searchValue.ToUpper();
                //    ItemListData = ItemListData.Where(m => m.ItemName.ToUpper().Contains(searchValue) || m.ItemID.ToUpper().Contains(searchValue)
                //    || m.Group.ToUpper().Contains(searchValue) || m.OEMNo.ToUpper().Contains(searchValue) || m.SampleCode.ToUpper().Contains(searchValue)
                //    || m.Item_leg_cd.ToUpper().Contains(searchValue) || m.Item_cat_no.ToUpper().Contains(searchValue)
                //    || m.Status.ToUpper().Contains(searchValue) || m.ItemCost.ToUpper().Contains(searchValue) || m.ItemPrice.ToUpper().Contains(searchValue)
                //    || m.Createdon.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue) || m.Active.ToUpper().Contains(searchValue) ||
                //    m.Image.ToUpper().Contains(searchValue)
                //    || m.AmendedOn.ToUpper().Contains(searchValue) || m.ApprovedOn.ToUpper().Contains(searchValue));
                //}

                //total number of rows count     
                //recordsTotal = ItemListData.Count();
                //Paging     
                var data = ItemListData.ToList();
                //var data = ItemListData.Take(pageSize).ToList();
                //Returning Json Data    
                //return Json(new {data = data });

                DataTable dt = ToItemListExl(data);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Item Detail", dt);


            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        private IEnumerable<ItemListModel> FilteredList(IEnumerable<ItemListModel> ItemListData, string searchValue)/* Added by Suraj Maurya on 04-10-2025 for filter */
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToUpper();
                ItemListData = ItemListData.Where(m => m.ItemName.ToUpper().Contains(searchValue) || m.ItemID.ToUpper().Contains(searchValue)
                        || m.Group.ToUpper().Contains(searchValue) || m.OEMNo.ToUpper().Contains(searchValue) || m.SampleCode.ToUpper().Contains(searchValue)
                         || m.Item_leg_cd.ToUpper().Contains(searchValue) || m.Item_cat_no.ToUpper().Contains(searchValue)
                        || m.Status.ToUpper().Contains(searchValue) || m.ItemCost.ToUpper().Contains(searchValue) || m.ItemPrice.ToUpper().Contains(searchValue)
                        || m.Createdon.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue) || m.Active.ToUpper().Contains(searchValue) ||
                        m.Image.ToUpper().Contains(searchValue)
                        || m.AmendedOn.ToUpper().Contains(searchValue) || m.ApprovedOn.ToUpper().Contains(searchValue));
            }

            return ItemListData;
        }
        [HttpPost]
        public ActionResult ExportCsv([System.Web.Http.FromBody] DataTableRequest request) /* Added by Suraj Maurya on 04-10-2025 for Csv with Dynamic Columns */
        {
            string keyword = "";
            // 🔹 Fetch data same as LoadData but ignore paging
            var query = getItemList(request.ItemListFilter);

            // Apply search filter
            if (!string.IsNullOrEmpty(request.search?.value))
            {
                keyword = request.search.value.ToLower();
            }
            IEnumerable<ItemListModel> _ItemListModelList = FilteredList(query, keyword);

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

        public DataTable ToItemListExl(List<ItemListModel> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr No.", typeof(int));
            dataTable.Columns.Add("Item Name", typeof(string));
            //dataTable.Columns.Add("Item ID", typeof(string));
            dataTable.Columns.Add("Group", typeof(string));
            dataTable.Columns.Add("UOM", typeof(string));
            dataTable.Columns.Add("OEM No.", typeof(string));
            dataTable.Columns.Add("Sample Code", typeof(string));
            dataTable.Columns.Add("Ref. No.", typeof(string));
            dataTable.Columns.Add("Catalogue Number", typeof(string));
            dataTable.Columns.Add("Active", typeof(string));
            dataTable.Columns.Add("Image", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("Item Cost", typeof(decimal));
            dataTable.Columns.Add("Item Price", typeof(decimal));
            dataTable.Columns.Add("Created By", typeof(string));
            dataTable.Columns.Add("Created On", typeof(string));
            dataTable.Columns.Add("Approved By", typeof(string));
            dataTable.Columns.Add("Approved On", typeof(string));
            dataTable.Columns.Add("Amended By", typeof(string));
            dataTable.Columns.Add("Amended On", typeof(string));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr No."] = item.SrNo;
                rows["Item Name"] = item.ItemName;
                //rows["Item ID"] = item.ItemID;
                rows["Group"] = item.Group;
                rows["UOM"] = item.UOM;
                rows["OEM No."] = item.OEMNo;
                rows["Sample Code"] = item.SampleCode;
                rows["Ref. No."] = item.Item_leg_cd;
                rows["Catalogue Number"] = item.Item_cat_no;
                rows["Active"] = item.Active;
                rows["Image"] = item.Image;
                rows["Status"] = item.Status;
                rows["Item Cost"] = item.ItemCost;
                rows["Item Price"] = item.ItemPrice;
                rows["Created By"] = item.CreatedBy;
                rows["Created On"] = item.Createdon;
                rows["Approved By"] = item.ApprovedBy;
                rows["Approved On"] = item.ApprovedOn;
                rows["Amended By"] = item.AmmendedBy;
                rows["Amended On"] = item.AmendedOn;

                dataTable.Rows.Add(rows);
            }

            return dataTable;
        }
        private List<ItemListModel> getItemList(string ItemListFilter)
        {
            _ItemListModel = new List<ItemListModel>();
            try
            {
                string User_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
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
                        fdata = Fstring.Split(',');

                        string ItemID, ItemGrpID, AttrName, AttrValue, ActStatus, ItmStatus, ItemPrfID, Imagefilter = string.Empty;
                        ItemID = fdata[1];
                        ItemGrpID = fdata[2];
                        AttrName = fdata[3];
                        AttrValue = fdata[4];
                        ActStatus = fdata[5];
                        ItmStatus = fdata[6];
                        ItemPrfID = fdata[7];
                        Imagefilter = fdata[8];

                        DSet = _ItemList_ISERVICES.GetItemListFilterDAL(CompID, ItemID, ItemGrpID, AttrName, AttrValue, ActStatus, ItmStatus, ItemPrfID, Imagefilter);
                    }
                    else
                    {
                        DSet = _ItemList_ISERVICES.GetItemListDAL(CompID, User_ID);
                        //Session.Remove("ILSearch");
                        //Session.Remove("IL_SSearch");
                    }
                }
                else
                {
                    DSet = _ItemList_ISERVICES.GetItemListDAL(CompID, User_ID);
                    //Session.Remove("ILSearch");
                    //Session.Remove("IL_SSearch");
                }
                ////if (Session["ILSearch"]!=null)
                //if (Session["ILSearch"]!=null)
                //{
                //    if (Session["ILSearch"].ToString() != "")
                //    {
                //        if (Session["IL_SSearch"] != null)
                //        {
                //            if (Session["IL_SSearch"].ToString() != "")
                //            {
                //                string Fstring = string.Empty;
                //                string[] fdata;
                //                Fstring = Session["IL_SSearch"].ToString();
                //                fdata = Fstring.Split(',');

                //                string ItemID, ItemGrpID, AttrName, AttrValue, ActStatus, ItmStatus, ItemPrfID = string.Empty;
                //                ItemID = fdata[1];
                //                ItemGrpID = fdata[2];
                //                AttrName = fdata[3];
                //                AttrValue = fdata[4];
                //                ActStatus = fdata[5];
                //                ItmStatus = fdata[6];
                //                ItemPrfID = fdata[7];

                //                DSet = _ItemList_ISERVICES.GetItemListFilterDAL(CompID, ItemID, ItemGrpID, AttrName, AttrValue, ActStatus, ItmStatus, ItemPrfID);

                //                Session.Remove("ILSearch");
                //                Session.Remove("IL_SSearch");
                //            }
                //            else
                //            {
                //                DSet = _ItemList_ISERVICES.GetItemListDAL(CompID, User_ID);
                //                Session.Remove("ILSearch");
                //                Session.Remove("IL_SSearch");
                //            }
                //        }
                //        else
                //        {
                //            DSet = _ItemList_ISERVICES.GetItemListDAL(CompID, User_ID);
                //            Session.Remove("ILSearch");
                //            Session.Remove("IL_SSearch");
                //        }
                //    }
                //    else
                //    {
                //        DSet = _ItemList_ISERVICES.GetItemListDAL(CompID, User_ID);
                //        Session.Remove("ILSearch");
                //        Session.Remove("IL_SSearch");
                //    }
                //}
                //else
                //{
                //    DSet = _ItemList_ISERVICES.GetItemListDAL(CompID, User_ID);
                //    Session.Remove("ILSearch");
                //    Session.Remove("IL_SSearch");
                //}


                if (DSet.Tables[0].Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ItemListModel _Item_List = new ItemListModel();
                        _Item_List.SrNo = rowno + 1;
                        _Item_List.ItemName = dr["Item_name"].ToString();
                        _Item_List.ItemID = dr["Item_id"].ToString();
                        _Item_List.Group = dr["item_group_name"].ToString();
                        _Item_List.UOM = dr["uom_name"].ToString();
                        _Item_List.OEMNo = dr["item_oem_no"].ToString();
                        _Item_List.SampleCode = dr["item_sam_cd"].ToString();
                        _Item_List.Item_leg_cd = dr["item_leg_cd"].ToString();
                        _Item_List.Item_cat_no = dr["item_cat_no"].ToString();
                        _Item_List.Active = dr["act_stat"].ToString();
                        _Item_List.Image = dr["imgflag"].ToString();
                        _Item_List.Status = dr["App_Status"].ToString();
                        _Item_List.ItemCost = dr["cost_price"].ToString();
                        _Item_List.ItemPrice = dr["sale_price"].ToString();
                        _Item_List.CreatedBy = dr["create_by"].ToString();
                        _Item_List.Createdon = dr["create_dt"].ToString();
                        _Item_List.ApprovedBy = dr["app_by"].ToString();
                        _Item_List.ApprovedOn = dr["app_dt"].ToString();
                        _Item_List.AmmendedBy = dr["mod_by"].ToString();
                        _Item_List.AmendedOn = dr["mod_dt"].ToString();

                        _ItemListModel.Add(_Item_List);
                        rowno = rowno + 1;
                    }
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
        public ActionResult DownloadFile(string reference_noflag)
        {
            try
            {
                string compId = "0";
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                DataTable ItemDetail = new DataTable();
                DataTable SubItem = new DataTable();
                DataTable Itematt = new DataTable();
                DataTable BrDetail = new DataTable();
                DataTable PortDetail = new DataTable();
                DataSet obj_ds = new DataSet();
                ItemDetail.Columns.Add("Item Description*(max 100 characters)", typeof(string));
                ItemDetail.Columns.Add("OEM No (max 150 characters)", typeof(string));
                ItemDetail.Columns.Add("Alias Name(max 250 characters)", typeof(string));
                if(reference_noflag == "N")
                {
                    ItemDetail.Columns.Add("Ref No (max 25 characters)", typeof(string));
                }
                else
                {
                    ItemDetail.Columns.Add("Ref No*(max 25 characters)", typeof(string));
                }
               
                ItemDetail.Columns.Add("Technical Specification(max 2000 characters)", typeof(string));
                ItemDetail.Columns.Add("Technical Description(max 2000 characters)", typeof(string));
                ItemDetail.Columns.Add("Items Group*", typeof(string));
                ItemDetail.Columns.Add("HSN Code*", typeof(string));
                ItemDetail.Columns.Add("Base UOM*", typeof(string));
                ItemDetail.Columns.Add("Unit Cost(Numeric only)", typeof(string));
                ItemDetail.Columns.Add("Unit Price(Numeric only)", typeof(string));
                ItemDetail.Columns.Add("Issue Method*", typeof(string));
                ItemDetail.Columns.Add("Costing Method", typeof(string));
                ItemDetail.Columns.Add("Minimum Stock Level(Numeric only)", typeof(string));
                ItemDetail.Columns.Add("Minimum Purchase Stock(Numeric only)", typeof(string));
                ItemDetail.Columns.Add("Reorder Level(Numeric only)", typeof(string));
                ItemDetail.Columns.Add("Weight In Kg(Numeric only)", typeof(string));
                ItemDetail.Columns.Add("Volume In Litres(Numeric only)", typeof(string));
                ItemDetail.Columns.Add("Remarks(max 200 characters)", typeof(string));
                ItemDetail.Columns.Add("Workstation", typeof(string));
                ItemDetail.Columns.Add("Tax Exempted", typeof(string));
                ItemDetail.Columns.Add("Sub-Item", typeof(string));

                SubItem.Columns.Add("Item Name", typeof(string));
                SubItem.Columns.Add("Sub-Item Name(max 50 characters)", typeof(string));

                Itematt.Columns.Add("Item Name *", typeof(string));
                Itematt.Columns.Add("Attribute Name", typeof(string));
                Itematt.Columns.Add("Attribute Value", typeof(string));

                BrDetail.Columns.Add("Item Name", typeof(string));
                BrDetail.Columns.Add("Branch Name", typeof(string));
                BrDetail.Columns.Add("Active", typeof(string));

                PortDetail.Columns.Add("Item Name", typeof(string));
                PortDetail.Columns.Add("Portfolio Name", typeof(string));
                PortDetail.Columns.Add("Portfolio Description(max 50 characters)", typeof(string));
                PortDetail.Columns.Add("Remarks(max 50 characters)", typeof(string));

                obj_ds.Tables.Add(ItemDetail);
                obj_ds.Tables.Add(SubItem);
                obj_ds.Tables.Add(Itematt);
                obj_ds.Tables.Add(BrDetail);
                obj_ds.Tables.Add(PortDetail);

                DataSet ds = _ItemList_ISERVICES.GetMasterDataForExcelFormat(compId);
                CommonController com_obj = new CommonController();
                string filePath = com_obj.CreateExcelFile("ImportItemTemplate", Server);
                com_obj.AppendExcel(filePath, ds, obj_ds, "ItemSetup");
                string fileName = Path.GetFileName(filePath);
                return File(filePath, "application/octet-stream", fileName);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ValidateExcelFile(string uploadStatus)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            conString = "Invalid File";
                            break;
                    }
                    if (conString == "Invalid File")
                        return Json("Invalid File. Please upload a valid file", JsonRequestBehavior.AllowGet);
                    DataSet ds = new DataSet();
                    DataTable dtItems = new DataTable();
                    DataTable dtSubItems = new DataTable();
                    DataTable dtItemAttributes = new DataTable();
                    DataTable dtItemBranch = new DataTable();
                    DataTable dtItemPortfolio = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();

                                string itemsQuery = "SELECT DISTINCT * FROM [ItemsDetail$] WHERE LEN([Item Description*(max 100 characters)]) > 0;";
                                string subItemsQuery = "SELECT DISTINCT * From [SubItemDetail$] ; ";
                                string itemAttributesQuery = "SELECT DISTINCT * From [ItemAttribute$] ; ";
                                string itemBranchQuery = "SELECT DISTINCT * From [BranchMapping$] ; ";
                                string itemPortfolioQuery = "SELECT DISTINCT * From [ItemPortfolio$] WHERE LEN([Item Name]) > 0;";

                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = itemsQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItems);

                                cmdExcel.CommandText = subItemsQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtSubItems);

                                cmdExcel.CommandText = itemAttributesQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemAttributes);
                                connExcel.Close();

                                cmdExcel.CommandText = itemBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemBranch);
                                connExcel.Close();

                                cmdExcel.CommandText = itemPortfolioQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemPortfolio);
                                connExcel.Close();

                                ds.Tables.Add(dtItems);
                                ds.Tables.Add(dtSubItems);
                                ds.Tables.Add(dtItemAttributes);
                                ds.Tables.Add(dtItemBranch);
                                ds.Tables.Add(dtItemPortfolio);
                                DataSet dts = VerifyData(ds, uploadStatus);
                                if (dts == null)
                                    return Json("Excel file is empty. Please fill data in excel file and try again");
                                ViewBag.ImportItemsPreview = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialImportItemsDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult ImportItemsDetailFromExcel()
        {
            try
            {
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable dtItems = new DataTable();
                    DataTable dtSubItems = new DataTable();
                    DataTable dtItemAttributes = new DataTable();
                    DataTable dtItemBranch = new DataTable();
                    DataTable dtItemportfilo = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                string sheetItems = "", sheetSubItems = "", sheetAttributes = "";
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string itemsQuery = "SELECT DISTINCT * FROM [ItemsDetail$] WHERE LEN([Item Description*(max 100 characters)]) > 0;";
                                string subItemsQuery = "SELECT DISTINCT * From [SubItemDetail$] ; ";
                                string itemAttributesQuery = "SELECT DISTINCT * From [ItemAttribute$] ; ";
                                string itemBranchQuery = "SELECT DISTINCT * From [BranchMapping$] ; ";
                                string itemPortfolioQuery = "SELECT DISTINCT * From [ItemPortfolio$] WHERE LEN([Item Name]) > 0;";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = itemsQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItems);

                                cmdExcel.CommandText = subItemsQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtSubItems);

                                cmdExcel.CommandText = itemAttributesQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemAttributes);
                                connExcel.Close();

                                cmdExcel.CommandText = itemBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemBranch);
                                connExcel.Close();

                                cmdExcel.CommandText = itemPortfolioQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemportfilo);
                                connExcel.Close();

                                ds.Tables.Add(dtItems);
                                ds.Tables.Add(dtSubItems);
                                ds.Tables.Add(dtItemAttributes);
                                ds.Tables.Add(dtItemBranch);
                                ds.Tables.Add(dtItemportfilo);
                                string msg = SaveItemsFromExcel(ds);
                                return Json(msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                }
                else
                    return Json("No file selected", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("cannot insert duplicate"))
                    return Json("Duplicate items found in excel file ", JsonRequestBehavior.AllowGet);
                else
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public DataSet PrepareDataset(DataSet dsItems)
        {
            try
            {
                string compId = "", brId = "", userId = "";
                if (Session["compid"] != null)
                    compId = Session["compid"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();

                DataTable dtItems = new DataTable();
                DataTable dtSubItems = new DataTable();
                DataTable dtAttributes = new DataTable();
                DataTable dtBranch = new DataTable();
                DataTable dtportfolio = new DataTable();
                //Items Details//
                dtItems.Columns.Add("item_name", typeof(string));
                dtItems.Columns.Add("item_oem_no", typeof(string));
                dtItems.Columns.Add("item_sam_cd", typeof(string));
                dtItems.Columns.Add("item_leg_cd", typeof(string));
                dtItems.Columns.Add("item_tech_spec", typeof(string));
                dtItems.Columns.Add("item_tech_des", typeof(string));
                dtItems.Columns.Add("item_grp_id", typeof(string));
                dtItems.Columns.Add("HSN_code", typeof(string));
                dtItems.Columns.Add("base_uom_id", typeof(string));
                dtItems.Columns.Add("cost_price", typeof(string));
                dtItems.Columns.Add("sale_price", typeof(string));
                dtItems.Columns.Add("issue_method", typeof(string));
                dtItems.Columns.Add("cost_method", typeof(string));
                dtItems.Columns.Add("min_stk_lvl", typeof(string));
                dtItems.Columns.Add("min_pr_stk", typeof(string));
                dtItems.Columns.Add("re_ord_lvl", typeof(string));
                dtItems.Columns.Add("wght_kg", typeof(string));
                dtItems.Columns.Add("wght_ltr", typeof(string));
                dtItems.Columns.Add("item_remarks", typeof(string));
                dtItems.Columns.Add("i_ws", typeof(string));
                dtItems.Columns.Add("tax_exemp", typeof(string));
                dtItems.Columns.Add("sub_item", typeof(string));
                for (int i = 0; i < dsItems.Tables[0].Rows.Count; i++)
                {
                    DataTable dtItem = dsItems.Tables[0];
                    DataRow dtr = dtItems.NewRow();
                    //dtr["item_name"] = dtItem.Rows[i][0].ToString().Trim();
                    dtr["item_name"] = dtItem.Rows[i][0].ToString().Trim().Replace(",", " ").Replace("_", " ");
                    dtr["item_oem_no"] = dtItem.Rows[i][1].ToString().Trim();
                    dtr["item_sam_cd"] = dtItem.Rows[i][2].ToString().Trim();
                    dtr["item_leg_cd"] = dtItem.Rows[i][3].ToString().Trim();
                    dtr["item_tech_spec"] = dtItem.Rows[i][4].ToString().Trim();
                    dtr["item_tech_des"] = dtItem.Rows[i][5].ToString().Trim();
                    dtr["item_grp_id"] = dtItem.Rows[i][6].ToString().Trim();
                    dtr["HSN_code"] = dtItem.Rows[i][7].ToString().Trim();
                    dtr["base_uom_id"] = dtItem.Rows[i][8].ToString().Trim();

                    double costPrice = 0;
                    double.TryParse(dtItem.Rows[i][9].ToString().Trim(), out costPrice);
                    dtr["cost_price"] = costPrice.ToString();

                    double salePrice = 0;
                    double.TryParse(dtItem.Rows[i][10].ToString().Trim(), out salePrice);
                    dtr["sale_price"] = salePrice.ToString();

                    dtr["issue_method"] = dtItem.Rows[i][11].ToString().Trim();
                    dtr["cost_method"] = dtItem.Rows[i][12].ToString().Trim();

                    double minStk = 0;
                    double.TryParse(dtItem.Rows[i][13].ToString().Trim(), out minStk);
                    dtr["min_stk_lvl"] = minStk.ToString();
                    double minPrStk = 0;
                    double.TryParse(dtItem.Rows[i][14].ToString().Trim(), out minPrStk);
                    dtr["min_pr_stk"] = minPrStk.ToString().Trim();

                    double reOrdLvl = 0;
                    double wghtkg = 0;
                    double wghtltr = 0;
                    double.TryParse(dtItem.Rows[i][15].ToString().Trim(), out reOrdLvl);
                    dtr["re_ord_lvl"] = reOrdLvl.ToString();
                    double.TryParse(dtItem.Rows[i][16].ToString().Trim(), out wghtkg);
                    dtr["wght_kg"] = wghtkg.ToString();
                    double.TryParse(dtItem.Rows[i][17].ToString().Trim(), out wghtltr);
                    dtr["wght_ltr"] = wghtltr.ToString();
                    dtr["item_remarks"] = dtItem.Rows[i][18].ToString().Trim();
                    dtr["i_ws"] = dtItem.Rows[i][19].ToString().Trim();
                    dtr["tax_exemp"] = dtItem.Rows[i][20].ToString().Trim();
                    dtr["sub_item"] = dtItem.Rows[i][21].ToString().Trim();
                    dtItems.Rows.Add(dtr);
                }
                //Sub Items Details//
                dtSubItems.Columns.Add("item_name", typeof(string));
                dtSubItems.Columns.Add("sub_item_name", typeof(string));
                for (int i = 0; i < dsItems.Tables[1].Rows.Count; i++)
                {
                    DataTable dtSubItm = dsItems.Tables[1];
                    DataRow dtr = dtSubItems.NewRow();
                    dtr["item_name"] = dtSubItm.Rows[i][0].ToString();
                    dtr["sub_item_name"] = dtSubItm.Rows[i][1].ToString();
                    dtSubItems.Rows.Add(dtr);
                }
                dtAttributes.Columns.Add("item_name", typeof(string));
                dtAttributes.Columns.Add("item_attr_name", typeof(string));
                dtAttributes.Columns.Add("item_attr_Val", typeof(string));
                for (int k = 0; k < dsItems.Tables[2].Rows.Count; k++)
                {
                    DataTable dtAttr = dsItems.Tables[2];
                    DataRow dtr = dtAttributes.NewRow();
                    dtr["item_name"] = dtAttr.Rows[k][0].ToString();
                    dtr["item_attr_name"] = dtAttr.Rows[k][1].ToString();
                    dtr["item_attr_Val"] = dtAttr.Rows[k][2].ToString();
                    dtAttributes.Rows.Add(dtr);
                }
                //-------------------------------Branch Detail-------------------------
                dtBranch.Columns.Add("item_name", typeof(string));
                dtBranch.Columns.Add("branch_name", typeof(string));
                dtBranch.Columns.Add("act_status", typeof(string));
                for (int i = 0; i < dsItems.Tables[3].Rows.Count; i++)
                {
                    DataTable dtbranchdetail = dsItems.Tables[3];
                    DataRow dtr = dtBranch.NewRow();
                    dtr["item_name"] = dtbranchdetail.Rows[i][0].ToString().Trim();
                    dtr["branch_name"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                    if (!string.IsNullOrEmpty(dtbranchdetail.Rows[i][2].ToString()))
                    {
                        dtr["act_status"] = dtbranchdetail.Rows[i][2].ToString().Trim().Substring(0, 1);
                    }
                    else
                    {
                        dtr["act_status"] = "N";
                    }
                    dtBranch.Rows.Add(dtr);
                }
                //---------------------------End-------------------------------------
                //-------------------------------Portfolio Detail-------------------------
                dtportfolio.Columns.Add("item_name", typeof(string));
                dtportfolio.Columns.Add("prf_name", typeof(string));
                dtportfolio.Columns.Add("prf_des", typeof(string));
                dtportfolio.Columns.Add("remarks", typeof(string));
                for (int i = 0; i < dsItems.Tables[4].Rows.Count; i++)
                {
                    DataTable dtportdetail = dsItems.Tables[4];
                    DataRow dtr = dtportfolio.NewRow();
                    dtr["item_name"] = dtportdetail.Rows[i][0].ToString().Trim();
                    dtr["prf_name"] = dtportdetail.Rows[i][1].ToString().Trim();
                    dtr["prf_des"] = dtportdetail.Rows[i][2].ToString().Trim();
                    dtr["remarks"] = dtportdetail.Rows[i][3].ToString().Trim();
                    dtportfolio.Rows.Add(dtr);
                }
                //---------------------------End-------------------------------------
                DataSet dts = new DataSet();

                dts.Tables.Add(dtItems);
                dts.Tables.Add(dtSubItems);
                dts.Tables.Add(dtAttributes);
                dts.Tables.Add(dtBranch);
                dts.Tables.Add(dtportfolio);
                return dts;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private string SaveItemsFromExcel(DataSet dsItems)
        {
            try
            {
                string compId = "";
                string BranchName = "";
                if (Session["compid"] != null)
                    compId = Session["compid"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                if (Session["userid"] != null)
                    UserId = Session["userid"].ToString();
                if (Session["BranchName"] != null)
                    BranchName = Session["BranchName"].ToString();
                DataSet dts = PrepareDataset(dsItems);
                string result = _ItemList_ISERVICES.BulkImportItemsDetail(compId, BrchID, dts.Tables[0], dts.Tables[1], dts.Tables[2], dts.Tables[3], dts.Tables[4], BranchName, UserId);
                return result;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataSet VerifyData(DataSet dsItems, string uploadStatus)
        {
            try
            {
                string compId = "";
                if (Session["compid"] != null)
                    compId = Session["compid"].ToString();
                DataSet dts = PrepareDataset(dsItems);
                if (dsItems.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsItems.Tables[0].Rows[0].ToString()))
                {
                    DataSet result = _ItemList_ISERVICES.GetVerifiedDataOfExcel(compId, dts.Tables[0], dts.Tables[1], dts.Tables[2], dts.Tables[3], dts.Tables[4]);
                    if (uploadStatus.Trim() == "0")
                        return result;

                    var filteredRows = result.Tables[0].AsEnumerable().Where(x => x.Field<string>("UploadStatus").ToUpper() == uploadStatus.ToUpper()).ToList();
                    DataTable newDataTable = filteredRows.Any() ? filteredRows.CopyToDataTable() : result.Tables[0].Clone();
                    result.Tables[0].Clear();

                    for (int i = 0; i < newDataTable.Rows.Count; i++)
                    {
                        result.Tables[0].ImportRow(newDataTable.Rows[i]);
                    }
                    result.Tables[0].AcceptChanges();
                    return result;
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
                throw ex;
            }
        }
        //Import data end//
        //Show Validation Error By ItemWise//
        public ActionResult ShowValidationError(string itemName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable dtItems = new DataTable();
                    DataTable dtSubItems = new DataTable();
                    DataTable dtItemAttributes = new DataTable();
                    DataTable dtItemBranch = new DataTable();
                    DataTable dtItemPortfolio = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                // sheetItems = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                // sheetSubItems = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
                                // sheetAttributes = dtExcelSchema.Rows[2]["TABLE_NAME"].ToString();
                                connExcel.Close();
                                string itemsQuery = "";
                                //string itemsQuery = "SELECT DISTINCT * From [ItemsDetail$] where [Item Description*_ (max 100 characters)] = '" + itemName + "' ; ";
                                if (string.IsNullOrWhiteSpace(itemName))
                                {
                                    itemsQuery = "SELECT DISTINCT * FROM [ItemsDetail$] WHERE [Item Description*(max 100 characters)] IS NULL OR LTRIM(RTRIM([Item Description*(max 100 characters)])) = '';";
                                }
                                else
                                {
                                     itemsQuery = "SELECT DISTINCT * FROM [ItemsDetail$] WHERE [Item Description*(max 100 characters)] = '" + itemName + "';";
                                }
                                string subItemsQuery = "SELECT DISTINCT * From [SubItemDetail$] where [Item Name] = '" + itemName + "' ; ";
                                string itemAttributesQuery = "SELECT DISTINCT * From [ItemAttribute$] where [Item Name *] = '" + itemName + "' ; ";
                                string ItemBranchQuery = "SELECT DISTINCT * From [BranchMapping$] where [Item Name] = '" + itemName + "' ; ";
                                string itemPortfolioQuery = "SELECT DISTINCT * From [ItemPortfolio$] where [Item Name] = '" + itemName + "' ; ";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = itemsQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItems);

                                cmdExcel.CommandText = subItemsQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtSubItems);

                                cmdExcel.CommandText = itemAttributesQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemAttributes);
                                connExcel.Close();

                                cmdExcel.CommandText = ItemBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemBranch);
                                connExcel.Close();

                                cmdExcel.CommandText = itemPortfolioQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemPortfolio);
                                connExcel.Close();

                                ds.Tables.Add(dtItems);
                                ds.Tables.Add(dtSubItems);
                                ds.Tables.Add(dtItemAttributes);
                                ds.Tables.Add(dtItemBranch); 
                                ds.Tables.Add(dtItemPortfolio);
                                DataTable dts = VerifySingleData(ds);
                                ViewBag.ErrorDetails = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialExportErrorDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataTable VerifySingleData(DataSet ds)
        {
            try
            {
                string compId = "";
                if (Session["compid"] != null)
                    compId = Session["compid"].ToString();
                DataSet dts = PrepareDataset(ds);
                DataTable result = _ItemList_ISERVICES.ShowExcelErrorDetail(compId, dts.Tables[0], dts.Tables[1], dts.Tables[2], dts.Tables[3], dts.Tables[4]);
                return result;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        //Validation Error End//
        //Bind Excel Sub Items//
        public ActionResult BindExcelSubItems(string itemName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataTable dtSubItems = new DataTable();
                    conString = string.Format(conString, filePath);
                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                // sheetItems = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                // sheetSubItems = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
                                // sheetAttributes = dtExcelSchema.Rows[2]["TABLE_NAME"].ToString();
                                connExcel.Close();
                                string subItemsQuery = "SELECT DISTINCT * From [SubItemDetail$] where [Item Name] = '" + itemName + "' ; ";

                                cmdExcel.CommandText = subItemsQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtSubItems);
                                DataTable dtSubItems1 = new DataTable();
                                dtSubItems1.Columns.Add("SrNo", typeof(int));
                                dtSubItems1.Columns.Add("sub_item_name", typeof(string));
                                for (int i = 0; i < dtSubItems.Rows.Count; i++)
                                {
                                    DataRow dtr = dtSubItems1.NewRow();
                                    dtr["SrNo"] = i + 1;
                                    dtr["sub_item_name"] = dtSubItems.Rows[i][1].ToString();
                                    dtSubItems1.Rows.Add(dtr);
                                }
                                ViewBag.SubItmDtl = dtSubItems1;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialExcelSubItem.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        //Excel Sub Item END//

        public ActionResult BindItemBranch(string itemName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();

                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": // Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": // Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            throw new Exception("Invalid file type");
                    }

                    DataTable dtItemdetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string itemQuery = "SELECT DISTINCT * From [BranchMapping$] where [Item Name] = '" + itemName + "' ; ";
                                cmdExcel.CommandText = itemQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemdetail);

                                DataTable ItemBranch = new DataTable();
                                ItemBranch.Columns.Add("item_name", typeof(string));
                                ItemBranch.Columns.Add("branch_name", typeof(string));
                                ItemBranch.Columns.Add("act_status", typeof(string));
                                for (int i = 0; i < dtItemdetail.Rows.Count; i++)
                                {
                                    DataTable dtbranchdetail = dtItemdetail;
                                    DataRow dtr = ItemBranch.NewRow();
                                    dtr["item_name"] = dtbranchdetail.Rows[i][0].ToString().Trim();
                                    dtr["branch_name"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                                    dtr["act_status"] = dtbranchdetail.Rows[i][2].ToString().Trim();
                                    ItemBranch.Rows.Add(dtr);
                                }
                                ViewBag.BranchDetail = ItemBranch;
                                ViewBag.PageName = "Item";
                            }
                        }
                    }
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialBranchMapping.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult BindItemPortfolio(string itemName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();

                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": // Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": // Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            throw new Exception("Invalid file type");
                    }

                    DataTable dtItemdport = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string PortfolioQuery = "SELECT DISTINCT * FROM [ItemPortfolio$] WHERE [Item Name] =  '" + itemName + "' AND LEN([Item Name]) > 0";

                                cmdExcel.CommandText = PortfolioQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItemdport);

                                DataTable ItemPort = new DataTable();
                                ItemPort.Columns.Add("prf_name", typeof(string));
                                ItemPort.Columns.Add("item_name", typeof(string));
                                ItemPort.Columns.Add("prf_des", typeof(string));
                                ItemPort.Columns.Add("remarks", typeof(string));
                                for (int i = 0; i < dtItemdport.Rows.Count; i++)
                                {
                                    DataTable dtbranchdetail = dtItemdport;
                                    DataRow dtr = ItemPort.NewRow();
                                    dtr["prf_name"] = dtbranchdetail.Rows[i][0].ToString().Trim();
                                    dtr["item_name"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                                    dtr["prf_des"] = dtbranchdetail.Rows[i][2].ToString().Trim();
                                    dtr["remarks"] = dtbranchdetail.Rows[i][3].ToString().Trim();
                                    ItemPort.Rows.Add(dtr);
                                }
                                ViewBag.PortfolioDetail = ItemPort;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialItemportfolio.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
    }
}