using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ServicePurchaseOrder;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ServicePurchaseOrder;
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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.ServicePurchaseOrder
{
    public class ServicePurchaseOrderController : Controller
    {
        List<ServicePurchaseOrderList> _ServicePurchaseOrderList;
        string DocumentMenuId = "105101135";
        string CompID, language, title, UserID, create_id, BrchID = string.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SPO_ISERVICE _SPO_ISERVICE;
        public ServicePurchaseOrderController(Common_IServices _Common_IServices, SPO_ISERVICE _SPO_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._SPO_ISERVICE = _SPO_ISERVICE;
        }
        // GET: ApplicationLayer/ServicePurchaseOrder
        public ActionResult ServicePurchaseOrder(SPOListModel _SPOListModel)
        {
            try
            {
                CommonPageDetails();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //SPOListModel _SPOListModel = new SPOListModel();
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _SPOListModel.WF_status = TempData["WF_status"].ToString();
                    if (_SPOListModel.WF_status != null)
                    {
                        _SPOListModel.wfstatus = _SPOListModel.WF_status;
                    }
                    else
                    {
                        _SPOListModel.wfstatus = "";
                    }
                }
                else
                {
                    if (_SPOListModel.WF_status != null)
                    {
                        _SPOListModel.wfstatus = _SPOListModel.WF_status;
                    }
                    else
                    {
                        _SPOListModel.wfstatus = "";
                    }
                }
                //if (Session["WF_Docid"] != null)
                if (DocumentMenuId != null)
                {
                    _SPOListModel.wfdocid = DocumentMenuId;
                }
                else
                {
                    _SPOListModel.wfdocid = "0";
                }

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");


                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string todate = range.ToDate;

                //  GetAutoCompleteSearchSuppList(_SPOListModel);


                //var other = new CommonController(_Common_IServices);                
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                //_SPOListModel.SuppID, _SPOListModel.PO_FromDate, _SPOListModel.PO_ToDate, _SPOListModel.Status,
                _SPOListModel.StatusList = statusLists;

                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    if (PRData != null && PRData != "")
                    {
                        var a = PRData.Split(',');
                        _SPOListModel.SuppID = a[0].Trim();
                        _SPOListModel.PO_FromDate = a[1].Trim();
                        _SPOListModel.PO_ToDate = a[2].Trim();
                        _SPOListModel.Status = a[3].Trim();
                        if (_SPOListModel.Status == "0")
                        {
                            _SPOListModel.Status = null;

                        }
                        _SPOListModel.FromDate = _SPOListModel.PO_FromDate;
                        _SPOListModel.ListFilterData = TempData["ListFilterData"].ToString();
                        //   _SPOListModel.SPOList = GetSPOList(_SPOListModel);
                    }
                    //else
                    //{
                    //    //_SPOListModel.SPOList = GetSPOList(_SPOListModel);

                    //    _SPOListModel.FromDate = startDate;
                    //}
                }
                else
                {
                    //   _SPOListModel.SPOList = GetSPOList(_SPOListModel);
                    _SPOListModel.FromDate = startDate;
                    _SPOListModel.PO_FromDate = startDate;
                    _SPOListModel.PO_ToDate = todate;
                }
                GetAllData(_SPOListModel); ;
                _SPOListModel.Title = title;
                //Session["SPOSearch"] = "0";           
                _SPOListModel.SPOSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseOrder/ServicePurchaseOrderList.cshtml", _SPOListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(SPOListModel _SPOListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            //  string SuppType = string.Empty;
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
                if (string.IsNullOrEmpty(_SPOListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _SPOListModel.SuppName;
                }

                DataSet SuppList1 = _SPO_ISERVICE.GetAllData(CompID, SupplierName, BrchID,
                     UserID, _SPOListModel.SuppID, _SPOListModel.PO_FromDate, _SPOListModel.PO_ToDate,
                     _SPOListModel.Status, _SPOListModel.wfdocid, _SPOListModel.wfstatus);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (DataRow data in SuppList1.Tables[0].Rows)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data["supp_id"].ToString();
                    _SuppDetail.supp_name = data["supp_name"].ToString();
                    _SuppList.Add(_SuppDetail);
                }
                _SuppList.Insert(0, new SupplierName() { supp_id = "0", supp_name = "All" });
                _SPOListModel.SupplierNameList = _SuppList;
                SetListData(SuppList1, _SPOListModel);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void SetListData(DataSet DSet, SPOListModel _SPOListModel)
        {
            List<ServicePurchaseOrderList> _ServicePurchaseOrderList = new List<ServicePurchaseOrderList>();
            if (DSet.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in DSet.Tables[1].Rows)
                {
                    ServicePurchaseOrderList _SPOList = new ServicePurchaseOrderList();
                    _SPOList.OrderNo = dr["OrderNo"].ToString();
                    _SPOList.OrderDate = dr["OrderDate"].ToString();
                    _SPOList.OrderDt = dr["OrderDt"].ToString();
                    _SPOList.SourceType = dr["SourceType"].ToString();
                    _SPOList.SourceDocNo = dr["src_doc_number"].ToString();
                    _SPOList.SuppName = dr["supp_name"].ToString();
                    _SPOList.Currency = dr["curr"].ToString();
                    _SPOList.OrderValue = dr["net_val_bs"].ToString();
                    _SPOList.OrderStauts = dr["OrderStauts"].ToString();
                    _SPOList.CreateDate = dr["CreateDate"].ToString();
                    _SPOList.ApproveDate = dr["ApproveDate"].ToString();
                    _SPOList.ModifyDate = dr["ModifyDate"].ToString();
                    _SPOList.create_by = dr["create_by"].ToString();
                    _SPOList.app_by = dr["app_by"].ToString();
                    _SPOList.mod_by = dr["mod_by"].ToString();
                    _ServicePurchaseOrderList.Add(_SPOList);
                }
            }
            _SPOListModel.SPOList = _ServicePurchaseOrderList;
            ViewBag.FinStDt = DSet.Tables[3].Rows[0]["findate"];
        }
        public ActionResult AddServicePurchaseOrderDetail()
        {
            SPODetailModel _SPODetailModel = new SPODetailModel();
            _SPODetailModel.Command = "Add";
            _SPODetailModel.TransType = "Save";
            _SPODetailModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = _SPODetailModel;
            //Session["Message"] = null;
            //Session["Command"] = "Add";
            //Session["SPO_No"] = null;
            //Session["SPO_Date"] = null;
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            TempData["ListFilterData"] = null;
            ViewBag.MenuPageName = getDocumentName();
            /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                SPOListModel pqModel = new SPOListModel();
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ServicePurchaseOrder", pqModel);
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("ServicePurchaseOrderDetail", "ServicePurchaseOrder");
        }
        public ActionResult ServicePurchaseOrderDetail(URLModelDetails URLModel)
        {
            CommonPageDetails();
            ViewBag.DocID = DocumentMenuId;
            ViewBag.DocumentMenuId = DocumentMenuId;
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
            /*Add by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.DocDate) == "TransNotAllow")
            //{
            //    //TempData["Message2"] = "TransNotAllow";
            //    ViewBag.Message = "TransNotAllow";
            //}
            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
            try
            {
                var _SPODetailModel = TempData["ModelData"] as SPODetailModel;
                if (_SPODetailModel != null)
                {
                    _SPODetailModel.ILSearch = null;
                    //var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.DocumentStatus = "D";
                    // SPODetailModel _SPODetailModel = new SPODetailModel();
                    _SPODetailModel.ValDigit = ValDigit;
                    _SPODetailModel.QtyDigit = QtyDigit;
                    _SPODetailModel.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _SPODetailModel.docNoLists = srcDocNoLists;
                    _SPODetailModel.SupplierNameList = suppLists;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _SPODetailModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _SPODetailModel.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["SPO_No"] != null && Session["SPO_Date"] != null)
                    if (_SPODetailModel.SPO_No != null && _SPODetailModel.SPO_Date != null)
                    {
                        string Doc_no = _SPODetailModel.SPO_No;
                        string Doc_date = _SPODetailModel.SPO_Date;
                        //string Doc_no = Session["SPO_No"].ToString();
                        //string Doc_date = Session["SPO_Date"].ToString();
                        DataSet ds = GetSPODetailEdit(Doc_no, Doc_date);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //_SPODetailModel.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                            //_SPODetailModel.POOrderType = ds.Tables[0].Rows[0]["order_type"].ToString();
                            _SPODetailModel.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();
                            //if (_SPODetailModel.Src_Type == "P") _SPODetailModel.Src_Type = "PR";
                            _SPODetailModel.SPO_No = ds.Tables[0].Rows[0]["SPO_No"].ToString();
                            _SPODetailModel.SPO_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["spo_dt"].ToString()).ToString("yyyy-MM-dd");
                            _SPODetailModel.Remarks = ds.Tables[0].Rows[0]["po_rem"].ToString();
                            _SPODetailModel.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _SPODetailModel.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _SPODetailModel.SuppID, supp_name = _SPODetailModel.SuppName });
                            _SPODetailModel.SupplierNameList = suppLists;
                            _SPODetailModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            //_SPODetailModel.SuppName = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _SPODetailModel.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bill_add_id"].ToString());
                            _SPODetailModel.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _SPODetailModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _SPODetailModel.Currency = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _SPODetailModel.Conv_Rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _SPODetailModel.ValidUpto = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                            _SPODetailModel.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _SPODetailModel.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _SPODetailModel.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _SPODetailModel.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_spec"]).ToString(ValDigit);
                            _SPODetailModel.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _SPODetailModel.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _SPODetailModel.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _SPODetailModel.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _SPODetailModel.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _SPODetailModel.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _SPODetailModel.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _SPODetailModel.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _SPODetailModel.StatusName = ds.Tables[0].Rows[0]["OrderStauts"].ToString();
                            _SPODetailModel.OrdStatus = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                            _SPODetailModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _SPODetailModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);//Cancelled
                            _SPODetailModel.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _SPODetailModel.SrcDocNo, SrcDocnoVal = _SPODetailModel.SrcDocNo });
                            _SPODetailModel.docNoLists = srcDocNoLists;
                            _SPODetailModel.Pymnt_trms = ds.Tables[0].Rows[0]["paym_terms"].ToString();
                            _SPODetailModel.Del_dstn = ds.Tables[0].Rows[0]["Delv_destn"].ToString();
                            _SPODetailModel.Amendment = ds.Tables[12].Rows[0]["Amendment"].ToString();
                            _SPODetailModel.AmendmentFlag = ds.Tables[0].Rows[0]["FlagAmend"].ToString();
                            _SPODetailModel.ForAmmendendBtn = ds.Tables[13].Rows[0]["flag"].ToString().Trim();
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _SPODetailModel.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            ViewBag.ItemDelSchdetails = ds.Tables[4];
                            ViewBag.ItemTermsdetails = ds.Tables[5];
                            create_id = ds.Tables[0].Rows[0]["createid"].ToString();   //
                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.AttechmentDetails = ds.Tables[11];
                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string Statuscode = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _SPODetailModel.Cancelled = true;
                            _SPODetailModel.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                        }
                        else
                        {
                            _SPODetailModel.Cancelled = false;
                        }
                        string ForceClose = ds.Tables[0].Rows[0]["force_close"].ToString().Trim();
                        if (ForceClose == "Y")
                        {
                            _SPODetailModel.FClosed = true;
                        }
                        else
                        {
                            _SPODetailModel.FClosed = false;
                        }
                        //Session["DocumentStatus"] = Statuscode;
                        ViewBag.DocumentStatus = Statuscode;
                        _SPODetailModel.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _SPODetailModel.Command != "Edit")
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
                                    //Session["BtnName"] = "BtnRefresh";
                                    _SPODetailModel.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SPODetailModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SPODetailModel.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SPODetailModel.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SPODetailModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SPODetailModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SPODetailModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SPODetailModel.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    _SPODetailModel.BtnName = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                    }
                    if (_SPODetailModel.AmendmentFlag == "HistoryTable")
                    {
                        _SPODetailModel.BtnName = "BtnAmendMent";
                        _SPODetailModel.Amend = null;
                    }
                    if (_SPODetailModel.Amend == "Amend")
                    {
                        _SPODetailModel.OrdStatus = "D";
                        _SPODetailModel.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                    }
                    _SPODetailModel.UserID = UserID;
                    _SPODetailModel.DocumentMenuId = DocumentMenuId;
                    _SPODetailModel.Title = title;
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseOrder/ServicePurchaseOrderDetail.cshtml", _SPODetailModel);
                }
                else
                {/*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{ TempData["Message1"] = "Financial Year not Exist";
                    //}
                    ///*End to chk Financial year exist or not*/
                    SPODetailModel _SPODetailModel1 = new SPODetailModel();
                    _SPODetailModel1.ILSearch = null;
                    ViewBag.DocumentStatus = "D";
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _SPODetailModel1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (URLModel.DocNo != null || URLModel.DocDate != null)
                    {
                        _SPODetailModel1.SPO_No = URLModel.DocNo;
                        _SPODetailModel1.SPO_Date = URLModel.DocDate;
                    }
                    if (URLModel.TransType != null)
                    {
                        _SPODetailModel1.TransType = URLModel.TransType;
                    }
                    else
                    {
                        _SPODetailModel1.TransType = "New";
                    }
                    if (URLModel.BtnName != null)
                    {
                        _SPODetailModel1.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        _SPODetailModel1.BtnName = "BtnRefresh";
                    }
                    if (URLModel.Command != null)
                    {
                        _SPODetailModel1.Command = URLModel.Command;
                    }
                    else
                    {
                        _SPODetailModel1.Command = "Refresh";
                    }
                    //var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    // SPODetailModel _SPODetailModel1 = new SPODetailModel();
                    _SPODetailModel1.ValDigit = ValDigit;
                    _SPODetailModel1.QtyDigit = QtyDigit;
                    _SPODetailModel1.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _SPODetailModel1.SupplierNameList = suppLists;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _SPODetailModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _SPODetailModel1.docNoLists = srcDocNoLists;
                    //if (Session["SPO_No"] != null && Session["SPO_Date"] != null)
                    if (_SPODetailModel1.SPO_No != null && _SPODetailModel1.SPO_Date != null)
                    {
                        string Doc_no = _SPODetailModel1.SPO_No;
                        string Doc_date = _SPODetailModel1.SPO_Date;
                        //string Doc_no = Session["SPO_No"].ToString();
                        //string Doc_date = Session["SPO_Date"].ToString();
                        DataSet ds = GetSPODetailEdit(Doc_no, Doc_date);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //_SPODetailModel1.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                            //_SPODetailModel1.POOrderType = ds.Tables[0].Rows[0]["order_type"].ToString();
                            _SPODetailModel1.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();
                            //if (_SPODetailModel1.Src_Type == "P") _SPODetailModel1.Src_Type = "PR";
                            _SPODetailModel1.SPO_No = ds.Tables[0].Rows[0]["SPO_No"].ToString();
                            _SPODetailModel1.SPO_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["spo_dt"].ToString()).ToString("yyyy-MM-dd");
                            _SPODetailModel1.Remarks = ds.Tables[0].Rows[0]["po_rem"].ToString();
                            _SPODetailModel1.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _SPODetailModel1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _SPODetailModel1.SuppID, supp_name = _SPODetailModel1.SuppName });
                            _SPODetailModel1.SupplierNameList = suppLists;
                            _SPODetailModel1.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            //_SPODetailModel1.SuppName = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _SPODetailModel1.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bill_add_id"].ToString());
                            _SPODetailModel1.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _SPODetailModel1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _SPODetailModel1.Currency = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _SPODetailModel1.Conv_Rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _SPODetailModel1.ValidUpto = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                            _SPODetailModel1.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _SPODetailModel1.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _SPODetailModel1.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _SPODetailModel1.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_spec"]).ToString(ValDigit);
                            _SPODetailModel1.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _SPODetailModel1.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _SPODetailModel1.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _SPODetailModel1.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _SPODetailModel1.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _SPODetailModel1.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _SPODetailModel1.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _SPODetailModel1.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _SPODetailModel1.StatusName = ds.Tables[0].Rows[0]["OrderStauts"].ToString();
                            _SPODetailModel1.OrdStatus = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                            _SPODetailModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _SPODetailModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);//Cancelled
                            _SPODetailModel1.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _SPODetailModel1.SrcDocNo, SrcDocnoVal = _SPODetailModel1.SrcDocNo });
                            _SPODetailModel1.docNoLists = srcDocNoLists;
                            _SPODetailModel1.Pymnt_trms = ds.Tables[0].Rows[0]["paym_terms"].ToString();
                            _SPODetailModel1.Del_dstn = ds.Tables[0].Rows[0]["Delv_destn"].ToString();
                            _SPODetailModel1.Amendment = ds.Tables[12].Rows[0]["Amendment"].ToString();
                            _SPODetailModel1.AmendmentFlag = ds.Tables[0].Rows[0]["FlagAmend"].ToString();
                            _SPODetailModel1.ForAmmendendBtn = ds.Tables[13].Rows[0]["flag"].ToString().Trim();
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _SPODetailModel1.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            ViewBag.ItemDelSchdetails = ds.Tables[4];
                            ViewBag.ItemTermsdetails = ds.Tables[5];
                            create_id = ds.Tables[0].Rows[0]["createid"].ToString();   //
                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.AttechmentDetails = ds.Tables[11];
                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string Statuscode = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _SPODetailModel1.Cancelled = true;
                            _SPODetailModel1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                        }
                        else
                        {
                            _SPODetailModel1.Cancelled = false;
                        }
                        string ForceClose = ds.Tables[0].Rows[0]["force_close"].ToString().Trim();
                        if (ForceClose == "Y")
                        {
                            _SPODetailModel1.FClosed = true;
                        }
                        else
                        {
                            _SPODetailModel1.FClosed = false;
                        }
                        //Session["DocumentStatus"] = Statuscode;
                        _SPODetailModel1.DocumentStatus = Statuscode;
                        ViewBag.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _SPODetailModel1.Command != "Edit")
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
                                    //Session["BtnName"] = "BtnRefresh";
                                    _SPODetailModel1.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SPODetailModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SPODetailModel1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SPODetailModel1.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SPODetailModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SPODetailModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SPODetailModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SPODetailModel1.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    _SPODetailModel1.BtnName = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                    }
                    if (_SPODetailModel1.AmendmentFlag == "HistoryTable")
                    {
                        _SPODetailModel1.BtnName = "BtnAmendMent";
                        _SPODetailModel1.Amend = null;
                    }
                    if (_SPODetailModel1.Amend == "Amend")
                    {
                        _SPODetailModel1.OrdStatus = "D";
                        _SPODetailModel1.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                    }
                    _SPODetailModel1.UserID = UserID;
                    _SPODetailModel1.DocumentMenuId = DocumentMenuId;
                    _SPODetailModel1.Title = title;
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseOrder/ServicePurchaseOrderDetail.cshtml", _SPODetailModel1);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetSPOList(string docid, string status)
        {
            SPOListModel _SPOListModel = new SPOListModel();
            //Session["WF_status"] = status;
            _SPOListModel.WF_status = status;
            //Session["WF_Docid"] = docid;           
            return RedirectToAction("ServicePurchaseOrder", "ServicePurchaseOrder", _SPOListModel);
        }
        public ActionResult DoubleClickOnList(string DocNo, string DocDate, string ListFilterData, string WF_status)
        {/*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    SPOListModel pqModel = new SPOListModel();
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/


            URLModelDetails URLModel = new URLModelDetails();
            SPODetailModel _SPODetailModel = new SPODetailModel();
            _SPODetailModel.Message = "New";
            _SPODetailModel.Command = "Update";
            _SPODetailModel.TransType = "Update";
            _SPODetailModel.BtnName = "BtnToDetailPage";
            _SPODetailModel.SPO_No = DocNo;
            _SPODetailModel.SPO_Date = DocDate;
            _SPODetailModel.WF_status1 = WF_status;
            TempData["ModelData"] = _SPODetailModel;
            URLModel.DocNo = DocNo;
            URLModel.DocDate = DocDate;
            URLModel.TransType = "Update";
            URLModel.Command = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["TransType"] = "Update";
            //Session["BtnName"] = "BtnToDetailPage";
            //Session["SPO_No"] = DocNo;
            //Session["SPO_Date"] = DocDate;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("ServicePurchaseOrderDetail", "ServicePurchaseOrder", URLModel);
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
        private List<ServicePurchaseOrderList> GetSPOList(SPOListModel _SPOListModel)
        {
            _ServicePurchaseOrderList = new List<ServicePurchaseOrderList>();
            try
            {
                //string User_ID = string.Empty;
                string SuppType = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                DataSet DSet = _SPO_ISERVICE.GetSPODetailList(CompID, BrchID, UserID, _SPOListModel.SuppID, _SPOListModel.PO_FromDate, _SPOListModel.PO_ToDate, _SPOListModel.Status, _SPOListModel.wfdocid, _SPOListModel.wfstatus);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ServicePurchaseOrderList _SPOList = new ServicePurchaseOrderList();
                        _SPOList.OrderNo = dr["OrderNo"].ToString();
                        _SPOList.OrderDate = dr["OrderDate"].ToString();
                        _SPOList.OrderDt = dr["OrderDt"].ToString();
                        _SPOList.SourceType = dr["SourceType"].ToString();
                        _SPOList.SourceDocNo = dr["src_doc_number"].ToString();
                        _SPOList.SuppName = dr["supp_name"].ToString();
                        _SPOList.Currency = dr["curr"].ToString();
                        _SPOList.OrderValue = dr["net_val_bs"].ToString();
                        _SPOList.OrderStauts = dr["OrderStauts"].ToString();
                        _SPOList.CreateDate = dr["CreateDate"].ToString();
                        _SPOList.ApproveDate = dr["ApproveDate"].ToString();
                        _SPOList.ModifyDate = dr["ModifyDate"].ToString();
                        _SPOList.create_by = dr["create_by"].ToString();
                        _SPOList.app_by = dr["app_by"].ToString();
                        _SPOList.mod_by = dr["mod_by"].ToString();
                        _ServicePurchaseOrderList.Add(_SPOList);
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
            return _ServicePurchaseOrderList;
        }
        [HttpPost]
        public ActionResult SearchSPODetail(string SuppId, string Fromdate, string Todate, string Status)
        {
            SPOListModel _SPOListModel = new SPOListModel();
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

                SuppType = "L";

                _ServicePurchaseOrderList = new List<ServicePurchaseOrderList>();
                DataSet DSet = _SPO_ISERVICE.GetSPODetailList(CompID, BrchID, User_ID, SuppId, Fromdate, Todate, Status, "0", "");
                _SPOListModel.SPOSearch = "SPO_Search";

                foreach (DataRow dr in DSet.Tables[0].Rows)
                {
                    ServicePurchaseOrderList _SPOList = new ServicePurchaseOrderList();
                    _SPOList.OrderNo = dr["OrderNo"].ToString();
                    _SPOList.OrderDate = dr["OrderDate"].ToString();
                    _SPOList.OrderDt = dr["OrderDt"].ToString();
                    _SPOList.SourceType = dr["SourceType"].ToString();
                    _SPOList.SourceDocNo = dr["src_doc_number"].ToString();
                    _SPOList.SuppName = dr["supp_name"].ToString();
                    _SPOList.Currency = dr["curr"].ToString();
                    _SPOList.OrderValue = dr["net_val_bs"].ToString();
                    _SPOList.OrderStauts = dr["OrderStauts"].ToString();
                    _SPOList.CreateDate = dr["CreateDate"].ToString();
                    _SPOList.ApproveDate = dr["ApproveDate"].ToString();
                    _SPOList.ModifyDate = dr["ModifyDate"].ToString();
                    _SPOList.create_by = dr["create_by"].ToString();
                    _SPOList.app_by = dr["app_by"].ToString();
                    _SPOList.mod_by = dr["mod_by"].ToString();
                    _ServicePurchaseOrderList.Add(_SPOList);
                }
                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                ViewBag.FinStDt = DSet.Tables[2].Rows[0]["findate"];
                _SPOListModel.SPOList = _ServicePurchaseOrderList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSPOList.cshtml", _SPOListModel);
        }
        public ActionResult GetAutoCompleteSearchSuppList(SPOListModel _SPOListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string SuppType = string.Empty;
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
                if (string.IsNullOrEmpty(_SPOListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _SPOListModel.SuppName;
                }

                SuppList = _SPO_ISERVICE.GetSupplierList(CompID, SupplierName, BrchID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in SuppList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _SPOListModel.SupplierNameList = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        private DataTable GetRoleList()//done
        {
            try
            {

                //string UserID = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
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
        public DataSet GetSPODetailEdit(string SPO_No, string SPO_Date)
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
                //DocumentMenuId = "105101135";       
                DataSet result = _SPO_ISERVICE.GetSPODetailDAL(CompID, BrchID, SPO_No, SPO_Date, UserID, DocumentMenuId);
                //DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                throw Ex;
            }
        }
        public ActionResult SPODetailSave(SPODetailModel _Model, string Command)
        {
            try
            {/*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                //PODetailsModel _Model = new PODetailsModel();
                if (_Model.Delete == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "AddNew":
                        SPODetailModel _SPODetailModel = new SPODetailModel();
                        _SPODetailModel.Command = "Add";
                        _SPODetailModel.TransType = "Save";
                        _SPODetailModel.BtnName = "BtnAddNew";
                        ViewBag.DocumentStatus = "D";
                        TempData["ModelData"] = _SPODetailModel;
                        //Session["Message"] = null;
                        //Session["SPO_No"] = null;
                        //Session["SPO_Date"] = null;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_Model.SPO_No))
                                return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.SPO_No, DocDate = _Model.SPO_Date, ListFilterData = _Model.ListFilterData1, WF_status = _Model.WFStatus });
                            else
                                _SPODetailModel.Command = "Refresh";
                            _SPODetailModel.TransType = "Refresh";
                            _SPODetailModel.BtnName = "BtnRefresh";
                            _SPODetailModel.DocumentStatus = null;
                            TempData["ModelData"] = _SPODetailModel;
                            return RedirectToAction("ServicePurchaseOrderDetail", _SPODetailModel);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ServicePurchaseOrderDetail");
                    case "Save":
                        _Model.TransType = Command;
                        if (_Model.Amend != null && _Model.Amend != "" && _Model.Amendment == null)
                        {
                            _Model.TransType = "Amendment";

                        }
                        InsertSPODetails(_Model);
                        if (_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        _Model.TransType = "Update";
                        _Model.Command = Command;
                        _Model.BtnName = "BtnSave";
                        _Model.Amend = null;
                        TempData["ModelData"] = _Model;
                        URLModelDetails URLModel = new URLModelDetails();
                        URLModel.BtnName = "BtnSave";
                        URLModel.Command = Command;
                        URLModel.TransType = "Update";
                        URLModel.DocNo = _Model.SPO_No;
                        URLModel.DocDate = _Model.SPO_Date;

                        //Session["TransType"] = "Update";
                        //Session["Command"] = Command;
                        //Session["BtnName"] = "BtnSave";
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("ServicePurchaseOrderDetail", URLModel);
                    case "Update":
                        _Model.TransType = Command;
                        if (_Model.Amend != null && _Model.Amend != "" && _Model.Amendment == null)
                        {
                            _Model.TransType = "Amendment";

                        }
                        InsertSPODetails(_Model);
                        if (_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        _Model.TransType = "Update";
                        _Model.Command = Command;
                        _Model.BtnName = "BtnSave";
                        URLModelDetails URLModelUpdate = new URLModelDetails();
                        URLModelUpdate.BtnName = "BtnSave";
                        URLModelUpdate.Command = Command;
                        URLModelUpdate.TransType = "Update";
                        URLModelUpdate.DocNo = _Model.SPO_No;
                        URLModelUpdate.DocDate = _Model.SPO_Date;
                        //Session["TransType"] = "Update";
                        //Session["Command"] = Command;
                        //Session["BtnName"] = "BtnSave";
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        if (_Model.OrdStatus == "PDL" || _Model.OrdStatus == "PR" || _Model.OrdStatus == "PN")
                        {
                            //ession["BtnName"] = "BtnToDetailPage";
                            _Model.BtnName = "BtnToDetailPage";
                            URLModelUpdate.BtnName = "BtnToDetailPage";
                        }
                        TempData["ModelData"] = _Model;
                        return RedirectToAction("ServicePurchaseOrderDetail", URLModelUpdate);
                    case "Approve":
                        /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        //string SPODate1 = _Model.SPO_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SPODate1) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.SPO_No, DocDate = _Model.SPO_Date, ListFilterData = _Model.ListFilterData1, WF_status = _Model.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        //_Model.TransType = Command;
                        InsertSPOApproveDetails(_Model);
                        URLModelDetails URLModelApprove = new URLModelDetails();
                        URLModelApprove.Command = Command;
                        URLModelApprove.BtnName = "BtnApprove";
                        URLModelApprove.DocNo = _Model.SPO_No;
                        URLModelApprove.DocDate = _Model.SPO_Date;
                        URLModelApprove.TransType = _Model.TransType;
                        _Model.Amend = "";//Added by Suraj Maurya on 12-12-2025 to resolve toolbar issue
                        TempData["ModelData"] = _Model;
                        //Session["Command"] = Command;
                        //Session["BtnName"] = "BtnApprove";
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("ServicePurchaseOrderDetail", URLModelApprove);
                    case "Edit":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.SPO_No, DocDate = _Model.SPO_Date, ListFilterData = _Model.ListFilterData1, WF_status = _Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        //string SPO_Date2 = _Model.SPO_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SPO_Date2) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.SPO_No, DocDate = _Model.SPO_Date, ListFilterData = _Model.ListFilterData1, WF_status = _Model.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        URLModelDetails URLModelEdit = new URLModelDetails();
                        if (_Model.OrdStatus == "A")
                        {
                            if (CheckDeliveryNoteAgainstDPO(_Model.SPO_No, _Model.SPO_Date))
                            {
                                //Session["Message"] = "deliverynoteunderprocess";
                                _Model.Message = "deliverynoteunderprocess";
                                URLModelEdit.DocDate = _Model.SPO_Date;
                                URLModelEdit.DocNo = _Model.SPO_No;
                                URLModelEdit.TransType = _Model.TransType;
                                URLModelEdit.Command = _Model.Command;
                                URLModelEdit.BtnName = _Model.BtnName;
                            }
                            else
                            {
                                //Session["Message"] = null;
                                //Session["Command"] = Command;
                                //Session["TransType"] = "Update";
                                //Session["BtnName"] = "BtnEdit";
                                _Model.Message = null;
                                _Model.Command = Command;
                                _Model.TransType = "Update";
                                _Model.BtnName = "BtnEdit";
                                URLModelEdit.DocDate = _Model.SPO_Date;
                                URLModelEdit.DocNo = _Model.SPO_No;
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.Command = Command;
                                URLModelEdit.BtnName = "BtnEdit";
                            }
                        }
                        else if (_Model.OrdStatus == "PDL" || _Model.OrdStatus == "PR" || _Model.OrdStatus == "PN")
                        {
                            if (CheckPurchaseOrderQtyforForceclosed(_Model.SPO_No, _Model.SPO_Date))
                            {
                                //Session["Message"] = null;
                                //Session["Command"] = Command;
                                //Session["TransType"] = "Update";
                                //Session["BtnName"] = "BtnEdit";
                                _Model.Message = null;
                                _Model.Command = Command;
                                _Model.TransType = "Update";
                                _Model.BtnName = "BtnEdit";
                                URLModelEdit.DocDate = _Model.SPO_Date;
                                URLModelEdit.DocNo = _Model.SPO_No;
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.Command = Command;
                                URLModelEdit.BtnName = "BtnEdit";
                            }
                        }
                        else
                        {
                            //Session["Message"] = null;
                            //Session["Command"] = Command;
                            //Session["TransType"] = "Update";
                            //Session["BtnName"] = "BtnEdit";
                            _Model.Message = null;
                            _Model.Command = Command;
                            _Model.TransType = "Update";
                            _Model.BtnName = "BtnEdit";
                            URLModelEdit.DocDate = _Model.SPO_Date;
                            URLModelEdit.DocNo = _Model.SPO_No;
                            URLModelEdit.TransType = "Update";
                            URLModelEdit.Command = Command;
                            URLModelEdit.BtnName = "BtnEdit";
                        }
                        TempData["ModelData"] = _Model;
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("ServicePurchaseOrderDetail", URLModelEdit);
                    case "Delete":
                        DeleteSPODetails(_Model.SPO_No, _Model.SPO_Date, _Model.Title);
                        SPODetailModel _SPODetailModelDelete = new SPODetailModel();
                        _SPODetailModelDelete.Command = "Refresh";
                        _SPODetailModelDelete.TransType = "New";
                        _SPODetailModelDelete.BtnName = "BtnRefresh";
                        _SPODetailModelDelete.Message = "Deleted";
                        TempData["ModelData"] = _SPODetailModelDelete;
                        //Session["SPO_No"] = null;
                        //Session["SPO_Date"] = null;
                        //Session["Command"] = "Refresh";
                        //Session["TransType"] = "New";
                        //Session["BtnName"] = "BtnRefresh";
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("ServicePurchaseOrderDetail");
                    case "Amendment":
                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.SPO_No, DocDate = _Model.SPO_Date, ListFilterData = _Model.ListFilterData1, WF_status = _Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        //string SPO_Date3 = _Model.SPO_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SPO_Date3) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.SPO_No, DocDate = _Model.SPO_Date, ListFilterData = _Model.ListFilterData1, WF_status = _Model.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        _Model.Amend = "Amend";
                        _Model.Message = null;
                        _Model.Command = "Edit";
                        _Model.TransType = "Update";
                        _Model.BtnName = "BtnEdit";
                        _Model.SPO_Date = _Model.SPO_Date;
                        _Model.SPO_No = _Model.SPO_No;
                        URLModelDetails URLModelAmend = new URLModelDetails();
                        URLModelAmend.DocDate = _Model.SPO_Date;
                        URLModelAmend.DocNo = _Model.SPO_No;
                        URLModelAmend.TransType = "Update";
                        URLModelAmend.Command = Command;
                        URLModelAmend.BtnName = "BtnEdit";
                        TempData["ModelData"] = _Model;
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("ServicePurchaseOrderDetail", URLModelAmend);
                    case "Print":
                        return GenratePdfFile(_Model);
                    case "Refresh":
                        SPODetailModel _SPODetailModelRefresh = new SPODetailModel();
                        _SPODetailModelRefresh.Command = Command;
                        _SPODetailModelRefresh.TransType = "New";
                        _SPODetailModelRefresh.BtnName = "BtnRefresh";
                        TempData["ModelData"] = _SPODetailModelRefresh;
                        //Session["Message"] = null;
                        //Session["Command"] = Command;
                        //Session["TransType"] = "New";
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["SPO_No"] = null;
                        //Session["SPO_Date"] = null;
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("ServicePurchaseOrderDetail");
                    case "BacktoList":
                        //Session["Message"] = null;
                        //Session["SPO_No"] = null;
                        //Session["SPO_Date"] = null;
                        TempData["WF_status"] = _Model.WF_status1;
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("ServicePurchaseOrder");
                }
                TempData["ListFilterData"] = _Model.ListFilterData1;
                return RedirectToAction("ServicePurchaseOrderDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType,string Mailerror)
        {
            //Session["Message"] = "";
            SPODetailModel _SPODetailModel = new SPODetailModel();
            var a = TrancType.Split(',');
            _SPODetailModel.SPO_No = a[0].Trim();
            _SPODetailModel.SPO_Date = a[1].Trim();
            _SPODetailModel.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            //_SPODetailModel.WF_status1 = WF_status1;
            _SPODetailModel.BtnName = "BtnToDetailPage";
            _SPODetailModel.Message = Mailerror;
            URLModelDetails URLModel = new URLModelDetails();
            URLModel.DocNo = a[0].Trim();
            URLModel.DocDate = a[01].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _SPODetailModel;
            TempData["WF_status1"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ServicePurchaseOrderDetail", "ServicePurchaseOrder", URLModel);
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
        public ActionResult InsertSPODetails(SPODetailModel _Model)
        {
            //getDocumentName(); /* To set Title*/
            string PageName = _Model.Title.Replace(" ", "");
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            string FSOId = string.Empty;
            string FinalSOId = string.Empty;
            string Comp_ID = string.Empty;
            string UserID = string.Empty;
            string BranchID = string.Empty;
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
                DataTable DtblDeliSchDetail = new DataTable();
                DataTable DtblTermsDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                if (_Model.Itemdetails != null)
                {
                    DtblHDetail = ToDtblHDetail(_Model);
                    DtblItemDetail = ToDtblItemDetail(_Model.Itemdetails);
                    DtblTaxDetail = ToDtblTaxDetail(_Model.ItemTaxdetails);
                    DtblOCDetail = ToDtblOCDetail(_Model.ItemOCdetails);
                    DtblDeliSchDetail = ToDtblDelSchDetail(_Model.ItemDelSchdetails);
                    DtblTermsDetail = ToDtblTermsDetail(_Model.ItemTermsdetails);
                    DtblAttchDetail = BindAttachData(DtblHDetail, _Model.attatchmentdetail);


                    FSOId = _SPO_ISERVICE.InsertSPODetails(DtblHDetail, DtblItemDetail, DtblTaxDetail, DtblOCDetail, DtblDeliSchDetail, DtblTermsDetail, DtblAttchDetail);

                    string PONo = FSOId.Split(',')[0];
                    string PODate = FSOId.Split(',')[1];
                    string Status = FSOId.Split(',')[3];
                    string transtype = FSOId.Split(',')[4]; ;
                    //string transtype = DtblHDetail.Rows[0]["TransType"].ToString();
                    if (PONo == "DataNotFound")
                    {
                        var msg = "Data Not found" + " " + PODate + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _Model.Message = PONo;
                        return RedirectToAction("ServicePurchaseOrderDetail");
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
                            //string fileName = "SPO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = _Model.SPO_No.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_Model, _Model.SPO_No, _Model.SPO_Date, fileName,_Model.DocumentMenuId,"C");//_Model Added by Nidhi 05-05-25
                            _Common_IServices.SendAlertEmail(CompID, BrchID, _Model.DocumentMenuId, _Model.SPO_No, "C", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        //Session["Message"] = "Cancelled";
                        //_Model.Message = "Cancelled";
                        _Model.Message = _Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";

                    }
                    _Model.SPO_No = PONo;
                    //Session["SPO_No"] = PONo;
                    _Model.SPO_Date = PODate;
                    //Session["SPO_Date"] = PODate;
                    //ResetImageLocation(PageName, PONo, transtype, DtblAttchDetail);
                    if (TempData["ModelDataattch"] != null)
                    {
                        var _attachModel = TempData["ModelDataattch"] as SPODetailModelattch;
                        TempData["ModelDataattch"] = null;
                        string guid = _attachModel.Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, PONo, _Model.TransType, DtblAttchDetail);
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

                getDocumentName(); /* To set Title*/
                PageName = title.Replace(" ", "");
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
        private DataTable ToDtblHDetail(SPODetailModel _Model)
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
                //dtheader.Columns.Add("OrderType", typeof(string));
                dtheader.Columns.Add("FClosed", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("SPO_No", typeof(string));
                dtheader.Columns.Add("SPO_Date", typeof(string));
                dtheader.Columns.Add("Src_Type", typeof(string));
                dtheader.Columns.Add("SrcDocNo", typeof(string));
                dtheader.Columns.Add("SrcDocDate", typeof(string));
                dtheader.Columns.Add("SuppName", typeof(int));
                dtheader.Columns.Add("Currency", typeof(int));
                dtheader.Columns.Add("Conv_Rate", typeof(string));
                dtheader.Columns.Add("ValidUpto", typeof(string));
                dtheader.Columns.Add("Remarks", typeof(string));
                dtheader.Columns.Add("CompID", typeof(string));
                dtheader.Columns.Add("BranchID", typeof(string));
                dtheader.Columns.Add("UserID", typeof(int));
                dtheader.Columns.Add("GrVal", typeof(string));
                //dtheader.Columns.Add("DiscAmt", typeof(string));
                dtheader.Columns.Add("TaxAmt", typeof(string));
                dtheader.Columns.Add("OcAmt", typeof(string));
                dtheader.Columns.Add("NetValBs", typeof(string));
                dtheader.Columns.Add("NetValSpec", typeof(string));//
                dtheader.Columns.Add("OrdStatus", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(int));
                //dtheader.Columns.Add("price_basis", typeof(string));            
                dtheader.Columns.Add("SystemDetail", typeof(string));
                dtheader.Columns.Add("paym_terms", typeof(int));
                dtheader.Columns.Add("Delv_destn", typeof(string));
                dtheader.Columns.Add("cancel_remarks", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _Model.TransType;
                //dtrowHeader["OrderType"] = _Model.POOrderType;
                dtrowHeader["FClosed"] = ConvertBoolToStrint(_Model.FClosed);
                dtrowHeader["Cancelled"] = ConvertBoolToStrint(_Model.Cancelled);
                dtrowHeader["SPO_No"] = _Model.SPO_No;
                dtrowHeader["SPO_Date"] = _Model.SPO_Date;
                dtrowHeader["Src_Type"] = _Model.Src_Type;
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
                dtrowHeader["Currency"] = _Model.curr_id;
                dtrowHeader["Conv_Rate"] = _Model.Conv_Rate;
                dtrowHeader["ValidUpto"] = _Model.ValidUpto;
                dtrowHeader["Remarks"] = _Model.Remarks;
                dtrowHeader["CompID"] = CompID;
                dtrowHeader["BranchID"] = BrchID;
                dtrowHeader["UserID"] = UserID;
                dtrowHeader["GrVal"] = IsNull(_Model.GrVal, "0");
                //dtrowHeader["DiscAmt"] = IsNull(_Model.DiscAmt, "0");
                dtrowHeader["TaxAmt"] = IsNull(_Model.TaxAmt, "0");
                dtrowHeader["OcAmt"] = IsNull(_Model.OcAmt, "0");
                dtrowHeader["NetValBs"] = IsNull(_Model.NetValBs, "0");
                dtrowHeader["NetValSpec"] = IsNull(_Model.NetValSpec, "0");
                dtrowHeader["OrdStatus"] = IsNull(_Model.OrdStatus, "D");
                dtrowHeader["bill_add_id"] = _Model.bill_add_id;
                
                //dtrowHeader["price_basis"] = _Model.PriceBasis;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["SystemDetail"] = mac_id;
                if (_Model.Pymnt_trms != null)
                {
                    dtrowHeader["paym_terms"] = _Model.Pymnt_trms;
                }
                else
                {
                    dtrowHeader["paym_terms"] = 0;
                }
                if (_Model.Del_dstn != null)
                {
                    dtrowHeader["Delv_destn"] = _Model.Del_dstn;
                }
                else
                {
                    dtrowHeader["Delv_destn"] = "";
                }
                dtrowHeader["cancel_remarks"] = _Model.CancelledRemarks;
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
        private DataTable ToDtblItemDetail(string pQItemDetail)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("HSN_No", typeof(string));
                dtItem.Columns.Add("OrderQty", typeof(string));
                dtItem.Columns.Add("InvQty", typeof(string));
                dtItem.Columns.Add("ItmRate", typeof(string));
                dtItem.Columns.Add("GrossVal", typeof(string));
                dtItem.Columns.Add("TaxAmt", typeof(string));
                dtItem.Columns.Add("OCAmt", typeof(string));
                dtItem.Columns.Add("NetValSpec", typeof(string));
                dtItem.Columns.Add("NetValBase", typeof(string));
                dtItem.Columns.Add("Remarks", typeof(string));
                dtItem.Columns.Add("TaxExempted", typeof(string));
                dtItem.Columns.Add("ManualGST", typeof(string));
                dtItem.Columns.Add("sr_no", typeof(int));
                if (pQItemDetail != null)
                {
                    JArray jObject = JArray.Parse(pQItemDetail);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["HSN_No"] = jObject[i]["Hsn_no"].ToString();
                        dtrowLines["OrderQty"] = jObject[i]["OrderQty"].ToString();
                        dtrowLines["InvQty"] = jObject[i]["InvQty"].ToString();
                        dtrowLines["ItmRate"] = jObject[i]["ItmRate"].ToString();
                        dtrowLines["GrossVal"] = jObject[i]["GrossVal"].ToString();
                        dtrowLines["TaxAmt"] = jObject[i]["TaxAmt"].ToString();
                        dtrowLines["OCAmt"] = jObject[i]["OCAmt"].ToString();
                        dtrowLines["NetValSpec"] = jObject[i]["NetValSpec"].ToString();
                        dtrowLines["NetValBase"] = jObject[i]["NetValBase"].ToString();
                        dtrowLines["Remarks"] = jObject[i]["Remarks"].ToString();
                        dtrowLines["TaxExempted"] = jObject[i]["TaxExempted"].ToString();
                        dtrowLines["ManualGST"] = jObject[i]["ManualGST"].ToString();
                        dtrowLines["sr_no"] = Convert.ToInt32(jObject[i]["sr_on"].ToString());
                        dtItem.Rows.Add(dtrowLines);
                    }
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
                if (OCDetails != null)
                {
                    JArray jObject = JArray.Parse(OCDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["OC_ID"] = jObject[i]["OC_ID"].ToString();
                        dtrowLines["OCValue"] = jObject[i]["OCValue"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    DtblItemOCDetail = dtItem;
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
        public Boolean CheckDeliveryNoteAgainstDPO(string DocNo, string DocDate)
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
                DataSet Deatils = _SPO_ISERVICE.CheckDeliveryNoteDPO(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = true;
                }
                //DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
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
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1)
        {
            //JArray jObjectBatch = JArray.Parse(list);
            SPODetailModel _Model = new SPODetailModel();
            URLModelDetails URLModel = new URLModelDetails();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _Model.SPO_No = jObjectBatch[i]["PONo"].ToString();
                    _Model.SPO_Date = jObjectBatch[i]["PODate"].ToString();
                    _Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (_Model.A_Status != "Approve")
            {
                _Model.A_Status = "Approve";
            }
            /*----------------Added by Suraj on 16-08-2024 Reason : to set default Values---------------*/
            _Model.TransType = "Update";
            _Model.BtnName = "BtnToDetailPage";
            _Model.Command = "Update";
            /*----------------Added by Suraj on 16-08-2024 Reason : to set default Values End---------------*/
            InsertSPOApproveDetails(_Model);

            TempData["ModelData"] = _Model;
            TempData["WF_status1"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            URLModel.DocNo = _Model.SPO_No;
            URLModel.DocDate = _Model.SPO_Date;
            URLModel.TransType = _Model.TransType;
            URLModel.BtnName = _Model.BtnName;
            URLModel.Command = _Model.Command;
            return RedirectToAction("ServicePurchaseOrderDetail", URLModel);
        }
        public JsonResult InsertSPOApproveDetails(SPODetailModel _Model)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
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
                var PONo = _Model.SPO_No;
                var PODate = _Model.SPO_Date;
                var A_Status = _Model.A_Status;
                var A_Level = _Model.A_Level;
                var A_Remarks = _Model.A_Remarks;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string POId = _SPO_ISERVICE.InsertSPOApproveDetail(PONo, PODate, Br_ID, DocumentMenuId, Comp_ID, UserID, mac_id, A_Status, A_Level, A_Remarks);
                try
                {
                   // string fileName = PONo.Replace("/", "") + ".pdf";
                    string fileName = PONo.Replace("/", "") + "_" +  System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(_Model, PONo, PODate, fileName,DocumentMenuId, "A");//_Model Added by Nidhi 05-05-25
                    _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, PONo, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _Model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }

                //Session["SPO_No"] = POId.Split(',')[0];
                //Session["Message"] = "Approved";
                _Model.SPO_No = POId.Split(',')[0];
                //_Model.Message = "Approved";
                _Model.Message = _Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                _Model.BtnName = "BtnApprove";
                _Model.Command = "Update";
                _Model.DocumentMenuId = DocumentMenuId;
                _Model.TransType = "Update";

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
        public JsonResult DeleteSPODetails(string SPONo, string SPODate, string title)
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
                string PODelete = _SPO_ISERVICE.SPO_Delete(Comp_ID, Br_ID, SPONo, SPODate);
                //Session["Message"] = "Deleted";
                if (!string.IsNullOrEmpty(SPONo))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string SPONo1 = SPONo.Replace("/", "");
                    other.DeleteTempFile(Comp_ID + Br_ID, PageName, SPONo1, Server);
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
                DataSet Deatils = _SPO_ISERVICE.CheckLPOQty_ForceClosed(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = true;
                }
                //DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
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
        public FileResult GenratePdfFile(SPODetailModel _Model)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("ShowDeliverySchedule", typeof(string));/*Add by Hina sharma on 26-12-2024*/
            dt.Columns.Add("ShowHsnNumber", typeof(string));/*Add by Hina sharma on 26-12-2024*/
            dt.Columns.Add("ShowRemarksBlwItm", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));
            dt.Columns.Add("ShowSuppAliasName", typeof(string));
            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = _Model.PrintFormat;
            dtr["ShowProdDesc"] = _Model.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = _Model.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = _Model.ShowProdTechDesc;
            dtr["ShowSubItem"] = _Model.ShowSubItem;
            dtr["ShowDeliverySchedule"] = _Model.ShowDeliverySchedule;
            dtr["ShowHsnNumber"] = _Model.ShowHSNNumber;
            dtr["ShowRemarksBlwItm"] = _Model.ShowRemarksBlwItm;
            dtr["ShowTotalQty"] = _Model.ShowTotalQty;
            dtr["ShowSuppAliasName"] = _Model.ShowSupplierAliasName;
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            return File(GetPdfData(dt, _Model.SPO_No, _Model.SPO_Date), "application/pdf", _Model.SPO_No + ".pdf");
        }
        public byte[] GetPdfData(DataTable dt, string spoNo, string spoDate)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
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
                Byte[] bytes;
                DataSet Deatils = _SPO_ISERVICE.GetPurchaseOrderDeatils(CompID, BrchID, spoNo, spoDate);
                ViewBag.PageName = "SPO";
                //ViewBag.Title = "Purchase Order";
                ViewBag.Title = "Work Order";
                ViewBag.Details = Deatils;
                ViewBag.InvoiceTo = "Invoice to:";
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                ViewBag.Website = Deatils.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                //Added By Suraj on 03-04-2024 Start
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp)
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Deatils.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Deatils.Tables[0].Rows[0]["digi_sign"].ToString();
                ViewBag.DocumentID = DocumentMenuId;
                //Added By Suraj on 03-04-2024 End
                string htmlcontent = "";
                if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")//Added By Hina sharma on 25-10-2024 only for format 2
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseOrder/SPOPrint.cshtml"));
                }
                else
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseOrder/SPOPrint.cshtml"));
                }
                #region Commented By Nitesh 25-12-2023 for Delivery Schedule in One Page
                #endregion
                // string DelSchedule = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_PrintReportDeliverySchedule.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {

                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);

                    /**Commented By Nitesh 25-12-2023 for Delivery Schedule in One Page **/
                    //  reader = new StringReader(DelSchedule);
                    // pdfDoc.NewPage();
                    // XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }

                }
                return bytes.ToArray();
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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
            string SPO_No = DtblHDetail.Rows[0]["SPO_No"].ToString();
            var _SPODetailModelattch = TempData["ModelDataattch"] as SPODetailModelattch;
            if (POAttachDeatil != null)
            {
                if (_SPODetailModelattch != null)
                {
                    //if (Session["AttachMentDetailItmStp"] != null)
                    if (_SPODetailModelattch.AttachMentDetailItmStp != null)
                    {
                        //dtAttach = Session["AttachMentDetailItmStp"] as DataTable;
                        dtAttach = _SPODetailModelattch.AttachMentDetailItmStp as DataTable;
                        //Session["AttachMentDetailItmStp"] = null;
                    }
                }
            }

            if (dtAttach.Rows.Count > 0)
            {
                foreach (DataRow dr in dtAttach.Rows)
                {
                    DataRow dtrowAttachment1 = dtAttachment.NewRow();
                    if (!string.IsNullOrEmpty(SPO_No))
                    {
                        dtrowAttachment1["id"] = SPO_No;
                    }
                    else
                    {
                        dtrowAttachment1["id"] = "0";
                    }
                    string path = dr["file_path"].ToString();
                    string file_name = dr["file_name"].ToString();
                    //if (Transtype == "Update")
                    //{
                    //    file_name = CompID + BrchID + SPO_No.Replace("/", "") + "_" + file_name;
                    //    path = dr["file_path"].ToString() + file_name;
                    //}
                    dtrowAttachment1["file_path"] = path;
                    dtrowAttachment1["file_name"] = file_name;
                    dtrowAttachment1["file_def"] = "Y";
                    dtrowAttachment1["comp_id"] = CompID;
                    dtAttachment.Rows.Add(dtrowAttachment1);
                }

            }
            return dtAttachment;
        }
        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                SPODetailModelattch _SPODetailModelattch = new SPODetailModelattch();
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
                //if (Session["SPO_No"] != null)
                //{
                //    PONo = Session["SPO_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _SPODetailModelattch.Guid = DocNo;
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
                    _SPODetailModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _SPODetailModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _SPODetailModelattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult GetPOAttatchDetailEdit(string SPO_No, string SPO_Date)
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
                DataSet result = _SPO_ISERVICE.GetPOAttatchDetailEdit(CompID, BrchID, SPO_No, SPO_Date);
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
        [HttpPost]
        public JsonResult GetSuppAddrDetail(string Supp_id)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _SPO_ISERVICE.GetSuppAddrDetail(Supp_id, Comp_ID);
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
        //public string SavePdfDocToSendOnEmailAlert(SPODetailModel _Model, string spoNo, string spoDate, string fileName)
        //{
        //    //DataTable dt = new DataTable();
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("PrintFormat", typeof(string));
        //    dt.Columns.Add("ShowProdDesc", typeof(string));
        //    dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
        //    dt.Columns.Add("ShowProdTechDesc", typeof(string));
        //    dt.Columns.Add("ShowSubItem", typeof(string));
        //    dt.Columns.Add("ShowDeliverySchedule", typeof(string));
        //    dt.Columns.Add("ShowHsnNumber", typeof(string));
        //    dt.Columns.Add("ShowRemarksBlwItm", typeof(string));
        //    dt.Columns.Add("ShowTotalQty", typeof(string));
        //    dt.Columns.Add("ShowSuppAliasName", typeof(string));
        //    DataRow dtr = dt.NewRow();
        //    dtr["PrintFormat"] = _Model.PrintFormat;
        //    dtr["ShowProdDesc"] = _Model.ShowProdDesc;
        //    dtr["ShowCustSpecProdDesc"] = _Model.ShowCustSpecProdDesc;
        //    dtr["ShowProdTechDesc"] = _Model.ShowProdTechDesc;
        //    dtr["ShowSubItem"] = _Model.ShowSubItem;
        //    dtr["ShowDeliverySchedule"] = _Model.ShowDeliverySchedule;
        //    dtr["ShowHsnNumber"] = _Model.ShowHSNNumber;
        //    dtr["ShowRemarksBlwItm"] = _Model.ShowRemarksBlwItm;
        //    dtr["ShowTotalQty"] = _Model.ShowTotalQty;
        //    dtr["ShowSuppAliasName"] = _Model.ShowSupplierAliasName;
        //    dt.Rows.Add(dtr);
        //    ViewBag.PrintOption = dt;
        //    var data = GetPdfData(dt, spoNo, spoDate);
        //    var commonCont = new CommonController(_Common_IServices);
        //    return commonCont.SaveAlertDocument(data, fileName);
        //}
        public string SavePdfDocToSendOnEmailAlert(SPODetailModel _Model, string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
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
            var commonCont = new CommonController(_Common_IServices);
            string mailattch = commonCont.CheckMailAttch(Comp_ID, Br_ID,docid, docstatus);
            if (!string.IsNullOrEmpty(mailattch))
            {
                if (mailattch.Trim() == "Yes")
                {
                    var dt = PrintOptionsDataTable(_Model);
                    ViewBag.PrintOption = dt;
                    var data = GetPdfData(dt, Doc_no, Doc_dt);
                    return commonCont.SaveAlertDocument(data, fileName);
                }
            }
            return null;
        }
        public ActionResult GetSPOTrackingDetail(string SPONo, string SPODate, string SuppName)
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
                DataSet result = _SPO_ISERVICE.GetSPOTrackingDetail(Comp_ID, BranchID, SPONo, SPODate);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.SPOTrackingList = result.Tables[0];
                ViewBag.spo_no = SPONo;
                ViewBag.spo_date = SPODate;
                ViewBag.suppName = SuppName;

                return View("~/Areas/ApplicationLayer/Views/Shared/ServicePurchaseOrderTracking.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        [HttpPost]
        public JsonResult GetSourceDocList()
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
                DataSet Details = _SPO_ISERVICE.GetSourceDocList(Comp_ID, Br_ID);
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
        public JsonResult GetDetailsAgainstPR(string Doc_no, string Doc_date)
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
                DataSet Details = _SPO_ISERVICE.GetDetailsAgainstQuotationOrPR(Comp_ID, Br_ID, Doc_no, Doc_date);

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
        public string SavePdfDocToSendOnEmailAlert_Ext(string Doc_no, string Doc_dt, string docid, string fileName, string PrintFormat)
        {
            DataTable dt;
            var commonCont = new CommonController(_Common_IServices);

            if (!string.IsNullOrEmpty(PrintFormat))
            {
                dt = commonCont.PrintOptionsDt(PrintFormat);
            }
            else
            {
                var model = new SPODetailModel(); 
                dt = PrintOptionsDataTable(model);
            }
            ViewBag.PrintOption = dt;
            var data = GetPdfData(dt, Doc_no, Doc_dt);
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
        public ActionResult SendEmailAlert(SPODetailModel _model, string mail_id, string status, string docid, string SrcType, string Doc_no, string Doc_dt, string statusAM, string filepath)
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
                //var _Model = TempData["ModelData"] as PODetailsModel;
                DataTable dt = new DataTable();
                string message = "";
                string mail_cont = "";
                string file_path = "";
                if (status == "A")
                {
                    try
                    {
                        if (filepath == "" || filepath == null)
                        {
                            string fileName = "SPO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            filepath = SavePdfDocToSendOnEmailAlert_Ext(Doc_no, Doc_dt, docid, fileName, "");
                        }
                        string keyword = @"\ExternalEmailAlertPDFs\";
                        int index = filepath.IndexOf(keyword);
                        file_path = (index >= 0) ? filepath.Substring(index) : filepath;
                        message = _Common_IServices.SendAlertEmailExternal(CompID, BrchID, UserID, docid, Doc_no, "A", mail_id, filepath);
                        if (message.Contains(","))
                        {
                            var a = message.Split(',');
                            message = a[0];
                            mail_cont = a[1];
                        }
                        if (message == "success")
                        {
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            if (message == "invalidemail")
                            {
                                mail_cont = "Invalid email body configuration";
                            }
                            if (message == "invalid")
                            {
                                mail_cont = "Invalid sender email configuration";
                            }
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                    catch (Exception exMail)
                    {
                        message = "ErrorInMail";
                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                }
                if (status == "C" || status == "FC" || status == "AM")
                {
                    try
                    {
                        message = _Common_IServices.SendAlertEmailExternal1(CompID, BrchID, UserID, docid, Doc_no, status, mail_id);
                        if (message.Contains(","))
                        {
                            var a = message.Split(',');
                            message = a[0];
                            mail_cont = a[1];
                        }
                        if (message == "success")
                        {
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            if (message == "invalidemail")
                            {
                                mail_cont = "Invalid email body configuration";
                            }
                            if (message == "invalid")
                            {
                                mail_cont = "Invalid sender email configuration";
                            }
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                    catch (Exception exMail)
                    {
                        message = "ErrorInMail";
                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
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
        //Added by nidhi on 29-08-2025
        private DataTable PrintOptionsDataTable(SPODetailModel model)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("ShowDeliverySchedule", typeof(string));
            dt.Columns.Add("ShowHsnNumber", typeof(string));
            dt.Columns.Add("ShowRemarksBlwItm", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));
            dt.Columns.Add("ShowSuppAliasName", typeof(string));

            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = model.PrintFormat;
            dtr["ShowProdDesc"] = model.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = model.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = model.ShowProdTechDesc;
            dtr["ShowSubItem"] = model.ShowSubItem;
            dtr["ShowDeliverySchedule"] = model.ShowDeliverySchedule;
            dtr["ShowHsnNumber"] = model.ShowHSNNumber;
            dtr["ShowRemarksBlwItm"] = model.ShowRemarksBlwItm;
            dtr["ShowTotalQty"] = model.ShowTotalQty;
            dtr["ShowSuppAliasName"] = model.ShowSupplierAliasName;
            dt.Rows.Add(dtr);
            return dt;
        }

    }
}
