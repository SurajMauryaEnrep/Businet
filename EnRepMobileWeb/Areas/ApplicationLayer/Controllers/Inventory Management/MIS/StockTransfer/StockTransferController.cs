using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockTransfer;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockTransfer;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.StockTransfer
{
    public class StockTransferController : Controller
    {
        string CompID, BrchID, language = String.Empty;
        string DocumentMenuId = "105102180115", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly StockTransfer_IService _stktrfrepo;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public StockTransferController(Common_IServices _Common_IServices, StockTransfer_IService stktrfrepo,
            GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._stktrfrepo = stktrfrepo;
            this._GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/StockTransfer
        public ActionResult StockTransfer()
        {
            ViewBag.MenuPageName = getDocumentName();
            StockTransfer_Model objModel = new StockTransfer_Model();
            objModel.SrcBranchList = GetSrcBranchList();
            objModel.SrcWarehouseList = GetSrcWarehouseList();
            objModel.DstnBranchList = GetSrcBranchList();
            objModel.DstnWarehouseList = GetSrcWarehouseList();
            objModel.ItemsList = GetItemsList();
            DataSet ds = GetFyList();

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                objModel.FromDate = ds.Tables[0].Rows[0]["currfy_startdt"].ToString();
            }
            objModel.ToDate = DateTime.Now.ToString("yyyy-MM-dd");
            getDocumentName();
            ViewBag.title = title;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/StockTransfer/StockTransfer.cshtml", objModel);
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
        private List<ItemsModel> GetItemsList()
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                List<ItemsModel> ItmList = new List<ItemsModel>();
                ItemsModel objNode = new ItemsModel()
                {
                    ItemId = "0",
                    ItemName = "---ALL---"
                };
                ItmList.Add(objNode);
                DataTable dt = _stktrfrepo.GetItemsList(CompID);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtr in dt.Rows)
                        {
                            objNode = new ItemsModel()
                            {
                                ItemId = dtr["Item_id"].ToString(),
                                ItemName = dtr["Item_name"].ToString()
                            };
                            ItmList.Add(objNode);
                        }
                    }
                }
                return ItmList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private List<STBranchModel> GetSrcBranchList()
        {
            try
            {
                List<STBranchModel> objList = new List<STBranchModel>();
                STBranchModel objNode = new STBranchModel();
                //{
                //    BrId = "0",
                //    BrName = "---ALL---"
                //};
                //objList.Add(objNode);
                DataTable dttbl = GetSourceBranchOrWarehouseList("B");
                if (dttbl.Rows.Count > 0)
                {
                    foreach (DataRow dtr in dttbl.Rows)
                    {
                        objNode = new STBranchModel()
                        {
                            BrId = dtr["Comp_Id"].ToString(),
                            BrName = dtr["comp_nm"].ToString()
                        };
                        objList.Add(objNode);
                    }
                }
                return objList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private List<STWareHouseModel> GetSrcWarehouseList()
        {
            try
            {
                List<STWareHouseModel> whList = new List<STWareHouseModel>();
                STWareHouseModel objNode = new STWareHouseModel();
                //{
                //    WhId = "0",
                //    WhName = "---ALL---"
                //};
                //whList.Add(objNode);
                DataTable dt = GetSourceBranchOrWarehouseList("W");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtr in dt.Rows)
                    {
                        objNode = new STWareHouseModel()
                        {
                            WhId = dtr["wh_id"].ToString(),
                            WhName = dtr["wh_name"].ToString()
                        };
                        whList.Add(objNode);
                    }
                }
                return whList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public DataTable GetSourceBranchOrWarehouseList(string flag)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                DataSet ds = _stktrfrepo.GetBranchAndWarehouseList(CompID, BrchID);
                if (flag == "B")
                    return ds.Tables[0];
                else
                    return ds.Tables[1];
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
        public ActionResult GetStockTransferMISReport(string itemId, string mtType, string srcBranch,
            string dstnBranch, string srcWarehouse, string dstnWarehouse, string fromDate, string toDate)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                DataTable dt = _stktrfrepo.GetStockTransferReport(CompID, BrchID, itemId, mtType, srcBranch, dstnBranch,
                    srcWarehouse, dstnWarehouse, fromDate, toDate);
                ViewBag.StkTrfData = dt;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_PartialStockTransferMISList.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public JsonResult GetSrcWarehouseList(string brId)
        {
            JsonResult jsonrows = null;
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                DataSet ds = _stktrfrepo.GetBranchAndWarehouseList(CompID, brId);
                jsonrows = Json(JsonConvert.SerializeObject(ds.Tables[1]));
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return jsonrows;
        }
        public JsonResult GetUomNoByItemId(string itemId)
        {
            JsonResult jsonrows = null;
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                DataTable dt = _stktrfrepo.GetUomByItemId(CompID, itemId);
                jsonrows = Json(JsonConvert.SerializeObject(dt));
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return jsonrows;
        }
        public ActionResult GetStockTransferPopupData(string actflag, string itemId, string mtType, string srcBranch,
               string dstnBranch, string srcWarehouse, string dstnWarehouse, string fromDate, string toDate)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                DataTable dt = _stktrfrepo.GetStockTransferPopupData(actflag, CompID, BrchID, itemId, mtType, srcBranch, dstnBranch,
                    srcWarehouse, dstnWarehouse, fromDate, toDate);
                ViewBag.StkTrfPopupData = dt;
                if (actflag.ToUpper() == "REQ")
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISRequisitionQuantityDetail.cshtml");
                else if(actflag.ToUpper() == "TRF")
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISTransferQuantityDetail.cshtml");
                else
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISReceiptQuantityDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
    }

}