using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MIS.MaterialTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.MIS.MaterialTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SubContracting.MIS.MaterialTracking
{
    public class MaterialTrackingController : Controller
    {
        string CompID, Br_ID, Language = String.Empty;
        string DocumentMenuId = "105108125110", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        MaterialTracking_ISERVICE _MaterialTracking_ISERVICE;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public MaterialTrackingController(Common_IServices _Common_IServices, MaterialTracking_ISERVICE _MaterialTracking_ISERVICE
            , GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._MaterialTracking_ISERVICE = _MaterialTracking_ISERVICE;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/MaterialTracking
        public ActionResult MaterialTracking()
        {
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string Language = string.Empty;
            MaterialTracking_Model _MaterialTracking_Model = new MaterialTracking_Model();
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
            CommonPageDetails();
            DateTime dtnow = DateTime.Now;
            string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            string ToDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
            //_MaterialTracking_Model.FromDate = startDate;

            DataSet dttbl = new DataSet();
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                var date = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                var fromdate = date.Split('-');
               // _MaterialTracking_Model.FromDate = fromdate[2] + "-" + fromdate[1] + "-" + fromdate[0];
                _MaterialTracking_Model.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                //ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                //ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }

            _MaterialTracking_Model.ToDate = ToDate;
            _MaterialTracking_Model.SupplierNameList = GetSuppliersList();
            //GetAutoCompleteSearchSuppList(_MaterialTracking_Model);

            List<JobOrderNoList> LstOrder = new List<JobOrderNoList>();
            JobOrderNoList objOrder = new JobOrderNoList()
            {
                JobOrdnoVal = "0",
                JobOrdnoId = "---All---",
                 
            };
            LstOrder.Add(objOrder);
            _MaterialTracking_Model.JobOrdList = LstOrder;

            _MaterialTracking_Model.MaterialTrackList = SearchMTTrackDetail_Onload(_MaterialTracking_Model);


            _MaterialTracking_Model.Title = title;

            _MaterialTracking_Model.MtrlTrackSearch = "";
            //ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/ApplicationLayer/Views/SubContracting/MIS/MaterialTracking/MaterialTrackingDetail.cshtml", _MaterialTracking_Model);
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
        private List<SupplierName> GetSuppliersList()
        {
            string compId = string.Empty;
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            SuppList = _MaterialTracking_ISERVICE.GetSupplierList(compId, "0", Br_ID);

            List<SupplierName> _SuppList = new List<SupplierName>();
            foreach (var data in SuppList)
            {
                SupplierName _SuppDetail = new SupplierName();
                _SuppDetail.supp_id = data.Key;
                _SuppDetail.supp_name = data.Value;
                _SuppList.Add(_SuppDetail);
            }
            return _SuppList;
        }
        //public ActionResult GetAutoCompleteSearchSuppList(MaterialTracking_Model _MaterialTracking_Model)
        //{
        //    string SupplierName = string.Empty;
        //    Dictionary<string, string> CustList = new Dictionary<string, string>();
        //    string Comp_ID = string.Empty;
        //    string Br_ID = string.Empty;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        if (string.IsNullOrEmpty(_MaterialTracking_Model.Supp_Name))
        //        {
        //            SupplierName = "0";
        //        }
        //        else
        //        {
        //            SupplierName = _MaterialTracking_Model.Supp_Name;
        //        }
        //        CustList = _MaterialTracking_ISERVICE.GetSupplierList(Comp_ID, SupplierName, Br_ID);

        //        List<SupplierName> _SuppList = new List<SupplierName>();
        //        foreach (var data in CustList)
        //        {
        //            SupplierName _SuppDetail = new SupplierName();
        //            _SuppDetail.supp_id = data.Key;
        //            _SuppDetail.supp_name = data.Value;
        //            _SuppList.Add(_SuppDetail);
        //        }
        //        _MaterialTracking_Model.SupplierNameList = _SuppList;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        //return Json("ErrorPage");
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult GetProductNameInDDLListPage(MaterialTracking_Model _MaterialTracking_Model)
        {
            JsonResult DataRows = null;
            string MtrlItmName = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(_MaterialTracking_Model.JO_ItemName))
                    {
                        MtrlItmName = "0";
                    }
                    else
                    {
                        MtrlItmName = _MaterialTracking_Model.JO_ItemName;
                    }

                    DataSet ProductList = _MaterialTracking_ISERVICE.BindProductNameInDDL(Comp_ID, Br_ID, MtrlItmName);
                    DataRow DRow = ProductList.Tables[0].NewRow();

                    DRow[0] = "0";
                    DRow[1] = "---All---";
                    DRow[2] = "0";
                    ProductList.Tables[0].Rows.InsertAt(DRow, 0);
                    DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/

                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        //[HttpPost]
        public JsonResult GetJobORDDocList()
        {
            JsonResult DataRows = null;
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
                DataTable Dt = _MaterialTracking_ISERVICE.GetJobORDDocList(Comp_ID, Br_ID);

                DataRows = Json(JsonConvert.SerializeObject(Dt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        private List<MaterialTrackListDetail> SearchMTTrackDetail_Onload(MaterialTracking_Model _MaterialTracking_Model)
        {
            try
            {
                List<MaterialTrackListDetail> _MaterialListDetail = new List<MaterialTrackListDetail>();
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
                dt = _MaterialTracking_ISERVICE.GetMTTrackDetailList_Onload(CompID, Br_ID, "0", "0", "0", _MaterialTracking_Model);


                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        MaterialTrackListDetail _MaterialTrackList = new MaterialTrackListDetail();
                        _MaterialTrackList.MaterialID = dr["MaterialID"].ToString();
                        _MaterialTrackList.MaterialName = dr["MaterialName"].ToString();
                        _MaterialTrackList.MaterialUOMId = dr["MUomId"].ToString();
                        _MaterialTrackList.MaterialUOM = dr["MUomName"].ToString();
                        _MaterialTrackList.SupplierId = dr["supp_id"].ToString();
                        _MaterialTrackList.SupplierName = dr["supp_name"].ToString();
                        _MaterialTrackList.OrderNo = dr["JobOrdNo"].ToString();
                        _MaterialTrackList.OrderDate = dr["JobOrdDate"].ToString();
                        _MaterialTrackList.OrderDt = dr["JobOrdDT"].ToString();
                        //_MaterialTrackList.FinishProductId = dr["fg_product_id"].ToString();
                        //_MaterialTrackList.FinishProductName = dr["FinishPrdctName"].ToString();
                        //_MaterialTrackList.FinishProducUOMId = dr["fg_uom_id"].ToString();
                        //_MaterialTrackList.FinishProducUOM = dr["FinishPrdctUOM"].ToString();
                        //_MaterialTrackList.OperationId = dr["op_id"].ToString();
                        //_MaterialTrackList.OperationName = dr["op_name"].ToString();
                        _MaterialTrackList.OpOutProductID = dr["OpOutPrdctId"].ToString();
                        _MaterialTrackList.OpOutProductName = dr["OpOutPrdctName"].ToString();
                        _MaterialTrackList.OpOutUOMId = dr["OpOutUomId"].ToString();
                        _MaterialTrackList.OpOutUOM = dr["OpOutUOM"].ToString();
                       
                        _MaterialTrackList.OrderQuantity = dr["OrdQty"].ToString();
                        _MaterialTrackList.IssueQuantity = dr["Issue_qty"].ToString();
                        _MaterialTrackList.ConsumeQuantity = dr["Consume_qty"].ToString();
                        _MaterialTrackList.ReturnQuantity = dr["Return_qty"].ToString();
                        _MaterialTrackList.BalanceQuantity = dr["Balance_qty"].ToString();
                        //_MaterialTrackList.ReworkableQuantity = dr["ReworkQty"].ToString();
                        //_MaterialTrackList.Status = dr["JobStatus"].ToString();
                        //_MaterialTrackList.StatusName = dr["issued"].ToString();

                        _MaterialListDetail.Add(_MaterialTrackList);
                    }
                }
                return _MaterialListDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchMaterialTrackDetail(string SuppId, string OutOPProdctID, string JobOrdNO, string Fromdate, string Todate)
        {
            try
            {
                List<MaterialTrackListDetail> _MaterialListDetail = new List<MaterialTrackListDetail>();
                MaterialTracking_Model _MaterialTracking_Model = new MaterialTracking_Model();
                string CompID = string.Empty;
                string BrachID = string.Empty;
                string Partial_View = string.Empty;
                DataTable dt = new DataTable();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrachID = Session["BranchId"].ToString();
                }
                dt = _MaterialTracking_ISERVICE.GetMaterialTrackDetail(CompID, BrachID, SuppId,OutOPProdctID, JobOrdNO, Fromdate, Todate);

                if (dt.Rows.Count > 0)
                {
                    //_StockDetailShopfloor_Model.StockBy = StockBy;
                    //_StockDetailShopfloor_Model.StockByFilter = StockBy;

                    foreach (DataRow dr in dt.Rows)
                    {
                        MaterialTrackListDetail _MaterialTrackList = new MaterialTrackListDetail();
                        _MaterialTrackList.MaterialID = dr["MaterialID"].ToString();
                        _MaterialTrackList.MaterialName = dr["MaterialName"].ToString();
                        _MaterialTrackList.MaterialUOMId = dr["MUomId"].ToString();
                        _MaterialTrackList.MaterialUOM = dr["MUomName"].ToString();
                        _MaterialTrackList.SupplierId = dr["supp_id"].ToString();
                        _MaterialTrackList.SupplierName = dr["supp_name"].ToString();
                        _MaterialTrackList.OrderNo = dr["JobOrdNo"].ToString();
                        _MaterialTrackList.OrderDate = dr["JobOrdDate"].ToString();
                        _MaterialTrackList.OrderDt = dr["JobOrdDT"].ToString();
                        _MaterialTrackList.OpOutProductID = dr["OpOutPrdctId"].ToString();
                        _MaterialTrackList.OpOutProductName = dr["OpOutPrdctName"].ToString();
                        _MaterialTrackList.OpOutUOMId = dr["OpOutUomId"].ToString();
                        _MaterialTrackList.OpOutUOM = dr["OpOutUOM"].ToString();
                        _MaterialTrackList.OrderQuantity = dr["OrdQty"].ToString();
                        _MaterialTrackList.IssueQuantity = dr["Issue_qty"].ToString();
                        _MaterialTrackList.ConsumeQuantity = dr["Consume_qty"].ToString();
                        _MaterialTrackList.ReturnQuantity = dr["Return_qty"].ToString();
                        _MaterialTrackList.BalanceQuantity = dr["Balance_qty"].ToString();
                        
                        _MaterialListDetail.Add(_MaterialTrackList);
                    }
                }

                _MaterialTracking_Model.MaterialTrackList = _MaterialListDetail;
                _MaterialTracking_Model.MtrlTrackSearch = "SEARCH";
                CommonPageDetails();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMaterialTrackingList.cshtml", _MaterialTracking_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult GetMTAllQtyItemDetailList(string Type, string JobOrdNo, string hdnJobOrdDt,string MaterialID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string Branch = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Branch = Session["BranchId"].ToString();
                }
                DataSet result = _MaterialTracking_ISERVICE.GetMTAllQtyItemDetailList(Comp_ID, Branch, Type, JobOrdNo, hdnJobOrdDt, MaterialID);

                //DataView dv = new DataView(result.Tables[0]);
                //dv.Sort = "CDate desc";
                //DataTable dt = new DataTable();
                //dt = dv.ToTable();
                //DataSet dset = new DataSet();
                //dset.Tables.Add(dt);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, Language);
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