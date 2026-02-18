using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ConsumableInvoice;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.Resources;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ConsumableInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Configuration;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.ConsumableInvoice
{
    public class ConsumableInvoiceController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105101152", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ConsumableInvoice_ISERVICE _ConsumableInvoice_ISERVICE;
        public ConsumableInvoiceController(Common_IServices _Common_IServices, ConsumableInvoice_ISERVICE _ConsumableInvoice_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._ConsumableInvoice_ISERVICE = _ConsumableInvoice_ISERVICE;
        }
        // GET: ApplicationLayer/ConsumableInvoice
        public ActionResult ConsumableInvoice(ConsumableInvoice_Model _ConsumableInvoice)
        {
            //ConsumableInvoice_Model _ConsumableInvoice = new ConsumableInvoice_Model();
            //string wfstatus = "";
            try
            {
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _ConsumableInvoice.WF_status = TempData["WF_status"].ToString();
                    if (_ConsumableInvoice.WF_status != null)
                    {
                        _ConsumableInvoice.wfstatus = _ConsumableInvoice.WF_status;
                    }
                    else
                    {
                        _ConsumableInvoice.wfstatus = "0";
                    }
                }
                else
                {
                    if (_ConsumableInvoice.WF_status != null)
                    {
                        _ConsumableInvoice.wfstatus = _ConsumableInvoice.WF_status;
                    }
                    else
                    {
                        _ConsumableInvoice.wfstatus = "0";
                    }
                }
                if (DocumentMenuId != null)
                {
                    _ConsumableInvoice.wfdocid = DocumentMenuId;
                }
                else
                {
                    _ConsumableInvoice.wfdocid = "0";
                }
                CommonPageDetails();
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _ConsumableInvoice.StatusList = statusLists;
                var SuppID = "";
                DateTime dtnow = DateTime.Now;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    //_ConsumableInvoice.SuppID = a[0].Trim();
                    SuppID = a[0].Trim();
                    _ConsumableInvoice.FromDate = a[1].Trim();
                    _ConsumableInvoice.ToDate = a[2].Trim();
                    _ConsumableInvoice.Status = a[3].Trim();
                    if (_ConsumableInvoice.Status == "0")
                    {
                        _ConsumableInvoice.Status = null;
                    }
                    _ConsumableInvoice.wfstatus = "";
                    _ConsumableInvoice.FilterData = TempData["ListFilterData"].ToString();
                    _ConsumableInvoice.CI_ToDate = _ConsumableInvoice.ToDate;
                    //_DPOListModel.ToDate =Convert.ToDateTime(_DPOListModel.PO_ToDate);
                }
                else
                {
                    //  string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                    var range = CommonController.Comman_GetFutureDateRange();
                    string startDate = range.FromDate;
                    string todate = range.ToDate;
                    _ConsumableInvoice.FromDate = startDate;
                    _ConsumableInvoice.ToDate = todate;
                }
                GetAllData(_ConsumableInvoice, SuppID);
                _ConsumableInvoice.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/Procurement/ConsumableInvoice/ConsumableInvoiceList.cshtml", _ConsumableInvoice);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private void GetAllData(ConsumableInvoice_Model _ConsumableInvoice,string SuppID)
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
                string GroupName = string.Empty;
                string type = string.Empty;
                if (string.IsNullOrEmpty(_ConsumableInvoice.SuppID))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _ConsumableInvoice.SuppID;
                }
                //DataTable dt = _ConsumableInvoice_ISERVICE.Getsuppplier(CompID, GroupName, type, BrchID);
                DataSet Data = _ConsumableInvoice_ISERVICE.GetAllData(CompID, GroupName, type, BrchID, _ConsumableInvoice.SuppID, _ConsumableInvoice.FromDate, _ConsumableInvoice.ToDate, _ConsumableInvoice.Status, UserID, _ConsumableInvoice.wfstatus, DocumentMenuId);


                List<Supplier> _SupplierList = new List<Supplier>();
                // DataTable dt = GetSupplierList(_ConsumableInvoice);
                foreach (DataRow dr in Data.Tables[0].Rows)
                {
                    Supplier _supplier = new Supplier();
                    _supplier.supp_id = dr["supp_id"].ToString();
                    _supplier.supp_name = dr["supp_name"].ToString();
                    _SupplierList.Add(_supplier);
                }
                _SupplierList.Insert(0, new Supplier() { supp_id = "0", supp_name = "All" });
                _ConsumableInvoice.SupplierNameList = _SupplierList;
                _ConsumableInvoice.SuppID = SuppID;
                // DataSet dt1 = _ConsumableInvoice_ISERVICE.GetCIDetailList(CompID, BrchID, _ConsumableInvoice.SuppID, _ConsumableInvoice.FromDate, _ConsumableInvoice.ToDate, _ConsumableInvoice.Status, UserID, _ConsumableInvoice.wfstatus, DocumentMenuId);
                ViewBag.ConsumableInvoiceList = Data.Tables[1];
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        [NonAction]
        private DataTable GetSupplierList(ConsumableInvoice_Model _ConsumableInvoice)
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
                string GroupName = string.Empty;
                string type = string.Empty;
                if (string.IsNullOrEmpty(_ConsumableInvoice.SuppID))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _ConsumableInvoice.SuppID;
                }
                DataTable dt = _ConsumableInvoice_ISERVICE.Getsuppplier(CompID, GroupName, type, BrchID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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
        public ActionResult AddConsumableInvoiceDetail()
        {
            ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails = new ConsumableInvoiceDetails_Model();
            _ConsumableInvoiceDetails.Message = "New";
            _ConsumableInvoiceDetails.Command = "Add";
            _ConsumableInvoiceDetails.AppStatus = "D";
            _ConsumableInvoiceDetails.TransType = "Save";
            _ConsumableInvoiceDetails.BtnName = "BtnAddNew";
            TempData["ModelData"] = _ConsumableInvoiceDetails;
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ConsumableInvoice");
            }
            /*End to chk Financial year exist or not*/
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("ConsumableInvoiceDetail", "ConsumableInvoice");
        }
        public ActionResult EditConsumableInvoice(string Inv_no, string Inv_dt, string ListFilterData, string WF_status)
        {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails = new ConsumableInvoiceDetails_Model();
            _ConsumableInvoiceDetails.Message = "New";
            _ConsumableInvoiceDetails.Command = "Add";
            _ConsumableInvoiceDetails.inv_no = Inv_no;
            _ConsumableInvoiceDetails.inv_dt = Inv_dt;
            _ConsumableInvoiceDetails.WF_status1 = WF_status;
            TempData["WF_status1"] = WF_status;
            TempData["ListFilterData"] = ListFilterData;
            _ConsumableInvoiceDetails.TransType = "Update";
            _ConsumableInvoiceDetails.AppStatus = "D";
            _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _ConsumableInvoiceDetails;
            var CI_NoURL = _ConsumableInvoiceDetails.inv_no;
            var CI_Date = _ConsumableInvoiceDetails.inv_dt;
            var TransType = _ConsumableInvoiceDetails.TransType;
            var BtnName = _ConsumableInvoiceDetails.BtnName;
            var Command = _ConsumableInvoiceDetails.Command;
            return( RedirectToAction("ConsumableInvoiceDetail", "ConsumableInvoice", new { CI_NoURL = CI_NoURL, CI_Date, TransType, BtnName, Command }));
        }
        public ActionResult GetConsumableInvoiceList(string docid, string status)
        {
            ConsumableInvoice_Model _ConsumableInvoice = new ConsumableInvoice_Model();
            _ConsumableInvoice.WF_status = status;
            //Session["WF_status"] = status;
            //Session["WF_Docid"] = docid;
            return RedirectToAction("ConsumableInvoice", "ConsumableInvoice", _ConsumableInvoice);
        }
        public ActionResult ConsumableInvoiceDetail(ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails1, string CI_NoURL,string CI_Date, string TransType, string BtnName, string Command)
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
            /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, CI_Date) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
                var _ConsumableInvoiceDetails = TempData["ModelData"] as ConsumableInvoiceDetails_Model;
                if (_ConsumableInvoiceDetails != null)
                {
                    ConsumableInvoice_Model _ConsumableInvoice = new ConsumableInvoice_Model();
                    CommonPageDetails();
                    List<Supplier> _SupplierList = new List<Supplier>();
                    DataTable dt = GetSupplierList(_ConsumableInvoice);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Supplier _supplier = new Supplier();
                        _supplier.supp_id = dr["supp_id"].ToString();
                        _supplier.supp_name = dr["supp_name"].ToString();
                        _SupplierList.Add(_supplier);
                    }
                    _SupplierList.Insert(0, new Supplier() { supp_id = "0", supp_name = "---Select---" });
                    _ConsumableInvoiceDetails.SupplierNameList = _SupplierList;

                    if (_ConsumableInvoiceDetails.BtnName == null)
                    {
                        _ConsumableInvoiceDetails.BtnName = "BtnAddNew";
                    }
                    if (_ConsumableInvoiceDetails.BtnName == null)
                    {
                        _ConsumableInvoiceDetails.Command = "Add";
                    }
                    _ConsumableInvoiceDetails.DocumentStatus = "";
                    _ConsumableInvoiceDetails.UserID = UserID;
                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _ConsumableInvoiceDetails.docNoLists = srcDocNoLists;

                    ViewBag.ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                    ViewBag.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                    ViewBag.RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ConsumableInvoiceDetails.FilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _ConsumableInvoiceDetails.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (_ConsumableInvoiceDetails.TransType == "Update" || _ConsumableInvoiceDetails.TransType == "Edit")
                    {
                        string VouType = "PV";
                        string inv_no = _ConsumableInvoiceDetails.inv_no;
                        string inv_dt = _ConsumableInvoiceDetails.inv_dt;
                        DataSet ds = _ConsumableInvoice_ISERVICE.Edit_CIDetail(CompID, BrchID, VouType, inv_no, inv_dt, UserID, DocumentMenuId);
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[8];
                        ViewBag.GLAccount = ds.Tables[7];
                        ViewBag.GLTOtal = ds.Tables[9];
                        ViewBag.ItemTaxDetails = ds.Tables[2];
                        ViewBag.OtherChargeDetails = ds.Tables[3];
                        ViewBag.TotalDetails = ds.Tables[0];
                        ViewBag.ItemTaxDetailsList = ds.Tables[10];
                        ViewBag.OCTaxDetails = ds.Tables[11];
                        ViewBag.CostCenterData = ds.Tables[12];
                        ViewBag.ItemTDSDetails = ds.Tables[13];
                        ViewBag.ItemOC_TDSDetails = ds.Tables[14];
                        _ConsumableInvoiceDetails.inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        _ConsumableInvoiceDetails.inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                        _ConsumableInvoiceDetails.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _ConsumableInvoiceDetails.bill_date = ds.Tables[0].Rows[0]["BillDate"].ToString();
                        _ConsumableInvoiceDetails.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _ConsumableInvoiceDetails.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _ConsumableInvoiceDetails.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        _ConsumableInvoiceDetails.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                        _ConsumableInvoiceDetails.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _ConsumableInvoiceDetails.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        _ConsumableInvoiceDetails.Createdon = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        _ConsumableInvoiceDetails.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        _ConsumableInvoiceDetails.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        _ConsumableInvoiceDetails.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        _ConsumableInvoiceDetails.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        _ConsumableInvoiceDetails.Status_name = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                        _ConsumableInvoiceDetails.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                        _ConsumableInvoiceDetails.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                        _ConsumableInvoiceDetails.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                        /***ShubhamMaurya on 13-10-2023 ***/
                        _ConsumableInvoiceDetails.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        //_ConsumableInvoiceDetails.Ship_StateCode = ds.Tables[0].Rows[0]["br_state_code"].ToString();
                        _ConsumableInvoiceDetails.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                        _ConsumableInvoiceDetails.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                        _ConsumableInvoiceDetails.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _ConsumableInvoiceDetails.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        _ConsumableInvoiceDetails.SrcDocDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _ConsumableInvoiceDetails.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();
                        _ConsumableInvoiceDetails.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                        _ConsumableInvoiceDetails.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                        _ConsumableInvoiceDetails.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        var state_code = ds.Tables[0].Rows[0]["state_code"];
                        var br_state_code = ds.Tables[0].Rows[0]["br_state_code"];
                        //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                        if (state_code.ToString() == br_state_code.ToString())
                        {
                            _ConsumableInvoiceDetails.Hd_GstType = "Both";
                        }
                        else
                        {
                            _ConsumableInvoiceDetails.Hd_GstType = "IGST";
                        }

                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        _ConsumableInvoiceDetails.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                        if (roundoff_status == "Y")
                        {
                            _ConsumableInvoiceDetails.RoundOffFlag = true;
                        }
                        else
                        {
                            _ConsumableInvoiceDetails.RoundOffFlag = false;
                        }
                        string RCMApplicable = ds.Tables[0].Rows[0]["rcm_app"].ToString().Trim();
                        if (RCMApplicable == "Y")
                        {
                            _ConsumableInvoiceDetails.RCMApplicable = true;
                        }
                        else
                        {
                            _ConsumableInvoiceDetails.RCMApplicable = false;
                        }
                        List<SrcDocNoList> _srcDocNoLists = new List<SrcDocNoList>();
                        srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _ConsumableInvoiceDetails.SrcDocNo, SrcDocnoVal = _ConsumableInvoiceDetails.SrcDocNo });
                        _ConsumableInvoiceDetails.docNoLists = srcDocNoLists;

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["inv_status1"].ToString().Trim();
                        _ConsumableInvoiceDetails.doc_status = doc_status;
                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            if (doc_status == "A" || doc_status == "C")
                            {
                                _ConsumableInvoiceDetails.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                            }
                            _ConsumableInvoiceDetails.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                            _ConsumableInvoiceDetails.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                            ViewBag.GLVoucherNo = _ConsumableInvoiceDetails.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
                        }
                        _ConsumableInvoiceDetails.DocumentStatus = doc_status;
                        if (doc_status == "C")
                        {
                            _ConsumableInvoiceDetails.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _ConsumableInvoiceDetails.CancelFlag = true;
                            _ConsumableInvoiceDetails.BtnName = "Refresh";
                        }
                        else
                        {
                            _ConsumableInvoiceDetails.CancelFlag = false;
                        }
                        _ConsumableInvoiceDetails.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _ConsumableInvoiceDetails.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        if (ViewBag.AppLevel != null && _ConsumableInvoiceDetails.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[4].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[5].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _ConsumableInvoiceDetails.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _ConsumableInvoiceDetails.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _ConsumableInvoiceDetails.Title = title;
                        _ConsumableInvoiceDetails.DeleteCommand = null;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        ViewBag.MenuPageName = getDocumentName();
                        return View("~/Areas/ApplicationLayer/Views/Procurement/ConsumableInvoice/ConsumableInvoiceDetail.cshtml", _ConsumableInvoiceDetails);
                    }
                    else
                    {
                        _ConsumableInvoiceDetails.Title = title;
                        _ConsumableInvoiceDetails.DeleteCommand = null;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        ViewBag.MenuPageName = getDocumentName();
                        return View("~/Areas/ApplicationLayer/Views/Procurement/ConsumableInvoice/ConsumableInvoiceDetail.cshtml", _ConsumableInvoiceDetails);
                    }
                }
                else
                {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    ConsumableInvoice_Model _ConsumableInvoice = new ConsumableInvoice_Model();
                    CommonPageDetails();
                    List<Supplier> _SupplierList = new List<Supplier>();
                    DataTable dt = GetSupplierList(_ConsumableInvoice);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Supplier _supplier = new Supplier();
                        _supplier.supp_id = dr["supp_id"].ToString();
                        _supplier.supp_name = dr["supp_name"].ToString();
                        _SupplierList.Add(_supplier);
                    }
                    _SupplierList.Insert(0, new Supplier() { supp_id = "0", supp_name = "---Select---" });
                    _ConsumableInvoiceDetails1.SupplierNameList = _SupplierList;
                    _ConsumableInvoiceDetails1.inv_no = CI_NoURL;
                    _ConsumableInvoiceDetails1.inv_dt = CI_Date;
                    _ConsumableInvoiceDetails1.TransType = TransType;
                    _ConsumableInvoiceDetails1.BtnName = BtnName;
                    _ConsumableInvoiceDetails1.Command = Command;
                    if (_ConsumableInvoiceDetails1.BtnName == null)
                    {
                        _ConsumableInvoiceDetails1.BtnName = "BtnAddNew";
                    }
                    if (_ConsumableInvoiceDetails1.BtnName == null)
                    {
                        _ConsumableInvoiceDetails1.Command = "Edit";
                    }
                    if (_ConsumableInvoiceDetails1.TransType == null)
                    {
                        _ConsumableInvoiceDetails1.TransType = "Save";
                    }
                    _ConsumableInvoiceDetails1.DocumentStatus = "";

                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _ConsumableInvoiceDetails1.docNoLists = srcDocNoLists;

                    ViewBag.ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                    ViewBag.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                    ViewBag.RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ConsumableInvoiceDetails1.FilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _ConsumableInvoiceDetails1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (_ConsumableInvoiceDetails1.TransType == "Update" || _ConsumableInvoiceDetails1.TransType == "Edit")
                    {
                        string VouType = "PV";
                        string inv_no = _ConsumableInvoiceDetails1.inv_no;
                        string inv_dt = _ConsumableInvoiceDetails1.inv_dt;
                        DataSet ds = _ConsumableInvoice_ISERVICE.Edit_CIDetail(CompID, BrchID, VouType, inv_no, inv_dt, UserID, DocumentMenuId);
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[8];
                        ViewBag.GLAccount = ds.Tables[7];
                        ViewBag.GLTOtal = ds.Tables[9];
                        ViewBag.ItemTaxDetails = ds.Tables[2];
                        ViewBag.OtherChargeDetails = ds.Tables[3];
                        ViewBag.TotalDetails = ds.Tables[0];
                        ViewBag.ItemTaxDetailsList = ds.Tables[10];
                        ViewBag.OCTaxDetails = ds.Tables[11];
                        ViewBag.CostCenterData = ds.Tables[12];
                        ViewBag.ItemTDSDetails = ds.Tables[13];
                        _ConsumableInvoiceDetails1.inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        _ConsumableInvoiceDetails1.inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                        _ConsumableInvoiceDetails1.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _ConsumableInvoiceDetails1.bill_date = ds.Tables[0].Rows[0]["BillDate"].ToString();
                        _ConsumableInvoiceDetails1.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _ConsumableInvoiceDetails1.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _ConsumableInvoiceDetails1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        _ConsumableInvoiceDetails1.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                        _ConsumableInvoiceDetails1.Createdon = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _ConsumableInvoiceDetails1.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        _ConsumableInvoiceDetails1.Createdon = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        _ConsumableInvoiceDetails1.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        _ConsumableInvoiceDetails1.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        _ConsumableInvoiceDetails1.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        _ConsumableInvoiceDetails1.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        _ConsumableInvoiceDetails1.Status_name = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                        _ConsumableInvoiceDetails1.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                        _ConsumableInvoiceDetails1.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                        _ConsumableInvoiceDetails1.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                        /***ShubhamMaurya on 13-10-2023 ***/
                        //_ConsumableInvoiceDetails1.Ship_StateCode = ds.Tables[0].Rows[0]["br_state_code"].ToString();
                        _ConsumableInvoiceDetails1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        _ConsumableInvoiceDetails1.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                        _ConsumableInvoiceDetails1.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails1.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails1.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails1.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails1.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails1.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ViewBag.ValDigit);
                        _ConsumableInvoiceDetails1.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();

                        _ConsumableInvoiceDetails1.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _ConsumableInvoiceDetails1.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        _ConsumableInvoiceDetails1.SrcDocDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _ConsumableInvoiceDetails1.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();/*Added By Nitesh 02-12-2023*/
                        _ConsumableInvoiceDetails1.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();/*Added By Nitesh 02-12-2023*/
                        _ConsumableInvoiceDetails1.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                        _ConsumableInvoiceDetails1.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                        var state_code = ds.Tables[0].Rows[0]["state_code"];
                        var br_state_code = ds.Tables[0].Rows[0]["br_state_code"];
                        //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                        if (state_code.ToString() == br_state_code.ToString())
                        {
                            _ConsumableInvoiceDetails1.Hd_GstType = "Both";
                        }
                        else
                        {
                            _ConsumableInvoiceDetails1.Hd_GstType = "IGST";
                        }
                        List<SrcDocNoList> _srcDocNoLists = new List<SrcDocNoList>();
                        srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _ConsumableInvoiceDetails1.SrcDocNo, SrcDocnoVal = _ConsumableInvoiceDetails1.SrcDocNo });
                        _ConsumableInvoiceDetails1.docNoLists = srcDocNoLists;

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["inv_status1"].ToString().Trim();
                        _ConsumableInvoiceDetails1.doc_status = doc_status;
                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            if (doc_status == "A" || doc_status == "C")
                            {
                                _ConsumableInvoiceDetails1.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                            }
                            _ConsumableInvoiceDetails1.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                            _ConsumableInvoiceDetails1.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                            ViewBag.GLVoucherNo = _ConsumableInvoiceDetails1.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
                        }
                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        _ConsumableInvoiceDetails1.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                        if (roundoff_status == "Y")
                        {
                            _ConsumableInvoiceDetails1.RoundOffFlag = true;
                        }
                        else
                        {
                            _ConsumableInvoiceDetails1.RoundOffFlag = false;
                        }

                        _ConsumableInvoiceDetails1.DocumentStatus = doc_status;
                        if (doc_status == "C")
                        {
                            _ConsumableInvoiceDetails1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _ConsumableInvoiceDetails1.CancelFlag = true;
                            _ConsumableInvoiceDetails1.BtnName = "Refresh";
                        }
                        else
                        {
                            _ConsumableInvoiceDetails1.CancelFlag = false;
                        }
                        string RCMApplicable = ds.Tables[0].Rows[0]["rcm_app"].ToString().Trim();
                        if (RCMApplicable == "Y")
                        {
                            _ConsumableInvoiceDetails1.RCMApplicable = true;
                        }
                        else
                        {
                            _ConsumableInvoiceDetails1.RCMApplicable = false;
                        }
                        _ConsumableInvoiceDetails1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _ConsumableInvoiceDetails1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        if (ViewBag.AppLevel != null && _ConsumableInvoiceDetails1.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[4].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[5].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _ConsumableInvoiceDetails1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _ConsumableInvoiceDetails1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _ConsumableInvoiceDetails1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _ConsumableInvoiceDetails1.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _ConsumableInvoiceDetails1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _ConsumableInvoiceDetails1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _ConsumableInvoiceDetails1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _ConsumableInvoiceDetails1.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _ConsumableInvoiceDetails1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                        _ConsumableInvoiceDetails1.Title = title;
                        _ConsumableInvoiceDetails1.DeleteCommand = null;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        ViewBag.MenuPageName = getDocumentName();
                        return View("~/Areas/ApplicationLayer/Views/Procurement/ConsumableInvoice/ConsumableInvoiceDetail.cshtml", _ConsumableInvoiceDetails1);
                    }
                    else
                    {
                        _ConsumableInvoiceDetails1.Title = title;
                        _ConsumableInvoiceDetails1.DeleteCommand = null;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        ViewBag.MenuPageName = getDocumentName();
                        return View("~/Areas/ApplicationLayer/Views/Procurement/ConsumableInvoice/ConsumableInvoiceDetail.cshtml", _ConsumableInvoiceDetails1);
                    }
                }            
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
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
        public ActionResult ConsumableInvoiceCommands(ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails, string Command)
        {
            try
            {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ConsumableInvoiceDetails.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "AddNew":
                        ConsumableInvoiceDetails_Model _ConsumableInvoiceDetailsAddNew = new ConsumableInvoiceDetails_Model();
                        _ConsumableInvoiceDetailsAddNew.Message = "New";
                        _ConsumableInvoiceDetailsAddNew.Command = "Add";
                        _ConsumableInvoiceDetailsAddNew.AppStatus = "D";
                        _ConsumableInvoiceDetailsAddNew.TransType = "Save";
                        _ConsumableInvoiceDetailsAddNew.BtnName = "BtnAddNew";
                        _ConsumableInvoiceDetails.DeleteCommand = null;
                        TempData["ListFilterData"] = null;
                        TempData["ModelData"] = _ConsumableInvoiceDetailsAddNew;
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                             if (!string.IsNullOrEmpty(_ConsumableInvoiceDetails.inv_no))
                                return RedirectToAction("EditConsumableInvoice", new { Inv_no = _ConsumableInvoiceDetails.inv_no, Inv_dt = _ConsumableInvoiceDetails.inv_dt, ListFilterData = _ConsumableInvoiceDetails.FilterData1, WF_status = _ConsumableInvoiceDetails.WFStatus });
                            else
                                _ConsumableInvoiceDetailsAddNew.Command = "Refresh";
                            _ConsumableInvoiceDetailsAddNew.TransType = "Refresh";
                            _ConsumableInvoiceDetailsAddNew.BtnName = "Refresh";
                            _ConsumableInvoiceDetailsAddNew.DocumentStatus = null;
                            TempData["ModelData"] = _ConsumableInvoiceDetailsAddNew;
                            return RedirectToAction("ConsumableInvoiceDetail", _ConsumableInvoiceDetailsAddNew);

                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ConsumableInvoiceDetail");
                    case "Edit":
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditConsumableInvoice", new { Inv_no = _ConsumableInvoiceDetails.inv_no, Inv_dt = _ConsumableInvoiceDetails.inv_dt, ListFilterData = _ConsumableInvoiceDetails.FilterData1, WF_status = _ConsumableInvoiceDetails.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string CInvDt = _ConsumableInvoiceDetails.inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, CInvDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditConsumableInvoice", new { Inv_no = _ConsumableInvoiceDetails.inv_no, Inv_dt = _ConsumableInvoiceDetails.inv_dt, ListFilterData = _ConsumableInvoiceDetails.FilterData1, WF_status = _ConsumableInvoiceDetails.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        var CI_NoURL = "";
                        var CI_Date = "";
                        var TransType = "";
                        var BtnName = "";
                        if (_ConsumableInvoiceDetails.doc_status == "A")
                        {
                            string checkforCancle = CheckPIForCancellationinVoucher(_ConsumableInvoiceDetails.inv_no, _ConsumableInvoiceDetails.inv_dt);
                            if (checkforCancle != "")
                            {
                                //Session["Message"] = checkforCancle;
                                _ConsumableInvoiceDetails.Message = checkforCancle;
                                _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
                                _ConsumableInvoiceDetails.Command = "Refresh";
                                _ConsumableInvoiceDetails.TransType = "Update";
                             CI_NoURL = _ConsumableInvoiceDetails.inv_no;
                                 CI_Date = _ConsumableInvoiceDetails.inv_dt;
                                TransType = _ConsumableInvoiceDetails.TransType;
                                BtnName = _ConsumableInvoiceDetails.BtnName;
                                TempData["ModelData"] = _ConsumableInvoiceDetails;

                                TempData["FilterData"] = _ConsumableInvoiceDetails.FilterData1;
                            }
                            else
                            {
                                _ConsumableInvoiceDetails.TransType = "Update";
                                _ConsumableInvoiceDetails.Command = Command;
                                _ConsumableInvoiceDetails.BtnName = "BtnEdit";
                                TempData["ModelData"] = _ConsumableInvoiceDetails;
                                CI_NoURL = _ConsumableInvoiceDetails.inv_no;
                               CI_Date = _ConsumableInvoiceDetails.inv_dt;
                                TransType = _ConsumableInvoiceDetails.TransType;
                                 BtnName = _ConsumableInvoiceDetails.BtnName;


                                TempData["ListFilterData"] = _ConsumableInvoiceDetails.FilterData1;
                            }
                        }
                        else
                        {


                            _ConsumableInvoiceDetails.Message = "";
                            _ConsumableInvoiceDetails.Command = Command;
                            _ConsumableInvoiceDetails.TransType = "Update";
                            _ConsumableInvoiceDetails.BtnName = "BtnEdit";
                            TempData["ModelData"] = _ConsumableInvoiceDetails;
                          CI_NoURL = _ConsumableInvoiceDetails.inv_no;
                           CI_Date = _ConsumableInvoiceDetails.inv_dt;
                           TransType = _ConsumableInvoiceDetails.TransType;
                           BtnName = _ConsumableInvoiceDetails.BtnName;
                            TempData["ListFilterData"] = _ConsumableInvoiceDetails.FilterData1;
                        }
                        return RedirectToAction("ConsumableInvoiceDetail", new { CI_NoURL = CI_NoURL,CI_Date, TransType, BtnName, Command });
                    case "Save":
                        _ConsumableInvoiceDetails.Command = Command;
                        if (_ConsumableInvoiceDetails.TransType == null)
                        {
                            _ConsumableInvoiceDetails.TransType= Command;
                        }
                        //_ConsumableInvoiceDetails.TransType = Command;
                        string checkforCancle_onSave = CheckPIForCancellationinVoucher(_ConsumableInvoiceDetails.inv_no, _ConsumableInvoiceDetails.inv_dt);
                        if (checkforCancle_onSave != "")//Added by Suraj on 21-09-2024 to check before cancel
                        {
                            _ConsumableInvoiceDetails.Message = checkforCancle_onSave;
                            _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
                            _ConsumableInvoiceDetails.Command = "Refresh";
                            _ConsumableInvoiceDetails.TransType = "Update";
                            CI_NoURL = _ConsumableInvoiceDetails.inv_no;
                            CI_Date = _ConsumableInvoiceDetails.inv_dt;
                            TransType = _ConsumableInvoiceDetails.TransType;
                            BtnName = _ConsumableInvoiceDetails.BtnName;
                            TempData["ModelData"] = _ConsumableInvoiceDetails;

                            TempData["FilterData"] = _ConsumableInvoiceDetails.FilterData1;
                        }
                        else
                        {
                            SaveDNSCDetail(_ConsumableInvoiceDetails);
                        }
                        
                        if (_ConsumableInvoiceDetails.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_ConsumableInvoiceDetails.Message == "Duplicate")
                        {
                            TempData["ModelData"] = _ConsumableInvoiceDetails;
                            return RedirectToAction("ProspectDetailInDuplicateCase");
                        }
                        CI_NoURL = _ConsumableInvoiceDetails.inv_no;
                        CI_Date = _ConsumableInvoiceDetails.inv_dt;
                        TransType = _ConsumableInvoiceDetails.TransType;
                        BtnName = _ConsumableInvoiceDetails.BtnName;
                        TempData["ModelData"] = _ConsumableInvoiceDetails;
                        return RedirectToAction("ConsumableInvoiceDetail", new { CI_NoURL = CI_NoURL, CI_Date, TransType, BtnName, Command });
                    case "Delete":
                        ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails_ModelDelete = new ConsumableInvoiceDetails_Model();
                        _ConsumableInvoiceDetails.Command = Command;
                        _ConsumableInvoiceDetails.BtnName = "Refresh";
                        DeleteCI_Details(_ConsumableInvoiceDetails, Command);
                        _ConsumableInvoiceDetails_ModelDelete.Message = "Deleted";
                        _ConsumableInvoiceDetails_ModelDelete.Command = "Refresh";
                        _ConsumableInvoiceDetails_ModelDelete.TransType = "Refresh";
                        _ConsumableInvoiceDetails_ModelDelete.AppStatus = "D";
                        _ConsumableInvoiceDetails_ModelDelete.BtnName = "BtnDelete";
                        TempData["ModelData"] = _ConsumableInvoiceDetails_ModelDelete;
                        TempData["ListFilterData"] = _ConsumableInvoiceDetails.FilterData1;
                        return RedirectToAction("ConsumableInvoiceDetail");
                    case "Forward":
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditConsumableInvoice", new { Inv_no = _ConsumableInvoiceDetails.inv_no, Inv_dt = _ConsumableInvoiceDetails.inv_dt, ListFilterData = _ConsumableInvoiceDetails.FilterData1, WF_status = _ConsumableInvoiceDetails.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string CInvDt1 = _ConsumableInvoiceDetails.inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, CInvDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditConsumableInvoice", new { Inv_no = _ConsumableInvoiceDetails.inv_no, Inv_dt = _ConsumableInvoiceDetails.inv_dt, ListFilterData = _ConsumableInvoiceDetails.FilterData1, WF_status = _ConsumableInvoiceDetails.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditConsumableInvoice", new { Inv_no = _ConsumableInvoiceDetails.inv_no, Inv_dt = _ConsumableInvoiceDetails.inv_dt, ListFilterData = _ConsumableInvoiceDetails.FilterData1, WF_status = _ConsumableInvoiceDetails.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string CInvDt2 = _ConsumableInvoiceDetails.inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, CInvDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditConsumableInvoice", new { Inv_no = _ConsumableInvoiceDetails.inv_no, Inv_dt = _ConsumableInvoiceDetails.inv_dt, ListFilterData = _ConsumableInvoiceDetails.FilterData1, WF_status = _ConsumableInvoiceDetails.WFStatus });
                        }
                        _ConsumableInvoiceDetails.Command = Command;
                        var inv_no = _ConsumableInvoiceDetails.inv_no;
                        var inv_dt = _ConsumableInvoiceDetails.inv_dt;
                        Approve_ConsumableInvoice(inv_no, inv_dt, "", "", "", _ConsumableInvoiceDetails.Nurration, ""
                            , _ConsumableInvoiceDetails.curr_id, _ConsumableInvoiceDetails.conv_rate, "", _ConsumableInvoiceDetails.BP_Nurration, _ConsumableInvoiceDetails.DN_Nurration);
                        _ConsumableInvoiceDetails.Message = "Approved";
                        _ConsumableInvoiceDetails.TransType = "Update";
                        _ConsumableInvoiceDetails.Command = "Approve";
                        _ConsumableInvoiceDetails.inv_no = inv_no;
                        _ConsumableInvoiceDetails.inv_dt = inv_dt;
                        _ConsumableInvoiceDetails.AppStatus = "D";
                        _ConsumableInvoiceDetails.BtnName = "BtnEdit";
                        TempData["ModelData"] = _ConsumableInvoiceDetails;
                        CI_NoURL = _ConsumableInvoiceDetails.inv_no;
                        CI_Date = _ConsumableInvoiceDetails.inv_dt;
                        TransType = _ConsumableInvoiceDetails.TransType;
                        BtnName = _ConsumableInvoiceDetails.BtnName;
                        return RedirectToAction("ConsumableInvoiceDetail", new { CI_NoURL = CI_NoURL, CI_Date, TransType, BtnName, Command });
                    case "Refresh":
                        ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails_ModelRefresh = new ConsumableInvoiceDetails_Model();
                        //_DeliveryNoteDetailSC_Model = new DeliveryNoteDetailSC_Model();
                        _ConsumableInvoiceDetails_ModelRefresh.Message = null;
                        //_DeliveryNoteDetailSC_Model.Command = command;
                        //_DeliveryNoteDetailSC_Model.TransType = "Refresh";
                        //_DeliveryNoteDetailSC_Model.BtnName = "Refresh";
                        //_DeliveryNoteDetailSC_Model.DocumentStatus = null;
                        _ConsumableInvoiceDetails_ModelRefresh.Command = Command;
                        _ConsumableInvoiceDetails_ModelRefresh.TransType = "Refresh";
                        _ConsumableInvoiceDetails_ModelRefresh.BtnName = "Refresh";
                        _ConsumableInvoiceDetails_ModelRefresh.DocumentStatus = null;
                        TempData["ListFilterData"] = _ConsumableInvoiceDetails.FilterData1;
                        TempData["ModelData"] = _ConsumableInvoiceDetails_ModelRefresh;
                        return RedirectToAction("ConsumableInvoiceDetail");
                    case "Print":
                        return GenratePdfFile(DocumentMenuId, _ConsumableInvoiceDetails.inv_no, _ConsumableInvoiceDetails.inv_dt);
                    case "BacktoList":
                        //TempData["ListFilterData"] = _ConsumableInvoiceDetails.ListFilterData1;
                        TempData["WF_status"] = _ConsumableInvoiceDetails.WF_status1;
                        TempData["ListFilterData"] = _ConsumableInvoiceDetails.FilterData1;
                        return RedirectToAction("ConsumableInvoice", "ConsumableInvoice");
                    default:
                        return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public string CheckPIForCancellationinVoucher(string DocNo, string DocDate)
        {
            string Result = "";
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
                DataSet Deatils = _ConsumableInvoice_ISERVICE.CheckCIDetail(Comp_ID, Br_ID, DocNo, DocDate);

                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = "PaymentsHasBeenAdjustedAgainstThisInvoiceItCanNotBeModified";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            return Result;
        }
        public ActionResult SaveDNSCDetail(ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails)
        {
            string SaveMessage = "";
            CommonPageDetails();
            string PageName = title.Replace(" ", "");
            try
            {
                if (_ConsumableInvoiceDetails.Cancelled == false)
                {
                    string FSOId = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BrchID = Session["BranchId"].ToString();
                    }
                    if (Session["Userid"] != null)
                    {
                        UserID = Session["Userid"].ToString();
                    }
                    DataTable DtblHDetail = new DataTable();
                    DataTable DtblItemDetail = new DataTable();
                    DataTable DtblTaxDetail = new DataTable();
                    DataTable DtblOCDetail = new DataTable();
                    DataTable DtblAttchDetail = new DataTable();
                    DataTable DtblOCTaxDetail = new DataTable();
                    DataTable CRCostCenterDetails = new DataTable();
                    DataTable DtblTdsDetail = new DataTable();
                    DataTable DtblOcTdsDetail = new DataTable();

                    DtblHDetail = ToDtblHDetail(_ConsumableInvoiceDetails);
                    DtblItemDetail = ToDtblItemDetail(_ConsumableInvoiceDetails.Itemdetails);
                    DtblTaxDetail = ToDtblTaxDetail(_ConsumableInvoiceDetails.ItemTaxdetails,"Tax");
                    DtblOCTaxDetail = ToDtblTaxDetail(_ConsumableInvoiceDetails.OC_TaxDetail,"OcTax");
                    DtblOCDetail = ToDtblOCDetail(_ConsumableInvoiceDetails.ItemOCdetails);
                    DtblTdsDetail = ToDtblTdsDetail(_ConsumableInvoiceDetails.tds_details, "");
                    DtblOcTdsDetail = ToDtblTdsDetail(_ConsumableInvoiceDetails.oc_tds_details, "OC");

                    //DtblAttchDetail = BindAttachData(DtblHDetail, _ConsumableInvoiceDetails.attatchmentdetail);
                    var _ConsumableInvoiceattch = TempData["ModelDataattch"] as ConsumableInvoiceattch;
                    TempData["ModelDataattch"] = null;

                    /**----------------Cost Center Section--------------------*/
                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("vou_sr_no", typeof(string));
                    CC_Details.Columns.Add("gl_sr_no", typeof(string));
                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));

                    JArray JAObj = JArray.Parse(_ConsumableInvoiceDetails.CC_DetailList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = CC_Details.NewRow();

                        dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                        dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();
                        dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                        dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                        dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                        dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();

                        CC_Details.Rows.Add(dtrowLines);
                    }
                    CRCostCenterDetails = CC_Details;

                    DataTable dtAttachment = new DataTable();
                    if (_ConsumableInvoiceDetails.attatchmentdetail != null)
                    {
                        if (_ConsumableInvoiceattch != null)
                        {
                            if (_ConsumableInvoiceattch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _ConsumableInvoiceattch.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                dtAttachment.Columns.Add("id", typeof(string));
                                dtAttachment.Columns.Add("file_name", typeof(string));
                                dtAttachment.Columns.Add("file_path", typeof(string));
                                dtAttachment.Columns.Add("file_def", typeof(char));
                                dtAttachment.Columns.Add("comp_id", typeof(Int32));
                            }
                        }
                        else
                        {
                            if (_ConsumableInvoiceDetails.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _ConsumableInvoiceDetails.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                dtAttachment.Columns.Add("id", typeof(string));
                                dtAttachment.Columns.Add("file_name", typeof(string));
                                dtAttachment.Columns.Add("file_path", typeof(string));
                                dtAttachment.Columns.Add("file_def", typeof(char));
                                dtAttachment.Columns.Add("comp_id", typeof(Int32));
                            }
                        }
                        JArray jObject1 = JArray.Parse(_ConsumableInvoiceDetails.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in dtAttachment.Rows)
                            {
                                string drImg = dr["file_name"].ToString();
                                string ObjImg = jObject1[i]["file_name"].ToString();
                                if (drImg == ObjImg)
                                {
                                    flag = "N";
                                }
                            }
                            if (flag == "Y")
                            {

                                DataRow dtrowAttachment1 = dtAttachment.NewRow();
                                if (!string.IsNullOrEmpty(_ConsumableInvoiceDetails.inv_no))
                                {
                                    dtrowAttachment1["id"] = _ConsumableInvoiceDetails.inv_no.Replace("/", "");
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                dtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        if (_ConsumableInvoiceDetails.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_ConsumableInvoiceDetails.inv_no))
                                {
                                    ItmCode = _ConsumableInvoiceDetails.inv_no;
                                }
                                else
                                {
                                    ItmCode = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + ItmCode.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in dtAttachment.Rows)
                                    {
                                        string drImgPath = dr["file_path"].ToString();
                                        if (drImgPath == fielpath.Replace("/",@"\"))
                                        {
                                            flag = "N";
                                        }
                                    }
                                    if (flag == "Y")
                                    {
                                        System.IO.File.Delete(fielpath);
                                    }
                                }
                            }
                        }
                        DtblAttchDetail = dtAttachment;
                    }

                    DataTable DtblVouDetail = new DataTable();
                    //DataTable vou_Details = new DataTable();
                    //vou_Details.Columns.Add("comp_id", typeof(string));
                    //vou_Details.Columns.Add("id", typeof(string));
                    //vou_Details.Columns.Add("type", typeof(string));
                    //vou_Details.Columns.Add("doctype", typeof(string));
                    //vou_Details.Columns.Add("Value", typeof(string));
                    //vou_Details.Columns.Add("DrAmt", typeof(string));
                    //vou_Details.Columns.Add("CrAmt", typeof(string));
                    //vou_Details.Columns.Add("TransType", typeof(string));
                    //vou_Details.Columns.Add("Gltype", typeof(string));
                    //if (_ConsumableInvoiceDetails.vouDetail != null)
                    //{

                    //    JArray jObjectVOU = JArray.Parse(_ConsumableInvoiceDetails.vouDetail);
                    //    for (int i = 0; i < jObjectVOU.Count; i++)
                    //    {
                    //        DataRow dtrowVouDetailsLines = vou_Details.NewRow();
                    //        dtrowVouDetailsLines["comp_id"] = jObjectVOU[i]["comp_id"].ToString();
                    //        dtrowVouDetailsLines["id"] = jObjectVOU[i]["id"].ToString();
                    //        dtrowVouDetailsLines["type"] = jObjectVOU[i]["type"].ToString();
                    //        dtrowVouDetailsLines["doctype"] = jObjectVOU[i]["doctype"].ToString();
                    //        dtrowVouDetailsLines["Value"] = jObjectVOU[i]["Value"].ToString();
                    //        dtrowVouDetailsLines["DrAmt"] = jObjectVOU[i]["DrAmt"].ToString();
                    //        dtrowVouDetailsLines["CrAmt"] = jObjectVOU[i]["CrAmt"].ToString();
                    //        dtrowVouDetailsLines["TransType"] = jObjectVOU[i]["TransType"].ToString();
                    //        dtrowVouDetailsLines["Gltype"] = jObjectVOU[i]["Gltype"].ToString();
                    //        vou_Details.Rows.Add(dtrowVouDetailsLines);
                    //    }
                    //}

                    DtblVouDetail = ToDtblVouDetail(_ConsumableInvoiceDetails.vouDetail);
                    string Nurr = "";
                    if (_ConsumableInvoiceDetails.CancelFlag)
                    {
                        Nurr = _ConsumableInvoiceDetails.Nurration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                    }
                    string tds_amt = _ConsumableInvoiceDetails.TDS_Amount == null ? "0" : _ConsumableInvoiceDetails.TDS_Amount;
                    SaveMessage = _ConsumableInvoice_ISERVICE.InsertCI_Details(DtblHDetail, DtblItemDetail, DtblTaxDetail, DtblOCDetail, DtblAttchDetail, DtblVouDetail, DtblOCTaxDetail, CRCostCenterDetails, Nurr, DtblTdsDetail, tds_amt, DtblOcTdsDetail);
                    string[] FDetail = SaveMessage.Split(',');
                    string Message = FDetail[5].ToString();
                    string Inv_no = FDetail[0].ToString();
                    string Inv_DATE = FDetail[6].ToString();
                    string Cansal = FDetail[1].ToString();
                    if (Message == "DataNotFound")
                    {
                        var msg = "Data Not found" + " " + Inv_DATE+" in "+PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _ConsumableInvoiceDetails.Message = Message;
                        return RedirectToAction("ConsumableInvoiceDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_ConsumableInvoiceattch != null)
                        {
                            //if (Session["Guid"] != null)
                            if (_ConsumableInvoiceattch.Guid != null)
                            {
                                //Guid = Session["Guid"].ToString();
                                Guid = _ConsumableInvoiceattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, _ConsumableInvoiceDetails.TransType, DtblAttchDetail);

                        //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                        //if (Directory.Exists(sourcePath))
                        //{
                        //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrchID + Guid + "_" + "*");
                        //    foreach (string file in filePaths)
                        //    {
                        //        string[] items = file.Split('\\');
                        //        string ItemName = items[items.Length - 1];
                        //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                        //        foreach (DataRow dr in DtblAttchDetail.Rows)
                        //        {
                        //            string DrItmNm = dr["file_name"].ToString();
                        //            if (ItemName == DrItmNm)
                        //            {
                        //                string Inv_no1 = Inv_no.Replace("/", "");
                        //                string img_nm = CompID + BrchID + Inv_no1 + "_" + Path.GetFileName(DrItmNm).ToString();
                        //                string doc_path = Path.Combine(Server.MapPath("~/Attachment/" + PageName + "/"), img_nm);
                        //                string DocumentPath = Server.MapPath("~/Attachment/" + PageName + "/");
                        //                if (!Directory.Exists(DocumentPath))
                        //                {
                        //                    DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
                        //                }

                        //                System.IO.File.Move(file, doc_path);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    if (Cansal == "C" && Message == "Update")
                    {
                        try
                        {
                            //string fileName = "CI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "ConsumbleInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert( _ConsumableInvoiceDetails.inv_no, _ConsumableInvoiceDetails.inv_dt, fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _ConsumableInvoiceDetails.inv_no, "Cancel", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _ConsumableInvoiceDetails.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        //_ConsumableInvoiceDetails.Message = "Cancelled";
                        _ConsumableInvoiceDetails.Message = _ConsumableInvoiceDetails.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        _ConsumableInvoiceDetails.Command = "Update";
                        _ConsumableInvoiceDetails.inv_no = Inv_no;
                        _ConsumableInvoiceDetails.inv_dt = Inv_DATE;
                        _ConsumableInvoiceDetails.TransType = "Update";
                        _ConsumableInvoiceDetails.AppStatus = "D";
                        _ConsumableInvoiceDetails.BtnName = "Refresh";
                        //Session["AttachMentDetailItmStp"] = null;
                        //Session["Guid"] = null;
                    }
                    else
                    {
                        if (Message == "Update" || Message == "Save")
                        {
                            _ConsumableInvoiceDetails.Message = "Save";
                            _ConsumableInvoiceDetails.inv_no = Inv_no;
                            _ConsumableInvoiceDetails.inv_dt = Inv_DATE;
                            _ConsumableInvoiceDetails.TransType = "Update";
                            _ConsumableInvoiceDetails.Command = "Update";
                            _ConsumableInvoiceDetails.AppStatus = "D";
                            _ConsumableInvoiceDetails.DocumentStatus = "D";

                            _ConsumableInvoiceDetails.BtnName = "BtnSave";
                            //TempData["ModelData"] = _ConsumableInvoiceDetails;
                        }
                    }
                    TempData["ModelData"] = _ConsumableInvoiceDetails;
                    return RedirectToAction("ConsumableInvoiceDetail");
                }
                else
                {

                }
                return RedirectToAction("DeliveryNoteSCDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_ConsumableInvoiceDetails.TransType == "Save")
                    {
                        string Guid = "";
                        if (_ConsumableInvoiceDetails.Guid != null)
                        {
                            Guid = _ConsumableInvoiceDetails.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        private DataTable ToDtblHDetail(ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails)
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
                dtheader.Columns.Add("MenuID", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(string));
                dtheader.Columns.Add("br_id", typeof(string));
                dtheader.Columns.Add("inv_no", typeof(string));
                dtheader.Columns.Add("inv_dt", typeof(string));
                dtheader.Columns.Add("supp_id", typeof(string));
                dtheader.Columns.Add("bill_no", typeof(string));
                dtheader.Columns.Add("bill_dt", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("tax_amt", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val_bs", typeof(string));
                dtheader.Columns.Add("inv_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("UserID", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(string));
                dtheader.Columns.Add("curr_id", typeof(int));
                dtheader.Columns.Add("conv_rate", typeof(int));
                dtheader.Columns.Add("src_type", typeof(string));
                dtheader.Columns.Add("src_doc_no", typeof(string));
                dtheader.Columns.Add("src_doc_dt", typeof(string));
                dtheader.Columns.Add("ewb_no", typeof(string));
                dtheader.Columns.Add("einv_no", typeof(string));
                dtheader.Columns.Add("roundoff", typeof(string));
                dtheader.Columns.Add("pm_flag", typeof(string));
                dtheader.Columns.Add("rcm_app", typeof(string));
                dtheader.Columns.Add("remarks", typeof(string));
                dtheader.Columns.Add("cancel_remarks", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _ConsumableInvoiceDetails.TransType;
                dtrowHeader["MenuID"] = DocumentMenuId;
                string cancelflag = _ConsumableInvoiceDetails.CancelFlag.ToString();
                if (cancelflag == "False")
                {
                    dtrowHeader["Cancelled"] = "N";
                }
                else
                {
                    dtrowHeader["Cancelled"] = "Y";
                }
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                dtrowHeader["inv_no"] = _ConsumableInvoiceDetails.inv_no;
                dtrowHeader["inv_dt"] = _ConsumableInvoiceDetails.inv_dt;
                dtrowHeader["supp_id"] = _ConsumableInvoiceDetails.SuppID;
                dtrowHeader["bill_no"] = _ConsumableInvoiceDetails.bill_no;
                dtrowHeader["bill_dt"] = _ConsumableInvoiceDetails.bill_date;
                dtrowHeader["gr_val"] = _ConsumableInvoiceDetails.GrVal;
                dtrowHeader["tax_amt"] = _ConsumableInvoiceDetails.TaxAmt;
                dtrowHeader["oc_amt"] = _ConsumableInvoiceDetails.OcAmt;
                dtrowHeader["net_val_bs"] = _ConsumableInvoiceDetails.NetValSpec;
                dtrowHeader["inv_status"] = IsNull(_ConsumableInvoiceDetails.AppStatus, "D");
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["UserID"] = Session["UserId"].ToString();
                dtrowHeader["bill_add_id"] = _ConsumableInvoiceDetails.bill_add_id;
                dtrowHeader["curr_id"] =Convert.ToInt32(_ConsumableInvoiceDetails.curr_id);
                dtrowHeader["conv_rate"] =Convert.ToInt32(_ConsumableInvoiceDetails.conv_rate);
                dtrowHeader["src_type"] = _ConsumableInvoiceDetails.Src_Type;
                if (_ConsumableInvoiceDetails.Src_Type == "O")
                {
                    dtrowHeader["src_doc_no"] = _ConsumableInvoiceDetails.SrcDocNo;
                    dtrowHeader["src_doc_dt"] = _ConsumableInvoiceDetails.SrcDocDate;
                }
                else
                {
                    dtrowHeader["src_doc_no"] = null;
                    dtrowHeader["src_doc_dt"] = null;
                }
                dtrowHeader["ewb_no"] = _ConsumableInvoiceDetails.EWBNNumber;
                dtrowHeader["einv_no"] = _ConsumableInvoiceDetails.EInvoive;
                string roundoffflag = _ConsumableInvoiceDetails.RoundOffFlag.ToString();
                if (roundoffflag == "False")
                {
                    dtrowHeader["roundoff"] = "N";
                }
                else
                {
                    dtrowHeader["roundoff"] = "Y";
                }
                dtrowHeader["pm_flag"] = _ConsumableInvoiceDetails.pmflagval;
                string RCMApplicable = _ConsumableInvoiceDetails.RCMApplicable.ToString();
                if (RCMApplicable == "False")
                {
                    dtrowHeader["rcm_app"] = "N";
                }
                else
                {
                    dtrowHeader["rcm_app"] = "Y";
                }
                dtrowHeader["remarks"] = _ConsumableInvoiceDetails.remarks;
                dtrowHeader["cancel_remarks"] = _ConsumableInvoiceDetails.CancelledRemarks;
                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;
                return DtblHDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
                //return null;
            }
        }
        private DataTable ToDtblItemDetail(string Itemdetails)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("inv_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_gr_val", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("claim_itc", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(string));
                dtItem.Columns.Add("sr_no", typeof(int));
                dtItem.Columns.Add("remarks", typeof(string));
                if (Itemdetails != null)
                {
                    JArray jObject = JArray.Parse(Itemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    { 
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["UOMID"].ToString());
                        dtrowLines["inv_qty"] = jObject[i]["InvQty"].ToString();
                        dtrowLines["item_rate"] = jObject[i]["ItmRate"].ToString();
                        dtrowLines["item_gr_val"] = jObject[i]["GrossVal"].ToString();
                        dtrowLines["item_tax_amt"] = jObject[i]["TaxAmt"].ToString();
                        dtrowLines["item_oc_amt"] = jObject[i]["OCAmt"].ToString();
                        dtrowLines["item_net_val_bs"] = jObject[i]["NetValBase"].ToString();
                        dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                        dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                        dtrowLines["claim_itc"] = jObject[i]["ClaimITC"].ToString();
                        dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                        dtrowLines["item_acc_id"] = jObject[i]["item_acc_id"].ToString();
                        dtrowLines["sr_no"] = jObject[i]["sr_no"].ToString();
                        dtrowLines["remarks"] = jObject[i]["Item_remarks"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                }
                DtblItemDetail = dtItem;
                return DtblItemDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable ToDtblTaxDetail(string TaxDetails,string type)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("tax_id", typeof(int));
                dtItem.Columns.Add("tax_rate", typeof(string));
                dtItem.Columns.Add("tax_val", typeof(string));
                dtItem.Columns.Add("tax_level", typeof(int));
                dtItem.Columns.Add("tax_apply_on", typeof(string));
                dtItem.Columns.Add("tax_recov", typeof(string));

                if (TaxDetails != null)
                {
                    JArray jObject = JArray.Parse(TaxDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["tax_id"] = jObject[i]["TaxID"].ToString();
                        string tax_rate = jObject[i]["TaxRate"].ToString();
                        tax_rate = tax_rate.Replace("%", "");
                        dtrowLines["tax_rate"] = tax_rate;
                        dtrowLines["tax_val"] = jObject[i]["TaxValue"].ToString();
                        dtrowLines["tax_level"] = jObject[i]["TaxLevel"].ToString();
                        dtrowLines["tax_apply_on"] = jObject[i]["TaxApplyOn"].ToString();
                        if (type == "Tax")
                        {
                            dtrowLines["tax_recov"] = jObject[i]["tax_recov"].ToString();
                        }
                        else
                        {
                            dtrowLines["tax_recov"] = "";
                        }
                        dtItem.Rows.Add(dtrowLines);
                    }
                }

                DtblItemTaxDetail = dtItem;
                return DtblItemTaxDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable ToDtblOCDetail(string OCDetails)
        {
            try
            {
                DataTable DtblItemOCDetail = new DataTable();
                DataTable dtItem = new DataTable();

                //dtItem.Columns.Add("OC_ID", typeof(int));
                //dtItem.Columns.Add("OCValue", typeof(string));
                dtItem.Columns.Add("oc_id", typeof(string));
                dtItem.Columns.Add("oc_val", typeof(string));
                dtItem.Columns.Add("tax_amt", typeof(string));
                dtItem.Columns.Add("total_amt", typeof(string));
                dtItem.Columns.Add("curr_id", typeof(string));
                dtItem.Columns.Add("conv_rate", typeof(string));
                dtItem.Columns.Add("supp_id", typeof(string));
                dtItem.Columns.Add("supp_type", typeof(string));
                dtItem.Columns.Add("bill_no", typeof(string));
                dtItem.Columns.Add("bill_date", typeof(string));
                dtItem.Columns.Add("round_off", typeof(string));
                dtItem.Columns.Add("pm_flag", typeof(string));
                dtItem.Columns.Add("tds_amt", typeof(string));

                if (OCDetails != null)
                {
                    JArray jObject = JArray.Parse(OCDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        //dtrowLines["OC_ID"] = jObject[i]["OC_ID"].ToString();
                        //dtrowLines["OCValue"] = jObject[i]["OCValue"].ToString();
                        dtrowLines["oc_id"] = jObject[i]["oc_id"].ToString();
                        dtrowLines["oc_val"] = jObject[i]["oc_val"].ToString();
                        dtrowLines["tax_amt"] = jObject[i]["tax_amt"].ToString();
                        dtrowLines["total_amt"] = jObject[i]["total_amt"].ToString();
                        dtrowLines["curr_id"] = jObject[i]["curr_id"].ToString();
                        dtrowLines["conv_rate"] = jObject[i]["OC_Conv"].ToString();
                        dtrowLines["supp_id"] = jObject[i]["supp_id"].ToString();
                        dtrowLines["supp_type"] = jObject[i]["supp_type"].ToString();
                        dtrowLines["bill_no"] = jObject[i]["bill_no"].ToString();
                        dtrowLines["bill_date"] = jObject[i]["bill_date"].ToString();
                        dtrowLines["round_off"] = jObject[i]["round_off"].ToString();
                        dtrowLines["pm_flag"] = jObject[i]["pm_flag"].ToString();
                        dtrowLines["tds_amt"] = jObject[i]["tds_amt"].ToString();
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
        private DataTable ToDtblVouDetail(string VouDetails)
        {
            try
            {
                DataTable DtblVouDetail = new DataTable();
                DataTable vou_Details = new DataTable();
                vou_Details.Columns.Add("comp_id", typeof(string));
                vou_Details.Columns.Add("vou_sr_no", typeof(string));
                vou_Details.Columns.Add("gl_sr_no", typeof(string));
                vou_Details.Columns.Add("id", typeof(string));
                vou_Details.Columns.Add("type", typeof(string));
                vou_Details.Columns.Add("doctype", typeof(string));
                vou_Details.Columns.Add("Value", typeof(string));
                vou_Details.Columns.Add("ValueInBase", typeof(string));
                vou_Details.Columns.Add("DrAmt", typeof(string));
                vou_Details.Columns.Add("CrAmt", typeof(string));
                vou_Details.Columns.Add("TransType", typeof(string));
                vou_Details.Columns.Add("Gltype", typeof(string));
                vou_Details.Columns.Add("parent", typeof(string));
                vou_Details.Columns.Add("DrAmtInBase", typeof(string));
                vou_Details.Columns.Add("CrAmtInBase", typeof(string));
                vou_Details.Columns.Add("curr_id", typeof(string));
                vou_Details.Columns.Add("conv_rate", typeof(string));
                vou_Details.Columns.Add("vou_type", typeof(string));
                vou_Details.Columns.Add("bill_no", typeof(string));
                vou_Details.Columns.Add("bill_date", typeof(string));
                vou_Details.Columns.Add("gl_narr", typeof(string));
                if (VouDetails != null)
                {
                    JArray jObjectVOU = JArray.Parse(VouDetails);
                    for (int i = 0; i < jObjectVOU.Count; i++)
                    {
                        DataRow dtrowVouDetailsLines = vou_Details.NewRow();
                        dtrowVouDetailsLines["comp_id"] = jObjectVOU[i]["comp_id"].ToString();
                        dtrowVouDetailsLines["vou_sr_no"] = jObjectVOU[i]["VouSrNo"].ToString();
                        dtrowVouDetailsLines["gl_sr_no"] = jObjectVOU[i]["GlSrNo"].ToString();
                        dtrowVouDetailsLines["id"] = jObjectVOU[i]["id"].ToString();
                        dtrowVouDetailsLines["type"] = jObjectVOU[i]["type"].ToString();
                        dtrowVouDetailsLines["doctype"] = jObjectVOU[i]["doctype"].ToString();
                        dtrowVouDetailsLines["Value"] = jObjectVOU[i]["Value"].ToString();
                        dtrowVouDetailsLines["ValueInBase"] = jObjectVOU[i]["ValueInBase"].ToString();
                        dtrowVouDetailsLines["DrAmt"] = jObjectVOU[i]["DrAmt"].ToString();
                        dtrowVouDetailsLines["CrAmt"] = jObjectVOU[i]["CrAmt"].ToString();
                        dtrowVouDetailsLines["TransType"] = jObjectVOU[i]["TransType"].ToString();
                        dtrowVouDetailsLines["Gltype"] = jObjectVOU[i]["Gltype"].ToString();
                        dtrowVouDetailsLines["parent"] = "0";// jObjectVOU[i]["Gltype"].ToString();
                        dtrowVouDetailsLines["DrAmtInBase"] = jObjectVOU[i]["DrAmtInBase"].ToString();
                        dtrowVouDetailsLines["CrAmtInBase"] = jObjectVOU[i]["CrAmtInBase"].ToString();
                        dtrowVouDetailsLines["curr_id"] = jObjectVOU[i]["curr_id"].ToString();
                        dtrowVouDetailsLines["conv_rate"] = jObjectVOU[i]["conv_rate"].ToString();
                        dtrowVouDetailsLines["vou_type"] = jObjectVOU[i]["vou_type"].ToString();
                        dtrowVouDetailsLines["bill_no"] = jObjectVOU[i]["bill_no"].ToString();
                        dtrowVouDetailsLines["bill_date"] = jObjectVOU[i]["bill_date"].ToString();
                        dtrowVouDetailsLines["gl_narr"] = jObjectVOU[i]["gl_narr"].ToString();
                        vou_Details.Rows.Add(dtrowVouDetailsLines);
                    }
                    //ViewData["VouDetail"] = dtVoudetail(jObjectVOU);
                }
                DtblVouDetail = vou_Details;
                return DtblVouDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblTdsDetail(string tdsDetails, string tdsType)
        {
            try
            {
                DataTable DtblItemtdsDetail = new DataTable();
                DataTable tds_detail = new DataTable();
                tds_detail.Columns.Add("tds_id", typeof(string));
                tds_detail.Columns.Add("tds_rate", typeof(string));
                tds_detail.Columns.Add("tds_val", typeof(string));
                tds_detail.Columns.Add("tds_level", typeof(string));
                tds_detail.Columns.Add("tds_apply_on", typeof(string));
                tds_detail.Columns.Add("tds_ass_val", typeof(string));
                if (tdsType == "OC")
                {
                    tds_detail.Columns.Add("oc_id", typeof(string));
                    tds_detail.Columns.Add("supp_id", typeof(string));
                }
                tds_detail.Columns.Add("tds_base_amt", typeof(string));
                tds_detail.Columns.Add("tds_ass_apply_on", typeof(string));

                if (tdsDetails != null)
                {
                    JArray jObjecttds = JArray.Parse(tdsDetails);
                    for (int i = 0; i < jObjecttds.Count; i++)
                    {
                        DataRow dtrowtdsDetailsLines = tds_detail.NewRow();
                        dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["Tds_id"].ToString();
                        string tds_rate = jObjecttds[i]["Tds_rate"].ToString();
                        tds_rate = tds_rate.Replace("%", "");
                        dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                        dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["Tds_level"].ToString();
                        dtrowtdsDetailsLines["tds_val"] = jObjecttds[i]["Tds_val"].ToString();
                        dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["Tds_apply_on"].ToString();
                        dtrowtdsDetailsLines["tds_ass_val"] = jObjecttds[i]["Tds_totalAmnt"].ToString();
                        if (tdsType == "OC")
                        {
                            dtrowtdsDetailsLines["oc_id"] = jObjecttds[i]["Tds_oc_id"].ToString();
                            dtrowtdsDetailsLines["supp_id"] = jObjecttds[i]["Tds_supp_id"].ToString();
                        }
                        dtrowtdsDetailsLines["tds_base_amt"] = jObjecttds[i]["Tds_totalAmnt"].ToString();
                        dtrowtdsDetailsLines["tds_ass_apply_on"] = jObjecttds[i]["Tds_AssValApplyOn"].ToString();

                        tds_detail.Rows.Add(dtrowtdsDetailsLines);
                    }
                }
                DtblItemtdsDetail = tds_detail;
                return DtblItemtdsDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public ActionResult DeleteCI_Details(ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails, string Command)
        {
            try
            {
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string BranchID = string.Empty;
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
                string GRN_Delete = _ConsumableInvoice_ISERVICE.Delete_CI_Detail(_ConsumableInvoiceDetails, Comp_ID, BranchID);
                /*--------------------------For Attatchment Start--------------------------*/
                if (!string.IsNullOrEmpty(_ConsumableInvoiceDetails.inv_no))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string InvNo1 = _ConsumableInvoiceDetails.inv_no.Replace("/", "");
                    other.DeleteTempFile(Comp_ID + BranchID, PageName, InvNo1, Server);
                }
                /*--------------------------For Attatchment End--------------------------*/
                _ConsumableInvoiceDetails.Message = "Deleted";
                _ConsumableInvoiceDetails.Command = "Refresh";
                _ConsumableInvoiceDetails.TransType = "Refresh";
                _ConsumableInvoiceDetails.AppStatus = "D";
                _ConsumableInvoiceDetails.BtnName = "BtnDelete";
                //TempData["ModelData"] = _ConsumableInvoiceDetails;
                return RedirectToAction("ConsumableInvoiceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult Approve_ConsumableInvoice(string Inv_No, string Inv_Date, string A_Status
            , string A_Level, string A_Remarks, string VoucherNarr, string FilterData,string curr_id
            ,string conv_rate, string WF_status1, string Bp_Nurr,string Dn_Nurr)
        {
            try
            {
                ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails = new ConsumableInvoiceDetails_Model();
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string BranchID = string.Empty;
                string MenuDocId = string.Empty;
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
                if (Session["MenuDocumentId"] != null)
                {
                    MenuDocId = Session["MenuDocumentId"].ToString();
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _ConsumableInvoice_ISERVICE.Approve_CI(Inv_No, Inv_Date, DocumentMenuId
                    , BranchID, Comp_ID, UserID, mac_id, A_Status, A_Level, A_Remarks, VoucherNarr
                    , curr_id,conv_rate, Bp_Nurr, Dn_Nurr);
                string[] FDetail = Message.Split(',');
                string ApMessage = FDetail[6].ToString().Trim();
                string INV_NO = FDetail[0].ToString();
                string INV_DT = FDetail[7].ToString();

                if (ApMessage == "A")
                {
                    try
                    {
                        //string fileName = "CI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "ConsumbleInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(INV_NO, INV_DT, fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, INV_NO, "AP", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _ConsumableInvoiceDetails.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                }
                _ConsumableInvoiceDetails.TransType = "Update";
                //_ConsumableInvoiceDetails.Command = "Approve";
                _ConsumableInvoiceDetails.Message = _ConsumableInvoiceDetails.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                _ConsumableInvoiceDetails.inv_no = INV_NO;
                _ConsumableInvoiceDetails.inv_dt = INV_DT;
                _ConsumableInvoiceDetails.AppStatus = "D";
                _ConsumableInvoiceDetails.BtnName = "BtnEdit";
                _ConsumableInvoiceDetails.WF_status1 = WF_status1;
                TempData["WF_status1"] = WF_status1;
                TempData["ListFilterData"] = FilterData;
                TempData["ModelData"] = _ConsumableInvoiceDetails;
                var CI_NoURL = INV_NO;
                var CI_Date = INV_DT;
                var TransType = "Update";
                var BtnName = "BtnEdit";
                var Command = "Approve";
                return RedirectToAction("ConsumableInvoiceDetail", new { CI_NoURL = CI_NoURL, CI_Date, TransType, BtnName, Command });
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (A_Status == "")
                {
                    throw ex;
                }
                else
                {
                    return Json("ErrorPage");
                }
                
            }
        }
        public ActionResult SearchCI_Detail(string SuppId, string Fromdate, string Todate, string Status)
        {
            try
            {
                ConsumableInvoice_Model _ConsumableInvoice = new ConsumableInvoice_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                string wfstatus = "";               
                _ConsumableInvoice.SuppID = SuppId;
                _ConsumableInvoice.FromDate = Fromdate;
                _ConsumableInvoice.ToDate = Todate;
                _ConsumableInvoice.Status = Status;
                DataSet dt1 = _ConsumableInvoice_ISERVICE.GetCIDetailList(CompID, BrchID, _ConsumableInvoice.SuppID, _ConsumableInvoice.FromDate, _ConsumableInvoice.ToDate, _ConsumableInvoice.Status, UserID, wfstatus, DocumentMenuId);
                ViewBag.ConsumableInvoiceList = dt1.Tables[0];
                _ConsumableInvoice.LPISearch = "LPI_Search";
                _ConsumableInvoice.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.MenuPageName = getDocumentName();
                CommonPageDetails();
                //return View("~/Areas/ApplicationLayer/Views/Procurement/ConsumableInvoice/ConsumableInvoiceList.cshtml", _ConsumableInvoice);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialConsumableInvoiceList.cshtml", _ConsumableInvoice);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
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
        public ActionResult ToRefreshByJS(string FilterData,string TrancType,string Mailerror)
        {
            ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails = new ConsumableInvoiceDetails_Model();
            var a = TrancType.Split(',');
            _ConsumableInvoiceDetails.inv_no = a[0].Trim();
            _ConsumableInvoiceDetails.inv_dt = a[1].Trim();
            _ConsumableInvoiceDetails.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            _ConsumableInvoiceDetails.BtnName = "BtnToDetailPage";
            _ConsumableInvoiceDetails.WF_status1 = WF_status1;
            TempData["WF_status1"] = WF_status1;
            TempData["ModelData"] = _ConsumableInvoiceDetails;
            TempData["ListFilterData"] = FilterData;
            var CI_NoURL = a[0].Trim();
            var CI_Date = a[1].Trim();
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var Command = "Refresh";
            _ConsumableInvoiceDetails.Message = Mailerror;
            return ( RedirectToAction("ConsumableInvoiceDetail", new { CI_NoURL = CI_NoURL, CI_Date, TransType, BtnName, Command }));
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                ConsumableInvoiceattch _ConsumableInvoiceattch = new ConsumableInvoiceattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string SuppCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();

                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                //SuppCode = SuppCode.Replace("/", "");
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = SuppCode;
                _ConsumableInvoiceattch.Guid = DocNo;

                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _ConsumableInvoiceattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _ConsumableInvoiceattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _ConsumableInvoiceattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
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

        /*------------- Cost Center Section-----------------*/

        public ActionResult GetCstCntrtype(string Flag, string Disableflag, string CC_rowdata)
        {
            try
            {
                CostCenterDt _CC_Model = new CostCenterDt();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                if (CC_rowdata.ToString() != null && CC_rowdata.ToString() != "" && CC_rowdata.ToString() != "[]")
                {
                    DataTable Cctype = new DataTable();
                    Cctype.Columns.Add("GlAccount", typeof(string));
                    Cctype.Columns.Add("ddl_cc_type", typeof(string));
                    Cctype.Columns.Add("dd_cc_type_id", typeof(string));
                    JArray arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow dtrowLines = Cctype.NewRow();
                        dtrowLines["GlAccount"] = arr[i]["GlAccount"].ToString();
                        dtrowLines["ddl_cc_type"] = arr[i]["ddl_CC_Type"].ToString();
                        dtrowLines["dd_cc_type_id"] = arr[i]["ddl_Type_Id"].ToString();
                        Cctype.Rows.Add(dtrowLines);
                    }
                    DataView dv = new DataView();
                    dv = Cctype.DefaultView;
                    ViewBag.CC_type = dv.ToTable(true, "GlAccount", "ddl_cc_type", "dd_cc_type_id");

                    DataTable ccitem = new DataTable();
                    ccitem.Columns.Add("dd_cc_typ_id", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name_id", typeof(string));
                    ccitem.Columns.Add("cc_Amount", typeof(string));

                    JArray Arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow DtrowLines = ccitem.NewRow();
                        DtrowLines["dd_cc_typ_id"] = arr[i]["ddl_Type_Id"].ToString();
                        DtrowLines["ddl_cc_name"] = arr[i]["ddl_CC_Name"].ToString();
                        DtrowLines["ddl_cc_name_id"] = arr[i]["ddl_Name_Id"].ToString();
                        DtrowLines["cc_Amount"] = arr[i]["CC_Amount"].ToString();
                        ccitem.Rows.Add(DtrowLines);
                    }
                    ViewBag.CC_Item = ccitem;
                }


                DataSet ds = _Common_IServices.GetCstCntrData(CompID, BrchID, "0", Flag);

                List<CostcntrType> Cctypelist = new List<CostcntrType>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CostcntrType Cc_type = new CostcntrType();
                    Cc_type.cc_id = dr["cc_id"].ToString();
                    Cc_type.cc_name = dr["cc_name"].ToString();
                    Cctypelist.Add(Cc_type);
                }
                Cctypelist.Insert(0, new CostcntrType() { cc_id = "0", cc_name = "---Select---" });
                _CC_Model.costcntrtype = Cctypelist;

                _CC_Model.disflag = Disableflag;

                return PartialView("~/Areas/Common/Views/Cmn_PartialCostCenterDetail.cshtml", _CC_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public JsonResult GetCstCntrName(string CCtypeid)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds = _Common_IServices.GetCstCntrData(CompID, BrchID, CCtypeid, "ccname");
                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        /*------------- Cost Center Section END ----------------*/

        public JsonResult GetSourceDocList( string SuppID)
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
                DataSet Deatils = _ConsumableInvoice_ISERVICE.GetSourceDocList(Comp_ID, Br_ID, SuppID);
                DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult getdatapoitemconsinvoice(string suppid, string srdocNo, string srcdoc_dt)
        {
            string BrchID = string.Empty;
            string CompID = string.Empty;
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds = _ConsumableInvoice_ISERVICE.getdataPOitemtabledata(CompID, BrchID, srdocNo, srcdoc_dt, suppid);
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
        public ActionResult GetBillNoBillDate(string suppid, string srdocNo, string srcdoc_dt)
        {
            string BrchID = string.Empty;
            string CompID = string.Empty;
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds = _ConsumableInvoice_ISERVICE.GetDataBillNoBillDate(CompID, BrchID, srdocNo, srcdoc_dt, suppid);
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
        /*-------------Print Section--------------*/
        public FileResult GenratePdfFile(string docMenuId, string invNo, string invDate)
        {
            return File(GetPdfData(docMenuId, invNo, invDate), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        }
        public byte[] GetPdfData(string docId, string invNo, string invDt)
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
                DataSet Details = _ConsumableInvoice_ISERVICE.GetConsumbleInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt);
                ViewBag.PageName = "PI";
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                ViewBag.Details = Details;
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                ViewBag.Title = "Consumble Invoice";
                htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/ConsumableInvoice/ConsumbleInvoicePrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
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
                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            finally
            {
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
        /*-----------Print Section End-----------*/
        /*------------- Added by Nidhi 13-05-2025 16:13 ----------*/
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData("105101152", Doc_no, Doc_dt);
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
        /*-------------- END 13-05-2025 16:13 ---------------*/
        public string SavePdfDocToSendOnEmailAlert_Ext(string docid, string Doc_no, string Doc_dt, string fileName)
        {
            DataTable dt = new DataTable();
            var commonCont = new CommonController(_Common_IServices);
            var data = GetPdfData(docid, Doc_no, Doc_dt);
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
        public ActionResult SendEmailAlert(string mail_id, string status, string docid, string Doc_no, string Doc_dt, string filepath)
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
                        string fileName = "CI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        filepath = SavePdfDocToSendOnEmailAlert_Ext(docid, Doc_no, Doc_dt, fileName);
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
    }

}