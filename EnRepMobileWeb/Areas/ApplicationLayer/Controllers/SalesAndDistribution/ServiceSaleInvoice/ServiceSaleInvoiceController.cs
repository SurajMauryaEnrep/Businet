using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ServiceSaleInvoice;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.Resources;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.ServiceSaleInvoice;
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
using ZXing;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.ServiceSaleInvoice
{
    public class ServiceSaleInvoiceController : Controller
    {
        List<ServiceSalesInvoiceList> _SSIList;
        string CompID, language, title, BrchID, UserID, create_id = string.Empty, crm ="Y";
        string DocumentMenuId = "105103147";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ServiceSI_ISERVICE _ServiceSI_ISERVICE;

        public ServiceSaleInvoiceController(Common_IServices _Common_IServices, ServiceSI_ISERVICE _ServiceSI_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._ServiceSI_ISERVICE = _ServiceSI_ISERVICE;
        }
        // GET: ApplicationLayer/ServiceSaleInvoice
        public ActionResult ServiceSaleInvoice(ServiceSIListModel _SSIListModel)
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
                if (DocumentMenuId != null)
                {
                    _SSIListModel.wfdocid = DocumentMenuId;
                }
                else
                {
                    _SSIListModel.wfdocid = "0";
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["crm"] != null)
                {
                    crm = Session["crm"].ToString();
                }
                if (UserID == "1001")
                {
                    crm = "Y";
                }
                ViewBag.crm = crm;
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                #region Commented By Nitesh 08-04-2024 For All Data In One Procedure
                #endregion
                // GetAutoCompleteSearchCustList(_SSIListModel);
                #region
                #endregion
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    if (PRData != null && PRData != "")
                    {
                        var a = PRData.Split(',');
                        _SSIListModel.CustID = a[0].Trim();
                        _SSIListModel.SSI_FromDate = a[1].Trim();
                        _SSIListModel.SSI_ToDate = a[2].Trim();
                        _SSIListModel.Status = a[3].Trim();
                        _SSIListModel.SQ_SalePerson = a[4].Trim();
                        if (_SSIListModel.Status == "0")
                        {
                            _SSIListModel.Status = null;

                        }
                        _SSIListModel.FromDate = _SSIListModel.SSI_FromDate;
                        _SSIListModel.ListFilterData = TempData["ListFilterData"].ToString();
                       // _SSIListModel.SSIList = GetServiceSIList(_SSIListModel);
                    }
                    else
                    {
                    //    _SSIListModel.SSIList = GetServiceSIList(_SSIListModel);
                        _SSIListModel.FromDate = startDate;
                        _SSIListModel.SSI_FromDate = startDate;
                        _SSIListModel.SSI_ToDate = CurrentDate;
                    }
                }
                else
                {
                    List<CustomerName> _CustList = new List<CustomerName>();

                    _CustList.Insert(0, new CustomerName() { Cust_id = "0", Cust_name = "---Select---" });
                    _SSIListModel.CustomerNameList = _CustList;
                   // _SSIListModel.SSIList = GetServiceSIList(_SSIListModel);
                    _SSIListModel.FromDate = startDate;
                    _SSIListModel.SSI_FromDate = startDate;
                    _SSIListModel.SSI_ToDate = CurrentDate;
                }
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
                _SSIListModel.StatusList = statusLists;
                GetAllData(_SSIListModel);
                _SSIListModel.Title = title;
                _SSIListModel.SSISearch = "0";
                _SSIListModel.GstApplicable = ViewBag.GstApplicable;
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceList.cshtml", _SSIListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(ServiceSIListModel _SSIListModel)
        {
            string CustomerName = string.Empty;
            string wfStatus = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string CustType = string.Empty;
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (string.IsNullOrEmpty(_SSIListModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _SSIListModel.CustName;
                }
                CustType = "D";
                if (_SSIListModel.WF_status != null)
                {
                    wfStatus = _SSIListModel.WF_status;
                }
                else
                {
                    wfStatus = null;
                }
                DataSet GetAllData = _ServiceSI_ISERVICE.GetAllData(Comp_ID, CustomerName, Br_ID, CustType
                    , UserID, _SSIListModel.CustID, _SSIListModel.SSI_FromDate, _SSIListModel.SSI_ToDate, _SSIListModel.Status, _SSIListModel.wfdocid, wfStatus, _SSIListModel.SQ_SalePerson);
                List<CustomerName> _CustmrList = new List<CustomerName>();
                foreach (DataRow data in GetAllData.Tables[0].Rows)
                {
                    CustomerName _SuppDetail = new CustomerName();
                    _SuppDetail.Cust_id = data["cust_id"].ToString();
                    _SuppDetail.Cust_name = data["cust_name"].ToString();
                    _CustmrList.Add(_SuppDetail);
                }
                _CustmrList.Insert(0,new CustomerName() { Cust_id="0",Cust_name="All"});
                _SSIListModel.CustomerNameList = _CustmrList;

                List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                foreach (DataRow data in GetAllData.Tables[4].Rows)
                {
                    SalePersonList _SlPrsnDetail = new SalePersonList();
                    _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                    _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                    _SlPrsnList.Add(_SlPrsnDetail);
                }
                _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                _SSIListModel.SalePersonList = _SlPrsnList;

                SetAllData(GetAllData, _SSIListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void SetAllData(DataSet DSet, ServiceSIListModel _SSIListModel)
        {
            List<ServiceSalesInvoiceList>  _SSIList = new List<ServiceSalesInvoiceList>();
            if (DSet.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in DSet.Tables[1].Rows)
                {
                    ServiceSalesInvoiceList _ServicePIList = new ServiceSalesInvoiceList();
                    _ServicePIList.InvNo = dr["inv_no"].ToString();
                    _ServicePIList.InvDate = dr["inv_dt"].ToString();
                    _ServicePIList.src_type = dr["src_type"].ToString();
                    _ServicePIList.src_doc_no = dr["src_doc_number"].ToString();
                    _ServicePIList.src_doc_dt = dr["src_doc_date"].ToString();
                    _ServicePIList.SalesPerson = dr["SalesPerson"].ToString();
                    _ServicePIList.InvDt = dr["InvDt"].ToString();
                    _ServicePIList.InvValue = dr["net_val_bs"].ToString();
                    _ServicePIList.CustName = dr["cust_name"].ToString();
                    _ServicePIList.RefDocNo = dr["ref_doc_no"].ToString();
                    _ServicePIList.RefDocDt = dr["ref_doc_dt"].ToString();
                    _ServicePIList.InvStauts = dr["InvStauts"].ToString();
                    _ServicePIList.CreateDate = dr["CreateDate"].ToString();
                    _ServicePIList.ApproveDate = dr["ApproveDate"].ToString();
                    _ServicePIList.ModifyDate = dr["ModifyDate"].ToString();
                    _ServicePIList.create_by = dr["create_by"].ToString();
                    _ServicePIList.app_by = dr["app_by"].ToString();
                    _ServicePIList.mod_by = dr["mod_by"].ToString();
                    _SSIList.Add(_ServicePIList);
                }
            }
            _SSIListModel.SSIList = _SSIList;
            ViewBag.FinStDt = DSet.Tables[3].Rows[0]["findate"];
        }
        public ActionResult AddServiceSaleInvoiceDetail()
        {
            ServiceSIModel _ServiceSIModel = new ServiceSIModel();
            _ServiceSIModel.Message = "New";
            _ServiceSIModel.Command = "Add";
            _ServiceSIModel.AppStatus = "D";
            _ServiceSIModel.TransType = "Save";
            _ServiceSIModel.BtnName = "BtnAddNew";
            _ServiceSIModel.DocumentStatus = "D";
            TempData["ModelData"] = _ServiceSIModel;
            TempData["ListFilterData"] = null;
            ViewBag.DocumentStatus = "D";
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            ViewBag.MenuPageName = getDocumentName();
            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ServiceSaleInvoice");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("ServiceSaleInvoiceDetail", "ServiceSaleInvoice");
        }
        public ActionResult ServiceSaleInvoiceDetail(URLModelDetails URLModel)
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
            if (Session["crm"] != null)
            {
                crm = Session["crm"].ToString();
            }
            if (UserID == "1001")
            {
                crm = "Y";
            }
            ViewBag.crm = crm;
            /*Add by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.Sinv_dt) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
                var _ServiceSIModel = TempData["ModelData"] as ServiceSIModel;
                if (_ServiceSIModel != null)
                {
                    //ServiceSIModel _ServiceSIModel = new ServiceSIModel();
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _ServiceSIModel.GstApplicable = ViewBag.GstApplicable;
                    //Session["SSISearch"] = null;
                    _ServiceSIModel.SSISearch = null;

                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.RateDigit = RateDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    _ServiceSIModel.SO_SourceType = "D";
                    List<CustomerName> _CustList = new List<CustomerName>();

                    _CustList.Insert(0, new CustomerName() { Cust_id = "0", Cust_name = "---Select---" });
                    _ServiceSIModel.CustomerNameList = _CustList;

                    List<SrcDocNo> _SrcDocNo = new List<SrcDocNo>();

                    _SrcDocNo.Insert(0, new SrcDocNo() { Src_DocNo = "0", SrcDocNoVal = "---Select---" });
                    _ServiceSIModel.SrcdocnoList = _SrcDocNo;
                    GetSalesPersonList(_ServiceSIModel);

                   _ServiceSIModel.Title = title;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ServiceSIModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _ServiceSIModel.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["Inv_No"] != null && Session["Inv_Date"] != null)
                    if (_ServiceSIModel.Sinv_no != null && _ServiceSIModel.Sinv_dt != null)
                    {
                        string Doc_no = _ServiceSIModel.Sinv_no;
                        string Doc_date = _ServiceSIModel.Sinv_dt;
                        //string Doc_no = Session["Inv_No"].ToString();
                        //string Doc_date = Session["Inv_Date"].ToString();

                        DataSet ds = GetServiceSaleInvoiceEdit(Doc_no, Doc_date);


                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            ViewBag.AttechmentDetails = ds.Tables[8];
                            ViewBag.GLAccount = ds.Tables[7];
                            ViewBag.GLTOtal = ds.Tables[9];
                            //ViewBag.TaxDetail = ds.Tables[2];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            //ViewBag.OCDetail = ds.Tables[3];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.TotalDetails = ds.Tables[0];
                            //ViewBag.TaxDetail2 = ds.Tables[10];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OCTaxDetails = ds.Tables[11];
                            ViewBag.CostCenterData = ds.Tables[12];
                            ViewBag.ItemTDSDetails = ds.Tables[13];
                            ViewBag.ItemOC_TDSDetails = ds.Tables[14];
                            ViewBag.PaymentScheduleData = ds.Tables[15];

                            _ServiceSIModel.Sinv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _ServiceSIModel.Sinv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                          
                          
                            _ServiceSIModel.CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();
                            _ServiceSIModel.CustID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                            
                            _CustList.Add(new CustomerName { Cust_id = _ServiceSIModel.CustID, Cust_name = _ServiceSIModel.CustName });
                            _ServiceSIModel.CustomerNameList = _CustList;

                            _ServiceSIModel.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _ServiceSIModel.SourceDocDate = ds.Tables[0].Rows[0]["src_doc_date"].ToString();
                            _SrcDocNo.Insert(0, new SrcDocNo() { Src_DocNo = _ServiceSIModel.SrcDocNo, SrcDocNoVal = _ServiceSIModel.SrcDocNo });
                            _ServiceSIModel.SrcdocnoList = _SrcDocNo;
                            _ServiceSIModel.SO_SourceType = ds.Tables[0].Rows[0]["src_type"].ToString().Trim();
                            _ServiceSIModel.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _ServiceSIModel.SourceDocDate = ds.Tables[0].Rows[0]["src_doc_date"].ToString();
                            _ServiceSIModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _ServiceSIModel.Create_by = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _ServiceSIModel.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _ServiceSIModel.Approved_by = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _ServiceSIModel.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _ServiceSIModel.Amended_by = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _ServiceSIModel.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _ServiceSIModel.StatusName = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                            _ServiceSIModel.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                            _ServiceSIModel.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                            _ServiceSIModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _ServiceSIModel.Address = ds.Tables[0].Rows[0]["CustAddress"].ToString();
                            _ServiceSIModel.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _ServiceSIModel.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _ServiceSIModel.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _ServiceSIModel.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _ServiceSIModel.SSIStatus = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            _ServiceSIModel.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            _ServiceSIModel.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            _ServiceSIModel.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                            _ServiceSIModel.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                            _ServiceSIModel.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            _ServiceSIModel.bs_curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            _ServiceSIModel.ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _ServiceSIModel.Currency = ds.Tables[0].Rows[0]["curr_Name"].ToString();
                            _ServiceSIModel.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                            _ServiceSIModel.RefDoc_No = ds.Tables[0].Rows[0]["ref_doc_no"].ToString();
                            _ServiceSIModel.RefDoc_Dt = ds.Tables[0].Rows[0]["ref_doc_dt"].ToString();
                            _ServiceSIModel.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                            _ServiceSIModel.slprsn_id = ds.Tables[0].Rows[0]["sls_per"].ToString();
                            if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            {
                                _ServiceSIModel.Hd_GstType = "Both";
                            }
                            else
                            {
                                _ServiceSIModel.Hd_GstType = "IGST";
                            }
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                            string doc_status = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            _ServiceSIModel.DocumentStatus = doc_status;
                            ViewBag.Approve_id = approval_id;
                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                if (doc_status == "A" || doc_status == "C")
                                {
                                    _ServiceSIModel.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                                }
                                _ServiceSIModel.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                                _ServiceSIModel.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                                ViewBag.GLVoucherNo = _ServiceSIModel.GLVoucherNo;/*add by Hina Sharma on 11-08-2025*/
                            }
                            _ServiceSIModel.ddlCustome_Reference = ds.Tables[0].Rows[0]["cust_ref"].ToString();
                            _ServiceSIModel.ddlPayment_term = ds.Tables[0].Rows[0]["pay_term"].ToString();
                            _ServiceSIModel.ddlDelivery_term = ds.Tables[0].Rows[0]["deli_term"].ToString();
                            _ServiceSIModel.Declaration_1 = ds.Tables[0].Rows[0]["declar_1"].ToString();
                            _ServiceSIModel.Declaration_2 = ds.Tables[0].Rows[0]["declar_2"].ToString();
                            _ServiceSIModel.Invoice_Heading = ds.Tables[0].Rows[0]["inv_heading"].ToString();
                            _ServiceSIModel.Ship_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                            _ServiceSIModel.ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                            _ServiceSIModel.ShipTo = ds.Tables[0].Rows[0]["ship_to"].ToString();
                            _ServiceSIModel.txt_PlcOfSupply = ds.Tables[0].Rows[0]["placeofsupply"].ToString();
                            string nontaxable = ds.Tables[0].Rows[0]["non_taxable"].ToString();
                            if (nontaxable == "Y")
                            {
                                _ServiceSIModel.nontaxable = true;
                            }
                            else
                            {
                                _ServiceSIModel.nontaxable = false;
                            }
                            string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                            _ServiceSIModel.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                            if (roundoff_status == "Y")
                            {
                                _ServiceSIModel.RoundOffFlag = true;
                            }
                            else
                            {
                                _ServiceSIModel.RoundOffFlag = false;
                            }
                            ViewBag.DocumentStatus = doc_status;
                            //Session["DocumentStatus"] = doc_status;
                            _ServiceSIModel.DocumentStatus = doc_status;
                            if (doc_status == "C")
                            {
                                _ServiceSIModel.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                _ServiceSIModel.Cancelled = true;
                                //Session["BtnName"] = "Refresh";
                                _ServiceSIModel.BtnName = "Refresh";
                            }
                            else
                            {
                                _ServiceSIModel.Cancelled = false;
                            }
                            _ServiceSIModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _ServiceSIModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                           
                            if (doc_status != "D" && doc_status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[6];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _ServiceSIModel.Command != "Edit")
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
                                        //Session["BtnName"] = "Refresh";
                                        _ServiceSIModel.BtnName = "Refresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServiceSIModel.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServiceSIModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServiceSIModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServiceSIModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (doc_status == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServiceSIModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServiceSIModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (doc_status == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServiceSIModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "Refresh";
                                        _ServiceSIModel.BtnName = "Refresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            //ViewBag.MenuPageName = getDocumentName();
                            _ServiceSIModel.Title = title;
                            _ServiceSIModel.DocumentMenuId = DocumentMenuId;

                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.ItemDetails = ds.Tables[1];
                            //ViewBag.VBRoleList = GetRoleList();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                        }
                    }
                    else
                    {
                        _ServiceSIModel.SourceType = "D";
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceDetail.cshtml", _ServiceSIModel);
                }
                else
                {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
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
                    ServiceSIModel _ServiceSIModel1 = new ServiceSIModel();
                    CommonPageDetails();
                    _ServiceSIModel1.GstApplicable = ViewBag.GstApplicable;
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    //Session["SSISearch"] = null;
                    _ServiceSIModel1.SSISearch = null;
                    if (URLModel.Sinv_no != null || URLModel.Sinv_dt != null)
                    {
                        _ServiceSIModel1.Sinv_no = URLModel.Sinv_no;
                        _ServiceSIModel1.Sinv_dt = URLModel.Sinv_dt;
                    }
                    if (URLModel.TransType != null)
                    {
                        _ServiceSIModel1.TransType = URLModel.TransType;
                    }
                    else
                    {
                        _ServiceSIModel1.TransType = "New";
                    }
                    if (URLModel.BtnName != null)
                    {
                        _ServiceSIModel1.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        _ServiceSIModel1.BtnName = "BtnRefresh";
                    }
                    if (URLModel.Command != null)
                    {
                        _ServiceSIModel1.Command = URLModel.Command;
                    }
                    else
                    {
                        _ServiceSIModel1.Command = "Refresh";
                    }
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.RateDigit = RateDigit;
                    ViewBag.QtyDigit = QtyDigit;

                    List<CustomerName> _CustList = new List<CustomerName>();

                    _CustList.Insert(0, new CustomerName() { Cust_id = "0", Cust_name = "---Select---" });
                    _ServiceSIModel1.CustomerNameList = _CustList;
                    _ServiceSIModel1.SO_SourceType = "D";
                    List<SrcDocNo> _SrcDocNo = new List<SrcDocNo>();
                    
                    _SrcDocNo.Insert(0, new SrcDocNo() { Src_DocNo = "0", SrcDocNoVal = "---Select---" });
                    _ServiceSIModel1.SrcdocnoList = _SrcDocNo;
                    GetSalesPersonList(_ServiceSIModel1);

                    _ServiceSIModel1.Title = title;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ServiceSIModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                    {
                        _ServiceSIModel1.WF_status1 = TempData["WF_status"].ToString();
                    }
                    //if (Session["Inv_No"] != null && Session["Inv_Date"] != null)
                    if (_ServiceSIModel1.Sinv_no != null && _ServiceSIModel1.Sinv_dt != null)
                    {
                        string Doc_no = _ServiceSIModel1.Sinv_no;
                        string Doc_date = _ServiceSIModel1.Sinv_dt;
                        //string Doc_no = Session["Inv_No"].ToString();
                        //string Doc_date = Session["Inv_Date"].ToString();

                        DataSet ds = GetServiceSaleInvoiceEdit(Doc_no, Doc_date);


                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            ViewBag.AttechmentDetails = ds.Tables[8];
                            ViewBag.GLAccount = ds.Tables[7];
                            ViewBag.GLTOtal = ds.Tables[9];

                            //ViewBag.TaxDetail = ds.Tables[2];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            //ViewBag.OCDetail = ds.Tables[3];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.TotalDetails = ds.Tables[0];
                            //ViewBag.TaxDetail2 = ds.Tables[10];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OCTaxDetails = ds.Tables[11];
                            ViewBag.CostCenterData = ds.Tables[12];
                            ViewBag.ItemTDSDetails = ds.Tables[13];
                            ViewBag.ItemOC_TDSDetails = ds.Tables[14];
                            ViewBag.PaymentScheduleData = ds.Tables[15];
                            _ServiceSIModel1.Sinv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _ServiceSIModel1.Sinv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                           
                            _ServiceSIModel1.CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();
                            _ServiceSIModel1.CustID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                            _CustList.Add(new CustomerName { Cust_id = _ServiceSIModel1.CustID, Cust_name = _ServiceSIModel1.CustName });
                            _ServiceSIModel1.CustomerNameList = _CustList;
                            _ServiceSIModel1.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _ServiceSIModel1.SrcDocDate = ds.Tables[0].Rows[0]["src_doc_date"].ToString();
                            _SrcDocNo.Insert(0, new SrcDocNo() { Src_DocNo = _ServiceSIModel1.SrcDocNo, SrcDocNoVal = _ServiceSIModel1.SrcDocNo });
                            _ServiceSIModel1.SrcdocnoList = _SrcDocNo;
                            _ServiceSIModel1.SO_SourceType = ds.Tables[0].Rows[0]["src_type"].ToString().Trim();
                            _ServiceSIModel1.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _ServiceSIModel1.SourceDocDate = ds.Tables[0].Rows[0]["src_doc_date"].ToString();
                            _ServiceSIModel1.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _ServiceSIModel1.Create_by = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _ServiceSIModel1.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _ServiceSIModel1.Approved_by = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _ServiceSIModel1.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _ServiceSIModel1.Amended_by = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _ServiceSIModel1.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _ServiceSIModel1.StatusName = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                            _ServiceSIModel1.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                            _ServiceSIModel1.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                            _ServiceSIModel1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _ServiceSIModel1.Address = ds.Tables[0].Rows[0]["CustAddress"].ToString();
                            _ServiceSIModel1.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _ServiceSIModel1.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _ServiceSIModel1.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _ServiceSIModel1.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _ServiceSIModel1.SSIStatus = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            _ServiceSIModel1.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            _ServiceSIModel1.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            _ServiceSIModel1.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                            _ServiceSIModel1.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                            _ServiceSIModel1.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            _ServiceSIModel1.bs_curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            _ServiceSIModel1.ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _ServiceSIModel1.Currency = ds.Tables[0].Rows[0]["curr_Name"].ToString();
                            _ServiceSIModel1.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                            _ServiceSIModel1.RefDoc_No = ds.Tables[0].Rows[0]["ref_doc_no"].ToString();
                            _ServiceSIModel1.RefDoc_Dt = ds.Tables[0].Rows[0]["ref_doc_dt"].ToString();
                            _ServiceSIModel1.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                            _ServiceSIModel1.slprsn_id = ds.Tables[0].Rows[0]["sls_per"].ToString();
                            if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            {
                                _ServiceSIModel1.Hd_GstType = "Both";
                            }
                            else
                            {
                                _ServiceSIModel1.Hd_GstType = "IGST";
                            }
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                            string doc_status = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            _ServiceSIModel1.DocumentStatus = doc_status;
                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                if (doc_status == "A" || doc_status == "C")
                                {
                                    _ServiceSIModel1.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                                }
                                _ServiceSIModel1.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                                _ServiceSIModel1.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                                ViewBag.GLVoucherNo = _ServiceSIModel1.GLVoucherNo;/*add by Hina Sharma on 11-08-2025*/
                            }
                            _ServiceSIModel1.ddlCustome_Reference = ds.Tables[0].Rows[0]["cust_ref"].ToString();
                            _ServiceSIModel1.ddlPayment_term = ds.Tables[0].Rows[0]["pay_term"].ToString();
                            _ServiceSIModel1.ddlDelivery_term = ds.Tables[0].Rows[0]["deli_term"].ToString();
                            _ServiceSIModel1.Declaration_1 = ds.Tables[0].Rows[0]["declar_1"].ToString();
                            _ServiceSIModel1.Declaration_2 = ds.Tables[0].Rows[0]["declar_2"].ToString();
                            _ServiceSIModel1.Invoice_Heading = ds.Tables[0].Rows[0]["inv_heading"].ToString();
                            _ServiceSIModel1.Ship_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                            _ServiceSIModel1.ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                            _ServiceSIModel1.ShipTo = ds.Tables[0].Rows[0]["ship_to"].ToString();
                            _ServiceSIModel1.txt_PlcOfSupply = ds.Tables[0].Rows[0]["placeofsupply"].ToString();
                            string nontaxable = ds.Tables[0].Rows[0]["non_taxable"].ToString();
                            if (nontaxable == "Y")
                            {
                                _ServiceSIModel1.nontaxable = true;
                            }
                            else
                            {
                                _ServiceSIModel1.nontaxable = false;
                            }
                            string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                            _ServiceSIModel1.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                            if (roundoff_status == "Y")
                            {
                                _ServiceSIModel1.RoundOffFlag = true;
                            }
                            else
                            {
                                _ServiceSIModel1.RoundOffFlag = false;
                            }
                            ViewBag.DocumentStatus = doc_status;
                            //Session["DocumentStatus"] = doc_status;
                            _ServiceSIModel1.DocumentStatus = doc_status;
                            if (doc_status == "C")
                            {
                                _ServiceSIModel1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                _ServiceSIModel1.Cancelled = true;
                                //Session["BtnName"] = "Refresh";
                                _ServiceSIModel1.BtnName = "Refresh";
                            }
                            else
                            {
                                _ServiceSIModel1.Cancelled = false;
                            }
                            _ServiceSIModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _ServiceSIModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                           
                            if (doc_status != "D" && doc_status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[6];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _ServiceSIModel1.Command != "Edit")
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
                                        //Session["BtnName"] = "Refresh";
                                        _ServiceSIModel1.BtnName = "Refresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServiceSIModel1.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServiceSIModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServiceSIModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServiceSIModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (doc_status == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServiceSIModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServiceSIModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (doc_status == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServiceSIModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "Refresh";
                                        _ServiceSIModel1.BtnName = "Refresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            //ViewBag.MenuPageName = getDocumentName();
                            _ServiceSIModel1.Title = title;
                            _ServiceSIModel1.DocumentMenuId = DocumentMenuId;

                            ViewBag.ItemDetails = ds.Tables[1];
                            //ViewBag.VBRoleList = GetRoleList();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                        }
                    }
                    else
                    {
                        _ServiceSIModel1.SourceType = "D";
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceDetail.cshtml", _ServiceSIModel1);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetInvoiceList(string docid, string status)
        {
            ServiceSIListModel _SSIListModel = new ServiceSIListModel();
            _SSIListModel.WF_status = status;
            //Session["WF_Docid"] = docid;
            return RedirectToAction("ServiceSaleInvoice", "ServiceSaleInvoice", _SSIListModel);
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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
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
        public ActionResult GetAutoCompleteSearchCustList(ServiceSIListModel _ServiceSIListModel)
        {
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string CustType = string.Empty;
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
                if (string.IsNullOrEmpty(_ServiceSIListModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _ServiceSIListModel.CustName;
                }
                CustType = "D";

                CustList = _ServiceSI_ISERVICE.GetCustomerList(Comp_ID, CustomerName, Br_ID,CustType);
                List<CustomerName> _CustmrList = new List<CustomerName>();
                foreach (var data in CustList)
                {
                    CustomerName _SuppDetail = new CustomerName();
                    _SuppDetail.Cust_id = data.Key;
                    _SuppDetail.Cust_name = data.Value;
                    _CustmrList.Add(_SuppDetail);
                }
                _ServiceSIListModel.CustomerNameList = _CustmrList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCustAddrDetail(string Cust_id,string DocumentMenuId)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                DataSet result = _ServiceSI_ISERVICE.GetCustAddrDetailDL(Cust_id, Comp_ID, br_id, DocumentMenuId);
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



        public ActionResult DoubleClickOnList(string DocNo, string DocDate, string ListFilterData, string WF_status)
        {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (Session["UserId"] != null)
            {
                UserID = Session["UserId"].ToString();
            }
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            URLModelDetails URLModel = new URLModelDetails();
            ServiceSIModel _ServiceSIModel = new ServiceSIModel();
            _ServiceSIModel.Message = "New";
            _ServiceSIModel.Command = "Update";
            _ServiceSIModel.TransType = "Update";
            _ServiceSIModel.BtnName = "BtnToDetailPage";
            _ServiceSIModel.Sinv_no = DocNo;
            _ServiceSIModel.Sinv_dt = DocDate;
            _ServiceSIModel.UserID = UserID;
            TempData["WF_status"] = WF_status;
            TempData["ListFilterData"] = ListFilterData;
            TempData["ModelData"] = _ServiceSIModel;
            URLModel.Sinv_no = DocNo;
            URLModel.Sinv_dt = DocDate;
            URLModel.TransType = "Update";
            URLModel.Command = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["TransType"] = "Update";
            //Session["BtnName"] = "BtnToDetailPage";
            //Session["Inv_No"] = DocNo;
            //Session["Inv_Date"] = DocDate;
            return RedirectToAction("ServiceSaleInvoiceDetail", "ServiceSaleInvoice", URLModel);
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType,string Mailerror)
        {
            //Session["Message"] = "";
            ServiceSIModel _ServiceSIModel = new ServiceSIModel();
            var a = TrancType.Split(',');
            _ServiceSIModel.Sinv_no = a[0].Trim();
            _ServiceSIModel.Sinv_dt = a[1].Trim();
            _ServiceSIModel.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            //_SPODetailModel.WF_status1 = WF_status1;
            _ServiceSIModel.BtnName = "BtnToDetailPage";
            _ServiceSIModel.Message =  Mailerror;
            URLModelDetails URLModel = new URLModelDetails();
            URLModel.Sinv_no = a[0].Trim();
            URLModel.Sinv_dt = a[01].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _ServiceSIModel;
            TempData["WF_status"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ServiceSaleInvoiceDetail", "ServiceSaleInvoice", URLModel);
        }
        [HttpPost]
        public JsonResult GetServiceVerifcationList(string Supp_id)
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
                DataSet result = _ServiceSI_ISERVICE.GetServiceVerifcationList(Supp_id, Comp_ID, Br_ID);

                DataRow Drow = result.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";

                result.Tables[0].Rows.InsertAt(Drow, 0);
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
        public JsonResult GetServiceVerifcationDetails(string VerNo, string VerDate)
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
                DataSet result = _ServiceSI_ISERVICE.GetServiceVerifcationDetail(VerNo, VerDate, Comp_ID, Br_ID);

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
        [ValidateAntiForgeryToken]
        public ActionResult ActionSSIDeatils(ServiceSIModel _ServiceSIModel, string command)
        {
            try
            {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ServiceSIModel.Delete == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        ServiceSIModel _ServiceSIModelAddNew = new ServiceSIModel();
                        _ServiceSIModelAddNew.AppStatus = "D";
                        _ServiceSIModelAddNew.BtnName = "BtnAddNew";
                        _ServiceSIModelAddNew.TransType = "Save";
                        _ServiceSIModelAddNew.Command = "New";
                        ViewBag.DocumentStatus = "D";
                        TempData["ModelData"] = _ServiceSIModelAddNew;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_ServiceSIModel.Sinv_no))
                            {
                                return RedirectToAction("DoubleClickOnList", new { DocNo = _ServiceSIModel.Sinv_no, DocDate = _ServiceSIModel.Sinv_dt, ListFilterData = _ServiceSIModel.ListFilterData1, WF_status = _ServiceSIModel.WFStatus });
                            }
                            else
                            {
                                _ServiceSIModelAddNew.Command = "Refresh";
                                _ServiceSIModelAddNew.TransType = "Refresh";
                                _ServiceSIModelAddNew.BtnName = "Refresh";
                                _ServiceSIModelAddNew.DocumentStatus = null;
                                TempData["ModelData"] = _ServiceSIModelAddNew;
                                return RedirectToAction("ServiceSaleInvoiceDetail", "ServiceSaleInvoice");
                            }
                        }
                       /*End to chk Financial year exist or not*/
                        return RedirectToAction("ServiceSaleInvoiceDetail", "ServiceSaleInvoice");

                    case "Edit":
                        URLModelDetails URLModelEdit = new URLModelDetails();
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ServiceSIModel.Sinv_no, DocDate = _ServiceSIModel.Sinv_dt, ListFilterData = _ServiceSIModel.ListFilterData1, WF_status = _ServiceSIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SSInvDate = _ServiceSIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SSInvDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ServiceSIModel.Sinv_no, DocDate = _ServiceSIModel.Sinv_dt, ListFilterData = _ServiceSIModel.ListFilterData1, WF_status = _ServiceSIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        if (_ServiceSIModel.SSIStatus == "A")
                        {
                            string checkforCancle = CheckPIForCancellationinVoucher(_ServiceSIModel.Sinv_no, _ServiceSIModel.Sinv_dt);
                            if (checkforCancle != "")
                            {
                                //Session["Message"] = checkforCancle;
                                _ServiceSIModel.Message = checkforCancle;
                                _ServiceSIModel.BtnName = "BtnToDetailPage";
                                URLModelEdit.Command = "Refresh";
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.Sinv_no = _ServiceSIModel.Sinv_no;
                                URLModelEdit.Sinv_dt = _ServiceSIModel.Sinv_dt;
                                TempData["ModelData"] = _ServiceSIModel;
                                TempData["FilterData"] = _ServiceSIModel.ListFilterData1;
                            }
                            else
                            {
                                _ServiceSIModel.TransType = "Update";
                                _ServiceSIModel.Command = command;
                                _ServiceSIModel.BtnName = "BtnEdit";
                                TempData["ModelData"] = _ServiceSIModel;
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.Command = command;
                                URLModelEdit.BtnName = "BtnEdit";
                                URLModelEdit.Sinv_no = _ServiceSIModel.Sinv_no;
                                URLModelEdit.Sinv_dt = _ServiceSIModel.Sinv_dt;
                                TempData["ListFilterData"] = _ServiceSIModel.ListFilterData1;
                            }
                        }
                        else
                        {
                            _ServiceSIModel.TransType = "Update";
                            _ServiceSIModel.Command = command;
                            _ServiceSIModel.BtnName = "BtnEdit";
                            TempData["ModelData"] = _ServiceSIModel;
                            URLModelEdit.TransType = "Update";
                            URLModelEdit.Command = command;
                            URLModelEdit.BtnName = "BtnEdit";
                            URLModelEdit.Sinv_no = _ServiceSIModel.Sinv_no;
                            URLModelEdit.Sinv_dt = _ServiceSIModel.Sinv_dt;
                            TempData["ListFilterData"] = _ServiceSIModel.ListFilterData1;
                        }
                        return RedirectToAction("ServiceSaleInvoiceDetail", URLModelEdit);
                    case "Delete":
                        ServiceSIModel _ServiceSIModelDelete = new ServiceSIModel();
                        _ServiceSIModel.Command = command;
                        _ServiceSIModel.BtnName = "Refresh";
                        //Inv_No = _ServiceSIModel.Sinv_no;
                        ServicePIDelete(_ServiceSIModel, command, _ServiceSIModel.Title);
                        _ServiceSIModelDelete.Message = "Deleted";
                        _ServiceSIModelDelete.Command = "Refresh";
                        _ServiceSIModelDelete.TransType = "Refresh";
                        _ServiceSIModelDelete.AppStatus = "DL";
                        _ServiceSIModelDelete.BtnName = "BtnDelete";
                        _ServiceSIModelDelete.SourceType = "D";
                        TempData["ModelData"] = _ServiceSIModelDelete;
                        TempData["ListFilterData"] = _ServiceSIModel.ListFilterData1;
                        return RedirectToAction("ServiceSaleInvoiceDetail");

                    case "Save":
                        //Session["Command"] = command;
                        _ServiceSIModel.Command = command;
                        if (_ServiceSIModel.TransType == null)
                        {
                            _ServiceSIModel.TransType = command;
                        }
                        string checkforCancle_onSave = CheckPIForCancellationinVoucher(_ServiceSIModel.Sinv_no, _ServiceSIModel.Sinv_dt);
                        if (checkforCancle_onSave != "")
                        {
                            //Session["Message"] = checkforCancle;
                            _ServiceSIModel.Message = checkforCancle_onSave;
                            _ServiceSIModel.BtnName = "BtnToDetailPage";
                            //URLModelEdit.Command = "Refresh";
                            //URLModelEdit.TransType = "Update";
                            //URLModelEdit.Sinv_no = _ServiceSIModel.Sinv_no;
                            //URLModelEdit.Sinv_dt = _ServiceSIModel.Sinv_dt;
                            TempData["ModelData"] = _ServiceSIModel;
                            TempData["FilterData"] = _ServiceSIModel.ListFilterData1;
                        }
                        else
                        {
                            SaveServiceSaleInvoice(_ServiceSIModel);
                        }
                        
                        if (_ServiceSIModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_ServiceSIModel.Message == "DocModify")
                        {
                            if (Session["UserId"] != null)
                            {
                                UserID = Session["UserId"].ToString();
                            }
                            if (Session["crm"] != null)
                            {
                                crm = Session["crm"].ToString();
                            }
                            if (UserID == "1001")
                            {
                                crm = "Y";
                            }
                            ViewBag.crm = crm;
                            DocumentMenuId = _ServiceSIModel.DocumentMenuId;
                            CommonPageDetails();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";
                            
                            List<CustomerName> suppLists = new List<CustomerName>();
                            suppLists.Add(new CustomerName { Cust_id = _ServiceSIModel.CustID, Cust_name = _ServiceSIModel.CustName });
                            _ServiceSIModel.CustomerNameList = suppLists;

                            List<SourceDoc> _DocumentNumberList = new List<SourceDoc>();
                            SourceDoc _DocumentNumber = new SourceDoc();
                            _DocumentNumber.doc_no = _ServiceSIModel.SrcDocNo;
                            _DocumentNumber.doc_dt = "0";
                            _DocumentNumberList.Add(_DocumentNumber);
                            _ServiceSIModel.SourceDocList = _DocumentNumberList;

                            List<CurrancyList> currancyLists = new List<CurrancyList>();
                            currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                            _ServiceSIModel.currancyLists = currancyLists;
                            _ServiceSIModel.Title = title;


                            _ServiceSIModel.Sinv_dt = DateTime.Now.ToString();
                            _ServiceSIModel.bill_no = _ServiceSIModel.bill_no;
                            _ServiceSIModel.bill_date = _ServiceSIModel.bill_date;
                            _ServiceSIModel.CustName = _ServiceSIModel.CustName;
                            _ServiceSIModel.Address = _ServiceSIModel.Address;
                            _ServiceSIModel.SrcDocNo = _ServiceSIModel.SrcDocNo;
                            _ServiceSIModel.SrcDocDate = _ServiceSIModel.SrcDocDate;

                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];
                            ViewBag.ItemTaxDetailsList = ViewData["SRSITaxDetails"];
                            ViewBag.OCTaxDetails = ViewData["OCTaxDetails"];
                            ViewBag.ItemOC_TDSDetails = ViewData["OCTDSDetails"];
                            ViewBag.GLAccount = ViewData["VouDetail"];
                            ViewBag.ItemTDSDetails = ViewData["TDSDetails"];
                            ViewBag.CostCenterData = ViewData["CCdetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _ServiceSIModel.BtnName = "Refresh";
                            _ServiceSIModel.Command = "Refresh";
                            _ServiceSIModel.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            _ServiceSIModel.GstApplicable = ViewBag.GstApplicable;
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceDetail.cshtml", _ServiceSIModel);

                        }
                        else
                        {
                            TempData["ModelData"] = _ServiceSIModel;
                            URLModelDetails URLModel = new URLModelDetails();
                            URLModel.BtnName = "BtnSave";
                            URLModel.Command = command;
                            URLModel.TransType = "Update";
                            URLModel.Sinv_no = _ServiceSIModel.Sinv_no;
                            URLModel.Sinv_dt = _ServiceSIModel.Sinv_dt;
                            TempData["ListFilterData"] = _ServiceSIModel.ListFilterData1;
                            return RedirectToAction("ServiceSaleInvoiceDetail", URLModel);
                        }
                    case "Forward":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ServiceSIModel.Sinv_no, DocDate = _ServiceSIModel.Sinv_dt, ListFilterData = _ServiceSIModel.ListFilterData1, WF_status = _ServiceSIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SSInvDate1 = _ServiceSIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SSInvDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ServiceSIModel.Sinv_no, DocDate = _ServiceSIModel.Sinv_dt, ListFilterData = _ServiceSIModel.ListFilterData1, WF_status = _ServiceSIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ServiceSIModel.Sinv_no, DocDate = _ServiceSIModel.Sinv_dt, ListFilterData = _ServiceSIModel.ListFilterData1, WF_status = _ServiceSIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SSInvDate2 = _ServiceSIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SSInvDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ServiceSIModel.Sinv_no, DocDate = _ServiceSIModel.Sinv_dt, ListFilterData = _ServiceSIModel.ListFilterData1, WF_status = _ServiceSIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _ServiceSIModel.Command = command;
                        ServiceSaleInvoiceApprove(_ServiceSIModel, _ServiceSIModel.PV_Narration, _ServiceSIModel.BP_Narration, _ServiceSIModel.DN_Narration);
                        URLModelDetails URLModelApprove = new URLModelDetails();
                        URLModelApprove.BtnName = _ServiceSIModel.BtnName;
                        URLModelApprove.Command = command;
                        URLModelApprove.TransType = _ServiceSIModel.TransType;
                        URLModelApprove.Sinv_no = _ServiceSIModel.Sinv_no;
                        URLModelApprove.Sinv_dt = _ServiceSIModel.Sinv_dt;
                        TempData["ModelData"] = _ServiceSIModel;
                        TempData["ListFilterData"] = _ServiceSIModel.ListFilterData1;
                        return RedirectToAction("ServiceSaleInvoiceDetail", URLModelApprove);

                    case "Refresh":
                        ServiceSIModel _ServiceSIModelRefresh = new ServiceSIModel();
                        _ServiceSIModelRefresh.BtnName = "Refresh";
                        _ServiceSIModelRefresh.Command = command;
                        _ServiceSIModelRefresh.TransType = "Save";
                        _ServiceSIModelRefresh.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                        TempData["ModelData"] = _ServiceSIModelRefresh;
                        TempData["ListFilterData"] = _ServiceSIModel.ListFilterData1;
                        return RedirectToAction("ServiceSaleInvoiceDetail");

                    case "Print":
                       
                        return GenratePdfFile(_ServiceSIModel);
                    case "BacktoList":

                        //ServiceSIListModel _SSIListModel = new ServiceSIListModel();
                        //_SSIListModel.WF_status = _ServiceSIModel.WF_status1;
                        //TempData["ListFilterData"] = _ServiceSIModel.ListFilterData1;
                        var WF_Status = _ServiceSIModel.WF_status1;
                        if (_ServiceSIModel.ListFilterData1 == "undefined")
                        {
                            TempData["ListFilterData"] = null;
                        }
                        else
                        {
                            TempData["ListFilterData"] = _ServiceSIModel.ListFilterData1;
                        }

                        return RedirectToAction("ServiceSaleInvoice", "ServiceSaleInvoice", new { WF_Status });

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
        public FileResult GenratePdfFile(ServiceSIModel _model)
        {
            PrintOptionsList ProntOption = new PrintOptionsList();
            if (_model.HdnPrintOptons == "Y")
            {
                ProntOption.PrtOpt_catlog_number = _model.PrtOpt_catlog_number == "Y" ? true : false;
                ProntOption.PrtOpt_item_code = _model.PrtOpt_item_code == "Y" ? true : false;
                ProntOption.PrtOpt_item_desc = _model.PrtOpt_item_desc == "Y" ? true : false;
            }
            else
            {
                ProntOption.PrtOpt_catlog_number = true;
                ProntOption.PrtOpt_item_code = true;
                ProntOption.PrtOpt_item_desc = true;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("PrintRemarks", typeof(string));/*Add by Hina on 25-09-2024*/
            dt.Columns.Add("ShowWithoutSybbol", typeof(string));
            dt.Columns.Add("showDeclare1", typeof(string));
            dt.Columns.Add("showDeclare2", typeof(string));
            dt.Columns.Add("showInvHeading", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("NumberOfCopy", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));

            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = _model.PrintFormat;
            dtr["ShowProdDesc"] = _model.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
            dtr["PrintRemarks"] = _model.PrintRemarks;/*Add by Hina on 25-09-2024*/
            dtr["ShowWithoutSybbol"] = _model.ShowWithoutSybbol;
            dtr["showDeclare1"] = _model.showDeclare1;
            dtr["showDeclare2"] = _model.showDeclare2;
            dtr["showInvHeading"] = _model.showInvHeading;
            dtr["CustAliasName"] = _model.CustomerAliasName;
            dtr["NumberOfCopy"] = _model.NumberofCopy;
            dtr["ShowTotalQty"] = _model.ShowTotalQty;
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;           
            //(_model.GstApplicable == "Y")
                return File(GetPdfDataOfGstInv(dt, DocumentMenuId, _model.Sinv_no, _model.Sinv_dt, _model.NumberofCopy, ProntOption), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
            //else
            //    return File(GetPdfData(_model.DocumentMenuId, _model.Sinv_no, _model.Sinv_dt, ProntOption), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        }
        public byte[] GetPdfDataOfGstInv(DataTable dt, string docId, string invNo, string invDt, int NumberofCopy, PrintOptionsList ProntOption)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
             //   string inv_type = "";
                string ReportType = "common";
                //inv_type = "SSI";
                DataSet Details = _ServiceSI_ISERVICE.GetSlsInvGstDtlForPrint(CompID, Br_ID, invNo, invDt);
                ViewBag.PageName = "SSI";
                string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();

                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.ProntOption = ProntOption;              
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
                string htmlcontent = "";
               // ViewBag.Title = "Tax Invoice";
               // if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
               // {
               //     htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceWithGSTPrintF2.cshtml"));
               // }
               // else
               // {
               //     htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceWithGSTPrint.cshtml"));
               //}
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    if (ReportType == "common")
                    {                     
                            pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 60f);                        
                    }
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    Int32 noofprint = NumberofCopy;/*Add code by Hina sharma on 01-10-2024 to show multiple print copy*/
                    for (int i = 1; i <= noofprint; i++)
                    {

                        if (noofprint > 1)
                        {
                            ViewBag.Copyno = i;
                        }
                        else
                        {
                            ViewBag.Copyno = null;
                        }
                        ViewBag.Title = "Tax Invoice";
                        if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
                        {
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceWithGSTPrintF2.cshtml"));
                        }
                        else
                        {
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceWithGSTPrint.cshtml"));
                        }
                        reader = new StringReader(htmlcontent);

                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                        pdfDoc.NewPage();
                    }
                    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = GSTHeaderFooterPagination(bytes, Details, ReportType, NumberofCopy);
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
        private Byte[] GSTHeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType, int NumberofCopy)
        {
            string Br_ID = string.Empty;
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            var docstatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
            var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.NORMAL);
            Font font1 = new Font(bf, 8, Font.NORMAL);
            Font fontb = new Font(bf, 9, Font.NORMAL);
            Font font2 = new Font(bf, 6, Font.NORMAL);
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 15, Font.BOLD);
            fonttitle.SetStyle("underline");
            //string logo = ConfigurationManager.AppSettings["LocalServerURL"].ToString() + Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());
            string State_Name = Details.Tables[5].Rows[0]["state_name"].ToString();
            String StateName = (State_Name).ToUpper();
            ViewBag.QrCode = QR;
            string draftImage = "";
            if (docstatus == "D" || docstatus == "F")
            {
                 draftImage = Server.MapPath("~/Content/Images/draft.png");
            }
            else if (docstatus == "C")
            {
                 draftImage = Server.MapPath("~/Content/Images/cancelled.png");
            }
            else
            {
                draftImage = Server.MapPath("~/Content/Images/draft.png");
            }
            string bnetImage = Server.MapPath("~/images/businet.png");

            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        if (ReportType == "common")
                        {
                            var draftimg = Image.GetInstance(draftImage);
                            draftimg.SetAbsolutePosition(20, 40);
                            draftimg.ScaleAbsolute(650f, 600f);

                            var qrCode = Image.GetInstance(QR);
                            qrCode.SetAbsolutePosition(475, 710);
                            qrCode.ScaleAbsolute(100f, 95f);

                            int PageCount = reader1.NumberOfPages;
                            var PageCount1 = reader1.NumberOfPages / NumberofCopy;

                            int count = 0;
                            for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                if (docstatus == "D" || docstatus == "F" || docstatus == "C")
                                {
                                    content.AddImage(draftimg);
                                }
                              
                                //try
                                //{
                                //    var image = Image.GetInstance(logo);
                                //    image.SetAbsolutePosition(30, 540);
                                //    image.ScaleAbsolute(90f, 25f);
                                //    if (i == 1)
                                //        content.AddImage(image);
                                //}
                                //catch { }
                                //if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["inv_qr_code"].ToString()))
                                //{
                                //    if (i == 1)
                                //        content.AddImage(qrCode);
                                //}
                                //Phrase ptitle = new Phrase(String.Format("Tax Invoice", i, PageCount), fonttitle);
                                if (PageCount1 > count)//add by shubham maurya on 17-04-2025 
                                    count = count + 1;
                                else
                                    count = 1;
                                if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["inv_qr_code"].ToString()))
                                {
                                    if (count == 1)
                                        content.AddImage(qrCode);
                                }
                                Phrase p = new Phrase(String.Format("Page {0} of {1}", count, PageCount1), font);
                                    if (count == PageCount1)
                                    {
                                        //try
                                        //{
                                        //    var bnetlogo = Image.GetInstance(bnetImage);
                                        //    bnetlogo.SetAbsolutePosition(322, 38);
                                        //    bnetlogo.ScaleAbsolute(40f, 10f);
                                        //    content.AddImage(bnetlogo);

                                        //}
                                        //catch { }
                                        //Phrase ftr = new Phrase(String.Format("This Document is generated by ", i, PageCount), font);
                                        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, ftr, 320, 40, 0);
                                        Phrase ftr1 = new Phrase(String.Format("SUBJECT TO " + StateName + " JURISDICTION ", i, PageCount), font2);
                                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ftr1, 298, 40, 0);
                                        // 500,560 is for left right alignment of text and 40,60 is for above or below text of align.

                                    }
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, p, 560, 15, 0);
                                    //if(i == 1)
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 400, 550, 0);
                                }
                            
                           
                        }

                    }
                    bytes = ms.ToArray();
                }
            }
            return bytes;
        }
        private string GenerateQRCode(string qrcodeText)
        {
            if (string.IsNullOrEmpty(qrcodeText))
                qrcodeText = "N/A";
            string path = Server.MapPath("~");
            string fileName = "QR_" + Guid.NewGuid().ToString();
            string folderPath = path + ("..\\LogsFile\\EmailAlertPDFs\\");
            string imagePath = folderPath + fileName + ".jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = imagePath;
            var barcodeBitmap = new System.Drawing.Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;
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
                /*Some changes in this proc due to third party tds on OC for auto generated DN by Hina on 08-07-2024  */
                DataSet Deatils = _ServiceSI_ISERVICE.CheckSSIDetail(Comp_ID, Br_ID, DocNo, DocDate);

                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = "InvoicecannotModifiedPaymentCreated";
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
        public ActionResult SaveServiceSaleInvoice(ServiceSIModel _ServiceSIModel)
        {
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = _ServiceSIModel.Title.Replace(" ", "");

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
            try
            {
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCTaxDetail = new DataTable();
                DataTable DtblIOCDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable DtblVouDetail = new DataTable();
                DataTable CRCostCenterDetails = new DataTable();
                DataTable DtblOCTdsDetail = new DataTable();
                DataTable DTPaymentSchedule = new DataTable();

                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("MenuID", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(string));
                dtheader.Columns.Add("br_id", typeof(string));
                dtheader.Columns.Add("inv_no", typeof(string));
                dtheader.Columns.Add("inv_dt", typeof(string));
                dtheader.Columns.Add("cust_id", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(string));
                dtheader.Columns.Add("remarks", typeof(string));
                dtheader.Columns.Add("inv_status", typeof(string));
                dtheader.Columns.Add("UserID", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("tax_amt", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val_bs", typeof(string));
                dtheader.Columns.Add("Narration", typeof(string));
                dtheader.Columns.Add("RefDocNo", typeof(string));
                dtheader.Columns.Add("RefDocDt", typeof(string));
                dtheader.Columns.Add("cancel_remarks", typeof(string));
                dtheader.Columns.Add("src_type", typeof(string));
                dtheader.Columns.Add("src_doc_number", typeof(string));
                dtheader.Columns.Add("src_doc_date", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _ServiceSIModel.TransType;
                dtrowHeader["MenuID"] = DocumentMenuId;
                string cancelflag = _ServiceSIModel.Cancelled.ToString();
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
                dtrowHeader["inv_no"] = _ServiceSIModel.Sinv_no;
                dtrowHeader["inv_dt"] = _ServiceSIModel.Sinv_dt;
                dtrowHeader["cust_id"] = _ServiceSIModel.CustID;
                dtrowHeader["bill_add_id"] = _ServiceSIModel.bill_add_id == null ? "0" : _ServiceSIModel.bill_add_id;
                dtrowHeader["remarks"] = _ServiceSIModel.Remarks;
                dtrowHeader["inv_status"] = IsNull(_ServiceSIModel.AppStatus, "D");
                dtrowHeader["UserID"] = Session["UserId"].ToString();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["gr_val"] = _ServiceSIModel.GrossValue;
                dtrowHeader["tax_amt"] = _ServiceSIModel.TaxAmount;
                dtrowHeader["oc_amt"] = _ServiceSIModel.OtherCharges;
                dtrowHeader["net_val_bs"] = _ServiceSIModel.NetAmountInBase;
                dtrowHeader["Narration"] = _ServiceSIModel.Narration;
                dtrowHeader["RefDocNo"] = _ServiceSIModel.RefDoc_No;/*Add by Hina on 06-12-2024*/
                dtrowHeader["RefDocDt"] = _ServiceSIModel.RefDoc_Dt;
                dtrowHeader["cancel_remarks"] = _ServiceSIModel.CancelledRemarks;
                dtrowHeader["src_type"] = _ServiceSIModel.SO_SourceType;
                dtrowHeader["src_doc_number"] = _ServiceSIModel.SourceDocNo;
                dtrowHeader["src_doc_date"] = _ServiceSIModel.SourceDocDate;

                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;

                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("item_id", typeof(string));
              
                dtItem.Columns.Add("inv_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                /*start code Add ItmDisPer,ItmDisAmt,DisVal by Hina sharma on 07-01-2025*/
                dtItem.Columns.Add("ItmDisPer", typeof(string));
                dtItem.Columns.Add("ItmDisAmt", typeof(string));
                dtItem.Columns.Add("DisVal", typeof(string));
                /*End code Add ItmDisPer,ItmDisAmt,DisVal by Hina sharma on 07-01-2025*/
                dtItem.Columns.Add("item_gr_val", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));/*Add TaxExempted,ManualGST by Hina sharma on 02-01-2025*/
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(string));
                dtItem.Columns.Add("item_remarks", typeof(string));
                dtItem.Columns.Add("HsnNo", typeof(string));
                dtItem.Columns.Add("order_qty", typeof(string));

                JArray jObject = JArray.Parse(_ServiceSIModel.Itemdetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();
                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["inv_qty"] = jObject[i]["inv_qty"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                    /*start code Add ItmDisPer,ItmDisAmt,DisVal by Hina sharma on 07-01-2025*/
                    dtrowLines["ItmDisPer"] = jObject[i]["ItmDisPer"].ToString();
                    dtrowLines["ItmDisAmt"] = jObject[i]["ItmDisAmt"].ToString();
                    dtrowLines["DisVal"] = jObject[i]["DisVal"].ToString();
                    /*End code Add ItmDisPer,ItmDisAmt,DisVal by Hina sharma on 07-01-2025*/
                    dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                    dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                    dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                    dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();/*Add TaxExempted,ManualGST by Hina sharma on 02-01-2025*/
                    dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                    dtrowLines["item_acc_id"] = jObject[i]["item_acc_id"].ToString();
                    dtrowLines["item_remarks"] = jObject[i]["item_remarks"].ToString();
                    dtrowLines["HsnNo"] = jObject[i]["Hsn_no"].ToString();
                    dtrowLines["order_qty"] = jObject[i]["order_qty"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemDetail = dtItem;
                ViewData["ItemDetails"] = dtitemdetail(jObject);

                DtblTaxDetail = ToDtblTaxDetail(_ServiceSIModel.ItemTaxdetails);
                ViewData["TaxDetails"] = ViewData["SRSITaxDetails"];
                DtblOCTaxDetail = ToDtblTaxDetail(_ServiceSIModel.OC_TaxDetail);
                ViewData["OCTaxDetails"] = ViewData["SRSITaxDetails"];

                DataTable OC_detail = new DataTable();
                OC_detail.Columns.Add("oc_id", typeof(int));
                OC_detail.Columns.Add("oc_val", typeof(string));
                OC_detail.Columns.Add("tax_amt", typeof(string));
                OC_detail.Columns.Add("total_amt", typeof(string));
                OC_detail.Columns.Add("curr_id", typeof(string));
                //OC_detail.Columns.Add("oc_name", typeof(string));
                //OC_detail.Columns.Add("curr_name", typeof(string));
                OC_detail.Columns.Add("conv_rate", typeof(string));
                
                //OC_detail.Columns.Add("OCValBs", typeof(string));
                
                
                OC_detail.Columns.Add("supp_id", typeof(string));
                OC_detail.Columns.Add("supp_type", typeof(string));
                OC_detail.Columns.Add("bill_no", typeof(string));
                OC_detail.Columns.Add("bill_date", typeof(string));
                OC_detail.Columns.Add("tds_amt", typeof(string)); /*Added by Hina on 05 - 07 - 2024 to chnge for tds on oc*/
                OC_detail.Columns.Add("roundoff", typeof(string));//Added by Suraj Maurya on 11-12-2024
                OC_detail.Columns.Add("pm_flag", typeof(string));//Added by Suraj Maurya on 11-12-2024
                if (_ServiceSIModel.ItemOCdetails != null)
                    {
                    JArray jObjectOC = JArray.Parse(_ServiceSIModel.ItemOCdetails);
                    for (int i = 0; i < jObjectOC.Count; i++)
                    {
                        DataRow dtrowOCDetailsLines = OC_detail.NewRow();
                        dtrowOCDetailsLines["oc_id"] = jObjectOC[i]["oc_id"];
                        dtrowOCDetailsLines["oc_val"] = jObjectOC[i]["oc_val"].ToString();
                        dtrowOCDetailsLines["tax_amt"] = jObjectOC[i]["tax_amt"].ToString();
                        dtrowOCDetailsLines["total_amt"] = jObjectOC[i]["total_amt"].ToString();
                        dtrowOCDetailsLines["curr_id"] = jObjectOC[i]["curr_id"].ToString();
                        //dtrowOCDetailsLines["oc_name"] = jObjectOC[i]["OCName"].ToString();
                        //dtrowOCDetailsLines["curr_name"] = jObjectOC[i]["OC_Curr"].ToString();
                        dtrowOCDetailsLines["conv_rate"] = jObjectOC[i]["OC_Conv"].ToString();
                        //dtrowOCDetailsLines["OCValBs"] = jObjectOC[i]["OC_AmtBs"].ToString();
                        dtrowOCDetailsLines["supp_id"] = jObjectOC[i]["supp_id"].ToString();
                        dtrowOCDetailsLines["supp_type"] = jObjectOC[i]["supp_type"].ToString();
                        dtrowOCDetailsLines["bill_no"] = jObjectOC[i]["bill_no"].ToString();
                        dtrowOCDetailsLines["bill_date"] = jObjectOC[i]["bill_date"].ToString();
                        dtrowOCDetailsLines["tds_amt"] = jObjectOC[i]["tds_amt"].ToString(); /* Added by Hina on 05 - 07 - 2024 to chnge for tds on oc */
                        dtrowOCDetailsLines["roundoff"] = jObjectOC[i]["round_off"].ToString();
                        dtrowOCDetailsLines["pm_flag"] = jObjectOC[i]["pm_flag"].ToString();

                        OC_detail.Rows.Add(dtrowOCDetailsLines);
                    }
                    ViewData["OCDetails"] = dtOCdetail(jObjectOC);
                }
                DtblIOCDetail = OC_detail;

                /*----------For Other charge Third party TDS-----------------------------*/
                DtblOCTdsDetail = ToDtblOCTdsDetail(_ServiceSIModel.oc_tds_details);
                
                
                /**----------------Cost Center Section--------------------*/
                DataTable CC_Details = new DataTable();

                CC_Details.Columns.Add("vou_sr_no", typeof(string));
                CC_Details.Columns.Add("gl_sr_no", typeof(string));
                CC_Details.Columns.Add("acc_id", typeof(string));
                CC_Details.Columns.Add("cc_id", typeof(int));
                CC_Details.Columns.Add("cc_val_id", typeof(int));
                CC_Details.Columns.Add("cc_amt", typeof(string));

                if (_ServiceSIModel.CC_DetailList != null)
                {
                    JArray JAObj = JArray.Parse(_ServiceSIModel.CC_DetailList);
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
                    ViewData["CCdetail"] = dtCCdetail(JAObj);
                }

                DataTable PaymentSchedule = new DataTable();


                PaymentSchedule.Columns.Add("sr_no", typeof(int));
                PaymentSchedule.Columns.Add("paym_mst", typeof(string));
                PaymentSchedule.Columns.Add("precentage", typeof(double));
                PaymentSchedule.Columns.Add("amt", typeof(double));

                if (_ServiceSIModel.HdnPaymentSchedule != null)
                {
                    JArray JPS = JArray.Parse(_ServiceSIModel.HdnPaymentSchedule);

                    for (int i = 0; i < JPS.Count; i++)
                    {
                        DataRow dtrowLines = PaymentSchedule.NewRow();

                        // 2. FIX: Convert values safely instead of just .ToString()
                        // This ensures "10.5" becomes a number (10.5), not a text string
                        dtrowLines["sr_no"] = Convert.ToInt32(JPS[i]["sr_no"]);
                        dtrowLines["paym_mst"] = JPS[i]["paym_mst"].ToString();
                        dtrowLines["precentage"] = Convert.ToDouble(JPS[i]["precentage"]);
                        dtrowLines["amt"] = Convert.ToDouble(JPS[i]["amt"]);

                        PaymentSchedule.Rows.Add(dtrowLines);
                    }
                }
                // Parse JSON


                DTPaymentSchedule = PaymentSchedule;
                ViewData["PaymentSchedule"] = DTPaymentSchedule;


                DataTable dtAttachment = new DataTable();
                var _ServiceSIModelattch = TempData["ModelDataattch"] as ServiceSIModelattch;
                TempData["ModelDataattch"] = null;
                if (_ServiceSIModel.attatchmentdetail != null)
                {
                    if (_ServiceSIModelattch != null)
                    {
                        if (_ServiceSIModelattch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _ServiceSIModelattch.AttachMentDetailItmStp as DataTable;
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
                        if (_ServiceSIModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _ServiceSIModel.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_ServiceSIModel.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_ServiceSIModel.Sinv_no))
                            {
                                dtrowAttachment1["id"] = _ServiceSIModel.Sinv_no;
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
                    if (_ServiceSIModel.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_ServiceSIModel.Sinv_no))
                            {
                                ItmCode = _ServiceSIModel.Sinv_no;
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
                if (_ServiceSIModel.vouDetail != null)
                {
                    JArray jObjectVOU = JArray.Parse(_ServiceSIModel.vouDetail);
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
                    ViewData["VouDetail"] = dtVoudetail(jObjectVOU);
                }
                DtblVouDetail = vou_Details;
                string Narr = "";
                string CN_Narr = "";
                if (cancelflag!= "False")
                {
                    Narr = _ServiceSIModel.PV_Narration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                    CN_Narr = _ServiceSIModel.CN_Narration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                }
                var nontax = "N";
                string nontaxable = _ServiceSIModel.nontaxable.ToString();
                if (nontaxable == "False")
                {
                    nontax = "N";
                }
                else
                {
                    nontax = "Y";
                }
                var roundof = "";
                var pm_flag = "";
                string roundoffflag = _ServiceSIModel.RoundOffFlag.ToString();
                if (roundoffflag == "False")
                {
                    roundof = "N";
                }
                else
                {
                    roundof = "Y";
                }
                pm_flag = _ServiceSIModel.pmflagval;
                SaveMessage = _ServiceSI_ISERVICE.InsertSSI_Details(DtblHDetail, DtblItemDetail, DtblTaxDetail
                    , DtblOCTaxDetail, DtblIOCDetail, DtblOCTdsDetail, DtblAttchDetail, DtblVouDetail, CRCostCenterDetails, Narr, CN_Narr, _ServiceSIModel.slprsn_id,
                    _ServiceSIModel.ddlCustome_Reference, _ServiceSIModel.ddlPayment_term, _ServiceSIModel.ddlDelivery_term, _ServiceSIModel.Declaration_1, _ServiceSIModel.Declaration_2,
                    _ServiceSIModel.Invoice_Heading, nontax, _ServiceSIModel.Ship_Add_Id, _ServiceSIModel.txt_PlcOfSupply, roundof, pm_flag, _ServiceSIModel.ShipTo, DTPaymentSchedule);
                if (SaveMessage == "DocModify")
                {
                    _ServiceSIModel.Message = "DocModify";
                    _ServiceSIModel.BtnName = "Refresh";
                    _ServiceSIModel.Command = "Refresh";
                    _ServiceSIModel.DocumentMenuId = DocumentMenuId;
                    TempData["ModelData"] = _ServiceSIModel;
                    return RedirectToAction("ServiceSaleInvoiceDetail");
                }
                else
                {
                    string[] FDetail = SaveMessage.Split(',');
                    string Message = FDetail[5].ToString();
                    string Inv_no = FDetail[0].ToString();
                    string Inv_DATE = FDetail[6].ToString();
                    string Cansal = FDetail[1].ToString();
                    if (Message == "DataNotFound")
                    {
                        var msg = "Data Not found" + " " + Inv_DATE + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _ServiceSIModel.Message = Message;
                        return RedirectToAction("ServiceSaleInvoiceDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_ServiceSIModelattch != null)
                        {
                            if (_ServiceSIModelattch.Guid != null)
                            {
                                Guid = _ServiceSIModelattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, _ServiceSIModel.TransType, DtblAttchDetail);

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
                            //string fileName = "SSI_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "TaxInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_ServiceSIModel, Inv_no, Inv_DATE,fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, Inv_no, "C", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _ServiceSIModel.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _ServiceSIModel.Message = _ServiceSIModel.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        //_ServiceSIModel.Message = "Cancelled";
                        _ServiceSIModel.Command = "Update";
                        _ServiceSIModel.Sinv_no = Inv_no;
                        _ServiceSIModel.Sinv_dt = Inv_DATE;
                        _ServiceSIModel.TransType = "Update";
                        _ServiceSIModel.AppStatus = "D";
                        _ServiceSIModel.BtnName = "Refresh";
                        //Session["Message"] = "Cancelled";
                        //Session["Command"] = "Update";
                        //Session["Inv_No"] = Inv_no;
                        //Session["Inv_Date"] = Inv_DATE;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "Refresh";
                        //Session["AttachMentDetailItmStp"] = null;
                        //Session["Guid"] = null;
                    }
                    else
                    {
                        if (Message == "Update" || Message == "Save")
                        {
                            _ServiceSIModel.Message = "Save";
                            _ServiceSIModel.Command = "Update";
                            _ServiceSIModel.Sinv_no = Inv_no;
                            _ServiceSIModel.Sinv_dt = Inv_DATE;
                            _ServiceSIModel.TransType = "Update";
                            _ServiceSIModel.AppStatus = "D";
                            _ServiceSIModel.BtnName = "BtnSave";
                            _ServiceSIModel.AttachMentDetailItmStp = null;
                            _ServiceSIModel.Guid = null;
                            // Session["Message"] = "Save";
                            // Session["Command"] = "Update";
                            // Session["Inv_No"] = Inv_no;
                            //Session["Inv_Date"] = Inv_DATE;
                            // Session["TransType"] = "Update";
                            // Session["AppStatus"] = 'D';
                            // Session["BtnName"] = "BtnSave";
                            // Session["AttachMentDetailItmStp"] = null;
                            // Session["Guid"] = null;
                        }
                    }
                    return RedirectToAction("ServiceSaleInvoiceDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ServiceSIModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ServiceSIModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ServiceSIModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetGLDetails(List<GL_Detail> GLDetail)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                DataTable DtblGLDetail = new DataTable();

                if (GLDetail != null)
                {
                    DtblGLDetail = ToDataTable(GLDetail);
                }
                DataSet GlDt = _ServiceSI_ISERVICE.GetAllGLDetails(DtblGLDetail);
                Validate = Json(GlDt);
                JsonResult DataRows = null;
                DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);

                return DataRows;
            }

            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            //return Validate;
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
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
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable ToDtblTaxDetail(string TaxDetails)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable Tax_detail = new DataTable();
                Tax_detail.Columns.Add("item_id", typeof(string));
                Tax_detail.Columns.Add("tax_id", typeof(string));
                Tax_detail.Columns.Add("tax_rate", typeof(string));
                Tax_detail.Columns.Add("tax_val", typeof(string));
                Tax_detail.Columns.Add("tax_level", typeof(string));
                Tax_detail.Columns.Add("tax_apply_on", typeof(string));
                if (TaxDetails != null)
                {
                    JArray jObjectTax = JArray.Parse(TaxDetails);
                    for (int i = 0; i < jObjectTax.Count; i++)
                    {
                        DataRow dtrowTaxDetailsLines = Tax_detail.NewRow();
                        dtrowTaxDetailsLines["item_id"] = jObjectTax[i]["item_id"].ToString();
                        dtrowTaxDetailsLines["tax_id"] = jObjectTax[i]["tax_id"].ToString();
                        string tax_rate = jObjectTax[i]["tax_rate"].ToString();
                        tax_rate = tax_rate.Replace("%", "");
                        dtrowTaxDetailsLines["tax_rate"] = tax_rate;
                        dtrowTaxDetailsLines["tax_level"] = jObjectTax[i]["tax_level"].ToString();
                        dtrowTaxDetailsLines["tax_val"] = jObjectTax[i]["tax_val"].ToString();
                        dtrowTaxDetailsLines["tax_apply_on"] = jObjectTax[i]["tax_apply_on"].ToString();
                        Tax_detail.Rows.Add(dtrowTaxDetailsLines);
                    }
                    ViewData["SRSITaxDetails"] = dtTaxdetail(jObjectTax);

                }
                DtblItemTaxDetail = Tax_detail;

                return DtblItemTaxDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblOCTdsDetail(string octdsDetails)
        {
            try
            {
                DataTable DtblOCtdsDetail = new DataTable();
                DataTable OCtds_detail = new DataTable();
                OCtds_detail.Columns.Add("tds_id", typeof(string));
                OCtds_detail.Columns.Add("tds_rate", typeof(string));
                OCtds_detail.Columns.Add("tds_val", typeof(string));
                OCtds_detail.Columns.Add("tds_level", typeof(string));
                OCtds_detail.Columns.Add("tds_apply_on", typeof(string));
                OCtds_detail.Columns.Add("oc_id", typeof(string));
                OCtds_detail.Columns.Add("supp_id", typeof(string));
                OCtds_detail.Columns.Add("tds_base_amt", typeof(string));//Added by Suraj Maurya on 11-12-2024
                OCtds_detail.Columns.Add("tds_ass_apply_on", typeof(string));//Added by Suraj Maurya on 11-12-2024


                if (octdsDetails != null)
                {
                    JArray jObjecttdsoc = JArray.Parse(octdsDetails);
                    for (int i = 0; i < jObjecttdsoc.Count; i++)
                    {
                        DataRow dtrowtdsDetailsLines = OCtds_detail.NewRow();
                        dtrowtdsDetailsLines["tds_id"] = jObjecttdsoc[i]["Tds_id"].ToString();
                        string tds_rate = jObjecttdsoc[i]["Tds_rate"].ToString();
                        tds_rate = tds_rate.Replace("%", "");
                        dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                        dtrowtdsDetailsLines["tds_level"] = jObjecttdsoc[i]["Tds_level"].ToString();
                        dtrowtdsDetailsLines["tds_val"] = jObjecttdsoc[i]["Tds_val"].ToString();
                        dtrowtdsDetailsLines["tds_apply_on"] = jObjecttdsoc[i]["Tds_apply_on"].ToString();
                        dtrowtdsDetailsLines["oc_id"] = jObjecttdsoc[i]["Tds_oc_id"].ToString();
                        dtrowtdsDetailsLines["supp_id"] = jObjecttdsoc[i]["Tds_supp_id"].ToString();
                        dtrowtdsDetailsLines["tds_base_amt"] = jObjecttdsoc[i]["Tds_totalAmnt"].ToString();
                        dtrowtdsDetailsLines["tds_ass_apply_on"] = jObjecttdsoc[i]["Tds_AssValApplyOn"].ToString();
                        OCtds_detail.Rows.Add(dtrowtdsDetailsLines);
                    }
                    ViewData["OCTDSDetails"] = dtOCTDSdetail(jObjecttdsoc);
                }
                DtblOCtdsDetail = OCtds_detail;
                return DtblOCtdsDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        public DataTable dtitemdetail(JArray jObject)
        {

            DataTable dtItem = new DataTable();


            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("inv_qty", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            /*start code Add ItmDisPer,ItmDisAmt,DisVal by Hina sharma on 07-01-2025*/
            dtItem.Columns.Add("item_disc_perc", typeof(string));
            dtItem.Columns.Add("item_disc_amt", typeof(string));
            dtItem.Columns.Add("item_disc_val", typeof(string));
            /*End code Add ItmDisPer,ItmDisAmt,DisVal by Hina sharma on 07-01-2025*/
            dtItem.Columns.Add("item_gr_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val_bs", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));/*Add TaxExempted,ManualGST by Hina sharma on 02-01-2025*/
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("hsn_no", typeof(string));
            dtItem.Columns.Add("item_remarks", typeof(string));




            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["item_name"] = jObject[i]["item_name"].ToString();

                dtrowLines["inv_qty"] = jObject[i]["inv_qty"].ToString();
                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                /*start code Add ItmDisPer,ItmDisAmt,DisVal by Hina sharma on 07-01-2025*/
                dtrowLines["item_disc_perc"] = jObject[i]["ItmDisPer"].ToString();
                dtrowLines["item_disc_amt"] = jObject[i]["ItmDisAmt"].ToString();
                dtrowLines["item_disc_val"] = jObject[i]["DisVal"].ToString();
                /*End code Add ItmDisPer,ItmDisAmt,DisVal by Hina sharma on 07-01-2025*/
                dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                if (jObject[i]["item_oc_amt"].ToString() == "" || jObject[i]["item_oc_amt"].ToString() == null)
                {
                    dtrowLines["item_oc_amt"] = "0.00";
                }
                else
                {
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                }
                dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                dtrowLines["hsn_no"] = jObject[i]["Hsn_no"].ToString();
                dtrowLines["item_remarks"] = jObject[i]["item_remarks"].ToString();

                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }

        public DataTable dtTaxdetail(JArray jObjectTax)
        {
            DataTable Tax_detail = new DataTable();

            Tax_detail.Columns.Add("item_id", typeof(string));
            Tax_detail.Columns.Add("tax_id", typeof(int));
            Tax_detail.Columns.Add("tax_name", typeof(string));
            Tax_detail.Columns.Add("tax_rate", typeof(string));
            Tax_detail.Columns.Add("tax_val", typeof(string));
            Tax_detail.Columns.Add("tax_level", typeof(int));
            Tax_detail.Columns.Add("tax_apply_on", typeof(string));
            Tax_detail.Columns.Add("tax_apply_Name", typeof(string));
            Tax_detail.Columns.Add("item_tax_amt", typeof(string));

            for (int i = 0; i < jObjectTax.Count; i++)
            {
                DataRow dtrowTaxDetailsLines = Tax_detail.NewRow();

                dtrowTaxDetailsLines["item_id"] = jObjectTax[i]["item_id"].ToString();
                dtrowTaxDetailsLines["tax_id"] = jObjectTax[i]["tax_id"].ToString();
                dtrowTaxDetailsLines["tax_name"] = jObjectTax[i]["TaxName"].ToString();
                //dtrowTaxDetailsLines["tax_rate"] = jObjectTax[i]["tax_rate"].ToString();
                string tax_rate = jObjectTax[i]["tax_rate"].ToString();
                tax_rate = tax_rate.Replace("%", "");
                dtrowTaxDetailsLines["tax_rate"] = tax_rate;
                dtrowTaxDetailsLines["tax_val"] = jObjectTax[i]["tax_val"].ToString();
                dtrowTaxDetailsLines["tax_level"] = jObjectTax[i]["tax_level"].ToString();
                dtrowTaxDetailsLines["tax_apply_on"] = jObjectTax[i]["tax_apply_on"].ToString();
                dtrowTaxDetailsLines["tax_apply_Name"] = jObjectTax[i]["tax_apply_onName"].ToString();
                dtrowTaxDetailsLines["item_tax_amt"] = jObjectTax[i]["totaltax_amt"].ToString();


                Tax_detail.Rows.Add(dtrowTaxDetailsLines);
            }

            return Tax_detail;
        }

        public DataTable dtOCTDSdetail(JArray jObjecttdsoc)
        {
            DataTable DtblItemtdsDetail = new DataTable();
            DataTable OCtds_detail = new DataTable();
            OCtds_detail.Columns.Add("tds_id", typeof(string));
            OCtds_detail.Columns.Add("tds_rate", typeof(string));
            OCtds_detail.Columns.Add("tds_val", typeof(string));
            OCtds_detail.Columns.Add("tds_level", typeof(string));
            OCtds_detail.Columns.Add("tds_apply_on", typeof(string));
            OCtds_detail.Columns.Add("tds_name", typeof(string));
            OCtds_detail.Columns.Add("tds_apply_Name", typeof(string));
            OCtds_detail.Columns.Add("oc_id", typeof(string));
            OCtds_detail.Columns.Add("supp_id", typeof(string));
            for (int i = 0; i < jObjecttdsoc.Count; i++)
            {
                DataRow dtrowtdsDetailsLines = OCtds_detail.NewRow();
                dtrowtdsDetailsLines["tds_id"] = jObjecttdsoc[i]["Tds_id"].ToString();
                string tds_rate = jObjecttdsoc[i]["Tds_rate"].ToString();
                tds_rate = tds_rate.Replace("%", "");
                dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                dtrowtdsDetailsLines["tds_level"] = jObjecttdsoc[i]["Tds_level"].ToString();
                dtrowtdsDetailsLines["tds_val"] = jObjecttdsoc[i]["Tds_val"].ToString();
                dtrowtdsDetailsLines["tds_apply_on"] = jObjecttdsoc[i]["Tds_apply_on"].ToString();
                dtrowtdsDetailsLines["tds_name"] = jObjecttdsoc[i]["Tds_name"].ToString();
                dtrowtdsDetailsLines["tds_apply_Name"] = jObjecttdsoc[i]["Tds_applyOnName"].ToString();
                dtrowtdsDetailsLines["oc_id"] = jObjecttdsoc[i]["Tds_oc_id"].ToString();
                dtrowtdsDetailsLines["supp_id"] = jObjecttdsoc[i]["Tds_supp_id"].ToString();
                OCtds_detail.Rows.Add(dtrowtdsDetailsLines);
            }

            return OCtds_detail;
        }
        public DataTable dtOCdetail(JArray jObjectOC)
        {
            DataTable OC_detail = new DataTable();

            //OC_detail.Columns.Add("oc_id", typeof(int));
            //OC_detail.Columns.Add("oc_name", typeof(string));
            //OC_detail.Columns.Add("curr_name", typeof(string));
            //OC_detail.Columns.Add("conv_rate", typeof(string));
            //OC_detail.Columns.Add("oc_val", typeof(string));
            //OC_detail.Columns.Add("OCValBs", typeof(string));
            //OC_detail.Columns.Add("tax_amt", typeof(string));
            //OC_detail.Columns.Add("total_amt", typeof(string));
            OC_detail.Columns.Add("oc_id", typeof(int));
            OC_detail.Columns.Add("oc_name", typeof(string));
            OC_detail.Columns.Add("curr_name", typeof(string));
            OC_detail.Columns.Add("conv_rate", typeof(string));
            OC_detail.Columns.Add("oc_val", typeof(string));
            OC_detail.Columns.Add("OCValBs", typeof(string));
            OC_detail.Columns.Add("tax_amt", typeof(string));
            OC_detail.Columns.Add("total_amt", typeof(string));
            OC_detail.Columns.Add("curr_id", typeof(string));
            OC_detail.Columns.Add("supp_id", typeof(string));
            OC_detail.Columns.Add("supp_type", typeof(string));
            OC_detail.Columns.Add("bill_no", typeof(string));
            OC_detail.Columns.Add("bill_date", typeof(string));
            for (int i = 0; i < jObjectOC.Count; i++)
            {
                DataRow dtrowOCDetailsLines = OC_detail.NewRow();

                //dtrowOCDetailsLines["oc_id"] = jObjectOC[i]["oc_id"].ToString();
                //dtrowOCDetailsLines["oc_name"] = jObjectOC[i]["OCName"].ToString();
                //dtrowOCDetailsLines["curr_name"] = jObjectOC[i]["OC_Curr"].ToString();
                //dtrowOCDetailsLines["conv_rate"] = jObjectOC[i]["OC_Conv"].ToString();
                //dtrowOCDetailsLines["oc_val"] = jObjectOC[i]["oc_val"].ToString();
                //dtrowOCDetailsLines["OCValBs"] = jObjectOC[i]["OC_AmtBs"].ToString();
                //dtrowOCDetailsLines["tax_amt"] = jObjectOC[i]["tax_amt"].ToString();
                //dtrowOCDetailsLines["total_amt"] = jObjectOC[i]["total_amt"].ToString();
                dtrowOCDetailsLines["oc_id"] = jObjectOC[i]["oc_id"].ToString();
                dtrowOCDetailsLines["oc_name"] = jObjectOC[i]["OCName"].ToString();
                dtrowOCDetailsLines["curr_name"] = jObjectOC[i]["OC_Curr"].ToString();
                dtrowOCDetailsLines["conv_rate"] = jObjectOC[i]["OC_Conv"].ToString();
                dtrowOCDetailsLines["oc_val"] = jObjectOC[i]["oc_val"].ToString();
                dtrowOCDetailsLines["OCValBs"] = jObjectOC[i]["OC_AmtBs"].ToString();
                dtrowOCDetailsLines["tax_amt"] = jObjectOC[i]["tax_amt"].ToString();
                dtrowOCDetailsLines["total_amt"] = jObjectOC[i]["total_amt"].ToString();
                dtrowOCDetailsLines["curr_id"] = jObjectOC[i]["curr_id"].ToString();
                dtrowOCDetailsLines["supp_id"] = jObjectOC[i]["supp_id"].ToString();
                dtrowOCDetailsLines["supp_type"] = jObjectOC[i]["supp_type"].ToString();
                dtrowOCDetailsLines["bill_no"] = jObjectOC[i]["bill_no"].ToString();
                dtrowOCDetailsLines["bill_date"] = jObjectOC[i]["bill_date"].ToString();
                OC_detail.Rows.Add(dtrowOCDetailsLines);
            }

            return OC_detail;
        }
        public DataTable dtCCdetail(JArray JAObj)
        {
            DataTable CC_Details = new DataTable();

            CC_Details.Columns.Add("vou_sr_no", typeof(string));
            CC_Details.Columns.Add("gl_sr_no", typeof(string));
            CC_Details.Columns.Add("acc_id", typeof(string));
            CC_Details.Columns.Add("cc_id", typeof(int));
            CC_Details.Columns.Add("cc_val_id", typeof(int));
            CC_Details.Columns.Add("cc_amt", typeof(string));
            CC_Details.Columns.Add("cc_name", typeof(string));
            CC_Details.Columns.Add("cc_val_name", typeof(string));
            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = CC_Details.NewRow();

                dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();
                dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                dtrowLines["cc_name"] = JAObj[i]["ddl_CC_Name"].ToString();
                dtrowLines["cc_val_name"] = JAObj[i]["ddl_CC_Type"].ToString();

                CC_Details.Rows.Add(dtrowLines);
            }
            return CC_Details;
        }
        public DataTable dtVoudetail(JArray jObjectVOU)
        {
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
                vou_Details.Rows.Add(dtrowVouDetailsLines);
            }
            return vou_Details;
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
        private ActionResult ServicePIDelete(ServiceSIModel _SSIModel, string command, string title)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();

                string BPNo = _SSIModel.Sinv_no;
                string BankPayNumber = BPNo.Replace("/", "");

                string Message = _ServiceSI_ISERVICE.ServiceSIDelete(_SSIModel, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(BankPayNumber))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, BankPayNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                _SSIModel.Message = "Deleted";
                _SSIModel.Command = "Refresh";
                _SSIModel.Sinv_no = null;
                _SSIModel.Sinv_dt = null;
                _SSIModel.TransType = "Refresh";
                _SSIModel.AppStatus = "DL";
                _SSIModel.BtnName = "BtnDelete";
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["Inv_No"] = null;
                //Session["Inv_Date"] = null;
                //_SSIModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("ServiceSaleInvoiceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult ServiceSaleInvoiceApprove(ServiceSIModel _SSIModel, string PV_VoucherNarr, string BP_VoucherNarr,string DN_VoucherNarr)
        {
            //JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            //JsonResult DataRows = null;
            try
            {
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
                if (DocumentMenuId != null)
                {
                    MenuDocId = DocumentMenuId;
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    MenuDocId = Session["MenuDocumentId"].ToString();
                //}
                var DocNo = _SSIModel.Sinv_no;
                var DocDate = _SSIModel.Sinv_dt;
                var A_Status = _SSIModel.A_Status;
                var A_Level = _SSIModel.A_Level;
                var A_Remarks = _SSIModel.A_Remarks;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string SaleVouMsg = _SSIModel.SaleVouMsg;

                string Message = _ServiceSI_ISERVICE.ApproveSSI(DocNo, DocDate, MenuDocId, BranchID, Comp_ID, UserID, mac_id, A_Status, A_Level, A_Remarks, SaleVouMsg, PV_VoucherNarr, BP_VoucherNarr,DN_VoucherNarr);/*Add by Hina on 06-07-2024 to add for tds third party OC*/
                string[] FDetail = Message.Split(',');
                string ApMessage = FDetail[6].ToString().Trim();
                string INV_NO = FDetail[0].ToString();
                string INV_DT = FDetail[7].ToString();

                if (ApMessage == "A")
                {
                    try
                    {
                        //string fileName = "SSI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "TaxInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_SSIModel, INV_NO, INV_DT, fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, INV_NO, "AP", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _SSIModel.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _SSIModel.Message = _SSIModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                    //Session["Message"] = "Approved";
                   // _SSIModel.Message = "Approved";
                }
                _SSIModel.TransType = "Update";
                _SSIModel.Command = "Approve";
                _SSIModel.Sinv_no = INV_NO;
                _SSIModel.Sinv_dt = INV_DT;
                _SSIModel.AppStatus = "D";
                _SSIModel.BtnName = "BtnEdit";

                // Session["TransType"] = "Update";
                // Session["Command"] = "Approve";
                // Session["Inv_No"] = INV_NO;
                //Session["Inv_Date"] = INV_DT;
                // Session["AppStatus"] = 'D';
                // Session["BtnName"] = "BtnEdit";
                //TempData["ListFilterData"] = FilterData;             
                return RedirectToAction("ServiceSaleInvoiceDetail");
                //DataRows = Json(GrnDetail);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            //return DataRows;
        }

        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1, string PV_VoucherNarr, string BP_VoucherNarr, string DN_VoucherNarr)
        {
            //JArray jObjectBatch = JArray.Parse(list);
            ServiceSIModel _SSIModel = new ServiceSIModel();
            URLModelDetails URLModel = new URLModelDetails();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _SSIModel.Sinv_no = jObjectBatch[i]["DocNo"].ToString();
                    _SSIModel.Sinv_dt = jObjectBatch[i]["DocDate"].ToString();
                    _SSIModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _SSIModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _SSIModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                    _SSIModel.SaleVouMsg = jObjectBatch[i]["SaleVouMsg"].ToString();

                }
            }
            if (_SSIModel.A_Status != "Approve")
            {
                _SSIModel.A_Status = "Approve";
            }
            _SSIModel.ListFilterData1 = ListFilterData1;
            ServiceSaleInvoiceApprove(_SSIModel, PV_VoucherNarr, BP_VoucherNarr,DN_VoucherNarr);/*Add by Hina on 06-07-2024 to add for tds third party OC*/
            TempData["ModelData"] = _SSIModel;
            TempData["WF_status"] = WF_status1;
            URLModel.Sinv_no = _SSIModel.Sinv_no;
            URLModel.Sinv_dt = _SSIModel.Sinv_dt;
            URLModel.TransType = _SSIModel.TransType;
            URLModel.BtnName = _SSIModel.BtnName;
            URLModel.Command = _SSIModel.Command;
            return RedirectToAction("ServiceSaleInvoiceDetail", URLModel);
        }

        //public ActionResult EditPurchaseInvoice(string Inv_no, string Inv_dt, string FilterData)
        //{
        //    Session["Message"] = "New";
        //    Session["Command"] = "Add";
        //    Session["Inv_No"] = Inv_no;
        //   Session["Inv_Date"] = Inv_dt;
        //    TempData["ListFilterData"] = FilterData;
        //    Session["TransType"] = "Update";
        //    Session["AppStatus"] = 'D';
        //    Session["BtnName"] = "BtnToDetailPage";
        //    return RedirectToAction("ServiceSaleInvoiceDetail");
        //}

        private List<ServiceSalesInvoiceList> GetServiceSIList(ServiceSIListModel _SSIListModel)
        {
            _SSIList = new List<ServiceSalesInvoiceList>();
            try
            {
                string UserID = string.Empty;
                string SuppType = string.Empty;
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                var wfStatus = "";
                if (_SSIListModel.WF_status != null)
                {
                    wfStatus = _SSIListModel.WF_status;
                }
                else
                {
                    wfStatus = null;
                }

                DataSet DSet = _ServiceSI_ISERVICE.GetSSIList(CompID, BrchID, UserID, _SSIListModel.CustID, _SSIListModel.SSI_FromDate, _SSIListModel.SSI_ToDate, _SSIListModel.Status, _SSIListModel.wfdocid, wfStatus, _SSIListModel.SQ_SalePerson);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ServiceSalesInvoiceList _ServicePIList = new ServiceSalesInvoiceList();
                        _ServicePIList.InvNo = dr["inv_no"].ToString();
                        _ServicePIList.InvDate = dr["inv_dt"].ToString();
                        _ServicePIList.src_type = dr["src_type"].ToString();
                        _ServicePIList.src_doc_no = dr["src_doc_number"].ToString();
                        _ServicePIList.src_doc_dt = dr["src_doc_date"].ToString();
                        _ServicePIList.SalesPerson = dr["SalesPerson"].ToString();
                        _ServicePIList.InvDt = dr["InvDt"].ToString();
                        _ServicePIList.InvValue = dr["net_val_bs"].ToString();
                        _ServicePIList.CustName = dr["cust_name"].ToString();
                        _ServicePIList.RefDocNo = dr["ref_doc_no"].ToString();
                        _ServicePIList.RefDocDt = dr["ref_doc_dt"].ToString();
                        _ServicePIList.InvStauts = dr["InvStauts"].ToString();
                        _ServicePIList.CreateDate = dr["CreateDate"].ToString();
                        _ServicePIList.ApproveDate = dr["ApproveDate"].ToString();
                        _ServicePIList.ModifyDate = dr["ModifyDate"].ToString();
                        _ServicePIList.create_by = dr["create_by"].ToString();
                        _ServicePIList.app_by = dr["app_by"].ToString();
                        _ServicePIList.mod_by = dr["mod_by"].ToString();
                        _SSIList.Add(_ServicePIList);
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
            return _SSIList;
        }

        public ActionResult SearchServiceSalesInvoiceList(string CustId, string Fromdate, string Todate, string Status,string sales_person)
        {
            ServiceSIListModel _SSIListModel = new ServiceSIListModel();
            try
            {
                //Session.Remove("WF_Docid");
                //Session.Remove("WF_status");
                string UserID = string.Empty;
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
                SuppType = "L";
                _SSIList = new List<ServiceSalesInvoiceList>();
                DataSet DSet = _ServiceSI_ISERVICE.GetSSIList(CompID, BrchID, UserID, CustId, Fromdate, Todate, Status, DocumentMenuId, _SSIListModel.WF_status, sales_person);
                //Session["SSISearch"] = "SPI_Search";
                _SSIListModel.SSISearch = "SSI_Search";

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ServiceSalesInvoiceList _ServicePIList = new ServiceSalesInvoiceList();
                        _ServicePIList.InvNo = dr["inv_no"].ToString();
                        _ServicePIList.InvDate = dr["inv_dt"].ToString();
                        _ServicePIList.src_type = dr["src_type"].ToString();
                        _ServicePIList.src_doc_no = dr["src_doc_number"].ToString();
                        _ServicePIList.src_doc_dt = dr["src_doc_date"].ToString();
                        _ServicePIList.SalesPerson = dr["SalesPerson"].ToString();
                        _ServicePIList.InvDt = dr["InvDt"].ToString();
                        _ServicePIList.InvValue = dr["net_val_bs"].ToString();
                        _ServicePIList.CustName = dr["cust_name"].ToString();
                        _ServicePIList.RefDocNo = dr["ref_doc_no"].ToString();
                        _ServicePIList.RefDocDt = dr["ref_doc_dt"].ToString();
                        _ServicePIList.InvStauts = dr["InvStauts"].ToString();
                        _ServicePIList.CreateDate = dr["CreateDate"].ToString();
                        _ServicePIList.ApproveDate = dr["ApproveDate"].ToString();
                        _ServicePIList.ModifyDate = dr["ModifyDate"].ToString();
                        _ServicePIList.create_by = dr["create_by"].ToString();
                        _ServicePIList.app_by = dr["app_by"].ToString();
                        _ServicePIList.mod_by = dr["mod_by"].ToString();
                        _SSIList.Add(_ServicePIList);
                    }
                }
                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                ViewBag.FinStDt = DSet.Tables[2].Rows[0]["findate"];
                _SSIListModel.SSIList = _SSIList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialServiceSaleInvoiceList.cshtml", _SSIListModel);
        }
        public DataSet GetServiceSaleInvoiceEdit(string Inv_No, string Inv_Date)
        {
            try
            {
                //JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string Br_ID = string.Empty;
                string Voutype = "SV";
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
                    UserID = Session["UserId"].ToString();
                }
                DataSet result = _ServiceSI_ISERVICE.GetServiceSalesInvoiceDetail(Comp_ID, Br_ID, Voutype, Inv_No, Inv_Date, UserID, DocumentMenuId);
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
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                ServiceSIModelattch _ServiceSIModelattch = new ServiceSIModelattch();
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
                _ServiceSIModelattch.Guid = DocNo;
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
                    _ServiceSIModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _ServiceSIModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _ServiceSIModelattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
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
        public ActionResult GetSalesPersonList(ServiceSIModel _ServiceSIModel)
        {
            string SalesPersonName = string.Empty;
            Dictionary<string, string> SPersonList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string UserName = string.Empty;
            string rpt_id = "0";
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
                if (Session["Userid"] != null)
                {
                    UserID = Session["Userid"].ToString();
                }
                if (Session["UserName"] != null)
                {
                    UserName = Session["UserName"].ToString();
                }
                if (Session["crm"] != null)
                {
                    crm = Session["crm"].ToString();
                }
                if (Session["rpt_id"] != null)
                {
                    rpt_id = Session["rpt_id"].ToString();
                }
                if (string.IsNullOrEmpty(_ServiceSIModel.SalePerson))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = _ServiceSIModel.SalePerson;
                }
                SPersonList = _ServiceSI_ISERVICE.GetSalesPersonList(Comp_ID, SalesPersonName, Br_ID, UserID, _ServiceSIModel.Sinv_no, _ServiceSIModel.Sinv_dt);
                List<SalesPersonName> _SlPrsnList = new List<SalesPersonName>();
                if ((rpt_id == "0" || _ServiceSIModel.TransType == "Update" || UserID == "1001") && crm == "Y" )
                {
                    foreach (var data in SPersonList)
                    {
                        SalesPersonName _SlPrsnDetail = new SalesPersonName();
                        _SlPrsnDetail.slprsn_id = data.Key;
                        _SlPrsnDetail.slprsn_name = data.Value;
                        _SlPrsnList.Add(_SlPrsnDetail);
                    }
                }
                else
                {
                    _SlPrsnList.Insert(0, new SalesPersonName() { slprsn_id = UserID, slprsn_name = UserName });
                }
                    
                _ServiceSIModel.SalesPersonNameList = _SlPrsnList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(SPersonList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //Added by Nidhi on 28-07-2025
        public string SavePdfDocToSendOnEmailAlert(ServiceSIModel _model,string DocNo, string DocDate,string fileName, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            PrintOptionsList ProntOption = new PrintOptionsList();
            if (_model.HdnPrintOptons == "Y")
            {
                ProntOption.PrtOpt_catlog_number = _model.PrtOpt_catlog_number == "Y" ? true : false;
                ProntOption.PrtOpt_item_code = _model.PrtOpt_item_code == "Y" ? true : false;
                ProntOption.PrtOpt_item_desc = _model.PrtOpt_item_desc == "Y" ? true : false;
            }
            else
            {
                ProntOption.PrtOpt_catlog_number = true;
                ProntOption.PrtOpt_item_code = true;
                ProntOption.PrtOpt_item_desc = true;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("PrintRemarks", typeof(string));
            dt.Columns.Add("ShowWithoutSybbol", typeof(string));
            dt.Columns.Add("showDeclare1", typeof(string));
            dt.Columns.Add("showDeclare2", typeof(string));
            dt.Columns.Add("showInvHeading", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("NumberOfCopy", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));

            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = _model.PrintFormat;
            dtr["ShowProdDesc"] = _model.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
            dtr["PrintRemarks"] = _model.PrintRemarks;
            dtr["ShowWithoutSybbol"] = _model.ShowWithoutSybbol;
            dtr["showDeclare1"] = _model.showDeclare1;
            dtr["showDeclare2"] = _model.showDeclare2;
            dtr["showInvHeading"] = _model.showInvHeading;
            dtr["CustAliasName"] = _model.CustomerAliasName;
            dtr["NumberOfCopy"] = _model.NumberofCopy;
            dtr["ShowTotalQty"] = _model.ShowTotalQty;
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfDataOfGstInv(dt, docid, DocNo, DocDate, _model.NumberofCopy, ProntOption);
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "ErrorPage";
            }
            return null;
        }
        public string SavePdfDocToSendOnEmailAlert_Ext(string Doc_no, string Doc_dt,string docid, string fileName, string PrintFormat)
        {
            var printOptionsList = JsonConvert.DeserializeObject<List<ServiceSIModel>>(PrintFormat);
            ServiceSIModel _model = new ServiceSIModel();
            ServiceSIModel _model1 = new ServiceSIModel();
            PrintOptionsList ProntOption = new PrintOptionsList();
            if (_model.HdnPrintOptons == "Y")
            {
                ProntOption.PrtOpt_catlog_number = _model.PrtOpt_catlog_number == "Y" ? true : false;
                ProntOption.PrtOpt_item_code = _model.PrtOpt_item_code == "Y" ? true : false;
                ProntOption.PrtOpt_item_desc = _model.PrtOpt_item_desc == "Y" ? true : false;
            }
            else
            {
                ProntOption.PrtOpt_catlog_number = true;
                ProntOption.PrtOpt_item_code = true;
                ProntOption.PrtOpt_item_desc = true;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("PrintRemarks", typeof(string));
            dt.Columns.Add("ShowWithoutSybbol", typeof(string));
            dt.Columns.Add("showDeclare1", typeof(string));
            dt.Columns.Add("showDeclare2", typeof(string));
            dt.Columns.Add("showInvHeading", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("NumberOfCopy", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));

            DataRow dtr = dt.NewRow();
            if(PrintFormat == "")
            {
                dtr["PrintFormat"] = _model1.PrintFormat;
                dtr["ShowProdDesc"] = _model1.ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = _model1.ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = _model1.ShowProdTechDesc;
                dtr["PrintRemarks"] = _model1.PrintRemarks;
                dtr["ShowWithoutSybbol"] = _model1.ShowWithoutSybbol;
                dtr["showDeclare1"] = _model1.showDeclare1;
                dtr["showDeclare2"] = _model1.showDeclare2;
                dtr["showInvHeading"] = _model1.showInvHeading;
                dtr["CustAliasName"] = _model1.CustomerAliasName;
                dtr["NumberOfCopy"] = _model1.NumberofCopy;
                dtr["ShowTotalQty"] = _model1.ShowTotalQty;
            }
            else
            {
                dtr["PrintFormat"] = printOptionsList[0].PrintFormat;
                dtr["ShowProdDesc"] = printOptionsList[0].ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = printOptionsList[0].ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = printOptionsList[0].ShowProdTechDesc;
                dtr["PrintRemarks"] = printOptionsList[0].PrintRemarks;
                dtr["ShowWithoutSybbol"] = printOptionsList[0].ShowWithoutSybbol;
                dtr["showDeclare1"] = printOptionsList[0].showDeclare1;
                dtr["showDeclare2"] = printOptionsList[0].showDeclare2;
                dtr["showInvHeading"] = printOptionsList[0].showInvHeading;
                dtr["CustAliasName"] = printOptionsList[0].CustomerAliasName;
                dtr["NumberOfCopy"] = printOptionsList[0].NumberofCopy;
                dtr["ShowTotalQty"] = printOptionsList[0].ShowTotalQty;
            }
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;

            var data = GetPdfDataOfGstInv(dt, docid, Doc_no, Doc_dt, _model.NumberofCopy, ProntOption);
            var commonCont = new CommonController(_Common_IServices);
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
        public ActionResult SendEmailAlert(ServiceSIModel _model, string mail_id, string status, string docid, string SrcType, string Doc_no, string Doc_dt, string statusAM, string filepath)
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
                            string fileName = "SSI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
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

        [HttpPost]
        public JsonResult GetSrcDocNumberList(string Cust_id)
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
                DataSet result = _ServiceSI_ISERVICE.GetSrcDocNumberList(Cust_id, Comp_ID, Br_ID);

                DataRow Drow = result.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";
                result.Tables[0].Rows.InsertAt(Drow, 0);
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
        public JsonResult GetItemDetailData(string Cust_id,string SourceDocNo, string SourceDocdt)
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
             
                DataSet result = _ServiceSI_ISERVICE.GetItemDetailData (Cust_id, Comp_ID, Br_ID, SourceDocNo, SourceDocdt);
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