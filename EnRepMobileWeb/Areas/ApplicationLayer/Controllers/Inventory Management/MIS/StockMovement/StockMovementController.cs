using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockMovement;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockMovement;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.StockMovement
{
    public class StockMovementController : Controller
    {
        string DocumentMenuId = "105102180105";
        string CompID, language, title = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        StockMovement_Model _StockMovement_Model;
        StockMovement_ISERVICE _StockMovement_ISERVICE;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public StockMovementController(Common_IServices _Common_IServices, StockMovement_ISERVICE _StockMovement_ISERVICE, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._StockMovement_ISERVICE = _StockMovement_ISERVICE;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/StockMovement
        public ActionResult StockMovement()
        {
            StockMovement_Model _StockMovement_Model = new StockMovement_Model();
            ViewBag.MenuPageName = getDocumentName();
           
            List<ItemName> _ItemName = new List<ItemName>();
            List<Batch> _Batch = new List<Batch>();
            List<Serial> _Serial = new List<Serial>();

            //DataSet Dset_hdata = GetItem_List();/*Commented by Suraj Maurya regaurding ticket id : 1536*/

            //foreach (DataRow dr in Dset_hdata.Tables[0].Rows)
            //{
            //    ItemName ddlItemName = new ItemName();
            //    ddlItemName.Id = dr["item_id"].ToString();
            //    ddlItemName.Name = dr["item_name"].ToString();
            //    _ItemName.Add(ddlItemName);
            //}
            _ItemName.Insert(0, new ItemName() { Id = "0", Name = "---Select---" });
            _StockMovement_Model.itemNameList = _ItemName;

            _Batch.Insert(0, new Batch() { Id = "0", Name = "---Select---" });
            _StockMovement_Model.batcheList = _Batch;

            _Serial.Insert(0, new Serial() { Id = "0", Name = "---Select---" });
            _StockMovement_Model.SerialList = _Serial;

            DateTime now = DateTime.Now;
            #region Commented By Nitesh  02-01-2024 for Financial start Year get from database 
            #endregion
            //  var startDate = new DateTime(now.Year, now.Month, 1);
            //  var endDate = startDate.AddMonths(1).AddDays(-1);
            //string startDate = new DateTime(now.Year, 4, 1).ToString("yyyy-MM-dd");  /*Modified Month By Nitesh 05-12-2023 add Fincacial Year in From Date*/
            DataSet dttbl = new DataSet();
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                _StockMovement_Model.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }
            string endDate = now.ToString("yyyy-MM-dd");

           // _StockMovement_Model.FromDate = startDate;
            _StockMovement_Model.ToDate = endDate;

            _StockMovement_Model.Title = title;
            //Session["MovementBy"] = null;
            _StockMovement_Model.MovementBy = null;
            List<MovementDetailsList> _MovementListDetail = new List<MovementDetailsList>();
            _StockMovement_Model.MovDetailsLists = _MovementListDetail;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/StockMovement/StockMovement.cshtml", _StockMovement_Model);
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
        private DataSet GetItem_List()
        {
            try
            {
                
                string Item = string.Empty;
                string Batch = string.Empty;

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
               
                DataSet Dset = _StockMovement_ISERVICE.BindAllDropdownLists(Comp_ID);
                return Dset;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public JsonResult GetBatchDetails(string ItemId)
        {
            JsonResult DataRows = null;
            string Comp_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet Dt_BatchList = _StockMovement_ISERVICE.GetBatch_Lists(Comp_ID, ItemId);
                DataRows = Json(JsonConvert.SerializeObject(Dt_BatchList));/*Result convert into Json Format for javasript*/
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
        public ActionResult SearchMovementDetail(string MovementBy, string ItemId, string BatchNo,string Serial_no, string Fromdt, string Todt)
        {
            try
            {
                List<MovementDetailsList> _MovementListDetail = new List<MovementDetailsList>();
                StockMovement_Model _Movement_Model = new StockMovement_Model();
                string CompID = string.Empty;
                string Br_Id = string.Empty;
                string Partial_View = string.Empty;
                DataTable dt = new DataTable();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_Id = Session["BranchId"].ToString();
                }
                dt = _StockMovement_ISERVICE.GetMovementList(CompID, Br_Id, MovementBy,ItemId,BatchNo, Serial_no, Fromdt,Todt);

                //Session["MovementBy"] = "Movementdata";
                _Movement_Model.MovementBy = "Movementdata";
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        MovementDetailsList _MoveList = new MovementDetailsList();
                        _MoveList.itemname = dr["item_name"].ToString();
                        _MoveList.uom = dr["uom_name"].ToString();
                        _MoveList.branch = dr["comp_nm"].ToString();
                        _MoveList.warehouse = dr["wh_name"].ToString();
                        _MoveList.lot = dr["lot_no"].ToString();
                        _MoveList.batch = dr["batch_no"].ToString();
                        _MoveList.serial_no = dr["serial_no"].ToString();
                        _MoveList.transdate = dr["trans_dt"].ToString();
                        _MoveList.sourcetype = dr["sourcetype"].ToString();
                        _MoveList.docno = dr["doc_no"].ToString();
                        _MoveList.billno = dr["bill_no"].ToString();
                        _MoveList.billdt = dr["bill_dt"].ToString();
                        _MoveList.costprice = dr["landedcost"].ToString();
                        _MoveList.openingstock = dr["opening_qty"].ToString();
                        _MoveList.inwardqty = dr["inward_qty"].ToString();
                        _MoveList.outwardqty = dr["outward_qty"].ToString();
                        _MoveList.closingstock = dr["closing_qty"].ToString();
                        _MoveList.closingvalue = dr["clsoing_val"].ToString();
                        //_MoveList.closingstock = dr["br_avl_stk"].ToString();
                        //_MoveList.closingvalue = dr["br_avl_stk_val"].ToString();
                        _MoveList.mfg_name = dr["mfg_name"].ToString();
                        _MoveList.mfg_mrp = dr["mfg_mrp"].ToString();
                        _MoveList.mfg_date = dr["mfg_date"].ToString();

                        _MovementListDetail.Add(_MoveList);
                    }
                }

                _Movement_Model.MovDetailsLists = _MovementListDetail;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_PartialStockMovementList.cshtml", _Movement_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

    }

}