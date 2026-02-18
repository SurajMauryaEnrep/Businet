using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISStockReservation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISStockReservation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.StockReservation;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.MISStockReservation
{
    public class MISStockReservationController : Controller
    {
        string CompID, BrchID, language = String.Empty;
        string DocumentMenuId = "105102180130", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly MISStockReservation_IService _istockreservation;
        private readonly StockDetail_ISERVICE _IStockDetail;
        private readonly StockReservation_ISERVICES _StockReservation_ISERVICES;
        public MISStockReservationController(Common_IServices _Common_IServices, MISStockReservation_IService istockreservation,
            StockDetail_ISERVICE IStockDetail, StockReservation_ISERVICES StockReservation_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            _istockreservation = istockreservation;
            _IStockDetail = IStockDetail;
            _StockReservation_ISERVICES = StockReservation_ISERVICES;
        }
        // GET: ApplicationLayer/MISStockReservation
        public ActionResult MISStockReservation()
        {
            getDocumentName();
            ViewBag.MenuPageName = getDocumentName();
            MISStockReservation_Model objStkResv = new MISStockReservation_Model();
            objStkResv.title = title;
            objStkResv.ItemsList = GetItemsList();
            objStkResv.ItemsGroupList = GetItemsGroup();
            objStkResv.WarehouseList = GetWarehouseList();
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/MISStockReservation/MISStockReservation.cshtml", objStkResv);
        }
        public ActionResult SearchStockReservation(string itemId, string itemGroupId, string warehouseId)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            MISStockReservation_Model objStkResv = new MISStockReservation_Model();
            objStkResv.SearchStatus = "Searched";
            DataTable dt = _istockreservation.GetStockReservationReport(CompID, BrchID, itemId, itemGroupId, warehouseId);
            ViewBag.StockReservationData = dt;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/MISStockReservationList.cshtml", objStkResv);
        }
        public List<ItemGroupModel> GetItemsGroup()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            Dictionary<string, string> ItemGroupList = new Dictionary<string, string>();
            ItemGroupList = _IStockDetail.ItemGroupList("0", CompID);

            List<ItemGroupModel> _ItemGroupList = new List<ItemGroupModel>();
            //_ItemGroupList.Add(new ItemGroupModel { Group_Id = "0", Group_Name = "---ALL---" });
            foreach (var data in ItemGroupList)
            {
                ItemGroupModel _ItemGroupDetail = new ItemGroupModel();
                _ItemGroupDetail.Group_Id = data.Key;
                _ItemGroupDetail.Group_Name = data.Value;
                if (data.Key != "0" && data.Value != "")
                    _ItemGroupList.Add(_ItemGroupDetail);
            }
            return _ItemGroupList;
        }
        public List<ItemsModel> GetItemsList()
        {
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            ItemList = _IStockDetail.ItemList("0", CompID);

            List<ItemsModel> _ItemList = new List<ItemsModel>();
            _ItemList.Add(new ItemsModel { Item_Id = "0", Item_Name = "---ALL---" });
            foreach (var data in ItemList)
            {
                ItemsModel _ItemDetail = new ItemsModel();
                _ItemDetail.Item_Id = data.Key;
                _ItemDetail.Item_Name = data.Value;
                if (data.Key != "0")
                    _ItemList.Add(_ItemDetail);
            }
            return _ItemList;
        }
        public List<WarehouseModel> GetWarehouseList()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            DataSet Wh_List = _IStockDetail.GetWarehouseList(CompID, BrchID);

            DataRow Drow = Wh_List.Tables[0].NewRow();
            Drow[1] = "---ALL---";
            Drow[0] = "0";
           // Wh_List.Tables[0].Rows.InsertAt(Drow, 0);

            List<WarehouseModel> _Whlist = new List<WarehouseModel>();

            foreach (DataRow dr in Wh_List.Tables[0].Rows)
            {
                WarehouseModel _WhDetail = new WarehouseModel();
                _WhDetail.Wh_Id = dr["wh_id"].ToString();
                _WhDetail.Wh_Name = dr["wh_name"].ToString();
                _Whlist.Add(_WhDetail);
            }
            return _Whlist;
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
        public ActionResult GetReservedItemDetail(string ItemID, string wh_id, string flag, string entity_id, string DocNo, string SelectedItemLotBatchdetail)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _StockReservation_ISERVICES.GetReservedItemDetail(CompID, BrchID, ItemID, wh_id, flag, entity_id, DocNo);

                if (flag == "rev" || flag == "unrev")
                {
                    if (SelectedItemLotBatchdetail != null && SelectedItemLotBatchdetail != "")
                    {
                        JArray jObjectBatch = JArray.Parse(SelectedItemLotBatchdetail);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            foreach (JObject item in jObjectBatch.Children())
                            {
                                if (flag == "rev")
                                {
                                    if (item.GetValue("itemid").ToString().Trim() == ds.Tables[0].Rows[i]["itemid"].ToString().Trim() && item.GetValue("lotno").ToString().Trim() == ds.Tables[0].Rows[i]["lotno"].ToString().Trim() && item.GetValue("batchno").ToString().Trim() == ds.Tables[0].Rows[i]["batchno"].ToString().Trim() && item.GetValue("serialno").ToString().Trim() == ds.Tables[0].Rows[i]["serialno"].ToString().Trim())
                                    {
                                        ds.Tables[0].Rows[i]["res_qty"] = item.GetValue("res_unresQty");
                                    }
                                }
                                if (flag == "unrev")
                                {
                                    if (item.GetValue("docno").ToString().Trim() == ds.Tables[0].Rows[i]["srcno"].ToString().Trim() && item.GetValue("itemid").ToString().Trim() == ds.Tables[0].Rows[i]["itemid"].ToString().Trim() && item.GetValue("lotno").ToString().Trim() == ds.Tables[0].Rows[i]["lotno"].ToString().Trim() && item.GetValue("batchno").ToString().Trim() == ds.Tables[0].Rows[i]["batchno"].ToString().Trim() && item.GetValue("serialno").ToString().Trim() == ds.Tables[0].Rows[i]["serialno"].ToString().Trim())
                                    {
                                        ds.Tables[0].Rows[i]["unres_qty"] = item.GetValue("res_unresQty");
                                    }
                                }
                            }
                        }

                    }
                }
                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult MISStk_GetSubItemWhAvlstockDetails(string Wh_id, string Item_id, string flag, string UomId = null)
        {
            //JsonResult DataRows = null;
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
                DataSet ds = _StockReservation_ISERVICES.GetSubItemWhAvlstockDetails(Comp_ID, Br_ID, Wh_id, Item_id, UomId, flag);
                ViewBag.SubitemAvlStockDetail = ds.Tables[0];
                if (flag == "whres")
                {
                    ViewBag.Flag = "ReservStock";
                }
                else
                {
                    ViewBag.Flag = "WH";
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
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