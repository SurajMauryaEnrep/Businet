using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockDetail;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MIS.ShopfloorTransferDetail;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.ShopfloorTransferDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ShopfloorSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.MIS.ShopfloorTransferDetail
{
    public class ShopfloorTransferDetailController : Controller
    {
        string CompID, language, title = String.Empty, BrID;
        string DocumentMenuId = "105105155125";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly StockDetail_ISERVICE _StockDetail_ISERVICE;
        private readonly ShopfloorTransferDetail_IService _shopfloorDetails;
        private readonly ShopfloorSetup_ISERVICES _ShopfloorSetup_ISERVICES;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public ShopfloorTransferDetailController(Common_IServices Common_IServices, StockDetail_ISERVICE StockDetail_ISERVICE,
            ShopfloorTransferDetail_IService shopfloorDetails, ShopfloorSetup_ISERVICES ShopfloorSetup_ISERVICES, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = Common_IServices;
            this._shopfloorDetails = shopfloorDetails;
            this._StockDetail_ISERVICE = StockDetail_ISERVICE;
            _ShopfloorSetup_ISERVICES = ShopfloorSetup_ISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/ShopfloorTransferDetail
        public ActionResult ShopfloorTransferDetail()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
               BrID = Session["BranchId"].ToString();
            
            // proc name - SP_GetShopfloorStockTransferMIS
            ViewBag.MenuPageName = getDocumentName();
            ShopfloortransferDetailModel objModel = new ShopfloortransferDetailModel();
            objModel.Title = title;
            //StockDetail_Model _StockDetail_Model = new StockDetail_Model();
            GetItemList(objModel);
            GetItemGrpList(objModel);
            DateTime dtnow = DateTime.Now;
            /*Commented By Nitesh 06-12-2023 from_date fincial start Date not month start date*/
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
            objModel.ToDate = ToDate;
          // objModel.FromDate = FromDate;
            objModel.ShopFloorList = GetShflList(Convert.ToInt32(CompID), Convert.ToInt32(BrID));
            objModel.StatusList = GetShflStatusList(CompID,BrID);
            ViewBag.ShflTrfDetails = _shopfloorDetails.GetShflTrfReport(CompID, BrID, "", "", "", objModel.FromDate, objModel.ToDate, "", "", "");
            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/ShopfloorTransferDetail/ShopfloorTransferDetail.cshtml",objModel);
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

        public ActionResult SearchShfltrfDetails(string trfType,string materialType,string itemId, string fromDate, string toDate,
            string itemGrpId, string shflId, string status)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            ShopfloortransferDetailModel objModel = new ShopfloortransferDetailModel();
            ViewBag.ShflTrfDetails = _shopfloorDetails.GetShflTrfReport(CompID, BrID, trfType, materialType, itemId,
                fromDate, toDate, itemGrpId, shflId, status);
            objModel.STFilter = "STFilter";
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialShopfloorTransferDetail.cshtml",objModel);
        }
        public ActionResult GetTrfBatchOrSerialDetails(string trfNo, string trfDate, string itemId)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            ViewBag.ShflTrfItemBatchDetails = _shopfloorDetails.GetShflStktrfBatchDetail(CompID, BrID, trfNo, trfDate, itemId);
            return PartialView("~/Areas/Common/Views/Comn_PartialMISBatchDetail.cshtml");
        }
        public List<StatuslistModel> GetShflStatusList(string compId, string brId)
        {
            List<StatuslistModel> statusList = new List<StatuslistModel>();
            var defaultStatus = new StatuslistModel
            {
                status_code = "0",
                status_name = "---ALL---"
            };
            statusList.Add(defaultStatus);
            DataTable dt = _shopfloorDetails.GetShflStatusList(compId, brId);
            foreach(DataRow dtr in dt.Rows)
            {
                 defaultStatus = new StatuslistModel
                {
                    status_code = dtr["status_code"].ToString(),
                    status_name = dtr["status_name"].ToString()
                 };
                statusList.Add(defaultStatus);
            }
            return statusList;
        }
        public List<ShopFloor> GetShflList(int compId, int brId)
        {
            List<ShopFloor> shflList = new List<ShopFloor>();
            ShopFloor shfl = new ShopFloor()
            {
                shfl_id = "0",
                shfl_name = "----ALL----"
            };
            shflList.Add(shfl);
            DataTable dt = _ShopfloorSetup_ISERVICES.GetShopFloorDetailsDAL(compId, brId);
            foreach (DataRow dtr in dt.Rows)
            {
                shfl = new ShopFloor()
                {
                    shfl_id = dtr["shfl_id"].ToString(),
                    shfl_name = dtr["shfl_name"].ToString()
                };
                shflList.Add(shfl);
            }
            return shflList;
        }
        public ActionResult GetItemList(ShopfloortransferDetailModel queryParameters)
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
                if (string.IsNullOrEmpty(queryParameters.ItemId))
                {
                    ItemName = "0";
                }
                else
                {
                    ItemName = queryParameters.ItemId;
                }
                ItemList = _StockDetail_ISERVICE.ItemList(ItemName, Comp_ID);

                List<ItemListModel> _ItemList = new List<ItemListModel>();
                foreach (var data in ItemList)
                {
                    ItemListModel _ItemDetail = new ItemListModel();
                    _ItemDetail.item_id = data.Key;
                    _ItemDetail.item_name = data.Value;
                    _ItemList.Add(_ItemDetail);
                }
                _ItemList.RemoveAt(0);
                _ItemList.Insert(0, new ItemListModel
                {
                    item_id = "0",
                    item_name = "---ALL---"
                });
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
        public ActionResult GetItemGrpList(ShopfloortransferDetailModel queryParameters)
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
                if (string.IsNullOrEmpty(queryParameters.ItemGroupId))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ItemGroupId;
                }
                ItemGroupList = _StockDetail_ISERVICE.ItemGroupList(GroupName, Comp_ID);

                List<ItemGroupModel> _ItemGroupList = new List<ItemGroupModel>();
                foreach (var data in ItemGroupList)
                {
                    ItemGroupModel _ItemGroupDetail = new ItemGroupModel();
                    _ItemGroupDetail.item_grp_id = data.Key;
                    _ItemGroupDetail.GroupName = data.Value;
                    _ItemGroupList.Add(_ItemGroupDetail);
                }
                _ItemGroupList.RemoveAt(0);
                _ItemGroupList.Insert(0, new ItemGroupModel
                {
                    item_grp_id = "0",
                    GroupName = "---ALL---"
                });
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
        public ActionResult GetSubItemDetails(string Item_id, string Flag, string Doc_no, string Doc_dt)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();    
                dt = _shopfloorDetails.TRF_GetSubItemDetails(CompID, BrID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    IsDisabled = "Y",
                    ShowStock = "N",
                    _subitemPageName = "ShopfloorTransferDetail"
                };
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}