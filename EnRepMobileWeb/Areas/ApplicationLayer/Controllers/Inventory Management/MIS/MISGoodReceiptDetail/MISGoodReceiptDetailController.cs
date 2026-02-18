using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISGoodReceiptDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.GoodsReceiptNote;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISGoodReceiptDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.MISGoodReceiptDetail
{
    public class MISGoodReceiptDetailController : Controller
    {
        string DocumentMenuId = "105102180120", title;
        string CompID, BrID, language = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly MISGoodReceiptDetail_IService _GRNRepo;
        private readonly ItemList_ISERVICES _itemSetup;
        private readonly GoodsReceiptNote_ISERVICE _GoodsReceiptNote_ISERVICE;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;

        public MISGoodReceiptDetailController(Common_IServices _Common_IServices, MISGoodReceiptDetail_IService GRNRepo,
            ItemList_ISERVICES itemSetup, GoodsReceiptNote_ISERVICE GoodsReceiptNote_ISERVICE, GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            _GRNRepo = GRNRepo;
            _itemSetup = itemSetup;
            _GoodsReceiptNote_ISERVICE = GoodsReceiptNote_ISERVICE;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }

        // GET: ApplicationLayer/MISGoodReceiptDetail
        public ActionResult MISGoodReceiptDetail()
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                MisGoodReceiptDetail_Model objModel = new MisGoodReceiptDetail_Model();
                DateTime dtnow = DateTime.Now;
                //  string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                DataSet dttbl = new DataSet();
                #region Added By Nitesh  02-01-2024 for Financial Year 
                #endregion
                dttbl = GetFyList();
                if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
                {
                    objModel.Fromdate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                    ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                    ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                    ViewBag.fylist = dttbl.Tables[1];
                }
                string FromDate = objModel.Fromdate;
                string ToDate = dtnow.ToString("yyyy-MM-dd");
                objModel.Title = title;
                //objModel.Fromdate = FromDate;
                objModel.ToDate = ToDate;
                objModel.SuppliersList = GetSuppliersList();
                objModel.ItemsList = GetItemsList();
                StatusBind(objModel);
                GetGRNMisItemWiseSummary(FromDate, ToDate, "0", "0", "");
                ViewBag.DocumentMenuId = DocumentMenuId;
                List<EntityNameList1> _EntityName = new List<EntityNameList1>();
                EntityNameList1 _EntityNameList = new EntityNameList1();
                _EntityNameList.entity_name = "All";
                _EntityNameList.entity_id = "0";
                _EntityName.Add(_EntityNameList);
                objModel.EntityNameList = _EntityName;
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/MISGoodReceiptDetail/MISGoodReceiptDetail.cshtml", objModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void StatusBind(MisGoodReceiptDetail_Model objModel)
        {
            CommonPageDetails();
            List<Status> list2 = new List<Status>();
            foreach (var dr in ViewBag.StatusList.Rows)
            {
                if (dr["status_code"] == "0" || dr["status_code"] == "F" || dr["status_code"] == "C" || dr["status_code"] == "D")
                {

                }
                else
                {
                    Status Status = new Status();
                    Status.status_id = dr["status_code"].ToString();
                    Status.status_name = dr["status_name"].ToString();
                    list2.Add(Status);
                }

            }
            objModel.StatusList = list2;
        }
        private void CommonPageDetails()
        {
            try
            {

                string Comp_ID = string.Empty;
                string Br_Id = string.Empty;
                string UserID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_Id = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(Comp_ID, Br_Id, UserID, "105102115101", language);
                //ViewBag.AppLevel = ds.Tables[0];
                //string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
                //ViewBag.VBRoleList = ds.Tables[3];
                ViewBag.StatusList = ds.Tables[4];

                //string[] Docpart = DocumentName.Split('>');
                //int len = Docpart.Length;
                //if (len > 1)
                //{
                //    title = Docpart[len - 1].Trim();
                //}
                //ViewBag.MenuPageName = DocumentName;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
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
        private List<ItemsModel> GetItemsList()
        {
            try
            {
                string compId = string.Empty;
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrID = Session["BranchId"].ToString();

                DataTable dt = _itemSetup.BindGetItemList("", compId, BrID);
                List<ItemsModel> itemsList = new List<ItemsModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    ItemsModel itmLst = new ItemsModel
                    {
                        ItemId = dr["item_id"].ToString(),
                        ItemName = dr["item_name"].ToString()
                    };
                    itemsList.Add(itmLst);
                }
                itemsList.Insert(0, new ItemsModel { ItemId = "0", ItemName = "---All---" });
                return itemsList;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        private List<SupplierModel> GetSuppliersList()
        {
            string compId = string.Empty;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            SuppList = _GoodsReceiptNote_ISERVICE.GetSupplierList(compId, "0", BrID);

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

        public DataTable GetGRNMisReport(string showAs, string fromdate, string toDate, string suppId, string itemId, string MultiselectStatusHdn, string ReceiptType, string EntityType)
        {

            string compId = string.Empty;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();

            DataTable dt = _GRNRepo.GetGoodReceiptNoteMISReport(compId, BrID, showAs, fromdate, toDate, suppId, itemId, MultiselectStatusHdn, ReceiptType, EntityType);
            return dt;
        }
        public ActionResult GetGRNMisItemWiseSummary(string fromdate, string toDate, string suppId, string itemId, string MultiselectStatusHdn)
        {
            MisGoodReceiptDetail_Model objModel = new MisGoodReceiptDetail_Model();
            objModel.SearchStatus = "SEARCH";
            ViewBag.ItemWiseSummary = GetGRNMisReport("S", fromdate, toDate, suppId, itemId, MultiselectStatusHdn, "GRN", "");
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISGoodsReceiptDetailItemWiseSummary.cshtml", objModel);
        }
        public ActionResult GetGRNMisSummary(string fromdate, string toDate, string suppId, string itemId, string MultiselectStatusHdn)
        {
            MisGoodReceiptDetail_Model objModel = new MisGoodReceiptDetail_Model();
            objModel.SearchStatus = "SEARCH";
            ViewBag.GRNSummary = GetGRNMisReport("SD", fromdate, toDate, suppId, itemId, MultiselectStatusHdn, "GRN", "");
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISGoodsReceiptSummary.cshtml", objModel);
        }
        public ActionResult GetGRNMisDetails(string fromdate, string toDate, string suppId, string itemId, string MultiselectStatusHdn, string ReceiptType, string EntityType)
        {
            MisGoodReceiptDetail_Model objModel = new MisGoodReceiptDetail_Model();

            if (ReceiptType == "GRN")
            {
                objModel.SearchStatus = "SEARCH";
                ViewBag.GRNDetails = GetGRNMisReport("D", fromdate, toDate, suppId, itemId, MultiselectStatusHdn, ReceiptType, "");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISGoodsReceiptDetail.cshtml", objModel);
            }
            else
            {
                objModel.SearchStatus = "SEARCH";
                ViewBag.ExternalReciptList = GetGRNMisReport("", fromdate, toDate, suppId, itemId, MultiselectStatusHdn, ReceiptType, EntityType);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISExternalReceipt.cshtml", objModel);
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

        public ActionResult GetSupp_CustList(MisGoodReceiptDetail_Model ListModel)
        {
            try
            {
                string BrchID = string.Empty;
                string UserID = string.Empty;
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
                    language = Session["Language"].ToString();
                }
                string EntityName = string.Empty;
                string EntityType = string.Empty;
                List<EntityNameList1> _EntityName = new List<EntityNameList1>();
                if (string.IsNullOrEmpty(ListModel.EntityName))
                {
                    EntityName = "0";
                }
                else
                {
                    EntityName = ListModel.EntityName.ToString();
                }
                if (string.IsNullOrEmpty(ListModel.entity_type))
                {
                    EntityType = "0";
                }
                else
                {
                    EntityType = ListModel.entity_type.ToString();
                }
                DataSet SuppCustList = _GRNRepo.getSuppCustList(CompID, BrchID, EntityName, EntityType);
                if (EntityType == "0")
                {
                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList1 _EntityNameList = new EntityNameList1();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                else
                {
                    DataRow Drow = SuppCustList.Tables[0].NewRow();
                    Drow[0] = "0";
                    Drow[1] = "---Select---";
                    SuppCustList.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList1 _EntityNameList = new EntityNameList1();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                ListModel.EntityNameList = _EntityName;
                return Json(_EntityName.Select(c => new { Name = c.entity_name, ID = c.entity_id }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public ActionResult GetBatchDeatilMIS(string recept_no, string recept_dt, string Item_id)
        {
            MisGoodReceiptDetail_Model objModel = new MisGoodReceiptDetail_Model();

            string compId = string.Empty;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();

            DataTable dt = _GRNRepo.GetBatchDeatilMIS(compId, BrID, recept_no, recept_dt, Item_id);
            ViewBag.BatchDetailExternalReciptList = dt;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISExternalReceiptBatchNumber.cshtml");
        }

        public ActionResult GetSerialDetailData(string recept_no, string recept_dt, string Item_id)
        {
            MisGoodReceiptDetail_Model objModel = new MisGoodReceiptDetail_Model();

            string compId = string.Empty;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();

          
            DataTable dt = _GRNRepo.GetMISSerialDetailData(compId, BrID, recept_no, recept_dt, Item_id);
            ViewBag.MISExternalSerialDetail = dt;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISExternalReceiptSerialNoDetail.cshtml");
        }




    }

}