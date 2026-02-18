using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesEnquiry;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesEnquiry;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.SalesEnquiry
{
    public class SalesEnquiryController : Controller
    {
        string CompID, language,BrchID, UserID = String.Empty;
        string DocumentMenuId = "105103117", title;
        List<SlsEnqryList> _SlsEnqryList;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SalesEnquiry_ISERVICES _SalesEnquiry_ISERVICES;

        public SalesEnquiryController(Common_IServices _Common_IServices, SalesEnquiry_ISERVICES _SalesEnquiry_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._SalesEnquiry_ISERVICES = _SalesEnquiry_ISERVICES;
        }
        // GET: ApplicationLayer/SalesEnquiry
        private void GetCompDetail()
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
        }
        public ActionResult SalesEnquiry(SEListModel _SEListModel)
        {
            CommonPageDetails();
            _SEListModel.DocumentMenuId = DocumentMenuId;
            //_SEListModel.CustTyp_List = "D";

            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;

            //GetAutoCompleteSearchCustList(_SEListModel, "0","");
            GetAllDDLAndListData(_SEListModel);
            CommonPageDetails();
            _SEListModel.Title = title;
            ViewBag.DocumentMenuId = DocumentMenuId;

            List<Status> statusLists = new List<Status>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                Status list = new Status();
                list.status_id = dr["status_code"].ToString();
                list.status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            _SEListModel.StatusList = statusLists;
            if (TempData["ListFilterData"] != null)
            {
                if (TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    _SEListModel.CustID = a[3].Trim();
                    _SEListModel.FromDate = a[8].Trim();
                    _SEListModel.ToDate = a[9].Trim();
                    _SEListModel.Status = a[10].Trim();
                    if (_SEListModel.Status == "0")
                    {
                        _SEListModel.Status = null;
                    }
                    _SEListModel.ListFilterData = TempData["ListFilterData"].ToString();
                }
            }
            else
            {
                _SEListModel.FromDate = startDate;
                _SEListModel.ToDate = CurrentDate;
                
            }
            _SEListModel.SalesEnquiryList = getSlsEnqryList(_SEListModel);
            _SEListModel.Title = title;
            _SEListModel.SESearch = "0";
            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesEnquiry/SalesEnquiryList.cshtml", _SEListModel);
        }
       
        public ActionResult AddSalesEnquiryDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //CommonPageDetails();
            //return RedirectToAction("SalesEnquiryDetail", "SalesEnquiry");
            URLDetailModel URLModel = new URLDetailModel();
            SalesEnquiryModel _SalesEnquiryModel = new SalesEnquiryModel();
            _SalesEnquiryModel.Message = "New";
            _SalesEnquiryModel.Command = "New";
            _SalesEnquiryModel.DocumentStatus = "D";
            _SalesEnquiryModel.TransType = "Save";
            _SalesEnquiryModel.BtnName = "BtnAddNew";
            _SalesEnquiryModel.DocumentMenuId = DocumentMenuId;
            //_SalesEnquiryModel.Enquiry_type = "D";
            TempData["ModelData"] = _SalesEnquiryModel;
            URLModel.DocumentMenuId = DocumentMenuId;
            URLModel.TransType = "Save";
            URLModel.BtnName = "BtnAddNew";
            URLModel.Command = "New";
            URLModel.DocDate = null;
            URLModel.DocNo = null;
            URLModel.EnquiryType = "D";
            URLModel.CustType = "C";
            //Session.Remove("ProspectFromQuot");
            //Session["Message"] = "New";
            //Session["Command"] = "New";
            //Session["DocumentStatus"] = "D";
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session.Remove("ProspectFromQuot");
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("SalesEnquiry");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("SalesEnquiryDetail", "SalesEnquiry", URLModel);
        }
        public ActionResult SalesEnquiryDetail(URLDetailModel URLModel,string ProsFlag, string ProsEnq)
        {
            try
            {
                GetCompDetail();
                
                var SEModel = TempData["ModelData"] as SalesEnquiryModel;
                if (SEModel != null)
                {
                    SEModel.UserID = UserID;
                    if (URLModel.DocumentMenuId != null)
                    {
                        DocumentMenuId = URLModel.DocumentMenuId;
                    }
                    else
                    {
                        DocumentMenuId = SEModel.DocumentMenuId;
                    }

                    if (URLModel.EnquiryType != null)
                    {
                        var EnquiryType = URLModel.EnquiryType;
                    }
                    else
                    {
                        var EnquiryType = SEModel.Enquiry_type;
                    }
                    if (URLModel.CustType != null)
                    {
                        var CustType = URLModel.CustType;
                    }
                    else
                    {
                        var CustType = SEModel.CustPros_type;
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    SEModel.DocumentMenuId = DocumentMenuId;
                    SEModel.GstApplicable = ViewBag.GstApplicable;
                    
                    List<CustomerName> _CustList = new List<CustomerName>();
                    CustomerName _CustName = new CustomerName();
                    _CustName.Cust_id = "0";
                    _CustName.Cust_name = "---Select---";
                    _CustList.Add(_CustName);
                    SEModel.CustomerNameList = _CustList;
                    GetAllData(SEModel, "C", "D");
                   

                    string ValDigit = "";
                    string QtyDigit = "";
                    string RateDigit = "";
                    //if (SEModel.DocumentMenuId == "105103145105")
                    //{
                    //    ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"].ToString()));
                    //    QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"].ToString()));
                    //    RateDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpRateDigit"].ToString()));
                    //}
                    //else
                    //{
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    //}
                    SEModel.ValDigit = ValDigit;
                    SEModel.QtyDigit = QtyDigit;
                    SEModel.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if (SEModel.TransType == "Update" || SEModel.TransType == "Edit")
                    {
                        ShowDataAfterSave(SEModel/*, RateDigit, QtyDigit, ValDigit*/);
                        //if (SEModel.Status == "D" && SEModel.hdnQuationCreated=="Y")
                        //{
                        //    SEModel.BtnName = "Refresh";
                        //}
                    }
                    else
                    {
                        ViewBag.DocumentStatus = "D";
                        SEModel.Title = title;
                     }
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesEnquiry/SalesEnquiryDetail.cshtml", SEModel);
                }
                else
                {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                    var commCont = new CommonController(_Common_IServices);
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {
                        TempData["Message1"] = "Financial Year not Exist";
                    }
                    /*End to chk Financial year exist or not*/
                    SalesEnquiryModel SEModel1 = new SalesEnquiryModel();
                    SEModel1.UserID = UserID;
                    if (URLModel.DocumentMenuId != null)
                    {
                        DocumentMenuId = URLModel.DocumentMenuId;
                    }
                    else
                    {
                        if (DocumentMenuId != null)
                        {
                            SEModel1.DocumentMenuId = DocumentMenuId;

                        }
                        else
                        {
                            DocumentMenuId = SEModel1.DocumentMenuId;

                        }
                    }
                     SEModel1.Message = "New";
                     SEModel1.DocumentStatus = "D";
                     SEModel1.BtnName = "BtnAddNew";
                     SEModel1.TransType = "Save";
                     SEModel1.Command = "New";
                    if (ProsFlag != null)
                    {
                        SEModel1.CustPros_type = "P";
                        SEModel1.Enquiry_type = ProsEnq;
                    }
                    
                    if (SEModel1.CustPros_type=="P")
                    {
                         SEModel1.ProspectFromEnquiry = "Y";
                    }
                    if (URLModel.EnquiryType != null)
                    {
                        var EnquiryType = URLModel.EnquiryType;
                    }
                    else
                    {
                        var EnquiryType = SEModel1.Enquiry_type;
                    }
                    if (URLModel.CustType != null)
                    {
                        var CustType = URLModel.CustType;
                    }
                    else
                    {
                        var CustType = SEModel1.CustPros_type;
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    SEModel1.DocumentMenuId = DocumentMenuId;
                    SEModel1.GstApplicable = ViewBag.GstApplicable;
                    

                    List<CustomerName> _CustList = new List<CustomerName>();
                    CustomerName _CustName = new CustomerName();
                    _CustName.Cust_id = "0";
                    _CustName.Cust_name = "---Select---";
                    _CustList.Add(_CustName);
                    SEModel1.CustomerNameList = _CustList;
                    if (ProsFlag != null)
                    {
                        GetAllData(SEModel1, SEModel1.CustPros_type, SEModel1.Enquiry_type);
                    }
                    else
                    {
                        GetAllData(SEModel1, "C", "D");
                    }
                        
                    
                    if (URLModel.DocNo != null || URLModel.DocDate != null)
                    {
                        SEModel1.EnquiryNo = URLModel.DocNo;
                        SEModel1.EnquiryDt = URLModel.DocDate;
                    }
                    if (URLModel.TransType != null)
                    {
                        SEModel1.TransType = URLModel.TransType;
                    }
                    if (URLModel.BtnName != null)
                    {
                        SEModel1.BtnName = URLModel.BtnName;
                    }
                    if (URLModel.Command != null)
                    {
                        SEModel1.Command = URLModel.Command;
                    }
                    
                    string ValDigit = "";
                    string QtyDigit = "";
                    string RateDigit = "";
                    //if (SEModel1.DocumentMenuId == "105103145105")
                    //{
                    //    ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"].ToString()));
                    //    QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"].ToString()));
                    //    RateDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpRateDigit"].ToString()));
                    //}
                    //else
                    //{
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    //}
                    SEModel1.ValDigit = ValDigit;
                    SEModel1.QtyDigit = QtyDigit;
                    SEModel1.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if (SEModel1.TransType == "Update" || SEModel1.TransType == "Edit")
                    {
                        ShowDataAfterSave(SEModel1/*, RateDigit, QtyDigit, ValDigit*/);
                        //if (SEModel1.Status == "D" && SEModel1.hdnQuationCreated == "Y")
                        //{
                        //    SEModel1.BtnName = "Refresh";
                        //}
                    }
                    else
                    {
                        ViewBag.DocumentStatus = "D";
                        SEModel1.Title = title;
                    }
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesEnquiry/SalesEnquiryDetail.cshtml", SEModel1);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ShowDataAfterSave(SalesEnquiryModel SEModel/*, string RateDigit, string QtyDigit, string ValDigit*/)
        {
            try
            {
                GetCompDetail();
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    SEModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                }
                DataSet ds = new DataSet();
                string SENo = SEModel.EnquiryNo;
                ds = _SalesEnquiry_ISERVICES.Edit_SEDetail(CompID, BrchID, DocumentMenuId, UserID, SENo);

                SEModel.EnquiryNo = ds.Tables[0].Rows[0]["enq_no"].ToString();
                SEModel.EnquiryDt = ds.Tables[0].Rows[0]["enq_dt"].ToString();
                SEModel.Enquiry_type = ds.Tables[0].Rows[0]["enq_type"].ToString();
                SEModel.CustPros_type = ds.Tables[0].Rows[0]["cust_type"].ToString();
                SEModel.EnquirySource = ds.Tables[0].Rows[0]["enq_sour"].ToString();
                SEModel.Cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                SEModel.SE_CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();

                List<CustomerName> _CustList = new List<CustomerName>();
                _CustList.Add(new CustomerName { Cust_id = SEModel.Cust_id, Cust_name = SEModel.SE_CustName });
                SEModel.CustomerNameList = _CustList;

                SEModel.BillingAddress = ds.Tables[0].Rows[0]["bill_address"].ToString();
                //SEModel.ShippingAddres = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                if (SEModel.CustPros_type == "C")
                {
                    SEModel.Billing_id = Convert.ToInt32(ds.Tables[0].Rows[0]["address_id"].ToString());
                    //SEModel.Shipping_id = Convert.ToInt32(ds.Tables[0].Rows[0]["_ship_addr_ID"].ToString());
                }
                SEModel.cont_pers = ds.Tables[0].Rows[0]["cont_pers"].ToString(); 
                SEModel.cont_email = ds.Tables[0].Rows[0]["cont_email"].ToString();
                SEModel.cont_num = ds.Tables[0].Rows[0]["cont_no"].ToString();
                SEModel.cont_web = ds.Tables[0].Rows[0]["cont_wsite"].ToString();
                SEModel.Currency = ds.Tables[0].Rows[0]["curr_name"].ToString();
                SEModel.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                SEModel.convrate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                SEModel.SE_SalePerson = ds.Tables[0].Rows[0]["slsprsn_id"].ToString();
                SEModel.SE_SalePersonID = ds.Tables[0].Rows[0]["slsprsn_id"].ToString();
                SEModel.SE_SalePersonName = ds.Tables[0].Rows[0]["sls_pers_name"].ToString();
                if(SEModel.SE_SalePerson == "0" && SEModel.SE_SalePersonName == "")
                {
                    SEModel.SE_SalePersonName = "---Select---";
                }
                //List<SalePerson> _slsprsnList = new List<SalePerson>();
                //_slsprsnList.Add(new SalePerson { salep_id = SEModel.SE_SalePerson, salep_name = SEModel.SE_SalePersonName });
                //SEModel.SalePersonList = _slsprsnList;
                SEModel.Remarks = ds.Tables[0].Rows[0]["remark"].ToString();


                
                SEModel.hdnQuationCreated = ds.Tables[0].Rows[0]["quot_crt"].ToString();
                if(SEModel.hdnQuationCreated=="Y")
                {
                    SEModel.QuotationCreated = true;
                }
                else
                {
                    SEModel.QuotationCreated = false;
                }
                SEModel.QuotationNumDt = ds.Tables[0].Rows[0]["quot_num_dt"].ToString();
                //SEModel.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                //SEModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                //SEModel.SpanCustPricePolicy = ds.Tables[0].Rows[0]["cust_pr_pol"].ToString();
                //SEModel.SpanCustPriceGroup = ds.Tables[0].Rows[0]["cust_pr_grp"].ToString();
                //GetSalesPersonList(SEModel);


                SEModel.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                SEModel.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                //SEModel.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                //SEModel.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                SEModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                SEModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                SEModel.Createid = ds.Tables[0].Rows[0]["Creator_id"].ToString();
                SEModel.SEStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                

                SEModel.SE_ItemDetail = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                SEModel.Communicationdetails = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                
                string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                string create_id = ds.Tables[0].Rows[0]["Creator_id"].ToString();
                string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                SEModel.Status = doc_status;
                if (doc_status == "D")
                {
                    if (create_id != UserID)
                    {
                        //Session["BtnName"] = "Refresh";
                        SEModel.BtnName = "Refresh";
                    }
                }
                if (SEModel.Status == "SQR" && SEModel.hdnQuationCreated == "Y")
                {
                    SEModel.BtnName = "Refresh";
                }
                SEModel.DocumentStatus = doc_status;
                SEModel.Title = title;
                ViewBag.ItemDetailsList = ds.Tables[1];
                ViewBag.CommunicationDtl = ds.Tables[3];
                ViewBag.AttechmentDetails = ds.Tables[4];
                ViewBag.SubItemDetails = ds.Tables[5];
                ViewBag.DocumentStatus = SEModel.DocumentStatus;
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalesEnquiryBtnCommand(SalesEnquiryModel SEModel, string command)
        {
            try
            {
                GetCompDetail();
                /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (SEModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        URLDetailModel URLModel = new URLDetailModel();
                        SalesEnquiryModel SEModelAddNew = new SalesEnquiryModel();
                        SEModelAddNew.Message = "New";
                        SEModelAddNew.DocumentStatus = "D";
                        SEModelAddNew.BtnName = "BtnAddNew";
                        SEModelAddNew.TransType = "Save";
                        SEModelAddNew.Command = "New";
                        SEModelAddNew.DocumentMenuId = SEModel.DocumentMenuId;
                        SEModelAddNew.Enquiry_type = SEModel.Enquiry_type;
                        SEModelAddNew.CustType = SEModel.CustPros_type;
                        TempData["ModelData"] = SEModelAddNew;
                        URLModel.DocumentMenuId = SEModel.DocumentMenuId;
                        URLModel.TransType = "Save";
                        URLModel.BtnName = "BtnAddNew";
                        URLModel.Command = "New";
                        URLModel.EnquiryType = SEModel.Enquiry_type;
                        URLModel.CustType = SEModel.CustPros_type;
                        //Session["Message"] = "New";
                        //Session["DocumentStatus"] = "D";
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/

                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist"; 
                            if (!string.IsNullOrEmpty(SEModel.EnquiryNo))
                                return RedirectToAction("EditUpdateSEDetail", new { SENo = SEModel.EnquiryNo, ListFilterData = SEModel.ListFilterData1, DocumentMenuId = SEModel.DocumentMenuId,EnquiryType = SEModel.Enquiry_type });
                            else
                                SEModelAddNew.Command = "Refresh";
                            SEModelAddNew.TransType = "Refresh";
                            SEModelAddNew.BtnName = "Refresh";
                            SEModelAddNew.DocumentStatus = null;
                            TempData["ModelData"] = SEModelAddNew;
                            return RedirectToAction("SalesEnquiryDetail", "SalesEnquiry");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("SalesEnquiryDetail", "SalesEnquiry", URLModel);

                    case "Edit":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/

                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("EditUpdateSEDetail", new { SENo = SEModel.EnquiryNo, ListFilterData = SEModel.ListFilterData1, DocumentMenuId = SEModel.DocumentMenuId,EnquiryType = SEModel.Enquiry_type });
                        }
                        /*End to chk Financial year exist or not*/
                        URLDetailModel URLModelEdit = new URLDetailModel();
                        
                            SEModel.TransType = "Update";
                            SEModel.Command = command;
                            SEModel.BtnName = "BtnEdit";
                            SEModel.Message = "New";
                            SEModel.AppStatus = "D";
                            TempData["ModelData"] = SEModel;
                            URLModelEdit.Command = command;
                            URLModelEdit.TransType = "Update";
                            URLModelEdit.BtnName = "BtnEdit";
                            URLModelEdit.DocumentMenuId = SEModel.DocumentMenuId;
                            URLModelEdit.DocDate = SEModel.EnquiryDt;
                            URLModelEdit.DocNo = SEModel.EnquiryNo;
                            //Session["TransType"] = "Update";
                            //Session["Command"] = command;
                            //Session["BtnName"] = "BtnEdit";
                            //Session["Message"] = "New";
                            //Session["AppStatus"] = 'D';
                            //Session["EnquiryNo"] = SEModel.EnquiryNo;
                            TempData["ListFilterData"] = SEModel.ListFilterData1;
                       return RedirectToAction("SalesEnquiryDetail", URLModelEdit);

                    case "Delete":
                        URLDetailModel URLModelDelete = new URLDetailModel();
                        SalesEnquiryModel SEModelDelete = new SalesEnquiryModel();
                        SlsEnquiryDelete(SEModel, command);
                        SEModelDelete.Message = "Deleted";
                        SEModelDelete.Command = "Refresh";
                        SEModelDelete.EnquiryNo = "";
                        SEModelDelete.TransType = "Refresh";
                        SEModelDelete.AppStatus = "DL";
                        SEModelDelete.BtnName = "BtnDelete";
                        SEModelDelete.DocumentMenuId = SEModel.DocumentMenuId;
                        SEModelDelete.CustType = SEModel.CustType;
                        TempData["ModelData"] = SEModelDelete;
                        URLModelDelete.DocumentMenuId = SEModel.DocumentMenuId;
                        URLModelDelete.EnquiryType = SEModel.Enquiry_type;
                        URLModelDelete.TransType = "Refresh";
                        URLModelDelete.BtnName = "BtnDelete";
                        URLModelDelete.Command = "Refresh";
                        TempData["ListFilterData"] = SEModel.ListFilterData1;
                        return RedirectToAction("SalesEnquiryDetail", URLModelDelete);

                    case "Save":
                        //Session["Command"] = command;
                        SEModel.Command = command;

                        if (SEModel.TransType == null)
                        {
                            SEModel.TransType = command;
                        }
                        
                        SaveSEDetail(SEModel);
                        if (SEModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        //SEModel.EnquiryNo = Session["EnquiryNo"].ToString();
                        SEModel.ProspectFromEnquiry = null;
                        URLDetailModel URLModelSave = new URLDetailModel();
                        URLModelSave.DocumentMenuId = SEModel.DocumentMenuId;
                        URLModelSave.EnquiryType = SEModel.Enquiry_type;
                        URLModelSave.TransType = "Update";
                        URLModelSave.BtnName = "BtnSave";
                        URLModelSave.Command = command;
                        URLModelSave.DocDate = SEModel.EnquiryDt;
                        URLModelSave.DocNo = SEModel.EnquiryNo;
                        TempData["ModelData"] = SEModel;
                        TempData["ListFilterData"] = SEModel.ListFilterData1;
                        //Session["SQ_Date"] = Session["SQ_Date"].ToString();
                        return RedirectToAction("SalesEnquiryDetail", URLModelSave);

                    //case "Forward":
                    //    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/

                    //    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //    {
                    //        TempData["Message"] = "Financial Year not Exist";
                    //        return RedirectToAction("EditUpdateSEDetail", new { SENo = SEModel.EnquiryNo, ListFilterData = SEModel.ListFilterData1, DocumentMenuId = SEModel.DocumentMenuId, EnquiryType = SEModel.Enquiry_type });
                    //    }
                    //    /*End to chk Financial year exist or not*/
                    //    return new EmptyResult();

                    //case "Approve":
                    //    //Session["Command"] = command;
                    //    URLDetailModel URLModelApprove = new URLDetailModel();
                    //    SEModel.Command = command;
                    //    SQListApprove(SEModel);
                    //    TempData["ModelData"] = SEModel;
                    //    URLModelApprove.Command = SEModel.Command;
                    //    URLModelApprove.TransType = SEModel.TransType;
                    //    URLModelApprove.DocNo = SEModel.EnquiryNo;
                    //    URLModelApprove.DocDate = SEModel.EnquiryDt;
                    //    URLModelApprove.DocumentMenuId = SEModel.DocumentMenuId;
                    //    URLModelApprove.Enquiry_type = SEModel.Enquiry_type;
                    //    TempData["ListFilterData"] = SEModel.ListFilterData1;
                    //    return RedirectToAction("SalesEnquiryDetail", URLModelApprove);

                    case "Refresh":
                        URLDetailModel URLModelRefresh = new URLDetailModel();
                        SalesEnquiryModel SEModelRefresh = new SalesEnquiryModel();
                        SEModelRefresh.BtnName = "Refresh";
                        SEModelRefresh.Command = command;
                        SEModelRefresh.TransType = "Save";
                        SEModelRefresh.DocumentMenuId = SEModel.DocumentMenuId;
                        SEModelRefresh.CustType = SEModel.CustType;
                        TempData["ModelData"] = SEModelRefresh;
                        //URLModelRefresh.Command = SEModel.Command;
                        //URLModelRefresh.TransType = SEModel.TransType;
                        //URLModelRefresh.DocNo = SEModel.EnquiryNo;
                        //URLModelRefresh.DocDate = SEModel.EnquiryDt;
                        URLModelRefresh.DocumentMenuId = SEModel.DocumentMenuId;
                        URLModelRefresh.EnquiryType = SEModel.Enquiry_type;
                        URLModelRefresh.BtnName = "Refresh";
                        SEModelRefresh.Command = command;
                        SEModelRefresh.TransType = "Save";
                        TempData["ListFilterData"] = SEModel.ListFilterData1;
                        return RedirectToAction("SalesEnquiryDetail", URLModelRefresh);
                    
                    case "Print":
                        return GenratePdfFile(SEModel);
                    case "BacktoList":
                        
                            TempData["ListFilterData"] = SEModel.ListFilterData1;
                            return RedirectToAction("SalesEnquiry", "SalesEnquiry");
                        
                    //Session.Remove("Message");
                    //Session.Remove("TransType");
                    //Session.Remove("Command");
                    //Session.Remove("BtnName");
                    //Session.Remove("DocumentStatus");
                    //Session.Remove("ProspectFromQuot");
                    //    TempData["ListFilterData"] = SEModel.ListFilterData1;
                    //return RedirectToAction("SalesEnquiryList", "SalesEnquiryList");
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
        [NonAction]
        public ActionResult SaveSEDetail(SalesEnquiryModel SEModel)
        {
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = SEModel.Title.Replace(" ", "");

            try
            {

                GetCompDetail();
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblCommunicationDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable dtheader = new DataTable();

                DtblHDetail = ToDtblHDetail(SEModel);
                DtblItemDetail = ToDtblItemDetail(SEModel.SE_ItemDetail);
                DtblCommunicationDetail = ToDtblCommunicationDetail(SEModel.Communicationdetails);
                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("qty", typeof(string));
                if (SEModel.SubItemDetailsDt != null)
                {
                    JArray jObject2 = JArray.Parse(SEModel.SubItemDetailsDt);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                }

                /*------------------Sub Item end----------------------*/
                //var hdnQuotationCreat = "";
                //if (SEModel.QuotationCreated == true)
                //{
                //    hdnQuotationCreat = SEModel.hdnQuationCreated;
                //    SEModel.hdnQuationCreated = "Y";
                //}
                //else
                //{
                //    hdnQuotationCreat = "";
                //    SEModel.hdnQuationCreated = "N";
                //}

                DataTable dtAttachment = new DataTable();
                var _SalesQuotationModelattch = TempData["ModelDataattch"] as SalesEnquiryModelattch;
                TempData["ModelDataattch"] = null;
                if (SEModel.attatchmentdetail != null)
                {
                    if (_SalesQuotationModelattch != null)
                    {
                        if (_SalesQuotationModelattch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _SalesQuotationModelattch.AttachMentDetailItmStp as DataTable;
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
                        if (SEModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = SEModel.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(SEModel.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(SEModel.EnquiryNo))
                            {
                                dtrowAttachment1["id"] = SEModel.EnquiryNo;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y";
                            dtrowAttachment1["comp_id"] = CompID;
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (SEModel.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string SQTCode = string.Empty;
                            if (!string.IsNullOrEmpty(SEModel.EnquiryNo))
                            {
                                SQTCode = SEModel.EnquiryNo;
                            }
                            else
                            {
                                SQTCode = "0";
                            }
                            string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + SQTCode.Replace("/", "") + "*");

                            foreach (var fielpath in filePaths)
                            {
                                string flag = "Y";
                                foreach (DataRow dr in dtAttachment.Rows)
                                {
                                    string drImgPath = dr["file_path"].ToString();
                                    if (drImgPath == fielpath.Replace("/", @"\"))
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
                SaveMessage = _SalesEnquiry_ISERVICES.InsertSE_Details(DtblHDetail, DtblItemDetail, DtblCommunicationDetail,dtSubItem, DtblAttchDetail);
                string[] Data = SaveMessage.Split(',');
                string SENo = Data[1];
                string EnquiryNo = SENo.Replace("/", "");
                string Message = Data[0];
                string SEDate = Data[2];
                string Message1 = Data[4];
                string StatusCode = Data[3];
                if (Message == "Data_Not_Found")
                {   var a = StatusCode.Split('-');// statuscode is Table Name
                    var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    SEModel.Message = Message.Replace("_", "");
                    return RedirectToAction("SalesEnquiryDetail");
                }
                /*-----------------Attachment Section Start------------------------*/
                if (Message == "Save")
                {
                    string Guid = "";
                    if (_SalesQuotationModelattch != null)
                    {
                        //if (Session["Guid"] != null)
                        if (_SalesQuotationModelattch.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _SalesQuotationModelattch.Guid;
                        }
                    }
                    string guid = Guid;
                    var comCont = new CommonController(_Common_IServices);
                    comCont.ResetImageLocation(CompID, BrchID, guid, PageName, EnquiryNo, SEModel.TransType, DtblAttchDetail);


                }
                /*-----------------Attachment Section End------------------------*/
                
                if (Message == "Update" || Message == "Save")
                {
                    SEModel.Message = "Save";
                    SEModel.Command = "Update";
                    SEModel.EnquiryNo = SENo;
                    SEModel.EnquiryDt = SEDate;
                    SEModel.TransType = "Update";
                    SEModel.AppStatus = "D";
                    SEModel.BtnName = "BtnSave";
                    
                }
                return RedirectToAction("SalesEnquiryDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (SEModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (SEModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = SEModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        private DataTable ToDtblHDetail(SalesEnquiryModel SEModel)
        {
            try
            {
                GetCompDetail();
                DataTable DtblHDetail = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("Enquiry_type", typeof(string));
                dtheader.Columns.Add("CustPros_Type", typeof(string));
                dtheader.Columns.Add("Enquiry_No", typeof(string));
                dtheader.Columns.Add("Enquiry_Dt", typeof(string));
                dtheader.Columns.Add("Enquiry_Src", typeof(string));
                dtheader.Columns.Add("cust_id", typeof(int));
                dtheader.Columns.Add("address_id", typeof(int));
                dtheader.Columns.Add("contct_prsn", typeof(string));
                dtheader.Columns.Add("contct_email", typeof(string));
                dtheader.Columns.Add("contct_nmbr", typeof(string));
                dtheader.Columns.Add("contct_webst", typeof(string));
                dtheader.Columns.Add("curr_id", typeof(int));
                dtheader.Columns.Add("conv_rate", typeof(string));
                dtheader.Columns.Add("slspersn_id", typeof(int));
                dtheader.Columns.Add("Remarks", typeof(string));
                dtheader.Columns.Add("SE_Status", typeof(string));
                dtheader.Columns.Add("Quot_create", typeof(string));
                dtheader.Columns.Add("QuotNumDt", typeof(string));
                dtheader.Columns.Add("user_id", typeof(int));
                dtheader.Columns.Add("mac_id", typeof(string));
                
                

                DataRow dtrowHeader = dtheader.NewRow();
                //dtrowHeader["TransType"] = Session["TransType"].ToString();
                dtrowHeader["TransType"] = SEModel.TransType;
                dtrowHeader["comp_id"] = CompID;
                dtrowHeader["br_id"] = BrchID;
                dtrowHeader["Enquiry_type"] = SEModel.Enquiry_type;
                dtrowHeader["CustPros_Type"] = SEModel.CustPros_type;
                dtrowHeader["Enquiry_No"] = SEModel.EnquiryNo;
                dtrowHeader["Enquiry_Dt"] = SEModel.EnquiryDt;
                dtrowHeader["Enquiry_Src"] = SEModel.EnquirySource;
                dtrowHeader["cust_id"] = SEModel.Cust_id;
                if (SEModel.CustPros_type == "C")
                {
                    dtrowHeader["address_id"] = SEModel.Billing_id;
                    //dtrowHeader["ship_add_id"] = SEModel.Shipping_id;
                }
                dtrowHeader["contct_prsn"] = SEModel.cont_pers;
                dtrowHeader["contct_email"] = SEModel.cont_email;
                dtrowHeader["contct_nmbr"] = SEModel.cont_num;
                dtrowHeader["contct_webst"] = SEModel.cont_web;
                dtrowHeader["curr_id"] = SEModel.curr_id;
                dtrowHeader["conv_rate"] = SEModel.convrate;
                dtrowHeader["slspersn_id"] = SEModel.SE_SalePerson;
                dtrowHeader["Remarks"] = SEModel.Remarks;
                dtrowHeader["SE_Status"] = "D";
                var hdnQuotationCreat = "";
                if (SEModel.QuotationCreated == true)
                {
                    hdnQuotationCreat = SEModel.hdnQuationCreated; 
                    SEModel.hdnQuationCreated = "Y";
                    dtrowHeader["Quot_create"] = SEModel.hdnQuationCreated;
                }
                else
                {
                    hdnQuotationCreat = "";
                    SEModel.hdnQuationCreated = "N";
                    dtrowHeader["Quot_create"] = SEModel.hdnQuationCreated; 
                }
                dtrowHeader["QuotNumDt"] = SEModel.QuotationNumDt;
                dtrowHeader["user_id"] = Session["UserId"].ToString();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                
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
        private DataTable ToDtblItemDetail(string SEItemList)
        {
            try
            {
                GetCompDetail();
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("br_id", typeof(int));
                dtItem.Columns.Add("SENo", typeof(string));
                dtItem.Columns.Add("SEDate", typeof(string));
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(int));
                dtItem.Columns.Add("SEQty", typeof(string));
                dtItem.Columns.Add("ItmRate", typeof(string));
                dtItem.Columns.Add("NetValBase", typeof(string));
                dtItem.Columns.Add("NetValSpec", typeof(string));
                dtItem.Columns.Add("ItmRemarks", typeof(string));
                

                if (SEItemList != null)
                {
                    JArray jObject = JArray.Parse(SEItemList);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["comp_id"] = CompID;
                        dtrowLines["br_id"] = BrchID;
                        dtrowLines["SENo"] = jObject[i]["SENo"].ToString();
                        dtrowLines["SEDate"] = jObject[i]["SEDate"].ToString();
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        if (jObject[i]["UOMID"].ToString() == "" || jObject[i]["UOMID"].ToString() == null)
                        {
                            dtrowLines["UOMID"] = 0;
                        }
                        else
                        {
                            dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        }

                        dtrowLines["SEQty"] = jObject[i]["SEQty"].ToString();
                        dtrowLines["ItmRate"] = jObject[i]["ItmRate"].ToString();
                        dtrowLines["NetValBase"] = jObject[i]["NetValBase"].ToString();
                        dtrowLines["NetValSpec"] = jObject[i]["NetValSpec"].ToString();
                        dtrowLines["ItmRemarks"] = jObject[i]["Remarks"].ToString();
                        
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
        private DataTable ToDtblCommunicationDetail(string SECommunictnDtlList)
        {
            try
            {
                GetCompDetail();
                DataTable DtblCommunicationDetail = new DataTable();
                DataTable dtCommuni = new DataTable();

                dtCommuni.Columns.Add("comp_id", typeof(int));
                dtCommuni.Columns.Add("br_id", typeof(int));
                dtCommuni.Columns.Add("SENo", typeof(string));
                dtCommuni.Columns.Add("SEDate", typeof(string));
                dtCommuni.Columns.Add("CommType", typeof(string));
                dtCommuni.Columns.Add("CallDate", typeof(string));
                dtCommuni.Columns.Add("ContactBy", typeof(string));
                dtCommuni.Columns.Add("ContactTo", typeof(string));
                dtCommuni.Columns.Add("ContactDtl", typeof(string));
                dtCommuni.Columns.Add("DiscusRemarks", typeof(string));


                if (SECommunictnDtlList != null)
                {
                    JArray jObject = JArray.Parse(SECommunictnDtlList);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtCommuni.NewRow();
                        dtrowLines["comp_id"] = CompID;
                        dtrowLines["br_id"] = BrchID;
                        dtrowLines["SENo"] = jObject[i]["SENo"].ToString();
                        dtrowLines["SEDate"] = jObject[i]["SEDate"].ToString();
                        dtrowLines["CommType"] = jObject[i]["CommuTyp"].ToString();
                        dtrowLines["CallDate"] = jObject[i]["CallDate"].ToString();
                        dtrowLines["ContactBy"] = jObject[i]["ContactBy"].ToString();
                        dtrowLines["ContactTo"] = jObject[i]["ContactTo"].ToString();
                        dtrowLines["ContactDtl"] = jObject[i]["ContactDtl"].ToString();
                        dtrowLines["DiscusRemarks"] = jObject[i]["DiscusRemark"].ToString();

                        dtCommuni.Rows.Add(dtrowLines);
                    }
                }

                DtblCommunicationDetail = dtCommuni;
                return DtblCommunicationDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        public ActionResult EditUpdateSEDetail(string SENo, string ListFilterData, string DocumentMenuId,string EnqTyp, string CustTyp)
        {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
            GetCompDetail();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message1"] = "Financial Year not Exist";
            }
            /*End to chk Financial year exist or not*/
            URLDetailModel URLModel = new URLDetailModel();
            SalesEnquiryModel  _Model = new SalesEnquiryModel();
            _Model.Message = "New";
            _Model.Command = "Add";
            _Model.EnquiryNo = SENo;
            _Model.TransType = "Update";
            _Model.AppStatus = "D";
            _Model.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _Model;
            URLModel.DocNo = SENo;
            URLModel.DocDate = "";
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.Command = "Add";
            URLModel.DocumentMenuId = DocumentMenuId;
            URLModel.EnquiryType = EnqTyp;
            URLModel.CustType = CustTyp;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("SalesEnquiryDetail", URLModel);
        }
        private ActionResult SlsEnquiryDelete(SalesEnquiryModel SEModel, string command)
        {
            try
            {

                GetCompDetail();

                string doc_no = SEModel.EnquiryNo;
                string Message = _SalesEnquiry_ISERVICES.SEdetailDelete(SEModel, CompID, BrchID);

                if (!string.IsNullOrEmpty(doc_no))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = SEModel.Title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, doc_no1, Server);
                }
                SEModel.Message = "Deleted";
                SEModel.Command = "Refresh";
                SEModel.EnquiryNo = "";
                SEModel.TransType = "Refresh";
                SEModel.AppStatus = "DL";
                SEModel.BtnName = "BtnDelete";
                TempData["ModelData"] = SEModel;
                
                return RedirectToAction("SalesEnquiryDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private void GetAllData(SalesEnquiryModel SEModel, string CustPros_type, string Enquiry_type)
        {
            //SalesEnquiryModel SEModel = new SalesEnquiryModel();
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            //string Comp_ID = string.Empty;
            //string Br_ID = string.Empty;
            try
            {
                GetCompDetail();
                if (string.IsNullOrEmpty(SEModel.SE_CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = SEModel.SE_CustName;
                }
                SEModel.CustPros_type = CustPros_type;
                SEModel.Enquiry_type = Enquiry_type;
                string SalesPersonName = string.Empty;
                if (string.IsNullOrEmpty(SEModel.SE_SalePerson))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = SEModel.SE_SalePerson;
                }
                
                DataSet AllData = _SalesEnquiry_ISERVICES.GetAllData(CompID, CustomerName, BrchID, CustPros_type, Enquiry_type, SalesPersonName);
                List<CustomerName> _CustList = new List<CustomerName>();
                foreach (DataRow data in AllData.Tables[0].Rows)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.Cust_id = data["cust_id"].ToString();
                    _CustDetail.Cust_name = data["cust_name"].ToString();
                    _CustList.Add(_CustDetail);
                }
                _CustList.Insert(0, new CustomerName() { Cust_id = "0", Cust_name = "---Select---" });
                SEModel.CustomerNameList = _CustList;

                List<SalePerson> _SlPrsnList = new List<SalePerson>();
                foreach (DataRow data in AllData.Tables[1].Rows)
                {
                    SalePerson _SlPrsnDetail = new SalePerson();
                    _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                    _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                    _SlPrsnList.Add(_SlPrsnDetail);
                }
                _SlPrsnList.Insert(0, new SalePerson() { salep_id = "0", salep_name = "---Select---" });
                SEModel.SalePersonList = _SlPrsnList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void GetAllDDLAndListData(SEListModel _SEListModel)
        {
            //SalesEnquiryModel SEModel = new SalesEnquiryModel();
            string CustType = string.Empty;
            string EnqryTyp = string.Empty;
            string CustomerName = string.Empty;
            string SalesPersonName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            //string Comp_ID = string.Empty;
            //string Br_ID = string.Empty;
            try
            {
                GetCompDetail();
                if (string.IsNullOrEmpty(_SEListModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _SEListModel.CustName;
                }
                if(_SEListModel.CustTyp ==null || _SEListModel.CustTyp == "0")
                {
                    _SEListModel.CustTyp = "0";
                    CustType = "C";
                    CustType = "0";
                }
                else
                {
                    //_SEListModel.CustTyp = _SEListModel.CustTyp;
                    CustType = _SEListModel.CustTyp; 
                }
                if (_SEListModel.EnqryTyp == null || _SEListModel.EnqryTyp == "0")
                {
                    _SEListModel.EnqryTyp = "0";
                    EnqryTyp = "D";
                    EnqryTyp = "0";
                }
                else
                {
                    //_SEListModel.EnqryTyp = _SEListModel.EnqryTyp;
                    EnqryTyp = _SEListModel.EnqryTyp;
                }
                
                if (string.IsNullOrEmpty(_SEListModel.SlsPrsn))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = _SEListModel.SlsPrsn;
                }

                DataSet AllData = _SalesEnquiry_ISERVICES.GetAllData(CompID, CustomerName, BrchID, CustType, EnqryTyp, SalesPersonName);
                List<CustNameOnList> _CustList = new List<CustNameOnList>();
                foreach (DataRow data in AllData.Tables[0].Rows)
                {
                    CustNameOnList _CustDetail = new CustNameOnList();
                    _CustDetail.Cust_id = data["cust_id"].ToString();
                    _CustDetail.Cust_name = data["cust_name"].ToString();
                    _CustList.Add(_CustDetail);
                }
                _CustList.Insert(0, new CustNameOnList() { Cust_id = "0", Cust_name = "---Select---" });
                _SEListModel.CustNameList = _CustList;
                /*******************************GEt Sales Person DropDownList*******************************************/
                List<SalePersonOnList> _SlPrsnList = new List<SalePersonOnList>();
                foreach (DataRow data in AllData.Tables[1].Rows)
                {
                    SalePersonOnList _SlPrsnDetail = new SalePersonOnList();
                    _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                    _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                    _SlPrsnList.Add(_SlPrsnDetail);
                }
                _SlPrsnList.Insert(0, new SalePersonOnList() { salep_id = "0", salep_name = "---Select---" });
                _SEListModel.SlsPrsnOnList = _SlPrsnList;
                
                /*******************************GEt Category DropDownList*******************************************/
                List<Category> _Category = new List<Category>();

                foreach (DataRow dr in AllData.Tables[2].Rows)
                {
                    Category ddlcategory = new Category();
                    ddlcategory.setup_id = dr["setup_id"].ToString();
                    ddlcategory.setup_val = dr["setup_val"].ToString();
                    _Category.Add(ddlcategory);
                }
                _Category.Insert(0, new Category() { setup_id = "0", setup_val = "All" });
                _SEListModel.CategoryList = _Category;
                /*****************************************End******************************************************/
                /******************PortFolio DropDown Bind****************/
                List<PortFolio> _PortFolioList = new List<PortFolio>();
                foreach (DataRow dr in AllData.Tables[3].Rows)
                {
                    PortFolio _PortFolio = new PortFolio();
                    _PortFolio.setup_id = dr["setup_id"].ToString();
                    _PortFolio.setup_val = dr["setup_val"].ToString();
                    _PortFolioList.Add(_PortFolio);
                }
                _PortFolioList.Insert(0, new PortFolio() { setup_id = "0", setup_val = "All" });
                _SEListModel.PortFolioList = _PortFolioList;
                /**************************End**********************/
                /******************PortFolio DropDown Bind****************/
                List<Region> _RgnList = new List<Region>();
                foreach (DataRow dr in AllData.Tables[4].Rows)
                {
                    Region _Rgn = new Region();
                    _Rgn.setup_id = dr["setup_id"].ToString();
                    _Rgn.setup_val = dr["setup_val"].ToString();
                    _RgnList.Add(_Rgn);
                }
                _RgnList.Insert(0, new Region() { setup_id = "0", setup_val = "All" });
                _SEListModel.RegionList = _RgnList;
                /**************************End**********************/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult SearchSlsEnqryDetail(string EnqType, string CustType, string EnqSrc, string CustId, string Catgry, string PrtFlio, string Regn, string SlsPrsn, string Fromdate, string Todate, string Status)
        {
             
            try
            {
                SEListModel _SEListModel = new SEListModel();
                
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
               
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                _SlsEnqryList = new List<SlsEnqryList>();
                _SEListModel.EnqryTyp = EnqType;
                _SEListModel.CustTyp = CustType;
                _SEListModel.EnqSrc = EnqSrc;
                _SEListModel.CustName = CustId;
                _SEListModel.Region = Regn;
                _SEListModel.Portfolio = PrtFlio;
                _SEListModel.Catgry = Catgry;
                _SEListModel.SlsPrsn = SlsPrsn;
                _SEListModel.FromDate = Fromdate;
                _SEListModel.ToDate = Todate;
                _SEListModel.Status = Status;
                DataSet dt = new DataSet();
                dt = _SalesEnquiry_ISERVICES.GetSlsEnqryListandSrchDetail(CompID, BrchID, _SEListModel,"","");
                _SEListModel.SESearch = "SE_Search";

                //Session["FinStDt"] = dt.Tables[2].Rows[0]["findate"];
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    SlsEnqryList _EnqList = new SlsEnqryList();
                    _EnqList.SENumber = dr["EnqNo"].ToString();
                    _EnqList.SEDate = dr["Enq_Date"].ToString();
                    _EnqList.SE_Dt = dr["Enq_Dt"].ToString();
                    _EnqList.Enqry_Src = dr["Enq_Src"].ToString();
                    _EnqList.Enqry_Typ = dr["EnqType"].ToString();
                    _EnqList.Cust_Typ = dr["CustType"].ToString();
                    _EnqList.CustName = dr["CustName"].ToString();
                    _EnqList.Region = dr["Region"].ToString();
                    _EnqList.Portfolio = dr["Portfolio"].ToString();
                    _EnqList.Category = dr["Category"].ToString();
                    _EnqList.Sls_Persn = dr["SlsPrsn"].ToString();
                    _EnqList.QuotaionNumDate = dr["QuotaionNoDate"].ToString();
                    _EnqList.SE_StatusList = dr["SEStatus"].ToString();
                    _EnqList.CreatedBy = dr["CreatedBy"].ToString();
                    _EnqList.CreatedON = dr["CreateDate"].ToString();
                    //_EnqList.ModifiedBy = dr["ModifyBy"].ToString();
                    //_EnqList.ModifiedOn = dr["ModifyDate"].ToString();
                    _SlsEnqryList.Add(_EnqList);
                }
                _SEListModel.SalesEnquiryList = _SlsEnqryList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSalesEnquiryList.cshtml", _SEListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private List<SlsEnqryList> getSlsEnqryList(SEListModel _SEListModel)
        {
            _SlsEnqryList = new List<SlsEnqryList>();
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                var docid = DocumentMenuId;
                

                DataSet DSet = _SalesEnquiry_ISERVICES.GetSlsEnqryListandSrchDetail(CompID, BrchID, _SEListModel, UserID,DocumentMenuId);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        SlsEnqryList _EnqList = new SlsEnqryList();
                        _EnqList.SENumber = dr["EnqNo"].ToString();
                        _EnqList.SEDate = dr["Enq_Date"].ToString();
                        _EnqList.SE_Dt = dr["Enq_Dt"].ToString();
                        _EnqList.Enqry_Src = dr["Enq_Src"].ToString();
                        _EnqList.Enqry_Typ = dr["EnqType"].ToString();
                        _EnqList.Cust_Typ = dr["CustType"].ToString();
                        _EnqList.CustName = dr["CustName"].ToString();
                        _EnqList.Region = dr["Region"].ToString();
                        _EnqList.Portfolio = dr["Portfolio"].ToString();
                        _EnqList.Category = dr["Category"].ToString();
                        _EnqList.Sls_Persn = dr["SlsPrsn"].ToString();
                        _EnqList.QuotaionNumDate = dr["QuotaionNoDate"].ToString();
                        _EnqList.SE_StatusList = dr["SEStatus"].ToString();
                        _EnqList.CreatedBy = dr["CreatedBy"].ToString();
                        _EnqList.CreatedON = dr["CreateDate"].ToString();
                        //_EnqList.ModifiedBy = dr["ModifyBy"].ToString();
                        //_EnqList.ModifiedOn = dr["ModifyDate"].ToString();
                       
                        //_EnqList.ApprovedOn = dr["ApproveDate"].ToString();
                        _EnqList.FinStDt = DSet.Tables[2].Rows[0]["findate"].ToString();

                        _SlsEnqryList.Add(_EnqList);
                    }
                }

                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;

            }
            return _SlsEnqryList;
        }
        public ActionResult GetAutoCompleteSearchCustList(SalesEnquiryModel SEModel, string CustPros_type, string Enquiry_type)
        {
            //SalesEnquiryModel SEModel = new SalesEnquiryModel();
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();

            try
            {
                GetCompDetail();
                if (string.IsNullOrEmpty(SEModel.SE_CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = SEModel.SE_CustName;
                }
                SEModel.CustPros_type = CustPros_type;
                SEModel.Enquiry_type = Enquiry_type;
                CustList = _SalesEnquiry_ISERVICES.GetCustomerList(CompID, CustomerName, BrchID, CustPros_type, Enquiry_type);

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetSalesEnquiryDashbordList(string docid, string status)
        {
            var WF_Status = status;
            return RedirectToAction("SalesEnquiry"/*, new { WF_Status }*/);
        }
        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
                    , string Flag, string Status, string Doc_no, string Doc_dt, string DocumentMenuId)
        {
            try
            {
                GetCompDetail();
                DataTable dt = new DataTable();
                int QtyDigit = 0;
                QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
                
                ViewBag.DocumentMenuId = DocumentMenuId;
                if (Flag == "Quantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _SalesEnquiry_ISERVICES.SE_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt/*, Flag*/).Tables[0];
                    }
                }
                

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    //_subitemPageName = "MTO",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y"

                };

                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag == "Quantity" ? Flag : "MTO";
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private string IsNull(string Str, string Str2)
        {
            if (!string.IsNullOrEmpty(Str))
            {
            }
            else
                Str = Str2;
            return Str;
        }
        /*--------------------------For Print Work Start--------------------------*/
        public FileResult GenratePdfFile(SalesEnquiryModel SEModel)
        {
            return File(GetPdfData(SEModel.EnquiryNo, SEModel.EnquiryDt), "application/pdf", "SalesEnquiry.pdf");
        }
        public byte[] GetPdfData(string EnquiryNo, string EnquiryDt)
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
                DataSet Deatils = _SalesEnquiry_ISERVICES.GetPrintDeatils(CompID, BrchID, EnquiryNo, EnquiryDt);
                ViewBag.PageName = "ENQ";
                ViewBag.Title = "Sales Enquiry";
                ViewBag.Details = Deatils;
                
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                //ViewBag.InvoiceTo = "Invoice to:";
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["enq_status"].ToString().Trim();
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesEnquiry/SalesEnquiryPrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 20f);
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
        /*--------------------------For Print Work End--------------------------*/
        private void CommonPageDetails()
        {
            try
            {
                GetCompDetail();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                ViewBag.VBRoleList = ds.Tables[3];
                ViewBag.StatusList = ds.Tables[4];
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
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
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType)
        {
            SalesEnquiryModel SEModel = new SalesEnquiryModel();
            var a = TrancType.Split(',');
            SEModel.EnquiryNo = a[0].Trim();
            SEModel.EnquiryDt = a[1].Trim();
            SEModel.DocumentMenuId = a[2].Trim();
            
            SEModel.CustType = a[4].Trim();
            SEModel.TransType = "Update";
           
            SEModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = SEModel;
           
            TempData["ListFilterData"] = ListFilterData1;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.DocNo = a[0].Trim();
            URLModel.DocDate = a[1].Trim();
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.DocumentMenuId = a[2].Trim();
            URLModel.CustType = a[4].Trim();
            return RedirectToAction("SalesEnquiryDetail", URLModel);
        }
        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                SalesEnquiryModelattch _SalesEnquiryModelattch = new SalesEnquiryModelattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string SQ_No = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["SQ_No"] != null)
                //{
                //    SQ_No = Session["SQ_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _SalesEnquiryModelattch.Guid = DocNo;
                GetCompDetail();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _SalesEnquiryModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _SalesEnquiryModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _SalesEnquiryModelattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        /*--------------------------For Attatchment End--------------------------*/
    }

}