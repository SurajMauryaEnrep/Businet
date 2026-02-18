using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.DomesticPurchaseOrder;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.DomesticPurchaseOrder;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.DomesticPurchaseOrder
{
    public class DPOController : Controller
    {
        string DocumentMenuId;
        List<DomesticPurchaseOrderList> _DomesticPurchaseOrderList;
        string CompID, language, title, UserID, create_id, BrchID = string.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DPOList_ISERVICE _DPOList_ISERVICE;
        DPODetail_ISERVICE _DPODetail_ISERVICE;
        Common_IServices _Common_IServices;

        public DPOController(Common_IServices _Common_IServices, DPOList_ISERVICE _DPOList_ISERVICE, DPODetail_ISERVICE _DPODetail_ISERVICE)
        {
            this._DPOList_ISERVICE = _DPOList_ISERVICE;
            this._DPODetail_ISERVICE = _DPODetail_ISERVICE;
            this._Common_IServices = _Common_IServices;
        }

        // GET: ApplicationLayer/DPO
        public ActionResult DPOList(DPOListModel _DPOListModel)//done
        {
            try
            {
                TempData["DocumentMenuId"] = "105101130";
                DocumentMenuId = "105101130";
                CommonPageDetails();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                _DPOListModel.DocumentMenuId = DocumentMenuId;
                _DPOListModel.GstApplicable = ViewBag.GstApplicable;
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _DPOListModel.WF_status = TempData["WF_status"].ToString();
                    if (_DPOListModel.WF_status != null)
                    {
                        _DPOListModel.wfstatus = _DPOListModel.WF_status;
                    }
                    else
                    {
                        _DPOListModel.wfstatus = "";
                    }
                }
                else
                {
                    if (_DPOListModel.WF_status != null)
                    {
                        _DPOListModel.wfstatus = _DPOListModel.WF_status;
                    }
                    else
                    {
                        _DPOListModel.wfstatus = "";
                    }
                }
                if (DocumentMenuId != null)
                {
                    _DPOListModel.wfdocid = DocumentMenuId;
                }
                else
                {
                    _DPOListModel.wfdocid = "0";
                }
                //DateTime dtnow = DateTime.Now;     /*Commented by by nitesh 21072025*/
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _DPOListModel.SuppID = a[0].Trim();
                    _DPOListModel.PO_FromDate = a[1].Trim();
                    _DPOListModel.PO_ToDate = a[2].Trim();
                    _DPOListModel.Status = a[3].Trim();
                    _DPOListModel.Itemtype = a[4].Trim();
                    if (_DPOListModel.Status == "0")
                    {
                        _DPOListModel.Status = null;
                    }
                    _DPOListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    //_DPOListModel.ToDate =Convert.ToDateTime(_DPOListModel.PO_ToDate);
                }

                if (_DPOListModel.PO_FromDate == null)
                {
                    _DPOListModel.FromDate = startDate;
                    _DPOListModel.PO_FromDate = startDate;
                    _DPOListModel.PO_ToDate = CurrentDate;
                }
                else
                {
                    _DPOListModel.FromDate = _DPOListModel.PO_FromDate;
                }
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _DPOListModel.StatusList = statusLists;
                _DPOListModel.Title = title;
                _DPOListModel.DPOSearch = "0";
                GetAllData(_DPOListModel, "D");/**Get Data Supplier And List Table Data in One Procedure**/
                //ViewBag.VBRoleList = GetRoleList();
                _DPOListModel.Pending_DocumentList = null;
                return View("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPOList.cshtml", _DPOListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult IPOList(DPOListModel _DPOListModel)//done
        {
            try
            {
                DocumentMenuId = "105101140101";
                CommonPageDetails();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                _DPOListModel.DocumentMenuId = DocumentMenuId;
                //DPOListModel _DPOListModel = new DPOListModel();
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _DPOListModel.WF_status = TempData["WF_status"].ToString();
                    if (_DPOListModel.WF_status != null)
                    {
                        _DPOListModel.wfstatus = _DPOListModel.WF_status;
                    }
                    else
                    {
                        _DPOListModel.wfstatus = "";
                    }
                }
                else
                {
                    if (_DPOListModel.WF_status != null)
                    {
                        _DPOListModel.wfstatus = _DPOListModel.WF_status;
                    }
                    else
                    {
                        _DPOListModel.wfstatus = "";
                    }
                }
                if (DocumentMenuId != null)
                {
                    _DPOListModel.wfdocid = DocumentMenuId;
                }
                else
                {
                    _DPOListModel.wfdocid = "0";
                }
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _DPOListModel.SuppID = a[0].Trim();
                    _DPOListModel.PO_FromDate = a[1].Trim();
                    _DPOListModel.PO_ToDate = a[2].Trim();
                    _DPOListModel.Status = a[3].Trim();
                    if (_DPOListModel.Status == "0")
                    {
                        _DPOListModel.Status = null;
                    }
                    _DPOListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    //_DPOListModel.ToDate =Convert.ToDateTime(_DPOListModel.PO_ToDate);
                }
                /*Commented by Nitesh 27-03-2024*/
                //var SuppType = "I";
                //_DPOListModel.SrcType = SuppType;
                //GetAutoCompleteSearchSuppList(_DPOListModel, SuppType);
                //_DPOListModel.DPOList = getLPOList(_DPOListModel, SuppType);
                if (_DPOListModel.PO_FromDate == null)
                {
                    _DPOListModel.FromDate = startDate;
                    _DPOListModel.PO_FromDate = startDate;
                    _DPOListModel.PO_ToDate = CurrentDate;
                }
                else
                {
                    _DPOListModel.FromDate = _DPOListModel.PO_FromDate;
                }
                //_DPOListModel.FromDate = startDate;
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                //ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _DPOListModel.StatusList = statusLists;

                GetAllData(_DPOListModel, "I");/**Get Data Supplier And List Table Data in One Procedure**/
                _DPOListModel.Title = title;
                _DPOListModel.DPOSearch = "0";
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPOList.cshtml", _DPOListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult CPOList(DPOListModel _DPOListModel)//done
        {
            try
            {
                DocumentMenuId = "105101136";
                CommonPageDetails();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                _DPOListModel.DocumentMenuId = DocumentMenuId;
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _DPOListModel.WF_status = TempData["WF_status"].ToString();
                    if (_DPOListModel.WF_status != null)
                    {
                        _DPOListModel.wfstatus = _DPOListModel.WF_status;
                    }
                    else
                    {
                        _DPOListModel.wfstatus = "";
                    }
                }
                else
                {
                    if (_DPOListModel.WF_status != null)
                    {
                        _DPOListModel.wfstatus = _DPOListModel.WF_status;
                    }
                    else
                    {
                        _DPOListModel.wfstatus = "";
                    }
                }
                if (DocumentMenuId != null)
                {
                    _DPOListModel.wfdocid = DocumentMenuId;
                }
                else
                {
                    _DPOListModel.wfdocid = "0";
                }
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _DPOListModel.SuppID = a[0].Trim();
                    _DPOListModel.PO_FromDate = a[1].Trim();
                    _DPOListModel.PO_ToDate = a[2].Trim();
                    _DPOListModel.Status = a[3].Trim();
                    if (_DPOListModel.Status == "0")
                    {
                        _DPOListModel.Status = null;
                    }
                    _DPOListModel.ListFilterData = TempData["ListFilterData"].ToString();
                }
                if (_DPOListModel.PO_FromDate == null)
                {
                    _DPOListModel.FromDate = startDate;
                    _DPOListModel.PO_FromDate = startDate;
                    _DPOListModel.PO_ToDate = CurrentDate;
                }
                else
                {
                    _DPOListModel.FromDate = _DPOListModel.PO_FromDate;
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _DPOListModel.StatusList = statusLists;

                GetAllData(_DPOListModel, "D");/**Get Data Supplier And List Table Data in One Procedure**/
                _DPOListModel.Title = title;
                _DPOListModel.DPOSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPOList.cshtml", _DPOListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(DPOListModel _DPOListModel, string SuppType)
        {
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string SupplierName = string.Empty;
            string User_ID = string.Empty;
            //var SuppType = "I";
            _DPOListModel.SrcType = SuppType;
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
                User_ID = Session["UserId"].ToString();
            }
            if (string.IsNullOrEmpty(_DPOListModel.SuppName))
            {
                SupplierName = "0";
            }
            else
            {
                SupplierName = _DPOListModel.SuppName;
            }
            SuppType = _DPOListModel.SrcType;
            DataSet CustList = _DPOList_ISERVICE.GetAllData(Comp_ID, SupplierName, Br_ID, SuppType, User_ID
                , _DPOListModel.SuppID, _DPOListModel.PO_FromDate, _DPOListModel.PO_ToDate, _DPOListModel.Status, _DPOListModel.wfdocid, _DPOListModel.wfstatus, _DPOListModel.Itemtype);

            List<SupplierName> _SuppList = new List<SupplierName>();
            foreach (DataRow data in CustList.Tables[0].Rows)
            {
                SupplierName _SuppDetail = new SupplierName();
                _SuppDetail.supp_id = data["supp_id"].ToString();
                _SuppDetail.supp_name = data["supp_name"].ToString();
                _SuppList.Add(_SuppDetail);
            }
            _SuppList.Insert(0, new SupplierName() { supp_id = "0", supp_name = "All" });
            _DPOListModel.SupplierNameList = _SuppList;
            //GetAutoCompleteSearchSuppList(_DPOListModel, SuppType);
            // _DPOListModel.DPOList = getLPOList(_DPOListModel, SuppType);
            SetDataList(CustList, _DPOListModel);
        }
        private void SetDataList(DataSet DSet, DPOListModel _DPOListModel)
        {
            List<DomesticPurchaseOrderList> _DomesticPurchaseOrderList = new List<DomesticPurchaseOrderList>();
            if (DSet.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in DSet.Tables[1].Rows)
                {
                    DomesticPurchaseOrderList _DPOList = new DomesticPurchaseOrderList();
                    _DPOList.OrderNo = dr["OrderNo"].ToString();
                    _DPOList.OrderDate = dr["OrderDate"].ToString();
                    _DPOList.OrderDt = dr["OrderDt"].ToString();
                    _DPOList.OrderType = dr["OrderType"].ToString();
                    _DPOList.SourceType = dr["SourceType"].ToString();
                    _DPOList.req_area = dr["req_area"].ToString();
                    _DPOList.SourceDocNo = dr["src_doc_number"].ToString();
                    _DPOList.SuppName = dr["supp_name"].ToString();
                    _DPOList.supp_id = dr["supp_id"].ToString();
                    _DPOList.Currency = dr["curr"].ToString();
                    _DPOList.OrderValue = dr["net_val_bs"].ToString();
                    _DPOList.OrderStauts = dr["OrderStauts"].ToString();
                    _DPOList.CreateDate = dr["CreateDate"].ToString();
                    _DPOList.ApproveDate = dr["ApproveDate"].ToString();
                    _DPOList.ModifyDate = dr["ModifyDate"].ToString();
                    _DPOList.create_by = dr["create_by"].ToString();
                    _DPOList.app_by = dr["app_by"].ToString();
                    _DPOList.mod_by = dr["mod_by"].ToString();
                    _DPOList.item_type = dr["item_type"].ToString();
                    _DPOList.imp_file_no = dr["imp_file_no"].ToString();
                    _DPOList.country_origin = dr["cntry_origin_name"].ToString();
                    _DomesticPurchaseOrderList.Add(_DPOList);
                }
            }

            //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
            ViewBag.FinStDt = DSet.Tables[3].Rows[0]["findate"];
            _DPOListModel.DPOList = _DomesticPurchaseOrderList;
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            var WF_status1 = "";
            //Session["Message"] = "";
            PODetailsModel _Model = new PODetailsModel();
            var a = TrancType.Split(',');
            _Model.PO_No = a[0].Trim();
            _Model.PO_Date = a[1].Trim();
            _Model.TransType = a[2].Trim();
            if (a[3].Trim() != "" && a[3].Trim() != null)
            {
                WF_status1 = a[3].Trim();
                _Model.WF_status1 = WF_status1;
            }
            var docId = a[4].Trim();
            _Model.Message = Mailerror;
            _Model.BtnName = "BtnToDetailPage";
            _Model.DocumentMenuId = docId;
            TempData["ModelData"] = _Model;
            TempData["WF_status1"] = WF_status1; ;
            TempData["ListFilterData"] = ListFilterData1;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.DocNo = a[0].Trim();
            URLModel.DocDate = a[01].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.DocumentMenuId = docId;
            URLModel.BtnName = "BtnToDetailPage";
            return RedirectToAction("DPODetail", URLModel);
        }
        private void CommonPageDetails()
        {
            try
            {
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
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
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
        //private DataTable GetRoleList()//done
        //{
        //    try
        //    {
        //        if (Session["MenuDocumentId"] != null)
        //        {
        //            if (Session["MenuDocumentId"].ToString() == "105101130")
        //            {
        //                DocumentMenuId = "105101130";
        //            }
        //            if (Session["MenuDocumentId"].ToString() == "105101140101")
        //            {
        //                DocumentMenuId = "105101140101";
        //            }
        //        }               
        //        string UserID = "";
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            UserID = Session["userid"].ToString();
        //        }
        //        DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

        //        return RoleList;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
        public ActionResult DPODetail(URLDetailModel URLModel)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            /*Add by Hina sharma on 02-05-2025 to check Existing with previous year transaction*/
            //var PODate = Convert.ToDateTime(URLModel.DocDate).ToString("yyyy-MM-dd");
            ////var PODate = URLModel.DocDate.Split('-').Reverse().Join('-');
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PODate) == "TransNotAllow")
            //{
            //    //TempData["Message2"] = "TransNotAllow";
            //    ViewBag.Message = "TransNotAllow";
            //}
            try
            {

                var _Model = TempData["ModelData"] as PODetailsModel;
                if (_Model != null)
                {
                    if (URLModel.DocumentMenuId != null)
                    {
                        DocumentMenuId = URLModel.DocumentMenuId;
                    }
                    else
                    {
                        DocumentMenuId = _Model.DocumentMenuId;
                    }

                    _Model.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.DocumentStatus = "D";
                    _Model.ILSearch = null;
                    if (DocumentMenuId == "105101140101")
                    {
                        _Model.SrcType = "I";
                    }
                    else
                    {
                        _Model.SrcType = "D";
                    }
                    //string ValDigit = ToFixDecimal(Convert.ToInt32((_Model.SrcType == "I" ? Session["ExpImpValDigit"] :Session["ValDigit"]).ToString()));
                    //string QtyDigit = ToFixDecimal(Convert.ToInt32((_Model.SrcType == "I" ? Session["ExpImpQtyDigit"] : Session["QtyDigit"]).ToString()));
                    //string RateDigit = ToFixDecimal(Convert.ToInt32((_Model.SrcType == "I" ? Session["ExpImpRateDigit"] : Session["RateDigit"]).ToString()));

                    DataTable dt = new DataTable();
                    List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                    dt = GetRequirmentreaList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        RequirementAreaList _RAList = new RequirementAreaList();
                        _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                        _RAList.req_val = dr["setup_val"].ToString();
                        requirementAreaLists.Add(_RAList);
                    }
                    requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "---Select---" });
                    _Model._requirementAreaLists = requirementAreaLists;


                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _Model.GstApplicable = ViewBag.GstApplicable;

                    //_Model.ValDigit = ValDigit;
                    //_Model.QtyDigit = QtyDigit;
                    //_Model.RateDigit = RateDigit;

                    //ViewBag.ValDigit = ValDigit;
                    //ViewBag.QtyDigit = QtyDigit;
                    //ViewBag.RateDigit = RateDigit;

                    SetDecimals(_Model);

                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _Model.SupplierNameList = suppLists;
                    List<CurrancyList> currancyLists = new List<CurrancyList>();
                    currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                    _Model.currancyLists = currancyLists;

                    List<CountryOfOrigin> CountryofList = new List<CountryOfOrigin>();
                    CountryofList.Add(new CountryOfOrigin { cntry_id = "0", cntry_name = "---Select---" });
                    _Model.countryOfOrigins = CountryofList;

                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _Model.docNoLists = srcDocNoLists;


                    List<trade_termList> _TermLists = new List<trade_termList>();
                    _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "0", TrdTrms_val = "---Select---" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    _Model.TradeTermsList = _TermLists;

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _Model.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (TempData["DocumentMenuId"] != null && TempData["DocumentMenuId"].ToString() != "")
                    {
                        _Model.DocumentMenuId = TempData["DocumentMenuId"].ToString();
                    }
                    if (_Model.PO_No != null && _Model.PO_Date != null)
                    {
                        string Doc_no = _Model.PO_No;
                        string Doc_date = _Model.PO_Date;
                        DataSet ds = GetPODetailEdit(Doc_no, Doc_date, DocumentMenuId);
                        ViewBag.UOMList = ds.Tables[16];
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //_Model.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                            _Model.Pymnt_trms = ds.Tables[0].Rows[0]["paym_terms"].ToString();
                            _Model.Del_dstn = ds.Tables[0].Rows[0]["Delv_destn"].ToString();
                            _Model.ShipTo = ds.Tables[0].Rows[0]["ship_to"].ToString();/*Add by Hina on 20-09-2024 */
                            _Model.POOrderType = ds.Tables[0].Rows[0]["order_type"].ToString();
                            _Model.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();
                            if (_Model.Src_Type == "P") _Model.Src_Type = "PR";
                            _Model.PO_No = ds.Tables[0].Rows[0]["po_no"].ToString();
                            _Model.PO_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["po_dt"].ToString()).ToString("yyyy-MM-dd");
                            _Model.Remarks = ds.Tables[0].Rows[0]["po_rem"].ToString();
                            _Model.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _Model.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _Model.SuppID, supp_name = _Model.SuppName });
                            _Model.SupplierNameList = suppLists;
                            _Model.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            //_Model.SuppName = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _Model.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bill_add_id"].ToString());
                            _Model.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _Model.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            string Curr_name = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _Model.Currency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            _Model.bs_curr_id = ds.Tables[0].Rows[0]["bs_curr_id"].ToString();
                            currancyLists.Add(new CurrancyList { curr_id = _Model.Currency, curr_name = Curr_name });
                            _Model.currancyLists = currancyLists;

                            string CntryOfOrg = ds.Tables[0].Rows[0]["country_name"].ToString();
                            if (CntryOfOrg == "") { CntryOfOrg = "---Select---"; }
                            _Model.CntryOrigin = ds.Tables[0].Rows[0]["cntry_origin"].ToString();
                            CountryofList.Add(new CountryOfOrigin { cntry_id = _Model.CntryOrigin, cntry_name = CntryOfOrg });
                            _Model.countryOfOrigins = CountryofList;
                            _Model.PortOrigin = ds.Tables[0].Rows[0]["port_origin"].ToString();
                            _Model.ImpFileNo = ds.Tables[0].Rows[0]["imp_file_no"].ToString();
                            _Model.trade_term = ds.Tables[0].Rows[0]["trade_terms"].ToString();
                            _Model.PriceBasis = ds.Tables[0].Rows[0]["price_basis"].ToString();
                            _Model.FreightType = ds.Tables[0].Rows[0]["freight_type"].ToString();
                            _Model.ModeOfTransport = ds.Tables[0].Rows[0]["mode_trans"].ToString();
                            _Model.Destination = ds.Tables[0].Rows[0]["dest"].ToString();


                            _Model.Conv_Rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(_Model.ExchDigit); ;
                            _Model.ValidUpto = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                            _Model.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(_Model.ValDigit);
                            _Model.AssessableVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(_Model.ValDigit);
                            _Model.DiscAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["disc_amt"]).ToString(_Model.ValDigit);
                            _Model.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(_Model.ValDigit);
                            _Model.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(_Model.ValDigit);
                            _Model.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_spec"]).ToString(_Model.ValDigit);
                            _Model.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(_Model.ValDigit);
                            _Model.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _Model.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _Model.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _Model.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _Model.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _Model.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _Model.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _Model.StatusName = ds.Tables[0].Rows[0]["OrderStauts"].ToString();
                            _Model.OrdStatus = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                            _Model.Item_type = ds.Tables[0].Rows[0]["item_type"].ToString().Trim(); // added By Nitesh 17-11-2023 for toggle item type
                            _Model.hdn_item_typ = ds.Tables[0].Rows[0]["item_type"].ToString().Trim(); // added By Nitesh 17-11-2023 for toggle item type
                            _Model.ContectPersone = ds.Tables[0].Rows[0]["cont_pers"].ToString().Trim(); // added By Nitesh 09-08-2024 for Contect Deatil
                            _Model.ContectNumber = ds.Tables[0].Rows[0]["cont_num"].ToString().Trim(); // added By Nitesh 09-08-2024 for Contect Deatil
                            _Model.req_area = ds.Tables[0].Rows[0]["req_area"].ToString().Trim(); // added By Nitesh 09-08-2024 for Contect Deatil


                            _Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);//Cancelled
                            _Model.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _Model.SrcDocNo, SrcDocnoVal = _Model.SrcDocNo });
                            _Model.docNoLists = srcDocNoLists;
                            _Model.ForAmmendendBtn = ds.Tables[12].Rows[0]["flag"].ToString();
                            _Model.Amendment = ds.Tables[13].Rows[0]["Amendment"].ToString();
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _Model.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            ViewBag.ItemDelSchdetails = ds.Tables[4];
                            ViewBag.ItemTermsdetails = ds.Tables[5];
                            create_id = ds.Tables[0].Rows[0]["createid"].ToString();   //

                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.OCTaxDetails = ds.Tables[15];
                            ViewBag.AttechmentDetails = ds.Tables[11];
                            ViewBag.SubItemDetails = ds.Tables[14];
                            ViewBag.QtyDigit = _Model.QtyDigit;
                            _Model.FCFlag= ds.Tables[17].Rows[0]["FCFlag"].ToString().Trim();
                        }
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string Statuscode = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _Model.Cancelled = true;
                            _Model.CancelledRemarks = ds.Tables[0].Rows[0]["Cancel_remarks"].ToString();
                        }
                        else if (Statuscode == "FC")
                        {
                            _Model.FClosed = true;
                        }
                        else
                        {
                            _Model.Cancelled = false;
                        }
                        string ForceClose = ds.Tables[0].Rows[0]["force_close"].ToString().Trim();
                        _Model.DocumentStatus = Statuscode;
                        ViewBag.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }
                        if (ViewBag.AppLevel != null && _Model.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[6].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[6].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[7].Rows[0]["nextlevel"].ToString().Trim();
                            }
                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _Model.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _Model.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _Model.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _Model.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            else if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _Model.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            else if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _Model.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _Model.BtnName = "BtnRefresh";
                                }
                            }
                            else
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _Model.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _Model.BtnName = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }
                    if (_Model.Amend == "Amend")
                    {
                        _Model.OrdStatus = "D";
                        _Model.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                    }

                    _Model.DocumentMenuId = DocumentMenuId;
                    _Model.Title = title;
                    _Model.UserID = UserID;

                    if (_Model.Amendment != "Amendment" && _Model.Amendment != "" && _Model.Amendment != null)
                    {
                        _Model.BtnName = "BtnRefresh";
                        _Model.wfDisableAmnd = "wfDisableAmnd";
                    }
                    return View("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPODetail.cshtml", _Model);
                }
                else
                {/*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/

                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    PODetailsModel _Model1 = new PODetailsModel();
                    DocumentMenuId = URLModel.DocumentMenuId;
                    _Model1.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.DocumentStatus = "D";
                    _Model1.ILSearch = null;
                    if (DocumentMenuId == "105101140101")
                    {
                        _Model1.SrcType = "I";
                    }
                    else
                    {
                        _Model1.SrcType = "D";
                    }

                    DataTable dt = new DataTable();
                    List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                    dt = GetRequirmentreaList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        RequirementAreaList _RAList = new RequirementAreaList();
                        _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                        _RAList.req_val = dr["setup_val"].ToString();
                        requirementAreaLists.Add(_RAList);
                    }
                    requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "---Select---" });
                    _Model1._requirementAreaLists = requirementAreaLists;

                    //string ValDigit = ToFixDecimal(Convert.ToInt32((_Model1.SrcType == "I" ? Session["ExpImpValDigit"] : Session["ValDigit"]).ToString()));
                    //string QtyDigit = ToFixDecimal(Convert.ToInt32((_Model1.SrcType == "I" ? Session["ExpImpQtyDigit"] : Session["QtyDigit"]).ToString()));
                    //string RateDigit = ToFixDecimal(Convert.ToInt32((_Model1.SrcType == "I" ? Session["ExpImpRateDigit"] : Session["RateDigit"]).ToString()));
                    //string ExchRateDigit = ToFixDecimal(Convert.ToInt32(Session["ExchDigit"].ToString()));
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _Model1.GstApplicable = ViewBag.GstApplicable;

                    //_Model1.ValDigit = ValDigit;
                    //_Model1.QtyDigit = QtyDigit;
                    //_Model1.RateDigit = RateDigit;
                    //_Model1.ExchRateDigit = ExchRateDigit;
                    //ViewBag.ValDigit = ValDigit;
                    //ViewBag.QtyDigit = QtyDigit;
                    //ViewBag.RateDigit = RateDigit;
                    //ViewBag.ExchRateDigit = ExchRateDigit;

                    SetDecimals(_Model1);

                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _Model1.SupplierNameList = suppLists;
                    List<CurrancyList> currancyLists = new List<CurrancyList>();
                    currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                    _Model1.currancyLists = currancyLists;

                    List<CountryOfOrigin> CountryofList = new List<CountryOfOrigin>();
                    CountryofList.Add(new CountryOfOrigin { cntry_id = "0", cntry_name = "---Select---" });
                    _Model1.countryOfOrigins = CountryofList;

                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _Model1.docNoLists = srcDocNoLists;


                    List<trade_termList> _TermLists = new List<trade_termList>();
                    _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "0", TrdTrms_val = "---Select---" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    _Model1.TradeTermsList = _TermLists;

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _Model1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (TempData["DocumentMenuId"] != null && TempData["DocumentMenuId"].ToString() != "")
                    {
                        _Model1.DocumentMenuId = TempData["DocumentMenuId"].ToString();
                    }
                    if (URLModel.DocNo != null || URLModel.DocDate != null)
                    {
                        _Model1.PO_No = URLModel.DocNo;
                        _Model1.PO_Date = URLModel.DocDate;
                    }
                    if (URLModel.TransType != null)
                    {
                        _Model1.TransType = URLModel.TransType;
                    }
                    if (URLModel.BtnName != null)
                    {
                        _Model1.BtnName = URLModel.BtnName;
                    }
                    if (URLModel.Command != null)
                    {
                        _Model1.Command = URLModel.Command;
                    }
                    if (_Model1.PO_No != null && _Model1.PO_Date != null)
                    {
                        string Doc_no = _Model1.PO_No;
                        string Doc_date = _Model1.PO_Date;
                        DataSet ds = GetPODetailEdit(Doc_no, Doc_date, DocumentMenuId);
                        ViewBag.UOMList = ds.Tables[16];
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            _Model1.Pymnt_trms = ds.Tables[0].Rows[0]["paym_terms"].ToString();
                            _Model1.Del_dstn = ds.Tables[0].Rows[0]["Delv_destn"].ToString();
                            _Model1.ShipTo = ds.Tables[0].Rows[0]["ship_to"].ToString();/*Add by Hina on 20-09-2024 */
                            _Model1.POOrderType = ds.Tables[0].Rows[0]["order_type"].ToString();
                            _Model1.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();
                            if (_Model1.Src_Type == "P") _Model1.Src_Type = "PR";
                            _Model1.PO_No = ds.Tables[0].Rows[0]["po_no"].ToString();
                            _Model1.PO_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["po_dt"].ToString()).ToString("yyyy-MM-dd");
                            _Model1.Remarks = ds.Tables[0].Rows[0]["po_rem"].ToString();
                            _Model1.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _Model1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _Model1.SuppID, supp_name = _Model1.SuppName });
                            _Model1.SupplierNameList = suppLists;
                            _Model1.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            //_Model1.SuppName = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _Model1.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bill_add_id"].ToString());
                            _Model1.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _Model1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            string Curr_name = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _Model1.Currency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            _Model1.bs_curr_id = ds.Tables[0].Rows[0]["bs_curr_id"].ToString();
                            currancyLists.Add(new CurrancyList { curr_id = _Model1.Currency, curr_name = Curr_name });
                            _Model1.currancyLists = currancyLists;

                            string CntryOfOrg = ds.Tables[0].Rows[0]["country_name"].ToString();
                            if (CntryOfOrg == "") { CntryOfOrg = "---Select---"; }
                            _Model1.CntryOrigin = ds.Tables[0].Rows[0]["cntry_origin"].ToString();
                            CountryofList.Add(new CountryOfOrigin { cntry_id = _Model1.CntryOrigin, cntry_name = CntryOfOrg });
                            _Model1.countryOfOrigins = CountryofList;
                            _Model1.PortOrigin = ds.Tables[0].Rows[0]["port_origin"].ToString();
                            _Model1.ImpFileNo = ds.Tables[0].Rows[0]["imp_file_no"].ToString();
                            _Model1.trade_term = ds.Tables[0].Rows[0]["trade_terms"].ToString();
                            _Model1.PriceBasis = ds.Tables[0].Rows[0]["price_basis"].ToString();
                            _Model1.FreightType = ds.Tables[0].Rows[0]["freight_type"].ToString();
                            _Model1.ModeOfTransport = ds.Tables[0].Rows[0]["mode_trans"].ToString();
                            _Model1.Destination = ds.Tables[0].Rows[0]["dest"].ToString();
                            _Model1.req_area = ds.Tables[0].Rows[0]["req_area"].ToString();


                            _Model1.Conv_Rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(_Model1.ExchDigit); ;
                            _Model1.ValidUpto = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                            _Model1.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(_Model1.ValDigit);
                            _Model1.AssessableVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(_Model1.ValDigit);
                            _Model1.DiscAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["disc_amt"]).ToString(_Model1.ValDigit);
                            _Model1.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(_Model1.ValDigit);
                            _Model1.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(_Model1.ValDigit);
                            _Model1.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_spec"]).ToString(_Model1.ValDigit);
                            _Model1.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(_Model1.ValDigit);
                            _Model1.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _Model1.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _Model1.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _Model1.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _Model1.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _Model1.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _Model1.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _Model1.StatusName = ds.Tables[0].Rows[0]["OrderStauts"].ToString();
                            _Model1.OrdStatus = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                            _Model1.Item_type = ds.Tables[0].Rows[0]["item_type"].ToString().Trim(); // added By Nitesh 17-11-2023 for toggle item type
                            _Model1.hdn_item_typ = ds.Tables[0].Rows[0]["item_type"].ToString().Trim(); // added By Nitesh 17-11-2023 for toggle item type
                            _Model1.ContectPersone = ds.Tables[0].Rows[0]["cont_pers"].ToString().Trim(); // added By Nitesh 09-08-2024 for Contect Deatil
                            _Model1.ContectNumber = ds.Tables[0].Rows[0]["cont_num"].ToString().Trim(); // added By Nitesh 09-08-2024 for Contect Deatil
                            _Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);//Cancelled
                            _Model1.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _Model1.SrcDocNo, SrcDocnoVal = _Model1.SrcDocNo });
                            _Model1.docNoLists = srcDocNoLists;
                            _Model1.ForAmmendendBtn = ds.Tables[12].Rows[0]["flag"].ToString();
                            _Model1.Amendment = ds.Tables[13].Rows[0]["Amendment"].ToString();
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _Model1.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            ViewBag.ItemDelSchdetails = ds.Tables[4];
                            ViewBag.ItemTermsdetails = ds.Tables[5];
                            create_id = ds.Tables[0].Rows[0]["createid"].ToString();   //

                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.OCTaxDetails = ds.Tables[15];
                            ViewBag.AttechmentDetails = ds.Tables[11];
                            ViewBag.SubItemDetails = ds.Tables[14];
                            ViewBag.QtyDigit = _Model1.QtyDigit;
                            _Model1.FCFlag = ds.Tables[17].Rows[0]["FCFlag"].ToString().Trim();
                        }
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string Statuscode = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _Model1.Cancelled = true;
                            _Model1.CancelledRemarks = ds.Tables[0].Rows[0]["Cancel_remarks"].ToString();
                        }
                        else if (Statuscode == "FC")
                        {
                            _Model1.FClosed = true;
                        }
                        else
                        {
                            _Model1.Cancelled = false;
                        }
                        string ForceClose = ds.Tables[0].Rows[0]["force_close"].ToString().Trim();
                        _Model1.DocumentStatus = Statuscode;
                        ViewBag.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }
                        if (ViewBag.AppLevel != null && _Model1.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[6].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[6].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[7].Rows[0]["nextlevel"].ToString().Trim();
                            }
                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _Model1.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";

                                            /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _Model1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _Model1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _Model1.BtnName = "BtnToDetailPage";

                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _Model1.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            else if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _Model1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            else
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _Model1.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _Model1.BtnName = "BtnRefresh";
                                }
                            }

                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                    }
                    _Model1.DocumentMenuId = DocumentMenuId;
                    if (_Model1.BtnName == null)
                    {
                        _Model1.BtnName = "BtnRefresh";
                    }
                    if (_Model1.TransType == null)
                    {
                        _Model1.TransType = "New";
                        _Model1.Command = "Refresh";
                    }
                    if (URLModel.Amend == "Amend")
                    {
                        _Model1.Amend = URLModel.Amend;
                        _Model1.OrdStatus = "D";
                        _Model1.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                    }

                    _Model1.Title = title;
                    _Model1.UserID = UserID;
                    if (_Model1.Amendment != "Amendment" && _Model1.Amendment != "" && _Model1.Amendment != null)
                    {
                        _Model1.BtnName = "BtnRefresh";
                        _Model1.wfDisableAmnd = "wfDisableAmnd";
                    }
                    return View("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPODetail.cshtml", _Model1);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        public ActionResult AddNewDPO(string DocumentMenuId)
        {
            /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                DPOListModel objModel = new DPOListModel();
                objModel.Message = "Financial Year not Exist";
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("DPOList", objModel);
            }
            /*End to chk Financial year exist or not*/
            //Session["Message"] = null;
            //Session["Command"] = "Add";
            //Session["PO_No"] = null;
            //Session["PO_Date"] = null;
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            URLDetailModel URLModel = new URLDetailModel();
            PODetailsModel _Model = new PODetailsModel();
            TempData["ListFilterData"] = null;
            _Model.Message = null;
            _Model.Command = "Add";
            _Model.PO_No = null;
            _Model.PO_Date = null;
            _Model.TransType = "Save";
            _Model.BtnName = "BtnAddNew";
            ViewBag.DocumentStatus = "D";
            TempData["ModelData"] = _Model;
            URLModel.DocumentMenuId = DocumentMenuId;
            URLModel.TransType = "Save";
            URLModel.BtnName = "BtnAddNew";
            URLModel.Command = "Add";
            URLModel.DocDate = null;
            URLModel.DocNo = null;

            return RedirectToAction("DPODetail", "DPO", URLModel);
        }
        public ActionResult DoubleClickOnList(string DocNo, string DocDate, string ListFilterData, string DocumentMenuId, string WF_status, string ListItem_type)
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["TransType"] = "Update";
            //Session["BtnName"] = "BtnToDetailPage";
            //Session["PO_No"] = DocNo;
            //Session["PO_Date"] = DocDate;

            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/

            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (ListItem_type == "Consumable")
            {
                TempData["DocumentMenuId"] = DocumentMenuId;
                DocumentMenuId = "105101136";
            }
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            URLDetailModel URLModel = new URLDetailModel();
            PODetailsModel _Model = new PODetailsModel();
            TempData["ListFilterData"] = ListFilterData;
            _Model.Message = "New";
            _Model.Command = "Update";
            _Model.TransType = "Update";
            _Model.BtnName = "BtnToDetailPage";
            _Model.PO_No = DocNo;
            _Model.PO_Date = DocDate;
            _Model.WF_status1 = WF_status;
            TempData["ModelData"] = _Model;
            URLModel.DocNo = DocNo;
            URLModel.DocDate = DocDate;
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.Command = "Update";
            URLModel.DocumentMenuId = DocumentMenuId;
            return RedirectToAction("DPODetail", "DPO", URLModel);
        }
        //private string getDocumentName()
        //{
        //    try
        //    {
        //        if (Session["MenuDocumentId"] != null)
        //        {
        //            if (Session["MenuDocumentId"].ToString() == "105101130")
        //            {
        //                DocumentMenuId = "105101130";
        //            }
        //            if (Session["MenuDocumentId"].ToString() == "105101140101")
        //            {
        //                DocumentMenuId = "105101140101";
        //            }
        //        }
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["Language"] != null)
        //        {
        //            language = Session["Language"].ToString();
        //        }
        //        string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
        //        string[] Docpart = DocumentName.Split('>');
        //        int len = Docpart.Length;
        //        if (len > 1)
        //        {
        //            title = Docpart[len - 1].Trim();
        //        }
        //        return DocumentName;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }    
        //}
        public ActionResult PODetailsActions(PODetailsModel _Model, string Command)
        {
            try
            {/*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                //PODetailsModel _Model = new PODetailsModel();
                if (_Model.Delete == "Delete")
                {
                    Command = "Delete";
                    // DeletePQDetails(_Model);
                }
                switch (Command)
                {
                    case "AddNew":
                        TempData["ListFilterData"] = null;
                        URLDetailModel URLModel = new URLDetailModel();
                        PODetailsModel _ModelAddNew = new PODetailsModel();
                        _ModelAddNew.Command = "Add";
                        _ModelAddNew.TransType = "Save";
                        _ModelAddNew.BtnName = "BtnAddNew";
                        _ModelAddNew.DocumentMenuId = _Model.DocumentMenuId;
                        ViewBag.DocumentStatus = "D";
                        TempData["ModelData"] = _ModelAddNew;
                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_Model.PO_No))
                                return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.PO_No, DocDate = _Model.PO_Date, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus });
                            else
                                _ModelAddNew.Command = "Refresh";
                            _ModelAddNew.TransType = "Refresh";
                            _ModelAddNew.BtnName = "BtnRefresh";
                            _ModelAddNew.DocumentStatus = null;
                            TempData["ModelData"] = _ModelAddNew;
                            return RedirectToAction("DPODetail", "DPO", _ModelAddNew);

                        }
                        /*End to chk Financial year exist or not*/
                        URLModel.DocumentMenuId = _Model.DocumentMenuId;
                        URLModel.TransType = "Save";
                        URLModel.BtnName = "BtnAddNew";
                        URLModel.Command = "Add";
                        URLModel.DocDate = null;
                        URLModel.DocNo = null;
                        //return RedirectToAction("AddNewDPO");
                        return RedirectToAction("DPODetail", "DPO", URLModel);
                    case "Save":
                        _Model.TransType = Command;
                        InsertPODetails(_Model);

                        if (_Model.Message == "DocModify")
                        {
                            //_Model.DocumentMenuId = _Model.DocumentMenuId;
                            DocumentMenuId = _Model.DocumentMenuId;
                            CommonPageDetails();
                            //ViewBag.DocID = _Model.DocumentMenuId;
                            ViewBag.DocumentMenuId = _Model.DocumentMenuId;
                            ViewBag.DocumentStatus = "D";
                            //_Model.ILSearch = null;
                            if (DocumentMenuId != "105101130")
                            {
                                _Model.SrcType = "I";
                            }
                            else
                            {
                                _Model.SrcType = "D";
                            }

                            DataTable dt = new DataTable();
                            List<SupplierName> suppLists = new List<SupplierName>();
                            suppLists.Add(new SupplierName { supp_id = _Model.SuppID, supp_name = _Model.SuppName });
                            _Model.SupplierNameList = suppLists;
                            List<CurrancyList> currancyLists = new List<CurrancyList>();
                            currancyLists.Add(new CurrancyList { curr_id = _Model.Currency, curr_name = _Model.Currency });
                            _Model.currancyLists = currancyLists;

                            List<CountryOfOrigin> CountryofList = new List<CountryOfOrigin>();
                            CountryofList.Add(new CountryOfOrigin { cntry_id = _Model.CountryId, cntry_name = _Model.CountryName });
                            _Model.countryOfOrigins = CountryofList;

                            List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                            srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = _Model.SrcDocNo });
                            _Model.docNoLists = srcDocNoLists;



                            List<trade_termList> _TermLists = new List<trade_termList>();
                            _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "0", TrdTrms_val = "---Select---" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                            _Model.TradeTermsList = _TermLists;

                            _Model.SuppName = _Model.SuppName;
                            _Model.Address = _Model.Address;
                            _Model.GrVal = _Model.GrVal;
                            _Model.TaxAmt = _Model.TaxAmt;
                            _Model.OcAmt = _Model.OcAmt;
                            _Model.NetValSpec = _Model.NetValSpec;
                            _Model.SrcDocNo = _Model.SrcDocNo;
                            _Model.SrcDocDate = _Model.SrcDocDate;
                            CommonPageDetails();
                            ViewBag.GstApplicable = ViewBag.GstApplicable;
                            ViewBag.ItemDetailsList = ViewData["ItemDetails"];
                            ViewBag.ItemDelSchdetails = ViewData["DelvScheDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];
                            ViewBag.ItemTaxDetailsList = ViewData["TaxDetails"];
                            ViewBag.OCTaxDetails = ViewData["OCTaxDetails"];
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemTermsdetails = ViewData["TrmAndConDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _Model.BtnName = "BtnRefresh";
                            _Model.Command = "Refresh";
                            _Model.DocumentStatus = "D";

                            SetDecimals(_Model);
                            _Model.Message = "Used";
                            //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            //string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            //string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            //ViewBag.ValDigit = ValDigit;
                            //ViewBag.QtyDigit = QtyDigit;
                            //ViewBag.RateDigit = RateDigit;
                            return View("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPODetail.cshtml", _Model);
                        }
                        else
                        {
                            TempData["ListFilterData"] = _Model.ListFilterData1;
                            _Model.TransType = "Update";
                            _Model.Command = Command;
                            _Model.BtnName = "BtnSave";
                            TempData["ModelData"] = _Model;
                            URLDetailModel URLModelSave = new URLDetailModel();
                            URLModelSave.DocumentMenuId = _Model.DocumentMenuId;
                            URLModelSave.TransType = "Update";
                            URLModelSave.BtnName = "BtnSave";
                            URLModelSave.Command = Command;
                            URLModelSave.DocDate = _Model.PO_Date;
                            URLModelSave.DocNo = _Model.PO_No;
                            TempData["ModelData"] = _Model;
                            //Session["TransType"] = "Update";
                            //Session["Command"] = Command;
                            //Session["BtnName"] = "BtnSave";
                            return RedirectToAction("DPODetail", URLModelSave);
                        }
                    case "Update":
                        _Model.TransType = Command;
                        if (_Model.Amend != null && _Model.Amend != "" && _Model.Amendment == null)
                        {
                            _Model.TransType = "Amendment";

                        }
                        InsertPODetails(_Model);
                        URLDetailModel URLModelUpdate = new URLDetailModel();
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        //Session["TransType"] = "Update";
                        //Session["Command"] = Command;
                        //Session["BtnName"] = "BtnSave";
                        _Model.TransType = "Update";
                        _Model.Command = Command;
                        _Model.BtnName = "BtnSave";
                        if (_Model.OrdStatus == "PDL" || _Model.OrdStatus == "PR" || _Model.OrdStatus == "PN")
                        {
                            //Session["BtnName"] = "BtnToDetailPage";
                            _Model.BtnName = "BtnToDetailPage";
                        }
                        URLModelUpdate.DocumentMenuId = _Model.DocumentMenuId;
                        URLModelUpdate.TransType = "Update";
                        URLModelUpdate.BtnName = _Model.BtnName;
                        URLModelUpdate.Command = Command;
                        URLModelUpdate.DocDate = _Model.PO_Date;
                        URLModelUpdate.DocNo = _Model.PO_No;
                        TempData["ModelData"] = _Model;
                        return RedirectToAction("DPODetail", URLModelUpdate);
                    case "Approve":
                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.PO_No, DocDate = _Model.PO_Date, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus });
                        //}
                        /*Commented and modify by Hina sharma on 02-05-2025 to check Existing with previous year transaction*/
                        //string PODate4 = _Model.PO_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PODate4) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.PO_No, DocDate = _Model.PO_Date, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        _Model.TransType = Command;
                        InsertPOApproveDetails(_Model, "");
                        URLDetailModel URLModelApprove = new URLDetailModel();
                        URLModelApprove.Command = Command;
                        URLModelApprove.BtnName = "BtnApprove";
                        URLModelApprove.TransType = _Model.TransType;
                        URLModelApprove.DocNo = _Model.PO_No;
                        URLModelApprove.DocDate = _Model.PO_Date;
                        URLModelApprove.DocumentMenuId = _Model.DocumentMenuId;
                        _Model.Command = Command;
                        _Model.BtnName = "BtnApprove";
                        TempData["ModelData"] = _Model;
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("DPODetail", URLModelApprove);
                    case "Edit":
                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (TempData["DocumentMenuId"] != null)
                        {
                            TempData["DocumentMenuId"] = TempData["DocumentMenuId"];
                        }
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.PO_No, DocDate = _Model.PO_Date, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus });
                        //}
                        /*Commented and modify by Hina sharma on 02-05-2025 to check Existing with previous year transaction*/
                        //string PODate = _Model.PO_Date;
                        ////string FResult = commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, PRDate);
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PODate) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.PO_No, DocDate = _Model.PO_Date, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus });

                        //}
                        /*End to chk Financial year exist or not*/
                        URLDetailModel URLModelEdit = new URLDetailModel();
                        if (_Model.OrdStatus == "A")
                        {
                            if (CheckDeliveryNoteAgainstDPO(_Model, _Model.PO_No, _Model.PO_Date))
                            {
                                TempData["ListFilterData"] = _Model.ListFilterData1;
                                //Session["Message"] = "deliverynoteunderprocess";
                                if ((_Model.Item_type == "C" || _Model.hdn_item_typ == "C") && _Model.Src_Type == "D")// Added By Nitesh 21-11-2023 Give msg when consumable invoice created
                                {
                                    _Model.Message = "Consumableinvoiceunderprocess";
                                }
                                else
                                {
                                    _Model.Message = "deliverynoteunderprocess";
                                }
                                _Model.BtnName = "BtnToDetailPage";
                                _Model.Command = "Add";
                                URLModelEdit.Command = "Add";
                                URLModelEdit.TransType = _Model.TransType;
                                URLModelEdit.BtnName = "BtnToDetailPage";
                                URLModelEdit.DocumentMenuId = _Model.DocumentMenuId;
                                URLModelEdit.DocDate = _Model.PO_Date;
                                URLModelEdit.DocNo = _Model.PO_No;
                                TempData["ModelData"] = _Model;
                            }
                            else
                            {
                                TempData["ListFilterData"] = _Model.ListFilterData1;
                                _Model.Message = null;
                                _Model.Command = Command;
                                _Model.TransType = "Update";
                                _Model.BtnName = "BtnEdit";
                                TempData["ModelData"] = _Model;
                                URLModelEdit.Command = Command;
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.BtnName = "BtnEdit";
                                URLModelEdit.DocumentMenuId = _Model.DocumentMenuId;
                                URLModelEdit.DocDate = _Model.PO_Date;
                                URLModelEdit.DocNo = _Model.PO_No;

                                //Session["Message"] = null;
                                //Session["Command"] = Command;
                                //Session["TransType"] = "Update";
                                //Session["BtnName"] = "BtnEdit";
                            }
                        }
                        else if (_Model.OrdStatus == "PDL" || _Model.OrdStatus == "PR" || _Model.OrdStatus == "PN")
                        {
                            if (CheckPurchaseOrderQtyforForceclosed(_Model.PO_No, _Model.PO_Date))
                            {
                                TempData["ListFilterData"] = _Model.ListFilterData1;
                                _Model.Message = null;
                                _Model.Command = Command;
                                _Model.TransType = "Update";
                                _Model.BtnName = "BtnEdit";
                                TempData["ModelData"] = _Model;
                                URLModelEdit.Command = Command;
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.BtnName = "BtnEdit";
                                URLModelEdit.DocumentMenuId = _Model.DocumentMenuId;
                                URLModelEdit.DocDate = _Model.PO_Date;
                                URLModelEdit.DocNo = _Model.PO_No;
                                //Session["Message"] = null;
                                //Session["Command"] = Command;
                                //Session["TransType"] = "Update";
                                //Session["BtnName"] = "BtnEdit";
                            }
                            else
                            {
                                TempData["ListFilterData"] = _Model.ListFilterData1;
                                _Model.Message = "deliverynoteunderprocess";
                                _Model.Command = "Update";
                                _Model.TransType = "Update";
                                _Model.BtnName = "BtnToDetailPage";
                                TempData["ModelData"] = _Model;
                                URLModelEdit.Command = "Update";
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.BtnName = "BtnToDetailPage";
                                URLModelEdit.DocumentMenuId = _Model.DocumentMenuId;
                                URLModelEdit.DocDate = _Model.PO_Date;
                                URLModelEdit.DocNo = _Model.PO_No;
                            }
                        }
                        else
                        {
                            TempData["ListFilterData"] = _Model.ListFilterData1;
                            _Model.Message = null;
                            _Model.Command = Command;
                            _Model.TransType = "Update";
                            _Model.BtnName = "BtnEdit";
                            TempData["ModelData"] = _Model;
                            URLModelEdit.Command = Command;
                            URLModelEdit.TransType = "Update";
                            URLModelEdit.BtnName = "BtnEdit";
                            URLModelEdit.DocumentMenuId = _Model.DocumentMenuId;
                            URLModelEdit.DocDate = _Model.PO_Date;
                            URLModelEdit.DocNo = _Model.PO_No;
                            //Session["Message"] = null;
                            //Session["Command"] = Command;
                            //Session["TransType"] = "Update";
                            //Session["BtnName"] = "BtnEdit";
                        }
                        return RedirectToAction("DPODetail", URLModelEdit);
                    case "Amendment":
                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.PO_No, DocDate = _Model.PO_Date, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus });
                        //}
                        /*Commented and modify by Hina sharma on 02-05-2025 to check Existing with previous year transaction*/
                        // string PODate1 = _Model.PO_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PODate1) == "TransNotAllow")
                        // {
                        //     TempData["Message1"] = "TransNotAllow";
                        //     return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.PO_No, DocDate = _Model.PO_Date, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus });
                        // }
                        /*End to chk Financial year exist or not*/
                        URLDetailModel URLModelAmendment = new URLDetailModel();
                        _Model.Command = "Edit";
                        _Model.TransType = "Update";
                        _Model.BtnName = "BtnEdit";
                        _Model.Amend = "Amend";
                        TempData["ModelData"] = _Model;
                        URLModelAmendment.Command = "Edit";
                        URLModelAmendment.TransType = "Update";
                        URLModelAmendment.BtnName = "BtnEdit";
                        URLModelAmendment.DocumentMenuId = _Model.DocumentMenuId;
                        URLModelAmendment.DocDate = _Model.PO_Date;
                        URLModelAmendment.DocNo = _Model.PO_No;
                        URLModelAmendment.Amend = "Amend";
                        return RedirectToAction("DPODetail", URLModelAmendment);
                    case "Delete":
                        DeletePODetails(_Model.PO_No, _Model.PO_Date, _Model.Title);
                        URLDetailModel URLModelDelete = new URLDetailModel();
                        PODetailsModel _ModelDelete = new PODetailsModel();
                        _ModelDelete.Message = "Deleted";
                        _ModelDelete.Command = "Refresh";
                        _ModelDelete.TransType = "New";
                        _ModelDelete.BtnName = "BtnRefresh";
                        _ModelDelete.DocumentMenuId = _Model.DocumentMenuId;
                        TempData["ModelData"] = _ModelDelete;
                        URLModelDelete.DocumentMenuId = _Model.DocumentMenuId;
                        URLModelDelete.TransType = "New";
                        URLModelDelete.BtnName = "BtnRefresh";
                        URLModelDelete.Command = "Refresh";
                        //Session["PO_No"] = null;
                        //Session["PO_Date"] = null;
                        //Session["Command"] = "Refresh";
                        //Session["TransType"] = "New";
                        //Session["BtnName"] = "BtnRefresh";
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("DPODetail", URLModelDelete);
                    case "Print":
                        return GenratePdfFile(_Model);
                    case "Refresh":
                        if (TempData["DocumentMenuId"] != null)
                        {
                            TempData["DocumentMenuId"] = TempData["DocumentMenuId"];
                        }
                        URLDetailModel URLModelRefresh = new URLDetailModel();
                        PODetailsModel _ModelRefresh = new PODetailsModel();
                        _ModelRefresh.Command = Command;
                        _ModelRefresh.TransType = "New";
                        _ModelRefresh.BtnName = "BtnRefresh";
                        _ModelRefresh.DocumentMenuId = _Model.DocumentMenuId;
                        TempData["ModelData"] = _ModelRefresh;
                        URLModelRefresh.DocumentMenuId = _Model.DocumentMenuId;
                        URLModelRefresh.TransType = "New";
                        URLModelRefresh.BtnName = "BtnRefresh";
                        URLModelRefresh.Command = Command;
                        //Session["Message"] = null;
                        //Session["Command"] = Command;
                        //Session["TransType"] = "New";
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["PO_No"] = null;
                        //Session["PO_Date"] = null;
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("DPODetail", URLModelRefresh);
                    case "BacktoList":
                        //Session["Message"] = null;
                        //Session["PO_No"] = null;
                        //Session["PO_Date"] = null;
                        if (TempData["DocumentMenuId"] != null)
                        {
                            _Model.DocumentMenuId = TempData["DocumentMenuId"].ToString();
                        }
                        if (_Model.DocumentMenuId == "105101140101")
                        {
                            TempData["WF_status"] = _Model.WF_status1;
                            TempData["ListFilterData"] = _Model.ListFilterData1;
                            return RedirectToAction("IPOList", "DPO");
                        }
                        else if (_Model.DocumentMenuId == "105101136")
                        {
                            TempData["WF_status"] = _Model.WF_status1;
                            TempData["ListFilterData"] = _Model.ListFilterData1;
                            return RedirectToAction("CPOList", "DPO");
                        }
                        else
                        {
                            TempData["WF_status"] = _Model.WF_status1;
                            TempData["ListFilterData"] = _Model.ListFilterData1;
                            return RedirectToAction("DPOList", "DPO");
                        }
                        // return RedirectToAction("DPOList");
                }
                return RedirectToAction("DPODetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public JsonResult CheckDependency(string OrdStatus,string PO_No,string PO_Date)
        {
            //Created By Suraj Maurya on 04-11-2025 to check dependency on click Save Button from front End.
            string result = ""; 
            try
            {
                PODetailsModel _Model = new PODetailsModel();
                if (OrdStatus == "A")
                {
                    if (CheckDeliveryNoteAgainstDPO(_Model, PO_No, PO_Date))
                    {
                        result = "Used";
                    }
                }
                else if (OrdStatus == "PDL" || OrdStatus == "PR" || OrdStatus == "PN")
                {
                    if (CheckPurchaseOrderQtyforForceclosed(_Model.PO_No, _Model.PO_Date)) 
                    {
                        result = "Used";
                    }
                }
                        return Json(result);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json(ex.Message);
            }
        }
        public ActionResult GetPurchadeOrdersList(string docid, string status)
        {
            //Session["MenuDocumentId"] = docid;
            //Session["WF_status"] = status;
            //Session["WF_Docid"] = docid;
            DPOListModel _DPOListModel = new DPOListModel();
            if (docid == "105101130")
            {
                //_DPOListModel.MenuDocumentId = docid;
                _DPOListModel.WF_status = status;
                //Session["WF_Docid"] = docid;
                return RedirectToAction("DPOList", "DPO", _DPOListModel);
            }
            else if (docid == "105101136")
            {
                //_DPOListModel.MenuDocumentId = docid;
                _DPOListModel.WF_status = status;
                //Session["WF_Docid"] = docid;
                return RedirectToAction("CPOList", "DPO", _DPOListModel);
            }
            else
            {
                //Session["WF_status"] = status;
                _DPOListModel.WF_status = status;
                return RedirectToAction("IPOList", "DPO", _DPOListModel);
            }
        }
        private List<DomesticPurchaseOrderList> getLPOList(DPOListModel _DPOListModel, string SuppType)
        {
            _DomesticPurchaseOrderList = new List<DomesticPurchaseOrderList>();
            try
            {
                string User_ID = string.Empty;
                //string SuppType = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }

                //if (Session["MenuDocumentId"] != null)
                //if (_DPOListModel.MenuDocumentId != null)
                //{
                //    if (_DPOListModel.MenuDocumentId == "105101130")
                //    {
                //        SuppType = "D";
                //    }
                //    if (_DPOListModel.MenuDocumentId == "105101140101")
                //    {
                //        SuppType = "I";
                //    }
                //}
                DataSet DSet = _DPOList_ISERVICE.GetPODetailList(CompID, BrchID, User_ID, _DPOListModel.SuppID, _DPOListModel.PO_FromDate, _DPOListModel.PO_ToDate, _DPOListModel.Status, _DPOListModel.wfdocid, _DPOListModel.wfstatus, SuppType, "");

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        DomesticPurchaseOrderList _DPOList = new DomesticPurchaseOrderList();
                        _DPOList.OrderNo = dr["OrderNo"].ToString();
                        _DPOList.OrderDate = dr["OrderDate"].ToString();
                        _DPOList.OrderDt = dr["OrderDt"].ToString();
                        _DPOList.OrderType = dr["OrderType"].ToString();
                        _DPOList.SourceType = dr["SourceType"].ToString();
                        _DPOList.req_area = dr["req_area"].ToString();
                        _DPOList.SourceDocNo = dr["src_doc_number"].ToString();
                        _DPOList.SuppName = dr["supp_name"].ToString();
                        _DPOList.supp_id = dr["supp_id"].ToString();
                        _DPOList.Currency = dr["curr"].ToString();
                        _DPOList.OrderValue = dr["net_val_bs"].ToString();
                        _DPOList.OrderStauts = dr["OrderStauts"].ToString();
                        _DPOList.CreateDate = dr["CreateDate"].ToString();
                        _DPOList.ApproveDate = dr["ApproveDate"].ToString();
                        _DPOList.ModifyDate = dr["ModifyDate"].ToString();
                        _DPOList.create_by = dr["create_by"].ToString();
                        _DPOList.app_by = dr["app_by"].ToString();
                        _DPOList.mod_by = dr["mod_by"].ToString();
                        _DPOList.item_type = dr["item_type"].ToString();
                        _DPOList.imp_file_no = dr["imp_file_no"].ToString();
                        _DPOList.country_origin = dr["cntry_origin_name"].ToString();
                        _DomesticPurchaseOrderList.Add(_DPOList);
                    }
                }

                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                ViewBag.FinStDt = DSet.Tables[2].Rows[0]["findate"];
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;

            }
            return _DomesticPurchaseOrderList;
        }
        [HttpPost]
        public ActionResult SearchDPODetail(string SuppId, string Fromdate, string Todate, string Status, string DocumentMenuId, string item_type)
        {
            DPOListModel _DPOListModel = new DPOListModel();
            try
            {
                //Session.Remove("WF_Docid");
                //Session.Remove("WF_status");
                string User_ID = string.Empty;
                string SuppType = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                if (DocumentMenuId != null)
                {
                    if (DocumentMenuId == "105101130" || DocumentMenuId == "105101136")
                    {
                        SuppType = "D";
                    }
                    if (DocumentMenuId == "105101140101")
                    {
                        SuppType = "I";
                    }
                }
                if (DocumentMenuId == "105101136")
                {
                    item_type = "C";
                }
                _DomesticPurchaseOrderList = new List<DomesticPurchaseOrderList>();
                //DataSet DSet = _DPOList_ISERVICE.GetPODetailList(CompID, BrchID, User_ID, SuppId, Fromdate, Todate, Status, "0", "", SuppType, item_type);//add DocMenuId by shubham maurya on 03-12-2024
                DataSet DSet = _DPOList_ISERVICE.GetPODetailList(CompID, BrchID, User_ID, SuppId, Fromdate, Todate, Status, DocumentMenuId, "", SuppType, item_type);
                //Session["DPOSearch"] = "DPO_Search";
                _DPOListModel.DPOSearch = "DPO_Search";

                foreach (DataRow dr in DSet.Tables[0].Rows)
                {
                    DomesticPurchaseOrderList _DPOList = new DomesticPurchaseOrderList();
                    _DPOList.OrderNo = dr["OrderNo"].ToString();
                    _DPOList.OrderDate = dr["OrderDate"].ToString();
                    _DPOList.OrderDt = dr["OrderDt"].ToString();
                    _DPOList.OrderType = dr["OrderType"].ToString();
                    _DPOList.SourceType = dr["SourceType"].ToString();
                    _DPOList.req_area = dr["req_area"].ToString();
                    _DPOList.SourceDocNo = dr["src_doc_number"].ToString();
                    _DPOList.SuppName = dr["supp_name"].ToString();
                    _DPOList.supp_id = dr["supp_id"].ToString();
                    _DPOList.Currency = dr["curr"].ToString();
                    _DPOList.OrderValue = dr["net_val_bs"].ToString();
                    _DPOList.OrderStauts = dr["OrderStauts"].ToString();
                    _DPOList.CreateDate = dr["CreateDate"].ToString();
                    _DPOList.ApproveDate = dr["ApproveDate"].ToString();
                    _DPOList.ModifyDate = dr["ModifyDate"].ToString();
                    _DPOList.create_by = dr["create_by"].ToString();
                    _DPOList.app_by = dr["app_by"].ToString();
                    _DPOList.mod_by = dr["mod_by"].ToString();
                    _DPOList.item_type = dr["item_type"].ToString();
                    _DPOList.imp_file_no = dr["imp_file_no"].ToString();
                    _DPOList.country_origin = dr["cntry_origin_name"].ToString();
                    _DomesticPurchaseOrderList.Add(_DPOList);
                }
                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                ViewBag.FinStDt = DSet.Tables[2].Rows[0]["findate"];
                _DPOListModel.DPOList = _DomesticPurchaseOrderList;
                _DPOListModel.DocumentMenuId = DocumentMenuId;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDPOList.cshtml", _DPOListModel);
        }
        public ActionResult GetAutoCompleteSearchSuppList(DPOListModel _DPOListModel, string SuppType)
        {
            string SupplierName = string.Empty;
            //string SrcType = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            //string SuppType = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_DPOListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _DPOListModel.SuppName;
                }
                SuppType = _DPOListModel.SrcType;

                CustList = _DPOList_ISERVICE.GetSupplierList(Comp_ID, SupplierName, Br_ID, SuppType);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }

                _DPOListModel.SupplierNameList = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteSearchItemList(PODetailsModel DPODetails)
        {
            string PoItmName = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(DPODetails.POItemName))
                {
                    PoItmName = "0";
                }
                else
                {
                    PoItmName = DPODetails.POItemName;
                }
                SuppList = _DPODetail_ISERVICE.GetPOItemListDAL(Comp_ID, Br_ID, PoItmName);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSuppAddrDetail(string Supp_id)
        {
            try
            {
                DataSet result = new DataSet();
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                result = _DPODetail_ISERVICE.GetSuppAddrDetailDAL(Supp_id, Comp_ID);

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
        [HttpPost]
        public ActionResult GetSuppContectDetail(string Suppid, string Status, string Cont_per, string Cont_no, string Disable)
        {
            try
            {
                DataSet result = new DataSet();

                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if ((Cont_per != null && Cont_per != "") || (Cont_no != null && Cont_no != "") || (Status != ""))
                {
                    ViewBag.Contectpersone = Cont_per;
                    ViewBag.contectNumber = Cont_no;
                }
                else
                {
                    result = _DPODetail_ISERVICE.GetSuppContectDetail(Suppid, Comp_ID);
                    ViewBag.Contectpersone = result.Tables[0].Rows[0]["cont_pers"].ToString();
                    ViewBag.contectNumber = result.Tables[0].Rows[0]["Contect_number"].ToString();
                }
                if (Disable == "Disable")
                {
                    ViewBag.Disable = true;
                }
                else
                {
                    ViewBag.Disable = false;
                }

                return View("~/Areas/ApplicationLayer/Views/Shared/PartialContactDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult GetPOItemDetail(string ItemID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _DPODetail_ISERVICE.GetPOItemDetailDAL(ItemID, Comp_ID);
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

        public DataSet GetPODetailEdit(string LPO_No, string LPO_Date, string DocumentMenuId)
        {
            try
            {
                //JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string User_ID = string.Empty;
                string Br_ID = string.Empty;
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
                    User_ID = Session["UserId"].ToString();
                }

                DataSet result = _DPODetail_ISERVICE.GetPODetailDAL(Comp_ID, Br_ID, LPO_No, LPO_Date, User_ID, DocumentMenuId);
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        [HttpPost]
        public JsonResult GetSuppExRateDetail(string Curr_id)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _DPODetail_ISERVICE.GetSuppExRateDetailDAL(Curr_id, Comp_ID);
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
        [HttpPost]
        public JsonResult GetPOItemUOM(string Itm_ID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _DPODetail_ISERVICE.GetPOItemUOMDAL(Itm_ID, Comp_ID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }
        [HttpPost]
        public JsonResult Getsuppcurr()
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet HoCompData = _DPODetail_ISERVICE.GetsuppcurrDAL(Comp_ID);
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

        public DataTable ToDataTable<T>(IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                object[] values = new object[props.Count];
                using (DataTable table = new DataTable())
                {
                    long _pCt = props.Count;
                    for (int i = 0; i < _pCt; ++i)
                    {
                        PropertyDescriptor prop = props[i];
                        table.Columns.Add(prop.Name, prop.PropertyType);
                    }
                    foreach (T item in data)
                    {
                        long _vCt = values.Length;
                        for (int i = 0; i < _vCt; ++i)
                        {
                            values[i] = props[i].GetValue(item);
                        }
                        table.Rows.Add(values);
                    }
                    return table;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public ActionResult InsertPODetails(PODetailsModel _Model)
        {
            //getDocumentName(); /* To set Title*/
            string PageName = _Model.Title.Replace(" ", "");

            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            string FSOId = string.Empty;
            string FinalSOId = string.Empty;
            string Comp_ID = string.Empty;
            string UserID = string.Empty;
            string BranchID = string.Empty;
            //string Del_SODate = string.Empty;
            //string SOOrderStatus = string.Empty;
            //string OrderType = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCDetail = new DataTable();
                DataTable DtblOCTaxDetail = new DataTable();
                DataTable DtblDeliSchDetail = new DataTable();
                DataTable DtblTermsDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable DtblSubItem = new DataTable();


                if (_Model.Itemdetails != null)
                {
                    DtblHDetail = ToDtblHDetail(_Model);
                    DtblItemDetail = ToDtblItemDetail(_Model.Itemdetails, _Model.DocumentMenuId);
                    DtblTaxDetail = ToDtblTaxDetail(_Model.ItemTaxdetails);
                    DtblOCDetail = ToDtblOCDetail(_Model.ItemOCdetails);
                    DtblOCTaxDetail = ToDtblTaxDetail(_Model.ItemOCTaxdetails);
                    ViewData["OCTaxDetails"] = ViewData["TaxDetails"];

                    DtblDeliSchDetail = ToDtblDelSchDetail(_Model.ItemDelSchdetails);
                    DtblTermsDetail = ToDtblTermsDetail(_Model.ItemTermsdetails);
                    DtblAttchDetail = BindAttachData(DtblHDetail, _Model.attatchmentdetail);
                    DtblSubItem = ToDtblSubItem(_Model.SubItemDetailsDt);



                    FSOId = _DPODetail_ISERVICE.InsertDPOTransactionDetails(DtblHDetail, DtblItemDetail, DtblTaxDetail, DtblOCDetail, DtblOCTaxDetail, DtblDeliSchDetail, DtblTermsDetail, DtblAttchDetail, DtblSubItem, _Model.req_area);
                    if (FSOId == "DocModify")
                    {
                        _Model.Message = "DocModify";
                        _Model.BtnName = "BtnRefresh";
                        _Model.Command = "Refresh";
                        TempData["ModelData"] = _Model;
                        return RedirectToAction("DPODetail");
                    }
                    else
                    {
                        _Model.Pending_SourceDocument = null;
                        string transtype = DtblHDetail.Rows[0]["TransType"].ToString();
                        string PONo = FSOId.Split(',')[0];
                        string PODate = FSOId.Split(',')[1];
                        string Status = FSOId.Split(',')[3];
                        ViewBag.PrintFormat = PrintFormatDataTable(_Model);
                        //string Amendment = FSOId.Split(',')[4];
                        if (transtype == "Amendment")
                        {
                            //string fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = _Model.PO_No.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_Model.PO_No, _Model.PO_Date, fileName, null, _Model.DocumentMenuId, "AM");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, _Model.DocumentMenuId, _Model.PO_No, "AM", UserID, "", filePath);
                            transtype = "Update";
                        }
                        if (transtype == "Save" || transtype == "Update")
                        {
                            //Session["Message"] = "Save";
                            _Model.Message = "Save";
                        }
                        if (Status == "Cancelled")
                        {
                            try
                            {
                               // string fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                string fileName = _Model.PO_No.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                var filePath = SavePdfDocToSendOnEmailAlert(_Model.PO_No, _Model.PO_Date, fileName, null, _Model.DocumentMenuId,"C");
                                _Common_IServices.SendAlertEmail(CompID, BrchID, _Model.DocumentMenuId, _Model.PO_No, "C", UserID, "", filePath);
                            }
                            catch (Exception exMail)
                            {
                                _Model.Message = "ErrorInMail";
                                string path = Server.MapPath("~");
                                Errorlog.LogError(path, exMail);
                            }
                            // _Model.Message = "Cancelled";
                            _Model.Message = _Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";

                        }
                        _Model.PO_No = PONo;
                        _Model.PO_Date = PODate;
                        //_Model.Amendment = Amendment;
                        //Session["PO_No"] = PONo;
                        //Session["PO_Date"] = PODate;
                        var _attachModel = TempData["ModelDataattch"] as PurchaseOrderattch;
                        TempData["ModelDataattch"] = null;
                        string guid = "";
                        if (_attachModel != null)
                            guid = _attachModel.Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, PONo, transtype, DtblAttchDetail);

                    }
                }
                if (!string.IsNullOrEmpty(FSOId))
                {

                }

                Validate = Json(FSOId);


            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

                //getDocumentName(); /* To set Title*/
                //PageName = title.Replace(" ", "");
                string Guid = "";
                //if (Session["Guid"] != null)
                if (_Model.Guid != null)
                {
                    //Guid = Session["Guid"].ToString();
                    Guid = _Model.Guid;
                }
                var other = new CommonController(_Common_IServices);
                other.DeleteTempFile(CompID + BranchID, PageName, Guid, Server);

                throw ex;
            }
            return Validate;
        }
        string IfElse(string data, string dataIf, string dataElse)
        {
            if (data == dataIf)
            {
                return dataElse;
            }
            else
            {
                return data;
            }
        }
        private DataTable ToDtblHDetail(PODetailsModel _Model)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataTable DtblHDetail = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("OrderType", typeof(string));
                dtheader.Columns.Add("FClosed", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("PO_No", typeof(string));
                dtheader.Columns.Add("PO_Date", typeof(string));
                dtheader.Columns.Add("Src_Type", typeof(string));
                dtheader.Columns.Add("SrcDocNo", typeof(string));
                dtheader.Columns.Add("SrcDocDate", typeof(string));
                dtheader.Columns.Add("SuppName", typeof(int));
                dtheader.Columns.Add("Currency", typeof(int));
                dtheader.Columns.Add("Conv_Rate", typeof(string));
                dtheader.Columns.Add("ValidUpto", typeof(string));
                dtheader.Columns.Add("ImpFileNo", typeof(string));
                dtheader.Columns.Add("CntryOrigin", typeof(string));
                dtheader.Columns.Add("PortOrigin", typeof(string));
                dtheader.Columns.Add("TradeOfTerm", typeof(string));
                dtheader.Columns.Add("Remarks", typeof(string));
                dtheader.Columns.Add("CompID", typeof(string));
                dtheader.Columns.Add("BranchID", typeof(string));
                dtheader.Columns.Add("UserID", typeof(int));
                dtheader.Columns.Add("GrVal", typeof(string));
                dtheader.Columns.Add("DiscAmt", typeof(string));
                dtheader.Columns.Add("TaxAmt", typeof(string));
                dtheader.Columns.Add("OcAmt", typeof(string));
                dtheader.Columns.Add("NetValBs", typeof(string));
                dtheader.Columns.Add("NetValSpec", typeof(string));//
                dtheader.Columns.Add("OrdStatus", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(int));
                dtheader.Columns.Add("price_basis", typeof(string));
                dtheader.Columns.Add("freight_type", typeof(string));
                dtheader.Columns.Add("mode_trans", typeof(string));
                dtheader.Columns.Add("dest", typeof(string));
                dtheader.Columns.Add("SystemDetail", typeof(string));
                dtheader.Columns.Add("peymt_terms", typeof(string));
                dtheader.Columns.Add("Delv_destn", typeof(string));
                dtheader.Columns.Add("ShipTo", typeof(string));/*Add by Hina on 20-09-2024 */

                dtheader.Columns.Add("item_type", typeof(string));
                dtheader.Columns.Add("cont_pers", typeof(string));
                dtheader.Columns.Add("cont_num", typeof(string));
                dtheader.Columns.Add("Cancel_remarks", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _Model.TransType;
                dtrowHeader["OrderType"] = _Model.POOrderType;
                dtrowHeader["FClosed"] = ConvertBoolToStrint(_Model.FClosed);
                dtrowHeader["Cancelled"] = ConvertBoolToStrint(_Model.Cancelled);
                dtrowHeader["PO_No"] = _Model.PO_No;
                dtrowHeader["PO_Date"] = _Model.PO_Date;
                dtrowHeader["Src_Type"] = IfElse(_Model.Src_Type, "PR", "P");
                if (_Model.SrcDocNo == "---Select---" || _Model.SrcDocNo == "0")
                {
                    dtrowHeader["SrcDocNo"] = "";
                }
                else
                {
                    dtrowHeader["SrcDocNo"] = _Model.SrcDocNo;
                }

                dtrowHeader["SrcDocDate"] = _Model.SrcDocDate;
                dtrowHeader["SuppName"] = _Model.SuppID;
                dtrowHeader["Currency"] = _Model.Currency;
                dtrowHeader["Conv_Rate"] = _Model.Conv_Rate;
                dtrowHeader["ValidUpto"] = _Model.ValidUpto;
                dtrowHeader["ImpFileNo"] = _Model.ImpFileNo;
                dtrowHeader["CntryOrigin"] = _Model.CntryOrigin;
                dtrowHeader["PortOrigin"] = _Model.PortOrigin;
                dtrowHeader["TradeOfTerm"] = _Model.trade_term;
                dtrowHeader["Remarks"] = _Model.Remarks;
                dtrowHeader["CompID"] = CompID;
                dtrowHeader["BranchID"] = BrchID;
                dtrowHeader["UserID"] = UserID;
                dtrowHeader["GrVal"] = IsNull(_Model.GrVal, "0");
                dtrowHeader["DiscAmt"] = IsNull(_Model.DiscAmt, "0");
                dtrowHeader["TaxAmt"] = IsNull(_Model.TaxAmt, "0");
                dtrowHeader["OcAmt"] = IsNull(_Model.OcAmt, "0");
                dtrowHeader["NetValBs"] = IsNull(_Model.NetValBs, "0");
                dtrowHeader["NetValSpec"] = IsNull(_Model.NetValSpec, "0");
                dtrowHeader["OrdStatus"] = IsNull(_Model.OrdStatus, "D");
                dtrowHeader["bill_add_id"] = _Model.bill_add_id;
                dtrowHeader["price_basis"] = _Model.PriceBasis;
                dtrowHeader["freight_type"] = _Model.FreightType;
                dtrowHeader["mode_trans"] = _Model.ModeOfTransport;
                dtrowHeader["dest"] = _Model.Destination;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["SystemDetail"] = mac_id;
                dtrowHeader["peymt_terms"] = _Model.Pymnt_trms;
                dtrowHeader["Delv_destn"] = _Model.Del_dstn;
                dtrowHeader["ShipTo"] = _Model.ShipTo;/*Add by Hina on 20-09-2024 */
                if (_Model.POOrderType == "I" && _Model.DocumentMenuId == "105101140101")
                {
                    dtrowHeader["item_type"] = "S";
                }
                if (_Model.POOrderType == "D" && _Model.DocumentMenuId == "105101130")
                {
                    if (_Model.Pending_SourceDocument == "CreateDocument_Pending")
                    {
                        dtrowHeader["item_type"] = "S";
                        _Model.Item_type = "S";
                    }
                    else
                    {
                        dtrowHeader["item_type"] = _Model.Item_type;
                    }

                }
                if (_Model.POOrderType == "D" && _Model.DocumentMenuId == "105101136")
                {
                    dtrowHeader["item_type"] = "C";
                }
                dtrowHeader["cont_pers"] = _Model.ContectPersone;
                dtrowHeader["cont_num"] = _Model.ContectNumber;
                dtrowHeader["Cancel_remarks"] = _Model.CancelledRemarks;
                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;


                return DtblHDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        private DataTable ToDtblItemDetail(string pQItemDetail,string doc_id)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(int));
                dtItem.Columns.Add("OrderQty", typeof(string));
                dtItem.Columns.Add("OrderBQty", typeof(string));
                dtItem.Columns.Add("GRNQty", typeof(string));
                dtItem.Columns.Add("InvQty", typeof(string));
                dtItem.Columns.Add("ItmRate", typeof(string));
                dtItem.Columns.Add("ItmDisPer", typeof(string));
                dtItem.Columns.Add("ItmDisAmt", typeof(string));
                dtItem.Columns.Add("DisVal", typeof(string));
                dtItem.Columns.Add("GrossVal", typeof(string));
                dtItem.Columns.Add("AssVal", typeof(string));
                dtItem.Columns.Add("TaxAmt", typeof(string));
                dtItem.Columns.Add("OCAmt", typeof(string));
                dtItem.Columns.Add("NetValSpec", typeof(string));
                dtItem.Columns.Add("NetValBase", typeof(string));
                dtItem.Columns.Add("SimpleIssue", typeof(string));
                dtItem.Columns.Add("MRSNo", typeof(string));
                dtItem.Columns.Add("FClosed", typeof(string));
                dtItem.Columns.Add("Remarks", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("sr_no", typeof(int));
                dtItem.Columns.Add("mrp", typeof(string));
                dtItem.Columns.Add("pack_size", typeof(string));

                if (pQItemDetail != null)
                {
                    JArray jObject = JArray.Parse(pQItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        dtrowLines["OrderQty"] = jObject[i]["OrderQty"].ToString();
                        dtrowLines["OrderBQty"] = jObject[i]["OrderBQty"].ToString();
                        dtrowLines["GRNQty"] = jObject[i]["GRNQty"].ToString();
                        dtrowLines["InvQty"] = jObject[i]["InvQty"].ToString();
                        dtrowLines["ItmRate"] = jObject[i]["ItmRate"].ToString();
                        dtrowLines["ItmDisPer"] = jObject[i]["ItmDisPer"].ToString();
                        dtrowLines["ItmDisAmt"] = jObject[i]["ItmDisAmt"].ToString();
                        dtrowLines["DisVal"] = jObject[i]["DisVal"].ToString();
                        dtrowLines["GrossVal"] = jObject[i]["GrossVal"].ToString();
                        dtrowLines["AssVal"] = jObject[i]["AssVal"].ToString();
                        dtrowLines["TaxAmt"] = jObject[i]["TaxAmt"].ToString();
                        dtrowLines["OCAmt"] = jObject[i]["OCAmt"].ToString();
                        dtrowLines["NetValSpec"] = jObject[i]["NetValSpec"].ToString();
                        dtrowLines["NetValBase"] = jObject[i]["NetValBase"].ToString();
                        dtrowLines["SimpleIssue"] = jObject[i]["SimpleIssue"].ToString();
                        dtrowLines["MRSNo"] = jObject[i]["MRSNo"].ToString();
                        dtrowLines["FClosed"] = jObject[i]["FClosed"].ToString();
                        dtrowLines["Remarks"] = jObject[i]["Remarks"].ToString();
                        dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                        dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                        dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                        dtrowLines["sr_no"] = jObject[i]["sr_no"].ToString();
                        if (doc_id == "105101140101")
                        {
                            dtrowLines["mrp"] = "0";
                            dtrowLines["pack_size"] = "";
                        }
                        else
                        {
                            dtrowLines["mrp"] = jObject[i]["mrp"].ToString();
                            dtrowLines["pack_size"] = jObject[i]["PackSize"].ToString();
                        }
                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["ItemDetails"] = dtitemdetail(jObject, doc_id);
                }

                DtblItemDetail = dtItem;
                return DtblItemDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        private DataTable ToDtblTaxDetail(string TaxDetails)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("TaxID", typeof(int));
                dtItem.Columns.Add("TaxRate", typeof(string));
                dtItem.Columns.Add("TaxValue", typeof(string));
                dtItem.Columns.Add("TaxLevel", typeof(int));
                dtItem.Columns.Add("TaxApplyOn", typeof(string));

                if (TaxDetails != null)
                {
                    JArray jObject = JArray.Parse(TaxDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["TaxID"] = jObject[i]["TaxID"].ToString();
                        dtrowLines["TaxRate"] = jObject[i]["TaxRate"].ToString();
                        dtrowLines["TaxValue"] = jObject[i]["TaxValue"].ToString();
                        dtrowLines["TaxLevel"] = jObject[i]["TaxLevel"].ToString();
                        dtrowLines["TaxApplyOn"] = jObject[i]["TaxApplyOn"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["TaxDetails"] = dtTaxdetail(jObject);
                }

                DtblItemTaxDetail = dtItem;
                return DtblItemTaxDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblOCDetail(string OCDetails)
        {
            try
            {
                DataTable DtblItemOCDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("OC_ID", typeof(int));
                dtItem.Columns.Add("OCValue", typeof(string));
                dtItem.Columns.Add("OCTaxAmt", typeof(string));
                dtItem.Columns.Add("OCTotalTaxAmt", typeof(string));
                if (OCDetails != null)
                {
                    JArray jObject = JArray.Parse(OCDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["OC_ID"] = jObject[i]["OC_ID"].ToString();
                        dtrowLines["OCValue"] = jObject[i]["OCValue"].ToString();
                        dtrowLines["OCTaxAmt"] = jObject[i]["OC_TaxAmt"].ToString();
                        dtrowLines["OCTotalTaxAmt"] = jObject[i]["OC_TotlAmt"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    DtblItemOCDetail = dtItem;
                    ViewData["OCDetails"] = dtOCdetail(jObject);
                }

                return DtblItemOCDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblDelSchDetail(string DelSchDetails)
        {
            try
            {
                DataTable DtblItemDelSchDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("SchDate", typeof(string));
                dtItem.Columns.Add("DeliveryQty", typeof(string));
                if (DelSchDetails != null)
                {
                    JArray jObject = JArray.Parse(DelSchDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["SchDate"] = jObject[i]["SchDate"].ToString();
                        dtrowLines["DeliveryQty"] = jObject[i]["DeliveryQty"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    DtblItemDelSchDetail = dtItem;
                    ViewData["DelvScheDetails"] = dtdeldetail(jObject);
                }

                return DtblItemDelSchDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblSubItem(string SubitemDetails)
        {
            try
            {
                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("ord_qty_spec", typeof(string));
                dtSubItem.Columns.Add("ord_qty_base", typeof(string));

                if (SubitemDetails != null)
                {
                    JArray jObject2 = JArray.Parse(SubitemDetails);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["ord_qty_spec"] = jObject2[i]["qty"].ToString();
                        dtrowItemdetails["ord_qty_base"] = jObject2[i]["qty"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                    ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
                }

                /*------------------Sub Item end----------------------*/
                return dtSubItem;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblTermsDetail(string TermsDetails)
        {
            try
            {
                DataTable DtblItemTermsDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("TermsDesc", typeof(string));
                dtItem.Columns.Add("sno", typeof(int));
                if (TermsDetails != null)
                {
                    JArray jObject = JArray.Parse(TermsDetails);
                    int sno = 1;
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["TermsDesc"] = jObject[i]["TermsDesc"].ToString();
                        dtrowLines["sno"] = sno;
                        dtItem.Rows.Add(dtrowLines);
                        sno += 1;
                    }
                    DtblItemTermsDetail = dtItem;
                    ViewData["TrmAndConDetails"] = dttermAndCondetail(jObject);
                }

                return DtblItemTermsDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        public DataTable dtitemdetail(JArray jObject,string doc_id)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("Avlstock", typeof(string));
            dtItem.Columns.Add("ord_qty_spec", typeof(string));
            dtItem.Columns.Add("ord_qty_base", typeof(string));
            dtItem.Columns.Add("dn_qty", typeof(string));
            dtItem.Columns.Add("grn_qty", typeof(string));
            dtItem.Columns.Add("inv_qty", typeof(string));
            dtItem.Columns.Add("prt_qty", typeof(string));
            dtItem.Columns.Add("qc_qty", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("item_disc_perc", typeof(string));
            dtItem.Columns.Add("item_disc_amt", typeof(string));
            dtItem.Columns.Add("item_disc_val", typeof(string));
            dtItem.Columns.Add("item_gr_val", typeof(string));
            dtItem.Columns.Add("item_ass_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val_spec", typeof(string));
            dtItem.Columns.Add("item_net_val_bs", typeof(string));
            dtItem.Columns.Add("sam_issue", typeof(string));
            dtItem.Columns.Add("mrs_no", typeof(string));
            dtItem.Columns.Add("force_close", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("tmplt_id", typeof(string));
            dtItem.Columns.Add("ItemType", typeof(string));
            dtItem.Columns.Add("mrp", typeof(string));
            dtItem.Columns.Add("pack_size", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["uom_name"] = jObject[i]["UOMName"].ToString();
                dtrowLines["Avlstock"] = jObject[i]["AvalStk"].ToString();
                dtrowLines["ord_qty_spec"] = jObject[i]["OrderQty"].ToString();
                dtrowLines["ord_qty_base"] = jObject[i]["OrderBQty"].ToString();
                dtrowLines["dn_qty"] = jObject[i]["DnQty"].ToString();
                dtrowLines["grn_qty"] = jObject[i]["GRNQty"].ToString();
                dtrowLines["inv_qty"] = jObject[i]["InvQty"].ToString();
                dtrowLines["qc_qty"] = jObject[i]["QCQty"].ToString();
                dtrowLines["prt_qty"] = "0";
                dtrowLines["item_rate"] = jObject[i]["ItmRate"].ToString();
                dtrowLines["item_disc_perc"] = jObject[i]["ItmDisPer"].ToString();
                dtrowLines["item_disc_amt"] = jObject[i]["ItmDisAmt"].ToString();
                dtrowLines["item_disc_val"] = jObject[i]["DisVal"].ToString();
                dtrowLines["item_gr_val"] = jObject[i]["GrossVal"].ToString();
                dtrowLines["item_ass_val"] = jObject[i]["AssVal"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["TaxAmt"].ToString();
                dtrowLines["item_oc_amt"] = jObject[i]["OCAmt"].ToString();
                dtrowLines["item_net_val_spec"] = jObject[i]["NetValSpec"].ToString();
                dtrowLines["item_net_val_bs"] = jObject[i]["NetValBase"].ToString();
                dtrowLines["sam_issue"] = jObject[i]["SimpleIssue"].ToString();
                dtrowLines["mrs_no"] = jObject[i]["MRSNo"].ToString();
                dtrowLines["force_close"] = jObject[i]["FClosed"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["Remarks"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                dtrowLines["tmplt_id"] = "0";
                dtrowLines["ItemType"] = jObject[i]["Itemtype"].ToString();
                if (doc_id == "105101140101") {
                    dtrowLines["mrp"] = "0";
                    dtrowLines["pack_size"] = "";
                }
                else
                {
                    dtrowLines["mrp"] = jObject[i]["mrp"].ToString();
                    dtrowLines["pack_size"] = jObject[i]["PackSize"].ToString();
                }
                dtItem.Rows.Add(dtrowLines);

            }

            return dtItem;
        }

        public DataTable dtTaxdetail(JArray jObject)
        {
            DataTable dttax = new DataTable();

            dttax.Columns.Add("item_id", typeof(string));
            dttax.Columns.Add("tax_id", typeof(int));
            dttax.Columns.Add("tax_name", typeof(string));
            dttax.Columns.Add("tax_rate", typeof(string));
            dttax.Columns.Add("tax_val", typeof(string));
            dttax.Columns.Add("tax_level", typeof(int));
            dttax.Columns.Add("tax_apply_on", typeof(string));
            dttax.Columns.Add("tax_apply_Name", typeof(string));
            dttax.Columns.Add("item_tax_amt", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dttax.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["tax_id"] = jObject[i]["TaxID"].ToString();
                dtrowLines["tax_name"] = jObject[i]["TaxName"].ToString();
                dtrowLines["tax_rate"] = jObject[i]["TaxRate"].ToString();
                dtrowLines["tax_val"] = jObject[i]["TaxValue"].ToString();
                dtrowLines["tax_level"] = jObject[i]["TaxLevel"].ToString();
                dtrowLines["tax_apply_on"] = jObject[i]["TaxApplyOn"].ToString();
                dtrowLines["tax_apply_Name"] = jObject[i]["taxapplyname"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["TotalTaxAmount"].ToString();
                dttax.Rows.Add(dtrowLines);
            }

            return dttax;
        }
        public DataTable dtOCdetail(JArray jObject)
        {
            DataTable dtOC = new DataTable();

            dtOC.Columns.Add("oc_id", typeof(int));
            dtOC.Columns.Add("oc_name", typeof(string));
            dtOC.Columns.Add("curr_name", typeof(string));
            dtOC.Columns.Add("conv_rate", typeof(float));
            dtOC.Columns.Add("oc_val", typeof(string));
            dtOC.Columns.Add("OCValBs", typeof(string));
            dtOC.Columns.Add("tax_amt", typeof(string));
            dtOC.Columns.Add("total_amt", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtOC.NewRow();

                dtrowLines["oc_id"] = jObject[i]["OC_ID"].ToString();
                dtrowLines["oc_name"] = jObject[i]["OCName"].ToString();
                dtrowLines["curr_name"] = jObject[i]["OC_Curr"].ToString();
                dtrowLines["conv_rate"] = jObject[i]["OC_Conv"].ToString();
                dtrowLines["oc_val"] = jObject[i]["OCValue"].ToString();
                dtrowLines["OCValBs"] = jObject[i]["OC_AmtBs"].ToString();
                dtrowLines["tax_amt"] = jObject[i]["OC_TaxAmt"].ToString();
                dtrowLines["total_amt"] = jObject[i]["OC_TotlAmt"].ToString();

                dtOC.Rows.Add(dtrowLines);
            }

            return dtOC;
        }
        public DataTable dtdeldetail(JArray jObject)
        {
            DataTable dtDelShed = new DataTable();

            dtDelShed.Columns.Add("item_id", typeof(string));
            dtDelShed.Columns.Add("item_name", typeof(string));
            dtDelShed.Columns.Add("sch_date", typeof(string));
            dtDelShed.Columns.Add("delv_qty", typeof(float));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtDelShed.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sch_date"] = jObject[i]["SchDate"].ToString();
                dtrowLines["delv_qty"] = jObject[i]["DeliveryQty"].ToString();
                dtDelShed.Rows.Add(dtrowLines);
            }
            return dtDelShed;
        }
        public DataTable dttermAndCondetail(JArray jObject)
        {
            DataTable dtterm = new DataTable();

            dtterm.Columns.Add("term_desc", typeof(string));


            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtterm.NewRow();

                dtrowLines["term_desc"] = jObject[i]["TermsDesc"].ToString();

                dtterm.Rows.Add(dtrowLines);
            }
            return dtterm;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("ord_qty_spec", typeof(string));
            dtSubItem.Columns.Add("ord_qty_base", typeof(string));
            dtSubItem.Columns.Add("Qty", typeof(string));


            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["ord_qty_spec"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["ord_qty_base"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["Qty"] = jObject2[i]["qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        private string ConvertBoolToStrint(Boolean _bool)
        {
            if (_bool)
                return "Y";
            else
                return "N";
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }

        public Boolean CheckDeliveryNoteAgainstDPO(PODetailsModel _Model, string DocNo, string DocDate)
        {
            Boolean Result = false;
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
                DataSet Details = _DPODetail_ISERVICE.CheckDeliveryNoteDPO(Comp_ID, Br_ID, DocNo, DocDate);
                if (Details.Tables[0].Rows.Count > 0)
                {
                    Result = true;
                }
                if (Details.Tables[1].Rows.Count > 0) //Added By Nitesh 21-11-2023 when src document is create not editable
                {
                    _Model.Item_type = "C";
                    _Model.hdn_item_typ = "C";
                    Result = true;

                }
                //DataRows = Json(JsonConvert.SerializeObject(Details));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //return Json("ErrorPage");
            }
            return Result;
        }
        public ActionResult ErrorPage()
        {
            try
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }



        protected DataTable BindAttachData(DataTable DtblHDetail, string POAttachDeatil)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            DataTable dtAttachment = new DataTable();
            dtAttachment.Columns.Add("id", typeof(string));
            dtAttachment.Columns.Add("file_name", typeof(string));
            dtAttachment.Columns.Add("file_path", typeof(string));
            dtAttachment.Columns.Add("file_def", typeof(char));
            dtAttachment.Columns.Add("comp_id", typeof(Int32));
            if (POAttachDeatil != null)
            {
                JArray jObject = JArray.Parse(POAttachDeatil);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtAttachment.NewRow();

                    dtrowLines["id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["file_name"] = jObject[i]["file_name"].ToString();
                    dtrowLines["file_path"] = jObject[i]["file_path"].ToString();
                    dtrowLines["file_def"] = jObject[i]["file_def"].ToString();
                    dtrowLines["comp_id"] = CompID;
                    dtAttachment.Rows.Add(dtrowLines);
                }
            }

            DataTable dtAttach = new DataTable();
            string Transtype = DtblHDetail.Rows[0]["TransType"].ToString();
            string PO_No = DtblHDetail.Rows[0]["PO_No"].ToString();
            var _PurchaseOrderattch = TempData["ModelDataattch"] as PurchaseOrderattch;
            //TempData["ModelDataattch"] = null;
            if (POAttachDeatil != null)
            {
                if (_PurchaseOrderattch != null)
                {
                    //if (Session["AttachMentDetailItmStp"] != null)
                    if (_PurchaseOrderattch.AttachMentDetailItmStp != null)
                    {
                        dtAttach = _PurchaseOrderattch.AttachMentDetailItmStp as DataTable;
                        //Session["AttachMentDetailItmStp"] = null;
                    }
                }
            }

            if (dtAttach.Rows.Count > 0)
            {
                foreach (DataRow dr in dtAttach.Rows)
                {
                    string flag = "Y";
                    foreach (DataRow dr1 in dtAttachment.Rows)
                    {
                        string drImg = dr1["file_name"].ToString();
                        string ObjImg = dr["file_name"].ToString();
                        if (drImg == ObjImg)
                        {
                            flag = "N";
                        }
                    }
                    if (flag == "Y")
                    {
                        DataRow dtrowAttachment1 = dtAttachment.NewRow();
                        if (!string.IsNullOrEmpty(PO_No))
                        {
                            dtrowAttachment1["id"] = PO_No;
                        }
                        else
                        {
                            dtrowAttachment1["id"] = "0";
                        }
                        string path = dr["file_path"].ToString();
                        string file_name = dr["file_name"].ToString();
                        //if (Transtype == "Update")
                        //{
                        //file_name = CompID + BrchID + PO_No.Replace("/", "") + "_" + file_name;
                        //path = dr["file_path"].ToString() + file_name;
                        //}
                        dtrowAttachment1["file_path"] = path;
                        dtrowAttachment1["file_name"] = file_name;
                        dtrowAttachment1["file_def"] = "Y";
                        dtrowAttachment1["comp_id"] = CompID;
                        dtAttachment.Rows.Add(dtrowAttachment1);
                    }

                }

            }
            return dtAttachment;
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1, string docid)
        {
            //JArray jObjectBatch = JArray.Parse(list);
            PODetailsModel _Model = new PODetailsModel();
            URLDetailModel URLModel = new URLDetailModel();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _Model.PO_No = jObjectBatch[i]["PONo"].ToString();
                    _Model.PO_Date = jObjectBatch[i]["PODate"].ToString();
                    _Model.SrcDocNo = jObjectBatch[i]["src_doc_number"].ToString();
                    _Model.SrcDocDate = jObjectBatch[i]["src_doc_date"].ToString();
                    _Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (_Model.A_Status != "Approve" || _Model.A_Status == "" || _Model.A_Status == null)
            {
                _Model.A_Status = "Approve";
            }
            InsertPOApproveDetails(_Model, docid);
            TempData["ModelData"] = _Model;
            TempData["WF_status1"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            URLModel.DocNo = _Model.PO_No;
            URLModel.DocDate = _Model.PO_Date;
            URLModel.TransType = _Model.TransType;
            URLModel.BtnName = _Model.BtnName;
            URLModel.Command = _Model.Command;
            URLModel.DocumentMenuId = docid;
            return RedirectToAction("DPODetail", URLModel);
        }

        public JsonResult InsertPOApproveDetails(PODetailsModel _Model, string docid)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                //PODetailsModel _Model = new PODetailsModel();
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string Br_ID = string.Empty;
                //string MenuDocId = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (_Model.DocumentMenuId != null)
                {
                    docid = _Model.DocumentMenuId;
                }
                var PONo = _Model.PO_No;
                var PODate = _Model.PO_Date;
                var src_doc_number = _Model.SrcDocNo;
                var src_doc_date = _Model.SrcDocDate;
                var A_Status = _Model.A_Status;
                var A_Level = _Model.A_Level;
                var A_Remarks = _Model.A_Remarks;
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105101130")
                //    {
                //        DocumentMenuId = "105101130";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105101140101")
                //    {
                //        DocumentMenuId = "105101140101";
                //    }
                //}
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string POId = _DPODetail_ISERVICE.InsertPOApproveDetailsDAL(PONo, PODate, Br_ID, src_doc_number, src_doc_date, docid, Comp_ID, UserID, mac_id, A_Status, A_Level, A_Remarks);
                ViewBag.PrintFormat = PrintFormatDataTable(_Model);
                _Model.PO_No = POId.Split(',')[0];
                _Model.PO_Date = PODate;
                try
                {
                    //string fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = _Model.PO_No.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(_Model.PO_No, _Model.PO_Date, fileName, null, docid, "AP");
                    _Common_IServices.SendAlertEmail(Comp_ID, Br_ID, docid, _Model.PO_No, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _Model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }

                //Session["PO_No"] = POId.Split(',')[0];
                //Session["Message"] = "Approved";
                
                //_Model.Message = "Approved";
                _Model.Message = _Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                _Model.BtnName = "BtnApprove";
                _Model.Command = "Update";
                _Model.DocumentMenuId = docid;
                _Model.TransType = "Update";
                _Model.Amend = null;

                TempData["ModelData"] = _Model;
                Validate = Json(POId);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Validate;
        }
        //[HttpPost]
        public JsonResult DeletePODetails(string PONo, string PODate, string Title)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                string PODelete = _DPODetail_ISERVICE.PO_DeleteDAL(Comp_ID, Br_ID, PONo, PODate);
                //Session["Message"] = "Deleted";
                if (!string.IsNullOrEmpty(PONo))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = Title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string PONo1 = PONo.Replace("/", "");
                    other.DeleteTempFile(Comp_ID + Br_ID, PageName, PONo1, Server);
                }
                Validate = Json(PODelete);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Validate;
        }

        [HttpPost]
        public JsonResult GetPO_Detail(string PO_No, string PO_Date)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string BranchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataSet result = _DPOList_ISERVICE.GetPO_Detail(Comp_ID, BranchID, PO_No, PO_Date);
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
        public ActionResult GetPOTrackingDetail(string PO_No, string PO_Date, string SuppName, string DocumentMenuId)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string BranchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataSet result = _DPOList_ISERVICE.GetPOTrackingDetail(Comp_ID, BranchID, PO_No, PO_Date);
                //DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                //ViewBag.POTrackingList = result.Tables[0];

                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.POTrackingList = result.Tables[0];
                ViewBag.po_no = PO_No; // Modified By Nitesh 13102023 1523 for set data in header of table
                ViewBag.PO_date = PO_Date;
                ViewBag.suppName = SuppName;
                ViewBag.DocumentMenuId = DocumentMenuId;

                return View("~/Areas/ApplicationLayer/Views/Shared/_PurchaseOrderTracking.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult GetSourceDocList(string Flag, string SuppID, string ItemType, string RequiredArea)
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
                if (RequiredArea == null)
                    RequiredArea = "0";
                DataSet Details = _DPODetail_ISERVICE.GetSourceDocList(Comp_ID, Br_ID, Flag, SuppID, ItemType, RequiredArea);
                //string doc_no = Details.Tables[0].Rows[0]["doc_no"].ToString();
                //string doc_dt = Details.Tables[0].Rows[0]["doc_dt"].ToString();
                //DataRow dr = Details.Tables[0].NewRow();
                //dr["doc_no"] = "---select---";
                //dr["doc_dt"] = "0";
                //dr["prDt"] = "0";
                //Details.Tables[0].Rows.Add(dr);
                DataRows = Json(JsonConvert.SerializeObject(Details));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public JsonResult GetDetailsAgainstQuotationOrPR(string Doc_no, string Doc_date, string Src_Type, string Ordertype)
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
                DataSet Details = _DPODetail_ISERVICE.GetDetailsAgainstQuotationOrPR(Comp_ID, Br_ID, Doc_no, Doc_date, Src_Type, Ordertype);

                DataRows = Json(JsonConvert.SerializeObject(Details));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        public FileResult GenratePdfFile(PODetailsModel _Model)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add("PrintFormat", typeof(string));
            //dt.Columns.Add("ShowProdDesc", typeof(string));
            //dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            //dt.Columns.Add("ShowProdTechDesc", typeof(string));
            //dt.Columns.Add("ShowSubItem", typeof(string));
            //DataRow dtr = dt.NewRow();
            //dtr["PrintFormat"] = _Model.PrintFormat; 
            //dtr["ShowProdDesc"] = _Model.ShowProdDesc;
            //dtr["ShowCustSpecProdDesc"] = _Model.ShowCustSpecProdDesc;
            //dtr["ShowProdTechDesc"] = _Model.ShowProdTechDesc;
            //dtr["ShowSubItem"] = _Model.ShowSubItem;
            //dt.Rows.Add(dtr);
            dt = PrintFormatDataTable(_Model);
            ViewBag.PrintOption = dt;
            //return File(GetPdfData(dt, _Model.PO_No, _Model.PO_Date,_Model.DocumentMenuId), "application/pdf", "PurchaseOrders.pdf");
            return File(GetPdfData(dt, _Model.PO_No, _Model.PO_Date, _Model.DocumentMenuId), "application/pdf", _Model.PO_No + ".pdf");
        }
        private DataTable PrintFormatDataTable(PODetailsModel _Model)
        {
            DataTable dt = new DataTable();
            //dt = PrintOptionsDt(PrintFormat);
            var commonCont = new CommonController(_Common_IServices);
            Cmn_PrintOptions cmn_PrintOptions = new Cmn_PrintOptions//Added by Suraj on 08-10-2024
            {
                PrintFormat = _Model.PrintFormat,
                ShowProdDesc = _Model.ShowProdDesc,
                ShowCustSpecProdDesc = _Model.ShowCustSpecProdDesc,
                ShowProdTechDesc = _Model.ShowProdTechDesc,
                ShowSubItem = _Model.ShowSubItem,
                ItemAliasName = _Model.ItemAliasName,
                ShowDeliverySchedule = _Model.ShowDeliverySchedule,
                ShowHsnNumber = _Model.ShowHSNNumber,
                ShowRemarksBlwItm = _Model.ShowRemarksBlwItm,
                SupplierAliasName = _Model.SupplierAliasName,
                ShowPayTerms = _Model.ShowPayTerms,
                ShowTotalQty = _Model.ShowTotalQty,
                ShowMRP = _Model.ShowMRP,
                ShowPackSize = _Model.ShowPackSize,
            };
            dt = commonCont.PrintOptionsDt(cmn_PrintOptions);
            /*Commented by Suraj Maurya on 08-10-2024 to create this table as common*/
            //dt.Columns.Add("PrintFormat", typeof(string));
            //dt.Columns.Add("ShowProdDesc", typeof(string));
            //dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            //dt.Columns.Add("ShowProdTechDesc", typeof(string));
            //dt.Columns.Add("ShowSubItem", typeof(string));
            //dt.Columns.Add("ItemAliasName", typeof(string));/*add by Hina on 14-08-2024*/
            //DataRow dtr = dt.NewRow();
            //dtr["PrintFormat"] = _Model.PrintFormat;
            //dtr["ShowProdDesc"] = _Model.ShowProdDesc;
            //dtr["ShowCustSpecProdDesc"] = _Model.ShowCustSpecProdDesc;
            //dtr["ShowProdTechDesc"] = _Model.ShowProdTechDesc;
            //dtr["ShowSubItem"] = _Model.ShowSubItem;
            //dtr["ItemAliasName"] = _Model.ItemAliasName;
            //dt.Rows.Add(dtr);
            return dt;
        }
        public byte[] GetPdfData(DataTable dt, string poNo, string poDate, string DocumentId)
        {
            //StringReader reader = null;
            //Document pdfDoc = null;
            //PdfWriter writer = null;
            string localLogoPath = "";
            string localDigiSignPath = "";
            string serverUrl = string.Empty;
            string path1 = Server.MapPath("~");
            CommonController common = new CommonController();
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet Details = _DPODetail_ISERVICE.GetPurchaseOrderDeatils(CompID, BrchID, poNo, poDate);
                //string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                //string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                //if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                //    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                //else
                //    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                serverUrl = common.Cmn_ServerUrl(Request);
                Errorlog.LogError_customsg(path1, serverUrl, "", "");
                string DigiSign = Details.Tables[0].Rows[0]["digi_sign"].ToString();//.Replace("/", "\\'");
                DigiSign = !string.IsNullOrEmpty(DigiSign) ? serverUrl + DigiSign : "";
                Errorlog.LogError_customsg(path1, DigiSign, "", "");
                ViewBag.PageName = "PO";
                ViewBag.Title = "Purchase Order";
                DocumentMenuId = DocumentId;
                ViewBag.DocId = DocumentId;
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                //string FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();//.Replace("/", "\\'");
                string FLogoPath = Details.Tables[0].Rows[0]["logo"].ToString();//.Replace("/", "\\'");
                FLogoPath = !string.IsNullOrEmpty(FLogoPath) ? serverUrl + FLogoPath : "";
                Errorlog.LogError_customsg(path1, FLogoPath, "", "");
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                /* Cmn_GenLocalFilePath commented by Suraj Maurya on 07-10-2025 due to not needed */
                //localLogoPath = common.Cmn_GenLocalFilePath(FLogoPath, "Logo", Server);// Downloading file localy and getting local path
                //localDigiSignPath = common.Cmn_GenLocalFilePath(DigiSign, "DigiSign", Server);// Downloading file localy and getting local path
                //localLogoPath = FLogoPath;// common.Cmn_GenLocalFilePath(FLogoPath, "Logo", Server);// Downloading file localy and getting local path
                //localDigiSignPath = DigiSign;// common.Cmn_GenLocalFilePath(DigiSign, "DigiSign", Server);// Downloading file localy and getting local path
                //Errorlog.LogError_customsg(path1, localLogoPath, "", "");
                //Errorlog.LogError_customsg(path1, localDigiSignPath, "", "");
                Cmn_pdfGenerate_model cmn_model = new Cmn_pdfGenerate_model() //Mendatory Code with Cmn_GenLocalFilePath
                {
                    localLogoPath = FLogoPath/*localLogoPath*/,
                    localDigiSignPath = DigiSign/*localDigiSignPath*/
                };
                ViewBag.FLogoPath = FLogoPath/*localLogoPath*/;
                ViewBag.DigiSign = DigiSign/*localDigiSignPath*/;

                string LogoImage = ViewBag.FLogoPath.ToString();
                string DigiSignatureImage = ViewBag.DigiSign.ToString();

                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "Invoice to:";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                var remarks = Details.Tables[0].Rows[0]["remarks"].ToString().Trim();
                var remarks1 = remarks.Split('\r');
                ViewBag.remarks1 = remarks1;

                string htmlcontent = "";
                if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
                {
                    htmlcontent = common.Cmn_ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPOprintFormat2.cshtml"), ControllerContext);
                }
                else
                {
                    htmlcontent = common.Cmn_ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPOprint.cshtml"), ControllerContext);
                }
                //string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPOprint.cshtml"));
                //string DelSchedule = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_PrintReportDeliverySchedule.cshtml")); //Commented By Nitesh 24-11-2023 For Delivery Schedule Print in one page with item print 
                byte[] pdfData = common.Cmn_PdfFileGenerate(htmlcontent, DocumentMenuId, ViewBag.DocStatus, Server, cmn_model, Details, LogoImage, DigiSignatureImage);

                return pdfData;
                //using (MemoryStream stream = new System.IO.MemoryStream())
                //{
                //    reader = new StringReader(htmlcontent);
                //    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                //    writer = PdfWriter.GetInstance(pdfDoc, stream);
                //    pdfDoc.Open();
                //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                //    pdfDoc.Close();
                //    Byte[] bytes = stream.ToArray();
                //    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                //    using (var reader1 = new PdfReader(bytes))
                //    {
                //        using (var ms = new MemoryStream())
                //        {
                //            using (var stamper = new PdfStamper(reader1, ms))
                //            {
                //                int PageCount = reader1.NumberOfPages;
                //                for (int i = 1; i <= PageCount; i++)
                //                {
                //                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                //                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                //                }
                //            }
                //            bytes = ms.ToArray();
                //        }
                //    }

                //    return bytes.ToArray();
                //}
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                common.Cmn_DeleteFile(localLogoPath);
                common.Cmn_DeleteFile(localDigiSignPath);
                return null;
            }
            finally
            {

            }
        }

        //public byte[] GetPdfData(DataTable dt, string poNo, string poDate)
        //{
        //    StringReader reader = null;
        //    Document pdfDoc = null;
        //    PdfWriter writer = null;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            BrchID = Session["BranchId"].ToString();
        //        }
        //        DataSet Details = _DPODetail_ISERVICE.GetPurchaseOrderDeatils(CompID, BrchID, poNo, poDate);
        //        string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
        //        string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
        //        if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
        //            serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
        //        else
        //            serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
        //        //ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();//.Replace("/", "\\'");

        //        ViewBag.PageName = "PO";
        //        ViewBag.Title = "Purchase Order";
        //        string path1 = Server.MapPath("~") + "..\\Attachment\\";
        //        string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
        //        ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
        //        ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();//.Replace("/", "\\'");
        //        //string path1 = Server.MapPath("~") + "..\\Attachment\\";
        //        //string DigiSignPath = path1 + Details.Tables[0].Rows[0]["digi_sign"].ToString().Replace("/", "\\'");
        //        //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");

        //        string lgPath = "http://alaskaerp.workonline.cloud:8081//CompanyLogo/D-20_Logo.png";
        //        string lacalLogoPath = Path.Combine(Path.GetTempPath(), "tmpLogo.png");
        //        //using (WebClient client = new WebClient())
        //        //{
        //        //    client.DownloadFile(lgPath, lacalLogoPath);
        //        //}
        //        string path2 = lgPath;// lacalLogoPath;// Server.MapPath("~") + "Content\\TempAttachmetForPrint\\D-20_Logo.png";

        //        ViewBag.FLogoPath = path2;
        //        //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
        //        ViewBag.DigiSign = DigiSignPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
        //        ViewBag.Details = Details;
        //        ViewBag.InvoiceTo = "Invoice to:";
        //        ViewBag.DocStatus = Details.Tables[0].Rows[0]["ord_status"].ToString().Trim();

        //       var remarks = Details.Tables[0].Rows[0]["remarks"].ToString().Trim();
        //        var remarks1 = remarks.Split('\r');
        //        ViewBag.remarks1 = remarks1;

        //        string htmlcontent = "";
        //        if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
        //        {
        //            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPOprint.cshtml"));
        //        }
        //        else
        //        {
        //            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPOprint.cshtml"));
        //        }
        //        //string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPOprint.cshtml"));
        //        //string DelSchedule = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_PrintReportDeliverySchedule.cshtml")); //Commented By Nitesh 24-11-2023 For Delivery Schedule Print in one page with item print 
        //        using (MemoryStream stream = new System.IO.MemoryStream())
        //        {
        //            reader = new StringReader(htmlcontent);
        //            pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
        //            writer = PdfWriter.GetInstance(pdfDoc, stream);
        //            pdfDoc.Open();
        //            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
        //            pdfDoc.Close();
        //            Byte[] bytes = stream.ToArray();
        //            BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
        //            using (var reader1 = new PdfReader(bytes))
        //            {
        //                using (var ms = new MemoryStream())
        //                {
        //                    using (var stamper = new PdfStamper(reader1, ms))
        //                    {
        //                        int PageCount = reader1.NumberOfPages;
        //                        for (int i = 1; i <= PageCount; i++)
        //                        {
        //                            Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
        //                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
        //                        }
        //                    }
        //                    bytes = ms.ToArray();
        //                }
        //            }

        //            return bytes.ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //    finally
        //    {

        //    }
        //}
        [NonAction]
        private DataTable GetRequirmentreaList()
        {
            try
            {
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataTable dt = _DPODetail_ISERVICE.GetRequirmentreaList(CompID, BrchID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }

        public Boolean CheckPurchaseOrderQtyforForceclosed(string DocNo, string DocDate)
        {
            //JsonResult DataRows = null;
            Boolean Result = false;
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
                DataSet Details = _DPODetail_ISERVICE.CheckLPOQty_ForceClosed(Comp_ID, Br_ID, DocNo, DocDate);
                if (Details.Tables[0].Rows.Count > 0)
                {
                    Result = true;
                }
                //DataRows = Json(JsonConvert.SerializeObject(Details));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return Json("ErrorPage");
                throw ex;
            }
            return Result;
        }

        //public ActionResult GetPurchaseOrderList(string docid, string status)
        //{
        //    if (docid == "105101130")
        //    {
        //        DPOListModel _DPOListModel = new DPOListModel();
        //        //_DPOListModel.MenuDocumentId = docid;
        //        _DPOListModel.WF_status = status;
        //        //Session["WF_Docid"] = docid;
        //        return RedirectToAction("DPOList", "DPO", _DPOListModel);
        //    }
        //    else
        //    {
        //        Session["WF_status"] = status;
        //        return RedirectToAction("IPOList", "DPO");
        //    }
        //    //Session["MenuDocumentId"] = docid;
        //    //Session["WF_status"] = status;
        //    //Session["WF_Docid"] = docid;
        //    //if (docid == "105103125")
        //    //{
        //    //    return RedirectToAction("DPOList");
        //    //}
        //    //else
        //    //{
        //    //    return RedirectToAction("DPOList");
        //    //}            
        //}

        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                PurchaseOrderattch _PurchaseOrderattch = new PurchaseOrderattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "Save";
                //string PONo = "";
                string branchID = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["po_no"] != null)
                //{
                //    PONo = Session["po_no"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _PurchaseOrderattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                //string br_id = Session["BranchId"].ToString();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + branchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _PurchaseOrderattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _PurchaseOrderattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _PurchaseOrderattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        public ActionResult GetPOAttatchDetailEdit(string PO_No, string PO_Date)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet result = _DPODetail_ISERVICE.GetPOAttatchDetailEdit(CompID, BrchID, PO_No, PO_Date);
                ViewBag.AttechmentDetails = result.Tables[0];
                ViewBag.Disable = true;
                return PartialView("~/Areas/Common/Views/cmn_imagebind.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        /*--------------------------For Attatchment End--------------------------*/
        public string SavePdfDocToSendOnEmailAlert(string poNo, string poDate, string fileName, string PrintFormat, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            DataTable dt = new DataTable();
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                if (ViewBag.PrintFormat == null)
                {
                    if (PrintFormat != null)
                    {
                        dt = commonCont.PrintOptionsDt(PrintFormat);
                    }
                }
                else
                {
                    dt = ViewBag.PrintFormat;
                }
                ViewBag.PrintOption = dt;
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(dt, poNo, poDate, "");
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return "ErrorPage";
            }
            return null;
        }
        /*---------------------------------Sub-Item Start-------------------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
     , string Flag, string Status, string Doc_no, string Doc_dt, string SRCDoc_no, string SRCDoc_date, string src_type, string UOMId)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();
                int QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
                if (Flag == "QuantitySpec" || Flag == "QuantityBase" || Flag == "POPROrdQty" || Flag == "POQTOrdQty")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        if ((src_type == "D" && Flag == "QuantitySpec") || (src_type == "D" && Flag == "QuantityBase"))
                        {
                            dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, "0", Item_id, UOMId, "br").Tables[0];
                            dt.Columns.Add("Qty", typeof(string));
                        }
                        else if ((src_type == "PR" && Flag == "POPROrdQty") || (src_type == "PR" && Flag == "QuantityBase") || (src_type == "R" && Flag == "POPROrdQty"))
                        {
                            if (Status == "")
                            {
                                dt = _DPODetail_ISERVICE.GetQTandPRSubItemwithWhAvlstock(CompID, BrchID, Item_id, SRCDoc_no, SRCDoc_date, Flag, src_type).Tables[0];
                            }
                            else
                            {
                                dt = _DPODetail_ISERVICE.GetQTandPRSubItemwithWhAvlstockAftrInsrt(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                            }
                            //dt.Columns.Add("Qty", typeof(string));
                        }
                        else
                        {
                            dt = _DPODetail_ISERVICE.GetQTandPRSubItemwithWhAvlstock(CompID, BrchID, Item_id, SRCDoc_no, SRCDoc_date, Flag, src_type).Tables[0];
                        }
                        //dt.Columns.Add("Qty", typeof(string));

                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    //dt.Rows[i]["Qty"] = item.GetValue("qty");
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));

                                }
                            }
                        }
                    }
                    else
                    {
                        if (Flag == "QuantitySpec" || Flag == "QuantityBase")
                        {
                            dt = _DPODetail_ISERVICE.PO_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                        }
                        else if (Flag == "POPROrdQty")
                        {
                            dt = _DPODetail_ISERVICE.GetQTandPRSubItemwithWhAvlstockAftrInsrt(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];

                        }
                        else
                        {
                            dt = _DPODetail_ISERVICE.GetQTandPRSubItemwithWhAvlstock(CompID, BrchID, Item_id, SRCDoc_no, SRCDoc_date, Flag, src_type).Tables[0];

                        }

                    }
                    Flag = "Quantity";
                }
                //else
                //{
                //    dt.Columns.Add("item_id", typeof(string));
                //    dt.Columns.Add("sub_item_id", typeof(string));
                //    dt.Columns.Add("sub_item_name", typeof(string));
                //    //dt.Columns.Add("Avl_stk", typeof(string));
                //    dt.Columns.Add("Qty", typeof(string));

                //    JArray arr = JArray.Parse(SubItemListwithPageData);
                //    foreach (JObject item in arr.Children())//
                //    {
                //        DataRow dRow = dt.NewRow();
                //        dRow["item_id"] = item.GetValue("item_id").ToString();
                //        dRow["sub_item_id"] = item.GetValue("sub_item_id").ToString();
                //        dRow["sub_item_name"] = item.GetValue("sub_item_name").ToString();
                //        //dRow["Avl_stk"] = item.GetValue("AvlStock").ToString();
                //        dRow["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));

                //        dt.Rows.Add(dRow);
                //    }
                //    Flag = "Quantity";
                //}
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    //_subitemPageName = "MTO",
                    ShowStock = (Status == "D" || Status == "F" || Status == "") ? "Y" : "N",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y"

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
        /*---------------------------------Sub-Item End-------------------------------------*/

        private void SetDecimals(PODetailsModel _model)//Added by Suraj Maurya on 23-10-2024
        {

            _model.ValDigit = ToFixDecimal(Convert.ToInt32((_model.SrcType == "I" ? Session["ExpImpValDigit"] : Session["ValDigit"]).ToString()));
            _model.QtyDigit = ToFixDecimal(Convert.ToInt32((_model.SrcType == "I" ? Session["ExpImpQtyDigit"] : Session["QtyDigit"]).ToString()));
            _model.RateDigit = ToFixDecimal(Convert.ToInt32((_model.SrcType == "I" ? Session["ExpImpRateDigit"] : Session["RateDigit"]).ToString()));
            _model.ExchDigit = ToFixDecimal(Convert.ToInt32(Session["ExchDigit"].ToString()));

            ViewBag.ValDigit = _model.ValDigit;
            ViewBag.QtyDigit = _model.QtyDigit;
            ViewBag.RateDigit = _model.RateDigit;
            ViewBag.ExchDigit = _model.ExchDigit;
        }

        public ActionResult GetPendingDocument(string Docid, string SrcType)
        {
            try
            {
                DPOListModel _DPOListModel = new DPOListModel();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                _DPOListModel.DocumentMenuId = Docid;
                DataSet DetailDatatable = new DataSet();

                DetailDatatable = _DPOList_ISERVICE.GetPendingDocumentData(CompID, BrchID, Docid, language, SrcType);

                List<PendingDocumentList> _DomesticPurchaseOrderList = new List<PendingDocumentList>();
                if (DetailDatatable.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DetailDatatable.Tables[0].Rows)
                    {
                        PendingDocumentList _DPOList = new PendingDocumentList();
                        _DPOList.Pending_SourceDocName = dr["Doc_name"].ToString();

                        _DPOList.Pending_Document_no = dr["Doc_no"].ToString();
                        _DPOList.Pending_Document_dt = dr["Doc_dt"].ToString();
                        _DPOList.Pending_Req_area_id = dr["req_area"].ToString();
                        _DPOList.Pending_Req_area = dr["setup_val"].ToString();
                        _DPOList.Pending_Supplier_Name = dr["supp_name"].ToString();
                        _DPOList.Pending_supplier_ID = dr["supp_id"].ToString();
                        _DPOList.Pending_Doc_Status = dr["status_name"].ToString();
                        _DPOList.Pending_CreateDate = dr["create_dt"].ToString();
                        _DPOList.Pending_ApproveDate = dr["app_dt"].ToString();
                        _DPOList.Pending_create_by = dr["Created_by"].ToString();
                        _DPOList.Pending_app_by = dr["appname"].ToString();
                        _DPOList.Pending_SourceDocAlias = dr["Src_doc_alias"].ToString();
                        _DPOList.Pending_Docdt = dr["pending_Docdt"].ToString();
                        _DPOList.flag = dr["flag"].ToString();
                        _DomesticPurchaseOrderList.Add(_DPOList);
                    }
                }
                _DPOListModel.Pending_DocumentList = _DomesticPurchaseOrderList;
                _DPOListModel.PendingDPOSearch = "PendingData";


                return PartialView("~/Areas/ApplicationLayer/Views/Shared/POPartialPendingSourceDocument.cshtml", _DPOListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult CreateDocuemntPendingList(string Doc_no, string Doc_dt, string supp_id, string Doc_id)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                PODetailsModel _Model = new PODetailsModel();
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                {
                    DPOListModel objModel = new DPOListModel();
                    objModel.Message = "Financial Year not Exist";
                    TempData["Message"] = "Financial Year not Exist";
                    return RedirectToAction("DPOList", objModel);
                }
                else
                {
                    DocumentMenuId = Doc_id;

                    TempData["ListFilterData"] = null;
                    _Model.Message = null;
                    _Model.Command = "Add";
                    _Model.PO_No = null;
                    _Model.PO_Date = null;
                    _Model.TransType = "Save";
                    _Model.BtnName = "BtnAddNew";
                    ViewBag.DocumentStatus = "D";

                    _Model.Pending_Src_DocNo = Doc_no;
                    _Model.Pending_Src_Docdt = Doc_dt;
                    _Model.Pending_Supp_id = supp_id;
                    _Model.DocumentMenuId = Doc_id;
                    CommonPageDetails();
                    ViewBag.DocID = Doc_id;
                    ViewBag.DocumentMenuId = Doc_id;
                    ViewBag.DocumentStatus = "D";
                    _Model.ILSearch = null;
                    if (Doc_id != "105101130")
                    {
                        _Model.SrcType = "I";
                    }
                    else
                    {
                        _Model.SrcType = "D";
                    }

                    _Model.Src_Type = "Q";

                    ViewBag.DocumentMenuId = Doc_id;
                    _Model.GstApplicable = ViewBag.GstApplicable;
                    SetDecimals(_Model);

                    //List<SupplierName> suppLists = new List<SupplierName>();
                    //suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    //_Model.SupplierNameList = suppLists;

                    GetAutoCompleteSearchSuppByCreatePendingDocument(_Model, _Model.SrcType);
                    _Model.SuppID = supp_id;
                    List<CurrancyList> currancyLists = new List<CurrancyList>();
                    currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                    _Model.currancyLists = currancyLists;

                    List<CountryOfOrigin> CountryofList = new List<CountryOfOrigin>();
                    CountryofList.Add(new CountryOfOrigin { cntry_id = "0", cntry_name = "---Select---" });
                    _Model.countryOfOrigins = CountryofList;

                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _Model.docNoLists = srcDocNoLists;

                    //DataTable dt = new DataTable();
                    List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();


                    RequirementAreaList _RAList = new RequirementAreaList();
                    _RAList.req_id = 0;
                    _RAList.req_val = "---Select---";
                    requirementAreaLists.Add(_RAList);

                    //requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "---Select---" });
                    _Model._requirementAreaLists = requirementAreaLists;

                    List<trade_termList> _TermLists = new List<trade_termList>();
                    _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "0", TrdTrms_val = "---Select---" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    _Model.TradeTermsList = _TermLists;
                    _Model.DocumentMenuId = Doc_id;
                    _Model.Title = title;
                    _Model.UserID = UserID;
                    _Model.Pending_SourceDocument = "CreateDocument_Pending";
                    _Model.Item_type = "S";
                }

                return View("~/Areas/ApplicationLayer/Views/Procurement/DomesticPurchaseOrder/DPODetail.cshtml", _Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public void GetAutoCompleteSearchSuppByCreatePendingDocument(PODetailsModel _DPOListModel, string SuppType)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_DPOListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _DPOListModel.SuppName;
                }
                SuppType = _DPOListModel.SrcType;

                CustList = _DPOList_ISERVICE.GetSupplierList(Comp_ID, SupplierName, Br_ID, SuppType);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }

                _DPOListModel.SupplierNameList = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                throw Ex;
                //return View("~/Views/Shared/Error.cshtml");
            }
            //  return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPendingDocumentitemDetail(string doc_no, string doc_dt, string flag)
        {
            try
            {
                PendingDocumentList _MaterialIssueList = new PendingDocumentList();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                List<PendingDocumentList> _MaterialIssueDetailList = new List<PendingDocumentList>();
                DataTable itemDetail = new DataTable();

                itemDetail = _DPOList_ISERVICE.GetPendingDocumentDataitemdetail(CompID, BrchID, doc_no, doc_dt, flag).Tables[0];
                ViewBag.PendingItemDetail = itemDetail;
                return PartialView("~/Areas/Common/Views/Cmn_ItemInformationPSDList.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult SendEmailAlert(PODetailsModel _Model, string mail_id, string status, string docid, string Doc_no, string Doc_dt, string filepath)
        {
            try
            {
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
                var commonCont = new CommonController(_Common_IServices);
                DataTable dt = new DataTable();
                string message = "";
                try
                {
                    if (filepath == "" || filepath == null)
                    {
                        string fileName = "";
                        if (docid == "105101140101")
                        {
                            fileName = "OTP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        }
                        if (docid == "105101130")
                        {
                            fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        }
                        if (docid == "105101136")
                        {
                            fileName = "CPI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        }
                        filepath = SavePdfDocToSendOnEmailAlert_Ext(_Model, Doc_no, Doc_dt, fileName, "");
                    }
                    message = commonCont.SendEmailAlert(CompID, BrchID, UserID, mail_id, status, docid, Doc_no, Doc_dt, filepath);
                }
                catch (Exception exMail)
                {
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                return Json(message);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        //Added by Nidhi on 06-08-2025
        public string SavePdfDocToSendOnEmailAlert_Ext(PODetailsModel _Model, string poNo, string poDate, string fileName, string PrintFormat)
        {
            DataTable dt = new DataTable();
            var commonCont = new CommonController(_Common_IServices);
            if (!string.IsNullOrEmpty(PrintFormat))
            {
                dt = commonCont.PrintOptionsDt(PrintFormat);
            }
            else
            {
                dt = PrintFormatDataTable(_Model);
            }
            ViewBag.PrintOption = dt;
            var data = GetPdfData(dt, poNo, poDate, "");
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
        public ActionResult BindReplicateWithlist(PODetailsModel _Model)
        {
            Dictionary<string, string> ItemList = new Dictionary<string, string>();

            try
            {
                string CompID = Session["CompId"]?.ToString();
                string Br_ID = Session["BranchId"]?.ToString();
                string SearchValue = string.IsNullOrEmpty(_Model?.item) ? "0" : _Model.item;

                if (!string.IsNullOrEmpty(CompID) && !string.IsNullOrEmpty(Br_ID))
                {
                    DataSet productList = _DPODetail_ISERVICE.getReplicateWith(
                        CompID,
                        Br_ID,
                        _Model.POOrderType,
                        SearchValue,
                        _Model.Item_type
                    );

                    if (productList != null && productList.Tables.Count > 0 && productList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in productList.Tables[0].Rows)
                        {
                            string Doc_no = row["Doc_no"]?.ToString();
                            string Doc_dt = row["Doc_dt"]?.ToString();
                            string supp_name = row["supp_name"]?.ToString();

                            string key = $"{Doc_no},{Doc_dt}";

                            // Avoid duplicate key errors
                            if (!ItemList.ContainsKey(key))
                                ItemList.Add(key, supp_name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }

            var jsonResult = ItemList
                .Select(c => new { Name = c.Value, ID = c.Key })
                .ToList();

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSPLDetail(string ItemID,string supp_id)
        {
            try
            {
                JsonResult DataRows = null;
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
                DataSet result = _DPODetail_ISERVICE.GetSPLDetails(Comp_ID,Br_ID, supp_id, ItemID);
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
    }
}