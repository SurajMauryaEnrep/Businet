using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISDeliveryNoteDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISDeliveryNoteDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.MISDeliveryNoteDetail
{
    public class MISDeliveryNoteDetailController : Controller
    {
        string CompID, BrID, language = String.Empty;
        string DocumentMenuId = "105102180117", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        private MISDeliveryNoteDetail_IService _MISDeliveryNoteDetail_IService;
        DataSet ds;
        public MISDeliveryNoteDetailController(Common_IServices _Common_IServices, MISDeliveryNoteDetail_IService _MISDeliveryNoteDetail_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._MISDeliveryNoteDetail_IService = _MISDeliveryNoteDetail_IService;
        }
        // GET: ApplicationLayer/MISDeliveryNoteDetail
        public ActionResult MISDeliveryNoteDetail()
        {
            ViewBag.MenuPageName = getDocumentName();
            MISDeliveryNoteDetail_Model objModel = new MISDeliveryNoteDetail_Model();
            DateTime dtnow = DateTime.Now;
            //  string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            DataSet dttbl = new DataSet();
           
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
            //_MISDNModel.SuppliersList = GetSuppliers&ItemList();
            ds= GetSuppliersAndItemList(objModel);
            List<ItemsModel> ItemList = new List<ItemsModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemsModel _ItemModel = new ItemsModel();
                _ItemModel.ItemId = dr["Item_id"].ToString();
                _ItemModel.ItemName = dr["Item_name"].ToString();
                ItemList.Add(_ItemModel);
            }
            ItemList.Insert(0, new ItemsModel() { ItemId = "0", ItemName = "---Select---" });
            objModel.ItemsList = ItemList;

            List<SupplierModel> supplist = new List<SupplierModel>();

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                SupplierModel _Supp = new SupplierModel();
                _Supp.SuppId = dr["supp_id"].ToString();
                _Supp.SuppName = dr["supp_name"].ToString();
                supplist.Add(_Supp);
            }
           // supplist.Insert(0, new SupplierModel() { SuppId = "0", SuppName = "---Select---" });
            objModel.SuppliersList = supplist;

            GetDNMisItemWiseSummary(FromDate, ToDate, "0", "0");


            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/MISDeliveryNoteDetail/MISDeliveryNoteDetail.cshtml", objModel);
        }


        private DataSet GetFyList()
        {
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
                DataSet dt = _MISDeliveryNoteDetail_IService.Get_FYList(Comp_ID, Br_Id);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [NonAction]
        private DataSet GetSuppliersAndItemList(MISDeliveryNoteDetail_Model _MISDNModel)
        {
            try
            {
                string CompID = string.Empty;
                string Supp_Name = string.Empty;
                string Item_Name = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_MISDNModel.SuppId))
                {
                    Supp_Name = "0";
                }
                else
                {
                    Supp_Name = _MISDNModel.SuppId;
                }
                if (string.IsNullOrEmpty(_MISDNModel.ItemId))
                {
                    Item_Name = "0";
                }
                else
                {
                    Item_Name = _MISDNModel.ItemId;
                }
                DataSet ds = _MISDeliveryNoteDetail_IService.GetSuppliersAndItemList(CompID, BrID, Supp_Name, Item_Name);
                return ds;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetDNMisItemWiseSummary(string fromdate, string toDate, string suppId, string itemId)
        {
            MISDeliveryNoteDetail_Model objModel = new MISDeliveryNoteDetail_Model();
            objModel.SearchStatus = "SEARCH";
            ViewBag.ItemWiseSummary = GetDNMISReport("S", fromdate, toDate, suppId, itemId);
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISDeliveryNoteDetailItemWiseSummary.cshtml", objModel);
        }
        //public ActionResult GetDNMISByDetails(string fromdate, string toDate, string suppId, string itemId)
        //{
        //    MISDeliveryNoteDetail_Model objModel = new MISDeliveryNoteDetail_Model();
        //    objModel.SearchStatus = "SEARCH";
        //    ViewBag.DNDataByDetails = GetDNMISReport("D", fromdate, toDate, suppId, itemId);
        //    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISDeliveryNoteDetail.cshtml", objModel);
        //}
        public DataTable GetDNMISReport(string showAs, string fromdate, string toDate, string suppId, string itemId)
        {
            string compId = string.Empty;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            DataTable dt = _MISDeliveryNoteDetail_IService.GetDeliveryNoteMISReport(compId, BrID, showAs, fromdate, toDate, suppId, itemId);
            return dt;
        }

        //public ActionResult GetGRNMisSummary(string fromdate, string toDate, string suppId, string itemId)
        //{
        //    MisGoodReceiptDetail_Model objModel = new MisGoodReceiptDetail_Model();
        //    objModel.SearchStatus = "SEARCH";
        //    ViewBag.GRNSummary = GetGRNMisReport("SD", fromdate, toDate, suppId, itemId);
        //    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISGoodsReceiptSummary.cshtml", objModel);
        //}
        public ActionResult GetSubItemDetails(string DnNo, string DnDate,string Item_id)
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

                DataTable dt = _MISDeliveryNoteDetail_IService.GetSubItemDetails(Comp_ID, Br_ID, DnNo, DnDate, Item_id);
                ViewBag.SubitemDetail = dt;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.Flag = "";

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISDeliveryNoteSubItemDetail.cshtml");

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
    }

}